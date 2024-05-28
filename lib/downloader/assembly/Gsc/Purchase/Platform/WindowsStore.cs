// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.Platform.WindowsStore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Auth;
using Gsc.Network;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Gsc.Purchase.Platform
{
  public class WindowsStore : FlowWithPurchaseKit
  {
    public WindowsStore(PurchaseHandler handler)
      : base(handler)
    {
    }

    protected override IWebTask CreateFulfillmentTask(PurchaseKit.PurchaseResponse response)
    {
      List<Gsc.Purchase.API.PAS.WindowsStore.Fulfillment.PurchaseData_t> purchaseDataList = new List<Gsc.Purchase.API.PAS.WindowsStore.Fulfillment.PurchaseData_t>();
      foreach (PurchaseKit.PurchaseData purchaseData in ((PurchaseKit.AbstractResponse<PurchaseKit.MetaPurchase, PurchaseKit.PurchaseData>) response).Values)
      {
        PurchaseKit.PurchaseData purchase = purchaseData;
        ProductInfo productInfo = ((IEnumerable<ProductInfo>) PurchaseFlow.ProductList).Where<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == purchase.ProductId)).FirstOrDefault<ProductInfo>();
        purchaseDataList.Add(new Gsc.Purchase.API.PAS.WindowsStore.Fulfillment.PurchaseData_t(productInfo?.CurrencyCode, productInfo != null ? productInfo.Price : 0.0f, purchase.Data0, purchase.ID));
      }
      WebTask<Gsc.Purchase.API.PAS.WindowsStore.Fulfillment, Gsc.Purchase.API.Response.Fulfillment> task = new Gsc.Purchase.API.PAS.WindowsStore.Fulfillment(Session.DefaultSession.DeviceID, purchaseDataList).GetTask(WebTaskAttribute.Reliable | WebTaskAttribute.Silent | WebTaskAttribute.Parallel);
      task.OnResponse(new VoidCallbackWithError<Gsc.Purchase.API.Response.Fulfillment>(((FlowWithPurchaseKit) this).OnFulfillmentResponse));
      return (IWebTask) task;
    }
  }
}
