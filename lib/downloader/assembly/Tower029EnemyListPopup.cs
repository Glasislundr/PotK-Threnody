// Decompiled with JetBrains decompiler
// Type: Tower029EnemyListPopup
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
public class Tower029EnemyListPopup : BackButtonMonoBehaiviour
{
  private const int BossIconWidth = 48;
  private const int BossIconHeight = 26;
  private const float GrayColor = 0.419607848f;
  [SerializeField]
  private Vector2 bossIconLocalPos;
  [SerializeField]
  private int bossIconDepth;
  [SerializeField]
  private NGxScroll2 scroll;
  [SerializeField]
  private UILabel lblPopupDesc;
  private BattleStageEnemy[] enemies;
  private List<BL.Unit> blUnitList;
  private GameObject iconPrefab;
  private List<UnitIconBase> iconList;
  private List<Tower029EnemyListPopup.EnemyUnitIconInfo> allEnemyInfo;
  private float scrool_start_y;
  private bool isInitialize;
  private bool isUpdateIcon;
  private GameObject goHpGauge;
  private int stageID = -1;
  private int floor;
  private TowerProgress progress;
  private GameObject enemyDetailPopup;
  private Tower029EnemyListPopup.EnemyUnitIconInfo bossIconInfo;
  private bool isClearAllFloor;

  private IEnumerator GetEnemyData(int tower_id, int floor)
  {
    TowerStage towerStage = ((IEnumerable<TowerStage>) MasterData.TowerStageList).Where<TowerStage>((Func<TowerStage, bool>) (s => s.tower_id == tower_id && s.floor == floor)).FirstOrDefault<TowerStage>();
    if (towerStage != null)
      this.stageID = towerStage.stage_id;
    IEnumerator e = MasterData.LoadBattleStageEnemy(MasterData.BattleStage[this.stageID]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.enemies = ((IEnumerable<BattleStageEnemy>) MasterData.BattleStageEnemyList).Where<BattleStageEnemy>((Func<BattleStageEnemy, bool>) (bse => !((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).Any<FacilityLevel>((Func<FacilityLevel, bool>) (fl => fl.unit_UnitUnit == bse.unit_UnitUnit)))).ToArray<BattleStageEnemy>();
  }

  private IEnumerator CreateEnemyInfo()
  {
    Tower029EnemyListPopup tower029EnemyListPopup = this;
    tower029EnemyListPopup.allEnemyInfo.Clear();
    for (int i = 0; i < tower029EnemyListPopup.enemies.Length; ++i)
    {
      Tower029EnemyListPopup.EnemyUnitIconInfo info = new Tower029EnemyListPopup.EnemyUnitIconInfo();
      info.playerUnit = PlayerUnit.FromEnemy(tower029EnemyListPopup.enemies[i]);
      info.gray = false;
      info.select = -1;
      info.for_battle = false;
      info.pricessType = false;
      info.isSpecialIcon = false;
      if (info.playerUnit.equippedGear != (PlayerItem) null)
        info.equip = false;
      if (tower029EnemyListPopup.isClearAllFloor)
        info.playerUnit.tower_hitpoint_rate = 0.0f;
      else if (tower029EnemyListPopup.progress.floor == tower029EnemyListPopup.floor)
      {
        TowerEnemy towerEnemy = ((IEnumerable<TowerEnemy>) tower029EnemyListPopup.progress.enemies).FirstOrDefault<TowerEnemy>((Func<TowerEnemy, bool>) (e => e.id == info.playerUnit.id));
        if (towerEnemy != null)
          info.playerUnit.tower_hitpoint_rate = towerEnemy.hitpoint_rate;
        else
          info.playerUnit.tower_hitpoint_rate = 100f;
      }
      else if (tower029EnemyListPopup.progress.floor > tower029EnemyListPopup.floor)
        info.playerUnit.tower_hitpoint_rate = 0.0f;
      else
        info.playerUnit.tower_hitpoint_rate = 100f;
      Future<BL.Unit> ft = tower029EnemyListPopup.createUnitByPlayerUnit(info.playerUnit, 0, false);
      IEnumerator e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      info.blUnit = ft.Result;
      List<BL.Unit> units = new List<BL.Unit>();
      units.Add(info.blUnit);
      List<BL.Unit> targets = new List<BL.Unit>();
      tower029EnemyListPopup.setLeaderSkills(units, targets);
      tower029EnemyListPopup.setPassiveSkills(units, targets);
      tower029EnemyListPopup.setGearSkills(units, targets);
      if (info.blUnit != (BL.Unit) null)
      {
        info.blUnit.setParameter(Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) info.blUnit, isUsePosition: false));
        info.blUnit.hp = TowerUtil.GetHp(info.blUnit.parameter.Hp, info.playerUnit.TowerHpRate);
        info.battleCombat = info.blUnit.parameter.Combat;
      }
      else
        info.battleCombat = info.playerUnit.combat;
      tower029EnemyListPopup.allEnemyInfo.Add(info);
      ft = (Future<BL.Unit>) null;
    }
    List<Tower029EnemyListPopup.EnemyUnitIconInfo> list1 = tower029EnemyListPopup.allEnemyInfo.Where<Tower029EnemyListPopup.EnemyUnitIconInfo>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, bool>) (x => (double) x.playerUnit.TowerHpRate > 0.0)).OrderByDescending<Tower029EnemyListPopup.EnemyUnitIconInfo, int>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, int>) (x => x.playerUnit.level)).ThenByDescending<Tower029EnemyListPopup.EnemyUnitIconInfo, int>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, int>) (x => x.battleCombat)).ThenByDescending<Tower029EnemyListPopup.EnemyUnitIconInfo, int>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, int>) (x => x.playerUnit.total_hp)).ToList<Tower029EnemyListPopup.EnemyUnitIconInfo>();
    List<Tower029EnemyListPopup.EnemyUnitIconInfo> list2 = tower029EnemyListPopup.allEnemyInfo.Where<Tower029EnemyListPopup.EnemyUnitIconInfo>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, bool>) (x => (double) x.playerUnit.TowerHpRate <= 0.0)).OrderByDescending<Tower029EnemyListPopup.EnemyUnitIconInfo, int>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, int>) (x => x.playerUnit.level)).ThenByDescending<Tower029EnemyListPopup.EnemyUnitIconInfo, int>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, int>) (x => x.battleCombat)).ThenByDescending<Tower029EnemyListPopup.EnemyUnitIconInfo, int>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, int>) (x => x.playerUnit.total_hp)).ToList<Tower029EnemyListPopup.EnemyUnitIconInfo>();
    list1.AddRange((IEnumerable<Tower029EnemyListPopup.EnemyUnitIconInfo>) list2);
    tower029EnemyListPopup.allEnemyInfo = list1;
    if (MasterData.BattleStage[tower029EnemyListPopup.stageID].victory_condition.enemy != null)
    {
      // ISSUE: reference to a compiler-generated method
      tower029EnemyListPopup.bossIconInfo = tower029EnemyListPopup.allEnemyInfo.Where<Tower029EnemyListPopup.EnemyUnitIconInfo>(new Func<Tower029EnemyListPopup.EnemyUnitIconInfo, bool>(tower029EnemyListPopup.\u003CCreateEnemyInfo\u003Eb__24_8)).FirstOrDefault<Tower029EnemyListPopup.EnemyUnitIconInfo>();
      if (tower029EnemyListPopup.bossIconInfo != null)
      {
        tower029EnemyListPopup.allEnemyInfo.Remove(tower029EnemyListPopup.bossIconInfo);
        tower029EnemyListPopup.allEnemyInfo.Insert(0, tower029EnemyListPopup.bossIconInfo);
      }
    }
  }

  private IEnumerator CreateBossIcon()
  {
    if (this.bossIconInfo != null && !Object.op_Equality((Object) this.bossIconInfo.icon, (Object) null))
    {
      Future<Sprite> ft = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Prefabs/UnitIcon/Materials/slc_icon_boss");
      IEnumerator e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!Object.op_Equality((Object) ft.Result, (Object) null))
      {
        GameObject gameObject = new GameObject("slc_icon_boss");
        UI2DSprite ui2Dsprite = gameObject.AddComponent<UI2DSprite>();
        ui2Dsprite.sprite2D = ft.Result;
        ((UIWidget) ui2Dsprite).depth = this.bossIconDepth;
        ((UIWidget) ui2Dsprite).SetDimensions(48, 26);
        gameObject.transform.SetParent(((Component) this.bossIconInfo.icon).transform);
        gameObject.transform.localPosition = new Vector3(this.bossIconLocalPos.x, this.bossIconLocalPos.y, 0.0f);
        if ((double) this.bossIconInfo.playerUnit.TowerHpRate <= 0.0)
          ((UIWidget) ui2Dsprite).color = new Color(0.419607848f, 0.419607848f, 0.419607848f);
      }
    }
  }

  private IEnumerator CreateUnitIcon()
  {
    this.iconList.Clear();
    int i;
    IEnumerator e;
    for (i = 0; i < Mathf.Min(UnitIcon.MaxValue, this.allEnemyInfo.Count); ++i)
    {
      UnitIconBase unitIcon = Object.Instantiate<GameObject>(this.iconPrefab).GetComponent<UnitIconBase>();
      if (this.allEnemyInfo[i].playerUnit != (PlayerUnit) null)
        unitIcon.unit = this.allEnemyInfo[i].playerUnit.unit;
      if (!this.isClearAllFloor && this.progress.floor <= this.floor)
      {
        if (Object.op_Equality((Object) this.goHpGauge, (Object) null))
        {
          Future<GameObject> f = Res.Prefabs.tower.dir_hp_gauge.Load<GameObject>();
          e = f.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.goHpGauge = f.Result;
          f = (Future<GameObject>) null;
        }
        if (Object.op_Inequality((Object) this.goHpGauge, (Object) null))
          this.goHpGauge.Clone(((UnitIcon) unitIcon).hp_gauge.transform);
      }
      this.iconList.Add(unitIcon);
      unitIcon.ShowBottomInfosLevelOnly();
      unitIcon.ForBattle = false;
      unitIcon.TowerEntry = false;
      unitIcon.CanAwake = false;
      unitIcon.UnitRental = false;
      unitIcon.SetupDeckStatusBlink();
      unitIcon.Equip = false;
      if (unitIcon is UnitIcon)
      {
        UnitIcon unitIcon1 = (UnitIcon) unitIcon;
        unitIcon1.princessType.DispPrincessType(false);
        unitIcon1.SpecialIcon = false;
        unitIcon1.SpecialIconType = -1;
      }
      ((Component) unitIcon).gameObject.SetActive(true);
      int height = this.progress.floor > this.floor ? UnitIcon.Height : UnitIcon.HeightWithHpGauge;
      this.scroll.Add(((Component) unitIcon).gameObject, UnitIcon.Width, height);
      unitIcon = (UnitIconBase) null;
    }
    for (int index = 0; index < Mathf.Min(UnitIcon.MaxValue, this.allEnemyInfo.Count); ++index)
      this.ResetUnitIcon(index);
    for (i = 0; i < Mathf.Min(UnitIcon.MaxValue, this.allEnemyInfo.Count); ++i)
    {
      e = this.CreateUnitIcon(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  protected virtual IEnumerator CreateUnitIcon(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    UnitIconBase unitIcon = this.iconList[unit_index];
    PlayerUnit playerUnit = this.allEnemyInfo[info_index].playerUnit;
    this.allEnemyInfo.Where<Tower029EnemyListPopup.EnemyUnitIconInfo>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<Tower029EnemyListPopup.EnemyUnitIconInfo>((Action<Tower029EnemyListPopup.EnemyUnitIconInfo>) (b => b.icon = (UnitIconBase) null));
    unitIcon.SetCounter(this.allEnemyInfo[info_index].count);
    this.allEnemyInfo[info_index].icon = unitIcon;
    IEnumerator e = unitIcon.SetPlayerUnit(playerUnit, this.getUnits(), baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitIcon.BottomModeValue = UnitIconBase.GetBottomModeLevel(playerUnit.unit, playerUnit);
    unitIcon.setLevelText(playerUnit);
    unitIcon.ForBattle = false;
    unitIcon.TowerEntry = false;
    unitIcon.UnitRental = false;
    unitIcon.CanAwake = false;
    unitIcon.SetupDeckStatusBlink();
    unitIcon.Equip = false;
    if (unitIcon is UnitIcon)
    {
      UnitIcon unitIcon1 = (UnitIcon) unitIcon;
      unitIcon1.princessType.DispPrincessType(this.allEnemyInfo[info_index].pricessType);
      unitIcon1.SpecialIconType = this.allEnemyInfo[info_index].specialIconType;
      unitIcon1.SpecialIcon = this.allEnemyInfo[info_index].isSpecialIcon;
    }
    else if (unitIcon is UnitDetailIcon)
      unitIcon.UnitIcon.princessType.DispPrincessType(this.allEnemyInfo[info_index].pricessType);
    unitIcon.SetCounter(this.allEnemyInfo[info_index].count);
    unitIcon.SetSelectionCounter(this.allEnemyInfo[info_index].SelectedCount);
    unitIcon.SelectMarker = this.allEnemyInfo[info_index].selectMarker;
    if (this.allEnemyInfo[info_index].alignmentSequence == 0)
    {
      if (this.allEnemyInfo[info_index].select == -1)
        unitIcon.Deselect();
      else
        unitIcon.Select(this.allEnemyInfo[info_index].select);
    }
    this.CreateUnitIconAction(info_index, unit_index);
    this.SetUnitIconHpGauge((UnitIcon) unitIcon, playerUnit);
    ((Component) unitIcon).gameObject.SetActive(true);
  }

  protected virtual void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    UnitIconBase unitIcon = this.iconList[unit_index];
    this.allEnemyInfo.Where<Tower029EnemyListPopup.EnemyUnitIconInfo>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<Tower029EnemyListPopup.EnemyUnitIconInfo>((Action<Tower029EnemyListPopup.EnemyUnitIconInfo>) (b => b.icon = (UnitIconBase) null));
    this.allEnemyInfo[info_index].icon = unitIcon;
    ((UnitIcon) unitIcon).SetPlayerUnitCache(this.allEnemyInfo[info_index].playerUnit, this.getUnits());
    ((UnitIcon) unitIcon).setBottom(this.allEnemyInfo[info_index].playerUnit);
    this.allEnemyInfo[info_index].icon.SetCounter(this.allEnemyInfo[info_index].count);
    this.allEnemyInfo[info_index].icon.SetSelectionCounter(this.allEnemyInfo[info_index].SelectedCount);
    unitIcon.ForBattle = false;
    unitIcon.TowerEntry = false;
    unitIcon.UnitRental = false;
    unitIcon.CanAwake = false;
    unitIcon.SetupDeckStatusBlink();
    unitIcon.Equip = false;
    if (unitIcon is UnitIcon)
    {
      UnitIcon unitIcon1 = (UnitIcon) unitIcon;
      unitIcon1.princessType.DispPrincessType(this.allEnemyInfo[info_index].pricessType);
      unitIcon1.SpecialIconType = this.allEnemyInfo[info_index].specialIconType;
      unitIcon1.SpecialIcon = this.allEnemyInfo[info_index].isSpecialIcon;
    }
    else if (unitIcon is UnitDetailIcon)
      unitIcon.UnitIcon.princessType.DispPrincessType(this.allEnemyInfo[info_index].pricessType);
    unitIcon.SelectMarker = this.allEnemyInfo[info_index].selectMarker;
    this.CreateUnitIconAction(info_index, unit_index);
    this.SetUnitIconHpGauge((UnitIcon) unitIcon, unitIcon.PlayerUnit);
    ((Component) unitIcon).gameObject.SetActive(true);
  }

  private void SetUnitIconHpGauge(UnitIcon icon, PlayerUnit playerUnit)
  {
    if (Object.op_Inequality((Object) icon.HpGauge, (Object) null))
      icon.HpGauge.TweenHpGauge.setValue(playerUnit.TowerHp, playerUnit.total_hp, false);
    if ((double) playerUnit.tower_hitpoint_rate > 0.0)
      return;
    icon.Gray = true;
  }

  private PlayerUnit[] getUnits()
  {
    return this.allEnemyInfo.Select<Tower029EnemyListPopup.EnemyUnitIconInfo, PlayerUnit>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>();
  }

  private IEnumerator ResourceLoad()
  {
    if (Object.op_Equality((Object) this.iconPrefab, (Object) null))
    {
      Future<GameObject> f = (Future<GameObject>) null;
      f = !Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/UnitIcon/normal").Load<GameObject>() : new ResourceObject("Prefabs/Sea/UnitIcon/normal_sea").Load<GameObject>();
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.iconPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  private void ScrollIconUpdate(int info_index, int unit_index)
  {
    this.ResetUnitIcon(unit_index);
    if (UnitIcon.IsCache(this.allEnemyInfo[info_index].playerUnit.unit))
      this.CreateUnitIconCache(info_index, unit_index);
    else
      this.StartCoroutine(this.CreateUnitIcon(info_index, unit_index));
  }

  protected void ScrollUpdate()
  {
    if ((!this.isInitialize || this.allEnemyInfo.Count <= UnitIcon.ScreenValue) && !this.isUpdateIcon)
      return;
    int num1 = UnitIcon.Height * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allEnemyInfo.Count - UnitIcon.ScreenValue - 1) / UnitIcon.ColumnValue * UnitIcon.Height);
    float num4 = (float) (UnitIcon.Height * UnitIcon.RowValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    while (true)
    {
      do
      {
        bool flag = false;
        int unit_index = 0;
        foreach (GameObject gameObject in this.scroll)
        {
          GameObject unit = gameObject;
          float num5 = unit.transform.localPosition.y + num2;
          if ((double) num5 > (double) num1)
          {
            int? nullable = this.allEnemyInfo.FirstIndexOrNull<Tower029EnemyListPopup.EnemyUnitIconInfo>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) unit)));
            int info_index = nullable.HasValue ? nullable.Value + UnitIcon.MaxValue : (this.allEnemyInfo.Count + 4) / 5 * 5;
            if (nullable.HasValue && info_index < (this.allEnemyInfo.Count + 4) / 5 * 5)
            {
              unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
              if (info_index >= this.allEnemyInfo.Count)
                unit.SetActive(false);
              else
                this.ScrollIconUpdate(info_index, unit_index);
              flag = true;
            }
          }
          else if ((double) num5 < -((double) num4 - (double) num1))
          {
            int num6 = UnitIcon.MaxValue;
            if (!unit.activeSelf)
            {
              unit.SetActive(true);
              num6 = 0;
            }
            int? nullable = this.allEnemyInfo.FirstIndexOrNull<Tower029EnemyListPopup.EnemyUnitIconInfo>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) unit)));
            int info_index = nullable.HasValue ? nullable.Value - num6 : -1;
            if (nullable.HasValue && info_index >= 0)
            {
              unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y + num4, 0.0f);
              this.ScrollIconUpdate(info_index, unit_index);
              flag = true;
            }
          }
          ++unit_index;
        }
        if (!flag)
          goto label_27;
      }
      while (!this.isUpdateIcon);
      this.isUpdateIcon = false;
    }
label_27:;
  }

  private void ResetUnitIcon(int index)
  {
    if (this.iconList == null || this.iconList.Count == 0)
      return;
    UnitIconBase unitIcon = this.iconList[index];
    ((UnitIcon) unitIcon).ResetUnit();
    ((Component) unitIcon).gameObject.SetActive(false);
    this.allEnemyInfo.Where<Tower029EnemyListPopup.EnemyUnitIconInfo>((Func<Tower029EnemyListPopup.EnemyUnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<Tower029EnemyListPopup.EnemyUnitIconInfo>((Action<Tower029EnemyListPopup.EnemyUnitIconInfo>) (b => b.icon = (UnitIconBase) null));
  }

  private void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase icon = this.iconList[unit_index];
    Tower029EnemyListPopup.EnemyUnitIconInfo unitInfo = this.allEnemyInfo[info_index];
    icon.onClick = (Action<UnitIconBase>) (u => this.StartCoroutine(this.ShowEnemyDetailPopup(unitInfo.blUnit)));
    icon.Button.onLongPress.Clear();
  }

  private IEnumerator ShowEnemyDetailPopup(BL.Unit unit)
  {
    if (Object.op_Equality((Object) this.enemyDetailPopup, (Object) null))
    {
      Future<GameObject> ft = new ResourceObject("Prefabs/battleUI_03/Battle030627_UI_PlayerStatus_2").Load<GameObject>();
      IEnumerator e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.enemyDetailPopup = ft.Result;
      ft = (Future<GameObject>) null;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<PopupManager>.GetInstance().open(this.enemyDetailPopup).GetComponent<BattleUI01UnitInformation>().InitFromTowerHistory(unit, false);
  }

  public IEnumerator InitializeAsync(TowerProgress progress, int selectedFloor, bool clearAllFloor)
  {
    Tower029EnemyListPopup tower029EnemyListPopup = this;
    tower029EnemyListPopup.isInitialize = false;
    tower029EnemyListPopup.scroll.Clear();
    BattleFuncs.environment.Reset((BL) null);
    if (Object.op_Inequality((Object) ((Component) tower029EnemyListPopup).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) tower029EnemyListPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    tower029EnemyListPopup.progress = progress;
    tower029EnemyListPopup.floor = selectedFloor;
    tower029EnemyListPopup.isClearAllFloor = clearAllFloor;
    tower029EnemyListPopup.lblPopupDesc.SetTextLocalize(Consts.GetInstance().POPUP_TOWER_ENEMY_LIST_DESC);
    IEnumerator e = tower029EnemyListPopup.GetEnemyData(progress.tower_id, selectedFloor);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029EnemyListPopup.iconList = new List<UnitIconBase>();
    tower029EnemyListPopup.allEnemyInfo = new List<Tower029EnemyListPopup.EnemyUnitIconInfo>();
    e = tower029EnemyListPopup.ResourceLoad();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = tower029EnemyListPopup.CreateEnemyInfo();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = tower029EnemyListPopup.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = tower029EnemyListPopup.CreateBossIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029EnemyListPopup.scrool_start_y = ((Component) tower029EnemyListPopup.scroll.scrollView).transform.localPosition.y;
    tower029EnemyListPopup.isInitialize = true;
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().dismiss();

  private Future<BL.Unit> createUnitByPlayerUnit(PlayerUnit pu, int index, bool friend)
  {
    if (pu.isDirtyOverkillersSlots)
      pu.resetOverkillersParameter();
    List<PlayerUnitSkills> list = ((IEnumerable<PlayerUnitSkills>) pu.skills).Concat<PlayerUnitSkills>((IEnumerable<PlayerUnitSkills>) pu.retrofitSkills).ToList<PlayerUnitSkills>();
    PlayerUnitSkills[] array1 = list.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)).ToArray<PlayerUnitSkills>();
    PlayerUnitSkills[] array2 = list.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.release)).ToArray<PlayerUnitSkills>();
    PlayerUnitSkills[] array3 = list.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).ToArray<PlayerUnitSkills>();
    PlayerUnitSkills[] array4 = list.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)).ToArray<PlayerUnitSkills>();
    GearGearSkill[] source1 = pu.equippedGear != (PlayerItem) null ? ((IEnumerable<GearGearSkill>) pu.equippedGear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)).ToArray<GearGearSkill>() : new GearGearSkill[0];
    GearGearSkill[] source2 = pu.equippedGear != (PlayerItem) null ? ((IEnumerable<GearGearSkill>) pu.equippedGear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)).ToArray<GearGearSkill>() : new GearGearSkill[0];
    GearGearSkill[] source3 = pu.equippedGear != (PlayerItem) null ? ((IEnumerable<GearGearSkill>) pu.equippedGear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).ToArray<GearGearSkill>() : new GearGearSkill[0];
    Future<BL.Skill> future1 = array2.Length == 0 ? Future.Single<BL.Skill>((BL.Skill) null) : this.createSkill(((IEnumerable<PlayerUnitSkills>) array2).Single<PlayerUnitSkills>());
    Future<List<BL.Skill>> future2 = ((IEnumerable<PlayerUnitSkills>) array1).Select<PlayerUnitSkills, Future<BL.Skill>>((Func<PlayerUnitSkills, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
    Future<List<BL.MagicBullet>> future3 = ((IEnumerable<PlayerUnitSkills>) array3).Select<PlayerUnitSkills, Future<BL.MagicBullet>>((Func<PlayerUnitSkills, Future<BL.MagicBullet>>) (v => this.createMagicBullet(v))).Sequence<BL.MagicBullet>();
    Future<List<BL.Skill>> future4 = ((IEnumerable<PlayerUnitSkills>) array4).Select<PlayerUnitSkills, Future<BL.Skill>>((Func<PlayerUnitSkills, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
    Future<List<BL.Skill>> future5 = ((IEnumerable<GearGearSkill>) source1).Select<GearGearSkill, Future<BL.Skill>>((Func<GearGearSkill, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
    Future<List<BL.Skill>> future6 = ((IEnumerable<GearGearSkill>) source2).Select<GearGearSkill, Future<BL.Skill>>((Func<GearGearSkill, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
    Future<List<BL.MagicBullet>> future7 = ((IEnumerable<GearGearSkill>) source3).Select<GearGearSkill, Future<BL.MagicBullet>>((Func<GearGearSkill, Future<BL.MagicBullet>>) (v => this.createMagicBullet(v))).Sequence<BL.MagicBullet>();
    Future<List<BL.Skill>> future8 = future2;
    Future<List<BL.MagicBullet>> future9 = future3;
    Future<List<BL.Skill>> future10 = future4;
    Future<List<BL.Skill>> future11 = future5;
    Future<List<BL.Skill>> future12 = future6;
    Future<List<BL.MagicBullet>> future13 = future7;
    Func<BL.Skill, List<BL.Skill>, List<BL.MagicBullet>, List<BL.Skill>, List<BL.Skill>, List<BL.Skill>, List<BL.MagicBullet>, BL.Unit> f = (Func<BL.Skill, List<BL.Skill>, List<BL.MagicBullet>, List<BL.Skill>, List<BL.Skill>, List<BL.Skill>, List<BL.MagicBullet>, BL.Unit>) ((ougi, skills, magicBullets, duel, gearCommand, gearDuel, gearMB) =>
    {
      BL.Weapon weapon = new BL.Weapon(pu);
      return new BL.Unit()
      {
        specificId = pu.id,
        unitId = pu.unit.ID,
        playerUnit = pu,
        lv = pu.level,
        spawnTurn = pu.spawn_turn,
        weapon = weapon,
        gearLeftHand = pu.isLeftHandWeapon,
        gearDualWield = pu.isDualWieldWeapon,
        AIMoveGroup = pu.ai_move_group,
        AIMoveGroupOrder = pu.ai_move_group_order,
        AIMoveTargetX = pu.ai_move_target_x,
        AIMoveTargetY = pu.ai_move_target_y,
        index = index,
        friend = friend,
        ougi = ougi,
        skills = skills.Concat<BL.Skill>((IEnumerable<BL.Skill>) gearCommand).ToArray<BL.Skill>(),
        duelSkills = duel.Concat<BL.Skill>((IEnumerable<BL.Skill>) gearDuel).OrderByDescending<BL.Skill, int>((Func<BL.Skill, int>) (x => x.skill.weight)).ThenBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).ToArray<BL.Skill>(),
        magicBullets = magicBullets.Concat<BL.MagicBullet>((IEnumerable<BL.MagicBullet>) gearMB).ToArray<BL.MagicBullet>(),
        skillfull_shield = XorShift.Range(1, 5),
        skillfull_weapon = XorShift.Range(1, 5),
        isSpawned = pu.spawn_turn <= 0
      };
    });
    return Future.WhenAllThen<BL.Skill, List<BL.Skill>, List<BL.MagicBullet>, List<BL.Skill>, List<BL.Skill>, List<BL.Skill>, List<BL.MagicBullet>, BL.Unit>(future1, future8, future9, future10, future11, future12, future13, f);
  }

  private Future<BL.Skill> createSkill(PlayerUnitSkills playerSkill)
  {
    BL.Skill skill = new BL.Skill()
    {
      id = playerSkill.skill.ID,
      level = playerSkill.level
    };
    skill.useTurn = playerSkill.skill.charge_turn - (skill.level - 1);
    skill.remain = playerSkill.skill.use_count == 0 ? new int?() : new int?(playerSkill.skill.use_count + (skill.level - 1));
    if (skill.remain.HasValue && playerSkill.skill.max_use_count != 0)
    {
      int? remain = skill.remain;
      int maxUseCount = playerSkill.skill.max_use_count;
      if (remain.GetValueOrDefault() > maxUseCount & remain.HasValue)
        skill.remain = new int?(playerSkill.skill.max_use_count);
    }
    return Future.Single<BL.Skill>(skill);
  }

  private Future<BL.Skill> createSkill(GearGearSkill gearSkill)
  {
    BL.Skill skill = new BL.Skill()
    {
      id = gearSkill.skill.ID,
      level = gearSkill.skill_level
    };
    skill.useTurn = gearSkill.skill.charge_turn - (skill.level - 1);
    skill.remain = gearSkill.skill.use_count == 0 ? new int?() : new int?(gearSkill.skill.use_count + (skill.level - 1));
    if (skill.remain.HasValue && gearSkill.skill.max_use_count != 0)
    {
      int? remain = skill.remain;
      int maxUseCount = gearSkill.skill.max_use_count;
      if (remain.GetValueOrDefault() > maxUseCount & remain.HasValue)
        skill.remain = new int?(gearSkill.skill.max_use_count);
    }
    return Future.Single<BL.Skill>(skill);
  }

  private Future<BL.MagicBullet> createMagicBullet(PlayerUnitSkills playerSkill)
  {
    return Future.Single<BL.MagicBullet>(new BL.MagicBullet()
    {
      skillId = playerSkill.skill.ID
    });
  }

  private Future<BL.MagicBullet> createMagicBullet(GearGearSkill gearSkill)
  {
    return Future.Single<BL.MagicBullet>(new BL.MagicBullet()
    {
      skillId = gearSkill.skill.ID
    });
  }

  private void setRangeSkills(
    List<BL.Unit> units,
    List<BL.Unit> targets,
    bool ownOnly,
    bool targetOnly,
    BL.Unit unit,
    BattleskillSkill skill,
    int level)
  {
    if (!targetOnly && skill.target_type == BattleskillTargetType.myself)
    {
      foreach (BattleskillEffect effect in skill.Effects)
        unit.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true));
    }
    else if (!targetOnly && (skill.target_type == BattleskillTargetType.player_single || skill.target_type == BattleskillTargetType.player_range))
    {
      foreach (BL.Unit unit1 in units)
      {
        foreach (BattleskillEffect effect in skill.Effects)
          unit1.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true));
      }
    }
    else if (!ownOnly && (skill.target_type == BattleskillTargetType.enemy_single || skill.target_type == BattleskillTargetType.enemy_range))
    {
      foreach (BL.Unit target in targets)
      {
        foreach (BattleskillEffect effect in skill.Effects)
          target.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true));
      }
    }
    else
    {
      if (skill.target_type != BattleskillTargetType.complex_single && skill.target_type != BattleskillTargetType.complex_range)
        return;
      foreach (BattleskillEffect effect in skill.Effects)
      {
        if (effect.is_targer_enemy)
        {
          if (!ownOnly)
          {
            foreach (BL.Unit target in targets)
              target.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true));
          }
        }
        else if (!targetOnly)
        {
          foreach (BL.Unit unit2 in units)
            unit2.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true));
        }
      }
    }
  }

  private void setLeaderSkills(
    List<BL.Unit> units,
    List<BL.Unit> targets,
    bool ownOnly = false,
    bool targetOnly = false)
  {
    foreach (BL.Unit unit in units)
    {
      if (unit.is_leader || unit.friend || unit.playerUnit.is_enemy_leader || unit.playerUnit.is_guest && unit.is_helper)
      {
        foreach (PlayerUnitLeader_skills leaderSkill in unit.playerUnit.leader_skills)
          this.setRangeSkills(units, targets, ownOnly, targetOnly, unit, leaderSkill.skill, leaderSkill.level);
      }
    }
  }

  private void setPassiveSkills(
    List<BL.Unit> units,
    List<BL.Unit> targets,
    bool ownOnly = false,
    bool targetOnly = false)
  {
    foreach (BL.Unit unit in units)
    {
      foreach (PlayerUnitSkills playerUnitSkills in ((IEnumerable<PlayerUnitSkills>) unit.playerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.passive)).ToArray<PlayerUnitSkills>())
      {
        if (!playerUnitSkills.skill.range_effect_passive_skill)
        {
          if (!targetOnly)
          {
            foreach (BattleskillEffect effect in playerUnitSkills.skill.Effects)
              unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, playerUnitSkills.skill, playerUnitSkills.level, true));
          }
        }
        else
          this.setRangeSkills(units, targets, ownOnly, targetOnly, unit, playerUnitSkills.skill, playerUnitSkills.level);
      }
    }
  }

  private void setGearSkills(
    List<BL.Unit> units,
    List<BL.Unit> targets,
    bool ownOnly = false,
    bool targetOnly = false)
  {
    foreach (BL.Unit unit in units)
    {
      PlayerItem equippedGear = unit.playerUnit.equippedGear;
      if (equippedGear != (PlayerItem) null)
      {
        foreach (GearGearSkill gearGearSkill in ((IEnumerable<GearGearSkill>) equippedGear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.passive)).ToArray<GearGearSkill>())
        {
          if (!gearGearSkill.skill.range_effect_passive_skill)
          {
            if (!targetOnly)
            {
              foreach (BattleskillEffect effect in gearGearSkill.skill.Effects)
                unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, gearGearSkill.skill, gearGearSkill.skill_level, true));
            }
          }
          else
            this.setRangeSkills(units, targets, ownOnly, targetOnly, unit, gearGearSkill.skill, gearGearSkill.skill_level);
        }
      }
    }
  }

  private class EnemyUnitIconInfo : UnitIconInfo
  {
    public BL.Unit blUnit;
    public int battleCombat;
  }
}
