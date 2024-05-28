// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnityValueUpPattern
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
  public class UnityValueUpPattern
  {
    public int ID;
    public int material_unit_UnitUnit;
    public int? rarity_UnitRarity;
    public int? kind_GearKind;
    public string families;
    public int? skill_BattleskillSkill;
    public int? character_UnitCharacter;
    public int? unit_UnitUnit;
    public int? group_large_category_UnitGroupLargeCategory;
    public int? group_small_category_UnitGroupSmallCategory;
    public string group_clothing_categories;
    public int? group_generation_category_UnitGroupGenerationCategory;
    public float up_value;
    private bool? wWildcard_;
    private UnitGroupClothingCategory[] wClothingCategories_;
    private UnitFamily[] wFamilies_;

    public static UnityValueUpPattern Parse(MasterDataReader reader)
    {
      return new UnityValueUpPattern()
      {
        ID = reader.ReadInt(),
        material_unit_UnitUnit = reader.ReadInt(),
        rarity_UnitRarity = reader.ReadIntOrNull(),
        kind_GearKind = reader.ReadIntOrNull(),
        families = reader.ReadStringOrNull(true),
        skill_BattleskillSkill = reader.ReadIntOrNull(),
        character_UnitCharacter = reader.ReadIntOrNull(),
        unit_UnitUnit = reader.ReadIntOrNull(),
        group_large_category_UnitGroupLargeCategory = reader.ReadIntOrNull(),
        group_small_category_UnitGroupSmallCategory = reader.ReadIntOrNull(),
        group_clothing_categories = reader.ReadStringOrNull(true),
        group_generation_category_UnitGroupGenerationCategory = reader.ReadIntOrNull(),
        up_value = reader.ReadFloat()
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

    public UnitCharacter character
    {
      get
      {
        if (!this.character_UnitCharacter.HasValue)
          return (UnitCharacter) null;
        UnitCharacter character;
        if (!MasterData.UnitCharacter.TryGetValue(this.character_UnitCharacter.Value, out character))
          Debug.LogError((object) ("Key not Found: MasterData.UnitCharacter[" + (object) this.character_UnitCharacter.Value + "]"));
        return character;
      }
    }

    public UnitUnit unit
    {
      get
      {
        if (!this.unit_UnitUnit.HasValue)
          return (UnitUnit) null;
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit.Value, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit.Value + "]"));
        return unit;
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
        this.wWildcard_ = new bool?(!this.rarity_UnitRarity.HasValue && !this.kind_GearKind.HasValue && string.IsNullOrEmpty(this.families) && !this.skill_BattleskillSkill.HasValue && !this.character_UnitCharacter.HasValue && !this.unit_UnitUnit.HasValue && !this.group_large_category_UnitGroupLargeCategory.HasValue && !this.group_small_category_UnitGroupSmallCategory.HasValue && string.IsNullOrEmpty(this.group_clothing_categories) && !this.group_generation_category_UnitGroupGenerationCategory.HasValue);
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

    public UnitFamily[] family
    {
      get
      {
        if (this.wFamilies_ != null)
          return this.wFamilies_;
        this.wFamilies_ = string.IsNullOrEmpty(this.families) ? new UnitFamily[0] : UnityValueUpPattern.convStringToInts(this.families).OrderBy<int, int>((Func<int, int>) (i => i)).Select<int, UnitFamily>((Func<int, UnitFamily>) (e => (UnitFamily) e)).ToArray<UnitFamily>();
        return this.wFamilies_;
      }
    }

    public static List<int> convStringToInts(string src)
    {
      if (string.IsNullOrEmpty(src))
        return new List<int>();
      List<string> list = ((IEnumerable<string>) src.Split(',')).Select<string, string>((Func<string, string>) (s => s.Trim())).ToList<string>();
      List<int> ints = new List<int>(list.Count);
      foreach (string s in list)
      {
        double result;
        if (double.TryParse(s, out result))
        {
          int num = (int) Math.Floor(result);
          ints.Add(num);
        }
        else
          ints.Add(0);
      }
      return ints;
    }

    public bool checkUnityValueUP(UnitUnit target, Func<UnitFamily[]> funcGetFamilies)
    {
      if (this.IsWildcard)
        return true;
      if (this.rarity_UnitRarity.HasValue && this.rarity_UnitRarity.Value != target.rarity_UnitRarity || this.kind_GearKind.HasValue && this.kind_GearKind.Value != target.kind_GearKind || !string.IsNullOrEmpty(this.families) && !((IEnumerable<UnitFamily>) this.family).SequenceEqual<UnitFamily>((IEnumerable<UnitFamily>) (funcGetFamilies != null ? (IEnumerable<UnitFamily>) funcGetFamilies() : (IEnumerable<UnitFamily>) target.Families).OrderBy<UnitFamily, int>((Func<UnitFamily, int>) (e => (int) e))))
        return false;
      if (this.skill_BattleskillSkill.HasValue)
      {
        int skillId = this.skill_BattleskillSkill.Value;
        if (!((IEnumerable<UnitSkill>) MasterData.UnitSkillList).Any<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_UnitUnit == target.ID && x.skill_BattleskillSkill == skillId)))
          return false;
      }
      if (this.character_UnitCharacter.HasValue && this.character_UnitCharacter.Value != target.character_UnitCharacter || this.unit_UnitUnit.HasValue && this.unit_UnitUnit.Value != target.ID)
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
