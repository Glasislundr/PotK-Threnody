// Decompiled with JetBrains decompiler
// Type: Quest00217Menu
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
[AddComponentMenu("Scenes/QuestExtra/TopMenu")]
public class Quest00217Menu : BackButtonMenuBase
{
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
  private PlayerQuestGate[] keyQuestsGate;
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
  private int[] corps_no_entry_ids;

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
    int[] corps_no_entry_ids,
    int[] Emphasis,
    QuestExtraTimetableNotice[] Notices,
    DateTime player_created_at,
    int activeTabIndex)
  {
    this.extraData = ((IEnumerable<PlayerExtraQuestS>) ExtraData).CheckMasterData().ToArray<PlayerExtraQuestS>();
    this.playerCreatedAt = player_created_at;
    this.TxtTitle.SetText(this.GetTitle());
    this.categories.Clear();
    this.setupBannerCount = 0;
    this.corps_no_entry_ids = corps_no_entry_ids;
    PlayerExtraQuestS[] extraQuests = ((IEnumerable<PlayerExtraQuestS>) this.extraData).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.extra_quest_area == 1 || x.extra_quest_area == 3)).Root(true);
    this.keyQuestsGate = SMManager.Get<PlayerQuestGate[]>();
    IEnumerable<int> second = ((IEnumerable<PlayerExtraQuestS>) this.extraData).Select<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x._quest_extra_s));
    bool isExistKeyQuests = ((IEnumerable<PlayerQuestGate>) this.keyQuestsGate).SelectMany<PlayerQuestGate, int>((Func<PlayerQuestGate, IEnumerable<int>>) (x => (IEnumerable<int>) x.quest_ids)).Intersect<int>(second).Any<int>();
    this.createScrollItemsCategory(this.extraData, Categories);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.serverTime = ServerTime.NowAppTimeAddDelta();
    SM.TowerPeriod[] towerPeriod = ((IEnumerable<SM.TowerPeriod>) SMManager.Get<SM.TowerPeriod[]>()).ToArray<SM.TowerPeriod>();
    CorpsPeriod[] corpsPeriod = Singleton<NGGameDataManager>.GetInstance().corpsUtil.activePeriods;
    EventInfo[] eventInfo = ((IEnumerable<EventInfo>) SMManager.Get<EventInfo[]>()).ToArray<EventInfo>();
    int length1 = extraQuests.Length;
    int length2 = eventInfo.Length;
    int length3 = towerPeriod.Length;
    int length4 = corpsPeriod.Length;
    List<Quest00217Menu.BannerClass> bannerList = new List<Quest00217Menu.BannerClass>(Enum.GetValues(typeof (Quest00217Menu.BannerType)).Length);
    if (isExistKeyQuests)
    {
      e = this.Create_Transitionbutton("metalkey", 1);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    HashSet<int> emphasisHashlist = new HashSet<int>((IEnumerable<int>) Emphasis);
    GameObject[] prefabs = new GameObject[Enum.GetValues(typeof (Quest00217Menu.BannerType)).Length];
    bannerList.Clear();
    for (int index = 0; index < extraQuests.Length; ++index)
    {
      PlayerExtraQuestS playerExtraQuestS = this.elementAt<PlayerExtraQuestS>(extraQuests, index);
      if (playerExtraQuestS != null && this.isQuestConditionEffective(playerExtraQuestS.quest_extra_s.quest_m) && !playerExtraQuestS.IsSideQuest())
        bannerList.Add(new Quest00217Menu.BannerClass(playerExtraQuestS.quest_extra_s.quest_m.priority, Quest00217Menu.BannerType.Extra, index));
    }
    for (int index = 0; index < eventInfo.Length; ++index)
    {
      EventInfo eventInfo1 = this.elementAt<EventInfo>(eventInfo, index);
      if (eventInfo1 != null && !eventInfo1.IsSideQuest())
        bannerList.Add(new Quest00217Menu.BannerClass(eventInfo1.Period() != null ? eventInfo1.Period().priority : int.MaxValue, Quest00217Menu.BannerType.Event, index));
    }
    for (int index = 0; index < towerPeriod.Length; ++index)
    {
      SM.TowerPeriod towerPeriod1 = this.elementAt<SM.TowerPeriod>(towerPeriod, index);
      if (towerPeriod1 != null)
        bannerList.Add(new Quest00217Menu.BannerClass(towerPeriod1.priority, Quest00217Menu.BannerType.Tower, index));
    }
    for (int index = 0; index < corpsPeriod.Length; ++index)
    {
      CorpsPeriod corpsPeriod1 = this.elementAt<CorpsPeriod>(corpsPeriod, index);
      if (corpsPeriod1 != null)
        bannerList.Add(new Quest00217Menu.BannerClass(corpsPeriod1.priority, Quest00217Menu.BannerType.Corps, index));
    }
    bannerList = bannerList.OrderBy<Quest00217Menu.BannerClass, int>((Func<Quest00217Menu.BannerClass, int>) (x => x.priority)).ThenBy<Quest00217Menu.BannerClass, Quest00217Menu.BannerType>((Func<Quest00217Menu.BannerClass, Quest00217Menu.BannerType>) (x => x.type)).ToList<Quest00217Menu.BannerClass>();
    Future<GameObject> ScrollPrefab;
    foreach (Quest00217Menu.BannerClass banner in bannerList)
    {
      if (banner != null)
      {
        switch (banner.type)
        {
          case Quest00217Menu.BannerType.Extra:
            if (!Object.op_Implicit((Object) prefabs[(int) banner.type]))
            {
              ScrollPrefab = Res.Prefabs.quest002_17.scroll.Load<GameObject>();
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
          case Quest00217Menu.BannerType.Event:
            if (!Object.op_Implicit((Object) prefabs[(int) banner.type]))
            {
              ScrollPrefab = Res.Prefabs.quest002_17.scroll_hunting.Load<GameObject>();
              e = ScrollPrefab.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              prefabs[(int) banner.type] = ScrollPrefab.Result;
              ScrollPrefab = (Future<GameObject>) null;
            }
            e = this.CreatePunitiveExpeditionEventButton(eventInfo[banner.index], prefabs[(int) banner.type]);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case Quest00217Menu.BannerType.Tower:
            if (!Object.op_Implicit((Object) prefabs[(int) banner.type]))
            {
              ScrollPrefab = new ResourceObject("Prefabs/quest002_17/scroll_tower").Load<GameObject>();
              e = ScrollPrefab.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              prefabs[(int) banner.type] = ScrollPrefab.Result;
              ScrollPrefab = (Future<GameObject>) null;
            }
            e = this.CreateTowerButton(towerPeriod[banner.index], prefabs[(int) banner.type]);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case Quest00217Menu.BannerType.Corps:
            if (!Object.op_Implicit((Object) prefabs[(int) banner.type]))
            {
              ScrollPrefab = new ResourceObject("Prefabs/quest002_17/scroll_corps").Load<GameObject>();
              e = ScrollPrefab.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              prefabs[(int) banner.type] = ScrollPrefab.Result;
              ScrollPrefab = (Future<GameObject>) null;
            }
            this.createCorpsButton(corpsPeriod[banner.index], prefabs[(int) banner.type]);
            break;
        }
      }
    }
    int categoryId = activeTabIndex;
    if (activeTabIndex <= 0)
      categoryId = !this.isOpenedAnyKeyQuest(this.extraData) ? (!(this.serverTime - this.playerCreatedAt < Quest00217Menu.biginnerSpn) ? (Quest00217Menu.beforeCategoryId <= 0 ? 2 : Quest00217Menu.beforeCategoryId) : 4) : 1;
    yield return (object) this.SetupCategory(categoryId);
    yield return (object) new WaitForEndOfFrame();
  }

  private bool isOpenedAnyKeyQuest(PlayerExtraQuestS[] ExtraData)
  {
    Func<PlayerExtraQuestS[], int, int?> GetLid = (Func<PlayerExtraQuestS[], int, int?>) ((ex, id_s) => Array.Find<PlayerExtraQuestS>(ex, (Predicate<PlayerExtraQuestS>) (fd => fd._quest_extra_s == id_s))?.quest_extra_s.quest_l_QuestExtraL);
    return ((IEnumerable<PlayerQuestGate>) this.keyQuestsGate).Where<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => ((IEnumerable<int>) x.quest_ids).Any<int>((Func<int, bool>) (y => ((IEnumerable<PlayerExtraQuestS>) ExtraData).Any<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (z => z._quest_extra_s == y)))))).Where<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (w =>
    {
      IEnumerable<int> idGateS = ((IEnumerable<PlayerQuestGate>) this.keyQuestsGate).SelectMany<PlayerQuestGate, int>((Func<PlayerQuestGate, IEnumerable<int>>) (s => (IEnumerable<int>) s.quest_ids));
      int? nullable = GetLid(ExtraData, w.quest_ids[0]);
      return nullable.HasValue && !((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) ExtraData).M(nullable.Value, true)).Where<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (extra_s => !idGateS.Contains<int>(extra_s._quest_extra_s))).Any<PlayerExtraQuestS>();
    })).Any<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (quest => quest.in_progress));
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
    foreach (QuestExtraCategory questExtraCategory in ((IEnumerable<int>) sortedCategory).Select<int, QuestExtraCategory>((Func<int, QuestExtraCategory>) (i => this.getQuestCategory(i))).ToList<QuestExtraCategory>())
    {
      Quest00217TabPage quest00217TabPage = questExtraCategory.ID <= this.tabPages.Length ? this.tabPages[questExtraCategory.ID - 1] : this.tabPages[1];
      quest00217TabPage.Init(this, questExtraCategory.ID);
      this.categories.Add(questExtraCategory.ID, quest00217TabPage);
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
    Quest00217Menu quest00217Menu = this;
    quest00217Menu.pauseDragScrollView();
    Description description;
    if (!quest00217Menu.dicHuntingDescriptions_.TryGetValue(info.period_id, out description))
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
        quest00217Menu.dicHuntingDescriptions_.Add(info.period_id, description);
        request = (Future<WebAPI.Response.EventTop>) null;
      }
    }
    if (description == null)
    {
      cntl.setEffectNoDescription();
      quest00217Menu.IsPush = false;
    }
    else
      Quest00228Scene.ChangeScene(description, true);
  }

  private IEnumerator CreateTowerButton(SM.TowerPeriod info, GameObject prefab)
  {
    this.categories[info.category_id].AddRequest(info, prefab);
    yield break;
  }

  private void createCorpsButton(CorpsPeriod info, GameObject prefab)
  {
    this.categories[2].AddRequest(info, prefab);
  }

  private void OnDisable() => this.resumeDragScrollView();

  private string GetTitle() => Consts.GetInstance().QUEST_00217_NORMAL_TITLE;

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
    Quest00217Menu.beforeCategoryId = categoryId;
    this.isPageLoading = false;
  }

  private IEnumerator SetupBanner(Quest00217TabPage page, Quest00217TabPage.RequestParam request)
  {
    IEnumerator e;
    switch (request.Param)
    {
      case Quest00217Scroll.Parameter parameter:
        e = this.ScrollInit(page, parameter, request.Prefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case EventInfo info1:
        e = this.coInitScrcollHunting(page, info1, request.Prefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case SM.TowerPeriod info2:
        e = this.coInitScrollTower(page, info2, request.Prefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case CorpsPeriod info3:
        e = this.coInitScrollCorps(page, info3, request.Prefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      default:
        if (!Object.op_Implicit((Object) request.Prefab))
          break;
        yield return (object) this.coInitScrollDirect(page, request.Prefab);
        break;
    }
  }

  public virtual IEnumerator ScrollInit(
    Quest00217TabPage page,
    Quest00217Scroll.Parameter param,
    GameObject prefab)
  {
    Quest00217Menu quest00217Menu = this;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    page.AddItem(gameObject);
    Quest00217Scroll qsi = gameObject.GetComponent<Quest00217Scroll>();
    if (quest00217Menu.setupBannerCount >= 4)
    {
      qsi.Setup(param, quest00217Menu.serverTime);
      Coroutine coroutine = quest00217Menu.StartCoroutine(qsi.SetAndCreate_BannerSprite());
      quest00217Menu.loadingCoroutineList.Add(coroutine);
    }
    else
    {
      IEnumerator e = qsi.InitScroll(param, quest00217Menu.serverTime);
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
      qsi.setActiveHasDescriptions(quest00217Menu.checkHasDescriptionScoreCampaing(questL));
      EventDelegate.Set(btnFormation.onLongPress_, (EventDelegate.Callback) (() => this.checkChangeDescriptionScoreCampaing(qsi, questL)));
    }
  }

  private IEnumerator coInitScrcollHunting(
    Quest00217TabPage page,
    EventInfo info,
    GameObject prefab)
  {
    Quest00217Menu quest00217Menu = this;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    Quest00217Scroll.Parameter parameter = new Quest00217Scroll.Parameter(info);
    parameter.isNew = false;
    parameter.isClear = false;
    parameter.isNotice = false;
    parameter.startTime = new DateTime?();
    parameter.isHighlighting = false;
    page.AddItem(gameObject);
    Quest00217ScrollHunting qsh = gameObject.GetComponent<Quest00217ScrollHunting>();
    if (quest00217Menu.setupBannerCount >= 4)
    {
      qsh.Setup(parameter, quest00217Menu.serverTime);
      Coroutine coroutine = quest00217Menu.StartCoroutine(qsh.SetAndCreate_BannerSprite());
      quest00217Menu.loadingCoroutineList.Add(coroutine);
    }
    else
    {
      IEnumerator e = qsh.InitScroll(parameter, quest00217Menu.serverTime);
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

  private IEnumerator coInitScrollTower(
    Quest00217TabPage page,
    SM.TowerPeriod info,
    GameObject prefab)
  {
    Quest00217Menu quest00217Menu = this;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    Quest00217Scroll.Parameter parameter = new Quest00217Scroll.Parameter(info);
    parameter.isNew = false;
    parameter.isClear = false;
    parameter.isNotice = false;
    parameter.startTime = new DateTime?();
    parameter.isHighlighting = false;
    TowerQuestEntryCondition[] array = ((IEnumerable<TowerQuestEntryCondition>) SMManager.Get<TowerQuestEntryCondition[]>()).ToArray<TowerQuestEntryCondition>();
    parameter.entryConditionID = 0;
    foreach (TowerQuestEntryCondition questEntryCondition in array)
    {
      if (questEntryCondition.tower_id == info.tower_id)
      {
        parameter.entryConditionID = questEntryCondition.id;
        break;
      }
    }
    page.AddItem(gameObject);
    Quest00217ScrollTower qsh = gameObject.GetComponent<Quest00217ScrollTower>();
    if (quest00217Menu.setupBannerCount >= 4)
    {
      qsh.Setup(parameter, quest00217Menu.serverTime);
      Coroutine coroutine = quest00217Menu.StartCoroutine(qsh.SetAndCreate_BannerSprite());
      quest00217Menu.loadingCoroutineList.Add(coroutine);
    }
    else
    {
      IEnumerator e = qsh.InitScroll(parameter, quest00217Menu.serverTime);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set((qsh.BtnFormation as LongPressFloatButton).onLongPress_, new EventDelegate.Callback(quest00217Menu.\u003CcoInitScrollTower\u003Eb__57_0));
  }

  private IEnumerator coInitScrollCorps(
    Quest00217TabPage page,
    CorpsPeriod info,
    GameObject prefab)
  {
    Quest00217Menu quest00217Menu = this;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    Quest00217Scroll.Parameter parameter = new Quest00217Scroll.Parameter(info);
    parameter.entryConditionID = 0;
    foreach (CorpsEntryConditions corpsEntryConditions in MasterData.CorpsEntryConditionsList)
    {
      if (corpsEntryConditions.setting_CorpsSetting == info.ID)
      {
        if (((IEnumerable<int>) quest00217Menu.corps_no_entry_ids).Contains<int>(corpsEntryConditions.ID))
        {
          parameter.entryConditionID = corpsEntryConditions.ID;
          break;
        }
        break;
      }
    }
    page.AddItem(gameObject);
    Quest00217ScrollCorps qsh = gameObject.GetComponent<Quest00217ScrollCorps>();
    if (quest00217Menu.setupBannerCount >= 4)
    {
      qsh.Setup(parameter, quest00217Menu.serverTime);
      Coroutine coroutine = quest00217Menu.StartCoroutine(qsh.SetAndCreate_BannerSprite());
      quest00217Menu.loadingCoroutineList.Add(coroutine);
    }
    else
    {
      IEnumerator e = qsh.InitScroll(parameter, quest00217Menu.serverTime);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    EventDelegate.Set((qsh.BtnFormation as LongPressFloatButton).onLongPress_, (EventDelegate.Callback) (() =>
    {
      if (this.IsPushAndSet())
        return;
      this.pauseDragScrollView();
      CorpsQuestManualScene.ChangeScene(info.setting);
    }));
  }

  private IEnumerator coInitScrollDirect(Quest00217TabPage page, GameObject prefab)
  {
    page.AddItem(Object.Instantiate<GameObject>(prefab));
    yield break;
  }

  public void onButtonOpenSideStory()
  {
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized || this.IsPush)
      return;
    string defaultSceneName = Quest002SideStoryScene.DefaultSceneName;
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != defaultSceneName) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Quest002SideStoryScene.ChangeScene(true);
  }

  private enum BannerType
  {
    Extra,
    Event,
    Tower,
    Corps,
  }

  private class BannerClass
  {
    public int priority;
    public Quest00217Menu.BannerType type;
    public int index;

    public BannerClass(int priority, Quest00217Menu.BannerType type, int index)
    {
      this.priority = priority;
      this.type = type;
      this.index = index;
    }
  }
}
