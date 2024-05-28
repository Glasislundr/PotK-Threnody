// Decompiled with JetBrains decompiler
// Type: TweenFillAmount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("NGUI/Tween/Tween FillAmount")]
public class TweenFillAmount : UITweener
{
  public float from;
  public float to;
  public bool updateTable;
  private UISprite mSprite;
  private UITable mTable;

  public UISprite cachedSprite
  {
    get
    {
      if (Object.op_Equality((Object) this.mSprite, (Object) null))
        this.mSprite = ((Component) this).gameObject.GetComponent<UISprite>();
      return this.mSprite;
    }
  }

  public float value
  {
    get => this.cachedSprite.fillAmount;
    set => this.cachedSprite.fillAmount = value;
  }

  [Obsolete("Use 'value' instead")]
  public float fillAmount
  {
    get => this.value;
    set => this.value = value;
  }

  protected virtual void OnUpdate(float factor, bool isFinished)
  {
    this.value = (float) ((double) this.from * (1.0 - (double) factor) + (double) this.to * (double) factor);
    if (!this.updateTable)
      return;
    if (Object.op_Equality((Object) this.mTable, (Object) null))
    {
      this.mTable = NGUITools.FindInParents<UITable>(((Component) this).gameObject);
      if (Object.op_Equality((Object) this.mTable, (Object) null))
      {
        this.updateTable = false;
        return;
      }
    }
    this.mTable.repositionNow = true;
  }

  public static TweenFillAmount Begin(GameObject go, float duration, float fillAmount)
  {
    TweenFillAmount tweenFillAmount = UITweener.Begin<TweenFillAmount>(go, duration);
    tweenFillAmount.from = tweenFillAmount.value;
    tweenFillAmount.to = fillAmount;
    if ((double) duration <= 0.0)
    {
      tweenFillAmount.Sample(1f, true);
      ((Behaviour) tweenFillAmount).enabled = false;
    }
    return tweenFillAmount;
  }

  [ContextMenu("Set 'From' to current value")]
  public virtual void SetStartToCurrentValue() => this.from = this.value;

  [ContextMenu("Set 'To' to current value")]
  public virtual void SetEndToCurrentValue() => this.to = this.value;

  [ContextMenu("Assume value of 'From'")]
  private void SetCurrentValueToStart() => this.value = this.from;

  [ContextMenu("Assume value of 'To'")]
  private void SetCurrentValueToEnd() => this.value = this.to;
}
