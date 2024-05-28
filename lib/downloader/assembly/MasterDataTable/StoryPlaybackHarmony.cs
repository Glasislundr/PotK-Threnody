// Decompiled with JetBrains decompiler
// Type: MasterDataTable.StoryPlaybackHarmony
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class StoryPlaybackHarmony
  {
    public int ID;
    public string name;
    public int quest_QuestHarmonyS;
    public int priority;

    public static StoryPlaybackHarmony Parse(MasterDataReader reader)
    {
      return new StoryPlaybackHarmony()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        quest_QuestHarmonyS = reader.ReadInt(),
        priority = reader.ReadInt()
      };
    }

    public QuestHarmonyS quest
    {
      get
      {
        QuestHarmonyS quest;
        if (!MasterData.QuestHarmonyS.TryGetValue(this.quest_QuestHarmonyS, out quest))
          Debug.LogError((object) ("Key not Found: MasterData.QuestHarmonyS[" + (object) this.quest_QuestHarmonyS + "]"));
        return quest;
      }
    }
  }
}
