// Decompiled with JetBrains decompiler
// Type: SM.GuildApplicant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildApplicant : KeyCompare
  {
    public GuildPlayerInfo player;
    public DateTime applied_at;

    public GuildApplicant()
    {
    }

    public GuildApplicant(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.player = json[nameof (player)] == null ? (GuildPlayerInfo) null : new GuildPlayerInfo((Dictionary<string, object>) json[nameof (player)]);
      this.applied_at = DateTime.Parse((string) json[nameof (applied_at)]);
    }
  }
}
