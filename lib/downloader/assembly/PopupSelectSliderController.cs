// Decompiled with JetBrains decompiler
// Type: PopupSelectSliderController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PopupSelectSliderController : MonoBehaviour
{
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UIButton minButton;
  [SerializeField]
  private UIButton maxButton;
  [SerializeField]
  private UIButton minusButton;
  [SerializeField]
  private UIButton plusButton;
  [SerializeField]
  private UILabel minLabel;
  [SerializeField]
  private UILabel maxLabel;
  private float minValue;
  private float maxValue;
  private float currentValue;
  private float stepValue;
  private bool useIntValue;
  private bool isLocked;
  private PopupSelectSliderController.SliderValueChangeListener sliderValueChangeListener;

  public void Initialize(
    float min,
    float max,
    float current,
    PopupSelectSliderController.SliderValueChangeListener listener = null,
    float step = 1f,
    bool roundToInt = true)
  {
    this.minValue = min;
    this.maxValue = max;
    this.currentValue = current;
    this.stepValue = step;
    this.useIntValue = roundToInt;
    this.sliderValueChangeListener = listener;
    this.minLabel.SetTextLocalize(this.minValue.ToString());
    this.maxLabel.SetTextLocalize(this.maxValue.ToString());
    this.UpdateSlider();
    this.UpdateButtons();
  }

  public void LockSlider() => this.isLocked = true;

  private void Awake() => this.DisableAllButtons();

  private void Update()
  {
    if (!this.isLocked)
      return;
    this.DisableAllButtons();
    ((Collider) ((Component) this.slider).GetComponentInChildren<BoxCollider>()).enabled = false;
    ((UIButtonColor) ((Component) ((UIProgressBar) this.slider).thumb).GetComponentInChildren<UIButton>()).isEnabled = false;
    ((Collider) ((Component) ((UIProgressBar) this.slider).thumb).GetComponentInChildren<BoxCollider>()).enabled = false;
  }

  private void UpdateSlider()
  {
    ((UIProgressBar) this.slider).value = (float) (((double) this.currentValue - (double) this.minValue) / ((double) this.maxValue - (double) this.minValue));
  }

  private void DisableAllButtons()
  {
    ((UIButtonColor) this.plusButton).isEnabled = false;
    ((UIButtonColor) this.maxButton).isEnabled = false;
    ((UIButtonColor) this.minusButton).isEnabled = false;
    ((UIButtonColor) this.minButton).isEnabled = false;
  }

  private void UpdateButtons()
  {
    this.DisableAllButtons();
    if ((double) this.currentValue < (double) this.maxValue)
    {
      ((UIButtonColor) this.plusButton).isEnabled = true;
      ((UIButtonColor) this.maxButton).isEnabled = true;
    }
    if ((double) this.currentValue <= (double) this.minValue)
      return;
    ((UIButtonColor) this.minusButton).isEnabled = true;
    ((UIButtonColor) this.minButton).isEnabled = true;
  }

  public void OnSliderValueChange()
  {
    this.currentValue = ((UIProgressBar) this.slider).value * (this.maxValue - this.minValue) + this.minValue;
    if (this.useIntValue)
    {
      this.currentValue = (float) Mathf.RoundToInt(this.currentValue);
      this.UpdateSlider();
    }
    this.UpdateButtons();
    if (this.sliderValueChangeListener == null)
      return;
    this.sliderValueChangeListener(this.currentValue);
  }

  public void OnClickMin()
  {
    this.currentValue = this.minValue;
    this.UpdateSlider();
    this.UpdateButtons();
  }

  public void OnClickMax()
  {
    this.currentValue = this.maxValue;
    this.UpdateSlider();
    this.UpdateButtons();
  }

  public void OnClickPlus()
  {
    this.currentValue += this.stepValue;
    if ((double) this.currentValue > (double) this.maxValue)
      this.currentValue = this.maxValue;
    this.UpdateSlider();
    this.UpdateButtons();
  }

  public void OnClickMinus()
  {
    this.currentValue -= this.stepValue;
    if ((double) this.currentValue < (double) this.minValue)
      this.currentValue = this.minValue;
    this.UpdateSlider();
    this.UpdateButtons();
  }

  public delegate void SliderValueChangeListener(float value);
}
