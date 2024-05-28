// Decompiled with JetBrains decompiler
// Type: SM.PlayerShopArticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerShopArticle : KeyCompare
  {
    public string article_description;
    public int? banner_id;
    public int payment_type_id;
    public string article_name;
    public string icon_resource;
    public int? limit_type;
    public DateTime? end_at;
    public int payment_amount;
    public int? limit;
    public string decoration_resource;
    public int _article;
    public PlayerShopArticleContents[] contents;

    public ShopArticle article
    {
      get
      {
        if (MasterData.ShopArticle.ContainsKey(this._article))
          return MasterData.ShopArticle[this._article];
        Debug.LogError((object) ("Key not Found: MasterData.ShopArticle[" + (object) this._article + "]"));
        return (ShopArticle) null;
      }
    }

    public PlayerShopArticle()
    {
    }

    public PlayerShopArticle(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.article_description = json[nameof (article_description)] == null ? (string) null : (string) json[nameof (article_description)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (banner_id)] != null)
      {
        nullable1 = (long?) json[nameof (banner_id)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.banner_id = nullable2;
      this.payment_type_id = (int) (long) json[nameof (payment_type_id)];
      this.article_name = json[nameof (article_name)] == null ? (string) null : (string) json[nameof (article_name)];
      this.icon_resource = (string) json[nameof (icon_resource)];
      int? nullable3;
      if (json[nameof (limit_type)] != null)
      {
        nullable1 = (long?) json[nameof (limit_type)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.limit_type = nullable3;
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.payment_amount = (int) (long) json[nameof (payment_amount)];
      int? nullable4;
      if (json[nameof (limit)] != null)
      {
        nullable1 = (long?) json[nameof (limit)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.limit = nullable4;
      this.decoration_resource = (string) json[nameof (decoration_resource)];
      this._article = (int) (long) json[nameof (article)];
      List<PlayerShopArticleContents> shopArticleContentsList = new List<PlayerShopArticleContents>();
      foreach (object json1 in (List<object>) json[nameof (contents)])
        shopArticleContentsList.Add(json1 == null ? (PlayerShopArticleContents) null : new PlayerShopArticleContents((Dictionary<string, object>) json1));
      this.contents = shopArticleContentsList.ToArray();
    }
  }
}
