// Decompiled with JetBrains decompiler
// Type: PopupCoinExchangeConfirmation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupCoinExchangeConfirmation : BackButtonMenuBase
{
  private const string numIntegrationFormat = "{0}/{1}";
  [Header("Icon")]
  [SerializeField]
  private CreateIconObject dynIcon;
  [SerializeField]
  private UI2DSprite slcIconConsume;
  [SerializeField]
  private UI2DSprite slcIconPossession;
  [Header("Label")]
  [SerializeField]
  private UILabel txtDescription;
  [SerializeField]
  private UILabel txtConsumptionCoin;
  [SerializeField]
  private UILabel txtPossessionCoin;
  [SerializeField]
  private UILabel txtNumIntegration;
  [Header("Slider")]
  [SerializeField]
  private UILabel txtSelectMin;
  [SerializeField]
  private UILabel txtSelectMax;
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UIButton[] sliderButtons;
  private const int maxCountLimit = 100;
  private int maxCount;
  private int selectedCount = 1;
  private int sliderCount = 1;
  private int price;
  [Header("Button")]
  public SpreadColorButton btnYes;
  private PlayerShopArticle playerShopArticle;
  private ShopContent content;
  private Func<IEnumerator> _onPurchased;

  public IEnumerator Init(
    PlayerShopArticle playerShopArticle,
    long holding,
    Func<IEnumerator> onPurchased)
  {
    this.playerShopArticle = playerShopArticle;
    this.content = playerShopArticle.article.ShopContents[0];
    this._onPurchased = onPurchased;
    CommonTicket commonTicket = MasterData.CommonTicket[playerShopArticle.article.pay_id.Value];
    this.txtDescription.SetTextLocalize(Consts.GetInstance().SHOP_POPUP_COIN_EXCHANGE_CONFIRM_DESCRIPTION.F((object) commonTicket.name, (object) this.content.article.name));
    this.maxCount = playerShopArticle.limit.Value;
    this.price = this.content.article.price;
    while (holding < (long) (this.maxCount * this.price))
      --this.maxCount;
    if (this.maxCount <= 1)
    {
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount + 1;
      ((Behaviour) this.slider).enabled = false;
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
      this.txtSelectMin.SetTextLocalize(0);
      this.txtSelectMax.SetTextLocalize(1);
      this.selectedCount = this.sliderCount = 1;
      foreach (UIButtonColor sliderButton in this.sliderButtons)
        sliderButton.isEnabled = false;
    }
    else
    {
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = true;
      ((Behaviour) this.slider).enabled = true;
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount;
      this.txtSelectMin.SetTextLocalize(1);
      this.txtSelectMax.SetTextLocalize(this.maxCount);
      this.selectedCount = 1;
      this.sliderCount = this.selectedCount - 1;
    }
    this.UpdateInfo();
    this.txtPossessionCoin.SetTextLocalize(holding * (long) this.selectedCount);
    Future<Sprite> future = commonTicket.LoadIconSSpriteF();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) this.dynIcon, (Object) null))
    {
      this.slcIconConsume.sprite2D = future.Result;
      this.slcIconPossession.sprite2D = future.Result;
      future = (Future<Sprite>) null;
      yield return (object) this.dynIcon.CreateThumbnail(this.content.entity_type, this.content.entity_id, this.content.quantity, isButton: false);
    }
  }

  public void OnValueChange()
  {
    this.sliderCount = Mathf.RoundToInt(((UIProgressBar) this.slider).value * ((float) this.maxCount - 1f));
    this.UpdateInfo();
  }

  private void UpdateInfo()
  {
    this.selectedCount = this.maxCount != 1 ? this.sliderCount + 1 : 1;
    this.txtNumIntegration.SetTextLocalize("{0}/{1}".F((object) this.selectedCount, (object) this.maxCount));
    ((UIProgressBar) this.slider).value = (float) this.sliderCount / ((float) this.maxCount - 1f);
    this.txtConsumptionCoin.SetTextLocalize(this.price * this.selectedCount);
  }

  public void IbtnDecrease()
  {
    --this.sliderCount;
    if (this.sliderCount <= 0)
      this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnIncrease()
  {
    ++this.sliderCount;
    if (this.sliderCount >= this.maxCount - 1)
      this.sliderCount = this.maxCount - 1;
    this.UpdateInfo();
  }

  public void IbtnSetMin()
  {
    this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnSetMax()
  {
    this.sliderCount = this.maxCount - 1;
    this.UpdateInfo();
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShopBuy());
  }

  private IEnumerator ShopBuy()
  {
    PopupCoinExchangeConfirmation exchangeConfirmation = this;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    Future<WebAPI.Response.ShopBuy> paramF = WebAPI.ShopBuy(exchangeConfirmation.playerShopArticle.article.ID, exchangeConfirmation.selectedCount, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      WebAPI.DefaultUserErrorCallback(e);
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
      Future<GameObject> popupF = new ResourceObject("Prefabs/Popup_Common/popup_WithThum_OK_Base").Load<GameObject>();
      e1 = popupF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject result = popupF.Result;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      // ISSUE: reference to a compiler-generated method
      e1 = Singleton<PopupManager>.GetInstance().open(result).GetComponent<PopupWithThumOk>().Initialize(Consts.GetInstance().SHOP_POPUP_COIN_EXCHANGE_RESULT_TITLE, Consts.GetInstance().SHOP_POPUP_COIN_EXCHANGE_RESULT_MSG.F((object) exchangeConfirmation.content.article.name, (object) (exchangeConfirmation.content.quantity * exchangeConfirmation.selectedCount)), exchangeConfirmation.content.entity_type, exchangeConfirmation.content.entity_id, callback: new Action(exchangeConfirmation.\u003CShopBuy\u003Eb__29_1));
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  public void cbEndResultPopup()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.StartCoroutine(this.cbEndResultPopupAsync());
  }

  public IEnumerator cbEndResultPopupAsync()
  {
    if (this._onPurchased != null)
    {
      IEnumerator e = this._onPurchased();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
