// Decompiled with JetBrains decompiler
// Type: SM.BattleEndDrop_common_ticket_entities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class BattleEndDrop_common_ticket_entities : KeyCompare
  {
    public int reward_quantity;
    public bool is_new;
    public int? reward_id;
    public int reward_type_id;

    public BattleEndDrop_common_ticket_entities()
    {
    }

    public BattleEndDrop_common_ticket_entities(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.reward_quantity = (int) (long) json[nameof (reward_quantity)];
      this.is_new = (bool) json[nameof (is_new)];
      int? nullable1;
      if (json[nameof (reward_id)] != null)
      {
        long? nullable2 = (long?) json[nameof (reward_id)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.reward_id = nullable1;
      this.reward_type_id = (int) (long) json[nameof (reward_type_id)];
    }
  }
}
