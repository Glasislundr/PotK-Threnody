// Decompiled with JetBrains decompiler
// Type: Colosseum02346Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Colosseum02346Menu : ResultMenuBase
{
  [SerializeField]
  private GameObject DirRankupEffect;
  [SerializeField]
  private GameObject DirBottomMessage;
  [SerializeField]
  private GameObject DirTitleExp;
  [SerializeField]
  private GameObject DirRankPoint;
  [SerializeField]
  private GameObject DirRankStatus;
  [SerializeField]
  private GameObject RankExpGauge;
  [SerializeField]
  private GameObject NextBattleEffect;
  [SerializeField]
  private UIButton touchToNext;
  [SerializeField]
  protected UILabel TxtGetPoint;
  [SerializeField]
  protected UILabel TxtLose;
  [SerializeField]
  protected UILabel TxtRankname;
  [SerializeField]
  protected UILabel TxtRankPoint;
  [SerializeField]
  protected UILabel TxtRemain;
  [SerializeField]
  protected UILabel TxtToNextRank;
  [SerializeField]
  protected UILabel TxtToNextRankPoint;
  [SerializeField]
  protected UILabel TxtWin;
  [SerializeField]
  protected UILabel TxtRemainNum;
  [SerializeField]
  protected UILabel TxtAttentionNum;
  [SerializeField]
  private GameObject[] Campaing;
  private ColosseumUtility.Info info;
  private ResultMenuBase.Param result;
  private GameObject RankBattleResultEffectPrefab;
  private GameObject NextBattleEffectPrefab;
  private GameObject TotalWinRewardPrefab;
  private GameObject NewEmblemRewardPrefab;
  private GameObject RankUpRewardPrefab;
  private GameObject NewRankNamePrefab;
  private GameObject CampaingPrefab1;
  private GameObject CampaingPrefab2;
  private GameObject CampaingPrefab3;
  private GameObject UnitPrefab;
  private GameObject GearPrefab;
  private GameObject UniquePrefab;
  private bool activeNextBattleEffect;
  private bool activeRankBattleEffect;
  private bool activeRankBattlePopupEffect;
  private int nextBattleType;
  private int rankChangeType;
  private int nowBattleType;

  public override IEnumerator Init(
    ColosseumUtility.Info info,
    ResultMenuBase.Param param,
    Gladiator gladiator)
  {
    Colosseum02346Menu colosseum02346Menu = this;
    colosseum02346Menu.info = info;
    colosseum02346Menu.result = param;
    colosseum02346Menu.nowBattleType = colosseum02346Menu.result.battle_type;
    colosseum02346Menu.nextBattleType = colosseum02346Menu.result.next_battle_type;
    colosseum02346Menu.rankChangeType = colosseum02346Menu.result.colosseum_result_rank_up.rank_change;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = colosseum02346Menu.\u003C\u003En__0(info, param, gladiator);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02346Menu.NextBattleEffectPrefab = (GameObject) null;
    Future<GameObject> NextBattleEffectF;
    if (colosseum02346Menu.nextBattleType == 1)
    {
      NextBattleEffectF = Res.Prefabs.colosseum.colosseum023_4_6.colosseum_next_rankup.Load<GameObject>();
      e = NextBattleEffectF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.NextBattleEffectPrefab = NextBattleEffectF.Result;
      NextBattleEffectF = (Future<GameObject>) null;
    }
    else if (colosseum02346Menu.nextBattleType == 2)
    {
      NextBattleEffectF = Res.Prefabs.colosseum.colosseum023_4_6.colosseum_next_rankdown.Load<GameObject>();
      e = NextBattleEffectF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.NextBattleEffectPrefab = NextBattleEffectF.Result;
      NextBattleEffectF = (Future<GameObject>) null;
    }
    if (colosseum02346Menu.result.bonus_rewards.Length != 0)
    {
      NextBattleEffectF = Res.Prefabs.popup.popup_023_4_18__anim_popup01.Load<GameObject>();
      e = NextBattleEffectF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.TotalWinRewardPrefab = NextBattleEffectF.Result;
      NextBattleEffectF = (Future<GameObject>) null;
    }
    Future<GameObject> CampaingPrefab3F;
    if (colosseum02346Menu.result.campaign_rewards.Length != 0 || colosseum02346Menu.result.campaign_next_rewards.Length != 0)
    {
      NextBattleEffectF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = NextBattleEffectF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.UnitPrefab = NextBattleEffectF.Result;
      Future<GameObject> gearPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = gearPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.GearPrefab = gearPrefabF.Result;
      Future<GameObject> uniquePrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
      e = uniquePrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.UniquePrefab = uniquePrefabF.Result;
      Future<GameObject> CampaingPrefab1F = Res.Prefabs.popup.popup_023_5_1__anim_popup01.Load<GameObject>();
      e = CampaingPrefab1F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.CampaingPrefab1 = CampaingPrefab1F.Result;
      Future<GameObject> CampaingPrefab2F = Res.Prefabs.popup.popup_023_5_2__anim_popup01.Load<GameObject>();
      e = CampaingPrefab2F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.CampaingPrefab2 = CampaingPrefab2F.Result;
      CampaingPrefab3F = Res.Prefabs.popup.popup_023_5_3__anim_popup01.Load<GameObject>();
      e = CampaingPrefab3F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.CampaingPrefab3 = CampaingPrefab3F.Result;
      NextBattleEffectF = (Future<GameObject>) null;
      gearPrefabF = (Future<GameObject>) null;
      uniquePrefabF = (Future<GameObject>) null;
      CampaingPrefab1F = (Future<GameObject>) null;
      CampaingPrefab2F = (Future<GameObject>) null;
      CampaingPrefab3F = (Future<GameObject>) null;
    }
    if (colosseum02346Menu.result.colosseum_finish.new_emblems != null && colosseum02346Menu.result.colosseum_finish.new_emblems.Length != 0)
    {
      CampaingPrefab3F = Res.Prefabs.popup.popup_999_14__anim_popup01.Load<GameObject>();
      e = CampaingPrefab3F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.NewEmblemRewardPrefab = CampaingPrefab3F.Result;
      CampaingPrefab3F = (Future<GameObject>) null;
    }
    if (colosseum02346Menu.result.colosseum_result_rank_up.rank_up_rewards.Length != 0)
    {
      CampaingPrefab3F = Res.Prefabs.popup.popup_023_4_4d__anim_popup01.Load<GameObject>();
      e = CampaingPrefab3F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      colosseum02346Menu.RankUpRewardPrefab = CampaingPrefab3F.Result;
      CampaingPrefab3F = (Future<GameObject>) null;
    }
    colosseum02346Menu.RankBattleResultEffectPrefab = (GameObject) null;
    if (colosseum02346Menu.nowBattleType != 0)
    {
      if (colosseum02346Menu.rankChangeType == 0)
      {
        CampaingPrefab3F = Res.Prefabs.colosseum.colosseum023_4_4._023_4_4c.Load<GameObject>();
        e = CampaingPrefab3F.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        colosseum02346Menu.RankBattleResultEffectPrefab = CampaingPrefab3F.Result;
        CampaingPrefab3F = (Future<GameObject>) null;
      }
      else if (colosseum02346Menu.rankChangeType == 1)
      {
        CampaingPrefab3F = Res.Prefabs.colosseum.colosseum023_4_4._023_4_4a.Load<GameObject>();
        e = CampaingPrefab3F.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        colosseum02346Menu.RankBattleResultEffectPrefab = CampaingPrefab3F.Result;
        CampaingPrefab3F = (Future<GameObject>) null;
      }
      else if (colosseum02346Menu.rankChangeType == 2)
      {
        CampaingPrefab3F = Res.Prefabs.colosseum.colosseum023_4_4._023_4_4b.Load<GameObject>();
        e = CampaingPrefab3F.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        colosseum02346Menu.RankBattleResultEffectPrefab = CampaingPrefab3F.Result;
        CampaingPrefab3F = (Future<GameObject>) null;
      }
    }
    Future<GameObject> RankNamePrefabF = Res.Prefabs.colosseum.colosseum023_4_6.colosseum_NewRankName.Load<GameObject>();
    e = RankNamePrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02346Menu.NewRankNamePrefab = RankNamePrefabF.Result;
    colosseum02346Menu.DirBottomMessage.SetActive(false);
    colosseum02346Menu.DirTitleExp.SetActive(false);
    colosseum02346Menu.DirRankPoint.SetActive(false);
    colosseum02346Menu.DirRankStatus.SetActive(false);
    colosseum02346Menu.DirUnitExp.SetActive(false);
    ((IEnumerable<GameObject>) colosseum02346Menu.Campaing).ForEach<GameObject>((Action<GameObject>) (x => x.SetActive(false)));
  }

  public void EndNextBattleEffect() => this.activeNextBattleEffect = false;

  public void EndRankBattleEffect() => this.activeRankBattleEffect = false;

  public void EndRankBattlePopupEffect() => this.activeRankBattlePopupEffect = false;

  public override IEnumerator Run()
  {
    Colosseum02346Menu colosseum02346Menu = this;
    Colosseum02346Menu.Runner[] runnerArray = new Colosseum02346Menu.Runner[12]
    {
      new Colosseum02346Menu.Runner(colosseum02346Menu.InitObjects),
      new Colosseum02346Menu.Runner(colosseum02346Menu.ShowRankUp),
      new Colosseum02346Menu.Runner(colosseum02346Menu.ShowRankInfo),
      new Colosseum02346Menu.Runner(colosseum02346Menu.ShowRankPoint),
      new Colosseum02346Menu.Runner(colosseum02346Menu.ShowTitleEXP),
      new Colosseum02346Menu.Runner(((ResultMenuBase) colosseum02346Menu).SkipPopupPlay),
      new Colosseum02346Menu.Runner(((ResultMenuBase) colosseum02346Menu).ShowUnitEXP),
      new Colosseum02346Menu.Runner(((ResultMenuBase) colosseum02346Menu).CharacterIntimatesPopup),
      new Colosseum02346Menu.Runner(((ResultMenuBase) colosseum02346Menu).CharacterStoryPopup),
      new Colosseum02346Menu.Runner(((ResultMenuBase) colosseum02346Menu).HarmonyStoryPopup),
      new Colosseum02346Menu.Runner(colosseum02346Menu.ShowBottomMessage),
      new Colosseum02346Menu.Runner(((ResultMenuBase) colosseum02346Menu).SkipPopupPlay)
    };
    for (int index = 0; index < runnerArray.Length; ++index)
    {
      IEnumerator e = runnerArray[index]();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runnerArray = (Colosseum02346Menu.Runner[]) null;
  }

  public override IEnumerator OnFinish()
  {
    Debug.Log((object) "onFinish");
    yield break;
  }

  private IEnumerator InitObjects()
  {
    Colosseum02346Menu colosseum02346Menu = this;
    colosseum02346Menu.deck_type_id = colosseum02346Menu.result.colosseum_finish.deck_type_id;
    colosseum02346Menu.deck_number = colosseum02346Menu.result.colosseum_finish.deck_number;
    colosseum02346Menu.makeDiffGearAccessoryRemainingAmounts(colosseum02346Menu.result.colosseum_finish.before_player_gears);
    if (BattleInfo.checkCustomDeck(colosseum02346Menu.deck_type_id))
    {
      colosseum02346Menu.patchPUNK_DEBUG_21142(colosseum02346Menu.result.colosseum_finish.before_player_gears, colosseum02346Menu.result.colosseum_finish.after_player_gears);
      int[] disappearedGears = colosseum02346Menu.result.colosseum_finish.disappeared_player_gears ?? new int[0];
      colosseum02346Menu.beforeUnits = ((IEnumerable<PlayerUnit>) colosseum02346Menu.createCustomDeckUnits(colosseum02346Menu.deck_number, colosseum02346Menu.result.colosseum_finish.before_player_units, ((IEnumerable<PlayerItem>) colosseum02346Menu.result.colosseum_finish.before_player_gears).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !((IEnumerable<int>) disappearedGears).Contains<int>(x.id))).ToArray<PlayerItem>())).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
      colosseum02346Menu.afterUnits = ((IEnumerable<PlayerUnit>) colosseum02346Menu.createCustomDeckUnits(colosseum02346Menu.deck_number, colosseum02346Menu.result.colosseum_finish.after_player_units, ((IEnumerable<PlayerItem>) colosseum02346Menu.result.colosseum_finish.after_player_gears).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !((IEnumerable<int>) disappearedGears).Contains<int>(x.id))).ToArray<PlayerItem>())).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    }
    else
    {
      colosseum02346Menu.beforeUnits = ((IEnumerable<PlayerUnit>) colosseum02346Menu.result.colosseum_finish.before_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
      colosseum02346Menu.afterUnits = ((IEnumerable<PlayerUnit>) colosseum02346Menu.result.colosseum_finish.after_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    }
    colosseum02346Menu.beforeGears = ((IEnumerable<PlayerItem>) colosseum02346Menu.result.colosseum_finish.before_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    colosseum02346Menu.afterGears = ((IEnumerable<PlayerItem>) colosseum02346Menu.result.colosseum_finish.after_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    colosseum02346Menu.updateLocalCustomDecks(colosseum02346Menu.result.colosseum_finish.disappeared_player_gears);
    colosseum02346Menu.characterIntimates.Clear();
    colosseum02346Menu.unlockCharacterQuestIDS.Clear();
    if (colosseum02346Menu.result.colosseum_finish.unlock_quests != null && colosseum02346Menu.result.colosseum_finish.unlock_quests.Length != 0)
      colosseum02346Menu.unlockCharacterQuestIDS = ((IEnumerable<UnlockQuest>) colosseum02346Menu.result.colosseum_finish.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 2)).Select<UnlockQuest, int>((Func<UnlockQuest, int>) (x => x.quest_s_id)).ToList<int>();
    colosseum02346Menu.disappearedPlayerGears.Clear();
    if (colosseum02346Menu.result.colosseum_finish.disappeared_player_gears != null && colosseum02346Menu.result.colosseum_finish.disappeared_player_gears.Length != 0)
    {
      foreach (int disappearedPlayerGear in colosseum02346Menu.result.colosseum_finish.disappeared_player_gears)
      {
        if (colosseum02346Menu.beforeGears.ContainsKey(disappearedPlayerGear))
          colosseum02346Menu.disappearedPlayerGears.Add(colosseum02346Menu.beforeGears[disappearedPlayerGear]);
      }
    }
    colosseum02346Menu.DirRankupEffect.SetActive(false);
    colosseum02346Menu.DirBottomMessage.SetActive(false);
    colosseum02346Menu.DirTitleExp.SetActive(false);
    colosseum02346Menu.DirRankPoint.SetActive(false);
    colosseum02346Menu.DirRankStatus.SetActive(false);
    colosseum02346Menu.DirUnitExp.SetActive(false);
    colosseum02346Menu.NextBattleEffect.SetActive(false);
    yield return (object) new WaitForSeconds(0.1f);
  }

  private IEnumerator ShowRankUp()
  {
    Colosseum02346Menu menu = this;
    if (menu.nowBattleType != 0)
    {
      if (menu.rankChangeType == 0)
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1502");
      else if (menu.rankChangeType == 1)
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1500");
      else if (menu.rankChangeType == 2)
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1501");
      ((Component) menu.touchToNext).gameObject.SetActive(false);
      menu.DirRankupEffect.SetActive(true);
      ((Component) menu.RankBattleResultEffectPrefab.Clone(menu.DirRankupEffect.transform).transform.Find("ddd")).GetComponent<ColosseumRankBattleEffect>().Init(menu);
      menu.activeRankBattleEffect = true;
      while (menu.activeRankBattleEffect)
        yield return (object) null;
      menu.NewRankNamePrefab.Clone(menu.DirRankupEffect.transform).GetComponent<ColosseumNewRankName>().Init(menu, menu.result.colosseum_result_rank_up, menu.nowBattleType);
      menu.activeRankBattlePopupEffect = true;
      while (menu.activeRankBattlePopupEffect)
        yield return (object) null;
      if (menu.result.colosseum_result_rank_up.rank_up_rewards.Length != 0)
      {
        GameObject popup = Singleton<PopupManager>.GetInstance().open(menu.RankUpRewardPrefab);
        popup.SetActive(false);
        Popup02344dMenu o = popup.GetComponent<Popup02344dMenu>();
        IEnumerator e = o.Init(menu.result.colosseum_result_rank_up.rank_up_rewards);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        popup.SetActive(true);
        while (!isFinished)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.6f);
        popup = (GameObject) null;
        o = (Popup02344dMenu) null;
      }
      menu.DirRankupEffect.SetActive(false);
      yield return (object) new WaitForSeconds(0.6f);
      ((Component) menu.touchToNext).gameObject.SetActive(true);
    }
  }

  private IEnumerator ShowRankInfo()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Colosseum02346Menu colosseum02346Menu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    colosseum02346Menu.TxtRankname.SetText(ColosseumUtility.GetRankName(colosseum02346Menu.result.colosseum_record.rank_pt));
    colosseum02346Menu.TxtWin.SetTextLocalize(colosseum02346Menu.result.colosseum_record.attack_win);
    colosseum02346Menu.TxtLose.SetTextLocalize(colosseum02346Menu.result.colosseum_record.attack_lose);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1012");
    colosseum02346Menu.PlayTween(colosseum02346Menu.DirRankStatus);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.9f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator ShowRankPoint()
  {
    Colosseum02346Menu menu = this;
    int enabledMaxRankPoint = menu.result.colosseum_record.enabled_max_rank_point;
    int beforePoint = menu.result.colosseum_result_rank_up.before_rank_pt;
    int afterPoint = menu.result.colosseum_result_rank_up.after_rank_pt;
    int beforeID = ColosseumUtility.GetRankID(beforePoint);
    int afterID = ColosseumUtility.GetRankID(afterPoint);
    float before = ColosseumUtility.GetNextRankRate(beforePoint, enabledMaxRankPoint);
    float after = ColosseumUtility.GetNextRankRate(afterPoint, enabledMaxRankPoint);
    int loop = Math.Abs(afterID - beforeID);
    IEnumerator e = GaugeRunner.Run(new GaugeRunner(menu.RankExpGauge, before, before, 0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (menu.info.campaigns != null && ((IEnumerable<Campaign>) menu.info.campaigns).FirstOrDefault<Campaign>((Func<Campaign, bool>) (x => x.campaign_type_id == 2)) != null)
      menu.Campaing[1].SetActive(true);
    int num = afterPoint - beforePoint;
    if (num >= 0)
      menu.TxtGetPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002346_GET_POINT_PLUS, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) num
        }
      }));
    else
      menu.TxtGetPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002346_GET_POINT_MINUS, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) Math.Abs(num)
        }
      }));
    menu.TxtRankPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002346_TOTAL_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) menu.result.colosseum_record.rank_pt
      }
    }));
    menu.TxtToNextRankPoint.SetTextLocalize(ColosseumUtility.GetNextRankPoint(menu.result.colosseum_record.rank_pt, menu.result.colosseum_record.enabled_max_rank_point).ToString() + Consts.GetInstance().COLOSSEUM_002351_PT);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1012");
    menu.PlayTween(menu.DirRankPoint);
    ((Component) menu.TxtGetPoint).gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(0.9f);
    menu.PlayTween(((Component) menu.TxtGetPoint).gameObject);
    bool isLow = false;
    if (beforeID > afterID)
      isLow = true;
    e = GaugeRunner.Run(new GaugeRunner(menu.RankExpGauge, before, after, loop, isLow: isLow));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (menu.nextBattleType != 0)
    {
      if (menu.nextBattleType == 1)
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1503");
      else if (menu.nextBattleType == 2)
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1504");
      menu.NextBattleEffect.SetActive(true);
      menu.NextBattleEffectPrefab.Clone(menu.DirRankPoint.transform).GetComponent<ColosseumNextBattleEffect>().Init(menu);
      menu.activeNextBattleEffect = true;
      while (menu.activeNextBattleEffect)
        yield return (object) null;
      menu.NextBattleEffect.SetActive(false);
      yield return (object) new WaitForSeconds(0.6f);
    }
    GameObject popup;
    if (menu.result.bonus_rewards.Length != 0)
    {
      popup = Singleton<PopupManager>.GetInstance().open(menu.TotalWinRewardPrefab);
      popup.SetActive(false);
      Popup023418Menu o = popup.GetComponent<Popup023418Menu>();
      e = o.Init(menu.result.bonus_rewards[0], menu.result.colosseum_record.attack_win);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bool isFinished = false;
      o.SetCallback((Action) (() => isFinished = true));
      popup.SetActive(true);
      while (!isFinished)
        yield return (object) null;
      yield return (object) new WaitForSeconds(0.6f);
      popup = (GameObject) null;
      o = (Popup023418Menu) null;
    }
    if (menu.result.campaign_rewards.Length != 0 || menu.result.campaign_next_rewards.Length != 0)
    {
      List<ResultMenuBase.CampaignReward> rewards = new List<ResultMenuBase.CampaignReward>();
      rewards.AddRange((IEnumerable<ResultMenuBase.CampaignReward>) menu.result.campaign_rewards);
      List<ResultMenuBase.CampaignNextReward> nexts = new List<ResultMenuBase.CampaignNextReward>();
      nexts.AddRange((IEnumerable<ResultMenuBase.CampaignNextReward>) menu.result.campaign_next_rewards);
      Popup0235MenuBase o;
      foreach (ResultMenuBase.CampaignReward campaignReward in rewards)
      {
        ResultMenuBase.CampaignReward reward = campaignReward;
        popup = (GameObject) null;
        ResultMenuBase.CampaignNextReward nextReward = nexts.Where<ResultMenuBase.CampaignNextReward>((Func<ResultMenuBase.CampaignNextReward, bool>) (x => x.campaign_id == reward.campaign_id)).FirstOrDefault<ResultMenuBase.CampaignNextReward>();
        if (nextReward != null)
        {
          popup = Singleton<PopupManager>.GetInstance().open(menu.CampaingPrefab2);
          nexts.Remove(nextReward);
        }
        else
          popup = Singleton<PopupManager>.GetInstance().open(menu.CampaingPrefab1);
        popup.SetActive(false);
        o = popup.GetComponent<Popup0235MenuBase>();
        e = o.Init(reward, nextReward, menu.GearPrefab, menu.UnitPrefab, menu.UniquePrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        popup.SetActive(true);
        while (!isFinished)
          yield return (object) null;
        popup = (GameObject) null;
        o = (Popup0235MenuBase) null;
      }
      foreach (ResultMenuBase.CampaignNextReward nextReward in nexts)
      {
        popup = Singleton<PopupManager>.GetInstance().open(menu.CampaingPrefab3);
        bool isFinished = false;
        popup.SetActive(false);
        o = popup.GetComponent<Popup0235MenuBase>();
        e = o.Init((ResultMenuBase.CampaignReward) null, nextReward, menu.GearPrefab, menu.UnitPrefab, menu.UniquePrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        o.SetCallback((Action) (() => isFinished = true));
        popup.SetActive(true);
        while (!isFinished)
          yield return (object) null;
        popup = (GameObject) null;
        o = (Popup0235MenuBase) null;
      }
      rewards.Clear();
      nexts.Clear();
      yield return (object) new WaitForSeconds(0.6f);
      rewards = (List<ResultMenuBase.CampaignReward>) null;
      nexts = (List<ResultMenuBase.CampaignNextReward>) null;
    }
    if (menu.result.colosseum_finish.new_emblems != null && menu.result.colosseum_finish.new_emblems.Length != 0)
    {
      PlayerEmblem[] playerEmblemArray = menu.result.colosseum_finish.new_emblems;
      for (int index = 0; index < playerEmblemArray.Length; ++index)
      {
        PlayerEmblem emblem = playerEmblemArray[index];
        popup = Singleton<PopupManager>.GetInstance().open(menu.NewEmblemRewardPrefab);
        popup.SetActive(false);
        Popup99914Menu o = popup.GetComponent<Popup99914Menu>();
        e = o.Init(emblem);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        popup.SetActive(true);
        while (!isFinished)
          yield return (object) null;
        popup = (GameObject) null;
        o = (Popup99914Menu) null;
      }
      playerEmblemArray = (PlayerEmblem[]) null;
      yield return (object) new WaitForSeconds(0.6f);
    }
    yield return (object) new WaitForSeconds(1f);
  }

  private IEnumerator ShowTitleEXP()
  {
    Colosseum02346Menu colosseum02346Menu = this;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1011", delay: 0.1f);
    if (colosseum02346Menu.info.campaigns != null && ((IEnumerable<Campaign>) colosseum02346Menu.info.campaigns).FirstOrDefault<Campaign>((Func<Campaign, bool>) (x => x.campaign_type_id == 1)) != null)
      colosseum02346Menu.Campaing[0].SetActive(true);
    colosseum02346Menu.PlayTween(colosseum02346Menu.DirTitleExp);
    yield return (object) new WaitForSeconds(0.25f);
  }

  private IEnumerator ShowBottomMessage()
  {
    Colosseum02346Menu colosseum02346Menu = this;
    if (colosseum02346Menu.info.is_tutorial && Persist.colosseumTutorial.Data.CurrentPage == 2)
    {
      Persist.colosseumTutorial.Data.CurrentPage = 3;
      Singleton<TutorialRoot>.GetInstance().ForceShowAdvice("colosseum3");
    }
    if (colosseum02346Menu.info.campaigns != null && ((IEnumerable<Campaign>) colosseum02346Menu.info.campaigns).FirstOrDefault<Campaign>((Func<Campaign, bool>) (x => x.campaign_type_id == 3)) != null)
      colosseum02346Menu.Campaing[2].SetActive(true);
    int num = colosseum02346Menu.result.colosseum_finish.remaining_times;
    if (num < 0)
      num = 0;
    colosseum02346Menu.TxtRemainNum.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002346_REMAIN_NUM, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) num
      }
    }));
    colosseum02346Menu.TxtAttentionNum.SetTextLocalize(Consts.Format(Consts.GetInstance().COLOSSEUM_002346_ATTENTION_NUM, (IDictionary) new Hashtable()
    {
      {
        (object) "cnt",
        (object) colosseum02346Menu.result.colosseum_finish.limit_times.ToString().ToConverter()
      }
    }));
    colosseum02346Menu.DirBottomMessage.SetActive(true);
    colosseum02346Menu.PlayTween(colosseum02346Menu.DirBottomMessage);
    yield return (object) new WaitForSeconds(1f);
  }

  private void Update()
  {
  }

  private delegate IEnumerator Runner();
}
