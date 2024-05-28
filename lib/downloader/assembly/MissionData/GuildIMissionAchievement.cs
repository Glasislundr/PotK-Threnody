// Decompiled with JetBrains decompiler
// Type: MissionData.GuildIMissionAchievement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MissionData
{
  internal class GuildIMissionAchievement : IMissionAchievement
  {
    private GuildMissionInfo data_;
    private GuildMission master_;
    private IMissionReward[] rewards_;
    private GuildIMission iMaster_;

    public bool isDaily => false;

    public bool isGuild => true;

    public bool isShow
    {
      get
      {
        DateTime? guildAchievedAt = this.data_.guild_achieved_at;
        DateTime? joinedAt = PlayerAffiliation.Current.joined_at;
        GuildMissionReward[] guildRewards = this.data_.guild_rewards;
        if ((guildRewards != null ? guildRewards.Length : 0) <= 0 || this.mission == null || !this.mission.isShow)
          return false;
        if (this.isReceived || !guildAchievedAt.HasValue)
          return true;
        if (!joinedAt.HasValue)
          return false;
        DateTime? nullable1 = joinedAt;
        DateTime? nullable2 = guildAchievedAt;
        return nullable1.HasValue & nullable2.HasValue && nullable1.GetValueOrDefault() < nullable2.GetValueOrDefault();
      }
    }

    public bool isCleared
    {
      get
      {
        if (this.isReceived)
          return true;
        return this.master_ != null && this.data_.guild_count >= this.master_.achievement_count;
      }
    }

    public bool isOwnCleared => this.master_ != null && this.data_.count >= this.master_.num;

    public bool isReceived => this.data_.received_count > 0;

    public object original => (object) this.data_;

    public int progress_count => this.data_.guild_count;

    public int own_progress_count => this.data_.count;

    public IMissionReward[] rewards
    {
      get
      {
        if (this.rewards_ != null)
          return this.rewards_;
        this.rewards_ = this.data_.guild_rewards != null ? (IMissionReward[]) ((IEnumerable<GuildMissionReward>) this.data_.guild_rewards).Select<GuildMissionReward, GuildIMissionReward>((Func<GuildMissionReward, GuildIMissionReward>) (r => new GuildIMissionReward(r))).ToArray<GuildIMissionReward>() : new IMissionReward[0];
        return this.rewards_;
      }
    }

    public int mission_id => this.data_.mission_id;

    public IMission mission => (IMission) this.iMaster_;

    public GuildIMissionAchievement(GuildMissionInfo dat)
    {
      this.data_ = dat;
      this.rewards_ = (IMissionReward[]) null;
      this.iMaster_ = MasterData.GuildMission.TryGetValue(this.data_.mission_id, out this.master_) ? new GuildIMission(this.master_) : (GuildIMission) null;
    }
  }
}
