// Decompiled with JetBrains decompiler
// Type: SwitchUnitComponentImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SwitchUnitComponentImage : SwitchUnitComponentBase
{
  [SerializeField]
  private Sprite[] bottomBaseSprites;
  private UI2DSprite image;

  protected override void Init()
  {
    if (Object.op_Implicit((Object) this.image) || !Object.op_Inequality((Object) ((Component) this).gameObject.GetComponent<UI2DSprite>(), (Object) null))
      return;
    this.materialType = SwitchUnitComponentBase.MATERIALTYPE.Image;
    this.image = ((Component) this).gameObject.GetComponent<UI2DSprite>();
  }

  public override void SwitchMaterial(int UnitID)
  {
    base.SwitchMaterial(UnitID);
    if (this.currUnitType == SwitchUnitComponentBase.UnitType.DefaultUnit)
    {
      if (!Object.op_Implicit((Object) this.bottomBaseSprites[0]))
        return;
      this.image.sprite2D = this.bottomBaseSprites[0];
    }
    else
    {
      if (!Object.op_Implicit((Object) this.bottomBaseSprites[1]))
        return;
      this.image.sprite2D = this.bottomBaseSprites[1];
    }
  }
}
