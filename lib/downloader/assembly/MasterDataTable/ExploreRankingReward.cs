// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ExploreRankingReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ExploreRankingReward
  {
    public int ID;
    public int group_id;
    public int ranking_category_ExploreRankingCondition;
    public int reward_type_CommonRewardType;
    public int reward_id;
    public int reward_quantity;
    public string reward_message;
    public string present_reward_title;
    public string present_reward_message;

    public static ExploreRankingReward Parse(MasterDataReader reader)
    {
      return new ExploreRankingReward()
      {
        ID = reader.ReadInt(),
        group_id = reader.ReadInt(),
        ranking_category_ExploreRankingCondition = reader.ReadInt(),
        reward_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadInt(),
        reward_message = reader.ReadString(true),
        present_reward_title = reader.ReadString(true),
        present_reward_message = reader.ReadString(true)
      };
    }

    public ExploreRankingCondition ranking_category
    {
      get
      {
        ExploreRankingCondition rankingCategory;
        if (!MasterData.ExploreRankingCondition.TryGetValue(this.ranking_category_ExploreRankingCondition, out rankingCategory))
          Debug.LogError((object) ("Key not Found: MasterData.ExploreRankingCondition[" + (object) this.ranking_category_ExploreRankingCondition + "]"));
        return rankingCategory;
      }
    }

    public CommonRewardType reward_type => (CommonRewardType) this.reward_type_CommonRewardType;
  }
}
