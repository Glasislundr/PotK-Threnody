// Decompiled with JetBrains decompiler
// Type: Versus0268Menu
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
public class Versus0268Menu : ResultMenuBase
{
  [SerializeField]
  protected UILabel TxtSubTitle;
  [SerializeField]
  protected UILabel TxtAttention1;
  [SerializeField]
  protected UILabel TxtAttentionNum;
  [SerializeField]
  protected UILabel TxtCharaEXP26;
  [SerializeField]
  protected UILabel TxtRemain;
  [SerializeField]
  protected UILabel TxtRemainNum;
  [SerializeField]
  private GameObject DirTitle;
  [SerializeField]
  private GameObject[] Campaing;
  [SerializeField]
  private GameObject DirBottomMessage;
  [SerializeField]
  private GameObject DirRecord;
  [SerializeField]
  private GameObject DirClassRecord;
  [SerializeField]
  private GameObject[] DirBattleResult;
  [SerializeField]
  private GameObject DirClassUnitExp;
  [SerializeField]
  private GameObject[] DirClassUnit;
  [SerializeField]
  private GameObject ClassCampaing;
  private Versus0268Menu.PvpParam param;
  private WebAPI.Response.PvpPlayerFinish info;
  private GameObject TotalWinRewardPrefab;
  private GameObject NewEmblemRewardPrefab;
  private GameObject FirstBattleRewardPrefab;
  private GameObject CampaingPrefab1;
  private GameObject CampaingPrefab2;
  private GameObject CampaingPrefab3;
  private List<PvPEndPlayer_character_intimates_in_battle> intimates = new List<PvPEndPlayer_character_intimates_in_battle>();

  public override IEnumerator Init(WebAPI.Response.PvpPlayerFinish info)
  {
    this.info = info;
    this.SetParam();
    this.SetActiveObj();
    IEnumerator e = this.SetPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = base.Init(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void SetParam() => this.param = new Versus0268Menu.PvpParam(this.info);

  public void SetActiveObj()
  {
    this.TxtSubTitle.SetText(this.param.title);
    this.DirBottomMessage.SetActive(false);
    this.DirRecord.SetActive(false);
    this.DirClassRecord.SetActive(false);
    this.DirUnitExp.SetActive(false);
    this.ClassCampaing.SetActive(false);
    this.DirClassUnitExp.SetActive(false);
    ((IEnumerable<GameObject>) this.DirClassUnit).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    this.DirTitle.SetActive(false);
    ((IEnumerable<GameObject>) this.Campaing).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    ((IEnumerable<GameObject>) this.DirBattleResult).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
    if (!this.param.isClassMatch)
      return;
    this.Campaing[0] = this.ClassCampaing;
    this.DirUnitExp = this.DirClassUnitExp;
    this.DirUnit = this.DirClassUnit;
  }

  private IEnumerator SetPrefabs()
  {
    Future<GameObject> TotalWinRewardPrefabF;
    IEnumerator e;
    if (this.param.bonus_rewards.Length != 0)
    {
      TotalWinRewardPrefabF = Res.Prefabs.popup.popup_026_8_1__anim_popup01.Load<GameObject>();
      e = TotalWinRewardPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.TotalWinRewardPrefab = TotalWinRewardPrefabF.Result;
      TotalWinRewardPrefabF = (Future<GameObject>) null;
    }
    if (this.info.pvp_finish.new_emblems != null && this.info.pvp_finish.new_emblems.Length != 0)
    {
      TotalWinRewardPrefabF = Res.Prefabs.popup.popup_999_14__anim_popup01.Load<GameObject>();
      e = TotalWinRewardPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.NewEmblemRewardPrefab = TotalWinRewardPrefabF.Result;
      TotalWinRewardPrefabF = (Future<GameObject>) null;
    }
    if (this.param.first_battle_rewards.Length != 0)
    {
      TotalWinRewardPrefabF = Res.Prefabs.popup.popup_026_8_6__anim_popup01.Load<GameObject>();
      e = TotalWinRewardPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.FirstBattleRewardPrefab = TotalWinRewardPrefabF.Result;
      TotalWinRewardPrefabF = (Future<GameObject>) null;
    }
    if (this.param.campaign_rewards.Length != 0 || this.param.campaign_next_rewards.Length != 0)
    {
      TotalWinRewardPrefabF = Res.Prefabs.popup.popup_026_8_2_1__anim_popup01.Load<GameObject>();
      e = TotalWinRewardPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.CampaingPrefab1 = TotalWinRewardPrefabF.Result;
      Future<GameObject> CampaingPrefab2F = Res.Prefabs.popup.popup_026_8_2_2__anim_popup01.Load<GameObject>();
      e = CampaingPrefab2F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.CampaingPrefab2 = CampaingPrefab2F.Result;
      Future<GameObject> CampaingPrefab3F = Res.Prefabs.popup.popup_026_8_2_3__anim_popup01.Load<GameObject>();
      e = CampaingPrefab3F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.CampaingPrefab3 = CampaingPrefab3F.Result;
      TotalWinRewardPrefabF = (Future<GameObject>) null;
      CampaingPrefab2F = (Future<GameObject>) null;
      CampaingPrefab3F = (Future<GameObject>) null;
    }
  }

  public override IEnumerator Run()
  {
    Versus0268Menu versus0268Menu = this;
    Versus0268Menu.Runner[] runnerArray = new Versus0268Menu.Runner[11]
    {
      new Versus0268Menu.Runner(versus0268Menu.InitObjects),
      new Versus0268Menu.Runner(versus0268Menu.ShowRecord),
      new Versus0268Menu.Runner(versus0268Menu.ShowAnotherPopup),
      new Versus0268Menu.Runner(versus0268Menu.ShowTitleEXP),
      new Versus0268Menu.Runner(((ResultMenuBase) versus0268Menu).SkipPopupPlay),
      new Versus0268Menu.Runner(((ResultMenuBase) versus0268Menu).ShowUnitEXP),
      new Versus0268Menu.Runner(versus0268Menu.PvpCharacterIntimatesPopup),
      new Versus0268Menu.Runner(((ResultMenuBase) versus0268Menu).CharacterStoryPopup),
      new Versus0268Menu.Runner(versus0268Menu.ShowBottomMessage),
      new Versus0268Menu.Runner(versus0268Menu.ShowFirstBattlePopup),
      new Versus0268Menu.Runner(((ResultMenuBase) versus0268Menu).SkipPopupPlay)
    };
    for (int index = 0; index < runnerArray.Length; ++index)
    {
      IEnumerator e = runnerArray[index]();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runnerArray = (Versus0268Menu.Runner[]) null;
  }

  public override IEnumerator OnFinish()
  {
    Debug.Log((object) "onFinish");
    yield break;
  }

  private IEnumerator InitObjects()
  {
    Versus0268Menu versus0268Menu = this;
    versus0268Menu.beforeUnits = ((IEnumerable<PlayerUnit>) versus0268Menu.info.pvp_finish.before_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    versus0268Menu.afterUnits = ((IEnumerable<PlayerUnit>) versus0268Menu.info.pvp_finish.after_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    versus0268Menu.beforeGears = ((IEnumerable<PlayerItem>) versus0268Menu.info.pvp_finish.before_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    versus0268Menu.afterGears = ((IEnumerable<PlayerItem>) versus0268Menu.info.pvp_finish.after_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    versus0268Menu.intimates.Clear();
    versus0268Menu.intimates.AddRange((IEnumerable<PvPEndPlayer_character_intimates_in_battle>) versus0268Menu.info.pvp_finish.player_character_intimates_in_battle);
    versus0268Menu.unlockCharacterQuestIDS.Clear();
    if (versus0268Menu.info.pvp_finish.unlock_quests != null && versus0268Menu.info.pvp_finish.unlock_quests.Length != 0)
      versus0268Menu.unlockCharacterQuestIDS = ((IEnumerable<UnlockQuest>) versus0268Menu.info.pvp_finish.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 2)).Select<UnlockQuest, int>((Func<UnlockQuest, int>) (x => x.quest_s_id)).ToList<int>();
    versus0268Menu.unlockHarmonyQuestIDS.Clear();
    if (versus0268Menu.info.pvp_finish.unlock_quests != null && versus0268Menu.info.pvp_finish.unlock_quests.Length != 0)
      versus0268Menu.unlockHarmonyQuestIDS = ((IEnumerable<UnlockQuest>) versus0268Menu.info.pvp_finish.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 4)).Select<UnlockQuest, int>((Func<UnlockQuest, int>) (x => x.quest_s_id)).ToList<int>();
    versus0268Menu.disappearedPlayerGears.Clear();
    if (versus0268Menu.info.pvp_finish.disappeared_player_gears != null && versus0268Menu.info.pvp_finish.disappeared_player_gears.Length != 0)
    {
      foreach (int disappearedPlayerGear in versus0268Menu.info.pvp_finish.disappeared_player_gears)
      {
        if (versus0268Menu.beforeGears.ContainsKey(disappearedPlayerGear))
          versus0268Menu.disappearedPlayerGears.Add(versus0268Menu.beforeGears[disappearedPlayerGear]);
      }
    }
    versus0268Menu.DirTitle.SetActive(true);
    bool flag1 = versus0268Menu.info.pvp_finish.battle_result == 1;
    bool flag2 = versus0268Menu.info.pvp_finish.battle_result == 2;
    bool flag3 = versus0268Menu.info.pvp_finish.battle_result <= 3;
    bool flag4 = versus0268Menu.info.pvp_finish.battle_result == 4;
    bool flag5 = versus0268Menu.info.pvp_finish.battle_result == 5;
    versus0268Menu.DirBattleResult[0].SetActive(flag1);
    versus0268Menu.DirBattleResult[1].SetActive(flag2);
    versus0268Menu.DirBattleResult[2].SetActive(flag3);
    versus0268Menu.DirBattleResult[3].SetActive(flag4);
    versus0268Menu.DirBattleResult[4].SetActive(flag5);
    yield return (object) new WaitForSeconds(1f);
  }

  private IEnumerator ShowRecord()
  {
    GameObject target = (GameObject) null;
    target = !this.param.isClassMatch ? this.DirRecord : this.DirClassRecord;
    Versus0268VictoryDetail targetSet = target.GetComponent<Versus0268VictoryDetail>();
    IEnumerator e = targetSet.SetDefault(this.info, this.param);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    target.SetActive(true);
    target.GetComponent<UITweener>().PlayForward();
    yield return (object) new WaitForSeconds(0.7f + targetSet.WaitTime);
  }

  private IEnumerator ShowTitleEXP()
  {
    Versus0268Menu versus0268Menu = this;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1011", delay: 0.1f);
    if (versus0268Menu.info.campaigns != null && ((IEnumerable<Campaign>) versus0268Menu.info.campaigns).FirstOrDefault<Campaign>((Func<Campaign, bool>) (x => x.campaign_type_id == 1)) != null)
      versus0268Menu.Campaing[0].SetActive(true);
    versus0268Menu.DirUnitExp.SetActive(true);
    versus0268Menu.PlayTween(versus0268Menu.DirUnitExp);
    yield return (object) new WaitForSeconds(0.25f);
  }

  private IEnumerator ShowAnotherPopup()
  {
    GameObject popup;
    IEnumerator e;
    if (this.param.bonus_rewards.Length != 0)
    {
      popup = Singleton<PopupManager>.GetInstance().open(this.TotalWinRewardPrefab);
      popup.SetActive(false);
      Popup02681Menu o = popup.GetComponent<Popup02681Menu>();
      e = o.Init(this.param.bonus_rewards[0], this.param.win);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      bool isFinished = false;
      o.SetCallback((Action) (() => isFinished = true));
      while (!isFinished)
        yield return (object) null;
      yield return (object) new WaitForSeconds(0.6f);
      popup = (GameObject) null;
      o = (Popup02681Menu) null;
    }
    if (this.param.campaign_rewards.Length != 0 || this.param.campaign_next_rewards.Length != 0)
    {
      List<Versus0268Menu.PvpParam.CampaignReward> campaignRewardList = new List<Versus0268Menu.PvpParam.CampaignReward>();
      campaignRewardList.AddRange((IEnumerable<Versus0268Menu.PvpParam.CampaignReward>) this.param.campaign_rewards);
      List<Versus0268Menu.PvpParam.CampaignNextReward> nexts = new List<Versus0268Menu.PvpParam.CampaignNextReward>();
      nexts.AddRange((IEnumerable<Versus0268Menu.PvpParam.CampaignNextReward>) this.param.campaign_next_rewards);
      foreach (Versus0268Menu.PvpParam.CampaignReward campaignReward in campaignRewardList)
      {
        Versus0268Menu.PvpParam.CampaignReward reward = campaignReward;
        popup = (GameObject) null;
        Versus0268Menu.PvpParam.CampaignNextReward nextReward = nexts.Where<Versus0268Menu.PvpParam.CampaignNextReward>((Func<Versus0268Menu.PvpParam.CampaignNextReward, bool>) (x => x.campaign_id == reward.campaign_id)).FirstOrDefault<Versus0268Menu.PvpParam.CampaignNextReward>();
        if (nextReward != null)
        {
          popup = Singleton<PopupManager>.GetInstance().open(this.CampaingPrefab2);
          nexts.Remove(nextReward);
        }
        else
          popup = Singleton<PopupManager>.GetInstance().open(this.CampaingPrefab1);
        popup.SetActive(false);
        Popup02682MenuBase o = popup.GetComponent<Popup02682MenuBase>();
        e = o.Init(reward, nextReward);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        popup = (GameObject) null;
        o = (Popup02682MenuBase) null;
      }
      foreach (Versus0268Menu.PvpParam.CampaignNextReward nextReward in nexts)
      {
        popup = Singleton<PopupManager>.GetInstance().open(this.CampaingPrefab3);
        popup.SetActive(false);
        Popup026823Menu o = popup.GetComponent<Popup026823Menu>();
        e = o.Init((Versus0268Menu.PvpParam.CampaignReward) null, nextReward);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        popup = (GameObject) null;
        o = (Popup026823Menu) null;
      }
      yield return (object) new WaitForSeconds(0.6f);
      nexts = (List<Versus0268Menu.PvpParam.CampaignNextReward>) null;
    }
    if (this.info.pvp_finish.new_emblems != null && this.info.pvp_finish.new_emblems.Length != 0)
    {
      PlayerEmblem[] playerEmblemArray = this.info.pvp_finish.new_emblems;
      for (int index = 0; index < playerEmblemArray.Length; ++index)
      {
        PlayerEmblem emblem = playerEmblemArray[index];
        popup = Singleton<PopupManager>.GetInstance().open(this.NewEmblemRewardPrefab);
        popup.SetActive(false);
        Popup99914Menu o = popup.GetComponent<Popup99914Menu>();
        e = o.Init(emblem);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        popup = (GameObject) null;
        o = (Popup99914Menu) null;
      }
      playerEmblemArray = (PlayerEmblem[]) null;
      yield return (object) new WaitForSeconds(0.6f);
    }
  }

  private IEnumerator PvpCharacterIntimatesPopup()
  {
    Versus0268Menu versus0268Menu = this;
    GameObject CharacterIntimateUpPrefab = (GameObject) null;
    for (int i = 0; i < versus0268Menu.intimates.Count<PvPEndPlayer_character_intimates_in_battle>(); ++i)
    {
      PvPEndPlayer_character_intimates_in_battle p = versus0268Menu.intimates[i];
      if (p.after_level > p.before_level)
      {
        if (Object.op_Equality((Object) CharacterIntimateUpPrefab, (Object) null))
        {
          Future<GameObject> CharacterIntimateUpPrefabF = Res.Prefabs.battle.popup_020_22_1__anim_popup01.Load<GameObject>();
          IEnumerator e = CharacterIntimateUpPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          CharacterIntimateUpPrefab = CharacterIntimateUpPrefabF.Result;
          CharacterIntimateUpPrefabF = (Future<GameObject>) null;
        }
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1020", delay: 0.5f, seChannel: i % 2 + 1);
        UnitCharacter unitCharacter1 = MasterData.UnitCharacter[p.character_id];
        UnitCharacter unitCharacter2 = MasterData.UnitCharacter[p.target_character_id];
        Battle020221Menu component = versus0268Menu.OpenPopup(CharacterIntimateUpPrefab).GetComponent<Battle020221Menu>();
        component.Init(p.character_id, p.target_character_id, unitCharacter1.name, unitCharacter2.name, p.before_level, p.after_level);
        bool isFinished = false;
        component.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.6f);
      }
      p = (PvPEndPlayer_character_intimates_in_battle) null;
    }
  }

  private IEnumerator ShowFirstBattlePopup()
  {
    if (this.param.first_battle_rewards.Length != 0)
    {
      GameObject popup = Singleton<PopupManager>.GetInstance().open(this.FirstBattleRewardPrefab);
      popup.SetActive(false);
      Popup02686Menu o = popup.GetComponent<Popup02686Menu>();
      IEnumerator e = o.Init(this.param.first_battle_rewards[0]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      bool isFinished = false;
      o.SetCallback((Action) (() => isFinished = true));
      while (!isFinished)
        yield return (object) null;
      yield return (object) new WaitForSeconds(0.6f);
      popup = (GameObject) null;
      o = (Popup02686Menu) null;
    }
    yield return (object) new WaitForSeconds(0.1f);
  }

  private IEnumerator ShowBottomMessage()
  {
    Versus0268Menu versus0268Menu = this;
    if (versus0268Menu.info.campaigns != null && ((IEnumerable<Campaign>) versus0268Menu.info.campaigns).FirstOrDefault<Campaign>((Func<Campaign, bool>) (x => x.campaign_type_id == 2)) != null)
      versus0268Menu.Campaing[1].SetActive(true);
    int num = versus0268Menu.info.pvp_finish.remaining_times;
    if (num < 0)
      num = 0;
    versus0268Menu.TxtRemainNum.SetTextLocalize(Consts.Format(Consts.GetInstance().VERSUS_00268_REMAIN_NUM, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) num
      }
    }));
    versus0268Menu.TxtAttentionNum.SetTextLocalize(Consts.Format(Consts.GetInstance().VERSUS_00268_ATTENTION_NUM, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) versus0268Menu.info.pvp_finish.limit_times.ToString().ToConverter()
      }
    }));
    versus0268Menu.DirBottomMessage.SetActive(true);
    versus0268Menu.PlayTween(versus0268Menu.DirBottomMessage);
    yield return (object) new WaitForSeconds(0.5f);
  }

  private void Update()
  {
  }

  private delegate IEnumerator Runner();

  public class PvpParam
  {
    public Versus0268Menu.PvpParam.BonusReward[] bonus_rewards;
    public bool is_battle;
    public Bonus[] bonus;
    public PlayerHelper[] gladiators;
    public int win;
    public int draw;
    public int lose;
    public int season_win;
    public int season_draw;
    public int season_lose;
    public Player player;
    public bool target_player_is_friend;
    public bool is_first_battle;
    public bool isClassMatch;
    public string title;
    public Versus0268Menu.PvpParam.CampaignReward[] campaign_rewards;
    public Versus0268Menu.PvpParam.CampaignNextReward[] campaign_next_rewards;
    public Versus0268Menu.PvpParam.FirstBattleReward[] first_battle_rewards;

    public PvpParam(WebAPI.Response.PvpPlayerFinish finish)
    {
      int length1 = finish.bonus_rewards.Length;
      this.bonus_rewards = new Versus0268Menu.PvpParam.BonusReward[length1];
      for (int index = 0; index < length1; ++index)
      {
        if (this.bonus_rewards[index] == null)
          this.bonus_rewards[index] = new Versus0268Menu.PvpParam.BonusReward(finish.bonus_rewards[index]);
        else
          this.bonus_rewards[index].SetInfo(finish.bonus_rewards[index]);
      }
      int length2 = finish.campaign_rewards.Length;
      this.campaign_rewards = new Versus0268Menu.PvpParam.CampaignReward[length2];
      for (int index = 0; index < length2; ++index)
        this.campaign_rewards[index] = new Versus0268Menu.PvpParam.CampaignReward(finish.campaign_rewards[index]);
      int length3 = finish.campaign_next_rewards.Length;
      this.campaign_next_rewards = new Versus0268Menu.PvpParam.CampaignNextReward[length3];
      for (int index = 0; index < length3; ++index)
        this.campaign_next_rewards[index] = new Versus0268Menu.PvpParam.CampaignNextReward(finish.campaign_next_rewards[index]);
      int length4 = finish.first_battle_rewards.Length;
      this.first_battle_rewards = new Versus0268Menu.PvpParam.FirstBattleReward[length4];
      for (int index = 0; index < length4; ++index)
        this.first_battle_rewards[index] = new Versus0268Menu.PvpParam.FirstBattleReward(finish.first_battle_rewards[index]);
      this.is_battle = finish.is_battle;
      this.bonus = finish.bonus;
      this.gladiators = finish.gladiators;
      this.player = finish.player;
      this.target_player_is_friend = finish.target_player_is_friend;
      this.isClassMatch = finish.matching_type == 6;
      if (this.isClassMatch)
      {
        this.title = Consts.GetInstance().VERSUS_002610TITLE;
        this.season_win = finish.pvp_class_record.current_season_win_count;
        this.season_lose = finish.pvp_class_record.current_season_loss_count;
        this.season_draw = finish.pvp_class_record.current_season_draw_count;
        this.win = finish.pvp_class_record.pvp_record.win;
        this.lose = finish.pvp_class_record.pvp_record.loss;
        this.draw = finish.pvp_class_record.pvp_record.draw;
      }
      else
      {
        bool flag = finish.matching_type == 1 || finish.matching_type == 2 || finish.matching_type == 3;
        this.title = flag ? Consts.GetInstance().VERSUS_00262TITLE_RANDOM : Consts.GetInstance().VERSUS_00262TITLE_FRIEND;
        this.win = flag ? finish.pvp_record.win : finish.pvp_record_by_friend.win;
        this.lose = flag ? finish.pvp_record.loss : finish.pvp_record_by_friend.loss;
        this.draw = flag ? finish.pvp_record.draw : finish.pvp_record_by_friend.draw;
      }
    }

    public class BonusReward
    {
      public int reward_quantity;
      public int reward_type_id;
      public int reward_id;

      public void SetInfo(
        WebAPI.Response.PvpPlayerFinishBonus_rewards reward)
      {
        this.reward_quantity = reward.reward_quantity;
        this.reward_type_id = reward.reward_type_id;
        this.reward_id = reward.reward_id;
      }

      public BonusReward(
        WebAPI.Response.PvpPlayerFinishBonus_rewards reward)
      {
        this.SetInfo(reward);
      }
    }

    public class CampaignReward
    {
      public int reward_quantity;
      public string show_text2;
      public int reward_type_id;
      public int campaign_id;
      public string show_title;
      public string show_text;
      public int reward_id;

      public void SetInfo(
        WebAPI.Response.PvpPlayerFinishCampaign_rewards reward)
      {
        this.reward_quantity = reward.reward_quantity;
        this.show_text2 = reward.show_text2;
        this.reward_type_id = reward.reward_type_id;
        this.campaign_id = reward.campaign_id;
        this.show_title = reward.show_title;
        this.show_text = reward.show_text;
        this.reward_id = reward.reward_id;
      }

      public CampaignReward(
        WebAPI.Response.PvpPlayerFinishCampaign_rewards reward)
      {
        this.SetInfo(reward);
      }
    }

    public class CampaignNextReward
    {
      public string next_reward_title;
      public int campaign_id;
      public string next_reward_text;

      public void SetInfo(
        WebAPI.Response.PvpPlayerFinishCampaign_next_rewards reward)
      {
        this.next_reward_title = reward.next_reward_title;
        this.campaign_id = reward.campaign_id;
        this.next_reward_text = reward.next_reward_text;
      }

      public CampaignNextReward(
        WebAPI.Response.PvpPlayerFinishCampaign_next_rewards reward)
      {
        this.SetInfo(reward);
      }
    }

    public class FirstBattleReward
    {
      public int reward_type_id;
      public int reward_id;
      public string show_text;

      public FirstBattleReward(
        WebAPI.Response.PvpPlayerFinishFirst_battle_rewards reward)
      {
        this.SetInfo(reward);
      }

      public void SetInfo(
        WebAPI.Response.PvpPlayerFinishFirst_battle_rewards reward)
      {
        this.reward_type_id = reward.reward_type_id;
        this.reward_id = reward.reward_id;
        this.show_text = reward.show_text;
      }
    }
  }
}
