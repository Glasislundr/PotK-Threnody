// Decompiled with JetBrains decompiler
// Type: SpreadColorButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpreadColorButton : UIButton
{
  private UISprite btnSprite;
  private UIWidget[] objs;

  protected virtual void OnInit()
  {
    base.OnInit();
    this.btnSprite = ((UIButtonColor) this).mWidget as UISprite;
    this.objs = ((Component) this).gameObject.GetComponentsInChildren<UIWidget>(true);
  }

  public void SetTweenColor(bool instant, float duration, Color color)
  {
    foreach (UIRect uiRect in this.objs)
    {
      TweenColor tweenColor = TweenColor.Begin(uiRect.cachedGameObject, duration, color);
      if (instant && Object.op_Inequality((Object) tweenColor, (Object) null) || (double) duration <= 0.0)
      {
        tweenColor.value = tweenColor.to;
        ((Behaviour) tweenColor).enabled = false;
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
    Color color = ((UIButtonColor) this).mColor;
    switch (state - 1)
    {
      case 0:
        color = ((UIButtonColor) this).hover;
        break;
      case 1:
        color = ((UIButtonColor) this).pressed;
        break;
      case 2:
        color = ((UIButtonColor) this).disabledColor;
        break;
    }
    this.SetTweenColor(instant, ((UIButtonColor) this).duration, color);
  }

  public void SetColor(Color color)
  {
    ((UIButtonColor) this).defaultColor = color;
    this.SetTweenColor(false, 0.0f, color);
  }

  protected virtual void OnHover(bool isOver)
  {
    ((UIButtonColor) this).hover = ((UIButtonColor) this).mColor;
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

  public void ReturnToNormal() => ((UIButtonColor) this).SetState((UIButtonColor.State) 0, true);

  public void SetSprite(string sp) => base.SetSprite(sp);
}
