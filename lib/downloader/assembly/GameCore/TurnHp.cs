// Decompiled with JetBrains decompiler
// Type: GameCore.TurnHp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace GameCore
{
  public class TurnHp
  {
    public int attackerHp;
    public int defenderHp;
    public bool attackerIsDontAction;
    public bool defenderIsDontAction;
    public bool attackerIsDontEvasion;
    public bool defenderIsDontEvasion;
    public bool attackerIsDontUseSkill;
    public bool defenderIsDontUseSkill;
    public bool attackerCantOneMore;
    public bool defenderCantOneMore;
    public Dictionary<BL.ISkillEffectListUnit, TurnOtherHp> otherHp;

    public bool isDieAttackerOrDefender() => this.attackerHp <= 0 || this.defenderHp <= 0;
  }
}
