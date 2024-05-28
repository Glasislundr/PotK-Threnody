// Decompiled with JetBrains decompiler
// Type: SM.QuestScoreBattleFinishContext
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class QuestScoreBattleFinishContext : KeyCompare
  {
    public DateTime start_at;
    public string bonus_rate;
    public DateTime latest_end_at;
    public bool battle_score_max_updated;
    public QuestScoreAcquisition[] score_acquisitions;
    public bool total_reward_exists;
    public int battle_score;
    public int rank;
    public DateTime end_at;
    public DateTime final_at;
    public bool is_open;
    public int battle_score_max;
    public QuestScoreBattleFinishContextScore_total_rewards[] score_total_rewards;
    public bool score_max_updated;
    public QuestScoreBattleFinishContextScore_achivement_rewards[] score_achivement_rewards;
    public int rank_before;
    public int score_max;
    public bool score_ranking_disabled;
    public int score_total;

    public QuestScoreBattleFinishContext()
    {
    }

    public QuestScoreBattleFinishContext(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.start_at = DateTime.Parse((string) json[nameof (start_at)]);
      this.bonus_rate = (string) json[nameof (bonus_rate)];
      this.latest_end_at = DateTime.Parse((string) json[nameof (latest_end_at)]);
      this.battle_score_max_updated = (bool) json[nameof (battle_score_max_updated)];
      List<QuestScoreAcquisition> scoreAcquisitionList = new List<QuestScoreAcquisition>();
      foreach (object json1 in (List<object>) json[nameof (score_acquisitions)])
        scoreAcquisitionList.Add(json1 == null ? (QuestScoreAcquisition) null : new QuestScoreAcquisition((Dictionary<string, object>) json1));
      this.score_acquisitions = scoreAcquisitionList.ToArray();
      this.total_reward_exists = (bool) json[nameof (total_reward_exists)];
      this.battle_score = (int) (long) json[nameof (battle_score)];
      this.rank = (int) (long) json[nameof (rank)];
      this.end_at = DateTime.Parse((string) json[nameof (end_at)]);
      this.final_at = DateTime.Parse((string) json[nameof (final_at)]);
      this.is_open = (bool) json[nameof (is_open)];
      this.battle_score_max = (int) (long) json[nameof (battle_score_max)];
      List<QuestScoreBattleFinishContextScore_total_rewards> scoreTotalRewardsList = new List<QuestScoreBattleFinishContextScore_total_rewards>();
      foreach (object json2 in (List<object>) json[nameof (score_total_rewards)])
        scoreTotalRewardsList.Add(json2 == null ? (QuestScoreBattleFinishContextScore_total_rewards) null : new QuestScoreBattleFinishContextScore_total_rewards((Dictionary<string, object>) json2));
      this.score_total_rewards = scoreTotalRewardsList.ToArray();
      this.score_max_updated = (bool) json[nameof (score_max_updated)];
      List<QuestScoreBattleFinishContextScore_achivement_rewards> achivementRewardsList = new List<QuestScoreBattleFinishContextScore_achivement_rewards>();
      foreach (object json3 in (List<object>) json[nameof (score_achivement_rewards)])
        achivementRewardsList.Add(json3 == null ? (QuestScoreBattleFinishContextScore_achivement_rewards) null : new QuestScoreBattleFinishContextScore_achivement_rewards((Dictionary<string, object>) json3));
      this.score_achivement_rewards = achivementRewardsList.ToArray();
      this.rank_before = (int) (long) json[nameof (rank_before)];
      this.score_max = (int) (long) json[nameof (score_max)];
      this.score_ranking_disabled = (bool) json[nameof (score_ranking_disabled)];
      this.score_total = (int) (long) json[nameof (score_total)];
    }
  }
}
