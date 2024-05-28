// Decompiled with JetBrains decompiler
// Type: SM.GuildEventReward
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
  public class GuildEventReward : KeyCompare
  {
    public int reward_quantity;
    public int _reward_type;
    public int _event_type;
    public DateTime? created_at;
    public string guild_id;
    public int reward_id;

    public CommonGuildRewardType reward_type
    {
      get
      {
        if (!Enum.IsDefined(typeof (CommonGuildRewardType), (object) this._reward_type))
          Debug.LogError((object) ("Key not Found: MasterDataTable.CommonGuildRewardType[" + (object) this._reward_type + "]"));
        return (CommonGuildRewardType) this._reward_type;
      }
    }

    public GuildEventType event_type
    {
      get
      {
        if (!Enum.IsDefined(typeof (GuildEventType), (object) this._event_type))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GuildEventType[" + (object) this._event_type + "]"));
        return (GuildEventType) this._event_type;
      }
    }

    public GuildEventReward()
    {
    }

    public GuildEventReward(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.reward_quantity = (int) (long) json[nameof (reward_quantity)];
      this._reward_type = (int) (long) json[nameof (reward_type)];
      this._event_type = (int) (long) json[nameof (event_type)];
      this.created_at = json[nameof (created_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (created_at)]));
      this.guild_id = (string) json[nameof (guild_id)];
      this.reward_id = (int) (long) json[nameof (reward_id)];
    }
  }
}
