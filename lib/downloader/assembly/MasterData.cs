// Decompiled with JetBrains decompiler
// Type: MasterData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public static class MasterData
{
  public static IEnumerator LoadBattleMapLandform(MasterDataTable.BattleMap map)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.BattleMapLandform>("BattleMapLandform", "BattleMapLandform_part_" + map.ID.ToString(), new Func<MasterDataReader, MasterDataTable.BattleMapLandform>(MasterDataTable.BattleMapLandform.Parse), (Func<MasterDataTable.BattleMapLandform, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static MasterDataTable.BattleskillEffect[] WhereBattleskillEffectBy(MasterDataTable.BattleskillSkill skill)
  {
    return MasterDataCache.Where<int, MasterDataTable.BattleskillEffect, Tuple<int>>("BattleskillEffect", "By", Tuple.Create<int>(skill.ID), (Func<MasterDataTable.BattleskillEffect, Tuple<int>>) (x => Tuple.Create<int>(x.skill.ID)), new Func<MasterDataReader, MasterDataTable.BattleskillEffect>(MasterDataTable.BattleskillEffect.Parse), (Func<MasterDataTable.BattleskillEffect, int>) (x => x.ID));
  }

  public static IEnumerator LoadBattleStageEnemy(MasterDataTable.BattleStage stage)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.BattleStageEnemy>("BattleStageEnemy", "BattleStageEnemy_part_" + stage.ID.ToString(), new Func<MasterDataReader, MasterDataTable.BattleStageEnemy>(MasterDataTable.BattleStageEnemy.Parse), (Func<MasterDataTable.BattleStageEnemy, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator LoadBattleStageUserUnit(MasterDataTable.BattleStage stage)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.BattleStageUserUnit>("BattleStageUserUnit", "BattleStageUserUnit_part_" + stage.ID.ToString(), new Func<MasterDataReader, MasterDataTable.BattleStageUserUnit>(MasterDataTable.BattleStageUserUnit.Parse), (Func<MasterDataTable.BattleStageUserUnit, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator LoadCorpsHowto(MasterDataTable.CorpsSetting setting)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.CorpsHowto>("CorpsHowto", "CorpsHowto_part_" + setting.ID.ToString(), new Func<MasterDataReader, MasterDataTable.CorpsHowto>(MasterDataTable.CorpsHowto.Parse), (Func<MasterDataTable.CorpsHowto, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator LoadDateScriptBase(int ID)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.DateScriptBase>("DateScriptBase", "DateScriptBase_part_" + ID.ToString(), new Func<MasterDataReader, MasterDataTable.DateScriptBase>(MasterDataTable.DateScriptBase.Parse), (Func<MasterDataTable.DateScriptBase, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator LoadDateScriptParts(int unitID)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.DateScriptParts>("DateScriptParts", "DateScriptParts_part_" + unitID.ToString(), new Func<MasterDataReader, MasterDataTable.DateScriptParts>(MasterDataTable.DateScriptParts.Parse), (Func<MasterDataTable.DateScriptParts, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator LoadDateScriptQuestion(int unitID)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.DateScriptQuestion>("DateScriptQuestion", "DateScriptQuestion_part_" + unitID.ToString(), new Func<MasterDataReader, MasterDataTable.DateScriptQuestion>(MasterDataTable.DateScriptQuestion.Parse), (Func<MasterDataTable.DateScriptQuestion, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator LoadExploreEnemy(int floor)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.ExploreEnemy>("ExploreEnemy", "ExploreEnemy_part_" + floor.ToString(), new Func<MasterDataReader, MasterDataTable.ExploreEnemy>(MasterDataTable.ExploreEnemy.Parse), (Func<MasterDataTable.ExploreEnemy, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static MasterDataTable.GearKindCorrelations UniqueGearKindCorrelationsBy(
    MasterDataTable.GearKind attacker,
    MasterDataTable.GearKind defender)
  {
    return MasterDataCache.Unique<int, MasterDataTable.GearKindCorrelations, Tuple<int, int>>("GearKindCorrelations", "By", Tuple.Create<int, int>(attacker.ID, defender.ID), (Func<MasterDataTable.GearKindCorrelations, Tuple<int, int>>) (x => Tuple.Create<int, int>(x.attacker.ID, x.defender.ID)), new Func<MasterDataReader, MasterDataTable.GearKindCorrelations>(MasterDataTable.GearKindCorrelations.Parse), (Func<MasterDataTable.GearKindCorrelations, int>) (x => x.ID));
  }

  public static MasterDataTable.GearKindIncr UniqueGearKindIncrBy(
    MasterDataTable.GearKind attack_kind,
    MasterDataTable.GearKind defense_kind,
    MasterDataTable.UnitProficiency proficiency)
  {
    return MasterDataCache.Unique<int, MasterDataTable.GearKindIncr, Tuple<int, int, int>>("GearKindIncr", "By", Tuple.Create<int, int, int>(attack_kind.ID, defense_kind.ID, proficiency.ID), (Func<MasterDataTable.GearKindIncr, Tuple<int, int, int>>) (x => Tuple.Create<int, int, int>(x.attack_kind.ID, x.defense_kind.ID, x.proficiency.ID)), new Func<MasterDataReader, MasterDataTable.GearKindIncr>(MasterDataTable.GearKindIncr.Parse), (Func<MasterDataTable.GearKindIncr, int>) (x => x.ID));
  }

  public static AssocList<int, MasterDataTable.AIScore> AIScore
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.AIScore>(nameof (AIScore), new Func<MasterDataReader, MasterDataTable.AIScore>(MasterDataTable.AIScore.Parse), (Func<MasterDataTable.AIScore, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.AIScoreCorrection> AIScoreCorrection
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.AIScoreCorrection>(nameof (AIScoreCorrection), new Func<MasterDataReader, MasterDataTable.AIScoreCorrection>(MasterDataTable.AIScoreCorrection.Parse), (Func<MasterDataTable.AIScoreCorrection, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.AIScorePattern> AIScorePattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.AIScorePattern>(nameof (AIScorePattern), new Func<MasterDataReader, MasterDataTable.AIScorePattern>(MasterDataTable.AIScorePattern.Parse), (Func<MasterDataTable.AIScorePattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.AppSetupTuning> AppSetupTuning
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.AppSetupTuning>(nameof (AppSetupTuning), new Func<MasterDataReader, MasterDataTable.AppSetupTuning>(MasterDataTable.AppSetupTuning.Parse), (Func<MasterDataTable.AppSetupTuning, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.AttackMethod> AttackMethod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.AttackMethod>(nameof (AttackMethod), new Func<MasterDataReader, MasterDataTable.AttackMethod>(MasterDataTable.AttackMethod.Parse), (Func<MasterDataTable.AttackMethod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.AwakeSkillCategory> AwakeSkillCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.AwakeSkillCategory>(nameof (AwakeSkillCategory), new Func<MasterDataReader, MasterDataTable.AwakeSkillCategory>(MasterDataTable.AwakeSkillCategory.Parse), (Func<MasterDataTable.AwakeSkillCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleAIScript> BattleAIScript
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleAIScript>(nameof (BattleAIScript), new Func<MasterDataReader, MasterDataTable.BattleAIScript>(MasterDataTable.BattleAIScript.Parse), (Func<MasterDataTable.BattleAIScript, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleCameraFilter> BattleCameraFilter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleCameraFilter>(nameof (BattleCameraFilter), new Func<MasterDataReader, MasterDataTable.BattleCameraFilter>(MasterDataTable.BattleCameraFilter.Parse), (Func<MasterDataTable.BattleCameraFilter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleEarthItemDropTable> BattleEarthItemDropTable
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleEarthItemDropTable>(nameof (BattleEarthItemDropTable), new Func<MasterDataReader, MasterDataTable.BattleEarthItemDropTable>(MasterDataTable.BattleEarthItemDropTable.Parse), (Func<MasterDataTable.BattleEarthItemDropTable, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleEarthStageGuest> BattleEarthStageGuest
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleEarthStageGuest>(nameof (BattleEarthStageGuest), new Func<MasterDataReader, MasterDataTable.BattleEarthStageGuest>(MasterDataTable.BattleEarthStageGuest.Parse), (Func<MasterDataTable.BattleEarthStageGuest, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleEarthStageGuestSkill> BattleEarthStageGuestSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleEarthStageGuestSkill>(nameof (BattleEarthStageGuestSkill), new Func<MasterDataReader, MasterDataTable.BattleEarthStageGuestSkill>(MasterDataTable.BattleEarthStageGuestSkill.Parse), (Func<MasterDataTable.BattleEarthStageGuestSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleEnemyAcquireSkill> BattleEnemyAcquireSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleEnemyAcquireSkill>(nameof (BattleEnemyAcquireSkill), new Func<MasterDataReader, MasterDataTable.BattleEnemyAcquireSkill>(MasterDataTable.BattleEnemyAcquireSkill.Parse), (Func<MasterDataTable.BattleEnemyAcquireSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleEnemyParameterDeviationTable> BattleEnemyParameterDeviationTable
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleEnemyParameterDeviationTable>(nameof (BattleEnemyParameterDeviationTable), new Func<MasterDataReader, MasterDataTable.BattleEnemyParameterDeviationTable>(MasterDataTable.BattleEnemyParameterDeviationTable.Parse), (Func<MasterDataTable.BattleEnemyParameterDeviationTable, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleEnemyParameterTable> BattleEnemyParameterTable
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleEnemyParameterTable>(nameof (BattleEnemyParameterTable), new Func<MasterDataReader, MasterDataTable.BattleEnemyParameterTable>(MasterDataTable.BattleEnemyParameterTable.Parse), (Func<MasterDataTable.BattleEnemyParameterTable, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleFieldEffect> BattleFieldEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleFieldEffect>(nameof (BattleFieldEffect), new Func<MasterDataReader, MasterDataTable.BattleFieldEffect>(MasterDataTable.BattleFieldEffect.Parse), (Func<MasterDataTable.BattleFieldEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleFieldEffectStage> BattleFieldEffectStage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleFieldEffectStage>(nameof (BattleFieldEffectStage), new Func<MasterDataReader, MasterDataTable.BattleFieldEffectStage>(MasterDataTable.BattleFieldEffectStage.Parse), (Func<MasterDataTable.BattleFieldEffectStage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleLandform> BattleLandform
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleLandform>(nameof (BattleLandform), new Func<MasterDataReader, MasterDataTable.BattleLandform>(MasterDataTable.BattleLandform.Parse), (Func<MasterDataTable.BattleLandform, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleLandformEffect> BattleLandformEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleLandformEffect>(nameof (BattleLandformEffect), new Func<MasterDataReader, MasterDataTable.BattleLandformEffect>(MasterDataTable.BattleLandformEffect.Parse), (Func<MasterDataTable.BattleLandformEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleLandformEffectGroup> BattleLandformEffectGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleLandformEffectGroup>(nameof (BattleLandformEffectGroup), new Func<MasterDataReader, MasterDataTable.BattleLandformEffectGroup>(MasterDataTable.BattleLandformEffectGroup.Parse), (Func<MasterDataTable.BattleLandformEffectGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleLandformFootstepType> BattleLandformFootstepType
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleLandformFootstepType>(nameof (BattleLandformFootstepType), new Func<MasterDataReader, MasterDataTable.BattleLandformFootstepType>(MasterDataTable.BattleLandformFootstepType.Parse), (Func<MasterDataTable.BattleLandformFootstepType, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleLandformIncr> BattleLandformIncr
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleLandformIncr>(nameof (BattleLandformIncr), new Func<MasterDataReader, MasterDataTable.BattleLandformIncr>(MasterDataTable.BattleLandformIncr.Parse), (Func<MasterDataTable.BattleLandformIncr, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleLandformTag> BattleLandformTag
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleLandformTag>(nameof (BattleLandformTag), new Func<MasterDataReader, MasterDataTable.BattleLandformTag>(MasterDataTable.BattleLandformTag.Parse), (Func<MasterDataTable.BattleLandformTag, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleMap> BattleMap
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleMap>(nameof (BattleMap), new Func<MasterDataReader, MasterDataTable.BattleMap>(MasterDataTable.BattleMap.Parse), (Func<MasterDataTable.BattleMap, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleMapFacilitySetting> BattleMapFacilitySetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleMapFacilitySetting>(nameof (BattleMapFacilitySetting), new Func<MasterDataReader, MasterDataTable.BattleMapFacilitySetting>(MasterDataTable.BattleMapFacilitySetting.Parse), (Func<MasterDataTable.BattleMapFacilitySetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleMapLandform> BattleMapLandform
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleMapLandform>(nameof (BattleMapLandform), new Func<MasterDataReader, MasterDataTable.BattleMapLandform>(MasterDataTable.BattleMapLandform.Parse), (Func<MasterDataTable.BattleMapLandform, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleReinforcement> BattleReinforcement
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleReinforcement>(nameof (BattleReinforcement), new Func<MasterDataReader, MasterDataTable.BattleReinforcement>(MasterDataTable.BattleReinforcement.Parse), (Func<MasterDataTable.BattleReinforcement, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleReinforcementLogic> BattleReinforcementLogic
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleReinforcementLogic>(nameof (BattleReinforcementLogic), new Func<MasterDataReader, MasterDataTable.BattleReinforcementLogic>(MasterDataTable.BattleReinforcementLogic.Parse), (Func<MasterDataTable.BattleReinforcementLogic, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleSpecialSkill> BattleSpecialSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleSpecialSkill>(nameof (BattleSpecialSkill), new Func<MasterDataReader, MasterDataTable.BattleSpecialSkill>(MasterDataTable.BattleSpecialSkill.Parse), (Func<MasterDataTable.BattleSpecialSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStage> BattleStage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStage>(nameof (BattleStage), new Func<MasterDataReader, MasterDataTable.BattleStage>(MasterDataTable.BattleStage.Parse), (Func<MasterDataTable.BattleStage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageClear> BattleStageClear
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageClear>(nameof (BattleStageClear), new Func<MasterDataReader, MasterDataTable.BattleStageClear>(MasterDataTable.BattleStageClear.Parse), (Func<MasterDataTable.BattleStageClear, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageEnemy> BattleStageEnemy
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageEnemy>(nameof (BattleStageEnemy), new Func<MasterDataReader, MasterDataTable.BattleStageEnemy>(MasterDataTable.BattleStageEnemy.Parse), (Func<MasterDataTable.BattleStageEnemy, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageEnemyAttackMethod> BattleStageEnemyAttackMethod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageEnemyAttackMethod>(nameof (BattleStageEnemyAttackMethod), new Func<MasterDataReader, MasterDataTable.BattleStageEnemyAttackMethod>(MasterDataTable.BattleStageEnemyAttackMethod.Parse), (Func<MasterDataTable.BattleStageEnemyAttackMethod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageEnemyJob> BattleStageEnemyJob
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageEnemyJob>(nameof (BattleStageEnemyJob), new Func<MasterDataReader, MasterDataTable.BattleStageEnemyJob>(MasterDataTable.BattleStageEnemyJob.Parse), (Func<MasterDataTable.BattleStageEnemyJob, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageEnemyReward> BattleStageEnemyReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageEnemyReward>(nameof (BattleStageEnemyReward), new Func<MasterDataReader, MasterDataTable.BattleStageEnemyReward>(MasterDataTable.BattleStageEnemyReward.Parse), (Func<MasterDataTable.BattleStageEnemyReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageEnemySkill> BattleStageEnemySkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageEnemySkill>(nameof (BattleStageEnemySkill), new Func<MasterDataReader, MasterDataTable.BattleStageEnemySkill>(MasterDataTable.BattleStageEnemySkill.Parse), (Func<MasterDataTable.BattleStageEnemySkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageGuest> BattleStageGuest
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageGuest>(nameof (BattleStageGuest), new Func<MasterDataReader, MasterDataTable.BattleStageGuest>(MasterDataTable.BattleStageGuest.Parse), (Func<MasterDataTable.BattleStageGuest, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageGuestAttackMethod> BattleStageGuestAttackMethod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageGuestAttackMethod>(nameof (BattleStageGuestAttackMethod), new Func<MasterDataReader, MasterDataTable.BattleStageGuestAttackMethod>(MasterDataTable.BattleStageGuestAttackMethod.Parse), (Func<MasterDataTable.BattleStageGuestAttackMethod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageGuestJob> BattleStageGuestJob
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageGuestJob>(nameof (BattleStageGuestJob), new Func<MasterDataReader, MasterDataTable.BattleStageGuestJob>(MasterDataTable.BattleStageGuestJob.Parse), (Func<MasterDataTable.BattleStageGuestJob, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageGuestSkill> BattleStageGuestSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageGuestSkill>(nameof (BattleStageGuestSkill), new Func<MasterDataReader, MasterDataTable.BattleStageGuestSkill>(MasterDataTable.BattleStageGuestSkill.Parse), (Func<MasterDataTable.BattleStageGuestSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStagePanelEvent> BattleStagePanelEvent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStagePanelEvent>(nameof (BattleStagePanelEvent), new Func<MasterDataReader, MasterDataTable.BattleStagePanelEvent>(MasterDataTable.BattleStagePanelEvent.Parse), (Func<MasterDataTable.BattleStagePanelEvent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStagePlayer> BattleStagePlayer
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStagePlayer>(nameof (BattleStagePlayer), new Func<MasterDataReader, MasterDataTable.BattleStagePlayer>(MasterDataTable.BattleStagePlayer.Parse), (Func<MasterDataTable.BattleStagePlayer, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleStageUserUnit> BattleStageUserUnit
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleStageUserUnit>(nameof (BattleStageUserUnit), new Func<MasterDataReader, MasterDataTable.BattleStageUserUnit>(MasterDataTable.BattleStageUserUnit.Parse), (Func<MasterDataTable.BattleStageUserUnit, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleUnitLandformFootstep> BattleUnitLandformFootstep
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleUnitLandformFootstep>(nameof (BattleUnitLandformFootstep), new Func<MasterDataReader, MasterDataTable.BattleUnitLandformFootstep>(MasterDataTable.BattleUnitLandformFootstep.Parse), (Func<MasterDataTable.BattleUnitLandformFootstep, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleUnitRule> BattleUnitRule
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleUnitRule>(nameof (BattleUnitRule), new Func<MasterDataReader, MasterDataTable.BattleUnitRule>(MasterDataTable.BattleUnitRule.Parse), (Func<MasterDataTable.BattleUnitRule, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleVictoryAreaCondition> BattleVictoryAreaCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleVictoryAreaCondition>(nameof (BattleVictoryAreaCondition), new Func<MasterDataReader, MasterDataTable.BattleVictoryAreaCondition>(MasterDataTable.BattleVictoryAreaCondition.Parse), (Func<MasterDataTable.BattleVictoryAreaCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleVictoryCondition> BattleVictoryCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleVictoryCondition>(nameof (BattleVictoryCondition), new Func<MasterDataReader, MasterDataTable.BattleVictoryCondition>(MasterDataTable.BattleVictoryCondition.Parse), (Func<MasterDataTable.BattleVictoryCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillAilmentEffect> BattleskillAilmentEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillAilmentEffect>(nameof (BattleskillAilmentEffect), new Func<MasterDataReader, MasterDataTable.BattleskillAilmentEffect>(MasterDataTable.BattleskillAilmentEffect.Parse), (Func<MasterDataTable.BattleskillAilmentEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillDuelClipEventEffectDataPreload> BattleskillDuelClipEventEffectDataPreload
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillDuelClipEventEffectDataPreload>(nameof (BattleskillDuelClipEventEffectDataPreload), new Func<MasterDataReader, MasterDataTable.BattleskillDuelClipEventEffectDataPreload>(MasterDataTable.BattleskillDuelClipEventEffectDataPreload.Parse), (Func<MasterDataTable.BattleskillDuelClipEventEffectDataPreload, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillDuelCutinPreload> BattleskillDuelCutinPreload
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillDuelCutinPreload>(nameof (BattleskillDuelCutinPreload), new Func<MasterDataReader, MasterDataTable.BattleskillDuelCutinPreload>(MasterDataTable.BattleskillDuelCutinPreload.Parse), (Func<MasterDataTable.BattleskillDuelCutinPreload, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillDuelEffect> BattleskillDuelEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillDuelEffect>(nameof (BattleskillDuelEffect), new Func<MasterDataReader, MasterDataTable.BattleskillDuelEffect>(MasterDataTable.BattleskillDuelEffect.Parse), (Func<MasterDataTable.BattleskillDuelEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillDuelEffectPreload> BattleskillDuelEffectPreload
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillDuelEffectPreload>(nameof (BattleskillDuelEffectPreload), new Func<MasterDataReader, MasterDataTable.BattleskillDuelEffectPreload>(MasterDataTable.BattleskillDuelEffectPreload.Parse), (Func<MasterDataTable.BattleskillDuelEffectPreload, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillEffect> BattleskillEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillEffect>(nameof (BattleskillEffect), new Func<MasterDataReader, MasterDataTable.BattleskillEffect>(MasterDataTable.BattleskillEffect.Parse), (Func<MasterDataTable.BattleskillEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillEffectLogic> BattleskillEffectLogic
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillEffectLogic>(nameof (BattleskillEffectLogic), new Func<MasterDataReader, MasterDataTable.BattleskillEffectLogic>(MasterDataTable.BattleskillEffectLogic.Parse), (Func<MasterDataTable.BattleskillEffectLogic, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillFieldEffect> BattleskillFieldEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillFieldEffect>(nameof (BattleskillFieldEffect), new Func<MasterDataReader, MasterDataTable.BattleskillFieldEffect>(MasterDataTable.BattleskillFieldEffect.Parse), (Func<MasterDataTable.BattleskillFieldEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillLifeCycle> BattleskillLifeCycle
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillLifeCycle>(nameof (BattleskillLifeCycle), new Func<MasterDataReader, MasterDataTable.BattleskillLifeCycle>(MasterDataTable.BattleskillLifeCycle.Parse), (Func<MasterDataTable.BattleskillLifeCycle, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillSkill> BattleskillSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillSkill>(nameof (BattleskillSkill), new Func<MasterDataReader, MasterDataTable.BattleskillSkill>(MasterDataTable.BattleskillSkill.Parse), (Func<MasterDataTable.BattleskillSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillTiming> BattleskillTiming
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillTiming>(nameof (BattleskillTiming), new Func<MasterDataReader, MasterDataTable.BattleskillTiming>(MasterDataTable.BattleskillTiming.Parse), (Func<MasterDataTable.BattleskillTiming, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BattleskillTimingLogic> BattleskillTimingLogic
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BattleskillTimingLogic>(nameof (BattleskillTimingLogic), new Func<MasterDataReader, MasterDataTable.BattleskillTimingLogic>(MasterDataTable.BattleskillTimingLogic.Parse), (Func<MasterDataTable.BattleskillTimingLogic, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BeginnerNaviCategory> BeginnerNaviCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BeginnerNaviCategory>(nameof (BeginnerNaviCategory), new Func<MasterDataReader, MasterDataTable.BeginnerNaviCategory>(MasterDataTable.BeginnerNaviCategory.Parse), (Func<MasterDataTable.BeginnerNaviCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BeginnerNaviDetail> BeginnerNaviDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BeginnerNaviDetail>(nameof (BeginnerNaviDetail), new Func<MasterDataReader, MasterDataTable.BeginnerNaviDetail>(MasterDataTable.BeginnerNaviDetail.Parse), (Func<MasterDataTable.BeginnerNaviDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BeginnerNaviMovePage> BeginnerNaviMovePage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BeginnerNaviMovePage>(nameof (BeginnerNaviMovePage), new Func<MasterDataReader, MasterDataTable.BeginnerNaviMovePage>(MasterDataTable.BeginnerNaviMovePage.Parse), (Func<MasterDataTable.BeginnerNaviMovePage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BeginnerNaviTitle> BeginnerNaviTitle
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BeginnerNaviTitle>(nameof (BeginnerNaviTitle), new Func<MasterDataReader, MasterDataTable.BeginnerNaviTitle>(MasterDataTable.BeginnerNaviTitle.Parse), (Func<MasterDataTable.BeginnerNaviTitle, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BingoBingo> BingoBingo
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BingoBingo>(nameof (BingoBingo), new Func<MasterDataReader, MasterDataTable.BingoBingo>(MasterDataTable.BingoBingo.Parse), (Func<MasterDataTable.BingoBingo, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BingoMission> BingoMission
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BingoMission>(nameof (BingoMission), new Func<MasterDataReader, MasterDataTable.BingoMission>(MasterDataTable.BingoMission.Parse), (Func<MasterDataTable.BingoMission, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BingoRewardGroup> BingoRewardGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BingoRewardGroup>(nameof (BingoRewardGroup), new Func<MasterDataReader, MasterDataTable.BingoRewardGroup>(MasterDataTable.BingoRewardGroup.Parse), (Func<MasterDataTable.BingoRewardGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BoostBonusGearCombine> BoostBonusGearCombine
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BoostBonusGearCombine>(nameof (BoostBonusGearCombine), new Func<MasterDataReader, MasterDataTable.BoostBonusGearCombine>(MasterDataTable.BoostBonusGearCombine.Parse), (Func<MasterDataTable.BoostBonusGearCombine, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BoostBonusGearDrilling> BoostBonusGearDrilling
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BoostBonusGearDrilling>(nameof (BoostBonusGearDrilling), new Func<MasterDataReader, MasterDataTable.BoostBonusGearDrilling>(MasterDataTable.BoostBonusGearDrilling.Parse), (Func<MasterDataTable.BoostBonusGearDrilling, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BoostBonusUnitBuildup> BoostBonusUnitBuildup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BoostBonusUnitBuildup>(nameof (BoostBonusUnitBuildup), new Func<MasterDataReader, MasterDataTable.BoostBonusUnitBuildup>(MasterDataTable.BoostBonusUnitBuildup.Parse), (Func<MasterDataTable.BoostBonusUnitBuildup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BoostBonusUnitCompose> BoostBonusUnitCompose
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BoostBonusUnitCompose>(nameof (BoostBonusUnitCompose), new Func<MasterDataReader, MasterDataTable.BoostBonusUnitCompose>(MasterDataTable.BoostBonusUnitCompose.Parse), (Func<MasterDataTable.BoostBonusUnitCompose, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BoostBonusUnitTransmigrate> BoostBonusUnitTransmigrate
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BoostBonusUnitTransmigrate>(nameof (BoostBonusUnitTransmigrate), new Func<MasterDataReader, MasterDataTable.BoostBonusUnitTransmigrate>(MasterDataTable.BoostBonusUnitTransmigrate.Parse), (Func<MasterDataTable.BoostBonusUnitTransmigrate, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BoostCampaignTypeName> BoostCampaignTypeName
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BoostCampaignTypeName>(nameof (BoostCampaignTypeName), new Func<MasterDataReader, MasterDataTable.BoostCampaignTypeName>(MasterDataTable.BoostCampaignTypeName.Parse), (Func<MasterDataTable.BoostCampaignTypeName, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BoostPeriod> BoostPeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BoostPeriod>(nameof (BoostPeriod), new Func<MasterDataReader, MasterDataTable.BoostPeriod>(MasterDataTable.BoostPeriod.Parse), (Func<MasterDataTable.BoostPeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BoostXExperience> BoostXExperience
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BoostXExperience>(nameof (BoostXExperience), new Func<MasterDataReader, MasterDataTable.BoostXExperience>(MasterDataTable.BoostXExperience.Parse), (Func<MasterDataTable.BoostXExperience, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.BreakThroughBuildupSkill> BreakThroughBuildupSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.BreakThroughBuildupSkill>(nameof (BreakThroughBuildupSkill), new Func<MasterDataReader, MasterDataTable.BreakThroughBuildupSkill>(MasterDataTable.BreakThroughBuildupSkill.Parse), (Func<MasterDataTable.BreakThroughBuildupSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallCharacter> CallCharacter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallCharacter>(nameof (CallCharacter), new Func<MasterDataReader, MasterDataTable.CallCharacter>(MasterDataTable.CallCharacter.Parse), (Func<MasterDataTable.CallCharacter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallGaugeRate> CallGaugeRate
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallGaugeRate>(nameof (CallGaugeRate), new Func<MasterDataReader, MasterDataTable.CallGaugeRate>(MasterDataTable.CallGaugeRate.Parse), (Func<MasterDataTable.CallGaugeRate, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallGiftRecipe> CallGiftRecipe
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallGiftRecipe>(nameof (CallGiftRecipe), new Func<MasterDataReader, MasterDataTable.CallGiftRecipe>(MasterDataTable.CallGiftRecipe.Parse), (Func<MasterDataTable.CallGiftRecipe, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallIntimateGaugeRate> CallIntimateGaugeRate
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallIntimateGaugeRate>(nameof (CallIntimateGaugeRate), new Func<MasterDataReader, MasterDataTable.CallIntimateGaugeRate>(MasterDataTable.CallIntimateGaugeRate.Parse), (Func<MasterDataTable.CallIntimateGaugeRate, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallItem> CallItem
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallItem>(nameof (CallItem), new Func<MasterDataReader, MasterDataTable.CallItem>(MasterDataTable.CallItem.Parse), (Func<MasterDataTable.CallItem, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallMessage> CallMessage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallMessage>(nameof (CallMessage), new Func<MasterDataReader, MasterDataTable.CallMessage>(MasterDataTable.CallMessage.Parse), (Func<MasterDataTable.CallMessage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallMission> CallMission
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallMission>(nameof (CallMission), new Func<MasterDataReader, MasterDataTable.CallMission>(MasterDataTable.CallMission.Parse), (Func<MasterDataTable.CallMission, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallSkillGaugeRate> CallSkillGaugeRate
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallSkillGaugeRate>(nameof (CallSkillGaugeRate), new Func<MasterDataReader, MasterDataTable.CallSkillGaugeRate>(MasterDataTable.CallSkillGaugeRate.Parse), (Func<MasterDataTable.CallSkillGaugeRate, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CallUnitGroup> CallUnitGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CallUnitGroup>(nameof (CallUnitGroup), new Func<MasterDataReader, MasterDataTable.CallUnitGroup>(MasterDataTable.CallUnitGroup.Parse), (Func<MasterDataTable.CallUnitGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ClassRankingHowto> ClassRankingHowto
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ClassRankingHowto>(nameof (ClassRankingHowto), new Func<MasterDataReader, MasterDataTable.ClassRankingHowto>(MasterDataTable.ClassRankingHowto.Parse), (Func<MasterDataTable.ClassRankingHowto, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CoinChargeLimit> CoinChargeLimit
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CoinChargeLimit>(nameof (CoinChargeLimit), new Func<MasterDataReader, MasterDataTable.CoinChargeLimit>(MasterDataTable.CoinChargeLimit.Parse), (Func<MasterDataTable.CoinChargeLimit, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CoinProduct> CoinProduct
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CoinProduct>(nameof (CoinProduct), new Func<MasterDataReader, MasterDataTable.CoinProduct>(MasterDataTable.CoinProduct.Parse), (Func<MasterDataTable.CoinProduct, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CoinProductDetail> CoinProductDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CoinProductDetail>(nameof (CoinProductDetail), new Func<MasterDataReader, MasterDataTable.CoinProductDetail>(MasterDataTable.CoinProductDetail.Parse), (Func<MasterDataTable.CoinProductDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ColosseumBonus> ColosseumBonus
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ColosseumBonus>(nameof (ColosseumBonus), new Func<MasterDataReader, MasterDataTable.ColosseumBonus>(MasterDataTable.ColosseumBonus.Parse), (Func<MasterDataTable.ColosseumBonus, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ColosseumBonusBloodType> ColosseumBonusBloodType
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ColosseumBonusBloodType>(nameof (ColosseumBonusBloodType), new Func<MasterDataReader, MasterDataTable.ColosseumBonusBloodType>(MasterDataTable.ColosseumBonusBloodType.Parse), (Func<MasterDataTable.ColosseumBonusBloodType, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ColosseumBonusZodiacType> ColosseumBonusZodiacType
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ColosseumBonusZodiacType>(nameof (ColosseumBonusZodiacType), new Func<MasterDataReader, MasterDataTable.ColosseumBonusZodiacType>(MasterDataTable.ColosseumBonusZodiacType.Parse), (Func<MasterDataTable.ColosseumBonusZodiacType, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ColosseumRank> ColosseumRank
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ColosseumRank>(nameof (ColosseumRank), new Func<MasterDataReader, MasterDataTable.ColosseumRank>(MasterDataTable.ColosseumRank.Parse), (Func<MasterDataTable.ColosseumRank, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ColosseumRankReward> ColosseumRankReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ColosseumRankReward>(nameof (ColosseumRankReward), new Func<MasterDataReader, MasterDataTable.ColosseumRankReward>(MasterDataTable.ColosseumRankReward.Parse), (Func<MasterDataTable.ColosseumRankReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ColosseumTotalVictoryReward> ColosseumTotalVictoryReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ColosseumTotalVictoryReward>(nameof (ColosseumTotalVictoryReward), new Func<MasterDataReader, MasterDataTable.ColosseumTotalVictoryReward>(MasterDataTable.ColosseumTotalVictoryReward.Parse), (Func<MasterDataTable.ColosseumTotalVictoryReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CommonElementName> CommonElementName
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CommonElementName>(nameof (CommonElementName), new Func<MasterDataReader, MasterDataTable.CommonElementName>(MasterDataTable.CommonElementName.Parse), (Func<MasterDataTable.CommonElementName, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CommonMypageSetting> CommonMypageSetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CommonMypageSetting>(nameof (CommonMypageSetting), new Func<MasterDataReader, MasterDataTable.CommonMypageSetting>(MasterDataTable.CommonMypageSetting.Parse), (Func<MasterDataTable.CommonMypageSetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CommonQuestBattleEffect> CommonQuestBattleEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CommonQuestBattleEffect>(nameof (CommonQuestBattleEffect), new Func<MasterDataReader, MasterDataTable.CommonQuestBattleEffect>(MasterDataTable.CommonQuestBattleEffect.Parse), (Func<MasterDataTable.CommonQuestBattleEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CommonStrengthComposePrice> CommonStrengthComposePrice
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CommonStrengthComposePrice>(nameof (CommonStrengthComposePrice), new Func<MasterDataReader, MasterDataTable.CommonStrengthComposePrice>(MasterDataTable.CommonStrengthComposePrice.Parse), (Func<MasterDataTable.CommonStrengthComposePrice, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CommonTicket> CommonTicket
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CommonTicket>(nameof (CommonTicket), new Func<MasterDataReader, MasterDataTable.CommonTicket>(MasterDataTable.CommonTicket.Parse), (Func<MasterDataTable.CommonTicket, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CommonTicketEndAt> CommonTicketEndAt
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CommonTicketEndAt>(nameof (CommonTicketEndAt), new Func<MasterDataReader, MasterDataTable.CommonTicketEndAt>(MasterDataTable.CommonTicketEndAt.Parse), (Func<MasterDataTable.CommonTicketEndAt, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ComposeMaxUnityValueSetting> ComposeMaxUnityValueSetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ComposeMaxUnityValueSetting>(nameof (ComposeMaxUnityValueSetting), new Func<MasterDataReader, MasterDataTable.ComposeMaxUnityValueSetting>(MasterDataTable.ComposeMaxUnityValueSetting.Parse), (Func<MasterDataTable.ComposeMaxUnityValueSetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ConstsConsts> ConstsConsts
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ConstsConsts>(nameof (ConstsConsts), new Func<MasterDataReader, MasterDataTable.ConstsConsts>(MasterDataTable.ConstsConsts.Parse), (Func<MasterDataTable.ConstsConsts, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsCameraFilter> CorpsCameraFilter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsCameraFilter>(nameof (CorpsCameraFilter), new Func<MasterDataReader, MasterDataTable.CorpsCameraFilter>(MasterDataTable.CorpsCameraFilter.Parse), (Func<MasterDataTable.CorpsCameraFilter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsEntryConditions> CorpsEntryConditions
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsEntryConditions>(nameof (CorpsEntryConditions), new Func<MasterDataReader, MasterDataTable.CorpsEntryConditions>(MasterDataTable.CorpsEntryConditions.Parse), (Func<MasterDataTable.CorpsEntryConditions, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsHowto> CorpsHowto
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsHowto>(nameof (CorpsHowto), new Func<MasterDataReader, MasterDataTable.CorpsHowto>(MasterDataTable.CorpsHowto.Parse), (Func<MasterDataTable.CorpsHowto, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsMissionReward> CorpsMissionReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsMissionReward>(nameof (CorpsMissionReward), new Func<MasterDataReader, MasterDataTable.CorpsMissionReward>(MasterDataTable.CorpsMissionReward.Parse), (Func<MasterDataTable.CorpsMissionReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsPeriod> CorpsPeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsPeriod>(nameof (CorpsPeriod), new Func<MasterDataReader, MasterDataTable.CorpsPeriod>(MasterDataTable.CorpsPeriod.Parse), (Func<MasterDataTable.CorpsPeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsPlaybackStory> CorpsPlaybackStory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsPlaybackStory>(nameof (CorpsPlaybackStory), new Func<MasterDataReader, MasterDataTable.CorpsPlaybackStory>(MasterDataTable.CorpsPlaybackStory.Parse), (Func<MasterDataTable.CorpsPlaybackStory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsPlaybackStoryDetail> CorpsPlaybackStoryDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsPlaybackStoryDetail>(nameof (CorpsPlaybackStoryDetail), new Func<MasterDataReader, MasterDataTable.CorpsPlaybackStoryDetail>(MasterDataTable.CorpsPlaybackStoryDetail.Parse), (Func<MasterDataTable.CorpsPlaybackStoryDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsSetting> CorpsSetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsSetting>(nameof (CorpsSetting), new Func<MasterDataReader, MasterDataTable.CorpsSetting>(MasterDataTable.CorpsSetting.Parse), (Func<MasterDataTable.CorpsSetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsStage> CorpsStage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsStage>(nameof (CorpsStage), new Func<MasterDataReader, MasterDataTable.CorpsStage>(MasterDataTable.CorpsStage.Parse), (Func<MasterDataTable.CorpsStage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsStageBackground> CorpsStageBackground
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsStageBackground>(nameof (CorpsStageBackground), new Func<MasterDataReader, MasterDataTable.CorpsStageBackground>(MasterDataTable.CorpsStageBackground.Parse), (Func<MasterDataTable.CorpsStageBackground, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsStageClearReward> CorpsStageClearReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsStageClearReward>(nameof (CorpsStageClearReward), new Func<MasterDataReader, MasterDataTable.CorpsStageClearReward>(MasterDataTable.CorpsStageClearReward.Parse), (Func<MasterDataTable.CorpsStageClearReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.CorpsStageOpenConditions> CorpsStageOpenConditions
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.CorpsStageOpenConditions>(nameof (CorpsStageOpenConditions), new Func<MasterDataReader, MasterDataTable.CorpsStageOpenConditions>(MasterDataTable.CorpsStageOpenConditions.Parse), (Func<MasterDataTable.CorpsStageOpenConditions, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DailyMission> DailyMission
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DailyMission>(nameof (DailyMission), new Func<MasterDataReader, MasterDataTable.DailyMission>(MasterDataTable.DailyMission.Parse), (Func<MasterDataTable.DailyMission, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DailyMissionTopPage> DailyMissionTopPage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DailyMissionTopPage>(nameof (DailyMissionTopPage), new Func<MasterDataReader, MasterDataTable.DailyMissionTopPage>(MasterDataTable.DailyMissionTopPage.Parse), (Func<MasterDataTable.DailyMissionTopPage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DateScriptBase> DateScriptBase
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DateScriptBase>(nameof (DateScriptBase), new Func<MasterDataReader, MasterDataTable.DateScriptBase>(MasterDataTable.DateScriptBase.Parse), (Func<MasterDataTable.DateScriptBase, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DateScriptParts> DateScriptParts
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DateScriptParts>(nameof (DateScriptParts), new Func<MasterDataReader, MasterDataTable.DateScriptParts>(MasterDataTable.DateScriptParts.Parse), (Func<MasterDataTable.DateScriptParts, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DateScriptQuestion> DateScriptQuestion
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DateScriptQuestion>(nameof (DateScriptQuestion), new Func<MasterDataReader, MasterDataTable.DateScriptQuestion>(MasterDataTable.DateScriptQuestion.Parse), (Func<MasterDataTable.DateScriptQuestion, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DuelDuelConfig> DuelDuelConfig
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DuelDuelConfig>(nameof (DuelDuelConfig), new Func<MasterDataReader, MasterDataTable.DuelDuelConfig>(MasterDataTable.DuelDuelConfig.Parse), (Func<MasterDataTable.DuelDuelConfig, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DuelEffectPreload> DuelEffectPreload
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DuelEffectPreload>(nameof (DuelEffectPreload), new Func<MasterDataReader, MasterDataTable.DuelEffectPreload>(MasterDataTable.DuelEffectPreload.Parse), (Func<MasterDataTable.DuelEffectPreload, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DuelElementBulletEffect> DuelElementBulletEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DuelElementBulletEffect>(nameof (DuelElementBulletEffect), new Func<MasterDataReader, MasterDataTable.DuelElementBulletEffect>(MasterDataTable.DuelElementBulletEffect.Parse), (Func<MasterDataTable.DuelElementBulletEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DuelElementHitEffect> DuelElementHitEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DuelElementHitEffect>(nameof (DuelElementHitEffect), new Func<MasterDataReader, MasterDataTable.DuelElementHitEffect>(MasterDataTable.DuelElementHitEffect.Parse), (Func<MasterDataTable.DuelElementHitEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.DuelElementTrailEffect> DuelElementTrailEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.DuelElementTrailEffect>(nameof (DuelElementTrailEffect), new Func<MasterDataReader, MasterDataTable.DuelElementTrailEffect>(MasterDataTable.DuelElementTrailEffect.Parse), (Func<MasterDataTable.DuelElementTrailEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthAwakeSkillCategory> EarthAwakeSkillCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthAwakeSkillCategory>(nameof (EarthAwakeSkillCategory), new Func<MasterDataReader, MasterDataTable.EarthAwakeSkillCategory>(MasterDataTable.EarthAwakeSkillCategory.Parse), (Func<MasterDataTable.EarthAwakeSkillCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthBattleStagePanelEvent> EarthBattleStagePanelEvent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthBattleStagePanelEvent>(nameof (EarthBattleStagePanelEvent), new Func<MasterDataReader, MasterDataTable.EarthBattleStagePanelEvent>(MasterDataTable.EarthBattleStagePanelEvent.Parse), (Func<MasterDataTable.EarthBattleStagePanelEvent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthDesertCharacter> EarthDesertCharacter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthDesertCharacter>(nameof (EarthDesertCharacter), new Func<MasterDataReader, MasterDataTable.EarthDesertCharacter>(MasterDataTable.EarthDesertCharacter.Parse), (Func<MasterDataTable.EarthDesertCharacter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthExtraQuest> EarthExtraQuest
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthExtraQuest>(nameof (EarthExtraQuest), new Func<MasterDataReader, MasterDataTable.EarthExtraQuest>(MasterDataTable.EarthExtraQuest.Parse), (Func<MasterDataTable.EarthExtraQuest, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthForcedSortieCharacter> EarthForcedSortieCharacter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthForcedSortieCharacter>(nameof (EarthForcedSortieCharacter), new Func<MasterDataReader, MasterDataTable.EarthForcedSortieCharacter>(MasterDataTable.EarthForcedSortieCharacter.Parse), (Func<MasterDataTable.EarthForcedSortieCharacter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthImpossibleOFSortieCharacter> EarthImpossibleOFSortieCharacter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthImpossibleOFSortieCharacter>(nameof (EarthImpossibleOFSortieCharacter), new Func<MasterDataReader, MasterDataTable.EarthImpossibleOFSortieCharacter>(MasterDataTable.EarthImpossibleOFSortieCharacter.Parse), (Func<MasterDataTable.EarthImpossibleOFSortieCharacter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthJoinCharacter> EarthJoinCharacter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthJoinCharacter>(nameof (EarthJoinCharacter), new Func<MasterDataReader, MasterDataTable.EarthJoinCharacter>(MasterDataTable.EarthJoinCharacter.Parse), (Func<MasterDataTable.EarthJoinCharacter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthQuestChapter> EarthQuestChapter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthQuestChapter>(nameof (EarthQuestChapter), new Func<MasterDataReader, MasterDataTable.EarthQuestChapter>(MasterDataTable.EarthQuestChapter.Parse), (Func<MasterDataTable.EarthQuestChapter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthQuestClearReward> EarthQuestClearReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthQuestClearReward>(nameof (EarthQuestClearReward), new Func<MasterDataReader, MasterDataTable.EarthQuestClearReward>(MasterDataTable.EarthQuestClearReward.Parse), (Func<MasterDataTable.EarthQuestClearReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthQuestEpisode> EarthQuestEpisode
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthQuestEpisode>(nameof (EarthQuestEpisode), new Func<MasterDataReader, MasterDataTable.EarthQuestEpisode>(MasterDataTable.EarthQuestEpisode.Parse), (Func<MasterDataTable.EarthQuestEpisode, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthQuestExtraStoryPlayback> EarthQuestExtraStoryPlayback
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthQuestExtraStoryPlayback>(nameof (EarthQuestExtraStoryPlayback), new Func<MasterDataReader, MasterDataTable.EarthQuestExtraStoryPlayback>(MasterDataTable.EarthQuestExtraStoryPlayback.Parse), (Func<MasterDataTable.EarthQuestExtraStoryPlayback, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthQuestKey> EarthQuestKey
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthQuestKey>(nameof (EarthQuestKey), new Func<MasterDataReader, MasterDataTable.EarthQuestKey>(MasterDataTable.EarthQuestKey.Parse), (Func<MasterDataTable.EarthQuestKey, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthQuestPologue> EarthQuestPologue
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthQuestPologue>(nameof (EarthQuestPologue), new Func<MasterDataReader, MasterDataTable.EarthQuestPologue>(MasterDataTable.EarthQuestPologue.Parse), (Func<MasterDataTable.EarthQuestPologue, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthQuestStoryPlayback> EarthQuestStoryPlayback
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthQuestStoryPlayback>(nameof (EarthQuestStoryPlayback), new Func<MasterDataReader, MasterDataTable.EarthQuestStoryPlayback>(MasterDataTable.EarthQuestStoryPlayback.Parse), (Func<MasterDataTable.EarthQuestStoryPlayback, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthShopArticle> EarthShopArticle
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthShopArticle>(nameof (EarthShopArticle), new Func<MasterDataReader, MasterDataTable.EarthShopArticle>(MasterDataTable.EarthShopArticle.Parse), (Func<MasterDataTable.EarthShopArticle, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EarthShopContent> EarthShopContent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EarthShopContent>(nameof (EarthShopContent), new Func<MasterDataReader, MasterDataTable.EarthShopContent>(MasterDataTable.EarthShopContent.Parse), (Func<MasterDataTable.EarthShopContent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EmblemEmblem> EmblemEmblem
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EmblemEmblem>(nameof (EmblemEmblem), new Func<MasterDataReader, MasterDataTable.EmblemEmblem>(MasterDataTable.EmblemEmblem.Parse), (Func<MasterDataTable.EmblemEmblem, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.EmblemRarity> EmblemRarity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.EmblemRarity>(nameof (EmblemRarity), new Func<MasterDataReader, MasterDataTable.EmblemRarity>(MasterDataTable.EmblemRarity.Parse), (Func<MasterDataTable.EmblemRarity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreCommonAnimation> ExploreCommonAnimation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreCommonAnimation>(nameof (ExploreCommonAnimation), new Func<MasterDataReader, MasterDataTable.ExploreCommonAnimation>(MasterDataTable.ExploreCommonAnimation.Parse), (Func<MasterDataTable.ExploreCommonAnimation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreDropReward> ExploreDropReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreDropReward>(nameof (ExploreDropReward), new Func<MasterDataReader, MasterDataTable.ExploreDropReward>(MasterDataTable.ExploreDropReward.Parse), (Func<MasterDataTable.ExploreDropReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreDropTable> ExploreDropTable
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreDropTable>(nameof (ExploreDropTable), new Func<MasterDataReader, MasterDataTable.ExploreDropTable>(MasterDataTable.ExploreDropTable.Parse), (Func<MasterDataTable.ExploreDropTable, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreEnemy> ExploreEnemy
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreEnemy>(nameof (ExploreEnemy), new Func<MasterDataReader, MasterDataTable.ExploreEnemy>(MasterDataTable.ExploreEnemy.Parse), (Func<MasterDataTable.ExploreEnemy, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreFloor> ExploreFloor
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreFloor>(nameof (ExploreFloor), new Func<MasterDataReader, MasterDataTable.ExploreFloor>(MasterDataTable.ExploreFloor.Parse), (Func<MasterDataTable.ExploreFloor, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreFloorReward> ExploreFloorReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreFloorReward>(nameof (ExploreFloorReward), new Func<MasterDataReader, MasterDataTable.ExploreFloorReward>(MasterDataTable.ExploreFloorReward.Parse), (Func<MasterDataTable.ExploreFloorReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreRankingCondition> ExploreRankingCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreRankingCondition>(nameof (ExploreRankingCondition), new Func<MasterDataReader, MasterDataTable.ExploreRankingCondition>(MasterDataTable.ExploreRankingCondition.Parse), (Func<MasterDataTable.ExploreRankingCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreRankingPeriod> ExploreRankingPeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreRankingPeriod>(nameof (ExploreRankingPeriod), new Func<MasterDataReader, MasterDataTable.ExploreRankingPeriod>(MasterDataTable.ExploreRankingPeriod.Parse), (Func<MasterDataTable.ExploreRankingPeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreRankingReward> ExploreRankingReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreRankingReward>(nameof (ExploreRankingReward), new Func<MasterDataReader, MasterDataTable.ExploreRankingReward>(MasterDataTable.ExploreRankingReward.Parse), (Func<MasterDataTable.ExploreRankingReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ExploreTimeConfig> ExploreTimeConfig
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ExploreTimeConfig>(nameof (ExploreTimeConfig), new Func<MasterDataReader, MasterDataTable.ExploreTimeConfig>(MasterDataTable.ExploreTimeConfig.Parse), (Func<MasterDataTable.ExploreTimeConfig, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.FacilityLevel> FacilityLevel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.FacilityLevel>(nameof (FacilityLevel), new Func<MasterDataReader, MasterDataTable.FacilityLevel>(MasterDataTable.FacilityLevel.Parse), (Func<MasterDataTable.FacilityLevel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.FacilitySkillGroup> FacilitySkillGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.FacilitySkillGroup>(nameof (FacilitySkillGroup), new Func<MasterDataReader, MasterDataTable.FacilitySkillGroup>(MasterDataTable.FacilitySkillGroup.Parse), (Func<MasterDataTable.FacilitySkillGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GachaTicket> GachaTicket
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GachaTicket>(nameof (GachaTicket), new Func<MasterDataReader, MasterDataTable.GachaTicket>(MasterDataTable.GachaTicket.Parse), (Func<MasterDataTable.GachaTicket, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GachaTutorial> GachaTutorial
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GachaTutorial>(nameof (GachaTutorial), new Func<MasterDataReader, MasterDataTable.GachaTutorial>(MasterDataTable.GachaTutorial.Parse), (Func<MasterDataTable.GachaTutorial, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GachaTutorialBanner> GachaTutorialBanner
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GachaTutorialBanner>(nameof (GachaTutorialBanner), new Func<MasterDataReader, MasterDataTable.GachaTutorialBanner>(MasterDataTable.GachaTutorialBanner.Parse), (Func<MasterDataTable.GachaTutorialBanner, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GachaTutorialDeck> GachaTutorialDeck
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GachaTutorialDeck>(nameof (GachaTutorialDeck), new Func<MasterDataReader, MasterDataTable.GachaTutorialDeck>(MasterDataTable.GachaTutorialDeck.Parse), (Func<MasterDataTable.GachaTutorialDeck, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GachaTutorialDeckEntity> GachaTutorialDeckEntity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GachaTutorialDeckEntity>(nameof (GachaTutorialDeckEntity), new Func<MasterDataReader, MasterDataTable.GachaTutorialDeckEntity>(MasterDataTable.GachaTutorialDeckEntity.Parse), (Func<MasterDataTable.GachaTutorialDeckEntity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GachaTutorialFixedEntity> GachaTutorialFixedEntity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GachaTutorialFixedEntity>(nameof (GachaTutorialFixedEntity), new Func<MasterDataReader, MasterDataTable.GachaTutorialFixedEntity>(MasterDataTable.GachaTutorialFixedEntity.Parse), (Func<MasterDataTable.GachaTutorialFixedEntity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GachaTutorialPeriod> GachaTutorialPeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GachaTutorialPeriod>(nameof (GachaTutorialPeriod), new Func<MasterDataReader, MasterDataTable.GachaTutorialPeriod>(MasterDataTable.GachaTutorialPeriod.Parse), (Func<MasterDataTable.GachaTutorialPeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GachaTutorialbutton> GachaTutorialbutton
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GachaTutorialbutton>(nameof (GachaTutorialbutton), new Func<MasterDataReader, MasterDataTable.GachaTutorialbutton>(MasterDataTable.GachaTutorialbutton.Parse), (Func<MasterDataTable.GachaTutorialbutton, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearAttachedElement> GearAttachedElement
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearAttachedElement>(nameof (GearAttachedElement), new Func<MasterDataReader, MasterDataTable.GearAttachedElement>(MasterDataTable.GearAttachedElement.Parse), (Func<MasterDataTable.GearAttachedElement, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearAttackClassificationTable> GearAttackClassificationTable
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearAttackClassificationTable>(nameof (GearAttackClassificationTable), new Func<MasterDataReader, MasterDataTable.GearAttackClassificationTable>(MasterDataTable.GearAttackClassificationTable.Parse), (Func<MasterDataTable.GearAttackClassificationTable, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearBuildup> GearBuildup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearBuildup>(nameof (GearBuildup), new Func<MasterDataReader, MasterDataTable.GearBuildup>(MasterDataTable.GearBuildup.Parse), (Func<MasterDataTable.GearBuildup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearBuildupLogic> GearBuildupLogic
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearBuildupLogic>(nameof (GearBuildupLogic), new Func<MasterDataReader, MasterDataTable.GearBuildupLogic>(MasterDataTable.GearBuildupLogic.Parse), (Func<MasterDataTable.GearBuildupLogic, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearClassificationPattern> GearClassificationPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearClassificationPattern>(nameof (GearClassificationPattern), new Func<MasterDataReader, MasterDataTable.GearClassificationPattern>(MasterDataTable.GearClassificationPattern.Parse), (Func<MasterDataTable.GearClassificationPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearCombineRecipe> GearCombineRecipe
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearCombineRecipe>(nameof (GearCombineRecipe), new Func<MasterDataReader, MasterDataTable.GearCombineRecipe>(MasterDataTable.GearCombineRecipe.Parse), (Func<MasterDataTable.GearCombineRecipe, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearDisappearanceType> GearDisappearanceType
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearDisappearanceType>(nameof (GearDisappearanceType), new Func<MasterDataReader, MasterDataTable.GearDisappearanceType>(MasterDataTable.GearDisappearanceType.Parse), (Func<MasterDataTable.GearDisappearanceType, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearDrilling> GearDrilling
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearDrilling>(nameof (GearDrilling), new Func<MasterDataReader, MasterDataTable.GearDrilling>(MasterDataTable.GearDrilling.Parse), (Func<MasterDataTable.GearDrilling, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearDrillingExpMythology> GearDrillingExpMythology
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearDrillingExpMythology>(nameof (GearDrillingExpMythology), new Func<MasterDataReader, MasterDataTable.GearDrillingExpMythology>(MasterDataTable.GearDrillingExpMythology.Parse), (Func<MasterDataTable.GearDrillingExpMythology, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearElementRatio> GearElementRatio
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearElementRatio>(nameof (GearElementRatio), new Func<MasterDataReader, MasterDataTable.GearElementRatio>(MasterDataTable.GearElementRatio.Parse), (Func<MasterDataTable.GearElementRatio, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearExpireDate> GearExpireDate
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearExpireDate>(nameof (GearExpireDate), new Func<MasterDataReader, MasterDataTable.GearExpireDate>(MasterDataTable.GearExpireDate.Parse), (Func<MasterDataTable.GearExpireDate, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearExtensionExclusion> GearExtensionExclusion
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearExtensionExclusion>(nameof (GearExtensionExclusion), new Func<MasterDataReader, MasterDataTable.GearExtensionExclusion>(MasterDataTable.GearExtensionExclusion.Parse), (Func<MasterDataTable.GearExtensionExclusion, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearExtensionItem> GearExtensionItem
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearExtensionItem>(nameof (GearExtensionItem), new Func<MasterDataReader, MasterDataTable.GearExtensionItem>(MasterDataTable.GearExtensionItem.Parse), (Func<MasterDataTable.GearExtensionItem, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearExtensionUnity> GearExtensionUnity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearExtensionUnity>(nameof (GearExtensionUnity), new Func<MasterDataReader, MasterDataTable.GearExtensionUnity>(MasterDataTable.GearExtensionUnity.Parse), (Func<MasterDataTable.GearExtensionUnity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearGear> GearGear
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearGear>(nameof (GearGear), new Func<MasterDataReader, MasterDataTable.GearGear>(MasterDataTable.GearGear.Parse), (Func<MasterDataTable.GearGear, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearGearComposeParameter> GearGearComposeParameter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearGearComposeParameter>(nameof (GearGearComposeParameter), new Func<MasterDataReader, MasterDataTable.GearGearComposeParameter>(MasterDataTable.GearGearComposeParameter.Parse), (Func<MasterDataTable.GearGearComposeParameter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearGearDescription> GearGearDescription
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearGearDescription>(nameof (GearGearDescription), new Func<MasterDataReader, MasterDataTable.GearGearDescription>(MasterDataTable.GearGearDescription.Parse), (Func<MasterDataTable.GearGearDescription, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearGearElement> GearGearElement
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearGearElement>(nameof (GearGearElement), new Func<MasterDataReader, MasterDataTable.GearGearElement>(MasterDataTable.GearGearElement.Parse), (Func<MasterDataTable.GearGearElement, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearGearSkill> GearGearSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearGearSkill>(nameof (GearGearSkill), new Func<MasterDataReader, MasterDataTable.GearGearSkill>(MasterDataTable.GearGearSkill.Parse), (Func<MasterDataTable.GearGearSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearKind> GearKind
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearKind>(nameof (GearKind), new Func<MasterDataReader, MasterDataTable.GearKind>(MasterDataTable.GearKind.Parse), (Func<MasterDataTable.GearKind, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearKindCorrelations> GearKindCorrelations
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearKindCorrelations>(nameof (GearKindCorrelations), new Func<MasterDataReader, MasterDataTable.GearKindCorrelations>(MasterDataTable.GearKindCorrelations.Parse), (Func<MasterDataTable.GearKindCorrelations, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearKindIncr> GearKindIncr
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearKindIncr>(nameof (GearKindIncr), new Func<MasterDataReader, MasterDataTable.GearKindIncr>(MasterDataTable.GearKindIncr.Parse), (Func<MasterDataTable.GearKindIncr, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearKindRatio> GearKindRatio
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearKindRatio>(nameof (GearKindRatio), new Func<MasterDataReader, MasterDataTable.GearKindRatio>(MasterDataTable.GearKindRatio.Parse), (Func<MasterDataTable.GearKindRatio, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearMaterialQuestInfo> GearMaterialQuestInfo
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearMaterialQuestInfo>(nameof (GearMaterialQuestInfo), new Func<MasterDataReader, MasterDataTable.GearMaterialQuestInfo>(MasterDataTable.GearMaterialQuestInfo.Parse), (Func<MasterDataTable.GearMaterialQuestInfo, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearModelKind> GearModelKind
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearModelKind>(nameof (GearModelKind), new Func<MasterDataReader, MasterDataTable.GearModelKind>(MasterDataTable.GearModelKind.Parse), (Func<MasterDataTable.GearModelKind, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearRank> GearRank
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearRank>(nameof (GearRank), new Func<MasterDataReader, MasterDataTable.GearRank>(MasterDataTable.GearRank.Parse), (Func<MasterDataTable.GearRank, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearRankExp> GearRankExp
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearRankExp>(nameof (GearRankExp), new Func<MasterDataReader, MasterDataTable.GearRankExp>(MasterDataTable.GearRankExp.Parse), (Func<MasterDataTable.GearRankExp, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearRankIncr> GearRankIncr
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearRankIncr>(nameof (GearRankIncr), new Func<MasterDataReader, MasterDataTable.GearRankIncr>(MasterDataTable.GearRankIncr.Parse), (Func<MasterDataTable.GearRankIncr, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearRarity> GearRarity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearRarity>(nameof (GearRarity), new Func<MasterDataReader, MasterDataTable.GearRarity>(MasterDataTable.GearRarity.Parse), (Func<MasterDataTable.GearRarity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearReisouChaosCreation> GearReisouChaosCreation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearReisouChaosCreation>(nameof (GearReisouChaosCreation), new Func<MasterDataReader, MasterDataTable.GearReisouChaosCreation>(MasterDataTable.GearReisouChaosCreation.Parse), (Func<MasterDataTable.GearReisouChaosCreation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearReisouFusion> GearReisouFusion
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearReisouFusion>(nameof (GearReisouFusion), new Func<MasterDataReader, MasterDataTable.GearReisouFusion>(MasterDataTable.GearReisouFusion.Parse), (Func<MasterDataTable.GearReisouFusion, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearReisouSkill> GearReisouSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearReisouSkill>(nameof (GearReisouSkill), new Func<MasterDataReader, MasterDataTable.GearReisouSkill>(MasterDataTable.GearReisouSkill.Parse), (Func<MasterDataTable.GearReisouSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearReisouSkillWeaponGroup> GearReisouSkillWeaponGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearReisouSkillWeaponGroup>(nameof (GearReisouSkillWeaponGroup), new Func<MasterDataReader, MasterDataTable.GearReisouSkillWeaponGroup>(MasterDataTable.GearReisouSkillWeaponGroup.Parse), (Func<MasterDataTable.GearReisouSkillWeaponGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearSpecialDrillingCost> GearSpecialDrillingCost
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearSpecialDrillingCost>(nameof (GearSpecialDrillingCost), new Func<MasterDataReader, MasterDataTable.GearSpecialDrillingCost>(MasterDataTable.GearSpecialDrillingCost.Parse), (Func<MasterDataTable.GearSpecialDrillingCost, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearSpecificationOfEquipmentUnit> GearSpecificationOfEquipmentUnit
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearSpecificationOfEquipmentUnit>(nameof (GearSpecificationOfEquipmentUnit), new Func<MasterDataReader, MasterDataTable.GearSpecificationOfEquipmentUnit>(MasterDataTable.GearSpecificationOfEquipmentUnit.Parse), (Func<MasterDataTable.GearSpecificationOfEquipmentUnit, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GearValuables> GearValuables
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GearValuables>(nameof (GearValuables), new Func<MasterDataReader, MasterDataTable.GearValuables>(MasterDataTable.GearValuables.Parse), (Func<MasterDataTable.GearValuables, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildApprovalPolicy> GuildApprovalPolicy
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildApprovalPolicy>(nameof (GuildApprovalPolicy), new Func<MasterDataReader, MasterDataTable.GuildApprovalPolicy>(MasterDataTable.GuildApprovalPolicy.Parse), (Func<MasterDataTable.GuildApprovalPolicy, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildAtmosphere> GuildAtmosphere
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildAtmosphere>(nameof (GuildAtmosphere), new Func<MasterDataReader, MasterDataTable.GuildAtmosphere>(MasterDataTable.GuildAtmosphere.Parse), (Func<MasterDataTable.GuildAtmosphere, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildAutoApproval> GuildAutoApproval
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildAutoApproval>(nameof (GuildAutoApproval), new Func<MasterDataReader, MasterDataTable.GuildAutoApproval>(MasterDataTable.GuildAutoApproval.Parse), (Func<MasterDataTable.GuildAutoApproval, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildAutokick> GuildAutokick
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildAutokick>(nameof (GuildAutokick), new Func<MasterDataReader, MasterDataTable.GuildAutokick>(MasterDataTable.GuildAutokick.Parse), (Func<MasterDataTable.GuildAutokick, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildAvailability> GuildAvailability
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildAvailability>(nameof (GuildAvailability), new Func<MasterDataReader, MasterDataTable.GuildAvailability>(MasterDataTable.GuildAvailability.Parse), (Func<MasterDataTable.GuildAvailability, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildBankEvent> GuildBankEvent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildBankEvent>(nameof (GuildBankEvent), new Func<MasterDataReader, MasterDataTable.GuildBankEvent>(MasterDataTable.GuildBankEvent.Parse), (Func<MasterDataTable.GuildBankEvent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildBankHowto> GuildBankHowto
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildBankHowto>(nameof (GuildBankHowto), new Func<MasterDataReader, MasterDataTable.GuildBankHowto>(MasterDataTable.GuildBankHowto.Parse), (Func<MasterDataTable.GuildBankHowto, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildBase> GuildBase
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildBase>(nameof (GuildBase), new Func<MasterDataReader, MasterDataTable.GuildBase>(MasterDataTable.GuildBase.Parse), (Func<MasterDataTable.GuildBase, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildBaseAnimation> GuildBaseAnimation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildBaseAnimation>(nameof (GuildBaseAnimation), new Func<MasterDataReader, MasterDataTable.GuildBaseAnimation>(MasterDataTable.GuildBaseAnimation.Parse), (Func<MasterDataTable.GuildBaseAnimation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildBaseBonus> GuildBaseBonus
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildBaseBonus>(nameof (GuildBaseBonus), new Func<MasterDataReader, MasterDataTable.GuildBaseBonus>(MasterDataTable.GuildBaseBonus.Parse), (Func<MasterDataTable.GuildBaseBonus, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildBasePos> GuildBasePos
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildBasePos>(nameof (GuildBasePos), new Func<MasterDataReader, MasterDataTable.GuildBasePos>(MasterDataTable.GuildBasePos.Parse), (Func<MasterDataTable.GuildBasePos, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildEmblemRarity> GuildEmblemRarity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildEmblemRarity>(nameof (GuildEmblemRarity), new Func<MasterDataReader, MasterDataTable.GuildEmblemRarity>(MasterDataTable.GuildEmblemRarity.Parse), (Func<MasterDataTable.GuildEmblemRarity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildEmblemUnit> GuildEmblemUnit
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildEmblemUnit>(nameof (GuildEmblemUnit), new Func<MasterDataReader, MasterDataTable.GuildEmblemUnit>(MasterDataTable.GuildEmblemUnit.Parse), (Func<MasterDataTable.GuildEmblemUnit, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildGiftEvent> GuildGiftEvent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildGiftEvent>(nameof (GuildGiftEvent), new Func<MasterDataReader, MasterDataTable.GuildGiftEvent>(MasterDataTable.GuildGiftEvent.Parse), (Func<MasterDataTable.GuildGiftEvent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildImagePattern> GuildImagePattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildImagePattern>(nameof (GuildImagePattern), new Func<MasterDataReader, MasterDataTable.GuildImagePattern>(MasterDataTable.GuildImagePattern.Parse), (Func<MasterDataTable.GuildImagePattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildMission> GuildMission
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildMission>(nameof (GuildMission), new Func<MasterDataReader, MasterDataTable.GuildMission>(MasterDataTable.GuildMission.Parse), (Func<MasterDataTable.GuildMission, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaid> GuildRaid
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaid>(nameof (GuildRaid), new Func<MasterDataReader, MasterDataTable.GuildRaid>(MasterDataTable.GuildRaid.Parse), (Func<MasterDataTable.GuildRaid, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidDamageReward> GuildRaidDamageReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidDamageReward>(nameof (GuildRaidDamageReward), new Func<MasterDataReader, MasterDataTable.GuildRaidDamageReward>(MasterDataTable.GuildRaidDamageReward.Parse), (Func<MasterDataTable.GuildRaidDamageReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidDamageRewardSet> GuildRaidDamageRewardSet
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidDamageRewardSet>(nameof (GuildRaidDamageRewardSet), new Func<MasterDataReader, MasterDataTable.GuildRaidDamageRewardSet>(MasterDataTable.GuildRaidDamageRewardSet.Parse), (Func<MasterDataTable.GuildRaidDamageRewardSet, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidEndless> GuildRaidEndless
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidEndless>(nameof (GuildRaidEndless), new Func<MasterDataReader, MasterDataTable.GuildRaidEndless>(MasterDataTable.GuildRaidEndless.Parse), (Func<MasterDataTable.GuildRaidEndless, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidEndlessKillReward> GuildRaidEndlessKillReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidEndlessKillReward>(nameof (GuildRaidEndlessKillReward), new Func<MasterDataReader, MasterDataTable.GuildRaidEndlessKillReward>(MasterDataTable.GuildRaidEndlessKillReward.Parse), (Func<MasterDataTable.GuildRaidEndlessKillReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidGuildDamageRankingReward> GuildRaidGuildDamageRankingReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidGuildDamageRankingReward>(nameof (GuildRaidGuildDamageRankingReward), new Func<MasterDataReader, MasterDataTable.GuildRaidGuildDamageRankingReward>(MasterDataTable.GuildRaidGuildDamageRankingReward.Parse), (Func<MasterDataTable.GuildRaidGuildDamageRankingReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidGuildDamageRankingRewardExtra> GuildRaidGuildDamageRankingRewardExtra
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidGuildDamageRankingRewardExtra>(nameof (GuildRaidGuildDamageRankingRewardExtra), new Func<MasterDataReader, MasterDataTable.GuildRaidGuildDamageRankingRewardExtra>(MasterDataTable.GuildRaidGuildDamageRankingRewardExtra.Parse), (Func<MasterDataTable.GuildRaidGuildDamageRankingRewardExtra, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidHowto> GuildRaidHowto
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidHowto>(nameof (GuildRaidHowto), new Func<MasterDataReader, MasterDataTable.GuildRaidHowto>(MasterDataTable.GuildRaidHowto.Parse), (Func<MasterDataTable.GuildRaidHowto, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidKillReward> GuildRaidKillReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidKillReward>(nameof (GuildRaidKillReward), new Func<MasterDataReader, MasterDataTable.GuildRaidKillReward>(MasterDataTable.GuildRaidKillReward.Parse), (Func<MasterDataTable.GuildRaidKillReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidKillRewardSet> GuildRaidKillRewardSet
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidKillRewardSet>(nameof (GuildRaidKillRewardSet), new Func<MasterDataReader, MasterDataTable.GuildRaidKillRewardSet>(MasterDataTable.GuildRaidKillRewardSet.Parse), (Func<MasterDataTable.GuildRaidKillRewardSet, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidPeriod> GuildRaidPeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidPeriod>(nameof (GuildRaidPeriod), new Func<MasterDataReader, MasterDataTable.GuildRaidPeriod>(MasterDataTable.GuildRaidPeriod.Parse), (Func<MasterDataTable.GuildRaidPeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidPersonalDamageRankingReward> GuildRaidPersonalDamageRankingReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidPersonalDamageRankingReward>(nameof (GuildRaidPersonalDamageRankingReward), new Func<MasterDataReader, MasterDataTable.GuildRaidPersonalDamageRankingReward>(MasterDataTable.GuildRaidPersonalDamageRankingReward.Parse), (Func<MasterDataTable.GuildRaidPersonalDamageRankingReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidRankingRewardCondition> GuildRaidRankingRewardCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidRankingRewardCondition>(nameof (GuildRaidRankingRewardCondition), new Func<MasterDataReader, MasterDataTable.GuildRaidRankingRewardCondition>(MasterDataTable.GuildRaidRankingRewardCondition.Parse), (Func<MasterDataTable.GuildRaidRankingRewardCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRaidSettings> GuildRaidSettings
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRaidSettings>(nameof (GuildRaidSettings), new Func<MasterDataReader, MasterDataTable.GuildRaidSettings>(MasterDataTable.GuildRaidSettings.Parse), (Func<MasterDataTable.GuildRaidSettings, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildRoleName> GuildRoleName
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildRoleName>(nameof (GuildRoleName), new Func<MasterDataReader, MasterDataTable.GuildRoleName>(MasterDataTable.GuildRoleName.Parse), (Func<MasterDataTable.GuildRoleName, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildSetting> GuildSetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildSetting>(nameof (GuildSetting), new Func<MasterDataReader, MasterDataTable.GuildSetting>(MasterDataTable.GuildSetting.Parse), (Func<MasterDataTable.GuildSetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildStamp> GuildStamp
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildStamp>(nameof (GuildStamp), new Func<MasterDataReader, MasterDataTable.GuildStamp>(MasterDataTable.GuildStamp.Parse), (Func<MasterDataTable.GuildStamp, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GuildStampGroup> GuildStampGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GuildStampGroup>(nameof (GuildStampGroup), new Func<MasterDataReader, MasterDataTable.GuildStampGroup>(MasterDataTable.GuildStampGroup.Parse), (Func<MasterDataTable.GuildStampGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GvgPeriod> GvgPeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GvgPeriod>(nameof (GvgPeriod), new Func<MasterDataReader, MasterDataTable.GvgPeriod>(MasterDataTable.GvgPeriod.Parse), (Func<MasterDataTable.GvgPeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GvgRule> GvgRule
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GvgRule>(nameof (GvgRule), new Func<MasterDataReader, MasterDataTable.GvgRule>(MasterDataTable.GvgRule.Parse), (Func<MasterDataTable.GvgRule, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GvgSettings> GvgSettings
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GvgSettings>(nameof (GvgSettings), new Func<MasterDataReader, MasterDataTable.GvgSettings>(MasterDataTable.GvgSettings.Parse), (Func<MasterDataTable.GvgSettings, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GvgStageFormation> GvgStageFormation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GvgStageFormation>(nameof (GvgStageFormation), new Func<MasterDataReader, MasterDataTable.GvgStageFormation>(MasterDataTable.GvgStageFormation.Parse), (Func<MasterDataTable.GvgStageFormation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.GvgStarCondition> GvgStarCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.GvgStarCondition>(nameof (GvgStarCondition), new Func<MasterDataReader, MasterDataTable.GvgStarCondition>(MasterDataTable.GvgStarCondition.Parse), (Func<MasterDataTable.GvgStarCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.HelpCategory> HelpCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.HelpCategory>(nameof (HelpCategory), new Func<MasterDataReader, MasterDataTable.HelpCategory>(MasterDataTable.HelpCategory.Parse), (Func<MasterDataTable.HelpCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.HelpHelp> HelpHelp
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.HelpHelp>(nameof (HelpHelp), new Func<MasterDataReader, MasterDataTable.HelpHelp>(MasterDataTable.HelpHelp.Parse), (Func<MasterDataTable.HelpHelp, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.HotdealPack> HotdealPack
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.HotdealPack>(nameof (HotdealPack), new Func<MasterDataReader, MasterDataTable.HotdealPack>(MasterDataTable.HotdealPack.Parse), (Func<MasterDataTable.HotdealPack, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.IgnoreGear> IgnoreGear
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.IgnoreGear>(nameof (IgnoreGear), new Func<MasterDataReader, MasterDataTable.IgnoreGear>(MasterDataTable.IgnoreGear.Parse), (Func<MasterDataTable.IgnoreGear, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.IgnoreOverkillers> IgnoreOverkillers
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.IgnoreOverkillers>(nameof (IgnoreOverkillers), new Func<MasterDataReader, MasterDataTable.IgnoreOverkillers>(MasterDataTable.IgnoreOverkillers.Parse), (Func<MasterDataTable.IgnoreOverkillers, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.InformationCategory> InformationCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.InformationCategory>(nameof (InformationCategory), new Func<MasterDataReader, MasterDataTable.InformationCategory>(MasterDataTable.InformationCategory.Parse), (Func<MasterDataTable.InformationCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.InformationInformation> InformationInformation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.InformationInformation>(nameof (InformationInformation), new Func<MasterDataReader, MasterDataTable.InformationInformation>(MasterDataTable.InformationInformation.Parse), (Func<MasterDataTable.InformationInformation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.InformationSubCategory> InformationSubCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.InformationSubCategory>(nameof (InformationSubCategory), new Func<MasterDataReader, MasterDataTable.InformationSubCategory>(MasterDataTable.InformationSubCategory.Parse), (Func<MasterDataTable.InformationSubCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.InitimateBreakthrough> InitimateBreakthrough
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.InitimateBreakthrough>(nameof (InitimateBreakthrough), new Func<MasterDataReader, MasterDataTable.InitimateBreakthrough>(MasterDataTable.InitimateBreakthrough.Parse), (Func<MasterDataTable.InitimateBreakthrough, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.InitimateLevel> InitimateLevel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.InitimateLevel>(nameof (InitimateLevel), new Func<MasterDataReader, MasterDataTable.InitimateLevel>(MasterDataTable.InitimateLevel.Parse), (Func<MasterDataTable.InitimateLevel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.IntimateDuelSupport> IntimateDuelSupport
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.IntimateDuelSupport>(nameof (IntimateDuelSupport), new Func<MasterDataReader, MasterDataTable.IntimateDuelSupport>(MasterDataTable.IntimateDuelSupport.Parse), (Func<MasterDataTable.IntimateDuelSupport, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.InvitationInvitation> InvitationInvitation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.InvitationInvitation>(nameof (InvitationInvitation), new Func<MasterDataReader, MasterDataTable.InvitationInvitation>(MasterDataTable.InvitationInvitation.Parse), (Func<MasterDataTable.InvitationInvitation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.JobChangeMaterials> JobChangeMaterials
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.JobChangeMaterials>(nameof (JobChangeMaterials), new Func<MasterDataReader, MasterDataTable.JobChangeMaterials>(MasterDataTable.JobChangeMaterials.Parse), (Func<MasterDataTable.JobChangeMaterials, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.JobChangePatterns> JobChangePatterns
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.JobChangePatterns>(nameof (JobChangePatterns), new Func<MasterDataReader, MasterDataTable.JobChangePatterns>(MasterDataTable.JobChangePatterns.Parse), (Func<MasterDataTable.JobChangePatterns, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.JobCharacteristics> JobCharacteristics
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.JobCharacteristics>(nameof (JobCharacteristics), new Func<MasterDataReader, MasterDataTable.JobCharacteristics>(MasterDataTable.JobCharacteristics.Parse), (Func<MasterDataTable.JobCharacteristics, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.JobCharacteristicsLevelupPattern> JobCharacteristicsLevelupPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.JobCharacteristicsLevelupPattern>(nameof (JobCharacteristicsLevelupPattern), new Func<MasterDataReader, MasterDataTable.JobCharacteristicsLevelupPattern>(MasterDataTable.JobCharacteristicsLevelupPattern.Parse), (Func<MasterDataTable.JobCharacteristicsLevelupPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.JobMaterialGroup> JobMaterialGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.JobMaterialGroup>(nameof (JobMaterialGroup), new Func<MasterDataReader, MasterDataTable.JobMaterialGroup>(MasterDataTable.JobMaterialGroup.Parse), (Func<MasterDataTable.JobMaterialGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.JobMaterialUsed> JobMaterialUsed
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.JobMaterialUsed>(nameof (JobMaterialUsed), new Func<MasterDataReader, MasterDataTable.JobMaterialUsed>(MasterDataTable.JobMaterialUsed.Parse), (Func<MasterDataTable.JobMaterialUsed, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.LoginbonusLoginbonus> LoginbonusLoginbonus
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.LoginbonusLoginbonus>(nameof (LoginbonusLoginbonus), new Func<MasterDataReader, MasterDataTable.LoginbonusLoginbonus>(MasterDataTable.LoginbonusLoginbonus.Parse), (Func<MasterDataTable.LoginbonusLoginbonus, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.LoginbonusReward> LoginbonusReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.LoginbonusReward>(nameof (LoginbonusReward), new Func<MasterDataReader, MasterDataTable.LoginbonusReward>(MasterDataTable.LoginbonusReward.Parse), (Func<MasterDataTable.LoginbonusReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.MapEditFacilityShaderSetting> MapEditFacilityShaderSetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.MapEditFacilityShaderSetting>(nameof (MapEditFacilityShaderSetting), new Func<MasterDataReader, MasterDataTable.MapEditFacilityShaderSetting>(MasterDataTable.MapEditFacilityShaderSetting.Parse), (Func<MasterDataTable.MapEditFacilityShaderSetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.MapFacility> MapFacility
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.MapFacility>(nameof (MapFacility), new Func<MasterDataReader, MasterDataTable.MapFacility>(MasterDataTable.MapFacility.Parse), (Func<MasterDataTable.MapFacility, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.MapTown> MapTown
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.MapTown>(nameof (MapTown), new Func<MasterDataReader, MasterDataTable.MapTown>(MasterDataTable.MapTown.Parse), (Func<MasterDataTable.MapTown, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.MaterialXLevelExp> MaterialXLevelExp
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.MaterialXLevelExp>(nameof (MaterialXLevelExp), new Func<MasterDataReader, MasterDataTable.MaterialXLevelExp>(MasterDataTable.MaterialXLevelExp.Parse), (Func<MasterDataTable.MaterialXLevelExp, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.Music> Music
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.Music>(nameof (Music), new Func<MasterDataReader, MasterDataTable.Music>(MasterDataTable.Music.Parse), (Func<MasterDataTable.Music, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.OverkillersGroup> OverkillersGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.OverkillersGroup>(nameof (OverkillersGroup), new Func<MasterDataReader, MasterDataTable.OverkillersGroup>(MasterDataTable.OverkillersGroup.Parse), (Func<MasterDataTable.OverkillersGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.OverkillersMaterial> OverkillersMaterial
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.OverkillersMaterial>(nameof (OverkillersMaterial), new Func<MasterDataReader, MasterDataTable.OverkillersMaterial>(MasterDataTable.OverkillersMaterial.Parse), (Func<MasterDataTable.OverkillersMaterial, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.OverkillersParameter> OverkillersParameter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.OverkillersParameter>(nameof (OverkillersParameter), new Func<MasterDataReader, MasterDataTable.OverkillersParameter>(MasterDataTable.OverkillersParameter.Parse), (Func<MasterDataTable.OverkillersParameter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.OverkillersSkillRelease> OverkillersSkillRelease
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.OverkillersSkillRelease>(nameof (OverkillersSkillRelease), new Func<MasterDataReader, MasterDataTable.OverkillersSkillRelease>(MasterDataTable.OverkillersSkillRelease.Parse), (Func<MasterDataTable.OverkillersSkillRelease, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.OverkillersSlotRelease> OverkillersSlotRelease
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.OverkillersSlotRelease>(nameof (OverkillersSlotRelease), new Func<MasterDataReader, MasterDataTable.OverkillersSlotRelease>(MasterDataTable.OverkillersSlotRelease.Parse), (Func<MasterDataTable.OverkillersSlotRelease, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PeriodBackground> PeriodBackground
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PeriodBackground>(nameof (PeriodBackground), new Func<MasterDataReader, MasterDataTable.PeriodBackground>(MasterDataTable.PeriodBackground.Parse), (Func<MasterDataTable.PeriodBackground, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PlayerLevelUpStatus> PlayerLevelUpStatus
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PlayerLevelUpStatus>(nameof (PlayerLevelUpStatus), new Func<MasterDataReader, MasterDataTable.PlayerLevelUpStatus>(MasterDataTable.PlayerLevelUpStatus.Parse), (Func<MasterDataTable.PlayerLevelUpStatus, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PointReward> PointReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PointReward>(nameof (PointReward), new Func<MasterDataReader, MasterDataTable.PointReward>(MasterDataTable.PointReward.Parse), (Func<MasterDataTable.PointReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PointRewardBox> PointRewardBox
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PointRewardBox>(nameof (PointRewardBox), new Func<MasterDataReader, MasterDataTable.PointRewardBox>(MasterDataTable.PointRewardBox.Parse), (Func<MasterDataTable.PointRewardBox, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PunitiveExpeditionEventGuildReward> PunitiveExpeditionEventGuildReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PunitiveExpeditionEventGuildReward>(nameof (PunitiveExpeditionEventGuildReward), new Func<MasterDataReader, MasterDataTable.PunitiveExpeditionEventGuildReward>(MasterDataTable.PunitiveExpeditionEventGuildReward.Parse), (Func<MasterDataTable.PunitiveExpeditionEventGuildReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PunitiveExpeditionEventReward> PunitiveExpeditionEventReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PunitiveExpeditionEventReward>(nameof (PunitiveExpeditionEventReward), new Func<MasterDataReader, MasterDataTable.PunitiveExpeditionEventReward>(MasterDataTable.PunitiveExpeditionEventReward.Parse), (Func<MasterDataTable.PunitiveExpeditionEventReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpBonus> PvpBonus
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpBonus>(nameof (PvpBonus), new Func<MasterDataReader, MasterDataTable.PvpBonus>(MasterDataTable.PvpBonus.Parse), (Func<MasterDataTable.PvpBonus, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpClassKind> PvpClassKind
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpClassKind>(nameof (PvpClassKind), new Func<MasterDataReader, MasterDataTable.PvpClassKind>(MasterDataTable.PvpClassKind.Parse), (Func<MasterDataTable.PvpClassKind, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpClassRankingReward> PvpClassRankingReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpClassRankingReward>(nameof (PvpClassRankingReward), new Func<MasterDataReader, MasterDataTable.PvpClassRankingReward>(MasterDataTable.PvpClassRankingReward.Parse), (Func<MasterDataTable.PvpClassRankingReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpClassReward> PvpClassReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpClassReward>(nameof (PvpClassReward), new Func<MasterDataReader, MasterDataTable.PvpClassReward>(MasterDataTable.PvpClassReward.Parse), (Func<MasterDataTable.PvpClassReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpMatchingType> PvpMatchingType
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpMatchingType>(nameof (PvpMatchingType), new Func<MasterDataReader, MasterDataTable.PvpMatchingType>(MasterDataTable.PvpMatchingType.Parse), (Func<MasterDataTable.PvpMatchingType, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpRankingCondition> PvpRankingCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpRankingCondition>(nameof (PvpRankingCondition), new Func<MasterDataReader, MasterDataTable.PvpRankingCondition>(MasterDataTable.PvpRankingCondition.Parse), (Func<MasterDataTable.PvpRankingCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpRankingKind> PvpRankingKind
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpRankingKind>(nameof (PvpRankingKind), new Func<MasterDataReader, MasterDataTable.PvpRankingKind>(MasterDataTable.PvpRankingKind.Parse), (Func<MasterDataTable.PvpRankingKind, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpRulePeriod> PvpRulePeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpRulePeriod>(nameof (PvpRulePeriod), new Func<MasterDataReader, MasterDataTable.PvpRulePeriod>(MasterDataTable.PvpRulePeriod.Parse), (Func<MasterDataTable.PvpRulePeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpSettings> PvpSettings
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpSettings>(nameof (PvpSettings), new Func<MasterDataReader, MasterDataTable.PvpSettings>(MasterDataTable.PvpSettings.Parse), (Func<MasterDataTable.PvpSettings, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpStageFormation> PvpStageFormation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpStageFormation>(nameof (PvpStageFormation), new Func<MasterDataReader, MasterDataTable.PvpStageFormation>(MasterDataTable.PvpStageFormation.Parse), (Func<MasterDataTable.PvpStageFormation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.PvpVictoryEffect> PvpVictoryEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.PvpVictoryEffect>(nameof (PvpVictoryEffect), new Func<MasterDataReader, MasterDataTable.PvpVictoryEffect>(MasterDataTable.PvpVictoryEffect.Parse), (Func<MasterDataTable.PvpVictoryEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCharacterDisplayCondition> QuestCharacterDisplayCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCharacterDisplayCondition>(nameof (QuestCharacterDisplayCondition), new Func<MasterDataReader, MasterDataTable.QuestCharacterDisplayCondition>(MasterDataTable.QuestCharacterDisplayCondition.Parse), (Func<MasterDataTable.QuestCharacterDisplayCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCharacterLimitation> QuestCharacterLimitation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCharacterLimitation>(nameof (QuestCharacterLimitation), new Func<MasterDataReader, MasterDataTable.QuestCharacterLimitation>(MasterDataTable.QuestCharacterLimitation.Parse), (Func<MasterDataTable.QuestCharacterLimitation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCharacterLimitationLabel> QuestCharacterLimitationLabel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCharacterLimitationLabel>(nameof (QuestCharacterLimitationLabel), new Func<MasterDataReader, MasterDataTable.QuestCharacterLimitationLabel>(MasterDataTable.QuestCharacterLimitationLabel.Parse), (Func<MasterDataTable.QuestCharacterLimitationLabel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCharacterM> QuestCharacterM
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCharacterM>(nameof (QuestCharacterM), new Func<MasterDataReader, MasterDataTable.QuestCharacterM>(MasterDataTable.QuestCharacterM.Parse), (Func<MasterDataTable.QuestCharacterM, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCharacterMReleaseCondition> QuestCharacterMReleaseCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCharacterMReleaseCondition>(nameof (QuestCharacterMReleaseCondition), new Func<MasterDataReader, MasterDataTable.QuestCharacterMReleaseCondition>(MasterDataTable.QuestCharacterMReleaseCondition.Parse), (Func<MasterDataTable.QuestCharacterMReleaseCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCharacterReleaseCondition> QuestCharacterReleaseCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCharacterReleaseCondition>(nameof (QuestCharacterReleaseCondition), new Func<MasterDataReader, MasterDataTable.QuestCharacterReleaseCondition>(MasterDataTable.QuestCharacterReleaseCondition.Parse), (Func<MasterDataTable.QuestCharacterReleaseCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCharacterS> QuestCharacterS
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCharacterS>(nameof (QuestCharacterS), new Func<MasterDataReader, MasterDataTable.QuestCharacterS>(MasterDataTable.QuestCharacterS.Parse), (Func<MasterDataTable.QuestCharacterS, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCommonBackground> QuestCommonBackground
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCommonBackground>(nameof (QuestCommonBackground), new Func<MasterDataReader, MasterDataTable.QuestCommonBackground>(MasterDataTable.QuestCommonBackground.Parse), (Func<MasterDataTable.QuestCommonBackground, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCommonChapterBG> QuestCommonChapterBG
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCommonChapterBG>(nameof (QuestCommonChapterBG), new Func<MasterDataReader, MasterDataTable.QuestCommonChapterBG>(MasterDataTable.QuestCommonChapterBG.Parse), (Func<MasterDataTable.QuestCommonChapterBG, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCommonDrop> QuestCommonDrop
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCommonDrop>(nameof (QuestCommonDrop), new Func<MasterDataReader, MasterDataTable.QuestCommonDrop>(MasterDataTable.QuestCommonDrop.Parse), (Func<MasterDataTable.QuestCommonDrop, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCommonJogDecoration> QuestCommonJogDecoration
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCommonJogDecoration>(nameof (QuestCommonJogDecoration), new Func<MasterDataReader, MasterDataTable.QuestCommonJogDecoration>(MasterDataTable.QuestCommonJogDecoration.Parse), (Func<MasterDataTable.QuestCommonJogDecoration, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestCommonSpecialColor> QuestCommonSpecialColor
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestCommonSpecialColor>(nameof (QuestCommonSpecialColor), new Func<MasterDataReader, MasterDataTable.QuestCommonSpecialColor>(MasterDataTable.QuestCommonSpecialColor.Parse), (Func<MasterDataTable.QuestCommonSpecialColor, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraCategory> QuestExtraCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraCategory>(nameof (QuestExtraCategory), new Func<MasterDataReader, MasterDataTable.QuestExtraCategory>(MasterDataTable.QuestExtraCategory.Parse), (Func<MasterDataTable.QuestExtraCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraDescription> QuestExtraDescription
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraDescription>(nameof (QuestExtraDescription), new Func<MasterDataReader, MasterDataTable.QuestExtraDescription>(MasterDataTable.QuestExtraDescription.Parse), (Func<MasterDataTable.QuestExtraDescription, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraEntryConditions> QuestExtraEntryConditions
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraEntryConditions>(nameof (QuestExtraEntryConditions), new Func<MasterDataReader, MasterDataTable.QuestExtraEntryConditions>(MasterDataTable.QuestExtraEntryConditions.Parse), (Func<MasterDataTable.QuestExtraEntryConditions, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraL> QuestExtraL
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraL>(nameof (QuestExtraL), new Func<MasterDataReader, MasterDataTable.QuestExtraL>(MasterDataTable.QuestExtraL.Parse), (Func<MasterDataTable.QuestExtraL, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraLL> QuestExtraLL
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraLL>(nameof (QuestExtraLL), new Func<MasterDataReader, MasterDataTable.QuestExtraLL>(MasterDataTable.QuestExtraLL.Parse), (Func<MasterDataTable.QuestExtraLL, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraLimitation> QuestExtraLimitation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraLimitation>(nameof (QuestExtraLimitation), new Func<MasterDataReader, MasterDataTable.QuestExtraLimitation>(MasterDataTable.QuestExtraLimitation.Parse), (Func<MasterDataTable.QuestExtraLimitation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraLimitationLabel> QuestExtraLimitationLabel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraLimitationLabel>(nameof (QuestExtraLimitationLabel), new Func<MasterDataReader, MasterDataTable.QuestExtraLimitationLabel>(MasterDataTable.QuestExtraLimitationLabel.Parse), (Func<MasterDataTable.QuestExtraLimitationLabel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraM> QuestExtraM
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraM>(nameof (QuestExtraM), new Func<MasterDataReader, MasterDataTable.QuestExtraM>(MasterDataTable.QuestExtraM.Parse), (Func<MasterDataTable.QuestExtraM, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraMission> QuestExtraMission
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraMission>(nameof (QuestExtraMission), new Func<MasterDataReader, MasterDataTable.QuestExtraMission>(MasterDataTable.QuestExtraMission.Parse), (Func<MasterDataTable.QuestExtraMission, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraReleaseConditionsPlayer> QuestExtraReleaseConditionsPlayer
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraReleaseConditionsPlayer>(nameof (QuestExtraReleaseConditionsPlayer), new Func<MasterDataReader, MasterDataTable.QuestExtraReleaseConditionsPlayer>(MasterDataTable.QuestExtraReleaseConditionsPlayer.Parse), (Func<MasterDataTable.QuestExtraReleaseConditionsPlayer, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraS> QuestExtraS
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraS>(nameof (QuestExtraS), new Func<MasterDataReader, MasterDataTable.QuestExtraS>(MasterDataTable.QuestExtraS.Parse), (Func<MasterDataTable.QuestExtraS, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraScoreAchivementReward> QuestExtraScoreAchivementReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraScoreAchivementReward>(nameof (QuestExtraScoreAchivementReward), new Func<MasterDataReader, MasterDataTable.QuestExtraScoreAchivementReward>(MasterDataTable.QuestExtraScoreAchivementReward.Parse), (Func<MasterDataTable.QuestExtraScoreAchivementReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraScoreRankingReward> QuestExtraScoreRankingReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraScoreRankingReward>(nameof (QuestExtraScoreRankingReward), new Func<MasterDataReader, MasterDataTable.QuestExtraScoreRankingReward>(MasterDataTable.QuestExtraScoreRankingReward.Parse), (Func<MasterDataTable.QuestExtraScoreRankingReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestExtraTotalScoreReward> QuestExtraTotalScoreReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestExtraTotalScoreReward>(nameof (QuestExtraTotalScoreReward), new Func<MasterDataReader, MasterDataTable.QuestExtraTotalScoreReward>(MasterDataTable.QuestExtraTotalScoreReward.Parse), (Func<MasterDataTable.QuestExtraTotalScoreReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestHarmonyDisplayCondition> QuestHarmonyDisplayCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestHarmonyDisplayCondition>(nameof (QuestHarmonyDisplayCondition), new Func<MasterDataReader, MasterDataTable.QuestHarmonyDisplayCondition>(MasterDataTable.QuestHarmonyDisplayCondition.Parse), (Func<MasterDataTable.QuestHarmonyDisplayCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestHarmonyLimitation> QuestHarmonyLimitation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestHarmonyLimitation>(nameof (QuestHarmonyLimitation), new Func<MasterDataReader, MasterDataTable.QuestHarmonyLimitation>(MasterDataTable.QuestHarmonyLimitation.Parse), (Func<MasterDataTable.QuestHarmonyLimitation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestHarmonyLimitationLabel> QuestHarmonyLimitationLabel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestHarmonyLimitationLabel>(nameof (QuestHarmonyLimitationLabel), new Func<MasterDataReader, MasterDataTable.QuestHarmonyLimitationLabel>(MasterDataTable.QuestHarmonyLimitationLabel.Parse), (Func<MasterDataTable.QuestHarmonyLimitationLabel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestHarmonyM> QuestHarmonyM
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestHarmonyM>(nameof (QuestHarmonyM), new Func<MasterDataReader, MasterDataTable.QuestHarmonyM>(MasterDataTable.QuestHarmonyM.Parse), (Func<MasterDataTable.QuestHarmonyM, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestHarmonyReleaseCondition> QuestHarmonyReleaseCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestHarmonyReleaseCondition>(nameof (QuestHarmonyReleaseCondition), new Func<MasterDataReader, MasterDataTable.QuestHarmonyReleaseCondition>(MasterDataTable.QuestHarmonyReleaseCondition.Parse), (Func<MasterDataTable.QuestHarmonyReleaseCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestHarmonyS> QuestHarmonyS
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestHarmonyS>(nameof (QuestHarmonyS), new Func<MasterDataReader, MasterDataTable.QuestHarmonyS>(MasterDataTable.QuestHarmonyS.Parse), (Func<MasterDataTable.QuestHarmonyS, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestMoviePath> QuestMoviePath
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestMoviePath>(nameof (QuestMoviePath), new Func<MasterDataReader, MasterDataTable.QuestMoviePath>(MasterDataTable.QuestMoviePath.Parse), (Func<MasterDataTable.QuestMoviePath, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestMovieQuest> QuestMovieQuest
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestMovieQuest>(nameof (QuestMovieQuest), new Func<MasterDataReader, MasterDataTable.QuestMovieQuest>(MasterDataTable.QuestMovieQuest.Parse), (Func<MasterDataTable.QuestMovieQuest, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaClearMessage> QuestSeaClearMessage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaClearMessage>(nameof (QuestSeaClearMessage), new Func<MasterDataReader, MasterDataTable.QuestSeaClearMessage>(MasterDataTable.QuestSeaClearMessage.Parse), (Func<MasterDataTable.QuestSeaClearMessage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaL> QuestSeaL
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaL>(nameof (QuestSeaL), new Func<MasterDataReader, MasterDataTable.QuestSeaL>(MasterDataTable.QuestSeaL.Parse), (Func<MasterDataTable.QuestSeaL, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaLimitation> QuestSeaLimitation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaLimitation>(nameof (QuestSeaLimitation), new Func<MasterDataReader, MasterDataTable.QuestSeaLimitation>(MasterDataTable.QuestSeaLimitation.Parse), (Func<MasterDataTable.QuestSeaLimitation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaLimitationLabel> QuestSeaLimitationLabel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaLimitationLabel>(nameof (QuestSeaLimitationLabel), new Func<MasterDataReader, MasterDataTable.QuestSeaLimitationLabel>(MasterDataTable.QuestSeaLimitationLabel.Parse), (Func<MasterDataTable.QuestSeaLimitationLabel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaM> QuestSeaM
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaM>(nameof (QuestSeaM), new Func<MasterDataReader, MasterDataTable.QuestSeaM>(MasterDataTable.QuestSeaM.Parse), (Func<MasterDataTable.QuestSeaM, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaMission> QuestSeaMission
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaMission>(nameof (QuestSeaMission), new Func<MasterDataReader, MasterDataTable.QuestSeaMission>(MasterDataTable.QuestSeaMission.Parse), (Func<MasterDataTable.QuestSeaMission, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaMissionReward> QuestSeaMissionReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaMissionReward>(nameof (QuestSeaMissionReward), new Func<MasterDataReader, MasterDataTable.QuestSeaMissionReward>(MasterDataTable.QuestSeaMissionReward.Parse), (Func<MasterDataTable.QuestSeaMissionReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaS> QuestSeaS
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaS>(nameof (QuestSeaS), new Func<MasterDataReader, MasterDataTable.QuestSeaS>(MasterDataTable.QuestSeaS.Parse), (Func<MasterDataTable.QuestSeaS, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestSeaXL> QuestSeaXL
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestSeaXL>(nameof (QuestSeaXL), new Func<MasterDataReader, MasterDataTable.QuestSeaXL>(MasterDataTable.QuestSeaXL.Parse), (Func<MasterDataTable.QuestSeaXL, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryClearMessage> QuestStoryClearMessage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryClearMessage>(nameof (QuestStoryClearMessage), new Func<MasterDataReader, MasterDataTable.QuestStoryClearMessage>(MasterDataTable.QuestStoryClearMessage.Parse), (Func<MasterDataTable.QuestStoryClearMessage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryL> QuestStoryL
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryL>(nameof (QuestStoryL), new Func<MasterDataReader, MasterDataTable.QuestStoryL>(MasterDataTable.QuestStoryL.Parse), (Func<MasterDataTable.QuestStoryL, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryLimitation> QuestStoryLimitation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryLimitation>(nameof (QuestStoryLimitation), new Func<MasterDataReader, MasterDataTable.QuestStoryLimitation>(MasterDataTable.QuestStoryLimitation.Parse), (Func<MasterDataTable.QuestStoryLimitation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryLimitationLabel> QuestStoryLimitationLabel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryLimitationLabel>(nameof (QuestStoryLimitationLabel), new Func<MasterDataReader, MasterDataTable.QuestStoryLimitationLabel>(MasterDataTable.QuestStoryLimitationLabel.Parse), (Func<MasterDataTable.QuestStoryLimitationLabel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryM> QuestStoryM
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryM>(nameof (QuestStoryM), new Func<MasterDataReader, MasterDataTable.QuestStoryM>(MasterDataTable.QuestStoryM.Parse), (Func<MasterDataTable.QuestStoryM, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryMission> QuestStoryMission
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryMission>(nameof (QuestStoryMission), new Func<MasterDataReader, MasterDataTable.QuestStoryMission>(MasterDataTable.QuestStoryMission.Parse), (Func<MasterDataTable.QuestStoryMission, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryMissionReward> QuestStoryMissionReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryMissionReward>(nameof (QuestStoryMissionReward), new Func<MasterDataReader, MasterDataTable.QuestStoryMissionReward>(MasterDataTable.QuestStoryMissionReward.Parse), (Func<MasterDataTable.QuestStoryMissionReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryS> QuestStoryS
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryS>(nameof (QuestStoryS), new Func<MasterDataReader, MasterDataTable.QuestStoryS>(MasterDataTable.QuestStoryS.Parse), (Func<MasterDataTable.QuestStoryS, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestStoryXL> QuestStoryXL
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestStoryXL>(nameof (QuestStoryXL), new Func<MasterDataReader, MasterDataTable.QuestStoryXL>(MasterDataTable.QuestStoryXL.Parse), (Func<MasterDataTable.QuestStoryXL, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestWave> QuestWave
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestWave>(nameof (QuestWave), new Func<MasterDataReader, MasterDataTable.QuestWave>(MasterDataTable.QuestWave.Parse), (Func<MasterDataTable.QuestWave, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestkeyCondition> QuestkeyCondition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestkeyCondition>(nameof (QuestkeyCondition), new Func<MasterDataReader, MasterDataTable.QuestkeyCondition>(MasterDataTable.QuestkeyCondition.Parse), (Func<MasterDataTable.QuestkeyCondition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.QuestkeyQuestkey> QuestkeyQuestkey
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.QuestkeyQuestkey>(nameof (QuestkeyQuestkey), new Func<MasterDataReader, MasterDataTable.QuestkeyQuestkey>(MasterDataTable.QuestkeyQuestkey.Parse), (Func<MasterDataTable.QuestkeyQuestkey, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.RaidPlaybackStory> RaidPlaybackStory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.RaidPlaybackStory>(nameof (RaidPlaybackStory), new Func<MasterDataReader, MasterDataTable.RaidPlaybackStory>(MasterDataTable.RaidPlaybackStory.Parse), (Func<MasterDataTable.RaidPlaybackStory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.RecoveryItemAPHeal> RecoveryItemAPHeal
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.RecoveryItemAPHeal>(nameof (RecoveryItemAPHeal), new Func<MasterDataReader, MasterDataTable.RecoveryItemAPHeal>(MasterDataTable.RecoveryItemAPHeal.Parse), (Func<MasterDataTable.RecoveryItemAPHeal, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ReisouDrilling> ReisouDrilling
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ReisouDrilling>(nameof (ReisouDrilling), new Func<MasterDataReader, MasterDataTable.ReisouDrilling>(MasterDataTable.ReisouDrilling.Parse), (Func<MasterDataTable.ReisouDrilling, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ReisouRankExp> ReisouRankExp
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ReisouRankExp>(nameof (ReisouRankExp), new Func<MasterDataReader, MasterDataTable.ReisouRankExp>(MasterDataTable.ReisouRankExp.Parse), (Func<MasterDataTable.ReisouRankExp, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ReisouRankIncr> ReisouRankIncr
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ReisouRankIncr>(nameof (ReisouRankIncr), new Func<MasterDataReader, MasterDataTable.ReisouRankIncr>(MasterDataTable.ReisouRankIncr.Parse), (Func<MasterDataTable.ReisouRankIncr, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ReviewReward> ReviewReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ReviewReward>(nameof (ReviewReward), new Func<MasterDataReader, MasterDataTable.ReviewReward>(MasterDataTable.ReviewReward.Parse), (Func<MasterDataTable.ReviewReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.RouletteR001FreeAnimationPattern> RouletteR001FreeAnimationPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.RouletteR001FreeAnimationPattern>(nameof (RouletteR001FreeAnimationPattern), new Func<MasterDataReader, MasterDataTable.RouletteR001FreeAnimationPattern>(MasterDataTable.RouletteR001FreeAnimationPattern.Parse), (Func<MasterDataTable.RouletteR001FreeAnimationPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.RouletteR001FreeDeckEntity> RouletteR001FreeDeckEntity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.RouletteR001FreeDeckEntity>(nameof (RouletteR001FreeDeckEntity), new Func<MasterDataReader, MasterDataTable.RouletteR001FreeDeckEntity>(MasterDataTable.RouletteR001FreeDeckEntity.Parse), (Func<MasterDataTable.RouletteR001FreeDeckEntity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.RouletteR001FreePeriod> RouletteR001FreePeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.RouletteR001FreePeriod>(nameof (RouletteR001FreePeriod), new Func<MasterDataReader, MasterDataTable.RouletteR001FreePeriod>(MasterDataTable.RouletteR001FreePeriod.Parse), (Func<MasterDataTable.RouletteR001FreePeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.RouletteR001FreeRoulette> RouletteR001FreeRoulette
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.RouletteR001FreeRoulette>(nameof (RouletteR001FreeRoulette), new Func<MasterDataReader, MasterDataTable.RouletteR001FreeRoulette>(MasterDataTable.RouletteR001FreeRoulette.Parse), (Func<MasterDataTable.RouletteR001FreeRoulette, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.RouletteR001FreelDeck> RouletteR001FreelDeck
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.RouletteR001FreelDeck>(nameof (RouletteR001FreelDeck), new Func<MasterDataReader, MasterDataTable.RouletteR001FreelDeck>(MasterDataTable.RouletteR001FreelDeck.Parse), (Func<MasterDataTable.RouletteR001FreelDeck, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ScriptScript> ScriptScript
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ScriptScript>(nameof (ScriptScript), new Func<MasterDataReader, MasterDataTable.ScriptScript>(MasterDataTable.ScriptScript.Parse), (Func<MasterDataTable.ScriptScript, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaAlbum> SeaAlbum
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaAlbum>(nameof (SeaAlbum), new Func<MasterDataReader, MasterDataTable.SeaAlbum>(MasterDataTable.SeaAlbum.Parse), (Func<MasterDataTable.SeaAlbum, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaAlbumPiece> SeaAlbumPiece
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaAlbumPiece>(nameof (SeaAlbumPiece), new Func<MasterDataReader, MasterDataTable.SeaAlbumPiece>(MasterDataTable.SeaAlbumPiece.Parse), (Func<MasterDataTable.SeaAlbumPiece, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaAlbumRewardGroup> SeaAlbumRewardGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaAlbumRewardGroup>(nameof (SeaAlbumRewardGroup), new Func<MasterDataReader, MasterDataTable.SeaAlbumRewardGroup>(MasterDataTable.SeaAlbumRewardGroup.Parse), (Func<MasterDataTable.SeaAlbumRewardGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaDateDateSpot> SeaDateDateSpot
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaDateDateSpot>(nameof (SeaDateDateSpot), new Func<MasterDataReader, MasterDataTable.SeaDateDateSpot>(MasterDataTable.SeaDateDateSpot.Parse), (Func<MasterDataTable.SeaDateDateSpot, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaDateDateSpotDisplaySetting> SeaDateDateSpotDisplaySetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaDateDateSpotDisplaySetting>(nameof (SeaDateDateSpotDisplaySetting), new Func<MasterDataReader, MasterDataTable.SeaDateDateSpotDisplaySetting>(MasterDataTable.SeaDateDateSpotDisplaySetting.Parse), (Func<MasterDataTable.SeaDateDateSpotDisplaySetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaDateHitExpansionLottery> SeaDateHitExpansionLottery
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaDateHitExpansionLottery>(nameof (SeaDateHitExpansionLottery), new Func<MasterDataReader, MasterDataTable.SeaDateHitExpansionLottery>(MasterDataTable.SeaDateHitExpansionLottery.Parse), (Func<MasterDataTable.SeaDateHitExpansionLottery, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaDateProhibition> SeaDateProhibition
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaDateProhibition>(nameof (SeaDateProhibition), new Func<MasterDataReader, MasterDataTable.SeaDateProhibition>(MasterDataTable.SeaDateProhibition.Parse), (Func<MasterDataTable.SeaDateProhibition, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaDateResult> SeaDateResult
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaDateResult>(nameof (SeaDateResult), new Func<MasterDataReader, MasterDataTable.SeaDateResult>(MasterDataTable.SeaDateResult.Parse), (Func<MasterDataTable.SeaDateResult, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaDateResultStaging> SeaDateResultStaging
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaDateResultStaging>(nameof (SeaDateResultStaging), new Func<MasterDataReader, MasterDataTable.SeaDateResultStaging>(MasterDataTable.SeaDateResultStaging.Parse), (Func<MasterDataTable.SeaDateResultStaging, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaDateSerifAtDepart> SeaDateSerifAtDepart
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaDateSerifAtDepart>(nameof (SeaDateSerifAtDepart), new Func<MasterDataReader, MasterDataTable.SeaDateSerifAtDepart>(MasterDataTable.SeaDateSerifAtDepart.Parse), (Func<MasterDataTable.SeaDateSerifAtDepart, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaHomeMap> SeaHomeMap
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaHomeMap>(nameof (SeaHomeMap), new Func<MasterDataReader, MasterDataTable.SeaHomeMap>(MasterDataTable.SeaHomeMap.Parse), (Func<MasterDataTable.SeaHomeMap, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaHomeResult> SeaHomeResult
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaHomeResult>(nameof (SeaHomeResult), new Func<MasterDataReader, MasterDataTable.SeaHomeResult>(MasterDataTable.SeaHomeResult.Parse), (Func<MasterDataTable.SeaHomeResult, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaHomeSerif> SeaHomeSerif
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaHomeSerif>(nameof (SeaHomeSerif), new Func<MasterDataReader, MasterDataTable.SeaHomeSerif>(MasterDataTable.SeaHomeSerif.Parse), (Func<MasterDataTable.SeaHomeSerif, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaHomeTimeZone> SeaHomeTimeZone
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaHomeTimeZone>(nameof (SeaHomeTimeZone), new Func<MasterDataReader, MasterDataTable.SeaHomeTimeZone>(MasterDataTable.SeaHomeTimeZone.Parse), (Func<MasterDataTable.SeaHomeTimeZone, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaHomeTrustProvisions> SeaHomeTrustProvisions
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaHomeTrustProvisions>(nameof (SeaHomeTrustProvisions), new Func<MasterDataReader, MasterDataTable.SeaHomeTrustProvisions>(MasterDataTable.SeaHomeTrustProvisions.Parse), (Func<MasterDataTable.SeaHomeTrustProvisions, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaPresentAffinity> SeaPresentAffinity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaPresentAffinity>(nameof (SeaPresentAffinity), new Func<MasterDataReader, MasterDataTable.SeaPresentAffinity>(MasterDataTable.SeaPresentAffinity.Parse), (Func<MasterDataTable.SeaPresentAffinity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaPresentPresent> SeaPresentPresent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaPresentPresent>(nameof (SeaPresentPresent), new Func<MasterDataReader, MasterDataTable.SeaPresentPresent>(MasterDataTable.SeaPresentPresent.Parse), (Func<MasterDataTable.SeaPresentPresent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaPresentPresentAffinity> SeaPresentPresentAffinity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaPresentPresentAffinity>(nameof (SeaPresentPresentAffinity), new Func<MasterDataReader, MasterDataTable.SeaPresentPresentAffinity>(MasterDataTable.SeaPresentPresentAffinity.Parse), (Func<MasterDataTable.SeaPresentPresentAffinity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeaPresentPresentResult> SeaPresentPresentResult
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeaPresentPresentResult>(nameof (SeaPresentPresentResult), new Func<MasterDataReader, MasterDataTable.SeaPresentPresentResult>(MasterDataTable.SeaPresentPresentResult.Parse), (Func<MasterDataTable.SeaPresentPresentResult, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SeasonTicketSeasonTicket> SeasonTicketSeasonTicket
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SeasonTicketSeasonTicket>(nameof (SeasonTicketSeasonTicket), new Func<MasterDataReader, MasterDataTable.SeasonTicketSeasonTicket>(MasterDataTable.SeasonTicketSeasonTicket.Parse), (Func<MasterDataTable.SeasonTicketSeasonTicket, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SelectTicket> SelectTicket
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SelectTicket>(nameof (SelectTicket), new Func<MasterDataReader, MasterDataTable.SelectTicket>(MasterDataTable.SelectTicket.Parse), (Func<MasterDataTable.SelectTicket, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SelectTicketSelectSample> SelectTicketSelectSample
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SelectTicketSelectSample>(nameof (SelectTicketSelectSample), new Func<MasterDataReader, MasterDataTable.SelectTicketSelectSample>(MasterDataTable.SelectTicketSelectSample.Parse), (Func<MasterDataTable.SelectTicketSelectSample, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ShopArticle> ShopArticle
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ShopArticle>(nameof (ShopArticle), new Func<MasterDataReader, MasterDataTable.ShopArticle>(MasterDataTable.ShopArticle.Parse), (Func<MasterDataTable.ShopArticle, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ShopContent> ShopContent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ShopContent>(nameof (ShopContent), new Func<MasterDataReader, MasterDataTable.ShopContent>(MasterDataTable.ShopContent.Parse), (Func<MasterDataTable.ShopContent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ShopShop> ShopShop
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ShopShop>(nameof (ShopShop), new Func<MasterDataReader, MasterDataTable.ShopShop>(MasterDataTable.ShopShop.Parse), (Func<MasterDataTable.ShopShop, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.ShopTopUnit> ShopTopUnit
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.ShopTopUnit>(nameof (ShopTopUnit), new Func<MasterDataReader, MasterDataTable.ShopTopUnit>(MasterDataTable.ShopTopUnit.Parse), (Func<MasterDataTable.ShopTopUnit, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SkillMetamorphosis> SkillMetamorphosis
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SkillMetamorphosis>(nameof (SkillMetamorphosis), new Func<MasterDataReader, MasterDataTable.SkillMetamorphosis>(MasterDataTable.SkillMetamorphosis.Parse), (Func<MasterDataTable.SkillMetamorphosis, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SlotS001MedalDeck> SlotS001MedalDeck
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SlotS001MedalDeck>(nameof (SlotS001MedalDeck), new Func<MasterDataReader, MasterDataTable.SlotS001MedalDeck>(MasterDataTable.SlotS001MedalDeck.Parse), (Func<MasterDataTable.SlotS001MedalDeck, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SlotS001MedalDeckEntity> SlotS001MedalDeckEntity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SlotS001MedalDeckEntity>(nameof (SlotS001MedalDeckEntity), new Func<MasterDataReader, MasterDataTable.SlotS001MedalDeckEntity>(MasterDataTable.SlotS001MedalDeckEntity.Parse), (Func<MasterDataTable.SlotS001MedalDeckEntity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SlotS001MedalRarity> SlotS001MedalRarity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SlotS001MedalRarity>(nameof (SlotS001MedalRarity), new Func<MasterDataReader, MasterDataTable.SlotS001MedalRarity>(MasterDataTable.SlotS001MedalRarity.Parse), (Func<MasterDataTable.SlotS001MedalRarity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SlotS001MedalReel> SlotS001MedalReel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SlotS001MedalReel>(nameof (SlotS001MedalReel), new Func<MasterDataReader, MasterDataTable.SlotS001MedalReel>(MasterDataTable.SlotS001MedalReel.Parse), (Func<MasterDataTable.SlotS001MedalReel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SlotS001MedalReelDetail> SlotS001MedalReelDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SlotS001MedalReelDetail>(nameof (SlotS001MedalReelDetail), new Func<MasterDataReader, MasterDataTable.SlotS001MedalReelDetail>(MasterDataTable.SlotS001MedalReelDetail.Parse), (Func<MasterDataTable.SlotS001MedalReelDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SlotS001MedalReelIcon> SlotS001MedalReelIcon
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SlotS001MedalReelIcon>(nameof (SlotS001MedalReelIcon), new Func<MasterDataReader, MasterDataTable.SlotS001MedalReelIcon>(MasterDataTable.SlotS001MedalReelIcon.Parse), (Func<MasterDataTable.SlotS001MedalReelIcon, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackCharacter> StoryPlaybackCharacter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackCharacter>(nameof (StoryPlaybackCharacter), new Func<MasterDataReader, MasterDataTable.StoryPlaybackCharacter>(MasterDataTable.StoryPlaybackCharacter.Parse), (Func<MasterDataTable.StoryPlaybackCharacter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackCharacterDetail> StoryPlaybackCharacterDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackCharacterDetail>(nameof (StoryPlaybackCharacterDetail), new Func<MasterDataReader, MasterDataTable.StoryPlaybackCharacterDetail>(MasterDataTable.StoryPlaybackCharacterDetail.Parse), (Func<MasterDataTable.StoryPlaybackCharacterDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackEventPlay> StoryPlaybackEventPlay
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackEventPlay>(nameof (StoryPlaybackEventPlay), new Func<MasterDataReader, MasterDataTable.StoryPlaybackEventPlay>(MasterDataTable.StoryPlaybackEventPlay.Parse), (Func<MasterDataTable.StoryPlaybackEventPlay, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackExtra> StoryPlaybackExtra
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackExtra>(nameof (StoryPlaybackExtra), new Func<MasterDataReader, MasterDataTable.StoryPlaybackExtra>(MasterDataTable.StoryPlaybackExtra.Parse), (Func<MasterDataTable.StoryPlaybackExtra, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackExtraDetail> StoryPlaybackExtraDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackExtraDetail>(nameof (StoryPlaybackExtraDetail), new Func<MasterDataReader, MasterDataTable.StoryPlaybackExtraDetail>(MasterDataTable.StoryPlaybackExtraDetail.Parse), (Func<MasterDataTable.StoryPlaybackExtraDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackHarmony> StoryPlaybackHarmony
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackHarmony>(nameof (StoryPlaybackHarmony), new Func<MasterDataReader, MasterDataTable.StoryPlaybackHarmony>(MasterDataTable.StoryPlaybackHarmony.Parse), (Func<MasterDataTable.StoryPlaybackHarmony, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackHarmonyDetail> StoryPlaybackHarmonyDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackHarmonyDetail>(nameof (StoryPlaybackHarmonyDetail), new Func<MasterDataReader, MasterDataTable.StoryPlaybackHarmonyDetail>(MasterDataTable.StoryPlaybackHarmonyDetail.Parse), (Func<MasterDataTable.StoryPlaybackHarmonyDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackRaidDetail> StoryPlaybackRaidDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackRaidDetail>(nameof (StoryPlaybackRaidDetail), new Func<MasterDataReader, MasterDataTable.StoryPlaybackRaidDetail>(MasterDataTable.StoryPlaybackRaidDetail.Parse), (Func<MasterDataTable.StoryPlaybackRaidDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackSea> StoryPlaybackSea
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackSea>(nameof (StoryPlaybackSea), new Func<MasterDataReader, MasterDataTable.StoryPlaybackSea>(MasterDataTable.StoryPlaybackSea.Parse), (Func<MasterDataTable.StoryPlaybackSea, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackSeaDetail> StoryPlaybackSeaDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackSeaDetail>(nameof (StoryPlaybackSeaDetail), new Func<MasterDataReader, MasterDataTable.StoryPlaybackSeaDetail>(MasterDataTable.StoryPlaybackSeaDetail.Parse), (Func<MasterDataTable.StoryPlaybackSeaDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackStory> StoryPlaybackStory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackStory>(nameof (StoryPlaybackStory), new Func<MasterDataReader, MasterDataTable.StoryPlaybackStory>(MasterDataTable.StoryPlaybackStory.Parse), (Func<MasterDataTable.StoryPlaybackStory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.StoryPlaybackStoryDetail> StoryPlaybackStoryDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.StoryPlaybackStoryDetail>(nameof (StoryPlaybackStoryDetail), new Func<MasterDataReader, MasterDataTable.StoryPlaybackStoryDetail>(MasterDataTable.StoryPlaybackStoryDetail.Parse), (Func<MasterDataTable.StoryPlaybackStoryDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.SupplySupply> SupplySupply
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.SupplySupply>(nameof (SupplySupply), new Func<MasterDataReader, MasterDataTable.SupplySupply>(MasterDataTable.SupplySupply.Parse), (Func<MasterDataTable.SupplySupply, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TipsLoadingBackground> TipsLoadingBackground
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TipsLoadingBackground>(nameof (TipsLoadingBackground), new Func<MasterDataReader, MasterDataTable.TipsLoadingBackground>(MasterDataTable.TipsLoadingBackground.Parse), (Func<MasterDataTable.TipsLoadingBackground, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TipsTextTips> TipsTextTips
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TipsTextTips>(nameof (TipsTextTips), new Func<MasterDataReader, MasterDataTable.TipsTextTips>(MasterDataTable.TipsTextTips.Parse), (Func<MasterDataTable.TipsTextTips, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TipsTips> TipsTips
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TipsTips>(nameof (TipsTips), new Func<MasterDataReader, MasterDataTable.TipsTips>(MasterDataTable.TipsTips.Parse), (Func<MasterDataTable.TipsTips, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TotalPaymentBonus> TotalPaymentBonus
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TotalPaymentBonus>(nameof (TotalPaymentBonus), new Func<MasterDataReader, MasterDataTable.TotalPaymentBonus>(MasterDataTable.TotalPaymentBonus.Parse), (Func<MasterDataTable.TotalPaymentBonus, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TotalPaymentBonusContent> TotalPaymentBonusContent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TotalPaymentBonusContent>(nameof (TotalPaymentBonusContent), new Func<MasterDataReader, MasterDataTable.TotalPaymentBonusContent>(MasterDataTable.TotalPaymentBonusContent.Parse), (Func<MasterDataTable.TotalPaymentBonusContent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TotalPaymentBonusReward> TotalPaymentBonusReward
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TotalPaymentBonusReward>(nameof (TotalPaymentBonusReward), new Func<MasterDataReader, MasterDataTable.TotalPaymentBonusReward>(MasterDataTable.TotalPaymentBonusReward.Parse), (Func<MasterDataTable.TotalPaymentBonusReward, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerBattleStageClear> TowerBattleStageClear
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerBattleStageClear>(nameof (TowerBattleStageClear), new Func<MasterDataReader, MasterDataTable.TowerBattleStageClear>(MasterDataTable.TowerBattleStageClear.Parse), (Func<MasterDataTable.TowerBattleStageClear, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerBgm> TowerBgm
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerBgm>(nameof (TowerBgm), new Func<MasterDataReader, MasterDataTable.TowerBgm>(MasterDataTable.TowerBgm.Parse), (Func<MasterDataTable.TowerBgm, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerCommon> TowerCommon
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerCommon>(nameof (TowerCommon), new Func<MasterDataReader, MasterDataTable.TowerCommon>(MasterDataTable.TowerCommon.Parse), (Func<MasterDataTable.TowerCommon, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerCommonBackground> TowerCommonBackground
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerCommonBackground>(nameof (TowerCommonBackground), new Func<MasterDataReader, MasterDataTable.TowerCommonBackground>(MasterDataTable.TowerCommonBackground.Parse), (Func<MasterDataTable.TowerCommonBackground, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerEntryConditions> TowerEntryConditions
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerEntryConditions>(nameof (TowerEntryConditions), new Func<MasterDataReader, MasterDataTable.TowerEntryConditions>(MasterDataTable.TowerEntryConditions.Parse), (Func<MasterDataTable.TowerEntryConditions, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerFloorName> TowerFloorName
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerFloorName>(nameof (TowerFloorName), new Func<MasterDataReader, MasterDataTable.TowerFloorName>(MasterDataTable.TowerFloorName.Parse), (Func<MasterDataTable.TowerFloorName, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerHowto> TowerHowto
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerHowto>(nameof (TowerHowto), new Func<MasterDataReader, MasterDataTable.TowerHowto>(MasterDataTable.TowerHowto.Parse), (Func<MasterDataTable.TowerHowto, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerOverkill> TowerOverkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerOverkill>(nameof (TowerOverkill), new Func<MasterDataReader, MasterDataTable.TowerOverkill>(MasterDataTable.TowerOverkill.Parse), (Func<MasterDataTable.TowerOverkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerPeriod> TowerPeriod
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerPeriod>(nameof (TowerPeriod), new Func<MasterDataReader, MasterDataTable.TowerPeriod>(MasterDataTable.TowerPeriod.Parse), (Func<MasterDataTable.TowerPeriod, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerPlaybackStory> TowerPlaybackStory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerPlaybackStory>(nameof (TowerPlaybackStory), new Func<MasterDataReader, MasterDataTable.TowerPlaybackStory>(MasterDataTable.TowerPlaybackStory.Parse), (Func<MasterDataTable.TowerPlaybackStory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerPlaybackStoryDetail> TowerPlaybackStoryDetail
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerPlaybackStoryDetail>(nameof (TowerPlaybackStoryDetail), new Func<MasterDataReader, MasterDataTable.TowerPlaybackStoryDetail>(MasterDataTable.TowerPlaybackStoryDetail.Parse), (Func<MasterDataTable.TowerPlaybackStoryDetail, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerStage> TowerStage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerStage>(nameof (TowerStage), new Func<MasterDataReader, MasterDataTable.TowerStage>(MasterDataTable.TowerStage.Parse), (Func<MasterDataTable.TowerStage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.TowerTower> TowerTower
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.TowerTower>(nameof (TowerTower), new Func<MasterDataReader, MasterDataTable.TowerTower>(MasterDataTable.TowerTower.Parse), (Func<MasterDataTable.TowerTower, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitActivityScenes> UnitActivityScenes
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitActivityScenes>(nameof (UnitActivityScenes), new Func<MasterDataReader, MasterDataTable.UnitActivityScenes>(MasterDataTable.UnitActivityScenes.Parse), (Func<MasterDataTable.UnitActivityScenes, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitAdvice> UnitAdvice
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitAdvice>(nameof (UnitAdvice), new Func<MasterDataReader, MasterDataTable.UnitAdvice>(MasterDataTable.UnitAdvice.Parse), (Func<MasterDataTable.UnitAdvice, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitAffiliationIcon> UnitAffiliationIcon
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitAffiliationIcon>(nameof (UnitAffiliationIcon), new Func<MasterDataReader, MasterDataTable.UnitAffiliationIcon>(MasterDataTable.UnitAffiliationIcon.Parse), (Func<MasterDataTable.UnitAffiliationIcon, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitAwakeningEffect> UnitAwakeningEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitAwakeningEffect>(nameof (UnitAwakeningEffect), new Func<MasterDataReader, MasterDataTable.UnitAwakeningEffect>(MasterDataTable.UnitAwakeningEffect.Parse), (Func<MasterDataTable.UnitAwakeningEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitBreakThrough> UnitBreakThrough
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitBreakThrough>(nameof (UnitBreakThrough), new Func<MasterDataReader, MasterDataTable.UnitBreakThrough>(MasterDataTable.UnitBreakThrough.Parse), (Func<MasterDataTable.UnitBreakThrough, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitBuildupMaterialPattern> UnitBuildupMaterialPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitBuildupMaterialPattern>(nameof (UnitBuildupMaterialPattern), new Func<MasterDataReader, MasterDataTable.UnitBuildupMaterialPattern>(MasterDataTable.UnitBuildupMaterialPattern.Parse), (Func<MasterDataTable.UnitBuildupMaterialPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitCameraPattern> UnitCameraPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitCameraPattern>(nameof (UnitCameraPattern), new Func<MasterDataReader, MasterDataTable.UnitCameraPattern>(MasterDataTable.UnitCameraPattern.Parse), (Func<MasterDataTable.UnitCameraPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitCharacter> UnitCharacter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitCharacter>(nameof (UnitCharacter), new Func<MasterDataReader, MasterDataTable.UnitCharacter>(MasterDataTable.UnitCharacter.Parse), (Func<MasterDataTable.UnitCharacter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitCharacterExtension> UnitCharacterExtension
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitCharacterExtension>(nameof (UnitCharacterExtension), new Func<MasterDataReader, MasterDataTable.UnitCharacterExtension>(MasterDataTable.UnitCharacterExtension.Parse), (Func<MasterDataTable.UnitCharacterExtension, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitComponent> UnitComponent
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitComponent>(nameof (UnitComponent), new Func<MasterDataReader, MasterDataTable.UnitComponent>(MasterDataTable.UnitComponent.Parse), (Func<MasterDataTable.UnitComponent, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitCutinInfo> UnitCutinInfo
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitCutinInfo>(nameof (UnitCutinInfo), new Func<MasterDataReader, MasterDataTable.UnitCutinInfo>(MasterDataTable.UnitCutinInfo.Parse), (Func<MasterDataTable.UnitCutinInfo, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitEvolutionPattern> UnitEvolutionPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitEvolutionPattern>(nameof (UnitEvolutionPattern), new Func<MasterDataReader, MasterDataTable.UnitEvolutionPattern>(MasterDataTable.UnitEvolutionPattern.Parse), (Func<MasterDataTable.UnitEvolutionPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitEvolutionUnit> UnitEvolutionUnit
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitEvolutionUnit>(nameof (UnitEvolutionUnit), new Func<MasterDataReader, MasterDataTable.UnitEvolutionUnit>(MasterDataTable.UnitEvolutionUnit.Parse), (Func<MasterDataTable.UnitEvolutionUnit, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitExpireDate> UnitExpireDate
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitExpireDate>(nameof (UnitExpireDate), new Func<MasterDataReader, MasterDataTable.UnitExpireDate>(MasterDataTable.UnitExpireDate.Parse), (Func<MasterDataTable.UnitExpireDate, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitExtensionStory> UnitExtensionStory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitExtensionStory>(nameof (UnitExtensionStory), new Func<MasterDataReader, MasterDataTable.UnitExtensionStory>(MasterDataTable.UnitExtensionStory.Parse), (Func<MasterDataTable.UnitExtensionStory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitFamilyValue> UnitFamilyValue
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitFamilyValue>(nameof (UnitFamilyValue), new Func<MasterDataReader, MasterDataTable.UnitFamilyValue>(MasterDataTable.UnitFamilyValue.Parse), (Func<MasterDataTable.UnitFamilyValue, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitFootstepType> UnitFootstepType
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitFootstepType>(nameof (UnitFootstepType), new Func<MasterDataReader, MasterDataTable.UnitFootstepType>(MasterDataTable.UnitFootstepType.Parse), (Func<MasterDataTable.UnitFootstepType, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitGenderText> UnitGenderText
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitGenderText>(nameof (UnitGenderText), new Func<MasterDataReader, MasterDataTable.UnitGenderText>(MasterDataTable.UnitGenderText.Parse), (Func<MasterDataTable.UnitGenderText, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitGroup> UnitGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitGroup>(nameof (UnitGroup), new Func<MasterDataReader, MasterDataTable.UnitGroup>(MasterDataTable.UnitGroup.Parse), (Func<MasterDataTable.UnitGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitGroupClothingCategory> UnitGroupClothingCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitGroupClothingCategory>(nameof (UnitGroupClothingCategory), new Func<MasterDataReader, MasterDataTable.UnitGroupClothingCategory>(MasterDataTable.UnitGroupClothingCategory.Parse), (Func<MasterDataTable.UnitGroupClothingCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitGroupGenerationCategory> UnitGroupGenerationCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitGroupGenerationCategory>(nameof (UnitGroupGenerationCategory), new Func<MasterDataReader, MasterDataTable.UnitGroupGenerationCategory>(MasterDataTable.UnitGroupGenerationCategory.Parse), (Func<MasterDataTable.UnitGroupGenerationCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitGroupLargeCategory> UnitGroupLargeCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitGroupLargeCategory>(nameof (UnitGroupLargeCategory), new Func<MasterDataReader, MasterDataTable.UnitGroupLargeCategory>(MasterDataTable.UnitGroupLargeCategory.Parse), (Func<MasterDataTable.UnitGroupLargeCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitGroupSmallCategory> UnitGroupSmallCategory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitGroupSmallCategory>(nameof (UnitGroupSmallCategory), new Func<MasterDataReader, MasterDataTable.UnitGroupSmallCategory>(MasterDataTable.UnitGroupSmallCategory.Parse), (Func<MasterDataTable.UnitGroupSmallCategory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitHomeVoicePattern> UnitHomeVoicePattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitHomeVoicePattern>(nameof (UnitHomeVoicePattern), new Func<MasterDataReader, MasterDataTable.UnitHomeVoicePattern>(MasterDataTable.UnitHomeVoicePattern.Parse), (Func<MasterDataTable.UnitHomeVoicePattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitIllustPattern> UnitIllustPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitIllustPattern>(nameof (UnitIllustPattern), new Func<MasterDataReader, MasterDataTable.UnitIllustPattern>(MasterDataTable.UnitIllustPattern.Parse), (Func<MasterDataTable.UnitIllustPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitInitialParam> UnitInitialParam
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitInitialParam>(nameof (UnitInitialParam), new Func<MasterDataReader, MasterDataTable.UnitInitialParam>(MasterDataTable.UnitInitialParam.Parse), (Func<MasterDataTable.UnitInitialParam, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitJob> UnitJob
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitJob>(nameof (UnitJob), new Func<MasterDataReader, MasterDataTable.UnitJob>(MasterDataTable.UnitJob.Parse), (Func<MasterDataTable.UnitJob, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitJobFamily> UnitJobFamily
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitJobFamily>(nameof (UnitJobFamily), new Func<MasterDataReader, MasterDataTable.UnitJobFamily>(MasterDataTable.UnitJobFamily.Parse), (Func<MasterDataTable.UnitJobFamily, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitJobRankName> UnitJobRankName
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitJobRankName>(nameof (UnitJobRankName), new Func<MasterDataReader, MasterDataTable.UnitJobRankName>(MasterDataTable.UnitJobRankName.Parse), (Func<MasterDataTable.UnitJobRankName, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitLeaderSkill> UnitLeaderSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitLeaderSkill>(nameof (UnitLeaderSkill), new Func<MasterDataReader, MasterDataTable.UnitLeaderSkill>(MasterDataTable.UnitLeaderSkill.Parse), (Func<MasterDataTable.UnitLeaderSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitLevel> UnitLevel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitLevel>(nameof (UnitLevel), new Func<MasterDataReader, MasterDataTable.UnitLevel>(MasterDataTable.UnitLevel.Parse), (Func<MasterDataTable.UnitLevel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitMaterialExclusion> UnitMaterialExclusion
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitMaterialExclusion>(nameof (UnitMaterialExclusion), new Func<MasterDataReader, MasterDataTable.UnitMaterialExclusion>(MasterDataTable.UnitMaterialExclusion.Parse), (Func<MasterDataTable.UnitMaterialExclusion, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitMaterialQuestInfo> UnitMaterialQuestInfo
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitMaterialQuestInfo>(nameof (UnitMaterialQuestInfo), new Func<MasterDataReader, MasterDataTable.UnitMaterialQuestInfo>(MasterDataTable.UnitMaterialQuestInfo.Parse), (Func<MasterDataTable.UnitMaterialQuestInfo, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitModel> UnitModel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitModel>(nameof (UnitModel), new Func<MasterDataReader, MasterDataTable.UnitModel>(MasterDataTable.UnitModel.Parse), (Func<MasterDataTable.UnitModel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitPickupSkill> UnitPickupSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitPickupSkill>(nameof (UnitPickupSkill), new Func<MasterDataReader, MasterDataTable.UnitPickupSkill>(MasterDataTable.UnitPickupSkill.Parse), (Func<MasterDataTable.UnitPickupSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitProficiency> UnitProficiency
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitProficiency>(nameof (UnitProficiency), new Func<MasterDataReader, MasterDataTable.UnitProficiency>(MasterDataTable.UnitProficiency.Parse), (Func<MasterDataTable.UnitProficiency, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitProficiencyIncr> UnitProficiencyIncr
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitProficiencyIncr>(nameof (UnitProficiencyIncr), new Func<MasterDataReader, MasterDataTable.UnitProficiencyIncr>(MasterDataTable.UnitProficiencyIncr.Parse), (Func<MasterDataTable.UnitProficiencyIncr, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitProficiencyLevel> UnitProficiencyLevel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitProficiencyLevel>(nameof (UnitProficiencyLevel), new Func<MasterDataReader, MasterDataTable.UnitProficiencyLevel>(MasterDataTable.UnitProficiencyLevel.Parse), (Func<MasterDataTable.UnitProficiencyLevel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitRarity> UnitRarity
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitRarity>(nameof (UnitRarity), new Func<MasterDataReader, MasterDataTable.UnitRarity>(MasterDataTable.UnitRarity.Parse), (Func<MasterDataTable.UnitRarity, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitRecommend> UnitRecommend
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitRecommend>(nameof (UnitRecommend), new Func<MasterDataReader, MasterDataTable.UnitRecommend>(MasterDataTable.UnitRecommend.Parse), (Func<MasterDataTable.UnitRecommend, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitReferenceImage> UnitReferenceImage
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitReferenceImage>(nameof (UnitReferenceImage), new Func<MasterDataReader, MasterDataTable.UnitReferenceImage>(MasterDataTable.UnitReferenceImage.Parse), (Func<MasterDataTable.UnitReferenceImage, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitRenderingPattern> UnitRenderingPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitRenderingPattern>(nameof (UnitRenderingPattern), new Func<MasterDataReader, MasterDataTable.UnitRenderingPattern>(MasterDataTable.UnitRenderingPattern.Parse), (Func<MasterDataTable.UnitRenderingPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitRole> UnitRole
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitRole>(nameof (UnitRole), new Func<MasterDataReader, MasterDataTable.UnitRole>(MasterDataTable.UnitRole.Parse), (Func<MasterDataTable.UnitRole, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSEASkill> UnitSEASkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSEASkill>(nameof (UnitSEASkill), new Func<MasterDataReader, MasterDataTable.UnitSEASkill>(MasterDataTable.UnitSEASkill.Parse), (Func<MasterDataTable.UnitSEASkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkill> UnitSkill
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkill>(nameof (UnitSkill), new Func<MasterDataReader, MasterDataTable.UnitSkill>(MasterDataTable.UnitSkill.Parse), (Func<MasterDataTable.UnitSkill, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillAwake> UnitSkillAwake
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillAwake>(nameof (UnitSkillAwake), new Func<MasterDataReader, MasterDataTable.UnitSkillAwake>(MasterDataTable.UnitSkillAwake.Parse), (Func<MasterDataTable.UnitSkillAwake, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillCharacterQuest> UnitSkillCharacterQuest
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillCharacterQuest>(nameof (UnitSkillCharacterQuest), new Func<MasterDataReader, MasterDataTable.UnitSkillCharacterQuest>(MasterDataTable.UnitSkillCharacterQuest.Parse), (Func<MasterDataTable.UnitSkillCharacterQuest, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillEvolution> UnitSkillEvolution
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillEvolution>(nameof (UnitSkillEvolution), new Func<MasterDataReader, MasterDataTable.UnitSkillEvolution>(MasterDataTable.UnitSkillEvolution.Parse), (Func<MasterDataTable.UnitSkillEvolution, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillGroup> UnitSkillGroup
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillGroup>(nameof (UnitSkillGroup), new Func<MasterDataReader, MasterDataTable.UnitSkillGroup>(MasterDataTable.UnitSkillGroup.Parse), (Func<MasterDataTable.UnitSkillGroup, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillHarmonyQuest> UnitSkillHarmonyQuest
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillHarmonyQuest>(nameof (UnitSkillHarmonyQuest), new Func<MasterDataReader, MasterDataTable.UnitSkillHarmonyQuest>(MasterDataTable.UnitSkillHarmonyQuest.Parse), (Func<MasterDataTable.UnitSkillHarmonyQuest, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillIntimate> UnitSkillIntimate
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillIntimate>(nameof (UnitSkillIntimate), new Func<MasterDataReader, MasterDataTable.UnitSkillIntimate>(MasterDataTable.UnitSkillIntimate.Parse), (Func<MasterDataTable.UnitSkillIntimate, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillLevelUpProbability> UnitSkillLevelUpProbability
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillLevelUpProbability>(nameof (UnitSkillLevelUpProbability), new Func<MasterDataReader, MasterDataTable.UnitSkillLevelUpProbability>(MasterDataTable.UnitSkillLevelUpProbability.Parse), (Func<MasterDataTable.UnitSkillLevelUpProbability, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillupSetting> UnitSkillupSetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillupSetting>(nameof (UnitSkillupSetting), new Func<MasterDataReader, MasterDataTable.UnitSkillupSetting>(MasterDataTable.UnitSkillupSetting.Parse), (Func<MasterDataTable.UnitSkillupSetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitSkillupSkillGroupSetting> UnitSkillupSkillGroupSetting
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitSkillupSkillGroupSetting>(nameof (UnitSkillupSkillGroupSetting), new Func<MasterDataReader, MasterDataTable.UnitSkillupSkillGroupSetting>(MasterDataTable.UnitSkillupSkillGroupSetting.Parse), (Func<MasterDataTable.UnitSkillupSkillGroupSetting, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTransmigrationMaterial> UnitTransmigrationMaterial
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTransmigrationMaterial>(nameof (UnitTransmigrationMaterial), new Func<MasterDataReader, MasterDataTable.UnitTransmigrationMaterial>(MasterDataTable.UnitTransmigrationMaterial.Parse), (Func<MasterDataTable.UnitTransmigrationMaterial, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTransmigrationPattern> UnitTransmigrationPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTransmigrationPattern>(nameof (UnitTransmigrationPattern), new Func<MasterDataReader, MasterDataTable.UnitTransmigrationPattern>(MasterDataTable.UnitTransmigrationPattern.Parse), (Func<MasterDataTable.UnitTransmigrationPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTrustLevelMaterialPattern> UnitTrustLevelMaterialPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTrustLevelMaterialPattern>(nameof (UnitTrustLevelMaterialPattern), new Func<MasterDataReader, MasterDataTable.UnitTrustLevelMaterialPattern>(MasterDataTable.UnitTrustLevelMaterialPattern.Parse), (Func<MasterDataTable.UnitTrustLevelMaterialPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTrustUpperLimitEffect> UnitTrustUpperLimitEffect
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTrustUpperLimitEffect>(nameof (UnitTrustUpperLimitEffect), new Func<MasterDataReader, MasterDataTable.UnitTrustUpperLimitEffect>(MasterDataTable.UnitTrustUpperLimitEffect.Parse), (Func<MasterDataTable.UnitTrustUpperLimitEffect, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitType> UnitType
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitType>(nameof (UnitType), new Func<MasterDataReader, MasterDataTable.UnitType>(MasterDataTable.UnitType.Parse), (Func<MasterDataTable.UnitType, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTypeParameter> UnitTypeParameter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTypeParameter>(nameof (UnitTypeParameter), new Func<MasterDataReader, MasterDataTable.UnitTypeParameter>(MasterDataTable.UnitTypeParameter.Parse), (Func<MasterDataTable.UnitTypeParameter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTypeSettings> UnitTypeSettings
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTypeSettings>(nameof (UnitTypeSettings), new Func<MasterDataReader, MasterDataTable.UnitTypeSettings>(MasterDataTable.UnitTypeSettings.Parse), (Func<MasterDataTable.UnitTypeSettings, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTypeTicket> UnitTypeTicket
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTypeTicket>(nameof (UnitTypeTicket), new Func<MasterDataReader, MasterDataTable.UnitTypeTicket>(MasterDataTable.UnitTypeTicket.Parse), (Func<MasterDataTable.UnitTypeTicket, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTypeTicketExclude> UnitTypeTicketExclude
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTypeTicketExclude>(nameof (UnitTypeTicketExclude), new Func<MasterDataReader, MasterDataTable.UnitTypeTicketExclude>(MasterDataTable.UnitTypeTicketExclude.Parse), (Func<MasterDataTable.UnitTypeTicketExclude, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitTypeTicketUnusable> UnitTypeTicketUnusable
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitTypeTicketUnusable>(nameof (UnitTypeTicketUnusable), new Func<MasterDataReader, MasterDataTable.UnitTypeTicketUnusable>(MasterDataTable.UnitTypeTicketUnusable.Parse), (Func<MasterDataTable.UnitTypeTicketUnusable, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnit> UnitUnit
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnit>(nameof (UnitUnit), new Func<MasterDataReader, MasterDataTable.UnitUnit>(MasterDataTable.UnitUnit.Parse), (Func<MasterDataTable.UnitUnit, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitBuildupAmount> UnitUnitBuildupAmount
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitBuildupAmount>(nameof (UnitUnitBuildupAmount), new Func<MasterDataReader, MasterDataTable.UnitUnitBuildupAmount>(MasterDataTable.UnitUnitBuildupAmount.Parse), (Func<MasterDataTable.UnitUnitBuildupAmount, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitBuildupLimitRelease> UnitUnitBuildupLimitRelease
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitBuildupLimitRelease>(nameof (UnitUnitBuildupLimitRelease), new Func<MasterDataReader, MasterDataTable.UnitUnitBuildupLimitRelease>(MasterDataTable.UnitUnitBuildupLimitRelease.Parse), (Func<MasterDataTable.UnitUnitBuildupLimitRelease, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitDescription> UnitUnitDescription
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitDescription>(nameof (UnitUnitDescription), new Func<MasterDataReader, MasterDataTable.UnitUnitDescription>(MasterDataTable.UnitUnitDescription.Parse), (Func<MasterDataTable.UnitUnitDescription, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitFamily> UnitUnitFamily
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitFamily>(nameof (UnitUnitFamily), new Func<MasterDataReader, MasterDataTable.UnitUnitFamily>(MasterDataTable.UnitUnitFamily.Parse), (Func<MasterDataTable.UnitUnitFamily, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitGearModelKind> UnitUnitGearModelKind
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitGearModelKind>(nameof (UnitUnitGearModelKind), new Func<MasterDataReader, MasterDataTable.UnitUnitGearModelKind>(MasterDataTable.UnitUnitGearModelKind.Parse), (Func<MasterDataTable.UnitUnitGearModelKind, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitGrowth> UnitUnitGrowth
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitGrowth>(nameof (UnitUnitGrowth), new Func<MasterDataReader, MasterDataTable.UnitUnitGrowth>(MasterDataTable.UnitUnitGrowth.Parse), (Func<MasterDataTable.UnitUnitGrowth, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitParameter> UnitUnitParameter
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitParameter>(nameof (UnitUnitParameter), new Func<MasterDataReader, MasterDataTable.UnitUnitParameter>(MasterDataTable.UnitUnitParameter.Parse), (Func<MasterDataTable.UnitUnitParameter, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitStory> UnitUnitStory
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitStory>(nameof (UnitUnitStory), new Func<MasterDataReader, MasterDataTable.UnitUnitStory>(MasterDataTable.UnitUnitStory.Parse), (Func<MasterDataTable.UnitUnitStory, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitUnitSupplement> UnitUnitSupplement
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitUnitSupplement>(nameof (UnitUnitSupplement), new Func<MasterDataReader, MasterDataTable.UnitUnitSupplement>(MasterDataTable.UnitUnitSupplement.Parse), (Func<MasterDataTable.UnitUnitSupplement, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitVoicePattern> UnitVoicePattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitVoicePattern>(nameof (UnitVoicePattern), new Func<MasterDataReader, MasterDataTable.UnitVoicePattern>(MasterDataTable.UnitVoicePattern.Parse), (Func<MasterDataTable.UnitVoicePattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitVoiceView> UnitVoiceView
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitVoiceView>(nameof (UnitVoiceView), new Func<MasterDataReader, MasterDataTable.UnitVoiceView>(MasterDataTable.UnitVoiceView.Parse), (Func<MasterDataTable.UnitVoiceView, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnitXLevel> UnitXLevel
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnitXLevel>(nameof (UnitXLevel), new Func<MasterDataReader, MasterDataTable.UnitXLevel>(MasterDataTable.UnitXLevel.Parse), (Func<MasterDataTable.UnitXLevel, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnityPureValueUpPattern> UnityPureValueUpPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnityPureValueUpPattern>(nameof (UnityPureValueUpPattern), new Func<MasterDataReader, MasterDataTable.UnityPureValueUpPattern>(MasterDataTable.UnityPureValueUpPattern.Parse), (Func<MasterDataTable.UnityPureValueUpPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnityValueUpItemQuest> UnityValueUpItemQuest
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnityValueUpItemQuest>(nameof (UnityValueUpItemQuest), new Func<MasterDataReader, MasterDataTable.UnityValueUpItemQuest>(MasterDataTable.UnityValueUpItemQuest.Parse), (Func<MasterDataTable.UnityValueUpItemQuest, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.UnityValueUpPattern> UnityValueUpPattern
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.UnityValueUpPattern>(nameof (UnityValueUpPattern), new Func<MasterDataReader, MasterDataTable.UnityValueUpPattern>(MasterDataTable.UnityValueUpPattern.Parse), (Func<MasterDataTable.UnityValueUpPattern, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.XJobInformation> XJobInformation
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.XJobInformation>(nameof (XJobInformation), new Func<MasterDataReader, MasterDataTable.XJobInformation>(MasterDataTable.XJobInformation.Parse), (Func<MasterDataTable.XJobInformation, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.XLevelLimits> XLevelLimits
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.XLevelLimits>(nameof (XLevelLimits), new Func<MasterDataReader, MasterDataTable.XLevelLimits>(MasterDataTable.XLevelLimits.Parse), (Func<MasterDataTable.XLevelLimits, int>) (x => x.ID));
    }
  }

  public static AssocList<int, MasterDataTable.XLevelStatus> XLevelStatus
  {
    get
    {
      return MasterDataCache.Get<int, MasterDataTable.XLevelStatus>(nameof (XLevelStatus), new Func<MasterDataReader, MasterDataTable.XLevelStatus>(MasterDataTable.XLevelStatus.Parse), (Func<MasterDataTable.XLevelStatus, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.AIScore[] AIScoreList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.AIScore>("AIScore", new Func<MasterDataReader, MasterDataTable.AIScore>(MasterDataTable.AIScore.Parse), (Func<MasterDataTable.AIScore, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.AIScoreCorrection[] AIScoreCorrectionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.AIScoreCorrection>("AIScoreCorrection", new Func<MasterDataReader, MasterDataTable.AIScoreCorrection>(MasterDataTable.AIScoreCorrection.Parse), (Func<MasterDataTable.AIScoreCorrection, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.AIScorePattern[] AIScorePatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.AIScorePattern>("AIScorePattern", new Func<MasterDataReader, MasterDataTable.AIScorePattern>(MasterDataTable.AIScorePattern.Parse), (Func<MasterDataTable.AIScorePattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.AppSetupTuning[] AppSetupTuningList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.AppSetupTuning>("AppSetupTuning", new Func<MasterDataReader, MasterDataTable.AppSetupTuning>(MasterDataTable.AppSetupTuning.Parse), (Func<MasterDataTable.AppSetupTuning, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.AttackMethod[] AttackMethodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.AttackMethod>("AttackMethod", new Func<MasterDataReader, MasterDataTable.AttackMethod>(MasterDataTable.AttackMethod.Parse), (Func<MasterDataTable.AttackMethod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.AwakeSkillCategory[] AwakeSkillCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.AwakeSkillCategory>("AwakeSkillCategory", new Func<MasterDataReader, MasterDataTable.AwakeSkillCategory>(MasterDataTable.AwakeSkillCategory.Parse), (Func<MasterDataTable.AwakeSkillCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleAIScript[] BattleAIScriptList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleAIScript>("BattleAIScript", new Func<MasterDataReader, MasterDataTable.BattleAIScript>(MasterDataTable.BattleAIScript.Parse), (Func<MasterDataTable.BattleAIScript, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleCameraFilter[] BattleCameraFilterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleCameraFilter>("BattleCameraFilter", new Func<MasterDataReader, MasterDataTable.BattleCameraFilter>(MasterDataTable.BattleCameraFilter.Parse), (Func<MasterDataTable.BattleCameraFilter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleEarthItemDropTable[] BattleEarthItemDropTableList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleEarthItemDropTable>("BattleEarthItemDropTable", new Func<MasterDataReader, MasterDataTable.BattleEarthItemDropTable>(MasterDataTable.BattleEarthItemDropTable.Parse), (Func<MasterDataTable.BattleEarthItemDropTable, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleEarthStageGuest[] BattleEarthStageGuestList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleEarthStageGuest>("BattleEarthStageGuest", new Func<MasterDataReader, MasterDataTable.BattleEarthStageGuest>(MasterDataTable.BattleEarthStageGuest.Parse), (Func<MasterDataTable.BattleEarthStageGuest, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleEarthStageGuestSkill[] BattleEarthStageGuestSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleEarthStageGuestSkill>("BattleEarthStageGuestSkill", new Func<MasterDataReader, MasterDataTable.BattleEarthStageGuestSkill>(MasterDataTable.BattleEarthStageGuestSkill.Parse), (Func<MasterDataTable.BattleEarthStageGuestSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleEnemyAcquireSkill[] BattleEnemyAcquireSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleEnemyAcquireSkill>("BattleEnemyAcquireSkill", new Func<MasterDataReader, MasterDataTable.BattleEnemyAcquireSkill>(MasterDataTable.BattleEnemyAcquireSkill.Parse), (Func<MasterDataTable.BattleEnemyAcquireSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleEnemyParameterDeviationTable[] BattleEnemyParameterDeviationTableList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleEnemyParameterDeviationTable>("BattleEnemyParameterDeviationTable", new Func<MasterDataReader, MasterDataTable.BattleEnemyParameterDeviationTable>(MasterDataTable.BattleEnemyParameterDeviationTable.Parse), (Func<MasterDataTable.BattleEnemyParameterDeviationTable, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleEnemyParameterTable[] BattleEnemyParameterTableList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleEnemyParameterTable>("BattleEnemyParameterTable", new Func<MasterDataReader, MasterDataTable.BattleEnemyParameterTable>(MasterDataTable.BattleEnemyParameterTable.Parse), (Func<MasterDataTable.BattleEnemyParameterTable, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleFieldEffect[] BattleFieldEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleFieldEffect>("BattleFieldEffect", new Func<MasterDataReader, MasterDataTable.BattleFieldEffect>(MasterDataTable.BattleFieldEffect.Parse), (Func<MasterDataTable.BattleFieldEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleFieldEffectStage[] BattleFieldEffectStageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleFieldEffectStage>("BattleFieldEffectStage", new Func<MasterDataReader, MasterDataTable.BattleFieldEffectStage>(MasterDataTable.BattleFieldEffectStage.Parse), (Func<MasterDataTable.BattleFieldEffectStage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleLandform[] BattleLandformList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleLandform>("BattleLandform", new Func<MasterDataReader, MasterDataTable.BattleLandform>(MasterDataTable.BattleLandform.Parse), (Func<MasterDataTable.BattleLandform, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleLandformEffect[] BattleLandformEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleLandformEffect>("BattleLandformEffect", new Func<MasterDataReader, MasterDataTable.BattleLandformEffect>(MasterDataTable.BattleLandformEffect.Parse), (Func<MasterDataTable.BattleLandformEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleLandformEffectGroup[] BattleLandformEffectGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleLandformEffectGroup>("BattleLandformEffectGroup", new Func<MasterDataReader, MasterDataTable.BattleLandformEffectGroup>(MasterDataTable.BattleLandformEffectGroup.Parse), (Func<MasterDataTable.BattleLandformEffectGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleLandformFootstepType[] BattleLandformFootstepTypeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleLandformFootstepType>("BattleLandformFootstepType", new Func<MasterDataReader, MasterDataTable.BattleLandformFootstepType>(MasterDataTable.BattleLandformFootstepType.Parse), (Func<MasterDataTable.BattleLandformFootstepType, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleLandformIncr[] BattleLandformIncrList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleLandformIncr>("BattleLandformIncr", new Func<MasterDataReader, MasterDataTable.BattleLandformIncr>(MasterDataTable.BattleLandformIncr.Parse), (Func<MasterDataTable.BattleLandformIncr, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleLandformTag[] BattleLandformTagList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleLandformTag>("BattleLandformTag", new Func<MasterDataReader, MasterDataTable.BattleLandformTag>(MasterDataTable.BattleLandformTag.Parse), (Func<MasterDataTable.BattleLandformTag, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleMap[] BattleMapList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleMap>("BattleMap", new Func<MasterDataReader, MasterDataTable.BattleMap>(MasterDataTable.BattleMap.Parse), (Func<MasterDataTable.BattleMap, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleMapFacilitySetting[] BattleMapFacilitySettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleMapFacilitySetting>("BattleMapFacilitySetting", new Func<MasterDataReader, MasterDataTable.BattleMapFacilitySetting>(MasterDataTable.BattleMapFacilitySetting.Parse), (Func<MasterDataTable.BattleMapFacilitySetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleMapLandform[] BattleMapLandformList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleMapLandform>("BattleMapLandform", new Func<MasterDataReader, MasterDataTable.BattleMapLandform>(MasterDataTable.BattleMapLandform.Parse), (Func<MasterDataTable.BattleMapLandform, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleReinforcement[] BattleReinforcementList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleReinforcement>("BattleReinforcement", new Func<MasterDataReader, MasterDataTable.BattleReinforcement>(MasterDataTable.BattleReinforcement.Parse), (Func<MasterDataTable.BattleReinforcement, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleReinforcementLogic[] BattleReinforcementLogicList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleReinforcementLogic>("BattleReinforcementLogic", new Func<MasterDataReader, MasterDataTable.BattleReinforcementLogic>(MasterDataTable.BattleReinforcementLogic.Parse), (Func<MasterDataTable.BattleReinforcementLogic, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleSpecialSkill[] BattleSpecialSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleSpecialSkill>("BattleSpecialSkill", new Func<MasterDataReader, MasterDataTable.BattleSpecialSkill>(MasterDataTable.BattleSpecialSkill.Parse), (Func<MasterDataTable.BattleSpecialSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStage[] BattleStageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStage>("BattleStage", new Func<MasterDataReader, MasterDataTable.BattleStage>(MasterDataTable.BattleStage.Parse), (Func<MasterDataTable.BattleStage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageClear[] BattleStageClearList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageClear>("BattleStageClear", new Func<MasterDataReader, MasterDataTable.BattleStageClear>(MasterDataTable.BattleStageClear.Parse), (Func<MasterDataTable.BattleStageClear, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageEnemy[] BattleStageEnemyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageEnemy>("BattleStageEnemy", new Func<MasterDataReader, MasterDataTable.BattleStageEnemy>(MasterDataTable.BattleStageEnemy.Parse), (Func<MasterDataTable.BattleStageEnemy, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageEnemyAttackMethod[] BattleStageEnemyAttackMethodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageEnemyAttackMethod>("BattleStageEnemyAttackMethod", new Func<MasterDataReader, MasterDataTable.BattleStageEnemyAttackMethod>(MasterDataTable.BattleStageEnemyAttackMethod.Parse), (Func<MasterDataTable.BattleStageEnemyAttackMethod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageEnemyJob[] BattleStageEnemyJobList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageEnemyJob>("BattleStageEnemyJob", new Func<MasterDataReader, MasterDataTable.BattleStageEnemyJob>(MasterDataTable.BattleStageEnemyJob.Parse), (Func<MasterDataTable.BattleStageEnemyJob, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageEnemyReward[] BattleStageEnemyRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageEnemyReward>("BattleStageEnemyReward", new Func<MasterDataReader, MasterDataTable.BattleStageEnemyReward>(MasterDataTable.BattleStageEnemyReward.Parse), (Func<MasterDataTable.BattleStageEnemyReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageEnemySkill[] BattleStageEnemySkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageEnemySkill>("BattleStageEnemySkill", new Func<MasterDataReader, MasterDataTable.BattleStageEnemySkill>(MasterDataTable.BattleStageEnemySkill.Parse), (Func<MasterDataTable.BattleStageEnemySkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageGuest[] BattleStageGuestList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageGuest>("BattleStageGuest", new Func<MasterDataReader, MasterDataTable.BattleStageGuest>(MasterDataTable.BattleStageGuest.Parse), (Func<MasterDataTable.BattleStageGuest, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageGuestAttackMethod[] BattleStageGuestAttackMethodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageGuestAttackMethod>("BattleStageGuestAttackMethod", new Func<MasterDataReader, MasterDataTable.BattleStageGuestAttackMethod>(MasterDataTable.BattleStageGuestAttackMethod.Parse), (Func<MasterDataTable.BattleStageGuestAttackMethod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageGuestJob[] BattleStageGuestJobList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageGuestJob>("BattleStageGuestJob", new Func<MasterDataReader, MasterDataTable.BattleStageGuestJob>(MasterDataTable.BattleStageGuestJob.Parse), (Func<MasterDataTable.BattleStageGuestJob, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageGuestSkill[] BattleStageGuestSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageGuestSkill>("BattleStageGuestSkill", new Func<MasterDataReader, MasterDataTable.BattleStageGuestSkill>(MasterDataTable.BattleStageGuestSkill.Parse), (Func<MasterDataTable.BattleStageGuestSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStagePanelEvent[] BattleStagePanelEventList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStagePanelEvent>("BattleStagePanelEvent", new Func<MasterDataReader, MasterDataTable.BattleStagePanelEvent>(MasterDataTable.BattleStagePanelEvent.Parse), (Func<MasterDataTable.BattleStagePanelEvent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStagePlayer[] BattleStagePlayerList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStagePlayer>("BattleStagePlayer", new Func<MasterDataReader, MasterDataTable.BattleStagePlayer>(MasterDataTable.BattleStagePlayer.Parse), (Func<MasterDataTable.BattleStagePlayer, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleStageUserUnit[] BattleStageUserUnitList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleStageUserUnit>("BattleStageUserUnit", new Func<MasterDataReader, MasterDataTable.BattleStageUserUnit>(MasterDataTable.BattleStageUserUnit.Parse), (Func<MasterDataTable.BattleStageUserUnit, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleUnitLandformFootstep[] BattleUnitLandformFootstepList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleUnitLandformFootstep>("BattleUnitLandformFootstep", new Func<MasterDataReader, MasterDataTable.BattleUnitLandformFootstep>(MasterDataTable.BattleUnitLandformFootstep.Parse), (Func<MasterDataTable.BattleUnitLandformFootstep, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleUnitRule[] BattleUnitRuleList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleUnitRule>("BattleUnitRule", new Func<MasterDataReader, MasterDataTable.BattleUnitRule>(MasterDataTable.BattleUnitRule.Parse), (Func<MasterDataTable.BattleUnitRule, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleVictoryAreaCondition[] BattleVictoryAreaConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleVictoryAreaCondition>("BattleVictoryAreaCondition", new Func<MasterDataReader, MasterDataTable.BattleVictoryAreaCondition>(MasterDataTable.BattleVictoryAreaCondition.Parse), (Func<MasterDataTable.BattleVictoryAreaCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleVictoryCondition[] BattleVictoryConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleVictoryCondition>("BattleVictoryCondition", new Func<MasterDataReader, MasterDataTable.BattleVictoryCondition>(MasterDataTable.BattleVictoryCondition.Parse), (Func<MasterDataTable.BattleVictoryCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillAilmentEffect[] BattleskillAilmentEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillAilmentEffect>("BattleskillAilmentEffect", new Func<MasterDataReader, MasterDataTable.BattleskillAilmentEffect>(MasterDataTable.BattleskillAilmentEffect.Parse), (Func<MasterDataTable.BattleskillAilmentEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillDuelClipEventEffectDataPreload[] BattleskillDuelClipEventEffectDataPreloadList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillDuelClipEventEffectDataPreload>("BattleskillDuelClipEventEffectDataPreload", new Func<MasterDataReader, MasterDataTable.BattleskillDuelClipEventEffectDataPreload>(MasterDataTable.BattleskillDuelClipEventEffectDataPreload.Parse), (Func<MasterDataTable.BattleskillDuelClipEventEffectDataPreload, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillDuelCutinPreload[] BattleskillDuelCutinPreloadList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillDuelCutinPreload>("BattleskillDuelCutinPreload", new Func<MasterDataReader, MasterDataTable.BattleskillDuelCutinPreload>(MasterDataTable.BattleskillDuelCutinPreload.Parse), (Func<MasterDataTable.BattleskillDuelCutinPreload, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillDuelEffect[] BattleskillDuelEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillDuelEffect>("BattleskillDuelEffect", new Func<MasterDataReader, MasterDataTable.BattleskillDuelEffect>(MasterDataTable.BattleskillDuelEffect.Parse), (Func<MasterDataTable.BattleskillDuelEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillDuelEffectPreload[] BattleskillDuelEffectPreloadList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillDuelEffectPreload>("BattleskillDuelEffectPreload", new Func<MasterDataReader, MasterDataTable.BattleskillDuelEffectPreload>(MasterDataTable.BattleskillDuelEffectPreload.Parse), (Func<MasterDataTable.BattleskillDuelEffectPreload, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillEffect[] BattleskillEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillEffect>("BattleskillEffect", new Func<MasterDataReader, MasterDataTable.BattleskillEffect>(MasterDataTable.BattleskillEffect.Parse), (Func<MasterDataTable.BattleskillEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillEffectLogic[] BattleskillEffectLogicList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillEffectLogic>("BattleskillEffectLogic", new Func<MasterDataReader, MasterDataTable.BattleskillEffectLogic>(MasterDataTable.BattleskillEffectLogic.Parse), (Func<MasterDataTable.BattleskillEffectLogic, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillFieldEffect[] BattleskillFieldEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillFieldEffect>("BattleskillFieldEffect", new Func<MasterDataReader, MasterDataTable.BattleskillFieldEffect>(MasterDataTable.BattleskillFieldEffect.Parse), (Func<MasterDataTable.BattleskillFieldEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillLifeCycle[] BattleskillLifeCycleList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillLifeCycle>("BattleskillLifeCycle", new Func<MasterDataReader, MasterDataTable.BattleskillLifeCycle>(MasterDataTable.BattleskillLifeCycle.Parse), (Func<MasterDataTable.BattleskillLifeCycle, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillSkill[] BattleskillSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillSkill>("BattleskillSkill", new Func<MasterDataReader, MasterDataTable.BattleskillSkill>(MasterDataTable.BattleskillSkill.Parse), (Func<MasterDataTable.BattleskillSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillTiming[] BattleskillTimingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillTiming>("BattleskillTiming", new Func<MasterDataReader, MasterDataTable.BattleskillTiming>(MasterDataTable.BattleskillTiming.Parse), (Func<MasterDataTable.BattleskillTiming, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BattleskillTimingLogic[] BattleskillTimingLogicList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BattleskillTimingLogic>("BattleskillTimingLogic", new Func<MasterDataReader, MasterDataTable.BattleskillTimingLogic>(MasterDataTable.BattleskillTimingLogic.Parse), (Func<MasterDataTable.BattleskillTimingLogic, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BeginnerNaviCategory[] BeginnerNaviCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BeginnerNaviCategory>("BeginnerNaviCategory", new Func<MasterDataReader, MasterDataTable.BeginnerNaviCategory>(MasterDataTable.BeginnerNaviCategory.Parse), (Func<MasterDataTable.BeginnerNaviCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BeginnerNaviDetail[] BeginnerNaviDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BeginnerNaviDetail>("BeginnerNaviDetail", new Func<MasterDataReader, MasterDataTable.BeginnerNaviDetail>(MasterDataTable.BeginnerNaviDetail.Parse), (Func<MasterDataTable.BeginnerNaviDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BeginnerNaviMovePage[] BeginnerNaviMovePageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BeginnerNaviMovePage>("BeginnerNaviMovePage", new Func<MasterDataReader, MasterDataTable.BeginnerNaviMovePage>(MasterDataTable.BeginnerNaviMovePage.Parse), (Func<MasterDataTable.BeginnerNaviMovePage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BeginnerNaviTitle[] BeginnerNaviTitleList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BeginnerNaviTitle>("BeginnerNaviTitle", new Func<MasterDataReader, MasterDataTable.BeginnerNaviTitle>(MasterDataTable.BeginnerNaviTitle.Parse), (Func<MasterDataTable.BeginnerNaviTitle, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BingoBingo[] BingoBingoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BingoBingo>("BingoBingo", new Func<MasterDataReader, MasterDataTable.BingoBingo>(MasterDataTable.BingoBingo.Parse), (Func<MasterDataTable.BingoBingo, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BingoMission[] BingoMissionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BingoMission>("BingoMission", new Func<MasterDataReader, MasterDataTable.BingoMission>(MasterDataTable.BingoMission.Parse), (Func<MasterDataTable.BingoMission, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BingoRewardGroup[] BingoRewardGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BingoRewardGroup>("BingoRewardGroup", new Func<MasterDataReader, MasterDataTable.BingoRewardGroup>(MasterDataTable.BingoRewardGroup.Parse), (Func<MasterDataTable.BingoRewardGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BoostBonusGearCombine[] BoostBonusGearCombineList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BoostBonusGearCombine>("BoostBonusGearCombine", new Func<MasterDataReader, MasterDataTable.BoostBonusGearCombine>(MasterDataTable.BoostBonusGearCombine.Parse), (Func<MasterDataTable.BoostBonusGearCombine, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BoostBonusGearDrilling[] BoostBonusGearDrillingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BoostBonusGearDrilling>("BoostBonusGearDrilling", new Func<MasterDataReader, MasterDataTable.BoostBonusGearDrilling>(MasterDataTable.BoostBonusGearDrilling.Parse), (Func<MasterDataTable.BoostBonusGearDrilling, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BoostBonusUnitBuildup[] BoostBonusUnitBuildupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BoostBonusUnitBuildup>("BoostBonusUnitBuildup", new Func<MasterDataReader, MasterDataTable.BoostBonusUnitBuildup>(MasterDataTable.BoostBonusUnitBuildup.Parse), (Func<MasterDataTable.BoostBonusUnitBuildup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BoostBonusUnitCompose[] BoostBonusUnitComposeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BoostBonusUnitCompose>("BoostBonusUnitCompose", new Func<MasterDataReader, MasterDataTable.BoostBonusUnitCompose>(MasterDataTable.BoostBonusUnitCompose.Parse), (Func<MasterDataTable.BoostBonusUnitCompose, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BoostBonusUnitTransmigrate[] BoostBonusUnitTransmigrateList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BoostBonusUnitTransmigrate>("BoostBonusUnitTransmigrate", new Func<MasterDataReader, MasterDataTable.BoostBonusUnitTransmigrate>(MasterDataTable.BoostBonusUnitTransmigrate.Parse), (Func<MasterDataTable.BoostBonusUnitTransmigrate, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BoostCampaignTypeName[] BoostCampaignTypeNameList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BoostCampaignTypeName>("BoostCampaignTypeName", new Func<MasterDataReader, MasterDataTable.BoostCampaignTypeName>(MasterDataTable.BoostCampaignTypeName.Parse), (Func<MasterDataTable.BoostCampaignTypeName, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BoostPeriod[] BoostPeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BoostPeriod>("BoostPeriod", new Func<MasterDataReader, MasterDataTable.BoostPeriod>(MasterDataTable.BoostPeriod.Parse), (Func<MasterDataTable.BoostPeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BoostXExperience[] BoostXExperienceList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BoostXExperience>("BoostXExperience", new Func<MasterDataReader, MasterDataTable.BoostXExperience>(MasterDataTable.BoostXExperience.Parse), (Func<MasterDataTable.BoostXExperience, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.BreakThroughBuildupSkill[] BreakThroughBuildupSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.BreakThroughBuildupSkill>("BreakThroughBuildupSkill", new Func<MasterDataReader, MasterDataTable.BreakThroughBuildupSkill>(MasterDataTable.BreakThroughBuildupSkill.Parse), (Func<MasterDataTable.BreakThroughBuildupSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallCharacter[] CallCharacterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallCharacter>("CallCharacter", new Func<MasterDataReader, MasterDataTable.CallCharacter>(MasterDataTable.CallCharacter.Parse), (Func<MasterDataTable.CallCharacter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallGaugeRate[] CallGaugeRateList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallGaugeRate>("CallGaugeRate", new Func<MasterDataReader, MasterDataTable.CallGaugeRate>(MasterDataTable.CallGaugeRate.Parse), (Func<MasterDataTable.CallGaugeRate, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallGiftRecipe[] CallGiftRecipeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallGiftRecipe>("CallGiftRecipe", new Func<MasterDataReader, MasterDataTable.CallGiftRecipe>(MasterDataTable.CallGiftRecipe.Parse), (Func<MasterDataTable.CallGiftRecipe, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallIntimateGaugeRate[] CallIntimateGaugeRateList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallIntimateGaugeRate>("CallIntimateGaugeRate", new Func<MasterDataReader, MasterDataTable.CallIntimateGaugeRate>(MasterDataTable.CallIntimateGaugeRate.Parse), (Func<MasterDataTable.CallIntimateGaugeRate, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallItem[] CallItemList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallItem>("CallItem", new Func<MasterDataReader, MasterDataTable.CallItem>(MasterDataTable.CallItem.Parse), (Func<MasterDataTable.CallItem, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallMessage[] CallMessageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallMessage>("CallMessage", new Func<MasterDataReader, MasterDataTable.CallMessage>(MasterDataTable.CallMessage.Parse), (Func<MasterDataTable.CallMessage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallMission[] CallMissionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallMission>("CallMission", new Func<MasterDataReader, MasterDataTable.CallMission>(MasterDataTable.CallMission.Parse), (Func<MasterDataTable.CallMission, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallSkillGaugeRate[] CallSkillGaugeRateList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallSkillGaugeRate>("CallSkillGaugeRate", new Func<MasterDataReader, MasterDataTable.CallSkillGaugeRate>(MasterDataTable.CallSkillGaugeRate.Parse), (Func<MasterDataTable.CallSkillGaugeRate, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CallUnitGroup[] CallUnitGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CallUnitGroup>("CallUnitGroup", new Func<MasterDataReader, MasterDataTable.CallUnitGroup>(MasterDataTable.CallUnitGroup.Parse), (Func<MasterDataTable.CallUnitGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ClassRankingHowto[] ClassRankingHowtoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ClassRankingHowto>("ClassRankingHowto", new Func<MasterDataReader, MasterDataTable.ClassRankingHowto>(MasterDataTable.ClassRankingHowto.Parse), (Func<MasterDataTable.ClassRankingHowto, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CoinChargeLimit[] CoinChargeLimitList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CoinChargeLimit>("CoinChargeLimit", new Func<MasterDataReader, MasterDataTable.CoinChargeLimit>(MasterDataTable.CoinChargeLimit.Parse), (Func<MasterDataTable.CoinChargeLimit, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CoinProduct[] CoinProductList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CoinProduct>("CoinProduct", new Func<MasterDataReader, MasterDataTable.CoinProduct>(MasterDataTable.CoinProduct.Parse), (Func<MasterDataTable.CoinProduct, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CoinProductDetail[] CoinProductDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CoinProductDetail>("CoinProductDetail", new Func<MasterDataReader, MasterDataTable.CoinProductDetail>(MasterDataTable.CoinProductDetail.Parse), (Func<MasterDataTable.CoinProductDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ColosseumBonus[] ColosseumBonusList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ColosseumBonus>("ColosseumBonus", new Func<MasterDataReader, MasterDataTable.ColosseumBonus>(MasterDataTable.ColosseumBonus.Parse), (Func<MasterDataTable.ColosseumBonus, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ColosseumBonusBloodType[] ColosseumBonusBloodTypeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ColosseumBonusBloodType>("ColosseumBonusBloodType", new Func<MasterDataReader, MasterDataTable.ColosseumBonusBloodType>(MasterDataTable.ColosseumBonusBloodType.Parse), (Func<MasterDataTable.ColosseumBonusBloodType, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ColosseumBonusZodiacType[] ColosseumBonusZodiacTypeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ColosseumBonusZodiacType>("ColosseumBonusZodiacType", new Func<MasterDataReader, MasterDataTable.ColosseumBonusZodiacType>(MasterDataTable.ColosseumBonusZodiacType.Parse), (Func<MasterDataTable.ColosseumBonusZodiacType, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ColosseumRank[] ColosseumRankList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ColosseumRank>("ColosseumRank", new Func<MasterDataReader, MasterDataTable.ColosseumRank>(MasterDataTable.ColosseumRank.Parse), (Func<MasterDataTable.ColosseumRank, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ColosseumRankReward[] ColosseumRankRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ColosseumRankReward>("ColosseumRankReward", new Func<MasterDataReader, MasterDataTable.ColosseumRankReward>(MasterDataTable.ColosseumRankReward.Parse), (Func<MasterDataTable.ColosseumRankReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ColosseumTotalVictoryReward[] ColosseumTotalVictoryRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ColosseumTotalVictoryReward>("ColosseumTotalVictoryReward", new Func<MasterDataReader, MasterDataTable.ColosseumTotalVictoryReward>(MasterDataTable.ColosseumTotalVictoryReward.Parse), (Func<MasterDataTable.ColosseumTotalVictoryReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CommonElementName[] CommonElementNameList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CommonElementName>("CommonElementName", new Func<MasterDataReader, MasterDataTable.CommonElementName>(MasterDataTable.CommonElementName.Parse), (Func<MasterDataTable.CommonElementName, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CommonMypageSetting[] CommonMypageSettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CommonMypageSetting>("CommonMypageSetting", new Func<MasterDataReader, MasterDataTable.CommonMypageSetting>(MasterDataTable.CommonMypageSetting.Parse), (Func<MasterDataTable.CommonMypageSetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CommonQuestBattleEffect[] CommonQuestBattleEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CommonQuestBattleEffect>("CommonQuestBattleEffect", new Func<MasterDataReader, MasterDataTable.CommonQuestBattleEffect>(MasterDataTable.CommonQuestBattleEffect.Parse), (Func<MasterDataTable.CommonQuestBattleEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CommonStrengthComposePrice[] CommonStrengthComposePriceList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CommonStrengthComposePrice>("CommonStrengthComposePrice", new Func<MasterDataReader, MasterDataTable.CommonStrengthComposePrice>(MasterDataTable.CommonStrengthComposePrice.Parse), (Func<MasterDataTable.CommonStrengthComposePrice, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CommonTicket[] CommonTicketList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CommonTicket>("CommonTicket", new Func<MasterDataReader, MasterDataTable.CommonTicket>(MasterDataTable.CommonTicket.Parse), (Func<MasterDataTable.CommonTicket, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CommonTicketEndAt[] CommonTicketEndAtList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CommonTicketEndAt>("CommonTicketEndAt", new Func<MasterDataReader, MasterDataTable.CommonTicketEndAt>(MasterDataTable.CommonTicketEndAt.Parse), (Func<MasterDataTable.CommonTicketEndAt, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ComposeMaxUnityValueSetting[] ComposeMaxUnityValueSettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ComposeMaxUnityValueSetting>("ComposeMaxUnityValueSetting", new Func<MasterDataReader, MasterDataTable.ComposeMaxUnityValueSetting>(MasterDataTable.ComposeMaxUnityValueSetting.Parse), (Func<MasterDataTable.ComposeMaxUnityValueSetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ConstsConsts[] ConstsConstsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ConstsConsts>("ConstsConsts", new Func<MasterDataReader, MasterDataTable.ConstsConsts>(MasterDataTable.ConstsConsts.Parse), (Func<MasterDataTable.ConstsConsts, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsCameraFilter[] CorpsCameraFilterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsCameraFilter>("CorpsCameraFilter", new Func<MasterDataReader, MasterDataTable.CorpsCameraFilter>(MasterDataTable.CorpsCameraFilter.Parse), (Func<MasterDataTable.CorpsCameraFilter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsEntryConditions[] CorpsEntryConditionsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsEntryConditions>("CorpsEntryConditions", new Func<MasterDataReader, MasterDataTable.CorpsEntryConditions>(MasterDataTable.CorpsEntryConditions.Parse), (Func<MasterDataTable.CorpsEntryConditions, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsHowto[] CorpsHowtoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsHowto>("CorpsHowto", new Func<MasterDataReader, MasterDataTable.CorpsHowto>(MasterDataTable.CorpsHowto.Parse), (Func<MasterDataTable.CorpsHowto, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsMissionReward[] CorpsMissionRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsMissionReward>("CorpsMissionReward", new Func<MasterDataReader, MasterDataTable.CorpsMissionReward>(MasterDataTable.CorpsMissionReward.Parse), (Func<MasterDataTable.CorpsMissionReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsPeriod[] CorpsPeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsPeriod>("CorpsPeriod", new Func<MasterDataReader, MasterDataTable.CorpsPeriod>(MasterDataTable.CorpsPeriod.Parse), (Func<MasterDataTable.CorpsPeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsPlaybackStory[] CorpsPlaybackStoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsPlaybackStory>("CorpsPlaybackStory", new Func<MasterDataReader, MasterDataTable.CorpsPlaybackStory>(MasterDataTable.CorpsPlaybackStory.Parse), (Func<MasterDataTable.CorpsPlaybackStory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsPlaybackStoryDetail[] CorpsPlaybackStoryDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsPlaybackStoryDetail>("CorpsPlaybackStoryDetail", new Func<MasterDataReader, MasterDataTable.CorpsPlaybackStoryDetail>(MasterDataTable.CorpsPlaybackStoryDetail.Parse), (Func<MasterDataTable.CorpsPlaybackStoryDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsSetting[] CorpsSettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsSetting>("CorpsSetting", new Func<MasterDataReader, MasterDataTable.CorpsSetting>(MasterDataTable.CorpsSetting.Parse), (Func<MasterDataTable.CorpsSetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsStage[] CorpsStageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsStage>("CorpsStage", new Func<MasterDataReader, MasterDataTable.CorpsStage>(MasterDataTable.CorpsStage.Parse), (Func<MasterDataTable.CorpsStage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsStageBackground[] CorpsStageBackgroundList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsStageBackground>("CorpsStageBackground", new Func<MasterDataReader, MasterDataTable.CorpsStageBackground>(MasterDataTable.CorpsStageBackground.Parse), (Func<MasterDataTable.CorpsStageBackground, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsStageClearReward[] CorpsStageClearRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsStageClearReward>("CorpsStageClearReward", new Func<MasterDataReader, MasterDataTable.CorpsStageClearReward>(MasterDataTable.CorpsStageClearReward.Parse), (Func<MasterDataTable.CorpsStageClearReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.CorpsStageOpenConditions[] CorpsStageOpenConditionsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.CorpsStageOpenConditions>("CorpsStageOpenConditions", new Func<MasterDataReader, MasterDataTable.CorpsStageOpenConditions>(MasterDataTable.CorpsStageOpenConditions.Parse), (Func<MasterDataTable.CorpsStageOpenConditions, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DailyMission[] DailyMissionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DailyMission>("DailyMission", new Func<MasterDataReader, MasterDataTable.DailyMission>(MasterDataTable.DailyMission.Parse), (Func<MasterDataTable.DailyMission, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DailyMissionTopPage[] DailyMissionTopPageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DailyMissionTopPage>("DailyMissionTopPage", new Func<MasterDataReader, MasterDataTable.DailyMissionTopPage>(MasterDataTable.DailyMissionTopPage.Parse), (Func<MasterDataTable.DailyMissionTopPage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DateScriptBase[] DateScriptBaseList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DateScriptBase>("DateScriptBase", new Func<MasterDataReader, MasterDataTable.DateScriptBase>(MasterDataTable.DateScriptBase.Parse), (Func<MasterDataTable.DateScriptBase, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DateScriptParts[] DateScriptPartsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DateScriptParts>("DateScriptParts", new Func<MasterDataReader, MasterDataTable.DateScriptParts>(MasterDataTable.DateScriptParts.Parse), (Func<MasterDataTable.DateScriptParts, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DateScriptQuestion[] DateScriptQuestionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DateScriptQuestion>("DateScriptQuestion", new Func<MasterDataReader, MasterDataTable.DateScriptQuestion>(MasterDataTable.DateScriptQuestion.Parse), (Func<MasterDataTable.DateScriptQuestion, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DuelDuelConfig[] DuelDuelConfigList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DuelDuelConfig>("DuelDuelConfig", new Func<MasterDataReader, MasterDataTable.DuelDuelConfig>(MasterDataTable.DuelDuelConfig.Parse), (Func<MasterDataTable.DuelDuelConfig, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DuelEffectPreload[] DuelEffectPreloadList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DuelEffectPreload>("DuelEffectPreload", new Func<MasterDataReader, MasterDataTable.DuelEffectPreload>(MasterDataTable.DuelEffectPreload.Parse), (Func<MasterDataTable.DuelEffectPreload, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DuelElementBulletEffect[] DuelElementBulletEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DuelElementBulletEffect>("DuelElementBulletEffect", new Func<MasterDataReader, MasterDataTable.DuelElementBulletEffect>(MasterDataTable.DuelElementBulletEffect.Parse), (Func<MasterDataTable.DuelElementBulletEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DuelElementHitEffect[] DuelElementHitEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DuelElementHitEffect>("DuelElementHitEffect", new Func<MasterDataReader, MasterDataTable.DuelElementHitEffect>(MasterDataTable.DuelElementHitEffect.Parse), (Func<MasterDataTable.DuelElementHitEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.DuelElementTrailEffect[] DuelElementTrailEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.DuelElementTrailEffect>("DuelElementTrailEffect", new Func<MasterDataReader, MasterDataTable.DuelElementTrailEffect>(MasterDataTable.DuelElementTrailEffect.Parse), (Func<MasterDataTable.DuelElementTrailEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthAwakeSkillCategory[] EarthAwakeSkillCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthAwakeSkillCategory>("EarthAwakeSkillCategory", new Func<MasterDataReader, MasterDataTable.EarthAwakeSkillCategory>(MasterDataTable.EarthAwakeSkillCategory.Parse), (Func<MasterDataTable.EarthAwakeSkillCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthBattleStagePanelEvent[] EarthBattleStagePanelEventList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthBattleStagePanelEvent>("EarthBattleStagePanelEvent", new Func<MasterDataReader, MasterDataTable.EarthBattleStagePanelEvent>(MasterDataTable.EarthBattleStagePanelEvent.Parse), (Func<MasterDataTable.EarthBattleStagePanelEvent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthDesertCharacter[] EarthDesertCharacterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthDesertCharacter>("EarthDesertCharacter", new Func<MasterDataReader, MasterDataTable.EarthDesertCharacter>(MasterDataTable.EarthDesertCharacter.Parse), (Func<MasterDataTable.EarthDesertCharacter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthExtraQuest[] EarthExtraQuestList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthExtraQuest>("EarthExtraQuest", new Func<MasterDataReader, MasterDataTable.EarthExtraQuest>(MasterDataTable.EarthExtraQuest.Parse), (Func<MasterDataTable.EarthExtraQuest, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthForcedSortieCharacter[] EarthForcedSortieCharacterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthForcedSortieCharacter>("EarthForcedSortieCharacter", new Func<MasterDataReader, MasterDataTable.EarthForcedSortieCharacter>(MasterDataTable.EarthForcedSortieCharacter.Parse), (Func<MasterDataTable.EarthForcedSortieCharacter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthImpossibleOFSortieCharacter[] EarthImpossibleOFSortieCharacterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthImpossibleOFSortieCharacter>("EarthImpossibleOFSortieCharacter", new Func<MasterDataReader, MasterDataTable.EarthImpossibleOFSortieCharacter>(MasterDataTable.EarthImpossibleOFSortieCharacter.Parse), (Func<MasterDataTable.EarthImpossibleOFSortieCharacter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthJoinCharacter[] EarthJoinCharacterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthJoinCharacter>("EarthJoinCharacter", new Func<MasterDataReader, MasterDataTable.EarthJoinCharacter>(MasterDataTable.EarthJoinCharacter.Parse), (Func<MasterDataTable.EarthJoinCharacter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthQuestChapter[] EarthQuestChapterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthQuestChapter>("EarthQuestChapter", new Func<MasterDataReader, MasterDataTable.EarthQuestChapter>(MasterDataTable.EarthQuestChapter.Parse), (Func<MasterDataTable.EarthQuestChapter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthQuestClearReward[] EarthQuestClearRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthQuestClearReward>("EarthQuestClearReward", new Func<MasterDataReader, MasterDataTable.EarthQuestClearReward>(MasterDataTable.EarthQuestClearReward.Parse), (Func<MasterDataTable.EarthQuestClearReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthQuestEpisode[] EarthQuestEpisodeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthQuestEpisode>("EarthQuestEpisode", new Func<MasterDataReader, MasterDataTable.EarthQuestEpisode>(MasterDataTable.EarthQuestEpisode.Parse), (Func<MasterDataTable.EarthQuestEpisode, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthQuestExtraStoryPlayback[] EarthQuestExtraStoryPlaybackList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthQuestExtraStoryPlayback>("EarthQuestExtraStoryPlayback", new Func<MasterDataReader, MasterDataTable.EarthQuestExtraStoryPlayback>(MasterDataTable.EarthQuestExtraStoryPlayback.Parse), (Func<MasterDataTable.EarthQuestExtraStoryPlayback, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthQuestKey[] EarthQuestKeyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthQuestKey>("EarthQuestKey", new Func<MasterDataReader, MasterDataTable.EarthQuestKey>(MasterDataTable.EarthQuestKey.Parse), (Func<MasterDataTable.EarthQuestKey, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthQuestPologue[] EarthQuestPologueList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthQuestPologue>("EarthQuestPologue", new Func<MasterDataReader, MasterDataTable.EarthQuestPologue>(MasterDataTable.EarthQuestPologue.Parse), (Func<MasterDataTable.EarthQuestPologue, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthQuestStoryPlayback[] EarthQuestStoryPlaybackList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthQuestStoryPlayback>("EarthQuestStoryPlayback", new Func<MasterDataReader, MasterDataTable.EarthQuestStoryPlayback>(MasterDataTable.EarthQuestStoryPlayback.Parse), (Func<MasterDataTable.EarthQuestStoryPlayback, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthShopArticle[] EarthShopArticleList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthShopArticle>("EarthShopArticle", new Func<MasterDataReader, MasterDataTable.EarthShopArticle>(MasterDataTable.EarthShopArticle.Parse), (Func<MasterDataTable.EarthShopArticle, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EarthShopContent[] EarthShopContentList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EarthShopContent>("EarthShopContent", new Func<MasterDataReader, MasterDataTable.EarthShopContent>(MasterDataTable.EarthShopContent.Parse), (Func<MasterDataTable.EarthShopContent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EmblemEmblem[] EmblemEmblemList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EmblemEmblem>("EmblemEmblem", new Func<MasterDataReader, MasterDataTable.EmblemEmblem>(MasterDataTable.EmblemEmblem.Parse), (Func<MasterDataTable.EmblemEmblem, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.EmblemRarity[] EmblemRarityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.EmblemRarity>("EmblemRarity", new Func<MasterDataReader, MasterDataTable.EmblemRarity>(MasterDataTable.EmblemRarity.Parse), (Func<MasterDataTable.EmblemRarity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreCommonAnimation[] ExploreCommonAnimationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreCommonAnimation>("ExploreCommonAnimation", new Func<MasterDataReader, MasterDataTable.ExploreCommonAnimation>(MasterDataTable.ExploreCommonAnimation.Parse), (Func<MasterDataTable.ExploreCommonAnimation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreDropReward[] ExploreDropRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreDropReward>("ExploreDropReward", new Func<MasterDataReader, MasterDataTable.ExploreDropReward>(MasterDataTable.ExploreDropReward.Parse), (Func<MasterDataTable.ExploreDropReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreDropTable[] ExploreDropTableList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreDropTable>("ExploreDropTable", new Func<MasterDataReader, MasterDataTable.ExploreDropTable>(MasterDataTable.ExploreDropTable.Parse), (Func<MasterDataTable.ExploreDropTable, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreEnemy[] ExploreEnemyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreEnemy>("ExploreEnemy", new Func<MasterDataReader, MasterDataTable.ExploreEnemy>(MasterDataTable.ExploreEnemy.Parse), (Func<MasterDataTable.ExploreEnemy, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreFloor[] ExploreFloorList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreFloor>("ExploreFloor", new Func<MasterDataReader, MasterDataTable.ExploreFloor>(MasterDataTable.ExploreFloor.Parse), (Func<MasterDataTable.ExploreFloor, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreFloorReward[] ExploreFloorRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreFloorReward>("ExploreFloorReward", new Func<MasterDataReader, MasterDataTable.ExploreFloorReward>(MasterDataTable.ExploreFloorReward.Parse), (Func<MasterDataTable.ExploreFloorReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreRankingCondition[] ExploreRankingConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreRankingCondition>("ExploreRankingCondition", new Func<MasterDataReader, MasterDataTable.ExploreRankingCondition>(MasterDataTable.ExploreRankingCondition.Parse), (Func<MasterDataTable.ExploreRankingCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreRankingPeriod[] ExploreRankingPeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreRankingPeriod>("ExploreRankingPeriod", new Func<MasterDataReader, MasterDataTable.ExploreRankingPeriod>(MasterDataTable.ExploreRankingPeriod.Parse), (Func<MasterDataTable.ExploreRankingPeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreRankingReward[] ExploreRankingRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreRankingReward>("ExploreRankingReward", new Func<MasterDataReader, MasterDataTable.ExploreRankingReward>(MasterDataTable.ExploreRankingReward.Parse), (Func<MasterDataTable.ExploreRankingReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ExploreTimeConfig[] ExploreTimeConfigList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ExploreTimeConfig>("ExploreTimeConfig", new Func<MasterDataReader, MasterDataTable.ExploreTimeConfig>(MasterDataTable.ExploreTimeConfig.Parse), (Func<MasterDataTable.ExploreTimeConfig, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.FacilityLevel[] FacilityLevelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.FacilityLevel>("FacilityLevel", new Func<MasterDataReader, MasterDataTable.FacilityLevel>(MasterDataTable.FacilityLevel.Parse), (Func<MasterDataTable.FacilityLevel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.FacilitySkillGroup[] FacilitySkillGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.FacilitySkillGroup>("FacilitySkillGroup", new Func<MasterDataReader, MasterDataTable.FacilitySkillGroup>(MasterDataTable.FacilitySkillGroup.Parse), (Func<MasterDataTable.FacilitySkillGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GachaTicket[] GachaTicketList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GachaTicket>("GachaTicket", new Func<MasterDataReader, MasterDataTable.GachaTicket>(MasterDataTable.GachaTicket.Parse), (Func<MasterDataTable.GachaTicket, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GachaTutorial[] GachaTutorialList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GachaTutorial>("GachaTutorial", new Func<MasterDataReader, MasterDataTable.GachaTutorial>(MasterDataTable.GachaTutorial.Parse), (Func<MasterDataTable.GachaTutorial, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GachaTutorialBanner[] GachaTutorialBannerList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GachaTutorialBanner>("GachaTutorialBanner", new Func<MasterDataReader, MasterDataTable.GachaTutorialBanner>(MasterDataTable.GachaTutorialBanner.Parse), (Func<MasterDataTable.GachaTutorialBanner, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GachaTutorialDeck[] GachaTutorialDeckList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GachaTutorialDeck>("GachaTutorialDeck", new Func<MasterDataReader, MasterDataTable.GachaTutorialDeck>(MasterDataTable.GachaTutorialDeck.Parse), (Func<MasterDataTable.GachaTutorialDeck, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GachaTutorialDeckEntity[] GachaTutorialDeckEntityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GachaTutorialDeckEntity>("GachaTutorialDeckEntity", new Func<MasterDataReader, MasterDataTable.GachaTutorialDeckEntity>(MasterDataTable.GachaTutorialDeckEntity.Parse), (Func<MasterDataTable.GachaTutorialDeckEntity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GachaTutorialFixedEntity[] GachaTutorialFixedEntityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GachaTutorialFixedEntity>("GachaTutorialFixedEntity", new Func<MasterDataReader, MasterDataTable.GachaTutorialFixedEntity>(MasterDataTable.GachaTutorialFixedEntity.Parse), (Func<MasterDataTable.GachaTutorialFixedEntity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GachaTutorialPeriod[] GachaTutorialPeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GachaTutorialPeriod>("GachaTutorialPeriod", new Func<MasterDataReader, MasterDataTable.GachaTutorialPeriod>(MasterDataTable.GachaTutorialPeriod.Parse), (Func<MasterDataTable.GachaTutorialPeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GachaTutorialbutton[] GachaTutorialbuttonList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GachaTutorialbutton>("GachaTutorialbutton", new Func<MasterDataReader, MasterDataTable.GachaTutorialbutton>(MasterDataTable.GachaTutorialbutton.Parse), (Func<MasterDataTable.GachaTutorialbutton, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearAttachedElement[] GearAttachedElementList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearAttachedElement>("GearAttachedElement", new Func<MasterDataReader, MasterDataTable.GearAttachedElement>(MasterDataTable.GearAttachedElement.Parse), (Func<MasterDataTable.GearAttachedElement, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearAttackClassificationTable[] GearAttackClassificationTableList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearAttackClassificationTable>("GearAttackClassificationTable", new Func<MasterDataReader, MasterDataTable.GearAttackClassificationTable>(MasterDataTable.GearAttackClassificationTable.Parse), (Func<MasterDataTable.GearAttackClassificationTable, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearBuildup[] GearBuildupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearBuildup>("GearBuildup", new Func<MasterDataReader, MasterDataTable.GearBuildup>(MasterDataTable.GearBuildup.Parse), (Func<MasterDataTable.GearBuildup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearBuildupLogic[] GearBuildupLogicList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearBuildupLogic>("GearBuildupLogic", new Func<MasterDataReader, MasterDataTable.GearBuildupLogic>(MasterDataTable.GearBuildupLogic.Parse), (Func<MasterDataTable.GearBuildupLogic, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearClassificationPattern[] GearClassificationPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearClassificationPattern>("GearClassificationPattern", new Func<MasterDataReader, MasterDataTable.GearClassificationPattern>(MasterDataTable.GearClassificationPattern.Parse), (Func<MasterDataTable.GearClassificationPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearCombineRecipe[] GearCombineRecipeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearCombineRecipe>("GearCombineRecipe", new Func<MasterDataReader, MasterDataTable.GearCombineRecipe>(MasterDataTable.GearCombineRecipe.Parse), (Func<MasterDataTable.GearCombineRecipe, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearDisappearanceType[] GearDisappearanceTypeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearDisappearanceType>("GearDisappearanceType", new Func<MasterDataReader, MasterDataTable.GearDisappearanceType>(MasterDataTable.GearDisappearanceType.Parse), (Func<MasterDataTable.GearDisappearanceType, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearDrilling[] GearDrillingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearDrilling>("GearDrilling", new Func<MasterDataReader, MasterDataTable.GearDrilling>(MasterDataTable.GearDrilling.Parse), (Func<MasterDataTable.GearDrilling, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearDrillingExpMythology[] GearDrillingExpMythologyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearDrillingExpMythology>("GearDrillingExpMythology", new Func<MasterDataReader, MasterDataTable.GearDrillingExpMythology>(MasterDataTable.GearDrillingExpMythology.Parse), (Func<MasterDataTable.GearDrillingExpMythology, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearElementRatio[] GearElementRatioList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearElementRatio>("GearElementRatio", new Func<MasterDataReader, MasterDataTable.GearElementRatio>(MasterDataTable.GearElementRatio.Parse), (Func<MasterDataTable.GearElementRatio, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearExpireDate[] GearExpireDateList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearExpireDate>("GearExpireDate", new Func<MasterDataReader, MasterDataTable.GearExpireDate>(MasterDataTable.GearExpireDate.Parse), (Func<MasterDataTable.GearExpireDate, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearExtensionExclusion[] GearExtensionExclusionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearExtensionExclusion>("GearExtensionExclusion", new Func<MasterDataReader, MasterDataTable.GearExtensionExclusion>(MasterDataTable.GearExtensionExclusion.Parse), (Func<MasterDataTable.GearExtensionExclusion, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearExtensionItem[] GearExtensionItemList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearExtensionItem>("GearExtensionItem", new Func<MasterDataReader, MasterDataTable.GearExtensionItem>(MasterDataTable.GearExtensionItem.Parse), (Func<MasterDataTable.GearExtensionItem, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearExtensionUnity[] GearExtensionUnityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearExtensionUnity>("GearExtensionUnity", new Func<MasterDataReader, MasterDataTable.GearExtensionUnity>(MasterDataTable.GearExtensionUnity.Parse), (Func<MasterDataTable.GearExtensionUnity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearGear[] GearGearList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearGear>("GearGear", new Func<MasterDataReader, MasterDataTable.GearGear>(MasterDataTable.GearGear.Parse), (Func<MasterDataTable.GearGear, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearGearComposeParameter[] GearGearComposeParameterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearGearComposeParameter>("GearGearComposeParameter", new Func<MasterDataReader, MasterDataTable.GearGearComposeParameter>(MasterDataTable.GearGearComposeParameter.Parse), (Func<MasterDataTable.GearGearComposeParameter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearGearDescription[] GearGearDescriptionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearGearDescription>("GearGearDescription", new Func<MasterDataReader, MasterDataTable.GearGearDescription>(MasterDataTable.GearGearDescription.Parse), (Func<MasterDataTable.GearGearDescription, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearGearElement[] GearGearElementList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearGearElement>("GearGearElement", new Func<MasterDataReader, MasterDataTable.GearGearElement>(MasterDataTable.GearGearElement.Parse), (Func<MasterDataTable.GearGearElement, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearGearSkill[] GearGearSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearGearSkill>("GearGearSkill", new Func<MasterDataReader, MasterDataTable.GearGearSkill>(MasterDataTable.GearGearSkill.Parse), (Func<MasterDataTable.GearGearSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearKind[] GearKindList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearKind>("GearKind", new Func<MasterDataReader, MasterDataTable.GearKind>(MasterDataTable.GearKind.Parse), (Func<MasterDataTable.GearKind, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearKindCorrelations[] GearKindCorrelationsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearKindCorrelations>("GearKindCorrelations", new Func<MasterDataReader, MasterDataTable.GearKindCorrelations>(MasterDataTable.GearKindCorrelations.Parse), (Func<MasterDataTable.GearKindCorrelations, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearKindIncr[] GearKindIncrList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearKindIncr>("GearKindIncr", new Func<MasterDataReader, MasterDataTable.GearKindIncr>(MasterDataTable.GearKindIncr.Parse), (Func<MasterDataTable.GearKindIncr, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearKindRatio[] GearKindRatioList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearKindRatio>("GearKindRatio", new Func<MasterDataReader, MasterDataTable.GearKindRatio>(MasterDataTable.GearKindRatio.Parse), (Func<MasterDataTable.GearKindRatio, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearMaterialQuestInfo[] GearMaterialQuestInfoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearMaterialQuestInfo>("GearMaterialQuestInfo", new Func<MasterDataReader, MasterDataTable.GearMaterialQuestInfo>(MasterDataTable.GearMaterialQuestInfo.Parse), (Func<MasterDataTable.GearMaterialQuestInfo, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearModelKind[] GearModelKindList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearModelKind>("GearModelKind", new Func<MasterDataReader, MasterDataTable.GearModelKind>(MasterDataTable.GearModelKind.Parse), (Func<MasterDataTable.GearModelKind, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearRank[] GearRankList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearRank>("GearRank", new Func<MasterDataReader, MasterDataTable.GearRank>(MasterDataTable.GearRank.Parse), (Func<MasterDataTable.GearRank, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearRankExp[] GearRankExpList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearRankExp>("GearRankExp", new Func<MasterDataReader, MasterDataTable.GearRankExp>(MasterDataTable.GearRankExp.Parse), (Func<MasterDataTable.GearRankExp, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearRankIncr[] GearRankIncrList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearRankIncr>("GearRankIncr", new Func<MasterDataReader, MasterDataTable.GearRankIncr>(MasterDataTable.GearRankIncr.Parse), (Func<MasterDataTable.GearRankIncr, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearRarity[] GearRarityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearRarity>("GearRarity", new Func<MasterDataReader, MasterDataTable.GearRarity>(MasterDataTable.GearRarity.Parse), (Func<MasterDataTable.GearRarity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearReisouChaosCreation[] GearReisouChaosCreationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearReisouChaosCreation>("GearReisouChaosCreation", new Func<MasterDataReader, MasterDataTable.GearReisouChaosCreation>(MasterDataTable.GearReisouChaosCreation.Parse), (Func<MasterDataTable.GearReisouChaosCreation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearReisouFusion[] GearReisouFusionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearReisouFusion>("GearReisouFusion", new Func<MasterDataReader, MasterDataTable.GearReisouFusion>(MasterDataTable.GearReisouFusion.Parse), (Func<MasterDataTable.GearReisouFusion, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearReisouSkill[] GearReisouSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearReisouSkill>("GearReisouSkill", new Func<MasterDataReader, MasterDataTable.GearReisouSkill>(MasterDataTable.GearReisouSkill.Parse), (Func<MasterDataTable.GearReisouSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearReisouSkillWeaponGroup[] GearReisouSkillWeaponGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearReisouSkillWeaponGroup>("GearReisouSkillWeaponGroup", new Func<MasterDataReader, MasterDataTable.GearReisouSkillWeaponGroup>(MasterDataTable.GearReisouSkillWeaponGroup.Parse), (Func<MasterDataTable.GearReisouSkillWeaponGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearSpecialDrillingCost[] GearSpecialDrillingCostList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearSpecialDrillingCost>("GearSpecialDrillingCost", new Func<MasterDataReader, MasterDataTable.GearSpecialDrillingCost>(MasterDataTable.GearSpecialDrillingCost.Parse), (Func<MasterDataTable.GearSpecialDrillingCost, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearSpecificationOfEquipmentUnit[] GearSpecificationOfEquipmentUnitList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearSpecificationOfEquipmentUnit>("GearSpecificationOfEquipmentUnit", new Func<MasterDataReader, MasterDataTable.GearSpecificationOfEquipmentUnit>(MasterDataTable.GearSpecificationOfEquipmentUnit.Parse), (Func<MasterDataTable.GearSpecificationOfEquipmentUnit, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GearValuables[] GearValuablesList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GearValuables>("GearValuables", new Func<MasterDataReader, MasterDataTable.GearValuables>(MasterDataTable.GearValuables.Parse), (Func<MasterDataTable.GearValuables, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildApprovalPolicy[] GuildApprovalPolicyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildApprovalPolicy>("GuildApprovalPolicy", new Func<MasterDataReader, MasterDataTable.GuildApprovalPolicy>(MasterDataTable.GuildApprovalPolicy.Parse), (Func<MasterDataTable.GuildApprovalPolicy, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildAtmosphere[] GuildAtmosphereList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildAtmosphere>("GuildAtmosphere", new Func<MasterDataReader, MasterDataTable.GuildAtmosphere>(MasterDataTable.GuildAtmosphere.Parse), (Func<MasterDataTable.GuildAtmosphere, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildAutoApproval[] GuildAutoApprovalList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildAutoApproval>("GuildAutoApproval", new Func<MasterDataReader, MasterDataTable.GuildAutoApproval>(MasterDataTable.GuildAutoApproval.Parse), (Func<MasterDataTable.GuildAutoApproval, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildAutokick[] GuildAutokickList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildAutokick>("GuildAutokick", new Func<MasterDataReader, MasterDataTable.GuildAutokick>(MasterDataTable.GuildAutokick.Parse), (Func<MasterDataTable.GuildAutokick, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildAvailability[] GuildAvailabilityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildAvailability>("GuildAvailability", new Func<MasterDataReader, MasterDataTable.GuildAvailability>(MasterDataTable.GuildAvailability.Parse), (Func<MasterDataTable.GuildAvailability, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildBankEvent[] GuildBankEventList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildBankEvent>("GuildBankEvent", new Func<MasterDataReader, MasterDataTable.GuildBankEvent>(MasterDataTable.GuildBankEvent.Parse), (Func<MasterDataTable.GuildBankEvent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildBankHowto[] GuildBankHowtoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildBankHowto>("GuildBankHowto", new Func<MasterDataReader, MasterDataTable.GuildBankHowto>(MasterDataTable.GuildBankHowto.Parse), (Func<MasterDataTable.GuildBankHowto, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildBase[] GuildBaseList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildBase>("GuildBase", new Func<MasterDataReader, MasterDataTable.GuildBase>(MasterDataTable.GuildBase.Parse), (Func<MasterDataTable.GuildBase, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildBaseAnimation[] GuildBaseAnimationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildBaseAnimation>("GuildBaseAnimation", new Func<MasterDataReader, MasterDataTable.GuildBaseAnimation>(MasterDataTable.GuildBaseAnimation.Parse), (Func<MasterDataTable.GuildBaseAnimation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildBaseBonus[] GuildBaseBonusList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildBaseBonus>("GuildBaseBonus", new Func<MasterDataReader, MasterDataTable.GuildBaseBonus>(MasterDataTable.GuildBaseBonus.Parse), (Func<MasterDataTable.GuildBaseBonus, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildBasePos[] GuildBasePosList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildBasePos>("GuildBasePos", new Func<MasterDataReader, MasterDataTable.GuildBasePos>(MasterDataTable.GuildBasePos.Parse), (Func<MasterDataTable.GuildBasePos, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildEmblemRarity[] GuildEmblemRarityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildEmblemRarity>("GuildEmblemRarity", new Func<MasterDataReader, MasterDataTable.GuildEmblemRarity>(MasterDataTable.GuildEmblemRarity.Parse), (Func<MasterDataTable.GuildEmblemRarity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildEmblemUnit[] GuildEmblemUnitList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildEmblemUnit>("GuildEmblemUnit", new Func<MasterDataReader, MasterDataTable.GuildEmblemUnit>(MasterDataTable.GuildEmblemUnit.Parse), (Func<MasterDataTable.GuildEmblemUnit, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildGiftEvent[] GuildGiftEventList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildGiftEvent>("GuildGiftEvent", new Func<MasterDataReader, MasterDataTable.GuildGiftEvent>(MasterDataTable.GuildGiftEvent.Parse), (Func<MasterDataTable.GuildGiftEvent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildImagePattern[] GuildImagePatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildImagePattern>("GuildImagePattern", new Func<MasterDataReader, MasterDataTable.GuildImagePattern>(MasterDataTable.GuildImagePattern.Parse), (Func<MasterDataTable.GuildImagePattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildMission[] GuildMissionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildMission>("GuildMission", new Func<MasterDataReader, MasterDataTable.GuildMission>(MasterDataTable.GuildMission.Parse), (Func<MasterDataTable.GuildMission, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaid[] GuildRaidList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaid>("GuildRaid", new Func<MasterDataReader, MasterDataTable.GuildRaid>(MasterDataTable.GuildRaid.Parse), (Func<MasterDataTable.GuildRaid, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidDamageReward[] GuildRaidDamageRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidDamageReward>("GuildRaidDamageReward", new Func<MasterDataReader, MasterDataTable.GuildRaidDamageReward>(MasterDataTable.GuildRaidDamageReward.Parse), (Func<MasterDataTable.GuildRaidDamageReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidDamageRewardSet[] GuildRaidDamageRewardSetList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidDamageRewardSet>("GuildRaidDamageRewardSet", new Func<MasterDataReader, MasterDataTable.GuildRaidDamageRewardSet>(MasterDataTable.GuildRaidDamageRewardSet.Parse), (Func<MasterDataTable.GuildRaidDamageRewardSet, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidEndless[] GuildRaidEndlessList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidEndless>("GuildRaidEndless", new Func<MasterDataReader, MasterDataTable.GuildRaidEndless>(MasterDataTable.GuildRaidEndless.Parse), (Func<MasterDataTable.GuildRaidEndless, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidEndlessKillReward[] GuildRaidEndlessKillRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidEndlessKillReward>("GuildRaidEndlessKillReward", new Func<MasterDataReader, MasterDataTable.GuildRaidEndlessKillReward>(MasterDataTable.GuildRaidEndlessKillReward.Parse), (Func<MasterDataTable.GuildRaidEndlessKillReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidGuildDamageRankingReward[] GuildRaidGuildDamageRankingRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidGuildDamageRankingReward>("GuildRaidGuildDamageRankingReward", new Func<MasterDataReader, MasterDataTable.GuildRaidGuildDamageRankingReward>(MasterDataTable.GuildRaidGuildDamageRankingReward.Parse), (Func<MasterDataTable.GuildRaidGuildDamageRankingReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidGuildDamageRankingRewardExtra[] GuildRaidGuildDamageRankingRewardExtraList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidGuildDamageRankingRewardExtra>("GuildRaidGuildDamageRankingRewardExtra", new Func<MasterDataReader, MasterDataTable.GuildRaidGuildDamageRankingRewardExtra>(MasterDataTable.GuildRaidGuildDamageRankingRewardExtra.Parse), (Func<MasterDataTable.GuildRaidGuildDamageRankingRewardExtra, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidHowto[] GuildRaidHowtoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidHowto>("GuildRaidHowto", new Func<MasterDataReader, MasterDataTable.GuildRaidHowto>(MasterDataTable.GuildRaidHowto.Parse), (Func<MasterDataTable.GuildRaidHowto, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidKillReward[] GuildRaidKillRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidKillReward>("GuildRaidKillReward", new Func<MasterDataReader, MasterDataTable.GuildRaidKillReward>(MasterDataTable.GuildRaidKillReward.Parse), (Func<MasterDataTable.GuildRaidKillReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidKillRewardSet[] GuildRaidKillRewardSetList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidKillRewardSet>("GuildRaidKillRewardSet", new Func<MasterDataReader, MasterDataTable.GuildRaidKillRewardSet>(MasterDataTable.GuildRaidKillRewardSet.Parse), (Func<MasterDataTable.GuildRaidKillRewardSet, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidPeriod[] GuildRaidPeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidPeriod>("GuildRaidPeriod", new Func<MasterDataReader, MasterDataTable.GuildRaidPeriod>(MasterDataTable.GuildRaidPeriod.Parse), (Func<MasterDataTable.GuildRaidPeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidPersonalDamageRankingReward[] GuildRaidPersonalDamageRankingRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidPersonalDamageRankingReward>("GuildRaidPersonalDamageRankingReward", new Func<MasterDataReader, MasterDataTable.GuildRaidPersonalDamageRankingReward>(MasterDataTable.GuildRaidPersonalDamageRankingReward.Parse), (Func<MasterDataTable.GuildRaidPersonalDamageRankingReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidRankingRewardCondition[] GuildRaidRankingRewardConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidRankingRewardCondition>("GuildRaidRankingRewardCondition", new Func<MasterDataReader, MasterDataTable.GuildRaidRankingRewardCondition>(MasterDataTable.GuildRaidRankingRewardCondition.Parse), (Func<MasterDataTable.GuildRaidRankingRewardCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRaidSettings[] GuildRaidSettingsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRaidSettings>("GuildRaidSettings", new Func<MasterDataReader, MasterDataTable.GuildRaidSettings>(MasterDataTable.GuildRaidSettings.Parse), (Func<MasterDataTable.GuildRaidSettings, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildRoleName[] GuildRoleNameList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildRoleName>("GuildRoleName", new Func<MasterDataReader, MasterDataTable.GuildRoleName>(MasterDataTable.GuildRoleName.Parse), (Func<MasterDataTable.GuildRoleName, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildSetting[] GuildSettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildSetting>("GuildSetting", new Func<MasterDataReader, MasterDataTable.GuildSetting>(MasterDataTable.GuildSetting.Parse), (Func<MasterDataTable.GuildSetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildStamp[] GuildStampList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildStamp>("GuildStamp", new Func<MasterDataReader, MasterDataTable.GuildStamp>(MasterDataTable.GuildStamp.Parse), (Func<MasterDataTable.GuildStamp, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GuildStampGroup[] GuildStampGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GuildStampGroup>("GuildStampGroup", new Func<MasterDataReader, MasterDataTable.GuildStampGroup>(MasterDataTable.GuildStampGroup.Parse), (Func<MasterDataTable.GuildStampGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GvgPeriod[] GvgPeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GvgPeriod>("GvgPeriod", new Func<MasterDataReader, MasterDataTable.GvgPeriod>(MasterDataTable.GvgPeriod.Parse), (Func<MasterDataTable.GvgPeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GvgRule[] GvgRuleList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GvgRule>("GvgRule", new Func<MasterDataReader, MasterDataTable.GvgRule>(MasterDataTable.GvgRule.Parse), (Func<MasterDataTable.GvgRule, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GvgSettings[] GvgSettingsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GvgSettings>("GvgSettings", new Func<MasterDataReader, MasterDataTable.GvgSettings>(MasterDataTable.GvgSettings.Parse), (Func<MasterDataTable.GvgSettings, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GvgStageFormation[] GvgStageFormationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GvgStageFormation>("GvgStageFormation", new Func<MasterDataReader, MasterDataTable.GvgStageFormation>(MasterDataTable.GvgStageFormation.Parse), (Func<MasterDataTable.GvgStageFormation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.GvgStarCondition[] GvgStarConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.GvgStarCondition>("GvgStarCondition", new Func<MasterDataReader, MasterDataTable.GvgStarCondition>(MasterDataTable.GvgStarCondition.Parse), (Func<MasterDataTable.GvgStarCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.HelpCategory[] HelpCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.HelpCategory>("HelpCategory", new Func<MasterDataReader, MasterDataTable.HelpCategory>(MasterDataTable.HelpCategory.Parse), (Func<MasterDataTable.HelpCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.HelpHelp[] HelpHelpList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.HelpHelp>("HelpHelp", new Func<MasterDataReader, MasterDataTable.HelpHelp>(MasterDataTable.HelpHelp.Parse), (Func<MasterDataTable.HelpHelp, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.HotdealPack[] HotdealPackList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.HotdealPack>("HotdealPack", new Func<MasterDataReader, MasterDataTable.HotdealPack>(MasterDataTable.HotdealPack.Parse), (Func<MasterDataTable.HotdealPack, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.IgnoreGear[] IgnoreGearList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.IgnoreGear>("IgnoreGear", new Func<MasterDataReader, MasterDataTable.IgnoreGear>(MasterDataTable.IgnoreGear.Parse), (Func<MasterDataTable.IgnoreGear, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.IgnoreOverkillers[] IgnoreOverkillersList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.IgnoreOverkillers>("IgnoreOverkillers", new Func<MasterDataReader, MasterDataTable.IgnoreOverkillers>(MasterDataTable.IgnoreOverkillers.Parse), (Func<MasterDataTable.IgnoreOverkillers, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.InformationCategory[] InformationCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.InformationCategory>("InformationCategory", new Func<MasterDataReader, MasterDataTable.InformationCategory>(MasterDataTable.InformationCategory.Parse), (Func<MasterDataTable.InformationCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.InformationInformation[] InformationInformationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.InformationInformation>("InformationInformation", new Func<MasterDataReader, MasterDataTable.InformationInformation>(MasterDataTable.InformationInformation.Parse), (Func<MasterDataTable.InformationInformation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.InformationSubCategory[] InformationSubCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.InformationSubCategory>("InformationSubCategory", new Func<MasterDataReader, MasterDataTable.InformationSubCategory>(MasterDataTable.InformationSubCategory.Parse), (Func<MasterDataTable.InformationSubCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.InitimateBreakthrough[] InitimateBreakthroughList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.InitimateBreakthrough>("InitimateBreakthrough", new Func<MasterDataReader, MasterDataTable.InitimateBreakthrough>(MasterDataTable.InitimateBreakthrough.Parse), (Func<MasterDataTable.InitimateBreakthrough, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.InitimateLevel[] InitimateLevelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.InitimateLevel>("InitimateLevel", new Func<MasterDataReader, MasterDataTable.InitimateLevel>(MasterDataTable.InitimateLevel.Parse), (Func<MasterDataTable.InitimateLevel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.IntimateDuelSupport[] IntimateDuelSupportList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.IntimateDuelSupport>("IntimateDuelSupport", new Func<MasterDataReader, MasterDataTable.IntimateDuelSupport>(MasterDataTable.IntimateDuelSupport.Parse), (Func<MasterDataTable.IntimateDuelSupport, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.InvitationInvitation[] InvitationInvitationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.InvitationInvitation>("InvitationInvitation", new Func<MasterDataReader, MasterDataTable.InvitationInvitation>(MasterDataTable.InvitationInvitation.Parse), (Func<MasterDataTable.InvitationInvitation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.JobChangeMaterials[] JobChangeMaterialsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.JobChangeMaterials>("JobChangeMaterials", new Func<MasterDataReader, MasterDataTable.JobChangeMaterials>(MasterDataTable.JobChangeMaterials.Parse), (Func<MasterDataTable.JobChangeMaterials, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.JobChangePatterns[] JobChangePatternsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.JobChangePatterns>("JobChangePatterns", new Func<MasterDataReader, MasterDataTable.JobChangePatterns>(MasterDataTable.JobChangePatterns.Parse), (Func<MasterDataTable.JobChangePatterns, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.JobCharacteristics[] JobCharacteristicsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.JobCharacteristics>("JobCharacteristics", new Func<MasterDataReader, MasterDataTable.JobCharacteristics>(MasterDataTable.JobCharacteristics.Parse), (Func<MasterDataTable.JobCharacteristics, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.JobCharacteristicsLevelupPattern[] JobCharacteristicsLevelupPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.JobCharacteristicsLevelupPattern>("JobCharacteristicsLevelupPattern", new Func<MasterDataReader, MasterDataTable.JobCharacteristicsLevelupPattern>(MasterDataTable.JobCharacteristicsLevelupPattern.Parse), (Func<MasterDataTable.JobCharacteristicsLevelupPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.JobMaterialGroup[] JobMaterialGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.JobMaterialGroup>("JobMaterialGroup", new Func<MasterDataReader, MasterDataTable.JobMaterialGroup>(MasterDataTable.JobMaterialGroup.Parse), (Func<MasterDataTable.JobMaterialGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.JobMaterialUsed[] JobMaterialUsedList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.JobMaterialUsed>("JobMaterialUsed", new Func<MasterDataReader, MasterDataTable.JobMaterialUsed>(MasterDataTable.JobMaterialUsed.Parse), (Func<MasterDataTable.JobMaterialUsed, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.LoginbonusLoginbonus[] LoginbonusLoginbonusList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.LoginbonusLoginbonus>("LoginbonusLoginbonus", new Func<MasterDataReader, MasterDataTable.LoginbonusLoginbonus>(MasterDataTable.LoginbonusLoginbonus.Parse), (Func<MasterDataTable.LoginbonusLoginbonus, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.LoginbonusReward[] LoginbonusRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.LoginbonusReward>("LoginbonusReward", new Func<MasterDataReader, MasterDataTable.LoginbonusReward>(MasterDataTable.LoginbonusReward.Parse), (Func<MasterDataTable.LoginbonusReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.MapEditFacilityShaderSetting[] MapEditFacilityShaderSettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.MapEditFacilityShaderSetting>("MapEditFacilityShaderSetting", new Func<MasterDataReader, MasterDataTable.MapEditFacilityShaderSetting>(MasterDataTable.MapEditFacilityShaderSetting.Parse), (Func<MasterDataTable.MapEditFacilityShaderSetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.MapFacility[] MapFacilityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.MapFacility>("MapFacility", new Func<MasterDataReader, MasterDataTable.MapFacility>(MasterDataTable.MapFacility.Parse), (Func<MasterDataTable.MapFacility, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.MapTown[] MapTownList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.MapTown>("MapTown", new Func<MasterDataReader, MasterDataTable.MapTown>(MasterDataTable.MapTown.Parse), (Func<MasterDataTable.MapTown, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.MaterialXLevelExp[] MaterialXLevelExpList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.MaterialXLevelExp>("MaterialXLevelExp", new Func<MasterDataReader, MasterDataTable.MaterialXLevelExp>(MasterDataTable.MaterialXLevelExp.Parse), (Func<MasterDataTable.MaterialXLevelExp, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.Music[] MusicList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.Music>("Music", new Func<MasterDataReader, MasterDataTable.Music>(MasterDataTable.Music.Parse), (Func<MasterDataTable.Music, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.OverkillersGroup[] OverkillersGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.OverkillersGroup>("OverkillersGroup", new Func<MasterDataReader, MasterDataTable.OverkillersGroup>(MasterDataTable.OverkillersGroup.Parse), (Func<MasterDataTable.OverkillersGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.OverkillersMaterial[] OverkillersMaterialList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.OverkillersMaterial>("OverkillersMaterial", new Func<MasterDataReader, MasterDataTable.OverkillersMaterial>(MasterDataTable.OverkillersMaterial.Parse), (Func<MasterDataTable.OverkillersMaterial, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.OverkillersParameter[] OverkillersParameterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.OverkillersParameter>("OverkillersParameter", new Func<MasterDataReader, MasterDataTable.OverkillersParameter>(MasterDataTable.OverkillersParameter.Parse), (Func<MasterDataTable.OverkillersParameter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.OverkillersSkillRelease[] OverkillersSkillReleaseList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.OverkillersSkillRelease>("OverkillersSkillRelease", new Func<MasterDataReader, MasterDataTable.OverkillersSkillRelease>(MasterDataTable.OverkillersSkillRelease.Parse), (Func<MasterDataTable.OverkillersSkillRelease, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.OverkillersSlotRelease[] OverkillersSlotReleaseList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.OverkillersSlotRelease>("OverkillersSlotRelease", new Func<MasterDataReader, MasterDataTable.OverkillersSlotRelease>(MasterDataTable.OverkillersSlotRelease.Parse), (Func<MasterDataTable.OverkillersSlotRelease, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PeriodBackground[] PeriodBackgroundList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PeriodBackground>("PeriodBackground", new Func<MasterDataReader, MasterDataTable.PeriodBackground>(MasterDataTable.PeriodBackground.Parse), (Func<MasterDataTable.PeriodBackground, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PlayerLevelUpStatus[] PlayerLevelUpStatusList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PlayerLevelUpStatus>("PlayerLevelUpStatus", new Func<MasterDataReader, MasterDataTable.PlayerLevelUpStatus>(MasterDataTable.PlayerLevelUpStatus.Parse), (Func<MasterDataTable.PlayerLevelUpStatus, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PointReward[] PointRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PointReward>("PointReward", new Func<MasterDataReader, MasterDataTable.PointReward>(MasterDataTable.PointReward.Parse), (Func<MasterDataTable.PointReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PointRewardBox[] PointRewardBoxList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PointRewardBox>("PointRewardBox", new Func<MasterDataReader, MasterDataTable.PointRewardBox>(MasterDataTable.PointRewardBox.Parse), (Func<MasterDataTable.PointRewardBox, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PunitiveExpeditionEventGuildReward[] PunitiveExpeditionEventGuildRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PunitiveExpeditionEventGuildReward>("PunitiveExpeditionEventGuildReward", new Func<MasterDataReader, MasterDataTable.PunitiveExpeditionEventGuildReward>(MasterDataTable.PunitiveExpeditionEventGuildReward.Parse), (Func<MasterDataTable.PunitiveExpeditionEventGuildReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PunitiveExpeditionEventReward[] PunitiveExpeditionEventRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PunitiveExpeditionEventReward>("PunitiveExpeditionEventReward", new Func<MasterDataReader, MasterDataTable.PunitiveExpeditionEventReward>(MasterDataTable.PunitiveExpeditionEventReward.Parse), (Func<MasterDataTable.PunitiveExpeditionEventReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpBonus[] PvpBonusList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpBonus>("PvpBonus", new Func<MasterDataReader, MasterDataTable.PvpBonus>(MasterDataTable.PvpBonus.Parse), (Func<MasterDataTable.PvpBonus, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpClassKind[] PvpClassKindList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpClassKind>("PvpClassKind", new Func<MasterDataReader, MasterDataTable.PvpClassKind>(MasterDataTable.PvpClassKind.Parse), (Func<MasterDataTable.PvpClassKind, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpClassRankingReward[] PvpClassRankingRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpClassRankingReward>("PvpClassRankingReward", new Func<MasterDataReader, MasterDataTable.PvpClassRankingReward>(MasterDataTable.PvpClassRankingReward.Parse), (Func<MasterDataTable.PvpClassRankingReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpClassReward[] PvpClassRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpClassReward>("PvpClassReward", new Func<MasterDataReader, MasterDataTable.PvpClassReward>(MasterDataTable.PvpClassReward.Parse), (Func<MasterDataTable.PvpClassReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpMatchingType[] PvpMatchingTypeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpMatchingType>("PvpMatchingType", new Func<MasterDataReader, MasterDataTable.PvpMatchingType>(MasterDataTable.PvpMatchingType.Parse), (Func<MasterDataTable.PvpMatchingType, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpRankingCondition[] PvpRankingConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpRankingCondition>("PvpRankingCondition", new Func<MasterDataReader, MasterDataTable.PvpRankingCondition>(MasterDataTable.PvpRankingCondition.Parse), (Func<MasterDataTable.PvpRankingCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpRankingKind[] PvpRankingKindList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpRankingKind>("PvpRankingKind", new Func<MasterDataReader, MasterDataTable.PvpRankingKind>(MasterDataTable.PvpRankingKind.Parse), (Func<MasterDataTable.PvpRankingKind, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpRulePeriod[] PvpRulePeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpRulePeriod>("PvpRulePeriod", new Func<MasterDataReader, MasterDataTable.PvpRulePeriod>(MasterDataTable.PvpRulePeriod.Parse), (Func<MasterDataTable.PvpRulePeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpSettings[] PvpSettingsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpSettings>("PvpSettings", new Func<MasterDataReader, MasterDataTable.PvpSettings>(MasterDataTable.PvpSettings.Parse), (Func<MasterDataTable.PvpSettings, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpStageFormation[] PvpStageFormationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpStageFormation>("PvpStageFormation", new Func<MasterDataReader, MasterDataTable.PvpStageFormation>(MasterDataTable.PvpStageFormation.Parse), (Func<MasterDataTable.PvpStageFormation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.PvpVictoryEffect[] PvpVictoryEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.PvpVictoryEffect>("PvpVictoryEffect", new Func<MasterDataReader, MasterDataTable.PvpVictoryEffect>(MasterDataTable.PvpVictoryEffect.Parse), (Func<MasterDataTable.PvpVictoryEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCharacterDisplayCondition[] QuestCharacterDisplayConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCharacterDisplayCondition>("QuestCharacterDisplayCondition", new Func<MasterDataReader, MasterDataTable.QuestCharacterDisplayCondition>(MasterDataTable.QuestCharacterDisplayCondition.Parse), (Func<MasterDataTable.QuestCharacterDisplayCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCharacterLimitation[] QuestCharacterLimitationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCharacterLimitation>("QuestCharacterLimitation", new Func<MasterDataReader, MasterDataTable.QuestCharacterLimitation>(MasterDataTable.QuestCharacterLimitation.Parse), (Func<MasterDataTable.QuestCharacterLimitation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCharacterLimitationLabel[] QuestCharacterLimitationLabelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCharacterLimitationLabel>("QuestCharacterLimitationLabel", new Func<MasterDataReader, MasterDataTable.QuestCharacterLimitationLabel>(MasterDataTable.QuestCharacterLimitationLabel.Parse), (Func<MasterDataTable.QuestCharacterLimitationLabel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCharacterM[] QuestCharacterMList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCharacterM>("QuestCharacterM", new Func<MasterDataReader, MasterDataTable.QuestCharacterM>(MasterDataTable.QuestCharacterM.Parse), (Func<MasterDataTable.QuestCharacterM, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCharacterMReleaseCondition[] QuestCharacterMReleaseConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCharacterMReleaseCondition>("QuestCharacterMReleaseCondition", new Func<MasterDataReader, MasterDataTable.QuestCharacterMReleaseCondition>(MasterDataTable.QuestCharacterMReleaseCondition.Parse), (Func<MasterDataTable.QuestCharacterMReleaseCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCharacterReleaseCondition[] QuestCharacterReleaseConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCharacterReleaseCondition>("QuestCharacterReleaseCondition", new Func<MasterDataReader, MasterDataTable.QuestCharacterReleaseCondition>(MasterDataTable.QuestCharacterReleaseCondition.Parse), (Func<MasterDataTable.QuestCharacterReleaseCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCharacterS[] QuestCharacterSList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCharacterS>("QuestCharacterS", new Func<MasterDataReader, MasterDataTable.QuestCharacterS>(MasterDataTable.QuestCharacterS.Parse), (Func<MasterDataTable.QuestCharacterS, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCommonBackground[] QuestCommonBackgroundList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCommonBackground>("QuestCommonBackground", new Func<MasterDataReader, MasterDataTable.QuestCommonBackground>(MasterDataTable.QuestCommonBackground.Parse), (Func<MasterDataTable.QuestCommonBackground, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCommonChapterBG[] QuestCommonChapterBGList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCommonChapterBG>("QuestCommonChapterBG", new Func<MasterDataReader, MasterDataTable.QuestCommonChapterBG>(MasterDataTable.QuestCommonChapterBG.Parse), (Func<MasterDataTable.QuestCommonChapterBG, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCommonDrop[] QuestCommonDropList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCommonDrop>("QuestCommonDrop", new Func<MasterDataReader, MasterDataTable.QuestCommonDrop>(MasterDataTable.QuestCommonDrop.Parse), (Func<MasterDataTable.QuestCommonDrop, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCommonJogDecoration[] QuestCommonJogDecorationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCommonJogDecoration>("QuestCommonJogDecoration", new Func<MasterDataReader, MasterDataTable.QuestCommonJogDecoration>(MasterDataTable.QuestCommonJogDecoration.Parse), (Func<MasterDataTable.QuestCommonJogDecoration, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestCommonSpecialColor[] QuestCommonSpecialColorList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestCommonSpecialColor>("QuestCommonSpecialColor", new Func<MasterDataReader, MasterDataTable.QuestCommonSpecialColor>(MasterDataTable.QuestCommonSpecialColor.Parse), (Func<MasterDataTable.QuestCommonSpecialColor, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraCategory[] QuestExtraCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraCategory>("QuestExtraCategory", new Func<MasterDataReader, MasterDataTable.QuestExtraCategory>(MasterDataTable.QuestExtraCategory.Parse), (Func<MasterDataTable.QuestExtraCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraDescription[] QuestExtraDescriptionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraDescription>("QuestExtraDescription", new Func<MasterDataReader, MasterDataTable.QuestExtraDescription>(MasterDataTable.QuestExtraDescription.Parse), (Func<MasterDataTable.QuestExtraDescription, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraEntryConditions[] QuestExtraEntryConditionsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraEntryConditions>("QuestExtraEntryConditions", new Func<MasterDataReader, MasterDataTable.QuestExtraEntryConditions>(MasterDataTable.QuestExtraEntryConditions.Parse), (Func<MasterDataTable.QuestExtraEntryConditions, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraL[] QuestExtraLList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraL>("QuestExtraL", new Func<MasterDataReader, MasterDataTable.QuestExtraL>(MasterDataTable.QuestExtraL.Parse), (Func<MasterDataTable.QuestExtraL, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraLL[] QuestExtraLLList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraLL>("QuestExtraLL", new Func<MasterDataReader, MasterDataTable.QuestExtraLL>(MasterDataTable.QuestExtraLL.Parse), (Func<MasterDataTable.QuestExtraLL, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraLimitation[] QuestExtraLimitationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraLimitation>("QuestExtraLimitation", new Func<MasterDataReader, MasterDataTable.QuestExtraLimitation>(MasterDataTable.QuestExtraLimitation.Parse), (Func<MasterDataTable.QuestExtraLimitation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraLimitationLabel[] QuestExtraLimitationLabelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraLimitationLabel>("QuestExtraLimitationLabel", new Func<MasterDataReader, MasterDataTable.QuestExtraLimitationLabel>(MasterDataTable.QuestExtraLimitationLabel.Parse), (Func<MasterDataTable.QuestExtraLimitationLabel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraM[] QuestExtraMList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraM>("QuestExtraM", new Func<MasterDataReader, MasterDataTable.QuestExtraM>(MasterDataTable.QuestExtraM.Parse), (Func<MasterDataTable.QuestExtraM, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraMission[] QuestExtraMissionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraMission>("QuestExtraMission", new Func<MasterDataReader, MasterDataTable.QuestExtraMission>(MasterDataTable.QuestExtraMission.Parse), (Func<MasterDataTable.QuestExtraMission, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraReleaseConditionsPlayer[] QuestExtraReleaseConditionsPlayerList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraReleaseConditionsPlayer>("QuestExtraReleaseConditionsPlayer", new Func<MasterDataReader, MasterDataTable.QuestExtraReleaseConditionsPlayer>(MasterDataTable.QuestExtraReleaseConditionsPlayer.Parse), (Func<MasterDataTable.QuestExtraReleaseConditionsPlayer, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraS[] QuestExtraSList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraS>("QuestExtraS", new Func<MasterDataReader, MasterDataTable.QuestExtraS>(MasterDataTable.QuestExtraS.Parse), (Func<MasterDataTable.QuestExtraS, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraScoreAchivementReward[] QuestExtraScoreAchivementRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraScoreAchivementReward>("QuestExtraScoreAchivementReward", new Func<MasterDataReader, MasterDataTable.QuestExtraScoreAchivementReward>(MasterDataTable.QuestExtraScoreAchivementReward.Parse), (Func<MasterDataTable.QuestExtraScoreAchivementReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraScoreRankingReward[] QuestExtraScoreRankingRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraScoreRankingReward>("QuestExtraScoreRankingReward", new Func<MasterDataReader, MasterDataTable.QuestExtraScoreRankingReward>(MasterDataTable.QuestExtraScoreRankingReward.Parse), (Func<MasterDataTable.QuestExtraScoreRankingReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestExtraTotalScoreReward[] QuestExtraTotalScoreRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestExtraTotalScoreReward>("QuestExtraTotalScoreReward", new Func<MasterDataReader, MasterDataTable.QuestExtraTotalScoreReward>(MasterDataTable.QuestExtraTotalScoreReward.Parse), (Func<MasterDataTable.QuestExtraTotalScoreReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestHarmonyDisplayCondition[] QuestHarmonyDisplayConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestHarmonyDisplayCondition>("QuestHarmonyDisplayCondition", new Func<MasterDataReader, MasterDataTable.QuestHarmonyDisplayCondition>(MasterDataTable.QuestHarmonyDisplayCondition.Parse), (Func<MasterDataTable.QuestHarmonyDisplayCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestHarmonyLimitation[] QuestHarmonyLimitationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestHarmonyLimitation>("QuestHarmonyLimitation", new Func<MasterDataReader, MasterDataTable.QuestHarmonyLimitation>(MasterDataTable.QuestHarmonyLimitation.Parse), (Func<MasterDataTable.QuestHarmonyLimitation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestHarmonyLimitationLabel[] QuestHarmonyLimitationLabelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestHarmonyLimitationLabel>("QuestHarmonyLimitationLabel", new Func<MasterDataReader, MasterDataTable.QuestHarmonyLimitationLabel>(MasterDataTable.QuestHarmonyLimitationLabel.Parse), (Func<MasterDataTable.QuestHarmonyLimitationLabel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestHarmonyM[] QuestHarmonyMList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestHarmonyM>("QuestHarmonyM", new Func<MasterDataReader, MasterDataTable.QuestHarmonyM>(MasterDataTable.QuestHarmonyM.Parse), (Func<MasterDataTable.QuestHarmonyM, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestHarmonyReleaseCondition[] QuestHarmonyReleaseConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestHarmonyReleaseCondition>("QuestHarmonyReleaseCondition", new Func<MasterDataReader, MasterDataTable.QuestHarmonyReleaseCondition>(MasterDataTable.QuestHarmonyReleaseCondition.Parse), (Func<MasterDataTable.QuestHarmonyReleaseCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestHarmonyS[] QuestHarmonySList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestHarmonyS>("QuestHarmonyS", new Func<MasterDataReader, MasterDataTable.QuestHarmonyS>(MasterDataTable.QuestHarmonyS.Parse), (Func<MasterDataTable.QuestHarmonyS, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestMoviePath[] QuestMoviePathList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestMoviePath>("QuestMoviePath", new Func<MasterDataReader, MasterDataTable.QuestMoviePath>(MasterDataTable.QuestMoviePath.Parse), (Func<MasterDataTable.QuestMoviePath, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestMovieQuest[] QuestMovieQuestList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestMovieQuest>("QuestMovieQuest", new Func<MasterDataReader, MasterDataTable.QuestMovieQuest>(MasterDataTable.QuestMovieQuest.Parse), (Func<MasterDataTable.QuestMovieQuest, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaClearMessage[] QuestSeaClearMessageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaClearMessage>("QuestSeaClearMessage", new Func<MasterDataReader, MasterDataTable.QuestSeaClearMessage>(MasterDataTable.QuestSeaClearMessage.Parse), (Func<MasterDataTable.QuestSeaClearMessage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaL[] QuestSeaLList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaL>("QuestSeaL", new Func<MasterDataReader, MasterDataTable.QuestSeaL>(MasterDataTable.QuestSeaL.Parse), (Func<MasterDataTable.QuestSeaL, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaLimitation[] QuestSeaLimitationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaLimitation>("QuestSeaLimitation", new Func<MasterDataReader, MasterDataTable.QuestSeaLimitation>(MasterDataTable.QuestSeaLimitation.Parse), (Func<MasterDataTable.QuestSeaLimitation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaLimitationLabel[] QuestSeaLimitationLabelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaLimitationLabel>("QuestSeaLimitationLabel", new Func<MasterDataReader, MasterDataTable.QuestSeaLimitationLabel>(MasterDataTable.QuestSeaLimitationLabel.Parse), (Func<MasterDataTable.QuestSeaLimitationLabel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaM[] QuestSeaMList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaM>("QuestSeaM", new Func<MasterDataReader, MasterDataTable.QuestSeaM>(MasterDataTable.QuestSeaM.Parse), (Func<MasterDataTable.QuestSeaM, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaMission[] QuestSeaMissionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaMission>("QuestSeaMission", new Func<MasterDataReader, MasterDataTable.QuestSeaMission>(MasterDataTable.QuestSeaMission.Parse), (Func<MasterDataTable.QuestSeaMission, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaMissionReward[] QuestSeaMissionRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaMissionReward>("QuestSeaMissionReward", new Func<MasterDataReader, MasterDataTable.QuestSeaMissionReward>(MasterDataTable.QuestSeaMissionReward.Parse), (Func<MasterDataTable.QuestSeaMissionReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaS[] QuestSeaSList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaS>("QuestSeaS", new Func<MasterDataReader, MasterDataTable.QuestSeaS>(MasterDataTable.QuestSeaS.Parse), (Func<MasterDataTable.QuestSeaS, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestSeaXL[] QuestSeaXLList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestSeaXL>("QuestSeaXL", new Func<MasterDataReader, MasterDataTable.QuestSeaXL>(MasterDataTable.QuestSeaXL.Parse), (Func<MasterDataTable.QuestSeaXL, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryClearMessage[] QuestStoryClearMessageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryClearMessage>("QuestStoryClearMessage", new Func<MasterDataReader, MasterDataTable.QuestStoryClearMessage>(MasterDataTable.QuestStoryClearMessage.Parse), (Func<MasterDataTable.QuestStoryClearMessage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryL[] QuestStoryLList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryL>("QuestStoryL", new Func<MasterDataReader, MasterDataTable.QuestStoryL>(MasterDataTable.QuestStoryL.Parse), (Func<MasterDataTable.QuestStoryL, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryLimitation[] QuestStoryLimitationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryLimitation>("QuestStoryLimitation", new Func<MasterDataReader, MasterDataTable.QuestStoryLimitation>(MasterDataTable.QuestStoryLimitation.Parse), (Func<MasterDataTable.QuestStoryLimitation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryLimitationLabel[] QuestStoryLimitationLabelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryLimitationLabel>("QuestStoryLimitationLabel", new Func<MasterDataReader, MasterDataTable.QuestStoryLimitationLabel>(MasterDataTable.QuestStoryLimitationLabel.Parse), (Func<MasterDataTable.QuestStoryLimitationLabel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryM[] QuestStoryMList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryM>("QuestStoryM", new Func<MasterDataReader, MasterDataTable.QuestStoryM>(MasterDataTable.QuestStoryM.Parse), (Func<MasterDataTable.QuestStoryM, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryMission[] QuestStoryMissionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryMission>("QuestStoryMission", new Func<MasterDataReader, MasterDataTable.QuestStoryMission>(MasterDataTable.QuestStoryMission.Parse), (Func<MasterDataTable.QuestStoryMission, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryMissionReward[] QuestStoryMissionRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryMissionReward>("QuestStoryMissionReward", new Func<MasterDataReader, MasterDataTable.QuestStoryMissionReward>(MasterDataTable.QuestStoryMissionReward.Parse), (Func<MasterDataTable.QuestStoryMissionReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryS[] QuestStorySList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryS>("QuestStoryS", new Func<MasterDataReader, MasterDataTable.QuestStoryS>(MasterDataTable.QuestStoryS.Parse), (Func<MasterDataTable.QuestStoryS, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestStoryXL[] QuestStoryXLList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestStoryXL>("QuestStoryXL", new Func<MasterDataReader, MasterDataTable.QuestStoryXL>(MasterDataTable.QuestStoryXL.Parse), (Func<MasterDataTable.QuestStoryXL, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestWave[] QuestWaveList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestWave>("QuestWave", new Func<MasterDataReader, MasterDataTable.QuestWave>(MasterDataTable.QuestWave.Parse), (Func<MasterDataTable.QuestWave, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestkeyCondition[] QuestkeyConditionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestkeyCondition>("QuestkeyCondition", new Func<MasterDataReader, MasterDataTable.QuestkeyCondition>(MasterDataTable.QuestkeyCondition.Parse), (Func<MasterDataTable.QuestkeyCondition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.QuestkeyQuestkey[] QuestkeyQuestkeyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.QuestkeyQuestkey>("QuestkeyQuestkey", new Func<MasterDataReader, MasterDataTable.QuestkeyQuestkey>(MasterDataTable.QuestkeyQuestkey.Parse), (Func<MasterDataTable.QuestkeyQuestkey, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.RaidPlaybackStory[] RaidPlaybackStoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.RaidPlaybackStory>("RaidPlaybackStory", new Func<MasterDataReader, MasterDataTable.RaidPlaybackStory>(MasterDataTable.RaidPlaybackStory.Parse), (Func<MasterDataTable.RaidPlaybackStory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.RecoveryItemAPHeal[] RecoveryItemAPHealList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.RecoveryItemAPHeal>("RecoveryItemAPHeal", new Func<MasterDataReader, MasterDataTable.RecoveryItemAPHeal>(MasterDataTable.RecoveryItemAPHeal.Parse), (Func<MasterDataTable.RecoveryItemAPHeal, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ReisouDrilling[] ReisouDrillingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ReisouDrilling>("ReisouDrilling", new Func<MasterDataReader, MasterDataTable.ReisouDrilling>(MasterDataTable.ReisouDrilling.Parse), (Func<MasterDataTable.ReisouDrilling, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ReisouRankExp[] ReisouRankExpList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ReisouRankExp>("ReisouRankExp", new Func<MasterDataReader, MasterDataTable.ReisouRankExp>(MasterDataTable.ReisouRankExp.Parse), (Func<MasterDataTable.ReisouRankExp, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ReisouRankIncr[] ReisouRankIncrList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ReisouRankIncr>("ReisouRankIncr", new Func<MasterDataReader, MasterDataTable.ReisouRankIncr>(MasterDataTable.ReisouRankIncr.Parse), (Func<MasterDataTable.ReisouRankIncr, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ReviewReward[] ReviewRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ReviewReward>("ReviewReward", new Func<MasterDataReader, MasterDataTable.ReviewReward>(MasterDataTable.ReviewReward.Parse), (Func<MasterDataTable.ReviewReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.RouletteR001FreeAnimationPattern[] RouletteR001FreeAnimationPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.RouletteR001FreeAnimationPattern>("RouletteR001FreeAnimationPattern", new Func<MasterDataReader, MasterDataTable.RouletteR001FreeAnimationPattern>(MasterDataTable.RouletteR001FreeAnimationPattern.Parse), (Func<MasterDataTable.RouletteR001FreeAnimationPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.RouletteR001FreeDeckEntity[] RouletteR001FreeDeckEntityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.RouletteR001FreeDeckEntity>("RouletteR001FreeDeckEntity", new Func<MasterDataReader, MasterDataTable.RouletteR001FreeDeckEntity>(MasterDataTable.RouletteR001FreeDeckEntity.Parse), (Func<MasterDataTable.RouletteR001FreeDeckEntity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.RouletteR001FreePeriod[] RouletteR001FreePeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.RouletteR001FreePeriod>("RouletteR001FreePeriod", new Func<MasterDataReader, MasterDataTable.RouletteR001FreePeriod>(MasterDataTable.RouletteR001FreePeriod.Parse), (Func<MasterDataTable.RouletteR001FreePeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.RouletteR001FreeRoulette[] RouletteR001FreeRouletteList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.RouletteR001FreeRoulette>("RouletteR001FreeRoulette", new Func<MasterDataReader, MasterDataTable.RouletteR001FreeRoulette>(MasterDataTable.RouletteR001FreeRoulette.Parse), (Func<MasterDataTable.RouletteR001FreeRoulette, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.RouletteR001FreelDeck[] RouletteR001FreelDeckList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.RouletteR001FreelDeck>("RouletteR001FreelDeck", new Func<MasterDataReader, MasterDataTable.RouletteR001FreelDeck>(MasterDataTable.RouletteR001FreelDeck.Parse), (Func<MasterDataTable.RouletteR001FreelDeck, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ScriptScript[] ScriptScriptList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ScriptScript>("ScriptScript", new Func<MasterDataReader, MasterDataTable.ScriptScript>(MasterDataTable.ScriptScript.Parse), (Func<MasterDataTable.ScriptScript, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaAlbum[] SeaAlbumList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaAlbum>("SeaAlbum", new Func<MasterDataReader, MasterDataTable.SeaAlbum>(MasterDataTable.SeaAlbum.Parse), (Func<MasterDataTable.SeaAlbum, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaAlbumPiece[] SeaAlbumPieceList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaAlbumPiece>("SeaAlbumPiece", new Func<MasterDataReader, MasterDataTable.SeaAlbumPiece>(MasterDataTable.SeaAlbumPiece.Parse), (Func<MasterDataTable.SeaAlbumPiece, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaAlbumRewardGroup[] SeaAlbumRewardGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaAlbumRewardGroup>("SeaAlbumRewardGroup", new Func<MasterDataReader, MasterDataTable.SeaAlbumRewardGroup>(MasterDataTable.SeaAlbumRewardGroup.Parse), (Func<MasterDataTable.SeaAlbumRewardGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaDateDateSpot[] SeaDateDateSpotList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaDateDateSpot>("SeaDateDateSpot", new Func<MasterDataReader, MasterDataTable.SeaDateDateSpot>(MasterDataTable.SeaDateDateSpot.Parse), (Func<MasterDataTable.SeaDateDateSpot, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaDateDateSpotDisplaySetting[] SeaDateDateSpotDisplaySettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaDateDateSpotDisplaySetting>("SeaDateDateSpotDisplaySetting", new Func<MasterDataReader, MasterDataTable.SeaDateDateSpotDisplaySetting>(MasterDataTable.SeaDateDateSpotDisplaySetting.Parse), (Func<MasterDataTable.SeaDateDateSpotDisplaySetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaDateHitExpansionLottery[] SeaDateHitExpansionLotteryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaDateHitExpansionLottery>("SeaDateHitExpansionLottery", new Func<MasterDataReader, MasterDataTable.SeaDateHitExpansionLottery>(MasterDataTable.SeaDateHitExpansionLottery.Parse), (Func<MasterDataTable.SeaDateHitExpansionLottery, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaDateProhibition[] SeaDateProhibitionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaDateProhibition>("SeaDateProhibition", new Func<MasterDataReader, MasterDataTable.SeaDateProhibition>(MasterDataTable.SeaDateProhibition.Parse), (Func<MasterDataTable.SeaDateProhibition, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaDateResult[] SeaDateResultList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaDateResult>("SeaDateResult", new Func<MasterDataReader, MasterDataTable.SeaDateResult>(MasterDataTable.SeaDateResult.Parse), (Func<MasterDataTable.SeaDateResult, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaDateResultStaging[] SeaDateResultStagingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaDateResultStaging>("SeaDateResultStaging", new Func<MasterDataReader, MasterDataTable.SeaDateResultStaging>(MasterDataTable.SeaDateResultStaging.Parse), (Func<MasterDataTable.SeaDateResultStaging, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaDateSerifAtDepart[] SeaDateSerifAtDepartList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaDateSerifAtDepart>("SeaDateSerifAtDepart", new Func<MasterDataReader, MasterDataTable.SeaDateSerifAtDepart>(MasterDataTable.SeaDateSerifAtDepart.Parse), (Func<MasterDataTable.SeaDateSerifAtDepart, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaHomeMap[] SeaHomeMapList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaHomeMap>("SeaHomeMap", new Func<MasterDataReader, MasterDataTable.SeaHomeMap>(MasterDataTable.SeaHomeMap.Parse), (Func<MasterDataTable.SeaHomeMap, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaHomeResult[] SeaHomeResultList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaHomeResult>("SeaHomeResult", new Func<MasterDataReader, MasterDataTable.SeaHomeResult>(MasterDataTable.SeaHomeResult.Parse), (Func<MasterDataTable.SeaHomeResult, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaHomeSerif[] SeaHomeSerifList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaHomeSerif>("SeaHomeSerif", new Func<MasterDataReader, MasterDataTable.SeaHomeSerif>(MasterDataTable.SeaHomeSerif.Parse), (Func<MasterDataTable.SeaHomeSerif, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaHomeTimeZone[] SeaHomeTimeZoneList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaHomeTimeZone>("SeaHomeTimeZone", new Func<MasterDataReader, MasterDataTable.SeaHomeTimeZone>(MasterDataTable.SeaHomeTimeZone.Parse), (Func<MasterDataTable.SeaHomeTimeZone, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaHomeTrustProvisions[] SeaHomeTrustProvisionsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaHomeTrustProvisions>("SeaHomeTrustProvisions", new Func<MasterDataReader, MasterDataTable.SeaHomeTrustProvisions>(MasterDataTable.SeaHomeTrustProvisions.Parse), (Func<MasterDataTable.SeaHomeTrustProvisions, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaPresentAffinity[] SeaPresentAffinityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaPresentAffinity>("SeaPresentAffinity", new Func<MasterDataReader, MasterDataTable.SeaPresentAffinity>(MasterDataTable.SeaPresentAffinity.Parse), (Func<MasterDataTable.SeaPresentAffinity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaPresentPresent[] SeaPresentPresentList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaPresentPresent>("SeaPresentPresent", new Func<MasterDataReader, MasterDataTable.SeaPresentPresent>(MasterDataTable.SeaPresentPresent.Parse), (Func<MasterDataTable.SeaPresentPresent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaPresentPresentAffinity[] SeaPresentPresentAffinityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaPresentPresentAffinity>("SeaPresentPresentAffinity", new Func<MasterDataReader, MasterDataTable.SeaPresentPresentAffinity>(MasterDataTable.SeaPresentPresentAffinity.Parse), (Func<MasterDataTable.SeaPresentPresentAffinity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeaPresentPresentResult[] SeaPresentPresentResultList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeaPresentPresentResult>("SeaPresentPresentResult", new Func<MasterDataReader, MasterDataTable.SeaPresentPresentResult>(MasterDataTable.SeaPresentPresentResult.Parse), (Func<MasterDataTable.SeaPresentPresentResult, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SeasonTicketSeasonTicket[] SeasonTicketSeasonTicketList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SeasonTicketSeasonTicket>("SeasonTicketSeasonTicket", new Func<MasterDataReader, MasterDataTable.SeasonTicketSeasonTicket>(MasterDataTable.SeasonTicketSeasonTicket.Parse), (Func<MasterDataTable.SeasonTicketSeasonTicket, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SelectTicket[] SelectTicketList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SelectTicket>("SelectTicket", new Func<MasterDataReader, MasterDataTable.SelectTicket>(MasterDataTable.SelectTicket.Parse), (Func<MasterDataTable.SelectTicket, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SelectTicketSelectSample[] SelectTicketSelectSampleList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SelectTicketSelectSample>("SelectTicketSelectSample", new Func<MasterDataReader, MasterDataTable.SelectTicketSelectSample>(MasterDataTable.SelectTicketSelectSample.Parse), (Func<MasterDataTable.SelectTicketSelectSample, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ShopArticle[] ShopArticleList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ShopArticle>("ShopArticle", new Func<MasterDataReader, MasterDataTable.ShopArticle>(MasterDataTable.ShopArticle.Parse), (Func<MasterDataTable.ShopArticle, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ShopContent[] ShopContentList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ShopContent>("ShopContent", new Func<MasterDataReader, MasterDataTable.ShopContent>(MasterDataTable.ShopContent.Parse), (Func<MasterDataTable.ShopContent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ShopShop[] ShopShopList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ShopShop>("ShopShop", new Func<MasterDataReader, MasterDataTable.ShopShop>(MasterDataTable.ShopShop.Parse), (Func<MasterDataTable.ShopShop, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.ShopTopUnit[] ShopTopUnitList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.ShopTopUnit>("ShopTopUnit", new Func<MasterDataReader, MasterDataTable.ShopTopUnit>(MasterDataTable.ShopTopUnit.Parse), (Func<MasterDataTable.ShopTopUnit, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SkillMetamorphosis[] SkillMetamorphosisList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SkillMetamorphosis>("SkillMetamorphosis", new Func<MasterDataReader, MasterDataTable.SkillMetamorphosis>(MasterDataTable.SkillMetamorphosis.Parse), (Func<MasterDataTable.SkillMetamorphosis, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SlotS001MedalDeck[] SlotS001MedalDeckList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SlotS001MedalDeck>("SlotS001MedalDeck", new Func<MasterDataReader, MasterDataTable.SlotS001MedalDeck>(MasterDataTable.SlotS001MedalDeck.Parse), (Func<MasterDataTable.SlotS001MedalDeck, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SlotS001MedalDeckEntity[] SlotS001MedalDeckEntityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SlotS001MedalDeckEntity>("SlotS001MedalDeckEntity", new Func<MasterDataReader, MasterDataTable.SlotS001MedalDeckEntity>(MasterDataTable.SlotS001MedalDeckEntity.Parse), (Func<MasterDataTable.SlotS001MedalDeckEntity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SlotS001MedalRarity[] SlotS001MedalRarityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SlotS001MedalRarity>("SlotS001MedalRarity", new Func<MasterDataReader, MasterDataTable.SlotS001MedalRarity>(MasterDataTable.SlotS001MedalRarity.Parse), (Func<MasterDataTable.SlotS001MedalRarity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SlotS001MedalReel[] SlotS001MedalReelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SlotS001MedalReel>("SlotS001MedalReel", new Func<MasterDataReader, MasterDataTable.SlotS001MedalReel>(MasterDataTable.SlotS001MedalReel.Parse), (Func<MasterDataTable.SlotS001MedalReel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SlotS001MedalReelDetail[] SlotS001MedalReelDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SlotS001MedalReelDetail>("SlotS001MedalReelDetail", new Func<MasterDataReader, MasterDataTable.SlotS001MedalReelDetail>(MasterDataTable.SlotS001MedalReelDetail.Parse), (Func<MasterDataTable.SlotS001MedalReelDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SlotS001MedalReelIcon[] SlotS001MedalReelIconList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SlotS001MedalReelIcon>("SlotS001MedalReelIcon", new Func<MasterDataReader, MasterDataTable.SlotS001MedalReelIcon>(MasterDataTable.SlotS001MedalReelIcon.Parse), (Func<MasterDataTable.SlotS001MedalReelIcon, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackCharacter[] StoryPlaybackCharacterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackCharacter>("StoryPlaybackCharacter", new Func<MasterDataReader, MasterDataTable.StoryPlaybackCharacter>(MasterDataTable.StoryPlaybackCharacter.Parse), (Func<MasterDataTable.StoryPlaybackCharacter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackCharacterDetail[] StoryPlaybackCharacterDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackCharacterDetail>("StoryPlaybackCharacterDetail", new Func<MasterDataReader, MasterDataTable.StoryPlaybackCharacterDetail>(MasterDataTable.StoryPlaybackCharacterDetail.Parse), (Func<MasterDataTable.StoryPlaybackCharacterDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackEventPlay[] StoryPlaybackEventPlayList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackEventPlay>("StoryPlaybackEventPlay", new Func<MasterDataReader, MasterDataTable.StoryPlaybackEventPlay>(MasterDataTable.StoryPlaybackEventPlay.Parse), (Func<MasterDataTable.StoryPlaybackEventPlay, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackExtra[] StoryPlaybackExtraList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackExtra>("StoryPlaybackExtra", new Func<MasterDataReader, MasterDataTable.StoryPlaybackExtra>(MasterDataTable.StoryPlaybackExtra.Parse), (Func<MasterDataTable.StoryPlaybackExtra, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackExtraDetail[] StoryPlaybackExtraDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackExtraDetail>("StoryPlaybackExtraDetail", new Func<MasterDataReader, MasterDataTable.StoryPlaybackExtraDetail>(MasterDataTable.StoryPlaybackExtraDetail.Parse), (Func<MasterDataTable.StoryPlaybackExtraDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackHarmony[] StoryPlaybackHarmonyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackHarmony>("StoryPlaybackHarmony", new Func<MasterDataReader, MasterDataTable.StoryPlaybackHarmony>(MasterDataTable.StoryPlaybackHarmony.Parse), (Func<MasterDataTable.StoryPlaybackHarmony, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackHarmonyDetail[] StoryPlaybackHarmonyDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackHarmonyDetail>("StoryPlaybackHarmonyDetail", new Func<MasterDataReader, MasterDataTable.StoryPlaybackHarmonyDetail>(MasterDataTable.StoryPlaybackHarmonyDetail.Parse), (Func<MasterDataTable.StoryPlaybackHarmonyDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackRaidDetail[] StoryPlaybackRaidDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackRaidDetail>("StoryPlaybackRaidDetail", new Func<MasterDataReader, MasterDataTable.StoryPlaybackRaidDetail>(MasterDataTable.StoryPlaybackRaidDetail.Parse), (Func<MasterDataTable.StoryPlaybackRaidDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackSea[] StoryPlaybackSeaList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackSea>("StoryPlaybackSea", new Func<MasterDataReader, MasterDataTable.StoryPlaybackSea>(MasterDataTable.StoryPlaybackSea.Parse), (Func<MasterDataTable.StoryPlaybackSea, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackSeaDetail[] StoryPlaybackSeaDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackSeaDetail>("StoryPlaybackSeaDetail", new Func<MasterDataReader, MasterDataTable.StoryPlaybackSeaDetail>(MasterDataTable.StoryPlaybackSeaDetail.Parse), (Func<MasterDataTable.StoryPlaybackSeaDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackStory[] StoryPlaybackStoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackStory>("StoryPlaybackStory", new Func<MasterDataReader, MasterDataTable.StoryPlaybackStory>(MasterDataTable.StoryPlaybackStory.Parse), (Func<MasterDataTable.StoryPlaybackStory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.StoryPlaybackStoryDetail[] StoryPlaybackStoryDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.StoryPlaybackStoryDetail>("StoryPlaybackStoryDetail", new Func<MasterDataReader, MasterDataTable.StoryPlaybackStoryDetail>(MasterDataTable.StoryPlaybackStoryDetail.Parse), (Func<MasterDataTable.StoryPlaybackStoryDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.SupplySupply[] SupplySupplyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.SupplySupply>("SupplySupply", new Func<MasterDataReader, MasterDataTable.SupplySupply>(MasterDataTable.SupplySupply.Parse), (Func<MasterDataTable.SupplySupply, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TipsLoadingBackground[] TipsLoadingBackgroundList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TipsLoadingBackground>("TipsLoadingBackground", new Func<MasterDataReader, MasterDataTable.TipsLoadingBackground>(MasterDataTable.TipsLoadingBackground.Parse), (Func<MasterDataTable.TipsLoadingBackground, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TipsTextTips[] TipsTextTipsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TipsTextTips>("TipsTextTips", new Func<MasterDataReader, MasterDataTable.TipsTextTips>(MasterDataTable.TipsTextTips.Parse), (Func<MasterDataTable.TipsTextTips, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TipsTips[] TipsTipsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TipsTips>("TipsTips", new Func<MasterDataReader, MasterDataTable.TipsTips>(MasterDataTable.TipsTips.Parse), (Func<MasterDataTable.TipsTips, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TotalPaymentBonus[] TotalPaymentBonusList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TotalPaymentBonus>("TotalPaymentBonus", new Func<MasterDataReader, MasterDataTable.TotalPaymentBonus>(MasterDataTable.TotalPaymentBonus.Parse), (Func<MasterDataTable.TotalPaymentBonus, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TotalPaymentBonusContent[] TotalPaymentBonusContentList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TotalPaymentBonusContent>("TotalPaymentBonusContent", new Func<MasterDataReader, MasterDataTable.TotalPaymentBonusContent>(MasterDataTable.TotalPaymentBonusContent.Parse), (Func<MasterDataTable.TotalPaymentBonusContent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TotalPaymentBonusReward[] TotalPaymentBonusRewardList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TotalPaymentBonusReward>("TotalPaymentBonusReward", new Func<MasterDataReader, MasterDataTable.TotalPaymentBonusReward>(MasterDataTable.TotalPaymentBonusReward.Parse), (Func<MasterDataTable.TotalPaymentBonusReward, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerBattleStageClear[] TowerBattleStageClearList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerBattleStageClear>("TowerBattleStageClear", new Func<MasterDataReader, MasterDataTable.TowerBattleStageClear>(MasterDataTable.TowerBattleStageClear.Parse), (Func<MasterDataTable.TowerBattleStageClear, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerBgm[] TowerBgmList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerBgm>("TowerBgm", new Func<MasterDataReader, MasterDataTable.TowerBgm>(MasterDataTable.TowerBgm.Parse), (Func<MasterDataTable.TowerBgm, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerCommon[] TowerCommonList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerCommon>("TowerCommon", new Func<MasterDataReader, MasterDataTable.TowerCommon>(MasterDataTable.TowerCommon.Parse), (Func<MasterDataTable.TowerCommon, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerCommonBackground[] TowerCommonBackgroundList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerCommonBackground>("TowerCommonBackground", new Func<MasterDataReader, MasterDataTable.TowerCommonBackground>(MasterDataTable.TowerCommonBackground.Parse), (Func<MasterDataTable.TowerCommonBackground, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerEntryConditions[] TowerEntryConditionsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerEntryConditions>("TowerEntryConditions", new Func<MasterDataReader, MasterDataTable.TowerEntryConditions>(MasterDataTable.TowerEntryConditions.Parse), (Func<MasterDataTable.TowerEntryConditions, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerFloorName[] TowerFloorNameList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerFloorName>("TowerFloorName", new Func<MasterDataReader, MasterDataTable.TowerFloorName>(MasterDataTable.TowerFloorName.Parse), (Func<MasterDataTable.TowerFloorName, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerHowto[] TowerHowtoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerHowto>("TowerHowto", new Func<MasterDataReader, MasterDataTable.TowerHowto>(MasterDataTable.TowerHowto.Parse), (Func<MasterDataTable.TowerHowto, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerOverkill[] TowerOverkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerOverkill>("TowerOverkill", new Func<MasterDataReader, MasterDataTable.TowerOverkill>(MasterDataTable.TowerOverkill.Parse), (Func<MasterDataTable.TowerOverkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerPeriod[] TowerPeriodList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerPeriod>("TowerPeriod", new Func<MasterDataReader, MasterDataTable.TowerPeriod>(MasterDataTable.TowerPeriod.Parse), (Func<MasterDataTable.TowerPeriod, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerPlaybackStory[] TowerPlaybackStoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerPlaybackStory>("TowerPlaybackStory", new Func<MasterDataReader, MasterDataTable.TowerPlaybackStory>(MasterDataTable.TowerPlaybackStory.Parse), (Func<MasterDataTable.TowerPlaybackStory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerPlaybackStoryDetail[] TowerPlaybackStoryDetailList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerPlaybackStoryDetail>("TowerPlaybackStoryDetail", new Func<MasterDataReader, MasterDataTable.TowerPlaybackStoryDetail>(MasterDataTable.TowerPlaybackStoryDetail.Parse), (Func<MasterDataTable.TowerPlaybackStoryDetail, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerStage[] TowerStageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerStage>("TowerStage", new Func<MasterDataReader, MasterDataTable.TowerStage>(MasterDataTable.TowerStage.Parse), (Func<MasterDataTable.TowerStage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.TowerTower[] TowerTowerList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.TowerTower>("TowerTower", new Func<MasterDataReader, MasterDataTable.TowerTower>(MasterDataTable.TowerTower.Parse), (Func<MasterDataTable.TowerTower, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitActivityScenes[] UnitActivityScenesList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitActivityScenes>("UnitActivityScenes", new Func<MasterDataReader, MasterDataTable.UnitActivityScenes>(MasterDataTable.UnitActivityScenes.Parse), (Func<MasterDataTable.UnitActivityScenes, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitAdvice[] UnitAdviceList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitAdvice>("UnitAdvice", new Func<MasterDataReader, MasterDataTable.UnitAdvice>(MasterDataTable.UnitAdvice.Parse), (Func<MasterDataTable.UnitAdvice, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitAffiliationIcon[] UnitAffiliationIconList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitAffiliationIcon>("UnitAffiliationIcon", new Func<MasterDataReader, MasterDataTable.UnitAffiliationIcon>(MasterDataTable.UnitAffiliationIcon.Parse), (Func<MasterDataTable.UnitAffiliationIcon, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitAwakeningEffect[] UnitAwakeningEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitAwakeningEffect>("UnitAwakeningEffect", new Func<MasterDataReader, MasterDataTable.UnitAwakeningEffect>(MasterDataTable.UnitAwakeningEffect.Parse), (Func<MasterDataTable.UnitAwakeningEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitBreakThrough[] UnitBreakThroughList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitBreakThrough>("UnitBreakThrough", new Func<MasterDataReader, MasterDataTable.UnitBreakThrough>(MasterDataTable.UnitBreakThrough.Parse), (Func<MasterDataTable.UnitBreakThrough, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitBuildupMaterialPattern[] UnitBuildupMaterialPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitBuildupMaterialPattern>("UnitBuildupMaterialPattern", new Func<MasterDataReader, MasterDataTable.UnitBuildupMaterialPattern>(MasterDataTable.UnitBuildupMaterialPattern.Parse), (Func<MasterDataTable.UnitBuildupMaterialPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitCameraPattern[] UnitCameraPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitCameraPattern>("UnitCameraPattern", new Func<MasterDataReader, MasterDataTable.UnitCameraPattern>(MasterDataTable.UnitCameraPattern.Parse), (Func<MasterDataTable.UnitCameraPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitCharacter[] UnitCharacterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitCharacter>("UnitCharacter", new Func<MasterDataReader, MasterDataTable.UnitCharacter>(MasterDataTable.UnitCharacter.Parse), (Func<MasterDataTable.UnitCharacter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitCharacterExtension[] UnitCharacterExtensionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitCharacterExtension>("UnitCharacterExtension", new Func<MasterDataReader, MasterDataTable.UnitCharacterExtension>(MasterDataTable.UnitCharacterExtension.Parse), (Func<MasterDataTable.UnitCharacterExtension, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitComponent[] UnitComponentList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitComponent>("UnitComponent", new Func<MasterDataReader, MasterDataTable.UnitComponent>(MasterDataTable.UnitComponent.Parse), (Func<MasterDataTable.UnitComponent, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitCutinInfo[] UnitCutinInfoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitCutinInfo>("UnitCutinInfo", new Func<MasterDataReader, MasterDataTable.UnitCutinInfo>(MasterDataTable.UnitCutinInfo.Parse), (Func<MasterDataTable.UnitCutinInfo, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitEvolutionPattern[] UnitEvolutionPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitEvolutionPattern>("UnitEvolutionPattern", new Func<MasterDataReader, MasterDataTable.UnitEvolutionPattern>(MasterDataTable.UnitEvolutionPattern.Parse), (Func<MasterDataTable.UnitEvolutionPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitEvolutionUnit[] UnitEvolutionUnitList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitEvolutionUnit>("UnitEvolutionUnit", new Func<MasterDataReader, MasterDataTable.UnitEvolutionUnit>(MasterDataTable.UnitEvolutionUnit.Parse), (Func<MasterDataTable.UnitEvolutionUnit, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitExpireDate[] UnitExpireDateList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitExpireDate>("UnitExpireDate", new Func<MasterDataReader, MasterDataTable.UnitExpireDate>(MasterDataTable.UnitExpireDate.Parse), (Func<MasterDataTable.UnitExpireDate, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitExtensionStory[] UnitExtensionStoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitExtensionStory>("UnitExtensionStory", new Func<MasterDataReader, MasterDataTable.UnitExtensionStory>(MasterDataTable.UnitExtensionStory.Parse), (Func<MasterDataTable.UnitExtensionStory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitFamilyValue[] UnitFamilyValueList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitFamilyValue>("UnitFamilyValue", new Func<MasterDataReader, MasterDataTable.UnitFamilyValue>(MasterDataTable.UnitFamilyValue.Parse), (Func<MasterDataTable.UnitFamilyValue, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitFootstepType[] UnitFootstepTypeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitFootstepType>("UnitFootstepType", new Func<MasterDataReader, MasterDataTable.UnitFootstepType>(MasterDataTable.UnitFootstepType.Parse), (Func<MasterDataTable.UnitFootstepType, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitGenderText[] UnitGenderTextList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitGenderText>("UnitGenderText", new Func<MasterDataReader, MasterDataTable.UnitGenderText>(MasterDataTable.UnitGenderText.Parse), (Func<MasterDataTable.UnitGenderText, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitGroup[] UnitGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitGroup>("UnitGroup", new Func<MasterDataReader, MasterDataTable.UnitGroup>(MasterDataTable.UnitGroup.Parse), (Func<MasterDataTable.UnitGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitGroupClothingCategory[] UnitGroupClothingCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitGroupClothingCategory>("UnitGroupClothingCategory", new Func<MasterDataReader, MasterDataTable.UnitGroupClothingCategory>(MasterDataTable.UnitGroupClothingCategory.Parse), (Func<MasterDataTable.UnitGroupClothingCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitGroupGenerationCategory[] UnitGroupGenerationCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitGroupGenerationCategory>("UnitGroupGenerationCategory", new Func<MasterDataReader, MasterDataTable.UnitGroupGenerationCategory>(MasterDataTable.UnitGroupGenerationCategory.Parse), (Func<MasterDataTable.UnitGroupGenerationCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitGroupLargeCategory[] UnitGroupLargeCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitGroupLargeCategory>("UnitGroupLargeCategory", new Func<MasterDataReader, MasterDataTable.UnitGroupLargeCategory>(MasterDataTable.UnitGroupLargeCategory.Parse), (Func<MasterDataTable.UnitGroupLargeCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitGroupSmallCategory[] UnitGroupSmallCategoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitGroupSmallCategory>("UnitGroupSmallCategory", new Func<MasterDataReader, MasterDataTable.UnitGroupSmallCategory>(MasterDataTable.UnitGroupSmallCategory.Parse), (Func<MasterDataTable.UnitGroupSmallCategory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitHomeVoicePattern[] UnitHomeVoicePatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitHomeVoicePattern>("UnitHomeVoicePattern", new Func<MasterDataReader, MasterDataTable.UnitHomeVoicePattern>(MasterDataTable.UnitHomeVoicePattern.Parse), (Func<MasterDataTable.UnitHomeVoicePattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitIllustPattern[] UnitIllustPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitIllustPattern>("UnitIllustPattern", new Func<MasterDataReader, MasterDataTable.UnitIllustPattern>(MasterDataTable.UnitIllustPattern.Parse), (Func<MasterDataTable.UnitIllustPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitInitialParam[] UnitInitialParamList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitInitialParam>("UnitInitialParam", new Func<MasterDataReader, MasterDataTable.UnitInitialParam>(MasterDataTable.UnitInitialParam.Parse), (Func<MasterDataTable.UnitInitialParam, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitJob[] UnitJobList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitJob>("UnitJob", new Func<MasterDataReader, MasterDataTable.UnitJob>(MasterDataTable.UnitJob.Parse), (Func<MasterDataTable.UnitJob, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitJobFamily[] UnitJobFamilyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitJobFamily>("UnitJobFamily", new Func<MasterDataReader, MasterDataTable.UnitJobFamily>(MasterDataTable.UnitJobFamily.Parse), (Func<MasterDataTable.UnitJobFamily, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitJobRankName[] UnitJobRankNameList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitJobRankName>("UnitJobRankName", new Func<MasterDataReader, MasterDataTable.UnitJobRankName>(MasterDataTable.UnitJobRankName.Parse), (Func<MasterDataTable.UnitJobRankName, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitLeaderSkill[] UnitLeaderSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitLeaderSkill>("UnitLeaderSkill", new Func<MasterDataReader, MasterDataTable.UnitLeaderSkill>(MasterDataTable.UnitLeaderSkill.Parse), (Func<MasterDataTable.UnitLeaderSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitLevel[] UnitLevelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitLevel>("UnitLevel", new Func<MasterDataReader, MasterDataTable.UnitLevel>(MasterDataTable.UnitLevel.Parse), (Func<MasterDataTable.UnitLevel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitMaterialExclusion[] UnitMaterialExclusionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitMaterialExclusion>("UnitMaterialExclusion", new Func<MasterDataReader, MasterDataTable.UnitMaterialExclusion>(MasterDataTable.UnitMaterialExclusion.Parse), (Func<MasterDataTable.UnitMaterialExclusion, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitMaterialQuestInfo[] UnitMaterialQuestInfoList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitMaterialQuestInfo>("UnitMaterialQuestInfo", new Func<MasterDataReader, MasterDataTable.UnitMaterialQuestInfo>(MasterDataTable.UnitMaterialQuestInfo.Parse), (Func<MasterDataTable.UnitMaterialQuestInfo, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitModel[] UnitModelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitModel>("UnitModel", new Func<MasterDataReader, MasterDataTable.UnitModel>(MasterDataTable.UnitModel.Parse), (Func<MasterDataTable.UnitModel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitPickupSkill[] UnitPickupSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitPickupSkill>("UnitPickupSkill", new Func<MasterDataReader, MasterDataTable.UnitPickupSkill>(MasterDataTable.UnitPickupSkill.Parse), (Func<MasterDataTable.UnitPickupSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitProficiency[] UnitProficiencyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitProficiency>("UnitProficiency", new Func<MasterDataReader, MasterDataTable.UnitProficiency>(MasterDataTable.UnitProficiency.Parse), (Func<MasterDataTable.UnitProficiency, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitProficiencyIncr[] UnitProficiencyIncrList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitProficiencyIncr>("UnitProficiencyIncr", new Func<MasterDataReader, MasterDataTable.UnitProficiencyIncr>(MasterDataTable.UnitProficiencyIncr.Parse), (Func<MasterDataTable.UnitProficiencyIncr, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitProficiencyLevel[] UnitProficiencyLevelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitProficiencyLevel>("UnitProficiencyLevel", new Func<MasterDataReader, MasterDataTable.UnitProficiencyLevel>(MasterDataTable.UnitProficiencyLevel.Parse), (Func<MasterDataTable.UnitProficiencyLevel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitRarity[] UnitRarityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitRarity>("UnitRarity", new Func<MasterDataReader, MasterDataTable.UnitRarity>(MasterDataTable.UnitRarity.Parse), (Func<MasterDataTable.UnitRarity, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitRecommend[] UnitRecommendList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitRecommend>("UnitRecommend", new Func<MasterDataReader, MasterDataTable.UnitRecommend>(MasterDataTable.UnitRecommend.Parse), (Func<MasterDataTable.UnitRecommend, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitReferenceImage[] UnitReferenceImageList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitReferenceImage>("UnitReferenceImage", new Func<MasterDataReader, MasterDataTable.UnitReferenceImage>(MasterDataTable.UnitReferenceImage.Parse), (Func<MasterDataTable.UnitReferenceImage, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitRenderingPattern[] UnitRenderingPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitRenderingPattern>("UnitRenderingPattern", new Func<MasterDataReader, MasterDataTable.UnitRenderingPattern>(MasterDataTable.UnitRenderingPattern.Parse), (Func<MasterDataTable.UnitRenderingPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitRole[] UnitRoleList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitRole>("UnitRole", new Func<MasterDataReader, MasterDataTable.UnitRole>(MasterDataTable.UnitRole.Parse), (Func<MasterDataTable.UnitRole, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSEASkill[] UnitSEASkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSEASkill>("UnitSEASkill", new Func<MasterDataReader, MasterDataTable.UnitSEASkill>(MasterDataTable.UnitSEASkill.Parse), (Func<MasterDataTable.UnitSEASkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkill[] UnitSkillList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkill>("UnitSkill", new Func<MasterDataReader, MasterDataTable.UnitSkill>(MasterDataTable.UnitSkill.Parse), (Func<MasterDataTable.UnitSkill, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillAwake[] UnitSkillAwakeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillAwake>("UnitSkillAwake", new Func<MasterDataReader, MasterDataTable.UnitSkillAwake>(MasterDataTable.UnitSkillAwake.Parse), (Func<MasterDataTable.UnitSkillAwake, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillCharacterQuest[] UnitSkillCharacterQuestList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillCharacterQuest>("UnitSkillCharacterQuest", new Func<MasterDataReader, MasterDataTable.UnitSkillCharacterQuest>(MasterDataTable.UnitSkillCharacterQuest.Parse), (Func<MasterDataTable.UnitSkillCharacterQuest, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillEvolution[] UnitSkillEvolutionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillEvolution>("UnitSkillEvolution", new Func<MasterDataReader, MasterDataTable.UnitSkillEvolution>(MasterDataTable.UnitSkillEvolution.Parse), (Func<MasterDataTable.UnitSkillEvolution, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillGroup[] UnitSkillGroupList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillGroup>("UnitSkillGroup", new Func<MasterDataReader, MasterDataTable.UnitSkillGroup>(MasterDataTable.UnitSkillGroup.Parse), (Func<MasterDataTable.UnitSkillGroup, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillHarmonyQuest[] UnitSkillHarmonyQuestList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillHarmonyQuest>("UnitSkillHarmonyQuest", new Func<MasterDataReader, MasterDataTable.UnitSkillHarmonyQuest>(MasterDataTable.UnitSkillHarmonyQuest.Parse), (Func<MasterDataTable.UnitSkillHarmonyQuest, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillIntimate[] UnitSkillIntimateList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillIntimate>("UnitSkillIntimate", new Func<MasterDataReader, MasterDataTable.UnitSkillIntimate>(MasterDataTable.UnitSkillIntimate.Parse), (Func<MasterDataTable.UnitSkillIntimate, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillLevelUpProbability[] UnitSkillLevelUpProbabilityList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillLevelUpProbability>("UnitSkillLevelUpProbability", new Func<MasterDataReader, MasterDataTable.UnitSkillLevelUpProbability>(MasterDataTable.UnitSkillLevelUpProbability.Parse), (Func<MasterDataTable.UnitSkillLevelUpProbability, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillupSetting[] UnitSkillupSettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillupSetting>("UnitSkillupSetting", new Func<MasterDataReader, MasterDataTable.UnitSkillupSetting>(MasterDataTable.UnitSkillupSetting.Parse), (Func<MasterDataTable.UnitSkillupSetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitSkillupSkillGroupSetting[] UnitSkillupSkillGroupSettingList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitSkillupSkillGroupSetting>("UnitSkillupSkillGroupSetting", new Func<MasterDataReader, MasterDataTable.UnitSkillupSkillGroupSetting>(MasterDataTable.UnitSkillupSkillGroupSetting.Parse), (Func<MasterDataTable.UnitSkillupSkillGroupSetting, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTransmigrationMaterial[] UnitTransmigrationMaterialList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTransmigrationMaterial>("UnitTransmigrationMaterial", new Func<MasterDataReader, MasterDataTable.UnitTransmigrationMaterial>(MasterDataTable.UnitTransmigrationMaterial.Parse), (Func<MasterDataTable.UnitTransmigrationMaterial, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTransmigrationPattern[] UnitTransmigrationPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTransmigrationPattern>("UnitTransmigrationPattern", new Func<MasterDataReader, MasterDataTable.UnitTransmigrationPattern>(MasterDataTable.UnitTransmigrationPattern.Parse), (Func<MasterDataTable.UnitTransmigrationPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTrustLevelMaterialPattern[] UnitTrustLevelMaterialPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTrustLevelMaterialPattern>("UnitTrustLevelMaterialPattern", new Func<MasterDataReader, MasterDataTable.UnitTrustLevelMaterialPattern>(MasterDataTable.UnitTrustLevelMaterialPattern.Parse), (Func<MasterDataTable.UnitTrustLevelMaterialPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTrustUpperLimitEffect[] UnitTrustUpperLimitEffectList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTrustUpperLimitEffect>("UnitTrustUpperLimitEffect", new Func<MasterDataReader, MasterDataTable.UnitTrustUpperLimitEffect>(MasterDataTable.UnitTrustUpperLimitEffect.Parse), (Func<MasterDataTable.UnitTrustUpperLimitEffect, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitType[] UnitTypeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitType>("UnitType", new Func<MasterDataReader, MasterDataTable.UnitType>(MasterDataTable.UnitType.Parse), (Func<MasterDataTable.UnitType, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTypeParameter[] UnitTypeParameterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTypeParameter>("UnitTypeParameter", new Func<MasterDataReader, MasterDataTable.UnitTypeParameter>(MasterDataTable.UnitTypeParameter.Parse), (Func<MasterDataTable.UnitTypeParameter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTypeSettings[] UnitTypeSettingsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTypeSettings>("UnitTypeSettings", new Func<MasterDataReader, MasterDataTable.UnitTypeSettings>(MasterDataTable.UnitTypeSettings.Parse), (Func<MasterDataTable.UnitTypeSettings, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTypeTicket[] UnitTypeTicketList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTypeTicket>("UnitTypeTicket", new Func<MasterDataReader, MasterDataTable.UnitTypeTicket>(MasterDataTable.UnitTypeTicket.Parse), (Func<MasterDataTable.UnitTypeTicket, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTypeTicketExclude[] UnitTypeTicketExcludeList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTypeTicketExclude>("UnitTypeTicketExclude", new Func<MasterDataReader, MasterDataTable.UnitTypeTicketExclude>(MasterDataTable.UnitTypeTicketExclude.Parse), (Func<MasterDataTable.UnitTypeTicketExclude, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitTypeTicketUnusable[] UnitTypeTicketUnusableList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitTypeTicketUnusable>("UnitTypeTicketUnusable", new Func<MasterDataReader, MasterDataTable.UnitTypeTicketUnusable>(MasterDataTable.UnitTypeTicketUnusable.Parse), (Func<MasterDataTable.UnitTypeTicketUnusable, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnit[] UnitUnitList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnit>("UnitUnit", new Func<MasterDataReader, MasterDataTable.UnitUnit>(MasterDataTable.UnitUnit.Parse), (Func<MasterDataTable.UnitUnit, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitBuildupAmount[] UnitUnitBuildupAmountList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitBuildupAmount>("UnitUnitBuildupAmount", new Func<MasterDataReader, MasterDataTable.UnitUnitBuildupAmount>(MasterDataTable.UnitUnitBuildupAmount.Parse), (Func<MasterDataTable.UnitUnitBuildupAmount, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitBuildupLimitRelease[] UnitUnitBuildupLimitReleaseList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitBuildupLimitRelease>("UnitUnitBuildupLimitRelease", new Func<MasterDataReader, MasterDataTable.UnitUnitBuildupLimitRelease>(MasterDataTable.UnitUnitBuildupLimitRelease.Parse), (Func<MasterDataTable.UnitUnitBuildupLimitRelease, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitDescription[] UnitUnitDescriptionList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitDescription>("UnitUnitDescription", new Func<MasterDataReader, MasterDataTable.UnitUnitDescription>(MasterDataTable.UnitUnitDescription.Parse), (Func<MasterDataTable.UnitUnitDescription, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitFamily[] UnitUnitFamilyList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitFamily>("UnitUnitFamily", new Func<MasterDataReader, MasterDataTable.UnitUnitFamily>(MasterDataTable.UnitUnitFamily.Parse), (Func<MasterDataTable.UnitUnitFamily, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitGearModelKind[] UnitUnitGearModelKindList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitGearModelKind>("UnitUnitGearModelKind", new Func<MasterDataReader, MasterDataTable.UnitUnitGearModelKind>(MasterDataTable.UnitUnitGearModelKind.Parse), (Func<MasterDataTable.UnitUnitGearModelKind, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitGrowth[] UnitUnitGrowthList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitGrowth>("UnitUnitGrowth", new Func<MasterDataReader, MasterDataTable.UnitUnitGrowth>(MasterDataTable.UnitUnitGrowth.Parse), (Func<MasterDataTable.UnitUnitGrowth, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitParameter[] UnitUnitParameterList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitParameter>("UnitUnitParameter", new Func<MasterDataReader, MasterDataTable.UnitUnitParameter>(MasterDataTable.UnitUnitParameter.Parse), (Func<MasterDataTable.UnitUnitParameter, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitStory[] UnitUnitStoryList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitStory>("UnitUnitStory", new Func<MasterDataReader, MasterDataTable.UnitUnitStory>(MasterDataTable.UnitUnitStory.Parse), (Func<MasterDataTable.UnitUnitStory, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitUnitSupplement[] UnitUnitSupplementList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitUnitSupplement>("UnitUnitSupplement", new Func<MasterDataReader, MasterDataTable.UnitUnitSupplement>(MasterDataTable.UnitUnitSupplement.Parse), (Func<MasterDataTable.UnitUnitSupplement, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitVoicePattern[] UnitVoicePatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitVoicePattern>("UnitVoicePattern", new Func<MasterDataReader, MasterDataTable.UnitVoicePattern>(MasterDataTable.UnitVoicePattern.Parse), (Func<MasterDataTable.UnitVoicePattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitVoiceView[] UnitVoiceViewList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitVoiceView>("UnitVoiceView", new Func<MasterDataReader, MasterDataTable.UnitVoiceView>(MasterDataTable.UnitVoiceView.Parse), (Func<MasterDataTable.UnitVoiceView, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnitXLevel[] UnitXLevelList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnitXLevel>("UnitXLevel", new Func<MasterDataReader, MasterDataTable.UnitXLevel>(MasterDataTable.UnitXLevel.Parse), (Func<MasterDataTable.UnitXLevel, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnityPureValueUpPattern[] UnityPureValueUpPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnityPureValueUpPattern>("UnityPureValueUpPattern", new Func<MasterDataReader, MasterDataTable.UnityPureValueUpPattern>(MasterDataTable.UnityPureValueUpPattern.Parse), (Func<MasterDataTable.UnityPureValueUpPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnityValueUpItemQuest[] UnityValueUpItemQuestList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnityValueUpItemQuest>("UnityValueUpItemQuest", new Func<MasterDataReader, MasterDataTable.UnityValueUpItemQuest>(MasterDataTable.UnityValueUpItemQuest.Parse), (Func<MasterDataTable.UnityValueUpItemQuest, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.UnityValueUpPattern[] UnityValueUpPatternList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.UnityValueUpPattern>("UnityValueUpPattern", new Func<MasterDataReader, MasterDataTable.UnityValueUpPattern>(MasterDataTable.UnityValueUpPattern.Parse), (Func<MasterDataTable.UnityValueUpPattern, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.XJobInformation[] XJobInformationList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.XJobInformation>("XJobInformation", new Func<MasterDataReader, MasterDataTable.XJobInformation>(MasterDataTable.XJobInformation.Parse), (Func<MasterDataTable.XJobInformation, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.XLevelLimits[] XLevelLimitsList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.XLevelLimits>("XLevelLimits", new Func<MasterDataReader, MasterDataTable.XLevelLimits>(MasterDataTable.XLevelLimits.Parse), (Func<MasterDataTable.XLevelLimits, int>) (x => x.ID));
    }
  }

  public static MasterDataTable.XLevelStatus[] XLevelStatusList
  {
    get
    {
      return MasterDataCache.GetList<int, MasterDataTable.XLevelStatus>("XLevelStatus", new Func<MasterDataReader, MasterDataTable.XLevelStatus>(MasterDataTable.XLevelStatus.Parse), (Func<MasterDataTable.XLevelStatus, int>) (x => x.ID));
    }
  }

  public static void ParseJson(Dictionary<string, object> json)
  {
  }

  public static IEnumerator LoadScriptScript(int ID)
  {
    IEnumerator e = MasterDataCache.LoadPartial<int, MasterDataTable.ScriptScript>("ScriptScript", "ScriptScript_part_" + ID.ToString(), new Func<MasterDataReader, MasterDataTable.ScriptScript>(MasterDataTable.ScriptScript.Parse), (Func<MasterDataTable.ScriptScript, int>) (x => x.ID));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static MasterDataTable.SkillMetamorphosis UniqueSkillMetamorphosisBy(
    MasterDataTable.UnitUnit unit,
    MasterDataTable.BattleskillSkill skill,
    int group_id)
  {
    return MasterDataCache.Unique<int, MasterDataTable.SkillMetamorphosis, Tuple<int, int, int>>("SkillMetamorphosis", "By", Tuple.Create<int, int, int>(unit.ID, skill.ID, group_id), (Func<MasterDataTable.SkillMetamorphosis, Tuple<int, int, int>>) (x => Tuple.Create<int, int, int>(x.unit.ID, x.skill.ID, x.group_id)), new Func<MasterDataReader, MasterDataTable.SkillMetamorphosis>(MasterDataTable.SkillMetamorphosis.Parse), (Func<MasterDataTable.SkillMetamorphosis, int>) (x => x.ID));
  }

  public static MasterDataTable.UnitCharacterExtension UniqueUnitCharacterExtensionBy(
    MasterDataTable.UnitUnit unit_id,
    int job_metamor_id)
  {
    return MasterDataCache.Unique<int, MasterDataTable.UnitCharacterExtension, Tuple<int, int>>("UnitCharacterExtension", "By", Tuple.Create<int, int>(unit_id.ID, job_metamor_id), (Func<MasterDataTable.UnitCharacterExtension, Tuple<int, int>>) (x => Tuple.Create<int, int>(x.unit_id.ID, x.job_metamor_id)), new Func<MasterDataReader, MasterDataTable.UnitCharacterExtension>(MasterDataTable.UnitCharacterExtension.Parse), (Func<MasterDataTable.UnitCharacterExtension, int>) (x => x.ID));
  }

  public static MasterDataTable.UnitJobFamily[] WhereUnitJobFamilyBy(MasterDataTable.UnitJob job)
  {
    return MasterDataCache.Where<int, MasterDataTable.UnitJobFamily, Tuple<int>>("UnitJobFamily", "By", Tuple.Create<int>(job.ID), (Func<MasterDataTable.UnitJobFamily, Tuple<int>>) (x => Tuple.Create<int>(x.job.ID)), new Func<MasterDataReader, MasterDataTable.UnitJobFamily>(MasterDataTable.UnitJobFamily.Parse), (Func<MasterDataTable.UnitJobFamily, int>) (x => x.ID));
  }

  public static MasterDataTable.UnitJobRankName[] WhereUnitJobRankNameBy(int ID)
  {
    return MasterDataCache.Where<int, MasterDataTable.UnitJobRankName, Tuple<int>>("UnitJobRankName", "By", Tuple.Create<int>(ID), (Func<MasterDataTable.UnitJobRankName, Tuple<int>>) (x => Tuple.Create<int>(x.ID)), new Func<MasterDataReader, MasterDataTable.UnitJobRankName>(MasterDataTable.UnitJobRankName.Parse), (Func<MasterDataTable.UnitJobRankName, int>) (x => x.ID));
  }

  public static MasterDataTable.UnitModel UniqueUnitModelBy(MasterDataTable.UnitUnit unit_id, int job_metamor_id)
  {
    return MasterDataCache.Unique<int, MasterDataTable.UnitModel, Tuple<int, int>>("UnitModel", "By", Tuple.Create<int, int>(unit_id.ID, job_metamor_id), (Func<MasterDataTable.UnitModel, Tuple<int, int>>) (x => Tuple.Create<int, int>(x.unit_id.ID, x.job_metamor_id)), new Func<MasterDataReader, MasterDataTable.UnitModel>(MasterDataTable.UnitModel.Parse), (Func<MasterDataTable.UnitModel, int>) (x => x.ID));
  }

  public static MasterDataTable.UnitProficiencyIncr UniqueUnitProficiencyIncrBy(
    MasterDataTable.GearKind kind,
    MasterDataTable.UnitProficiency proficiency)
  {
    return MasterDataCache.Unique<int, MasterDataTable.UnitProficiencyIncr, Tuple<int, int>>("UnitProficiencyIncr", "By", Tuple.Create<int, int>(kind.ID, proficiency.ID), (Func<MasterDataTable.UnitProficiencyIncr, Tuple<int, int>>) (x => Tuple.Create<int, int>(x.kind.ID, x.proficiency.ID)), new Func<MasterDataReader, MasterDataTable.UnitProficiencyIncr>(MasterDataTable.UnitProficiencyIncr.Parse), (Func<MasterDataTable.UnitProficiencyIncr, int>) (x => x.ID));
  }

  public static MasterDataTable.UnitSkillGroup[] WhereUnitSkillGroupBy(MasterDataTable.UnitUnit unit)
  {
    return MasterDataCache.Where<int, MasterDataTable.UnitSkillGroup, Tuple<int>>("UnitSkillGroup", "By", Tuple.Create<int>(unit.ID), (Func<MasterDataTable.UnitSkillGroup, Tuple<int>>) (x => Tuple.Create<int>(x.unit.ID)), new Func<MasterDataReader, MasterDataTable.UnitSkillGroup>(MasterDataTable.UnitSkillGroup.Parse), (Func<MasterDataTable.UnitSkillGroup, int>) (x => x.ID));
  }

  public static MasterDataTable.UnitUnitFamily[] WhereUnitUnitFamilyBy(MasterDataTable.UnitUnit unit)
  {
    return MasterDataCache.Where<int, MasterDataTable.UnitUnitFamily, Tuple<int>>("UnitUnitFamily", "By", Tuple.Create<int>(unit.ID), (Func<MasterDataTable.UnitUnitFamily, Tuple<int>>) (x => Tuple.Create<int>(x.unit.ID)), new Func<MasterDataReader, MasterDataTable.UnitUnitFamily>(MasterDataTable.UnitUnitFamily.Parse), (Func<MasterDataTable.UnitUnitFamily, int>) (x => x.ID));
  }

  public static MasterDataTable.UnitUnitGearModelKind UniqueUnitUnitGearModelKindBy(
    MasterDataTable.UnitUnit unit,
    int? job_metamor_id,
    MasterDataTable.GearModelKind gear_model_kind)
  {
    return MasterDataCache.Unique<int, MasterDataTable.UnitUnitGearModelKind, Tuple<int, int?, int>>("UnitUnitGearModelKind", "By", Tuple.Create<int, int?, int>(unit.ID, job_metamor_id, gear_model_kind.ID), (Func<MasterDataTable.UnitUnitGearModelKind, Tuple<int, int?, int>>) (x => Tuple.Create<int, int?, int>(x.unit.ID, x.job_metamor_id, x.gear_model_kind.ID)), new Func<MasterDataReader, MasterDataTable.UnitUnitGearModelKind>(MasterDataTable.UnitUnitGearModelKind.Parse), (Func<MasterDataTable.UnitUnitGearModelKind, int>) (x => x.ID));
  }

  public static MasterDataTable.SkillMetamorphosis FindSkillMetamorphosis(
    int unit_id,
    int skill_id,
    int group_id)
  {
    return Array.Find<MasterDataTable.SkillMetamorphosis>(MasterData.SkillMetamorphosisList, (Predicate<MasterDataTable.SkillMetamorphosis>) (x => x.unit_UnitUnit == unit_id && x.skill_BattleskillSkill == skill_id && x.group_id == group_id));
  }
}
