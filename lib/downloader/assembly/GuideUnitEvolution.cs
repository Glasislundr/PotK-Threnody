// Decompiled with JetBrains decompiler
// Type: GuideUnitEvolution
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using UnityEngine;

#nullable disable
public class GuideUnitEvolution : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite rarityStars;
  [SerializeField]
  private GameObject selectObj;
  [SerializeField]
  private UIButton button;
  private bool isEnable = true;
  private PlayerUnit playerUnit;
  private Guide01122Menu menu;
  private GuideRaritySelectDialog dialog;

  public void pressButton() => this.dialog.onEvolutionButton(this.playerUnit);

  public void Set(
    PlayerUnit playerUnit,
    Guide01122Menu menu01122,
    GuideRaritySelectDialog dialog,
    bool select,
    bool isEnable,
    bool awakeFlg)
  {
    this.menu = menu01122;
    this.dialog = dialog;
    this.isEnable = isEnable;
    this.playerUnit = playerUnit;
    RarityIcon.SetRarity(this.playerUnit, this.rarityStars, false, true, awakeFlg);
    this.SetSelect(select);
  }

  public void SetSelect(bool flag)
  {
    this.selectObj.SetActive(flag && this.isEnable);
    if (!Object.op_Implicit((Object) this.button))
      return;
    ((UIButtonColor) this.button).isEnabled = this.isEnable;
    ((Behaviour) this.button).enabled = !flag;
  }
}
