// Decompiled with JetBrains decompiler
// Type: Battle01CommandOugi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01CommandOugi : NGBattleMenuBase, IButtonEnableBeheviour
{
  [SerializeField]
  private SelectParts selectParts;
  [SerializeField]
  private UILabel turnLabel;
  [SerializeField]
  private GameObject DisableRootObject;
  [SerializeField]
  private GameObject LoadingRootObject;
  [SerializeField]
  private GameObject sprite_turn_left_for_secret_base;
  private Battle01SelectNode selectNode;
  private BL.BattleModified<BL.CurrentUnit> currentModified;
  private BL.BattleModified<BL.PhaseState> phaseModified;
  private BL.BattleModified<BL.UnitPosition> unitPositionModified;
  private BL.Unit currentUnitLog;
  private UIButton[] buttons;

  public bool buttonEnable
  {
    set
    {
      foreach (UIButtonColor button in this.buttons)
        button.isEnabled = value;
    }
  }

  public override IEnumerator onInitAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01CommandOugi behaviour = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    behaviour.buttons = ((Component) behaviour).GetComponentsInChildren<UIButton>(true);
    EventDelegate eventDelegate = new EventDelegate((MonoBehaviour) behaviour, "onClick");
    foreach (UIButton button in behaviour.buttons)
      EventDelegate.Set(button.onClick, eventDelegate);
    behaviour.battleManager.getManager<NGBattleUIManager>().controller.setButtonBehaviour((IButtonEnableBeheviour) behaviour);
    return false;
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01CommandOugi battle01CommandOugi = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01CommandOugi.selectNode = NGUITools.FindInParents<Battle01SelectNode>(((Component) battle01CommandOugi).transform);
    battle01CommandOugi.currentModified = BL.Observe<BL.CurrentUnit>(battle01CommandOugi.env.core.unitCurrent);
    battle01CommandOugi.phaseModified = BL.Observe<BL.PhaseState>(battle01CommandOugi.env.core.phaseState);
    battle01CommandOugi.unitPositionModified = BL.Observe<BL.UnitPosition>(battle01CommandOugi.env.core.currentUnitPosition);
    return false;
  }

  protected override void Update_Battle()
  {
    int num1 = this.currentModified.isChangedOnce() ? 1 : 0;
    bool flag1 = this.phaseModified.isChangedOnce();
    BL.BattleModified<BL.UnitPosition> positionModified = this.unitPositionModified;
    bool flag2 = positionModified != null && positionModified.isChangedOnce();
    int num2 = flag1 ? 1 : 0;
    if ((num1 | num2 | (flag2 ? 1 : 0)) == 0)
      return;
    this.currentUnitLog = (BL.Unit) null;
    BL.Unit unit = this.currentModified.value.unit;
    if (!(unit != (BL.Unit) null) || unit.ougi == null)
      return;
    int v = unit.ougi.useTurn - this.phaseModified.value.absoluteTurnCount;
    if (!BattleFuncs.checkUseSkillInvokeGameMode((BL.ISkillEffectListUnit) unit, unit.ougi, false) || !BattleFuncs.checkUseOugiSkillMaxCountInDeck((BL.ISkillEffectListUnit) unit, unit.ougi))
    {
      this.selectParts.setValue(1);
      this.DisableRootObject.SetActive(false);
      this.LoadingRootObject.SetActive(false);
      this.sprite_turn_left_for_secret_base.SetActive(false);
    }
    else if (unit.ougi.skill.target_type == BattleskillTargetType.myself && unit.skillEffects.CanUseSkill(unit.ougi.skill, unit.ougi.level, (BL.ISkillEffectListUnit) unit, this.env.core, (BL.ISkillEffectListUnit) unit, unit.ougi.nowUseCount) == 1)
    {
      this.selectParts.setValue(1);
      this.DisableRootObject.SetActive(false);
      this.LoadingRootObject.SetActive(true);
      this.sprite_turn_left_for_secret_base.SetActive(true);
    }
    else if (v <= 0)
    {
      if (!this.env.core.currentUnitPosition.isActionComleted)
      {
        if (unit.ougi.remain.HasValue)
        {
          int? remain = unit.ougi.remain;
          int num3 = 0;
          if (remain.GetValueOrDefault() <= num3 & remain.HasValue)
            goto label_11;
        }
        if (!unit.IsDontUseOugi(unit.ougi) && unit.ougi.canUseTurn(this.phaseModified.value.absoluteTurnCount))
        {
          this.selectParts.setValue(0);
          this.currentUnitLog = unit;
          return;
        }
      }
label_11:
      this.selectParts.setValue(1);
      this.DisableRootObject.SetActive(false);
      this.LoadingRootObject.SetActive(false);
      this.sprite_turn_left_for_secret_base.SetActive(false);
    }
    else
    {
      this.sprite_turn_left_for_secret_base.SetActive(true);
      this.DisableRootObject.SetActive(true);
      this.LoadingRootObject.SetActive(false);
      this.selectParts.setValue(1);
      this.setText(this.turnLabel, v);
    }
  }

  public void onClick()
  {
    if (!this.battleManager.isBattleEnable || this.battleManager.getController<BattleStateController>().isWaitCurrentAIActionCancel)
      return;
    BattleInputObserver controller = this.battleManager.getController<BattleInputObserver>();
    if (Object.op_Inequality((Object) controller, (Object) null) && controller.isUnitScrollDragging)
      return;
    BL.Unit currentUnitLog = this.currentUnitLog;
    if (currentUnitLog == (BL.Unit) null || currentUnitLog != this.env.core.unitCurrent.unit || !(currentUnitLog != (BL.Unit) null) || currentUnitLog.ougi == null || !Object.op_Inequality((Object) this.selectNode, (Object) null) || this.env.core.getUnitPosition(currentUnitLog).isCompleted || this.env.core.getUnitPosition(currentUnitLog).isActionComleted || currentUnitLog.ougi.useTurn - this.phaseModified.value.absoluteTurnCount > 0)
      return;
    int? remain = currentUnitLog.ougi.remain;
    if (remain.HasValue)
    {
      remain = currentUnitLog.ougi.remain;
      if (!remain.HasValue)
        return;
      remain = currentUnitLog.ougi.remain;
      int num = 0;
      if (!(remain.GetValueOrDefault() > num & remain.HasValue))
        return;
    }
    if (currentUnitLog.IsDontUseOugi(currentUnitLog.ougi) || !currentUnitLog.ougi.canUseTurn(this.phaseModified.value.absoluteTurnCount))
      return;
    this.selectNode.useOugi(currentUnitLog, currentUnitLog.ougi);
  }

  public void resetCurrentUnitPosition(bool bClear = false)
  {
    this.unitPositionModified = bClear ? (BL.BattleModified<BL.UnitPosition>) null : BL.Observe<BL.UnitPosition>(this.env.core.currentUnitPosition);
  }
}
