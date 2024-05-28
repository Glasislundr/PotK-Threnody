// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitBuildupMaterialPattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitBuildupMaterialPattern
  {
    public int ID;
    public int material_unit_UnitUnit;
    public int? target_unit_UnitUnit;
    public int group_large_category_id_UnitGroupLargeCategory;
    public int group_small_category_id_UnitGroupSmallCategory;
    public int group_clothing_category_id_UnitGroupClothingCategory;

    public static UnitBuildupMaterialPattern Parse(MasterDataReader reader)
    {
      return new UnitBuildupMaterialPattern()
      {
        ID = reader.ReadInt(),
        material_unit_UnitUnit = reader.ReadInt(),
        target_unit_UnitUnit = reader.ReadIntOrNull(),
        group_large_category_id_UnitGroupLargeCategory = reader.ReadInt(),
        group_small_category_id_UnitGroupSmallCategory = reader.ReadInt(),
        group_clothing_category_id_UnitGroupClothingCategory = reader.ReadInt()
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
  }
}
