// Decompiled with JetBrains decompiler
// Type: Quest00214aMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/QuestCharacter/SMenu")]
public class Quest00214aMenu : BackButtonMenuBase
{
  [SerializeField]
  private NGTweenParts[] cntlOpenClose;
  [SerializeField]
  private UILabel txtTitle;
  public NGHorizontalScrollParts indicator;
  public Quest00214DetailDisplay container;
  public UIScrollView ScrollView;
  public UIGrid grid;
  public GameObject mask;
  public GameObject release;
  private bool releaseConditionActiveFlag;
  private bool ButtonMove;
  private int QuestQuantity;
  private GameObject CenterObj;
  private PlayerQuestSConverter[] quests_;
  private Tuple<QuestSConverter, PlayerQuestSConverter>[] tblEpisodeQuest_;
  private List<QuestDisplayConditionConverter>[] tblDisplayConditions_ = new List<QuestDisplayConditionConverter>[0];
  private List<GameObject> StageObjects = new List<GameObject>();
  private int countTween;
  private Quest00214aMenu.Mode mode;
  [NonSerialized]
  public bool SceneStart;
  private List<UITweener> StartTweeners;
  private List<UITweener> EndTweeners;
  private List<UITweener> BothTweeners;
  private GameObject compareCenterObj;
  [SerializeField]
  private GameObject ButtonMission;
  [SerializeField]
  private GameObject Character;
  [SerializeField]
  private GameObject CharacterCombi;
  [SerializeField]
  private GameObject CharacterCombiTarget;
  [SerializeField]
  private GameObject CharacterTrioLeft;
  [SerializeField]
  private GameObject CharacterTrioCenter;
  [SerializeField]
  private GameObject CharacterTrioRight;
  [SerializeField]
  private NGxMaskSpriteWithScale SubBG;
  private UICenterOnChild ctrlCenter_;
  private QuestDetailManager detailManager_;
  private bool isChangedQuestDetail_;
  private bool isResetToneCondition_;
  private GameObject unitIconPrefab;
  private GameObject releaseConditionPrefab;
  private GameObject hscrollPrefab;
  private Texture2D maskTexture;
  private Texture2D bgMaskTexture;
  private bool isPopup_;
  private Action<Action> onBeforeChangeScne_;
  private bool? isStartTweenFinished;

  private UICenterOnChild ctrlCenter
  {
    get
    {
      if (Object.op_Equality((Object) this.ctrlCenter_, (Object) null))
        this.ctrlCenter_ = ((Component) this.grid).GetComponent<UICenterOnChild>();
      return this.ctrlCenter_;
    }
  }

  private QuestDetailManager detailManager
  {
    get
    {
      if (this.detailManager_ == null)
        this.detailManager_ = new QuestDetailManager();
      return this.detailManager_;
    }
  }

  public List<EventDelegate> onClose { get; private set; } = new List<EventDelegate>();

  public List<EventDelegate> onCloseFinished { get; private set; } = new List<EventDelegate>();

  public bool isLoadedResources { get; private set; }

  public IEnumerator doLoadResources()
  {
    this.isLoadedResources = false;
    Future<GameObject> unitIconPrefabF;
    if (Object.op_Equality((Object) this.unitIconPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      yield return (object) unitIconPrefabF.Wait();
      this.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.releaseConditionPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.quest002_14.Condition.Load<GameObject>();
      yield return (object) unitIconPrefabF.Wait();
      this.releaseConditionPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    Future<Texture2D> maskF;
    if (Object.op_Equality((Object) this.maskTexture, (Object) null))
    {
      maskF = Res.GUI._009_3_sozai.mask_Chara_C.Load<Texture2D>();
      yield return (object) maskF.Wait();
      this.maskTexture = maskF.Result;
      maskF = (Future<Texture2D>) null;
    }
    if (Object.op_Equality((Object) this.bgMaskTexture, (Object) null))
    {
      maskF = Res.GUI._002_2_sozai.bg_mask_quest_stage_select.Load<Texture2D>();
      yield return (object) maskF.Wait();
      this.bgMaskTexture = maskF.Result;
      maskF = (Future<Texture2D>) null;
    }
    if (Object.op_Equality((Object) this.hscrollPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.quest002_14.hscroll_640.Load<GameObject>();
      yield return (object) unitIconPrefabF.Wait();
      this.hscrollPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    this.isLoadedResources = true;
  }

  protected override void Update()
  {
    if (!((Component) this).gameObject.activeSelf || !this.SceneStart || this.StageObjects.Count == 0 || this.isChangedQuestDetail_)
      return;
    base.Update();
    bool flag1 = !this.ScrollView.isDragging && !this.ButtonMove;
    for (int index = 0; index < this.StageObjects.Count; ++index)
    {
      GameObject stageObject = this.StageObjects[index];
      Quest00214Hscroll component = stageObject.GetComponent<Quest00214Hscroll>();
      PlayerQuestSConverter playerQuestSconverter = this.tblEpisodeQuest_[index].Item2;
      bool flag2 = playerQuestSconverter != null && !playerQuestSconverter.is_clear;
      if (!flag1 || !flag2)
      {
        if ((double) Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x + component.defaultPosition()) < (double) component.spaceValue())
          component.ChangeToneConditionJudge(Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x) - Mathf.Abs(component.defaultPosition()));
        else
          component.ChangeToneConditionJudge(Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x + component.defaultPosition()));
        if (flag2)
        {
          component.centerAnimation(false);
          component.NotTouch(true);
        }
      }
      else if (Object.op_Equality((Object) stageObject, (Object) this.ctrlCenter.centeredObject) & flag1)
      {
        component.centerAnimation(true);
        component.NotTouch(false);
      }
      else if (this.isResetToneCondition_)
      {
        if ((double) Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x + component.defaultPosition()) < (double) component.spaceValue())
          component.ChangeToneConditionJudge(Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x) - Mathf.Abs(component.defaultPosition()));
        else
          component.ChangeToneConditionJudge(Mathf.Abs(((Component) this.ScrollView).transform.localPosition.x + component.defaultPosition()));
      }
      if (Object.op_Equality((Object) stageObject, (Object) this.ctrlCenter.centeredObject) && Object.op_Inequality((Object) stageObject, (Object) this.compareCenterObj))
      {
        this.compareCenterObj = stageObject;
        QuestSConverter StageData = this.tblEpisodeQuest_[index].Item1;
        this.container.InitDetailDisplay(StageData, this.tblEpisodeQuest_[index].Item2, component.stageNumber());
        if (this.tblDisplayConditions_[index] == null)
          this.tblDisplayConditions_[index] = this.getDisplayConditionList(StageData.ID);
        this.StartCoroutine(this.InitReleases(this.tblDisplayConditions_[index]));
      }
    }
    this.isResetToneCondition_ = false;
    if (!this.ScrollView.isDragging && !this.ButtonMove)
      return;
    this.TweenStart(true);
  }

  public override void onBackButton()
  {
    if (this.countTween > 0 || this.IsPushAndSet())
      return;
    this.Ending();
  }

  private List<QuestDisplayConditionConverter> getDisplayConditionList(int questId)
  {
    IEnumerable<QuestDisplayConditionConverter> source = this.mode == Quest00214aMenu.Mode.Character ? ((IEnumerable<QuestCharacterDisplayCondition>) MasterData.QuestCharacterDisplayConditionList).Where<QuestCharacterDisplayCondition>((Func<QuestCharacterDisplayCondition, bool>) (q => q.quest_s_QuestCharacterS == questId)).Select<QuestCharacterDisplayCondition, QuestDisplayConditionConverter>((Func<QuestCharacterDisplayCondition, QuestDisplayConditionConverter>) (q => new QuestDisplayConditionConverter(q))) : (this.mode == Quest00214aMenu.Mode.Harmony ? ((IEnumerable<QuestHarmonyDisplayCondition>) MasterData.QuestHarmonyDisplayConditionList).Where<QuestHarmonyDisplayCondition>((Func<QuestHarmonyDisplayCondition, bool>) (q => q.quest_s_QuestHarmonyS == questId)).Select<QuestHarmonyDisplayCondition, QuestDisplayConditionConverter>((Func<QuestHarmonyDisplayCondition, QuestDisplayConditionConverter>) (q => new QuestDisplayConditionConverter(q))) : (IEnumerable<QuestDisplayConditionConverter>) null);
    return source == null ? new List<QuestDisplayConditionConverter>() : source.OrderBy<QuestDisplayConditionConverter, int>((Func<QuestDisplayConditionConverter, int>) (x => x.priority)).ToList<QuestDisplayConditionConverter>();
  }

  private void CommonInit()
  {
    this.indicator.SeEnable = false;
    ((Component) this).GetComponent<UIRect>().alpha = 0.0f;
    foreach (NGTweenParts ngTweenParts in this.cntlOpenClose)
    {
      ngTweenParts.resetActive(false);
      ((Component) ngTweenParts).GetComponent<UIRect>().alpha = 0.0f;
    }
    this.quests_ = (PlayerQuestSConverter[]) null;
    this.tblEpisodeQuest_ = (Tuple<QuestSConverter, PlayerQuestSConverter>[]) null;
    this.StageObjects.Clear();
    this.indicator.destroyParts();
  }

  private IEnumerator LoadCharacterSprite(int id, GameObject locationObject)
  {
    locationObject.transform.Clear();
    IEnumerator e = MasterData.UnitUnit[id].LoadQuestWithMask(locationObject.transform, locationObject.GetComponent<UIWidget>().depth, Res.GUI._002_2_sozai.mask_chara.Load<Texture2D>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadCharacterStorySprite(int id, GameObject locationObject)
  {
    locationObject.transform.Clear();
    Future<GameObject> f = MasterData.UnitUnit[id].LoadStory();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject go = f.Result.Clone(locationObject.transform);
    MasterData.UnitUnit[id].SetStoryData(go);
    go.GetComponent<NGxMaskSpriteWithScale>().maskTexture = this.maskTexture;
  }

  private void SetTween()
  {
    this.StartTweeners = new List<UITweener>();
    this.EndTweeners = new List<UITweener>();
    this.BothTweeners = new List<UITweener>();
    UITweener[] componentsInChildren = ((Component) this).GetComponentsInChildren<UITweener>();
    this.countTween = 0;
    foreach (UITweener uiTweener in componentsInChildren)
    {
      switch (uiTweener.tweenGroup)
      {
        case 22:
          ++this.countTween;
          this.StartTweeners.Add(uiTweener);
          break;
        case 33:
          this.EndTweeners.Add(uiTweener);
          break;
        case 44:
          this.BothTweeners.Add(uiTweener);
          break;
      }
    }
  }

  private IEnumerator SetBg(QuestSConverter questData, PlayerQuestSConverter playerData, int index)
  {
    Future<Sprite> bgSprite = Singleton<ResourceManager>.GetInstance().Load<Sprite>(questData.quest_m.background_image_path);
    IEnumerator e = bgSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SubBG.MainUI2DSprite.sprite2D = bgSprite.Result;
    this.SubBG.maskTexture = this.bgMaskTexture;
    this.SubBG.xOffsetPixel = (int) questData.quest_m.offset_x;
    this.SubBG.yOffsetPixel = (int) questData.quest_m.offset_y;
    this.SubBG.scale = questData.quest_m.scale;
    this.SubBG.FitMask();
    this.container.StartInit();
    this.container.InitDetailDisplay(questData, playerData, index + 1);
  }

  private void SetGrayQuestIcon(int questLength, GameObject character)
  {
    // ISSUE: method pointer
    this.ctrlCenter.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(\u003CSetGrayQuestIcon\u003Eb__69_0));
    if (questLength != 0)
      return;
    ((UIWidget) ((Component) character.transform.GetChild(0)).GetComponent<UI2DSprite>()).color = Color.Lerp(Color.white, Color.black, 0.5f);
  }

  public IEnumerator Init(int id, WebAPI.Response.QuestProgressCharacter apiResponse)
  {
    this.Character.gameObject.SetActive(true);
    this.CharacterCombi.gameObject.SetActive(false);
    this.CharacterCombiTarget.gameObject.SetActive(false);
    this.CharacterTrioLeft.gameObject.SetActive(false);
    this.CharacterTrioCenter.gameObject.SetActive(false);
    this.CharacterTrioRight.gameObject.SetActive(false);
    IEnumerator e = this.initializeCharacterQuest(id, apiResponse);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator initializeCharacterQuest(
    int id,
    WebAPI.Response.QuestProgressCharacter apiResponse)
  {
    this.mode = Quest00214aMenu.Mode.Character;
    this.CommonInit();
    IEnumerator e1 = ServerTime.WaitSync();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    PlayerQuestSConverter[] Quests = ((IEnumerable<PlayerCharacterQuestS>) SMManager.Get<PlayerCharacterQuestS[]>().SelectReleased()).Where<PlayerCharacterQuestS>((Func<PlayerCharacterQuestS, bool>) (x => x.quest_character_s.unit_UnitUnit == id)).Select<PlayerCharacterQuestS, PlayerQuestSConverter>((Func<PlayerCharacterQuestS, PlayerQuestSConverter>) (y => new PlayerQuestSConverter(y))).ToArray<PlayerQuestSConverter>();
    QuestSConverter[] episodes = ((IEnumerable<QuestCharacterS>) MasterData.QuestCharacterSList).Where<QuestCharacterS>((Func<QuestCharacterS, bool>) (w => w.unit_UnitUnit == id && QuestCharacterS.CheckIsReleased(w.start_at))).Select<QuestCharacterS, QuestSConverter>((Func<QuestCharacterS, QuestSConverter>) (s =>
    {
      WebAPI.Response.QuestProgressCharacterCharacter_quest_s_lost_aps characterQuestSLostAps = Array.Find<WebAPI.Response.QuestProgressCharacterCharacter_quest_s_lost_aps>(apiResponse.character_quest_s_lost_aps, (Predicate<WebAPI.Response.QuestProgressCharacterCharacter_quest_s_lost_aps>) (sd => sd.quest_s_id == s.ID));
      int lostAp = characterQuestSLostAps != null ? characterQuestSLostAps.lost_ap : 0;
      return new QuestSConverter(s, lostAp);
    })).OrderBy<QuestSConverter, int>((Func<QuestSConverter, int>) (x => x.priority)).ToArray<QuestSConverter>();
    this.tblEpisodeQuest_ = ((IEnumerable<QuestSConverter>) episodes).Select<QuestSConverter, Tuple<QuestSConverter, PlayerQuestSConverter>>((Func<QuestSConverter, Tuple<QuestSConverter, PlayerQuestSConverter>>) (e => Tuple.Create<QuestSConverter, PlayerQuestSConverter>(e, Array.Find<PlayerQuestSConverter>(Quests, (Predicate<PlayerQuestSConverter>) (q => q._quest_s_id == e.ID))))).ToArray<Tuple<QuestSConverter, PlayerQuestSConverter>>();
    this.quests_ = ((IEnumerable<Tuple<QuestSConverter, PlayerQuestSConverter>>) this.tblEpisodeQuest_).Where<Tuple<QuestSConverter, PlayerQuestSConverter>>((Func<Tuple<QuestSConverter, PlayerQuestSConverter>, bool>) (tbl => tbl.Item2 != null)).Select<Tuple<QuestSConverter, PlayerQuestSConverter>, PlayerQuestSConverter>((Func<Tuple<QuestSConverter, PlayerQuestSConverter>, PlayerQuestSConverter>) (tbl => tbl.Item2)).ToArray<PlayerQuestSConverter>();
    this.QuestQuantity = episodes.Length;
    this.tblDisplayConditions_ = new List<QuestDisplayConditionConverter>[this.QuestQuantity];
    int initCenter = 0;
    if (this.quests_.Length != 0)
    {
      for (int index = 0; index < this.tblEpisodeQuest_.Length; ++index)
      {
        if (this.tblEpisodeQuest_[index].Item2 != null && !this.tblEpisodeQuest_[index].Item2.is_clear)
        {
          initCenter = index;
          break;
        }
      }
    }
    e1 = this.LoadCharacterSprite(id, this.Character);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.SetGrayQuestIcon(this.quests_.Length, this.Character);
    e1 = this.SetBg(episodes[initCenter], this.tblEpisodeQuest_[initCenter].Item2, initCenter);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.releaseConditionActiveFlag = false;
    this.SetTween();
    this.tblDisplayConditions_[initCenter] = this.getDisplayConditionList(episodes[initCenter].ID);
    e1 = this.InitReleases(this.tblDisplayConditions_[initCenter]);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    e1 = this.InitHscroll(initCenter);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.txtTitle.SetTextLocalize(episodes.Length != 0 ? episodes[0].quest_m.name : "");
  }

  public IEnumerator showPopup(
    int id,
    WebAPI.Response.QuestProgressCharacter apiResponse,
    Action<Action> beforeChangeScene)
  {
    Quest00214aMenu quest00214aMenu = this;
    ((Component) quest00214aMenu).gameObject.SetActive(true);
    quest00214aMenu.isPopup_ = true;
    quest00214aMenu.onBeforeChangeScne_ = beforeChangeScene;
    quest00214aMenu.mask.SetActive(true);
    quest00214aMenu.Character.gameObject.SetActive(true);
    IEnumerator e = quest00214aMenu.initializeCharacterQuest(id, apiResponse);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(
    int unitId,
    int[] targetUnitIds,
    WebAPI.Response.QuestProgressCharacter apiResponse,
    bool isTrio)
  {
    this.mode = Quest00214aMenu.Mode.Harmony;
    this.CommonInit();
    IEnumerator e1 = ServerTime.WaitSync();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    PlayerQuestSConverter[] Quests = ((IEnumerable<PlayerHarmonyQuestS>) SMManager.Get<PlayerHarmonyQuestS[]>().SelectReleased()).Where<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (x => x.quest_harmony_s.unit_UnitUnit == unitId)).Select<PlayerHarmonyQuestS, PlayerQuestSConverter>((Func<PlayerHarmonyQuestS, PlayerQuestSConverter>) (y => new PlayerQuestSConverter(y))).ToArray<PlayerQuestSConverter>();
    QuestSConverter[] episodes = ((IEnumerable<QuestHarmonyS>) MasterData.QuestHarmonySList).Where<QuestHarmonyS>((Func<QuestHarmonyS, bool>) (w => w.unit_UnitUnit == unitId && QuestCharacterS.CheckIsReleased(w.start_at))).Select<QuestHarmonyS, QuestSConverter>((Func<QuestHarmonyS, QuestSConverter>) (s =>
    {
      WebAPI.Response.QuestProgressCharacterHarmony_quest_s_lost_aps harmonyQuestSLostAps = Array.Find<WebAPI.Response.QuestProgressCharacterHarmony_quest_s_lost_aps>(apiResponse.harmony_quest_s_lost_aps, (Predicate<WebAPI.Response.QuestProgressCharacterHarmony_quest_s_lost_aps>) (sd => sd.quest_s_id == s.ID));
      int lostAp = harmonyQuestSLostAps != null ? harmonyQuestSLostAps.lost_ap : 0;
      return new QuestSConverter(s, lostAp);
    })).OrderBy<QuestSConverter, int>((Func<QuestSConverter, int>) (x => x.priority)).ToArray<QuestSConverter>();
    this.tblEpisodeQuest_ = ((IEnumerable<QuestSConverter>) episodes).Select<QuestSConverter, Tuple<QuestSConverter, PlayerQuestSConverter>>((Func<QuestSConverter, Tuple<QuestSConverter, PlayerQuestSConverter>>) (e => Tuple.Create<QuestSConverter, PlayerQuestSConverter>(e, Array.Find<PlayerQuestSConverter>(Quests, (Predicate<PlayerQuestSConverter>) (q => q._quest_s_id == e.ID))))).ToArray<Tuple<QuestSConverter, PlayerQuestSConverter>>();
    this.quests_ = ((IEnumerable<Tuple<QuestSConverter, PlayerQuestSConverter>>) this.tblEpisodeQuest_).Where<Tuple<QuestSConverter, PlayerQuestSConverter>>((Func<Tuple<QuestSConverter, PlayerQuestSConverter>, bool>) (tbl => tbl.Item2 != null)).Select<Tuple<QuestSConverter, PlayerQuestSConverter>, PlayerQuestSConverter>((Func<Tuple<QuestSConverter, PlayerQuestSConverter>, PlayerQuestSConverter>) (tbl => tbl.Item2)).ToArray<PlayerQuestSConverter>();
    this.QuestQuantity = episodes.Length;
    this.tblDisplayConditions_ = new List<QuestDisplayConditionConverter>[this.QuestQuantity];
    int initCenter = 0;
    if (this.quests_.Length != 0)
    {
      for (int index = 0; index < this.tblEpisodeQuest_.Length; ++index)
      {
        if (this.tblEpisodeQuest_[index].Item2 != null && !this.tblEpisodeQuest_[index].Item2.is_clear)
        {
          initCenter = index;
          break;
        }
      }
    }
    if (isTrio && targetUnitIds.Length > 1)
    {
      e1 = this.LoadCharacterStorySprite(unitId, this.CharacterTrioLeft);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      e1 = this.LoadCharacterStorySprite(targetUnitIds[0], this.CharacterTrioCenter);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      e1 = this.LoadCharacterStorySprite(targetUnitIds[1], this.CharacterTrioRight);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      this.Character.gameObject.SetActive(false);
      this.CharacterCombi.gameObject.SetActive(false);
      this.CharacterCombiTarget.gameObject.SetActive(false);
      this.CharacterTrioLeft.gameObject.SetActive(true);
      this.CharacterTrioCenter.gameObject.SetActive(true);
      this.CharacterTrioRight.gameObject.SetActive(true);
      this.SetGrayQuestIcon(this.quests_.Length, this.CharacterTrioLeft);
      this.SetGrayQuestIcon(this.quests_.Length, this.CharacterTrioCenter);
      this.SetGrayQuestIcon(this.quests_.Length, this.CharacterTrioRight);
    }
    else
    {
      e1 = this.LoadCharacterStorySprite(unitId, this.CharacterCombi);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      e1 = this.LoadCharacterStorySprite(targetUnitIds[0], this.CharacterCombiTarget);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      this.Character.gameObject.SetActive(false);
      this.CharacterCombi.gameObject.SetActive(true);
      this.CharacterCombiTarget.gameObject.SetActive(true);
      this.CharacterTrioLeft.gameObject.SetActive(false);
      this.CharacterTrioCenter.gameObject.SetActive(false);
      this.CharacterTrioRight.gameObject.SetActive(false);
      this.SetGrayQuestIcon(this.quests_.Length, this.CharacterCombi);
      this.SetGrayQuestIcon(this.quests_.Length, this.CharacterCombiTarget);
    }
    e1 = this.SetBg(episodes[initCenter], this.tblEpisodeQuest_[initCenter].Item2, initCenter);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.releaseConditionActiveFlag = false;
    this.SetTween();
    this.tblDisplayConditions_[initCenter] = this.getDisplayConditionList(episodes[initCenter].ID);
    e1 = this.InitReleases(this.tblDisplayConditions_[initCenter]);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    e1 = this.InitHscroll(initCenter);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    this.txtTitle.SetTextLocalize(episodes.Length != 0 ? episodes[0].quest_m.name : "");
  }

  public IEnumerator InitReleases(List<QuestDisplayConditionConverter> list)
  {
    if (list.Count > 0)
    {
      IEnumerator e = this.release.GetComponent<Quest00214ReleaseCondition>().InitRelease(list, this.unitIconPrefab, this.releaseConditionPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      this.release.gameObject.SetActive(false);
  }

  private IEnumerator InitHscroll(int iCenter)
  {
    Quest00214aMenu quest00214aMenu = this;
    for (int index = 0; index < quest00214aMenu.QuestQuantity; ++index)
    {
      GameObject gameObject = quest00214aMenu.indicator.instantiateParts(quest00214aMenu.hscrollPrefab);
      quest00214aMenu.StageObjects.Add(gameObject);
      Quest00214Hscroll component = gameObject.GetComponent<Quest00214Hscroll>();
      if (quest00214aMenu.tblEpisodeQuest_[index].Item2 != null)
        component.UnLockCondition();
      else
        component.LockCondition();
      quest00214aMenu.initHscroll(component, quest00214aMenu.tblEpisodeQuest_[index].Item1, quest00214aMenu.quests_, index + 1, quest00214aMenu.grid.cellWidth, iCenter);
    }
    quest00214aMenu.indicator.resetScrollView();
    quest00214aMenu.indicator.setItemPositionQuick(iCenter);
    yield return (object) null;
    quest00214aMenu.ctrlCenter.CenterOn(quest00214aMenu.StageObjects[iCenter].transform);
    quest00214aMenu.compareCenterObj = quest00214aMenu.StageObjects[iCenter];
    foreach (GameObject stageObject in quest00214aMenu.StageObjects)
    {
      Quest00214Hscroll component = stageObject.GetComponent<Quest00214Hscroll>();
      if (Object.op_Inequality((Object) stageObject, (Object) quest00214aMenu.compareCenterObj))
      {
        component.centerAnimation(false);
        component.NotTouch(true);
      }
      else
      {
        PlayerQuestSConverter playerQuestSconverter = quest00214aMenu.tblEpisodeQuest_[iCenter].Item2;
        bool flag = playerQuestSconverter != null && !playerQuestSconverter.is_clear;
        component.centerAnimation(flag);
        component.NotTouch(!flag);
      }
    }
    quest00214aMenu.isResetToneCondition_ = true;
    foreach (GameObject stageObject in quest00214aMenu.StageObjects)
      stageObject.GetComponent<Quest00214Hscroll>().onSetValue();
    quest00214aMenu.Starting();
    quest00214aMenu.ToneChangeStart();
    quest00214aMenu.Update();
  }

  private void initHscroll(
    Quest00214Hscroll hscroll,
    QuestSConverter StageData,
    PlayerQuestSConverter[] charaque,
    int num,
    float gridWidth,
    int center)
  {
    hscroll.onBeforeChangeScene = this.onBeforeChangeScne_;
    hscroll.Init(StageData, charaque, num, gridWidth, center, (EventDelegate.Callback) (() =>
    {
      if (this.IsPushAndSet())
        return;
      this.isChangedQuestDetail_ = true;
      Quest00221Scene.changeDetailScene(this.detailManager.getData(StageData.data_type == QuestSConverter.DataType.Character ? CommonQuestType.Character : CommonQuestType.Harmony, StageData.ID), true);
    }));
  }

  private void OnEnable()
  {
    if (!this.isChangedQuestDetail_)
      return;
    this.isChangedQuestDetail_ = false;
    this.ScrollView.Press(false);
  }

  private void Starting()
  {
    this.IsPush = true;
    this.mask.SetActive(true);
    this.setEffectBackMask(true);
    ((Component) this).GetComponent<UIRect>().alpha = 1f;
    this.isStartTweenFinished = new bool?(true);
    this.countTween = this.StartTweeners.Count;
    foreach (UITweener startTweener in this.StartTweeners)
    {
      this.setEventFinished(startTweener);
      startTweener.ResetToBeginning();
      startTweener.PlayForward();
    }
    foreach (UITweener bothTweener in this.BothTweeners)
    {
      bothTweener.ResetToBeginning();
      bothTweener.PlayForward();
    }
  }

  public void Hide() => this.Ending();

  private void Ending()
  {
    EventDelegate.Execute(this.onClose);
    this.setEffectBackMask(false);
    this.isStartTweenFinished = new bool?(false);
    this.countTween = this.EndTweeners.Count;
    foreach (UITweener endTweener in this.EndTweeners)
    {
      this.setEventFinished(endTweener);
      endTweener.ResetToBeginning();
      endTweener.PlayForward();
    }
    foreach (UITweener bothTweener in this.BothTweeners)
    {
      bothTweener.ResetToBeginning();
      bothTweener.PlayReverse();
    }
    if (this.releaseConditionActiveFlag)
      this.ibtnReleaseCondition();
    for (int index = 0; index < this.cntlOpenClose.Length; ++index)
      this.cntlOpenClose[index].isActive = false;
  }

  private void setEffectBackMask(bool bForward)
  {
    TweenAlpha tweenAlpha = this.isPopup_ ? this.mask.GetComponentInChildren<TweenAlpha>(true) : this.mask.GetComponent<TweenAlpha>();
    if (bForward)
    {
      ((UITweener) tweenAlpha).ResetToBeginning();
      ((UITweener) tweenAlpha).PlayForward();
    }
    else
    {
      ((UITweener) tweenAlpha).ResetToBeginning();
      ((UITweener) tweenAlpha).PlayReverse();
    }
  }

  private void setEventFinished(UITweener tween)
  {
    tween.SetOnFinished((EventDelegate.Callback) (() =>
    {
      this.onTweenFinished();
      tween.onFinished.Clear();
    }));
  }

  private void onTweenFinished()
  {
    if (!this.isStartTweenFinished.HasValue || --this.countTween > 0)
      return;
    if (this.isStartTweenFinished.Value)
    {
      for (int index = 0; index < this.cntlOpenClose.Length; ++index)
        this.cntlOpenClose[index].isActive = true;
      this.indicator.SeEnable = true;
      this.IsPush = false;
    }
    else
    {
      this.mask.SetActive(false);
      ((Component) this).gameObject.SetActive(false);
      EventDelegate.Execute(this.onCloseFinished);
    }
    this.isStartTweenFinished = new bool?();
  }

  public void ToneChangeStart() => this.SceneStart = true;

  public void changeScene(PlayerCharacterQuestS episode)
  {
    Quest0028Scene.changeScene(true, episode);
  }

  public void ibtnReleaseCondition()
  {
    this.releaseConditionActiveFlag = !this.releaseConditionActiveFlag;
    this.release.GetComponent<Quest00214ReleaseCondition>().StartTweenClick(this.releaseConditionActiveFlag);
  }

  public void TweenStart(bool flag)
  {
    if (this.ButtonMove != flag)
    {
      this.container.StartTween(flag);
      if (this.releaseConditionActiveFlag && this.release.activeSelf)
        this.release.GetComponent<Quest00214ReleaseCondition>().StartTween(flag);
    }
    this.ButtonMove = flag;
  }

  private enum Mode
  {
    Character,
    Harmony,
  }

  private enum AnimGroup
  {
    Start = 22, // 0x00000016
    End = 33, // 0x00000021
    StartEnd = 44, // 0x0000002C
  }
}
