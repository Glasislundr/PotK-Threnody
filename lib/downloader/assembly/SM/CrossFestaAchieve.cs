// Decompiled with JetBrains decompiler
// Type: SM.CrossFestaAchieve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class CrossFestaAchieve : KeyCompare
  {
    public DateTime? start_at;
    public string description;
    public string thumbnail;
    public int beyond_achieve_id;
    public DateTime? end_at;
    public int id;

    public CrossFestaAchieve()
    {
    }

    public CrossFestaAchieve(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.start_at = json[nameof (start_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_at)]));
      this.description = (string) json[nameof (description)];
      this.thumbnail = (string) json[nameof (thumbnail)];
      this.beyond_achieve_id = (int) (long) json[nameof (beyond_achieve_id)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
