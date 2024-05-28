// Decompiled with JetBrains decompiler
// Type: GameCore.BattleInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnitRegulation;

#nullable disable
namespace GameCore
{
  [Serializable]
  public class BattleInfo
  {
    public bool pvp;
    public bool pvp_restart;
    public bool pvp_vs_npc;
    public string host;
    public int port;
    public string battleToken;
    public bool gvg;
    public GVGSetting gvgSetting;
    public KeyValuePair<int, int> raidBossDamage;
    public bool isSimulation;
    public Dictionary<int, float> enemyHpRate;
    public PlayerDeck mTower_deck;
    public Dictionary<int, int> enemyRestHp;
    public bool isResume;
    public bool isEarthMode;
    public bool isExtraEarthQuest;
    public bool isLoadData;
    public string battleId = "";
    public int quest_s_id;
    public int quest_loop_count;
    public int raidEndlessLoopCount;
    public CommonQuestType quest_type = CommonQuestType.Story;
    private int mStageId = 1;
    public int period_id = -1;
    public bool isAutoBattleEnable = true;
    public bool isContinueEnable = true;
    public bool isRetreatEnable = true;
    public bool isStoryEnable = true;
    public bool isTempDeck;
    public bool isCustomDeck;
    public PlayerHelper helper;
    public PlayerUnit[] helper_overkillers;
    [NonSerialized]
    private DeckInfo _deck;
    public int deckIndex;
    public int[] guest_ids = new int[0];
    private bool isSplitedFacilityFromEnemy;
    private int[] mEnemy_ids;
    private Tuple<int, Reward>[] mEnemy_items;
    private PlayerUnit[] mUser_units;
    private PlayerItem[] mUser_items;
    private int[] mUser_enemy_ids;
    private Tuple<int, Reward>[] mUser_enemy_items;
    private int[] mPanel_ids;
    private Tuple<int, Reward>[] mPanel_items;
    public PlayerUnit[] facility_units;
    public Tuple<int, int>[] facility_coordinates;
    public PlayerStoryQuestS storyQuest;
    public PlayerExtraQuestS extraQuest;
    public PlayerCharacterQuestS charaQuest;
    public PlayerHarmonyQuestS harmonyQuest;
    public PlayerSeaQuestS seaQuest;
    public BattleInfo.CallSkillParam playerCallSkillParam;
    public BattleInfo.CallSkillParam enemyCallSkillParam;
    public BattleInfo.WaveInfo[] waveInfos;
    private int mCurrentWave = -1;

    public PlayerDeck tower_deck
    {
      get => this.mTower_deck;
      set => this.mTower_deck = value;
    }

    public int stageId
    {
      get => this.mStageId;
      set
      {
        if (this.isWave)
          return;
        this.mStageId = value;
      }
    }

    public BattleStage stage => MasterData.BattleStage[this.stageId];

    public DeckInfo deck
    {
      get
      {
        if (this.isTempDeck)
        {
          if (this._deck == null)
            this.CreateTempDeck();
          return this._deck;
        }
        switch (this.quest_type)
        {
          case CommonQuestType.Tower:
            return PlayerDeck.createDeckInfo(this.mTower_deck);
          case CommonQuestType.Sea:
            if (this._deck == null)
              this._deck = PlayerSeaDeck.createDeckInfo(SMManager.Get<PlayerSeaDeck[]>()[this.deckIndex]);
            return this._deck;
          case CommonQuestType.GuildRaid:
            return this.isCustomDeck ? PlayerCustomDeck.createGuildRaidDeckInfo(SMManager.Get<PlayerCustomDeck[]>()[this.deckIndex]) : this._deck;
          case CommonQuestType.Corps:
            return this._deck;
          default:
            return this.isCustomDeck ? PlayerCustomDeck.createDeckInfo(SMManager.Get<PlayerCustomDeck[]>()[this.deckIndex]) : PlayerDeck.createDeckInfo(SMManager.Get<PlayerDeck[]>()[this.deckIndex]);
        }
      }
    }

    public PlayerItem[] items
    {
      get
      {
        switch (this.quest_type)
        {
          case CommonQuestType.Tower:
            return SMManager.Get<PlayerItem[]>().AllTowerSupplies();
          case CommonQuestType.GuildRaid:
            return SMManager.Get<PlayerItem[]>().AllRaidSupplies();
          case CommonQuestType.Corps:
            return Array.Find<PlayerCorps>(SMManager.Get<PlayerCorps[]>(), (Predicate<PlayerCorps>) (x => x.period_id == this.period_id)).supplies;
          default:
            return SMManager.Get<PlayerItem[]>().AllBattleSupplies();
        }
      }
    }

    public BattleStageGuest[] Guests
    {
      get
      {
        return this.guest_ids != null ? ((IEnumerable<int>) this.guest_ids).Select<int, BattleStageGuest>((Func<int, BattleStageGuest>) (x => MasterData.BattleStageGuest[x])).ToArray<BattleStageGuest>() : (BattleStageGuest[]) null;
      }
    }

    public BattleEarthStageGuest[] EarthGuests
    {
      get
      {
        return this.guest_ids != null ? ((IEnumerable<int>) this.guest_ids).Select<int, BattleEarthStageGuest>((Func<int, BattleEarthStageGuest>) (x => MasterData.BattleEarthStageGuest[x])).ToArray<BattleEarthStageGuest>() : (BattleEarthStageGuest[]) null;
      }
    }

    public int[] enemy_ids
    {
      get => this.mEnemy_ids;
      set
      {
        if (this.isWave)
          return;
        this.mEnemy_ids = value;
        this.isSplitedFacilityFromEnemy = false;
      }
    }

    public BattleStageEnemy[] Enemies
    {
      get
      {
        return this.enemy_ids != null ? ((IEnumerable<int>) this.enemy_ids).Select<int, BattleStageEnemy>((Func<int, BattleStageEnemy>) (x => MasterData.BattleStageEnemy[x])).ToArray<BattleStageEnemy>() : (BattleStageEnemy[]) null;
      }
    }

    public BattleReinforcement[] EnemyReinforcements
    {
      get
      {
        int[] enemyIds = this.enemy_ids;
        List<int> source = (enemyIds != null ? ((IEnumerable<int>) enemyIds).ToList<int>() : (List<int>) null) ?? new List<int>();
        if (this.enemy_facility_ids != null)
          source.AddRange((IEnumerable<int>) this.enemy_facility_ids);
        return source.Select<int, BattleReinforcement>((Func<int, BattleReinforcement>) (i => MasterData.BattleStageEnemy[i]?.reinforcement)).Where<BattleReinforcement>((Func<BattleReinforcement, bool>) (r => r != null)).ToArray<BattleReinforcement>();
      }
    }

    public Tuple<int, Reward>[] enemy_items
    {
      get => this.mEnemy_items;
      set
      {
        if (this.isWave)
          return;
        this.mEnemy_items = value;
      }
    }

    public Tuple<int, Reward>[] EnemyItems => this.enemy_items;

    public PlayerUnit[] user_units
    {
      get => this.mUser_units;
      set
      {
        if (this.isWave)
          return;
        this.mUser_units = value;
      }
    }

    public PlayerItem[] user_items
    {
      get => this.mUser_items;
      set
      {
        if (this.isWave)
          return;
        this.mUser_items = value;
      }
    }

    public int[] user_enemy_ids
    {
      get => this.mUser_enemy_ids;
      set
      {
        if (this.isWave)
          return;
        this.mUser_enemy_ids = value;
      }
    }

    public BattleStageUserUnit[] UserEnemies
    {
      get
      {
        return this.user_enemy_ids != null ? ((IEnumerable<int>) this.user_enemy_ids).Select<int, BattleStageUserUnit>((Func<int, BattleStageUserUnit>) (x => MasterData.BattleStageUserUnit[x])).ToArray<BattleStageUserUnit>() : (BattleStageUserUnit[]) null;
      }
    }

    public Tuple<int, Reward>[] user_enemy_items
    {
      get => this.mUser_enemy_items;
      set
      {
        if (this.isWave)
          return;
        this.mUser_enemy_items = value;
      }
    }

    public Tuple<int, Reward>[] UserEnemyItems => this.user_enemy_items;

    public int[] panel_ids
    {
      get => this.mPanel_ids;
      set
      {
        if (this.isWave)
          return;
        this.mPanel_ids = value;
      }
    }

    public BattleStagePanelEvent[] Panels
    {
      get
      {
        return this.panel_ids != null ? ((IEnumerable<int>) this.panel_ids).Select<int, BattleStagePanelEvent>((Func<int, BattleStagePanelEvent>) (x => MasterData.BattleStagePanelEvent[x])).ToArray<BattleStagePanelEvent>() : (BattleStagePanelEvent[]) null;
      }
    }

    public Tuple<int, Reward>[] panel_items
    {
      get => this.mPanel_items;
      set
      {
        if (this.isWave)
          return;
        this.mPanel_items = value;
      }
    }

    public Tuple<int, Reward>[] PanelItems => this.panel_items;

    public PlayerUnit[] pvp_player_units { get; set; }

    public PlayerUnit[] pvp_enemy_units { get; set; }

    public PlayerItem[] pvp_player_items { get; set; }

    public PlayerItem[] pvp_enemy_items { get; set; }

    public PlayerGearReisouSchema[] pvp_player_reisou_items { get; set; }

    public PlayerGearReisouSchema[] pvp_enemy_reisou_items { get; set; }

    public PlayerCharacterIntimate[] pvp_player_character_intimates { get; set; }

    public PlayerCharacterIntimate[] pvp_enemy_character_intimates { get; set; }

    public PlayerAwakeSkill[] pvp_player_awake_skill { get; set; }

    public PlayerAwakeSkill[] pvp_enemy_awake_skill { get; set; }

    public PlayerUnit[] gvg_player_helpers { get; set; }

    public PlayerUnit[] gvg_enemy_helpers { get; set; }

    public PlayerItem[] gvg_player_helper_items { get; set; }

    public PlayerItem[] gvg_enemy_helper_items { get; set; }

    public PlayerGearReisouSchema[] gvg_player_helper_reisou_items { get; set; }

    public PlayerGearReisouSchema[] gvg_enemy_helper_reisou_items { get; set; }

    public PlayerCharacterIntimate[] gvg_player_helper_character_intimates { get; set; }

    public PlayerCharacterIntimate[] gvg_enemy_helper_character_intimates { get; set; }

    public PlayerAwakeSkill[] gvg_player_helper_awake_skill { get; set; }

    public PlayerAwakeSkill[] gvg_enemy_helper_awake_skill { get; set; }

    public int Coin => SMManager.Get<Player>().coin;

    public bool isFirstAllDead
    {
      get
      {
        int? gameOverCount = SMManager.Get<Player>().game_over_count;
        int num = 0;
        return gameOverCount.GetValueOrDefault() == num & gameOverCount.HasValue;
      }
    }

    public int[] enemy_facility_ids { get; private set; }

    public bool isExtra => this.extraQuest != null;

    public bool isSea => this.seaQuest != null;

    public bool hasMission
    {
      get
      {
        if (this.isExtra)
        {
          foreach (QuestExtraMission questExtraMission in MasterData.QuestExtraMissionList)
          {
            if (questExtraMission.quest_s.ID == this.extraQuest.quest_extra_s.ID)
              return true;
          }
          return false;
        }
        if (this.isSea)
        {
          if (this.seaQuest == null)
            return false;
          foreach (QuestSeaMission questSeaMission in MasterData.QuestSeaMissionList)
          {
            if (questSeaMission.quest_s.ID == this.seaQuest.quest_sea_s.ID)
              return true;
          }
          return false;
        }
        if (this.storyQuest == null)
          return false;
        foreach (QuestStoryMission questStoryMission in MasterData.QuestStoryMissionList)
        {
          if (questStoryMission.quest_s.ID == this.storyQuest.quest_story_s.ID)
            return true;
        }
        return false;
      }
    }

    public Bonus[] pvp_bonus_list { get; set; }

    public string pvp_start_date { get; set; }

    public GuildBaseBonusEffect[] gvg_player_base_bonus_list { get; set; }

    public GuildBaseBonusEffect[] gvg_enemy_base_bonus_list { get; set; }

    public GvgPeriod gvgPeriod
    {
      get
      {
        GvgPeriod gvgPeriod;
        return this.gvg && MasterData.GvgPeriod.TryGetValue(this.period_id, out gvgPeriod) ? gvgPeriod : (GvgPeriod) null;
      }
    }

    public Checker checkUnitRules
    {
      get
      {
        if (this.quest_type == CommonQuestType.PvP && this.gvg)
        {
          GvgPeriod gvgPeriod = this.gvgPeriod;
          if (gvgPeriod != null && gvgPeriod.rule_no.IsValid())
            return GvgRule.createCheckRules(gvgPeriod.rule_no);
        }
        return (Checker) null;
      }
    }

    public static BattleInfo MakeBattleInfo(
      string battle_uuid,
      CommonQuestType quest_type,
      int quest_s_id,
      int deck_type_id,
      int quest_loop_count,
      int deck_number,
      PlayerHelper helper,
      int[] enemies,
      Tuple<int, int, int, int>[] enemy_items,
      PlayerUnit[] user_units,
      PlayerItem[] user_items,
      int[] user_enemies,
      Tuple<int, int, int, int>[] user_enemy_items,
      int[] panels,
      Tuple<int, int, int, int>[] panel_items,
      int[] guests,
      PlayerUnit[] facility_units,
      Tuple<int, int>[] facility_coordinates)
    {
      BattleInfo battleInfo = new BattleInfo();
      battleInfo.pvp = false;
      battleInfo.helper = helper;
      if (helper != null && helper.leader_unit != (PlayerUnit) null)
        battleInfo.helper_overkillers = helper.leader_unit.cache_overkillers_units;
      battleInfo.quest_s_id = quest_s_id;
      battleInfo.quest_type = quest_type;
      battleInfo.quest_loop_count = quest_loop_count;
      battleInfo.battleId = battle_uuid;
      battleInfo.guest_ids = guests;
      battleInfo.enemy_ids = enemies;
      battleInfo.enemy_items = ((IEnumerable<Tuple<int, int, int, int>>) enemy_items).Select<Tuple<int, int, int, int>, Tuple<int, Reward>>((Func<Tuple<int, int, int, int>, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.Item1, new Reward((MasterDataTable.CommonRewardType) x.Item2, x.Item3, x.Item4)))).ToArray<Tuple<int, Reward>>();
      battleInfo.user_units = user_units;
      battleInfo.user_items = user_items;
      battleInfo.user_enemy_ids = user_enemies;
      battleInfo.user_enemy_items = ((IEnumerable<Tuple<int, int, int, int>>) user_enemy_items).Select<Tuple<int, int, int, int>, Tuple<int, Reward>>((Func<Tuple<int, int, int, int>, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.Item1, new Reward((MasterDataTable.CommonRewardType) x.Item2, x.Item3, x.Item4)))).ToArray<Tuple<int, Reward>>();
      battleInfo.panel_ids = panels;
      battleInfo.panel_items = ((IEnumerable<Tuple<int, int, int, int>>) panel_items).Select<Tuple<int, int, int, int>, Tuple<int, Reward>>((Func<Tuple<int, int, int, int>, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.Item1, new Reward((MasterDataTable.CommonRewardType) x.Item2, x.Item3, x.Item4)))).ToArray<Tuple<int, Reward>>();
      bool flag1 = false;
      bool flag2 = false;
      switch (quest_type)
      {
        case CommonQuestType.Story:
          battleInfo.storyQuest = ((IEnumerable<PlayerStoryQuestS>) SMManager.Get<PlayerStoryQuestS[]>()).First<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.ID == quest_s_id));
          battleInfo.stageId = battleInfo.storyQuest.quest_story_s.stage.ID;
          flag1 = battleInfo.storyQuest.enable_autobattle;
          flag2 = battleInfo.storyQuest.quest_story_s.disable_continue;
          battleInfo.isEarthMode = false;
          battleInfo.isCustomDeck = BattleInfo.checkCustomDeck(deck_type_id);
          battleInfo.deckIndex = BattleInfo.getDeckIndex(deck_type_id, deck_number);
          break;
        case CommonQuestType.Character:
          battleInfo.charaQuest = ((IEnumerable<PlayerCharacterQuestS>) SMManager.Get<PlayerCharacterQuestS[]>()).First<PlayerCharacterQuestS>((Func<PlayerCharacterQuestS, bool>) (x => x.quest_character_s.ID == quest_s_id));
          battleInfo.stageId = battleInfo.charaQuest.quest_character_s.stage.ID;
          flag1 = battleInfo.charaQuest.enable_autobattle;
          flag2 = battleInfo.charaQuest.quest_character_s.disable_continue;
          battleInfo.isEarthMode = false;
          battleInfo.isCustomDeck = BattleInfo.checkCustomDeck(deck_type_id);
          battleInfo.deckIndex = BattleInfo.getDeckIndex(deck_type_id, deck_number);
          break;
        case CommonQuestType.Extra:
          battleInfo.extraQuest = ((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).CheckMasterData().ToArray<PlayerExtraQuestS>()).First<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s.ID == quest_s_id));
          battleInfo.stageId = battleInfo.extraQuest.quest_extra_s.stage.ID;
          flag1 = battleInfo.extraQuest.enable_autobattle;
          flag2 = battleInfo.extraQuest.quest_extra_s.disable_continue;
          battleInfo.isEarthMode = false;
          battleInfo.isTempDeck = deck_type_id == 4;
          battleInfo.isCustomDeck = BattleInfo.checkCustomDeck(deck_type_id);
          if (!battleInfo.isTempDeck)
          {
            battleInfo.deckIndex = BattleInfo.getDeckIndex(deck_type_id, deck_number);
            break;
          }
          break;
        case CommonQuestType.Harmony:
          battleInfo.harmonyQuest = ((IEnumerable<PlayerHarmonyQuestS>) SMManager.Get<PlayerHarmonyQuestS[]>()).First<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (x => x.quest_harmony_s.ID == quest_s_id));
          battleInfo.stageId = battleInfo.harmonyQuest.quest_harmony_s.stage.ID;
          flag1 = battleInfo.harmonyQuest.enable_autobattle;
          flag2 = battleInfo.harmonyQuest.quest_harmony_s.disable_continue;
          battleInfo.isEarthMode = false;
          battleInfo.isCustomDeck = BattleInfo.checkCustomDeck(deck_type_id);
          battleInfo.deckIndex = BattleInfo.getDeckIndex(deck_type_id, deck_number);
          break;
        case CommonQuestType.Earth:
          EarthQuestEpisode earthQuestEpisode = MasterData.EarthQuestEpisode[quest_s_id];
          battleInfo.stageId = earthQuestEpisode.stage.ID;
          flag1 = false;
          flag2 = true;
          battleInfo.isEarthMode = true;
          battleInfo.isExtraEarthQuest = false;
          battleInfo.deckIndex = ((IEnumerable<PlayerDeck>) SMManager.Get<PlayerDeck[]>()).FirstIndexOrNull<PlayerDeck>((Func<PlayerDeck, bool>) (x => x.deck_type_id == deck_type_id && x.deck_number == deck_number)).Value;
          break;
        case CommonQuestType.EarthExtra:
          EarthExtraQuest earthExtraQuest = MasterData.EarthExtraQuest[quest_s_id];
          battleInfo.stageId = earthExtraQuest.stage.ID;
          flag1 = false;
          flag2 = true;
          battleInfo.isEarthMode = true;
          battleInfo.isExtraEarthQuest = true;
          battleInfo.deckIndex = ((IEnumerable<PlayerDeck>) SMManager.Get<PlayerDeck[]>()).FirstIndexOrNull<PlayerDeck>((Func<PlayerDeck, bool>) (x => x.deck_type_id == deck_type_id && x.deck_number == deck_number)).Value;
          break;
        case CommonQuestType.Sea:
          battleInfo.seaQuest = ((IEnumerable<PlayerSeaQuestS>) SMManager.Get<PlayerSeaQuestS[]>()).First<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.quest_sea_s.ID == quest_s_id));
          battleInfo.stageId = battleInfo.seaQuest.quest_sea_s.stage.ID;
          flag1 = battleInfo.seaQuest.enable_autobattle;
          flag2 = battleInfo.seaQuest.quest_sea_s.disable_continue;
          battleInfo.isEarthMode = false;
          battleInfo.isCustomDeck = BattleInfo.checkCustomDeck(deck_type_id);
          battleInfo.deckIndex = BattleInfo.getDeckIndex(deck_type_id, deck_number, true);
          break;
        default:
          Debug.LogError((object) ("error: " + quest_type.ToString()));
          break;
      }
      battleInfo.isContinueEnable = !flag2;
      if (quest_loop_count == 0)
        battleInfo.isAutoBattleEnable = flag1;
      else
        battleInfo.isStoryEnable = false;
      battleInfo.pvp_bonus_list = (Bonus[]) null;
      battleInfo.pvp_start_date = string.Empty;
      battleInfo.gvg_player_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.gvg_enemy_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.facility_units = facility_units;
      battleInfo.facility_coordinates = facility_coordinates;
      battleInfo.playerCallSkillParam = quest_type == CommonQuestType.Earth ? new BattleInfo.CallSkillParam() : SMManager.Get<Player>().GetCallSkillParam();
      battleInfo.enemyCallSkillParam = new BattleInfo.CallSkillParam();
      return battleInfo;
    }

    public static bool checkCustomDeck(int deck_type_id)
    {
      switch (deck_type_id)
      {
        case 5:
        case 6:
        case 7:
          return true;
        default:
          return false;
      }
    }

    private static int getDeckIndex(int deck_type_id, int deck_number, bool bSea = false)
    {
      int? nullable = BattleInfo.checkCustomDeck(deck_type_id) ? ((IEnumerable<PlayerCustomDeck>) SMManager.Get<PlayerCustomDeck[]>()).FirstIndexOrNull<PlayerCustomDeck>((Func<PlayerCustomDeck, bool>) (x => x.deck_type_id == deck_type_id && x.deck_number == deck_number)) : (bSea ? ((IEnumerable<PlayerSeaDeck>) SMManager.Get<PlayerSeaDeck[]>()).FirstIndexOrNull<PlayerSeaDeck>((Func<PlayerSeaDeck, bool>) (x => x.deck_type_id == deck_type_id && x.deck_number == deck_number)) : ((IEnumerable<PlayerDeck>) SMManager.Get<PlayerDeck[]>()).FirstIndexOrNull<PlayerDeck>((Func<PlayerDeck, bool>) (x => x.deck_type_id == deck_type_id && x.deck_number == deck_number)));
      return !nullable.HasValue ? 0 : nullable.Value;
    }

    public static BattleInfo MakePvpBattleInfo(WebAPI.Response.InternalPvpStart result)
    {
      BattleInfo.CallSkillParam callSkillParam1 = result.player1.GetCallSkillParam();
      BattleInfo.CallSkillParam callSkillParam2 = result.player2.GetCallSkillParam();
      return BattleInfo.MakePvpBattleInfo(result.battle_uuid, result.stage.stage_id, ((IEnumerable<PlayerUnit>) result.player1_units).Take<PlayerUnit>(5).ToArray<PlayerUnit>(), ((IEnumerable<PlayerUnit>) result.player2_units).Take<PlayerUnit>(5).ToArray<PlayerUnit>(), result.player1_items, result.player2_items, result.player1_units_over_killers, result.player2_units_over_killers, result.player1_reisou_items, result.player2_reisou_items, result.bonus, result.player1_character_intimates, result.player2_character_intimates, result.player1_awake_skills, result.player2_awake_skills, result.battle_start_at, callSkillParam1, callSkillParam2);
    }

    public static BattleInfo MakePvpBattleInfo(
      string battle_uuid,
      int stage_id,
      PlayerUnit[] player1_units,
      PlayerUnit[] player2_units,
      PlayerItem[] player1_items,
      PlayerItem[] player2_items,
      PlayerUnit[] player1_overkillers,
      PlayerUnit[] player2_overkillers,
      PlayerGearReisouSchema[] player1_reisou_items,
      PlayerGearReisouSchema[] player2_reisou_items,
      Bonus[] bonus_list,
      PlayerCharacterIntimate[] player1_character_intimates,
      PlayerCharacterIntimate[] player2_character_intimates,
      PlayerAwakeSkill[] player1_awake_skills,
      PlayerAwakeSkill[] player2_awake_skills,
      DateTime start_time,
      BattleInfo.CallSkillParam callSkillParamPlayer,
      BattleInfo.CallSkillParam callSkillParamEnemy)
    {
      BattleInfo battleInfo = new BattleInfo();
      battleInfo.pvp = true;
      battleInfo.quest_type = CommonQuestType.PvP;
      battleInfo.stageId = stage_id;
      battleInfo.pvp_player_units = player1_units;
      battleInfo.pvp_enemy_units = player2_units;
      battleInfo.pvp_player_items = player1_items;
      battleInfo.pvp_enemy_items = player2_items;
      if (player1_overkillers != null)
        BattleInfo.attachOverkillers(player1_units, player1_overkillers);
      if (player2_overkillers != null)
        BattleInfo.attachOverkillers(player2_units, player2_overkillers);
      battleInfo.pvp_player_reisou_items = player1_reisou_items;
      battleInfo.pvp_enemy_reisou_items = player2_reisou_items;
      battleInfo.pvp_player_character_intimates = player1_character_intimates;
      battleInfo.pvp_enemy_character_intimates = player2_character_intimates;
      battleInfo.pvp_player_awake_skill = player1_awake_skills;
      battleInfo.pvp_enemy_awake_skill = player2_awake_skills;
      battleInfo.isStoryEnable = false;
      battleInfo.isAutoBattleEnable = PerformanceConfig.GetInstance().EnablePvPAutoButton;
      battleInfo.pvp_bonus_list = bonus_list;
      battleInfo.gvg_player_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.gvg_enemy_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.pvp_start_date = string.Format("{0:D2}{1:D2}", (object) start_time.Month, (object) start_time.Day);
      battleInfo.playerCallSkillParam = callSkillParamPlayer;
      battleInfo.enemyCallSkillParam = callSkillParamEnemy;
      return battleInfo;
    }

    public static BattleInfo MakePvNpcBattleInfo(WebAPI.Response.PvpPlayerNpcStart response)
    {
      BattleInfo battleInfo = new BattleInfo();
      battleInfo.quest_type = CommonQuestType.PvP;
      battleInfo.isStoryEnable = false;
      battleInfo.isAutoBattleEnable = PerformanceConfig.GetInstance().EnablePvPAutoButton;
      battleInfo.pvp = true;
      battleInfo.pvp_vs_npc = true;
      battleInfo.pvp_start_date = string.Format("{0:D2}{1:D2}", (object) response.battle_start_at.Month, (object) response.battle_start_at.Day);
      battleInfo.battleId = response.battle_uuid;
      battleInfo.stageId = response.stage.stage_id;
      battleInfo.pvp_bonus_list = response.bonus;
      battleInfo.pvp_player_units = response.player_units;
      BattleInfo.attachOverkillers(battleInfo.pvp_player_units, response.player_units_over_killers);
      battleInfo.pvp_player_character_intimates = response.player_character_intimates;
      battleInfo.pvp_player_awake_skill = response.player_awake_skills;
      battleInfo.pvp_player_items = response.player_items;
      battleInfo.pvp_player_reisou_items = response.player_reisou_items;
      battleInfo.pvp_enemy_units = response.target_player_units;
      BattleInfo.attachOverkillers(battleInfo.pvp_enemy_units, response.target_player_units_over_killers);
      battleInfo.pvp_enemy_character_intimates = response.target_player_character_intimates;
      battleInfo.pvp_enemy_awake_skill = response.target_player_awake_skills;
      battleInfo.pvp_enemy_items = response.target_player_items;
      battleInfo.pvp_enemy_reisou_items = response.target_player_reisou_items;
      battleInfo.playerCallSkillParam = response.player.GetCallSkillParam();
      battleInfo.enemyCallSkillParam = response.target_player.GetCallSkillParam();
      return battleInfo;
    }

    private static void attachOverkillers(PlayerUnit[] playerUnits, PlayerUnit[] overkillers)
    {
      if (playerUnits == null)
        return;
      for (int index = 0; index < playerUnits.Length; ++index)
      {
        if (playerUnits[index] != (PlayerUnit) null)
        {
          playerUnits[index].importOverkillersUnits(overkillers);
          playerUnits[index].resetOverkillersParameter();
          playerUnits[index].resetOverkillersSkills();
        }
      }
    }

    public static BattleInfo MakeGvgBattleInfo(
      string battle_uuid,
      int period_id,
      int stage_id,
      PlayerUnit[] player_units,
      PlayerUnit[] enemy_units,
      PlayerUnit[] player_guest_units,
      PlayerUnit[] enemy_guest_units,
      PlayerItem[] player_items,
      PlayerItem[] enemy_items,
      PlayerItem[] player_guest_items,
      PlayerItem[] enemy_guest_items,
      PlayerGearReisouSchema[] player_reisou_items,
      PlayerGearReisouSchema[] enemy_reisou_items,
      PlayerGearReisouSchema[] player_guest_reisou_items,
      PlayerGearReisouSchema[] enemy_guest_reisou_items,
      GuildBaseBonusEffect[] player_base_bonus_list,
      GuildBaseBonusEffect[] enemy_base_bonus_list,
      PlayerCharacterIntimate[] player1_character_intimates,
      PlayerCharacterIntimate[] player2_character_intimates,
      PlayerAwakeSkill[] player1_awake_skills,
      PlayerAwakeSkill[] player1_guest_awake_skills,
      PlayerAwakeSkill[] player2_awake_skills,
      PlayerAwakeSkill[] player2_guest_awake_skills,
      DateTime start_time,
      PlayerUnit[] facility_units,
      Tuple<int, int>[] facility_coordinates)
    {
      return new BattleInfo()
      {
        battleId = battle_uuid,
        gvg = true,
        quest_type = CommonQuestType.PvP,
        period_id = period_id,
        stageId = stage_id,
        pvp_player_units = player_units,
        gvg_player_helpers = player_guest_units,
        pvp_enemy_units = enemy_units,
        gvg_enemy_helpers = enemy_guest_units,
        pvp_player_items = player_items,
        pvp_enemy_items = enemy_items,
        pvp_player_reisou_items = player_reisou_items,
        pvp_enemy_reisou_items = enemy_reisou_items,
        pvp_player_character_intimates = player1_character_intimates,
        pvp_enemy_character_intimates = player2_character_intimates,
        pvp_player_awake_skill = player1_awake_skills,
        pvp_enemy_awake_skill = player2_awake_skills,
        gvg_player_helper_items = player_guest_items,
        gvg_enemy_helper_items = enemy_guest_items,
        gvg_player_helper_reisou_items = player_guest_reisou_items,
        gvg_enemy_helper_reisou_items = enemy_guest_reisou_items,
        gvg_player_helper_awake_skill = player1_guest_awake_skills,
        gvg_enemy_helper_awake_skill = player2_guest_awake_skills,
        isStoryEnable = false,
        isAutoBattleEnable = true,
        pvp_bonus_list = (Bonus[]) null,
        gvg_player_base_bonus_list = player_base_bonus_list,
        gvg_enemy_base_bonus_list = enemy_base_bonus_list,
        pvp_start_date = string.Format("{0:D2}{1:D2}", (object) start_time.Month, (object) start_time.Day),
        facility_units = facility_units,
        facility_coordinates = facility_coordinates,
        playerCallSkillParam = SMManager.Get<Player>().GetCallSkillParam(),
        enemyCallSkillParam = new BattleInfo.CallSkillParam()
      };
    }

    public static BattleInfo MakeTowerBattleInfo(
      string battle_uuid,
      int tower_loop_count,
      int stage_id,
      TowerEnemy[] enemies,
      Tuple<int, int, int, int>[] enemy_items,
      PlayerDeck tower_deck,
      PlayerUnit[] user_units,
      PlayerItem[] user_items)
    {
      BattleInfo battleInfo = new BattleInfo();
      battleInfo.pvp = false;
      battleInfo.helper = (PlayerHelper) null;
      battleInfo.quest_type = CommonQuestType.Tower;
      battleInfo.quest_loop_count = tower_loop_count;
      battleInfo.stageId = stage_id;
      battleInfo.isEarthMode = false;
      battleInfo.battleId = battle_uuid;
      battleInfo.enemy_ids = ((IEnumerable<TowerEnemy>) enemies).Select<TowerEnemy, int>((Func<TowerEnemy, int>) (x => x.id)).ToArray<int>();
      battleInfo.enemy_items = ((IEnumerable<Tuple<int, int, int, int>>) enemy_items).Select<Tuple<int, int, int, int>, Tuple<int, Reward>>((Func<Tuple<int, int, int, int>, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.Item1, new Reward((MasterDataTable.CommonRewardType) x.Item2, x.Item3, x.Item4)))).ToArray<Tuple<int, Reward>>();
      battleInfo.tower_deck = tower_deck;
      battleInfo.user_units = user_units;
      battleInfo.user_items = user_items;
      battleInfo.isStoryEnable = tower_loop_count == 1;
      battleInfo.isAutoBattleEnable = true;
      battleInfo.isContinueEnable = false;
      battleInfo.pvp_bonus_list = (Bonus[]) null;
      battleInfo.pvp_start_date = string.Empty;
      battleInfo.gvg_player_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.gvg_enemy_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.user_enemy_ids = new int[0];
      battleInfo.user_enemy_items = new Tuple<int, Reward>[0];
      battleInfo.guest_ids = new int[0];
      battleInfo.enemyHpRate = new Dictionary<int, float>();
      foreach (TowerEnemy enemy in enemies)
        battleInfo.enemyHpRate.Add(enemy.id, enemy.hitpoint_rate);
      battleInfo.playerCallSkillParam = SMManager.Get<Player>().GetCallSkillParam();
      battleInfo.enemyCallSkillParam = new BattleInfo.CallSkillParam();
      return battleInfo;
    }

    public static BattleInfo MakeCorpsBattleInfo(
      WebAPI.Response.QuestCorpsBattleStart webResponse,
      int period_id)
    {
      BattleInfo battleInfo = new BattleInfo();
      battleInfo.pvp = false;
      battleInfo.isEarthMode = false;
      battleInfo.isStoryEnable = true;
      battleInfo.isAutoBattleEnable = true;
      battleInfo.isContinueEnable = false;
      battleInfo.battleId = webResponse.battle_uuid;
      battleInfo.quest_type = (CommonQuestType) webResponse.quest_type;
      battleInfo.stageId = webResponse.player_corps_stage.stage_id;
      battleInfo.period_id = period_id;
      PlayerDeck d = new PlayerDeck() { member_limit = 6 };
      d.player_unit_ids = new int?[d.member_limit];
      for (int index = 0; index < webResponse.player_corps_deck.deck_player_unit_ids.Length; ++index)
        d.player_unit_ids[index] = new int?(webResponse.player_corps_deck.deck_player_unit_ids[index]);
      battleInfo.CreateTempDeck(d);
      battleInfo.enemy_ids = ((IEnumerable<CorpsEnemyStatus>) webResponse.player_corps_stage.enemies).Select<CorpsEnemyStatus, int>((Func<CorpsEnemyStatus, int>) (x => x.id)).ToArray<int>();
      battleInfo.enemy_items = ((IEnumerable<WebAPI.Response.QuestCorpsBattleStartEnemy_items>) webResponse.enemy_items).Select<WebAPI.Response.QuestCorpsBattleStartEnemy_items, Tuple<int, Reward>>((Func<WebAPI.Response.QuestCorpsBattleStartEnemy_items, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.id, new Reward((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToArray<Tuple<int, Reward>>();
      battleInfo.enemyRestHp = !webResponse.player_corps_stage.is_first ? ((IEnumerable<CorpsEnemyStatus>) webResponse.player_corps_stage.enemies).ToDictionary<CorpsEnemyStatus, int, int>((Func<CorpsEnemyStatus, int>) (x => x.id), (Func<CorpsEnemyStatus, int>) (x => x.hp)) : new Dictionary<int, int>(0);
      battleInfo.playerCallSkillParam = SMManager.Get<Player>().GetCallSkillParam();
      battleInfo.enemyCallSkillParam = new BattleInfo.CallSkillParam();
      return battleInfo;
    }

    public static BattleInfo MakeCorpsBattleInfo(
      WebAPI.Response.QuestCorpsBattleResume webResponse,
      int period_id)
    {
      BattleInfo battleInfo = new BattleInfo();
      battleInfo.pvp = false;
      battleInfo.isEarthMode = false;
      battleInfo.isStoryEnable = true;
      battleInfo.isAutoBattleEnable = true;
      battleInfo.isContinueEnable = false;
      battleInfo.battleId = webResponse.battle_uuid;
      battleInfo.quest_type = (CommonQuestType) webResponse.quest_type;
      battleInfo.stageId = webResponse.player_corps_stage.stage_id;
      battleInfo.period_id = period_id;
      PlayerDeck d = new PlayerDeck() { member_limit = 6 };
      d.player_unit_ids = new int?[d.member_limit];
      for (int index = 0; index < webResponse.player_corps_deck.deck_player_unit_ids.Length; ++index)
        d.player_unit_ids[index] = new int?(webResponse.player_corps_deck.deck_player_unit_ids[index]);
      battleInfo.CreateTempDeck(d);
      battleInfo.enemy_ids = ((IEnumerable<CorpsEnemyStatus>) webResponse.player_corps_stage.enemies).Select<CorpsEnemyStatus, int>((Func<CorpsEnemyStatus, int>) (x => x.id)).ToArray<int>();
      battleInfo.enemy_items = ((IEnumerable<WebAPI.Response.QuestCorpsBattleResumeEnemy_items>) webResponse.enemy_items).Select<WebAPI.Response.QuestCorpsBattleResumeEnemy_items, Tuple<int, Reward>>((Func<WebAPI.Response.QuestCorpsBattleResumeEnemy_items, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.id, new Reward((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToArray<Tuple<int, Reward>>();
      battleInfo.enemyRestHp = !webResponse.player_corps_stage.is_first ? ((IEnumerable<CorpsEnemyStatus>) webResponse.player_corps_stage.enemies).ToDictionary<CorpsEnemyStatus, int, int>((Func<CorpsEnemyStatus, int>) (x => x.id), (Func<CorpsEnemyStatus, int>) (x => x.hp)) : new Dictionary<int, int>(0);
      battleInfo.playerCallSkillParam = SMManager.Get<Player>().GetCallSkillParam();
      battleInfo.enemyCallSkillParam = new BattleInfo.CallSkillParam();
      return battleInfo;
    }

    public static BattleInfo MakeRaidBattleInfo(WebAPI.Response.GuildraidBattleStart webResponse)
    {
      BattleInfo battleInfo = new BattleInfo()
      {
        isSimulation = webResponse.is_simulation,
        pvp = false,
        isEarthMode = false,
        battleId = webResponse.battle_uuid,
        quest_loop_count = webResponse.quest_loop_count,
        raidEndlessLoopCount = webResponse.loop_count
      };
      battleInfo.isStoryEnable = !battleInfo.isSimulation;
      battleInfo.isAutoBattleEnable = true;
      battleInfo.isContinueEnable = false;
      battleInfo.quest_type = (CommonQuestType) webResponse.quest_type;
      battleInfo.quest_s_id = webResponse.quest_s_id;
      battleInfo.stageId = MasterData.GuildRaid[battleInfo.quest_s_id].stage_id;
      battleInfo.isCustomDeck = BattleInfo.checkCustomDeck(webResponse.deck_type_id);
      PlayerHelper[] helpers = webResponse.helpers;
      for (int index = 0; index < helpers.Length; ++index)
      {
        helpers[index].leader_unit = webResponse.helper_player_units[index];
        helpers[index].leader_unit.importOverkillersUnits(webResponse.helper_player_unit_over_killers);
        helpers[index].leader_unit.primary_equipped_gear = helpers[index].leader_unit.FindEquippedGear(webResponse.helper_player_gears);
        helpers[index].leader_unit.primary_equipped_gear2 = helpers[index].leader_unit.FindEquippedGear2(webResponse.helper_player_gears);
        helpers[index].leader_unit.primary_equipped_gear3 = helpers[index].leader_unit.FindEquippedGear3(webResponse.helper_player_gears);
        helpers[index].leader_unit.primary_equipped_reisou = helpers[index].leader_unit.FindEquippedReisou(webResponse.helper_player_gears, webResponse.helper_player_reisou_gears);
        helpers[index].leader_unit.primary_equipped_reisou2 = helpers[index].leader_unit.FindEquippedReisou2(webResponse.helper_player_gears, webResponse.helper_player_reisou_gears);
        helpers[index].leader_unit.primary_equipped_reisou3 = helpers[index].leader_unit.FindEquippedReisou3(webResponse.helper_player_gears, webResponse.helper_player_reisou_gears);
        helpers[index].leader_unit.primary_equipped_awake_skill = helpers[index].leader_unit.FindEquippedExtraSkill(webResponse.helper_player_awake_skills);
        helpers[index].leader_unit.usedPrimary = PlayerUnit.UsedPrimary.All;
      }
      battleInfo.helper = ((IEnumerable<PlayerHelper>) helpers).FirstOrDefault<PlayerHelper>();
      if (battleInfo.helper != null && battleInfo.helper.leader_unit != (PlayerUnit) null)
        battleInfo.helper_overkillers = battleInfo.helper.leader_unit.cache_overkillers_units;
      battleInfo.guest_ids = webResponse.guest_ids;
      battleInfo.user_units = webResponse.player_units;
      battleInfo.user_items = webResponse.player_gears;
      battleInfo.user_enemy_ids = webResponse.user_deck_enemy;
      battleInfo.user_enemy_items = ((IEnumerable<WebAPI.Response.GuildraidBattleStartUser_deck_enemy_item>) webResponse.user_deck_enemy_item).Select<WebAPI.Response.GuildraidBattleStartUser_deck_enemy_item, Tuple<int, Reward>>((Func<WebAPI.Response.GuildraidBattleStartUser_deck_enemy_item, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.id, new Reward((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToArray<Tuple<int, Reward>>();
      battleInfo.enemy_ids = webResponse.enemy;
      battleInfo.enemy_items = ((IEnumerable<WebAPI.Response.GuildraidBattleStartEnemy_item>) webResponse.enemy_item).Select<WebAPI.Response.GuildraidBattleStartEnemy_item, Tuple<int, Reward>>((Func<WebAPI.Response.GuildraidBattleStartEnemy_item, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.id, new Reward((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToArray<Tuple<int, Reward>>();
      battleInfo.panel_ids = webResponse.panel;
      battleInfo.panel_items = ((IEnumerable<WebAPI.Response.GuildraidBattleStartPanel_item>) webResponse.panel_item).Select<WebAPI.Response.GuildraidBattleStartPanel_item, Tuple<int, Reward>>((Func<WebAPI.Response.GuildraidBattleStartPanel_item, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.id, new Reward((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToArray<Tuple<int, Reward>>();
      battleInfo.pvp_bonus_list = (Bonus[]) null;
      battleInfo.pvp_start_date = string.Empty;
      battleInfo.gvg_player_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.gvg_enemy_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.facility_units = (PlayerUnit[]) null;
      battleInfo.facility_coordinates = (Tuple<int, int>[]) null;
      battleInfo.raidBossDamage = new KeyValuePair<int, int>(webResponse.boss_id, webResponse.boss_total_damage);
      if (battleInfo.isCustomDeck)
        battleInfo.deckIndex = BattleInfo.getDeckIndex(webResponse.deck_type_id, webResponse.deck_number);
      else
        battleInfo.CreateTempDeck();
      battleInfo.playerCallSkillParam = SMManager.Get<Player>().GetCallSkillParam();
      battleInfo.enemyCallSkillParam = new BattleInfo.CallSkillParam();
      return battleInfo;
    }

    public void CreateTempDeck(PlayerDeck d = null)
    {
      if (d == null)
      {
        d = new PlayerDeck();
        d.member_limit = 5;
        d.player_unit_ids = new int?[d.member_limit];
        for (int index = 0; index < this.user_units.Length; ++index)
          d.player_unit_ids[index] = new int?(this.user_units[index].id);
      }
      this._deck = PlayerDeck.createDeckInfo(d);
    }

    public static BattleInfo MakeRaidBattleInfo(WebAPI.Response.GuildraidBattleResume webResponse)
    {
      BattleInfo battleInfo = new BattleInfo()
      {
        isSimulation = webResponse.is_simulation,
        pvp = false,
        isEarthMode = false,
        battleId = webResponse.battle_uuid,
        quest_loop_count = webResponse.quest_loop_count
      };
      battleInfo.isStoryEnable = !battleInfo.isSimulation;
      battleInfo.isAutoBattleEnable = true;
      battleInfo.isContinueEnable = false;
      battleInfo.quest_type = (CommonQuestType) webResponse.quest_type;
      battleInfo.quest_s_id = webResponse.quest_s_id;
      battleInfo.stageId = MasterData.GuildRaid[battleInfo.quest_s_id].stage_id;
      battleInfo.isCustomDeck = BattleInfo.checkCustomDeck(webResponse.deck_type_id);
      PlayerHelper[] helpers = webResponse.helpers;
      for (int index = 0; index < helpers.Length; ++index)
      {
        helpers[index].leader_unit = webResponse.helper_player_units[index];
        helpers[index].leader_unit.primary_equipped_gear = helpers[index].leader_unit.FindEquippedGear(webResponse.helper_player_gears);
        helpers[index].leader_unit.primary_equipped_gear2 = helpers[index].leader_unit.FindEquippedGear2(webResponse.helper_player_gears);
        helpers[index].leader_unit.primary_equipped_gear3 = helpers[index].leader_unit.FindEquippedGear3(webResponse.helper_player_gears);
        helpers[index].leader_unit.primary_equipped_reisou = helpers[index].leader_unit.FindEquippedReisou(webResponse.helper_player_gears, webResponse.helper_player_reisou_gears);
        helpers[index].leader_unit.primary_equipped_reisou2 = helpers[index].leader_unit.FindEquippedReisou2(webResponse.helper_player_gears, webResponse.helper_player_reisou_gears);
        helpers[index].leader_unit.primary_equipped_reisou3 = helpers[index].leader_unit.FindEquippedReisou3(webResponse.helper_player_gears, webResponse.helper_player_reisou_gears);
        helpers[index].leader_unit.primary_equipped_awake_skill = helpers[index].leader_unit.FindEquippedExtraSkill(webResponse.helper_player_awake_skills);
        helpers[index].leader_unit.usedPrimary = PlayerUnit.UsedPrimary.All;
      }
      battleInfo.helper = ((IEnumerable<PlayerHelper>) helpers).FirstOrDefault<PlayerHelper>();
      battleInfo.guest_ids = webResponse.guest_ids;
      battleInfo.user_units = webResponse.player_units;
      battleInfo.user_items = webResponse.player_gears;
      battleInfo.user_enemy_ids = webResponse.user_deck_enemy;
      battleInfo.user_enemy_items = ((IEnumerable<WebAPI.Response.GuildraidBattleResumeUser_deck_enemy_item>) webResponse.user_deck_enemy_item).Select<WebAPI.Response.GuildraidBattleResumeUser_deck_enemy_item, Tuple<int, Reward>>((Func<WebAPI.Response.GuildraidBattleResumeUser_deck_enemy_item, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.id, new Reward((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToArray<Tuple<int, Reward>>();
      battleInfo.enemy_ids = webResponse.enemy;
      battleInfo.enemy_items = ((IEnumerable<WebAPI.Response.GuildraidBattleResumeEnemy_item>) webResponse.enemy_item).Select<WebAPI.Response.GuildraidBattleResumeEnemy_item, Tuple<int, Reward>>((Func<WebAPI.Response.GuildraidBattleResumeEnemy_item, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.id, new Reward((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToArray<Tuple<int, Reward>>();
      battleInfo.panel_ids = webResponse.panel;
      battleInfo.panel_items = ((IEnumerable<WebAPI.Response.GuildraidBattleResumePanel_item>) webResponse.panel_item).Select<WebAPI.Response.GuildraidBattleResumePanel_item, Tuple<int, Reward>>((Func<WebAPI.Response.GuildraidBattleResumePanel_item, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.id, new Reward((MasterDataTable.CommonRewardType) x.reward_type_id, x.reward_id, x.reward_quantity)))).ToArray<Tuple<int, Reward>>();
      battleInfo.pvp_bonus_list = (Bonus[]) null;
      battleInfo.pvp_start_date = string.Empty;
      battleInfo.gvg_player_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.gvg_enemy_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.facility_units = (PlayerUnit[]) null;
      battleInfo.facility_coordinates = (Tuple<int, int>[]) null;
      battleInfo.raidBossDamage = new KeyValuePair<int, int>(webResponse.boss_id, webResponse.boss_total_damage);
      battleInfo.raidEndlessLoopCount = webResponse.loop_count;
      if (battleInfo.isCustomDeck)
      {
        battleInfo.deckIndex = BattleInfo.getDeckIndex(webResponse.deck_type_id, webResponse.deck_number);
      }
      else
      {
        PlayerDeck d = new PlayerDeck() { member_limit = 5 };
        d.player_unit_ids = new int?[d.member_limit];
        for (int index = 0; index < battleInfo.user_units.Length; ++index)
          d.player_unit_ids[index] = new int?(battleInfo.user_units[index].id);
        battleInfo.CreateTempDeck(d);
      }
      battleInfo.playerCallSkillParam = SMManager.Get<Player>().GetCallSkillParam();
      battleInfo.enemyCallSkillParam = new BattleInfo.CallSkillParam();
      return battleInfo;
    }

    public bool isWave => this.waveInfos != null;

    public int currentWave
    {
      get => this.mCurrentWave;
      set
      {
        if (!this.isWave || this.mCurrentWave == value)
          return;
        this.mCurrentWave = value;
        BattleInfo.WaveInfo waveInfo = this.waveInfos[value];
        this.mStageId = waveInfo.stage_id;
        this.mUser_units = waveInfo.user_units;
        this.mUser_items = waveInfo.user_items;
        this.mEnemy_ids = waveInfo.enemy_ids;
        this.mEnemy_items = waveInfo.enemy_items;
        this.mUser_enemy_ids = waveInfo.user_enemy_ids;
        this.mUser_enemy_items = waveInfo.user_enemy_items;
        this.mPanel_ids = waveInfo.panel_ids;
        this.mPanel_items = waveInfo.panel_items;
        this.isSplitedFacilityFromEnemy = false;
      }
    }

    public static BattleInfo MakeBattleInfo(
      string battle_uuid,
      CommonQuestType quest_type,
      int quest_s_id,
      int deck_type_id,
      int quest_loop_count,
      int deck_number,
      PlayerHelper helper,
      int[] guests,
      IEnumerable<BattleInfo.Wave> wave)
    {
      BattleInfo battleInfo = new BattleInfo();
      battleInfo.pvp = false;
      battleInfo.helper = helper;
      if (helper != null && helper.leader_unit != (PlayerUnit) null)
        battleInfo.helper_overkillers = helper.leader_unit.cache_overkillers_units;
      battleInfo.quest_s_id = quest_s_id;
      battleInfo.quest_type = quest_type;
      battleInfo.quest_loop_count = quest_loop_count;
      battleInfo.waveInfos = wave.Select<BattleInfo.Wave, BattleInfo.WaveInfo>((Func<BattleInfo.Wave, BattleInfo.WaveInfo>) (x => new BattleInfo.WaveInfo(x))).ToArray<BattleInfo.WaveInfo>();
      bool flag1 = false;
      bool flag2 = false;
      switch (quest_type)
      {
        case CommonQuestType.Story:
          battleInfo.storyQuest = ((IEnumerable<PlayerStoryQuestS>) SMManager.Get<PlayerStoryQuestS[]>()).First<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.ID == quest_s_id));
          flag1 = battleInfo.storyQuest.enable_autobattle;
          flag2 = battleInfo.storyQuest.quest_story_s.disable_continue;
          battleInfo.isEarthMode = false;
          break;
        case CommonQuestType.Character:
          battleInfo.charaQuest = ((IEnumerable<PlayerCharacterQuestS>) SMManager.Get<PlayerCharacterQuestS[]>()).First<PlayerCharacterQuestS>((Func<PlayerCharacterQuestS, bool>) (x => x.quest_character_s.ID == quest_s_id));
          flag1 = battleInfo.charaQuest.enable_autobattle;
          flag2 = battleInfo.charaQuest.quest_character_s.disable_continue;
          battleInfo.isEarthMode = false;
          break;
        case CommonQuestType.Extra:
          battleInfo.extraQuest = ((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).CheckMasterData().ToArray<PlayerExtraQuestS>()).First<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s.ID == quest_s_id));
          flag1 = battleInfo.extraQuest.enable_autobattle;
          flag2 = battleInfo.extraQuest.quest_extra_s.disable_continue;
          battleInfo.isEarthMode = false;
          break;
        case CommonQuestType.Harmony:
          battleInfo.harmonyQuest = ((IEnumerable<PlayerHarmonyQuestS>) SMManager.Get<PlayerHarmonyQuestS[]>()).First<PlayerHarmonyQuestS>((Func<PlayerHarmonyQuestS, bool>) (x => x.quest_harmony_s.ID == quest_s_id));
          flag1 = battleInfo.harmonyQuest.enable_autobattle;
          flag2 = battleInfo.harmonyQuest.quest_harmony_s.disable_continue;
          battleInfo.isEarthMode = false;
          break;
        case CommonQuestType.Earth:
          EarthQuestEpisode earthQuestEpisode = MasterData.EarthQuestEpisode[quest_s_id];
          flag1 = false;
          flag2 = true;
          battleInfo.isEarthMode = true;
          break;
        case CommonQuestType.Sea:
          battleInfo.seaQuest = ((IEnumerable<PlayerSeaQuestS>) SMManager.Get<PlayerSeaQuestS[]>()).First<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.quest_sea_s.ID == quest_s_id));
          flag1 = battleInfo.seaQuest.enable_autobattle;
          flag2 = battleInfo.seaQuest.quest_sea_s.disable_continue;
          battleInfo.isEarthMode = false;
          break;
        default:
          Debug.LogError((object) ("error: " + quest_type.ToString()));
          break;
      }
      if (battleInfo.isEarthMode)
      {
        battleInfo.deckIndex = ((IEnumerable<PlayerDeck>) SMManager.Get<PlayerDeck[]>()).FirstIndexOrNull<PlayerDeck>((Func<PlayerDeck, bool>) (x => x.deck_type_id == deck_type_id && x.deck_number == deck_number)).Value;
      }
      else
      {
        battleInfo.isCustomDeck = BattleInfo.checkCustomDeck(deck_type_id);
        battleInfo.deckIndex = BattleInfo.getDeckIndex(deck_type_id, deck_number, battleInfo.isSea);
      }
      battleInfo.battleId = battle_uuid;
      battleInfo.guest_ids = guests;
      battleInfo.isContinueEnable = !flag2;
      if (quest_loop_count == 0)
        battleInfo.isAutoBattleEnable = flag1;
      else
        battleInfo.isStoryEnable = false;
      battleInfo.pvp_bonus_list = (Bonus[]) null;
      battleInfo.pvp_start_date = string.Empty;
      battleInfo.gvg_player_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.gvg_enemy_base_bonus_list = (GuildBaseBonusEffect[]) null;
      battleInfo.playerCallSkillParam = SMManager.Get<Player>().GetCallSkillParam();
      battleInfo.enemyCallSkillParam = new BattleInfo.CallSkillParam();
      return battleInfo;
    }

    public void SplitFacilityFromEnemyIds()
    {
      if (this.isSplitedFacilityFromEnemy || this.enemy_ids == null || this.enemy_ids.Length == 0)
        return;
      int[] enemies;
      PlayerUnit[] facilities;
      Tuple<int, int>[] facilityCoordinates;
      BattleInfo.SplitFacilityFromEnemyIds((IEnumerable<int>) this.enemy_ids, out enemies, out facilities, out facilityCoordinates);
      this.mEnemy_ids = enemies;
      this.enemy_facility_ids = ((IEnumerable<PlayerUnit>) facilities).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (f => f.id)).ToArray<int>();
      this.isSplitedFacilityFromEnemy = true;
      if (facilities.Length == 0)
        return;
      if (this.facility_units != null && this.facility_units.Length != 0)
      {
        this.facility_units = ((IEnumerable<PlayerUnit>) this.facility_units).Concat<PlayerUnit>((IEnumerable<PlayerUnit>) facilities).ToArray<PlayerUnit>();
        this.facility_coordinates = ((IEnumerable<Tuple<int, int>>) this.facility_coordinates).Concat<Tuple<int, int>>((IEnumerable<Tuple<int, int>>) facilityCoordinates).ToArray<Tuple<int, int>>();
      }
      else
      {
        this.facility_units = facilities;
        this.facility_coordinates = facilityCoordinates;
      }
    }

    public static void SplitFacilityFromEnemyIds(
      IEnumerable<int> ids,
      out int[] enemies,
      out PlayerUnit[] facilities,
      out Tuple<int, int>[] facilityCoordinates)
    {
      IEnumerable<Tuple<BattleStageEnemy, UnitUnit>> tuples = ids.Select<int, Tuple<BattleStageEnemy, UnitUnit>>((Func<int, Tuple<BattleStageEnemy, UnitUnit>>) (i =>
      {
        BattleStageEnemy battleStageEnemy = (BattleStageEnemy) null;
        MasterData.BattleStageEnemy.TryGetValue(i, out battleStageEnemy);
        UnitUnit unit = battleStageEnemy?.unit;
        return unit == null ? (Tuple<BattleStageEnemy, UnitUnit>) null : Tuple.Create<BattleStageEnemy, UnitUnit>(battleStageEnemy, unit);
      })).Where<Tuple<BattleStageEnemy, UnitUnit>>((Func<Tuple<BattleStageEnemy, UnitUnit>, bool>) (e => e != null));
      List<int> intList = new List<int>();
      List<Tuple<BattleStageEnemy, UnitUnit>> source = new List<Tuple<BattleStageEnemy, UnitUnit>>();
      FacilityLevel[] facilityLevelList = MasterData.FacilityLevelList;
      foreach (Tuple<BattleStageEnemy, UnitUnit> tuple in tuples)
      {
        Tuple<BattleStageEnemy, UnitUnit> s = tuple;
        if (Array.Find<FacilityLevel>(facilityLevelList, (Predicate<FacilityLevel>) (f => f.unit_UnitUnit == s.Item1.unit_UnitUnit)) == null)
          intList.Add(s.Item1.ID);
        else
          source.Add(Tuple.Create<BattleStageEnemy, UnitUnit>(s.Item1, s.Item2));
      }
      enemies = intList.ToArray();
      facilityCoordinates = source.Select<Tuple<BattleStageEnemy, UnitUnit>, Tuple<int, int>>((Func<Tuple<BattleStageEnemy, UnitUnit>, Tuple<int, int>>) (fl => Tuple.Create<int, int>(fl.Item1.initial_coordinate_x, fl.Item1.initial_coordinate_y))).ToArray<Tuple<int, int>>();
      facilities = source.Select<Tuple<BattleStageEnemy, UnitUnit>, PlayerUnit>((Func<Tuple<BattleStageEnemy, UnitUnit>, PlayerUnit>) (fl => PlayerUnit.FromFacility(fl.Item2, fl.Item1))).ToArray<PlayerUnit>();
    }

    public enum DeckType
    {
      Normal = 1,
      Sea = 2,
      GuildRaid = 3,
      Solo = 4,
      Custom = 5,
      CustomGvgAttack = 6,
      CustomGvgDefense = 7,
    }

    [Serializable]
    public class CallSkillParam
    {
      public int same_character_id;
      public int intimate_rank;
      public int player_rank;
    }

    public class Wave
    {
      public int stage_id;
      public PlayerUnit[] user_units;
      public PlayerItem[] user_items;
      public int[] enemies;
      public Tuple<int, int, int, int>[] enemy_items;
      public int[] user_enemies;
      public Tuple<int, int, int, int>[] user_enemy_items;
      public int[] panels;
      public Tuple<int, int, int, int>[] panel_items;
    }

    [Serializable]
    public class WaveInfo
    {
      public int stage_id;
      public PlayerUnit[] user_units;
      public PlayerItem[] user_items;
      public int[] enemy_ids;
      public Tuple<int, Reward>[] enemy_items = new Tuple<int, Reward>[0];
      public int[] user_enemy_ids = new int[0];
      public Tuple<int, Reward>[] user_enemy_items = new Tuple<int, Reward>[0];
      public int[] panel_ids = new int[0];
      public Tuple<int, Reward>[] panel_items = new Tuple<int, Reward>[0];

      public WaveInfo(BattleInfo.Wave wave)
      {
        this.stage_id = wave.stage_id;
        this.user_units = wave.user_units;
        this.user_items = wave.user_items;
        this.enemy_ids = wave.enemies;
        this.enemy_items = ((IEnumerable<Tuple<int, int, int, int>>) wave.enemy_items).Select<Tuple<int, int, int, int>, Tuple<int, Reward>>((Func<Tuple<int, int, int, int>, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.Item1, new Reward((MasterDataTable.CommonRewardType) x.Item2, x.Item3, x.Item4)))).ToArray<Tuple<int, Reward>>();
        this.user_enemy_ids = wave.user_enemies;
        this.user_enemy_items = ((IEnumerable<Tuple<int, int, int, int>>) wave.user_enemy_items).Select<Tuple<int, int, int, int>, Tuple<int, Reward>>((Func<Tuple<int, int, int, int>, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.Item1, new Reward((MasterDataTable.CommonRewardType) x.Item2, x.Item3, x.Item4)))).ToArray<Tuple<int, Reward>>();
        this.panel_ids = wave.panels;
        this.panel_items = ((IEnumerable<Tuple<int, int, int, int>>) wave.panel_items).Select<Tuple<int, int, int, int>, Tuple<int, Reward>>((Func<Tuple<int, int, int, int>, Tuple<int, Reward>>) (x => Tuple.Create<int, Reward>(x.Item1, new Reward((MasterDataTable.CommonRewardType) x.Item2, x.Item3, x.Item4)))).ToArray<Tuple<int, Reward>>();
      }
    }
  }
}
