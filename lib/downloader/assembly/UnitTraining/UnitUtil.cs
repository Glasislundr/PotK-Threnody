// Decompiled with JetBrains decompiler
// Type: UnitTraining.UnitUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace UnitTraining
{
  public static class UnitUtil
  {
    public const int MAX_CELL_COUNT = 30;
    public const int UNSET_SKILL = 0;
    private const int TYRHUNG_AWAKE = 101414;

    public static bool checkAnyInheritance(PlayerUnit target)
    {
      return target.dexterity.inheritance != 0 || target.agility.inheritance != 0 || target.mind.inheritance != 0 || target.strength.inheritance != 0 || target.vitality.inheritance != 0 || target.hp.inheritance != 0 || target.intelligence.inheritance != 0 || target.lucky.inheritance != 0;
    }

    public static EvolutionType getEvolutionType(
      UnitUnit target,
      ref List<UnitEvolutionPattern> result)
    {
      UnitEvolutionPattern[] evolutionPattern = target.IsEvolution ? target.EvolutionPattern : (UnitEvolutionPattern[]) null;
      if (evolutionPattern == null || evolutionPattern.Length == 0)
        return EvolutionType.Limit;
      if (result != null)
        result.AddRange((IEnumerable<UnitEvolutionPattern>) evolutionPattern);
      if (!target.CanAwakeUnitFlag)
        return EvolutionType.Normal;
      return target.ID == 101414 ? EvolutionType.Awake : EvolutionType.CommonAwake;
    }

    public static bool checkReincarnation(PlayerUnit unit, bool bCheckLevel = true, bool bCheckMemory = true)
    {
      bool flag = bCheckLevel && unit.level >= unit.unit.rarity.reincarnation_level;
      if (!flag & bCheckMemory)
      {
        PlayerTransmigrateMemoryPlayerUnitIds current = PlayerTransmigrateMemoryPlayerUnitIds.Current;
        if (current != null && current.transmigrate_memory_player_unit_ids != null)
          flag = ((IEnumerable<int?>) current.transmigrate_memory_player_unit_ids).Any<int?>((Func<int?, bool>) (x =>
          {
            if (!x.HasValue)
              return false;
            int id = unit.id;
            int? nullable = x;
            int valueOrDefault = nullable.GetValueOrDefault();
            return id == valueOrDefault & nullable.HasValue;
          }));
      }
      return flag;
    }
  }
}
