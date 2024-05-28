// Decompiled with JetBrains decompiler
// Type: MasterDataTable.EarthShopArticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class EarthShopArticle
  {
    public int ID;
    public string name;
    public string description;
    public int view_order;
    public int pay_type_CommonPayType;
    public int price;
    public bool disablel_if_sold_out;
    public string unit;
    public int? limit;
    public string disable_until_purchase_on_article_id;
    public string disable_until_sold_out_article_id;
    public int? start_id;
    public int? end_id;

    public static EarthShopArticle Parse(MasterDataReader reader)
    {
      return new EarthShopArticle()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        description = reader.ReadString(true),
        view_order = reader.ReadInt(),
        pay_type_CommonPayType = reader.ReadInt(),
        price = reader.ReadInt(),
        disablel_if_sold_out = reader.ReadBool(),
        unit = reader.ReadString(true),
        limit = reader.ReadIntOrNull(),
        disable_until_purchase_on_article_id = reader.ReadString(true),
        disable_until_sold_out_article_id = reader.ReadString(true),
        start_id = reader.ReadIntOrNull(),
        end_id = reader.ReadIntOrNull()
      };
    }

    public CommonPayType pay_type => (CommonPayType) this.pay_type_CommonPayType;

    public EarthShopContent ShopContents
    {
      get
      {
        return ((IEnumerable<EarthShopContent>) MasterData.EarthShopContentList).FirstOrDefault<EarthShopContent>((Func<EarthShopContent, bool>) (x => x.article.ID == this.ID));
      }
    }

    public bool IsDisableUntilPurchaseOnArticleID
    {
      get
      {
        if (string.IsNullOrEmpty(this.disable_until_purchase_on_article_id))
          return true;
        string[] strArray = this.disable_until_purchase_on_article_id.Split(',');
        EarthDataManager instanceOrNull = Singleton<EarthDataManager>.GetInstanceOrNull();
        if (!Object.op_Inequality((Object) instanceOrNull, (Object) null))
          return false;
        foreach (string s in strArray)
        {
          int id = int.Parse(s);
          if (instanceOrNull.GetShopPurchaseCount(id) == 0)
            return false;
        }
        return true;
      }
    }

    public bool IsDisableUntilSoldOutArticleID
    {
      get
      {
        if (string.IsNullOrEmpty(this.disable_until_sold_out_article_id))
          return true;
        string[] strArray = this.disable_until_sold_out_article_id.Split(',');
        EarthDataManager instanceOrNull = Singleton<EarthDataManager>.GetInstanceOrNull();
        if (!Object.op_Inequality((Object) instanceOrNull, (Object) null))
          return false;
        foreach (string s in strArray)
        {
          int num = int.Parse(s);
          EarthShopArticle earthShopArticle = MasterData.EarthShopArticle[num];
          if (earthShopArticle.limit.HasValue && earthShopArticle.limit.Value - instanceOrNull.GetShopPurchaseCount(num) > 0)
            return false;
        }
        return true;
      }
    }
  }
}
