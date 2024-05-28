// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleUnitRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleUnitRule
  {
    public int ID;
    public int rule_no;
    public int value_type_BattleUnitRuleType;
    public int value_operator_BattleUnitRuleOperator;
    public string value;
    private int[] values_;

    public static BattleUnitRule Parse(MasterDataReader reader)
    {
      return new BattleUnitRule()
      {
        ID = reader.ReadInt(),
        rule_no = reader.ReadInt(),
        value_type_BattleUnitRuleType = reader.ReadInt(),
        value_operator_BattleUnitRuleOperator = reader.ReadInt(),
        value = reader.ReadString(true)
      };
    }

    public BattleUnitRuleType value_type => (BattleUnitRuleType) this.value_type_BattleUnitRuleType;

    public BattleUnitRuleOperator value_operator
    {
      get => (BattleUnitRuleOperator) this.value_operator_BattleUnitRuleOperator;
    }

    private bool checkRules(PlayerUnit playerUnit, UnitUnit unit)
    {
      if (this.values_ == null)
        this.values_ = this.value.CommaSeparatedToInts().ToArray();
      switch (this.value_type)
      {
        case BattleUnitRuleType.GearKind:
          return this.checkGearKind(playerUnit, unit);
        case BattleUnitRuleType.Cost:
          return this.checkCost(playerUnit, unit);
        case BattleUnitRuleType.Element:
          return this.checkElement(playerUnit, unit);
        case BattleUnitRuleType.SameCharacterID:
          return this.checkSameCharacterID(playerUnit, unit);
        case BattleUnitRuleType.GroupLarge:
          return this.checkGroupLarge(playerUnit, unit);
        case BattleUnitRuleType.GroupSmall:
          return this.checkGroupSmall(playerUnit, unit);
        case BattleUnitRuleType.GroupClothing:
          return this.checkGroupClothing(playerUnit, unit);
        case BattleUnitRuleType.GroupGeneration:
          return this.checkGroupGeneration(playerUnit, unit);
        default:
          Debug.LogError((object) string.Format("エンジニアの対応が必要なルール(={0})です", (object) this.value_type));
          return false;
      }
    }

    private bool checkGearKind(PlayerUnit playerUnit, UnitUnit unit)
    {
      return ((IEnumerable<int>) this.values_).Contains<int>(unit.kind_GearKind);
    }

    private bool checkCost(PlayerUnit playerUnit, UnitUnit unit)
    {
      switch (this.value_operator)
      {
        case BattleUnitRuleOperator.none:
          Debug.LogError((object) "コスト制限には区分の指定が必要です");
          return false;
        case BattleUnitRuleOperator.LessEqual:
          return playerUnit.cost <= ((IEnumerable<int>) this.values_).First<int>();
        case BattleUnitRuleOperator.Equal:
          return playerUnit.cost == ((IEnumerable<int>) this.values_).First<int>();
        case BattleUnitRuleOperator.GreaterEqual:
          return playerUnit.cost >= ((IEnumerable<int>) this.values_).First<int>();
        default:
          Debug.LogError((object) string.Format("エンジニアの対応が必要な区分(={0})です", (object) this.value_operator));
          return false;
      }
    }

    private bool checkElement(PlayerUnit playerUnit, UnitUnit unit)
    {
      return ((IEnumerable<PlayerUnitSkills>) playerUnit.GetAcquireSkills()).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => ((IEnumerable<int>) this.values_).Contains<int>(x.skill_id)));
    }

    private bool checkSameCharacterID(PlayerUnit playerUnit, UnitUnit unit)
    {
      return ((IEnumerable<int>) this.values_).Contains<int>(unit.same_character_id);
    }

    private bool checkGroupLarge(PlayerUnit playerUnit, UnitUnit unit)
    {
      if (BattleUnitRule.isInvalidCategory(((IEnumerable<int>) this.values_).First<int>()))
        return true;
      UnitGroup unitGroup = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == unit.ID));
      return unitGroup != null && !BattleUnitRule.isInvalidCategory(unitGroup.group_large_category_id_UnitGroupLargeCategory) && ((IEnumerable<int>) this.values_).Contains<int>(unitGroup.group_large_category_id_UnitGroupLargeCategory);
    }

    private bool checkGroupSmall(PlayerUnit playerUnit, UnitUnit unit)
    {
      if (BattleUnitRule.isInvalidCategory(((IEnumerable<int>) this.values_).First<int>()))
        return true;
      UnitGroup unitGroup = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == unit.ID));
      return unitGroup != null && !BattleUnitRule.isInvalidCategory(unitGroup.group_small_category_id_UnitGroupSmallCategory) && ((IEnumerable<int>) this.values_).Contains<int>(unitGroup.group_small_category_id_UnitGroupSmallCategory);
    }

    private bool checkGroupClothing(PlayerUnit playerUnit, UnitUnit unit)
    {
      if (BattleUnitRule.isInvalidCategory(((IEnumerable<int>) this.values_).First<int>()))
        return true;
      UnitGroup unitGroup = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == unit.ID));
      if (unitGroup == null)
        return false;
      List<int> source = new List<int>(2);
      if (!BattleUnitRule.isInvalidCategory(unitGroup.group_clothing_category_id_UnitGroupClothingCategory))
        source.Add(unitGroup.group_clothing_category_id_UnitGroupClothingCategory);
      if (!BattleUnitRule.isInvalidCategory(unitGroup.group_clothing_category_id_2_UnitGroupClothingCategory))
        source.Add(unitGroup.group_clothing_category_id_2_UnitGroupClothingCategory);
      return source.Any<int>((Func<int, bool>) (i => ((IEnumerable<int>) this.values_).Contains<int>(i)));
    }

    private bool checkGroupGeneration(PlayerUnit playerUnit, UnitUnit unit)
    {
      if (BattleUnitRule.isInvalidCategory(((IEnumerable<int>) this.values_).First<int>()))
        return true;
      UnitGroup unitGroup = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == unit.ID));
      return unitGroup != null && !BattleUnitRule.isInvalidCategory(unitGroup.group_generation_category_id_UnitGroupGenerationCategory) && ((IEnumerable<int>) this.values_).Contains<int>(unitGroup.group_generation_category_id_UnitGroupGenerationCategory);
    }

    private static bool isInvalidCategory(int category) => category == 1;

    public static Func<PlayerUnit, bool> createChecker(int rule_no)
    {
      BattleUnitRule[] rules = BattleUnitRule.getRules(rule_no);
      return rules.IsNullOrEmpty<BattleUnitRule>() ? (Func<PlayerUnit, bool>) null : (Func<PlayerUnit, bool>) (v => BattleUnitRule.checkRules(rules, v));
    }

    public static bool checkRules(BattleUnitRule[] rules, PlayerUnit target)
    {
      UnitUnit unit = target?.unit;
      if (unit == null)
        return false;
      if (rules.IsNullOrEmpty<BattleUnitRule>())
        return true;
      foreach (BattleUnitRule rule in rules)
      {
        if (!rule.checkRules(target, unit))
          return false;
      }
      return true;
    }

    public static BattleUnitRule[] getRules(int rule_no)
    {
      return ((IEnumerable<BattleUnitRule>) MasterData.BattleUnitRuleList).Where<BattleUnitRule>((Func<BattleUnitRule, bool>) (x => x.rule_no == rule_no)).ToArray<BattleUnitRule>();
    }
  }
}
