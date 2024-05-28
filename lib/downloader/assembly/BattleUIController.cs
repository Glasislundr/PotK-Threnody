// Decompiled with JetBrains decompiler
// Type: BattleUIController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BattleUIController : BattleMonoBehaviour
{
  private BattleInputObserver inputObserver;
  private BattleCameraController cameraController;
  private BattleUnitController unitController;
  private MaterialController materialController;
  private BattleTimeManager _btm;
  private BL.BattleModified<BL.StructValue<bool>> isViewDengerAreaModified;
  private BL.BattleModified<BL.StructValue<int>> sightModified;
  private List<IButtonEnableBeheviour> buttonBehaviours = new List<IButtonEnableBeheviour>();
  private bool mUIButtonEnable = true;

  private BattleTimeManager btm
  {
    get
    {
      if (Object.op_Equality((Object) this._btm, (Object) null))
        this._btm = this.battleManager.getManager<BattleTimeManager>();
      return this._btm;
    }
  }

  public BattleCameraController CameraController => this.cameraController;

  private void Awake()
  {
    this.inputObserver = ((Component) this).gameObject.AddComponent<BattleInputObserver>();
    this.cameraController = ((Component) this).gameObject.AddComponent<BattleCameraController>();
    this.unitController = ((Component) this).gameObject.AddComponent<BattleUnitController>();
    this.materialController = ((Component) this).gameObject.AddComponent<MaterialController>();
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleUIController battleUiController = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    battleUiController.isViewDengerAreaModified = BL.Observe<BL.StructValue<bool>>(battleUiController.env.core.isViewDengerArea);
    battleUiController.sightModified = BL.Observe<BL.StructValue<int>>(battleUiController.env.core.sight);
    return false;
  }

  protected override void LateUpdate_Battle()
  {
    if (!this.battleManager.isBattleEnable)
      return;
    if (this.isViewDengerAreaModified.isChangedOnce())
    {
      if (this.isViewDengerAreaModified.value.value)
      {
        this.env.core.createDangerAria();
        this.env.core.viewDangerAria();
      }
      else
        this.env.core.hideDangerAria();
    }
    if (!this.sightModified.isChangedOnce())
      return;
    this.cameraController.sightDistance = this.battleManager.sightDistances[this.sightModified.value.value];
  }

  public void setButtonBehaviour(IButtonEnableBeheviour behaviour)
  {
    if (this.buttonBehaviours.Contains(behaviour))
      return;
    this.buttonBehaviours.Add(behaviour);
    behaviour.buttonEnable = this.mUIButtonEnable;
  }

  public bool uiButtonEnable
  {
    get => this.mUIButtonEnable;
    set
    {
      this.mUIButtonEnable = value;
      foreach (IButtonEnableBeheviour buttonBehaviour in this.buttonBehaviours)
        buttonBehaviour.buttonEnable = value;
    }
  }

  public void uiWait()
  {
    if (this.env.core.unitCurrent.unit == (BL.Unit) null)
      return;
    BL.UnitPosition unitPosition = this.env.core.getUnitPosition(this.env.core.unitCurrent.unit);
    if (this.battleManager.useGameEngine)
    {
      this.battleManager.gameEngine.moveUnit(unitPosition);
    }
    else
    {
      this.env.core.setSomeAction();
      unitPosition.completeActionUnit(this.env.core, true);
    }
  }

  public void startPreDuel(BL.UnitPosition attack, BL.UnitPosition defense)
  {
    this.battleManager.isBattleEnable = false;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (this.battleManager.isSea)
      instance.changeScene("BattleUI_04_sea", true, (object) attack, (object) defense);
    else
      instance.changeScene("BattleUI_04", true, (object) attack, (object) defense);
  }

  public void startHeal(BL.UnitPosition src, BL.UnitPosition dst)
  {
    this.battleManager.isBattleEnable = false;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (this.battleManager.isSea)
      instance.changeScene("battle019_6_1_sea", true, (object) src, (object) dst);
    else
      instance.changeScene("battle019_6_1", true, (object) src, (object) dst);
  }
}
