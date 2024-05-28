// Decompiled with JetBrains decompiler
// Type: InventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using UnityEngine;

#nullable disable
public class InventoryItem
{
  public static readonly int GearRankMax = 10;
  private static readonly int BrokenSellValue = 1;
  public GameCore.ItemInfo Item;
  public ItemIcon icon;
  public bool select;
  public int index;
  public int selectCount;
  public bool Gray;
  public bool removeButton;

  public InventoryItem() => this.removeButton = true;

  public InventoryItem(GameCore.ItemInfo item) => this.Item = item;

  public InventoryItem(PlayerItem playerItem, bool bExpireDate = false, Func<bool> checkEquipped = null)
  {
    this.Item = new GameCore.ItemInfo(playerItem, bExpireDate, checkEquipped);
  }

  public InventoryItem(PlayerMaterialGear playerItem, int sameIndex = 0)
  {
    this.Item = new GameCore.ItemInfo(playerItem, sameIndex);
  }

  public string GetName() => this.Item.Name();

  public string GetDescription() => this.Item.Description();

  public long GetSingleSellPrice() => this.Item.SellPrice();

  public long GetSellPrice()
  {
    if (!this.select)
      return 0;
    long sellPrice;
    if (this.Item.itemType != GameCore.ItemInfo.ItemType.Supply)
    {
      if (this.Item.broken)
      {
        sellPrice = (long) InventoryItem.BrokenSellValue;
      }
      else
      {
        float num = this.Item.gearLevel == 0 ? 1f : 1f * (float) this.Item.gearLevel;
        sellPrice = this.Item.itemType != GameCore.ItemInfo.ItemType.Exchangable ? (this.Item.itemType != GameCore.ItemInfo.ItemType.Gear ? (long) ((double) this.Item.SellPrice() * (double) num) * (long) this.selectCount : (long) ((double) this.Item.SellPrice() * (double) num)) : this.Item.SellPrice() * (long) this.selectCount;
      }
    }
    else
      sellPrice = this.Item.SellPrice() * (long) this.selectCount;
    return sellPrice;
  }

  public int GetRepairPrice(int zenyBoostCntCnt = 0)
  {
    int repairPrice = this.Item.RepairPrice();
    if (zenyBoostCntCnt > 0)
      repairPrice *= zenyBoostCntCnt + 1;
    return repairPrice;
  }

  public Future<Sprite> LoadSpriteThumbnail() => this.Item.LoadSpriteThumbnail();
}
