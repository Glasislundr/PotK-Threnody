// Decompiled with JetBrains decompiler
// Type: Raid032RankingRewardConfMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Raid032RankingRewardConfMenu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScrollMasonry scrollContainerTop100;
  [SerializeField]
  private NGxScrollMasonry scrollContainerGuild;
  [SerializeField]
  private SpreadColorButton ibtnTop100;
  [SerializeField]
  private SpreadColorButton ibtnGuild;
  private EventPointType listType = EventPointType.all;
  private GameObject marginPrefab;

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onBtnTop100()
  {
    if (this.listType == EventPointType.all)
      return;
    this.ResetScrollObject(EventPointType.all);
  }

  public void onBtnGuild()
  {
    if (this.listType == EventPointType.guild)
      return;
    this.ResetScrollObject(EventPointType.guild);
  }

  private void ResetScrollObject(EventPointType type)
  {
    this.listType = type;
    switch (this.listType)
    {
      case EventPointType.all:
        ((Component) this.scrollContainerTop100).gameObject.SetActive(true);
        ((Component) this.scrollContainerGuild).gameObject.SetActive(false);
        this.ibtnTop100.SetColor(Color.white);
        this.ibtnGuild.SetColor(Color.gray);
        break;
      default:
        ((Component) this.scrollContainerGuild).gameObject.SetActive(true);
        ((Component) this.scrollContainerTop100).gameObject.SetActive(false);
        this.ibtnGuild.SetColor(Color.white);
        this.ibtnTop100.SetColor(Color.gray);
        break;
    }
  }

  public IEnumerator Init(GuildRaid raid)
  {
    Future<GameObject> boxPrefabF = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
    IEnumerator e = boxPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject boxPrefab = boxPrefabF.Result;
    if (Object.op_Equality((Object) this.marginPrefab, (Object) null))
    {
      Future<GameObject> marginPrefabF = Res.Prefabs.versus026_12.dir_Between_Reward.Load<GameObject>();
      e = marginPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.marginPrefab = marginPrefabF.Result;
      marginPrefabF = (Future<GameObject>) null;
    }
    EventPointType useType = EventPointType.guild;
    List<GuildRaidPersonalDamageRankingReward> list1 = MasterData.GuildRaidPersonalDamageRankingReward.Where<KeyValuePair<int, GuildRaidPersonalDamageRankingReward>>((Func<KeyValuePair<int, GuildRaidPersonalDamageRankingReward>, bool>) (x => x.Value.period.ID == raid.period_id && x.Value.raid_boss_id == raid.boss_id && x.Value.ranking_type == GuildRaidRankingType.indiv_all_ranking)).Select<KeyValuePair<int, GuildRaidPersonalDamageRankingReward>, GuildRaidPersonalDamageRankingReward>((Func<KeyValuePair<int, GuildRaidPersonalDamageRankingReward>, GuildRaidPersonalDamageRankingReward>) (x => x.Value)).ToList<GuildRaidPersonalDamageRankingReward>();
    Dictionary<int, List<GuildRaidPersonalDamageRankingReward>> source1 = new Dictionary<int, List<GuildRaidPersonalDamageRankingReward>>();
    for (int index = 0; index < list1.Count; ++index)
    {
      if (!source1.ContainsKey(list1[index].condition_id.ID))
        source1.Add(list1[index].condition_id.ID, new List<GuildRaidPersonalDamageRankingReward>());
      source1[list1[index].condition_id.ID].Add(list1[index]);
    }
    if (source1 != null && source1.Count<KeyValuePair<int, List<GuildRaidPersonalDamageRankingReward>>>() > 0)
    {
      ((Component) this.scrollContainerTop100.Scroll).transform.Clear();
      this.scrollContainerTop100.Reset();
      foreach (KeyValuePair<int, List<GuildRaidPersonalDamageRankingReward>> keyValuePair in source1)
      {
        List<Versus02612ScrollRewardBox.RewardData> rewardData = new List<Versus02612ScrollRewardBox.RewardData>();
        for (int index = 0; index < keyValuePair.Value.Count; ++index)
          rewardData.Add(new Versus02612ScrollRewardBox.RewardData()
          {
            rewardID = keyValuePair.Value[index].reward_id.HasValue ? keyValuePair.Value[index].reward_id.Value : 0,
            type = keyValuePair.Value[index].reward_type,
            txt = CommonRewardType.GetRewardName(keyValuePair.Value[index].reward_type, keyValuePair.Value[index].reward_id.HasValue ? keyValuePair.Value[index].reward_id.Value : 0, keyValuePair.Value[index].reward_quantity.Value)
          });
        GameObject gameObject = boxPrefab.Clone();
        this.scrollContainerTop100.Add(gameObject);
        this.scrollContainerTop100.Add(this.marginPrefab.Clone());
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().InitRaid(rewardData, keyValuePair.Key);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.scrollContainerTop100.ResolvePosition();
    }
    List<GuildRaidPersonalDamageRankingReward> list2 = MasterData.GuildRaidPersonalDamageRankingReward.Where<KeyValuePair<int, GuildRaidPersonalDamageRankingReward>>((Func<KeyValuePair<int, GuildRaidPersonalDamageRankingReward>, bool>) (x => x.Value.period.ID == raid.period_id && x.Value.raid_boss_id == raid.boss_id && x.Value.ranking_type == GuildRaidRankingType.indiv_guild_ranking)).Select<KeyValuePair<int, GuildRaidPersonalDamageRankingReward>, GuildRaidPersonalDamageRankingReward>((Func<KeyValuePair<int, GuildRaidPersonalDamageRankingReward>, GuildRaidPersonalDamageRankingReward>) (x => x.Value)).ToList<GuildRaidPersonalDamageRankingReward>();
    Dictionary<int, List<GuildRaidPersonalDamageRankingReward>> source2 = new Dictionary<int, List<GuildRaidPersonalDamageRankingReward>>();
    for (int index = 0; index < list2.Count; ++index)
    {
      if (!source2.ContainsKey(list2[index].condition_id.ID))
        source2.Add(list2[index].condition_id.ID, new List<GuildRaidPersonalDamageRankingReward>());
      source2[list2[index].condition_id.ID].Add(list2[index]);
    }
    if (source2 != null && source2.Count<KeyValuePair<int, List<GuildRaidPersonalDamageRankingReward>>>() > 0)
    {
      ((Component) this.scrollContainerGuild.Scroll).transform.Clear();
      this.scrollContainerGuild.Reset();
      foreach (KeyValuePair<int, List<GuildRaidPersonalDamageRankingReward>> keyValuePair in source2)
      {
        List<Versus02612ScrollRewardBox.RewardData> rewardData = new List<Versus02612ScrollRewardBox.RewardData>();
        for (int index = 0; index < keyValuePair.Value.Count; ++index)
          rewardData.Add(new Versus02612ScrollRewardBox.RewardData()
          {
            rewardID = keyValuePair.Value[index].reward_id.HasValue ? keyValuePair.Value[index].reward_id.Value : 0,
            type = keyValuePair.Value[index].reward_type,
            txt = CommonRewardType.GetRewardName(keyValuePair.Value[index].reward_type, keyValuePair.Value[index].reward_id.HasValue ? keyValuePair.Value[index].reward_id.Value : 0, keyValuePair.Value[index].reward_quantity.Value)
          });
        GameObject gameObject = boxPrefab.Clone();
        this.scrollContainerGuild.Add(gameObject);
        this.scrollContainerGuild.Add(this.marginPrefab.Clone());
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().InitRaid(rewardData, keyValuePair.Key);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.scrollContainerGuild.ResolvePosition();
    }
    this.ResetScrollObject(useType);
  }
}
