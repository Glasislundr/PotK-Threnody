// Decompiled with JetBrains decompiler
// Type: SM.PlayerSupply
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerSupply : KeyCompare
  {
    public int _entity_type;
    public int box_type_id;
    public bool broken;
    public int entity_id;
    public int equipped_reisou_player_gear_id;
    public bool favorite;
    public bool for_battle;
    public int gear_accessory_remaining_amount;
    public int gear_exp;
    public int gear_exp_next;
    public int gear_level;
    public int gear_level_limit;
    public int gear_level_limit_max;
    public int gear_level_unlimit;
    public int gear_total_exp;
    public int id;
    public bool is_new;
    public string player_id;
    public int quantity;

    public PlayerSupply()
    {
    }

    public PlayerSupply(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this._entity_type = (int) (long) json[nameof (_entity_type)];
      this.box_type_id = (int) (long) json[nameof (box_type_id)];
      this.broken = (bool) json[nameof (broken)];
      this.entity_id = (int) (long) json[nameof (entity_id)];
      this.equipped_reisou_player_gear_id = (int) (long) json[nameof (equipped_reisou_player_gear_id)];
      this.favorite = (bool) json[nameof (favorite)];
      this.for_battle = (bool) json[nameof (for_battle)];
      this.gear_accessory_remaining_amount = (int) (long) json[nameof (gear_accessory_remaining_amount)];
      this.gear_exp = (int) (long) json[nameof (gear_exp)];
      this.gear_exp_next = (int) (long) json[nameof (gear_exp_next)];
      this.gear_level = (int) (long) json[nameof (gear_level)];
      this.gear_level_limit = (int) (long) json[nameof (gear_level_limit)];
      this.gear_level_limit_max = (int) (long) json[nameof (gear_level_limit_max)];
      this.gear_level_unlimit = (int) (long) json[nameof (gear_level_unlimit)];
      this.gear_total_exp = (int) (long) json[nameof (gear_total_exp)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this.is_new = (bool) json[nameof (is_new)];
      this.player_id = (string) json[nameof (player_id)];
      this.quantity = (int) (long) json[nameof (quantity)];
    }
  }
}
