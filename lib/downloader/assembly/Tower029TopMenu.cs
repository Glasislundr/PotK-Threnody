// Decompiled with JetBrains decompiler
// Type: Tower029TopMenu
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
public class Tower029TopMenu : BackButtonMenuBase
{
  private const int Width = 268;
  private const int Height = 72;
  private const int ColumnValue = 1;
  private const int RowValue = 14;
  private const int ScreenValue = 10;
  private const float ScrollWheelFactor = 0.25f;
  public float cellMinScale = 0.83f;
  private GameObject recoveryByStonePopup;
  private GameObject unitSelectionStartPopup;
  private GameObject unitSelectionPopup;
  private GameObject restartUnitSelectionPopup;
  private GameObject restartPopup;
  private GameObject floorListPrefab;
  private GameObject mapCheckPrefab;
  private GameObject bgPrefab;
  private bool isInit;
  private bool isClearAllFloor;
  private bool isEndScene;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UILabel lblVictoryCondition;
  [SerializeField]
  private UILabel lblVictoryConditionValue;
  [SerializeField]
  private UILabel lblReward;
  [SerializeField]
  private UILabel lblRewardValue;
  [SerializeField]
  private UILabel lblRewardAchieved;
  [SerializeField]
  private GameObject dir_bg;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private UICenterOnChild centerOnChild;
  [SerializeField]
  private GameObject slc_tower_level_selected_frame;
  [SerializeField]
  private GameObject btnBattleChallenge;
  [SerializeField]
  private GameObject btnRestart;
  [SerializeField]
  private SpreadColorButton btnRecovery;
  [SerializeField]
  private UIButton btnUnitList;
  [SerializeField]
  private GameObject dir_recovery_time;
  [SerializeField]
  private UILabel lblRecovery;
  [SerializeField]
  private UILabel lblRecoveryTime;
  [SerializeField]
  private UILabel lblFreeRecovery;
  [SerializeField]
  private UILabel lblExistenceUnit;
  [SerializeField]
  private UILabel lblExistenceValue;
  [SerializeField]
  private GameObject allowBtnUp;
  [SerializeField]
  private GameObject allowBtnDown;
  [SerializeField]
  private UI2DSprite slc_icon_kiseki;
  [SerializeField]
  private UI2DSprite slc_icon_tower_medal;
  [SerializeField]
  private GameObject slc_Badge_bikkuri_shop;
  [SerializeField]
  private GameObject slc_Badge_bikkuri_ranking;
  [SerializeField]
  private GameObject dyn_reward_icon_unit;
  [SerializeField]
  private GameObject dyn_reward_icon_item;
  [SerializeField]
  private GameObject btn_ranking;
  private TowerProgress progress;
  private float bgHeight;
  private GameObject bgObject;
  private float scroll_start_y;
  private bool isScroll;
  private float scroll_delta_tmp;
  private int floorNum;
  private int currentFloor;
  private int towerID = -1;
  private GameObject enemyListPopup;
  private TimeSpan recovery_span;
  private int requiredCoinNum;
  private Tower029TopMenu.Mode mode;
  private DateTime now;
  private Tower029TopMenu.Direction direction;
  private TowerLevelList currentTowerLevelList;
  private bool isCreatingBonusIcon;
  private PlayerUnit[] towerEntryUnit;
  private int maxScrollHeight;
  private int minScrollHeight;

  private IEnumerator ResourceLoad(int towerID)
  {
    Future<GameObject> f;
    IEnumerator e;
    if (Object.op_Equality((Object) this.recoveryByStonePopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_unit_life_recovery__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.recoveryByStonePopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.unitSelectionPopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_unit_selection_recovery__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitSelectionPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.restartUnitSelectionPopup, (Object) null))
    {
      f = new ResourceObject("Prefabs/popup/popup_029_tower_unit_restart_selection_recovery__anim_popup01").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.restartUnitSelectionPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.floorListPrefab, (Object) null))
    {
      f = new ResourceObject(string.Format("Prefabs/tower029_top/tower_level_list/dir_tower_level_list_{0}", (object) towerID)).Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.floorListPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.bgPrefab, (Object) null))
    {
      f = new ResourceObject(string.Format("Prefabs/tower029_top/tower_bg/dir_tower_bg_{0}", (object) towerID)).Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.bgPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.unitSelectionStartPopup, (Object) null))
    {
      f = Res.Prefabs.popup.popup_029_tower_unit_selection_start__anim_popup01.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.unitSelectionStartPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.restartPopup, (Object) null))
    {
      f = new ResourceObject("Prefabs/popup/popup_029_tower_restart__anim_popup01").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.restartPopup = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  private IEnumerator ShowUnitSelectionScene(
    TowerUtil.UnitSelectionMode mode,
    TowerUtil.SequenceType type)
  {
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    Tower029UnitSelectionScene.ChangeScene(mode, this.progress, type);
  }

  private void SetFloor(int maxFloor, int currentFloor)
  {
    MasterDataTable.TowerPeriod towerPeriod = ((IEnumerable<MasterDataTable.TowerPeriod>) MasterData.TowerPeriodList).FirstOrDefault<MasterDataTable.TowerPeriod>((Func<MasterDataTable.TowerPeriod, bool>) (x => x.ID == this.progress.period_id));
    this.direction = towerPeriod.direction == 0 ? Tower029TopMenu.Direction.Up : Tower029TopMenu.Direction.Down;
    if (((Component) this.centerOnChild).transform.childCount > 0)
    {
      for (int index = 0; index < ((Component) this.centerOnChild).transform.childCount; ++index)
        ((Component) ((Component) this.centerOnChild).transform.GetChild(index)).GetComponent<TowerLevelList>().Init(this.isClearAllFloor || this.progress.floor > index + 1, this.progress.floor < index + 1, index + 1, (GameObject) null, (Action<GameObject>) (obj => this.centerOnChild.CenterOn(obj.transform)), towerPeriod?.floor_id);
      this.centerOnChild.CenterOn(((Component) ((Component) this.centerOnChild).transform.GetChild(currentFloor - 1)).transform);
    }
    else
    {
      for (int index = 0; index < maxFloor; ++index)
      {
        if (index % 10 == 0)
          this.floorListPrefab.GetComponent<TowerLevelList>().spriteDirect.SetSpriteName<int>(index / 10 + 1, false);
        GameObject gameObject = this.floorListPrefab.Clone(((Component) this.centerOnChild).transform);
        gameObject.GetComponent<TowerLevelList>().Init(this.isClearAllFloor || this.progress.floor > index + 1, this.progress.floor < index + 1, index + 1, (GameObject) null, (Action<GameObject>) (obj => this.centerOnChild.CenterOn(obj.transform)), towerPeriod?.floor_id);
        gameObject.transform.localPosition = new Vector3(0.0f, (float) this.direction * this.grid.cellHeight * (float) index, 0.0f);
      }
      this.scroll_start_y = ((Component) this.scrollView).transform.localPosition.y;
      if (this.direction == Tower029TopMenu.Direction.Up)
      {
        this.maxScrollHeight = 0;
        this.minScrollHeight = -((int) this.grid.cellHeight * (maxFloor - 1));
      }
      else
      {
        this.maxScrollHeight = (int) this.grid.cellHeight * (maxFloor - 1);
        this.minScrollHeight = 0;
      }
      this.centerOnChild.CenterOn(((Component) ((Component) this.centerOnChild).transform.GetChild(currentFloor - 1)).transform);
    }
  }

  private void SetBackGround()
  {
    if (Object.op_Inequality((Object) this.bgObject, (Object) null))
      return;
    this.bgObject = this.bgPrefab.Clone(this.dir_bg.transform);
    UIWidget component = this.bgObject.GetComponent<UIWidget>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    this.bgHeight = (float) component.height;
  }

  private void onFinished()
  {
    if (this.isCreatingBonusIcon)
    {
      this.StopCoroutine("CreateClearBonusIcon");
      this.isCreatingBonusIcon = false;
    }
    this.StartCoroutine("CreateClearBonusIcon");
  }

  private IEnumerator CreateClearBonusIcon()
  {
    this.isCreatingBonusIcon = true;
    for (int index = 0; index < this.dyn_reward_icon_unit.transform.childCount; ++index)
      Object.Destroy((Object) ((Component) this.dyn_reward_icon_unit.transform.GetChild(0)).gameObject);
    this.dyn_reward_icon_unit.SetActive(false);
    for (int index = 0; index < this.dyn_reward_icon_item.transform.childCount; ++index)
      Object.Destroy((Object) ((Component) this.dyn_reward_icon_item.transform.GetChild(0)).gameObject);
    this.dyn_reward_icon_item.SetActive(false);
    while (this.isScroll)
      yield return (object) null;
    int stage_id = -1;
    TowerStage towerStage = ((IEnumerable<TowerStage>) MasterData.TowerStageList).Where<TowerStage>((Func<TowerStage, bool>) (s => s.tower_id == this.progress.tower_id && s.floor == this.currentFloor)).FirstOrDefault<TowerStage>();
    if (towerStage != null)
      stage_id = towerStage.stage_id;
    if (stage_id != -1)
    {
      TowerBattleStageClear battleStageClear = ((IEnumerable<TowerBattleStageClear>) MasterData.TowerBattleStageClearList).Where<TowerBattleStageClear>((Func<TowerBattleStageClear, bool>) (x => x.stage_id == stage_id && x.lap == this.progress.completed_count)).FirstOrDefault<TowerBattleStageClear>();
      if (battleStageClear != null)
      {
        GameObject iconBase = (GameObject) null;
        iconBase = battleStageClear.entity_type == MasterDataTable.CommonRewardType.unit || battleStageClear.entity_type == MasterDataTable.CommonRewardType.material_unit ? this.dyn_reward_icon_unit : this.dyn_reward_icon_item;
        IEnumerator e = iconBase.GetOrAddComponent<CreateIconObject>().CreateThumbnail(battleStageClear.entity_type, battleStageClear.reward_id, isButton: false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) null;
        iconBase.SetActive(true);
        iconBase = (GameObject) null;
      }
      this.isCreatingBonusIcon = false;
    }
  }

  private void UpdateLevelListScale()
  {
    float num1 = ((Component) this.scrollView).transform.localPosition.y - this.scroll_start_y;
    Vector3 localPosition = this.slc_tower_level_selected_frame.transform.localPosition;
    TowerLevelList towerLevelList = (TowerLevelList) null;
    float num2 = (float) int.MaxValue;
    for (int index = 0; index < ((Component) this.grid).transform.childCount; ++index)
    {
      Transform child = ((Component) this.grid).transform.GetChild(index);
      float num3 = Mathf.Abs(child.localPosition.y + num1);
      if ((double) num3 < (double) num2)
      {
        num2 = num3;
        towerLevelList = ((Component) child).gameObject.GetComponent<TowerLevelList>();
      }
      float num4 = (double) num3 < (double) this.grid.cellHeight ? (float) (1.0 - (1.0 - (double) this.cellMinScale) * ((double) num3 / (double) this.grid.cellHeight)) : this.cellMinScale;
      child.localScale = new Vector3(num4, num4, 0.0f);
    }
    if (!Object.op_Inequality((Object) towerLevelList, (Object) null) || !Object.op_Inequality((Object) this.currentTowerLevelList, (Object) towerLevelList))
      return;
    this.currentTowerLevelList = towerLevelList;
    this.currentFloor = towerLevelList.floorNum;
    this.allowBtnUp.SetActive(this.currentFloor != (this.direction == Tower029TopMenu.Direction.Up ? this.floorNum : 1));
    this.allowBtnDown.SetActive(this.currentFloor != (this.direction == Tower029TopMenu.Direction.Up ? 1 : this.floorNum));
    if (this.mode == Tower029TopMenu.Mode.Normal)
    {
      if (towerLevelList.isClear || towerLevelList.isLocked)
        ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = false;
      else
        ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = true;
    }
    else if (this.mode == Tower029TopMenu.Mode.NoUnit)
      ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = towerLevelList.floorNum == this.progress.floor;
    ((Component) this.lblRewardAchieved).gameObject.SetActive(towerLevelList.isClear && !string.IsNullOrEmpty(this.lblRewardValue.text));
    Singleton<CommonRoot>.GetInstance().GetTowerHeaderComponent().SetHederTowerName(this.GetHeaderTowerName(this.currentFloor));
    this.dyn_reward_icon_unit.SetActive(false);
    this.dyn_reward_icon_item.SetActive(false);
  }

  private void UpdateBgScroll()
  {
    if (Object.op_Equality((Object) this.bgObject, (Object) null))
      return;
    float num1 = ((Component) this.scrollView).transform.localPosition.y - this.scroll_start_y;
    float num2 = this.grid.cellHeight * (float) (this.floorNum - 1);
    this.bgObject.transform.localPosition = new Vector3(this.bgObject.transform.localPosition.x, Mathf.Min(0.0f, Mathf.Max((this.direction == Tower029TopMenu.Direction.Up ? 0.0f : -this.bgHeight) + this.bgHeight * num1 / num2, -this.bgHeight)), this.bgObject.transform.localPosition.z);
  }

  private void UpdateFloorInfo()
  {
    if (this.progress == null)
      return;
    Vector3 localPosition = ((Component) this.scrollView).transform.localPosition;
    double scrollStartY = (double) this.scroll_start_y;
    int stage_id = -1;
    TowerStage towerStage = ((IEnumerable<TowerStage>) MasterData.TowerStageList).Where<TowerStage>((Func<TowerStage, bool>) (s => s.tower_id == this.progress.tower_id && s.floor == this.currentFloor)).FirstOrDefault<TowerStage>();
    if (towerStage != null)
      stage_id = towerStage.stage_id;
    if (stage_id == -1)
      return;
    string text = MasterData.BattleStage[stage_id].victory_condition.sub_name;
    if (string.IsNullOrEmpty(text))
      text = Consts.GetInstance().TOWER_VICTORY_CONDITION_DEFAULT;
    this.lblVictoryConditionValue.SetTextLocalize(text);
    TowerBattleStageClear battleStageClear = ((IEnumerable<TowerBattleStageClear>) MasterData.TowerBattleStageClearList).Where<TowerBattleStageClear>((Func<TowerBattleStageClear, bool>) (x => x.stage_id == stage_id && x.lap == this.progress.completed_count)).FirstOrDefault<TowerBattleStageClear>();
    if (battleStageClear != null)
      this.lblRewardValue.SetTextLocalize(battleStageClear.reward_name);
    else
      this.lblRewardValue.SetTextLocalize(string.Empty);
  }

  private IEnumerator ShowEnemyListPopup()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    IEnumerator e;
    if (Object.op_Equality((Object) this.enemyListPopup, (Object) null))
    {
      Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_029_tower_enemy_status__anim_popup01").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.enemyListPopup = f.Result;
      f = (Future<GameObject>) null;
    }
    GameObject popup = this.enemyListPopup.Clone();
    popup.SetActive(false);
    e = popup.GetComponent<Tower029EnemyListPopup>().InitializeAsync(this.progress, this.currentFloor, this.isClearAllFloor);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
  }

  private IEnumerator ShowMapCheckPopup()
  {
    Tower029TopMenu tower029TopMenu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    int stage_id = -1;
    // ISSUE: reference to a compiler-generated method
    TowerStage towerStage = ((IEnumerable<TowerStage>) MasterData.TowerStageList).Where<TowerStage>(new Func<TowerStage, bool>(tower029TopMenu.\u003CShowMapCheckPopup\u003Eb__79_0)).FirstOrDefault<TowerStage>();
    if (towerStage != null)
      stage_id = towerStage.stage_id;
    if (stage_id != -1)
    {
      IEnumerator e;
      if (Object.op_Equality((Object) tower029TopMenu.mapCheckPrefab, (Object) null))
      {
        Future<GameObject> f = new ResourceObject("Prefabs/popup/popup_029_tower_stage_status__anim_popup01").Load<GameObject>();
        e = f.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        tower029TopMenu.mapCheckPrefab = f.Result;
        f = (Future<GameObject>) null;
      }
      GameObject popup = tower029TopMenu.mapCheckPrefab.Clone();
      popup.SetActive(false);
      e = popup.GetComponent<Tower029MapcheckPopup>().InitializeAsync(stage_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    }
  }

  private IEnumerator UpdateRecoveryTime()
  {
    while (!this.isEndScene)
    {
      if (this.recovery_span.Milliseconds > 0)
      {
        this.dir_recovery_time.SetActive(true);
        this.lblRecoveryTime.SetTextLocalize(string.Format("{0:00} : {1:00} : {2:00}", (object) this.recovery_span.Hours, (object) this.recovery_span.Minutes, (object) this.recovery_span.Seconds));
        this.lblFreeRecovery.SetTextLocalize(TowerUtil.RecoveryCoinNum);
        yield return (object) new WaitForSeconds(1f);
        this.recovery_span = this.recovery_span.Add(new TimeSpan(0, 0, -1));
      }
      else
      {
        this.dir_recovery_time.SetActive(false);
        this.lblFreeRecovery.SetTextLocalize(Consts.GetInstance().TOWER_RECOVERY_FREE);
        break;
      }
    }
  }

  private string GetHeaderTowerName(int floor)
  {
    MasterDataTable.TowerPeriod towerPeriod = ((IEnumerable<MasterDataTable.TowerPeriod>) MasterData.TowerPeriodList).FirstOrDefault<MasterDataTable.TowerPeriod>((Func<MasterDataTable.TowerPeriod, bool>) (x => x.ID == this.progress.period_id));
    if (towerPeriod == null || towerPeriod.floor_id == null)
      return string.Empty;
    string str = string.Format("{0}{1}{2}", towerPeriod.floor_id.prefix == null ? (object) string.Empty : (object) towerPeriod.floor_id.prefix, (object) (floor * towerPeriod.floor_id.interval), towerPeriod.floor_id.suffix == null ? (object) string.Empty : (object) towerPeriod.floor_id.suffix);
    return string.Format("{0} {1}", (object) towerPeriod.tower_name, (object) str);
  }

  private void SetBgm()
  {
    foreach (TowerBgm towerBgm in ((IEnumerable<TowerBgm>) MasterData.TowerBgmList).Where<TowerBgm>((Func<TowerBgm, bool>) (x => x.period_id == this.progress.period_id)).OrderByDescending<TowerBgm, int>((Func<TowerBgm, int>) (x => x.floor)).ToList<TowerBgm>())
    {
      if (this.progress.floor >= towerBgm.floor)
      {
        TowerUtil.BgmFile = towerBgm.bgm_file;
        TowerUtil.BgmName = towerBgm.bgm_name;
        break;
      }
    }
  }

  private void ShowUnitSelectionStartPopup()
  {
    if (!Object.op_Inequality((Object) this.unitSelectionStartPopup, (Object) null))
      return;
    GameObject prefab = this.unitSelectionStartPopup.Clone();
    prefab.SetActive(false);
    prefab.GetComponent<Tower029UnitSelectionStartPopup>().Initialize((Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType>) ((x, y) => this.StartCoroutine(this.ShowUnitSelectionScene(x, y))), (Action) null);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private void ShowNoAvailableUnitPopup()
  {
    ModalWindow.Show(Consts.GetInstance().TOWER_MODAL_WARNING_NO_UNIT_TITLE, Consts.Format(Consts.GetInstance().TOWER_MODAL_WARNING_NO_UNIT_DESC, (IDictionary) new Hashtable()
    {
      {
        (object) "level",
        (object) TowerUtil.BorderLevel
      }
    }), (Action) (() => { }));
  }

  private void CheckMode()
  {
    MasterDataTable.TowerPeriod towerPeriod = ((IEnumerable<MasterDataTable.TowerPeriod>) MasterData.TowerPeriodList).FirstOrDefault<MasterDataTable.TowerPeriod>((Func<MasterDataTable.TowerPeriod, bool>) (p => p.ID == this.progress.period_id));
    if (towerPeriod != null)
    {
      DateTime now1 = this.now;
      DateTime? nullable = towerPeriod.end_at;
      if ((nullable.HasValue ? (now1 >= nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
      {
        DateTime now2 = this.now;
        nullable = towerPeriod.end_at_disp;
        if ((nullable.HasValue ? (now2 < nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0)
        {
          this.SetMode(Tower029TopMenu.Mode.Restriction);
          goto label_21;
        }
      }
    }
    if (!((IEnumerable<PlayerUnit>) this.towerEntryUnit).Any<PlayerUnit>())
    {
      if (this.isClearAllFloor)
        this.SetMode(Tower029TopMenu.Mode.Complete);
      else if (((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Any<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.level >= TowerUtil.BorderLevel)))
      {
        if (this.progress.is_entry)
        {
          this.SetMode(Tower029TopMenu.Mode.Annihilation);
          if (!this.isInit)
            ModalWindow.Show(Consts.GetInstance().TOWER_MODAL_UNIT_ANNIHILATION_TITLE, Consts.GetInstance().TOWER_MODAL_UNIT_ANNIHILATION_DESC, (Action) (() => { }));
        }
        else
        {
          this.SetMode(Tower029TopMenu.Mode.NoUnit);
          if (!this.isInit)
            this.ShowUnitSelectionStartPopup();
        }
      }
      else
      {
        this.SetMode(Tower029TopMenu.Mode.Restriction);
        if (!this.isInit)
          this.ShowNoAvailableUnitPopup();
      }
    }
    else if (!((IEnumerable<PlayerUnit>) this.towerEntryUnit).Any<PlayerUnit>((Func<PlayerUnit, bool>) (u => (double) u.TowerHpRate > 0.0)))
    {
      this.SetMode(Tower029TopMenu.Mode.Annihilation);
      if (!this.isInit)
        ModalWindow.Show(Consts.GetInstance().TOWER_MODAL_UNIT_ANNIHILATION_TITLE, Consts.GetInstance().TOWER_MODAL_UNIT_ANNIHILATION_DESC, (Action) (() => { }));
    }
    else if (this.isClearAllFloor)
      this.SetMode(Tower029TopMenu.Mode.Complete);
    else
      this.SetMode(Tower029TopMenu.Mode.Normal);
label_21:
    TowerTower towerTower;
    if (MasterData.TowerTower.TryGetValue(this.towerID, out towerTower))
    {
      if (!towerTower.ranking_flag)
        this.btn_ranking.SetActive(false);
      else
        this.btn_ranking.SetActive(true);
    }
    else
      this.btn_ranking.SetActive(true);
  }

  private void SetMode(Tower029TopMenu.Mode mode)
  {
    switch (mode)
    {
      case Tower029TopMenu.Mode.Normal:
        this.btnRestart.SetActive(false);
        this.btnBattleChallenge.SetActive(true);
        if (Object.op_Inequality((Object) this.currentTowerLevelList, (Object) null) && this.currentTowerLevelList.floorNum == this.progress.floor)
          ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = true;
        else
          ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = false;
        ((UIButtonColor) this.btnUnitList).isEnabled = true;
        ((UIButtonColor) this.btnRecovery).isEnabled = true;
        break;
      case Tower029TopMenu.Mode.Restriction:
        ((UIButtonColor) this.btnUnitList).isEnabled = false;
        ((UIButtonColor) this.btnRecovery).isEnabled = false;
        this.btnRestart.SetActive(false);
        this.btnBattleChallenge.SetActive(true);
        ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = false;
        break;
      case Tower029TopMenu.Mode.NoUnit:
        ((UIButtonColor) this.btnUnitList).isEnabled = false;
        ((UIButtonColor) this.btnRecovery).isEnabled = false;
        this.btnRestart.SetActive(false);
        this.btnBattleChallenge.SetActive(true);
        if (Object.op_Inequality((Object) this.currentTowerLevelList, (Object) null) && this.currentTowerLevelList.floorNum == this.progress.floor)
        {
          ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = true;
          break;
        }
        ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = false;
        break;
      case Tower029TopMenu.Mode.Complete:
        ((UIButtonColor) this.btnUnitList).isEnabled = false;
        ((UIButtonColor) this.btnRecovery).isEnabled = false;
        this.btnBattleChallenge.SetActive(false);
        this.btnRestart.SetActive(true);
        break;
      case Tower029TopMenu.Mode.Annihilation:
        ((UIButtonColor) this.btnUnitList).isEnabled = true;
        ((UIButtonColor) this.btnRecovery).isEnabled = true;
        this.btnRestart.SetActive(false);
        this.btnBattleChallenge.SetActive(true);
        ((UIButtonColor) this.btnBattleChallenge.GetComponent<SpreadColorButton>()).isEnabled = false;
        break;
    }
    if (!((UIButtonColor) this.btnRecovery).isEnabled)
      this.recovery_span = new TimeSpan(0L);
    else if (this.progress.recovered_at.HasValue)
    {
      TimeSpan offset = new TimeSpan((int) Consts.GetInstance().APP_TIME_ZONE, 0, 0);
      DateTime dateTime = TimeZoneInfo.ConvertTimeToUtc(this.now).AddHours(Consts.GetInstance().APP_TIME_ZONE);
      DateTimeOffset dateTimeOffset = new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(this.progress.recovered_at.Value).AddHours(Consts.GetInstance().APP_TIME_ZONE).AddDays(1.0).Ticks, offset);
      dateTimeOffset = new DateTimeOffset(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, 0, 0, 0, offset);
      this.recovery_span = dateTimeOffset - new DateTimeOffset(dateTime.Ticks, offset);
    }
    else
      this.recovery_span = new TimeSpan(0L);
    this.mode = mode;
  }

  private IEnumerator SetHeaderIcon()
  {
    Singleton<CommonRoot>.GetInstance().GetTowerHeaderComponent().Slc_tower_medal_icon.sprite2D = ShopCommon.PayTypeTowerMedal;
    Future<Sprite> ft = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Icons/Kiseki_Icon");
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) ft.Result, (Object) null))
      Singleton<CommonRoot>.GetInstance().GetTowerHeaderComponent().Slc_kiseki_icon.sprite2D = ft.Result;
  }

  protected override void Update()
  {
    base.Update();
    if ((double) ((Component) this.scrollView).transform.localPosition.y > (double) this.maxScrollHeight)
    {
      this.scrollView.scrollWheelFactor = 1f / 1000f;
      ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, (float) this.maxScrollHeight, 0.0f);
    }
    else if ((double) ((Component) this.scrollView).transform.localPosition.y < (double) this.minScrollHeight)
    {
      this.scrollView.scrollWheelFactor = 1f / 1000f;
      ((Component) this.scrollView).transform.localPosition = new Vector3(((Component) this.scrollView).transform.localPosition.x, (float) this.minScrollHeight, 0.0f);
    }
    else
      this.scrollView.scrollWheelFactor = 0.25f;
    this.isScroll = (double) ((Component) this.scrollView).transform.localPosition.y - (double) this.scroll_start_y != (double) this.scroll_delta_tmp;
    if (!this.isScroll)
      return;
    this.scroll_delta_tmp = ((Component) this.scrollView).transform.localPosition.y - this.scroll_start_y;
    this.UpdateLevelListScale();
    this.UpdateFloorInfo();
    this.UpdateBgScroll();
  }

  public IEnumerator InitializeAsync(bool forceInitialize = false)
  {
    Tower029TopMenu tower029TopMenu = this;
    tower029TopMenu.isEndScene = false;
    tower029TopMenu.towerID = -1;
    IEnumerator e1 = ServerTime.WaitSync();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    tower029TopMenu.now = ServerTime.NowAppTime();
    if (SMManager.Get<SM.TowerPeriod[]>() == null)
    {
      // ISSUE: reference to a compiler-generated method
      MasterDataTable.TowerPeriod towerPeriod = ((IEnumerable<MasterDataTable.TowerPeriod>) MasterData.TowerPeriodList).Where<MasterDataTable.TowerPeriod>(new Func<MasterDataTable.TowerPeriod, bool>(tower029TopMenu.\u003CInitializeAsync\u003Eb__89_4)).FirstOrDefault<MasterDataTable.TowerPeriod>();
      if (towerPeriod != null)
      {
        tower029TopMenu.towerID = towerPeriod.tower_id;
      }
      else
      {
        MypageScene.ChangeSceneOnError();
        yield break;
      }
    }
    else
      tower029TopMenu.towerID = SMManager.Get<SM.TowerPeriod[]>()[0].tower_id;
    if (forceInitialize && tower029TopMenu.isInit)
      tower029TopMenu.isInit = false;
    Future<WebAPI.Response.TowerTop> f = WebAPI.TowerTop(tower029TopMenu.towerID, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
    e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (f.Result == null)
    {
      tower029TopMenu.towerID = -1;
      TowerUtil.GotoMypage();
    }
    else
    {
      tower029TopMenu.progress = f.Result.tower_progress;
      tower029TopMenu.isClearAllFloor = f.Result.is_completed_last_floor;
      TowerUtil.TowerPlayer = f.Result.tower_player;
      TowerUtil.towerDeckUnits = ((IEnumerable<TowerDeckUnit>) f.Result.tower_deck_units).OrderBy<TowerDeckUnit, int>((Func<TowerDeckUnit, int>) (u => u.position_id)).ToArray<TowerDeckUnit>();
      // ISSUE: reference to a compiler-generated method
      tower029TopMenu.floorNum = ((IEnumerable<TowerStage>) MasterData.TowerStageList).Count<TowerStage>(new Func<TowerStage, bool>(tower029TopMenu.\u003CInitializeAsync\u003Eb__89_2));
      tower029TopMenu.slc_Badge_bikkuri_shop.SetActive(false);
      tower029TopMenu.slc_Badge_bikkuri_ranking.SetActive(f.Result.has_new_best_score);
      tower029TopMenu.SetBgm();
      e1 = tower029TopMenu.ResourceLoad(tower029TopMenu.towerID);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      tower029TopMenu.towerEntryUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.tower_is_entry)).ToArray<PlayerUnit>();
      tower029TopMenu.lblExistenceValue.SetTextLocalize(Consts.Format(Consts.GetInstance().TOWER_EXISTENCE_UNIT_VALUE, (IDictionary) new Hashtable()
      {
        {
          (object) "unit_num",
          (object) ((IEnumerable<PlayerUnit>) tower029TopMenu.towerEntryUnit).Count<PlayerUnit>((Func<PlayerUnit, bool>) (u => (double) u.TowerHpRate > 0.0))
        },
        {
          (object) "total_unit_num",
          (object) tower029TopMenu.towerEntryUnit.Length
        }
      }));
      Future<Sprite> ft = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Icons/TowerMedal_Icon");
      e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      tower029TopMenu.slc_icon_tower_medal.sprite2D = ft.Result;
      ShopCommon.PayTypeTowerMedal = ft.Result;
      ft = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Icons/Item_Icon_Kiseki");
      e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (Object.op_Inequality((Object) ft.Result, (Object) null))
        tower029TopMenu.slc_icon_kiseki.sprite2D = ft.Result;
      e1 = tower029TopMenu.SetHeaderIcon();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (!tower029TopMenu.isInit)
      {
        if (tower029TopMenu.currentFloor <= 0)
          tower029TopMenu.currentFloor = tower029TopMenu.progress.floor;
        // ISSUE: method pointer
        tower029TopMenu.centerOnChild.onFinished = new SpringPanel.OnFinished((object) tower029TopMenu, __methodptr(onFinished));
        tower029TopMenu.lblVictoryCondition.SetTextLocalize(Consts.GetInstance().TOWER_VICTORY_CONDITION);
        tower029TopMenu.lblReward.SetTextLocalize(Consts.GetInstance().TOWER_CLEAR_BUNUS_TITLE);
        tower029TopMenu.lblRecovery.SetTextLocalize(Consts.GetInstance().TOWER_RECOVERY_REST);
        tower029TopMenu.lblExistenceUnit.SetTextLocalize(Consts.GetInstance().TOWER_EXISTENCE_UNIT);
        tower029TopMenu.lblRewardAchieved.SetTextLocalize(Consts.GetInstance().TOWER_REWARD_ACHIEVED);
      }
      Singleton<CommonRoot>.GetInstance().GetTowerHeaderComponent().SetHeaderInfo(Player.Current.coin, TowerUtil.TowerPlayer.tower_medal, Player.Current.name, tower029TopMenu.GetHeaderTowerName(tower029TopMenu.currentFloor));
    }
  }

  public void StartScene()
  {
    if (this.towerID == -1)
      return;
    if (!this.isInit)
    {
      this.SetBackGround();
      this.SetFloor(this.floorNum, this.progress.floor);
      this.UpdateBgScroll();
      this.UpdateLevelListScale();
      this.UpdateFloorInfo();
    }
    bool flag = false;
    try
    {
      if (!Persist.towerSetting.Exists)
      {
        Persist.towerSetting.Data.reset();
        Persist.towerSetting.Flush();
      }
      if (Persist.towerSetting.Exists)
      {
        if (Persist.towerSetting.Data.isFirstTime)
        {
          flag = Persist.towerSetting.Data.isFirstTime;
          Persist.towerSetting.Data.isFirstTime = false;
          Persist.towerSetting.Flush();
        }
      }
    }
    catch (Exception ex)
    {
      Persist.towerSetting.Delete();
    }
    if (flag)
    {
      Tower029ManualScene.ChangeScene(true);
    }
    else
    {
      this.CheckMode();
      this.StartCoroutine(this.UpdateRecoveryTime());
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      this.isInit = true;
    }
  }

  public void onEndScene() => this.isEndScene = true;

  public void onUnitInfoButton()
  {
    if (this.isScroll)
      return;
    Tower029UnitListScene.ChangeScene();
  }

  public void onHowtoplayButton()
  {
    if (this.isScroll)
      return;
    Tower029ManualScene.ChangeScene(true);
  }

  public void onChallengeButton()
  {
    if (this.isScroll)
      return;
    if (this.mode == Tower029TopMenu.Mode.Normal)
    {
      if (TowerUtil.TowerPlayer.tower_medal >= TowerUtil.MaxTowerMedalNum)
        ModalWindow.Show(Consts.GetInstance().TOWER_MODAL_GOTO_SHOPPING_TITLE, Consts.GetInstance().TOWER_MODAL_GOTO_SHOPPING_DESC, (Action) (() => { }));
      else
        Tower029BattleEntryScene.ChangeScene(this.progress, this.centerOnChild.centeredObject.GetComponent<TowerLevelList>());
    }
    else
    {
      if (this.mode != Tower029TopMenu.Mode.NoUnit)
        return;
      this.ShowUnitSelectionStartPopup();
    }
  }

  public void onRestartButton()
  {
    if (this.isScroll)
      return;
    GameObject prefab = this.restartPopup.Clone();
    Tower029RestartPopup component = prefab.GetComponent<Tower029RestartPopup>();
    prefab.SetActive(false);
    GameObject unitSelectionPopup = this.restartUnitSelectionPopup;
    TowerProgress progress = this.progress;
    Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType> action = (Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType>) ((x, y) => this.StartCoroutine(this.ShowUnitSelectionScene(x, y)));
    int restartCoinNum = this.progress.completed_count == 1 ? 0 : TowerUtil.RestartCoinNum;
    component.Initialize(unitSelectionPopup, progress, action, restartCoinNum);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public void onEnemyInfoButton()
  {
    if (this.isScroll)
      return;
    this.StartCoroutine(this.ShowEnemyListPopup());
  }

  public void onRankingButton()
  {
    if (this.isScroll || this.IsPushAndSet())
      return;
    Tower029RankingScene.changeScene(this.progress.period_id, this.progress.tower_id);
  }

  public void onShopButton()
  {
    if (this.isScroll || this.IsPushAndSet())
      return;
    Tower029ShopScene.changeScene();
  }

  public void onRecoveryButton()
  {
    if (this.isScroll)
      return;
    GameObject prefab = this.recoveryByStonePopup.Clone();
    Tower029UnitLifeRecoveryPopup component = prefab.GetComponent<Tower029UnitLifeRecoveryPopup>();
    prefab.SetActive(false);
    bool flag = !((IEnumerable<PlayerUnit>) this.towerEntryUnit).Any<PlayerUnit>();
    GameObject popup = flag ? this.unitSelectionStartPopup : this.unitSelectionPopup;
    TowerProgress progress = this.progress;
    Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType> action = (Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType>) ((x, y) => this.StartCoroutine(this.ShowUnitSelectionScene(x, y)));
    int recoveryCoinNum = this.dir_recovery_time.activeSelf ? TowerUtil.RecoveryCoinNum : 0;
    int num = flag ? 1 : 0;
    component.Initialize(popup, progress, action, recoveryCoinNum, num != 0);
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public void onStageInfoButton()
  {
    if (this.isScroll)
      return;
    this.StartCoroutine(this.ShowMapCheckPopup());
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public enum Direction
  {
    Down = -1, // 0xFFFFFFFF
    Up = 1,
  }

  public enum Mode
  {
    Normal = 1,
    Restriction = 2,
    NoUnit = 3,
    Complete = 4,
    Annihilation = 5,
  }
}
