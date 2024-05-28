// Decompiled with JetBrains decompiler
// Type: SM.RankUpInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class RankUpInfo : KeyCompare
  {
    public RankUpInfoRank_up_rewards[] rank_up_rewards;
    public int after_rank_pt;
    public int rank_change;
    public int before_rank_pt;

    public RankUpInfo()
    {
    }

    public RankUpInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<RankUpInfoRank_up_rewards> infoRankUpRewardsList = new List<RankUpInfoRank_up_rewards>();
      foreach (object json1 in (List<object>) json[nameof (rank_up_rewards)])
        infoRankUpRewardsList.Add(json1 == null ? (RankUpInfoRank_up_rewards) null : new RankUpInfoRank_up_rewards((Dictionary<string, object>) json1));
      this.rank_up_rewards = infoRankUpRewardsList.ToArray();
      this.after_rank_pt = (int) (long) json[nameof (after_rank_pt)];
      this.rank_change = (int) (long) json[nameof (rank_change)];
      this.before_rank_pt = (int) (long) json[nameof (before_rank_pt)];
    }
  }
}
