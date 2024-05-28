// Decompiled with JetBrains decompiler
// Type: Quest002271Menu
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
public class Quest002271Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtTotalPoint;
  [SerializeField]
  private NGxScrollMasonry scroll;

  public IEnumerator Initialize(QuestScoreCampaignProgress progress, string title, int rank)
  {
    this.txtTitle.SetTextLocalize(title);
    this.txtTotalPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_RANK, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (rank),
        (object) rank
      }
    }));
    this.scroll.Reset();
    ((Component) this.scroll.Scroll).gameObject.SetActive(false);
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
    IEnumerable<IGrouping<int, QuestScoreRankingReward>> source1 = ((IEnumerable<QuestScoreRankingReward>) progress.score_ranking_rewards).GroupBy<QuestScoreRankingReward, int>((Func<QuestScoreRankingReward, int>) (x => x.ranking_group_id));
    List<KeyValuePair<int, QuestExtraScoreRankingReward>> sameCampaignData = MasterData.QuestExtraScoreRankingReward.Where<KeyValuePair<int, QuestExtraScoreRankingReward>>((Func<KeyValuePair<int, QuestExtraScoreRankingReward>, bool>) (x => x.Value.campaign_id == progress.id)).ToList<KeyValuePair<int, QuestExtraScoreRankingReward>>();
    foreach (IGrouping<int, QuestScoreRankingReward> source2 in (IEnumerable<IGrouping<int, QuestScoreRankingReward>>) source1.OrderBy<IGrouping<int, QuestScoreRankingReward>, int>((Func<IGrouping<int, QuestScoreRankingReward>, int>) (x => sameCampaignData.FirstOrDefault<KeyValuePair<int, QuestExtraScoreRankingReward>>((Func<KeyValuePair<int, QuestExtraScoreRankingReward>, bool>) (y => y.Value.group_id == x.ToList<QuestScoreRankingReward>()[0].ranking_group_id)).Value.alignment)))
    {
      GameObject gameObject = boxPrefab.Clone();
      this.scroll.Add(gameObject);
      e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(source2.ToList<QuestScoreRankingReward>(), progress.id);
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
