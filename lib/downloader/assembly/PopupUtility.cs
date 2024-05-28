// Decompiled with JetBrains decompiler
// Type: PopupUtility
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
public static class PopupUtility
{
  public static IEnumerator _999_11_1()
  {
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = Res.Prefabs.popup.popup_999_11_1__anim_popup01.Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(fobj.Result).GetComponent<Shop999111Menu>().SetText();
  }

  public static IEnumerator _999_12_1()
  {
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = Res.Prefabs.popup.popup_999_12_1__anim_popup01.Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(fobj.Result).GetComponent<Shop999121Menu>().SetText();
  }

  public static IEnumerator _007_16(float wait = 0.0f)
  {
    Player player_data = SMManager.Get<Player>();
    yield return (object) new WaitForSeconds(wait);
    int quantity = MasterData.ShopContent[100000033].quantity;
    int pri = MasterData.ShopArticle[10000003].price;
    int now = player_data.max_items;
    int max = player_data.max_items + quantity;
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = Res.Prefabs.popup.popup_007_16__anim_popup01.Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(fobj.Result);
    gameObject.GetComponent<Shop00716Menu>().Init(pri, now, max);
    gameObject.GetComponent<Shop00716SetText>().SetText(now, max);
  }

  public static IEnumerator _007_14(float wait)
  {
    SMManager.Get<Player>();
    yield return (object) new WaitForSeconds(wait);
    int add = MasterData.ShopContent[100000022].quantity;
    int pri = MasterData.ShopArticle[10000002].price;
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = Res.Prefabs.popup.popup_007_14__anim_popup01.Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(fobj.Result);
    gameObject.GetComponent<Shop00714Menu>().Init(pri, add, new Action<int>(gameObject.GetComponent<Shop00714SetText>().SetText));
  }

  public static IEnumerator _999_7_1(ShopArticle shopArticle)
  {
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = Res.Prefabs.popup.popup_999_7_1__anim_popup01.Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(fobj.Result).GetComponent<Shop99971Menu>().SetText(shopArticle);
  }

  public static IEnumerator _999_7_1(CommonPayType payType)
  {
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = Res.Prefabs.popup.popup_999_7_1__anim_popup01.Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(fobj.Result).GetComponent<Shop99971Menu>().SetText(payType);
  }

  public static IEnumerator _999_6_1(bool Gacha, bool bStackScene = true)
  {
    Player player_data = SMManager.Get<Player>();
    int now = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Count<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && !x.isReisou()));
    int max = player_data.max_items;
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = !Gacha ? Res.Prefabs.popup.popup_999_6_1__anim_popup01.Load<GameObject>() : Res.Prefabs.popup.popup_999_6_1a__anim_popup01.Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(fobj.Result);
    if (Gacha)
    {
      Gacha99961aMenu component = gameObject.GetComponent<Gacha99961aMenu>();
      component.isStackScene = bStackScene;
      component.SetText(now, max, player_data);
    }
    else
    {
      Quest99961Menu component = gameObject.GetComponent<Quest99961Menu>();
      component.isStackScene = bStackScene;
      component.SetText(now, max, player_data);
    }
  }

  public static IEnumerator popupMaxReisou()
  {
    Player player = SMManager.Get<Player>();
    int now = ((IEnumerable<PlayerItem>) ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.isReisou())).ToArray<PlayerItem>()).ToList<PlayerItem>().Count;
    int max = player.max_reisou_items;
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = new ResourceObject("Prefabs/popup/popup_Item_Possession_limit_Over__anim_popup01").Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(fobj.Result).GetComponent<Quest999ReisouMenu>().SetText(now, max);
  }

  public static IEnumerator _999_5_1(
    Action<PopupUtility.SceneTo> callChangedScene = null,
    bool bStackScene = true)
  {
    Player player_data = SMManager.Get<Player>();
    int now = SMManager.Get<PlayerUnit[]>().Length;
    int max = player_data.max_units;
    int reserves_now = SMManager.Get<PlayerUnitReservesCount>().count;
    int reserves_max = player_data.max_unit_reserves;
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = Res.Prefabs.popup.popup_999_5_1a__anim_popup01.Load<GameObject>();
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Gacha99951aMenu component = Singleton<PopupManager>.GetInstance().open(fobj.Result).GetComponent<Gacha99951aMenu>();
    component.onChangedScene = callChangedScene;
    component.isStackScene = bStackScene;
    component.SetText(now, max, reserves_now, reserves_max, player_data);
  }

  public static IEnumerator BuyKiseki(bool isBattle = false)
  {
    IEnumerator e = PurchaseBehavior.OpenItemList(isBattle);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator RecoveryAP(bool isAPShortage = false, Action questChange = null)
  {
    IEnumerator e;
    if (questChange == null)
    {
      e = PurchaseBehavior.OpenAPRecoveryItemList(isAPShortage, (Action) (() => { }));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = PurchaseBehavior.OpenAPRecoveryItemList(isAPShortage, questChange);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator _999_15(string name = null)
  {
    Future<GameObject> fobj = (Future<GameObject>) null;
    fobj = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/popup/popup_999_15__anim_popup");
    IEnumerator e = fobj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(fobj.Result).GetComponent<Shop99915Menu>().SetText(name);
  }

  public static IEnumerator _007_19()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_007_19__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<Shop00719Menu>().setData();
  }

  public static IEnumerator _007_18()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_007_18__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<Shop00718Menu>().setData();
  }

  public static IEnumerator _999_3_1(
    string text1 = "",
    string text2 = "",
    string text3 = "",
    Gacha99931Menu.PaymentType paymentType = Gacha99931Menu.PaymentType.ALL)
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_999_3_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<Gacha99931Menu>().SetText(text1, text2, text3, paymentType);
  }

  public static IEnumerator SeaError(WebAPI.Response.UserError error)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    ModalWindow.Show(error.Code, error.Reason, (Action) (() =>
    {
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
      Singleton<NGGameDataManager>.GetInstance().isCallHomeUpdateAllData = true;
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
      MypageScene.ChangeSceneOnError();
    }));
    yield return (object) null;
  }

  public static IEnumerator SeaErrorStartUp(WebAPI.Response.UserError error)
  {
    ModalWindow.Show(error.Code, error.Reason, (Action) (() =>
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy((Action<NGGameDataManager.StartSceneProxyResult>) (data => MypageScene.ChangeSceneOnError()));
    }));
    yield return (object) null;
  }

  public enum SceneTo
  {
    UnitTraining,
    UnitSale,
    UnitStorage,
  }
}
