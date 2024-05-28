// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.PAS.DMMGamesStore.Purchase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.PAS.DMMGamesStore
{
  public class Purchase : Request<Gsc.Purchase.API.PAS.DMMGamesStore.Purchase, Gsc.Purchase.API.PAS.DMMGamesStore.Purchase.Response>
  {
    private const string ___path = "{0}/pas/dmmgamesstore/{1}/purchase";

    public int ViewerID { get; set; }

    public string OnetimeToken { get; set; }

    public string ProductID { get; set; }

    public Purchase(int viewerId, string onetimeToken, string productId)
    {
      this.ViewerID = viewerId;
      this.OnetimeToken = onetimeToken;
      this.ProductID = productId;
    }

    public override string GetUrl()
    {
      return string.Format("{0}/pas/dmmgamesstore/{1}/purchase", (object) SDK.Configuration.Env.NativeBaseUrl, (object) SDK.Configuration.AppName);
    }

    public override string GetPath() => "{0}/pas/dmmgamesstore/{1}/purchase";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["dmm_viewer_id"] = Serializer.Instance.Add<int>(new Func<int, object>(Serializer.From<int>)).Serialize<int>(this.ViewerID),
        ["dmm_onetime_token"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.OnetimeToken),
        ["product_id"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.ProductID)
      };
    }

    public class Response : Gsc.Network.Response<Gsc.Purchase.API.PAS.DMMGamesStore.Purchase.Response>
    {
      public string PaymentId { get; private set; }

      public Response(byte[] payload)
      {
        Dictionary<string, object> result = Gsc.Network.Response<Gsc.Purchase.API.PAS.DMMGamesStore.Purchase.Response>.GetResult(payload);
        this.PaymentId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["dmm_payment_id"]);
      }
    }
  }
}
