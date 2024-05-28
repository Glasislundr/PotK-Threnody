﻿// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.IPurchaseFlowImpl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Purchase
{
  public interface IPurchaseFlowImpl
  {
    void Init(string[] productIds);

    void UpdateProducts(string[] productIds);

    void Resume();

    bool Confirmed();

    bool Purchase(ProductInfo product, string accountId);

    void Consume(string productId, string receiptId);
  }
}
