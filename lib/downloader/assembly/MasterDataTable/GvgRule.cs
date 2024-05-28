// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GvgRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitRegulation;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GvgRule
  {
    public int ID;
    public int rule_no;
    public int value_type_GvgRuleType;
    public int value_operator_GvgRuleOperator;
    public string value;
    private int[] values_;

    public static GvgRule Parse(MasterDataReader reader)
    {
      return new GvgRule()
      {
        ID = reader.ReadInt(),
        rule_no = reader.ReadInt(),
        value_type_GvgRuleType = reader.ReadInt(),
        value_operator_GvgRuleOperator = reader.ReadInt(),
        value = reader.ReadString(true)
      };
    }

    public GvgRuleType value_type => (GvgRuleType) this.value_type_GvgRuleType;

    public GvgRuleOperator value_operator => (GvgRuleOperator) this.value_operator_GvgRuleOperator;

    private bool checkRules(PlayerUnit target)
    {
      if (this.values_ == null)
        this.values_ = this.value.CommaSeparatedToInts().ToArray();
      switch (this.value_type)
      {
        case GvgRuleType.GearKind:
          return this.checkGearKind(target);
        case GvgRuleType.Cost:
          return this.checkCost(target);
        case GvgRuleType.Element:
          return this.checkElement(target);
        default:
          Debug.LogError((object) string.Format("エンジニアの対応が必要なルール(={0})です", (object) this.value_type));
          return false;
      }
    }

    private bool checkGearKind(PlayerUnit target)
    {
      return ((IEnumerable<int>) this.values_).Contains<int>(target.unit.kind_GearKind);
    }

    private bool checkCost(PlayerUnit target)
    {
      switch (this.value_operator)
      {
        case GvgRuleOperator.none:
          Debug.LogError((object) "コスト制限には区分の指定が必要です");
          return false;
        case GvgRuleOperator.LessEqual:
          return target.cost <= ((IEnumerable<int>) this.values_).First<int>();
        case GvgRuleOperator.Equal:
          return target.cost == ((IEnumerable<int>) this.values_).First<int>();
        case GvgRuleOperator.GreaterEqual:
          return target.cost >= ((IEnumerable<int>) this.values_).First<int>();
        default:
          Debug.LogError((object) string.Format("エンジニアの対応が必要な区分(={0})です", (object) this.value_operator));
          return false;
      }
    }

    private bool checkElement(PlayerUnit target)
    {
      return ((IEnumerable<PlayerUnitSkills>) target.GetAcquireSkills()).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => ((IEnumerable<int>) this.values_).Contains<int>(x.skill_id)));
    }

    public static Checker createCheckRules(int rule_no)
    {
      GvgRule[] rules = GvgRule.getRules(rule_no);
      return rules.IsNullOrEmpty<GvgRule>() ? (Checker) (_ => true) : (Checker) (playerUnit => GvgRule.checkRules(rules, playerUnit));
    }

    public static bool checkRules(GvgRule[] rules, PlayerUnit target)
    {
      if (target?.unit == null)
        return false;
      if (rules.IsNullOrEmpty<GvgRule>())
        return true;
      foreach (GvgRule rule in rules)
      {
        if (!rule.checkRules(target))
          return false;
      }
      return true;
    }

    public static GvgRule[] getRules(int rule_no)
    {
      return ((IEnumerable<GvgRule>) MasterData.GvgRuleList).Where<GvgRule>((Func<GvgRule, bool>) (x => x.rule_no == rule_no)).ToArray<GvgRule>();
    }
  }
}
