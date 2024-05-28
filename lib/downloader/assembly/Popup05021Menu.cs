// Decompiled with JetBrains decompiler
// Type: Popup05021Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
public class Popup05021Menu : BackButtonMenuBase
{
  private Action onBackCallback;

  public void Init(Action onBackCallback) => this.onBackCallback = onBackCallback;

  public override void onBackButton() => this.IbtnNo();

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Action onBackCallback = this.onBackCallback;
    if (onBackCallback == null)
      return;
    onBackCallback();
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Persist.earthBattleEnvironment.Delete();
  }
}
