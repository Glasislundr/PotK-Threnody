// Decompiled with JetBrains decompiler
// Type: Popup0235MenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup0235MenuBase : BackButtonMenuBase
{
  protected Action onCallback;

  public void SetCallback(Action callback) => this.onCallback = callback;

  public virtual IEnumerator Init(
    ResultMenuBase.CampaignReward reward,
    ResultMenuBase.CampaignNextReward nextReward,
    GameObject gearObject,
    GameObject unitObject,
    GameObject uniqueObject)
  {
    yield break;
  }

  public void IbtnOK()
  {
    if (this.IsPushAndSet())
      return;
    if (this.onCallback != null)
      this.onCallback();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnOK();
}
