// Decompiled with JetBrains decompiler
// Type: SM.GuildMembership
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
  public class GuildMembership : KeyCompare
  {
    public int action_point;
    public int total_defense;
    public int total_attack;
    public GuildPlayerInfo player;
    public int own_star;
    public DateTime joined_at;
    public int defense_priority;
    public bool is_defense_membership;
    public int _role_name;
    public int _role;
    public int capture_star;
    public bool scouted;
    public bool in_battle;
    public int contribution;
    public bool in_attack;

    public GuildRoleName role_name
    {
      get
      {
        if (MasterData.GuildRoleName.ContainsKey(this._role_name))
          return MasterData.GuildRoleName[this._role_name];
        Debug.LogError((object) ("Key not Found: MasterData.GuildRoleName[" + (object) this._role_name + "]"));
        return (GuildRoleName) null;
      }
    }

    public GuildRole role
    {
      get
      {
        if (!Enum.IsDefined(typeof (GuildRole), (object) this._role))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GuildRole[" + (object) this._role + "]"));
        return (GuildRole) this._role;
      }
    }

    public GuildMembership()
    {
    }

    public GuildMembership(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.action_point = (int) (long) json[nameof (action_point)];
      this.total_defense = (int) (long) json[nameof (total_defense)];
      this.total_attack = (int) (long) json[nameof (total_attack)];
      this.player = json[nameof (player)] == null ? (GuildPlayerInfo) null : new GuildPlayerInfo((Dictionary<string, object>) json[nameof (player)]);
      this.own_star = (int) (long) json[nameof (own_star)];
      this.joined_at = DateTime.Parse((string) json[nameof (joined_at)]);
      this.defense_priority = (int) (long) json[nameof (defense_priority)];
      this.is_defense_membership = (bool) json[nameof (is_defense_membership)];
      this._role_name = (int) (long) json[nameof (role_name)];
      this._role = (int) (long) json[nameof (role)];
      this.capture_star = (int) (long) json[nameof (capture_star)];
      this.scouted = (bool) json[nameof (scouted)];
      this.in_battle = (bool) json[nameof (in_battle)];
      this.contribution = (int) (long) json[nameof (contribution)];
      this.in_attack = (bool) json[nameof (in_attack)];
    }
  }
}
