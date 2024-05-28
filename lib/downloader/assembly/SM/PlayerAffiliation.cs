// Decompiled with JetBrains decompiler
// Type: SM.PlayerAffiliation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnitRegulation;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerAffiliation : KeyCompare
  {
    public int _status;
    public DateTime? applied_at;
    public GuildRegistration guild;
    public int guild_medal;
    public DateTime? joined_at;
    public bool guild_map_enabled;
    public string applicant_guild_id;
    public int default_town_slot_number;
    public int? _role;
    public int[] stamp_groups;
    public int? _role_name;
    public string guild_id;
    public DateTime? leaved_at;
    public RaidPeriod raid_period;
    public bool raid_aggregating;

    public bool onGvgOperation
    {
      get
      {
        return this.guild.gvg_status == GvgStatus.matching || this.guild.gvg_status == GvgStatus.preparing || this.guild.gvg_status == GvgStatus.fighting || this.guild.gvg_status == GvgStatus.aggregating || this.guild.gvg_status == GvgStatus.finished;
      }
    }

    public bool onGvgTransition
    {
      get
      {
        return this.guild.gvg_status == GvgStatus.preparing || this.guild.gvg_status == GvgStatus.fighting;
      }
    }

    public PlayerAffiliation Clone() => (PlayerAffiliation) this.MemberwiseClone();

    public static PlayerAffiliation Current => SMManager.Get<PlayerAffiliation>();

    public GuildPlayerInfo Player
    {
      get
      {
        return ((IEnumerable<GuildMembership>) PlayerAffiliation.Current.guild.memberships).Single<GuildMembership>((Func<GuildMembership, bool>) (x => x.player.player_id == SM.Player.Current.id)).player;
      }
    }

    public bool isGuildMember()
    {
      return this.status != GuildMembershipStatus.not_exist && this.status != GuildMembershipStatus.applicant && this.status != GuildMembershipStatus.withdraw;
    }

    public GuildMembershipStatus status
    {
      get
      {
        if (!Enum.IsDefined(typeof (GuildMembershipStatus), (object) this._status))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GuildMembershipStatus[" + (object) this._status + "]"));
        return (GuildMembershipStatus) this._status;
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

    public GuildRoleName role_name
    {
      get
      {
        if (!this._role_name.HasValue)
          return (GuildRoleName) null;
        if (MasterData.GuildRoleName.ContainsKey(this._role_name.Value))
          return MasterData.GuildRoleName[this._role_name.Value];
        Debug.LogError((object) ("Key not Found: MasterData.GuildRoleName[" + (object) this._role_name + "]"));
        return (GuildRoleName) null;
      }
    }

    public PlayerAffiliation()
    {
    }

    public PlayerAffiliation(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._status = (int) (long) json[nameof (status)];
      this.applied_at = json[nameof (applied_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (applied_at)]));
      this.guild = json[nameof (guild)] == null ? (GuildRegistration) null : new GuildRegistration((Dictionary<string, object>) json[nameof (guild)]);
      this.guild_medal = (int) (long) json[nameof (guild_medal)];
      this.joined_at = json[nameof (joined_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (joined_at)]));
      this.guild_map_enabled = (bool) json[nameof (guild_map_enabled)];
      this.applicant_guild_id = json[nameof (applicant_guild_id)] == null ? (string) null : (string) json[nameof (applicant_guild_id)];
      this.default_town_slot_number = (int) (long) json[nameof (default_town_slot_number)];
      this._role = json[nameof (role)] == null ? new int?() : new int?((int) (long) json[nameof (role)]);
      this.stamp_groups = ((IEnumerable<object>) json[nameof (stamp_groups)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this._role_name = json[nameof (role_name)] == null ? new int?() : new int?((int) (long) json[nameof (role_name)]);
      this.guild_id = json[nameof (guild_id)] == null ? (string) null : (string) json[nameof (guild_id)];
      this.leaved_at = json[nameof (leaved_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (leaved_at)]));
    }

    public int gvg_period_id
    {
      get => !this.guild.active_gvg_period_id.HasValue ? 0 : this.guild.active_gvg_period_id.Value;
    }

    public GvgPeriod gvgPeriod
    {
      get
      {
        GvgPeriod gvgPeriod;
        return this.gvg_period_id.IsValid() && MasterData.GvgPeriod.TryGetValue(this.gvg_period_id, out gvgPeriod) ? gvgPeriod : (GvgPeriod) null;
      }
    }

    public Checker gvgCheckRules
    {
      get
      {
        int? ruleNo;
        return !(ruleNo = this.gvgPeriod?.rule_no).HasValue ? (Checker) null : GvgRule.createCheckRules(ruleNo.Value);
      }
    }

    public bool IsRaidGuildNotMovePeriod()
    {
      DateTime dateTime1 = ServerTime.NowAppTime();
      if (this.raid_period != null)
      {
        DateTime? entryEndAt = this.raid_period.entry_end_at;
        DateTime dateTime2 = dateTime1;
        if ((entryEndAt.HasValue ? (entryEndAt.GetValueOrDefault() <= dateTime2 ? 1 : 0) : 0) != 0)
        {
          DateTime? endAt = this.raid_period.end_at;
          DateTime dateTime3 = dateTime1;
          if ((endAt.HasValue ? (endAt.GetValueOrDefault() >= dateTime3 ? 1 : 0) : 0) != 0)
            return true;
        }
      }
      if (this.raid_aggregating)
        return true;
      RaidPeriod raidPeriod = this.raid_period;
      return false;
    }
  }
}
