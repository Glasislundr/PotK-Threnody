// Decompiled with JetBrains decompiler
// Type: BattleHealCharacterInfoInjured
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleHealCharacterInfoInjured : BattleHealCharacterInfoBase
{
  [SerializeField]
  protected UILabel hpNumberAfter;
  [SerializeField]
  protected UILabel hpNumberBefore;
  [SerializeField]
  private UI2DSprite[] buffSprites;

  public override IEnumerator Init(BL.UnitPosition up, AttackStatus[] attacks)
  {
    if (up == null)
    {
      Debug.LogWarning((object) "unit is null");
    }
    else
    {
      IEnumerator e = base.Init(up, attacks);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.hpNumberBefore.SetTextLocalize(up.unit.parameter.Hp);
    }
  }

  public void setCurrentHP(int healHP)
  {
    BL.Unit unit = this.currentUnit.unit;
    int n = Mathf.Min(healHP + unit.hp, unit.parameter.Hp);
    this.setHPNumbers(n.ToString());
    this.hpBar.setValue(unit.hp, unit.parameter.Hp, false);
    this.consumeBar.setValue(n, unit.parameter.Hp, false);
  }

  private new void setHPNumbers(string hp) => this.hpNumberAfter.SetTextLocalize(hp);

  protected override ResourceObject maskResource() => Res.GUI._009_3_sozai.mask_Chara_R;
}
