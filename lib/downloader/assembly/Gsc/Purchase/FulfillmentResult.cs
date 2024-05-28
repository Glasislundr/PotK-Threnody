// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.FulfillmentResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Network.Support.MiniJsonHelper;
using System;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Purchase
{
  public class FulfillmentResult : IResponseObject, IObject
  {
    public readonly int CurrentFreeCoin;
    public readonly int CurrentPaidCoin;
    public readonly FulfillmentResult.OrderInfo[] SucceededTransactions;
    public readonly FulfillmentResult.OrderInfo[] DuplicatedTransactions;

    public FulfillmentResult(
      int currentFreeCoin,
      int currentPaidCoin,
      FulfillmentResult.OrderInfo[] succeededTransactions,
      FulfillmentResult.OrderInfo[] duplicatedTransactions)
    {
      this.CurrentFreeCoin = currentFreeCoin;
      this.CurrentPaidCoin = currentPaidCoin;
      this.SucceededTransactions = succeededTransactions;
      this.DuplicatedTransactions = duplicatedTransactions;
    }

    public FulfillmentResult(object payload)
      : this((Dictionary<string, object>) payload)
    {
    }

    public FulfillmentResult(Dictionary<string, object> payload)
    {
      this.CurrentFreeCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(payload["current_free_coin"]);
      this.CurrentPaidCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(payload["current_paid_coin"]);
      this.SucceededTransactions = Deserializer.Instance.WithArray<FulfillmentResult.OrderInfo>().Add<FulfillmentResult.OrderInfo>(new Func<object, FulfillmentResult.OrderInfo>(Deserializer.ToObject<FulfillmentResult.OrderInfo>)).Deserialize<FulfillmentResult.OrderInfo[]>(payload["succeeded_orders"]);
      this.DuplicatedTransactions = Deserializer.Instance.WithArray<FulfillmentResult.OrderInfo>().Add<FulfillmentResult.OrderInfo>(new Func<object, FulfillmentResult.OrderInfo>(Deserializer.ToObject<FulfillmentResult.OrderInfo>)).Deserialize<FulfillmentResult.OrderInfo[]>(payload["duplicated_orders"]);
    }

    public class OrderInfo : IResponseObject, IObject
    {
      public readonly int FreeCoin;
      public readonly int PaidCoin;
      public readonly string ProductId;
      public readonly string TransactionId;

      public OrderInfo(int freeCoin, int paidCoin, string productId, string transactionId)
      {
        this.FreeCoin = freeCoin;
        this.PaidCoin = paidCoin;
        this.ProductId = productId;
        this.TransactionId = transactionId;
      }

      public OrderInfo(object payload)
        : this((Dictionary<string, object>) payload)
      {
      }

      public OrderInfo(Dictionary<string, object> payload)
      {
        this.FreeCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(payload["free_coin"]);
        this.PaidCoin = Deserializer.Instance.Add<int>(new Func<object, int>(Deserializer.ToIntegerType.int32)).Deserialize<int>(payload["paid_coin"]);
        this.ProductId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(payload["product_id"]);
        this.TransactionId = Deserializer.Instance.Add<string>(new Func<object, string>(Deserializer.To<string>)).Deserialize<string>(payload["order_id"]);
      }
    }
  }
}
