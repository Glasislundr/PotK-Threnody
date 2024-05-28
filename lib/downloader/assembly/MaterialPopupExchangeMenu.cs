// Decompiled with JetBrains decompiler
// Type: MaterialPopupExchangeMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class MaterialPopupExchangeMenu : BackButtonMenuBase
{
  public GameObject topIconUnit_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtDetail_;
  [SerializeField]
  private UIButton btnOk_;
  [SerializeField]
  private UIButton btnCancel_;
  [SerializeField]
  private UILabel txtTicketNum;
  [SerializeField]
  private PopupSelectSliderController sliderController;
  [SerializeField]
  private UILabel exchangeNum;
  [SerializeField]
  private GameObject exchangeNumL;
  private ShopMaterialExchangeListMenu menu_;
  private SelectTicketSelectSample sample_;
  private bool isInitialized_;
  private bool isOpenPopup_;
  private bool isOK_;
  private List<UIButton> pauseButtons_ = new List<UIButton>();
  private int nums;
  private int limitCount;
  private int useCount = 1;
  private SM.SelectTicket unitTicket_;
  private SelectTicketChoices choice;

  public IEnumerator coInitialize(
    ShopMaterialExchangeListMenu menu,
    SelectTicketSelectSample unitSample,
    PlayerSelectTicketSummary playerUnitTicket,
    SM.SelectTicket unitTicket)
  {
    MaterialPopupExchangeMenu popupExchangeMenu = this;
    popupExchangeMenu.isInitialized_ = false;
    popupExchangeMenu.menu_ = menu;
    popupExchangeMenu.sample_ = unitSample;
    popupExchangeMenu.unitTicket_ = unitTicket;
    IEnumerator coroutine = popupExchangeMenu.menu_.SetIcon(popupExchangeMenu.sample_, popupExchangeMenu.topIconUnit_.transform, true);
    yield return (object) popupExchangeMenu.StartCoroutine(coroutine);
    popupExchangeMenu.nums = (int) coroutine.Current;
    popupExchangeMenu.txtName_.SetTextLocalize(popupExchangeMenu.sample_.reward_title);
    popupExchangeMenu.isOK_ = playerUnitTicket.quantity >= unitTicket.cost;
    popupExchangeMenu.btnCancel_.onClick.Clear();
    popupExchangeMenu.btnCancel_.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.onBackButton())));
    ((UIButtonColor) popupExchangeMenu.btnOk_).isEnabled = popupExchangeMenu.isOK_;
    int ticketMax = Mathf.Min(playerUnitTicket.quantity / unitTicket.cost, popupExchangeMenu.menu_.QUANTITY_DISPLAY_MAX);
    Action action = (Action) (() =>
    {
      ((Component) this.exchangeNum).gameObject.SetActive(false);
      this.exchangeNumL.SetActive(false);
      this.InitSliderController(1, 1, ticketMax);
    });
    if (unitTicket.exchange_limit)
    {
      popupExchangeMenu.choice = ((IEnumerable<SelectTicketChoices>) unitTicket.choices).FirstOrDefault<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (u => u.reward_id == this.sample_.reward_id));
      if (popupExchangeMenu.choice.exchangeable_count.HasValue)
      {
        if (popupExchangeMenu.choice != null)
          popupExchangeMenu.limitCount = popupExchangeMenu.choice.exchangeable_count.Value;
        PlayerSelectTicketSummaryPlayer_exchange_count_list exchangeCountList = ((IEnumerable<PlayerSelectTicketSummaryPlayer_exchange_count_list>) playerUnitTicket.player_exchange_count_list).FirstOrDefault<PlayerSelectTicketSummaryPlayer_exchange_count_list>((Func<PlayerSelectTicketSummaryPlayer_exchange_count_list, bool>) (u => u.reward_id == this.sample_.reward_id));
        if (exchangeCountList != null)
          popupExchangeMenu.limitCount = popupExchangeMenu.choice.exchangeable_count.Value - exchangeCountList.exchange_count;
        popupExchangeMenu.InitSliderController(1, 1, Mathf.Min(popupExchangeMenu.limitCount, ticketMax));
      }
      else
        action();
    }
    else
      action();
    popupExchangeMenu.isInitialized_ = true;
  }

  private void OnDestroy() => this.StopCoroutine("waitPopupClose");

  public override void onBackButton() => this.onClickCancel();

  private void InitSliderController(int costTicket, int minTicket, int maxTicket)
  {
    if (costTicket < maxTicket)
    {
      this.sliderController.Initialize((float) minTicket, (float) maxTicket, (float) costTicket, (PopupSelectSliderController.SliderValueChangeListener) (val =>
      {
        this.useCount = Mathf.Max(1, Mathf.RoundToInt(val));
        this.OnSliderValueChange(Mathf.RoundToInt((float) (this.useCount * this.unitTicket_.cost)), this.useCount);
      }));
    }
    else
    {
      this.sliderController.Initialize(0.0f, 1f, 1f);
      this.sliderController.LockSlider();
    }
    this.OnSliderValueChange(this.unitTicket_.cost, 1);
  }

  private void OnSliderValueChange(int value1, int value2)
  {
    if (this.choice != null)
      this.exchangeNum.SetTextLocalize(this.limitCount.ToString() + "→[FFFF00]" + (object) (this.limitCount - value2) + "[-]");
    this.txtTicketNum.SetTextLocalize(this.menu_.quantity_.ToString() + "→[FFFF00]" + (object) (this.menu_.quantity_ - value1) + "[-]");
    this.txtDetail_.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_POPUP_MATERIAL_EXCHANGE_MULTI_DETAIL, (object) value1, (object) value2));
  }

  public void onClickOk()
  {
    if (this.IsPushAndSet())
      return;
    this.menu_.InitObj(((Component) this.topIconUnit_.transform.GetChild(0)).gameObject);
    Singleton<PopupManager>.GetInstance().dismiss();
    this.menu_.doExchangeMaterial(this.nums, this.useCount);
  }

  public void onClickCancel()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
