// Decompiled with JetBrains decompiler
// Type: Raid032GuildRankingRewardMenu
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
public class Raid032GuildRankingRewardMenu : BackButtonMenuBase
{
  private List<Raid032GuildRankingRewardMenu.GuildRankingReward> rewardList = new List<Raid032GuildRankingRewardMenu.GuildRankingReward>();
  public UIButton backBtn;
  public UITable body;
  private GameObject rewardBox;
  private int periodId;
  private bool isInitialized_;

  private void Start()
  {
    this.backBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.onBackButton())));
  }

  public override void onBackButton() => this.onClickedClose();

  public IEnumerator Initalize(int periodid)
  {
    this.isInitialized_ = false;
    this.periodId = periodid;
    Future<GameObject> load = Res.Prefabs.versus026_12.slc_Reward_Box.Load<GameObject>();
    IEnumerator e = load.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.rewardBox = load.Result;
    e = this.SetRewardInfo(true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.isInitialized_ = true;
  }

  private IEnumerator SetRewardInfo(bool isImmediate = false)
  {
    Raid032GuildRankingRewardMenu rankingRewardMenu = this;
    if (rankingRewardMenu.isInitialized_)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
    }
    // ISSUE: reference to a compiler-generated method
    List<GuildRaidGuildDamageRankingReward> list1 = MasterData.GuildRaidGuildDamageRankingReward.Where<KeyValuePair<int, GuildRaidGuildDamageRankingReward>>(new Func<KeyValuePair<int, GuildRaidGuildDamageRankingReward>, bool>(rankingRewardMenu.\u003CSetRewardInfo\u003Eb__10_0)).Select<KeyValuePair<int, GuildRaidGuildDamageRankingReward>, GuildRaidGuildDamageRankingReward>((Func<KeyValuePair<int, GuildRaidGuildDamageRankingReward>, GuildRaidGuildDamageRankingReward>) (x => x.Value)).ToList<GuildRaidGuildDamageRankingReward>();
    // ISSUE: reference to a compiler-generated method
    List<GuildRaidGuildDamageRankingRewardExtra> list2 = MasterData.GuildRaidGuildDamageRankingRewardExtra.Where<KeyValuePair<int, GuildRaidGuildDamageRankingRewardExtra>>(new Func<KeyValuePair<int, GuildRaidGuildDamageRankingRewardExtra>, bool>(rankingRewardMenu.\u003CSetRewardInfo\u003Eb__10_2)).Select<KeyValuePair<int, GuildRaidGuildDamageRankingRewardExtra>, GuildRaidGuildDamageRankingRewardExtra>((Func<KeyValuePair<int, GuildRaidGuildDamageRankingRewardExtra>, GuildRaidGuildDamageRankingRewardExtra>) (x => x.Value)).ToList<GuildRaidGuildDamageRankingRewardExtra>();
    Dictionary<int, List<Raid032GuildRankingRewardMenu.GuildRankingReward>> dictionary = new Dictionary<int, List<Raid032GuildRankingRewardMenu.GuildRankingReward>>();
    if (list1.Count <= 0 && list2.Count <= 0)
      Debug.LogError((object) ("該当期間IDのReward情報なし 期間ID: " + (object) rankingRewardMenu.periodId));
    for (int index = 0; index < list1.Count; ++index)
    {
      if (!dictionary.ContainsKey(list1[index].condition_id.ID))
        dictionary.Add(list1[index].condition_id.ID, new List<Raid032GuildRankingRewardMenu.GuildRankingReward>());
      dictionary[list1[index].condition_id.ID].Add(new Raid032GuildRankingRewardMenu.GuildRankingReward(list1[index].reward_id, list1[index].condition_id, list1[index].reward_type, list1[index].reward_quantity));
    }
    for (int index = 0; index < list2.Count; ++index)
    {
      if (!dictionary.ContainsKey(list2[index].condition_id.ID))
        dictionary.Add(list2[index].condition_id.ID, new List<Raid032GuildRankingRewardMenu.GuildRankingReward>());
      dictionary[list2[index].condition_id.ID].Add(new Raid032GuildRankingRewardMenu.GuildRankingReward(list2[index].reward_id, list2[index].condition_id, GuildUtil.getCommonRewardType(list2[index].reward_type), list2[index].reward_quantity));
    }
    foreach (KeyValuePair<int, List<Raid032GuildRankingRewardMenu.GuildRankingReward>> keyValuePair in dictionary)
    {
      Versus02612ScrollRewardBox component = rankingRewardMenu.rewardBox.Clone(((Component) rankingRewardMenu.body).transform).GetComponent<Versus02612ScrollRewardBox>();
      List<Versus02612ScrollRewardBox.RewardData> rewardData = new List<Versus02612ScrollRewardBox.RewardData>();
      for (int index = 0; index < keyValuePair.Value.Count; ++index)
      {
        Raid032GuildRankingRewardMenu.GuildRankingReward guildRankingReward = keyValuePair.Value[index];
        rewardData.Add(new Versus02612ScrollRewardBox.RewardData()
        {
          rewardID = keyValuePair.Value[index].id.HasValue ? keyValuePair.Value[index].id.Value : 0,
          type = keyValuePair.Value[index].type,
          txt = CommonRewardType.GetRewardName(guildRankingReward.type, guildRankingReward.id.HasValue ? guildRankingReward.id.Value : 0, guildRankingReward.quantity, guildRankingReward.type == MasterDataTable.CommonRewardType.emblem && guildRankingReward.id.HasValue && guildRankingReward.id.Value > 1000),
          isGuildReward = guildRankingReward.type == MasterDataTable.CommonRewardType.emblem && guildRankingReward.id.HasValue && guildRankingReward.id.Value > 1000
        });
      }
      IEnumerator e = component.InitRaid(rewardData, keyValuePair.Key);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    rankingRewardMenu.repositionScrollView(rankingRewardMenu.body);
    if (rankingRewardMenu.isInitialized_)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  private void repositionScrollView(UITable grid)
  {
    grid.Reposition();
    UIScrollView component = ((Component) ((Component) grid).transform.parent).GetComponent<UIScrollView>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.ResetPosition();
  }

  public void onClickedClose()
  {
    if (Singleton<CommonRoot>.GetInstance().isLoading || this.IsPushAndSet())
      return;
    this.backScene();
  }

  private class GuildRankingReward
  {
    public GuildRaidRankingRewardCondition condition;
    public int? id;
    public MasterDataTable.CommonRewardType type;
    public int quantity;

    public GuildRankingReward(
      int? _id,
      GuildRaidRankingRewardCondition _condition,
      MasterDataTable.CommonRewardType _type,
      int _quantity)
    {
      this.id = _id;
      this.condition = _condition;
      this.type = _type;
      this.quantity = _quantity;
    }
  }
}
