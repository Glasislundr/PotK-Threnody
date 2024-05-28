// Decompiled with JetBrains decompiler
// Type: DeckOrganization.Creator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace DeckOrganization
{
  public class Creator
  {
    protected bool isDistinctCharacter_;
    protected bool isDistinctSameCharacter_;
    protected bool isDistinctUnit_;
    protected bool isDistinctHistoryGroup_;

    protected PlayerUnit[] regulars_ { get; private set; }

    protected PlayerUnit[] playerUnits_ { get; private set; }

    protected List<Filter> filters_ { get; private set; }

    protected int minQuantity_ { get; private set; }

    protected int maxQuantity_ { get; private set; }

    protected int limitedCost_ { get; private set; }

    protected bool isLimitedCost => this.limitedCost_ > 0;

    public List<PlayerUnit> result_ { get; private set; }

    public bool isSuccess => this.result_ != null && this.result_.Count > 0;

    public Creator(
      PlayerUnit[] regulars,
      PlayerUnit[] playerUnits,
      List<Filter> filters,
      int minQuantity,
      int maxQuantity = 0,
      int limitedCost = 0)
    {
      this.regulars_ = regulars;
      this.playerUnits_ = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null && u.unit != null)).ToArray<PlayerUnit>();
      HashSet<int> excludeIds = new HashSet<int>();
      for (int index1 = 0; index1 < this.playerUnits_.Length; ++index1)
      {
        PlayerUnit playerUnit = this.playerUnits_[index1];
        playerUnit.resetOnceOverkillers();
        if (playerUnit.isAnyCacheOverkillersUnits)
        {
          for (int index2 = 0; index2 < playerUnit.cache_overkillers_units.Length; ++index2)
          {
            if (playerUnit.cache_overkillers_units[index2] != (PlayerUnit) null)
              excludeIds.Add(playerUnit.cache_overkillers_units[index2].id);
          }
        }
      }
      if (excludeIds.Any<int>())
        this.playerUnits_ = ((IEnumerable<PlayerUnit>) this.playerUnits_).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => !excludeIds.Contains(u.id))).ToArray<PlayerUnit>();
      this.filters_ = filters;
      this.minQuantity_ = minQuantity;
      this.maxQuantity_ = maxQuantity;
      this.limitedCost_ = limitedCost;
      this.checkOption();
      if (this.maxQuantity_ < this.minQuantity_)
        this.maxQuantity_ = this.minQuantity_;
      this.result_ = (List<PlayerUnit>) null;
    }

    public IEnumerator Wait()
    {
      IEnumerator e = this.coMake();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    protected void setResult(IEnumerable<DeckPosition> deck)
    {
      this.setResult(deck != null ? deck.Where<DeckPosition>((Func<DeckPosition, bool>) (dp => dp.unit_ != null)).Select<DeckPosition, PlayerUnit>((Func<DeckPosition, PlayerUnit>) (dp => dp.unit_.unit_)).ToList<PlayerUnit>() : (List<PlayerUnit>) null);
    }

    protected void setResult(IEnumerable<Unit> units)
    {
      this.setResult(units != null ? units.Select<Unit, PlayerUnit>((Func<Unit, PlayerUnit>) (u => u?.unit_)).ToList<PlayerUnit>() : (List<PlayerUnit>) null);
    }

    protected void setResult(List<PlayerUnit> units)
    {
      this.result_ = units == null || units.Count<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null)) < this.minQuantity_ ? (List<PlayerUnit>) null : units;
    }

    protected virtual IEnumerator coMake()
    {
      List<PlayerUnit> playerUnitList = this.regulars_ != null ? ((IEnumerable<PlayerUnit>) this.regulars_).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u != (PlayerUnit) null)).ToList<PlayerUnit>() : new List<PlayerUnit>();
      if (this.maxQuantity_ - playerUnitList.Count >= 0)
      {
        int num1 = playerUnitList.Sum<PlayerUnit>((Func<PlayerUnit, int>) (u => u.cost));
        if (!this.isLimitedCost || num1 <= this.limitedCost_)
        {
          List<Unit> list = this.basicSort(this.getUnits(playerUnitList)).ToList<Unit>();
          if (this.minQuantity_ <= playerUnitList.Count + list.Count)
          {
            List<DeckPosition> ret = this.createDeckPositions(this.maxQuantity_);
            this.resetFilterLimitCount(this.filters_);
            int num2 = 0;
            bool flag = false;
            int index1 = 0;
            int index2 = -1;
            int count = ret.Count;
            while (true)
            {
              while (index1 < count)
              {
                if (ret[index1].unit_ != null)
                {
                  ++index1;
                }
                else
                {
                  IEnumerable<Unit> units = (IEnumerable<Unit>) new List<Unit>((IEnumerable<Unit>) list);
                  if (this.isDistinctUnit_)
                    units = units.Where<Unit>((Func<Unit, bool>) (u => !ret.Any<DeckPosition>((Func<DeckPosition, bool>) (dp => dp.unit_ != null && dp.unit_.unit_.unit.ID == u.unit_.unit.ID))));
                  if (this.isDistinctCharacter_)
                    units = units.Where<Unit>((Func<Unit, bool>) (u => !ret.Any<DeckPosition>((Func<DeckPosition, bool>) (dp => dp.unit_ != null && dp.unit_.unit_.unit.character_UnitCharacter == u.unit_.unit.character_UnitCharacter))));
                  if (this.isDistinctSameCharacter_)
                    units = units.Where<Unit>((Func<Unit, bool>) (u => !ret.Any<DeckPosition>((Func<DeckPosition, bool>) (dp => dp.unit_ != null && dp.unit_.unit_.unit.same_character_id == u.unit_.unit.same_character_id))));
                  if (this.isDistinctHistoryGroup_)
                    units = units.Where<Unit>((Func<Unit, bool>) (u => !ret.Any<DeckPosition>((Func<DeckPosition, bool>) (dp => dp.unit_ != null && dp.unit_.unit_.unit.history_group_number == u.unit_.unit.history_group_number))));
                  if (this.isLimitedCost)
                    num2 = this.limitedCost_ - ret.Sum<DeckPosition>((Func<DeckPosition, int>) (dp => dp.unit_ == null ? 0 : dp.unit_.unit_.cost));
                  Unit unit = this.setFilters(units, this.filters_, this.isLimitedCost ? new int?(num2) : new int?()).FirstOrDefault<Unit>();
                  if (unit == null)
                  {
                    if (this.filters_ != null)
                    {
                      foreach (Filter filter in this.filters_)
                        filter.setEnabledLimitLessEqual(false);
                      unit = this.setFilters(units, this.filters_, this.isLimitedCost ? new int?(num2) : new int?()).FirstOrDefault<Unit>();
                      if (unit == null)
                      {
                        foreach (Filter filter in this.filters_)
                          filter.setEnabledLimitLessEqual(true);
                      }
                    }
                    if (unit == null)
                    {
                      if (this.isLimitedCost && this.setFilters(units, this.filters_, new int?()).FirstOrDefault<Unit>() != null)
                      {
                        flag = true;
                        break;
                      }
                      break;
                    }
                  }
                  ret[index1].setUnit(unit);
                  index2 = index1;
                  this.incFilterLimitCount(this.filters_);
                  ++index1;
                }
              }
              if (flag)
              {
                flag = false;
                if (ret.Where<DeckPosition>((Func<DeckPosition, bool>) (dp => dp.unit_ != null)).Count<DeckPosition>() < this.minQuantity_ || !this.checkResultLimit())
                {
                  if (index2 >= 0)
                  {
                    this.revertSet(ret, index2);
                    index1 = index2;
                    index2 = -1;
                  }
                  else
                    goto label_40;
                }
                else
                  goto label_39;
              }
              else
                break;
            }
            if (ret.Where<DeckPosition>((Func<DeckPosition, bool>) (dp => dp.unit_ != null)).Count<DeckPosition>() < this.minQuantity_ || !this.checkResultLimit())
              goto label_40;
label_39:
            this.setResult((IEnumerable<DeckPosition>) ret);
            yield break;
          }
        }
      }
label_40:
      this.setResult((List<PlayerUnit>) null);
    }

    private void revertSet(List<DeckPosition> deck, int index)
    {
      if (index < 0 || deck.Count >= index || deck[index].unit_ == null)
        return;
      deck[index].setUnit((Unit) null);
      this.decFilterLimitCount(this.filters_);
    }

    private void checkOption()
    {
      this.isDistinctCharacter_ = false;
      this.isDistinctSameCharacter_ = false;
      this.isDistinctUnit_ = false;
      this.isDistinctHistoryGroup_ = false;
      if (this.filters_ == null)
        return;
      foreach (Filter filter in this.filters_)
      {
        switch (filter.factor_)
        {
          case Filter.Factor.DistinctCharacter:
            this.isDistinctCharacter_ = true;
            this.checkOptionDistinct(filter.expression_, filter.param_);
            filter.resetForce(false);
            continue;
          case Filter.Factor.DistinctSameCharacter:
            this.isDistinctSameCharacter_ = true;
            this.checkOptionDistinct(filter.expression_, filter.param_);
            filter.resetForce(false);
            continue;
          case Filter.Factor.DistinctUnit:
            this.isDistinctUnit_ = true;
            this.checkOptionDistinct(filter.expression_, filter.param_);
            filter.resetForce(false);
            continue;
          case Filter.Factor.DistinctHistoryGroup:
            this.isDistinctHistoryGroup_ = true;
            this.checkOptionDistinct(filter.expression_, filter.param_);
            filter.resetForce(false);
            continue;
          case Filter.Factor.UnitSetMin:
            this.minQuantity_ = (int) filter.param_;
            filter.resetForce(false);
            continue;
          case Filter.Factor.UnitSetMax:
            this.maxQuantity_ = (int) filter.param_;
            filter.resetForce(false);
            continue;
          case Filter.Factor.TotalCost:
            if (this.isLimitedCost)
            {
              if (this.limitedCost_ > (int) filter.param_)
                this.limitedCost_ = (int) filter.param_;
            }
            else
              this.limitedCost_ = (int) filter.param_;
            filter.resetForce(false);
            continue;
          default:
            continue;
        }
      }
    }

    private void checkOptionDistinct(Filter.Expression exp, object param)
    {
      if (exp != Filter.Expression.Include)
        return;
      foreach (int num in param as List<int>)
      {
        switch (num)
        {
          case 26:
            this.isDistinctCharacter_ = true;
            continue;
          case 27:
            this.isDistinctSameCharacter_ = true;
            continue;
          case 28:
            this.isDistinctUnit_ = true;
            continue;
          case 29:
            this.isDistinctHistoryGroup_ = true;
            continue;
          default:
            continue;
        }
      }
    }

    protected List<DeckPosition> createDeckPositions(int setnum)
    {
      List<DeckPosition> deckPositions1 = this.regulars_ != null ? ((IEnumerable<PlayerUnit>) this.regulars_).Select<PlayerUnit, DeckPosition>((Func<PlayerUnit, int, DeckPosition>) ((u, i) =>
      {
        DeckPosition deckPositions2 = new DeckPosition(i);
        if (u != (PlayerUnit) null)
          deckPositions2.setUnit(new Unit(u, true));
        return deckPositions2;
      })).ToList<DeckPosition>() : new List<DeckPosition>();
      int count = deckPositions1.Count;
      while (count < setnum)
        deckPositions1.Add(new DeckPosition(count++));
      return deckPositions1;
    }

    protected IEnumerable<Unit> getUnits(List<PlayerUnit> regulars = null)
    {
      return regulars == null || regulars.Count <= 0 ? ((IEnumerable<PlayerUnit>) this.playerUnits_).Select<PlayerUnit, Unit>((Func<PlayerUnit, Unit>) (u => new Unit(u))) : ((IEnumerable<PlayerUnit>) this.playerUnits_).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => !regulars.Any<PlayerUnit>((Func<PlayerUnit, bool>) (a => a.id == u.id)))).Select<PlayerUnit, Unit>((Func<PlayerUnit, Unit>) (u => new Unit(u)));
    }

    protected IEnumerable<Unit> basicSort(IEnumerable<Unit> units)
    {
      return (IEnumerable<Unit>) units.OrderByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.level)).ThenByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.unit.FinalEvolutionCost)).ThenByDescending<Unit, int>((Func<Unit, int>) (u => u.param_.Combat)).ThenByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.id)).ThenByDescending<Unit, DateTime>((Func<Unit, DateTime>) (u => u.unit_.created_at));
    }

    protected IEnumerable<Unit> setFilters(
      IEnumerable<Unit> units,
      List<Filter> filters,
      int? limitedCost)
    {
      if (units == null)
        return (IEnumerable<Unit>) null;
      IEnumerable<Unit> source = units.Where<Unit>((Func<Unit, bool>) (u => !u.hasIndex && !u.hasTrackIndex));
      if (limitedCost.HasValue)
        source = source.Where<Unit>((Func<Unit, bool>) (u => u.unit_.cost <= limitedCost.Value));
      if (filters == null || filters.Count == 0)
        return source;
      Dictionary<Filter.Factor, Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>> dictionary = new Dictionary<Filter.Factor, Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>>()
      {
        {
          Filter.Factor.UnitType,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterUnitType)
        },
        {
          Filter.Factor.Job,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterJob)
        },
        {
          Filter.Factor.UnitFamily,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterUnitFamily)
        },
        {
          Filter.Factor.MoveType,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterMoveType)
        },
        {
          Filter.Factor.Blood,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterBlood)
        },
        {
          Filter.Factor.Height,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterHeight)
        },
        {
          Filter.Factor.Bust,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterBust)
        },
        {
          Filter.Factor.Hips,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterHips)
        },
        {
          Filter.Factor.Cost,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterCost)
        },
        {
          Filter.Factor.Rarity,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterRarity)
        },
        {
          Filter.Factor.Character,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterCharacter)
        },
        {
          Filter.Factor.UnitID,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterUnitID)
        },
        {
          Filter.Factor.Level,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterLevel)
        },
        {
          Filter.Factor.GearKind,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterGearKind)
        },
        {
          Filter.Factor.Skill,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterSkill)
        },
        {
          Filter.Factor.Weight,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterWeight)
        },
        {
          Filter.Factor.Waist,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterWaist)
        },
        {
          Filter.Factor.BirthdayMonth,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterBirthdayMonth)
        },
        {
          Filter.Factor.ZodiacSign,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterZodiacSign)
        },
        {
          Filter.Factor.SameCharacter,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterSameCharacter)
        },
        {
          Filter.Factor.HistoryGroup,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterHistoryGroup)
        },
        {
          Filter.Factor.GroupLarge,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterGroupLarge)
        },
        {
          Filter.Factor.GroupSmall,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterGroupSmall)
        },
        {
          Filter.Factor.GroupClothing,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterGroupClothing)
        },
        {
          Filter.Factor.GroupGeneration,
          new Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>>(this.setFilterGroupGeneration)
        }
      };
      foreach (Filter filter in filters)
      {
        Func<IEnumerable<Unit>, Filter.Expression, bool, object, IEnumerable<Unit>> func;
        if (filter.isEnabled_ && dictionary.TryGetValue(filter.factor_, out func))
          source = func(source, filter.expression_, filter.isInvertedExpression_, filter.param_);
      }
      return source;
    }

    protected void resetFilterLimitCount(List<Filter> filters)
    {
      if (filters == null)
        return;
      foreach (Filter filter in filters)
      {
        if (filter.hasLimit)
        {
          filter.limitCount = 0;
          filter.setEnabledLimitLessEqual(true);
        }
      }
    }

    protected void incFilterLimitCount(List<Filter> filters)
    {
      if (filters == null)
        return;
      foreach (Filter filter in filters)
      {
        if (filter.hasLimit)
          ++filter.limitCount;
      }
    }

    protected void decFilterLimitCount(List<Filter> filters)
    {
      if (filters == null)
        return;
      foreach (Filter filter in filters)
      {
        if (filter.hasLimit)
          --filter.limitCount;
      }
    }

    protected bool checkResultLimit()
    {
      if (this.filters_ == null || this.filters_.Count == 0)
        return true;
      foreach (Filter filter in this.filters_)
      {
        switch (filter.limit_)
        {
          case Filter.Limit.Equal:
          case Filter.Limit.GreaterEqual:
            if (filter.limitCount_ < filter.limitParam_)
              return false;
            continue;
          default:
            continue;
        }
      }
      return true;
    }

    private IEnumerable<Unit> setFilterUnitType(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => u.unit_._unit_type == num)) : units.Where<Unit>((Func<Unit, bool>) (u => u.unit_._unit_type != num));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_._unit_type))) : units.Where<Unit>((Func<Unit, bool>) (u => !nums1.Contains(u.unit_._unit_type)));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_._unit_type))) : units.Where<Unit>((Func<Unit, bool>) (u => nums2.Contains(u.unit_._unit_type)));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterJob(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.getJobData().ID == num)) : units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.getJobData().ID != num));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.getJobData().ID))) : units.Where<Unit>((Func<Unit, bool>) (u => !nums1.Contains(u.unit_.getJobData().ID)));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.getJobData().ID))) : units.Where<Unit>((Func<Unit, bool>) (u => nums2.Contains(u.unit_.getJobData().ID)));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterUnitFamily(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.HasFamily((UnitFamily) num))) : units.Where<Unit>((Func<Unit, bool>) (u => !u.unit_.HasFamily((UnitFamily) num)));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitFamily[] fms = u.unit_.Families;
              return fms != null && fms.Length != 0 && nums1.Any<int>((Func<int, bool>) (n => ((IEnumerable<UnitFamily>) fms).Any<UnitFamily>((Func<UnitFamily, bool>) (fn => fn == (UnitFamily) n)))) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitFamily[] fms = u.unit_.Families;
              return fms != null && fms.Length != 0 && nums2.Any<int>((Func<int, bool>) (n => ((IEnumerable<UnitFamily>) fms).Any<UnitFamily>((Func<UnitFamily, bool>) (fn => fn == (UnitFamily) n)))) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterMoveType(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.getJobData().move_type_UnitMoveType == num)) : units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.getJobData().move_type_UnitMoveType != num));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.getJobData().move_type_UnitMoveType))) : units.Where<Unit>((Func<Unit, bool>) (u => !nums1.Contains(u.unit_.getJobData().move_type_UnitMoveType)));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return !isInvertedExpression ? units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.getJobData().move_type_UnitMoveType))) : units.Where<Unit>((Func<Unit, bool>) (u => nums2.Contains(u.unit_.getJobData().move_type_UnitMoveType)));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterBlood(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          string blood = param as string;
          if (!string.IsNullOrEmpty(blood))
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && character.blood_type == blood ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Include:
          List<string> strs1 = param as List<string>;
          if (strs1 != null && strs1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && strs1.Contains(character.blood_type) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<string> strs2 = param as List<string>;
          if (strs2 != null && strs2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && strs2.Contains(character.blood_type) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterHeight(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num1 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.height, out result) && result == num1 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterThan:
          int num2 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character == null || character == null || !int.TryParse(character.height, out result) || result <= num2 ? bFail : bSucceed;
          }));
        case Filter.Expression.LessThan:
          int num3 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.height, out result) && result < num3 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterEqual:
          int num4 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.height, out result) && result >= num4 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessEqual:
          int num5 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.height, out result) && result <= num5 ? bSucceed : bFail;
          }));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.height, out result) && nums1.Contains(result) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.height, out result) && nums2.Contains(result) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterBust(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num1 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.bust, out result) && result == num1 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterThan:
          int num2 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.bust, out result) && result > num2 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessThan:
          int num3 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.bust, out result) && result < num3 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterEqual:
          int num4 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.bust, out result) && result >= num4 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessEqual:
          int num5 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.bust, out result) && result <= num5 ? bSucceed : bFail;
          }));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.bust, out result) && nums1.Contains(result) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.bust, out result) && nums2.Contains(result) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterHips(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num1 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.hip, out result) && result == num1 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterThan:
          int num2 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.hip, out result) && result > num2 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessThan:
          int num3 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.hip, out result) && result < num3 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterEqual:
          int num4 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.hip, out result) && result >= num4 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessEqual:
          int num5 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.hip, out result) && result <= num5 ? bSucceed : bFail;
          }));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.hip, out result) && nums1.Contains(result) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.hip, out result) && nums2.Contains(result) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterCost(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num1 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.cost == num1 ? bSucceed : bFail));
        case Filter.Expression.GreaterThan:
          int num2 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.cost > num2 ? bSucceed : bFail));
        case Filter.Expression.LessThan:
          int num3 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.cost < num3 ? bSucceed : bFail));
        case Filter.Expression.GreaterEqual:
          int num4 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.cost >= num4 ? bSucceed : bFail));
        case Filter.Expression.LessEqual:
          int num5 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.cost <= num5 ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.cost) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.cost) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterRarity(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num1 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.rarity.index == num1 ? bSucceed : bFail));
        case Filter.Expression.GreaterThan:
          int num2 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.rarity.index > num2 ? bSucceed : bFail));
        case Filter.Expression.LessThan:
          int num3 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.rarity.index < num3 ? bSucceed : bFail));
        case Filter.Expression.GreaterEqual:
          int num4 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.rarity.index >= num4 ? bSucceed : bFail));
        case Filter.Expression.LessEqual:
          int num5 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.rarity.index <= num5 ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.unit.rarity.index) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.unit.rarity.index) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterCharacter(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.character_UnitCharacter == num ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.unit.character_UnitCharacter) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.unit.character_UnitCharacter) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterUnitID(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.ID == num ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.unit.ID) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.unit.ID) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterLevel(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num1 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.level == num1 ? bSucceed : bFail));
        case Filter.Expression.GreaterThan:
          int num2 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.level > num2 ? bSucceed : bFail));
        case Filter.Expression.LessThan:
          int num3 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.level < num3 ? bSucceed : bFail));
        case Filter.Expression.GreaterEqual:
          int num4 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.level >= num4 ? bSucceed : bFail));
        case Filter.Expression.LessEqual:
          int num5 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.level <= num5 ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.level) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.level) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterGearKind(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.kind_GearKind == num ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.unit.kind_GearKind) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.unit.kind_GearKind) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterSkill(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => ((IEnumerable<PlayerUnitSkills>) u.unit_.GetAcquireSkills()).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (s => s.skill_id == num)) ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => ((IEnumerable<PlayerUnitSkills>) u.unit_.GetAcquireSkills()).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (s => nums1.Contains(s.skill_id))) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !((IEnumerable<PlayerUnitSkills>) u.unit_.GetAcquireSkills()).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (s => nums2.Contains(s.skill_id))) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterWeight(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num1 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.weight, out result) && result == num1 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterThan:
          int num2 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            if (character == null)
              return false;
            int result;
            return character != null && int.TryParse(character.weight, out result) && result > num2 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessThan:
          int num3 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.weight, out result) && result < num3 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterEqual:
          int num4 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.weight, out result) && result >= num4 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessEqual:
          int num5 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.weight, out result) && result <= num5 ? bSucceed : bFail;
          }));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.weight, out result) && nums1.Contains(result) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.weight, out result) && nums2.Contains(result) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterWaist(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num1 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.waist, out result) && result == num1 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterThan:
          int num2 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            if (character == null)
              return false;
            int result;
            return character != null && int.TryParse(character.waist, out result) && result > num2 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessThan:
          int num3 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.waist, out result) && result < num3 ? bSucceed : bFail;
          }));
        case Filter.Expression.GreaterEqual:
          int num4 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.waist, out result) && result >= num4 ? bSucceed : bFail;
          }));
        case Filter.Expression.LessEqual:
          int num5 = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u =>
          {
            UnitCharacter character = u.unit_.unit.character;
            int result;
            return character != null && int.TryParse(character.waist, out result) && result <= num5 ? bSucceed : bFail;
          }));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.waist, out result) && nums1.Contains(result) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              int result;
              return character != null && int.TryParse(character.waist, out result) && nums2.Contains(result) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterBirthdayMonth(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          string bm = param as string;
          if (!string.IsNullOrEmpty(bm))
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && character.birthday.Length == 4 && character.birthday.Substring(0, 2) == bm ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Include:
          List<string> strs1 = param as List<string>;
          if (strs1 != null && strs1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && character.birthday.Length == 4 && strs1.Contains(character.birthday.Substring(0, 2)) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<string> strs2 = param as List<string>;
          if (strs2 != null && strs2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && character.birthday.Length == 4 && strs2.Contains(character.birthday.Substring(0, 2)) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterZodiacSign(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          string zsign = param as string;
          if (!string.IsNullOrEmpty(zsign))
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && character.zodiac_sign == zsign ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Include:
          List<string> strs1 = param as List<string>;
          if (strs1 != null && strs1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && strs1.Contains(character.zodiac_sign) ? bSucceed : bFail;
            }));
          break;
        case Filter.Expression.Ignore:
          List<string> strs2 = param as List<string>;
          if (strs2 != null && strs2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u =>
            {
              UnitCharacter character = u.unit_.unit.character;
              return character != null && strs2.Contains(character.zodiac_sign) ? bFail : bSucceed;
            }));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterSameCharacter(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.same_character_id == num ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.unit.same_character_id) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.unit.same_character_id) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterHistoryGroup(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.unit.history_group_number == num ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => nums1.Contains(u.unit_.unit.history_group_number) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => !nums2.Contains(u.unit_.unit.history_group_number) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterGroupLarge(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ != null && u.unitGroup_.group_large_category_id_UnitGroupLargeCategory == num ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ != null && nums1.Contains(u.unitGroup_.group_large_category_id_UnitGroupLargeCategory) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ == null || !nums2.Contains(u.unitGroup_.group_large_category_id_UnitGroupLargeCategory) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterGroupSmall(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ != null && u.unitGroup_.group_small_category_id_UnitGroupSmallCategory == num ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ != null && nums1.Contains(u.unitGroup_.group_small_category_id_UnitGroupSmallCategory) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ == null || !nums2.Contains(u.unitGroup_.group_small_category_id_UnitGroupSmallCategory) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterGroupClothing(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ != null && (u.unitGroup_.group_clothing_category_id_UnitGroupClothingCategory == num || u.unitGroup_.group_clothing_category_id_2_UnitGroupClothingCategory == num) ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ != null && (nums1.Contains(u.unitGroup_.group_clothing_category_id_UnitGroupClothingCategory) || nums1.Contains(u.unitGroup_.group_clothing_category_id_2_UnitGroupClothingCategory)) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ == null || !nums2.Contains(u.unitGroup_.group_clothing_category_id_UnitGroupClothingCategory) && !nums2.Contains(u.unitGroup_.group_clothing_category_id_2_UnitGroupClothingCategory) ? bSucceed : bFail));
          break;
      }
      return units;
    }

    private IEnumerable<Unit> setFilterGroupGeneration(
      IEnumerable<Unit> units,
      Filter.Expression ex,
      bool isInvertedExpression,
      object param)
    {
      bool bSucceed = !isInvertedExpression;
      bool bFail = isInvertedExpression;
      switch (ex)
      {
        case Filter.Expression.Equal:
          int num = (int) param;
          return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ != null && u.unitGroup_.group_generation_category_id_UnitGroupGenerationCategory == num ? bSucceed : bFail));
        case Filter.Expression.Include:
          List<int> nums1 = param as List<int>;
          if (nums1 != null && nums1.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ != null && nums1.Contains(u.unitGroup_.group_generation_category_id_UnitGroupGenerationCategory) ? bSucceed : bFail));
          break;
        case Filter.Expression.Ignore:
          List<int> nums2 = param as List<int>;
          if (nums2 != null && nums2.Count != 0)
            return units.Where<Unit>((Func<Unit, bool>) (u => u.unitGroup_ == null || !nums2.Contains(u.unitGroup_.group_generation_category_id_UnitGroupGenerationCategory) ? bSucceed : bFail));
          break;
      }
      return units;
    }
  }
}
