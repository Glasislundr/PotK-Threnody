// Decompiled with JetBrains decompiler
// Type: GearKindButtonIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class GearKindButtonIcon : MonoBehaviour
{
  public void Init(GearKindEnum kind)
  {
    ((Component) this).GetComponent<UI2DSprite>().sprite2D = this.GetIdle(kind);
  }

  public Sprite GetIdle(GearKind kind) => this.GetIdle(kind.Enum);

  public Sprite GetPress(GearKind kind) => this.GetPress(kind.Enum);

  public Sprite GetIdle(GearKindEnum kind)
  {
    return Resources.Load<Sprite>(string.Format("Icons/Materials/GuideWeaponBtn/slc_{0}_idle", (object) kind.ToString()));
  }

  public Sprite GetPress(GearKindEnum kind)
  {
    return Resources.Load<Sprite>(string.Format("Icons/Materials/GuideWeaponBtn/slc_{0}_pressed", (object) kind.ToString()));
  }
}
