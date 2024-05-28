// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitTransMigrateMemoryList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitTransMigrateMemoryList : KeyCompare
  {
    public PlayerUnitTransMigrateMemoryListTransmigrate_memory[] transmigrate_memory;

    public PlayerUnitTransMigrateMemoryList()
    {
    }

    public PlayerUnitTransMigrateMemoryList(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<PlayerUnitTransMigrateMemoryListTransmigrate_memory> transmigrateMemoryList = new List<PlayerUnitTransMigrateMemoryListTransmigrate_memory>();
      foreach (object json1 in (List<object>) json[nameof (transmigrate_memory)])
        transmigrateMemoryList.Add(json1 == null ? (PlayerUnitTransMigrateMemoryListTransmigrate_memory) null : new PlayerUnitTransMigrateMemoryListTransmigrate_memory((Dictionary<string, object>) json1));
      this.transmigrate_memory = transmigrateMemoryList.ToArray();
    }

    public static PlayerUnitTransMigrateMemoryList Current
    {
      get => SMManager.Get<PlayerUnitTransMigrateMemoryList>();
    }
  }
}
