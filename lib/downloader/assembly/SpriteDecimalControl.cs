// Decompiled with JetBrains decompiler
// Type: SpriteDecimalControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SpriteDecimalControl : MonoBehaviour
{
  [SerializeField]
  [Tooltip("ONの時、表現数値以上の桁を0で埋める")]
  private bool fillZero_;
  [SerializeField]
  [Tooltip("字詰用。未設定時は字詰無し")]
  private UIGrid repositionControl_;
  [SerializeField]
  [Tooltip("1の位から上位桁の順に設定")]
  private SpriteNumberSelectDirect[] digits_;
  private bool firstReposition_ = true;
  private const int UNIT_DECIMAL = 10;
  private int? wMaxValue_;

  private void Awake() => this.initialize();

  private void initialize()
  {
    if (this.digits_ != null && this.digits_.Length != 0)
      return;
    this.digits_ = ((Component) this).GetComponentsInChildren<SpriteNumberSelectDirect>(true);
  }

  private int maxValue_
  {
    get
    {
      if (this.wMaxValue_.HasValue)
        return this.wMaxValue_.Value;
      this.initialize();
      int num = 1;
      foreach (SpriteNumberSelectDirect digit in this.digits_)
        num *= 10;
      int maxValue = num - 1;
      this.wMaxValue_ = new int?(maxValue);
      return maxValue;
    }
  }

  public void resetNumber(int num) => this.setNumber(num, true);

  public void setNumber(int num) => this.setNumber(num, false);

  private void setNumber(int value, bool immediate)
  {
    value = Mathf.Min(value, this.maxValue_);
    int num = this.fillZero_ ? this.digits_.Length : (value != 0 ? (int) Mathf.Log10((float) value) + 1 : 1);
    if (immediate)
      this.firstReposition_ = true;
    bool flag = this.firstReposition_;
    foreach (SpriteNumberSelectDirect digit in this.digits_)
    {
      if (num > 0)
      {
        if (!((Component) digit).gameObject.activeSelf)
        {
          ((Component) digit).gameObject.SetActive(true);
          flag = true;
        }
        digit.setNumber(value % 10);
        value /= 10;
        --num;
      }
      else if (((Component) digit).gameObject.activeSelf)
      {
        ((Component) digit).gameObject.SetActive(false);
        flag = true;
      }
    }
    if (!(!this.fillZero_ & flag) || !Object.op_Inequality((Object) this.repositionControl_, (Object) null))
      return;
    if (this.firstReposition_)
    {
      this.repositionControl_.animateSmoothly = false;
      // ISSUE: method pointer
      this.repositionControl_.onReposition = new UIGrid.OnReposition((object) this, __methodptr(\u003CsetNumber\u003Eb__12_0));
    }
    else
      this.repositionControl_.animateSmoothly = true;
    this.repositionControl_.repositionNow = true;
    this.repositionControl_.Reposition();
  }
}
