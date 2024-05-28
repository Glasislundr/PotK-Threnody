// Decompiled with JetBrains decompiler
// Type: Popup005513Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Popup005513Menu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private GameObject txtDescriptionLimit;
  private Action action;

  public void Init(Action action, bool isMoneyOverAlert)
  {
    this.action = action;
    this.txtDescriptionLimit.SetActive(isMoneyOverAlert);
  }

  public void IbtnYes()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (this.action == null)
      return;
    this.action();
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
