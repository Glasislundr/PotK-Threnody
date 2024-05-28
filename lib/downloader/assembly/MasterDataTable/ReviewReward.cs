// Decompiled with JetBrains decompiler
// Type: MasterDataTable.ReviewReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class ReviewReward
  {
    public int ID;
    public int quest_type_CommonQuestType;
    public int quest_s_id;
    public int reward_type_CommonRewardType;
    public int reward_id;
    public int reward_quantity;
    public string title;
    public string message;

    public static ReviewReward Parse(MasterDataReader reader)
    {
      return new ReviewReward()
      {
        ID = reader.ReadInt(),
        quest_type_CommonQuestType = reader.ReadInt(),
        quest_s_id = reader.ReadInt(),
        reward_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        reward_quantity = reader.ReadInt(),
        title = reader.ReadString(true),
        message = reader.ReadString(true)
      };
    }

    public CommonQuestType quest_type => (CommonQuestType) this.quest_type_CommonQuestType;

    public CommonRewardType reward_type => (CommonRewardType) this.reward_type_CommonRewardType;
  }
}
