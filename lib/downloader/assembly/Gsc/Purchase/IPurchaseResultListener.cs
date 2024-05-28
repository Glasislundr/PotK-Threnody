// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.IPurchaseResultListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Purchase
{
  public interface IPurchaseResultListener
  {
    void OnPurchaseSucceeded(FulfillmentResult result);

    void OnPurchaseFailed();

    void OnPurchaseCanceled();

    void OnPurchaseAlreadyOwned();

    void OnPurchaseDeferred();

    void OnPurchasePending();

    void OnPurchasePendingExists();

    void OnOverCreditLimited();

    void OnInsufficientBalances();

    void OnFinished(bool isSuccess);
  }
}
