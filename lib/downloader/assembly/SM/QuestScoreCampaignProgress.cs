// Decompiled with JetBrains decompiler
// Type: SM.QuestScoreCampaignProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestScoreCampaignProgress : KeyCompare
  {
    public DateTime start_at;
    public QuestScoreCampaignDescriptionBlock description;
    public DateTime latest_end_at;
    public bool score_ranking_disabled;
    public bool total_reward_exists;
    public DateTime end_at;
    public DateTime final_at;
    public PlayerQuestScoreProgress player;
    public int quest_extra_l;
    public bool is_open;
    public QuestScoreTotalReward[] score_total_rewards;
    public QuestScoreCampaignProgressScore_achivement_rewards[] score_achivement_rewards;
    public QuestScoreRankingReward[] score_ranking_rewards;
    public int id;

    public QuestScoreCampaignProgress()
    {
    }

    public QuestScoreCampaignProgress(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.start_at = DateTime.Parse((string) json[nameof (start_at)]);
      this.description = json[nameof (description)] == null ? (QuestScoreCampaignDescriptionBlock) null : new QuestScoreCampaignDescriptionBlock((Dictionary<string, object>) json[nameof (description)]);
      this.latest_end_at = DateTime.Parse((string) json[nameof (latest_end_at)]);
      this.score_ranking_disabled = (bool) json[nameof (score_ranking_disabled)];
      this.total_reward_exists = (bool) json[nameof (total_reward_exists)];
      this.end_at = DateTime.Parse((string) json[nameof (end_at)]);
      this.final_at = DateTime.Parse((string) json[nameof (final_at)]);
      this.player = json[nameof (player)] == null ? (PlayerQuestScoreProgress) null : new PlayerQuestScoreProgress((Dictionary<string, object>) json[nameof (player)]);
      this.quest_extra_l = (int) (long) json[nameof (quest_extra_l)];
      this.is_open = (bool) json[nameof (is_open)];
      List<QuestScoreTotalReward> scoreTotalRewardList = new List<QuestScoreTotalReward>();
      foreach (object json1 in (List<object>) json[nameof (score_total_rewards)])
        scoreTotalRewardList.Add(json1 == null ? (QuestScoreTotalReward) null : new QuestScoreTotalReward((Dictionary<string, object>) json1));
      this.score_total_rewards = scoreTotalRewardList.ToArray();
      List<QuestScoreCampaignProgressScore_achivement_rewards> achivementRewardsList = new List<QuestScoreCampaignProgressScore_achivement_rewards>();
      foreach (object json2 in (List<object>) json[nameof (score_achivement_rewards)])
        achivementRewardsList.Add(json2 == null ? (QuestScoreCampaignProgressScore_achivement_rewards) null : new QuestScoreCampaignProgressScore_achivement_rewards((Dictionary<string, object>) json2));
      this.score_achivement_rewards = achivementRewardsList.ToArray();
      List<QuestScoreRankingReward> scoreRankingRewardList = new List<QuestScoreRankingReward>();
      foreach (object json3 in (List<object>) json[nameof (score_ranking_rewards)])
        scoreRankingRewardList.Add(json3 == null ? (QuestScoreRankingReward) null : new QuestScoreRankingReward((Dictionary<string, object>) json3));
      this.score_ranking_rewards = scoreRankingRewardList.ToArray();
      this.id = (int) (long) json[nameof (id)];
    }

    public int GetQuestSScore(int sid)
    {
      int questSscore = 0;
      if (this.player.battle_score_max == null || this.player.battle_score_max.Length == 0)
        return 0;
      PlayerQuestScoreExtraS questScoreExtraS = ((IEnumerable<PlayerQuestScoreExtraS>) this.player.battle_score_max).FirstOrDefault<PlayerQuestScoreExtraS>((Func<PlayerQuestScoreExtraS, bool>) (x => x.quest_extra_s == sid));
      if (questScoreExtraS != null)
        questSscore = questScoreExtraS.score_max;
      return questSscore;
    }

    public int GetQuestMScoreFromSID(int sid)
    {
      int score = 0;
      PlayerExtraQuestS[] self = SMManager.Get<PlayerExtraQuestS[]>();
      if (self == null || this.player.battle_score_max.Length == 0)
        return 0;
      PlayerExtraQuestS playerExtraQuestS = ((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) self).CheckMasterData().ToArray<PlayerExtraQuestS>()).FirstOrDefault<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x._quest_extra_s == sid));
      if (playerExtraQuestS != null)
      {
        int questM = playerExtraQuestS.quest_extra_s.quest_m_QuestExtraM;
        MasterData.QuestExtraS.Where<KeyValuePair<int, QuestExtraS>>((Func<KeyValuePair<int, QuestExtraS>, bool>) (x => x.Value.quest_m_QuestExtraM == questM)).ForEach<KeyValuePair<int, QuestExtraS>>((Action<KeyValuePair<int, QuestExtraS>>) (x => score += this.GetQuestSScore(x.Value.ID)));
      }
      return score;
    }

    public int GetQuestMScoreFromMID(int mid)
    {
      int score = 0;
      MasterData.QuestExtraS.Where<KeyValuePair<int, QuestExtraS>>((Func<KeyValuePair<int, QuestExtraS>, bool>) (x => x.Value.quest_m_QuestExtraM == mid)).ForEach<KeyValuePair<int, QuestExtraS>>((Action<KeyValuePair<int, QuestExtraS>>) (x => score += this.GetQuestSScore(x.Value.ID)));
      return score;
    }
  }
}
