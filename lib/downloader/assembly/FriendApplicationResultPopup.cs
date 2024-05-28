// Decompiled with JetBrains decompiler
// Type: FriendApplicationResultPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class FriendApplicationResultPopup : BackButtonMenuBase
{
  public UIButton OK;

  public virtual void IbtnPopupOk() => Singleton<PopupManager>.GetInstance().dismiss();

  public override void onBackButton() => this.IbtnPopupOk();
}
