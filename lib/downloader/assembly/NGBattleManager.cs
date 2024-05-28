// Decompiled with JetBrains decompiler
// Type: NGBattleManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Client;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class NGBattleManager : Singleton<NGBattleManager>
{
  public string topSceneNormal = "battleUI_01";
  public string topScenePvp = "battleUI_pvp";
  public string topSceneEarth = "battleUI_51";
  public string topSceneGvg = "battleUI_guild";
  public string topSceneSea = "battleUI_01_sea";
  public string duelSceneNormal = "battle018_1";
  public string duelSceneSea = "battle018_1_sea";
  public string storySceneNormal = "story009_3";
  public string storySceneSea = "story009_3_sea";
  public bool noDuelScene;
  private GameObject managers;
  private GameObject effects;
  private System.Type[] managerList;
  private BattleInfo mBattleInfo;
  private string mErrorString;
  private bool _thisInitialized;
  private bool _envInitialized;
  private BE _environment;
  private PVPManager _pvpManager;
  private PVNpcManager _pvnpcManager;
  private GVGManager _gvgManager;
  public bool isRetire;
  public Vector2 mapOffset = new Vector2(-10f, -5f);
  public float unitAngle = -40f;
  public float panelSize = 2f;
  public GameObject battleCamera;
  public Vector3 cameraPositionOffset = new Vector3(0.0f, 0.0f, 5f);
  public float cameraAngle = -40f;
  public float cameraSwipeTime = 0.3f;
  public float cameraSwipeVelocity = 1f;
  public float cameraSwipeMove = 6f;
  public float defaultUnitSpeed = 4f;
  public float[] sightDistances;
  private string effectPrefabPath = "Animations/battle_effect/{0}";
  public BattleEffects battleEffects;
  public bool isSuspend;
  public GameObject fieldJumpEffectPrefab;
  public bool isDeadEffectPlaying;
  public bool isAfterDuelEffectWaiting;
  private Battle01SelectNode selectNode;
  private NGBattleManager.OrderValues orderValues;
  private const string strErrorCodeInfoIsNull = "エラーコード:100";
  private const string strErrorCodeDeckIsNull = "エラーコード:200";
  private const string strErrorCodeDeckUnitsIsNull = "エラーコード:300";
  private const string strErrorCodeRMIsNull = "エラーコード:400";
  private const string strErrorCodeEnemiesIsNull = "エラーコード:101";
  private const string strErrorCodeEnemyIsNull = "エラーコード:102";
  private const string strErrorCodeUserEnemiesIsNull = "エラーコード:103";
  private const string strErrorCodeUserEnemyIsNull = "エラーコード:104";
  private const string strErrorCodeEarthGuestsIsNull = "エラーコード:105";
  private const string strErrorCodeEarthGuestIsNull = "エラーコード:106";
  private const string strErrorCodeGuestsIsNull = "エラーコード:107";
  private const string strErrorCodeGuestIsNull = "エラーコード:108";
  private const string strErrorCodePvNpcStartError = "エラーコード:500";
  private const string strErrorCodePvPStartError = "エラーコード:600";
  private const string strErrorCodeGvGStartError = "エラーコード:700";
  private const string strErrorCodeQuestResumeFailed = "エラーコード:800";
  public bool mIsBattleEnable;
  public BattleInfo CharacterQuestAfterBattleInfo;
  public int CharacterQuestAfterBattleScriptId;
  [NonSerialized]
  private Dictionary<int, BattleskillEffect[]> battleSkillEffect_ = new Dictionary<int, BattleskillEffect[]>();

  public string topScene
  {
    get
    {
      if (this.isPvp || this.isPvnpc)
        return this.topScenePvp;
      if (this.isGvg)
        return this.topSceneGvg;
      if (this.isEarth)
        return this.topSceneEarth;
      return this.isSea ? this.topSceneSea : this.topSceneNormal;
    }
  }

  public string duelScene => this.isSea ? this.duelSceneSea : this.duelSceneNormal;

  public string storyScene => this.isSea ? this.storySceneSea : this.storySceneNormal;

  public BattleInfo battleInfo
  {
    get => this.mBattleInfo;
    set => this.mBattleInfo = value;
  }

  public bool isError => this.mErrorString != null;

  public string errorString
  {
    get => this.mErrorString;
    set => this.mErrorString = value;
  }

  public bool initialized
  {
    get => this._thisInitialized && this._envInitialized;
    set => this._thisInitialized = this._envInitialized = value;
  }

  public BE environment
  {
    get => this._environment;
    set => this._environment = value;
  }

  public bool isWave => this.mBattleInfo != null && this.mBattleInfo.isWave;

  public int waveLength => this.isWave ? this.mBattleInfo.waveInfos.Length : -1;

  public PVPManager pvpManager => this._pvpManager;

  public bool isPvp
  {
    get
    {
      if (Object.op_Inequality((Object) this._pvpManager, (Object) null))
        return true;
      return this.mBattleInfo != null && this.mBattleInfo.pvp && !this.mBattleInfo.pvp_vs_npc;
    }
  }

  public PVNpcManager pvnpcManager => this._pvnpcManager;

  public bool isPvnpc
  {
    get
    {
      if (Object.op_Inequality((Object) this._pvnpcManager, (Object) null))
        return true;
      return this.mBattleInfo != null && this.mBattleInfo.pvp && this.mBattleInfo.pvp_vs_npc;
    }
  }

  public GVGManager gvgManager => this._gvgManager;

  public bool isGvg => Object.op_Inequality((Object) this._gvgManager, (Object) null);

  public bool isOvo => this.isPvp || this.isPvnpc || this.isGvg;

  public int timeLimit
  {
    get
    {
      int maxValue = int.MaxValue;
      if (this.isPvp)
        maxValue = this.pvpManager.timeLimit.value;
      else if (this.isPvnpc)
        maxValue = this.pvnpcManager.timeLimit.value;
      else if (this.isGvg)
        maxValue = this.gvgManager.timeLimit.value;
      return maxValue;
    }
  }

  public bool isTower
  {
    get => this.mBattleInfo != null && this.mBattleInfo.quest_type == CommonQuestType.Tower;
  }

  public bool isRaid
  {
    get => this.mBattleInfo != null && this.mBattleInfo.quest_type == CommonQuestType.GuildRaid;
  }

  public bool isCorps
  {
    get => this.mBattleInfo != null && this.mBattleInfo.quest_type == CommonQuestType.Corps;
  }

  public bool isRaidBoss(BL.Unit unit)
  {
    return this.isRaid && this.environment.core.enemyUnits.value.First<BL.Unit>((Func<BL.Unit, bool>) (u => u.playerUnit.id == this.battleInfo.raidBossDamage.Key)).Equals(unit);
  }

  public IGameEngine gameEngine
  {
    get
    {
      if (Object.op_Inequality((Object) this._pvnpcManager, (Object) null))
        return (IGameEngine) this._pvnpcManager;
      if (Object.op_Inequality((Object) this._pvpManager, (Object) null))
        return (IGameEngine) this._pvpManager;
      return Object.op_Inequality((Object) this._gvgManager, (Object) null) ? (IGameEngine) this._gvgManager : (IGameEngine) null;
    }
  }

  public bool useGameEngine => this.gameEngine != null;

  public bool isEarth => this.mBattleInfo != null && this.mBattleInfo.isEarthMode;

  public bool isSea
  {
    get => this.mBattleInfo != null && this.mBattleInfo.isSea && this.mBattleInfo.seaQuest != null;
  }

  public void setUiNode(Battle01SelectNode node) => this.selectNode = node;

  public Battle01SelectNode getUiNode() => this.selectNode;

  public int order
  {
    set => this.orderValues = new NGBattleManager.OrderValues(value, this);
    get
    {
      if (this.orderValues == null)
        this.orderValues = new NGBattleManager.OrderValues(1, this);
      return this.orderValues.order;
    }
  }

  public float unitAngleValue => this.orderValues.unitAngleValue;

  public float cameraAngleYValue => this.orderValues.cameraAngleYValue;

  public Vector3 cameraPositionOffsetValue => this.orderValues.cameraPositionOffsetValue;

  public Vector3 unitPositionOffsetValue => this.orderValues.unitPositionOffsetValue;

  public Vector3 unitShadowOffsetValue => this.orderValues.unitShadowOffsetValue;

  public Quaternion unitNonTransformRotationValue => this.orderValues.unitNonTransformRotationValue;

  public void setManagers(GameObject o) => this.managers = o;

  protected override void Initialize()
  {
    this.managerList = new List<System.Type>()
    {
      typeof (NGBattleUIManager),
      typeof (NGBattleAIManager),
      typeof (NGBattle3DObjectManager),
      typeof (TreasureBoxManager),
      typeof (BattleTimeManager)
    }.ToArray();
  }

  public IEnumerator initMasterData(BattleInfo info = null)
  {
    if (info == null)
      info = this.mBattleInfo;
    IEnumerator e = MasterData.LoadBattleMapLandform(info.stage.map);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.battleInfo.enemy_ids != null && this.battleInfo.enemy_ids.Length != 0)
    {
      e = MasterData.LoadBattleStageEnemy(info.stage);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (this.battleInfo.user_enemy_ids != null && this.battleInfo.user_enemy_ids.Length != 0)
    {
      e = MasterData.LoadBattleStageUserUnit(info.stage);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void resetEnvironment()
  {
    this.environment = new BE();
    this.environment.core = (BL) new Core(this.environment);
    BattleFuncs.environment.Reset(this.environment.core);
  }

  public IEnumerator makeResumeInfo()
  {
    Future<WebAPI.Response.BattleResume> battleF = WebAPI.BattleResume((Action<WebAPI.Response.UserError>) (e => ModalWindow.Show(e.Code, e.Reason, (Action) (() =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
    }))));
    IEnumerator e1 = battleF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (battleF.Result != null)
    {
      WebAPI.Response.BattleResume battle = battleF.Result;
      bool flag = battle.deck_type_id == 4;
      PlayerHelper helper;
      int[] guests;
      if (!flag)
      {
        for (int index = 0; index < battle.helpers.Length; ++index)
        {
          battle.helpers[index].leader_unit = battle.helper_player_units[index];
          battle.helpers[index].leader_unit.importOverkillersUnits(battle.helper_player_unit_over_killers);
          battle.helpers[index].leader_unit.primary_equipped_gear = battle.helpers[index].leader_unit.FindEquippedGear(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_gear2 = battle.helpers[index].leader_unit.FindEquippedGear2(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_gear3 = battle.helpers[index].leader_unit.FindEquippedGear3(battle.helper_player_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou = battle.helpers[index].leader_unit.FindEquippedReisou(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou2 = battle.helpers[index].leader_unit.FindEquippedReisou2(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_reisou3 = battle.helpers[index].leader_unit.FindEquippedReisou3(battle.helper_player_gears, battle.helper_player_reisou_gears);
          battle.helpers[index].leader_unit.primary_equipped_awake_skill = battle.helpers[index].leader_unit.FindEquippedExtraSkill(battle.helper_player_awake_skills);
        }
        helper = ((IEnumerable<PlayerHelper>) battle.helpers).FirstOrDefault<PlayerHelper>();
        guests = GuestUnit.GetGuestsID(battle.quest_s_id);
      }
      else
      {
        helper = (PlayerHelper) null;
        guests = new int[0];
      }
      if (battle.wave_stage.Length != 0)
      {
        List<BattleInfo.Wave> wave = new List<BattleInfo.Wave>();
        foreach (BattleWaveStageInfo battleWaveStageInfo in battle.wave_stage)
          wave.Add(new BattleInfo.Wave()
          {
            stage_id = battleWaveStageInfo.stage_id,
            enemies = battleWaveStageInfo.enemy,
            enemy_items = ((IEnumerable<BattleWaveStageInfoEnemy_item>) battleWaveStageInfo.enemy_item).Select<BattleWaveStageInfoEnemy_item, Tuple<int, int, int, int>>((Func<BattleWaveStageInfoEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(),
            user_enemies = battleWaveStageInfo.user_deck_enemy,
            user_enemy_items = ((IEnumerable<BattleWaveStageInfoUser_deck_enemy_item>) battleWaveStageInfo.user_deck_enemy_item).Select<BattleWaveStageInfoUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<BattleWaveStageInfoUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(),
            panels = battleWaveStageInfo.panel,
            panel_items = ((IEnumerable<BattleWaveStageInfoPanel_item>) battleWaveStageInfo.panel_item).Select<BattleWaveStageInfoPanel_item, Tuple<int, int, int, int>>((Func<BattleWaveStageInfoPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(),
            user_units = battleWaveStageInfo.user_deck_units,
            user_items = battleWaveStageInfo.user_deck_gears
          });
        this.mBattleInfo = BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, helper, guests, (IEnumerable<BattleInfo.Wave>) wave);
      }
      else
      {
        PlayerUnit[] playerUnitArray;
        if (flag)
          playerUnitArray = new PlayerUnit[1]
          {
            Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == battle.player_unit_id))
          };
        else
          playerUnitArray = battle.user_deck_units;
        PlayerUnit[] user_units = playerUnitArray;
        this.mBattleInfo = BattleInfo.MakeBattleInfo(battle.battle_uuid, (CommonQuestType) battle.quest_type, battle.quest_s_id, battle.deck_type_id, battle.quest_loop_count, battle.deck_number, helper, battle.enemy, ((IEnumerable<WebAPI.Response.BattleResumeEnemy_item>) battle.enemy_item).Select<WebAPI.Response.BattleResumeEnemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleResumeEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), user_units, battle.user_deck_gears, battle.user_deck_enemy, ((IEnumerable<WebAPI.Response.BattleResumeUser_deck_enemy_item>) battle.user_deck_enemy_item).Select<WebAPI.Response.BattleResumeUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleResumeUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), battle.panel, ((IEnumerable<WebAPI.Response.BattleResumePanel_item>) battle.panel_item).Select<WebAPI.Response.BattleResumePanel_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleResumePanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), guests, (PlayerUnit[]) null, (Tuple<int, int>[]) null);
      }
      this.mBattleInfo.isLoadData = false;
    }
  }

  public IEnumerator resumeTowerBattle()
  {
    Future<WebAPI.Response.TowerBattleResume> ft = WebAPI.TowerBattleResume((Action<WebAPI.Response.UserError>) (e => ModalWindow.Show(e.Code, e.Reason, (Action) (() =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
    }))));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (ft.Result != null)
    {
      int stage_id = -1;
      TowerStage towerStage = ((IEnumerable<TowerStage>) MasterData.TowerStageList).FirstOrDefault<TowerStage>((Func<TowerStage, bool>) (x => x.tower_id == ft.Result.tower_id && x.floor == ft.Result.floor));
      if (towerStage != null)
        stage_id = towerStage.stage_id;
      Tuple<int, int, int, int>[] array = ((IEnumerable<WebAPI.Response.TowerBattleResumeEnemy_items>) ft.Result.enemy_items).Select<WebAPI.Response.TowerBattleResumeEnemy_items, Tuple<int, int, int, int>>((Func<WebAPI.Response.TowerBattleResumeEnemy_items, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>();
      PlayerDeck tower_deck = new PlayerDeck();
      tower_deck.member_limit = 6;
      List<int?> nullableList = new List<int?>();
      for (int index = 0; index < tower_deck.member_limit; ++index)
      {
        if (index < ft.Result.tower_deck_units.Length)
          nullableList.Add(new int?(ft.Result.tower_deck_units[index].player_unit_id));
        else
          nullableList.Add(new int?());
      }
      tower_deck.player_unit_ids = nullableList.ToArray();
      tower_deck.cost_limit = 999;
      tower_deck.deck_number = 0;
      this.mBattleInfo = BattleInfo.MakeTowerBattleInfo(ft.Result.battle_uuid, ft.Result.completed_count, stage_id, ft.Result.enemies, array, tower_deck, new PlayerUnit[0], new PlayerItem[0]);
    }
  }

  public IEnumerator resumeCorpsBattle(int period_id)
  {
    bool errorWait = true;
    Future<WebAPI.Response.QuestCorpsBattleResume> battleF = WebAPI.QuestCorpsBattleResume((Action<WebAPI.Response.UserError>) (e => ModalWindow.Show(e.Code, e.Reason, (Action) (() => errorWait = false))));
    IEnumerator e1 = battleF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (battleF.Result == null)
    {
      while (errorWait)
        yield return (object) null;
      e1 = WebAPI.QuestCorpsBattleForceClose().Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    else
    {
      this.mBattleInfo = BattleInfo.MakeCorpsBattleInfo(battleF.Result, period_id);
      this.mBattleInfo.isLoadData = false;
    }
  }

  public IEnumerator resumeRaidBattle()
  {
    bool errorWait = true;
    Future<WebAPI.Response.GuildraidBattleResume> battleF = WebAPI.GuildraidBattleResume((Action<WebAPI.Response.UserError>) (e => ModalWindow.Show(e.Code, e.Reason, (Action) (() => errorWait = false))));
    IEnumerator e1 = battleF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (battleF.Result == null)
    {
      while (errorWait)
        yield return (object) null;
      e1 = WebAPI.BattleForceClose().Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    else
    {
      this.mBattleInfo = BattleInfo.MakeRaidBattleInfo(battleF.Result);
      this.mBattleInfo.isLoadData = false;
    }
  }

  public IEnumerator initEnvironment(int continueCount)
  {
    if (!this._envInitialized)
    {
      bool isModeResume = false;
      CommonQuestType mode = CommonQuestType.Story;
      int period_id = -1;
      if (this.mBattleInfo != null && this.mBattleInfo.isResume)
      {
        isModeResume = true;
        mode = this.mBattleInfo.quest_type;
        period_id = this.mBattleInfo.period_id;
        this.mBattleInfo = (BattleInfo) null;
      }
      this.isRetire = false;
      Future<None> pvnpcF;
      IEnumerator e;
      if (this.mBattleInfo != null && this.mBattleInfo.pvp)
      {
        if (this.mBattleInfo.pvp_vs_npc)
        {
          this._pvnpcManager = PVNpcManager.createPVNpcManager();
          pvnpcF = this._pvnpcManager.startBattle(this.mBattleInfo, this.mBattleInfo.pvp_restart);
          e = pvnpcF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (pvnpcF.Exception != null)
            this.setError("エラーコード:500");
          pvnpcF = (Future<None>) null;
        }
        else
        {
          this._pvpManager = PVPManager.createPVPManager();
          pvnpcF = this._pvpManager.startPVP(this.mBattleInfo.host, this.mBattleInfo.port, this.mBattleInfo.battleToken, this.mBattleInfo.pvp_restart);
          e = pvnpcF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (pvnpcF.Exception != null)
            this.setError("エラーコード:600");
          pvnpcF = (Future<None>) null;
        }
      }
      else if (this.mBattleInfo != null && this.mBattleInfo.gvg)
      {
        this._gvgManager = GVGManager.createGVGManager();
        pvnpcF = this._gvgManager.startGVG(this.mBattleInfo, this.mBattleInfo.pvp_restart);
        e = pvnpcF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (pvnpcF.Exception != null)
          this.setError("エラーコード:700");
        pvnpcF = (Future<None>) null;
      }
      else
      {
        this.order = 0;
        if (this.hasSavedEnvironment())
        {
          try
          {
            this.loadEnvironment();
            this.mBattleInfo = this.environment.core.battleInfo;
            this.mBattleInfo.isLoadData = true;
            if (this.mBattleInfo.quest_type == CommonQuestType.Sea)
              Singleton<NGGameDataManager>.GetInstance().IsSea = true;
            this.environment.core.continueCount = continueCount;
            BattleFuncs.environment.Reset(this.environment.core);
            this._envInitialized = true;
          }
          catch (Exception ex)
          {
            Debug.LogError((object) "loadEnvironment() failed: {0}, {1}\n{2}\n{3}".F((object) ex, (object) ex.Message, (object) ex.Source, (object) ex.StackTrace));
            this.mBattleInfo = (BattleInfo) null;
            this.deleteSavedEnvironment();
          }
          if (this._envInitialized)
          {
            yield return (object) this.initMasterData(this.mBattleInfo);
            yield break;
          }
        }
        if (this.mBattleInfo == null)
        {
          yield return (object) this.resumeQuestFromWeb(isModeResume, mode, period_id);
          if (this.mBattleInfo == null)
          {
            this.setError("エラーコード:800");
            yield break;
          }
        }
        if (this.checkForStoryOnly())
        {
          yield break;
        }
        else
        {
          this.resetEnvironment();
          yield return (object) new BattleLogicInitializer().doStart(this.mBattleInfo, this.environment.core);
          this.environment.core.battleInfo = this.mBattleInfo;
          this.environment.core.continueCount = continueCount;
          this.environment.core.battleLogger.Random();
          if (this.isEarth)
            this.saveEnvironment();
        }
      }
      this._envInitialized = true;
    }
  }

  private IEnumerator resumeQuestFromWeb(bool isModeResume, CommonQuestType mode, int period_id)
  {
    if (isModeResume)
    {
      switch (mode)
      {
        case CommonQuestType.Tower:
          yield return (object) this.resumeTowerBattle();
          break;
        case CommonQuestType.GuildRaid:
          yield return (object) this.resumeRaidBattle();
          break;
        case CommonQuestType.Corps:
          yield return (object) this.resumeCorpsBattle(period_id);
          break;
        default:
          Debug.LogError((object) "UnSupport Quest Type Resume.");
          break;
      }
    }
    else
      yield return (object) this.makeResumeInfo();
  }

  public IEnumerator initBattle()
  {
    if (!this._thisInitialized)
    {
      this.managers = new GameObject("Battle Managers");
      string[] strArray = new string[5]
      {
        "NGBattleUIManager/",
        "NGBattleAIManager/",
        "NGBattle3DObjectManager/",
        "TreasureBoxManager/",
        "BattleTimeManager/"
      };
      System.Type[] typeArray = this.managerList;
      IEnumerator e;
      for (int index = 0; index < typeArray.Length; ++index)
      {
        e = (this.managers.AddComponent(typeArray[index]) as BattleManagerBase).initialize(this.battleInfo);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      typeArray = (System.Type[]) null;
      string path = this.effectPrefabPath.F((object) MasterData.CommonQuestBattleEffect[(int) this.battleInfo.quest_type].file_name);
      Future<GameObject> fo = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
      e = fo.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = fo.Result;
      float num = 960f / (float) ((Component) Singleton<CommonRoot>.GetInstance()).GetComponent<UIRoot>().manualHeight;
      this.effects = new GameObject("Effects Root");
      this.effects.transform.localPosition = Vector3.zero;
      this.effects.transform.localScale = new Vector3(num, num, 1f);
      this.battleEffects = result.CloneAndAddComponent<BattleEffects>(this.effects.transform);
      e = this.battleEffects.onBattleInitSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<NGDuelDataManager>.GetInstance().Init();
      e = Singleton<NGDuelDataManager>.GetInstance().CreateMapCache(this.environment.core.stage, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = Singleton<NGDuelDataManager>.GetInstance().PreloadCommonDuelEffect();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this._thisInitialized = true;
    }
  }

  public IEnumerator cleanupBattle()
  {
    IEnumerator e;
    if (Object.op_Inequality((Object) this.managers, (Object) null))
    {
      System.Type[] typeArray = this.managerList;
      for (int index = 0; index < typeArray.Length; ++index)
      {
        BattleManagerBase component = this.managers.GetComponent(typeArray[index]) as BattleManagerBase;
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          e = component.cleanup();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      typeArray = (System.Type[]) null;
      Object.Destroy((Object) this.managers);
      this.managers = (GameObject) null;
    }
    if (Object.op_Inequality((Object) this.effects, (Object) null))
    {
      Object.Destroy((Object) this.effects);
      this.effects = (GameObject) null;
    }
    yield return (object) PVPManager.destroyPVPManager();
    yield return (object) PVNpcManager.destroyPVNpcManager();
    yield return (object) GVGManager.destroyGVGManager();
    e = Singleton<NGGameDataManager>.GetInstance().LoadOtherBattleAtlas();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.isSuspend)
      this.deleteSavedEnvironment();
    this.GetSaveData().Clear();
    this.orderValues = (NGBattleManager.OrderValues) null;
    this.environment = (BE) null;
    this.battleCamera = (GameObject) null;
    this.errorString = (string) null;
    this.battleEffects = (BattleEffects) null;
    this.battleInfo = (BattleInfo) null;
    this._thisInitialized = false;
    this._envInitialized = false;
    this.battleSkillEffect_ = new Dictionary<int, BattleskillEffect[]>();
    BattleskillEffect.AllClearCache();
    BattleLandformEffect.AllClearCache();
  }

  public T getManager<T>() where T : BattleManagerBase
  {
    return Object.op_Equality((Object) this.managers, (Object) null) ? default (T) : this.managers.GetComponent<T>();
  }

  public T getController<T>() where T : BattleMonoBehaviour
  {
    return Object.op_Equality((Object) this.managers, (Object) null) ? default (T) : this.managers.GetComponent<T>();
  }

  private void setError(string s)
  {
    this.errorString = s;
    Debug.LogError((object) s);
  }

  public List<string> createResourceLoadList(BattleInfo info = null)
  {
    if (info == null)
    {
      info = this.mBattleInfo;
      return new List<string>();
    }
    if (info == null)
    {
      this.setError("エラーコード:100");
      return new List<string>();
    }
    if (info.deck == null)
    {
      this.setError("エラーコード:200");
      return new List<string>();
    }
    PlayerUnit[] playerUnits = info.deck.player_units;
    if (playerUnits == null)
    {
      this.setError("エラーコード:300");
      return new List<string>();
    }
    ResourceManager instance = Singleton<ResourceManager>.GetInstance();
    if (Object.op_Equality((Object) instance, (Object) null))
    {
      this.setError("エラーコード:400");
      return new List<string>();
    }
    List<string> resourceLoadList = new List<string>();
    if (this.isOvo)
    {
      foreach (PlayerUnit playerUnit in ((IEnumerable<PlayerUnit>) info.pvp_player_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)))
        resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(playerUnit.unit));
      if (info.gvg_player_helpers != null)
      {
        foreach (PlayerUnit playerUnit in ((IEnumerable<PlayerUnit>) info.gvg_player_helpers).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)))
          resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(playerUnit.unit));
      }
      foreach (PlayerUnit playerUnit in ((IEnumerable<PlayerUnit>) info.pvp_enemy_units).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)))
        resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(playerUnit.unit));
      if (info.gvg_enemy_helpers != null)
      {
        foreach (PlayerUnit playerUnit in ((IEnumerable<PlayerUnit>) info.gvg_enemy_helpers).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)))
          resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(playerUnit.unit));
      }
    }
    else
    {
      if (info.Enemies == null)
      {
        this.setError("エラーコード:101");
        return new List<string>();
      }
      foreach (BattleStageEnemy enemy in info.Enemies)
      {
        if (enemy == null)
        {
          this.setError("エラーコード:102");
          return new List<string>();
        }
      }
      if (info.UserEnemies == null)
      {
        this.setError("エラーコード:103");
        return new List<string>();
      }
      foreach (BattleStageUserUnit userEnemy in info.UserEnemies)
      {
        if (userEnemy == null)
        {
          this.setError("エラーコード:104");
          return new List<string>();
        }
      }
      if (this.battleInfo.quest_type == CommonQuestType.Earth || this.battleInfo.quest_type == CommonQuestType.EarthExtra)
      {
        if (info.EarthGuests == null)
        {
          this.setError("エラーコード:105");
          return new List<string>();
        }
        foreach (BattleEarthStageGuest earthGuest in info.EarthGuests)
        {
          if (earthGuest == null)
          {
            this.setError("エラーコード:106");
            return new List<string>();
          }
        }
      }
      else
      {
        if (info.Guests == null)
        {
          this.setError("エラーコード:107");
          return new List<string>();
        }
        foreach (BattleStageGuest guest in info.Guests)
        {
          if (guest == null)
          {
            this.setError("エラーコード:108");
            return new List<string>();
          }
        }
      }
      foreach (PlayerUnit playerUnit in ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)))
        resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(playerUnit.unit));
      if (info.helper != null && info.helper.leader_unit != (PlayerUnit) null && info.helper.leader_unit.unit != null)
        resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(info.helper.leader_unit.unit));
      if (info.Enemies != null)
      {
        foreach (BattleStageEnemy enemy in info.Enemies)
          resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(enemy.unit));
      }
      if (info.user_units != null)
      {
        foreach (PlayerUnit userUnit in info.user_units)
          resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(userUnit.unit));
      }
      if (this.battleInfo.quest_type == CommonQuestType.Earth || this.battleInfo.quest_type == CommonQuestType.EarthExtra)
      {
        if (info.EarthGuests != null)
        {
          foreach (BattleEarthStageGuest earthGuest in info.EarthGuests)
            resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(earthGuest.unit));
        }
      }
      else if (info.Guests != null)
      {
        foreach (BattleStageGuest guest in info.Guests)
          resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromUnit(guest.unit));
      }
    }
    resourceLoadList.AddRange((IEnumerable<string>) instance.PathsFromBattleMap(info.stage.map));
    return resourceLoadList;
  }

  private IEnumerator doStartBattle(int continueCount)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    this.errorString = (string) null;
    IEnumerator e = this.initEnvironment(continueCount);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.isError)
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromBgm(this.battleInfo.stage.field_player_bgm_file));
      stringList.AddRange((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromBgm(this.battleInfo.stage.field_enemy_bgm_file));
      yield return (object) OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) stringList.ToArray(), false);
      if (this.checkForStoryOnly())
      {
        this.startStoryOnlyScript();
        yield break;
      }
    }
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    e = sm.destroyLoadedScenesImmediate();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGGameDataManager>.GetInstance().CacheClear();
    sm.changeScene(this.topScene, false);
    e = Singleton<NGGameDataManager>.GetInstance().UnLoadOtherBattleAtlas();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GC.Collect();
    GC.WaitForPendingFinalizers();
    Singleton<ResourceManager>.GetInstance().ClearCache();
    yield return (object) Resources.UnloadUnusedAssets();
  }

  public void startBattle(BattleInfo battleInfo, int continueCount = 0)
  {
    if (battleInfo != null)
      this.battleInfo = battleInfo;
    this.StartCoroutine(this.doStartBattle(continueCount));
  }

  private bool duelHpSkillEffects(
    Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> skillTargets,
    Dictionary<BL.Unit, Tuple<int, int>> hpCount,
    BL.Unit attack,
    BL.Unit defense)
  {
    bool flag1 = false;
    HashSet<BL.Unit> hpDispUnit = new HashSet<BL.Unit>();
    Tuple<BL.Unit, BattleskillSkill> hpDispKey = (Tuple<BL.Unit, BattleskillSkill>) null;
    foreach (Tuple<BL.Unit, BattleskillSkill> key in skillTargets.Keys)
    {
      List<BL.Unit> skillTarget = skillTargets[key];
      if (skillTarget.Count >= 1)
      {
        foreach (BL.Unit unit in skillTarget)
        {
          if (unit.isView)
            hpDispUnit.Add(unit);
        }
        hpDispKey = key;
      }
    }
    foreach (Tuple<BL.Unit, BattleskillSkill> key1 in skillTargets.Keys)
    {
      Tuple<BL.Unit, BattleskillSkill> key = key1;
      if (skillTargets[key].Count<BL.Unit>() >= 1)
      {
        BattleskillSkill battleskillSkill = key.Item2;
        BL.Unit unit1 = key.Item1;
        bool flag2 = attack == unit1;
        IEnumerable<BattleskillEffect> source1 = ((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.curse_reflection || x.EffectLogic.Enum == BattleskillEffectLogicEnum.life_absorb));
        HashSet<BL.Unit> source2 = (HashSet<BL.Unit>) null;
        if (source1 != null && source1.Count<BattleskillEffect>() > 0)
        {
          source2 = new HashSet<BL.Unit>();
          foreach (BattleskillEffect battleskillEffect in source1)
          {
            BL.Unit unit2 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) == 0 ? (flag2 ? attack : defense) : (flag2 ? defense : attack);
            source2.Add(unit2);
          }
        }
        BE.SkillResource skillResource = this.environment.skillResource[battleskillSkill.passive_effect.ID];
        List<BL.Unit> skillTarget = skillTargets[key];
        this.battleEffects.skillFieldEffectStartCore(battleskillSkill.passive_effect, (BL.Unit) null, skillTarget.ToList<BL.Unit>(), skillResource.effectPrefab, skillResource.invokedEffectPrefab, skillResource.targetEffectPrefab, (Action) null, (Action) (() =>
        {
          if (key != hpDispKey)
            return;
          foreach (BL.Unit key2 in hpDispUnit)
          {
            BE.UnitResource unitResource = this.environment.unitResource[key2];
            unitResource.unitParts_.dispHpNumber(hpCount[key2].Item1, hpCount[key2].Item2);
            unitResource.unitParts_.setHpGauge(hpCount[key2].Item1, hpCount[key2].Item2);
          }
        }), source2.ToList<BL.Unit>());
        flag1 = true;
      }
    }
    return flag1;
  }

  private void setBattleEffectSchedule(
    BattleTimeManager btm,
    List<BL.DuelTurn> turns,
    BE.UnitResource attack,
    int preAttackerHP,
    BE.UnitResource defense,
    int preDefenseHP)
  {
    BL.DuelTurn turnLast = turns.LastOrDefault<BL.DuelTurn>();
    btm.setScheduleAction((Action) (() =>
    {
      if (turnLast.isAtackker)
        attack.unitParts_.SetRun(true);
      else
        defense.unitParts_.SetRun(true);
    }));
    foreach (BL.DuelTurn turn1 in turns)
    {
      BL.DuelTurn turn = turn1;
      if (((IEnumerable<BL.Skill>) turn.invokeDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
      {
        BattleskillGenre? genre1 = x.genre1;
        BattleskillGenre battleskillGenre = BattleskillGenre.attack;
        return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
      })) || ((IEnumerable<BL.Skill>) turn.invokeDefenderDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
      {
        BattleskillGenre? genre1 = x.genre1;
        BattleskillGenre battleskillGenre = BattleskillGenre.defense;
        return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
      })))
      {
        btm.setScheduleAction((Action) (() =>
        {
          if (((IEnumerable<BL.Skill>) turn.invokeDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
          {
            BattleskillGenre? genre1 = x.genre1;
            BattleskillGenre battleskillGenre = BattleskillGenre.attack;
            return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
          })))
          {
            BL.Skill skill = ((IEnumerable<BL.Skill>) turn.invokeDuelSkills).FirstOrDefault<BL.Skill>((Func<BL.Skill, bool>) (x =>
            {
              BattleskillGenre? genre1 = x.genre1;
              BattleskillGenre battleskillGenre = BattleskillGenre.attack;
              return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
            }));
            if (skill != null)
            {
              if (turn.isAtackker)
                attack.unitParts_.DispSkillIconEffect(skill.skill);
              else
                defense.unitParts_.DispSkillIconEffect(skill.skill);
            }
          }
          if (!((IEnumerable<BL.Skill>) turn.invokeDefenderDuelSkills).Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
          {
            BattleskillGenre? genre1 = x.genre1;
            BattleskillGenre battleskillGenre = BattleskillGenre.defense;
            return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
          })))
            return;
          BL.Skill skill1 = ((IEnumerable<BL.Skill>) turn.invokeDefenderDuelSkills).FirstOrDefault<BL.Skill>((Func<BL.Skill, bool>) (x =>
          {
            BattleskillGenre? genre1 = x.genre1;
            BattleskillGenre battleskillGenre = BattleskillGenre.defense;
            return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
          }));
          if (skill1 == null)
            return;
          if (!turn.isAtackker)
            attack.unitParts_.DispSkillIconEffect(skill1.skill);
          else
            defense.unitParts_.DispSkillIconEffect(skill1.skill);
        }));
        btm.setEnableWait(1f);
      }
    }
    btm.setEnableWait(0.5f);
    btm.setScheduleAction((Action) (() =>
    {
      if (turnLast.isAtackker)
      {
        attack.unitParts_.SetRun(false);
        if (preDefenseHP != turnLast.defenderRestHp)
          defense.unitParts_.dispHpNumber(preDefenseHP, turnLast.defenderRestHp);
        defense.unitParts_.DispDamageEffect(turns);
        if (preAttackerHP != turnLast.attackerRestHp)
          attack.unitParts_.dispHpNumber(preAttackerHP, turnLast.attackerRestHp);
      }
      else
      {
        defense.unitParts_.SetRun(false);
        if (preAttackerHP != turnLast.attackerRestHp)
          attack.unitParts_.dispHpNumber(preAttackerHP, turnLast.attackerRestHp);
        attack.unitParts_.DispDamageEffect(turns);
        if (preDefenseHP != turnLast.defenderRestHp)
          defense.unitParts_.dispHpNumber(preDefenseHP, turnLast.defenderRestHp);
      }
      defense.unitParts_.setHpGauge(preDefenseHP, turnLast.defenderRestHp);
      attack.unitParts_.setHpGauge(preAttackerHP, turnLast.attackerRestHp);
      foreach (BL.DuelTurn turn in turns)
      {
        Dictionary<BL.Unit, List<BattleskillAilmentEffect>> dictionary = new Dictionary<BL.Unit, List<BattleskillAilmentEffect>>();
        foreach (var data in ((IEnumerable<int>) turn.investSkillIds).Select((skillId, index) => new
        {
          skillId = skillId,
          index = index
        }))
        {
          BattleskillSkill battleskillSkill = MasterData.BattleskillSkill[data.skillId];
          if (battleskillSkill.skill_type == BattleskillSkillType.ailment)
          {
            BL.Unit originalUnit = turn.investUnit[data.index].originalUnit;
            if (!dictionary.ContainsKey(originalUnit))
              dictionary[originalUnit] = new List<BattleskillAilmentEffect>();
            dictionary[originalUnit].Add(battleskillSkill.ailment_effect);
          }
        }
        foreach (BL.Unit key in dictionary.Keys)
        {
          if (key.isView)
            this.environment.unitResource[key].unitParts_.DispAilmentEffect(dictionary[key].ToArray());
        }
      }
    }));
    btm.setEnableWait(1.5f);
    btm.setScheduleAction((Action) (() =>
    {
      attack.unitParts_.SetRun(false);
      defense.unitParts_.SetRun(false);
    }));
  }

  public void startDuel(DuelResult duelResult, DuelEnvironment duelEnv = null, bool isStack = false)
  {
    List<Action> afterDuelSchedules = new List<Action>();
    List<Action> sceneEndSchedules = new List<Action>();
    BattleTimeManager btm = this.getManager<BattleTimeManager>();
    bool flag1 = this.noDuelScene || duelResult.defense.isFacility;
    this.mIsBattleEnable = false;
    List<BL.Story> storys = (List<BL.Story>) null;
    BL.Stage stage = (BL.Stage) null;
    if (this.environment != null)
    {
      this.environment.core.battleLogger.Duel(duelResult);
      this.environment.core.setSomeAction();
      BL.Unit attack = duelResult.attack;
      BL.Unit defense = duelResult.defense;
      if (PerformanceConfig.GetInstance().IsNotUseDeepCopy)
      {
        duelResult.attack = attack.Clone();
        duelResult.defense = defense.Clone();
      }
      else
      {
        duelResult.attack = CopyUtil.DeepCopy<BL.Unit>(attack);
        duelResult.defense = CopyUtil.DeepCopy<BL.Unit>(defense);
      }
      storys = this.environment.core.getDuelStorys(duelResult.moveUnit, duelResult.moveUnit == attack ? duelResult.defense : duelResult.attack);
      if (flag1 && storys.Any<BL.Story>())
      {
        bool flag2 = duelResult.defense.isFacility || flag1 && !storys.Any<BL.Story>((Func<BL.Story, bool>) (story => story.type == BL.StoryType.duel_start));
        bool flag3 = duelResult.isPlayerAttack && duelResult.isDieAttack || !duelResult.isPlayerAttack && duelResult.isDieDefense;
        flag1 = duelResult.defense.isFacility || flag2 && !(storys.Any<BL.Story>((Func<BL.Story, bool>) (story => story.type == BL.StoryType.duel_unit_dead)) & flag3);
      }
      if (attack.playerUnit.equip_gear_ids != null)
      {
        for (int index = 0; index < attack.playerUnit.equip_gear_ids.Length; ++index)
          duelResult.attack.playerUnit.equip_gear_ids[index] = attack.playerUnit.equip_gear_ids[index];
      }
      if (defense.playerUnit.equip_gear_ids != null)
      {
        for (int index = 0; index < defense.playerUnit.equip_gear_ids.Length; ++index)
          duelResult.defense.playerUnit.equip_gear_ids[index] = defense.playerUnit.equip_gear_ids[index];
      }
      BL.Unit[] effectModeUnits = this.environment.core.unitPositions.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => !x.unit.isDead && x.unit.isEnable)).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToArray<BL.Unit>();
      foreach (BL.Unit key in effectModeUnits)
      {
        if (key.isView)
          this.environment.unitResource[key].unitParts_.SetEffectMode(true, true);
      }
      attack.hp -= duelResult.attackDamage;
      defense.hp -= duelResult.defenseDamage;
      int duelAttackHp = attack.hp;
      int duelDefenseHp = defense.hp;
      if (this.isPvnpc)
        duelResult.disableDuelSkillEffects = this.pvnpcManager.checkDuelDeadUnitAndFinish(attack, defense);
      if (this.isGvg)
        duelResult.disableDuelSkillEffects = this.gvgManager.checkDuelDeadUnitAndFinish(attack, defense);
      int afterApplyDuelSkillEffectsHp = duelResult.moveUnit.hp;
      bool againUnitBecomeCharm = false;
      btm.setScheduleAction((Action) (() =>
      {
        if (againUnitBecomeCharm)
        {
          this.environment.core.getUnitPosition(duelResult.moveUnit).completeActionUnit(this.environment.core, true);
        }
        else
        {
          if (afterApplyDuelSkillEffectsHp <= 0)
            return;
          bool flag4 = duelResult.moveUnit != attack.originalUnit;
          this.environment.core.getUnitPosition(duelResult.moveUnit).actionActionUnit(this.environment.core, attack: !flag4 ? attack : defense, defense: !flag4 ? defense : attack, defenseHp: !flag4 ? duelDefenseHp : duelAttackHp);
        }
      }));
      BL.UnitPosition attackUp = this.environment.core.getUnitPosition(attack);
      int attackRow = attackUp.row;
      int attackColumn = attackUp.column;
      BL.UnitPosition defenseUp = this.environment.core.getUnitPosition(defense);
      int defenseRow = defenseUp.row;
      int defenseColumn = defenseUp.column;
      if (flag1)
      {
        if (attack.isView && defense.isView)
          afterDuelSchedules.Add((Action) (() =>
          {
            this.environment.core.setCurrentUnitWith((BL.Unit) null, (Action<BL.UnitPosition>) (_ => { }));
            BE.UnitResource defense1 = this.environment.unitResource[defense];
            BE.UnitResource attack1 = this.environment.unitResource[attack];
            int preAttackerHP = duelResult.attack.hp;
            int preDefenseHP = duelResult.defense.hp;
            Battle01SelectNode.MaskContinuer mc = this.selectNode.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
            btm.setScheduleAction((Action) (() =>
            {
              float? lookDirection = this.environment.core.getLookDirection(defenseRow, defenseColumn, attackRow, attackColumn, defense.isFacility);
              if (lookDirection.HasValue)
                defenseUp.direction = lookDirection.Value;
              lookDirection = this.environment.core.getLookDirection(attackRow, attackColumn, defenseRow, defenseColumn, attack.isFacility);
              if (!lookDirection.HasValue)
                return;
              attackUp.direction = lookDirection.Value;
            }));
            btm.setEnableWait(0.5f);
            List<List<BL.DuelTurn>> duelTurnListList = new List<List<BL.DuelTurn>>();
            int num = ((IEnumerable<BL.DuelTurn>) duelResult.turns).Count<BL.DuelTurn>();
            int attackCount;
            for (int index1 = 0; index1 < num; index1 += attackCount)
            {
              attackCount = duelResult.turns[index1].attackCount;
              List<BL.DuelTurn> duelTurnList = new List<BL.DuelTurn>();
              for (int index2 = 0; index2 < attackCount; ++index2)
              {
                if (num > index1 + index2)
                  duelTurnList.Add(duelResult.turns[index1 + index2]);
              }
              duelTurnListList.Add(duelTurnList);
            }
            foreach (List<BL.DuelTurn> duelTurnList in duelTurnListList)
            {
              BL.DuelTurn duelTurn = duelTurnList.LastOrDefault<BL.DuelTurn>();
              this.setBattleEffectSchedule(btm, duelTurnList, attack1, preAttackerHP, defense1, preDefenseHP);
              preAttackerHP = duelTurn.attackerRestHp;
              preDefenseHP = duelTurn.defenderRestHp;
            }
            this.selectNode.setMaskActive(false, mc);
          }));
      }
      else
      {
        int attackHp = attack.hp;
        int defenseHp = defense.hp;
        sceneEndSchedules.Add((Action) (() =>
        {
          if (attack.isView)
            this.environment.unitResource[attack].unitParts_.setHpGauge(attackHp);
          if (!defense.isView)
            return;
          this.environment.unitResource[defense].unitParts_.setHpGauge(defenseHp);
        }));
      }
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> lifeAbsorbSkillTarget = new Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>>();
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> curseReflectionSkillTarget = new Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>>();
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, Tuple<List<BL.Unit>, float>> penetrateSkillTarget = new Dictionary<Tuple<BL.Unit, BattleskillSkill>, Tuple<List<BL.Unit>, float>>();
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> rangeAttackSkillTarget = new Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>>();
      Dictionary<BL.Unit, Dictionary<BattleskillSkill, List<BL.Unit>>> damageShareSkillTarget = new Dictionary<BL.Unit, Dictionary<BattleskillSkill, List<BL.Unit>>>();
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> stealSkillTarget = new Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>>();
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>> removeSkillEffectSkillTarget = new Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Unit>>();
      Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Panel>> afterDuelInvestLandTagSkillTarget = new Dictionary<Tuple<BL.Unit, BattleskillSkill>, List<BL.Panel>>();
      List<Tuple<BL.Unit, BattleskillSkill>> afterDuelInvestLandTagKeys = new List<Tuple<BL.Unit, BattleskillSkill>>();
      Dictionary<BL.Unit, Tuple<int, int>> effectTargetsAddHp = new Dictionary<BL.Unit, Tuple<int, int>>();
      Dictionary<BL.Unit, Tuple<int, int>> effectTargetsSubHp = new Dictionary<BL.Unit, Tuple<int, int>>();
      Dictionary<BL.Unit, Tuple<int, int>> effectTargetsPenetrateSubHp = new Dictionary<BL.Unit, Tuple<int, int>>();
      Dictionary<BL.Unit, Tuple<int, int>> effectTargetsRangeAttackSubHp = new Dictionary<BL.Unit, Tuple<int, int>>();
      Dictionary<BL.Unit, Tuple<int, int>> effectTargetsDamageShareSubHp = new Dictionary<BL.Unit, Tuple<int, int>>();
      Dictionary<Tuple<BattleskillSkill, BL.Unit>, HashSet<BL.Unit>> effectTargetsSnake = new Dictionary<Tuple<BattleskillSkill, BL.Unit>, HashSet<BL.Unit>>();
      Dictionary<Tuple<BattleskillSkill, BL.Unit>, float?> effectTargetsSnakeRotate = new Dictionary<Tuple<BattleskillSkill, BL.Unit>, float?>();
      Dictionary<BL.Unit, Tuple<int, int>> dispHpUnitsSnake = new Dictionary<BL.Unit, Tuple<int, int>>();
      Dictionary<BL.Unit, Tuple<int, int>> swapHealDamageHp = new Dictionary<BL.Unit, Tuple<int, int>>();
      Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>> removeSkillEffects = new Dictionary<BL.ISkillEffectListUnit, List<BL.SkillEffect>>();
      BattleFuncs.applyDuelResultEffects(duelResult, (BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense, this.environment.core, (Action<BL.Unit, Tuple<int, int>>) ((unit, hp) => swapHealDamageHp[unit] = hp));
      BattleFuncs.applyDuelSkillEffects(duelResult, (BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense, this.environment.core, removeSkillEffects, (Action<BL.ISkillEffectListUnit, int>) ((target, prevHp) => effectTargetsAddHp[target as BL.Unit] = new Tuple<int, int>(prevHp, target.hp)), (Action<BL.ISkillEffectListUnit, int>) ((target, prevHp) =>
      {
        BL.Unit key = target as BL.Unit;
        if (!effectTargetsSubHp.ContainsKey(key))
          effectTargetsSubHp[key] = new Tuple<int, int>(prevHp, target.hp);
        else
          effectTargetsSubHp[key] = new Tuple<int, int>(effectTargetsSubHp[key].Item1, target.hp);
      }), (Action<BL.ISkillEffectListUnit, int>) ((target, prevHp) => effectTargetsPenetrateSubHp[target as BL.Unit] = new Tuple<int, int>(prevHp, target.hp)), (Action<BL.ISkillEffectListUnit, int>) ((target, prevHp) =>
      {
        BL.Unit key = target as BL.Unit;
        if (!effectTargetsRangeAttackSubHp.ContainsKey(key))
          effectTargetsRangeAttackSubHp[key] = new Tuple<int, int>(prevHp, target.hp);
        else
          effectTargetsRangeAttackSubHp[key] = new Tuple<int, int>(effectTargetsRangeAttackSubHp[key].Item1, target.hp);
      }), (Action<BL.ISkillEffectListUnit, int>) ((target, prevHp) =>
      {
        BL.Unit key = target as BL.Unit;
        if (!effectTargetsDamageShareSubHp.ContainsKey(key))
          effectTargetsDamageShareSubHp[key] = new Tuple<int, int>(prevHp, target.hp);
        else
          effectTargetsDamageShareSubHp[key] = new Tuple<int, int>(effectTargetsDamageShareSubHp[key].Item1, target.hp);
      }), (Action<BattleskillSkill, BL.Unit, Dictionary<BL.Unit, Tuple<int, int>>, float?>) ((skill, unit, targets, rotate) =>
      {
        Tuple<BattleskillSkill, BL.Unit> key1 = Tuple.Create<BattleskillSkill, BL.Unit>(skill, unit);
        if (!effectTargetsSnake.ContainsKey(key1))
        {
          effectTargetsSnake[key1] = new HashSet<BL.Unit>();
          effectTargetsSnakeRotate[key1] = rotate;
        }
        foreach (BL.Unit key2 in targets.Keys)
        {
          effectTargetsSnake[key1].Add(key2);
          dispHpUnitsSnake[key2] = dispHpUnitsSnake.ContainsKey(key2) ? new Tuple<int, int>(dispHpUnitsSnake[key2].Item1, targets[key2].Item2) : new Tuple<int, int>(targets[key2].Item1, targets[key2].Item2);
        }
      }), lifeAbsorbSkillTarget, curseReflectionSkillTarget, penetrateSkillTarget, rangeAttackSkillTarget, damageShareSkillTarget, stealSkillTarget, removeSkillEffectSkillTarget, afterDuelInvestLandTagSkillTarget, afterDuelInvestLandTagKeys);
      BattleFuncs.applyDuelResultEffectsLate(duelResult, (BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense, this.environment.core, removeSkillEffects);
      afterApplyDuelSkillEffectsHp = duelResult.moveUnit.hp;
      if (this.useGameEngine)
      {
        this.gameEngine.applyDeadUnit(attack, defense);
        List<BL.Unit> source = new List<BL.Unit>();
        foreach (List<BL.Unit> collection in lifeAbsorbSkillTarget.Values)
          source.AddRange((IEnumerable<BL.Unit>) collection);
        foreach (List<BL.Unit> collection in curseReflectionSkillTarget.Values)
          source.AddRange((IEnumerable<BL.Unit>) collection);
        foreach (Tuple<List<BL.Unit>, float> tuple in penetrateSkillTarget.Values)
          source.AddRange((IEnumerable<BL.Unit>) tuple.Item1);
        foreach (List<BL.Unit> collection in rangeAttackSkillTarget.Values)
          source.AddRange((IEnumerable<BL.Unit>) collection);
        foreach (BL.Unit key in damageShareSkillTarget.Keys)
        {
          source.Add(key);
          foreach (List<BL.Unit> collection in damageShareSkillTarget[key].Values)
            source.AddRange((IEnumerable<BL.Unit>) collection);
        }
        foreach (BL.Unit key in dispHpUnitsSnake.Keys)
          source.Add(key);
        foreach (BL.Unit key in swapHealDamageHp.Keys)
          source.Add(key);
        foreach (BL.Unit attack2 in source.Distinct<BL.Unit>())
        {
          if (!attack2.Equals(attack) && !attack2.Equals(defense))
            this.gameEngine.applyDeadUnit(attack2, (BL.Unit) null);
        }
      }
      if (swapHealDamageHp.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          btm.setScheduleAction((Action) (() =>
          {
            foreach (BL.Unit key in swapHealDamageHp.Keys)
            {
              BE.UnitResource unitResource = this.environment.unitResource[key];
              unitResource.unitParts_.dispHpNumber(swapHealDamageHp[key].Item1, swapHealDamageHp[key].Item2);
              unitResource.unitParts_.setHpGauge(swapHealDamageHp[key].Item1, swapHealDamageHp[key].Item2);
            }
          }));
          btm.setEnableWait(1f);
        }));
      if (removeSkillEffectSkillTarget.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          List<BattleskillFieldEffect> battleskillFieldEffectList = new List<BattleskillFieldEffect>();
          List<List<BL.Unit>> unitListList1 = new List<List<BL.Unit>>();
          List<GameObject> gameObjectList1 = new List<GameObject>();
          List<GameObject> gameObjectList2 = new List<GameObject>();
          List<List<BL.Unit>> unitListList2 = new List<List<BL.Unit>>();
          foreach (Tuple<BL.Unit, BattleskillSkill> key in removeSkillEffectSkillTarget.Keys)
          {
            if (removeSkillEffectSkillTarget[key].Count >= 1)
            {
              BattleskillSkill battleskillSkill = key.Item2;
              BE.SkillResource skillResource = this.environment.skillResource[battleskillSkill.passive_effect.ID];
              battleskillFieldEffectList.Add(battleskillSkill.passive_effect);
              unitListList1.Add(removeSkillEffectSkillTarget[key]);
              gameObjectList1.Add(skillResource.invokedEffectPrefab);
              gameObjectList2.Add(skillResource.targetEffectPrefab);
              unitListList2.Add(new List<BL.Unit>()
              {
                key.Item1
              });
            }
          }
          if (battleskillFieldEffectList.Count >= 1)
            this.battleEffects.skillFieldEffectMultiStartCore(battleskillFieldEffectList.ToArray(), unitListList1.ToArray(), gameObjectList1.ToArray(), gameObjectList2.ToArray(), unitListList2.ToArray());
          btm.setEnableWait(1.5f);
        }));
      if (afterDuelInvestLandTagKeys.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          List<BattleskillFieldEffect> source = new List<BattleskillFieldEffect>();
          List<List<BL.Unit>> unitListList3 = new List<List<BL.Unit>>();
          List<GameObject> gameObjectList3 = new List<GameObject>();
          List<GameObject> gameObjectList4 = new List<GameObject>();
          List<List<BL.Unit>> unitListList4 = new List<List<BL.Unit>>();
          List<List<BL.Panel>> panelListList = new List<List<BL.Panel>>();
          List<BL.Panel> second = new List<BL.Panel>();
          afterDuelInvestLandTagKeys.Reverse();
          foreach (Tuple<BL.Unit, BattleskillSkill> key in afterDuelInvestLandTagKeys)
          {
            if (afterDuelInvestLandTagSkillTarget[key].Count >= 1)
            {
              BattleskillSkill battleskillSkill = key.Item2;
              BE.SkillResource skillResource = this.environment.skillResource[battleskillSkill.passive_effect.ID];
              List<BL.Panel> list = afterDuelInvestLandTagSkillTarget[key].Except<BL.Panel>((IEnumerable<BL.Panel>) second).ToList<BL.Panel>();
              second.AddRange((IEnumerable<BL.Panel>) list);
              source.Add(battleskillSkill.passive_effect);
              unitListList3.Add((List<BL.Unit>) null);
              gameObjectList3.Add(skillResource.invokedEffectPrefab);
              gameObjectList4.Add(skillResource.targetEffectPrefab);
              unitListList4.Add(new List<BL.Unit>()
              {
                key.Item1
              });
              panelListList.Add(list);
            }
          }
          if (source.Count<BattleskillFieldEffect>() >= 1)
            this.battleEffects.skillFieldEffectMultiStartCore(source.ToArray(), unitListList3.ToArray(), gameObjectList3.ToArray(), gameObjectList4.ToArray(), unitListList4.ToArray(), aryTargetPanels: panelListList.ToArray());
          btm.setEnableWait(1.5f);
        }));
      if (attackUp.row != attackRow || attackUp.column != attackColumn || defenseUp.row != defenseRow || defenseUp.column != defenseColumn)
        afterDuelSchedules.Add((Action) (() =>
        {
          btm.setScheduleAction((Action) (() =>
          {
            foreach (BL.Unit unit in effectModeUnits)
            {
              if (unit.isView)
              {
                this.environment.unitResource[unit].unitParts_.SetEffectMode(true);
                this.environment.core.getUnitPosition(unit).commit();
              }
            }
          }));
          btm.setEnableWait(1.5f);
        }));
      if (stealSkillTarget.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          List<BattleskillFieldEffect> source = new List<BattleskillFieldEffect>();
          List<List<BL.Unit>> unitListList5 = new List<List<BL.Unit>>();
          List<GameObject> gameObjectList5 = new List<GameObject>();
          List<GameObject> gameObjectList6 = new List<GameObject>();
          List<List<BL.Unit>> unitListList6 = new List<List<BL.Unit>>();
          foreach (Tuple<BL.Unit, BattleskillSkill> key in stealSkillTarget.Keys)
          {
            if (stealSkillTarget[key].Count<BL.Unit>() >= 1)
            {
              BattleskillSkill battleskillSkill = key.Item2;
              BE.SkillResource skillResource = this.environment.skillResource[battleskillSkill.passive_effect.ID];
              source.Add(battleskillSkill.passive_effect);
              unitListList5.Add(stealSkillTarget[key]);
              gameObjectList5.Add(skillResource.invokedEffectPrefab);
              gameObjectList6.Add(skillResource.targetEffectPrefab);
              unitListList6.Add(new List<BL.Unit>()
              {
                key.Item1
              });
            }
          }
          if (source.Count<BattleskillFieldEffect>() >= 1)
            this.battleEffects.skillFieldEffectMultiStartCore(source.ToArray(), unitListList5.ToArray(), gameObjectList5.ToArray(), gameObjectList6.ToArray(), unitListList6.ToArray());
          btm.setEnableWait(1.5f);
        }));
      if (effectTargetsDamageShareSubHp.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          List<BattleskillFieldEffect> source1 = new List<BattleskillFieldEffect>();
          List<List<BL.Unit>> unitListList7 = new List<List<BL.Unit>>();
          List<GameObject> gameObjectList7 = new List<GameObject>();
          List<GameObject> gameObjectList8 = new List<GameObject>();
          List<List<BL.Unit>> unitListList8 = new List<List<BL.Unit>>();
          foreach (BL.Unit key3 in damageShareSkillTarget.Keys)
          {
            BL.Unit unit = key3;
            foreach (BattleskillSkill key4 in damageShareSkillTarget[key3].Keys)
            {
              BE.SkillResource skillResource = this.environment.skillResource[key4.passive_effect.ID];
              List<BL.Unit> source2 = new List<BL.Unit>()
              {
                unit
              };
              source2.AddRange((IEnumerable<BL.Unit>) damageShareSkillTarget[key3][key4]);
              source1.Add(key4.passive_effect);
              unitListList7.Add(source2.Distinct<BL.Unit>().ToList<BL.Unit>());
              gameObjectList7.Add((GameObject) null);
              gameObjectList8.Add(skillResource.targetEffectPrefab);
              unitListList8.Add((List<BL.Unit>) null);
            }
          }
          if (source1.Count<BattleskillFieldEffect>() >= 1)
            this.battleEffects.skillFieldEffectMultiStartCore(source1.ToArray(), unitListList7.ToArray(), gameObjectList7.ToArray(), gameObjectList8.ToArray(), unitListList8.ToArray());
          btm.setScheduleAction((Action) (() =>
          {
            foreach (BL.Unit key in effectTargetsDamageShareSubHp.Keys)
            {
              BE.UnitResource unitResource = this.environment.unitResource[key];
              unitResource.unitParts_.dispHpNumber(effectTargetsDamageShareSubHp[key].Item1, effectTargetsDamageShareSubHp[key].Item2);
              unitResource.unitParts_.setHpGauge(effectTargetsDamageShareSubHp[key].Item1, effectTargetsDamageShareSubHp[key].Item2);
            }
          }));
          btm.setEnableWait(1.5f);
        }));
      if (effectTargetsAddHp.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          if (!this.duelHpSkillEffects(lifeAbsorbSkillTarget, effectTargetsAddHp, attack, defense))
            return;
          btm.setEnableWait(0.5f);
        }));
      if (effectTargetsPenetrateSubHp.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          List<BattleskillFieldEffect> source = new List<BattleskillFieldEffect>();
          List<List<BL.Unit>> unitListList9 = new List<List<BL.Unit>>();
          List<GameObject> gameObjectList9 = new List<GameObject>();
          List<GameObject> gameObjectList10 = new List<GameObject>();
          List<List<BL.Unit>> unitListList10 = new List<List<BL.Unit>>();
          List<List<Quaternion?>> nullableListList = new List<List<Quaternion?>>();
          foreach (Tuple<BL.Unit, BattleskillSkill> key in penetrateSkillTarget.Keys)
          {
            if (penetrateSkillTarget[key].Item1.Count<BL.Unit>() >= 1)
            {
              BattleskillSkill battleskillSkill = key.Item2;
              BE.SkillResource skillResource = this.environment.skillResource[battleskillSkill.passive_effect.ID];
              source.Add(battleskillSkill.passive_effect);
              unitListList9.Add(penetrateSkillTarget[key].Item1);
              gameObjectList9.Add(skillResource.invokedEffectPrefab);
              gameObjectList10.Add(skillResource.targetEffectPrefab);
              unitListList10.Add(new List<BL.Unit>()
              {
                this.environment.core.getForceID(key.Item1) == this.environment.core.getForceID(attack) ? defense : attack
              });
              nullableListList.Add(new List<Quaternion?>()
              {
                new Quaternion?(Quaternion.Euler(0.0f, penetrateSkillTarget[key].Item2, 0.0f))
              });
            }
          }
          if (source.Count<BattleskillFieldEffect>() >= 1)
            this.battleEffects.skillFieldEffectMultiStartCore(source.ToArray(), unitListList9.ToArray(), gameObjectList9.ToArray(), gameObjectList10.ToArray(), unitListList10.ToArray(), nullableListList.ToArray());
          btm.setScheduleAction((Action) (() =>
          {
            foreach (BL.Unit key in effectTargetsPenetrateSubHp.Keys)
            {
              if (key.isView)
              {
                BE.UnitResource unitResource = this.environment.unitResource[key];
                unitResource.unitParts_.dispHpNumber(effectTargetsPenetrateSubHp[key].Item1, effectTargetsPenetrateSubHp[key].Item2);
                unitResource.unitParts_.setHpGauge(effectTargetsPenetrateSubHp[key].Item1, effectTargetsPenetrateSubHp[key].Item2);
              }
            }
          }));
          btm.setEnableWait(1.5f);
        }));
      if (effectTargetsRangeAttackSubHp.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          List<BattleskillFieldEffect> source3 = new List<BattleskillFieldEffect>();
          List<List<BL.Unit>> unitListList11 = new List<List<BL.Unit>>();
          List<GameObject> gameObjectList11 = new List<GameObject>();
          List<GameObject> gameObjectList12 = new List<GameObject>();
          List<List<BL.Unit>> unitListList12 = new List<List<BL.Unit>>();
          foreach (Tuple<BL.Unit, BattleskillSkill> key in rangeAttackSkillTarget.Keys)
          {
            if (rangeAttackSkillTarget[key].Count<BL.Unit>() >= 1)
            {
              BattleskillSkill battleskillSkill = key.Item2;
              bool flag5 = key.Item1 == attack;
              IEnumerable<BattleskillEffect> battleskillEffects = ((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.range_attack));
              HashSet<BL.Unit> source4 = new HashSet<BL.Unit>();
              foreach (BattleskillEffect battleskillEffect in battleskillEffects)
              {
                BL.Unit unit = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) == 0 ? (flag5 ? attack : defense) : (flag5 ? defense : attack);
                source4.Add(unit);
              }
              BE.SkillResource skillResource = this.environment.skillResource[battleskillSkill.passive_effect.ID];
              source3.Add(battleskillSkill.passive_effect);
              unitListList11.Add(rangeAttackSkillTarget[key]);
              gameObjectList11.Add(skillResource.invokedEffectPrefab);
              gameObjectList12.Add(skillResource.targetEffectPrefab);
              unitListList12.Add(source4.ToList<BL.Unit>());
            }
          }
          if (source3.Count<BattleskillFieldEffect>() >= 1)
            this.battleEffects.skillFieldEffectMultiStartCore(source3.ToArray(), unitListList11.ToArray(), gameObjectList11.ToArray(), gameObjectList12.ToArray(), unitListList12.ToArray());
          btm.setScheduleAction((Action) (() =>
          {
            foreach (BL.Unit key in effectTargetsRangeAttackSubHp.Keys)
            {
              if (key.isView)
              {
                BE.UnitResource unitResource = this.environment.unitResource[key];
                unitResource.unitParts_.dispHpNumber(effectTargetsRangeAttackSubHp[key].Item1, effectTargetsRangeAttackSubHp[key].Item2);
                unitResource.unitParts_.setHpGauge(effectTargetsRangeAttackSubHp[key].Item1, effectTargetsRangeAttackSubHp[key].Item2);
              }
            }
          }));
          btm.setEnableWait(1.5f);
        }));
      if (effectTargetsSubHp.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          if (!this.duelHpSkillEffects(curseReflectionSkillTarget, effectTargetsSubHp, attack, defense))
            return;
          btm.setEnableWait(1.5f);
        }));
      if (effectTargetsSnake.Count >= 1)
        afterDuelSchedules.Add((Action) (() =>
        {
          List<BattleskillFieldEffect> source5 = new List<BattleskillFieldEffect>();
          List<List<BL.Unit>> unitListList13 = new List<List<BL.Unit>>();
          List<GameObject> gameObjectList13 = new List<GameObject>();
          List<GameObject> gameObjectList14 = new List<GameObject>();
          List<List<BL.Unit>> unitListList14 = new List<List<BL.Unit>>();
          List<List<Quaternion?>> nullableListList1 = new List<List<Quaternion?>>();
          foreach (Tuple<BattleskillSkill, BL.Unit> key in effectTargetsSnake.Keys)
          {
            BattleskillSkill battleskillSkill = key.Item1;
            BL.Unit unit1 = key.Item2;
            HashSet<BL.Unit> source6 = effectTargetsSnake[key];
            if (source6.Count >= 1)
            {
              BattleskillFieldEffect battleskillFieldEffect = battleskillSkill.skill_type == BattleskillSkillType.duel ? battleskillSkill.field_effect : battleskillSkill.passive_effect;
              bool flag6 = unit1 == attack;
              IEnumerable<BattleskillEffect> source7 = ((IEnumerable<BattleskillEffect>) battleskillSkill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.snake_venom || x.EffectLogic.Enum == BattleskillEffectLogicEnum.snake_venom_damage));
              HashSet<BL.Unit> source8 = (HashSet<BL.Unit>) null;
              if (source7 != null && source7.Count<BattleskillEffect>() > 0)
              {
                source8 = new HashSet<BL.Unit>();
                foreach (BattleskillEffect battleskillEffect in source7)
                {
                  BL.Unit unit2 = battleskillEffect.GetInt(BattleskillEffectLogicArgumentEnum.is_range_from_enemy) == 0 ? (flag6 ? attack : defense) : (flag6 ? defense : attack);
                  source8.Add(unit2);
                }
              }
              BE.SkillResource skillResource = this.environment.skillResource[battleskillFieldEffect.ID];
              source5.Add(battleskillFieldEffect);
              unitListList13.Add(source6.ToList<BL.Unit>());
              gameObjectList13.Add(skillResource.invokedEffectPrefab);
              gameObjectList14.Add(skillResource.targetEffectPrefab);
              unitListList14.Add(source8.ToList<BL.Unit>());
              List<List<Quaternion?>> nullableListList2 = nullableListList1;
              List<Quaternion?> nullableList;
              if (!effectTargetsSnakeRotate[key].HasValue)
              {
                nullableList = (List<Quaternion?>) null;
              }
              else
              {
                nullableList = new List<Quaternion?>();
                nullableList.Add(new Quaternion?(Quaternion.Euler(0.0f, effectTargetsSnakeRotate[key].Value, 0.0f)));
              }
              nullableListList2.Add(nullableList);
            }
          }
          if (source5.Count<BattleskillFieldEffect>() >= 1)
            this.battleEffects.skillFieldEffectMultiStartCore(source5.ToArray(), unitListList13.ToArray(), gameObjectList13.ToArray(), gameObjectList14.ToArray(), unitListList14.ToArray(), nullableListList1.ToArray());
          if (dispHpUnitsSnake.Count<KeyValuePair<BL.Unit, Tuple<int, int>>>() < 1)
            return;
          btm.setScheduleAction((Action) (() =>
          {
            foreach (BL.Unit key in dispHpUnitsSnake.Keys)
            {
              if (key.isView)
              {
                BE.UnitResource unitResource = this.environment.unitResource[key];
                unitResource.unitParts_.dispHpNumber(dispHpUnitsSnake[key].Item1, dispHpUnitsSnake[key].Item2);
                unitResource.unitParts_.setHpGauge(dispHpUnitsSnake[key].Item1, dispHpUnitsSnake[key].Item2);
              }
            }
          }));
          btm.setEnableWait(1.5f);
        }));
      if (duelResult.moveUnit.hp > 0)
      {
        BL.UnitPosition unitPosition = this.environment.core.getUnitPosition(duelResult.moveUnit);
        bool flag7 = duelResult.moveUnit != attack.originalUnit;
        if (BattleFuncs.getNextCompleteActionCount((BL.ISkillEffectListUnit) duelResult.moveUnit, unitPosition, !flag7 ? (BL.ISkillEffectListUnit) attack : (BL.ISkillEffectListUnit) defense, !flag7 ? (BL.ISkillEffectListUnit) defense : (BL.ISkillEffectListUnit) attack, !flag7 ? duelDefenseHp : duelAttackHp, true).Item1 >= 1)
        {
          BL.SkillEffect[] array = duelResult.moveUnit.skillEffects.All().ToArray();
          List<List<BL.ExecuteSkillEffectResult>> esrll;
          List<BL.UnitPosition> fupl = this.environment.core.completedPositionExecuteSkillEffects(unitPosition, out esrll);
          if (!duelResult.moveUnitIsCharm && unitPosition.unit.IsCharm)
            againUnitBecomeCharm = true;
          foreach (BL.SkillEffect skillEffect in duelResult.moveUnit.skillEffects.All().Except<BL.SkillEffect>((IEnumerable<BL.SkillEffect>) array))
            skillEffect.moveDistance = new int?();
          afterDuelSchedules.Add((Action) (() =>
          {
            Battle01SelectNode.MaskContinuer mc = this.selectNode.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
            BattleStateController controller = this.getController<BattleStateController>();
            int num = 0;
            bool flag8 = false;
            BattleStateController.ApplyFacilitySkillDeads facilitySkillDeads = new BattleStateController.ApplyFacilitySkillDeads(controller);
            foreach (BL.UnitPosition up in fupl)
            {
              foreach (BL.ExecuteSkillEffectResult es in esrll[num++])
              {
                if (es.targets.Count > 0 || es.targetPanels.Count > 0)
                {
                  facilitySkillDeads.Add(es);
                  controller.doExecuteFacilitySkillEffects(up, es);
                  flag8 = true;
                }
              }
            }
            facilitySkillDeads.Execute();
            if (flag8)
              btm.setEnableWait(1.5f);
            this.selectNode.setMaskActive(false, mc);
          }));
        }
      }
      afterDuelSchedules.Add((Action) (() => btm.setScheduleAction((Action) (() =>
      {
        foreach (BL.Unit key in effectModeUnits)
        {
          if (key.isView)
            this.environment.unitResource[key].unitParts_.SetEffectMode(false);
        }
      }))));
      attack.commit();
      defense.commit();
      this.environment.core.updateUnitBattleStatus(duelResult, attack, defense);
      stage = this.environment.core.stage;
      btm.setScheduleAction((Action) (() =>
      {
        foreach (BL.Story story in storys)
          story.isRead = true;
        this.checkReinforcementForBattle(duelResult.moveUnit, duelResult.moveUnit == attack ? duelResult.defense : duelResult.attack);
        this.checkReinforceUnitForSmash();
        if (PerformanceConfig.GetInstance().IsNotAutoSaveBeforeDuel && (this._environment.core.isAutoBattle.value || !attack.isPlayerForce))
          return;
        this.saveEnvironment();
      }));
    }
    if (duelEnv == null)
    {
      if (storys == null)
        storys = new List<BL.Story>();
      if (stage == null)
        stage = new BL.Stage(1);
      duelEnv = new DuelEnvironment();
      duelEnv.storys = storys;
      duelEnv.stage = stage;
    }
    if (flag1)
    {
      if (this.topScene != Singleton<NGSceneManager>.GetInstance().sceneName)
        btm.backSceneWithReturnWait();
      else
        this.mIsBattleEnable = true;
    }
    else
      btm.changeSceneWithReturnWait(this.duelScene, (isStack ? 1 : 0) != 0, (Action) (() =>
      {
        NGBattle3DObjectManager manager = this.getManager<NGBattle3DObjectManager>();
        if (Object.op_Inequality((Object) manager, (Object) null))
          manager.setRootActive(false);
        Singleton<NGSoundManager>.GetInstance().crossFadeCurrentBGM(2f, 1f);
      }), (Action) null, (Action) (() =>
      {
        foreach (Action action in sceneEndSchedules)
          action();
      }), (object) duelResult, (object) duelEnv);
    if (afterDuelSchedules.Count >= 1)
    {
      this.isAfterDuelEffectWaiting = true;
      btm.setScheduleAction((Action) (() =>
      {
        foreach (Action action in afterDuelSchedules)
          action();
        this.isAfterDuelEffectWaiting = false;
      }));
    }
    this.saveAlreadyReadStory((IEnumerable<BL.Story>) storys);
  }

  public void checkReinforcementForBattle(BL.Unit attack, BL.Unit defence)
  {
    List<BL.UnitPosition> list = this.environment.core.unitPositions.value.Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.playerUnit.reinforcement != null && !x.unit.isDead && !x.unit.isEnable && x.unit.playerUnit.reinforcement.isSpawnForBattle(attack, defence))).ToList<BL.UnitPosition>();
    if (list.Count <= 0)
      return;
    foreach (BL.UnitPosition unitPosition in list)
    {
      if (!this.environment.core.spawnUnits.value.Contains(unitPosition))
      {
        this.environment.core.spawnUnits.value.Add(unitPosition);
        this.environment.core.spawnUnits.commit();
      }
    }
  }

  public bool checkReinforceUnitForSmash()
  {
    List<BL.UnitPosition> source = new List<BL.UnitPosition>();
    foreach (BL.UnitPosition unitPosition in this.environment.core.unitPositions.value)
    {
      if (!unitPosition.unit.isDead && !unitPosition.unit.isEnable && this.environment.core.isReinforceUnitForSmash(unitPosition.unit.playerUnit))
        source.Add(unitPosition);
    }
    bool flag = false;
    if (source.Count<BL.UnitPosition>() > 0)
    {
      foreach (BL.UnitPosition unitPosition in source)
      {
        if (!this.environment.core.spawnUnits.value.Contains(unitPosition))
        {
          this.environment.core.spawnUnits.value.Add(unitPosition);
          this.environment.core.spawnUnits.commit();
          flag = true;
        }
      }
    }
    return flag;
  }

  public void startStory(BL.Story story)
  {
    this.saveAlreadyReadStory(story);
    this.getManager<BattleTimeManager>().changeSceneWithReturnWait(this.storyScene, true, (Action) (() =>
    {
      if (story.type != BL.StoryType.battle_start)
        Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
    }), (Action) (() =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (story.type != BL.StoryType.battle_win)
        return;
      BattleCameraFilter.DesotryBattleWin();
    }), (Action) null, (object) story.scriptId);
  }

  private void saveAlreadyReadStory(BL.Story story)
  {
    if (!this.isRaid)
      return;
    Persist.raidStoryAlreadyRead.Data.addReadStoryId(this.environment.core.battleInfo.stageId, story.scriptId);
    Persist.raidStoryAlreadyRead.Flush();
  }

  private void saveAlreadyReadStory(IEnumerable<BL.Story> storys)
  {
    if (!this.isRaid)
      return;
    foreach (BL.Story story in storys)
      Persist.raidStoryAlreadyRead.Data.addReadStoryId(this.environment.core.battleInfo.stageId, story.scriptId);
    Persist.raidStoryAlreadyRead.Flush();
  }

  public bool isBattleEnable
  {
    get => this.mIsBattleEnable && Singleton<NGSceneManager>.GetInstance().isSceneInitialized;
    set
    {
      if (value)
      {
        string sceneName = Singleton<NGSceneManager>.GetInstance().sceneName;
        if (sceneName == null || !(sceneName == this.topScene))
          return;
        this.mIsBattleEnable = value;
      }
      else
        this.mIsBattleEnable = value;
    }
  }

  public GameObject popupOpen(
    GameObject prefab,
    bool alert = false,
    EventDelegate ed = null,
    bool isCloned = false,
    bool nonBattleEnableControl = false,
    bool isUnmask = false,
    bool isViewBack = true,
    bool isNonSe = false,
    bool isAlertButtonNoneState = false)
  {
    PopupManager instance = Singleton<PopupManager>.GetInstance();
    if (!nonBattleEnableControl)
      this.isBattleEnable = false;
    if (!alert)
      return instance.open(prefab, isUnmask, isCloned: isCloned, isViewBack: isViewBack, isNonSe: isNonSe);
    if (ed == null)
      ed = new EventDelegate((MonoBehaviour) this, "doPopupDismiss");
    return instance.openAlert(prefab, isUnmask, ed: ed, isCloned: isCloned, isViewBack: isViewBack, isNonSe: isNonSe, isCreateButtonNoneState: isAlertButtonNoneState);
  }

  private IEnumerator doDismissWait()
  {
    PopupManager pm = Singleton<PopupManager>.GetInstance();
    while (pm.isOpen)
      yield return (object) null;
    this.isBattleEnable = true;
  }

  public void popupDismiss(bool nonBattleEnableControl = false, bool withoutAnim = false)
  {
    PopupManager instance = Singleton<PopupManager>.GetInstance();
    if (withoutAnim)
      instance.dismissWithoutAnim();
    else
      instance.dismiss();
    if (nonBattleEnableControl)
      return;
    this.StartCoroutine(this.doDismissWait());
  }

  public void popupCloseAll(bool nonBattleEnableControl = false)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    if (nonBattleEnableControl)
      return;
    this.StartCoroutine(this.doDismissWait());
  }

  public void doPopupDismiss() => this.popupDismiss();

  public Persist<BE> GetSaveData()
  {
    if (this.isEarth)
      return Persist.earthBattleEnvironment;
    return this.isGvg ? Persist.gvgBattleEnvironment : Persist.battleEnvironment;
  }

  public void deleteSavedEnvironment() => this.GetSaveData().Delete();

  public bool hasSavedEnvironment() => this.GetSaveData().Exists;

  public void saveEnvironment(bool forcibly = false)
  {
    if (!forcibly && this.mBattleInfo.pvp && !this.mBattleInfo.pvp_vs_npc)
      return;
    this.GetSaveData().Data = this.environment;
    foreach (BL.Unit unit in this.environment.core.playerUnits.value)
      unit.SaveEquipedGears();
    this.GetSaveData().Flush();
  }

  public void loadEnvironment()
  {
    this.environment = this.GetSaveData().Data;
    foreach (BL.Unit unit in this.environment.core.playerUnits.value)
      unit.LoadEquipedGears();
  }

  private bool checkForStoryOnly()
  {
    bool flag = false;
    switch (this.battleInfo.quest_type)
    {
      case CommonQuestType.Story:
        QuestStoryS questStoryS;
        if (MasterData.QuestStoryS.TryGetValue(this.battleInfo.quest_s_id, out questStoryS))
        {
          flag = questStoryS.story_only;
          break;
        }
        break;
      case CommonQuestType.Character:
        QuestCharacterS questCharacterS;
        if (MasterData.QuestCharacterS.TryGetValue(this.battleInfo.quest_s_id, out questCharacterS))
        {
          flag = questCharacterS.story_only;
          break;
        }
        break;
      case CommonQuestType.Extra:
        QuestExtraS questExtraS;
        if (MasterData.QuestExtraS.TryGetValue(this.battleInfo.quest_s_id, out questExtraS))
        {
          flag = questExtraS.story_only;
          break;
        }
        break;
      case CommonQuestType.Sea:
        QuestSeaS questSeaS;
        if (MasterData.QuestSeaS.TryGetValue(this.battleInfo.quest_s_id, out questSeaS))
        {
          flag = questSeaS.story_only;
          break;
        }
        break;
    }
    return flag;
  }

  private void startStoryOnlyScript()
  {
    int scriptId = 0;
    switch (this.battleInfo.quest_type)
    {
      case CommonQuestType.Story:
        StoryPlaybackStoryDetail playbackStoryDetail = ((IEnumerable<StoryPlaybackStoryDetail>) MasterData.StoryPlaybackStoryDetailList).Where<StoryPlaybackStoryDetail>((Func<StoryPlaybackStoryDetail, bool>) (x => x.quest_s_id_QuestStoryS == this.battleInfo.quest_s_id && x.timing == StoryPlaybackTiming.select_stage)).FirstOrDefault<StoryPlaybackStoryDetail>();
        if (playbackStoryDetail != null)
        {
          scriptId = playbackStoryDetail.script_id;
          break;
        }
        break;
      case CommonQuestType.Character:
        IEnumerable<StoryPlaybackCharacterDetail> source = ((IEnumerable<StoryPlaybackCharacterDetail>) MasterData.StoryPlaybackCharacterDetailList).Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.quest_QuestCharacterS == this.battleInfo.quest_s_id));
        if (source.Count<StoryPlaybackCharacterDetail>() > 0)
        {
          StoryPlaybackCharacterDetail playbackCharacterDetail1 = (source.Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == StoryPlaybackTiming.select_stage)).FirstOrDefault<StoryPlaybackCharacterDetail>() ?? source.Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == StoryPlaybackTiming.before_battle)).FirstOrDefault<StoryPlaybackCharacterDetail>()) ?? source.Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == StoryPlaybackTiming.after_battle)).FirstOrDefault<StoryPlaybackCharacterDetail>();
          if (playbackCharacterDetail1 != null)
          {
            scriptId = playbackCharacterDetail1.script_id;
            if (playbackCharacterDetail1.timing != StoryPlaybackTiming.after_battle)
            {
              StoryPlaybackCharacterDetail playbackCharacterDetail2 = source.Where<StoryPlaybackCharacterDetail>((Func<StoryPlaybackCharacterDetail, bool>) (x => x.timing == StoryPlaybackTiming.after_battle)).FirstOrDefault<StoryPlaybackCharacterDetail>();
              if (playbackCharacterDetail2 != null)
              {
                this.CharacterQuestAfterBattleInfo = this.battleInfo;
                this.CharacterQuestAfterBattleScriptId = playbackCharacterDetail2.script_id;
                break;
              }
              break;
            }
            break;
          }
          break;
        }
        break;
      case CommonQuestType.Extra:
        StoryPlaybackExtraDetail playbackExtraDetail = ((IEnumerable<StoryPlaybackExtraDetail>) MasterData.StoryPlaybackExtraDetailList).Where<StoryPlaybackExtraDetail>((Func<StoryPlaybackExtraDetail, bool>) (x => x.quest_QuestExtraS == this.battleInfo.quest_s_id && x.timing == StoryPlaybackTiming.select_stage)).FirstOrDefault<StoryPlaybackExtraDetail>();
        if (playbackExtraDetail != null)
        {
          scriptId = playbackExtraDetail.script_id;
          break;
        }
        break;
      case CommonQuestType.Sea:
        StoryPlaybackSeaDetail playbackSeaDetail = ((IEnumerable<StoryPlaybackSeaDetail>) MasterData.StoryPlaybackSeaDetailList).Where<StoryPlaybackSeaDetail>((Func<StoryPlaybackSeaDetail, bool>) (x => x.quest_s_id_QuestSeaS == this.battleInfo.quest_s_id && x.timing == StoryPlaybackTiming.select_stage)).FirstOrDefault<StoryPlaybackSeaDetail>();
        if (playbackSeaDetail != null)
        {
          scriptId = playbackSeaDetail.script_id;
          break;
        }
        break;
    }
    Story0093Scene.changeScene(true, scriptId, new bool?(this.isSea), (Action) (() => this.StartCoroutine(this.endStoryOnlyStage())));
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private IEnumerator endStoryOnlyStage()
  {
    NGBattleManager ngBattleManager = this;
    if (ngBattleManager.CharacterQuestAfterBattleInfo != null && ngBattleManager.CharacterQuestAfterBattleScriptId != 0)
    {
      // ISSUE: reference to a compiler-generated method
      Story0093Scene.changeScene(true, ngBattleManager.CharacterQuestAfterBattleScriptId, new bool?(ngBattleManager.isSea), new Action(ngBattleManager.\u003CendStoryOnlyStage\u003Eb__174_0));
      ngBattleManager.CharacterQuestAfterBattleInfo = (BattleInfo) null;
      ngBattleManager.CharacterQuestAfterBattleScriptId = 0;
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<BattleEnd> f = WebAPI.BattleFinish(new WebAPI.Request.BattleFinish()
      {
        quest_type = ngBattleManager.battleInfo.quest_type,
        win = true,
        is_game_over = false,
        battle_uuid = ngBattleManager.battleInfo.battleId,
        player_money = 0,
        battle_turn = 0,
        continue_count = 0,
        week_element_attack_count = 0,
        week_kind_attack_count = 0
      }, (BE) null, (Action<WebAPI.Response.UserError>) (e =>
      {
        NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
        instance.clearStack();
        instance.destroyCurrentScene();
        instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
      }));
      IEnumerator e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result != null)
      {
        Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
        BattleUI05Scene.ChangeScene(ngBattleManager.battleInfo, true, f.Result);
      }
    }
  }

  public BattleskillEffect[] GetSkillEffect(BattleskillSkill skill)
  {
    int id = skill.ID;
    if (!this.battleSkillEffect_.ContainsKey(id))
      this.battleSkillEffect_.Add(id, MasterData.WhereBattleskillEffectBy(skill));
    return this.battleSkillEffect_[id];
  }

  private class OrderValues
  {
    private int _order;
    private float _unitAngleValue;
    private float _cameraAngleYValue;
    private Vector3 _cameraPositionOffsetValue;
    private Vector3 _unitPositionOffsetValue;
    private Vector3 _unitShadowOffsetValue;
    private Quaternion _unitNonTransformRotationValue;

    public OrderValues(int order, NGBattleManager parent)
    {
      this._order = order;
      if (order == 0)
      {
        this._unitAngleValue = parent.unitAngle;
        this._cameraAngleYValue = 0.0f;
        this._cameraPositionOffsetValue = parent.cameraPositionOffset;
        this._unitPositionOffsetValue = new Vector3(0.0f, 0.0f, -0.5f);
        this._unitShadowOffsetValue = new Vector3(0.0f, 0.0f, -0.3f);
        this._unitNonTransformRotationValue = Quaternion.identity;
      }
      else
      {
        this._unitAngleValue = parent.unitAngle * -1f;
        this._cameraAngleYValue = 180f;
        this._cameraPositionOffsetValue = new Vector3(parent.cameraPositionOffset.x, parent.cameraPositionOffset.y, parent.cameraPositionOffset.z * -1f);
        this._unitPositionOffsetValue = new Vector3(0.0f, 0.0f, 0.5f);
        this._unitShadowOffsetValue = new Vector3(0.0f, 0.0f, 0.3f);
        this._unitNonTransformRotationValue = Quaternion.Euler(0.0f, 180f, 0.0f);
      }
    }

    public int order => this._order;

    public float unitAngleValue => this._unitAngleValue;

    public float cameraAngleYValue => this._cameraAngleYValue;

    public Vector3 cameraPositionOffsetValue => this._cameraPositionOffsetValue;

    public Vector3 unitPositionOffsetValue => this._unitPositionOffsetValue;

    public Vector3 unitShadowOffsetValue => this._unitShadowOffsetValue;

    public Quaternion unitNonTransformRotationValue => this._unitNonTransformRotationValue;
  }
}
