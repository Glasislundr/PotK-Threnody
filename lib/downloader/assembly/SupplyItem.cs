// Decompiled with JetBrains decompiler
// Type: SupplyItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class SupplyItem
{
  public readonly SupplySupply Supply;
  public int SelectCount;
  public int DeckIndex;
  public bool Gray;
  public ItemIcon icon;
  public bool removeButton;
  private int itemQuantity;

  public SupplyItem(SupplySupply supply, int itemQuantity_)
  {
    this.Supply = supply;
    this.itemQuantity = itemQuantity_;
  }

  public SupplyItem(PlayerItem item, int itemQuantity_)
    : this(item.supply, itemQuantity_)
  {
  }

  public bool IsShowDeck => this.DeckIndex > 0;

  public int Count => this.ItemQuantity - this.SelectCount;

  public int RestCount => Math.Max(0, this.ItemQuantity - this.SelectCount);

  public int ItemQuantity
  {
    get => this.itemQuantity;
    set => this.itemQuantity = value;
  }

  public SupplyItem Copy()
  {
    return new SupplyItem(this.Supply, this.ItemQuantity)
    {
      DeckIndex = this.DeckIndex,
      SelectCount = this.SelectCount
    };
  }

  public static SupplyItem[] Merge(PlayerItem[] basePlayerItems, PlayerItem[] deckPlayerItems)
  {
    Dictionary<int, SupplyItem> dictionary = new Dictionary<int, SupplyItem>();
    foreach (PlayerItem basePlayerItem in basePlayerItems)
    {
      int entityId = basePlayerItem.entity_id;
      if (dictionary.ContainsKey(entityId))
        dictionary[entityId].itemQuantity += basePlayerItem.quantity;
      else
        dictionary[entityId] = new SupplyItem(basePlayerItem.supply, basePlayerItem.quantity);
    }
    int num = 0;
    foreach (PlayerItem deckPlayerItem in deckPlayerItems)
    {
      int entityId = deckPlayerItem.entity_id;
      if (dictionary.ContainsKey(entityId))
        dictionary[entityId].itemQuantity += deckPlayerItem.quantity;
      else
        dictionary[entityId] = new SupplyItem(deckPlayerItem.supply, deckPlayerItem.quantity);
      dictionary[entityId].SelectCount += deckPlayerItem.quantity;
      dictionary[entityId].DeckIndex = ++num;
    }
    return dictionary.Values.ToArray<SupplyItem>();
  }

  public SupplyItem[] ShowSupplyItems()
  {
    List<SupplyItem> supplyItemList = new List<SupplyItem>();
    if (this.Count > 0)
      supplyItemList.Add(new SupplyItem(this.Supply, this.Count));
    return supplyItemList.ToArray();
  }
}
