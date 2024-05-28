// Decompiled with JetBrains decompiler
// Type: SprGearAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SprGearAttack : MonoBehaviour
{
  public UI2DSprite iconSprite;
  private const int KIND_NONE = 1;
  [SerializeField]
  private Sprite[] icons;

  public void InitGearAttackID(int index) => this.iconSprite.sprite2D = this.icons[index];

  public void InitGearAttackNone() => this.InitGearAttackID(1);
}
