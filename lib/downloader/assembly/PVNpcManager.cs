// Decompiled with JetBrains decompiler
// Type: PVNpcManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Client;
using GameCore;
using MasterDataTable;
using Net;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniLinq;
using UnityEngine;

#nullable disable
public class PVNpcManager : Singleton<PVNpcManager>, IGameEngine
{
  private double POINT_LEADER_FACTOR = 2.0;
  private double POINT_NO_LEADER_FACTOR = 1.0;
  private double POINT_COST_FACTOR = 1.0;
  private double POINT_RARITY_FACTOR = 7.0;
  private double POINT_BASE_FACTOR = 12.0;
  private double RESPAWN_BASE_FACTOR = 1.0;
  private double RESPAWN_RARITY_FACTOR = 1.0;
  private double RESPAWN_COST_FACTOR = 1.0;
  private double TURNS_FACTOR = 1.0;
  private int TIME_OUT_DEFAULT = 30;
  private int TIME_OUT_LOCATE_UNITS = 50;
  private int TIME_OUT_MOVE_UNIT_REQUEST = 20;
  private const string winLosePrefabPath = "Prefabs/battle/dir_PvpResults";
  private const string spawnPlayerPrefabPath = "BattleEffects/field/ef657_fe_Multi_Unit_Sporn";
  private const string spawnEnemyPrefabPath = "BattleEffects/field/ef658_fe_Multi_Enemy_Sporn";
  private GameObject winLosePrefab;
  private GameObject spawnPlayerPrefab;
  private GameObject spawnEnemyPrefab;
  private NGBattleManager battleManager;
  public BL envCore;
  private BattleTimeManager _timeManager;
  private BattleAIController _aiController;
  private BattleInputObserver _inputObserver;
  private bool _isWaitAction;
  private bool _isDisposition;
  public bool isResult;
  private Stopwatch stopWatch;
  private BL.StructValue<int> _remainTurn;
  private BL.StructValue<int> _timeLimit;
  private BL.StructValue<int> _playerPoint;
  private BL.StructValue<int> _enemyPoint;
  private BL.StructValue<bool> _isPlayerWipedOut;
  private BL.StructValue<bool> _isEnemyWipedOut;
  private BL.Phase resumePhase = BL.Phase.battle_start_init;
  public WebAPI.Response.PvpFriend enemyInfo;
  private HashSet<BL.Panel> _formationPanel;
  private HashSet<BL.Panel> _formationPanelEnemy;
  private IEnumerator coroutineMain;
  private Action nextStateWaitAction;
  private Dictionary<BattleMonoBehaviour, bool> nextStateFlags = new Dictionary<BattleMonoBehaviour, bool>();
  private List<BattleMonoBehaviour> nextStateFlagsKeys = new List<BattleMonoBehaviour>();
  private int currentLimit;
  private bool actionCompleted;

  private void loadSettingFromMasterData()
  {
    this.POINT_LEADER_FACTOR = (double) this.getPvpFactor("POINT_LEADER_FACTOR");
    this.POINT_NO_LEADER_FACTOR = (double) this.getPvpFactor("POINT_NO_LEADER_FACTOR");
    this.POINT_COST_FACTOR = (double) this.getPvpFactor("POINT_COST_FACTOR");
    this.POINT_RARITY_FACTOR = (double) this.getPvpFactor("POINT_RARITY_FACTOR");
    this.POINT_BASE_FACTOR = (double) this.getPvpFactor("POINT_BASE_FACTOR");
    this.RESPAWN_BASE_FACTOR = (double) this.getPvpFactor("RESPAWN_BASE_FACTOR");
    this.RESPAWN_RARITY_FACTOR = (double) this.getPvpFactor("RESPAWN_RARITY_FACTOR");
    this.RESPAWN_COST_FACTOR = (double) this.getPvpFactor("RESPAWN_COST_FACTOR");
    this.TURNS_FACTOR = (double) this.getPvpFactor("TURNS_FACTOR");
    this.TIME_OUT_DEFAULT = this.getPvpTimeOut("TIME_OUT_DEFAULT");
    this.TIME_OUT_LOCATE_UNITS = this.getPvpTimeOut("TIME_OUT_LOCATE_UNITS");
    this.TIME_OUT_MOVE_UNIT_REQUEST = this.getPvpTimeOut("TIME_OUT_MOVE_UNIT_REQUEST");
  }

  private float getPvpFactor(string key)
  {
    int? nullable = MasterData.PvpSettings.FirstIndexOrNull<KeyValuePair<int, PvpSettings>>((Func<KeyValuePair<int, PvpSettings>, bool>) (x => x.Value.key.Equals(key)));
    return !nullable.HasValue ? 1f : MasterData.PvpSettingsList[nullable.Value].value;
  }

  private int getPvpTimeOut(string key)
  {
    int? nullable = MasterData.PvpSettings.FirstIndexOrNull<KeyValuePair<int, PvpSettings>>((Func<KeyValuePair<int, PvpSettings>, bool>) (x => x.Value.key.Equals(key)));
    return !nullable.HasValue ? this.TIME_OUT_DEFAULT : (int) MasterData.PvpSettingsList[nullable.Value].value;
  }

  public static PVNpcManager createPVNpcManager()
  {
    PVNpcManager pvNpcManager = Singleton<PVNpcManager>.GetInstanceOrNull();
    if (Object.op_Equality((Object) pvNpcManager, (Object) null))
      pvNpcManager = new GameObject("PvNPC_Manager").AddComponent<PVNpcManager>();
    return pvNpcManager;
  }

  public static IEnumerator destroyPVNpcManager()
  {
    PVNpcManager manager = Singleton<PVNpcManager>.GetInstanceOrNull();
    if (!Object.op_Equality((Object) manager, (Object) null))
    {
      yield return (object) manager.cleanup();
      Object.Destroy((Object) ((Component) manager).gameObject);
      manager.clearInstance();
    }
  }

  private BattleTimeManager timeManager
  {
    get
    {
      if (Object.op_Equality((Object) this._timeManager, (Object) null))
        this._timeManager = this.battleManager.getManager<BattleTimeManager>();
      return this._timeManager;
    }
  }

  private BattleAIController aiController
  {
    get
    {
      if (Object.op_Equality((Object) this._aiController, (Object) null))
        this._aiController = this.battleManager.getController<BattleAIController>();
      return this._aiController;
    }
  }

  private BattleInputObserver inputObserver
  {
    get
    {
      if (Object.op_Equality((Object) this._inputObserver, (Object) null))
        this._inputObserver = this.battleManager.getController<BattleInputObserver>();
      return this._inputObserver;
    }
  }

  public bool isWaitAction => this._isWaitAction;

  public bool isDisposition => this._isDisposition;

  public int endPoint => this.stage.point;

  public int endTurn => (int) ((double) this.stage.turns * this.TURNS_FACTOR);

  public BL.StructValue<int> remainTurn => this._remainTurn;

  public BL.StructValue<int> timeLimit => this._timeLimit;

  public BL.StructValue<int> playerPoint => this._playerPoint;

  public BL.StructValue<int> enemyPoint => this._enemyPoint;

  public BL.StructValue<bool> isPlayerWipedOut => this._isPlayerWipedOut;

  public BL.StructValue<bool> isEnemyWipedOut => this._isEnemyWipedOut;

  private int playerPoint_reserve
  {
    get => this.battleManager.environment.core.playerPoint;
    set => this.battleManager.environment.core.playerPoint = value;
  }

  private int enemyPoint_reserve
  {
    get => this.battleManager.environment.core.enemyPoint;
    set => this.battleManager.environment.core.enemyPoint = value;
  }

  public MpStage stage
  {
    get => Persist.pvpSuspend.Data.stage;
    set => Persist.pvpSuspend.Data.stage = value;
  }

  public Player player
  {
    get => Persist.pvpSuspend.Data.player;
    set => Persist.pvpSuspend.Data.player = value;
  }

  public Player enemy
  {
    get => Persist.pvpSuspend.Data.enemy;
    set => Persist.pvpSuspend.Data.enemy = value;
  }

  public PvpMatchingTypeEnum matchingType
  {
    get => Persist.pvpSuspend.Data.matchingType;
    set => Persist.pvpSuspend.Data.matchingType = value;
  }

  public string playerName => this.player.name;

  public string enemyName => this.enemy.name;

  public int playerEmblem => this.player.current_emblem_id;

  public int enemyEmblem => this.enemy.current_emblem_id;

  public HashSet<BL.Panel> formationPanel
  {
    get
    {
      if (this._formationPanel == null)
        this._formationPanel = this.createFormationPanels(this.playerOrder);
      return this._formationPanel;
    }
  }

  private HashSet<BL.Panel> formationPanelEnemy
  {
    get
    {
      if (this._formationPanelEnemy == null)
        this._formationPanelEnemy = this.createFormationPanels(this.playerOrder == 0 ? 1 : 0);
      return this._formationPanelEnemy;
    }
  }

  private HashSet<BL.Panel> createFormationPanels(int order)
  {
    HashSet<BL.Panel> formationPanels = new HashSet<BL.Panel>();
    foreach (PvpStageFormation pvpStageFormation in ((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).Where<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.stage.ID == this.stage.stage_id && x.player_order == order)))
      formationPanels.Add(this.envCore.getFieldPanel(pvpStageFormation.formation_y - 1, pvpStageFormation.formation_x - 1));
    return formationPanels;
  }

  public int playerOrder => this.battleManager.order;

  protected override void Initialize()
  {
    this._remainTurn = new BL.StructValue<int>(0);
    this._timeLimit = new BL.StructValue<int>(0);
    this._playerPoint = new BL.StructValue<int>(0);
    this._enemyPoint = new BL.StructValue<int>(0);
    this._isPlayerWipedOut = new BL.StructValue<bool>(false);
    this._isEnemyWipedOut = new BL.StructValue<bool>(false);
    this.loadSettingFromMasterData();
  }

  public IEnumerator cleanup()
  {
    this.stopMain();
    this.coroutineMain = (IEnumerator) null;
    this.stopWatch = (Stopwatch) null;
    yield break;
  }

  public Future<None> startBattle(BattleInfo battleInfo, bool isRestart)
  {
    return new Future<None>((Func<Promise<None>, IEnumerator>) (promise => this._startBattle(promise, battleInfo, isRestart)));
  }

  private IEnumerator _startBattle(Promise<None> promise, BattleInfo battleInfo, bool isRestart)
  {
    PVNpcManager pvNpcManager = this;
    pvNpcManager.battleManager = Singleton<NGBattleManager>.GetInstance();
    pvNpcManager.battleManager.order = 0;
    yield return (object) pvNpcManager.initPrefabs();
    if (pvNpcManager.stopWatch == null)
      pvNpcManager.stopWatch = new Stopwatch();
    if (!isRestart)
    {
      yield return (object) pvNpcManager.battleReady(battleInfo);
    }
    else
    {
      bool resumeSucceeded = false;
      yield return (object) pvNpcManager.battleResume((Action) (() => resumeSucceeded = true));
      if (!resumeSucceeded)
      {
        yield return (object) pvNpcManager.battleForceClose();
        promise.Exception = new Exception("restarting pvnpc was failed!");
        yield break;
      }
    }
    if (pvNpcManager.coroutineMain == null)
      pvNpcManager.coroutineMain = pvNpcManager.mainUpdate();
    pvNpcManager.StartCoroutine(pvNpcManager.coroutineMain);
    promise.Result = None.Value;
  }

  private IEnumerator battleForceClose()
  {
    IEnumerator e = WebAPI.PvpNpcForceClose(true, new Action<WebAPI.Response.UserError>(this.webErrorCallback)).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bool popupOpen = true;
    ModalWindow.Show(Consts.GetInstance().VERSUS_POPUP_BATTLE_RESUME_ERROR_TITLE, Consts.GetInstance().VERSUS_POPUP_BATTLE_RESUME_ERROR_MESSAGE, (Action) (() => popupOpen = false));
    yield return (object) new WaitWhile((Func<bool>) (() => popupOpen));
  }

  private void webErrorCallback(WebAPI.Response.UserError error)
  {
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(PopupCommon.Show(error.Code, error.Reason, (Action) (() =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
    })));
  }

  private IEnumerator initPrefabs()
  {
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    Future<GameObject> loadF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.winLosePrefab, (Object) null))
    {
      loadF = rm.Load<GameObject>("Prefabs/battle/dir_PvpResults");
      e = loadF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.winLosePrefab = loadF.Result;
      loadF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.spawnPlayerPrefab, (Object) null))
    {
      loadF = rm.Load<GameObject>("BattleEffects/field/ef657_fe_Multi_Unit_Sporn");
      e = loadF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.spawnPlayerPrefab = loadF.Result;
      loadF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.spawnEnemyPrefab, (Object) null))
    {
      loadF = rm.Load<GameObject>("BattleEffects/field/ef658_fe_Multi_Enemy_Sporn");
      e = loadF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.spawnEnemyPrefab = loadF.Result;
      loadF = (Future<GameObject>) null;
    }
  }

  private IEnumerator battleReady(BattleInfo battleInfo)
  {
    yield return (object) this.createEnvironment(battleInfo);
    this.remainTurn.value = this.endTurn;
    this.playerPoint.value = this.envCore.playerPointView;
    this.enemyPoint.value = this.envCore.enemyPointView;
    Persist.pvpSuspend.Flush();
  }

  private IEnumerator createEnvironment(BattleInfo battleInfo)
  {
    BE env = new BE();
    env.core = (BL) new Core(env);
    env.core.battleInfo = battleInfo;
    foreach (PlayerUnit pvpPlayerUnit in battleInfo.pvp_player_units)
      pvpPlayerUnit.is_enemy = false;
    foreach (PlayerUnit pvpEnemyUnit in battleInfo.pvp_enemy_units)
      pvpEnemyUnit.is_enemy = true;
    BattleFuncs.environment.Reset(env.core);
    yield return (object) new BattleLogicInitializer().doStart(battleInfo, env.core);
    this.battleManager.environment = env;
    this.battleManager.battleInfo = battleInfo;
    this.battleManager.saveEnvironment(true);
    this.envCore = this.battleManager.environment.core;
  }

  private IEnumerator battleResume(Action onSuccessCallBack)
  {
    if (this.battleManager.hasSavedEnvironment())
      yield return (object) this.loadEnvironment();
    if (Persist.pvpSuspend.Exists && this.battleManager.hasSavedEnvironment())
    {
      this.resumePhase = this.battleManager.environment.core.phaseState.state;
      if (this.resumePhase == BL.Phase.battle_start)
        this.envCore.phaseState.setStateWith(this.resumePhase, this.envCore, (Action) (() => { }));
      else
        this.envCore.phaseState.setStateWith(BL.Phase.pvp_restart, this.envCore, (Action) (() => { }));
      this.remainTurn.value = this.endTurn - this.envCore.phaseState.turnCount;
      this.playerPoint.value = this.envCore.playerPointView;
      this.enemyPoint.value = this.envCore.enemyPointView;
      onSuccessCallBack();
    }
  }

  private IEnumerator loadEnvironment()
  {
    bool flag = false;
    try
    {
      this.battleManager.loadEnvironment();
      this.battleManager.battleInfo = this.battleManager.environment.core.battleInfo;
      BattleFuncs.environment.Reset(this.battleManager.environment.core);
      flag = true;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) "loadEnvironment() failed: {0}, {1}\n{2}\n{3}".F((object) ex, (object) ex.Message, (object) ex.Source, (object) ex.StackTrace));
      this.battleManager.deleteSavedEnvironment();
    }
    if (flag)
    {
      yield return (object) this.battleManager.initMasterData(this.battleManager.battleInfo);
      this.envCore = this.battleManager.environment.core;
    }
  }

  public void startMain() => this.turnInitialize();

  private void stopMain()
  {
    if (this.stopWatch != null)
      this.stopWatch.Stop();
    if (this.coroutineMain == null)
      return;
    this.StopCoroutine(this.coroutineMain);
  }

  private IEnumerator mainUpdate()
  {
    PVNpcManager pvNpcManager = this;
    // ISSUE: reference to a compiler-generated method
    yield return (object) new WaitUntil(new Func<bool>(pvNpcManager.\u003CmainUpdate\u003Eb__115_0));
    NGSceneManager sceneManager = Singleton<NGSceneManager>.GetInstance();
    while (true)
    {
      // ISSUE: explicit non-virtual call
      while ((Object.op_Equality((Object) pvNpcManager.timeManager, (Object) null) || pvNpcManager.timeManager.isRunning || sceneManager.changeSceneQueueCount > 0 || !sceneManager.isSceneInitialized || !pvNpcManager.battleManager.isBattleEnable || __nonvirtual (pvNpcManager.isWaitAction)) && (pvNpcManager.battleManager.isBattleEnable || pvNpcManager.envCore.phaseState.state != BL.Phase.pvp_disposition))
      {
        pvNpcManager.timeCount();
        if (pvNpcManager.envCore.phaseState.state == BL.Phase.player && pvNpcManager.isTimeOut())
        {
          yield return (object) pvNpcManager.timeoutExec();
          break;
        }
        yield return (object) new WaitForSeconds(0.1f);
      }
      pvNpcManager.timeCount();
      if (pvNpcManager.nextStateWaitAction != null && pvNpcManager.checkNextStateFlags())
      {
        pvNpcManager.nextStateWaitAction();
        pvNpcManager.nextStateWaitAction = (Action) null;
      }
      else
      {
        switch (pvNpcManager.envCore.phaseState.state)
        {
          case BL.Phase.player:
            if (pvNpcManager.isTimeOut())
            {
              yield return (object) pvNpcManager.timeoutExec();
              break;
            }
            break;
          case BL.Phase.pvp_disposition:
            if (pvNpcManager.isTimeOut())
            {
              pvNpcManager.battleManager.popupCloseAll();
              // ISSUE: explicit non-virtual call
              __nonvirtual (pvNpcManager.locateUnitsCompleted());
              break;
            }
            break;
          case BL.Phase.pvp_restart:
            // ISSUE: reference to a compiler-generated method
            pvNpcManager.timeManager.setScheduleAction(new Action(pvNpcManager.\u003CmainUpdate\u003Eb__115_1));
            switch (pvNpcManager.resumePhase)
            {
              case BL.Phase.player:
              case BL.Phase.enemy:
                pvNpcManager.nextState(pvNpcManager.resumePhase);
                break;
              default:
                pvNpcManager.timeManager.setPhaseState(pvNpcManager.resumePhase);
                break;
            }
            break;
        }
      }
      yield return (object) new WaitForSeconds(0.1f);
    }
  }

  private bool isGameFinish()
  {
    return this.playerPoint_reserve >= this.endPoint || this.enemyPoint_reserve >= this.endPoint || this.remainTurn.value <= 0;
  }

  private void turnInitialize()
  {
    this.remainTurn.value = this.endTurn - this.envCore.phaseState.turnCount;
    if (this.isGameFinish())
    {
      this.finishBattle();
    }
    else
    {
      List<BL.Unit> respawnUnits1;
      this.updateRespawnCount((IEnumerable<BL.Unit>) this.envCore.playerUnits.value, out respawnUnits1);
      List<BL.Unit> respawnUnits2;
      this.updateRespawnCount((IEnumerable<BL.Unit>) this.envCore.enemyUnits.value, out respawnUnits2);
      if (respawnUnits1.Count > 0)
      {
        this.respawn((IEnumerable<BL.Unit>) respawnUnits1, this.formationPanel);
        this.setSpawnsEffects(respawnUnits1, this.spawnPlayerPrefab);
      }
      if (respawnUnits2.Count > 0)
      {
        this.respawn((IEnumerable<BL.Unit>) respawnUnits2, this.formationPanelEnemy);
        this.setSpawnsEffects(respawnUnits2, this.spawnEnemyPrefab);
      }
      this.timeManager.setPhaseState(BL.Phase.turn_initialize);
    }
  }

  private void updateRespawnCount(IEnumerable<BL.Unit> units, out List<BL.Unit> respawnUnits)
  {
    respawnUnits = new List<BL.Unit>();
    bool flag = this.checkAnnihilation(units);
    foreach (BL.Unit unit in units)
    {
      if (!unit.isFacility && unit.isDead)
      {
        if (flag)
          unit.pvpRespawnCount = 0;
        else
          --unit.pvpRespawnCount;
        if (unit.pvpRespawnCount == 0)
          respawnUnits.Add(unit);
      }
    }
  }

  private bool checkAnnihilation(IEnumerable<BL.Unit> units)
  {
    foreach (BL.Unit unit in units)
    {
      if (unit.isEnable && !unit.isDead)
        return false;
    }
    return true;
  }

  private void respawn(IEnumerable<BL.Unit> respawnUnits, HashSet<BL.Panel> formationPanels)
  {
    foreach (BL.Unit respawnUnit in respawnUnits)
    {
      BL.UnitPosition unitPosition = this.envCore.getUnitPosition(respawnUnit);
      foreach (BL.Panel panel in formationPanels.Shuffle<BL.Panel>())
      {
        if (this.envCore.getFieldUnit(panel, includeJumping: true) == null && this.envCore.isMoveOKPanel(panel, respawnUnit, false, unitPosition.moveCost))
        {
          this.resetPosition(unitPosition, panel.row, panel.column, true);
          break;
        }
      }
    }
  }

  private void resetPosition(BL.UnitPosition up, int row, int column, bool resetDirection)
  {
    RecoveryUtility.resetPosition(up, row, column, this.envCore);
    if (!resetDirection)
      return;
    up.direction = this.playerOrder != 0 ? (up.unit.isPlayerControl ? 180f : 0.0f) : (up.unit.isPlayerControl ? 0.0f : 180f);
    if (!up.unit.isView)
      return;
    this.battleManager.environment.unitResource[up.unit].unitParts_.moveStayUpdate();
  }

  private void setSpawnsEffects(List<BL.Unit> spawns, GameObject prefab)
  {
    BE env = this.battleManager.environment;
    this.battleManager.battleEffects.skillFieldEffectStartCore(new BattleskillFieldEffect()
    {
      user_move_camera = false,
      user_wait_seconds = 0.0f,
      targets_multiple_effect = false,
      target_move_camera = true,
      target_wait_seconds = 0.5f
    }, (BL.Unit) null, spawns, (GameObject) null, (GameObject) null, prefab, (Action) null, (Action) (() =>
    {
      foreach (BL.Unit spawn in spawns)
      {
        spawn.rebirth(env.core);
        spawn.rebirthBE(env);
      }
    }), (List<BL.Unit>) null);
  }

  public bool checkDuelDeadUnitAndFinish(BL.Unit attack, BL.Unit defense)
  {
    return this.deadUnitAndFinish(attack) || this.deadUnitAndFinish(defense);
  }

  private bool deadUnitAndFinish(BL.Unit u)
  {
    if (u.hp > 0 || BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) u).Any<BL.SkillEffect>())
      return false;
    int point = this.calcPoint(u);
    int num = BattleFuncs.useOvoPointInterference(u, point, true);
    return this.envCore.getForceID(u) == BL.ForceID.enemy ? this.playerPoint_reserve + num >= this.endPoint : this.enemyPoint_reserve + num >= this.endPoint;
  }

  public void applyDeadUnit(BL.Unit attack, BL.Unit defense)
  {
    if (attack.hp <= 0 && !BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) attack).Any<BL.SkillEffect>())
      this.apllyUnitData(attack, defense);
    if (!(defense != (BL.Unit) null) || defense.hp > 0 || BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) defense).Any<BL.SkillEffect>())
      return;
    this.apllyUnitData(defense, attack);
  }

  private void apllyUnitData(BL.Unit deadUnit, BL.Unit killUnit)
  {
    int point = this.calcPoint(deadUnit);
    int num = BattleFuncs.useOvoPointInterference(deadUnit, point);
    if (this.envCore.getForceID(deadUnit) == BL.ForceID.enemy)
      this.playerPoint_reserve += num;
    else
      this.enemyPoint_reserve += num;
    if (killUnit != (BL.Unit) null)
      killUnit.pvpPoint += num;
    deadUnit.pvpRespawnCount = this.calcRespawnCount(deadUnit);
  }

  private int calcPoint(BL.Unit unit)
  {
    return (int) (this.POINT_BASE_FACTOR + ((double) (unit.unit.rarity.index + 1) * this.POINT_RARITY_FACTOR + (double) unit.playerUnit.cost * this.POINT_COST_FACTOR) * (unit.is_leader ? this.POINT_LEADER_FACTOR : this.POINT_NO_LEADER_FACTOR));
  }

  private int calcRespawnCount(BL.Unit unit)
  {
    return (int) (this.RESPAWN_BASE_FACTOR + ((double) (unit.unit.rarity.index + 1) * this.RESPAWN_RARITY_FACTOR + (double) unit.playerUnit.cost * this.RESPAWN_COST_FACTOR));
  }

  public void deadReserveToPoint(bool isEnemyDead, bool needAnnihilateCheck)
  {
    if (this.envCore.phaseState.state == BL.Phase.finalize)
      return;
    if (isEnemyDead)
    {
      if (this.playerPoint.value != this.playerPoint_reserve)
        this.playerPoint.value = this.envCore.playerPointView = this.playerPoint_reserve;
    }
    else if (this.enemyPoint.value != this.enemyPoint_reserve)
      this.enemyPoint.value = this.envCore.enemyPointView = this.enemyPoint_reserve;
    this.clearNextStateFlags();
    this.timeManager.setEnableWait(0.2f);
    if (!needAnnihilateCheck)
      return;
    if (isEnemyDead)
    {
      if (!this.checkAnnihilation((IEnumerable<BL.Unit>) this.envCore.enemyUnits.value))
        return;
      this.timeManager.setScheduleAction((Action) (() =>
      {
        this.isPlayerWipedOut.value = true;
        this.playerPoint.value = this.envCore.playerPointView = (this.playerPoint_reserve += this.stage.annihilation_point);
        ++this.envCore.enemyAnnihilationCount;
      }));
      this.timeManager.setEnableWait(0.2f);
    }
    else
    {
      if (!this.checkAnnihilation((IEnumerable<BL.Unit>) this.envCore.playerUnits.value))
        return;
      this.timeManager.setScheduleAction((Action) (() =>
      {
        this.isEnemyWipedOut.value = true;
        this.enemyPoint.value = this.envCore.enemyPointView = (this.enemyPoint_reserve += this.stage.annihilation_point);
        ++this.envCore.playerAnnihilationCount;
      }));
      this.timeManager.setEnableWait(0.2f);
    }
  }

  private void finishBattle()
  {
    this.envCore.isWin = this.playerPoint_reserve > this.enemyPoint_reserve;
    this.battleManager.battleEffects.startEffect((Transform) null, 3f, popupPrefab: this.winLosePrefab, cloneAction: (Action<GameObject>) (o => o.GetComponent<PopupPvpMatchResult>().setResult(this.getFinishBattle(), this.playerOrder)));
    this.timeManager.setPhaseState(BL.Phase.pvp_result);
    this.stopMain();
  }

  private AppPeer.FinishBattle getFinishBattle()
  {
    AppPeer.FinishBattle finishBattle = new AppPeer.FinishBattle();
    finishBattle.victoryEffects = new PvpVictoryEffectEnum[1];
    finishBattle.victoryEffects[0] = this.GetPlayerVictoryEffect();
    return finishBattle;
  }

  public PvpVictoryTypeEnum GetPlayerVictory()
  {
    return this.GetLeftVictory(this.playerPoint.value, this.enemyPoint.value);
  }

  public PvpVictoryTypeEnum GetEnemyVictory()
  {
    return this.GetLeftVictory(this.enemyPoint.value, this.playerPoint.value);
  }

  private PvpVictoryTypeEnum GetLeftVictory(int leftPoint, int rightPoint)
  {
    int num1 = Mathf.Min(leftPoint, this.stage.point);
    int num2 = Mathf.Min(rightPoint, this.stage.point);
    if (num1 > num2)
      return PvpVictoryTypeEnum.win;
    return num2 > num1 ? PvpVictoryTypeEnum.lose : PvpVictoryTypeEnum.draw;
  }

  public int GetPlayerPointValue() => this.playerPoint.value;

  public int GetEnemyPointValue() => this.enemyPoint.value;

  public PvpVictoryEffectEnum GetPlayerVictoryEffect()
  {
    return this.GetLeftVictoryEffect(this.playerPoint.value, this.enemyPoint.value);
  }

  public PvpVictoryEffectEnum GetEnemyVictoryEffect()
  {
    return this.GetLeftVictoryEffect(this.enemyPoint.value, this.playerPoint.value);
  }

  private PvpVictoryEffectEnum GetLeftVictoryEffect(int leftPoint, int rightPoint)
  {
    int num1 = Mathf.Min(leftPoint, this.stage.point);
    int num2 = Mathf.Min(rightPoint, this.stage.point);
    if (num1 < num2)
      return PvpVictoryEffectEnum.lose_effect;
    if (num1 == num2)
      return PvpVictoryEffectEnum.draw_effect;
    return num2 == 0 ? (this.envCore.phaseState.turnCount <= this.endTurn ? PvpVictoryEffectEnum.excellent_effect : PvpVictoryEffectEnum.win_effect) : (num1 >= this.stage.point ? PvpVictoryEffectEnum.great_effect : PvpVictoryEffectEnum.win_effect);
  }

  public void execNextState(BattleMonoBehaviour bmb)
  {
    if (!this.nextStateFlags.ContainsKey(bmb))
      this.nextStateFlagsKeys.Add(bmb);
    this.nextStateFlags[bmb] = true;
  }

  private bool checkNextStateFlags()
  {
    foreach (KeyValuePair<BattleMonoBehaviour, bool> nextStateFlag in this.nextStateFlags)
    {
      if (!nextStateFlag.Value)
        return false;
    }
    return true;
  }

  private void clearNextStateFlags()
  {
    foreach (BattleMonoBehaviour nextStateFlagsKey in this.nextStateFlagsKeys)
      this.nextStateFlags[nextStateFlagsKey] = false;
  }

  private bool setNextPhase(List<BL.UnitPosition> units, BL.Phase state, BL.Phase cantChangeState)
  {
    if (this.envCore.currentUnitPosition.cantChangeCurrent)
    {
      this.timeManager.setPhaseState(cantChangeState);
      if (cantChangeState == BL.Phase.pvp_player_start)
      {
        this.resetTimeLimit(this.TIME_OUT_MOVE_UNIT_REQUEST);
        this._isWaitAction = false;
      }
      else
        this._isWaitAction = true;
      return true;
    }
    if (units.Count > 0)
    {
      this.timeManager.setPhaseState(state);
      switch (state)
      {
        case BL.Phase.pvp_player_start:
          this.resetTimeLimit(this.TIME_OUT_MOVE_UNIT_REQUEST);
          this._isWaitAction = false;
          break;
        case BL.Phase.pvp_enemy_start:
          this._isWaitAction = true;
          break;
      }
      return true;
    }
    this._isWaitAction = false;
    return false;
  }

  private void nextState(BL.Phase state)
  {
    this.envCore.phaseState.setStateWith(BL.Phase.none, this.envCore, (Action) (() => { }));
    this.nextStateWaitAction = (Action) (() =>
    {
      if (this.isGameFinish())
      {
        this.finishBattle();
      }
      else
      {
        switch (state)
        {
          case BL.Phase.player:
          case BL.Phase.pvp_move_unit_waiting:
            if (this.setNextPhase(this.envCore.enemyActionUnits.value, BL.Phase.pvp_enemy_start, BL.Phase.pvp_player_start) || this.setNextPhase(this.envCore.playerActionUnits.value, BL.Phase.pvp_player_start, BL.Phase.pvp_player_start))
              break;
            this.turnInitialize();
            break;
          case BL.Phase.enemy:
          case BL.Phase.turn_initialize:
            if (this.setNextPhase(this.envCore.playerActionUnits.value, BL.Phase.pvp_player_start, BL.Phase.pvp_enemy_start) || this.setNextPhase(this.envCore.enemyActionUnits.value, BL.Phase.pvp_enemy_start, BL.Phase.pvp_enemy_start))
              break;
            this.turnInitialize();
            break;
        }
      }
    });
    this.clearNextStateFlags();
  }

  private void setTimeLimit(int limit, long swms)
  {
    long num1 = 100;
    int num2 = (int) (((long) (limit * 1000) + num1 - swms) / 1000L);
    if (num2 < 0)
      num2 = 0;
    if (this.timeLimit.value == num2)
      return;
    this.timeLimit.value = num2;
  }

  private void resetTimeLimit(int limit)
  {
    this.currentLimit = limit;
    this.actionCompleted = false;
    this.timeManager.setScheduleAction((Action) (() =>
    {
      this.stopWatch.Reset();
      this.stopWatch.Start();
      this.setTimeLimit(limit, this.stopWatch.ElapsedMilliseconds);
    }));
  }

  private void timeCount()
  {
    if (this.stopWatch == null || !this.stopWatch.IsRunning || this._isWaitAction)
      return;
    this.setTimeLimit(this.currentLimit, this.stopWatch.ElapsedMilliseconds);
  }

  private bool isTimeOut()
  {
    return this.stopWatch != null && this.stopWatch.IsRunning && this._timeLimit.value == 0;
  }

  private bool isAnyUnitMoving(BE env)
  {
    foreach (BL.UnitPosition up in env.core.unitPositions.value)
    {
      if (up.isMoving(env))
        return true;
    }
    return false;
  }

  private IEnumerator timeoutExec()
  {
    PVNpcManager pvNpcManager = this;
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    pvNpcManager.stopWatch.Reset();
    BL.UnitPosition cup = pvNpcManager.envCore.currentUnitPosition;
    if (!pvNpcManager.actionCompleted)
    {
      while (pvNpcManager.timeManager.isRunning || sm.changeSceneQueueCount > 0 || !sm.isSceneInitialized)
        yield return (object) new WaitForSeconds(0.1f);
      if (sm.sceneName != pvNpcManager.battleManager.topScene)
        pvNpcManager.timeManager.backSceneWithReturnWait();
      pvNpcManager.battleManager.popupCloseAll();
      if (cup.cantChangeCurrent)
        cup.cancelMove(pvNpcManager.battleManager.environment);
      else
        pvNpcManager.battleManager.environment.setCurrentUnit_((BL.Unit) null);
      pvNpcManager.inputObserver.onCancel(true);
      while (pvNpcManager.timeManager.isRunning || sm.sceneName != pvNpcManager.battleManager.topScene || sm.changeSceneQueueCount > 0 || !sm.isSceneInitialized)
        yield return (object) new WaitForSeconds(0.1f);
      // ISSUE: reference to a compiler-generated method
      yield return (object) new WaitWhile(new Func<bool>(pvNpcManager.\u003CtimeoutExec\u003Eb__155_0));
      if (cup.cantChangeCurrent)
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (pvNpcManager.moveUnit(cup));
      }
      else
      {
        BL.UnitPosition up = pvNpcManager.envCore.playerActionUnits.value[0];
        // ISSUE: explicit non-virtual call
        __nonvirtual (pvNpcManager.moveUnit(up));
      }
    }
  }

  public void readyComplited()
  {
    this.timeManager.setPhaseState(BL.Phase.pvp_disposition);
    this.resetTimeLimit(this.TIME_OUT_LOCATE_UNITS);
  }

  public void autoOnRequest()
  {
  }

  public void locateUnitsCompleted()
  {
    foreach (BL.Unit unit in this.envCore.playerUnits.value)
      this.envCore.getUnitPosition(unit).completeActionUnit(this.envCore);
    this._isDisposition = true;
    this.stopWatch.Reset();
    this.timeManager.setPhaseState(BL.Phase.pvp_start_init);
  }

  public void turnInitializeCompleted() => this.nextState(BL.Phase.turn_initialize);

  public void actionUnitCompleted()
  {
    this.aiController.clearAIActionOrder();
    this._isWaitAction = false;
    this.nextState(this.battleManager.environment.core.phaseState.state);
  }

  public void wipedOutCompleted()
  {
  }

  public void moveUnit(BL.UnitPosition up)
  {
    up.completeActionUnit(this.envCore, true);
    this.actionCompleted = true;
    this.battleManager.environment.core.setSomeAction();
    this.timeManager.setScheduleAction((Action) (() => this.nextState(this.envCore.phaseState.state)));
  }

  public void moveUnitWithAttack(
    BL.UnitPosition attack,
    BL.UnitPosition defense,
    bool isHeal,
    int attackStatusIndex)
  {
    this._isWaitAction = true;
    this.actionCompleted = true;
    this.battleManager.environment.core.setSomeAction();
    BE environment = this.battleManager.environment;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    AttackStatus attackStatus = BattleFuncs.getAttackStatusArray(attack, defense, true, isHeal)[attackStatusIndex];
    if (isHeal)
    {
      if (instance.sceneName != this.battleManager.topScene)
        this.timeManager.backSceneWithReturnWait();
      BE be = environment;
      BL.MagicBullet magicBullet = attackStatus.magicBullet;
      int attack1 = attackStatus.healAttack((BL.ISkillEffectListUnit) attack.unit, (BL.ISkillEffectListUnit) defense.unit);
      BL.Unit unit = attack.unit;
      List<BL.Unit> targets = new List<BL.Unit>();
      targets.Add(defense.unit);
      BattleTimeManager timeManager = this.timeManager;
      be.useMagicBullet(magicBullet, attack1, unit, targets, timeManager);
    }
    else
      this.battleManager.startDuel(BattleFuncs.calcDuel(attackStatus, attack, defense));
    this.timeManager.setScheduleAction((Action) (() =>
    {
      this._isWaitAction = false;
      this.nextState(this.envCore.phaseState.state);
    }));
  }

  public void moveUnitWithAttack(
    BL.Unit attack,
    BL.Unit defense,
    bool isHeal,
    int attackStatusIndex)
  {
    this.moveUnitWithAttack(this.envCore.getUnitPosition(attack), this.envCore.getUnitPosition(defense), isHeal, attackStatusIndex);
  }

  public void moveUnitWithSkill(
    BL.Unit unit,
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Panel> panels)
  {
    this.actionCompleted = true;
    this.battleManager.environment.core.setSomeAction();
    this.battleManager.environment.useSkill(unit, skill, targets, panels, (BL.BattleSkillResult) null, this.timeManager);
    this.timeManager.setScheduleAction((Action) (() => this.nextState(this.envCore.phaseState.state)));
  }

  public void useCallSkill(BL.Skill skill, List<BL.Unit> targets, bool isPlayer)
  {
    if (isPlayer)
      this.stopWatch.Stop();
    this.battleManager.environment.useCallSkill(skill, targets, this.timeManager, isPlayer);
    if (!isPlayer)
      return;
    this.timeManager.setScheduleAction((Action) (() =>
    {
      this.currentLimit = this.TIME_OUT_MOVE_UNIT_REQUEST;
      this.stopWatch.Reset();
      this.stopWatch.Start();
      this.setTimeLimit(this.TIME_OUT_MOVE_UNIT_REQUEST, this.stopWatch.ElapsedMilliseconds);
    }));
  }
}
