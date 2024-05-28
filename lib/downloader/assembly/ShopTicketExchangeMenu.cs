// Decompiled with JetBrains decompiler
// Type: ShopTicketExchangeMenu
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
public class ShopTicketExchangeMenu : BackButtonMenuBase
{
  [Header("チケット種別の切り替えボタン")]
  [SerializeField]
  private GameObject MaterialTicketBatch;
  [SerializeField]
  private UILabel MaterialTicketNum;
  [SerializeField]
  private UIButton MaterialTicketButton;
  [SerializeField]
  private UILabel MaterialTicketButtonLabel;
  [SerializeField]
  private GameObject KillersTicketBatch;
  [SerializeField]
  private UIButton KillersTicketButton;
  [SerializeField]
  private UILabel KillersTicketButtonLabel;
  [SerializeField]
  private UILabel KillersTicketNum;
  [Header("素材選択チケット表示部")]
  [SerializeField]
  private GameObject MaterialTicketContainer;
  [SerializeField]
  private UIScrollView MaterialTicketScrollView;
  [SerializeField]
  private UIGrid MaterialTicketGrid;
  [Header("キラーズチケット表示部")]
  [SerializeField]
  private GameObject KillersTicketContainer;
  [SerializeField]
  private UIScrollView KillersTicketScrollView;
  [SerializeField]
  private UIGrid KillersTicketGrid;
  [SerializeField]
  private GameObject NoTicketView;
  private List<PlayerSelectTicketSummary> materialTickets;
  private List<PlayerSelectTicketSummary> killersTickets;
  private bool isInitMaterialTicket;
  private bool isInitKillersTicket;
  private GameObject ItemPrefab;
  private TicketType currentTicketType;
  private bool isInit;
  public static bool IsUpdate;

  public IEnumerator Init()
  {
    if (ShopTicketExchangeMenu.IsUpdate)
    {
      this.isInit = false;
      ShopTicketExchangeMenu.IsUpdate = false;
    }
    if (!this.isInit)
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/shop007_23_1/slc_Ticket_list_Item").Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ItemPrefab = prefabF.Result;
      this.materialTickets = this.GetPlayerSelectTicketSummary(TicketType.Material);
      this.killersTickets = this.GetPlayerSelectTicketSummary(TicketType.Killers);
      SM.SelectTicket[] selectTicketArray = SMManager.Get<SM.SelectTicket[]>();
      int num1 = 0;
      foreach (SM.SelectTicket selectTicket in selectTicketArray)
      {
        SM.SelectTicket unitTicket = selectTicket;
        PlayerSelectTicketSummary selectTicketSummary = this.materialTickets.FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == unitTicket.id));
        if (selectTicketSummary != null && selectTicketSummary.quantity >= unitTicket.cost)
          ++num1;
      }
      if (num1 > 0)
      {
        this.MaterialTicketBatch.SetActive(true);
        this.MaterialTicketNum.text = num1.ToString();
      }
      else
        this.MaterialTicketBatch.SetActive(false);
      int num2 = 0;
      foreach (SM.SelectTicket selectTicket in selectTicketArray)
      {
        SM.SelectTicket unitTicket = selectTicket;
        PlayerSelectTicketSummary selectTicketSummary = this.killersTickets.FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == unitTicket.id));
        if (selectTicketSummary != null && selectTicketSummary.quantity >= unitTicket.cost)
          ++num2;
      }
      if (num2 > 0)
      {
        this.KillersTicketBatch.SetActive(true);
        this.KillersTicketNum.text = num2.ToString();
      }
      else
        this.KillersTicketBatch.SetActive(false);
      this.isInitMaterialTicket = false;
      this.isInitKillersTicket = false;
      if (this.currentTicketType == TicketType.Material)
        this.IbtnMaterialTicket();
      else if (this.currentTicketType == TicketType.Killers)
        this.IbtnKillersTicket();
      this.isInit = true;
    }
  }

  public void IbtnMaterialTicket()
  {
    this.currentTicketType = TicketType.Material;
    if (!this.isInitMaterialTicket)
      ((Component) this.MaterialTicketGrid).transform.Clear();
    ((UIButtonColor) this.MaterialTicketButton).isEnabled = false;
    ((UIButtonColor) this.KillersTicketButton).isEnabled = true;
    ((UIWidget) this.MaterialTicketButtonLabel).color = Color.white;
    ((UIWidget) this.KillersTicketButtonLabel).color = ShopCommon.TabDisableTextColor;
    this.MaterialTicketContainer.SetActive(true);
    this.KillersTicketContainer.SetActive(false);
    if (this.materialTickets.Count <= 0)
    {
      this.NoTicketView.SetActive(true);
    }
    else
    {
      this.NoTicketView.SetActive(false);
      if (!this.isInitMaterialTicket)
        this.StartCoroutine(this.ChangeCreateScrollItems(TicketType.Material));
      this.isInitMaterialTicket = true;
    }
  }

  public void IbtnKillersTicket()
  {
    this.currentTicketType = TicketType.Killers;
    if (!this.isInitKillersTicket)
      ((Component) this.KillersTicketGrid).transform.Clear();
    ((UIButtonColor) this.MaterialTicketButton).isEnabled = true;
    ((UIButtonColor) this.KillersTicketButton).isEnabled = false;
    ((UIWidget) this.MaterialTicketButtonLabel).color = ShopCommon.TabDisableTextColor;
    ((UIWidget) this.KillersTicketButtonLabel).color = Color.white;
    this.MaterialTicketContainer.SetActive(false);
    this.KillersTicketContainer.SetActive(true);
    if (this.killersTickets.Count <= 0)
    {
      this.NoTicketView.SetActive(true);
    }
    else
    {
      this.NoTicketView.SetActive(false);
      if (!this.isInitKillersTicket)
        this.StartCoroutine(this.ChangeCreateScrollItems(TicketType.Killers));
      this.isInitKillersTicket = true;
    }
  }

  private List<PlayerSelectTicketSummary> GetPlayerSelectTicketSummary(TicketType ticketType)
  {
    List<PlayerSelectTicketSummary> selectTicketSummary1 = new List<PlayerSelectTicketSummary>();
    PlayerSelectTicketSummary[] selectTicketSummaryArray = SMManager.Get<PlayerSelectTicketSummary[]>();
    if (selectTicketSummaryArray == null || selectTicketSummaryArray.Length == 0)
      return selectTicketSummary1;
    foreach (PlayerSelectTicketSummary selectTicketSummary2 in selectTicketSummaryArray)
    {
      PlayerSelectTicketSummary playerSelectTicket = selectTicketSummary2;
      if (playerSelectTicket != null && playerSelectTicket.quantity > 0)
      {
        MasterDataTable.SelectTicket selectTicket = ((IEnumerable<MasterDataTable.SelectTicket>) MasterData.SelectTicketList).First<MasterDataTable.SelectTicket>((Func<MasterDataTable.SelectTicket, bool>) (x => x.ID == playerSelectTicket.ticket_id));
        switch (ticketType)
        {
          case TicketType.Material:
            if (selectTicket.category == SelectTicketCategory.Item)
            {
              selectTicketSummary1.Add(playerSelectTicket);
              continue;
            }
            continue;
          case TicketType.Killers:
            if (selectTicket.category == SelectTicketCategory.Unit)
            {
              selectTicketSummary1.Add(playerSelectTicket);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    return selectTicketSummary1;
  }

  private IEnumerator ChangeCreateScrollItems(TicketType ticketType)
  {
    List<PlayerSelectTicketSummary> playerSelectTickets = new List<PlayerSelectTicketSummary>();
    switch (ticketType)
    {
      case TicketType.Material:
        playerSelectTickets = this.materialTickets;
        break;
      case TicketType.Killers:
        playerSelectTickets = this.killersTickets;
        break;
      default:
        Debug.LogError((object) ("想定していないTicketTypeです " + ticketType.ToString()));
        break;
    }
    if (playerSelectTickets.Count > 0)
    {
      UIScrollView scrollView = (UIScrollView) null;
      UIGrid grid = (UIGrid) null;
      switch (ticketType)
      {
        case TicketType.Material:
          scrollView = this.MaterialTicketScrollView;
          grid = this.MaterialTicketGrid;
          break;
        case TicketType.Killers:
          scrollView = this.KillersTicketScrollView;
          grid = this.KillersTicketGrid;
          break;
      }
      SM.SelectTicket[] source = SMManager.Get<SM.SelectTicket[]>();
      int[] ticketIds = playerSelectTickets.Select<PlayerSelectTicketSummary, int>((Func<PlayerSelectTicketSummary, int>) (pt => pt.ticket_id)).ToArray<int>();
      Func<SM.SelectTicket, bool> predicate = (Func<SM.SelectTicket, bool>) (t => ((IEnumerable<int>) ticketIds).Contains<int>(t.id));
      IOrderedEnumerable<SM.SelectTicket> orderedEnumerable = ((IEnumerable<SM.SelectTicket>) source).Where<SM.SelectTicket>(predicate).OrderBy<SM.SelectTicket, DateTime>((Func<SM.SelectTicket, DateTime>) (x => !x.end_at.HasValue ? DateTime.MaxValue : x.end_at.Value)).ThenBy<SM.SelectTicket, int>((Func<SM.SelectTicket, int>) (s => this.GetTicketsPriority(playerSelectTickets.FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == s.id)))));
      ((Component) grid).gameObject.SetActive(false);
      foreach (SM.SelectTicket selectTicket in (IEnumerable<SM.SelectTicket>) orderedEnumerable)
      {
        SM.SelectTicket unitTicket = selectTicket;
        PlayerSelectTicketSummary playerUnitTicket = playerSelectTickets.FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == unitTicket.id));
        GameObject gameObject = this.ItemPrefab.Clone(((Component) grid).transform);
        IEnumerator e;
        switch (ticketType)
        {
          case TicketType.Material:
            e = gameObject.GetComponent<Shop007231Coupon>().coInitializeMaterial(playerUnitTicket, unitTicket);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            continue;
          case TicketType.Killers:
            e = gameObject.GetComponent<Shop007231Coupon>().coInitializeKillers(playerUnitTicket, unitTicket);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            continue;
          default:
            continue;
        }
      }
      yield return (object) null;
      ((Component) grid).gameObject.SetActive(true);
      grid.Reposition();
      scrollView.ResetPosition();
    }
  }

  private int GetTicketsPriority(PlayerSelectTicketSummary playerUnitTicket)
  {
    return ((IEnumerable<MasterDataTable.SelectTicket>) MasterData.SelectTicketList).First<MasterDataTable.SelectTicket>((Func<MasterDataTable.SelectTicket, bool>) (x => x.ID == playerUnitTicket.ticket_id)).priority;
  }

  public void selectedCoupon(PlayerSelectTicketSummary playerUnitTicket)
  {
    if (this.IsPushAndSet())
      return;
    Shop00723Scene.changeScene(playerUnitTicket);
  }

  public void onClickedDescription(SM.SelectTicket ticket)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine("coPopupDescription", (object) ticket);
  }

  public override void onBackButton() => this.OnIbtnBack();

  private void OnIbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onStartScene() => ((Behaviour) this).enabled = true;

  public void onEndScene() => ((Behaviour) this).enabled = false;
}
