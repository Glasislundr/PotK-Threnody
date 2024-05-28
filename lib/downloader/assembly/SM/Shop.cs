// Decompiled with JetBrains decompiler
// Type: SM.Shop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class Shop : KeyCompare
  {
    public PlayerShopArticle[] articles;
    public int id;

    public Shop()
    {
    }

    public Shop(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<PlayerShopArticle> playerShopArticleList = new List<PlayerShopArticle>();
      foreach (object json1 in (List<object>) json[nameof (articles)])
        playerShopArticleList.Add(json1 == null ? (PlayerShopArticle) null : new PlayerShopArticle((Dictionary<string, object>) json1));
      this.articles = playerShopArticleList.ToArray();
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
