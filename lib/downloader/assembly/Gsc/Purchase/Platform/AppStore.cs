// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.Platform.AppStore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Purchase.API.Gacct.AppStore;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gsc.Purchase.Platform
{
  public class AppStore : FlowWithPurchaseKit
  {
    public AppStore(PurchaseHandler handler)
      : base(handler)
    {
    }

    protected override IWebTask CreateFulfillmentTask(PurchaseKit.PurchaseResponse response)
    {
      List<Verify.PurchaseData_t> receipts = new List<Verify.PurchaseData_t>();
      foreach (PurchaseKit.PurchaseData purchaseData in ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values)
      {
        PurchaseKit.PurchaseData purchase = purchaseData;
        ProductInfo productInfo = ((IEnumerable<ProductInfo>) PurchaseFlow.ProductList).Where<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == purchase.ProductId)).FirstOrDefault<ProductInfo>();
        receipts.Add(new Verify.PurchaseData_t(productInfo?.CurrencyCode, productInfo != null ? productInfo.Price : 0.0f, purchase.ID));
      }
      WebTask<Verify, Verify.Response> task = new Verify(receipts, ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Meta.Data0).GetTask(WebTaskAttribute.Reliable | WebTaskAttribute.Silent | WebTaskAttribute.Parallel);
      task.OnResponse((VoidCallbackWithError<Verify.Response>) ((r, e) =>
      {
        FulfillmentResult result = (FulfillmentResult) null;
        if (e == null)
        {
          FulfillmentResult.OrderInfo[] succeededTransactions = new FulfillmentResult.OrderInfo[r.SuccessTransactionIds.Length];
          FulfillmentResult.OrderInfo[] duplicatedTransactions = new FulfillmentResult.OrderInfo[r.DuplicatedTransactionIds.Length];
          for (int i = 0; i < r.SuccessTransactionIds.Length; ++i)
          {
            PurchaseKit.PurchaseData purchaseData = ((IEnumerable<PurchaseKit.PurchaseData>) ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values).Where<PurchaseKit.PurchaseData>((Func<PurchaseKit.PurchaseData, bool>) (x => x.ID == r.SuccessTransactionIds[i])).First<PurchaseKit.PurchaseData>();
            succeededTransactions[i] = new FulfillmentResult.OrderInfo(0, 0, purchaseData.ProductId, purchaseData.ID);
          }
          for (int i = 0; i < r.DuplicatedTransactionIds.Length; ++i)
          {
            PurchaseKit.PurchaseData purchaseData = ((IEnumerable<PurchaseKit.PurchaseData>) ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values).Where<PurchaseKit.PurchaseData>((Func<PurchaseKit.PurchaseData, bool>) (x => x.ID == r.DuplicatedTransactionIds[i])).First<PurchaseKit.PurchaseData>();
            duplicatedTransactions[i] = new FulfillmentResult.OrderInfo(0, 0, purchaseData.ProductId, purchaseData.ID);
          }
          result = new FulfillmentResult(r.CurrentFreeCoin, r.CurrentPaidCoin, succeededTransactions, duplicatedTransactions);
        }
        this.OnFulfillmentResult(result, e);
      }));
      return (IWebTask) task;
    }

    private void OnFulfillmentResult(FulfillmentResult result, IErrorResponse error)
    {
      if (error != null)
        this.handler.OnPurchaseResult(ResultCode.AlreadyOwned, (FulfillmentResult) null);
      else if ((result.SucceededTransactions == null || result.SucceededTransactions.Length == 0) && (result.DuplicatedTransactions == null || result.SucceededTransactions.Length == 0))
        this.Resume();
      else
        this.handler.OnPurchaseResult(ResultCode.Succeeded, result);
    }
  }
}
