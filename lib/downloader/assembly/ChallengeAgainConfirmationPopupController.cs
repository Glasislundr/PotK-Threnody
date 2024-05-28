// Decompiled with JetBrains decompiler
// Type: ChallengeAgainConfirmationPopupController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ChallengeAgainConfirmationPopupController : MonoBehaviour
{
  private Action dismissAction;
  private Action challengeAction;

  public IEnumerator Init(Action dismissAction, Action challengeAction)
  {
    this.challengeAction = challengeAction;
    this.dismissAction = dismissAction;
    yield return (object) null;
  }

  public void OnChanllenge()
  {
    this.challengeAction();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void OnReturn()
  {
    this.dismissAction();
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
