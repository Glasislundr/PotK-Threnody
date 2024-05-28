// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestSeaClearMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestSeaClearMessage
  {
    public int ID;
    public bool is_firsttime;
    public int quest_s_id;
    public string title;
    public string message;

    public static QuestSeaClearMessage Parse(MasterDataReader reader)
    {
      return new QuestSeaClearMessage()
      {
        ID = reader.ReadInt(),
        is_firsttime = reader.ReadBool(),
        quest_s_id = reader.ReadInt(),
        title = reader.ReadString(true),
        message = reader.ReadString(true)
      };
    }
  }
}
