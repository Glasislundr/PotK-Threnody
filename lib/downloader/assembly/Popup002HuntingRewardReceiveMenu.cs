// Decompiled with JetBrains decompiler
// Type: Popup002HuntingRewardReceiveMenu
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
public class Popup002HuntingRewardReceiveMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject slcTouchToNext;
  [SerializeField]
  private UILabel txt_CharaEXP;
  [SerializeField]
  private GameObject dir_GuildHuntingPt;
  [SerializeField]
  private UILabel txt_GuildHuntingPt;
  [SerializeField]
  private GameObject dir_AllPlayerTotalHuntingPt;
  [SerializeField]
  private UILabel txt_AllPlayerTotalHuntingPt;
  [SerializeField]
  private UILabel txt_PlayerTotalHuntingPt;
  [SerializeField]
  private UILabel txt_PlayerTotalHunting;
  [SerializeField]
  private UILabel txt_CharaEXP_26;
  [SerializeField]
  private NGxScrollMasonry scroll;
  private Action tapCallback;
  private IEnumerable<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>> rewardListGroup;

  public IEnumerator Init(
    int allPlayerPoint,
    int playerPoint,
    int[] getRewardIds,
    int[] getGuildRewardIds,
    bool isGuild)
  {
    this.txt_CharaEXP.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_EXPEDITION_GET_REWARD_TITLE));
    UILabel label;
    if (isGuild)
    {
      ((UIRect) this.txt_PlayerTotalHunting).SetAnchor(((Component) this.txt_GuildHuntingPt).gameObject);
      ((UIRect) this.txt_PlayerTotalHuntingPt).SetAnchor(((Component) this.txt_GuildHuntingPt).gameObject);
      this.dir_GuildHuntingPt.SetActive(true);
      this.dir_AllPlayerTotalHuntingPt.SetActive(false);
      label = this.txt_GuildHuntingPt;
    }
    else
    {
      ((UIRect) this.txt_PlayerTotalHunting).SetAnchor(((Component) this.txt_AllPlayerTotalHuntingPt).gameObject);
      ((UIRect) this.txt_PlayerTotalHuntingPt).SetAnchor(((Component) this.txt_AllPlayerTotalHuntingPt).gameObject);
      this.dir_AllPlayerTotalHuntingPt.SetActive(true);
      this.dir_GuildHuntingPt.SetActive(false);
      label = this.txt_AllPlayerTotalHuntingPt;
    }
    label.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) allPlayerPoint
      }
    }));
    this.txt_PlayerTotalHuntingPt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) playerPoint
      }
    }));
    this.txt_CharaEXP.SetTextLocalize(Consts.Format(Consts.GetInstance().GUILD_EXPEDITION_REWARD_LIST_TITLE));
    List<PunitiveExpeditionEventReward> source = new List<PunitiveExpeditionEventReward>();
    foreach (int getRewardId in getRewardIds)
    {
      int id = getRewardId;
      KeyValuePair<int, PunitiveExpeditionEventReward> keyValuePair = MasterData.PunitiveExpeditionEventReward.FirstOrDefault<KeyValuePair<int, PunitiveExpeditionEventReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, bool>) (x => x.Value.ID == id));
      if (keyValuePair.Value != null)
        source.Add(keyValuePair.Value);
    }
    foreach (int getGuildRewardId in getGuildRewardIds)
    {
      int id = getGuildRewardId;
      PunitiveExpeditionEventReward expeditionEventReward = new PunitiveExpeditionEventReward();
      KeyValuePair<int, PunitiveExpeditionEventGuildReward> keyValuePair = MasterData.PunitiveExpeditionEventGuildReward.FirstOrDefault<KeyValuePair<int, PunitiveExpeditionEventGuildReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, bool>) (x => x.Value.ID == id));
      if (keyValuePair.Value != null)
        expeditionEventReward.ConvertGuildReward(keyValuePair.Value);
      source.Add(expeditionEventReward);
    }
    this.rewardListGroup = source.GroupBy<PunitiveExpeditionEventReward, Tuple<EventPointType, int>>((Func<PunitiveExpeditionEventReward, Tuple<EventPointType, int>>) (x => new Tuple<EventPointType, int>(x.point_type, x.point)));
    this.rewardListGroup = (IEnumerable<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>) this.rewardListGroup.OrderByDescending<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>, EventPointType>((Func<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>, EventPointType>) (x => x.Key.Item1)).ThenBy<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>, int>((Func<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>, int>) (x => x.Key.Item2)).ToList<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>();
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
    if (this.rewardListGroup != null && this.rewardListGroup.Count<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>() > 0)
    {
      foreach (IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward> source in this.rewardListGroup)
      {
        GameObject gameObject = boxPrefab.Clone();
        this.scroll.Add(gameObject);
        List<PunitiveExpeditionEventReward> list = source.ToList<PunitiveExpeditionEventReward>();
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(list, list[0].point_type == EventPointType.all, true);
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
