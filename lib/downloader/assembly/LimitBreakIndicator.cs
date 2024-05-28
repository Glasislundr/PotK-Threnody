// Decompiled with JetBrains decompiler
// Type: LimitBreakIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Unit/LimitBreakIndicator")]
public class LimitBreakIndicator : MonoBehaviour
{
  [SerializeField]
  private GameObject[] scale_;
  [SerializeField]
  private GameObject[] lamp_;
  [SerializeField]
  private GameObject[] blank_;

  public void set(int num, int max)
  {
    max = Mathf.Min(max, this.scale_.Length);
    num = Mathf.Min(num, max);
    int index;
    for (index = 0; index < max; ++index)
    {
      bool flag = index < num;
      this.lamp_[index].SetActive(flag);
      this.blank_[index].SetActive(!flag);
      this.scale_[index].SetActive(true);
    }
    for (; index < this.scale_.Length; ++index)
      this.scale_[index].SetActive(false);
  }
}
