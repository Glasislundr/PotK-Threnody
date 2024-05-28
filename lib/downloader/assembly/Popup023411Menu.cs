// Decompiled with JetBrains decompiler
// Type: Popup023411Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup023411Menu : BackButtonMenuBase
{
  public virtual void IbtnBack() => Singleton<PopupManager>.GetInstance().onDismiss();

  public virtual void IbtnRecover() => this.StartCoroutine(this.ShowBPAlertPopup());

  public void IbtnNo() => this.IbtnBack();

  public override void onBackButton() => this.IbtnNo();

  private IEnumerator ShowBPAlertPopup()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Future<GameObject> prefabf = Res.Prefabs.popup.popup_023_4_12__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabf.Result).GetComponent<Popup023412Menu>().Init();
  }
}
