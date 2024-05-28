// Decompiled with JetBrains decompiler
// Type: CalcUnitCompose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class CalcUnitCompose
{
  private static int BREAKTHROUGH_BASE = 3000;
  private static float[] BREAKTHROUGH_REVISION = new float[4]
  {
    1f,
    1.5f,
    3f,
    5f
  };

  public static int getBaseValue(
    PlayerUnit unit,
    PlayerUnit[] material,
    CalcUnitCompose.ComposeType type)
  {
    IEnumerable<PlayerUnit> source = ((IEnumerable<PlayerUnit>) material).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && unit.unit.ID == x.unit.ID));
    switch (type)
    {
      case CalcUnitCompose.ComposeType.HP:
        int num1 = source.Count<PlayerUnit>() > 0 ? source.Max<PlayerUnit>((Func<PlayerUnit, int>) (x => x.hp.inheritance)) : 0;
        return unit.hp.inheritance >= num1 ? unit.self_total_hp : unit.hp.initial + unit.hp.level + unit.hp.compose + num1 + unit.hp.buildup;
      case CalcUnitCompose.ComposeType.STRENGTH:
        int num2 = source.Count<PlayerUnit>() > 0 ? source.Max<PlayerUnit>((Func<PlayerUnit, int>) (x => x.strength.inheritance)) : 0;
        return unit.strength.inheritance >= num2 ? unit.self_total_strength : unit.strength.initial + unit.strength.level + unit.strength.compose + num2 + unit.strength.buildup;
      case CalcUnitCompose.ComposeType.INTELLIGENCE:
        int num3 = source.Count<PlayerUnit>() > 0 ? source.Max<PlayerUnit>((Func<PlayerUnit, int>) (x => x.intelligence.inheritance)) : 0;
        return unit.intelligence.inheritance >= num3 ? unit.self_total_intelligence : unit.intelligence.initial + unit.intelligence.level + unit.intelligence.compose + num3 + unit.intelligence.buildup;
      case CalcUnitCompose.ComposeType.VITALITY:
        int num4 = source.Count<PlayerUnit>() > 0 ? source.Max<PlayerUnit>((Func<PlayerUnit, int>) (x => x.vitality.inheritance)) : 0;
        return unit.vitality.inheritance >= num4 ? unit.self_total_vitality : unit.vitality.initial + unit.vitality.level + unit.vitality.compose + num4 + unit.vitality.buildup;
      case CalcUnitCompose.ComposeType.MIND:
        int num5 = source.Count<PlayerUnit>() > 0 ? source.Max<PlayerUnit>((Func<PlayerUnit, int>) (x => x.mind.inheritance)) : 0;
        return unit.mind.inheritance >= num5 ? unit.self_total_mind : unit.mind.initial + unit.mind.level + unit.mind.compose + num5 + unit.mind.buildup;
      case CalcUnitCompose.ComposeType.AGILITY:
        int num6 = source.Count<PlayerUnit>() > 0 ? source.Max<PlayerUnit>((Func<PlayerUnit, int>) (x => x.agility.inheritance)) : 0;
        return unit.agility.inheritance >= num6 ? unit.self_total_agility : unit.agility.initial + unit.agility.level + unit.agility.compose + num6 + unit.agility.buildup;
      case CalcUnitCompose.ComposeType.DEXTERITY:
        int num7 = source.Count<PlayerUnit>() > 0 ? source.Max<PlayerUnit>((Func<PlayerUnit, int>) (x => x.dexterity.inheritance)) : 0;
        return unit.dexterity.inheritance >= num7 ? unit.self_total_dexterity : unit.dexterity.initial + unit.dexterity.level + unit.dexterity.compose + num7 + unit.dexterity.buildup;
      case CalcUnitCompose.ComposeType.LUCKY:
        int num8 = source.Count<PlayerUnit>() > 0 ? source.Max<PlayerUnit>((Func<PlayerUnit, int>) (x => x.lucky.inheritance)) : 0;
        return unit.lucky.inheritance >= num8 ? unit.self_total_lucky : unit.lucky.initial + unit.lucky.level + unit.lucky.compose + num8 + unit.lucky.buildup;
      default:
        return 0;
    }
  }

  public static int getComposeValue(
    PlayerUnit unit,
    PlayerUnit[] material,
    CalcUnitCompose.ComposeType type)
  {
    int num1 = 0;
    int num2 = 0;
    switch (type)
    {
      case CalcUnitCompose.ComposeType.HP:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.hp_compose));
        num2 = unit.compose_hp_max - unit.hp.compose;
        break;
      case CalcUnitCompose.ComposeType.STRENGTH:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.strength_compose));
        num2 = unit.compose_strength_max - unit.strength.compose;
        break;
      case CalcUnitCompose.ComposeType.INTELLIGENCE:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.intelligence_compose));
        num2 = unit.compose_intelligence_max - unit.intelligence.compose;
        break;
      case CalcUnitCompose.ComposeType.VITALITY:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.vitality_compose));
        num2 = unit.compose_vitality_max - unit.vitality.compose;
        break;
      case CalcUnitCompose.ComposeType.MIND:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.mind_compose));
        num2 = unit.compose_mind_max - unit.mind.compose;
        break;
      case CalcUnitCompose.ComposeType.AGILITY:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.agility_compose));
        num2 = unit.compose_agility_max - unit.agility.compose;
        break;
      case CalcUnitCompose.ComposeType.DEXTERITY:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.dexterity_compose));
        num2 = unit.compose_dexterity_max - unit.dexterity.compose;
        break;
      case CalcUnitCompose.ComposeType.LUCKY:
        IEnumerable<PlayerUnit> source1 = ((IEnumerable<PlayerUnit>) material).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && !x.unit.IsMaterialUnit && x.unit.same_character_id == unit.unit.same_character_id));
        IEnumerable<PlayerUnit> source2 = source1.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.rarity.index >= unit.unit.rarity.index));
        int num3 = source2.Count<PlayerUnit>() + source2.Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => x.lucky.compose));
        foreach (PlayerUnit playerUnit in source1.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.rarity.index < unit.unit.rarity.index)))
          num3 += 1 + playerUnit.unity_value;
        num1 = num3 + ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) || !x.unit.IsMaterialUnit ? 0 : x.unit.lucky_compose));
        num2 = unit.compose_lucky_max - unit.lucky.compose;
        break;
    }
    if (num2 < 0)
      num2 = 0;
    if (num1 < 0)
      num1 = 0;
    return num2 >= num1 ? num1 : num2;
  }

  public static int getBuildupMaterialCnt(PlayerUnit[] material, CalcUnitCompose.ComposeType type)
  {
    switch (type)
    {
      case CalcUnitCompose.ComposeType.HP:
        return ((IEnumerable<PlayerUnit>) material).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.hp_buildup > 0));
      case CalcUnitCompose.ComposeType.STRENGTH:
        return ((IEnumerable<PlayerUnit>) material).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.strength_buildup > 0));
      case CalcUnitCompose.ComposeType.INTELLIGENCE:
        return ((IEnumerable<PlayerUnit>) material).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.intelligence_buildup > 0));
      case CalcUnitCompose.ComposeType.VITALITY:
        return ((IEnumerable<PlayerUnit>) material).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.vitality_buildup > 0));
      case CalcUnitCompose.ComposeType.MIND:
        return ((IEnumerable<PlayerUnit>) material).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.mind_buildup > 0));
      case CalcUnitCompose.ComposeType.AGILITY:
        return ((IEnumerable<PlayerUnit>) material).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.agility_buildup > 0));
      case CalcUnitCompose.ComposeType.DEXTERITY:
        return ((IEnumerable<PlayerUnit>) material).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.dexterity_buildup > 0));
      case CalcUnitCompose.ComposeType.LUCKY:
        return ((IEnumerable<PlayerUnit>) material).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.unit.lucky_buildup > 0));
      default:
        return 0;
    }
  }

  public static int getBuildupValue(
    PlayerUnit unit,
    PlayerUnit[] material,
    CalcUnitCompose.ComposeType type)
  {
    int num1 = 0;
    int num2 = 0;
    switch (type)
    {
      case CalcUnitCompose.ComposeType.HP:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.hp_buildup));
        num2 = unit.hp.level_up_max_status - (unit.hp.level + unit.hp.buildup);
        break;
      case CalcUnitCompose.ComposeType.STRENGTH:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.strength_buildup));
        num2 = unit.strength.level_up_max_status - (unit.strength.level + unit.strength.buildup);
        break;
      case CalcUnitCompose.ComposeType.INTELLIGENCE:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.intelligence_buildup));
        num2 = unit.intelligence.level_up_max_status - (unit.intelligence.level + unit.intelligence.buildup);
        break;
      case CalcUnitCompose.ComposeType.VITALITY:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.vitality_buildup));
        num2 = unit.vitality.level_up_max_status - (unit.vitality.level + unit.vitality.buildup);
        break;
      case CalcUnitCompose.ComposeType.MIND:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.mind_buildup));
        num2 = unit.mind.level_up_max_status - (unit.mind.level + unit.mind.buildup);
        break;
      case CalcUnitCompose.ComposeType.AGILITY:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.agility_buildup));
        num2 = unit.agility.level_up_max_status - (unit.agility.level + unit.agility.buildup);
        break;
      case CalcUnitCompose.ComposeType.DEXTERITY:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.dexterity_buildup));
        num2 = unit.dexterity.level_up_max_status - (unit.dexterity.level + unit.dexterity.buildup);
        break;
      case CalcUnitCompose.ComposeType.LUCKY:
        num1 = ((IEnumerable<PlayerUnit>) material).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : x.unit.lucky_buildup));
        num2 = unit.lucky.level_up_max_status - (unit.lucky.level + unit.lucky.buildup);
        break;
    }
    if (num2 < 0)
      num2 = 0;
    if (num1 < 0)
      num1 = 0;
    return num2 >= num1 ? num1 : num2;
  }

  public static float getComposeUnity(
    PlayerUnit player_unit,
    PlayerUnit[] material,
    bool typeNormal)
  {
    if (player_unit.unity_value >= PlayerUnit.GetUnityValueMax() || material == null || material.Length == 0)
      return 0.0f;
    UnitUnit unit1 = player_unit.unit;
    float composeUnity = 0.0f;
    Func<PlayerUnit, float> func = typeNormal ? (Func<PlayerUnit, float>) (pu => (float) (pu.unity_value + 1)) : (Func<PlayerUnit, float>) (pu => pu.buildup_unity_value_f);
    foreach (PlayerUnit playerUnit in material)
    {
      if (!(playerUnit == (PlayerUnit) null))
      {
        UnitUnit unit2 = playerUnit.unit;
        if (unit2.IsNormalUnit)
        {
          if (unit1.same_character_id == unit2.same_character_id)
            composeUnity += func(playerUnit);
        }
        else if (unit2.is_unity_value_up)
        {
          if (typeNormal)
          {
            if (unit2.FindPureValueUpPattern(unit1) != null)
              ++composeUnity;
          }
          else
          {
            UnityValueUpPattern valueUpPattern = unit2.FindValueUpPattern(unit1, (Func<UnitFamily[]>) (() => player_unit.Families));
            if (valueUpPattern != null)
              composeUnity += valueUpPattern.up_value;
          }
        }
      }
    }
    return composeUnity;
  }

  public static bool isComposeMax(PlayerUnit unit, CalcUnitCompose.ComposeType type)
  {
    bool flag = true;
    switch (type)
    {
      case CalcUnitCompose.ComposeType.HP:
        if (unit.compose_hp_max > unit.hp.compose)
        {
          flag = false;
          break;
        }
        break;
      case CalcUnitCompose.ComposeType.STRENGTH:
        if (unit.compose_strength_max > unit.strength.compose)
        {
          flag = false;
          break;
        }
        break;
      case CalcUnitCompose.ComposeType.INTELLIGENCE:
        if (unit.compose_intelligence_max > unit.intelligence.compose)
        {
          flag = false;
          break;
        }
        break;
      case CalcUnitCompose.ComposeType.VITALITY:
        if (unit.compose_vitality_max > unit.vitality.compose)
        {
          flag = false;
          break;
        }
        break;
      case CalcUnitCompose.ComposeType.MIND:
        if (unit.compose_mind_max > unit.mind.compose)
        {
          flag = false;
          break;
        }
        break;
      case CalcUnitCompose.ComposeType.AGILITY:
        if (unit.compose_agility_max > unit.agility.compose)
        {
          flag = false;
          break;
        }
        break;
      case CalcUnitCompose.ComposeType.DEXTERITY:
        if (unit.compose_dexterity_max > unit.dexterity.compose)
        {
          flag = false;
          break;
        }
        break;
      case CalcUnitCompose.ComposeType.LUCKY:
        if (unit.compose_lucky_max > unit.lucky.compose)
        {
          flag = false;
          break;
        }
        break;
    }
    return flag;
  }

  public static long priceCompose(PlayerUnit base_unit, PlayerUnit[] material_units)
  {
    NGGameDataManager.Boost boostInfo = Singleton<NGGameDataManager>.GetInstance().BoostInfo;
    Decimal num = boostInfo == null ? 1.0M : boostInfo.DiscountUnitCompose;
    return (long) ((Decimal) CalcUnitCompose.price(base_unit, material_units, CalcUnitCompose.PriceMode.Compose) * num);
  }

  public static long priceStringth(PlayerUnit base_unit, PlayerUnit[] material_units)
  {
    NGGameDataManager.Boost boostInfo = Singleton<NGGameDataManager>.GetInstance().BoostInfo;
    Decimal num = boostInfo == null ? 1.0M : boostInfo.DiscountUnitBuildup;
    return (long) ((Decimal) CalcUnitCompose.price(base_unit, material_units, CalcUnitCompose.PriceMode.Stringth) * num);
  }

  private static long price(
    PlayerUnit base_unit,
    PlayerUnit[] material_units,
    CalcUnitCompose.PriceMode mode)
  {
    if (base_unit == (PlayerUnit) null || material_units.Length < 1)
      return 0;
    long num1;
    if (mode == CalcUnitCompose.PriceMode.Compose)
    {
      long num2 = (long) (1 + base_unit.amountIncrementInCompose);
      num1 = !base_unit.isMaxParamInCompose ? num2 * (long) ((IEnumerable<CommonStrengthComposePrice>) MasterData.CommonStrengthComposePriceList).FirstOrDefault<CommonStrengthComposePrice>((Func<CommonStrengthComposePrice, bool>) (x => x.ID == 2)).price * (long) material_units.Length : num2 * (long) ((IEnumerable<CommonStrengthComposePrice>) MasterData.CommonStrengthComposePriceList).FirstOrDefault<CommonStrengthComposePrice>((Func<CommonStrengthComposePrice, bool>) (x => x.ID == 1)).price * (long) material_units.Length;
    }
    else
      num1 = (long) (1 + base_unit.buildup_count) * (long) ((IEnumerable<CommonStrengthComposePrice>) MasterData.CommonStrengthComposePriceList).FirstOrDefault<CommonStrengthComposePrice>((Func<CommonStrengthComposePrice, bool>) (x => x.ID == 3)).price * (long) material_units.Length;
    long num3 = (long) base_unit.breakthrough_count;
    int sameCharacterId = base_unit.unit.same_character_id;
    int index = base_unit.unit.rarity.index;
    foreach (PlayerUnit materialUnit in material_units)
    {
      UnitUnit unit = materialUnit.unit;
      if (unit.IsBreakThrough || unit.same_character_id == sameCharacterId || unit.IsPureValueUp)
      {
        int num4 = unit.IsBreakThrough || unit.IsPureValueUp ? 1 : (unit.rarity.index < index ? materialUnit.unity_value + 1 : materialUnit.breakthrough_count + 1);
        num3 += (long) num4;
      }
    }
    if (num3 >= (long) base_unit.unit.breakthrough_limit)
      num3 = (long) base_unit.unit.breakthrough_limit;
    long num5 = 0;
    for (int breakthroughCount = base_unit.breakthrough_count; (long) breakthroughCount < num3; ++breakthroughCount)
      num5 += (long) (int) ((double) CalcUnitCompose.BREAKTHROUGH_BASE * (double) CalcUnitCompose.BREAKTHROUGH_REVISION[breakthroughCount]);
    long num6 = num5;
    return num1 + num6;
  }

  public enum ComposeType
  {
    HP,
    STRENGTH,
    INTELLIGENCE,
    VITALITY,
    MIND,
    AGILITY,
    DEXTERITY,
    LUCKY,
  }

  private enum PriceMode
  {
    Compose,
    Stringth,
  }
}
