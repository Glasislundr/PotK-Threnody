// Decompiled with JetBrains decompiler
// Type: SeaDateSpotUIButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SeaDateSpotUIButton : UIButton
{
  protected virtual void SetState(UIButtonColor.State state, bool immediate)
  {
    base.SetState(state, immediate);
    if (state != 2)
      this.playTweenScale(false);
    else
      this.playTweenScale(true);
  }

  protected void playTweenScale(bool isPressed)
  {
    TweenScale componentInChildren = ((Component) this).GetComponentInChildren<TweenScale>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    if (isPressed)
      ((UITweener) componentInChildren).PlayForward();
    else
      ((UITweener) componentInChildren).ResetToBeginning();
  }
}
