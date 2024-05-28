// Decompiled with JetBrains decompiler
// Type: SM.CoinBonusReward
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
  public class CoinBonusReward : KeyCompare
  {
    public int coin_bonus_id;
    public string client_coin_shop_title;
    public int id;
    public int _coin_product_id;

    public CoinProduct coin_product_id
    {
      get
      {
        if (MasterData.CoinProduct.ContainsKey(this._coin_product_id))
          return MasterData.CoinProduct[this._coin_product_id];
        Debug.LogError((object) ("Key not Found: MasterData.CoinProduct[" + (object) this._coin_product_id + "]"));
        return (CoinProduct) null;
      }
    }

    public CoinBonusReward()
    {
    }

    public CoinBonusReward(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.coin_bonus_id = (int) (long) json[nameof (coin_bonus_id)];
      this.client_coin_shop_title = (string) json[nameof (client_coin_shop_title)];
      this.id = (int) (long) json[nameof (id)];
      this._coin_product_id = (int) (long) json[nameof (coin_product_id)];
    }
  }
}
