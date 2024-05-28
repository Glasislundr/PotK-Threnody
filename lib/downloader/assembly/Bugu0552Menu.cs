// Decompiled with JetBrains decompiler
// Type: Bugu0552Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Bugu0552Menu : Bugu005ItemListMenuBase
{
  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu0552SortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    PlayerItem[] source = SMManager.Get<PlayerItem[]>();
    List<PlayerItem> list1 = ((IEnumerable<PlayerItem>) source).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.isExchangable())).Distinct<PlayerItem>((IEqualityComparer<PlayerItem>) new LambdaEqualityComparer<PlayerItem>((Func<PlayerItem, PlayerItem, bool>) ((a, b) => a.gear.ID == b.gear.ID))).ToList<PlayerItem>();
    List<PlayerItem> list2 = ((IEnumerable<PlayerItem>) source).ToList<PlayerItem>();
    foreach (PlayerItem playerItem in list1)
    {
      PlayerItem exchangable = playerItem;
      exchangable.quantity = list2.Count<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear != null && x.gear.ID == exchangable.gear.ID));
      list2.RemoveAll((Predicate<PlayerItem>) (x => x.gear != null && x.gear.ID == exchangable.gear.ID));
      list2.Add(exchangable);
    }
    return list2;
  }

  protected override void UpdateInventoryItemList()
  {
    List<InventoryItem> list = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item != null && !x.removeButton)).ToList<InventoryItem>();
    if (list != null && list.Count<InventoryItem>() > 0)
    {
      PlayerItem[] source = SMManager.Get<PlayerItem[]>();
      foreach (InventoryItem inventoryItem in list)
      {
        InventoryItem invItem = inventoryItem;
        PlayerItem playerItem = ((IEnumerable<PlayerItem>) source).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == invItem.Item.itemID));
        if (playerItem != (PlayerItem) null)
          this.UpdateInvetoryItem(invItem, playerItem);
      }
    }
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
  }

  protected override void ChangeDetailScene(GameCore.ItemInfo item)
  {
    if (item == null)
      return;
    if (item.isWeapon)
      Unit05443Scene.changeScene(true, item);
    else
      Bugu05561Scene.changeScene(true, item);
  }
}
