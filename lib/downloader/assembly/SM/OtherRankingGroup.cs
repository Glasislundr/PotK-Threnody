// Decompiled with JetBrains decompiler
// Type: SM.OtherRankingGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class OtherRankingGroup : KeyCompare
  {
    public DateTime? finish_time;
    public DateTime? start_time;
    public int period_id;
    public DateTime? reward_receivable_period;
    public PvPRankingPlayer[] group_ranking;

    public OtherRankingGroup()
    {
    }

    public OtherRankingGroup(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.finish_time = json[nameof (finish_time)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (finish_time)]));
      this.start_time = json[nameof (start_time)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_time)]));
      this.period_id = (int) (long) json[nameof (period_id)];
      this.reward_receivable_period = json[nameof (reward_receivable_period)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (reward_receivable_period)]));
      List<PvPRankingPlayer> pvPrankingPlayerList = new List<PvPRankingPlayer>();
      foreach (object json1 in (List<object>) json[nameof (group_ranking)])
        pvPrankingPlayerList.Add(json1 == null ? (PvPRankingPlayer) null : new PvPRankingPlayer((Dictionary<string, object>) json1));
      this.group_ranking = pvPrankingPlayerList.ToArray();
    }
  }
}
