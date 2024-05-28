// Decompiled with JetBrains decompiler
// Type: GuildRaidRankingRewardPopupSequence
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
public class GuildRaidRankingRewardPopupSequence : MonoBehaviour
{
  private GameObject raidRankingResultAnimPrefab;
  private GameObject raidResultRewardPrefab;
  private int period_id;
  private int ranking;
  private string guild_name;
  private long pt_damage;
  private Dictionary<int, List<Versus02612ScrollRewardBox.RewardData>> rewardsList = new Dictionary<int, List<Versus02612ScrollRewardBox.RewardData>>();

  public void addRewardData(
    int condition_id,
    MasterDataTable.CommonRewardType reward_type,
    int reward_id,
    int reward_quantity,
    bool isGuild = false)
  {
    Versus02612ScrollRewardBox.RewardData rewardData = new Versus02612ScrollRewardBox.RewardData()
    {
      rewardID = reward_id,
      type = reward_type,
      txt = CommonRewardType.GetRewardName(reward_type, reward_id, reward_quantity, isGuild),
      isGuildReward = isGuild
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

  public IEnumerator Init(int period_id, int ranking, string guild_name, long pt_damage)
  {
    this.rewardsList.Clear();
    this.period_id = period_id;
    this.ranking = ranking;
    this.guild_name = guild_name;
    this.pt_damage = pt_damage;
    yield return (object) this.LoadPopupPrefab();
  }

  public IEnumerator InitTest()
  {
    yield return (object) this.Init(101, 543210, "ギルドのおなまえ", 12345L);
    foreach (int condition_id in new List<int>()
    {
      MasterData.GuildRaidRankingRewardConditionList[0].ID,
      MasterData.GuildRaidRankingRewardConditionList[1].ID,
      MasterData.GuildRaidRankingRewardConditionList[2].ID
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
    if (Object.op_Equality((Object) this.raidRankingResultAnimPrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/raid032_rankingresult/dir_raid_Rankingresult_anim").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.raidRankingResultAnimPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.raidResultRewardPrefab, (Object) null))
    {
      f = new ResourceObject("Prefabs/raid032_rankingresult/Raid_RankingResult").Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.raidResultRewardPrefab = f.Result;
      f = (Future<GameObject>) null;
    }
  }

  public IEnumerator Run()
  {
    if (this.ranking > 0)
    {
      GuildRaidPeriod periodData = MasterData.GuildRaidPeriod[this.period_id];
      Singleton<PopupManager>.GetInstance().open((GameObject) null);
      GameObject prefab = this.raidRankingResultAnimPrefab.Clone();
      prefab.SetActive(false);
      GuildRaidRankingResultAnim rankingResultAnim = prefab.GetComponent<GuildRaidRankingResultAnim>();
      rankingResultAnim.Initialize(periodData.period_name, this.ranking);
      prefab.SetActive(true);
      GameObject obj1 = Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      yield return (object) new WaitForSecondsRealtime(3f);
      rankingResultAnim.setBtnNextEnable(true);
      while (!Object.op_Equality((Object) obj1, (Object) null))
        yield return (object) null;
      rankingResultAnim = (GuildRaidRankingResultAnim) null;
      obj1 = (GameObject) null;
      if (this.rewardsList.Any<KeyValuePair<int, List<Versus02612ScrollRewardBox.RewardData>>>())
      {
        obj1 = this.raidResultRewardPrefab.Clone();
        obj1.SetActive(false);
        IEnumerator e = obj1.GetComponent<GuildRaidResultReward>().Initialize(periodData, this.guild_name, this.ranking, this.pt_damage, this.rewardsList);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        obj1.SetActive(true);
        GameObject obj2 = Singleton<PopupManager>.GetInstance().open(obj1, isCloned: true);
        while (!Object.op_Equality((Object) obj2, (Object) null))
          yield return (object) null;
        obj1 = (GameObject) null;
        obj2 = (GameObject) null;
      }
      Singleton<PopupManager>.GetInstance().dismiss();
    }
  }
}
