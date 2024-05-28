// Decompiled with JetBrains decompiler
// Type: BattlePrepareCharacterInfoEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattlePrepareCharacterInfoEnemy : BattlePrepareCharacterInfoBase
{
  [SerializeField]
  private GameObject commandGameObject;
  [SerializeField]
  private BattleUI04CommandPrefab commandComponent;

  public override IEnumerator Init(
    BL.UnitPosition up,
    AttackStatus[] attacks,
    bool isFacility,
    BattleUI04Menu baseMenu)
  {
    BattlePrepareCharacterInfoEnemy characterInfoEnemy = this;
    if (up == null)
    {
      Debug.LogWarning((object) "unit is null");
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      IEnumerator e = characterInfoEnemy.\u003C\u003En__0(up, attacks, isFacility, baseMenu);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      AttackStatus attack = BattleFuncs.selectCounterAttackStatus(attacks);
      if (attack != null)
      {
        characterInfoEnemy.commandGameObject.SetActive(true);
        e = characterInfoEnemy.commandComponent.InitOppoSide(attack, up.unit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
        characterInfoEnemy.commandGameObject.SetActive(false);
      characterInfoEnemy.setCurrentAttack(attack);
    }
  }

  protected override Tuple<BL.Skill, float, bool> SimulateDuelSkill(
    BL.UnitPosition opponent,
    AttackStatus opponentAS)
  {
    return BattleFuncs.SimulateDuelSkill(opponent, opponentAS, this.getCurrent(), this.getCurrentAttack(), false);
  }

  protected override ResourceObject maskResource() => Res.GUI._009_3_sozai.mask_Chara_R;
}
