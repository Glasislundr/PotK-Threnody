// Decompiled with JetBrains decompiler
// Type: GameCore.AttackStatus
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
  [Serializable]
  public class AttackStatus
  {
    public BL.MagicBullet magicBullet;
    public BL.Weapon weapon;
    public CommonElement overwriteElement;
    public List<CommonElement> attackElements;
    public bool isAbsoluteCounterAttack;
    public Judgement.BeforeDuelParameter duelParameter;
    public float elementAttackRate;
    public float elementDamageRate;
    public float attackElementDamageRate;
    public float attackClassificationRate;

    public bool isMagic { get; set; }

    public bool isHeal => this.magicBullet != null && this.magicBullet.isHeal;

    public bool isDrain => this.magicBullet != null && this.magicBullet.isDrain;

    public float drainRate => this.magicBullet == null ? 0.0f : this.magicBullet.drainRate;

    public float attackRate { get; set; }

    public float normalDamageRate { get; set; }

    public int normalAttackCount { get; set; }

    public int attack => NC.Clamp(1, int.MaxValue, (int) this.originalAttack);

    public float originalAttack
    {
      get
      {
        return !this.isMagic ? this.duelParameter.CalcPhysicalAttack(this.attackRate) : this.duelParameter.CalcMagicAttack(this.attackRate);
      }
    }

    public float originalAttackExcludeBaseDamage
    {
      get
      {
        return !this.isMagic ? this.duelParameter.CalcPhysicalAttack(this.attackRate, true) : this.duelParameter.CalcMagicAttack(this.attackRate, true);
      }
    }

    public int healAttack(BL.ISkillEffectListUnit attack, BL.ISkillEffectListUnit defense)
    {
      if (((IEnumerable<BattleskillEffect>) this.magicBullet.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.power_heal)))
        return NC.Clamp(1, int.MaxValue, this.duelParameter.attackerUnitParameter.MagicAttack);
      long val = 0;
      foreach (BattleskillEffect effect in this.magicBullet.skill.Effects)
      {
        switch (effect.EffectLogic.Enum)
        {
          case BattleskillEffectLogicEnum.fix_heal:
            val += (long) effect.GetInt(BattleskillEffectLogicArgumentEnum.value);
            break;
          case BattleskillEffectLogicEnum.parameter_reference_heal:
            val += (long) BattleFuncs.getParameterReferenceHealValue(attack, defense, effect);
            break;
        }
      }
      return Judgement.CalcMaximumLongToInt(val);
    }

    public int attackCount => this.duelParameter.AttackCount;

    public float hit => NC.Clampf(0.0f, 1f, (float) this.duelParameter.DisplayHit / 100f);

    public float critical => NC.Clampf(0.0f, 1f, (float) this.duelParameter.DisplayCritical / 100f);

    public int dexerityDisplay() => NC.Clamp(0, 100, this.duelParameter.DisplayHit);

    public int criticalDisplay() => NC.Clamp(0, 100, this.duelParameter.DisplayCritical);

    public int normalDamage() => this.attack * this.attackCount;

    public int expectationDamage()
    {
      return (int) ((double) (this.attack * this.attackCount) * (double) this.hit);
    }

    public void calcElementAttackRate(
      BL.ISkillEffectListUnit attack,
      BL.ISkillEffectListUnit defense)
    {
      Tuple<float, CommonElement?, List<CommonElement>, float, float> elementAttackRate = BattleDuelSkill.getElementAttackRate(attack, this, defense);
      this.elementAttackRate = elementAttackRate.Item1;
      this.overwriteElement = !elementAttackRate.Item2.HasValue ? (CommonElement) 0 : elementAttackRate.Item2.Value;
      this.attackElements = elementAttackRate.Item3;
      this.elementDamageRate = elementAttackRate.Item4;
      this.attackElementDamageRate = elementAttackRate.Item5;
    }

    public CommonElement? GetOverwriteElement()
    {
      return this.overwriteElement == (CommonElement) 0 ? new CommonElement?() : new CommonElement?(this.overwriteElement);
    }

    public List<CommonElement> GetAttackElements() => this.attackElements;

    public void calcAttackClassificationRate(
      BL.ISkillEffectListUnit attack,
      BL.ISkillEffectListUnit defense,
      BL.Panel attackPanel = null,
      BL.Panel defensePanel = null)
    {
      this.attackClassificationRate = BattleFuncs.calcAttackClassificationRate(this, attack, defense, attackPanel, defensePanel);
    }

    public float realHit
    {
      get
      {
        return NC.Clampf(0.0f, 1f, this.hit + Mathf.Sin((float) ((1.0 - (double) this.hit) * 2.0 * 3.1415927410125732)) / 10f);
      }
    }

    public bool calcHit(float value) => (double) this.realHit >= (double) value;

    public string ShowAttackStatus(BL.ISkillEffectListUnit attack, BL.ISkillEffectListUnit defense)
    {
      return string.Format("magic({0}) attack({1}) heal({2}) count({3}) hit({4}) critical({5})", this.magicBullet != null ? (object) (this.magicBullet.name + " : " + (object) this.magicBullet.cost) : (object) "-", (object) attack, (object) this.healAttack(attack, defense), (object) this.attackCount, (object) this.dexerityDisplay(), (object) this.criticalDisplay());
    }
  }
}
