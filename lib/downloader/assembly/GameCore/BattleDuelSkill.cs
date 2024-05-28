// Decompiled with JetBrains decompiler
// Type: GameCore.BattleDuelSkill
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace GameCore
{
  public class BattleDuelSkill
  {
    private bool isHit;
    private bool isCritical;
    private BL.ISkillEffectListUnit attacker;
    private AttackStatus attackStatus;
    private BL.Panel attackPanel;
    private BL.ISkillEffectListUnit defender;
    private AttackStatus defenseStatus;
    private BL.Panel defensePanel;
    private int distance;
    private int currentAttakerHp;
    private int currentDefenderHp;
    private int defenderDuelBeginHp;
    private XorShift random;
    private int? colosseumTurn;
    private bool isAI;
    private bool isBiattack;
    private bool isPrecede;
    private bool attakerIsDontUseSkill;
    private bool defenderIsDontUseSkill;
    private bool isAttacker;
    private bool isInvokedAmbush;
    private bool isInvokedPrayer;
    private int finalAttack;
    private BattleDuelSkill biAttackDuelSkill;
    private BL.Skill invokeAttackerSkill;
    private float? invokeRate;
    private bool isOneMoreAttack;
    private bool isInvalidAttackDuelSkill;
    private BattleDuelSkill.InvokeGenericWork invokeDefenseGenericWork;
    private CommonElement? overwriteElement;
    private List<CommonElement> attackElements;
    private List<BattleFuncs.InvalidSpecificSkillLogic> attackerInvalidSkillLogics;
    private TurnHp turnHp;
    private bool isSimulate;
    [NonSerialized]
    public List<BattleFuncs.InvokeLotteryInfo> lotteryInfos = new List<BattleFuncs.InvokeLotteryInfo>();
    private static BL.Skill[] noneSkills = new BL.Skill[0];

    private bool isColossume => this.colosseumTurn.HasValue;

    public float damageRate { get; private set; }

    public float[] biAttackDamageRate { get; private set; }

    public float attackRate { get; private set; }

    public float damageValue { get; private set; }

    public int? FixDamage { get; private set; }

    public float drainRate { get; private set; }

    public float defenseDownPhysicalRate { get; private set; }

    public float defenseDownMagicRate { get; private set; }

    public int attackCount { get; private set; }

    public float? FixHit { get; private set; }

    public int FixHitPriority { get; private set; }

    public float? FixCritical { get; private set; }

    public bool isChageAttackType { get; private set; }

    public bool isInvokeCounterAttack { get; private set; }

    public float counterDamageRate { get; private set; }

    public float counterAttackRate { get; private set; }

    public float counterDamageHpPercentage { get; private set; }

    public int counterDamageValue { get; private set; }

    public float? PercentageDamageRate { get; private set; }

    public int PercentageDamageMax { get; private set; }

    public float drainRateRatio { get; private set; }

    public float defenseDownPhysicalRateRatio { get; private set; }

    public float defenseDownMagicRateRatio { get; private set; }

    public bool isSuppressCritical { get; private set; }

    public bool attackerCantOneMore { get; private set; }

    public bool isSuppressDuelSkill { get; private set; }

    public bool isAbsoluteDefense { get; private set; }

    public float elementDamageRate { get; private set; }

    public float attackElementDamageRate { get; private set; }

    public float additionalDamage { get; private set; }

    public BL.Skill[] attackerSkills { get; private set; }

    public BL.Skill[] defenderSkills { get; private set; }

    public BL.Skill[] attackerElementSkills { get; private set; }

    public BL.Skill[] defenderElementSkills { get; private set; }

    public List<List<BattleDuelSkill.InvestSkills>> investSkills { get; private set; }

    public List<int> invokeAttackerDuelSkillEffectIds { get; private set; }

    public List<int> invokeDefenderDuelSkillEffectIds { get; private set; }

    public BL.ISkillEffectListUnit[] attackerCombiUnit { get; private set; }

    public List<BattleskillEffect> invokeAttackerSkillEffects { get; private set; }

    public static BattleDuelSkill invokeBiAttackSkills(
      BL.ISkillEffectListUnit attacker,
      AttackStatus attackStatus,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defender,
      AttackStatus defenseStatus,
      BL.Panel defensePanel,
      int distance,
      int currentAttakerHp,
      int currentDefenderHp,
      bool attakerIsDontUseSkill,
      bool defenderIsDontUseSkill,
      XorShift random,
      bool isAI,
      int? colosseumTurn,
      bool isAttacker,
      bool isInvokedAmbush,
      float? invokeRate,
      bool isOneMoreAttack,
      bool isInvalidAttackDuelSkill,
      TurnHp turnHp)
    {
      BattleDuelSkill battleDuelSkill = new BattleDuelSkill()
      {
        random = random,
        attacker = attacker,
        attackStatus = attackStatus,
        attackPanel = attackPanel,
        defender = defender,
        defenseStatus = defenseStatus,
        defensePanel = defensePanel,
        investSkills = (List<List<BattleDuelSkill.InvestSkills>>) null,
        attakerIsDontUseSkill = attakerIsDontUseSkill,
        defenderIsDontUseSkill = defenderIsDontUseSkill,
        isAI = isAI,
        colosseumTurn = colosseumTurn,
        isChageAttackType = false,
        distance = distance,
        currentAttakerHp = currentAttakerHp,
        currentDefenderHp = currentDefenderHp,
        isAttacker = isAttacker,
        isInvokedAmbush = isInvokedAmbush,
        isOneMoreAttack = isOneMoreAttack,
        turnHp = turnHp,
        isPrecede = true,
        isBiattack = false,
        invokeRate = new float?()
      };
      battleDuelSkill.defenderSkills = battleDuelSkill.InvokePrecedeDefender();
      float damageRate = battleDuelSkill.damageRate;
      battleDuelSkill.damageRate = 1f;
      battleDuelSkill.invokeRate = invokeRate;
      battleDuelSkill.invokeAttackerSkillEffects = new List<BattleskillEffect>();
      battleDuelSkill.isInvalidAttackDuelSkill = isInvalidAttackDuelSkill;
      battleDuelSkill.attackerSkills = battleDuelSkill.InvokeBiAttack();
      battleDuelSkill.damageRate *= damageRate;
      battleDuelSkill.applyPrecedeDefenseBiattack();
      return battleDuelSkill;
    }

    public static BattleDuelSkill simulateBiAttackSkills(
      BL.ISkillEffectListUnit attacker,
      AttackStatus attackStatus,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defender,
      AttackStatus defenseStatus,
      BL.Panel defensePanel,
      int distance,
      int currentAttakerHp,
      int currentDefenderHp,
      bool attakerIsDontUseSkill,
      bool defenderIsDontUseSkill,
      bool isAI,
      int? colosseumTurn,
      bool isAttacker,
      bool isInvokedAmbush,
      float? invokeRate,
      bool isOneMoreAttack,
      bool isInvalidAttackDuelSkill,
      TurnHp turnHp)
    {
      BattleDuelSkill battleDuelSkill = new BattleDuelSkill();
      battleDuelSkill.isSimulate = true;
      battleDuelSkill.random = new XorShift();
      battleDuelSkill.attacker = attacker;
      battleDuelSkill.attackStatus = attackStatus;
      battleDuelSkill.attackPanel = attackPanel;
      battleDuelSkill.defender = defender;
      battleDuelSkill.defenseStatus = defenseStatus;
      battleDuelSkill.defensePanel = defensePanel;
      battleDuelSkill.investSkills = (List<List<BattleDuelSkill.InvestSkills>>) null;
      battleDuelSkill.attakerIsDontUseSkill = attakerIsDontUseSkill;
      battleDuelSkill.defenderIsDontUseSkill = defenderIsDontUseSkill;
      battleDuelSkill.isAI = isAI;
      battleDuelSkill.colosseumTurn = colosseumTurn;
      battleDuelSkill.isChageAttackType = false;
      battleDuelSkill.distance = distance;
      battleDuelSkill.currentAttakerHp = currentAttakerHp;
      battleDuelSkill.currentDefenderHp = currentDefenderHp;
      battleDuelSkill.isAttacker = isAttacker;
      battleDuelSkill.isInvokedAmbush = isInvokedAmbush;
      battleDuelSkill.isOneMoreAttack = isOneMoreAttack;
      battleDuelSkill.turnHp = turnHp;
      battleDuelSkill.isPrecede = true;
      battleDuelSkill.isBiattack = false;
      battleDuelSkill.defenderSkills = BattleDuelSkill.noneSkills;
      float damageRate = battleDuelSkill.damageRate;
      battleDuelSkill.damageRate = 1f;
      battleDuelSkill.invokeRate = invokeRate;
      battleDuelSkill.invokeAttackerSkillEffects = new List<BattleskillEffect>();
      battleDuelSkill.isInvalidAttackDuelSkill = isInvalidAttackDuelSkill;
      battleDuelSkill.attackerSkills = battleDuelSkill.InvokeBiAttack();
      battleDuelSkill.damageRate *= damageRate;
      return battleDuelSkill;
    }

    public static Tuple<float, CommonElement?, List<CommonElement>, float, float> getElementAttackRate(
      BL.ISkillEffectListUnit attacker,
      AttackStatus attackStatus,
      BL.ISkillEffectListUnit defender)
    {
      BattleDuelSkill battleDuelSkill = new BattleDuelSkill();
      battleDuelSkill.attacker = attacker;
      battleDuelSkill.attackStatus = attackStatus;
      battleDuelSkill.defender = defender;
      battleDuelSkill.InvokeElementSkill();
      return Tuple.Create<float, CommonElement?, List<CommonElement>, float, float>(battleDuelSkill.damageRate, battleDuelSkill.overwriteElement, battleDuelSkill.attackElements, battleDuelSkill.elementDamageRate, battleDuelSkill.attackElementDamageRate);
    }

    public static BattleDuelSkill invokeDuelSkills(
      BL.ISkillEffectListUnit attacker,
      AttackStatus attackStatus,
      BL.Panel attackPanel,
      BL.ISkillEffectListUnit defender,
      AttackStatus defenseStatus,
      BL.Panel defensePanel,
      int distance,
      int currentAttakerHp,
      int currentDefenderHp,
      bool attakerIsDontUseSkill,
      bool defenderIsDontUseSkill,
      XorShift random,
      bool isAI,
      int? colosseumTurn,
      bool isBiattack,
      bool isAttacker,
      bool isInvokedAmbush,
      bool isInvokedPrayer,
      BattleDuelSkill biAttackDuelSkill,
      float? invokeRate,
      int defenderDuelBeginHp,
      bool isOneMoreAttack,
      bool isInvalidAttackDuelSkill,
      List<BattleFuncs.InvalidSpecificSkillLogic> attackerInvalidSkillLogics,
      TurnHp turnHp)
    {
      BattleDuelSkill battleDuelSkill = new BattleDuelSkill();
      battleDuelSkill.random = random;
      battleDuelSkill.attacker = attacker;
      battleDuelSkill.attackStatus = attackStatus;
      battleDuelSkill.attackPanel = attackPanel;
      battleDuelSkill.defender = defender;
      battleDuelSkill.defenseStatus = defenseStatus;
      battleDuelSkill.defensePanel = defensePanel;
      battleDuelSkill.distance = distance;
      battleDuelSkill.currentAttakerHp = currentAttakerHp;
      battleDuelSkill.currentDefenderHp = currentDefenderHp;
      battleDuelSkill.defenderDuelBeginHp = defenderDuelBeginHp;
      battleDuelSkill.isAI = isAI;
      battleDuelSkill.colosseumTurn = colosseumTurn;
      battleDuelSkill.investSkills = (List<List<BattleDuelSkill.InvestSkills>>) null;
      battleDuelSkill.attakerIsDontUseSkill = attakerIsDontUseSkill;
      battleDuelSkill.defenderIsDontUseSkill = defenderIsDontUseSkill;
      battleDuelSkill.isChageAttackType = false;
      battleDuelSkill.isBiattack = isBiattack;
      battleDuelSkill.isAttacker = isAttacker;
      battleDuelSkill.isInvokedAmbush = isInvokedAmbush;
      battleDuelSkill.isInvokedPrayer = isInvokedPrayer;
      battleDuelSkill.invokeAttackerSkillEffects = new List<BattleskillEffect>();
      battleDuelSkill.biAttackDuelSkill = biAttackDuelSkill;
      battleDuelSkill.invokeRate = invokeRate;
      battleDuelSkill.isOneMoreAttack = isOneMoreAttack;
      battleDuelSkill.isInvalidAttackDuelSkill = isInvalidAttackDuelSkill;
      battleDuelSkill.turnHp = turnHp;
      battleDuelSkill.attackerInvalidSkillLogics = attackerInvalidSkillLogics;
      battleDuelSkill.attackerSkills = BattleDuelSkill.noneSkills;
      battleDuelSkill.InvokeElementSkill();
      return battleDuelSkill;
    }

    public void invokeDefenderSkill(bool isCritical)
    {
      this.isCritical = isCritical;
      this.invokeRate = new float?();
      this.defenderSkills = this.InvokeDefender();
    }

    public static BattleDuelSkill invokeAilmentSkills(
      BL.ISkillEffectListUnit attacker,
      AttackStatus attackStatus,
      BL.ISkillEffectListUnit defender,
      bool isHit,
      bool attakerIsDontUseSkill,
      XorShift random,
      bool isAI,
      int? colosseumTurn,
      TurnHp turnHp,
      BL.Panel attackPanel,
      BL.Panel defensePanel,
      int currentAttakerHp,
      int currentDefenderHp)
    {
      BattleDuelSkill battleDuelSkill = new BattleDuelSkill()
      {
        random = random,
        attacker = attacker,
        attackStatus = attackStatus,
        defender = defender,
        isHit = isHit,
        investSkills = (List<List<BattleDuelSkill.InvestSkills>>) null,
        isAI = isAI,
        colosseumTurn = colosseumTurn,
        attakerIsDontUseSkill = attakerIsDontUseSkill,
        turnHp = turnHp,
        attackPanel = attackPanel,
        defensePanel = defensePanel,
        currentAttakerHp = currentAttakerHp,
        currentDefenderHp = currentDefenderHp
      };
      battleDuelSkill.attackerSkills = battleDuelSkill.InvokeAilmentSkills(isAI);
      return battleDuelSkill;
    }

    private BattleDuelSkill()
    {
      this.damageRate = 1f;
      this.biAttackDamageRate = (float[]) null;
      this.attackRate = 1f;
      this.damageValue = 0.0f;
      this.FixDamage = new int?();
      this.drainRate = 0.0f;
      this.defenseDownPhysicalRate = 1f;
      this.defenseDownMagicRate = 1f;
      this.attackCount = 1;
      this.FixHit = new float?();
      this.FixHitPriority = 0;
      this.FixCritical = new float?();
      this.attacker = (BL.ISkillEffectListUnit) null;
      this.attackStatus = (AttackStatus) null;
      this.attackPanel = (BL.Panel) null;
      this.defender = (BL.ISkillEffectListUnit) null;
      this.defenseStatus = (AttackStatus) null;
      this.defensePanel = (BL.Panel) null;
      this.distance = 0;
      this.currentAttakerHp = 0;
      this.currentDefenderHp = 0;
      this.isInvokeCounterAttack = false;
      this.counterDamageRate = 0.0f;
      this.counterAttackRate = 0.0f;
      this.counterDamageHpPercentage = 0.0f;
      this.counterDamageValue = 0;
      this.PercentageDamageRate = new float?();
      this.PercentageDamageMax = 0;
      this.invokeAttackerSkill = (BL.Skill) null;
      this.invokeAttackerSkillEffects = (List<BattleskillEffect>) null;
      this.biAttackDuelSkill = (BattleDuelSkill) null;
      this.drainRateRatio = 1f;
      this.defenseDownPhysicalRateRatio = 1f;
      this.defenseDownMagicRateRatio = 1f;
      this.isSuppressCritical = false;
      this.attackerCantOneMore = false;
      this.isSuppressDuelSkill = false;
      this.isAbsoluteDefense = false;
      this.invokeRate = new float?();
      this.invokeAttackerDuelSkillEffectIds = new List<int>();
      this.invokeDefenderDuelSkillEffectIds = new List<int>();
      this.additionalDamage = 0.0f;
    }

    private BL.Skill[] InvokeDefender()
    {
      if (this.biAttackDuelSkill.defenderSkills != BattleDuelSkill.noneSkills || this.defender.IsDontAction || this.defenderIsDontUseSkill || BattleFuncs.cantInvokeDuelSkill(1, this.defender, this.attacker, this.defensePanel, this.attackPanel))
        return BattleDuelSkill.noneSkills;
      BL.Skill[] skillArray = this.InvokeDefenderGeneric();
      if (skillArray != BattleDuelSkill.noneSkills)
        return skillArray;
      Dictionary<BattleskillEffectLogicEnum, Func<BL.Skill, BattleskillEffect, bool>> dictionary = new Dictionary<BattleskillEffectLogicEnum, Func<BL.Skill, BattleskillEffect, bool>>()
      {
        {
          BattleskillEffectLogicEnum.shield,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcShield)
        },
        {
          BattleskillEffectLogicEnum.out_of_range_defense,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcOutOfRangeDefence)
        },
        {
          BattleskillEffectLogicEnum.counter,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcCounter)
        }
      };
      foreach (BL.Skill duelSkill in this.defender.originalUnit.duelSkills)
      {
        if (!this.defender.IsDontUseSkill(duelSkill.id))
        {
          foreach (BattleskillEffect effect in duelSkill.skill.Effects)
          {
            BattleskillEffectLogicEnum key = effect.EffectLogic.Enum;
            if (dictionary.ContainsKey(key) && dictionary[key](duelSkill, effect))
              return new BL.Skill[1]{ duelSkill };
          }
        }
      }
      return BattleDuelSkill.noneSkills;
    }

    private BL.Skill[] InvokePrecedeDefender()
    {
      if (this.defender.IsDontAction || this.defenderIsDontUseSkill || BattleFuncs.cantInvokeDuelSkill(1, this.defender, this.attacker, this.defensePanel, this.attackPanel))
        return BattleDuelSkill.noneSkills;
      BL.Skill[] skillArray = this.InvokeDefenderGeneric();
      return skillArray != BattleDuelSkill.noneSkills ? skillArray : BattleDuelSkill.noneSkills;
    }

    private void applyPrecedeDefenseBiattack()
    {
      if (this.invokeDefenseGenericWork == null || !this.invokeDefenseGenericWork.HasKey(BattleskillEffectLogicArgumentEnum.skill_id1))
        return;
      for (int attackNo = 0; attackNo < this.attackCount; ++attackNo)
        this.funcGenericSkillInvest(this.invokeDefenseGenericWork, this.defender, this.attacker, attackNo);
    }

    private BL.Skill[] InvokeBiAttack()
    {
      if (this.isSuppressDuelSkill || this.attacker.IsDontAction || this.attakerIsDontUseSkill || this.attackStatus.magicBullet != null && this.attackStatus.magicBullet.percentageDamage != null || BattleFuncs.cantInvokeDuelSkill(0, this.attacker, this.defender, this.attackPanel, this.defensePanel))
        return BattleDuelSkill.noneSkills;
      BL.Skill[] skillArray = this.InvokeAttackerGeneric();
      if (skillArray != BattleDuelSkill.noneSkills)
        return skillArray;
      Dictionary<BattleskillEffectLogicEnum, Func<BL.Skill, BattleskillEffect, bool>> dictionary = new Dictionary<BattleskillEffectLogicEnum, Func<BL.Skill, BattleskillEffect, bool>>()
      {
        {
          BattleskillEffectLogicEnum.suisei,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcBiAttack)
        },
        {
          BattleskillEffectLogicEnum.ryusei,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcRyusei)
        },
        {
          BattleskillEffectLogicEnum.gekko,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcGekko)
        },
        {
          BattleskillEffectLogicEnum.taiyo,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcTaiyo)
        },
        {
          BattleskillEffectLogicEnum.magic_suisei,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcMagicSuisei)
        },
        {
          BattleskillEffectLogicEnum.combi_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcCombiAttack)
        },
        {
          BattleskillEffectLogicEnum.magic_physical_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcIntelligenceToPhysicalAttackUp)
        },
        {
          BattleskillEffectLogicEnum.agility_physical_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcAgilityToPhysicalAttackUp)
        },
        {
          BattleskillEffectLogicEnum.dexterity_physical_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcDexterityToPhysicalAttackUp)
        },
        {
          BattleskillEffectLogicEnum.luck_physical_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcLuckToPhysicalAttackUp)
        },
        {
          BattleskillEffectLogicEnum.strength_magic_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcStrengthToMagicAttackUp)
        },
        {
          BattleskillEffectLogicEnum.agility_magic_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcAgilityToMagicAttackUp)
        },
        {
          BattleskillEffectLogicEnum.dexterity_magic_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcDexterityToMagicAttackUp)
        },
        {
          BattleskillEffectLogicEnum.luck_magic_attack,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcLuckToMagicAttackUp)
        },
        {
          BattleskillEffectLogicEnum.instant_death,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcInstantDeath)
        },
        {
          BattleskillEffectLogicEnum.revenge,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcRevenge)
        },
        {
          BattleskillEffectLogicEnum.mdmg_combi,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcMdmgCombi)
        },
        {
          BattleskillEffectLogicEnum.change_attack_type,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcChangeAttackType)
        },
        {
          BattleskillEffectLogicEnum.invest_passive,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcInvokePassive)
        },
        {
          BattleskillEffectLogicEnum.anohana_trio,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcAnohanaTrio)
        },
        {
          BattleskillEffectLogicEnum.snake_venom,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcSnakeVenom)
        },
        {
          BattleskillEffectLogicEnum.percentage_damage,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcPercentageDamage)
        }
      };
      foreach (BL.Skill duelSkill in this.attacker.originalUnit.duelSkills)
      {
        this.lotteryInfos.Clear();
        if (this.isInvalidAttackDuelSkill)
        {
          BattleskillGenre? genre1 = duelSkill.genre1;
          BattleskillGenre battleskillGenre = BattleskillGenre.attack;
          if (genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue)
            continue;
        }
        if (!this.attacker.IsDontUseSkill(duelSkill.id))
        {
          foreach (BattleskillEffect effect in duelSkill.skill.Effects)
          {
            BattleskillEffectLogicEnum key = effect.EffectLogic.Enum;
            if (dictionary.ContainsKey(key) && dictionary[key](duelSkill, effect))
            {
              this.invokeAttackerSkill = duelSkill;
              this.invokeAttackerSkillEffects.Add(effect);
              if (!this.isSimulate)
                return new BL.Skill[1]{ duelSkill };
            }
          }
          if (this.isSimulate && this.invokeAttackerSkill != null)
            return new BL.Skill[1]{ duelSkill };
        }
      }
      return BattleDuelSkill.noneSkills;
    }

    private BL.Skill[] InvokeAilmentSkills(bool isAI)
    {
      if (!BattleFuncs.isSkillsAndEffectsInvalid(this.attacker, this.defender))
      {
        IEnumerable<BattleskillEffect> investSkillEffect = this.attackStatus.magicBullet != null ? this.attackStatus.magicBullet.investSkillEffect : (IEnumerable<BattleskillEffect>) null;
        if (investSkillEffect != null && investSkillEffect.Any<BattleskillEffect>())
        {
          foreach (BattleskillEffect battleskillEffect in investSkillEffect)
          {
            int num = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
            bool flag1;
            if (num < 0)
            {
              flag1 = true;
              num = -num;
            }
            else
              flag1 = false;
            if (num != 0 && MasterData.BattleskillSkill.ContainsKey(num) && MasterData.BattleskillSkill[num].skill_type == BattleskillSkillType.ailment)
            {
              foreach (BL.ISkillEffectListUnit skillEffectListUnit in this.getInvestUnit(this.attacker, this.defender, num, battleskillEffect.HasKey(BattleskillEffectLogicArgumentEnum.range_type) ? battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.range_type) : 0))
              {
                List<BL.SkillEffect> useResistEffects;
                bool flag2 = BattleFuncs.isAilmentInvest(battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest), num, skillEffectListUnit, this.attacker, this.random, this.colosseumTurn, out useResistEffects, this.turnHp, new int?(this.getUnitCurrentHp(skillEffectListUnit)), new int?(this.getUnitCurrentHp(this.attacker)));
                this.addInvestSkills(skillEffectListUnit, BattleFuncs.ailmentInvest(num, skillEffectListUnit), new int[1]
                {
                  num
                }, this.attacker, battleskillEffect.skill.ID, true, (flag1 ? 1 : 0) != 0, 0, isSuccess: (flag2 ? 1 : 0) != 0, useResistEffects: useResistEffects);
              }
            }
          }
        }
      }
      if (this.attacker.IsDontAction || this.attakerIsDontUseSkill || BattleFuncs.cantInvokeDuelSkill(0, this.attacker, this.defender, this.attackPanel, this.defensePanel))
        return BattleDuelSkill.noneSkills;
      Dictionary<BattleskillEffectLogicEnum, Func<BL.Skill, BattleskillEffect, bool, bool>> dictionary = new Dictionary<BattleskillEffectLogicEnum, Func<BL.Skill, BattleskillEffect, bool, bool>>()
      {
        {
          BattleskillEffectLogicEnum.invest_skilleffect,
          new Func<BL.Skill, BattleskillEffect, bool, bool>(this.funcInvestSkillEffect)
        }
      };
      foreach (BL.Skill duelSkill in this.attacker.originalUnit.duelSkills)
      {
        if (!this.attacker.IsDontUseSkill(duelSkill.id))
        {
          foreach (BattleskillEffect effect in duelSkill.skill.Effects)
          {
            BattleskillEffectLogicEnum key = effect.EffectLogic.Enum;
            if (dictionary.ContainsKey(key) && dictionary[key](duelSkill, effect, isAI))
              return new BL.Skill[1]{ duelSkill };
          }
        }
      }
      return BattleDuelSkill.noneSkills;
    }

    private void InvokeElementSkill()
    {
      Decimal num1 = 1.0M;
      List<CommonElement> element = new List<CommonElement>();
      this.attackerElementSkills = BattleDuelSkill.noneSkills;
      this.defenderElementSkills = BattleDuelSkill.noneSkills;
      int num2 = 0;
      BL.SkillEffect skillEffect = this.attacker.skillEffects.Where(BattleskillEffectLogicEnum.invest_attack_element).FirstOrDefault<BL.SkillEffect>();
      if (skillEffect != null)
      {
        int key = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element_skill_id);
        if (key != 0 && MasterData.BattleskillSkill.ContainsKey(key))
          num2 = key;
      }
      if (num2 != 0)
      {
        BL.Skill skill = new BL.Skill() { id = num2 };
        this.attackerElementSkills = new BL.Skill[1]
        {
          skill
        };
        element.Add(skill.skill.element);
        this.overwriteElement = new CommonElement?(skill.skill.element);
      }
      else
      {
        this.attackerElementSkills = ((IEnumerable<BL.Skill>) this.attacker.originalUnit.duelSkills).Where<BL.Skill>((Func<BL.Skill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.EffectLogic.Enum == BattleskillEffectLogicEnum.invest_element)))).ToArray<BL.Skill>();
        element.AddRange(((IEnumerable<BL.Skill>) this.attackerElementSkills).Select<BL.Skill, CommonElement>((Func<BL.Skill, CommonElement>) (x => x.skill.element)));
      }
      this.attackElements = element;
      this.defenderElementSkills = ((IEnumerable<BL.Skill>) this.defender.originalUnit.duelSkills).Where<BL.Skill>((Func<BL.Skill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.EffectLogic.Enum == BattleskillEffectLogicEnum.effect_element && ef.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && element.Any<CommonElement>((Func<CommonElement, bool>) (e => (CommonElement) ef.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == e)))))).ToArray<BL.Skill>();
      if (this.defenderElementSkills.Length != 0)
      {
        foreach (CommonElement commonElement in element)
        {
          CommonElement e = commonElement;
          foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BL.Skill>) this.defenderElementSkills).SelectMany<BL.Skill, BattleskillEffect>((Func<BL.Skill, IEnumerable<BattleskillEffect>>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.EffectLogic.Enum == BattleskillEffectLogicEnum.effect_element && ef.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && (CommonElement) ef.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == e)).Select<BattleskillEffect, BattleskillEffect>((Func<BattleskillEffect, BattleskillEffect>) (ef => ef)))))
            num1 *= (Decimal) battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_ratio);
        }
      }
      Decimal d = 1.0M;
      element = new List<CommonElement>();
      if (this.attackStatus.isMagic && this.attackStatus.magicBullet != null && this.attackStatus.magicBullet.skill != null && this.attackStatus.magicBullet.skill.Effects != null)
        element.Add(this.attackStatus.magicBullet.skill.element);
      else if (this.attackStatus.weapon != null)
        element.Add(this.attackStatus.weapon.attackMethod.element);
      else
        element.Add(this.attacker.originalUnit.playerUnit.equippedGearOrInitial.attachedElement);
      BL.Skill[] array = ((IEnumerable<BL.Skill>) this.defender.originalUnit.duelSkills).Where<BL.Skill>((Func<BL.Skill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.EffectLogic.Enum == BattleskillEffectLogicEnum.effect_element && ef.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && element.Any<CommonElement>((Func<CommonElement, bool>) (e => (CommonElement) ef.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == e)))))).ToArray<BL.Skill>();
      if (array.Length != 0)
      {
        foreach (CommonElement commonElement in element)
        {
          CommonElement e = commonElement;
          foreach (BattleskillEffect battleskillEffect in ((IEnumerable<BL.Skill>) array).SelectMany<BL.Skill, BattleskillEffect>((Func<BL.Skill, IEnumerable<BattleskillEffect>>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.EffectLogic.Enum == BattleskillEffectLogicEnum.effect_element && ef.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && (CommonElement) ef.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == e)))))
          {
            Decimal num3 = (Decimal) battleskillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.counter_ratio);
            if (num3 != 0M)
              d *= num3;
          }
        }
      }
      foreach (CommonElement argumentCheckValue in element)
        d *= (Decimal) BattleFuncs.calcSpecResistDamageRate(this.attacker, this.defender, (int) argumentCheckValue, BattleskillEffectLogicEnum.attack_element_spec_ratio, BattleskillEffectLogicEnum.attack_element_resist_ratio, BattleskillEffectLogicArgumentEnum.attack_element_id, BattleskillEffectLogicArgumentEnum.target_attack_element_id);
      Decimal num4 = Math.Round(d, 4);
      this.elementDamageRate = (float) num1;
      this.attackElementDamageRate = (float) num4;
      this.damageRate = (float) Math.Round((Decimal) this.damageRate * num1 * num4, 4);
    }

    public void InvokeDamageSkill(int finalAttack)
    {
      this.finalAttack = finalAttack;
      this.isHit = finalAttack >= 1;
      this.invokeRate = new float?();
      BL.Skill[] second1 = this.InvokeAttackerDamageSkill();
      if (second1 != BattleDuelSkill.noneSkills)
        this.attackerSkills = ((IEnumerable<BL.Skill>) this.attackerSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) second1).ToArray<BL.Skill>();
      BL.Skill[] second2 = this.InvokeDefenderDamageSkill();
      if (second2 == BattleDuelSkill.noneSkills)
        return;
      this.defenderSkills = ((IEnumerable<BL.Skill>) this.defenderSkills).Concat<BL.Skill>((IEnumerable<BL.Skill>) second2).ToArray<BL.Skill>();
    }

    public BL.Skill[] InvokeAttackerDamageSkill() => BattleDuelSkill.noneSkills;

    public BL.Skill[] InvokeDefenderDamageSkill()
    {
      if (this.defender.IsDontAction || this.defenderIsDontUseSkill || BattleFuncs.cantInvokeDuelSkill(1, this.defender, this.attacker, this.defensePanel, this.attackPanel))
        return BattleDuelSkill.noneSkills;
      Dictionary<BattleskillEffectLogicEnum, Func<BL.Skill, BattleskillEffect, bool>> dictionary = new Dictionary<BattleskillEffectLogicEnum, Func<BL.Skill, BattleskillEffect, bool>>()
      {
        {
          BattleskillEffectLogicEnum.prayer,
          new Func<BL.Skill, BattleskillEffect, bool>(this.funcPrayer)
        }
      };
      foreach (BL.Skill duelSkill in this.defender.originalUnit.duelSkills)
      {
        if (!this.defender.IsDontUseSkill(duelSkill.id))
        {
          foreach (BattleskillEffect effect in duelSkill.skill.Effects)
          {
            BattleskillEffectLogicEnum key = effect.EffectLogic.Enum;
            if (dictionary.ContainsKey(key) && dictionary[key](duelSkill, effect))
              return new BL.Skill[1]{ duelSkill };
          }
        }
      }
      return BattleDuelSkill.noneSkills;
    }

    private bool funcBiAttack(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.attackCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_count);
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      return true;
    }

    private bool funcRyusei(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      return true;
    }

    private bool funcMagicSuisei(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage) * effect.GetFloat(BattleskillEffectLogicArgumentEnum.attack_count);
      return true;
    }

    private bool funcGekko(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      this.defenseDownPhysicalRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_decrease);
      return true;
    }

    private bool funcTaiyo(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      float num1 = effect.GetFloat(BattleskillEffectLogicArgumentEnum.hit_value);
      if ((double) num1 > 0.0)
        this.FixHit = new float?(num1);
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      float num2 = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_drain);
      if ((double) this.drainRate < (double) num2)
        this.drainRate = num2;
      return true;
    }

    private bool funcParameterToPhysicalAttackUp(
      BL.Skill skill,
      BattleskillEffect effect,
      int base_parameter)
    {
      if (this.attackStatus.isMagic || !this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.damageValue = (float) base_parameter * effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      return true;
    }

    private bool funcIntelligenceToPhysicalAttackUp(BL.Skill skill, BattleskillEffect effect)
    {
      return this.funcParameterToPhysicalAttackUp(skill, effect, this.attackStatus.duelParameter.attackerUnitParameter.Intelligence);
    }

    private bool funcAgilityToPhysicalAttackUp(BL.Skill skill, BattleskillEffect effect)
    {
      return this.funcParameterToPhysicalAttackUp(skill, effect, this.attackStatus.duelParameter.attackerUnitParameter.Agility);
    }

    private bool funcDexterityToPhysicalAttackUp(BL.Skill skill, BattleskillEffect effect)
    {
      return this.funcParameterToPhysicalAttackUp(skill, effect, this.attackStatus.duelParameter.attackerUnitParameter.Dexterity);
    }

    private bool funcLuckToPhysicalAttackUp(BL.Skill skill, BattleskillEffect effect)
    {
      return this.funcParameterToPhysicalAttackUp(skill, effect, this.attackStatus.duelParameter.attackerUnitParameter.Luck);
    }

    private bool funcParameterToMagicAttackUp(
      BL.Skill skill,
      BattleskillEffect effect,
      int base_parameter)
    {
      if (!this.attackStatus.isMagic || !this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.damageValue = (float) base_parameter * effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      return true;
    }

    private bool funcStrengthToMagicAttackUp(BL.Skill skill, BattleskillEffect effect)
    {
      return this.funcParameterToMagicAttackUp(skill, effect, this.attackStatus.duelParameter.attackerUnitParameter.Strength);
    }

    private bool funcAgilityToMagicAttackUp(BL.Skill skill, BattleskillEffect effect)
    {
      return this.funcParameterToMagicAttackUp(skill, effect, this.attackStatus.duelParameter.attackerUnitParameter.Agility);
    }

    private bool funcDexterityToMagicAttackUp(BL.Skill skill, BattleskillEffect effect)
    {
      return this.funcParameterToMagicAttackUp(skill, effect, this.attackStatus.duelParameter.attackerUnitParameter.Dexterity);
    }

    private bool funcLuckToMagicAttackUp(BL.Skill skill, BattleskillEffect effect)
    {
      return this.funcParameterToMagicAttackUp(skill, effect, this.attackStatus.duelParameter.attackerUnitParameter.Luck);
    }

    private bool funcInstantDeath(BL.Skill skill, BattleskillEffect effect)
    {
      if (!BattleFuncs.isGearEquipped(this.attacker.originalUnit.playerUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.equip_gear_king_id)) || !this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.FixDamage = new int?(this.currentDefenderHp);
      return true;
    }

    private bool funcRevenge(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      int num1 = this.attacker.originalUnit.parameter.Hp - this.currentAttakerHp;
      float num2 = 0.0f;
      if (effect.HasKey(BattleskillEffectLogicArgumentEnum.base_value_damage))
        num2 = effect.GetFloat(BattleskillEffectLogicArgumentEnum.base_value_damage);
      this.damageValue = num2 + (float) num1 * effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      return true;
    }

    private bool funcMdmgCombi(BL.Skill skill, BattleskillEffect effect)
    {
      if (this.isColossume)
        return false;
      List<BL.ISkillEffectListUnit> list1 = BattleFuncs.getTargets(this.attacker.originalUnit, new int[2]
      {
        effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
        effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
      }, new BL.ForceID[1]
      {
        BattleFuncs.getForceID(this.attacker.originalUnit)
      }, BL.Unit.TargetAttribute.all, (this.isAI ? 1 : 0) != 0, nonFacility: true).Select<BL.UnitPosition, BL.ISkillEffectListUnit>((Func<BL.UnitPosition, BL.ISkillEffectListUnit>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x))).ToList<BL.ISkillEffectListUnit>();
      Judgement.BeforeDuelParameter beforeDuelParameter = (Judgement.BeforeDuelParameter) null;
      bool flag1 = false;
      float percentage_invocation = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation);
      float num1 = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      if (list1.Count > 0)
      {
        int target_skill_id = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_id);
        List<BL.ISkillEffectListUnit> list2 = list1.Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => ((IEnumerable<BL.Skill>) x.originalUnit.duelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (duelSkill => duelSkill.id == target_skill_id)) && this.attacker.originalUnit.unit.character.ID != x.originalUnit.unit.character.ID)).ToList<BL.ISkillEffectListUnit>();
        if (list2.Count > 0)
        {
          Tuple<int, int> pos = BattleFuncs.getUnitCell(this.attacker.originalUnit, this.isAI);
          BL.ISkillEffectListUnit beAttackUnit = list2.OrderBy<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x =>
          {
            int num2 = 0;
            Tuple<int, int> unitCell = BattleFuncs.getUnitCell(x.originalUnit, this.isAI);
            int num3 = BL.fieldDistance(pos.Item1, pos.Item2, unitCell.Item1, unitCell.Item2);
            int num4 = unitCell.Item1 - pos.Item1;
            int num5 = unitCell.Item2 - pos.Item2;
            int num6 = 0;
            for (int index = 0; index < num3; ++index)
              num6 += 4 * index;
            if (num4 < 0)
              num2 = num6 + num3 * 3 - num5;
            else if (num4 > 0)
              num2 = num6 + num3 + num5;
            else if (num5 > 0)
              num2 = num6 + num3 * 2;
            else if (num5 < 0)
              num2 = num6;
            return num2;
          })).FirstOrDefault<BL.ISkillEffectListUnit>();
          if (beAttackUnit != null)
          {
            Tuple<int, int> unitCell1 = BattleFuncs.getUnitCell(this.defender.originalUnit, this.isAI);
            BL.MagicBullet beAttackMagicBullet;
            if (beAttackUnit.originalUnit.unit.magic_warrior_flag)
            {
              beAttackMagicBullet = (BL.MagicBullet) null;
              flag1 = false;
            }
            else
            {
              beAttackMagicBullet = ((IEnumerable<BL.MagicBullet>) beAttackUnit.originalUnit.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x => x.isAttack)).OrderBy<BL.MagicBullet, int>((Func<BL.MagicBullet, int>) (x => x.cost)).FirstOrDefault<BL.MagicBullet>();
              flag1 = beAttackMagicBullet != null;
            }
            beforeDuelParameter = Judgement.BeforeDuelParameter.CreateDuelSkill(beAttackUnit, beAttackMagicBullet, BattleFuncs.getPanel(pos.Item1, pos.Item2), this.defender, BattleFuncs.getPanel(unitCell1.Item1, unitCell1.Item2), this.distance, this.currentDefenderHp);
            Tuple<int, int> unitCell2 = BattleFuncs.getUnitCell(beAttackUnit.originalUnit, this.isAI);
            int num7 = BL.fieldDistance(pos.Item1, pos.Item2, unitCell2.Item1, unitCell2.Item2);
            int num8 = Mathf.Max(0, effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) - (num7 - 1));
            percentage_invocation += (float) num8 * effect.GetFloat(BattleskillEffectLogicArgumentEnum.range_add_precentage_invocation);
            num1 += (float) num8 * effect.GetFloat(BattleskillEffectLogicArgumentEnum.range_add_precentage_damage);
          }
        }
      }
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, percentage_invocation, this.currentAttakerHp, this.currentDefenderHp))
        return false;
      if (beforeDuelParameter != null)
        this.additionalDamage = !flag1 ? (float) beforeDuelParameter.DisplayPhysicalAttack : (float) beforeDuelParameter.DisplayMagicAttack;
      else
        num1 *= 1.5f;
      this.damageRate *= num1;
      int num9 = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
      List<BL.SkillEffect> useResistEffects;
      bool flag2 = BattleFuncs.isAilmentInvest(1f, num9, this.defender, this.attacker, this.random, this.colosseumTurn, out useResistEffects, this.turnHp, new int?(this.getUnitCurrentHp(this.defender)), new int?(this.getUnitCurrentHp(this.attacker)));
      this.addInvestSkills(this.defender, BattleFuncs.ailmentInvest(num9, this.defender), new int[1]
      {
        num9
      }, this.attacker, skill.id, true, false, 0, isSuccess: (flag2 ? 1 : 0) != 0, useResistEffects: useResistEffects);
      return true;
    }

    private bool funcChangeAttackType(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.isChageAttackType = true;
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      this.attackRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_attack);
      return true;
    }

    private bool funcInvokePassive(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
      foreach (BL.ISkillEffectListUnit skillEffectListUnit in this.getInvestUnit(this.attacker, this.defender, num, effect.HasKey(BattleskillEffectLogicArgumentEnum.range_type) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.range_type) : 0))
      {
        float lottery = 1f;
        if (effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage_invest))
          lottery = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest) + (effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage_invest_levelup) ? effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest_levelup) : 0.0f) * (float) skill.level;
        if (MasterData.BattleskillSkill[num].skill_type == BattleskillSkillType.ailment)
        {
          List<BL.SkillEffect> useResistEffects;
          bool flag = BattleFuncs.isAilmentInvest(lottery, num, skillEffectListUnit, this.attacker, this.random, this.colosseumTurn, out useResistEffects, this.turnHp, new int?(this.getUnitCurrentHp(skillEffectListUnit)), new int?(this.getUnitCurrentHp(this.attacker)));
          this.addInvestSkills(skillEffectListUnit, BattleFuncs.ailmentInvest(num, skillEffectListUnit), new int[1]
          {
            num
          }, this.attacker, skill.id, true, false, 0, isSuccess: (flag ? 1 : 0) != 0, useResistEffects: useResistEffects);
        }
        else if ((double) lottery >= (double) this.random.NextFloat())
          this.addInvestSkills(skillEffectListUnit, new BL.Skill[1]
          {
            new BL.Skill() { id = num }
          }, new int[1]{ num }, this.attacker, skill.id, false, false, 0);
      }
      return true;
    }

    private bool funcAnohanaTrio(BL.Skill skill, BattleskillEffect effect)
    {
      if (this.isColossume)
        return false;
      List<BL.ISkillEffectListUnit> list1 = BattleFuncs.getTargets(this.attacker.originalUnit, new int[2]
      {
        1,
        effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
      }, new BL.ForceID[1]
      {
        BattleFuncs.getForceID(this.attacker.originalUnit)
      }, BL.Unit.TargetAttribute.all, (this.isAI ? 1 : 0) != 0, nonFacility: true).Select<BL.UnitPosition, BL.ISkillEffectListUnit>((Func<BL.UnitPosition, BL.ISkillEffectListUnit>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x))).ToList<BL.ISkillEffectListUnit>();
      Judgement.BeforeDuelParameter beforeDuelParameter1 = (Judgement.BeforeDuelParameter) null;
      Judgement.BeforeDuelParameter beforeDuelParameter2 = (Judgement.BeforeDuelParameter) null;
      bool flag1 = false;
      bool flag2 = false;
      float percentage_invocation = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation);
      float num1 = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      if (list1.Count > 1)
      {
        BL.ISkillEffectListUnit beAttackUnit1 = (BL.ISkillEffectListUnit) null;
        BL.ISkillEffectListUnit beAttackUnit2 = (BL.ISkillEffectListUnit) null;
        int target_skill_id = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_id1);
        List<BL.ISkillEffectListUnit> list2 = list1.Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => ((IEnumerable<BL.Skill>) x.originalUnit.duelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (duelSkill => duelSkill.id == target_skill_id)) && this.attacker.originalUnit.unit.character.ID != x.originalUnit.unit.character.ID)).ToList<BL.ISkillEffectListUnit>();
        Tuple<int, int> attacker_pos = BattleFuncs.getUnitCell(this.attacker.originalUnit, this.isAI);
        if (list2.Count > 0)
          beAttackUnit1 = list2.OrderBy<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x =>
          {
            int num2 = 0;
            Tuple<int, int> unitCell = BattleFuncs.getUnitCell(x.originalUnit, this.isAI);
            int num3 = BL.fieldDistance(attacker_pos.Item1, attacker_pos.Item2, unitCell.Item1, unitCell.Item2);
            int num4 = unitCell.Item1 - attacker_pos.Item1;
            int num5 = unitCell.Item2 - attacker_pos.Item2;
            int num6 = 0;
            for (int index = 0; index < num3; ++index)
              num6 += 4 * index;
            if (num4 < 0)
              num2 = num6 + num3 * 3 - num5;
            else if (num4 > 0)
              num2 = num6 + num3 + num5;
            else if (num5 > 0)
              num2 = num6 + num3 * 2;
            else if (num5 < 0)
              num2 = num6;
            return num2;
          })).FirstOrDefault<BL.ISkillEffectListUnit>();
        target_skill_id = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_id2);
        List<BL.ISkillEffectListUnit> list3 = list1.Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => ((IEnumerable<BL.Skill>) x.originalUnit.duelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (duelSkill => duelSkill.id == target_skill_id)) && this.attacker.originalUnit.unit.character.ID != x.originalUnit.unit.character.ID)).ToList<BL.ISkillEffectListUnit>();
        if (list3.Count > 0)
          beAttackUnit2 = list3.OrderBy<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x =>
          {
            int num7 = 0;
            Tuple<int, int> unitCell = BattleFuncs.getUnitCell(x.originalUnit, this.isAI);
            int num8 = BL.fieldDistance(attacker_pos.Item1, attacker_pos.Item2, unitCell.Item1, unitCell.Item2);
            int num9 = unitCell.Item1 - attacker_pos.Item1;
            int num10 = unitCell.Item2 - attacker_pos.Item2;
            int num11 = 0;
            for (int index = 0; index < num8; ++index)
              num11 += 4 * index;
            if (num9 < 0)
              num7 = num11 + num8 * 3 - num10;
            else if (num9 > 0)
              num7 = num11 + num8 + num10;
            else if (num10 > 0)
              num7 = num11 + num8 * 2;
            else if (num10 < 0)
              num7 = num11;
            return num7;
          })).FirstOrDefault<BL.ISkillEffectListUnit>();
        if (beAttackUnit1 != null && beAttackUnit2 != null)
        {
          Tuple<int, int> unitCell1 = BattleFuncs.getUnitCell(beAttackUnit1.originalUnit, this.isAI);
          Tuple<int, int> unitCell2 = BattleFuncs.getUnitCell(beAttackUnit2.originalUnit, this.isAI);
          Tuple<int, int> unitCell3 = BattleFuncs.getUnitCell(this.defender.originalUnit, this.isAI);
          BL.MagicBullet beAttackMagicBullet1;
          if (beAttackUnit1.originalUnit.unit.magic_warrior_flag)
          {
            beAttackMagicBullet1 = (BL.MagicBullet) null;
            flag1 = false;
          }
          else
          {
            beAttackMagicBullet1 = ((IEnumerable<BL.MagicBullet>) beAttackUnit1.originalUnit.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x => x.isAttack)).OrderBy<BL.MagicBullet, int>((Func<BL.MagicBullet, int>) (x => x.cost)).FirstOrDefault<BL.MagicBullet>();
            flag1 = beAttackMagicBullet1 != null;
          }
          beforeDuelParameter1 = Judgement.BeforeDuelParameter.CreateDuelSkill(beAttackUnit1, beAttackMagicBullet1, BattleFuncs.getPanel(attacker_pos.Item1, attacker_pos.Item2), this.defender, BattleFuncs.getPanel(unitCell3.Item1, unitCell3.Item2), this.distance, this.currentDefenderHp);
          BL.MagicBullet beAttackMagicBullet2;
          if (beAttackUnit2.originalUnit.unit.magic_warrior_flag)
          {
            beAttackMagicBullet2 = (BL.MagicBullet) null;
            flag2 = false;
          }
          else
          {
            beAttackMagicBullet2 = ((IEnumerable<BL.MagicBullet>) beAttackUnit2.originalUnit.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x => x.isAttack)).OrderBy<BL.MagicBullet, int>((Func<BL.MagicBullet, int>) (x => x.cost)).FirstOrDefault<BL.MagicBullet>();
            flag2 = beAttackMagicBullet2 != null;
          }
          beforeDuelParameter2 = Judgement.BeforeDuelParameter.CreateDuelSkill(beAttackUnit2, beAttackMagicBullet2, BattleFuncs.getPanel(attacker_pos.Item1, attacker_pos.Item2), this.defender, BattleFuncs.getPanel(unitCell3.Item1, unitCell3.Item2), this.distance, this.currentDefenderHp);
          int num12 = BL.fieldDistance(unitCell1.Item1, unitCell1.Item2, attacker_pos.Item1, attacker_pos.Item2);
          int num13 = BL.fieldDistance(unitCell2.Item1, unitCell2.Item2, attacker_pos.Item1, attacker_pos.Item2);
          int num14 = num12 > num13 ? num12 : num13;
          int num15 = Mathf.Max(0, effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) - (num14 - 1));
          percentage_invocation += (float) num15 * effect.GetFloat(BattleskillEffectLogicArgumentEnum.range_add_precentage_invocation);
          num1 += (float) num15 * effect.GetFloat(BattleskillEffectLogicArgumentEnum.range_add_precentage_damage);
        }
      }
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, percentage_invocation, this.currentAttakerHp, this.currentDefenderHp))
        return false;
      if (beforeDuelParameter1 != null && beforeDuelParameter2 != null)
      {
        long num16 = !flag1 ? (long) beforeDuelParameter1.DisplayPhysicalAttack : (long) beforeDuelParameter1.DisplayMagicAttack;
        this.additionalDamage = (float) Judgement.CalcMaximumLongToInt(!flag2 ? num16 + (long) beforeDuelParameter2.DisplayPhysicalAttack : num16 + (long) beforeDuelParameter2.DisplayMagicAttack);
      }
      else
        num1 *= 2f;
      this.damageRate *= num1;
      int skillId = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
      BL.Skill[] skills = new BL.Skill[1]
      {
        new BL.Skill() { id = skillId }
      };
      foreach (BL.ISkillEffectListUnit unit in this.getInvestUnit(this.attacker, this.defender, skillId, 0))
        this.addInvestSkills(unit, skills, new int[1]
        {
          skillId
        }, this.attacker, skill.id, false, false, 0);
      return true;
    }

    private bool funcSnakeVenom(BL.Skill skill, BattleskillEffect effect)
    {
      bool flag = this.isInvokedAmbush ? !this.isAttacker : this.isAttacker;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 1 && !flag || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 2 & flag || !this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      this.damageValue += (float) effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_value);
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.attack_percentage);
      BL.Skill[] skills = new BL.Skill[1]{ skill };
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_hit_only) != 0)
        this.addInvestSkills(this.defender, skills, new int[1]
        {
          skill.id
        }, this.attacker, skill.id, false, false, 0);
      else
        this.addInvestSkills(this.defender, skills, new int[1]
        {
          skill.id
        }, this.attacker, skill.id, false, true, 0);
      return true;
    }

    private bool funcPercentageDamage(BL.Skill skill, BattleskillEffect effect)
    {
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentAttakerHp, this.currentDefenderHp))
        return false;
      float num = effect.GetFloat(BattleskillEffectLogicArgumentEnum.hit_value);
      if ((double) num > 0.0)
        this.FixHit = new float?(num);
      this.PercentageDamageRate = new float?(effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage));
      this.PercentageDamageMax = effect.HasKey(BattleskillEffectLogicArgumentEnum.max_value) ? effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value) : 0;
      return true;
    }

    private bool funcCombiAttack(BL.Skill skill, BattleskillEffect effect)
    {
      if (this.isColossume)
        return false;
      int num1 = effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_count);
      List<BL.ISkillEffectListUnit> list1 = BattleFuncs.getTargets(this.attacker.originalUnit, new int[2]
      {
        1,
        effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
      }, new BL.ForceID[1]
      {
        BattleFuncs.getForceID(this.attacker.originalUnit)
      }, BL.Unit.TargetAttribute.all, (this.isAI ? 1 : 0) != 0, nonFacility: true).Select<BL.UnitPosition, BL.ISkillEffectListUnit>((Func<BL.UnitPosition, BL.ISkillEffectListUnit>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x))).ToList<BL.ISkillEffectListUnit>();
      Judgement.BeforeDuelParameter beforeDuelParameter = (Judgement.BeforeDuelParameter) null;
      bool flag1 = false;
      float percentage_invocation = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation);
      float num2 = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      if (list1.Count > 0)
      {
        int target_skill_id = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_id);
        List<BL.ISkillEffectListUnit> list2 = list1.Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => ((IEnumerable<BL.Skill>) x.originalUnit.duelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (duelSkill => duelSkill.id == target_skill_id)) && this.attacker.originalUnit.unit.character.ID != x.originalUnit.unit.character.ID)).ToList<BL.ISkillEffectListUnit>();
        if (list2.Count > 0)
        {
          Tuple<int, int> pos = BattleFuncs.getUnitCell(this.attacker.originalUnit, this.isAI);
          BL.ISkillEffectListUnit beAttackUnit = list2.OrderBy<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x =>
          {
            int num3 = 0;
            Tuple<int, int> unitCell = BattleFuncs.getUnitCell(x.originalUnit, this.isAI);
            int num4 = BL.fieldDistance(pos.Item1, pos.Item2, unitCell.Item1, unitCell.Item2);
            int num5 = unitCell.Item1 - pos.Item1;
            int num6 = unitCell.Item2 - pos.Item2;
            int num7 = 0;
            for (int index = 0; index < num4; ++index)
              num7 += 4 * index;
            if (num5 < 0)
              num3 = num7 + num4 * 3 - num6;
            else if (num5 > 0)
              num3 = num7 + num4 + num6;
            else if (num6 > 0)
              num3 = num7 + num4 * 2;
            else if (num6 < 0)
              num3 = num7;
            return num3;
          })).FirstOrDefault<BL.ISkillEffectListUnit>();
          if (beAttackUnit != null)
          {
            Tuple<int, int> unitCell1 = BattleFuncs.getUnitCell(this.defender.originalUnit, this.isAI);
            BL.MagicBullet beAttackMagicBullet;
            if (beAttackUnit.originalUnit.unit.magic_warrior_flag)
            {
              beAttackMagicBullet = (BL.MagicBullet) null;
              flag1 = false;
            }
            else
            {
              beAttackMagicBullet = ((IEnumerable<BL.MagicBullet>) beAttackUnit.originalUnit.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x => x.isAttack)).OrderBy<BL.MagicBullet, int>((Func<BL.MagicBullet, int>) (x => x.cost)).FirstOrDefault<BL.MagicBullet>();
              flag1 = beAttackMagicBullet != null;
            }
            beforeDuelParameter = Judgement.BeforeDuelParameter.CreateDuelSkill(beAttackUnit, beAttackMagicBullet, BattleFuncs.getPanel(pos.Item1, pos.Item2), this.defender, BattleFuncs.getPanel(unitCell1.Item1, unitCell1.Item2), this.distance, this.currentDefenderHp);
            Tuple<int, int> unitCell2 = BattleFuncs.getUnitCell(beAttackUnit.originalUnit, this.isAI);
            int num8 = BL.fieldDistance(pos.Item1, pos.Item2, unitCell2.Item1, unitCell2.Item2);
            int num9 = Mathf.Max(0, effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) - (num8 - 1));
            percentage_invocation += (float) num9 * effect.GetFloat(BattleskillEffectLogicArgumentEnum.range_add_precentage_invocation);
            num2 += (float) num9 * effect.GetFloat(BattleskillEffectLogicArgumentEnum.range_add_precentage_damage);
          }
        }
      }
      if (!this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, skill.level, percentage_invocation, this.currentAttakerHp, this.currentDefenderHp))
        return false;
      if (beforeDuelParameter != null)
        this.additionalDamage = !flag1 ? (float) beforeDuelParameter.DisplayPhysicalAttack : (float) beforeDuelParameter.DisplayMagicAttack;
      else
        num2 *= 1.5f;
      this.damageRate *= num2;
      this.attackCount = num1;
      float num10 = effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_drain);
      if ((double) this.drainRate < (double) num10)
        this.drainRate = num10;
      float num11 = effect.GetFloat(BattleskillEffectLogicArgumentEnum.hit_value);
      if ((double) num11 > 0.0)
        this.FixHit = new float?(num11);
      int num12 = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
      if (num12 != 0 && MasterData.BattleskillSkill.ContainsKey(num12))
      {
        if (MasterData.BattleskillSkill[num12].skill_type == BattleskillSkillType.ailment)
        {
          List<BL.SkillEffect> useResistEffects;
          bool flag2 = BattleFuncs.isAilmentInvest(1f, num12, this.defender, this.attacker, this.random, this.colosseumTurn, out useResistEffects, this.turnHp, new int?(this.getUnitCurrentHp(this.defender)), new int?(this.getUnitCurrentHp(this.attacker)));
          this.addInvestSkills(this.defender, BattleFuncs.ailmentInvest(num12, this.defender), new int[1]
          {
            num12
          }, this.attacker, skill.id, true, false, 0, isSuccess: (flag2 ? 1 : 0) != 0, useResistEffects: useResistEffects);
        }
        else
        {
          BL.Skill[] skills = new BL.Skill[1]
          {
            new BL.Skill() { id = num12 }
          };
          foreach (BL.ISkillEffectListUnit unit in this.getInvestUnit(this.attacker, this.defender, num12, 0))
            this.addInvestSkills(unit, skills, new int[1]
            {
              num12
            }, this.attacker, skill.id, false, false, 0);
        }
      }
      return true;
    }

    private bool funcShield(BL.Skill skill, BattleskillEffect effect)
    {
      if (!BattleFuncs.isGearEquipped(this.attacker.originalUnit.playerUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id)) || !this.isInvoke(this.defender, this.attacker, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus.duelParameter.attackerUnitParameter, this.defenseStatus, this.attackStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentDefenderHp, this.currentAttakerHp))
        return false;
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      return true;
    }

    private bool funcOutOfRangeDefence(BL.Skill skill, BattleskillEffect effect)
    {
      if (this.distance == 0 || this.isDefenderInRange() || !this.isInvoke(this.defender, this.attacker, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus.duelParameter.attackerUnitParameter, this.defenseStatus, this.attackStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentDefenderHp, this.currentAttakerHp))
        return false;
      this.damageRate *= effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_decrease);
      return true;
    }

    private bool funcCounter(BL.Skill skill, BattleskillEffect effect)
    {
      if (!BattleFuncs.isGearEquipped(this.attacker.originalUnit.playerUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id)) || !NC.IsReach(effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range), effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range), this.distance))
        return false;
      this.isInvokeCounterAttack = true;
      this.counterDamageRate = effect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_ratio);
      this.counterAttackRate = effect.GetFloat(BattleskillEffectLogicArgumentEnum.counter_attack_ratio);
      return true;
    }

    private bool funcPrayer(BL.Skill skill, BattleskillEffect effect)
    {
      if (this.defenderSkills.Length >= 1 || this.biAttackDuelSkill.defenderSkills.Length >= 1 || this.isInvokedPrayer || this.defenderDuelBeginHp <= 1 || this.finalAttack < this.currentDefenderHp || effect.HasKey(BattleskillEffectLogicArgumentEnum.invoke_gamemode) && !BattleFuncs.checkInvokeGamemode(effect.GetInt(BattleskillEffectLogicArgumentEnum.invoke_gamemode), this.isColossume, this.defender))
        return false;
      if (this.distance != 0 && effect.HasKey(BattleskillEffectLogicArgumentEnum.out_of_range))
      {
        int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.out_of_range);
        if (num != 0)
        {
          bool flag = this.isDefenderInRange();
          if (num == 1 & flag || num == 2 && !flag)
            return false;
        }
      }
      return !BattleFuncs.checkInvalidEffect(effect, this.attackerInvalidSkillLogics) && this.isInvoke(this.defender, this.attacker, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus.duelParameter.attackerUnitParameter, this.defenseStatus, this.attackStatus, skill.level, effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invocation), this.currentDefenderHp, this.currentAttakerHp, effect);
    }

    private bool funcInvestSkillEffect(BL.Skill skill, BattleskillEffect effect, bool isAI)
    {
      if (this.isHit)
      {
        double lottery = (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest) + (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_invest_levelup) * (double) skill.level;
        int skill_id = effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
        int skillID = skill_id;
        BL.ISkillEffectListUnit defender = this.defender;
        BL.ISkillEffectListUnit attacker = this.attacker;
        XorShift random = this.random;
        int? colosseumTurn = this.colosseumTurn;
        List<BL.SkillEffect> useResistEffects;
        ref List<BL.SkillEffect> local = ref useResistEffects;
        TurnHp turnHp = this.turnHp;
        int? targetHp = new int?(this.getUnitCurrentHp(this.defender));
        int? unitHp = new int?(this.getUnitCurrentHp(this.attacker));
        bool flag = BattleFuncs.isAilmentInvest((float) lottery, skillID, defender, attacker, random, colosseumTurn, out local, turnHp, targetHp, unitHp);
        this.addInvestSkills(this.defender, BattleFuncs.ailmentInvest(skill_id, this.defender), new int[1]
        {
          skill_id
        }, this.attacker, skill.id, true, false, 0, isSuccess: (flag ? 1 : 0) != 0, useResistEffects: useResistEffects);
      }
      return true;
    }

    private bool isInvoke(
      BL.ISkillEffectListUnit invoke,
      BL.ISkillEffectListUnit target,
      Judgement.BeforeDuelUnitParameter invokeParameter,
      Judgement.BeforeDuelUnitParameter targetParameter,
      AttackStatus invokeAS,
      AttackStatus targetAS,
      int skill_level,
      float percentage_invocation,
      int invokeHp,
      int targetHp,
      BattleskillEffect effect = null,
      float base_invocation = 0.0f,
      float invocation_skill_ratio = 1f,
      float invocation_luck_ratio = 1f,
      List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics = null)
    {
      BattleFuncs.InvokeLotteryInfo info;
      int num = BattleFuncs.isInvoke(out info, invoke, target, invokeParameter, targetParameter, invokeAS, targetAS, skill_level, percentage_invocation, this.random, true, invokeHp, targetHp, this.colosseumTurn, this.invokeRate, effect, base_invocation, invocation_skill_ratio, invocation_luck_ratio, invalidSkillLogics) ? 1 : 0;
      this.lotteryInfos.Add(info);
      return num != 0 || this.isSimulate;
    }

    private bool CheckInvokeGeneric(
      BattleDuelSkill.InvokeGenericWork work,
      BL.ISkillEffectListUnit invokeUnit,
      List<Tuple<int, int>> suppressSkill)
    {
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.invoke_gamemode) && !BattleFuncs.checkInvokeGamemode(work.GetInt(BattleskillEffectLogicArgumentEnum.invoke_gamemode), this.isColossume, invokeUnit) || work.HasKey(BattleskillEffectLogicArgumentEnum.attacker_gear_kind_id) && !BattleFuncs.isGearEquipped(this.attacker.originalUnit.playerUnit, work.GetInt(BattleskillEffectLogicArgumentEnum.attacker_gear_kind_id)) || work.HasKey(BattleskillEffectLogicArgumentEnum.defender_gear_kind_id) && !BattleFuncs.isGearEquipped(this.defender.originalUnit.playerUnit, work.GetInt(BattleskillEffectLogicArgumentEnum.defender_gear_kind_id)))
        return false;
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.attacker_base_gear_kind_id))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.attacker_base_gear_kind_id);
        if (num != 0 && num != this.attacker.originalUnit.unit.kind.ID)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.defender_base_gear_kind_id))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.defender_base_gear_kind_id);
        if (num != 0 && num != this.defender.originalUnit.unit.kind.ID)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.attacker_element))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.attacker_element);
        if (num != 0 && (CommonElement) num != this.attacker.originalUnit.playerUnit.GetElement())
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.defender_element))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.defender_element);
        if (num != 0 && (CommonElement) num != this.defender.originalUnit.playerUnit.GetElement())
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.attacker_job_id))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.attacker_job_id);
        if (num != 0 && num != this.attacker.originalUnit.job.ID)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.defender_job_id))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.defender_job_id);
        if (num != 0 && num != this.defender.originalUnit.job.ID)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.attacker_family_id))
      {
        int family = work.GetInt(BattleskillEffectLogicArgumentEnum.attacker_family_id);
        if (family != 0 && !this.attacker.originalUnit.playerUnit.HasFamily((UnitFamily) family))
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.defender_family_id))
      {
        int family = work.GetInt(BattleskillEffectLogicArgumentEnum.defender_family_id);
        if (family != 0 && !this.defender.originalUnit.playerUnit.HasFamily((UnitFamily) family))
          return false;
      }
      if (this.distance != 0)
      {
        if (work.HasKey(BattleskillEffectLogicArgumentEnum.min_range))
        {
          int num = work.GetInt(BattleskillEffectLogicArgumentEnum.min_range);
          if (num != 0 && this.distance < num)
            return false;
        }
        if (work.HasKey(BattleskillEffectLogicArgumentEnum.max_range))
        {
          int num = work.GetInt(BattleskillEffectLogicArgumentEnum.max_range);
          if (num != 0 && this.distance > num)
            return false;
        }
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.attack_phase))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.attack_phase);
        if (num == 1 && this.isOneMoreAttack || num == 2 && !this.isOneMoreAttack)
          return false;
      }
      BL.ISkillEffectListUnit[] skillEffectListUnitArray = new BL.ISkillEffectListUnit[2]
      {
        this.attacker,
        this.defender
      };
      string[] strArray1 = new string[2]
      {
        "attacker",
        "defender"
      };
      int[] numArray = new int[2]
      {
        this.currentAttakerHp,
        this.currentDefenderHp
      };
      string[] strArray2 = new string[4]
      {
        "gt",
        "lt",
        "ge",
        "le"
      };
      Func<int, int, bool>[] funcArray = new Func<int, int, bool>[4]
      {
        (Func<int, int, bool>) ((hp, border) => hp > border),
        (Func<int, int, bool>) ((hp, border) => hp < border),
        (Func<int, int, bool>) ((hp, border) => hp >= border),
        (Func<int, int, bool>) ((hp, border) => hp <= border)
      };
      for (int index1 = 0; index1 < 2; ++index1)
      {
        string str = strArray1[index1] + "_hp";
        for (int index2 = 0; index2 < 4; ++index2)
        {
          BattleskillEffectLogicArgumentEnum logicArgumentEnum1 = BattleskillEffectLogic.GetLogicArgumentEnum(str + "_percentage_" + strArray2[index2]);
          if (work.HasKey(logicArgumentEnum1))
          {
            float num = work.GetFloat(logicArgumentEnum1);
            if ((double) num != 0.0 && !funcArray[index2](numArray[index1], Mathf.CeilToInt((float) skillEffectListUnitArray[index1].originalUnit.parameter.Hp * num)))
              return false;
          }
          BattleskillEffectLogicArgumentEnum logicArgumentEnum2 = BattleskillEffectLogic.GetLogicArgumentEnum(str + "_value_" + strArray2[index2]);
          if (work.HasKey(logicArgumentEnum2))
          {
            int num = work.GetInt(logicArgumentEnum2);
            if (num != 0 && !funcArray[index2](numArray[index1], num))
              return false;
          }
        }
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.use_count))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.use_count);
        if (num != 0 && invokeUnit.skillEffects.GetDuelSkillEffectIdInvokeCount(work.effects[0].ID) >= num)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.start_total_count))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.start_total_count);
        if (num != 0 && invokeUnit.skillEffects.GetDuelSkillIdInvokeCount(work.skill.id) < num)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.end_total_count))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.end_total_count);
        if (num != 0 && invokeUnit.skillEffects.GetDuelSkillIdInvokeCount(work.skill.id) >= num)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.start_total_count2))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.start_total_count2);
        if (num != 0 && invokeUnit.skillEffects.GetDuelSkillIdInvokeCount2(work.skill.id) < num)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.end_total_count2))
      {
        int num = work.GetInt(BattleskillEffectLogicArgumentEnum.end_total_count2);
        if (num != 0 && invokeUnit.skillEffects.GetDuelSkillIdInvokeCount2(work.skill.id) >= num)
          return false;
      }
      int num1 = this.isColossume ? this.colosseumTurn.Value : BattleFuncs.getPhaseState().absoluteTurnCount;
      int num2 = 0;
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.start_turn))
      {
        num2 = work.GetInt(BattleskillEffectLogicArgumentEnum.start_turn);
        if (num2 != 0 && num1 < num2)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.end_turn))
      {
        int num3 = work.GetInt(BattleskillEffectLogicArgumentEnum.end_turn);
        if (num3 != 0 && num1 >= num3)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.turn_cycle))
      {
        int num4 = work.GetInt(BattleskillEffectLogicArgumentEnum.turn_cycle);
        if (num4 != 0 && (num1 - num2) % num4 != 0)
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.attacker_skill_group_id))
      {
        int skillGroupId = work.GetInt(BattleskillEffectLogicArgumentEnum.attacker_skill_group_id);
        if (skillGroupId != 0 && !this.attacker.originalUnit.unit.HasSkillGroupId(skillGroupId))
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.attacker_exclude_skill_group_id))
      {
        int skillGroupId = work.GetInt(BattleskillEffectLogicArgumentEnum.attacker_exclude_skill_group_id);
        if (skillGroupId != 0 && this.attacker.originalUnit.unit.HasSkillGroupId(skillGroupId))
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.defender_skill_group_id))
      {
        int skillGroupId = work.GetInt(BattleskillEffectLogicArgumentEnum.defender_skill_group_id);
        if (skillGroupId != 0 && !this.defender.originalUnit.unit.HasSkillGroupId(skillGroupId))
          return false;
      }
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.defender_exclude_skill_group_id))
      {
        int skillGroupId = work.GetInt(BattleskillEffectLogicArgumentEnum.defender_exclude_skill_group_id);
        if (skillGroupId != 0 && this.defender.originalUnit.unit.HasSkillGroupId(skillGroupId))
          return false;
      }
      if (suppressSkill.Any<Tuple<int, int>>((Func<Tuple<int, int>, bool>) (x =>
      {
        int num5 = x.Item1;
        int logicId = x.Item2;
        if (num5 != 0 && num5 != work.skill.id)
          return false;
        return logicId == 0 || work.effects.Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (y => logicId == y.EffectLogic.ID));
      })))
        return false;
      if (work.HasKey(BattleskillEffectLogicArgumentEnum.attacker_attack_type))
      {
        int num6 = work.GetInt(BattleskillEffectLogicArgumentEnum.attacker_attack_type);
        if (num6 == 1 && (this.attackStatus == null || this.attackStatus.isMagic) || num6 == 2 && (this.attackStatus == null || !this.attackStatus.isMagic))
          return false;
      }
      Func<BL.Panel, BattleskillEffectLogicArgumentEnum, bool, bool> func = (Func<BL.Panel, BattleskillEffectLogicArgumentEnum, bool, bool>) ((panel, logic, isExist) =>
      {
        if (!work.HasKey(logic))
          return true;
        if (panel == null)
          return false;
        BattleLandform landform = panel.landform;
        if (landform == null)
          return false;
        for (int index = 1; index <= 10; ++index)
        {
          int tag = work.GetInt(logic);
          if (tag == 0)
            return !isExist;
          if (landform.HasTag(tag))
            return isExist;
          if (panel.hasLandTag((this.isAI ? 1 : 0) != 0, tag))
            return isExist;
          ++logic;
        }
        return !isExist;
      });
      return func(this.attackPanel, BattleskillEffectLogicArgumentEnum.attacker_land_tag1, true) && func(this.defensePanel, BattleskillEffectLogicArgumentEnum.defender_land_tag1, true) && func(this.attackPanel, BattleskillEffectLogicArgumentEnum.attacker_exclude_land_tag1, false) && func(this.defensePanel, BattleskillEffectLogicArgumentEnum.defender_exclude_land_tag1, false);
    }

    private List<List<BattleskillEffect>> CreateEffectPack(
      BL.Skill skill,
      BattleskillEffectLogicArgumentEnum borderKey)
    {
      List<List<BattleskillEffect>> effectPack = new List<List<BattleskillEffect>>();
      List<BattleskillEffect> battleskillEffectList = (List<BattleskillEffect>) null;
      for (int index = 0; index < skill.skill.Effects.Length; ++index)
      {
        BattleskillEffect effect = skill.skill.Effects[index];
        if (effect.HasKey(borderKey))
        {
          if (effect.checkLevel(skill.level))
          {
            battleskillEffectList = new List<BattleskillEffect>();
            effectPack.Add(battleskillEffectList);
          }
          else
            battleskillEffectList = (List<BattleskillEffect>) null;
        }
        battleskillEffectList?.Add(effect);
      }
      return effectPack;
    }

    private void funcGenericSkillInvest(
      BattleDuelSkill.InvokeGenericWork work,
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      int attackNo,
      BL.ISkillEffectListUnit[] combiUnits = null)
    {
      bool flag1 = true;
      int rangeType = work.HasKey(BattleskillEffectLogicArgumentEnum.skill_range_type) ? work.GetInt(BattleskillEffectLogicArgumentEnum.skill_range_type) : 0;
      List<BL.ISkillEffectListUnit> second = new List<BL.ISkillEffectListUnit>();
      int num1 = 0;
      if (combiUnits != null && work.HasKey(BattleskillEffectLogicArgumentEnum.skill_invest_combi))
      {
        num1 = work.GetInt(BattleskillEffectLogicArgumentEnum.skill_invest_combi);
        if (num1 != 0)
        {
          for (int index = 0; index < combiUnits.Length; ++index)
          {
            if (combiUnits[index] != null)
              second.Add(combiUnits[index]);
          }
        }
      }
      int num2 = 1;
      while (true)
      {
        BattleskillEffectLogicArgumentEnum logicArgumentEnum1 = BattleskillEffectLogic.GetLogicArgumentEnum("skill_id" + (object) num2);
        if (logicArgumentEnum1 != BattleskillEffectLogicArgumentEnum.none && work.HasKey(logicArgumentEnum1))
        {
          int num3 = work.GetInt(logicArgumentEnum1);
          bool flag2;
          if (num3 < 0)
          {
            flag2 = true;
            num3 = -num3;
          }
          else
            flag2 = false;
          if (num3 != 0 && MasterData.BattleskillSkill.ContainsKey(num3))
          {
            float lottery = 1f;
            BattleskillEffectLogicArgumentEnum logicArgumentEnum2 = BattleskillEffectLogic.GetLogicArgumentEnum("skill_percentage_invest" + (object) num2);
            bool flag3 = work.HasKey(logicArgumentEnum2);
            if (flag3)
            {
              float num4 = work.GetFloat(logicArgumentEnum2);
              BattleskillEffectLogicArgumentEnum logicArgumentEnum3 = BattleskillEffectLogic.GetLogicArgumentEnum("skill_percentage_invest_levelup" + (object) num2);
              if ((double) num4 != 0.0)
                lottery = num4 + (work.HasKey(logicArgumentEnum3) ? work.GetFloat(logicArgumentEnum3) : 0.0f) * (float) work.skill.level;
              else
                flag3 = false;
            }
            IEnumerable<BL.ISkillEffectListUnit> skillEffectListUnits;
            switch (num1)
            {
              case 1:
                skillEffectListUnits = this.getInvestUnit(myself, enemy, num3, rangeType).Union<BL.ISkillEffectListUnit>((IEnumerable<BL.ISkillEffectListUnit>) second);
                break;
              case 2:
                skillEffectListUnits = (IEnumerable<BL.ISkillEffectListUnit>) second;
                break;
              default:
                skillEffectListUnits = this.getInvestUnit(myself, enemy, num3, rangeType);
                break;
            }
            BattleskillEffectLogicArgumentEnum logicArgumentEnum4 = BattleskillEffectLogic.GetLogicArgumentEnum("skill_once_invest" + (object) num2);
            int onceInvestFlag = work.HasKey(logicArgumentEnum4) ? work.GetInt(logicArgumentEnum4) : 0;
            if (MasterData.BattleskillSkill[num3].skill_type == BattleskillSkillType.ailment)
            {
              foreach (BL.ISkillEffectListUnit skillEffectListUnit in skillEffectListUnits)
              {
                List<BL.SkillEffect> useResistEffects;
                bool flag4 = BattleFuncs.isAilmentInvest(lottery, num3, skillEffectListUnit, myself, this.random, this.colosseumTurn, out useResistEffects, this.turnHp, new int?(this.getUnitCurrentHp(skillEffectListUnit)), new int?(this.getUnitCurrentHp(myself)));
                this.addInvestSkills(skillEffectListUnit, BattleFuncs.ailmentInvest(num3, skillEffectListUnit), new int[1]
                {
                  num3
                }, myself, work.skill.id, true, (flag2 ? 1 : 0) != 0, attackNo, onceInvestFlag, (flag4 ? 1 : 0) != 0, useResistEffects);
              }
            }
            else
            {
              bool flag5 = flag1;
              flag1 = false;
              foreach (BL.ISkillEffectListUnit unit in skillEffectListUnits)
              {
                bool flag6 = !flag3 ? flag5 : (double) lottery >= (double) this.random.NextFloat();
                flag1 |= flag6;
                if (flag6)
                  this.addInvestSkills(unit, new BL.Skill[1]
                  {
                    new BL.Skill() { id = num3, level = 1 }
                  }, new int[1]{ num3 }, myself, work.skill.id, false, (flag2 ? 1 : 0) != 0, attackNo, onceInvestFlag);
              }
            }
          }
          ++num2;
        }
        else
          break;
      }
    }

    private List<Tuple<int, int>> GetSuppressSkill(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit target,
      BL.Panel myselfPanel)
    {
      return myself.skillEffects.Where(BattleskillEffectLogicEnum.suppress_duel_skill, (Func<BL.SkillEffect, bool>) (x =>
      {
        BattleskillEffect effect = x.effect;
        return (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == myself.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == myself.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || myself.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || myself.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !myself.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id))) && x.effect.GetPackedSkillEffect().CheckLandTag(myselfPanel, this.isAI);
      })).Select<BL.SkillEffect, Tuple<int, int>>((Func<BL.SkillEffect, Tuple<int, int>>) (x => new Tuple<int, int>(x.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id), x.effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id)))).ToList<Tuple<int, int>>();
    }

    private BL.Skill[] InvokeAttackerGeneric()
    {
      Dictionary<BattleskillEffectLogicArgumentEnum, Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>> dictionary = new Dictionary<BattleskillEffectLogicArgumentEnum, Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>>()
      {
        {
          BattleskillEffectLogicArgumentEnum.change_attack_type,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericChangeAttackType)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_damage,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPercentageDamage)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_damage_s1,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericBiAttackPercentageDamage)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPercentageAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.base_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericBaseAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_drain,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPercentageDrain)
        },
        {
          BattleskillEffectLogicArgumentEnum.hit_value,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericHitValue)
        },
        {
          BattleskillEffectLogicArgumentEnum.critical_rate,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericCriticalRate)
        },
        {
          BattleskillEffectLogicArgumentEnum.skill_id1,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericSkillId)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_decrease,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPercentageDecrease)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_magic_decrease,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPercentageMagicDecrease)
        },
        {
          BattleskillEffectLogicArgumentEnum.intelligence_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericIntelligencePhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.agility_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericAgilityPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.dexterity_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericDexterityPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.luck_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericLuckPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.strength_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericStrengthMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.agility_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericAgilityMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.dexterity_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericDexterityMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.luck_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericLuckMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.instant_death,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericInstantDeath)
        },
        {
          BattleskillEffectLogicArgumentEnum.revenge,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericRevenge)
        },
        {
          BattleskillEffectLogicArgumentEnum.rate_damage,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericRateDamage)
        },
        {
          BattleskillEffectLogicArgumentEnum.combi_target_skill_id1,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericCombiTargetSkillId)
        },
        {
          BattleskillEffectLogicArgumentEnum.counter_damage_hp_percentage,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericCounterDamageHpPercentage)
        },
        {
          BattleskillEffectLogicArgumentEnum.counter_damage_value,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericCounterDamageValue)
        },
        {
          BattleskillEffectLogicArgumentEnum.hp_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericHpPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.max_hp_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericMaxHpPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.strength_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericStrengthPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.vitality_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericVitalityPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.mind_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericMindPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.hit_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericHitPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.evasion_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericEvasionPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.critical_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericCriticalPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.physical_attack_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPhysicalAttackPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.magic_attack_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericMagicAttackPhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.physical_defense_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPhysicalDefensePhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.magic_defense_physical_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericMagicDefensePhysicalAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.hp_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericHpMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.max_hp_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericMaxHpMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.intelligence_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericIntelligenceMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.vitality_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericVitalityMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.mind_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericMindMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.hit_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericHitMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.evasion_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericEvasionMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.critical_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericCriticalMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.physical_attack_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPhysicalAttackMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.magic_attack_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericMagicAttackMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.physical_defense_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericPhysicalDefenseMagicAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.magic_defense_magic_attack,
          new Func<BattleDuelSkill.InvokeAttackerGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcGenericMagicDefenseMagicAttack)
        }
      };
      List<Tuple<int, int>> suppressSkill = (List<Tuple<int, int>>) null;
      foreach (BL.Skill duelSkill1 in this.attacker.originalUnit.duelSkills)
      {
        if (((IEnumerable<BattleskillEffect>) duelSkill1.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(BattleskillEffectLogicArgumentEnum.gda_percentage_invocation))))
        {
          this.lotteryInfos.Clear();
          if (this.isInvalidAttackDuelSkill)
          {
            BattleskillGenre? genre1 = duelSkill1.genre1;
            BattleskillGenre battleskillGenre = BattleskillGenre.attack;
            if (genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue)
              continue;
          }
          if (!this.attacker.IsDontUseSkill(duelSkill1.id) && this.attacker.checkEnableSkill(duelSkill1.skill))
          {
            HashSet<int> intSet = new HashSet<int>();
            foreach (List<BattleskillEffect> collection in this.CreateEffectPack(duelSkill1, BattleskillEffectLogicArgumentEnum.gda_percentage_invocation))
            {
              BattleDuelSkill.InvokeAttackerGenericWork attackerGenericWork = new BattleDuelSkill.InvokeAttackerGenericWork();
              attackerGenericWork.skill = duelSkill1;
              attackerGenericWork.effects = collection;
              BattleDuelSkill.InvokeAttackerGenericWork work = attackerGenericWork;
              int num1 = work.HasKey(BattleskillEffectLogicArgumentEnum.duel_skill_effect_group) ? work.GetInt(BattleskillEffectLogicArgumentEnum.duel_skill_effect_group) : 0;
              if (num1 == 0 || !intSet.Contains(num1))
              {
                int num2 = 1;
                if (work.HasKey(BattleskillEffectLogicArgumentEnum.attack_count))
                  num2 = work.GetInt(BattleskillEffectLogicArgumentEnum.attack_count);
                if (suppressSkill == null)
                  suppressSkill = this.GetSuppressSkill(this.defender, this.attacker, this.defensePanel);
                if (this.CheckInvokeGeneric((BattleDuelSkill.InvokeGenericWork) work, this.attacker, suppressSkill))
                {
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.is_attack))
                  {
                    bool flag = this.isInvokedAmbush ? !this.isAttacker : this.isAttacker;
                    int num3 = work.GetInt(BattleskillEffectLogicArgumentEnum.is_attack);
                    if (num3 == 1 && !flag || num3 == 2 & flag)
                      continue;
                  }
                  float percentage_invocation = work.GetFloat(BattleskillEffectLogicArgumentEnum.gda_percentage_invocation);
                  work.combiUnits = (BL.ISkillEffectListUnit[]) null;
                  if (!this.isColossume)
                  {
                    int length = 0;
                    List<int> targetSkillIds = new List<int>();
                    while (true)
                    {
                      BattleskillEffectLogicArgumentEnum logicArgumentEnum = BattleskillEffectLogic.GetLogicArgumentEnum("combi_target_skill_id" + (object) (length + 1));
                      if (work.HasKey(logicArgumentEnum) && work.GetInt(logicArgumentEnum) != 0)
                      {
                        targetSkillIds.Add(work.GetInt(logicArgumentEnum));
                        ++length;
                      }
                      else
                        break;
                    }
                    if (length >= 1)
                    {
                      BL.ForceID forceId = BattleFuncs.getForceID(this.attacker.originalUnit);
                      if ((work.HasKey(BattleskillEffectLogicArgumentEnum.combi_not_must_partake) ? work.GetInt(BattleskillEffectLogicArgumentEnum.combi_not_must_partake) : 1) == 0)
                      {
                        List<BL.Unit> source = BattleFuncs.forceUnits(forceId).value;
                        int no = 0;
                        while (no < length && source.Any<BL.Unit>((Func<BL.Unit, bool>) (x => ((IEnumerable<BL.Skill>) x.originalUnit.duelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (duelSkill => duelSkill.id == targetSkillIds[no])) && this.attacker.originalUnit.unit.character.ID != x.originalUnit.unit.character.ID)))
                          no++;
                        if (no < length)
                          continue;
                      }
                      int num4 = 1;
                      if (work.HasKey(BattleskillEffectLogicArgumentEnum.combi_min_range))
                        num4 = work.GetInt(BattleskillEffectLogicArgumentEnum.combi_min_range);
                      int[] range = new int[2]
                      {
                        num4,
                        work.GetInt(BattleskillEffectLogicArgumentEnum.combi_max_range)
                      };
                      List<BL.ISkillEffectListUnit> list1 = BattleFuncs.getTargets(this.attacker.originalUnit, range, new BL.ForceID[1]
                      {
                        forceId
                      }, BL.Unit.TargetAttribute.all, (this.isAI ? 1 : 0) != 0, nonFacility: true).Select<BL.UnitPosition, BL.ISkillEffectListUnit>((Func<BL.UnitPosition, BL.ISkillEffectListUnit>) (x => BattleFuncs.unitPositionToISkillEffectListUnit(x))).ToList<BL.ISkillEffectListUnit>();
                      work.duelParam = new Judgement.BeforeDuelParameter[length];
                      work.isMagic = new bool[length];
                      work.rangeAddDamage = 0.0f;
                      work.combiUnits = new BL.ISkillEffectListUnit[length];
                      int num5 = int.MinValue;
                      BL.ISkillEffectListUnit skillEffectListUnit1 = (BL.ISkillEffectListUnit) null;
                      if (list1.Count >= length)
                      {
                        Tuple<int, int> pos = BattleFuncs.getUnitCell(this.attacker.originalUnit, this.isAI);
                        Tuple<int, int> epos = BattleFuncs.getUnitCell(this.defender.originalUnit, this.isAI);
                        BL.ISkillEffectListUnit[] skillEffectListUnitArray = new BL.ISkillEffectListUnit[length];
                        int combi_cross_type = work.HasKey(BattleskillEffectLogicArgumentEnum.combi_cross_type) ? work.GetInt(BattleskillEffectLogicArgumentEnum.combi_cross_type) : 0;
                        for (int index1 = 0; index1 < length; ++index1)
                        {
                          int target_skill_id = targetSkillIds[index1];
                          List<BL.ISkillEffectListUnit> list2 = list1.Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x => ((IEnumerable<BL.Skill>) x.originalUnit.duelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (duelSkill => duelSkill.id == target_skill_id)) && this.attacker.originalUnit.unit.character.ID != x.originalUnit.unit.character.ID)).Where<BL.ISkillEffectListUnit>((Func<BL.ISkillEffectListUnit, bool>) (x =>
                          {
                            if (combi_cross_type == 0)
                              return true;
                            Tuple<int, int> unitCell = BattleFuncs.getUnitCell(x.originalUnit, this.isAI);
                            if (epos.Item1 == pos.Item1 && epos.Item2 == pos.Item2 || epos.Item1 == unitCell.Item1 && epos.Item2 == unitCell.Item2 || pos.Item1 == unitCell.Item1 && pos.Item2 == unitCell.Item2)
                              return false;
                            if (combi_cross_type == 1)
                            {
                              if ((pos.Item1 != unitCell.Item1 || epos.Item1 != pos.Item1 || epos.Item1 != unitCell.Item1 || pos.Item2 < epos.Item2 && unitCell.Item2 < epos.Item2 || pos.Item2 > epos.Item2 && unitCell.Item2 > epos.Item2) && (pos.Item2 != unitCell.Item2 || epos.Item2 != pos.Item2 || epos.Item2 != unitCell.Item2 || pos.Item1 < epos.Item1 && unitCell.Item1 < epos.Item1 || pos.Item1 > epos.Item1 && unitCell.Item1 > epos.Item1))
                                return false;
                            }
                            else if (combi_cross_type == 2 && (epos.Item1 != pos.Item1 || epos.Item2 != unitCell.Item2) && (epos.Item2 != pos.Item2 || epos.Item1 != unitCell.Item1))
                              return false;
                            return true;
                          })).ToList<BL.ISkillEffectListUnit>();
                          if (list2.Count > 0)
                          {
                            BL.ISkillEffectListUnit skillEffectListUnit2 = list2.OrderBy<BL.ISkillEffectListUnit, int>((Func<BL.ISkillEffectListUnit, int>) (x =>
                            {
                              int num6 = 0;
                              Tuple<int, int> unitCell = BattleFuncs.getUnitCell(x.originalUnit, this.isAI);
                              int num7 = BL.fieldDistance(pos.Item1, pos.Item2, unitCell.Item1, unitCell.Item2);
                              int num8 = unitCell.Item1 - pos.Item1;
                              int num9 = unitCell.Item2 - pos.Item2;
                              int num10 = 0;
                              for (int index2 = 0; index2 < num7; ++index2)
                                num10 += 4 * index2;
                              if (num8 < 0)
                                num6 = num10 + num7 * 3 - num9;
                              else if (num8 > 0)
                                num6 = num10 + num7 + num9;
                              else if (num9 > 0)
                                num6 = num10 + num7 * 2;
                              else if (num9 < 0)
                                num6 = num10;
                              return num6;
                            })).FirstOrDefault<BL.ISkillEffectListUnit>();
                            if (skillEffectListUnit2 != null)
                              skillEffectListUnitArray[index1] = skillEffectListUnit2;
                            else
                              break;
                          }
                          if (skillEffectListUnitArray[index1] == null)
                            break;
                        }
                        if (skillEffectListUnitArray[length - 1] != null)
                        {
                          for (int index = 0; index < length; ++index)
                          {
                            BL.MagicBullet beAttackMagicBullet;
                            if (skillEffectListUnitArray[index].originalUnit.unit.magic_warrior_flag)
                            {
                              beAttackMagicBullet = (BL.MagicBullet) null;
                              work.isMagic[index] = false;
                            }
                            else
                            {
                              beAttackMagicBullet = ((IEnumerable<BL.MagicBullet>) skillEffectListUnitArray[index].originalUnit.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x => x.isAttack)).OrderBy<BL.MagicBullet, int>((Func<BL.MagicBullet, int>) (x => x.cost)).FirstOrDefault<BL.MagicBullet>();
                              work.isMagic[index] = beAttackMagicBullet != null;
                            }
                            work.duelParam[index] = Judgement.BeforeDuelParameter.CreateDuelSkill(skillEffectListUnitArray[index], beAttackMagicBullet, BattleFuncs.getPanel(pos.Item1, pos.Item2), this.defender, BattleFuncs.getPanel(epos.Item1, epos.Item2), this.distance, this.currentDefenderHp);
                            Tuple<int, int> unitCell = BattleFuncs.getUnitCell(skillEffectListUnitArray[index].originalUnit, this.isAI);
                            int num11 = BL.fieldDistance(pos.Item1, pos.Item2, unitCell.Item1, unitCell.Item2);
                            if (num11 > num5)
                            {
                              num5 = num11;
                              skillEffectListUnit1 = skillEffectListUnitArray[index];
                            }
                            work.combiUnits[index] = skillEffectListUnitArray[index];
                          }
                          int num12 = Mathf.Max(0, range[1] - (num5 - 1));
                          if (work.HasKey(BattleskillEffectLogicArgumentEnum.combi_range_add_percentage_invocation))
                            percentage_invocation += (float) num12 * work.GetFloat(BattleskillEffectLogicArgumentEnum.combi_range_add_percentage_invocation);
                          if (work.HasKey(BattleskillEffectLogicArgumentEnum.combi_range_add_percentage_damage))
                            work.rangeAddDamage = (float) num12 * work.GetFloat(BattleskillEffectLogicArgumentEnum.combi_range_add_percentage_damage);
                        }
                      }
                      if (!work.HasKey(BattleskillEffectLogicArgumentEnum.combi_cross_type) || work.GetInt(BattleskillEffectLogicArgumentEnum.combi_cross_type) == 0 || skillEffectListUnit1 != null)
                      {
                        int num13 = 0;
                        int num14 = 0;
                        if (work.HasKey(BattleskillEffectLogicArgumentEnum.combi_min_distance))
                          num13 = work.GetInt(BattleskillEffectLogicArgumentEnum.combi_min_distance);
                        if (work.HasKey(BattleskillEffectLogicArgumentEnum.combi_max_distance))
                          num14 = work.GetInt(BattleskillEffectLogicArgumentEnum.combi_max_distance);
                        if (num13 != 0 && num14 != 0 && (num5 == int.MinValue || num5 < num13 || num5 > num14) || work.HasKey(BattleskillEffectLogicArgumentEnum.combi_is_none) && work.GetInt(BattleskillEffectLogicArgumentEnum.combi_is_none) == 1 && num5 != int.MinValue)
                          continue;
                      }
                      else
                        continue;
                    }
                  }
                  if (num1 != 0)
                    intSet.Add(num1);
                  if (this.isInvoke(this.attacker, this.defender, this.attackStatus.duelParameter.attackerUnitParameter, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus, this.defenseStatus, duelSkill1.level, percentage_invocation, this.currentAttakerHp, this.currentDefenderHp, base_invocation: work.HasKey(BattleskillEffectLogicArgumentEnum.base_invocation) ? work.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation) : 0.0f, invocation_skill_ratio: work.HasKey(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) ? work.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) : 1f, invocation_luck_ratio: work.HasKey(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) ? work.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) : 1f, invalidSkillLogics: work.GetInvalidSkillsAndLogics()))
                  {
                    bool flag = false;
                    work.attackCount = num2;
                    foreach (BattleskillEffectLogicArgumentEnum key in dictionary.Keys)
                    {
                      if (work.HasKey(key) && dictionary[key](work, key))
                        flag = true;
                    }
                    if (flag || num2 >= 2 || work.HasKey(BattleskillEffectLogicArgumentEnum.invalid_skill_id) || work.HasKey(BattleskillEffectLogicArgumentEnum.invalid_logic_id))
                    {
                      this.attackCount = num2;
                      if (work.combiUnits != null && work.combiUnits[work.combiUnits.Length - 1] != null)
                        this.attackerCombiUnit = work.combiUnits;
                      this.invokeAttackerSkill = duelSkill1;
                      this.invokeAttackerSkillEffects.AddRange((IEnumerable<BattleskillEffect>) collection);
                      this.invokeAttackerDuelSkillEffectIds.Add(collection[0].ID);
                      if (!this.isSimulate)
                        return new BL.Skill[1]{ duelSkill1 };
                    }
                  }
                }
              }
            }
            if (this.isSimulate && this.invokeAttackerSkill != null)
              return new BL.Skill[1]{ duelSkill1 };
          }
        }
      }
      return BattleDuelSkill.noneSkills;
    }

    private bool funcGenericPercentageDamage(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.damageRate *= work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      return true;
    }

    private bool funcGenericBiAttackPercentageDamage(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.biAttackDamageRate = new float[work.attackCount];
      float num1 = 1f;
      for (int index = 1; index <= work.attackCount; ++index)
      {
        BattleskillEffectLogicArgumentEnum logicArgumentEnum = BattleskillEffectLogic.GetLogicArgumentEnum("percentage_damage_s" + (object) index);
        if (work.HasKey(logicArgumentEnum))
        {
          float num2 = work.GetFloat(logicArgumentEnum);
          if ((double) num2 != 0.0)
            num1 = num2;
        }
        this.biAttackDamageRate[index - 1] = num1;
      }
      return true;
    }

    private bool funcGenericPercentageAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.attackRate *= work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_attack);
      return true;
    }

    private bool funcGenericBaseAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.damageValue += work.GetFloat(BattleskillEffectLogicArgumentEnum.base_attack);
      return true;
    }

    private bool funcGenericPercentageDrain(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      float num = work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_drain);
      if ((double) this.drainRate < (double) num)
        this.drainRate = num;
      return true;
    }

    private bool funcGenericHitValue(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      float num = work.GetFloat(BattleskillEffectLogicArgumentEnum.hit_value);
      if ((double) num > 0.0)
      {
        this.FixHit = new float?(num);
        if (work.HasKey(BattleskillEffectLogicArgumentEnum.hit_priority))
          this.FixHitPriority = work.GetInt(BattleskillEffectLogicArgumentEnum.hit_priority);
      }
      return true;
    }

    private bool funcGenericCriticalRate(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      float num = work.GetFloat(BattleskillEffectLogicArgumentEnum.critical_rate);
      if ((double) num > 0.0)
        this.FixCritical = new float?(num);
      return true;
    }

    private bool funcGenericSkillId(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      int num = work.attackCount < 1 ? 1 : work.attackCount;
      for (int attackNo = 0; attackNo < num; ++attackNo)
        this.funcGenericSkillInvest((BattleDuelSkill.InvokeGenericWork) work, this.attacker, this.defender, attackNo, work.combiUnits);
      return true;
    }

    private bool funcGenericPercentageDecrease(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.defenseDownPhysicalRate *= work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_decrease);
      return true;
    }

    private bool funcGenericPercentageMagicDecrease(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.defenseDownMagicRate *= work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_magic_decrease);
      return true;
    }

    private bool funcGenericParameterToPhysicalAttackUp(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      int base_parameter,
      BattleskillEffectLogicArgumentEnum param)
    {
      if ((this.isChageAttackType || this.attackStatus.isMagic) && (!this.isChageAttackType || !this.attackStatus.isMagic))
        return false;
      this.damageValue += (float) base_parameter * work.GetFloat(param);
      return true;
    }

    private bool funcGenericHpPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.currentAttakerHp, param);
    }

    private bool funcGenericMaxHpPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attacker.originalUnit.parameter.Hp, param);
    }

    private bool funcGenericStrengthPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Strength, param);
    }

    private bool funcGenericIntelligencePhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Intelligence, param);
    }

    private bool funcGenericVitalityPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Vitality, param);
    }

    private bool funcGenericMindPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Mind, param);
    }

    private bool funcGenericAgilityPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Agility, param);
    }

    private bool funcGenericDexterityPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Dexterity, param);
    }

    private bool funcGenericLuckPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Luck, param);
    }

    private bool funcGenericHitPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Hit, param);
    }

    private bool funcGenericEvasionPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Evasion, param);
    }

    private bool funcGenericCriticalPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Critical, param);
    }

    private bool funcGenericPhysicalAttackPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.PhysicalAttack, param);
    }

    private bool funcGenericMagicAttackPhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.MagicAttack, param);
    }

    private bool funcGenericPhysicalDefensePhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.PhysicalDefense, param);
    }

    private bool funcGenericMagicDefensePhysicalAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToPhysicalAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.MagicDefense, param);
    }

    private bool funcGenericParameterToMagicAttackUp(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      int base_parameter,
      BattleskillEffectLogicArgumentEnum param)
    {
      if ((this.isChageAttackType || !this.attackStatus.isMagic) && (!this.isChageAttackType || this.attackStatus.isMagic))
        return false;
      this.damageValue += (float) base_parameter * work.GetFloat(param);
      return true;
    }

    private bool funcGenericHpMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.currentAttakerHp, param);
    }

    private bool funcGenericMaxHpMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attacker.originalUnit.parameter.Hp, param);
    }

    private bool funcGenericStrengthMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Strength, param);
    }

    private bool funcGenericIntelligenceMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Intelligence, param);
    }

    private bool funcGenericVitalityMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Vitality, param);
    }

    private bool funcGenericMindMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Mind, param);
    }

    private bool funcGenericAgilityMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Agility, param);
    }

    private bool funcGenericDexterityMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Dexterity, param);
    }

    private bool funcGenericLuckMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Luck, param);
    }

    private bool funcGenericHitMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Hit, param);
    }

    private bool funcGenericEvasionMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Evasion, param);
    }

    private bool funcGenericCriticalMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.Critical, param);
    }

    private bool funcGenericPhysicalAttackMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.PhysicalAttack, param);
    }

    private bool funcGenericMagicAttackMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.MagicAttack, param);
    }

    private bool funcGenericPhysicalDefenseMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.PhysicalDefense, param);
    }

    private bool funcGenericMagicDefenseMagicAttack(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      return this.funcGenericParameterToMagicAttackUp(work, this.attackStatus.duelParameter.attackerUnitParameter.MagicDefense, param);
    }

    private bool funcGenericInstantDeath(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.FixDamage = new int?(this.currentDefenderHp);
      return true;
    }

    private bool funcGenericRevenge(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.damageValue += (float) (this.attacker.originalUnit.parameter.Hp - this.currentAttakerHp) * work.GetFloat(BattleskillEffectLogicArgumentEnum.revenge);
      return true;
    }

    private bool funcGenericChangeAttackType(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.isChageAttackType = true;
      return true;
    }

    private bool funcGenericRateDamage(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.PercentageDamageRate = new float?(work.GetFloat(BattleskillEffectLogicArgumentEnum.rate_damage));
      this.PercentageDamageMax = work.HasKey(BattleskillEffectLogicArgumentEnum.rate_damage_max_value) ? work.GetInt(BattleskillEffectLogicArgumentEnum.rate_damage_max_value) : 0;
      return true;
    }

    private bool funcGenericCombiTargetSkillId(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      if (work.duelParam == null)
        return false;
      this.damageRate += work.rangeAddDamage;
      int length = work.duelParam.Length;
      if (work.duelParam[length - 1] != null)
      {
        long val = 0;
        for (int index = 0; index < length; ++index)
        {
          if (work.isMagic[index])
            val += (long) work.duelParam[index].DisplayMagicAttack;
          else
            val += (long) work.duelParam[index].DisplayPhysicalAttack;
        }
        this.additionalDamage = Judgement.CalcMaximumFloatValue((Decimal) this.additionalDamage + (Decimal) Judgement.CalcMaximumLongToInt(val));
      }
      else if (work.HasKey(BattleskillEffectLogicArgumentEnum.combi_none_percentage_damage))
        this.damageRate *= work.GetFloat(BattleskillEffectLogicArgumentEnum.combi_none_percentage_damage);
      return true;
    }

    private bool funcGenericCounterDamageHpPercentage(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.counterDamageHpPercentage = work.GetFloat(BattleskillEffectLogicArgumentEnum.counter_damage_hp_percentage);
      return true;
    }

    private bool funcGenericCounterDamageValue(
      BattleDuelSkill.InvokeAttackerGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.counterDamageValue = work.GetInt(BattleskillEffectLogicArgumentEnum.counter_damage_value);
      return true;
    }

    private BL.Skill[] InvokeDefenderGeneric()
    {
      Dictionary<BattleskillEffectLogicArgumentEnum, Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>> dictionary = new Dictionary<BattleskillEffectLogicArgumentEnum, Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>>()
      {
        {
          BattleskillEffectLogicArgumentEnum.percentage_damage,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericPercentageDamage)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_attack,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericPercentageAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.base_attack,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericBaseAttack)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_drain_ratio,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericPercentageDrainRatio)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_decrease_ratio,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericPercentageDecreaseRatio)
        },
        {
          BattleskillEffectLogicArgumentEnum.percentage_magic_decrease_ratio,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericPercentageMagicDecreaseRatio)
        },
        {
          BattleskillEffectLogicArgumentEnum.skill_id1,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericSkillId)
        },
        {
          BattleskillEffectLogicArgumentEnum.suppress_critical_percentage,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericSuppressCriticalPercentage)
        },
        {
          BattleskillEffectLogicArgumentEnum.suppress_one_more_attack_percentage,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericSuppressOneMoreAttackPercentage)
        },
        {
          BattleskillEffectLogicArgumentEnum.suppress_duel_skill_percentage,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericSuppressDuelSkillPercentage)
        },
        {
          BattleskillEffectLogicArgumentEnum.absolute_defense_percentage,
          new Func<BattleDuelSkill.InvokeDefenderGenericWork, BattleskillEffectLogicArgumentEnum, bool>(this.funcDefenderGenericAbsoluteDefense)
        }
      };
      List<Tuple<int, int>> suppressSkill = (List<Tuple<int, int>>) null;
      foreach (BL.Skill duelSkill in this.defender.originalUnit.duelSkills)
      {
        if (((IEnumerable<BattleskillEffect>) duelSkill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(BattleskillEffectLogicArgumentEnum.gdd_percentage_invocation))) && !this.defender.IsDontUseSkill(duelSkill.id) && this.defender.checkEnableSkill(duelSkill.skill))
        {
          HashSet<int> intSet = new HashSet<int>();
          foreach (List<BattleskillEffect> battleskillEffectList in this.CreateEffectPack(duelSkill, BattleskillEffectLogicArgumentEnum.gdd_percentage_invocation))
          {
            BattleDuelSkill.InvokeDefenderGenericWork defenderGenericWork = new BattleDuelSkill.InvokeDefenderGenericWork();
            defenderGenericWork.skill = duelSkill;
            defenderGenericWork.effects = battleskillEffectList;
            BattleDuelSkill.InvokeDefenderGenericWork work = defenderGenericWork;
            bool flag1 = work.HasKey(BattleskillEffectLogicArgumentEnum.suppress_duel_skill_percentage);
            if ((flag1 || !this.isPrecede) && (!flag1 || this.isPrecede))
            {
              int num1 = work.HasKey(BattleskillEffectLogicArgumentEnum.duel_skill_effect_group) ? work.GetInt(BattleskillEffectLogicArgumentEnum.duel_skill_effect_group) : 0;
              if (num1 == 0 || !intSet.Contains(num1))
              {
                if (suppressSkill == null)
                  suppressSkill = this.GetSuppressSkill(this.attacker, this.defender, this.attackPanel);
                if (this.CheckInvokeGeneric((BattleDuelSkill.InvokeGenericWork) work, this.defender, suppressSkill))
                {
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.is_attack))
                  {
                    bool flag2 = this.isInvokedAmbush ? this.isAttacker : !this.isAttacker;
                    int num2 = work.GetInt(BattleskillEffectLogicArgumentEnum.is_attack);
                    if (num2 == 1 && !flag2 || num2 == 2 & flag2)
                      continue;
                  }
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.skill_id))
                  {
                    int num3 = work.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
                    if (num3 != 0)
                    {
                      BL.Skill invokeAttackerSkill = this.biAttackDuelSkill.invokeAttackerSkill;
                      if (invokeAttackerSkill == null || invokeAttackerSkill.id != num3)
                        continue;
                    }
                  }
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.logic_id))
                  {
                    int logic_id = work.GetInt(BattleskillEffectLogicArgumentEnum.logic_id);
                    if (logic_id != 0)
                    {
                      List<BattleskillEffect> attackerSkillEffects = this.biAttackDuelSkill.invokeAttackerSkillEffects;
                      if (attackerSkillEffects == null || !attackerSkillEffects.Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.ID == logic_id)))
                        continue;
                    }
                  }
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.invoke_suisei))
                  {
                    int num4 = work.GetInt(BattleskillEffectLogicArgumentEnum.invoke_suisei);
                    if (num4 == 1 && !this.isBiattack || num4 == 2 && this.isBiattack)
                      continue;
                  }
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.invoke_drain))
                  {
                    int num5 = work.GetInt(BattleskillEffectLogicArgumentEnum.invoke_drain);
                    if (num5 != 0)
                    {
                      bool flag3 = this.isDefenderGenericDrainTarget();
                      if (num5 == 1 && !flag3 || num5 == 2 & flag3)
                        continue;
                    }
                  }
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.invoke_decrease))
                  {
                    int num6 = work.GetInt(BattleskillEffectLogicArgumentEnum.invoke_decrease);
                    if (num6 != 0)
                    {
                      bool flag4 = (double) this.biAttackDuelSkill.defenseDownPhysicalRate != 1.0;
                      if (num6 == 1 && !flag4 || num6 == 2 & flag4)
                        continue;
                    }
                  }
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.invoke_magic_decrease))
                  {
                    int num7 = work.GetInt(BattleskillEffectLogicArgumentEnum.invoke_magic_decrease);
                    if (num7 != 0)
                    {
                      bool flag5 = (double) this.biAttackDuelSkill.defenseDownMagicRate != 1.0;
                      if (num7 == 1 && !flag5 || num7 == 2 & flag5)
                        continue;
                    }
                  }
                  if (this.distance != 0 && work.HasKey(BattleskillEffectLogicArgumentEnum.out_of_range))
                  {
                    int num8 = work.GetInt(BattleskillEffectLogicArgumentEnum.out_of_range);
                    if (num8 != 0)
                    {
                      bool flag6 = this.isDefenderInRange();
                      if (num8 == 1 & flag6 || num8 == 2 && !flag6)
                        continue;
                    }
                  }
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.invoke_skill))
                  {
                    int num9 = work.GetInt(BattleskillEffectLogicArgumentEnum.invoke_skill);
                    if (num9 != 0)
                    {
                      bool flag7 = this.biAttackDuelSkill.invokeAttackerSkill != null;
                      if (num9 == 1 && !flag7 || num9 == 2 & flag7)
                        continue;
                    }
                  }
                  if (work.HasKey(BattleskillEffectLogicArgumentEnum.invoke_critical))
                  {
                    int num10 = work.GetInt(BattleskillEffectLogicArgumentEnum.invoke_critical);
                    if (num10 != 0 && (num10 == 1 && !this.isCritical || num10 == 2 && this.isCritical))
                      continue;
                  }
                  if (num1 != 0)
                    intSet.Add(num1);
                  float percentage_invocation = work.GetFloat(BattleskillEffectLogicArgumentEnum.gdd_percentage_invocation);
                  if (this.isInvoke(this.defender, this.attacker, this.attackStatus.duelParameter.defenderUnitParameter, this.attackStatus.duelParameter.attackerUnitParameter, this.defenseStatus, this.attackStatus, duelSkill.level, percentage_invocation, this.currentDefenderHp, this.currentAttakerHp, base_invocation: work.HasKey(BattleskillEffectLogicArgumentEnum.base_invocation) ? work.GetFloat(BattleskillEffectLogicArgumentEnum.base_invocation) : 0.0f, invocation_skill_ratio: work.HasKey(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) ? work.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_skill_ratio) : 1f, invocation_luck_ratio: work.HasKey(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) ? work.GetFloat(BattleskillEffectLogicArgumentEnum.invocation_luck_ratio) : 1f, invalidSkillLogics: work.GetInvalidSkillsAndLogics()))
                  {
                    bool flag8 = false;
                    foreach (BattleskillEffectLogicArgumentEnum key in dictionary.Keys)
                    {
                      if (work.HasKey(key) && dictionary[key](work, key))
                        flag8 = true;
                    }
                    if (flag8)
                    {
                      this.invokeDefenseGenericWork = (BattleDuelSkill.InvokeGenericWork) work;
                      this.invokeDefenderDuelSkillEffectIds.Add(battleskillEffectList[0].ID);
                      return new BL.Skill[1]{ duelSkill };
                    }
                  }
                }
              }
            }
          }
        }
      }
      return BattleDuelSkill.noneSkills;
    }

    private bool isDefenderGenericDrainTarget()
    {
      if ((double) this.biAttackDuelSkill.drainRate > 0.0)
        return true;
      return this.attackStatus.isDrain && this.biAttackDuelSkill.invokeAttackerSkill != null;
    }

    private bool funcDefenderGenericPercentageDamage(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.damageRate *= work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_damage);
      return true;
    }

    private bool funcDefenderGenericPercentageAttack(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.attackRate *= work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_attack);
      return true;
    }

    private bool funcDefenderGenericBaseAttack(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.damageValue += work.GetFloat(BattleskillEffectLogicArgumentEnum.base_attack);
      return true;
    }

    private bool funcDefenderGenericPercentageDrainRatio(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      if (!this.isPrecede && !this.isDefenderGenericDrainTarget())
        return true;
      this.drainRateRatio = work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_drain_ratio);
      return true;
    }

    private bool funcDefenderGenericPercentageDecreaseRatio(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.defenseDownPhysicalRateRatio = work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_decrease_ratio);
      return true;
    }

    private bool funcDefenderGenericPercentageMagicDecreaseRatio(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.defenseDownMagicRateRatio = work.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_magic_decrease_ratio);
      return true;
    }

    private bool funcDefenderGenericSkillId(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      if (this.isPrecede)
        return true;
      this.funcGenericSkillInvest((BattleDuelSkill.InvokeGenericWork) work, this.defender, this.attacker, 0);
      return true;
    }

    private bool funcDefenderGenericSuppressCriticalPercentage(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.isSuppressCritical = (double) work.GetFloat(BattleskillEffectLogicArgumentEnum.suppress_critical_percentage) + (work.HasKey(BattleskillEffectLogicArgumentEnum.suppress_critical_percentage_levelup) ? (double) work.GetFloat(BattleskillEffectLogicArgumentEnum.suppress_critical_percentage_levelup) : 0.0) * (double) work.skill.level >= (double) this.random.NextFloat();
      return true;
    }

    private bool funcDefenderGenericSuppressOneMoreAttackPercentage(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.attackerCantOneMore = (double) work.GetFloat(BattleskillEffectLogicArgumentEnum.suppress_one_more_attack_percentage) + (work.HasKey(BattleskillEffectLogicArgumentEnum.suppress_one_more_attack_percentage_levelup) ? (double) work.GetFloat(BattleskillEffectLogicArgumentEnum.suppress_one_more_attack_percentage_levelup) : 0.0) * (double) work.skill.level >= (double) this.random.NextFloat();
      return true;
    }

    private bool funcDefenderGenericSuppressDuelSkillPercentage(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      this.isSuppressDuelSkill = (double) work.GetFloat(BattleskillEffectLogicArgumentEnum.suppress_duel_skill_percentage) + (work.HasKey(BattleskillEffectLogicArgumentEnum.suppress_duel_skill_percentage_levelup) ? (double) work.GetFloat(BattleskillEffectLogicArgumentEnum.suppress_duel_skill_percentage_levelup) : 0.0) * (double) work.skill.level >= (double) this.random.NextFloat();
      return true;
    }

    private bool funcDefenderGenericAbsoluteDefense(
      BattleDuelSkill.InvokeDefenderGenericWork work,
      BattleskillEffectLogicArgumentEnum param)
    {
      if (BattleFuncs.checkInvalidEffect(work.GetHasKeyEffect(BattleskillEffectLogicArgumentEnum.absolute_defense_percentage), this.attackerInvalidSkillLogics))
        return false;
      this.isAbsoluteDefense = (double) work.GetFloat(BattleskillEffectLogicArgumentEnum.absolute_defense_percentage) + (work.HasKey(BattleskillEffectLogicArgumentEnum.absolute_defense_percentage_levelup) ? (double) work.GetFloat(BattleskillEffectLogicArgumentEnum.absolute_defense_percentage_levelup) : 0.0) * (double) work.skill.level >= (double) this.random.NextFloat();
      return true;
    }

    public void addInvestSkills(
      BL.ISkillEffectListUnit unit,
      BL.Skill[] skills,
      int[] skillIds,
      BL.ISkillEffectListUnit from,
      int fromSkillId,
      bool isAilment,
      bool isUnconditional,
      int attackNo,
      int onceInvestFlag = 0,
      bool isSuccess = true,
      List<BL.SkillEffect> useResistEffects = null)
    {
      if (this.investSkills == null)
        this.investSkills = new List<List<BattleDuelSkill.InvestSkills>>();
      while (this.investSkills.Count <= attackNo)
        this.investSkills.Add(new List<BattleDuelSkill.InvestSkills>());
      if (skills != null)
        this.investSkills[attackNo].Add(new BattleDuelSkill.InvestSkills(unit, skills, from, fromSkillId, isAilment, isUnconditional, onceInvestFlag, isSuccess, useResistEffects));
      if (!isAilment || this.isColossume)
        return;
      int num1;
      BL.Panel myPanel;
      if (unit == this.attacker)
      {
        num1 = this.currentAttakerHp;
        myPanel = this.attackPanel;
      }
      else if (unit == this.defender)
      {
        num1 = this.currentDefenderHp;
        myPanel = this.defensePanel;
      }
      else
      {
        num1 = !this.turnHp.otherHp.ContainsKey(unit) ? unit.hp : this.turnHp.otherHp[unit].hp;
        myPanel = BattleFuncs.getPanel(BattleFuncs.iSkillEffectListUnitToUnitPosition(unit));
      }
      int num2 = from != this.attacker ? (from != this.defender ? (!this.turnHp.otherHp.ContainsKey(from) ? from.hp : this.turnHp.otherHp[from].hp) : this.currentDefenderHp) : this.currentAttakerHp;
      foreach (int skillId in skillIds)
      {
        foreach (BL.SkillEffect triggerSkillEffect in BattleFuncs.getAilmentTriggerSkillEffects(skillId, unit, from, myPanel, new int?(num1), new int?(num2)))
        {
          BL.Skill skill = new BL.Skill()
          {
            id = triggerSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id),
            level = 1
          };
          this.investSkills[attackNo].Add(new BattleDuelSkill.InvestSkills(unit, new BL.Skill[1]
          {
            skill
          }, unit, triggerSkillEffect.baseSkillId, (skill.skill.skill_type == BattleskillSkillType.ailment ? 1 : 0) != 0, (isUnconditional ? 1 : 0) != 0, triggerSkillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.once_invest), true, (List<BL.SkillEffect>) null));
        }
      }
    }

    private bool canSkillInvest(
      BattleDuelSkill.InvestSkills invest,
      List<BL.DuelTurn> turns,
      List<BL.ISkillEffectListUnit> turnInvestUnit,
      List<int> turnInvestSkillIds,
      List<BL.ISkillEffectListUnit> turnInvestFrom)
    {
      if (turns == null)
        return true;
      if ((invest.onceInvestFlag & 1) != 0 && turns.Any<BL.DuelTurn>((Func<BL.DuelTurn, bool>) (turn =>
      {
        int length = turn.investUnit.Length;
        for (int i = 0; i < length; i++)
        {
          if (turn.investUnit[i] == invest.unit && turn.investFrom[i] == invest.from && ((IEnumerable<BL.Skill>) invest.skills).Any<BL.Skill>((Func<BL.Skill, bool>) (x => turn.investSkillIds[i] == x.id)))
            return true;
        }
        return false;
      })))
        return false;
      if ((invest.onceInvestFlag & 2) != 0)
      {
        int count = turnInvestUnit.Count;
        for (int i = 0; i < count; i++)
        {
          if (turnInvestUnit[i] == invest.unit && turnInvestFrom[i] == invest.from && ((IEnumerable<BL.Skill>) invest.skills).Any<BL.Skill>((Func<BL.Skill, bool>) (x => turnInvestSkillIds[i] == x.id)))
            return false;
        }
      }
      return true;
    }

    public BL.Skill[] getInvestSkills(
      BL.ISkillEffectListUnit unit,
      bool isAilment,
      bool isUnconditional,
      int attackNo,
      List<BL.DuelTurn> turns = null,
      List<BL.ISkillEffectListUnit> turnInvestUnit = null,
      List<int> turnInvestSkillIds = null,
      List<BL.ISkillEffectListUnit> turnInvestFrom = null)
    {
      if (this.investSkills == null)
        return (BL.Skill[]) null;
      if (this.investSkills.Count <= attackNo)
        return (BL.Skill[]) null;
      IEnumerable<BattleDuelSkill.InvestSkills> source = this.investSkills[attackNo].Where<BattleDuelSkill.InvestSkills>((Func<BattleDuelSkill.InvestSkills, bool>) (x => x.unit == unit && x.isAilment == isAilment && x.isUnconditional == isUnconditional));
      if (!source.Any<BattleDuelSkill.InvestSkills>())
        return (BL.Skill[]) null;
      List<BL.Skill> skillList = new List<BL.Skill>();
      foreach (BattleDuelSkill.InvestSkills invest in source)
      {
        if (this.canSkillInvest(invest, turns, turnInvestUnit, turnInvestSkillIds, turnInvestFrom) && invest.isSuccess)
          skillList.AddRange((IEnumerable<BL.Skill>) invest.skills);
      }
      return skillList.ToArray();
    }

    public Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]> getAllInvestList(
      bool isAilment,
      bool isUnconditional,
      int attackNo,
      List<BL.DuelTurn> turns = null,
      List<BL.ISkillEffectListUnit> turnInvestUnit = null,
      List<int> turnInvestSkillIds = null,
      List<BL.ISkillEffectListUnit> turnInvestFrom = null)
    {
      if (this.investSkills == null)
        return (Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]>) null;
      if (this.investSkills.Count <= attackNo)
        return (Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]>) null;
      IEnumerable<BattleDuelSkill.InvestSkills> source = this.investSkills[attackNo].Where<BattleDuelSkill.InvestSkills>((Func<BattleDuelSkill.InvestSkills, bool>) (x => x.isAilment == isAilment && x.isUnconditional == isUnconditional));
      if (!source.Any<BattleDuelSkill.InvestSkills>())
        return (Tuple<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]>) null;
      List<BL.ISkillEffectListUnit> skillEffectListUnitList1 = new List<BL.ISkillEffectListUnit>();
      List<int> intList1 = new List<int>();
      List<BL.ISkillEffectListUnit> skillEffectListUnitList2 = new List<BL.ISkillEffectListUnit>();
      List<int> intList2 = new List<int>();
      List<BL.ISkillEffectListUnit> skillEffectListUnitList3 = new List<BL.ISkillEffectListUnit>();
      List<BL.SkillEffect> skillEffectList = new List<BL.SkillEffect>();
      foreach (BattleDuelSkill.InvestSkills invest in source)
      {
        if (this.canSkillInvest(invest, turns, turnInvestUnit, turnInvestSkillIds, turnInvestFrom))
        {
          if (invest.isSuccess)
          {
            foreach (BL.Skill skill in invest.skills)
            {
              skillEffectListUnitList1.Add(invest.unit);
              intList1.Add(skill.id);
              skillEffectListUnitList2.Add(invest.from);
              intList2.Add(invest.fromSkillId);
            }
          }
          if (isAilment)
          {
            foreach (BL.SkillEffect useResistEffect in invest.useResistEffects)
            {
              skillEffectListUnitList3.Add(invest.unit);
              skillEffectList.Add(useResistEffect);
            }
          }
        }
      }
      return Tuple.Create<BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], int[], BL.ISkillEffectListUnit[], BL.SkillEffect[]>(skillEffectListUnitList1.ToArray(), intList1.ToArray(), skillEffectListUnitList2.ToArray(), intList2.ToArray(), skillEffectListUnitList3.ToArray(), skillEffectList.ToArray());
    }

    private IEnumerable<BL.ISkillEffectListUnit> getInvestUnit(
      BL.ISkillEffectListUnit myself,
      BL.ISkillEffectListUnit enemy,
      int skillId,
      int rangeType)
    {
      return BattleFuncs.getInvestUnit(myself, enemy, skillId, rangeType, this.isAI, this.isColossume);
    }

    private bool isDefenderInRange()
    {
      if (this.distance == 0)
        return true;
      Tuple<int, int> addRange = this.defensePanel.getEffectsAddRange(this.defender.originalUnit);
      bool flag = this.defender.originalUnit.magicBullets.Length != 0;
      if (flag && ((IEnumerable<BL.MagicBullet>) this.defender.originalUnit.magicBullets).Where<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x =>
      {
        if (x != null && x.isAttack)
        {
          BL.Unit.MagicRange magicRange = this.defender.magicRange(x);
          if (NC.IsReach(magicRange.Min + addRange.Item1, magicRange.Max + addRange.Item2, this.distance))
            return true;
        }
        return false;
      })).Any<BL.MagicBullet>())
        return true;
      if (!flag || this.defender.originalUnit.unit.magic_warrior_flag)
      {
        BL.Unit.GearRange gearRange = this.defender.gearRange();
        if (NC.IsReach(gearRange.Min + addRange.Item1, gearRange.Max + addRange.Item2, this.distance))
          return true;
      }
      return false;
    }

    private int getUnitCurrentHp(BL.ISkillEffectListUnit unit)
    {
      if (unit == this.attacker)
        return this.currentAttakerHp;
      if (unit == this.defender)
        return this.currentDefenderHp;
      return this.turnHp.otherHp.ContainsKey(unit) ? this.turnHp.otherHp[unit].hp : unit.hp;
    }

    private class InvokeGenericWork
    {
      public BL.Skill skill;
      public List<BattleskillEffect> effects;

      public bool HasKey(BattleskillEffectLogicArgumentEnum key)
      {
        return this.effects.Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(key)));
      }

      public int GetInt(BattleskillEffectLogicArgumentEnum key)
      {
        return this.effects.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(key))).First<BattleskillEffect>().GetInt(key);
      }

      public float GetFloat(BattleskillEffectLogicArgumentEnum key)
      {
        return this.effects.Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.HasKey(key))).First<BattleskillEffect>().GetFloat(key);
      }

      public List<BattleFuncs.InvalidSpecificSkillLogic> GetInvalidSkillsAndLogics()
      {
        return BattleFuncs.GetInvalidSkillsAndLogics((IEnumerable<BattleskillEffect>) this.effects);
      }

      public BattleskillEffect GetHasKeyEffect(BattleskillEffectLogicArgumentEnum key)
      {
        return this.effects.Find((Predicate<BattleskillEffect>) (x => x.HasKey(key)));
      }
    }

    private class InvokeAttackerGenericWork : BattleDuelSkill.InvokeGenericWork
    {
      public Judgement.BeforeDuelParameter[] duelParam;
      public bool[] isMagic;
      public float rangeAddDamage;
      public BL.ISkillEffectListUnit[] combiUnits;
      public int attackCount;
    }

    private class InvokeDefenderGenericWork : BattleDuelSkill.InvokeGenericWork
    {
    }

    public class InvestSkills
    {
      public BL.ISkillEffectListUnit unit { get; private set; }

      public BL.Skill[] skills { get; private set; }

      public BL.ISkillEffectListUnit from { get; private set; }

      public int fromSkillId { get; private set; }

      public bool isAilment { get; private set; }

      public bool isUnconditional { get; private set; }

      public int onceInvestFlag { get; private set; }

      public bool isSuccess { get; private set; }

      public List<BL.SkillEffect> useResistEffects { get; private set; }

      public InvestSkills(
        BL.ISkillEffectListUnit unit,
        BL.Skill[] skills,
        BL.ISkillEffectListUnit from,
        int fromSkillId,
        bool isAilment,
        bool isUnconditional,
        int onceInvestFlag,
        bool isSuccess,
        List<BL.SkillEffect> useResistEffects)
      {
        this.unit = unit;
        this.skills = skills;
        this.from = from;
        this.fromSkillId = fromSkillId;
        this.isAilment = isAilment;
        this.isUnconditional = isUnconditional;
        this.onceInvestFlag = onceInvestFlag;
        this.isSuccess = isSuccess;
        this.useResistEffects = useResistEffects != null ? useResistEffects : new List<BL.SkillEffect>();
      }
    }
  }
}
