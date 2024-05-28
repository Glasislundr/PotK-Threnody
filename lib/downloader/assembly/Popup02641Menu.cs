// Decompiled with JetBrains decompiler
// Type: Popup02641Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using UnityEngine;

#nullable disable
public class Popup02641Menu : BackButtonMonoBehaiviour
{
  [SerializeField]
  private UILabel txt_Title;
  [SerializeField]
  private UILabel txt_Description;
  private Action ok;
  private Action no;

  public void Initialize(Action ok, Action no)
  {
    Consts instance = Consts.GetInstance();
    this.SetText(instance.VERSUS_002641POPUP_TITLE, instance.VERSUS_002641POPUP_DESCRIPTION);
    this.ok = ok;
    this.no = no;
  }

  private void SetText(string t, string d)
  {
    this.txt_Title.SetText(t);
    this.txt_Description.SetText(d);
  }

  public void IbtnOk()
  {
    Singleton<PopupManager>.GetInstance().onDismiss(true);
    if (this.ok == null)
      return;
    this.ok();
  }

  public void IbtnNo()
  {
    if (this.no == null)
      return;
    this.no();
  }

  public override void onBackButton() => this.IbtnNo();
}
