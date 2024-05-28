// Decompiled with JetBrains decompiler
// Type: Explore033StatusOwn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;

#nullable disable
public class Explore033StatusOwn : Battle0181CharacterStatus
{
  public override IEnumerator Init(
    BL.UnitPosition up,
    AttackStatus attackStatus,
    int firstAttack,
    bool isColosseum,
    bool isDemoMode)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Explore033StatusOwn explore033StatusOwn = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    explore033StatusOwn.current = up;
    explore033StatusOwn.maxHp = explore033StatusOwn.current.unit.initialHp;
    explore033StatusOwn.currentHp = explore033StatusOwn.current.unit.exploreHp;
    explore033StatusOwn.hpGauge.setValue(explore033StatusOwn.currentHp, explore033StatusOwn.maxHp, false);
    explore033StatusOwn.setHPNumbers(explore033StatusOwn.currentHp.ToString());
    explore033StatusOwn.txt_consumeHp.SetTextLocalize("");
    explore033StatusOwn.txt_characterName_ElementOn.SetTextLocalize("探索チーム");
    explore033StatusOwn.isHpDamaged = false;
    return false;
  }

  public override void Healed(int heal)
  {
  }
}
