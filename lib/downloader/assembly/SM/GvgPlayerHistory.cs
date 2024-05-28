// Decompiled with JetBrains decompiler
// Type: SM.GvgPlayerHistory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GvgPlayerHistory : KeyCompare
  {
    public string player_name;
    public int? leader_unit_level;
    public int? role;
    public int? leader_unit_unit_type_id;
    public int? leader_unit_job_id;
    public int? attack_count;
    public int? defense_count;
    public int? leader_unit_unit_id;
    public string player_id;
    public int? defense_star;
    public int? attack_star;
    public int? player_emblem_id;
    public string gvg_uuid;
    public int? contribution;
    public string guild_id;
    public int? leader_unit_id;

    public GvgPlayerHistory()
    {
    }

    public GvgPlayerHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.player_name = json[nameof (player_name)] == null ? (string) null : (string) json[nameof (player_name)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (leader_unit_level)] != null)
      {
        nullable1 = (long?) json[nameof (leader_unit_level)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.leader_unit_level = nullable2;
      int? nullable3;
      if (json[nameof (role)] != null)
      {
        nullable1 = (long?) json[nameof (role)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.role = nullable3;
      int? nullable4;
      if (json[nameof (leader_unit_unit_type_id)] != null)
      {
        nullable1 = (long?) json[nameof (leader_unit_unit_type_id)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.leader_unit_unit_type_id = nullable4;
      int? nullable5;
      if (json[nameof (leader_unit_job_id)] != null)
      {
        nullable1 = (long?) json[nameof (leader_unit_job_id)];
        nullable5 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable5 = new int?();
      this.leader_unit_job_id = nullable5;
      int? nullable6;
      if (json[nameof (attack_count)] != null)
      {
        nullable1 = (long?) json[nameof (attack_count)];
        nullable6 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable6 = new int?();
      this.attack_count = nullable6;
      int? nullable7;
      if (json[nameof (defense_count)] != null)
      {
        nullable1 = (long?) json[nameof (defense_count)];
        nullable7 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable7 = new int?();
      this.defense_count = nullable7;
      int? nullable8;
      if (json[nameof (leader_unit_unit_id)] != null)
      {
        nullable1 = (long?) json[nameof (leader_unit_unit_id)];
        nullable8 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable8 = new int?();
      this.leader_unit_unit_id = nullable8;
      this.player_id = json[nameof (player_id)] == null ? (string) null : (string) json[nameof (player_id)];
      int? nullable9;
      if (json[nameof (defense_star)] != null)
      {
        nullable1 = (long?) json[nameof (defense_star)];
        nullable9 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable9 = new int?();
      this.defense_star = nullable9;
      int? nullable10;
      if (json[nameof (attack_star)] != null)
      {
        nullable1 = (long?) json[nameof (attack_star)];
        nullable10 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable10 = new int?();
      this.attack_star = nullable10;
      int? nullable11;
      if (json[nameof (player_emblem_id)] != null)
      {
        nullable1 = (long?) json[nameof (player_emblem_id)];
        nullable11 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable11 = new int?();
      this.player_emblem_id = nullable11;
      this.gvg_uuid = json[nameof (gvg_uuid)] == null ? (string) null : (string) json[nameof (gvg_uuid)];
      int? nullable12;
      if (json[nameof (contribution)] != null)
      {
        nullable1 = (long?) json[nameof (contribution)];
        nullable12 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable12 = new int?();
      this.contribution = nullable12;
      this.guild_id = json[nameof (guild_id)] == null ? (string) null : (string) json[nameof (guild_id)];
      int? nullable13;
      if (json[nameof (leader_unit_id)] != null)
      {
        nullable1 = (long?) json[nameof (leader_unit_id)];
        nullable13 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable13 = new int?();
      this.leader_unit_id = nullable13;
    }
  }
}
