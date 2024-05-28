// Decompiled with JetBrains decompiler
// Type: SPAtkTypeIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class SPAtkTypeIcon : MonoBehaviour
{
  public UI2DSprite iconSprite;
  private const int KIND_NONE = 0;
  [SerializeField]
  private Sprite[] icons;

  private void InitKindId(int index) => this.iconSprite.sprite2D = this.icons[index];

  public void InitKindId(UnitFamily family) => this.InitKindId((int) family);

  public void InitKindNone() => this.InitKindId(0);
}
