// Decompiled with JetBrains decompiler
// Type: ExploreRankingRewardPopupSequence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ExploreRankingRewardPopupSequence : MonoBehaviour
{
  private GameObject exploreRankingResultAnimPrefab;
  private GameObject exploreResultRewardPrefab;
  private int period_id;
  private int rank;
  private int defeat;
  private int floor;
  private Dictionary<int, List<Versus02612ScrollRewardBox.RewardData>> rewardsList = new Dictionary<int, List<Versus02612ScrollRewardBox.RewardData>>();

  public void addRewardData(
    int condition_id,
    MasterDataTable.CommonRewardType reward_type,
    int reward_id,
    int reward_quantity)
  {
    Versus02612ScrollRewardBox.RewardData rewardData = new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = reward_id,
      type = reward_type,
      txt = CommonRewardType.GetRewardName(reward_type, reward_id, reward_quantity)
    };
    if (this.rewardsList.ContainsKey(condition_id))
    {
      this.rewardsList[condition_id].Add(rewardData);
    }
    else
    {
      List<Versus02612ScrollRewardBox.RewardData> rewardDataList = new List<Versus02612ScrollRewardBox.RewardData>()
      {
        rewardData
      };
      this.rewardsList.Add(condition_id, rewardDataList);
    }
  }

  public IEnumerator Init(int period_id, int rank, int defeat, int floor)
  {
    this.rewardsList.Clear();
    this.period_id = period_id;
    this.rank = rank;
    this.defeat = defeat;
    this.floor = floor;
    yield return (object) this.LoadPopupPrefab();
  }

  public IEnumerator InitTest()
  {
    yield return (object) this.Init(1, 543210, 12345, 678);
    foreach (int condition_id in new List<int>()
    {
      MasterData.ExploreRankingReward[1].ID,
      MasterData.ExploreRankingReward[2].ID,
      MasterData.ExploreRankingReward[3].ID,
      MasterData.ExploreRankingReward[4].ID,
      MasterData.ExploreRankingReward[5].ID,
      MasterData.ExploreRankingReward[6].ID
    })
    {
      this.addRewardData(condition_id, MasterDataTable.CommonRewardType.unit, 100111, 1);
      this.addRewardData(condition_id, MasterDataTable.CommonRewardType.unit, 100111, 2);
    }
  }

  private IEnumerator LoadPopupPrefab()
  {
    Future<GameObject> f;
    IEnumerator e;
    if (Object.op_Equality((Object) this.exploreRankingResultAnimPrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/explore033_Top/explore_RankingResult").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.exploreRankingResultAnimPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.exploreResultRewardPrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/explore033_Top/explore_RankingReward").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.exploreResultRewardPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  public IEnumerator Run()
  {
    if (this.rank > 0)
    {
      ExploreRankingPeriod period_data = MasterData.ExploreRankingPeriod[this.period_id];
      Singleton<PopupManager>.GetInstance().open((GameObject) null);
      GameObject prefab = this.exploreRankingResultAnimPrefab.Clone();
      prefab.SetActive(false);
      prefab.GetComponent<ExploreRankingResultAnim>().Initialize(period_data, this.rank, this.defeat, this.floor);
      prefab.SetActive(true);
      GameObject obj1 = Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      while (!Object.op_Equality((Object) obj1, (Object) null))
        yield return (object) null;
      obj1 = (GameObject) null;
      if (this.rewardsList.Any<KeyValuePair<int, List<Versus02612ScrollRewardBox.RewardData>>>())
      {
        obj1 = this.exploreResultRewardPrefab.Clone();
        obj1.SetActive(false);
        ExploreRankingResultReward rankingResultPopup = obj1.GetComponent<ExploreRankingResultReward>();
        IEnumerator e = rankingResultPopup.Initialize(this.rewardsList);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        obj1.SetActive(true);
        rankingResultPopup.scrollResolvePosition();
        GameObject obj2 = Singleton<PopupManager>.GetInstance().open(obj1, isCloned: true);
        while (!Object.op_Equality((Object) obj2, (Object) null))
          yield return (object) null;
        obj1 = (GameObject) null;
        rankingResultPopup = (ExploreRankingResultReward) null;
        obj2 = (GameObject) null;
      }
      Singleton<PopupManager>.GetInstance().dismiss();
    }
  }
}
