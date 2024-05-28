// Decompiled with JetBrains decompiler
// Type: MasterDataTable.StoryPlaybackCharacter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class StoryPlaybackCharacter
  {
    public int ID;
    public string name;
    public int quest_QuestCharacterS;
    public int priority;

    public static StoryPlaybackCharacter Parse(MasterDataReader reader)
    {
      return new StoryPlaybackCharacter()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        quest_QuestCharacterS = reader.ReadInt(),
        priority = reader.ReadInt()
      };
    }

    public QuestCharacterS quest
    {
      get
      {
        QuestCharacterS quest;
        if (!MasterData.QuestCharacterS.TryGetValue(this.quest_QuestCharacterS, out quest))
          Debug.LogError((object) ("Key not Found: MasterData.QuestCharacterS[" + (object) this.quest_QuestCharacterS + "]"));
        return quest;
      }
    }
  }
}
