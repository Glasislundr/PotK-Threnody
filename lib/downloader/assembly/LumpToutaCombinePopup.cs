// Decompiled with JetBrains decompiler
// Type: LumpToutaCombinePopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/LumpTouta/CombinePopup")]
public class LumpToutaCombinePopup : BackButtonMenuBase
{
  [SerializeField]
  private Transform dyn_thum;
  [SerializeField]
  private UILabel unitName;
  [SerializeField]
  private UILabel unitFlavor;
  [SerializeField]
  private UILabel selectNumValue;
  private int selectedCount;
  [SerializeField]
  private UILabel selectNumMaxValue;
  private int selectNumMax;
  [SerializeField]
  private UIButton ibtnMin;
  [SerializeField]
  private UIButton ibtnMax;
  [SerializeField]
  private UIButton ibtnPlus;
  [SerializeField]
  private UIButton ibtnMinus;
  [SerializeField]
  private UISlider selectSlider;
  private Action<int> onOk;

  public void Init(
    UnitIcon baseUnitIcon,
    UnitIconInfo unitIconInfo,
    int maxCount,
    Action<int> onOk)
  {
    this.onOk = onOk;
    this.SetUnitIcon(baseUnitIcon);
    this.unitName.text = unitIconInfo.unit.name;
    this.unitFlavor.text = unitIconInfo.unit.description;
    this.selectNumValue.text = unitIconInfo.count.ToString();
    this.selectedCount = unitIconInfo.count;
    this.selectNumMaxValue.text = maxCount.ToString();
    this.selectNumMax = maxCount;
    ((UIProgressBar) this.selectSlider).value = (float) this.selectedCount / (float) this.selectNumMax;
  }

  private void SetUnitIcon(UnitIcon baseUnitIcon)
  {
    UnitIcon component = ((Component) baseUnitIcon).gameObject.Clone(this.dyn_thum).GetComponent<UnitIcon>();
    component.HideCheckIcon();
    component.SetCounter(0);
  }

  public void sliderChange()
  {
    int num = Mathf.RoundToInt(((UIProgressBar) this.selectSlider).value * (float) this.selectNumMax);
    if (num < 0)
      num = 0;
    this.selectedCount = num;
    this.ChangeCurrentValue();
  }

  public void OnMinus()
  {
    this.selectedCount = Math.Max(0, this.selectedCount - 1);
    this.ChangeCurrentValue();
  }

  public void OnPlus()
  {
    this.selectedCount = Math.Min(this.selectNumMax, this.selectedCount + 1);
    this.ChangeCurrentValue();
  }

  public void OnMin()
  {
    this.selectedCount = 0;
    this.ChangeCurrentValue();
  }

  public void OnMax()
  {
    this.selectedCount = this.selectNumMax;
    this.ChangeCurrentValue();
  }

  private void ChangeCurrentValue()
  {
    ((UIProgressBar) this.selectSlider).value = (float) this.selectedCount / (float) this.selectNumMax;
    this.selectNumValue.text = this.selectedCount.ToString();
    if (this.selectedCount <= 0)
    {
      ((UIButtonColor) this.ibtnMin).isEnabled = false;
      ((UIButtonColor) this.ibtnMinus).isEnabled = false;
    }
    else
    {
      ((UIButtonColor) this.ibtnMin).isEnabled = true;
      ((UIButtonColor) this.ibtnMinus).isEnabled = true;
    }
    if (this.selectedCount >= this.selectNumMax)
    {
      ((UIButtonColor) this.ibtnMax).isEnabled = false;
      ((UIButtonColor) this.ibtnPlus).isEnabled = false;
    }
    else
    {
      ((UIButtonColor) this.ibtnMax).isEnabled = true;
      ((UIButtonColor) this.ibtnPlus).isEnabled = true;
    }
  }

  public void OnOK()
  {
    if (this.IsPushAndSet())
      return;
    this.onOk(this.selectedCount);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnRetrun();

  private void IbtnRetrun()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
