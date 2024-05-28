// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestStoryMissionReward
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestStoryMissionReward
  {
    public int ID;
    public int quest_m_QuestStoryM;
    public int reward_type_CommonRewardType;
    public int reward_id;
    public int quantity;
    public string message;
    public string result_message;

    public static QuestStoryMissionReward Parse(MasterDataReader reader)
    {
      return new QuestStoryMissionReward()
      {
        ID = reader.ReadInt(),
        quest_m_QuestStoryM = reader.ReadInt(),
        reward_type_CommonRewardType = reader.ReadInt(),
        reward_id = reader.ReadInt(),
        quantity = reader.ReadInt(),
        message = reader.ReadString(true),
        result_message = reader.ReadString(true)
      };
    }

    public QuestStoryM quest_m
    {
      get
      {
        QuestStoryM questM;
        if (!MasterData.QuestStoryM.TryGetValue(this.quest_m_QuestStoryM, out questM))
          Debug.LogError((object) ("Key not Found: MasterData.QuestStoryM[" + (object) this.quest_m_QuestStoryM + "]"));
        return questM;
      }
    }

    public CommonRewardType reward_type => (CommonRewardType) this.reward_type_CommonRewardType;
  }
}
