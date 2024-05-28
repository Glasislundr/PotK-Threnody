// Decompiled with JetBrains decompiler
// Type: Earth.EarthCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace Earth
{
  [Serializable]
  public class EarthCharacter : BL.ModelBase
  {
    private int mID;
    private int mIndex;
    private int mBattleIndex;
    private int mEventQuestBattleIndex;
    private bool mIsFall;
    private bool mIsDesert;
    private int mCharactorID;
    private int mUnitID;
    private int mExperience;
    private int mEquipeGearID;
    private int mLevelupHp;
    private int mLevelupStrength;
    private int mLevelupIntelligence;
    private int mLevelupVitality;
    private int mLevelupMind;
    private int mLevelupAgility;
    private int mLevelupDexterity;
    private int mLevelupLucky;
    private Dictionary<int, int> mGearProficiencyExperiences;
    [NonSerialized]
    private PlayerUnit mPlayerUnit;
    [NonSerialized]
    private BL.BattleModified<EarthCharacter> mModified;
    private static readonly string serverDataFormat = "{{\"id\":{15},\"battle_index\":{16},\"event_quest_battle_index\":{18},\"index\":{0},\"is_fall\":{1},\"is_desert\":{17},\"character_id\":{2},\"unit_id\":{3},\"experience\":{4},\"equipe_gear_id\":{5},\"levelup_hp\":{6},\"levelup_strength\":{7},\"levelup_intelligence\":{8},\"levelup_vitality\":{9},\"levelup_mind\":{10},\"levelup_agility\":{11},\"levelup_dexteruty\":{12},\"levelup_lucky\":{13},\"gear_proficiencies\":[{14}]}}";
    private static readonly string serverGearProficiencyDataFormat = "{{\"gear_kind_id\":{0},\"proficiency\":{1}}}";

    public int ID => this.mID;

    public int index => this.mIndex;

    public int battleIndex => this.mBattleIndex;

    public int eventQuestBattleIndex => this.mEventQuestBattleIndex;

    public bool isFall => this.mIsFall;

    public bool isDesert => this.mIsDesert;

    public int equipeGearID => this.mEquipeGearID;

    public UnitCharacter character => MasterData.UnitCharacter[this.mCharactorID];

    public UnitUnit unit => MasterData.UnitUnit[this.mUnitID];

    public UnitLevel unitLevel
    {
      get
      {
        UnitLevel unitLevel = ((IEnumerable<UnitLevel>) MasterData.UnitLevelList).Where<UnitLevel>((Func<UnitLevel, bool>) (x => x.pattern_id == this.unit.parameter_data._level_pattern_id)).FirstOrDefault<UnitLevel>((Func<UnitLevel, bool>) (x => x.from_exp <= this.mExperience && x.to_exp >= this.mExperience));
        if (unitLevel == null || this.unit.parameter_data._initial_max_level <= unitLevel.level)
        {
          unitLevel = ((IEnumerable<UnitLevel>) MasterData.UnitLevelList).Where<UnitLevel>((Func<UnitLevel, bool>) (x => x.pattern_id == this.unit.parameter_data._level_pattern_id)).FirstOrDefault<UnitLevel>((Func<UnitLevel, bool>) (x => x.level == this.unit.parameter_data._initial_max_level));
          this.mExperience = unitLevel.from_exp;
        }
        return unitLevel;
      }
    }

    public int experience => this.mExperience;

    public static EarthCharacter Create(EarthJoinCharacter data, int index)
    {
      EarthCharacter earthCharacter = new EarthCharacter();
      earthCharacter.mID = EarthDataManager.GetAutoIndex();
      earthCharacter.mCharactorID = data.charctor.ID;
      earthCharacter.mUnitID = data.unit.ID;
      earthCharacter.mExperience = data.experience;
      earthCharacter.mIsFall = false;
      earthCharacter.mIsDesert = false;
      earthCharacter.mIndex = !data.unit.IsNormalUnit ? -1 : index;
      earthCharacter.mBattleIndex = 0;
      earthCharacter.mEventQuestBattleIndex = 0;
      earthCharacter.mLevelupHp = data.initial_add_hp;
      earthCharacter.mLevelupStrength = data.initial_add_strength;
      earthCharacter.mLevelupIntelligence = data.initial_add_intelligence;
      earthCharacter.mLevelupVitality = data.initial_add_vitality;
      earthCharacter.mLevelupMind = data.initial_add_mind;
      earthCharacter.mLevelupAgility = data.initial_add_agility;
      earthCharacter.mLevelupDexterity = data.initial_add_dexterity;
      earthCharacter.mLevelupLucky = data.initial_add_lucky;
      if (data.gear != null)
      {
        EarthGear earthGear = Singleton<EarthDataManager>.GetInstance().EarthGearAdd(data.gear.ID);
        earthCharacter.mEquipeGearID = earthGear.ID;
      }
      earthCharacter.mGearProficiencyExperiences = new Dictionary<int, int>();
      earthCharacter.mGearProficiencyExperiences.Add((int) data.unit.kind.Enum, MasterData.UnitProficiencyLevel[MasterData.UnitUnitSupplement[data.unit.ID].default_weapon_proficiency.ID].from_exp);
      earthCharacter.mGearProficiencyExperiences.Add(7, MasterData.UnitProficiencyLevel[MasterData.UnitUnitSupplement[data.unit.ID].default_shield_proficiency.ID].from_exp);
      if (data.unit.IsNormalUnit)
        Singleton<EarthDataManager>.GetInstance().CreateIntimate(earthCharacter.mCharactorID);
      earthCharacter.commit();
      return earthCharacter;
    }

    public static EarthCharacter Create(int characterID, int unitID, int index)
    {
      EarthCharacter earthCharacter = new EarthCharacter();
      earthCharacter.mID = EarthDataManager.GetAutoIndex();
      earthCharacter.mCharactorID = characterID;
      earthCharacter.mUnitID = unitID;
      earthCharacter.mExperience = 0;
      earthCharacter.mIsFall = false;
      earthCharacter.mIndex = index;
      earthCharacter.mBattleIndex = 0;
      earthCharacter.mEventQuestBattleIndex = 0;
      earthCharacter.mLevelupHp = 0;
      earthCharacter.mLevelupStrength = 0;
      earthCharacter.mLevelupIntelligence = 0;
      earthCharacter.mLevelupVitality = 0;
      earthCharacter.mLevelupMind = 0;
      earthCharacter.mLevelupAgility = 0;
      earthCharacter.mLevelupDexterity = 0;
      earthCharacter.mLevelupLucky = 0;
      earthCharacter.mEquipeGearID = 0;
      earthCharacter.mGearProficiencyExperiences = new Dictionary<int, int>();
      earthCharacter.commit();
      return earthCharacter;
    }

    public void SetDeath()
    {
      this.mIsFall = true;
      this.mEquipeGearID = 0;
      this.mIndex = -1;
      this.commit();
    }

    public void SetRebarth()
    {
      this.mIsFall = false;
      this.mIndex = int.MaxValue;
      this.commit();
    }

    public void SetDesert(bool isResert)
    {
      if (this.mIsDesert == isResert)
        return;
      this.mIsDesert = isResert;
      this.mEquipeGearID = 0;
      this.mIndex = -1;
      this.commit();
    }

    private int CalcUpParamter(float growth)
    {
      return (double) Random.value >= (double) Mathf.Min(0.95f, growth) ? 0 : 1;
    }

    public void AddExperience(int experience, Dictionary<int, int> gearProficiencyExperiences)
    {
      UnitLevel unitLevel1 = this.unitLevel;
      this.mExperience = Mathf.Max(0, this.mExperience + experience);
      UnitLevel unitLevel2 = this.unitLevel;
      if (this.unitLevel.level > unitLevel1.level)
      {
        int num1 = this.unitLevel.level - unitLevel1.level;
        UnitUnitGrowth unitUnitGrowth = MasterData.UnitUnitGrowth[this.mUnitID];
        while (num1 > 0)
        {
          int num2 = this.CalcUpParamter(unitUnitGrowth.hp_growth);
          int num3 = this.CalcUpParamter(unitUnitGrowth.strength_growth);
          int num4 = this.CalcUpParamter(unitUnitGrowth.intelligence_growth);
          int num5 = this.CalcUpParamter(unitUnitGrowth.vitality_growth);
          int num6 = this.CalcUpParamter(unitUnitGrowth.mind_growth);
          int num7 = this.CalcUpParamter(unitUnitGrowth.agility_growth);
          int num8 = this.CalcUpParamter(unitUnitGrowth.dexterity_growth);
          int num9 = this.CalcUpParamter(unitUnitGrowth.lucky_growth);
          if (num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9 > 0)
          {
            this.mLevelupHp = Mathf.Min(this.mLevelupHp + num2, this.unit.hp_max);
            this.mLevelupStrength = Mathf.Min(this.mLevelupStrength + num3, this.unit.strength_max);
            this.mLevelupIntelligence = Mathf.Min(this.mLevelupIntelligence + num4, this.unit.intelligence_max);
            this.mLevelupVitality = Mathf.Min(this.mLevelupVitality + num5, this.unit.vitality_max);
            this.mLevelupMind = Mathf.Min(this.mLevelupMind + num6, this.unit.mind_max);
            this.mLevelupAgility = Mathf.Min(this.mLevelupAgility + num7, this.unit.agility_max);
            this.mLevelupDexterity = Mathf.Min(this.mLevelupDexterity + num8, this.unit.dexterity_max);
            this.mLevelupLucky = Mathf.Min(this.mLevelupLucky + num9, this.unit.lucky_max);
            --num1;
          }
        }
      }
      foreach (KeyValuePair<int, int> proficiencyExperience in gearProficiencyExperiences)
      {
        if (this.mGearProficiencyExperiences.ContainsKey(proficiencyExperience.Key))
          this.mGearProficiencyExperiences[proficiencyExperience.Key] += proficiencyExperience.Value;
        else
          this.mGearProficiencyExperiences.Add(proficiencyExperience.Key, proficiencyExperience.Value);
        int exp = this.mGearProficiencyExperiences[proficiencyExperience.Key];
        if (((IEnumerable<UnitProficiencyLevel>) MasterData.UnitProficiencyLevelList).FirstOrDefault<UnitProficiencyLevel>((Func<UnitProficiencyLevel, bool>) (x => x.from_exp <= exp && x.to_exp >= exp)) == null)
          exp = ((IEnumerable<UnitProficiencyLevel>) MasterData.UnitProficiencyLevelList).OrderByDescending<UnitProficiencyLevel, int>((Func<UnitProficiencyLevel, int>) (x => x.level)).FirstOrDefault<UnitProficiencyLevel>().from_exp;
      }
      this.commit();
    }

    public int EquipGear(int gear_id)
    {
      int mEquipeGearId = this.mEquipeGearID;
      this.mEquipeGearID = gear_id;
      this.commit();
      if (!(this.mPlayerUnit != (PlayerUnit) null))
        return mEquipeGearId;
      if (this.mEquipeGearID != 0)
      {
        this.mPlayerUnit.equip_gear_ids[0] = new int?(this.mEquipeGearID);
        return mEquipeGearId;
      }
      this.mPlayerUnit.equip_gear_ids[0] = new int?();
      return mEquipeGearId;
    }

    public int SetIndex(int index)
    {
      int mIndex = this.mIndex;
      this.mIndex = index;
      return mIndex;
    }

    public int SetBattleIndex(int battleIndex)
    {
      int mBattleIndex = this.mBattleIndex;
      this.mBattleIndex = battleIndex;
      return mBattleIndex;
    }

    public int SetEventQuestBattleIndex(int battleIndex)
    {
      int questBattleIndex = this.mEventQuestBattleIndex;
      this.mEventQuestBattleIndex = battleIndex;
      return questBattleIndex;
    }

    public bool Evolution(int patternID)
    {
      UnitEvolutionPattern evolutionPattern = MasterData.UnitEvolutionPattern[patternID];
      if (evolutionPattern.unit.ID != this.mUnitID || evolutionPattern.threshold_level > this.unitLevel.level)
        return false;
      this.mUnitID = evolutionPattern.target_unit.ID;
      this.mExperience = 0;
      this.commit();
      return true;
    }

    public PlayerUnit GetPlayerUnit(bool isCopy = false)
    {
      if (this.mPlayerUnit != (PlayerUnit) null && !this.mModified.isChangedOnce())
      {
        if (!isCopy)
          return this.mPlayerUnit;
        PlayerUnit playerUnit = CopyUtil.DeepCopy<PlayerUnit>(this.mPlayerUnit);
        playerUnit.equip_gear_ids[0] = new int?();
        if (this.mEquipeGearID != 0)
          playerUnit.equip_gear_ids[0] = new int?(this.mEquipeGearID);
        return playerUnit;
      }
      if (this.mPlayerUnit == (PlayerUnit) null)
      {
        this.mModified = BL.Observe<EarthCharacter>(this);
        this.mPlayerUnit = PlayerUnit.CreateForKey(this.mID);
        this.mPlayerUnit.player_id = SMManager.Get<Player>().id;
        this.mPlayerUnit.hp = new PlayerUnitHp();
        this.mPlayerUnit.strength = new PlayerUnitStrength();
        this.mPlayerUnit.intelligence = new PlayerUnitIntelligence();
        this.mPlayerUnit.vitality = new PlayerUnitVitality();
        this.mPlayerUnit.mind = new PlayerUnitMind();
        this.mPlayerUnit.agility = new PlayerUnitAgility();
        this.mPlayerUnit.dexterity = new PlayerUnitDexterity();
        this.mPlayerUnit.lucky = new PlayerUnitLucky();
        this.mPlayerUnit.equip_gear_ids = new int?[1];
      }
      this.mPlayerUnit._unit = this.mUnitID;
      this.mPlayerUnit.id = this.mID;
      this.mPlayerUnit.is_enemy = false;
      this.mPlayerUnit.favorite = false;
      this.mPlayerUnit._unit_type = 1;
      UnitUnit unit = this.mPlayerUnit.unit;
      this.mPlayerUnit.job_id = unit.job_UnitJob;
      this.mPlayerUnit.total_exp = this.mExperience;
      this.mPlayerUnit.exp = this.mExperience - this.unitLevel.from_exp;
      this.mPlayerUnit.exp_next = this.unitLevel.to_exp - this.mExperience;
      this.mPlayerUnit.level = this.unitLevel.level;
      this.mPlayerUnit.max_level = unit.parameter_data._initial_max_level;
      this.mPlayerUnit.move = unit.job.movement;
      this.mPlayerUnit.hp.initial = unit.hp_initial + unit.job.hp_initial;
      this.mPlayerUnit.hp.level = this.mLevelupHp;
      this.mPlayerUnit.strength.initial = unit.strength_initial + unit.job.strength_initial;
      this.mPlayerUnit.strength.level = this.mLevelupStrength;
      this.mPlayerUnit.intelligence.initial = unit.intelligence_initial + unit.job.intelligence_initial;
      this.mPlayerUnit.intelligence.level = this.mLevelupIntelligence;
      this.mPlayerUnit.vitality.initial = unit.vitality_initial + unit.job.vitality_initial;
      this.mPlayerUnit.vitality.level = this.mLevelupVitality;
      this.mPlayerUnit.mind.initial = unit.mind_initial + unit.job.mind_initial;
      this.mPlayerUnit.mind.level = this.mLevelupMind;
      this.mPlayerUnit.agility.initial = unit.agility_initial + unit.job.agility_initial;
      this.mPlayerUnit.agility.level = this.mLevelupAgility;
      this.mPlayerUnit.dexterity.initial = unit.dexterity_initial + unit.job.dexterity_initial;
      this.mPlayerUnit.dexterity.level = this.mLevelupDexterity;
      this.mPlayerUnit.lucky.initial = unit.lucky_initial + unit.job.lucky_initial;
      this.mPlayerUnit.lucky.level = this.mLevelupLucky;
      this.mPlayerUnit.equip_gear_ids[0] = new int?();
      if (this.mEquipeGearID != 0)
        this.mPlayerUnit.equip_gear_ids[0] = new int?(this.mEquipeGearID);
      List<PlayerUnitGearProficiency> unitGearProficiencyList = new List<PlayerUnitGearProficiency>();
      foreach (KeyValuePair<int, int> proficiencyExperience in this.mGearProficiencyExperiences)
      {
        KeyValuePair<int, int> gearProficiencyData = proficiencyExperience;
        PlayerUnitGearProficiency unitGearProficiency = new PlayerUnitGearProficiency();
        UnitProficiencyLevel proficiencyLevel = ((IEnumerable<UnitProficiencyLevel>) MasterData.UnitProficiencyLevelList).FirstOrDefault<UnitProficiencyLevel>((Func<UnitProficiencyLevel, bool>) (x => x.from_exp <= gearProficiencyData.Value && x.to_exp >= gearProficiencyData.Value)) ?? ((IEnumerable<UnitProficiencyLevel>) MasterData.UnitProficiencyLevelList).OrderByDescending<UnitProficiencyLevel, int>((Func<UnitProficiencyLevel, int>) (x => x.level)).FirstOrDefault<UnitProficiencyLevel>();
        unitGearProficiency.total_exp = gearProficiencyData.Value;
        unitGearProficiency.gear_kind_id = gearProficiencyData.Key;
        unitGearProficiency.exp = unitGearProficiency.total_exp - proficiencyLevel.from_exp;
        unitGearProficiency.exp = proficiencyLevel.to_exp - unitGearProficiency.total_exp;
        unitGearProficiency.level = proficiencyLevel.level;
        unitGearProficiencyList.Add(unitGearProficiency);
      }
      this.mPlayerUnit.gear_proficiencies = unitGearProficiencyList.ToArray();
      this.mPlayerUnit.leader_skills = new PlayerUnitLeader_skills[0];
      this.mPlayerUnit.skills = ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit.ID == this.mUnitID && x.level <= this.unitLevel.level)).Select<UnitSkill, PlayerUnitSkills>((Func<UnitSkill, PlayerUnitSkills>) (x => new PlayerUnitSkills()
      {
        skill_id = x.skill.ID,
        level = 1
      })).ToArray<PlayerUnitSkills>();
      if (!isCopy)
        return this.mPlayerUnit;
      PlayerUnit playerUnit1 = CopyUtil.DeepCopy<PlayerUnit>(this.mPlayerUnit);
      playerUnit1.equip_gear_ids[0] = new int?();
      if (this.mEquipeGearID != 0)
        playerUnit1.equip_gear_ids[0] = new int?(this.mEquipeGearID);
      return playerUnit1;
    }

    public List<PlayerUnit> GetEvolutionPlayerUnits(int[] patternIDs)
    {
      List<PlayerUnit> evolutionPlayerUnits = new List<PlayerUnit>();
      foreach (int patternId in patternIDs)
      {
        UnitEvolutionPattern evolutionPattern = MasterData.UnitEvolutionPattern[patternId];
        if (evolutionPattern.unit.ID == this.mUnitID)
        {
          PlayerUnit playerUnit = new PlayerUnit();
          playerUnit.hp = new PlayerUnitHp();
          playerUnit.strength = new PlayerUnitStrength();
          playerUnit.intelligence = new PlayerUnitIntelligence();
          playerUnit.vitality = new PlayerUnitVitality();
          playerUnit.mind = new PlayerUnitMind();
          playerUnit.agility = new PlayerUnitAgility();
          playerUnit.dexterity = new PlayerUnitDexterity();
          playerUnit.lucky = new PlayerUnitLucky();
          playerUnit.equip_gear_ids = new int?[1];
          playerUnit._unit = evolutionPattern.target_unit.ID;
          playerUnit.id = this.mCharactorID;
          playerUnit.is_enemy = false;
          playerUnit.favorite = false;
          playerUnit._unit_type = 1;
          UnitUnit targetUnitUnit = playerUnit.unit;
          playerUnit.job_id = targetUnitUnit.job_UnitJob;
          int exp = 0;
          UnitLevel unitLevel = ((IEnumerable<UnitLevel>) MasterData.UnitLevelList).Where<UnitLevel>((Func<UnitLevel, bool>) (x => x.pattern_id == targetUnitUnit.parameter_data._level_pattern_id)).FirstOrDefault<UnitLevel>((Func<UnitLevel, bool>) (x => x.from_exp <= exp && x.to_exp >= exp));
          playerUnit.total_exp = exp;
          playerUnit.exp = exp - unitLevel.from_exp;
          playerUnit.exp_next = unitLevel.to_exp - exp;
          playerUnit.level = unitLevel.level;
          playerUnit.max_level = targetUnitUnit.parameter_data._initial_max_level;
          playerUnit.move = targetUnitUnit.job.movement;
          playerUnit.hp.initial = targetUnitUnit.hp_initial + targetUnitUnit.job.hp_initial;
          playerUnit.hp.level = this.mLevelupHp;
          playerUnit.strength.initial = targetUnitUnit.strength_initial + targetUnitUnit.job.strength_initial;
          playerUnit.strength.level = this.mLevelupStrength;
          playerUnit.intelligence.initial = targetUnitUnit.intelligence_initial + targetUnitUnit.job.intelligence_initial;
          playerUnit.intelligence.level = this.mLevelupIntelligence;
          playerUnit.vitality.initial = targetUnitUnit.vitality_initial + targetUnitUnit.job.vitality_initial;
          playerUnit.vitality.level = this.mLevelupVitality;
          playerUnit.mind.initial = targetUnitUnit.mind_initial + targetUnitUnit.job.mind_initial;
          playerUnit.mind.level = this.mLevelupMind;
          playerUnit.agility.initial = targetUnitUnit.agility_initial + targetUnitUnit.job.agility_initial;
          playerUnit.agility.level = this.mLevelupAgility;
          playerUnit.dexterity.initial = targetUnitUnit.dexterity_initial + targetUnitUnit.job.dexterity_initial;
          playerUnit.dexterity.level = this.mLevelupDexterity;
          playerUnit.lucky.initial = targetUnitUnit.lucky_initial + targetUnitUnit.job.lucky_initial;
          playerUnit.lucky.level = this.mLevelupLucky;
          playerUnit.equip_gear_ids[0] = new int?();
          if (this.mEquipeGearID != 0)
            playerUnit.equip_gear_ids[0] = new int?(this.mEquipeGearID);
          List<PlayerUnitGearProficiency> unitGearProficiencyList = new List<PlayerUnitGearProficiency>();
          foreach (KeyValuePair<int, int> proficiencyExperience in this.mGearProficiencyExperiences)
            unitGearProficiencyList.Add(new PlayerUnitGearProficiency()
            {
              total_exp = proficiencyExperience.Value,
              gear_kind_id = proficiencyExperience.Key,
              exp = 0,
              exp_next = 0,
              level = 1
            });
          playerUnit.gear_proficiencies = unitGearProficiencyList.ToArray();
          playerUnit.leader_skills = new PlayerUnitLeader_skills[0];
          playerUnit.skills = ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit.ID == targetUnitUnit.ID && x.level <= unitLevel.level)).Select<UnitSkill, PlayerUnitSkills>((Func<UnitSkill, PlayerUnitSkills>) (x => new PlayerUnitSkills()
          {
            skill_id = x.skill.ID,
            level = 1
          })).ToArray<PlayerUnitSkills>();
          evolutionPlayerUnits.Add(playerUnit);
        }
      }
      return evolutionPlayerUnits;
    }

    public string GetSeverString()
    {
      string str = string.Join(",", this.mGearProficiencyExperiences.Select<KeyValuePair<int, int>, string>((Func<KeyValuePair<int, int>, string>) (x => string.Format(EarthCharacter.serverGearProficiencyDataFormat, (object) x.Key, (object) x.Value))).ToArray<string>());
      return string.Format(EarthCharacter.serverDataFormat, (object) this.mIndex, (object) (this.mIsFall ? 1 : 0), (object) this.mCharactorID, (object) this.mUnitID, (object) this.mExperience, (object) this.mEquipeGearID, (object) this.mLevelupHp, (object) this.mLevelupStrength, (object) this.mLevelupIntelligence, (object) this.mLevelupVitality, (object) this.mLevelupMind, (object) this.mLevelupAgility, (object) this.mLevelupDexterity, (object) this.mLevelupLucky, (object) str, (object) this.mID, (object) this.mBattleIndex, (object) (this.mIsDesert ? 1 : 0), (object) this.mEventQuestBattleIndex);
    }

    public static EarthCharacter JsonLoad(Dictionary<string, object> json)
    {
      EarthCharacter earthCharacter = new EarthCharacter();
      earthCharacter.mID = (int) (long) json["id"];
      earthCharacter.mIndex = (int) (long) json["index"];
      earthCharacter.mBattleIndex = (int) (long) json["battle_index"];
      earthCharacter.mIsFall = (int) (long) json["is_fall"] != 0;
      earthCharacter.mCharactorID = (int) (long) json["character_id"];
      earthCharacter.mUnitID = (int) (long) json["unit_id"];
      earthCharacter.mExperience = (int) (long) json["experience"];
      earthCharacter.mEquipeGearID = (int) (long) json["equipe_gear_id"];
      earthCharacter.mLevelupHp = (int) (long) json["levelup_hp"];
      earthCharacter.mLevelupStrength = (int) (long) json["levelup_strength"];
      earthCharacter.mLevelupIntelligence = (int) (long) json["levelup_intelligence"];
      earthCharacter.mLevelupVitality = (int) (long) json["levelup_vitality"];
      earthCharacter.mLevelupMind = (int) (long) json["levelup_mind"];
      earthCharacter.mLevelupAgility = (int) (long) json["levelup_agility"];
      earthCharacter.mLevelupDexterity = (int) (long) json["levelup_dexteruty"];
      earthCharacter.mLevelupLucky = (int) (long) json["levelup_lucky"];
      earthCharacter.mGearProficiencyExperiences = new Dictionary<int, int>();
      foreach (Dictionary<string, object> dictionary in (List<object>) json["gear_proficiencies"])
        earthCharacter.mGearProficiencyExperiences.Add((int) (long) dictionary["gear_kind_id"], (int) (long) dictionary["proficiency"]);
      if (json.ContainsKey("is_desert"))
        earthCharacter.mIsDesert = (int) (long) json["is_desert"] != 0;
      if (json.ContainsKey("event_quest_battle_index"))
        earthCharacter.mEventQuestBattleIndex = (int) (long) json["event_quest_battle_index"];
      return earthCharacter;
    }
  }
}
