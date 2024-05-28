// Decompiled with JetBrains decompiler
// Type: SM.PlayerCharacterQuestS
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
  public class PlayerCharacterQuestS : KeyCompare
  {
    public int _quest_character_s;
    public int max_battle_count_limit;
    public int[] clear_rewards;
    public bool is_new;
    public int? remain_battle_count;
    public bool enable_autobattle;
    public bool is_clear;
    public int consumed_ap;

    public QuestCharacterS quest_character_s
    {
      get
      {
        if (MasterData.QuestCharacterS.ContainsKey(this._quest_character_s))
          return MasterData.QuestCharacterS[this._quest_character_s];
        Debug.LogError((object) ("Key not Found: MasterData.QuestCharacterS[" + (object) this._quest_character_s + "]"));
        return (QuestCharacterS) null;
      }
    }

    public PlayerCharacterQuestS()
    {
    }

    public PlayerCharacterQuestS(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._quest_character_s = (int) (long) json[nameof (quest_character_s)];
      this.max_battle_count_limit = (int) (long) json[nameof (max_battle_count_limit)];
      this.clear_rewards = ((IEnumerable<object>) json[nameof (clear_rewards)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.is_new = (bool) json[nameof (is_new)];
      int? nullable1;
      if (json[nameof (remain_battle_count)] != null)
      {
        long? nullable2 = (long?) json[nameof (remain_battle_count)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.remain_battle_count = nullable1;
      this.enable_autobattle = (bool) json[nameof (enable_autobattle)];
      this.is_clear = (bool) json[nameof (is_clear)];
      this.consumed_ap = (int) (long) json[nameof (consumed_ap)];
    }
  }
}
