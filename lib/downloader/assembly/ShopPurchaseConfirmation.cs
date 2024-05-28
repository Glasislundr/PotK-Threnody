// Decompiled with JetBrains decompiler
// Type: ShopPurchaseConfirmation
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
public class ShopPurchaseConfirmation : BackButtonMenuBase
{
  [Header("上段フレーム")]
  [SerializeField]
  private Transform ThumParent;
  [SerializeField]
  private GameObject Loupe;
  [SerializeField]
  private UI2DSprite PackThum;
  [SerializeField]
  private GameObject PackThumMount;
  [SerializeField]
  private UILabel ItemName;
  [SerializeField]
  private UILabel BuyLimit;
  [SerializeField]
  private UIGrid MarkGrid;
  [SerializeField]
  private UI2DSprite Decoration;
  [SerializeField]
  private GameObject PackMark;
  [SerializeField]
  private UILabel HaveItemCount;
  [SerializeField]
  private UIScrollView DescriptionScrollView;
  [SerializeField]
  private UILabel Description;
  [Header("中段 アイテム一覧 スクロール")]
  [SerializeField]
  private UIScrollView ScrollView;
  [SerializeField]
  private UIGrid Grid;
  [Header("中段 購入数調整 スライダー")]
  [SerializeField]
  private UILabel CurrentSliderSelectCount;
  [SerializeField]
  private UISlider Slider;
  [SerializeField]
  private UILabel TxtSelectMin;
  [SerializeField]
  private UILabel TxtSelectMax;
  [SerializeField]
  private UIButton MinButton;
  [SerializeField]
  private UIButton MinusButton;
  [SerializeField]
  private UIButton MaxButton;
  [SerializeField]
  private UIButton PlusButton;
  [Header("下段フレーム")]
  [SerializeField]
  private GameObject Paid;
  [SerializeField]
  private UILabel PaidCoinHaveCount;
  [SerializeField]
  private UILabel FreeCoinHaveCount;
  [SerializeField]
  private GameObject Normal;
  [SerializeField]
  private UI2DSprite NormalPayTypeIcon;
  [SerializeField]
  private UILabel NormalHaveCount;
  [SerializeField]
  private UIButton BuyButton;
  [SerializeField]
  private UI2DSprite BuyPayTypeIcon;
  [SerializeField]
  private GameObject BuyPaid;
  [SerializeField]
  private UILabel BuyPayCount;
  [SerializeField]
  private GameObject SpecificAndFondsButton;
  private GameObject ItemPrefab;
  private ShopItemIconInfo info;
  private string shopTime;
  private Player player = SMManager.Get<Player>();
  private int selectMaxCount;
  private int selectedCount = 1;
  private int sliderCount;

  public IEnumerator Init(ShopItemIconInfo info, string shopTime)
  {
    ShopPurchaseConfirmation purchaseConfirmation = this;
    ((Component) purchaseConfirmation).gameObject.SetActive(false);
    purchaseConfirmation.info = info;
    purchaseConfirmation.shopTime = shopTime;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/shop007_4_1/dir_Pack_Item_List").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    purchaseConfirmation.ItemPrefab = prefabF.Result;
    if (Object.op_Inequality((Object) purchaseConfirmation.Loupe, (Object) null))
      purchaseConfirmation.Loupe.SetActive(true);
    if (info.playerShopArticle.icon_resource != "")
    {
      purchaseConfirmation.PackThumMount.SetActive(true);
      ((Component) purchaseConfirmation.PackThum).gameObject.SetActive(true);
      yield return (object) ShopCommon.CreateShopPack(purchaseConfirmation.PackThum, info.playerShopArticle.icon_resource);
    }
    else
      yield return (object) ShopCommon.CreateThum(purchaseConfirmation.ThumParent, info.CommonRewardType, info.RewardId);
    purchaseConfirmation.ItemName.text = info.ItemName;
    purchaseConfirmation.BuyLimit.text = ShopCommon.GetLimitCountText(info.LimitType, info.LimitCount);
    if (info.playerShopArticle.decoration_resource != "")
    {
      ((Component) purchaseConfirmation.Decoration).gameObject.SetActive(true);
      yield return (object) ShopCommon.CreateShopIcon(purchaseConfirmation.Decoration, info.playerShopArticle.decoration_resource);
    }
    else
      ((Component) purchaseConfirmation.Decoration).gameObject.SetActive(false);
    if (info.IsPack)
      purchaseConfirmation.PackMark.SetActive(true);
    if (Object.op_Inequality((Object) purchaseConfirmation.MarkGrid, (Object) null))
      purchaseConfirmation.MarkGrid.Reposition();
    if (Object.op_Inequality((Object) purchaseConfirmation.HaveItemCount, (Object) null))
    {
      long haveCount = CommonRewardType.GetHaveCount((MasterDataTable.CommonRewardType) info.playerShopArticle.contents[0].reward_type_id, info.playerShopArticle.contents[0].reward_id);
      purchaseConfirmation.HaveItemCount.text = haveCount.ToString();
    }
    purchaseConfirmation.Description.text = info.playerShopArticle.article_description;
    purchaseConfirmation.DescriptionScrollView.verticalScrollBar.value = 0.0f;
    purchaseConfirmation.DescriptionScrollView.ResetPosition();
    if (Object.op_Inequality((Object) purchaseConfirmation.Grid, (Object) null) && Object.op_Inequality((Object) purchaseConfirmation.ScrollView, (Object) null))
    {
      ((Component) purchaseConfirmation.Grid).gameObject.SetActive(false);
      for (int i = 0; i < info.playerShopArticle.contents.Length; ++i)
        yield return (object) purchaseConfirmation.ItemPrefab.Clone(((Component) purchaseConfirmation.Grid).transform).GetComponent<ShopPurchaseConfirmationItem>().Init(info.playerShopArticle.contents[i], purchaseConfirmation.ScrollView);
      ((Component) purchaseConfirmation.Grid).gameObject.SetActive(true);
      yield return (object) null;
      purchaseConfirmation.Grid.Reposition();
      purchaseConfirmation.ScrollView.ResetPosition();
    }
    if (Object.op_Inequality((Object) purchaseConfirmation.Slider, (Object) null))
    {
      purchaseConfirmation.TxtSelectMin.text = "1";
      int num1 = info.LimitCount.HasValue ? Mathf.Min(info.LimitCount.Value, 999) : 999;
      if (info.PayType != CommonPayType.paid_coin && info.PayType != CommonPayType.coin)
        num1 = Mathf.Min((int) (ShopCommon.GetHaveCount(info.PayType) / (long) info.PayCount), num1);
      int num2 = num1 <= 0 ? 1 : num1;
      purchaseConfirmation.TxtSelectMax.text = num2.ToString();
      purchaseConfirmation.selectMaxCount = num2;
      purchaseConfirmation.OnMin();
    }
    if (info.PayType == CommonPayType.paid_coin || info.PayType == CommonPayType.coin)
    {
      purchaseConfirmation.Paid.SetActive(true);
      purchaseConfirmation.Normal.SetActive(false);
      purchaseConfirmation.PaidCoinHaveCount.text = purchaseConfirmation.player.paid_coin.ToString();
      purchaseConfirmation.FreeCoinHaveCount.text = purchaseConfirmation.player.free_common_coin.ToString();
    }
    else
    {
      purchaseConfirmation.Paid.SetActive(false);
      purchaseConfirmation.Normal.SetActive(true);
      purchaseConfirmation.NormalPayTypeIcon.sprite2D = ShopCommon.GetPayTypeIcon(info.PayType);
      purchaseConfirmation.NormalHaveCount.text = ShopCommon.GetHaveCount(info.PayType).ToString();
      purchaseConfirmation.BuyPayTypeIcon.sprite2D = ShopCommon.GetPayTypeIcon(info.PayType);
    }
    if (info.LimitCount.HasValue)
    {
      int? limitCount = info.LimitCount;
      int num = 0;
      if (limitCount.GetValueOrDefault() <= num & limitCount.HasValue)
      {
        ((UIButtonColor) purchaseConfirmation.BuyButton).isEnabled = false;
        if (Object.op_Inequality((Object) purchaseConfirmation.Slider, (Object) null))
        {
          purchaseConfirmation.CurrentSliderSelectCount.text = "0/0";
          purchaseConfirmation.TxtSelectMin.text = "0";
          purchaseConfirmation.TxtSelectMax.text = "0";
          ((UIButtonColor) purchaseConfirmation.MinButton).isEnabled = false;
          ((UIButtonColor) purchaseConfirmation.MinusButton).isEnabled = false;
          ((UIButtonColor) purchaseConfirmation.MaxButton).isEnabled = false;
          ((UIButtonColor) purchaseConfirmation.PlusButton).isEnabled = false;
        }
      }
    }
    if (info.PayType == CommonPayType.paid_coin)
      purchaseConfirmation.BuyPaid.SetActive(true);
    else
      purchaseConfirmation.BuyPaid.SetActive(false);
    purchaseConfirmation.BuyPayCount.text = info.PayCount.ToString();
    if (info.PayType == CommonPayType.paid_coin || info.PayType == CommonPayType.coin)
      purchaseConfirmation.SpecificAndFondsButton.SetActive(true);
    else
      purchaseConfirmation.SpecificAndFondsButton.SetActive(false);
    ((Component) purchaseConfirmation).gameObject.SetActive(true);
  }

  public void OnIcon()
  {
    if (this.info.IsPack)
      return;
    if (this.info.playerShopArticle.icon_resource != "")
    {
      MasterDataTable.CommonRewardType rewardTypeId = (MasterDataTable.CommonRewardType) this.info.playerShopArticle.contents[0].reward_type_id;
      int rewardId = this.info.playerShopArticle.contents[0].reward_id;
      switch (this.info.CommonRewardType)
      {
        case MasterDataTable.CommonRewardType.gacha_ticket:
        case MasterDataTable.CommonRewardType.unit_ticket:
        case MasterDataTable.CommonRewardType.stamp:
        case MasterDataTable.CommonRewardType.challenge_point:
        case MasterDataTable.CommonRewardType.recovery_item:
          Singleton<CommonRoot>.GetInstance().StartCoroutine(ShopCommon.ShowSimpleDetailPopup(rewardTypeId, rewardId));
          break;
        default:
          Singleton<CommonRoot>.GetInstance().StartCoroutine(ShopCommon.ShowMoreDetailPopup(rewardTypeId, rewardId));
          break;
      }
    }
    else
    {
      switch (this.info.CommonRewardType)
      {
        case MasterDataTable.CommonRewardType.gacha_ticket:
        case MasterDataTable.CommonRewardType.unit_ticket:
        case MasterDataTable.CommonRewardType.stamp:
        case MasterDataTable.CommonRewardType.challenge_point:
        case MasterDataTable.CommonRewardType.recovery_item:
          Singleton<CommonRoot>.GetInstance().StartCoroutine(ShopCommon.ShowSimpleDetailPopup(this.info.CommonRewardType, this.info.RewardId));
          break;
        default:
          Singleton<CommonRoot>.GetInstance().StartCoroutine(ShopCommon.ShowMoreDetailPopup(this.info.CommonRewardType, this.info.RewardId));
          break;
      }
    }
  }

  public void SliderChange()
  {
    if (this.info.LimitCount.HasValue)
    {
      int? limitCount = this.info.LimitCount;
      int num = 0;
      if (limitCount.GetValueOrDefault() <= num & limitCount.HasValue)
        return;
    }
    this.sliderCount = Mathf.RoundToInt(((UIProgressBar) this.Slider).value * (float) (this.selectMaxCount - 1));
    this.ChangeCurrentValue();
  }

  private void ChangeCurrentValue()
  {
    if (this.selectMaxCount == 1)
    {
      this.selectedCount = 1;
      ((UIProgressBar) this.Slider).value = 1f;
    }
    else
    {
      this.selectedCount = this.sliderCount + 1;
      ((UIProgressBar) this.Slider).value = (float) this.sliderCount / ((float) this.selectMaxCount - 1f);
    }
    this.CurrentSliderSelectCount.text = string.Format("{0}/{1}", (object) this.selectedCount.ToString(), (object) this.selectMaxCount);
    this.BuyPayCount.text = (this.info.PayCount * this.selectedCount).ToString();
    if (this.selectMaxCount == 1)
    {
      ((UIButtonColor) this.MinusButton).isEnabled = false;
      ((UIButtonColor) this.PlusButton).isEnabled = false;
      ((UIButtonColor) this.MinButton).isEnabled = false;
      ((UIButtonColor) this.MaxButton).isEnabled = false;
    }
    else if (this.sliderCount <= 0)
    {
      ((UIButtonColor) this.MinusButton).isEnabled = false;
      ((UIButtonColor) this.PlusButton).isEnabled = true;
      ((UIButtonColor) this.MinButton).isEnabled = false;
      ((UIButtonColor) this.MaxButton).isEnabled = true;
    }
    else if (this.sliderCount >= this.selectMaxCount - 1)
    {
      ((UIButtonColor) this.MinusButton).isEnabled = true;
      ((UIButtonColor) this.PlusButton).isEnabled = false;
      ((UIButtonColor) this.MinButton).isEnabled = true;
      ((UIButtonColor) this.MaxButton).isEnabled = false;
    }
    else
    {
      ((UIButtonColor) this.MinusButton).isEnabled = true;
      ((UIButtonColor) this.PlusButton).isEnabled = true;
      ((UIButtonColor) this.MinButton).isEnabled = true;
      ((UIButtonColor) this.MaxButton).isEnabled = true;
    }
  }

  public void OnMin()
  {
    this.sliderCount = 0;
    this.ChangeCurrentValue();
  }

  public void OnMax()
  {
    this.sliderCount = this.selectMaxCount - 1;
    this.ChangeCurrentValue();
  }

  public void OnMinus()
  {
    --this.sliderCount;
    if (this.sliderCount <= 0)
      this.sliderCount = 0;
    this.ChangeCurrentValue();
  }

  public void OnPlus()
  {
    ++this.sliderCount;
    if (this.sliderCount >= this.selectMaxCount - 1)
      this.sliderCount = this.selectMaxCount - 1;
    this.ChangeCurrentValue();
  }

  public void IbtnFonds() => this.StartCoroutine(PopupUtility._007_19());

  public void IbtnSpecific() => this.StartCoroutine(PopupUtility._007_18());

  public void IbtnBuy()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    if (ShopCommon.GetHaveCount(this.info.PayType) < (long) (this.selectedCount * this.info.playerShopArticle.payment_amount))
    {
      this.StartCoroutine(this.OpenNotEnoughPopup());
    }
    else
    {
      if (this.info.playerShopArticle.article.shop.ID != 5000 && this.HaveMaxOver())
        return;
      this.StartCoroutine(this.ShowConfirmationPopup2());
    }
  }

  private bool HaveMaxOver()
  {
    switch (this.info.CommonRewardType)
    {
      case MasterDataTable.CommonRewardType.unit:
        if (MasterData.UnitUnit.ContainsKey(this.info.RewardId) && MasterData.UnitUnit[this.info.RewardId].IsMaterialUnit || (SMManager.Get<PlayerUnit[]>().Length + this.selectedCount > this.player.max_units ? 1 : 0) == 0)
          return false;
        this.StartCoroutine(PopupUtility._999_5_1());
        return true;
      case MasterDataTable.CommonRewardType.gear:
        if (MasterData.GearGear.ContainsKey(this.info.RewardId) && MasterData.GearGear[this.info.RewardId].isMaterial() || (((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && !x.isReisou())).Count<PlayerItem>() + this.selectedCount > this.player.max_items ? 1 : 0) == 0)
          return false;
        this.StartCoroutine(PopupUtility._999_6_1(true));
        return true;
      case MasterDataTable.CommonRewardType.quest_key:
        PlayerQuestKey playerQuestKey = ((IEnumerable<PlayerQuestKey>) SMManager.Get<PlayerQuestKey[]>()).Where<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x => x.quest_key_id == this.info.RewardId)).FirstOrDefault<PlayerQuestKey>();
        int quantity = playerQuestKey == null ? 0 : playerQuestKey.quantity;
        if (playerQuestKey.max_quantity < quantity + this.selectedCount)
        {
          this.StartCoroutine(PopupUtility._999_15(MasterData.QuestkeyQuestkey[playerQuestKey.quest_key_id].name));
          return true;
        }
        break;
      case MasterDataTable.CommonRewardType.season_ticket:
        PlayerSeasonTicket playerSeasonTicket = ((IEnumerable<PlayerSeasonTicket>) SMManager.Get<PlayerSeasonTicket[]>()).FirstOrDefault<PlayerSeasonTicket>((Func<PlayerSeasonTicket, bool>) (x => x.season_ticket_id == this.info.RewardId));
        if (playerSeasonTicket != null && playerSeasonTicket.max_quantity < playerSeasonTicket.quantity + this.selectedCount)
        {
          this.StartCoroutine(PopupUtility._999_15(MasterData.SeasonTicketSeasonTicket[playerSeasonTicket.season_ticket_id].name));
          return true;
        }
        break;
      case MasterDataTable.CommonRewardType.guild_facility:
        PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x.master.ID == this.info.RewardId));
        if (playerGuildFacility != null && playerGuildFacility.hasnum + this.selectedCount > this.info.playerShopArticle.article.ShopContents[0].upper_limit_count)
        {
          this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().POPUP_GUILD_SHOP_LIMIT_OVER_TITLE, Consts.GetInstance().POPUP_GUILD_SHOP_LIMIT_OVER_DESC));
          return true;
        }
        break;
      case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
        PlayerUnitTypeTicket playerUnitTypeTicket = ((IEnumerable<PlayerUnitTypeTicket>) SMManager.Get<PlayerUnitTypeTicket[]>()).FirstOrDefault<PlayerUnitTypeTicket>((Func<PlayerUnitTypeTicket, bool>) (x => x.ticket_id == this.info.RewardId));
        if (playerUnitTypeTicket != null && this.info.playerShopArticle.article.ShopContents[0].upper_limit_check && playerUnitTypeTicket.quantity + this.selectedCount > this.info.playerShopArticle.article.ShopContents[0].upper_limit_count)
        {
          this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().POPUP_GUILD_SHOP_LIMIT_OVER_TITLE, Consts.GetInstance().POPUP_GUILD_SHOP_LIMIT_OVER_DESC));
          return true;
        }
        break;
      case MasterDataTable.CommonRewardType.challenge_point:
        NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
        if (instance.challenge_point + this.selectedCount > instance.challenge_point_max)
        {
          this.StartCoroutine(PopupUtility._999_15(this.info.playerShopArticle.article.name));
          return true;
        }
        break;
    }
    return false;
  }

  private IEnumerator OpenNotEnoughPopup()
  {
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (this.info.playerShopArticle.payment_type_id == 1)
      yield return (object) PopupUtility._999_3_1(Consts.GetInstance().SHOP_99931_TXT_DESCRIPTION);
    else if (this.info.playerShopArticle.payment_type_id == 9)
    {
      yield return (object) PopupUtility._999_3_1(Consts.GetInstance().SHOP_99931_TXT_DESCRIPTION_COMPENSATION, paymentType: Gacha99931Menu.PaymentType.Compensation);
    }
    else
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_999_7_1__anim_popup01.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefabF.Result;
      yield return (object) PopupUtility._999_7_1(this.info.playerShopArticle.article);
      prefabF = (Future<GameObject>) null;
    }
  }

  private IEnumerator ShowConfirmationPopup2()
  {
    Future<GameObject> prefab00771F = Res.Prefabs.popup.popup_007_7_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab00771F.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    yield return (object) Singleton<PopupManager>.GetInstance().open(prefab00771F.Result).GetComponent<Shop00771Menu>().Init(this.info, this.selectedCount, this.shopTime);
  }

  public override void onBackButton() => this.IbtnBack();

  private void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
