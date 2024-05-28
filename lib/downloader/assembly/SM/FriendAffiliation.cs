// Decompiled with JetBrains decompiler
// Type: SM.FriendAffiliation
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
  public class FriendAffiliation : KeyCompare
  {
    public int _status;
    public int _leader_unit_unit;
    public GuildDirectory guild;
    public int player_level;
    public int leader_unit_job_id;
    public DateTime last_signed_in_at;
    public int _leader_unit_unit_type;
    public string player_id;
    public int? _role;
    public int player_emblem_id;
    public string player_name;
    public int leader_unit_id;
    public string guild_id;
    public int leader_unit_level;

    public GuildMembershipStatus status
    {
      get
      {
        if (!Enum.IsDefined(typeof (GuildMembershipStatus), (object) this._status))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GuildMembershipStatus[" + (object) this._status + "]"));
        return (GuildMembershipStatus) this._status;
      }
    }

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

    public GuildRole? role
    {
      get
      {
        if (!this._role.HasValue)
          return new GuildRole?();
        if (!Enum.IsDefined(typeof (GuildRole), (object) this._role))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GuildRole[" + (object) this._role + "]"));
        return new GuildRole?((GuildRole) this._role.Value);
      }
    }

    public FriendAffiliation()
    {
    }

    public FriendAffiliation(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._status = (int) (long) json[nameof (status)];
      this._leader_unit_unit = (int) (long) json[nameof (leader_unit_unit)];
      this.guild = json[nameof (guild)] == null ? (GuildDirectory) null : new GuildDirectory((Dictionary<string, object>) json[nameof (guild)]);
      this.player_level = (int) (long) json[nameof (player_level)];
      this.leader_unit_job_id = (int) (long) json[nameof (leader_unit_job_id)];
      this.last_signed_in_at = DateTime.Parse((string) json[nameof (last_signed_in_at)]);
      this._leader_unit_unit_type = (int) (long) json[nameof (leader_unit_unit_type)];
      this.player_id = (string) json[nameof (player_id)];
      this._role = json[nameof (role)] == null ? new int?() : new int?((int) (long) json[nameof (role)]);
      this.player_emblem_id = (int) (long) json[nameof (player_emblem_id)];
      this.player_name = (string) json[nameof (player_name)];
      this.leader_unit_id = (int) (long) json[nameof (leader_unit_id)];
      this.guild_id = json[nameof (guild_id)] == null ? (string) null : (string) json[nameof (guild_id)];
      this.leader_unit_level = (int) (long) json[nameof (leader_unit_level)];
    }
  }
}
