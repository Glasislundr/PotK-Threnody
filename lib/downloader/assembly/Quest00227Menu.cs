// Decompiled with JetBrains decompiler
// Type: Quest00227Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00227Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txt_title;
  [SerializeField]
  private NGxScroll scroll;
  private GameObject QuestBar;
  private GameObject RankingBar;

  public IEnumerator Initialize(QuestScoreCampaignProgress qscp)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.AddBarObject(qscp);
    this.txt_title.SetTextLocalize(Consts.GetInstance().QUEST_00227_MENU_TITLE);
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> questBarF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.QuestBar, (Object) null))
    {
      questBarF = Res.Prefabs.quest002_27.list_quest_rank.Load<GameObject>();
      e = questBarF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.QuestBar = questBarF.Result;
      questBarF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.RankingBar, (Object) null))
    {
      questBarF = Res.Prefabs.quest002_27.list_total_rank.Load<GameObject>();
      e = questBarF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.RankingBar = questBarF.Result;
      questBarF = (Future<GameObject>) null;
    }
  }

  private void AddBarObject(QuestScoreCampaignProgress progress)
  {
    this.scroll.Clear();
    if (progress.total_reward_exists)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.QuestBar);
      this.InitBarObject(gameObject, Quest00227Menu.BarType.TotalScore, progress);
      this.scroll.Add(gameObject);
    }
    QuestScoreCampaignProgressScore_achivement_rewards[] achivementRewards = progress.score_achivement_rewards;
    for (int index = 0; index < achivementRewards.Length; ++index)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.QuestBar);
      this.InitBarObject(gameObject, Quest00227Menu.BarType.Quest, progress, index);
      this.scroll.Add(gameObject);
    }
    if (!progress.score_ranking_disabled)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.RankingBar);
      this.InitBarObject(gameObject, Quest00227Menu.BarType.Ranking, progress);
      this.scroll.Add(gameObject);
    }
    this.scroll.ResolvePosition();
  }

  private void InitBarObject(
    GameObject obj,
    Quest00227Menu.BarType type,
    QuestScoreCampaignProgress progress,
    int index = 0)
  {
    int rank = 0;
    UIButton componentInChildren1 = obj.GetComponentInChildren<UIButton>();
    UILabel componentInChildren2 = obj.GetComponentInChildren<UILabel>();
    switch (type)
    {
      case Quest00227Menu.BarType.Quest:
        QuestScoreCampaignProgressScore_achivement_rewards achivementReward = progress.score_achivement_rewards[index];
        QuestExtraM m;
        if (!MasterData.QuestExtraM.TryGetValue(achivementReward.quest_extra_m, out m))
          break;
        rank = progress.GetQuestMScoreFromMID(m.ID);
        EventDelegate.Add(componentInChildren1.onClick, (EventDelegate.Callback) (() => Quest002272Scene.ChangeScene(true, achivementReward, progress.player.score_achivement_reward_cleared, m.name, rank)));
        componentInChildren2.SetTextLocalize(m.name);
        break;
      case Quest00227Menu.BarType.Ranking:
        rank = progress.player.rank;
        EventDelegate.Add(componentInChildren1.onClick, (EventDelegate.Callback) (() => Quest002271Scene.ChangeScene(true, progress, Consts.GetInstance().QUEST_00227_RANKING_BARNAME, rank)));
        componentInChildren2.SetTextLocalize(Consts.GetInstance().QUEST_00227_RANKING_BARNAME);
        break;
      case Quest00227Menu.BarType.TotalScore:
        EventDelegate.Add(componentInChildren1.onClick, (EventDelegate.Callback) (() => Quest002272Scene.ChangeScene(true, progress.score_total_rewards, Consts.GetInstance().QUEST_00227_TOTALSCORE_BARNAME, progress.player.score_total)));
        componentInChildren2.SetTextLocalize(Consts.GetInstance().QUEST_00227_TOTALSCORE_BARNAME);
        break;
    }
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  private enum BarType
  {
    Quest,
    Ranking,
    TotalScore,
  }
}
