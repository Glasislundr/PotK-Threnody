// Decompiled with JetBrains decompiler
// Type: UnitHpGauge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class UnitHpGauge : MonoBehaviour
{
  [SerializeField]
  private NGTweenGaugeScale hpGauge;
  [SerializeField]
  private GameObject slc_dropout;

  public NGTweenGaugeScale TweenHpGauge => this.hpGauge;

  public bool Dropout
  {
    set => this.slc_dropout.SetActive(value);
    get => this.slc_dropout.activeSelf;
  }

  public void SetGaugeAndDropoutIcon(int n, int max, bool doTween = true)
  {
    this.SetGauge(n, max, doTween);
    this.Dropout = (double) n <= 0.0;
  }

  public void SetGauge(int n, int max, bool doTween = true)
  {
    this.hpGauge.setValue(n, max, doTween);
  }
}
