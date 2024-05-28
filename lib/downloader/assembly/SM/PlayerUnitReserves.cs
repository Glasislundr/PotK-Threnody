// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitReserves
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitReserves : KeyCompare
  {
    public PlayerUnitReservesDexterity dexterity;
    public bool can_equip_awake_skill;
    public PlayerUnitReservesIntelligence intelligence;
    public int move;
    public PlayerUnitReservesMind mind;
    public bool tower_is_entry;
    public string player_id;
    public int id;
    public int _unit;
    public bool is_trust;
    public float trust_max_rate;
    public PlayerUnitReservesStrength strength;
    public int?[] equip_awake_skill_ids;
    public int breakthrough_count;
    public int _unit_type;
    public float tower_hitpoint_rate;
    public int?[] equip_gear_ids;
    public PlayerUnitReservesHp hp;
    public PlayerUnitReservesAgility agility;
    public PlayerUnitReservesLeader_skills[] leader_skills;
    public int max_level;
    public int buildup_limit;
    public PlayerUnitReservesLucky lucky;
    public PlayerUnitReservesVitality vitality;
    public float trust_rate;
    public PlayerUnitGearProficiency[] gear_proficiencies;
    public int exp_next;
    public int level;
    public PlayerUnitReservesSkills[] skills;
    public DateTime created_at;
    public int total_exp;
    public bool favorite;
    public int exp;
    public int buildup_count;

    public UnitUnit unit
    {
      get
      {
        if (MasterData.UnitUnit.ContainsKey(this._unit))
          return MasterData.UnitUnit[this._unit];
        Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this._unit + "]"));
        return (UnitUnit) null;
      }
    }

    public MasterDataTable.UnitType unit_type
    {
      get
      {
        if (MasterData.UnitType.ContainsKey(this._unit_type))
          return MasterData.UnitType[this._unit_type];
        Debug.LogError((object) ("Key not Found: MasterData.UnitType[" + (object) this._unit_type + "]"));
        return (MasterDataTable.UnitType) null;
      }
    }

    public PlayerUnitReserves()
    {
    }

    public PlayerUnitReserves(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this.dexterity = json[nameof (dexterity)] == null ? (PlayerUnitReservesDexterity) null : new PlayerUnitReservesDexterity((Dictionary<string, object>) json[nameof (dexterity)]);
      this.can_equip_awake_skill = (bool) json[nameof (can_equip_awake_skill)];
      this.intelligence = json[nameof (intelligence)] == null ? (PlayerUnitReservesIntelligence) null : new PlayerUnitReservesIntelligence((Dictionary<string, object>) json[nameof (intelligence)]);
      this.move = (int) (long) json[nameof (move)];
      this.mind = json[nameof (mind)] == null ? (PlayerUnitReservesMind) null : new PlayerUnitReservesMind((Dictionary<string, object>) json[nameof (mind)]);
      this.tower_is_entry = (bool) json[nameof (tower_is_entry)];
      this.player_id = (string) json[nameof (player_id)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this._unit = (int) (long) json[nameof (unit)];
      this.is_trust = (bool) json[nameof (is_trust)];
      this.trust_max_rate = (float) (double) json[nameof (trust_max_rate)];
      this.strength = json[nameof (strength)] == null ? (PlayerUnitReservesStrength) null : new PlayerUnitReservesStrength((Dictionary<string, object>) json[nameof (strength)]);
      this.equip_awake_skill_ids = ((IEnumerable<object>) json[nameof (equip_awake_skill_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      this.breakthrough_count = (int) (long) json[nameof (breakthrough_count)];
      this._unit_type = (int) (long) json[nameof (unit_type)];
      this.tower_hitpoint_rate = (float) (double) json[nameof (tower_hitpoint_rate)];
      this.equip_gear_ids = ((IEnumerable<object>) json[nameof (equip_gear_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      this.hp = json[nameof (hp)] == null ? (PlayerUnitReservesHp) null : new PlayerUnitReservesHp((Dictionary<string, object>) json[nameof (hp)]);
      this.agility = json[nameof (agility)] == null ? (PlayerUnitReservesAgility) null : new PlayerUnitReservesAgility((Dictionary<string, object>) json[nameof (agility)]);
      List<PlayerUnitReservesLeader_skills> reservesLeaderSkillsList = new List<PlayerUnitReservesLeader_skills>();
      foreach (object json1 in (List<object>) json[nameof (leader_skills)])
        reservesLeaderSkillsList.Add(json1 == null ? (PlayerUnitReservesLeader_skills) null : new PlayerUnitReservesLeader_skills((Dictionary<string, object>) json1));
      this.leader_skills = reservesLeaderSkillsList.ToArray();
      this.max_level = (int) (long) json[nameof (max_level)];
      this.buildup_limit = (int) (long) json[nameof (buildup_limit)];
      this.lucky = json[nameof (lucky)] == null ? (PlayerUnitReservesLucky) null : new PlayerUnitReservesLucky((Dictionary<string, object>) json[nameof (lucky)]);
      this.vitality = json[nameof (vitality)] == null ? (PlayerUnitReservesVitality) null : new PlayerUnitReservesVitality((Dictionary<string, object>) json[nameof (vitality)]);
      this.trust_rate = (float) (double) json[nameof (trust_rate)];
      List<PlayerUnitGearProficiency> unitGearProficiencyList = new List<PlayerUnitGearProficiency>();
      foreach (object json2 in (List<object>) json[nameof (gear_proficiencies)])
        unitGearProficiencyList.Add(json2 == null ? (PlayerUnitGearProficiency) null : new PlayerUnitGearProficiency((Dictionary<string, object>) json2));
      this.gear_proficiencies = unitGearProficiencyList.ToArray();
      this.exp_next = (int) (long) json[nameof (exp_next)];
      this.level = (int) (long) json[nameof (level)];
      List<PlayerUnitReservesSkills> unitReservesSkillsList = new List<PlayerUnitReservesSkills>();
      foreach (object json3 in (List<object>) json[nameof (skills)])
        unitReservesSkillsList.Add(json3 == null ? (PlayerUnitReservesSkills) null : new PlayerUnitReservesSkills((Dictionary<string, object>) json3));
      this.skills = unitReservesSkillsList.ToArray();
      this.created_at = DateTime.Parse((string) json[nameof (created_at)]);
      this.total_exp = (int) (long) json[nameof (total_exp)];
      this.favorite = (bool) json[nameof (favorite)];
      this.exp = (int) (long) json[nameof (exp)];
      this.buildup_count = (int) (long) json[nameof (buildup_count)];
    }
  }
}
