// Decompiled with JetBrains decompiler
// Type: Battle01SkillUseSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Battle01SkillUseSelect : BattleHorizontalSelect<BL.Unit>
{
  [SerializeField]
  private Battle01SkillSubject skillSubject;

  protected override void initialize(BE e)
  {
    this.skillSubject = NGUITools.FindInParents<Battle01SkillSubject>(((Component) this).transform);
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
    if (unit == (BL.Unit) null && panel == null)
      return;
    this.skillSubject.useUnit(unit, panel);
  }

  public override void onClick()
  {
    if (!this.battleManager.isBattleEnable || this.modified == null)
      return;
    this.onSelect(NGUITools.FindInParents<Battle01PlayerUnit>(UICamera.selectedObject).getUnit(), (BL.Panel) null);
  }

  public void setTargets(
    BL.Unit unit,
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Unit> grayTargets,
    List<BL.Panel> panels)
  {
    this.modified = BL.Observe<BL.ClassValue<List<BL.Unit>>>(new BL.ClassValue<List<BL.Unit>>(targets));
    List<BL.Unit> list = targets.Concat<BL.Unit>((IEnumerable<BL.Unit>) grayTargets).Distinct<BL.Unit>().ToList<BL.Unit>();
    List<BL.Unit> attackTargets;
    List<BL.Unit> healTargets;
    if (skill.targetType == BattleskillTargetType.complex_range || skill.targetType == BattleskillTargetType.complex_single)
    {
      BL.ForceID forceId = this.env.core.getForceID(unit);
      attackTargets = new List<BL.Unit>();
      healTargets = new List<BL.Unit>();
      foreach (BL.Unit unit1 in list)
      {
        if (this.env.core.getForceID(unit1) == forceId)
          healTargets.Add(unit1);
        else
          attackTargets.Add(unit1);
      }
    }
    else if (skill.isOwn)
    {
      attackTargets = new List<BL.Unit>();
      healTargets = list;
    }
    else
    {
      attackTargets = list;
      healTargets = new List<BL.Unit>();
    }
    Singleton<NGBattleManager>.GetInstance().getController<BattleInputObserver>().setTargetSelectMode(attackTargets, healTargets, grayTargets, panels, new Action<BL.Unit, BL.Panel>(this.onSelect));
  }
}
