// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleStageGuest
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
  public class BattleStageGuest
  {
    public int ID;
    public int stage_BattleStage;
    public int unit_UnitUnit;
    public int deck_position;
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

    public BattleStageGuestSkill[] GuestSkills
    {
      get
      {
        return ((IEnumerable<BattleStageGuestSkill>) MasterData.BattleStageGuestSkillList).Where<BattleStageGuestSkill>((Func<BattleStageGuestSkill, bool>) (x =>
        {
          int skillGroupId1 = x.skill_group_id;
          int? skillGroupId2 = this.skill_group_id;
          int valueOrDefault = skillGroupId2.GetValueOrDefault();
          return skillGroupId1 == valueOrDefault & skillGroupId2.HasValue;
        })).ToArray<BattleStageGuestSkill>();
      }
    }

    public int initial_coordinate_y(int stageID)
    {
      return MasterData.BattleStagePlayer.FirstOrDefault<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == stageID && x.Value.deck_position == this.deck_position)).Value.initial_coordinate_y;
    }

    public int initial_coordinate_x(int stageID)
    {
      return MasterData.BattleStagePlayer.FirstOrDefault<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == stageID && x.Value.deck_position == this.deck_position)).Value.initial_coordinate_x;
    }

    public float initial_direction(int stageID)
    {
      return MasterData.BattleStagePlayer.FirstOrDefault<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == stageID && x.Value.deck_position == this.deck_position)).Value.initial_direction;
    }

    public static BattleStageGuest Parse(MasterDataReader reader)
    {
      return new BattleStageGuest()
      {
        ID = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        deck_position = reader.ReadInt(),
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
        lucky = reader.ReadInt()
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
  }
}
