// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitGroup
  {
    public int ID;
    public int unit_id;
    public int group_large_category_id_UnitGroupLargeCategory;
    public int group_small_category_id_UnitGroupSmallCategory;
    public int group_clothing_category_id_UnitGroupClothingCategory;
    public int group_clothing_category_id_2_UnitGroupClothingCategory;
    public int group_generation_category_id_UnitGroupGenerationCategory;

    public static UnitGroup Parse(MasterDataReader reader)
    {
      return new UnitGroup()
      {
        ID = reader.ReadInt(),
        unit_id = reader.ReadInt(),
        group_large_category_id_UnitGroupLargeCategory = reader.ReadInt(),
        group_small_category_id_UnitGroupSmallCategory = reader.ReadInt(),
        group_clothing_category_id_UnitGroupClothingCategory = reader.ReadInt(),
        group_clothing_category_id_2_UnitGroupClothingCategory = reader.ReadInt(),
        group_generation_category_id_UnitGroupGenerationCategory = reader.ReadInt()
      };
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

    public UnitGroupClothingCategory group_clothing_category_id_2
    {
      get
      {
        UnitGroupClothingCategory clothingCategoryId2;
        if (!MasterData.UnitGroupClothingCategory.TryGetValue(this.group_clothing_category_id_2_UnitGroupClothingCategory, out clothingCategoryId2))
          Debug.LogError((object) ("Key not Found: MasterData.UnitGroupClothingCategory[" + (object) this.group_clothing_category_id_2_UnitGroupClothingCategory + "]"));
        return clothingCategoryId2;
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
