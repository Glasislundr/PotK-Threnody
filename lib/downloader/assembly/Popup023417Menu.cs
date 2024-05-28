// Decompiled with JetBrains decompiler
// Type: Popup023417Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
public class Popup023417Menu : BackButtonMenuBase
{
  public Action execChangeScene { get; set; }

  public virtual void IbtnBack() => Singleton<PopupManager>.GetInstance().onDismiss();

  public virtual void IbtnTeamSetting()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.execChangeScene();
  }

  public void IbtnNo() => this.IbtnBack();

  public override void onBackButton() => this.IbtnNo();
}
