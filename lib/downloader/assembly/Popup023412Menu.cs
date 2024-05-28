// Decompiled with JetBrains decompiler
// Type: Popup023412Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup023412Menu : NGMenuBase
{
  public GameObject[] CpOnObject;
  public UILabel HimeNum;

  public void Init()
  {
    Player player = SMManager.Get<Player>();
    this.HimeNum.SetTextLocalize(player.coin);
    for (int index = 0; index < this.CpOnObject.Length; ++index)
      this.CpOnObject[index].SetActive(index < player.bp);
  }

  public virtual void IbtnYes()
  {
    if (SMManager.Get<Player>().coin > 0)
      this.StartCoroutine(this.CpRecovery());
    else
      this.StartCoroutine(this.ShowStoneAlert());
  }

  public virtual void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  private IEnumerator CpRecovery()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.ShopBuy> paramF = WebAPI.ShopBuy(10000004, 1, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = paramF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (paramF.Result != null)
    {
      e1 = OnDemandDownload.WaitLoadHasUnitResource(false);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Future<GameObject> prefabf = Res.Prefabs.popup.popup_023_4_13__anim_popup01.Load<GameObject>();
      e1 = prefabf.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(prefabf.Result);
    }
  }

  private IEnumerator ShowStoneAlert()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    Future<GameObject> prefabf = Res.Prefabs.popup.popup_023_4_14__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabf.Result).GetComponent<Popup023414Menu>().Init();
  }
}
