// Decompiled with JetBrains decompiler
// Type: Shop0079Menu
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
public class Shop0079Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtPopupkisekinum;
  [SerializeField]
  private SpreadColorButton[] tabButtons;
  [SerializeField]
  private UISprite[] tabOn;
  [SerializeField]
  private UISprite[] tabOff;
  private List<MonoBehaviour> tabPages = new List<MonoBehaviour>();
  private List<bool> tabPagesInit = new List<bool>();
  [SerializeField]
  private GameObject chargeConspicuousMark;
  [SerializeField]
  private GameObject accumulationConspicuousMark;
  [SerializeField]
  private GameObject giftWeekConspicuousMark;
  [SerializeField]
  private GameObject giftMonthConspicuousMark;
  [SerializeField]
  private ShopCharge charge;
  [SerializeField]
  private ShopAccumulation accumulation;
  [SerializeField]
  private ShopGiftWeek giftWeek;
  [SerializeField]
  private ShopGiftMonth giftMonth;
  private static WebAPI.Response.CoinbonusHistory coinbonusHistory;
  public static ShopTabType CurrentTabType;
  public static bool IsBuyOrReceiveBack;
  private long playerRevision = -1;
  private GameObject withLoupeIcon;

  public IEnumerator Init(WebAPI.Response.CoinbonusHistory history)
  {
    Shop0079Menu shop0079Menu = this;
    Shop0079Menu.coinbonusHistory = history;
    shop0079Menu.tabPages.Add((MonoBehaviour) shop0079Menu.charge);
    shop0079Menu.tabPages.Add((MonoBehaviour) shop0079Menu.accumulation);
    shop0079Menu.tabPages.Add((MonoBehaviour) shop0079Menu.giftWeek);
    shop0079Menu.tabPages.Add((MonoBehaviour) shop0079Menu.giftMonth);
    foreach (MonoBehaviour tabPage in shop0079Menu.tabPages)
      shop0079Menu.tabPagesInit.Add(false);
    yield return (object) shop0079Menu.charge.Init(((Component) shop0079Menu).gameObject.GetComponent<PurchaseView>(), Shop0079Menu.coinbonusHistory);
    ShopTabType firstTab = shop0079Menu.InitTab();
    switch (firstTab)
    {
      case ShopTabType.Charge:
        yield return (object) shop0079Menu.InitChargeTab();
        break;
      case ShopTabType.Accumulation:
        yield return (object) shop0079Menu.InitAccumulationTab();
        break;
      case ShopTabType.GiftWeek:
        yield return (object) shop0079Menu.InitGiftWeekTab();
        break;
      case ShopTabType.GiftMonth:
        yield return (object) shop0079Menu.InitGiftMonthTab();
        break;
    }
    shop0079Menu.ChangeTab(firstTab);
  }

  private ShopTabType InitTab()
  {
    List<bool> source = new List<bool>();
    bool flag1 = false;
    bool flag2 = false;
    foreach (SimplePackInfo simplePack in Shop0079Menu.coinbonusHistory.simple_packs)
    {
      if (simplePack.player_pack.rest_receive_day.HasValue && !simplePack.player_pack.is_received)
      {
        flag1 = true;
        break;
      }
    }
    foreach (BeginnerPackInfo beginnerPack in Shop0079Menu.coinbonusHistory.beginner_packs)
    {
      if (beginnerPack.player_pack.rest_receive_day.HasValue && !beginnerPack.player_pack.is_received)
        flag1 = true;
      if (beginnerPack.player_pack.purchased_count <= 0)
        flag2 = true;
    }
    source.Add(flag1);
    this.chargeConspicuousMark.SetActive(flag1);
    bool flag3 = false;
    if (Shop0079Menu.coinbonusHistory.stepup_packs.Length == 0)
    {
      ((Component) this.tabButtons[1]).gameObject.SetActive(false);
    }
    else
    {
      foreach (StepupPackInfo stepupPack in Shop0079Menu.coinbonusHistory.stepup_packs)
      {
        if (stepupPack.player_pack.rest_receive_day.HasValue && !stepupPack.player_pack.is_received)
        {
          flag3 = true;
          break;
        }
      }
    }
    source.Add(flag3);
    this.accumulationConspicuousMark.SetActive(flag3);
    bool flag4 = false;
    if (Shop0079Menu.coinbonusHistory.weekly_packs.Length == 0)
    {
      ((Component) this.tabButtons[2]).gameObject.SetActive(false);
    }
    else
    {
      foreach (WeeklyPackInfo weeklyPack in Shop0079Menu.coinbonusHistory.weekly_packs)
      {
        if (weeklyPack.player_pack.rest_receive_day.HasValue && !weeklyPack.player_pack.is_received)
        {
          flag4 = true;
          break;
        }
      }
    }
    source.Add(flag4);
    this.giftWeekConspicuousMark.SetActive(flag4);
    bool flag5 = false;
    if (Shop0079Menu.coinbonusHistory.monthly_packs.Length == 0)
    {
      ((Component) this.tabButtons[3]).gameObject.SetActive(false);
    }
    else
    {
      foreach (MonthlyPackInfo monthlyPack in Shop0079Menu.coinbonusHistory.monthly_packs)
      {
        if (monthlyPack.player_pack.rest_receive_day.HasValue && !monthlyPack.player_pack.is_received)
        {
          flag5 = true;
          break;
        }
      }
    }
    source.Add(flag5);
    this.giftMonthConspicuousMark.SetActive(flag5);
    Singleton<NGGameDataManager>.GetInstance().newbiePacks = flag2;
    Singleton<NGGameDataManager>.GetInstance().receivableGift = source.Any<bool>((Func<bool, bool>) (x => x));
    Singleton<CommonRoot>.GetInstance().UpdateHeaderBikkuriIcon();
    Singleton<CommonRoot>.GetInstance().UpdateFooterBikkuriIcon();
    Singleton<CommonRoot>.GetInstance().UpdateFooterNewbiePacksIcon();
    Singleton<CommonRoot>.GetInstance().GetTowerHeaderComponent().UpdateHeaderBikkuriIcon();
    Singleton<CommonRoot>.GetInstance().GetSeaHeaderComponent().UpdateHeaderBikkuriIcon();
    if (Shop0079Menu.IsBuyOrReceiveBack)
    {
      Shop0079Menu.IsBuyOrReceiveBack = false;
      return Shop0079Menu.CurrentTabType;
    }
    if (source[0])
      return ShopTabType.Charge;
    if (source[1])
      return ShopTabType.Accumulation;
    if (source[2])
      return ShopTabType.GiftWeek;
    return source[3] ? ShopTabType.GiftMonth : ShopTabType.Charge;
  }

  private IEnumerator InitChargeTab()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Shop0079Menu shop0079Menu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      shop0079Menu.tabPagesInit[0] = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) shop0079Menu.charge.Init(((Component) shop0079Menu).gameObject.GetComponent<PurchaseView>(), Shop0079Menu.coinbonusHistory);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator InitAccumulationTab()
  {
    yield return (object) this.accumulation.Init(Shop0079Menu.coinbonusHistory);
    this.tabPagesInit[1] = true;
  }

  private IEnumerator InitGiftWeekTab()
  {
    yield return (object) this.giftWeek.Init(Shop0079Menu.coinbonusHistory);
    this.tabPagesInit[2] = true;
  }

  private IEnumerator InitGiftMonthTab()
  {
    yield return (object) this.giftMonth.Init(Shop0079Menu.coinbonusHistory);
    this.tabPagesInit[3] = true;
  }

  private void ChangeTab(ShopTabType tabType)
  {
    Shop0079Menu.CurrentTabType = tabType;
    for (int index = 0; index < this.tabButtons.Length; ++index)
    {
      if ((ShopTabType) index == tabType)
      {
        this.tabButtons[index].SetSprite(this.tabButtons[index].normalSprite);
        ((Component) this.tabOn[index]).gameObject.SetActive(true);
        ((Component) this.tabOff[index]).gameObject.SetActive(false);
      }
      else
      {
        this.tabButtons[index].SetSprite(this.tabButtons[index].disabledSprite);
        ((Component) this.tabOn[index]).gameObject.SetActive(false);
        ((Component) this.tabOff[index]).gameObject.SetActive(true);
      }
    }
    for (int index = 0; index < this.tabPages.Count; ++index)
    {
      if ((ShopTabType) index == tabType)
        ((Component) this.tabPages[index]).gameObject.SetActive(true);
      else
        ((Component) this.tabPages[index]).gameObject.SetActive(false);
    }
    if (this.tabPagesInit[(int) tabType])
      return;
    switch (tabType)
    {
      case ShopTabType.Charge:
        this.StartCoroutine(this.InitChargeTab());
        break;
      case ShopTabType.Accumulation:
        this.StartCoroutine(this.InitAccumulationTab());
        break;
      case ShopTabType.GiftWeek:
        this.StartCoroutine(this.InitGiftWeekTab());
        break;
      case ShopTabType.GiftMonth:
        this.StartCoroutine(this.InitGiftMonthTab());
        break;
    }
  }

  protected override void Update()
  {
    base.Update();
    long num = SMManager.Revision<Player>();
    if (this.playerRevision != num)
      this.TxtPopupkisekinum.SetTextLocalize(SMManager.Get<Player>().coin - PurchaseBehavior.UsedCoinInBattleHere);
    this.playerRevision = num;
  }

  public void OnChargeTab()
  {
    if (Shop0079Menu.CurrentTabType == ShopTabType.Charge)
      return;
    this.ChangeTab(ShopTabType.Charge);
  }

  public void OnAccumulationTab()
  {
    if (Shop0079Menu.CurrentTabType == ShopTabType.Accumulation)
      return;
    this.ChangeTab(ShopTabType.Accumulation);
  }

  public void OnGiftWeekTab()
  {
    if (Shop0079Menu.CurrentTabType == ShopTabType.GiftWeek)
      return;
    this.ChangeTab(ShopTabType.GiftWeek);
  }

  public void OnGiftMonthTab()
  {
    if (Shop0079Menu.CurrentTabType == ShopTabType.GiftMonth)
      return;
    this.ChangeTab(ShopTabType.GiftMonth);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    PurchaseBehavior.PopupDismiss();
    SeaGlobalMenu objectOfType1 = Object.FindObjectOfType<SeaGlobalMenu>();
    if (Object.op_Inequality((Object) objectOfType1, (Object) null))
      objectOfType1.UpdateBikkuriIcon();
    ShopTopMenu objectOfType2 = Object.FindObjectOfType<ShopTopMenu>();
    if (!Object.op_Inequality((Object) objectOfType2, (Object) null))
      return;
    objectOfType2.UpdateCampaign();
  }

  public override void onBackButton() => this.IbtnNo();

  public void IbtnFonds() => this.StartCoroutine(PopupUtility._007_19());

  public void IbtnSpecific() => this.StartCoroutine(PopupUtility._007_18());

  public static void OnPurchaseSucceeded(ProductInfo productInfo, int currentCoin, int addCoin)
  {
    PurchaseBehavior.PopupDismiss(true);
    CoinProduct activeProductData = MasterData.CoinProductList.GetActiveProductData(productInfo.ProductId);
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    if (activeProductData.type == 1)
      Singleton<CommonRoot>.GetInstance().StartCoroutine(Shop0079Menu.onPurchaseSucceeded(productInfo, currentCoin, addCoin));
    else
      Singleton<CommonRoot>.GetInstance().StartCoroutine(Shop0079Menu.OnPurchaseOrReciveSucceeded(true, activeProductData, addCoin));
  }

  private static IEnumerator onPurchaseSucceeded(
    ProductInfo productInfo,
    int currentCoin,
    int addCoin)
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_007_11__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = PurchaseBehavior.PopupOpen(prefabF.Result).GetComponent<Shop00711Menu>().Init(productInfo, currentCoin - addCoin, currentCoin);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public static IEnumerator OnReciveSucceeded(int type, int masterPackId)
  {
    yield return (object) Shop0079Menu.NormalRecive(type, masterPackId, 0);
  }

  public static IEnumerator OnPurchaseOrReciveSucceeded(
    bool isBuy,
    CoinProduct coinProduct,
    int addCoin)
  {
    int masterPackId = 0;
    if (coinProduct.type == 2)
    {
      foreach (SimplePackInfo simplePack in Shop0079Menu.coinbonusHistory.simple_packs)
      {
        SimplePackInfo packInfo = simplePack;
        if (((IEnumerable<CoinGroup>) Shop0079Menu.coinbonusHistory.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == packInfo.pack.coin_group_id)).GetCoinId() == coinProduct.ID)
        {
          masterPackId = packInfo.pack.id;
          break;
        }
      }
    }
    else if (coinProduct.type == 6)
    {
      foreach (BeginnerPackInfo beginnerPack in Shop0079Menu.coinbonusHistory.beginner_packs)
      {
        BeginnerPackInfo packInfo = beginnerPack;
        if (((IEnumerable<CoinGroup>) Shop0079Menu.coinbonusHistory.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == packInfo.pack.coin_group_id)).GetCoinId() == coinProduct.ID)
        {
          masterPackId = packInfo.pack.id;
          break;
        }
      }
    }
    else if (coinProduct.type == 3)
    {
      foreach (StepupPackInfo stepupPack in Shop0079Menu.coinbonusHistory.stepup_packs)
      {
        foreach (StepupPackInfoPack_steps packStep in stepupPack.pack_steps)
        {
          StepupPackInfoPack_steps step = packStep;
          if (((IEnumerable<CoinGroup>) Shop0079Menu.coinbonusHistory.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == step.pack_set.coin_group_id)).GetCoinId() == coinProduct.ID)
          {
            masterPackId = stepupPack.pack.id;
            break;
          }
        }
      }
    }
    else if (coinProduct.type == 4)
    {
      foreach (WeeklyPackInfo weeklyPack in Shop0079Menu.coinbonusHistory.weekly_packs)
      {
        WeeklyPackInfo packInfo = weeklyPack;
        if (((IEnumerable<CoinGroup>) Shop0079Menu.coinbonusHistory.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == packInfo.pack.coin_group_id)).GetCoinId() == coinProduct.ID)
        {
          masterPackId = packInfo.pack.id;
          break;
        }
      }
    }
    else if (coinProduct.type == 5)
    {
      foreach (MonthlyPackInfo monthlyPack in Shop0079Menu.coinbonusHistory.monthly_packs)
      {
        MonthlyPackInfo packInfo = monthlyPack;
        if (((IEnumerable<CoinGroup>) Shop0079Menu.coinbonusHistory.coin_groups).First<CoinGroup>((Func<CoinGroup, bool>) (x => x.id == packInfo.pack.coin_group_id)).GetCoinId() == coinProduct.ID)
        {
          masterPackId = packInfo.pack.id;
          break;
        }
      }
    }
    else if (coinProduct.type != 7)
    {
      Debug.LogError((object) string.Format("OnPurchaseOrReciveSucceeded Error: 想定していないcoinProduct.typeです. {0}", (object) coinProduct.type));
      yield break;
    }
    IEnumerator e;
    if (isBuy)
    {
      if (coinProduct.type == 7)
      {
        Future<WebAPI.Response.HotdealPresentPack> handler = WebAPI.HotdealPresentPack(Singleton<NGGameDataManager>.GetInstance().PurchaseHotDeal.pack_id);
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (handler.Result == null)
        {
          Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
          NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
          instance.PurchaseHotDeal.is_modal = false;
          instance.PurchaseHotDeal = (HotDealInfo) null;
          Singleton<PopupManager>.GetInstance().closeAll();
        }
        else
        {
          yield return (object) Shop0079Menu.PurchaseOrReciveSucceededResult(handler.Result, addCoin);
          handler = (Future<WebAPI.Response.HotdealPresentPack>) null;
        }
      }
      else
      {
        Future<WebAPI.Response.CoinbonusPresentPack> handler = WebAPI.CoinbonusPresentPack();
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) Shop0079Menu.PurchaseOrReciveSucceededResult(handler.Result, addCoin);
        handler = (Future<WebAPI.Response.CoinbonusPresentPack>) null;
      }
    }
    else
      yield return (object) Shop0079Menu.NormalRecive(coinProduct.type, masterPackId, addCoin);
  }

  private static IEnumerator NormalRecive(int productType, int masterPackId, int addCoin)
  {
    Future<WebAPI.Response.CoinbonusReceivePackReward> handler;
    IEnumerator e;
    switch (productType)
    {
      case 2:
        handler = WebAPI.CoinbonusReceivePackReward(1, masterPackId);
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) Shop0079Menu.PurchaseOrReciveSucceededResult(handler.Result, addCoin);
        handler = (Future<WebAPI.Response.CoinbonusReceivePackReward>) null;
        break;
      case 3:
        handler = WebAPI.CoinbonusReceivePackReward(2, masterPackId);
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) Shop0079Menu.PurchaseOrReciveSucceededResult(handler.Result, addCoin);
        handler = (Future<WebAPI.Response.CoinbonusReceivePackReward>) null;
        break;
      case 4:
        handler = WebAPI.CoinbonusReceivePackReward(3, masterPackId);
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) Shop0079Menu.PurchaseOrReciveSucceededResult(handler.Result, addCoin);
        handler = (Future<WebAPI.Response.CoinbonusReceivePackReward>) null;
        break;
      case 5:
        handler = WebAPI.CoinbonusReceivePackReward(4, masterPackId);
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) Shop0079Menu.PurchaseOrReciveSucceededResult(handler.Result, addCoin);
        handler = (Future<WebAPI.Response.CoinbonusReceivePackReward>) null;
        break;
      case 6:
        handler = WebAPI.CoinbonusReceivePackReward(5, masterPackId);
        e = handler.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) Shop0079Menu.PurchaseOrReciveSucceededResult(handler.Result, addCoin);
        handler = (Future<WebAPI.Response.CoinbonusReceivePackReward>) null;
        break;
    }
  }

  private static IEnumerator PurchaseOrReciveSucceededResult(
    WebAPI.Response.CoinbonusPresentPack response,
    int addCoin)
  {
    List<ShopItemGetResultInfo> rewardList = new List<ShopItemGetResultInfo>();
    foreach (WebAPI.Response.CoinbonusPresentPackRewards reward in response.rewards)
      rewardList.Add(new ShopItemGetResultInfo((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity));
    yield return (object) Shop0079Menu.DoPurchaseOrReciveSucceededResult(rewardList, response.player_presents.Length != 0, addCoin);
  }

  private static IEnumerator PurchaseOrReciveSucceededResult(
    WebAPI.Response.HotdealPresentPack response,
    int addCoin)
  {
    List<ShopItemGetResultInfo> rewardList = new List<ShopItemGetResultInfo>();
    foreach (GiftRewardSchema reward in response.rewards)
      rewardList.Add(new ShopItemGetResultInfo((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity));
    yield return (object) Shop0079Menu.DoPurchaseOrReciveSucceededResult(rewardList, response.player_presents.Length != 0, addCoin, true);
  }

  private static IEnumerator PurchaseOrReciveSucceededResult(
    WebAPI.Response.CoinbonusReceivePackReward response,
    int addCoin)
  {
    List<ShopItemGetResultInfo> rewardList = new List<ShopItemGetResultInfo>();
    foreach (WebAPI.Response.CoinbonusReceivePackRewardRewards reward in response.rewards)
      rewardList.Add(new ShopItemGetResultInfo((MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity));
    yield return (object) Shop0079Menu.DoPurchaseOrReciveSucceededResult(rewardList, response.player_presents.Length != 0, addCoin);
  }

  private static IEnumerator DoPurchaseOrReciveSucceededResult(
    List<ShopItemGetResultInfo> rewardList,
    bool isOverflow,
    int addCoin,
    bool isHotDeal = false)
  {
    if (addCoin > 0)
      rewardList.Insert(0, new ShopItemGetResultInfo(MasterDataTable.CommonRewardType.coin, 0, addCoin));
    if (rewardList.Count > 11)
      rewardList.RemoveRange(11, rewardList.Count - 11);
    Future<GameObject> prefabF = new ResourceObject("Prefabs/shop007_9/dir_Result_Shop_Get_reward").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopItemGetResult component = PurchaseBehavior.PopupOpen(prefabF.Result).GetComponent<ShopItemGetResult>();
    if (isHotDeal)
    {
      e = component.Init(rewardList, isOverflow, new Action(Shop0079Menu.OnFinishHotDealItemGetResult));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = component.Init(rewardList, isOverflow, new Action(Shop0079Menu.OnFinishShopItemGetResult));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  private static void OnFinishShopItemGetResult()
  {
    PurchaseBehavior.PopupDismiss();
    if (PurchaseBehavior.IsBattleNow)
      return;
    Shop0079Menu.IsBuyOrReceiveBack = true;
    Singleton<NGSceneManager>.GetInstance().StartCoroutine(PopupUtility.BuyKiseki(PurchaseBehavior.IsBattleNow));
  }

  private static void OnFinishHotDealItemGetResult()
  {
    NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
    HotDealInfo purchaseHotDeal = instance.PurchaseHotDeal;
    purchaseHotDeal.IsPurchased = true;
    purchaseHotDeal.is_modal = false;
    instance.PurchaseHotDeal = (HotDealInfo) null;
    if (((IEnumerable<HotDealInfo>) instance.HotDealInfo).Any<HotDealInfo>((Func<HotDealInfo, bool>) (x => x.IsActive)))
      Singleton<PopupManager>.GetInstance().dismiss();
    else
      Singleton<PopupManager>.GetInstance().closeAll();
  }
}
