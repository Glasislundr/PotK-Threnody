// Decompiled with JetBrains decompiler
// Type: MasterDataTable.QuestHarmonyDisplayCondition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class QuestHarmonyDisplayCondition
  {
    public int ID;
    public int quest_s_QuestHarmonyS;
    public int priority;
    public int unit_UnitUnit;
    public string name;

    public static QuestHarmonyDisplayCondition Parse(MasterDataReader reader)
    {
      return new QuestHarmonyDisplayCondition()
      {
        ID = reader.ReadInt(),
        quest_s_QuestHarmonyS = reader.ReadInt(),
        priority = reader.ReadInt(),
        unit_UnitUnit = reader.ReadInt(),
        name = reader.ReadString(true)
      };
    }

    public QuestHarmonyS quest_s
    {
      get
      {
        QuestHarmonyS questS;
        if (!MasterData.QuestHarmonyS.TryGetValue(this.quest_s_QuestHarmonyS, out questS))
          Debug.LogError((object) ("Key not Found: MasterData.QuestHarmonyS[" + (object) this.quest_s_QuestHarmonyS + "]"));
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
