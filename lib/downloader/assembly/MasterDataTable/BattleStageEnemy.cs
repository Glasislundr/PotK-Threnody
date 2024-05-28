// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleStageEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleStageEnemy
  {
    public int ID;
    public int stage_BattleStage;
    public int unit_UnitUnit;
    public int money;
    public int initial_coordinate_x;
    public int initial_coordinate_y;
    public int? reinforcement_BattleReinforcement;
    public float initial_direction;
    public int ai_move_group;
    public int ai_target_move_x;
    public int ai_target_move_y;
    public string ai_skill_function;
    public string ai_attack;
    public string ai_move;
    public string ai_heal;
    public string ai_skill;
    public string ai_use;
    public int? skill_group_id;
    public int gear_GearGear;
    public int gear_rank;
    public int proficiency_UnitProficiency;
    public int level;
    public int hp;
    public int strength;
    public int vitality;
    public int intelligence;
    public int mind;
    public int agility;
    public int dexterity;
    public int lucky;
    public int? parameter_table_BattleEnemyParameterTable;
    public int? parameter_deviation_table_BattleEnemyParameterDeviationTable;
    public int acquire_skill_group_id;
    public int level_correction;
    public int? group_id;
    public int? ai_script_id_BattleAIScript;

    public BattleStageEnemySkill[] EnemySkills
    {
      get
      {
        return ((IEnumerable<BattleStageEnemySkill>) MasterData.BattleStageEnemySkillList).Where<BattleStageEnemySkill>((Func<BattleStageEnemySkill, bool>) (x =>
        {
          int skillGroupId1 = x.skill_group_id;
          int? skillGroupId2 = this.skill_group_id;
          int valueOrDefault = skillGroupId2.GetValueOrDefault();
          return skillGroupId1 == valueOrDefault & skillGroupId2.HasValue;
        })).ToArray<BattleStageEnemySkill>();
      }
    }

    public static BattleStageEnemy Parse(MasterDataReader reader)
    {
      return new BattleStageEnemy()
      {
        ID = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        money = reader.ReadInt(),
        initial_coordinate_x = reader.ReadInt(),
        initial_coordinate_y = reader.ReadInt(),
        reinforcement_BattleReinforcement = reader.ReadIntOrNull(),
        initial_direction = reader.ReadFloat(),
        ai_move_group = reader.ReadInt(),
        ai_target_move_x = reader.ReadInt(),
        ai_target_move_y = reader.ReadInt(),
        ai_skill_function = reader.ReadString(true),
        ai_attack = reader.ReadString(true),
        ai_move = reader.ReadString(true),
        ai_heal = reader.ReadString(true),
        ai_skill = reader.ReadString(true),
        ai_use = reader.ReadString(true),
        skill_group_id = reader.ReadIntOrNull(),
        gear_GearGear = reader.ReadInt(),
        gear_rank = reader.ReadInt(),
        proficiency_UnitProficiency = reader.ReadInt(),
        level = reader.ReadInt(),
        hp = reader.ReadInt(),
        strength = reader.ReadInt(),
        vitality = reader.ReadInt(),
        intelligence = reader.ReadInt(),
        mind = reader.ReadInt(),
        agility = reader.ReadInt(),
        dexterity = reader.ReadInt(),
        lucky = reader.ReadInt(),
        parameter_table_BattleEnemyParameterTable = reader.ReadIntOrNull(),
        parameter_deviation_table_BattleEnemyParameterDeviationTable = reader.ReadIntOrNull(),
        acquire_skill_group_id = reader.ReadInt(),
        level_correction = reader.ReadInt(),
        group_id = reader.ReadIntOrNull(),
        ai_script_id_BattleAIScript = reader.ReadIntOrNull()
      };
    }

    public BattleStage stage
    {
      get
      {
        BattleStage stage;
        if (!MasterData.BattleStage.TryGetValue(this.stage_BattleStage, out stage))
          Debug.LogError((object) ("Key not Found: MasterData.BattleStage[" + (object) this.stage_BattleStage + "]"));
        return stage;
      }
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }

    public BattleReinforcement reinforcement
    {
      get
      {
        if (!this.reinforcement_BattleReinforcement.HasValue)
          return (BattleReinforcement) null;
        BattleReinforcement reinforcement;
        if (!MasterData.BattleReinforcement.TryGetValue(this.reinforcement_BattleReinforcement.Value, out reinforcement))
          Debug.LogError((object) ("Key not Found: MasterData.BattleReinforcement[" + (object) this.reinforcement_BattleReinforcement.Value + "]"));
        return reinforcement;
      }
    }

    public GearGear gear
    {
      get
      {
        GearGear gear;
        if (!MasterData.GearGear.TryGetValue(this.gear_GearGear, out gear))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.gear_GearGear + "]"));
        return gear;
      }
    }

    public UnitProficiency proficiency
    {
      get
      {
        UnitProficiency proficiency;
        if (!MasterData.UnitProficiency.TryGetValue(this.proficiency_UnitProficiency, out proficiency))
          Debug.LogError((object) ("Key not Found: MasterData.UnitProficiency[" + (object) this.proficiency_UnitProficiency + "]"));
        return proficiency;
      }
    }

    public BattleEnemyParameterTable parameter_table
    {
      get
      {
        if (!this.parameter_table_BattleEnemyParameterTable.HasValue)
          return (BattleEnemyParameterTable) null;
        BattleEnemyParameterTable parameterTable;
        if (!MasterData.BattleEnemyParameterTable.TryGetValue(this.parameter_table_BattleEnemyParameterTable.Value, out parameterTable))
          Debug.LogError((object) ("Key not Found: MasterData.BattleEnemyParameterTable[" + (object) this.parameter_table_BattleEnemyParameterTable.Value + "]"));
        return parameterTable;
      }
    }

    public BattleEnemyParameterDeviationTable parameter_deviation_table
    {
      get
      {
        if (!this.parameter_deviation_table_BattleEnemyParameterDeviationTable.HasValue)
          return (BattleEnemyParameterDeviationTable) null;
        BattleEnemyParameterDeviationTable parameterDeviationTable;
        if (!MasterData.BattleEnemyParameterDeviationTable.TryGetValue(this.parameter_deviation_table_BattleEnemyParameterDeviationTable.Value, out parameterDeviationTable))
          Debug.LogError((object) ("Key not Found: MasterData.BattleEnemyParameterDeviationTable[" + (object) this.parameter_deviation_table_BattleEnemyParameterDeviationTable.Value + "]"));
        return parameterDeviationTable;
      }
    }

    public BattleAIScript ai_script_id
    {
      get
      {
        if (!this.ai_script_id_BattleAIScript.HasValue)
          return (BattleAIScript) null;
        BattleAIScript aiScriptId;
        if (!MasterData.BattleAIScript.TryGetValue(this.ai_script_id_BattleAIScript.Value, out aiScriptId))
          Debug.LogError((object) ("Key not Found: MasterData.BattleAIScript[" + (object) this.ai_script_id_BattleAIScript.Value + "]"));
        return aiScriptId;
      }
    }
  }
}
