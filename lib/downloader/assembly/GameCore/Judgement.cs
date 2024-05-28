// Decompiled with JetBrains decompiler
// Type: GameCore.Judgement
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
  public class Judgement
  {
    public static readonly int MaximumDamageValue = 1000000000;
    public static readonly int MinimumDamageValue = -1000000000;

    public static float CalcMaximumFloatValue(Decimal val)
    {
      if (val >= (Decimal) Judgement.MaximumDamageValue)
        return (float) Judgement.MaximumDamageValue;
      return val <= (Decimal) Judgement.MinimumDamageValue ? (float) Judgement.MinimumDamageValue : (float) val;
    }

    public static int CalcMaximumCeilToIntValue(Decimal val)
    {
      if (val >= (Decimal) Judgement.MaximumDamageValue)
        return Judgement.MaximumDamageValue;
      return val <= (Decimal) Judgement.MinimumDamageValue ? Judgement.MinimumDamageValue : Mathf.CeilToInt((float) val);
    }

    public static int CalcMaximumFloorToIntValue(Decimal val)
    {
      if (val >= (Decimal) Judgement.MaximumDamageValue)
        return Judgement.MaximumDamageValue;
      return val <= (Decimal) Judgement.MinimumDamageValue ? Judgement.MinimumDamageValue : Mathf.FloorToInt((float) val);
    }

    public static int CalcMaximumLongToInt(long val)
    {
      if (val >= (long) Judgement.MaximumDamageValue)
        return Judgement.MaximumDamageValue;
      return val <= (long) Judgement.MinimumDamageValue ? Judgement.MinimumDamageValue : (int) val;
    }

    public static bool CheckEnabledBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BattleskillInvokeGameModeEnum gameMode,
      BL.Panel panel,
      bool isHp,
      bool isAI)
    {
      BattleskillEffect effect = x.effect;
      return effect.GetPackedSkillEffect().CheckLandTag(panel, isAI) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 && BattleFuncs.checkElement(unit, effect.GetInt(BattleskillEffectLogicArgumentEnum.element)) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && (isHp || !BattleFuncs.isSealedSkillEffect(unit, x)) && effect.isEnableGameMode(gameMode, unit);
    }

    public static bool CheckEnabledBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType,
      BattleskillInvokeGameModeEnum gameMode,
      BL.Panel panel,
      bool isHp,
      bool isAI)
    {
      BattleskillEffect effect = x.effect;
      if (!effect.GetPackedSkillEffect().CheckLandTag(panel, isAI) || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != unit.originalUnit.job.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != attackType || !BattleFuncs.checkElement(unit, effect.GetInt(BattleskillEffectLogicArgumentEnum.element)) || !BattleFuncs.checkElement(target, effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element)) || !isHp && BattleFuncs.isSealedSkillEffect(unit, x) || !effect.isEnableGameMode(gameMode, unit) || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target))
        return false;
      return isHp || BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledBuffDebuff(
      BattleskillEffect x,
      BL.Unit unit,
      BattleskillInvokeGameModeEnum gameMode)
    {
      BattleskillEffect battleskillEffect = x;
      return (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.job.ID) && (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.unit.kind.ID) && BattleFuncs.checkPassiveEffectEnabledElement((BL.ISkillEffectListUnit) unit, battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element)) && battleskillEffect.isEnableGameMode(gameMode, (BL.ISkillEffectListUnit) unit);
    }

    public static bool CheckEnabledBuffDebuff2(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BattleskillInvokeGameModeEnum gameMode,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      return (!effect.HasKey(BattleskillEffectLogicArgumentEnum.attack_type) || effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_type) == 0) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && (isHp || !BattleFuncs.isSealedSkillEffect(unit, x)) && effect.isEnableGameMode(gameMode, unit);
    }

    public static bool CheckEnabledBuffDebuff2(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType,
      BattleskillInvokeGameModeEnum gameMode,
      bool isHp,
      bool? isMagic)
    {
      BattleskillEffect effect = x.effect;
      if (effect.HasKey(BattleskillEffectLogicArgumentEnum.attack_type))
      {
        int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_type);
        if (num == 1 && (!isMagic.HasValue || isMagic.Value) || num == 2 && (!isMagic.HasValue || !isMagic.Value))
          return false;
      }
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != attackType || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement() || !isHp && BattleFuncs.isSealedSkillEffect(unit, x) || !effect.isEnableGameMode(gameMode, unit) || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target))
        return false;
      return isHp || BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledBuffDebuff2(
      BattleskillEffect x,
      BL.Unit unit,
      BattleskillInvokeGameModeEnum gameMode)
    {
      BattleskillEffect battleskillEffect = x;
      return (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.unit.kind.ID) && (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.playerUnit.GetElement()) && battleskillEffect.isEnableGameMode(gameMode, (BL.ISkillEffectListUnit) unit);
    }

    public static bool CheckEnabledBuffDebuff3(BL.SkillEffect x, BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect effect = x.effect;
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && !BattleFuncs.isSealedSkillEffect(unit, x);
    }

    public static bool CheckEnabledBuffDebuff3(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType)
    {
      BattleskillEffect effect = x.effect;
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == attackType) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && !BattleFuncs.isSealedSkillEffect(unit, x) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledBuffDebuff3(BattleskillEffect x, BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect battleskillEffect = x;
      if (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID)
        return false;
      return battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
    }

    public static bool CheckEnabledBuffDebuff4(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) != 0 && !unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id)))
        return false;
      return isHp || !BattleFuncs.isSealedSkillEffect(unit, x);
    }

    public static bool CheckEnabledBuffDebuff4(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != attackType || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) != 0 && !unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id)) || !isHp && BattleFuncs.isSealedSkillEffect(unit, x) || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target))
        return false;
      return isHp || BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledBuffDebuff4(BattleskillEffect x, BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect battleskillEffect = x;
      if (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement())
        return false;
      return battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id));
    }

    public static BL.SkillEffect GetEnabledRangeBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int distance,
      int attackType)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledRangeBuffDebuff(x, unit, target, distance, attackType))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => Mathf.Abs(distance - effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range)))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledRangeBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int distance,
      int attackType)
    {
      BattleskillEffect effect = x.effect;
      return (!effect.HasKey(BattleskillEffectLogicArgumentEnum.is_attack_nc) || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc) == attackType) && !BattleFuncs.isSealedSkillEffect(unit, x) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledRangeBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return true;
    }

    public static BL.SkillEffect GetEnabledHpLeBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      float hpRatio)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledHpLeBuffDebuff(x, unit, hpRatio))).OrderBy<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect => effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledHpLeBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      float hpRatio)
    {
      if (BattleFuncs.isSealedSkillEffect(unit, effect) || (double) hpRatio > (double) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != unit.originalUnit.job.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0)
        return false;
      return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id));
    }

    public static BL.SkillEffect GetEnabledHpLeBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      float hpRatio)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledHpLeBuffDebuff(x, unit, target, hpRatio))).OrderBy<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect => effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledHpLeBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      float hpRatio)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && (double) hpRatio <= (double) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == target.originalUnit.job.ID) && (!effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static BL.SkillEffect GetEnabledHpGeBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      float hpRatio)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledHpGeBuffDebuff(x, unit, hpRatio))).OrderByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect => effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledHpGeBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      float hpRatio)
    {
      if (BattleFuncs.isSealedSkillEffect(unit, effect) || (double) hpRatio < (double) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != unit.originalUnit.job.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0)
        return false;
      return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id));
    }

    public static BL.SkillEffect GetEnabledHpGeBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      float hpRatio)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledHpGeBuffDebuff(x, unit, target, hpRatio))).OrderByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect => effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledHpGeBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      float hpRatio)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && (double) hpRatio >= (double) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.border) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == target.originalUnit.job.ID) && (!effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledHpBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != unit.originalUnit.job.ID)
        return false;
      return !effect.HasKey(BattleskillEffectLogicArgumentEnum.skill_group_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id));
    }

    public static bool CheckEnabledTargetCountBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ForceID[] targetForceId,
      BL.Panel panel,
      bool isAI)
    {
      if (BattleFuncs.isSealedSkillEffect(unit, effect))
        return false;
      return BattleFuncs.getTargets(panel.row, panel.column, new int[2]
      {
        effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
        effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
      }, targetForceId, BL.Unit.TargetAttribute.all, (isAI ? 1 : 0) != 0, nonFacility: true).Any<BL.UnitPosition>();
    }

    public static bool CheckEnabledTargetCountBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BL.ForceID[] targetForceId,
      BL.Panel panel,
      bool isAI)
    {
      if (!BattleFuncs.isSealedSkillEffect(unit, effect))
      {
        if (BattleFuncs.getTargets(panel.row, panel.column, new int[2]
        {
          effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
          effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
        }, targetForceId, BL.Unit.TargetAttribute.all, (isAI ? 1 : 0) != 0, nonFacility: true).Any<BL.UnitPosition>() && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target))
          return !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
      }
      return false;
    }

    public static bool CheckEnabledTargetCountBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return true;
    }

    public static bool CheckEnabledCharismaBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.Panel effectPanel,
      bool isAI)
    {
      BL.ISkillEffectListUnit unit1 = unit;
      if (BattleFuncs.isSealedSkillEffect(unit1, effect) || 0 < effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range) || 0 > effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit1.originalUnit.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != unit.originalUnit.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit1.originalUnit.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != unit.originalUnit.job.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) != 0)
        return false;
      BattleFuncs.PackedSkillEffect packedSkillEffect = effect.effect.GetPackedSkillEffect();
      if (!packedSkillEffect.CheckLandTag(effectPanel, isAI))
        return false;
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.level_up_status_type))
      {
        int statusType1 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
        if (statusType1 != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit1, statusType1);
          Decimal num1 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
          Decimal num2 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
          if (num1 != 0M && levelUpStatusRatio < num1 || num2 != 0M && levelUpStatusRatio >= num2)
            return false;
        }
        int statusType2 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_level_up_status_type);
        if (statusType2 != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType2);
          Decimal num3 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_min);
          Decimal num4 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_max);
          if (num3 != 0M && levelUpStatusRatio < num3 || num4 != 0M && levelUpStatusRatio >= num4)
            return false;
        }
      }
      return true;
    }

    public static IEnumerable<BL.SkillEffect> GetEnabledCharismaBuffDebuff(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit effectUnit,
      int effectTarget,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int distance,
      BL.Panel panel,
      BL.Panel effectPanel,
      bool isAI)
    {
      return self.Where(e, (Func<BL.SkillEffect, bool>) (effect => Judgement.CheckEnabledCharismaBuffDebuff(effect, effectUnit, effectTarget, unit, target, distance, panel, effectPanel, isAI)));
    }

    public static bool CheckEnabledCharismaBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit effectUnit,
      int effectTarget,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int distance,
      BL.Panel panel,
      BL.Panel effectPanel,
      bool isAI)
    {
      if (BattleFuncs.isSealedSkillEffect(effectUnit, effect) || distance < effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range) || distance > effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.excluding_slanting) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting) != 0 && panel.row != effectPanel.row && panel.column != effectPanel.column || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != unit.originalUnit.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != unit.originalUnit.job.ID || effectTarget != effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) || effectTarget != 0 && unit.originalUnit.isFacility || effectUnit == target && BattleFuncs.isSkillsAndEffectsInvalid(target, unit) || BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) || BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return false;
      BattleFuncs.PackedSkillEffect packedSkillEffect = effect.effect.GetPackedSkillEffect();
      if (!packedSkillEffect.CheckLandTag(effectPanel, isAI))
        return false;
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.level_up_status_type))
      {
        int statusType1 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
        if (statusType1 != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(effectUnit, statusType1);
          Decimal num1 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
          Decimal num2 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
          if (num1 != 0M && levelUpStatusRatio < num1 || num2 != 0M && levelUpStatusRatio >= num2)
            return false;
        }
        int statusType2 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_level_up_status_type);
        if (statusType2 != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType2);
          Decimal num3 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_min);
          Decimal num4 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_max);
          if (num3 != 0M && levelUpStatusRatio < num3 || num4 != 0M && levelUpStatusRatio >= num4)
            return false;
        }
      }
      return true;
    }

    public static bool CheckEnabledCharismaBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      BL.ISkillEffectListUnit skillEffectListUnit = unit;
      if (effect.HasKey(BattleskillEffectLogicArgumentEnum.element) && effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != skillEffectListUnit.originalUnit.playerUnit.GetElement() || effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != skillEffectListUnit.originalUnit.unit.kind.ID)
        return false;
      BattleFuncs.PackedSkillEffect packedSkillEffect = effect.GetPackedSkillEffect();
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.level_up_status_type))
      {
        int statusType = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
        if (statusType != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType);
          Decimal num1 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
          Decimal num2 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
          if (num1 != 0M && levelUpStatusRatio < num1 || num2 != 0M && levelUpStatusRatio >= num2)
            return false;
        }
      }
      return true;
    }

    public static IEnumerable<BL.SkillEffect> GetEnabledCharismaBuffDebuffByCheckInvokeGeneric(
      BL.SkillEffectList self,
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit effectUnit,
      int effectTarget,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int distance,
      BL.Panel panel,
      BL.Panel effectPanel,
      int? colosseumTurn,
      int attackType,
      int? effectUnitHp,
      int? unitHp)
    {
      return self.Where(e, (Func<BL.SkillEffect, bool>) (effect => Judgement.CheckEnabledCharismaBuffDebuffByCheckInvokeGeneric(effect, effectUnit, effectTarget, unit, target, distance, panel, effectPanel, colosseumTurn, attackType, effectUnitHp, unitHp)));
    }

    public static bool CheckEnabledCharismaBuffDebuffByCheckInvokeGeneric(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit effectUnit,
      int effectTarget,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int distance,
      BL.Panel panel,
      BL.Panel effectPanel,
      int? colosseumTurn,
      int attackType,
      int? effectUnitHp,
      int? unitHp)
    {
      return distance >= effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range) && distance <= effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range) && (!effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.excluding_slanting) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.excluding_slanting) == 0 || panel.row == effectPanel.row || panel.column == effectPanel.column) && effectTarget == effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) && effect.GetCheckInvokeGeneric().DoCheck(effectUnit, unit, colosseumTurn, unitHp: effectUnitHp, targetHp: unitHp, attackType: attackType, unitPanel: effectPanel, targetPanel: panel, effect: effect) && (effectTarget == 0 || !unit.originalUnit.isFacility) && (effectUnit != target || !BattleFuncs.isSkillsAndEffectsInvalid(target, unit)) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledCharismaPanelBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      bool isAI)
    {
      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitToISkillEffectListUnit(effect.parentUnit, isAI);
      if (iskillEffectListUnit == unit || unit.originalUnit.isFacility || iskillEffectListUnit.IsJumping)
        return false;
      int num1 = BattleFuncs.getForceID(unit.originalUnit) == BattleFuncs.getForceID(iskillEffectListUnit.originalUnit) ? 0 : 1;
      if (BattleFuncs.isSealedSkillEffect(iskillEffectListUnit, effect) || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != iskillEffectListUnit.originalUnit.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != unit.originalUnit.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != iskillEffectListUnit.originalUnit.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != unit.originalUnit.job.ID || num1 != effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target))
        return false;
      BattleFuncs.PackedSkillEffect packedSkillEffect = effect.effect.GetPackedSkillEffect();
      BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(iskillEffectListUnit);
      if (unitPosition != null)
      {
        BL.Panel panel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
        if (!packedSkillEffect.CheckLandTag(panel, isAI))
          return false;
      }
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.level_up_status_type))
      {
        int statusType1 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
        if (statusType1 != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(iskillEffectListUnit, statusType1);
          Decimal num2 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
          Decimal num3 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
          if (num2 != 0M && levelUpStatusRatio < num2 || num3 != 0M && levelUpStatusRatio >= num3)
            return false;
        }
        int statusType2 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_level_up_status_type);
        if (statusType2 != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType2);
          Decimal num4 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_min);
          Decimal num5 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_max);
          if (num4 != 0M && levelUpStatusRatio < num4 || num5 != 0M && levelUpStatusRatio >= num5)
            return false;
        }
      }
      return true;
    }

    public static IEnumerable<BL.SkillEffect> GetEnabledCharismaPanelBuffDebuff(
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BL.Panel panel,
      bool isAI)
    {
      return panel.getSkillEffects(isAI).value.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effect.effect_logic.Enum == e && Judgement.CheckEnabledCharismaPanelBuffDebuff(effect, unit, target, isAI)));
    }

    public static bool CheckEnabledCharismaPanelBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      bool isAI)
    {
      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitToISkillEffectListUnit(effect.parentUnit, isAI);
      if (iskillEffectListUnit == unit || iskillEffectListUnit == target || unit.originalUnit.isFacility || iskillEffectListUnit.IsJumping)
        return false;
      int num1 = BattleFuncs.getForceID(unit.originalUnit) == BattleFuncs.getForceID(iskillEffectListUnit.originalUnit) ? 0 : 1;
      if (BattleFuncs.isSealedSkillEffect(iskillEffectListUnit, effect) || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != iskillEffectListUnit.originalUnit.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != unit.originalUnit.playerUnit.GetElement() || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != iskillEffectListUnit.originalUnit.unit.kind.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) != unit.originalUnit.job.ID || num1 != effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) || iskillEffectListUnit == target && BattleFuncs.isSkillsAndEffectsInvalid(target, unit) || BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) || BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return false;
      BattleFuncs.PackedSkillEffect packedSkillEffect = effect.effect.GetPackedSkillEffect();
      BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(iskillEffectListUnit);
      if (unitPosition != null)
      {
        BL.Panel panel = BattleFuncs.getPanel(unitPosition.row, unitPosition.column);
        if (!packedSkillEffect.CheckLandTag(panel, isAI))
          return false;
      }
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.level_up_status_type))
      {
        int statusType1 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
        if (statusType1 != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(iskillEffectListUnit, statusType1);
          Decimal num2 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
          Decimal num3 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
          if (num2 != 0M && levelUpStatusRatio < num2 || num3 != 0M && levelUpStatusRatio >= num3)
            return false;
        }
        int statusType2 = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_level_up_status_type);
        if (statusType2 != 0)
        {
          Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType2);
          Decimal num4 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_min);
          Decimal num5 = (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.target_level_up_status_max);
          if (num4 != 0M && levelUpStatusRatio < num4 || num5 != 0M && levelUpStatusRatio >= num5)
            return false;
        }
      }
      return true;
    }

    public static IEnumerable<BL.SkillEffect> GetEnabledCharismaPanelBuffDebuffByCheckInvokeGeneric(
      BattleskillEffectLogicEnum e,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BL.Panel panel,
      bool isAI,
      int? colosseumTurn,
      int attackType,
      int? unitHp)
    {
      return panel.getSkillEffects(isAI).value.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (effect => effect.effect.effect_logic.Enum == e && Judgement.CheckEnabledCharismaPanelBuffDebuffByCheckInvokeGeneric(effect, unit, target, panel, isAI, colosseumTurn, attackType, unitHp)));
    }

    public static bool CheckEnabledCharismaPanelBuffDebuffByCheckInvokeGeneric(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BL.Panel panel,
      bool isAI,
      int? colosseumTurn,
      int attackType,
      int? unitHp)
    {
      BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitToISkillEffectListUnit(effect.parentUnit, isAI);
      if (iskillEffectListUnit == unit || iskillEffectListUnit == target || unit.originalUnit.isFacility || iskillEffectListUnit.IsJumping)
        return false;
      int num1 = BattleFuncs.getForceID(unit.originalUnit) == BattleFuncs.getForceID(iskillEffectListUnit.originalUnit) ? 0 : 1;
      BL.UnitPosition unitPosition = BattleFuncs.iSkillEffectListUnitToUnitPosition(iskillEffectListUnit);
      BL.Panel panel1 = unitPosition != null ? BattleFuncs.getPanel(unitPosition.row, unitPosition.column) : (BL.Panel) null;
      int num2 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target);
      return num1 == num2 && effect.GetCheckInvokeGeneric().DoCheck(iskillEffectListUnit, unit, colosseumTurn, targetHp: unitHp, attackType: attackType, unitPanel: panel1, targetPanel: panel, effect: effect) && (iskillEffectListUnit != target || !BattleFuncs.isSkillsAndEffectsInvalid(target, unit)) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static BL.SkillEffect GetEnabledCavalryRushBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      int distance)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledCavalryRushBuffDebuff(x, unit, distance))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => Mathf.Abs(distance - effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range)))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledCavalryRushBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      int distance)
    {
      switch (!effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.is_attack_nc) ? 1 : effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc))
      {
        case 0:
        case 1:
          return !BattleFuncs.isSealedSkillEffect(unit, effect);
        default:
          return false;
      }
    }

    public static BL.SkillEffect GetEnabledCavalryRushBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int distance,
      int attackType)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledCavalryRushBuffDebuff(x, unit, target, distance, attackType))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => Mathf.Abs(distance - effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.range)))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledCavalryRushBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int distance,
      int attackType)
    {
      int num = !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.is_attack_nc) ? 1 : effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack_nc);
      return (num == 0 || num == attackType) && !BattleFuncs.isSealedSkillEffect(unit, effect) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledCavalryRushBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return true;
    }

    public static bool CheckEnabledRaidMissionBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit effectUnit,
      int effectTarget,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      return !BattleFuncs.isSealedSkillEffect(effectUnit, effect) && effectTarget == effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) && (effectUnit != target || !BattleFuncs.isSkillsAndEffectsInvalid(target, unit)) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledRaidMissionBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return true;
    }

    public static BL.SkillEffect GetEnabledExtremeOfForceBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledExtremeOfForceBuffDebuff(x, unit))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => Mathf.Abs(effect.killCount - effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.kill_count)))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledExtremeOfForceBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect);
    }

    public static BL.SkillEffect GetEnabledExtremeOfForceBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledExtremeOfForceBuffDebuff(x, unit, target))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => Mathf.Abs(effect.killCount - effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.kill_count)))).ThenByDescending<BL.SkillEffect, float>((Func<BL.SkillEffect, float>) (effect =>
      {
        if (effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.value))
          return effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.value);
        return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.percentage) ? 0.0f : effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
      })).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledExtremeOfForceBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledExtremeOfForceBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return true;
    }

    public static BL.SkillEffect GetEnabledOnemanChargeBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ForceID[] targetForceId,
      BL.Panel panel,
      bool isAI)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledOnemanChargeBuffDebuff(x, unit, targetForceId, panel, isAI))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledOnemanChargeBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ForceID[] targetForceId,
      BL.Panel panel,
      bool isAI)
    {
      if (BattleFuncs.isSealedSkillEffect(unit, effect))
        return false;
      int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.search_target);
      int[] range = new int[2]
      {
        effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
        effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
      };
      int num2 = 0;
      Judgement.OnemanChargeSearchTargetCheck ocstc = new Judgement.OnemanChargeSearchTargetCheck(effect);
      if (num1 == 0 || num1 == 2)
        num2 += BattleFuncs.getTargets(panel.row, panel.column, range, new BL.ForceID[1]
        {
          BattleFuncs.getForceID(unit.originalUnit)
        }, BL.Unit.TargetAttribute.all, (isAI ? 1 : 0) != 0, nonFacility: true).Count<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x =>
        {
          BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(x);
          return iskillEffectListUnit != unit && ocstc.DoCheck(iskillEffectListUnit);
        }));
      if (num1 == 1 || num1 == 2)
        num2 += BattleFuncs.getTargets(panel.row, panel.column, range, targetForceId, BL.Unit.TargetAttribute.all, isAI, nonFacility: true).Count<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => ocstc.DoCheck(BattleFuncs.unitPositionToISkillEffectListUnit(x))));
      return num2 <= effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_unit_count) && num2 >= effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_unit_count);
    }

    public static BL.SkillEffect GetEnabledOnemanChargeBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BL.ForceID[] targetForceId,
      BL.Panel panel,
      bool isAI)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledOnemanChargeBuffDebuff(x, unit, target, targetForceId, panel, isAI))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledOnemanChargeBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BL.ForceID[] targetForceId,
      BL.Panel panel,
      bool isAI)
    {
      if (BattleFuncs.isSealedSkillEffect(unit, effect))
        return false;
      int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.search_target);
      int[] range = new int[2]
      {
        effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
        effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
      };
      int num2 = 0;
      Judgement.OnemanChargeSearchTargetCheck ocstc = new Judgement.OnemanChargeSearchTargetCheck(effect);
      if (num1 == 0 || num1 == 2)
        num2 += BattleFuncs.getTargets(panel.row, panel.column, range, new BL.ForceID[1]
        {
          BattleFuncs.getForceID(unit.originalUnit)
        }, BL.Unit.TargetAttribute.all, (isAI ? 1 : 0) != 0, nonFacility: true).Count<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x =>
        {
          BL.ISkillEffectListUnit iskillEffectListUnit = BattleFuncs.unitPositionToISkillEffectListUnit(x);
          return iskillEffectListUnit != unit && ocstc.DoCheck(iskillEffectListUnit);
        }));
      if (num1 == 1 || num1 == 2)
        num2 += BattleFuncs.getTargets(panel.row, panel.column, range, targetForceId, BL.Unit.TargetAttribute.all, isAI, nonFacility: true).Count<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => ocstc.DoCheck(BattleFuncs.unitPositionToISkillEffectListUnit(x))));
      return num2 <= effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_unit_count) && num2 >= effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_unit_count) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledOnemanChargeBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return true;
    }

    public static bool CheckEnabledInOutSideBattleBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0;
    }

    public static bool CheckEnabledInOutSideBattleBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == target.originalUnit.job.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledInOutSideBattleBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) != unit.originalUnit.job.ID)
        return false;
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id));
    }

    public static bool CheckEnabledEvenIllusionBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      int turnCount)
    {
      int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn);
      int num2 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.turn_cycle);
      if (num2 == 0)
        num2 = 1;
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && turnCount >= num1 && (turnCount - num1) % num2 == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0;
    }

    public static bool CheckEnabledEvenIllusionBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      int absoluteTurnCount = BattleFuncs.getPhaseState().absoluteTurnCount;
      int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn);
      int num2 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.turn_cycle);
      if (num2 == 0)
        num2 = 1;
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && absoluteTurnCount >= num1 && (absoluteTurnCount - num1) % num2 == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == target.originalUnit.job.ID) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledEvenIllusionBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement())
        return false;
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == unit.originalUnit.job.ID;
    }

    public static bool CheckEnabledSpecificUnitBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      bool isHp)
    {
      if (!isHp && BattleFuncs.isSealedSkillEffect(unit, effect) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) != unit.originalUnit.unit.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_id) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) != unit.originalUnit.unit.character.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_character_id) != 0 || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.unit_type_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_type_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_type_id) != unit.originalUnit.playerUnit.unit_type.ID)
        return false;
      return !effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_unit_type_id) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_type_id) == 0;
    }

    public static bool CheckEnabledSpecificUnitBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      bool isHp)
    {
      if (!isHp && BattleFuncs.isSealedSkillEffect(unit, effect) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) != unit.originalUnit.unit.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_id) != target.originalUnit.unit.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) != unit.originalUnit.unit.character.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_character_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_character_id) != target.originalUnit.unit.character.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.unit_type_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_type_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_type_id) != unit.originalUnit.playerUnit.unit_type.ID || effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_unit_type_id) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_type_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_unit_type_id) != target.originalUnit.playerUnit.unit_type.ID || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target))
        return false;
      return isHp || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledSpecificUnitBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) != unit.originalUnit.unit.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.character_id) != unit.originalUnit.unit.character.ID)
        return false;
      return !effect.HasKey(BattleskillEffectLogicArgumentEnum.unit_type_id) || effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_type_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_type_id) == unit.originalUnit.playerUnit.unit_type.ID;
    }

    public static BL.SkillEffect GetEnabledUnitRarityBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledUnitRarityBuffDebuff(x, unit))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledUnitRarityBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_rarity) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_rarity) <= unit.originalUnit.unit.rarity.index + 1) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_min_rarity) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_rarity) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_rarity) >= unit.originalUnit.unit.rarity.index + 1) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_max_rarity) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0;
    }

    public static BL.SkillEffect GetEnabledUnitRarityBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledUnitRarityBuffDebuff(x, unit, target))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledUnitRarityBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_rarity) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_rarity) <= unit.originalUnit.unit.rarity.index + 1) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_min_rarity) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_min_rarity) <= target.originalUnit.unit.rarity.index + 1) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_rarity) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_rarity) >= unit.originalUnit.unit.rarity.index + 1) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_max_rarity) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_max_rarity) >= target.originalUnit.unit.rarity.index + 1) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledUnitRarityBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.min_rarity) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.min_rarity) > unit.originalUnit.unit.rarity.index + 1 || effect.GetInt(BattleskillEffectLogicArgumentEnum.max_rarity) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.max_rarity) < unit.originalUnit.unit.rarity.index + 1 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID)
        return false;
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
    }

    public static bool CheckEnabledEquipGearBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (BattleFuncs.isSealedSkillEffect(unit, effect) || !BattleFuncs.isGearEquipped(unit.originalUnit.playerUnit, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.equip_gear_king_id)) || !BattleFuncs.isGearModelEquipped(unit.originalUnit.playerUnit, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.equip_gear_model_king_id)))
        return false;
      return effect.unit == (BL.Unit) null || effect.effect.skill.skill_type != BattleskillSkillType.leader;
    }

    public static bool CheckEnabledEquipGearBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && BattleFuncs.isGearEquipped(unit.originalUnit.playerUnit, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.equip_gear_king_id)) && BattleFuncs.isGearModelEquipped(unit.originalUnit.playerUnit, effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.equip_gear_model_king_id)) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledEquipGearBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return BattleFuncs.isGearEquipped(unit.originalUnit.playerUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.equip_gear_king_id)) && BattleFuncs.isGearModelEquipped(unit.originalUnit.playerUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.equip_gear_model_king_id));
    }

    public static BL.SkillEffect GetEnabledDeadCountBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      int turnCount,
      IEnumerable<BL.Unit> unitDeckUnits,
      IEnumerable<BL.Unit> targetDeckUnits,
      BattleskillInvokeGameModeEnum gameMode,
      bool isAI)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledDeadCountBuffDebuff(x, unit, gameMode))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect =>
      {
        int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.turn_range);
        int unitId = effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.unit_id) ? effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) : 0;
        int borderTurn = turnCount - (num1 - 1);
        int num2 = 0;
        if (unitDeckUnits != null)
        {
          IEnumerable<BL.Unit> source = unitDeckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (y => unitId == 0 || unitId == y.unit.ID));
          if (num1 == 0)
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u => u.deadCount));
          else
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u => u.deadTurn.Count<int>((Func<int, bool>) (t => t >= borderTurn))));
          if (isAI)
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u =>
            {
              BL.AIUnit aiUnit = BattleFuncs.getEnv().getAIUnit(u);
              return aiUnit != null && !aiUnit.originalUnit.isDead ? aiUnit.deadCount : 0;
            }));
        }
        if (targetDeckUnits != null)
        {
          IEnumerable<BL.Unit> source = targetDeckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (y => unitId == 0 || unitId == y.unit.ID));
          if (num1 == 0)
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u => u.deadCount));
          else
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u => u.deadTurn.Count<int>((Func<int, bool>) (t => t >= borderTurn))));
          if (isAI)
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u =>
            {
              BL.AIUnit aiUnit = BattleFuncs.getEnv().getAIUnit(u);
              return aiUnit != null && !aiUnit.originalUnit.isDead ? aiUnit.deadCount : 0;
            }));
        }
        return Mathf.Abs(num2 - effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.dead_count));
      })).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledDeadCountBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BattleskillInvokeGameModeEnum gameMode)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && (!effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0) && effect.effect.isEnableGameMode(gameMode, unit);
    }

    public static BL.SkillEffect GetEnabledDeadCountBuffDebuff(
      List<BL.SkillEffect> effects,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int turnCount,
      IEnumerable<BL.Unit> unitDeckUnits,
      IEnumerable<BL.Unit> targetDeckUnits,
      BattleskillInvokeGameModeEnum gameMode,
      bool isAI)
    {
      return effects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => Judgement.CheckEnabledDeadCountBuffDebuff(x, unit, target, gameMode))).OrderBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect =>
      {
        int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.turn_range);
        int unitId = effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.unit_id) ? effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.unit_id) : 0;
        int borderTurn = turnCount - (num1 - 1);
        int num2 = 0;
        if (unitDeckUnits != null)
        {
          IEnumerable<BL.Unit> source = unitDeckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (y => unitId == 0 || unitId == y.unit.ID));
          if (num1 == 0)
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u => u.deadCount));
          else
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u => u.deadTurn.Count<int>((Func<int, bool>) (t => t >= borderTurn))));
          if (isAI)
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u =>
            {
              BL.AIUnit aiUnit = BattleFuncs.getEnv().getAIUnit(u);
              return aiUnit != null && !aiUnit.originalUnit.isDead ? aiUnit.deadCount : 0;
            }));
        }
        if (targetDeckUnits != null)
        {
          IEnumerable<BL.Unit> source = targetDeckUnits.Where<BL.Unit>((Func<BL.Unit, bool>) (y => unitId == 0 || unitId == y.unit.ID));
          if (num1 == 0)
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u => u.deadCount));
          else
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u => u.deadTurn.Count<int>((Func<int, bool>) (t => t >= borderTurn))));
          if (isAI)
            num2 += source.Sum<BL.Unit>((Func<BL.Unit, int>) (u =>
            {
              BL.AIUnit aiUnit = BattleFuncs.getEnv().getAIUnit(u);
              return aiUnit != null && !aiUnit.originalUnit.isDead ? aiUnit.deadCount : 0;
            }));
        }
        return Mathf.Abs(num2 - effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.dead_count));
      })).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (effect => effect.effectId)).FirstOrDefault<BL.SkillEffect>();
    }

    public static bool CheckEnabledDeadCountBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BattleskillInvokeGameModeEnum gameMode)
    {
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && (!effect.effect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == target.originalUnit.job.ID) && effect.effect.isEnableGameMode(gameMode, unit) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledDeadCountBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit,
      BattleskillInvokeGameModeEnum gameMode)
    {
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.isEnableGameMode(gameMode, unit);
    }

    public static bool CheckEnabledSpecificGroupBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      bool isHp)
    {
      if (!isHp && BattleFuncs.isSealedSkillEffect(unit, effect) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != 0 && (unit.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != unit.originalUnit.unitGroup.group_large_category_id.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != 0 && (unit.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != unit.originalUnit.unitGroup.group_small_category_id.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != 0 && (unit.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id.ID && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id_2.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != 0 && (unit.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != unit.originalUnit.unitGroup.group_generation_category_id.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id) != 0)
        return false;
      BattleFuncs.PackedSkillEffect packedSkillEffect = effect.effect.GetPackedSkillEffect();
      return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 ? 1 : 0)) != 0;
    }

    public static bool CheckEnabledSpecificGroupBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      bool isHp)
    {
      if (!isHp && BattleFuncs.isSealedSkillEffect(unit, effect) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != 0 && (unit.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != unit.originalUnit.unitGroup.group_large_category_id.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id) != 0 && (target.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id) != target.originalUnit.unitGroup.group_large_category_id.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != 0 && (unit.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != unit.originalUnit.unitGroup.group_small_category_id.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id) != 0 && (target.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id) != target.originalUnit.unitGroup.group_small_category_id.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != 0 && (unit.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id.ID && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id_2.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != 0 && (target.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != target.originalUnit.unitGroup.group_clothing_category_id.ID && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != target.originalUnit.unitGroup.group_clothing_category_id_2.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != 0 && (unit.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != unit.originalUnit.unitGroup.group_generation_category_id.ID) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id) != 0 && (target.originalUnit.unitGroup == null || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id) != target.originalUnit.unitGroup.group_generation_category_id.ID) || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) || !isHp && BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return false;
      BattleFuncs.PackedSkillEffect packedSkillEffect = effect.effect.GetPackedSkillEffect();
      return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 ? 1 : ((CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement() ? 1 : 0))) != 0;
    }

    public static bool CheckEnabledSpecificGroupBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != 0 && (unit.originalUnit.unitGroup == null || effect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != unit.originalUnit.unitGroup.group_large_category_id.ID) || effect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != 0 && (unit.originalUnit.unitGroup == null || effect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != unit.originalUnit.unitGroup.group_small_category_id.ID) || effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != 0 && (unit.originalUnit.unitGroup == null || effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id.ID && effect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id_2.ID) || effect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != 0 && (unit.originalUnit.unitGroup == null || effect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != unit.originalUnit.unitGroup.group_generation_category_id.ID))
        return false;
      BattleFuncs.PackedSkillEffect packedSkillEffect = effect.GetPackedSkillEffect();
      return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID ? (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 ? 1 : ((CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement() ? 1 : 0)) : 0) != 0;
    }

    public static bool CheckEnabledSpecificSkillGroupBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      bool isHp)
    {
      return (isHp || !BattleFuncs.isSealedSkillEffect(unit, effect)) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !unit.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id))) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_group_id) == 0;
    }

    public static bool CheckEnabledSpecificSkillGroupBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      bool isHeal,
      bool isHp)
    {
      if (!isHp && BattleFuncs.isSealedSkillEffect(unit, effect) || isHeal && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_heal) != 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) != 0 && !unit.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id)) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) != 0 && unit.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id)) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement() || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_group_id) != 0 && !target.originalUnit.unit.HasSkillGroupId(effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_skill_group_id)) || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target))
        return false;
      return isHp || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledSpecificSkillGroupBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) != 0 && !unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id)))
        return false;
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id) == 0 || !unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.exclude_skill_group_id));
    }

    public static bool CheckEnabledEnemyBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType,
      BattleskillInvokeGameModeEnum gameMode)
    {
      BL.ISkillEffectListUnit unit1 = target;
      BL.ISkillEffectListUnit skillEffectListUnit = unit;
      switch (attackType)
      {
        case 1:
          attackType = 2;
          break;
        case 2:
          attackType = 1;
          break;
      }
      BattleskillEffect effect = x.effect;
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit1.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit1.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || skillEffectListUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == skillEffectListUnit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == attackType) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit1.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == skillEffectListUnit.originalUnit.playerUnit.GetElement()) && !BattleFuncs.isSealedSkillEffect(unit1, x) && effect.isEnableGameMode(gameMode, unit1) && !BattleFuncs.isSkillsAndEffectsInvalid(target, unit) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledEnemyBuffDebuff(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit,
      BattleskillInvokeGameModeEnum gameMode)
    {
      BL.ISkillEffectListUnit unit1 = unit;
      BattleskillEffect battleskillEffect = x;
      return (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit1.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit1.originalUnit.unit.kind.ID) && (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit1.originalUnit.playerUnit.GetElement()) && battleskillEffect.isEnableGameMode(gameMode, unit1);
    }

    public static bool CheckEnabledParamDiffBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      Judgement.NonBattleParameter.FromPlayerUnitCache nbpCache,
      int selfHp)
    {
      BattleskillEffect effect = x.effect;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type) != 0 || BattleFuncs.isSealedSkillEffect(unit, x) || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0)
        return false;
      int num = -BattleFuncs.GetParamDiffValue(effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type), nbpCache, selfHp);
      return num >= effect.GetInt(BattleskillEffectLogicArgumentEnum.min_value) && num <= effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value);
    }

    public static bool CheckEnabledParamDiffBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      Judgement.BeforeDuelUnitParameter.FromBeUnitWork work,
      int selfHp,
      int targetHp)
    {
      BattleskillEffect effect = x.effect;
      if (BattleFuncs.isSealedSkillEffect(unit, x) || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement() || BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target) || BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return false;
      int paramDiffValue = BattleFuncs.GetParamDiffValue(effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type), work.self.nbpCache, selfHp);
      int num = BattleFuncs.GetParamDiffValue(effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type), work.target.nbpCache, targetHp) - paramDiffValue;
      return num >= effect.GetInt(BattleskillEffectLogicArgumentEnum.min_value) && num <= effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value);
    }

    public static bool CheckEnabledParamDiffBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
    }

    public static bool CheckEnabledParamDiffEnemyBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      Judgement.BeforeDuelUnitParameter.FromBeUnitWork work,
      int selfHp,
      int targetHp)
    {
      BL.ISkillEffectListUnit unit1 = target;
      BL.ISkillEffectListUnit skillEffectListUnit = unit;
      BattleskillEffect effect = x.effect;
      if (BattleFuncs.isSealedSkillEffect(unit1, x) || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit1.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != skillEffectListUnit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !skillEffectListUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != skillEffectListUnit.originalUnit.playerUnit.GetElement() || BattleFuncs.isSkillsAndEffectsInvalid(target, unit) || BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return false;
      int paramDiffValue = BattleFuncs.GetParamDiffValue(effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type), work.target.nbpCache, targetHp);
      int num = BattleFuncs.GetParamDiffValue(effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type), work.self.nbpCache, selfHp) - paramDiffValue;
      return num >= effect.GetInt(BattleskillEffectLogicArgumentEnum.min_value) && num <= effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value);
    }

    public static bool CheckEnabledAttackClassBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      GearGear equippedGearOrInitial = unit.originalUnit.playerUnit.equippedGearOrInitial;
      GearAttackClassification attackClassification = equippedGearOrInitial.hasAttackClass ? equippedGearOrInitial.gearClassification.attack_classification : unit.originalUnit.playerUnit.initial_gear.gearClassification.attack_classification;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_classification_id) != 0 && (GearAttackClassification) effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_classification_id) != attackClassification || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_attack_classification_id) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0)
        return false;
      return isHp || !BattleFuncs.isSealedSkillEffect(unit, x);
    }

    public static bool CheckEnabledAttackClassBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      GearGear equippedGearOrInitial1 = unit.originalUnit.playerUnit.equippedGearOrInitial;
      GearAttackClassification attackClassification1 = equippedGearOrInitial1.hasAttackClass ? equippedGearOrInitial1.gearClassification.attack_classification : unit.originalUnit.playerUnit.initial_gear.gearClassification.attack_classification;
      GearGear equippedGearOrInitial2 = target.originalUnit.playerUnit.equippedGearOrInitial;
      GearAttackClassification attackClassification2 = equippedGearOrInitial2.hasAttackClass ? equippedGearOrInitial2.gearClassification.attack_classification : target.originalUnit.playerUnit.initial_gear.gearClassification.attack_classification;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_classification_id) != 0 && (GearAttackClassification) effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_classification_id) != attackClassification1 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_attack_classification_id) != 0 && (GearAttackClassification) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_attack_classification_id) != attackClassification2 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || !isHp && BattleFuncs.isSealedSkillEffect(unit, x) || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target))
        return false;
      return isHp || BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledAttackClassBuffDebuff(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect battleskillEffect = x;
      GearGear equippedGearOrInitial = unit.originalUnit.playerUnit.equippedGearOrInitial;
      GearAttackClassification attackClassification = equippedGearOrInitial.hasAttackClass ? equippedGearOrInitial.gearClassification.attack_classification : unit.originalUnit.playerUnit.initial_gear.gearClassification.attack_classification;
      if (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_classification_id) != 0 && (GearAttackClassification) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_classification_id) != attackClassification || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement())
        return false;
      return battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id));
    }

    public static bool CheckEnabledAttackElementBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      CommonElement attachedElement = unit.originalUnit.playerUnit.equippedGearOrInitial.attachedElement;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_element_id) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_element_id) != attachedElement || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_attack_element_id) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0)
        return false;
      return isHp || !BattleFuncs.isSealedSkillEffect(unit, x);
    }

    public static bool CheckEnabledAttackElementBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      CommonElement attachedElement1 = unit.originalUnit.playerUnit.equippedGearOrInitial.attachedElement;
      CommonElement attachedElement2 = target.originalUnit.playerUnit.equippedGearOrInitial.attachedElement;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_element_id) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_element_id) != attachedElement1 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_attack_element_id) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_attack_element_id) != attachedElement2 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement() || effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || !isHp && BattleFuncs.isSealedSkillEffect(unit, x) || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target))
        return false;
      return isHp || BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledAttackElementBuffDebuff(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect battleskillEffect = x;
      CommonElement attachedElement = unit.originalUnit.playerUnit.equippedGearOrInitial.attachedElement;
      if (battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_element_id) != 0 && (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_element_id) != attachedElement || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement())
        return false;
      return battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id));
    }

    public static bool CheckEnabledInvestLogicBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func = !pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) ? (Func<bool>) null : (Func<bool>) (() =>
      {
        if (pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 || pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0)
          return false;
        return pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
      });
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 && (func == null || func()) && effect.GetInt(BattleskillEffectLogicArgumentEnum.condition_target) == 0 && BattleFuncs.checkSkillLogicInvest(unit, (BL.ISkillEffectListUnit) null, effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type), effect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type), effect.GetInt(BattleskillEffectLogicArgumentEnum.condition_target)) && !BattleFuncs.isSealedSkillEffect(unit, x);
    }

    public static bool CheckEnabledInvestLogicBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType)
    {
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func = !pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) ? (Func<bool>) null : (Func<bool>) (() =>
      {
        if (pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement())
          return false;
        return pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
      });
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == attackType) && (func == null || func()) && BattleFuncs.checkSkillLogicInvest(unit, target, effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type), effect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type), effect.GetInt(BattleskillEffectLogicArgumentEnum.condition_target)) && !BattleFuncs.isSealedSkillEffect(unit, x) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledInvestLogicBuffDebuff(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleFuncs.PackedSkillEffect packedSkillEffect = x.GetPackedSkillEffect();
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID)
        return false;
      return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
    }

    public static bool CheckEnabledEnemyInvestLogicBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType)
    {
      BL.ISkillEffectListUnit effectUnit = target;
      BL.ISkillEffectListUnit targetUnit = unit;
      switch (attackType)
      {
        case 1:
          attackType = 2;
          break;
        case 2:
          attackType = 1;
          break;
      }
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func = !pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) ? (Func<bool>) null : (Func<bool>) (() =>
      {
        if (pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != targetUnit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != targetUnit.originalUnit.playerUnit.GetElement())
          return false;
        return pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == effectUnit.originalUnit.playerUnit.GetElement();
      });
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == attackType) && (func == null || func()) && BattleFuncs.checkSkillLogicInvest(effectUnit, targetUnit, effect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id), effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type), effect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type), effect.GetInt(BattleskillEffectLogicArgumentEnum.condition_target)) && !BattleFuncs.isSealedSkillEffect(effectUnit, x) && !BattleFuncs.isSkillsAndEffectsInvalid(target, unit) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledEnemyInvestLogicBuffDebuff(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleFuncs.PackedSkillEffect packedSkillEffect = x.GetPackedSkillEffect();
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID)
        return false;
      return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
    }

    public static bool CheckEnabledEvenIllusion2BuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      int turnCount,
      BattleskillInvokeGameModeEnum gameMode)
    {
      int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn);
      int num2 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.end_turn);
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && turnCount >= num1 && (num2 == 0 || turnCount < num2) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && effect.effect.isEnableGameMode(gameMode, unit);
    }

    public static bool CheckEnabledEvenIllusion2BuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int turnCount,
      BattleskillInvokeGameModeEnum gameMode)
    {
      int num1 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn);
      int num2 = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.end_turn);
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && turnCount >= num1 && (num2 == 0 || turnCount < num2) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && effect.effect.isEnableGameMode(gameMode, unit) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledEvenIllusion2BuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit,
      BattleskillInvokeGameModeEnum gameMode)
    {
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.isEnableGameMode(gameMode, unit);
    }

    public static bool CheckEnabledEvenIllusion3BuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      int turnCount,
      BattleskillInvokeGameModeEnum gameMode)
    {
      int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn2);
      if (gameMode == BattleskillInvokeGameModeEnum.colosseum)
        ++num;
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && turnCount - effect.investTurn >= num && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && effect.effect.isEnableGameMode(gameMode, unit);
    }

    public static bool CheckEnabledEvenIllusion3BuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int turnCount,
      BattleskillInvokeGameModeEnum gameMode)
    {
      int num = effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.start_turn2);
      if (gameMode == BattleskillInvokeGameModeEnum.colosseum)
        ++num;
      return !BattleFuncs.isSealedSkillEffect(unit, effect) && turnCount - effect.investTurn >= num && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && effect.effect.isEnableGameMode(gameMode, unit) && !BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledEvenIllusion3BuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit,
      BattleskillInvokeGameModeEnum gameMode)
    {
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.isEnableGameMode(gameMode, unit);
    }

    public static bool CheckEnabledPeculiarParameterRangeBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func = !pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) ? (Func<bool>) null : (Func<bool>) (() =>
      {
        if (pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 || pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0)
          return false;
        return pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
      });
      int paramType = effect.GetInt(BattleskillEffectLogicArgumentEnum.peculiar_parameter_type);
      if (paramType != 0)
      {
        float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(unit, paramType);
        if ((double) peculiarParameterValue < (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_min) || (double) peculiarParameterValue > (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_max))
          return false;
      }
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_type) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 && (func == null || func()) && !BattleFuncs.isSealedSkillEffect(unit, x);
    }

    public static bool CheckEnabledPeculiarParameterRangeBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType)
    {
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func = !pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) ? (Func<bool>) null : (Func<bool>) (() =>
      {
        if (pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement())
          return false;
        return pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
      });
      int paramType1 = effect.GetInt(BattleskillEffectLogicArgumentEnum.peculiar_parameter_type);
      if (paramType1 != 0)
      {
        float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(unit, paramType1);
        if ((double) peculiarParameterValue < (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_min) || (double) peculiarParameterValue > (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_max))
          return false;
      }
      int paramType2 = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_type);
      if (paramType2 != 0)
      {
        float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(target, paramType2);
        if ((double) peculiarParameterValue < (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_min) || (double) peculiarParameterValue > (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_max))
          return false;
      }
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == attackType) && (func == null || func()) && !BattleFuncs.isSealedSkillEffect(unit, x) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledPeculiarParameterRangeBuffDebuff(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      int paramType = x.GetInt(BattleskillEffectLogicArgumentEnum.peculiar_parameter_type);
      if (paramType != 0)
      {
        float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(unit, paramType);
        if ((double) peculiarParameterValue < (double) x.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_min) || (double) peculiarParameterValue > (double) x.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_max))
          return false;
      }
      BattleFuncs.PackedSkillEffect packedSkillEffect = x.GetPackedSkillEffect();
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID)
        return false;
      return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
    }

    public static bool CheckEnabledEnemyPeculiarParameterRangeBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType)
    {
      BL.ISkillEffectListUnit effectUnit = target;
      BL.ISkillEffectListUnit targetUnit = unit;
      switch (attackType)
      {
        case 1:
          attackType = 2;
          break;
        case 2:
          attackType = 1;
          break;
      }
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func = !pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) ? (Func<bool>) null : (Func<bool>) (() =>
      {
        if (pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != targetUnit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != targetUnit.originalUnit.playerUnit.GetElement())
          return false;
        return pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == effectUnit.originalUnit.playerUnit.GetElement();
      });
      int paramType1 = effect.GetInt(BattleskillEffectLogicArgumentEnum.peculiar_parameter_type);
      if (paramType1 != 0)
      {
        float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(effectUnit, paramType1);
        if ((double) peculiarParameterValue < (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_min) || (double) peculiarParameterValue > (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.peculiar_parameter_max))
          return false;
      }
      int paramType2 = effect.GetInt(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_type);
      if (paramType2 != 0)
      {
        float peculiarParameterValue = BattleFuncs.GetPeculiarParameterValue(targetUnit, paramType2);
        if ((double) peculiarParameterValue < (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_min) || (double) peculiarParameterValue > (double) effect.GetFloat(BattleskillEffectLogicArgumentEnum.target_peculiar_parameter_max))
          return false;
      }
      return (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == attackType) && (func == null || func()) && !BattleFuncs.isSealedSkillEffect(effectUnit, x) && !BattleFuncs.isSkillsAndEffectsInvalid(target, unit) && !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledLevelUpStatusBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func = !pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) ? (Func<bool>) null : (Func<bool>) (() =>
      {
        if (pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 || pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0)
          return false;
        return pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
      });
      int statusType = effect.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
      if (statusType != 0)
      {
        Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType);
        Decimal num1 = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
        Decimal num2 = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
        if (num1 != 0M && levelUpStatusRatio < num1 || num2 != 0M && levelUpStatusRatio >= num2)
          return false;
      }
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0 || func != null && !func())
        return false;
      return isHp || !BattleFuncs.isSealedSkillEffect(unit, x);
    }

    public static bool CheckEnabledLevelUpStatusBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType,
      bool isHp)
    {
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func = !pse.HasKey(BattleskillEffectLogicArgumentEnum.target_family_id) ? (Func<bool>) null : (Func<bool>) (() =>
      {
        if (pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !target.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != target.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != 0 && (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) != target.originalUnit.playerUnit.GetElement())
          return false;
        return pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
      });
      int statusType = effect.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
      if (statusType != 0)
      {
        Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType);
        Decimal num1 = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
        Decimal num2 = (Decimal) effect.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
        if (num1 != 0M && levelUpStatusRatio < num1 || num2 != 0M && levelUpStatusRatio >= num2)
          return false;
      }
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != attackType || func != null && !func() || !isHp && BattleFuncs.isSealedSkillEffect(unit, x) || !isHp && BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target))
        return false;
      return isHp || BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledLevelUpStatusBuffDebuff(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      int statusType = x.GetInt(BattleskillEffectLogicArgumentEnum.level_up_status_type);
      if (statusType != 0)
      {
        Decimal levelUpStatusRatio = BattleFuncs.GetLevelUpStatusRatio(unit, statusType);
        Decimal num1 = (Decimal) x.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_min);
        Decimal num2 = (Decimal) x.GetFloat(BattleskillEffectLogicArgumentEnum.level_up_status_max);
        if (num1 != 0M && levelUpStatusRatio < num1 || num2 != 0M && levelUpStatusRatio >= num2)
          return false;
      }
      BattleFuncs.PackedSkillEffect packedSkillEffect = x.GetPackedSkillEffect();
      if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID)
        return false;
      return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement();
    }

    private static BL.SkillEffect SelectUseRemainSkillEffectOne<T>(
      List<BL.SkillEffect> effects,
      Func<BL.SkillEffect, T> calcFunc,
      bool isBuff)
      where T : IComparable
    {
      return effects.Count <= 0 ? (BL.SkillEffect) null : (!isBuff ? effects.OrderBy<BL.SkillEffect, T>((Func<BL.SkillEffect, T>) (x => calcFunc(x))) : effects.OrderByDescending<BL.SkillEffect, T>((Func<BL.SkillEffect, T>) (x => calcFunc(x)))).ThenByDescending<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkill.weight)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.baseSkillId)).ThenBy<BL.SkillEffect, int>((Func<BL.SkillEffect, int>) (x => x.effectId)).First<BL.SkillEffect>();
    }

    private static void ApplyUseRemainSkillEffect(
      Dictionary<int, List<BL.SkillEffect>> useRemainSkillEffects,
      List<BattleFuncs.SkillParam>[] skillParams,
      BL.ISkillEffectListUnit effectUnit,
      Judgement.BeforeDuelUnitParameter r = null)
    {
      foreach (KeyValuePair<int, List<BL.SkillEffect>> remainSkillEffect in useRemainSkillEffects)
      {
        int key = remainSkillEffect.Key;
        List<BL.SkillEffect> skillEffectList = remainSkillEffect.Value;
        List<BL.SkillEffect> effects1 = new List<BL.SkillEffect>();
        List<BL.SkillEffect> effects2 = new List<BL.SkillEffect>();
        List<BL.SkillEffect> effects3 = new List<BL.SkillEffect>();
        List<BL.SkillEffect> effects4 = new List<BL.SkillEffect>();
        Func<BL.SkillEffect, int> calcFunc1 = (Func<BL.SkillEffect, int>) (effect => effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio));
        Func<BL.SkillEffect, float> calcFunc2 = (Func<BL.SkillEffect, float>) (effect => (float) ((Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
        foreach (BL.SkillEffect skillEffect in skillEffectList)
        {
          if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
          {
            int num = calcFunc1(skillEffect);
            if (num > 0)
              effects1.Add(skillEffect);
            else if (num < 0)
              effects2.Add(skillEffect);
          }
          else
          {
            float num = calcFunc2(skillEffect);
            if ((double) num > 1.0)
              effects3.Add(skillEffect);
            else if ((double) num < 1.0)
              effects4.Add(skillEffect);
          }
        }
        BL.SkillEffect effect1 = Judgement.SelectUseRemainSkillEffectOne<int>(effects1, calcFunc1, true);
        BL.SkillEffect effect2 = Judgement.SelectUseRemainSkillEffectOne<int>(effects2, calcFunc1, false);
        BL.SkillEffect effect3 = Judgement.SelectUseRemainSkillEffectOne<float>(effects3, calcFunc2, true);
        BL.SkillEffect effect4 = Judgement.SelectUseRemainSkillEffectOne<float>(effects4, calcFunc2, false);
        List<BattleFuncs.SkillParam> skillParam = skillParams[key];
        if (effect1 != null)
        {
          skillParam.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit.originalUnit, effect1, (float) calcFunc1(effect1)));
          r?.useSkillEffects.Add(BL.UseSkillEffect.Create(effect1, BL.UseSkillEffect.Type.Decrement));
        }
        if (effect2 != null)
        {
          skillParam.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit.originalUnit, effect2, (float) calcFunc1(effect2)));
          r?.useSkillEffects.Add(BL.UseSkillEffect.Create(effect2, BL.UseSkillEffect.Type.Decrement));
        }
        if (effect3 != null)
        {
          skillParam.Add(BattleFuncs.SkillParam.CreateMul(effectUnit.originalUnit, effect3, calcFunc2(effect3)));
          r?.useSkillEffects.Add(BL.UseSkillEffect.Create(effect3, BL.UseSkillEffect.Type.Decrement));
        }
        if (effect4 != null)
        {
          skillParam.Add(BattleFuncs.SkillParam.CreateMul(effectUnit.originalUnit, effect4, calcFunc2(effect4)));
          r?.useSkillEffects.Add(BL.UseSkillEffect.Create(effect4, BL.UseSkillEffect.Type.Decrement));
        }
      }
    }

    public static bool CheckEnabledUseRemainBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleskillEffect effect = x.effect;
      if (x.useRemain.HasValue)
      {
        int? useRemain = x.useRemain;
        int num = 1;
        if (!(useRemain.GetValueOrDefault() >= num & useRemain.HasValue))
          goto label_4;
      }
      if ((effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 && (effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))))
        return !BattleFuncs.isSealedSkillEffect(unit, x);
label_4:
      return false;
    }

    public static bool CheckEnabledUseRemainBuffDebuff(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType)
    {
      BattleskillEffect effect = x.effect;
      if (x.useRemain.HasValue)
      {
        int? useRemain = x.useRemain;
        int num = 1;
        if (!(useRemain.GetValueOrDefault() >= num & useRemain.HasValue))
          goto label_6;
      }
      if ((effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == attackType) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || target.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == target.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == target.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id))) && !BattleFuncs.isSealedSkillEffect(unit, x) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target))
        return BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
label_6:
      return false;
    }

    public static bool CheckEnabledUseRemainBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID || effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) != unit.originalUnit.playerUnit.GetElement())
        return false;
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id) == 0 || unit.originalUnit.unit.HasSkillGroupId(effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_group_id));
    }

    public static bool CheckEnabledBuffDebuffClamp(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.Panel effectPanel,
      bool isAI)
    {
      BattleskillEffect effect = x.effect;
      BattleFuncs.PackedSkillEffect pse = effect.GetPackedSkillEffect();
      Func<bool> func1 = !pse.HasKey(BattleskillEffectLogicArgumentEnum.family_id) ? (Func<bool>) null : (Func<bool>) (() => (pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || unit.originalUnit.playerUnit.HasFamily((UnitFamily) pse.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 && (pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || pse.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == unit.originalUnit.unit.kind.ID) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 && (pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) pse.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement()) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0);
      Func<bool> func2 = !pse.HasKey(BattleskillEffectLogicArgumentEnum.group_large_id) ? (Func<bool>) null : (Func<bool>) (() => (pse.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) == 0 || unit.originalUnit.unitGroup != null && pse.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) == unit.originalUnit.unitGroup.group_large_category_id.ID) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id) == 0 && (pse.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) == 0 || unit.originalUnit.unitGroup != null && pse.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) == unit.originalUnit.unitGroup.group_small_category_id.ID) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id) == 0 && (pse.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) == 0 || unit.originalUnit.unitGroup != null && (pse.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) == unit.originalUnit.unitGroup.group_clothing_category_id.ID || pse.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) == unit.originalUnit.unitGroup.group_clothing_category_id_2.ID)) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) == 0 && (pse.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) == 0 || unit.originalUnit.unitGroup != null && pse.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) == unit.originalUnit.unitGroup.group_generation_category_id.ID) && pse.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id) == 0);
      return effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) == 0 && (func1 == null || func1()) && (func2 == null || func2()) && pse.CheckLandTag(effectPanel, isAI) && !BattleFuncs.isSealedSkillEffect(unit, x);
    }

    public static bool CheckEnabledBuffDebuffClamp(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType,
      BL.Panel effectPanel,
      bool isAI)
    {
      BattleskillEffect effect = x.effect;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != attackType || !Judgement.CheckEnabledBuffDebuffClampExtArg(x, unit, target, effectPanel, isAI) || BattleFuncs.isSealedSkillEffect(unit, x) || BattleFuncs.isEffectEnemyRangeAndInvalid(x, unit, target))
        return false;
      return BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledBuffDebuffClamp(
      BattleskillEffect x,
      BL.ISkillEffectListUnit unit)
    {
      BattleFuncs.PackedSkillEffect packedSkillEffect = x.GetPackedSkillEffect();
      return (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !unit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != unit.originalUnit.unit.kind.ID ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 ? 1 : ((CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == unit.originalUnit.playerUnit.GetElement() ? 1 : 0))) != 0) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.group_large_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != 0 && (unit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != unit.originalUnit.unitGroup.group_large_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != 0 && (unit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != unit.originalUnit.unitGroup.group_small_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != 0 && (unit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id.ID && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != unit.originalUnit.unitGroup.group_clothing_category_id_2.ID) ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) == 0 ? 1 : (unit.originalUnit.unitGroup == null ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) == unit.originalUnit.unitGroup.group_generation_category_id.ID ? 1 : 0)))) != 0);
    }

    public static bool CheckEnabledEnemyBuffDebuffClamp(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int attackType,
      BL.Panel effectPanel,
      bool isAI)
    {
      BL.ISkillEffectListUnit skillEffectListUnit = target;
      BL.ISkillEffectListUnit targetUnit = unit;
      switch (attackType)
      {
        case 1:
          attackType = 2;
          break;
        case 2:
          attackType = 1;
          break;
      }
      BattleskillEffect effect = x.effect;
      if (effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != 0 && effect.GetInt(BattleskillEffectLogicArgumentEnum.is_attack) != attackType || !Judgement.CheckEnabledBuffDebuffClampExtArg(x, skillEffectListUnit, targetUnit, effectPanel, isAI) || BattleFuncs.isSealedSkillEffect(skillEffectListUnit, x) || !BattleFuncs.isBonusSkillId(effect.skill.ID) && BattleFuncs.isSkillsAndEffectsInvalid(target, unit))
        return false;
      return BattleFuncs.isBonusSkillId(effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledBuffDebuffClampExtArg(
      BL.SkillEffect x,
      BL.ISkillEffectListUnit effectUnit,
      BL.ISkillEffectListUnit targetUnit,
      BL.Panel effectPanel,
      bool isAI)
    {
      BattleFuncs.PackedSkillEffect packedSkillEffect = x.effect.GetPackedSkillEffect();
      return (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.family_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) != 0 && !effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.family_id)) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) != 0 && !targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id)) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) != effectUnit.originalUnit.unit.kind.ID || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) != targetUnit.originalUnit.unit.kind.ID || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != 0 && (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) != effectUnit.originalUnit.playerUnit.GetElement() ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 ? 1 : ((CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == targetUnit.originalUnit.playerUnit.GetElement() ? 1 : 0))) != 0) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.group_large_id) || (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != 0 && (effectUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_large_id) != effectUnit.originalUnit.unitGroup.group_large_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id) != 0 && (targetUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_large_id) != targetUnit.originalUnit.unitGroup.group_large_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != 0 && (effectUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_small_id) != effectUnit.originalUnit.unitGroup.group_small_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id) != 0 && (targetUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_small_id) != targetUnit.originalUnit.unitGroup.group_small_category_id.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != 0 && (effectUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != effectUnit.originalUnit.unitGroup.group_clothing_category_id.ID && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_clothing_id) != effectUnit.originalUnit.unitGroup.group_clothing_category_id_2.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != 0 && (targetUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != targetUnit.originalUnit.unitGroup.group_clothing_category_id.ID && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_clothing_id) != targetUnit.originalUnit.unitGroup.group_clothing_category_id_2.ID) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != 0 && (effectUnit.originalUnit.unitGroup == null || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.group_generation_id) != effectUnit.originalUnit.unitGroup.group_generation_category_id.ID) ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id) == 0 ? 1 : (targetUnit.originalUnit.unitGroup == null ? 0 : (packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_group_generation_id) == targetUnit.originalUnit.unitGroup.group_generation_category_id.ID ? 1 : 0)))) != 0) && packedSkillEffect.CheckLandTag(effectPanel, isAI);
    }

    public static bool CheckEnabledGenericBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      int? colosseumTurn,
      Judgement.NonBattleParameter.FromPlayerUnitCache nbpCache,
      int? hp,
      BL.Panel panel,
      bool isHp)
    {
      return effect.GetCheckInvokeGeneric().DoCheck(unit, colosseumTurn: colosseumTurn, unitNbpCache: nbpCache, unitHp: hp, unitPanel: panel, effect: !isHp ? effect : (BL.SkillEffect) null);
    }

    public static bool CheckEnabledGenericBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int? colosseumTurn,
      Judgement.NonBattleParameter.FromPlayerUnitCache unitNbpCache,
      Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache,
      int? unitHp,
      int? targetHp,
      int attackType,
      BL.Panel unitPanel,
      BL.Panel targetPanel,
      bool isHp)
    {
      if (!effect.GetCheckInvokeGeneric().DoCheck(unit, target, colosseumTurn, unitNbpCache, targetNbpCache, unitHp, targetHp, attackType, unitPanel, targetPanel, !isHp ? effect : (BL.SkillEffect) null))
        return false;
      if (isHp)
        return true;
      if (BattleFuncs.isEffectEnemyRangeAndInvalid(effect, unit, target))
        return false;
      return BattleFuncs.isBonusSkillId(effect.effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledGenericBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return effect.GetCheckInvokeGeneric().DoCheckFix(unit, false, true);
    }

    public static bool CheckEnabledEnemyGenericBuffDebuff(
      BL.SkillEffect effect,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      int? colosseumTurn,
      Judgement.NonBattleParameter.FromPlayerUnitCache unitNbpCache,
      Judgement.NonBattleParameter.FromPlayerUnitCache targetNbpCache,
      int? unitHp,
      int? targetHp,
      int attackType,
      BL.Panel unitPanel,
      BL.Panel targetPanel)
    {
      switch (attackType)
      {
        case 1:
          attackType = 2;
          break;
        case 2:
          attackType = 1;
          break;
      }
      if (!effect.GetCheckInvokeGeneric().DoCheck(target, unit, colosseumTurn, targetNbpCache, unitNbpCache, targetHp, unitHp, attackType, targetPanel, unitPanel, effect) || BattleFuncs.isSkillsAndEffectsInvalid(target, unit))
        return false;
      return BattleFuncs.isBonusSkillId(effect.effect.skill.ID) || !BattleFuncs.isSkillsAndEffectsInvalid(unit, target);
    }

    public static bool CheckEnabledEnemyGenericBuffDebuff(
      BattleskillEffect effect,
      BL.ISkillEffectListUnit unit)
    {
      return effect.GetCheckInvokeGeneric().DoCheckFix(unit, false, true);
    }

    public static void GetFixEffectParamValue(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit unit,
      BattleskillEffectLogicEnum logic)
    {
      foreach (Tuple<BL.SkillEffect, int> fixEffectParam in unit.skillEffects.GetFixEffectParams(logic))
      {
        if (!BattleFuncs.isSealedSkillEffect(unit, fixEffectParam.Item1))
          skillParams.Add(BattleFuncs.SkillParam.CreateAdd(unit.originalUnit, fixEffectParam.Item1, (float) fixEffectParam.Item2));
      }
    }

    public static void GetFixEffectParamValue(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BattleskillEffectLogicEnum logic)
    {
      IEnumerable<Tuple<BL.SkillEffect, int>> fixEffectParams = unit.skillEffects.GetFixEffectParams(logic);
      if (fixEffectParams.Any<Tuple<BL.SkillEffect, int>>() && BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return;
      foreach (Tuple<BL.SkillEffect, int> tuple in fixEffectParams)
      {
        if (!BattleFuncs.isSealedSkillEffect(unit, tuple.Item1) && !BattleFuncs.isEffectEnemyRangeAndInvalid(tuple.Item1, unit, target))
          skillParams.Add(BattleFuncs.SkillParam.CreateAdd(unit.originalUnit, tuple.Item1, (float) tuple.Item2));
      }
    }

    public static List<BattleFuncs.SkillParam> GetFixEffectParamValue(
      BL.ISkillEffectListUnit unit,
      BattleskillEffectLogicEnum logic)
    {
      IEnumerable<Tuple<BL.SkillEffect, int>> fixEffectParams = unit.skillEffects.GetFixEffectParams(logic);
      List<BattleFuncs.SkillParam> effectParamValue = (List<BattleFuncs.SkillParam>) null;
      foreach (Tuple<BL.SkillEffect, int> tuple in fixEffectParams)
      {
        if (!BattleFuncs.isSealedSkillEffect(unit, tuple.Item1))
        {
          if (effectParamValue == null)
            effectParamValue = new List<BattleFuncs.SkillParam>();
          effectParamValue.Add(BattleFuncs.SkillParam.CreateAdd(unit.originalUnit, tuple.Item1, (float) tuple.Item2));
        }
      }
      return effectParamValue;
    }

    public static List<BattleFuncs.SkillParam> GetFixEffectParamValue(
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BattleskillEffectLogicEnum logic)
    {
      IEnumerable<Tuple<BL.SkillEffect, int>> fixEffectParams = unit.skillEffects.GetFixEffectParams(logic);
      if (fixEffectParams.Any<Tuple<BL.SkillEffect, int>>() && BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return (List<BattleFuncs.SkillParam>) null;
      List<BattleFuncs.SkillParam> effectParamValue = (List<BattleFuncs.SkillParam>) null;
      foreach (Tuple<BL.SkillEffect, int> tuple in fixEffectParams)
      {
        if (!BattleFuncs.isSealedSkillEffect(unit, tuple.Item1) && !BattleFuncs.isEffectEnemyRangeAndInvalid(tuple.Item1, unit, target))
        {
          if (effectParamValue == null)
            effectParamValue = new List<BattleFuncs.SkillParam>();
          effectParamValue.Add(BattleFuncs.SkillParam.CreateAdd(unit.originalUnit, tuple.Item1, (float) tuple.Item2));
        }
      }
      return effectParamValue;
    }

    public static void GetRatioEffectParamValue(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit unit,
      BattleskillEffectLogicEnum logic)
    {
      foreach (Tuple<BL.SkillEffect, float> ratioEffectParam in unit.skillEffects.GetRatioEffectParams(logic))
      {
        if (!BattleFuncs.isSealedSkillEffect(unit, ratioEffectParam.Item1))
          skillParams.Add(BattleFuncs.SkillParam.CreateMul(unit.originalUnit, ratioEffectParam.Item1, ratioEffectParam.Item2));
      }
    }

    public static void GetRatioEffectParamValue(
      List<BattleFuncs.SkillParam> skillParams,
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BattleskillEffectLogicEnum logic)
    {
      IEnumerable<Tuple<BL.SkillEffect, float>> ratioEffectParams = unit.skillEffects.GetRatioEffectParams(logic);
      if (ratioEffectParams.Any<Tuple<BL.SkillEffect, float>>() && BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return;
      foreach (Tuple<BL.SkillEffect, float> tuple in ratioEffectParams)
      {
        if (!BattleFuncs.isSealedSkillEffect(unit, tuple.Item1) && !BattleFuncs.isEffectEnemyRangeAndInvalid(tuple.Item1, unit, target))
          skillParams.Add(BattleFuncs.SkillParam.CreateMul(unit.originalUnit, tuple.Item1, tuple.Item2));
      }
    }

    public static List<BattleFuncs.SkillParam> GetRatioEffectParamValue(
      BL.ISkillEffectListUnit unit,
      BattleskillEffectLogicEnum logic)
    {
      IEnumerable<Tuple<BL.SkillEffect, float>> ratioEffectParams = unit.skillEffects.GetRatioEffectParams(logic);
      List<BattleFuncs.SkillParam> effectParamValue = (List<BattleFuncs.SkillParam>) null;
      foreach (Tuple<BL.SkillEffect, float> tuple in ratioEffectParams)
      {
        if (!BattleFuncs.isSealedSkillEffect(unit, tuple.Item1))
        {
          if (effectParamValue == null)
            effectParamValue = new List<BattleFuncs.SkillParam>();
          effectParamValue.Add(BattleFuncs.SkillParam.CreateMul(unit.originalUnit, tuple.Item1, tuple.Item2));
        }
      }
      return effectParamValue;
    }

    public static List<BattleFuncs.SkillParam> GetRatioEffectParamValue(
      BL.ISkillEffectListUnit unit,
      BL.ISkillEffectListUnit target,
      BattleskillEffectLogicEnum logic)
    {
      IEnumerable<Tuple<BL.SkillEffect, float>> ratioEffectParams = unit.skillEffects.GetRatioEffectParams(logic);
      if (ratioEffectParams.Any<Tuple<BL.SkillEffect, float>>() && BattleFuncs.isSkillsAndEffectsInvalid(unit, target))
        return (List<BattleFuncs.SkillParam>) null;
      List<BattleFuncs.SkillParam> effectParamValue = (List<BattleFuncs.SkillParam>) null;
      foreach (Tuple<BL.SkillEffect, float> tuple in ratioEffectParams)
      {
        if (!BattleFuncs.isSealedSkillEffect(unit, tuple.Item1) && !BattleFuncs.isEffectEnemyRangeAndInvalid(tuple.Item1, unit, target))
        {
          if (effectParamValue == null)
            effectParamValue = new List<BattleFuncs.SkillParam>();
          effectParamValue.Add(BattleFuncs.SkillParam.CreateMul(unit.originalUnit, tuple.Item1, tuple.Item2));
        }
      }
      return effectParamValue;
    }

    public static void GetDeckEverySkillAddFilter(
      List<BattleFuncs.SkillParam> skillParams,
      List<BattleFuncs.SkillParam> sp)
    {
      if (sp.Count == 1)
      {
        skillParams.Add(sp[0]);
      }
      else
      {
        foreach (IGrouping<\u003C\u003Ef__AnonymousType9<int, int, int, int, int, int, int, int>, BattleFuncs.SkillParam> grouping in sp.GroupBy(x =>
        {
          BattleskillEffect effect = x.effect.effect;
          return new
          {
            a = effect.GetInt(BattleskillEffectLogicArgumentEnum.type),
            b = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count1),
            c = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count2),
            d = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count3),
            e = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count4),
            f = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count5),
            g = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count6),
            h = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count7)
          };
        }))
        {
          BattleFuncs.SkillParam skillParam1 = (BattleFuncs.SkillParam) null;
          int minValue = int.MinValue;
          BattleFuncs.SkillParam skillParam2 = (BattleFuncs.SkillParam) null;
          int maxValue = int.MaxValue;
          foreach (BattleFuncs.SkillParam skillParam3 in (IEnumerable<BattleFuncs.SkillParam>) grouping)
          {
            float? addParam = skillParam3.addParam;
            float num1 = 0.0f;
            if ((double) addParam.GetValueOrDefault() > (double) num1 & addParam.HasValue)
            {
              addParam = skillParam3.addParam;
              float num2 = (float) minValue;
              if ((double) addParam.GetValueOrDefault() > (double) num2 & addParam.HasValue)
              {
                minValue = (int) skillParam3.addParam.Value;
                skillParam1 = skillParam3;
              }
            }
            else
            {
              addParam = skillParam3.addParam;
              float num3 = 0.0f;
              if ((double) addParam.GetValueOrDefault() < (double) num3 & addParam.HasValue)
              {
                addParam = skillParam3.addParam;
                float num4 = (float) maxValue;
                if ((double) addParam.GetValueOrDefault() < (double) num4 & addParam.HasValue)
                {
                  maxValue = (int) skillParam3.addParam.Value;
                  skillParam2 = skillParam3;
                }
              }
            }
          }
          if (skillParam1 != null)
            skillParams.Add(skillParam1);
          if (skillParam2 != null)
            skillParams.Add(skillParam2);
        }
      }
    }

    public static void GetDeckEverySkillMulFilter(
      List<BattleFuncs.SkillParam> skillParams,
      List<BattleFuncs.SkillParam> sp)
    {
      if (sp.Count == 1)
      {
        skillParams.Add(sp[0]);
      }
      else
      {
        foreach (IGrouping<\u003C\u003Ef__AnonymousType9<int, int, int, int, int, int, int, int>, BattleFuncs.SkillParam> grouping in sp.GroupBy(x =>
        {
          BattleskillEffect effect = x.effect.effect;
          return new
          {
            a = effect.GetInt(BattleskillEffectLogicArgumentEnum.type),
            b = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count1),
            c = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count2),
            d = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count3),
            e = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count4),
            f = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count5),
            g = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count6),
            h = effect.GetInt(BattleskillEffectLogicArgumentEnum.kind_count7)
          };
        }))
        {
          BattleFuncs.SkillParam skillParam1 = (BattleFuncs.SkillParam) null;
          float minValue = float.MinValue;
          BattleFuncs.SkillParam skillParam2 = (BattleFuncs.SkillParam) null;
          float maxValue = float.MaxValue;
          foreach (BattleFuncs.SkillParam skillParam3 in (IEnumerable<BattleFuncs.SkillParam>) grouping)
          {
            float? mulParam = skillParam3.mulParam;
            float num1 = 1f;
            if ((double) mulParam.GetValueOrDefault() > (double) num1 & mulParam.HasValue)
            {
              mulParam = skillParam3.mulParam;
              float num2 = minValue;
              if ((double) mulParam.GetValueOrDefault() > (double) num2 & mulParam.HasValue)
              {
                minValue = skillParam3.mulParam.Value;
                skillParam1 = skillParam3;
              }
            }
            else
            {
              mulParam = skillParam3.mulParam;
              float num3 = 1f;
              if ((double) mulParam.GetValueOrDefault() < (double) num3 & mulParam.HasValue)
              {
                mulParam = skillParam3.mulParam;
                float num4 = maxValue;
                if ((double) mulParam.GetValueOrDefault() < (double) num4 & mulParam.HasValue)
                {
                  maxValue = skillParam3.mulParam.Value;
                  skillParam2 = skillParam3;
                }
              }
            }
          }
          if (skillParam1 != null)
            skillParams.Add(skillParam1);
          if (skillParam2 != null)
            skillParams.Add(skillParam2);
        }
      }
    }

    public enum Params
    {
      Hp,
      Strength,
      Intelligence,
      Vitality,
      Mind,
      Agility,
      Dexterity,
      Luck,
      Move,
      PhysicalAttack,
      PhysicalDefense,
      MagicAttack,
      MagicDefense,
      Hit,
      Critical,
      Evasion,
      CriticalEvasion,
      AttackSpeed,
      Kind,
    }

    [Serializable]
    public class GearParameter
    {
      public int Power;
      public int PhysicalDefense;
      public int MagicDefense;
      public int Hit;
      public int Critical;
      public int Evasion;
      public int Hp;
      public int Strength;
      public int Vitality;
      public int Intelligence;
      public int Mind;
      public int Agility;
      public int Dexterity;
      public int Luck;
      public int PhysicalPower;
      public int MagicalPower;
      public GearAttackType AttackType;

      public static Judgement.GearParameter FromPlayerGear(ItemInfo item)
      {
        return Judgement.GearParameter.FromPlayerGear(Array.Find<PlayerItem>(SMManager.Get<PlayerItem[]>(), (Predicate<PlayerItem>) (x => x.id == item.itemID)));
      }

      public static Judgement.GearParameter FromPlayerGear(PlayerItem pi)
      {
        return new Judgement.GearParameter()
        {
          Power = pi.power,
          PhysicalDefense = pi.physical_defense,
          MagicDefense = pi.magic_defense,
          Hit = pi.hit,
          Critical = pi.critical,
          Evasion = pi.evasion,
          Hp = pi.hp_incremental,
          Strength = pi.strength_incremental,
          Vitality = pi.vitality_incremental,
          Intelligence = pi.intelligence_incremental,
          Mind = pi.mind_incremental,
          Agility = pi.agility_incremental,
          Dexterity = pi.dexterity_incremental,
          Luck = pi.lucky_incremental,
          PhysicalPower = pi.gear == null || pi.gear.attack_type != GearAttackType.physical ? 0 : pi.power,
          MagicalPower = pi.gear == null || pi.gear.attack_type != GearAttackType.magic ? 0 : pi.power,
          AttackType = pi.gear.attack_type
        };
      }

      public static Judgement.GearParameter FromGearGear(GearGear gear)
      {
        return new Judgement.GearParameter()
        {
          Power = gear.power,
          PhysicalDefense = gear.physical_defense,
          MagicDefense = gear.magic_defense,
          Hit = gear.hit,
          Critical = gear.critical,
          Evasion = gear.evasion,
          Hp = gear.hp_incremental,
          Strength = gear.strength_incremental,
          Vitality = gear.vitality_incremental,
          Intelligence = gear.intelligence_incremental,
          Mind = gear.mind_incremental,
          Agility = gear.agility_incremental,
          Dexterity = gear.dexterity_incremental,
          Luck = gear.lucky_incremental,
          PhysicalPower = gear.attack_type != GearAttackType.physical ? 0 : gear.power,
          MagicalPower = gear.attack_type != GearAttackType.magic ? 0 : gear.power,
          AttackType = gear.attack_type
        };
      }

      public static Judgement.GearParameter Mix(
        Judgement.GearParameter lhs,
        Judgement.GearParameter rhs)
      {
        return new Judgement.GearParameter()
        {
          Power = Math.Max(lhs.Power, rhs.Power),
          PhysicalDefense = Math.Max(lhs.PhysicalDefense, rhs.PhysicalDefense),
          MagicDefense = Math.Max(lhs.MagicDefense, rhs.MagicDefense),
          Hit = Math.Max(lhs.Hit, rhs.Hit),
          Critical = Math.Max(lhs.Critical, rhs.Critical),
          Evasion = Math.Max(lhs.Evasion, rhs.Evasion),
          Hp = Math.Max(lhs.Hp, rhs.Hp),
          Strength = Math.Max(lhs.Strength, rhs.Strength),
          Vitality = Math.Max(lhs.Vitality, rhs.Vitality),
          Intelligence = Math.Max(lhs.Intelligence, rhs.Intelligence),
          Mind = Math.Max(lhs.Mind, rhs.Mind),
          Agility = Math.Max(lhs.Agility, rhs.Agility),
          Dexterity = Math.Max(lhs.Dexterity, rhs.Dexterity),
          Luck = Math.Max(lhs.Luck, rhs.Luck),
          PhysicalPower = Math.Max(lhs.PhysicalPower, rhs.PhysicalPower),
          MagicalPower = Math.Max(lhs.MagicalPower, rhs.MagicalPower),
          AttackType = lhs.AttackType
        };
      }

      public static Judgement.GearParameter Add(
        Judgement.GearParameter lhs,
        Judgement.GearParameter rhs)
      {
        return new Judgement.GearParameter()
        {
          Power = lhs.Power + rhs.Power,
          PhysicalDefense = lhs.PhysicalDefense + rhs.PhysicalDefense,
          MagicDefense = lhs.MagicDefense + rhs.MagicDefense,
          Hit = lhs.Hit + rhs.Hit,
          Critical = lhs.Critical + rhs.Critical,
          Evasion = lhs.Evasion + rhs.Evasion,
          Hp = lhs.Hp + rhs.Hp,
          Strength = lhs.Strength + rhs.Strength,
          Vitality = lhs.Vitality + rhs.Vitality,
          Intelligence = lhs.Intelligence + rhs.Intelligence,
          Mind = lhs.Mind + rhs.Mind,
          Agility = lhs.Agility + rhs.Agility,
          Dexterity = lhs.Dexterity + rhs.Dexterity,
          Luck = lhs.Luck + rhs.Luck,
          PhysicalPower = lhs.PhysicalPower + rhs.PhysicalPower,
          MagicalPower = lhs.MagicalPower + rhs.MagicalPower,
          AttackType = lhs.AttackType
        };
      }

      public static Judgement.GearParameter AddReisou(
        Judgement.GearParameter lhs,
        Judgement.GearParameter rhs)
      {
        if (lhs.AttackType == rhs.AttackType)
          return Judgement.GearParameter.Add(lhs, rhs);
        return new Judgement.GearParameter()
        {
          Power = lhs.Power,
          PhysicalDefense = lhs.PhysicalDefense + rhs.PhysicalDefense,
          MagicDefense = lhs.MagicDefense + rhs.MagicDefense,
          Hit = lhs.Hit + rhs.Hit,
          Critical = lhs.Critical + rhs.Critical,
          Evasion = lhs.Evasion + rhs.Evasion,
          Hp = lhs.Hp + rhs.Hp,
          Strength = lhs.Strength + rhs.Strength,
          Vitality = lhs.Vitality + rhs.Vitality,
          Intelligence = lhs.Intelligence + rhs.Intelligence,
          Mind = lhs.Mind + rhs.Mind,
          Agility = lhs.Agility + rhs.Agility,
          Dexterity = lhs.Dexterity + rhs.Dexterity,
          Luck = lhs.Luck + rhs.Luck,
          PhysicalPower = lhs.PhysicalPower + rhs.PhysicalPower,
          MagicalPower = lhs.MagicalPower + rhs.MagicalPower,
          AttackType = lhs.AttackType
        };
      }
    }

    [Serializable]
    public class NonBattleParameter
    {
      public int Hp;
      public int Strength;
      public int Intelligence;
      public int Vitality;
      public int Mind;
      public int Agility;
      public int Dexterity;
      public int Luck;
      public int Move;
      public int PhysicalAttack;
      public int PhysicalDefense;
      public int MagicAttack;
      public int MagicDefense;
      public int Hit;
      public int Critical;
      public int Evasion;
      public int Combat;
      public int Cost;

      public static Judgement.NonBattleParameter FromPlayerUnitWithPlayerGear(
        PlayerUnit playerUnit,
        bool bSelfAbility,
        PlayerItem playerGear,
        PlayerItem playerGear2 = null,
        PlayerItem playerReisou = null,
        PlayerItem playerReisou2 = null,
        PlayerItem playerReisou3 = null)
      {
        Judgement.GearParameter gearParameter = playerGear != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerGear) : (Judgement.GearParameter) null;
        Judgement.GearParameter gearParameter2 = playerGear2 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerGear2) : (Judgement.GearParameter) null;
        Judgement.GearParameter reisouParameter = playerReisou != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerReisou) : (Judgement.GearParameter) null;
        Judgement.GearParameter reisouParameter2 = playerReisou2 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerReisou2) : (Judgement.GearParameter) null;
        Judgement.GearParameter reisouParameter3 = playerReisou3 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerReisou3) : (Judgement.GearParameter) null;
        GearGear gearGear = !(playerGear != (PlayerItem) null) ? (!(playerGear2 != (PlayerItem) null) ? playerUnit.initial_gear : playerGear2.gear) : playerGear.gear;
        return Judgement.NonBattleParameter.FromPlayerUnitWithGearParameter(playerUnit, bSelfAbility, gearParameter, gearGear.attack_type, playerUnit.GetProficiencyIncr(gearGear.kind), gearParameter2, reisouParameter, reisouParameter2, reisouParameter3);
      }

      private static Judgement.NonBattleParameter FromPlayerUnitWithGearParameter(
        PlayerUnit playerUnit,
        bool bSelfAbility,
        Judgement.GearParameter gearParameter,
        GearAttackType atkType,
        UnitProficiencyIncr proficiency,
        Judgement.GearParameter gearParameter2 = null,
        Judgement.GearParameter reisouParameter = null,
        Judgement.GearParameter reisouParameter2 = null,
        Judgement.GearParameter reisouParameter3 = null)
      {
        if (gearParameter != null && reisouParameter != null)
          gearParameter = Judgement.GearParameter.AddReisou(gearParameter, reisouParameter);
        if (gearParameter2 != null && reisouParameter2 != null)
          gearParameter2 = Judgement.GearParameter.AddReisou(gearParameter2, reisouParameter2);
        if (gearParameter == null)
          gearParameter = gearParameter2 != null ? gearParameter2 : Judgement.GearParameter.FromGearGear(playerUnit.initial_gear);
        else if (gearParameter2 != null)
          gearParameter = Judgement.GearParameter.Mix(gearParameter, gearParameter2);
        if (gearParameter != null && reisouParameter3 != null)
          gearParameter = Judgement.GearParameter.AddReisou(gearParameter, reisouParameter3);
        Judgement.NonBattleParameter nonBattleParameter = new Judgement.NonBattleParameter();
        playerUnit.resetOnceOverkillers();
        if (bSelfAbility)
        {
          nonBattleParameter.Hp = playerUnit.self_total_hp + gearParameter.Hp;
          nonBattleParameter.Strength = playerUnit.self_total_strength + gearParameter.Strength;
          nonBattleParameter.Intelligence = playerUnit.self_total_intelligence + gearParameter.Intelligence;
          nonBattleParameter.Vitality = playerUnit.self_total_vitality + gearParameter.Vitality;
          nonBattleParameter.Mind = playerUnit.self_total_mind + gearParameter.Mind;
          nonBattleParameter.Agility = playerUnit.self_total_agility + gearParameter.Agility;
          nonBattleParameter.Dexterity = playerUnit.self_total_dexterity + gearParameter.Dexterity;
          nonBattleParameter.Luck = playerUnit.self_total_lucky + gearParameter.Luck;
        }
        else
        {
          nonBattleParameter.Hp = playerUnit.total_hp + gearParameter.Hp;
          nonBattleParameter.Strength = playerUnit.total_strength + gearParameter.Strength;
          nonBattleParameter.Intelligence = playerUnit.total_intelligence + gearParameter.Intelligence;
          nonBattleParameter.Vitality = playerUnit.total_vitality + gearParameter.Vitality;
          nonBattleParameter.Mind = playerUnit.total_mind + gearParameter.Mind;
          nonBattleParameter.Agility = playerUnit.total_agility + gearParameter.Agility;
          nonBattleParameter.Dexterity = playerUnit.total_dexterity + gearParameter.Dexterity;
          nonBattleParameter.Luck = playerUnit.total_lucky + gearParameter.Luck;
        }
        nonBattleParameter.Move = playerUnit.move;
        int num1 = 0;
        int num2 = 0;
        if (playerUnit.unit.magic_warrior_flag)
        {
          num1 = gearParameter.PhysicalPower;
          num2 = gearParameter.MagicalPower;
        }
        else
        {
          Judgement.GearParameter lhs = reisouParameter;
          if (reisouParameter2 != null)
            lhs = lhs != null ? Judgement.GearParameter.Mix(lhs, reisouParameter2) : reisouParameter2;
          if (reisouParameter3 != null)
            lhs = lhs != null ? Judgement.GearParameter.Add(lhs, reisouParameter3) : reisouParameter3;
          if (atkType == GearAttackType.none)
            atkType = playerUnit.initial_gear.attack_type;
          if (atkType == GearAttackType.magic)
          {
            num2 = gearParameter.MagicalPower;
            if (lhs != null)
              num1 = lhs.PhysicalPower;
          }
          else
          {
            num1 = gearParameter.PhysicalPower;
            if (lhs != null)
              num2 = lhs.MagicalPower;
          }
        }
        nonBattleParameter.PhysicalAttack = nonBattleParameter.Strength + num1 + proficiency.physical_attack;
        nonBattleParameter.PhysicalDefense = nonBattleParameter.Vitality + gearParameter.PhysicalDefense;
        nonBattleParameter.MagicAttack = nonBattleParameter.Intelligence + playerUnit.MinMagicBulletPower + num2 + proficiency.magic_attack;
        nonBattleParameter.MagicDefense = nonBattleParameter.Mind + gearParameter.MagicDefense;
        nonBattleParameter.Hit = (nonBattleParameter.Dexterity * 3 + nonBattleParameter.Luck) / 2 + gearParameter.Hit + proficiency.hit;
        nonBattleParameter.Critical = nonBattleParameter.Dexterity / 2 + gearParameter.Critical;
        nonBattleParameter.Evasion = (nonBattleParameter.Agility * 3 + nonBattleParameter.Luck) / 2 + gearParameter.Evasion + proficiency.evasion;
        nonBattleParameter.Combat = nonBattleParameter.Hp >= 5000 ? nonBattleParameter.PhysicalAttack + nonBattleParameter.PhysicalDefense + nonBattleParameter.MagicAttack + nonBattleParameter.MagicDefense + (nonBattleParameter.Hit + nonBattleParameter.Critical + nonBattleParameter.Evasion) / 2 + 5000 + (int) ((double) (nonBattleParameter.Hp - 5000) * 0.005) : nonBattleParameter.PhysicalAttack + nonBattleParameter.PhysicalDefense + nonBattleParameter.MagicAttack + nonBattleParameter.MagicDefense + (nonBattleParameter.Hit + nonBattleParameter.Critical + nonBattleParameter.Evasion) / 2 + nonBattleParameter.Hp;
        nonBattleParameter.Cost = playerUnit.cost;
        return nonBattleParameter;
      }

      public static Judgement.NonBattleParameter FromPlayerUnitMemoryWithGearParameter(
        PlayerUnit playerUnit,
        Judgement.GearParameter gearParameter,
        Judgement.GearParameter reisouParameter,
        GearAttackType atkType,
        UnitProficiencyIncr proficiency)
      {
        Judgement.NonBattleParameter nonBattleParameter = new Judgement.NonBattleParameter();
        nonBattleParameter.Hp = playerUnit.memory_hp + gearParameter.Hp;
        nonBattleParameter.Strength = playerUnit.memory_strength + gearParameter.Strength;
        nonBattleParameter.Intelligence = playerUnit.memory_intelligence + gearParameter.Intelligence;
        nonBattleParameter.Vitality = playerUnit.memory_vitality + gearParameter.Vitality;
        nonBattleParameter.Mind = playerUnit.memory_mind + gearParameter.Mind;
        nonBattleParameter.Agility = playerUnit.memory_agility + gearParameter.Agility;
        nonBattleParameter.Dexterity = playerUnit.memory_dexterity + gearParameter.Dexterity;
        nonBattleParameter.Luck = playerUnit.memory_lucky + gearParameter.Luck;
        nonBattleParameter.Move = playerUnit.move;
        int num1 = 0;
        int num2 = 0;
        if (playerUnit.unit.magic_warrior_flag)
        {
          num1 = gearParameter.PhysicalPower;
          num2 = gearParameter.MagicalPower;
        }
        else if (atkType == GearAttackType.magic)
        {
          num2 = gearParameter.MagicalPower;
          if (reisouParameter != null)
            num1 = reisouParameter.PhysicalPower;
        }
        else
        {
          num1 = gearParameter.PhysicalPower;
          if (reisouParameter != null)
            num2 = reisouParameter.MagicalPower;
        }
        nonBattleParameter.PhysicalAttack = nonBattleParameter.Strength + num1 + proficiency.physical_attack;
        nonBattleParameter.PhysicalDefense = nonBattleParameter.Vitality + gearParameter.PhysicalDefense;
        nonBattleParameter.MagicAttack = nonBattleParameter.Intelligence + playerUnit.MinMagicBulletPower + num2 + proficiency.magic_attack;
        nonBattleParameter.MagicDefense = nonBattleParameter.Mind + gearParameter.MagicDefense;
        nonBattleParameter.Hit = (nonBattleParameter.Dexterity * 3 + nonBattleParameter.Luck) / 2 + gearParameter.Hit + proficiency.hit;
        nonBattleParameter.Critical = nonBattleParameter.Dexterity / 2 + gearParameter.Critical;
        nonBattleParameter.Evasion = (nonBattleParameter.Agility * 3 + nonBattleParameter.Luck) / 2 + gearParameter.Evasion + proficiency.evasion;
        nonBattleParameter.Combat = nonBattleParameter.Hp >= 5000 ? nonBattleParameter.PhysicalAttack + nonBattleParameter.PhysicalDefense + nonBattleParameter.MagicAttack + nonBattleParameter.MagicDefense + (nonBattleParameter.Hit + nonBattleParameter.Critical + nonBattleParameter.Evasion) / 2 + 5000 + (int) ((double) (nonBattleParameter.Hp - 5000) * 0.005) : nonBattleParameter.PhysicalAttack + nonBattleParameter.PhysicalDefense + nonBattleParameter.MagicAttack + nonBattleParameter.MagicDefense + (nonBattleParameter.Hit + nonBattleParameter.Critical + nonBattleParameter.Evasion) / 2 + nonBattleParameter.Hp;
        return nonBattleParameter;
      }

      public static Judgement.NonBattleParameter FromPlayerUnitMemoryWithGearParameter(
        PlayerUnit playerUnit,
        Judgement.GearParameter gearParameter,
        Judgement.GearParameter reisouParameter)
      {
        return Judgement.NonBattleParameter.FromPlayerUnitMemoryWithGearParameter(playerUnit, gearParameter, reisouParameter, playerUnit.equippedGearOrInitial.attack_type, playerUnit.ProficiencyIncr);
      }

      public static Judgement.NonBattleParameter FromPlayerUnit(
        PlayerUnit playerUnit,
        bool bSelfAbility = false)
      {
        Judgement.GearParameter gearParameter = playerUnit.equippedGear != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedGear) : (Judgement.GearParameter) null;
        Judgement.GearParameter gearParameter2 = playerUnit.equippedGear2 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedGear2) : (Judgement.GearParameter) null;
        GearGear gearGear = !(playerUnit.equippedGear != (PlayerItem) null) ? (!(playerUnit.equippedGear2 != (PlayerItem) null) ? playerUnit.initial_gear : playerUnit.equippedGear2.gear) : playerUnit.equippedGear.gear;
        Judgement.GearParameter reisouParameter = playerUnit.equippedReisou != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou) : (Judgement.GearParameter) null;
        Judgement.GearParameter reisouParameter2 = playerUnit.equippedReisou2 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou2) : (Judgement.GearParameter) null;
        Judgement.GearParameter reisouParameter3 = playerUnit.equippedReisou3 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou3) : (Judgement.GearParameter) null;
        return Judgement.NonBattleParameter.FromPlayerUnitWithGearParameter(playerUnit, bSelfAbility, gearParameter, gearGear.attack_type, playerUnit.GetProficiencyIncr(gearGear.kind), gearParameter2, reisouParameter, reisouParameter2, reisouParameter3);
      }

      public static Judgement.NonBattleParameter FromPlayerUnitInitialGear(
        PlayerUnit playerUnit,
        bool bSelfAbility = false)
      {
        GearGear initialGear = playerUnit.initial_gear;
        Judgement.GearParameter gearParameter = Judgement.GearParameter.FromGearGear(initialGear);
        return Judgement.NonBattleParameter.FromPlayerUnitWithGearParameter(playerUnit, bSelfAbility, gearParameter, initialGear.attack_type, playerUnit.GetProficiencyIncr(initialGear.kind));
      }

      public static Judgement.NonBattleParameter FromPlayerUnitMemory(PlayerUnit playerUnit)
      {
        Judgement.GearParameter gearParameter1 = playerUnit.equippedGear != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedGear) : (Judgement.GearParameter) null;
        Judgement.GearParameter gearParameter2 = playerUnit.equippedGear2 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedGear2) : (Judgement.GearParameter) null;
        Judgement.GearParameter gearParameter3 = playerUnit.equippedReisou != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou) : (Judgement.GearParameter) null;
        Judgement.GearParameter rhs1 = playerUnit.equippedReisou2 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou2) : (Judgement.GearParameter) null;
        Judgement.GearParameter rhs2 = playerUnit.equippedReisou3 != (PlayerItem) null ? Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou3) : (Judgement.GearParameter) null;
        if (gearParameter1 != null && gearParameter3 != null)
          gearParameter1 = Judgement.GearParameter.AddReisou(gearParameter1, gearParameter3);
        if (gearParameter2 != null && rhs1 != null)
          gearParameter2 = Judgement.GearParameter.AddReisou(gearParameter2, rhs1);
        if (gearParameter1 == null)
          gearParameter1 = gearParameter2 ?? Judgement.GearParameter.FromGearGear(playerUnit.initial_gear);
        else if (gearParameter2 != null)
          gearParameter1 = Judgement.GearParameter.Mix(gearParameter1, gearParameter2);
        if (rhs1 != null)
          gearParameter3 = gearParameter3 == null ? rhs1 : Judgement.GearParameter.Mix(gearParameter3, rhs1);
        if (rhs2 != null)
          gearParameter3 = gearParameter3 == null ? rhs2 : Judgement.GearParameter.Add(gearParameter3, rhs2);
        return Judgement.NonBattleParameter.FromPlayerUnitMemoryWithGearParameter(playerUnit, gearParameter1, gearParameter3);
      }

      public static Judgement.NonBattleParameter FromPlayerUnitWithoutGear(PlayerUnit playerUnit)
      {
        return Judgement.NonBattleParameter.FromPlayerUnitWithGearParameter(playerUnit, false, Judgement.GearParameter.FromGearGear(playerUnit.initial_gear));
      }

      private static Judgement.NonBattleParameter FromPlayerUnitWithGearParameter(
        PlayerUnit playerUnit,
        bool bSelfAbility,
        Judgement.GearParameter gearParameter)
      {
        return Judgement.NonBattleParameter.FromPlayerUnitWithGearParameter(playerUnit, bSelfAbility, gearParameter, playerUnit.equippedGearOrInitial.attack_type, playerUnit.ProficiencyIncr);
      }

      public static Judgement.NonBattleParameter FromPlayerUnitMemoryWithoutGear(
        PlayerUnit playerUnit)
      {
        return Judgement.NonBattleParameter.FromPlayerUnitMemoryWithGearParameter(playerUnit, Judgement.GearParameter.FromGearGear(playerUnit.unit.initial_gear), (Judgement.GearParameter) null);
      }

      public class FromPlayerUnitCache
      {
        private PlayerUnit unit;
        private Judgement.NonBattleParameter parameterCache;

        public FromPlayerUnitCache(PlayerUnit unit) => this.unit = unit;

        public Judgement.NonBattleParameter parameter
        {
          get
          {
            if (this.parameterCache == null)
              this.parameterCache = Judgement.NonBattleParameter.FromPlayerUnit(this.unit);
            return this.parameterCache;
          }
        }
      }
    }

    private class OnemanChargeSearchTargetCheck : BattleFuncs.OnemanChargeSearchTargetCheck
    {
      public OnemanChargeSearchTargetCheck(BL.SkillEffect effect)
        : base(effect.effect.GetPackedSkillEffect(), BattleskillEffectLogicArgumentEnum.search_gear_kind_id, BattleskillEffectLogicArgumentEnum.search_element, BattleskillEffectLogicArgumentEnum.search_job_id, BattleskillEffectLogicArgumentEnum.search_family_id, BattleskillEffectLogicArgumentEnum.search_character_id, BattleskillEffectLogicArgumentEnum.search_same_character_id, BattleskillEffectLogicArgumentEnum.search_unit_id, BattleskillEffectLogicArgumentEnum.search_group_large_id, BattleskillEffectLogicArgumentEnum.search_group_small_id, BattleskillEffectLogicArgumentEnum.search_group_clothing_id, BattleskillEffectLogicArgumentEnum.search_group_generation_id, BattleskillEffectLogicArgumentEnum.search_skill_group_id)
      {
      }
    }

    [Serializable]
    public class BattleParameter
    {
      public int Hp;
      private int Strength_;
      private int Intelligence_;
      private int Vitality_;
      private int Mind_;
      private int Agility_;
      private int Dexterity_;
      private int Luck_;
      private int Move_;
      private int HpIncr_;
      private int StrengthIncr_;
      private int IntelligenceIncr_;
      private int VitalityIncr_;
      private int MindIncr_;
      private int AgilityIncr_;
      private int DexterityIncr_;
      private int LuckIncr_;
      private int MoveIncr_;
      private int PhysicalAttack_;
      private int PhysicalDefense_;
      private int MagicAttack_;
      private int MagicDefense_;
      private int Hit_;
      private int Critical_;
      private int Evasion_;
      private int Combat_;
      private int PhysicalAttackIncr_;
      private int PhysicalDefenseIncr_;
      private int MagicAttackIncr_;
      private int MagicDefenseIncr_;
      private int HitIncr_;
      private int CriticalIncr_;
      private int EvasionIncr_;
      private int CombatIncr_;
      private BL.ISkillEffectListUnit beUnit;
      [NonSerialized]
      private bool isCalcMove;
      [NonSerialized]
      private bool isCalcAll;
      [NonSerialized]
      private bool isGetMinimumState;
      [NonSerialized]
      private bool isGetAllState;
      [NonSerialized]
      private List<BattleFuncs.SkillParam>[] skillParams;
      [NonSerialized]
      private bool isAI;
      [NonSerialized]
      private PlayerUnit playerUnit;
      [NonSerialized]
      private Judgement.GearParameter gearParameter;
      [NonSerialized]
      private Judgement.GearParameter reisouParameter;
      [NonSerialized]
      private float hpRatio;
      [NonSerialized]
      private BL.ForceID forceId;
      [NonSerialized]
      private BL.ForceID[] targetForceIds;
      [NonSerialized]
      private IEnumerable<BL.Unit> forceUnits;
      [NonSerialized]
      private IEnumerable<BL.Unit> targetForceUnits;
      [NonSerialized]
      private int absoluteTurnCount;
      [NonSerialized]
      private BL.UnitPosition unitPosition;
      [NonSerialized]
      private BL.Panel panel;
      [NonSerialized]
      private BattleLandform landform;
      [NonSerialized]
      private BattleLandformIncr landform_incr;
      [NonSerialized]
      private BattleLandformIncr.LandformDuelSkillIncr landform_skill;
      [NonSerialized]
      private int move_range;
      [NonSerialized]
      private Judgement.NonBattleParameter.FromPlayerUnitCache nbpCache;
      [NonSerialized]
      private BattleFuncs.BuffDebuffSwapState swapState;
      [NonSerialized]
      private int[] stealAdd;
      [NonSerialized]
      private BattleFuncs.SkillParamClamp[] skillParamClamps;

      public int Strength
      {
        get
        {
          this.CalcAll();
          return this.Strength_;
        }
        set => this.Strength_ = value;
      }

      public int Intelligence
      {
        get
        {
          this.CalcAll();
          return this.Intelligence_;
        }
        set => this.Intelligence_ = value;
      }

      public int Vitality
      {
        get
        {
          this.CalcAll();
          return this.Vitality_;
        }
        set => this.Vitality_ = value;
      }

      public int Mind
      {
        get
        {
          this.CalcAll();
          return this.Mind_;
        }
        set => this.Mind_ = value;
      }

      public int Agility
      {
        get
        {
          this.CalcAll();
          return this.Agility_;
        }
        set => this.Agility_ = value;
      }

      public int Dexterity
      {
        get
        {
          this.CalcAll();
          return this.Dexterity_;
        }
        set => this.Dexterity_ = value;
      }

      public int Luck
      {
        get
        {
          this.CalcAll();
          return this.Luck_;
        }
        set => this.Luck_ = value;
      }

      public int Move
      {
        get
        {
          this.CalcMove();
          return this.Move_;
        }
        set => this.Move_ = value;
      }

      public int HpIncr
      {
        get
        {
          this.CalcAll();
          return this.HpIncr_;
        }
        set => this.HpIncr_ = value;
      }

      public int StrengthIncr
      {
        get
        {
          this.CalcAll();
          return this.StrengthIncr_;
        }
        set => this.StrengthIncr_ = value;
      }

      public int IntelligenceIncr
      {
        get
        {
          this.CalcAll();
          return this.IntelligenceIncr_;
        }
        set => this.IntelligenceIncr_ = value;
      }

      public int VitalityIncr
      {
        get
        {
          this.CalcAll();
          return this.VitalityIncr_;
        }
        set => this.VitalityIncr_ = value;
      }

      public int MindIncr
      {
        get
        {
          this.CalcAll();
          return this.MindIncr_;
        }
        set => this.MindIncr_ = value;
      }

      public int AgilityIncr
      {
        get
        {
          this.CalcAll();
          return this.AgilityIncr_;
        }
        set => this.AgilityIncr_ = value;
      }

      public int DexterityIncr
      {
        get
        {
          this.CalcAll();
          return this.DexterityIncr_;
        }
        set => this.DexterityIncr_ = value;
      }

      public int LuckIncr
      {
        get
        {
          this.CalcAll();
          return this.LuckIncr_;
        }
        set => this.LuckIncr_ = value;
      }

      public int MoveIncr
      {
        get
        {
          this.CalcAll();
          return this.MoveIncr_;
        }
        set => this.MoveIncr_ = value;
      }

      public int PhysicalAttack
      {
        get
        {
          this.CalcAll();
          return this.PhysicalAttack_;
        }
        set => this.PhysicalAttack_ = value;
      }

      public int PhysicalDefense
      {
        get
        {
          this.CalcAll();
          return this.PhysicalDefense_;
        }
        set => this.PhysicalDefense_ = value;
      }

      public int MagicAttack
      {
        get
        {
          this.CalcAll();
          return this.MagicAttack_;
        }
        set => this.MagicAttack_ = value;
      }

      public int MagicDefense
      {
        get
        {
          this.CalcAll();
          return this.MagicDefense_;
        }
        set => this.MagicDefense_ = value;
      }

      public int Hit
      {
        get
        {
          this.CalcAll();
          return this.Hit_;
        }
        set => this.Hit_ = value;
      }

      public int Critical
      {
        get
        {
          this.CalcAll();
          return this.Critical_;
        }
        set => this.Critical_ = value;
      }

      public int Evasion
      {
        get
        {
          this.CalcAll();
          return this.Evasion_;
        }
        set => this.Evasion_ = value;
      }

      public int Combat
      {
        get
        {
          this.CalcAll();
          return this.Combat_;
        }
        set => this.Combat_ = value;
      }

      public int PhysicalAttackIncr
      {
        get
        {
          this.CalcAll();
          return this.PhysicalAttackIncr_;
        }
        set => this.PhysicalAttackIncr_ = value;
      }

      public int PhysicalDefenseIncr
      {
        get
        {
          this.CalcAll();
          return this.PhysicalDefenseIncr_;
        }
        set => this.PhysicalDefenseIncr_ = value;
      }

      public int MagicAttackIncr
      {
        get
        {
          this.CalcAll();
          return this.MagicAttackIncr_;
        }
        set => this.MagicAttackIncr_ = value;
      }

      public int MagicDefenseIncr
      {
        get
        {
          this.CalcAll();
          return this.MagicDefenseIncr_;
        }
        set => this.MagicDefenseIncr_ = value;
      }

      public int HitIncr
      {
        get
        {
          this.CalcAll();
          return this.HitIncr_;
        }
        set => this.HitIncr_ = value;
      }

      public int CriticalIncr
      {
        get
        {
          this.CalcAll();
          return this.CriticalIncr_;
        }
        set => this.CriticalIncr_ = value;
      }

      public int EvasionIncr
      {
        get
        {
          this.CalcAll();
          return this.EvasionIncr_;
        }
        set => this.EvasionIncr_ = value;
      }

      public int CombatIncr
      {
        get
        {
          this.CalcAll();
          return this.CombatIncr_;
        }
        set => this.CombatIncr_ = value;
      }

      public List<BattleFuncs.SkillParam>[] SkillParams => this.skillParams;

      public static Judgement.BattleParameter FromBeUnit(
        BL.ISkillEffectListUnit beUnit,
        bool isCalcAll = false,
        bool isUsePosition = true)
      {
        Judgement.BattleParameter battleParameter = new Judgement.BattleParameter();
        battleParameter.beUnit = beUnit;
        battleParameter.isCalcMove = false;
        battleParameter.isCalcAll = false;
        battleParameter.isGetMinimumState = false;
        battleParameter.isGetAllState = false;
        battleParameter.skillParams = new List<BattleFuncs.SkillParam>[18];
        battleParameter.skillParamClamps = new BattleFuncs.SkillParamClamp[18];
        for (int index = 0; index < 18; ++index)
        {
          battleParameter.skillParams[index] = new List<BattleFuncs.SkillParam>();
          battleParameter.skillParamClamps[index] = new BattleFuncs.SkillParamClamp();
        }
        battleParameter.GetMinimumState(isUsePosition);
        battleParameter.CalcParamLogics(0);
        battleParameter.Hp = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) battleParameter.skillParams[0], battleParameter.skillParamClamps[0], (double) battleParameter.playerUnit.total_hp, battleParameter.landform_skill.skillMulHp, (float) (battleParameter.gearParameter.Hp + battleParameter.landform_skill.skillAddHp));
        if (isCalcAll)
          battleParameter.CalcAll();
        return battleParameter;
      }

      private void CalcParamLogics(int process)
      {
        if (process == 2 && this.panel != null)
        {
          foreach (BL.SkillEffect effect in this.panel.getSkillEffects(this.isAI).value)
          {
            List<BattleFuncs.SkillParam> skillParam = this.skillParams[effect.effect.EffectLogic.opt_test4];
            if (effect.effect.EffectLogic.opt_test3 == 7 && Judgement.CheckEnabledCharismaPanelBuffDebuff(effect, this.beUnit, this.isAI))
            {
              if (effect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                skillParam.Add(BattleFuncs.SkillParam.CreateAdd(effect.parentUnit, effect, (float) (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
              else
                skillParam.Add(BattleFuncs.SkillParam.CreateMul(effect.parentUnit, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
            }
          }
        }
        BL.ISkillEffectListUnit beUnit1 = this.beUnit;
        bool isHp = process == 0;
        Dictionary<int, List<BL.SkillEffect>> useRemainSkillEffects = new Dictionary<int, List<BL.SkillEffect>>();
        foreach (object processEffect in beUnit1.skillEffects.GetProcessEffects(process))
        {
          BL.SkillEffect skillEffect = !(processEffect is List<BL.SkillEffect> effects) ? (BL.SkillEffect) processEffect : effects[0];
          int optTest4 = skillEffect.effect.EffectLogic.opt_test4;
          List<BattleFuncs.SkillParam> skillParam = this.skillParams[optTest4];
          int num1 = 0;
          bool flag = true;
          BattleFuncs.SkillParamClamp skillParamClamp = this.skillParamClamps[skillEffect.effect.EffectLogic.opt_test4];
          int? nullable1;
          switch (skillEffect.effect.EffectLogic.opt_test3)
          {
            case 0:
              if (Judgement.CheckEnabledBuffDebuff(skillEffect, this.beUnit, BattleskillInvokeGameModeEnum.quest, this.panel, isHp, this.isAI))
              {
                num1 = 1;
                break;
              }
              continue;
            case 1:
              if (Judgement.CheckEnabledEquipGearBuffDebuff(skillEffect, this.beUnit))
                break;
              continue;
            case 4:
              skillEffect = Judgement.GetEnabledHpLeBuffDebuff(effects, this.beUnit, this.hpRatio);
              if (skillEffect != null)
                break;
              continue;
            case 5:
              skillEffect = Judgement.GetEnabledHpGeBuffDebuff(effects, this.beUnit, this.hpRatio);
              if (skillEffect != null)
                break;
              continue;
            case 6:
              if (this.targetForceIds != null && this.panel != null && Judgement.CheckEnabledTargetCountBuffDebuff(skillEffect, this.beUnit, this.targetForceIds, this.panel, this.isAI))
              {
                int num2 = BattleFuncs.getTargets(this.panel.row, this.panel.column, new int[2]
                {
                  skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
                  skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
                }, this.targetForceIds, BL.Unit.TargetAttribute.all, (this.isAI ? 1 : 0) != 0, nonFacility: true).Count<BL.UnitPosition>();
                int num3 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_target_count);
                if (num3 >= 1 && num2 > num3)
                  num2 = num3;
                if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                  skillParam.Add(BattleFuncs.SkillParam.CreateAdd(beUnit1.originalUnit, skillEffect, (float) (num2 * (int) ((double) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_lv_add) + (double) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_lv_mul) * (double) (skillEffect.baseSkillLevel - 1)))));
                else
                  skillParam.Add(BattleFuncs.SkillParam.CreateMul(beUnit1.originalUnit, skillEffect, (float) (1.0 + (double) num2 * ((double) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_lv_add) + (double) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_lv_mul) * (double) (skillEffect.baseSkillLevel - 1)))));
                num1 = -1;
                break;
              }
              continue;
            case 7:
              if (this.panel == null || !Judgement.CheckEnabledCharismaBuffDebuff(skillEffect, beUnit1, this.panel, this.isAI))
                continue;
              break;
            case 8:
              if (this.move_range >= 0)
              {
                skillEffect = Judgement.GetEnabledCavalryRushBuffDebuff(effects, this.beUnit, this.move_range);
                if (skillEffect != null)
                  break;
                continue;
              }
              continue;
            case 10:
              skillEffect = Judgement.GetEnabledExtremeOfForceBuffDebuff(effects, this.beUnit);
              if (skillEffect != null)
                break;
              continue;
            case 11:
              if (this.panel != null)
              {
                skillEffect = Judgement.GetEnabledOnemanChargeBuffDebuff(effects, this.beUnit, this.targetForceIds, this.panel, this.isAI);
                if (skillEffect != null)
                  break;
                continue;
              }
              continue;
            case 12:
              if (this.landform.in_out != BattleInOutSide.outside || !Judgement.CheckEnabledInOutSideBattleBuffDebuff(skillEffect, this.beUnit))
                continue;
              break;
            case 13:
              if (this.landform.in_out != BattleInOutSide.inside || !Judgement.CheckEnabledInOutSideBattleBuffDebuff(skillEffect, this.beUnit))
                continue;
              break;
            case 14:
              if (Judgement.CheckEnabledEvenIllusionBuffDebuff(skillEffect, this.beUnit, this.absoluteTurnCount))
                break;
              continue;
            case 15:
              if (Judgement.CheckEnabledSpecificUnitBuffDebuff(skillEffect, this.beUnit, isHp))
                break;
              continue;
            case 16:
              skillEffect = Judgement.GetEnabledUnitRarityBuffDebuff(effects, this.beUnit);
              if (skillEffect != null)
                break;
              continue;
            case 17:
              if (this.forceUnits != null)
              {
                skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, this.beUnit, this.absoluteTurnCount, this.forceUnits, (IEnumerable<BL.Unit>) null, BattleskillInvokeGameModeEnum.quest, this.isAI);
                if (skillEffect != null)
                  break;
                continue;
              }
              continue;
            case 18:
              if (this.targetForceUnits != null)
              {
                skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, this.beUnit, this.absoluteTurnCount, (IEnumerable<BL.Unit>) null, this.targetForceUnits, BattleskillInvokeGameModeEnum.quest, this.isAI);
                if (skillEffect != null)
                  break;
                continue;
              }
              continue;
            case 19:
              if (this.forceUnits != null && this.targetForceUnits != null)
              {
                skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, this.beUnit, this.absoluteTurnCount, this.forceUnits, this.targetForceUnits, BattleskillInvokeGameModeEnum.quest, this.isAI);
                if (skillEffect != null)
                  break;
                continue;
              }
              continue;
            case 20:
              if (Judgement.CheckEnabledBuffDebuff2(skillEffect, this.beUnit, BattleskillInvokeGameModeEnum.quest, isHp))
              {
                num1 = 2;
                break;
              }
              continue;
            case 21:
              if (Judgement.CheckEnabledSpecificGroupBuffDebuff(skillEffect, this.beUnit, isHp))
                break;
              continue;
            case 22:
              if (Judgement.CheckEnabledSpecificGroupBuffDebuff(skillEffect, this.beUnit, isHp))
                break;
              continue;
            case 23:
              if (Judgement.CheckEnabledSpecificSkillGroupBuffDebuff(skillEffect, this.beUnit, isHp))
                break;
              continue;
            case 25:
              if (Judgement.CheckEnabledParamDiffBuffDebuff(skillEffect, this.beUnit, this.nbpCache, this.beUnit.hp))
                break;
              continue;
            case 29:
              if (Judgement.CheckEnabledBuffDebuff3(skillEffect, this.beUnit))
              {
                num1 = 3;
                break;
              }
              continue;
            case 30:
              if (Judgement.CheckEnabledBuffDebuff4(skillEffect, this.beUnit, isHp))
              {
                num1 = 4;
                break;
              }
              continue;
            case 31:
              if (Judgement.CheckEnabledAttackClassBuffDebuff(skillEffect, this.beUnit, isHp))
                break;
              continue;
            case 32:
              if (Judgement.CheckEnabledAttackElementBuffDebuff(skillEffect, this.beUnit, isHp))
                break;
              continue;
            case 33:
              if (Judgement.CheckEnabledInvestLogicBuffDebuff(skillEffect, this.beUnit))
                break;
              continue;
            case 35:
              if (Judgement.CheckEnabledEvenIllusion2BuffDebuff(skillEffect, this.beUnit, this.absoluteTurnCount, BattleskillInvokeGameModeEnum.quest))
              {
                flag = false;
                break;
              }
              continue;
            case 36:
              if (Judgement.CheckEnabledEvenIllusion3BuffDebuff(skillEffect, this.beUnit, this.absoluteTurnCount, BattleskillInvokeGameModeEnum.quest))
              {
                flag = false;
                break;
              }
              continue;
            case 37:
              if (Judgement.CheckEnabledPeculiarParameterRangeBuffDebuff(skillEffect, this.beUnit))
                break;
              continue;
            case 39:
              if (Judgement.CheckEnabledLevelUpStatusBuffDebuff(skillEffect, this.beUnit, isHp))
                break;
              continue;
            case 40:
              if (Judgement.CheckEnabledUseRemainBuffDebuff(skillEffect, this.beUnit))
              {
                if (!useRemainSkillEffects.ContainsKey(optTest4))
                  useRemainSkillEffects[optTest4] = new List<BL.SkillEffect>();
                useRemainSkillEffects[optTest4].Add(skillEffect);
                num1 = -1;
                break;
              }
              continue;
            case 41:
              if (Judgement.CheckEnabledBuffDebuffClamp(skillEffect, this.beUnit, this.panel, this.isAI))
              {
                if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                {
                  int num4 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value);
                  int num5 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_value);
                  if (skillParamClamp.fixMax.HasValue)
                  {
                    int num6 = num4;
                    nullable1 = skillParamClamp.fixMax;
                    int valueOrDefault = nullable1.GetValueOrDefault();
                    if (!(num6 < valueOrDefault & nullable1.HasValue))
                      goto label_69;
                  }
                  skillParamClamp.fixMax = new int?(num4);
label_69:
                  if (skillParamClamp.fixMin.HasValue)
                  {
                    int num7 = num5;
                    nullable1 = skillParamClamp.fixMin;
                    int valueOrDefault = nullable1.GetValueOrDefault();
                    if (!(num7 > valueOrDefault & nullable1.HasValue))
                      goto label_78;
                  }
                  skillParamClamp.fixMin = new int?(num5);
                }
                else
                {
                  Decimal num8 = (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage);
                  Decimal num9 = (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_percentage);
                  Decimal? nullable2;
                  if (skillParamClamp.ratioMax.HasValue)
                  {
                    Decimal num10 = num8;
                    nullable2 = skillParamClamp.ratioMax;
                    Decimal valueOrDefault = nullable2.GetValueOrDefault();
                    if (!(num10 < valueOrDefault & nullable2.HasValue))
                      goto label_75;
                  }
                  skillParamClamp.ratioMax = new Decimal?(num8);
label_75:
                  if (skillParamClamp.ratioMin.HasValue)
                  {
                    Decimal num11 = num9;
                    nullable2 = skillParamClamp.ratioMin;
                    Decimal valueOrDefault = nullable2.GetValueOrDefault();
                    if (!(num11 > valueOrDefault & nullable2.HasValue))
                      goto label_78;
                  }
                  skillParamClamp.ratioMin = new Decimal?(num9);
                }
label_78:
                num1 = -1;
                break;
              }
              continue;
            case 43:
              BL.SkillEffect effect = skillEffect;
              BL.ISkillEffectListUnit beUnit2 = this.beUnit;
              nullable1 = new int?();
              int? colosseumTurn = nullable1;
              Judgement.NonBattleParameter.FromPlayerUnitCache nbpCache = this.nbpCache;
              nullable1 = new int?();
              int? hp = nullable1;
              BL.Panel panel = this.panel;
              int num12 = isHp ? 1 : 0;
              if (!Judgement.CheckEnabledGenericBuffDebuff(effect, beUnit2, colosseumTurn, nbpCache, hp, panel, num12 != 0))
                continue;
              break;
            default:
              continue;
          }
          if (num1 >= 0)
          {
            if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
            {
              int addParam = flag ? skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + skillEffect.baseSkillLevel * skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio) : skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value);
              skillParam.Add(BattleFuncs.SkillParam.CreateAdd(beUnit1.originalUnit, skillEffect, (float) addParam, (object) num1));
            }
            else
            {
              float mulParam = flag ? (float) ((Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) skillEffect.baseSkillLevel * (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)) : skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
              skillParam.Add(BattleFuncs.SkillParam.CreateMul(beUnit1.originalUnit, skillEffect, mulParam, (object) num1));
            }
          }
        }
        Judgement.ApplyUseRemainSkillEffect(useRemainSkillEffects, this.skillParams, beUnit1);
        if (process == 0)
          return;
        foreach (IGrouping<BattleskillEffectLogicEnum, Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> source in this.beUnit.skillEffects.GetAllEffectParams().Where<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, bool>) (x =>
        {
          int optTest4 = x.Item2.effect.EffectLogic.opt_test4;
          if (process == 1 && optTest4 == 8)
            return true;
          return process == 2 && optTest4 != 8;
        })).GroupBy<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, BattleskillEffectLogicEnum>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, BattleskillEffectLogicEnum>) (x => x.Item1)))
        {
          BL.SkillEffect skillEffect = source.First<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>().Item2;
          List<BattleFuncs.SkillParam> skillParam = this.skillParams[skillEffect.effect.EffectLogic.opt_test4];
          switch (skillEffect.effect.EffectLogic.opt_test3)
          {
            case 1001:
            case 1002:
            case 1007:
            case 1008:
              if (this.forceUnits != null)
              {
                List<BattleFuncs.SkillParam> sp1 = (List<BattleFuncs.SkillParam>) null;
                List<BattleFuncs.SkillParam> sp2 = (List<BattleFuncs.SkillParam>) null;
                foreach (Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> tuple in (IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>) source)
                {
                  if (!BattleFuncs.isSealedSkillEffect(this.beUnit, tuple.Item2))
                  {
                    if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                    {
                      if (sp1 == null)
                        sp1 = new List<BattleFuncs.SkillParam>();
                      sp1.Add(BattleFuncs.SkillParam.CreateAdd(this.beUnit.originalUnit, tuple.Item2, tuple.Item3));
                    }
                    else
                    {
                      if (sp2 == null)
                        sp2 = new List<BattleFuncs.SkillParam>();
                      sp2.Add(BattleFuncs.SkillParam.CreateMul(this.beUnit.originalUnit, tuple.Item2, tuple.Item3));
                    }
                  }
                }
                if (sp1 != null)
                  Judgement.GetDeckEverySkillAddFilter(skillParam, sp1);
                if (sp2 != null)
                {
                  Judgement.GetDeckEverySkillMulFilter(skillParam, sp2);
                  continue;
                }
                continue;
              }
              continue;
            case 1003:
            case 1004:
            case 1005:
            case 1006:
            case 1009:
            case 1010:
            case 1011:
            case 1012:
              if (this.forceUnits != null)
              {
                using (IEnumerator<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> enumerator = source.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> current = enumerator.Current;
                    if (!BattleFuncs.isSealedSkillEffect(this.beUnit, current.Item2))
                    {
                      if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                        skillParam.Add(BattleFuncs.SkillParam.CreateAdd(this.beUnit.originalUnit, current.Item2, current.Item3));
                      else
                        skillParam.Add(BattleFuncs.SkillParam.CreateMul(this.beUnit.originalUnit, current.Item2, current.Item3));
                    }
                  }
                  continue;
                }
              }
              else
                continue;
            default:
              continue;
          }
        }
      }

      private void GetMinimumState(bool isUsePosition = true)
      {
        if (this.isGetMinimumState)
          return;
        this.isGetMinimumState = true;
        this.isAI = this.beUnit is BL.AIUnit;
        this.playerUnit = this.beUnit.originalUnit.playerUnit;
        this.gearParameter = this.playerUnit.equippedGear == (PlayerItem) null ? Judgement.GearParameter.FromGearGear(this.playerUnit.equippedGearOrInitial) : Judgement.GearParameter.FromPlayerGear(this.playerUnit.equippedGear);
        this.reisouParameter = this.playerUnit.equippedReisou == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(this.playerUnit.equippedReisou);
        if (this.reisouParameter != null)
          this.gearParameter = Judgement.GearParameter.AddReisou(this.gearParameter, this.reisouParameter);
        if (this.playerUnit.equippedGear2 != (PlayerItem) null)
        {
          Judgement.GearParameter gearParameter = Judgement.GearParameter.FromPlayerGear(this.playerUnit.equippedGear2);
          Judgement.GearParameter rhs = this.playerUnit.equippedReisou2 == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(this.playerUnit.equippedReisou2);
          if (rhs != null)
          {
            gearParameter = Judgement.GearParameter.AddReisou(gearParameter, rhs);
            this.reisouParameter = this.reisouParameter == null ? rhs : Judgement.GearParameter.Mix(this.reisouParameter, rhs);
          }
          this.gearParameter = !(this.playerUnit.equippedGear == (PlayerItem) null) ? Judgement.GearParameter.Mix(this.gearParameter, gearParameter) : gearParameter;
        }
        if (this.playerUnit.equippedReisou3 != (PlayerItem) null)
        {
          Judgement.GearParameter rhs = Judgement.GearParameter.FromPlayerGear(this.playerUnit.equippedReisou3);
          if (rhs != null)
          {
            this.gearParameter = Judgement.GearParameter.AddReisou(this.gearParameter, rhs);
            this.reisouParameter = this.reisouParameter == null ? rhs : Judgement.GearParameter.Add(this.reisouParameter, rhs);
          }
        }
        this.unitPosition = !isUsePosition ? (BL.UnitPosition) null : BattleFuncs.iSkillEffectListUnitToUnitPosition(this.beUnit);
        if (this.unitPosition != null)
        {
          this.panel = BattleFuncs.getPanel(this.unitPosition.row, this.unitPosition.column);
          this.landform = this.panel.landform;
          this.landform_incr = this.landform.GetIncr(this.beUnit.originalUnit);
          this.landform_skill = this.landform_incr.GetDuelSkillIncr(this.beUnit.originalUnit);
        }
        else
        {
          this.panel = (BL.Panel) null;
          this.landform = (BattleLandform) null;
          this.landform_incr = (BattleLandformIncr) null;
          this.landform_skill = new BattleLandformIncr.LandformDuelSkillIncr();
          this.landform_skill.skillAddHp = 0;
          this.landform_skill.skillAddStrength = 0;
          this.landform_skill.skillAddIntelligence = 0;
          this.landform_skill.skillAddVitality = 0;
          this.landform_skill.skillAddMind = 0;
          this.landform_skill.skillAddAgility = 0;
          this.landform_skill.skillAddDexterity = 0;
          this.landform_skill.skillAddLuck = 0;
          this.landform_skill.skillAddMove = 0;
          this.landform_skill.skillAddPhysicalAttack = 0;
          this.landform_skill.skillAddPhysicalDefense = 0;
          this.landform_skill.skillAddMagicAttack = 0;
          this.landform_skill.skillAddMagicDefense = 0;
          this.landform_skill.skillAddHit = 0;
          this.landform_skill.skillAddCritical = 0;
          this.landform_skill.skillAddEvasion = 0;
          this.landform_skill.skillMulHp = 1f;
          this.landform_skill.skillMulStrength = 1f;
          this.landform_skill.skillMulIntelligence = 1f;
          this.landform_skill.skillMulVitality = 1f;
          this.landform_skill.skillMulMind = 1f;
          this.landform_skill.skillMulAgility = 1f;
          this.landform_skill.skillMulDexterity = 1f;
          this.landform_skill.skillMulLuck = 1f;
          this.landform_skill.skillMulMove = 1f;
          this.landform_skill.skillMulPhysicalAttack = 1f;
          this.landform_skill.skillMulPhysicalDefense = 1f;
          this.landform_skill.skillMulMagicAttack = 1f;
          this.landform_skill.skillMulMagicDefense = 1f;
          this.landform_skill.skillMulHit = 1f;
          this.landform_skill.skillMulCritical = 1f;
          this.landform_skill.skillMulEvasion = 1f;
        }
      }

      private void GetAllState()
      {
        if (this.isGetAllState)
          return;
        this.isGetAllState = true;
        this.GetMinimumState();
        this.hpRatio = (float) ((Decimal) this.beUnit.hp / (Decimal) this.Hp * 100M);
        if (BattleFuncs.getEnv() != null)
        {
          this.forceId = BattleFuncs.getForceID(this.beUnit.originalUnit);
          this.targetForceIds = BattleFuncs.getTargetForce(this.beUnit.originalUnit, false);
          this.forceUnits = (IEnumerable<BL.Unit>) BattleFuncs.forceUnits(this.forceId).value;
          this.targetForceUnits = ((IEnumerable<BL.ForceID>) this.targetForceIds).SelectMany<BL.ForceID, BL.Unit>((Func<BL.ForceID, IEnumerable<BL.Unit>>) (x => (IEnumerable<BL.Unit>) BattleFuncs.forceUnits(x).value));
        }
        else
        {
          this.forceId = BL.ForceID.none;
          this.targetForceIds = (BL.ForceID[]) null;
          this.forceUnits = (IEnumerable<BL.Unit>) null;
          this.targetForceUnits = (IEnumerable<BL.Unit>) null;
        }
        this.absoluteTurnCount = BattleFuncs.getPhaseState() != null ? BattleFuncs.getPhaseState().absoluteTurnCount : 0;
        this.move_range = this.unitPosition == null ? -1 : BL.fieldDistance(BattleFuncs.getPanel(this.unitPosition.originalRow, this.unitPosition.originalColumn), this.panel);
        this.nbpCache = new Judgement.NonBattleParameter.FromPlayerUnitCache(this.playerUnit);
        this.swapState = BattleFuncs.BuffDebuffSwapState.Create(this.beUnit, panel: this.panel);
        foreach (BL.SkillEffect skillEffect in this.beUnit.skillEffects.Where(BattleskillEffectLogicEnum.steal_effect))
        {
          if (this.stealAdd == null)
            this.stealAdd = new int[18];
          this.stealAdd[(int) skillEffect.work[0]] += (int) skillEffect.work[1];
        }
      }

      private void CalcMove()
      {
        if (this.isCalcMove)
          return;
        this.isCalcMove = true;
        this.GetAllState();
        this.CalcParamLogics(1);
        this.Move = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) this.skillParams[8], this.skillParamClamps[8], (double) this.playerUnit.move, this.landform_skill.skillMulMove, (float) (this.landform_skill.skillAddMove + (this.stealAdd != null ? this.stealAdd[8] : 0)));
      }

      private void CalcAll()
      {
        if (this.isCalcAll)
          return;
        this.isCalcAll = true;
        this.CalcMove();
        this.GetAllState();
        this.CalcParamLogics(2);
        UnitProficiencyIncr proficiencyIncr = this.playerUnit.ProficiencyIncr;
        float num1;
        float num2;
        float num3;
        float num4;
        float num5;
        float num6;
        float num7;
        float num8;
        if (this.landform_incr != null)
        {
          List<BattleFuncs.SkillParam> skillParams1 = new List<BattleFuncs.SkillParam>();
          List<BattleFuncs.SkillParam> skillParams2 = new List<BattleFuncs.SkillParam>();
          List<BattleFuncs.SkillParam> skillParams3 = new List<BattleFuncs.SkillParam>();
          List<BattleFuncs.SkillParam> skillParams4 = new List<BattleFuncs.SkillParam>();
          BattleFuncs.GetLandBlessingSkillAdd(skillParams1, this.beUnit, BattleskillEffectLogicEnum.land_blessing_fix_physical_defense, this.landform);
          BattleFuncs.GetLandBlessingSkillAdd(skillParams2, this.beUnit, BattleskillEffectLogicEnum.land_blessing_fix_magic_defense, this.landform);
          BattleFuncs.GetLandBlessingSkillAdd(skillParams3, this.beUnit, BattleskillEffectLogicEnum.land_blessing_fix_hit, this.landform);
          BattleFuncs.GetLandBlessingSkillAdd(skillParams4, this.beUnit, BattleskillEffectLogicEnum.land_blessing_fix_evasion, this.landform);
          BattleFuncs.GetLandBlessingSkillMul(skillParams1, this.beUnit, BattleskillEffectLogicEnum.land_blessing_ratio_physical_defense, this.landform);
          BattleFuncs.GetLandBlessingSkillMul(skillParams2, this.beUnit, BattleskillEffectLogicEnum.land_blessing_ratio_magic_defense, this.landform);
          BattleFuncs.GetLandBlessingSkillMul(skillParams3, this.beUnit, BattleskillEffectLogicEnum.land_blessing_ratio_hit, this.landform);
          BattleFuncs.GetLandBlessingSkillMul(skillParams4, this.beUnit, BattleskillEffectLogicEnum.land_blessing_ratio_evasion, this.landform);
          int num9 = BattleFuncs.calcSkillParamAdd(skillParams1);
          int num10 = BattleFuncs.calcSkillParamAdd(skillParams2);
          int num11 = BattleFuncs.calcSkillParamAdd(skillParams3);
          int num12 = BattleFuncs.calcSkillParamAdd(skillParams4);
          float num13 = BattleFuncs.calcSkillParamMul(skillParams1);
          float num14 = BattleFuncs.calcSkillParamMul(skillParams2);
          float num15 = BattleFuncs.calcSkillParamMul(skillParams3);
          float num16 = BattleFuncs.calcSkillParamMul(skillParams4);
          num1 = this.landform_incr.physical_defense_ratio_incr.HasValue ? (float) num9 : Mathf.Ceil((float) this.landform_incr.physical_defense_incr * num13) + (float) num9;
          num2 = this.landform_incr.magic_defense_ratio_incr.HasValue ? (float) num10 : Mathf.Ceil((float) this.landform_incr.magic_defense_incr * num14) + (float) num10;
          num3 = this.landform_incr.hit_ratio_incr.HasValue ? (float) num11 : Mathf.Ceil((float) this.landform_incr.hit_incr * num15) + (float) num11;
          num4 = this.landform_incr.evasion_ratio_incr.HasValue ? (float) num12 : Mathf.Ceil((float) this.landform_incr.evasion_incr * num16) + (float) num12;
          num5 = this.landform_incr.physical_defense_ratio_incr.HasValue ? this.landform_incr.physical_defense_ratio_incr.Value * num13 : 1f;
          num6 = this.landform_incr.magic_defense_ratio_incr.HasValue ? this.landform_incr.magic_defense_ratio_incr.Value * num14 : 1f;
          num7 = this.landform_incr.hit_ratio_incr.HasValue ? this.landform_incr.hit_ratio_incr.Value * num15 : 1f;
          num8 = this.landform_incr.evasion_ratio_incr.HasValue ? this.landform_incr.evasion_ratio_incr.Value * num16 : 1f;
        }
        else
        {
          num1 = 0.0f;
          num2 = 0.0f;
          num3 = 0.0f;
          num4 = 0.0f;
          num5 = 1f;
          num6 = 1f;
          num7 = 1f;
          num8 = 1f;
        }
        Judgement.BattleParameter battleParameter = this;
        battleParameter.Agility = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) this.skillParams[5], this.skillParamClamps[5], (double) this.playerUnit.total_agility, this.landform_skill.skillMulAgility, (float) (this.gearParameter.Agility + this.landform_skill.skillAddAgility + (this.stealAdd != null ? this.stealAdd[5] : 0)), this.swapState);
        battleParameter.Dexterity = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) this.skillParams[6], this.skillParamClamps[6], (double) this.playerUnit.total_dexterity, this.landform_skill.skillMulDexterity, (float) (this.gearParameter.Dexterity + this.landform_skill.skillAddDexterity + (this.stealAdd != null ? this.stealAdd[6] : 0)), this.swapState);
        battleParameter.Luck = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) this.skillParams[7], this.skillParamClamps[7], (double) this.playerUnit.total_lucky, this.landform_skill.skillMulLuck, (float) (this.gearParameter.Luck + this.landform_skill.skillAddLuck + (this.stealAdd != null ? this.stealAdd[7] : 0)), this.swapState);
        int num17 = 0;
        int num18 = 0;
        if (this.playerUnit.unit.magic_warrior_flag)
        {
          num17 = this.gearParameter.PhysicalPower;
          num18 = this.gearParameter.MagicalPower;
        }
        else if (this.gearParameter.AttackType == GearAttackType.magic)
        {
          num18 = this.gearParameter.MagicalPower;
          if (this.reisouParameter != null)
            num17 = this.reisouParameter.PhysicalPower;
        }
        else
        {
          num17 = this.gearParameter.PhysicalPower;
          if (this.reisouParameter != null)
            num18 = this.reisouParameter.MagicalPower;
        }
        battleParameter.Hit = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) this.skillParams[13], this.skillParamClamps[13], (double) ((battleParameter.Dexterity * 3 + battleParameter.Luck) / 2 + this.gearParameter.Hit + proficiencyIncr.hit), this.landform_skill.skillMulHit * num7, (float) ((double) this.landform_skill.skillAddHit + (double) num3 + (this.stealAdd != null ? (double) this.stealAdd[13] : 0.0)), this.swapState);
        battleParameter.Critical = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) this.skillParams[14], this.skillParamClamps[14], (double) (battleParameter.Dexterity / 2 + this.gearParameter.Critical), this.landform_skill.skillMulCritical, (float) (this.landform_skill.skillAddCritical + (this.stealAdd != null ? this.stealAdd[14] : 0)), this.swapState);
        battleParameter.Evasion = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) this.skillParams[15], this.skillParamClamps[15], (double) ((battleParameter.Agility * 3 + battleParameter.Luck) / 2 + this.gearParameter.Evasion + proficiencyIncr.evasion), this.landform_skill.skillMulEvasion * num8, (float) ((double) this.landform_skill.skillAddEvasion + (double) num4 + (this.stealAdd != null ? (double) this.stealAdd[15] : 0.0)), this.swapState);
        Tuple<float, float> tuple1 = BattleFuncs.calcSkillParam2(this.skillParams[1], this.skillParamClamps[1], (float) this.playerUnit.total_strength, this.landform_skill.skillMulStrength, (float) (this.gearParameter.Strength + this.landform_skill.skillAddStrength + (this.stealAdd != null ? this.stealAdd[1] : 0)), this.skillParams[9], this.skillParamClamps[9], (float) (num17 + proficiencyIncr.physical_attack), this.landform_skill.skillMulPhysicalAttack, (float) (this.landform_skill.skillAddPhysicalAttack + (this.stealAdd != null ? this.stealAdd[9] : 0)), this.swapState);
        Tuple<float, float> tuple2 = BattleFuncs.calcSkillParam2(this.skillParams[2], this.skillParamClamps[2], (float) this.playerUnit.total_intelligence, this.landform_skill.skillMulIntelligence, (float) (this.gearParameter.Intelligence + this.landform_skill.skillAddIntelligence + (this.stealAdd != null ? this.stealAdd[2] : 0)), this.skillParams[11], this.skillParamClamps[11], (float) (this.playerUnit.MinMagicBulletPower + num18 + proficiencyIncr.magic_attack), this.landform_skill.skillMulMagicAttack, (float) (this.landform_skill.skillAddMagicAttack + (this.stealAdd != null ? this.stealAdd[11] : 0)), this.swapState);
        Tuple<float, float> tuple3 = BattleFuncs.calcSkillParam2(this.skillParams[3], this.skillParamClamps[3], (float) this.playerUnit.total_vitality, this.landform_skill.skillMulVitality, (float) (this.gearParameter.Vitality + this.landform_skill.skillAddVitality + (this.stealAdd != null ? this.stealAdd[3] : 0)), this.skillParams[10], this.skillParamClamps[10], (float) this.gearParameter.PhysicalDefense, this.landform_skill.skillMulPhysicalDefense * num5, (float) ((double) this.landform_skill.skillAddPhysicalDefense + (double) num1 + (this.stealAdd != null ? (double) this.stealAdd[10] : 0.0)), this.swapState);
        Tuple<float, float> tuple4 = BattleFuncs.calcSkillParam2(this.skillParams[4], this.skillParamClamps[4], (float) this.playerUnit.total_mind, this.landform_skill.skillMulMind, (float) (this.gearParameter.Mind + this.landform_skill.skillAddMind + (this.stealAdd != null ? this.stealAdd[4] : 0)), this.skillParams[12], this.skillParamClamps[12], (float) this.gearParameter.MagicDefense, this.landform_skill.skillMulMagicDefense * num6, (float) ((double) this.landform_skill.skillAddMagicDefense + (double) num2 + (this.stealAdd != null ? (double) this.stealAdd[12] : 0.0)), this.swapState);
        battleParameter.Strength = (int) tuple1.Item1;
        battleParameter.PhysicalAttack = (int) tuple1.Item2;
        battleParameter.Intelligence = (int) tuple2.Item1;
        battleParameter.MagicAttack = (int) tuple2.Item2;
        battleParameter.Vitality = (int) tuple3.Item1;
        battleParameter.PhysicalDefense = (int) tuple3.Item2;
        battleParameter.Mind = (int) tuple4.Item1;
        battleParameter.MagicDefense = (int) tuple4.Item2;
        battleParameter.Combat = battleParameter.Hp >= 5000 ? battleParameter.PhysicalAttack + battleParameter.PhysicalDefense + battleParameter.MagicAttack + battleParameter.MagicDefense + (battleParameter.Hit + battleParameter.Critical + battleParameter.Evasion) / 2 + 5000 + (int) ((double) (battleParameter.Hp - 5000) * 0.005) : battleParameter.PhysicalAttack + battleParameter.PhysicalDefense + battleParameter.MagicAttack + battleParameter.MagicDefense + (battleParameter.Hit + battleParameter.Critical + battleParameter.Evasion) / 2 + battleParameter.Hp;
        Judgement.NonBattleParameter parameter = this.nbpCache.parameter;
        battleParameter.HpIncr = battleParameter.Hp - parameter.Hp;
        battleParameter.StrengthIncr = battleParameter.Strength - parameter.Strength;
        battleParameter.IntelligenceIncr = battleParameter.Intelligence - parameter.Intelligence;
        battleParameter.VitalityIncr = battleParameter.Vitality - parameter.Vitality;
        battleParameter.MindIncr = battleParameter.Mind - parameter.Mind;
        battleParameter.AgilityIncr = battleParameter.Agility - parameter.Agility;
        battleParameter.DexterityIncr = battleParameter.Dexterity - parameter.Dexterity;
        battleParameter.LuckIncr = battleParameter.Luck - parameter.Luck;
        battleParameter.MoveIncr = battleParameter.Move - parameter.Move;
        battleParameter.PhysicalAttackIncr = battleParameter.PhysicalAttack - parameter.PhysicalAttack;
        battleParameter.PhysicalDefenseIncr = battleParameter.PhysicalDefense - parameter.PhysicalDefense;
        battleParameter.MagicAttackIncr = battleParameter.MagicAttack - parameter.MagicAttack;
        battleParameter.MagicDefenseIncr = battleParameter.MagicDefense - parameter.MagicDefense;
        battleParameter.HitIncr = battleParameter.Hit - parameter.Hit;
        battleParameter.CriticalIncr = battleParameter.Critical - parameter.Critical;
        battleParameter.EvasionIncr = battleParameter.Evasion - parameter.Evasion;
        battleParameter.CombatIncr = battleParameter.Combat - parameter.Combat;
        this.playerUnit = (PlayerUnit) null;
        this.gearParameter = (Judgement.GearParameter) null;
        this.reisouParameter = (Judgement.GearParameter) null;
        this.targetForceIds = (BL.ForceID[]) null;
        this.forceUnits = (IEnumerable<BL.Unit>) null;
        this.targetForceUnits = (IEnumerable<BL.Unit>) null;
        this.unitPosition = (BL.UnitPosition) null;
        this.panel = (BL.Panel) null;
        this.landform = (BattleLandform) null;
        this.landform_incr = (BattleLandformIncr) null;
        this.nbpCache = (Judgement.NonBattleParameter.FromPlayerUnitCache) null;
      }

      public static Judgement.BattleParameter FromBeColosseumUnit(
        BL.Unit beUnit,
        PlayerItem equipped_gear,
        PlayerItem equipped_gear2,
        PlayerItem equipped_reisou,
        PlayerItem equipped_reisou2,
        PlayerItem equipped_reisou3)
      {
        List<BattleFuncs.SkillParam>[] skillParamListArray = new List<BattleFuncs.SkillParam>[18];
        BattleFuncs.SkillParamClamp[] skillParamClampArray = new BattleFuncs.SkillParamClamp[18];
        for (int index = 0; index < 18; ++index)
        {
          skillParamListArray[index] = new List<BattleFuncs.SkillParam>();
          skillParamClampArray[index] = new BattleFuncs.SkillParamClamp();
        }
        for (int process = 0; process <= 2; ++process)
        {
          BL.Unit unit = beUnit;
          bool isHp = process == 0;
          foreach (object processEffect in unit.skillEffects.GetProcessEffects(process))
          {
            BL.SkillEffect skillEffect = !(processEffect is List<BL.SkillEffect> skillEffectList) ? (BL.SkillEffect) processEffect : skillEffectList[0];
            List<BattleFuncs.SkillParam> skillParamList = skillParamListArray[skillEffect.effect.EffectLogic.opt_test4];
            int num = 0;
            BattleFuncs.SkillParamClamp skillParamClamp = skillParamClampArray[skillEffect.effect.EffectLogic.opt_test4];
            switch (skillEffect.effect.EffectLogic.opt_test3)
            {
              case 0:
                if (Judgement.CheckEnabledBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, BattleskillInvokeGameModeEnum.colosseum, (BL.Panel) null, isHp, false))
                {
                  num = 1;
                  break;
                }
                continue;
              case 1:
                if (Judgement.CheckEnabledEquipGearBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit))
                  break;
                continue;
              case 15:
                if (!isHp || !Judgement.CheckEnabledSpecificUnitBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, isHp))
                  continue;
                break;
              case 20:
                if (Judgement.CheckEnabledBuffDebuff2(skillEffect, (BL.ISkillEffectListUnit) beUnit, BattleskillInvokeGameModeEnum.colosseum, isHp))
                {
                  num = 2;
                  break;
                }
                continue;
              case 21:
                if (!isHp || !Judgement.CheckEnabledSpecificGroupBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, isHp))
                  continue;
                break;
              case 22:
                if (!isHp || !Judgement.CheckEnabledSpecificGroupBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, isHp))
                  continue;
                break;
              case 23:
                if (!isHp || !Judgement.CheckEnabledSpecificSkillGroupBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, isHp))
                  continue;
                break;
              case 30:
                if (isHp && Judgement.CheckEnabledBuffDebuff4(skillEffect, (BL.ISkillEffectListUnit) beUnit, isHp))
                {
                  num = 4;
                  break;
                }
                continue;
              case 31:
                if (!isHp || !Judgement.CheckEnabledAttackClassBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, isHp))
                  continue;
                break;
              case 32:
                if (!isHp || !Judgement.CheckEnabledAttackElementBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, isHp))
                  continue;
                break;
              case 39:
                if (!isHp || !Judgement.CheckEnabledLevelUpStatusBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, isHp))
                  continue;
                break;
              case 43:
                if (!isHp || !Judgement.CheckEnabledGenericBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, new int?(0), (Judgement.NonBattleParameter.FromPlayerUnitCache) null, new int?(), (BL.Panel) null, isHp))
                  continue;
                break;
              default:
                continue;
            }
            if (num >= 0)
            {
              if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(unit.originalUnit, skillEffect, (float) (skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + skillEffect.baseSkillLevel * skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio)), (object) num));
              else
                skillParamList.Add(BattleFuncs.SkillParam.CreateMul(unit.originalUnit, skillEffect, skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) skillEffect.baseSkillLevel * skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio), (object) num));
            }
          }
        }
        PlayerUnit playerUnit = beUnit.playerUnit;
        Judgement.GearParameter lhs = equipped_gear == (PlayerItem) null ? Judgement.GearParameter.FromGearGear(playerUnit.initial_gear) : Judgement.GearParameter.FromPlayerGear(equipped_gear);
        Judgement.GearParameter gearParameter1 = equipped_reisou == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(equipped_reisou);
        Judgement.GearParameter rhs1 = equipped_reisou2 == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(equipped_reisou2);
        Judgement.GearParameter rhs2 = equipped_reisou3 == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(equipped_reisou3);
        if (gearParameter1 != null)
          lhs = Judgement.GearParameter.AddReisou(lhs, gearParameter1);
        if (equipped_gear2 != (PlayerItem) null)
        {
          Judgement.GearParameter gearParameter2 = Judgement.GearParameter.FromPlayerGear(equipped_gear2);
          if (rhs1 != null)
          {
            gearParameter2 = Judgement.GearParameter.AddReisou(gearParameter2, rhs1);
            gearParameter1 = gearParameter1 == null ? rhs1 : Judgement.GearParameter.Mix(gearParameter1, rhs1);
          }
          lhs = !(equipped_gear == (PlayerItem) null) ? Judgement.GearParameter.Mix(lhs, gearParameter2) : gearParameter2;
        }
        if (rhs2 != null)
        {
          lhs = Judgement.GearParameter.AddReisou(lhs, rhs2);
          gearParameter1 = gearParameter1 == null ? rhs2 : Judgement.GearParameter.Add(gearParameter1, rhs2);
        }
        UnitProficiencyIncr proficiencyIncr = playerUnit.ProficiencyIncr;
        Judgement.BattleParameter battleParameter = new Judgement.BattleParameter();
        battleParameter.isCalcAll = true;
        battleParameter.isCalcMove = true;
        battleParameter.Hp = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParamListArray[0], skillParamClampArray[0], (double) playerUnit.total_hp, extraAdd: (float) lhs.Hp);
        battleParameter.Agility = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParamListArray[5], skillParamClampArray[5], (double) playerUnit.total_agility, extraAdd: (float) lhs.Agility);
        battleParameter.Dexterity = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParamListArray[6], skillParamClampArray[6], (double) playerUnit.total_dexterity, extraAdd: (float) lhs.Dexterity);
        battleParameter.Luck = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParamListArray[7], skillParamClampArray[7], (double) playerUnit.total_lucky, extraAdd: (float) lhs.Luck);
        battleParameter.Move = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParamListArray[8], skillParamClampArray[8], (double) playerUnit.move);
        int num1 = 0;
        int num2 = 0;
        if (playerUnit.unit.magic_warrior_flag)
        {
          num1 = lhs.PhysicalPower;
          num2 = lhs.MagicalPower;
        }
        else if (lhs.AttackType == GearAttackType.magic)
        {
          num2 = lhs.MagicalPower;
          if (gearParameter1 != null)
            num1 = gearParameter1.PhysicalPower;
        }
        else
        {
          num1 = lhs.PhysicalPower;
          if (gearParameter1 != null)
            num1 = gearParameter1.MagicalPower;
        }
        battleParameter.Hit = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParamListArray[13], skillParamClampArray[13], (double) ((battleParameter.Dexterity * 3 + battleParameter.Luck) / 2 + lhs.Hit + proficiencyIncr.hit));
        battleParameter.Critical = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParamListArray[14], skillParamClampArray[14], (double) (battleParameter.Dexterity / 2 + lhs.Critical));
        battleParameter.Evasion = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParamListArray[15], skillParamClampArray[15], (double) ((battleParameter.Agility * 3 + battleParameter.Luck) / 2 + lhs.Evasion + proficiencyIncr.evasion));
        Tuple<float, float> tuple1 = BattleFuncs.calcSkillParam2(skillParamListArray[1], skillParamClampArray[1], (float) playerUnit.total_strength, 1f, (float) lhs.Strength, skillParamListArray[9], skillParamClampArray[9], (float) (num1 + proficiencyIncr.physical_attack), 1f, 0.0f);
        Tuple<float, float> tuple2 = BattleFuncs.calcSkillParam2(skillParamListArray[2], skillParamClampArray[2], (float) playerUnit.total_intelligence, 1f, (float) lhs.Intelligence, skillParamListArray[11], skillParamClampArray[11], (float) (playerUnit.MinMagicBulletPower + num2 + proficiencyIncr.magic_attack), 1f, 0.0f);
        Tuple<float, float> tuple3 = BattleFuncs.calcSkillParam2(skillParamListArray[3], skillParamClampArray[3], (float) playerUnit.total_vitality, 1f, (float) lhs.Vitality, skillParamListArray[10], skillParamClampArray[10], (float) lhs.PhysicalDefense, 1f, 0.0f);
        Tuple<float, float> tuple4 = BattleFuncs.calcSkillParam2(skillParamListArray[4], skillParamClampArray[4], (float) playerUnit.total_mind, 1f, (float) lhs.Mind, skillParamListArray[12], skillParamClampArray[12], (float) lhs.MagicDefense, 1f, 0.0f);
        battleParameter.Strength = (int) tuple1.Item1;
        battleParameter.PhysicalAttack = (int) tuple1.Item2;
        battleParameter.Intelligence = (int) tuple2.Item1;
        battleParameter.MagicAttack = (int) tuple2.Item2;
        battleParameter.Vitality = (int) tuple3.Item1;
        battleParameter.PhysicalDefense = (int) tuple3.Item2;
        battleParameter.Mind = (int) tuple4.Item1;
        battleParameter.MagicDefense = (int) tuple4.Item2;
        battleParameter.Combat = battleParameter.Hp >= 5000 ? battleParameter.PhysicalAttack + battleParameter.PhysicalDefense + battleParameter.MagicAttack + battleParameter.MagicDefense + (battleParameter.Hit + battleParameter.Critical + battleParameter.Evasion) / 2 + 5000 + (int) ((double) (battleParameter.Hp - 5000) * 0.005) : battleParameter.PhysicalAttack + battleParameter.PhysicalDefense + battleParameter.MagicAttack + battleParameter.MagicDefense + (battleParameter.Hit + battleParameter.Critical + battleParameter.Evasion) / 2 + battleParameter.Hp;
        Judgement.NonBattleParameter nonBattleParameter = Judgement.NonBattleParameter.FromPlayerUnit(playerUnit);
        battleParameter.HpIncr = battleParameter.Hp - nonBattleParameter.Hp;
        battleParameter.StrengthIncr = battleParameter.Strength - nonBattleParameter.Strength;
        battleParameter.IntelligenceIncr = battleParameter.Intelligence - nonBattleParameter.Intelligence;
        battleParameter.VitalityIncr = battleParameter.Vitality - nonBattleParameter.Vitality;
        battleParameter.MindIncr = battleParameter.Mind - nonBattleParameter.Mind;
        battleParameter.AgilityIncr = battleParameter.Agility - nonBattleParameter.Agility;
        battleParameter.DexterityIncr = battleParameter.Dexterity - nonBattleParameter.Dexterity;
        battleParameter.LuckIncr = battleParameter.Luck - nonBattleParameter.Luck;
        battleParameter.MoveIncr = battleParameter.Move - nonBattleParameter.Move;
        battleParameter.PhysicalAttackIncr = battleParameter.PhysicalAttack - nonBattleParameter.PhysicalAttack;
        battleParameter.PhysicalDefenseIncr = battleParameter.PhysicalDefense - nonBattleParameter.PhysicalDefense;
        battleParameter.MagicAttackIncr = battleParameter.MagicAttack - nonBattleParameter.MagicAttack;
        battleParameter.MagicDefenseIncr = battleParameter.MagicDefense - nonBattleParameter.MagicDefense;
        battleParameter.HitIncr = battleParameter.Hit - nonBattleParameter.Hit;
        battleParameter.CriticalIncr = battleParameter.Critical - nonBattleParameter.Critical;
        battleParameter.EvasionIncr = battleParameter.Evasion - nonBattleParameter.Evasion;
        battleParameter.CombatIncr = battleParameter.Combat - nonBattleParameter.Combat;
        return battleParameter;
      }

      public bool GetParamsValue(Judgement.Params param, out int value)
      {
        switch (param)
        {
          case Judgement.Params.Hp:
            value = this.Hp;
            break;
          case Judgement.Params.Strength:
            value = this.Strength;
            break;
          case Judgement.Params.Intelligence:
            value = this.Intelligence;
            break;
          case Judgement.Params.Vitality:
            value = this.Vitality;
            break;
          case Judgement.Params.Mind:
            value = this.Mind;
            break;
          case Judgement.Params.Agility:
            value = this.Agility;
            break;
          case Judgement.Params.Dexterity:
            value = this.Dexterity;
            break;
          case Judgement.Params.Luck:
            value = this.Luck;
            break;
          case Judgement.Params.Move:
            value = this.Move;
            break;
          case Judgement.Params.PhysicalAttack:
            value = this.PhysicalAttack;
            break;
          case Judgement.Params.PhysicalDefense:
            value = this.PhysicalDefense;
            break;
          case Judgement.Params.MagicAttack:
            value = this.MagicAttack;
            break;
          case Judgement.Params.MagicDefense:
            value = this.MagicDefense;
            break;
          case Judgement.Params.Hit:
            value = this.Hit;
            break;
          case Judgement.Params.Critical:
            value = this.Critical;
            break;
          case Judgement.Params.Evasion:
            value = this.Evasion;
            break;
          default:
            value = 0;
            return false;
        }
        return true;
      }
    }

    [Serializable]
    public class BeforeDuelUnitParameter
    {
      public int Hp;
      public int Strength;
      public int Intelligence;
      public int Vitality;
      public int Mind;
      public int Agility;
      public int Dexterity;
      public int Luck;
      public int Move;
      public int PhysicalAttack;
      public int PhysicalDefense;
      public int MagicAttack;
      public int MagicDefense;
      public int Hit;
      public int Critical;
      public int Evasion;
      public int CriticalEvasion;
      public int AttackSpeed;
      public bool IsDontEvasion;
      public List<BL.UseSkillEffect> useSkillEffects = new List<BL.UseSkillEffect>();

      public static Judgement.BeforeDuelUnitParameter FromBeUnit(
        BL.ISkillEffectListUnit beUnit,
        BL.ISkillEffectListUnit beTarget,
        BattleLandform landform,
        BL.Unit[] neighborUnits,
        BL.MagicBullet beMagicBullet,
        int attackType,
        int distance,
        Judgement.BeforeDuelUnitParameter.FromBeUnitWork work,
        Judgement.BeforeDuelUnitParameter.FromBeUnitData data = null)
      {
        int distance1;
        int distance2;
        int num1;
        int num2;
        BL.ForceID[] forceIdArray;
        BL.Panel panel1;
        BL.Panel panel2;
        BL.ISkillEffectListUnit effectUnit1;
        IEnumerable<BL.Unit> unitDeckUnits;
        IEnumerable<BL.Unit> targetDeckUnits;
        bool flag1;
        bool isAI;
        bool? isMagic;
        bool flag2;
        BL.Panel effectPanel;
        if (data != null)
        {
          distance1 = data.move_distance;
          distance2 = data.move_range;
          num1 = data.attackHp;
          num2 = data.defenseHp;
          forceIdArray = data.targetForceId;
          panel1 = data.panel;
          panel2 = data.targetPanel;
          effectUnit1 = data.raidMissionUnit;
          unitDeckUnits = data.deckUnits;
          targetDeckUnits = data.targetDeckUnits;
          flag1 = data.isHeal;
          isAI = data.isAI;
          isMagic = data.isMagic;
          flag2 = data.disableUseCountSkillEffect;
          effectPanel = data.targetPanelNonNull;
        }
        else
        {
          distance1 = 0;
          distance2 = -1;
          num1 = 0;
          num2 = 0;
          forceIdArray = (BL.ForceID[]) null;
          panel1 = (BL.Panel) null;
          panel2 = (BL.Panel) null;
          effectUnit1 = (BL.ISkillEffectListUnit) null;
          unitDeckUnits = (IEnumerable<BL.Unit>) null;
          targetDeckUnits = (IEnumerable<BL.Unit>) null;
          flag1 = false;
          isAI = false;
          isMagic = new bool?();
          flag2 = false;
          effectPanel = (BL.Panel) null;
        }
        if (num1 <= 0)
          num1 = beUnit.hp;
        if (num2 <= 0)
          num2 = beTarget.hp;
        PlayerUnit playerUnit = beUnit.originalUnit.playerUnit;
        Judgement.GearParameter lhs = playerUnit.equippedGear == (PlayerItem) null ? Judgement.GearParameter.FromGearGear(playerUnit.equippedGearOrInitial) : Judgement.GearParameter.FromPlayerGear(playerUnit.equippedGear);
        Judgement.GearParameter rhs1 = playerUnit.equippedReisou == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou);
        if (rhs1 != null)
          lhs = Judgement.GearParameter.AddReisou(lhs, rhs1);
        if (playerUnit.equippedGear2 != (PlayerItem) null)
        {
          Judgement.GearParameter gearParameter = Judgement.GearParameter.FromPlayerGear(playerUnit.equippedGear2);
          Judgement.GearParameter rhs2 = playerUnit.equippedReisou2 == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou2);
          if (rhs2 != null)
            gearParameter = Judgement.GearParameter.AddReisou(gearParameter, rhs2);
          lhs = !(playerUnit.equippedGear == (PlayerItem) null) ? Judgement.GearParameter.Mix(lhs, gearParameter) : gearParameter;
        }
        if (playerUnit.equippedReisou3 != (PlayerItem) null)
        {
          Judgement.GearParameter rhs3 = Judgement.GearParameter.FromPlayerGear(playerUnit.equippedReisou3);
          if (lhs != null)
            lhs = Judgement.GearParameter.AddReisou(lhs, rhs3);
        }
        Judgement.BeforeDuelUnitParameter r = new Judgement.BeforeDuelUnitParameter();
        BattleLandformIncr incr = landform.GetIncr(beUnit.originalUnit);
        BattleLandformIncr.LandformDuelSkillIncr duelSkillIncr = incr.GetDuelSkillIncr(beUnit.originalUnit, beTarget.originalUnit, attackType);
        float hpRatio = 0.0f;
        int absoluteTurnCount = BattleFuncs.getPhaseState() != null ? BattleFuncs.getPhaseState().absoluteTurnCount : 0;
        List<BattleFuncs.SkillParam>[] skillParams1 = new List<BattleFuncs.SkillParam>[18];
        BattleFuncs.SkillParamClamp[] skillParamClampArray = new BattleFuncs.SkillParamClamp[18];
        for (int index = 0; index < 18; ++index)
        {
          skillParams1[index] = new List<BattleFuncs.SkillParam>();
          skillParamClampArray[index] = new BattleFuncs.SkillParamClamp();
        }
        if (panel1 != null)
        {
          foreach (BL.SkillEffect effect in panel1.getSkillEffects(isAI).value)
          {
            int optTest4 = effect.effect.EffectLogic.opt_test4;
            List<BattleFuncs.SkillParam> skillParamList = skillParams1[optTest4];
            if (effect.effect.EffectLogic.opt_test3 == 7 && panel2 != null && Judgement.CheckEnabledCharismaPanelBuffDebuff(effect, beUnit, beTarget, isAI))
            {
              if (effect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(effect.parentUnit, effect, (float) (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + effect.baseSkillLevel * effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio))));
              else
                skillParamList.Add(BattleFuncs.SkillParam.CreateMul(effect.parentUnit, effect, effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (float) effect.baseSkillLevel * effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)));
            }
          }
        }
        for (int process = 0; process < 4; ++process)
        {
          int effectTarget = process != 3 ? 0 : 1;
          BL.ISkillEffectListUnit effectUnit2 = process != 3 ? beUnit : beTarget;
          bool isHp = process == 0;
          Dictionary<int, List<BL.SkillEffect>> useRemainSkillEffects = new Dictionary<int, List<BL.SkillEffect>>();
          foreach (object processEffect in effectUnit2.skillEffects.GetProcessEffects(process))
          {
            BL.SkillEffect skillEffect = !(processEffect is List<BL.SkillEffect> effects) ? (BL.SkillEffect) processEffect : effects[0];
            int optTest4 = skillEffect.effect.EffectLogic.opt_test4;
            List<BattleFuncs.SkillParam> skillParamList = skillParams1[optTest4];
            int num3 = 0;
            bool flag3 = true;
            BattleFuncs.SkillParamClamp skillParamClamp = skillParamClampArray[skillEffect.effect.EffectLogic.opt_test4];
            switch (skillEffect.effect.EffectLogic.opt_test3)
            {
              case 0:
                if (Judgement.CheckEnabledBuffDebuff(skillEffect, beUnit, beTarget, attackType, BattleskillInvokeGameModeEnum.quest, panel1, isHp, isAI))
                {
                  num3 = 1;
                  break;
                }
                continue;
              case 1:
                if (Judgement.CheckEnabledEquipGearBuffDebuff(skillEffect, beUnit, beTarget))
                  break;
                continue;
              case 2:
                skillEffect = Judgement.GetEnabledRangeBuffDebuff(effects, beUnit, beTarget, distance, attackType);
                if (skillEffect != null)
                  break;
                continue;
              case 3:
                if (distance1 > 0)
                {
                  skillEffect = Judgement.GetEnabledRangeBuffDebuff(effects, beUnit, beTarget, distance1, attackType);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 4:
                skillEffect = Judgement.GetEnabledHpLeBuffDebuff(effects, beUnit, beTarget, hpRatio);
                if (skillEffect != null)
                  break;
                continue;
              case 5:
                skillEffect = Judgement.GetEnabledHpGeBuffDebuff(effects, beUnit, beTarget, hpRatio);
                if (skillEffect != null)
                  break;
                continue;
              case 6:
                if (forceIdArray != null && panel1 != null && Judgement.CheckEnabledTargetCountBuffDebuff(skillEffect, beUnit, beTarget, forceIdArray, panel1, isAI))
                {
                  int num4 = BattleFuncs.getTargets(panel1.row, panel1.column, new int[2]
                  {
                    skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_range),
                    skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_range)
                  }, forceIdArray, BL.Unit.TargetAttribute.all, (isAI ? 1 : 0) != 0, nonFacility: true).Count<BL.UnitPosition>();
                  int num5 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_target_count);
                  if (num5 >= 1 && num4 > num5)
                    num4 = num5;
                  if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                    skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit2.originalUnit, skillEffect, (float) (num4 * (int) ((double) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_lv_add) + (double) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_lv_mul) * (double) (skillEffect.baseSkillLevel - 1)))));
                  else
                    skillParamList.Add(BattleFuncs.SkillParam.CreateMul(effectUnit2.originalUnit, skillEffect, (float) (1.0 + (double) num4 * ((double) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_lv_add) + (double) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_lv_mul) * (double) (skillEffect.baseSkillLevel - 1)))));
                  num3 = -1;
                  break;
                }
                continue;
              case 7:
                if (panel1 == null || panel2 == null || !Judgement.CheckEnabledCharismaBuffDebuff(skillEffect, effectUnit2, effectTarget, beUnit, beTarget, effectTarget == 0 ? 0 : distance, panel1, effectTarget == 0 ? panel1 : panel2, isAI))
                  continue;
                break;
              case 8:
                if (distance2 >= 0)
                {
                  skillEffect = Judgement.GetEnabledCavalryRushBuffDebuff(effects, beUnit, beTarget, distance2, attackType);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 9:
                if (effectUnit1 == null || beUnit.originalUnit.isFacility || effectUnit2 != effectUnit1 || !Judgement.CheckEnabledRaidMissionBuffDebuff(skillEffect, effectUnit1, effectTarget, beUnit, beTarget))
                  continue;
                break;
              case 10:
                skillEffect = Judgement.GetEnabledExtremeOfForceBuffDebuff(effects, beUnit, beTarget);
                if (skillEffect != null)
                  break;
                continue;
              case 11:
                if (panel1 != null)
                {
                  skillEffect = Judgement.GetEnabledOnemanChargeBuffDebuff(effects, beUnit, beTarget, forceIdArray, panel1, isAI);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 12:
                if (landform.in_out != BattleInOutSide.outside || !Judgement.CheckEnabledInOutSideBattleBuffDebuff(skillEffect, beUnit, beTarget))
                  continue;
                break;
              case 13:
                if (landform.in_out != BattleInOutSide.inside || !Judgement.CheckEnabledInOutSideBattleBuffDebuff(skillEffect, beUnit, beTarget))
                  continue;
                break;
              case 14:
                if (Judgement.CheckEnabledEvenIllusionBuffDebuff(skillEffect, beUnit, beTarget))
                  break;
                continue;
              case 15:
                if (flag1 && !isHp || !Judgement.CheckEnabledSpecificUnitBuffDebuff(skillEffect, beUnit, beTarget, isHp))
                  continue;
                break;
              case 16:
                skillEffect = Judgement.GetEnabledUnitRarityBuffDebuff(effects, beUnit, beTarget);
                if (skillEffect != null)
                  break;
                continue;
              case 17:
                if (unitDeckUnits != null)
                {
                  skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, beUnit, beTarget, absoluteTurnCount, unitDeckUnits, (IEnumerable<BL.Unit>) null, BattleskillInvokeGameModeEnum.quest, isAI);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 18:
                if (targetDeckUnits != null)
                {
                  skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, beUnit, beTarget, absoluteTurnCount, (IEnumerable<BL.Unit>) null, targetDeckUnits, BattleskillInvokeGameModeEnum.quest, isAI);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 19:
                if (unitDeckUnits != null && targetDeckUnits != null)
                {
                  skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, beUnit, beTarget, absoluteTurnCount, unitDeckUnits, targetDeckUnits, BattleskillInvokeGameModeEnum.quest, isAI);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 20:
                if (Judgement.CheckEnabledBuffDebuff2(skillEffect, beUnit, beTarget, attackType, BattleskillInvokeGameModeEnum.quest, isHp, isMagic))
                {
                  num3 = 2;
                  break;
                }
                continue;
              case 21:
                if (Judgement.CheckEnabledSpecificGroupBuffDebuff(skillEffect, beUnit, beTarget, isHp))
                  break;
                continue;
              case 22:
                if (flag1 && !isHp || !Judgement.CheckEnabledSpecificGroupBuffDebuff(skillEffect, beUnit, beTarget, isHp))
                  continue;
                break;
              case 23:
                if (Judgement.CheckEnabledSpecificSkillGroupBuffDebuff(skillEffect, beUnit, beTarget, !isHp && flag1, isHp))
                  break;
                continue;
              case 24:
                if (Judgement.CheckEnabledEnemyBuffDebuff(skillEffect, beUnit, beTarget, attackType, BattleskillInvokeGameModeEnum.quest))
                  break;
                continue;
              case 25:
                if (Judgement.CheckEnabledParamDiffBuffDebuff(skillEffect, beUnit, beTarget, work, num1, num2))
                  break;
                continue;
              case 26:
                if (Judgement.CheckEnabledParamDiffEnemyBuffDebuff(skillEffect, beUnit, beTarget, work, num1, num2))
                  break;
                continue;
              case 27:
                if (Judgement.CheckEnabledParamDiffBuffDebuff(skillEffect, beUnit, beTarget, work, num1, num2))
                {
                  float num6 = skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_diff) + (float) skillEffect.baseSkillLevel * skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
                  int paramDiffValue = BattleFuncs.GetParamDiffValue(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type), work.self.nbpCache, num1);
                  int num7 = Math.Abs(BattleFuncs.GetParamDiffValue(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type), work.target.nbpCache, num2) - paramDiffValue);
                  int addParam = (double) num6 < 0.0 ? Mathf.FloorToInt((float) ((Decimal) num7 * (Decimal) num6)) : Mathf.CeilToInt((float) ((Decimal) num7 * (Decimal) num6));
                  skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit2.originalUnit, skillEffect, (float) addParam));
                  num3 = -1;
                  break;
                }
                continue;
              case 28:
                if (Judgement.CheckEnabledParamDiffEnemyBuffDebuff(skillEffect, beUnit, beTarget, work, num1, num2))
                {
                  float num8 = skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_diff) + (float) skillEffect.baseSkillLevel * skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
                  int paramDiffValue = BattleFuncs.GetParamDiffValue(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type), work.target.nbpCache, num2);
                  int num9 = Math.Abs(BattleFuncs.GetParamDiffValue(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type), work.self.nbpCache, num1) - paramDiffValue);
                  int addParam = (double) num8 < 0.0 ? Mathf.FloorToInt((float) ((Decimal) num9 * (Decimal) num8)) : Mathf.CeilToInt((float) ((Decimal) num9 * (Decimal) num8));
                  skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit2.originalUnit, skillEffect, (float) addParam));
                  num3 = -1;
                  break;
                }
                continue;
              case 29:
                if (Judgement.CheckEnabledBuffDebuff3(skillEffect, beUnit, beTarget, attackType))
                {
                  num3 = 3;
                  break;
                }
                continue;
              case 30:
                if (Judgement.CheckEnabledBuffDebuff4(skillEffect, beUnit, beTarget, attackType, isHp))
                {
                  num3 = 4;
                  break;
                }
                continue;
              case 31:
                if (Judgement.CheckEnabledAttackClassBuffDebuff(skillEffect, beUnit, beTarget, isHp))
                  break;
                continue;
              case 32:
                if (Judgement.CheckEnabledAttackElementBuffDebuff(skillEffect, beUnit, beTarget, isHp))
                  break;
                continue;
              case 33:
                if (Judgement.CheckEnabledInvestLogicBuffDebuff(skillEffect, beUnit, beTarget, attackType))
                  break;
                continue;
              case 34:
                if (Judgement.CheckEnabledEnemyInvestLogicBuffDebuff(skillEffect, beUnit, beTarget, attackType))
                  break;
                continue;
              case 35:
                if (Judgement.CheckEnabledEvenIllusion2BuffDebuff(skillEffect, beUnit, beTarget, BattleFuncs.getPhaseState().absoluteTurnCount, BattleskillInvokeGameModeEnum.quest))
                {
                  flag3 = false;
                  break;
                }
                continue;
              case 36:
                if (Judgement.CheckEnabledEvenIllusion3BuffDebuff(skillEffect, beUnit, beTarget, BattleFuncs.getPhaseState().absoluteTurnCount, BattleskillInvokeGameModeEnum.quest))
                {
                  flag3 = false;
                  break;
                }
                continue;
              case 37:
                if (Judgement.CheckEnabledPeculiarParameterRangeBuffDebuff(skillEffect, beUnit, beTarget, attackType))
                  break;
                continue;
              case 38:
                if (Judgement.CheckEnabledEnemyPeculiarParameterRangeBuffDebuff(skillEffect, beUnit, beTarget, attackType))
                  break;
                continue;
              case 39:
                if (Judgement.CheckEnabledLevelUpStatusBuffDebuff(skillEffect, beUnit, beTarget, attackType, isHp))
                  break;
                continue;
              case 40:
                if ((!flag1 || isHp) && !flag2 && Judgement.CheckEnabledUseRemainBuffDebuff(skillEffect, beUnit, beTarget, attackType))
                {
                  if (!useRemainSkillEffects.ContainsKey(optTest4))
                    useRemainSkillEffects[optTest4] = new List<BL.SkillEffect>();
                  useRemainSkillEffects[optTest4].Add(skillEffect);
                  num3 = -1;
                  break;
                }
                continue;
              case 41:
              case 42:
                if (skillEffect.effect.EffectLogic.opt_test3 == 41)
                {
                  if (!Judgement.CheckEnabledBuffDebuffClamp(skillEffect, beUnit, beTarget, attackType, panel1, isAI))
                    continue;
                }
                else if (!Judgement.CheckEnabledEnemyBuffDebuffClamp(skillEffect, beUnit, beTarget, attackType, effectPanel, isAI))
                  continue;
                if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                {
                  int num10 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value);
                  int num11 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_value);
                  if (skillParamClamp.fixMax.HasValue)
                  {
                    int num12 = num10;
                    int? fixMax = skillParamClamp.fixMax;
                    int valueOrDefault = fixMax.GetValueOrDefault();
                    if (!(num12 < valueOrDefault & fixMax.HasValue))
                      goto label_103;
                  }
                  skillParamClamp.fixMax = new int?(num10);
label_103:
                  if (skillParamClamp.fixMin.HasValue)
                  {
                    int num13 = num11;
                    int? fixMin = skillParamClamp.fixMin;
                    int valueOrDefault = fixMin.GetValueOrDefault();
                    if (!(num13 > valueOrDefault & fixMin.HasValue))
                      goto label_112;
                  }
                  skillParamClamp.fixMin = new int?(num11);
                }
                else
                {
                  Decimal num14 = (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage);
                  Decimal num15 = (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_percentage);
                  if (skillParamClamp.ratioMax.HasValue)
                  {
                    Decimal num16 = num14;
                    Decimal? ratioMax = skillParamClamp.ratioMax;
                    Decimal valueOrDefault = ratioMax.GetValueOrDefault();
                    if (!(num16 < valueOrDefault & ratioMax.HasValue))
                      goto label_109;
                  }
                  skillParamClamp.ratioMax = new Decimal?(num14);
label_109:
                  if (skillParamClamp.ratioMin.HasValue)
                  {
                    Decimal num17 = num15;
                    Decimal? ratioMin = skillParamClamp.ratioMin;
                    Decimal valueOrDefault = ratioMin.GetValueOrDefault();
                    if (!(num17 > valueOrDefault & ratioMin.HasValue))
                      goto label_112;
                  }
                  skillParamClamp.ratioMin = new Decimal?(num15);
                }
label_112:
                num3 = -1;
                break;
              case 43:
                if (Judgement.CheckEnabledGenericBuffDebuff(skillEffect, beUnit, beTarget, new int?(), work.self.nbpCache, work.target.nbpCache, new int?(num1), new int?(num2), attackType, panel1, panel2, isHp))
                  break;
                continue;
              case 44:
                if (!Judgement.CheckEnabledEnemyGenericBuffDebuff(skillEffect, beUnit, beTarget, new int?(), work.self.nbpCache, work.target.nbpCache, new int?(num1), new int?(num2), attackType, panel1, panel2))
                  continue;
                break;
              default:
                continue;
            }
            if (num3 >= 0)
            {
              if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
              {
                int addParam = flag3 ? skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + skillEffect.baseSkillLevel * skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio) : skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value);
                skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit2.originalUnit, skillEffect, (float) addParam, (object) num3));
              }
              else
              {
                float mulParam = flag3 ? (float) ((Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) skillEffect.baseSkillLevel * (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)) : skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
                skillParamList.Add(BattleFuncs.SkillParam.CreateMul(effectUnit2.originalUnit, skillEffect, mulParam, (object) num3));
              }
            }
          }
          if (!flag2)
            Judgement.ApplyUseRemainSkillEffect(useRemainSkillEffects, skillParams1, effectUnit2, r);
          if (process == 0)
          {
            r.Hp = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[0], skillParamClampArray[0], (double) playerUnit.total_hp, duelSkillIncr.skillMulHp, (float) (lhs.Hp + duelSkillIncr.skillAddHp));
            hpRatio = (float) ((Decimal) num1 / (Decimal) r.Hp * 100M);
          }
        }
        IEnumerable<IGrouping<BattleskillEffectLogicEnum, Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>> groupings = beUnit.skillEffects.GetAllEffectParams().GroupBy<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, BattleskillEffectLogicEnum>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, BattleskillEffectLogicEnum>) (x => x.Item1));
        bool flag4 = false;
        foreach (IGrouping<BattleskillEffectLogicEnum, Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> source in groupings)
        {
          if (!flag4)
          {
            if (!BattleFuncs.isSkillsAndEffectsInvalid(beUnit, beTarget))
              flag4 = true;
            else
              break;
          }
          BL.SkillEffect skillEffect = source.First<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>().Item2;
          List<BattleFuncs.SkillParam> skillParams2 = skillParams1[skillEffect.effect.EffectLogic.opt_test4];
          switch (skillEffect.effect.EffectLogic.opt_test3)
          {
            case 1001:
            case 1002:
            case 1007:
            case 1008:
              if (unitDeckUnits != null)
              {
                List<BattleFuncs.SkillParam> sp1 = (List<BattleFuncs.SkillParam>) null;
                List<BattleFuncs.SkillParam> sp2 = (List<BattleFuncs.SkillParam>) null;
                foreach (Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> tuple in (IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>) source)
                {
                  if (!BattleFuncs.isSealedSkillEffect(beUnit, tuple.Item2) && !BattleFuncs.isEffectEnemyRangeAndInvalid(tuple.Item2, beUnit, beTarget))
                  {
                    if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                    {
                      if (sp1 == null)
                        sp1 = new List<BattleFuncs.SkillParam>();
                      sp1.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, tuple.Item2, tuple.Item3));
                    }
                    else
                    {
                      if (sp2 == null)
                        sp2 = new List<BattleFuncs.SkillParam>();
                      sp2.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, tuple.Item2, tuple.Item3));
                    }
                  }
                }
                if (sp1 != null)
                  Judgement.GetDeckEverySkillAddFilter(skillParams2, sp1);
                if (sp2 != null)
                {
                  Judgement.GetDeckEverySkillMulFilter(skillParams2, sp2);
                  continue;
                }
                continue;
              }
              continue;
            case 1003:
            case 1004:
            case 1005:
            case 1006:
            case 1009:
            case 1010:
            case 1011:
            case 1012:
              if (unitDeckUnits != null)
              {
                using (IEnumerator<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> enumerator = source.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> current = enumerator.Current;
                    if (!BattleFuncs.isSealedSkillEffect(beUnit, current.Item2) && !BattleFuncs.isEffectEnemyRangeAndInvalid(current.Item2, beUnit, beTarget))
                    {
                      if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                        skillParams2.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, current.Item2, current.Item3));
                      else
                        skillParams2.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, current.Item2, current.Item3));
                    }
                  }
                  continue;
                }
              }
              else
                continue;
            default:
              continue;
          }
        }
        UnitProficiencyIncr proficiencyIncr = playerUnit.ProficiencyIncr;
        float elementOrKindRatio = playerUnit.GetElementOrKindRatio(beTarget.originalUnit.playerUnit);
        Tuple<int, int> gearKindIncr = playerUnit.GetGearKindIncr(beTarget.originalUnit.playerUnit);
        BattleFuncs.BeforeDuelDuelSupport beforeDuelDuelSupport = BattleFuncs.GetBeforeDuelDuelSupport(beUnit, beTarget, neighborUnits);
        int power = beMagicBullet == null ? 0 : beMagicBullet.power;
        List<BattleFuncs.SkillParam> skillParams3 = new List<BattleFuncs.SkillParam>();
        List<BattleFuncs.SkillParam> skillParams4 = new List<BattleFuncs.SkillParam>();
        List<BattleFuncs.SkillParam> skillParams5 = new List<BattleFuncs.SkillParam>();
        List<BattleFuncs.SkillParam> skillParams6 = new List<BattleFuncs.SkillParam>();
        BattleFuncs.BuffDebuffSwapState swapState = BattleFuncs.BuffDebuffSwapState.Create(beUnit, panel: panel1);
        BattleFuncs.GetLandBlessingSkillAdd(skillParams3, beUnit, beTarget, BattleskillEffectLogicEnum.land_blessing_fix_physical_defense, landform, (BattleLandformEffectPhase) 0);
        BattleFuncs.GetLandBlessingSkillAdd(skillParams4, beUnit, beTarget, BattleskillEffectLogicEnum.land_blessing_fix_magic_defense, landform, (BattleLandformEffectPhase) 0);
        BattleFuncs.GetLandBlessingSkillAdd(skillParams5, beUnit, beTarget, BattleskillEffectLogicEnum.land_blessing_fix_hit, landform, (BattleLandformEffectPhase) 0);
        BattleFuncs.GetLandBlessingSkillAdd(skillParams6, beUnit, beTarget, BattleskillEffectLogicEnum.land_blessing_fix_evasion, landform, (BattleLandformEffectPhase) 0);
        BattleFuncs.GetLandBlessingSkillMul(skillParams3, beUnit, beTarget, BattleskillEffectLogicEnum.land_blessing_ratio_physical_defense, landform, (BattleLandformEffectPhase) 0);
        BattleFuncs.GetLandBlessingSkillMul(skillParams4, beUnit, beTarget, BattleskillEffectLogicEnum.land_blessing_ratio_magic_defense, landform, (BattleLandformEffectPhase) 0);
        BattleFuncs.GetLandBlessingSkillMul(skillParams5, beUnit, beTarget, BattleskillEffectLogicEnum.land_blessing_ratio_hit, landform, (BattleLandformEffectPhase) 0);
        BattleFuncs.GetLandBlessingSkillMul(skillParams6, beUnit, beTarget, BattleskillEffectLogicEnum.land_blessing_ratio_evasion, landform, (BattleLandformEffectPhase) 0);
        int num18 = BattleFuncs.calcSkillParamAdd(skillParams3);
        int num19 = BattleFuncs.calcSkillParamAdd(skillParams4);
        int num20 = BattleFuncs.calcSkillParamAdd(skillParams5);
        int num21 = BattleFuncs.calcSkillParamAdd(skillParams6);
        float num22 = BattleFuncs.calcSkillParamMul(skillParams3);
        float num23 = BattleFuncs.calcSkillParamMul(skillParams4);
        float num24 = BattleFuncs.calcSkillParamMul(skillParams5);
        float num25 = BattleFuncs.calcSkillParamMul(skillParams6);
        float num26 = incr.physical_defense_ratio_incr.HasValue ? (float) num18 : Mathf.Ceil((float) incr.physical_defense_incr * num22) + (float) num18;
        float num27 = incr.magic_defense_ratio_incr.HasValue ? (float) num19 : Mathf.Ceil((float) incr.magic_defense_incr * num23) + (float) num19;
        float num28 = incr.hit_ratio_incr.HasValue ? (float) num20 : Mathf.Ceil((float) incr.hit_incr * num24) + (float) num20;
        float num29 = incr.evasion_ratio_incr.HasValue ? (float) num21 : Mathf.Ceil((float) incr.evasion_incr * num25) + (float) num21;
        float num30 = incr.physical_defense_ratio_incr.HasValue ? incr.physical_defense_ratio_incr.Value * num22 : 1f;
        float num31 = incr.magic_defense_ratio_incr.HasValue ? incr.magic_defense_ratio_incr.Value * num23 : 1f;
        float num32 = incr.hit_ratio_incr.HasValue ? incr.hit_ratio_incr.Value * num24 : 1f;
        float num33 = incr.evasion_ratio_incr.HasValue ? incr.evasion_ratio_incr.Value * num25 : 1f;
        int[] numArray = (int[]) null;
        foreach (BL.SkillEffect skillEffect in beUnit.skillEffects.Where(BattleskillEffectLogicEnum.steal_effect))
        {
          if (numArray == null)
            numArray = new int[18];
          numArray[(int) skillEffect.work[0]] += (int) skillEffect.work[1];
        }
        r.Agility = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[5], skillParamClampArray[5], (double) playerUnit.total_agility, duelSkillIncr.skillMulAgility, (float) (lhs.Agility + duelSkillIncr.skillAddAgility + (numArray != null ? numArray[5] : 0)), swapState);
        r.Dexterity = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[6], skillParamClampArray[6], (double) playerUnit.total_dexterity, duelSkillIncr.skillMulDexterity, (float) (lhs.Dexterity + duelSkillIncr.skillAddDexterity + (numArray != null ? numArray[6] : 0)), swapState);
        r.Luck = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[7], skillParamClampArray[7], (double) playerUnit.total_lucky, duelSkillIncr.skillMulLuck, (float) (lhs.Luck + duelSkillIncr.skillAddLuck + (numArray != null ? numArray[7] : 0)), swapState);
        r.Move = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[8], skillParamClampArray[8], (double) playerUnit.move, duelSkillIncr.skillMulMove, (float) (duelSkillIncr.skillAddMove + (numArray != null ? numArray[8] : 0)));
        r.Hit = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[13], skillParamClampArray[13], (double) ((r.Dexterity * 3 + r.Luck) / 2 + lhs.Hit + proficiencyIncr.hit), duelSkillIncr.skillMulHit * num32, (float) ((double) (duelSkillIncr.skillAddHit + gearKindIncr.Item2 + beforeDuelDuelSupport.hit) + (double) num28 + (numArray != null ? (double) numArray[13] : 0.0)), swapState);
        r.Critical = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[14], skillParamClampArray[14], (double) (r.Dexterity / 2 + lhs.Critical), duelSkillIncr.skillMulCritical, (float) (duelSkillIncr.skillAddCritical + beforeDuelDuelSupport.critical + (numArray != null ? numArray[14] : 0)), swapState);
        r.Evasion = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[15], skillParamClampArray[15], (double) ((r.Agility * 3 + r.Luck) / 2 + lhs.Evasion + proficiencyIncr.evasion), duelSkillIncr.skillMulEvasion * num33, (float) ((double) (duelSkillIncr.skillAddEvasion + beforeDuelDuelSupport.evasion) + (double) num29 + (numArray != null ? (double) numArray[15] : 0.0)), swapState);
        r.CriticalEvasion = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[16], skillParamClampArray[16], (double) r.Luck, duelSkillIncr.skillMulCriticalEvasion, (float) (duelSkillIncr.skillAddCriticalEvasion + beforeDuelDuelSupport.criticalEvasion + (numArray != null ? numArray[16] : 0)), swapState);
        int physicalPower = lhs.PhysicalPower;
        int magicalPower = lhs.MagicalPower;
        Tuple<float, float> tuple1 = BattleFuncs.calcSkillParam2(skillParams1[1], skillParamClampArray[1], (float) playerUnit.total_strength, duelSkillIncr.skillMulStrength, (float) (lhs.Strength + duelSkillIncr.skillAddStrength + (numArray != null ? numArray[1] : 0)), skillParams1[9], skillParamClampArray[9], (float) physicalPower * elementOrKindRatio + (float) proficiencyIncr.physical_attack, duelSkillIncr.skillMulPhysicalAttack, (float) (duelSkillIncr.skillAddPhysicalAttack + gearKindIncr.Item1 + (numArray != null ? numArray[9] : 0)), swapState);
        Tuple<float, float> tuple2 = BattleFuncs.calcSkillParam2(skillParams1[2], skillParamClampArray[2], (float) playerUnit.total_intelligence, duelSkillIncr.skillMulIntelligence, (float) (lhs.Intelligence + duelSkillIncr.skillAddIntelligence + (numArray != null ? numArray[2] : 0)), skillParams1[11], skillParamClampArray[11], (float) power + (float) magicalPower * elementOrKindRatio + (float) proficiencyIncr.magic_attack, duelSkillIncr.skillMulMagicAttack, (float) (duelSkillIncr.skillAddMagicAttack + gearKindIncr.Item1 + (numArray != null ? numArray[11] : 0)), swapState);
        Tuple<float, float> tuple3 = BattleFuncs.calcSkillParam2(skillParams1[3], skillParamClampArray[3], (float) playerUnit.total_vitality, duelSkillIncr.skillMulVitality, (float) (lhs.Vitality + duelSkillIncr.skillAddVitality + (numArray != null ? numArray[3] : 0)), skillParams1[10], skillParamClampArray[10], (float) lhs.PhysicalDefense, duelSkillIncr.skillMulPhysicalDefense * num30, (float) ((double) duelSkillIncr.skillAddPhysicalDefense + (double) num26 + (numArray != null ? (double) numArray[10] : 0.0)), swapState);
        Tuple<float, float> tuple4 = BattleFuncs.calcSkillParam2(skillParams1[4], skillParamClampArray[4], (float) playerUnit.total_mind, duelSkillIncr.skillMulMind, (float) (lhs.Mind + duelSkillIncr.skillAddMind + (numArray != null ? numArray[4] : 0)), skillParams1[12], skillParamClampArray[12], (float) lhs.MagicDefense, duelSkillIncr.skillMulMagicDefense * num31, (float) ((double) duelSkillIncr.skillAddMagicDefense + (double) num27 + (numArray != null ? (double) numArray[12] : 0.0)), swapState);
        r.Strength = (int) tuple1.Item1;
        r.PhysicalAttack = (int) tuple1.Item2;
        r.Intelligence = (int) tuple2.Item1;
        r.MagicAttack = (int) tuple2.Item2;
        r.Vitality = (int) tuple3.Item1;
        r.PhysicalDefense = (int) tuple3.Item2;
        r.Mind = (int) tuple4.Item1;
        r.MagicDefense = (int) tuple4.Item2;
        int num34 = BattleFuncs.calcEquippedGearWeight(playerUnit.initial_gear, playerUnit.equippedGear, playerUnit.equippedGear2, playerUnit.equippedGear3);
        r.AttackSpeed = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[17], skillParamClampArray[17], (double) (r.Agility - num34 - (beMagicBullet == null ? 0 : beMagicBullet.weight)), extraAdd: numArray != null ? (float) numArray[17] : 0.0f, swapState: swapState);
        if (BattleFuncs.isCriticalGuardEnable(beTarget, beUnit, panel2))
          r.Critical = 0;
        r.IsDontEvasion = beUnit.IsDontEvasion;
        return r;
      }

      public static Judgement.BeforeDuelUnitParameter FromBeColosseumUnit(
        BL.Unit beUnit,
        BL.Unit beTarget,
        BL.Unit[] neighborUnits,
        BL.MagicBullet beMagicBullet,
        PlayerItem equipped_gear,
        PlayerItem equipped_gear2,
        PlayerItem equipped_gear3,
        PlayerItem equipped_reisou,
        PlayerItem equipped_reisou2,
        PlayerItem equipped_reisou3,
        int attackType,
        BL.Unit[] deckUnits,
        BL.Unit[] targetDeckUnits,
        int attackHp,
        int defenseHp,
        int battleCount,
        bool? isMagic,
        Judgement.BeforeDuelUnitParameter.FromBeUnitWork work,
        int colosseumTurn)
      {
        PlayerUnit playerUnit = beUnit.playerUnit;
        Judgement.GearParameter lhs = equipped_gear == (PlayerItem) null ? Judgement.GearParameter.FromGearGear(playerUnit.initial_gear) : Judgement.GearParameter.FromPlayerGear(equipped_gear);
        Judgement.GearParameter rhs1 = equipped_reisou == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(equipped_reisou);
        if (rhs1 != null)
          lhs = Judgement.GearParameter.AddReisou(lhs, rhs1);
        if (equipped_gear2 != (PlayerItem) null)
        {
          Judgement.GearParameter gearParameter = Judgement.GearParameter.FromPlayerGear(equipped_gear2);
          Judgement.GearParameter rhs2 = equipped_reisou2 == (PlayerItem) null ? (Judgement.GearParameter) null : Judgement.GearParameter.FromPlayerGear(equipped_reisou2);
          if (rhs2 != null)
            gearParameter = Judgement.GearParameter.AddReisou(gearParameter, rhs2);
          lhs = !(equipped_gear == (PlayerItem) null) ? Judgement.GearParameter.Mix(lhs, gearParameter) : gearParameter;
        }
        if (equipped_reisou3 != (PlayerItem) null)
        {
          Judgement.GearParameter rhs3 = Judgement.GearParameter.FromPlayerGear(equipped_reisou3);
          if (lhs != null)
            lhs = Judgement.GearParameter.AddReisou(lhs, rhs3);
        }
        Judgement.BeforeDuelUnitParameter r = new Judgement.BeforeDuelUnitParameter();
        float hpRatio = 0.0f;
        List<BattleFuncs.SkillParam>[] skillParams1 = new List<BattleFuncs.SkillParam>[18];
        BattleFuncs.SkillParamClamp[] skillParamClampArray = new BattleFuncs.SkillParamClamp[18];
        for (int index = 0; index < 18; ++index)
        {
          skillParams1[index] = new List<BattleFuncs.SkillParam>();
          skillParamClampArray[index] = new BattleFuncs.SkillParamClamp();
        }
        for (int process = 0; process < 4; ++process)
        {
          BL.Unit effectUnit = process != 3 ? beUnit : beTarget;
          bool isHp = process == 0;
          Dictionary<int, List<BL.SkillEffect>> useRemainSkillEffects = new Dictionary<int, List<BL.SkillEffect>>();
          foreach (object processEffect in effectUnit.skillEffects.GetProcessEffects(process))
          {
            BL.SkillEffect skillEffect = !(processEffect is List<BL.SkillEffect> effects) ? (BL.SkillEffect) processEffect : effects[0];
            int optTest4 = skillEffect.effect.EffectLogic.opt_test4;
            List<BattleFuncs.SkillParam> skillParamList = skillParams1[optTest4];
            int num1 = 0;
            bool flag = true;
            BattleFuncs.SkillParamClamp skillParamClamp = skillParamClampArray[skillEffect.effect.EffectLogic.opt_test4];
            switch (skillEffect.effect.EffectLogic.opt_test3)
            {
              case 0:
                if (Judgement.CheckEnabledBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType, BattleskillInvokeGameModeEnum.colosseum, (BL.Panel) null, isHp, false))
                {
                  num1 = 1;
                  break;
                }
                continue;
              case 1:
                if (Judgement.CheckEnabledEquipGearBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget))
                  break;
                continue;
              case 4:
                skillEffect = Judgement.GetEnabledHpLeBuffDebuff(effects, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, hpRatio);
                if (skillEffect != null)
                  break;
                continue;
              case 5:
                skillEffect = Judgement.GetEnabledHpGeBuffDebuff(effects, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, hpRatio);
                if (skillEffect != null)
                  break;
                continue;
              case 15:
                if (Judgement.CheckEnabledSpecificUnitBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, isHp))
                  break;
                continue;
              case 16:
                skillEffect = Judgement.GetEnabledUnitRarityBuffDebuff(effects, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget);
                if (skillEffect != null)
                  break;
                continue;
              case 17:
                if (deckUnits != null)
                {
                  skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, battleCount, (IEnumerable<BL.Unit>) deckUnits, (IEnumerable<BL.Unit>) null, BattleskillInvokeGameModeEnum.colosseum, false);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 18:
                if (targetDeckUnits != null)
                {
                  skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, battleCount, (IEnumerable<BL.Unit>) null, (IEnumerable<BL.Unit>) targetDeckUnits, BattleskillInvokeGameModeEnum.colosseum, false);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 19:
                if (deckUnits != null && targetDeckUnits != null)
                {
                  skillEffect = Judgement.GetEnabledDeadCountBuffDebuff(effects, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, battleCount, (IEnumerable<BL.Unit>) deckUnits, (IEnumerable<BL.Unit>) targetDeckUnits, BattleskillInvokeGameModeEnum.colosseum, false);
                  if (skillEffect != null)
                    break;
                  continue;
                }
                continue;
              case 20:
                if (Judgement.CheckEnabledBuffDebuff2(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType, BattleskillInvokeGameModeEnum.colosseum, isHp, isMagic))
                {
                  num1 = 2;
                  break;
                }
                continue;
              case 21:
                if (Judgement.CheckEnabledSpecificGroupBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, isHp))
                  break;
                continue;
              case 22:
                if (Judgement.CheckEnabledSpecificGroupBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, isHp))
                  break;
                continue;
              case 23:
                if (Judgement.CheckEnabledSpecificSkillGroupBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, false, isHp))
                  break;
                continue;
              case 24:
                if (Judgement.CheckEnabledEnemyBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType, BattleskillInvokeGameModeEnum.colosseum))
                  break;
                continue;
              case 25:
                if (Judgement.CheckEnabledParamDiffBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, work, attackHp, defenseHp))
                  break;
                continue;
              case 26:
                if (Judgement.CheckEnabledParamDiffEnemyBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, work, attackHp, defenseHp))
                  break;
                continue;
              case 27:
                if (Judgement.CheckEnabledParamDiffBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, work, attackHp, defenseHp))
                {
                  float num2 = skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_diff) + (float) skillEffect.baseSkillLevel * skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
                  int paramDiffValue = BattleFuncs.GetParamDiffValue(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type), work.self.nbpCache, attackHp);
                  int num3 = Math.Abs(BattleFuncs.GetParamDiffValue(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type), work.target.nbpCache, defenseHp) - paramDiffValue);
                  int addParam = (double) num2 < 0.0 ? Mathf.FloorToInt((float) ((Decimal) num3 * (Decimal) num2)) : Mathf.CeilToInt((float) ((Decimal) num3 * (Decimal) num2));
                  skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit.originalUnit, skillEffect, (float) addParam));
                  num1 = -1;
                  break;
                }
                continue;
              case 28:
                if (Judgement.CheckEnabledParamDiffEnemyBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, work, attackHp, defenseHp))
                {
                  float num4 = skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage_diff) + (float) skillEffect.baseSkillLevel * skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio);
                  int paramDiffValue = BattleFuncs.GetParamDiffValue(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.param_type), work.target.nbpCache, defenseHp);
                  int num5 = Math.Abs(BattleFuncs.GetParamDiffValue(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.target_param_type), work.self.nbpCache, attackHp) - paramDiffValue);
                  int addParam = (double) num4 < 0.0 ? Mathf.FloorToInt((float) ((Decimal) num5 * (Decimal) num4)) : Mathf.CeilToInt((float) ((Decimal) num5 * (Decimal) num4));
                  skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit.originalUnit, skillEffect, (float) addParam));
                  num1 = -1;
                  break;
                }
                continue;
              case 29:
                if (Judgement.CheckEnabledBuffDebuff3(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType))
                {
                  num1 = 3;
                  break;
                }
                continue;
              case 30:
                if (Judgement.CheckEnabledBuffDebuff4(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType, isHp))
                {
                  num1 = 4;
                  break;
                }
                continue;
              case 31:
                if (Judgement.CheckEnabledAttackClassBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, isHp))
                  break;
                continue;
              case 32:
                if (Judgement.CheckEnabledAttackElementBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, isHp))
                  break;
                continue;
              case 33:
                if (Judgement.CheckEnabledInvestLogicBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType))
                  break;
                continue;
              case 34:
                if (Judgement.CheckEnabledEnemyInvestLogicBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType))
                  break;
                continue;
              case 35:
                if (Judgement.CheckEnabledEvenIllusion2BuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, colosseumTurn, BattleskillInvokeGameModeEnum.colosseum))
                {
                  flag = false;
                  break;
                }
                continue;
              case 36:
                if (Judgement.CheckEnabledEvenIllusion3BuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, colosseumTurn, BattleskillInvokeGameModeEnum.colosseum))
                {
                  flag = false;
                  break;
                }
                continue;
              case 37:
                if (Judgement.CheckEnabledPeculiarParameterRangeBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType))
                  break;
                continue;
              case 38:
                if (Judgement.CheckEnabledEnemyPeculiarParameterRangeBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType))
                  break;
                continue;
              case 39:
                if (Judgement.CheckEnabledLevelUpStatusBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType, isHp))
                  break;
                continue;
              case 40:
                if (Judgement.CheckEnabledUseRemainBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType))
                {
                  if (!useRemainSkillEffects.ContainsKey(optTest4))
                    useRemainSkillEffects[optTest4] = new List<BL.SkillEffect>();
                  useRemainSkillEffects[optTest4].Add(skillEffect);
                  num1 = -1;
                  break;
                }
                continue;
              case 41:
              case 42:
                if (skillEffect.effect.EffectLogic.opt_test3 == 41)
                {
                  if (!Judgement.CheckEnabledBuffDebuffClamp(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType, (BL.Panel) null, false))
                    continue;
                }
                else if (!Judgement.CheckEnabledEnemyBuffDebuffClamp(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, attackType, (BL.Panel) null, false))
                  continue;
                if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                {
                  int num6 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.max_value);
                  int num7 = skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.min_value);
                  if (skillParamClamp.fixMax.HasValue)
                  {
                    int num8 = num6;
                    int? fixMax = skillParamClamp.fixMax;
                    int valueOrDefault = fixMax.GetValueOrDefault();
                    if (!(num8 < valueOrDefault & fixMax.HasValue))
                      goto label_67;
                  }
                  skillParamClamp.fixMax = new int?(num6);
label_67:
                  if (skillParamClamp.fixMin.HasValue)
                  {
                    int num9 = num7;
                    int? fixMin = skillParamClamp.fixMin;
                    int valueOrDefault = fixMin.GetValueOrDefault();
                    if (!(num9 > valueOrDefault & fixMin.HasValue))
                      goto label_76;
                  }
                  skillParamClamp.fixMin = new int?(num7);
                }
                else
                {
                  Decimal num10 = (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage);
                  Decimal num11 = (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_percentage);
                  if (skillParamClamp.ratioMax.HasValue)
                  {
                    Decimal num12 = num10;
                    Decimal? ratioMax = skillParamClamp.ratioMax;
                    Decimal valueOrDefault = ratioMax.GetValueOrDefault();
                    if (!(num12 < valueOrDefault & ratioMax.HasValue))
                      goto label_73;
                  }
                  skillParamClamp.ratioMax = new Decimal?(num10);
label_73:
                  if (skillParamClamp.ratioMin.HasValue)
                  {
                    Decimal num13 = num11;
                    Decimal? ratioMin = skillParamClamp.ratioMin;
                    Decimal valueOrDefault = ratioMin.GetValueOrDefault();
                    if (!(num13 > valueOrDefault & ratioMin.HasValue))
                      goto label_76;
                  }
                  skillParamClamp.ratioMin = new Decimal?(num11);
                }
label_76:
                num1 = -1;
                break;
              case 43:
                if (Judgement.CheckEnabledGenericBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, new int?(colosseumTurn), work.self.nbpCache, work.target.nbpCache, new int?(attackHp), new int?(defenseHp), attackType, (BL.Panel) null, (BL.Panel) null, isHp))
                  break;
                continue;
              case 44:
                if (!Judgement.CheckEnabledEnemyGenericBuffDebuff(skillEffect, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget, new int?(colosseumTurn), work.self.nbpCache, work.target.nbpCache, new int?(attackHp), new int?(defenseHp), attackType, (BL.Panel) null, (BL.Panel) null))
                  continue;
                break;
              default:
                continue;
            }
            if (num1 >= 0)
            {
              if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
              {
                int addParam = flag ? skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value) + skillEffect.baseSkillLevel * skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio) : skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.value);
                skillParamList.Add(BattleFuncs.SkillParam.CreateAdd(effectUnit.originalUnit, skillEffect, (float) addParam, (object) num1));
              }
              else
              {
                float mulParam = flag ? (float) ((Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage) + (Decimal) skillEffect.baseSkillLevel * (Decimal) skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio)) : skillEffect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.percentage);
                skillParamList.Add(BattleFuncs.SkillParam.CreateMul(effectUnit.originalUnit, skillEffect, mulParam, (object) num1));
              }
            }
          }
          Judgement.ApplyUseRemainSkillEffect(useRemainSkillEffects, skillParams1, (BL.ISkillEffectListUnit) effectUnit, r);
          if (process == 0)
          {
            r.Hp = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[0], skillParamClampArray[0], (double) playerUnit.total_hp, extraAdd: (float) lhs.Hp);
            hpRatio = (float) ((Decimal) attackHp / (Decimal) r.Hp * 100M);
          }
        }
        IEnumerable<IGrouping<BattleskillEffectLogicEnum, Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>> groupings = beUnit.skillEffects.GetAllEffectParams().GroupBy<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, BattleskillEffectLogicEnum>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, BattleskillEffectLogicEnum>) (x => x.Item1));
        bool flag1 = false;
        foreach (IGrouping<BattleskillEffectLogicEnum, Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> source in groupings)
        {
          if (!flag1)
          {
            if (!BattleFuncs.isSkillsAndEffectsInvalid((BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget))
              flag1 = true;
            else
              break;
          }
          BL.SkillEffect skillEffect = source.First<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>().Item2;
          List<BattleFuncs.SkillParam> skillParams2 = skillParams1[skillEffect.effect.EffectLogic.opt_test4];
          switch (skillEffect.effect.EffectLogic.opt_test3)
          {
            case 1001:
            case 1002:
            case 1007:
            case 1008:
              if (deckUnits != null)
              {
                List<BattleFuncs.SkillParam> sp1 = (List<BattleFuncs.SkillParam>) null;
                List<BattleFuncs.SkillParam> sp2 = (List<BattleFuncs.SkillParam>) null;
                foreach (Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> tuple in (IEnumerable<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>>) source)
                {
                  if (!BattleFuncs.isSealedSkillEffect((BL.ISkillEffectListUnit) beUnit, tuple.Item2) && !BattleFuncs.isEffectEnemyRangeAndInvalid(tuple.Item2, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget))
                  {
                    if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                    {
                      if (sp1 == null)
                        sp1 = new List<BattleFuncs.SkillParam>();
                      sp1.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, tuple.Item2, tuple.Item3));
                    }
                    else
                    {
                      if (sp2 == null)
                        sp2 = new List<BattleFuncs.SkillParam>();
                      sp2.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, tuple.Item2, tuple.Item3));
                    }
                  }
                }
                if (sp1 != null)
                  Judgement.GetDeckEverySkillAddFilter(skillParams2, sp1);
                if (sp2 != null)
                {
                  Judgement.GetDeckEverySkillMulFilter(skillParams2, sp2);
                  continue;
                }
                continue;
              }
              continue;
            case 1003:
            case 1004:
            case 1005:
            case 1006:
            case 1009:
            case 1010:
            case 1011:
            case 1012:
              if (deckUnits != null)
              {
                using (IEnumerator<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>> enumerator = source.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float> current = enumerator.Current;
                    if (!BattleFuncs.isSealedSkillEffect((BL.ISkillEffectListUnit) beUnit, current.Item2) && !BattleFuncs.isEffectEnemyRangeAndInvalid(current.Item2, (BL.ISkillEffectListUnit) beUnit, (BL.ISkillEffectListUnit) beTarget))
                    {
                      if (skillEffect.effect.EffectLogic.effect_tag2 == BattleskillEffectTag.fix_value)
                        skillParams2.Add(BattleFuncs.SkillParam.CreateAdd(beUnit.originalUnit, current.Item2, current.Item3));
                      else
                        skillParams2.Add(BattleFuncs.SkillParam.CreateMul(beUnit.originalUnit, current.Item2, current.Item3));
                    }
                  }
                  continue;
                }
              }
              else
                continue;
            default:
              continue;
          }
        }
        UnitProficiencyIncr proficiencyIncr = playerUnit.ProficiencyIncr;
        float elementOrKindRatio = playerUnit.GetElementOrKindRatio(beTarget.playerUnit);
        Tuple<int, int> gearKindIncr = playerUnit.GetGearKindIncr(beTarget.playerUnit);
        IntimateDuelSupport intimateDuelSupport = playerUnit.GetIntimateDuelSupport(((IEnumerable<BL.Unit>) neighborUnits).Select<BL.Unit, PlayerUnit>((Func<BL.Unit, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>());
        int power = beMagicBullet == null ? 0 : beMagicBullet.power;
        BattleFuncs.BuffDebuffSwapState swapState = BattleFuncs.BuffDebuffSwapState.Create((BL.ISkillEffectListUnit) beUnit);
        r.Agility = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[5], skillParamClampArray[5], (double) playerUnit.total_agility, extraAdd: (float) lhs.Agility, swapState: swapState);
        r.Dexterity = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[6], skillParamClampArray[6], (double) playerUnit.total_dexterity, extraAdd: (float) lhs.Dexterity, swapState: swapState);
        r.Luck = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[7], skillParamClampArray[7], (double) playerUnit.total_lucky, extraAdd: (float) lhs.Luck, swapState: swapState);
        r.Move = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[8], skillParamClampArray[8], (double) playerUnit.move);
        r.Hit = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[13], skillParamClampArray[13], (double) ((r.Dexterity * 3 + r.Luck) / 2 + lhs.Hit + proficiencyIncr.hit), extraAdd: (float) (gearKindIncr.Item2 + intimateDuelSupport.hit), swapState: swapState);
        r.Critical = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[14], skillParamClampArray[14], (double) (r.Dexterity / 2 + lhs.Critical), extraAdd: (float) intimateDuelSupport.critical, swapState: swapState);
        r.Evasion = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[15], skillParamClampArray[15], (double) ((r.Agility * 3 + r.Luck) / 2 + lhs.Evasion + proficiencyIncr.evasion), extraAdd: (float) intimateDuelSupport.evasion, swapState: swapState);
        r.CriticalEvasion = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[16], skillParamClampArray[16], (double) r.Luck, extraAdd: (float) intimateDuelSupport.critical_evasion, swapState: swapState);
        int physicalPower = lhs.PhysicalPower;
        int magicalPower = lhs.MagicalPower;
        Tuple<float, float> tuple1 = BattleFuncs.calcSkillParam2(skillParams1[1], skillParamClampArray[1], (float) playerUnit.total_strength, 1f, (float) lhs.Strength, skillParams1[9], skillParamClampArray[9], (float) physicalPower * elementOrKindRatio + (float) proficiencyIncr.physical_attack, 1f, (float) gearKindIncr.Item1, swapState);
        Tuple<float, float> tuple2 = BattleFuncs.calcSkillParam2(skillParams1[2], skillParamClampArray[2], (float) playerUnit.total_intelligence, 1f, (float) lhs.Intelligence, skillParams1[11], skillParamClampArray[11], (float) power + (float) magicalPower * elementOrKindRatio + (float) proficiencyIncr.magic_attack, 1f, (float) gearKindIncr.Item1, swapState);
        Tuple<float, float> tuple3 = BattleFuncs.calcSkillParam2(skillParams1[3], skillParamClampArray[3], (float) playerUnit.total_vitality, 1f, (float) lhs.Vitality, skillParams1[10], skillParamClampArray[10], (float) lhs.PhysicalDefense, 1f, 0.0f, swapState);
        Tuple<float, float> tuple4 = BattleFuncs.calcSkillParam2(skillParams1[4], skillParamClampArray[4], (float) playerUnit.total_mind, 1f, (float) lhs.Mind, skillParams1[12], skillParamClampArray[12], (float) lhs.MagicDefense, 1f, 0.0f, swapState);
        r.Strength = (int) tuple1.Item1;
        r.PhysicalAttack = (int) tuple1.Item2;
        r.Intelligence = (int) tuple2.Item1;
        r.MagicAttack = (int) tuple2.Item2;
        r.Vitality = (int) tuple3.Item1;
        r.PhysicalDefense = (int) tuple3.Item2;
        r.Mind = (int) tuple4.Item1;
        r.MagicDefense = (int) tuple4.Item2;
        int num = BattleFuncs.calcEquippedGearWeight(playerUnit.initial_gear, equipped_gear, equipped_gear2, equipped_gear3);
        r.AttackSpeed = (int) BattleFuncs.calcSkillParam((IEnumerable<BattleFuncs.SkillParam>) skillParams1[17], skillParamClampArray[17], (double) (r.Agility - num - (beMagicBullet == null ? 0 : beMagicBullet.weight)), swapState: swapState);
        if (BattleFuncs.isCriticalGuardEnable((BL.ISkillEffectListUnit) beTarget, (BL.ISkillEffectListUnit) beUnit))
          r.Critical = 0;
        r.IsDontEvasion = beUnit.IsDontEvasion;
        return r;
      }

      public class FromBeUnitWork
      {
        public Judgement.BeforeDuelUnitParameter.FromBeUnitWork.Unit self { get; private set; }

        public Judgement.BeforeDuelUnitParameter.FromBeUnitWork.Unit target { get; private set; }

        public FromBeUnitWork(BL.ISkillEffectListUnit selfUnit, BL.ISkillEffectListUnit targetUnit)
        {
          this.self = new Judgement.BeforeDuelUnitParameter.FromBeUnitWork.Unit(selfUnit);
          this.target = new Judgement.BeforeDuelUnitParameter.FromBeUnitWork.Unit(targetUnit);
        }

        public void Swap()
        {
          Judgement.BeforeDuelUnitParameter.FromBeUnitWork.Unit self = this.self;
          this.self = this.target;
          this.target = self;
        }

        public class Unit
        {
          private BL.ISkillEffectListUnit unit;
          private Judgement.NonBattleParameter.FromPlayerUnitCache cache;

          public Unit(BL.ISkillEffectListUnit unit) => this.unit = unit;

          public Judgement.NonBattleParameter.FromPlayerUnitCache nbpCache
          {
            get
            {
              if (this.cache == null)
                this.cache = new Judgement.NonBattleParameter.FromPlayerUnitCache(this.unit.originalUnit.playerUnit);
              return this.cache;
            }
          }
        }
      }

      public class FromBeUnitData
      {
        public int move_distance;
        public int move_range;
        public int attackHp;
        public int defenseHp;
        public BL.ForceID[] targetForceId;
        public BL.Panel panel;
        public BL.Panel targetPanel;
        public BL.ISkillEffectListUnit raidMissionUnit;
        public IEnumerable<BL.Unit> deckUnits;
        public IEnumerable<BL.Unit> targetDeckUnits;
        public bool isHeal;
        public bool isAI;
        public bool? isMagic;
        public bool disableUseCountSkillEffect;
        public BL.Panel targetPanelNonNull;
      }
    }

    [Serializable]
    public class BeforeDuelParameter
    {
      public int AttackCount;
      public int? FixDamage;
      public int? HitMin;
      public int? HitMax;
      public int? FixHit;
      public int? CriticalMin;
      public int? CriticalMax;
      public float DamageRate;
      public int BaseDamage;

      public int DisplayPhysicalAttack => (int) this.CalcPhysicalAttack(1f);

      public int DisplayMagicAttack => (int) this.CalcMagicAttack(1f);

      public float CalcPhysicalAttack(float rate, bool excludeBaseDamage = false)
      {
        return this.CalcAttack(this.attackerUnitParameter.PhysicalAttack, this.defenderUnitParameter.PhysicalDefense, rate, excludeBaseDamage);
      }

      public float CalcMagicAttack(float rate, bool excludeBaseDamage = false)
      {
        return this.CalcAttack(this.attackerUnitParameter.MagicAttack, this.defenderUnitParameter.MagicDefense, rate, excludeBaseDamage);
      }

      private float CalcAttack(int attack, int defense, float rate, bool excludeBaseDamage)
      {
        Decimal num1 = !this.FixDamage.HasValue ? (Decimal) attack * (Decimal) rate - (Decimal) defense : (Decimal) this.FixDamage.Value * (Decimal) rate;
        if (num1 < 0M)
          num1 = 1M;
        if (num1 > 2147483647M)
          num1 = 2147483647M;
        Decimal num2 = num1 * (Decimal) this.DamageRate;
        if (!excludeBaseDamage && this.BaseDamage != 0)
        {
          if (num2 < 1M)
            num2 = 1M;
          else if (num2 >= (Decimal) Judgement.MaximumDamageValue)
            num2 = (Decimal) Judgement.MaximumDamageValue;
          num2 += (Decimal) this.BaseDamage;
        }
        if (num2 >= (Decimal) Judgement.MaximumDamageValue)
          num2 = (Decimal) Judgement.MaximumDamageValue;
        return Mathf.Max(1f, (float) num2);
      }

      public int DisplayHit
      {
        get
        {
          if (this.defenderUnitParameter.IsDontEvasion)
            return 100;
          int displayHit = !this.FixHit.HasValue ? this.attackerUnitParameter.Hit - this.defenderUnitParameter.Evasion : this.FixHit.Value;
          if (this.HitMax.HasValue && displayHit > this.HitMax.Value)
            displayHit = this.HitMax.Value;
          if (this.HitMin.HasValue && displayHit < this.HitMin.Value)
            displayHit = this.HitMin.Value;
          return displayHit;
        }
      }

      public int DisplayCritical
      {
        get
        {
          int num = this.defenderUnitParameter.CriticalEvasion;
          if (num < 0)
            num = 0;
          int displayCritical = this.attackerUnitParameter.Critical - num;
          if (this.CriticalMax.HasValue && displayCritical > this.CriticalMax.Value)
            displayCritical = this.CriticalMax.Value;
          if (this.CriticalMin.HasValue && displayCritical < this.CriticalMin.Value)
            displayCritical = this.CriticalMin.Value;
          return displayCritical;
        }
      }

      public Judgement.BeforeDuelUnitParameter attackerUnitParameter { get; protected set; }

      public Judgement.BeforeDuelUnitParameter defenderUnitParameter { get; protected set; }

      public static Judgement.BeforeDuelParameter CreateSingle(
        BL.ISkillEffectListUnit beAttackUnit,
        BL.MagicBullet beAttackMagicBullet,
        BattleLandform attackPanel,
        BL.Unit[] beAttackNeighborUnits,
        BL.ISkillEffectListUnit beDefenseUnit,
        BL.MagicBullet beDefenseMagicBullet,
        BattleLandform defensePanel,
        BL.Unit[] beDefenseNeighborUnits,
        bool isAttack,
        int distance = 0,
        int move_distance = 0,
        int move_range = -1,
        int attackHp = 0,
        int defenseHp = 0,
        BL.ForceID[] attackTargetForceId = null,
        BL.ForceID[] defenceTargetForceId = null,
        BL.Panel blAttackPanel = null,
        BL.Panel blDefencePanel = null,
        IEnumerable<BL.Unit> beAttackDeckUnits = null,
        IEnumerable<BL.Unit> beDefenseDeckUnits = null,
        IEnumerable<BL.Unit> beAttackTargetDeckUnits = null,
        IEnumerable<BL.Unit> beDefenseTargetDeckUnits = null,
        bool isHeal = false,
        bool isAI = false,
        bool? isMagic = null,
        bool checkInvokeAbsoluteCounterAttack = false,
        BL.Weapon weapon = null)
      {
        BL.ISkillEffectListUnit skillEffectListUnit = !checkInvokeAbsoluteCounterAttack ? (!isAttack || isHeal || blDefencePanel == null || blAttackPanel == null || BattleFuncs.isCounterAttack(beAttackUnit, blAttackPanel, beAttackNeighborUnits, beDefenseUnit, blDefencePanel, beDefenseNeighborUnits, defenseHp, isAI) ? (BL.ISkillEffectListUnit) null : beAttackUnit) : beDefenseUnit;
        Judgement.BeforeDuelUnitParameter.FromBeUnitWork work = new Judgement.BeforeDuelUnitParameter.FromBeUnitWork(beAttackUnit, beDefenseUnit);
        Judgement.BeforeDuelUnitParameter.FromBeUnitData data = new Judgement.BeforeDuelUnitParameter.FromBeUnitData()
        {
          move_distance = isAttack ? move_distance : 0,
          move_range = move_range,
          attackHp = attackHp,
          defenseHp = defenseHp,
          targetForceId = attackTargetForceId,
          panel = blAttackPanel,
          targetPanel = blDefencePanel,
          raidMissionUnit = skillEffectListUnit,
          deckUnits = beAttackDeckUnits,
          targetDeckUnits = beAttackTargetDeckUnits,
          isHeal = isHeal,
          isAI = isAI,
          isMagic = isMagic,
          disableUseCountSkillEffect = false,
          targetPanelNonNull = blDefencePanel
        };
        Judgement.BeforeDuelUnitParameter attack = Judgement.BeforeDuelUnitParameter.FromBeUnit(beAttackUnit, beDefenseUnit, attackPanel, beAttackNeighborUnits, beAttackMagicBullet, isAttack ? 1 : 2, distance, work, data);
        work.Swap();
        data.move_distance = !isAttack ? move_distance : 0;
        data.attackHp = defenseHp;
        data.defenseHp = attackHp;
        data.targetForceId = defenceTargetForceId;
        data.panel = blDefencePanel;
        data.targetPanel = blAttackPanel;
        data.deckUnits = beDefenseDeckUnits;
        data.targetDeckUnits = beDefenseTargetDeckUnits;
        data.isMagic = new bool?();
        data.targetPanelNonNull = blAttackPanel;
        Judgement.BeforeDuelUnitParameter defense = Judgement.BeforeDuelUnitParameter.FromBeUnit(beDefenseUnit, beAttackUnit, defensePanel, beDefenseNeighborUnits, beDefenseMagicBullet, isAttack ? 2 : 1, distance, work, data);
        Judgement.BeforeDuelParameter single = new Judgement.BeforeDuelParameter()
        {
          attackerUnitParameter = attack,
          defenderUnitParameter = defense
        };
        single.AttackCount = BattleFuncs.attackCount(beAttackUnit, beDefenseUnit);
        single.AttackCount *= beAttackUnit.originalUnit.playerUnit.normalAttackCount;
        if (BattleFuncs.canOneMore(attack, defense, beAttackUnit, beDefenseUnit, isAttack, attackPanel: blAttackPanel, defensePanel: blDefencePanel))
          single.AttackCount *= 2;
        single.FixDamage = Judgement.BeforeDuelParameter.MakeFixDamage(beAttackUnit, beAttackMagicBullet, defenseHp <= 0 ? beDefenseUnit.hp : defenseHp);
        Tuple<int?, int?, int?> tuple1 = Judgement.BeforeDuelParameter.MakeFixHit(beAttackUnit, beDefenseUnit, beAttackMagicBullet, blAttackPanel, distance, isAI, isMagic, blDefencePanel);
        single.HitMin = tuple1.Item1;
        single.HitMax = tuple1.Item2;
        single.FixHit = tuple1.Item3;
        Tuple<int?, int?> tuple2 = Judgement.BeforeDuelParameter.MakeFixCritical(beAttackUnit, beDefenseUnit, blAttackPanel, isMagic, blDefencePanel, isAI);
        single.CriticalMin = tuple2.Item1;
        single.CriticalMax = tuple2.Item2;
        single.DamageRate = Judgement.BeforeDuelParameter.MakeDamageRate(beAttackUnit, beDefenseUnit, true, new int?(), blAttackPanel, distance, isAI, isMagic, blDefencePanel, weapon, new bool?(isAttack), new int?(attackHp), new int?(defenseHp));
        single.BaseDamage = Judgement.BeforeDuelParameter.MakeBaseDamage(beAttackUnit, beDefenseUnit, blAttackPanel, isAI);
        return single;
      }

      public static Judgement.BeforeDuelParameter CreateDuelSkill(
        BL.ISkillEffectListUnit beAttackUnit,
        BL.MagicBullet beAttackMagicBullet,
        BL.Panel attackPanel,
        BL.ISkillEffectListUnit beDefenseUnit,
        BL.Panel defensePanel,
        int distance = 0,
        int defenseHp = 0,
        bool disableUseCountSkillEffect = true)
      {
        BL env = BattleFuncs.getEnv();
        bool flag = beAttackUnit is BL.AIUnit;
        BattleLandform landform1 = attackPanel.landform;
        BattleLandform landform2 = defensePanel.landform;
        BL.ForceID[] source1;
        BL.ForceID[] source2;
        IEnumerable<BL.Unit> units1;
        IEnumerable<BL.Unit> units2;
        IEnumerable<BL.Unit> units3;
        IEnumerable<BL.Unit> units4;
        if (env != null)
        {
          BL.ForceID forceId1 = env.getForceID(beAttackUnit.originalUnit);
          BL.ForceID forceId2 = env.getForceID(beDefenseUnit.originalUnit);
          source1 = env.getTargetForce(beAttackUnit.originalUnit, beAttackUnit.IsCharm);
          source2 = env.getTargetForce(beDefenseUnit.originalUnit, beDefenseUnit.IsCharm);
          units1 = (IEnumerable<BL.Unit>) env.forceUnits(forceId1).value;
          units2 = ((IEnumerable<BL.ForceID>) source1).SelectMany<BL.ForceID, BL.Unit>((Func<BL.ForceID, IEnumerable<BL.Unit>>) (x => (IEnumerable<BL.Unit>) env.forceUnits(x).value));
          if (forceId1 == forceId2)
          {
            units3 = units1;
            units4 = units2;
          }
          else
          {
            units3 = (IEnumerable<BL.Unit>) env.forceUnits(forceId2).value;
            units4 = ((IEnumerable<BL.ForceID>) source2).SelectMany<BL.ForceID, BL.Unit>((Func<BL.ForceID, IEnumerable<BL.Unit>>) (x => (IEnumerable<BL.Unit>) env.forceUnits(x).value));
          }
        }
        else
        {
          source1 = (BL.ForceID[]) null;
          source2 = (BL.ForceID[]) null;
          units1 = (IEnumerable<BL.Unit>) null;
          units2 = (IEnumerable<BL.Unit>) null;
          units3 = (IEnumerable<BL.Unit>) null;
          units4 = (IEnumerable<BL.Unit>) null;
        }
        Judgement.BeforeDuelUnitParameter.FromBeUnitWork work = new Judgement.BeforeDuelUnitParameter.FromBeUnitWork(beAttackUnit, beDefenseUnit);
        Judgement.BeforeDuelUnitParameter.FromBeUnitData data = new Judgement.BeforeDuelUnitParameter.FromBeUnitData()
        {
          move_distance = 0,
          move_range = -1,
          attackHp = 0,
          defenseHp = 0,
          targetForceId = source1,
          panel = attackPanel,
          targetPanel = (BL.Panel) null,
          raidMissionUnit = (BL.ISkillEffectListUnit) null,
          deckUnits = units1,
          targetDeckUnits = units2,
          isHeal = false,
          isAI = flag,
          isMagic = new bool?(),
          disableUseCountSkillEffect = disableUseCountSkillEffect,
          targetPanelNonNull = defensePanel
        };
        Judgement.BeforeDuelUnitParameter duelUnitParameter1 = Judgement.BeforeDuelUnitParameter.FromBeUnit(beAttackUnit, beDefenseUnit, landform1, new BL.Unit[0], beAttackMagicBullet, 0, distance, work, data);
        work.Swap();
        data.move_distance = 0;
        data.attackHp = 0;
        data.defenseHp = 0;
        data.targetForceId = source2;
        data.panel = defensePanel;
        data.targetPanel = (BL.Panel) null;
        data.deckUnits = units3;
        data.targetDeckUnits = units4;
        data.isMagic = new bool?();
        data.targetPanelNonNull = attackPanel;
        Judgement.BeforeDuelUnitParameter duelUnitParameter2 = Judgement.BeforeDuelUnitParameter.FromBeUnit(beDefenseUnit, beAttackUnit, landform2, new BL.Unit[0], (BL.MagicBullet) null, 0, distance, work, data);
        Judgement.BeforeDuelParameter duelSkill = new Judgement.BeforeDuelParameter();
        duelSkill.attackerUnitParameter = duelUnitParameter1;
        duelSkill.defenderUnitParameter = duelUnitParameter2;
        duelSkill.FixDamage = Judgement.BeforeDuelParameter.MakeFixDamage(beAttackUnit, beAttackMagicBullet, defenseHp);
        Tuple<int?, int?, int?> tuple1 = Judgement.BeforeDuelParameter.MakeFixHit(beAttackUnit, beDefenseUnit, beAttackMagicBullet);
        duelSkill.HitMin = tuple1.Item1;
        duelSkill.HitMax = tuple1.Item2;
        duelSkill.FixHit = tuple1.Item3;
        Tuple<int?, int?> tuple2 = Judgement.BeforeDuelParameter.MakeFixCritical(beAttackUnit, beDefenseUnit);
        duelSkill.CriticalMin = tuple2.Item1;
        duelSkill.CriticalMax = tuple2.Item2;
        duelSkill.DamageRate = Judgement.BeforeDuelParameter.MakeDamageRate(beAttackUnit, beDefenseUnit, false, new int?(), defenseHp: new int?(defenseHp));
        duelSkill.BaseDamage = Judgement.BeforeDuelParameter.MakeBaseDamage(beAttackUnit, beDefenseUnit);
        return duelSkill;
      }

      public static Judgement.BeforeDuelParameter CreateColosseumSingle(
        BL.Unit beAttackUnit,
        BL.MagicBullet beAttackMagicBullet,
        BL.Unit[] beAttackNeighborUnits,
        BL.Unit[] beAttackDeckUnits,
        PlayerItem beAttackEquippedGear,
        PlayerItem beAttackEquippedGear2,
        PlayerItem beAttackEquippedGear3,
        PlayerItem beAttackEquippedReisou,
        PlayerItem beAttackEquippedReisou2,
        PlayerItem beAttackEquippedReisou3,
        BL.Unit beDefenseUnit,
        BL.MagicBullet beDefenseMagicBullet,
        BL.Unit[] beDefenseNeighborUnits,
        BL.Unit[] beDefenseDeckUnits,
        PlayerItem beDefenseEquippedGear,
        PlayerItem beDefenseEquippedGear2,
        PlayerItem beDefenseEquippedGear3,
        PlayerItem beDefenseEquippedReisou,
        PlayerItem beDefenseEquippedReisou2,
        PlayerItem beDefenseEquippedReisou3,
        bool isAttack,
        bool isSample,
        int attackHp,
        int defenseHp,
        int battleCount,
        bool? isMagic,
        BL.Weapon weapon,
        int colosseumTurn)
      {
        Judgement.BeforeDuelUnitParameter.FromBeUnitWork work = new Judgement.BeforeDuelUnitParameter.FromBeUnitWork((BL.ISkillEffectListUnit) beAttackUnit, (BL.ISkillEffectListUnit) beDefenseUnit);
        Judgement.BeforeDuelUnitParameter attack = Judgement.BeforeDuelUnitParameter.FromBeColosseumUnit(beAttackUnit, beDefenseUnit, beAttackNeighborUnits, beAttackMagicBullet, beAttackEquippedGear, beAttackEquippedGear2, beAttackEquippedGear3, beAttackEquippedReisou, beAttackEquippedReisou2, beAttackEquippedReisou3, isSample ? 0 : (isAttack ? 1 : 2), beAttackDeckUnits, beDefenseDeckUnits, attackHp, defenseHp, battleCount, isMagic, work, colosseumTurn);
        work.Swap();
        Judgement.BeforeDuelUnitParameter defense = Judgement.BeforeDuelUnitParameter.FromBeColosseumUnit(beDefenseUnit, beAttackUnit, beDefenseNeighborUnits, beDefenseMagicBullet, beDefenseEquippedGear, beDefenseEquippedGear2, beDefenseEquippedGear3, beDefenseEquippedReisou, beDefenseEquippedReisou2, beDefenseEquippedReisou3, isSample ? 0 : (isAttack ? 2 : 1), beDefenseDeckUnits, beAttackDeckUnits, defenseHp, attackHp, battleCount, new bool?(), work, colosseumTurn);
        Judgement.BeforeDuelParameter colosseumSingle = new Judgement.BeforeDuelParameter()
        {
          attackerUnitParameter = attack,
          defenderUnitParameter = defense
        };
        colosseumSingle.AttackCount = BattleFuncs.attackCount((BL.ISkillEffectListUnit) beAttackUnit, (BL.ISkillEffectListUnit) beDefenseUnit);
        colosseumSingle.AttackCount *= beAttackUnit.originalUnit.playerUnit.normalAttackCount;
        if (BattleFuncs.canOneMore(attack, defense, (BL.ISkillEffectListUnit) beAttackUnit, (BL.ISkillEffectListUnit) beDefenseUnit, isAttack, colosseumIsSample: isSample))
          colosseumSingle.AttackCount *= 2;
        colosseumSingle.FixDamage = Judgement.BeforeDuelParameter.MakeFixDamage((BL.ISkillEffectListUnit) beAttackUnit, beAttackMagicBullet, defenseHp);
        Tuple<int?, int?, int?> tuple1 = Judgement.BeforeDuelParameter.MakeFixHit((BL.ISkillEffectListUnit) beAttackUnit, (BL.ISkillEffectListUnit) beDefenseUnit, beAttackMagicBullet, isMagic: isMagic);
        colosseumSingle.HitMin = tuple1.Item1;
        colosseumSingle.HitMax = tuple1.Item2;
        colosseumSingle.FixHit = tuple1.Item3;
        Tuple<int?, int?> tuple2 = Judgement.BeforeDuelParameter.MakeFixCritical((BL.ISkillEffectListUnit) beAttackUnit, (BL.ISkillEffectListUnit) beDefenseUnit, isMagic: isMagic);
        colosseumSingle.CriticalMin = tuple2.Item1;
        colosseumSingle.CriticalMax = tuple2.Item2;
        colosseumSingle.DamageRate = Judgement.BeforeDuelParameter.MakeDamageRate((BL.ISkillEffectListUnit) beAttackUnit, (BL.ISkillEffectListUnit) beDefenseUnit, false, new int?(colosseumTurn), isMagic: isMagic, weapon: weapon, isAttack: isSample ? new bool?() : new bool?(isAttack), attackHp: new int?(attackHp), defenseHp: new int?(defenseHp));
        colosseumSingle.BaseDamage = Judgement.BeforeDuelParameter.MakeBaseDamage((BL.ISkillEffectListUnit) beAttackUnit, (BL.ISkillEffectListUnit) beDefenseUnit);
        return colosseumSingle;
      }

      private static int? MakeFixDamage(
        BL.ISkillEffectListUnit beAttackUnit,
        BL.MagicBullet beAttackMagicBullet,
        int defenderHp)
      {
        if (beAttackMagicBullet != null)
        {
          BattleskillEffect percentageDamage = beAttackMagicBullet.percentageDamage;
          if (percentageDamage != null)
          {
            int maxDamage = percentageDamage.HasKey(BattleskillEffectLogicArgumentEnum.max_value) ? percentageDamage.GetInt(BattleskillEffectLogicArgumentEnum.max_value) : 0;
            return new int?(BattleFuncs.calcPercentageDamage(defenderHp, percentageDamage.GetFloat(BattleskillEffectLogicArgumentEnum.percentage), maxDamage));
          }
        }
        return beAttackUnit.originalUnit.weapon.gear.fix_damage;
      }

      private static Tuple<int?, int?, int?> MakeFixHit(
        BL.ISkillEffectListUnit beAttackUnit,
        BL.ISkillEffectListUnit beDefenseUnit,
        BL.MagicBullet beAttackMagicBullet,
        BL.Panel attackPanel = null,
        int distance = 0,
        bool isAI = false,
        bool? isMagic = null,
        BL.Panel defensePanel = null)
      {
        float? nullable1 = new float?();
        if (beAttackMagicBullet != null)
        {
          BattleskillEffect percentageDamage = beAttackMagicBullet.percentageDamage;
          if (percentageDamage != null)
          {
            float num = percentageDamage.GetFloat(BattleskillEffectLogicArgumentEnum.hit_value);
            if ((double) num > 0.0)
              nullable1 = new float?(num);
          }
        }
        List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
        Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int> action = (Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int>) ((effectUnit, targetUnit, effect_target) =>
        {
          if (BattleFuncs.isSkillsAndEffectsInvalid(effectUnit, targetUnit))
            return;
          foreach (BL.SkillEffect effect in effectUnit.enabledSkillEffect(BattleskillEffectLogicEnum.clamp_hit).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
          {
            BattleskillEffect effect = x.effect;
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(x);
            if (effect_target == 0 && packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.attack_type))
            {
              int num = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_type);
              if (num == 1 && (!isMagic.HasValue || isMagic.Value) || num == 2 && (!isMagic.HasValue || !isMagic.Value))
                return false;
            }
            BL.Panel panel = (BL.Panel) null;
            if (effect_target == 0 && attackPanel != null)
              panel = attackPanel;
            else if (effect_target == 1 && defensePanel != null)
              panel = defensePanel;
            return (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) && effect_target == 0 || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) && (double) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.effect_target) == (double) effect_target) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == effectUnit.originalUnit.unit.kind.ID) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == targetUnit.originalUnit.unit.kind.ID) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == effectUnit.originalUnit.playerUnit.GetElement()) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == targetUnit.originalUnit.playerUnit.GetElement()) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.job_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == effectUnit.originalUnit.job.ID) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == targetUnit.originalUnit.job.ID) && packedSkillEffect.CheckLandTag(panel, isAI);
          })))
            skillParams.Add(BattleFuncs.SkillParam.Create(effectUnit.originalUnit, effect));
        });
        action(beAttackUnit, beDefenseUnit, 0);
        action(beDefenseUnit, beAttackUnit, 1);
        if (attackPanel != null)
        {
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaPanelBuffDebuff(BattleskillEffectLogicEnum.charisma_clamp_hit, beAttackUnit, beDefenseUnit, attackPanel, isAI))
            skillParams.Add(BattleFuncs.SkillParam.Create(effect.parentUnit, effect));
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaBuffDebuff(beAttackUnit.skillEffects, BattleskillEffectLogicEnum.charisma_clamp_hit, beAttackUnit, 0, beAttackUnit, beDefenseUnit, 0, attackPanel, attackPanel, isAI))
            skillParams.Add(BattleFuncs.SkillParam.Create(beAttackUnit.originalUnit, effect));
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaBuffDebuff(beDefenseUnit.skillEffects, BattleskillEffectLogicEnum.charisma_clamp_hit, beDefenseUnit, 1, beAttackUnit, beDefenseUnit, distance, attackPanel, defensePanel, isAI))
            skillParams.Add(BattleFuncs.SkillParam.Create(beDefenseUnit.originalUnit, effect));
        }
        Decimal? nullable2 = new Decimal?();
        Decimal? nullable3 = new Decimal?();
        foreach (BattleFuncs.SkillParam skillParam in BattleFuncs.gearSkillParamFilter(skillParams))
        {
          BL.SkillEffect effect = skillParam.effect;
          Decimal num1 = (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_skill_ratio);
          if (nullable2.HasValue)
          {
            Decimal num2 = num1;
            Decimal? nullable4 = nullable2;
            Decimal valueOrDefault = nullable4.GetValueOrDefault();
            if (!(num2 > valueOrDefault & nullable4.HasValue))
              goto label_31;
          }
          nullable2 = new Decimal?(num1);
label_31:
          Decimal num3 = (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_skill_ratio);
          if (nullable3.HasValue)
          {
            Decimal num4 = num3;
            Decimal? nullable5 = nullable3;
            Decimal valueOrDefault = nullable5.GetValueOrDefault();
            if (!(num4 < valueOrDefault & nullable5.HasValue))
              continue;
          }
          nullable3 = new Decimal?(num3);
        }
        int? nullable6;
        if (!nullable2.HasValue)
        {
          nullable6 = new int?();
        }
        else
        {
          Decimal? nullable7 = nullable2;
          Decimal num = (Decimal) 100;
          nullable6 = nullable7.HasValue ? new int?((int) (nullable7.GetValueOrDefault() * num)) : new int?();
        }
        int? nullable8;
        if (!nullable3.HasValue)
        {
          nullable8 = new int?();
        }
        else
        {
          Decimal? nullable9 = nullable3;
          Decimal num = (Decimal) 100;
          nullable8 = nullable9.HasValue ? new int?((int) (nullable9.GetValueOrDefault() * num)) : new int?();
        }
        int? nullable10 = nullable8;
        int? nullable11;
        if (!nullable1.HasValue)
        {
          nullable11 = new int?();
        }
        else
        {
          float? nullable12 = nullable1;
          float num = 100f;
          nullable11 = nullable12.HasValue ? new int?((int) ((double) nullable12.GetValueOrDefault() * (double) num)) : new int?();
        }
        int? nullable13 = nullable11;
        int? nullable14 = nullable10;
        int? nullable15 = nullable13;
        return new Tuple<int?, int?, int?>(nullable6, nullable14, nullable15);
      }

      private static Tuple<int?, int?> MakeFixCritical(
        BL.ISkillEffectListUnit beAttackUnit,
        BL.ISkillEffectListUnit beDefenseUnit,
        BL.Panel attackPanel = null,
        bool? isMagic = null,
        BL.Panel defensePanel = null,
        bool isAI = false)
      {
        List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
        Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int> action = (Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int>) ((effectUnit, targetUnit, effect_target) =>
        {
          if (BattleFuncs.isSkillsAndEffectsInvalid(effectUnit, targetUnit))
            return;
          foreach (BL.SkillEffect effect in effectUnit.enabledSkillEffect(BattleskillEffectLogicEnum.clamp_critical).Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
          {
            BattleskillEffect effect = x.effect;
            BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(x);
            if (effect_target == 0 && packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.attack_type))
            {
              int num = packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_type);
              if (num == 1 && (!isMagic.HasValue || isMagic.Value) || num == 2 && (!isMagic.HasValue || !isMagic.Value))
                return false;
            }
            BL.Panel panel = (BL.Panel) null;
            if (effect_target == 0 && attackPanel != null)
              panel = attackPanel;
            else if (effect_target == 1 && defensePanel != null)
              panel = defensePanel;
            return (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) && effect_target == 0 || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.effect_target) && (double) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.effect_target) == (double) effect_target) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.gear_kind_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == effectUnit.originalUnit.unit.kind.ID) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == targetUnit.originalUnit.unit.kind.ID) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.element) == effectUnit.originalUnit.playerUnit.GetElement()) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_element) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == targetUnit.originalUnit.playerUnit.GetElement()) && (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.target_job_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == targetUnit.originalUnit.job.ID) && packedSkillEffect.CheckLandTag(panel, isAI);
          })))
            skillParams.Add(BattleFuncs.SkillParam.Create(effectUnit.originalUnit, effect));
        });
        action(beAttackUnit, beDefenseUnit, 0);
        action(beDefenseUnit, beAttackUnit, 1);
        Decimal? nullable1 = new Decimal?();
        Decimal? nullable2 = new Decimal?();
        foreach (BattleFuncs.SkillParam skillParam in BattleFuncs.gearSkillParamFilter(skillParams))
        {
          BL.SkillEffect effect = skillParam.effect;
          Decimal num1 = (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.min_skill_ratio);
          Decimal? nullable3;
          if (nullable1.HasValue)
          {
            Decimal num2 = num1;
            nullable3 = nullable1;
            Decimal valueOrDefault = nullable3.GetValueOrDefault();
            if (!(num2 > valueOrDefault & nullable3.HasValue))
              goto label_5;
          }
          nullable1 = new Decimal?(num1);
label_5:
          Decimal num3 = (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.max_skill_ratio);
          if (nullable2.HasValue)
          {
            Decimal num4 = num3;
            nullable3 = nullable2;
            Decimal valueOrDefault = nullable3.GetValueOrDefault();
            if (!(num4 < valueOrDefault & nullable3.HasValue))
              continue;
          }
          nullable2 = new Decimal?(num3);
        }
        Decimal? nullable4;
        int? nullable5;
        if (!nullable1.HasValue)
        {
          nullable5 = new int?();
        }
        else
        {
          nullable4 = nullable1;
          Decimal num = (Decimal) 100;
          nullable5 = nullable4.HasValue ? new int?((int) (nullable4.GetValueOrDefault() * num)) : new int?();
        }
        int? nullable6;
        if (!nullable2.HasValue)
        {
          nullable6 = new int?();
        }
        else
        {
          nullable4 = nullable2;
          Decimal num = (Decimal) 100;
          nullable6 = nullable4.HasValue ? new int?((int) (nullable4.GetValueOrDefault() * num)) : new int?();
        }
        int? nullable7 = nullable6;
        return new Tuple<int?, int?>(nullable5, nullable7);
      }

      private static int MakeBaseDamage(
        BL.ISkillEffectListUnit beAttackUnit,
        BL.ISkillEffectListUnit beDefenseUnit,
        BL.Panel attackPanel = null,
        bool isAI = false)
      {
        int baseDamage = 0;
        ((Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int>) ((effectUnit, targetUnit, effect_target) =>
        {
          foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(effectUnit.originalUnit, effectUnit.skillEffects.Where(BattleskillEffectLogicEnum.base_damage, (Func<BL.SkillEffect, bool>) (x =>
          {
            BattleskillEffect effect = x.effect;
            return !BattleFuncs.isSealedSkillEffect(effectUnit, x) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.gear_kind_id) == effectUnit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_gear_kind_id) == targetUnit.originalUnit.unit.kind.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.element) == effectUnit.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == 0 || (CommonElement) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_element) == targetUnit.originalUnit.playerUnit.GetElement()) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.job_id) == effectUnit.originalUnit.job.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == 0 || effect.GetInt(BattleskillEffectLogicArgumentEnum.target_job_id) == targetUnit.originalUnit.job.ID) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id) == 0 || effectUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.family_id))) && (effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id) == 0 || targetUnit.originalUnit.playerUnit.HasFamily((UnitFamily) effect.GetInt(BattleskillEffectLogicArgumentEnum.target_family_id))) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, beAttackUnit, beDefenseUnit) && !BattleFuncs.isSkillsAndEffectsInvalid(beAttackUnit, beDefenseUnit) && effect.GetPackedSkillEffect().CheckLandTag(attackPanel, isAI);
          }))))
            baseDamage += skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.damage_value) + skillEffect.baseSkillLevel * skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_ratio);
        }))(beAttackUnit, beDefenseUnit, 0);
        return baseDamage;
      }

      private static float MakeDamageRate(
        BL.ISkillEffectListUnit beAttackUnit,
        BL.ISkillEffectListUnit beDefenseUnit,
        bool enableInvalidSkillsAndLogics,
        int? colosseumTurn,
        BL.Panel attackPanel = null,
        int distance = 0,
        bool isAI = false,
        bool? isMagic = null,
        BL.Panel defensePanel = null,
        BL.Weapon weapon = null,
        bool? isAttack = null,
        int? attackHp = null,
        int? defenseHp = null)
      {
        List<BattleFuncs.SkillParam> skillParams = new List<BattleFuncs.SkillParam>();
        List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics = new List<BattleFuncs.InvalidSpecificSkillLogic>();
        if (attackHp.HasValue)
        {
          int? nullable = attackHp;
          int num = 0;
          if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
            attackHp = new int?(beAttackUnit.hp);
        }
        if (defenseHp.HasValue)
        {
          int? nullable = defenseHp;
          int num = 0;
          if (nullable.GetValueOrDefault() <= num & nullable.HasValue)
            defenseHp = new int?(beDefenseUnit.hp);
        }
        if (enableInvalidSkillsAndLogics)
        {
          foreach (BL.SkillEffect skillEffect in BattleFuncs.gearSkillEffectFilter(beAttackUnit.originalUnit, beAttackUnit.skillEffects.Where(BattleskillEffectLogicEnum.invalid_specific_skills_and_logics, (Func<BL.SkillEffect, bool>) (x =>
          {
            BattleskillEffect effect = x.effect;
            BattleFuncs.PackedSkillEffect pse = BattleFuncs.PackedSkillEffect.Create(x);
            return !BattleFuncs.isSealedSkillEffect(beAttackUnit, x) && BattleFuncs.checkInvokeSkillEffect(pse, beAttackUnit, beDefenseUnit) && pse.CheckLandTag(attackPanel, isAI) && !BattleFuncs.isEffectEnemyRangeAndInvalid(x, beAttackUnit, beDefenseUnit) && !BattleFuncs.isSkillsAndEffectsInvalid(beAttackUnit, beDefenseUnit);
          }))))
            invalidSkillLogics.Add(BattleFuncs.InvalidSpecificSkillLogic.Create(skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_skill_id), skillEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.invalid_logic_id), (object) skillEffect));
        }
        Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int> action = (Action<BL.ISkillEffectListUnit, BL.ISkillEffectListUnit, int>) ((effectUnit, targetUnit, effect_target) =>
        {
          IEnumerable<BL.SkillEffect> skillEffects = effectUnit.skillEffects.Where(BattleskillEffectLogicEnum.damage_rate);
          if (effect_target == 0 && weapon != null && isMagic.HasValue)
            skillEffects = skillEffects.Concat<BL.SkillEffect>(BattleFuncs.getAttackMethodExtraSkillEffects(weapon, isMagic.Value, BattleskillEffectLogicEnum.damage_rate));
          foreach (BL.SkillEffect effect in skillEffects.Where<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x =>
          {
            BattleskillEffect effect = x.effect;
            if (effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) != effect_target)
              return false;
            if (effect_target == 0 && effect.HasKey(BattleskillEffectLogicArgumentEnum.attack_type))
            {
              int num = effect.GetInt(BattleskillEffectLogicArgumentEnum.attack_type);
              if (num == 1 && (!isMagic.HasValue || isMagic.Value) || num == 2 && (!isMagic.HasValue || !isMagic.Value))
                return false;
            }
            if (effect_target == 1 && Judgement.BeforeDuelParameter.IsInvalidDamageRateEffect(x, invalidSkillLogics))
              return false;
            BL.Panel unitPanel;
            BL.Panel targetPanel;
            int attackType;
            int? unitHp;
            int? targetHp;
            if (effect_target == 0)
            {
              unitPanel = attackPanel;
              targetPanel = defensePanel;
              attackType = !isAttack.HasValue ? 0 : (isAttack.Value ? 1 : 2);
              unitHp = attackHp;
              targetHp = defenseHp;
            }
            else
            {
              unitPanel = defensePanel;
              targetPanel = attackPanel;
              attackType = !isAttack.HasValue ? 0 : (isAttack.Value ? 2 : 1);
              unitHp = defenseHp;
              targetHp = attackHp;
            }
            BattleFuncs.PackedSkillEffect packedSkillEffect = x.GetPackedSkillEffect();
            if (!x.GetCheckInvokeGeneric().DoCheck(effectUnit, targetUnit, colosseumTurn, unitHp: unitHp, targetHp: targetHp, attackType: attackType, unitPanel: unitPanel, targetPanel: targetPanel, effect: x) || effectUnit == beDefenseUnit && BattleFuncs.isSkillsAndEffectsInvalid(beDefenseUnit, beAttackUnit, x) || BattleFuncs.isEffectEnemyRangeAndInvalid(x, beAttackUnit, beDefenseUnit) || BattleFuncs.isSkillsAndEffectsInvalid(beAttackUnit, beDefenseUnit, x))
              return false;
            return !packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.logic_id) || BattleFuncs.checkSkillLogicInvest(effectUnit, targetUnit, packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.logic_id), packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.ailment_group_id), packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id), packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.skill_type), packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.invest_type), packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.condition_target));
          })))
            skillParams.Add(BattleFuncs.SkillParam.Create(effectUnit.originalUnit, effect));
        });
        action(beAttackUnit, beDefenseUnit, 0);
        action(beDefenseUnit, beAttackUnit, 1);
        if (attackPanel != null)
        {
          Func<BL.SkillEffect, BL.ISkillEffectListUnit, bool> func = (Func<BL.SkillEffect, BL.ISkillEffectListUnit, bool>) ((e, opponentUnit) =>
          {
            BattleFuncs.PackedSkillEffect packedSkillEffect = e.effect.GetPackedSkillEffect();
            if (packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.opponent_gear_kind_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_gear_kind_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_gear_kind_id) != opponentUnit.originalUnit.unit.kind.ID || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.opponent_element) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_element) != 0 && (CommonElement) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_element) != opponentUnit.originalUnit.playerUnit.GetElement() || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.opponent_job_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_job_id) != 0 && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_job_id) != opponentUnit.originalUnit.job.ID || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.opponent_family_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_family_id) != 0 && !opponentUnit.originalUnit.playerUnit.HasFamily((UnitFamily) packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_family_id)) || packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.opponent_skill_group_id) && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_skill_group_id) != 0 && !opponentUnit.originalUnit.unit.HasSkillGroupId(packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_skill_group_id)))
              return false;
            if (!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.opponent_group_large_id) || packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_group_large_id) == 0)
              return true;
            return opponentUnit.originalUnit.unitGroup != null && packedSkillEffect.GetInt(BattleskillEffectLogicArgumentEnum.opponent_group_large_id) == opponentUnit.originalUnit.unitGroup.group_large_category_id.ID;
          });
          int attackType1 = !isAttack.HasValue ? 0 : (isAttack.Value ? 1 : 2);
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaPanelBuffDebuffByCheckInvokeGeneric(BattleskillEffectLogicEnum.charisma_damage_rate, beAttackUnit, beDefenseUnit, attackPanel, isAI, colosseumTurn, attackType1, attackHp))
          {
            if (func(effect, beDefenseUnit) && (effect.effect.GetInt(BattleskillEffectLogicArgumentEnum.effect_target) == 0 || !Judgement.BeforeDuelParameter.IsInvalidDamageRateEffect(effect, invalidSkillLogics)))
              skillParams.Add(BattleFuncs.SkillParam.Create(effect.parentUnit, effect));
          }
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaBuffDebuffByCheckInvokeGeneric(beAttackUnit.skillEffects, BattleskillEffectLogicEnum.charisma_damage_rate, beAttackUnit, 0, beAttackUnit, beDefenseUnit, 0, attackPanel, attackPanel, colosseumTurn, attackType1, attackHp, attackHp))
          {
            if (func(effect, beDefenseUnit))
              skillParams.Add(BattleFuncs.SkillParam.Create(beAttackUnit.originalUnit, effect));
          }
          int attackType2 = !isAttack.HasValue ? 0 : (isAttack.Value ? 2 : 1);
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaBuffDebuffByCheckInvokeGeneric(beDefenseUnit.skillEffects, BattleskillEffectLogicEnum.charisma_damage_rate, beDefenseUnit, 1, beAttackUnit, beDefenseUnit, distance, attackPanel, defensePanel, colosseumTurn, attackType2, defenseHp, attackHp))
          {
            if (func(effect, beDefenseUnit) && !Judgement.BeforeDuelParameter.IsInvalidDamageRateEffect(effect, invalidSkillLogics))
              skillParams.Add(BattleFuncs.SkillParam.Create(beDefenseUnit.originalUnit, effect));
          }
          int attackType3 = !isAttack.HasValue ? 0 : (isAttack.Value ? 2 : 1);
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaPanelBuffDebuffByCheckInvokeGeneric(BattleskillEffectLogicEnum.charisma_enemy_damage_rate, beDefenseUnit, beAttackUnit, defensePanel, isAI, colosseumTurn, attackType3, defenseHp))
          {
            if (func(effect, beAttackUnit) && !Judgement.BeforeDuelParameter.IsInvalidDamageRateEffect(effect, invalidSkillLogics))
              skillParams.Add(BattleFuncs.SkillParam.Create(effect.parentUnit, effect));
          }
          int attackType4 = !isAttack.HasValue ? 0 : (isAttack.Value ? 1 : 2);
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaBuffDebuffByCheckInvokeGeneric(beAttackUnit.skillEffects, BattleskillEffectLogicEnum.charisma_enemy_damage_rate, beAttackUnit, 1, beDefenseUnit, beAttackUnit, distance, defensePanel, attackPanel, colosseumTurn, attackType4, attackHp, defenseHp))
          {
            if (func(effect, beAttackUnit) && !Judgement.BeforeDuelParameter.IsInvalidDamageRateEffect(effect, invalidSkillLogics))
              skillParams.Add(BattleFuncs.SkillParam.Create(beAttackUnit.originalUnit, effect));
          }
          int attackType5 = !isAttack.HasValue ? 0 : (isAttack.Value ? 2 : 1);
          foreach (BL.SkillEffect effect in Judgement.GetEnabledCharismaBuffDebuffByCheckInvokeGeneric(beDefenseUnit.skillEffects, BattleskillEffectLogicEnum.charisma_enemy_damage_rate, beDefenseUnit, 0, beDefenseUnit, beAttackUnit, 0, defensePanel, defensePanel, colosseumTurn, attackType5, defenseHp, defenseHp))
          {
            if (func(effect, beAttackUnit) && !Judgement.BeforeDuelParameter.IsInvalidDamageRateEffect(effect, invalidSkillLogics))
              skillParams.Add(BattleFuncs.SkillParam.Create(beDefenseUnit.originalUnit, effect));
          }
        }
        Decimal num1 = 1.0M;
        foreach (IGrouping<int, BattleFuncs.SkillParam> grouping in BattleFuncs.gearSkillParamFilter(skillParams).GroupBy<BattleFuncs.SkillParam, int>((Func<BattleFuncs.SkillParam, int>) (x => x.effect.effectId)))
        {
          Decimal num2 = 1.0M;
          bool flag = true;
          Decimal? nullable1 = new Decimal?();
          foreach (BattleFuncs.SkillParam skillParam in (IEnumerable<BattleFuncs.SkillParam>) grouping)
          {
            BL.SkillEffect effect = skillParam.effect;
            num2 *= (Decimal) Judgement.BeforeDuelParameter.CalcDamageRateEffect(effect);
            if (flag)
            {
              flag = false;
            }
            else
            {
              if (!nullable1.HasValue)
              {
                BattleFuncs.PackedSkillEffect packedSkillEffect = BattleFuncs.PackedSkillEffect.Create(effect);
                nullable1 = new Decimal?(!packedSkillEffect.HasKey(BattleskillEffectLogicArgumentEnum.max_percentage) ? Decimal.MaxValue : (Decimal) packedSkillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.max_percentage));
              }
              Decimal num3 = num2;
              Decimal? nullable2 = nullable1;
              Decimal valueOrDefault = nullable2.GetValueOrDefault();
              if (num3 > valueOrDefault & nullable2.HasValue)
              {
                num2 = nullable1.Value;
                break;
              }
            }
          }
          num1 *= num2;
        }
        return (float) num1;
      }

      private static float CalcDamageRateEffect(BL.SkillEffect effect)
      {
        return (float) ((Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_percentage) + (Decimal) effect.baseSkillLevel * (Decimal) effect.effect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio));
      }

      private static bool IsInvalidDamageRateEffect(
        BL.SkillEffect effect,
        List<BattleFuncs.InvalidSpecificSkillLogic> invalidSkillLogics)
      {
        float rate = Judgement.BeforeDuelParameter.CalcDamageRateEffect(effect);
        Func<BattleFuncs.InvalidSpecificSkillLogic, bool> funcExtraCheck = (Func<BattleFuncs.InvalidSpecificSkillLogic, bool>) (issl =>
        {
          int num = ((BL.SkillEffect) issl.param).effect.GetInt(BattleskillEffectLogicArgumentEnum.condition);
          if (num == 0 || num == 1 && (double) rate > 1.0)
            return true;
          return num == 2 && (double) rate < 1.0;
        });
        return BattleFuncs.checkInvalidEffect(effect, invalidSkillLogics, funcExtraCheck);
      }

      public static Tuple<BL.Skill, float, bool> SimulateDS(
        BL.ISkillEffectListUnit attack,
        AttackStatus attackAS,
        BL.Panel attackPanel,
        BL.ISkillEffectListUnit defense,
        AttackStatus defenseAS,
        BL.Panel defensePanel,
        int distance,
        bool isAttacker)
      {
        TurnHp hp = new TurnHp()
        {
          attackerHp = attack.hp,
          defenderHp = defense.hp,
          attackerIsDontAction = attack.IsDontAction,
          defenderIsDontAction = defense.IsDontAction,
          attackerIsDontEvasion = attack.IsDontEvasion,
          defenderIsDontEvasion = defense.IsDontEvasion,
          attackerIsDontUseSkill = attack.IsDontAction,
          defenderIsDontUseSkill = defense.IsDontAction,
          attackerCantOneMore = false,
          defenderCantOneMore = false,
          otherHp = new Dictionary<BL.ISkillEffectListUnit, TurnOtherHp>()
        };
        hp.attackerIsDontUseSkill |= BattleFuncs.isSkillsAndEffectsInvalid(attack, defense);
        hp.defenderIsDontUseSkill |= BattleFuncs.isSkillsAndEffectsInvalid(defense, attack);
        if (isAttacker)
          return Judgement.BeforeDuelParameter.SimulateDS(attack, attackAS, attackPanel, defense, defenseAS, defensePanel, distance, isAttacker, ref hp);
        return defenseAS != null ? Judgement.BeforeDuelParameter.SimulateDS(defense, defenseAS, defensePanel, attack, attackAS, attackPanel, distance, isAttacker, ref hp) : new Tuple<BL.Skill, float, bool>((BL.Skill) null, 0.0f, false);
      }

      private static Tuple<BL.Skill, float, bool> SimulateDS(
        BL.ISkillEffectListUnit attack,
        AttackStatus attackStatus,
        BL.Panel attackPanel,
        BL.ISkillEffectListUnit defense,
        AttackStatus defenseStatus,
        BL.Panel defensePanel,
        int distance,
        bool isAttacker,
        ref TurnHp hp)
      {
        BL.MagicBullet magicBullet = attackStatus.magicBullet;
        int cost = magicBullet != null ? magicBullet.cost : 0;
        if (cost > 0 && (isAttacker ? hp.attackerHp : hp.defenderHp) <= cost)
          return new Tuple<BL.Skill, float, bool>((BL.Skill) null, 0.0f, false);
        if (isAttacker)
          hp.attackerHp -= cost;
        else
          hp.defenderHp -= cost;
        BattleDuelSkill battleDuelSkill = BattleDuelSkill.simulateBiAttackSkills(attack, attackStatus, attackPanel, defense, defenseStatus, defensePanel, distance, isAttacker ? hp.attackerHp : hp.defenderHp, isAttacker ? hp.defenderHp : hp.attackerHp, isAttacker ? hp.attackerIsDontUseSkill : hp.defenderIsDontUseSkill, isAttacker ? hp.defenderIsDontUseSkill : hp.attackerIsDontUseSkill, false, new int?(), isAttacker, false, new float?(), false, false, hp);
        if (battleDuelSkill.attackerSkills.Length < 1)
          return new Tuple<BL.Skill, float, bool>((BL.Skill) null, 0.0f, false);
        Decimal num1 = 1M;
        bool flag1 = false;
        foreach (BattleFuncs.InvokeLotteryInfo lotteryInfo in battleDuelSkill.lotteryInfos)
        {
          Decimal num2 = 1M - Math.Min(Math.Max((Decimal) lotteryInfo.Final * 0.01M, 0M), 1M);
          num1 *= num2;
          flag1 = ((flag1 ? 1 : 0) | (!lotteryInfo.Max.HasValue ? 0 : ((double) lotteryInfo.Max.Value <= 0.0 ? 1 : 0))) != 0;
        }
        Decimal num3 = (1M - num1) * 100M;
        bool flag2 = num3 <= 0M & flag1;
        return new Tuple<BL.Skill, float, bool>(battleDuelSkill.attackerSkills[0], (float) num3, flag2);
      }
    }
  }
}
