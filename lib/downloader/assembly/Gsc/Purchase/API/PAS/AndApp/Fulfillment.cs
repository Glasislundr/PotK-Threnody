// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.PAS.AndApp.Fulfillment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.PAS.AndApp
{
  public class Fulfillment : Request<Fulfillment, Gsc.Purchase.API.Response.Fulfillment>
  {
    private const string ___path = "{0}/pas/andapp/{1}/fulfill";

    public string DeviceId { get; set; }

    public List<Fulfillment.PurchaseData_t> PurchaseDataList { get; set; }

    public Fulfillment(string deviceId, List<Fulfillment.PurchaseData_t> purchaseDataList)
    {
      this.DeviceId = deviceId;
      this.PurchaseDataList = purchaseDataList;
    }

    public override string GetUrl()
    {
      return string.Format("{0}/pas/andapp/{1}/fulfill", (object) SDK.Configuration.Env.NativeBaseUrl, (object) SDK.Configuration.AppName);
    }

    public override string GetPath() => (string) null;

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["device_id"] = (object) this.DeviceId,
        ["platform"] = (object) "andapp",
        ["version"] = (object) "v1",
        ["receipts"] = Serializer.Instance.WithArray<Fulfillment.PurchaseData_t>().Add<Fulfillment.PurchaseData_t>(new Func<Fulfillment.PurchaseData_t, object>(Serializer.FromObject<Fulfillment.PurchaseData_t>)).Serialize<List<Fulfillment.PurchaseData_t>>(this.PurchaseDataList)
      };
    }

    public class PurchaseData_t : IRequestObject, IObject
    {
      public string ReceiptData { get; set; }

      public PurchaseData_t(string receiptData) => this.ReceiptData = receiptData;

      public Dictionary<string, object> GetPayload()
      {
        return new Dictionary<string, object>()
        {
          ["transaction_token"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.ReceiptData)
        };
      }
    }
  }
}
