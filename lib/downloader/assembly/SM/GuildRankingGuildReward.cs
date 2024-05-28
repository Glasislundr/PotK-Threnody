// Decompiled with JetBrains decompiler
// Type: SM.GuildRankingGuildReward
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
  public class GuildRankingGuildReward : KeyCompare
  {
    public int reward_quantity;
    public int _reward_type;
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

    public GuildRankingGuildReward()
    {
    }

    public GuildRankingGuildReward(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.reward_quantity = (int) (long) json[nameof (reward_quantity)];
      this._reward_type = (int) (long) json[nameof (reward_type)];
      this.reward_id = (int) (long) json[nameof (reward_id)];
    }
  }
}
