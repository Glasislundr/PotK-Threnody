// Decompiled with JetBrains decompiler
// Type: QuestConverterData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;

#nullable disable
public class QuestConverterData
{
  public int? id_XL;
  public int id_L;
  public int id_M;
  public int id_S;
  public bool is_new;
  public bool is_clear;
  public string title_M;
  public string title_S;
  public int numM_in_thisL;
  public int numS_in_thisM;
  public int lost_ap;
  public string victory_sub_name;
  public string victory_name;
  public string hscroll_bg_name;
  public string sub_bg_name;
  public bool canPlay;
  public int? has_reward;
  public QuestExtra.SeekType seek_type;
  public int? button_folder_ID;
  public CommonQuestType type;
  public CommonQuestMode? mode;
  public float offset_x;
  public float offset_y;
  public float scale;
  public int? remain_battle_count;
  public int daily_limit;
  public int daily_limit_strictly;
  public int max_battle_count_limit;
  public int[] clear_rewards;
  public QuestWave wave;
  public int top_category;
  public bool is_skip_sortie;
  public int player_unit_id;
  public bool no_return_scene;

  public QuestConverterData(PlayerStoryQuestS story)
  {
    QuestStoryS questStoryS = story.quest_story_s;
    this.SetData(new int?(questStoryS.quest_xl_QuestStoryXL), questStoryS.quest_l_QuestStoryL, questStoryS.quest_m_QuestStoryM, questStoryS.ID, story.is_new, story.is_clear, questStoryS.quest_m.number_m, questStoryS.number_s, questStoryS.quest_m.name, questStoryS.name, story.consumed_ap, questStoryS.stage.victory_condition.sub_name, questStoryS.stage.victory_condition.name, questStoryS.quest_m.background_button_name, questStoryS.quest_m.background.background_name, true, questStoryS.has_reward, QuestExtra.SeekType.None, new int?(), CommonQuestType.Story, new CommonQuestMode?(questStoryS.quest_l.quest_mode), questStoryS.quest_m.background.offset_x, questStoryS.quest_m.background.offset_y, questStoryS.quest_m.background.scale, story.remain_battle_count, 0, 0, story.max_battle_count_limit, story.clear_rewards, (QuestWave) null, 0);
  }

  public QuestConverterData(PlayerExtraQuestS extra)
  {
    QuestExtraS questExtraS = extra.quest_extra_s;
    this.SetData(new int?(), questExtraS.quest_l_QuestExtraL, questExtraS.quest_m_QuestExtraM, questExtraS.ID, extra.is_new, extra.is_clear, questExtraS.quest_m.priority, questExtraS.number_s, questExtraS.quest_m.name, questExtraS.name, extra.consumed_ap, questExtraS.stage.victory_condition.sub_name, questExtraS.stage.victory_condition.name, questExtraS.quest_m.background_button_name, questExtraS.quest_m.background.background_name, !extra.time_locked_at.HasValue, questExtraS.has_reward, QuestExtra.toSeekType(extra.seek_type), questExtraS.quest_m.button_type, CommonQuestType.Extra, new CommonQuestMode?(), questExtraS.quest_m.background.offset_x, questExtraS.quest_m.background.offset_y, questExtraS.quest_m.background.scale, extra.remain_battle_count, extra.daily_limit, extra.daily_limit_strictly, extra.max_battle_count_limit, extra.clear_rewards, extra.quest_extra_s.wave, extra.top_category.ID);
  }

  public QuestConverterData(PlayerSeaQuestS story)
  {
    QuestSeaS questSeaS = story.quest_sea_s;
    this.SetData(new int?(questSeaS.quest_xl_QuestSeaXL), questSeaS.quest_l_QuestSeaL, questSeaS.quest_m_QuestSeaM, questSeaS.ID, story.is_new, story.is_clear, questSeaS.quest_m.number_m, questSeaS.number_s, questSeaS.quest_m.name, questSeaS.name, story.consumed_ap, questSeaS.stage.victory_condition.sub_name, questSeaS.stage.victory_condition.name, questSeaS.quest_m.background_button_name, questSeaS.quest_m.background.background_name, true, questSeaS.has_reward, QuestExtra.SeekType.None, new int?(), CommonQuestType.Sea, new CommonQuestMode?(questSeaS.quest_l.quest_mode), questSeaS.quest_m.background.offset_x, questSeaS.quest_m.background.offset_y, questSeaS.quest_m.background.scale, story.remain_battle_count, 0, 0, story.max_battle_count_limit, story.clear_rewards, (QuestWave) null, 0);
  }

  private void SetData(
    int? xl,
    int l,
    int m,
    int s,
    bool thisnew,
    bool thisclear,
    int numM,
    int numS,
    string titleM,
    string titleS,
    int ap,
    string victSubName,
    string victName,
    string hscroll_bg,
    string sub_bg,
    bool canPlay,
    int? reward,
    QuestExtra.SeekType seek,
    int? folderID,
    CommonQuestType questtype,
    CommonQuestMode? questmode,
    float offset_x,
    float offset_y,
    float scale,
    int? remainBattleCount,
    int dailyLimit,
    int dailyLimitStrictly,
    int maxBattleCountLimit,
    int[] clear_rewards,
    QuestWave wave,
    int top_category)
  {
    this.id_XL = xl;
    this.id_L = l;
    this.id_M = m;
    this.id_S = s;
    this.is_new = thisnew;
    this.is_clear = thisclear;
    this.numM_in_thisL = numM;
    this.numS_in_thisM = numS;
    this.title_M = titleM;
    this.title_S = titleS;
    this.lost_ap = ap;
    this.victory_sub_name = victSubName;
    this.victory_name = victName;
    this.hscroll_bg_name = hscroll_bg;
    this.sub_bg_name = sub_bg;
    this.has_reward = reward;
    this.seek_type = seek;
    this.button_folder_ID = folderID;
    this.type = questtype;
    this.mode = questmode;
    this.offset_x = offset_x;
    this.offset_y = offset_y;
    this.scale = scale;
    this.remain_battle_count = remainBattleCount;
    this.daily_limit = dailyLimit;
    this.daily_limit_strictly = dailyLimitStrictly;
    this.max_battle_count_limit = maxBattleCountLimit;
    this.clear_rewards = clear_rewards;
    this.wave = wave;
    this.top_category = top_category;
    this.is_skip_sortie = false;
    this.player_unit_id = 0;
    if (this.remain_battle_count.HasValue)
    {
      if (this.remain_battle_count.Value > 0 & canPlay)
        this.canPlay = true;
      else
        this.canPlay = false;
    }
    else
      this.canPlay = canPlay;
  }
}
