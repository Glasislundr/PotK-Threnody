// Decompiled with JetBrains decompiler
// Type: MasterDataTable.StoryPlaybackSea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class StoryPlaybackSea
  {
    public int ID;
    public string name;
    public int quest_QuestSeaS;
    public int priority;

    public static StoryPlaybackSea Parse(MasterDataReader reader)
    {
      return new StoryPlaybackSea()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        quest_QuestSeaS = reader.ReadInt(),
        priority = reader.ReadInt()
      };
    }

    public QuestSeaS quest
    {
      get
      {
        QuestSeaS quest;
        if (!MasterData.QuestSeaS.TryGetValue(this.quest_QuestSeaS, out quest))
          Debug.LogError((object) ("Key not Found: MasterData.QuestSeaS[" + (object) this.quest_QuestSeaS + "]"));
        return quest;
      }
    }
  }
}
