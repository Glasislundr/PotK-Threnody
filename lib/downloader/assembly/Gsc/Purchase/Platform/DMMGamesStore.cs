// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.Platform.DMMGamesStore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Core;
using Gsc.Network;
using Gsc.Purchase.API.PAS.DMMGamesStore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace Gsc.Purchase.Platform
{
  public class DMMGamesStore : IPurchaseFlowImpl
  {
    private readonly PurchaseHandler handler;
    private ProductInfo processProduct;

    private static int ViewerID => Gsc.Auth.GAuth.DMMGamesStore.Device.Instance.ViewerId;

    private static string OnetimeToken => Gsc.Auth.GAuth.DMMGamesStore.Device.Instance.OnetimeToken;

    public DMMGamesStore(PurchaseHandler handler) => this.handler = handler;

    public void Init(string[] productIds) => this.UpdateProducts(productIds);

    public void Resume() => Gsc.Purchase.Platform.DMMGamesStore.InnerFlow.Resume(this.handler);

    public bool Purchase(ProductInfo product, string accountId)
    {
      this.processProduct = product;
      this.handler.Confirm(product);
      return true;
    }

    public bool Confirmed()
    {
      if (this.processProduct == null)
        return false;
      PurchaseHandler.Log(0, "dmmgamesstore.Purchase", "product_id: " + this.processProduct.ProductId);
      Gsc.Purchase.Platform.DMMGamesStore.InnerFlow.Purchase(this.handler, this.processProduct.ProductId);
      this.processProduct = (ProductInfo) null;
      return true;
    }

    public void UpdateProducts(string[] productIds)
    {
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
          this.handler.OnInitResult(ResultCode.Succeeded);
          this.Resume();
        }
      }));
    }

    public void Consume(string productId, string receiptId)
    {
      PurchaseHandler.Log(0, "dmmgamesstore.Consume", "transaction_id: " + receiptId);
      new Gsc.Purchase.API.PAS.DMMGamesStore.Consume(Gsc.Purchase.Platform.DMMGamesStore.ViewerID, Gsc.Purchase.Platform.DMMGamesStore.OnetimeToken, receiptId).Cast();
    }

    private class InnerFlow
    {
      private const float TIMEOUT_SECONDS = 30f;
      private readonly PurchaseHandler handler;
      private string waitingPaymentId;
      private string paymentStatus;

      private InnerFlow(PurchaseHandler handler) => this.handler = handler;

      public static void Purchase(PurchaseHandler handler, string productId)
      {
        ImmortalObject.Instance.StartCoroutine(new Gsc.Purchase.Platform.DMMGamesStore.InnerFlow(handler)._Purchase(productId));
      }

      public static void Resume(PurchaseHandler handler)
      {
        ImmortalObject.Instance.StartCoroutine(Gsc.Purchase.Platform.DMMGamesStore.InnerFlow._Resume(handler));
      }

      private IEnumerator _Purchase(string productId)
      {
        WebInternalTask<Gsc.Purchase.API.PAS.DMMGamesStore.Purchase, Gsc.Purchase.API.PAS.DMMGamesStore.Purchase.Response> task = WebInternalTask.Create<Gsc.Purchase.API.PAS.DMMGamesStore.Purchase, Gsc.Purchase.API.PAS.DMMGamesStore.Purchase.Response>((Gsc.Network.Request<Gsc.Purchase.API.PAS.DMMGamesStore.Purchase, Gsc.Purchase.API.PAS.DMMGamesStore.Purchase.Response>) new Gsc.Purchase.API.PAS.DMMGamesStore.Purchase(Gsc.Purchase.Platform.DMMGamesStore.ViewerID, Gsc.Purchase.Platform.DMMGamesStore.OnetimeToken, productId));
        task.OnStart();
        yield return (object) task;
        task.OnFinish();
        if (task.Result == WebTaskResult.MustErrorHandle)
        {
          switch (task.error.ErrorCode)
          {
            case "insufficient_balances":
              this.handler.OnPurchaseResult(ResultCode.InsufficientBalances, (FulfillmentResult) null);
              break;
            case "already_owned":
              this.handler.OnPurchaseResult(ResultCode.AlreadyOwned, (FulfillmentResult) null);
              break;
          }
        }
        else
        {
          PurchaseHandler.Log(0, "dmmgamesstore.api.Purchase", "product_id: " + productId + ", result: " + (object) task.Result);
          if (task.Result == WebTaskResult.ServerError)
            yield return (object) this._GetPaymentId(productId);
          if (task.Result == WebTaskResult.Success)
            this.waitingPaymentId = task.Response.PaymentId;
          if (this.waitingPaymentId != null)
          {
            yield return (object) this._WaitPurchase();
            if (this.paymentStatus == "ok")
            {
              yield return (object) Gsc.Purchase.Platform.DMMGamesStore.InnerFlow.CreateFulfillmentTask(this.handler, new List<string>()
              {
                this.waitingPaymentId
              });
              yield break;
            }
          }
          this.handler.OnPurchaseResult(ResultCode.Failed, (FulfillmentResult) null);
        }
      }

      private IEnumerator _GetPaymentId(string productId)
      {
        float startTime = Time.unscaledTime;
        do
        {
          WebInternalTask<PurchaseList, PurchaseList.Response> task = WebInternalTask.Create<PurchaseList, PurchaseList.Response>((Gsc.Network.Request<PurchaseList, PurchaseList.Response>) new PurchaseList(Gsc.Purchase.Platform.DMMGamesStore.ViewerID, Gsc.Purchase.Platform.DMMGamesStore.OnetimeToken));
          task.OnStart();
          yield return (object) task;
          task.OnFinish();
          switch (task.Result)
          {
            case WebTaskResult.Success:
              PurchaseList.Response.PurchaseInfo_t purchaseInfoT = ((IEnumerable<PurchaseList.Response.PurchaseInfo_t>) task.Response.Infos).Where<PurchaseList.Response.PurchaseInfo_t>((Func<PurchaseList.Response.PurchaseInfo_t, bool>) (x => x.ProductId == productId)).FirstOrDefault<PurchaseList.Response.PurchaseInfo_t>();
              if (purchaseInfoT != null)
              {
                this.waitingPaymentId = purchaseInfoT.PaymentId;
                yield break;
              }
              else
              {
                yield return (object) new WaitForSeconds(1f);
                break;
              }
            case WebTaskResult.ServerError:
              yield return (object) new WaitForSeconds(1f);
              break;
            default:
              yield break;
          }
          task = (WebInternalTask<PurchaseList, PurchaseList.Response>) null;
        }
        while ((double) Time.unscaledTime - (double) startTime < 30.0);
      }

      private IEnumerator _WaitPurchase()
      {
        float startTime = Time.unscaledTime;
        do
        {
          WebInternalTask<PurchaseUpdate, PurchaseUpdate.Response> task = WebInternalTask.Create<PurchaseUpdate, PurchaseUpdate.Response>((Gsc.Network.Request<PurchaseUpdate, PurchaseUpdate.Response>) new PurchaseUpdate(Gsc.Purchase.Platform.DMMGamesStore.ViewerID, Gsc.Purchase.Platform.DMMGamesStore.OnetimeToken, new List<string>()
          {
            this.waitingPaymentId
          }));
          task.OnStart();
          yield return (object) task;
          task.OnFinish();
          switch (task.Result)
          {
            case WebTaskResult.Success:
              PurchaseUpdate.Response.Status_t result = task.Response.Results[0];
              if (result.Status != "waiting")
              {
                this.paymentStatus = result.Status;
                yield break;
              }
              else
              {
                yield return (object) new WaitForSeconds(1f);
                break;
              }
            case WebTaskResult.ServerError:
              yield return (object) new WaitForSeconds(1f);
              break;
            default:
              yield break;
          }
          task = (WebInternalTask<PurchaseUpdate, PurchaseUpdate.Response>) null;
        }
        while ((double) Time.unscaledTime - (double) startTime < 30.0);
      }

      private static IEnumerator _Resume(PurchaseHandler handler)
      {
        WebInternalTask<PurchaseList, PurchaseList.Response> task1 = WebInternalTask.Create<PurchaseList, PurchaseList.Response>((Gsc.Network.Request<PurchaseList, PurchaseList.Response>) new PurchaseList(Gsc.Purchase.Platform.DMMGamesStore.ViewerID, Gsc.Purchase.Platform.DMMGamesStore.OnetimeToken));
        task1.OnStart();
        yield return (object) task1;
        task1.OnFinish();
        if (task1.Result == WebTaskResult.Success)
        {
          List<string> list = ((IEnumerable<PurchaseList.Response.PurchaseInfo_t>) task1.Response.Infos).Select<PurchaseList.Response.PurchaseInfo_t, string>((Func<PurchaseList.Response.PurchaseInfo_t, string>) (x => x.PaymentId)).ToList<string>();
          task1 = (WebInternalTask<PurchaseList, PurchaseList.Response>) null;
          if (list != null && list.Count > 0)
          {
            WebInternalTask<PurchaseUpdate, PurchaseUpdate.Response> task2 = WebInternalTask.Create<PurchaseUpdate, PurchaseUpdate.Response>((Gsc.Network.Request<PurchaseUpdate, PurchaseUpdate.Response>) new PurchaseUpdate(Gsc.Purchase.Platform.DMMGamesStore.ViewerID, Gsc.Purchase.Platform.DMMGamesStore.OnetimeToken, list));
            task2.OnStart();
            yield return (object) task2;
            task2.OnFinish();
            if (task2.Result != WebTaskResult.Success)
            {
              yield break;
            }
            else
            {
              list = ((IEnumerable<PurchaseUpdate.Response.Status_t>) task2.Response.Results).Where<PurchaseUpdate.Response.Status_t>((Func<PurchaseUpdate.Response.Status_t, bool>) (x => x.Status == "ok")).Select<PurchaseUpdate.Response.Status_t, string>((Func<PurchaseUpdate.Response.Status_t, string>) (x => x.PaymentId)).ToList<string>();
              task2 = (WebInternalTask<PurchaseUpdate, PurchaseUpdate.Response>) null;
            }
          }
          if (list != null && list.Count > 0)
            yield return (object) Gsc.Purchase.Platform.DMMGamesStore.InnerFlow.CreateFulfillmentTask(handler, list);
        }
      }

      private static IWebTask CreateFulfillmentTask(
        PurchaseHandler handler,
        List<string> paymentIds)
      {
        List<Gsc.Purchase.API.PAS.DMMGamesStore.Fulfillment.PurchaseData_t> purchaseDataList = new List<Gsc.Purchase.API.PAS.DMMGamesStore.Fulfillment.PurchaseData_t>(paymentIds.Count);
        for (int index = 0; index < paymentIds.Count; ++index)
          purchaseDataList.Add(new Gsc.Purchase.API.PAS.DMMGamesStore.Fulfillment.PurchaseData_t(Gsc.Purchase.Platform.DMMGamesStore.ViewerID, Gsc.Purchase.Platform.DMMGamesStore.OnetimeToken, paymentIds[index]));
        WebTask<Gsc.Purchase.API.PAS.DMMGamesStore.Fulfillment, Gsc.Purchase.API.Response.Fulfillment> task = new Gsc.Purchase.API.PAS.DMMGamesStore.Fulfillment(Gsc.Auth.Session.DefaultSession.DeviceID, purchaseDataList).GetTask(WebTaskAttribute.Reliable | WebTaskAttribute.Silent | WebTaskAttribute.Parallel);
        task.OnResponse((VoidCallbackWithError<Gsc.Purchase.API.Response.Fulfillment>) ((response, error) =>
        {
          if (error != null)
            handler.OnPurchaseResult(ResultCode.AlreadyOwned, (FulfillmentResult) null);
          else
            handler.OnPurchaseResult(ResultCode.Succeeded, response.Result);
        }));
        return (IWebTask) task;
      }
    }
  }
}
