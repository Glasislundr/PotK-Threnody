// Decompiled with JetBrains decompiler
// Type: TotalPaymentBonusPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class TotalPaymentBonusPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel mDescriptionTxt;
  [SerializeField]
  private UILabel mPeriodTxt;
  [SerializeField]
  private UILabel mPaidKisekiNumTxt;
  [SerializeField]
  private NGxScroll mScrollGrid;
  private MypageEventButton mMypageButton;
  private GameObject mListItemPrefab;
  private List<TotalPaymentBonusListItem> mListItems;

  public IEnumerator Initialize(
    WebAPI.Response.PaymentbonusList response,
    MypageEventButton mypageEventButton = null)
  {
    TotalPaymentBonusPopup paymentBonusPopup = this;
    paymentBonusPopup.IsPush = true;
    paymentBonusPopup.mMypageButton = mypageEventButton;
    paymentBonusPopup.RefreshInfomation(response.period, response.spend_coins);
    Future<GameObject> loader = new ResourceObject("Prefabs/totalpaymentbonus/dir_TotalPaymentBonusItem_Element").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    paymentBonusPopup.mListItemPrefab = loader.Result;
    e = paymentBonusPopup.RefreshContentList(response.paymentbonus_list);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    paymentBonusPopup.IsPush = false;
  }

  private void RefreshInfomation(PaymentBonusPeriod period, int spendCoin)
  {
    TotalPaymentBonus totalPaymentBonus = (TotalPaymentBonus) null;
    if (MasterData.TotalPaymentBonus.TryGetValue(period.id, out totalPaymentBonus) && !string.IsNullOrEmpty(totalPaymentBonus.description))
      this.mDescriptionTxt.SetTextLocalize(totalPaymentBonus.description);
    this.mPeriodTxt.SetTextLocalize(string.Format("{0:yyyy/MM/dd}～{1:yyyy/MM/dd}", (object) period.start_at, (object) period.end_at));
    this.mPaidKisekiNumTxt.SetTextLocalize(spendCoin);
  }

  private IEnumerator RefreshContentList(PlayerPaymentBonusReceiveHistory[] receiveHistory)
  {
    TotalPaymentBonusPopup parent = this;
    if (parent.mListItems != null)
    {
      parent.mScrollGrid.Reset();
      foreach (Component mListItem in parent.mListItems)
        Object.Destroy((Object) mListItem.gameObject);
      parent.mListItems = (List<TotalPaymentBonusListItem>) null;
    }
    List<PlayerPaymentBonusReceiveHistory> bonusReceiveHistoryList = new List<PlayerPaymentBonusReceiveHistory>();
    bonusReceiveHistoryList.AddRange((IEnumerable<PlayerPaymentBonusReceiveHistory>) receiveHistory);
    bonusReceiveHistoryList.Sort((Comparison<PlayerPaymentBonusReceiveHistory>) ((x, y) =>
    {
      if (x.is_received && !y.is_received)
        return 1;
      if (!x.is_received && y.is_received || x.is_archived && !y.is_archived)
        return -1;
      return !x.is_archived && y.is_archived ? 1 : x.require_paid_coins - y.require_paid_coins;
    }));
    parent.mListItems = new List<TotalPaymentBonusListItem>(bonusReceiveHistoryList.Count);
    ((UIRect) parent.mScrollGrid.scrollView.panel).alpha = 0.0f;
    foreach (PlayerPaymentBonusReceiveHistory bonus in bonusReceiveHistoryList)
    {
      TotalPaymentBonusListItem contentObj = parent.mListItemPrefab.CloneAndGetComponent<TotalPaymentBonusListItem>();
      parent.mScrollGrid.Add(((Component) contentObj).gameObject);
      IEnumerator e = contentObj.Initialize(bonus, parent);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      parent.mListItems.Add(contentObj);
      contentObj = (TotalPaymentBonusListItem) null;
    }
    // ISSUE: method pointer
    parent.mScrollGrid.grid.onReposition = new UIGrid.OnReposition((object) parent, __methodptr(\u003CRefreshContentList\u003Eb__9_1));
    parent.mScrollGrid.ResolvePosition();
    if (Object.op_Inequality((Object) parent.mMypageButton, (Object) null))
    {
      bool flag = ((IEnumerable<PlayerPaymentBonusReceiveHistory>) receiveHistory).Any<PlayerPaymentBonusReceiveHistory>((Func<PlayerPaymentBonusReceiveHistory, bool>) (x => x.is_archived && !x.is_received));
      Singleton<NGGameDataManager>.GetInstance().hasReceivableTotalPaymentBonus = flag;
      parent.mMypageButton.SetBadgeActive(flag);
    }
  }

  public void OnCloseButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void OnHelpButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    Help0152Scene.ChangeScene(true, MasterData.HelpCategory[31]);
  }

  public override void onBackButton() => this.OnCloseButton();

  public void OnListItemGetButton(int contentId)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.SendContentGet(contentId));
  }

  private IEnumerator SendContentGet(int contentId)
  {
    TotalPaymentBonusPopup paymentBonusPopup = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.PaymentbonusReceive> api = WebAPI.PaymentbonusReceive(contentId, new Action<WebAPI.Response.UserError>(paymentBonusPopup.\u003CSendContentGet\u003Eb__14_0));
    IEnumerator e = api.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!api.HasResult || api.Result == null)
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Singleton<PopupManager>.GetInstance().dismiss();
    }
    else
    {
      WebAPI.Response.PaymentbonusReceive response = api.Result;
      Singleton<NGGameDataManager>.GetInstance().hasReceivableTotalPaymentBonus = response.has_receivable_paymentbonus;
      if (Object.op_Inequality((Object) paymentBonusPopup.mMypageButton, (Object) null))
        paymentBonusPopup.mMypageButton.SetBadgeActive(response.has_receivable_paymentbonus);
      paymentBonusPopup.RefreshInfomation(response.period, response.spend_coins);
      e = paymentBonusPopup.RefreshContentList(response.paymentbonus_list);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Future<GameObject> prefabF = new ResourceObject("Prefabs/shop007_9/dir_Result_Shop_Get_reward").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefabF.Result;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      ShopItemGetResult component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<ShopItemGetResult>();
      ((Component) component).transform.localScale = Vector3.zero;
      e = component.Init(paymentBonusPopup.ConvertToResultInfoList(response), false, new Action(paymentBonusPopup.OnFinishShopItemGetResult));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void OnFinishShopItemGetResult()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    this.StartCoroutine(this.IsPushOff());
  }

  private List<ShopItemGetResultInfo> ConvertToResultInfoList(
    WebAPI.Response.PaymentbonusReceive response)
  {
    List<ShopItemGetResultInfo> resultInfoList = new List<ShopItemGetResultInfo>();
    foreach (PlayerPresent playerPresent in response.player_presents)
      resultInfoList.Add(new ShopItemGetResultInfo((MasterDataTable.CommonRewardType) playerPresent.reward_type_id.Value, playerPresent.reward_id.Value, playerPresent.reward_quantity.Value));
    return resultInfoList;
  }
}
