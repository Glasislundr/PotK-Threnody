// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UniLinq;
using UnityEngine;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnit : KeyCompare
  {
    [NonSerialized]
    private bool? hasJumpSkill_;
    private PlayerUnit.UnitType unitType;
    public bool is_storage;
    public bool is_enemy_leader;
    public int used_primary;
    public PlayerItem primary_equipped_gear;
    public PlayerItem primary_equipped_gear2;
    public PlayerItem primary_equipped_gear3;
    public PlayerItem primary_equipped_reisou;
    public PlayerItem primary_equipped_reisou2;
    public PlayerItem primary_equipped_reisou3;
    public PlayerAwakeSkill primary_equipped_awake_skill;
    public int ai_move_target_x = -1;
    public int ai_move_target_y = -1;
    public int ai_move_group;
    public int ai_move_group_order;
    public string ai_attack = string.Empty;
    public string ai_move = string.Empty;
    public string ai_heal = string.Empty;
    public string ai_skill = string.Empty;
    public string ai_use = string.Empty;
    public string ai_script_file = string.Empty;
    public string ai_skill_function = string.Empty;
    public int spawn_turn;
    public int? group_id;
    public BattleReinforcement reinforcement;
    private PlayerCharacterIntimate[] my_intimates;
    private PlayerUnitTransMigrateMemoryListTransmigrate_memory memoryData;
    private static int? TrustMax;
    private static int? ExtraSkillRelease;
    private static int? TrustComposeRate;
    private static int? UnityValueMax;
    private static int? UnityValueTrustRate;
    private static int? UnityValue;
    private CommonElement? _element;
    [NonSerialized]
    private MasterDataTable.UnitJob unitJob_;
    [NonSerialized]
    private PlayerUnitSkills[] magicSkills_;
    [NonSerialized]
    private PlayerUnitSkills[] passiveSkills_;
    [NonSerialized]
    private PlayerUnitSkills[] retrofitSkills_;
    [NonSerialized]
    private bool? isJobChange_;
    [NonSerialized]
    private UnitFamily[] families_;
    [NonSerialized]
    private IAttackMethod[] battleOptionAttacks_;
    private GearGear _initial_gear;
    [SerializeField]
    private bool is_dirty_overkillers_parameter_ = true;
    [SerializeField]
    private bool is_dirty_overkillers_slots_ = true;
    [NonSerialized]
    private PlayerUnit[] cache_overkillers_units_;
    private int[] cache_overkillers_unit_ids_;
    private int[] cache_overkillers_unit_job_ids_;
    [SerializeField]
    private PlayerUnitSkills[] equippedOverkillersSkills_;
    private static int? SEA_skill_unlock_conditions_;
    public PlayerUnitDexterity dexterity;
    public bool can_equip_awake_skill;
    public PlayerUnitIntelligence intelligence;
    public int move;
    public PlayerUnitMind mind;
    public bool tower_is_entry;
    public string player_id;
    public int id;
    public int _unit;
    public bool is_trust;
    public PlayerUnitStrength strength;
    public int?[] equip_gear_ids;
    public int job_id;
    public int?[] equip_awake_skill_ids;
    public int breakthrough_count;
    public float buildup_unity_value_f;
    public int _unit_type;
    public float tower_hitpoint_rate;
    public int[] over_killers_player_unit_ids;
    public PlayerUnitJob_abilities[] job_abilities;
    public int?[] changed_job_ids;
    public PlayerUnitHp hp;
    public int unity_value;
    public PlayerUnitAll_saved_job_abilities[] all_saved_job_abilities;
    public PlayerUnitAgility agility;
    public PlayerUnitLeader_skills[] leader_skills;
    public int max_level;
    public int buildup_limit;
    public PlayerUnitLucky lucky;
    public PlayerUnitVitality vitality;
    public float trust_rate;
    public PlayerUnitGearProficiency[] gear_proficiencies;
    public int exp_next;
    public PlayerUnitX_job_proficiencies[] x_job_proficiencies;
    public int level;
    public PlayerUnitSkills[] skills;
    public DateTime created_at;
    public int total_exp;
    public bool favorite;
    public PlayerUnitXJobStatus x_job_status;
    public int exp;
    public int buildup_count;
    [NonSerialized]
    private bool? _corps_is_entry;
    [NonSerialized]
    public UnitIconInfo UnitIconInfo;
    [NonSerialized]
    private byte noOpenedEquippedGear3_;
    [NonSerialized]
    private int? _smallCategoryId;
    [NonSerialized]
    private UnitCutinInfo cutinInfo_;

    public int cost
    {
      get
      {
        MasterDataTable.UnitJob jobData = this.getJobData();
        return jobData != null && jobData.new_cost != 0 ? jobData.new_cost : this.unit.cost;
      }
    }

    public float trust_max_rate
    {
      get
      {
        Consts instance = Consts.GetInstance();
        float num = (float) Mathf.Min(this.unity_value, PlayerUnit.GetUnityValueTrustRate());
        return instance.TRUST_RATE_LEVEL_SIZE + num * instance.ADDED_MAX_TRUST_RATE_PER_UNITY;
      }
    }

    public float GetUnitAverageRisingValue()
    {
      if (this.hp == null || this.strength == null || this.intelligence == null || this.vitality == null || this.mind == null || this.agility == null || this.dexterity == null || this.lucky == null)
        return 0.0f;
      float num1 = (float) (this.hp.level + this.strength.level + this.intelligence.level + this.vitality.level + this.mind.level + this.agility.level + this.dexterity.level + this.lucky.level);
      float num2 = (float) (this.level - 1);
      return (double) num1 <= 0.0 || (double) num2 <= 0.0 ? 0.0f : Mathf.Round((float) ((double) num1 / (double) num2 * 10.0)) / 10f;
    }

    public PlayerUnit.DualWieldSkillData getDualWieldSkillData()
    {
      if (this.job_abilities == null || this.job_abilities.Length == 0)
        return (PlayerUnit.DualWieldSkillData) null;
      foreach (PlayerUnitJob_abilities jobAbility in this.job_abilities)
      {
        BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[jobAbility.skill_id];
        if (battleskillSkill != null)
        {
          foreach (BattleskillEffect effect in battleskillSkill.Effects)
          {
            if (effect.EffectLogic.Enum == BattleskillEffectLogicEnum.dual_wield && effect.checkLevel(jobAbility.level))
              return new PlayerUnit.DualWieldSkillData()
              {
                jobAbility = jobAbility,
                skillEffect = effect
              };
          }
        }
      }
      return (PlayerUnit.DualWieldSkillData) null;
    }

    public int normalAttackCount
    {
      get
      {
        PlayerUnit.DualWieldSkillData dualWieldSkillData = this.getDualWieldSkillData();
        return dualWieldSkillData == null ? 1 : dualWieldSkillData.skillEffect.GetInt(BattleskillEffectLogicArgumentEnum.attack_count);
      }
    }

    public float normalAttackDamageRate
    {
      get
      {
        PlayerUnit.DualWieldSkillData dualWieldSkillData = this.getDualWieldSkillData();
        return dualWieldSkillData == null ? 1f : dualWieldSkillData.skillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.damage_percentage) + dualWieldSkillData.skillEffect.GetFloat(BattleskillEffectLogicArgumentEnum.skill_ratio) * (float) dualWieldSkillData.jobAbility.level;
      }
    }

    public bool hasJumpSkill
    {
      get
      {
        if (!this.hasJumpSkill_.HasValue)
        {
          this.hasJumpSkill_ = new bool?(false);
          foreach (PlayerUnitSkills skill in this.skills)
          {
            if (((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.jump)))
            {
              this.hasJumpSkill_ = new bool?(true);
              break;
            }
          }
        }
        return this.hasJumpSkill_.Value;
      }
    }

    public override bool Equals(object rhs) => this.Equals(rhs as PlayerUnit);

    public override int GetHashCode() => 0;

    public bool Equals(PlayerUnit rhs)
    {
      if ((object) rhs == null)
        return false;
      if ((object) this == (object) rhs)
        return true;
      return !(this.GetType() != rhs.GetType()) && this.unitType == rhs.unitType && this.id == rhs.id && this.player_id == rhs.player_id;
    }

    public static bool operator ==(PlayerUnit lhs, PlayerUnit rhs)
    {
      return (object) lhs == null ? (object) rhs == null : lhs.Equals(rhs);
    }

    public static bool operator !=(PlayerUnit lhs, PlayerUnit rhs) => !(lhs == rhs);

    public bool is_enemy
    {
      get => this.unitType == PlayerUnit.UnitType.Enemy;
      set
      {
        if (value)
          this.unitType = PlayerUnit.UnitType.Enemy;
        else
          this.unitType = PlayerUnit.UnitType.Player;
      }
    }

    public bool is_guest => this.unitType == PlayerUnit.UnitType.Guest;

    public PlayerUnit.UsedPrimary usedPrimary
    {
      get => (PlayerUnit.UsedPrimary) this.used_primary;
      set => this.used_primary = (int) value;
    }

    public void resetUsedPrimary()
    {
      PlayerUnit.UsedPrimary usedPrimary = PlayerUnit.UsedPrimary.None;
      if (this.primary_equipped_gear != (PlayerItem) null)
        usedPrimary = PlayerUnit.UsedPrimary.EquippedGear;
      if (this.primary_equipped_gear2 != (PlayerItem) null)
        usedPrimary |= PlayerUnit.UsedPrimary.EquippedGear2;
      if (this.primary_equipped_gear3 != (PlayerItem) null)
        usedPrimary |= PlayerUnit.UsedPrimary.EquippedGear3;
      if (this.primary_equipped_reisou != (PlayerItem) null)
        usedPrimary |= PlayerUnit.UsedPrimary.EquippedReisou;
      if (this.primary_equipped_reisou2 != (PlayerItem) null)
        usedPrimary |= PlayerUnit.UsedPrimary.EquippedReisou2;
      if (this.primary_equipped_reisou3 != (PlayerItem) null)
        usedPrimary |= PlayerUnit.UsedPrimary.EquippedReisou3;
      if (this.primary_equipped_awake_skill != null)
        usedPrimary |= PlayerUnit.UsedPrimary.EquippedSkill;
      this.used_primary = (int) usedPrimary;
    }

    public PlayerUnitTransMigrateMemoryListTransmigrate_memory MemoryData => this.memoryData;

    public PlayerUnit Clone()
    {
      PlayerUnit playerUnit = (PlayerUnit) this.MemberwiseClone();
      playerUnit.resetCacheMember();
      return playerUnit;
    }

    public static PlayerUnit create_by_unitunit(UnitUnit unit, int level = 0)
    {
      PlayerUnit byUnitunit = new PlayerUnit();
      byUnitunit._unit = unit.ID;
      byUnitunit.job_id = unit.job_UnitJob;
      if (level != 0)
        byUnitunit.level = level;
      return byUnitunit;
    }

    public static PlayerUnit CreateByPlayerMaterialUnit(PlayerMaterialUnit playerUnit, int count = 0)
    {
      return new PlayerUnit()
      {
        _unit = playerUnit._unit,
        level = 1,
        max_level = 1,
        id = playerUnit.id,
        player_id = count.ToString(),
        _unit_type = MasterData.UnitTypeList[0].ID
      };
    }

    public static PlayerUnit CreateForKey(int id)
    {
      PlayerUnit forKey = new PlayerUnit();
      forKey._hasKey = true;
      int num1;
      int num2 = num1 = id;
      forKey.id = num1;
      forKey._key = (object) num2;
      return forKey;
    }

    public static int CalcEnemyParameter(
      float level,
      int initial,
      float growthRate,
      float deviation_min,
      float deviation_max,
      XorShift random)
    {
      return Mathf.CeilToInt((float) (((double) initial + (double) level * (double) growthRate) * (random != null ? (double) random.RangeFloat(deviation_min, deviation_max) : 1.0)));
    }

    public static PlayerUnit FromEnemy(
      BattleStageEnemy enemy,
      float indicator_level = 0.0f,
      XorShift random = null,
      int raidLoopCount = 0,
      int raidID = 0,
      bool isRaidBoss = false)
    {
      PlayerUnit pu = new PlayerUnit();
      pu.id = enemy.ID;
      pu.unitType = PlayerUnit.UnitType.Enemy;
      BattleStageEnemyJob battleStageEnemyJob = (BattleStageEnemyJob) null;
      MasterData.BattleStageEnemyJob.TryGetValue(enemy.ID, out battleStageEnemyJob);
      if (MasterData.BattleStageEnemyAttackMethodList != null)
        pu.setBattleOptionAttacks(((IEnumerable<BattleStageEnemyAttackMethod>) MasterData.BattleStageEnemyAttackMethodList).Where<BattleStageEnemyAttackMethod>((Func<BattleStageEnemyAttackMethod, bool>) (x => x.stage_enemy_unit_BattleStageEnemy == enemy.ID)).Select<BattleStageEnemyAttackMethod, IAttackMethod>((Func<BattleStageEnemyAttackMethod, IAttackMethod>) (y => y.CreateInterface())).ToArray<IAttackMethod>());
      else
        pu.setBattleOptionAttacks(new IAttackMethod[0]);
      float level = Mathf.Round(Mathf.Max((float) enemy.level, indicator_level + (float) enemy.level_correction));
      int enemy_level = (int) level;
      pu.dexterity = new PlayerUnitDexterity();
      pu.agility = new PlayerUnitAgility();
      pu.mind = new PlayerUnitMind();
      pu.strength = new PlayerUnitStrength();
      pu.vitality = new PlayerUnitVitality();
      pu.hp = new PlayerUnitHp();
      pu.intelligence = new PlayerUnitIntelligence();
      pu.lucky = new PlayerUnitLucky();
      if (enemy.parameter_table == null || enemy.parameter_deviation_table == null || (double) indicator_level == 0.0)
      {
        pu.dexterity.initial = enemy.dexterity;
        pu.agility.initial = enemy.agility;
        pu.mind.initial = enemy.mind;
        pu.strength.initial = enemy.strength;
        pu.vitality.initial = enemy.vitality;
        pu.hp.initial = enemy.hp;
        pu.intelligence.initial = enemy.intelligence;
        pu.lucky.initial = enemy.lucky;
        pu.level = enemy.level;
        pu.max_level = enemy.level;
      }
      else
      {
        pu.dexterity.initial = PlayerUnit.CalcEnemyParameter(level, enemy.parameter_table.initial_dexterity, enemy.parameter_table.growth_rate_dexterity, enemy.parameter_deviation_table.deviation_min_dexterity, enemy.parameter_deviation_table.deviation_max_dexterity, random);
        pu.agility.initial = PlayerUnit.CalcEnemyParameter(level, enemy.parameter_table.initial_agility, enemy.parameter_table.growth_rate_agility, enemy.parameter_deviation_table.deviation_min_agility, enemy.parameter_deviation_table.deviation_max_agility, random);
        pu.mind.initial = PlayerUnit.CalcEnemyParameter(level, enemy.parameter_table.initial_mind, enemy.parameter_table.growth_rate_mind, enemy.parameter_deviation_table.deviation_min_mind, enemy.parameter_deviation_table.deviation_max_mind, random);
        pu.strength.initial = PlayerUnit.CalcEnemyParameter(level, enemy.parameter_table.initial_strength, enemy.parameter_table.growth_rate_strength, enemy.parameter_deviation_table.deviation_min_strength, enemy.parameter_deviation_table.deviation_max_strength, random);
        pu.vitality.initial = PlayerUnit.CalcEnemyParameter(level, enemy.parameter_table.initial_vitality, enemy.parameter_table.growth_rate_vitality, enemy.parameter_deviation_table.deviation_min_vitality, enemy.parameter_deviation_table.deviation_max_vitality, random);
        pu.hp.initial = PlayerUnit.CalcEnemyParameter(level, enemy.parameter_table.initial_hp, enemy.parameter_table.growth_rate_hp, enemy.parameter_deviation_table.deviation_min_hp, enemy.parameter_deviation_table.deviation_max_hp, random);
        pu.intelligence.initial = PlayerUnit.CalcEnemyParameter(level, enemy.parameter_table.initial_intelligence, enemy.parameter_table.growth_rate_intelligence, enemy.parameter_deviation_table.deviation_min_intelligence, enemy.parameter_deviation_table.deviation_max_intelligence, random);
        pu.lucky.initial = PlayerUnit.CalcEnemyParameter(level, enemy.parameter_table.initial_lucky, enemy.parameter_table.growth_rate_lucky, enemy.parameter_deviation_table.deviation_min_lucky, enemy.parameter_deviation_table.deviation_max_lucky, random);
        pu.level = enemy_level;
        pu.max_level = enemy_level;
      }
      if (isRaidBoss)
      {
        int num = 5;
        int periodID = ((IEnumerable<GuildRaid>) MasterData.GuildRaidList).FirstOrDefault<GuildRaid>((Func<GuildRaid, bool>) (x => x.ID == raidID)).period_id;
        KeyValuePair<int, GuildRaid> keyValuePair = MasterData.GuildRaid.Where<KeyValuePair<int, GuildRaid>>((Func<KeyValuePair<int, GuildRaid>, bool>) (x => x.Value.period_id == periodID)).OrderByDescending<KeyValuePair<int, GuildRaid>, int>((Func<KeyValuePair<int, GuildRaid>, int>) (x => x.Value.lap)).FirstOrDefault<KeyValuePair<int, GuildRaid>>();
        if (keyValuePair.Value != null)
          num = keyValuePair.Value.lap;
        if (raidLoopCount > num)
        {
          raidLoopCount -= num;
          GuildRaidEndless guildRaidEndless = ((IEnumerable<GuildRaidEndless>) MasterData.GuildRaidEndlessList).FirstOrDefault<GuildRaidEndless>((Func<GuildRaidEndless, bool>) (x => x.ID == raidID));
          if (guildRaidEndless != null)
          {
            pu.hp.initial += guildRaidEndless.hp * raidLoopCount;
            pu.strength.initial += guildRaidEndless.strength * raidLoopCount;
            pu.vitality.initial += guildRaidEndless.vitality * raidLoopCount;
            pu.intelligence.initial += guildRaidEndless.intelligence * raidLoopCount;
            pu.mind.initial += guildRaidEndless.mind * raidLoopCount;
            pu.agility.initial += guildRaidEndless.agility * raidLoopCount;
            pu.dexterity.initial += guildRaidEndless.dexterity * raidLoopCount;
            pu.lucky.initial += guildRaidEndless.lucky * raidLoopCount;
          }
        }
      }
      pu.breakthrough_count = 0;
      pu.favorite = false;
      pu.total_exp = 0;
      pu._unit_type = MasterData.UnitTypeList[0].ID;
      pu._unit = enemy.unit_UnitUnit;
      pu.equip_gear_ids = (int?[]) null;
      if (battleStageEnemyJob == null)
      {
        pu.job_id = pu.unit.job_UnitJob;
      }
      else
      {
        pu.job_id = battleStageEnemyJob.job_UnitJob;
        List<PlayerUnitJob_abilities> source = new List<PlayerUnitJob_abilities>(4);
        if (battleStageEnemyJob.ability1_JobCharacteristics.HasValue)
          source.Add(PlayerUnit.createJobAbility(battleStageEnemyJob.ability1, battleStageEnemyJob.level1));
        if (battleStageEnemyJob.ability2_JobCharacteristics.HasValue)
          source.Add(PlayerUnit.createJobAbility(battleStageEnemyJob.ability2, battleStageEnemyJob.level2));
        if (battleStageEnemyJob.ability3_JobCharacteristics.HasValue)
          source.Add(PlayerUnit.createJobAbility(battleStageEnemyJob.ability3, battleStageEnemyJob.level3));
        if (battleStageEnemyJob.ability4_JobCharacteristics.HasValue)
          source.Add(PlayerUnit.createJobAbility(battleStageEnemyJob.ability4, battleStageEnemyJob.level4));
        if (source.Any<PlayerUnitJob_abilities>())
          pu.job_abilities = source.ToArray();
      }
      pu.move = pu.getJobData().movement;
      if (enemy.acquire_skill_group_id == 0 || (double) indicator_level == 0.0)
      {
        pu.skills = ((IEnumerable<BattleStageEnemySkill>) enemy.EnemySkills).Where<BattleStageEnemySkill>((Func<BattleStageEnemySkill, bool>) (x => x.skill.skill_type != BattleskillSkillType.leader)).Select<BattleStageEnemySkill, PlayerUnitSkills>((Func<BattleStageEnemySkill, PlayerUnitSkills>) (x => new PlayerUnitSkills()
        {
          skill_id = x.skill.ID,
          level = x.skill_level
        })).ToArray<PlayerUnitSkills>();
        pu.leader_skills = ((IEnumerable<BattleStageEnemySkill>) enemy.EnemySkills).Where<BattleStageEnemySkill>((Func<BattleStageEnemySkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.leader)).Select<BattleStageEnemySkill, PlayerUnitLeader_skills>((Func<BattleStageEnemySkill, PlayerUnitLeader_skills>) (x => new PlayerUnitLeader_skills()
        {
          skill_id = x.skill.ID,
          level = x.skill_level
        })).ToArray<PlayerUnitLeader_skills>();
      }
      else
      {
        IEnumerable<BattleEnemyAcquireSkill> source = ((IEnumerable<BattleEnemyAcquireSkill>) MasterData.BattleEnemyAcquireSkillList).Where<BattleEnemyAcquireSkill>((Func<BattleEnemyAcquireSkill, bool>) (x => x.group_id == enemy.acquire_skill_group_id && x.level <= enemy_level));
        pu.skills = source.Where<BattleEnemyAcquireSkill>((Func<BattleEnemyAcquireSkill, bool>) (x => x.skill.skill_type != BattleskillSkillType.leader)).Select<BattleEnemyAcquireSkill, PlayerUnitSkills>((Func<BattleEnemyAcquireSkill, PlayerUnitSkills>) (x =>
        {
          int num = Mathf.Min(x.skill.upper_level, (enemy_level - x.level) / x.skill_level_up_rate + 1);
          return new PlayerUnitSkills()
          {
            skill_id = x.skill.ID,
            level = num
          };
        })).ToArray<PlayerUnitSkills>();
        pu.leader_skills = source.Where<BattleEnemyAcquireSkill>((Func<BattleEnemyAcquireSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.leader)).Select<BattleEnemyAcquireSkill, PlayerUnitLeader_skills>((Func<BattleEnemyAcquireSkill, PlayerUnitLeader_skills>) (x =>
        {
          int num = Mathf.Min(x.skill.upper_level, (enemy_level - x.level) / x.skill_level_up_rate + 1);
          return new PlayerUnitLeader_skills()
          {
            skill_id = x.skill.ID,
            level = num
          };
        })).ToArray<PlayerUnitLeader_skills>();
      }
      pu.is_enemy_leader = pu.leader_skills.Length != 0;
      pu.primary_equipped_gear = new PlayerItem();
      pu.primary_equipped_gear.id = XorShift.Range(int.MinValue, -1);
      pu.primary_equipped_gear.broken = false;
      pu.primary_equipped_gear._entity_type = 3;
      pu.primary_equipped_gear.entity_id = enemy.gear_GearGear;
      pu.primary_equipped_gear.gear_level = enemy.gear_rank;
      pu.primary_equipped_gear.favorite = false;
      pu.primary_equipped_gear.for_battle = false;
      pu.primary_equipped_gear.quantity = 1;
      pu.primary_equipped_gear2 = (PlayerItem) null;
      pu.primary_equipped_gear3 = (PlayerItem) null;
      pu.resetUsedPrimary();
      pu.ai_attack = enemy.ai_attack;
      pu.ai_move = enemy.ai_move;
      pu.ai_heal = enemy.ai_heal;
      pu.ai_skill = enemy.ai_skill;
      pu.ai_skill_function = enemy.ai_skill_function;
      pu.ai_script_file = enemy.ai_script_id != null ? enemy.ai_script_id.file_name : string.Empty;
      pu.ai_move_target_x = enemy.ai_target_move_x - 1;
      pu.ai_move_target_y = enemy.ai_target_move_y - 1;
      pu.ai_move_group = enemy.ai_move_group;
      pu.ai_move_group_order = enemy.ID;
      pu.ai_use = enemy.ai_use;
      pu.group_id = enemy.group_id;
      PlayerUnit.settingReinforcement(pu, enemy.reinforcement);
      pu.gear_proficiencies = new PlayerUnitGearProficiency[1]
      {
        new PlayerUnitGearProficiency()
        {
          level = enemy.proficiency.ID,
          gear_kind_id = pu.unit.kind.ID
        }
      };
      return pu;
    }

    private static void settingReinforcement(PlayerUnit pu, BattleReinforcement reinfo)
    {
      pu.spawn_turn = reinfo == null ? 0 : (reinfo.reinforcement_logic.Enum == BattleReinforcementLogicEnum.turn ? reinfo.arg1_value : int.MaxValue);
      pu.reinforcement = reinfo;
    }

    private static PlayerUnitJob_abilities createJobAbility(JobCharacteristics ja, int level)
    {
      return new PlayerUnitJob_abilities()
      {
        job_ability_id = ja.ID,
        skill_id = ja.skill_BattleskillSkill,
        level = level
      };
    }

    public static PlayerUnit FromGuest(BattleStageGuest guest)
    {
      PlayerUnit playerUnit = new PlayerUnit();
      playerUnit.id = guest.ID;
      playerUnit.unitType = PlayerUnit.UnitType.Guest;
      BattleStageGuestJob battleStageGuestJob = (BattleStageGuestJob) null;
      MasterData.BattleStageGuestJob.TryGetValue(guest.ID, out battleStageGuestJob);
      if (MasterData.BattleStageGuestAttackMethodList != null)
        playerUnit.setBattleOptionAttacks(((IEnumerable<BattleStageGuestAttackMethod>) MasterData.BattleStageGuestAttackMethodList).Where<BattleStageGuestAttackMethod>((Func<BattleStageGuestAttackMethod, bool>) (x => x.stage_guest_unit_BattleStageGuest == guest.ID)).Select<BattleStageGuestAttackMethod, IAttackMethod>((Func<BattleStageGuestAttackMethod, IAttackMethod>) (y => y.CreateInterface())).ToArray<IAttackMethod>());
      else
        playerUnit.setBattleOptionAttacks(new IAttackMethod[0]);
      playerUnit.dexterity = new PlayerUnitDexterity();
      playerUnit.agility = new PlayerUnitAgility();
      playerUnit.mind = new PlayerUnitMind();
      playerUnit.strength = new PlayerUnitStrength();
      playerUnit.vitality = new PlayerUnitVitality();
      playerUnit.hp = new PlayerUnitHp();
      playerUnit.intelligence = new PlayerUnitIntelligence();
      playerUnit.lucky = new PlayerUnitLucky();
      playerUnit.dexterity.initial = guest.dexterity;
      playerUnit.agility.initial = guest.agility;
      playerUnit.mind.initial = guest.mind;
      playerUnit.strength.initial = guest.strength;
      playerUnit.vitality.initial = guest.vitality;
      playerUnit.hp.initial = guest.hp;
      playerUnit.intelligence.initial = guest.intelligence;
      playerUnit.lucky.initial = guest.lucky;
      playerUnit.level = guest.level;
      UnitUnitParameter parameterData = guest.unit.parameter_data;
      playerUnit.max_level = parameterData == null ? guest.level : parameterData._initial_max_level + parameterData.breakthrough_limit * parameterData._level_per_breakthrough;
      playerUnit.breakthrough_count = 0;
      playerUnit.favorite = false;
      playerUnit.total_exp = 0;
      playerUnit._unit_type = MasterData.UnitTypeList[0].ID;
      playerUnit._unit = guest.unit_UnitUnit;
      playerUnit.equip_gear_ids = (int?[]) null;
      if (battleStageGuestJob == null)
      {
        playerUnit.job_id = playerUnit.unit.job_UnitJob;
      }
      else
      {
        playerUnit.job_id = battleStageGuestJob.job_UnitJob;
        List<PlayerUnitJob_abilities> source = new List<PlayerUnitJob_abilities>(4);
        if (battleStageGuestJob.ability1_JobCharacteristics.HasValue)
          source.Add(PlayerUnit.createJobAbility(battleStageGuestJob.ability1, battleStageGuestJob.level1));
        if (battleStageGuestJob.ability2_JobCharacteristics.HasValue)
          source.Add(PlayerUnit.createJobAbility(battleStageGuestJob.ability2, battleStageGuestJob.level2));
        if (battleStageGuestJob.ability3_JobCharacteristics.HasValue)
          source.Add(PlayerUnit.createJobAbility(battleStageGuestJob.ability3, battleStageGuestJob.level3));
        if (battleStageGuestJob.ability4_JobCharacteristics.HasValue)
          source.Add(PlayerUnit.createJobAbility(battleStageGuestJob.ability4, battleStageGuestJob.level4));
        if (source.Any<PlayerUnitJob_abilities>())
          playerUnit.job_abilities = source.ToArray();
      }
      playerUnit.move = playerUnit.getJobData().movement;
      playerUnit.skills = ((IEnumerable<BattleStageGuestSkill>) guest.GuestSkills).Where<BattleStageGuestSkill>((Func<BattleStageGuestSkill, bool>) (x => x.skill.skill_type != BattleskillSkillType.leader)).Select<BattleStageGuestSkill, PlayerUnitSkills>((Func<BattleStageGuestSkill, PlayerUnitSkills>) (x => new PlayerUnitSkills()
      {
        skill_id = x.skill.ID,
        level = x.skill_level
      })).ToArray<PlayerUnitSkills>();
      playerUnit.leader_skills = ((IEnumerable<BattleStageGuestSkill>) guest.GuestSkills).Where<BattleStageGuestSkill>((Func<BattleStageGuestSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.leader)).Select<BattleStageGuestSkill, PlayerUnitLeader_skills>((Func<BattleStageGuestSkill, PlayerUnitLeader_skills>) (x => new PlayerUnitLeader_skills()
      {
        skill_id = x.skill.ID,
        level = x.skill_level
      })).ToArray<PlayerUnitLeader_skills>();
      playerUnit.primary_equipped_gear = new PlayerItem();
      playerUnit.primary_equipped_gear.id = XorShift.Range(int.MinValue, -1);
      playerUnit.primary_equipped_gear.broken = false;
      playerUnit.primary_equipped_gear._entity_type = 3;
      playerUnit.primary_equipped_gear.entity_id = guest.gear_GearGear;
      playerUnit.primary_equipped_gear.gear_level = guest.gear_rank;
      playerUnit.primary_equipped_gear.favorite = false;
      playerUnit.primary_equipped_gear.for_battle = false;
      playerUnit.primary_equipped_gear.quantity = 1;
      playerUnit.primary_equipped_gear2 = (PlayerItem) null;
      playerUnit.primary_equipped_gear3 = (PlayerItem) null;
      playerUnit.resetUsedPrimary();
      playerUnit.spawn_turn = 0;
      playerUnit.gear_proficiencies = new PlayerUnitGearProficiency[1]
      {
        new PlayerUnitGearProficiency()
        {
          level = guest.proficiency.ID,
          gear_kind_id = playerUnit.unit.kind.ID
        }
      };
      return playerUnit;
    }

    public static PlayerUnit FromGuest(BattleEarthStageGuest guest)
    {
      PlayerUnit playerUnit = new PlayerUnit()
      {
        id = guest.ID,
        unitType = PlayerUnit.UnitType.Guest,
        dexterity = new PlayerUnitDexterity(),
        agility = new PlayerUnitAgility(),
        mind = new PlayerUnitMind(),
        strength = new PlayerUnitStrength(),
        vitality = new PlayerUnitVitality(),
        hp = new PlayerUnitHp(),
        intelligence = new PlayerUnitIntelligence(),
        lucky = new PlayerUnitLucky()
      };
      playerUnit.dexterity.initial = guest.dexterity;
      playerUnit.agility.initial = guest.agility;
      playerUnit.mind.initial = guest.mind;
      playerUnit.strength.initial = guest.strength;
      playerUnit.vitality.initial = guest.vitality;
      playerUnit.hp.initial = guest.hp;
      playerUnit.intelligence.initial = guest.intelligence;
      playerUnit.lucky.initial = guest.lucky;
      playerUnit.level = guest.level;
      UnitUnitParameter parameterData = guest.unit.parameter_data;
      playerUnit.max_level = parameterData == null ? guest.level : parameterData._initial_max_level + parameterData.breakthrough_limit * parameterData._level_per_breakthrough;
      playerUnit.breakthrough_count = 0;
      playerUnit.move = guest.unit.job.movement;
      playerUnit.favorite = false;
      playerUnit.total_exp = 0;
      playerUnit._unit_type = MasterData.UnitTypeList[0].ID;
      playerUnit._unit = guest.unit_UnitUnit;
      playerUnit.job_id = playerUnit.unit.job_UnitJob;
      playerUnit.equip_gear_ids = (int?[]) null;
      playerUnit.skills = ((IEnumerable<BattleEarthStageGuestSkill>) guest.GuestSkills).Where<BattleEarthStageGuestSkill>((Func<BattleEarthStageGuestSkill, bool>) (x => x.skill.skill_type != BattleskillSkillType.leader)).Select<BattleEarthStageGuestSkill, PlayerUnitSkills>((Func<BattleEarthStageGuestSkill, PlayerUnitSkills>) (x => new PlayerUnitSkills()
      {
        skill_id = x.skill.ID,
        level = x.skill_level
      })).ToArray<PlayerUnitSkills>();
      playerUnit.leader_skills = ((IEnumerable<BattleEarthStageGuestSkill>) guest.GuestSkills).Where<BattleEarthStageGuestSkill>((Func<BattleEarthStageGuestSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.leader)).Select<BattleEarthStageGuestSkill, PlayerUnitLeader_skills>((Func<BattleEarthStageGuestSkill, PlayerUnitLeader_skills>) (x => new PlayerUnitLeader_skills()
      {
        skill_id = x.skill.ID,
        level = x.skill_level
      })).ToArray<PlayerUnitLeader_skills>();
      playerUnit.primary_equipped_gear = new PlayerItem();
      playerUnit.primary_equipped_gear.id = XorShift.Range(int.MinValue, -1);
      playerUnit.primary_equipped_gear.broken = false;
      playerUnit.primary_equipped_gear._entity_type = 3;
      playerUnit.primary_equipped_gear.entity_id = guest.gear_GearGear;
      playerUnit.primary_equipped_gear.gear_level = guest.gear_rank;
      playerUnit.primary_equipped_gear.favorite = false;
      playerUnit.primary_equipped_gear.for_battle = false;
      playerUnit.primary_equipped_gear.quantity = 1;
      playerUnit.primary_equipped_gear2 = (PlayerItem) null;
      playerUnit.primary_equipped_gear3 = (PlayerItem) null;
      playerUnit.resetUsedPrimary();
      playerUnit.spawn_turn = 0;
      playerUnit.gear_proficiencies = new PlayerUnitGearProficiency[1]
      {
        new PlayerUnitGearProficiency()
        {
          level = guest.proficiency.ID,
          gear_kind_id = playerUnit.unit.kind.ID
        }
      };
      return playerUnit;
    }

    public static PlayerUnit FromFacility(UnitUnit unit, BattleStageEnemy enemy)
    {
      PlayerUnit pu = PlayerUnit.FromFacility(unit, enemy.ID);
      PlayerUnit.settingReinforcement(pu, enemy.reinforcement);
      return pu;
    }

    public static PlayerUnit FromFacility(UnitUnit unit, int ID = -1)
    {
      PlayerUnit playerUnit = new PlayerUnit()
      {
        id = ID,
        unitType = PlayerUnit.UnitType.Enemy,
        dexterity = new PlayerUnitDexterity(),
        agility = new PlayerUnitAgility(),
        mind = new PlayerUnitMind(),
        strength = new PlayerUnitStrength(),
        vitality = new PlayerUnitVitality(),
        hp = new PlayerUnitHp(),
        intelligence = new PlayerUnitIntelligence(),
        lucky = new PlayerUnitLucky()
      };
      playerUnit.dexterity.initial = unit.job.dexterity_initial;
      playerUnit.agility.initial = unit.job.agility_initial;
      playerUnit.mind.initial = unit.job.mind_initial;
      playerUnit.strength.initial = unit.job.strength_initial;
      playerUnit.vitality.initial = unit.job.vitality_initial;
      playerUnit.hp.initial = unit.job.hp_initial;
      playerUnit.intelligence.initial = unit.job.intelligence_initial;
      playerUnit.lucky.initial = unit.job.lucky_initial;
      playerUnit.level = unit.facilityLevel;
      playerUnit.max_level = unit.facilityLevel;
      playerUnit.breakthrough_count = 0;
      playerUnit.move = unit.job.movement;
      playerUnit.favorite = false;
      playerUnit.total_exp = 0;
      playerUnit._unit_type = MasterData.UnitTypeList[0].ID;
      playerUnit._unit = unit.ID;
      playerUnit.job_id = unit.job_UnitJob;
      playerUnit.equip_gear_ids = (int?[]) null;
      playerUnit.skills = unit.facilitySkills;
      playerUnit.leader_skills = new PlayerUnitLeader_skills[0];
      playerUnit.primary_equipped_gear = (PlayerItem) null;
      playerUnit.used_primary = (int) sbyte.MaxValue;
      playerUnit.ai_move_group_order = 100;
      return playerUnit;
    }

    public static PlayerUnit FromUnit(UnitUnit unit, int unitType, int ID = -1)
    {
      UnitInitialParam unitInitialParam = ((IEnumerable<UnitInitialParam>) MasterData.UnitInitialParamList).Where<UnitInitialParam>((Func<UnitInitialParam, bool>) (x => x.ID == unit.ID)).FirstOrDefault<UnitInitialParam>();
      PlayerUnit playerUnit = new PlayerUnit()
      {
        id = ID,
        unitType = PlayerUnit.UnitType.Player,
        job_id = unit.job_UnitJob,
        dexterity = new PlayerUnitDexterity(),
        agility = new PlayerUnitAgility(),
        mind = new PlayerUnitMind(),
        strength = new PlayerUnitStrength(),
        vitality = new PlayerUnitVitality(),
        hp = new PlayerUnitHp(),
        intelligence = new PlayerUnitIntelligence(),
        lucky = new PlayerUnitLucky()
      };
      playerUnit.dexterity.initial = unit.job.dexterity_initial + (unitInitialParam != null ? unitInitialParam.dexterity_initial : 0);
      playerUnit.agility.initial = unit.job.agility_initial + (unitInitialParam != null ? unitInitialParam.agility_initial : 0);
      playerUnit.mind.initial = unit.job.mind_initial + (unitInitialParam != null ? unitInitialParam.mind_initial : 0);
      playerUnit.strength.initial = unit.job.strength_initial + (unitInitialParam != null ? unitInitialParam.strength_initial : 0);
      playerUnit.vitality.initial = unit.job.vitality_initial + (unitInitialParam != null ? unitInitialParam.vitality_initial : 0);
      playerUnit.hp.initial = unit.job.hp_initial + (unitInitialParam != null ? unitInitialParam.hp_initial : 0);
      playerUnit.intelligence.initial = unit.job.intelligence_initial + (unitInitialParam != null ? unitInitialParam.intelligence_initial : 0);
      playerUnit.lucky.initial = unit.job.lucky_initial + (unitInitialParam != null ? unitInitialParam.lucky_initial : 0);
      playerUnit.level = 1;
      playerUnit.max_level = unitInitialParam != null ? unitInitialParam.level_max : 50;
      playerUnit.exp = 0;
      playerUnit.total_exp = 0;
      playerUnit.exp_next = 1;
      playerUnit.breakthrough_count = 0;
      playerUnit.move = unit.job.movement;
      playerUnit.favorite = false;
      playerUnit.total_exp = 0;
      playerUnit._unit_type = unitType;
      playerUnit._unit = unit.ID;
      playerUnit.job_id = unit.job_UnitJob;
      playerUnit.equip_gear_ids = (int?[]) null;
      playerUnit.skills = ((IEnumerable<UnitSkill>) playerUnit.unit.RememberUnitSkills(playerUnit._unit_type)).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.level == 1)).Select<UnitSkill, PlayerUnitSkills>((Func<UnitSkill, PlayerUnitSkills>) (x => new PlayerUnitSkills()
      {
        skill_id = x.skill.ID,
        level = 1
      })).ToArray<PlayerUnitSkills>();
      playerUnit.leader_skills = ((IEnumerable<UnitLeaderSkill>) MasterData.UnitLeaderSkillList).Where<UnitLeaderSkill>((Func<UnitLeaderSkill, bool>) (x => x.unit_UnitUnit == unit.ID && x.skill.skill_type == BattleskillSkillType.leader)).Select<UnitLeaderSkill, PlayerUnitLeader_skills>((Func<UnitLeaderSkill, PlayerUnitLeader_skills>) (x => new PlayerUnitLeader_skills()
      {
        skill_id = x.skill.ID,
        level = 1
      })).ToArray<PlayerUnitLeader_skills>();
      playerUnit.primary_equipped_gear = (PlayerItem) null;
      playerUnit.primary_equipped_gear2 = (PlayerItem) null;
      playerUnit.primary_equipped_gear3 = (PlayerItem) null;
      playerUnit.group_id = new int?(((IEnumerable<UnitGroup>) MasterData.UnitGroupList).Where<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == unit.ID)).FirstOrDefault<UnitGroup>().ID);
      playerUnit.gear_proficiencies = new PlayerUnitGearProficiency[1]
      {
        new PlayerUnitGearProficiency()
        {
          level = 1,
          gear_kind_id = playerUnit.unit.kind.ID
        }
      };
      return playerUnit;
    }

    public static int GetTrustRateMax()
    {
      if (!PlayerUnit.TrustMax.HasValue)
      {
        float result = 0.0f;
        float.TryParse(Consts.GetInstance().TRUST_MAX, out result);
        PlayerUnit.TrustMax = new int?((int) result);
      }
      return PlayerUnit.TrustMax.Value;
    }

    public static int GetExtraSkillReleaseRate()
    {
      if (!PlayerUnit.ExtraSkillRelease.HasValue)
      {
        float result = 0.0f;
        float.TryParse(Consts.GetInstance().EQUIP_AWAKE_SKILL_RELEASE_1, out result);
        PlayerUnit.ExtraSkillRelease = new int?((int) result);
      }
      return PlayerUnit.ExtraSkillRelease.Value;
    }

    public static int GetTrustComposeRate()
    {
      if (!PlayerUnit.TrustComposeRate.HasValue)
      {
        float result = 0.0f;
        float.TryParse(Consts.GetInstance().COMPOSE_TRUST_BASE, out result);
        PlayerUnit.TrustComposeRate = new int?((int) result);
      }
      return PlayerUnit.TrustComposeRate.Value;
    }

    public float unityTotal
    {
      get
      {
        return Mathf.Min((float) this.unity_value + this.buildup_unity_value_f, (float) PlayerUnit.GetUnityValueMax());
      }
    }

    public void setUnity(int v) => this.unity_value = Mathf.Min(v, PlayerUnit.GetUnityValueMax());

    public void addUnity(int v)
    {
      this.unity_value = Mathf.Min(this.unity_value + v, PlayerUnit.GetUnityValueMax());
    }

    public void setBuildupUnity(int v)
    {
      this.buildup_unity_value_f = (float) Mathf.Min(v, PlayerUnit.GetUnityValueMax());
    }

    public void addBuildupUnity(int v)
    {
      this.buildup_unity_value_f = Mathf.Min(this.buildup_unity_value_f + (float) v, (float) PlayerUnit.GetUnityValueMax());
    }

    public static int GetUnityValueMax()
    {
      if (!PlayerUnit.UnityValueMax.HasValue)
      {
        float result = 0.0f;
        float.TryParse(Consts.GetInstance().UNITY_VALUE_LIMIT, out result);
        PlayerUnit.UnityValueMax = new int?((int) result);
      }
      return PlayerUnit.UnityValueMax.Value;
    }

    public static int GetUnityValueTrustRate()
    {
      if (!PlayerUnit.UnityValueTrustRate.HasValue)
      {
        float result = 0.0f;
        float.TryParse(Consts.GetInstance().UNITY_VALUE_TRUST_RATE, out result);
        PlayerUnit.UnityValueTrustRate = new int?((int) result);
      }
      return PlayerUnit.UnityValueTrustRate.Value;
    }

    public static int GetUnityValue()
    {
      if (!PlayerUnit.UnityValue.HasValue)
      {
        float result = 0.0f;
        float.TryParse(Consts.GetInstance().UNITY_VALUE, out result);
        PlayerUnit.UnityValue = new int?((int) result);
      }
      return PlayerUnit.UnityValue.Value;
    }

    public bool checkGroup(int id) => this.group_id.HasValue && this.group_id.Value == id;

    public void SetUserEnemyUnit(
      BattleStageUserUnit data,
      PlayerItem gear,
      PlayerItem gear2,
      PlayerItem gear3,
      PlayerItem reisou,
      PlayerItem reisou2,
      PlayerItem reisou3,
      bool is_leader)
    {
      this.unitType = PlayerUnit.UnitType.Enemy;
      if (gear != (PlayerItem) null)
        this.primary_equipped_gear = gear;
      if (gear2 != (PlayerItem) null)
        this.primary_equipped_gear2 = gear2;
      if (gear3 != (PlayerItem) null)
        this.primary_equipped_gear3 = gear3;
      if (gear != (PlayerItem) null)
        this.primary_equipped_reisou = reisou;
      if (gear2 != (PlayerItem) null)
        this.primary_equipped_reisou2 = reisou2;
      if (gear3 != (PlayerItem) null)
        this.primary_equipped_reisou3 = reisou3;
      this.resetUsedPrimary();
      this.is_enemy_leader = is_leader;
      this.ai_attack = data.ai_attack;
      this.ai_heal = data.ai_heal;
      this.ai_move = data.ai_move;
      this.ai_move_group = data.ai_move_group;
      this.ai_move_group_order = data.ID;
    }

    public Judgement.NonBattleParameter nonbattleParameter
    {
      get => Judgement.NonBattleParameter.FromPlayerUnit(this);
    }

    public int combat => Judgement.NonBattleParameter.FromPlayerUnit(this).Combat;

    public int total_hp
    {
      get
      {
        return this.unit.IsNormalUnit ? this.hp.initial + this.hp.level + this.hp.compose + this.hp.inheritance + this.hp.buildup + this.hp.transmigrate + this.hp.x_level + this.bonus_hp + this.hp.overkillersValue : 1;
      }
    }

    public int self_total_hp
    {
      get
      {
        return this.unit.IsNormalUnit ? this.hp.initial + this.hp.level + this.hp.compose + this.hp.inheritance + this.hp.buildup + this.hp.transmigrate + this.hp.x_level + this.bonus_hp : 1;
      }
    }

    public int self_hp_without_x
    {
      get
      {
        return this.unit.IsNormalUnit ? this.hp.initial + this.hp.level + this.hp.compose + this.hp.inheritance + this.hp.buildup + this.hp.transmigrate + this.bonus_hp : 1;
      }
    }

    public int bonus_hp => this.getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus.hp_add);

    public int total_strength
    {
      get
      {
        return this.unit.IsNormalUnit ? this.strength.initial + this.strength.level + this.strength.compose + this.strength.inheritance + this.strength.buildup + this.strength.transmigrate + this.strength.x_level + this.bonus_strength + this.strength.overkillersValue : 1;
      }
    }

    public int self_total_strength
    {
      get
      {
        return this.unit.IsNormalUnit ? this.strength.initial + this.strength.level + this.strength.compose + this.strength.inheritance + this.strength.buildup + this.strength.transmigrate + this.strength.x_level + this.bonus_strength : 1;
      }
    }

    public int self_strength_without_x
    {
      get
      {
        return this.unit.IsNormalUnit ? this.strength.initial + this.strength.level + this.strength.compose + this.strength.inheritance + this.strength.buildup + this.strength.transmigrate + this.bonus_strength : 1;
      }
    }

    public int bonus_strength
    {
      get => this.getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus.strength_add);
    }

    public int total_intelligence
    {
      get
      {
        return this.unit.IsNormalUnit ? this.intelligence.initial + this.intelligence.level + this.intelligence.compose + this.intelligence.inheritance + this.intelligence.buildup + this.intelligence.transmigrate + this.intelligence.x_level + this.bonus_intelligence + this.intelligence.overkillersValue : 1;
      }
    }

    public int self_total_intelligence
    {
      get
      {
        return this.unit.IsNormalUnit ? this.intelligence.initial + this.intelligence.level + this.intelligence.compose + this.intelligence.inheritance + this.intelligence.buildup + this.intelligence.transmigrate + this.intelligence.x_level + this.bonus_intelligence : 1;
      }
    }

    public int self_intelligence_without_x
    {
      get
      {
        return this.unit.IsNormalUnit ? this.intelligence.initial + this.intelligence.level + this.intelligence.compose + this.intelligence.inheritance + this.intelligence.buildup + this.intelligence.transmigrate + this.bonus_intelligence : 1;
      }
    }

    public int bonus_intelligence
    {
      get => this.getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus.intelligence_add);
    }

    public int total_vitality
    {
      get
      {
        return this.unit.IsNormalUnit ? this.vitality.initial + this.vitality.level + this.vitality.compose + this.vitality.inheritance + this.vitality.buildup + this.vitality.transmigrate + this.vitality.x_level + this.bonus_vitality + this.vitality.overkillersValue : 1;
      }
    }

    public int self_total_vitality
    {
      get
      {
        return this.unit.IsNormalUnit ? this.vitality.initial + this.vitality.level + this.vitality.compose + this.vitality.inheritance + this.vitality.buildup + this.vitality.transmigrate + this.vitality.x_level + this.bonus_vitality : 1;
      }
    }

    public int self_vitality_without_x
    {
      get
      {
        return this.unit.IsNormalUnit ? this.vitality.initial + this.vitality.level + this.vitality.compose + this.vitality.inheritance + this.vitality.buildup + this.vitality.transmigrate + this.bonus_vitality : 1;
      }
    }

    public int bonus_vitality
    {
      get => this.getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus.vitality_add);
    }

    public int total_dexterity
    {
      get
      {
        return this.unit.IsNormalUnit ? this.dexterity.initial + this.dexterity.level + this.dexterity.compose + this.dexterity.inheritance + this.dexterity.buildup + this.dexterity.transmigrate + this.dexterity.x_level + this.bonus_dexterity + this.dexterity.overkillersValue : 1;
      }
    }

    public int self_total_dexterity
    {
      get
      {
        return this.unit.IsNormalUnit ? this.dexterity.initial + this.dexterity.level + this.dexterity.compose + this.dexterity.inheritance + this.dexterity.buildup + this.dexterity.transmigrate + this.dexterity.x_level + this.bonus_dexterity : 1;
      }
    }

    public int self_dexterity_without_x
    {
      get
      {
        return this.unit.IsNormalUnit ? this.dexterity.initial + this.dexterity.level + this.dexterity.compose + this.dexterity.inheritance + this.dexterity.buildup + this.dexterity.transmigrate + this.bonus_dexterity : 1;
      }
    }

    public int bonus_dexterity
    {
      get => this.getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus.dexterity_add);
    }

    public int total_agility
    {
      get
      {
        return this.unit.IsNormalUnit ? this.agility.initial + this.agility.level + this.agility.compose + this.agility.inheritance + this.agility.buildup + this.agility.transmigrate + this.agility.x_level + this.bonus_agility + this.agility.overkillersValue : 1;
      }
    }

    public int self_total_agility
    {
      get
      {
        return this.unit.IsNormalUnit ? this.agility.initial + this.agility.level + this.agility.compose + this.agility.inheritance + this.agility.buildup + this.agility.transmigrate + this.agility.x_level + this.bonus_agility : 1;
      }
    }

    public int self_agility_without_x
    {
      get
      {
        return this.unit.IsNormalUnit ? this.agility.initial + this.agility.level + this.agility.compose + this.agility.inheritance + this.agility.buildup + this.agility.transmigrate + this.bonus_agility : 1;
      }
    }

    public int bonus_agility
    {
      get => this.getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus.agility_add);
    }

    public int total_mind
    {
      get
      {
        return this.unit.IsNormalUnit ? this.mind.initial + this.mind.level + this.mind.compose + this.mind.inheritance + this.mind.buildup + this.mind.transmigrate + this.mind.x_level + this.bonus_mind + this.mind.overkillersValue : 1;
      }
    }

    public int self_total_mind
    {
      get
      {
        return this.unit.IsNormalUnit ? this.mind.initial + this.mind.level + this.mind.compose + this.mind.inheritance + this.mind.buildup + this.mind.transmigrate + this.mind.x_level + this.bonus_mind : 1;
      }
    }

    public int self_mind_without_x
    {
      get
      {
        return this.unit.IsNormalUnit ? this.mind.initial + this.mind.level + this.mind.compose + this.mind.inheritance + this.mind.buildup + this.mind.transmigrate + this.bonus_mind : 1;
      }
    }

    public int bonus_mind
    {
      get => this.getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus.mind_add);
    }

    public int total_lucky
    {
      get
      {
        return this.unit.IsNormalUnit ? this.lucky.initial + this.lucky.level + this.lucky.compose + this.lucky.inheritance + this.lucky.buildup + this.lucky.transmigrate + this.lucky.x_level + this.bonus_lucky + this.lucky.overkillersValue : 1;
      }
    }

    public int self_total_lucky
    {
      get
      {
        return this.unit.IsNormalUnit ? this.lucky.initial + this.lucky.level + this.lucky.compose + this.lucky.inheritance + this.lucky.buildup + this.lucky.transmigrate + this.lucky.x_level + this.bonus_lucky : 1;
      }
    }

    public int self_lucky_without_x
    {
      get
      {
        return this.unit.IsNormalUnit ? this.lucky.initial + this.lucky.level + this.lucky.compose + this.lucky.inheritance + this.lucky.buildup + this.lucky.transmigrate + this.bonus_lucky : 1;
      }
    }

    public int bonus_lucky
    {
      get => this.getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus.lucky_add);
    }

    public int getJobAbilityLevelmaxBonus(JobCharacteristicsLevelmaxBonus type)
    {
      return this.all_saved_job_abilities == null ? 0 : ((IEnumerable<PlayerUnitAll_saved_job_abilities>) this.all_saved_job_abilities).Sum<PlayerUnitAll_saved_job_abilities>((Func<PlayerUnitAll_saved_job_abilities, int>) (x =>
      {
        int abilityLevelmaxBonus = 0;
        JobCharacteristics jobCharacteristics;
        if (MasterData.JobCharacteristics.TryGetValue(x.job_ability_id, out jobCharacteristics) && jobCharacteristics.skill.upper_level <= x.level)
        {
          if (jobCharacteristics.levelmax_bonus == type)
            abilityLevelmaxBonus += jobCharacteristics.levelmax_bonus_value;
          if (jobCharacteristics.levelmax_bonus2 == type)
            abilityLevelmaxBonus += jobCharacteristics.levelmax_bonus_value2;
          if (jobCharacteristics.levelmax_bonus3 == type)
            abilityLevelmaxBonus += jobCharacteristics.levelmax_bonus_value3;
        }
        return abilityLevelmaxBonus;
      }));
    }

    public int memory_hp
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null ? (this.hp.level_up_max_status < this.MemoryData.hp + this.hp.buildup ? this.hp.level_up_max_status : this.MemoryData.hp + this.hp.buildup) + this.hp.initial + this.hp.compose + this.hp.inheritance + this.hp.transmigrate + this.bonus_hp : 1;
      }
    }

    public int memory_strength
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null ? (this.strength.level_up_max_status < this.MemoryData.strength + this.strength.buildup ? this.strength.level_up_max_status : this.MemoryData.strength + this.strength.buildup) + this.strength.initial + this.strength.compose + this.strength.inheritance + this.strength.transmigrate + this.bonus_strength : 1;
      }
    }

    public int memory_intelligence
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null ? (this.intelligence.level_up_max_status < this.MemoryData.intelligence + this.intelligence.buildup ? this.intelligence.level_up_max_status : this.MemoryData.intelligence + this.intelligence.buildup) + this.intelligence.initial + this.intelligence.compose + this.intelligence.inheritance + this.intelligence.transmigrate + this.bonus_intelligence : 1;
      }
    }

    public int memory_vitality
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null ? (this.vitality.level_up_max_status < this.MemoryData.vitality + this.vitality.buildup ? this.vitality.level_up_max_status : this.MemoryData.vitality + this.vitality.buildup) + this.vitality.initial + this.vitality.compose + this.vitality.inheritance + this.vitality.transmigrate + this.bonus_vitality : 1;
      }
    }

    public int memory_dexterity
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null ? (this.dexterity.level_up_max_status < this.MemoryData.dexterity + this.dexterity.buildup ? this.dexterity.level_up_max_status : this.MemoryData.dexterity + this.dexterity.buildup) + this.dexterity.initial + this.dexterity.compose + this.dexterity.inheritance + this.dexterity.transmigrate + this.bonus_dexterity : 1;
      }
    }

    public int memory_agility
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null ? (this.agility.level_up_max_status < this.MemoryData.agility + this.agility.buildup ? this.agility.level_up_max_status : this.MemoryData.agility + this.agility.buildup) + this.agility.initial + this.agility.compose + this.agility.inheritance + this.agility.transmigrate + this.bonus_agility : 1;
      }
    }

    public int memory_mind
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null ? (this.mind.level_up_max_status < this.MemoryData.mind + this.mind.buildup ? this.mind.level_up_max_status : this.MemoryData.mind + this.mind.buildup) + this.mind.initial + this.mind.compose + this.mind.inheritance + this.mind.transmigrate + this.bonus_mind : 1;
      }
    }

    public int memory_lucky
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null ? (this.lucky.level_up_max_status < this.MemoryData.lucky + this.lucky.buildup ? this.lucky.level_up_max_status : this.MemoryData.lucky + this.lucky.buildup) + this.lucky.initial + this.lucky.compose + this.lucky.inheritance + this.lucky.transmigrate + this.bonus_lucky : 1;
      }
    }

    public bool is_memory_hp_max
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.hp.level_up_max_status <= this.MemoryData.hp + this.hp.buildup;
      }
    }

    public bool is_memory_strength_max
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.strength.level_up_max_status <= this.MemoryData.strength + this.strength.buildup;
      }
    }

    public bool is_memory_intelligence_max
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.intelligence.level_up_max_status <= this.MemoryData.intelligence + this.intelligence.buildup;
      }
    }

    public bool is_memory_vitality_max
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.vitality.level_up_max_status <= this.MemoryData.vitality + this.vitality.buildup;
      }
    }

    public bool is_memory_dexterity_max
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.dexterity.level_up_max_status <= this.MemoryData.dexterity + this.dexterity.buildup;
      }
    }

    public bool is_memory_agility_max
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.agility.level_up_max_status <= this.MemoryData.agility + this.agility.buildup;
      }
    }

    public bool is_memory_mind_max
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.mind.level_up_max_status <= this.MemoryData.mind + this.mind.buildup;
      }
    }

    public bool is_memory_lucky_max
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.lucky.level_up_max_status <= this.MemoryData.lucky + this.lucky.buildup;
      }
    }

    public bool is_memory_over
    {
      get
      {
        return this.is_memory_hp_over || this.is_memory_strength_over || this.is_memory_intelligence_over || this.is_memory_vitality_over || this.is_memory_dexterity_over || this.is_memory_agility_over || this.is_memory_mind_over || this.is_memory_lucky_over;
      }
    }

    public bool is_memory_hp_over
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.hp.level_up_max_status < this.MemoryData.hp + this.hp.buildup;
      }
    }

    public bool is_memory_strength_over
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.strength.level_up_max_status < this.MemoryData.strength + this.strength.buildup;
      }
    }

    public bool is_memory_intelligence_over
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.intelligence.level_up_max_status < this.MemoryData.intelligence + this.intelligence.buildup;
      }
    }

    public bool is_memory_vitality_over
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.vitality.level_up_max_status < this.MemoryData.vitality + this.vitality.buildup;
      }
    }

    public bool is_memory_dexterity_over
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.dexterity.level_up_max_status < this.MemoryData.dexterity + this.dexterity.buildup;
      }
    }

    public bool is_memory_agility_over
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.agility.level_up_max_status < this.MemoryData.agility + this.agility.buildup;
      }
    }

    public bool is_memory_mind_over
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.mind.level_up_max_status < this.MemoryData.mind + this.mind.buildup;
      }
    }

    public bool is_memory_lucky_over
    {
      get
      {
        return this.unit.IsNormalUnit && this.MemoryData != null && this.lucky.level_up_max_status < this.MemoryData.lucky + this.lucky.buildup;
      }
    }

    public int memory_level
    {
      get => this.unit.IsNormalUnit && this.MemoryData != null ? this.MemoryData.level : 1;
    }

    public int compose_hp_max
    {
      get
      {
        return this.UnitTypeParameter.hp_compose_max + this.getComposeAddValue(PlayerUnit.ParamType.HP);
      }
    }

    public int compose_strength_max
    {
      get
      {
        return this.UnitTypeParameter.strength_compose_max + this.getComposeAddValue(PlayerUnit.ParamType.STRENGTH);
      }
    }

    public int compose_vitality_max
    {
      get
      {
        return this.UnitTypeParameter.vitality_compose_max + this.getComposeAddValue(PlayerUnit.ParamType.VITALITY);
      }
    }

    public int compose_intelligence_max
    {
      get
      {
        return this.UnitTypeParameter.intelligence_compose_max + this.getComposeAddValue(PlayerUnit.ParamType.INTELLIGENCE);
      }
    }

    public int compose_mind_max
    {
      get
      {
        return this.UnitTypeParameter.mind_compose_max + this.getComposeAddValue(PlayerUnit.ParamType.MIND);
      }
    }

    public int compose_agility_max
    {
      get
      {
        return this.UnitTypeParameter.agility_compose_max + this.getComposeAddValue(PlayerUnit.ParamType.AGILITY);
      }
    }

    public int compose_dexterity_max
    {
      get
      {
        return this.UnitTypeParameter.dexterity_compose_max + this.getComposeAddValue(PlayerUnit.ParamType.DEXTERITY);
      }
    }

    public int compose_lucky_max
    {
      get
      {
        return this.UnitTypeParameter.lucky_compose_max + this.getComposeAddValue(PlayerUnit.ParamType.LUCKY);
      }
    }

    private string getComposeAddData(PlayerUnit.ParamType type)
    {
      switch (type)
      {
        case PlayerUnit.ParamType.HP:
          return this.unit.compose_max_unity_value_setting_id.hp_compose_add_max;
        case PlayerUnit.ParamType.STRENGTH:
          return this.unit.compose_max_unity_value_setting_id.strength_compose_add_max;
        case PlayerUnit.ParamType.INTELLIGENCE:
          return this.unit.compose_max_unity_value_setting_id.intelligence_compose_add_max;
        case PlayerUnit.ParamType.VITALITY:
          return this.unit.compose_max_unity_value_setting_id.vitality_compose_add_max;
        case PlayerUnit.ParamType.MIND:
          return this.unit.compose_max_unity_value_setting_id.mind_compose_add_max;
        case PlayerUnit.ParamType.AGILITY:
          return this.unit.compose_max_unity_value_setting_id.agility_compose_add_max;
        case PlayerUnit.ParamType.DEXTERITY:
          return this.unit.compose_max_unity_value_setting_id.dexterity_compose_add_max;
        case PlayerUnit.ParamType.LUCKY:
          return this.unit.compose_max_unity_value_setting_id.lucky_compose_add_max;
        default:
          return string.Empty;
      }
    }

    public int getComposeAddValue(PlayerUnit.ParamType type)
    {
      if (this.unit.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting <= 0)
        return 0;
      string composeAddData = this.getComposeAddData(type);
      if (string.IsNullOrEmpty(composeAddData))
        return 0;
      int composeAddValue = 0;
      string str = composeAddData;
      char[] chArray = new char[1]{ ',' };
      foreach (string s in str.Split(chArray))
      {
        int result = 0;
        if (int.TryParse(s, out result) && (double) this.unityTotal >= (double) result)
          ++composeAddValue;
      }
      return composeAddValue;
    }

    public int getComposeAddMax(PlayerUnit.ParamType type)
    {
      if (this.unit.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting <= 0)
        return 0;
      string composeAddData = this.getComposeAddData(type);
      if (string.IsNullOrEmpty(composeAddData))
        return 0;
      int composeAddMax = 0;
      string str = composeAddData;
      char[] chArray = new char[1]{ ',' };
      foreach (string s in str.Split(chArray))
      {
        int result = 0;
        if (int.TryParse(s, out result))
          ++composeAddMax;
      }
      return composeAddMax;
    }

    public PlayerItem equippedGear
    {
      get
      {
        return this.primary_equipped_gear != (PlayerItem) null ? (this.primary_equipped_gear.id == 0 ? (PlayerItem) null : this.primary_equipped_gear) : (this.used_primary != 0 ? (PlayerItem) null : this.FindEquippedGear(SMManager.Get<PlayerItem[]>()));
      }
    }

    public PlayerItem equippedGear2
    {
      get
      {
        if (!this.unit.awake_unit_flag)
          return (PlayerItem) null;
        return this.primary_equipped_gear2 != (PlayerItem) null ? (this.primary_equipped_gear2.id == 0 ? (PlayerItem) null : this.primary_equipped_gear2) : (this.used_primary != 0 ? (PlayerItem) null : this.FindEquippedGear2(SMManager.Get<PlayerItem[]>()));
      }
    }

    public bool isPossibleEquippedGear3 => this.unit.isPossibleEquippedGear3;

    public PlayerItem equippedGear3
    {
      get
      {
        if (!this.isPossibleEquippedGear3)
          return (PlayerItem) null;
        return this.primary_equipped_gear3 != (PlayerItem) null ? (this.primary_equipped_gear3.id == 0 ? (PlayerItem) null : this.primary_equipped_gear3) : (this.used_primary != 0 ? (PlayerItem) null : this.FindEquippedGear3(SMManager.Get<PlayerItem[]>()));
      }
    }

    public PlayerItem equippedReisou
    {
      get
      {
        if (this.primary_equipped_reisou != (PlayerItem) null)
          return this.primary_equipped_reisou.id == 0 ? (PlayerItem) null : this.primary_equipped_reisou;
        if (this.equip_gear_ids == null || this.equip_gear_ids.Length == 0)
          return (PlayerItem) null;
        if (this.used_primary != 0)
          return (PlayerItem) null;
        if (SMManager.Get<PlayerItem[]>() == null)
          return (PlayerItem) null;
        PlayerItem gear1 = this.equippedGear;
        if (gear1 == (PlayerItem) null || !gear1.isReisouSet)
          return (PlayerItem) null;
        PlayerItem playerItem = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x != (PlayerItem) null && x.entity_type == MasterDataTable.CommonRewardType.gear && x.id == gear1.equipped_reisou_player_gear_id && x.isReisou()));
        return playerItem == (PlayerItem) null ? (PlayerItem) null : playerItem;
      }
    }

    public PlayerItem equippedReisou2
    {
      get
      {
        if (!this.unit.awake_unit_flag)
          return (PlayerItem) null;
        if (this.primary_equipped_reisou2 != (PlayerItem) null)
          return this.primary_equipped_reisou2.id == 0 ? (PlayerItem) null : this.primary_equipped_reisou2;
        if (this.used_primary != 0)
          return (PlayerItem) null;
        if (SMManager.Get<PlayerItem[]>() == null)
          return (PlayerItem) null;
        PlayerItem gear2 = this.equippedGear2;
        if (gear2 == (PlayerItem) null || !gear2.isReisouSet)
          return (PlayerItem) null;
        PlayerItem playerItem = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x != (PlayerItem) null && x.entity_type == MasterDataTable.CommonRewardType.gear && x.id == gear2.equipped_reisou_player_gear_id && x.isReisou()));
        return playerItem == (PlayerItem) null ? (PlayerItem) null : playerItem;
      }
    }

    public PlayerItem equippedReisou3
    {
      get
      {
        if (!this.isPossibleEquippedGear3)
          return (PlayerItem) null;
        if (this.primary_equipped_reisou3 != (PlayerItem) null)
          return this.primary_equipped_reisou3.id == 0 ? (PlayerItem) null : this.primary_equipped_reisou3;
        if (this.used_primary != 0)
          return (PlayerItem) null;
        if (SMManager.Get<PlayerItem[]>() == null)
          return (PlayerItem) null;
        PlayerItem gear3 = this.equippedGear3;
        if (gear3 == (PlayerItem) null || !gear3.isReisouSet)
          return (PlayerItem) null;
        PlayerItem playerItem = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x != (PlayerItem) null && x.entity_type == MasterDataTable.CommonRewardType.gear && x.id == gear3.equipped_reisou_player_gear_id && x.isReisou()));
        return playerItem == (PlayerItem) null ? (PlayerItem) null : playerItem;
      }
    }

    public bool IsBrokenEquippedGear
    {
      get
      {
        PlayerItem equippedGear = this.equippedGear;
        PlayerItem equippedGear2 = this.equippedGear2;
        PlayerItem equippedGear3 = this.equippedGear3;
        return (!(equippedGear == (PlayerItem) null) || !(equippedGear2 == (PlayerItem) null) || !(equippedGear3 == (PlayerItem) null)) && (equippedGear != (PlayerItem) null && equippedGear.broken || equippedGear2 != (PlayerItem) null && equippedGear2.broken || equippedGear3 != (PlayerItem) null && equippedGear3.broken);
      }
    }

    public bool IsAllEquipUnit
    {
      get
      {
        return ((IEnumerable<string>) Consts.GetInstance().ALL_GEAR_EQUIP_UNIT_IDS.Split(',')).Contains<string>(this.unit.ID.ToString());
      }
    }

    public PlayerItem FindEquippedGear(PlayerItem[] items)
    {
      if (this.equip_gear_ids.IsNullOrEmpty<int?>() || items.IsNullOrEmpty<PlayerItem>())
        return (PlayerItem) null;
      int? id = this.equip_gear_ids[0];
      if (id.IsInvalid())
        return (PlayerItem) null;
      PlayerItem playerItem = Array.Find<PlayerItem>(items, (Predicate<PlayerItem>) (x =>
      {
        int id1 = x.id;
        int? nullable = id;
        int valueOrDefault = nullable.GetValueOrDefault();
        return id1 == valueOrDefault & nullable.HasValue;
      }));
      return playerItem == (PlayerItem) null ? (PlayerItem) null : playerItem;
    }

    public PlayerItem FindEquippedGear2(PlayerItem[] items, bool isCheckAwake = true)
    {
      if (isCheckAwake && !this.unit.awake_unit_flag)
        return (PlayerItem) null;
      if (this.equip_gear_ids.IsNullOrLess<int?>(2) || items.IsNullOrEmpty<PlayerItem>())
        return (PlayerItem) null;
      int? id = this.equip_gear_ids[1];
      if (id.IsInvalid())
        return (PlayerItem) null;
      PlayerItem playerItem = Array.Find<PlayerItem>(items, (Predicate<PlayerItem>) (x =>
      {
        int id1 = x.id;
        int? nullable = id;
        int valueOrDefault = nullable.GetValueOrDefault();
        return id1 == valueOrDefault & nullable.HasValue;
      }));
      return playerItem == (PlayerItem) null ? (PlayerItem) null : playerItem;
    }

    public PlayerItem FindEquippedGear3(PlayerItem[] items)
    {
      if (!this.isPossibleEquippedGear3)
        return (PlayerItem) null;
      if (!this.unit.awake_unit_flag)
        return this.FindEquippedGear2(items, false);
      if (this.equip_gear_ids.IsNullOrLess<int?>(3) || items.IsNullOrEmpty<PlayerItem>())
        return (PlayerItem) null;
      int? id = this.equip_gear_ids[2];
      if (id.IsInvalid())
        return (PlayerItem) null;
      PlayerItem playerItem = Array.Find<PlayerItem>(items, (Predicate<PlayerItem>) (x =>
      {
        int id1 = x.id;
        int? nullable = id;
        int valueOrDefault = nullable.GetValueOrDefault();
        return id1 == valueOrDefault & nullable.HasValue;
      }));
      return playerItem == (PlayerItem) null ? (PlayerItem) null : playerItem;
    }

    public PlayerItem FindEquippedReisou(PlayerItem[] gears, PlayerGearReisouSchema[] reisous)
    {
      if (reisous.IsNullOrEmpty<PlayerGearReisouSchema>())
        return (PlayerItem) null;
      PlayerItem equippedGear = this.FindEquippedGear(gears);
      if (equippedGear == (PlayerItem) null || !equippedGear.isReisouSet)
        return (PlayerItem) null;
      int id = equippedGear.equipped_reisou_player_gear_id;
      return (id != 0 ? Array.Find<PlayerGearReisouSchema>(reisous, (Predicate<PlayerGearReisouSchema>) (x => x.id == id)) : (PlayerGearReisouSchema) null)?.getReisouItemForSchema();
    }

    public PlayerItem FindEquippedReisou2(PlayerItem[] gears, PlayerGearReisouSchema[] reisous)
    {
      if (reisous.IsNullOrEmpty<PlayerGearReisouSchema>())
        return (PlayerItem) null;
      PlayerItem equippedGear2 = this.FindEquippedGear2(gears);
      if (equippedGear2 == (PlayerItem) null || !equippedGear2.isReisouSet)
        return (PlayerItem) null;
      int id = equippedGear2.equipped_reisou_player_gear_id;
      return (id != 0 ? Array.Find<PlayerGearReisouSchema>(reisous, (Predicate<PlayerGearReisouSchema>) (x => x.id == id)) : (PlayerGearReisouSchema) null)?.getReisouItemForSchema();
    }

    public PlayerItem FindEquippedReisou3(PlayerItem[] gears, PlayerGearReisouSchema[] reisous)
    {
      if (reisous.IsNullOrEmpty<PlayerGearReisouSchema>())
        return (PlayerItem) null;
      PlayerItem equippedGear3 = this.FindEquippedGear3(gears);
      if (equippedGear3 == (PlayerItem) null || !equippedGear3.isReisouSet)
        return (PlayerItem) null;
      int id = equippedGear3.equipped_reisou_player_gear_id;
      return (id != 0 ? Array.Find<PlayerGearReisouSchema>(reisous, (Predicate<PlayerGearReisouSchema>) (x => x.id == id)) : (PlayerGearReisouSchema) null)?.getReisouItemForSchema();
    }

    public PlayerAwakeSkill equippedExtraSkill
    {
      get
      {
        if (this.primary_equipped_awake_skill != null)
          return this.primary_equipped_awake_skill.id == 0 ? (PlayerAwakeSkill) null : this.primary_equipped_awake_skill;
        if (this.used_primary != 0)
          return (PlayerAwakeSkill) null;
        if (this.equip_awake_skill_ids == null)
          return (PlayerAwakeSkill) null;
        if (this.equip_awake_skill_ids.Length == 0)
          return (PlayerAwakeSkill) null;
        return !this.equip_awake_skill_ids[0].HasValue ? (PlayerAwakeSkill) null : ((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).FirstOrDefault<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x =>
        {
          int id = x.id;
          int? equipAwakeSkillId = this.equip_awake_skill_ids[0];
          int valueOrDefault = equipAwakeSkillId.GetValueOrDefault();
          return id == valueOrDefault & equipAwakeSkillId.HasValue;
        })) ?? (PlayerAwakeSkill) null;
      }
    }

    public PlayerAwakeSkill FindEquippedExtraSkill(PlayerAwakeSkill[] skills)
    {
      if (this.equip_awake_skill_ids == null)
        return (PlayerAwakeSkill) null;
      if (this.equip_awake_skill_ids.Length == 0)
        return (PlayerAwakeSkill) null;
      int? id = this.equip_awake_skill_ids[0];
      return !id.HasValue ? (PlayerAwakeSkill) null : ((IEnumerable<PlayerAwakeSkill>) skills).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.id == id.Value)).FirstOrDefault<PlayerAwakeSkill>() ?? (PlayerAwakeSkill) null;
    }

    public UnitTypeParameter UnitTypeParameter
    {
      get
      {
        return ((IEnumerable<UnitTypeParameter>) MasterData.UnitTypeParameterList).Single<UnitTypeParameter>((Func<UnitTypeParameter, bool>) (x => x.unit_type.ID == this.unit_type.ID && x.rarity.ID == this.unit.rarity.ID));
      }
    }

    public int amountIncrementInCompose
    {
      get
      {
        return this.unit.IsNormalUnit ? this.hp.compose + this.vitality.compose + this.strength.compose + this.lucky.compose + this.intelligence.compose + this.mind.compose + this.agility.compose + this.dexterity.compose : 0;
      }
    }

    public bool isMaxParamInCompose
    {
      get
      {
        return this.unit.IsNormalUnit && this.hp.compose >= this.compose_hp_max && this.vitality.compose >= this.compose_vitality_max && this.strength.compose >= this.compose_strength_max && this.lucky.compose >= this.compose_lucky_max && this.intelligence.compose >= this.compose_intelligence_max && this.mind.compose >= this.compose_mind_max && this.agility.compose >= this.compose_agility_max && this.dexterity.compose >= this.compose_dexterity_max;
      }
    }

    public bool isMaxParamInComposeEx
    {
      get
      {
        if (!this.unit.IsNormalUnit)
          return false;
        int num1 = this.isMaxParamInCompose ? 1 : 0;
        bool flag1 = ((IEnumerable<PlayerUnitSkills>) this.skills).Count<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.level < x.skill.upper_level)) == 0;
        bool flag2 = this.breakthrough_count >= this.unit.breakthrough_limit;
        int num2 = flag1 ? 1 : 0;
        return (num1 & num2 & (flag2 ? 1 : 0)) != 0;
      }
    }

    public bool isMaxParamInReinforce => this.buildup_count >= this.buildup_limit;

    public bool isMaxLevelupParam
    {
      get
      {
        return this.hp.is_max && this.strength.is_max && this.intelligence.is_max && this.vitality.is_max && this.mind.is_max && this.agility.is_max && this.lucky.is_max;
      }
    }

    public string equippedGearName
    {
      get
      {
        PlayerItem equippedGear = this.equippedGear;
        return equippedGear == (PlayerItem) null ? this.initial_gear.name : equippedGear.name;
      }
    }

    public string equippedGearName2
    {
      get
      {
        PlayerItem equippedGear2 = this.equippedGear2;
        return equippedGear2 == (PlayerItem) null ? (string) null : equippedGear2.name;
      }
    }

    public string equippedGearName3
    {
      get
      {
        PlayerItem equippedGear3 = this.equippedGear3;
        return equippedGear3 == (PlayerItem) null ? (string) null : equippedGear3.name;
      }
    }

    public GearGear equippedGearOrInitial
    {
      get
      {
        PlayerItem equippedGear = this.equippedGear;
        return equippedGear == (PlayerItem) null ? this.initial_gear : equippedGear.gear;
      }
    }

    public GearGear equippedGear2OrInitial
    {
      get
      {
        PlayerItem equippedGear2 = this.equippedGear2;
        return equippedGear2 == (PlayerItem) null ? this.initial_gear : equippedGear2.gear;
      }
    }

    public GearGear equippedGear3OrInitial
    {
      get
      {
        PlayerItem equippedGear3 = this.equippedGear3;
        return equippedGear3 == (PlayerItem) null ? this.initial_gear : equippedGear3.gear;
      }
    }

    public GearGear equippedWeaponGearOrInitial
    {
      get
      {
        PlayerItem equippedGear = this.equippedGear;
        if (equippedGear == (PlayerItem) null)
          return this.initial_gear;
        if (this.unit.kind_GearKind != 8)
        {
          if (equippedGear.gear.kind.isNonWeapon)
            return this.initial_gear;
        }
        else if (!this.IsAllEquipUnit)
        {
          GearGear initialGear = this.initial_gear;
          if (equippedGear.gear.kind_GearKind != initialGear.kind_GearKind)
            return initialGear;
        }
        return equippedGear.gear;
      }
    }

    public GearGear equippedShieldGearOrNull
    {
      get
      {
        PlayerItem equippedGear;
        if ((equippedGear = this.equippedGear) != (PlayerItem) null && equippedGear.gear.kind_GearKind == 7)
          return equippedGear.gear;
        PlayerItem equippedGear2;
        return (equippedGear2 = this.equippedGear2) != (PlayerItem) null && equippedGear2.gear.kind_GearKind == 7 ? equippedGear2.gear : (GearGear) null;
      }
    }

    public GearGear equippedAssistGear
    {
      get
      {
        PlayerItem equippedGear;
        if ((equippedGear = this.equippedGear) != (PlayerItem) null && equippedGear.gear.kind.isAssist)
          return equippedGear.gear;
        PlayerItem equippedGear2;
        return (equippedGear2 = this.equippedGear2) != (PlayerItem) null && equippedGear2.gear.kind.isAssist ? equippedGear2.gear : (GearGear) null;
      }
    }

    public bool isLeftHandWeapon
    {
      get
      {
        UnitUnitGearModelKind unitGearModelKind = (UnitUnitGearModelKind) null;
        GearGear weaponGearOrInitial = this.equippedWeaponGearOrInitial;
        if (this.job_id != this.unit.job_UnitJob)
          unitGearModelKind = this.unit.getUnitGearModelKind(weaponGearOrInitial.model_kind, this.job_id);
        if (unitGearModelKind == null)
          unitGearModelKind = this.unit.getUnitGearModelKind(weaponGearOrInitial.model_kind);
        return unitGearModelKind != null && unitGearModelKind.is_left_hand_weapon == 1;
      }
    }

    public bool isDualWieldWeapon
    {
      get
      {
        UnitUnitGearModelKind unitGearModelKind = (UnitUnitGearModelKind) null;
        GearGear weaponGearOrInitial = this.equippedWeaponGearOrInitial;
        if (this.job_id != this.unit.job_UnitJob)
          unitGearModelKind = this.unit.getUnitGearModelKind(weaponGearOrInitial.model_kind, this.job_id);
        if (unitGearModelKind == null)
          unitGearModelKind = this.unit.getUnitGearModelKind(weaponGearOrInitial.model_kind);
        return unitGearModelKind != null && unitGearModelKind.is_left_hand_weapon == 2;
      }
    }

    public int buildupLimitBreakCnt
    {
      get
      {
        int buildupLimitBreakCnt = 0;
        foreach (PlayerUnitSkills skill in this.skills)
        {
          PlayerUnitSkills s = skill;
          if (((IEnumerable<BreakThroughBuildupSkill>) MasterData.BreakThroughBuildupSkillList).Any<BreakThroughBuildupSkill>((Func<BreakThroughBuildupSkill, bool>) (x => x.skill_id == s.skill_id)))
            buildupLimitBreakCnt += s.level;
        }
        return buildupLimitBreakCnt;
      }
    }

    public PlayerUnitLeader_skills leader_skill
    {
      get
      {
        return this.leader_skills.Length == 0 ? (PlayerUnitLeader_skills) null : this.leader_skills[0];
      }
    }

    public UnitProficiency proficiency => this.GetProficiency(this.equippedGearOrInitial.kind);

    public UnitProficiency weaponProficiency() => this.GetProficiency(this.unit.kind);

    public UnitProficiency shildProficiency()
    {
      int key = 1;
      if (this.gear_proficiencies != null)
      {
        PlayerUnitGearProficiency unitGearProficiency = Array.Find<PlayerUnitGearProficiency>(this.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == 7));
        key = unitGearProficiency == null ? 1 : unitGearProficiency.level;
      }
      return MasterData.UnitProficiency[key];
    }

    public UnitProficiencyIncr ProficiencyIncr
    {
      get
      {
        return this.GetProficiencyIncr((this.equippedGear != (PlayerItem) null ? this.equippedGear.gear : (this.equippedGear2 != (PlayerItem) null ? this.equippedGear2.gear : this.initial_gear)).kind);
      }
    }

    public UnitProficiency GetProficiency(GearKind kind)
    {
      int key = 1;
      if (this.gear_proficiencies != null)
      {
        PlayerUnitGearProficiency unitGearProficiency = Array.Find<PlayerUnitGearProficiency>(this.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == kind.ID));
        key = unitGearProficiency == null ? 1 : unitGearProficiency.level;
      }
      return MasterData.UnitProficiency[key];
    }

    public UnitProficiencyIncr GetProficiencyIncr(GearKind kind)
    {
      return UnitProficiencyIncr.FromKindProficiency(kind, this.GetProficiency(kind));
    }

    public int MinMagicBulletPower
    {
      get
      {
        if (this.skills == null)
          return 0;
        PlayerUnitSkills[] array = ((IEnumerable<PlayerUnitSkills>) this.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type == BattleskillSkillType.magic)).ToArray<PlayerUnitSkills>();
        return array.Length != 0 ? ((IEnumerable<PlayerUnitSkills>) array).Min<PlayerUnitSkills>((Func<PlayerUnitSkills, int>) (x => x.skill.power)) : 0;
      }
    }

    public float GetElementOrKindRatio(PlayerUnit target)
    {
      UnitFamily[] families = target.Families;
      GearKind kind = this.unit.kind;
      foreach (UnitFamily unitFamily in families)
      {
        UnitFamily family = unitFamily;
        foreach (GearGearElement element1 in this.equippedGearOrInitial.elements)
        {
          GearGearElement element = element1;
          GearElementRatio gearElementRatio = Array.Find<GearElementRatio>(MasterData.GearElementRatioList, (Predicate<GearElementRatio>) (elementRatio => elementRatio.element == element.element && elementRatio.family == family));
          if (gearElementRatio != null)
            return gearElementRatio.ratio;
        }
        GearKindRatio gearKindRatio = Array.Find<GearKindRatio>(MasterData.GearKindRatioList, (Predicate<GearKindRatio>) (kindRatio => kindRatio.kind.ID == kind.ID && kindRatio.family == family));
        if (gearKindRatio != null)
          return gearKindRatio.ratio;
      }
      return 1f;
    }

    public Tuple<int, int> GetGearKindIncr(PlayerUnit target)
    {
      GearKindCorrelations kindCorrelations = MasterData.UniqueGearKindCorrelationsBy(this.unit.kind, target.unit.kind);
      if (kindCorrelations != null)
      {
        GearKindIncr gearKindIncr = MasterData.UniqueGearKindIncrBy(this.unit.kind, target.unit.kind, (kindCorrelations.is_advantage ? this : target).proficiency);
        if (gearKindIncr != null)
          return Tuple.Create<int, int>(gearKindIncr.attack, gearKindIncr.hit);
      }
      return Tuple.Create<int, int>(0, 0);
    }

    public void SetIntimateList(
      PlayerCharacterIntimate[] playerCharactoreIntimates)
    {
      if (playerCharactoreIntimates == null)
        return;
      int characterID = this.unit.character.ID;
      this.my_intimates = ((IEnumerable<PlayerCharacterIntimate>) playerCharactoreIntimates).Where<PlayerCharacterIntimate>((Func<PlayerCharacterIntimate, bool>) (x => x.character.ID == characterID || x.target_character.ID == characterID)).ToArray<PlayerCharacterIntimate>();
    }

    public int GetIntimateValue(PlayerUnit target)
    {
      if (this.my_intimates == null)
        return 0;
      int characterID = target.unit.character.ID;
      PlayerCharacterIntimate characterIntimate = ((IEnumerable<PlayerCharacterIntimate>) this.my_intimates).FirstOrDefault<PlayerCharacterIntimate>((Func<PlayerCharacterIntimate, bool>) (x => x.target_character.ID == characterID || x.character.ID == characterID));
      return characterIntimate == null ? 1 : characterIntimate.level;
    }

    public IntimateDuelSupport GetIntimateDuelSupport(PlayerUnit[] neighborUnits)
    {
      int intimateValue = ((IEnumerable<PlayerUnit>) neighborUnits).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (x => this.GetIntimateValue(x))).Sum();
      return ((IEnumerable<IntimateDuelSupport>) MasterData.IntimateDuelSupportList).Single<IntimateDuelSupport>((Func<IntimateDuelSupport, bool>) (x => x.intimate_value == intimateValue));
    }

    public bool CheckForNormalDeck()
    {
      foreach (PlayerDeck playerDeck in SMManager.Get<PlayerDeck[]>())
      {
        if (playerDeck != null)
        {
          foreach (PlayerUnit playerUnit in playerDeck.player_units)
          {
            if (!(playerUnit == (PlayerUnit) null) && playerUnit.id == this.id)
              return true;
          }
        }
      }
      return false;
    }

    public bool CheckForSeaDeck()
    {
      foreach (PlayerSeaDeck playerSeaDeck in SMManager.Get<PlayerSeaDeck[]>())
      {
        if (playerSeaDeck != null)
        {
          foreach (PlayerUnit playerUnit in playerSeaDeck.player_units)
          {
            if (!(playerUnit == (PlayerUnit) null) && playerUnit.id == this.id)
              return true;
          }
        }
      }
      return false;
    }

    public bool CheckForExploreDeck()
    {
      foreach (ExploreDeck exploreDeck in SMManager.Get<ExploreDeck[]>())
      {
        if (exploreDeck != null)
        {
          foreach (PlayerUnit playerUnit in exploreDeck.player_units)
          {
            if (!(playerUnit == (PlayerUnit) null) && playerUnit.id == this.id)
              return true;
          }
        }
      }
      return false;
    }

    public PlayerUnitSkills[] GetAcquireSkills()
    {
      return this.skills == null ? new PlayerUnitSkills[0] : ((IEnumerable<PlayerUnitSkills>) this.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type != BattleskillSkillType.magic && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<PlayerUnitSkills>();
    }

    public Dictionary<int, int> GetAcquireSkillsDictionary()
    {
      Dictionary<int, int> skillsDictionary = new Dictionary<int, int>();
      foreach (PlayerUnitSkills acquireSkill in this.GetAcquireSkills())
        skillsDictionary.Add(acquireSkill.skill_id, acquireSkill.level);
      return skillsDictionary;
    }

    public UnitSkill[] GetSkills()
    {
      return ((IEnumerable<UnitSkill>) this.unit.RememberUnitSkills(this._unit_type)).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type != BattleskillSkillType.magic && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkill>();
    }

    public BattleskillSkill[] GetBattleSkills()
    {
      List<BattleskillSkill> list = ((IEnumerable<UnitSkill>) this.GetSkills()).Select<UnitSkill, BattleskillSkill>((Func<UnitSkill, BattleskillSkill>) (x => x.skill)).ToList<BattleskillSkill>();
      list.AddRange((IEnumerable<BattleskillSkill>) ((IEnumerable<UnitSkillCharacterQuest>) this.GetCharacterSkills()).Select<UnitSkillCharacterQuest, BattleskillSkill>((Func<UnitSkillCharacterQuest, BattleskillSkill>) (x => x.skill)).ToArray<BattleskillSkill>());
      list.AddRange((IEnumerable<BattleskillSkill>) ((IEnumerable<UnitSkillCharacterQuest>) this.GetCharacterSkills()).Where<UnitSkillCharacterQuest>((Func<UnitSkillCharacterQuest, bool>) (x => x.skill_after_evolution > 0)).Select<UnitSkillCharacterQuest, BattleskillSkill>((Func<UnitSkillCharacterQuest, BattleskillSkill>) (x => x.skillOfEvolution)).ToArray<BattleskillSkill>());
      list.AddRange((IEnumerable<BattleskillSkill>) ((IEnumerable<UnitSkillHarmonyQuest>) this.GetHarmonySkills()).Select<UnitSkillHarmonyQuest, BattleskillSkill>((Func<UnitSkillHarmonyQuest, BattleskillSkill>) (x => x.skill)).ToArray<BattleskillSkill>());
      return list.ToArray();
    }

    public UnitSkillCharacterQuest[] GetCharacterSkills()
    {
      return ((IEnumerable<UnitSkillCharacterQuest>) MasterData.UnitSkillCharacterQuestList).Where<UnitSkillCharacterQuest>((Func<UnitSkillCharacterQuest, bool>) (x => x.unit.ID == this.unit.ID && x.skill.skill_type != BattleskillSkillType.magic && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillCharacterQuest>();
    }

    public UnitSkillHarmonyQuest[] GetHarmonySkills()
    {
      return ((IEnumerable<UnitSkillHarmonyQuest>) MasterData.UnitSkillHarmonyQuestList).Where<UnitSkillHarmonyQuest>((Func<UnitSkillHarmonyQuest, bool>) (x => x.character.ID == this.unit.character.ID && x.skill.skill_type != BattleskillSkillType.magic && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillHarmonyQuest>();
    }

    public UnitSkillIntimate[] GetIntimateSkills()
    {
      return ((IEnumerable<UnitSkillIntimate>) MasterData.UnitSkillIntimateList).Where<UnitSkillIntimate>((Func<UnitSkillIntimate, bool>) (x => x.unit.ID == this.unit.ID && x.skill.skill_type != BattleskillSkillType.magic && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillIntimate>();
    }

    public UnitSkillAwake[] GetAwakeSkills()
    {
      return ((IEnumerable<UnitSkillAwake>) MasterData.UnitSkillAwakeList).Where<UnitSkillAwake>((Func<UnitSkillAwake, bool>) (x => x.character_id == this.unit.same_character_id)).ToArray<UnitSkillAwake>();
    }

    public BattleskillSkill evolutionSkill(BattleskillSkill skill)
    {
      int[] array = ((IEnumerable<PlayerUnitSkills>) this.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type != BattleskillSkillType.magic && x.skill.skill_type != BattleskillSkillType.leader)).Select<PlayerUnitSkills, int>((Func<PlayerUnitSkills, int>) (x => x.skill.ID)).ToArray<int>();
      Dictionary<int, UnitSkillEvolution> dictionary = ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Where<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit.ID == this.unit.ID)).ToDictionary<UnitSkillEvolution, int>((Func<UnitSkillEvolution, int>) (x => x.before_skill.ID));
      return dictionary.ContainsKey(skill.ID) && ((IEnumerable<int>) array).Contains<int>(dictionary[skill.ID].after_skill.ID) ? dictionary[skill.ID].after_skill : skill;
    }

    public void resetCacheMember()
    {
      this._element = new CommonElement?();
      this.unitJob_ = (MasterDataTable.UnitJob) null;
      this.isJobChange_ = new bool?();
      this.families_ = (UnitFamily[]) null;
      this.battleOptionAttacks_ = (IAttackMethod[]) null;
      this.resetCacheOverkillersUnits((PlayerUnit[]) null);
    }

    public CommonElement GetElement()
    {
      if (!this._element.HasValue)
      {
        if (this.skills != null)
        {
          this._element = new CommonElement?(CommonElement.none);
          for (int index = 0; index < this.skills.Length; ++index)
          {
            if (BattleskillSkill.InvestElementSkillIds.Contains(this.skills[index].skill_id))
            {
              this._element = new CommonElement?(this.skills[index].skill.element);
              break;
            }
          }
        }
        else
          this._element = new CommonElement?(this.unit.GetElement());
      }
      return this._element.Value;
    }

    public string GetElementName()
    {
      CommonElementName commonElementName = ((IEnumerable<CommonElementName>) MasterData.CommonElementNameList).First<CommonElementName>((Func<CommonElementName, bool>) (x => (CommonElement) x.ID == this.GetElement()));
      return commonElementName != null ? commonElementName.name : "";
    }

    public float TowerHpRate => Mathf.Max(0.0f, Mathf.Min(this.tower_hitpoint_rate, 100f));

    public int TowerHp
    {
      get
      {
        int totalHp = this.total_hp;
        if ((double) this.TowerHpRate == 100.0)
          return totalHp;
        int towerHp = Mathf.CeilToInt((float) ((double) totalHp * (double) this.TowerHpRate / 100.0));
        if (towerHp == totalHp && (double) this.TowerHpRate < 100.0)
          --towerHp;
        return towerHp;
      }
    }

    public void SetMemoryData(
      PlayerUnitTransMigrateMemoryListTransmigrate_memory memoryData)
    {
      this.memoryData = memoryData;
    }

    public MasterDataTable.UnitJob getJobData()
    {
      return this.unitJob_ ?? (this.unitJob_ = MasterData.UnitJob[this.job_id != 0 ? this.job_id : (this.job_id = this.unit.job_UnitJob)]);
    }

    private void clearCacheRetrofitSkills()
    {
      this.equippedOverkillersSkills_ = (PlayerUnitSkills[]) null;
      this.magicSkills_ = (PlayerUnitSkills[]) null;
      this.passiveSkills_ = (PlayerUnitSkills[]) null;
      this.retrofitSkills_ = (PlayerUnitSkills[]) null;
    }

    public PlayerUnitSkills[] magicSkills
    {
      get
      {
        return this.magicSkills_ ?? (this.magicSkills_ = ((IEnumerable<PlayerUnitSkills>) this.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (s => s.skill.skill_type == BattleskillSkillType.magic)).Concat<PlayerUnitSkills>(((IEnumerable<PlayerUnitSkills>) this.retrofitSkills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (s => s.skill.skill_type == BattleskillSkillType.magic))).ToArray<PlayerUnitSkills>());
      }
    }

    public PlayerUnitSkills[] passiveSkills
    {
      get
      {
        return this.passiveSkills_ ?? (this.passiveSkills_ = ((IEnumerable<PlayerUnitSkills>) this.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (s => s.skill.skill_type == BattleskillSkillType.passive)).Concat<PlayerUnitSkills>(((IEnumerable<PlayerUnitSkills>) this.retrofitSkills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (s => s.skill.skill_type == BattleskillSkillType.passive))).ToArray<PlayerUnitSkills>());
      }
    }

    public PlayerUnitSkills[] retrofitSkills
    {
      get => this.retrofitSkills_ ?? (this.retrofitSkills_ = this.getRetrofitSkills());
    }

    private PlayerUnitSkills[] getRetrofitSkills()
    {
      PlayerUnitSkills seaSkill = this.SEASkill;
      List<PlayerUnitSkills> playerUnitSkillsList = new List<PlayerUnitSkills>((seaSkill != null ? 1 : 0) + (this.job_abilities != null ? this.job_abilities.Length : 0) + OverkillersSlotRelease.MaxSlot);
      if (seaSkill != null)
        playerUnitSkillsList.Add(seaSkill);
      playerUnitSkillsList.AddRange((IEnumerable<PlayerUnitSkills>) this.equippedOverkillersSkills);
      if (this.job_abilities != null && this.job_abilities.Length != 0)
      {
        for (int index = 0; index < this.job_abilities.Length; ++index)
        {
          PlayerUnitJob_abilities jobAbility = this.job_abilities[index];
          if (jobAbility.job_ability_id != 0)
          {
            if (!MasterData.BattleskillSkill.ContainsKey(jobAbility.skill_id))
            {
              Debug.LogError((object) string.Format("Not Found SkillId\"{0}\" in job_abilities", (object) jobAbility.skill_id));
            }
            else
            {
              playerUnitSkillsList.Add(new PlayerUnitSkills()
              {
                skill_id = jobAbility.skill_id,
                level = jobAbility.level
              });
              if (jobAbility.skill2_id != 0)
              {
                if (MasterData.BattleskillSkill.ContainsKey(jobAbility.skill2_id))
                  playerUnitSkillsList.Add(new PlayerUnitSkills()
                  {
                    skill_id = jobAbility.skill2_id,
                    level = jobAbility.level
                  });
                else
                  Debug.LogError((object) string.Format("Not Found Skill2 Id\"{0}\" in job_abilities", (object) jobAbility.skill2_id));
              }
            }
          }
        }
      }
      return playerUnitSkillsList.ToArray();
    }

    public bool isJobChange()
    {
      return !this.isJobChange_.HasValue ? (this.isJobChange_ = new bool?(this.job_id != this.unit.job_UnitJob)).Value : this.isJobChange_.Value;
    }

    public UnitFamily[] Families
    {
      get
      {
        return this.families_ ?? (this.families_ = this.isJobChange() ? this.unit.FamiliesWithJob(this.job_id) : this.unit.Families);
      }
    }

    public bool HasFamily(UnitFamily family)
    {
      return ((IEnumerable<UnitFamily>) this.Families).Any<UnitFamily>((Func<UnitFamily, bool>) (x => x == family));
    }

    public AttackMethod[] attackMethods
    {
      get
      {
        return ((IEnumerable<AttackMethod>) MasterData.AttackMethodList).Where<AttackMethod>((Func<AttackMethod, bool>) (x => x.unit_UnitUnit == this._unit && x.job_UnitJob == this.job_id)).ToArray<AttackMethod>();
      }
    }

    public IAttackMethod[] battleOptionAttacks
    {
      get
      {
        return this.battleOptionAttacks_ ?? (this.battleOptionAttacks_ = ((IEnumerable<AttackMethod>) MasterData.AttackMethodList).Where<AttackMethod>((Func<AttackMethod, bool>) (x => x.unit_UnitUnit == this._unit && x.job_UnitJob == this.job_id)).Select<AttackMethod, IAttackMethod>((Func<AttackMethod, IAttackMethod>) (y => y.CreateInterface())).ToArray<IAttackMethod>());
      }
    }

    public void setBattleOptionAttacks(IAttackMethod[] attacks)
    {
      this.battleOptionAttacks_ = attacks;
    }

    public GearGear initial_gear
    {
      get
      {
        if (this._initial_gear != null)
          return this._initial_gear;
        if (this.isJobChange())
        {
          UnitModel unitModel = Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.initial_gear != null && x.unit_id_UnitUnit == this._unit && x.job_metamor_id == this.job_id));
          if (unitModel != null)
            this._initial_gear = unitModel.initial_gear;
        }
        if (this._initial_gear == null)
          this._initial_gear = this.unit.initial_gear;
        return this._initial_gear;
      }
    }

    public void clearInitialGear() => this._initial_gear = (GearGear) null;

    public bool isDirtyOverkillersSlots => this.is_dirty_overkillers_slots_;

    public int overkillers_base_id
    {
      get
      {
        int? overkillersBaseId = this.getOverkillersBaseId();
        return !overkillersBaseId.HasValue ? -1 : overkillersBaseId.Value;
      }
    }

    protected virtual int? getOverkillersBaseId()
    {
      UnitUnit unit;
      if (this.id == 0 || (unit = this.unit) == null)
        return new int?();
      if (!unit.isPossibleOverkillers())
        return new int?(-1);
      foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
      {
        if (playerUnit.unit.exist_overkillers_slot && playerUnit.over_killers_player_unit_ids != null && playerUnit.over_killers_player_unit_ids.Length != 0 && playerUnit.over_killers_player_unit_ids[0] >= 0)
        {
          for (int index = 0; index < playerUnit.over_killers_player_unit_ids.Length && playerUnit.over_killers_player_unit_ids[index] >= 0; ++index)
          {
            if (playerUnit.over_killers_player_unit_ids[index] == this.id)
              return new int?(playerUnit.id);
          }
        }
      }
      return new int?(0);
    }

    public PlayerUnit[] cache_overkillers_units => this.cache_overkillers_units_;

    public int[] cache_overkillers_unit_ids => this.cache_overkillers_unit_ids_;

    public int[] cache_overkillers_unit_job_ids => this.cache_overkillers_unit_job_ids_;

    public bool isAnyCacheOverkillersUnits
    {
      get
      {
        if (this.cache_overkillers_units_ != null && this.cache_overkillers_units_.Length != 0)
        {
          for (int index = 0; index < this.cache_overkillers_units_.Length; ++index)
          {
            if (this.cache_overkillers_units_[index] != (PlayerUnit) null)
              return true;
          }
        }
        return false;
      }
    }

    public OverkillersSkillRelease overkillersSkill => OverkillersSkillRelease.find(this.unit);

    public bool resetOnceOverkillers()
    {
      int num = this.isDirtyOverkillersSlots ? 1 : 0;
      Player current = Player.Current;
      if (num == 0)
      {
        if (current == null)
          return num != 0;
        if (!(this.player_id == current.id))
          return num != 0;
        if (!this.isModifiedCacheOverkillers)
          return num != 0;
      }
      if (current != null && this.player_id == current.id)
        this.importOverkillersUnits();
      this.resetOverkillersParameter();
      this.resetOverkillersSkills();
      return num != 0;
    }

    private bool isModifiedCacheOverkillers
    {
      get
      {
        PlayerUnit[] array = SMManager.Get<PlayerUnit[]>();
        if (this.cache_overkillers_units == null || array == null || this.cache_overkillers_units.Length == 0 || array.Length == 0)
          return false;
        for (int index = 0; index < this.cache_overkillers_units.Length; ++index)
        {
          PlayerUnit cou = this.cache_overkillers_units[index];
          if (!(cou == (PlayerUnit) null))
          {
            PlayerUnit playerUnit = Array.Find<PlayerUnit>(array, (Predicate<PlayerUnit>) (x => x.id == cou.id));
            if (playerUnit != (PlayerUnit) null && playerUnit.is_dirty_overkillers_parameter_)
              return true;
          }
        }
        return false;
      }
    }

    public void importOverkillersUnits()
    {
      this.importOverkillersUnits(SMManager.Get<PlayerUnit[]>(), true);
    }

    public void importOverkillersUnits(PlayerUnit[] overkillersUnits, bool clearDirtyFlag = false)
    {
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>(OverkillersSlotRelease.MaxSlot);
      if (overkillersUnits != null)
      {
        PlayerUnit overkillersUnit;
        for (int slot_no = 0; (overkillersUnit = this.getOverkillersUnit(overkillersUnits, slot_no)) != (PlayerUnit) null || this.isReleasedOverkillersSlot(slot_no); ++slot_no)
          playerUnitList.Add(overkillersUnit);
      }
      if (clearDirtyFlag)
      {
        foreach (PlayerUnit playerUnit in playerUnitList)
        {
          if (playerUnit != (PlayerUnit) null)
            playerUnit.is_dirty_overkillers_parameter_ = false;
        }
      }
      this.resetCacheOverkillersUnits(playerUnitList.ToArray());
    }

    public void resetCacheOverkillersUnits(PlayerUnit[] playerUnits)
    {
      this.cache_overkillers_units_ = playerUnits;
      this.cache_overkillers_unit_ids_ = (int[]) null;
      this.cache_overkillers_unit_job_ids_ = (int[]) null;
      if (playerUnits != null)
      {
        List<int> intList1 = new List<int>();
        List<int> intList2 = new List<int>();
        for (int index = 0; index < playerUnits.Length; ++index)
        {
          if (!(playerUnits[index] == (PlayerUnit) null))
          {
            intList1.Add(playerUnits[index].unit.ID);
            intList2.Add(playerUnits[index].job_id);
          }
        }
        this.cache_overkillers_unit_ids_ = intList1.ToArray();
        this.cache_overkillers_unit_job_ids_ = intList2.ToArray();
      }
      this.clearCacheRetrofitSkills();
      this.is_dirty_overkillers_slots_ = true;
    }

    public bool isReleasedOverkillersSlot(int slot_no)
    {
      return this.unit.exist_overkillers_slot && this.over_killers_player_unit_ids != null && this.over_killers_player_unit_ids.Length > slot_no && this.over_killers_player_unit_ids[slot_no] > -1;
    }

    public bool isAnyOverkillersUnits
    {
      get
      {
        if (!this.unit.exist_overkillers_slot || this.over_killers_player_unit_ids == null)
          return false;
        for (int index = 0; index < this.over_killers_player_unit_ids.Length && this.over_killers_player_unit_ids[index] >= 0; ++index)
        {
          if (this.over_killers_player_unit_ids[index] > 0)
            return true;
        }
        return false;
      }
    }

    public bool hasOverkillersUnit(int slot_no)
    {
      return this.unit.exist_overkillers_slot && this.over_killers_player_unit_ids != null && this.over_killers_player_unit_ids.Length > slot_no && this.over_killers_player_unit_ids[slot_no] > 0;
    }

    public PlayerUnit getOverkillersUnit(PlayerUnit[] units, int slot_no)
    {
      if (!this.hasOverkillersUnit(slot_no))
        return (PlayerUnit) null;
      int killersPlayerUnitId = this.over_killers_player_unit_ids[slot_no];
      foreach (PlayerUnit unit in units)
      {
        if (!(unit == (PlayerUnit) null) && unit.id == killersPlayerUnitId)
          return unit;
      }
      return (PlayerUnit) null;
    }

    public void clearOverkillersParameter()
    {
      this.dexterity.resetOverkillersValue();
      this.intelligence.resetOverkillersValue();
      this.mind.resetOverkillersValue();
      this.strength.resetOverkillersValue();
      this.agility.resetOverkillersValue();
      this.hp.resetOverkillersValue();
      this.lucky.resetOverkillersValue();
      this.vitality.resetOverkillersValue();
      this.is_dirty_overkillers_slots_ = false;
    }

    public void resetOverkillersParameter()
    {
      if (this.cache_overkillers_units == null || this.cache_overkillers_units.Length == 0)
        return;
      UnitUnit unit = this.unit;
      if (unit == null || !unit.exist_overkillers_slot)
        return;
      OverkillersSlotRelease overkillersSlotRelease = OverkillersSlotRelease.find(unit.same_character_id);
      if (overkillersSlotRelease == null)
        return;
      int length = overkillersSlotRelease.getConditions().Length;
      do
        ;
      while (--length >= 0 && !this.isReleasedOverkillersSlot(length));
      if (length < 0)
        return;
      int num1 = length + 1;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = 0;
      int num6 = 0;
      int num7 = 0;
      int num8 = 0;
      int num9 = 0;
      for (int index = 0; index < num1; ++index)
      {
        PlayerUnit cacheOverkillersUnit = this.cache_overkillers_units.Length > index ? this.cache_overkillers_units[index] : (PlayerUnit) null;
        if (!(cacheOverkillersUnit == (PlayerUnit) null))
        {
          OverkillersParameter overkillersParameter = this.getOverkillersParameter(cacheOverkillersUnit);
          if (overkillersParameter != null)
          {
            num2 += OverkillersParameter.calcParameter(cacheOverkillersUnit.self_total_dexterity, overkillersParameter.dexterity);
            num3 += OverkillersParameter.calcParameter(cacheOverkillersUnit.self_total_intelligence, overkillersParameter.intelligence);
            num4 += OverkillersParameter.calcParameter(cacheOverkillersUnit.self_total_mind, overkillersParameter.mind);
            num5 += OverkillersParameter.calcParameter(cacheOverkillersUnit.self_total_strength, overkillersParameter.strength);
            num6 += OverkillersParameter.calcParameter(cacheOverkillersUnit.self_total_agility, overkillersParameter.agility);
            num7 += OverkillersParameter.calcParameter(cacheOverkillersUnit.self_total_hp, overkillersParameter.hp);
            num8 += OverkillersParameter.calcParameter(cacheOverkillersUnit.self_total_lucky, overkillersParameter.lucky);
            num9 += OverkillersParameter.calcParameter(cacheOverkillersUnit.self_total_vitality, overkillersParameter.vitality);
          }
        }
      }
      this.dexterity.resetOverkillersValue(num2);
      this.intelligence.resetOverkillersValue(num3);
      this.mind.resetOverkillersValue(num4);
      this.strength.resetOverkillersValue(num5);
      this.agility.resetOverkillersValue(num6);
      this.hp.resetOverkillersValue(num7);
      this.lucky.resetOverkillersValue(num8);
      this.vitality.resetOverkillersValue(num9);
      this.is_dirty_overkillers_slots_ = false;
    }

    public OverkillersParameter getOverkillersParameter(PlayerUnit target_unit)
    {
      return OverkillersParameter.getParameter(this.getOverkillersParameterNo(this.unit.ID, target_unit.unit), (int) target_unit.unityTotal);
    }

    public int getOverkillersParameterNo(int base_unit_id, UnitUnit target_unit)
    {
      foreach (OverkillersGroup overkillersGroup in MasterData.OverkillersGroupList)
      {
        if (overkillersGroup.child_unit_id == target_unit.ID)
        {
          int[] parentUnitIds = overkillersGroup.parent_unit_ids;
          for (int index = 0; index < parentUnitIds.Length; ++index)
          {
            if (parentUnitIds[index] == base_unit_id)
              return overkillersGroup.parameter_no_list[index];
          }
        }
      }
      return target_unit.overkillers_parameter;
    }

    public PlayerUnitSkills[] equippedOverkillersSkills
    {
      get
      {
        if (this.equippedOverkillersSkills_ != null)
          return this.equippedOverkillersSkills_;
        this.resetOverkillersSkills();
        return this.equippedOverkillersSkills_;
      }
    }

    public void resetOverkillersSkills()
    {
      List<PlayerUnitSkills> playerUnitSkillsList = new List<PlayerUnitSkills>(OverkillersSlotRelease.MaxSlot);
      if (this.isAnyCacheOverkillersUnits)
      {
        for (int index = 0; index < this.cache_overkillers_units.Length; ++index)
        {
          OverkillersSkillRelease overkillersSkill;
          if (this.cache_overkillers_units[index] != (PlayerUnit) null && (overkillersSkill = this.cache_overkillers_units[index].overkillersSkill) != null && (double) overkillersSkill.unity_value <= (double) this.cache_overkillers_units[index].unityTotal)
            playerUnitSkillsList.Add(new PlayerUnitSkills()
            {
              skill_id = overkillersSkill.skill_BattleskillSkill,
              level = overkillersSkill.skill.upper_level
            });
        }
      }
      this.clearCacheRetrofitSkills();
      this.equippedOverkillersSkills_ = playerUnitSkillsList.ToArray();
    }

    public static int SEASkillUnlockConditions
    {
      get
      {
        if (PlayerUnit.SEA_skill_unlock_conditions_.HasValue)
          return PlayerUnit.SEA_skill_unlock_conditions_.Value;
        PlayerUnit.SEA_skill_unlock_conditions_ = new int?(int.Parse(Consts.GetInstance().SEA_SKILL_UNLOCK_CONDITIONS));
        return PlayerUnit.SEA_skill_unlock_conditions_.Value;
      }
    }

    public bool hasSEASkills
    {
      get
      {
        UnitUnit unit = this.unit;
        return unit != null && unit.hasSEASkills;
      }
    }

    public bool isUnlockedSEASkill => this.isUnlockedSEASkillWithoutMasterData && this.hasSEASkills;

    public bool isUnlockedSEASkillWithoutMasterData
    {
      get => this.isReleasedOverkillersSlot(PlayerUnit.SEASkillUnlockConditions - 1);
    }

    public int countEquippedOverkillers
    {
      get
      {
        return this.over_killers_player_unit_ids == null || this.over_killers_player_unit_ids.Length == 0 ? 0 : ((IEnumerable<int>) this.over_killers_player_unit_ids).Count<int>((Func<int, bool>) (n => n > 0));
      }
    }

    public PlayerUnitSkills SEASkill => this.getSEASkill(this.countEquippedOverkillers);

    public PlayerUnitSkills getSEASkill(int numEquipped = 0, bool bCheckUnlocked = true)
    {
      UnitSEASkill seaSkill = this.unit?.SEASkill;
      if (seaSkill == null || !seaSkill.hasSkills)
        return (PlayerUnitSkills) null;
      if (bCheckUnlocked && (numEquipped <= 0 || !this.isUnlockedSEASkillWithoutMasterData))
        return (PlayerUnitSkills) null;
      if (numEquipped > 0)
        return seaSkill.getSkill(numEquipped - 1);
      PlayerUnitSkills skill = seaSkill.getSkill(0);
      if (skill != null)
        skill.level = 0;
      return skill;
    }

    public IEnumerable<int> getPrincessSkillIds(bool bExcludeMagic)
    {
      Func<UnitSkill, bool> predicate = bExcludeMagic ? (Func<UnitSkill, bool>) (x => x.unit_type == this._unit_type && x.skill.skill_type != BattleskillSkillType.magic) : (Func<UnitSkill, bool>) (x => x.unit_type == this._unit_type);
      return ((IEnumerable<UnitSkill>) this.unit.RememberUnitAllSkills()).Where<UnitSkill>(predicate).Select<UnitSkill, int>((Func<UnitSkill, int>) (y => y.skill_BattleskillSkill));
    }

    public int total_level => this.level + this.x_level;

    public int total_max_level => this.max_level + this.max_x_level;

    public int x_level
    {
      get => this.x_job_status == null ? 0 : UnitXLevel.expToLevel(this.x_job_status.total_exp);
    }

    public int max_x_level
    {
      get
      {
        if (this.all_saved_job_abilities == null || this.all_saved_job_abilities.Length == 0)
          return 0;
        int maxXLevel = 0;
        for (int index = 0; index < this.all_saved_job_abilities.Length; ++index)
        {
          PlayerUnitAll_saved_job_abilities allSavedJobAbility = this.all_saved_job_abilities[index];
          JobCharacteristics jobCharacteristics;
          if (MasterData.JobCharacteristics.TryGetValue(allSavedJobAbility.job_ability_id, out jobCharacteristics))
          {
            XLevelLimits xlevelLimits = jobCharacteristics.xlevel_limits;
            if (xlevelLimits != null)
              maxXLevel += xlevelLimits.getLimit(allSavedJobAbility.level);
          }
        }
        return maxXLevel;
      }
    }

    public bool hasXLevel
    {
      get
      {
        MasterDataTable.UnitJob unitJob;
        return this.changed_job_ids != null && this.changed_job_ids.Length != 0 && ((IEnumerable<int?>) this.changed_job_ids).Any<int?>((Func<int?, bool>) (v => v.HasValue && MasterData.UnitJob.TryGetValue(v.Value, out unitJob) && unitJob.is_vertex_x));
      }
    }

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

    public PlayerUnit()
    {
    }

    public PlayerUnit(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this.dexterity = json[nameof (dexterity)] == null ? (PlayerUnitDexterity) null : new PlayerUnitDexterity((Dictionary<string, object>) json[nameof (dexterity)]);
      this.can_equip_awake_skill = (bool) json[nameof (can_equip_awake_skill)];
      this.intelligence = json[nameof (intelligence)] == null ? (PlayerUnitIntelligence) null : new PlayerUnitIntelligence((Dictionary<string, object>) json[nameof (intelligence)]);
      this.move = (int) (long) json[nameof (move)];
      this.mind = json[nameof (mind)] == null ? (PlayerUnitMind) null : new PlayerUnitMind((Dictionary<string, object>) json[nameof (mind)]);
      this.tower_is_entry = (bool) json[nameof (tower_is_entry)];
      this.player_id = (string) json[nameof (player_id)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this._unit = (int) (long) json[nameof (unit)];
      this.is_trust = (bool) json[nameof (is_trust)];
      this.strength = json[nameof (strength)] == null ? (PlayerUnitStrength) null : new PlayerUnitStrength((Dictionary<string, object>) json[nameof (strength)]);
      this.equip_gear_ids = ((IEnumerable<object>) json[nameof (equip_gear_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      this.job_id = (int) (long) json[nameof (job_id)];
      this.equip_awake_skill_ids = ((IEnumerable<object>) json[nameof (equip_awake_skill_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      this.breakthrough_count = (int) (long) json[nameof (breakthrough_count)];
      this.buildup_unity_value_f = (float) (double) json[nameof (buildup_unity_value_f)];
      this._unit_type = (int) (long) json[nameof (unit_type)];
      this.tower_hitpoint_rate = (float) (double) json[nameof (tower_hitpoint_rate)];
      this.over_killers_player_unit_ids = ((IEnumerable<object>) json[nameof (over_killers_player_unit_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      List<PlayerUnitJob_abilities> unitJobAbilitiesList = new List<PlayerUnitJob_abilities>();
      foreach (object json1 in (List<object>) json[nameof (job_abilities)])
        unitJobAbilitiesList.Add(json1 == null ? (PlayerUnitJob_abilities) null : new PlayerUnitJob_abilities((Dictionary<string, object>) json1));
      this.job_abilities = unitJobAbilitiesList.ToArray();
      this.changed_job_ids = ((IEnumerable<object>) json[nameof (changed_job_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
      this.hp = json[nameof (hp)] == null ? (PlayerUnitHp) null : new PlayerUnitHp((Dictionary<string, object>) json[nameof (hp)]);
      this.unity_value = (int) (long) json[nameof (unity_value)];
      List<PlayerUnitAll_saved_job_abilities> savedJobAbilitiesList = new List<PlayerUnitAll_saved_job_abilities>();
      foreach (object json2 in (List<object>) json[nameof (all_saved_job_abilities)])
        savedJobAbilitiesList.Add(json2 == null ? (PlayerUnitAll_saved_job_abilities) null : new PlayerUnitAll_saved_job_abilities((Dictionary<string, object>) json2));
      this.all_saved_job_abilities = savedJobAbilitiesList.ToArray();
      this.agility = json[nameof (agility)] == null ? (PlayerUnitAgility) null : new PlayerUnitAgility((Dictionary<string, object>) json[nameof (agility)]);
      List<PlayerUnitLeader_skills> unitLeaderSkillsList = new List<PlayerUnitLeader_skills>();
      foreach (object json3 in (List<object>) json[nameof (leader_skills)])
        unitLeaderSkillsList.Add(json3 == null ? (PlayerUnitLeader_skills) null : new PlayerUnitLeader_skills((Dictionary<string, object>) json3));
      this.leader_skills = unitLeaderSkillsList.ToArray();
      this.max_level = (int) (long) json[nameof (max_level)];
      this.buildup_limit = (int) (long) json[nameof (buildup_limit)];
      this.lucky = json[nameof (lucky)] == null ? (PlayerUnitLucky) null : new PlayerUnitLucky((Dictionary<string, object>) json[nameof (lucky)]);
      this.vitality = json[nameof (vitality)] == null ? (PlayerUnitVitality) null : new PlayerUnitVitality((Dictionary<string, object>) json[nameof (vitality)]);
      this.trust_rate = (float) (double) json[nameof (trust_rate)];
      List<PlayerUnitGearProficiency> unitGearProficiencyList = new List<PlayerUnitGearProficiency>();
      foreach (object json4 in (List<object>) json[nameof (gear_proficiencies)])
        unitGearProficiencyList.Add(json4 == null ? (PlayerUnitGearProficiency) null : new PlayerUnitGearProficiency((Dictionary<string, object>) json4));
      this.gear_proficiencies = unitGearProficiencyList.ToArray();
      this.exp_next = (int) (long) json[nameof (exp_next)];
      List<PlayerUnitX_job_proficiencies> jobProficienciesList = new List<PlayerUnitX_job_proficiencies>();
      foreach (object json5 in (List<object>) json[nameof (x_job_proficiencies)])
        jobProficienciesList.Add(json5 == null ? (PlayerUnitX_job_proficiencies) null : new PlayerUnitX_job_proficiencies((Dictionary<string, object>) json5));
      this.x_job_proficiencies = jobProficienciesList.ToArray();
      this.level = (int) (long) json[nameof (level)];
      List<PlayerUnitSkills> playerUnitSkillsList = new List<PlayerUnitSkills>();
      foreach (object json6 in (List<object>) json[nameof (skills)])
        playerUnitSkillsList.Add(json6 == null ? (PlayerUnitSkills) null : new PlayerUnitSkills((Dictionary<string, object>) json6));
      this.skills = playerUnitSkillsList.ToArray();
      this.created_at = DateTime.Parse((string) json[nameof (created_at)]);
      this.total_exp = (int) (long) json[nameof (total_exp)];
      this.favorite = (bool) json[nameof (favorite)];
      this.x_job_status = json[nameof (x_job_status)] == null ? (PlayerUnitXJobStatus) null : new PlayerUnitXJobStatus((Dictionary<string, object>) json[nameof (x_job_status)]);
      this.exp = (int) (long) json[nameof (exp)];
      this.buildup_count = (int) (long) json[nameof (buildup_count)];
    }

    public void resetByCustomDeck()
    {
      this.primary_equipped_gear = (PlayerItem) null;
      this.primary_equipped_gear2 = (PlayerItem) null;
      this.primary_equipped_gear3 = (PlayerItem) null;
      this.primary_equipped_reisou = (PlayerItem) null;
      this.primary_equipped_reisou2 = (PlayerItem) null;
      this.primary_equipped_reisou3 = (PlayerItem) null;
      this.primary_equipped_awake_skill = (PlayerAwakeSkill) null;
      if (this.equip_gear_ids.Length != 0)
        this.equip_gear_ids = new int?[0];
      if (this.equip_awake_skill_ids.Length != 0)
        this.equip_awake_skill_ids = new int?[0];
      if (this.unit.exist_overkillers_slot && this.over_killers_player_unit_ids.Length != 0)
      {
        this.hp = PlayerUnit.classClone<PlayerUnitHp>(this.hp);
        this.strength = PlayerUnit.classClone<PlayerUnitStrength>(this.strength);
        this.vitality = PlayerUnit.classClone<PlayerUnitVitality>(this.vitality);
        this.intelligence = PlayerUnit.classClone<PlayerUnitIntelligence>(this.intelligence);
        this.mind = PlayerUnit.classClone<PlayerUnitMind>(this.mind);
        this.agility = PlayerUnit.classClone<PlayerUnitAgility>(this.agility);
        this.dexterity = PlayerUnit.classClone<PlayerUnitDexterity>(this.dexterity);
        this.lucky = PlayerUnit.classClone<PlayerUnitLucky>(this.lucky);
        this.over_killers_player_unit_ids = ((IEnumerable<int>) this.over_killers_player_unit_ids).Select<int, int>((Func<int, int>) (i => i >= 0 ? 0 : i)).ToArray<int>();
        this.clearOverkillersParameter();
        this.resetOverkillersParameter();
        this.resetOverkillersSkills();
      }
      this.is_dirty_overkillers_parameter_ = false;
      this.is_dirty_overkillers_slots_ = false;
      this.usedPrimary = PlayerUnit.UsedPrimary.All;
    }

    private static T classClone<T>(T src) where T : class
    {
      using (MemoryStream serializationStream = new MemoryStream())
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize((Stream) serializationStream, (object) src);
        serializationStream.Seek(0L, SeekOrigin.Begin);
        return (T) binaryFormatter.Deserialize((Stream) serializationStream);
      }
    }

    public void restoreByCustomDeck(PlayerUnit src, Util.RestoreUnit restoreUnit)
    {
      foreach (FieldInfo field in typeof (PlayerUnit).GetFields(BindingFlags.Instance | BindingFlags.Public))
        field.SetValue((object) this, field.GetValue((object) src));
      this._hasKey = true;
      this._key = (object) this.id;
      this.resetCacheMember();
      this.usedPrimary = PlayerUnit.UsedPrimary.None;
      if (restoreUnit == null)
        return;
      this.equip_gear_ids = restoreUnit.equip_gear_ids;
      this.over_killers_player_unit_ids = restoreUnit.over_killers_player_unit_ids;
      this.equip_awake_skill_ids = restoreUnit.equip_awake_skill_ids;
    }

    public bool corps_is_entry
    {
      get
      {
        if (!this._corps_is_entry.HasValue)
          this._corps_is_entry = new bool?(Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids.Contains(this.id));
        return this._corps_is_entry.Value;
      }
    }

    public static int UnityToPercent(float unity) => (int) unity.CarryPercent();

    public int unityInt => this.unityTotal.GetInteger();

    public int unityDec => this.unityTotal.GetDecimalAsPercent();

    public string SpecialEffectType(
      IEnumerable<QuestScoreBonusTimetable> activeTables,
      IEnumerable<UnitBonus> activeUnitBonus)
    {
      string str = (string) null;
      foreach (QuestScoreBonusTimetable activeTable in activeTables)
      {
        QuestScoreBonusTimetable table = activeTable;
        if (table.rules != null && (table.targeUnitIds.Count<int>() > 0 && table.targeUnitIds.Contains(this._unit) || table.targetSkillIds.Count<int>() > 0 && ((IEnumerable<PlayerUnitSkills>) this.GetAcquireSkills()).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => table.targetSkillIds.Contains(x.skill_id))) != null || table.targetJobIds.Count<int>() > 0 && table.targetJobIds.Contains(this.unit.job_UnitJob)))
        {
          str = table.color_code;
          break;
        }
      }
      if (string.IsNullOrEmpty(str))
      {
        foreach (UnitBonus activeUnitBonu in activeUnitBonus)
        {
          if (activeUnitBonu != null && activeUnitBonu.target_unit_id_list != null && ((IEnumerable<int>) activeUnitBonu.target_unit_id_list).Contains<int>(this.unit.ID))
          {
            str = activeUnitBonu.color_code;
            break;
          }
        }
      }
      return str;
    }

    public string SpecialEffectFactor(
      IEnumerable<QuestScoreBonusTimetable> activeTables,
      IEnumerable<UnitBonus> activeUnitBonus)
    {
      string str = (string) null;
      foreach (QuestScoreBonusTimetable activeTable in activeTables)
      {
        QuestScoreBonusRule[] rules = activeTable.rules;
        if (rules != null)
        {
          foreach (QuestScoreBonusRule questScoreBonusRule in rules)
          {
            QuestScoreBonusRule rule = questScoreBonusRule;
            int? nullable;
            switch (rule.bonus_type)
            {
              case 1:
                nullable = rule.target_unit_id;
                int id = this.unit.ID;
                if (nullable.GetValueOrDefault() == id & nullable.HasValue)
                {
                  str = activeTable.GetBreakthroughRate(this.breakthrough_count);
                  break;
                }
                break;
              case 2:
                if (((IEnumerable<PlayerUnitSkills>) this.GetAcquireSkills()).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x =>
                {
                  int skillId = x.skill_id;
                  int? targetSkillId = rule.target_skill_id;
                  int valueOrDefault = targetSkillId.GetValueOrDefault();
                  return skillId == valueOrDefault & targetSkillId.HasValue;
                })) != null)
                {
                  str = activeTable.GetBreakthroughRate(this.breakthrough_count);
                  break;
                }
                break;
              case 3:
                nullable = rule.target_job_id;
                int jobUnitJob = this.unit.job_UnitJob;
                if (nullable.GetValueOrDefault() == jobUnitJob & nullable.HasValue)
                {
                  str = activeTable.GetBreakthroughRate(this.breakthrough_count);
                  break;
                }
                break;
            }
          }
        }
      }
      if (string.IsNullOrEmpty(str))
      {
        foreach (UnitBonus activeUnitBonu in activeUnitBonus)
        {
          if (activeUnitBonu != null && activeUnitBonu.target_unit_id_list != null && ((IEnumerable<int>) activeUnitBonu.target_unit_id_list).Contains<int>(this.unit.ID))
          {
            str = activeUnitBonu.GetBreakthroughRate(this.breakthrough_count);
            break;
          }
        }
      }
      return str;
    }

    public string SpecialEffectEventName(
      IEnumerable<QuestScoreBonusTimetable> activeTables,
      IEnumerable<UnitBonus> activeUnitBonus)
    {
      string str = (string) null;
      foreach (QuestScoreBonusTimetable activeTable in activeTables)
      {
        QuestScoreBonusRule[] rules = activeTable.rules;
        if (rules != null)
        {
          foreach (QuestScoreBonusRule questScoreBonusRule in rules)
          {
            QuestScoreBonusRule rule = questScoreBonusRule;
            int? nullable;
            switch (rule.bonus_type)
            {
              case 1:
                nullable = rule.target_unit_id;
                int id = this.unit.ID;
                if (nullable.GetValueOrDefault() == id & nullable.HasValue)
                {
                  str = MasterData.QuestExtraS[activeTable.quest_s_id].quest_l.name;
                  break;
                }
                break;
              case 2:
                if (((IEnumerable<PlayerUnitSkills>) this.GetAcquireSkills()).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x =>
                {
                  int skillId = x.skill_id;
                  int? targetSkillId = rule.target_skill_id;
                  int valueOrDefault = targetSkillId.GetValueOrDefault();
                  return skillId == valueOrDefault & targetSkillId.HasValue;
                })) != null)
                {
                  str = MasterData.QuestExtraS[activeTable.quest_s_id].quest_l.name;
                  break;
                }
                break;
              case 3:
                nullable = rule.target_job_id;
                int jobUnitJob = this.unit.job_UnitJob;
                if (nullable.GetValueOrDefault() == jobUnitJob & nullable.HasValue)
                {
                  str = MasterData.QuestExtraS[activeTable.quest_s_id].quest_l.name;
                  break;
                }
                break;
            }
          }
        }
      }
      if (string.IsNullOrEmpty(str))
      {
        foreach (UnitBonus activeUnitBonu in activeUnitBonus)
        {
          if (activeUnitBonu != null && activeUnitBonu.target_unit_id_list != null && ((IEnumerable<int>) activeUnitBonu.target_unit_id_list).Contains<int>(this.unit.ID))
          {
            str = activeUnitBonu.eventPeriod.event_name;
            break;
          }
        }
      }
      return str;
    }

    public void setStatusOpenedEquippedGear3(bool bOpened)
    {
      this.noOpenedEquippedGear3_ = this.isPossibleEquippedGear3 & bOpened ? (byte) 2 : (byte) 1;
    }

    public bool isOpenedEquippedGear3
    {
      get
      {
        if (this.noOpenedEquippedGear3_ == (byte) 0 && this.player_id == Player.Current.id)
          this.setStatusOpenedEquippedGear3(Singleton<NGGameDataManager>.GetInstance().opened_equip_number_player_unit_ids.Contains(this.id));
        return this.noOpenedEquippedGear3_ == (byte) 2;
      }
    }

    public bool hasAwakeState
    {
      get
      {
        return ((IEnumerable<int>) SMManager.Get<PlayerAwakeStatePlayerUnitIds>().awake_state_player_unit_ids).Contains<int>(this.id);
      }
    }

    public int[] equippedGearIndexMapByCustomDeck
    {
      get
      {
        UnitUnit unit = this.unit;
        if (unit == null)
          return new int[0];
        if (!unit.awake_unit_flag)
        {
          if (!unit.isPossibleEquippedGear3 || !this.isOpenedEquippedGear3)
            return new int[1];
          return new int[2]{ 0, 2 };
        }
        return !unit.isPossibleEquippedGear3 || !this.isOpenedEquippedGear3 ? new int[2]
        {
          0,
          1
        } : new int[3]{ 0, 1, 2 };
      }
    }

    public int[] equippedGearIndexMapWithoutConditions
    {
      get
      {
        UnitUnit unit = this.unit;
        if (unit == null)
          return new int[0];
        if (!unit.awake_unit_flag)
        {
          if (!unit.isPossibleEquippedGear3)
            return new int[1];
          return new int[2]{ 0, 2 };
        }
        return !unit.isPossibleEquippedGear3 ? new int[2]
        {
          0,
          1
        } : new int[3]{ 0, 1, 2 };
      }
    }

    public bool hasOverkillersState
    {
      get
      {
        int[] killersPlayerUnitIds = this.over_killers_player_unit_ids;
        return killersPlayerUnitIds != null && ((IEnumerable<int>) killersPlayerUnitIds).Any<int>((Func<int, bool>) (i => i >= 0));
      }
    }

    public static PlayerUnit create_by_unitunit(Helper helper)
    {
      return new PlayerUnit()
      {
        _unit = helper.leader_unit_from_cache.ID,
        level = helper.leader_unit_level,
        job_id = helper.leader_unit_job_id != 0 ? helper.leader_unit_job_id : helper.leader_unit_from_cache.job_UnitJob
      };
    }

    public static PlayerUnit create_by_unitunit(Gladiator gladiator)
    {
      return new PlayerUnit()
      {
        _unit = MasterData.UnitUnit[gladiator.leader_unit_id].ID,
        level = gladiator.leader_unit_level,
        job_id = gladiator.leader_unit_job_id != 0 ? gladiator.leader_unit_job_id : MasterData.UnitUnit[gladiator.leader_unit_id].job_UnitJob
      };
    }

    public static Dictionary<UnitTypeEnum, PlayerUnit>[] create_for_pickup(
      WebAPI.Response.GachaGetPickupUnitMaxStatus data)
    {
      Dictionary<UnitTypeEnum, PlayerUnit>[] forPickup = new Dictionary<UnitTypeEnum, PlayerUnit>[data.pickup_units.Length];
      for (int index1 = 0; index1 < forPickup.Length; ++index1)
      {
        WebAPI.Response.GachaGetPickupUnitMaxStatusPickup_units pickupUnit = data.pickup_units[index1];
        forPickup[index1] = new Dictionary<UnitTypeEnum, PlayerUnit>(pickupUnit.statuses.Length);
        UnitUnit unit = MasterData.UnitUnit[pickupUnit.unit_id];
        UnitUnitParameter parameterData = unit.parameter_data;
        MasterDataTable.UnitJob job = unit.job;
        UnitBattleSkillOrigin[] leaderSkill = unit.MakeLeaderSkillOrigins();
        UnitBattleSkillOrigin[][] skills = unit.MakeSkillOrigins();
        for (int index2 = 0; index2 < pickupUnit.statuses.Length; ++index2)
          forPickup[index1].Add((UnitTypeEnum) pickupUnit.statuses[index2].unit_type, PlayerUnit.create_for_pickup(unit, parameterData, job, leaderSkill, skills, pickupUnit.statuses[index2]));
      }
      return forPickup;
    }

    private static PlayerUnit create_for_pickup(
      UnitUnit unit,
      UnitUnitParameter param,
      MasterDataTable.UnitJob job,
      UnitBattleSkillOrigin[] leaderSkill,
      UnitBattleSkillOrigin[][] skills,
      WebAPI.Response.GachaGetPickupUnitMaxStatusPickup_unitsStatuses data)
    {
      int num = param._initial_max_level + param.breakthrough_limit * param._level_per_breakthrough;
      return new PlayerUnit()
      {
        dexterity = new PlayerUnitDexterity()
        {
          initial = data.dexterity,
          is_max = true
        },
        intelligence = new PlayerUnitIntelligence()
        {
          initial = data.intelligence,
          is_max = true
        },
        move = job.movement,
        mind = new PlayerUnitMind()
        {
          initial = data.mind,
          is_max = true
        },
        _unit = unit.ID,
        strength = new PlayerUnitStrength()
        {
          initial = data.strength,
          is_max = true
        },
        job_id = job.ID,
        breakthrough_count = param.breakthrough_limit,
        _unit_type = data.unit_type,
        hp = new PlayerUnitHp()
        {
          initial = data.hp,
          is_max = true
        },
        unity_value = PlayerUnit.GetUnityValueMax(),
        agility = new PlayerUnitAgility()
        {
          initial = data.agility,
          is_max = true
        },
        leader_skills = PlayerUnit.makeLeaderSkillsForPickup(leaderSkill),
        max_level = num,
        lucky = new PlayerUnitLucky()
        {
          initial = data.lucky,
          is_max = true
        },
        vitality = new PlayerUnitVitality()
        {
          initial = data.vitality,
          is_max = true
        },
        level = num,
        skills = PlayerUnit.makeSkillsForPickup(skills, data.unit_type),
        equip_gear_ids = new int?[0]
      };
    }

    public static Dictionary<UnitTypeEnum, PlayerUnit> create_for_pickup_without_param(int unitID)
    {
      UnitUnit unit = MasterData.UnitUnit[unitID];
      UnitTypeEnum[] validUnitTypes = unit.validUnitTypes;
      Dictionary<UnitTypeEnum, PlayerUnit> pickupWithoutParam = new Dictionary<UnitTypeEnum, PlayerUnit>(validUnitTypes.Length);
      UnitUnitParameter parameterData = unit.parameter_data;
      MasterDataTable.UnitJob job = unit.job;
      UnitBattleSkillOrigin[] leaderSkill = unit.MakeLeaderSkillOrigins();
      UnitBattleSkillOrigin[][] skills = unit.MakeSkillOrigins();
      for (int index = 0; index < validUnitTypes.Length; ++index)
        pickupWithoutParam.Add(validUnitTypes[index], PlayerUnit.create_for_pickup_without_param(unit, validUnitTypes[index], parameterData, job, leaderSkill, skills));
      return pickupWithoutParam;
    }

    private static PlayerUnit create_for_pickup_without_param(
      UnitUnit unit,
      UnitTypeEnum unitType,
      UnitUnitParameter param,
      MasterDataTable.UnitJob job,
      UnitBattleSkillOrigin[] leaderSkill,
      UnitBattleSkillOrigin[][] skills)
    {
      int initialMaxLevel = param._initial_max_level;
      return new PlayerUnit()
      {
        dexterity = new PlayerUnitDexterity()
        {
          initial = param.dexterity_initial + job.dexterity_initial
        },
        intelligence = new PlayerUnitIntelligence()
        {
          initial = param.intelligence_initial + job.intelligence_initial
        },
        move = job.movement,
        mind = new PlayerUnitMind()
        {
          initial = param.mind_initial + job.mind_initial
        },
        _unit = unit.ID,
        strength = new PlayerUnitStrength()
        {
          initial = param.strength_initial + job.strength_initial
        },
        job_id = job.ID,
        _unit_type = (int) unitType,
        hp = new PlayerUnitHp()
        {
          initial = Math.Max(1, param.hp_initial + job.hp_initial)
        },
        unity_value = PlayerUnit.GetUnityValueMax(),
        agility = new PlayerUnitAgility()
        {
          initial = param.agility_initial + job.agility_initial
        },
        leader_skills = PlayerUnit.makeLeaderSkillsForPickup(leaderSkill),
        max_level = initialMaxLevel,
        lucky = new PlayerUnitLucky()
        {
          initial = param.lucky_initial + job.lucky_initial
        },
        vitality = new PlayerUnitVitality()
        {
          initial = param.vitality_initial + job.vitality_initial
        },
        level = initialMaxLevel,
        skills = PlayerUnit.makeSkillsForPickup(skills, (int) unitType),
        equip_gear_ids = new int?[0]
      };
    }

    private static PlayerUnitLeader_skills[] makeLeaderSkillsForPickup(UnitBattleSkillOrigin[] skill)
    {
      UnitBattleSkillOrigin battleSkillOrigin = ((IEnumerable<UnitBattleSkillOrigin>) skill).LastOrDefault<UnitBattleSkillOrigin>();
      if (battleSkillOrigin == null)
        return new PlayerUnitLeader_skills[0];
      return new PlayerUnitLeader_skills[1]
      {
        new PlayerUnitLeader_skills()
        {
          skill_id = battleSkillOrigin.skill_.ID,
          level = battleSkillOrigin.skill_.upper_level
        }
      };
    }

    private static PlayerUnitSkills[] makeSkillsForPickup(
      UnitBattleSkillOrigin[][] skills,
      int unitType)
    {
      return ((IEnumerable<UnitBattleSkillOrigin[]>) skills).Where<UnitBattleSkillOrigin[]>((Func<UnitBattleSkillOrigin[], bool>) (x =>
      {
        UnitBattleSkillOrigin battleSkillOrigin = ((IEnumerable<UnitBattleSkillOrigin>) x).First<UnitBattleSkillOrigin>();
        if (battleSkillOrigin.IsOriginAwake)
          return false;
        return !battleSkillOrigin.IsOriginBasic || battleSkillOrigin.Basic.unit_type == 0 || battleSkillOrigin.Basic.unit_type == unitType;
      })).Select<UnitBattleSkillOrigin[], PlayerUnitSkills>((Func<UnitBattleSkillOrigin[], PlayerUnitSkills>) (y =>
      {
        BattleskillSkill skill = ((IEnumerable<UnitBattleSkillOrigin>) y).Last<UnitBattleSkillOrigin>().skill_;
        return new PlayerUnitSkills()
        {
          skill_id = skill.ID,
          level = skill.upper_level
        };
      })).ToArray<PlayerUnitSkills>();
    }

    public Future<GameObject> LoadEquippedNonShieldGearModel()
    {
      return this.unit.non_disp_weapon > 0 ? Future.Single<GameObject>((GameObject) null) : MasterData.GearGear[this.equippedWeaponGearOrInitial.ID].LoadModel();
    }

    public Future<GameObject> LoadEquippedGearModel()
    {
      return this.unit.non_disp_weapon > 0 ? Future.Single<GameObject>((GameObject) null) : MasterData.GearGear[this.equippedGearOrInitial.ID].LoadModel();
    }

    public string getDuelAnimatorControllerName(int metamorId = 0)
    {
      return this.getGearModelKind(metamorId).duel_animator_controller_name;
    }

    public Future<RuntimeAnimatorController> LoadDuelAnimator(int metamorId = 0)
    {
      return Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(string.Format("Animators/duel/{0}", (object) this.getDuelAnimatorControllerName(metamorId)));
    }

    public string getWinAnimatorControllerName(int metamorId = 0)
    {
      return this.getGearModelKind(metamorId).winning_animator_controller_name;
    }

    private UnitUnitGearModelKind getGearModelKind(int metamorId)
    {
      GearGear gearGear = this.equippedWeaponGearOrInitial;
      UnitUnitGearModelKind gearModelKind = (UnitUnitGearModelKind) null;
      while (true)
      {
        if (this.isJobChange() || metamorId != 0)
          gearModelKind = this.getUnitGearModelKind(gearGear.model_kind, metamorId);
        if (gearModelKind == null)
        {
          gearModelKind = this.unit.getUnitGearModelKind(gearGear.model_kind);
          if (gearModelKind == null && gearGear != this.initial_gear)
            gearGear = this.initial_gear;
          else
            break;
        }
        else
          break;
      }
      return gearModelKind;
    }

    public Future<RuntimeAnimatorController> LoadWinAnimator(int metamorId = 0)
    {
      return Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(string.Format("Animators/duel_win/{0}", (object) this.getWinAnimatorControllerName(metamorId)));
    }

    public Future<GameObject> LoadModelDuel(int metamorId = 0)
    {
      if (metamorId != 0)
        return this.unit.LoadModelDuel(metamorId);
      return this.isJobChange() ? this.unit.LoadModelDuel(this.job_id) : this.unit.LoadModelDuel();
    }

    public Future<GameObject> LoadModelField(int metamorId = 0)
    {
      if (metamorId != 0)
        return this.unit.LoadModelField(metamorId);
      return this.isJobChange() ? this.unit.LoadModelField(this.job_id) : this.unit.LoadModelField();
    }

    public Future<GameObject> LoadModelUnitAuraEffect(
      out string attach_node,
      int metamorId = 0,
      bool isLightModel = false)
    {
      return metamorId != 0 || !this.isJobChange() ? this.unit.LoadModelUnitAuraEffect(out attach_node, metamorId, isLightModel) : this.unit.LoadModelUnitAuraEffect(out attach_node, this.job_id, isLightModel);
    }

    public UnitUnitGearModelKind getUnitGearModelKind(GearModelKind gearModelKind, int metamorId = 0)
    {
      return (metamorId != 0 ? Array.Find<UnitUnitGearModelKind>(MasterData.UnitUnitGearModelKindList, (Predicate<UnitUnitGearModelKind>) (x => x.unit_UnitUnit == this._unit && x.gear_model_kind_GearModelKind == gearModelKind.ID && x.job_metamor_id.HasValue && x.job_metamor_id.Value == metamorId)) : (UnitUnitGearModelKind) null) ?? Array.Find<UnitUnitGearModelKind>(MasterData.UnitUnitGearModelKindList, (Predicate<UnitUnitGearModelKind>) (x => x.unit_UnitUnit == this._unit && x.gear_model_kind_GearModelKind == gearModelKind.ID && x.job_metamor_id.HasValue && x.job_metamor_id.Value == this.job_id));
    }

    public Future<Material> LoadFieldNormalFaceMaterial(int metamorId = 0)
    {
      if (metamorId != 0)
        return this.unit.LoadFieldNormalFaceMaterial(metamorId);
      return this.isJobChange() ? this.unit.LoadFieldNormalFaceMaterial(this.job_id) : this.unit.LoadFieldNormalFaceMaterial();
    }

    public int SmallCategoryId
    {
      get
      {
        if (!this._smallCategoryId.HasValue)
          this._smallCategoryId = new int?(this.unit.SmallCategoryId);
        return this._smallCategoryId.Value;
      }
    }

    public UnitCutinInfo CutinInfo
    {
      get
      {
        if (this.cutinInfo_ == null)
          this.cutinInfo_ = UnitCutinInfo.find(this.unit) ?? new UnitCutinInfo();
        return this.cutinInfo_;
      }
    }

    private enum UnitType
    {
      Player,
      Enemy,
      Guest,
    }

    public enum ParamType
    {
      HP,
      STRENGTH,
      INTELLIGENCE,
      VITALITY,
      MIND,
      AGILITY,
      DEXTERITY,
      LUCKY,
    }

    public class DualWieldSkillData
    {
      public PlayerUnitJob_abilities jobAbility;
      public BattleskillEffect skillEffect;
    }

    [Flags]
    public enum UsedPrimary
    {
      None = 0,
      EquippedGear = 1,
      EquippedGear2 = 2,
      EquippedReisou = 4,
      EquippedReisou2 = 8,
      EquippedSkill = 16, // 0x00000010
      EquippedGear3 = 32, // 0x00000020
      EquippedReisou3 = 64, // 0x00000040
      All = EquippedReisou3 | EquippedGear3 | EquippedSkill | EquippedReisou2 | EquippedReisou | EquippedGear2 | EquippedGear, // 0x0000007F
    }
  }
}
