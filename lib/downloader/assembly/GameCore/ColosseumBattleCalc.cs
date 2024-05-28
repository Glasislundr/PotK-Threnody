// Decompiled with JetBrains decompiler
// Type: GameCore.ColosseumBattleCalc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace GameCore
{
  public static class ColosseumBattleCalc
  {
    private const int BattingFirstPlayer = 0;
    private const int BattingFirstOpponent = 1;
    private const int ColosseumMaxDuelTurn = 4;
    private const int CriticalDamageRate = 3;
    private const float COMMANDSKILL_PROBABILITY_BASE = 30f;
    private const float COMMANDSKILL_PROBABILITY_ADJUST = 40f;
    private const float COMMANDSKILL_PROBABILITY_MIN = 5f;
    private const float COMMANDSKILL_PROBABILITY_MAX = 100f;
    private const float RELEASESKILL_PROBABILITY_BASE = 25f;
    private const float RELEASESKILL_PROBABILITY_ADJUST = 0.09f;
    private const float RELEASESKILL_PROBABILITY_MIN = 3f;
    private const float RELEASESKILL_PROBABILITY_MAX = 100f;
    private static GameGlobalVariable<XorShift> random = GameGlobalVariable<XorShift>.Null();
    private static GameGlobalVariable<ColosseumEnvironment> _env = GameGlobalVariable<ColosseumEnvironment>.Null();

    public static AttackStatus selectAttackStatus(
      BL.Unit attack,
      BL.Unit[] attackNeighbors,
      BL.Unit[] attackDeckUnits,
      PlayerItem attackEquippedGear,
      PlayerItem attackEquippedGear2,
      PlayerItem attackEquippedGear3,
      PlayerItem attackEquippedReisou,
      PlayerItem attackEquippedReisou2,
      PlayerItem attackEquippedReisou3,
      BL.Unit defense,
      BL.Unit[] defenseNeighbors,
      BL.Unit[] defenseDeckUnits,
      PlayerItem defenseEquippedGear,
      PlayerItem defenseEquippedGear2,
      PlayerItem defenseEquippedGear3,
      PlayerItem defenseEquippedReisou,
      PlayerItem defenseEquippedReisou2,
      PlayerItem defenseEquippedReisou3,
      bool isAttack,
      int attackHp,
      bool isSample,
      int defenseHp,
      int battleCount,
      int colosseumTurn)
    {
      AttackStatus[] attackStatusArray = ColosseumBattleCalc.getAttackStatusArray(attack, attackNeighbors, attackDeckUnits, attackEquippedGear, attackEquippedGear2, attackEquippedGear3, attackEquippedReisou, attackEquippedReisou2, attackEquippedReisou3, defense, defenseNeighbors, defenseDeckUnits, defenseEquippedGear, defenseEquippedGear2, defenseEquippedGear3, defenseEquippedReisou, defenseEquippedReisou2, defenseEquippedReisou3, isAttack, attackHp, isSample, defenseHp, battleCount, colosseumTurn);
      return ((IEnumerable<AttackStatus>) attackStatusArray).Count<AttackStatus>() == 0 ? (AttackStatus) null : ((IEnumerable<AttackStatus>) attackStatusArray).OrderByDescending<AttackStatus, int>((Func<AttackStatus, int>) (x =>
      {
        float damageRate = x.duelParameter.DamageRate;
        x.duelParameter.DamageRate *= x.elementAttackRate * x.attackClassificationRate * x.normalDamageRate;
        int num = Mathf.Max(Mathf.FloorToInt(x.originalAttack), 1) * x.normalAttackCount;
        x.duelParameter.DamageRate = damageRate;
        return num;
      })).ThenBy<AttackStatus, int>((Func<AttackStatus, int>) (x => x.magicBullet != null ? x.magicBullet.cost : 0)).FirstOrDefault<AttackStatus>();
    }

    public static AttackStatus[] getAttackStatusArray(
      BL.Unit attack,
      BL.Unit[] attackNeighbors,
      BL.Unit[] attackDeckUnits,
      PlayerItem attackEquippedGear,
      PlayerItem attackEquippedGear2,
      PlayerItem attackEquippedGear3,
      PlayerItem attackEquippedReisou,
      PlayerItem attackEquippedReisou2,
      PlayerItem attackEquippedReisou3,
      BL.Unit defense,
      BL.Unit[] defenseNeighbors,
      BL.Unit[] defenseDeckUnits,
      PlayerItem defenseEquippedGear,
      PlayerItem defenseEquippedGear2,
      PlayerItem defenseEquippedGear3,
      PlayerItem defenseEquippedReisou,
      PlayerItem defenseEquippedReisou2,
      PlayerItem defenseEquippedReisou3,
      bool isAttack,
      int attackHp,
      bool isSample,
      int defenseHp,
      int battleCount,
      int colosseumTurn)
    {
      bool flag = false;
      AttackStatus[] second = new AttackStatus[0];
      AttackStatus[] first1 = new AttackStatus[0];
      AttackStatus[] first2 = new AttackStatus[attack.optionWeapons.Length];
      if (((IEnumerable<BL.MagicBullet>) attack.magicBullets).Any<BL.MagicBullet>())
      {
        flag = true;
        second = ((IEnumerable<BL.MagicBullet>) attack.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x => x != null && x.isAttack && attackHp > x.cost)).Select<BL.MagicBullet, AttackStatus>((Func<BL.MagicBullet, AttackStatus>) (x => ColosseumBattleCalc.getAttackStatus(x, (BL.Weapon) null, attack, attackNeighbors, attackDeckUnits, attackEquippedGear, attackEquippedGear2, attackEquippedGear3, attackEquippedReisou, attackEquippedReisou2, attackEquippedReisou3, defense, defenseNeighbors, defenseDeckUnits, defenseEquippedGear, defenseEquippedGear2, defenseEquippedGear3, defenseEquippedReisou, defenseEquippedReisou2, defenseEquippedReisou3, isAttack, attackHp, isSample, defenseHp, battleCount, colosseumTurn))).ToArray<AttackStatus>();
      }
      if (!flag || attack.unit.magic_warrior_flag)
        first1 = new AttackStatus[1]
        {
          ColosseumBattleCalc.getAttackStatus((BL.MagicBullet) null, (BL.Weapon) null, attack, attackNeighbors, attackDeckUnits, attackEquippedGear, attackEquippedGear2, attackEquippedGear3, attackEquippedReisou, attackEquippedReisou2, attackEquippedReisou3, defense, defenseNeighbors, defenseDeckUnits, defenseEquippedGear, defenseEquippedGear2, defenseEquippedGear3, defenseEquippedReisou, defenseEquippedReisou2, defenseEquippedReisou3, isAttack, attackHp, isSample, defenseHp, battleCount, colosseumTurn)
        };
      for (int index = 0; index < attack.optionWeapons.Length; ++index)
        first2[index] = ColosseumBattleCalc.getAttackStatus((BL.MagicBullet) null, attack.optionWeapons[index], attack, attackNeighbors, attackDeckUnits, attackEquippedGear, attackEquippedGear2, attackEquippedGear3, attackEquippedReisou, attackEquippedReisou2, attackEquippedReisou3, defense, defenseNeighbors, defenseDeckUnits, defenseEquippedGear, defenseEquippedGear2, defenseEquippedGear3, defenseEquippedReisou, defenseEquippedReisou2, defenseEquippedReisou3, isAttack, attackHp, isSample, defenseHp, battleCount, colosseumTurn);
      return ((IEnumerable<AttackStatus>) first1).Concat<AttackStatus>(((IEnumerable<AttackStatus>) first2).Concat<AttackStatus>((IEnumerable<AttackStatus>) second)).ToArray<AttackStatus>();
    }

    private static AttackStatus getAttackStatus(
      BL.MagicBullet magicBullet,
      BL.Weapon weapon,
      BL.Unit attack,
      BL.Unit[] attackNeighbors,
      BL.Unit[] attackDeckUnits,
      PlayerItem attackEquippedGear,
      PlayerItem attackEquippedGear2,
      PlayerItem attackEquippedGear3,
      PlayerItem attackEquippedReisou,
      PlayerItem attackEquippedReisou2,
      PlayerItem attackEquippedReisou3,
      BL.Unit defense,
      BL.Unit[] defenseNeighbors,
      BL.Unit[] defenseDeckUnits,
      PlayerItem defenseEquippedGear,
      PlayerItem defenseEquippedGear2,
      PlayerItem defenseEquippedGear3,
      PlayerItem defenseEquippedReisou,
      PlayerItem defenseEquippedReisou2,
      PlayerItem defenseEquippedReisou3,
      bool isAttack,
      int attackHp,
      bool isSample,
      int defenseHp,
      int battleCount,
      int colosseumTurn)
    {
      bool flag;
      if (weapon != null)
        flag = false;
      else if (attack.unit.magic_warrior_flag)
      {
        flag = magicBullet != null;
      }
      else
      {
        GearGear gear = attack.weapon.gear;
        flag = (gear.attack_type == GearAttackType.none ? (int) attack.playerUnit.initial_gear.attack_type : (int) gear.attack_type) == 6;
      }
      Judgement.BeforeDuelParameter colosseumSingle = Judgement.BeforeDuelParameter.CreateColosseumSingle(attack, magicBullet, attackNeighbors, attackDeckUnits, attackEquippedGear, attackEquippedGear2, attackEquippedGear3, attackEquippedReisou, attackEquippedReisou2, attackEquippedReisou3, defense, (BL.MagicBullet) null, defenseNeighbors, defenseDeckUnits, defenseEquippedGear, defenseEquippedGear2, defenseEquippedGear3, defenseEquippedReisou, defenseEquippedReisou2, defenseEquippedReisou3, isAttack, isSample, attackHp, defenseHp, battleCount, new bool?(flag), weapon, colosseumTurn);
      float num = 1f;
      float attackDamageRate = attack.originalUnit.playerUnit.normalAttackDamageRate;
      AttackStatus attackStatus = new AttackStatus();
      attackStatus.duelParameter = colosseumSingle;
      attackStatus.isMagic = flag;
      attackStatus.magicBullet = magicBullet;
      attackStatus.weapon = weapon;
      attackStatus.attackRate = num;
      attackStatus.normalDamageRate = attackDamageRate;
      attackStatus.normalAttackCount = attack.originalUnit.playerUnit.normalAttackCount;
      attackStatus.calcElementAttackRate((BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense);
      attackStatus.calcAttackClassificationRate((BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense);
      return attackStatus;
    }

    public static DuelColosseumResult calcColosseumDuel(
      BL.Unit playerUnit,
      int playerUnitHp,
      PlayerItem playerEquippedGear,
      PlayerItem playerEquippedGear2,
      PlayerItem playerEquippedGear3,
      PlayerItem playerEquippedReisou,
      PlayerItem playerEquippedReisou2,
      PlayerItem playerEquippedReisou3,
      BL.Unit opponentUnit,
      int opponentUnitHp,
      PlayerItem opponentEquippedGear,
      PlayerItem opponentEquippedGear2,
      PlayerItem opponentEquippedGear3,
      PlayerItem opponentEquippedReisou,
      PlayerItem opponentEquippedReisou2,
      PlayerItem opponentEquippedReisou3,
      Bonus[] bonusList,
      int battleCount)
    {
      return ColosseumBattleCalc.calcColosseumDuelAttack(playerUnit, playerUnitHp, playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, opponentUnit, opponentUnitHp, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3, bonusList, battleCount);
    }

    public static DuelColosseumResult calcColosseumDuelAttack(
      BL.Unit playerUnit,
      int playerUnitHp,
      PlayerItem playerEquippedGear,
      PlayerItem playerEquippedGear2,
      PlayerItem playerEquippedGear3,
      PlayerItem playerEquippedReisou,
      PlayerItem playerEquippedReisou2,
      PlayerItem playerEquippedReisou3,
      BL.Unit opponentUnit,
      int opponentUnitHp,
      PlayerItem opponentEquippedGear,
      PlayerItem opponentEquippedGear2,
      PlayerItem opponentEquippedGear3,
      PlayerItem opponentEquippedReisou,
      PlayerItem opponentEquippedReisou2,
      PlayerItem opponentEquippedReisou3,
      Bonus[] bonusList,
      int battleCount)
    {
      DuelColosseumResult duelColosseumResult = new DuelColosseumResult();
      BL.Unit[] array1 = ColosseumBattleCalc._env.Get().playerUnitDict.Values.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x != (BL.Unit) null)).ToArray<BL.Unit>();
      BL.Unit[] array2 = ColosseumBattleCalc._env.Get().opponentUnitDict.Values.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x != (BL.Unit) null)).ToArray<BL.Unit>();
      AttackStatus status1 = ColosseumBattleCalc.selectAttackStatus(playerUnit, new BL.Unit[0], array1, playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, opponentUnit, new BL.Unit[0], array2, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3, true, playerUnitHp, true, opponentUnitHp, battleCount, 0);
      AttackStatus status2 = ColosseumBattleCalc.selectAttackStatus(opponentUnit, new BL.Unit[0], array2, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3, playerUnit, new BL.Unit[0], array1, playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, true, opponentUnitHp, true, playerUnitHp, battleCount, 0);
      duelColosseumResult.playerBeforeBonusParam = status1 != null ? new ColosseumBeforBonusParam(status1) : (ColosseumBeforBonusParam) null;
      duelColosseumResult.opponentBeforeBonusParam = status2 != null ? new ColosseumBeforBonusParam(status2) : (ColosseumBeforBonusParam) null;
      duelColosseumResult.colosseumPlayerFirstAttack = duelColosseumResult.playerBeforeBonusParam != null ? duelColosseumResult.playerBeforeBonusParam.attack : 0;
      duelColosseumResult.colosseumOpponentFirstAttack = duelColosseumResult.opponentBeforeBonusParam != null ? duelColosseumResult.opponentBeforeBonusParam.attack : 0;
      duelColosseumResult.playerActiveBonus = ColosseumBattleCalc.SetEnableColosseumBonusEffect(playerUnit, bonusList);
      duelColosseumResult.opponentActiveBonus = ColosseumBattleCalc.SetEnableColosseumBonusEffect(opponentUnit, bonusList);
      playerUnit.setParameter(Judgement.BattleParameter.FromBeColosseumUnit(playerUnit, playerEquippedGear, playerEquippedGear2, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3));
      playerUnit.hp = playerUnit.parameter.Hp;
      playerUnitHp = playerUnit.hp;
      foreach (BL.MagicBullet magicBullet in playerUnit.magicBullets)
        magicBullet.setAdditionalCost(playerUnit.hp);
      opponentUnit.setParameter(Judgement.BattleParameter.FromBeColosseumUnit(opponentUnit, opponentEquippedGear, opponentEquippedGear2, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3));
      opponentUnit.hp = opponentUnit.parameter.Hp;
      opponentUnitHp = opponentUnit.hp;
      foreach (BL.MagicBullet magicBullet in opponentUnit.magicBullets)
        magicBullet.setAdditionalCost(opponentUnit.hp);
      int attackOrder = ColosseumBattleCalc.getAttackOrder(playerUnit, playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, playerUnitHp, opponentUnit, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3, opponentUnitHp);
      duelColosseumResult.isPlayerFirstAttacker = attackOrder == 0;
      duelColosseumResult.player = playerUnit;
      duelColosseumResult.playerEq = playerEquippedGear;
      duelColosseumResult.playerEq2 = playerEquippedGear2;
      duelColosseumResult.playerEq3 = playerEquippedGear3;
      duelColosseumResult.playerReisou = playerEquippedReisou;
      duelColosseumResult.playerReisou2 = playerEquippedReisou2;
      duelColosseumResult.playerReisou3 = playerEquippedReisou3;
      duelColosseumResult.opponent = opponentUnit;
      duelColosseumResult.opponentEq = opponentEquippedGear;
      duelColosseumResult.opponentEq2 = opponentEquippedGear2;
      duelColosseumResult.opponentEq3 = opponentEquippedGear3;
      duelColosseumResult.opponentReisou = opponentEquippedReisou;
      duelColosseumResult.opponentReisou2 = opponentEquippedReisou2;
      duelColosseumResult.opponentReisou3 = opponentEquippedReisou3;
      duelColosseumResult.playerAttackStatus = ColosseumBattleCalc.selectAttackStatus(playerUnit, new BL.Unit[0], array1, playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, opponentUnit, new BL.Unit[0], array2, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3, true, playerUnitHp, true, opponentUnitHp, battleCount, 0);
      duelColosseumResult.opponentAttackStatus = ColosseumBattleCalc.selectAttackStatus(opponentUnit, new BL.Unit[0], array2, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3, playerUnit, new BL.Unit[0], array1, playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, true, opponentUnitHp, true, playerUnitHp, battleCount, 0);
      BL.Unit unit1;
      int num1;
      PlayerItem playerItem1;
      PlayerItem playerItem2;
      PlayerItem playerItem3;
      PlayerItem playerItem4;
      PlayerItem playerItem5;
      PlayerItem playerItem6;
      BL.Unit[] unitArray1;
      BL.Unit unit2;
      int num2;
      PlayerItem playerItem7;
      PlayerItem playerItem8;
      PlayerItem playerItem9;
      PlayerItem playerItem10;
      PlayerItem playerItem11;
      PlayerItem playerItem12;
      BL.Unit[] unitArray2;
      if (attackOrder == 0)
      {
        unit1 = playerUnit;
        num1 = playerUnitHp;
        playerItem1 = playerEquippedGear;
        playerItem2 = playerEquippedGear2;
        playerItem3 = playerEquippedGear3;
        playerItem4 = playerEquippedReisou;
        playerItem5 = playerEquippedReisou2;
        playerItem6 = playerEquippedReisou3;
        unitArray1 = array1;
        unit2 = opponentUnit;
        num2 = opponentUnitHp;
        playerItem7 = opponentEquippedGear;
        playerItem8 = opponentEquippedGear2;
        playerItem9 = opponentEquippedGear3;
        playerItem10 = opponentEquippedReisou;
        playerItem11 = opponentEquippedReisou2;
        playerItem12 = opponentEquippedReisou3;
        unitArray2 = array2;
      }
      else
      {
        unit2 = playerUnit;
        num2 = playerUnitHp;
        playerItem7 = playerEquippedGear;
        playerItem8 = playerEquippedGear2;
        playerItem9 = playerEquippedGear3;
        playerItem10 = playerEquippedReisou;
        playerItem11 = playerEquippedReisou2;
        playerItem12 = playerEquippedReisou3;
        unitArray2 = array1;
        unit1 = opponentUnit;
        num1 = opponentUnitHp;
        playerItem1 = opponentEquippedGear;
        playerItem2 = opponentEquippedGear2;
        playerItem3 = opponentEquippedGear3;
        playerItem4 = opponentEquippedReisou;
        playerItem5 = opponentEquippedReisou2;
        playerItem6 = opponentEquippedReisou3;
        unitArray1 = array2;
      }
      List<BL.DuelTurn> duelTurnList = new List<BL.DuelTurn>();
      ColosseumBattleCalc.CalcCommandAndReleaseSkill(playerUnit, opponentUnit, attackOrder == 0 ? 1 : 2);
      ColosseumBattleCalc.CalcCommandAndReleaseSkill(opponentUnit, playerUnit, attackOrder == 1 ? 1 : 2);
      duelColosseumResult.colosseumNewPAS = ColosseumBattleCalc.selectAttackStatus(playerUnit, new BL.Unit[0], array1, playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, opponentUnit, new BL.Unit[0], array2, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3, attackOrder == 0, playerUnitHp, false, opponentUnitHp, battleCount, 0);
      duelColosseumResult.colosseumNewOAS = ColosseumBattleCalc.selectAttackStatus(opponentUnit, new BL.Unit[0], array2, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou3, playerUnit, new BL.Unit[0], array1, playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, attackOrder != 0, opponentUnitHp, false, playerUnitHp, battleCount, 0);
      bool invokedAttackerPrayer = false;
      bool invokedDefenderPrayer = false;
      for (int index = 0; index < 4; ++index)
      {
        bool flag = false;
        unit1.skillEffects.ColosseumTurnStart(index == 0);
        unit2.skillEffects.ColosseumTurnStart(index == 0);
        int colosseumTurn = index + 1;
        AttackStatus attackStatus1 = ColosseumBattleCalc.selectAttackStatus(unit1, new BL.Unit[0], unitArray1, playerItem1, playerItem2, playerItem3, playerItem4, playerItem5, playerItem6, unit2, new BL.Unit[0], unitArray2, playerItem7, playerItem8, playerItem9, playerItem10, playerItem11, playerItem12, !flag, num1, false, num2, battleCount, colosseumTurn);
        AttackStatus attackStatus2 = ColosseumBattleCalc.selectAttackStatus(unit2, new BL.Unit[0], unitArray2, playerItem7, playerItem8, playerItem9, playerItem10, playerItem11, playerItem12, unit1, new BL.Unit[0], unitArray1, playerItem1, playerItem2, playerItem3, playerItem4, playerItem5, playerItem6, flag, num2, false, num1, battleCount, colosseumTurn);
        BL.SkillEffectList skillEffects1 = unit1.skillEffects;
        BL.SkillEffectList skillEffects2 = unit2.skillEffects;
        unit1.setSkillEffects(CopyUtil.DeepCopy<BL.SkillEffectList>(skillEffects1));
        unit2.setSkillEffects(CopyUtil.DeepCopy<BL.SkillEffectList>(skillEffects2));
        BL.DuelTurn[] duelTurnArray = ColosseumBattleCalc.calcTurns(unit1, num1, attackStatus1, unit2, num2, attackStatus2, flag, invokedAttackerPrayer, invokedDefenderPrayer, colosseumTurn);
        unit1.setSkillEffects(skillEffects1);
        unit2.setSkillEffects(skillEffects2);
        if (duelTurnArray.Length != 0)
        {
          if (!flag)
          {
            duelTurnArray[0].attackerStatus = attackStatus1;
            duelTurnArray[0].defenderStatus = attackStatus2;
          }
          else
          {
            duelTurnArray[0].defenderStatus = attackStatus1;
            duelTurnArray[0].attackerStatus = attackStatus2;
          }
        }
        int num3 = 0;
        int num4 = 0;
        foreach (BL.DuelTurn duelTurn in duelTurnArray)
        {
          if (duelTurn.isAtackker)
          {
            num4 += duelTurn.damage - duelTurn.defenderDrainDamage;
            num3 += duelTurn.counterDamage - duelTurn.drainDamage;
          }
          else
          {
            num3 += duelTurn.damage - duelTurn.defenderDrainDamage;
            num4 += duelTurn.counterDamage - duelTurn.drainDamage;
          }
        }
        duelColosseumResult.firstAttackerDamage += num3;
        duelColosseumResult.secondAttackerDamage += num4;
        num1 = Mathf.Min(unit1.parameter.Hp, num1 - num3);
        num2 = Mathf.Min(unit2.parameter.Hp, num2 - num4);
        duelColosseumResult.isDieFirstAttacker = num1 <= 0;
        duelColosseumResult.isDieSecondAttacker = num2 <= 0;
        duelTurnList.AddRange((IEnumerable<BL.DuelTurn>) duelTurnArray);
        if (duelColosseumResult.isDieFirstAttacker || duelColosseumResult.isDieSecondAttacker)
        {
          if (duelColosseumResult.isDieFirstAttacker)
          {
            unit1.deadTurn.Add(battleCount);
            ++unit1.deadCount;
          }
          if (duelColosseumResult.isDieSecondAttacker)
          {
            unit2.deadTurn.Add(battleCount);
            ++unit2.deadCount;
            break;
          }
          break;
        }
        BattleFuncs.consumeSkillEffects(duelTurnArray, (BL.ISkillEffectListUnit) unit1, (BL.ISkillEffectListUnit) unit2, true);
        BattleFuncs.consumeSkillEffectsLate((BL.ISkillEffectListUnit) unit1, (BL.ISkillEffectListUnit) unit2, attackStatus1, attackStatus2, true);
      }
      duelColosseumResult.turns = duelTurnList.ToArray();
      duelColosseumResult.firstAttackerFromDamage = ((IEnumerable<BL.DuelTurn>) duelColosseumResult.turns).Sum<BL.DuelTurn>((Func<BL.DuelTurn, int>) (x => x.isAtackker ? 0 : x.dispDamage));
      duelColosseumResult.secondAttackerFromDamage = ((IEnumerable<BL.DuelTurn>) duelColosseumResult.turns).Sum<BL.DuelTurn>((Func<BL.DuelTurn, int>) (x => !x.isAtackker ? 0 : x.dispDamage));
      return duelColosseumResult;
    }

    private static BL.DuelTurn[] calcTurns(
      BL.Unit attack,
      int attackHp,
      AttackStatus attackAS,
      BL.Unit defense,
      int defenseHp,
      AttackStatus defenseAS,
      bool isAttakerSwap,
      bool invokedAttackerPrayer,
      bool invokedDefenderPrayer,
      int colosseumTurn)
    {
      List<BL.DuelTurn> turns = new List<BL.DuelTurn>();
      TurnHp hp = new TurnHp();
      hp.attackerHp = attackHp;
      hp.defenderHp = defenseHp;
      hp.attackerCantOneMore = false;
      hp.defenderCantOneMore = false;
      hp.otherHp = new Dictionary<BL.ISkillEffectListUnit, TurnOtherHp>();
      if (BattleFuncs.isSkillsAndEffectsInvalid((BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense))
        hp.attackerIsDontUseSkill = true;
      if (BattleFuncs.isSkillsAndEffectsInvalid((BL.ISkillEffectListUnit) defense, (BL.ISkillEffectListUnit) attack))
        hp.defenderIsDontUseSkill = true;
      bool flag1 = BattleFuncs.checkRushInvoke((BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense, attackAS, defenseAS, attackHp, defenseHp, ColosseumBattleCalc.random.Get(), new int?(colosseumTurn));
      bool flag2 = BattleFuncs.checkRushInvoke((BL.ISkillEffectListUnit) defense, (BL.ISkillEffectListUnit) attack, defenseAS, attackAS, defenseHp, attackHp, ColosseumBattleCalc.random.Get(), new int?(colosseumTurn));
      Action action1 = (Action) (() =>
      {
        if (hp.attackerCantOneMore || attackAS == null || !BattleFuncs.canOneMore(attackAS.duelParameter.attackerUnitParameter, attackAS.duelParameter.defenderUnitParameter, (BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense, true, random: ColosseumBattleCalc.random.Get(), attackStatus: attackAS, defenseStatus: defenseAS, myselfHp: hp.attackerHp, enemyHp: hp.defenderHp, colosseumTurn: new int?(colosseumTurn)))
          return;
        ColosseumBattleCalc.calcMultiAttack(turns, hp, true, attack, attackAS, defense, defenseAS, invokedDefenderPrayer, defenseHp, true, colosseumTurn);
      });
      Action action2 = (Action) (() =>
      {
        if (hp.defenderCantOneMore || defenseAS == null || !BattleFuncs.canOneMore(defenseAS.duelParameter.attackerUnitParameter, defenseAS.duelParameter.defenderUnitParameter, (BL.ISkillEffectListUnit) defense, (BL.ISkillEffectListUnit) attack, false, random: ColosseumBattleCalc.random.Get(), attackStatus: defenseAS, defenseStatus: attackAS, myselfHp: hp.defenderHp, enemyHp: hp.attackerHp, colosseumTurn: new int?(colosseumTurn)))
          return;
        ColosseumBattleCalc.calcMultiAttack(turns, hp, false, defense, defenseAS, attack, attackAS, invokedAttackerPrayer, attackHp, true, colosseumTurn);
      });
      if (!isAttakerSwap)
      {
        ColosseumBattleCalc.calcMultiAttack(turns, hp, true, attack, attackAS, defense, defenseAS, invokedDefenderPrayer, defenseHp, false, colosseumTurn);
        if (flag1)
        {
          action1();
          action1 = (Action) null;
        }
        ColosseumBattleCalc.calcMultiAttack(turns, hp, false, defense, defenseAS, attack, attackAS, invokedAttackerPrayer, attackHp, false, colosseumTurn);
        if (flag2)
        {
          action2();
          action2 = (Action) null;
        }
        if (action1 != null)
          action1();
        if (action2 != null)
          action2();
      }
      else
      {
        ColosseumBattleCalc.calcMultiAttack(turns, hp, false, defense, defenseAS, attack, attackAS, invokedAttackerPrayer, attackHp, false, colosseumTurn);
        if (flag2)
        {
          action2();
          action2 = (Action) null;
        }
        ColosseumBattleCalc.calcMultiAttack(turns, hp, true, attack, attackAS, defense, defenseAS, invokedDefenderPrayer, defenseHp, false, colosseumTurn);
        if (flag1)
        {
          action1();
          action1 = (Action) null;
        }
        if (action2 != null)
          action2();
        if (action1 != null)
          action1();
      }
      return turns.ToArray();
    }

    private static void calcMultiAttack(
      List<BL.DuelTurn> turns,
      TurnHp hp,
      bool isAttacker,
      BL.Unit attack,
      AttackStatus attackStatus,
      BL.Unit defense,
      AttackStatus defenseStatus,
      bool invokedPrayer,
      int defenseHp,
      bool isOneMoreAttack,
      int colosseumTurn)
    {
      if (attackStatus == null || hp.isDieAttackerOrDefender())
        return;
      List<BL.DuelTurn> duelTurnList = new List<BL.DuelTurn>();
      int num = BattleFuncs.attackCount((BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense);
      int attackedCount = 0;
      bool isInvalidAttackDuelSkill = false;
      for (; attackedCount < num; ++attackedCount)
        BattleFuncs.calcSingleAttack(turns.Concat<BL.DuelTurn>((IEnumerable<BL.DuelTurn>) duelTurnList).ToList<BL.DuelTurn>(), duelTurnList, hp, isAttacker, (BL.ISkillEffectListUnit) attack, attackStatus, (BL.Panel) null, (BL.ISkillEffectListUnit) defense, defenseStatus, (BL.Panel) null, 0, ColosseumBattleCalc.random.Get(), false, new int?(colosseumTurn), false, invokedPrayer, defenseHp, isOneMoreAttack, attackedCount, isInvalidAttackDuelSkill, false);
      bool flag1 = ColosseumBattleCalc.IsInvokeDuelSkill(duelTurnList);
      int normalAttackCount = attack.originalUnit.playerUnit.normalAttackCount;
      if (normalAttackCount >= 2 && duelTurnList.Any<BL.DuelTurn>() && !(hp.isDieAttackerOrDefender() & flag1))
      {
        if (hp.isDieAttackerOrDefender())
          isInvalidAttackDuelSkill = true;
        for (int index = num * normalAttackCount; attackedCount < index; ++attackedCount)
          BattleFuncs.calcSingleAttack(turns.Concat<BL.DuelTurn>((IEnumerable<BL.DuelTurn>) duelTurnList).ToList<BL.DuelTurn>(), duelTurnList, hp, isAttacker, (BL.ISkillEffectListUnit) attack, attackStatus, (BL.Panel) null, (BL.ISkillEffectListUnit) defense, defenseStatus, (BL.Panel) null, 0, ColosseumBattleCalc.random.Get(), false, new int?(colosseumTurn), false, invokedPrayer, defenseHp, isOneMoreAttack, attackedCount, isInvalidAttackDuelSkill, false);
      }
      bool flag2 = ColosseumBattleCalc.IsInvokeDuelSkill(duelTurnList);
      if (!duelTurnList.Any<BL.DuelTurn>())
        return;
      int count = duelTurnList.Count;
      foreach (BL.DuelTurn duelTurn in duelTurnList)
      {
        if (flag2)
        {
          duelTurn.attackCount = 1;
          duelTurn.isDualSingleAttack = true;
        }
        else
          duelTurn.attackCount = count;
        turns.Add(duelTurn);
        --count;
      }
    }

    private static bool IsInvokeDuelSkill(List<BL.DuelTurn> turns)
    {
      foreach (BL.DuelTurn turn in turns)
      {
        if (((IEnumerable<BL.Skill>) turn.invokeDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
        {
          BattleskillGenre? genre1 = x.genre1;
          BattleskillGenre battleskillGenre = BattleskillGenre.attack;
          return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
        })))
          return true;
      }
      return false;
    }

    private static int getPreemptValue(
      Judgement.BeforeDuelUnitParameter unit,
      GearGear equippedGear,
      int gearWeight)
    {
      return (unit.Agility - gearWeight * 3 + unit.Move * 2 + unit.Luck * 2) * equippedGear.kind.colosseum_preempt_coefficient * 100;
    }

    private static bool getInvokeAmbush(BL.Unit unit, BL.Unit enemyUnit, int hp)
    {
      return !BattleFuncs.isSkillsAndEffectsInvalid((BL.ISkillEffectListUnit) unit, (BL.ISkillEffectListUnit) enemyUnit) && !BattleFuncs.cantInvokeDuelSkill(2, (BL.ISkillEffectListUnit) unit, (BL.ISkillEffectListUnit) enemyUnit, (BL.Panel) null, (BL.Panel) null) && ((IEnumerable<Tuple<BattleskillSkill, int, int>>) unit.unitAndGearSkills).Where<Tuple<BattleskillSkill, int, int>>((Func<Tuple<BattleskillSkill, int, int>, bool>) (x => BattleFuncs.CreatePackedSkillEffects(x.Item1, x.Item2).Any<BattleFuncs.PackedSkillEffect>((Func<BattleFuncs.PackedSkillEffect, bool>) (effect => effect.LogicEnum() == BattleskillEffectLogicEnum.ambush && effect.GetHasKeyEffect(BattleskillEffectLogicArgumentEnum.invoke_gamemode).isEnableGameMode(BattleskillInvokeGameModeEnum.colosseum, (BL.ISkillEffectListUnit) unit) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.unit.kind.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == enemyUnit.unit.kind.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.element) || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.playerUnit.GetElement()) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == enemyUnit.playerUnit.GetElement()) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.job.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == enemyUnit.job.ID) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || enemyUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (!effect.HasKey(BattleskillEffectLogicArgumentEnum.attacker_attack_type) || effect.GetInt(BattleskillEffectLogicArgumentEnum.attacker_attack_type) == 0) && !effect.HasKey(BattleskillEffectLogicArgumentEnum.land_tag1) && (double) hp <= ((double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) * 100.0 + (double) (x.Item2 * 2)) / 100.0 * (double) unit.parameter.Hp)))).FirstOrDefault<Tuple<BattleskillSkill, int, int>>() != null;
    }

    private static int getAttackOrder(
      BL.Unit playerUnit,
      PlayerItem playerEquippedGear,
      PlayerItem playerEquippedGear2,
      PlayerItem playerEquippedGear3,
      PlayerItem playerEquippedReisou,
      PlayerItem playerEquippedReisou2,
      PlayerItem playerEquippedReisou3,
      int playerHp,
      BL.Unit opponentUnit,
      PlayerItem opponentEquippedGear,
      PlayerItem opponentEquippedGear2,
      PlayerItem opponentEquippedGear3,
      PlayerItem opponentEquippedReisou,
      PlayerItem opponentEquippedReisou2,
      PlayerItem opponentEquippedReisou3,
      int oppentHp)
    {
      bool invokeAmbush1 = ColosseumBattleCalc.getInvokeAmbush(playerUnit, opponentUnit, playerHp);
      bool invokeAmbush2 = ColosseumBattleCalc.getInvokeAmbush(opponentUnit, playerUnit, oppentHp);
      if (invokeAmbush1 && !invokeAmbush2)
        return 0;
      if (!invokeAmbush1 & invokeAmbush2)
        return 1;
      Judgement.BeforeDuelParameter colosseumSingle = Judgement.BeforeDuelParameter.CreateColosseumSingle(playerUnit, (BL.MagicBullet) null, new BL.Unit[0], ColosseumBattleCalc._env.Get().playerUnitDict.Values.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x != (BL.Unit) null)).ToArray<BL.Unit>(), playerEquippedGear, playerEquippedGear2, playerEquippedGear3, playerEquippedReisou, playerEquippedReisou2, playerEquippedReisou3, opponentUnit, (BL.MagicBullet) null, new BL.Unit[0], ColosseumBattleCalc._env.Get().opponentUnitDict.Values.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x != (BL.Unit) null)).ToArray<BL.Unit>(), opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3, opponentEquippedReisou, opponentEquippedReisou2, opponentEquippedReisou2, false, true, playerHp, oppentHp, 0, new bool?(), (BL.Weapon) null, 0);
      return ColosseumBattleCalc.getPreemptValue(colosseumSingle.attackerUnitParameter, playerEquippedGear == (PlayerItem) null ? playerUnit.playerUnit.initial_gear : playerEquippedGear.gear, BattleFuncs.calcEquippedGearWeight(playerUnit.playerUnit.initial_gear, playerEquippedGear, playerEquippedGear2, playerEquippedGear3)) < ColosseumBattleCalc.getPreemptValue(colosseumSingle.defenderUnitParameter, opponentEquippedGear == (PlayerItem) null ? opponentUnit.playerUnit.initial_gear : opponentEquippedGear.gear, BattleFuncs.calcEquippedGearWeight(opponentUnit.playerUnit.initial_gear, opponentEquippedGear, opponentEquippedGear2, opponentEquippedGear3)) ? 1 : 0;
    }

    private static void CalcCommandAndReleaseSkill(BL.Unit unit, BL.Unit target, int attackOrder)
    {
      if (BattleFuncs.isSkillsAndEffectsInvalid((BL.ISkillEffectListUnit) unit, (BL.ISkillEffectListUnit) target))
      {
        unit.skills = new BL.Skill[0];
        unit.ougi = (BL.Skill) null;
      }
      else
      {
        List<BL.Skill> skillList = new List<BL.Skill>();
        foreach (BL.Skill skill in unit.skills)
        {
          int? remain = skill.remain;
          if (remain.HasValue && skill.skill.checkEnableUnit((BL.ISkillEffectListUnit) unit) && !((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.transformation)) && BattleFuncs.checkUseSkillInvokeGameMode((BL.ISkillEffectListUnit) unit, skill, true))
          {
            remain = skill.remain;
            float num = Mathf.Clamp(Mathf.Floor((float) (30.0 - (double) Mathf.Pow((float) remain.Value, -1f) * 40.0)), 5f, 100f);
            if (ColosseumBattleCalc.random.Get().RangeInt(0, 100) < (int) num)
            {
              if (skill.targetType == BattleskillTargetType.complex_range || skill.targetType == BattleskillTargetType.complex_single)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                {
                  if (effect.is_targer_enemy)
                  {
                    if (effect.checkUseSkillCount(0))
                      target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.level));
                  }
                  else if (skill.range[0] == 0 && effect.checkUseSkillCount(0))
                    unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.level));
                }
              }
              else if (skill.targetType == BattleskillTargetType.myself)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                {
                  if (effect.checkUseSkillCount(0))
                    unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.level));
                }
              }
              else if (skill.targetType == BattleskillTargetType.player_range || skill.targetType == BattleskillTargetType.player_single)
              {
                if (skill.range[0] == 0)
                {
                  foreach (BattleskillEffect effect in skill.skill.Effects)
                  {
                    if (effect.checkUseSkillCount(0))
                      unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.level));
                  }
                }
              }
              else if (skill.targetType == BattleskillTargetType.enemy_range || skill.targetType == BattleskillTargetType.enemy_single)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                {
                  if (effect.checkUseSkillCount(0))
                    target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.level));
                }
              }
              skillList.Add(skill);
            }
          }
        }
        Func<BattleskillSkill, int, bool> checkAttackOrderSkill = (Func<BattleskillSkill, int, bool>) ((skill, level) => skill.skill_type == BattleskillSkillType.passive && ((IEnumerable<BattleskillEffect>) skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (effect =>
        {
          if (effect.EffectLogic.HasTag(BattleskillEffectTag.ext_arg) || !effect.checkLevel(level))
            return false;
          BattleFuncs.PackedSkillEffect packedSkillEffect = effect.GetPackedSkillEffect();
          return packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.is_attack) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == attackOrder;
        })));
        IEnumerable<PlayerUnitSkills> playerUnitSkillses = ((IEnumerable<PlayerUnitSkills>) unit.playerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => checkAttackOrderSkill(v.skill, v.level)));
        GearGearSkill[] skills = unit.playerUnit.equippedGearOrInitial.skills;
        PlayerItem pGear1 = unit.playerUnit.primary_equipped_gear;
        Func<GearGearSkill, bool> predicate1 = (Func<GearGearSkill, bool>) (v => v.isReleased(pGear1) && checkAttackOrderSkill(v.skill, v.skill_level));
        IEnumerable<GearGearSkill> gearGearSkills1 = ((IEnumerable<GearGearSkill>) skills).Where<GearGearSkill>(predicate1);
        List<GearGearSkill> source1 = unit.playerUnit.equippedGear2 != (PlayerItem) null ? new List<GearGearSkill>((IEnumerable<GearGearSkill>) unit.playerUnit.equippedGear2.skills) : new List<GearGearSkill>();
        PlayerItem pGear2 = unit.playerUnit.primary_equipped_gear2;
        Func<GearGearSkill, bool> predicate2 = (Func<GearGearSkill, bool>) (v => v.isReleased(pGear2) && checkAttackOrderSkill(v.skill, v.skill_level));
        IEnumerable<GearGearSkill> gearGearSkills2 = source1.Where<GearGearSkill>(predicate2);
        List<GearGearSkill> source2 = unit.playerUnit.equippedGear3 != (PlayerItem) null ? new List<GearGearSkill>((IEnumerable<GearGearSkill>) unit.playerUnit.equippedGear3.skills) : new List<GearGearSkill>();
        PlayerItem pGear3 = unit.playerUnit.primary_equipped_gear3;
        Func<GearGearSkill, bool> predicate3 = (Func<GearGearSkill, bool>) (v => v.isReleased(pGear3) && checkAttackOrderSkill(v.skill, v.skill_level));
        IEnumerable<GearGearSkill> gearGearSkills3 = source2.Where<GearGearSkill>(predicate3);
        IEnumerable<GearReisouSkill> gearReisouSkills1 = (unit.playerUnit.equippedReisou != (PlayerItem) null ? (IEnumerable<GearReisouSkill>) new List<GearReisouSkill>((IEnumerable<GearReisouSkill>) unit.playerUnit.equippedReisou.getReisouSkills(unit.playerUnit.equippedGear.entity_id)) : (IEnumerable<GearReisouSkill>) new List<GearReisouSkill>()).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => checkAttackOrderSkill(v.skill, v.skill_level)));
        IEnumerable<GearReisouSkill> gearReisouSkills2 = (unit.playerUnit.equippedReisou2 != (PlayerItem) null ? (IEnumerable<GearReisouSkill>) new List<GearReisouSkill>((IEnumerable<GearReisouSkill>) unit.playerUnit.equippedReisou2.getReisouSkills(unit.playerUnit.equippedGear2.entity_id)) : (IEnumerable<GearReisouSkill>) new List<GearReisouSkill>()).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => checkAttackOrderSkill(v.skill, v.skill_level)));
        IEnumerable<GearReisouSkill> gearReisouSkills3 = (unit.playerUnit.equippedReisou3 != (PlayerItem) null ? (IEnumerable<GearReisouSkill>) new List<GearReisouSkill>((IEnumerable<GearReisouSkill>) unit.playerUnit.equippedReisou3.getReisouSkills(unit.playerUnit.equippedGear3.entity_id)) : (IEnumerable<GearReisouSkill>) new List<GearReisouSkill>()).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => checkAttackOrderSkill(v.skill, v.skill_level)));
        PlayerAwakeSkill[] source3;
        if (unit.playerUnit.equippedExtraSkill == null)
          source3 = new PlayerAwakeSkill[0];
        else
          source3 = new PlayerAwakeSkill[1]
          {
            unit.playerUnit.equippedExtraSkill
          };
        foreach (PlayerAwakeSkill playerAwakeSkill in ((IEnumerable<PlayerAwakeSkill>) source3).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => checkAttackOrderSkill(v.masterData, v.level))))
          skillList.Add(new BL.Skill()
          {
            id = playerAwakeSkill.skill_id,
            level = playerAwakeSkill.level,
            useTurn = 0,
            remain = new int?(0)
          });
        foreach (PlayerUnitSkills playerUnitSkills in playerUnitSkillses)
          skillList.Add(new BL.Skill()
          {
            id = playerUnitSkills.skill_id,
            level = playerUnitSkills.level,
            useTurn = 0,
            remain = new int?(0)
          });
        foreach (GearGearSkill gearGearSkill in gearGearSkills1)
          skillList.Add(new BL.Skill()
          {
            id = gearGearSkill.skill.ID,
            level = gearGearSkill.skill_level,
            useTurn = 0,
            remain = new int?(0)
          });
        foreach (GearGearSkill gearGearSkill in gearGearSkills2)
          skillList.Add(new BL.Skill()
          {
            id = gearGearSkill.skill.ID,
            level = gearGearSkill.skill_level,
            useTurn = 0,
            remain = new int?(0)
          });
        foreach (GearGearSkill gearGearSkill in gearGearSkills3)
          skillList.Add(new BL.Skill()
          {
            id = gearGearSkill.skill.ID,
            level = gearGearSkill.skill_level,
            useTurn = 0,
            remain = new int?(0)
          });
        foreach (GearReisouSkill gearReisouSkill in gearReisouSkills1)
          skillList.Add(new BL.Skill()
          {
            id = gearReisouSkill.skill.ID,
            level = gearReisouSkill.skill_level,
            useTurn = 0,
            remain = new int?(0)
          });
        foreach (GearReisouSkill gearReisouSkill in gearReisouSkills2)
          skillList.Add(new BL.Skill()
          {
            id = gearReisouSkill.skill.ID,
            level = gearReisouSkill.skill_level,
            useTurn = 0,
            remain = new int?(0)
          });
        foreach (GearReisouSkill gearReisouSkill in gearReisouSkills3)
          skillList.Add(new BL.Skill()
          {
            id = gearReisouSkill.skill.ID,
            level = gearReisouSkill.skill_level,
            useTurn = 0,
            remain = new int?(0)
          });
        unit.skills = skillList.ToArray();
        if (!unit.hasOugi)
          return;
        if (unit.ougi.useTurn > 0 && !((IEnumerable<BattleskillEffect>) unit.ougi.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.transformation)) && BattleFuncs.checkUseSkillInvokeGameMode((BL.ISkillEffectListUnit) unit, unit.ougi, true))
        {
          float num = Mathf.Clamp(Mathf.Floor((float) (25.0 - (double) Mathf.Pow((float) unit.ougi.useTurn, 2f) * 0.090000003576278687)), 3f, 100f);
          if (ColosseumBattleCalc.random.Get().RangeInt(0, 100) < (int) num)
          {
            if (unit.ougi.targetType == BattleskillTargetType.complex_range || unit.ougi.targetType == BattleskillTargetType.complex_single)
            {
              foreach (BattleskillEffect effect in unit.ougi.skill.Effects)
              {
                if (effect.is_targer_enemy)
                {
                  if (effect.checkUseSkillCount(0))
                    target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, unit.ougi.skill, unit.ougi.level));
                }
                else if (unit.ougi.range[0] == 0 && effect.checkUseSkillCount(0))
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, unit.ougi.skill, unit.ougi.level));
              }
            }
            else if (unit.ougi.targetType == BattleskillTargetType.myself)
            {
              foreach (BattleskillEffect effect in unit.ougi.skill.Effects)
              {
                if (effect.checkUseSkillCount(0))
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, unit.ougi.skill, unit.ougi.level));
              }
            }
            else if (unit.ougi.targetType == BattleskillTargetType.player_range || unit.ougi.targetType == BattleskillTargetType.player_single)
            {
              if (unit.ougi.range[0] != 0)
                return;
              foreach (BattleskillEffect effect in unit.ougi.skill.Effects)
              {
                if (effect.checkUseSkillCount(0))
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, unit.ougi.skill, unit.ougi.level));
              }
            }
            else
            {
              if (unit.ougi.targetType != BattleskillTargetType.enemy_range && unit.ougi.targetType != BattleskillTargetType.enemy_single)
                return;
              foreach (BattleskillEffect effect in unit.ougi.skill.Effects)
              {
                if (effect.checkUseSkillCount(0))
                  target.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, unit.ougi.skill, unit.ougi.level));
              }
            }
          }
          else
            unit.ougi = (BL.Skill) null;
        }
        else
          unit.ougi = (BL.Skill) null;
      }
    }

    private static Bonus[] SetEnableColosseumBonusEffect(BL.Unit unit, Bonus[] enableBonusList)
    {
      List<Bonus> bonusList = new List<Bonus>();
      if (enableBonusList == null)
        return bonusList.ToArray();
      foreach (Bonus enableBonus in enableBonusList)
      {
        if (Bonus.IsEnableBonus(unit, enableBonus, ColosseumBattleCalc._env.Get().today))
        {
          foreach (BattleskillEffect effect in enableBonus.skill.Effects)
            unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, enableBonus.skill, 1));
          bonusList.Add(enableBonus);
        }
      }
      return bonusList.ToArray();
    }

    public static ColosseumResult calcColosseum(ColosseumEnvironment env, string player_id = null)
    {
      ColosseumBattleCalc.random.Reset(new XorShift(env.colosseumTransactionID));
      if (player_id == null)
        player_id = env.opponentUnitDict[5].playerUnit.player_id;
      ColosseumBattleCalc._env.Reset(env);
      ColosseumResult colosseumResult = new ColosseumResult(env.colosseumTransactionID, player_id);
      for (int index = 1; index <= 5; ++index)
      {
        DuelColosseumResult result = (DuelColosseumResult) null;
        BL.Unit unit1 = env.playerUnitDict[index];
        BL.Unit unit2 = env.opponentUnitDict[index];
        if (unit1 != (BL.Unit) null && unit2 != (BL.Unit) null)
          result = ColosseumBattleCalc.calcColosseumDuel(unit1, unit1.hp, env.playerGearDict[index], env.playerGearDict2[index], env.playerGearDict3[index], env.playerReisouDict[index], env.playerReisouDict2[index], env.playerReisouDict3[index], unit2, unit2.hp, env.opponentGearDict[index], env.opponentGearDict2[index], env.opponentGearDict3[index], env.opponentReisouDict[index], env.opponentReisouDict2[index], env.opponentReisouDict3[index], env.bonusList, index);
        else if (unit1 == (BL.Unit) null && unit2 == (BL.Unit) null)
          result = new DuelColosseumResult();
        else if (unit1 != (BL.Unit) null)
        {
          result = new DuelColosseumResult();
          result.player = unit1;
          result.playerEq = env.playerGearDict[index];
          result.playerEq2 = env.playerGearDict2[index];
          result.playerReisou = env.playerReisouDict[index];
          result.playerReisou2 = env.playerReisouDict2[index];
          result.playerActiveBonus = ColosseumBattleCalc.SetEnableColosseumBonusEffect(unit1, env.bonusList);
          result.isDieOpponent = true;
          result.isPlayerFirstAttacker = true;
        }
        else if (unit2 != (BL.Unit) null)
        {
          result = new DuelColosseumResult();
          result.opponent = unit2;
          result.opponentActiveBonus = ColosseumBattleCalc.SetEnableColosseumBonusEffect(unit2, env.bonusList);
          result.isDiePlayer = true;
          result.isPlayerFirstAttacker = false;
        }
        colosseumResult.SetData(index - 1, result);
      }
      return colosseumResult;
    }
  }
}
