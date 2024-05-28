// Decompiled with JetBrains decompiler
// Type: Battle01PlayerSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Battle01PlayerSelect : BattleHorizontalSelect<BL.Unit>
{
  protected override void initialize(BE e)
  {
    this.modified = BL.Observe<BL.ClassValue<List<BL.Unit>>>(e.core.playerUnits);
  }

  protected override Future<GameObject> resPrefab()
  {
    return this.battleManager.isSea ? Res.Prefabs.battle.Battle01_Player_Unit_sea.Load<GameObject>() : Res.Prefabs.battle.Battle01_Player_Unit.Load<GameObject>();
  }

  protected override void setParts(GameObject o, BL.Unit parts)
  {
    Battle01PlayerUnit component = o.GetComponent<Battle01PlayerUnit>();
    component.setUnit(parts);
    component.isViewCounter = true;
  }

  public override void onClick()
  {
    if (!this.battleManager.isBattleEnable || this.env.core.isAutoBattle.value || this.battleManager.getController<BattleStateController>().isWaitCurrentAIActionCancel || this.env.core.unitPositions.value.Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (up => up.isMoving(this.env))))
      return;
    Battle01PlayerUnit inParents = NGUITools.FindInParents<Battle01PlayerUnit>(UICamera.selectedObject);
    if (!Object.op_Inequality((Object) inParents, (Object) null))
      return;
    BL.Unit unit = inParents.getUnit();
    if (!(unit != (BL.Unit) null) || unit.isDead || !(unit != this.env.core.unitCurrent.unit))
      return;
    if (this.env.core.phaseState.turnCount == 1)
      this.env.unitResource[unit].PlayVoiceDuelStart(unit);
    this.battleManager.getManager<BattleTimeManager>().setCurrentUnit(unit);
    this.battleManager.StartCoroutine(this.doSelectMask());
  }

  private IEnumerator doSelectMask()
  {
    CommonRoot cr = Singleton<CommonRoot>.GetInstance();
    cr.isTouchBlock = true;
    yield return (object) new WaitForSeconds(0.1f);
    cr.isTouchBlock = false;
  }
}
