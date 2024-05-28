// Decompiled with JetBrains decompiler
// Type: GameCore.RecoveryUtility
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
  public static class RecoveryUtility
  {
    public static void setMoveDistance(BL.UnitPosition up, int row, int column, BL env)
    {
      int num1 = BL.fieldDistance(up.originalRow, up.originalColumn, up.row, up.column) + BL.fieldDistance(up.row, up.column, row, column);
      BL.Panel fieldPanel1 = env.getFieldPanel(up.originalRow, up.originalColumn);
      BL.Panel fieldPanel2 = env.getFieldPanel(up.row, up.column);
      up.usedMoveCost += env.getRouteCostNonCache(up, fieldPanel2, fieldPanel1, up.movePanels, up.completePanels);
      up.moveDistance += num1;
      foreach (BL.SkillEffect skillEffect1 in BattleFuncs.unitPositionToISkillEffectListUnit(up).skillEffects.All())
      {
        int? moveDistance = skillEffect1.moveDistance;
        if (moveDistance.HasValue)
        {
          BL.SkillEffect skillEffect2 = skillEffect1;
          moveDistance = skillEffect2.moveDistance;
          int num2 = num1;
          skillEffect2.moveDistance = moveDistance.HasValue ? new int?(moveDistance.GetValueOrDefault() + num2) : new int?();
        }
        else
          skillEffect1.moveDistance = new int?(num1);
      }
    }

    public static void resetPosition(
      BL.UnitPosition up,
      int row,
      int column,
      BL env,
      bool noCountReset = false,
      bool countMoveDistance = false)
    {
      if (countMoveDistance)
        RecoveryUtility.setMoveDistance(up, row, column, env);
      up.row = row;
      up.column = column;
      up.resetOriginalPosition(env, noCountReset);
    }

    public static void Apply(RecoveryType rt, BL env)
    {
      List<BL.UnitPosition> unitPositionList1 = new List<BL.UnitPosition>();
      List<BL.UnitPosition> unitPositionList2 = new List<BL.UnitPosition>();
      env.completedActionUnits.value = new List<BL.UnitPosition>();
      env.resetActionList(BL.ForceID.player);
      env.resetActionList(BL.ForceID.neutral);
      env.resetActionList(BL.ForceID.enemy);
      foreach (BL.UnitPosition unitPosition in env.unitPositions.value)
      {
        env.removeZocPanels((BL.ISkillEffectListUnit) unitPosition.unit, unitPosition.originalRow, unitPosition.originalColumn);
        unitPosition.removePanelSkillEffects();
      }
      for (int r = env.getFieldHeight() - 1; r >= 0; r--)
      {
        for (int c = env.getFieldWidth() - 1; c >= 0; c--)
        {
          BL.Panel fieldPanel = env.getFieldPanel(r, c);
          if (fieldPanel.isJumping)
            fieldPanel.isJumping = false;
          fieldPanel.getSkillEffects().value.RemoveAll((Predicate<BL.SkillEffect>) (x => !BattleFuncs.isCharismaEffect(x.effect.EffectLogic.Enum)));
          RecoveryPanel recoveryPanel = ((IEnumerable<RecoveryPanel>) rt.panels).FirstOrDefault<RecoveryPanel>((Func<RecoveryPanel, bool>) (x => x.row == r && x.column == c));
          if (recoveryPanel != null)
          {
            foreach (RecoverySkillEffect skillEffect in recoveryPanel.skillEffectList)
            {
              BL.SkillEffect se = BL.SkillEffect.FromRecovery(skillEffect, env);
              if (!BattleFuncs.isCharismaEffect(se.effect.EffectLogic.Enum))
                fieldPanel.addSkillEffect(se, (BL.ISkillEffectListUnit) se.parentUnit);
            }
          }
        }
      }
      foreach (RecoveryUnit unit in rt.units)
      {
        BL.UnitPosition up = BL.UnitPosition.FromNetwork(new int?(unit.unitPositionId), env);
        up.unit.hp = unit.hp;
        up.unit.setIsDead(unit.hp <= 0, env, dontRemoveSteal: true);
        up.unit.deadTurn = new List<int>((IEnumerable<int>) unit.deadTurn);
        up.unit.lastDeadTurn = unit.lastDeadTurn;
        up.unit.deadCount = unit.deadCount;
        up.unit.deadCountExceptImmediateRebirth = unit.deadCountExceptImmediateRebirth;
        up.unit.pvpRespawnCount = unit.respawnCount;
        up.unit.skillEffects = new BL.SkillEffectList();
        foreach (RecoverySkillEffect skillEffect in unit.skillEffectList)
          up.unit.skillEffects.Add(BL.SkillEffect.FromRecovery(skillEffect, env));
        foreach (RecoverySkillEffectParam skillFixEffectParam in unit.skillFixEffectParams)
        {
          BL.SkillEffect effect = BL.SkillEffect.FromRecovery(skillFixEffectParam.skillEffect, env);
          up.unit.skillEffects.AddFixEffectParam(effect.effect.effect_logic.Enum, effect, (int) skillFixEffectParam.value);
        }
        foreach (RecoverySkillEffectParam ratioEffectParam in unit.skillRatioEffectParams)
        {
          BL.SkillEffect effect = BL.SkillEffect.FromRecovery(ratioEffectParam.skillEffect, env);
          up.unit.skillEffects.AddRatioEffectParam(effect.effect.effect_logic.Enum, effect, ratioEffectParam.value);
        }
        foreach (RecoverySkillEffect removedBaseSkillEffect in unit.removedBaseSkillEffects)
          up.unit.skillEffects.AddRemovedBaseSkillEffect(BL.SkillEffect.FromRecovery(removedBaseSkillEffect, env));
        foreach (RecoverySkillEffectParam removedFixEffectParam in unit.removedFixEffectParams)
        {
          BL.SkillEffect skillEffect = BL.SkillEffect.FromRecovery(removedFixEffectParam.skillEffect, env);
          up.unit.skillEffects.AddRemovedFixEffectParam(new Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>(skillEffect.effect.effect_logic.Enum, skillEffect, (int) removedFixEffectParam.value));
        }
        foreach (RecoverySkillEffectParam ratioEffectParam in unit.removedRatioEffectParams)
        {
          BL.SkillEffect skillEffect = BL.SkillEffect.FromRecovery(ratioEffectParam.skillEffect, env);
          up.unit.skillEffects.AddRemovedRatioEffectParam(new Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>(skillEffect.effect.effect_logic.Enum, skillEffect, ratioEffectParam.value));
        }
        foreach (int[] numArray in unit.duelSkillEffectIdInvokeCount)
          up.unit.skillEffects.SetDuelSkillEffectIdInvokeCount(numArray[0], numArray[1]);
        foreach (int[] numArray in unit.duelSkillIdInvokeCount)
          up.unit.skillEffects.SetDuelSkillIdInvokeCount(numArray[0], numArray[1]);
        foreach (int[] numArray in unit.duelSkillIdInvokeCount2)
          up.unit.skillEffects.SetDuelSkillIdInvokeCount2(numArray[0], numArray[1]);
        foreach (RecoverySkillEffect overwriteSkillEffect in unit.removedOverwriteSkillEffects)
          up.unit.skillEffects.AddRemovedOverwriteSkillEffect(BL.SkillEffect.FromRecovery(overwriteSkillEffect, env));
        up.unit.mIsExecCompletedSkillEffect = unit.isExecCompletedSkillEffect;
        foreach (RecoverySkillEffect transformationSkillEffect in unit.waitingTransformationSkillEffects)
          up.unit.skillEffects.AddWaitingTransformationSkillEffect(BL.SkillEffect.FromRecovery(transformationSkillEffect, env));
        up.unit.facilitySpawnOrder = unit.facilitySpawnOrder;
        if (up.unit.isFacility)
          up.unit.isEnable = up.unit.isSpawned = unit.isSpawned;
        RecoveryUtility.resetPosition(up, unit.row, unit.column, env);
        if (unit.completed)
        {
          if (!up.isCompleted)
            up.recoveryCompleteUnit();
        }
        else if (!up.unit.isFacility)
        {
          if (up.unit.isPlayerControl)
            unitPositionList1.Add(up);
          else
            unitPositionList2.Add(up);
        }
        foreach (RecoverySkill skill1 in unit.skillList)
        {
          BL.Skill skill2 = (BL.Skill) null;
          if (up.unit.hasOugi && up.unit.ougi.id == skill1.id)
            skill2 = up.unit.ougi;
          else if (up.unit.hasSEASkill && up.unit.SEASkill.id == skill1.id)
          {
            skill2 = up.unit.SEASkill;
          }
          else
          {
            foreach (BL.Skill skill3 in up.unit.skills)
            {
              if (skill3.id == skill1.id)
                skill2 = skill3;
            }
          }
          if (skill2 != null)
          {
            skill2.level = skill1.level;
            skill2.remain = skill1.remain;
            skill2.useTurn = skill1.useTurn;
            skill2.nowUseCount = skill1.nowUseCount;
          }
        }
      }
      env.playerActionUnits.value = unitPositionList1;
      env.enemyActionUnits.value = unitPositionList2;
    }
  }
}
