// Decompiled with JetBrains decompiler
// Type: SM.HotDealInfo
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
  public class HotDealInfo : KeyCompare
  {
    public HotDealRewardSchema[] rewards;
    public int ios_coin_id;
    public string name;
    public int pack_id;
    public int priority;
    public int purchase_limit_sec;
    public bool is_modal;
    public int android_coin_id;
    public int dmmgamesstore_coin_id;
    public int aumarket_coin_id;
    public PackDescription[] descriptions;
    public DateTime EndDateTime;
    public bool IsPurchased;

    public HotDealInfo()
    {
    }

    public HotDealInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<HotDealRewardSchema> dealRewardSchemaList = new List<HotDealRewardSchema>();
      foreach (object json1 in (List<object>) json[nameof (rewards)])
        dealRewardSchemaList.Add(json1 == null ? (HotDealRewardSchema) null : new HotDealRewardSchema((Dictionary<string, object>) json1));
      this.rewards = dealRewardSchemaList.ToArray();
      this.ios_coin_id = (int) (long) json[nameof (ios_coin_id)];
      this.name = (string) json[nameof (name)];
      this.pack_id = (int) (long) json[nameof (pack_id)];
      this.priority = (int) (long) json[nameof (priority)];
      this.purchase_limit_sec = (int) (long) json[nameof (purchase_limit_sec)];
      this.is_modal = (bool) json[nameof (is_modal)];
      this.android_coin_id = (int) (long) json[nameof (android_coin_id)];
      this.dmmgamesstore_coin_id = (int) (long) json[nameof (dmmgamesstore_coin_id)];
      this.aumarket_coin_id = (int) (long) json[nameof (aumarket_coin_id)];
      List<PackDescription> packDescriptionList = new List<PackDescription>();
      foreach (object json2 in (List<object>) json[nameof (descriptions)])
        packDescriptionList.Add(json2 == null ? (PackDescription) null : new PackDescription((Dictionary<string, object>) json2));
      this.descriptions = packDescriptionList.ToArray();
    }

    public int GetCoinId()
    {
      switch (Device.Platform)
      {
        case "android":
        case "googleplay":
          return this.android_coin_id;
        case "appstore":
        case "ios":
          return this.ios_coin_id;
        case "aumarket":
          return this.aumarket_coin_id;
        case "dmmgamesstore":
        case "windows":
        case "windowsstore":
          return this.dmmgamesstore_coin_id;
        default:
          return 0;
      }
    }

    public string GetProductId()
    {
      int coinId = this.GetCoinId();
      return ((IEnumerable<CoinProduct>) MasterData.CoinProductList).FirstOrDefault<CoinProduct>((Func<CoinProduct, bool>) (x => x.ID == coinId))?.product_id;
    }

    public int GetAdditionalPaidCoin()
    {
      int coinId = this.GetCoinId();
      CoinProduct coinProduct = ((IEnumerable<CoinProduct>) MasterData.CoinProductList).FirstOrDefault<CoinProduct>((Func<CoinProduct, bool>) (x => x.ID == coinId));
      return coinProduct == null ? 0 : coinProduct.additional_paid_coin;
    }

    public int? GetProductDetailGroupNo()
    {
      int coinId = this.GetCoinId();
      return ((IEnumerable<CoinProduct>) MasterData.CoinProductList).FirstOrDefault<CoinProduct>((Func<CoinProduct, bool>) (x => x.ID == coinId))?.product_detail_group_no;
    }

    public string GetProductName()
    {
      int coinId = this.GetCoinId();
      return ((IEnumerable<CoinProduct>) MasterData.CoinProductList).FirstOrDefault<CoinProduct>((Func<CoinProduct, bool>) (x => x.ID == coinId))?.name;
    }

    public HotdealPack GetHotdealPack()
    {
      return !this.IsActive ? (HotdealPack) null : MasterData.HotdealPack[this.pack_id];
    }

    public string ModalResourceName
    {
      get
      {
        HotdealPack hotdealPack = this.GetHotdealPack();
        return hotdealPack == null ? string.Empty : hotdealPack.modal_resource_name;
      }
    }

    public bool IsActive => !this.IsPurchased && this.EndDateTime >= DateTime.Now;
  }
}
