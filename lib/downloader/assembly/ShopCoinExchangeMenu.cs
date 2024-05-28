// Decompiled with JetBrains decompiler
// Type: ShopCoinExchangeMenu
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
public class ShopCoinExchangeMenu : ShopArticleListMenu
{
  private const int shop_id = 9000;
  private Shop[] shopStatus;
  private bool isCacheShopStatus;
  [SerializeField]
  private NGHorizontalScrollParts indicatorCoin;
  [SerializeField]
  private UIScrollView scrollViewCoin;
  [SerializeField]
  private UIGrid gridCoin;
  [SerializeField]
  private UICenterOnChild uiCenterCoin;
  private List<int> coinIds;
  private int selectCoinIndex;
  private GameObject nowCenterCoinObj;
  private GameObject dirCoinExchangeIconPrefab;
  private List<GameObject> iconsCoin;
  private int selectId;
  [SerializeField]
  private UILabel txtCoinExplanation;
  [SerializeField]
  private UILabel txtCoinNum;
  [SerializeField]
  private UILabel txtAfter;
  [SerializeField]
  private UILabel txtDayValue;
  [SerializeField]
  private UILabel txtDay;
  [SerializeField]
  private UILabel txtNoDeadLine;
  [SerializeField]
  private NGxScroll2 scrollArticles;
  [SerializeField]
  private UIScrollView scrollViewArticles;
  [SerializeField]
  private UIGrid gridArticles;
  [SerializeField]
  private UIScrollBar scrollBarArticles;
  [SerializeField]
  private GameObject dirNone;
  private GameObject articleItemPrefab;
  private GameObject unitIconPrefab;
  private GameObject itemIconPrefab;
  private GameObject uniqueIconPrefab;
  private List<ArticleInfo> articleInfos = new List<ArticleInfo>();
  private List<ArticleInfo> articleInfosSoldout = new List<ArticleInfo>();
  private List<ArticleInfo> sortedArticleInfos = new List<ArticleInfo>();
  private List<GameObject> articleItemList = new List<GameObject>();
  private ArticleInfo info = new ArticleInfo();
  private const int coinPosX = 98;
  private const int articleDispNum = 3;

  protected override void Update()
  {
    base.Update();
    this.UpdateCoinIcons();
  }

  public IEnumerator Init(int select_id)
  {
    ShopCoinExchangeMenu coinExchangeMenu = this;
    coinExchangeMenu.coinIds = new List<int>();
    coinExchangeMenu.shopStatus = SMManager.Get<Shop[]>();
    coinExchangeMenu.isCacheShopStatus = true;
    foreach (Shop shopStatu in coinExchangeMenu.shopStatus)
    {
      if (shopStatu.id == 9000)
      {
        foreach (PlayerShopArticle article in shopStatu.articles)
        {
          if (article.article.pay_id.HasValue)
          {
            int pay_id = article.article.pay_id.Value;
            if (!coinExchangeMenu.coinIds.Exists((Predicate<int>) (x => x == pay_id)))
              coinExchangeMenu.coinIds.Add(article.article.pay_id.Value);
          }
        }
      }
    }
    if (coinExchangeMenu.coinIds.Count <= 0)
    {
      coinExchangeMenu.indicatorCoin.leftArrow.SetActive(false);
      coinExchangeMenu.indicatorCoin.rightArrow.SetActive(false);
      ((Component) coinExchangeMenu.txtCoinExplanation).gameObject.SetActive(false);
      ((Component) coinExchangeMenu.txtCoinNum).gameObject.SetActive(false);
      ((Component) coinExchangeMenu.txtAfter).gameObject.SetActive(false);
      ((Component) coinExchangeMenu.txtDayValue).gameObject.SetActive(false);
      ((Component) coinExchangeMenu.txtDay).gameObject.SetActive(false);
      ((Component) coinExchangeMenu.txtNoDeadLine).gameObject.SetActive(false);
      coinExchangeMenu.dirNone?.SetActive(true);
    }
    else
    {
      coinExchangeMenu.selectId = select_id;
      if (coinExchangeMenu.selectId == 0)
      {
        coinExchangeMenu.selectId = coinExchangeMenu.coinIds[0];
      }
      else
      {
        bool flag = false;
        foreach (int coinId in coinExchangeMenu.coinIds)
        {
          if (coinId == coinExchangeMenu.selectId)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          coinExchangeMenu.selectId = coinExchangeMenu.coinIds[0];
      }
      Future<GameObject> dirCoinExchangeIconPrefabF = new ResourceObject("Prefabs/shop007_CoinExchange/dir_CoinExchange_Icon").Load<GameObject>();
      IEnumerator e = dirCoinExchangeIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      coinExchangeMenu.dirCoinExchangeIconPrefab = dirCoinExchangeIconPrefabF.Result;
      dirCoinExchangeIconPrefabF = (Future<GameObject>) null;
      dirCoinExchangeIconPrefabF = new ResourceObject("Prefabs/shop007_4_1/vscroll7_4_1").Load<GameObject>();
      e = dirCoinExchangeIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      coinExchangeMenu.articleItemPrefab = dirCoinExchangeIconPrefabF.Result;
      dirCoinExchangeIconPrefabF = (Future<GameObject>) null;
      if (Object.op_Equality((Object) coinExchangeMenu.unitIconPrefab, (Object) null))
      {
        dirCoinExchangeIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
        e = dirCoinExchangeIconPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        coinExchangeMenu.unitIconPrefab = dirCoinExchangeIconPrefabF.Result;
        dirCoinExchangeIconPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) coinExchangeMenu.itemIconPrefab, (Object) null))
      {
        dirCoinExchangeIconPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
        e = dirCoinExchangeIconPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        coinExchangeMenu.itemIconPrefab = dirCoinExchangeIconPrefabF.Result;
        dirCoinExchangeIconPrefabF = (Future<GameObject>) null;
      }
      if (Object.op_Equality((Object) coinExchangeMenu.uniqueIconPrefab, (Object) null))
      {
        dirCoinExchangeIconPrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
        e = dirCoinExchangeIconPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        coinExchangeMenu.uniqueIconPrefab = dirCoinExchangeIconPrefabF.Result;
        dirCoinExchangeIconPrefabF = (Future<GameObject>) null;
      }
      e = coinExchangeMenu.LoadDetailPopup();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      coinExchangeMenu.selectCoinIndex = 0;
      coinExchangeMenu.indicatorCoin.destroyParts();
      coinExchangeMenu.iconsCoin = new List<GameObject>();
      for (int i = 0; i < coinExchangeMenu.coinIds.Count; ++i)
      {
        GameObject icon = coinExchangeMenu.indicatorCoin.instantiateParts(coinExchangeMenu.dirCoinExchangeIconPrefab, false);
        icon.GetComponent<UIDragScrollView>().scrollView = coinExchangeMenu.scrollViewCoin;
        yield return (object) icon.GetComponent<CoinExchangeIcon>().Init(coinExchangeMenu.coinIds[i], i, new Action<int>(coinExchangeMenu.OnCoinIcon));
        icon.transform.localPosition = new Vector3((float) (98 * i), 0.0f);
        coinExchangeMenu.iconsCoin.Add(icon);
        if (coinExchangeMenu.selectId == coinExchangeMenu.coinIds[i])
          coinExchangeMenu.selectCoinIndex = i;
        icon = (GameObject) null;
      }
      if (coinExchangeMenu.selectCoinIndex == 0)
        coinExchangeMenu.indicatorCoin.leftArrow.SetActive(false);
      else if (coinExchangeMenu.selectCoinIndex >= coinExchangeMenu.coinIds.Count - 1)
        coinExchangeMenu.indicatorCoin.rightArrow.SetActive(false);
      yield return (object) coinExchangeMenu.SetContents(new ShopCoinExchangeMenu.SetContentsParam(coinExchangeMenu.selectId));
    }
  }

  private IEnumerator SetContents(ShopCoinExchangeMenu.SetContentsParam sParam)
  {
    ShopCoinExchangeMenu coinExchangeMenu = this;
    CommonTicket commonTicket = MasterData.CommonTicket[sParam.id];
    if (commonTicket != null)
    {
      coinExchangeMenu.articleInfos.Clear();
      coinExchangeMenu.articleInfosSoldout.Clear();
      coinExchangeMenu.sortedArticleInfos.Clear();
      if (!coinExchangeMenu.isCacheShopStatus)
      {
        coinExchangeMenu.shopStatus = SMManager.Get<Shop[]>();
        coinExchangeMenu.isCacheShopStatus = true;
      }
      foreach (Shop shopStatu in coinExchangeMenu.shopStatus)
      {
        if (shopStatu.id == 9000)
        {
          foreach (PlayerShopArticle article in shopStatu.articles)
          {
            if (article.article.pay_id.Equals((object) sParam.id))
            {
              coinExchangeMenu.info = new ArticleInfo();
              coinExchangeMenu.info.shop = shopStatu;
              coinExchangeMenu.info.article = article;
              coinExchangeMenu.info.menu = (ShopArticleListMenu) coinExchangeMenu;
              coinExchangeMenu.info.onPurchased = new Func<IEnumerator>(coinExchangeMenu.ResetScrollList);
              coinExchangeMenu.info.onPurchasedHolding = new Action<long>(((ShopArticleListMenu) coinExchangeMenu).UpdatePurchasedHolding);
              if ((!article.limit.HasValue ? 0 : (article.limit.Value <= 0 ? 1 : 0)) != 0)
                coinExchangeMenu.articleInfosSoldout.Add(coinExchangeMenu.info);
              else
                coinExchangeMenu.articleInfos.Add(coinExchangeMenu.info);
            }
          }
        }
      }
      foreach (ArticleInfo articleInfo in coinExchangeMenu.articleInfosSoldout)
        coinExchangeMenu.articleInfos.Add(articleInfo);
      if (coinExchangeMenu.articleInfos.Count > 3 && sParam.isSetLoadingLayer)
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
      coinExchangeMenu.txtCoinExplanation.SetTextLocalize(commonTicket.name);
      int ticketNum = 0;
      foreach (PlayerCommonTicket playerCommonTicket in SMManager.Get<PlayerCommonTicket[]>())
      {
        if (playerCommonTicket.ticket_id == sParam.id)
        {
          ticketNum = NC.Clamp(0, Consts.GetInstance().SUBCOIN_DISP_MAX, playerCommonTicket.quantity);
          break;
        }
      }
      coinExchangeMenu.txtCoinNum.SetTextLocalize(Consts.GetInstance().SHOP_COIN_NUM.F((object) ticketNum));
      if (MasterData.CommonTicketEndAt.ContainsKey(sParam.id))
      {
        TimeSpan timeSpan = MasterData.CommonTicketEndAt[sParam.id].end_at - ServerTime.NowAppTime();
        ((Component) coinExchangeMenu.txtAfter).gameObject.SetActive(true);
        ((Component) coinExchangeMenu.txtDayValue).gameObject.SetActive(true);
        ((Component) coinExchangeMenu.txtDay).gameObject.SetActive(true);
        ((Component) coinExchangeMenu.txtNoDeadLine).gameObject.SetActive(false);
        coinExchangeMenu.txtDayValue.SetTextLocalize(timeSpan.Days);
      }
      else
      {
        ((Component) coinExchangeMenu.txtAfter).gameObject.SetActive(false);
        ((Component) coinExchangeMenu.txtDayValue).gameObject.SetActive(false);
        ((Component) coinExchangeMenu.txtDay).gameObject.SetActive(false);
        ((Component) coinExchangeMenu.txtNoDeadLine).gameObject.SetActive(true);
      }
      float sliderValue = 0.0f;
      if (sParam.isHoldPositionY)
        sliderValue = coinExchangeMenu.scrollViewArticles.verticalScrollBar.value;
      ((Component) coinExchangeMenu.gridArticles).transform.Clear();
      coinExchangeMenu.articleItemList = new List<GameObject>();
      foreach (ArticleInfo articleInfo in coinExchangeMenu.articleInfos)
      {
        GameObject gameObject = coinExchangeMenu.articleItemPrefab.Clone(((Component) coinExchangeMenu.gridArticles).transform);
        gameObject.SetActive(false);
        coinExchangeMenu.articleItemList.Add(gameObject);
      }
      int init_index = 0;
      if (sParam.isHoldPositionY)
      {
        for (; init_index < coinExchangeMenu.articleInfos.Count && init_index < coinExchangeMenu.articleItemList.Count; ++init_index)
          yield return (object) coinExchangeMenu.InitArticleItem(coinExchangeMenu.articleInfos[init_index], coinExchangeMenu.articleItemList[init_index], ticketNum);
      }
      else
      {
        for (; init_index < 3 && init_index < coinExchangeMenu.articleInfos.Count && init_index < coinExchangeMenu.articleItemList.Count; ++init_index)
          yield return (object) coinExchangeMenu.InitArticleItem(coinExchangeMenu.articleInfos[init_index], coinExchangeMenu.articleItemList[init_index], ticketNum);
      }
      foreach (GameObject articleItem in coinExchangeMenu.articleItemList)
        articleItem.SetActive(true);
      coinExchangeMenu.gridArticles.Reposition();
      coinExchangeMenu.scrollViewArticles.ResetPosition();
      ((UIProgressBar) coinExchangeMenu.scrollBarArticles).ForceUpdate();
      coinExchangeMenu.scrollArticles.ResolvePosition(new Vector2(0.0f, sliderValue));
      coinExchangeMenu.dirNone?.SetActive(coinExchangeMenu.articleItemList.Count <= 0);
      if (sParam.isSetLoadingLayer)
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      coinExchangeMenu.StartCoroutine(nameof (SetContents), (object) new ShopCoinExchangeMenu.ContinueParam(init_index, ticketNum));
    }
  }

  private IEnumerator SetContents(ShopCoinExchangeMenu.ContinueParam cParam)
  {
    int numInfos = this.articleInfos.Count;
    int numItems = this.articleItemList.Count;
    for (int n = cParam.index; n < numInfos && n < numItems; ++n)
    {
      IEnumerator e = this.InitArticleItem(this.articleInfos[n], this.articleItemList[n], cParam.ticketNum);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator InitArticleItem(ArticleInfo info, GameObject item, int ticketNum)
  {
    yield return (object) item.GetComponent<Shop0074Scroll>().Init(info.article, info.shop, info.menu, info.onPurchased, info.onPurchasedHolding, this.unitIconPrefab, this.itemIconPrefab, this.uniqueIconPrefab, sub_holding: (long) ticketNum);
  }

  private IEnumerator ResetScrollList()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ShopCoinExchangeMenu coinExchangeMenu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    coinExchangeMenu.isCacheShopStatus = false;
    coinExchangeMenu.StopCoroutine("SetContents");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) coinExchangeMenu.SetContents(new ShopCoinExchangeMenu.SetContentsParam(coinExchangeMenu.selectId, true, true));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void InitArticleScroll()
  {
    if (this.iconsCoin == null)
      return;
    this.selectCoinIndex = 0;
    for (int index = 0; index < this.coinIds.Count; ++index)
    {
      if (this.selectId == this.coinIds[index])
      {
        this.selectCoinIndex = index;
        break;
      }
    }
    GameObject gameObject = this.iconsCoin[this.selectCoinIndex];
    this.uiCenterCoin.CenterOn(gameObject.transform);
    this.nowCenterCoinObj = gameObject;
    this.indicatorCoin.setItemPositionQuick(this.selectCoinIndex);
    gameObject.GetComponent<CoinExchangeIcon>().setBaseOnOff(true);
    this.gridArticles.Reposition();
    this.scrollViewArticles.ResetPosition();
    ((UIProgressBar) this.scrollBarArticles).ForceUpdate();
    this.StartCoroutine(this.WaitScrollSe());
  }

  protected IEnumerator WaitScrollSe()
  {
    yield return (object) new WaitForSeconds(0.3f);
    this.indicatorCoin.SeEnable = true;
  }

  protected void UpdateCoinIcons()
  {
    if (this.iconsCoin == null || Object.op_Equality((Object) this.nowCenterCoinObj, (Object) null))
      return;
    GameObject centeredObject = ((Component) ((Component) this.gridCoin).transform).GetComponent<UICenterOnChild>().centeredObject;
    if (!Object.op_Inequality((Object) this.nowCenterCoinObj, (Object) centeredObject))
      return;
    int num = 0;
    foreach (GameObject targetIcon in this.iconsCoin)
    {
      this.HscrollButtonCenterChange(targetIcon);
      if (Object.op_Equality((Object) targetIcon, (Object) centeredObject))
        this.selectCoinIndex = num;
      ++num;
    }
    this.nowCenterCoinObj = centeredObject;
  }

  private void HscrollButtonCenterChange(GameObject targetIcon)
  {
    CoinExchangeIcon component = targetIcon.GetComponent<CoinExchangeIcon>();
    if (Object.op_Equality((Object) targetIcon, (Object) ((Component) ((Component) this.gridCoin).transform).GetComponent<UICenterOnChild>().centeredObject))
    {
      component.setBaseOnOff(true);
      this.StopCoroutine("SetContents");
      this.selectId = component.id;
      this.StartCoroutine("SetContents", (object) new ShopCoinExchangeMenu.SetContentsParam(this.selectId, true));
    }
    else
      component.setBaseOnOff(false);
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnLeftArrow()
  {
    if (this.IsPushAndSet())
      return;
    int selectCoinIndex = this.selectCoinIndex;
    --this.selectCoinIndex;
    if (this.selectCoinIndex < 0)
      this.selectCoinIndex = 0;
    if (this.selectCoinIndex != selectCoinIndex)
    {
      this.SetCenterCoin();
      this.indicatorCoin.playSE();
    }
    this.StartCoroutine(this.IsPushOff());
  }

  public void IbtnRightArrow()
  {
    if (this.IsPushAndSet())
      return;
    int selectCoinIndex = this.selectCoinIndex;
    ++this.selectCoinIndex;
    if (this.selectCoinIndex > this.iconsCoin.Count - 1)
      this.selectCoinIndex = this.iconsCoin.Count - 1;
    if (this.selectCoinIndex != selectCoinIndex)
    {
      this.SetCenterCoin();
      this.indicatorCoin.playSE();
    }
    this.StartCoroutine(this.IsPushOff());
  }

  protected void SetCenterCoin()
  {
    if (this.iconsCoin == null)
      return;
    GameObject gameObject = this.iconsCoin[this.selectCoinIndex];
    this.uiCenterCoin.CenterOn(gameObject.transform);
    this.nowCenterCoinObj = gameObject;
    this.indicatorCoin.setItemPositionQuick(this.selectCoinIndex);
    foreach (GameObject targetIcon in this.iconsCoin)
      this.HscrollButtonCenterChange(targetIcon);
  }

  public void OnCoinIcon(int index)
  {
    if (this.IsPushAndSet())
      return;
    int selectCoinIndex = this.selectCoinIndex;
    this.selectCoinIndex = index;
    if (this.selectCoinIndex != selectCoinIndex)
    {
      this.SetCenterCoin();
      this.indicatorCoin.playSE();
    }
    this.StartCoroutine(this.IsPushOff());
  }

  private class SetContentsParam
  {
    public int id { get; private set; }

    public bool isSetLoadingLayer { get; private set; }

    public bool isHoldPositionY { get; private set; }

    public SetContentsParam(int id, bool isSetLoadingLayer = false, bool isHoldPositionY = false)
    {
      this.id = id;
      this.isSetLoadingLayer = isSetLoadingLayer;
      this.isHoldPositionY = isHoldPositionY;
    }
  }

  private class ContinueParam
  {
    public int index { get; private set; }

    public int ticketNum { get; private set; }

    public ContinueParam(int index, int ticketNum)
    {
      this.index = index;
      this.ticketNum = ticketNum;
    }
  }
}
