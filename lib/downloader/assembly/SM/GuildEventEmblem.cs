// Decompiled with JetBrains decompiler
// Type: SM.GuildEventEmblem
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
  public class GuildEventEmblem : KeyCompare
  {
    public DateTime? created_at;
    public string guild_id;
    public int _event_type;
    public int _unit;

    public GuildEventType event_type
    {
      get
      {
        if (!Enum.IsDefined(typeof (GuildEventType), (object) this._event_type))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GuildEventType[" + (object) this._event_type + "]"));
        return (GuildEventType) this._event_type;
      }
    }

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

    public GuildEventEmblem()
    {
    }

    public GuildEventEmblem(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.created_at = json[nameof (created_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (created_at)]));
      this.guild_id = (string) json[nameof (guild_id)];
      this._event_type = (int) (long) json[nameof (event_type)];
      this._unit = (int) (long) json[nameof (unit)];
    }
  }
}
