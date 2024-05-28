// Decompiled with JetBrains decompiler
// Type: Quest00230Menu
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
public class Quest00230Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtTitleLabe;
  [SerializeField]
  private UI2DSprite slcEventAnimFade01;
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private GameObject dirHuntingSolo;
  [SerializeField]
  private UILabel txtPlayerPointTitleLabe;
  [SerializeField]
  private UILabel txtPlayerPointLabe;
  [SerializeField]
  private GameObject dirHuntingTotal;
  [SerializeField]
  private UILabel txtTotalPointTitleLabe;
  [SerializeField]
  private UILabel txtTotalPointLabe;
  [SerializeField]
  private GameObject dirHuntingGuildTotal;
  [SerializeField]
  private UILabel txtGuildTotalPointTitleLabe;
  [SerializeField]
  private UILabel txtGuildTotalPointLabe;
  [SerializeField]
  private UILabel txtEventTermLabel;
  [SerializeField]
  private UILabel txtRewardReciveTermLabel;
  [SerializeField]
  private GameObject dirRewardReachedEnd;
  [SerializeField]
  private GameObject dirRewardRecieveTime;
  [SerializeField]
  private GameObject dirPointBonus;
  [SerializeField]
  private NGHorizontalScrollParts indicatorContainerAnimFade01;
  [SerializeField]
  private Quest00230NextRewardObject[] nextRewardObjects;
  [SerializeField]
  private BattleUI05PunitiveExpeditionResultMenu battleUI05PunitiveExpeditionResultMenu;
  private GameObject targetUnitPrefab;
  private GameObject targetUnitDetailPrefab;
  private GameObject targetUnitQuestListPrefab;
  private bool isInit = true;
  private EventInfo eventInfo;
  private WebAPI.Response.EventTop eventTopInfo;
  private Description description;
  [SerializeField]
  private GameObject dir_GuildFind;
  private PunitiveExpeditionEventReward[] rewardList;
  private List<int> enableNextRewardObjectIndexList;
  private int nowDisplayNextRewardIndex;
  private const int NEXT_REWARD_OBJECT_INDEX_ALL = 0;
  private const int NEXT_REWARD_OBJECT_INDEX_PERSONAL = 1;
  private const int NEXT_REWARD_OBJECT_INDEX_GUILD = 2;
  private const int TARGET_ICON_MIN = 3;
  private bool isGuild;

  protected override void Update() => base.Update();

  public override void onBackButton()
  {
    if (Singleton<PopupManager>.GetInstance().isOpen)
      Singleton<PopupManager>.GetInstance().dismiss();
    else if (Singleton<CommonRoot>.GetInstance().guildChatManager.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.DetailedView)
      Singleton<CommonRoot>.GetInstance().guildChatManager.OnBackButtonClicked();
    else
      this.IbtnBack();
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void IbtnHelp()
  {
    if (this.IsPushAndSet())
      return;
    Quest00228Scene.ChangeScene(this.description, true);
  }

  public void IbtnLeftArrow()
  {
    if (this.indicatorContainerAnimFade01.selected <= 0)
      return;
    this.indicatorContainerAnimFade01.setItemPosition(this.indicatorContainerAnimFade01.selected - 1);
  }

  public void IbtnRightArrow()
  {
    if (this.indicatorContainerAnimFade01.selected >= this.indicatorContainerAnimFade01.PartsCnt)
      return;
    this.indicatorContainerAnimFade01.setItemPosition(this.indicatorContainerAnimFade01.selected + 1);
  }

  public void IbtnRewardList()
  {
    if (this.IsPushAndSet())
      return;
    Quest00231Scene.ChangeScene(this.eventTopInfo, this.rewardList, true);
  }

  public void ShowTargetDetail(EnemyTopInfo[] infos)
  {
    this.StartCoroutine(this.ShowTargetDetailAsync(infos));
  }

  private IEnumerator ShowTargetDetailAsync(EnemyTopInfo[] infos)
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.EventDetail> receive = WebAPI.EventDetail(this.eventInfo.period_id, ((IEnumerable<EnemyTopInfo>) infos).Select<EnemyTopInfo, int>((Func<EnemyTopInfo, int>) (x => x.unit_id)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(error);
    }));
    IEnumerator e = receive.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (receive.Result != null)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      yield return (object) new WaitForSeconds(0.1f);
      GameObject prefab = this.targetUnitDetailPrefab.Clone();
      prefab.SetActive(false);
      e = prefab.GetComponent<Popup00230HuntingTargetDetail>().Init(receive.Result, infos, this.targetUnitQuestListPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      prefab.SetActive(true);
      prefab.GetComponent<Popup00230HuntingTargetDetail>().ResetScrollPosition();
    }
  }

  public IEnumerator Init(EventInfo eventInfo, WebAPI.Response.EventTop eventTopInfo)
  {
    Quest00230Menu quest00230Menu = this;
    if (quest00230Menu.isInit)
    {
      quest00230Menu.isInit = false;
      quest00230Menu.eventInfo = eventInfo;
      quest00230Menu.eventTopInfo = eventTopInfo;
      quest00230Menu.isGuild = quest00230Menu.eventInfo.IsGuild();
      quest00230Menu.description = eventTopInfo.description;
      List<PunitiveExpeditionEventReward> list = MasterData.PunitiveExpeditionEventReward.Where<KeyValuePair<int, PunitiveExpeditionEventReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, bool>) (x => x.Value.period == eventTopInfo.period_id)).Select<KeyValuePair<int, PunitiveExpeditionEventReward>, PunitiveExpeditionEventReward>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, PunitiveExpeditionEventReward>) (x => x.Value)).ToList<PunitiveExpeditionEventReward>();
      foreach (PunitiveExpeditionEventGuildReward guildReward in MasterData.PunitiveExpeditionEventGuildReward.Where<KeyValuePair<int, PunitiveExpeditionEventGuildReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, bool>) (x => x.Value.period == eventTopInfo.period_id)).Select<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, PunitiveExpeditionEventGuildReward>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, PunitiveExpeditionEventGuildReward>) (x => x.Value)).ToArray<PunitiveExpeditionEventGuildReward>())
      {
        PunitiveExpeditionEventReward expeditionEventReward = new PunitiveExpeditionEventReward();
        expeditionEventReward.ConvertGuildReward(guildReward);
        list.Add(expeditionEventReward);
      }
      quest00230Menu.rewardList = list.ToArray();
      IEnumerator e = Singleton<NGGameDataManager>.GetInstance().GetWebImage(eventTopInfo.top_image_url, quest00230Menu.slcEventAnimFade01);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      quest00230Menu.dirHuntingSolo.SetActive(true);
      quest00230Menu.txtPlayerPointLabe.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) string.Format("{0:#,0}", (object) eventTopInfo.player_point)
        }
      }));
      if (quest00230Menu.isGuild)
      {
        quest00230Menu.dirHuntingGuildTotal.SetActive(true);
        quest00230Menu.dirHuntingTotal.SetActive(false);
        quest00230Menu.txtGuildTotalPointLabe.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
        {
          {
            (object) "point",
            (object) string.Format("{0:#,0}", (object) eventTopInfo.guild_point)
          }
        }));
      }
      else
      {
        quest00230Menu.dirHuntingTotal.SetActive(true);
        quest00230Menu.dirHuntingGuildTotal.SetActive(false);
        quest00230Menu.txtTotalPointLabe.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
        {
          {
            (object) "point",
            (object) string.Format("{0:#,0}", (object) eventTopInfo.all_player_point)
          }
        }));
      }
      quest00230Menu.txtEventTermLabel.SetTextLocalize(string.Format(Consts.GetInstance().QUEST_00230_EVENT_TERM, (object) eventTopInfo.end_at));
      quest00230Menu.txtRewardReciveTermLabel.SetTextLocalize(string.Format(Consts.GetInstance().QUEST_00230_EVENT_TERM, (object) eventTopInfo.final_at));
      Future<GameObject> prefabF;
      if (Object.op_Equality((Object) quest00230Menu.targetUnitPrefab, (Object) null))
      {
        prefabF = Res.Prefabs.quest002_30.dir_unit_wanted.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        quest00230Menu.targetUnitPrefab = prefabF.Result;
        prefabF = (Future<GameObject>) null;
      }
      quest00230Menu.dirPointBonus.SetActive(false);
      if (eventTopInfo.is_bonus_term)
        quest00230Menu.dirPointBonus.SetActive(true);
      if (quest00230Menu.isGuild && !PlayerAffiliation.Current.isGuildMember())
      {
        ((IEnumerable<Quest00230NextRewardObject>) quest00230Menu.nextRewardObjects).ForEach<Quest00230NextRewardObject>((Action<Quest00230NextRewardObject>) (x => ((Component) x).gameObject.SetActive(false)));
        quest00230Menu.dirRewardRecieveTime.SetActive(false);
        quest00230Menu.dir_GuildFind.SetActive(true);
      }
      else
      {
        e = ServerTime.WaitSync();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (ServerTime.NowAppTime() < eventTopInfo.end_at)
        {
          quest00230Menu.dirRewardRecieveTime.SetActive(false);
          quest00230Menu.dir_GuildFind.SetActive(false);
          List<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>> nextRewardList = ((IEnumerable<PunitiveExpeditionEventReward>) quest00230Menu.rewardList).Where<PunitiveExpeditionEventReward>((Func<PunitiveExpeditionEventReward, bool>) (x => !((IEnumerable<int>) eventTopInfo.reward_history_ids).Any<int>((Func<int, bool>) (y => y == x.ID)))).GroupBy<PunitiveExpeditionEventReward, Tuple<EventPointType, int, int>>((Func<PunitiveExpeditionEventReward, Tuple<EventPointType, int, int>>) (x => new Tuple<EventPointType, int, int>(x.point_type, x.point, x.must_point))).ToList<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>();
          e = quest00230Menu.nextRewardObjects[0].Init(eventTopInfo, nextRewardList, EventPointType.all, new Action(quest00230Menu.NextRewardDisplayWait), new Action(quest00230Menu.NextRewardDisplayStart));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = quest00230Menu.nextRewardObjects[1].Init(eventTopInfo, nextRewardList, EventPointType.personal, new Action(quest00230Menu.NextRewardDisplayWait), new Action(quest00230Menu.NextRewardDisplayStart));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          e = quest00230Menu.nextRewardObjects[2].Init(eventTopInfo, nextRewardList, EventPointType.guild, new Action(quest00230Menu.NextRewardDisplayWait), new Action(quest00230Menu.NextRewardDisplayStart));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          quest00230Menu.enableNextRewardObjectIndexList = new List<int>();
          int length = quest00230Menu.nextRewardObjects.Length;
          for (int index = 0; index < length; ++index)
          {
            if (quest00230Menu.nextRewardObjects[index].Enable)
              quest00230Menu.enableNextRewardObjectIndexList.Add(index);
          }
          quest00230Menu.nowDisplayNextRewardIndex = -1;
          if (quest00230Menu.enableNextRewardObjectIndexList.Count<int>() > 0)
          {
            quest00230Menu.nowDisplayNextRewardIndex = 0;
            quest00230Menu.nextRewardObjects[quest00230Menu.enableNextRewardObjectIndexList[quest00230Menu.nowDisplayNextRewardIndex]].Display();
          }
          if (quest00230Menu.nowDisplayNextRewardIndex == -1)
            quest00230Menu.dirRewardReachedEnd.SetActive(true);
          nextRewardList = (List<IGrouping<Tuple<EventPointType, int, int>, PunitiveExpeditionEventReward>>) null;
        }
        else
        {
          quest00230Menu.dirRewardRecieveTime.SetActive(true);
          ((IEnumerable<Quest00230NextRewardObject>) quest00230Menu.nextRewardObjects).ForEach<Quest00230NextRewardObject>((Action<Quest00230NextRewardObject>) (x => ((Component) x).gameObject.SetActive(false)));
        }
      }
      if (Object.op_Equality((Object) quest00230Menu.targetUnitDetailPrefab, (Object) null))
      {
        prefabF = Res.Prefabs.popup.popup_002_hunting_target_detail__anim_popup01.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        quest00230Menu.targetUnitDetailPrefab = prefabF.Result;
        prefabF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) quest00230Menu.targetUnitQuestListPrefab, (Object) null))
      {
        prefabF = Res.Prefabs.quest002_hunting.dir_target_quest_list.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        quest00230Menu.targetUnitQuestListPrefab = prefabF.Result;
        prefabF = (Future<GameObject>) null;
      }
      quest00230Menu.indicatorContainerAnimFade01.destroyParts();
      int targetCnt = 0;
      if (eventTopInfo.enemy_infos != null && eventTopInfo.enemy_infos.Length != 0)
      {
        IEnumerable<IGrouping<int, EnemyTopInfo>> source1 = eventTopInfo.enemy_infos.GroupBy();
        targetCnt = source1.Count<IGrouping<int, EnemyTopInfo>>();
        foreach (IGrouping<int, EnemyTopInfo> source2 in source1)
        {
          e = quest00230Menu.indicatorContainerAnimFade01.instantiateParts(quest00230Menu.targetUnitPrefab).GetComponent<UnitWanted>().Init(source2.ToArray<EnemyTopInfo>(), new Action<EnemyTopInfo[]>(quest00230Menu.ShowTargetDetail));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      if (targetCnt < 3)
      {
        for (int i = targetCnt; i < 3; ++i)
        {
          e = quest00230Menu.indicatorContainerAnimFade01.instantiateParts(quest00230Menu.targetUnitPrefab).GetComponent<UnitWanted>().Init((EnemyTopInfo[]) null, new Action<EnemyTopInfo[]>(quest00230Menu.ShowTargetDetail));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      quest00230Menu.indicatorContainerAnimFade01.resetScrollView();
      if (eventTopInfo.get_reward_ids != null && eventTopInfo.get_reward_ids.Length != 0 || eventTopInfo.get_guild_reward_ids != null && eventTopInfo.get_guild_reward_ids.Length != 0)
      {
        e = quest00230Menu.battleUI05PunitiveExpeditionResultMenu.Init(eventTopInfo);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = quest00230Menu.battleUI05PunitiveExpeditionResultMenu.Run();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private void NextRewardDisplay()
  {
    int num = this.nowDisplayNextRewardIndex + 1;
    if (num >= this.enableNextRewardObjectIndexList.Count<int>())
      num = 0;
    if (num == this.nowDisplayNextRewardIndex)
      return;
    this.nextRewardObjects[this.enableNextRewardObjectIndexList[this.nowDisplayNextRewardIndex]].Hidden();
    this.nowDisplayNextRewardIndex = num;
  }

  private void NextRewardDisplayWait() => this.Invoke("NextRewardDisplay", 5f);

  private void NextRewardDisplayStart()
  {
    this.nextRewardObjects[this.enableNextRewardObjectIndexList[this.nowDisplayNextRewardIndex]].Display();
  }

  public void BtnGuildFind()
  {
    if (this.IsPushAndSet())
      return;
    Guild02811Scene.ChangeScene();
  }
}
