// Decompiled with JetBrains decompiler
// Type: PopupCoinLimit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupCoinLimit : BackButtonMenuBase
{
  [SerializeField]
  public CoinLimitScrollContainer CoinLimitScrollContainer;

  public IEnumerator Init()
  {
    ((Component) this.CoinLimitScrollContainer.scrollView).gameObject.SetActive(false);
    IEnumerator e = this.CoinLimitScrollContainer.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) this.CoinLimitScrollContainer.scrollView).gameObject.SetActive(true);
    this.CoinLimitScrollContainer.scrollView.SetDragAmount(0.0f, ((UIProgressBar) this.CoinLimitScrollContainer.scrollBar).value, true);
  }

  public void IbtnClose() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnClose();

  public void IbtnCoinExchange()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.StartCoroutine(this.ChangeCoinShop());
  }

  public IEnumerator ChangeCoinShop()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (!WebAPI.IsResponsedAtRecent("ShopStatus"))
    {
      Future<WebAPI.Response.ShopStatus> shoplistF = WebAPI.ShopStatus((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
      })).Then<WebAPI.Response.ShopStatus>((Func<WebAPI.Response.ShopStatus, WebAPI.Response.ShopStatus>) (result =>
      {
        Singleton<NGGameDataManager>.GetInstance().Parse(result);
        return result;
      }));
      IEnumerator e1 = shoplistF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (shoplistF.Result == null)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        yield break;
      }
      else
        shoplistF = (Future<WebAPI.Response.ShopStatus>) null;
    }
    ShopCoinExchangeScene.changeScene();
  }
}
