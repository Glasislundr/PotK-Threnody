// Decompiled with JetBrains decompiler
// Type: FollowColorButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Custum/UI/FollowColorButton")]
public class FollowColorButton : UIButton
{
  [SerializeField]
  private Color followNormal = Color.white;
  [SerializeField]
  private Color followHover = new Color(0.882352948f, 0.784313738f, 0.5882353f, 1f);
  [SerializeField]
  private Color followPressed = new Color(0.7176471f, 0.6392157f, 0.482352942f, 1f);
  [SerializeField]
  private Color followDisabled = Color.grey;
  protected Color followColor;
  [SerializeField]
  private UIWidget[] followWidgets;

  protected virtual void OnInit()
  {
    base.OnInit();
    this.followColor = this.followNormal;
    if (this.followWidgets != null && this.followWidgets.Length != 0)
      return;
    UIWidget[] noent = ((Component) this).gameObject.GetComponents<UIWidget>();
    this.followWidgets = ((IEnumerable<UIWidget>) ((Component) this).gameObject.GetComponentsInChildren<UIWidget>(true)).Where<UIWidget>((Func<UIWidget, bool>) (x => !((IEnumerable<UIWidget>) noent).Contains<UIWidget>(x))).ToArray<UIWidget>();
  }

  public void SetTweenColor(bool instant, float duration, Color color)
  {
    foreach (UIRect followWidget in this.followWidgets)
    {
      GameObject cachedGameObject = followWidget.cachedGameObject;
      if (cachedGameObject.activeSelf)
      {
        TweenColor tweenColor = TweenColor.Begin(cachedGameObject, duration, color);
        if (instant && Object.op_Inequality((Object) tweenColor, (Object) null) || (double) duration <= 0.0)
        {
          tweenColor.value = tweenColor.to;
          ((Behaviour) tweenColor).enabled = false;
        }
      }
    }
  }

  protected virtual void SetState(UIButtonColor.State state, bool instant)
  {
    UIButtonColor.State mState = ((UIButtonColor) this).mState;
    base.SetState(state, instant);
    UIButtonColor.State state1 = state;
    if (mState == state1)
      return;
    switch (state - 1)
    {
      case 0:
        this.followColor = this.followHover;
        break;
      case 1:
        this.followColor = this.followPressed;
        break;
      case 2:
        this.followColor = this.followDisabled;
        break;
      default:
        this.followColor = this.followNormal;
        break;
    }
    this.SetTweenColor(instant, ((UIButtonColor) this).duration, this.followColor);
  }

  protected virtual void OnHover(bool isOver)
  {
    ((UIButtonColor) this).hover = ((UIButtonColor) this).mColor;
    this.followHover = this.followColor;
  }

  protected virtual void OnPress(bool isPressed)
  {
    if (!((UIButtonColor) this).isEnabled || UICamera.currentTouch == null)
      return;
    if (!((UIButtonColor) this).mInitDone)
      ((UIButtonColor) this).OnInit();
    if (!Object.op_Inequality((Object) ((UIButtonColor) this).tweenTarget, (Object) null))
      return;
    if (isPressed)
      ((UIButtonColor) this).SetState((UIButtonColor.State) 2, false);
    else if (Object.op_Equality((Object) UICamera.currentTouch.current, (Object) ((Component) this).gameObject))
    {
      if (UICamera.currentScheme == 2)
        ((UIButtonColor) this).SetState((UIButtonColor.State) 1, false);
      else if (UICamera.currentScheme == null && Object.op_Equality((Object) UICamera.hoveredObject, (Object) ((Component) this).gameObject))
        ((UIButtonColor) this).SetState((UIButtonColor.State) 0, false);
      else
        ((UIButtonColor) this).SetState((UIButtonColor.State) 0, false);
    }
    else
      ((UIButtonColor) this).SetState((UIButtonColor.State) 0, false);
  }
}
