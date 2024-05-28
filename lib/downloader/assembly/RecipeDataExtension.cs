// Decompiled with JetBrains decompiler
// Type: RecipeDataExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class RecipeDataExtension
{
  public static IEnumerable<RecipeData> FilterBy(this IEnumerable<RecipeData> self, bool[] filters)
  {
    List<RecipeData> list = self.Select<RecipeData, RecipeData>((Func<RecipeData, RecipeData>) (v => v)).ToList<RecipeData>();
    List<RecipeData> recipeDataList = new List<RecipeData>();
    for (int index = 0; index < list.Count; ++index)
    {
      RecipeData recipeData = list[index];
      if (RecipeDataExtension.CheckWeaponType(recipeData, filters) && RecipeDataExtension.CheckRarity(recipeData, filters) && RecipeDataExtension.CheckAttackType(recipeData, filters) && (!filters[22] || RecipeDataExtension.CheckWeapon(recipeData, filters)) && (!filters[23] || RecipeDataExtension.CheckSpecialWeapon(recipeData, filters)))
        recipeDataList.Add(recipeData);
    }
    return (IEnumerable<RecipeData>) recipeDataList;
  }

  private static bool CheckAllEnumStateEqual(bool[] blist)
  {
    return ((IEnumerable<bool>) blist).All<bool>((Func<bool, bool>) (v => v == blist[0]));
  }

  private static bool CheckWeaponType(RecipeData item, bool[] filters)
  {
    if (RecipeDataExtension.CheckAllEnumStateEqual(new bool[8]
    {
      filters[1],
      filters[2],
      filters[3],
      filters[4],
      filters[5],
      filters[6],
      filters[7],
      filters[8]
    }))
      return true;
    switch (item.combinedGear.kind.Enum)
    {
      case GearKindEnum.sword:
        return filters[1];
      case GearKindEnum.axe:
        return filters[2];
      case GearKindEnum.spear:
        return filters[3];
      case GearKindEnum.bow:
        return filters[4];
      case GearKindEnum.gun:
        return filters[5];
      case GearKindEnum.staff:
        return filters[6];
      case GearKindEnum.shield:
        return filters[7];
      case GearKindEnum.accessories:
        return filters[8];
      default:
        return false;
    }
  }

  private static bool CheckRarity(RecipeData item, bool[] filters)
  {
    if (!filters[9] && !filters[10] && !filters[11] && !filters[12] && !filters[13] && !filters[14] && !filters[15])
      return true;
    switch (item.combinedGear.rarity.index)
    {
      case 1:
        return filters[9];
      case 2:
        return filters[10];
      case 3:
        return filters[11];
      case 4:
        return filters[12];
      case 5:
        return filters[13];
      case 6:
        return filters[14];
      case 7:
        return filters[15];
      default:
        return false;
    }
  }

  private static bool CheckWeapon(RecipeData item, bool[] filters)
  {
    if (filters[22] && filters[23])
      return true;
    return !item.combinedGear.hasSpecificationOfEquipmentUnits && filters[22];
  }

  private static bool CheckSpecialWeapon(RecipeData item, bool[] filters)
  {
    if (filters[22] && filters[23])
      return true;
    return item.combinedGear.hasSpecificationOfEquipmentUnits && filters[23];
  }

  private static bool CheckAttackType(RecipeData item, bool[] filters)
  {
    if (!filters[16] && !filters[17] && !filters[18] && !filters[19] && !filters[20] && !filters[21])
      return true;
    if (item.combinedGear.kind.Enum != GearKindEnum.sword && item.combinedGear.kind.Enum != GearKindEnum.axe && item.combinedGear.kind.Enum != GearKindEnum.spear && item.combinedGear.kind.Enum != GearKindEnum.bow && item.combinedGear.kind.Enum != GearKindEnum.gun && item.combinedGear.kind.Enum != GearKindEnum.staff && item.combinedGear.kind.Enum != GearKindEnum.accessories && item.combinedGear.kind.Enum != GearKindEnum.shield && item.combinedGear.gearClassification == null)
      return filters[16];
    if (item.combinedGear.gearClassification == null)
      return false;
    switch (item.combinedGear.gearClassification.attack_classification)
    {
      case GearAttackClassification.none:
        return filters[16];
      case GearAttackClassification.slash:
        return filters[17];
      case GearAttackClassification.blow:
        return filters[18];
      case GearAttackClassification.pierce:
        return filters[19];
      case GearAttackClassification.shoot:
        return filters[20];
      case GearAttackClassification.magic:
        return filters[21];
      default:
        return false;
    }
  }

  public static IEnumerable<RecipeData> SortBy(
    this IEnumerable<RecipeData> self,
    RecipeSortAndFilter.SORT_TYPES sortType,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isRecipeExist)
  {
    switch (sortType)
    {
      case RecipeSortAndFilter.SORT_TYPES.Recommended:
        return self.SortByRecommended(order, isRecipeExist);
      case RecipeSortAndFilter.SORT_TYPES.BranchOfWeapon:
        return self.SortByBranchOfWeapon(order, isRecipeExist);
      case RecipeSortAndFilter.SORT_TYPES.Rarity:
        return self.SortByRarity(order, isRecipeExist);
      case RecipeSortAndFilter.SORT_TYPES.Name:
        return self.SortByName(order, isRecipeExist);
      default:
        return (IEnumerable<RecipeData>) self.ToList<RecipeData>();
    }
  }

  private static IOrderedEnumerable<RecipeData> RecipeExistFirst(this IEnumerable<RecipeData> self)
  {
    return self.OrderByDescending<RecipeData, bool>((Func<RecipeData, bool>) (x => x.isCombinEnable));
  }

  public static IEnumerable<RecipeData> SortByRecommended(
    this IEnumerable<RecipeData> self,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isRecipeExist)
  {
    IOrderedEnumerable<RecipeData> source = !isRecipeExist ? (order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? self.OrderByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.priority)) : self.OrderBy<RecipeData, int>((Func<RecipeData, int>) (x => x.priority))) : (order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? self.RecipeExistFirst().ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.priority)) : self.RecipeExistFirst().ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.priority)));
    return order == SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? (IEnumerable<RecipeData>) source.ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)).ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.resource_reference_gear_id_GearGear)).ToList<RecipeData>() : (IEnumerable<RecipeData>) source.ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)).ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.resource_reference_gear_id_GearGear)).ToList<RecipeData>();
  }

  public static IEnumerable<RecipeData> SortByBranchOfWeapon(
    this IEnumerable<RecipeData> self,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isRecipeExist)
  {
    IOrderedEnumerable<RecipeData> source = !isRecipeExist ? (order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? self.OrderByDescending<RecipeData, GearKindEnum>((Func<RecipeData, GearKindEnum>) (x => x.combinedGear.kind.Enum)) : self.OrderBy<RecipeData, GearKindEnum>((Func<RecipeData, GearKindEnum>) (x => x.combinedGear.kind.Enum))) : (order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? self.RecipeExistFirst().ThenByDescending<RecipeData, GearKindEnum>((Func<RecipeData, GearKindEnum>) (x => x.combinedGear.kind.Enum)) : self.RecipeExistFirst().ThenBy<RecipeData, GearKindEnum>((Func<RecipeData, GearKindEnum>) (x => x.combinedGear.kind.Enum)));
    return order == SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? (IEnumerable<RecipeData>) source.ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.resource_reference_gear_id_GearGear)).ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)).ToList<RecipeData>() : (IEnumerable<RecipeData>) source.ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.resource_reference_gear_id_GearGear)).ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)).ToList<RecipeData>();
  }

  public static IEnumerable<RecipeData> SortByRarity(
    this IEnumerable<RecipeData> self,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isRecipeExist)
  {
    IOrderedEnumerable<RecipeData> source = !isRecipeExist ? (order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? self.OrderBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)) : self.OrderByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index))) : (order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? self.RecipeExistFirst().ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)) : self.RecipeExistFirst().ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)));
    return order == SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? (IEnumerable<RecipeData>) source.ThenBy<RecipeData, GearKindEnum>((Func<RecipeData, GearKindEnum>) (x => x.combinedGear.kind.Enum)).ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.resource_reference_gear_id_GearGear)).ToList<RecipeData>() : (IEnumerable<RecipeData>) source.ThenByDescending<RecipeData, GearKindEnum>((Func<RecipeData, GearKindEnum>) (x => x.combinedGear.kind.Enum)).ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.resource_reference_gear_id_GearGear)).ToList<RecipeData>();
  }

  public static IEnumerable<RecipeData> SortByName(
    this IEnumerable<RecipeData> self,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isRecipeExist)
  {
    IOrderedEnumerable<RecipeData> source = !isRecipeExist ? (order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? self.OrderByDescending<RecipeData, string>((Func<RecipeData, string>) (x => x.combinedGear.name)) : self.OrderBy<RecipeData, string>((Func<RecipeData, string>) (x => x.combinedGear.name))) : (order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? self.RecipeExistFirst().ThenByDescending<RecipeData, string>((Func<RecipeData, string>) (x => x.combinedGear.name)) : self.RecipeExistFirst().ThenBy<RecipeData, string>((Func<RecipeData, string>) (x => x.combinedGear.name)));
    return order == SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? (IEnumerable<RecipeData>) source.ThenBy<RecipeData, GearKindEnum>((Func<RecipeData, GearKindEnum>) (x => x.combinedGear.kind.Enum)).ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.resource_reference_gear_id_GearGear)).ThenByDescending<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)).ToList<RecipeData>() : (IEnumerable<RecipeData>) source.ThenByDescending<RecipeData, GearKindEnum>((Func<RecipeData, GearKindEnum>) (x => x.combinedGear.kind.Enum)).ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.resource_reference_gear_id_GearGear)).ThenBy<RecipeData, int>((Func<RecipeData, int>) (x => x.combinedGear.rarity.index)).ToList<RecipeData>();
  }
}
