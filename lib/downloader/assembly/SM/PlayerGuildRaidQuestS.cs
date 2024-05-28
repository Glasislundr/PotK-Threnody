// Decompiled with JetBrains decompiler
// Type: SM.PlayerGuildRaidQuestS
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
  public class PlayerGuildRaidQuestS : KeyCompare
  {
    public int loop_count;
    public int challenge_count;
    public int _quest_guildraid_s;

    public GuildRaid quest_guildraid_s
    {
      get
      {
        if (MasterData.GuildRaid.ContainsKey(this._quest_guildraid_s))
          return MasterData.GuildRaid[this._quest_guildraid_s];
        Debug.LogError((object) ("Key not Found: MasterData.GuildRaid[" + (object) this._quest_guildraid_s + "]"));
        return (GuildRaid) null;
      }
    }

    public PlayerGuildRaidQuestS()
    {
    }

    public PlayerGuildRaidQuestS(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.loop_count = (int) (long) json[nameof (loop_count)];
      this.challenge_count = (int) (long) json[nameof (challenge_count)];
      this._quest_guildraid_s = (int) (long) json[nameof (quest_guildraid_s)];
    }
  }
}
