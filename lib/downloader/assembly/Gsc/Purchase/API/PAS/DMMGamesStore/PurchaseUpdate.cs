// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.PAS.DMMGamesStore.PurchaseUpdate
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
  public class PurchaseUpdate : Request<PurchaseUpdate, PurchaseUpdate.Response>
  {
    private const string ___path = "{0}/pas/dmmgamesstore/{1}/purchase/update";

    public int ViewerID { get; set; }

    public string OnetimeToken { get; set; }

    public List<string> PaymentIds { get; set; }

    public PurchaseUpdate(int viewerId, string onetimeToken, List<string> paymentIds)
    {
      this.ViewerID = viewerId;
      this.OnetimeToken = onetimeToken;
      this.PaymentIds = paymentIds;
    }

    public override string GetUrl()
    {
      return string.Format("{0}/pas/dmmgamesstore/{1}/purchase/update", (object) SDK.Configuration.Env.NativeBaseUrl, (object) SDK.Configuration.AppName);
    }

    public override string GetPath() => "{0}/pas/dmmgamesstore/{1}/purchase/update";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["dmm_viewer_id"] = Serializer.Instance.Add<int>(new Func<int, object>(Serializer.From<int>)).Serialize<int>(this.ViewerID),
        ["dmm_onetime_token"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.OnetimeToken),
        ["dmm_payment_ids"] = Serializer.Instance.WithArray<string>().Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<List<string>>(this.PaymentIds)
      };
    }

    public class Response : Gsc.Network.Response<PurchaseUpdate.Response>
    {
      public PurchaseUpdate.Response.Status_t[] Results { get; private set; }

      public Response(byte[] payload)
      {
        Dictionary<string, object> result = Gsc.Network.Response<PurchaseUpdate.Response>.GetResult(payload);
        this.Results = Deserializer.Instance.WithArray<PurchaseUpdate.Response.Status_t>().Add<PurchaseUpdate.Response.Status_t>(new Func<object, PurchaseUpdate.Response.Status_t>(Deserializer.ToObject<PurchaseUpdate.Response.Status_t>)).Deserialize<PurchaseUpdate.Response.Status_t[]>(result["statuses"]);
      }

      public class Status_t : IResponseObject, IObject
      {
        public string PaymentId { get; private set; }

        public string Status { get; private set; }

        public Status_t(Dictionary<string, object> result)
        {
          this.PaymentId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["dmm_payment_id"]);
          this.Status = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["status"]);
        }
      }
    }
  }
}
