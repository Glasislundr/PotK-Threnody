// Decompiled with JetBrains decompiler
// Type: Shop99991Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop99991Menu : BackButtonMenuBase
{
  public Shop0079Menu menu0079;
  [SerializeField]
  private UILabel TxtDescription;
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription02;
  [SerializeField]
  private UILabel TxtDescription03;
  [SerializeField]
  private UILabel TxtDescription04;
  [SerializeField]
  private UILabel TxtDescription05;
  [SerializeField]
  private UILabel TxtDescription06;
  [SerializeField]
  private UILabel TxtDescription07;
  [SerializeField]
  private UILabel TxtPopuptitle;

  public bool IsDisabledIbtnNoPopupDismiss { get; set; }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    PurchaseBehavior.ShowInputBirthday(false);
    if (this.IsDisabledIbtnNoPopupDismiss)
      return;
    PurchaseBehavior.PopupDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnPopupYes()
  {
    if (this.IsPushAndSet())
      return;
    PurchaseBehavior.PopupDismiss();
    PurchaseBehavior.ShowInputBirthday(true);
  }

  private IEnumerator popup()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.CoinbonusHistory> handler = WebAPI.CoinbonusHistory((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = handler.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (handler.Result != null)
    {
      Future<GameObject> prefab = Res.Prefabs.popup.popup_007_9__anim_popup01.Load<GameObject>();
      e1 = prefab.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      e1 = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Shop0079Menu>().Init(handler.Result);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }
}
