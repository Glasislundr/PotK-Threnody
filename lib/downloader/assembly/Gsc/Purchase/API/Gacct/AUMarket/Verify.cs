﻿// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.API.Gacct.AUMarket.Verify
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase.API.Gacct.AUMarket
{
  public class Verify : Request<Verify, Verify.Response>
  {
    private const string ___path = "/aumarket/verify";

    public List<Verify.PurchaseData_t> PurchaseDataList { get; set; }

    public string Signature { get; set; }

    public string ReceiptData { get; set; }

    public Verify(
      string signature,
      string receiptData,
      List<Verify.PurchaseData_t> purchaseDataList)
    {
      this.Signature = signature;
      this.ReceiptData = receiptData;
      this.PurchaseDataList = purchaseDataList;
    }

    public override string GetPath()
    {
      return SDK.Configuration.Env.PurchaseApiPrefix + "/aumarket/verify";
    }

    public override string GetMethod() => "POST";

    protected override Dictionary<string, object> GetParameters()
    {
      return new Dictionary<string, object>()
      {
        ["receipts"] = Serializer.Instance.WithArray<Verify.PurchaseData_t>().Add<Verify.PurchaseData_t>(new Func<Verify.PurchaseData_t, object>(Serializer.FromObject<Verify.PurchaseData_t>)).Serialize<List<Verify.PurchaseData_t>>(this.PurchaseDataList),
        ["signature"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Signature),
        ["receipt"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.ReceiptData)
      };
    }

    public class PurchaseData_t : IRequestObject, IObject
    {
      public string Currency { get; set; }

      public float Price { get; set; }

      public string PayInfoNo { get; set; }

      public PurchaseData_t(string currency, float price, string payInfoNo)
      {
        this.Currency = currency;
        this.Price = price;
        this.PayInfoNo = payInfoNo;
      }

      public Dictionary<string, object> GetPayload()
      {
        return new Dictionary<string, object>()
        {
          ["currency"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.Currency),
          ["price"] = Serializer.Instance.Add<float>(new Func<float, object>(Serializer.From<float>)).Serialize<float>(this.Price),
          ["pay_info_no"] = Serializer.Instance.Add<string>(new Func<string, object>(Serializer.From<string>)).Serialize<string>(this.PayInfoNo)
        };
      }
    }

    public class Response : Gsc.Network.Response<Verify.Response>
    {
      public string[] SuccessTransactionIds { get; private set; }

      public string[] DuplicatedTransactionIds { get; private set; }

      public int CurrentPaidCoin { get; private set; }

      public int CurrentFreeCoin { get; private set; }

      public int AdditionalPaidCoin { get; private set; }

      public int AdditionalFreeCoin { get; private set; }

      public Response(byte[] payload)
      {
        Dictionary<string, object> result = Gsc.Network.Response<Verify.Response>.GetResult(payload);
        this.SuccessTransactionIds = Deserializer.Instance.WithArray<string>().Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string[]>(result["success_pay_info_no"]);
        this.DuplicatedTransactionIds = Deserializer.Instance.WithArray<string>().Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string[]>(result["duplicate_pay_info_nos"]);
        this.CurrentPaidCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["current_paid_coin"]);
        this.CurrentFreeCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["current_free_coin"]);
        this.AdditionalPaidCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["additional_paid_coin"]);
        this.AdditionalFreeCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(result["additional_free_coin"]);
      }
    }
  }
}
