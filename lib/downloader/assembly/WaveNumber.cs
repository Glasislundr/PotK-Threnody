// Decompiled with JetBrains decompiler
// Type: WaveNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WaveNumber : MonoBehaviour
{
  [SerializeField]
  private GameObject dir_1;
  [SerializeField]
  private GameObject num_root_1_1;
  [SerializeField]
  private GameObject denom_root_1_1;
  [SerializeField]
  private GameObject dir_2;
  [SerializeField]
  private GameObject num_root_2_1;
  [SerializeField]
  private GameObject denom_root_2_1;
  [SerializeField]
  private GameObject num_root_2_2;
  [SerializeField]
  private GameObject denom_root_2_2;
  [SerializeField]
  private GameObject dir_3;
  [SerializeField]
  private GameObject num_root_3_1;
  [SerializeField]
  private GameObject denom_root_3_1;
  [SerializeField]
  private GameObject num_root_3_2;
  [SerializeField]
  private GameObject denom_root_3_2;
  [SerializeField]
  private GameObject num_root_3_3;
  [SerializeField]
  private GameObject denom_root_3_3;
  private SpriteNumberSelect[] num_1_1;
  private SpriteNumberSelect[] denom_1_1;
  private SpriteNumberSelect[] num_2_1;
  private SpriteNumberSelect[] denom_2_1;
  private SpriteNumberSelect[] num_2_2;
  private SpriteNumberSelect[] denom_2_2;
  private SpriteNumberSelect[] num_3_1;
  private SpriteNumberSelect[] denom_3_1;
  private SpriteNumberSelect[] num_3_2;
  private SpriteNumberSelect[] denom_3_2;
  private SpriteNumberSelect[] num_3_3;
  private SpriteNumberSelect[] denom_3_3;

  private void Awake() => this.initialize();

  private void initialize()
  {
    if (this.num_1_1 != null)
      return;
    this.num_1_1 = EffectNumber.getSpriteNumberSelects(this.num_root_1_1);
    this.denom_1_1 = EffectNumber.getSpriteNumberSelects(this.denom_root_1_1);
    this.num_2_1 = EffectNumber.getSpriteNumberSelects(this.num_root_2_1);
    this.denom_2_1 = EffectNumber.getSpriteNumberSelects(this.denom_root_2_1);
    this.num_2_2 = EffectNumber.getSpriteNumberSelects(this.num_root_2_2);
    this.denom_2_2 = EffectNumber.getSpriteNumberSelects(this.denom_root_2_2);
    this.num_3_1 = EffectNumber.getSpriteNumberSelects(this.num_root_3_1);
    this.denom_3_1 = EffectNumber.getSpriteNumberSelects(this.denom_root_3_1);
    this.num_3_2 = EffectNumber.getSpriteNumberSelects(this.num_root_3_2);
    this.denom_3_2 = EffectNumber.getSpriteNumberSelects(this.denom_root_3_2);
    this.num_3_3 = EffectNumber.getSpriteNumberSelects(this.num_root_3_3);
    this.denom_3_3 = EffectNumber.getSpriteNumberSelects(this.denom_root_3_3);
  }

  public void setNumber(int wave, int waveMax)
  {
    this.initialize();
    if (waveMax < 10)
    {
      EffectNumber.setNumberSprite(this.num_1_1, wave);
      EffectNumber.setNumberSprite(this.denom_1_1, waveMax);
      this.dir_1.SetActive(true);
      this.dir_2.SetActive(false);
      this.dir_3.SetActive(false);
    }
    else if (waveMax < 100)
    {
      EffectNumber.setNumberSprite(this.num_2_1, wave % 10);
      EffectNumber.setNumberSprite(this.num_2_2, wave / 10);
      EffectNumber.setNumberSprite(this.denom_2_1, waveMax % 10);
      EffectNumber.setNumberSprite(this.denom_2_2, waveMax / 10);
      this.dir_1.SetActive(false);
      this.dir_2.SetActive(true);
      this.dir_3.SetActive(false);
    }
    else
    {
      EffectNumber.setNumberSprite(this.num_3_1, wave % 10);
      EffectNumber.setNumberSprite(this.num_3_2, wave % 100 / 10);
      EffectNumber.setNumberSprite(this.num_3_3, wave / 100);
      EffectNumber.setNumberSprite(this.denom_3_1, waveMax % 10);
      EffectNumber.setNumberSprite(this.denom_3_2, waveMax % 100 / 10);
      EffectNumber.setNumberSprite(this.denom_3_3, waveMax / 100);
      this.dir_1.SetActive(false);
      this.dir_2.SetActive(false);
      this.dir_3.SetActive(true);
    }
  }
}
