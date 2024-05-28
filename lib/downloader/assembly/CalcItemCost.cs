// Decompiled with JetBrains decompiler
// Type: CalcItemCost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class CalcItemCost
{
  public static long GetSellCost(List<InventoryItem> items)
  {
    long cost = 0;
    items.ForEach((Action<InventoryItem>) (item => cost += item.GetSellPrice()));
    return cost;
  }

  public static int GetCompositeCost(List<InventoryItem> items)
  {
    int totalItemLevel = 0;
    int totalItemRarity = 0;
    int useGears = 0;
    items.ForEach((Action<InventoryItem>) (item =>
    {
      if (item.Item.gear == null)
        return;
      totalItemLevel += item.Item.gear.compose_level;
      totalItemRarity += item.Item.gear.rarity.index;
      ++useGears;
    }));
    if (useGears < 1)
      useGears = 1;
    int index = totalItemRarity / useGears - 1;
    if (index < 0)
      index = 0;
    NGGameDataManager.Boost boostInfo = Singleton<NGGameDataManager>.GetInstance().BoostInfo;
    return (int) ((boostInfo == null ? 1.0M : boostInfo.DiscountGearCombine) * (Decimal) totalItemLevel * 50M * (Decimal) GearRarity.ComposeRatio(index));
  }

  public static float GetDrillingCostForOne(GameCore.ItemInfo targetGear, InventoryItem item)
  {
    float num = 0.0f;
    int gearLevelLimit = targetGear.playerItem.gear_level_limit;
    return targetGear == null ? num : (!CalcItemCost.IsSpecialDrilling(targetGear, item.Item, gearLevelLimit) ? CalcItemCost.GetNormalDrillingCost(item.Item) : CalcItemCost.GetSpecialDrillingCost(targetGear, gearLevelLimit, item.Item));
  }

  public static float GetDrillingCost(
    GameCore.ItemInfo targetGear,
    List<InventoryItem> items,
    int selectedNums = 0)
  {
    float drillingCost = 0.0f;
    int gearLevelLimit = targetGear.playerItem.gear_level_limit;
    if (targetGear == null || items.Count<InventoryItem>() == 0)
      return drillingCost;
    int num = gearLevelLimit + selectedNums;
    foreach (InventoryItem inventoryItem in items)
    {
      if (CalcItemCost.IsSpecialDrilling(targetGear, inventoryItem.Item, num))
      {
        drillingCost += CalcItemCost.GetSpecialDrillingCost(targetGear, num, inventoryItem.Item);
        ++num;
      }
      else
        drillingCost += CalcItemCost.GetNormalDrillingCost(inventoryItem.Item) * (inventoryItem.Item.tempSelectedCount > 1 ? (float) inventoryItem.Item.tempSelectedCount : 1f);
    }
    return drillingCost;
  }

  private static float GetNormalDrillingCost(GameCore.ItemInfo item)
  {
    return item == null ? 0.0f : (float) (50 * item.gear.compose_level) * item.gear.rarity.compose_ratio;
  }

  private static float GetSpecialDrillingCost(
    GameCore.ItemInfo targetGear,
    int ItemLimitLevel,
    GameCore.ItemInfo item)
  {
    int specialDrillingCost1 = 0;
    int num = ItemLimitLevel;
    if (item == null)
      return (float) specialDrillingCost1;
    GearSpecialDrillingCost specialDrillingCost2 = (GearSpecialDrillingCost) null;
    foreach (GearSpecialDrillingCost specialDrillingCost3 in (IEnumerable<GearSpecialDrillingCost>) ((IEnumerable<GearSpecialDrillingCost>) MasterData.GearSpecialDrillingCostList).OrderBy<GearSpecialDrillingCost, int>((Func<GearSpecialDrillingCost, int>) (x => x.rarity == null ? 1 : 0)))
    {
      if (specialDrillingCost3.level == num && (specialDrillingCost3.rarity == null || specialDrillingCost3.rarity != null && specialDrillingCost3.rarity.index == targetGear.gear.rarity.index))
      {
        specialDrillingCost2 = specialDrillingCost3;
        break;
      }
    }
    return specialDrillingCost2 == null ? (float) ((IEnumerable<GearSpecialDrillingCost>) MasterData.GearSpecialDrillingCostList).OrderByDescending<GearSpecialDrillingCost, int>((Func<GearSpecialDrillingCost, int>) (x => x.price)).First<GearSpecialDrillingCost>().price : (float) specialDrillingCost2.price;
  }

  private static bool IsSpecialDrilling(GameCore.ItemInfo targetGear, GameCore.ItemInfo item, int itemLimitLevel)
  {
    GearGear gear = item.gear;
    if (!gear.kind.isEquip && gear.kind.Enum != GearKindEnum.special_drilling || itemLimitLevel >= targetGear.playerItem.gear_level_limit_max)
      return false;
    if (gear.group_id == targetGear.gear.group_id)
      return true;
    if (gear.kind.Enum != GearKindEnum.special_drilling)
      return false;
    if (gear.special_drilling_kind == null)
      return true;
    return gear.special_drilling_kind != null && gear.special_drilling_kind.Enum == targetGear.gear.kind.Enum;
  }

  public static int GetBuildupCost(List<GameCore.ItemInfo> items)
  {
    int buildupCost = 0;
    foreach (GameCore.ItemInfo itemInfo in items)
    {
      GearKindEnum kind = itemInfo.gear.kind.Enum;
      int rank = itemInfo.gearLevel;
      buildupCost += ((IEnumerable<GearBuildup>) MasterData.GearBuildupList).Where<GearBuildup>((Func<GearBuildup, bool>) (x => x.kind.Enum == kind && x.rank == rank)).First<GearBuildup>().amount;
    }
    return buildupCost;
  }

  public static int GetReisouMixingCost(List<InventoryItem> items)
  {
    int totalCost = 0;
    items.ForEach((Action<InventoryItem>) (item =>
    {
      if (item.Item.gear == null)
        return;
      int mixingCostSingle = CalcItemCost.GetReisouMixingCostSingle(item.Item);
      int num = 1;
      if (item.Item.tempSelectedCount > 1)
        num = item.Item.tempSelectedCount;
      totalCost += mixingCostSingle * num;
    }));
    return totalCost;
  }

  public static int GetReisouMixingCostSingle(GameCore.ItemInfo item)
  {
    return (int) ((Decimal) (item.gear.compose_level * 50) * (Decimal) GearRarity.ComposeRatio(item.gear.rarity.index));
  }
}
