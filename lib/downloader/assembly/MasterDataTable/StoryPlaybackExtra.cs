// Decompiled with JetBrains decompiler
// Type: MasterDataTable.StoryPlaybackExtra
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class StoryPlaybackExtra
  {
    public int ID;
    public string name;
    public int quest_QuestExtraS;
    public int priority;
    public DateTime? display_expire_at;

    public static StoryPlaybackExtra Parse(MasterDataReader reader)
    {
      return new StoryPlaybackExtra()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        quest_QuestExtraS = reader.ReadInt(),
        priority = reader.ReadInt(),
        display_expire_at = reader.ReadDateTimeOrNull()
      };
    }

    public QuestExtraS quest
    {
      get
      {
        QuestExtraS quest;
        if (!MasterData.QuestExtraS.TryGetValue(this.quest_QuestExtraS, out quest))
          Debug.LogError((object) ("Key not Found: MasterData.QuestExtraS[" + (object) this.quest_QuestExtraS + "]"));
        return quest;
      }
    }
  }
}
