// Decompiled with JetBrains decompiler
// Type: GuildSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class GuildSetting
{
  public string guildName;
  public string atmosphere;
  public string approval;
  public string autoApproval;
  public string availability;
  public string autokick;

  public GuildSetting()
  {
    this.guildName = "";
    this.atmosphere = Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL;
    this.approval = Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL;
    this.autoApproval = Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL;
    this.availability = Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL;
    this.autokick = Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL;
  }

  public int atmosphereID
  {
    get
    {
      return this.atmosphere == Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL ? -1 : ((IEnumerable<GuildAtmosphere>) MasterData.GuildAtmosphereList).Where<GuildAtmosphere>((Func<GuildAtmosphere, bool>) (x => x.name == this.atmosphere)).First<GuildAtmosphere>().ID;
    }
  }

  public int approvalID
  {
    get
    {
      return this.approval == Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL ? -1 : ((IEnumerable<GuildApprovalPolicy>) MasterData.GuildApprovalPolicyList).Where<GuildApprovalPolicy>((Func<GuildApprovalPolicy, bool>) (x => x.name == this.approval)).First<GuildApprovalPolicy>().ID;
    }
  }

  public int autoApprovalID
  {
    get
    {
      return this.autoApproval == Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL ? -1 : ((IEnumerable<GuildAutoApproval>) MasterData.GuildAutoApprovalList).Where<GuildAutoApproval>((Func<GuildAutoApproval, bool>) (x => x.name == this.autoApproval)).First<GuildAutoApproval>().ID;
    }
  }

  public int availabilityID
  {
    get
    {
      return this.availability == Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL ? -1 : ((IEnumerable<GuildAvailability>) MasterData.GuildAvailabilityList).Where<GuildAvailability>((Func<GuildAvailability, bool>) (x => x.name == this.availability)).First<GuildAvailability>().ID;
    }
  }

  public int autokickID
  {
    get
    {
      return this.autokick == Consts.GetInstance().GUILD_SETTING_CONDITIONS_NULL ? -1 : ((IEnumerable<GuildAutokick>) MasterData.GuildAutokickList).Where<GuildAutokick>((Func<GuildAutokick, bool>) (x => x.name == this.autokick)).First<GuildAutokick>().ID;
    }
  }
}
