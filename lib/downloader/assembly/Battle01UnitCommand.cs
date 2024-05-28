// Decompiled with JetBrains decompiler
// Type: Battle01UnitCommand
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
public class Battle01UnitCommand : NGBattleMenuBase
{
  [SerializeField]
  private SelectParts selectParts;
  private SelectParts completeSelectParts;
  private Battle01CommandNode[] nodes_;
  private Battle01CommandNode.CommandFlag[] partsAttribute_;
  private BL.ForceID forceId;
  private BL.BattleModified<BL.CurrentUnit> currentUnitModified;
  private BL.BattleModified<BL.Unit> unitModified;
  private int index_log;

  public override IEnumerator onInitAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01UnitCommand battle01UnitCommand = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01UnitCommand.completeSelectParts = ((Component) battle01UnitCommand).GetComponent<SelectParts>();
    battle01UnitCommand.currentUnitModified = BL.Observe<BL.CurrentUnit>(battle01UnitCommand.env.core.unitCurrent);
    battle01UnitCommand.forceId = BL.ForceID.none;
    battle01UnitCommand.nodes_ = ((IEnumerable<NGTweenParts>) battle01UnitCommand.selectParts.objects).Select<NGTweenParts, Battle01CommandNode>((Func<NGTweenParts, Battle01CommandNode>) (p => ((Component) p).GetComponent<Battle01CommandNode>())).Where<Battle01CommandNode>((Func<Battle01CommandNode, bool>) (cn => Object.op_Inequality((Object) cn, (Object) null))).ToArray<Battle01CommandNode>();
    if (battle01UnitCommand.nodes_.Length == battle01UnitCommand.selectParts.objects.Length)
    {
      battle01UnitCommand.partsAttribute_ = ((IEnumerable<Battle01CommandNode>) battle01UnitCommand.nodes_).Select<Battle01CommandNode, Battle01CommandNode.CommandFlag>((Func<Battle01CommandNode, Battle01CommandNode.CommandFlag>) (c => c.ActiveCommands)).ToArray<Battle01CommandNode.CommandFlag>();
    }
    else
    {
      battle01UnitCommand.nodes_ = (Battle01CommandNode[]) null;
      battle01UnitCommand.partsAttribute_ = (Battle01CommandNode.CommandFlag[]) null;
    }
    return false;
  }

  protected override void LateUpdate_Battle()
  {
    if (!this.currentUnitModified.isChangedOnce() && (this.unitModified == null || !this.unitModified.isChangedOnce()))
      return;
    this.selectCurrentUnitCommand(this.currentUnitModified.value.unit);
  }

  private void selectCurrentUnitCommand(BL.Unit unit)
  {
    if (unit == (BL.Unit) null)
    {
      this.forceId = BL.ForceID.none;
      this.unitModified = (BL.BattleModified<BL.Unit>) null;
    }
    else
    {
      if (this.unitModified == null || unit != this.unitModified.value)
      {
        this.unitModified = BL.Observe<BL.Unit>(unit);
        this.unitModified.isChangedOnce();
      }
      BL.ForceID forceId = this.env.core.getForceID(unit);
      bool flag1 = this.forceId != forceId;
      this.forceId = forceId;
      if (this.env.core.isCompleted(unit))
      {
        this.completeSelectParts.setValue(1);
      }
      else
      {
        this.completeSelectParts.setValue(0);
        if (this.partsAttribute_ == null)
          return;
        bool flag2 = false;
        Battle01CommandNode.CommandFlag commandFlag = Battle01CommandNode.CommandFlag.Wait;
        if (!unit.IsDontAction)
        {
          if (unit.hasOugi && (!unit.CantChangeCurrent || !unit.IsDontUseOugi((BL.Skill) null)))
            commandFlag |= Battle01CommandNode.CommandFlag.Ougi;
          if ((!unit.CantChangeCurrent || !unit.IsDontUseCommand((BL.Skill) null)) && unit.hasMapSkill)
            commandFlag |= Battle01CommandNode.CommandFlag.Skill;
          if (!unit.crippled && (!unit.CantChangeCurrent || !unit.IsDontUseSEA((BL.Skill) null)))
            flag2 = unit.playerUnit.isUnlockedSEASkill;
        }
        int v = Array.IndexOf<Battle01CommandNode.CommandFlag>(this.partsAttribute_, commandFlag);
        if (v < 0)
          v = Array.IndexOf<Battle01CommandNode.CommandFlag>(this.partsAttribute_, Battle01CommandNode.CommandFlag.Wait | Battle01CommandNode.CommandFlag.SEA);
        if (flag1)
        {
          this.selectParts.setFirstValue(v);
          this.index_log = v;
        }
        else
          this.selectParts.setValue(v);
        for (int index = 0; index < this.nodes_.Length; ++index)
        {
          this.nodes_[index].resetCurrentUnitPosition(index != v);
          bool isDelay = !flag2 && this.index_log != v && index == this.index_log;
          this.nodes_[index].setSEAButtonActive(flag2, isDelay);
        }
        this.index_log = v;
      }
    }
  }
}
