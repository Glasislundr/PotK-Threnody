// Decompiled with JetBrains decompiler
// Type: Battle01ItemUseSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Battle01ItemUseSelect : BattleHorizontalSelect<BL.Unit>
{
  [SerializeField]
  private Battle01ItemSubject itemSubject;

  protected override void initialize(BE e)
  {
    this.itemSubject = NGUITools.FindInParents<Battle01ItemSubject>(((Component) this).transform);
  }

  protected override Future<GameObject> resPrefab()
  {
    return this.battleManager.isSea ? Res.Prefabs.battle.Battle01_Player_Unit_sea.Load<GameObject>() : Res.Prefabs.battle.Battle01_Player_Unit.Load<GameObject>();
  }

  protected override void setParts(GameObject o, BL.Unit parts)
  {
    Battle01PlayerUnit component = o.GetComponent<Battle01PlayerUnit>();
    component.setUnit(parts);
    component.isViewCounter = false;
  }

  private void onSelect(BL.Unit unit, BL.Panel panel)
  {
    if (unit == (BL.Unit) null)
      return;
    this.itemSubject.useUnit(unit);
  }

  public override void onClick()
  {
    if (!this.battleManager.isBattleEnable || this.modified == null)
      return;
    this.onSelect(NGUITools.FindInParents<Battle01PlayerUnit>(UICamera.selectedObject).getUnit(), (BL.Panel) null);
  }

  public void setTargets(List<BL.Unit> targets, bool isHeal)
  {
    this.modified = BL.Observe<BL.ClassValue<List<BL.Unit>>>(new BL.ClassValue<List<BL.Unit>>(targets));
    List<BL.Unit> attackTargets;
    List<BL.Unit> healTargets;
    if (isHeal)
    {
      attackTargets = new List<BL.Unit>();
      healTargets = targets;
    }
    else
    {
      attackTargets = targets;
      healTargets = new List<BL.Unit>();
    }
    Singleton<NGBattleManager>.GetInstance().getController<BattleInputObserver>().setTargetSelectMode(attackTargets, healTargets, new List<BL.Unit>(), new List<BL.Panel>(), new Action<BL.Unit, BL.Panel>(this.onSelect));
  }
}
