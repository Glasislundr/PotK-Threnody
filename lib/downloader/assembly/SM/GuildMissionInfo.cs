// Decompiled with JetBrains decompiler
// Type: SM.GuildMissionInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildMissionInfo : KeyCompare
  {
    public int count;
    public string achieved_guild_id;
    public int mission_id;
    public int guild_count;
    public DateTime? guild_achieved_at;
    public GuildMissionReward[] guild_rewards;
    public int received_count;

    public GuildMissionInfo()
    {
    }

    public GuildMissionInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.count = (int) (long) json[nameof (count)];
      this.achieved_guild_id = json[nameof (achieved_guild_id)] == null ? (string) null : (string) json[nameof (achieved_guild_id)];
      this.mission_id = (int) (long) json[nameof (mission_id)];
      this.guild_count = (int) (long) json[nameof (guild_count)];
      this.guild_achieved_at = json[nameof (guild_achieved_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (guild_achieved_at)]));
      List<GuildMissionReward> guildMissionRewardList = new List<GuildMissionReward>();
      foreach (object json1 in (List<object>) json[nameof (guild_rewards)])
        guildMissionRewardList.Add(json1 == null ? (GuildMissionReward) null : new GuildMissionReward((Dictionary<string, object>) json1));
      this.guild_rewards = guildMissionRewardList.ToArray();
      this.received_count = (int) (long) json[nameof (received_count)];
    }
  }
}
