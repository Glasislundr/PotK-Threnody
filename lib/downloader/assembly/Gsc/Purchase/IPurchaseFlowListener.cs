// Decompiled with JetBrains decompiler
// Type: Gsc.Purchase.IPurchaseFlowListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace Gsc.Purchase
{
  public interface IPurchaseFlowListener : IPurchaseResultListener
  {
    void InputBirthday(PurchaseFlow flow);

    void Confirm(PurchaseFlow flow, ProductInfo product);

    void OnInvalidBirthday(PurchaseFlow flow);

    void OnProducts(PurchaseFlow flow, ProductInfo[] products);
  }
}
