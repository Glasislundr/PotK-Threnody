// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.PAS.DMMGamesStore.PurchaseList
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
  public class PurchaseList : Request<PurchaseList, PurchaseList.Response>
  {
    private const string ___path = "{0}/pas/dmmgamesstore/{1}/purchase/list";

    public int ViewerID { get; set; }

    public string OnetimeToken { get; set; }

    public PurchaseList(int viewerId, string onetimeToken)
    {
      this.ViewerID = viewerId;
      this.OnetimeToken = onetimeToken;
    }

    public override string GetUrl()
    {
      return string.Format("{0}/pas/dmmgamesstore/{1}/purchase/list", (object) SDK.Configuration.Env.NativeBaseUrl, (object) SDK.Configuration.AppName);
    }

    public override string GetPath() => "{0}/pas/dmmgamesstore/{1}/purchase/list";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["dmm_viewer_id"] = Serializer.Instance.Add<int>(new Func<int, object>(Serializer.From<int>)).Serialize<int>(this.ViewerID),
        ["dmm_onetime_token"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.OnetimeToken)
      };
    }

    public class Response : Gsc.Network.Response<PurchaseList.Response>
    {
      public PurchaseList.Response.PurchaseInfo_t[] Infos { get; private set; }

      public Response(byte[] payload)
      {
        Dictionary<string, object> result = Gsc.Network.Response<PurchaseList.Response>.GetResult(payload);
        this.Infos = Deserializer.Instance.WithArray<PurchaseList.Response.PurchaseInfo_t>().Add<PurchaseList.Response.PurchaseInfo_t>(new Func<object, PurchaseList.Response.PurchaseInfo_t>(Deserializer.ToObject<PurchaseList.Response.PurchaseInfo_t>)).Deserialize<PurchaseList.Response.PurchaseInfo_t[]>(result["purchase_infos"]);
      }

      public class PurchaseInfo_t : IResponseObject, IObject
      {
        public string PaymentId { get; private set; }

        public string ProductId { get; private set; }

        public PurchaseInfo_t(Dictionary<string, object> result)
        {
          this.PaymentId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["dmm_payment_id"]);
          this.ProductId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["product_id"]);
        }
      }
    }
  }
}
