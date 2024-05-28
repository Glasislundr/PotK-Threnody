// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestStoryLimitation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestStoryLimitation
  {
    public int ID;
    public int quest_s_id_QuestStoryS;

    public static QuestStoryLimitation Parse(MasterDataReader reader)
    {
      return new QuestStoryLimitation()
      {
        ID = reader.ReadInt(),
        quest_s_id_QuestStoryS = reader.ReadInt()
      };
    }

    public QuestStoryS quest_s_id
    {
      get
      {
        QuestStoryS questSId;
        if (!MasterData.QuestStoryS.TryGetValue(this.quest_s_id_QuestStoryS, out questSId))
          Debug.LogError((object) ("Key not Found: MasterData.QuestStoryS[" + (object) this.quest_s_id_QuestStoryS + "]"));
        return questSId;
      }
    }
  }
}
