// Decompiled with JetBrains decompiler
// Type: Battle02UIPlayerStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using UnityEngine;

#nullable disable
public class Battle02UIPlayerStatus : Battle02MenuBase
{
  [SerializeField]
  protected UI2DSprite link_Character;
  [SerializeField]
  protected UILabel txt_CharaName_18;
  [SerializeField]
  protected UILabel txt_Job_22;
  [SerializeField]
  protected UILabel txt_Lv_22;
  [SerializeField]
  protected UILabel txt_HP_22;
  [SerializeField]
  protected UILabel txt_HP_Max_22;
  [SerializeField]
  protected UILabel txt_Combat_22;

  public override void UpdateData()
  {
    if (this.modified == null || !this.modified.isChangedOnce())
      return;
    BL.Unit beUnit = this.modified.value;
    this.CreateUnitSprite(this.link_Character);
    UILabel txtCharaName18 = this.txt_CharaName_18;
    UnitUnit unit = beUnit.unit;
    SkillMetamorphosis metamorphosis = beUnit.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    string name = unit.getName(metamorphosisId);
    this.setText(txtCharaName18, name);
    this.setText(this.txt_Job_22, beUnit.job.name);
    this.setText(this.txt_Lv_22, beUnit.lv);
    this.setText(this.txt_HP_22, beUnit.hp);
    if (beUnit.hp <= beUnit.parameter.Hp / 10)
    {
      ((UIWidget) this.txt_HP_22).color = this.mRed;
      ((UITweener) TweenColor.Begin(((Component) this.txt_HP_22).gameObject, 1f, new Color(0.5f, 0.0f, 0.0f))).style = (UITweener.Style) 1;
    }
    if (beUnit.parameter.HpIncr > 0)
      this.setText(this.txt_HP_Max_22, "[00dc1e]" + (object) beUnit.parameter.Hp + "[-]");
    else if (beUnit.parameter.HpIncr < 0)
      this.setText(this.txt_HP_Max_22, "[fa0000]" + (object) beUnit.parameter.Hp + "[-]");
    else
      this.setText(this.txt_HP_Max_22, beUnit.parameter.Hp);
    Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) beUnit);
    this.setColordText(this.txt_Combat_22, battleParameter.Combat, battleParameter.CombatIncr);
  }

  public override void onBackButton()
  {
  }
}
