// Decompiled with JetBrains decompiler
// Type: ShopArticleListMenu
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
public class ShopArticleListMenu : BackButtonMenuBase
{
  [SerializeField]
  private int EnableShopID;
  [SerializeField]
  public ShopArticleListMenu.CurrencyType currencyType = ShopArticleListMenu.CurrencyType.Zeny;
  [SerializeField]
  private int CellWidth = 620;
  [SerializeField]
  private int CellHeight = 180;
  [SerializeField]
  private int ColumnValue = 1;
  [SerializeField]
  private int CellScreenValue = 8;
  [SerializeField]
  private int CellMaxValue = 12;
  [SerializeField]
  protected NGxScroll2 scroll;
  private float sliderValue;
  private List<Shop0074Scroll> allCellObject = new List<Shop0074Scroll>();
  private List<ArticleInfo> allArticleInfos = new List<ArticleInfo>();
  private bool isInitialize;
  private float scrool_start_y;
  private GameObject CellObject;
  private GameObject detailPopup;
  protected GameObject mapDetailPopup;
  protected GameObject facilityDetailPopup;
  private GameObject unitIconPrefab;
  private GameObject itemIconPrefab;
  private GameObject uniqueIconPrefab;

  public List<Shop0074Scroll> ScrollList => this.allCellObject;

  public GameObject DetailPopup => this.detailPopup;

  public GameObject MapDetailPopup => this.mapDetailPopup;

  public GameObject FacilityDetailPopup => this.facilityDetailPopup;

  protected virtual IEnumerator LoadDetailPopup()
  {
    if (Object.op_Equality((Object) this.detailPopup, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  protected virtual void UpdatePurchasedHolding(int nextholding)
  {
  }

  protected virtual void UpdatePurchasedHolding(long nextholding)
  {
  }

  private IEnumerator ResetScrollList()
  {
    IEnumerator e = this.Init((Future<GameObject>) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void CreateArticleInfoList()
  {
    Shop[] shopArray = SMManager.Get<Shop[]>();
    PlayerItem[] self1 = SMManager.Get<PlayerItem[]>();
    PlayerMaterialGear[] self2 = SMManager.Get<PlayerMaterialGear[]>();
    this.allArticleInfos.Clear();
    foreach (Shop shop in shopArray)
    {
      foreach (PlayerShopArticle playerShopArticle in ((IEnumerable<PlayerShopArticle>) shop.articles).OrderBy<PlayerShopArticle, bool>((Func<PlayerShopArticle, bool>) (x =>
      {
        if (x.article.limit.HasValue && x.limit.Value <= 0)
          return true;
        return x.article.daily_limit.HasValue && x.limit.Value <= 0;
      })).ThenBy<PlayerShopArticle, int>((Func<PlayerShopArticle, int>) (x => x.article.view_order)).ToList<PlayerShopArticle>())
      {
        if (playerShopArticle.article.shop.ID == this.EnableShopID)
        {
          int entityId = playerShopArticle.article.ShopContents[0].entity_id;
          MasterDataTable.CommonRewardType entityType = playerShopArticle.article.ShopContents[0].entity_type;
          int num = 0 + self1.AmountHavingTargetItem(entityId, entityType);
          self2.AmountHavingTargetItem(entityId);
          this.allArticleInfos.Add(new ArticleInfo()
          {
            shop = shop,
            article = playerShopArticle,
            menu = this,
            onPurchased = new Func<IEnumerator>(this.ResetScrollList),
            onPurchasedHolding = new Action<long>(this.UpdatePurchasedHolding)
          });
        }
      }
    }
  }

  public virtual IEnumerator Init(Future<GameObject> cellPrefab)
  {
    ShopArticleListMenu shopArticleListMenu = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = shopArticleListMenu.LoadDetailPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) shopArticleListMenu.CellObject, (Object) null) && cellPrefab != null)
    {
      e = cellPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      shopArticleListMenu.CellObject = cellPrefab.Result;
    }
    Future<GameObject> unitIconPrefabF;
    if (Object.op_Equality((Object) shopArticleListMenu.unitIconPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      shopArticleListMenu.unitIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) shopArticleListMenu.itemIconPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      shopArticleListMenu.itemIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) shopArticleListMenu.uniqueIconPrefab, (Object) null))
    {
      unitIconPrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
      e = unitIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      shopArticleListMenu.uniqueIconPrefab = unitIconPrefabF.Result;
      unitIconPrefabF = (Future<GameObject>) null;
    }
    shopArticleListMenu.isInitialize = false;
    shopArticleListMenu.scroll.Clear();
    shopArticleListMenu.CreateArticleInfoList();
    if (shopArticleListMenu.allArticleInfos.Count > 0)
    {
      e = shopArticleListMenu.CreateCellBase(shopArticleListMenu.CellObject);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    shopArticleListMenu.scrool_start_y = ((Component) shopArticleListMenu.scroll.scrollView).transform.localPosition.y;
    shopArticleListMenu.isInitialize = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    shopArticleListMenu.StartCoroutine(shopArticleListMenu.DragScroll());
  }

  private IEnumerator DragScroll()
  {
    this.scroll.ResolvePosition(new Vector2(0.0f, this.sliderValue));
    yield break;
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  protected void ScrollUpdate()
  {
    if (!this.isInitialize || this.allArticleInfos.Count <= this.CellScreenValue)
      return;
    int num1 = this.CellHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.allArticleInfos.Count - this.CellScreenValue - 1) / this.ColumnValue * this.CellHeight);
    float num4 = (float) (this.CellHeight * this.CellMaxValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    bool flag;
    do
    {
      flag = false;
      int num5 = 0;
      foreach (GameObject gameObject in this.scroll)
      {
        GameObject go = gameObject;
        float num6 = go.transform.localPosition.y + num2;
        if ((double) num6 > (double) num1)
        {
          int? nullable = this.allArticleInfos.FirstIndexOrNull<ArticleInfo>((Func<ArticleInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) go)));
          int info_index = nullable.HasValue ? nullable.Value + this.CellMaxValue : this.allArticleInfos.Count;
          if (nullable.HasValue && info_index < this.allArticleInfos.Count)
          {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.allArticleInfos.Count)
            {
              go.SetActive(false);
            }
            else
            {
              this.ResetScroll(num5);
              this.StartCoroutine(this.CreateCell(info_index, num5));
            }
            flag = true;
          }
        }
        else if ((double) num6 < -((double) num4 - (double) num1))
        {
          int num7 = this.CellMaxValue;
          if (!go.activeSelf)
          {
            go.SetActive(true);
            num7 = 0;
          }
          int? nullable = this.allArticleInfos.FirstIndexOrNull<ArticleInfo>((Func<ArticleInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) go)));
          int info_index = nullable.HasValue ? nullable.Value - num7 : -1;
          if (nullable.HasValue && info_index >= 0)
          {
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y + num4, 0.0f);
            this.ResetScroll(num5);
            this.StartCoroutine(this.CreateCell(info_index, num5));
            flag = true;
          }
        }
        ++num5;
      }
    }
    while (flag);
  }

  private void ResetScroll(int index)
  {
    Shop0074Scroll scroll = this.allCellObject[index];
    ((Component) scroll).gameObject.SetActive(false);
    this.allArticleInfos.Where<ArticleInfo>((Func<ArticleInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<ArticleInfo>((Action<ArticleInfo>) (b => b.scroll = (Shop0074Scroll) null));
  }

  private IEnumerator CreateCellBase(GameObject prefab)
  {
    this.allCellObject.Clear();
    for (int index = 0; index < Mathf.Min(this.CellMaxValue, this.allArticleInfos.Count); ++index)
      this.allCellObject.Add(Object.Instantiate<GameObject>(prefab).GetComponent<Shop0074Scroll>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(this.CellMaxValue, this.allCellObject.Count); ++index)
      this.scroll.AddColumn1(((Component) this.allCellObject[index]).gameObject, this.CellWidth, this.CellHeight);
    this.scroll.CreateScrollPointHeight(this.CellHeight, this.allArticleInfos.Count);
    this.sliderValue = this.scroll.scrollView.verticalScrollBar.value;
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(this.CellMaxValue, this.allArticleInfos.Count); ++index)
      this.ResetScroll(index);
    for (int i = 0; i < Mathf.Min(this.CellMaxValue, this.allArticleInfos.Count); ++i)
    {
      IEnumerator e = this.CreateCell(i, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator CreateCell(int info_index, int obj_index)
  {
    Shop0074Scroll scroll = this.allCellObject[obj_index];
    this.allArticleInfos.Where<ArticleInfo>((Func<ArticleInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<ArticleInfo>((Action<ArticleInfo>) (b => b.scroll = (Shop0074Scroll) null));
    ArticleInfo allArticleInfo = this.allArticleInfos[info_index];
    allArticleInfo.scroll = scroll;
    IEnumerator e = scroll.Init(allArticleInfo.article, allArticleInfo.shop, allArticleInfo.menu, allArticleInfo.onPurchased, allArticleInfo.onPurchasedHolding, this.unitIconPrefab, this.itemIconPrefab, this.uniqueIconPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) scroll).gameObject.SetActive(true);
  }

  public override void onBackButton()
  {
  }

  public enum CurrencyType
  {
    RareMedal,
    BattleMedal,
    Zeny,
    TowerMedal,
    GuildMedal,
    RaidMedal,
    SubCoin,
  }
}
