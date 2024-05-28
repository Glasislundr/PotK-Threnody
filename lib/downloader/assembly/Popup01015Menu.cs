// Decompiled with JetBrains decompiler
// Type: Popup01015Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup01015Menu : BackButtonMenuBase
{
  public UILabel TextDescription;
  private Setting01013Menu menu01013;
  private string newName;

  public IEnumerator Init(Setting01013Menu menu, string name)
  {
    this.menu01013 = menu;
    this.newName = name;
    this.TextDescription.SetText(Consts.Format(Consts.GetInstance().POPUP_01015_DESCRIPTION, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (name),
        (object) name
      }
    }));
    yield break;
  }

  private IEnumerator Popup01017()
  {
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_010_1_7__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Singleton<PopupManager>.GetInstance().open(result);
  }

  private IEnumerator Popup01018()
  {
    Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_010_1_8__anim_popup01.Load<GameObject>();
    IEnumerator e = popupPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = popupPrefabF.Result;
    Singleton<PopupManager>.GetInstance().open(result);
  }

  private IEnumerator BtnYes()
  {
    Popup01015Menu popup01015Menu = this;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.PlayerNameEdit> result = WebAPI.PlayerNameEdit(popup01015Menu.newName, new Action<WebAPI.Response.UserError>(popup01015Menu.\u003CBtnYes\u003Eb__6_0));
    IEnumerator e = result.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (result.Result != null)
    {
      Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_010_1_6__anim_popup01.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result1 = popupPrefabF.Result;
      Popup01016Menu component = Singleton<PopupManager>.GetInstance().open(result1).GetComponent<Popup01016Menu>();
      popup01015Menu.StartCoroutine(component.Init(popup01015Menu.menu01013, result.Result.player.name));
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void IbtnYes()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.StartCoroutine(this.BtnYes());
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();
}
