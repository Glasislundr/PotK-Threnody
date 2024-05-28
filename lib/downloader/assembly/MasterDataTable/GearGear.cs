// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GearGear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GearGear
  {
    [NonSerialized]
    private GearGearElement[] _elements;
    private GearClassificationPattern gearClassification_;
    private CommonElement? attachedElement_;
    private List<int> _specificationOfEquipmentUnits;
    private List<int> _specificationOfEquipmentJobs;
    private Dictionary<int, int?> _specificationOfEquipmentUnitJobs;
    private static int? manaSeedRecoveryAmount_;
    public int ID;
    public string name;
    public int gear_description_GearGearDescription;
    public int compose_parameter_GearGearComposeParameter;
    public int rarity_GearRarity;
    public int kind_GearKind;
    public int model_kind_GearModelKind;
    public int resource_reference_gear_id_GearGear;
    public int attack_type_GearAttackType;
    public int power;
    public int? fix_damage;
    public int physical_defense;
    public int magic_defense;
    public int hit;
    public int critical;
    public int evasion;
    public int weight;
    public int min_range;
    public int max_range;
    public int hp_incremental;
    public int strength_incremental;
    public int vitality_incremental;
    public int intelligence_incremental;
    public int mind_incremental;
    public int agility_incremental;
    public int dexterity_incremental;
    public int lucky_incremental;
    public int disappearance_type_GearDisappearanceType;
    public int? disappearance_num;
    public int heal;
    public int customize_flag;
    public int history_group_number;
    public DateTime published_at;
    public float hp_incremental_ratio;
    public float strength_incremental_ratio;
    public float vitality_incremental_ratio;
    public float intelligence_incremental_ratio;
    public float mind_incremental_ratio;
    public float agility_incremental_ratio;
    public float dexterity_incremental_ratio;
    public float lucky_incremental_ratio;
    public int? specification_of_equipment_unit_group_id;
    public string bullet_prefab_name;
    public string shoot_ready_effect_prefab_name;
    public float drilling_rate;
    public int? special_drilling_kind_GearKind;
    public int rank_incr_group;
    public int? classification_GearClassificationPattern;
    public int? expire_date_GearExpireDate;
    public int reisou_type_GearReisouType;
    public int? drilling_exp_mythology_id;
    public bool is_jingi;

    public string description => this.gear_description.description;

    public int group_id => this.compose_parameter.group_id;

    public int compose_level => this.compose_parameter.compose_level;

    public GearModelKind compose_kind => this.compose_parameter.compose_kind;

    public int sell_price => this.compose_parameter.sell_price;

    public int repair_price => this.compose_parameter.repair_price;

    public float repair_success_ratio => this.compose_parameter.repair_success_ratio;

    public int hp_buildup_limit => this.compose_parameter.hp_buildup_limit;

    public int strength_buildup_limit => this.compose_parameter.strength_buildup_limit;

    public int vitality_buildup_limit => this.compose_parameter.vitality_buildup_limit;

    public int intelligence_buildup_limit => this.compose_parameter.intelligence_buildup_limit;

    public int mind_buildup_limit => this.compose_parameter.mind_buildup_limit;

    public int agility_buildup_limit => this.compose_parameter.agility_buildup_limit;

    public int dexterity_buildup_limit => this.compose_parameter.dexterity_buildup_limit;

    public int lucky_buildup_limit => this.compose_parameter.lucky_buildup_limit;

    public GearGearElement[] elements
    {
      get
      {
        if (this._elements == null)
          this._elements = ((IEnumerable<GearGearElement>) MasterData.GearGearElementList).Where<GearGearElement>((Func<GearGearElement, bool>) (x => x.gear.ID == this.ID)).ToArray<GearGearElement>();
        return this._elements;
      }
    }

    public UnitFamily[] SpecialAttackTargets
    {
      get
      {
        List<UnitFamily> unitFamilyList = new List<UnitFamily>();
        foreach (GearGearElement element in this.elements)
          unitFamilyList.AddRange((IEnumerable<UnitFamily>) this.GetSpecialAttackTargetsByElement(element.element));
        foreach (GearKindRatio gearKindRatio in ((IEnumerable<GearKindRatio>) MasterData.GearKindRatioList).Where<GearKindRatio>((Func<GearKindRatio, bool>) (x => x.kind.ID == this.kind.ID && (double) x.ratio > 1.0)))
          unitFamilyList.Add(gearKindRatio.family);
        return unitFamilyList.ToArray();
      }
    }

    public UnitFamily[] GetSpecialAttackTargetsByElement(CommonElement element)
    {
      List<UnitFamily> unitFamilyList = new List<UnitFamily>();
      foreach (GearElementRatio gearElementRatio in ((IEnumerable<GearElementRatio>) MasterData.GearElementRatioList).Where<GearElementRatio>((Func<GearElementRatio, bool>) (x => x.element == element && (double) x.ratio > 1.0)))
        unitFamilyList.Add(gearElementRatio.family);
      return unitFamilyList.ToArray();
    }

    public bool hasAttackClass
    {
      get
      {
        return this.gearClassification != null && this.gearClassification.attack_classification != GearAttackClassification.none;
      }
    }

    public GearClassificationPattern gearClassification
    {
      get
      {
        if (this.gearClassification_ != null)
          return this.gearClassification_;
        this.gearClassification_ = this.classification;
        if (this.gearClassification_ == null)
          this.gearClassification_ = this.classificationDefault;
        return this.gearClassification_;
      }
    }

    private GearClassificationPattern classificationDefault
    {
      get
      {
        return Array.Find<GearClassificationPattern>(MasterData.GearClassificationPatternList, (Predicate<GearClassificationPattern>) (x => x.kind_GearKind == this.kind_GearKind));
      }
    }

    public string attackClassificationName
    {
      get
      {
        GearClassificationPattern gearClassification = this.gearClassification;
        return gearClassification == null ? string.Empty : MasterData.GearAttackClassificationTable[gearClassification.attack_classification_GearAttackClassification].name;
      }
    }

    public CommonElement attachedElement
    {
      get
      {
        if (this.attachedElement_.HasValue)
          return this.attachedElement_.Value;
        BattleskillSkill attachedElementSkill = this.attachedElementSkill;
        return (this.attachedElement_ = new CommonElement?(attachedElementSkill != null ? attachedElementSkill.element : CommonElement.none)).Value;
      }
    }

    public BattleskillSkill attachedElementSkill => GearAttachedElement.GetSkillByGearID(this.ID);

    public GearGearSkill[] skills
    {
      get
      {
        List<GearGearSkill> gearGearSkillList = new List<GearGearSkill>();
        List<GearGearSkill> list = ((IEnumerable<GearGearSkill>) MasterData.GearGearSkillList).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.gear.ID == this.ID)).ToList<GearGearSkill>();
        if (list.Count > 0)
        {
          foreach (IGrouping<int, GearGearSkill> source in list.GroupBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.skill_group)))
            gearGearSkillList.Add(source.OrderBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.release_rank)).First<GearGearSkill>());
        }
        return gearGearSkillList.ToArray();
      }
    }

    public List<List<GearGearSkill>> rememberSkills
    {
      get
      {
        List<List<GearGearSkill>> rememberSkills = new List<List<GearGearSkill>>();
        List<GearGearSkill> list = ((IEnumerable<GearGearSkill>) MasterData.GearGearSkillList).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.gear.ID == this.ID)).ToList<GearGearSkill>();
        if (list.Count > 0)
        {
          foreach (IGrouping<int, GearGearSkill> source in list.GroupBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.skill_group)))
            rememberSkills.Add(new List<GearGearSkill>((IEnumerable<GearGearSkill>) source.OrderBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.release_rank))));
        }
        return rememberSkills;
      }
    }

    public List<List<GearReisouSkill>> rememberReisouSkills
    {
      get
      {
        List<List<GearReisouSkill>> rememberReisouSkills = new List<List<GearReisouSkill>>();
        List<GearReisouSkill> list = ((IEnumerable<GearReisouSkill>) MasterData.GearReisouSkillList).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (x => x.gear.ID == this.ID)).ToList<GearReisouSkill>();
        if (list.Count > 0)
        {
          foreach (IGrouping<int, GearReisouSkill> source in list.GroupBy<GearReisouSkill, int>((Func<GearReisouSkill, int>) (x => x.skill_group)))
            rememberReisouSkills.Add(new List<GearReisouSkill>((IEnumerable<GearReisouSkill>) source.OrderBy<GearReisouSkill, int>((Func<GearReisouSkill, int>) (x => x.release_rank))));
        }
        return rememberReisouSkills;
      }
    }

    public List<int> specificationOfEquipmentUnits
    {
      get
      {
        if (this._specificationOfEquipmentUnits == null)
          this._specificationOfEquipmentUnits = !this.specification_of_equipment_unit_group_id.HasValue ? new List<int>() : ((IEnumerable<GearSpecificationOfEquipmentUnit>) MasterData.GearSpecificationOfEquipmentUnitList).Where<GearSpecificationOfEquipmentUnit>((Func<GearSpecificationOfEquipmentUnit, bool>) (x => x.group_id == this.specification_of_equipment_unit_group_id.Value && !x.job_id.HasValue)).Select<GearSpecificationOfEquipmentUnit, int>((Func<GearSpecificationOfEquipmentUnit, int>) (x => x.unit_id)).ToList<int>();
        return this._specificationOfEquipmentUnits;
      }
    }

    public List<int> specificationOfEquipmentJobs
    {
      get
      {
        if (this._specificationOfEquipmentJobs == null)
          this._specificationOfEquipmentJobs = !this.specification_of_equipment_unit_group_id.HasValue ? new List<int>() : ((IEnumerable<GearSpecificationOfEquipmentUnit>) MasterData.GearSpecificationOfEquipmentUnitList).Where<GearSpecificationOfEquipmentUnit>((Func<GearSpecificationOfEquipmentUnit, bool>) (x => x.group_id == this.specification_of_equipment_unit_group_id.Value && x.job_id.HasValue && x.unit_id == 0)).Select<GearSpecificationOfEquipmentUnit, int>((Func<GearSpecificationOfEquipmentUnit, int>) (x => x.job_id.Value)).ToList<int>();
        return this._specificationOfEquipmentJobs;
      }
    }

    public Dictionary<int, int?> specificationOfEquipmentUnitJobs
    {
      get
      {
        if (this._specificationOfEquipmentUnitJobs == null)
          this._specificationOfEquipmentUnitJobs = !this.specification_of_equipment_unit_group_id.HasValue ? new Dictionary<int, int?>() : ((IEnumerable<GearSpecificationOfEquipmentUnit>) MasterData.GearSpecificationOfEquipmentUnitList).Where<GearSpecificationOfEquipmentUnit>((Func<GearSpecificationOfEquipmentUnit, bool>) (x => x.group_id == this.specification_of_equipment_unit_group_id.Value && x.unit_id != 0)).ToDictionary<GearSpecificationOfEquipmentUnit, int, int?>((Func<GearSpecificationOfEquipmentUnit, int>) (x => x.unit_id), (Func<GearSpecificationOfEquipmentUnit, int?>) (x => x.job_id));
        return this._specificationOfEquipmentUnitJobs;
      }
    }

    public bool hasSpecificationOfEquipmentUnits
    {
      get => this.specification_of_equipment_unit_group_id.HasValue;
    }

    public bool enableEquipmentUnit(PlayerUnit unit)
    {
      return unit.IsAllEquipUnit ? !((IEnumerable<IgnoreGear>) MasterData.IgnoreGearList).Any<IgnoreGear>((Func<IgnoreGear, bool>) (v => v.gear_id == this.ID)) : this.enableEquipmentUnitWithoutCheckAllEquipUnit(unit);
    }

    public bool enableEquipmentUnitWithoutCheckAllEquipUnit(PlayerUnit playerUnit)
    {
      if (!this.specification_of_equipment_unit_group_id.HasValue || this.specificationOfEquipmentUnits.Contains(playerUnit._unit) || this.specificationOfEquipmentJobs.Contains(playerUnit.job_id))
        return true;
      int? nullable1;
      if (this.specificationOfEquipmentUnitJobs.TryGetValue(playerUnit._unit, out nullable1))
      {
        int jobId = playerUnit.job_id;
        int? nullable2 = nullable1;
        int valueOrDefault = nullable2.GetValueOrDefault();
        if (jobId == valueOrDefault & nullable2.HasValue)
          return true;
      }
      return false;
    }

    public bool checkCanEquipByProficiency(
      PlayerUnit unit,
      PlayerUnitGearProficiency playerGearProficiency = null)
    {
      if (this.kind_GearKind == 10)
        return true;
      foreach (PlayerUnitGearProficiency gearProficiency in unit.gear_proficiencies)
      {
        if (gearProficiency.gear_kind_id == this.kind_GearKind)
        {
          playerGearProficiency = gearProficiency;
          break;
        }
      }
      UnitProficiency unitProficiency;
      return playerGearProficiency != null && MasterData.UnitProficiency.TryGetValue(this.rarity.index, out unitProficiency) && unitProficiency.gear_rarity <= playerGearProficiency.level;
    }

    public bool isEquipment(PlayerUnit unit)
    {
      if (unit.IsAllEquipUnit)
        return !((IEnumerable<IgnoreGear>) MasterData.IgnoreGearList).Any<IgnoreGear>((Func<IgnoreGear, bool>) (v => v.gear_id == this.ID));
      return this.enableEquipmentUnit(unit) && this.checkCanEquipByProficiency(unit);
    }

    public bool isMaterial()
    {
      return this.kind.Enum == GearKindEnum.smith || this.kind.Enum == GearKindEnum.drilling || this.kind.Enum == GearKindEnum.special_drilling || this.kind.Enum == GearKindEnum.sea_present || this.kind.Enum == GearKindEnum.call_gift;
    }

    public bool isCallGift() => this.kind.Enum == GearKindEnum.call_gift;

    public bool isReisou() => this.reisou_type != 0;

    public bool isHolyReisou() => this.reisou_type == GearReisouType.holy;

    public bool isChaosReisou() => this.reisou_type == GearReisouType.chaos;

    public bool isMythologyReisou() => this.reisou_type == GearReisouType.mythology;

    public bool isComposeManaSeed()
    {
      int num = this.disappearance_num.HasValue ? this.disappearance_num.Value : 0;
      return this.kind.Enum == GearKindEnum.accessories && this.disappearance_type_GearDisappearanceType == 1 && num >= 1;
    }

    public bool isManaSeed()
    {
      return this.kind.Enum == GearKindEnum.accessories && this.disappearance_type_GearDisappearanceType == 1;
    }

    public static int manaSeedRecoveryAmount
    {
      get
      {
        return !GearGear.manaSeedRecoveryAmount_.HasValue ? (GearGear.manaSeedRecoveryAmount_ = new int?(int.Parse(Consts.GetInstance().MANASEED_RECOVERY_AMOUNT))).Value : GearGear.manaSeedRecoveryAmount_.Value;
      }
    }

    public int manaSeedRecoveryLimit
    {
      get
      {
        return (this.disappearance_num.HasValue ? this.disappearance_num.Value : 0) * GearGear.manaSeedRecoveryAmount;
      }
    }

    public CommonElement GetElement()
    {
      CommonElement element = CommonElement.none;
      IEnumerable<GearGearSkill> source1 = ((IEnumerable<GearGearSkill>) MasterData.GearGearSkillList).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.gear.ID == this.ID)).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.release_rank <= 1));
      if (source1.Any<GearGearSkill>())
      {
        List<GearGearSkill> source2 = new List<GearGearSkill>();
        foreach (IGrouping<int, GearGearSkill> source3 in source1.GroupBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.skill_group)))
          source2.Add(source3.OrderBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.release_rank)).First<GearGearSkill>());
        IEnumerable<BattleskillSkill> source4 = source2.Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.effect_logic.Enum == BattleskillEffectLogicEnum.invest_element)))).Select<GearGearSkill, BattleskillSkill>((Func<GearGearSkill, BattleskillSkill>) (x => x.skill));
        if (source4.Any<BattleskillSkill>())
          element = source4.First<BattleskillSkill>().element;
      }
      return element;
    }

    public static bool CanSpecialDrill(GearGear baseGear, GearGear materialGear)
    {
      if (baseGear == null || materialGear == null || baseGear.disappearance_num.HasValue || materialGear.disappearance_num.HasValue || (!baseGear.kind.isEquip || !materialGear.kind.isEquip) && materialGear.kind.Enum != GearKindEnum.special_drilling)
        return false;
      if (materialGear.group_id == baseGear.group_id)
        return true;
      if (materialGear.kind.Enum != GearKindEnum.special_drilling)
        return false;
      return materialGear.special_drilling_kind == null || materialGear.special_drilling_kind.Enum == baseGear.kind.Enum;
    }

    public static GearGear Parse(MasterDataReader reader)
    {
      return new GearGear()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        gear_description_GearGearDescription = reader.ReadInt(),
        compose_parameter_GearGearComposeParameter = reader.ReadInt(),
        rarity_GearRarity = reader.ReadInt(),
        kind_GearKind = reader.ReadInt(),
        model_kind_GearModelKind = reader.ReadInt(),
        resource_reference_gear_id_GearGear = reader.ReadInt(),
        attack_type_GearAttackType = reader.ReadInt(),
        power = reader.ReadInt(),
        fix_damage = reader.ReadIntOrNull(),
        physical_defense = reader.ReadInt(),
        magic_defense = reader.ReadInt(),
        hit = reader.ReadInt(),
        critical = reader.ReadInt(),
        evasion = reader.ReadInt(),
        weight = reader.ReadInt(),
        min_range = reader.ReadInt(),
        max_range = reader.ReadInt(),
        hp_incremental = reader.ReadInt(),
        strength_incremental = reader.ReadInt(),
        vitality_incremental = reader.ReadInt(),
        intelligence_incremental = reader.ReadInt(),
        mind_incremental = reader.ReadInt(),
        agility_incremental = reader.ReadInt(),
        dexterity_incremental = reader.ReadInt(),
        lucky_incremental = reader.ReadInt(),
        disappearance_type_GearDisappearanceType = reader.ReadInt(),
        disappearance_num = reader.ReadIntOrNull(),
        heal = reader.ReadInt(),
        customize_flag = reader.ReadInt(),
        history_group_number = reader.ReadInt(),
        published_at = reader.ReadDateTime(),
        hp_incremental_ratio = reader.ReadFloat(),
        strength_incremental_ratio = reader.ReadFloat(),
        vitality_incremental_ratio = reader.ReadFloat(),
        intelligence_incremental_ratio = reader.ReadFloat(),
        mind_incremental_ratio = reader.ReadFloat(),
        agility_incremental_ratio = reader.ReadFloat(),
        dexterity_incremental_ratio = reader.ReadFloat(),
        lucky_incremental_ratio = reader.ReadFloat(),
        specification_of_equipment_unit_group_id = reader.ReadIntOrNull(),
        bullet_prefab_name = reader.ReadStringOrNull(true),
        shoot_ready_effect_prefab_name = reader.ReadStringOrNull(true),
        drilling_rate = reader.ReadFloat(),
        special_drilling_kind_GearKind = reader.ReadIntOrNull(),
        rank_incr_group = reader.ReadInt(),
        classification_GearClassificationPattern = reader.ReadIntOrNull(),
        expire_date_GearExpireDate = reader.ReadIntOrNull(),
        reisou_type_GearReisouType = reader.ReadInt(),
        drilling_exp_mythology_id = reader.ReadIntOrNull(),
        is_jingi = reader.ReadBool()
      };
    }

    public GearGearDescription gear_description
    {
      get
      {
        GearGearDescription gearDescription;
        if (!MasterData.GearGearDescription.TryGetValue(this.gear_description_GearGearDescription, out gearDescription))
          Debug.LogError((object) ("Key not Found: MasterData.GearGearDescription[" + (object) this.gear_description_GearGearDescription + "]"));
        return gearDescription;
      }
    }

    public GearGearComposeParameter compose_parameter
    {
      get
      {
        GearGearComposeParameter composeParameter;
        if (!MasterData.GearGearComposeParameter.TryGetValue(this.compose_parameter_GearGearComposeParameter, out composeParameter))
          Debug.LogError((object) ("Key not Found: MasterData.GearGearComposeParameter[" + (object) this.compose_parameter_GearGearComposeParameter + "]"));
        return composeParameter;
      }
    }

    public GearRarity rarity
    {
      get
      {
        GearRarity rarity;
        if (!MasterData.GearRarity.TryGetValue(this.rarity_GearRarity, out rarity))
          Debug.LogError((object) ("Key not Found: MasterData.GearRarity[" + (object) this.rarity_GearRarity + "]"));
        return rarity;
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

    public GearModelKind model_kind
    {
      get
      {
        GearModelKind modelKind;
        if (!MasterData.GearModelKind.TryGetValue(this.model_kind_GearModelKind, out modelKind))
          Debug.LogError((object) ("Key not Found: MasterData.GearModelKind[" + (object) this.model_kind_GearModelKind + "]"));
        return modelKind;
      }
    }

    public GearGear resource_reference_gear_id
    {
      get
      {
        GearGear resourceReferenceGearId;
        if (!MasterData.GearGear.TryGetValue(this.resource_reference_gear_id_GearGear, out resourceReferenceGearId))
          Debug.LogError((object) ("Key not Found: MasterData.GearGear[" + (object) this.resource_reference_gear_id_GearGear + "]"));
        return resourceReferenceGearId;
      }
    }

    public GearAttackType attack_type => (GearAttackType) this.attack_type_GearAttackType;

    public GearDisappearanceType disappearance_type
    {
      get
      {
        GearDisappearanceType disappearanceType;
        if (!MasterData.GearDisappearanceType.TryGetValue(this.disappearance_type_GearDisappearanceType, out disappearanceType))
          Debug.LogError((object) ("Key not Found: MasterData.GearDisappearanceType[" + (object) this.disappearance_type_GearDisappearanceType + "]"));
        return disappearanceType;
      }
    }

    public GearKind special_drilling_kind
    {
      get
      {
        if (!this.special_drilling_kind_GearKind.HasValue)
          return (GearKind) null;
        GearKind specialDrillingKind;
        if (!MasterData.GearKind.TryGetValue(this.special_drilling_kind_GearKind.Value, out specialDrillingKind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.special_drilling_kind_GearKind.Value + "]"));
        return specialDrillingKind;
      }
    }

    public GearClassificationPattern classification
    {
      get
      {
        if (!this.classification_GearClassificationPattern.HasValue)
          return (GearClassificationPattern) null;
        GearClassificationPattern classification;
        if (!MasterData.GearClassificationPattern.TryGetValue(this.classification_GearClassificationPattern.Value, out classification))
          Debug.LogError((object) ("Key not Found: MasterData.GearClassificationPattern[" + (object) this.classification_GearClassificationPattern.Value + "]"));
        return classification;
      }
    }

    public GearExpireDate expire_date
    {
      get
      {
        if (!this.expire_date_GearExpireDate.HasValue)
          return (GearExpireDate) null;
        GearExpireDate expireDate;
        if (!MasterData.GearExpireDate.TryGetValue(this.expire_date_GearExpireDate.Value, out expireDate))
          Debug.LogError((object) ("Key not Found: MasterData.GearExpireDate[" + (object) this.expire_date_GearExpireDate.Value + "]"));
        return expireDate;
      }
    }

    public GearReisouType reisou_type => (GearReisouType) this.reisou_type_GearReisouType;

    public List<string> ResourcePaths()
    {
      return new List<string>()
      {
        string.Format("AssetBundle/Resources/Gears/{0}/2D/weapon_thum", (object) this.resource_reference_gear_id.ID),
        string.Format("AssetBundle/Resources/Gears/{0}/2D/weapon_basic", (object) this.resource_reference_gear_id.ID),
        string.Format("Gears/{0}/3D/prefab", (object) this.resource_reference_gear_id.ID)
      };
    }

    public Future<Sprite> LoadSpriteThumbnail()
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Gears/{0}/2D/weapon_thum", (object) this.resource_reference_gear_id.ID));
    }

    public Future<Sprite> LoadSpriteBasic(float pixelsPerUnit = 1f)
    {
      return Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/Gears/{0}/2D/weapon_basic", (object) this.resource_reference_gear_id.ID), pixelsPerUnit);
    }

    public Future<GameObject> LoadModel()
    {
      return Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("Gears/{0}/3D/prefab", (object) this.resource_reference_gear_id.ID));
    }
  }
}
