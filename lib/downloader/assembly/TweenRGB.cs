// Decompiled with JetBrains decompiler
// Type: TweenRGB
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Custum/Tween/Tween RGB")]
public class TweenRGB : UITweener
{
  public Color from = Color.white;
  public Color to = Color.white;
  private bool mCached;
  private UIWidget mWidget;
  private Material mMat;
  private Light mLight;

  private void Cache()
  {
    this.mCached = true;
    this.mWidget = ((Component) this).GetComponent<UIWidget>();
    Renderer component = ((Component) this).GetComponent<Renderer>();
    if (Object.op_Inequality((Object) component, (Object) null))
      this.mMat = component.material;
    this.mLight = ((Component) this).GetComponent<Light>();
    if (!Object.op_Equality((Object) this.mWidget, (Object) null) || !Object.op_Equality((Object) this.mMat, (Object) null) || !Object.op_Equality((Object) this.mLight, (Object) null))
      return;
    this.mWidget = ((Component) this).GetComponentInChildren<UIWidget>();
  }

  [Obsolete("Use 'value' instead")]
  public Color color
  {
    get => this.value;
    set => this.value = value;
  }

  public Color value
  {
    get
    {
      if (!this.mCached)
        this.Cache();
      if (Object.op_Inequality((Object) this.mWidget, (Object) null))
        return this.mWidget.color;
      if (Object.op_Inequality((Object) this.mLight, (Object) null))
        return this.mLight.color;
      return Object.op_Inequality((Object) this.mMat, (Object) null) ? this.mMat.color : Color.black;
    }
    set
    {
      if (!this.mCached)
        this.Cache();
      if (Object.op_Inequality((Object) this.mWidget, (Object) null))
        this.mWidget.color = new Color(value.r, value.g, value.b, this.mWidget.color.a);
      if (Object.op_Inequality((Object) this.mMat, (Object) null))
        this.mMat.color = new Color(value.r, value.g, value.b, this.mMat.color.a);
      if (!Object.op_Inequality((Object) this.mLight, (Object) null))
        return;
      this.mLight.color = new Color(value.r, value.g, value.b, this.mLight.color.a);
      ((Behaviour) this.mLight).enabled = (double) value.r + (double) value.g + (double) value.b > 0.0099999997764825821;
    }
  }

  protected virtual void OnUpdate(float factor, bool isFinished)
  {
    this.value = Color.Lerp(this.from, this.to, factor);
  }

  public static TweenRGB Begin(GameObject go, float duration, Color color)
  {
    TweenRGB tweenRgb = UITweener.Begin<TweenRGB>(go, duration);
    Color color1 = tweenRgb.value;
    tweenRgb.from = color1;
    tweenRgb.to = new Color(color.r, color.g, color.b, color1.a);
    if ((double) duration <= 0.0)
    {
      tweenRgb.Sample(1f, true);
      ((Behaviour) tweenRgb).enabled = false;
    }
    return tweenRgb;
  }

  [ContextMenu("Set 'From' to current value")]
  public virtual void SetStartToCurrentValue() => TweenRGB.setRGB(ref this.from, this.value);

  [ContextMenu("Set 'To' to current value")]
  public virtual void SetEndToCurrentValue() => TweenRGB.setRGB(ref this.to, this.value);

  [ContextMenu("Assume value of 'From'")]
  private void SetCurrentValueToStart() => this.value = this.from;

  [ContextMenu("Assume value of 'To'")]
  private void SetCurrentValueToEnd() => this.value = this.to;

  private static void setRGB(ref Color des, Color src)
  {
    des.r = src.r;
    des.g = src.g;
    des.b = src.b;
  }
}
