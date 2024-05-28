// Decompiled with JetBrains decompiler
// Type: BattleUI05ResultMenu
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
public class BattleUI05ResultMenu : ResultMenuBase
{
  [SerializeField]
  protected UILabel TxtREADME30;
  [SerializeField]
  protected UILabel TxtSubTitle24;
  [SerializeField]
  protected UILabel TxtTitle30;
  [SerializeField]
  protected UILabel TxtCharaEXP26;
  [SerializeField]
  protected UILabel TxtGetPlayerEXP24;
  [SerializeField]
  protected UILabel TxtGetZenie28;
  [SerializeField]
  protected UILabel TxtNextLevel18;
  [SerializeField]
  protected UILabel TxtNextLevelNB20;
  [SerializeField]
  private GameObject Title;
  [SerializeField]
  private GameObject Scene_Result;
  [SerializeField]
  private GameObject Block_Zeni;
  [SerializeField]
  private GameObject Block_Exp;
  [SerializeField]
  private GameObject PlayerEXPGauge;
  [SerializeField]
  private GameObject Top;
  [HideInInspector]
  public List<bool> BonusFlg;
  private int bonusCategory;
  private GameObject mPlayerLevelupPrefab;
  protected GameObject mStageMissionPrefab;
  protected GameObject mStageMissionItemPrefab;
  private GameObject mStageMissionItemCompPrefab;
  private GameObject mTrustSkillPrefab;
  private GameObject mDressSkillPrefab;
  private GameObject bonus;
  private BattleUI05Bonus bonusScript;
  protected BattleEnd result;
  private int nowLevelupCount;
  protected NGSoundManager soundMgr;
  private bool isQuestAutoLap;
  private const float LINK_WIDTH = 120f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 0.882352948f;
  private List<GameObject> mResultUnitPanels = new List<GameObject>();

  public int BonusCategory => this.bonusCategory;

  public override void OnDestroy()
  {
    this.mResultUnitPanels.Clear();
    base.OnDestroy();
  }

  private void Awake()
  {
    this.soundMgr = Singleton<NGSoundManager>.GetInstance();
    this.mResultUnitPanels.Clear();
  }

  private IEnumerator LoadResources(BattleInfo info)
  {
    Future<GameObject> h = Res.Prefabs.battle.PlayerLevelUpPrefab.Load<GameObject>();
    IEnumerator e = h.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mPlayerLevelupPrefab = h.Result;
    h = (Future<GameObject>) null;
    h = !Singleton<NGGameDataManager>.GetInstance().IsSea || info.seaQuest == null ? new ResourceObject("Prefabs/quest002_2/dir_Mission_List_Result").Load<GameObject>() : new ResourceObject("Prefabs/quest002_2_sea/dir_Mission_List_Result_sea").Load<GameObject>();
    e = h.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mStageMissionPrefab = h.Result;
    h = Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/quest002_2_sea/dir_Misson_List_Item_sea").Load<GameObject>() : new ResourceObject("Prefabs/quest002_2/dir_Misson_List_Item").Load<GameObject>();
    e = h.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mStageMissionItemPrefab = h.Result;
    h = Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/quest002_2_sea/dir_Misson_List_Item_Comp_sea").Load<GameObject>() : new ResourceObject("Prefabs/quest002_2/dir_Misson_List_Item_Comp").Load<GameObject>();
    e = h.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mStageMissionItemCompPrefab = h.Result;
    h = (Future<GameObject>) null;
    h = new ResourceObject("Animations/extraskill/FavorabilityRatingEffect").Load<GameObject>();
    e = h.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mTrustSkillPrefab = h.Result;
    h = (Future<GameObject>) null;
    h = new ResourceObject("Animations/common_gear_skill/CommonGearSkillEffect").Load<GameObject>();
    e = h.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mDressSkillPrefab = h.Result;
    h = (Future<GameObject>) null;
  }

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    battleUi05ResultMenu.info = info;
    battleUi05ResultMenu.result = result;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = battleUi05ResultMenu.\u003C\u003En__0(info, result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = battleUi05ResultMenu.LoadResources(info);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (info != null)
    {
      switch (info.quest_type)
      {
        case CommonQuestType.Story:
          battleUi05ResultMenu.bonusCategory = info.storyQuest.bonus_category;
          battleUi05ResultMenu.SetQuestName(MasterData.QuestStoryS[info.quest_s_id].name);
          break;
        case CommonQuestType.Character:
          battleUi05ResultMenu.SetQuestName(MasterData.QuestCharacterS[info.quest_s_id].name);
          break;
        case CommonQuestType.Extra:
          battleUi05ResultMenu.SetQuestName(MasterData.QuestExtraS[info.quest_s_id].name);
          break;
        case CommonQuestType.Harmony:
          battleUi05ResultMenu.SetQuestName(MasterData.QuestHarmonyS[info.quest_s_id].name);
          break;
        case CommonQuestType.Sea:
          battleUi05ResultMenu.bonusCategory = info.seaQuest.bonus_category;
          battleUi05ResultMenu.SetQuestName(MasterData.QuestSeaS[info.quest_s_id].name);
          break;
        case CommonQuestType.GuildRaid:
          battleUi05ResultMenu.SetQuestName(MasterData.GuildRaid[info.quest_s_id].stage_name);
          break;
      }
    }
    battleUi05ResultMenu.SetZenie(result.player_incr_money);
    if (Object.op_Equality((Object) battleUi05ResultMenu.bonus, (Object) null))
    {
      Future<GameObject> bonusPrefab = Res.Prefabs.battle.dir_Bonus.Load<GameObject>();
      e = bonusPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleUi05ResultMenu.bonus = bonusPrefab.Result.Clone(battleUi05ResultMenu.Top.transform);
      battleUi05ResultMenu.bonusScript = battleUi05ResultMenu.bonus.GetComponent<BattleUI05Bonus>();
      battleUi05ResultMenu.bonus.SetActive(true);
      bonusPrefab = (Future<GameObject>) null;
    }
    battleUi05ResultMenu.Scene_Result.SetActive(false);
  }

  public void SetZenie(long zenie) => this.TxtGetZenie28.SetTextLocalize(zenie);

  public void SetQuestName(string txt) => this.TxtSubTitle24.SetTextLocalize(txt);

  public bool IsNotLastPlayerLevelUp()
  {
    return this.result.before_player.level + this.nowLevelupCount >= SMManager.Get<Player>().level - 1;
  }

  public IEnumerator OnLastPlayerLevelup(GameObject obj, int count)
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    yield return (object) new WaitForSeconds(0.1f);
    GaugeRunner.PauseSE();
    Player player = SMManager.Get<Player>();
    Battle020171Menu o = battleUi05ResultMenu.OpenPopup(battleUi05ResultMenu.mPlayerLevelupPrefab).GetComponent<Battle020171Menu>();
    o.SetLv(player.level - 1, player.level);
    o.SetName(player.name);
    List<string> self = new List<string>();
    self.Add(Consts.GetInstance().BATTLE_RESULT_RECOVERY_AP);
    int num1;
    if ((num1 = player.ap_max - battleUi05ResultMenu.result.before_player.ap_max) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_AP, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num1
        }
      }));
    int num2;
    if ((num2 = player.max_cost - battleUi05ResultMenu.result.before_player.max_cost) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_DECK_COST, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num2
        }
      }));
    int num3;
    if ((num3 = player.max_friends - battleUi05ResultMenu.result.before_player.max_friends) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_FRIEND_COUNT, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num3
        }
      }));
    o.SetExplanetion(self.Join("\n"));
    bool onFinished = false;
    o.SetCallback((Action) (() => onFinished = true));
    while (!onFinished)
    {
      if (battleUi05ResultMenu.isQuestAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        o.IbtnScreen();
      }
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.1f);
    GaugeRunner.ResumeSE();
  }

  public IEnumerator OnPlayerLevelup(GameObject obj, int count)
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    battleUi05ResultMenu.nowLevelupCount = count;
    yield return (object) new WaitForSeconds(0.1f);
    GaugeRunner.PauseSE();
    Battle020171Menu o = battleUi05ResultMenu.OpenPopup(battleUi05ResultMenu.mPlayerLevelupPrefab).GetComponent<Battle020171Menu>();
    int before = battleUi05ResultMenu.result.before_player.level + count;
    int after = battleUi05ResultMenu.result.before_player.level + count + 1;
    Player player = SMManager.Get<Player>();
    o.SetLv(before, after);
    o.SetName(player.name);
    List<string> self = new List<string>();
    self.Add(Consts.GetInstance().BATTLE_RESULT_RECOVERY_AP);
    int num1;
    if ((num1 = player.ap_max - battleUi05ResultMenu.result.before_player.ap_max) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_AP, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num1
        }
      }));
    int num2;
    if ((num2 = player.max_cost - battleUi05ResultMenu.result.before_player.max_cost) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_DECK_COST, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num2
        }
      }));
    int num3;
    if ((num3 = player.max_friends - battleUi05ResultMenu.result.before_player.max_friends) > 0)
      self.Add(Consts.Format(Consts.GetInstance().BATTLE_RESULT_INCR_FRIEND_COUNT, (IDictionary) new Hashtable()
      {
        {
          (object) "value",
          (object) num3
        }
      }));
    o.SetExplanetion(self.Join("\n"));
    bool onFinished = false;
    o.SetCallback((Action) (() => onFinished = true));
    while (!onFinished)
    {
      if (battleUi05ResultMenu.isQuestAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        o.IbtnScreen();
      }
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.1f);
    GaugeRunner.ResumeSE();
  }

  public override IEnumerator Run()
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    battleUi05ResultMenu.Scene_Result.SetActive(true);
    battleUi05ResultMenu.Title.SetActive(true);
    BattleUI05ResultMenu.Runner[] runnerArray1 = new BattleUI05ResultMenu.Runner[14]
    {
      new BattleUI05ResultMenu.Runner(battleUi05ResultMenu.InitObjects),
      new BattleUI05ResultMenu.Runner(battleUi05ResultMenu.ShowMoney),
      new BattleUI05ResultMenu.Runner(battleUi05ResultMenu.ShowPlayerEXP),
      new BattleUI05ResultMenu.Runner(((ResultMenuBase) battleUi05ResultMenu).SkipPopupPlay),
      new BattleUI05ResultMenu.Runner(battleUi05ResultMenu.ShowTitleEXP),
      new BattleUI05ResultMenu.Runner(((ResultMenuBase) battleUi05ResultMenu).ShowUnitEXP),
      new BattleUI05ResultMenu.Runner(((ResultMenuBase) battleUi05ResultMenu).CharacterIntimatesPopup),
      new BattleUI05ResultMenu.Runner(((ResultMenuBase) battleUi05ResultMenu).CharacterLoveLimitPopup),
      new BattleUI05ResultMenu.Runner(((ResultMenuBase) battleUi05ResultMenu).CharacterStoryPopup),
      new BattleUI05ResultMenu.Runner(((ResultMenuBase) battleUi05ResultMenu).HarmonyStoryPopup),
      new BattleUI05ResultMenu.Runner(battleUi05ResultMenu.TrustSkillPopup),
      new BattleUI05ResultMenu.Runner(battleUi05ResultMenu.StageMissionPopup),
      new BattleUI05ResultMenu.Runner(battleUi05ResultMenu.StageMissionCompletePopup),
      new BattleUI05ResultMenu.Runner(((ResultMenuBase) battleUi05ResultMenu).SkipPopupPlay)
    };
    battleUi05ResultMenu.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
    BattleUI05ResultMenu.Runner[] runnerArray = runnerArray1;
    for (int index = 0; index < runnerArray.Length; ++index)
    {
      IEnumerator e = runnerArray[index]();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    runnerArray = (BattleUI05ResultMenu.Runner[]) null;
  }

  public override IEnumerator OnFinish()
  {
    this.Scene_Result.SetActive(false);
    this.Title.SetActive(false);
    yield break;
  }

  private IEnumerator InitObjects()
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    foreach (var data in battleUi05ResultMenu.bonusScript.Ids.Select((s, i) => new
    {
      s = s,
      i = i
    }))
    {
      // ISSUE: reference to a compiler-generated method
      battleUi05ResultMenu.BonusFlg[data.i] = battleUi05ResultMenu.bonusCategory != 0 && ((IEnumerable<int>) data.s).Any<int>(new Func<int, bool>(battleUi05ResultMenu.\u003CInitObjects\u003Eb__46_10));
    }
    // ISSUE: reference to a compiler-generated method
    int? nullable = ((IEnumerable<PlayerUnit>) battleUi05ResultMenu.result.before_player_units).FirstIndexOrNull<PlayerUnit>(new Func<PlayerUnit, bool>(battleUi05ResultMenu.\u003CInitObjects\u003Eb__46_0));
    if (nullable.HasValue)
      battleUi05ResultMenu.mvp_index = nullable.Value;
    battleUi05ResultMenu.questType = battleUi05ResultMenu.info.quest_type;
    battleUi05ResultMenu.questSID = battleUi05ResultMenu.info.quest_s_id;
    battleUi05ResultMenu.deck_type_id = battleUi05ResultMenu.result.deck_type_id;
    battleUi05ResultMenu.deck_number = battleUi05ResultMenu.result.deck_number;
    battleUi05ResultMenu.makeDiffGearAccessoryRemainingAmounts(battleUi05ResultMenu.result.before_player_gears);
    if (BattleInfo.checkCustomDeck(battleUi05ResultMenu.deck_type_id))
    {
      battleUi05ResultMenu.patchPUNK_DEBUG_21142(battleUi05ResultMenu.result.before_player_gears, battleUi05ResultMenu.result.after_player_gears);
      int[] disappearedGears = battleUi05ResultMenu.result.disappeared_player_gears ?? new int[0];
      battleUi05ResultMenu.beforeUnits = ((IEnumerable<PlayerUnit>) battleUi05ResultMenu.createCustomDeckUnits(battleUi05ResultMenu.deck_number, battleUi05ResultMenu.result.before_player_units, ((IEnumerable<PlayerItem>) battleUi05ResultMenu.result.before_player_gears).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !((IEnumerable<int>) disappearedGears).Contains<int>(x.id))).ToArray<PlayerItem>())).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
      battleUi05ResultMenu.afterUnits = ((IEnumerable<PlayerUnit>) battleUi05ResultMenu.createCustomDeckUnits(battleUi05ResultMenu.deck_number, battleUi05ResultMenu.result.after_player_units, ((IEnumerable<PlayerItem>) battleUi05ResultMenu.result.after_player_gears).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !((IEnumerable<int>) disappearedGears).Contains<int>(x.id))).ToArray<PlayerItem>())).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    }
    else
    {
      battleUi05ResultMenu.beforeUnits = ((IEnumerable<PlayerUnit>) battleUi05ResultMenu.result.before_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
      battleUi05ResultMenu.afterUnits = ((IEnumerable<PlayerUnit>) battleUi05ResultMenu.result.after_player_units).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.id));
    }
    battleUi05ResultMenu.beforeGears = ((IEnumerable<PlayerItem>) battleUi05ResultMenu.result.before_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    battleUi05ResultMenu.afterGears = ((IEnumerable<PlayerItem>) battleUi05ResultMenu.result.after_player_gears).ToDictionary<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id));
    battleUi05ResultMenu.updateLocalCustomDecks(battleUi05ResultMenu.result.disappeared_player_gears);
    battleUi05ResultMenu.characterIntimates.AddRange((IEnumerable<BattleEndPlayer_character_intimates_in_battle>) battleUi05ResultMenu.result.player_character_intimates_in_battle);
    battleUi05ResultMenu.unlockIntimateSkills = ((IEnumerable<UnlockIntimateSkill>) battleUi05ResultMenu.result.unlock_intimate_skills).ToList<UnlockIntimateSkill>();
    battleUi05ResultMenu.trusutUpperLimits.AddRange((IEnumerable<BattleEndTrust_upper_limit>) battleUi05ResultMenu.result.trust_upper_limit);
    battleUi05ResultMenu.unlockCharacterQuestIDS.Clear();
    if (battleUi05ResultMenu.result.unlock_quests != null && battleUi05ResultMenu.result.unlock_quests.Length != 0)
      battleUi05ResultMenu.unlockCharacterQuestIDS = ((IEnumerable<UnlockQuest>) battleUi05ResultMenu.result.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 2)).Select<UnlockQuest, int>((Func<UnlockQuest, int>) (x => x.quest_s_id)).ToList<int>();
    battleUi05ResultMenu.unlockHarmonyQuestIDS.Clear();
    if (battleUi05ResultMenu.result.unlock_quests != null && battleUi05ResultMenu.result.unlock_quests.Length != 0)
      battleUi05ResultMenu.unlockHarmonyQuestIDS = ((IEnumerable<UnlockQuest>) battleUi05ResultMenu.result.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 4)).Select<UnlockQuest, int>((Func<UnlockQuest, int>) (x => x.quest_s_id)).ToList<int>();
    battleUi05ResultMenu.disappearedPlayerGears.Clear();
    if (battleUi05ResultMenu.result.disappeared_player_gears != null && battleUi05ResultMenu.result.disappeared_player_gears.Length != 0)
    {
      foreach (int disappearedPlayerGear in battleUi05ResultMenu.result.disappeared_player_gears)
      {
        if (battleUi05ResultMenu.beforeGears.ContainsKey(disappearedPlayerGear))
          battleUi05ResultMenu.disappearedPlayerGears.Add(battleUi05ResultMenu.beforeGears[disappearedPlayerGear]);
      }
    }
    battleUi05ResultMenu.Block_Zeni.SetActive(false);
    battleUi05ResultMenu.Block_Exp.SetActive(false);
    yield return (object) new WaitForSeconds(0.1f);
  }

  private IEnumerator ShowMoney()
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    battleUi05ResultMenu.soundMgr.playSE("SE_1011");
    battleUi05ResultMenu.soundMgr.playSE("SE_1012");
    battleUi05ResultMenu.soundMgr.playSE("SE_1013", delay: 1.28f);
    battleUi05ResultMenu.Block_Zeni.SetActive(true);
    if (battleUi05ResultMenu.BonusFlg[2])
      battleUi05ResultMenu.SetBonus(battleUi05ResultMenu.bonusCategory);
    yield return (object) battleUi05ResultMenu.SkipWaitForSecond(1.5f);
  }

  private IEnumerator ShowPlayerEXP()
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    Player player = SMManager.Get<Player>();
    float after = (float) player.exp / (float) (player.exp + player.exp_next);
    int loopNum = player.level - battleUi05ResultMenu.result.before_player.level;
    GaugeRunner r = new GaugeRunner(battleUi05ResultMenu.PlayerEXPGauge, (float) battleUi05ResultMenu.result.before_player.exp / (float) (battleUi05ResultMenu.result.before_player.exp + battleUi05ResultMenu.result.before_player.exp_next), after, loopNum, new Func<GameObject, int, IEnumerator>(battleUi05ResultMenu.OnPlayerLevelup));
    battleUi05ResultMenu.TxtGetPlayerEXP24.SetTextLocalize("+" + (object) battleUi05ResultMenu.result.player_incr_exp);
    battleUi05ResultMenu.TxtNextLevelNB20.SetTextLocalize(player.exp_next <= 0 ? "MAX" : player.exp_next.ToString());
    battleUi05ResultMenu.soundMgr.playSE("SE_1012");
    battleUi05ResultMenu.Block_Exp.SetActive(true);
    if (battleUi05ResultMenu.BonusFlg[1])
      battleUi05ResultMenu.SetBonus(battleUi05ResultMenu.bonusCategory);
    yield return (object) battleUi05ResultMenu.SkipWaitForSecond(0.9f);
    IEnumerator e = GaugeRunner.Run(r);
    while (e.MoveNext() && !battleUi05ResultMenu.isSkip)
      yield return e.Current;
    e = (IEnumerator) null;
    Object.Destroy((Object) battleUi05ResultMenu.PlayerEXPGauge.GetComponent<TweenScale>());
    if (battleUi05ResultMenu.isSkip)
    {
      bool play = loopNum > 0 && r.count < loopNum;
      r = new GaugeRunner(battleUi05ResultMenu.PlayerEXPGauge, after - 0.01f, after, 0);
      e = GaugeRunner.Run(r);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (play)
      {
        e = battleUi05ResultMenu.OnPlayerLevelup((GameObject) null, loopNum - 1);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator ShowTitleEXP()
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    battleUi05ResultMenu.soundMgr.playSE("SE_1011", delay: 0.1f);
    if (battleUi05ResultMenu.BonusFlg[0])
      battleUi05ResultMenu.SetBonus(battleUi05ResultMenu.bonusCategory);
    yield return (object) battleUi05ResultMenu.SkipWaitForSecond(0.25f);
  }

  private IEnumerator TrustSkillPopup()
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    if (battleUi05ResultMenu.result.gain_trust_info != null && battleUi05ResultMenu.result.gain_trust_info.Length != 0)
    {
      List<Tuple<PlayerUnit, PlayerUnit, bool, bool>> tupleList = new List<Tuple<PlayerUnit, PlayerUnit, bool, bool>>();
      List<int> intList = new List<int>();
      foreach (PlayerUnit playerUnit1 in battleUi05ResultMenu.info.deck.player_units)
      {
        PlayerUnit unit = playerUnit1;
        if (!(unit == (PlayerUnit) null))
        {
          int? nullable = ((IEnumerable<BattleEndGain_trust_info>) battleUi05ResultMenu.result.gain_trust_info).FirstIndexOrNull<BattleEndGain_trust_info>((Func<BattleEndGain_trust_info, bool>) (x => x.player_unit_id == unit.id));
          if (nullable.HasValue)
          {
            intList.Add(nullable.Value);
            BattleEndGain_trust_info endGainTrustInfo = battleUi05ResultMenu.result.gain_trust_info[nullable.Value];
            PlayerUnit playerUnit2 = ((IEnumerable<PlayerUnit>) battleUi05ResultMenu.result.after_player_units).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == unit.id));
            PlayerUnit playerUnit3 = ((IEnumerable<PlayerUnit>) battleUi05ResultMenu.result.before_player_units).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == unit.id));
            tupleList.Add(Tuple.Create<PlayerUnit, PlayerUnit, bool, bool>(playerUnit3, playerUnit2, endGainTrustInfo.gain_trust_result.is_equip_awake_skill_release, endGainTrustInfo.gain_trust_result.has_new_player_awake_skill));
          }
        }
      }
      foreach (Tuple<PlayerUnit, PlayerUnit, bool, bool> tuple in tupleList)
      {
        if (battleUi05ResultMenu.isSkip)
        {
          battleUi05ResultMenu.skipPopupList.Add(battleUi05ResultMenu.TrustSkillUnitPopup(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
        }
        else
        {
          IEnumerator e = battleUi05ResultMenu.TrustSkillUnitPopup(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
  }

  private IEnumerator TrustSkillUnitPopup(
    PlayerUnit beforeUnit,
    PlayerUnit afterUnit,
    bool isRelease,
    bool isAcquire)
  {
    List<FavorabilityRatingEffect.AnimationType> animationTypeList = new List<FavorabilityRatingEffect.AnimationType>();
    if (isRelease)
      animationTypeList.Add(FavorabilityRatingEffect.AnimationType.SkillFrameRelease);
    if (isAcquire)
      animationTypeList.Add(FavorabilityRatingEffect.AnimationType.SkillRelease);
    foreach (FavorabilityRatingEffect.AnimationType anmType in animationTypeList)
    {
      bool isFinished = false;
      GameObject go = (GameObject) null;
      if (afterUnit.unit.IsSea)
        go = Singleton<PopupManager>.GetInstance().open(this.mTrustSkillPrefab, isNonSe: true, isNonOpenAnime: true);
      else if (afterUnit.unit.IsResonanceUnit)
        go = Singleton<PopupManager>.GetInstance().open(this.mDressSkillPrefab, isNonSe: true, isNonOpenAnime: true);
      if (!Object.op_Equality((Object) go, (Object) null))
      {
        Consts instance = Consts.GetInstance();
        bool flag = (double) beforeUnit.trust_rate < (double) instance.TRUST_RATE_LEVEL_SIZE && (double) afterUnit.trust_rate >= (double) instance.TRUST_RATE_LEVEL_SIZE;
        FavorabilityRatingEffect favorabilityRatingEffect = go.GetComponent<FavorabilityRatingEffect>();
        IEnumerator e = favorabilityRatingEffect.Init(anmType, afterUnit, (Action) (() =>
        {
          isFinished = true;
          Singleton<PopupManager>.GetInstance().dismiss();
        }), !flag);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        favorabilityRatingEffect.StartEffect();
        Singleton<PopupManager>.GetInstance().startOpenAnime(go, true);
        while (!isFinished || Singleton<PopupManager>.GetInstance().isOpen)
          yield return (object) null;
        go = (GameObject) null;
        favorabilityRatingEffect = (FavorabilityRatingEffect) null;
      }
    }
  }

  protected virtual IEnumerator StageMissionPopup()
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    if (battleUi05ResultMenu.isSkip)
    {
      battleUi05ResultMenu.skipPopupList.Add(battleUi05ResultMenu.StageMissionPopup());
    }
    else
    {
      object[] missions = (object[]) null;
      switch (battleUi05ResultMenu.info.quest_type)
      {
        case CommonQuestType.Story:
          missions = ((IEnumerable<QuestStoryMission>) MasterData.QuestStoryMissionList).Where<QuestStoryMission>((Func<QuestStoryMission, bool>) (x => x.quest_s.ID == this.info.quest_s_id)).Select<QuestStoryMission, object>((Func<QuestStoryMission, object>) (x => (object) x)).ToArray<object>();
          break;
        case CommonQuestType.Extra:
          missions = ((IEnumerable<QuestExtraMission>) MasterData.QuestExtraMissionList).Where<QuestExtraMission>((Func<QuestExtraMission, bool>) (x => x.quest_s.ID == this.info.quest_s_id)).Select<QuestExtraMission, object>((Func<QuestExtraMission, object>) (x => (object) x)).ToArray<object>();
          break;
        case CommonQuestType.Sea:
          missions = ((IEnumerable<QuestSeaMission>) MasterData.QuestSeaMissionList).Where<QuestSeaMission>((Func<QuestSeaMission, bool>) (x => x.quest_s.ID == this.info.quest_s_id)).Select<QuestSeaMission, object>((Func<QuestSeaMission, object>) (x => (object) x)).ToArray<object>();
          break;
      }
      if (missions != null && missions.Length != 0)
      {
        PlayerMissionHistory[] historyArray = (PlayerMissionHistory[]) null;
        PlayerMissionHistory[] histextra = (PlayerMissionHistory[]) null;
        PlayerMissionHistory[] histstory = (PlayerMissionHistory[]) null;
        bool flag = true;
        List<int> clearedMissionIds = ((IEnumerable<PlayerMissionHistory>) battleUi05ResultMenu.result.player_mission_results).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).ToList<int>();
        switch (battleUi05ResultMenu.info.quest_type)
        {
          case CommonQuestType.Story:
            historyArray = SMManager.Get<PlayerMissionHistory[]>();
            histstory = ((IEnumerable<PlayerMissionHistory>) historyArray).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 1)).ToArray<PlayerMissionHistory>();
            for (int index = 0; index < missions.Length; ++index)
            {
              QuestStoryMission mission = (QuestStoryMission) missions[index];
              if (!((IEnumerable<PlayerMissionHistory>) histstory).Any<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.mission_id == mission.ID)) || clearedMissionIds.Contains(mission.ID))
              {
                flag = false;
                break;
              }
            }
            break;
          case CommonQuestType.Extra:
            histextra = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 3)).ToArray<PlayerMissionHistory>();
            for (int index = 0; index < missions.Length; ++index)
            {
              QuestExtraMission mission = (QuestExtraMission) missions[index];
              if (!((IEnumerable<PlayerMissionHistory>) histextra).Any<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.mission_id == mission.ID)) || clearedMissionIds.Contains(mission.ID))
              {
                flag = false;
                break;
              }
            }
            break;
          case CommonQuestType.Sea:
            histstory = ((IEnumerable<PlayerMissionHistory>) SMManager.Get<PlayerMissionHistory[]>()).Where<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.story_category == 9)).ToArray<PlayerMissionHistory>();
            for (int index = 0; index < missions.Length; ++index)
            {
              QuestSeaMission mission = (QuestSeaMission) missions[index];
              if (!((IEnumerable<PlayerMissionHistory>) histstory).Any<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.mission_id == mission.ID)) || clearedMissionIds.Contains(mission.ID))
              {
                flag = false;
                break;
              }
            }
            break;
        }
        if (!flag)
        {
          GameObject popup = battleUi05ResultMenu.OpenPopup(battleUi05ResultMenu.mStageMissionPrefab);
          ((IEnumerable<UITweener>) popup.GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
          {
            ((Behaviour) x).enabled = false;
            x.onFinished.Clear();
          }));
          Quest0022MissionDescriptions o = popup.GetComponent<Quest0022MissionDescriptions>();
          int length = missions.Length;
          Quest0022MissionDescription oo = o.description;
          int num = oo.UpdateScrollViewHeight(length);
          for (int index = 0; index < length; ++index)
          {
            Quest0022MissionList component = battleUi05ResultMenu.mStageMissionItemPrefab.Clone(((Component) oo.grid).transform).GetComponent<Quest0022MissionList>();
            ((Component) component).transform.localPosition = new Vector3(0.0f, (float) -index * oo.grid.cellHeight, 0.0f);
            oo.MissionList.Add(component);
            oo.ThumbnailList.Add(((Component) component.LinkParent).gameObject);
          }
          oo.grid.Reposition();
          oo.scrollView.ResetPosition();
          IEnumerator e = oo.LoadAnimation(Mathf.FloorToInt((float) num / oo.grid.cellHeight));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          int i;
          switch (battleUi05ResultMenu.info.quest_type)
          {
            case CommonQuestType.Story:
              if (historyArray == null)
              {
                o = (Quest0022MissionDescriptions) null;
                Singleton<PopupManager>.GetInstance().dismiss();
                break;
              }
              for (i = 0; i < missions.Length; ++i)
              {
                QuestStoryMission mission = (QuestStoryMission) missions[i];
                bool clearFlag = true;
                PlayerMissionHistory playerMissionHistory = ((IEnumerable<PlayerMissionHistory>) histstory).FirstOrDefault<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.mission_id == mission.ID));
                if (!clearedMissionIds.Contains(mission.ID))
                  clearFlag = playerMissionHistory != null;
                if (oo.MissionList.Count > i)
                {
                  e = oo.MissionList[i].SetValue(mission.name, clearFlag, mission.entity_type, mission.entity_id, mission.quantity, clearedMissionIds.Contains(mission.ID), new CommonQuestType?(CommonQuestType.Story));
                  while (e.MoveNext())
                    yield return e.Current;
                  e = (IEnumerator) null;
                }
              }
              break;
            case CommonQuestType.Extra:
              for (i = 0; i < missions.Length; ++i)
              {
                QuestExtraMission mission = (QuestExtraMission) missions[i];
                bool clearFlag = true;
                PlayerMissionHistory playerMissionHistory = ((IEnumerable<PlayerMissionHistory>) histextra).FirstOrDefault<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.mission_id == mission.ID));
                if (!clearedMissionIds.Contains(mission.ID))
                  clearFlag = playerMissionHistory != null;
                if (oo.MissionList.Count > i)
                {
                  e = oo.MissionList[i].SetValue(mission.name, clearFlag, mission.entity_type, mission.entity_id, mission.quantity, clearedMissionIds.Contains(mission.ID), new CommonQuestType?(CommonQuestType.Extra));
                  while (e.MoveNext())
                    yield return e.Current;
                  e = (IEnumerator) null;
                }
              }
              break;
            case CommonQuestType.Sea:
              for (i = 0; i < missions.Length; ++i)
              {
                QuestSeaMission mission = (QuestSeaMission) missions[i];
                bool clearFlag = true;
                PlayerMissionHistory playerMissionHistory = ((IEnumerable<PlayerMissionHistory>) histstory).FirstOrDefault<PlayerMissionHistory>((Func<PlayerMissionHistory, bool>) (x => x.mission_id == mission.ID));
                if (!clearedMissionIds.Contains(mission.ID))
                  clearFlag = playerMissionHistory != null;
                if (oo.MissionList.Count > i)
                {
                  e = oo.MissionList[i].SetValue(mission.name, clearFlag, mission.entity_type, mission.entity_id, mission.quantity, clearedMissionIds.Contains(mission.ID), new CommonQuestType?(CommonQuestType.Sea));
                  while (e.MoveNext())
                    yield return e.Current;
                  e = (IEnumerator) null;
                }
              }
              break;
          }
          if (Object.op_Inequality((Object) o, (Object) null))
          {
            oo.MissionList.ForEach((Action<Quest0022MissionList>) (x => x.InitEffects()));
            battleUi05ResultMenu.soundMgr.playSE("SE_1035");
            o.StartTweenClick(true);
            yield return (object) new WaitForSeconds(0.5f);
            bool completedMissionExists = false;
            if (oo.MissionList.Exists((Predicate<Quest0022MissionList>) (x => x.IsClear)))
            {
              ((Behaviour) oo.scrollView).enabled = false;
              completedMissionExists = true;
              oo.PlayMissionCompleteTitleAnimation();
            }
            yield return (object) new WaitForSeconds(1f);
            for (int index = 0; index < oo.MissionList.Count; ++index)
            {
              Quest0022MissionList mission = oo.MissionList[index];
              if (mission.IsClear)
                mission.ResultNowGet();
            }
            if (completedMissionExists)
              battleUi05ResultMenu.soundMgr.playSE("SE_1036");
            yield return (object) new WaitForSeconds(0.5f);
            bool isFinished = false;
            bool closeMissionPopupAnim = false;
            Action closeStageMissionPopupAction = (Action) (() =>
            {
              oo.PlayMissionCompleteTitleAnimation("Anim_Out");
              closeMissionPopupAnim = true;
              if (this.isQuestAutoLap)
              {
                for (int index = 0; index < oo.MissionList.Count; ++index)
                  oo.MissionList[index].ClearEffectDisable();
              }
              o.StartTweenClick(false, (EventDelegate.Callback) (() => isFinished = true));
            });
            battleUi05ResultMenu.CreateTouchObject(new EventDelegate.Callback(closeStageMissionPopupAction.Invoke), popup.transform.parent);
            EventDelegate.Add(oo.dragScrollViewButton.onClick, new EventDelegate.Callback(closeStageMissionPopupAction.Invoke));
            yield return (object) new WaitForSeconds(0.5f);
            ((Behaviour) oo.scrollView).enabled = true;
            while (!isFinished)
            {
              if (battleUi05ResultMenu.isQuestAutoLap && !closeMissionPopupAnim)
              {
                yield return (object) new WaitForSeconds(0.5f);
                closeStageMissionPopupAction();
              }
              yield return (object) null;
            }
            Singleton<PopupManager>.GetInstance().dismiss();
            closeStageMissionPopupAction = (Action) null;
          }
        }
      }
    }
  }

  private IEnumerator StageMissionCompletePopup()
  {
    BattleUI05ResultMenu battleUi05ResultMenu = this;
    if (battleUi05ResultMenu.isSkip)
    {
      battleUi05ResultMenu.skipPopupList.Add(battleUi05ResultMenu.StageMissionCompletePopup());
    }
    else
    {
      BattleEndMission_complete_rewards[] missionCompleteRewards = battleUi05ResultMenu.result.mission_complete_rewards;
      if (missionCompleteRewards != null && missionCompleteRewards.Length != 0)
      {
        Resolution windowSize = Screen.currentResolution;
        Future<Sprite> textureLoader = Res.Prefabs.BackGround.black.Load<Sprite>();
        IEnumerator e = textureLoader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Future<GameObject> loader = Res.Animations.clearBonus.missionClear.Load<GameObject>();
        e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject colorLayer = new GameObject("Color Layer")
        {
          transform = {
            parent = ((Component) battleUi05ResultMenu).gameObject.transform
          },
          layer = ((Component) battleUi05ResultMenu).gameObject.layer
        };
        colorLayer.transform.localScale = new Vector3(1f, 1f, 1f);
        UIPanel uiPanel = colorLayer.AddComponent<UIPanel>();
        UI2DSprite ui2Dsprite = colorLayer.AddComponent<UI2DSprite>();
        uiPanel.depth = 20;
        ui2Dsprite.sprite2D = textureLoader.Result;
        ((UIRect) ui2Dsprite).alpha = 0.75f;
        ((UIWidget) ui2Dsprite).height = ((Resolution) ref windowSize).height;
        ((UIWidget) ui2Dsprite).width = ((Resolution) ref windowSize).width;
        GameObject bonus = loader.Result.Clone(colorLayer.transform);
        e = bonus.GetComponent<BattleUI05MissionCompleteBonusMenu>().Init(battleUi05ResultMenu.info, battleUi05ResultMenu.result);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bool isFinished = false;
        battleUi05ResultMenu.CreateTouchObject((EventDelegate.Callback) (() => isFinished = true), bonus.transform.parent);
        while (!isFinished)
        {
          if (battleUi05ResultMenu.isQuestAutoLap)
          {
            yield return (object) new WaitForSeconds(3f);
            isFinished = true;
          }
          yield return (object) null;
        }
        Object.DestroyObject((Object) colorLayer);
      }
    }
  }

  public void SetBonus(int category, bool isClear = false)
  {
    this.bonusScript.SetBonusTitle(category, isClear);
  }

  public void DisableBonusTitle() => this.SetBonus(0, true);

  private delegate IEnumerator Runner();
}
