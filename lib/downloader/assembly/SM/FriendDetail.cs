// Decompiled with JetBrains decompiler
// Type: SM.FriendDetail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class FriendDetail : KeyCompare
  {
    public int pvp_weekly_class_point;
    public PlayerUnit[] player_units;
    public int player_current_emblem_id;
    public int pvp_total_win;
    public GuildDirectory guild;
    public bool is_join_pvp_weekly_class;
    public int player_level;
    public PlayerGearReisouSchema[] player_reisou_items;
    public PlayerItem[] player_items;
    public DateTime target_player_last_signed_in_at;
    public bool is_display_pvp_history;
    public string pvp_weekly_class_name;
    public bool is_join_class_match;
    public int pvp_weekly_class_rank;
    public PlayerAwakeSkill[] player_awake_skills;
    public string player_name;
    public string pvp_current_class_name;
    public string player_comment;

    public FriendDetail()
    {
    }

    public FriendDetail(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.pvp_weekly_class_point = (int) (long) json[nameof (pvp_weekly_class_point)];
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      foreach (object json1 in (List<object>) json[nameof (player_units)])
        playerUnitList.Add(json1 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json1));
      this.player_units = playerUnitList.ToArray();
      this.player_current_emblem_id = (int) (long) json[nameof (player_current_emblem_id)];
      this.pvp_total_win = (int) (long) json[nameof (pvp_total_win)];
      this.guild = json[nameof (guild)] == null ? (GuildDirectory) null : new GuildDirectory((Dictionary<string, object>) json[nameof (guild)]);
      this.is_join_pvp_weekly_class = (bool) json[nameof (is_join_pvp_weekly_class)];
      this.player_level = (int) (long) json[nameof (player_level)];
      List<PlayerGearReisouSchema> gearReisouSchemaList = new List<PlayerGearReisouSchema>();
      foreach (object json2 in (List<object>) json[nameof (player_reisou_items)])
        gearReisouSchemaList.Add(json2 == null ? (PlayerGearReisouSchema) null : new PlayerGearReisouSchema((Dictionary<string, object>) json2));
      this.player_reisou_items = gearReisouSchemaList.ToArray();
      List<PlayerItem> playerItemList = new List<PlayerItem>();
      foreach (object json3 in (List<object>) json[nameof (player_items)])
        playerItemList.Add(json3 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json3));
      this.player_items = playerItemList.ToArray();
      this.target_player_last_signed_in_at = DateTime.Parse((string) json[nameof (target_player_last_signed_in_at)]);
      this.is_display_pvp_history = (bool) json[nameof (is_display_pvp_history)];
      this.pvp_weekly_class_name = (string) json[nameof (pvp_weekly_class_name)];
      this.is_join_class_match = (bool) json[nameof (is_join_class_match)];
      this.pvp_weekly_class_rank = (int) (long) json[nameof (pvp_weekly_class_rank)];
      List<PlayerAwakeSkill> playerAwakeSkillList = new List<PlayerAwakeSkill>();
      foreach (object json4 in (List<object>) json[nameof (player_awake_skills)])
        playerAwakeSkillList.Add(json4 == null ? (PlayerAwakeSkill) null : new PlayerAwakeSkill((Dictionary<string, object>) json4));
      this.player_awake_skills = playerAwakeSkillList.ToArray();
      this.player_name = (string) json[nameof (player_name)];
      this.pvp_current_class_name = (string) json[nameof (pvp_current_class_name)];
      this.player_comment = (string) json[nameof (player_comment)];
    }
  }
}
