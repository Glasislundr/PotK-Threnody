// Decompiled with JetBrains decompiler
// Type: TouchFloatingDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TouchFloatingDialog : FloatingDialogBase
{
  private bool touchEnable = true;

  protected bool TouchEnable
  {
    get => this.touchEnable;
    set => this.touchEnable = value;
  }

  protected override void Update()
  {
    if (!Input.GetMouseButtonDown(0) || !this.IsShow || !this.touchEnable)
      return;
    this.Hide();
  }

  public override void Show()
  {
    ((Component) this).gameObject.SetActive(true);
    if (Object.op_Inequality((Object) this.tweenAlpha, (Object) null))
    {
      ((Behaviour) this.tweenAlpha).enabled = true;
      ((UITweener) this.tweenAlpha).onFinished.Clear();
      ((UITweener) this.tweenAlpha).PlayForward();
    }
    if (Object.op_Inequality((Object) this.tweenScale, (Object) null))
    {
      ((Behaviour) this.tweenScale).enabled = true;
      ((UITweener) this.tweenScale).onFinished.Clear();
      ((UITweener) this.tweenScale).PlayForward();
    }
    this.touchEnable = true;
    this.show = true;
  }

  public override void Hide()
  {
    this.touchEnable = false;
    EventDelegate.Callback callback = (EventDelegate.Callback) (() =>
    {
      if ((Object.op_Equality((Object) this.tweenAlpha, (Object) null) ? 0 : (!((Behaviour) this.tweenAlpha).enabled ? 1 : 0)) != (Object.op_Equality((Object) this.tweenScale, (Object) null) ? 0 : (!((Behaviour) this.tweenScale).enabled ? 1 : 0)))
        return;
      this.show = false;
      ((Component) this).gameObject.SetActive(false);
    });
    if (Object.op_Inequality((Object) this.tweenAlpha, (Object) null))
    {
      ((UITweener) this.tweenAlpha).onFinished.Clear();
      ((UITweener) this.tweenAlpha).AddOnFinished(callback);
      ((UITweener) this.tweenAlpha).PlayReverse();
    }
    if (!Object.op_Inequality((Object) this.tweenScale, (Object) null))
      return;
    ((UITweener) this.tweenScale).onFinished.Clear();
    ((UITweener) this.tweenScale).AddOnFinished(callback);
    ((UITweener) this.tweenScale).PlayReverse();
  }
}
