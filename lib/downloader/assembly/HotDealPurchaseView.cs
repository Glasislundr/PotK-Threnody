// Decompiled with JetBrains decompiler
// Type: HotDealPurchaseView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Core;
using Gsc.Purchase;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class HotDealPurchaseView : MonoBehaviour, IPurchaseFlowListener, IPurchaseResultListener
{
  [NonSerialized]
  public bool isInitialized;

  public bool IsOnProducts { get; private set; }

  public bool IsInputBirthday { get; private set; }

  public IEnumerator Init()
  {
    HotDealPurchaseView listener = this;
    IEnumerator e = PurchaseBehaviorLoadingLayer.Enable();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!PurchaseFlow.initialized)
    {
      while (!PurchaseFlow.initialized)
        yield return (object) null;
    }
    PurchaseFlow.LaunchFlow<HotDealPurchaseView>(listener, true);
  }

  public void InputBirthday(PurchaseFlow flow)
  {
    this.IsInputBirthday = true;
    PurchaseBehavior.actualFlow = flow;
    ImmortalObject.Instance.StartCoroutine(this._InputBirthday());
  }

  private IEnumerator _InputBirthday()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_999_9_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.PopupOpen(prefab.Result).GetComponent<Shop99991Menu>().IsDisabledIbtnNoPopupDismiss = true;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void Confirm(PurchaseFlow flow, ProductInfo product)
  {
    ModalWindow.ShowYesNo("確認が必要です", string.Format("{0}を{1}で購入しますか？", (object) product.LocalizedTitle, (object) product.LocalizedPrice), (Action) (() => flow.Confirmed(true)), (Action) (() => flow.Confirmed(false)));
  }

  public void OnInvalidBirthday(PurchaseFlow flow)
  {
    PurchaseBehavior.ShowPopupWithMessage(Consts.GetInstance().SHOP_99981_MENU_01, Consts.GetInstance().SHOP_99981_MENU_02);
  }

  public void OnProducts(PurchaseFlow flow, ProductInfo[] productInfos)
  {
    this.IsOnProducts = true;
    if (productInfos == null)
      return;
    int length = productInfos.Length;
  }

  public void OnFinished(bool isSuccess)
  {
  }

  public void OnOverCreditLimited()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage(Consts.GetInstance().PAYMENT_LISTENER_ON_CHARGE_LIMIT_RESPONSE, Consts.GetInstance().SHOP_999101_SET_TEXT);
  }

  public void OnInsufficientBalances()
  {
    PurchaseBehavior.ShowPopupYesNo("購入エラー", "DMMポイントが不足しています。チャージしますか？", (Action) (() => Application.OpenURL("https://point.dmm.com/choice/pay?basket_service_type=freegame")));
  }

  public void OnPurchaseSucceeded(FulfillmentResult result)
  {
  }

  public void OnPurchaseFailed() => PurchaseBehaviorLoadingLayer.Disable();

  public void OnPurchaseCanceled() => PurchaseBehaviorLoadingLayer.Disable();

  public void OnPurchaseAlreadyOwned()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage("購入エラー", "すでに購入済みです。データを再ダウンロードします。", (Action) (() =>
    {
      ImmortalObject.Instance.StartCoroutine(PurchaseBehaviorLoadingLayer.Enable());
      PurchaseFlow.Resume();
    }));
  }

  public void OnPurchaseDeferred()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage("購入エラー", "購入承認待ちです。しばらくお待ち下さい。");
  }

  public void OnPurchasePending()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage("購入エラー", "購入支払い待ちです。");
  }

  public void OnPurchasePendingExists()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage("購入エラー", "既に購入支払い待ちがあります。");
  }
}
