// Decompiled with JetBrains decompiler
// Type: SM.PvPRankingPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PvPRankingPlayer : KeyCompare
  {
    public int ranking;
    public int total_win;
    public string name;
    public int current_class_id;
    public int current_rank_id;
    public string player_id;
    public int current_emblem_id;
    public int leader_unit_level;
    public int ranking_pt;
    public int leader_unit_id;
    public int leader_unit_job_id;

    public PvPRankingPlayer()
    {
    }

    public PvPRankingPlayer(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.ranking = (int) (long) json[nameof (ranking)];
      this.total_win = (int) (long) json[nameof (total_win)];
      this.name = (string) json[nameof (name)];
      this.current_class_id = (int) (long) json[nameof (current_class_id)];
      this.current_rank_id = (int) (long) json[nameof (current_rank_id)];
      this.player_id = (string) json[nameof (player_id)];
      this.current_emblem_id = (int) (long) json[nameof (current_emblem_id)];
      this.leader_unit_level = (int) (long) json[nameof (leader_unit_level)];
      this.ranking_pt = (int) (long) json[nameof (ranking_pt)];
      this.leader_unit_id = (int) (long) json[nameof (leader_unit_id)];
      this.leader_unit_job_id = (int) (long) json[nameof (leader_unit_job_id)];
    }
  }
}
