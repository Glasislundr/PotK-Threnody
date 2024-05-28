// Decompiled with JetBrains decompiler
// Type: SM.GuildLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildLog : KeyCompare
  {
    public bool is_deleted;
    public string log_id;
    public int kind_id;
    public string created_at;
    public GuildLogTransition transition;
    public string log_author_id;
    public string log_author_name;
    public string log_text;
    public int stamp_id;
    public int log_type;

    public GuildLog()
    {
    }

    public GuildLog(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.is_deleted = (bool) json[nameof (is_deleted)];
      this.log_id = (string) json[nameof (log_id)];
      this.kind_id = (int) (long) json[nameof (kind_id)];
      this.created_at = json[nameof (created_at)] == null ? (string) null : (string) json[nameof (created_at)];
      this.transition = json[nameof (transition)] == null ? (GuildLogTransition) null : new GuildLogTransition((Dictionary<string, object>) json[nameof (transition)]);
      this.log_author_id = json[nameof (log_author_id)] == null ? (string) null : (string) json[nameof (log_author_id)];
      this.log_author_name = json[nameof (log_author_name)] == null ? (string) null : (string) json[nameof (log_author_name)];
      this.log_text = (string) json[nameof (log_text)];
      this.stamp_id = (int) (long) json[nameof (stamp_id)];
      this.log_type = (int) (long) json[nameof (log_type)];
    }
  }
}
