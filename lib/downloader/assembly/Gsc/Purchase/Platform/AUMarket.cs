// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.Platform.AUMarket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Network;
using Gsc.Purchase.API.Gacct.AUMarket;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gsc.Purchase.Platform
{
  public class AUMarket : FlowWithPurchaseKit
  {
    public const int RESULT_AUMARKET_OVER_CREDIT_LIMIT = 49;
    private string[] productIds;

    public AUMarket(PurchaseHandler handler)
      : base(handler)
    {
    }

    public override void Init(string[] productIds) => this.UpdateProducts(productIds);

    public override void UpdateProducts(string[] productIds)
    {
      this.productIds = productIds;
      new Gsc.Purchase.API.Request.ProductList().GetTask(WebTaskAttribute.Reliable | WebTaskAttribute.Silent | WebTaskAttribute.Parallel).OnResponse((VoidCallbackWithError<Gsc.Purchase.API.Response.ProductList>) ((r, e) =>
      {
        if (e != null)
        {
          if (!this.handler.initialized)
            this.handler.OnInitResult(ResultCode.Failed);
          else
            this.handler.OnProductResult(ResultCode.Failed, (ProductInfo[]) null);
        }
        else
        {
          Gsc.Purchase.API.Response.ProductList.ProductData_t[] array = ((IEnumerable<Gsc.Purchase.API.Response.ProductList.ProductData_t>) r.Products).Where<Gsc.Purchase.API.Response.ProductList.ProductData_t>((Func<Gsc.Purchase.API.Response.ProductList.ProductData_t, bool>) (x => x.Currency == "JPY" && Array.IndexOf<string>(productIds, x.ProductId) >= 0)).ToArray<Gsc.Purchase.API.Response.ProductList.ProductData_t>();
          ProductInfo[] productInfos = new ProductInfo[array.Length];
          for (int index = 0; index < array.Length; ++index)
          {
            Gsc.Purchase.API.Response.ProductList.ProductData_t productDataT = array[index];
            productInfos[index] = new ProductInfo(productDataT.ProductId, productDataT.Name, productDataT.Description, productDataT.LocalizedPrice, productDataT.Currency, productDataT.Price);
          }
          this.handler.OnProductResult(ResultCode.Succeeded, productInfos);
          if (this.handler.initialized)
            return;
          base.Init((string[]) null);
        }
      }));
    }

    public override void OnInitResult(int resultCode) => base.OnInitResult(resultCode);

    public override void Resume()
    {
    }

    protected override IWebTask CreateFulfillmentTask(PurchaseKit.PurchaseResponse response)
    {
      List<Verify.PurchaseData_t> purchaseDataList = new List<Verify.PurchaseData_t>(((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values.Length);
      foreach (PurchaseKit.PurchaseData purchaseData in ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values)
      {
        PurchaseKit.PurchaseData purchase = purchaseData;
        ProductInfo productInfo = ((IEnumerable<ProductInfo>) PurchaseFlow.ProductList).Where<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == purchase.ProductId)).FirstOrDefault<ProductInfo>();
        purchaseDataList.Add(new Verify.PurchaseData_t(productInfo.CurrencyCode, productInfo.Price, purchase.ID));
      }
      WebTask<Verify, Verify.Response> task = new Verify(((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Meta.Data1, ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Meta.Data0, purchaseDataList).GetTask(WebTaskAttribute.Reliable | WebTaskAttribute.Silent | WebTaskAttribute.Parallel);
      task.OnResponse((VoidCallbackWithError<Verify.Response>) ((r, e) =>
      {
        if (e != null)
        {
          this.handler.OnPurchaseResult(ResultCode.AlreadyOwned, (FulfillmentResult) null);
        }
        else
        {
          FulfillmentResult.OrderInfo[] succeededTransactions = new FulfillmentResult.OrderInfo[r.SuccessTransactionIds.Length];
          FulfillmentResult.OrderInfo[] duplicatedTransactions = new FulfillmentResult.OrderInfo[r.DuplicatedTransactionIds.Length];
          for (int index = 0; index < r.SuccessTransactionIds.Length; ++index)
          {
            string tx = r.SuccessTransactionIds[index];
            PurchaseKit.PurchaseData purchaseData = ((IEnumerable<PurchaseKit.PurchaseData>) ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values).Where<PurchaseKit.PurchaseData>((Func<PurchaseKit.PurchaseData, bool>) (x => x.ID == tx)).FirstOrDefault<PurchaseKit.PurchaseData>();
            succeededTransactions[index] = new FulfillmentResult.OrderInfo(0, 0, purchaseData?.ProductId, tx);
          }
          for (int index = 0; index < r.DuplicatedTransactionIds.Length; ++index)
          {
            string tx = r.DuplicatedTransactionIds[index];
            PurchaseKit.PurchaseData purchaseData = ((IEnumerable<PurchaseKit.PurchaseData>) ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values).Where<PurchaseKit.PurchaseData>((Func<PurchaseKit.PurchaseData, bool>) (x => x.ID == tx)).FirstOrDefault<PurchaseKit.PurchaseData>();
            duplicatedTransactions[index] = new FulfillmentResult.OrderInfo(0, 0, purchaseData?.ProductId, tx);
          }
          this.handler.OnPurchaseResult(ResultCode.Succeeded, new FulfillmentResult(r.CurrentFreeCoin, r.CurrentPaidCoin, succeededTransactions, duplicatedTransactions));
        }
      }));
      return (IWebTask) task;
    }
  }
}
