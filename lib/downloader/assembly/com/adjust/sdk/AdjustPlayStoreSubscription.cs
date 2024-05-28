// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustPlayStoreSubscription
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace com.adjust.sdk
{
  public class AdjustPlayStoreSubscription
  {
    internal string price;
    internal string currency;
    internal string sku;
    internal string orderId;
    internal string signature;
    internal string purchaseToken;
    internal string billingStore;
    internal string purchaseTime;
    internal List<string> partnerList;
    internal List<string> callbackList;

    public AdjustPlayStoreSubscription(
      string price,
      string currency,
      string sku,
      string orderId,
      string signature,
      string purchaseToken)
    {
      this.price = price;
      this.currency = currency;
      this.sku = sku;
      this.orderId = orderId;
      this.signature = signature;
      this.purchaseToken = purchaseToken;
    }

    public void setPurchaseTime(string purchaseTime) => this.purchaseTime = purchaseTime;

    public void addCallbackParameter(string key, string value)
    {
      if (this.callbackList == null)
        this.callbackList = new List<string>();
      this.callbackList.Add(key);
      this.callbackList.Add(value);
    }

    public void addPartnerParameter(string key, string value)
    {
      if (this.partnerList == null)
        this.partnerList = new List<string>();
      this.partnerList.Add(key);
      this.partnerList.Add(value);
    }
  }
}
