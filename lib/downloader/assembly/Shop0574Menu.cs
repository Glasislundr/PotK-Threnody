// Decompiled with JetBrains decompiler
// Type: Shop0574Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Shop0574Menu : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll scroll;
  private GameObject ListPrefab;
  private GameObject ConfirmPopupPrefab;
  private GameObject FinalConfirmPopupPrefab;
  private GameObject DetailPopupPrefab;

  public IEnumerator InitSceneAsync()
  {
    Future<GameObject> scrollPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.ListPrefab, (Object) null))
    {
      scrollPrefabF = Res.Prefabs.shop057_4.vscroll57_4.Load<GameObject>();
      e = scrollPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ListPrefab = scrollPrefabF.Result;
      scrollPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.ConfirmPopupPrefab, (Object) null))
    {
      scrollPrefabF = Res.Prefabs.popup.popup_057_1.Load<GameObject>();
      e = scrollPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ConfirmPopupPrefab = scrollPrefabF.Result;
      scrollPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.FinalConfirmPopupPrefab, (Object) null))
    {
      scrollPrefabF = Res.Prefabs.popup.popup_057_2.Load<GameObject>();
      e = scrollPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.FinalConfirmPopupPrefab = scrollPrefabF.Result;
      scrollPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.DetailPopupPrefab, (Object) null))
    {
      scrollPrefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = scrollPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.DetailPopupPrefab = scrollPrefabF.Result;
      scrollPrefabF = (Future<GameObject>) null;
    }
  }

  public IEnumerator StartSceneAsync()
  {
    Shop0574Menu shop0574Menu = this;
    int episodeID = 0;
    EarthDataManager earthDataManager = Singleton<EarthDataManager>.GetInstanceOrNull();
    if (Object.op_Inequality((Object) earthDataManager, (Object) null))
      episodeID = earthDataManager.questProgress.currentEpisode.ID;
    List<EarthShopArticle> list = ((IEnumerable<EarthShopArticle>) MasterData.EarthShopArticleList).Where<EarthShopArticle>((Func<EarthShopArticle, bool>) (x =>
    {
      int num1 = 0;
      if (Object.op_Inequality((Object) earthDataManager, (Object) null))
        num1 = earthDataManager.GetShopPurchaseCount(x.ID);
      if (x.disablel_if_sold_out && x.limit.HasValue && x.limit.Value - num1 <= 0)
        return false;
      int num2 = x.start_id.HasValue ? x.start_id.Value : 0;
      int num3 = x.end_id.HasValue ? x.end_id.Value : 10000;
      int num4 = episodeID;
      return num2 <= num4 && episodeID <= num3 && x.IsDisableUntilPurchaseOnArticleID && x.IsDisableUntilSoldOutArticleID;
    })).ToList<EarthShopArticle>();
    shop0574Menu.scroll.Clear();
    foreach (EarthShopArticle article in list)
    {
      GameObject gameObject = shop0574Menu.ListPrefab.Clone();
      shop0574Menu.scroll.Add(gameObject);
      gameObject.gameObject.SetActive(false);
      IEnumerator e = gameObject.GetComponent<Shop0574Scroll>().Init(article, new Action<EarthShopArticle, GameObject>(shop0574Menu.openConfirmPopup), new Action<EarthShopArticle>(shop0574Menu.openDetailPopup));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    foreach (Component child in ((Component) shop0574Menu.scroll.grid).transform.GetChildren())
      child.gameObject.SetActive(true);
    shop0574Menu.scroll.ResolvePosition();
  }

  public override void onBackButton()
  {
    if (Singleton<PopupManager>.GetInstance().isOpen)
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
    }
    else
    {
      Singleton<NGSceneManager>.GetInstance().clearStack();
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
      Singleton<NGSceneManager>.GetInstance().changeScene("mypage051", false);
    }
  }

  private void openDetailPopup(EarthShopArticle article)
  {
    this.StartCoroutine(this.openDetailPopupAsync(article));
  }

  private IEnumerator openDetailPopupAsync(EarthShopArticle article)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.DetailPopupPrefab);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Shop00742Menu>().Init(article.ShopContents);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  private void completeBuy(EarthShopArticle article, GameObject thum, int buyNum)
  {
    EarthDataManager instanceOrNull = Singleton<EarthDataManager>.GetInstanceOrNull();
    if (!Object.op_Inequality((Object) instanceOrNull, (Object) null))
      return;
    instanceOrNull.ShopBuy(article, buyNum);
    this.StartCoroutine(this.StartSceneAsync());
    this.StartCoroutine(this.openCompletePopup(article, thum, buyNum));
  }

  private void openConfirmPopup(EarthShopArticle article, GameObject thum)
  {
    if (Singleton<PopupManager>.GetInstance().isOpen)
      return;
    GameObject prefab = this.ConfirmPopupPrefab.Clone();
    prefab.GetComponent<Shop0574ConfirmPopup>().Init(article, thum, this.FinalConfirmPopupPrefab, new Action<EarthShopArticle, GameObject, int>(this.completeBuy));
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  private IEnumerator openCompletePopup(EarthShopArticle article, GameObject thum, int buyNum)
  {
    Future<GameObject> prefab0078F = Res.Prefabs.popup.popup_007_8__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab0078F.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefab0078F.Result.Clone();
    Shop0078Menu component = prefab.GetComponent<Shop0078Menu>();
    component.InitObj(thum);
    component.InitDataSet(article.name, buyNum, article.ShopContents.GetPossessionNum() - buyNum);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }
}
