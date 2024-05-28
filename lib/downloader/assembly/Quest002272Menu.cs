// Decompiled with JetBrains decompiler
// Type: Quest002272Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest002272Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtTotalPoint;
  [SerializeField]
  private UILabel txtTotalTxt;
  [SerializeField]
  private NGxScrollMasonry scroll;

  public IEnumerator Initialize(
    QuestScoreCampaignProgressScore_achivement_rewards achivement_reward,
    int[] achivement_cleard,
    string title,
    int score)
  {
    this.txtTitle.SetTextLocalize(title);
    this.txtTotalPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) score
      }
    }));
    this.txtTotalTxt.SetTextLocalize(Consts.GetInstance().RESULT_RANKING_MENU_HIGHSCOREPOINT_TEXT);
    Future<GameObject> boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
    IEnumerator e = boxPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject boxPrefab = boxPrefabF.Result;
    Future<GameObject> marginPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
    e = marginPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject marginPrefab = marginPrefabF.Result;
    foreach (IGrouping<int, QuestScoreAchivementReward> source in (IEnumerable<IGrouping<int, QuestScoreAchivementReward>>) ((IEnumerable<QuestScoreAchivementReward>) achivement_reward.rewards).GroupBy<QuestScoreAchivementReward, int>((Func<QuestScoreAchivementReward, int>) (x => x.score_needed)).OrderBy<IGrouping<int, QuestScoreAchivementReward>, int>((Func<IGrouping<int, QuestScoreAchivementReward>, int>) (y => MasterData.QuestExtraScoreAchivementReward.FirstOrDefault<KeyValuePair<int, QuestExtraScoreAchivementReward>>((Func<KeyValuePair<int, QuestExtraScoreAchivementReward>, bool>) (x => x.Value.ID == y.ToList<QuestScoreAchivementReward>()[0].id)).Value.alignement)))
    {
      GameObject gameObject = boxPrefab.Clone();
      this.scroll.Add(gameObject);
      e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(source.ToList<QuestScoreAchivementReward>(), achivement_cleard);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.scroll.Add(marginPrefab.Clone());
    }
    ((Component) this.scroll.Scroll).gameObject.SetActive(true);
    this.scroll.ResolvePosition();
  }

  public IEnumerator Initialize(QuestScoreTotalReward[] rewards, string title, int score)
  {
    this.txtTitle.SetTextLocalize(title);
    this.txtTotalPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) score
      }
    }));
    this.txtTotalTxt.SetTextLocalize(Consts.GetInstance().RESULT_RANKING_MENU_TOTALSCOREPOINT_TEXT);
    Future<GameObject> boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
    IEnumerator e = boxPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject boxPrefab = boxPrefabF.Result;
    Future<GameObject> marginPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
    e = marginPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject marginPrefab = marginPrefabF.Result;
    foreach (IGrouping<int, QuestScoreTotalReward> source in (IEnumerable<IGrouping<int, QuestScoreTotalReward>>) ((IEnumerable<QuestScoreTotalReward>) rewards).GroupBy<QuestScoreTotalReward, int>((Func<QuestScoreTotalReward, int>) (x => x.score_needed)).OrderBy<IGrouping<int, QuestScoreTotalReward>, int>((Func<IGrouping<int, QuestScoreTotalReward>, int>) (y => y.First<QuestScoreTotalReward>().score_needed)))
    {
      GameObject gameObject = boxPrefab.Clone();
      this.scroll.Add(gameObject);
      e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init((IEnumerable<QuestScoreTotalReward>) source.ToList<QuestScoreTotalReward>(), score);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.scroll.Add(marginPrefab.Clone());
    }
    ((Component) this.scroll.Scroll).gameObject.SetActive(true);
    this.scroll.ResolvePosition();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
