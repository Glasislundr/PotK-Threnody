// Decompiled with JetBrains decompiler
// Type: QuestDisplayConditionConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
public class QuestDisplayConditionConverter
{
  public int ID;
  public int quest_s_id;
  public int priority;
  public int unit_UnitUnit;
  public string name;
  public QuestSConverter quest_s;
  public UnitUnit unit;

  public QuestDisplayConditionConverter(QuestCharacterDisplayCondition quest)
  {
    this.ID = quest.ID;
    this.quest_s_id = quest.quest_s_QuestCharacterS;
    this.priority = quest.priority;
    this.unit_UnitUnit = quest.unit_UnitUnit;
    this.name = quest.name;
    this.quest_s = new QuestSConverter(quest.quest_s, 0);
    this.unit = quest.unit;
  }

  public QuestDisplayConditionConverter(QuestHarmonyDisplayCondition quest)
  {
    this.ID = quest.ID;
    this.quest_s_id = quest.quest_s_QuestHarmonyS;
    this.priority = quest.priority;
    this.unit_UnitUnit = quest.unit_UnitUnit;
    this.name = quest.name;
    this.quest_s = new QuestSConverter(quest.quest_s, 0);
    this.unit = quest.unit;
  }
}
