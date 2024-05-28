// Decompiled with JetBrains decompiler
// Type: PurchaseListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Gsc.Core;
using Gsc.Purchase;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public class PurchaseListener : IPurchaseGlobalListener, IPurchaseResultListener
{
  private int currentCoint;

  public void OnInitialized() => Debug.Log((object) "*** PurchaseListener OnInitialized ***");

  public void OnPurchaseSucceeded(FulfillmentResult result)
  {
    PurchaseBehaviorLoadingLayer.Disable();
    int currentPaidCoin = result.CurrentPaidCoin;
    int currentFreeCoin = result.CurrentFreeCoin;
    int coinInBattleHere = PurchaseBehavior.UsedCoinInBattleHere;
    Player data = SMManager.Get<Player>();
    if (data != null)
    {
      data.free_coin = currentFreeCoin;
      data.paid_coin = currentPaidCoin;
      SMManager.Change<Player>(data);
    }
    this.currentCoint = currentPaidCoin + currentFreeCoin + data.common_coin - coinInBattleHere;
    foreach (FulfillmentResult.OrderInfo succeededTransaction in result.SucceededTransactions)
      this.OnPurchaseSucceeded(succeededTransaction);
    foreach (FulfillmentResult.OrderInfo duplicatedTransaction in result.DuplicatedTransactions)
      this.OnPurchaseSucceeded(duplicatedTransaction);
    ImmortalObject.Instance.StartCoroutine(WebAPI.CoinbonusPresent().Wait());
  }

  private void OnPurchaseSucceeded(FulfillmentResult.OrderInfo order)
  {
    ProductInfo productInfo = ((IEnumerable<ProductInfo>) PurchaseFlow.ProductList).Where<ProductInfo>((Func<ProductInfo, bool>) (x => x.ProductId == order.ProductId)).FirstOrDefault<ProductInfo>();
    if (productInfo == null)
      return;
    int addCoin = order.PaidCoin + order.FreeCoin;
    if (addCoin == 0)
    {
      CoinProduct activeProductData = MasterData.CoinProductList.GetActiveProductData(order.ProductId);
      addCoin = activeProductData.additional_free_coin + activeProductData.additional_paid_coin;
    }
    Shop0079Menu.OnPurchaseSucceeded(productInfo, this.currentCoint, addCoin);
    this.currentCoint -= addCoin;
    EventTracker.SendPayment(productInfo);
  }

  public void OnPurchaseFailed() => Debug.Log((object) "*** PurchaseListener OnPurchaseFailed ***");

  public void OnPurchaseCanceled()
  {
    Debug.Log((object) "*** PurchaseListener OnPurchaseCanceled ***");
  }

  public void OnPurchaseAlreadyOwned()
  {
    Debug.Log((object) "*** PurchaseListener OnPurchaseAlreadyOwned ***");
  }

  public void OnPurchaseDeferred()
  {
    Debug.Log((object) "*** PurchaseListener OnPurchaseDeferred ***");
  }

  public void OnPurchasePending()
  {
    Debug.Log((object) "*** PurchaseListener OnPurchasePending ***");
  }

  public void OnPurchasePendingExists()
  {
    Debug.Log((object) "*** PurchaseListener OnPurchasePendingExists ***");
  }

  public void OnOverCreditLimited()
  {
    Debug.Log((object) "*** PurchaseListener OnOverCreditLimited ***");
  }

  public void OnInsufficientBalances()
  {
  }

  public void OnFinished(bool isSuccess)
  {
    Debug.Log((object) ("*** PurchaseListener OnFinished : " + isSuccess.ToString() + " ***"));
  }
}
