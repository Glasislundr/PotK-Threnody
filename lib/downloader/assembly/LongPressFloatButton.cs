// Decompiled with JetBrains decompiler
// Type: LongPressFloatButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LongPressFloatButton : FloatButton
{
  public List<EventDelegate> onLongPress_ = new List<EventDelegate>();
  public float longPressDuration_ = 1f;
  private bool isActiveLongPress_;
  private bool longPressed_;

  private void startLongPress()
  {
    this.isActiveLongPress_ = true;
    ((MonoBehaviour) this).StartCoroutine("DoLongPress");
    this.longPressed_ = false;
  }

  private void stopLongPress()
  {
    if (!this.isActiveLongPress_)
      return;
    ((MonoBehaviour) this).StopCoroutine("DoLongPress");
    this.isActiveLongPress_ = false;
  }

  private IEnumerator DoLongPress()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    LongPressFloatButton pressFloatButton = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      pressFloatButton.longPressed_ = true;
      UIButton.current = (UIButton) pressFloatButton;
      EventDelegate.Execute(pressFloatButton.onLongPress_);
      UIButton.current = (UIButton) null;
      pressFloatButton.isActiveLongPress_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(pressFloatButton.longPressDuration_);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected void OnDragStart() => this.stopLongPress();

  protected override void OnPress(bool isPressed)
  {
    base.OnPress(isPressed);
    if (!((UIButtonColor) this).isEnabled)
      return;
    if (isPressed)
      this.startLongPress();
    else
      this.stopLongPress();
  }

  protected virtual void OnClick()
  {
    if (this.longPressed_)
      return;
    base.OnClick();
  }

  protected virtual void OnDisable()
  {
    ((UIButtonColor) this).OnDisable();
    this.stopLongPress();
  }
}
