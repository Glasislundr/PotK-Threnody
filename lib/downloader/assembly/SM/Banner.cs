// Decompiled with JetBrains decompiler
// Type: SM.Banner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class Banner : KeyCompare
  {
    public int duration_seconds;
    public string url;
    public Transition transition;
    public DateTime? end_at;
    public int priority;
    public bool emphasis;
    public int id;
    public bool show_exp;

    public Banner()
    {
    }

    public Banner(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.duration_seconds = (int) (long) json[nameof (duration_seconds)];
      this.url = (string) json[nameof (url)];
      this.transition = json[nameof (transition)] == null ? (Transition) null : new Transition((Dictionary<string, object>) json[nameof (transition)]);
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.priority = (int) (long) json[nameof (priority)];
      this.emphasis = (bool) json[nameof (emphasis)];
      this.id = (int) (long) json[nameof (id)];
      this.show_exp = (bool) json[nameof (show_exp)];
    }
  }
}
