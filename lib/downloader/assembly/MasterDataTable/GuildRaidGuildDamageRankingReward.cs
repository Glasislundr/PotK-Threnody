// Decompiled with JetBrains decompiler
// Type: MasterDataTable.GuildRaidGuildDamageRankingReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class GuildRaidGuildDamageRankingReward
  {
    public int ID;
    public int? period_GuildRaidPeriod;
    public int condition_id_GuildRaidRankingRewardCondition;
    public int reward_type_CommonRewardType;
    public int? reward_id;
    public int reward_quantity;

    public static GuildRaidGuildDamageRankingReward Parse(MasterDataReader reader)
    {
      return new GuildRaidGuildDamageRankingReward()
      {
        ID = reader.ReadInt(),
        period_GuildRaidPeriod = reader.ReadIntOrNull(),
        condition_id_GuildRaidRankingRewardCondition = reader.ReadInt(),
        reward_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadIntOrNull(),
        reward_quantity = reader.ReadInt()
      };
    }

    public GuildRaidPeriod period
    {
      get
      {
        if (!this.period_GuildRaidPeriod.HasValue)
          return (GuildRaidPeriod) null;
        GuildRaidPeriod period;
        if (!MasterData.GuildRaidPeriod.TryGetValue(this.period_GuildRaidPeriod.Value, out period))
          Debug.LogError((object) ("Key not Found: MasterData.GuildRaidPeriod[" + (object) this.period_GuildRaidPeriod.Value + "]"));
        return period;
      }
    }

    public GuildRaidRankingRewardCondition condition_id
    {
      get
      {
        GuildRaidRankingRewardCondition conditionId;
        if (!MasterData.GuildRaidRankingRewardCondition.TryGetValue(this.condition_id_GuildRaidRankingRewardCondition, out conditionId))
          Debug.LogError((object) ("Key not Found: MasterData.GuildRaidRankingRewardCondition[" + (object) this.condition_id_GuildRaidRankingRewardCondition + "]"));
        return conditionId;
      }
    }

    public CommonRewardType reward_type => (CommonRewardType) this.reward_type_CommonRewardType;
  }
}
