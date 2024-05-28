// Decompiled with JetBrains decompiler
// Type: PlayerQuestSConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;

#nullable disable
public class PlayerQuestSConverter
{
  public int _quest_s_id;
  public bool is_clear;
  public int consumed_ap;
  public bool enable_autobattle;
  public bool is_new;
  public int[] clear_rewards;
  public int? remain_battle_count;
  public int max_battle_count_limit;
  public QuestSConverter questS;

  public PlayerQuestSConverter(PlayerCharacterQuestS quest)
  {
    this._quest_s_id = quest._quest_character_s;
    this.is_clear = quest.is_clear;
    this.consumed_ap = quest.consumed_ap;
    this.enable_autobattle = quest.enable_autobattle;
    this.is_new = quest.is_new;
    this.clear_rewards = quest.clear_rewards;
    this.remain_battle_count = quest.remain_battle_count;
    this.max_battle_count_limit = quest.max_battle_count_limit;
    this.questS = new QuestSConverter(quest.quest_character_s, quest.consumed_ap);
  }

  public PlayerQuestSConverter(PlayerHarmonyQuestS quest)
  {
    this._quest_s_id = quest._quest_harmony_s;
    this.is_clear = quest.is_clear;
    this.consumed_ap = quest.consumed_ap;
    this.enable_autobattle = quest.enable_autobattle;
    this.is_new = quest.is_new;
    this.clear_rewards = quest.clear_rewards;
    this.remain_battle_count = quest.remain_battle_count;
    this.max_battle_count_limit = quest.max_battle_count_limit;
    this.questS = new QuestSConverter(quest.quest_harmony_s, quest.consumed_ap);
  }
}
