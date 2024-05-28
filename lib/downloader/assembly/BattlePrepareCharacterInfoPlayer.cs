// Decompiled with JetBrains decompiler
// Type: BattlePrepareCharacterInfoPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattlePrepareCharacterInfoPlayer : BattlePrepareCharacterInfoBase
{
  [SerializeField]
  private GameObject selectCommand;
  [SerializeField]
  private NGHorizontalScrollParts indicator;
  [SerializeField]
  private GameObject mDotContainer;
  private GameObject commandPrefab;

  public override IEnumerator LoadPrefab(bool isSea)
  {
    IEnumerator e = base.LoadPrefab(isSea);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> ft = isSea ? Res.Prefabs.battleUI_04_sea.command_sea.Load<GameObject>() : Res.Prefabs.battleUI_04.command.Load<GameObject>();
    e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.commandPrefab = ft.Result;
  }

  public override IEnumerator Init(
    BL.UnitPosition up,
    AttackStatus[] attackStatus,
    bool isFacility,
    BattleUI04Menu baseMenu)
  {
    BattlePrepareCharacterInfoPlayer characterInfoPlayer = this;
    characterInfoPlayer.selectCommand.SetActive(false);
    characterInfoPlayer.indicator.destroyParts();
    characterInfoPlayer.indicator.resetScrollView();
    if (up == null)
    {
      Debug.LogWarning((object) "unit is null");
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      IEnumerator e = characterInfoPlayer.\u003C\u003En__1(up, attackStatus, isFacility, baseMenu);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      characterInfoPlayer.selectCommand.SetActive(true);
      AttackStatus[] attackStatusArray = characterInfoPlayer.attacks;
      for (int index = 0; index < attackStatusArray.Length; ++index)
      {
        AttackStatus attack = attackStatusArray[index];
        e = characterInfoPlayer.indicator.instantiateParts(characterInfoPlayer.commandPrefab).GetComponent<BattleUI04CommandPrefab>().Init(attack, up.unit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      attackStatusArray = (AttackStatus[]) null;
      if (Object.op_Inequality((Object) characterInfoPlayer.mDotContainer, (Object) null))
      {
        if (((IEnumerable<AttackStatus>) characterInfoPlayer.attacks).Count<AttackStatus>((Func<AttackStatus, bool>) (x => !x.isHeal)) > 1)
        {
          Vector3 localPosition = characterInfoPlayer.mDotContainer.transform.localPosition;
          localPosition.y = -32f;
          characterInfoPlayer.mDotContainer.transform.localPosition = localPosition;
          characterInfoPlayer.mDotContainer.SetActive(true);
        }
        else
          characterInfoPlayer.mDotContainer.SetActive(false);
      }
      characterInfoPlayer.setCurrentAttack(((IEnumerable<AttackStatus>) attackStatus).Any<AttackStatus>() ? attackStatus[0] : (AttackStatus) null);
      characterInfoPlayer.indicator.resetScrollView();
      characterInfoPlayer.setIndicatorVisible();
    }
  }

  private void setIndicatorVisible()
  {
    ((UITweener) ((Component) this.indicator).gameObject.GetComponent<TweenAlpha>()).PlayForward();
  }

  protected override Tuple<BL.Skill, float, bool> SimulateDuelSkill(
    BL.UnitPosition opponent,
    AttackStatus opponentAS)
  {
    return BattleFuncs.SimulateDuelSkill(this.getCurrent(), this.getCurrentAttack(), opponent, opponentAS, true);
  }

  protected override ResourceObject maskResource() => Res.GUI._009_3_sozai.mask_Chara_L;
}
