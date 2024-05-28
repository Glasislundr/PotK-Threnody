// Decompiled with JetBrains decompiler
// Type: BattleTimeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattleTimeManager : BattleManagerBase
{
  private Queue<Schedule> sQueue;
  private BE environment;
  private bool _isRunning;
  private bool _insertMode;
  private Queue<Schedule> _insertQueue;

  public override IEnumerator initialize(BattleInfo battleInfo, BE env_ = null)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleTimeManager battleTimeManager = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleTimeManager.sQueue = new Queue<Schedule>();
    battleTimeManager.environment = env_ != null ? env_ : Singleton<NGBattleManager>.GetInstance().environment;
    battleTimeManager.StartCoroutine("exec");
    return false;
  }

  public override IEnumerator cleanup()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleTimeManager battleTimeManager = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleTimeManager.sQueue = (Queue<Schedule>) null;
    battleTimeManager.StopCoroutine("exec");
    return false;
  }

  public bool insertMode
  {
    set
    {
      if (value == this._insertMode)
        return;
      if (value && this._insertQueue == null)
        this._insertQueue = new Queue<Schedule>();
      this._insertMode = value;
    }
    get => this._insertMode;
  }

  private IEnumerator exec()
  {
    this._isRunning = false;
    while (true)
    {
      while (this.sQueue.Count <= 0)
      {
        if (this._isRunning)
          this._isRunning = false;
        else
          yield return (object) null;
      }
      this._isRunning = true;
      Schedule s = this.doStart(this.sQueue.Peek());
      while (!s.completedp())
        yield return (object) null;
      this.doEnd(s);
      this.sQueue.Dequeue();
      if (this._insertQueue != null)
      {
        foreach (Schedule s1 in this.sQueue)
          this._insertQueue.Enqueue(s1);
        this.sQueue = this._insertQueue;
        this._insertQueue = (Queue<Schedule>) null;
      }
      s = (Schedule) null;
    }
  }

  public bool isRunning => this.sQueue.Count > 0 || this._isRunning;

  private Schedule doStart(Schedule s)
  {
    s.startTime = Time.time;
    if (s.isSetBattleEnable)
      Singleton<NGBattleManager>.GetInstance().isBattleEnable = s.isBattleEnable;
    if (!s.completedp() && s.state != BL.Phase.unset)
      this.environment.core.phaseState.setState_(BL.Phase.none, this.environment.core);
    s.execBody();
    return s;
  }

  private Schedule doEnd(Schedule s)
  {
    if (s.endAction != null)
      s.endAction();
    if (s.state != BL.Phase.unset)
      this.environment.core.phaseState.setState_(s.state, this.environment.core);
    return s;
  }

  private bool checkDuplicationState(BL.Phase state)
  {
    if (this._insertQueue != null)
    {
      foreach (Schedule insert in this._insertQueue)
      {
        if (insert.state == state)
          return true;
      }
    }
    foreach (Schedule s in this.sQueue)
    {
      if (s.state == state)
        return true;
    }
    return false;
  }

  public void setSchedule(Schedule s)
  {
    if (this._insertQueue != null)
      this._insertQueue.Enqueue(s);
    else
      this.sQueue.Enqueue(s);
  }

  public void setPhaseState(BL.Phase state, bool isOnce = false)
  {
    if (isOnce && this.checkDuplicationState(state))
      return;
    this.setSchedule(new Schedule() { state = state });
  }

  public void setEnableWait(float wait)
  {
    this.setSchedule((Schedule) new ScheduleEnableWait(wait));
    this.setSchedule(new Schedule()
    {
      isSetBattleEnable = true,
      isBattleEnable = true
    });
  }

  public void setEnableWait(Func<bool> waitF)
  {
    this.setSchedule((Schedule) new ScheduleEnableFuncWait(waitF));
    this.setSchedule(new Schedule()
    {
      isSetBattleEnable = true,
      isBattleEnable = true
    });
  }

  public void setTargetPanel(
    BL.Panel panel,
    float wait,
    Action func = null,
    Action endAction = null,
    bool isWaitCameraMove = false)
  {
    this.setSchedule((Schedule) new BattleTimeManager.TargetCamera(panel, wait, func, endAction, isWaitCameraMove));
  }

  public void setTargetUnit(
    BL.UnitPosition up,
    float wait,
    GameObject effect = null,
    Action func = null,
    Action endAction = null,
    bool isWaitCameraMove = false)
  {
    this.setSchedule((Schedule) new BattleTimeManager.TargetCamera(this.environment.core.getFieldPanel(up), wait, func, (Action) (() =>
    {
      if (Object.op_Inequality((Object) effect, (Object) null))
        Singleton<NGBattleManager>.GetInstance().battleEffects.fieldEffectsStart(effect, up.unit);
      if (endAction == null)
        return;
      endAction();
    }), isWaitCameraMove));
  }

  public void setTargetBeforeMoveUnit(BL.Panel panel, Action endAction)
  {
    BattleTimeManager.TargetCamera s = new BattleTimeManager.TargetCamera(panel, 0.0f, (Action) null, endAction, true);
    s.EnableBeforeMoveUnit();
    this.setSchedule((Schedule) s);
  }

  public void setTargetMoveUnit(BL.Panel panel, float time)
  {
    BattleTimeManager.TargetCamera s = new BattleTimeManager.TargetCamera(panel, 0.0f, (Action) null, (Action) null, false);
    s.EnableMoveUnit(time);
    this.setSchedule((Schedule) s);
  }

  public void setCurrentUnit(BL.UnitPosition up, float wait = 0.0f, bool isWaitCameraMove = false)
  {
    Debug.LogWarning((object) (" ======= setCurrentUnit:" + (up == null ? "null" : up.unit.unit.name)));
    if (up != null)
    {
      if (this.IsCameraMove(up.unit))
        this.setTargetUnit(up, wait, func: (Action) (() => this.environment.setCurrentUnit_(up.unit)), isWaitCameraMove: isWaitCameraMove);
      else
        this.environment.setCurrentUnit_(up.unit);
    }
    else
      this.setScheduleAction((Action) (() => this.environment.setCurrentUnit_((BL.Unit) null)), wait);
  }

  public void setCurrentUnit(BL.Unit unit, float wait = 0.1f, bool isWaitCameraMove = false)
  {
    this.setCurrentUnit(unit, new Func<BL.Unit, bool>(this.IsCameraMove), new Action<BL.Unit>(this.environment.setCurrentUnit_), wait, isWaitCameraMove);
  }

  public void setCurrentUnitByPlayerInput(BL.Unit unit)
  {
    this.setCurrentUnit(unit, new Func<BL.Unit, bool>(this.IsCameraMoveByPlayerInput), new Action<BL.Unit>(this.environment.setCurrentUnitByPlayerInput), 0.1f, false);
  }

  private void setCurrentUnit(
    BL.Unit unit,
    Func<BL.Unit, bool> isCameraMove,
    Action<BL.Unit> setCurrentAction,
    float wait,
    bool isWaitCameraMove)
  {
    if (unit != (BL.Unit) null)
    {
      if (isCameraMove(unit))
        this.setTargetUnit(this.environment.core.getUnitPosition(unit), wait, func: (Action) (() => setCurrentAction(unit)), isWaitCameraMove: isWaitCameraMove);
      else
        setCurrentAction(unit);
    }
    else
      this.setScheduleAction((Action) (() => setCurrentAction((BL.Unit) null)), wait);
  }

  private bool IsCameraMove(BL.Unit unit)
  {
    return this.environment.core.phaseState.state != BL.Phase.pvp_disposition;
  }

  private bool IsCameraMoveByPlayerInput(BL.Unit unit)
  {
    return this.environment.core.phaseState.state != BL.Phase.pvp_disposition && unit != (BL.Unit) null && unit.isPlayerForce;
  }

  public void setScheduleAction(
    Action action,
    float wait = 0.0f,
    Action endAction = null,
    Func<bool> comleteCheckFunc = null,
    bool isInsertMode = false)
  {
    this.setSchedule((Schedule) new BattleTimeManager.ScheduleAction(action, wait, endAction, comleteCheckFunc, isInsertMode));
  }

  public void changeSceneWithReturnWait(
    string sceneName,
    bool isStack = false,
    Action initAction = null,
    Action endAction = null,
    Action sceneEndAction = null,
    params object[] args)
  {
    NGBattleManager bm = Singleton<NGBattleManager>.GetInstance();
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    this.setSchedule((Schedule) new BattleTimeManager.ScheduleAction((Action) (() =>
    {
      if (initAction != null)
        initAction();
      bm.popupCloseAll(true);
      bm.isBattleEnable = false;
      this.StartCoroutine(this.ChangeScene(sceneName, isStack, args));
    }), 0.0f, (Action) (() =>
    {
      if (sceneEndAction != null)
        sceneEndAction();
      if (!(bm.topScene != sm.sceneName))
        return;
      bm.isBattleEnable = false;
    }), (Func<bool>) (() => sm.isSceneInitialized && sm.changeSceneQueueCount <= 0), false));
    this.setSchedule((Schedule) new BattleTimeManager.ScheduleAction((Action) null, 0.0f, endAction, (Func<bool>) (() => bm.isBattleEnable), false));
  }

  private IEnumerator ChangeScene(string sceneName, bool isStack = false, params object[] args)
  {
    yield return (object) new WaitWhile((Func<bool>) (() => Singleton<NGDuelDataManager>.GetInstance().IsBackgroundPreloading()));
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, isStack, args);
  }

  public void backSceneWithReturnWait(bool isStack = false, Action initAction = null)
  {
    NGBattleManager bm = Singleton<NGBattleManager>.GetInstance();
    NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
    this.setSchedule((Schedule) new BattleTimeManager.ScheduleAction((Action) (() =>
    {
      if (initAction != null)
        initAction();
      bm.popupCloseAll(true);
      bm.isBattleEnable = false;
      sm.backScene();
    }), 0.0f, (Action) (() =>
    {
      if (!(bm.topScene != sm.sceneName))
        return;
      bm.isBattleEnable = false;
    }), (Func<bool>) (() => sm.isSceneInitialized && sm.changeSceneQueueCount <= 0), false));
    this.setSchedule((Schedule) new BattleTimeManager.ScheduleAction((Action) null, 0.0f, (Action) null, (Func<bool>) (() => bm.isBattleEnable), false));
  }

  private class TargetCamera : Schedule
  {
    private BL.Panel panel;
    private float wait;
    private Action func;
    private bool isWaitCameraMove;
    private bool isBeforeMoveUnitCamera;
    private bool isMoveUnitCamera;
    private float moveUnitCameraTime;
    private NGBattleManager bm;
    private BattleCameraController cc;

    public TargetCamera(
      BL.Panel panel,
      float wait,
      Action func,
      Action endAction,
      bool isWaitCameraMove)
    {
      this.panel = panel;
      this.wait = wait;
      this.func = func;
      this.endAction = endAction;
      this.isWaitCameraMove = isWaitCameraMove;
      this.bm = Singleton<NGBattleManager>.GetInstance();
      this.cc = this.bm.getController<BattleCameraController>();
    }

    public void EnableBeforeMoveUnit()
    {
      this.isMoveUnitCamera = false;
      this.isBeforeMoveUnitCamera = true;
    }

    public void EnableMoveUnit(float time)
    {
      this.isMoveUnitCamera = true;
      this.isBeforeMoveUnitCamera = false;
      this.moveUnitCameraTime = time;
    }

    public override bool body()
    {
      if (this.isBeforeMoveUnitCamera)
        this.cc.setBeforeMoveUnit(this.panel);
      else if (this.isMoveUnitCamera)
        this.cc.setMoveUnit(this.panel, this.moveUnitCameraTime);
      else
        this.cc.setLookAtTarget(this.panel);
      this.bm.environment.core.setCurrentField(this.panel);
      if (this.func != null)
        this.func();
      return true;
    }

    public override bool completedp()
    {
      bool flag = (double) this.deltaTime >= (double) this.wait;
      if (flag && this.isWaitCameraMove)
        flag = !this.cc.isCameraMove;
      return flag;
    }
  }

  private class ScheduleAction : Schedule
  {
    private Action action;
    private float wait;
    private Func<bool> comleteCheckFunc;

    public ScheduleAction(
      Action action,
      float wait,
      Action endAction,
      Func<bool> comleteCheckFunc,
      bool isInsertMode)
    {
      this.action = action;
      this.wait = wait;
      this.endAction = endAction;
      this.comleteCheckFunc = comleteCheckFunc;
      this.isInsertMode = isInsertMode;
    }

    public override bool body()
    {
      if (this.action != null)
        this.action();
      return true;
    }

    public override bool completedp()
    {
      bool flag = (double) this.deltaTime >= (double) this.wait;
      if (flag && this.comleteCheckFunc != null)
        flag = this.comleteCheckFunc();
      return flag;
    }
  }
}
