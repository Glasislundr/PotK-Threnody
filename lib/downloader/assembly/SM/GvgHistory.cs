// Decompiled with JetBrains decompiler
// Type: SM.GvgHistory
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
  public class GvgHistory : KeyCompare
  {
    public int _status;
    public int? opponent_defense_star;
    public string target_guild_id;
    public DateTime? created_at;
    public int? defense_star;
    public string gvg_uuid;
    public int? opponent_attack_star;
    public int? target_guild_emblem_id;
    public string target_guild_name;
    public int? attack_star;
    public string guild_id;
    public GvgPlayerHistory[] player_histories;

    public GvgBattleStatus status
    {
      get
      {
        if (!Enum.IsDefined(typeof (GvgBattleStatus), (object) this._status))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GvgBattleStatus[" + (object) this._status + "]"));
        return (GvgBattleStatus) this._status;
      }
    }

    public GvgHistory()
    {
    }

    public GvgHistory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._status = (int) (long) json[nameof (status)];
      long? nullable1;
      int? nullable2;
      if (json[nameof (opponent_defense_star)] != null)
      {
        nullable1 = (long?) json[nameof (opponent_defense_star)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.opponent_defense_star = nullable2;
      this.target_guild_id = json[nameof (target_guild_id)] == null ? (string) null : (string) json[nameof (target_guild_id)];
      this.created_at = json[nameof (created_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (created_at)]));
      int? nullable3;
      if (json[nameof (defense_star)] != null)
      {
        nullable1 = (long?) json[nameof (defense_star)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.defense_star = nullable3;
      this.gvg_uuid = json[nameof (gvg_uuid)] == null ? (string) null : (string) json[nameof (gvg_uuid)];
      int? nullable4;
      if (json[nameof (opponent_attack_star)] != null)
      {
        nullable1 = (long?) json[nameof (opponent_attack_star)];
        nullable4 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable4 = new int?();
      this.opponent_attack_star = nullable4;
      int? nullable5;
      if (json[nameof (target_guild_emblem_id)] != null)
      {
        nullable1 = (long?) json[nameof (target_guild_emblem_id)];
        nullable5 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable5 = new int?();
      this.target_guild_emblem_id = nullable5;
      this.target_guild_name = json[nameof (target_guild_name)] == null ? (string) null : (string) json[nameof (target_guild_name)];
      int? nullable6;
      if (json[nameof (attack_star)] != null)
      {
        nullable1 = (long?) json[nameof (attack_star)];
        nullable6 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable6 = new int?();
      this.attack_star = nullable6;
      this.guild_id = json[nameof (guild_id)] == null ? (string) null : (string) json[nameof (guild_id)];
      List<GvgPlayerHistory> gvgPlayerHistoryList = new List<GvgPlayerHistory>();
      foreach (object json1 in (List<object>) json[nameof (player_histories)])
        gvgPlayerHistoryList.Add(json1 == null ? (GvgPlayerHistory) null : new GvgPlayerHistory((Dictionary<string, object>) json1));
      this.player_histories = gvgPlayerHistoryList.ToArray();
    }
  }
}
