// Decompiled with JetBrains decompiler
// Type: SM.PlayerStoryQuestS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerStoryQuestS : KeyCompare
  {
    public int consumed_ap;
    public int[] clear_rewards;
    public bool is_new;
    public int bonus_category;
    public DateTime? end_at;
    public int? remain_battle_count;
    public int remaining_time;
    public bool enable_autobattle;
    public int _quest_story_s;
    public bool is_clear;
    public int max_battle_count_limit;

    public QuestStoryS quest_story_s
    {
      get
      {
        if (MasterData.QuestStoryS.ContainsKey(this._quest_story_s))
          return MasterData.QuestStoryS[this._quest_story_s];
        Debug.LogError((object) ("Key not Found: MasterData.QuestStoryS[" + (object) this._quest_story_s + "]"));
        return (QuestStoryS) null;
      }
    }

    public PlayerStoryQuestS()
    {
    }

    public PlayerStoryQuestS(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.consumed_ap = (int) (long) json[nameof (consumed_ap)];
      this.clear_rewards = ((IEnumerable<object>) json[nameof (clear_rewards)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.is_new = (bool) json[nameof (is_new)];
      this.bonus_category = (int) (long) json[nameof (bonus_category)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      int? nullable1;
      if (json[nameof (remain_battle_count)] != null)
      {
        long? nullable2 = (long?) json[nameof (remain_battle_count)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.remain_battle_count = nullable1;
      this.remaining_time = (int) (long) json[nameof (remaining_time)];
      this.enable_autobattle = (bool) json[nameof (enable_autobattle)];
      this._quest_story_s = (int) (long) json[nameof (quest_story_s)];
      this.is_clear = (bool) json[nameof (is_clear)];
      this.max_battle_count_limit = (int) (long) json[nameof (max_battle_count_limit)];
    }
  }
}
