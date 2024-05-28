// Decompiled with JetBrains decompiler
// Type: SM.QuestLimitationBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestLimitationBase : KeyCompare
  {
    public int limitation_type;
    public string value;
    public int comparison_operator;
    public int? sub_comparison_operator;
    public int? sub_value;
    private static Dictionary<QuestLimitedCondition, DeckOrganization.Filter.Factor> dicConditionFactor_;
    private static Dictionary<OperatorEnum, DeckOrganization.Filter.Expression> dicOperatorExpression_;

    public QuestLimitationBase()
    {
    }

    public QuestLimitationBase(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.limitation_type = (int) (long) json[nameof (limitation_type)];
      this.value = (string) json[nameof (value)];
      this.comparison_operator = (int) (long) json[nameof (comparison_operator)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (sub_comparison_operator)] != null)
      {
        nullable1 = (long?) json[nameof (sub_comparison_operator)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.sub_comparison_operator = nullable2;
      int? nullable3;
      if (json[nameof (sub_value)] != null)
      {
        nullable1 = (long?) json[nameof (sub_value)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.sub_value = nullable3;
    }

    public QuestLimitedCondition limitedCondition => (QuestLimitedCondition) this.limitation_type;

    public OperatorEnum operatorEnum => (OperatorEnum) this.comparison_operator;

    public List<string> stringArguments
    {
      get
      {
        if (string.IsNullOrEmpty(this.value))
          return new List<string>();
        return ((IEnumerable<string>) this.value.Split(',')).Select<string, string>((Func<string, string>) (s => s.Trim())).ToList<string>();
      }
    }

    public List<int> intArguments
    {
      get
      {
        List<string> stringArguments = this.stringArguments;
        if (stringArguments.Count == 0)
          return new List<int>();
        List<int> intArguments = new List<int>();
        foreach (string s in stringArguments)
        {
          double result;
          if (double.TryParse(s, out result))
          {
            int num = (int) Math.Floor(result);
            intArguments.Add(num);
          }
          else
          {
            intArguments.Clear();
            break;
          }
        }
        return intArguments;
      }
    }

    public List<int> unitTypeArguments
    {
      get
      {
        List<string> stringArguments = this.stringArguments;
        if (stringArguments.Count == 0)
          return new List<int>();
        List<int> unitTypeArguments = new List<int>();
        foreach (string str in stringArguments)
        {
          string s = str;
          MasterDataTable.UnitType unitType = ((IEnumerable<MasterDataTable.UnitType>) MasterData.UnitTypeList).FirstOrDefault<MasterDataTable.UnitType>((Func<MasterDataTable.UnitType, bool>) (u => u.name == s));
          if (unitType != null)
          {
            unitTypeArguments.Add(unitType.ID);
          }
          else
          {
            unitTypeArguments.Clear();
            break;
          }
        }
        return unitTypeArguments;
      }
    }

    public List<int> familyArguments
    {
      get
      {
        List<string> stringArguments = this.stringArguments;
        if (stringArguments.Count == 0)
          return new List<int>();
        List<int> familyArguments = new List<int>();
        foreach (string str in stringArguments)
        {
          string s = str;
          UnitFamilyValue unitFamilyValue = ((IEnumerable<UnitFamilyValue>) MasterData.UnitFamilyValueList).FirstOrDefault<UnitFamilyValue>((Func<UnitFamilyValue, bool>) (u => u.name == s));
          if (unitFamilyValue != null)
          {
            familyArguments.Add(unitFamilyValue.ID);
          }
          else
          {
            familyArguments.Clear();
            break;
          }
        }
        return familyArguments;
      }
    }

    public List<int> rarityArguments
    {
      get
      {
        List<string> stringArguments = this.stringArguments;
        if (stringArguments.Count == 0)
          return new List<int>();
        List<int> rarityArguments = new List<int>();
        foreach (string str in stringArguments)
        {
          string s = str;
          UnitRarity unitRarity = ((IEnumerable<UnitRarity>) MasterData.UnitRarityList).FirstOrDefault<UnitRarity>((Func<UnitRarity, bool>) (u => u.name == s));
          if (unitRarity != null)
          {
            rarityArguments.Add(unitRarity.index + 1);
          }
          else
          {
            rarityArguments.Clear();
            break;
          }
        }
        return rarityArguments;
      }
    }

    private void getLimitParam(out DeckOrganization.Filter.Limit limit, out int limitParam)
    {
      limit = DeckOrganization.Filter.Limit.None;
      limitParam = 0;
      if (!this.sub_comparison_operator.HasValue)
        return;
      int num1 = this.sub_comparison_operator.Value;
      int num2 = this.sub_value.HasValue ? this.sub_value.Value : 0;
      switch (num1)
      {
        case 1:
          if (num2 <= 0)
            break;
          limit = DeckOrganization.Filter.Limit.Equal;
          limitParam = num2;
          break;
        case 4:
          if (num2 <= 0)
            break;
          limit = DeckOrganization.Filter.Limit.GreaterEqual;
          limitParam = num2;
          break;
        case 5:
          if (num2 <= 0)
            break;
          limit = DeckOrganization.Filter.Limit.LessEqual;
          limitParam = num2;
          break;
      }
    }

    private Dictionary<QuestLimitedCondition, DeckOrganization.Filter.Factor> dicConditionFactor
    {
      get
      {
        if (QuestLimitationBase.dicConditionFactor_ == null)
          QuestLimitationBase.dicConditionFactor_ = new Dictionary<QuestLimitedCondition, DeckOrganization.Filter.Factor>()
          {
            {
              QuestLimitedCondition.unit_type,
              DeckOrganization.Filter.Factor.UnitType
            },
            {
              QuestLimitedCondition.job,
              DeckOrganization.Filter.Factor.Job
            },
            {
              QuestLimitedCondition.population,
              DeckOrganization.Filter.Factor.UnitFamily
            },
            {
              QuestLimitedCondition.move_type,
              DeckOrganization.Filter.Factor.MoveType
            },
            {
              QuestLimitedCondition.blood,
              DeckOrganization.Filter.Factor.Blood
            },
            {
              QuestLimitedCondition.height,
              DeckOrganization.Filter.Factor.Height
            },
            {
              QuestLimitedCondition.bust,
              DeckOrganization.Filter.Factor.Bust
            },
            {
              QuestLimitedCondition.hip,
              DeckOrganization.Filter.Factor.Hips
            },
            {
              QuestLimitedCondition.cost,
              DeckOrganization.Filter.Factor.Cost
            },
            {
              QuestLimitedCondition.rarity,
              DeckOrganization.Filter.Factor.Rarity
            },
            {
              QuestLimitedCondition.unit,
              DeckOrganization.Filter.Factor.UnitID
            },
            {
              QuestLimitedCondition.skill,
              DeckOrganization.Filter.Factor.Skill
            },
            {
              QuestLimitedCondition.gear_kind,
              DeckOrganization.Filter.Factor.GearKind
            },
            {
              QuestLimitedCondition.same_character,
              DeckOrganization.Filter.Factor.SameCharacter
            },
            {
              QuestLimitedCondition.history_number,
              DeckOrganization.Filter.Factor.HistoryGroup
            },
            {
              QuestLimitedCondition.large_category,
              DeckOrganization.Filter.Factor.GroupLarge
            },
            {
              QuestLimitedCondition.small_category,
              DeckOrganization.Filter.Factor.GroupSmall
            },
            {
              QuestLimitedCondition.clothing_category,
              DeckOrganization.Filter.Factor.GroupClothing
            },
            {
              QuestLimitedCondition.generation_category,
              DeckOrganization.Filter.Factor.GroupGeneration
            },
            {
              QuestLimitedCondition.minimum_unit_count,
              DeckOrganization.Filter.Factor.UnitSetMin
            },
            {
              QuestLimitedCondition.max_unit_count,
              DeckOrganization.Filter.Factor.UnitSetMax
            },
            {
              QuestLimitedCondition.total_cost,
              DeckOrganization.Filter.Factor.TotalCost
            },
            {
              QuestLimitedCondition.same_unit,
              DeckOrganization.Filter.Factor.DistinctUnit
            },
            {
              QuestLimitedCondition.character,
              DeckOrganization.Filter.Factor.Character
            },
            {
              QuestLimitedCondition.unit_level,
              DeckOrganization.Filter.Factor.Level
            },
            {
              QuestLimitedCondition.weight,
              DeckOrganization.Filter.Factor.Weight
            },
            {
              QuestLimitedCondition.waist,
              DeckOrganization.Filter.Factor.Waist
            },
            {
              QuestLimitedCondition.zodiac_sign,
              DeckOrganization.Filter.Factor.ZodiacSign
            },
            {
              QuestLimitedCondition.birth_month,
              DeckOrganization.Filter.Factor.BirthdayMonth
            }
          };
        return QuestLimitationBase.dicConditionFactor_;
      }
    }

    public DeckOrganization.Filter.Factor filterFactor
    {
      get
      {
        DeckOrganization.Filter.Factor factor;
        return this.dicConditionFactor.TryGetValue(this.limitedCondition, out factor) ? factor : DeckOrganization.Filter.Factor.None;
      }
    }

    private Dictionary<OperatorEnum, DeckOrganization.Filter.Expression> dicOperatorExpression
    {
      get
      {
        if (QuestLimitationBase.dicOperatorExpression_ == null)
          QuestLimitationBase.dicOperatorExpression_ = new Dictionary<OperatorEnum, DeckOrganization.Filter.Expression>()
          {
            {
              OperatorEnum.equal,
              DeckOrganization.Filter.Expression.Equal
            },
            {
              OperatorEnum.greater_than,
              DeckOrganization.Filter.Expression.GreaterThan
            },
            {
              OperatorEnum.less_than,
              DeckOrganization.Filter.Expression.LessThan
            },
            {
              OperatorEnum.greater_than_or_equal_to,
              DeckOrganization.Filter.Expression.GreaterEqual
            },
            {
              OperatorEnum.less_than_or_equal_to,
              DeckOrganization.Filter.Expression.LessEqual
            },
            {
              OperatorEnum.In,
              DeckOrganization.Filter.Expression.Include
            },
            {
              OperatorEnum.Out,
              DeckOrganization.Filter.Expression.Ignore
            }
          };
        return QuestLimitationBase.dicOperatorExpression_;
      }
    }

    public DeckOrganization.Filter.Expression filterExpression
    {
      get
      {
        DeckOrganization.Filter.Expression expression;
        return this.dicOperatorExpression.TryGetValue(this.operatorEnum, out expression) ? expression : DeckOrganization.Filter.Expression.None;
      }
    }

    public DeckOrganization.Filter createFilter(int filterno)
    {
      DeckOrganization.Filter.Factor filterFactor = this.filterFactor;
      DeckOrganization.Filter.Expression filterExpression = this.filterExpression;
      DeckOrganization.Filter.Limit limit;
      int limitParam;
      this.getLimitParam(out limit, out limitParam);
      switch (filterFactor)
      {
        case DeckOrganization.Filter.Factor.UnitType:
          if (filterExpression != DeckOrganization.Filter.Expression.None)
          {
            List<int> unitTypeArguments = this.unitTypeArguments;
            if (unitTypeArguments.Count != 0)
            {
              switch (filterExpression)
              {
                case DeckOrganization.Filter.Expression.Include:
                case DeckOrganization.Filter.Expression.Ignore:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, unitTypeArguments, limit, limitParam);
                default:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, unitTypeArguments.First<int>(), limit, limitParam);
              }
            }
            else
              break;
          }
          else
            break;
        case DeckOrganization.Filter.Factor.Job:
        case DeckOrganization.Filter.Factor.MoveType:
        case DeckOrganization.Filter.Factor.Height:
        case DeckOrganization.Filter.Factor.Bust:
        case DeckOrganization.Filter.Factor.Hips:
        case DeckOrganization.Filter.Factor.Cost:
        case DeckOrganization.Filter.Factor.Character:
        case DeckOrganization.Filter.Factor.UnitID:
        case DeckOrganization.Filter.Factor.Level:
        case DeckOrganization.Filter.Factor.GearKind:
        case DeckOrganization.Filter.Factor.Skill:
        case DeckOrganization.Filter.Factor.Weight:
        case DeckOrganization.Filter.Factor.Waist:
        case DeckOrganization.Filter.Factor.SameCharacter:
        case DeckOrganization.Filter.Factor.HistoryGroup:
        case DeckOrganization.Filter.Factor.GroupLarge:
        case DeckOrganization.Filter.Factor.GroupSmall:
        case DeckOrganization.Filter.Factor.GroupClothing:
        case DeckOrganization.Filter.Factor.GroupGeneration:
          if (filterExpression != DeckOrganization.Filter.Expression.None)
          {
            List<int> intArguments = this.intArguments;
            if (intArguments.Count != 0)
            {
              switch (filterExpression)
              {
                case DeckOrganization.Filter.Expression.Include:
                case DeckOrganization.Filter.Expression.Ignore:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, intArguments, limit, limitParam);
                default:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, intArguments.First<int>(), limit, limitParam);
              }
            }
            else
              break;
          }
          else
            break;
        case DeckOrganization.Filter.Factor.UnitFamily:
          if (filterExpression != DeckOrganization.Filter.Expression.None)
          {
            List<int> familyArguments = this.familyArguments;
            if (familyArguments.Count != 0)
            {
              switch (filterExpression)
              {
                case DeckOrganization.Filter.Expression.Include:
                case DeckOrganization.Filter.Expression.Ignore:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, familyArguments, limit, limitParam);
                default:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, familyArguments.First<int>(), limit, limitParam);
              }
            }
            else
              break;
          }
          else
            break;
        case DeckOrganization.Filter.Factor.Blood:
        case DeckOrganization.Filter.Factor.BirthdayMonth:
        case DeckOrganization.Filter.Factor.ZodiacSign:
          if (filterExpression != DeckOrganization.Filter.Expression.None)
          {
            List<string> stringArguments = this.stringArguments;
            if (stringArguments.Count != 0)
            {
              switch (filterExpression)
              {
                case DeckOrganization.Filter.Expression.Include:
                case DeckOrganization.Filter.Expression.Ignore:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, stringArguments, limit, limitParam);
                default:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, stringArguments.First<string>(), limit, limitParam);
              }
            }
            else
              break;
          }
          else
            break;
        case DeckOrganization.Filter.Factor.Rarity:
          if (filterExpression != DeckOrganization.Filter.Expression.None)
          {
            List<int> rarityArguments = this.rarityArguments;
            if (rarityArguments.Count != 0)
            {
              switch (filterExpression)
              {
                case DeckOrganization.Filter.Expression.Include:
                case DeckOrganization.Filter.Expression.Ignore:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, rarityArguments, limit, limitParam);
                default:
                  return new DeckOrganization.Filter(filterno, filterFactor, filterExpression, rarityArguments.First<int>(), limit, limitParam);
              }
            }
            else
              break;
          }
          else
            break;
        case DeckOrganization.Filter.Factor.DistinctUnit:
          List<int> intArguments1 = this.intArguments;
          if (intArguments1.Count != 0)
          {
            List<int> intList = new List<int>();
            foreach (int num in intArguments1)
            {
              switch (num)
              {
                case 1:
                  intList.Add(29);
                  continue;
                case 2:
                  intList.Add(27);
                  continue;
                case 3:
                  intList.Add(26);
                  continue;
                case 4:
                  intList.Add(28);
                  continue;
                default:
                  continue;
              }
            }
            if (intList.Count != 0)
              return intList.Count == 1 ? new DeckOrganization.Filter(filterno, (DeckOrganization.Filter.Factor) intList.First<int>()) : new DeckOrganization.Filter(filterno, (DeckOrganization.Filter.Factor) intList.First<int>(), DeckOrganization.Filter.Expression.Include, intList);
            break;
          }
          break;
        case DeckOrganization.Filter.Factor.UnitSetMin:
        case DeckOrganization.Filter.Factor.UnitSetMax:
        case DeckOrganization.Filter.Factor.TotalCost:
          List<int> intArguments2 = this.intArguments;
          if (intArguments2.Count != 0)
            return new DeckOrganization.Filter(filterno, filterFactor, intArguments2.First<int>());
          break;
      }
      return (DeckOrganization.Filter) null;
    }
  }
}
