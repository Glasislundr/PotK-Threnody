// Decompiled with JetBrains decompiler
// Type: SM.PlayerTransmigrateMemoryPlayerUnitIds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerTransmigrateMemoryPlayerUnitIds : KeyCompare
  {
    public int?[] transmigrate_memory_player_unit_ids;

    public PlayerTransmigrateMemoryPlayerUnitIds()
    {
    }

    public PlayerTransmigrateMemoryPlayerUnitIds(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.transmigrate_memory_player_unit_ids = ((IEnumerable<object>) json[nameof (transmigrate_memory_player_unit_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
    }

    public static PlayerTransmigrateMemoryPlayerUnitIds Current
    {
      get => SMManager.Get<PlayerTransmigrateMemoryPlayerUnitIds>();
    }

    public void AddMemoryData(PlayerUnit playerUnit)
    {
      PlayerUnitTransMigrateMemoryListTransmigrate_memory[] self = PlayerUnitTransMigrateMemoryList.Current != null ? PlayerUnitTransMigrateMemoryList.Current.transmigrate_memory : new PlayerUnitTransMigrateMemoryListTransmigrate_memory[0];
      int? nullable = ((IEnumerable<PlayerUnitTransMigrateMemoryListTransmigrate_memory>) self).FirstIndexOrNull<PlayerUnitTransMigrateMemoryListTransmigrate_memory>((Func<PlayerUnitTransMigrateMemoryListTransmigrate_memory, bool>) (x => x.player_unit_id == playerUnit.id));
      if (!nullable.HasValue)
        return;
      playerUnit.SetMemoryData(self[nullable.Value]);
    }

    public List<PlayerUnit> PlayerUnits()
    {
      PlayerUnit[] self1 = SMManager.Get<PlayerUnit[]>();
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      PlayerUnitTransMigrateMemoryListTransmigrate_memory[] self2 = PlayerUnitTransMigrateMemoryList.Current != null ? PlayerUnitTransMigrateMemoryList.Current.transmigrate_memory : new PlayerUnitTransMigrateMemoryListTransmigrate_memory[0];
      for (int index = 0; index < this.transmigrate_memory_player_unit_ids.Length; ++index)
      {
        int? x = this.transmigrate_memory_player_unit_ids[index];
        int? nullable1 = ((IEnumerable<PlayerUnit>) self1).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (y =>
        {
          int id = y.id;
          int? nullable2 = x;
          int valueOrDefault = nullable2.GetValueOrDefault();
          return id == valueOrDefault & nullable2.HasValue;
        }));
        int? nullable3 = ((IEnumerable<PlayerUnitTransMigrateMemoryListTransmigrate_memory>) self2).FirstIndexOrNull<PlayerUnitTransMigrateMemoryListTransmigrate_memory>((Func<PlayerUnitTransMigrateMemoryListTransmigrate_memory, bool>) (y =>
        {
          int playerUnitId = y.player_unit_id;
          int? nullable4 = x;
          int valueOrDefault = nullable4.GetValueOrDefault();
          return playerUnitId == valueOrDefault & nullable4.HasValue;
        }));
        if (nullable1.HasValue && nullable3.HasValue)
        {
          self1[nullable1.Value].SetMemoryData(self2[nullable3.Value]);
          playerUnitList.Add(self1[nullable1.Value]);
        }
        else
          playerUnitList.Add((PlayerUnit) null);
      }
      return playerUnitList;
    }
  }
}
