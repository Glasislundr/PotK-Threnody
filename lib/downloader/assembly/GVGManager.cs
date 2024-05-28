// Decompiled with JetBrains decompiler
// Type: GVGManager
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
public class GVGManager : Singleton<GVGManager>, IGameEngine
{
  private string finishPrefabPath = "Prefabs/battle/dir_PvpResults";
  private string spawnPlayerPrefabPath = "BattleEffects/field/ef657_fe_Multi_Unit_Sporn";
  private string spawnEnemyPrefabPath = "BattleEffects/field/ef658_fe_Multi_Enemy_Sporn";
  private GameObject finishPrefab;
  private GameObject spawnPlayerPrefab;
  private GameObject spawnEnemyPrefab;
  private NGBattleManager bm;
  private BattleTimeManager _btm;
  private BattleAIController _aiController;
  private BattleInputObserver _inputObserver;
  private bool _isRunning;
  private bool _isWaitAction;
  private bool _isDisposition;
  public bool isResult;
  private Stopwatch _stopwatch;
  private BL.StructValue<int> _remainTurn;
  private BL.StructValue<int> _timeLimit;
  private BL.StructValue<int> _playerPoint;
  private BL.StructValue<int> _enemyPoint;
  private BL.StructValue<bool> _isPlayerWipedOut;
  private BL.StructValue<bool> _isEnemyWipedOut;
  private HashSet<BL.Panel> _formationPanel;
  private HashSet<BL.Panel> _formationPanelEnemy;
  private int _starNum;
  private string _victory_condition = string.Empty;
  private string _victory_sub_condition = string.Empty;
  private BL.Phase saveState = BL.Phase.none;
  private Dictionary<BattleMonoBehaviour, bool> nextStateFlags = new Dictionary<BattleMonoBehaviour, bool>();
  private List<BattleMonoBehaviour> nextStateFlagsKeys = new List<BattleMonoBehaviour>();
  private Action nextStateWaitAction;
  private int currentLimit;
  private bool actionCompleted;

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

  public BL.StructValue<int> remainTurn => this._remainTurn;

  public BL.StructValue<int> timeLimit => this._timeLimit;

  public BL.StructValue<int> playerPoint => this._playerPoint;

  public BL.StructValue<int> enemyPoint => this._enemyPoint;

  public BL.StructValue<bool> isPlayerWipedOut => this._isPlayerWipedOut;

  public BL.StructValue<bool> isEnemyWipedOut => this._isEnemyWipedOut;

  public string playerGuildName => this.setting.myGuildName;

  public string playerName => SMManager.Observe<Player>().Value.name;

  public int playerEmblem => SMManager.Observe<Player>().Value.current_emblem_id;

  public string enemyGuildName => this.setting.enemyGuildname;

  public string enemyName => this.setting.enemyPlayerName;

  public int enemyEmblem => this.setting.enemyEmblemID;

  public int enemyContribution => this.setting.enemyContribution;

  public int enemyLevel => this.setting.enemyLevel;

  public int enemyGuildPosition => this.setting.enemyGuildPosition;

  public int enemyKeepStar => this.setting.enemyKeepStar;

  public int enemyTownLevel => this.setting.enemyTownLevel;

  private int playerPoint_reserve
  {
    get => this.bm.environment.core.playerPoint;
    set => this.bm.environment.core.playerPoint = value;
  }

  private int enemyPoint_reserve
  {
    get => this.bm.environment.core.enemyPoint;
    set => this.bm.environment.core.enemyPoint = value;
  }

  public int playerAnnihilationCount => this.bm.environment.core.playerAnnihilationCount;

  public int enemyAnnihilationCount => this.bm.environment.core.enemyAnnihilationCount;

  public PvpMatchingTypeEnum matchingType
  {
    get => Persist.pvpSuspend.Data.matchingType;
    set => Persist.pvpSuspend.Data.matchingType = value;
  }

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

  public int starNum => this._starNum;

  public string victory_condition
  {
    get
    {
      return Consts.Format(Consts.GetInstance().GUILD_BATTLE_VICTORY_CONDITION, (IDictionary) new Hashtable()
      {
        {
          (object) "cnt",
          (object) this.bm.battleInfo.gvgSetting.turns.ToLocalizeNumberText()
        }
      });
    }
  }

  public string victory_sub_condition
  {
    get
    {
      return Consts.Format(Consts.GetInstance().GUILD_BATTLE_SUB_CONDITION1, (IDictionary) new Hashtable()
      {
        {
          (object) "cnt",
          (object) this.bm.battleInfo.gvgSetting.turns.ToLocalizeNumberText()
        }
      });
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
  }

  public int playerOrder => this.bm.order;

  public void deadReserveToPoint(bool isEnemy, bool checkAnnihilation)
  {
    if (this.bm.environment.core.phaseState.state == BL.Phase.finalize)
      return;
    if (isEnemy)
    {
      if (this.playerPoint.value != this.playerPoint_reserve)
        this.playerPoint.value = this.bm.environment.core.playerPointView = this.playerPoint_reserve;
    }
    else if (this.enemyPoint.value != this.enemyPoint_reserve)
      this.enemyPoint.value = this.bm.environment.core.enemyPointView = this.enemyPoint_reserve;
    this.clearNextStateFlags();
    this.btm.setEnableWait(0.2f);
    if (!checkAnnihilation)
      return;
    BL core = this.bm.environment.core;
    if (this.checkAnnihilationp((IEnumerable<BL.Unit>) core.playerUnits.value) && !isEnemy)
    {
      this.btm.setScheduleAction((Action) (() =>
      {
        this.isEnemyWipedOut.value = true;
        this.enemyPoint.value = this.bm.environment.core.enemyPointView = (this.enemyPoint_reserve += this.setting.annihilation_point);
        ++this.bm.environment.core.playerAnnihilationCount;
      }));
      this.btm.setEnableWait(0.2f);
    }
    if (!(this.checkAnnihilationp((IEnumerable<BL.Unit>) core.enemyUnits.value) & isEnemy))
      return;
    this.btm.setScheduleAction((Action) (() =>
    {
      this.isPlayerWipedOut.value = true;
      this.playerPoint.value = this.bm.environment.core.playerPointView = (this.playerPoint_reserve += this.setting.annihilation_point);
      ++this.bm.environment.core.enemyAnnihilationCount;
    }));
    this.btm.setEnableWait(0.2f);
  }

  public IEnumerator cleanupGVG()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    GVGManager gvgManager = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    gvgManager._stopwatch = (Stopwatch) null;
    gvgManager.StopCoroutine("doGvgMain");
    return false;
  }

  public Future<None> startGVG(BattleInfo battleInfo, bool isRestart)
  {
    return new Future<None>((Func<Promise<None>, IEnumerator>) (promise => this._startGVG(promise, battleInfo, isRestart)));
  }

  private IEnumerator _startGVG(Promise<None> promise, BattleInfo battleInfo, bool isRestart)
  {
    this.bm.order = 0;
    IEnumerator e;
    if (!isRestart)
    {
      e = this._battleReady(promise, battleInfo);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      if (this.bm.hasSavedEnvironment())
      {
        bool flag;
        try
        {
          this.bm.loadEnvironment();
          this.bm.battleInfo = this.bm.environment.core.battleInfo;
          BattleFuncs.environment.Reset(this.bm.environment.core);
          flag = true;
        }
        catch (Exception ex)
        {
          Debug.LogError((object) "loadEnvironment() failed: {0}, {1}\n{2}\n{3}".F((object) ex, (object) ex.Message, (object) ex.Source, (object) ex.StackTrace));
          this.bm.deleteSavedEnvironment();
          flag = false;
        }
        if (flag)
        {
          e = this.bm.initMasterData(this.bm.battleInfo);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      if (!this.bm.hasSavedEnvironment())
      {
        e = this.closeGvg();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        promise.Exception = new Exception("restarting gvg was failed!");
        yield break;
      }
      else if (this.bm.environment.core.phaseState.state != BL.Phase.battle_start)
      {
        this.saveState = this.bm.environment.core.phaseState.state;
        this.bm.environment.core.phaseState.setStateWith(BL.Phase.pvp_restart, this.bm.environment.core, (Action) (() => { }));
      }
    }
    e = this.initPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.playerPoint.value = this.bm.environment.core.playerPointView;
    this.enemyPoint.value = this.bm.environment.core.enemyPointView;
    this.remainTurn.value = this.endTurn - this.bm.environment.core.phaseState.turnCount;
    this.startGVGMain();
    promise.Result = None.Value;
  }

  private IEnumerator _battleReady(Promise<None> promise, BattleInfo battleInfo)
  {
    IEnumerator e = this.createEnvironment(battleInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    promise.Result = None.Value;
  }

  private IEnumerator createEnvironment(BattleInfo battleInfo)
  {
    BE env = new BE();
    env.core = (BL) new Core(env);
    env.core.battleInfo = battleInfo;
    foreach (PlayerUnit pvpPlayerUnit in battleInfo.pvp_player_units)
      pvpPlayerUnit.is_enemy = false;
    foreach (PlayerUnit gvgPlayerHelper in battleInfo.gvg_player_helpers)
      gvgPlayerHelper.is_enemy = false;
    foreach (PlayerUnit pvpEnemyUnit in battleInfo.pvp_enemy_units)
      pvpEnemyUnit.is_enemy = true;
    foreach (PlayerUnit gvgEnemyHelper in battleInfo.gvg_enemy_helpers)
      gvgEnemyHelper.is_enemy = true;
    IEnumerator e = this.bm.initMasterData(battleInfo);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    BattleFuncs.environment.Reset(env.core);
    e = new BattleLogicInitializer().doStart(battleInfo, env.core);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.bm.environment = env;
    this.bm.battleInfo = battleInfo;
    this.bm.saveEnvironment(true);
  }

  private IEnumerator initPrefabs()
  {
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    Future<GameObject> tmpF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.finishPrefab, (Object) null))
    {
      tmpF = rm.Load<GameObject>(this.finishPrefabPath);
      e = tmpF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.finishPrefab = tmpF.Result;
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

  public static GVGManager createGVGManager()
  {
    GVGManager gvgManager = Singleton<GVGManager>.GetInstanceOrNull();
    if (Object.op_Equality((Object) gvgManager, (Object) null))
      gvgManager = new GameObject("GVG Manager").AddComponent<GVGManager>();
    return gvgManager;
  }

  public static IEnumerator destroyGVGManager()
  {
    GVGManager pm = Singleton<GVGManager>.GetInstanceOrNull();
    if (!Object.op_Equality((Object) pm, (Object) null))
    {
      IEnumerator e = pm.cleanupGVG();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Object.Destroy((Object) ((Component) pm).gameObject);
      pm.clearInstance();
    }
  }

  private HashSet<BL.Panel> createFormationPanels(int order)
  {
    BL ec = this.bm.environment.core;
    HashSet<BL.Panel> formationPanels = new HashSet<BL.Panel>();
    foreach (GvgStageFormation gvgStageFormation in ((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage.ID == ec.stage.id)).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.player_order == order)))
      formationPanels.Add(ec.getFieldPanel(gvgStageFormation.formation_y - 1, gvgStageFormation.formation_x - 1));
    return formationPanels;
  }

  private void startGVGMain()
  {
    this._stopwatch = new Stopwatch();
    this.StartCoroutine("doGvgMain");
  }

  public void startMain() => this.turnInitialize();

  private void timeCount()
  {
    if (this._stopwatch == null || !this._stopwatch.IsRunning || this._isWaitAction)
      return;
    this.setTimeLimit(this.currentLimit, this._stopwatch.ElapsedMilliseconds);
  }

  private bool timeLimitp()
  {
    return this._stopwatch != null && this._stopwatch.IsRunning && this._timeLimit.value == 0;
  }

  private bool moveingUnitp(BE env)
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
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    BL ec = this.bm.environment.core;
    this._stopwatch.Reset();
    BL.UnitPosition cup = ec.currentUnitPosition;
    if (!this.actionCompleted)
    {
      while (this.btm.isRunning || sm.changeSceneQueueCount > 0 || !sm.isSceneInitialized)
        yield return (object) new WaitForSeconds(0.1f);
      if (sm.sceneName != this.bm.topScene)
        this.btm.backSceneWithReturnWait();
      this.bm.popupCloseAll();
      if (cup.cantChangeCurrent)
        cup.cancelMove(this.bm.environment);
      else
        this.bm.environment.setCurrentUnit_((BL.Unit) null);
      this.inputObserver.onCancel(true);
      while (this.btm.isRunning || sm.sceneName != this.bm.topScene || sm.changeSceneQueueCount > 0 || !sm.isSceneInitialized)
        yield return (object) new WaitForSeconds(0.1f);
      while (this.moveingUnitp(this.bm.environment))
        yield return (object) null;
      if (cup.cantChangeCurrent)
        this.moveUnit(cup);
      else
        this.moveUnit(ec.playerActionUnits.value[0]);
    }
  }

  private IEnumerator doGvgMain()
  {
    GVGManager gvgManager = this;
    while (!gvgManager.bm.initialized)
      yield return (object) null;
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    BL ec = gvgManager.bm.environment.core;
    while (true)
    {
      IEnumerator e;
      while ((Object.op_Equality((Object) gvgManager.btm, (Object) null) || gvgManager.btm.isRunning || sm.changeSceneQueueCount > 0 || !sm.isSceneInitialized || !gvgManager.bm.isBattleEnable || gvgManager._isWaitAction) && (gvgManager.bm.isBattleEnable || ec.phaseState.state != BL.Phase.pvp_disposition))
      {
        gvgManager.timeCount();
        if (ec.phaseState.state == BL.Phase.player && gvgManager.timeLimitp())
        {
          e = gvgManager.timeoutExec();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        }
        yield return (object) new WaitForSeconds(0.1f);
      }
      gvgManager.timeCount();
      if (gvgManager.nextStateWaitAction != null && gvgManager.checkNextStateFlags())
      {
        gvgManager.nextStateWaitAction();
        gvgManager.nextStateWaitAction = (Action) null;
      }
      else
      {
        switch (ec.phaseState.state)
        {
          case BL.Phase.player:
            if (gvgManager.timeLimitp())
            {
              e = gvgManager.timeoutExec();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            }
            break;
          case BL.Phase.pvp_disposition:
            if (gvgManager.timeLimitp())
            {
              gvgManager._stopwatch.Reset();
              gvgManager.bm.popupCloseAll();
              // ISSUE: explicit non-virtual call
              __nonvirtual (gvgManager.locateUnitsCompleted());
              break;
            }
            break;
          case BL.Phase.pvp_restart:
            // ISSUE: reference to a compiler-generated method
            gvgManager.btm.setScheduleAction(new Action(gvgManager.\u003CdoGvgMain\u003Eb__116_0));
            switch (gvgManager.saveState)
            {
              case BL.Phase.player:
              case BL.Phase.enemy:
                gvgManager.nextState(gvgManager.saveState);
                break;
              default:
                gvgManager.btm.setPhaseState(gvgManager.saveState);
                break;
            }
            break;
        }
      }
      yield return (object) new WaitForSeconds(0.1f);
    }
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

  private bool checkAnnihilationp(IEnumerable<BL.Unit> units)
  {
    foreach (BL.Unit unit in units)
    {
      if (unit.isEnable && !unit.isDead)
        return false;
    }
    return true;
  }

  private void turnInitialize()
  {
    BL core1 = this.bm.environment.core;
    this.remainTurn.value = this.endTurn - core1.phaseState.turnCount;
    if (this.isGameFinish())
    {
      this.finishBattle();
    }
    else
    {
      bool flag1 = this.checkAnnihilationp((IEnumerable<BL.Unit>) core1.playerUnits.value);
      bool flag2 = this.checkAnnihilationp((IEnumerable<BL.Unit>) core1.enemyUnits.value);
      foreach (BL.UnitPosition unitPosition in core1.unitPositions.value)
      {
        if (unitPosition.unit.isDead)
        {
          if (flag1 && core1.playerUnits.value.Contains(unitPosition.unit))
            unitPosition.unit.pvpRespawnCount = 0;
          else if (flag2 && core1.enemyUnits.value.Contains(unitPosition.unit))
            unitPosition.unit.pvpRespawnCount = 0;
          else
            --unitPosition.unit.pvpRespawnCount;
        }
      }
      List<BL.Unit> spawns1 = new List<BL.Unit>();
      List<BL.Unit> spawns2 = new List<BL.Unit>();
      foreach (BL.UnitPosition up in core1.unitPositions.value)
      {
        if (up.unit.isDead && !up.unit.isFacility && up.unit.pvpRespawnCount == 0)
        {
          HashSet<BL.Panel> self = !up.unit.isPlayerControl ? this.formationPanelEnemy : this.formationPanel;
          BL core2 = this.bm.environment.core;
          foreach (BL.Panel panel in self.Shuffle<BL.Panel>())
          {
            if (core1.isMoveOKPanel(panel, up.unit, false, up.moveCost) && core1.getFieldUnit(panel, includeJumping: true) == null)
            {
              this.resetPosition(up, panel.row, panel.column, true);
              break;
            }
          }
          if (up.unit.isPlayerControl)
            spawns1.Add(up.unit);
          else
            spawns2.Add(up.unit);
        }
      }
      if (spawns1.Count > 0)
        this.spawnsEffects(spawns1, this.spawnPlayerPrefab);
      if (spawns2.Count > 0)
        this.spawnsEffects(spawns2, this.spawnEnemyPrefab);
      this.btm.setPhaseState(BL.Phase.turn_initialize);
    }
  }

  private bool checkCharmOnly(List<BL.UnitPosition> l)
  {
    foreach (BL.UnitPosition unitPosition in l)
    {
      if (!unitPosition.unit.IsCharm)
        return false;
    }
    return true;
  }

  private bool setNextPhase(List<BL.UnitPosition> units, BL.Phase state, BL.Phase cantChangeState)
  {
    if (this.bm.environment.core.currentUnitPosition.cantChangeCurrent)
    {
      this.btm.setPhaseState(cantChangeState);
      if (cantChangeState == BL.Phase.pvp_player_start)
      {
        this.resetTimeLimit(this.setting.timeLimit);
        this._isWaitAction = false;
      }
      else
        this._isWaitAction = true;
      return true;
    }
    if (units.Count > 0)
    {
      this.btm.setPhaseState(state);
      switch (state)
      {
        case BL.Phase.pvp_player_start:
          if (this.checkCharmOnly(this.bm.environment.core.playerActionUnits.value))
          {
            this._isWaitAction = true;
            break;
          }
          this.resetTimeLimit(this.setting.timeLimit);
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
    this.bm.environment.core.phaseState.setStateWith(BL.Phase.none, this.bm.environment.core, (Action) (() => { }));
    this.nextStateWaitAction = (Action) (() =>
    {
      BL core = this.bm.environment.core;
      if (this.isGameFinish())
        this.finishBattle();
      else if (state == BL.Phase.enemy || state == BL.Phase.turn_initialize)
      {
        if (this.setNextPhase(core.playerActionUnits.value, BL.Phase.pvp_player_start, BL.Phase.pvp_enemy_start) || this.setNextPhase(core.enemyActionUnits.value, BL.Phase.pvp_enemy_start, BL.Phase.pvp_enemy_start))
          return;
        this.turnInitialize();
      }
      else
      {
        if (state != BL.Phase.player && state != BL.Phase.pvp_move_unit_waiting || this.setNextPhase(core.enemyActionUnits.value, BL.Phase.pvp_enemy_start, BL.Phase.pvp_player_start) || this.setNextPhase(core.playerActionUnits.value, BL.Phase.pvp_player_start, BL.Phase.pvp_player_start))
          return;
        this.turnInitialize();
      }
    });
    this.clearNextStateFlags();
  }

  public void locateUnitsCompleted()
  {
    this._stopwatch.Reset();
    BL core = this.bm.environment.core;
    foreach (BL.Unit unit in core.playerUnits.value)
      core.getUnitPosition(unit).completeActionUnit(core);
    this._isDisposition = true;
    this.btm.setPhaseState(BL.Phase.pvp_start_init);
  }

  private void resetTimeLimit(int limit)
  {
    this.currentLimit = limit;
    this.actionCompleted = false;
    this.btm.setScheduleAction((Action) (() =>
    {
      this._stopwatch.Reset();
      this._stopwatch.Start();
      this.setTimeLimit(limit, this._stopwatch.ElapsedMilliseconds);
    }));
  }

  public void turnInitializeCompleted() => this.nextState(BL.Phase.turn_initialize);

  public void actionUnitCompleted()
  {
    BL core = this.bm.environment.core;
    this.aiController.clearAIActionOrder();
    this._isWaitAction = false;
    this.nextState(core.phaseState.state);
  }

  public void wipedOutCompleted()
  {
  }

  public void moveUnit(BL.UnitPosition up)
  {
    BL ec = this.bm.environment.core;
    if (ec.phaseState.state == BL.Phase.none)
      return;
    up.completeActionUnit(ec, true);
    this.actionCompleted = true;
    this.bm.environment.core.setSomeAction();
    this.btm.setScheduleAction((Action) (() => this.nextState(ec.phaseState.state)));
  }

  public void moveUnitWithAttack(
    BL.UnitPosition attack,
    BL.UnitPosition defense,
    bool isHeal,
    int attackStatusIndex)
  {
    this._isWaitAction = true;
    this.actionCompleted = true;
    this.bm.environment.core.setSomeAction();
    BE environment = this.bm.environment;
    BL ec = environment.core;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    AttackStatus attackStatus = BattleFuncs.getAttackStatusArray(attack, defense, true, isHeal)[attackStatusIndex];
    if (isHeal)
    {
      if (instance.sceneName != this.bm.topScene)
        this.btm.backSceneWithReturnWait();
      BE be = environment;
      BL.MagicBullet magicBullet = attackStatus.magicBullet;
      int attack1 = attackStatus.healAttack((BL.ISkillEffectListUnit) attack.unit, (BL.ISkillEffectListUnit) defense.unit);
      BL.Unit unit = attack.unit;
      List<BL.Unit> targets = new List<BL.Unit>();
      targets.Add(defense.unit);
      BattleTimeManager btm = this.btm;
      be.useMagicBullet(magicBullet, attack1, unit, targets, btm);
    }
    else
      this.bm.startDuel(BattleFuncs.calcDuel(attackStatus, attack, defense));
    this.btm.setScheduleAction((Action) (() =>
    {
      this._isWaitAction = false;
      this.nextState(ec.phaseState.state);
    }));
  }

  public void moveUnitWithAttack(
    BL.Unit attack,
    BL.Unit defense,
    bool isHeal,
    int attackStatusIndex)
  {
    BL core = this.bm.environment.core;
    this.moveUnitWithAttack(core.getUnitPosition(attack), core.getUnitPosition(defense), isHeal, attackStatusIndex);
  }

  public void moveUnitWithSkill(
    BL.Unit unit,
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Panel> panels)
  {
    this.actionCompleted = true;
    this.bm.environment.core.setSomeAction();
    BL ec = this.bm.environment.core;
    this.bm.environment.useSkill(unit, skill, targets, panels, (BL.BattleSkillResult) null, this.btm);
    this.btm.setScheduleAction((Action) (() => this.nextState(ec.phaseState.state)));
  }

  public void useCallSkill(BL.Skill skill, List<BL.Unit> targets, bool isPlayer)
  {
    if (isPlayer)
      this._stopwatch.Stop();
    this.bm.environment.useCallSkill(skill, targets, this.btm, isPlayer);
    if (!isPlayer)
      return;
    this.btm.setScheduleAction((Action) (() =>
    {
      this.currentLimit = this.setting.timeLimit;
      this._stopwatch.Reset();
      this._stopwatch.Start();
      this.setTimeLimit(this.setting.timeLimit, this._stopwatch.ElapsedMilliseconds);
    }));
  }

  public void readyComplited()
  {
    this.btm.setPhaseState(BL.Phase.pvp_disposition);
    this.resetTimeLimit(this.setting.timeLimit);
  }

  private void resetPosition(BL.UnitPosition up, int row, int column, bool resetDirection)
  {
    BE environment = this.bm.environment;
    RecoveryUtility.resetPosition(up, row, column, environment.core);
    if (!resetDirection)
      return;
    up.direction = this.playerOrder != 0 ? (up.unit.isPlayerControl ? 180f : 0.0f) : (up.unit.isPlayerControl ? 0.0f : 180f);
    if (!up.unit.isView)
      return;
    environment.unitResource[up.unit].unitParts_.moveStayUpdate();
  }

  private void finishBattle()
  {
    this.bm.battleEffects.startEffect((Transform) null, 3f, popupPrefab: this.finishPrefab, cloneAction: (Action<GameObject>) (o => o.GetComponent<PopupPvpMatchResult>().setResult(this.getFinishBattle(), this.playerOrder)));
    this.btm.setPhaseState(BL.Phase.pvp_result);
  }

  private void apllyUnitData(BL.Unit deadUnit, BL.Unit killUnit)
  {
    BL core = this.bm.environment.core;
    int point = this.calcPoint(deadUnit);
    int num = BattleFuncs.useOvoPointInterference(deadUnit, point);
    BL.Unit unit = deadUnit;
    if (core.getForceID(unit) == BL.ForceID.enemy)
      this.playerPoint_reserve += num;
    else
      this.enemyPoint_reserve += num;
    if (killUnit != (BL.Unit) null)
      killUnit.pvpPoint += num;
    deadUnit.pvpRespawnCount = this.calcRespawnCount(deadUnit);
  }

  public void applyDeadUnit(BL.Unit attack, BL.Unit defense)
  {
    if (attack.hp <= 0 && !BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) attack).Any<BL.SkillEffect>())
      this.apllyUnitData(attack, defense);
    if (!(defense != (BL.Unit) null) || defense.hp > 0 || BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) defense).Any<BL.SkillEffect>())
      return;
    this.apllyUnitData(defense, attack);
  }

  private bool deadUnitAndFinish(BL.Unit u)
  {
    if (u.hp > 0 || BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) u).Any<BL.SkillEffect>())
      return false;
    BL core = this.bm.environment.core;
    int point = this.calcPoint(u);
    int num = BattleFuncs.useOvoPointInterference(u, point, true);
    BL.Unit unit = u;
    return core.getForceID(unit) == BL.ForceID.enemy ? this.playerPoint_reserve + num >= this.endPoint : this.enemyPoint_reserve + num >= this.endPoint;
  }

  public bool checkDuelDeadUnitAndFinish(BL.Unit attack, BL.Unit defense)
  {
    return this.deadUnitAndFinish(attack) || this.deadUnitAndFinish(defense);
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

  public int endTurn => (int) ((double) this.setting.turns * (double) this.setting.turns_factor);

  public int endPoint => this.setting.point;

  private GVGSetting setting => this.bm.battleInfo.gvgSetting;

  private int calcPoint(BL.Unit unit)
  {
    return !unit.isFacility ? (int) ((double) this.setting.point_base_factor + ((double) (unit.unit.rarity.index + 1) * (double) this.setting.point_rarity_factor + (double) unit.playerUnit.cost * (double) this.setting.point_cost_factor) * (unit.is_leader ? (double) this.setting.point_leader_factor : (double) this.setting.point_no_leader_factor)) : 0;
  }

  private int calcRespawnCount(BL.Unit unit)
  {
    return (int) ((double) this.setting.respawn_base_factor + ((double) (unit.unit.rarity.index + 1) * (double) this.setting.respawn_rarity_factor + (double) unit.playerUnit.cost * (double) this.setting.respawn_cost_factor));
  }

  public bool isGameFinish()
  {
    BL core = this.bm.environment.core;
    int num = this.playerPoint_reserve >= this.endPoint || this.enemyPoint_reserve >= this.endPoint ? 1 : (this.remainTurn.value == 0 ? 1 : 0);
    if (num == 0)
      return num != 0;
    core.isWin = this.playerPoint_reserve > this.enemyPoint_reserve;
    return num != 0;
  }

  private AppPeer.FinishBattle getFinishBattle()
  {
    AppPeer.FinishBattle finishBattle = new AppPeer.FinishBattle();
    this.getVictoryEffectEnum(finishBattle);
    return finishBattle;
  }

  private void getVictoryEffectEnum(AppPeer.FinishBattle finishBattle)
  {
    int num = this.getStarNum();
    if (finishBattle != null)
    {
      finishBattle.victoryEffects = new PvpVictoryEffectEnum[1];
      switch (num)
      {
        case 0:
          finishBattle.victoryEffects[0] = this.playerPoint.value == this.enemyPoint.value || this.playerPoint.value >= this.endPoint && this.enemyPoint.value >= this.endPoint ? PvpVictoryEffectEnum.draw_effect : PvpVictoryEffectEnum.lose_effect;
          break;
        case 1:
          finishBattle.victoryEffects[0] = PvpVictoryEffectEnum.win_effect;
          break;
        case 2:
          finishBattle.victoryEffects[0] = PvpVictoryEffectEnum.great_effect;
          break;
        case 3:
          finishBattle.victoryEffects[0] = PvpVictoryEffectEnum.excellent_effect;
          break;
        default:
          finishBattle.victoryEffects[0] = PvpVictoryEffectEnum.lose_effect;
          break;
      }
    }
    if (this.setting.enemyKeepStar < num)
      num = this.setting.enemyKeepStar;
    this._starNum = num;
  }

  private int getStarNum()
  {
    if (!this.bm.environment.core.isWin)
      return 0;
    bool flag = this.bm.environment.core.playerUnits.value.Sum<BL.Unit>((Func<BL.Unit, int>) (x => x.deadCountExceptImmediateRebirth)) <= 0;
    int num1 = Mathf.CeilToInt((float) (this.endPoint - this.enemyPoint.value) * 100f / (float) this.endPoint);
    int num2 = Mathf.CeilToInt((float) (this.endPoint - this.playerPoint.value) * 100f / (float) this.endPoint);
    int num3 = this.endTurn - this.bm.environment.core.phaseState.turnCount;
    foreach (GvgStarCondition gvgStarCondition in ((IEnumerable<GvgStarCondition>) MasterData.GvgStarConditionList).OrderBy<GvgStarCondition, int>((Func<GvgStarCondition, int>) (x => x.ID)).ToList<GvgStarCondition>())
    {
      if ((gvgStarCondition.breakaway_condition != GvgBreakawayCondition.no_breakaway || flag) && !(gvgStarCondition.breakaway_condition == GvgBreakawayCondition.breakaway & flag) && (!gvgStarCondition.player_gauge_condition || num1 >= gvgStarCondition.player_gauge_value) && (!gvgStarCondition.enemy_gauge_condition || num2 <= gvgStarCondition.enemy_gauge_value) && (!gvgStarCondition.remain_turn_condition || num3 >= gvgStarCondition.remain_turn_value))
        return gvgStarCondition.star_num;
    }
    return 1;
  }

  private IEnumerator closeGvg()
  {
    bool end = false;
    if (!string.IsNullOrEmpty(GuildUtil.gvgBattleIDServer))
    {
      IEnumerator e = this.forceClose((Action) (() => ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_MESSAGE, (Action) (() => end = true))));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_MESSAGE, (Action) (() => end = true));
    while (!end)
      yield return (object) null;
  }

  private IEnumerator forceClose(Action action)
  {
    Singleton<NGSceneManager>.GetInstance();
    bool maintenance = false;
    IEnumerator e1 = WebAPI.GvgBattleForceClose(GuildUtil.gvgBattleIDServer, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (!e.Code.Equals("GLD014"))
        return;
      maintenance = true;
      WebAPI.DefaultUserErrorCallback(e);
    })).Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    GuildUtil.gvgBattleIDServer = string.Empty;
    if (!maintenance && action != null)
      action();
  }

  public void autoOnRequest()
  {
  }
}
