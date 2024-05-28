// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleEarthStageGuest
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
  public class BattleEarthStageGuest
  {
    public int ID;
    public int stage_BattleStage;
    public int unit_UnitUnit;
    public int initial_coordinate_x;
    public int initial_coordinate_y;
    public float initial_direction;
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

    public BattleEarthStageGuestSkill[] GuestSkills
    {
      get
      {
        return ((IEnumerable<BattleEarthStageGuestSkill>) MasterData.BattleEarthStageGuestSkillList).Where<BattleEarthStageGuestSkill>((Func<BattleEarthStageGuestSkill, bool>) (x =>
        {
          int skillGroupId1 = x.skill_group_id;
          int? skillGroupId2 = this.skill_group_id;
          int valueOrDefault = skillGroupId2.GetValueOrDefault();
          return skillGroupId1 == valueOrDefault & skillGroupId2.HasValue;
        })).ToArray<BattleEarthStageGuestSkill>();
      }
    }

    public static BattleEarthStageGuest Parse(MasterDataReader reader)
    {
      return new BattleEarthStageGuest()
      {
        ID = reader.ReadInt(),
        stage_BattleStage = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        initial_coordinate_x = reader.ReadInt(),
        initial_coordinate_y = reader.ReadInt(),
        initial_direction = reader.ReadFloat(),
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
