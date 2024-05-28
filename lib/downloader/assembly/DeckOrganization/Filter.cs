// Decompiled with JetBrains decompiler
// Type: DeckOrganization.Filter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace DeckOrganization
{
  public class Filter
  {
    public const int UnknownIndex = -1;

    public bool isEnabled_ { get; private set; }

    public void setEnabledLimitLessEqual(bool isEnabled)
    {
      if (this.limit_ != Filter.Limit.LessEqual || this.limitParam_ <= 0 || this.limitParam_ <= this.limitCount_)
        return;
      this.isEnabled_ = isEnabled;
      this.limitCount = this.limitCount_;
    }

    public int index_ { get; private set; }

    public Filter.Factor factor_ { get; private set; }

    public bool isInvertedExpression_ { get; private set; }

    public Filter.Expression expression_ { get; private set; }

    public object param_ { get; private set; }

    public Filter.Limit limit_ { get; private set; }

    public bool hasLimit => this.limit_ != 0;

    public int limitParam_ { get; private set; }

    public int limitCount_ { get; private set; }

    public int limitCount
    {
      get => this.limitCount_;
      set
      {
        this.limitCount_ = value;
        if (this.limitParam_ <= 0)
          return;
        switch (this.limit_)
        {
          case Filter.Limit.Equal:
            this.isInvertedExpression_ = this.limitParam_ <= value;
            break;
          case Filter.Limit.GreaterEqual:
            this.isEnabled_ = this.limitParam_ > value;
            break;
          case Filter.Limit.LessEqual:
            if (!this.isEnabled_)
              break;
            this.isInvertedExpression_ = this.limitParam_ <= value;
            break;
        }
      }
    }

    public Filter(int index, Filter.Factor factor)
    {
      this.initCommon(index, factor, Filter.Expression.None, (object) 0, Filter.Limit.None, 0);
    }

    public Filter(int index, Filter.Factor factor, int param)
    {
      this.initCommon(index, factor, Filter.Expression.None, (object) param, Filter.Limit.None, 0);
    }

    public Filter(
      int index,
      Filter.Factor factor,
      Filter.Expression ex,
      int param,
      Filter.Limit limit = Filter.Limit.None,
      int limitParam = 0)
    {
      this.initCommon(index, factor, ex, (object) param, limit, limitParam);
    }

    public Filter(
      int index,
      Filter.Factor factor,
      Filter.Expression ex,
      List<int> nums,
      Filter.Limit limit = Filter.Limit.None,
      int limitParam = 0)
    {
      this.initCommon(index, factor, ex, nums != null ? (object) new List<int>((IEnumerable<int>) nums) : (object) (List<int>) null, limit, limitParam);
    }

    public Filter(
      int index,
      Filter.Factor factor,
      Filter.Expression ex,
      string param,
      Filter.Limit limit = Filter.Limit.None,
      int limitParam = 0)
    {
      this.initCommon(index, factor, ex, (object) param, limit, limitParam);
    }

    public Filter(
      int index,
      Filter.Factor factor,
      Filter.Expression ex,
      List<string> strs,
      Filter.Limit limit = Filter.Limit.None,
      int limitParam = 0)
    {
      this.initCommon(index, factor, ex, strs != null ? (object) new List<string>((IEnumerable<string>) strs) : (object) (List<string>) null, limit, limitParam);
    }

    private void initCommon(
      int index,
      Filter.Factor factor,
      Filter.Expression ex,
      object objParam,
      Filter.Limit limit,
      int limitParam)
    {
      this.isEnabled_ = true;
      this.index_ = index;
      this.factor_ = factor;
      this.isInvertedExpression_ = false;
      this.expression_ = ex;
      this.param_ = objParam;
      this.limit_ = limit;
      this.limitParam_ = limitParam;
      this.limitCount_ = 0;
    }

    public void resetForce(bool bEnabled, Filter.Limit limit = Filter.Limit.None, int limitParam = 0)
    {
      this.isEnabled_ = bEnabled;
      this.limit_ = limit;
      this.limitParam_ = limitParam;
    }

    public enum Factor
    {
      None,
      UnitType,
      Job,
      UnitFamily,
      MoveType,
      Blood,
      Height,
      Bust,
      Hips,
      Cost,
      Rarity,
      Character,
      UnitID,
      Level,
      GearKind,
      Skill,
      Weight,
      Waist,
      BirthdayMonth,
      ZodiacSign,
      SameCharacter,
      HistoryGroup,
      GroupLarge,
      GroupSmall,
      GroupClothing,
      GroupGeneration,
      DistinctCharacter,
      DistinctSameCharacter,
      DistinctUnit,
      DistinctHistoryGroup,
      UnitSetMin,
      UnitSetMax,
      TotalCost,
    }

    public enum Expression
    {
      None,
      Equal,
      GreaterThan,
      LessThan,
      GreaterEqual,
      LessEqual,
      Include,
      Ignore,
    }

    public enum Limit
    {
      None,
      Equal,
      GreaterEqual,
      LessEqual,
    }
  }
}
