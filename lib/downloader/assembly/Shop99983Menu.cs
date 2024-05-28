// Decompiled with JetBrains decompiler
// Type: Shop99983Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Shop99983Menu : BackButtonMenuBase
{
  public virtual void IbtnPopupOK()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll(true);
    this.StartCoroutine(PopupUtility.BuyKiseki());
  }

  public override void onBackButton() => this.IbtnPopupOK();
}
