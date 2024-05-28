// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestStoryM
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestStoryM
  {
    public int ID;
    public string name;
    public int quest_xl_QuestStoryXL;
    public int quest_l_QuestStoryL;
    public int number_m;
    public int priority;
    public int background_QuestCommonBackground;
    public string background_button_name;
    public string short_name;

    public static QuestStoryM Parse(MasterDataReader reader)
    {
      return new QuestStoryM()
      {
        ID = reader.ReadInt(),
        name = reader.ReadString(true),
        quest_xl_QuestStoryXL = reader.ReadInt(),
        quest_l_QuestStoryL = reader.ReadInt(),
        number_m = reader.ReadInt(),
        priority = reader.ReadInt(),
        background_QuestCommonBackground = reader.ReadInt(),
        background_button_name = reader.ReadString(true),
        short_name = reader.ReadString(true)
      };
    }

    public QuestStoryXL quest_xl
    {
      get
      {
        QuestStoryXL questXl;
        if (!MasterData.QuestStoryXL.TryGetValue(this.quest_xl_QuestStoryXL, out questXl))
          Debug.LogError((object) ("Key not Found: MasterData.QuestStoryXL[" + (object) this.quest_xl_QuestStoryXL + "]"));
        return questXl;
      }
    }

    public QuestStoryL quest_l
    {
      get
      {
        QuestStoryL questL;
        if (!MasterData.QuestStoryL.TryGetValue(this.quest_l_QuestStoryL, out questL))
          Debug.LogError((object) ("Key not Found: MasterData.QuestStoryL[" + (object) this.quest_l_QuestStoryL + "]"));
        return questL;
      }
    }

    public QuestCommonBackground background
    {
      get
      {
        QuestCommonBackground background;
        if (!MasterData.QuestCommonBackground.TryGetValue(this.background_QuestCommonBackground, out background))
          Debug.LogError((object) ("Key not Found: MasterData.QuestCommonBackground[" + (object) this.background_QuestCommonBackground + "]"));
        return background;
      }
    }
  }
}
