// Decompiled with JetBrains decompiler
// Type: SM.PlayerGuildFacility
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
  public class PlayerGuildFacility : KeyCompare
  {
    public int _unit;
    public int _master;
    public int id;
    public int hasnum;
    public int level;

    public UnitUnit unit
    {
      get
      {
        if (MasterData.UnitUnit.ContainsKey(this._unit))
          return MasterData.UnitUnit[this._unit];
        Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this._unit + "]"));
        return (UnitUnit) null;
      }
    }

    public MapFacility master
    {
      get
      {
        if (MasterData.MapFacility.ContainsKey(this._master))
          return MasterData.MapFacility[this._master];
        Debug.LogError((object) ("Key not Found: MasterData.MapFacility[" + (object) this._master + "]"));
        return (MapFacility) null;
      }
    }

    public PlayerGuildFacility()
    {
    }

    public PlayerGuildFacility(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this._unit = (int) (long) json[nameof (unit)];
      this._master = (int) (long) json[nameof (master)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this.hasnum = (int) (long) json[nameof (hasnum)];
      this.level = (int) (long) json[nameof (level)];
    }
  }
}
