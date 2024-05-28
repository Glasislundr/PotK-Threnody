// Decompiled with JetBrains decompiler
// Type: SM.RaidPeriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class RaidPeriod : KeyCompare
  {
    public DateTime? start_at;
    public string name;
    public DateTime? entry_end_at;
    public bool is_endless;
    public DateTime? end_at;
    public int id;
    public DateTime? entry_start_at;

    public RaidPeriod()
    {
    }

    public RaidPeriod(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.start_at = json[nameof (start_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_at)]));
      this.name = json[nameof (name)] == null ? (string) null : (string) json[nameof (name)];
      this.entry_end_at = json[nameof (entry_end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (entry_end_at)]));
      this.is_endless = (bool) json[nameof (is_endless)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.id = (int) (long) json[nameof (id)];
      this.entry_start_at = json[nameof (entry_start_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (entry_start_at)]));
    }
  }
}
