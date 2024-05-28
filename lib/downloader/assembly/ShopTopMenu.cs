// Decompiled with JetBrains decompiler
// Type: ShopTopMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ShopTopMenu : BackButtonMenuBase
{
  [SerializeField]
  private Animator shopAnimator;
  [SerializeField]
  private GameObject mainPanel;
  [Header("Middle")]
  [SerializeField]
  private GameObject kisekiCampaign;
  [SerializeField]
  private GameObject begginerCampaign;
  [SerializeField]
  private GameObject bikkuriIcon;
  [SerializeField]
  private GameObject TicketExchangeBatch;
  [SerializeField]
  private UILabel TicketExchangeNum;
  [Header("Bottom ショップ種類切り替えタブ")]
  [SerializeField]
  private UIButton LimitedTabButton;
  [SerializeField]
  private UISprite LimitedTabButtonSprite;
  [SerializeField]
  private UIButton NormalTabButton;
  [SerializeField]
  private UISprite NormalTabButtonSprite;
  [Header("Bottom 限定ショップ")]
  [SerializeField]
  private UIWidget LimitedShopContainer;
  [SerializeField]
  private UIScrollView LimitedShopScroll;
  [SerializeField]
  private UICenterOnChild LimitedCenterOnChild;
  [SerializeField]
  private UIGrid LimitedShopGrid;
  private List<GameObject> LimitedShopBanners = new List<GameObject>();
  [SerializeField]
  private GameObject LimitedLeftArrow;
  [SerializeField]
  private UIButton LimitedLeftArrowButton;
  [SerializeField]
  private GameObject LimitedRightArrow;
  [SerializeField]
  private UIButton LimitedRightArrowButton;
  [SerializeField]
  private GameObject LimitedShopDot;
  private List<ShopBannerDotContainer> LimitedDots = new List<ShopBannerDotContainer>();
  [Header("Bottom 通常ショップ")]
  [SerializeField]
  private GameObject NormalShopContainer;
  [SerializeField]
  private UIScrollView NormalShopScroll;
  [SerializeField]
  private UICenterOnChild NormalCenterOnChild;
  [SerializeField]
  private UIGrid NormalShopGrid;
  private List<GameObject> NormalShopBanners = new List<GameObject>();
  [SerializeField]
  private GameObject NormalLeftArrow;
  [SerializeField]
  private UIButton NormalLeftArrowButton;
  [SerializeField]
  private GameObject NormalRightArrow;
  [SerializeField]
  private UIButton NormalRightArrowButton;
  [SerializeField]
  private GameObject NormalShopDot;
  private List<ShopBannerDotContainer> NormalDots = new List<ShopBannerDotContainer>();
  private WebAPI.Response.ShopStatus shopStatus;
  private bool isInit;
  private GameObject currentLimitedBanner;
  private GameObject currentNormalBanner;
  private GameObject LimitedShopBannerPrefab;
  private GameObject CommingSoonBannerPrefab;
  private GameObject BannerDotPrefab;
  [Header("アニメーション関係")]
  [SerializeField]
  private ShopAnimation shopAnimation;

  public IEnumerator Init(WebAPI.Response.ShopStatus shopStatus)
  {
    ShopTopMenu menu = this;
    menu.mainPanel.SetActive(true);
    menu.shopStatus = shopStatus;
    SelectTicket[] selectTicketArray = SMManager.Get<SelectTicket[]>();
    PlayerSelectTicketSummary[] source = SMManager.Get<PlayerSelectTicketSummary[]>();
    int num = 0;
    foreach (SelectTicket selectTicket in selectTicketArray)
    {
      SelectTicket unitTicket = selectTicket;
      PlayerSelectTicketSummary selectTicketSummary = ((IEnumerable<PlayerSelectTicketSummary>) source).FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == unitTicket.id));
      if (selectTicketSummary != null && selectTicketSummary.quantity >= unitTicket.cost)
        ++num;
    }
    if (num > 0)
    {
      menu.TicketExchangeBatch.SetActive(true);
      menu.TicketExchangeNum.text = string.Format("[b]{0}[-]", (object) num);
    }
    else
      menu.TicketExchangeBatch.SetActive(false);
    IEnumerator e = Singleton<CommonRoot>.GetInstance().UpdateFooterLimitedShopButton();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (menu.IsOpenLimitedShop())
    {
      foreach (Component component in ((Component) menu.LimitedShopGrid).transform)
        component.GetComponent<ShopBanner>().UpdateNewIcon();
    }
    Future<GameObject> prefabF;
    if (menu.isInit)
    {
      if (menu.LimitedShopDot.activeSelf)
      {
        prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/BackGround/ShopLimitedBackground");
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        prefabF.Result.GetComponent<ShopBackgroundAnimation>();
        Singleton<CommonRoot>.GetInstance().setBackground(prefabF.Result);
        prefabF = (Future<GameObject>) null;
      }
      else
      {
        prefabF = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        prefabF.Result.GetComponent<ShopBackgroundAnimation>();
        Singleton<CommonRoot>.GetInstance().setBackground(prefabF.Result);
        prefabF = (Future<GameObject>) null;
      }
      ShopBackgroundAnimation.CurrentShopBackground = Singleton<CommonRoot>.GetInstance().getCommonBackground().Current;
    }
    else if (menu.LimitedShopDot.activeSelf)
    {
      prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/BackGround/ShopLimitedBackground");
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      prefabF.Result.GetComponent<ShopBackgroundAnimation>().Change();
      prefabF = (Future<GameObject>) null;
    }
    else
    {
      prefabF = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      prefabF.Result.GetComponent<ShopBackgroundAnimation>().Change();
      prefabF = (Future<GameObject>) null;
    }
    if (!menu.isInit)
    {
      yield return (object) menu.LoadResources();
      menu.LimitedShopBanners.Clear();
      menu.NormalShopBanners.Clear();
      ((Component) menu.LimitedShopGrid).transform.Clear();
      menu.LimitedShopDot.transform.Clear();
      menu.NormalShopDot.transform.Clear();
      menu.UpdateCampaign();
      if (menu.IsOpenLimitedShop())
      {
        foreach (LimitedShopBanner limitedShopBanner1 in (IEnumerable<LimitedShopBanner>) ((IEnumerable<LimitedShopBanner>) shopStatus.shop_banners).OrderByDescending<LimitedShopBanner, int>((Func<LimitedShopBanner, int>) (x => x.id)))
        {
          LimitedShopBanner banner = limitedShopBanner1;
          GameObject limitedShopBanner = menu.LimitedShopBannerPrefab.Clone(((Component) menu.LimitedShopGrid).transform);
          UI2DSprite component1 = limitedShopBanner.GetComponent<UI2DSprite>();
          e = Singleton<NGGameDataManager>.GetInstance().GetWebImage(banner.banner_url, component1);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          menu.LimitedShopBanners.Add(limitedShopBanner);
          ShopBanner component2 = limitedShopBanner.GetComponent<ShopBanner>();
          component2.Init(menu, banner.id, ((IEnumerable<LimitedShopBanner>) shopStatus.shop_banners).First<LimitedShopBanner>((Func<LimitedShopBanner, bool>) (x => x.id == banner.id)).name);
          limitedShopBanner.GetComponent<UIButton>();
          if (banner.end_at.HasValue)
          {
            ShopCommon.LimitEmphasiePrefab.Clone(component2.limitedParent).GetComponent<BannerLimitEmphasie>().Init(ShopCommon.LoginTime, banner.end_at);
            component2.SetBannerTime(banner.end_at);
          }
          limitedShopBanner = (GameObject) null;
        }
        menu.LimitedShopScroll.ResetPosition();
        menu.LimitedShopGrid.Reposition();
      }
      else
      {
        GameObject gameObject = menu.CommingSoonBannerPrefab.Clone(((Component) menu.LimitedShopGrid).transform);
        menu.LimitedShopBanners.Add(gameObject);
      }
      foreach (Transform transform in ((Component) menu.NormalShopGrid).transform)
      {
        menu.NormalShopBanners.Add(((Component) transform).gameObject);
        ((Component) transform).GetComponent<ShopBanner>().Init(menu, 0, "");
        ((Component) transform).GetComponent<UIButton>();
      }
      menu.LimitedDots.Clear();
      foreach (Transform transform in ((Component) menu.LimitedShopGrid).transform)
      {
        GameObject gameObject = menu.BannerDotPrefab.Clone(menu.LimitedShopDot.transform);
        menu.LimitedDots.Add(gameObject.GetComponent<ShopBannerDotContainer>());
      }
      menu.LimitedDots[0].On();
      UIGrid component3 = menu.LimitedShopDot.GetComponent<UIGrid>();
      component3.repositionNow = true;
      component3.Reposition();
      menu.NormalDots.Clear();
      foreach (Transform transform in ((Component) menu.NormalShopGrid).transform)
      {
        GameObject gameObject = menu.BannerDotPrefab.Clone(menu.NormalShopDot.transform);
        menu.NormalDots.Add(gameObject.GetComponent<ShopBannerDotContainer>());
      }
      menu.NormalDots[0].On();
      UIGrid component4 = menu.NormalShopDot.GetComponent<UIGrid>();
      component4.repositionNow = true;
      component4.Reposition();
      ((UIButtonColor) menu.LimitedTabButton).isEnabled = false;
      ((UIButtonColor) menu.NormalTabButton).isEnabled = true;
      ((UIWidget) menu.LimitedTabButtonSprite).color = ShopCommon.TabDisableTextColor;
      ((UIWidget) menu.NormalTabButtonSprite).color = new Color(0.349019617f, 0.349019617f, 0.349019617f);
      menu.LimitedShopDot.SetActive(true);
      menu.NormalShopDot.SetActive(false);
      // ISSUE: method pointer
      menu.LimitedCenterOnChild.onFinished = new SpringPanel.OnFinished((object) menu, __methodptr(\u003CInit\u003Eb__41_0));
      // ISSUE: method pointer
      menu.NormalCenterOnChild.onFinished = new SpringPanel.OnFinished((object) menu, __methodptr(\u003CInit\u003Eb__41_1));
      menu.LimitedCenterOnChild.CenterOn(menu.LimitedShopBanners[0].transform);
      menu.NormalCenterOnChild.CenterOn(menu.NormalShopBanners[0].transform);
    }
  }

  public IEnumerator InitAnimation()
  {
    ShopTopMenu shopTopMenu = this;
    if (!shopTopMenu.isInit)
    {
      ((Behaviour) shopTopMenu.shopAnimator).enabled = true;
      shopTopMenu.StartCoroutine(shopTopMenu.shopAnimation.Init());
      while ((double) ((UIRect) shopTopMenu.LimitedShopContainer).alpha < 1.0)
        yield return (object) null;
      shopTopMenu.NormalShopContainer.SetActive(true);
      shopTopMenu.isInit = true;
    }
  }

  public void UpdateCampaign()
  {
    if (Singleton<NGGameDataManager>.GetInstance().newbiePacks)
    {
      this.kisekiCampaign.SetActive(false);
      this.begginerCampaign.SetActive(true);
    }
    else
    {
      this.kisekiCampaign.SetActive(true);
      this.begginerCampaign.SetActive(false);
    }
    if (Singleton<NGGameDataManager>.GetInstance().receivableGift)
      this.bikkuriIcon.SetActive(true);
    else
      this.bikkuriIcon.SetActive(false);
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> prefabF = new ResourceObject("Prefabs/shop_Top/ibtn_LimitedShop").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.LimitedShopBannerPrefab = prefabF.Result;
    prefabF = new ResourceObject("Prefabs/shop_Top/comming_soon").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.CommingSoonBannerPrefab = prefabF.Result;
    prefabF = new ResourceObject("Prefabs/shop_Top/ShopBannerDotContainer").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.BannerDotPrefab = prefabF.Result;
  }

  private void BannerCenterOnChildEvent(bool isLimited)
  {
    int num1 = 0;
    int num2 = 0;
    if (isLimited)
    {
      GameObject centeredObject = this.LimitedCenterOnChild.centeredObject;
      foreach (GameObject limitedShopBanner in this.LimitedShopBanners)
      {
        ShopBanner component = limitedShopBanner.GetComponent<ShopBanner>();
        if (Object.op_Equality((Object) limitedShopBanner, (Object) centeredObject))
        {
          num1 = num2;
          if (Object.op_Inequality((Object) component.Button, (Object) null))
            ((UIButtonColor) component.Button).isEnabled = true;
        }
        else
        {
          ((UIButtonColor) component.Button).isEnabled = false;
          ++num2;
        }
      }
      for (int index = 0; index < this.LimitedDots.Count; ++index)
      {
        if (index == num1)
          this.LimitedDots[index].On();
        else
          this.LimitedDots[index].Off();
      }
      if (this.LimitedShopBanners.Count == 1)
      {
        this.LimitedLeftArrow.SetActive(false);
        this.LimitedRightArrow.SetActive(false);
      }
      else if (num1 == 0)
      {
        this.LimitedLeftArrow.SetActive(false);
        this.LimitedRightArrow.SetActive(true);
      }
      else if (num1 == this.LimitedShopBanners.Count - 1)
      {
        this.LimitedLeftArrow.SetActive(true);
        this.LimitedRightArrow.SetActive(false);
      }
      else
      {
        this.LimitedLeftArrow.SetActive(true);
        this.LimitedRightArrow.SetActive(true);
      }
      if (Object.op_Inequality((Object) this.currentLimitedBanner, (Object) null) && Object.op_Inequality((Object) this.currentLimitedBanner, (Object) centeredObject))
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1005");
      this.currentLimitedBanner = centeredObject;
    }
    else
    {
      GameObject centeredObject = this.NormalCenterOnChild.centeredObject;
      foreach (GameObject normalShopBanner in this.NormalShopBanners)
      {
        ShopBanner component = normalShopBanner.GetComponent<ShopBanner>();
        if (Object.op_Equality((Object) normalShopBanner, (Object) centeredObject))
        {
          num1 = num2;
          ((UIButtonColor) component.Button).isEnabled = true;
        }
        else
        {
          ((UIButtonColor) component.Button).isEnabled = false;
          ++num2;
        }
      }
      for (int index = 0; index < this.NormalDots.Count; ++index)
      {
        if (index == num1)
          this.NormalDots[index].On();
        else
          this.NormalDots[index].Off();
      }
      if (this.NormalShopBanners.Count == 1)
      {
        this.NormalLeftArrow.SetActive(false);
        this.NormalRightArrow.SetActive(false);
      }
      else if (num1 == 0)
      {
        this.NormalLeftArrow.SetActive(false);
        this.NormalRightArrow.SetActive(true);
      }
      else if (num1 == this.NormalShopBanners.Count - 1)
      {
        this.NormalLeftArrow.SetActive(true);
        this.NormalRightArrow.SetActive(false);
      }
      else
      {
        this.NormalLeftArrow.SetActive(true);
        this.NormalRightArrow.SetActive(true);
      }
      if (Object.op_Inequality((Object) this.currentNormalBanner, (Object) null) && Object.op_Inequality((Object) this.currentNormalBanner, (Object) centeredObject))
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1005");
      this.currentNormalBanner = centeredObject;
    }
  }

  private bool IsOpenLimitedShop() => this.shopStatus.shop_banners.Length != 0;

  public void IbtnBuyKiseki()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(PopupUtility.BuyKiseki());
  }

  public void IbtnMedalSlot()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("shop007_20", true);
  }

  public void IbtnTicketExchange()
  {
    if (this.IsPushAndSet())
      return;
    ShopTicketExchangeScene.ChangeScene();
  }

  public void IbtnCoinShop()
  {
    if (this.IsPushAndSet())
      return;
    ShopCoinExchangeScene.changeScene();
  }

  public void IbtnUnitPlus()
  {
    if (this.IsPushAndSet())
      return;
    if (SMManager.Observe<Player>().Value.CheckLimitMaxUnit())
      this.StartCoroutine(PopupUtility._999_11_1());
    else
      this.StartCoroutine(this.popup00714());
  }

  private IEnumerator popup00714()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_007_14__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab.Result);
    int quantity = MasterData.ShopContent[100000022].quantity;
    int price = MasterData.ShopArticle[10000002].price;
    gameObject.GetComponent<Shop00714Menu>().Init(price, quantity, new Action<int>(gameObject.GetComponent<Shop00714SetText>().SetText));
  }

  private bool IsAllAnimationWait()
  {
    return this.shopAnimation.IsWait() && ShopBackgroundAnimation.CurrentShopBackground.GetComponent<ShopBackgroundAnimation>().IsWait();
  }

  public void IbtnLimitedTab()
  {
    if (!this.IsAllAnimationWait())
      return;
    ((UIButtonColor) this.LimitedTabButton).isEnabled = false;
    ((UIButtonColor) this.NormalTabButton).isEnabled = true;
    ((UIWidget) this.LimitedTabButtonSprite).color = ShopCommon.TabDisableTextColor;
    ((UIWidget) this.NormalTabButtonSprite).color = new Color(0.349019617f, 0.349019617f, 0.349019617f);
    this.LimitedShopDot.SetActive(true);
    this.NormalShopDot.SetActive(false);
    this.shopAnimation.ChangeBanners(true);
    this.StartCoroutine(this.ChangeBackgroundLimitedTab());
    this.shopAnimation.ChangeStandCharacter(true);
  }

  public void IbtnNormalTab()
  {
    if (!this.IsAllAnimationWait())
      return;
    ((UIButtonColor) this.LimitedTabButton).isEnabled = true;
    ((UIButtonColor) this.NormalTabButton).isEnabled = false;
    ((UIWidget) this.LimitedTabButtonSprite).color = new Color(0.349019617f, 0.349019617f, 0.349019617f);
    ((UIWidget) this.NormalTabButtonSprite).color = ShopCommon.TabDisableTextColor;
    this.LimitedShopDot.SetActive(false);
    this.NormalShopDot.SetActive(true);
    this.shopAnimation.ChangeBanners(false);
    this.StartCoroutine(this.ChangeBackgroundNormalTab());
    this.shopAnimation.ChangeStandCharacter(false);
  }

  private IEnumerator ChangeBackgroundLimitedTab()
  {
    Future<GameObject> prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/BackGround/ShopLimitedBackground");
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    prefabF.Result.GetComponent<ShopBackgroundAnimation>().Change();
  }

  private IEnumerator ChangeBackgroundNormalTab()
  {
    Future<GameObject> prefabF = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    prefabF.Result.GetComponent<ShopBackgroundAnimation>().Change();
  }

  public void IbtnLimitedLeftArrow()
  {
    this.OnArrowButton(this.LimitedCenterOnChild, this.LimitedShopBanners, true);
  }

  public void IbtnLimitedRightArrow()
  {
    this.OnArrowButton(this.LimitedCenterOnChild, this.LimitedShopBanners, false);
  }

  public void IbtnNormalLeftArrow()
  {
    this.OnArrowButton(this.NormalCenterOnChild, this.NormalShopBanners, true);
  }

  public void IbtnNormalRightArrow()
  {
    this.OnArrowButton(this.NormalCenterOnChild, this.NormalShopBanners, false);
  }

  private void OnArrowButton(
    UICenterOnChild uICenterOnChild,
    List<GameObject> banners,
    bool isLeft)
  {
    int num1 = 0;
    int num2 = 0;
    GameObject centeredObject = uICenterOnChild.centeredObject;
    foreach (Object banner in banners)
    {
      if (Object.op_Equality(banner, (Object) centeredObject))
      {
        num2 = num1;
        break;
      }
      ++num1;
    }
    if (isLeft)
    {
      if (num2 <= 0)
        return;
      GameObject banner = banners[num2 - 1];
      uICenterOnChild.CenterOn(banner.transform);
    }
    else
    {
      if (num2 >= banners.Count - 1)
        return;
      GameObject banner = banners[num2 + 1];
      uICenterOnChild.CenterOn(banner.transform);
    }
  }

  public bool IsBannerSpringPanelSlideing()
  {
    return Object.op_Inequality((Object) this.LimitedCenterOnChild.centeredObject, (Object) this.currentLimitedBanner) || Object.op_Inequality((Object) this.NormalCenterOnChild.centeredObject, (Object) this.currentNormalBanner);
  }

  public override void onBackButton() => this.IbtnBack();

  private void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public IEnumerator FadeOut()
  {
    TweenAlpha alpha = this.mainPanel.GetOrAddComponent<TweenAlpha>();
    alpha.from = 1f;
    alpha.to = 0.0f;
    ((UITweener) alpha).style = (UITweener.Style) 0;
    ((UITweener) alpha).duration = 0.2f;
    ((UITweener) alpha).animationCurve = new AnimationCurve();
    ((UITweener) alpha).animationCurve.AddKey(new Keyframe(0.0f, 0.0f, 2f, 2f));
    ((UITweener) alpha).animationCurve.AddKey(new Keyframe(1f, 1f, 0.0f, 0.0f));
    while ((double) alpha.value > 0.0)
      yield return (object) null;
    this.mainPanel.SetActive(false);
    yield return (object) null;
    Object.Destroy((Object) alpha);
    ((UIRect) this.mainPanel.GetComponent<UIPanel>()).alpha = 1f;
  }
}
