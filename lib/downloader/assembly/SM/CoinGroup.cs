// Decompiled with JetBrains decompiler
// Type: SM.CoinGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Auth;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class CoinGroup : KeyCompare
  {
    public int ios_coin_id;
    public int aumarket_coin_id;
    public int android_coin_id;
    public int dmmgamesstore_coin_id;
    public int id;
    public int logic_type_id;

    public string GetProductId()
    {
      switch (Device.Platform)
      {
        case "appstore":
          return ((IEnumerable<CoinProduct>) MasterData.CoinProductList).First<CoinProduct>((Func<CoinProduct, bool>) (x => x.ID == this.ios_coin_id)).product_id;
        case "googleplay":
          return ((IEnumerable<CoinProduct>) MasterData.CoinProductList).First<CoinProduct>((Func<CoinProduct, bool>) (x => x.ID == this.android_coin_id)).product_id;
        case "dmmgamesstore":
          return ((IEnumerable<CoinProduct>) MasterData.CoinProductList).First<CoinProduct>((Func<CoinProduct, bool>) (x => x.ID == this.dmmgamesstore_coin_id)).product_id;
        case "aumarket":
          return ((IEnumerable<CoinProduct>) MasterData.CoinProductList).First<CoinProduct>((Func<CoinProduct, bool>) (x => x.ID == this.aumarket_coin_id)).product_id;
        default:
          Debug.LogError((object) ("GetProductId Error: 想定しないていないPlatformです: " + Device.Platform));
          return "";
      }
    }

    public int GetCoinId()
    {
      switch (Device.Platform)
      {
        case "appstore":
          return this.ios_coin_id;
        case "googleplay":
          return this.android_coin_id;
        case "dmmgamesstore":
          return this.dmmgamesstore_coin_id;
        case "aumarket":
          return this.aumarket_coin_id;
        default:
          Debug.LogError((object) ("GetPlatformId Error: 想定しないていないPlatformです: " + Device.Platform));
          return 0;
      }
    }

    public CoinGroup()
    {
    }

    public CoinGroup(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.ios_coin_id = (int) (long) json[nameof (ios_coin_id)];
      this.aumarket_coin_id = (int) (long) json[nameof (aumarket_coin_id)];
      this.android_coin_id = (int) (long) json[nameof (android_coin_id)];
      this.dmmgamesstore_coin_id = (int) (long) json[nameof (dmmgamesstore_coin_id)];
      this.id = (int) (long) json[nameof (id)];
      this.logic_type_id = (int) (long) json[nameof (logic_type_id)];
    }
  }
}
