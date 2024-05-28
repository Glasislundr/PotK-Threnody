// Decompiled with JetBrains decompiler
// Type: Popup006SliderSelectMenu
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
public class Popup006SliderSelectMenu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel ticketName;
  [SerializeField]
  private UILabel description;
  [SerializeField]
  private UILabel reservedTicketCountLabel;
  [SerializeField]
  private UILabel totalTicketCostLabel;
  [SerializeField]
  private UILabel maxTicketCostLabel;
  [SerializeField]
  private UILabel totalPlayTimeLabel;
  [SerializeField]
  private PopupSelectSliderController sliderController;
  public int currentPlayTime;
  private int maxPlayableTime;
  private int minPlayableTime;
  private int paymentCount;
  private int selectedGachaTicketCount;
  private Action playGachaTicket;

  public void Init(GachaModuleGacha gachaData, Action playGacha)
  {
    int key = gachaData.payment_id.Value;
    GachaTicket gachaTicket = MasterData.GachaTicket[key];
    Dictionary<int, PlayerGachaTicket> dictionary = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).ToDictionary<PlayerGachaTicket, int>((Func<PlayerGachaTicket, int>) (x => x.ticket_id));
    this.paymentCount = gachaData.payment_amount;
    int rollCount = gachaData.roll_count;
    this.minPlayableTime = 1;
    this.currentPlayTime = this.minPlayableTime;
    this.maxPlayableTime = dictionary[key].quantity / this.paymentCount;
    this.selectedGachaTicketCount = this.paymentCount * this.currentPlayTime;
    int num1 = 10;
    if (gachaData.is_one_hundred_ream)
      num1 = 100;
    int num2 = num1 / rollCount;
    if (this.maxPlayableTime > num2)
      this.maxPlayableTime = num2;
    Consts instance = Consts.GetInstance();
    this.maxTicketCostLabel.SetTextLocalize(Consts.Format(instance.GACHA_00635TICKET_MAX_TICKET_COST, (IDictionary) new Hashtable()
    {
      {
        (object) "count",
        (object) (this.paymentCount * num2)
      }
    }));
    this.ticketName.SetTextLocalize(gachaTicket.name);
    this.description.SetTextLocalize(Consts.Format(instance.GACHA_00635TICKET_DESCRIPTION));
    this.reservedTicketCountLabel.SetTextLocalize(dictionary[key].quantity);
    this.totalTicketCostLabel.SetTextLocalize(this.selectedGachaTicketCount);
    this.totalPlayTimeLabel.SetTextLocalize(Consts.Format(instance.GACHA_00635TICKET_TOTAL_PLAY_TIME, (IDictionary) new Hashtable()
    {
      {
        (object) "count",
        (object) this.currentPlayTime
      }
    }));
    if (this.maxPlayableTime != this.minPlayableTime)
    {
      this.sliderController.Initialize((float) this.minPlayableTime, (float) this.maxPlayableTime, (float) this.currentPlayTime, new PopupSelectSliderController.SliderValueChangeListener(this.OnSliderValueChanged));
    }
    else
    {
      this.sliderController.Initialize(0.0f, (float) this.maxPlayableTime, (float) this.minPlayableTime);
      this.sliderController.LockSlider();
    }
    this.playGachaTicket = playGacha;
  }

  private void OnSliderValueChanged(float value)
  {
    this.currentPlayTime = Mathf.RoundToInt(value);
    this.totalPlayTimeLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().GACHA_00635TICKET_TOTAL_PLAY_TIME, (IDictionary) new Hashtable()
    {
      {
        (object) "count",
        (object) this.currentPlayTime
      }
    }));
    this.selectedGachaTicketCount = this.paymentCount * this.currentPlayTime;
    this.totalTicketCostLabel.SetTextLocalize(this.selectedGachaTicketCount);
  }

  public void IbtnPlayTicketGacha()
  {
    if (this.IsPushAndSet())
      return;
    this.playGachaTicket();
  }

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnBack();
}
