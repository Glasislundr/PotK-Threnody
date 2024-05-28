// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.PAS.DMMGamesStore.Fulfillment
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
  public class Fulfillment : Request<Fulfillment, Gsc.Purchase.API.Response.Fulfillment>
  {
    private const string ___path = "{0}/pas/dmmgamesstore/{1}/fulfill";

    public List<Fulfillment.PurchaseData_t> PurchaseDataList { get; set; }

    public string DeviceId { get; set; }

    public Fulfillment(string deviceId, List<Fulfillment.PurchaseData_t> purchaseDataList)
    {
      this.DeviceId = deviceId;
      this.PurchaseDataList = purchaseDataList;
    }

    public override string GetUrl()
    {
      return string.Format("{0}/pas/dmmgamesstore/{1}/fulfill", (object) SDK.Configuration.Env.NativeBaseUrl, (object) SDK.Configuration.AppName);
    }

    public override string GetPath() => "{0}/pas/dmmgamesstore/{1}/fulfill";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["receipts"] = Serializer.Instance.WithArray<Fulfillment.PurchaseData_t>().Add<Fulfillment.PurchaseData_t>(new Func<Fulfillment.PurchaseData_t, object>(Serializer.FromObject<Fulfillment.PurchaseData_t>)).Serialize<List<Fulfillment.PurchaseData_t>>(this.PurchaseDataList),
        ["platform"] = (object) "dmmgamesstore",
        ["version"] = (object) "v1",
        ["device_id"] = (object) this.DeviceId
      };
    }

    public class PurchaseData_t : IRequestObject, IObject
    {
      public int ViewerID { get; set; }

      public string OnetimeToken { get; set; }

      public string PaymentId { get; set; }

      public PurchaseData_t(int viewerId, string onetimeToken, string paymentId)
      {
        this.ViewerID = viewerId;
        this.OnetimeToken = onetimeToken;
        this.PaymentId = paymentId;
      }

      public Dictionary<string, object> GetPayload()
      {
        return new Dictionary<string, object>()
        {
          ["dmm_payment_id"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.PaymentId),
          ["dmm_viewer_id"] = Serializer.Instance.Add<int>(new Func<int, object>(Serializer.From<int>)).Serialize<int>(this.ViewerID),
          ["dmm_onetime_token"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.OnetimeToken)
        };
      }
    }
  }
}
