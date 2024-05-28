// Decompiled with JetBrains decompiler
// Type: OverkillersUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public static class OverkillersUtil
{
  public static bool checkDelete(PlayerUnit[] units)
  {
    for (int index = 0; index < units.Length; ++index)
    {
      if (!(units[index] == (PlayerUnit) null) && (units[index].isAnyOverkillersUnits || units[index].overkillers_base_id > 0))
        return false;
    }
    return true;
  }

  public static bool checkDelete(PlayerUnit unit)
  {
    return !unit.isAnyOverkillersUnits && unit.overkillers_base_id <= 0;
  }

  public static HashSet<int> checkCompletedDeck(
    PlayerUnit[] units,
    out bool bCompleted,
    HashSet<int> excludeIds = null,
    bool[] ignores = null)
  {
    bCompleted = true;
    if (ignores == null)
      ignores = Enumerable.Repeat<bool>(false, units.Length).ToArray<bool>();
    else if (units.Length > ignores.Length)
      ignores = ((IEnumerable<bool>) ignores).Concat<bool>(Enumerable.Repeat<bool>(false, units.Length - ignores.Length)).ToArray<bool>();
    if (excludeIds == null)
    {
      excludeIds = new HashSet<int>();
      for (int index1 = 0; index1 < units.Length; ++index1)
      {
        if (!ignores[index1])
        {
          PlayerUnit unit = units[index1];
          if (!(unit == (PlayerUnit) null) && unit.over_killers_player_unit_ids != null && unit.over_killers_player_unit_ids.Length != 0)
          {
            bool flag = true;
            for (int index2 = 0; index2 < unit.over_killers_player_unit_ids.Length && unit.over_killers_player_unit_ids[index2] >= 0; ++index2)
            {
              if (unit.over_killers_player_unit_ids[index2] > 0)
              {
                flag = false;
                excludeIds.Add(unit.over_killers_player_unit_ids[index2]);
              }
            }
            if (flag)
            {
              int overkillersBaseId = unit.overkillers_base_id;
              if (overkillersBaseId > 0)
                excludeIds.Add(overkillersBaseId);
            }
          }
        }
      }
    }
    for (int index = 0; index < units.Length; ++index)
    {
      if (!ignores[index] && units[index] != (PlayerUnit) null && excludeIds.Contains(units[index].id))
      {
        bCompleted = false;
        break;
      }
    }
    return excludeIds;
  }

  public static HashSet<int> checkCompletedCustomDeck(
    PlayerUnit[] units,
    out bool bCompleted,
    HashSet<int> excludeIds = null,
    bool[] ignores = null)
  {
    bCompleted = true;
    if (ignores == null)
      ignores = Enumerable.Repeat<bool>(false, units.Length).ToArray<bool>();
    else if (units.Length > ignores.Length)
      ignores = ((IEnumerable<bool>) ignores).Concat<bool>(Enumerable.Repeat<bool>(false, units.Length - ignores.Length)).ToArray<bool>();
    if (excludeIds == null)
    {
      excludeIds = new HashSet<int>();
      for (int index1 = 0; index1 < units.Length; ++index1)
      {
        if (!ignores[index1])
        {
          PlayerUnit unit = units[index1];
          if (!(unit == (PlayerUnit) null) && unit.cache_overkillers_units != null && unit.cache_overkillers_units.Length != 0)
          {
            for (int index2 = 0; index2 < unit.cache_overkillers_units.Length; ++index2)
            {
              if (unit.cache_overkillers_units[index2] != (PlayerUnit) null)
                excludeIds.Add(unit.cache_overkillers_units[index2].id);
            }
          }
        }
      }
    }
    for (int index = 0; index < units.Length; ++index)
    {
      if (!ignores[index] && units[index] != (PlayerUnit) null && excludeIds.Contains(units[index].id))
      {
        bCompleted = false;
        break;
      }
    }
    return excludeIds;
  }

  public static List<int> createEquipedUnitIDList()
  {
    List<int> equipedUnitIdList = new List<int>();
    ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
    {
      if (playerUnit.over_killers_player_unit_ids != null)
      {
        foreach (int killersPlayerUnitId in playerUnit.over_killers_player_unit_ids)
        {
          if (killersPlayerUnitId > 0)
          {
            equipedUnitIdList.Add(killersPlayerUnitId);
            break;
          }
        }
      }
    }
    return equipedUnitIdList;
  }
}
