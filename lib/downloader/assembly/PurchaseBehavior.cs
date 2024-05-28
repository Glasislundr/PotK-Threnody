// Decompiled with JetBrains decompiler
// Type: PurchaseBehavior
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using Gsc.Purchase;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PurchaseBehavior : Singleton<PurchaseBehavior>
{
  public static PurchaseFlow actualFlow;

  public static bool IsBattleNow { get; set; }

  public static bool IsOpen { get; set; }

  public static int UsedCoinInBattleHere
  {
    get
    {
      if (PurchaseBehavior.IsBattleNow)
      {
        NGBattleManager instance = Singleton<NGBattleManager>.GetInstance();
        if (Object.op_Inequality((Object) instance, (Object) null) && instance.environment != null)
          return instance.environment.core.continueCount;
      }
      return 0;
    }
  }

  protected override void Initialize()
  {
  }

  public static IEnumerator OpenAPRecoveryItemList(bool isAPShortage, Action questChangeScene)
  {
    PurchaseBehavior.IsOpen = false;
    PurchaseBehavior.IsBattleNow = false;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    yield return (object) null;
    PlayerRecoveryItem[] apRecoveryItems = SMManager.Get<PlayerRecoveryItem[]>();
    Future<GameObject> prefab = new ResourceObject("Prefabs/popup/popup_AP_Recovery").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PurchaseBehavior.IsOpen = true;
    e = PurchaseBehavior.PopupOpen(prefab.Result).GetComponent<APRecovery>().Init(isAPShortage, apRecoveryItems, questChangeScene);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public static IEnumerator OpenItemList(bool isBattle)
  {
    PurchaseBehavior.IsOpen = false;
    PurchaseBehavior.IsBattleNow = isBattle;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.CoinbonusHistory> handler = WebAPI.CoinbonusHistory();
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_007_9__anim_popup01.Load<GameObject>();
    e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PurchaseBehavior.IsOpen = true;
    e = PurchaseBehavior.PopupOpen(prefab.Result).GetComponent<Shop0079Menu>().Init(handler.Result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public static GameObject PopupOpen(GameObject prefab)
  {
    return PurchaseBehavior.IsBattleNow ? Singleton<NGBattleManager>.GetInstance().popupOpen(prefab) : Singleton<PopupManager>.GetInstance().open(prefab);
  }

  public static void PopupDismiss(bool withoutAnim = false)
  {
    if (PurchaseBehavior.IsBattleNow)
      Singleton<NGBattleManager>.GetInstance().popupDismiss(withoutAnim: withoutAnim);
    else if (withoutAnim)
      Singleton<PopupManager>.GetInstance().dismissWithoutAnim();
    else
      Singleton<PopupManager>.GetInstance().dismiss();
  }

  public static void PopupAllDismiss()
  {
    if (PurchaseBehavior.IsBattleNow)
      Singleton<NGBattleManager>.GetInstance().popupCloseAll();
    else
      Singleton<PopupManager>.GetInstance().closeAll();
  }

  public static void ShowPopupYesNo(string title, string message, Action callback = null)
  {
    ModalWindow.ShowYesNo(title, message, (Action) (() =>
    {
      PurchaseBehaviorLoadingLayer.Disable();
      if (callback == null)
        return;
      callback();
    }), (Action) (() => PurchaseBehaviorLoadingLayer.Disable()));
  }

  public static void ShowPopupWithMessage(string title, string message, Action callback = null)
  {
    PopupCommon component = PurchaseBehavior.PopupOpen(PopupCommon.LoadPrefab()).GetComponent<PopupCommon>();
    component.Init(title, message);
    component.OK.onClick.Clear();
    EventDelegate.Add(component.OK.onClick, (EventDelegate.Callback) (() => PurchaseBehavior.PopupDismiss()));
    if (callback == null)
      return;
    EventDelegate.Add(component.OK.onClick, (EventDelegate.Callback) (() => callback()));
  }

  public static void SendAge(int year, int month, int day)
  {
    PurchaseBehavior.actualFlow.InputBirthday(year, month, day);
  }

  public static void ShowInputBirthday(bool isYes)
  {
    if (isYes)
    {
      Singleton<PurchaseBehavior>.GetInstance().StartCoroutine(Singleton<PurchaseBehavior>.GetInstance().ShowPopupInputBirthday());
    }
    else
    {
      PurchaseBehaviorLoadingLayer.Disable();
      PurchaseBehavior.PopupDismiss();
    }
  }

  private IEnumerator ShowPopupInputBirthday()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_999_8_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = PurchaseBehavior.PopupOpen(prefab.Result);
    ((Component) gameObject.transform.parent).GetComponent<UIPanel>().depth = 110;
    gameObject.GetComponent<Shop99981Menu>().SetOnCancel((Action) (() => PurchaseBehaviorLoadingLayer.Disable()));
  }
}
