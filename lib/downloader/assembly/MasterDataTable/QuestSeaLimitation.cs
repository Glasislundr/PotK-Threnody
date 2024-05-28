// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestSeaLimitation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestSeaLimitation
  {
    public int ID;
    public int quest_s_id_QuestSeaS;

    public static QuestSeaLimitation Parse(MasterDataReader reader)
    {
      return new QuestSeaLimitation()
      {
        ID = reader.ReadInt(),
        quest_s_id_QuestSeaS = reader.ReadInt()
      };
    }

    public QuestSeaS quest_s_id
    {
      get
      {
        QuestSeaS questSId;
        if (!MasterData.QuestSeaS.TryGetValue(this.quest_s_id_QuestSeaS, out questSId))
          Debug.LogError((object) ("Key not Found: MasterData.QuestSeaS[" + (object) this.quest_s_id_QuestSeaS + "]"));
        return questSId;
      }
    }
  }
}
