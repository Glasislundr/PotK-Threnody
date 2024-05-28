// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustAppStoreSubscription
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace com.adjust.sdk
{
  public class AdjustAppStoreSubscription
  {
    internal string price;
    internal string currency;
    internal string transactionId;
    internal string receipt;
    internal string billingStore;
    internal string transactionDate;
    internal string salesRegion;
    internal List<string> partnerList;
    internal List<string> callbackList;

    public AdjustAppStoreSubscription(
      string price,
      string currency,
      string transactionId,
      string receipt)
    {
      this.price = price;
      this.currency = currency;
      this.transactionId = transactionId;
      this.receipt = receipt;
    }

    public void setTransactionDate(string transactionDate)
    {
      this.transactionDate = transactionDate;
    }

    public void setSalesRegion(string salesRegion) => this.salesRegion = salesRegion;

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
