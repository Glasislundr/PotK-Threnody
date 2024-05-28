// Decompiled with JetBrains decompiler
// Type: Explore033RankingRewardMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Explore033RankingRewardMenu : BackButtonMenuBase
{
  private Explore033RankingRewardMenu.RewardTabType tabType = Explore033RankingRewardMenu.RewardTabType.FLOOR;
  [SerializeField]
  private NGxScrollMasonry scrollContainerRanking;
  [SerializeField]
  private NGxScrollMasonry scrollContainerFloor;
  [SerializeField]
  private UIScrollView scrollViewFloor;
  [SerializeField]
  private SpreadColorButton btnRanking;
  [SerializeField]
  private SpreadColorButton btnFloor;
  private GameObject boxPrefab;
  private GameObject marginPrefab;
  private int floorRewardIndex;
  private Dictionary<int, List<ExploreFloorReward>> floorRewardsDic = new Dictionary<int, List<ExploreFloorReward>>();

  public IEnumerator InitializeAsync()
  {
    Future<GameObject> boxPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.boxPrefab, (Object) null))
    {
      boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
      e = boxPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.boxPrefab = boxPrefabF.Result;
      boxPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.marginPrefab, (Object) null))
    {
      boxPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
      e = boxPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.marginPrefab = boxPrefabF.Result;
      boxPrefabF = (Future<GameObject>) null;
    }
    ExploreDataManager dataMgr = Singleton<ExploreDataManager>.GetInstance();
    ExploreRankingPeriod periodData = dataMgr.RankingPeriod;
    if (Object.op_Inequality((Object) this.scrollContainerRanking, (Object) null))
    {
      ((Component) this.scrollContainerRanking.Scroll).transform.Clear();
      this.scrollContainerRanking.Reset();
      if (periodData != null)
      {
        int reward_group_id = periodData.reward_group_id;
        IEnumerable<ExploreRankingReward> exploreRankingRewards = ((IEnumerable<ExploreRankingReward>) MasterData.ExploreRankingRewardList).Where<ExploreRankingReward>((Func<ExploreRankingReward, bool>) (x => x.group_id == reward_group_id));
        Dictionary<int, List<ExploreRankingReward>> dictionary = new Dictionary<int, List<ExploreRankingReward>>();
        foreach (ExploreRankingReward exploreRankingReward in exploreRankingRewards)
        {
          if (!dictionary.ContainsKey(exploreRankingReward.ranking_category_ExploreRankingCondition))
            dictionary.Add(exploreRankingReward.ranking_category_ExploreRankingCondition, new List<ExploreRankingReward>());
          dictionary[exploreRankingReward.ranking_category_ExploreRankingCondition].Add(exploreRankingReward);
        }
        foreach (KeyValuePair<int, List<ExploreRankingReward>> keyValuePair in dictionary)
        {
          List<Versus02612ScrollRewardBox.RewardData> rewardData = new List<Versus02612ScrollRewardBox.RewardData>();
          for (int index = 0; index < keyValuePair.Value.Count; ++index)
            rewardData.Add(new Versus02612ScrollRewardBox.RewardData()
            {
              rewardID = keyValuePair.Value[index].reward_id,
              type = keyValuePair.Value[index].reward_type,
              txt = CommonRewardType.GetRewardName(keyValuePair.Value[index].reward_type, keyValuePair.Value[index].reward_id, keyValuePair.Value[index].reward_quantity)
            });
          GameObject gameObject = this.boxPrefab.Clone();
          this.scrollContainerRanking.Add(gameObject);
          e = gameObject.GetComponent<Versus02612ScrollRewardBox>().InitExploreRanking(rewardData, keyValuePair.Key);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.scrollContainerRanking.Add(this.marginPrefab.Clone());
        }
      }
      this.scrollContainerRanking.ResolvePosition();
    }
    if (Object.op_Inequality((Object) this.scrollContainerFloor, (Object) null))
    {
      ((Component) this.scrollContainerFloor.Scroll).transform.Clear();
      this.scrollContainerFloor.Reset();
      int floor = MasterData.ExploreFloor[dataMgr.FrontFloorId].floor;
      int num = -1;
      this.floorRewardsDic.Clear();
      foreach (ExploreFloorReward exploreFloorReward in ((IEnumerable<ExploreFloorReward>) MasterData.ExploreFloorRewardList).Where<ExploreFloorReward>((Func<ExploreFloorReward, bool>) (x => x.period_id == periodData.ID)))
      {
        if (!this.floorRewardsDic.ContainsKey(exploreFloorReward.floor))
          this.floorRewardsDic.Add(exploreFloorReward.floor, new List<ExploreFloorReward>());
        this.floorRewardsDic[exploreFloorReward.floor].Add(exploreFloorReward);
        if (floor >= exploreFloorReward.floor)
          num = this.floorRewardsDic.Count - 1;
      }
      this.floorRewardIndex = Mathf.Min(num + 1, this.floorRewardsDic.Count - 1);
      GameObject indexObj = (GameObject) null;
      int index = 0;
      foreach (KeyValuePair<int, List<ExploreFloorReward>> keyValuePair in this.floorRewardsDic)
      {
        List<Versus02612ScrollRewardBox.RewardData> rewardData = new List<Versus02612ScrollRewardBox.RewardData>();
        foreach (ExploreFloorReward exploreFloorReward in keyValuePair.Value)
          rewardData.Add(new Versus02612ScrollRewardBox.RewardData()
          {
            rewardID = exploreFloorReward.reward_id,
            type = exploreFloorReward.reward_type,
            txt = CommonRewardType.GetRewardName(exploreFloorReward.reward_type, exploreFloorReward.reward_id, exploreFloorReward.reward_quantity)
          });
        GameObject gameObject = this.boxPrefab.Clone();
        this.scrollContainerFloor.Add(gameObject);
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().InitExploreFloor(rewardData, keyValuePair.Key, floor);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (index == this.floorRewardIndex)
          indexObj = this.scrollContainerFloor.Arr.LastOrDefault<GameObject>();
        this.scrollContainerFloor.Add(this.marginPrefab.Clone());
        ++index;
      }
      if (Object.op_Equality((Object) indexObj, (Object) null))
      {
        this.scrollContainerFloor.ResolvePosition();
      }
      else
      {
        this.scrollViewFloor.panel.clipOffset = new Vector2(0.0f, indexObj.transform.localPosition.y);
        this.scrollViewFloor.UpdateScrollbars(true);
      }
      indexObj = (GameObject) null;
    }
    yield return (object) null;
    this.ResetScrollObject(this.tabType);
    yield return (object) null;
  }

  private void ResetScrollObject(Explore033RankingRewardMenu.RewardTabType type)
  {
    this.tabType = type;
    switch (this.tabType)
    {
      case Explore033RankingRewardMenu.RewardTabType.RANKING:
        ((Component) this.scrollContainerRanking)?.gameObject.SetActive(true);
        ((Component) this.scrollContainerFloor)?.gameObject.SetActive(false);
        this.btnRanking?.SetColor(Color.white);
        this.btnFloor?.SetColor(Color.gray);
        break;
      default:
        ((Component) this.scrollContainerFloor)?.gameObject.SetActive(true);
        ((Component) this.scrollContainerRanking)?.gameObject.SetActive(false);
        this.btnFloor?.SetColor(Color.white);
        this.btnRanking?.SetColor(Color.gray);
        break;
    }
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onRankingButton()
  {
    if (this.tabType == Explore033RankingRewardMenu.RewardTabType.RANKING)
      return;
    this.ResetScrollObject(Explore033RankingRewardMenu.RewardTabType.RANKING);
  }

  public void onFloorButton()
  {
    if (this.tabType == Explore033RankingRewardMenu.RewardTabType.FLOOR)
      return;
    this.ResetScrollObject(Explore033RankingRewardMenu.RewardTabType.FLOOR);
  }

  private enum RewardTabType
  {
    RANKING,
    FLOOR,
  }
}
