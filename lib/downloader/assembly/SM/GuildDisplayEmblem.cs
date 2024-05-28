// Decompiled with JetBrains decompiler
// Type: SM.GuildDisplayEmblem
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
  public class GuildDisplayEmblem : KeyCompare
  {
    public bool is_enabled;
    public DateTime? created_at;
    public int _unit;
    public bool in_use;

    public GuildEmblemUnit unit
    {
      get
      {
        if (MasterData.GuildEmblemUnit.ContainsKey(this._unit))
          return MasterData.GuildEmblemUnit[this._unit];
        Debug.LogError((object) ("Key not Found: MasterData.GuildEmblemUnit[" + (object) this._unit + "]"));
        return (GuildEmblemUnit) null;
      }
    }

    public GuildDisplayEmblem()
    {
    }

    public GuildDisplayEmblem(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.is_enabled = (bool) json[nameof (is_enabled)];
      this.created_at = json[nameof (created_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (created_at)]));
      this._unit = (int) (long) json[nameof (unit)];
      this.in_use = (bool) json[nameof (in_use)];
    }
  }
}
