// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitJob_abilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitJob_abilities : KeyCompare
  {
    public int job_ability_id;
    public int skill_id;
    public int level;
    private int? skill2_id_;
    private JobCharacteristics master_;

    public PlayerUnitJob_abilities()
    {
    }

    public PlayerUnitJob_abilities(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.job_ability_id = (int) (long) json[nameof (job_ability_id)];
      this.skill_id = (int) (long) json[nameof (skill_id)];
      this.level = (int) (long) json[nameof (level)];
    }

    public void resetJobAbilityID(int id)
    {
      if (this.job_ability_id == id)
        return;
      this.job_ability_id = id;
      this.skill2_id_ = new int?();
      this.master_ = (JobCharacteristics) null;
    }

    public int skill2_id
    {
      get
      {
        return !this.skill2_id_.HasValue ? (this.skill2_id_ = new int?(this.master != null ? (this.master.skill2_BattleskillSkill.HasValue ? this.master.skill2_BattleskillSkill.Value : 0) : 0)).Value : this.skill2_id_.Value;
      }
    }

    public BattleskillSkill skill
    {
      get
      {
        if (this.master != null)
          return this.master.skill;
        BattleskillSkill skill = (BattleskillSkill) null;
        if (this.skill_id != 0 && !MasterData.BattleskillSkill.TryGetValue(this.skill_id, out skill))
          Debug.LogError((object) string.Format("Not Found MasterData.BattleskillSkill[PlayerUnitJob_abilities.skill_id({0})]", (object) this.skill_id));
        return skill;
      }
    }

    public BattleskillSkill skill2 => this.master?.skill2;

    public JobCharacteristics master
    {
      get
      {
        JobCharacteristics jobCharacteristics;
        return this.master_ ?? (this.master_ = this.job_ability_id == 0 || !MasterData.JobCharacteristics.TryGetValue(this.job_ability_id, out jobCharacteristics) ? (JobCharacteristics) null : jobCharacteristics);
      }
    }

    public JobCharacteristicsLevelupPattern current_levelup_pattern
    {
      get
      {
        JobCharacteristicsLevelupPattern currentLevelupPattern = (JobCharacteristicsLevelupPattern) null;
        if (this.master != null && this.level >= 0 && this.master.levelup_patterns.Length > this.level)
          MasterData.JobCharacteristicsLevelupPattern.TryGetValue(this.master.levelup_patterns[this.level], out currentLevelupPattern);
        return currentLevelupPattern;
      }
    }

    public List<KeyValuePair<UnitUnit, int>> getLevelupMaterials(
      PlayerUnit playerUnit,
      int checkLevel = -1)
    {
      if (checkLevel < 0)
        checkLevel = this.level;
      if (playerUnit == (PlayerUnit) null || checkLevel < 0 || this.master == null || this.master.levelup_patterns.Length <= checkLevel)
        return (List<KeyValuePair<UnitUnit, int>>) null;
      JobCharacteristicsLevelupPattern characteristicsLevelupPattern;
      if (!MasterData.JobCharacteristicsLevelupPattern.TryGetValue(this.master.levelup_patterns[checkLevel], out characteristicsLevelupPattern))
        return (List<KeyValuePair<UnitUnit, int>>) null;
      JobMaterialGroup[] jobMaterialGroupArray = new JobMaterialGroup[5]
      {
        characteristicsLevelupPattern.material_group_id1,
        characteristicsLevelupPattern.material_group_id2,
        characteristicsLevelupPattern.material_group_id3,
        characteristicsLevelupPattern.material_group_id4,
        characteristicsLevelupPattern.material_group_id5
      };
      int?[] nullableArray = new int?[5]
      {
        characteristicsLevelupPattern.quantity1,
        characteristicsLevelupPattern.quantity2,
        characteristicsLevelupPattern.quantity3,
        characteristicsLevelupPattern.quantity4,
        characteristicsLevelupPattern.quantity5
      };
      List<KeyValuePair<UnitUnit, int>> levelupMaterials = new List<KeyValuePair<UnitUnit, int>>();
      UnitUnit unit1 = playerUnit.unit;
      int kindId = unit1.kind_GearKind;
      int elementId = unit1.GetElementSkillID();
      for (int index = 0; index < jobMaterialGroupArray.Length; ++index)
      {
        if (jobMaterialGroupArray[index] != null && nullableArray[index].HasValue)
        {
          JobMaterialUsed jobMaterialUsed = (JobMaterialUsed) null;
          JobMaterialGroup materialGroup = jobMaterialGroupArray[index];
          JobCheckItem? checkItemId = materialGroup.check_item_id;
          if (checkItemId.HasValue)
          {
            checkItemId = materialGroup.check_item_id;
            switch (checkItemId.Value)
            {
              case JobCheckItem.kind:
                jobMaterialUsed = Array.Find<JobMaterialUsed>(MasterData.JobMaterialUsedList, (Predicate<JobMaterialUsed>) (x =>
                {
                  if (x.material_group_id_JobMaterialGroup.HasValue)
                  {
                    int? nullable = x.material_group_id_JobMaterialGroup;
                    int id = materialGroup.ID;
                    if (nullable.GetValueOrDefault() == id & nullable.HasValue)
                    {
                      nullable = x.check_item_id;
                      int num = kindId;
                      return nullable.GetValueOrDefault() == num & nullable.HasValue;
                    }
                  }
                  return false;
                }));
                break;
              case JobCheckItem.element:
                jobMaterialUsed = Array.Find<JobMaterialUsed>(MasterData.JobMaterialUsedList, (Predicate<JobMaterialUsed>) (x =>
                {
                  if (x.material_group_id_JobMaterialGroup.HasValue)
                  {
                    int? nullable = x.material_group_id_JobMaterialGroup;
                    int id = materialGroup.ID;
                    if (nullable.GetValueOrDefault() == id & nullable.HasValue)
                    {
                      nullable = x.check_item_id;
                      int num = elementId;
                      return nullable.GetValueOrDefault() == num & nullable.HasValue;
                    }
                  }
                  return false;
                }));
                break;
              case JobCheckItem.population:
                foreach (UnitFamily family in playerUnit.Families)
                {
                  UnitFamily f = family;
                  jobMaterialUsed = Array.Find<JobMaterialUsed>(MasterData.JobMaterialUsedList, (Predicate<JobMaterialUsed>) (x =>
                  {
                    if (x.material_group_id_JobMaterialGroup.HasValue)
                    {
                      int? nullable = x.material_group_id_JobMaterialGroup;
                      int id = materialGroup.ID;
                      if (nullable.GetValueOrDefault() == id & nullable.HasValue)
                      {
                        nullable = x.check_item_id;
                        int num = (int) f;
                        return nullable.GetValueOrDefault() == num & nullable.HasValue;
                      }
                    }
                    return false;
                  }));
                  if (jobMaterialUsed != null)
                    break;
                }
                break;
            }
          }
          else
            jobMaterialUsed = Array.Find<JobMaterialUsed>(MasterData.JobMaterialUsedList, (Predicate<JobMaterialUsed>) (x =>
            {
              if (!x.material_group_id_JobMaterialGroup.HasValue)
                return false;
              int? jobMaterialGroup = x.material_group_id_JobMaterialGroup;
              int id = materialGroup.ID;
              return jobMaterialGroup.GetValueOrDefault() == id & jobMaterialGroup.HasValue;
            }));
          if (jobMaterialUsed != null)
          {
            UnitUnit unit2 = jobMaterialUsed.unit;
            if (unit2 != null)
              levelupMaterials.Add(new KeyValuePair<UnitUnit, int>(unit2, nullableArray[index].Value));
          }
        }
      }
      return levelupMaterials;
    }

    public string levelmaxBonusTypeText1
    {
      get => this.genLevelmaxBonusTypeText(this.master.levelmax_bonus);
    }

    public string levelmaxBonusTypeText2
    {
      get => this.genLevelmaxBonusTypeText(this.master.levelmax_bonus2);
    }

    public string levelmaxBonusTypeText3
    {
      get => this.genLevelmaxBonusTypeText(this.master.levelmax_bonus3);
    }

    private string genLevelmaxBonusTypeText(JobCharacteristicsLevelmaxBonus bonus)
    {
      switch (bonus)
      {
        case JobCharacteristicsLevelmaxBonus.hp_add:
          return "HP";
        case JobCharacteristicsLevelmaxBonus.strength_add:
          return "力";
        case JobCharacteristicsLevelmaxBonus.intelligence_add:
          return "魔";
        case JobCharacteristicsLevelmaxBonus.vitality_add:
          return "守";
        case JobCharacteristicsLevelmaxBonus.mind_add:
          return "精";
        case JobCharacteristicsLevelmaxBonus.agility_add:
          return "速";
        case JobCharacteristicsLevelmaxBonus.dexterity_add:
          return "技";
        case JobCharacteristicsLevelmaxBonus.lucky_add:
          return "運";
        case JobCharacteristicsLevelmaxBonus.movement_add:
          return "移動";
        default:
          return (string) null;
      }
    }
  }
}
