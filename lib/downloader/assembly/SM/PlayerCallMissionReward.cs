// Decompiled with JetBrains decompiler
// Type: SM.PlayerCallMissionReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerCallMissionReward : KeyCompare
  {
    public int? reward_quantity;
    public bool is_already_received;
    public bool is_recipe;
    public int? reward_type_id;
    public int? reward_id;

    public PlayerCallMissionReward()
    {
    }

    public PlayerCallMissionReward(Dictionary<string, object> json)
    {
      this._hasKey = false;
      long? nullable1;
      int? nullable2;
      if (json[nameof (reward_quantity)] != null)
      {
        nullable1 = (long?) json[nameof (reward_quantity)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.reward_quantity = nullable2;
      this.is_already_received = (bool) json[nameof (is_already_received)];
      this.is_recipe = (bool) json[nameof (is_recipe)];
      int? nullable3;
      if (json[nameof (reward_type_id)] != null)
      {
        nullable1 = (long?) json[nameof (reward_type_id)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.reward_type_id = nullable3;
      int? nullable4;
      if (json[nameof (reward_id)] != null)
      {
        nullable1 = (long?) json[nameof (reward_id)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.reward_id = nullable4;
    }
  }
}
