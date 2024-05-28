// Decompiled with JetBrains decompiler
// Type: Bugu005SupplyListMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu005SupplyListMenu : Bugu005ItemListMenuBase
{
  private bool needClearCache = true;

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu005SupplyListSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    List<PlayerItem> list1 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.isSupply())).ToList<PlayerItem>();
    List<PlayerItem> list2 = list1.Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id == 1)).ToList<PlayerItem>();
    foreach (IGrouping<int, PlayerItem> grouping in list1.Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id != 1)).GroupBy<PlayerItem, int>((Func<PlayerItem, int>) (x => x.supply.ID)))
    {
      PlayerItem pi = grouping.FirstOrDefault<PlayerItem>();
      if (!(pi == (PlayerItem) null))
      {
        pi = pi.Clone();
        pi.quantity = 0;
        grouping.ForEach<PlayerItem>((Action<PlayerItem>) (item => pi.quantity += item.quantity));
        list2.Add(pi);
      }
    }
    return list2;
  }

  protected override void UpdateInventoryItemList()
  {
    List<InventoryItem> list1 = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
    {
      if (x.Item == null || x.removeButton)
        return false;
      return x.Item.isWeapon || x.Item.isSupply;
    })).ToList<InventoryItem>();
    if (list1 != null && list1.Count<InventoryItem>() > 0)
    {
      List<PlayerItem> itemList = this.GetItemList();
      foreach (InventoryItem inventoryItem in list1)
      {
        InventoryItem invItem = inventoryItem;
        PlayerItem playerItem = itemList.FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == invItem.Item.itemID));
        if (playerItem != (PlayerItem) null)
          this.UpdateInvetoryItem(invItem, playerItem);
      }
    }
    List<InventoryItem> list2 = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
    {
      if (x.Item == null || x.removeButton)
        return false;
      return x.Item.isCompse || x.Item.isExchangable;
    })).ToList<InventoryItem>();
    if (list2 != null && list2.Count<InventoryItem>() > 0)
    {
      PlayerMaterialGear[] source = SMManager.Get<PlayerMaterialGear[]>();
      foreach (InventoryItem inventoryItem in list2)
      {
        InventoryItem invItem = inventoryItem;
        PlayerMaterialGear playerMaterialGear = ((IEnumerable<PlayerMaterialGear>) source).FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.id == invItem.Item.itemID));
        if (playerMaterialGear != (PlayerMaterialGear) null)
          this.UpdateInvetoryItem(invItem, playerMaterialGear);
      }
    }
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
  }

  protected virtual void OnEnable()
  {
    if (!this.scroll.scrollView.isDragging)
      return;
    this.scroll.scrollView.Press(false);
  }

  public void onBackScene()
  {
    if (Object.op_Inequality((Object) this.SortPopupPrefab, (Object) null))
      this.SortPopupPrefab.GetComponent<ItemSortAndFilter>().Initialize((Bugu005ItemListMenuBase) this);
    float num = this.scroll.scrollView.verticalScrollBar.value;
    this.Sort(this.SortCategory, this.OrderBuySort, this.isEquipFirst);
    this.scroll.ResolvePosition(new Vector2(0.0f, num));
    this.needClearCache = true;
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Persist.sortOrder.Flush();
    if (!this.needClearCache)
      return;
    ItemIcon.ClearCache();
  }

  public void IbtnSell()
  {
    if (this.IsPushAndSet())
      return;
    this.needClearCache = false;
    Bugu00525Scene.ChangeScene(true, Bugu00525Scene.Mode.Supply);
  }
}
