// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitTrustLevelMaterialPattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitTrustLevelMaterialPattern
  {
    public int ID;
    public int material_unit_UnitUnit;
    public int? rarity_UnitRarity;
    public int? kind_GearKind;
    public int? element_UnitFamily;
    public int? skill_BattleskillSkill;
    public int? target_character_UnitCharacter;
    public int? target_unit_UnitUnit;
    public int group_large_category_id_UnitGroupLargeCategory;
    public int group_small_category_id_UnitGroupSmallCategory;
    public int group_clothing_category_id_UnitGroupClothingCategory;
    public int group_generation_category_id_UnitGroupGenerationCategory;
    public float increase_value;

    public static UnitTrustLevelMaterialPattern Parse(MasterDataReader reader)
    {
      return new UnitTrustLevelMaterialPattern()
      {
        ID = reader.ReadInt(),
        material_unit_UnitUnit = reader.ReadInt(),
        rarity_UnitRarity = reader.ReadIntOrNull(),
        kind_GearKind = reader.ReadIntOrNull(),
        element_UnitFamily = reader.ReadIntOrNull(),
        skill_BattleskillSkill = reader.ReadIntOrNull(),
        target_character_UnitCharacter = reader.ReadIntOrNull(),
        target_unit_UnitUnit = reader.ReadIntOrNull(),
        group_large_category_id_UnitGroupLargeCategory = reader.ReadInt(),
        group_small_category_id_UnitGroupSmallCategory = reader.ReadInt(),
        group_clothing_category_id_UnitGroupClothingCategory = reader.ReadInt(),
        group_generation_category_id_UnitGroupGenerationCategory = reader.ReadInt(),
        increase_value = reader.ReadFloat()
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

    public UnitRarity rarity
    {
      get
      {
        if (!this.rarity_UnitRarity.HasValue)
          return (UnitRarity) null;
        UnitRarity rarity;
        if (!MasterData.UnitRarity.TryGetValue(this.rarity_UnitRarity.Value, out rarity))
          Debug.LogError((object) ("Key not Found: MasterData.UnitRarity[" + (object) this.rarity_UnitRarity.Value + "]"));
        return rarity;
      }
    }

    public GearKind kind
    {
      get
      {
        if (!this.kind_GearKind.HasValue)
          return (GearKind) null;
        GearKind kind;
        if (!MasterData.GearKind.TryGetValue(this.kind_GearKind.Value, out kind))
          Debug.LogError((object) ("Key not Found: MasterData.GearKind[" + (object) this.kind_GearKind.Value + "]"));
        return kind;
      }
    }

    public UnitFamily? element
    {
      get
      {
        int? elementUnitFamily = this.element_UnitFamily;
        return !elementUnitFamily.HasValue ? new UnitFamily?() : new UnitFamily?((UnitFamily) elementUnitFamily.GetValueOrDefault());
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

    public UnitCharacter target_character
    {
      get
      {
        if (!this.target_character_UnitCharacter.HasValue)
          return (UnitCharacter) null;
        UnitCharacter targetCharacter;
        if (!MasterData.UnitCharacter.TryGetValue(this.target_character_UnitCharacter.Value, out targetCharacter))
          Debug.LogError((object) ("Key not Found: MasterData.UnitCharacter[" + (object) this.target_character_UnitCharacter.Value + "]"));
        return targetCharacter;
      }
    }

    public UnitUnit target_unit
    {
      get
      {
        if (!this.target_unit_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit targetUnit;
        if (!MasterData.UnitUnit.TryGetValue(this.target_unit_UnitUnit.Value, out targetUnit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.target_unit_UnitUnit.Value + "]"));
        return targetUnit;
      }
    }

    public UnitGroupLargeCategory group_large_category_id
    {
      get
      {
        UnitGroupLargeCategory groupLargeCategoryId;
        if (!MasterData.UnitGroupLargeCategory.TryGetValue(this.group_large_category_id_UnitGroupLargeCategory, out groupLargeCategoryId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitGroupLargeCategory[" + (object) this.group_large_category_id_UnitGroupLargeCategory + "]"));
        return groupLargeCategoryId;
      }
    }

    public UnitGroupSmallCategory group_small_category_id
    {
      get
      {
        UnitGroupSmallCategory groupSmallCategoryId;
        if (!MasterData.UnitGroupSmallCategory.TryGetValue(this.group_small_category_id_UnitGroupSmallCategory, out groupSmallCategoryId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitGroupSmallCategory[" + (object) this.group_small_category_id_UnitGroupSmallCategory + "]"));
        return groupSmallCategoryId;
      }
    }

    public UnitGroupClothingCategory group_clothing_category_id
    {
      get
      {
        UnitGroupClothingCategory clothingCategoryId;
        if (!MasterData.UnitGroupClothingCategory.TryGetValue(this.group_clothing_category_id_UnitGroupClothingCategory, out clothingCategoryId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitGroupClothingCategory[" + (object) this.group_clothing_category_id_UnitGroupClothingCategory + "]"));
        return clothingCategoryId;
      }
    }

    public UnitGroupGenerationCategory group_generation_category_id
    {
      get
      {
        UnitGroupGenerationCategory generationCategoryId;
        if (!MasterData.UnitGroupGenerationCategory.TryGetValue(this.group_generation_category_id_UnitGroupGenerationCategory, out generationCategoryId))
          Debug.LogError((object) ("Key not Found: MasterData.UnitGroupGenerationCategory[" + (object) this.group_generation_category_id_UnitGroupGenerationCategory + "]"));
        return generationCategoryId;
      }
    }
  }
}
