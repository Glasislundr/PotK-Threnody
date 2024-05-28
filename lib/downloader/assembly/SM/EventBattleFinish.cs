// Decompiled with JetBrains decompiler
// Type: SM.EventBattleFinish
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
  public class EventBattleFinish : KeyCompare
  {
    public string period_name;
    public string bonus_rate;
    public int current_sum_point;
    public int all_player_point;
    public EventBattleFinishDestroy_enemys[] destroy_enemys;
    public bool is_quest_target;
    public bool is_bonus_term;
    public int guild_point;
    public int period_id;
    public int original_sum_point;
    public int contribution;
    public int[] get_reward_ids;
    public int player_point;
    public int[] get_guild_rward_ids;
    public int period_type;

    public EventBattleFinish()
    {
    }

    public EventBattleFinish(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.period_name = (string) json[nameof (period_name)];
      this.bonus_rate = (string) json[nameof (bonus_rate)];
      this.current_sum_point = (int) (long) json[nameof (current_sum_point)];
      this.all_player_point = (int) (long) json[nameof (all_player_point)];
      List<EventBattleFinishDestroy_enemys> finishDestroyEnemysList = new List<EventBattleFinishDestroy_enemys>();
      foreach (object json1 in (List<object>) json[nameof (destroy_enemys)])
        finishDestroyEnemysList.Add(json1 == null ? (EventBattleFinishDestroy_enemys) null : new EventBattleFinishDestroy_enemys((Dictionary<string, object>) json1));
      this.destroy_enemys = finishDestroyEnemysList.ToArray();
      this.is_quest_target = (bool) json[nameof (is_quest_target)];
      this.is_bonus_term = (bool) json[nameof (is_bonus_term)];
      this.guild_point = (int) (long) json[nameof (guild_point)];
      this.period_id = (int) (long) json[nameof (period_id)];
      this.original_sum_point = (int) (long) json[nameof (original_sum_point)];
      this.contribution = (int) (long) json[nameof (contribution)];
      this.get_reward_ids = ((IEnumerable<object>) json[nameof (get_reward_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.player_point = (int) (long) json[nameof (player_point)];
      this.get_guild_rward_ids = ((IEnumerable<object>) json[nameof (get_guild_rward_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.period_type = (int) (long) json[nameof (period_type)];
    }
  }
}
