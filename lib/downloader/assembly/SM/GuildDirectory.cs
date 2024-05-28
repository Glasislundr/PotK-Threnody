// Decompiled with JetBrains decompiler
// Type: SM.GuildDirectory
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
  public class GuildDirectory : KeyCompare
  {
    public int _atmosphere;
    public DateTime? entried_at;
    public int _auto_approval;
    public bool in_gvg;
    public int _auto_kick;
    public GuildAppearance appearance;
    public string guild_name;
    public GuildLevelBonus level_bonus;
    public int _approval_policy;
    public string guild_id;

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

    public GuildDirectory()
    {
    }

    public GuildDirectory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this._atmosphere = (int) (long) json[nameof (atmosphere)];
      this.entried_at = json[nameof (entried_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (entried_at)]));
      this._auto_approval = (int) (long) json[nameof (auto_approval)];
      this.in_gvg = (bool) json[nameof (in_gvg)];
      this._auto_kick = (int) (long) json[nameof (auto_kick)];
      this.appearance = json[nameof (appearance)] == null ? (GuildAppearance) null : new GuildAppearance((Dictionary<string, object>) json[nameof (appearance)]);
      this.guild_name = (string) json[nameof (guild_name)];
      this.level_bonus = json[nameof (level_bonus)] == null ? (GuildLevelBonus) null : new GuildLevelBonus((Dictionary<string, object>) json[nameof (level_bonus)]);
      this._approval_policy = (int) (long) json[nameof (approval_policy)];
      this.guild_id = (string) json[nameof (guild_id)];
    }
  }
}
