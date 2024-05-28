// Decompiled with JetBrains decompiler
// Type: SM.ColosseumRecord
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class ColosseumRecord : KeyCompare
  {
    public int defence_lose;
    public int attack_max_consecutive_wins;
    public int total_win;
    public int enabled_max_rank;
    public int total_lose;
    public int defence_win;
    public int rank_pt;
    public int max_rank;
    public int attack_consecutive_wins;
    public int entry_count;
    public int defence_consecutive_wins;
    public int current_rank;
    public int defence_max_consecutive_wins;
    public int attack_lose;
    public int enabled_max_rank_point;
    public int current_emblem_id;
    public int attack_win;

    public ColosseumRecord()
    {
    }

    public ColosseumRecord(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.defence_lose = (int) (long) json[nameof (defence_lose)];
      this.attack_max_consecutive_wins = (int) (long) json[nameof (attack_max_consecutive_wins)];
      this.total_win = (int) (long) json[nameof (total_win)];
      this.enabled_max_rank = (int) (long) json[nameof (enabled_max_rank)];
      this.total_lose = (int) (long) json[nameof (total_lose)];
      this.defence_win = (int) (long) json[nameof (defence_win)];
      this.rank_pt = (int) (long) json[nameof (rank_pt)];
      this.max_rank = (int) (long) json[nameof (max_rank)];
      this.attack_consecutive_wins = (int) (long) json[nameof (attack_consecutive_wins)];
      this.entry_count = (int) (long) json[nameof (entry_count)];
      this.defence_consecutive_wins = (int) (long) json[nameof (defence_consecutive_wins)];
      this.current_rank = (int) (long) json[nameof (current_rank)];
      this.defence_max_consecutive_wins = (int) (long) json[nameof (defence_max_consecutive_wins)];
      this.attack_lose = (int) (long) json[nameof (attack_lose)];
      this.enabled_max_rank_point = (int) (long) json[nameof (enabled_max_rank_point)];
      this.current_emblem_id = (int) (long) json[nameof (current_emblem_id)];
      this.attack_win = (int) (long) json[nameof (attack_win)];
    }
  }
}
