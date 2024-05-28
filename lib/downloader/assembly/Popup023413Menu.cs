// Decompiled with JetBrains decompiler
// Type: Popup023413Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class Popup023413Menu : BackButtonMenuBase
{
  public virtual void IbtnOk() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnOk();
}
