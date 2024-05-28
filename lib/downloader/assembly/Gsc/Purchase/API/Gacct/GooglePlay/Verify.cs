// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Gacct.GooglePlay.Verify
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.Gacct.GooglePlay
{
  public class Verify : Request<Verify, Verify.Response>
  {
    private const string ___path = "/v2/android/verify";

    public string Currency { get; set; }

    public float Price { get; set; }

    public string Signature { get; set; }

    public string ReceiptData { get; set; }

    public Verify(string currency, float price, string signature, string receiptData)
    {
      this.Currency = currency;
      this.Price = price;
      this.Signature = signature;
      this.ReceiptData = receiptData;
    }

    public override string GetPath()
    {
      return SDK.Configuration.Env.PurchaseApiPrefix + "/v2/android/verify";
    }

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["currency"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Currency),
        ["price"] = Serializer.Instance.Add<float>(new Func<float, object>(Serializer.From<float>)).Serialize<float>(this.Price),
        ["signature"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Signature),
        ["signed_data"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.ReceiptData)
      };
    }

    public class Response : Gsc.Network.Response<Verify.Response>
    {
      public string SuccessTransactionId { get; private set; }

      public int CurrentPaidCoin { get; private set; }

      public int CurrentFreeCoin { get; private set; }

      public int AdditionalPaidCoin { get; private set; }

      public int AdditionalFreeCoin { get; private set; }

      public Response(byte[] payload)
      {
        Dictionary<string, object> result = Gsc.Network.Response<Verify.Response>.GetResult(payload);
        this.SuccessTransactionId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(result["success_transaction_id"]);
        this.CurrentPaidCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["current_paid_coin"]);
        this.CurrentFreeCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["current_free_coin"]);
        this.AdditionalPaidCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["additional_paid_coin"]);
        this.AdditionalFreeCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["additional_free_coin"]);
      }
    }
  }
}
