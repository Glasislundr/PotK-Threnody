// Decompiled with JetBrains decompiler
// Type: CorpsQuestClearEffectPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class CorpsQuestClearEffectPopup : BackButtonMenuBase
{
  private IEnumerator Start()
  {
    CorpsQuestClearEffectPopup clearEffectPopup = this;
    Animator anim = ((Component) clearEffectPopup).GetComponentInChildren<Animator>();
    AnimatorStateInfo animatorStateInfo;
    while (true)
    {
      animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
      if ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime == 0.0)
        yield return (object) null;
      else
        break;
    }
    while (true)
    {
      animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
      if ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0)
        yield return (object) null;
      else
        break;
    }
    clearEffectPopup.onBackButton();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
