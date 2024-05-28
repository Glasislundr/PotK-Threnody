// Decompiled with JetBrains decompiler
// Type: Popup00635Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup00635Menu : BackButtonMenuBase
{
  private Action playGachaTicket;
  [SerializeField]
  private UILabel ticketName;
  [SerializeField]
  private UILabel description;
  [SerializeField]
  private UILabel ticketCount;
  [SerializeField]
  private UILabel m_Cost;

  public void Init(GachaModuleGacha gachaData, Action playGacha)
  {
    int key = gachaData.payment_id.Value;
    GachaTicket gachaTicket = MasterData.GachaTicket[key];
    Dictionary<int, PlayerGachaTicket> dictionary = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).ToDictionary<PlayerGachaTicket, int>((Func<PlayerGachaTicket, int>) (x => x.ticket_id));
    this.ticketName.SetTextLocalize(gachaTicket.name);
    this.SetDescription(Consts.Format(Consts.GetInstance().GACHA_00635TICKET_DESCRIPTION));
    this.ticketCount.SetTextLocalize(dictionary[key].quantity);
    this.playGachaTicket = playGacha;
    this.m_Cost.SetTextLocalize(gachaData.payment_amount);
  }

  public void SetDescription(string txt) => this.description.SetTextLocalize(txt);

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
