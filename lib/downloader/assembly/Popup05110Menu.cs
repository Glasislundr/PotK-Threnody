// Decompiled with JetBrains decompiler
// Type: Popup05110Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup05110Menu : BackButtonMenuBase
{
  public override void onBackButton() => this.IbtnClose();

  public void IbtnClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnReset()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.StartCoroutine(this.ShowDetaResetPopup());
  }

  private IEnumerator ShowDetaResetPopup()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_051_11__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result.Clone();
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }
}
