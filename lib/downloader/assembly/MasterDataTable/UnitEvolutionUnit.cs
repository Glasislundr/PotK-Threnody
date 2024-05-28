// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitEvolutionUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitEvolutionUnit
  {
    public int ID;
    public int evolution_pattern_UnitEvolutionPattern;
    public int unit_UnitUnit;

    public static UnitEvolutionUnit Parse(MasterDataReader reader)
    {
      return new UnitEvolutionUnit()
      {
        ID = reader.ReadInt(),
        evolution_pattern_UnitEvolutionPattern = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt()
      };
    }

    public UnitEvolutionPattern evolution_pattern
    {
      get
      {
        UnitEvolutionPattern evolutionPattern;
        if (!MasterData.UnitEvolutionPattern.TryGetValue(this.evolution_pattern_UnitEvolutionPattern, out evolutionPattern))
          Debug.LogError((object) ("Key not Found: MasterData.UnitEvolutionPattern[" + (object) this.evolution_pattern_UnitEvolutionPattern + "]"));
        return evolutionPattern;
      }
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }
  }
}
