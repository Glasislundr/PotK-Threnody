// Decompiled with JetBrains decompiler
// Type: DownloadGauge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DownloadGauge : MonoBehaviour
{
  [SerializeField]
  private UILabel percentage;
  [SerializeField]
  private NGTweenGaugeScale gauge;

  public void setValue(int n, int max, bool doTween = true)
  {
    if (!this.gauge.setValue(n, max, doTween))
      return;
    this.percentage.SetTextLocalize(Mathf.RoundToInt((float) ((double) n / (double) max * 100.0)).ToString() + "%");
  }
}
