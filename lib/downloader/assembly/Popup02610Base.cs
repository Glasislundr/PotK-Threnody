// Decompiled with JetBrains decompiler
// Type: Popup02610Base
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup02610Base : BackButtonMenuBase
{
  protected WebAPI.Response.PvpBoot pvpInfo;
  protected Versus02610Menu menu;

  public virtual void Init(WebAPI.Response.PvpBoot pvpInfo, Versus02610Menu menu)
  {
    this.pvpInfo = pvpInfo;
    this.menu = menu;
  }

  public virtual IEnumerator InitCoroutine(WebAPI.Response.PvpBoot pvpInfo, Versus02610Menu menu)
  {
    this.pvpInfo = pvpInfo;
    this.menu = menu;
    yield break;
  }

  public virtual IEnumerator Return2688Popup()
  {
    Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_8__anim_popup01.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(pF.Result).GetComponent<Popup02688Menu>().InitCoroutine(this.pvpInfo, this.menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual IEnumerator Return2687Popup()
  {
    Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_7__anim_popup01.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Popup02687Menu p = Singleton<PopupManager>.GetInstance().open(pF.Result).GetComponent<Popup02687Menu>();
    ((Component) p).gameObject.SetActive(false);
    e = p.InitCoroutine(this.pvpInfo, this.menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    ((Component) p).gameObject.SetActive(true);
  }

  public virtual void IbtnNo()
  {
  }

  public override void onBackButton() => this.IbtnNo();
}
