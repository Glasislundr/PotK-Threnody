// Decompiled with JetBrains decompiler
// Type: SwitchUnitComponentSprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SwitchUnitComponentSprite : SwitchUnitComponentBase
{
  [Header("通常ユニットSpriteパス")]
  public string defaultUnitSpritePath;
  [Header("魔法少女まどかマギカSpriteパス")]
  public string madomagiUnitSpritePath;
  private UISprite sprite;

  protected override void Init()
  {
    if (Object.op_Implicit((Object) this.sprite) || !Object.op_Inequality((Object) ((Component) this).gameObject.GetComponent<UISprite>(), (Object) null))
      return;
    this.materialType = SwitchUnitComponentBase.MATERIALTYPE.Sprite;
    this.sprite = ((Component) this).gameObject.GetComponent<UISprite>();
  }

  public override void SwitchMaterial(int UnitID)
  {
    base.SwitchMaterial(UnitID);
    if (this.currUnitType == SwitchUnitComponentBase.UnitType.DefaultUnit)
    {
      if (this.defaultUnitSpritePath.Equals(""))
        return;
      this.sprite.spriteName = this.defaultUnitSpritePath;
    }
    else
    {
      if (this.madomagiUnitSpritePath.Equals(""))
        return;
      this.sprite.spriteName = this.madomagiUnitSpritePath;
    }
  }
}
