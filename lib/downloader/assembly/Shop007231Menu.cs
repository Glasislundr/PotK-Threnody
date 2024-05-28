// Decompiled with JetBrains decompiler
// Type: Shop007231Menu
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
public class Shop007231Menu : BackButtonMenuBase
{
  [SerializeField]
  private Shop007231Scene scene_;
  [SerializeField]
  private UIScrollView scroll_;
  [SerializeField]
  private UIGrid grid_;
  [SerializeField]
  private GameObject ticketsList_;
  [SerializeField]
  private GameObject noTicketTxt_;
  private float slideValue;

  public IEnumerator coInitialize(
    IEnumerable<SelectTicket> unitTickets,
    List<PlayerSelectTicketSummary> playerUnitTickets)
  {
    if (playerUnitTickets.Count == 0)
    {
      this.noTicketTxt_.SetActive(true);
      this.ticketsList_.SetActive(false);
    }
    else
    {
      this.noTicketTxt_.SetActive(false);
      this.ticketsList_.SetActive(true);
      Future<GameObject> ldPrefab = Res.Prefabs.shop007_23_1.slc_Ticket_list_Item.Load<GameObject>();
      IEnumerator e = ldPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = ldPrefab.Result;
      if (!Object.op_Equality((Object) result, (Object) null))
      {
        ldPrefab = (Future<GameObject>) null;
        foreach (SelectTicket unitTicket1 in unitTickets)
        {
          SelectTicket unitTicket = unitTicket1;
          playerUnitTickets.FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == unitTicket.id));
          result.Clone(((Component) this.grid_).transform);
        }
        this.grid_.Reposition();
        this.scroll_.ResetPosition();
        if ((double) this.slideValue > 0.0)
        {
          this.scroll_.verticalScrollBar.value = this.slideValue;
          this.slideValue = 0.0f;
        }
      }
    }
  }

  public void onEndMenu()
  {
    foreach (Component child in ((Component) this.grid_).transform.GetChildren())
      Object.Destroy((Object) child.gameObject);
  }

  public void selectedCoupon(PlayerSelectTicketSummary playerUnitTicket)
  {
    if (this.IsPushAndSet())
      return;
    this.slideValue = this.scroll_.verticalScrollBar.value;
    Shop00723Scene.changeScene(playerUnitTicket);
  }

  public void onClickedDescription(SelectTicket ticket)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine("coPopupDescription", (object) ticket);
  }

  private IEnumerator coPopupDescription(SelectTicket ticket)
  {
    Future<GameObject> ldprefab = Res.Prefabs.popup.popup_007_23_1__anim_popup01.Load<GameObject>();
    IEnumerator e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = ldprefab.Result;
    Singleton<PopupManager>.GetInstance().open(result).GetComponent<Shop007231Description>().initialize(ticket);
  }

  public override void onBackButton() => this.OnIbtnBack();

  public void OnIbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
