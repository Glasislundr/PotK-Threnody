// Decompiled with JetBrains decompiler
// Type: Popup002HuntingNextRewardReceiveMenu
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
public class Popup002HuntingNextRewardReceiveMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject slcTouchToNext;
  [SerializeField]
  private UILabel txt_CharaEXP;
  [SerializeField]
  private NGxScrollMasonry scroll;
  private Action tapCallback;
  private List<PunitiveExpeditionEventReward> earchRewardList;
  private List<PunitiveExpeditionEventReward> totalRewardList;

  public IEnumerator Init(
    int periodID,
    int allPlayerPoint,
    int playerPoint,
    int guildPoint,
    bool isGuild)
  {
    this.txt_CharaEXP.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_EXPEDITION_NEXT_REWARD_TITLE));
    List<PunitiveExpeditionEventReward> list = MasterData.PunitiveExpeditionEventReward.Where<KeyValuePair<int, PunitiveExpeditionEventReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, bool>) (x =>
    {
      if (x.Value.period != periodID)
        return false;
      if (x.Value.point_type == EventPointType.personal && x.Value.point > playerPoint || x.Value.point_type == EventPointType.all && (x.Value.point > allPlayerPoint || x.Value.must_point > playerPoint))
        return true;
      if (x.Value.point_type != EventPointType.guild)
        return false;
      return x.Value.point > guildPoint || x.Value.must_point > playerPoint;
    })).Select<KeyValuePair<int, PunitiveExpeditionEventReward>, PunitiveExpeditionEventReward>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, PunitiveExpeditionEventReward>) (x => x.Value)).ToList<PunitiveExpeditionEventReward>();
    foreach (PunitiveExpeditionEventGuildReward guildReward in MasterData.PunitiveExpeditionEventGuildReward.Where<KeyValuePair<int, PunitiveExpeditionEventGuildReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, bool>) (x => x.Value.period == periodID && x.Value.point > guildPoint)).Select<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, PunitiveExpeditionEventGuildReward>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, PunitiveExpeditionEventGuildReward>) (x => x.Value)).ToArray<PunitiveExpeditionEventGuildReward>())
    {
      PunitiveExpeditionEventReward expeditionEventReward = new PunitiveExpeditionEventReward();
      expeditionEventReward.ConvertGuildReward(guildReward);
      list.Add(expeditionEventReward);
    }
    IEnumerable<int> source1 = list.Where<PunitiveExpeditionEventReward>((Func<PunitiveExpeditionEventReward, bool>) (x => x.point_type == EventPointType.personal)).Select<PunitiveExpeditionEventReward, int>((Func<PunitiveExpeditionEventReward, int>) (x => x.point));
    int earchDataPoint = 0;
    if (source1.Count<int>() > 0)
      earchDataPoint = source1.Min();
    this.earchRewardList = list.Where<PunitiveExpeditionEventReward>((Func<PunitiveExpeditionEventReward, bool>) (x => x.point_type == EventPointType.personal && x.point == earchDataPoint)).ToList<PunitiveExpeditionEventReward>();
    IEnumerable<int> source2 = !isGuild ? list.Where<PunitiveExpeditionEventReward>((Func<PunitiveExpeditionEventReward, bool>) (x => x.point_type == EventPointType.all)).Select<PunitiveExpeditionEventReward, int>((Func<PunitiveExpeditionEventReward, int>) (x => x.point)) : list.Where<PunitiveExpeditionEventReward>((Func<PunitiveExpeditionEventReward, bool>) (x => x.point_type == EventPointType.guild)).Select<PunitiveExpeditionEventReward, int>((Func<PunitiveExpeditionEventReward, int>) (x => x.point));
    int totalDataPoint = 0;
    if (source2.Count<int>() > 0)
      totalDataPoint = source2.Min();
    this.totalRewardList = !isGuild ? list.Where<PunitiveExpeditionEventReward>((Func<PunitiveExpeditionEventReward, bool>) (x => x.point_type == EventPointType.all && x.point == totalDataPoint)).ToList<PunitiveExpeditionEventReward>() : list.Where<PunitiveExpeditionEventReward>((Func<PunitiveExpeditionEventReward, bool>) (x => x.point_type == EventPointType.guild && x.point == totalDataPoint)).ToList<PunitiveExpeditionEventReward>();
    IEnumerator e = this.StartScroll();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator StartScroll()
  {
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
    if (this.earchRewardList != null || this.totalRewardList != null)
    {
      if (this.totalRewardList != null && this.totalRewardList.Count > 0)
      {
        GameObject gameObject = boxPrefab.Clone();
        this.scroll.Add(gameObject);
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(this.totalRewardList, true, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.scroll.Add(marginPrefab.Clone());
      }
      if (this.earchRewardList != null && this.earchRewardList.Count > 0)
      {
        GameObject gameObject = boxPrefab.Clone();
        this.scroll.Add(gameObject);
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(this.earchRewardList, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.scroll.Add(marginPrefab.Clone());
      }
    }
    ((Component) this.scroll.Scroll).gameObject.SetActive(true);
  }

  public void ResetScrollPosition()
  {
    this.scroll.Scroll.verticalScrollBar.value = 0.0f;
    this.scroll.ResolvePosition();
  }

  public void SetTapCallBack(Action callback) => this.tapCallback = callback;

  public void onTapToNext()
  {
    if (this.tapCallback == null)
      return;
    this.tapCallback();
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().onDismiss();
}
