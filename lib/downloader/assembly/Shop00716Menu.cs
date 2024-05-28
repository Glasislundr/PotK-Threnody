// Decompiled with JetBrains decompiler
// Type: Shop00716Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop00716Menu : BackButtonMenuBase
{
  private int price;
  private bool success;
  private int nextMax;
  [SerializeField]
  private UILabel txtDescription2;
  [SerializeField]
  private UILabel txtNumber;

  public void Init(int pri, int prev, int next)
  {
    this.price = pri;
    this.nextMax = next;
    this.txtDescription2.SetText(Consts.GetInstance().SHOP_00716_MENU + ":" + prev.ToLocalizeNumberText() + "→[fff000]" + next.ToLocalizeNumberText() + "[-]");
    this.txtNumber.SetTextLocalize(SMManager.Observe<Player>().Value.coin);
  }

  private IEnumerator ItemBoxPlus()
  {
    Future<WebAPI.Response.ShopBuy> paramF = WebAPI.ShopBuy(10000003, 1, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    })).Then<WebAPI.Response.ShopBuy>((Func<WebAPI.Response.ShopBuy, WebAPI.Response.ShopBuy>) (result =>
    {
      Singleton<NGGameDataManager>.GetInstance().Parse(result);
      return result;
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
      e1 = this.popup00717();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  public void IbtnPopupYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss(true);
    if (SMManager.Get<Player>().CheckKiseki(this.price))
      Singleton<PopupManager>.GetInstance().monitorCoroutine(this.ItemBoxPlusAsync());
    else
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._999_3_1(Consts.GetInstance().SHOP_99931_TXT_DESCRIPTION));
  }

  private IEnumerator ItemBoxPlusAsync()
  {
    Singleton<PopupManager>.GetInstance().closeAll(true);
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.ItemBoxPlus();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator popup00717()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_007_17__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Shop00717SetText>().SetText(this.nextMax);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
