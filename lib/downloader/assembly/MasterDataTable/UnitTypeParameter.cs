// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitTypeParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitTypeParameter
  {
    public int ID;
    public int unit_type_UnitType;
    public int rarity_UnitRarity;
    public float hp_levelup_max_correction;
    public float strength_levelup_max_correction;
    public float vitality_levelup_max_correction;
    public float intelligence_levelup_max_correction;
    public float mind_levelup_max_correction;
    public float agility_levelup_max_correction;
    public float dexterity_levelup_max_correction;
    public float lucky_levelup_max_correction;
    public int hp_compose_max;
    public int strength_compose_max;
    public int vitality_compose_max;
    public int intelligence_compose_max;
    public int mind_compose_max;
    public int agility_compose_max;
    public int dexterity_compose_max;
    public int lucky_compose_max;

    public static UnitTypeParameter Parse(MasterDataReader reader)
    {
      return new UnitTypeParameter()
      {
        ID = reader.ReadInt(),
        unit_type_UnitType = reader.ReadInt(),
        rarity_UnitRarity = reader.ReadInt(),
        hp_levelup_max_correction = reader.ReadFloat(),
        strength_levelup_max_correction = reader.ReadFloat(),
        vitality_levelup_max_correction = reader.ReadFloat(),
        intelligence_levelup_max_correction = reader.ReadFloat(),
        mind_levelup_max_correction = reader.ReadFloat(),
        agility_levelup_max_correction = reader.ReadFloat(),
        dexterity_levelup_max_correction = reader.ReadFloat(),
        lucky_levelup_max_correction = reader.ReadFloat(),
        hp_compose_max = reader.ReadInt(),
        strength_compose_max = reader.ReadInt(),
        vitality_compose_max = reader.ReadInt(),
        intelligence_compose_max = reader.ReadInt(),
        mind_compose_max = reader.ReadInt(),
        agility_compose_max = reader.ReadInt(),
        dexterity_compose_max = reader.ReadInt(),
        lucky_compose_max = reader.ReadInt()
      };
    }

    public UnitType unit_type
    {
      get
      {
        UnitType unitType;
        if (!MasterData.UnitType.TryGetValue(this.unit_type_UnitType, out unitType))
          Debug.LogError((object) ("Key not Found: MasterData.UnitType[" + (object) this.unit_type_UnitType + "]"));
        return unitType;
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
  }
}
