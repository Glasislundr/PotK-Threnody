// Decompiled with JetBrains decompiler
// Type: SM.PlayerMythologyGearStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerMythologyGearStatus : KeyCompare
  {
    public int chaos_gear_level;
    public int chaos_gear_total_exp;
    public int holy_gear_exp;
    public int holy_gear_exp_next;
    public int chaos_gear_exp;
    public int holy_gear_level;
    public int holy_gear_level_limit;
    public int mythology_player_gear_id;
    public int holy_gear_total_exp;
    public int chaos_gear_exp_next;
    public int chaos_gear_level_limit;

    public PlayerMythologyGearStatus()
    {
    }

    public PlayerMythologyGearStatus(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this.chaos_gear_level = (int) (long) json[nameof (chaos_gear_level)];
      this.chaos_gear_total_exp = (int) (long) json[nameof (chaos_gear_total_exp)];
      this.holy_gear_exp = (int) (long) json[nameof (holy_gear_exp)];
      this.holy_gear_exp_next = (int) (long) json[nameof (holy_gear_exp_next)];
      this.chaos_gear_exp = (int) (long) json[nameof (chaos_gear_exp)];
      this.holy_gear_level = (int) (long) json[nameof (holy_gear_level)];
      this.holy_gear_level_limit = (int) (long) json[nameof (holy_gear_level_limit)];
      this._key = (object) (this.mythology_player_gear_id = (int) (long) json[nameof (mythology_player_gear_id)]);
      this.holy_gear_total_exp = (int) (long) json[nameof (holy_gear_total_exp)];
      this.chaos_gear_exp_next = (int) (long) json[nameof (chaos_gear_exp_next)];
      this.chaos_gear_level_limit = (int) (long) json[nameof (chaos_gear_level_limit)];
    }
  }
}
