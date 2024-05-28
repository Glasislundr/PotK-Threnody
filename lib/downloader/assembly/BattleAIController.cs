// Decompiled with JetBrains decompiler
// Type: BattleAIController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleAIController : BattleMonoBehaviour
{
  private NGBattleAIManager aiManager;
  private BattleTimeManager btm;
  private const float waitTime = 0.5f;
  private bool mIsAction;
  private bool mWaitingInitUnits;
  private bool isTerminate;

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleAIController battleAiController = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleAiController.aiManager = ((Component) battleAiController).GetComponent<NGBattleAIManager>();
    battleAiController.btm = battleAiController.battleManager.getManager<BattleTimeManager>();
    return false;
  }

  public bool isAction => this.mIsAction;

  public bool isCompleted
  {
    get
    {
      if (!this.battleManager.isPvp)
        return this.aiManager.ai.isCompleted;
      return this.env.core.aiActionOrder.value != null && this.env.core.aiActionOrder.value.Count > 0;
    }
  }

  public bool startAI(List<BL.UnitPosition> units, bool isCharm, int max = 0)
  {
    if (this.mIsAction || this.aiManager.ai.isInitialized || this.mWaitingInitUnits)
      return false;
    App.SetAutoSleep(false);
    Action action = (Action) (() =>
    {
      List<BL.UnitPosition> units1 = new List<BL.UnitPosition>();
      foreach (BL.UnitPosition unit in units)
      {
        if (unit.unit.IsCharm == isCharm)
          units1.Add(unit);
      }
      this.aiManager.ai.initUnits(units1, max);
      this.mWaitingInitUnits = false;
    });
    Singleton<CommonRoot>.GetInstance().isActive3DUIMask = true;
    if (!this.battleManager.isPvp && !isCharm && Persist.autoBattleSetting.Data.isCallSkill)
    {
      bool flag = false;
      bool isPlayer = true;
      BL.Skill skill = new BL.Skill();
      if (this.env.core.phaseState.state == BL.Phase.enemy || this.env.core.phaseState.state == BL.Phase.pvp_enemy_start)
      {
        flag = false;
        isPlayer = false;
        skill.id = this.env.core.enemyCallSkillState.skillId;
        skill.remain = new int?(1);
      }
      else if (this.env.core.phaseState.state != BL.Phase.neutral)
      {
        flag = this.env.core.playerCallSkillState.isCanUseCallSkill;
        isPlayer = true;
        skill.id = this.env.core.playerCallSkillState.skillId;
        skill.remain = new int?(1);
      }
      if (flag)
      {
        List<BL.Unit> list = this.env.core.getCallSkillTargetUnits(skill, isPlayer).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).Where<BL.Unit>((Func<BL.Unit, bool>) (x => x.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) x, this.env.core, (BL.ISkillEffectListUnit) null, 1, new bool?(isPlayer)) == 0)).ToList<BL.Unit>();
        if (list.Any<BL.Unit>())
        {
          if (this.battleManager.useGameEngine)
            this.battleManager.gameEngine.useCallSkill(skill, list, isPlayer);
          else
            this.env.useCallSkill(skill, list, this.battleManager.getManager<BattleTimeManager>(), isPlayer);
          this.mWaitingInitUnits = true;
          this.btm.setScheduleAction(action);
        }
      }
    }
    if (!this.mWaitingInitUnits)
      action();
    return true;
  }

  public void startAIAction(float waitForShowAiStopButton = 3f, Action endAction = null)
  {
    if (this.mIsAction || this.env.core.aiActionOrder.value == null)
      return;
    Singleton<CommonRoot>.GetInstance().isActive3DUIMask = true;
    this.isTerminate = false;
    this.StartCoroutine(this.actionAIUnits(waitForShowAiStopButton, endAction));
  }

  public void stopAIAction()
  {
    App.SetAutoSleep(true);
    if (this.mIsAction)
    {
      Debug.LogWarning((object) " === stopAIAction");
      this.isTerminate = true;
    }
    this.aiManager.ai.cleanup();
    Singleton<CommonRoot>.GetInstance().isActive3DUIMask = false;
  }

  public void enqueueAIActionOrder(BL.AIUnit aiUnit)
  {
    if (this.env.core.aiActionOrder.value == null)
      this.env.core.aiActionOrder.value = new Queue<BL.AIUnit>();
    this.env.core.aiActionOrder.value.Enqueue(aiUnit);
    this.env.core.aiActionOrder.commit();
  }

  public void clearAIActionOrder()
  {
    this.env.core.aiActionOrder.value = (Queue<BL.AIUnit>) null;
    this.env.core.aiActionOrder.commit();
  }

  private bool enable
  {
    get
    {
      if (!this.battleManager.isPvp && !this.battleManager.isBattleEnable)
        return false;
      foreach (BL.UnitPosition up in this.env.core.unitPositions.value)
      {
        if (up.isMoving(this.env))
          return false;
      }
      return !this.btm.isRunning;
    }
  }

  private bool moveUnit(BL.UnitPosition up, int row, int column)
  {
    if (up.row == row && up.column == column)
      return false;
    List<BL.Panel> moveRoute = this.env.core.getRouteNonCache(up, this.env.core.getFieldPanel(up), this.env.core.getFieldPanel(row, column), up.movePanels);
    if (moveRoute.Count > 0)
      this.btm.setTargetBeforeMoveUnit(moveRoute.First<BL.Panel>(), (Action) (() => this.btm.setScheduleAction((Action) (() =>
      {
        up.startMoveRoute(moveRoute, this.battleManager.defaultUnitSpeed, this.env, (Action) (() =>
        {
          NGBattle3DObjectManager manager = this.battleManager.getManager<NGBattle3DObjectManager>();
          if (!Object.op_Inequality((Object) manager, (Object) null))
            return;
          manager.hideWaitButton(moveRoute.First<BL.Panel>());
        }));
        BattleCameraController controller = this.battleManager.getController<BattleCameraController>();
        if (!Object.op_Inequality((Object) controller, (Object) null))
          return;
        controller.setMoveUnit(moveRoute.Last<BL.Panel>(), up.calcMoveTime(moveRoute.Count, this.battleManager.defaultUnitSpeed));
      }), 0.5f, comleteCheckFunc: (Func<bool>) (() =>
      {
        foreach (BL.UnitPosition up1 in this.env.core.unitPositions.value)
        {
          if (up1.isMoving(this.env))
            return false;
        }
        return true;
      }))));
    return true;
  }

  private IEnumerator actionAIUnits(float waitForShowAiStopButton, Action endAction)
  {
    BattleAIController battleAiController1 = this;
    CommonRoot root = Singleton<CommonRoot>.GetInstance();
    battleAiController1.mIsAction = true;
    while (!battleAiController1.isCompleted)
    {
      waitForShowAiStopButton -= Time.deltaTime;
      yield return (object) null;
      if (battleAiController1.isTerminate)
        goto label_63;
    }
    while (battleAiController1.env.core.aiActionOrder.value != null && battleAiController1.env.core.aiActionOrder.value.Count > 0)
    {
      BattleAIController battleAiController = battleAiController1;
      NGSceneManager sm = Singleton<NGSceneManager>.GetInstance();
      bool isPvpNextScene = sm.sceneName != battleAiController1.battleManager.topScene;
      while (!battleAiController1.enable)
      {
        waitForShowAiStopButton -= Time.deltaTime;
        yield return (object) null;
        if (battleAiController1.isTerminate)
          goto label_63;
      }
      BL.AIUnit aiUnit = battleAiController1.env.core.aiActionOrder.value.Peek();
      BL.UnitPosition up = aiUnit.unitPosition;
      if ((battleAiController1.env.core.phaseState.state == BL.Phase.player || battleAiController1.env.core.phaseState.state == BL.Phase.pvp_move_unit_waiting) && battleAiController1.env.core.phaseState.turnCount == 1)
        battleAiController1.env.unitResource[up.unit].PlayVoiceDuelStart(up.unit);
      up.movePanels = (HashSet<BL.Panel>) null;
      battleAiController1.btm.setCurrentUnit(up.unit);
      if (aiUnit.actionResults == null & isPvpNextScene)
        battleAiController1.btm.setScheduleAction((Action) (() => sm.backScene()));
      while (!battleAiController1.enable)
      {
        waitForShowAiStopButton -= Time.deltaTime;
        yield return (object) null;
        if (battleAiController1.isTerminate)
          goto label_63;
      }
      if (aiUnit.action != null)
        battleAiController1.btm.setScheduleAction((Action) (() => aiUnit.action()));
      if (aiUnit.actionResults != null)
      {
        if (!Singleton<NGBattleManager>.GetInstance().noDuelScene)
        {
          foreach (ActionResult actionResult in aiUnit.actionResults.Where<ActionResult>((Func<ActionResult, bool>) (ar => ar is DuelResult)))
          {
            DuelResult duelResult = actionResult as DuelResult;
            Singleton<NGDuelDataManager>.GetInstance().StartBackGroundPreload(duelResult);
          }
        }
        foreach (ActionResult actionResult in aiUnit.actionResults)
        {
          ActionResult ar = actionResult;
          DuelResult duelResult = ar as DuelResult;
          BL.BattleSkillResult skillResult = ar as BL.BattleSkillResult;
          int row = ar.isMove ? ar.row : aiUnit.row;
          int column = ar.isMove ? ar.column : aiUnit.column;
          battleAiController1.env.core.setSomeAction();
          if (battleAiController1.moveUnit(up, row, column))
          {
            do
            {
              waitForShowAiStopButton -= Time.deltaTime;
              yield return (object) null;
              if (battleAiController1.isTerminate)
                goto label_57;
            }
            while (!battleAiController1.enable);
          }
          if (duelResult != null)
          {
            if (duelResult.isHeal & isPvpNextScene)
              battleAiController1.btm.setScheduleAction((Action) (() => sm.backScene()));
            BL.Unit attack = duelResult.attack;
            BL.Unit defense = duelResult.defense;
            if (!isPvpNextScene)
              battleAiController1.btm.setScheduleAction((Action) (() => closure_4.env.core.lookDirection(attack, defense)), 0.5f);
            do
            {
              waitForShowAiStopButton -= Time.deltaTime;
              yield return (object) null;
              if (battleAiController1.isTerminate)
                goto label_57;
            }
            while (!battleAiController1.enable);
            if (duelResult.isHeal)
            {
              AttackStatus attackAttackStatus = duelResult.attackAttackStatus;
              BE env = battleAiController1.env;
              BL.MagicBullet magicBullet = attackAttackStatus.magicBullet;
              int attack1 = attackAttackStatus.healAttack((BL.ISkillEffectListUnit) attack, (BL.ISkillEffectListUnit) defense);
              BL.Unit unit = attack;
              List<BL.Unit> targets = new List<BL.Unit>();
              targets.Add(defense);
              BattleTimeManager btm = battleAiController1.btm;
              env.useMagicBullet(magicBullet, attack1, unit, targets, btm);
            }
            else
            {
              battleAiController1.btm.setScheduleAction((Action) (() =>
              {
                if (closure_7.isTerminate)
                  return;
                closure_7.battleManager.startDuel(duelResult, isStack: !isPvpNextScene);
              }));
              do
              {
                waitForShowAiStopButton -= Time.deltaTime;
                yield return (object) null;
                if (battleAiController1.isTerminate)
                  goto label_57;
              }
              while (battleAiController1.btm.isRunning || !battleAiController1.battleManager.isBattleEnable);
            }
          }
          else if (skillResult != null)
            battleAiController1.env.useSkill(skillResult.invocation, skillResult.skill, skillResult.targets, skillResult.panels, skillResult, battleAiController1.btm, skillResult.random);
          bool flag = false;
          if (battleAiController1.battleManager.isGvg && battleAiController1.battleManager.gvgManager.isGameFinish())
            flag = true;
          if (flag && endAction != null)
          {
            endAction();
            do
            {
              waitForShowAiStopButton -= Time.deltaTime;
              yield return (object) null;
            }
            while (!battleAiController1.isTerminate);
          }
          else
          {
            if (ar.terminate)
            {
              battleAiController1.btm.setScheduleAction((Action) (() => up.completeActionUnit(battleAiController.env.core, true)));
              break;
            }
            do
            {
              waitForShowAiStopButton -= Time.deltaTime;
              yield return (object) null;
              if (battleAiController1.isTerminate)
                goto label_57;
            }
            while (!battleAiController1.enable);
            skillResult = (BL.BattleSkillResult) null;
            ar = (ActionResult) null;
            continue;
          }
label_57:
          goto label_63;
        }
      }
      battleAiController1.env.core.aiActionOrder.value.Dequeue();
      do
      {
        waitForShowAiStopButton -= Time.deltaTime;
        yield return (object) null;
        if (battleAiController1.isTerminate)
          goto label_63;
      }
      while (!battleAiController1.enable);
    }
label_63:
    if ((double) waitForShowAiStopButton > 0.0)
      yield return (object) new WaitForSeconds(waitForShowAiStopButton);
    if (battleAiController1.isTerminate)
      battleAiController1.btm.setCurrentUnit((BL.Unit) null);
    if (endAction != null)
      endAction();
    battleAiController1.aiManager.ai.cleanup();
    battleAiController1.mIsAction = false;
    root.isActive3DUIMask = false;
  }

  public void clearCache() => this.aiManager.clearCache();
}
