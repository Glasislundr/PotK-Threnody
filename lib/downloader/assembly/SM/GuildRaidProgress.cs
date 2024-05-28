// Decompiled with JetBrains decompiler
// Type: SM.GuildRaidProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildRaidProgress : KeyCompare
  {
    public int loop_count;
    public int boss_total_damage;
    public int quest_s_id;
    public int order;

    public GuildRaidProgress()
    {
    }

    public GuildRaidProgress(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.loop_count = (int) (long) json[nameof (loop_count)];
      this.boss_total_damage = (int) (long) json[nameof (boss_total_damage)];
      this.quest_s_id = (int) (long) json[nameof (quest_s_id)];
      this.order = (int) (long) json[nameof (order)];
    }
  }
}
