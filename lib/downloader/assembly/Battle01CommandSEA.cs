// Decompiled with JetBrains decompiler
// Type: Battle01CommandSEA
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01CommandSEA : BattleMonoBehaviour, IButtonEnableBeheviour
{
  private Battle01SelectNode selectNode;
  private BL.BattleModified<BL.CurrentUnit> currentModified;
  private BL.BattleModified<BL.UnitPosition> unitPositionModified;
  private BL.BattleModified<BL.StructValue<bool>> waitAIActionCancelModified;
  private UIButton button;
  private bool mButtonEnable;
  private BL.Unit currentUnitLog;

  public bool buttonEnable
  {
    set
    {
      this.mButtonEnable = value;
      bool flag = this.isUsableSEASkill();
      ((UIButtonColor) this.button).isEnabled = ((!this.mButtonEnable || this.env.core.currentUnitPosition.isActionComleted ? 0 : (!this.battleManager.getController<BattleStateController>().isWaitCurrentAIActionCancel ? 1 : 0)) & (flag ? 1 : 0)) != 0;
      this.currentUnitLog = (BL.Unit) null;
      if (!flag)
        return;
      this.currentUnitLog = this.env.core.unitCurrent.unit;
    }
  }

  private bool isUsableSEASkill()
  {
    if (this.env.core.unitCurrent.unit == (BL.Unit) null)
      return false;
    BL.Skill seaSkill = this.env.core.unitCurrent.unit.SEASkill;
    if (seaSkill == null)
      return false;
    int? remain = seaSkill.remain;
    if (remain.HasValue)
    {
      remain = seaSkill.remain;
      int num = 0;
      if (remain.GetValueOrDefault() <= num & remain.HasValue)
        return false;
    }
    return !this.env.core.unitCurrent.unit.IsDontUseSEA(seaSkill);
  }

  public override IEnumerator onInitAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01CommandSEA behaviour = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    behaviour.button = ((Component) behaviour).GetComponent<UIButton>();
    EventDelegate.Set(behaviour.button.onClick, new EventDelegate((MonoBehaviour) behaviour, "onClick"));
    behaviour.battleManager.getManager<NGBattleUIManager>().controller.setButtonBehaviour((IButtonEnableBeheviour) behaviour);
    return false;
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Battle01CommandSEA battle01CommandSea = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battle01CommandSea.selectNode = NGUITools.FindInParents<Battle01SelectNode>(((Component) battle01CommandSea).transform);
    battle01CommandSea.currentModified = BL.Observe<BL.CurrentUnit>(battle01CommandSea.env.core.unitCurrent);
    battle01CommandSea.unitPositionModified = BL.Observe<BL.UnitPosition>(battle01CommandSea.env.core.currentUnitPosition);
    battle01CommandSea.waitAIActionCancelModified = BL.Observe<BL.StructValue<bool>>(battle01CommandSea.battleManager.getController<BattleStateController>().instWaitCurrentAIActionCancel);
    return false;
  }

  protected override void Update_Battle()
  {
    if (!this.battleManager.isBattleEnable)
      return;
    int num1 = this.currentModified.isChangedOnce() ? 1 : 0;
    BL.BattleModified<BL.UnitPosition> positionModified = this.unitPositionModified;
    bool flag1 = positionModified != null && positionModified.isChangedOnce();
    bool flag2 = this.waitAIActionCancelModified.isChangedOnce();
    int num2 = flag1 ? 1 : 0;
    if ((num1 | num2 | (flag2 ? 1 : 0)) == 0 || !Object.op_Inequality((Object) this.button, (Object) null))
      return;
    bool flag3 = this.IsEnabledSEASkillButton();
    ((UIButtonColor) this.button).isEnabled = flag3;
    this.currentUnitLog = (BL.Unit) null;
    if (!flag3)
      return;
    this.currentUnitLog = this.env.core.unitCurrent.unit;
  }

  private bool IsEnabledSEASkillButton()
  {
    return this.mButtonEnable && !this.env.core.currentUnitPosition.isActionComleted && !this.waitAIActionCancelModified.value.value && this.isUsableSEASkill();
  }

  public void onClick()
  {
    if (!this.battleManager.isBattleEnable || this.waitAIActionCancelModified.value.value)
      return;
    BattleInputObserver controller = this.battleManager.getController<BattleInputObserver>();
    if (Object.op_Inequality((Object) controller, (Object) null) && controller.isUnitScrollDragging)
      return;
    BL.Unit currentUnitLog = this.currentUnitLog;
    if (currentUnitLog == (BL.Unit) null || currentUnitLog != this.env.core.unitCurrent.unit || !(currentUnitLog != (BL.Unit) null) || this.env.core.getUnitPosition(currentUnitLog).isCompleted || this.env.core.getUnitPosition(currentUnitLog).isActionComleted)
      return;
    this.selectNode.useSEA(currentUnitLog, currentUnitLog.SEASkill);
  }

  public void resetCurrentUnitPosition(bool bClear = false)
  {
    this.unitPositionModified = bClear ? (BL.BattleModified<BL.UnitPosition>) null : BL.Observe<BL.UnitPosition>(this.env.core.currentUnitPosition);
  }

  private void CreateTouchGuard()
  {
    GameObject gameObject = new GameObject("touch_guard", new System.Type[2]
    {
      typeof (UIWidget),
      typeof (BoxCollider)
    });
    gameObject.transform.SetParent(((Component) this).transform, false);
    BoxCollider component1 = ((Component) this).GetComponent<BoxCollider>();
    BoxCollider component2 = gameObject.GetComponent<BoxCollider>();
    component2.size = component1.size;
    component2.center = component1.center;
    ((Collider) component2).isTrigger = ((Collider) component1).isTrigger;
    UIWidget component3 = ((Component) this).GetComponent<UIWidget>();
    UIWidget component4 = gameObject.GetComponent<UIWidget>();
    component4.depth = component3.depth - 1;
    component4.autoResizeBoxCollider = true;
    component4.SetDimensions(component3.width, component3.height);
  }
}
