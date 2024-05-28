// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestCharacterDisplayCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestCharacterDisplayCondition
  {
    public int ID;
    public int quest_s_QuestCharacterS;
    public int priority;
    public int unit_UnitUnit;
    public string name;

    public static QuestCharacterDisplayCondition Parse(MasterDataReader reader)
    {
      return new QuestCharacterDisplayCondition()
      {
        ID = reader.ReadInt(),
        quest_s_QuestCharacterS = reader.ReadInt(),
        priority = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        name = reader.ReadString(true)
      };
    }

    public QuestCharacterS quest_s
    {
      get
      {
        QuestCharacterS questS;
        if (!MasterData.QuestCharacterS.TryGetValue(this.quest_s_QuestCharacterS, out questS))
          Debug.LogError((object) ("Key not Found: MasterData.QuestCharacterS[" + (object) this.quest_s_QuestCharacterS + "]"));
        return questS;
      }
    }

    public UnitUnit unit
    {
      get
      {
        UnitUnit unit;
        if (!MasterData.UnitUnit.TryGetValue(this.unit_UnitUnit, out unit))
          Debug.LogError((object) ("Key not Found: MasterData.UnitUnit[" + (object) this.unit_UnitUnit + "]"));
        return unit;
      }
    }
  }
}
