// Decompiled with JetBrains decompiler
// Type: Gacha0063Menu
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
public class Gacha0063Menu : BackButtonMenuBase
{
  public static readonly int PaymentTypeIDCompensation = 9;
  public Gacha0063Scene scene;
  public GameObject gachaChargePrefab;
  public GameObject gachaTicketSliderSelectPopupPrefab;
  public GameObject gachaPtErrorPrefab;
  public GameObject popupSheet;
  public CommonElementIcon commonElementIcon;
  public bool isSheetPopup;

  public bool CheckGachaCharge(GachaModuleGacha gachaData, int gachaNumber)
  {
    return !this.isSheetPopup && this.GachaCheckSelectedPickup(gachaData, gachaNumber) && this.GachaCheckSufficiency(gachaData) && this.GachaCheckUnit(gachaData);
  }

  public bool CheckGachaPt(GachaModuleGacha gachaData)
  {
    return !this.isSheetPopup && this.GachaCheckFriend(gachaData) && this.ManaGachaCheckUnit() && this.GachaCheckItem() && this.GachaCheckReisou();
  }

  public bool CheckGachaTicket(GachaModuleGacha gacha_data)
  {
    return !this.isSheetPopup && this.TicketGachaCheckUnit() && this.GachaCheckItem() && this.GachaCheckReisou();
  }

  private bool GachaCheckSelectedPickup(GachaModuleGacha gachaData, int gachaNumber)
  {
    if (!gachaData.is_pickup_select || gachaData.is_selected_pickup)
      return true;
    this.StartCoroutine(this.PickupEmptyError(gachaData, gachaNumber));
    return false;
  }

  private IEnumerator PickupEmptyError(GachaModuleGacha gachaData, int gachaNumber)
  {
    Gacha0063Menu gacha0063Menu = this;
    if (!gacha0063Menu.IsPushAndSet())
    {
      Consts instance = Consts.GetInstance();
      int nWait = 0;
      PopupCommonNoYes.Show(instance.PICKUPSELECT_POPUP_PICKUPEMPTY_TITLE, instance.PICKUPSELECT_POPUP_PICKUPEMPTY_TEXT, (Action) (() => nWait = 1), (Action) (() => nWait = 2), (NGUIText.Alignment) 1);
      while (nWait == 0)
        yield return (object) null;
      if (nWait == 1)
      {
        Singleton<NGGameDataManager>.GetInstance().currentGachaNumber = gachaNumber;
        GachaPickupSelectScene.changeScene(true, gachaData, (Action) (() => this.forceApiUpdate()));
      }
      else
        gacha0063Menu.IsPush = false;
    }
  }

  public void forceApiUpdate()
  {
    if (!Object.op_Implicit((Object) this.scene))
      return;
    this.scene.apiUpdate = true;
  }

  private bool GachaCheckSufficiency(GachaModuleGacha gachaData)
  {
    Player player = SMManager.Get<Player>();
    string text1 = "";
    string text2 = "";
    string text3 = "";
    Gacha99931Menu.PaymentType paymentType = Gacha99931Menu.PaymentType.ALL;
    if (gachaData.payment_type_id == Gacha0063Menu.PaymentTypeIDCompensation)
    {
      if (player.CheckCompensationKiseki(gachaData.payment_amount))
        return true;
      text1 = Consts.GetInstance().GACHA_99931MENU_DESCRIPTION01_COMPENSATION;
      text2 = Consts.GetInstance().GACHA_99931MENU_DESCRIPTION02_COMPENSATION;
      text3 = Consts.GetInstance().GACHA_99931MENU_DESCRIPTION03_COMPENSATION;
      paymentType = Gacha99931Menu.PaymentType.Compensation;
    }
    else
    {
      if (gachaData.payment_type_id == 5)
      {
        if (gachaData.payment_id.HasValue)
        {
          int id = gachaData.payment_id.Value;
          PlayerGachaTicket playerGachaTicket = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).Where<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket.ID == id)).FirstOrDefault<PlayerGachaTicket>();
          int quantity = playerGachaTicket == null ? 0 : playerGachaTicket.quantity;
          if (gachaData.payment_amount <= quantity)
            return true;
        }
        this.StartCoroutine(Gacha0063Menu.openPopupGachaTicketShortage());
        return false;
      }
      if (player.CheckKiseki(gachaData.payment_amount))
        return true;
    }
    this.StartCoroutine(this.ChargeError(text1, text2, text3, paymentType));
    return false;
  }

  private IEnumerator ChargeError(
    string text1 = "",
    string text2 = "",
    string text3 = "",
    Gacha99931Menu.PaymentType paymentType = Gacha99931Menu.PaymentType.ALL)
  {
    if (!this.IsPushAndSet())
    {
      IEnumerator e = PopupUtility._999_3_1(text1, text2, text3, paymentType);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static IEnumerator openPopupGachaTicketShortage()
  {
    Future<GameObject> prefab = new ResourceObject("Prefabs/popup/popup_GachaTicket_Lack__anim_popup01").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab.Result);
  }

  private bool GachaCheckFriend(GachaModuleGacha gacha_data)
  {
    if (SMManager.Get<Player>().CheckFrendPoint(gacha_data.payment_amount))
      return true;
    this.StartCoroutine(this.PtError());
    return false;
  }

  private IEnumerator PtError()
  {
    if (Object.op_Equality((Object) this.gachaPtErrorPrefab, (Object) null))
    {
      Singleton<PopupManager>.GetInstance().open((GameObject) null);
      Future<GameObject> prefabF = Res.Prefabs.gacha999_4_1.popup_999_4_1__anim_popup01.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.gachaPtErrorPrefab = prefabF.Result;
      Singleton<PopupManager>.GetInstance().dismiss();
      prefabF = (Future<GameObject>) null;
    }
    Singleton<PopupManager>.GetInstance().open(this.gachaPtErrorPrefab).GetComponent<Gacha99941Menu>().SetText();
  }

  private bool GachaCheckUnit(GachaModuleGacha gachaData)
  {
    Player player = SMManager.Get<Player>();
    int num1 = SMManager.Get<PlayerUnitReservesCount>().count + gachaData.roll_count;
    int num2 = SMManager.Get<PlayerUnit[]>().Length + gachaData.roll_count;
    if (!player.CheckLimitOverExtMaxUnitReserves(num1) || !player.CheckLimitOverCapMaxUnit(num2))
      return true;
    this.StartCoroutine(PopupUtility._999_5_1());
    return false;
  }

  private bool TicketGachaCheckUnit()
  {
    if (!SMManager.Get<Player>().CheckCapMaxUnit())
      return true;
    this.StartCoroutine(PopupUtility._999_5_1());
    return false;
  }

  private bool ManaGachaCheckUnit()
  {
    if (!SMManager.Get<Player>().CheckMaxHavingUnit())
      return true;
    this.StartCoroutine(PopupUtility._999_5_1());
    return false;
  }

  private bool GachaCheckItem()
  {
    if (!SMManager.Get<Player>().CheckMaxHavingGear())
      return true;
    this.StartCoroutine(PopupUtility._999_6_1(true));
    return false;
  }

  private bool GachaCheckReisou()
  {
    if (!SMManager.Get<Player>().CheckMaxHavingReisou())
      return true;
    this.StartCoroutine(PopupUtility.popupMaxReisou());
    return false;
  }

  public IEnumerator CreatePrefab()
  {
    Future<GameObject> prefabF = Res.Prefabs.gacha006_5.popup_006_5__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.gachaChargePrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = new ResourceObject("Prefabs/popup/popup_006_slider_select_anim_popup01").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.gachaTicketSliderSelectPopupPrefab = prefabF.Result;
    prefabF = (Future<GameObject>) null;
    prefabF = Res.Prefabs.gacha006_3.dir_SheetGacha.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popupSheet = prefabF.Result;
    prefabF = (Future<GameObject>) null;
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public IEnumerator popup_Coin_Detail(GachaModuleGacha gachaData)
  {
    Future<GameObject> prefab = new ResourceObject("Prefabs/gacha006_3/popup_Coin_Detail").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<PopupCoinDetail>().Init(gachaData.common_ticket_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void IbtnBuyKiseki()
  {
    if (this.IsPushAndSet())
      return;
    PopupUtility.BuyKiseki();
  }

  public virtual void IbtnGachaCharge()
  {
  }

  public virtual void IbtnGachaPt01()
  {
  }

  public virtual void IbtnGachaPt02()
  {
  }

  public virtual void IbtnGachaTicket01()
  {
  }

  public virtual void IbtnGachaTicket02()
  {
  }

  public virtual void IbtnGetList01()
  {
  }

  public virtual void IbtnGetList02()
  {
  }

  public virtual void IbtnUnitlist()
  {
  }
}
