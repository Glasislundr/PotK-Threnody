// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Request.ChargeV2Limit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gsc.Purchase.API.Request
{
  public class ChargeV2Limit : Gsc.Network.Request<ChargeV2Limit, Gsc.Purchase.API.Response.ChargeV2Limit>
  {
    private const string ___path = "/v2/limit";

    public List<float> Prices { get; set; }

    public string Currency { get; set; }

    public ChargeV2Limit(List<float> prices, string currency)
    {
      this.Prices = prices;
      this.Currency = currency;
    }

    public override string GetPath()
    {
      return SDK.Configuration.Env.PurchaseApiPrefix + "/v2/limit?currency=" + this.Currency + string.Join("", this.Prices.Select<float, string>((Func<float, string>) (x => "&price=" + (object) x)).ToArray<string>());
    }

    public override string GetMethod() => "GET";

    protected override Dictionary<string, object> GetParameters()
    {
      return (Dictionary<string, object>) null;
    }
  }
}
