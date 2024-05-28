// Decompiled with JetBrains decompiler
// Type: SM.GachaPeriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GachaPeriod : KeyCompare
  {
    public bool display_count_down;
    public DateTime? start_at;
    public DateTime? end_at;

    public GachaPeriod()
    {
    }

    public GachaPeriod(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.display_count_down = (bool) json[nameof (display_count_down)];
      this.start_at = json[nameof (start_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_at)]));
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
    }
  }
}
