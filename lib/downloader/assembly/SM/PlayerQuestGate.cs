// Decompiled with JetBrains decompiler
// Type: SM.PlayerQuestGate
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
  public class PlayerQuestGate : KeyCompare
  {
    public int[] quest_ids;
    public bool can_open;
    public DateTime? start_at;
    public int quest_gate_id;
    public DateTime? end_at;
    public int quest_key_id;
    public int time;
    public string player_id;
    public bool in_progress;
    public int consume_quantity;

    public PlayerQuestGate()
    {
    }

    public PlayerQuestGate(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.quest_ids = ((IEnumerable<object>) json[nameof (quest_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.can_open = (bool) json[nameof (can_open)];
      this.start_at = json[nameof (start_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (start_at)]));
      this.quest_gate_id = (int) (long) json[nameof (quest_gate_id)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
      this.quest_key_id = (int) (long) json[nameof (quest_key_id)];
      this.time = (int) (long) json[nameof (time)];
      this.player_id = (string) json[nameof (player_id)];
      this.in_progress = (bool) json[nameof (in_progress)];
      this.consume_quantity = (int) (long) json[nameof (consume_quantity)];
    }
  }
}
