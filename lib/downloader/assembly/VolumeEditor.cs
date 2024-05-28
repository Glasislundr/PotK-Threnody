// Decompiled with JetBrains decompiler
// Type: VolumeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("Utility/Behaviour/VolumeEditor")]
public class VolumeEditor : MonoBehaviour
{
  [Header("値をボタンとSliderで編集用設定")]
  public UIButton btnMin;
  public UIButton btnMinus;
  public UISlider uiSlider;
  public UIButton btnPlus;
  public UIButton btnMax;
  public int minVolume;
  public int maxVolume = 999;
  public UILabel labelMin;
  public UILabel labelMax;
  private int? minReal_;
  private int? maxReal_;

  public int nowVolume { get; private set; }

  public Func<int, int> onChangedVolume { get; set; }

  public int minReal
  {
    get
    {
      return !this.minReal_.HasValue ? (this.minReal_ = new int?(this.minVolume)).Value : this.minReal_.Value;
    }
    set => this.minReal_ = new int?(value);
  }

  public int maxReal
  {
    get
    {
      return !this.maxReal_.HasValue ? (this.maxReal_ = new int?(this.maxVolume)).Value : this.maxReal_.Value;
    }
    set => this.maxReal_ = new int?(value);
  }

  public void initialize(int value = 0)
  {
    if (Object.op_Inequality((Object) this.labelMin, (Object) null))
      this.labelMin.SetTextLocalize(this.minVolume);
    if (Object.op_Inequality((Object) this.labelMax, (Object) null))
      this.labelMax.SetTextLocalize(this.maxVolume);
    this.setVolume(value, true);
  }

  public void resetMaxVolume(int max)
  {
    this.maxVolume = max;
    this.maxReal_ = new int?();
    if (!Object.op_Inequality((Object) this.labelMax, (Object) null))
      return;
    this.labelMax.SetTextLocalize(this.maxVolume);
  }

  public void setVolume(int value, bool setSlider = false)
  {
    int? nullable = new int?();
    int num;
    do
    {
      num = this.clampVolume(value);
      if (setSlider || value != num)
        ((UIProgressBar) this.uiSlider).value = (float) (num - this.minVolume) / (float) (this.maxVolume - this.minVolume);
      ((UIButtonColor) this.btnMin).isEnabled = ((UIButtonColor) this.btnMinus).isEnabled = this.minReal < num;
      ((UIButtonColor) this.btnMax).isEnabled = ((UIButtonColor) this.btnPlus).isEnabled = this.maxReal > num;
      this.nowVolume = num;
      if (!nullable.HasValue)
      {
        Func<int, int> onChangedVolume = this.onChangedVolume;
        nullable = onChangedVolume != null ? new int?(onChangedVolume(num)) : new int?();
        if (nullable.HasValue)
        {
          value = nullable.Value;
          setSlider = true;
        }
      }
      else
        goto label_7;
    }
    while (nullable.HasValue && nullable.Value != num);
    goto label_8;
label_7:
    return;
label_8:;
  }

  public virtual int clampVolume(int value) => Mathf.Clamp(value, this.minReal, this.maxReal);

  public virtual void onChangeSlider()
  {
    this.setVolume(Mathf.RoundToInt(((UIProgressBar) this.uiSlider).value * (float) (this.maxVolume - this.minVolume)) + this.minVolume);
  }

  public virtual void onClickedMin() => this.setVolume(this.minReal, true);

  public virtual void onClickedMinus() => this.setVolume(this.nowVolume - 1, true);

  public virtual void onClickedPlus() => this.setVolume(this.nowVolume + 1, true);

  public virtual void onClickedMax() => this.setVolume(this.maxReal, true);
}
