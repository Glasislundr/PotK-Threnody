// Decompiled with JetBrains decompiler
// Type: Unit00499ReincarnationChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Unit00499ReincarnationChange : NGMenuBase
{
  private const int PanelDepthFront = 10;
  private const int PanelDepthBack = 9;
  public Unit00499UnitStatus AfterUnit;
  public Unit00499UnitStatus RecordUnit;
  public UIPanel after;
  public UIPanel record;
  public Animator animator;
  private Action afterAction;
  private Action recordAction;
  [SerializeField]
  private UIButton afterChangeBtn;
  [SerializeField]
  private UIButton recordChangeBtn;
  private bool buttonEnable;

  public void SetActiveChangeButton(bool flag)
  {
    ((Component) this.animator).gameObject.SetActive(false);
    ((Behaviour) this.animator).enabled = flag;
    ((UIButtonColor) this.afterChangeBtn).isEnabled = flag;
    ((UIButtonColor) this.recordChangeBtn).isEnabled = flag;
    this.buttonEnable = flag;
    ((Component) this.animator).gameObject.SetActive(true);
  }

  public void SetAction(Action afterAction, Action recordAction)
  {
    this.afterAction = afterAction;
    this.recordAction = recordAction;
  }

  public void IbtnChangeRecord()
  {
    if (!this.buttonEnable)
      return;
    this.recordAction();
    this.animator.SetTrigger("is_ReinforceMemory_up");
  }

  public void IbtnChangeAfter()
  {
    if (!this.buttonEnable)
      return;
    this.afterAction();
    this.animator.SetTrigger("is_After_up");
  }

  public void AfterFront()
  {
    this.after.depth = 10;
    this.record.depth = 9;
  }

  public void RecordFront()
  {
    this.after.depth = 9;
    this.record.depth = 10;
  }
}
