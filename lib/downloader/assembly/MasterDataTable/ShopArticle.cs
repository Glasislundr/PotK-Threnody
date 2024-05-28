// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ShopArticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ShopArticle
  {
    public int ID;
    public string name;
    public string description;
    public int shop_ShopShop;
    public int view_order;
    public int pay_type_CommonPayType;
    public int? pay_id;
    public int price;
    public int? limit;
    public int? daily_limit;
    public DateTime? end_at;

    public static ShopArticle Parse(MasterDataReader reader)
    {
      return new ShopArticle()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        shop_ShopShop = reader.ReadInt(),
        view_order = reader.ReadInt(),
        pay_type_CommonPayType = reader.ReadInt(),
        pay_id = reader.ReadIntOrNull(),
        price = reader.ReadInt(),
        limit = reader.ReadIntOrNull(),
        daily_limit = reader.ReadIntOrNull(),
        end_at = reader.ReadDateTimeOrNull()
      };
    }

    public ShopShop shop
    {
      get
      {
        ShopShop shop;
        if (!MasterData.ShopShop.TryGetValue(this.shop_ShopShop, out shop))
          Debug.LogError((object) ("Key not Found: MasterData.ShopShop[" + (object) this.shop_ShopShop + "]"));
        return shop;
      }
    }

    public CommonPayType pay_type => (CommonPayType) this.pay_type_CommonPayType;

    public ShopContent[] ShopContents
    {
      get
      {
        return ((IEnumerable<ShopContent>) MasterData.ShopContentList).Where<ShopContent>((Func<ShopContent, bool>) (x => x.article.ID == this.ID)).ToArray<ShopContent>();
      }
    }

    public Future<Sprite> LoadSpriteThumbnail()
    {
      ShopContent shopContent = this.ShopContents[0];
      if (shopContent.entity_type == CommonRewardType.supply)
        return MasterData.SupplySupply[shopContent.entity_id].LoadSpriteThumbnail();
      if (shopContent.entity_type == CommonRewardType.gear)
        return MasterData.GearGear[shopContent.entity_id].LoadSpriteThumbnail();
      if (shopContent.entity_type == CommonRewardType.unit || shopContent.entity_type == CommonRewardType.material_unit)
        return MasterData.UnitUnit[shopContent.entity_id].LoadSpriteThumbnail();
      return shopContent.entity_type == CommonRewardType.quest_key ? MasterData.QuestkeyQuestkey[shopContent.entity_id].LoadSpriteThumbnail() : Singleton<ResourceManager>.GetInstance().Load<Sprite>("Sprites/1x1_pink");
    }
  }
}
