// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestCharacterLimitationLabel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestCharacterLimitationLabel
  {
    public int ID;
    public int quest_s_id_QuestCharacterS;
    public string label;

    public static QuestCharacterLimitationLabel Parse(MasterDataReader reader)
    {
      return new QuestCharacterLimitationLabel()
      {
        ID = reader.ReadInt(),
        quest_s_id_QuestCharacterS = reader.ReadInt(),
        label = reader.ReadString(true)
      };
    }

    public QuestCharacterS quest_s_id
    {
      get
      {
        QuestCharacterS questSId;
        if (!MasterData.QuestCharacterS.TryGetValue(this.quest_s_id_QuestCharacterS, out questSId))
          Debug.LogError((object) ("Key not Found: MasterData.QuestCharacterS[" + (object) this.quest_s_id_QuestCharacterS + "]"));
        return questSId;
      }
    }
  }
}
