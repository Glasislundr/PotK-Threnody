// Decompiled with JetBrains decompiler
// Type: SM.GuildPlayerInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildPlayerInfo : KeyCompare
  {
    public int _leader_unit_unit;
    public string player_name;
    public int player_level;
    public int gift_reward_id;
    public DateTime last_signed_in_at;
    public int gift_reward_type_id;
    public int gift_reward_quantity;
    public int _leader_unit_unit_type;
    public string player_id;
    public int leader_player_unit_id;
    public int wish_gift_id;
    public int player_emblem_id;
    public int leader_unit_level;
    public int leader_unit_job_id;

    public UnitUnit leader_unit_unit
    {
      get
      {
        if (MasterData.UnitUnit.ContainsKey(this._leader_unit_unit))
          return MasterData.UnitUnit[this._leader_unit_unit];
        Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this._leader_unit_unit + "]"));
        return (UnitUnit) null;
      }
    }

    public MasterDataTable.UnitType leader_unit_unit_type
    {
      get
      {
        if (MasterData.UnitType.ContainsKey(this._leader_unit_unit_type))
          return MasterData.UnitType[this._leader_unit_unit_type];
        Debug.LogError((object) ("Key not Found: MasterData.UnitType[" + (object) this._leader_unit_unit_type + "]"));
        return (MasterDataTable.UnitType) null;
      }
    }

    public GuildPlayerInfo()
    {
    }

    public GuildPlayerInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._leader_unit_unit = (int) (long) json[nameof (leader_unit_unit)];
      this.player_name = (string) json[nameof (player_name)];
      this.player_level = (int) (long) json[nameof (player_level)];
      this.gift_reward_id = (int) (long) json[nameof (gift_reward_id)];
      this.last_signed_in_at = DateTime.Parse((string) json[nameof (last_signed_in_at)]);
      this.gift_reward_type_id = (int) (long) json[nameof (gift_reward_type_id)];
      this.gift_reward_quantity = (int) (long) json[nameof (gift_reward_quantity)];
      this._leader_unit_unit_type = (int) (long) json[nameof (leader_unit_unit_type)];
      this.player_id = (string) json[nameof (player_id)];
      this.leader_player_unit_id = (int) (long) json[nameof (leader_player_unit_id)];
      this.wish_gift_id = (int) (long) json[nameof (wish_gift_id)];
      this.player_emblem_id = (int) (long) json[nameof (player_emblem_id)];
      this.leader_unit_level = (int) (long) json[nameof (leader_unit_level)];
      this.leader_unit_job_id = (int) (long) json[nameof (leader_unit_job_id)];
    }
  }
}
