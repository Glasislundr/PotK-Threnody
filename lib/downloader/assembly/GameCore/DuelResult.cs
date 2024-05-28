// Decompiled with JetBrains decompiler
// Type: GameCore.DuelResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore
{
  [Serializable]
  public class DuelResult : ActionResult
  {
    public bool isPlayerAttack;
    public BL.Unit moveUnit;
    public BL.Unit attack;
    public AttackStatus attackAttackStatus;
    public AttackStatus colosseumNewAAS;
    public int colosseumAttackFirstAttack;
    public int? attackDuelSupportId;
    public int attackDuelSupportHitIncr;
    public int attackDuelSupportEvasionIncr;
    public int attackDuelSupportCriticalIncr;
    public int attackDuelSupportCriticalEvasionIncr;
    public BL.Unit defense;
    public AttackStatus defenseAttackStatus;
    public AttackStatus colosseumNewDAS;
    public int colosseumDefenseFirstAttack;
    public int? defenseDuelSupportId;
    public int defenseDuelSupportHitIncr;
    public int defenseDuelSupportEvasionIncr;
    public int defenseDuelSupportCriticalIncr;
    public int defenseDuelSupportCriticalEvasionIncr;
    public BL.DuelTurn[] turns;
    public int attackDamage;
    public int attackFromDamage;
    public int defenseDamage;
    public int defenseFromDamage;
    public bool isDieAttack;
    public bool isDieDefense;
    public bool isBossBattle;
    public bool isFirstBoss;
    public bool isColosseum;
    public bool isExplore;
    public bool disableDuelSkillEffects;
    public bool moveUnitIsCharm;
    public int[] beforeAttakerAilmentEffectIDs;
    public int[] beforeDefenderAilmentEffectIDs;
    public int distance;

    public override ActionResultNetwork ToNetworkLocal(BL env)
    {
      return (ActionResultNetwork) new DuelResultNetwork()
      {
        isPlayerAttack = this.isPlayerAttack,
        moveUnit = (this.moveUnit == (BL.Unit) null ? new int?() : this.moveUnit.ToNetwork(env)),
        attack = (this.attack == (BL.Unit) null ? new int?() : this.attack.ToNetwork(env)),
        attackAttackStatus = this.attackAttackStatus,
        colosseumNewAAS = this.colosseumNewAAS,
        colosseumAttackFirstAttack = this.colosseumAttackFirstAttack,
        attackDuelSupportId = this.attackDuelSupportId,
        attackDuelSupportHitIncr = this.attackDuelSupportHitIncr,
        attackDuelSupportEvasionIncr = this.attackDuelSupportEvasionIncr,
        attackDuelSupportCriticalIncr = this.attackDuelSupportCriticalIncr,
        attackDuelSupportCriticalEvasionIncr = this.attackDuelSupportCriticalEvasionIncr,
        defense = (this.defense == (BL.Unit) null ? new int?() : this.defense.ToNetwork(env)),
        defenseAttackStatus = this.defenseAttackStatus,
        colosseumNewDAS = this.colosseumNewDAS,
        colosseumDefenseFirstAttack = this.colosseumDefenseFirstAttack,
        defenseDuelSupportId = this.defenseDuelSupportId,
        defenseDuelSupportHitIncr = this.defenseDuelSupportHitIncr,
        defenseDuelSupportEvasionIncr = this.defenseDuelSupportEvasionIncr,
        defenseDuelSupportCriticalIncr = this.defenseDuelSupportCriticalIncr,
        defenseDuelSupportCriticalEvasionIncr = this.defenseDuelSupportCriticalEvasionIncr,
        turns = (this.turns == null ? (BL.DuelTurnNetwork[]) null : ((IEnumerable<BL.DuelTurn>) this.turns).Select<BL.DuelTurn, BL.DuelTurnNetwork>((Func<BL.DuelTurn, BL.DuelTurnNetwork>) (x => x.ToNetwork(env))).ToArray<BL.DuelTurnNetwork>()),
        attackDamage = this.attackDamage,
        attackFromDamage = this.attackFromDamage,
        defenseDamage = this.defenseDamage,
        defenseFromDamage = this.defenseFromDamage,
        isDieAttack = this.isDieAttack,
        isDieDefense = this.isDieDefense,
        isBossBattle = this.isBossBattle,
        isFirstBoss = this.isFirstBoss,
        isColosseum = this.isColosseum,
        distance = this.distance,
        beforeAttakerAilmentEffectIDs = this.beforeAttakerAilmentEffectIDs,
        beforeDefenderAilmentEffectIDs = this.beforeDefenderAilmentEffectIDs,
        disableAffterSkills = this.disableDuelSkillEffects,
        moveUnitIsCharm = this.moveUnitIsCharm
      };
    }

    public static ActionResult FromNetwork(ActionResultNetwork nnw, BL env)
    {
      if (!(nnw is DuelResultNetwork duelResultNetwork))
        return (ActionResult) null;
      return (ActionResult) new DuelResult()
      {
        moveUnit = BL.Unit.FromNetwork(duelResultNetwork.moveUnit, env),
        attack = BL.Unit.FromNetwork(duelResultNetwork.attack, env),
        attackAttackStatus = duelResultNetwork.attackAttackStatus,
        colosseumNewAAS = duelResultNetwork.colosseumNewAAS,
        colosseumAttackFirstAttack = duelResultNetwork.colosseumAttackFirstAttack,
        attackDuelSupportId = duelResultNetwork.attackDuelSupportId,
        attackDuelSupportHitIncr = duelResultNetwork.attackDuelSupportHitIncr,
        attackDuelSupportEvasionIncr = duelResultNetwork.attackDuelSupportEvasionIncr,
        attackDuelSupportCriticalIncr = duelResultNetwork.attackDuelSupportCriticalIncr,
        attackDuelSupportCriticalEvasionIncr = duelResultNetwork.attackDuelSupportCriticalEvasionIncr,
        defense = BL.Unit.FromNetwork(duelResultNetwork.defense, env),
        defenseAttackStatus = duelResultNetwork.defenseAttackStatus,
        colosseumNewDAS = duelResultNetwork.colosseumNewDAS,
        colosseumDefenseFirstAttack = duelResultNetwork.colosseumDefenseFirstAttack,
        defenseDuelSupportId = duelResultNetwork.defenseDuelSupportId,
        defenseDuelSupportHitIncr = duelResultNetwork.defenseDuelSupportHitIncr,
        defenseDuelSupportEvasionIncr = duelResultNetwork.defenseDuelSupportEvasionIncr,
        defenseDuelSupportCriticalIncr = duelResultNetwork.defenseDuelSupportCriticalIncr,
        defenseDuelSupportCriticalEvasionIncr = duelResultNetwork.defenseDuelSupportCriticalEvasionIncr,
        turns = (duelResultNetwork.turns == null ? (BL.DuelTurn[]) null : ((IEnumerable<BL.DuelTurnNetwork>) duelResultNetwork.turns).Select<BL.DuelTurnNetwork, BL.DuelTurn>((Func<BL.DuelTurnNetwork, BL.DuelTurn>) (x => BL.DuelTurn.FromNetwork(x, env))).ToArray<BL.DuelTurn>()),
        attackDamage = duelResultNetwork.attackDamage,
        attackFromDamage = duelResultNetwork.attackFromDamage,
        defenseDamage = duelResultNetwork.defenseDamage,
        defenseFromDamage = duelResultNetwork.defenseFromDamage,
        isDieAttack = duelResultNetwork.isDieAttack,
        isDieDefense = duelResultNetwork.isDieDefense,
        isBossBattle = duelResultNetwork.isBossBattle,
        isFirstBoss = duelResultNetwork.isFirstBoss,
        isColosseum = duelResultNetwork.isColosseum,
        distance = duelResultNetwork.distance,
        beforeAttakerAilmentEffectIDs = duelResultNetwork.beforeAttakerAilmentEffectIDs,
        beforeDefenderAilmentEffectIDs = duelResultNetwork.beforeDefenderAilmentEffectIDs,
        disableDuelSkillEffects = duelResultNetwork.disableAffterSkills,
        moveUnitIsCharm = duelResultNetwork.moveUnitIsCharm,
        isPlayerAttack = BL.Unit.FromNetwork(duelResultNetwork.attack, env).isPlayerControl
      };
    }

    public BL.Unit playerUnit() => !this.isPlayerAttack ? this.defense : this.attack;

    public int[] playerUnitBeforeAilmentEffectIDs()
    {
      return !this.isPlayerAttack ? this.beforeDefenderAilmentEffectIDs : this.beforeAttakerAilmentEffectIDs;
    }

    public BL.Unit enemyUnit() => !this.isPlayerAttack ? this.attack : this.defense;

    public int[] enemyUnitBeforeAilmentEffectIDs()
    {
      return !this.isPlayerAttack ? this.beforeAttakerAilmentEffectIDs : this.beforeDefenderAilmentEffectIDs;
    }

    public AttackStatus playerAttackStatus()
    {
      return !this.isPlayerAttack ? this.defenseAttackStatus : this.attackAttackStatus;
    }

    public AttackStatus enemyAttackStatus()
    {
      return !this.isPlayerAttack ? this.attackAttackStatus : this.defenseAttackStatus;
    }

    public AttackStatus playerColosseumNAS()
    {
      return !this.isPlayerAttack ? this.colosseumNewDAS : this.colosseumNewAAS;
    }

    public AttackStatus enemyColosseumNAS()
    {
      return !this.isPlayerAttack ? this.colosseumNewAAS : this.colosseumNewDAS;
    }

    public int playerColosseumFirstAttack()
    {
      return !this.isPlayerAttack ? this.colosseumDefenseFirstAttack : this.colosseumAttackFirstAttack;
    }

    public int enemyColosseumFirstAttack()
    {
      return !this.isPlayerAttack ? this.colosseumAttackFirstAttack : this.colosseumDefenseFirstAttack;
    }

    public int playerAttackDamage()
    {
      return !this.isPlayerAttack ? this.defenseDamage : this.attackDamage;
    }

    public int enemyAttackDamage() => !this.isPlayerAttack ? this.attackDamage : this.defenseDamage;

    public bool isHeal
    {
      get
      {
        return this.attackAttackStatus.magicBullet != null && this.attackAttackStatus.magicBullet.isHeal;
      }
    }

    public IntimateDuelSupport attackDuelSupport
    {
      get
      {
        return !this.attackDuelSupportId.HasValue ? (IntimateDuelSupport) null : MasterData.IntimateDuelSupport[this.attackDuelSupportId.Value];
      }
      set => this.attackDuelSupportId = value == null ? new int?() : new int?(value.ID);
    }

    public IntimateDuelSupport defenseDuelSupport
    {
      get
      {
        return !this.defenseDuelSupportId.HasValue ? (IntimateDuelSupport) null : MasterData.IntimateDuelSupport[this.defenseDuelSupportId.Value];
      }
      set => this.defenseDuelSupportId = value == null ? new int?() : new int?(value.ID);
    }

    public int sumSwapHealDamage(bool isAttacker)
    {
      return ((IEnumerable<BL.DuelTurn>) this.turns).Sum<BL.DuelTurn>((Func<BL.DuelTurn, int>) (x => x.isAtackker != isAttacker ? x.defenderSwapHealDamage : x.attackerSwapHealDamage));
    }
  }
}
