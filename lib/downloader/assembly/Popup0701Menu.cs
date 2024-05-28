// Decompiled with JetBrains decompiler
// Type: Popup0701Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup0701Menu : BackButtonMonoBehaiviour
{
  private Action<bool> mCloseCallback;
  private GameObject popup2;
  private bool pushEnable;

  public void Init(Action<bool> closeCallBack, GameObject popup0702)
  {
    this.popup2 = popup0702;
    this.mCloseCallback = closeCallBack;
    this.pushEnable = false;
  }

  public IEnumerator pushOnWait()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.pushEnable = true;
  }

  private void OnEnable() => this.StartCoroutine(this.pushOnWait());

  public void onYes()
  {
    if (!this.pushEnable)
      return;
    this.pushEnable = false;
    this.Save();
  }

  private void Save()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    this.mCloseCallback(false);
  }

  public void onNo()
  {
    if (!this.pushEnable)
      return;
    this.pushEnable = false;
    Singleton<PopupManager>.GetInstance().open(this.popup2).GetComponent<Popup0702Menu>().Init(this.mCloseCallback);
  }

  public override void onBackButton() => this.onNo();
}
