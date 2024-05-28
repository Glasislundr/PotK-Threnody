// Decompiled with JetBrains decompiler
// Type: Tower029UnitRestartSelectionRecoveryPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Tower029UnitRestartSelectionRecoveryPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel lblPopupTitle;
  [SerializeField]
  private UILabel lblPopupDesc;
  private Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType> actionUnitSelection;
  private TowerProgress progress;
  private TowerUtil.SequenceType sequenceType;

  public void Initialize(
    TowerProgress progress,
    Action<TowerUtil.UnitSelectionMode, TowerUtil.SequenceType> action,
    TowerUtil.SequenceType sequenceType)
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.actionUnitSelection = action;
    this.sequenceType = sequenceType;
    this.progress = progress;
  }

  public void onAutoButton()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.actionUnitSelection == null)
      return;
    this.actionUnitSelection(TowerUtil.UnitSelectionMode.Auto, this.sequenceType);
  }

  public void onManualButton()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.actionUnitSelection == null)
      return;
    this.actionUnitSelection(TowerUtil.UnitSelectionMode.Manual, this.sequenceType);
  }

  public void onOkButton()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Tower029SupplyEditScene.ChangeScene(((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (u => u.tower_is_entry)).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (u => u.id)).ToArray<int>(), this.progress, this.sequenceType);
  }

  public override void onBackButton()
  {
  }
}
