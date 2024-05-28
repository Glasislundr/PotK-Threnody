// Decompiled with JetBrains decompiler
// Type: PurchaseView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Core;
using Gsc.Purchase;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class PurchaseView : MonoBehaviour, IPurchaseFlowListener, IPurchaseResultListener
{
  [NonSerialized]
  public bool isInitialized;
  private NGxScroll ngxScroll;
  private WebAPI.Response.CoinbonusHistory history;
  private DisplayTouchEffectController displayTouchEffectController;

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) this.displayTouchEffectController, (Object) null))
      return;
    this.displayTouchEffectController.SetTapLock(false);
  }

  public IEnumerator Init(NGxScroll ngS, WebAPI.Response.CoinbonusHistory h)
  {
    PurchaseView listener = this;
    listener.displayTouchEffectController = Singleton<CommonRoot>.GetInstance().TouchEffectController;
    if (Object.op_Inequality((Object) listener.displayTouchEffectController, (Object) null))
      listener.displayTouchEffectController.SetTapLock(true);
    listener.ngxScroll = ngS;
    listener.history = h;
    if (!PurchaseFlow.initialized)
    {
      while (!PurchaseFlow.initialized)
        yield return (object) null;
    }
    PurchaseFlow.LaunchFlow<PurchaseView>(listener, true);
  }

  public void InputBirthday(PurchaseFlow flow)
  {
    PurchaseBehavior.actualFlow = flow;
    this.StartCoroutine(this._InputBirthday());
  }

  private IEnumerator _InputBirthday()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_999_9_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PurchaseBehavior.PopupOpen(prefab.Result);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
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
    if (productInfos == null || productInfos.Length == 0)
      return;
    this.StartCoroutine(this.CreateItemsList(productInfos));
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

  public void Close()
  {
  }

  public void OnPurchaseSucceeded(FulfillmentResult result)
  {
  }

  public void OnPurchaseFailed() => PurchaseBehaviorLoadingLayer.Disable();

  public void OnPurchaseCanceled() => PurchaseBehaviorLoadingLayer.Disable();

  public void OnPurchaseAlreadyOwned()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage("購入エラー", "すでに姫石を購入済みです。データを再ダウンロードします。", (Action) (() =>
    {
      ImmortalObject.Instance.StartCoroutine(PurchaseBehaviorLoadingLayer.Enable());
      PurchaseFlow.Resume();
    }));
  }

  public void OnPurchaseDeferred()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage("購入エラー", "姫石の購入承認待ちです。しばらくお待ち下さい。");
  }

  public void OnPurchasePending()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage("購入エラー", "姫石の購入支払い待ちです。");
  }

  public void OnPurchasePendingExists()
  {
    PurchaseBehaviorLoadingLayer.Disable();
    PurchaseBehavior.ShowPopupWithMessage("購入エラー", "既に姫石の購入支払い待ちがあります。");
  }

  private IEnumerator CreateItemsList(ProductInfo[] productsInfos)
  {
    if (!this.isInitialized)
    {
      this.ngxScroll.Clear();
      Future<GameObject> scrollPrefabF = Res.Prefabs.shop007_9.vscroll_540_7.Load<GameObject>();
      IEnumerator e = scrollPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject scrollPrefab = scrollPrefabF.Result;
      e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      DateTime now = ServerTime.NowAppTime();
      List<Scroll0079Arg> scroll0079ArgList = new List<Scroll0079Arg>();
      List<Scroll0079Arg> scroll0079Args_simplePack = new List<Scroll0079Arg>();
      List<Scroll0079Arg> scroll0079Args_normal = new List<Scroll0079Arg>();
      foreach (ProductInfo productsInfo in productsInfos)
      {
        CoinProduct activeProductData = MasterData.CoinProductList.GetActiveProductData(productsInfo.ProductId);
        if (activeProductData.type != 3 && activeProductData.type != 4 && activeProductData.type != 5 && activeProductData.type != 7)
        {
          int sortIndex = 0;
          List<CoinBonusReward> list = this.history.coin_bonus_rewards.GetActiveList(productsInfo.ProductId).ToList<CoinBonusReward>();
          CoinBonusReward coinBonusReward = (CoinBonusReward) null;
          if (list != null && list.Count > 0)
          {
            List<CoinBonusReward> coinBonusRewardList = new List<CoinBonusReward>();
            foreach (CoinBonusReward coinBonusReward1 in list)
            {
              CoinBonusReward reward = coinBonusReward1;
              if (((IEnumerable<PlayerCoinBonusHistory>) this.history.player_coin_bonus_history).Count<PlayerCoinBonusHistory>((Func<PlayerCoinBonusHistory, bool>) (x => x.coinbonus_id == reward.coin_bonus_id)) == 0)
                coinBonusRewardList.Add(reward);
            }
            if (coinBonusRewardList.Count > 0)
              coinBonusReward = coinBonusRewardList[0];
          }
          CoinBonus coinBonus = (CoinBonus) null;
          int index1 = 0;
          PlayerCoinBonusInfo playerCoinBonusInfo = (PlayerCoinBonusInfo) null;
          if (coinBonusReward != null)
          {
            for (int index2 = 0; index2 < this.history.coin_bonuses.Length; ++index2)
            {
              if (this.history.coin_bonuses[index2].id == coinBonusReward.coin_bonus_id)
              {
                coinBonus = this.history.coin_bonuses[index2];
                index1 = index2;
                break;
              }
            }
            playerCoinBonusInfo = ((IEnumerable<PlayerCoinBonusInfo>) this.history.player_coin_bonus_infos).First<PlayerCoinBonusInfo>((Func<PlayerCoinBonusInfo, bool>) (x => x.id == coinBonusReward.coin_bonus_id));
          }
          WebAPI.Response.CoinbonusHistoryCoin_bonus_details coinbonusHistoryCoinBonusDetails = (WebAPI.Response.CoinbonusHistoryCoin_bonus_details) null;
          if (coinBonus != null)
            coinbonusHistoryCoinBonusDetails = this.history.coin_bonus_details[index1];
          SimplePackInfo simplePackInfo = (SimplePackInfo) null;
          if (activeProductData.type == 2)
          {
            for (int index3 = 0; index3 < this.history.simple_packs.Length; ++index3)
            {
              SimplePackInfo packInfo = this.history.simple_packs[index3];
              if (((IEnumerable<CoinGroup>) this.history.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == packInfo.pack.coin_group_id)).GetCoinId() == activeProductData.ID)
              {
                sortIndex = index3;
                simplePackInfo = packInfo;
                break;
              }
            }
          }
          BeginnerPackInfo beginnerPackInfo = (BeginnerPackInfo) null;
          if (activeProductData.type == 6)
          {
            foreach (BeginnerPackInfo beginnerPack in this.history.beginner_packs)
            {
              BeginnerPackInfo packInfo = beginnerPack;
              if (((IEnumerable<CoinGroup>) this.history.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == packInfo.pack.coin_group_id)).GetCoinId() == activeProductData.ID)
              {
                beginnerPackInfo = packInfo;
                break;
              }
            }
          }
          if (activeProductData.type != 2 && activeProductData.type != 6 || simplePackInfo != null || beginnerPackInfo != null)
          {
            Scroll0079Arg scroll0079Arg = new Scroll0079Arg(productsInfo, activeProductData, coinBonus, coinBonusReward, playerCoinBonusInfo, coinbonusHistoryCoinBonusDetails, simplePackInfo, beginnerPackInfo, now, sortIndex);
            if (beginnerPackInfo != null)
              scroll0079ArgList.Add(scroll0079Arg);
            else if (simplePackInfo != null)
              scroll0079Args_simplePack.Add(scroll0079Arg);
            else
              scroll0079Args_normal.Add(scroll0079Arg);
          }
        }
      }
      yield return (object) this.CreateScrollItem(scrollPrefab, scroll0079ArgList.ToArray());
      Scroll0079Arg[] array1 = scroll0079Args_simplePack.OrderBy<Scroll0079Arg, int>((Func<Scroll0079Arg, int>) (x => x.sortIndex)).ToArray<Scroll0079Arg>();
      yield return (object) this.CreateScrollItem(scrollPrefab, array1);
      Scroll0079Arg[] array2 = scroll0079Args_normal.OrderBy<Scroll0079Arg, float>((Func<Scroll0079Arg, float>) (x => x.productInfo.Price)).ToArray<Scroll0079Arg>();
      yield return (object) this.CreateScrollItem(scrollPrefab, array2);
      this.isInitialized = true;
    }
  }

  private IEnumerator CreateScrollItem(GameObject scrollPrefab, Scroll0079Arg[] scroll0079Args)
  {
    Scroll0079Arg[] scroll0079ArgArray = scroll0079Args;
    for (int index = 0; index < scroll0079ArgArray.Length; ++index)
    {
      Scroll0079Arg scroll0079Arg = scroll0079ArgArray[index];
      GameObject gameObject = scrollPrefab.Clone();
      this.ngxScroll.Add(gameObject);
      IEnumerator e = gameObject.GetComponent<Scroll0079>().Init(scroll0079Arg);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    scroll0079ArgArray = (Scroll0079Arg[]) null;
  }
}
