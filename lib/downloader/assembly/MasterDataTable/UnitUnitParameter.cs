// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitUnitParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitUnitParameter
  {
    public int ID;
    public int _level_pattern_id;
    public int _initial_max_level;
    public int breakthrough_limit;
    public int _level_per_breakthrough;
    public int hp_max;
    public int strength_max;
    public int vitality_max;
    public int intelligence_max;
    public int mind_max;
    public int agility_max;
    public int dexterity_max;
    public int lucky_max;
    public int hp_initial;
    public int strength_initial;
    public int vitality_initial;
    public int intelligence_initial;
    public int mind_initial;
    public int agility_initial;
    public int dexterity_initial;
    public int lucky_initial;
    public int hp_compose;
    public int strength_compose;
    public int vitality_compose;
    public int intelligence_compose;
    public int mind_compose;
    public int agility_compose;
    public int dexterity_compose;
    public int lucky_compose;
    public int hp_buildup;
    public int strength_buildup;
    public int vitality_buildup;
    public int intelligence_buildup;
    public int mind_buildup;
    public int agility_buildup;
    public int dexterity_buildup;
    public int lucky_buildup;
    public int buildup_limit;
    public int default_weapon_proficiency_UnitProficiency;
    public int default_shield_proficiency_UnitProficiency;

    public static UnitUnitParameter Parse(MasterDataReader reader)
    {
      return new UnitUnitParameter()
      {
        ID = reader.ReadInt(),
        _level_pattern_id = reader.ReadInt(),
        _initial_max_level = reader.ReadInt(),
        breakthrough_limit = reader.ReadInt(),
        _level_per_breakthrough = reader.ReadInt(),
        hp_max = reader.ReadInt(),
        strength_max = reader.ReadInt(),
        vitality_max = reader.ReadInt(),
        intelligence_max = reader.ReadInt(),
        mind_max = reader.ReadInt(),
        agility_max = reader.ReadInt(),
        dexterity_max = reader.ReadInt(),
        lucky_max = reader.ReadInt(),
        hp_initial = reader.ReadInt(),
        strength_initial = reader.ReadInt(),
        vitality_initial = reader.ReadInt(),
        intelligence_initial = reader.ReadInt(),
        mind_initial = reader.ReadInt(),
        agility_initial = reader.ReadInt(),
        dexterity_initial = reader.ReadInt(),
        lucky_initial = reader.ReadInt(),
        hp_compose = reader.ReadInt(),
        strength_compose = reader.ReadInt(),
        vitality_compose = reader.ReadInt(),
        intelligence_compose = reader.ReadInt(),
        mind_compose = reader.ReadInt(),
        agility_compose = reader.ReadInt(),
        dexterity_compose = reader.ReadInt(),
        lucky_compose = reader.ReadInt(),
        hp_buildup = reader.ReadInt(),
        strength_buildup = reader.ReadInt(),
        vitality_buildup = reader.ReadInt(),
        intelligence_buildup = reader.ReadInt(),
        mind_buildup = reader.ReadInt(),
        agility_buildup = reader.ReadInt(),
        dexterity_buildup = reader.ReadInt(),
        lucky_buildup = reader.ReadInt(),
        buildup_limit = reader.ReadInt(),
        default_weapon_proficiency_UnitProficiency = reader.ReadInt(),
        default_shield_proficiency_UnitProficiency = reader.ReadInt()
      };
    }

    public UnitProficiency default_weapon_proficiency
    {
      get
      {
        UnitProficiency weaponProficiency;
        if (!MasterData.UnitProficiency.TryGetValue(this.default_weapon_proficiency_UnitProficiency, out weaponProficiency))
          Debug.LogError((object) ("Key not Found: MasterData.UnitProficiency[" + (object) this.default_weapon_proficiency_UnitProficiency + "]"));
        return weaponProficiency;
      }
    }

    public UnitProficiency default_shield_proficiency
    {
      get
      {
        UnitProficiency shieldProficiency;
        if (!MasterData.UnitProficiency.TryGetValue(this.default_shield_proficiency_UnitProficiency, out shieldProficiency))
          Debug.LogError((object) ("Key not Found: MasterData.UnitProficiency[" + (object) this.default_shield_proficiency_UnitProficiency + "]"));
        return shieldProficiency;
      }
    }
  }
}
