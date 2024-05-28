// Decompiled with JetBrains decompiler
// Type: SM.ExploreProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class ExploreProgress : KeyCompare
  {
    public string count;
    public int last_damage;
    public int defeat_count;
    public int head_floor_id;
    public DateTime? timestamp;
    public int floor_id;
    public int win_count;
    public int state;
    public int? seed;
    public int takeover_time;
    public int[] box_reward_ids;
    public int progress;
    public int lose_count;
    public int? last_enemy_id;
    public int waiting_time;

    public ExploreProgress()
    {
    }

    public ExploreProgress(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.count = (string) json[nameof (count)];
      this.last_damage = (int) (long) json[nameof (last_damage)];
      this.defeat_count = (int) (long) json[nameof (defeat_count)];
      this.head_floor_id = (int) (long) json[nameof (head_floor_id)];
      this.timestamp = json[nameof (timestamp)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (timestamp)]));
      this.floor_id = (int) (long) json[nameof (floor_id)];
      this.win_count = (int) (long) json[nameof (win_count)];
      this.state = (int) (long) json[nameof (state)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (seed)] != null)
      {
        nullable1 = (long?) json[nameof (seed)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.seed = nullable2;
      this.takeover_time = (int) (long) json[nameof (takeover_time)];
      this.box_reward_ids = ((IEnumerable<object>) json[nameof (box_reward_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.progress = (int) (long) json[nameof (progress)];
      this.lose_count = (int) (long) json[nameof (lose_count)];
      int? nullable3;
      if (json[nameof (last_enemy_id)] != null)
      {
        nullable1 = (long?) json[nameof (last_enemy_id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.last_enemy_id = nullable3;
      this.waiting_time = (int) (long) json[nameof (waiting_time)];
    }
  }
}
