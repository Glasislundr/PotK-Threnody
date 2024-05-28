// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitBreakThrough
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitBreakThrough
  {
    public int ID;
    public int material_unit_UnitUnit;
    public int? rarity_UnitRarity;
    public int? kind_GearKind;
    public int? target_unit_UnitUnit;
    public int? skill_id_BattleskillSkill;

    public static UnitBreakThrough Parse(MasterDataReader reader)
    {
      return new UnitBreakThrough()
      {
        ID = reader.ReadInt(),
        material_unit_UnitUnit = reader.ReadInt(),
        rarity_UnitRarity = reader.ReadIntOrNull(),
        kind_GearKind = reader.ReadIntOrNull(),
        target_unit_UnitUnit = reader.ReadIntOrNull(),
        skill_id_BattleskillSkill = reader.ReadIntOrNull()
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

    public BattleskillSkill skill_id
    {
      get
      {
        if (!this.skill_id_BattleskillSkill.HasValue)
          return (BattleskillSkill) null;
        BattleskillSkill skillId;
        if (!MasterData.BattleskillSkill.TryGetValue(this.skill_id_BattleskillSkill.Value, out skillId))
          Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) this.skill_id_BattleskillSkill.Value + "]"));
        return skillId;
      }
    }
  }
}
