// Decompiled with JetBrains decompiler
// Type: SM.GuildRaidPlayerHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GuildRaidPlayerHelpers : KeyCompare
  {
    public int? leader_skill_id;
    public bool is_friend;
    public int level;
    public int leader_unit_job_id;
    public string target_player_id;
    public DateTime target_player_last_signed_in_at;
    public PlayerUnit leader_unit;
    public string target_player_name;
    public int leader_player_unit_id;
    public bool is_guild_member;
    public int current_emblem_id;
    public int leader_unit_id;
    public int equip_gear_id;
    public int leader_unit_level;

    public GuildRaidPlayerHelpers()
    {
    }

    public GuildRaidPlayerHelpers(Dictionary<string, object> json)
    {
      this._hasKey = false;
      int? nullable1;
      if (json[nameof (leader_skill_id)] != null)
      {
        long? nullable2 = (long?) json[nameof (leader_skill_id)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.leader_skill_id = nullable1;
      this.is_friend = (bool) json[nameof (is_friend)];
      this.level = (int) (long) json[nameof (level)];
      this.leader_unit_job_id = (int) (long) json[nameof (leader_unit_job_id)];
      this.target_player_id = (string) json[nameof (target_player_id)];
      this.target_player_last_signed_in_at = DateTime.Parse((string) json[nameof (target_player_last_signed_in_at)]);
      this.leader_unit = json[nameof (leader_unit)] == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json[nameof (leader_unit)]);
      this.target_player_name = (string) json[nameof (target_player_name)];
      this.leader_player_unit_id = (int) (long) json[nameof (leader_player_unit_id)];
      this.is_guild_member = (bool) json[nameof (is_guild_member)];
      this.current_emblem_id = (int) (long) json[nameof (current_emblem_id)];
      this.leader_unit_id = (int) (long) json[nameof (leader_unit_id)];
      this.equip_gear_id = (int) (long) json[nameof (equip_gear_id)];
      this.leader_unit_level = (int) (long) json[nameof (leader_unit_level)];
    }
  }
}
