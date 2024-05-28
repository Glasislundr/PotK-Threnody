// Decompiled with JetBrains decompiler
// Type: Shop00720EffectGetItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Shop00720EffectGetItem : MonoBehaviour
{
  [SerializeField]
  private Shop00720EffectRarity[] rarities_;

  public void getItemSoundPlay(int n) => this.rarities_[n - 1].getItemSoundPlay(n);
}
