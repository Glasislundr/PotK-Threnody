// Decompiled with JetBrains decompiler
// Type: BattleUnitController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUnitController : BattleMonoBehaviour
{
  private BattleTimeManager btm;
  private BL.BattleModified<BL.CurrentUnit> currentUnitModified;
  private BL.BattleModified<BL.UnitPosition> unitPositionModified;
  private BattleCameraController cameraController;
  private BattleInputObserver inputObserver;

  private bool isCameraMoveMode => this.inputObserver.isCameraMoveMode;

  private bool isDispositionMode => this.inputObserver.isDispositionMode;

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleUnitController battleUnitController = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleUnitController.btm = battleUnitController.battleManager.getManager<BattleTimeManager>();
    battleUnitController.currentUnitModified = BL.Observe<BL.CurrentUnit>(battleUnitController.env.core.unitCurrent);
    battleUnitController.unitPositionModified = BL.Observe<BL.UnitPosition>(battleUnitController.env.core.currentUnitPosition);
    battleUnitController.cameraController = ((Component) battleUnitController).GetComponent<BattleCameraController>();
    battleUnitController.inputObserver = ((Component) battleUnitController).GetComponent<BattleInputObserver>();
    return false;
  }

  protected override void LateUpdate_Battle()
  {
    if (!this.battleManager.isBattleEnable)
      return;
    bool flag = false;
    if (this.currentUnitModified.isChangedOnce() && this.currentUnitModified.value.unit != (BL.Unit) null)
    {
      this.unitPositionModified.value = this.env.core.currentUnitPosition;
      this.unitPositionModified.notifyChanged();
      flag = true;
    }
    if (this.isDispositionMode || !this.unitPositionModified.isChangedOnce() || this.isCameraMoveMode || !(this.env.core.unitCurrent.unit != (BL.Unit) null) || this.battleManager.isDeadEffectPlaying || this.env.core.unitCurrent.unit.skillEffects.IsMoveSkillActionWaiting() && this.btm.isRunning || flag && this.env.core.unitCurrent.IsPlayerInput)
      return;
    BL.Panel panel = this.env.core.getFieldPanel(this.unitPositionModified.value);
    if (!flag)
      return;
    this.btm.setScheduleAction((Action) (() => this.cameraController.setLookAtTarget(panel)));
  }

  public bool CanMoveUnit(BL.UnitPosition up)
  {
    return !this.env.core.isAutoBattle.value && up.unit.isPlayerControl && !up.isCompleted && this.env.core.currentPhaseUnitp(up);
  }

  public bool isMoveUnit(GameObject o)
  {
    BL.Unit unit = this.env.core.unitCurrent.unit;
    return this.battleManager.isBattleEnable && unit != (BL.Unit) null && Object.op_Equality((Object) this.env.unitResource[unit].gameObject, (Object) o) && !this.env.core.isCompleted(unit) && this.env.core.currentPhaseUnitp(unit) && !unit.IsDontMove;
  }
}
