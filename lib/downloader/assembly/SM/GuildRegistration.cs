// Decompiled with JetBrains decompiler
// Type: SM.GuildRegistration
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
  public class GuildRegistration : KeyCompare
  {
    public int _atmosphere;
    public int gvg_count;
    public int _auto_approval;
    public GuildLevelBonus level_bonus;
    public GuildHq[] hqs;
    public int money;
    public int _auto_kick;
    public GuildAppearance appearance;
    public string guild_name;
    public GuildMembership[] memberships;
    public int? active_gvg_period_id;
    public DateTime? gvg_started_at;
    public bool gvg_finished;
    public int _approval_policy;
    public string private_message;
    public int _gvg_status;
    public string guild_id;
    public int gvg_max_star_possession;
    public GuildApplicant[] applicants;

    public GuildAtmosphere atmosphere
    {
      get
      {
        if (MasterData.GuildAtmosphere.ContainsKey(this._atmosphere))
          return MasterData.GuildAtmosphere[this._atmosphere];
        Debug.LogError((object) ("Key not Found: MasterData.GuildAtmosphere[" + (object) this._atmosphere + "]"));
        return (GuildAtmosphere) null;
      }
    }

    public GuildAutoApproval auto_approval
    {
      get
      {
        if (MasterData.GuildAutoApproval.ContainsKey(this._auto_approval))
          return MasterData.GuildAutoApproval[this._auto_approval];
        Debug.LogError((object) ("Key not Found: MasterData.GuildAutoApproval[" + (object) this._auto_approval + "]"));
        return (GuildAutoApproval) null;
      }
    }

    public GuildAutokick auto_kick
    {
      get
      {
        if (MasterData.GuildAutokick.ContainsKey(this._auto_kick))
          return MasterData.GuildAutokick[this._auto_kick];
        Debug.LogError((object) ("Key not Found: MasterData.GuildAutokick[" + (object) this._auto_kick + "]"));
        return (GuildAutokick) null;
      }
    }

    public GuildApprovalPolicy approval_policy
    {
      get
      {
        if (MasterData.GuildApprovalPolicy.ContainsKey(this._approval_policy))
          return MasterData.GuildApprovalPolicy[this._approval_policy];
        Debug.LogError((object) ("Key not Found: MasterData.GuildApprovalPolicy[" + (object) this._approval_policy + "]"));
        return (GuildApprovalPolicy) null;
      }
    }

    public GvgStatus gvg_status
    {
      get
      {
        if (!Enum.IsDefined(typeof (GvgStatus), (object) this._gvg_status))
          Debug.LogError((object) ("Key not Found: MasterDataTable.GvgStatus[" + (object) this._gvg_status + "]"));
        return (GvgStatus) this._gvg_status;
      }
    }

    public GuildRegistration()
    {
    }

    public GuildRegistration(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._atmosphere = (int) (long) json[nameof (atmosphere)];
      this.gvg_count = (int) (long) json[nameof (gvg_count)];
      this._auto_approval = (int) (long) json[nameof (auto_approval)];
      this.level_bonus = json[nameof (level_bonus)] == null ? (GuildLevelBonus) null : new GuildLevelBonus((Dictionary<string, object>) json[nameof (level_bonus)]);
      List<GuildHq> guildHqList = new List<GuildHq>();
      foreach (object json1 in (List<object>) json[nameof (hqs)])
        guildHqList.Add(json1 == null ? (GuildHq) null : new GuildHq((Dictionary<string, object>) json1));
      this.hqs = guildHqList.ToArray();
      this.money = (int) (long) json[nameof (money)];
      this._auto_kick = (int) (long) json[nameof (auto_kick)];
      this.appearance = json[nameof (appearance)] == null ? (GuildAppearance) null : new GuildAppearance((Dictionary<string, object>) json[nameof (appearance)]);
      this.guild_name = (string) json[nameof (guild_name)];
      List<GuildMembership> guildMembershipList = new List<GuildMembership>();
      foreach (object json2 in (List<object>) json[nameof (memberships)])
        guildMembershipList.Add(json2 == null ? (GuildMembership) null : new GuildMembership((Dictionary<string, object>) json2));
      this.memberships = guildMembershipList.ToArray();
      int? nullable1;
      if (json[nameof (active_gvg_period_id)] != null)
      {
        long? nullable2 = (long?) json[nameof (active_gvg_period_id)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.active_gvg_period_id = nullable1;
      this.gvg_started_at = json[nameof (gvg_started_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (gvg_started_at)]));
      this.gvg_finished = (bool) json[nameof (gvg_finished)];
      this._approval_policy = (int) (long) json[nameof (approval_policy)];
      this.private_message = (string) json[nameof (private_message)];
      this._gvg_status = (int) (long) json[nameof (gvg_status)];
      this.guild_id = (string) json[nameof (guild_id)];
      this.gvg_max_star_possession = (int) (long) json[nameof (gvg_max_star_possession)];
      List<GuildApplicant> guildApplicantList = new List<GuildApplicant>();
      foreach (object json3 in (List<object>) json[nameof (applicants)])
        guildApplicantList.Add(json3 == null ? (GuildApplicant) null : new GuildApplicant((Dictionary<string, object>) json3));
      this.applicants = guildApplicantList.ToArray();
    }
  }
}
