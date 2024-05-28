// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitUnit
  {
    public int ID;
    public string name;
    public string english_name;
    public int parameter_data_UnitUnitParameter;
    public int etc_data_UnitUnitDescription;
    public DateTime? published_at;
    public int same_character_id;
    public int character_UnitCharacter;
    public int resource_reference_unit_id_UnitUnit;
    public int model_reference_id;
    public int rarity_UnitRarity;
    public int cost;
    public int job_UnitJob;
    public int is_consume_only;
    public int is_evolution_only;
    public int skillup_type;
    public int is_breakthrough_only;
    public int is_buildup_only;
    public int kind_GearKind;
    public int history_group_number;
    public int _base_sell_price;
    public int initial_gear_GearGear;
    public string vehicle_model_name;
    public string equip_model_name;
    public string equip_model_b_name;
    public string field_normal_face_material_name;
    public string field_gray_body_material_name;
    public string field_gray_face_material_name;
    public string field_gray_vehicle_material_name;
    public string field_gray_equip_material_name;
    public string field_gray_equip_b_material_name;
    public float duel_model_scale;
    public float field_model_scale;
    public float duel_shadow_scale_x;
    public float duel_shadow_scale_z;
    public int footstep_type_UnitFootstepType;
    public int camera_pattern_UnitCameraPattern;
    public int illust_pattern_UnitIllustPattern;
    public int? cutin_pattern_id;
    public int unit_voice_pattern_id;
    public int non_disp_weapon;
    public int buildup_limit_release_id_UnitUnitBuildupLimitRelease;
    public bool rainbow_on;
    public bool trust_target_flag;
    public bool awake_unit_flag;
    public bool can_awake_unit_flag;
    public string formal_name;
    public int? country_attribute;
    public int? inclusion_ip;
    public bool magic_warrior_flag;
    public int? awake_special_skill_category_id;
    public int compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting;
    public bool is_unity_value_up;
    public bool job_characteristics_levelup_pattern;
    public bool exist_overkillers_slot;
    public bool exist_overkillers_skill;
    public int overkillers_parameter;
    public int? expire_date_UnitExpireDate;
    public bool is_exp_material;
    public bool upper_attribute_flag;
    [NonSerialized]
    private UnitFamily[] _families;
    [NonSerialized]
    private int[] _SkillGroupIds;
    private CommonElement? _element;
    private bool? isPossibleEquippedGear3_;
    private static readonly string PATH_CUTIN_PATTERN = "AssetBundle/Resources/Characters/{0}/battle_cutin_{1}";
    public static readonly string duelAnimatorRootPath = "Animators/duel/{0}";
    private static readonly string homeAnimatorRootPath = "Animators/home/anim_home_{0}";
    public static readonly string winAnimatorRootPath = "Animators/duel_win/{0}";
    private UnitVoicePattern _unitVoicePattern;
    private bool? _isEvolution;
    private bool? _isEvolutioned;
    private UnityValueUpPattern[] wUnityValueUpPatterns_;
    private Dictionary<UnitMaterialExclusionType, HashSet<int>> wUnitMaterialExclusions_;
    private UnityPureValueUpPattern[] wPureUnityValueUpPatterns_;
    private bool? isTrustMaterial_;

    public static UnitUnit Parse(MasterDataReader reader)
    {
      return new UnitUnit()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        english_name = reader.ReadString(true),
        parameter_data_UnitUnitParameter = reader.ReadInt(),
        etc_data_UnitUnitDescription = reader.ReadInt(),
        published_at = reader.ReadDateTimeOrNull(),
        same_character_id = reader.ReadInt(),
        character_UnitCharacter = reader.ReadInt(),
        resource_reference_unit_id_UnitUnit = reader.ReadInt(),
        model_reference_id = reader.ReadInt(),
        rarity_UnitRarity = reader.ReadInt(),
        cost = reader.ReadInt(),
        job_UnitJob = reader.ReadInt(),
        is_consume_only = reader.ReadInt(),
        is_evolution_only = reader.ReadInt(),
        skillup_type = reader.ReadInt(),
        is_breakthrough_only = reader.ReadInt(),
        is_buildup_only = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        history_group_number = reader.ReadInt(),
        _base_sell_price = reader.ReadInt(),
        initial_gear_GearGear = reader.ReadInt(),
        vehicle_model_name = reader.ReadStringOrNull(true),
        equip_model_name = reader.ReadStringOrNull(true),
        equip_model_b_name = reader.ReadStringOrNull(true),
        field_normal_face_material_name = reader.ReadString(true),
        field_gray_body_material_name = reader.ReadString(true),
        field_gray_face_material_name = reader.ReadString(true),
        field_gray_vehicle_material_name = reader.ReadString(true),
        field_gray_equip_material_name = reader.ReadString(true),
        field_gray_equip_b_material_name = reader.ReadString(true),
        duel_model_scale = reader.ReadFloat(),
        field_model_scale = reader.ReadFloat(),
        duel_shadow_scale_x = reader.ReadFloat(),
        duel_shadow_scale_z = reader.ReadFloat(),
        footstep_type_UnitFootstepType = reader.ReadInt(),
        camera_pattern_UnitCameraPattern = reader.ReadInt(),
        illust_pattern_UnitIllustPattern = reader.ReadInt(),
        cutin_pattern_id = reader.ReadIntOrNull(),
        unit_voice_pattern_id = reader.ReadInt(),
        non_disp_weapon = reader.ReadInt(),
        buildup_limit_release_id_UnitUnitBuildupLimitRelease = reader.ReadInt(),
        rainbow_on = reader.ReadBool(),
        trust_target_flag = reader.ReadBool(),
        awake_unit_flag = reader.ReadBool(),
        can_awake_unit_flag = reader.ReadBool(),
        formal_name = reader.ReadStringOrNull(true),
        country_attribute = reader.ReadIntOrNull(),
        inclusion_ip = reader.ReadIntOrNull(),
        magic_warrior_flag = reader.ReadBool(),
        awake_special_skill_category_id = reader.ReadIntOrNull(),
        compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting = reader.ReadInt(),
        is_unity_value_up = reader.ReadBool(),
        job_characteristics_levelup_pattern = reader.ReadBool(),
        exist_overkillers_slot = reader.ReadBool(),
        exist_overkillers_skill = reader.ReadBool(),
        overkillers_parameter = reader.ReadInt(),
        expire_date_UnitExpireDate = reader.ReadIntOrNull(),
        is_exp_material = reader.ReadBool(),
        upper_attribute_flag = reader.ReadBool()
      };
    }

    public UnitUnitParameter parameter_data
    {
      get
      {
        UnitUnitParameter parameterData;
        if (!MasterData.UnitUnitParameter.TryGetValue(this.parameter_data_UnitUnitParameter, out parameterData))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnitParameter[" + (object) this.parameter_data_UnitUnitParameter + "]"));
        return parameterData;
      }
    }

    public UnitUnitDescription etc_data
    {
      get
      {
        UnitUnitDescription etcData;
        if (!MasterData.UnitUnitDescription.TryGetValue(this.etc_data_UnitUnitDescription, out etcData))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnitDescription[" + (object) this.etc_data_UnitUnitDescription + "]"));
        return etcData;
      }
    }

    public UnitCharacter character
    {
      get
      {
        UnitCharacter character;
        if (!MasterData.UnitCharacter.TryGetValue(this.character_UnitCharacter, out character))
          Debug.LogError((object) ("Key not Found: MasterData.UnitCharacter[" + (object) this.character_UnitCharacter + "]"));
        return character;
      }
    }

    public UnitUnit resource_reference_unit_id
    {
      get
      {
        UnitUnit resourceReferenceUnitId;
        if (!MasterData.UnitUnit.TryGetValue(this.resource_reference_unit_id_UnitUnit, out resourceReferenceUnitId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.resource_reference_unit_id_UnitUnit + "]"));
        return resourceReferenceUnitId;
      }
    }

    public UnitRarity rarity
    {
      get
      {
        UnitRarity rarity;
        if (!MasterData.UnitRarity.TryGetValue(this.rarity_UnitRarity, out rarity))
          Debug.LogError((object) ("Key not Found: MasterData.UnitRarity[" + (object) this.rarity_UnitRarity + "]"));
        return rarity;
      }
    }

    public UnitJob job
    {
      get
      {
        UnitJob job;
        if (!MasterData.UnitJob.TryGetValue(this.job_UnitJob, out job))
          Debug.LogError((object) ("Key not Found: MasterData.UnitJob[" + (object) this.job_UnitJob + "]"));
        return job;
      }
    }

    public GearKind kind
    {
      get
      {
        GearKind kind;
        if (!MasterData.GearKind.TryGetValue(this.kind_GearKind, out kind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.kind_GearKind + "]"));
        return kind;
      }
    }

    public GearGear initial_gear
    {
      get
      {
        GearGear initialGear;
        if (!MasterData.GearGear.TryGetValue(this.initial_gear_GearGear, out initialGear))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.initial_gear_GearGear + "]"));
        return initialGear;
      }
    }

    public UnitFootstepType footstep_type
    {
      get
      {
        UnitFootstepType footstepType;
        if (!MasterData.UnitFootstepType.TryGetValue(this.footstep_type_UnitFootstepType, out footstepType))
          Debug.LogError((object) ("Key not Found: MasterData.UnitFootstepType[" + (object) this.footstep_type_UnitFootstepType + "]"));
        return footstepType;
      }
    }

    public UnitCameraPattern camera_pattern
    {
      get
      {
        UnitCameraPattern cameraPattern;
        if (!MasterData.UnitCameraPattern.TryGetValue(this.camera_pattern_UnitCameraPattern, out cameraPattern))
          Debug.LogError((object) ("Key not Found: MasterData.UnitCameraPattern[" + (object) this.camera_pattern_UnitCameraPattern + "]"));
        return cameraPattern;
      }
    }

    public UnitIllustPattern illust_pattern
    {
      get
      {
        UnitIllustPattern illustPattern;
        if (!MasterData.UnitIllustPattern.TryGetValue(this.illust_pattern_UnitIllustPattern, out illustPattern))
          Debug.LogError((object) ("Key not Found: MasterData.UnitIllustPattern[" + (object) this.illust_pattern_UnitIllustPattern + "]"));
        return illustPattern;
      }
    }

    public UnitUnitBuildupLimitRelease buildup_limit_release_id
    {
      get
      {
        UnitUnitBuildupLimitRelease buildupLimitReleaseId;
        if (!MasterData.UnitUnitBuildupLimitRelease.TryGetValue(this.buildup_limit_release_id_UnitUnitBuildupLimitRelease, out buildupLimitReleaseId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnitBuildupLimitRelease[" + (object) this.buildup_limit_release_id_UnitUnitBuildupLimitRelease + "]"));
        return buildupLimitReleaseId;
      }
    }

    public ComposeMaxUnityValueSetting compose_max_unity_value_setting_id
    {
      get
      {
        ComposeMaxUnityValueSetting unityValueSettingId;
        if (!MasterData.ComposeMaxUnityValueSetting.TryGetValue(this.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting, out unityValueSettingId))
          Debug.LogError((object) ("Key not Found: MasterData.ComposeMaxUnityValueSetting[" + (object) this.compose_max_unity_value_setting_id_ComposeMaxUnityValueSetting + "]"));
        return unityValueSettingId;
      }
    }

    public UnitExpireDate expire_date
    {
      get
      {
        if (!this.expire_date_UnitExpireDate.HasValue)
          return (UnitExpireDate) null;
        UnitExpireDate expireDate;
        if (!MasterData.UnitExpireDate.TryGetValue(this.expire_date_UnitExpireDate.Value, out expireDate))
          Debug.LogError((object) ("Key not Found: MasterData.UnitExpireDate[" + (object) this.expire_date_UnitExpireDate.Value + "]"));
        return expireDate;
      }
    }

    public bool IsNormalUnit
    {
      get
      {
        return this.is_consume_only == 0 && this.is_evolution_only == 0 && this.skillup_type == 0 && this.is_breakthrough_only == 0 && this.is_buildup_only == 0 && !this.is_unity_value_up && !this.is_exp_material;
      }
    }

    public bool IsMaterialUnit
    {
      get
      {
        return this.is_consume_only != 0 || this.is_evolution_only != 0 || this.skillup_type != 0 || this.is_breakthrough_only != 0 || this.is_buildup_only != 0 || this.is_unity_value_up || this.is_exp_material;
      }
    }

    public bool IsTougouUnit
    {
      get
      {
        return this.is_consume_only != 0 || this.skillup_type != 0 || this.is_breakthrough_only != 0 || this.is_buildup_only != 0 || this.is_unity_value_up;
      }
    }

    public bool IsSinkaUnit => this.is_evolution_only == 1;

    public bool IsTenseiUnit => this.is_evolution_only == 2;

    public bool IsBreakThrough => this.is_breakthrough_only == 1;

    public bool IsBuildup => this.is_buildup_only == 1;

    public bool IsPureValueUp
    {
      get
      {
        return this.is_consume_only == 1 && ((IEnumerable<UnityPureValueUpPattern>) MasterData.UnityPureValueUpPatternList).Any<UnityPureValueUpPattern>((Func<UnityPureValueUpPattern, bool>) (x => x.material_unit == this));
      }
    }

    public bool IsBreakThrougPureValueUp(PlayerUnit basePlayerUnit)
    {
      return this.IsPureValueUp && basePlayerUnit.breakthrough_count < basePlayerUnit.unit.breakthrough_limit;
    }

    public bool IsSkillLevelUpPureValueUp(PlayerUnit basePlayerUnit)
    {
      return this.IsPureValueUp && ((IEnumerable<PlayerUnitSkills>) basePlayerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.level < x.skill.upper_level)).ToArray<PlayerUnitSkills>().Length != 0;
    }

    public string SecondaryName => this.formal_name?.Replace(this.name, string.Empty).Trim('・');

    public bool CheckBreakThroughMaterial(PlayerUnit baseUnit)
    {
      List<UnitBreakThrough> list = ((IEnumerable<UnitBreakThrough>) MasterData.UnitBreakThroughList).Where<UnitBreakThrough>((Func<UnitBreakThrough, bool>) (x => x.material_unit == this)).ToList<UnitBreakThrough>();
      return list.Count != 0 && (this.CheckBreakThroughInTargetUnit(list, baseUnit) || this.CheckBreakThroughRarityAndGearAndSkill(list, baseUnit) || this.CheckBreakThroughAllFree(list));
    }

    public bool CheckBreakThroughInTargetUnit(List<UnitBreakThrough> list, PlayerUnit baseUnit)
    {
      return list.Any<UnitBreakThrough>((Func<UnitBreakThrough, bool>) (x => x.target_unit != null && x.target_unit == baseUnit.unit));
    }

    public bool CheckBreakThroughRarityAndGearAndSkill(
      List<UnitBreakThrough> list,
      PlayerUnit baseUnit)
    {
      return list.Where<UnitBreakThrough>((Func<UnitBreakThrough, bool>) (x =>
      {
        if (x.skill_id == null)
          return true;
        return x.skill_id != null && ((IEnumerable<PlayerUnitSkills>) baseUnit.skills).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (y => y.skill_id == x.skill_id.ID));
      })).Where<UnitBreakThrough>((Func<UnitBreakThrough, bool>) (x =>
      {
        if (x.rarity == null)
          return true;
        return x.rarity != null && x.rarity.ID == baseUnit.unit.rarity.ID;
      })).Any<UnitBreakThrough>((Func<UnitBreakThrough, bool>) (x =>
      {
        if (x.rarity != null && x.kind == null)
          return true;
        return x.kind != null && x.kind.Enum == baseUnit.unit.kind.Enum;
      }));
    }

    public bool CheckBreakThroughAllFree(List<UnitBreakThrough> list)
    {
      return list.Any<UnitBreakThrough>((Func<UnitBreakThrough, bool>) (x => x.target_unit == null && x.rarity == null && x.kind == null));
    }

    public bool IsMaterialUnitSkillUp => this.skillup_type != 0;

    public bool IsBuildUpMaterial(PlayerUnit baseUnit)
    {
      return !((IEnumerable<UnitBuildupMaterialPattern>) MasterData.UnitBuildupMaterialPatternList).Any<UnitBuildupMaterialPattern>((Func<UnitBuildupMaterialPattern, bool>) (x => x.material_unit == this)) ? this.IsBuildup : this.BuildupMaterialUnit(baseUnit) != null;
    }

    public UnitBuildupMaterialPattern BuildupMaterialUnit(PlayerUnit BaseUnit)
    {
      UnitUnit bUnit = BaseUnit.unit;
      UnitGroup baseUnitGroupIDs = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == bUnit.ID));
      return baseUnitGroupIDs == null ? (UnitBuildupMaterialPattern) null : ((IEnumerable<UnitBuildupMaterialPattern>) MasterData.UnitBuildupMaterialPatternList).Where<UnitBuildupMaterialPattern>((Func<UnitBuildupMaterialPattern, bool>) (x => x.material_unit == this)).Where<UnitBuildupMaterialPattern>((Func<UnitBuildupMaterialPattern, bool>) (x => !x.target_unit_UnitUnit.HasValue || x.target_unit_UnitUnit.Value == BaseUnit._unit)).Where<UnitBuildupMaterialPattern>((Func<UnitBuildupMaterialPattern, bool>) (x => x.group_large_category_id_UnitGroupLargeCategory == 1 || x.group_large_category_id.ID == baseUnitGroupIDs.group_large_category_id.ID)).Where<UnitBuildupMaterialPattern>((Func<UnitBuildupMaterialPattern, bool>) (x => x.group_small_category_id_UnitGroupSmallCategory == 1 || x.group_small_category_id.ID == baseUnitGroupIDs.group_small_category_id.ID)).Where<UnitBuildupMaterialPattern>((Func<UnitBuildupMaterialPattern, bool>) (x => x.group_clothing_category_id_UnitGroupClothingCategory == 1 || x.group_clothing_category_id.ID == baseUnitGroupIDs.group_clothing_category_id.ID || x.group_clothing_category_id.ID == baseUnitGroupIDs.group_clothing_category_id_2.ID)).FirstOrDefault<UnitBuildupMaterialPattern>() ?? (UnitBuildupMaterialPattern) null;
    }

    public UnitTransmigrationPattern TransmigratePattern
    {
      get
      {
        return ((IEnumerable<UnitTransmigrationPattern>) MasterData.UnitTransmigrationPatternList).Where<UnitTransmigrationPattern>((Func<UnitTransmigrationPattern, bool>) (p => p.rarity_name.index == this.rarity.index)).First<UnitTransmigrationPattern>();
      }
    }

    public UnitUnit[] TransmigrateUnits
    {
      get
      {
        UnitTransmigrationPattern pattern = this.TransmigratePattern;
        return ((IEnumerable<UnitTransmigrationMaterial>) MasterData.UnitTransmigrationMaterialList).Where<UnitTransmigrationMaterial>((Func<UnitTransmigrationMaterial, bool>) (u => u.pattern_UnitTransmigrationPattern == pattern.ID)).Select<UnitTransmigrationMaterial, UnitUnit>((Func<UnitTransmigrationMaterial, UnitUnit>) (u => u.material)).ToArray<UnitUnit>();
      }
    }

    public UnitEvolutionPattern[] EvolutionPattern
    {
      get
      {
        return ((IEnumerable<UnitEvolutionPattern>) MasterData.UnitEvolutionPatternList).Where<UnitEvolutionPattern>((Func<UnitEvolutionPattern, bool>) (p => p.unit_UnitUnit == this.ID)).ToArray<UnitEvolutionPattern>();
      }
    }

    public int FinalEvolutionCost
    {
      get
      {
        IEnumerable<UnitUnit> source = ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.same_character_id == this.same_character_id)).Where<UnitUnit>((Func<UnitUnit, bool>) (x => !x.awake_unit_flag));
        source.ToArray<UnitUnit>();
        return source.Max<UnitUnit>((Func<UnitUnit, int>) (x => x.cost));
      }
    }

    public Dictionary<int, UnitUnit[]> EvolutionUnits
    {
      get
      {
        Dictionary<int, UnitUnit[]> evolutionUnits = new Dictionary<int, UnitUnit[]>();
        foreach (UnitEvolutionPattern evolutionPattern in this.EvolutionPattern)
        {
          UnitEvolutionPattern pattern = evolutionPattern;
          UnitUnit[] array = ((IEnumerable<UnitEvolutionUnit>) MasterData.UnitEvolutionUnitList).Where<UnitEvolutionUnit>((Func<UnitEvolutionUnit, bool>) (u => u.evolution_pattern.ID == pattern.ID)).Select<UnitEvolutionUnit, UnitUnit>((Func<UnitEvolutionUnit, UnitUnit>) (u => u.unit)).ToArray<UnitUnit>();
          evolutionUnits.Add(pattern.ID, array);
        }
        return evolutionUnits;
      }
    }

    public UnitFamily[] Families
    {
      get
      {
        if (this._families == null)
          this._families = ((IEnumerable<UnitUnitFamily>) MasterData.WhereUnitUnitFamilyBy(this)).Select<UnitUnitFamily, UnitFamily>((Func<UnitUnitFamily, UnitFamily>) (x => x.element)).ToArray<UnitFamily>();
        return this._families;
      }
    }

    public UnitFamily[] FamiliesWithJob(int job_id)
    {
      IEnumerable<UnitFamily> source = ((IEnumerable<UnitUnitFamily>) MasterData.WhereUnitUnitFamilyBy(this)).Select<UnitUnitFamily, UnitFamily>((Func<UnitUnitFamily, UnitFamily>) (x => x.element));
      UnitJob unitJob = (UnitJob) null;
      if (!MasterData.UnitJob.TryGetValue(job_id, out unitJob))
        return source.ToArray<UnitFamily>();
      IEnumerable<UnitFamily> unitFamilies = ((IEnumerable<UnitJobFamily>) MasterData.WhereUnitJobFamilyBy(MasterData.UnitJob[job_id])).Select<UnitJobFamily, UnitFamily>((Func<UnitJobFamily, UnitFamily>) (x => x.element));
      if (!unitFamilies.Any<UnitFamily>())
        return source.ToArray<UnitFamily>();
      IEnumerable<UnitFamily> second = source.Where<UnitFamily>((Func<UnitFamily, bool>) (x => !MasterData.UnitFamilyValue[(int) x].is_disp));
      return unitFamilies.Concat<UnitFamily>(second).ToArray<UnitFamily>();
    }

    public bool HasFamily(UnitFamily family)
    {
      return ((IEnumerable<UnitFamily>) this.Families).Any<UnitFamily>((Func<UnitFamily, bool>) (x => x == family));
    }

    public int[] SkillGroupIds
    {
      get
      {
        if (this._SkillGroupIds == null)
          this._SkillGroupIds = ((IEnumerable<UnitSkillGroup>) MasterData.WhereUnitSkillGroupBy(this)).Select<UnitSkillGroup, int>((Func<UnitSkillGroup, int>) (x => x.skill_groupID)).ToArray<int>();
        return this._SkillGroupIds;
      }
    }

    public bool HasSkillGroupId(int skillGroupId)
    {
      return ((IEnumerable<int>) this.SkillGroupIds).Contains<int>(skillGroupId);
    }

    public BattleskillSkill RememberLeaderSkill
    {
      get
      {
        return ((IEnumerable<UnitLeaderSkill>) MasterData.UnitLeaderSkillList).FirstOrDefault<UnitLeaderSkill>((Func<UnitLeaderSkill, bool>) (x => x.unit_UnitUnit == this.ID))?.skill;
      }
    }

    public BattleskillSkill[] RememberSkills(int unit_type_id = 0)
    {
      return ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Where<UnitSkill>((Func<UnitSkill, bool>) (x =>
      {
        if (x.unit_UnitUnit != this.ID)
          return false;
        return x.unit_type == 0 || x.unit_type == unit_type_id;
      })).Select<UnitSkill, BattleskillSkill>((Func<UnitSkill, BattleskillSkill>) (x => x.skill)).ToArray<BattleskillSkill>();
    }

    public UnitSkill[] RememberUnitSkills(int unit_type_id = 0)
    {
      return ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Where<UnitSkill>((Func<UnitSkill, bool>) (x =>
      {
        if (x.unit_UnitUnit != this.ID)
          return false;
        return x.unit_type == 0 || x.unit_type == unit_type_id;
      })).ToArray<UnitSkill>();
    }

    public UnitSkill[] RememberUnitAllSkills()
    {
      return ((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_UnitUnit == this.ID)).ToArray<UnitSkill>();
    }

    public CommonElement GetElement()
    {
      if (!this._element.HasValue)
      {
        this._element = new CommonElement?(CommonElement.none);
        if (!this.IsMaterialUnit)
        {
          for (int index = 0; index < MasterData.UnitSkillList.Length; ++index)
          {
            if (MasterData.UnitSkillList[index].unit_UnitUnit == this.ID && BattleskillSkill.InvestElementSkillIds.Contains(MasterData.UnitSkillList[index].skill_BattleskillSkill))
            {
              this._element = new CommonElement?(MasterData.UnitSkillList[index].skill.element);
              break;
            }
          }
        }
      }
      return this._element.Value;
    }

    public int GetElementSkillID()
    {
      if (this.GetElement() != CommonElement.none && !this.IsMaterialUnit)
      {
        for (int index = 0; index < MasterData.UnitSkillList.Length; ++index)
        {
          if (MasterData.UnitSkillList[index].unit_UnitUnit == this.ID && BattleskillSkill.InvestElementSkillIds.Contains(MasterData.UnitSkillList[index].skill_BattleskillSkill))
            return MasterData.UnitSkillList[index].skill.ID;
        }
      }
      return 0;
    }

    public string cv_name => this.etc_data.cv_name;

    public string description => this.etc_data.description;

    public string illustrator_name => this.etc_data.illustrator_name;

    public int breakthrough_limit => this.parameter_data.breakthrough_limit;

    public int _level_per_breakthrough => this.parameter_data._level_per_breakthrough;

    public int hp_max => this.parameter_data.hp_max;

    public int strength_max => this.parameter_data.strength_max;

    public int vitality_max => this.parameter_data.vitality_max;

    public int intelligence_max => this.parameter_data.intelligence_max;

    public int mind_max => this.parameter_data.mind_max;

    public int agility_max => this.parameter_data.agility_max;

    public int dexterity_max => this.parameter_data.dexterity_max;

    public int lucky_max => this.parameter_data.lucky_max;

    public int hp_initial => this.parameter_data.hp_initial;

    public int strength_initial => this.parameter_data.strength_initial;

    public int vitality_initial => this.parameter_data.vitality_initial;

    public int intelligence_initial => this.parameter_data.intelligence_initial;

    public int mind_initial => this.parameter_data.mind_initial;

    public int agility_initial => this.parameter_data.agility_initial;

    public int dexterity_initial => this.parameter_data.dexterity_initial;

    public int lucky_initial => this.parameter_data.lucky_initial;

    public int hp_compose => this.parameter_data.hp_compose;

    public int strength_compose => this.parameter_data.strength_compose;

    public int vitality_compose => this.parameter_data.vitality_compose;

    public int intelligence_compose => this.parameter_data.intelligence_compose;

    public int mind_compose => this.parameter_data.mind_compose;

    public int agility_compose => this.parameter_data.agility_compose;

    public int dexterity_compose => this.parameter_data.dexterity_compose;

    public int lucky_compose => this.parameter_data.lucky_compose;

    public int hp_buildup => this.parameter_data.hp_buildup;

    public int strength_buildup => this.parameter_data.strength_buildup;

    public int vitality_buildup => this.parameter_data.vitality_buildup;

    public int intelligence_buildup => this.parameter_data.intelligence_buildup;

    public int mind_buildup => this.parameter_data.mind_buildup;

    public int agility_buildup => this.parameter_data.agility_buildup;

    public int dexterity_buildup => this.parameter_data.dexterity_buildup;

    public int lucky_buildup => this.parameter_data.lucky_buildup;

    public int buildup_limit => this.parameter_data.buildup_limit;

    public UnitProficiency default_weapon_proficiency
    {
      get => this.parameter_data.default_weapon_proficiency;
    }

    public UnitProficiency default_shield_proficiency
    {
      get => this.parameter_data.default_shield_proficiency;
    }

    public UnitUnitGearModelKind getUnitGearModelKind(GearModelKind gearModelKind)
    {
      return Array.Find<UnitUnitGearModelKind>(MasterData.UnitUnitGearModelKindList, (Predicate<UnitUnitGearModelKind>) (x => x.unit_UnitUnit == this.ID && !x.job_metamor_id.HasValue && x.gear_model_kind_GearModelKind == gearModelKind.ID));
    }

    public UnitUnitGearModelKind getUnitGearModelKind(GearModelKind gearModelKind, int ext_id)
    {
      return Array.Find<UnitUnitGearModelKind>(MasterData.UnitUnitGearModelKindList, (Predicate<UnitUnitGearModelKind>) (x =>
      {
        if (x.unit_UnitUnit == this.ID)
        {
          int? jobMetamorId = x.job_metamor_id;
          int num = ext_id;
          if (jobMetamorId.GetValueOrDefault() == num & jobMetamorId.HasValue)
            return x.gear_model_kind_GearModelKind == gearModelKind.ID;
        }
        return false;
      }));
    }

    public GearGear GetInitialGear(int job_id)
    {
      if (job_id == 0 || this.job_UnitJob == job_id)
        return this.initial_gear;
      UnitModel unitModel = Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.initial_gear != null && x.unit_id_UnitUnit == this.ID && x.job_metamor_id == job_id));
      return unitModel != null ? unitModel.initial_gear : this.initial_gear;
    }

    public bool isLeftHandInitialWeapon(int job_id)
    {
      GearGear initialGear = this.GetInitialGear(job_id);
      UnitUnitGearModelKind unitGearModelKind = (UnitUnitGearModelKind) null;
      if (job_id != this.job_UnitJob)
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind, job_id);
      if (unitGearModelKind == null)
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind);
      return unitGearModelKind.is_left_hand_weapon == 1;
    }

    public bool isDualWieldInitialWeapon(int job_id)
    {
      UnitUnitGearModelKind unitGearModelKind = (UnitUnitGearModelKind) null;
      GearGear initialGear = this.GetInitialGear(job_id);
      if (job_id != this.job_UnitJob)
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind, job_id);
      if (unitGearModelKind == null)
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind);
      return unitGearModelKind != null && unitGearModelKind.is_left_hand_weapon == 2;
    }

    public BattleskillSkill PickupSkill
    {
      get
      {
        return ((IEnumerable<UnitPickupSkill>) MasterData.UnitPickupSkillList).FirstOrDefault<UnitPickupSkill>((Func<UnitPickupSkill, bool>) (x => x.unit.ID == this.ID))?.skill;
      }
    }

    public bool isComposeParameter
    {
      get
      {
        bool composeParameter = true;
        if (this.hp_compose == 0 && this.strength_compose == 0 && this.vitality_compose == 0 && this.intelligence_compose == 0 && this.mind_compose == 0 && this.agility_compose == 0 && this.dexterity_compose == 0)
          composeParameter = false;
        return composeParameter;
      }
    }

    public BattleUnitLandformFootstep GetFootstep(BattleLandform landform)
    {
      return ((IEnumerable<BattleUnitLandformFootstep>) MasterData.BattleUnitLandformFootstepList).Single<BattleUnitLandformFootstep>((Func<BattleUnitLandformFootstep, bool>) (x => x.unit_footstep_type.ID == this.footstep_type.ID && x.landform_footstep_type.ID == landform.footstep_type.ID));
    }

    public bool IsSea
    {
      get
      {
        UnitGroup groupInfo = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == this.ID));
        if (groupInfo != null)
        {
          UnitGroupLargeCategory groupLargeCategory = ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).FirstOrDefault<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x => x.ID == groupInfo.group_large_category_id_UnitGroupLargeCategory));
          if (groupLargeCategory != null && groupLargeCategory.ID == 4)
            return true;
        }
        return false;
      }
    }

    public MapFacility facility
    {
      get
      {
        return ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).Where<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.unit_UnitUnit == this.ID)).FirstOrDefault<FacilityLevel>()?.facility;
      }
    }

    public bool IsResonanceUnit
    {
      get
      {
        UnitGroup groupInfo = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == this.ID));
        if (groupInfo != null)
        {
          UnitGroupLargeCategory groupLargeCategory = ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).FirstOrDefault<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x => x.ID == groupInfo.group_large_category_id_UnitGroupLargeCategory));
          if (groupLargeCategory != null && (groupLargeCategory.ID == 5 || groupLargeCategory.ID == 7 || groupLargeCategory.ID == 8 || groupLargeCategory.ID == 2))
            return true;
        }
        return false;
      }
    }

    public int LargeCategoryId
    {
      get
      {
        UnitGroup unitGroup = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == this.ID));
        return unitGroup != null ? unitGroup.group_large_category_id_UnitGroupLargeCategory : 1;
      }
    }

    public int SmallCategoryId
    {
      get
      {
        UnitGroup unitGroup = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == this.ID));
        return unitGroup != null ? unitGroup.group_small_category_id_UnitGroupSmallCategory : 1;
      }
    }

    public int facilityLevel
    {
      get
      {
        FacilityLevel facilityLevel = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).Where<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.unit_UnitUnit == this.ID)).FirstOrDefault<FacilityLevel>();
        return facilityLevel == null ? 0 : facilityLevel.level;
      }
    }

    public PlayerUnitSkills[] facilitySkills
    {
      get
      {
        PlayerUnitSkills[] facilitySkills = new PlayerUnitSkills[0];
        FacilityLevel levelInfo = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).Where<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.unit_UnitUnit == this.ID)).FirstOrDefault<FacilityLevel>();
        if (levelInfo != null)
          facilitySkills = ((IEnumerable<FacilitySkillGroup>) MasterData.FacilitySkillGroupList).Where<FacilitySkillGroup>((Func<FacilitySkillGroup, bool>) (x => x.facility_level_info_FacilityLevel == levelInfo.ID)).Select<FacilitySkillGroup, PlayerUnitSkills>((Func<FacilitySkillGroup, PlayerUnitSkills>) (x => new PlayerUnitSkills()
          {
            skill_id = x.skill_BattleskillSkill,
            level = 1
          })).ToArray<PlayerUnitSkills>();
        return facilitySkills;
      }
    }

    public bool canUseAllGearHackSkill
    {
      get
      {
        return this.awake_special_skill_category_id.HasValue && this.awake_special_skill_category_id.Value == 4;
      }
    }

    public bool CanEquipAwakeSkillByType(AwakeSkillCategory.Type type)
    {
      switch (type)
      {
        case AwakeSkillCategory.Type.Normal:
          return false;
        case AwakeSkillCategory.Type.Dress:
          return true;
        case AwakeSkillCategory.Type.Trust:
          return this.LargeCategoryId == 4;
        case AwakeSkillCategory.Type.SecondIllusion:
          return this.LargeCategoryId == 5 || this.LargeCategoryId == 7 || this.LargeCategoryId == 8 || this.canUseAllGearHackSkill;
        case AwakeSkillCategory.Type.SecondDevil:
          return this.SmallCategoryId == 11 || this.canUseAllGearHackSkill;
        case AwakeSkillCategory.Type.SecondAngel:
          return this.SmallCategoryId == 10 || this.canUseAllGearHackSkill;
        case AwakeSkillCategory.Type.SecondBeast:
          return this.SmallCategoryId == 12 || this.canUseAllGearHackSkill;
        case AwakeSkillCategory.Type.SecondFairy:
          return this.SmallCategoryId == 13 || this.canUseAllGearHackSkill;
        case AwakeSkillCategory.Type.SecondCommand:
          return this.SmallCategoryId == 16 || this.canUseAllGearHackSkill;
        case AwakeSkillCategory.Type.ThirdIntegral:
          return this.SmallCategoryId == 17 || this.canUseAllGearHackSkill;
        case AwakeSkillCategory.Type.School:
          return this.LargeCategoryId == 2;
        case AwakeSkillCategory.Type.ThirdImitate:
          return this.SmallCategoryId == 18 || this.canUseAllGearHackSkill;
        case AwakeSkillCategory.Type.FourthRagnarok:
          return this.SmallCategoryId == 20 || this.canUseAllGearHackSkill;
        default:
          return false;
      }
    }

    public bool CanEquipAwakeSkill(PlayerAwakeSkill skill)
    {
      return this.CanEquipAwakeSkillByType((AwakeSkillCategory.Type) skill.masterData.awake_skill_category_id);
    }

    public PlayerAwakeSkill[] GetAllEquipableAwakeSkills(PlayerAwakeSkill[] skills = null)
    {
      return ((IEnumerable<PlayerAwakeSkill>) (skills ?? SMManager.Get<PlayerAwakeSkill[]>())).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => this.CanEquipAwakeSkill(x))).ToArray<PlayerAwakeSkill>();
    }

    public UnitAffiliationIcon getAffiliationIcon()
    {
      int num1 = 1;
      int num2 = 1;
      int num3 = 1;
      int num4 = 1;
      UnitGroup unitGroup = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == this.ID));
      if (unitGroup != null)
      {
        num1 = unitGroup.group_large_category_id_UnitGroupLargeCategory;
        num2 = unitGroup.group_small_category_id_UnitGroupSmallCategory;
        num3 = unitGroup.group_clothing_category_id_UnitGroupClothingCategory;
        num4 = unitGroup.group_generation_category_id_UnitGroupGenerationCategory;
      }
      UnitAffiliationIcon affiliationIcon = (UnitAffiliationIcon) null;
      foreach (UnitAffiliationIcon unitAffiliationIcon in MasterData.UnitAffiliationIconList)
      {
        if (affiliationIcon == null)
          affiliationIcon = unitAffiliationIcon;
        else if (unitAffiliationIcon.priority < affiliationIcon.priority)
        {
          switch (unitAffiliationIcon.unit_group_head)
          {
            case UnitGroupHead.group_all:
              affiliationIcon = unitAffiliationIcon;
              continue;
            case UnitGroupHead.group_large:
              if (unitAffiliationIcon.group_refer_id == num1)
                goto case UnitGroupHead.group_all;
              else
                continue;
            case UnitGroupHead.group_small:
              if (unitAffiliationIcon.group_refer_id == num2)
                goto case UnitGroupHead.group_all;
              else
                continue;
            case UnitGroupHead.group_clothing:
              if (unitAffiliationIcon.group_refer_id == num3)
                goto case UnitGroupHead.group_all;
              else
                continue;
            case UnitGroupHead.group_generation:
              if (unitAffiliationIcon.group_refer_id != num4)
                continue;
              goto case UnitGroupHead.group_all;
            default:
              Debug.LogError((object) "unknown unit_group_head");
              goto case UnitGroupHead.group_all;
          }
        }
      }
      return affiliationIcon;
    }

    public bool isPossibleOverkillers()
    {
      if (this.overkillers_parameter != 0)
        return true;
      foreach (OverkillersGroup overkillersGroup in MasterData.OverkillersGroupList)
      {
        if (overkillersGroup.child_unit_id == this.ID)
          return true;
      }
      return false;
    }

    public UnitTypeEnum[] validUnitTypes
    {
      get
      {
        HashSet<UnitTypeEnum> exclude = new HashSet<UnitTypeEnum>()
        {
          UnitTypeEnum.random
        };
        string[] source = Consts.GetInstance().ALL_UNIT_TYPE_ENTRY_UNIT_IDS.Split(',');
        if ((this.magic_warrior_flag ? 1 : (((IEnumerable<string>) source).Contains<string>(this.ID.ToString()) ? 1 : 0)) == 0)
        {
          switch (this.kind.Enum)
          {
            case GearKindEnum.gun:
            case GearKindEnum.staff:
              exclude.Add(UnitTypeEnum.kouki);
              break;
            case GearKindEnum.unique_wepon:
              UnitTypeSettings unitTypeSettings = Array.Find<UnitTypeSettings>(MasterData.UnitTypeSettingsList, (Predicate<UnitTypeSettings>) (x =>
              {
                if (!x.unit_UnitUnit.HasValue)
                  return false;
                int id = this.ID;
                int? unitUnitUnit = x.unit_UnitUnit;
                int valueOrDefault = unitUnitUnit.GetValueOrDefault();
                return id == valueOrDefault & unitUnitUnit.HasValue;
              }));
              if (unitTypeSettings != null && unitTypeSettings.attack_type == GearAttackType.magic)
              {
                exclude.Add(UnitTypeEnum.kouki);
                break;
              }
              exclude.Add(UnitTypeEnum.maki);
              break;
            default:
              exclude.Add(UnitTypeEnum.maki);
              break;
          }
        }
        return ((IEnumerable<UnitType>) MasterData.UnitTypeList).Where<UnitType>((Func<UnitType, bool>) (x => !exclude.Contains(x.Enum))).Select<UnitType, UnitTypeEnum>((Func<UnitType, UnitTypeEnum>) (y => y.Enum)).ToArray<UnitTypeEnum>();
      }
    }

    public GearKindEnum primaryGearKind
    {
      get
      {
        return this.kind_GearKind == 8 ? (GearKindEnum) this.initial_gear.kind_GearKind : (GearKindEnum) this.kind_GearKind;
      }
    }

    public bool hasSEASkills
    {
      get
      {
        UnitSEASkill unitSeaSkill;
        return this.exist_overkillers_slot && MasterData.UnitSEASkill.TryGetValue(this.same_character_id, out unitSeaSkill) && unitSeaSkill.hasSkills;
      }
    }

    public UnitSEASkill SEASkill
    {
      get
      {
        UnitSEASkill seaSkill;
        if (this.exist_overkillers_slot)
          MasterData.UnitSEASkill.TryGetValue(this.same_character_id, out seaSkill);
        else
          seaSkill = (UnitSEASkill) null;
        return seaSkill;
      }
    }

    public bool isPossibleEquippedGear3
    {
      get
      {
        if (this.isPossibleEquippedGear3_.HasValue)
          return this.isPossibleEquippedGear3_.Value;
        if (!PerformanceConfig.GetInstance().IsGear3)
        {
          this.isPossibleEquippedGear3_ = new bool?(false);
          return false;
        }
        this.isPossibleEquippedGear3_ = new bool?(this.IsNormalUnit && this.rarity.index >= 5 && !MasterData.GearExtensionExclusion.ContainsKey(this.same_character_id));
        return this.isPossibleEquippedGear3_.Value;
      }
    }

    public UnitBattleSkillOrigin[] MakeLeaderSkillOrigins()
    {
      UnitLeaderSkill origin = ((IEnumerable<UnitLeaderSkill>) MasterData.UnitLeaderSkillList).FirstOrDefault<UnitLeaderSkill>((Func<UnitLeaderSkill, bool>) (x => x.unit_UnitUnit == this.ID));
      List<UnitBattleSkillOrigin> battleSkillOriginList;
      if (origin == null)
      {
        battleSkillOriginList = new List<UnitBattleSkillOrigin>();
      }
      else
      {
        battleSkillOriginList = new List<UnitBattleSkillOrigin>();
        battleSkillOriginList.Add(new UnitBattleSkillOrigin((object) origin, origin.skill));
      }
      List<UnitBattleSkillOrigin> source = battleSkillOriginList;
      foreach (UnitSkillCharacterQuest skillCharacterQuest in ((IEnumerable<UnitSkillCharacterQuest>) MasterData.UnitSkillCharacterQuestList).Where<UnitSkillCharacterQuest>((Func<UnitSkillCharacterQuest, bool>) (x => x.unit_UnitUnit == this.ID && x.skill.skill_type == BattleskillSkillType.leader)).ToArray<UnitSkillCharacterQuest>())
      {
        UnitSkillCharacterQuest s = skillCharacterQuest;
        BattleskillSkill es = (BattleskillSkill) null;
        bool flag = MasterData.BattleskillSkill.TryGetValue(s.skill_after_evolution, out es);
        if (source.FirstOrDefault<UnitBattleSkillOrigin>((Func<UnitBattleSkillOrigin, bool>) (l => l.skill_ != null && l.skill_.ID == s.skill_BattleskillSkill)) != null & flag && !source.Any<UnitBattleSkillOrigin>((Func<UnitBattleSkillOrigin, bool>) (l => l.skill_.ID == es.ID)))
          source.Add(new UnitBattleSkillOrigin((object) s, es));
      }
      return source.ToArray();
    }

    public UnitBattleSkillOrigin[][] MakeSkillOrigins(bool includeMagic = true)
    {
      List<List<UnitBattleSkillOrigin>> list1 = ((IEnumerable<UnitSkill>) this.RememberUnitAllSkills()).Where<UnitSkill>((Func<UnitSkill, bool>) (x =>
      {
        if (x.unit_UnitUnit != this.ID)
          return false;
        return includeMagic || x.skill.skill_type != BattleskillSkillType.magic;
      })).Select<UnitSkill, List<UnitBattleSkillOrigin>>((Func<UnitSkill, List<UnitBattleSkillOrigin>>) (s => new List<UnitBattleSkillOrigin>()
      {
        new UnitBattleSkillOrigin((object) s, s.skill)
      })).ToList<List<UnitBattleSkillOrigin>>();
      foreach (UnitSkillCharacterQuest skillCharacterQuest in ((IEnumerable<UnitSkillCharacterQuest>) MasterData.UnitSkillCharacterQuestList).Where<UnitSkillCharacterQuest>((Func<UnitSkillCharacterQuest, bool>) (x => x.unit_UnitUnit == this.ID && (includeMagic || x.skill.skill_type != BattleskillSkillType.magic) && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillCharacterQuest>())
      {
        UnitSkillCharacterQuest s = skillCharacterQuest;
        BattleskillSkill skill1 = (BattleskillSkill) null;
        bool flag = MasterData.BattleskillSkill.TryGetValue(s.skill_after_evolution, out skill1);
        List<UnitBattleSkillOrigin> battleSkillOriginList = list1.FirstOrDefault<List<UnitBattleSkillOrigin>>((Func<List<UnitBattleSkillOrigin>, bool>) (tl => tl.Any<UnitBattleSkillOrigin>((Func<UnitBattleSkillOrigin, bool>) (l => l.skill_ != null && l.skill_.ID == s.skill_BattleskillSkill))));
        if (battleSkillOriginList == null)
        {
          BattleskillSkill skill2 = (BattleskillSkill) null;
          if (MasterData.BattleskillSkill.TryGetValue(s.skill_BattleskillSkill, out skill2))
          {
            battleSkillOriginList = new List<UnitBattleSkillOrigin>()
            {
              new UnitBattleSkillOrigin((object) s, skill2)
            };
            list1.Add(battleSkillOriginList);
          }
          else
            continue;
        }
        if (flag)
          battleSkillOriginList.Add(new UnitBattleSkillOrigin((object) s, skill1));
      }
      int cId = this.character.ID;
      UnitSkillHarmonyQuest[] array = ((IEnumerable<UnitSkillHarmonyQuest>) MasterData.UnitSkillHarmonyQuestList).Where<UnitSkillHarmonyQuest>((Func<UnitSkillHarmonyQuest, bool>) (x => x.character.ID == cId && (includeMagic || x.skill.skill_type != BattleskillSkillType.magic) && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillHarmonyQuest>();
      if (array.Length != 0)
        list1.AddRange((IEnumerable<List<UnitBattleSkillOrigin>>) ((IEnumerable<UnitSkillHarmonyQuest>) array).Select<UnitSkillHarmonyQuest, List<UnitBattleSkillOrigin>>((Func<UnitSkillHarmonyQuest, List<UnitBattleSkillOrigin>>) (hs => new List<UnitBattleSkillOrigin>()
        {
          new UnitBattleSkillOrigin((object) hs, MasterData.BattleskillSkill[hs.skill_BattleskillSkill])
        })).ToList<List<UnitBattleSkillOrigin>>());
      foreach (KeyValuePair<int, UnitSkillEvolution> keyValuePair in ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Where<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit_UnitUnit == this.ID)).ToDictionary<UnitSkillEvolution, int>((Func<UnitSkillEvolution, int>) (d => d.before_skill_BattleskillSkill)))
      {
        KeyValuePair<int, UnitSkillEvolution> item = keyValuePair;
        foreach (List<UnitBattleSkillOrigin> battleSkillOriginList in list1)
        {
          int index = battleSkillOriginList.FindIndex((Predicate<UnitBattleSkillOrigin>) (s => s.skill_.ID == item.Key));
          if (index >= 0)
          {
            if (battleSkillOriginList.Count - 1 == index)
            {
              battleSkillOriginList.Add(new UnitBattleSkillOrigin((object) item.Value, item.Value.after_skill));
              break;
            }
            if (battleSkillOriginList[index + 1].skill_.ID == item.Value.after_skill.ID)
              break;
            break;
          }
        }
      }
      List<UnitSkillAwake> list2 = ((IEnumerable<UnitSkillAwake>) MasterData.UnitSkillAwakeList).Where<UnitSkillAwake>((Func<UnitSkillAwake, bool>) (x => x.character_id == this.same_character_id && MasterData.BattleskillSkill.ContainsKey(x.skill_BattleskillSkill))).ToList<UnitSkillAwake>();
      if (list2.Any<UnitSkillAwake>())
        list1.AddRange((IEnumerable<List<UnitBattleSkillOrigin>>) list2.Select<UnitSkillAwake, List<UnitBattleSkillOrigin>>((Func<UnitSkillAwake, List<UnitBattleSkillOrigin>>) (x => new List<UnitBattleSkillOrigin>()
        {
          new UnitBattleSkillOrigin((object) x, MasterData.BattleskillSkill[x.skill_BattleskillSkill])
        })).ToList<List<UnitBattleSkillOrigin>>());
      UnitSEASkill seaSkill = this.SEASkill;
      if (seaSkill != null && seaSkill.hasSkills)
        list1.Add(new List<UnitBattleSkillOrigin>()
        {
          new UnitBattleSkillOrigin((object) seaSkill, seaSkill.skill_1)
        });
      return list1.Select<List<UnitBattleSkillOrigin>, UnitBattleSkillOrigin[]>((Func<List<UnitBattleSkillOrigin>, UnitBattleSkillOrigin[]>) (lst => lst.ToArray())).ToArray<UnitBattleSkillOrigin[]>();
    }

    public int numOverkillersSlot
    {
      get
      {
        OverkillersSlotRelease overkillersSlotRelease;
        return this.exist_overkillers_slot && MasterData.OverkillersSlotRelease.TryGetValue(this.same_character_id, out overkillersSlotRelease) ? overkillersSlotRelease.num_slot : 0;
      }
    }

    public List<string> GetUIResourcePaths(bool imageLarge = true, bool imageFull = false)
    {
      string str = this.directory2D();
      List<string> uiResourcePaths = new List<string>()
      {
        str + "c_thum"
      };
      if (imageLarge)
        uiResourcePaths.Add(str + "unit_large");
      if (imageFull)
        uiResourcePaths.Add(str + "unit_full");
      return uiResourcePaths;
    }

    public Future<Sprite> LoadCutin(int metamor_id = 0)
    {
      if (metamor_id != 0)
      {
        UnitModel unitModel = Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.unit_id_UnitUnit == this.ID && x.job_metamor_id == metamor_id));
        if (unitModel != null)
        {
          UnitReferenceImage referenceImage;
          if ((referenceImage = unitModel.cutin_frame?.reference_image) != null)
            return Singleton<ResourceManager>.GetInstance().Load<Sprite>(referenceImage.filePath(this));
          if (unitModel.cutin_pattern.HasValue)
            return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format(UnitUnit.PATH_CUTIN_PATTERN, (object) this.character_UnitCharacter, (object) unitModel.cutin_pattern.Value));
        }
      }
      UnitReferenceImage referenceImage1;
      if ((referenceImage1 = UnitCutinInfo.find(this)?.reference_image) != null)
        return Singleton<ResourceManager>.GetInstance().Load<Sprite>(referenceImage1.filePath(this));
      return this.cutin_pattern_id.HasValue ? Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format(UnitUnit.PATH_CUTIN_PATTERN, (object) this.character_UnitCharacter, (object) this.cutin_pattern_id.Value)) : Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Characters/{0}/battle_cutin", (object) this.character_UnitCharacter));
    }

    public Future<Sprite> LoadSpriteMedium(int jobOrMetamorId, float pixelsPerUnit = 1f)
    {
      string path = this.combinePath2D(jobOrMetamorId, "c_400x400").Replace("AssetBundle/Resources/", "");
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(path, pixelsPerUnit);
    }

    public Future<Sprite> LoadSpriteMedium(float pixelsPerUnit = 1f)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("Units/{0}/2D/c_400x400", (object) this.resource_reference_unit_id.ID), pixelsPerUnit);
    }

    public Future<Sprite> LoadSpriteLarge(float pixelsPerUnit = 1f)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/unit_large", (object) this.resource_reference_unit_id.ID), pixelsPerUnit);
    }

    public Future<Sprite> LoadSpriteLarge(int jobOrMetamorId, float pixelsPerUnit = 1f)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(this.combinePath2D(jobOrMetamorId, "unit_large"), pixelsPerUnit);
    }

    public bool ExistSpriteStory()
    {
      return Singleton<ResourceManager>.GetInstance().Contains(string.Format("AssetBundle/Resources/Units/{0}/2D/unit_story", (object) this.resource_reference_unit_id.ID));
    }

    public bool ExistSpriteStory(int jobOrMetamorId)
    {
      return Singleton<ResourceManager>.GetInstance().Contains((this.job_UnitJob == jobOrMetamorId ? UnitUnit.sDirectory2D(this.resource_reference_unit_id_UnitUnit) : UnitUnit.sDirectory2D(this.ID, this.resource_reference_unit_id_UnitUnit, jobOrMetamorId)) + "unit_story");
    }

    public Future<Sprite> LoadSpriteStory()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/unit_story", (object) this.resource_reference_unit_id.ID));
    }

    public Future<Sprite> LoadSpriteStory(int jobOrMetamorId)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(this.combinePath2D(jobOrMetamorId, "unit_story"));
    }

    private static string GetSpriteFacePath(int id, string name)
    {
      return UnitUnit.sDirectory2D(id) + "Face/" + name;
    }

    private static string GetSpriteFacePath(int unitId, int id, int jobOrMetamorId, string name)
    {
      return UnitUnit.sDirectory2D(unitId, id, jobOrMetamorId) + "Face/" + name;
    }

    public bool HasSpriteFace(string name)
    {
      return Singleton<ResourceManager>.GetInstance().Contains(UnitUnit.GetSpriteFacePath(this.resource_reference_unit_id.ID, name));
    }

    public bool HasSpriteFace(int jobOrMetamorId, string name)
    {
      return Singleton<ResourceManager>.GetInstance().Contains(this.job_UnitJob == jobOrMetamorId ? UnitUnit.GetSpriteFacePath(this.resource_reference_unit_id_UnitUnit, name) : UnitUnit.GetSpriteFacePath(this.ID, this.resource_reference_unit_id_UnitUnit, jobOrMetamorId, name));
    }

    public Future<Sprite> LoadSpriteFace(string name)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(UnitUnit.GetSpriteFacePath(this.resource_reference_unit_id.ID, name));
    }

    public Future<Sprite> LoadSpriteFace(int jobOrMetamorId, string name)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(this.job_UnitJob == jobOrMetamorId ? UnitUnit.GetSpriteFacePath(this.resource_reference_unit_id_UnitUnit, name) : UnitUnit.GetSpriteFacePath(this.ID, this.resource_reference_unit_id_UnitUnit, jobOrMetamorId, name));
    }

    private static string GetSpriteEyePath(int id, string name)
    {
      return UnitUnit.sDirectory2D(id) + "Eye/" + name;
    }

    private static string GetSpriteEyePath(int unitId, int refId, int jobOrMetamorId, string name)
    {
      return UnitUnit.sDirectory2D(unitId, refId, jobOrMetamorId) + "Eye/" + name;
    }

    public bool HasSpriteEye(string name)
    {
      return Singleton<ResourceManager>.GetInstance().Contains(UnitUnit.GetSpriteEyePath(this.resource_reference_unit_id.ID, name));
    }

    public bool HasSpriteEye(int jobOrMetamorId, string name)
    {
      return Singleton<ResourceManager>.GetInstance().Contains(this.job_UnitJob == jobOrMetamorId ? UnitUnit.GetSpriteEyePath(this.resource_reference_unit_id_UnitUnit, name) : UnitUnit.GetSpriteEyePath(this.ID, this.resource_reference_unit_id_UnitUnit, jobOrMetamorId, name));
    }

    public Future<Sprite> LoadSpriteEye(string name)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(UnitUnit.GetSpriteEyePath(this.resource_reference_unit_id.ID, name));
    }

    public Future<Sprite> LoadSpriteEye(int jobOrMetamorId, string name)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(this.job_UnitJob == jobOrMetamorId ? UnitUnit.GetSpriteEyePath(this.resource_reference_unit_id_UnitUnit, name) : UnitUnit.GetSpriteEyePath(this.ID, this.resource_reference_unit_id_UnitUnit, jobOrMetamorId, name));
    }

    public Future<Sprite> LoadSpriteThumbnail()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/c_thum", (object) this.resource_reference_unit_id.ID));
    }

    public Future<Sprite> LoadSpriteThumbnail(int jobOrMetamorId)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(this.combinePath2D(jobOrMetamorId, "c_thum"));
    }

    private string combinePath2D(string filename) => this.directory2D() + filename;

    private string combinePath2D(int jobOrMetamorId, string filename)
    {
      if (this.job_UnitJob == jobOrMetamorId)
        return this.combinePath2D(filename);
      string path = this.directory2D(jobOrMetamorId) + filename;
      if (!Singleton<ResourceManager>.GetInstance().Contains(path))
        path = this.combinePath2D(filename);
      return path;
    }

    private string directory2DWithJob(int jobOrMetamorId)
    {
      return this.job_UnitJob != jobOrMetamorId ? this.directory2D(jobOrMetamorId) : this.directory2D();
    }

    private string directory2D(int jobOrMetamorId)
    {
      return UnitUnit.sDirectory2D(this.ID, this.resource_reference_unit_id_UnitUnit, jobOrMetamorId);
    }

    private static string sDirectory2D(int unitId, int refId, int jobOrMetamorId)
    {
      UnitModel unitModel = Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.unit_id_UnitUnit == unitId && x.job_metamor_id == jobOrMetamorId));
      return unitModel == null || !unitModel.resource_reference_unit_id.HasValue ? "AssetBundle/Resources/Units/" + refId.ToString() + "/" + jobOrMetamorId.ToString() + "/2D/" : "AssetBundle/Resources/Units/" + unitModel.resource_reference_unit_id.Value.ToString() + "/" + jobOrMetamorId.ToString() + "/2D/";
    }

    private string directory2D() => UnitUnit.sDirectory2D(this.resource_reference_unit_id_UnitUnit);

    private static string sDirectory2D(int refId)
    {
      return "AssetBundle/Resources/Units/" + refId.ToString() + "/2D/";
    }

    public Future<Sprite> LoadSpriteBasic()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/unit_large", (object) this.resource_reference_unit_id.ID));
    }

    public Future<Sprite> LoadSpriteBasic(int jobOrMetamorId)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(this.combinePath2D(jobOrMetamorId, "unit_large"));
    }

    public Future<GameObject> LoadModelDuel(int ext_id)
    {
      string path = string.Format("Units/{0}/{1}/3D/duel/prefab", (object) (this.model_reference_id != 0 ? this.model_reference_id : this.resource_reference_unit_id_UnitUnit), (object) ext_id);
      return Singleton<ResourceManager>.GetInstance().Contains(path) ? Singleton<ResourceManager>.GetInstance().Load<GameObject>(path) : this.LoadModelDuel();
    }

    public Future<GameObject> LoadModelDuel()
    {
      return this.model_reference_id != 0 ? Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("Units/{0}/3D/duel/prefab", (object) this.model_reference_id)) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("Units/{0}/3D/duel/prefab", (object) this.resource_reference_unit_id_UnitUnit));
    }

    public Future<GameObject> LoadModelField(int ext_id)
    {
      string path = string.Format("Units/{0}/{1}/3D/field/prefab", (object) (this.model_reference_id != 0 ? this.model_reference_id : this.resource_reference_unit_id_UnitUnit), (object) ext_id);
      return Singleton<ResourceManager>.GetInstance().Contains(path) ? Singleton<ResourceManager>.GetInstance().Load<GameObject>(path) : this.LoadModelField();
    }

    public Future<GameObject> LoadModelField()
    {
      return this.model_reference_id != 0 ? Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("Units/{0}/3D/field/prefab", (object) this.model_reference_id)) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("Units/{0}/3D/field/prefab", (object) this.resource_reference_unit_id_UnitUnit));
    }

    public Future<GameObject> LoadModelDuelVehicle()
    {
      return string.IsNullOrEmpty(this.vehicle_model_name) ? Future.Single<GameObject>((GameObject) null) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("UnitVehicle/{0}/3D/duel/prefab", (object) this.vehicle_model_name));
    }

    public Future<RuntimeAnimatorController> LoadAnimatorControllerDuelVehicle(
      GearModelKind modelKind)
    {
      return string.IsNullOrEmpty(this.vehicle_model_name) ? Future.Single<RuntimeAnimatorController>((RuntimeAnimatorController) null) : Singleton<ResourceManager>.GetInstance().LoadOrNull<RuntimeAnimatorController>(string.Format("UnitVehicle/{0}/3D/duel/controller/{1}", (object) this.vehicle_model_name, (object) modelKind.ID));
    }

    public Future<GameObject> LoadModelFieldVehicle()
    {
      return string.IsNullOrEmpty(this.vehicle_model_name) ? Future.Single<GameObject>((GameObject) null) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("UnitVehicle/{0}/3D/field/prefab", (object) this.vehicle_model_name));
    }

    public Future<GameObject> LoadModelDuelEquip()
    {
      return string.IsNullOrEmpty(this.equip_model_name) ? Future.Single<GameObject>((GameObject) null) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("UnitEquips/{0}/3D/duel/prefab", (object) this.equip_model_name));
    }

    public Future<GameObject> LoadModelFieldEquip()
    {
      return string.IsNullOrEmpty(this.equip_model_name) ? Future.Single<GameObject>((GameObject) null) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("UnitEquips/{0}/3D/field/prefab", (object) this.equip_model_name));
    }

    public Future<GameObject> LoadModelDuelEquipB()
    {
      return string.IsNullOrEmpty(this.equip_model_b_name) ? Future.Single<GameObject>((GameObject) null) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("UnitEquips_b/{0}/3D/duel/prefab", (object) this.equip_model_b_name));
    }

    public Future<GameObject> LoadModelFieldEquipB()
    {
      return string.IsNullOrEmpty(this.equip_model_b_name) ? Future.Single<GameObject>((GameObject) null) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("UnitEquips_b/{0}/3D/field/prefab", (object) this.equip_model_b_name));
    }

    public Future<GameObject> LoadModelUnitAuraEffect(
      out string attach_node,
      int job_id = 0,
      bool isLightModel = false)
    {
      attach_node = string.Empty;
      if (job_id == 0)
        return Future.Single<GameObject>((GameObject) null);
      UnitModel unitModel = Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.unit_id_UnitUnit == this.ID && x.job_metamor_id == job_id));
      if (unitModel == null || !unitModel.attach_effect_id.HasValue)
        return Future.Single<GameObject>((GameObject) null);
      string str = isLightModel ? "field" : "duel";
      string path = string.Format("UnitAuraEffects/{0}/{1}/effect", (object) unitModel.attach_effect_id, (object) str);
      if (!Singleton<ResourceManager>.GetInstance().Contains(path))
        return Future.Single<GameObject>((GameObject) null);
      if (!string.IsNullOrEmpty(unitModel.attach_node))
        attach_node = unitModel.attach_node;
      return Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
    }

    public Future<GameObject> LoadAwakeEffect()
    {
      string path = "UnitAuraEffects/1001/ef_root";
      return !Singleton<ResourceManager>.GetInstance().Contains(path) ? Future.Single<GameObject>((GameObject) null) : Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
    }

    public IEnumerator ProcessAttachAwakeEffect(GameObject unitObj)
    {
      if (this.awake_unit_flag && !UnitUnit.IsAttachedAwakeEffect(unitObj))
      {
        IEnumerator e = this.AttachAwakeEffect(unitObj);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }

    public void ProcessAttachAwakeEffect(GameObject unitObj, GameObject effectPrefab)
    {
      if (!this.awake_unit_flag || UnitUnit.IsAttachedAwakeEffect(unitObj))
        return;
      this.AttachAwakeEffect(unitObj, effectPrefab);
    }

    private void AttachAwakeEffect(GameObject unitObj, GameObject effectPrefab)
    {
      Transform childInFind = unitObj.transform.GetChildInFind("Bip");
      if (Object.op_Equality((Object) childInFind, (Object) null))
        childInFind = unitObj.transform.GetChildInFind("bip");
      if (!Object.op_Inequality((Object) childInFind, (Object) null) || !Object.op_Inequality((Object) effectPrefab, (Object) null))
        return;
      effectPrefab.Clone(childInFind);
    }

    private IEnumerator AttachAwakeEffect(GameObject unitObj)
    {
      Future<GameObject> effect = this.LoadAwakeEffect();
      IEnumerator e = effect.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.AttachAwakeEffect(unitObj, effect.Result);
    }

    private static bool IsAttachedAwakeEffect(GameObject unitObj)
    {
      return Object.op_Inequality((Object) unitObj.transform.GetChildInFind("ef_root"), (Object) null);
    }

    public Future<Sprite> LoadFullSprite(float pixelsPerUnit = 1f)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/unit_full", (object) this.resource_reference_unit_id.ID), pixelsPerUnit);
    }

    public Future<Sprite> LoadFullSprite(int ext_id, float pixelsPerUnit = 1f)
    {
      string path = string.Format("AssetBundle/Resources/Units/{0}/{1}/2D/unit_full", (object) this.resource_reference_unit_id.ID, (object) ext_id);
      return Singleton<ResourceManager>.GetInstance().Contains(path) ? Singleton<ResourceManager>.GetInstance().Load<Sprite>(path) : this.LoadFullSprite(pixelsPerUnit);
    }

    public bool ContsinsFullSprite(int ext_id)
    {
      string path = string.Format("AssetBundle/Resources/Units/{0}/{1}/2D/unit_full", (object) this.resource_reference_unit_id.ID, (object) ext_id);
      return Singleton<ResourceManager>.GetInstance().Contains(path);
    }

    public Future<Sprite> LoadHiResSprite()
    {
      return Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/unit_hires", (object) this.resource_reference_unit_id.ID));
    }

    public Future<Sprite> LoadHiResSprite(int job_id)
    {
      string path = string.Format("AssetBundle/Resources/Units/{0}/{1}/2D/unit_hires", (object) this.resource_reference_unit_id.ID, (object) job_id);
      return Singleton<ResourceManager>.GetInstance().Contains(path) ? Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path) : this.LoadHiResSprite();
    }

    public Future<Sprite> LoadJobFullSprite(int ext_id)
    {
      if (this.job_UnitJob == ext_id)
        return this.LoadFullSprite();
      string path = UnitUnit.sDirectory2D(this.ID, this.resource_reference_unit_id_UnitUnit, ext_id) + "unit_full";
      return Singleton<ResourceManager>.GetInstance().Contains(path) ? Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path) : this.LoadFullSprite();
    }

    public Future<Sprite> LoadAwakeningText(int textNumber)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/text{1}", (object) this.resource_reference_unit_id.ID, (object) textNumber));
    }

    public Future<Sprite> LoadSpriteEventImage(float pixelsPerUnit = 1f)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/EventImages/c{0}", (object) this.resource_reference_unit_id.ID), pixelsPerUnit);
    }

    public Future<Sprite> LoadSpriteFlavorText(float pixelsPerUnit = 1f)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/flavor_text", (object) this.resource_reference_unit_id.ID), pixelsPerUnit);
    }

    public Future<Material> LoadFieldNormalFaceMaterial()
    {
      if (string.IsNullOrEmpty(this.field_normal_face_material_name))
        return Future.Single<Material>((Material) null);
      string path = string.Format("Units/Shared/{0}", (object) this.field_normal_face_material_name);
      return Singleton<ResourceManager>.GetInstance().Load<Material>(path);
    }

    public Future<Material> LoadFieldNormalFaceMaterial(int extID)
    {
      UnitModel unitModel = Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.unit_id_UnitUnit == this.ID && x.job_metamor_id == extID));
      if (unitModel == null || string.IsNullOrEmpty(unitModel.field_normal_face_material_name))
        return this.LoadFieldNormalFaceMaterial();
      string path = string.Format("Units/Shared/{0}", (object) unitModel.field_normal_face_material_name);
      return Singleton<ResourceManager>.GetInstance().Load<Material>(path);
    }

    public IEnumerator SetLargeSprite(GameObject go, int depth = 1000)
    {
      UI2DSprite sprite = go.GetComponent<UI2DSprite>();
      Future<Sprite> large = this.LoadSpriteLarge();
      IEnumerator e = large.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sprite.sprite2D = large.Result;
      ((UIWidget) sprite).depth = depth;
    }

    public IEnumerator SetLargeSpriteSnap(GameObject go, int depth = 1000)
    {
      UI2DSprite sprite = go.GetComponent<UI2DSprite>();
      Future<Sprite> large = this.LoadSpriteLarge();
      IEnumerator e = large.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sprite.sprite2D = large.Result;
      ((UIWidget) sprite).depth = depth;
      UI2DSprite ui2Dsprite = sprite;
      Rect textureRect1 = sprite.sprite2D.textureRect;
      int width = (int) ((Rect) ref textureRect1).width;
      Rect textureRect2 = sprite.sprite2D.textureRect;
      int height = (int) ((Rect) ref textureRect2).height;
      ((UIWidget) ui2Dsprite).SetDimensions(width, height);
    }

    public IEnumerator SetLargeSpriteSnap(int jobOrMetamorId, GameObject go, int depth = 1000)
    {
      UI2DSprite sprite = go.GetComponent<UI2DSprite>();
      Future<Sprite> large = this.LoadSpriteLarge(jobOrMetamorId, 1f);
      IEnumerator e = large.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sprite.sprite2D = large.Result;
      ((UIWidget) sprite).depth = depth;
      UI2DSprite ui2Dsprite = sprite;
      Rect textureRect1 = sprite.sprite2D.textureRect;
      int width = (int) ((Rect) ref textureRect1).width;
      Rect textureRect2 = sprite.sprite2D.textureRect;
      int height = (int) ((Rect) ref textureRect2).height;
      ((UIWidget) ui2Dsprite).SetDimensions(width, height);
    }

    public IEnumerator SetLargeSpriteWithMask(
      GameObject go,
      Future<Texture2D> maskFuture,
      int depth = 1000,
      int x = 0,
      int y = 0)
    {
      UI2DSprite ui2dsprite = go.GetComponent<UI2DSprite>();
      Future<Sprite> spriteFuture = this.LoadSpriteLarge();
      IEnumerator e = spriteFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = maskFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result1 = spriteFuture.Result;
      Texture2D result2 = maskFuture.Result;
      string str = "unit" + (object) this.ID;
      ((Object) result1).name = str;
      ui2dsprite.sprite2D = spriteFuture.Result;
      ((UIWidget) ui2dsprite).depth = depth;
      NGxMaskSpriteWithScale component = go.GetComponent<NGxMaskSpriteWithScale>();
      component.maskTexture = result2;
      component.xOffsetPixel += x;
      component.yOffsetPixel += y;
      component.FitMask();
    }

    public IEnumerator SetLargeSpriteWithMask(
      int jobOrMetamorId,
      GameObject go,
      Future<Texture2D> maskFuture,
      int depth = 1000,
      int x = 0,
      int y = 0)
    {
      UI2DSprite ui2dsprite = go.GetComponent<UI2DSprite>();
      Future<Sprite> spriteFuture = this.LoadSpriteLarge(jobOrMetamorId, 1f);
      IEnumerator e = spriteFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = maskFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result1 = spriteFuture.Result;
      Texture2D result2 = maskFuture.Result;
      string str = "unit" + (object) this.ID;
      ((Object) result1).name = str;
      ui2dsprite.sprite2D = spriteFuture.Result;
      ((UIWidget) ui2dsprite).depth = depth;
      NGxMaskSpriteWithScale component = go.GetComponent<NGxMaskSpriteWithScale>();
      component.maskTexture = result2;
      component.xOffsetPixel += x;
      component.yOffsetPixel += y;
      component.FitMask();
    }

    public IEnumerator SetMediumSpriteSnap(GameObject go, int depth = 1000)
    {
      UI2DSprite sprite = go.GetComponent<UI2DSprite>();
      Future<Sprite> medium = this.LoadSpriteMedium();
      IEnumerator e = medium.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sprite.sprite2D = medium.Result;
      ((UIWidget) sprite).depth = depth;
      UI2DSprite ui2Dsprite = sprite;
      Rect textureRect1 = sprite.sprite2D.textureRect;
      int width = (int) ((Rect) ref textureRect1).width;
      Rect textureRect2 = sprite.sprite2D.textureRect;
      int height = (int) ((Rect) ref textureRect2).height;
      ((UIWidget) ui2Dsprite).SetDimensions(width, height);
    }

    public IEnumerator SetMediumSpriteWithMask(
      GameObject go,
      Future<Texture2D> maskFuture,
      int depth = 1000,
      int x = 0,
      int y = 0)
    {
      UI2DSprite ui2dsprite = go.GetComponent<UI2DSprite>();
      Future<Sprite> spriteFuture = this.LoadSpriteMedium();
      IEnumerator e = spriteFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = maskFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result1 = spriteFuture.Result;
      Texture2D result2 = maskFuture.Result;
      string str = "unit" + (object) this.ID;
      ((Object) result1).name = str;
      ui2dsprite.sprite2D = spriteFuture.Result;
      ((UIWidget) ui2dsprite).depth = depth;
      NGxMaskSpriteWithScale component = go.GetComponent<NGxMaskSpriteWithScale>();
      component.maskTexture = result2;
      component.xOffsetPixel += x;
      component.yOffsetPixel += y;
      component.FitMask();
    }

    private Future<GameObject> fullGameObject(float scale, int x, int y)
    {
      return Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/UnitIcon/full").Then<GameObject>((Func<GameObject, GameObject>) (full =>
      {
        NGxMaskSpriteWithScale component = full.GetComponent<NGxMaskSpriteWithScale>();
        component.xOffsetPixel = x;
        component.yOffsetPixel = y;
        component.scale = scale;
        return full;
      }));
    }

    private Future<GameObject> fullGameObject()
    {
      return Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/UnitIcon/full").Then<GameObject>((Func<GameObject, GameObject>) (full =>
      {
        UnitUnitStory unitUnitStory = (UnitUnitStory) null;
        float num1 = 1f;
        int num2 = 0;
        int num3 = 0;
        if (MasterData.UnitUnitStory.TryGetValue(this.resource_reference_unit_id_UnitUnit, out unitUnitStory))
        {
          num1 = unitUnitStory.story_texture_scale;
          num2 = unitUnitStory.story_texture_x;
          num3 = unitUnitStory.story_texture_y;
        }
        NGxMaskSpriteWithScale component = full.GetComponent<NGxMaskSpriteWithScale>();
        component.xOffsetPixel = num2;
        component.yOffsetPixel = num3;
        component.scale = num1;
        return full;
      }));
    }

    private Future<GameObject> fullWithFaceGameObject()
    {
      return Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/UnitIcon/fullWithFace");
    }

    public void SetStoryData(GameObject go, string name = "normal", int? ext_id = null)
    {
      int num1;
      if (ext_id.HasValue && ext_id.Value != 0)
      {
        int jobUnitJob = this.job_UnitJob;
        int? nullable = ext_id;
        int valueOrDefault = nullable.GetValueOrDefault();
        num1 = !(jobUnitJob == valueOrDefault & nullable.HasValue) ? 1 : 0;
      }
      else
        num1 = 0;
      bool flag = num1 != 0;
      UnitUnitStory unitUnitStory = (UnitUnitStory) null;
      float num2 = 1f;
      int num3 = 0;
      int num4 = 0;
      UnitExtensionStory unitExtensionStory;
      if (flag && (unitExtensionStory = Array.Find<UnitExtensionStory>(MasterData.UnitExtensionStoryList, (Predicate<UnitExtensionStory>) (s =>
      {
        if (s.unit != this.resource_reference_unit_id_UnitUnit)
          return false;
        int jobMetamorId = s.job_metamor_id;
        int? nullable = ext_id;
        int valueOrDefault = nullable.GetValueOrDefault();
        return jobMetamorId == valueOrDefault & nullable.HasValue;
      }))) != null)
      {
        num2 = unitExtensionStory.story_texture_scale;
        num3 = unitExtensionStory.story_texture_x;
        num4 = unitExtensionStory.story_texture_y;
      }
      else if (MasterData.UnitUnitStory.TryGetValue(this.resource_reference_unit_id_UnitUnit, out unitUnitStory))
      {
        num2 = unitUnitStory.story_texture_scale;
        num3 = unitUnitStory.story_texture_x;
        num4 = unitUnitStory.story_texture_y;
      }
      ((Object) go).name = "StoryUnitPrefab";
      NGxMaskSpriteWithScale component1 = go.GetComponent<NGxMaskSpriteWithScale>();
      component1.xOffsetPixel = num3;
      component1.yOffsetPixel = num4;
      component1.scale = num2;
      NGxFaceSprite component2 = go.GetComponent<NGxFaceSprite>();
      component2.UnitID = this.ID;
      if (flag)
        component2.ExtID = ext_id;
      component2.faceName = name;
      NGxEyeSprite component3 = go.GetComponent<NGxEyeSprite>();
      if (!Object.op_Inequality((Object) component3, (Object) null))
        return;
      ((Behaviour) component3).enabled = false;
      ((Component) component3.EyeUI2DSprite).gameObject.SetActive(false);
      component3.UnitID = this.ID;
      if (flag)
        component3.ExtID = ext_id;
      component3.EyeName = name;
    }

    public Future<GameObject> LoadStory() => this.fullWithFaceGameObject();

    public Future<GameObject> LoadQuest() => this.fullGameObject(0.7f, 74, -14);

    public IEnumerator LoadQuestWithMask(Transform parent, int depth, Future<Texture2D> maskFuture)
    {
      Future<GameObject> LeaderF = this.LoadQuest();
      IEnumerator e = LeaderF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject go = LeaderF.Result.Clone(parent);
      go.GetComponent<NGxMaskSpriteWithScale>().isTopFit = false;
      e = this.SetLargeSpriteWithMask(go, maskFuture, depth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    public IEnumerator LoadQuestWithMask(
      int jobOrMetamorId,
      Transform parent,
      int depth,
      Future<Texture2D> maskFuture)
    {
      Future<GameObject> LeaderF = this.LoadQuest();
      IEnumerator e = LeaderF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject go = LeaderF.Result.Clone(parent);
      go.GetComponent<NGxMaskSpriteWithScale>().isTopFit = false;
      e = this.SetLargeSpriteWithMask(jobOrMetamorId, go, maskFuture, depth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    public Future<GameObject> LoadShopContent() => this.fullGameObject(0.5f, 66, 51);

    public Future<GameObject> LoadShopContentUnitOther() => this.fullGameObject(0.7f, 13, 23);

    public IEnumerator LoadShopContentWithMask(
      Transform parent,
      int depth,
      Future<Texture2D> maskFuture)
    {
      Future<GameObject> LeaderF = this.LoadShopContent();
      IEnumerator e = LeaderF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject go = LeaderF.Result.Clone(parent);
      go.GetComponent<NGxMaskSpriteWithScale>().isTopFit = false;
      e = this.SetLargeSpriteWithMask(go, maskFuture, depth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    public IEnumerator LoadShopContentUnitOtherWithMask(
      Transform parent,
      int depth,
      Future<Texture2D> maskFuture)
    {
      Future<GameObject> LeaderF = this.LoadShopContentUnitOther();
      IEnumerator e = LeaderF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject go = LeaderF.Result.Clone(parent);
      go.GetComponent<NGxMaskSpriteWithScale>().isTopFit = false;
      e = this.SetMediumSpriteWithMask(go, maskFuture, depth);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }

    public Future<GameObject> LoadCompose() => this.fullGameObject(0.6f, 0, -130);

    public Future<GameObject> LoadLove() => this.fullGameObject(0.6f, -35, -91);

    public Future<GameObject> LoadMypage() => this.fullGameObject(1f, 0, 0);

    public Future<GameObject> LoadShop() => this.fullGameObject(0.8f, -72, 70);

    public Future<GameObject> LoadGacha() => this.fullGameObject(0.84f, 0, 0);

    public Future<GameObject> LoadColosseum() => this.fullGameObject();

    public Future<GameObject> LoadInitialEquippedGearModel()
    {
      return MasterData.GearGear[this.initial_gear.ID].LoadModel();
    }

    public void SetCuntrySpriteName(ref UISprite refSprite)
    {
      if (!this.country_attribute.HasValue || !Object.op_Inequality((Object) refSprite, (Object) null))
        return;
      string str = string.Format("slc_{0}.png__GUI__unit_sort_country__unit_sort_country_prefab", (object) this.country_attribute.Value);
      UISpriteData sprite = refSprite.atlas.GetSprite(str);
      if (sprite == null)
        return;
      refSprite.spriteName = str;
      ((UIWidget) refSprite).SetDimensions(sprite.width, sprite.height);
    }

    public void SetNonCuntrySpriteName(ref UISprite refSprite)
    {
      if (!Object.op_Inequality((Object) refSprite, (Object) null))
        return;
      string str = "slc_blank.png__GUI__unit_sort_country__unit_sort_country_prefab";
      UISpriteData sprite = refSprite.atlas.GetSprite(str);
      if (sprite == null)
        return;
      refSprite.spriteName = str;
      ((UIWidget) refSprite).SetDimensions(sprite.width, sprite.height);
    }

    public IEnumerator SetInclusionIP(UI2DSprite sprite)
    {
      if (this.inclusion_ip.HasValue)
      {
        Future<Sprite> sprite2d = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Units/{0}/2D/slc_attribute", (object) this.inclusion_ip.Value));
        IEnumerator e = sprite2d.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Sprite result = sprite2d.Result;
        sprite.sprite2D = result;
        UIWidget component = ((Component) sprite).GetComponent<UIWidget>();
        Rect rect1 = result.rect;
        int width = (int) ((Rect) ref rect1).width;
        Rect rect2 = result.rect;
        int height = (int) ((Rect) ref rect2).height;
        component.SetDimensions(width, height);
        sprite2d = (Future<Sprite>) null;
      }
    }

    private string duelAnimatorControllerName(int job_id, int metamor_id)
    {
      GearGear initialGear = this.GetInitialGear(job_id);
      UnitUnitGearModelKind unitGearModelKind = (UnitUnitGearModelKind) null;
      if (metamor_id != 0)
      {
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind, metamor_id);
        if (unitGearModelKind != null && string.IsNullOrEmpty(unitGearModelKind.duel_animator_controller_name))
          unitGearModelKind = (UnitUnitGearModelKind) null;
      }
      if (unitGearModelKind == null && job_id != this.job_UnitJob)
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind, job_id);
      if (unitGearModelKind == null)
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind);
      return unitGearModelKind?.duel_animator_controller_name;
    }

    public Future<RuntimeAnimatorController> LoadInitialDuelAnimator(int job_id, int metamor_id = 0)
    {
      string path = "";
      GearModelKind modelKind = this.GetInitialGear(job_id).model_kind;
      UnitUnitGearModelKind unitGearModelKind = (metamor_id != 0 ? this.getUnitGearModelKind(modelKind, metamor_id) : (UnitUnitGearModelKind) null) ?? this.getUnitGearModelKind(modelKind, job_id);
      if (unitGearModelKind != null)
        path = string.Format(UnitUnit.duelAnimatorRootPath, (object) unitGearModelKind.duel_animator_controller_name);
      return Singleton<ResourceManager>.GetInstance().Contains(path) ? Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(path) : Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(string.Format(UnitUnit.duelAnimatorRootPath, (object) this.duelAnimatorControllerName(job_id, 0)));
    }

    public Future<RuntimeAnimatorController> LoadHomeDuelAnimator()
    {
      return this.character.personality == UnitPersonality.none ? Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(string.Format(UnitUnit.homeAnimatorRootPath, (object) UnitPersonality.normal.ToString())) : Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(string.Format(UnitUnit.homeAnimatorRootPath, (object) this.character.personality.ToString()));
    }

    private string winAnimatorControllerName(int job_id, int metamor_id)
    {
      GearGear initialGear = this.GetInitialGear(job_id);
      UnitUnitGearModelKind unitGearModelKind = metamor_id != 0 ? this.getUnitGearModelKind(initialGear.model_kind, metamor_id) : (UnitUnitGearModelKind) null;
      if (unitGearModelKind == null && job_id != 0 && job_id != this.job_UnitJob)
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind, job_id);
      if (unitGearModelKind == null)
        unitGearModelKind = this.getUnitGearModelKind(initialGear.model_kind);
      return unitGearModelKind?.winning_animator_controller_name;
    }

    public Future<RuntimeAnimatorController> LoadInitialWinAnimator(int job_id, int metamor_id = 0)
    {
      return Singleton<ResourceManager>.GetInstance().Load<RuntimeAnimatorController>(string.Format(UnitUnit.winAnimatorRootPath, (object) this.winAnimatorControllerName(job_id, metamor_id)));
    }

    public string getEventImageName()
    {
      int[] numArray1;
      if (this.ID == this.resource_reference_unit_id_UnitUnit)
        numArray1 = new int[1]{ this.ID };
      else
        numArray1 = new int[2]
        {
          this.ID,
          this.resource_reference_unit_id_UnitUnit
        };
      int[] numArray2 = numArray1;
      string eventImageName = (string) null;
      for (int index = 0; index < numArray2.Length; ++index)
      {
        string path = string.Format("AssetBundle/Resources/EventImages/c{0}", (object) numArray2[index]);
        if (Singleton<ResourceManager>.GetInstance().Contains(path + "_1"))
        {
          eventImageName = path + "_1";
          break;
        }
        if (Singleton<ResourceManager>.GetInstance().Contains(path))
        {
          eventImageName = path;
          break;
        }
      }
      return eventImageName;
    }

    public UnitVoicePattern unitVoicePattern
    {
      get
      {
        if (this.unit_voice_pattern_id == 0)
          return (UnitVoicePattern) null;
        return this._unitVoicePattern == null && !MasterData.UnitVoicePattern.TryGetValue(this.unit_voice_pattern_id, out this._unitVoicePattern) ? (UnitVoicePattern) null : this._unitVoicePattern;
      }
    }

    public UnitVoicePattern getVoicePattern(int job_metamor_id)
    {
      if (job_metamor_id != 0)
      {
        UnitModel unitModel = Array.Find<UnitModel>(MasterData.UnitModelList, (Predicate<UnitModel>) (x => x.unit_id_UnitUnit == this.ID && x.job_metamor_id == job_metamor_id));
        if (unitModel != null)
        {
          UnitVoicePattern voicePattern = unitModel.voice_pattern;
          if (voicePattern != null)
            return voicePattern;
        }
      }
      return this.unitVoicePattern;
    }

    public string getName(int metamor_id)
    {
      UnitCharacterExtension characterExtension = this.getCharacterExtension(metamor_id);
      return characterExtension == null ? this.name : characterExtension.name;
    }

    public string getFormalName(int metamor_id)
    {
      UnitCharacterExtension characterExtension = this.getCharacterExtension(metamor_id);
      return characterExtension != null ? (!string.IsNullOrEmpty(characterExtension.formal_name) ? characterExtension.formal_name : characterExtension.name) : (!string.IsNullOrEmpty(this.formal_name) ? this.formal_name : this.name);
    }

    public UnitCharacterExtension getCharacterExtension(int job_metamor_id)
    {
      return job_metamor_id == 0 ? (UnitCharacterExtension) null : Array.Find<UnitCharacterExtension>(MasterData.UnitCharacterExtensionList, (Predicate<UnitCharacterExtension>) (x => x.unit_id_UnitUnit == this.ID && x.job_metamor_id == job_metamor_id));
    }

    public bool CanAwakeUnitFlag => this.can_awake_unit_flag && this.IsEvolution;

    public bool IsEvolution
    {
      get
      {
        if (!this._isEvolution.HasValue)
        {
          this._isEvolution = new bool?(false);
          for (int index = 0; index < MasterData.UnitEvolutionPatternList.Length; ++index)
          {
            if (MasterData.UnitEvolutionPatternList[index].unit_UnitUnit == this.ID)
            {
              DateTime? publishedAt = MasterData.UnitEvolutionPatternList[index].target_unit.published_at;
              DateTime dateTime = ServerTime.NowAppTime();
              if ((publishedAt.HasValue ? (publishedAt.GetValueOrDefault() < dateTime ? 1 : 0) : 0) != 0)
              {
                this._isEvolution = new bool?(true);
                break;
              }
            }
          }
        }
        return this._isEvolution.Value;
      }
    }

    public bool IsEvolutioned
    {
      get
      {
        if (!this._isEvolutioned.HasValue)
        {
          this._isEvolutioned = new bool?(false);
          DateTime dateTime = ServerTime.NowAppTime();
          foreach (UnitEvolutionPattern evolutionPattern in MasterData.UnitEvolutionPatternList)
          {
            if (evolutionPattern.target_unit_UnitUnit == this.ID)
            {
              DateTime? publishedAt = evolutionPattern.unit.published_at;
              if (!publishedAt.HasValue || publishedAt.Value <= dateTime)
              {
                this._isEvolutioned = new bool?(true);
                break;
              }
            }
          }
        }
        return this._isEvolutioned.Value;
      }
    }

    public bool GetPiece
    {
      get
      {
        return Singleton<NGGameDataManager>.GetInstance().GetTablePieceSameCharacterIds.ContainsKey(this.same_character_id);
      }
    }

    public UnityValueUpPattern FindValueUpPattern(
      UnitUnit target,
      Func<UnitFamily[]> funcGetFamilies)
    {
      UnityValueUpPattern[] unityValueUpPatterns = this.UnityValueUpPatterns;
      return unityValueUpPatterns == null || UnitMaterialExclusion.checkExclusion(target, this.UnitMaterialExclusions) ? (UnityValueUpPattern) null : Array.Find<UnityValueUpPattern>(unityValueUpPatterns, (Predicate<UnityValueUpPattern>) (p => p.checkUnityValueUP(target, funcGetFamilies)));
    }

    public UnityValueUpPattern[] UnityValueUpPatterns
    {
      get
      {
        if (!this.is_unity_value_up)
          return (UnityValueUpPattern[]) null;
        if (this.wUnityValueUpPatterns_ != null)
          return this.wUnityValueUpPatterns_;
        this.wUnityValueUpPatterns_ = ((IEnumerable<UnityValueUpPattern>) MasterData.UnityValueUpPatternList).Where<UnityValueUpPattern>((Func<UnityValueUpPattern, bool>) (u => u.material_unit_UnitUnit == this.ID)).ToArray<UnityValueUpPattern>();
        return this.wUnityValueUpPatterns_;
      }
    }

    public Dictionary<UnitMaterialExclusionType, HashSet<int>> UnitMaterialExclusions
    {
      get
      {
        if (!this.is_unity_value_up && !this.isTrustMaterial)
          return (Dictionary<UnitMaterialExclusionType, HashSet<int>>) null;
        if (this.wUnitMaterialExclusions_ != null)
          return this.wUnitMaterialExclusions_;
        this.wUnitMaterialExclusions_ = UnitMaterialExclusion.getExclusions(this.ID);
        return this.wUnitMaterialExclusions_;
      }
    }

    public UnityPureValueUpPattern FindPureValueUpPattern(UnitUnit target)
    {
      UnityPureValueUpPattern[] pureValueUpPatterns = this.UnityPureValueUpPatterns;
      return pureValueUpPatterns == null ? (UnityPureValueUpPattern) null : Array.Find<UnityPureValueUpPattern>(pureValueUpPatterns, (Predicate<UnityPureValueUpPattern>) (p => p.checkUnityPureValueUP(target)));
    }

    public UnityPureValueUpPattern[] UnityPureValueUpPatterns
    {
      get
      {
        if (!this.is_unity_value_up)
          return (UnityPureValueUpPattern[]) null;
        if (this.wPureUnityValueUpPatterns_ != null)
          return this.wPureUnityValueUpPatterns_;
        this.wPureUnityValueUpPatterns_ = ((IEnumerable<UnityPureValueUpPattern>) MasterData.UnityPureValueUpPatternList).Where<UnityPureValueUpPattern>((Func<UnityPureValueUpPattern, bool>) (u => u.material_unit_UnitUnit == this.ID)).ToArray<UnityPureValueUpPattern>();
        return this.wPureUnityValueUpPatterns_;
      }
    }

    private bool isTrustMaterial
    {
      get
      {
        if (this.isTrustMaterial_.HasValue)
          return this.isTrustMaterial_.Value;
        this.isTrustMaterial_ = new bool?(((IEnumerable<UnitTrustLevelMaterialPattern>) MasterData.UnitTrustLevelMaterialPatternList).Any<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => x.material_unit_UnitUnit == this.ID)));
        return this.isTrustMaterial_.Value;
      }
    }

    public bool IsTrustMaterial(PlayerUnit baseUnit)
    {
      return this.isTrustMaterial && this.TrustMaterialUnit(baseUnit) != null;
    }

    public UnitTrustLevelMaterialPattern TrustMaterialUnit(PlayerUnit BaseUnit)
    {
      UnitUnit bUnit = BaseUnit.unit;
      if (UnitMaterialExclusion.checkExclusion(bUnit, this.UnitMaterialExclusions))
        return (UnitTrustLevelMaterialPattern) null;
      UnitGroup baseUnitGroupIDs = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == bUnit.ID));
      return baseUnitGroupIDs == null ? (UnitTrustLevelMaterialPattern) null : ((IEnumerable<UnitTrustLevelMaterialPattern>) MasterData.UnitTrustLevelMaterialPatternList).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => x.material_unit_UnitUnit == this.ID)).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => !x.rarity_UnitRarity.HasValue || x.rarity == bUnit.rarity)).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => !x.kind_GearKind.HasValue || x.kind == bUnit.kind)).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => !x.element_UnitFamily.HasValue || ((IEnumerable<UnitFamily>) BaseUnit.Families).Any<UnitFamily>((Func<UnitFamily, bool>) (y =>
      {
        int num = (int) y;
        UnitFamily? element = x.element;
        int valueOrDefault = (int) element.GetValueOrDefault();
        return num == valueOrDefault & element.HasValue;
      })))).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => !x.skill_BattleskillSkill.HasValue || ((IEnumerable<PlayerUnitSkills>) BaseUnit.skills).Any<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (y => y.skill == x.skill)))).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => !x.target_character_UnitCharacter.HasValue || x.target_character == bUnit.character)).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => !x.target_unit_UnitUnit.HasValue || x.target_unit_UnitUnit.Value == BaseUnit._unit)).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => x.group_large_category_id_UnitGroupLargeCategory == 1 || x.group_large_category_id.ID == baseUnitGroupIDs.group_large_category_id.ID)).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => x.group_small_category_id_UnitGroupSmallCategory == 1 || x.group_small_category_id.ID == baseUnitGroupIDs.group_small_category_id.ID)).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => x.group_clothing_category_id_UnitGroupClothingCategory == 1 || x.group_clothing_category_id.ID == baseUnitGroupIDs.group_clothing_category_id.ID || x.group_clothing_category_id.ID == baseUnitGroupIDs.group_clothing_category_id_2.ID)).Where<UnitTrustLevelMaterialPattern>((Func<UnitTrustLevelMaterialPattern, bool>) (x => x.group_generation_category_id_UnitGroupGenerationCategory == 1 || x.group_generation_category_id.ID == baseUnitGroupIDs.group_generation_category_id.ID)).FirstOrDefault<UnitTrustLevelMaterialPattern>();
    }

    public List<int> getClassChangeJobIdList()
    {
      List<int> classChangeJobIdList = new List<int>();
      JobChangePatterns jobChangePatterns = JobChangeUtil.getJobChangePatterns(this.ID, this.job_UnitJob);
      if (jobChangePatterns != null)
      {
        classChangeJobIdList.Add(jobChangePatterns.job1_UnitJob);
        classChangeJobIdList.Add(jobChangePatterns.job2_UnitJob);
        if (jobChangePatterns.job3_UnitJob.HasValue)
          classChangeJobIdList.Add(jobChangePatterns.job3_UnitJob.Value);
        else
          classChangeJobIdList.Add(0);
        if (jobChangePatterns.job4_UnitJob.HasValue)
          classChangeJobIdList.Add(jobChangePatterns.job4_UnitJob.Value);
        else
          classChangeJobIdList.Add(0);
        if (jobChangePatterns.job5_UnitJob.HasValue)
          classChangeJobIdList.Add(jobChangePatterns.job5_UnitJob.Value);
        else
          classChangeJobIdList.Add(0);
        if (jobChangePatterns.job6_UnitJob.HasValue)
          classChangeJobIdList.Add(jobChangePatterns.job6_UnitJob.Value);
        else
          classChangeJobIdList.Add(0);
        if (jobChangePatterns.job7_UnitJob.HasValue)
          classChangeJobIdList.Add(jobChangePatterns.job7_UnitJob.Value);
        else
          classChangeJobIdList.Add(0);
      }
      return classChangeJobIdList;
    }

    public bool IsAllEquipUnit
    {
      get
      {
        return ((IEnumerable<string>) Consts.GetInstance().ALL_GEAR_EQUIP_UNIT_IDS.Split(',')).Contains<string>(this.ID.ToString());
      }
    }

    public bool checkEquipPossible(UnitUnit target)
    {
      if (this.IsAllEquipUnit)
        return (target.overkillers_parameter != 0 || target.exist_overkillers_skill) && target.character_UnitCharacter != this.character_UnitCharacter && !((IEnumerable<IgnoreOverkillers>) MasterData.IgnoreOverkillersList).Any<IgnoreOverkillers>((Func<IgnoreOverkillers, bool>) (v => v.same_character_id == target.same_character_id));
      if (OverkillersGroup.IsForAllUnits(target.ID))
      {
        if (target.same_character_id == this.same_character_id || target.overkillers_parameter == 0 && !target.exist_overkillers_skill)
          return false;
      }
      else if (target.character_UnitCharacter != this.character_UnitCharacter || target.same_character_id == this.same_character_id || target.overkillers_parameter == 0)
        return OverkillersGroup.getOverkillersGroupsByParent(this.ID).Any<OverkillersGroup>((Func<OverkillersGroup, bool>) (x => x.child_unit_id == target.ID));
      return true;
    }
  }
}
