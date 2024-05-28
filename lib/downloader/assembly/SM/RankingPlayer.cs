// Decompiled with JetBrains decompiler
// Type: SM.RankingPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class RankingPlayer : KeyCompare
  {
    public int ranking;
    public string name;
    public int max_rank_pt;
    public int leader_unit_job_id;
    public int rank_pt;
    public int lose;
    public string player_id;
    public int current_emblem_id;
    public int leader_unit_id;
    public int win;
    public int leader_unit_level;

    public RankingPlayer()
    {
    }

    public RankingPlayer(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.ranking = (int) (long) json[nameof (ranking)];
      this.name = (string) json[nameof (name)];
      this.max_rank_pt = (int) (long) json[nameof (max_rank_pt)];
      this.leader_unit_job_id = (int) (long) json[nameof (leader_unit_job_id)];
      this.rank_pt = (int) (long) json[nameof (rank_pt)];
      this.lose = (int) (long) json[nameof (lose)];
      this.player_id = (string) json[nameof (player_id)];
      this.current_emblem_id = (int) (long) json[nameof (current_emblem_id)];
      this.leader_unit_id = (int) (long) json[nameof (leader_unit_id)];
      this.win = (int) (long) json[nameof (win)];
      this.leader_unit_level = (int) (long) json[nameof (leader_unit_level)];
    }
  }
}
