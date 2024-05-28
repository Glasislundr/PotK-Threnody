// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestHarmonyReleaseCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestHarmonyReleaseCondition
  {
    public int ID;
    public int quest_s_QuestHarmonyS;
    public int? required_quest_s_QuestHarmonyS;
    public int character_id;
    public int target_character_id;
    public int required_intimacy_level;

    public static QuestHarmonyReleaseCondition Parse(MasterDataReader reader)
    {
      return new QuestHarmonyReleaseCondition()
      {
        ID = reader.ReadInt(),
        quest_s_QuestHarmonyS = reader.ReadInt(),
        required_quest_s_QuestHarmonyS = reader.ReadIntOrNull(),
        character_id = reader.ReadInt(),
        target_character_id = reader.ReadInt(),
        required_intimacy_level = reader.ReadInt()
      };
    }

    public QuestHarmonyS quest_s
    {
      get
      {
        QuestHarmonyS questS;
        if (!MasterData.QuestHarmonyS.TryGetValue(this.quest_s_QuestHarmonyS, out questS))
          Debug.LogError((object) ("Key not Found: MasterData.QuestHarmonyS[" + (object) this.quest_s_QuestHarmonyS + "]"));
        return questS;
      }
    }

    public QuestHarmonyS required_quest_s
    {
      get
      {
        if (!this.required_quest_s_QuestHarmonyS.HasValue)
          return (QuestHarmonyS) null;
        QuestHarmonyS requiredQuestS;
        if (!MasterData.QuestHarmonyS.TryGetValue(this.required_quest_s_QuestHarmonyS.Value, out requiredQuestS))
          Debug.LogError((object) ("Key not Found: MasterData.QuestHarmonyS[" + (object) this.required_quest_s_QuestHarmonyS.Value + "]"));
        return requiredQuestS;
      }
    }
  }
}
