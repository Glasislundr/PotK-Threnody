// Decompiled with JetBrains decompiler
// Type: EffectNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectNumber : MonoBehaviour
{
  [SerializeField]
  private GameObject root_1_1;
  [SerializeField]
  private GameObject root_2_1;
  [SerializeField]
  private GameObject root_2_2;
  [SerializeField]
  private GameObject root_3_1;
  [SerializeField]
  private GameObject root_3_2;
  [SerializeField]
  private GameObject root_3_3;
  private SpriteNumberSelect[] digit_1_1;
  private SpriteNumberSelect[] digit_2_1;
  private SpriteNumberSelect[] digit_2_2;
  private SpriteNumberSelect[] digit_3_1;
  private SpriteNumberSelect[] digit_3_2;
  private SpriteNumberSelect[] digit_3_3;

  private void Awake() => this.initialize();

  private void initialize()
  {
    if (this.digit_1_1 != null)
      return;
    this.digit_1_1 = EffectNumber.getSpriteNumberSelects(this.root_1_1);
    this.digit_2_1 = EffectNumber.getSpriteNumberSelects(this.root_2_1);
    this.digit_2_2 = EffectNumber.getSpriteNumberSelects(this.root_2_2);
    this.digit_3_1 = EffectNumber.getSpriteNumberSelects(this.root_3_1);
    this.digit_3_2 = EffectNumber.getSpriteNumberSelects(this.root_3_2);
    this.digit_3_3 = EffectNumber.getSpriteNumberSelects(this.root_3_2);
  }

  public static SpriteNumberSelect[] getSpriteNumberSelects(GameObject o)
  {
    return o.GetComponentsInChildren<SpriteNumberSelect>(true);
  }

  public void setNumber(int n)
  {
    this.initialize();
    if (n < 10)
    {
      EffectNumber.setNumberSprite(this.digit_1_1, n);
      this.root_1_1.SetActive(true);
      this.root_2_1.SetActive(false);
      this.root_2_2.SetActive(false);
      this.root_3_1.SetActive(false);
      this.root_3_2.SetActive(false);
      this.root_3_3.SetActive(false);
    }
    else if (n < 100)
    {
      EffectNumber.setNumberSprite(this.digit_2_1, n % 10);
      EffectNumber.setNumberSprite(this.digit_2_2, n / 10);
      this.root_1_1.SetActive(false);
      this.root_2_1.SetActive(true);
      this.root_2_2.SetActive(true);
      this.root_3_1.SetActive(false);
      this.root_3_2.SetActive(false);
      this.root_3_3.SetActive(false);
    }
    else
    {
      EffectNumber.setNumberSprite(this.digit_3_1, n % 10);
      EffectNumber.setNumberSprite(this.digit_3_2, n % 100 / 10);
      EffectNumber.setNumberSprite(this.digit_3_3, n / 100);
      this.root_1_1.SetActive(false);
      this.root_2_1.SetActive(false);
      this.root_2_2.SetActive(false);
      this.root_3_1.SetActive(true);
      this.root_3_2.SetActive(true);
      this.root_3_3.SetActive(true);
    }
  }

  public static void setNumberSprite(SpriteNumberSelect[] s, int n)
  {
    foreach (SpriteNumberSelect spriteNumberSelect in s)
      spriteNumberSelect.setNumber(n);
  }
}
