// Decompiled with JetBrains decompiler
// Type: CorpsQuestTopMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using LocaleTimeZone;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/TopMenu")]
public class CorpsQuestTopMenu : BackButtonMenuBase
{
  [SerializeField]
  private float CellMinScale = 0.83f;
  [SerializeField]
  private float ScrollStartY = -6.5f;
  [SerializeField]
  private UIScrollView ScrollView;
  [SerializeField]
  private UIGrid Grid;
  [SerializeField]
  private UICenterOnChild CenterOnChild;
  [SerializeField]
  private Transform SelectedFramePosition;
  [SerializeField]
  private GameObject AllowBtnUp;
  [SerializeField]
  private GameObject AllowBtnDown;
  [Space(8f)]
  [SerializeField]
  private UI2DSprite ShopTicketIcon;
  [SerializeField]
  private UILabel LblVictoryConditionValue;
  [SerializeField]
  private UILabel LblRewardValue;
  [SerializeField]
  private UILabel LblRewardAchieved;
  [SerializeField]
  private GameObject DynRewardIconUnit;
  [SerializeField]
  private GameObject DynRewardIconItem;
  [Space(8f)]
  [SerializeField]
  private UIButton BtnBattleChallenge;
  [SerializeField]
  private UITweener TweenBattleChallengeSub;
  [SerializeField]
  private UILabel LblExistenceValue;
  [SerializeField]
  private UIButton BtnRecovery;
  [SerializeField]
  private UITweener TweenRecoverySub;
  [SerializeField]
  private UIButton BtnUnitList;
  [SerializeField]
  private UIButton BtnEnemyInfo;
  [SerializeField]
  private UITweener TweenEnemyInfoSub;
  [SerializeField]
  private UIButton BtnMapInfo;
  [SerializeField]
  private UITweener TweenMapInfoSub;
  [Space(8f)]
  [SerializeField]
  private GameObject DirBackground;
  private GameObject BgObject;
  private CorpsPeriod PeriodMaster;
  private CorpsSetting SettingMaster;
  private CorpsStage[] StageMaster;
  private PlayerCorps Progress;
  private PlayerUnit[] EntryUnits;
  private PlayerUnit[] UnUsedUnits;
  private int[] ClearedStageIDs;
  private int[] ClearedStageNos;
  private int[] ChallengeStageIDs;
  private bool IsClearAllFloor;
  private DateTime SceneEntryTime;
  private CorpsQuestTopMenu.Mode ViewMode;
  private CorpsStageListItem CurrentlListItem;
  private float ScrollDeltaTmp;
  private bool IsScroll;
  private int TopStageID;
  private int BottomStageID;
  private bool OneTimeEventDirty = true;
  private GameObject BgObjPrefab;
  private GameObject MapCheckPrefab;
  private GameObject EntryPopupPrefab;
  private GameObject EnemyListPopupPrefab;
  private GameObject MissionListPrefab;
  private GameObject StageRewardPrefab;
  private GameObject ClearEffectPrefeb;
  [SerializeField]
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  [SerializeField]
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();

  public IEnumerator LoadResources()
  {
    Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_corpsquest_unit_selection__anim_popup01").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.EntryPopupPrefab = f.Result;
    f = (Future<GameObject>) null;
    f = new ResourceObject("Prefabs/CorpsQuest/CorpsQuest_BG/Corps_BG_anim").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.BgObjPrefab = f.Result;
    f = (Future<GameObject>) null;
    f = new ResourceObject("Prefabs/CorpsQuest/CorpsQuest_battle/dir_CorpsClear_Anim").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ClearEffectPrefeb = f.Result;
    f = (Future<GameObject>) null;
  }

  public IEnumerator InitializeAsync(CorpsPeriod period, PlayerCorps progress)
  {
    CorpsQuestTopMenu corpsQuestTopMenu = this;
    corpsQuestTopMenu.PeriodMaster = period;
    corpsQuestTopMenu.SettingMaster = corpsQuestTopMenu.PeriodMaster.setting;
    // ISSUE: reference to a compiler-generated method
    corpsQuestTopMenu.StageMaster = ((IEnumerable<CorpsStage>) MasterData.CorpsStageList).Where<CorpsStage>(new Func<CorpsStage, bool>(corpsQuestTopMenu.\u003CInitializeAsync\u003Eb__56_0)).OrderBy<CorpsStage, int>((Func<CorpsStage, int>) (x => x.number)).ToArray<CorpsStage>();
    corpsQuestTopMenu.Progress = progress;
    corpsQuestTopMenu.ClearedStageIDs = corpsQuestTopMenu.Progress.current_cleared_stage_ids;
    corpsQuestTopMenu.ClearedStageNos = ((IEnumerable<int>) corpsQuestTopMenu.ClearedStageIDs).Select<int, int>((Func<int, int>) (x => MasterData.CorpsStage[x].number)).ToArray<int>();
    corpsQuestTopMenu.ChallengeStageIDs = ((IEnumerable<PlayerCorpsStage>) corpsQuestTopMenu.Progress.corps_stages).Select<PlayerCorpsStage, int>((Func<PlayerCorpsStage, int>) (x => x.stage_id)).ToArray<int>();
    corpsQuestTopMenu.IsClearAllFloor = ((IEnumerable<int>) corpsQuestTopMenu.ClearedStageIDs).Contains<int>(((IEnumerable<CorpsStage>) corpsQuestTopMenu.StageMaster).First<CorpsStage>((Func<CorpsStage, bool>) (x => x.isBoss)).ID);
    IEnumerator e = corpsQuestTopMenu.CreateStageList();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = corpsQuestTopMenu.SetTicketIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = corpsQuestTopMenu.SetBackGround();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void ResetEntryUnits(PlayerCorps progress) => this.Progress = progress;

  public void OnStartScene(bool isAllClearNow)
  {
    this.SceneEntryTime = ServerTime.NowAppTimeAddDelta();
    this.EntryUnits = ((IEnumerable<int>) this.Progress.entry_player_unit_ids).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (x => ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (y => y.id == x)))).ToArray<PlayerUnit>();
    this.UnUsedUnits = ((IEnumerable<PlayerUnit>) this.EntryUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !((IEnumerable<int>) this.Progress.used_player_unit_ids).Contains<int>(x.id))).ToArray<PlayerUnit>();
    this.LblExistenceValue.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_EXISTENCE_UNIT_VALUE, (IDictionary) new Hashtable()
    {
      {
        (object) "unit_num",
        (object) this.UnUsedUnits.Length
      },
      {
        (object) "total_unit_num",
        (object) this.EntryUnits.Length
      }
    }));
    this.UpdateLevelListScale();
    this.UpdateFloorInfo();
    this.UpdateMode();
    if (!this.OneTimeEventDirty)
      return;
    this.ShowOneTimePopup(isAllClearNow);
    this.OneTimeEventDirty = false;
  }

  private IEnumerator ShowUnitSelectionScene(
    CorpsUtil.UnitSelectionMode mode,
    CorpsUtil.SequenceType type)
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    CorpsQuestUnitSelectionScene.ChangeScene(this.Progress, mode, type);
  }

  private IEnumerator CreateStageList()
  {
    this.CurrentlListItem = (CorpsStageListItem) null;
    ((Component) this.CenterOnChild).transform.Clear();
    this.ScrollView.ResetPosition();
    Future<GameObject> f = new ResourceObject("Prefabs/CorpsQuest/CorpsQuest_Stage_btn/dir_CorpsQuest_Stage_btn_1").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = f.Result;
    AssocList<int, CorpsStageOpenConditions> stageOpenConditions = MasterData.CorpsStageOpenConditions;
    List<CorpsStageListItem> source = new List<CorpsStageListItem>();
    for (int index = 0; index < this.StageMaster.Length; ++index)
    {
      CorpsStage stage = this.StageMaster[index];
      if (stage.CheckOpen(this.ClearedStageNos))
      {
        CorpsStageListItem component = result.CloneAndGetComponent<CorpsStageListItem>(((Component) this.CenterOnChild).gameObject);
        bool isRewardGotten = ((IEnumerable<CorpsStageClearReward>) MasterData.CorpsStageClearRewardList).Any<CorpsStageClearReward>((Func<CorpsStageClearReward, bool>) (x => x.stage_id == stage.ID)) && ((IEnumerable<int>) this.Progress.cleared_stage_ids).Contains<int>(stage.ID);
        component.Init(stage, ((IEnumerable<int>) this.ClearedStageIDs).Contains<int>(stage.ID), isRewardGotten, (Action<GameObject>) (obj => this.CenterOnChild.CenterOn(obj.transform)));
        source.Add(component);
      }
    }
    List<CorpsStageListItem> list = source.OrderBy<CorpsStageListItem, bool>((Func<CorpsStageListItem, bool>) (x => x.IsCleared)).ThenBy<CorpsStageListItem, int>((Func<CorpsStageListItem, int>) (x => x.Floor)).ToList<CorpsStageListItem>();
    for (int index = 0; index < list.Count; ++index)
      ((Component) list[index]).transform.localPosition = new Vector3(0.0f, -1f * this.Grid.cellHeight * (float) index, 0.0f);
    this.TopStageID = list[0].StageID;
    this.BottomStageID = list[list.Count - 1].StageID;
    this.CenterOnChild.CenterOn(((Component) list[0]).transform);
  }

  private IEnumerator SetTicketIcon()
  {
    CommonTicket commonTicket;
    if (MasterData.CommonTicket.TryGetValue(this.PeriodMaster.trade_coin_id, out commonTicket))
    {
      string path = string.Format("Coin/{0}/Coin_S", (object) (commonTicket.icon_id != 0 ? commonTicket.icon_id : commonTicket.ID));
      Future<Sprite> future = Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ShopTicketIcon.sprite2D = future.Result;
      ((Component) this.ShopTicketIcon).gameObject.SetActive(true);
      future = (Future<Sprite>) null;
    }
    else
      ((Component) this.ShopTicketIcon).gameObject.SetActive(false);
  }

  private IEnumerator SetBackGround()
  {
    if (!Object.op_Inequality((Object) this.BgObject, (Object) null))
    {
      this.BgObject = this.BgObjPrefab.Clone(this.DirBackground.transform);
      string backgroundFile = this.SettingMaster.background_file;
      if (!string.IsNullOrEmpty(backgroundFile))
      {
        Future<Sprite> f = new ResourceObject("Prefabs/BackGround/" + backgroundFile).Load<Sprite>();
        IEnumerator e = f.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.BgObject.GetComponentInChildren<UI2DSprite>().sprite2D = f.Result;
        f = (Future<Sprite>) null;
      }
    }
  }

  private IEnumerator CreateClearBonusIcon()
  {
    this.DynRewardIconUnit.transform.Clear();
    this.DynRewardIconUnit.SetActive(false);
    this.DynRewardIconItem.transform.Clear();
    this.DynRewardIconItem.SetActive(false);
    while (this.IsScroll)
      yield return (object) null;
    int stage_id = this.CurrentlListItem.StageID;
    CorpsStageClearReward stageClearReward = Array.Find<CorpsStageClearReward>(MasterData.CorpsStageClearRewardList, (Predicate<CorpsStageClearReward>) (x => x.stage_id == stage_id));
    if (stageClearReward != null)
    {
      GameObject iconBase = (GameObject) null;
      switch (stageClearReward.entity_type)
      {
        case MasterDataTable.CommonRewardType.unit:
        case MasterDataTable.CommonRewardType.material_unit:
          iconBase = this.DynRewardIconUnit;
          break;
        default:
          iconBase = this.DynRewardIconItem;
          break;
      }
      IEnumerator e = iconBase.GetOrAddComponent<CreateIconObject>().CreateThumbnail(stageClearReward.entity_type, stageClearReward.reward_id, isButton: false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
      iconBase.SetActive(true);
      iconBase = (GameObject) null;
    }
  }

  private void UpdateLevelListScale()
  {
    float num1 = ((Component) this.ScrollView).transform.localPosition.y - this.ScrollStartY;
    Vector3 localPosition = this.SelectedFramePosition.localPosition;
    CorpsStageListItem corpsStageListItem = (CorpsStageListItem) null;
    float num2 = (float) int.MaxValue;
    for (int index = 0; index < ((Component) this.Grid).transform.childCount; ++index)
    {
      Transform child = ((Component) this.Grid).transform.GetChild(index);
      float num3 = Mathf.Abs(child.localPosition.y + num1);
      if ((double) num3 < (double) num2)
      {
        num2 = num3;
        corpsStageListItem = ((Component) child).gameObject.GetComponent<CorpsStageListItem>();
      }
      float num4 = (double) num3 < (double) this.Grid.cellHeight ? (float) (1.0 - (1.0 - (double) this.CellMinScale) * ((double) num3 / (double) this.Grid.cellHeight)) : this.CellMinScale;
      child.localScale = new Vector3(num4, num4, 0.0f);
    }
    if (!Object.op_Inequality((Object) corpsStageListItem, (Object) null) || !Object.op_Inequality((Object) this.CurrentlListItem, (Object) corpsStageListItem))
      return;
    this.CurrentlListItem = corpsStageListItem;
    this.AllowBtnUp.SetActive(this.CurrentlListItem.StageID != this.TopStageID);
    this.AllowBtnDown.SetActive(this.CurrentlListItem.StageID != this.BottomStageID);
    if (this.ViewMode == CorpsQuestTopMenu.Mode.Normal)
      this.SetChallengeButtonEnable(!this.CurrentlListItem.IsCleared);
    ((Component) this.LblRewardAchieved).gameObject.SetActive(this.CurrentlListItem.IsRewardGotten);
    this.StopCoroutine("CreateClearBonusIcon");
    this.StartCoroutine("CreateClearBonusIcon");
  }

  private void UpdateFloorInfo()
  {
    if (this.Progress == null)
      return;
    Vector3 localPosition = ((Component) this.ScrollView).transform.localPosition;
    double scrollStartY = (double) this.ScrollStartY;
    CorpsStage corpsStage = ((IEnumerable<CorpsStage>) this.StageMaster).FirstOrDefault<CorpsStage>((Func<CorpsStage, bool>) (x => x.ID == this.CurrentlListItem.StageID));
    if (corpsStage == null)
      return;
    int stage_id = corpsStage.ID;
    string subName = MasterData.BattleStage[stage_id].victory_condition.sub_name;
    if (!string.IsNullOrEmpty(subName))
      this.LblVictoryConditionValue.SetTextLocalize(subName);
    else
      this.LblVictoryConditionValue.SetTextLocalize(Consts.GetInstance().TOWER_VICTORY_CONDITION_DEFAULT);
    CorpsStageClearReward stageClearReward = ((IEnumerable<CorpsStageClearReward>) MasterData.CorpsStageClearRewardList).FirstOrDefault<CorpsStageClearReward>((Func<CorpsStageClearReward, bool>) (x => x.stage_id == stage_id));
    if (stageClearReward != null)
      this.LblRewardValue.SetTextLocalize(stageClearReward.reward_name);
    else
      this.LblRewardValue.SetTextLocalize("なし");
  }

  private CorpsQuestEntryPopup ShowEntryPopup(bool entry)
  {
    CorpsQuestEntryPopup component = Singleton<PopupManager>.GetInstance().open(this.EntryPopupPrefab).GetComponent<CorpsQuestEntryPopup>();
    if (entry)
      component.InitializeEntry();
    else
      component.InitializeRetry();
    return component;
  }

  private IEnumerator ShowEnemyListPopup()
  {
    CorpsQuestTopMenu corpsQuestTopMenu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    yield return (object) null;
    IEnumerator e;
    if (Object.op_Equality((Object) corpsQuestTopMenu.EnemyListPopupPrefab, (Object) null))
    {
      Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_corpsquest_enemy_status__anim_popup01").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      corpsQuestTopMenu.EnemyListPopupPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    CorpsQuestEnemyListPopup popup = Singleton<PopupManager>.GetInstance().open(corpsQuestTopMenu.EnemyListPopupPrefab, isNonSe: true, isNonOpenAnime: true).GetComponent<CorpsQuestEnemyListPopup>();
    int stageId = corpsQuestTopMenu.CurrentlListItem.StageID;
    bool isCleared = corpsQuestTopMenu.CurrentlListItem.IsCleared;
    PlayerCorpsStage playerCorpsStage = ((IEnumerable<PlayerCorpsStage>) corpsQuestTopMenu.Progress.corps_stages).FirstOrDefault<PlayerCorpsStage>((Func<PlayerCorpsStage, bool>) (x => x.stage_id == stageId));
    CorpsEnemyStatus[] enemyInfos = playerCorpsStage == null || playerCorpsStage.is_first ? new CorpsEnemyStatus[0] : playerCorpsStage.enemies;
    e = popup.InitializeAsync(stageId, isCleared, enemyInfos);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<PopupManager>.GetInstance().startOpenAnime(((Component) popup).gameObject);
    corpsQuestTopMenu.IsPush = false;
  }

  private IEnumerator ShowMapCheckPopup()
  {
    CorpsQuestTopMenu corpsQuestTopMenu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    yield return (object) null;
    IEnumerator e;
    if (Object.op_Equality((Object) corpsQuestTopMenu.MapCheckPrefab, (Object) null))
    {
      Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_029_tower_stage_status__anim_popup01").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      corpsQuestTopMenu.MapCheckPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    GameObject popup = corpsQuestTopMenu.MapCheckPrefab.Clone();
    Tower029MapcheckPopup component = popup.GetComponent<Tower029MapcheckPopup>();
    popup.SetActive(false);
    e = component.InitializeAsync(corpsQuestTopMenu.CurrentlListItem.StageID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    corpsQuestTopMenu.IsPush = false;
  }

  private IEnumerator ShowMissionListPopup()
  {
    CorpsQuestTopMenu corpsQuestTopMenu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    yield return (object) null;
    IEnumerator e;
    if (Object.op_Equality((Object) corpsQuestTopMenu.MissionListPrefab, (Object) null))
    {
      Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_battle_mission_list_popup").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      corpsQuestTopMenu.MissionListPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    PopupBattleMissionList popup = corpsQuestTopMenu.MissionListPrefab.CloneAndGetComponent<PopupBattleMissionList>();
    e = popup.InitializeCorps(corpsQuestTopMenu.Progress.corps_id, corpsQuestTopMenu.Progress.cleared_mission_ids);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) popup).gameObject.SetActive(false);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<PopupManager>.GetInstance().open(((Component) popup).gameObject, isCloned: true);
    ((Component) popup).gameObject.SetActive(true);
    corpsQuestTopMenu.IsPush = false;
  }

  private IEnumerator ShowStageRewardPopup()
  {
    CorpsQuestTopMenu corpsQuestTopMenu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    yield return (object) null;
    IEnumerator e;
    if (Object.op_Equality((Object) corpsQuestTopMenu.StageRewardPrefab, (Object) null))
    {
      Future<GameObject> f = new ResourceObject("Prefabs/CorpsQuest/popup_ClearRewardBox_anim").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      corpsQuestTopMenu.StageRewardPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    CorpsQuestStageRewardPopup popup = corpsQuestTopMenu.StageRewardPrefab.CloneAndGetComponent<CorpsQuestStageRewardPopup>();
    e = popup.Initialize(corpsQuestTopMenu.CurrentlListItem.StageID, corpsQuestTopMenu.CurrentlListItem.IsRewardGotten);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) popup).gameObject.SetActive(false);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<PopupManager>.GetInstance().open(((Component) popup).gameObject, isCloned: true);
    ((Component) popup).gameObject.SetActive(true);
    corpsQuestTopMenu.IsPush = false;
  }

  private void UpdateMode()
  {
    if (this.SceneEntryTime >= TimeZoneInfo.ConvertTime(this.PeriodMaster.end_at, Japan.CreateTimeZone(), TimeZoneInfo.Local))
      this.SetMode(CorpsQuestTopMenu.Mode.TimeOver);
    else if (this.IsClearAllFloor)
      this.SetMode(CorpsQuestTopMenu.Mode.Complete);
    else if (!((IEnumerable<PlayerUnit>) this.EntryUnits).Any<PlayerUnit>())
    {
      if (((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Any<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.level >= this.SettingMaster.min_unit_level)))
      {
        if (((IEnumerable<PlayerCorpsStage>) this.Progress.corps_stages).Any<PlayerCorpsStage>())
          this.SetMode(CorpsQuestTopMenu.Mode.Annihilation);
        else
          this.SetMode(CorpsQuestTopMenu.Mode.NoUnit);
      }
      else
        this.SetMode(CorpsQuestTopMenu.Mode.Restriction);
    }
    else if (((IEnumerable<PlayerUnit>) this.UnUsedUnits).Any<PlayerUnit>())
      this.SetMode(CorpsQuestTopMenu.Mode.Normal);
    else
      this.SetMode(CorpsQuestTopMenu.Mode.Annihilation);
  }

  private void ShowOneTimePopup(bool isAllClearNow)
  {
    if (isAllClearNow)
    {
      Singleton<PopupManager>.GetInstance().open(this.ClearEffectPrefeb);
    }
    else
    {
      if (this.IsClearAllFloor)
        return;
      switch (this.ViewMode)
      {
        case CorpsQuestTopMenu.Mode.Restriction:
          ModalWindow.Show(Consts.GetInstance().TOWER_MODAL_WARNING_NO_UNIT_TITLE, Consts.Format(Consts.GetInstance().TOWER_MODAL_WARNING_NO_UNIT_DESC, (IDictionary) new Hashtable()
          {
            {
              (object) "level",
              (object) this.SettingMaster.min_unit_level
            }
          }), (Action) (() => { }));
          break;
        case CorpsQuestTopMenu.Mode.NoUnit:
          this.StartCoroutine(this.EntryProc());
          break;
        case CorpsQuestTopMenu.Mode.Annihilation:
          ModalWindow.Show(Consts.GetInstance().CORPS_MODAL_UNIT_ANNIHILATION_TITLE, Consts.GetInstance().CORPS_MODAL_UNIT_ANNIHILATION_DESC, (Action) (() => { }));
          break;
      }
    }
  }

  private void SetMode(CorpsQuestTopMenu.Mode mode)
  {
    this.ViewMode = mode;
    switch (this.ViewMode)
    {
      case CorpsQuestTopMenu.Mode.Normal:
        ((UIButtonColor) this.BtnUnitList).isEnabled = true;
        this.SetRecoveryButtonEnable(true);
        this.SetChallengeButtonEnable(Object.op_Inequality((Object) this.CurrentlListItem, (Object) null) && !this.CurrentlListItem.IsCleared);
        this.SetStageInfoButtonEnable(true);
        break;
      case CorpsQuestTopMenu.Mode.TimeOver:
      case CorpsQuestTopMenu.Mode.Restriction:
        ((UIButtonColor) this.BtnUnitList).isEnabled = false;
        this.SetRecoveryButtonEnable(false);
        this.SetChallengeButtonEnable(false);
        this.SetStageInfoButtonEnable(false);
        break;
      case CorpsQuestTopMenu.Mode.NoUnit:
        ((UIButtonColor) this.BtnUnitList).isEnabled = false;
        this.SetRecoveryButtonEnable(false);
        this.SetChallengeButtonEnable(true);
        this.SetStageInfoButtonEnable(true);
        break;
      case CorpsQuestTopMenu.Mode.Complete:
        ((UIButtonColor) this.BtnUnitList).isEnabled = false;
        this.SetRecoveryButtonEnable(true);
        this.SetChallengeButtonEnable(false);
        this.SetStageInfoButtonEnable(true);
        break;
      case CorpsQuestTopMenu.Mode.Annihilation:
        ((UIButtonColor) this.BtnUnitList).isEnabled = true;
        this.SetRecoveryButtonEnable(true);
        this.SetChallengeButtonEnable(false);
        this.SetStageInfoButtonEnable(true);
        break;
    }
  }

  private void SetChallengeButtonEnable(bool enable)
  {
    if (enable)
    {
      ((UIButtonColor) this.BtnBattleChallenge).isEnabled = true;
      this.TweenBattleChallengeSub.PlayReverse();
    }
    else
    {
      ((UIButtonColor) this.BtnBattleChallenge).isEnabled = false;
      this.TweenBattleChallengeSub.PlayForward();
    }
  }

  private void SetRecoveryButtonEnable(bool enable)
  {
    if (enable)
    {
      ((UIButtonColor) this.BtnRecovery).isEnabled = true;
      this.TweenRecoverySub.PlayReverse();
    }
    else
    {
      ((UIButtonColor) this.BtnRecovery).isEnabled = false;
      this.TweenRecoverySub.PlayForward();
    }
  }

  private void SetStageInfoButtonEnable(bool enable)
  {
    ((UIButtonColor) this.BtnEnemyInfo).isEnabled = enable;
    ((UIButtonColor) this.BtnMapInfo).isEnabled = enable;
    if (enable)
    {
      this.TweenEnemyInfoSub.PlayReverse();
      this.TweenMapInfoSub.PlayReverse();
    }
    else
    {
      this.TweenEnemyInfoSub.PlayForward();
      this.TweenMapInfoSub.PlayForward();
    }
  }

  private IEnumerator EntryProc()
  {
    CorpsQuestTopMenu corpsQuestTopMenu = this;
    CorpsUtil.SequenceType type = CorpsUtil.SequenceType.Start;
    CorpsQuestEntryPopup entryPopup = corpsQuestTopMenu.ShowEntryPopup(true);
    while (entryPopup.Selected == CorpsQuestEntryPopup.Select.None)
      yield return (object) null;
    switch (entryPopup.Selected)
    {
      case CorpsQuestEntryPopup.Select.Manual:
        CorpsQuestUnitSelectionScene.ChangeScene(corpsQuestTopMenu.Progress, CorpsUtil.UnitSelectionMode.Manual, type);
        break;
      case CorpsQuestEntryPopup.Select.Auto:
        CorpsQuestUnitSelectionScene.ChangeScene(corpsQuestTopMenu.Progress, CorpsUtil.UnitSelectionMode.Auto, type);
        break;
      default:
        corpsQuestTopMenu.IsPush = false;
        break;
    }
  }

  private IEnumerator MemberResetProc()
  {
    CorpsQuestTopMenu corpsQuestTopMenu = this;
    bool wait = true;
    bool cancel = false;
    PopupCommonNoYes.Show(Consts.GetInstance().CORPS_POPUP_PROGRESS_RESET_TITLE, Consts.GetInstance().CORPS_POPUP_PROGRESS_RESET_MESSAGE, (Action) (() => wait = false), (Action) (() =>
    {
      cancel = true;
      wait = false;
    }));
    while (wait)
      yield return (object) null;
    if (cancel)
    {
      corpsQuestTopMenu.IsPush = false;
    }
    else
    {
      CorpsUtil.SequenceType type = CorpsUtil.SequenceType.Reset;
      CorpsQuestEntryPopup entryPopup = corpsQuestTopMenu.ShowEntryPopup(false);
      while (entryPopup.Selected == CorpsQuestEntryPopup.Select.None)
        yield return (object) null;
      switch (entryPopup.Selected)
      {
        case CorpsQuestEntryPopup.Select.Manual:
          CorpsQuestUnitSelectionScene.ChangeScene(corpsQuestTopMenu.Progress, CorpsUtil.UnitSelectionMode.Manual, type);
          break;
        case CorpsQuestEntryPopup.Select.Auto:
          CorpsQuestUnitSelectionScene.ChangeScene(corpsQuestTopMenu.Progress, CorpsUtil.UnitSelectionMode.Auto, type);
          break;
        case CorpsQuestEntryPopup.Select.Continue:
          PlayerItem[] array = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.isSupply())).ToArray<PlayerItem>();
          corpsQuestTopMenu.SupplyItems = ((IEnumerable<SupplyItem>) SupplyItem.Merge(array.AllSupplies(), corpsQuestTopMenu.Progress.supplies)).ToList<SupplyItem>();
          corpsQuestTopMenu.SaveDeck = corpsQuestTopMenu.SupplyItems.Copy();
          Quest00210aScene.ChangeScene(true, new Quest00210Menu.Param()
          {
            SupplyItems = corpsQuestTopMenu.SupplyItems,
            SaveDeck = corpsQuestTopMenu.SaveDeck,
            removeButton = false,
            limitedOnly = true,
            mode = Quest00210Scene.Mode.Corps
          }, ((IEnumerable<PlayerUnit>) corpsQuestTopMenu.EntryUnits).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id)).ToArray<int>(), corpsQuestTopMenu.Progress, type);
          break;
        default:
          corpsQuestTopMenu.IsPush = false;
          break;
      }
    }
  }

  protected override void Update()
  {
    base.Update();
    float num = ((Component) this.ScrollView).transform.localPosition.y - this.ScrollStartY;
    this.IsScroll = (double) num != (double) this.ScrollDeltaTmp;
    this.ScrollDeltaTmp = num;
    if (!this.IsScroll)
      return;
    this.UpdateLevelListScale();
    this.UpdateFloorInfo();
  }

  public void onChallengeButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    if (this.ViewMode == CorpsQuestTopMenu.Mode.Normal)
    {
      CorpsQuestBattleEntryScene.ChangeScene(this.PeriodMaster.ID, this.CurrentlListItem.StageID);
    }
    else
    {
      if (this.ViewMode != CorpsQuestTopMenu.Mode.NoUnit)
        return;
      this.StartCoroutine(this.EntryProc());
    }
  }

  public void onResetButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.MemberResetProc());
  }

  public void onEnemyInfoButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowEnemyListPopup());
  }

  public void onStageInfoButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowMapCheckPopup());
  }

  public void onMissionListButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowMissionListPopup());
  }

  public void onStageRewardButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    if (((IEnumerable<CorpsStageClearReward>) MasterData.CorpsStageClearRewardList).Count<CorpsStageClearReward>((Func<CorpsStageClearReward, bool>) (x => x.stage_id == this.CurrentlListItem.StageID)) > 1)
      this.StartCoroutine(this.ShowStageRewardPopup());
    else
      this.IsPush = false;
  }

  public void onUnitInfoButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    CorpsQuestUnitListScene.ChangeScene(this.Progress.period_id);
  }

  public void onHowtoplayButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    CorpsQuestManualScene.ChangeScene(this.SettingMaster);
  }

  public void onShopButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.StartChangeShopScene());
  }

  private IEnumerator StartChangeShopScene()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<WebAPI.Response.ShopStatus> api = WebAPI.ShopStatus();
    IEnumerator e = api.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (api.Result == null)
      MypageScene.ChangeSceneOnError();
    else
      ShopCoinExchangeScene.changeScene(this.PeriodMaster.trade_coin_id);
  }

  public override void onBackButton()
  {
    if (this.IsScroll || this.IsPushAndSet())
      return;
    MypageScene.ChangeScene();
  }

  private enum Direction
  {
    Down = -1, // 0xFFFFFFFF
    Up = 1,
  }

  private enum Mode
  {
    Normal = 1,
    TimeOver = 2,
    Restriction = 3,
    NoUnit = 4,
    Complete = 5,
    Annihilation = 6,
  }
}
