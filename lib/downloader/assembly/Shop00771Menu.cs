// Decompiled with JetBrains decompiler
// Type: Shop00771Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Shop00771Menu : BackButtonMenuBase
{
  [SerializeField]
  private Transform ThumParent;
  [SerializeField]
  private UI2DSprite PackThum;
  [SerializeField]
  private GameObject PackThumMount;
  [SerializeField]
  private UIGrid MarkGrid;
  [SerializeField]
  private UI2DSprite Decoration;
  [SerializeField]
  private GameObject PackMark;
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription03;
  [SerializeField]
  private UILabel TxtDescription04;
  [SerializeField]
  private UILabel TxtDescription05;
  [SerializeField]
  private GameObject Paid;
  [SerializeField]
  private UILabel PaidBeforeAfterPayValue;
  [SerializeField]
  private UILabel CoinBeforeAfterPayValue;
  [SerializeField]
  private GameObject Normal;
  [SerializeField]
  private UI2DSprite NormalPayTypeIcon;
  [SerializeField]
  private UILabel NormalBeforeAfterPayValue;
  [SerializeField]
  private UIButton ibtnY;
  [SerializeField]
  private UIButton ibtnN;
  private GameObject linkTarget;
  private ShopItemIconInfo _info;
  public int _itemId;
  public string _itemName;
  public int _purchasedQuantity;
  public int _playerQuantity;
  public int _price;
  public long _holding;
  private Action<long> _onPurchasedHolding;
  private Func<IEnumerator> _onPurchased;
  public long holding_next;
  public Sprite[] backSprite;
  [SerializeField]
  private UILabel shopLimitTime;
  [SerializeField]
  private GameObject commercialObj;
  [SerializeField]
  private UISprite popupBaseSprite;

  public PlayerShopArticle _playerShopArticle { get; set; }

  public List<Shop0074Scroll> _scrolls { get; set; }

  public Player _player { get; set; }

  public IEnumerator Init(ShopItemIconInfo info, int selectCount, string shopTime)
  {
    if (info.playerShopArticle.icon_resource != "")
    {
      this.PackThumMount.SetActive(true);
      ((Component) this.PackThum).gameObject.SetActive(true);
      yield return (object) ShopCommon.CreateShopPack(this.PackThum, info.playerShopArticle.icon_resource);
    }
    else
    {
      this.PackThumMount.SetActive(false);
      ((Component) this.PackThum).gameObject.SetActive(false);
      yield return (object) ShopCommon.CreateThum(this.ThumParent, info.CommonRewardType, info.RewardId);
    }
    if (info.playerShopArticle.decoration_resource != "")
    {
      ((Component) this.Decoration).gameObject.SetActive(true);
      yield return (object) ShopCommon.CreateShopIcon(this.Decoration, info.playerShopArticle.decoration_resource);
    }
    else
      ((Component) this.Decoration).gameObject.SetActive(false);
    if (info.IsPack)
    {
      this.PackMark.SetActive(true);
      selectCount = 1;
    }
    if (!string.IsNullOrEmpty(shopTime))
    {
      this.commercialObj.SetActive(true);
      this.shopLimitTime.SetTextLocalize(shopTime);
    }
    this.MarkGrid.Reposition();
    this._info = info;
    this._itemId = info.playerShopArticle._article;
    this._itemName = info.ItemName;
    this.TxtDescription01.text = info.ItemName;
    this._price = info.playerShopArticle.payment_amount;
    this.SetTxtDescriptionPayPrice(info.playerShopArticle);
    this._purchasedQuantity = selectCount;
    this.SetTxtDescriptionMessage(info.playerShopArticle);
    this.SetTxtDescriptionSumPrice(info.playerShopArticle);
    this.SetTxtDescriptionBeforAfter(info.PayType);
  }

  public void InitData(
    PlayerShopArticle playerShopArticle,
    int itemId,
    string itemName,
    int purchasedQuantity,
    int playerQuantity,
    int price,
    long holding,
    List<Shop0074Scroll> scrolls,
    PlayerShopArticle[] articles,
    Func<IEnumerator> onPurchased,
    Action<long> onPurcheasedHolding)
  {
    if (this._info == null)
    {
      this.PackThumMount.SetActive(false);
      ((Component) this.PackThum).gameObject.SetActive(false);
    }
    else if (this._info.playerShopArticle.icon_resource != "")
    {
      this.PackThumMount.SetActive(true);
      ((Component) this.PackThum).gameObject.SetActive(true);
    }
    else
    {
      this.PackThumMount.SetActive(false);
      ((Component) this.PackThum).gameObject.SetActive(false);
    }
    this._playerShopArticle = playerShopArticle;
    this._itemId = itemId;
    this._itemName = itemName;
    this._purchasedQuantity = purchasedQuantity;
    this._playerQuantity = playerQuantity;
    this._price = price;
    this._holding = holding;
    this._scrolls = scrolls;
    this._onPurchasedHolding = onPurcheasedHolding;
    this._onPurchased = onPurchased;
    this.CalcBalance();
    this.TxtDescription01.SetTextLocalize(this._itemName);
    this.SetTxtDescriptionBeforAfter(this._playerShopArticle.article.pay_type);
    this.SetTxtDescriptionPayPrice(this._playerShopArticle);
    this.SetTxtDescriptionMessage(this._playerShopArticle);
    this.SetTxtDescriptionSumPrice(this._playerShopArticle);
  }

  public void InitObject(GameObject obj)
  {
    this.linkTarget = obj;
    GameObject gameObject = obj.Clone(this.ThumParent);
    UnitIcon component1 = gameObject.GetComponent<UnitIcon>();
    ItemIcon component2 = gameObject.GetComponent<ItemIcon>();
    if (Object.op_Implicit((Object) component1))
    {
      component1.SetIconBoxCollider(false);
    }
    else
    {
      if (!Object.op_Implicit((Object) component2))
        return;
      component2.isButtonActive = false;
    }
  }

  private void SetTxtDescriptionPayPrice(PlayerShopArticle psa)
  {
    if (psa.article.pay_type == CommonPayType.money)
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MONEY, (IDictionary) new Hashtable()
      {
        {
          (object) "money",
          (object) this._price
        }
      }));
    else if (psa.article.pay_type == CommonPayType.medal)
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this._price
        }
      }));
    else if (psa.article.pay_type == CommonPayType.battle_medal)
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this._price
        }
      }));
    else if (psa.article.pay_type == CommonPayType.tower_medal)
    {
      Hashtable args = new Hashtable()
      {
        {
          (object) "medal",
          (object) this._price
        },
        {
          (object) "unit",
          (object) Consts.GetInstance().SHOP_TOWERMEDAL_UNIT
        }
      };
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_xMEDALxUNIT, (IDictionary) args));
    }
    else if (psa.article.pay_type == CommonPayType.guild_medal)
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this._price
        }
      }));
    else if (psa.article.pay_type == CommonPayType.raid_medal)
    {
      this.TxtDescription03.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION03_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) this._price
        }
      }));
    }
    else
    {
      if (psa.article.pay_type != CommonPayType.coin && psa.article.pay_type != CommonPayType.paid_coin)
        return;
      this.TxtDescription03.text = string.Format("価格：{0}個", (object) this._price);
    }
  }

  private void SetTxtDescriptionMessage(PlayerShopArticle psa)
  {
    string localizeNumberText = this._purchasedQuantity.ToLocalizeNumberText();
    this.TxtDescription04.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION04, (IDictionary) new Hashtable()
    {
      {
        (object) "quantity",
        (object) localizeNumberText
      }
    }));
  }

  private void SetTxtDescriptionSumPrice(PlayerShopArticle psa)
  {
    if (psa.article.pay_type == CommonPayType.money)
      this.TxtDescription05.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MONEY, (IDictionary) new Hashtable()
      {
        {
          (object) "money",
          (object) (this._purchasedQuantity * this._price).ToLocalizeNumberText()
        }
      }));
    else if (psa.article.pay_type == CommonPayType.medal)
      this.TxtDescription05.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) (this._purchasedQuantity * this._price).ToLocalizeNumberText()
        }
      }));
    else if (psa.article.pay_type == CommonPayType.battle_medal)
      this.TxtDescription05.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) (this._purchasedQuantity * this._price).ToLocalizeNumberText()
        }
      }));
    else if (psa.article.pay_type == CommonPayType.tower_medal)
    {
      Hashtable args = new Hashtable()
      {
        {
          (object) "medal",
          (object) (this._purchasedQuantity * this._price).ToLocalizeNumberText()
        },
        {
          (object) "unit",
          (object) Consts.GetInstance().SHOP_TOWERMEDAL_UNIT
        }
      };
      this.TxtDescription05.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_xMEDALxUNIT, (IDictionary) args));
    }
    else if (psa.article.pay_type == CommonPayType.guild_medal)
      this.TxtDescription05.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) (this._purchasedQuantity * this._price).ToLocalizeNumberText()
        }
      }));
    else if (psa.article.pay_type == CommonPayType.raid_medal)
    {
      this.TxtDescription05.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_007X_TXT_DESCRIPTION05_MEDAL, (IDictionary) new Hashtable()
      {
        {
          (object) "medal",
          (object) (this._purchasedQuantity * this._price).ToLocalizeNumberText()
        }
      }));
    }
    else
    {
      if (psa.article.pay_type != CommonPayType.coin && psa.article.pay_type != CommonPayType.paid_coin)
        return;
      this.TxtDescription05.text = string.Format("合計価格：[ffff00]{0}[-]個", (object) (this._purchasedQuantity * this._price));
    }
  }

  private void SetTxtDescriptionBeforAfter(CommonPayType paytype)
  {
    switch (paytype)
    {
      case CommonPayType.coin:
        int num1 = this._purchasedQuantity * this._price;
        int paidCoin = SMManager.Get<Player>().paid_coin;
        int freeCommonCoin = SMManager.Get<Player>().free_common_coin;
        int num2 = paidCoin;
        int num3 = freeCommonCoin;
        int num4;
        int num5;
        if (freeCommonCoin - num1 >= 0)
        {
          num4 = paidCoin;
          num5 = freeCommonCoin - num1;
        }
        else
        {
          int num6 = -(freeCommonCoin - num1);
          num4 = paidCoin - num6;
          num5 = 0;
        }
        this.PaidBeforeAfterPayValue.text = string.Format("所持個数：{0}個→[ffff00]{1}[-]個", (object) num2, (object) num4);
        this.CoinBeforeAfterPayValue.text = string.Format("所持個数：{0}個→[ffff00]{1}[-]個", (object) num3, (object) num5);
        this.Paid.SetActive(true);
        this.Normal.SetActive(false);
        break;
      case CommonPayType.money:
        this._holding = ShopCommon.GetHaveCount(paytype);
        this.CalcBalance();
        string localizeNumberText1 = this._holding.ToLocalizeNumberText();
        string localizeNumberText2 = this.holding_next.ToLocalizeNumberText();
        this.NormalBeforeAfterPayValue.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION03_MONEY, (IDictionary) new Hashtable()
        {
          {
            (object) "money",
            (object) localizeNumberText1
          },
          {
            (object) "moneyNext",
            (object) localizeNumberText2
          }
        }));
        this.Paid.SetActive(false);
        this.Normal.SetActive(true);
        this.NormalPayTypeIcon.sprite2D = ShopCommon.PayTypeZeny;
        break;
      case CommonPayType.medal:
        this._holding = ShopCommon.GetHaveCount(paytype);
        this.CalcBalance();
        string localizeNumberText3 = this._holding.ToLocalizeNumberText();
        string localizeNumberText4 = this.holding_next.ToLocalizeNumberText();
        this.NormalBeforeAfterPayValue.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION02_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) localizeNumberText3
          },
          {
            (object) "medalNext",
            (object) localizeNumberText4
          }
        }));
        this.Paid.SetActive(false);
        this.Normal.SetActive(true);
        this.NormalPayTypeIcon.sprite2D = ShopCommon.PayTypeMedal;
        break;
      case CommonPayType.battle_medal:
        this._holding = ShopCommon.GetHaveCount(paytype);
        this.CalcBalance();
        string localizeNumberText5 = this._holding.ToLocalizeNumberText();
        string localizeNumberText6 = this.holding_next.ToLocalizeNumberText();
        this.NormalBeforeAfterPayValue.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION02_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) localizeNumberText5
          },
          {
            (object) "medalNext",
            (object) localizeNumberText6
          }
        }));
        this.Paid.SetActive(false);
        this.Normal.SetActive(true);
        this.NormalPayTypeIcon.sprite2D = ShopCommon.PayTypeBattleMedal;
        break;
      case CommonPayType.tower_medal:
        this._holding = ShopCommon.GetHaveCount(paytype);
        this.CalcBalance();
        Hashtable args = new Hashtable()
        {
          {
            (object) "medal",
            (object) this._holding.ToLocalizeNumberText()
          },
          {
            (object) "medalNext",
            (object) this.holding_next.ToLocalizeNumberText()
          },
          {
            (object) "unit",
            (object) Consts.GetInstance().SHOP_TOWERMEDAL_UNIT
          }
        };
        this.NormalBeforeAfterPayValue.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION02_xMEDALxUNIT, (IDictionary) args));
        this.Paid.SetActive(false);
        this.Normal.SetActive(true);
        this.NormalPayTypeIcon.sprite2D = ShopCommon.PayTypeTowerMedal;
        break;
      case CommonPayType.paid_coin:
        this._holding = (long) SMManager.Get<Player>().paid_coin;
        this.holding_next = this._holding - (long) (this._purchasedQuantity * this._price);
        this.PaidBeforeAfterPayValue.text = string.Format("所持個数：{0}個→[ffff00]{1}[-]個", (object) this._holding, (object) this.holding_next);
        this._holding = (long) SMManager.Get<Player>().free_common_coin;
        this.holding_next = this._holding;
        this.CoinBeforeAfterPayValue.text = string.Format("所持個数：{0}個→[ffff00]{1}[-]個", (object) this._holding, (object) this.holding_next);
        this.Paid.SetActive(true);
        this.Normal.SetActive(false);
        break;
      case CommonPayType.guild_medal:
        this._holding = ShopCommon.GetHaveCount(paytype);
        this.CalcBalance();
        string localizeNumberText7 = this._holding.ToLocalizeNumberText();
        string localizeNumberText8 = this.holding_next.ToLocalizeNumberText();
        this.NormalBeforeAfterPayValue.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION02_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) localizeNumberText7
          },
          {
            (object) "medalNext",
            (object) localizeNumberText8
          }
        }));
        this.Paid.SetActive(false);
        this.Normal.SetActive(true);
        this.NormalPayTypeIcon.sprite2D = ShopCommon.PayTypeGuildMedal;
        break;
      case CommonPayType.raid_medal:
        this._holding = ShopCommon.GetHaveCount(paytype);
        this.CalcBalance();
        string localizeNumberText9 = this._holding.ToLocalizeNumberText();
        string localizeNumberText10 = this.holding_next.ToLocalizeNumberText();
        this.NormalBeforeAfterPayValue.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_00771_TXT_DESCRIPTION02_MEDAL, (IDictionary) new Hashtable()
        {
          {
            (object) "medal",
            (object) localizeNumberText9
          },
          {
            (object) "medalNext",
            (object) localizeNumberText10
          }
        }));
        this.Paid.SetActive(false);
        this.Normal.SetActive(true);
        this.NormalPayTypeIcon.sprite2D = ShopCommon.PayTypeRaidJuel;
        break;
    }
    if (this.Normal.activeSelf)
    {
      this.commercialObj.SetActive(false);
      ((UIWidget) this.popupBaseSprite).SetDimensions(((UIWidget) this.popupBaseSprite).width, 620);
    }
    else
    {
      if (!this.Paid.activeSelf)
        return;
      this.commercialObj.SetActive(true);
    }
  }

  private void CalcBalance()
  {
    this.holding_next = this._holding - (long) (this._purchasedQuantity * this._price);
    if (this.holding_next > 0L)
      return;
    this.holding_next = 0L;
  }

  public void IbtnPopupYes()
  {
    this.PushAfter();
    Singleton<PopupManager>.GetInstance().dismiss();
    this.StartCoroutine(this.ShopBuy());
  }

  public void PushAfter()
  {
    ((Behaviour) this.ibtnY).enabled = false;
    ((Behaviour) this.ibtnN).enabled = false;
  }

  private IEnumerator ShopBuy()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    int beforeHavingNum = this._info != null ? (int) CommonRewardType.GetHaveCount((MasterDataTable.CommonRewardType) this._info.playerShopArticle.contents[0].reward_type_id, this._info.playerShopArticle.contents[0].reward_id) : this._playerQuantity;
    WebAPI.Response.UserError shopBuyError = (WebAPI.Response.UserError) null;
    Future<WebAPI.Response.ShopBuy> shopInfoF = WebAPI.ShopBuy(this._itemId, this._purchasedQuantity, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (!(error.Code == "SHP001"))
        return;
      shopBuyError = error;
    })).Then<WebAPI.Response.ShopBuy>((Func<WebAPI.Response.ShopBuy, WebAPI.Response.ShopBuy>) (result =>
    {
      Singleton<NGGameDataManager>.GetInstance().Parse(result);
      return result;
    }));
    IEnumerator e = shopInfoF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (shopBuyError != null)
      ModalWindow.Show(shopBuyError.Code, shopBuyError.Reason, (Action) (() => MypageScene.ChangeScene(MypageRootMenu.Mode.STORY)));
    else if (shopInfoF.Result != null)
    {
      SMManager.UpdateList<PlayerBattleMedal>(shopInfoF.Result.after.battle_medals);
      Future<GameObject> prefabF;
      if (this._info != null && this._info.IsPack)
      {
        prefabF = new ResourceObject("Prefabs/popup/popup_007_8__anim_popup_pack").Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        prefabF = Res.Prefabs.popup.popup_007_8__anim_popup01.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      yield return (object) null;
      GameObject popup = Singleton<PopupManager>.GetInstance().open(prefabF.Result);
      popup.SetActive(false);
      Shop0078Menu popupScript = popup.GetComponent<Shop0078Menu>();
      if (this._info == null)
      {
        popupScript.InitObj(this.linkTarget);
        popupScript.InitDataSet(this._itemName, this._purchasedQuantity, this._playerQuantity);
        if (this._onPurchasedHolding != null)
          this._onPurchasedHolding(this.holding_next);
        if (this._onPurchased != null)
        {
          e = this._onPurchased();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else
      {
        yield return (object) popupScript.Init(this._info);
        if (this._info.IsPack)
          popupScript.InitDataSet(this._itemName, this._purchasedQuantity, -1);
        else
          popupScript.InitDataSet(this._itemName, this._purchasedQuantity, beforeHavingNum);
        if (this._info.LimitCount.HasValue)
        {
          ShopItemIconInfo info = this._info;
          int? limitCount = info.LimitCount;
          int purchasedQuantity = this._purchasedQuantity;
          info.LimitCount = limitCount.HasValue ? new int?(limitCount.GetValueOrDefault() - purchasedQuantity) : new int?();
        }
        ShopItemListMenu shopItemListMenu = Object.FindObjectsOfType<ShopItemListMenu>()[0];
        if (shopItemListMenu.ShopId == 4000)
          shopItemListMenu.UpdateHaveBattleMedal();
        yield return (object) shopItemListMenu.UpdateIconsHavingNum();
      }
      popup.SetActive(true);
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  private void IbtnNo()
  {
    this.PushAfter();
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public override void onBackButton() => this.IbtnNo();
}
