// Decompiled with JetBrains decompiler
// Type: NGTweenGaugeWidth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class NGTweenGaugeWidth : MonoBehaviour
{
  [SerializeField]
  protected GameObject gauge;
  private UIWidget uiWidget;
  private int defaultWidth;
  private float gaugeValue = -1f;
  private TweenWidth _gaugeTweener;

  private TweenWidth gaugeTweener
  {
    get
    {
      if (Object.op_Equality((Object) this._gaugeTweener, (Object) null))
      {
        TweenWidth[] componentsInChildren = this.gauge.GetComponentsInChildren<TweenWidth>();
        if (componentsInChildren.Length != 0)
          this._gaugeTweener = componentsInChildren[0];
      }
      return this._gaugeTweener;
    }
  }

  public bool setValue(int n, int max, bool doTween = true, float delay = -1f, float duration = -1f)
  {
    if (Object.op_Inequality((Object) this.gaugeTweener, (Object) null))
    {
      if ((double) delay < 0.0)
        delay = ((UITweener) this.gaugeTweener).delay;
      if ((double) duration < 0.0)
        duration = ((UITweener) this.gaugeTweener).duration;
    }
    if (Object.op_Equality((Object) this.uiWidget, (Object) null))
    {
      this.uiWidget = this.gauge.GetComponent<UIWidget>();
      this.defaultWidth = this.uiWidget.width;
    }
    if (Object.op_Equality((Object) this.uiWidget, (Object) null) || Mathf.Approximately((float) max, 0.0f))
      return false;
    float num = (float) n / (float) max;
    if (Mathf.Approximately(this.gaugeValue, num))
      return false;
    if (doTween && Object.op_Inequality((Object) this.gaugeTweener, (Object) null))
    {
      this.gaugeTweener.from = this.uiWidget.width;
      this.gaugeTweener.to = Mathf.FloorToInt((float) this.defaultWidth * num);
      ((UITweener) this.gaugeTweener).duration = duration;
      ((UITweener) this.gaugeTweener).delay = delay;
      NGTween.playTween((UITweener) this.gaugeTweener);
    }
    else
    {
      if (Object.op_Inequality((Object) this.gaugeTweener, (Object) null))
        ((Behaviour) this.gaugeTweener).enabled = false;
      this.uiWidget.width = Mathf.FloorToInt((float) this.defaultWidth * num);
    }
    this.gaugeValue = num;
    return true;
  }
}
