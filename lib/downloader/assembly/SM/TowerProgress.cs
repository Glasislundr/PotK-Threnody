// Decompiled with JetBrains decompiler
// Type: SM.TowerProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class TowerProgress : KeyCompare
  {
    public int tower_id;
    public int floor;
    public int overkill_damage;
    public int completed_count;
    public int turn_count;
    public int period_id;
    public DateTime? recovered_at;
    public string player_id;
    public bool is_entry;
    public int unit_death_count;
    public TowerEnemy[] enemies;

    public TowerProgress()
    {
    }

    public TowerProgress(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.tower_id = (int) (long) json[nameof (tower_id)];
      this.floor = (int) (long) json[nameof (floor)];
      this.overkill_damage = (int) (long) json[nameof (overkill_damage)];
      this.completed_count = (int) (long) json[nameof (completed_count)];
      this.turn_count = (int) (long) json[nameof (turn_count)];
      this.period_id = (int) (long) json[nameof (period_id)];
      this.recovered_at = json[nameof (recovered_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (recovered_at)]));
      this.player_id = (string) json[nameof (player_id)];
      this.is_entry = (bool) json[nameof (is_entry)];
      this.unit_death_count = (int) (long) json[nameof (unit_death_count)];
      List<TowerEnemy> towerEnemyList = new List<TowerEnemy>();
      foreach (object json1 in (List<object>) json[nameof (enemies)])
        towerEnemyList.Add(json1 == null ? (TowerEnemy) null : new TowerEnemy((Dictionary<string, object>) json1));
      this.enemies = towerEnemyList.ToArray();
    }
  }
}
