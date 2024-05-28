// Decompiled with JetBrains decompiler
// Type: NGTweenGaugeFillAmount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NGTweenGaugeFillAmount : MonoBehaviour
{
  [SerializeField]
  protected GameObject gauge;
  private TweenFillAmount _gaugeTweener;

  private TweenFillAmount gaugeTweener
  {
    get
    {
      if (Object.op_Equality((Object) this._gaugeTweener, (Object) null))
      {
        TweenFillAmount[] componentsInChildren = this.gauge.GetComponentsInChildren<TweenFillAmount>();
        if (componentsInChildren.Length != 0)
          this._gaugeTweener = componentsInChildren[0];
      }
      return this._gaugeTweener;
    }
  }

  public bool isAnimationPlaying
  {
    get
    {
      return Object.op_Inequality((Object) this._gaugeTweener, (Object) null) && ((Behaviour) this._gaugeTweener).isActiveAndEnabled;
    }
  }

  public bool setValue(int targetValue, int maxValue, bool doTween, float delay = -1f, float duration = -1f)
  {
    if (Object.op_Inequality((Object) this.gaugeTweener, (Object) null))
    {
      if ((double) delay < 0.0)
        delay = this.gaugeTweener.delay;
      if ((double) duration < 0.0)
        duration = this.gaugeTweener.duration;
    }
    UISprite component = this.gauge.GetComponent<UISprite>();
    if (Mathf.Approximately((float) maxValue, 0.0f))
      return false;
    float num = (float) targetValue / (float) maxValue;
    if (doTween && Object.op_Inequality((Object) this.gaugeTweener, (Object) null))
    {
      this.gaugeTweener.from = component.fillAmount;
      this.gaugeTweener.to = num;
      this.gaugeTweener.duration = duration;
      this.gaugeTweener.delay = delay;
      NGTween.playTween((UITweener) this.gaugeTweener);
    }
    else
      component.fillAmount = num;
    return true;
  }

  public void setValue(
    float fromPercent,
    float toPercent,
    bool doTween,
    float delay = -1f,
    float duration = -1f)
  {
    if (Object.op_Inequality((Object) this.gaugeTweener, (Object) null))
    {
      if ((double) delay < 0.0)
        delay = this.gaugeTweener.delay;
      if ((double) duration < 0.0)
        duration = this.gaugeTweener.duration;
    }
    UISprite component = this.gauge.GetComponent<UISprite>();
    if (doTween && Object.op_Inequality((Object) this.gaugeTweener, (Object) null))
    {
      this.gaugeTweener.from = fromPercent;
      this.gaugeTweener.to = toPercent;
      this.gaugeTweener.duration = duration;
      this.gaugeTweener.delay = delay;
      NGTween.playTween((UITweener) this.gaugeTweener);
    }
    else
      component.fillAmount = toPercent;
  }
}
