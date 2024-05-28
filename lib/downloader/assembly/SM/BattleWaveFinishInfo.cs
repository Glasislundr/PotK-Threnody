// Decompiled with JetBrains decompiler
// Type: SM.BattleWaveFinishInfo
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
  public class BattleWaveFinishInfo : KeyCompare
  {
    public int stage_id;
    public int[] enemy_results_enemy_id;
    public int[] enemy_results_dead_count;
    public int[] enemy_results_overkill_damage;
    public int[] drop_entity_ids;
    public int[] enemy_results_level_difference;
    public int[] enemy_results_kill_count;
    public int[] panel_entity_ids;

    public Dictionary<string, object> ToDict()
    {
      return new Dictionary<string, object>()
      {
        ["stage_id"] = (object) this.stage_id,
        ["enemy_results_enemy_id"] = (object) this.enemy_results_enemy_id,
        ["enemy_results_dead_count"] = (object) this.enemy_results_dead_count,
        ["enemy_results_overkill_damage"] = (object) this.enemy_results_overkill_damage,
        ["drop_entity_ids"] = (object) this.drop_entity_ids,
        ["enemy_results_level_difference"] = (object) this.enemy_results_level_difference,
        ["enemy_results_kill_count"] = (object) this.enemy_results_kill_count,
        ["panel_entity_ids"] = (object) this.panel_entity_ids
      };
    }

    public BattleWaveFinishInfo()
    {
    }

    public BattleWaveFinishInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.stage_id = (int) (long) json[nameof (stage_id)];
      this.enemy_results_enemy_id = ((IEnumerable<object>) json[nameof (enemy_results_enemy_id)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.enemy_results_dead_count = ((IEnumerable<object>) json[nameof (enemy_results_dead_count)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.enemy_results_overkill_damage = ((IEnumerable<object>) json[nameof (enemy_results_overkill_damage)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.drop_entity_ids = ((IEnumerable<object>) json[nameof (drop_entity_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.enemy_results_level_difference = ((IEnumerable<object>) json[nameof (enemy_results_level_difference)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.enemy_results_kill_count = ((IEnumerable<object>) json[nameof (enemy_results_kill_count)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.panel_entity_ids = ((IEnumerable<object>) json[nameof (panel_entity_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
    }
  }
}
