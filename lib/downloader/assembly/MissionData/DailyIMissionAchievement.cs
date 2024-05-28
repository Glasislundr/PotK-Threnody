// Decompiled with JetBrains decompiler
// Type: MissionData.DailyIMissionAchievement
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
  internal class DailyIMissionAchievement : IMissionAchievement
  {
    private PlayerDailyMissionAchievement data_;
    private DailyMission master_;
    private IMissionReward[] rewards_;
    private DailyIMission iMaster_;

    public bool isDaily => true;

    public bool isGuild => false;

    public bool isShow
    {
      get
      {
        if (this.data_.received_count >= this.data_.limit_count)
          return false;
        DailyMissionReward[] rewards = this.data_.rewards;
        return (rewards != null ? rewards.Length : 0) > 0;
      }
    }

    public bool isCleared => this.data_.count >= this.data_.max_count;

    public bool isOwnCleared => this.isCleared;

    public bool isReceived => this.data_.received_count >= this.data_.limit_count;

    public object original => (object) this.data_;

    public int progress_count => this.data_.count;

    public int own_progress_count => this.data_.count;

    public IMissionReward[] rewards
    {
      get
      {
        if (this.rewards_ != null)
          return this.rewards_;
        this.rewards_ = this.data_.rewards != null ? (IMissionReward[]) ((IEnumerable<DailyMissionReward>) this.data_.rewards).Select<DailyMissionReward, DailyIMissionReward>((Func<DailyMissionReward, DailyIMissionReward>) (r => new DailyIMissionReward(r))).ToArray<DailyIMissionReward>() : new IMissionReward[0];
        return this.rewards_;
      }
    }

    public int mission_id => this.data_.mission_id;

    public IMission mission => (IMission) this.iMaster_;

    public DailyIMissionAchievement(PlayerDailyMissionAchievement dat)
    {
      this.data_ = dat;
      this.rewards_ = (IMissionReward[]) null;
      this.iMaster_ = MasterData.DailyMission.TryGetValue(this.data_.mission_id, out this.master_) ? new DailyIMission(this.master_) : (DailyIMission) null;
    }
  }
}
