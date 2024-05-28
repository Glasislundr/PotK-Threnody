// Decompiled with JetBrains decompiler
// Type: MasterDataTable.UnitEvolutionPattern
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class UnitEvolutionPattern
  {
    public int ID;
    public int unit_UnitUnit;
    public int target_unit_UnitUnit;
    public int threshold_level;
    public int threshold_unity_value;
    public int money;

    public static UnitEvolutionPattern Parse(MasterDataReader reader)
    {
      return new UnitEvolutionPattern()
      {
        ID = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        target_unit_UnitUnit = reader.ReadInt(),
        threshold_level = reader.ReadInt(),
        threshold_unity_value = reader.ReadInt(),
        money = reader.ReadInt()
      };
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

    public UnitUnit target_unit
    {
      get
      {
        UnitUnit targetUnit;
        if (!MasterData.UnitUnit.TryGetValue(this.target_unit_UnitUnit, out targetUnit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.target_unit_UnitUnit + "]"));
        return targetUnit;
      }
    }

    public static UnitEvolutionPattern[] getGenealogy(int unitId)
    {
      LinkedList<UnitEvolutionPattern> linkedList = new LinkedList<UnitEvolutionPattern>();
      UnitUnit unitUnit;
      if (MasterData.UnitUnit.TryGetValue(unitId, out unitUnit) && unitUnit.IsNormalUnit)
      {
        UnitEvolutionPattern target = Array.Find<UnitEvolutionPattern>(MasterData.UnitEvolutionPatternList, (Predicate<UnitEvolutionPattern>) (x =>
        {
          if (x.unit_UnitUnit == unitId && x.target_unit.IsNormalUnit)
            return true;
          return x.target_unit_UnitUnit == unitId && x.unit.IsNormalUnit;
        }));
        if (target != null)
        {
          if (target.unit_UnitUnit == unitId)
          {
            UnitEvolutionPattern.pickupEvolutions(target, linkedList);
            UnitEvolutionPattern.pickupRegressions((UnitEvolutionPattern) null, linkedList, target.unit_UnitUnit);
          }
          else
          {
            UnitEvolutionPattern.pickupRegressions(target, linkedList);
            UnitEvolutionPattern.pickupEvolutions((UnitEvolutionPattern) null, linkedList, target.target_unit_UnitUnit);
          }
        }
      }
      return linkedList.ToArray<UnitEvolutionPattern>();
    }

    public static int[] getGenealogyIds(int unitId)
    {
      UnitEvolutionPattern[] genealogy = UnitEvolutionPattern.getGenealogy(unitId);
      if (genealogy.Length == 0)
        return new int[0];
      int[] genealogyIds = new int[genealogy.Length + 1];
      genealogyIds[0] = genealogy[0].unit_UnitUnit;
      for (int index = 0; index < genealogy.Length; ++index)
        genealogyIds[index + 1] = genealogy[index].target_unit_UnitUnit;
      return genealogyIds;
    }

    private static void pickupRegressions(
      UnitEvolutionPattern target,
      LinkedList<UnitEvolutionPattern> outlist,
      int targetId = 0)
    {
      UnitEvolutionPattern evolutionPattern = target;
      int id = targetId;
      do
      {
        if (evolutionPattern != null)
        {
          if (outlist.Contains(evolutionPattern))
            break;
          outlist.AddFirst(evolutionPattern);
          id = evolutionPattern.unit_UnitUnit;
        }
        evolutionPattern = Array.Find<UnitEvolutionPattern>(MasterData.UnitEvolutionPatternList, (Predicate<UnitEvolutionPattern>) (x => x.target_unit_UnitUnit == id && x.unit.IsNormalUnit));
      }
      while (evolutionPattern != null);
    }

    private static void pickupEvolutions(
      UnitEvolutionPattern target,
      LinkedList<UnitEvolutionPattern> outlist,
      int targetId = 0)
    {
      UnitEvolutionPattern evolutionPattern = target;
      int id = targetId;
      do
      {
        if (evolutionPattern != null)
        {
          if (outlist.Contains(evolutionPattern))
            break;
          outlist.AddLast(evolutionPattern);
          id = evolutionPattern.target_unit_UnitUnit;
        }
        evolutionPattern = Array.Find<UnitEvolutionPattern>(MasterData.UnitEvolutionPatternList, (Predicate<UnitEvolutionPattern>) (x => x.unit_UnitUnit == id && x.target_unit.IsNormalUnit));
      }
      while (evolutionPattern != null);
    }
  }
}
