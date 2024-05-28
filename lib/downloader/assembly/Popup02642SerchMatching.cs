// Decompiled with JetBrains decompiler
// Type: Popup02642SerchMatching
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
public class Popup02642SerchMatching : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtDescription;
  [SerializeField]
  private GameObject btnNo;
  private Action action;

  public void Init(Action noAction)
  {
    this.action = noAction;
    Consts instance = Consts.GetInstance();
    this.txtTitle.SetText(instance.VERSUS_002642POPUP_TITLE);
    this.txtDescription.SetText(instance.VERSUS_002642POPUP_DESCRIPTION);
  }

  public void IbtnNo() => this.action();

  public override void onBackButton() => this.IbtnNo();

  public void DisableButton()
  {
    ((UIButtonColor) this.btnNo.GetComponent<UIButton>()).isEnabled = false;
  }
}
