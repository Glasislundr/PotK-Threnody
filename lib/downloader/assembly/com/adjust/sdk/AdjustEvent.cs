﻿// Decompiled with JetBrains decompiler
// Type: com.adjust.sdk.AdjustEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace com.adjust.sdk
{
  public class AdjustEvent
  {
    internal string currency;
    internal string eventToken;
    internal string callbackId;
    internal string transactionId;
    internal double? revenue;
    internal List<string> partnerList;
    internal List<string> callbackList;
    internal string receipt;
    internal bool isReceiptSet;

    public AdjustEvent(string eventToken)
    {
      this.eventToken = eventToken;
      this.isReceiptSet = false;
    }

    public void setRevenue(double amount, string currency)
    {
      this.revenue = new double?(amount);
      this.currency = currency;
    }

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

    public void setTransactionId(string transactionId) => this.transactionId = transactionId;

    public void setCallbackId(string callbackId) => this.callbackId = callbackId;

    [Obsolete("This is an obsolete method. Please use the adjust purchase SDK for purchase verification (https://github.com/adjust/unity_purchase_sdk)")]
    public void setReceipt(string receipt, string transactionId)
    {
      this.receipt = receipt;
      this.transactionId = transactionId;
      this.isReceiptSet = true;
    }
  }
}
