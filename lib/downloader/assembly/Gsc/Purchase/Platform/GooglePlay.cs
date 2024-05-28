// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.Platform.GooglePlay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Purchase.API.Gacct.GooglePlay;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gsc.Purchase.Platform
{
  public class GooglePlay : FlowWithPurchaseKit
  {
    public GooglePlay(PurchaseHandler handler)
      : base(handler)
    {
    }

    protected override IWebTask CreateFulfillmentTask(PurchaseKit.PurchaseResponse response)
    {
      int count = 0;
      bool hasError = false;
      IWebTask fulfillmentTask = (IWebTask) null;
      FulfillmentResult.OrderInfo[] succeededTransactions = new FulfillmentResult.OrderInfo[((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values.Length];
      for (int index = 0; index < succeededTransactions.Length; ++index)
      {
        PurchaseKit.PurchaseData purchaseData = ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values[index];
        succeededTransactions[index] = new FulfillmentResult.OrderInfo(0, 0, purchaseData.ProductId, purchaseData.ID);
      }
      foreach (PurchaseKit.PurchaseData purchaseData in ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values)
      {
        PurchaseKit.PurchaseData purchase = purchaseData;
        ProductInfo productInfo = ((IEnumerable<ProductInfo>) PurchaseFlow.ProductList).Where<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == purchase.ProductId)).FirstOrDefault<ProductInfo>();
        string currencyCode = productInfo?.CurrencyCode;
        double price = productInfo != null ? (double) productInfo.Price : 0.0;
        string data1 = purchase.Data1;
        string data0 = purchase.Data0;
        ((WebTask<Verify, Verify.Response>) (fulfillmentTask = (IWebTask) new Verify(currencyCode, (float) price, data1, data0).GetTask(WebTaskAttribute.Reliable | WebTaskAttribute.Silent | WebTaskAttribute.Parallel))).OnResponse((VoidCallbackWithError<Verify.Response>) ((r, e) =>
        {
          if (e != null)
            hasError = true;
          if (++count < ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values.Length)
            return;
          if (hasError)
            this.handler.OnPurchaseResult(ResultCode.AlreadyOwned, (FulfillmentResult) null);
          else
            this.handler.OnPurchaseResult(ResultCode.Succeeded, new FulfillmentResult(r.CurrentFreeCoin, r.CurrentPaidCoin, ((IEnumerable<FulfillmentResult.OrderInfo>) succeededTransactions).ToArray<FulfillmentResult.OrderInfo>(), new FulfillmentResult.OrderInfo[0]));
        }));
      }
      return fulfillmentTask;
    }
  }
}
