// Decompiled with JetBrains decompiler
// Type: MasterDataTable.PvpClassRankingReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class PvpClassRankingReward
  {
    public int ID;
    public int term_id;
    public int ranking_kind_PvpRankingKind;
    public int ranking_category_PvpRankingCondition;
    public int reward_type_CommonRewardType;
    public int reward_id;
    public int reward_quantity;
    public string reward_message;

    public static PvpClassRankingReward Parse(MasterDataReader reader)
    {
      return new PvpClassRankingReward()
      {
        ID = reader.ReadInt(),
        term_id = reader.ReadInt(),
        ranking_kind_PvpRankingKind = reader.ReadInt(),
        ranking_category_PvpRankingCondition = reader.ReadInt(),
        reward_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadInt(),
        reward_message = reader.ReadString(true)
      };
    }

    public PvpRankingKind ranking_kind
    {
      get
      {
        PvpRankingKind rankingKind;
        if (!MasterData.PvpRankingKind.TryGetValue(this.ranking_kind_PvpRankingKind, out rankingKind))
          Debug.LogError((object) ("Key not Found: MasterData.PvpRankingKind[" + (object) this.ranking_kind_PvpRankingKind + "]"));
        return rankingKind;
      }
    }

    public PvpRankingCondition ranking_category
    {
      get
      {
        PvpRankingCondition rankingCategory;
        if (!MasterData.PvpRankingCondition.TryGetValue(this.ranking_category_PvpRankingCondition, out rankingCategory))
          Debug.LogError((object) ("Key not Found: MasterData.PvpRankingCondition[" + (object) this.ranking_category_PvpRankingCondition + "]"));
        return rankingCategory;
      }
    }

    public CommonRewardType reward_type => (CommonRewardType) this.reward_type_CommonRewardType;
  }
}
