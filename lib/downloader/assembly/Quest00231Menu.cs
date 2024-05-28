// Decompiled with JetBrains decompiler
// Type: Quest00231Menu
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
public class Quest00231Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScrollMasonry scrollContainerAll;
  [SerializeField]
  private NGxScrollMasonry scrollContainerPersonal;
  [SerializeField]
  private NGxScrollMasonry scrollContainerGuild;
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtRewardRecevieLimit;
  [SerializeField]
  private UILabel txtHuntingPoint;
  [SerializeField]
  private UILabel txtHuntingPointValue;
  [SerializeField]
  private UILabel txtHuntingPointAll;
  [SerializeField]
  private UILabel txtHuntingPointAllValue;
  [SerializeField]
  private UILabel txtHuntingPointGuild;
  [SerializeField]
  private UILabel txtHuntingPointGuildValue;
  [SerializeField]
  private GameObject dir_hunting_point_personal;
  [SerializeField]
  private GameObject dirHuntingPointAll;
  [SerializeField]
  private GameObject dirHuntingPointGuild;
  [SerializeField]
  private SpreadColorButton ibtnAll;
  [SerializeField]
  private SpreadColorButton ibtnPresonal;
  [SerializeField]
  private SpreadColorButton ibtnGuild;
  [SerializeField]
  private GameObject dir_none_guild_member;
  private bool isGuild;
  private EventPointType listType = EventPointType.all;
  private GameObject marginPrefab;

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onBtnAll()
  {
    if (this.listType == EventPointType.all)
      return;
    this.ResetScrollObject(EventPointType.all);
  }

  public void onBtnPersonal()
  {
    if (this.listType == EventPointType.personal)
      return;
    this.ResetScrollObject(EventPointType.personal);
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
    if (this.listType == EventPointType.all)
    {
      ((Component) this.scrollContainerAll).gameObject.SetActive(true);
      ((Component) this.scrollContainerGuild).gameObject.SetActive(false);
      ((Component) this.scrollContainerPersonal).gameObject.SetActive(false);
      this.ibtnAll.SetColor(Color.white);
      this.ibtnPresonal.SetColor(Color.gray);
    }
    else if (this.listType == EventPointType.personal)
    {
      ((Component) this.scrollContainerPersonal).gameObject.SetActive(true);
      this.ibtnPresonal.SetColor(Color.white);
      if (this.isGuild)
      {
        ((Component) this.scrollContainerGuild).gameObject.SetActive(false);
        this.ibtnGuild.SetColor(Color.gray);
      }
      else
      {
        ((Component) this.scrollContainerAll).gameObject.SetActive(false);
        this.ibtnAll.SetColor(Color.gray);
      }
    }
    else
    {
      if (this.listType != EventPointType.guild)
        return;
      ((Component) this.scrollContainerGuild).gameObject.SetActive(true);
      ((Component) this.scrollContainerAll).gameObject.SetActive(false);
      ((Component) this.scrollContainerPersonal).gameObject.SetActive(false);
      this.ibtnGuild.SetColor(Color.white);
      this.ibtnPresonal.SetColor(Color.gray);
    }
  }

  public IEnumerator Init(
    WebAPI.Response.EventTop eventInfo,
    PunitiveExpeditionEventReward[] rewardList)
  {
    this.isGuild = eventInfo.IsGuild();
    DateTime finalAt = eventInfo.final_at;
    this.txtRewardRecevieLimit.SetTextLocalize(string.Format(Consts.GetInstance().QUEST_00231_REWARD_RECIVE_LIMIT, (object) finalAt));
    this.txtHuntingPointValue.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) string.Format("{0:#,0}", (object) eventInfo.player_point)
      }
    }));
    if (this.isGuild)
    {
      ((Component) ((Component) this.ibtnGuild).transform.parent).gameObject.SetActive(true);
      this.dirHuntingPointAll.SetActive(false);
      if (PlayerAffiliation.Current.isGuildMember())
      {
        this.dirHuntingPointGuild.SetActive(true);
        this.txtHuntingPointGuildValue.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
        {
          {
            (object) "point",
            (object) string.Format("{0:#,0}", (object) eventInfo.guild_point)
          }
        }));
        ((UIRect) this.txtHuntingPoint).SetAnchor(((Component) this.txtHuntingPointGuild).gameObject);
        ((UIRect) this.txtHuntingPointValue).leftAnchor.target = ((Component) this.txtHuntingPointGuildValue).transform;
      }
      else
      {
        this.dir_hunting_point_personal.SetActive(false);
        this.dirHuntingPointGuild.SetActive(false);
        this.dir_none_guild_member.SetActive(true);
      }
    }
    else
    {
      ((Component) ((Component) this.ibtnAll).transform.parent).gameObject.SetActive(true);
      this.dirHuntingPointAll.SetActive(true);
      this.dirHuntingPointGuild.SetActive(false);
      this.txtHuntingPointAllValue.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) string.Format("{0:#,0}", (object) eventInfo.all_player_point)
        }
      }));
      ((UIRect) this.txtHuntingPoint).SetAnchor(((Component) this.txtHuntingPointAll).gameObject);
      ((UIRect) this.txtHuntingPointValue).leftAnchor.target = ((Component) this.txtHuntingPointAllValue).transform;
    }
    this.txtTitle.SetTextLocalize(Consts.GetInstance().QUEST_00231_TITLE);
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
    NGxScrollMasonry tmpScroll = this.scrollContainerAll;
    EventPointType useType = EventPointType.all;
    int usePoint = eventInfo.all_player_point;
    if (this.isGuild)
    {
      tmpScroll = this.scrollContainerGuild;
      useType = EventPointType.guild;
      usePoint = eventInfo.guild_point;
    }
    IEnumerable<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>> rewardListGroup = ((IEnumerable<PunitiveExpeditionEventReward>) rewardList).GroupBy<PunitiveExpeditionEventReward, Tuple<EventPointType, int>>((Func<PunitiveExpeditionEventReward, Tuple<EventPointType, int>>) (x => new Tuple<EventPointType, int>(x.point_type, x.point)));
    rewardListGroup = (IEnumerable<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>) rewardListGroup.OrderByDescending<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>, EventPointType>((Func<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>, EventPointType>) (x => x.Key.Item1)).ToList<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>();
    if (rewardListGroup != null && rewardListGroup.Count<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>() > 0)
    {
      List<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>> list1 = rewardListGroup.Where<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>((Func<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>, bool>) (x => x.Key.Item1 == EventPointType.personal)).ToList<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>();
      ((Component) this.scrollContainerPersonal.Scroll).transform.Clear();
      this.scrollContainerPersonal.Reset();
      foreach (IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward> grouping in list1)
      {
        IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward> group = grouping;
        GameObject gameObject = boxPrefab.Clone();
        this.scrollContainerPersonal.Add(gameObject);
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(group.ToList<PunitiveExpeditionEventReward>(), false, ((IEnumerable<int>) eventInfo.reward_history_ids).Any<int>((Func<int, bool>) (x => x == group.First<PunitiveExpeditionEventReward>().ID)) || group.First<PunitiveExpeditionEventReward>().point <= eventInfo.player_point);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.scrollContainerPersonal.Add(this.marginPrefab.Clone());
      }
      this.scrollContainerPersonal.ResolvePosition();
      List<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>> list2 = rewardListGroup.Where<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>((Func<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>, bool>) (x => x.Key.Item1 == useType)).ToList<IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward>>();
      ((Component) tmpScroll.Scroll).transform.Clear();
      tmpScroll.Reset();
      foreach (IGrouping<Tuple<EventPointType, int>, PunitiveExpeditionEventReward> source in list2)
      {
        GameObject gameObject = boxPrefab.Clone();
        tmpScroll.Add(gameObject);
        PunitiveExpeditionEventReward data = source.First<PunitiveExpeditionEventReward>();
        e = gameObject.GetComponent<Versus02612ScrollRewardBox>().Init(source.ToList<PunitiveExpeditionEventReward>(), true, ((IEnumerable<int>) eventInfo.reward_history_ids).Any<int>((Func<int, bool>) (x => x == data.ID)) || data.must_point <= eventInfo.player_point && data.point <= usePoint);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        tmpScroll.Add(this.marginPrefab.Clone());
      }
      tmpScroll.ResolvePosition();
    }
    this.ResetScrollObject(useType);
  }

  public void BtnNotGuildMember()
  {
    if (this.IsPushAndSet())
      return;
    Guild02811Scene.ChangeScene();
  }
}
