// Decompiled with JetBrains decompiler
// Type: Unit0042FloatingSpecialPointDialog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_2/Unit0042FloatingSpecialPointDialog")]
public class Unit0042FloatingSpecialPointDialog : Unit0042FloatingDialogBase
{
  [SerializeField]
  protected UILabel txt_Factor;
  [SerializeField]
  protected UILabel txt_EventName;
  [SerializeField]
  protected UISprite specialIcon;
  private static readonly string specialIconSpriteBaseName = "slc_icon_specific_effectiveness_{0}.png__GUI__unit_detail{1}__unit_detail{1}_prefab";

  public new void Show()
  {
    if (this.DialogConteiner.activeInHierarchy && this.isShow)
      return;
    base.Show();
    this.dir_FamilyType.SetActive(false);
    this.dir_SpecialPoint.SetActive(true);
  }

  public void setData(string strFactor, string strEventName, string iconSpriteName)
  {
    this.txt_Factor.SetTextLocalize(strFactor);
    this.txt_EventName.SetTextLocalize(strEventName);
    this.specialIcon.spriteName = iconSpriteName;
  }
}
