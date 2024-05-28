// Decompiled with JetBrains decompiler
// Type: GameCore.RecoveryUnit
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
  public class RecoveryUnit
  {
    public int unitPositionId;
    public int row;
    public int column;
    public bool completed;
    public int hp;
    public int respawnCount;
    public RecoverySkill[] skillList;
    public RecoverySkillEffect[] skillEffectList;
    public int[] deadTurn;
    public int lastDeadTurn;
    public RecoverySkillEffectParam[] skillFixEffectParams;
    public RecoverySkillEffectParam[] skillRatioEffectParams;
    public RecoverySkillEffect[] removedBaseSkillEffects;
    public RecoverySkillEffectParam[] removedFixEffectParams;
    public RecoverySkillEffectParam[] removedRatioEffectParams;
    public int[][] duelSkillEffectIdInvokeCount;
    public int[][] duelSkillIdInvokeCount;
    public int[][] duelSkillIdInvokeCount2;
    public RecoverySkillEffect[] removedOverwriteSkillEffects;
    public bool isExecCompletedSkillEffect;
    public int deadCount;
    public int deadCountExceptImmediateRebirth;
    public RecoverySkillEffect[] waitingTransformationSkillEffects;
    public int facilitySpawnOrder;
    public bool isSpawned;

    public RecoveryUnit(BL.UnitPosition up, BL env)
    {
      this.unitPositionId = up.id;
      this.row = up.row;
      this.column = up.column;
      this.completed = up.isCompleted;
      this.hp = up.unit.hp;
      this.respawnCount = up.unit.pvpRespawnCount;
      this.skillList = ((IEnumerable<BL.Skill>) up.unit.skills).Select<BL.Skill, RecoverySkill>((Func<BL.Skill, RecoverySkill>) (se => new RecoverySkill(se))).ToArray<RecoverySkill>();
      if (up.unit.hasOugi)
      {
        Array.Resize<RecoverySkill>(ref this.skillList, this.skillList.Length + 1);
        this.skillList[this.skillList.Length - 1] = new RecoverySkill(up.unit.ougi);
      }
      if (up.unit.hasSEASkill)
      {
        Array.Resize<RecoverySkill>(ref this.skillList, this.skillList.Length + 1);
        this.skillList[this.skillList.Length - 1] = new RecoverySkill(up.unit.SEASkill);
      }
      this.skillEffectList = up.unit.skillEffects.All().Select<BL.SkillEffect, RecoverySkillEffect>((Func<BL.SkillEffect, RecoverySkillEffect>) (se => new RecoverySkillEffect(se, env))).ToArray<RecoverySkillEffect>();
      this.deadTurn = up.unit.deadTurn.ToArray();
      this.lastDeadTurn = up.unit.lastDeadTurn;
      this.skillFixEffectParams = up.unit.skillEffects.GetAllFixEffectParams().Select<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, RecoverySkillEffectParam>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, RecoverySkillEffectParam>) (sep =>
      {
        BL.SkillEffect se = sep.Item2;
        return new RecoverySkillEffectParam()
        {
          skillEffect = new RecoverySkillEffect(se, env),
          value = (float) sep.Item3
        };
      })).ToArray<RecoverySkillEffectParam>();
      this.skillRatioEffectParams = up.unit.skillEffects.GetAllRatioEffectParams().Select<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, RecoverySkillEffectParam>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, RecoverySkillEffectParam>) (sep =>
      {
        BL.SkillEffect se = sep.Item2;
        return new RecoverySkillEffectParam()
        {
          skillEffect = new RecoverySkillEffect(se, env),
          value = sep.Item3
        };
      })).ToArray<RecoverySkillEffectParam>();
      this.removedBaseSkillEffects = up.unit.skillEffects.GetAllRemovedBaseSkillEffects().Select<BL.SkillEffect, RecoverySkillEffect>((Func<BL.SkillEffect, RecoverySkillEffect>) (se => new RecoverySkillEffect(se, env))).ToArray<RecoverySkillEffect>();
      this.removedFixEffectParams = up.unit.skillEffects.GetAllRemovedFixEffectParams().Select<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, RecoverySkillEffectParam>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, int>, RecoverySkillEffectParam>) (sep =>
      {
        BL.SkillEffect se = sep.Item2;
        return new RecoverySkillEffectParam()
        {
          skillEffect = new RecoverySkillEffect(se, env),
          value = (float) sep.Item3
        };
      })).ToArray<RecoverySkillEffectParam>();
      this.removedRatioEffectParams = up.unit.skillEffects.GetAllRemovedRatioEffectParams().Select<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, RecoverySkillEffectParam>((Func<Tuple<BattleskillEffectLogicEnum, BL.SkillEffect, float>, RecoverySkillEffectParam>) (sep =>
      {
        BL.SkillEffect se = sep.Item2;
        return new RecoverySkillEffectParam()
        {
          skillEffect = new RecoverySkillEffect(se, env),
          value = sep.Item3
        };
      })).ToArray<RecoverySkillEffectParam>();
      this.duelSkillEffectIdInvokeCount = up.unit.skillEffects.GetAllDuelSkillEffectIdInvokeCount().Select<KeyValuePair<int, int>, int[]>((Func<KeyValuePair<int, int>, int[]>) (kvp => new int[2]
      {
        kvp.Key,
        kvp.Value
      })).ToArray<int[]>();
      this.duelSkillIdInvokeCount = up.unit.skillEffects.GetAllDuelSkillIdInvokeCount().Select<KeyValuePair<int, int>, int[]>((Func<KeyValuePair<int, int>, int[]>) (kvp => new int[2]
      {
        kvp.Key,
        kvp.Value
      })).ToArray<int[]>();
      this.duelSkillIdInvokeCount2 = up.unit.skillEffects.GetAllDuelSkillIdInvokeCount2().Select<KeyValuePair<int, int>, int[]>((Func<KeyValuePair<int, int>, int[]>) (kvp => new int[2]
      {
        kvp.Key,
        kvp.Value
      })).ToArray<int[]>();
      this.removedOverwriteSkillEffects = up.unit.skillEffects.GetAllRemovedOverwriteSkillEffects().Select<BL.SkillEffect, RecoverySkillEffect>((Func<BL.SkillEffect, RecoverySkillEffect>) (se => new RecoverySkillEffect(se, env))).ToArray<RecoverySkillEffect>();
      this.isExecCompletedSkillEffect = up.unit.mIsExecCompletedSkillEffect;
      this.deadCount = up.unit.deadCount;
      this.deadCountExceptImmediateRebirth = up.unit.deadCountExceptImmediateRebirth;
      this.waitingTransformationSkillEffects = up.unit.skillEffects.GetAllWaitingTransformationSkillEffects().Select<BL.SkillEffect, RecoverySkillEffect>((Func<BL.SkillEffect, RecoverySkillEffect>) (se => new RecoverySkillEffect(se, env))).ToArray<RecoverySkillEffect>();
      this.facilitySpawnOrder = up.unit.facilitySpawnOrder;
      this.isSpawned = up.unit.isSpawned;
    }
  }
}
