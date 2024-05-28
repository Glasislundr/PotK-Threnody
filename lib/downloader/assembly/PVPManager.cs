// Decompiled with JetBrains decompiler
// Type: PVPManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Client;
using GameCore;
using GameCore.FastMiniJSON;
using MasterDataTable;
using Net;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UniLinq;
using UnityEngine;

#nullable disable
public class PVPManager : Singleton<PVPManager>, IGameEngine
{
  [SerializeField]
  private int timeoutMilliseconds = 90000;
  [SerializeField]
  private float loadingMinTime = 0.5f;
  private string winLosePrefabPath = "Prefabs/battle/dir_PvpResults";
  private string loadingPrefabPath = "Prefabs/common/Loading/Loading_BattlePrefab";
  private string spawnPlayerPrefabPath = "BattleEffects/field/ef657_fe_Multi_Unit_Sporn";
  private string spawnEnemyPrefabPath = "BattleEffects/field/ef658_fe_Multi_Enemy_Sporn";
  private GameObject winLosePrefab;
  private GameObject loadingPrefab;
  private GameObject spawnPlayerPrefab;
  private GameObject spawnEnemyPrefab;
  private Battle01SelectNode selectNode;
  private bool isPvpExceptionLoadingDisp;
  public Exception exception;
  private string _errorCode;
  private NGBattleManager bm;
  private AppPeer peer;
  private BattleTimeManager _btm;
  private BattleAIController _aiController;
  private BattleInputObserver _inputObserver;
  private bool _isRunning;
  private bool _isWaitAction;
  private bool _isDisposition;
  private bool sendLocateComplited;
  public bool isResult;
  private Stopwatch _stopwatch;
  private BL.StructValue<int> _remainTurn;
  private BL.StructValue<int> _timeLimit;
  private BL.StructValue<int> _playerPoint;
  private BL.StructValue<int> _enemyPoint;
  private BL.StructValue<bool> _isPlayerWipedOut;
  private BL.StructValue<bool> _isEnemyWipedOut;
  private int playerPoint_reserve;
  private int enemyPoint_reserve;
  public WebAPI.Response.PvpFriend enemyInfo;
  public XorShift random;
  private HashSet<BL.Panel> _formationPanel;
  private DateTime loadingStartTime;
  private bool isLoading;
  public bool isSending;

  private void OnValidate()
  {
  }

  public string errorCode
  {
    get => string.IsNullOrEmpty(this._errorCode) ? (string) null : this._errorCode;
    set
    {
      if (string.IsNullOrEmpty(this._errorCode))
      {
        this._errorCode = value;
      }
      else
      {
        if (!string.IsNullOrEmpty(value))
          return;
        this._errorCode = value;
      }
    }
  }

  private BattleTimeManager btm
  {
    get
    {
      if (Object.op_Equality((Object) this._btm, (Object) null))
        this._btm = this.bm.getManager<BattleTimeManager>();
      return this._btm;
    }
  }

  private BattleAIController aiController
  {
    get
    {
      if (Object.op_Equality((Object) this._aiController, (Object) null))
        this._aiController = this.bm.getController<BattleAIController>();
      return this._aiController;
    }
  }

  private BattleInputObserver inputObserver
  {
    get
    {
      if (Object.op_Equality((Object) this._inputObserver, (Object) null))
        this._inputObserver = this.bm.getController<BattleInputObserver>();
      return this._inputObserver;
    }
  }

  public bool isRunning => this._isRunning;

  public bool isWaitAction => this._isWaitAction;

  public bool isDisposition => this._isDisposition;

  public int endPoint => this.stage.point;

  public BL.StructValue<int> remainTurn => this._remainTurn;

  public BL.StructValue<int> timeLimit => this._timeLimit;

  public BL.StructValue<int> playerPoint => this._playerPoint;

  public BL.StructValue<int> enemyPoint => this._enemyPoint;

  public BL.StructValue<bool> isPlayerWipedOut => this._isPlayerWipedOut;

  public BL.StructValue<bool> isEnemyWipedOut => this._isEnemyWipedOut;

  private string host
  {
    get => Persist.pvpSuspend.Data.host;
    set => Persist.pvpSuspend.Data.host = value;
  }

  private int port
  {
    get => Persist.pvpSuspend.Data.port;
    set => Persist.pvpSuspend.Data.port = value;
  }

  private string battleToken
  {
    get => Persist.pvpSuspend.Data.token;
    set => Persist.pvpSuspend.Data.token = value;
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

  protected override void Initialize()
  {
    this.bm = Singleton<NGBattleManager>.GetInstance();
    this._remainTurn = new BL.StructValue<int>(0);
    this._timeLimit = new BL.StructValue<int>(0);
    this._playerPoint = new BL.StructValue<int>(0);
    this._enemyPoint = new BL.StructValue<int>(0);
    this._isPlayerWipedOut = new BL.StructValue<bool>(false);
    this._isEnemyWipedOut = new BL.StructValue<bool>(false);
    this.playerPoint_reserve = this.playerPoint.value;
    this.enemyPoint_reserve = this.enemyPoint.value;
  }

  public void setUiNode(Battle01SelectNode node)
  {
    this.selectNode = node;
    this.isPvpExceptionLoadingDisp = false;
  }

  public int playerOrder => this.bm.order;

  public void deadReserveToPoint(bool isEnemy, bool needAnnihilateCheck)
  {
    if (this.bm.environment.core.phaseState.state == BL.Phase.finalize)
      return;
    if (isEnemy)
    {
      if (this.playerPoint.value == this.playerPoint_reserve)
        return;
      this.playerPoint.value = this.playerPoint_reserve;
    }
    else
    {
      if (this.enemyPoint.value == this.enemyPoint_reserve)
        return;
      this.enemyPoint.value = this.enemyPoint_reserve;
    }
  }

  public void applyDeadUnit(BL.Unit attack, BL.Unit defense)
  {
    if (attack.hp <= 0 && !BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) attack).Any<BL.SkillEffect>())
      BattleFuncs.useOvoPointInterference(attack, 0);
    if (!(defense != (BL.Unit) null) || defense.hp > 0 || BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) defense).Any<BL.SkillEffect>())
      return;
    BattleFuncs.useOvoPointInterference(defense, 0);
  }

  public PVPMatching getMatchingBehaviour()
  {
    PVPMatching matchingBehaviour = ((Component) this).gameObject.GetComponent<PVPMatching>();
    if (Object.op_Equality((Object) matchingBehaviour, (Object) null))
      matchingBehaviour = ((Component) this).gameObject.AddComponent<PVPMatching>();
    return matchingBehaviour;
  }

  public void startMain()
  {
    if (this._stopwatch == null)
      this._stopwatch = new Stopwatch();
    if (!this.isDisposition)
    {
      this.StartCoroutine("doPvpDisposition");
    }
    else
    {
      if (this.isRunning)
        return;
      this._isRunning = true;
      this.StartCoroutine("doPvpMain");
    }
  }

  public void stopPVPMain()
  {
    if (!this.isDisposition)
      this.StopCoroutine("doPvpDisposition");
    else if (this.isRunning)
    {
      this.StopCoroutine("doPvpMain");
      this._isRunning = false;
    }
    this._stopwatch = (Stopwatch) null;
    if (!this.isLoading)
      return;
    this._viewLoading(false);
  }

  public IEnumerator cleanupPVP()
  {
    this.stopPVPMain();
    if (this.peer != null)
    {
      this.peer.Close();
      this.peer = (AppPeer) null;
      yield break;
    }
  }

  public Future<None> startPVP(string host, int port, string battleToken, bool isRestart)
  {
    return new Future<None>((Func<Promise<None>, IEnumerator>) (promise => this._startPVP(promise, host, port, battleToken, isRestart)));
  }

  private IEnumerator _startPVP(
    Promise<None> promise,
    string host,
    int port,
    string battleToken,
    bool isRestart)
  {
    PVPManager pvpManager = this;
    pvpManager.host = host;
    pvpManager.port = port;
    pvpManager.battleToken = battleToken;
    if (Object.op_Equality((Object) pvpManager.bm, (Object) null))
      pvpManager.bm = Singleton<NGBattleManager>.GetInstance();
    IEnumerator e;
    if (!isRestart)
    {
      Future<AppPeer> cF = pvpManager.connectServer(host, port, battleToken);
      e = cF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (cF.Exception != null)
      {
        pvpManager.errorCode = "002607";
        promise.Exception = cF.Exception;
        yield break;
      }
      else
      {
        pvpManager.peer = cF.Result;
        e = pvpManager._battleReady(promise);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        cF = (Future<AppPeer>) null;
      }
    }
    else
    {
      if (pvpManager.bm.hasSavedEnvironment())
      {
        try
        {
          pvpManager.bm.loadEnvironment();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) "loadEnvironment() failed: {0}, {1}\n{2}\n{3}".F((object) ex, (object) ex.Message, (object) ex.Source, (object) ex.StackTrace));
          pvpManager.bm.deleteSavedEnvironment();
        }
      }
      if (!pvpManager.bm.hasSavedEnvironment())
      {
        Future<WebAPI.Response.PvpResume> resumeF = WebAPI.PvpResume();
        e = resumeF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        PVPManager.CreateEnvInfo info = new PVPManager.CreateEnvInfo();
        info.EncodeResume(resumeF.Result);
        e = pvpManager.createEnvironment(info);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        resumeF = (Future<WebAPI.Response.PvpResume>) null;
      }
      e = pvpManager.bm.initMasterData(pvpManager.bm.environment.core.battleInfo);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      BattleFuncs.environment.Reset(pvpManager.bm.environment.core);
      pvpManager.bm.battleInfo = pvpManager.bm.environment.core.battleInfo;
      pvpManager.bm.battleInfo.host = host;
      pvpManager.bm.battleInfo.port = port;
      pvpManager.bm.battleInfo.battleToken = battleToken;
      pvpManager.bm.order = Persist.pvpSuspend.Data.order;
      pvpManager.bm.environment.core.phaseState.setStateWith(BL.Phase.pvp_restart, pvpManager.bm.environment.core, (Action) (() => { }));
      pvpManager.StartCoroutine(pvpManager.doErrorRecovery(true));
    }
    promise.Result = None.Value;
  }

  public Future<AppPeer> connectServer(string host, int port, string battleToken)
  {
    return new Future<AppPeer>((Func<Promise<AppPeer>, IEnumerator>) (promise => this._connectServer(promise, host, port, battleToken)));
  }

  private IEnumerator _connectServer(
    Promise<AppPeer> promise,
    string host,
    int port,
    string battleToken)
  {
    AppPeer apeer = new AppPeer();
    Future<None> connectF = apeer.Connect(host, port);
    IEnumerator e = connectF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (connectF.Exception != null)
    {
      promise.Exception = connectF.Exception;
    }
    else
    {
      if (battleToken != null)
      {
        Future<AppPeer.SendMessage> joinF = apeer.JoinRoom(battleToken);
        e = joinF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (joinF.Exception != null)
        {
          this.errorCode = "002608";
          promise.Exception = joinF.Exception;
          yield break;
        }
        else
          joinF = (Future<AppPeer.SendMessage>) null;
      }
      promise.Result = apeer;
    }
  }

  private IEnumerator _battleReady(Promise<None> promise)
  {
    Future<AppPeer.ReceivedMessage> readyF = this.peer.ReceiveOps(this.timeoutMilliseconds, AppServerOperation.Ready);
    IEnumerator e = readyF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (readyF.Result.Error != null)
    {
      this.errorCode = "002609";
      promise.Exception = readyF.Result.Error;
    }
    else
    {
      PVPManager.CreateEnvInfo info = new PVPManager.CreateEnvInfo();
      info.EncodeReady(readyF.Result.Ready);
      e = this.createEnvironment(info);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      promise.Result = None.Value;
    }
  }

  private IEnumerator createEnvironment(PVPManager.CreateEnvInfo info)
  {
    BE env = new BE();
    BattleLogicInitializer initializer = new BattleLogicInitializer();
    PlayerUnit[] player1Units = info.player1_units;
    PlayerUnit[] player2Units = info.player2_units;
    if (info.order == 0)
    {
      this.player = info.player1;
      this.enemy = info.player2;
      foreach (PlayerUnit playerUnit in player1Units)
        playerUnit.is_enemy = false;
      foreach (PlayerUnit playerUnit in player2Units)
        playerUnit.is_enemy = true;
    }
    else
    {
      this.player = info.player2;
      this.enemy = info.player1;
      foreach (PlayerUnit playerUnit in player2Units)
        playerUnit.is_enemy = false;
      foreach (PlayerUnit playerUnit in player1Units)
        playerUnit.is_enemy = true;
    }
    BattleInfo.CallSkillParam callSkillParam1 = this.player.GetCallSkillParam();
    BattleInfo.CallSkillParam callSkillParam2 = this.enemy.GetCallSkillParam();
    BattleInfo battleInfo = BattleInfo.MakePvpBattleInfo(info.battle_uuid, info.stage.stage_id, player1Units, player2Units, info.player1_items, info.player2_items, (PlayerUnit[]) null, (PlayerUnit[]) null, info.player1_reisou_items, info.player2_reisou_items, info.bonus, info.player1_character_intimates, info.player2_character_intimates, info.player1_awake_skills, info.player2_awake_skills, info.battle_start_at, callSkillParam1, callSkillParam2);
    env.core = (BL) new Core(env);
    env.core.battleInfo = battleInfo;
    IEnumerator e = this.bm.initMasterData(battleInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    BattleFuncs.environment.Reset(env.core);
    e = initializer.doStart(battleInfo, env.core);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.bm.environment = env;
    this.bm.battleInfo = battleInfo;
    this.bm.order = Persist.pvpSuspend.Data.order = info.order;
    this.stage = info.stage;
    this.resetPlayerControllUnits(this.bm.order);
    env.core.battleInfo.host = this.host;
    env.core.battleInfo.port = this.port;
    env.core.battleInfo.battleToken = this.battleToken;
    BL.UnitPosition[] array = env.core.playerUnits.value.Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (u => env.core.getUnitPosition(u))).ToArray<BL.UnitPosition>();
    Persist.PvpUnitPositions data = Persist.PvpUnitPositions.getData(this.stage, array.Length, this.playerOrder);
    if (data != null)
    {
      for (int index = 0; index < array.Length; ++index)
        RecoveryUtility.resetPosition(array[index], data.positions[index].Item1, data.positions[index].Item2, env.core);
    }
    this.bm.saveEnvironment(true);
    Persist.pvpSuspend.Flush();
  }

  private void resetPlayerControllUnits(int order)
  {
    if (order != 1)
      return;
    BE environment = this.bm.environment;
    BL.ClassValue<List<BL.Unit>> playerUnits = environment.core.playerUnits;
    environment.core.playerUnits = environment.core.enemyUnits;
    environment.core.enemyUnits = playerUnits;
    foreach (BL.Unit unit in environment.core.playerUnits.value)
    {
      unit.isPlayerControl = true;
      unit.isPlayerForce = true;
    }
    foreach (BL.Unit unit in environment.core.enemyUnits.value)
    {
      unit.isPlayerControl = false;
      unit.isPlayerForce = false;
    }
    foreach (BL.Unit unit in environment.core.facilityUnits.value)
    {
      unit.playerUnit.is_enemy = !unit.playerUnit.is_enemy;
      unit.facility.thisForce = unit.playerUnit.is_enemy ? BL.ForceID.enemy : BL.ForceID.player;
    }
  }

  private IEnumerator initPrefabs()
  {
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    Future<GameObject> tmpF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.loadingPrefab, (Object) null))
    {
      tmpF = rm.Load<GameObject>(this.loadingPrefabPath);
      e = tmpF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.loadingPrefab = tmpF.Result;
    }
    if (Object.op_Equality((Object) this.winLosePrefab, (Object) null))
    {
      tmpF = rm.Load<GameObject>(this.winLosePrefabPath);
      e = tmpF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.winLosePrefab = tmpF.Result;
    }
    if (Object.op_Equality((Object) this.spawnPlayerPrefab, (Object) null))
    {
      tmpF = rm.Load<GameObject>(this.spawnPlayerPrefabPath);
      e = tmpF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.spawnPlayerPrefab = tmpF.Result;
    }
    if (Object.op_Equality((Object) this.spawnEnemyPrefab, (Object) null))
    {
      tmpF = rm.Load<GameObject>(this.spawnEnemyPrefabPath);
      e = tmpF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.spawnEnemyPrefab = tmpF.Result;
    }
  }

  private IEnumerator doPvpDisposition()
  {
    BE env = this.bm.environment;
    IEnumerator e = this.initPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<AppPeer.ReceivedMessage> gF;
    AppPeer.GameInitialize gameInitialize;
    while (true)
    {
      Future<AppPeer.ReceivedMessage> rF;
      int limit;
      AppPeer.LocateUnits locateUnits;
      do
      {
        this._viewLoading(true);
        rF = this.peer.ReceiveOps(this.timeoutMilliseconds, AppServerOperation.LocateUnits);
        e = rF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = this.hideLoading();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (rF.Result.Error != null)
        {
          this.errorCode = "002610";
          this.exception = rF.Result.Error;
          goto label_41;
        }
        else
        {
          locateUnits = rF.Result.LocateUnits;
          limit = 0;
        }
      }
      while (locateUnits == null);
      this.btm.setPhaseState(BL.Phase.pvp_disposition);
      this._stopwatch.Start();
      int timeout = locateUnits.locationTimeoutMilliseconds;
      this.timeLimit.value = limit = timeout / 1000;
      while (!this.sendLocateComplited && this._stopwatch.ElapsedMilliseconds < (long) (timeout + 50))
      {
        this.setTimeLimit(limit, this._stopwatch.ElapsedMilliseconds);
        yield return (object) null;
        if (env.core.phaseState.state == BL.Phase.finalize)
          yield break;
      }
      if (!this.sendLocateComplited)
        this.locateUnitsCompleted();
      this.btm.setPhaseState(BL.Phase.pvp_wait_preparing);
      gF = this.peer.ReceiveOps(this.timeoutMilliseconds, AppServerOperation.GameInitialize);
      e = gF.Wait();
      while (e.MoveNext())
      {
        yield return e.Current;
        this.setTimeLimit(limit, this._stopwatch.ElapsedMilliseconds);
        if (env.core.phaseState.state == BL.Phase.finalize)
        {
          IEnumerator ee = this.hideLoading();
          while (ee.MoveNext())
            yield return ee.Current;
          ee = (IEnumerator) null;
          yield break;
        }
      }
      e = (IEnumerator) null;
      if (this.isPvpExceptionLoadingDisp)
      {
        if (Object.op_Implicit((Object) this.selectNode))
          this.selectNode.setActivePvpExceptionLoadingUI(false);
        this.isPvpExceptionLoadingDisp = false;
      }
      e = this.hideLoading();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.bm.popupCloseAll();
      if (gF.Result.Error == null)
      {
        gameInitialize = gF.Result.GameInitialize;
        if (gameInitialize == null)
        {
          rF = (Future<AppPeer.ReceivedMessage>) null;
          gF = (Future<AppPeer.ReceivedMessage>) null;
        }
        else
          goto label_36;
      }
      else
        break;
    }
    this.errorCode = "002611";
    this.exception = gF.Result.Error;
    goto label_41;
label_36:
    foreach (Tuple<int, int, int> unitPosition in gameInitialize.unitPositions)
      this.resetPosition(unitPosition.Item1, unitPosition.Item2, unitPosition.Item3, true);
    this.bm.popupCloseAll();
    this._isDisposition = true;
    this.btm.setPhaseState(BL.Phase.pvp_start_init);
    yield break;
label_41:
    this.btm.setPhaseState(BL.Phase.pvp_exception);
  }

  private void spawnsEffects(List<BL.Unit> spawns, GameObject prefab)
  {
    BE env = this.bm.environment;
    this.bm.battleEffects.skillFieldEffectStartCore(new BattleskillFieldEffect()
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

  private IEnumerator doPvpMain()
  {
    BE env = this.bm.environment;
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    this._isWaitAction = false;
    IEnumerator e = this.initPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    while (Object.op_Equality((Object) this.btm, (Object) null) || this.btm.isRunning)
      yield return (object) new WaitForSeconds(0.1f);
    while (this._isRunning)
    {
      this._stopwatch.Reset();
      Future<AppPeer.ReceivedMessage> rF = this.peer.ReceiveOps(this.timeoutMilliseconds, AppServerOperation.MoveUnitRequest, AppServerOperation.TurnInitialize, AppServerOperation.FinishBattle, AppServerOperation.Recovery, AppServerOperation.WipedOut, AppServerOperation.NoRoom);
      e = rF.Wait();
      while (e.MoveNext())
      {
        if (env.core.phaseState.state == BL.Phase.finalize)
        {
          this.stopPVPMain();
          yield break;
        }
        else
        {
          if (sm.sceneName != this.bm.topScene)
            this.btm.setScheduleAction((Action) (() => sm.backScene()));
          yield return e.Current;
        }
      }
      e = (IEnumerator) null;
      if (this.isSending)
        yield return (object) null;
      e = this.hideLoading();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (rF.Result.Error != null)
      {
        this.errorCode = "002612";
        this.exception = rF.Result.Error;
      }
      else
      {
        int limit = 0;
        AppPeer.MoveUnitRequest moveUnitRequest = rF.Result.MoveUnitRequest;
        AppPeer.TurnInitialize treq = rF.Result.TurnInitialize;
        AppPeer.FinishBattle finishBattle = rF.Result.FinishBattle;
        AppPeer.Recovery recovery1 = rF.Result.Recovery;
        AppPeer.NoRoom noRoom1 = rF.Result.NoRoom;
        AppPeer.WipedOut wipedOut = rF.Result.WipedOut;
        if (moveUnitRequest != null)
        {
          this._stopwatch.Start();
          this.timeLimit.value = limit = moveUnitRequest.moveTimeoutMilliseconds / 1000;
          if (moveUnitRequest.order == this.playerOrder)
            this.btm.setPhaseState(BL.Phase.pvp_player_start);
          else
            this.btm.setPhaseState(BL.Phase.pvp_enemy_start);
          Future<AppPeer.ReceivedMessage> r2F = this.peer.ReceiveOps(this.timeoutMilliseconds + limit * 1000, AppServerOperation.ActionUnit, AppServerOperation.Recovery, AppServerOperation.NoRoom, AppServerOperation.ActionCallSkill);
          e = r2F.Wait();
          while (e.MoveNext())
          {
            if (env.core.phaseState.state == BL.Phase.finalize)
            {
              this.stopPVPMain();
              yield break;
            }
            else
            {
              this.setTimeLimit(limit, this._stopwatch.ElapsedMilliseconds);
              yield return e.Current;
              if (this.timeLimit.value == 0 && env.core.phaseState.state == BL.Phase.player)
              {
                while (sm.changeSceneQueueCount > 0 || !sm.isSceneInitialized)
                  yield return (object) new WaitForSeconds(0.1f);
                if (sm.sceneName != this.bm.topScene)
                  this.btm.setScheduleAction((Action) (() => sm.backScene()));
                this._viewLoading(true);
                this.inputObserver.onCancel(true);
                while (this.btm.isRunning || sm.sceneName != this.bm.topScene || sm.changeSceneQueueCount > 0 || !sm.isSceneInitialized || this.isSending)
                  yield return (object) new WaitForSeconds(0.1f);
                if (env.core.phaseState.state == BL.Phase.player)
                {
                  this.moveUnitTimeout(!(env.core.unitCurrent.unit != (BL.Unit) null) || env.core.unitCurrent.unit.isFacility ? (BL.UnitPosition) null : env.core.currentUnitPosition);
                  do
                  {
                    yield return (object) null;
                  }
                  while (this.isSending);
                }
              }
            }
          }
          e = (IEnumerator) null;
          if (r2F.Result.Error != null)
          {
            this.errorCode = "002613";
            this.exception = r2F.Result.Error;
          }
          else
          {
            if (this.isSending)
              yield return (object) null;
            e = this.hideLoading();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            AppPeer.ActionUnit ureq = r2F.Result.ActionUnit;
            AppPeer.Recovery recovery2 = r2F.Result.Recovery;
            AppPeer.NoRoom noRoom2 = r2F.Result.NoRoom;
            AppPeer.ActionCallSkill creq = r2F.Result.ActionCallSkill;
            if (recovery2 != null)
            {
              this.applyRecovery(recovery2.recovery);
              continue;
            }
            if (noRoom2 != null)
            {
              e = this.noRoomFinish(noRoom2);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              continue;
            }
            if (env.core.phaseState.state == BL.Phase.finalize)
            {
              this.stopPVPMain();
              break;
            }
            this.bm.popupCloseAll();
            this.btm.setScheduleAction((Action) (() =>
            {
              if (env.core.phaseState.state == BL.Phase.finalize)
              {
                this.stopPVPMain();
              }
              else
              {
                if (ureq != null)
                {
                  BL.AIUnit aiUnit = BL.AIUnit.FromNetwork(r2F.Result.ActionUnit.aiUnit, env.core);
                  if (ureq != null)
                  {
                    aiUnit.action = (Action) (() =>
                    {
                      this.playerPoint_reserve = ureq.points[this.playerOrder];
                      this.enemyPoint_reserve = ureq.points[this.playerOrder == 0 ? 1 : 0];
                    });
                    foreach (Tuple<int, int> unitRespawnCount in ureq.deadUnitRespawnCounts)
                      BL.Unit.FromNetwork(new int?(unitRespawnCount.Item1), env.core).pvpRespawnCount = unitRespawnCount.Item2;
                  }
                  this.aiController.enqueueAIActionOrder(aiUnit);
                }
                else
                {
                  bool isPlayer = env.core.phaseState.state != BL.Phase.pvp_enemy_start && env.core.phaseState.state != BL.Phase.enemy;
                  BL.BattleCallSkillResult battleCallSkillResult = BL.BattleCallSkillResult.FromNetwork(creq.battleCallSkillResult, env.core, isPlayer);
                  foreach (Tuple<int, int> unitRespawnCount in creq.deadUnitRespawnCounts)
                    BL.Unit.FromNetwork(new int?(unitRespawnCount.Item1), env.core).pvpRespawnCount = unitRespawnCount.Item2;
                  env.useCallSkill(battleCallSkillResult.skill, battleCallSkillResult.targets, this.btm, isPlayer);
                  this.btm.setScheduleAction((Action) null, 1.5f, (Action) (() =>
                  {
                    this.playerPoint_reserve = creq.points[this.playerOrder];
                    this.enemyPoint_reserve = creq.points[this.playerOrder == 0 ? 1 : 0];
                    env.core.playerCallSkillState.callSkillPoint = creq.callSkillPoints[this.playerOrder];
                    env.core.enemyCallSkillState.callSkillPoint = creq.callSkillPoints[this.playerOrder == 0 ? 1 : 0];
                    this.actionUnitCompleted();
                  }));
                }
                this.bm.isBattleEnable = true;
                this._isWaitAction = true;
              }
            }));
            do
            {
              yield return (object) null;
            }
            while (this.btm.isRunning || this._isWaitAction);
            rF = (Future<AppPeer.ReceivedMessage>) null;
            treq = (AppPeer.TurnInitialize) null;
            continue;
          }
        }
        else
        {
          if (treq != null)
          {
            this.remainTurn.value = treq.remainTurn;
            this.playerPoint_reserve = treq.points[this.playerOrder];
            this.enemyPoint_reserve = treq.points[this.playerOrder == 0 ? 1 : 0];
            env.core.playerCallSkillState.callSkillPoint = treq.callSkillPoints[this.playerOrder];
            env.core.enemyCallSkillState.callSkillPoint = treq.callSkillPoints[this.playerOrder == 0 ? 1 : 0];
            this.random = treq.random;
            foreach (Tuple<int, int> unitRespawnCount in treq.deadUnitRespawnCounts)
              BL.Unit.FromNetwork(new int?(unitRespawnCount.Item1), env.core).pvpRespawnCount = unitRespawnCount.Item2;
            if (treq.respawnUnitPositions.Length != 0)
            {
              do
              {
                yield return (object) null;
              }
              while (this.btm.isRunning);
              List<BL.Unit> spawns1 = new List<BL.Unit>();
              List<BL.Unit> spawns2 = new List<BL.Unit>();
              foreach (Tuple<int, int, int> respawnUnitPosition in treq.respawnUnitPositions)
              {
                BL.UnitPosition up = BL.UnitPosition.FromNetwork(new int?(respawnUnitPosition.Item1), env.core);
                this.resetPosition(up, respawnUnitPosition.Item2, respawnUnitPosition.Item3, true);
                if (up.unit.isPlayerControl)
                  spawns1.Add(up.unit);
                else
                  spawns2.Add(up.unit);
              }
              if (spawns1.Count > 0)
                this.spawnsEffects(spawns1, this.spawnPlayerPrefab);
              if (spawns2.Count > 0)
                this.spawnsEffects(spawns2, this.spawnEnemyPrefab);
              do
              {
                yield return (object) null;
              }
              while (this.btm.isRunning);
            }
            this.btm.setPhaseState(BL.Phase.turn_initialize);
            this._viewLoading(true);
            do
            {
              yield return (object) null;
            }
            while (this.btm.isRunning);
            continue;
          }
          if (finishBattle != null)
          {
            this.finishBattle(finishBattle);
            continue;
          }
          if (recovery1 != null)
          {
            this.applyRecovery(recovery1.recovery);
            continue;
          }
          if (wipedOut != null)
          {
            int index = this.playerOrder == 0 ? 1 : 0;
            this.isPlayerWipedOut.value = wipedOut.isWipedOuts[this.playerOrder];
            this.playerPoint.value = wipedOut.points[this.playerOrder];
            this.playerPoint_reserve = wipedOut.points[this.playerOrder];
            this.isEnemyWipedOut.value = wipedOut.isWipedOuts[index];
            this.enemyPoint.value = wipedOut.points[index];
            this.enemyPoint_reserve = wipedOut.points[index];
            do
            {
              yield return (object) null;
            }
            while (this.btm.isRunning);
            this.wipedOutCompleted();
            continue;
          }
          if (noRoom1 != null)
          {
            e = this.noRoomFinish(noRoom1);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            continue;
          }
          this._viewLoading(true);
          continue;
        }
      }
      this.btm.setPhaseState(BL.Phase.pvp_exception);
      this.bm.popupCloseAll();
      this._isRunning = false;
      this._stopwatch = (Stopwatch) null;
      break;
    }
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

  private void _viewLoading(bool isView)
  {
    if (isView)
      this.isSending = false;
    if (this.isLoading == isView || Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    this.bm.isBattleEnable = true;
    if (isView)
    {
      this.loadingStartTime = DateTime.Now;
      this.bm.popupCloseAll();
      this.bm.popupOpen(this.loadingPrefab, isViewBack: false, isNonSe: true);
    }
    else if (this.isPvpExceptionLoadingDisp)
    {
      if (Object.op_Implicit((Object) this.selectNode))
        this.selectNode.setActivePvpExceptionLoadingUI(false);
      this.isPvpExceptionLoadingDisp = false;
    }
    else
      this.bm.popupDismiss(true);
    this.isLoading = isView;
  }

  private IEnumerator hideLoading()
  {
    if (this.isLoading)
    {
      float num = this.loadingMinTime - (float) (DateTime.Now - this.loadingStartTime).TotalSeconds;
      if ((double) num > 0.0)
        yield return (object) new WaitForSeconds(num);
      this._viewLoading(false);
    }
  }

  private IEnumerator _ReadyCompleted()
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.ReadyCompleted();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002614";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
    {
      this.startMain();
      this.isSending = false;
    }
  }

  private IEnumerator _LocateUnitsCompleted(Tuple<int, int, int>[] unitPositions)
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.LocateUnitsCompleted(unitPositions);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.sendLocateComplited = true;
    if (f.Exception != null)
    {
      this.errorCode = "002615";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
    {
      if (Object.op_Implicit((Object) this.selectNode))
        this.selectNode.setActivePvpExceptionLoadingUI(true);
      this.isPvpExceptionLoadingDisp = true;
    }
  }

  private IEnumerator _ActionUnitCompleted()
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.ActionUnitCompleted();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002620";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
    {
      this._isWaitAction = false;
      this._viewLoading(true);
    }
  }

  private IEnumerator _WipedOutCompleted()
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.WipedOutCompleted();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002621";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
      this._viewLoading(true);
  }

  private IEnumerator _TurnInitializeCompleted()
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.TurnInitializeCompleted();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002616";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
      this._viewLoading(true);
  }

  private IEnumerator _MoveUnitTimeout(int? currentUnitPositionId)
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.MoveUnitTimeout(currentUnitPositionId);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
      this._viewLoading(true);
  }

  private IEnumerator _MoveUnit(int unitPositionId, int row, int column)
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.MoveUnit(unitPositionId, row, column);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002617";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
      this._viewLoading(true);
  }

  private IEnumerator _MoveUnitWithAttack(
    int unitPositionId,
    int row,
    int column,
    int targetUnitPositionId,
    bool isHeal,
    int attackStatusIndex)
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.MoveUnitWithAttack(unitPositionId, row, column, targetUnitPositionId, isHeal, attackStatusIndex);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002618";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
      this._viewLoading(true);
  }

  private IEnumerator _MoveUnitWithSkill(
    int unitPositionId,
    int row,
    int column,
    int[] targetPositionIds,
    int skillId,
    int[] panelRows,
    int[] panelColumns)
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.MoveUnitWithSkill(unitPositionId, row, column, targetPositionIds, skillId, panelRows, panelColumns);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002619";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
      this._viewLoading(true);
  }

  private IEnumerator _AutoOnRequest(bool canCallSkillAuto)
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.AutoOnRequest(canCallSkillAuto);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
  }

  private IEnumerator _UseCallSkill(int[] targetPositionIds, int skillId)
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.UseCallSkill(targetPositionIds, skillId);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002627";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
      this._viewLoading(true);
  }

  public static PVPManager createPVPManager()
  {
    PVPManager pvpManager = Singleton<PVPManager>.GetInstanceOrNull();
    if (Object.op_Equality((Object) pvpManager, (Object) null))
      pvpManager = new GameObject("PVP Manager").AddComponent<PVPManager>();
    return pvpManager;
  }

  public static IEnumerator destroyPVPManager()
  {
    PVPManager pm = Singleton<PVPManager>.GetInstanceOrNull();
    if (!Object.op_Equality((Object) pm, (Object) null))
    {
      IEnumerator e = pm.cleanupPVP();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      PVPMatching component = ((Component) pm).gameObject.GetComponent<PVPMatching>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.cleanupDestroy();
      Object.Destroy((Object) ((Component) pm).gameObject);
      pm.clearInstance();
    }
  }

  private HashSet<BL.Panel> createFormationPanels(int order)
  {
    BE environment = this.bm.environment;
    HashSet<BL.Panel> formationPanels = new HashSet<BL.Panel>();
    foreach (PvpStageFormation pvpStageFormation in ((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).Where<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.stage.ID == this.stage.stage_id)).Where<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.player_order == order)))
      formationPanels.Add(environment.core.getFieldPanel(pvpStageFormation.formation_y - 1, pvpStageFormation.formation_x - 1));
    return formationPanels;
  }

  public void locateUnitsCompleted()
  {
    if (this.exception != null)
      return;
    BE environment = this.bm.environment;
    List<Tuple<int, int, int>> tupleList = new List<Tuple<int, int, int>>();
    foreach (BL.Unit unit in environment.core.playerUnits.value)
    {
      BL.UnitPosition unitPosition = environment.core.getUnitPosition(unit);
      tupleList.Add(new Tuple<int, int, int>(unitPosition.id, unitPosition.row, unitPosition.column));
    }
    this.StartCoroutine(this._LocateUnitsCompleted(tupleList.ToArray()));
  }

  public void turnInitializeCompleted()
  {
    if (this.exception != null)
      return;
    this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
    this.btm.setScheduleAction((Action) (() => this.StartCoroutine(this._TurnInitializeCompleted())));
  }

  public void actionUnitCompleted()
  {
    if (this.exception != null)
      return;
    this.aiController.clearAIActionOrder();
    this.StartCoroutine(this._ActionUnitCompleted());
  }

  public void wipedOutCompleted()
  {
    if (this.exception != null)
      return;
    this.StartCoroutine(this._WipedOutCompleted());
  }

  public void moveUnitTimeout(BL.UnitPosition up)
  {
    if (this.exception != null)
      return;
    this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
    this.StartCoroutine(this._MoveUnitTimeout(up?.id));
  }

  public void moveUnit(BL.UnitPosition up)
  {
    if (this.exception != null)
      return;
    this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
    this.StartCoroutine(this._MoveUnit(up.id, up.row, up.column));
  }

  public void moveUnitWithAttack(
    BL.UnitPosition attack,
    BL.UnitPosition defense,
    bool isHeal,
    int attackStatusIndex)
  {
    if (this.exception != null)
      return;
    this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
    this.StartCoroutine(this._MoveUnitWithAttack(attack.id, attack.row, attack.column, defense.id, isHeal, attackStatusIndex));
  }

  public void moveUnitWithAttack(
    BL.Unit attack,
    BL.Unit defense,
    bool isHeal,
    int attackStatusIndex)
  {
    if (this.exception != null)
      return;
    BE environment = this.bm.environment;
    this.moveUnitWithAttack(environment.core.getUnitPosition(attack), environment.core.getUnitPosition(defense), isHeal, attackStatusIndex);
  }

  public void moveUnitWithSkill(
    BL.UnitPosition up,
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Panel> panels)
  {
    if (this.exception != null)
      return;
    BE environment = this.bm.environment;
    this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
    List<int> intList1 = new List<int>();
    foreach (BL.Unit target in targets)
      intList1.Add(environment.core.getUnitPosition(target).id);
    List<int> intList2 = new List<int>();
    List<int> intList3 = new List<int>();
    foreach (BL.Panel panel in panels)
    {
      intList2.Add(panel.row);
      intList3.Add(panel.column);
    }
    this.StartCoroutine(this._MoveUnitWithSkill(up.id, up.row, up.column, intList1.ToArray(), skill.id, intList2.ToArray(), intList3.ToArray()));
  }

  public void moveUnitWithSkill(
    BL.Unit unit,
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Panel> panels)
  {
    if (this.exception != null)
      return;
    this.moveUnitWithSkill(this.bm.environment.core.getUnitPosition(unit), skill, targets, panels);
  }

  public void readyComplited()
  {
    if (this.exception != null)
      return;
    this.StartCoroutine(this._ReadyCompleted());
  }

  public void autoOnRequest()
  {
    if (this.exception != null)
      return;
    this.StartCoroutine(this._AutoOnRequest(Persist.autoBattleSetting.Data.isCallSkill));
  }

  public void useCallSkill(BL.Skill skill, List<BL.Unit> targets, bool isPlayer)
  {
    if (this.exception != null)
      return;
    BE environment = this.bm.environment;
    this.btm.setPhaseState(BL.Phase.pvp_move_unit_waiting);
    List<int> intList = new List<int>();
    foreach (BL.Unit target in targets)
      intList.Add(environment.core.getUnitPosition(target).id);
    this.StartCoroutine(this._UseCallSkill(intList.ToArray(), skill.id));
  }

  public void errorRecovery()
  {
    this.exception = (Exception) null;
    this.btm.setPhaseState(BL.Phase.none);
    this.StartCoroutine(this.doErrorRecovery());
  }

  private IEnumerator _RecoveryRequest(string bt)
  {
    this.isSending = true;
    Future<AppPeer.SendMessage> f = this.peer.RecoveryRequest(bt);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (f.Exception != null)
    {
      this.errorCode = "002622";
      this.exception = f.Exception;
      this.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
      this._viewLoading(true);
  }

  private IEnumerator doErrorRecovery(bool enableCheck = false)
  {
    PVPManager pvpManager = this;
    if (enableCheck)
    {
      while (!pvpManager.bm.isBattleEnable)
        yield return (object) null;
    }
    while (!pvpManager.bm.initialized)
      yield return (object) null;
    if (pvpManager.peer != null)
      pvpManager.peer.Close();
    Future<AppPeer> cF = pvpManager.connectServer(pvpManager.host, pvpManager.port, (string) null);
    IEnumerator e = cF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (cF.Exception != null)
    {
      pvpManager.errorCode = "002623";
      pvpManager.exception = cF.Exception;
      pvpManager.btm.setPhaseState(BL.Phase.pvp_exception);
    }
    else
    {
      pvpManager.peer = cF.Result;
      if (!pvpManager.isRunning)
      {
        // ISSUE: explicit non-virtual call
        if (!__nonvirtual (pvpManager.isDisposition))
          pvpManager._isDisposition = true;
        pvpManager.StartCoroutine(pvpManager.doRecoveryStartPVPMain());
      }
      else
      {
        e = pvpManager._RecoveryRequest(pvpManager.battleToken);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator doRecoveryStartPVPMain()
  {
    this.startMain();
    IEnumerator e = this._RecoveryRequest(this.battleToken);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void applyRecovery(RecoveryType rt)
  {
    BE environment = this.bm.environment;
    this.remainTurn.value = rt.remainTurn;
    this.playerPoint.value = rt.points[this.playerOrder];
    this.enemyPoint.value = rt.points[this.playerOrder == 0 ? 1 : 0];
    environment.core.playerCallSkillState.callSkillPoint = rt.callSkillPoints[this.playerOrder];
    environment.core.enemyCallSkillState.callSkillPoint = rt.callSkillPoints[this.playerOrder == 0 ? 1 : 0];
    environment.core.playerCallSkillState.isUsedCallSkill = rt.usedCallSkills[this.playerOrder];
    environment.core.enemyCallSkillState.isUsedCallSkill = rt.usedCallSkills[this.playerOrder == 0 ? 1 : 0];
    environment.core.playerCallSkillState.isSomeAction = rt.someActions[this.playerOrder];
    environment.core.enemyCallSkillState.isSomeAction = rt.someActions[this.playerOrder == 0 ? 1 : 0];
    environment.core.phaseState.absoluteTurnCount = rt.absoluteTurnCount;
    environment.core.phaseState.turnCount = rt.turnCount;
    RecoveryUtility.Apply(rt, environment.core);
    foreach (BL.UnitPosition unitPosition in environment.core.unitPositions.value)
    {
      if (unitPosition.unit.isView)
      {
        BattleUnitParts unitParts = environment.unitResource[unitPosition.unit].unitParts_;
        unitParts.moveStayUpdate();
        unitParts.setActive(!unitPosition.unit.isDead);
      }
    }
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private void resetPosition(BL.UnitPosition up, int row, int column, bool resetDirection)
  {
    BE environment = this.bm.environment;
    RecoveryUtility.resetPosition(up, row, column, environment.core);
    if (!resetDirection)
      return;
    PvpStageFormation pvpStageFormation1 = ((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).First<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.stage.ID == this.stage.stage_id && x.player_order == 0));
    PvpStageFormation pvpStageFormation2 = ((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).First<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.stage.ID == this.stage.stage_id && x.player_order != 0));
    up.direction = this.playerOrder != 0 ? (!up.unit.isPlayerControl ? pvpStageFormation1.initial_direction : pvpStageFormation2.initial_direction) : (!up.unit.isPlayerControl ? pvpStageFormation2.initial_direction : pvpStageFormation1.initial_direction);
    if (!up.unit.isView)
      return;
    environment.unitResource[up.unit].unitParts_.moveStayUpdate();
  }

  private void resetPosition(int unitPositionId, int row, int column, bool resetDirection)
  {
    BE environment = this.bm.environment;
    this.resetPosition(BL.UnitPosition.FromNetwork(new int?(unitPositionId), environment.core), row, column, resetDirection);
  }

  public string getErrorMessage(Exception e) => Consts.GetInstance().VERSUS_02694POPUP_DESCRIPTION;

  public void PopupBattleReceiveError(string title, string description, string errorCode = "")
  {
    ModalWindow.ShowYesNo(title, string.Format(description, (object) errorCode), (Action) (() => this.errorRecovery()), (Action) (() => this.btm.setPhaseState(BL.Phase.finalize)));
  }

  private void finishBattle(AppPeer.FinishBattle freq)
  {
    this.bm.battleEffects.startEffect((Transform) null, 3f, popupPrefab: this.winLosePrefab, cloneAction: (Action<GameObject>) (o => o.GetComponent<PopupPvpMatchResult>().setResult(freq, this.playerOrder)));
    this.btm.setPhaseState(BL.Phase.pvp_result);
    this.stopPVPMain();
  }

  private IEnumerator noRoomFinish(AppPeer.NoRoom nreq)
  {
    PVPManager pvpManager = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 2;
    Future<WebAPI.Response.PvpPlayerStatus> futureP = WebAPI.PvpPlayerStatus(new Action<WebAPI.Response.UserError>(pvpManager.errorCallback));
    IEnumerator e = futureP.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    if (futureP.Result != null)
    {
      string title;
      string message;
      if (pvpManager.isResult || futureP.Result.has_battle_result)
      {
        pvpManager.isResult = pvpManager.isResult || futureP.Result.has_battle_result;
        title = Consts.GetInstance().boot_continue_pvp_result_title;
        message = Consts.GetInstance().boot_continue_pvp_result_text;
      }
      else
      {
        title = Consts.GetInstance().PVP_NO_ROOM_FINISH_ERROR_POPUP_TITLE;
        message = Consts.GetInstance().PVP_NO_ROOM_FINISH_ERROR_POPUP_MESSAGE;
      }
      bool close = false;
      ModalWindow.Show(title, message, (Action) (() => close = true));
      while (!close)
        yield return (object) null;
      pvpManager.btm.setPhaseState(BL.Phase.finalize);
      pvpManager.stopPVPMain();
    }
  }

  private void OnApplicationPause(bool pause)
  {
    if (!pause || this.peer == null)
      return;
    this.peer.Close();
  }

  private void errorCallback(WebAPI.Response.UserError error)
  {
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(PopupCommon.Show(error.Code, error.Reason, (Action) (() =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      instance.clearStack();
      instance.destroyCurrentScene();
      instance.changeScene(Singleton<CommonRoot>.GetInstance().startScene, false);
    })));
  }

  private class CreateEnvInfo
  {
    public int order;
    public PlayerUnit[] player1_units;
    public PlayerUnit[] player2_units;
    public Player player1;
    public Player player2;
    public string battle_uuid;
    public MpStage stage;
    public PlayerItem[] player1_items;
    public PlayerItem[] player2_items;
    public PlayerGearReisouSchema[] player1_reisou_items;
    public PlayerGearReisouSchema[] player2_reisou_items;
    public Bonus[] bonus;
    public PlayerCharacterIntimate[] player1_character_intimates;
    public PlayerCharacterIntimate[] player2_character_intimates;
    public PlayerAwakeSkill[] player1_awake_skills;
    public PlayerAwakeSkill[] player2_awake_skills;
    public DateTime battle_start_at;

    public void EncodeReady(AppPeer.Ready r)
    {
      WebAPI.Response.InternalPvpStart internalPvpStart = new WebAPI.Response.InternalPvpStart((Dictionary<string, object>) Json.Deserialize(Encoding.UTF8.GetString(r.buffer)));
      this.order = r.order;
      this.player1_units = ((IEnumerable<PlayerUnit>) internalPvpStart.player1_units).Take<PlayerUnit>(5).ToArray<PlayerUnit>();
      this.attachOverkillers(this.player1_units, internalPvpStart.player1_units_over_killers);
      this.player2_units = ((IEnumerable<PlayerUnit>) internalPvpStart.player2_units).Take<PlayerUnit>(5).ToArray<PlayerUnit>();
      this.attachOverkillers(this.player2_units, internalPvpStart.player2_units_over_killers);
      this.player1 = internalPvpStart.player1;
      this.player2 = internalPvpStart.player2;
      this.battle_uuid = internalPvpStart.battle_uuid;
      this.stage = internalPvpStart.stage;
      this.player1_items = internalPvpStart.player1_items;
      this.player2_items = internalPvpStart.player2_items;
      this.player1_reisou_items = internalPvpStart.player1_reisou_items;
      this.player2_reisou_items = internalPvpStart.player2_reisou_items;
      this.bonus = internalPvpStart.bonus;
      this.player1_character_intimates = internalPvpStart.player1_character_intimates;
      this.player2_character_intimates = internalPvpStart.player2_character_intimates;
      this.player1_awake_skills = internalPvpStart.player1_awake_skills;
      this.player2_awake_skills = internalPvpStart.player2_awake_skills;
      this.battle_start_at = internalPvpStart.battle_start_at;
    }

    public void EncodeResume(WebAPI.Response.PvpResume r)
    {
      this.order = r.order;
      this.player1_units = ((IEnumerable<PlayerUnit>) r.player1_units).Take<PlayerUnit>(5).ToArray<PlayerUnit>();
      this.attachOverkillers(this.player1_units, r.player1_units_over_killers);
      this.player2_units = ((IEnumerable<PlayerUnit>) r.player2_units).Take<PlayerUnit>(5).ToArray<PlayerUnit>();
      this.attachOverkillers(this.player2_units, r.player2_units_over_killers);
      this.player1 = r.player1;
      this.player2 = r.player2;
      this.battle_uuid = r.battle_uuid;
      this.stage = r.stage;
      this.player1_items = r.player1_items;
      this.player2_items = r.player2_items;
      this.player1_reisou_items = r.player1_reisou_items;
      this.player2_reisou_items = r.player2_reisou_items;
      this.bonus = r.bonus;
      this.player1_character_intimates = r.player1_character_intimates;
      this.player2_character_intimates = r.player2_character_intimates;
      this.player1_awake_skills = r.player1_awake_skills;
      this.player2_awake_skills = r.player2_awake_skills;
      this.battle_start_at = r.battle_start_at;
    }

    private void attachOverkillers(PlayerUnit[] playerUnits, PlayerUnit[] overkillers)
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
  }
}
