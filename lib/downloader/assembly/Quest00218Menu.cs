// Decompiled with JetBrains decompiler
// Type: Quest00218Menu
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
[AddComponentMenu("Scenes/QuestExtra/LxM_Menu")]
public class Quest00218Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle_;
  [SerializeField]
  private UIScrollView scrollView_;
  [SerializeField]
  private UIGrid grid_;
  [SerializeField]
  private UI2DSprite eventSprite_;
  [SerializeField]
  private GameObject topDragScroll_;
  [Header("AnchorTopControl")]
  [Tooltip("eventSprite_ の有無に合わせてScrollViewのTop位置を調整")]
  [SerializeField]
  private UIWidget widgetScrollViewTop_;
  [SerializeField]
  private int whenOnEventSprite_;
  [SerializeField]
  private int whenOffEventSprige_;
  private DateTime serverTime_;
  private List<UIDragScrollView> pauseDragScrollViews_;
  private PlayerExtraQuestS[] extraData_;
  private QuestExtraLL headerInfo_;
  private int tabNo_;
  private GameObject scrollItem_;
  private int runningCoroutine_;
  public bool isSideQuest;
  private Queue<Tuple<PlayerExtraQuestS, IEnumerator>> lateAddItems_;
  private const int DEF_QUEST_CATEGORY = 4;

  public IEnumerator initializeAsync(
    QuestExtraLL headerInfo,
    PlayerExtraQuestS[] extraData,
    int[] Emphasis,
    QuestExtraTimetableNotice[] Notices,
    int? SId)
  {
    Quest00218Menu quest00218Menu = this;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    Quest00218Menu.\u003C\u003Ec__DisplayClass18_0 cDisplayClass180 = new Quest00218Menu.\u003C\u003Ec__DisplayClass18_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass180.\u003C\u003E4__this = this;
    quest00218Menu.runningCoroutine_ = 0;
    quest00218Menu.StartCoroutine(quest00218Menu.doLoadPrefabs());
    quest00218Menu.StartCoroutine(quest00218Menu.doWaitWebAPI());
    quest00218Menu.headerInfo_ = headerInfo;
    if (SId.HasValue)
    {
      // ISSUE: reference to a compiler-generated field
      MasterData.QuestExtraS.TryGetValue(SId.Value, out cDisplayClass180.focus);
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      cDisplayClass180.focus = (QuestExtraS) null;
    }
    if (headerInfo.enabled_header)
    {
      ((UIRect) quest00218Menu.widgetScrollViewTop_).topAnchor.absolute = quest00218Menu.whenOnEventSprite_;
      ((Component) quest00218Menu.eventSprite_).gameObject.SetActive(true);
      Future<Texture2D> futureEvent = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(quest00218Menu.loadSpriteEventPath(headerInfo.banner_image_id.GetValueOrDefault(headerInfo.ID)));
      yield return (object) futureEvent.Wait();
      Texture2D result = futureEvent.Result;
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      quest00218Menu.eventSprite_.sprite2D = sprite;
      futureEvent = (Future<Texture2D>) null;
    }
    else
    {
      ((UIRect) quest00218Menu.widgetScrollViewTop_).topAnchor.absolute = quest00218Menu.whenOffEventSprige_;
      ((Component) quest00218Menu.eventSprite_).gameObject.SetActive(false);
    }
    quest00218Menu.extraData_ = extraData;
    HashSet<int> emphasis = new HashSet<int>((IEnumerable<int>) Emphasis);
    while (quest00218Menu.runningCoroutine_ > 0)
      yield return (object) null;
    quest00218Menu.lateAddItems_ = new Queue<Tuple<PlayerExtraQuestS, IEnumerator>>();
    PlayerExtraQuestS[] allStatus = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).CheckMasterData().ToArray<PlayerExtraQuestS>();
    PlayerExtraQuestS[] playerExtraQuestSArray = quest00218Menu.extraData_;
    for (int index = 0; index < playerExtraQuestSArray.Length; ++index)
    {
      PlayerExtraQuestS extraData1 = playerExtraQuestSArray[index];
      if (quest00218Menu.isQuestConditionEffective(extraData1.quest_extra_s.quest_m_QuestExtraM))
      {
        IEnumerator e = quest00218Menu.initScrollItem(extraData1, allStatus, emphasis, Notices);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    playerExtraQuestSArray = (PlayerExtraQuestS[]) null;
    if (quest00218Menu.lateAddItems_.Any<Tuple<PlayerExtraQuestS, IEnumerator>>())
    {
      List<PlayerExtraQuestS> sortLst = ((IEnumerable<PlayerExtraQuestS>) extraData).ToList<PlayerExtraQuestS>();
      do
      {
        Tuple<PlayerExtraQuestS, IEnumerator> tuple = quest00218Menu.lateAddItems_.Dequeue();
        sortLst.Remove(tuple.Item1);
        sortLst.Add(tuple.Item1);
        yield return (object) tuple.Item2;
      }
      while (quest00218Menu.lateAddItems_.Any<Tuple<PlayerExtraQuestS, IEnumerator>>());
      quest00218Menu.extraData_ = sortLst.ToArray();
      sortLst = (List<PlayerExtraQuestS>) null;
    }
    quest00218Menu.lateAddItems_ = (Queue<Tuple<PlayerExtraQuestS, IEnumerator>>) null;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    cDisplayClass180.firstIndex = cDisplayClass180.focus != null ? (cDisplayClass180.focus.seek_type == QuestExtra.SeekType.M ? Array.FindIndex<PlayerExtraQuestS>(quest00218Menu.extraData_, new Predicate<PlayerExtraQuestS>(cDisplayClass180.\u003CinitializeAsync\u003Eb__0)) : Array.FindIndex<PlayerExtraQuestS>(quest00218Menu.extraData_, new Predicate<PlayerExtraQuestS>(cDisplayClass180.\u003CinitializeAsync\u003Eb__1))) : 0;
    if (quest00218Menu.extraData_.Length != 0)
    {
      PlayerExtraQuestS playerExtraQuestS = ((IEnumerable<PlayerExtraQuestS>) quest00218Menu.extraData_).First<PlayerExtraQuestS>();
      quest00218Menu.tabNo_ = playerExtraQuestS.seek_type == PlayerExtraQuestS.SeekType.L ? playerExtraQuestS.quest_extra_s.quest_l.category_QuestExtraCategory : playerExtraQuestS.quest_extra_s.quest_m.category_QuestExtraCategory;
    }
    else
      quest00218Menu.tabNo_ = 4;
    // ISSUE: method pointer
    quest00218Menu.grid_.onReposition = new UIGrid.OnReposition((object) cDisplayClass180, __methodptr(\u003CinitializeAsync\u003Eb__2));
    quest00218Menu.grid_.repositionNow = true;
    quest00218Menu.grid_.Reposition();
  }

  private IEnumerator doLoadPrefabs()
  {
    ++this.runningCoroutine_;
    if (Object.op_Equality((Object) this.scrollItem_, (Object) null))
    {
      Future<GameObject> ScrollPrefab = Res.Prefabs.quest002_17.scroll.Load<GameObject>();
      yield return (object) ScrollPrefab.Wait();
      this.scrollItem_ = ScrollPrefab.Result;
      ScrollPrefab = (Future<GameObject>) null;
    }
    --this.runningCoroutine_;
  }

  private IEnumerator doWaitWebAPI()
  {
    ++this.runningCoroutine_;
    yield return (object) ServerTime.WaitSync();
    this.serverTime_ = ServerTime.NowAppTimeAddDelta();
    --this.runningCoroutine_;
  }

  private void resetScrollPosition(int index)
  {
    this.scrollView_.ResetPosition();
    this.grid_.onReposition = (UIGrid.OnReposition) null;
    if (index <= 0)
      return;
    Bounds bounds1 = this.scrollView_.bounds;
    int num1 = (int) ((double) ((Bounds) ref bounds1).size.x / (double) this.grid_.cellWidth);
    if (num1 < 1)
      num1 = 1;
    float num2 = (float) (((Component) this.grid_).transform.childCount / num1);
    if (((Component) this.grid_).transform.childCount % num1 > 0)
      ++num2;
    if ((double) num2 < 1.0)
      num2 = 1f;
    Bounds bounds2 = this.scrollView_.bounds;
    float num3 = ((Bounds) ref bounds2).size.y - this.scrollView_.panel.height;
    Bounds bounds3 = this.scrollView_.bounds;
    double num4 = (double) ((Bounds) ref bounds3).size.y / (double) num2;
    float num5 = (float) (num4 / 2.0) / num3;
    float num6 = this.scrollView_.panel.height / 2f / num3;
    float num7 = (float) (num4 * (double) (index / num1) / (double) num3 - ((double) num6 - (double) num5));
    if ((double) num3 <= 0.0)
      num7 = 0.0f;
    else if ((double) num7 < 0.0)
      num7 = 0.0f;
    else if ((double) num7 > 1.0)
      num7 = 1f;
    this.scrollView_.SetDragAmount(0.0f, num7, false);
    this.scrollView_.SetDragAmount(0.0f, num7, true);
  }

  private string loadSpriteEventPath(int llId)
  {
    string path = "Prefabs/Banners/ExtraQuest/LL/" + llId.ToString() + "/Specialquest_Story";
    return Singleton<ResourceManager>.GetInstance().Contains(path) ? path : "Prefabs/Banners/ExtraQuest/L/4/Specialquest_Story";
  }

  private bool isQuestConditionEffective(int questMId)
  {
    QuestExtraReleaseConditionsPlayer conditionsPlayer = Array.Find<QuestExtraReleaseConditionsPlayer>(MasterData.QuestExtraReleaseConditionsPlayerList, (Predicate<QuestExtraReleaseConditionsPlayer>) (x => x.quest_m_QuestExtraM == questMId));
    if (conditionsPlayer == null)
      return true;
    int level = Player.Current.level;
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

  private IEnumerator initScrollItem(
    PlayerExtraQuestS extraData,
    PlayerExtraQuestS[] ExtraData,
    HashSet<int> Emphasis,
    QuestExtraTimetableNotice[] Notices)
  {
    QuestExtraS questExtraS = extraData.quest_extra_s;
    Quest00217Scroll.Parameter parameter = new Quest00217Scroll.Parameter();
    if (extraData.seek_type == PlayerExtraQuestS.SeekType.L)
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
    QuestExtraTimetableNotice extraTimetableNotice = Array.Find<QuestExtraTimetableNotice>(Notices, (Predicate<QuestExtraTimetableNotice>) (n => n._quest_extra_s == extraData._quest_extra_s));
    if (extraTimetableNotice != null && extraTimetableNotice.start_at.HasValue)
    {
      parameter.isNotice = true;
      parameter.startTime = extraTimetableNotice.start_at;
    }
    parameter.setMainData(extraData);
    if (parameter.isClearedToday)
      this.lateAddItems_.Enqueue(Tuple.Create<PlayerExtraQuestS, IEnumerator>(extraData, this.initScrollItem(this.grid_, parameter)));
    else
      yield return (object) this.initScrollItem(this.grid_, parameter);
  }

  public IEnumerator updateTime()
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.serverTime_ = ServerTime.NowAppTimeAddDelta();
    foreach (Quest00217Scroll quest00217Scroll in ((Component) this.grid_).transform.GetChildren().Select<Transform, Quest00217Scroll>((Func<Transform, Quest00217Scroll>) (t => ((Component) t).gameObject.GetComponent<Quest00217Scroll>())))
      quest00217Scroll.SetTime(this.serverTime_, quest00217Scroll.RankingEventTerm);
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    if (!this.isSideQuest)
      Quest00217Scene.backOrChangeScene(this.tabNo_);
    else
      Quest002SideStoryScene.backOrChangeScene(this.tabNo_);
    this.isSideQuest = false;
  }

  public override void onBackButton() => this.IbtnBack();

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
    this.scrollView_.Press(false);
  }

  private void resumeDragScrollView()
  {
    if (this.pauseDragScrollViews_ == null || !this.pauseDragScrollViews_.Any<UIDragScrollView>())
      return;
    foreach (Behaviour pauseDragScrollView in this.pauseDragScrollViews_)
      pauseDragScrollView.enabled = true;
    this.pauseDragScrollViews_.Clear();
  }

  private IEnumerator initScrollItem(UIGrid grid, Quest00217Scroll.Parameter param)
  {
    Quest00217Scroll qsi = this.scrollItem_.Clone(((Component) grid).transform).GetComponent<Quest00217Scroll>();
    IEnumerator e = qsi.InitScroll(param, this.serverTime_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    LongPressFloatButton btnFormation = qsi.BtnFormation as LongPressFloatButton;
    if (param.descriptions != null && param.descriptions.Length != 0)
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
      EventDelegate.Set(btnFormation.onLongPress_, (EventDelegate.Callback) (() => qsi.setEffectNoDescription()));
  }
}
