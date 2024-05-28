// Decompiled with JetBrains decompiler
// Type: SM.RankingGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class RankingGroup : KeyCompare
  {
    public PvPRankingPlayer[] high_group_ranking;
    public DateTime? finish_time;
    public DateTime? start_time;
    public PvPRankingPlayer my_ranking;
    public int period_id;
    public PvPRankingPlayer[] top_group_ranking;
    public DateTime? reward_receivable_period;

    public RankingGroup()
    {
    }

    public RankingGroup(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<PvPRankingPlayer> pvPrankingPlayerList1 = new List<PvPRankingPlayer>();
      foreach (object json1 in (List<object>) json[nameof (high_group_ranking)])
        pvPrankingPlayerList1.Add(json1 == null ? (PvPRankingPlayer) null : new PvPRankingPlayer((Dictionary<string, object>) json1));
      this.high_group_ranking = pvPrankingPlayerList1.ToArray();
      this.finish_time = json[nameof (finish_time)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (finish_time)]));
      this.start_time = json[nameof (start_time)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_time)]));
      this.my_ranking = json[nameof (my_ranking)] == null ? (PvPRankingPlayer) null : new PvPRankingPlayer((Dictionary<string, object>) json[nameof (my_ranking)]);
      this.period_id = (int) (long) json[nameof (period_id)];
      List<PvPRankingPlayer> pvPrankingPlayerList2 = new List<PvPRankingPlayer>();
      foreach (object json2 in (List<object>) json[nameof (top_group_ranking)])
        pvPrankingPlayerList2.Add(json2 == null ? (PvPRankingPlayer) null : new PvPRankingPlayer((Dictionary<string, object>) json2));
      this.top_group_ranking = pvPrankingPlayerList2.ToArray();
      this.reward_receivable_period = json[nameof (reward_receivable_period)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (reward_receivable_period)]));
    }
  }
}
