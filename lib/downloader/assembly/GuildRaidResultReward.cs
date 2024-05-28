// Decompiled with JetBrains decompiler
// Type: GuildRaidResultReward
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
public class GuildRaidResultReward : MonoBehaviour
{
  [SerializeField]
  private NGxScrollMasonry scrollContainer;
  [SerializeField]
  private UILabel txtEventName;
  [SerializeField]
  private UILabel txtGuildName;
  [SerializeField]
  private UILabel txtTotalRaidGuildRanking;
  [SerializeField]
  private UILabel txtPeriodValue;
  [SerializeField]
  private UILabel txtPtValue;
  private GameObject marginPrefab;
  private GameObject boxPrefab;

  public IEnumerator Initialize(
    GuildRaidPeriod periodData,
    string guild_name,
    int guild_ranking,
    long pt_damage,
    Dictionary<int, List<Versus02612ScrollRewardBox.RewardData>> rewardsList)
  {
    this.setHeaderInfo(periodData, guild_name, guild_ranking, pt_damage);
    IEnumerator e = this.lordResource();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.setScrollRewardBox(rewardsList);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.scrollContainer.ResolvePosition();
  }

  private void setHeaderInfo(
    GuildRaidPeriod periodData,
    string guild_name,
    int guild_ranking,
    long pt_damage)
  {
    this.txtEventName.SetTextLocalize(periodData.period_name);
    this.txtPeriodValue.SetTextLocalize(Consts.GetInstance().GUILD_RAID_PERIOD_FORMAT.F((object) periodData.start_at.Value.Month, (object) periodData.start_at.Value.Day, (object) periodData.end_at.Value.Month, (object) periodData.end_at.Value.Day));
    this.txtGuildName.SetTextLocalize(guild_name);
    this.txtTotalRaidGuildRanking.SetTextLocalize(guild_ranking);
    this.txtPtValue.SetTextLocalize(pt_damage);
  }

  private IEnumerator lordResource()
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
  }

  private IEnumerator setScrollRewardBox(
    Dictionary<int, List<Versus02612ScrollRewardBox.RewardData>> rewardsList)
  {
    if (rewardsList != null && rewardsList.Count<KeyValuePair<int, List<Versus02612ScrollRewardBox.RewardData>>>() > 0)
    {
      ((Component) this.scrollContainer.Scroll).transform.Clear();
      this.scrollContainer.Reset();
      foreach (KeyValuePair<int, List<Versus02612ScrollRewardBox.RewardData>> rewards in rewardsList)
      {
        GameObject gameObject = this.boxPrefab.Clone();
        this.scrollContainer.Add(gameObject);
        IEnumerator e = gameObject.GetComponent<Versus02612ScrollRewardBox>().InitRaid(rewards.Value, rewards.Key);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.scrollContainer.Add(this.marginPrefab.Clone());
      }
    }
  }

  public void Close() => Singleton<PopupManager>.GetInstance().dismiss();
}
