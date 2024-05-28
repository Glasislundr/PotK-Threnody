// Decompiled with JetBrains decompiler
// Type: CommonHeaderLevel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CommonHeaderLevel : MonoBehaviour
{
  public GameObject gauge;
  public UILabel levelText;
  private TweenRotation gaugeTweener;
  private float gaugeMax = 9f;
  private float gaugeMin = -74f;
  private int levelValue;
  private float gaugeValue;

  private void Awake()
  {
    this.gaugeTweener = this.gauge.GetComponent<TweenRotation>();
    this.gauge.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, this.gaugeMin);
  }

  public void setLevel(int v)
  {
    if (v == this.levelValue)
      return;
    this.levelText.SetTextLocalize(string.Concat((object) v));
    this.levelValue = v;
  }

  public void setExperience(int exp, int next)
  {
    float num1 = next == 0 ? 0.0f : (float) exp / (float) next;
    if ((double) this.gaugeValue == (double) num1)
      return;
    float num2 = this.gauge.transform.localEulerAngles.z;
    float num3 = this.gaugeMin + Mathf.Abs(this.gaugeMax - this.gaugeMin) * num1;
    if ((double) Mathf.Round(num2) > (double) Mathf.Round(this.gaugeMax))
      num2 = (float) -(360.0 - (double) num2);
    else if ((double) Mathf.Round(num2) < (double) Mathf.Round(this.gaugeMin))
      num2 += 360f;
    ((UITweener) this.gaugeTweener).duration = 3f;
    this.gaugeTweener.from = new Vector3(0.0f, 0.0f, num2);
    this.gaugeTweener.to = new Vector3(0.0f, 0.0f, num3);
    NGTween.playTween((UITweener) this.gaugeTweener);
    this.gaugeValue = num1;
  }
}
