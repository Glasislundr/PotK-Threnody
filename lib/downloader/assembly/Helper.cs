// Decompiled with JetBrains decompiler
// Type: Helper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;

#nullable disable
public class Helper
{
  public int? leader_skill_id;
  public bool is_friend;
  public int level;
  public int leader_unit_job_id;
  public string target_player_id;
  public DateTime target_player_last_signed_in_at;
  public PlayerUnit leader_unit;
  public string target_player_name;
  public string target_comment;
  public bool is_guild_member;
  public int current_emblem_id;
  public int leader_unit_id;
  public int equip_gear_id;
  public int leader_unit_level;
  public int rental_player_unit_id;

  public PlayerHelper Clone()
  {
    return new PlayerHelper()
    {
      is_friend = this.is_friend,
      leader_skill_id = this.leader_skill_id,
      level = this.level,
      target_player_id = this.target_player_id,
      target_player_last_signed_in_at = this.target_player_last_signed_in_at,
      leader_unit = this.leader_unit,
      target_player_name = this.target_player_name,
      is_guild_member = this.is_guild_member,
      current_emblem_id = this.current_emblem_id,
      leader_player_unit_id = this.rental_player_unit_id,
      leader_unit_id = this.leader_unit_id,
      equip_gear_id = this.equip_gear_id,
      leader_unit_level = this.leader_unit_level
    };
  }

  public Helper(PlayerHelper helper)
  {
    this.leader_skill_id = helper.leader_skill_id;
    this.is_friend = helper.is_friend;
    this.level = helper.level;
    this.leader_unit_job_id = helper.leader_unit_job_id;
    this.target_player_id = helper.target_player_id;
    this.target_player_last_signed_in_at = helper.target_player_last_signed_in_at;
    this.leader_unit = helper.leader_unit;
    this.target_player_name = helper.target_player_name;
    this.is_guild_member = helper.is_guild_member;
    this.current_emblem_id = helper.current_emblem_id;
    this.leader_unit_id = helper.leader_unit_id;
    this.equip_gear_id = helper.equip_gear_id;
    this.leader_unit_level = helper.leader_unit_level;
    this.rental_player_unit_id = helper.leader_player_unit_id;
  }

  public Helper(SeaPlayerHelper helper)
  {
    this.leader_skill_id = helper.leader_skill_id;
    this.is_friend = helper.is_friend;
    this.level = helper.level;
    this.leader_unit_job_id = helper.leader_unit_job_id;
    this.target_player_id = helper.target_player_id;
    this.target_player_last_signed_in_at = helper.target_player_last_signed_in_at;
    this.leader_unit = helper.leader_unit;
    this.target_player_name = helper.target_player_name;
    this.is_guild_member = helper.is_guild_member;
    this.current_emblem_id = helper.current_emblem_id;
    this.leader_unit_id = helper.leader_unit_id;
    this.equip_gear_id = helper.equip_gear_id;
    this.leader_unit_level = helper.leader_unit_level;
    this.rental_player_unit_id = helper.leader_player_unit_id;
  }

  public UnitUnit leader_unit_from_cache => MasterData.UnitUnit[this.leader_unit_id];
}
