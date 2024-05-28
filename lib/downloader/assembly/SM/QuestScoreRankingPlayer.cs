// Decompiled with JetBrains decompiler
// Type: SM.QuestScoreRankingPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestScoreRankingPlayer : KeyCompare
  {
    public string name;
    public int leader_unit_job_id;
    public int rank;
    public int score;
    public string player_id;
    public int current_emblem_id;
    public int leader_unit_id;
    public int leader_unit_level;

    public QuestScoreRankingPlayer()
    {
    }

    public QuestScoreRankingPlayer(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.name = (string) json[nameof (name)];
      this.leader_unit_job_id = (int) (long) json[nameof (leader_unit_job_id)];
      this.rank = (int) (long) json[nameof (rank)];
      this.score = (int) (long) json[nameof (score)];
      this.player_id = (string) json[nameof (player_id)];
      this.current_emblem_id = (int) (long) json[nameof (current_emblem_id)];
      this.leader_unit_id = (int) (long) json[nameof (leader_unit_id)];
      this.leader_unit_level = (int) (long) json[nameof (leader_unit_level)];
    }
  }
}
