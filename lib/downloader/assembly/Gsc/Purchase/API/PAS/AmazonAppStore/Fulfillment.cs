// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.PAS.AmazonAppStore.Fulfillment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.PAS.AmazonAppStore
{
  public class Fulfillment : Request<Fulfillment, Gsc.Purchase.API.Response.Fulfillment>
  {
    private const string ___path = "{0}/pas/amazonappstore/{1}/fulfill";

    public string DeviceId { get; set; }

    public string UserId { get; set; }

    public List<Fulfillment.PurchaseData_t> PurchaseDataList { get; set; }

    public Fulfillment(
      string deviceId,
      string userId,
      List<Fulfillment.PurchaseData_t> purchaseDataList)
    {
      this.DeviceId = deviceId;
      this.UserId = userId;
      this.PurchaseDataList = purchaseDataList;
    }

    public override string GetUrl()
    {
      return string.Format("{0}/pas/amazonappstore/{1}/fulfill", (object) SDK.Configuration.Env.NativeBaseUrl, (object) SDK.Configuration.AppName);
    }

    public override string GetPath() => "{0}/pas/amazonappstore/{1}/fulfill";

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["receipt"] = (object) new Dictionary<string, object>()
        {
          ["user_id"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.UserId),
          ["receipts"] = Serializer.Instance.WithArray<Fulfillment.PurchaseData_t>().Add<Fulfillment.PurchaseData_t>(new Func<Fulfillment.PurchaseData_t, object>(Serializer.FromObject<Fulfillment.PurchaseData_t>)).Serialize<List<Fulfillment.PurchaseData_t>>(this.PurchaseDataList)
        },
        ["platform"] = (object) "amazonappstore",
        ["version"] = (object) "v1",
        ["device_id"] = (object) this.DeviceId
      };
    }

    public class PurchaseData_t : IRequestObject, IObject
    {
      public string Currency { get; set; }

      public float Price { get; set; }

      public string ReceiptId { get; set; }

      public PurchaseData_t(string currency, float price, string receiptId)
      {
        this.Currency = currency;
        this.Price = price;
        this.ReceiptId = receiptId;
      }

      public Dictionary<string, object> GetPayload()
      {
        return new Dictionary<string, object>()
        {
          ["currency"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Currency),
          ["price"] = Serializer.Instance.Add<float>(new Func<float, object>(Serializer.From<float>)).Serialize<float>(this.Price),
          ["receipt_id"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.ReceiptId)
        };
      }
    }
  }
}
