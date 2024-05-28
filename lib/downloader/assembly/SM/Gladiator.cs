// Decompiled with JetBrains decompiler
// Type: SM.Gladiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class Gladiator : KeyCompare
  {
    public string name;
    public int player_level;
    public int leader_unit_job_id;
    public int total_power;
    public int rank_pt;
    public int matching_type;
    public string player_id;
    public int current_emblem_id;
    public int leader_unit_id;
    public int leader_unit_level;

    public Gladiator()
    {
    }

    public Gladiator(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.name = (string) json[nameof (name)];
      this.player_level = (int) (long) json[nameof (player_level)];
      this.leader_unit_job_id = (int) (long) json[nameof (leader_unit_job_id)];
      this.total_power = (int) (long) json[nameof (total_power)];
      this.rank_pt = (int) (long) json[nameof (rank_pt)];
      this.matching_type = (int) (long) json[nameof (matching_type)];
      this.player_id = (string) json[nameof (player_id)];
      this.current_emblem_id = (int) (long) json[nameof (current_emblem_id)];
      this.leader_unit_id = (int) (long) json[nameof (leader_unit_id)];
      this.leader_unit_level = (int) (long) json[nameof (leader_unit_level)];
    }
  }
}
