// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnityPureValueUpPattern
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
  public class UnityPureValueUpPattern
  {
    public int ID;
    public int material_unit_UnitUnit;
    public int? skill_BattleskillSkill;
    public int? target_same_character_id_UnitUnit;
    public int? group_large_category_UnitGroupLargeCategory;
    public int? group_small_category_UnitGroupSmallCategory;
    public string group_clothing_categories;
    public int? group_generation_category_UnitGroupGenerationCategory;
    private bool? wWildcard_;
    private UnitGroupClothingCategory[] wClothingCategories_;

    public static UnityPureValueUpPattern Parse(MasterDataReader reader)
    {
      return new UnityPureValueUpPattern()
      {
        ID = reader.ReadInt(),
        material_unit_UnitUnit = reader.ReadInt(),
        skill_BattleskillSkill = reader.ReadIntOrNull(),
        target_same_character_id_UnitUnit = reader.ReadIntOrNull(),
        group_large_category_UnitGroupLargeCategory = reader.ReadIntOrNull(),
        group_small_category_UnitGroupSmallCategory = reader.ReadIntOrNull(),
        group_clothing_categories = reader.ReadStringOrNull(true),
        group_generation_category_UnitGroupGenerationCategory = reader.ReadIntOrNull()
      };
    }

    public UnitUnit material_unit
    {
      get
      {
        UnitUnit materialUnit;
        if (!MasterData.UnitUnit.TryGetValue(this.material_unit_UnitUnit, out materialUnit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.material_unit_UnitUnit + "]"));
        return materialUnit;
      }
    }

    public BattleskillSkill skill
    {
      get
      {
        if (!this.skill_BattleskillSkill.HasValue)
          return (BattleskillSkill) null;
        BattleskillSkill skill;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_BattleskillSkill.Value, out skill))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_BattleskillSkill.Value + "]"));
        return skill;
      }
    }

    public UnitUnit target_same_character_id
    {
      get
      {
        if (!this.target_same_character_id_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit targetSameCharacterId;
        if (!MasterData.UnitUnit.TryGetValue(this.target_same_character_id_UnitUnit.Value, out targetSameCharacterId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.target_same_character_id_UnitUnit.Value + "]"));
        return targetSameCharacterId;
      }
    }

    public UnitGroupLargeCategory group_large_category
    {
      get
      {
        if (!this.group_large_category_UnitGroupLargeCategory.HasValue)
          return (UnitGroupLargeCategory) null;
        UnitGroupLargeCategory groupLargeCategory;
        if (!MasterData.UnitGroupLargeCategory.TryGetValue(this.group_large_category_UnitGroupLargeCategory.Value, out groupLargeCategory))
          Debug.LogError((object) ("Key not Found: MasterData.UnitGroupLargeCategory[" + (object) this.group_large_category_UnitGroupLargeCategory.Value + "]"));
        return groupLargeCategory;
      }
    }

    public UnitGroupSmallCategory group_small_category
    {
      get
      {
        if (!this.group_small_category_UnitGroupSmallCategory.HasValue)
          return (UnitGroupSmallCategory) null;
        UnitGroupSmallCategory groupSmallCategory;
        if (!MasterData.UnitGroupSmallCategory.TryGetValue(this.group_small_category_UnitGroupSmallCategory.Value, out groupSmallCategory))
          Debug.LogError((object) ("Key not Found: MasterData.UnitGroupSmallCategory[" + (object) this.group_small_category_UnitGroupSmallCategory.Value + "]"));
        return groupSmallCategory;
      }
    }

    public UnitGroupGenerationCategory group_generation_category
    {
      get
      {
        if (!this.group_generation_category_UnitGroupGenerationCategory.HasValue)
          return (UnitGroupGenerationCategory) null;
        UnitGroupGenerationCategory generationCategory;
        if (!MasterData.UnitGroupGenerationCategory.TryGetValue(this.group_generation_category_UnitGroupGenerationCategory.Value, out generationCategory))
          Debug.LogError((object) ("Key not Found: MasterData.UnitGroupGenerationCategory[" + (object) this.group_generation_category_UnitGroupGenerationCategory.Value + "]"));
        return generationCategory;
      }
    }

    private bool IsWildcard
    {
      get
      {
        if (this.wWildcard_.HasValue)
          return this.wWildcard_.Value;
        this.wWildcard_ = new bool?(!this.skill_BattleskillSkill.HasValue && !this.target_same_character_id_UnitUnit.HasValue && !this.group_large_category_UnitGroupLargeCategory.HasValue && !this.group_small_category_UnitGroupSmallCategory.HasValue && string.IsNullOrEmpty(this.group_clothing_categories) && !this.group_generation_category_UnitGroupGenerationCategory.HasValue);
        return this.wWildcard_.Value;
      }
    }

    public bool HasCheckUnitGroup
    {
      get
      {
        return this.group_large_category_UnitGroupLargeCategory.HasValue || this.group_small_category_UnitGroupSmallCategory.HasValue || !string.IsNullOrEmpty(this.group_clothing_categories) || this.group_generation_category_UnitGroupGenerationCategory.HasValue;
      }
    }

    public UnitGroupClothingCategory[] group_clothing_category
    {
      get
      {
        if (this.wClothingCategories_ != null)
          return this.wClothingCategories_;
        this.wClothingCategories_ = string.IsNullOrEmpty(this.group_clothing_categories) ? new UnitGroupClothingCategory[0] : UnityValueUpPattern.convStringToInts(this.group_clothing_categories).OrderBy<int, int>((Func<int, int>) (i => i)).Select<int, UnitGroupClothingCategory>((Func<int, UnitGroupClothingCategory>) (id => MasterData.UnitGroupClothingCategory[id])).ToArray<UnitGroupClothingCategory>();
        return this.wClothingCategories_;
      }
    }

    public bool checkUnityPureValueUP(UnitUnit target)
    {
      if (this.IsWildcard)
        return true;
      if (this.skill_BattleskillSkill.HasValue)
      {
        int skillId = this.skill_BattleskillSkill.Value;
        if (!((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Any<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_UnitUnit == target.ID && x.skill_BattleskillSkill == skillId)))
          return false;
      }
      if (this.target_same_character_id_UnitUnit.HasValue && this.target_same_character_id_UnitUnit.Value != target.same_character_id)
        return false;
      UnitGroup unitGroup = this.HasCheckUnitGroup ? Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (u => u.unit_id == target.ID)) : (UnitGroup) null;
      if (this.group_large_category_UnitGroupLargeCategory.HasValue && (unitGroup == null || this.group_large_category_UnitGroupLargeCategory.Value != unitGroup.group_large_category_id_UnitGroupLargeCategory) || this.group_small_category_UnitGroupSmallCategory.HasValue && (unitGroup == null || this.group_small_category_UnitGroupSmallCategory.Value != unitGroup.group_small_category_id_UnitGroupSmallCategory))
        return false;
      if (!string.IsNullOrEmpty(this.group_clothing_categories))
      {
        if (unitGroup == null)
          return false;
        int[] source = new int[2]
        {
          unitGroup.group_clothing_category_id_UnitGroupClothingCategory,
          unitGroup.group_clothing_category_id_2_UnitGroupClothingCategory
        };
        int[] array = ((IEnumerable<UnitGroupClothingCategory>) this.group_clothing_category).Select<UnitGroupClothingCategory, int>((Func<UnitGroupClothingCategory, int>) (g => g.ID)).ToArray<int>();
        if (array.Length > 1)
        {
          if (!((IEnumerable<int>) array).SequenceEqual<int>((IEnumerable<int>) ((IEnumerable<int>) source).OrderBy<int, int>((Func<int, int>) (i => i))))
            return false;
        }
        else if (!((IEnumerable<int>) source).Contains<int>(array[0]))
          return false;
      }
      return !this.group_generation_category_UnitGroupGenerationCategory.HasValue || unitGroup != null && this.group_generation_category_UnitGroupGenerationCategory.Value == unitGroup.group_generation_category_id_UnitGroupGenerationCategory;
    }
  }
}
