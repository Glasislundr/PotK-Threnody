// Decompiled with JetBrains decompiler
// Type: Quest002SideStoryMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/quest002_17/SideStoryMenu")]
public class Quest002SideStoryMenu : BackButtonMenuBase
{
  private readonly int[] categoriesID = new int[3]
  {
    6,
    7,
    8
  };
  private const int CATEGORY_METALKEY = 1;
  private const int CATEGORY_LIMITED = 2;
  private const int CATEGORY_BEGINNER = 4;
  private static readonly TimeSpan biginnerSpn = new TimeSpan(168, 0, 0);
  private static int beforeCategoryId = -1;
  [SerializeField]
  protected UILabel TxtTitle;
  public UIScrollView[] scrollviews;
  public GameObject topDragScroll_;
  private DateTime serverTime;
  private Dictionary<int, Quest00217TabPage> categories = new Dictionary<int, Quest00217TabPage>();
  private List<UIDragScrollView> pauseDragScrollViews_;
  private Dictionary<int, Description> dicHuntingDescriptions_ = new Dictionary<int, Description>();
  private Dictionary<int, (QuestScoreCampaignProgress[] data, int patterns)> dicScoreCampaigns_ = new Dictionary<int, (QuestScoreCampaignProgress[], int)>();
  [SerializeField]
  private Quest00217TabPage[] tabPages = new Quest00217TabPage[4];
  private PlayerExtraQuestS[] extraData;
  private DateTime playerCreatedAt;
  private List<Coroutine> loadingCoroutineList = new List<Coroutine>();
  private bool isPageLoading;
  private const int maxDefaultSetupBanner = 4;
  private int setupBannerCount;

  private void OnDestroy()
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
    {
      foreach (Coroutine loadingCoroutine in this.loadingCoroutineList)
      {
        if (loadingCoroutine != null)
          instance.StopCoroutine(loadingCoroutine);
      }
    }
    this.loadingCoroutineList.Clear();
  }

  private T elementAt<T>(T[] a, int index) where T : class
  {
    return a.Length <= index ? default (T) : a[index];
  }

  public IEnumerator Init(
    PlayerExtraQuestS[] ExtraData,
    int[] Categories,
    int[] Emphasis,
    QuestExtraTimetableNotice[] Notices,
    DateTime player_created_at,
    int activeTabIndex)
  {
    Categories = this.categoriesID;
    this.extraData = ((IEnumerable<PlayerExtraQuestS>) ExtraData).CheckMasterData().ToArray<PlayerExtraQuestS>();
    this.playerCreatedAt = player_created_at;
    this.categories.Clear();
    this.setupBannerCount = 0;
    PlayerExtraQuestS[] extraQuests = ((IEnumerable<PlayerExtraQuestS>) this.extraData).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.extra_quest_area == 1 || x.extra_quest_area == 3)).Root(true);
    this.createScrollItemsCategory(this.extraData, Categories);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.serverTime = ServerTime.NowAppTimeAddDelta();
    SM.TowerPeriod[] array1 = ((IEnumerable<SM.TowerPeriod>) SMManager.Get<SM.TowerPeriod[]>()).ToArray<SM.TowerPeriod>();
    CorpsPeriod[] activePeriods = Singleton<NGGameDataManager>.GetInstance().corpsUtil.activePeriods;
    EventInfo[] array2 = ((IEnumerable<EventInfo>) SMManager.Get<EventInfo[]>()).ToArray<EventInfo>();
    int length1 = extraQuests.Length;
    int length2 = array2.Length;
    int length3 = array1.Length;
    int length4 = activePeriods.Length;
    List<Quest002SideStoryMenu.BannerClass> source = new List<Quest002SideStoryMenu.BannerClass>(Enum.GetValues(typeof (Quest002SideStoryMenu.BannerType)).Length);
    HashSet<int> emphasisHashlist = new HashSet<int>((IEnumerable<int>) Emphasis);
    GameObject[] prefabs = new GameObject[Enum.GetValues(typeof (Quest002SideStoryMenu.BannerType)).Length];
    source.Clear();
    for (int index = 0; index < extraQuests.Length; ++index)
    {
      PlayerExtraQuestS playerExtraQuestS = this.elementAt<PlayerExtraQuestS>(extraQuests, index);
      if (playerExtraQuestS != null && this.isQuestConditionEffective(playerExtraQuestS.quest_extra_s.quest_m) && playerExtraQuestS.IsSideQuest())
        source.Add(new Quest002SideStoryMenu.BannerClass(playerExtraQuestS.quest_extra_s.quest_m.priority, Quest002SideStoryMenu.BannerType.Extra, index));
    }
    for (int index = 0; index < array2.Length; ++index)
    {
      EventInfo eventInfo = this.elementAt<EventInfo>(array2, index);
      if (eventInfo != null && eventInfo.IsSideQuest())
        source.Add(new Quest002SideStoryMenu.BannerClass(eventInfo.Period() != null ? eventInfo.Period().priority : int.MaxValue, Quest002SideStoryMenu.BannerType.Event, index));
    }
    foreach (Quest002SideStoryMenu.BannerClass bannerClass in source.OrderBy<Quest002SideStoryMenu.BannerClass, int>((Func<Quest002SideStoryMenu.BannerClass, int>) (x => x.priority)).ThenBy<Quest002SideStoryMenu.BannerClass, Quest002SideStoryMenu.BannerType>((Func<Quest002SideStoryMenu.BannerClass, Quest002SideStoryMenu.BannerType>) (x => x.type)).ToList<Quest002SideStoryMenu.BannerClass>())
    {
      Quest002SideStoryMenu.BannerClass banner = bannerClass;
      if (banner != null)
      {
        switch (banner.type)
        {
          case Quest002SideStoryMenu.BannerType.Extra:
            if (!Object.op_Implicit((Object) prefabs[(int) banner.type]))
            {
              Future<GameObject> ScrollPrefab = Res.Prefabs.quest002_17.scroll.Load<GameObject>();
              e = ScrollPrefab.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              prefabs[(int) banner.type] = ScrollPrefab.Result;
              ScrollPrefab = (Future<GameObject>) null;
            }
            e = this.InitLoopScrolls(extraQuests[banner.index], this.extraData, prefabs[(int) banner.type], emphasisHashlist, Notices);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
        }
        banner = (Quest002SideStoryMenu.BannerClass) null;
      }
    }
    int categoryId = activeTabIndex;
    if (activeTabIndex <= 0)
      categoryId = !(this.serverTime - this.playerCreatedAt < Quest002SideStoryMenu.biginnerSpn) ? (Quest002SideStoryMenu.beforeCategoryId <= 0 ? 2 : Quest002SideStoryMenu.beforeCategoryId) : 4;
    yield return (object) this.SetupCategory(categoryId);
    yield return (object) new WaitForEndOfFrame();
  }

  private bool isQuestConditionEffective(QuestExtraM m)
  {
    QuestExtraReleaseConditionsPlayer conditionsPlayer = ((IEnumerable<QuestExtraReleaseConditionsPlayer>) MasterData.QuestExtraReleaseConditionsPlayerList).FirstOrDefault<QuestExtraReleaseConditionsPlayer>((Func<QuestExtraReleaseConditionsPlayer, bool>) (x => x.quest_m == m));
    if (conditionsPlayer == null)
      return true;
    int level = SMManager.Observe<Player>().Value.level;
    switch (conditionsPlayer.comparison_operator)
    {
      case "<=":
        int num = level;
        int? playerLevel = conditionsPlayer.player_level;
        int valueOrDefault = playerLevel.GetValueOrDefault();
        if (num <= valueOrDefault & playerLevel.HasValue)
          return true;
        break;
      case ">=":
        return true;
    }
    return false;
  }

  private void createScrollItemsCategory(PlayerExtraQuestS[] extraData, int[] sortedCategory)
  {
    ((IEnumerable<int>) sortedCategory).Select<int, QuestExtraCategory>((Func<int, QuestExtraCategory>) (i => this.getQuestCategory(i))).ToList<QuestExtraCategory>();
    int[] array = ((IEnumerable<int>) this.categoriesID).Reverse<int>().ToArray<int>();
    for (int index = 0; index < this.tabPages.Length; ++index)
    {
      this.tabPages[index].Init(this, array[index]);
      this.categories.Add(array[index], this.tabPages[index]);
    }
  }

  private QuestExtraCategory getQuestCategory(int id)
  {
    QuestExtraCategory questCategory = (QuestExtraCategory) null;
    if (MasterData.QuestExtraCategory.TryGetValue(id, out questCategory))
      return questCategory;
    return new QuestExtraCategory()
    {
      ID = id,
      name = string.Format("Category:{0}", (object) id)
    };
  }

  private IEnumerator InitLoopScrolls(
    PlayerExtraQuestS extraData,
    PlayerExtraQuestS[] ExtraData,
    GameObject prefab,
    HashSet<int> Emphasis,
    QuestExtraTimetableNotice[] Notices)
  {
    List<QuestExtraTimetableNotice> list = ((IEnumerable<QuestExtraTimetableNotice>) Notices).ToList<QuestExtraTimetableNotice>();
    QuestExtraS questExtraS = extraData.quest_extra_s;
    Quest00217Scroll.Parameter parameter = new Quest00217Scroll.Parameter(extraData);
    parameter.isNew = true;
    QuestExtraLL questLL;
    if ((questLL = extraData.quest_ll) != null)
    {
      parameter.seek = QuestExtra.SeekType.LL;
      if (questLL.description.HasValue)
        parameter.descriptions = ((IEnumerable<QuestExtraDescription>) MasterData.QuestExtraDescriptionList).Where<QuestExtraDescription>((Func<QuestExtraDescription, bool>) (qd => qd.descriptionID == questLL.description.Value)).ToArray<QuestExtraDescription>();
      QuestExtra.getStatusLL(questLL, ExtraData, Emphasis, out parameter.isNew, out parameter.isClear, out parameter.isHighlighting, out parameter.isClearedToday, out parameter.entryConditionID, out parameter.lastEndTime);
    }
    else if (extraData.seek_type == PlayerExtraQuestS.SeekType.L)
    {
      QuestExtraL questL = questExtraS.quest_l;
      if (questL.description.HasValue)
        parameter.descriptions = ((IEnumerable<QuestExtraDescription>) MasterData.QuestExtraDescriptionList).Where<QuestExtraDescription>((Func<QuestExtraDescription, bool>) (qd => qd.descriptionID == questL.description.Value)).ToArray<QuestExtraDescription>();
      QuestExtra.getStatusL(questL.ID, ExtraData, Emphasis, out parameter.isNew, out parameter.isClear, out parameter.isHighlighting, out parameter.isClearedToday, out parameter.entryConditionID);
    }
    else
    {
      QuestExtraM questM = questExtraS.quest_m;
      if (questM.description.HasValue)
        parameter.descriptions = ((IEnumerable<QuestExtraDescription>) MasterData.QuestExtraDescriptionList).Where<QuestExtraDescription>((Func<QuestExtraDescription, bool>) (qd => qd.descriptionID == questM.description.Value)).ToArray<QuestExtraDescription>();
      QuestExtra.getStatusM(questM.ID, ExtraData, Emphasis, out parameter.isNew, out parameter.isClear, out parameter.isHighlighting, out parameter.isClearedToday, out parameter.isSkipSortie, out parameter.entryConditionID);
    }
    Predicate<QuestExtraTimetableNotice> match = (Predicate<QuestExtraTimetableNotice>) (n => n._quest_extra_s == extraData._quest_extra_s);
    QuestExtraTimetableNotice extraTimetableNotice = list.Find(match);
    if (extraTimetableNotice != null && extraTimetableNotice.start_at.HasValue)
    {
      parameter.isNotice = true;
      parameter.startTime = extraTimetableNotice.start_at;
    }
    this.categories[parameter.extra.top_category.ID].AddRequest(parameter, prefab);
    yield break;
  }

  public IEnumerator UpdateTime()
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.serverTime = ServerTime.NowAppTimeAddDelta();
    foreach (Quest00217TabPage tabPage in this.tabPages)
      tabPage.UpdateTime(this.serverTime);
  }

  private void checkChangeDescriptionScoreCampaing(Quest00217Scroll cntl, int id)
  {
    (int LL, int index, QuestScoreCampaignProgress[] data, int patterns) scoreCampaingInfo = this.getScoreCampaingInfo(id);
    if (this.checkHasDescriptionScoreCampaing(scoreCampaingInfo))
    {
      if (this.IsPushAndSet())
        return;
      this.pauseDragScrollView();
      Quest00228Scene.ChangeScene(scoreCampaingInfo.data[scoreCampaingInfo.index], true);
    }
    else
      cntl.setEffectNoDescription();
  }

  private (int LL, int index, QuestScoreCampaignProgress[] data, int patterns) getScoreCampaingInfo(
    int L)
  {
    if (!this.dicScoreCampaigns_.Any<KeyValuePair<int, (QuestScoreCampaignProgress[], int)>>())
      this.dicScoreCampaigns_ = SMManager.Get<QuestScoreCampaignProgress[]>().CreateLLMap();
    return this.dicScoreCampaigns_.FindL(L);
  }

  private bool checkHasDescriptionScoreCampaing(int Lid)
  {
    return this.checkHasDescriptionScoreCampaing(this.getScoreCampaingInfo(Lid));
  }

  private bool checkHasDescriptionScoreCampaing(
    (int LL, int index, QuestScoreCampaignProgress[] data, int patterns) info)
  {
    return info.data != null && info.LL == 0;
  }

  public virtual void Foreground()
  {
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnEvent()
  {
  }

  public virtual void VScrollBar()
  {
  }

  private IEnumerator Create_Transitionbutton(string name, int categoryId)
  {
    string path = string.Format("Prefabs/quest002_17/{0}", (object) name);
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
    {
      Debug.LogWarning((object) "don't exit path");
    }
    else
    {
      Future<GameObject> prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.categories[categoryId].AddRequest(prefabF.Result);
    }
  }

  private IEnumerator CreatePunitiveExpeditionEventButton(EventInfo info, GameObject prefab)
  {
    this.categories[info.category_id].AddRequest(info, prefab);
    yield break;
  }

  private IEnumerator coChangeHuntinDescription(Quest00217ScrollHunting cntl, EventInfo info)
  {
    Quest002SideStoryMenu quest002SideStoryMenu = this;
    quest002SideStoryMenu.pauseDragScrollView();
    Description description;
    if (!quest002SideStoryMenu.dicHuntingDescriptions_.TryGetValue(info.period_id, out description))
    {
      Future<WebAPI.Response.EventTop> request = WebAPI.EventTop(info.period_id, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = request.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      WebAPI.Response.EventTop result = request.Result;
      if (result == null)
      {
        yield break;
      }
      else
      {
        description = result.description;
        quest002SideStoryMenu.dicHuntingDescriptions_.Add(info.period_id, description);
        request = (Future<WebAPI.Response.EventTop>) null;
      }
    }
    if (description == null)
    {
      cntl.setEffectNoDescription();
      quest002SideStoryMenu.IsPush = false;
    }
    else
      Quest00228Scene.ChangeScene(description, true);
  }

  private void OnDisable() => this.resumeDragScrollView();

  private void pauseDragScrollView()
  {
    this.resumeDragScrollView();
    this.pauseDragScrollViews_ = new List<UIDragScrollView>();
    foreach (UIDragScrollView componentsInChild in this.topDragScroll_.GetComponentsInChildren<UIDragScrollView>())
    {
      if (((Behaviour) componentsInChild).enabled)
      {
        this.pauseDragScrollViews_.Add(componentsInChild);
        ((Behaviour) componentsInChild).enabled = false;
      }
    }
    foreach (UIScrollView scrollview in this.scrollviews)
      scrollview.Press(false);
  }

  private void resumeDragScrollView()
  {
    if (this.pauseDragScrollViews_ == null || !this.pauseDragScrollViews_.Any<UIDragScrollView>())
      return;
    foreach (Behaviour pauseDragScrollView in this.pauseDragScrollViews_)
      pauseDragScrollView.enabled = true;
    this.pauseDragScrollViews_.Clear();
  }

  public void OnPushPageTab(int index) => this.setPageTabActive(index);

  private void setPageTabActive(int index)
  {
    if (this.isPageLoading)
      return;
    this.isPageLoading = true;
    this.loadingCoroutineList.Add(this.StartCoroutine(this.SetupCategory(index)));
  }

  private IEnumerator SetupCategory(int categoryId)
  {
    Quest00217TabPage page = this.categories[categoryId];
    foreach (Quest00217TabPage quest00217TabPage in this.categories.Values)
      quest00217TabPage.SetPageActive(false, Object.op_Equality((Object) page, (Object) quest00217TabPage));
    page.SetButtonActive(true);
    yield return (object) null;
    this.setupBannerCount = 0;
    foreach (Quest00217TabPage.RequestParam request in page.RequestList.Concat<Quest00217TabPage.RequestParam>((IEnumerable<Quest00217TabPage.RequestParam>) page.RequestLateList))
    {
      IEnumerator e = this.SetupBanner(page, request);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ++this.setupBannerCount;
    }
    if (this.setupBannerCount != 0)
    {
      page.ClearRequest();
      page.RepositionGrid();
    }
    page.SetPageActive(true);
    Quest002SideStoryMenu.beforeCategoryId = categoryId;
    this.isPageLoading = false;
  }

  private IEnumerator SetupBanner(Quest00217TabPage page, Quest00217TabPage.RequestParam request)
  {
    object obj = request.Param;
    IEnumerator e;
    if (obj != null)
    {
      if (!(obj is Quest00217Scroll.Parameter parameter))
      {
        if (obj is EventInfo info)
        {
          e = this.coInitScrcollHunting(page, info, request.Prefab);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          yield break;
        }
      }
      else
      {
        e = this.ScrollInit(page, parameter, request.Prefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield break;
      }
    }
    if (Object.op_Implicit((Object) request.Prefab))
      yield return (object) this.coInitScrollDirect(page, request.Prefab);
  }

  public virtual IEnumerator ScrollInit(
    Quest00217TabPage page,
    Quest00217Scroll.Parameter param,
    GameObject prefab)
  {
    Quest002SideStoryMenu quest002SideStoryMenu = this;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    page.AddItem(gameObject);
    Quest00217Scroll qsi = gameObject.GetComponent<Quest00217Scroll>();
    if (quest002SideStoryMenu.setupBannerCount >= 4)
    {
      qsi.Setup(param, quest002SideStoryMenu.serverTime);
      Coroutine coroutine = quest002SideStoryMenu.StartCoroutine(qsi.SetAndCreate_BannerSprite());
      quest002SideStoryMenu.loadingCoroutineList.Add(coroutine);
    }
    else
    {
      IEnumerator e = qsi.InitScroll(param, quest002SideStoryMenu.serverTime);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    LongPressFloatButton btnFormation = qsi.BtnFormation as LongPressFloatButton;
    if (param.descriptions != null && ((IEnumerable<QuestExtraDescription>) param.descriptions).Any<QuestExtraDescription>())
    {
      qsi.setActiveHasDescriptions();
      EventDelegate.Set(btnFormation.onLongPress_, (EventDelegate.Callback) (() =>
      {
        if (this.IsPushAndSet())
          return;
        this.pauseDragScrollView();
        Quest00228Scene.ChangeScene(param.descriptions, true);
      }));
    }
    else
    {
      int questL = param.extra.quest_extra_s.quest_l_QuestExtraL;
      qsi.setActiveHasDescriptions(quest002SideStoryMenu.checkHasDescriptionScoreCampaing(questL));
      EventDelegate.Set(btnFormation.onLongPress_, (EventDelegate.Callback) (() => this.checkChangeDescriptionScoreCampaing(qsi, questL)));
    }
  }

  private IEnumerator coInitScrcollHunting(
    Quest00217TabPage page,
    EventInfo info,
    GameObject prefab)
  {
    Quest002SideStoryMenu quest002SideStoryMenu = this;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    Quest00217Scroll.Parameter parameter = new Quest00217Scroll.Parameter(info);
    parameter.isNew = false;
    parameter.isClear = false;
    parameter.isNotice = false;
    parameter.startTime = new DateTime?();
    parameter.isHighlighting = false;
    page.AddItem(gameObject);
    Quest00217ScrollHunting qsh = gameObject.GetComponent<Quest00217ScrollHunting>();
    if (quest002SideStoryMenu.setupBannerCount >= 4)
    {
      qsh.Setup(parameter, quest002SideStoryMenu.serverTime);
      Coroutine coroutine = quest002SideStoryMenu.StartCoroutine(qsh.SetAndCreate_BannerSprite());
      quest002SideStoryMenu.loadingCoroutineList.Add(coroutine);
    }
    else
    {
      IEnumerator e = qsh.InitScroll(parameter, quest002SideStoryMenu.serverTime);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    EventDelegate.Set((qsh.BtnFormation as LongPressFloatButton).onLongPress_, (EventDelegate.Callback) (() =>
    {
      if (this.IsPushAndSet())
        return;
      this.StartCoroutine(this.coChangeHuntinDescription(qsh, info));
    }));
  }

  private IEnumerator coInitScrollDirect(Quest00217TabPage page, GameObject prefab)
  {
    page.AddItem(Object.Instantiate<GameObject>(prefab));
    yield break;
  }

  private enum BannerType
  {
    Extra,
    Event,
  }

  private class BannerClass
  {
    public int priority;
    public Quest002SideStoryMenu.BannerType type;
    public int index;

    public BannerClass(int priority, Quest002SideStoryMenu.BannerType type, int index)
    {
      this.priority = priority;
      this.type = type;
      this.index = index;
    }
  }
}
