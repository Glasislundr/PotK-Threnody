// Decompiled with JetBrains decompiler
// Type: QuestSConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
public class QuestSConverter
{
  public QuestSConverter.DataType data_type;
  public int ID;
  public string name;
  public int unit_id;
  public int target_unit_id;
  public int quest_m_id;
  public int priority;
  public int? has_reward;
  public int lost_ap;
  public int stage_BattleStage;
  public bool disable_continue;
  public bool story_only;
  public UnitUnit unit;
  public UnitUnit target_unit;
  public BattleStage stage;
  public QuestMConverter quest_m;
  public UnitGender gender_restriction;

  public QuestSConverter(QuestCharacterS quest, int lost_ap)
  {
    this.data_type = QuestSConverter.DataType.Character;
    this.ID = quest.ID;
    this.name = quest.name;
    this.unit_id = quest.unit_UnitUnit;
    this.target_unit_id = 0;
    this.priority = quest.priority;
    this.has_reward = quest.has_reward;
    this.lost_ap = lost_ap > 0 ? lost_ap : quest.lost_ap;
    this.stage_BattleStage = quest.stage_BattleStage;
    this.disable_continue = quest.disable_continue;
    this.story_only = quest.story_only;
    this.unit = quest.unit;
    this.target_unit = (UnitUnit) null;
    this.stage = quest.stage;
    this.quest_m = new QuestMConverter(quest.quest_m);
    this.gender_restriction = quest.gender_restriction;
  }

  public QuestSConverter(QuestHarmonyS quest, int lost_ap)
  {
    this.data_type = QuestSConverter.DataType.Harmony;
    this.ID = quest.ID;
    this.name = quest.name;
    this.unit_id = quest.unit_UnitUnit;
    this.target_unit_id = quest.target_unit_UnitUnit;
    this.priority = quest.priority;
    this.has_reward = quest.has_reward;
    this.lost_ap = lost_ap > 0 ? lost_ap : quest.lost_ap;
    this.stage_BattleStage = quest.stage_BattleStage;
    this.disable_continue = false;
    this.story_only = quest.story_only;
    this.unit = quest.unit;
    this.target_unit = quest.target_unit;
    this.stage = quest.stage;
    this.quest_m = new QuestMConverter(quest.quest_m);
    this.gender_restriction = quest.gender_restriction;
  }

  public enum DataType
  {
    Character,
    Harmony,
  }
}
