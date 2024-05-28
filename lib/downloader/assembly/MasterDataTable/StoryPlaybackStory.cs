// Decompiled with JetBrains decompiler
// Type: MasterDataTable.StoryPlaybackStory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class StoryPlaybackStory
  {
    public int ID;
    public string name;
    public int quest_QuestStoryS;
    public int priority;

    public static StoryPlaybackStory Parse(MasterDataReader reader)
    {
      return new StoryPlaybackStory()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        quest_QuestStoryS = reader.ReadInt(),
        priority = reader.ReadInt()
      };
    }

    public QuestStoryS quest
    {
      get
      {
        QuestStoryS quest;
        if (!MasterData.QuestStoryS.TryGetValue(this.quest_QuestStoryS, out quest))
          Debug.LogError((object) ("Key not Found: MasterData.QuestStoryS[" + (object) this.quest_QuestStoryS + "]"));
        return quest;
      }
    }
  }
}
