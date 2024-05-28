// Decompiled with JetBrains decompiler
// Type: LongPressButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LongPressButton : UIButton
{
  public List<EventDelegate> onLongPress = new List<EventDelegate>();
  public Func<IEnumerator> onLongPressLoop;
  public float longPressDuration = 1f;
  private bool longPressed;
  private bool isActiveLongPress_;

  private void startLongPress()
  {
    this.isActiveLongPress_ = true;
    ((MonoBehaviour) this).StartCoroutine("DoLongPress");
    this.longPressed = false;
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
    LongPressButton longPressButton = this;
    yield return (object) new WaitForSeconds(longPressButton.longPressDuration);
    longPressButton.longPressed = true;
    UIButton.current = (UIButton) longPressButton;
    EventDelegate.Execute(longPressButton.onLongPress);
    if (longPressButton.onLongPressLoop != null)
    {
      IEnumerator e = longPressButton.onLongPressLoop();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    UIButton.current = (UIButton) null;
    longPressButton.isActiveLongPress_ = false;
  }

  protected void OnDragStart() => this.stopLongPress();

  protected virtual void OnPress(bool isPressed)
  {
    ((UIButtonColor) this).OnPress(isPressed);
    if (!((UIButtonColor) this).isEnabled)
      return;
    if (isPressed)
      this.startLongPress();
    else
      this.stopLongPress();
  }

  protected virtual void OnClick()
  {
    if (this.longPressed)
      return;
    base.OnClick();
  }

  protected virtual void OnDisable()
  {
    ((UIButtonColor) this).OnDisable();
    this.stopLongPress();
  }
}
