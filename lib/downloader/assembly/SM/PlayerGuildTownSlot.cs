// Decompiled with JetBrains decompiler
// Type: SM.PlayerGuildTownSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerGuildTownSlot : KeyCompare
  {
    public PlayerGuildTownSlotPosition[] facilities_data;
    public int _master;
    public int id;
    public int slot_number;

    public MapTown master
    {
      get
      {
        if (MasterData.MapTown.ContainsKey(this._master))
          return MasterData.MapTown[this._master];
        Debug.LogError((object) ("Key not Found: MasterData.MapTown[" + (object) this._master + "]"));
        return (MapTown) null;
      }
    }

    public PlayerGuildTownSlot()
    {
    }

    public PlayerGuildTownSlot(Dictionary<string, object> json)
    {
      this._hasKey = true;
      List<PlayerGuildTownSlotPosition> townSlotPositionList = new List<PlayerGuildTownSlotPosition>();
      foreach (object json1 in (List<object>) json[nameof (facilities_data)])
        townSlotPositionList.Add(json1 == null ? (PlayerGuildTownSlotPosition) null : new PlayerGuildTownSlotPosition((Dictionary<string, object>) json1));
      this.facilities_data = townSlotPositionList.ToArray();
      this._master = (int) (long) json[nameof (master)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this.slot_number = (int) (long) json[nameof (slot_number)];
    }
  }
}
