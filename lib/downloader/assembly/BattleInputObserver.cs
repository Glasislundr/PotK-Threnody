// Decompiled with JetBrains decompiler
// Type: BattleInputObserver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleInputObserver : BattleMonoBehaviour
{
  private BattleInputObserver.Mode mode;
  public bool isCameraMoveMode;
  public bool isDispositionMode;
  public bool isTouchEnable;
  public bool isUnitScrollDragging;
  private BattleCameraController cameraController;
  private BattleUnitController unitController;
  private NGBattle3DObjectManager objectManager;
  private BattleTimeManager btm;
  private BL.Panel lastClickedPanel;
  private int clickedSamePanelCounter;
  private BL.BattleModified<BL.CurrentUnit> currentUnitForLastClickedPanel;
  private Battle01PVPNode pvpNode;
  private BL.BattleModified<BL.CurrentUnit> currentUnitForDispositionMode;
  private BL.Unit unitForDispositionMode;
  private Func<bool, bool> checkCancelPressed;
  private Func<bool> checkCancelClick;
  private BattleInputObserver.SelectTarget mSelectTargets;
  private HashSet<BL.Panel> dispositionPanels;
  private BL.Panel firstDownPanel;
  private const float LONG_PRESS_TIME = 0.3f;
  private float firstDownPanelLongPressTime = 0.3f;
  private BL.Panel mDownPanel;

  public bool isTargetSelectMode => this.mode == BattleInputObserver.Mode.targetselect;

  public void setFuncCheckCancelPressed(Func<bool, bool> func = null)
  {
    this.checkCancelPressed = func;
  }

  public void setFuncCheckCancelClick(Func<bool> func = null) => this.checkCancelClick = func;

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleInputObserver battleInputObserver = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    UICamera.fallThrough = ((Component) battleInputObserver).gameObject;
    battleInputObserver.cameraController = ((Component) battleInputObserver).GetComponent<BattleCameraController>();
    battleInputObserver.unitController = ((Component) battleInputObserver).GetComponent<BattleUnitController>();
    battleInputObserver.objectManager = battleInputObserver.battleManager.getManager<NGBattle3DObjectManager>();
    battleInputObserver.btm = battleInputObserver.battleManager.getManager<BattleTimeManager>();
    battleInputObserver.isTouchEnable = true;
    battleInputObserver.isUnitScrollDragging = false;
    battleInputObserver.currentUnitForLastClickedPanel = BL.Observe<BL.CurrentUnit>(battleInputObserver.env.core.unitCurrent);
    battleInputObserver.currentUnitForDispositionMode = BL.Observe<BL.CurrentUnit>(battleInputObserver.env.core.unitCurrent);
    Battle01CommandWait.SetOnClickAction(new Action(battleInputObserver.ResetClickedSamePanelCounter));
    return false;
  }

  protected override void LateUpdate_Battle()
  {
    this.UpdateLastClickedPanelByCurrentUnitChanged();
    this.UpdateLastClickedPanelByModifiedUI();
    this.UpdateFirstDownPanelLongPressTime((Action) (() =>
    {
      BL.UnitPosition fieldUnit = this.env.core.getFieldUnit(this.firstDownPanel, includeJumping: true);
      this.battleManager.getUiNode().OpenUnitInfoPopup(fieldUnit?.unit);
      this.ResetClickedSamePanelCounter();
      this.isCameraMoveMode = false;
    }));
    this.UpdateDispositionMode();
    if (this.env.core.isTouchWait.value)
      return;
    this.ResetClickedSamePanelCounter();
  }

  private void OnDisable() => UICamera.fallThrough = (GameObject) null;

  private void OnHover(bool isOver)
  {
  }

  public bool isCurrentUnitAction
  {
    get
    {
      BL.UnitPosition currentUnitPosition = this.env.core.currentUnitPosition;
      if (!(currentUnitPosition.unit != (BL.Unit) null))
        return false;
      if (this.isDispositionMode)
        return true;
      return !currentUnitPosition.isCompleted && currentUnitPosition.unit.isPlayerControl && currentUnitPosition.unit.hp > 0 && this.env.core.currentPhaseUnitp(currentUnitPosition);
    }
  }

  private void setUnitMoveMode(BL.UnitPosition up)
  {
    if (this.isCurrentUnitAction && up.unit != this.env.core.unitCurrent.unit || this.isTargetSelectMode || !this.unitController.CanMoveUnit(up))
      return;
    this.btm.setCurrentUnit(up);
    if (this.isCameraMoveMode)
    {
      this.cameraController.onCancel();
      this.isCameraMoveMode = false;
    }
    this.selectTargets = (BattleInputObserver.SelectTarget) null;
  }

  private void setCameraMoveMode()
  {
    this.isCameraMoveMode = true;
    this.cameraController.onPress();
  }

  private BattleInputObserver.SelectTarget selectTargets
  {
    get => this.mSelectTargets;
    set
    {
      if (this.mSelectTargets != null && this.mSelectTargets.Equals((object) value))
        return;
      if (this.mSelectTargets != null)
      {
        foreach (BL.Unit attackTarget in this.mSelectTargets.attackTargets)
        {
          BL.Panel fieldPanel = this.env.core.getFieldPanel(this.env.core.getUnitPosition(attackTarget));
          fieldPanel.unsetAttribute(BL.PanelAttribute.target_attack);
          if (this.mSelectTargets.selectFunc != null)
            this.objectManager.hideButton(fieldPanel);
        }
        foreach (BL.Unit healTarget in this.mSelectTargets.healTargets)
        {
          BL.Panel fieldPanel = this.env.core.getFieldPanel(this.env.core.getUnitPosition(healTarget));
          fieldPanel.unsetAttribute(BL.PanelAttribute.target_heal);
          if (this.mSelectTargets.selectFunc != null)
            this.objectManager.hideButton(fieldPanel);
        }
        foreach (BL.Panel panel in this.mSelectTargets.panels)
        {
          panel.unsetAttribute(BL.PanelAttribute.target_attack);
          if (this.mSelectTargets.selectFunc != null)
            this.objectManager.hideButton(panel);
        }
      }
      if (value != null)
      {
        foreach (BL.Unit attackTarget in value.attackTargets)
        {
          BL.Panel fieldPanel = this.env.core.getFieldPanel(this.env.core.getUnitPosition(attackTarget));
          fieldPanel.setAttribute(BL.PanelAttribute.target_attack);
          if (value.selectFunc != null)
            this.objectManager.setButton(fieldPanel, false, value.grayTargets.Contains(attackTarget));
        }
        foreach (BL.Unit healTarget in value.healTargets)
        {
          BL.Panel fieldPanel = this.env.core.getFieldPanel(this.env.core.getUnitPosition(healTarget));
          fieldPanel.setAttribute(BL.PanelAttribute.target_heal);
          if (value.selectFunc != null)
            this.objectManager.setButton(fieldPanel, true, value.grayTargets.Contains(healTarget));
        }
        foreach (BL.Panel panel in value.panels)
        {
          panel.setAttribute(BL.PanelAttribute.target_attack);
          if (value.selectFunc != null)
            this.objectManager.setButton(panel, false, false);
        }
      }
      this.mSelectTargets = value;
    }
  }

  public bool containsTargetSelect(BL.Unit unit)
  {
    if (!this.isTargetSelectMode)
      return false;
    if (this.selectTargets.targets.Contains(unit))
      return true;
    BL.UnitPosition unitPosition = this.env.core.getUnitPosition(unit);
    return unitPosition != null && this.selectTargets.panels.Contains(this.env.core.getFieldPanel(unitPosition));
  }

  public bool containsGrayTargetSelect(BL.Unit unit)
  {
    return this.isTargetSelectMode && this.selectTargets.grayTargets.Contains(unit);
  }

  public void setTargetSelectMode(
    List<BL.Unit> attackTargets,
    List<BL.Unit> healTargets,
    List<BL.Unit> grayTargets,
    List<BL.Panel> panels,
    Action<BL.Unit, BL.Panel> func)
  {
    attackTargets = attackTargets.Where<BL.Unit>((Func<BL.Unit, bool>) (u => u.isEnable && !u.isDead)).ToList<BL.Unit>();
    healTargets = healTargets.Where<BL.Unit>((Func<BL.Unit, bool>) (u => u.isEnable && !u.isDead)).ToList<BL.Unit>();
    grayTargets = grayTargets.Where<BL.Unit>((Func<BL.Unit, bool>) (u => u.isEnable && !u.isDead)).ToList<BL.Unit>();
    this.mode = BattleInputObserver.Mode.targetselect;
    this.selectTargets = new BattleInputObserver.SelectTarget(attackTargets, healTargets, grayTargets, panels, func);
    this.cameraController.onCancel();
    this.env.core.currentUnitPosition.commit();
  }

  public void cancelTargetSelect()
  {
    if (!this.isTargetSelectMode)
      return;
    this.selectTargets = (BattleInputObserver.SelectTarget) null;
    this.mode = BattleInputObserver.Mode.none;
    this.env.core.currentUnitPosition.commit();
  }

  private void delegateSelectTargets(BL.Unit unit, BL.Panel panel)
  {
    if (!this.isTargetSelectMode || this.selectTargets.selectFunc == null)
      return;
    BL.Unit unit1 = !(unit != (BL.Unit) null) || !this.selectTargets.targets.Contains(unit) ? (BL.Unit) null : unit;
    BL.Panel panel1 = this.selectTargets.panels.Contains(panel) ? panel : (BL.Panel) null;
    if (!(unit1 != (BL.Unit) null) && panel1 == null)
      return;
    this.selectTargets.selectFunc(unit, panel);
  }

  public void setDispositionMode(HashSet<BL.Panel> pl)
  {
    Debug.LogWarning((object) (" === setDispositionMode:" + (object) pl));
    if (pl == null)
    {
      this.isDispositionMode = false;
      this.dispositionPanels = (HashSet<BL.Panel>) null;
    }
    else
    {
      this.isDispositionMode = true;
      this.dispositionPanels = pl;
    }
  }

  public HashSet<BL.Panel> getMovePanels(BL.UnitPosition up)
  {
    return !this.isDispositionMode ? up.movePanels : this.dispositionPanels;
  }

  private bool containsMovePanels(BL.Panel panel, BL.UnitPosition up)
  {
    return (this.isDispositionMode ? BattleFuncs.moveCompletePanels_(this.dispositionPanels, up.unit, isOriginal: false) : up.completePanels).Contains(panel);
  }

  public void onCancel(bool isTargetSelectCancel = false)
  {
    if (isTargetSelectCancel)
      this.cancelTargetSelect();
    if (!this.isCameraMoveMode)
      return;
    this.cameraController.onCancel();
    this.isCameraMoveMode = false;
  }

  private BL.Panel downPanel
  {
    set
    {
      if (this.mDownPanel == value)
        return;
      if (this.firstDownPanel == null || value == null)
      {
        this.firstDownPanel = value;
        this.firstDownPanelLongPressTime = 0.3f;
      }
      if (this.mDownPanel != null)
        this.env.panelResource[this.mDownPanel].gameObject.GetComponent<BattlePanelParts>().buttonDown(false);
      this.mDownPanel = value;
      if (this.mDownPanel == null)
        return;
      this.UpdateLastClickedPanel(this.mDownPanel);
      this.env.panelResource[this.mDownPanel].gameObject.GetComponent<BattlePanelParts>().buttonDown(true);
    }
  }

  private void UpdateFirstDownPanelLongPressTime(Action action)
  {
    if (this.firstDownPanel == null || this.mDownPanel != this.firstDownPanel)
      return;
    this.firstDownPanelLongPressTime -= Time.unscaledDeltaTime;
    if ((double) this.firstDownPanelLongPressTime > 0.0)
      return;
    if (action != null)
      action();
    this.firstDownPanel = (BL.Panel) null;
  }

  private void OnPress(bool pressed)
  {
    if (!this.isTouchEnable || !this.battleManager.isBattleEnable || this.checkCancelPressed != null && this.checkCancelPressed(pressed))
      return;
    if (pressed)
    {
      if (!this.battleManager.isOvo && this.env.core.CheckStageFinished())
        return;
      foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
      {
        if (unitPosition.unit.hp <= 0 && !unitPosition.unit.isDead)
          return;
      }
      BL.Panel panel = NGBattle3DObjectManager.hitPanel(UICamera.lastTouchPosition);
      if (panel != null)
        this.downPanel = panel;
      this.setCameraMoveMode();
    }
    else
    {
      this.downPanel = (BL.Panel) null;
      if (!this.isCameraMoveMode)
        return;
      this.cameraController.onRelease();
      this.isCameraMoveMode = false;
    }
  }

  private void OnClick_(BL.Panel panel)
  {
    if (!this.battleManager.isOvo && this.env.core.CheckStageFinished())
      return;
    foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
    {
      if (unitPosition.unit.hp <= 0 && !unitPosition.unit.isDead)
        return;
    }
    if (this.isTargetSelectMode)
    {
      BL.UnitPosition fieldUnit = this.env.core.getFieldUnit(panel, includeJumping: true);
      BL.Unit unit = fieldUnit == null || !this.selectTargets.targets.Contains(fieldUnit.unit) ? (BL.Unit) null : fieldUnit.unit;
      BL.Panel panel1 = this.selectTargets.panels.Contains(panel) ? panel : (BL.Panel) null;
      if (!(unit != (BL.Unit) null) && panel1 == null)
        return;
      this.delegateSelectTargets(unit, panel1);
    }
    else if (this.isDispositionMode)
      this.UpdateCurrentUnitPositionByOnClickWithDispositionMode(panel);
    else
      this.UpdateCurrentUnitPositionByOnClick(panel);
  }

  private void UpdateCurrentUnitPositionByOnClick(BL.Panel panel)
  {
    BL.UnitPosition fieldUnit = this.env.core.getFieldUnit(panel, includeJumping: true);
    this.PlayVoiceDuelStart(fieldUnit);
    if (this.CanChangeUnitMoveMode(panel, fieldUnit))
    {
      this.ChangeUnitMoveMode(panel, fieldUnit);
    }
    else
    {
      if (this.IsClickedSamePanel(panel, fieldUnit))
        this.AddClickedSamePanelCounter();
      this.UpdateCurrentUnitPosition(panel);
    }
  }

  private void PlayVoiceDuelStart(BL.UnitPosition up)
  {
    if (up == null || this.env.core.phaseState.turnCount != 1 || !(this.env.core.unitCurrent.unit == (BL.Unit) null) || !up.unit.isPlayerControl)
      return;
    this.env.unitResource[up.unit].PlayVoiceDuelStart(up.unit);
  }

  private void ChangeUnitMoveMode(BL.Panel panel, BL.UnitPosition up)
  {
    if (this.env.core.unitCurrent.unit == (BL.Unit) null || !this.env.core.unitCurrent.unit.isPlayerControl)
    {
      this.setUnitMoveMode(up);
      this.UpdateCurrentUnitPosition(panel);
    }
    else
    {
      this.battleManager.getUiNode().CommandBack.FindSelectNode();
      if (!this.battleManager.getUiNode().CommandBack.OnBackButtonImmediate())
        this.UpdateCurrentUnitPosition(panel);
      else
        this.btm.setScheduleAction((Action) (() =>
        {
          this.setUnitMoveMode(up);
          this.UpdateCurrentUnitPosition(panel);
        }));
    }
  }

  private bool CanChangeUnitMoveMode(BL.Panel panel, BL.UnitPosition up)
  {
    if (up == null || this.env.core.unitCurrent.unit == (BL.Unit) null || this.env.core.unitCurrent.unit == up.unit || up.unit.isPlayerForce && this.env.core.isCompleted(up.unit) || this.env.core.unitCurrent.unit.isPlayerForce && this.env.core.unitCurrent.unit.isPlayerControl && !up.unit.isPlayerForce || this.env.core.unitCurrent.unit.isPlayerForce && this.env.core.unitCurrent.unit.isPlayerControl && up.unit.isFacility && !up.unit.facility.isView || this.env.core.currentUnitPosition.cantChangeCurrent)
      return false;
    if (!this.isDispositionMode && this.isCurrentUnitAction)
    {
      BL.UnitPosition currentUnitPosition = this.env.core.currentUnitPosition;
      if (this.GetUnitPositionInTarget(panel, currentUnitPosition, BattleFuncs.getAttackTargetPanels(currentUnitPosition)) != null || this.GetUnitPositionInTarget(panel, currentUnitPosition, BattleFuncs.getHealTargetPanels(currentUnitPosition)) != null)
        return false;
    }
    return true;
  }

  private void UpdateCurrentUnitPositionByOnClickWithDispositionMode(BL.Panel panel)
  {
    BL.UnitPosition up = this.env.core.getFieldUnit(panel, includeJumping: true);
    if (this.CanChangeUnitMoveMode(panel, up))
    {
      if (this.CanChangeUnitPosition(up))
      {
        this.ChangeUnitPosition(up);
        return;
      }
      if (up != null && !up.unit.isFacility)
      {
        if (this.env.core.unitCurrent.unit.isPlayerControl)
        {
          this.battleManager.getUiNode().CommandBack.FindSelectNode();
          this.battleManager.getUiNode().CommandBack.OnBackButtonImmediate();
        }
        this.btm.setScheduleAction((Action) (() =>
        {
          this.env.core.fieldCurrent.value = panel;
          this.btm.setCurrentUnit(up.unit);
        }));
        return;
      }
    }
    this.UpdateCurrentUnitPosition(panel);
  }

  private void ChangeUnitPosition(BL.UnitPosition up)
  {
    int row1 = this.env.core.currentUnitPosition.row;
    int column1 = this.env.core.currentUnitPosition.column;
    int row2 = up.row;
    int column2 = up.column;
    BattleUnitParts unitParts1 = this.battleManager.environment.unitResource[this.env.core.currentUnitPosition.unit].unitParts_;
    BattleUnitParts unitParts2 = this.battleManager.environment.unitResource[up.unit].unitParts_;
    unitParts1.resetStatus(row2, column2, this.env.core.currentUnitPosition.direction);
    int row3 = row1;
    int column3 = column1;
    double direction = (double) up.direction;
    unitParts2.resetStatus(row3, column3, (float) direction);
    up.resetOriginalPosition(this.env.core, true);
    this.env.core.currentUnitPosition.resetOriginalPosition(this.env.core, true);
    if (Object.op_Equality((Object) this.pvpNode, (Object) null))
      this.pvpNode = ((Component) this.battleManager.getUiNode()).gameObject.GetComponent<Battle01PVPNode>();
    if (!Object.op_Inequality((Object) this.pvpNode, (Object) null))
      return;
    this.pvpNode.DispositionDecide.onClick();
  }

  private bool CanChangeUnitPosition(BL.UnitPosition up)
  {
    return up != null && up.unit != (BL.Unit) null && up.unit.isPlayerControl && this.env.core.unitCurrent.unit != (BL.Unit) null && this.env.core.unitCurrent.unit != up.unit && this.env.core.unitCurrent.unit.isPlayerControl;
  }

  private bool CanSelect(BL.UnitPosition up, BL.UnitPosition tup)
  {
    if (!up.unit.skillEffects.IsMoveSkillActionWaiting() && !this.battleManager.isDeadEffectPlaying)
      return true;
    return tup.unit.hp >= 1 && !this.btm.isRunning;
  }

  private BL.UnitPosition GetUnitPositionInTarget(
    BL.Panel panel,
    BL.UnitPosition up,
    HashSet<BL.Panel> targets)
  {
    if (up == null || targets == null || !targets.Contains(panel))
      return (BL.UnitPosition) null;
    BL.UnitPosition fieldUnit = this.env.core.getFieldUnit(panel);
    return !this.CanSelect(up, fieldUnit) ? (BL.UnitPosition) null : fieldUnit;
  }

  private void UpdateCurrentUnitPosition(BL.Panel panel)
  {
    BL.UnitPosition currentUnitPosition = this.env.core.currentUnitPosition;
    if (this.isCurrentUnitAction)
    {
      if (currentUnitPosition.unit.isPlayerControl && this.containsMovePanels(panel, currentUnitPosition))
      {
        this.MoveUnit(panel, currentUnitPosition);
      }
      else
      {
        if (this.isDispositionMode || currentUnitPosition.isActionComleted || this.isUnitScrollDragging)
          return;
        BattleUIController controller = this.battleManager.getController<BattleUIController>();
        if (!controller.uiButtonEnable)
          return;
        BL.UnitPosition positionInTarget1 = this.GetUnitPositionInTarget(panel, currentUnitPosition, BattleFuncs.getAttackTargetPanels(currentUnitPosition));
        if (positionInTarget1 != null)
        {
          this.env.core.lookDirection(currentUnitPosition, positionInTarget1);
          controller.startPreDuel(currentUnitPosition, positionInTarget1);
        }
        BL.UnitPosition positionInTarget2 = this.GetUnitPositionInTarget(panel, currentUnitPosition, BattleFuncs.getHealTargetPanels(currentUnitPosition));
        if (positionInTarget2 == null)
          return;
        this.env.core.lookDirection(currentUnitPosition, positionInTarget2);
        controller.startHeal(currentUnitPosition, positionInTarget2);
      }
    }
    else
    {
      BL.UnitPosition unitPosition = (BL.UnitPosition) null;
      BL.UnitPosition[] fieldUnits = this.env.core.getFieldUnits(panel.row, panel.column, includeJumping: true);
      if (fieldUnits != null)
        unitPosition = ((IEnumerable<BL.UnitPosition>) fieldUnits).FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => !x.unit.isFacility || x.unit.facility.isView)) ?? ((IEnumerable<BL.UnitPosition>) fieldUnits).FirstOrDefault<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.unit.isFacility && !x.unit.facility.isView));
      if (!this.isDispositionMode)
      {
        if (this.env.core.phaseState.state == BL.Phase.player)
        {
          foreach (BL.UnitPosition up in this.env.core.unitPositions.value)
          {
            if (up.isLocalMoved)
              up.cancelMove(this.env, (Action) (() => this.env.core.unitCurrent.commit()));
          }
        }
        this.env.core.fieldCurrent.value = panel;
        if (unitPosition == null || !unitPosition.unit.isView || unitPosition.unit.isDead)
          return;
        this.btm.setCurrentUnitByPlayerInput(unitPosition.unit);
      }
      else
      {
        if (unitPosition == null || unitPosition.unit.isFacility)
          return;
        this.env.core.fieldCurrent.value = panel;
        this.btm.setCurrentUnitByPlayerInput(unitPosition.unit);
      }
    }
  }

  private void MoveUnit(BL.Panel panel, BL.UnitPosition up)
  {
    List<BL.Panel> moveRoute = this.env.core.getRouteNonCache(up, this.env.core.getFieldPanel(up), panel, this.getMovePanels(up));
    double num = (double) up.calcMoveTime(moveRoute.Count, this.battleManager.defaultUnitSpeed);
    this.btm.setTargetBeforeMoveUnit(moveRoute.First<BL.Panel>(), (Action) (() =>
    {
      up.startMoveRoute(moveRoute, this.battleManager.defaultUnitSpeed, this.env);
      this.btm.setTargetMoveUnit(moveRoute.Last<BL.Panel>(), up.calcMoveTime(moveRoute.Count, this.battleManager.defaultUnitSpeed));
    }));
  }

  private bool IsClickedSamePanel(BL.Panel panel, BL.UnitPosition up)
  {
    return this.env.core.isTouchWait.value && this.battleManager.getUiNode().IsEnabledCommandWait && up != null && !(up.unit == (BL.Unit) null) && !this.env.core.isCompleted(up.unit) && up.unit.isPlayerControl && !(this.env.core.unitCurrent.unit == (BL.Unit) null) && !(this.env.core.unitCurrent.unit != up.unit) && panel == this.lastClickedPanel;
  }

  private void AddClickedSamePanelCounter()
  {
    if (this.env.core.unitPositions.value.Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (x => x.isMoving(this.env))))
      return;
    ++this.clickedSamePanelCounter;
    this.UpdateClickedSamePanelCounter();
  }

  private void ResetClickedSamePanelCounter()
  {
    if (this.clickedSamePanelCounter == 0)
      return;
    this.clickedSamePanelCounter = 0;
    this.UpdateClickedSamePanelCounter();
  }

  private void UpdateClickedSamePanelCounter()
  {
    if (this.clickedSamePanelCounter == 1)
    {
      if (this.lastClickedPanel != null)
        this.objectManager.showWaitButton(this.lastClickedPanel);
    }
    else
    {
      foreach (BL.Panel movePanel in this.env.core.currentUnitPosition.movePanels)
        this.objectManager.hideWaitButton(movePanel);
      if (this.lastClickedPanel != null)
        this.objectManager.hideWaitButton(this.lastClickedPanel);
    }
    if (this.clickedSamePanelCounter <= 1)
      return;
    if (this.battleManager.getUiNode().IsEnabledCommandWait)
      this.battleManager.getUiNode().CommandWait.onClick();
    this.clickedSamePanelCounter = 0;
  }

  private void UpdateLastClickedPanel(BL.Panel panel)
  {
    if (panel == null || !this.env.core.currentUnitPosition.movePanels.Any<BL.Panel>((Func<BL.Panel, bool>) (x => x == panel)))
    {
      this.ResetClickedSamePanelCounter();
    }
    else
    {
      if (panel != this.lastClickedPanel && this.lastClickedPanel != null)
        this.ResetClickedSamePanelCounter();
      this.lastClickedPanel = panel;
    }
  }

  private void UpdateLastClickedPanelByCurrentUnitChanged()
  {
    if (!this.currentUnitForLastClickedPanel.isChangedOnce())
      return;
    this.ResetClickedSamePanelCounter();
    if (this.env.core.unitCurrent.unit == (BL.Unit) null || this.mDownPanel != null)
      this.lastClickedPanel = (BL.Panel) null;
    else
      this.lastClickedPanel = this.env.core.fieldCurrent.value;
  }

  private void UpdateLastClickedPanelByModifiedUI()
  {
    Battle01SelectNode uiNode = this.battleManager?.getUiNode();
    if (Object.op_Equality((Object) uiNode, (Object) null) || uiNode.IsEnabledCommandWait)
      return;
    this.ResetClickedSamePanelCounter();
  }

  private void UpdateDispositionMode()
  {
    if (!this.isDispositionMode)
      return;
    if (this.currentUnitForDispositionMode.isChangedOnce() && this.unitForDispositionMode != (BL.Unit) null)
      this.ResetDispositionModeUnit(this.unitForDispositionMode);
    this.unitForDispositionMode = this.currentUnitForDispositionMode.value.unit;
  }

  private void ResetDispositionModeUnit(BL.Unit unit)
  {
    if (!unit.isPlayerForce)
      return;
    if (this.battleManager.isGvg)
    {
      GvgStageFormation gvgStageFormation = ((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).First<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage.ID == this.env.core.stage.stage.ID && x.player_order == Singleton<GVGManager>.GetInstance().playerOrder));
      this.ResetDispositionModeUnit(unit, gvgStageFormation.initial_direction);
    }
    else
    {
      PvpStageFormation pvpStageFormation = ((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).First<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.stage.ID == this.env.core.stage.stage.ID && x.player_order == Singleton<PVPManager>.GetInstance().playerOrder));
      this.ResetDispositionModeUnit(unit, pvpStageFormation.initial_direction);
    }
  }

  private void ResetDispositionModeUnit(BL.Unit unit, float initial_direction)
  {
    BL.UnitPosition up = this.env.core.getUnitPosition(unit);
    if (up.isLocalMoved)
      up.cancelMove(this.env, (Action) (() => up.direction = initial_direction));
    else
      up.direction = initial_direction;
  }

  private void OnClick()
  {
    if (!this.isTouchEnable || !this.battleManager.isBattleEnable || this.isUnitScrollDragging || this.checkCancelClick != null && this.checkCancelClick())
      return;
    BL.Panel panel = NGBattle3DObjectManager.hitPanel(UICamera.lastTouchPosition);
    if (panel != null)
      this.OnClick_(panel);
    this.UpdateLastClickedPanel(panel);
  }

  private void OnDoubleClick()
  {
  }

  private void OnSelect(bool selected)
  {
    if (!this.battleManager.isBattleEnable)
      return;
    this.onCancel();
  }

  private void OnDrag(Vector2 delta)
  {
    if (!this.battleManager.isBattleEnable)
      return;
    if (this.isCameraMoveMode)
      this.cameraController.onDrag(delta);
    this.firstDownPanel = (BL.Panel) null;
  }

  private void OnDrop(GameObject go)
  {
  }

  private void OnInput(string text)
  {
  }

  private void OnSubmit()
  {
  }

  private void OnScroll(float delta)
  {
  }

  private enum Mode
  {
    none,
    targetselect,
  }

  private class SelectTarget
  {
    public List<BL.Unit> targets;
    public List<BL.Unit> grayTargets;
    public List<BL.Unit> attackTargets;
    public List<BL.Unit> healTargets;
    public List<BL.Panel> panels;
    public Action<BL.Unit, BL.Panel> selectFunc;

    public SelectTarget(
      List<BL.Unit> attackTargets,
      List<BL.Unit> healTargets,
      List<BL.Unit> grayTargets,
      List<BL.Panel> panels,
      Action<BL.Unit, BL.Panel> selectFunc)
    {
      this.attackTargets = attackTargets;
      this.healTargets = healTargets;
      this.targets = attackTargets.Concat<BL.Unit>((IEnumerable<BL.Unit>) healTargets).Except<BL.Unit>((IEnumerable<BL.Unit>) grayTargets).ToList<BL.Unit>();
      this.grayTargets = grayTargets;
      this.panels = panels;
      this.selectFunc = selectFunc;
    }
  }
}
