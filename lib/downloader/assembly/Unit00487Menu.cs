// Decompiled with JetBrains decompiler
// Type: Unit00487Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit00487Menu : Unit00486Menu
{
  public IEnumerator Init(
    Player player,
    PlayerUnit basePlayerUnit,
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits,
    PlayerUnit[] selectUnits,
    PlayerDeck[] playerDeck,
    bool isEquip,
    int selMax)
  {
    Unit00487Menu unit00487Menu = this;
    IEnumerator e = unit00487Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00487Menu.player = player;
    unit00487Menu.baseUnit = basePlayerUnit;
    unit00487Menu.playerUnitMax = playerUnits.Length;
    unit00487Menu.SelectMax = selMax;
    PlayerUnit[] playerUnitArray = new PlayerUnit[0];
    PlayerMaterialUnit[] array = ((IEnumerable<PlayerMaterialUnit>) playerMaterialUnits).Where<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => this.isComposeUnit(basePlayerUnit, x))).ToArray<PlayerMaterialUnit>();
    unit00487Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) playerUnitArray, (IEnumerable<PlayerMaterialUnit>) array, Persist.unit00487SortAndFilter, isEquip, false, true, true, true, false, (Action) (() => this.InitializeAllUnitInfosExtend(playerDeck)));
    unit00487Menu.CreateSelectIconInfo(selectUnits, true);
    e = unit00487Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00487Menu.UpdateInfomation();
    unit00487Menu.InitializeEnd();
  }

  public override bool isComposeUnit(PlayerUnit baseUnit, PlayerMaterialUnit unit)
  {
    bool flag = false;
    if (unit.unit.IsBuildup && unit.unit.IsBuildUpMaterial(baseUnit))
    {
      if (baseUnit.buildup_count < baseUnit.buildup_limit)
      {
        if (unit.unit.hp_buildup != 0 && !baseUnit.hp.is_max && baseUnit.hp.possibleBuildupCnt(baseUnit) > 0)
          flag = true;
        if (unit.unit.strength_buildup != 0 && !baseUnit.strength.is_max && baseUnit.strength.possibleBuildupCnt(baseUnit) > 0)
          flag = true;
        if (unit.unit.vitality_buildup != 0 && !baseUnit.vitality.is_max && baseUnit.vitality.possibleBuildupCnt(baseUnit) > 0)
          flag = true;
        if (unit.unit.intelligence_buildup != 0 && !baseUnit.intelligence.is_max && baseUnit.intelligence.possibleBuildupCnt(baseUnit) > 0)
          flag = true;
        if (unit.unit.mind_buildup != 0 && !baseUnit.mind.is_max && baseUnit.mind.possibleBuildupCnt(baseUnit) > 0)
          flag = true;
        if (unit.unit.agility_buildup != 0 && !baseUnit.agility.is_max && baseUnit.agility.possibleBuildupCnt(baseUnit) > 0)
          flag = true;
        if (unit.unit.dexterity_buildup != 0 && !baseUnit.dexterity.is_max && baseUnit.dexterity.possibleBuildupCnt(baseUnit) > 0)
          flag = true;
        if (unit.unit.lucky_buildup != 0 && !baseUnit.lucky.is_max && baseUnit.lucky.possibleBuildupCnt(baseUnit) > 0)
          flag = true;
      }
      else
        flag = false;
    }
    return flag;
  }

  protected override void returnScene(
    List<UnitIconInfo> list,
    PlayerUnit _basePlayerUnit,
    bool isEnter)
  {
    if (!isEnter)
    {
      this.backScene();
    }
    else
    {
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      foreach (UnitIconInfo unitIconInfo in list)
      {
        if (!unitIconInfo.playerUnit.favorite)
        {
          if (isEnter)
            unitIconInfo.SelectedCount = unitIconInfo.tempSelectedCount;
          unitIconInfo.isTempSelectedCount = false;
          unitIconInfo.tempSelectedCount = 0;
          playerUnitList.Add(unitIconInfo.playerUnit);
        }
      }
      Singleton<NGSceneManager>.GetInstance().clearStack(Unit004TrainingScene.DefSceneName);
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      Unit004TrainingScene.changeReinforce(false, _basePlayerUnit, playerUnitList.ToArray(), exceptionBackScene: this.exceptionBackScene);
    }
  }

  protected override void SetPrice(PlayerUnit[] materialPlayerUnits)
  {
    long num = CalcUnitCompose.priceStringth(this.baseUnit, materialPlayerUnits);
    this.TxtNumberzeny.SetTextLocalize(num.ToString());
    if (num > this.player.money)
    {
      ((UIWidget) this.TxtNumberzeny).color = Color.red;
      ((UIButtonColor) this.ibtnEnter).isEnabled = false;
    }
    else
    {
      ((UIWidget) this.TxtNumberzeny).color = Color.white;
      if (materialPlayerUnits.Length == 0)
        return;
      ((UIButtonColor) this.ibtnEnter).isEnabled = true;
    }
  }

  protected override void SetUpperParameter(PlayerUnit[] materialPlayerUnits)
  {
    int incParam1 = 0;
    int incParam2 = 0;
    int incParam3 = 0;
    int incParam4 = 0;
    int incParam5 = 0;
    int incParam6 = 0;
    int incParam7 = 0;
    int incParam8 = 0;
    foreach (PlayerUnit materialPlayerUnit in materialPlayerUnits)
    {
      incParam1 += materialPlayerUnit.unit.hp_buildup;
      incParam2 += materialPlayerUnit.unit.strength_buildup;
      incParam3 += materialPlayerUnit.unit.intelligence_buildup;
      incParam4 += materialPlayerUnit.unit.vitality_buildup;
      incParam5 += materialPlayerUnit.unit.mind_buildup;
      incParam6 += materialPlayerUnit.unit.agility_buildup;
      incParam7 += materialPlayerUnit.unit.dexterity_buildup;
      incParam8 += materialPlayerUnit.unit.lucky_buildup;
    }
    this.SetUpperParamterLabel(this.TxtIncHP, this.baseUnit.hp.buildup, this.baseUnit.hp.possibleBuildupCnt(this.baseUnit), incParam1);
    this.SetUpperParamterLabel(this.TxtIncPower, this.baseUnit.strength.buildup, this.baseUnit.strength.possibleBuildupCnt(this.baseUnit), incParam2);
    this.SetUpperParamterLabel(this.TxtIncMagic, this.baseUnit.intelligence.buildup, this.baseUnit.intelligence.possibleBuildupCnt(this.baseUnit), incParam3);
    this.SetUpperParamterLabel(this.TxtIncProtect, this.baseUnit.vitality.buildup, this.baseUnit.vitality.possibleBuildupCnt(this.baseUnit), incParam4);
    this.SetUpperParamterLabel(this.TxtIncSprit, this.baseUnit.mind.buildup, this.baseUnit.mind.possibleBuildupCnt(this.baseUnit), incParam5);
    this.SetUpperParamterLabel(this.TxtIncSpeed, this.baseUnit.agility.buildup, this.baseUnit.agility.possibleBuildupCnt(this.baseUnit), incParam6);
    this.SetUpperParamterLabel(this.TxtIncTechnique, this.baseUnit.dexterity.buildup, this.baseUnit.dexterity.possibleBuildupCnt(this.baseUnit), incParam7);
    this.SetUpperParamterLabel(this.TxtIncLucky, this.baseUnit.lucky.buildup, this.baseUnit.lucky.possibleBuildupCnt(this.baseUnit), incParam8);
  }

  protected override void CreateAllUnitInfo(
    IEnumerable<PlayerUnit> playerUnits,
    IEnumerable<PlayerMaterialUnit> playerMaterialUnits,
    bool isEquip,
    bool removeButton,
    bool for_battle_check,
    bool princessType,
    bool isSpecialIcon,
    int maxDispMaterialUnit)
  {
    this.allUnitInfos.Clear();
    if (removeButton)
      this.allUnitInfos.Add(new UnitIconInfo()
      {
        removeButton = true,
        gray = false,
        select = -1,
        for_battle = false,
        pricessType = false,
        equip = false,
        isSpecialIcon = false
      });
    if (playerUnits != null)
    {
      foreach (PlayerUnit playerUnit in playerUnits)
      {
        UnitIconInfo unitIconInfo = new UnitIconInfo();
        unitIconInfo.playerUnit = playerUnit;
        unitIconInfo.gray = false;
        unitIconInfo.select = -1;
        unitIconInfo.for_battle = false;
        unitIconInfo.pricessType = princessType;
        unitIconInfo.isSpecialIcon = false;
        if (unitIconInfo.playerUnit.equippedGear != (PlayerItem) null)
          unitIconInfo.equip = isEquip;
        this.allUnitInfos.Add(unitIconInfo);
      }
    }
    if (playerMaterialUnits != null)
    {
      int num1 = 0;
      foreach (PlayerMaterialUnit playerMaterialUnit in playerMaterialUnits)
      {
        if (playerMaterialUnit.unit.IsBuildup && playerMaterialUnit.unit.IsBuildUpMaterial(this.baseUnit))
        {
          if (playerMaterialUnit.unit.hp_buildup != 0)
            num1 = this.baseUnit.hp.possibleBuildupCnt(this.baseUnit) - this.baseUnit.hp.buildup;
          if (playerMaterialUnit.unit.strength_buildup != 0)
            num1 = this.baseUnit.strength.possibleBuildupCnt(this.baseUnit) - this.baseUnit.strength.buildup;
          if (playerMaterialUnit.unit.vitality_buildup != 0)
            num1 = this.baseUnit.vitality.possibleBuildupCnt(this.baseUnit) - this.baseUnit.vitality.buildup;
          if (playerMaterialUnit.unit.intelligence_buildup != 0)
            num1 = this.baseUnit.intelligence.possibleBuildupCnt(this.baseUnit) - this.baseUnit.intelligence.buildup;
          if (playerMaterialUnit.unit.mind_buildup != 0)
            num1 = this.baseUnit.mind.possibleBuildupCnt(this.baseUnit) - this.baseUnit.mind.buildup;
          if (playerMaterialUnit.unit.agility_buildup != 0)
            num1 = this.baseUnit.agility.possibleBuildupCnt(this.baseUnit) - this.baseUnit.agility.buildup;
          if (playerMaterialUnit.unit.dexterity_buildup != 0)
            num1 = this.baseUnit.dexterity.possibleBuildupCnt(this.baseUnit) - this.baseUnit.dexterity.buildup;
          if (playerMaterialUnit.unit.lucky_buildup != 0)
            num1 = this.baseUnit.lucky.possibleBuildupCnt(this.baseUnit) - this.baseUnit.lucky.buildup;
        }
        int num2;
        if (num1 <= 0)
          num2 = playerMaterialUnit.quantity;
        else
          num2 = Mathf.Min(new int[3]
          {
            num1,
            playerMaterialUnit.quantity,
            maxDispMaterialUnit
          });
        num1 = num2;
        for (int count = 0; count < num1; ++count)
          this.allUnitInfos.Add(new UnitIconInfo()
          {
            playerUnit = PlayerUnit.CreateByPlayerMaterialUnit(playerMaterialUnit, count),
            gray = false,
            select = -1,
            for_battle = false,
            pricessType = false,
            isSpecialIcon = false
          });
      }
    }
    if (for_battle_check)
    {
      this.ForBattle((Func<UnitIconInfo, PlayerUnit, bool>) ((info, unit) => info.playerUnit != (PlayerUnit) null && !info.removeButton && unit.id == info.playerUnit.id));
      this.UpdateAllUnitTowerEntryView();
      foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
      {
        if (SMManager.Get<PlayerRentalPlayerUnitIds>() != null)
          allUnitInfo.is_rental = SMManager.Get<PlayerRentalPlayerUnitIds>().rental_player_unit_ids != null && allUnitInfo.playerUnit != (PlayerUnit) null && ((IEnumerable<int?>) SMManager.Get<PlayerRentalPlayerUnitIds>().rental_player_unit_ids).Contains<int?>(new int?(allUnitInfo.playerUnit.id));
      }
    }
    if (isSpecialIcon)
    {
      QuestScoreBonusTimetable[] tables = ((IEnumerable<QuestScoreBonusTimetable>) SMManager.Get<QuestScoreBonusTimetable[]>()).Where<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => x.start_at < this.serverTime && x.end_at > this.serverTime)).ToArray<QuestScoreBonusTimetable>();
      UnitBonus[] unitBonus = UnitBonus.getActiveUnitBonus(ServerTime.NowAppTime());
      if (tables.Length != 0 || unitBonus.Length != 0)
        this.allUnitInfos.ForEach((Action<UnitIconInfo>) (x =>
        {
          if (!(x.playerUnit != (PlayerUnit) null))
            return;
          string color_code = x.playerUnit.SpecialEffectType((IEnumerable<QuestScoreBonusTimetable>) tables, (IEnumerable<UnitBonus>) unitBonus);
          int? specialIconType = UnitIcon.GetSpecialIconType(color_code);
          x.specialIconType = specialIconType.HasValue ? specialIconType.Value : 0;
          x.isSpecialIcon = !string.IsNullOrEmpty(color_code);
        }));
    }
    if (this.extendFunc == null)
      return;
    this.extendFunc();
  }
}
