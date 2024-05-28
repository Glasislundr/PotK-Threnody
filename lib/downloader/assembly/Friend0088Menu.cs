// Decompiled with JetBrains decompiler
// Type: Friend0088Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Friend0088Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtMessage;
  private Action callback;

  public virtual void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (this.callback == null)
      return;
    this.callback();
  }

  public void SetMessage(string txt = null)
  {
    if (txt == null)
      return;
    this.txtMessage.SetTextLocalize(txt);
  }

  public void InitPopup(string txt = null, Action callback = null)
  {
    this.txtMessage.SetTextLocalize(txt);
    this.callback = callback;
  }

  public override void onBackButton() => this.IbtnOk();
}
