// Decompiled with JetBrains decompiler
// Type: Earth.EarthItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace Earth
{
  [Serializable]
  public class EarthItem : BL.ModelBase
  {
    public const int BoxTypeID_Normal = 1;
    public const int BoxTypeID_Battle = 2;
    private const int SUPPLY_MAX_COUNT = 99;
    private int mItemID;
    private int mQuantity;
    private int mBattleSetCount;
    [NonSerialized]
    private List<PlayerItem> playerItems;
    [NonSerialized]
    private BL.BattleModified<EarthItem> mModified;
    private static readonly string serverDataFormat = "{{\"supply_id\":{0},\"quantity\":{1},\"battle_set_count\":{2}}}";

    public int ID => this.mItemID;

    public int quantity
    {
      get => this.mQuantity;
      set
      {
        if (this.mQuantity == value)
          return;
        this.mQuantity = Mathf.Max(0, value);
        this.commit();
      }
    }

    public int battleSetCount
    {
      get => this.mBattleSetCount;
      set
      {
        if (this.mBattleSetCount == value)
          return;
        this.mBattleSetCount = Mathf.Min(Mathf.Min(value, this.supply.battle_stack_limit), this.mQuantity);
        this.commit();
      }
    }

    public int itemID => this.mItemID;

    public SupplySupply supply => MasterData.SupplySupply[this.mItemID];

    public static EarthItem Create(int itemID, int quantity, int boxID = 1)
    {
      return new EarthItem()
      {
        mItemID = itemID,
        mQuantity = quantity,
        mBattleSetCount = 0
      };
    }

    public void UseItem(int count)
    {
      this.quantity = Mathf.Max(0, this.quantity - count);
      this.battleSetCount = Mathf.Max(0, this.battleSetCount - count);
    }

    private PlayerItem CreatePlayerItem(int boxTypeID)
    {
      PlayerItem forKey = PlayerItem.CreateForKey(EarthDataManager.GetAutoIndex());
      forKey._entity_type = 2;
      forKey.player_id = SMManager.Get<Player>().id;
      forKey.entity_id = this.mItemID;
      forKey.broken = false;
      forKey.favorite = false;
      forKey.is_new = false;
      forKey.for_battle = false;
      forKey.box_type_id = boxTypeID;
      forKey.quantity = 0;
      return forKey;
    }

    public List<PlayerItem> GetPlayerItemList()
    {
      if (this.playerItems != null && !this.mModified.isChangedOnce())
        return this.playerItems;
      if (this.playerItems == null)
      {
        this.playerItems = new List<PlayerItem>();
        this.mModified = new BL.BattleModified<EarthItem>(this);
      }
      if (this.mBattleSetCount > 0)
      {
        PlayerItem playerItem = this.playerItems.FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id == 2));
        if (playerItem == (PlayerItem) null)
        {
          playerItem = this.CreatePlayerItem(2);
          this.playerItems.Add(playerItem);
        }
        playerItem.quantity = this.mBattleSetCount;
      }
      else
      {
        PlayerItem playerItem = this.playerItems.FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id == 2));
        if (playerItem != (PlayerItem) null)
          this.playerItems.Remove(playerItem);
      }
      int num = this.mQuantity - this.mBattleSetCount;
      if (num > 0)
      {
        List<PlayerItem> playerItemList = new List<PlayerItem>();
        foreach (PlayerItem playerItem in (IEnumerable<PlayerItem>) this.playerItems.Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id == 1)).OrderByDescending<PlayerItem, int>((Func<PlayerItem, int>) (x => x.quantity)))
        {
          if (num > 0)
          {
            playerItem.quantity = Mathf.Min(num, 99);
            num -= playerItem.quantity;
          }
          else
            playerItemList.Add(playerItem);
        }
        foreach (PlayerItem playerItem in playerItemList)
          this.playerItems.Remove(playerItem);
        if (num > 0)
        {
          do
          {
            PlayerItem playerItem = this.CreatePlayerItem(1);
            playerItem.quantity = Mathf.Min(num, 99);
            this.playerItems.Add(playerItem);
            num -= playerItem.quantity;
          }
          while (num > 0);
        }
      }
      else
      {
        foreach (PlayerItem playerItem in this.playerItems.Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id == 1)).ToList<PlayerItem>())
          this.playerItems.Remove(playerItem);
      }
      return this.playerItems;
    }

    public string GetSeverString()
    {
      return string.Format(EarthItem.serverDataFormat, (object) this.mItemID, (object) this.mQuantity, (object) this.mBattleSetCount);
    }

    public static EarthItem JsonLoad(Dictionary<string, object> json)
    {
      return new EarthItem()
      {
        mItemID = (int) (long) json["supply_id"],
        mQuantity = (int) (long) json["quantity"],
        mBattleSetCount = (int) (long) json["battle_set_count"]
      };
    }
  }
}
