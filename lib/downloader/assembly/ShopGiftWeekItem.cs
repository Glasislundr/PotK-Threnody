// Decompiled with JetBrains decompiler
// Type: ShopGiftWeekItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Purchase;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class ShopGiftWeekItem : MonoBehaviour
{
  [SerializeField]
  private UILabel giftName;
  [SerializeField]
  private GameObject kisekiInfo;
  [SerializeField]
  private UILabel kisekiInfoLabel;
  [SerializeField]
  private Transform rewardIconsParent;
  [SerializeField]
  private Transform rewardIcons1Row;
  [SerializeField]
  private Transform rewardIcons2Row;
  [SerializeField]
  private GameObject receiveRestDay;
  [SerializeField]
  private UILabel receiveRestDayText;
  [SerializeField]
  private UIButton priceOrReceiveButton;
  [SerializeField]
  private UILabel priceOrReceiveLabel;
  [SerializeField]
  private GameObject buyLimitPeriod;
  [SerializeField]
  private UILabel buyPeriodLimitText;
  private WeeklyPackInfo packInfo;
  private string productId;
  private bool isBuy;

  public IEnumerator Init(
    GameObject baseIcon,
    WeeklyPackInfo packInfo,
    string productId,
    string price,
    DateTime now)
  {
    ShopGiftWeekItem shopGiftWeekItem = this;
    shopGiftWeekItem.packInfo = packInfo;
    shopGiftWeekItem.giftName.text = packInfo.pack.name;
    shopGiftWeekItem.productId = productId;
    float rewardPositionX = 0.0f;
    int i;
    for (i = 0; i < 5 && i < packInfo.rewards.Length; ++i)
    {
      yield return (object) shopGiftWeekItem.CreateIcon(baseIcon, shopGiftWeekItem.rewardIcons1Row, packInfo.rewards[i], rewardPositionX);
      rewardPositionX += 74f;
    }
    rewardPositionX = 0.0f;
    for (i = 5; i < 10 && i < packInfo.rewards.Length; ++i)
    {
      yield return (object) shopGiftWeekItem.CreateIcon(baseIcon, shopGiftWeekItem.rewardIcons2Row, packInfo.rewards[i], rewardPositionX);
      rewardPositionX += 74f;
    }
    if (packInfo.pack.purchase_limit > 0)
    {
      TimeSpan timeSpan = packInfo.pack.end_at.Value - now;
      if (timeSpan.Days > 0)
      {
        shopGiftWeekItem.buyPeriodLimitText.text = string.Format("あと{0}日", (object) timeSpan.Days);
        shopGiftWeekItem.buyLimitPeriod.SetActive(true);
      }
      else if (timeSpan.Hours > 0)
      {
        shopGiftWeekItem.buyPeriodLimitText.text = string.Format("あと{0}時間", (object) timeSpan.Hours);
        shopGiftWeekItem.buyLimitPeriod.SetActive(true);
      }
      else if (timeSpan.Minutes >= 0)
      {
        shopGiftWeekItem.buyPeriodLimitText.text = string.Format("あと{0}分", (object) timeSpan.Minutes);
        shopGiftWeekItem.buyLimitPeriod.SetActive(true);
      }
      else
        shopGiftWeekItem.buyLimitPeriod.SetActive(false);
    }
    else
      shopGiftWeekItem.buyLimitPeriod.SetActive(false);
    if (!packInfo.player_pack.rest_receive_day.HasValue)
    {
      shopGiftWeekItem.receiveRestDay.SetActive(false);
      ((Component) shopGiftWeekItem.receiveRestDayText).gameObject.SetActive(false);
    }
    else
    {
      shopGiftWeekItem.receiveRestDay.SetActive(true);
      ((Component) shopGiftWeekItem.receiveRestDayText).gameObject.SetActive(true);
      int? restReceiveDay = packInfo.player_pack.rest_receive_day;
      int num = 0;
      if (restReceiveDay.GetValueOrDefault() > num & restReceiveDay.HasValue)
      {
        shopGiftWeekItem.receiveRestDayText.text = string.Format("残り{0}日", (object) packInfo.player_pack.rest_receive_day);
      }
      else
      {
        TimeSpan timeSpan = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59) - now;
        shopGiftWeekItem.receiveRestDayText.text = timeSpan.Hours <= 0 ? string.Format("残り{0}分", (object) timeSpan.Minutes) : string.Format("残り{0}時間", (object) timeSpan.Hours);
      }
    }
    if (packInfo.player_pack.is_purchasable)
    {
      shopGiftWeekItem.isBuy = true;
      shopGiftWeekItem.priceOrReceiveLabel.SetTextLocalize(price.Replace("\\", "￥"));
    }
    else if (packInfo.player_pack.rest_receive_day.HasValue)
    {
      shopGiftWeekItem.isBuy = false;
      shopGiftWeekItem.priceOrReceiveLabel.text = !packInfo.player_pack.is_received ? "受取" : "受取済";
    }
    if (shopGiftWeekItem.isBuy)
    {
      shopGiftWeekItem.kisekiInfo.SetActive(true);
      CoinProduct activeProductData = MasterData.CoinProductList.GetActiveProductData(productId);
      shopGiftWeekItem.kisekiInfoLabel.text = string.Format("購入時、有償石{0}個と特典付与", (object) activeProductData.additional_paid_coin);
    }
    else
      shopGiftWeekItem.kisekiInfo.SetActive(false);
    if (!shopGiftWeekItem.isBuy)
    {
      Transform transform1 = ((Component) shopGiftWeekItem.rewardIcons1Row).gameObject.transform;
      transform1.localPosition = new Vector3(transform1.localPosition.x, transform1.localPosition.y + 48f, 0.0f);
      Transform transform2 = ((Component) shopGiftWeekItem.rewardIcons2Row).gameObject.transform;
      transform2.localPosition = new Vector3(transform2.localPosition.x, transform2.localPosition.y + 48f, 0.0f);
      Transform transform3 = shopGiftWeekItem.receiveRestDay.transform;
      transform3.localPosition = new Vector3(transform3.localPosition.x, transform3.localPosition.y + 48f, 0.0f);
      Transform transform4 = ((Component) shopGiftWeekItem.receiveRestDayText).transform;
      transform4.localPosition = new Vector3(transform4.localPosition.x, transform4.localPosition.y + 48f, 0.0f);
      Transform transform5 = ((Component) shopGiftWeekItem.priceOrReceiveButton).transform;
      transform5.localPosition = new Vector3(transform5.localPosition.x, transform5.localPosition.y + 48f, 0.0f);
      shopGiftWeekItem.rewardIconsParent.localPosition = new Vector3(shopGiftWeekItem.rewardIconsParent.localPosition.x, -57f, 0.0f);
    }
    if (packInfo.player_pack.is_purchasable || !packInfo.player_pack.is_received)
      ((UIButtonColor) shopGiftWeekItem.priceOrReceiveButton).isEnabled = true;
    else
      ((UIButtonColor) shopGiftWeekItem.priceOrReceiveButton).isEnabled = false;
    UISprite component1 = ((Component) shopGiftWeekItem).GetComponent<UISprite>();
    if (packInfo.rewards.Length <= 5)
    {
      if (!shopGiftWeekItem.isBuy)
        ((UIWidget) component1).height = 224;
    }
    else if (shopGiftWeekItem.isBuy)
      ((UIWidget) component1).height = 384;
    else
      ((UIWidget) component1).height = 333;
    BoxCollider component2 = ((Component) shopGiftWeekItem).GetComponent<BoxCollider>();
    component2.size = new Vector3((float) ((UIWidget) component1).width, (float) ((UIWidget) component1).height);
    component2.center = new Vector3(0.0f, (float) (-((UIWidget) component1).height / 2));
    if (packInfo.rewards.Length <= 5)
      ((Component) shopGiftWeekItem.rewardIcons2Row).gameObject.SetActive(false);
  }

  private IEnumerator CreateIcon(
    GameObject baseIcon,
    Transform parent,
    WeeklyPackReward reward,
    float rewardPositionX)
  {
    GameObject scrollItem = baseIcon.Clone(parent);
    IEnumerator e = scrollItem.GetComponent<ItemIconDetail>().Init((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity, iconInfo: new TransformSizeInfo(0.54f, 46f, -48f), quantityInfo: new TransformSizeInfo(1f, 36f, -90f), quantitySpriteInfo: new SpriteTransformSizeInfo(10f, 4f, 26, 72), stoneInfo: new TransformSizeInfo(1f, 54.5f, -11f));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Transform transform = scrollItem.transform;
    transform.localPosition = new Vector3(rewardPositionX, transform.localPosition.y, 0.0f);
  }

  public void OnDetailButton()
  {
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.ShowOnDetailButton());
  }

  private IEnumerator ShowOnDetailButton()
  {
    Future<GameObject> detailPopupf = Res.Prefabs.popup.popup_006_3_1__anim_popup01.Load<GameObject>();
    IEnumerator e = detailPopupf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(detailPopupf.Result).GetComponent<Popup00631Menu>().InitGachaDetail("週パック", this.packInfo.descriptions);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
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
    ShopGiftWeekItem shopGiftWeekItem = this;
    Future<WebAPI.Response.CoinbonusPackVerifyCheck> handler = WebAPI.CoinbonusPackVerifyCheck(3, shopGiftWeekItem.packInfo.pack.id, 0);
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (handler.Result != null && PurchaseFlow.Instance.Purchase(shopGiftWeekItem.productId))
      shopGiftWeekItem.StartCoroutine(PurchaseBehaviorLoadingLayer.Enable());
  }

  private IEnumerator DoReceive()
  {
    PurchaseBehavior.PopupDismiss(true);
    CoinProduct activeProductData = MasterData.CoinProductList.GetActiveProductData(this.productId);
    yield return (object) Singleton<NGSceneManager>.GetInstance().StartCoroutine(Shop0079Menu.OnReciveSucceeded(activeProductData.type, this.packInfo.pack.id));
  }
}
