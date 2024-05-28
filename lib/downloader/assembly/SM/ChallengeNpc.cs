// Decompiled with JetBrains decompiler
// Type: SM.ChallengeNpc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class ChallengeNpc : KeyCompare
  {
    public string name;
    public int player_level;
    public int leader_unit_job_id;
    public int total_power;
    public int win_count;
    public string player_id;
    public int lose_count;
    public int leader_unit_id;
    public int leader_unit_level;

    public ChallengeNpc()
    {
    }

    public ChallengeNpc(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.name = (string) json[nameof (name)];
      this.player_level = (int) (long) json[nameof (player_level)];
      this.leader_unit_job_id = (int) (long) json[nameof (leader_unit_job_id)];
      this.total_power = (int) (long) json[nameof (total_power)];
      this.win_count = (int) (long) json[nameof (win_count)];
      this.player_id = (string) json[nameof (player_id)];
      this.lose_count = (int) (long) json[nameof (lose_count)];
      this.leader_unit_id = (int) (long) json[nameof (leader_unit_id)];
      this.leader_unit_level = (int) (long) json[nameof (leader_unit_level)];
    }
  }
}
