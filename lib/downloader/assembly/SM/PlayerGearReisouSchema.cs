// Decompiled with JetBrains decompiler
// Type: SM.PlayerGearReisouSchema
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerGearReisouSchema : KeyCompare
  {
    public int chaos_gear_level;
    public int gear_level;
    public int gear_id;
    public int id;
    public int holy_gear_level;

    public PlayerItem getReisouItemForSchema() => this.getReisouItemForSchema(this);

    public PlayerItem getReisouItemForSchema(PlayerGearReisouSchema schema)
    {
      PlayerItem reisouItemForSchema = new PlayerItem();
      reisouItemForSchema.entity_id = schema.gear_id;
      reisouItemForSchema._entity_type = 3;
      reisouItemForSchema.for_battle = true;
      reisouItemForSchema.gear_level = schema.gear_level;
      reisouItemForSchema.gear_level_limit = 99;
      reisouItemForSchema.favorite = false;
      reisouItemForSchema.gear_exp_next = 1;
      reisouItemForSchema.is_new = false;
      reisouItemForSchema.broken = false;
      reisouItemForSchema.id = schema.id;
      reisouItemForSchema.quantity = 1;
      if (reisouItemForSchema.gear.isMythologyReisou())
      {
        reisouItemForSchema.ReisouHolyLv = schema.holy_gear_level;
        reisouItemForSchema.ReisouChaosLv = schema.chaos_gear_level;
        reisouItemForSchema.SetPlayerMythologyGearStatusCache(new PlayerMythologyGearStatus()
        {
          holy_gear_level = schema.holy_gear_level,
          chaos_gear_level = schema.chaos_gear_level
        });
      }
      else
      {
        reisouItemForSchema.ReisouHolyLv = 0;
        reisouItemForSchema.ReisouChaosLv = 0;
      }
      return reisouItemForSchema;
    }

    public PlayerGearReisouSchema()
    {
    }

    public PlayerGearReisouSchema(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.chaos_gear_level = (int) (long) json[nameof (chaos_gear_level)];
      this.gear_level = (int) (long) json[nameof (gear_level)];
      this.gear_id = (int) (long) json[nameof (gear_id)];
      this.id = (int) (long) json[nameof (id)];
      this.holy_gear_level = (int) (long) json[nameof (holy_gear_level)];
    }
  }
}
