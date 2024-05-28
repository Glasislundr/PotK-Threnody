// Decompiled with JetBrains decompiler
// Type: InventoryItemExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class InventoryItemExtension
{
  private static bool isEquipFirst = true;
  private static SortAndFilter.SORT_TYPE_ORDER_BUY orderBuy;

  public static InventoryItem FindByItem(this List<InventoryItem> invItem, GameCore.ItemInfo findItem)
  {
    return invItem.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item != null && x.Item.itemID == findItem.itemID));
  }

  public static InventoryItem FindByItemIndex(this List<InventoryItem> invItem, GameCore.ItemInfo findItem)
  {
    return invItem.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item != null && x.Item.itemID == findItem.itemID && x.Item.sameItemIdx == findItem.sameItemIdx));
  }

  public static List<int> ToGearId(this List<InventoryItem> xs)
  {
    List<int> gearId = new List<int>();
    foreach (InventoryItem inventoryItem in xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Gear)))
      gearId.Add(inventoryItem.Item.itemID);
    return gearId;
  }

  public static List<int> ToMaterialId(this List<InventoryItem> xs)
  {
    IEnumerable<InventoryItem> inventoryItems = xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Compse || x.Item.itemType == GameCore.ItemInfo.ItemType.Exchangable || x.Item.itemType == GameCore.ItemInfo.ItemType.WeaponMaterial));
    List<int> materialId = new List<int>();
    foreach (InventoryItem inventoryItem in inventoryItems)
      materialId.Add(inventoryItem.Item.itemID);
    return materialId;
  }

  public static List<int> ToMaterialCounts(this List<InventoryItem> xs)
  {
    IEnumerable<InventoryItem> inventoryItems = xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Compse || x.Item.itemType == GameCore.ItemInfo.ItemType.Exchangable || x.Item.itemType == GameCore.ItemInfo.ItemType.WeaponMaterial));
    List<int> materialCounts = new List<int>();
    foreach (InventoryItem inventoryItem in inventoryItems)
      materialCounts.Add(inventoryItem.Item.tempSelectedCount);
    return materialCounts;
  }

  public static List<int> ToNotSupplyID(this List<InventoryItem> xs)
  {
    List<int> notSupplyId = new List<int>();
    foreach (InventoryItem inventoryItem in xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Gear)))
      notSupplyId.Add(inventoryItem.Item.itemID);
    PlayerItem[] source = SMManager.Get<PlayerItem[]>();
    foreach (InventoryItem inventoryItem in xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Compse || x.Item.itemType == GameCore.ItemInfo.ItemType.Exchangable)))
    {
      InventoryItem s = inventoryItem;
      List<PlayerItem> list = ((IEnumerable<PlayerItem>) source).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear != null && x.gear.ID == s.Item.masterID)).ToList<PlayerItem>();
      for (int index = 0; index < s.selectCount; ++index)
        notSupplyId.Add(list[index].id);
    }
    return notSupplyId;
  }

  public static List<int> ToEntityIdBySupply(this List<InventoryItem> xs)
  {
    IEnumerable<InventoryItem> inventoryItems = xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Supply));
    List<int> entityIdBySupply = new List<int>();
    foreach (InventoryItem inventoryItem in inventoryItems)
      entityIdBySupply.Add(inventoryItem.Item.masterID);
    return entityIdBySupply;
  }

  public static List<int> ToSelectQuantityBySupply(this List<InventoryItem> xs)
  {
    IEnumerable<InventoryItem> inventoryItems = xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Supply));
    List<int> quantityBySupply = new List<int>();
    foreach (InventoryItem inventoryItem in inventoryItems)
      quantityBySupply.Add(inventoryItem.selectCount);
    return quantityBySupply;
  }

  public static List<int> ToEntityIdByMaterial(this List<InventoryItem> xs)
  {
    IEnumerable<InventoryItem> inventoryItems = xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Compse || x.Item.itemType == GameCore.ItemInfo.ItemType.Exchangable || x.Item.itemType == GameCore.ItemInfo.ItemType.WeaponMaterial));
    List<int> entityIdByMaterial = new List<int>();
    foreach (InventoryItem inventoryItem in inventoryItems)
      entityIdByMaterial.Add(inventoryItem.Item.masterID);
    return entityIdByMaterial;
  }

  public static List<long> ToSelectQuantityByMaterial(this List<InventoryItem> xs)
  {
    IEnumerable<InventoryItem> inventoryItems = xs.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemType == GameCore.ItemInfo.ItemType.Compse || x.Item.itemType == GameCore.ItemInfo.ItemType.Exchangable || x.Item.itemType == GameCore.ItemInfo.ItemType.WeaponMaterial));
    List<long> quantityByMaterial = new List<long>();
    foreach (InventoryItem inventoryItem in inventoryItems)
      quantityByMaterial.Add((long) inventoryItem.selectCount);
    return quantityByMaterial;
  }

  private static int GetSortRank(GameCore.ItemInfo item)
  {
    return !item.gear.kind.isEquip || item.gear.disappearance_type_GearDisappearanceType != 0 ? item.gear.kind_GearKind : 0;
  }

  public static IEnumerable<InventoryItem> SortBy(
    this IEnumerable<InventoryItem> self,
    ItemSortAndFilter.SORT_TYPES sortType,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool equipFirst)
  {
    InventoryItem inventoryItem1 = (InventoryItem) null;
    List<InventoryItem> source = new List<InventoryItem>();
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    InventoryItemExtension.isEquipFirst = equipFirst;
    InventoryItemExtension.orderBuy = order;
    foreach (InventoryItem inventoryItem2 in self)
    {
      if (inventoryItem2.removeButton)
        inventoryItem1 = inventoryItem2;
      else
        source.Add(inventoryItem2);
    }
    switch (sortType)
    {
      case ItemSortAndFilter.SORT_TYPES.BranchOfWeapon:
        inventoryItemList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>();
        goto case ItemSortAndFilter.SORT_TYPES.Attribute;
      case ItemSortAndFilter.SORT_TYPES.Attribute:
      case ItemSortAndFilter.SORT_TYPES.Favorite:
      case ItemSortAndFilter.SORT_TYPES.Category:
        if (inventoryItem1 != null)
          inventoryItemList.Insert(0, inventoryItem1);
        return (IEnumerable<InventoryItem>) inventoryItemList;
      case ItemSortAndFilter.SORT_TYPES.Rarity:
        inventoryItemList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.rarity.index : 0)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.rarity.index : 0)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>();
        goto case ItemSortAndFilter.SORT_TYPES.Attribute;
      case ItemSortAndFilter.SORT_TYPES.Rank:
        inventoryItemList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>();
        goto case ItemSortAndFilter.SORT_TYPES.Attribute;
      case ItemSortAndFilter.SORT_TYPES.RankMax:
        inventoryItemList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevelLimit() : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevelLimit() : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>();
        goto case ItemSortAndFilter.SORT_TYPES.Attribute;
      case ItemSortAndFilter.SORT_TYPES.GetOrder:
        inventoryItemList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ToList<InventoryItem>();
        goto case ItemSortAndFilter.SORT_TYPES.Attribute;
      case ItemSortAndFilter.SORT_TYPES.Name:
        inventoryItemList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenByDescending<InventoryItem, string>((Func<InventoryItem, string>) (x => x.Item.Name().Split('+')[0])).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenBy<InventoryItem, string>((Func<InventoryItem, string>) (x => x.Item.Name().Split('+')[0])).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>();
        goto case ItemSortAndFilter.SORT_TYPES.Attribute;
      case ItemSortAndFilter.SORT_TYPES.HistoryGroupNumber:
        inventoryItemList = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null && x.Item.gear.history_group_number != 0 ? x.Item.gear.history_group_number : 0)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null && x.Item.gear.history_group_number != 0 ? x.Item.gear.history_group_number : 99999)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.kind_GearKind : int.MaxValue)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.GetSortLevel() : x.Item.supply.ID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>();
        goto case ItemSortAndFilter.SORT_TYPES.Attribute;
      case ItemSortAndFilter.SORT_TYPES.CallItem:
        inventoryItemList = source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.rarity.index : 0)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear != null ? x.Item.gear.ID : x.Item.supply.ID)).ToList<InventoryItem>();
        goto case ItemSortAndFilter.SORT_TYPES.Attribute;
      default:
        throw new Exception();
    }
  }

  private static IOrderedEnumerable<InventoryItem> EquipFirst(
    this IOrderedEnumerable<InventoryItem> ordered)
  {
    IOrderedEnumerable<InventoryItem> orderedEnumerable = ordered;
    if (InventoryItemExtension.isEquipFirst)
      orderedEnumerable = ordered.ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.ForBattle));
    return orderedEnumerable;
  }

  public static IEnumerable<InventoryItem> ReisouSortBy(
    this IEnumerable<InventoryItem> self,
    ReisouSortAndFilter.SORT_TYPES sortType,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool equipFirst)
  {
    InventoryItem inventoryItem1 = (InventoryItem) null;
    List<InventoryItem> source = new List<InventoryItem>();
    List<InventoryItem> inventoryItemList1 = new List<InventoryItem>();
    InventoryItemExtension.isEquipFirst = equipFirst;
    InventoryItemExtension.orderBuy = order;
    foreach (InventoryItem inventoryItem2 in self)
    {
      if (inventoryItem2.removeButton)
        inventoryItem1 = inventoryItem2;
      else
        source.Add(inventoryItem2);
    }
    List<InventoryItem> inventoryItemList2;
    switch (sortType)
    {
      case ReisouSortAndFilter.SORT_TYPES.BranchOfWeapon:
        inventoryItemList2 = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isReisou)).ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.AwakeReisouSkill)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.kind_GearKind)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.GetSortLevel())).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.ID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isReisou)).ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.AwakeReisouSkill)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.kind_GearKind)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.GetSortLevel())).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.ID)).ToList<InventoryItem>();
        break;
      case ReisouSortAndFilter.SORT_TYPES.Rarity:
        inventoryItemList2 = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isReisou)).ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.AwakeReisouSkill)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.GetSortLevel())).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.ID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isReisou)).ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.AwakeReisouSkill)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.GetSortLevel())).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.ID)).ToList<InventoryItem>();
        break;
      case ReisouSortAndFilter.SORT_TYPES.Rank:
        inventoryItemList2 = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isReisou)).ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.AwakeReisouSkill)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.GetSortLevel())).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.ID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isReisou)).ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.AwakeReisouSkill)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.GetSortLevel())).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.ID)).ToList<InventoryItem>();
        break;
      case ReisouSortAndFilter.SORT_TYPES.GetOrder:
        inventoryItemList2 = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isReisou)).ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.AwakeReisouSkill)).EquipFirst().ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.GetSortLevel())).ThenBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.ID)).ToList<InventoryItem>() : source.OrderByDescending<InventoryItem, bool>((Func<InventoryItem, bool>) (x => x.Item.isReisou)).ThenBy<InventoryItem, bool>((Func<InventoryItem, bool>) (x => !x.Item.AwakeReisouSkill)).EquipFirst().ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.GetSortLevel())).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.ID)).ToList<InventoryItem>();
        break;
      default:
        throw new Exception();
    }
    if (inventoryItem1 != null)
      inventoryItemList2.Insert(0, inventoryItem1);
    return (IEnumerable<InventoryItem>) inventoryItemList2;
  }

  public static IEnumerable<InventoryItem> FilterBy(
    this IEnumerable<InventoryItem> self,
    bool[] filters)
  {
    InventoryItem inventoryItem1 = (InventoryItem) null;
    List<InventoryItem> inventoryItemList1 = new List<InventoryItem>();
    List<InventoryItem> inventoryItemList2 = new List<InventoryItem>();
    foreach (InventoryItem inventoryItem2 in self)
    {
      if (inventoryItem2.removeButton)
        inventoryItem1 = inventoryItem2;
      else
        inventoryItemList1.Add(inventoryItem2);
    }
    for (int index = 0; index < inventoryItemList1.Count; ++index)
    {
      InventoryItem inventoryItem3 = inventoryItemList1[index];
      if (InventoryItemExtension.CheckWeaponType(inventoryItem3, filters) && InventoryItemExtension.CheckRarity(inventoryItem3, filters) && InventoryItemExtension.CheckFavorite(inventoryItem3, filters) && InventoryItemExtension.CheckLevel(inventoryItem3, filters) && InventoryItemExtension.CheckEquip(inventoryItem3, filters) && InventoryItemExtension.CheckIsReisou(inventoryItem3, filters) && InventoryItemExtension.CheckAttackType(inventoryItem3, filters) && (!filters[24] && !filters[25] && !filters[9] && !filters[23] && !filters[28] && !filters[30] && !filters[40] || InventoryItemExtension.CheckWeapon(inventoryItem3, filters) || InventoryItemExtension.CheckSpecialWeapon(inventoryItem3, filters) || InventoryItemExtension.CheckWeaponSmith(inventoryItem3, filters) || InventoryItemExtension.CheckAlchemist(inventoryItem3, filters) || InventoryItemExtension.CheckMoney(inventoryItem3, filters) || InventoryItemExtension.CheckWeaponMaterial(inventoryItem3, filters) || InventoryItemExtension.CheckPresent(inventoryItem3, filters)))
        inventoryItemList2.Add(inventoryItem3);
    }
    if (inventoryItem1 != null)
      inventoryItemList2.Insert(0, inventoryItem1);
    return (IEnumerable<InventoryItem>) inventoryItemList2;
  }

  private static bool CheckFavorite(InventoryItem item, bool[] filters)
  {
    if (!filters[26] && !filters[27])
      return true;
    return item.Item.favorite ? filters[26] : filters[27];
  }

  private static bool checkAllEnumStateEqual(bool[] blist)
  {
    for (int index = 0; index < blist.Length; ++index)
    {
      if (blist[index] != blist[0])
        return false;
    }
    return true;
  }

  private static bool CheckWeaponType(InventoryItem item, bool[] filters)
  {
    if (InventoryItemExtension.checkAllEnumStateEqual(new bool[8]
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
    bool flag = false;
    if (item.Item.gear == null)
    {
      if (!Singleton<NGGameDataManager>.GetInstance().IsEarth)
        flag = true;
      return flag;
    }
    switch (item.Item.gear.kind.Enum)
    {
      case GearKindEnum.sword:
        flag = filters[1];
        break;
      case GearKindEnum.axe:
        flag = filters[2];
        break;
      case GearKindEnum.spear:
        flag = filters[3];
        break;
      case GearKindEnum.bow:
        flag = filters[4];
        break;
      case GearKindEnum.gun:
        flag = filters[5];
        break;
      case GearKindEnum.staff:
        flag = filters[6];
        break;
      case GearKindEnum.shield:
        flag = filters[7];
        break;
      case GearKindEnum.accessories:
        flag = filters[8];
        break;
    }
    return flag;
  }

  private static bool CheckAlchemist(InventoryItem item, bool[] filters)
  {
    return item.Item.isDrilling && filters[28];
  }

  private static bool CheckWeaponSmith(InventoryItem item, bool[] filters)
  {
    return item.Item.gear != null && item.Item.gear.kind.Enum == GearKindEnum.smith && !item.Item.isExchangable && filters[9];
  }

  private static bool CheckMoney(InventoryItem item, bool[] filters)
  {
    bool flag = false;
    if (item.Item.isExchangable)
      flag = filters[23];
    return flag;
  }

  private static bool CheckWeaponElement(InventoryItem item, bool[] filters)
  {
    bool flag = false;
    if (item.Item.gear == null)
      return true;
    switch (item.Item.gear.GetElement())
    {
      case CommonElement.none:
        flag = filters[10];
        break;
      case CommonElement.fire:
        flag = filters[11];
        break;
      case CommonElement.wind:
        flag = filters[12];
        break;
      case CommonElement.thunder:
        flag = filters[13];
        break;
      case CommonElement.ice:
        flag = filters[14];
        break;
      case CommonElement.light:
        flag = filters[15];
        break;
      case CommonElement.dark:
        flag = filters[16];
        break;
    }
    return flag;
  }

  private static bool CheckRarity(InventoryItem item, bool[] filters)
  {
    if (!filters[17] && !filters[18] && !filters[19] && !filters[20] && !filters[21] && !filters[22] && !filters[31])
      return true;
    bool flag = false;
    if (item.Item.gear == null)
      return true;
    switch (item.Item.gear.rarity.index)
    {
      case 1:
        flag = filters[17];
        break;
      case 2:
        flag = filters[18];
        break;
      case 3:
        flag = filters[19];
        break;
      case 4:
        flag = filters[20];
        break;
      case 5:
        flag = filters[21];
        break;
      case 6:
        flag = filters[22];
        break;
      case 7:
        flag = filters[31];
        break;
    }
    return flag;
  }

  private static bool CheckWeapon(InventoryItem item, bool[] filters)
  {
    if (filters[24] && filters[25] && item.Item.itemType == GameCore.ItemInfo.ItemType.Gear)
      return true;
    return item.Item.itemType == GameCore.ItemInfo.ItemType.Gear && !item.Item.gear.hasSpecificationOfEquipmentUnits && filters[24];
  }

  private static bool CheckSpecialWeapon(InventoryItem item, bool[] filters)
  {
    if (filters[24] && filters[25] && item.Item.itemType == GameCore.ItemInfo.ItemType.Gear)
      return true;
    return item.Item.itemType == GameCore.ItemInfo.ItemType.Gear && item.Item.gear.hasSpecificationOfEquipmentUnits && filters[25];
  }

  private static bool CheckDrilled(InventoryItem item, bool[] filters)
  {
    return filters[29] || item.Item.gear == null || item.Item.gearExp < 1 && item.Item.gearLevel <= 1;
  }

  private static bool CheckWeaponMaterial(InventoryItem item, bool[] filters)
  {
    return item.Item.isWeaponMaterial && filters[30];
  }

  private static bool CheckLevel(InventoryItem item, bool[] filters)
  {
    if (!filters[32] && !filters[33])
      return true;
    return item.Item.gearLevel >= InventoryItem.GearRankMax && item.Item.itemType == GameCore.ItemInfo.ItemType.Gear ? filters[32] : filters[33];
  }

  private static bool CheckPresent(InventoryItem item, bool[] filters)
  {
    return item.Item.gear != null && item.Item.gear.kind.Enum == GearKindEnum.sea_present && filters[40];
  }

  private static bool CheckEquip(InventoryItem item, bool[] filters)
  {
    if (!filters[41] && !filters[42])
      return true;
    return item.Item.ForBattle ? filters[41] : filters[42];
  }

  private static bool CheckIsReisou(InventoryItem item, bool[] filters)
  {
    if (!filters[43] && !filters[44])
      return true;
    if (item.Item.isWeapon)
      return filters[43];
    return item.Item.isReisou && filters[44];
  }

  private static bool CheckAttackType(InventoryItem item, bool[] filters)
  {
    if (!filters[34] && !filters[35] && !filters[36] && !filters[37] && !filters[38] && !filters[39])
      return true;
    bool flag = false;
    if (item.Item.gear == null)
      return false;
    if (item.Item.gear.kind.Enum != GearKindEnum.sword && item.Item.gear.kind.Enum != GearKindEnum.axe && item.Item.gear.kind.Enum != GearKindEnum.spear && item.Item.gear.kind.Enum != GearKindEnum.bow && item.Item.gear.kind.Enum != GearKindEnum.gun && item.Item.gear.kind.Enum != GearKindEnum.staff && item.Item.gear.kind.Enum != GearKindEnum.accessories && item.Item.gear.kind.Enum != GearKindEnum.shield && item.Item.gear.gearClassification == null)
      return filters[34];
    if (item.Item.gear.gearClassification == null)
      return false;
    switch (item.Item.gear.gearClassification.attack_classification)
    {
      case GearAttackClassification.none:
        flag = filters[34];
        break;
      case GearAttackClassification.slash:
        flag = filters[35];
        break;
      case GearAttackClassification.blow:
        flag = filters[36];
        break;
      case GearAttackClassification.pierce:
        flag = filters[37];
        break;
      case GearAttackClassification.shoot:
        flag = filters[38];
        break;
      case GearAttackClassification.magic:
        flag = filters[39];
        break;
    }
    return flag;
  }

  public static IEnumerable<InventoryItem> ReisouFilterBy(
    this IEnumerable<InventoryItem> self,
    bool[] filters)
  {
    InventoryItem inventoryItem1 = (InventoryItem) null;
    List<InventoryItem> inventoryItemList1 = new List<InventoryItem>();
    List<InventoryItem> inventoryItemList2 = new List<InventoryItem>();
    foreach (InventoryItem inventoryItem2 in self)
    {
      if (inventoryItem2.removeButton)
        inventoryItem1 = inventoryItem2;
      else
        inventoryItemList1.Add(inventoryItem2);
    }
    for (int index = 0; index < inventoryItemList1.Count; ++index)
    {
      InventoryItem inventoryItem3 = inventoryItemList1[index];
      if (InventoryItemExtension.CheckReisouWeaponType(inventoryItem3, filters) && InventoryItemExtension.CheckReisouRarity(inventoryItem3, filters) && InventoryItemExtension.CheckReisouEquip(inventoryItem3, filters) && InventoryItemExtension.CheckReisouDrilled(inventoryItem3, filters) && InventoryItemExtension.CheckReisouType(inventoryItem3, filters))
        inventoryItemList2.Add(inventoryItem3);
    }
    if (inventoryItem1 != null)
      inventoryItemList2.Insert(0, inventoryItem1);
    return (IEnumerable<InventoryItem>) inventoryItemList2;
  }

  private static bool CheckReisouWeaponType(InventoryItem item, bool[] filters)
  {
    if (InventoryItemExtension.checkAllEnumStateEqual(new bool[8]
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
    bool flag = false;
    switch (item.Item.gear.kind.Enum)
    {
      case GearKindEnum.sword:
        flag = filters[1];
        break;
      case GearKindEnum.axe:
        flag = filters[2];
        break;
      case GearKindEnum.spear:
        flag = filters[3];
        break;
      case GearKindEnum.bow:
        flag = filters[4];
        break;
      case GearKindEnum.gun:
        flag = filters[5];
        break;
      case GearKindEnum.staff:
        flag = filters[6];
        break;
      case GearKindEnum.shield:
        flag = filters[7];
        break;
      case GearKindEnum.accessories:
        flag = filters[8];
        break;
    }
    return flag;
  }

  private static bool CheckReisouRarity(InventoryItem item, bool[] filters)
  {
    if (!filters[9] && !filters[10] && !filters[11] && !filters[12] && !filters[13] && !filters[14] && !filters[15])
      return true;
    bool flag = false;
    switch (item.Item.gear.rarity.index)
    {
      case 1:
        flag = filters[9];
        break;
      case 2:
        flag = filters[10];
        break;
      case 3:
        flag = filters[11];
        break;
      case 4:
        flag = filters[12];
        break;
      case 5:
        flag = filters[13];
        break;
      case 6:
        flag = filters[14];
        break;
      case 7:
        flag = filters[15];
        break;
    }
    return flag;
  }

  private static bool CheckReisouEquip(InventoryItem item, bool[] filters)
  {
    if (!filters[21] && !filters[22])
      return true;
    return item.Item.ForBattle ? filters[21] : filters[22];
  }

  private static bool CheckReisouDrilled(InventoryItem item, bool[] filters)
  {
    if (!filters[20] && !filters[19])
      return true;
    if (item.Item.playerItem.isMythologyReisou())
    {
      PlayerMythologyGearStatus mythologyGearStatus = item.Item.playerItem.GetPlayerMythologyGearStatus();
      return mythologyGearStatus.holy_gear_exp >= 1 || mythologyGearStatus.holy_gear_level > 1 || mythologyGearStatus.chaos_gear_exp >= 1 || mythologyGearStatus.chaos_gear_level > 1 ? filters[20] : filters[19];
    }
    return item.Item.gearExp >= 1 || item.Item.gearLevel > 1 ? filters[20] : filters[19];
  }

  private static bool CheckReisouType(InventoryItem item, bool[] filters)
  {
    if (InventoryItemExtension.checkAllEnumStateEqual(new bool[3]
    {
      filters[16],
      filters[17],
      filters[18]
    }))
      return true;
    bool flag = false;
    switch (item.Item.gear.reisou_type_GearReisouType)
    {
      case 1:
        flag = filters[16];
        break;
      case 2:
        flag = filters[17];
        break;
      case 3:
        flag = filters[18];
        break;
    }
    return flag;
  }
}
