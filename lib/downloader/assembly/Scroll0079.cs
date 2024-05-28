// Decompiled with JetBrains decompiler
// Type: Scroll0079
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Purchase;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Scroll0079 : MonoBehaviour
{
  [SerializeField]
  private UILabel TxtExplanation;
  [SerializeField]
  private UILabel TxtItemname;
  [SerializeField]
  private UIButton BuyOrReceiveButton;
  [SerializeField]
  private UILabel TxtPrice;
  [SerializeField]
  private UILabel TxtPopupkisekinum;
  [SerializeField]
  private UI2DSprite linkItem;
  [SerializeField]
  private UIButton infoButton;
  [SerializeField]
  private UIButton iconLoupe;
  [SerializeField]
  private UISprite benefitsFirstLimited;
  [SerializeField]
  private UISprite benefitsMultibuy;
  [SerializeField]
  private UISprite benefitsOnceADay;
  [SerializeField]
  private UISprite benefitsBenefits;
  [SerializeField]
  private UISprite benefitsLimitedTime;
  [SerializeField]
  private GameObject itemPackBase;
  [SerializeField]
  private GameObject numPurchases;
  [SerializeField]
  private UILabel txtPurchases;
  [SerializeField]
  private UILabel numRemainingCount;
  [SerializeField]
  private UILabel numPurchasesCount;
  [SerializeField]
  private GameObject buyLimit;
  [SerializeField]
  private UILabel buyLimitText;
  private ProductInfo productInfo;
  private CoinProduct coinProduct;
  private CoinBonusReward coinbonusReward;
  private WebAPI.Response.CoinbonusHistoryCoin_bonus_details coinbonusHistoryCoinBonusDetails;
  private SimplePackInfo simplePackInfo;
  private BeginnerPackInfo beginnerPackInfo;
  private bool isBuy;

  public IEnumerator Init(Scroll0079Arg arg)
  {
    this.productInfo = arg.productInfo;
    this.coinProduct = arg.coinProduct;
    this.coinbonusReward = arg.coinbonusReward;
    this.coinbonusHistoryCoinBonusDetails = arg.coinbonusHistoryCoinBonusDetails;
    this.simplePackInfo = arg.simplePackInfo;
    this.beginnerPackInfo = arg.beginnerPackInfo;
    if (this.coinProduct.type == 2 || this.coinProduct.type == 6)
      ((Component) this.iconLoupe).gameObject.SetActive(false);
    else
      ((Component) this.iconLoupe).gameObject.SetActive(true);
    IEnumerator e = this.SetSprite(this.coinProduct, this.simplePackInfo == null ? (this.beginnerPackInfo == null ? "" : this.beginnerPackInfo.pack.icon_resource_name) : this.simplePackInfo.pack.icon_resource_name);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (this.coinProduct.type == 2 && this.simplePackInfo != null)
      this.SetTextPack(this.simplePackInfo.pack.name, this.simplePackInfo.pack.description);
    else if (this.coinProduct.type == 6 && this.beginnerPackInfo != null)
      this.SetTextPack(this.beginnerPackInfo.pack.name, this.beginnerPackInfo.pack.description);
    else
      this.SetTextNormal(this.coinProduct, this.productInfo);
    if (this.coinProduct.type == 2 && this.simplePackInfo != null && this.simplePackInfo.player_pack.purchased_count > 0 && this.simplePackInfo.player_pack.rest_receive_day.HasValue)
    {
      this.isBuy = false;
      if (this.simplePackInfo.player_pack.is_received)
        this.TxtPrice.SetTextLocalize("受取済");
      else
        this.TxtPrice.SetTextLocalize("受取");
    }
    else if (this.coinProduct.type == 6 && this.beginnerPackInfo != null && this.beginnerPackInfo.player_pack.purchased_count > 0 && this.beginnerPackInfo.player_pack.rest_receive_day.HasValue)
    {
      this.isBuy = false;
      if (this.beginnerPackInfo.player_pack.is_received)
        this.TxtPrice.SetTextLocalize("受取済");
      else
        this.TxtPrice.SetTextLocalize("受取");
    }
    else
    {
      this.isBuy = true;
      this.TxtPrice.SetTextLocalize(this.productInfo.LocalizedPrice.Replace("\\", "￥"));
    }
    if (this.coinProduct.type == 2 && this.simplePackInfo != null && !this.isBuy && this.simplePackInfo.player_pack.is_received)
      ((UIButtonColor) this.BuyOrReceiveButton).isEnabled = false;
    else if (this.coinProduct.type == 6 && this.beginnerPackInfo != null && !this.isBuy && this.beginnerPackInfo.player_pack.is_received)
      ((UIButtonColor) this.BuyOrReceiveButton).isEnabled = false;
    else
      ((UIButtonColor) this.BuyOrReceiveButton).isEnabled = true;
    if (this.coinProduct.type == 2 && this.simplePackInfo != null)
      this.SetBenefits((Badgecategory) this.simplePackInfo.pack.badge_category);
    else if (this.coinProduct.type == 6 && this.beginnerPackInfo != null)
      this.SetBenefits((Badgecategory) this.beginnerPackInfo.pack.badge_category);
    else if (arg.playerCoinBonusInfo != null)
      this.SetBenefits((Badgecategory) arg.playerCoinBonusInfo.badge_category);
    else
      this.SetBenefits(Badgecategory.None);
    this.txtPurchases.text = arg.playerCoinBonusInfo != null ? "特典回数" : "購入回数";
    if (this.coinProduct.type == 2 && this.simplePackInfo != null)
      this.SetNumPurchases(this.simplePackInfo.pack.purchase_limit - this.simplePackInfo.player_pack.purchased_count, this.simplePackInfo.pack.purchase_limit);
    else if (this.coinProduct.type == 6 && this.beginnerPackInfo != null)
      this.SetNumPurchases(this.beginnerPackInfo.pack.purchase_limit - this.beginnerPackInfo.player_pack.purchased_count, this.beginnerPackInfo.pack.purchase_limit);
    else if (arg.playerCoinBonusInfo != null)
      this.SetNumPurchases(arg.playerCoinBonusInfo.purchase_limit - arg.playerCoinBonusInfo.purchased_count, arg.playerCoinBonusInfo.purchase_limit);
    else
      this.numPurchases.SetActive(false);
    if (this.coinProduct.type == 2 && this.simplePackInfo != null)
    {
      if (!this.simplePackInfo.player_pack.rest_receive_day.HasValue)
        this.SetBuyLimit(arg.now, this.simplePackInfo.pack.end_at);
      else
        this.SetReceiveLimit(this.simplePackInfo.player_pack.rest_receive_day, arg.now);
    }
    else if (this.coinProduct.type == 6 && this.beginnerPackInfo != null)
    {
      if (!this.beginnerPackInfo.player_pack.rest_receive_day.HasValue)
        this.SetBuyLimit(arg.now, this.beginnerPackInfo.beginner_end_at);
      else
        this.SetReceiveLimit(this.beginnerPackInfo.player_pack.rest_receive_day, arg.now);
    }
    else if (arg.coinBonus != null)
      this.SetBuyLimit(arg.now, arg.coinBonus.end_at);
    else
      this.buyLimit.SetActive(false);
  }

  private IEnumerator SetSprite(CoinProduct coinProduct, string packIconResourceName)
  {
    Future<Sprite> linkItemF = CoinProduct.LoadSpriteThumbnail(false, packIconResourceName);
    IEnumerator e = linkItemF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.linkItem.sprite2D = linkItemF.Result;
    if (coinProduct.type == 2 || coinProduct.type == 6)
      this.itemPackBase.SetActive(true);
    else
      this.itemPackBase.SetActive(false);
  }

  private void SetTextPack(string name, string description)
  {
    this.TxtItemname.SetTextLocalize(name);
    this.TxtExplanation.SetTextLocalize(description);
    ((Component) this.TxtPopupkisekinum).gameObject.SetActive(false);
  }

  private void SetTextNormal(CoinProduct coinProduct, ProductInfo productInfo)
  {
    this.TxtItemname.SetTextLocalize(coinProduct.name);
    if (this.coinbonusReward != null)
      this.TxtExplanation.SetTextLocalize(this.coinbonusReward.client_coin_shop_title.ToConverter());
    else
      this.TxtExplanation.SetTextLocalize(coinProduct.description);
    int num = coinProduct.additional_free_coin + coinProduct.additional_paid_coin;
    this.TxtPopupkisekinum.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_0079_TXT_POPUP_KISEKINUM, (IDictionary) new Hashtable()
    {
      {
        (object) "kisekiNum",
        (object) num
      }
    }));
  }

  private void SetBenefits(Badgecategory badgecategory)
  {
    ((Component) this.benefitsFirstLimited).gameObject.SetActive(false);
    ((Component) this.benefitsMultibuy).gameObject.SetActive(false);
    ((Component) this.benefitsOnceADay).gameObject.SetActive(false);
    ((Component) this.benefitsBenefits).gameObject.SetActive(false);
    ((Component) this.benefitsLimitedTime).gameObject.SetActive(false);
    switch (badgecategory)
    {
      case Badgecategory.First:
        ((Component) this.benefitsFirstLimited).gameObject.SetActive(true);
        break;
      case Badgecategory.Multiple:
        ((Component) this.benefitsMultibuy).gameObject.SetActive(true);
        break;
      case Badgecategory.OnedayFirst:
        ((Component) this.benefitsOnceADay).gameObject.SetActive(true);
        break;
      case Badgecategory.Benefits:
        ((Component) this.benefitsBenefits).gameObject.SetActive(true);
        break;
      case Badgecategory.Period:
        ((Component) this.benefitsLimitedTime).gameObject.SetActive(true);
        break;
    }
  }

  private void SetNumPurchases(int purchaseCount, int purchaseLimit)
  {
    if (purchaseLimit == 0)
    {
      this.numPurchases.SetActive(false);
    }
    else
    {
      this.numPurchases.SetActive(true);
      this.numRemainingCount.text = purchaseCount.ToString();
      this.numPurchasesCount.text = purchaseLimit.ToString();
    }
  }

  private void SetBuyLimit(DateTime now, DateTime? endAt)
  {
    if (!endAt.HasValue)
      return;
    this.buyLimit.SetActive(true);
    DateTime? nullable1 = endAt;
    DateTime dateTime = now;
    TimeSpan? nullable2 = nullable1.HasValue ? new TimeSpan?(nullable1.GetValueOrDefault() - dateTime) : new TimeSpan?();
    if (nullable2.Value.Days > 0)
    {
      this.buyLimitText.text = "あと" + nullable2.Value.Days.ToLocalizeNumberText() + "日";
    }
    else
    {
      TimeSpan timeSpan = nullable2.Value;
      if (timeSpan.Hours > 0)
      {
        UILabel buyLimitText = this.buyLimitText;
        timeSpan = nullable2.Value;
        string str = "あと" + timeSpan.Hours.ToLocalizeNumberText() + "時間";
        buyLimitText.text = str;
      }
      else
      {
        UILabel buyLimitText = this.buyLimitText;
        timeSpan = nullable2.Value;
        string str = "あと" + timeSpan.Minutes.ToLocalizeNumberText() + "分";
        buyLimitText.text = str;
      }
    }
  }

  private void SetReceiveLimit(int? day, DateTime now)
  {
    if (!day.HasValue)
      return;
    int? nullable = day;
    int num = 0;
    if (nullable.GetValueOrDefault() > num & nullable.HasValue)
    {
      this.buyLimitText.text = "あと" + day.ToString() + "日";
    }
    else
    {
      TimeSpan timeSpan = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59) - now;
      if (timeSpan.Hours > 0)
        this.buyLimitText.text = string.Format("あと{0}時間", (object) timeSpan.Hours);
      else
        this.buyLimitText.text = string.Format("あと{0}分", (object) timeSpan.Minutes);
    }
  }

  public void OnIconLoupe() => this.StartCoroutine(this.ShowIconLoupe());

  private IEnumerator ShowIconLoupe()
  {
    Future<GameObject> f = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = f.Result.Clone();
    popup.SetActive(false);
    e = popup.GetComponent<ItemDetailPopupBase>().SetInfo(MasterDataTable.CommonRewardType.coin, 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    popup.SetActive(true);
  }

  public void OnItemDetailButton()
  {
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.ShowItemDetailPopup());
  }

  private IEnumerator ShowItemDetailPopup()
  {
    Scroll0079 scroll0079 = this;
    Future<GameObject> detailPopupf = Res.Prefabs.popup.popup_006_3_1__anim_popup01.Load<GameObject>();
    IEnumerator e = detailPopupf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Popup00631Menu component = Singleton<PopupManager>.GetInstance().open(detailPopupf.Result).GetComponent<Popup00631Menu>();
    if (scroll0079.simplePackInfo != null)
    {
      e = component.InitGachaDetail(scroll0079.TxtItemname.text, scroll0079.simplePackInfo.descriptions);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (scroll0079.beginnerPackInfo != null)
    {
      e = component.InitGachaDetail(scroll0079.TxtItemname.text, scroll0079.beginnerPackInfo.descriptions);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (scroll0079.coinbonusHistoryCoinBonusDetails != null)
    {
      e = component.InitGachaDetail(scroll0079.TxtItemname.text, scroll0079.coinbonusHistoryCoinBonusDetails.details);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      CoinProductDetail[] array = ((IEnumerable<CoinProductDetail>) MasterData.CoinProductDetailList).Where<CoinProductDetail>(new Func<CoinProductDetail, bool>(scroll0079.\u003CShowItemDetailPopup\u003Eb__38_0)).ToArray<CoinProductDetail>();
      e = component.InitGachaDetail(scroll0079.TxtItemname.text, array);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void OnBuyOrReceive()
  {
    if (this.isBuy)
      this.StartCoroutine(this.DoBuy());
    else
      this.StartCoroutine(this.DoReceive());
  }

  private IEnumerator DoBuy()
  {
    Scroll0079 scroll0079 = this;
    Future<WebAPI.Response.CoinbonusPackVerifyCheck> handler;
    IEnumerator e;
    if (scroll0079.simplePackInfo != null)
    {
      handler = WebAPI.CoinbonusPackVerifyCheck(1, scroll0079.simplePackInfo.pack.id, 0);
      e = handler.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (handler.Result == null)
        yield break;
      else
        handler = (Future<WebAPI.Response.CoinbonusPackVerifyCheck>) null;
    }
    else if (scroll0079.beginnerPackInfo != null)
    {
      handler = WebAPI.CoinbonusPackVerifyCheck(5, scroll0079.beginnerPackInfo.pack.id, 0);
      e = handler.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (handler.Result == null)
        yield break;
      else
        handler = (Future<WebAPI.Response.CoinbonusPackVerifyCheck>) null;
    }
    if (PurchaseFlow.Instance.Purchase(scroll0079.productInfo.ProductId))
      scroll0079.StartCoroutine(PurchaseBehaviorLoadingLayer.Enable());
  }

  private IEnumerator DoReceive()
  {
    PurchaseBehavior.PopupDismiss();
    yield return (object) Singleton<NGSceneManager>.GetInstance().StartCoroutine(Shop0079Menu.OnPurchaseOrReciveSucceeded(false, this.coinProduct, 0));
  }
}
