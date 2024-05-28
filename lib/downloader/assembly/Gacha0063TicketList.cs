// Decompiled with JetBrains decompiler
// Type: Gacha0063TicketList
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
public class Gacha0063TicketList : Gacha0063hindicator
{
  [SerializeField]
  private UIGrid mScrollGrid;
  [SerializeField]
  private NGOverlapScrollView mOverlapScrollView;
  [SerializeField]
  private UIPanel mScrollViewPanel;
  [SerializeField]
  private int topOffest;
  [SerializeField]
  private int bottomOffest;
  [SerializeField]
  private NGxScroll UIScroll;
  private List<GachaModuleGacha> mDispTicketGachaList;
  private Gacha0063Menu menu;
  private GameObject listItem;
  private GachaModule tmpGachaModule;
  private List<Gacha0063TicketItem> setupList = new List<Gacha0063TicketItem>();
  private GameObject detailPopupObj;

  public override void InitGachaModuleGacha(
    Gacha0063Menu gacha0063Menu,
    GachaModule gachaModule,
    DateTime serverTime,
    UIScrollView scrollView,
    int prefabCount)
  {
    Dictionary<int, PlayerGachaTicket> ticketDict = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).ToDictionary<PlayerGachaTicket, int>((Func<PlayerGachaTicket, int>) (x => x.ticket_id));
    Transform transform = ((Component) ((Component) gacha0063Menu).transform.Find("MainPanel")).transform;
    this.menu = gacha0063Menu;
    this.GachaModule = gachaModule;
    this.tmpGachaModule = gachaModule;
    this.PrefabCount = prefabCount;
    ((UIRect) this.mScrollViewPanel).topAnchor.target = transform;
    ((UIRect) this.mScrollViewPanel).topAnchor.absolute = this.topOffest;
    ((UIRect) this.mScrollViewPanel).topAnchor.relative = 0.5f;
    ((UIRect) this.mScrollViewPanel).bottomAnchor.target = transform;
    ((UIRect) this.mScrollViewPanel).bottomAnchor.absolute = this.bottomOffest;
    this.mDispTicketGachaList = ((IEnumerable<GachaModuleGacha>) gachaModule.gacha).Where<GachaModuleGacha>((Func<GachaModuleGacha, bool>) (x => x.payment_id.HasValue && ticketDict.ContainsKey(x.payment_id.Value) && ticketDict[x.payment_id.Value].quantity > 0)).OrderBy<GachaModuleGacha, int>((Func<GachaModuleGacha, int>) (x => ticketDict[x.payment_id.Value].quantity < x.payment_amount ? 1 : 0)).ThenBy<GachaModuleGacha, int>((Func<GachaModuleGacha, int>) (x => ticketDict[x.payment_id.Value].ticket.priority)).ThenBy<GachaModuleGacha, DateTime>((Func<GachaModuleGacha, DateTime>) (x => !x.end_at.HasValue ? DateTime.MaxValue : x.end_at.Value)).ToList<GachaModuleGacha>();
    this.mOverlapScrollView.SetOverlapScrollView(scrollView);
  }

  public override IEnumerator Set(GameObject detailPopup)
  {
    Gacha0063TicketList gacha0063TicketList = this;
    IEnumerator e;
    if (Object.op_Equality((Object) gacha0063TicketList.listItem, (Object) null))
    {
      Future<GameObject> listItemF = Res.Prefabs.gacha006_3.slc_Gacha_list_Item.Load<GameObject>();
      e = listItemF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gacha0063TicketList.listItem = listItemF.Result;
      listItemF = (Future<GameObject>) null;
    }
    if (!Object.op_Equality((Object) gacha0063TicketList.listItem, (Object) null))
    {
      gacha0063TicketList.detailPopupObj = detailPopup;
      ((Component) gacha0063TicketList.mScrollGrid).transform.Clear();
      gacha0063TicketList.setupList.Clear();
      gacha0063TicketList.setupList = new List<Gacha0063TicketItem>();
      foreach (GachaModuleGacha mDispTicketGacha in gacha0063TicketList.mDispTicketGachaList)
      {
        Gacha0063TicketItem component = gacha0063TicketList.listItem.CloneAndGetComponent<Gacha0063TicketItem>(((Component) gacha0063TicketList.mScrollGrid).gameObject);
        component.Init(gacha0063TicketList.tmpGachaModule, mDispTicketGacha, gacha0063TicketList.menu);
        gacha0063TicketList.setupList.Add(component);
      }
      gacha0063TicketList.mOverlapScrollView.ResetPosition();
      gacha0063TicketList.mScrollGrid.repositionNow = true;
      NGxScroll componentInChildren = ((Component) ((Component) gacha0063TicketList).transform).GetComponentInChildren<NGxScroll>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null))
        componentInChildren.ResolvePosition(Vector2.zero);
      BoxCollider[] components = ((Component) gacha0063TicketList.mScrollGrid).GetComponents<BoxCollider>();
      components[0].center = new Vector3(0.0f, components[0].size.y, 0.0f);
      components[1].center = new Vector3(0.0f, (float) -gacha0063TicketList.mDispTicketGachaList.Count * gacha0063TicketList.mScrollGrid.cellHeight, 0.0f);
      if (PerformanceConfig.GetInstance().IsTuningGachaInitialize)
      {
        gacha0063TicketList.StartCoroutine(gacha0063TicketList.MakeList(gacha0063TicketList.detailPopupObj, gacha0063TicketList.setupList));
      }
      else
      {
        foreach (Gacha0063TicketItem setup in gacha0063TicketList.setupList)
        {
          e = setup.LoadResource(gacha0063TicketList.detailPopupObj);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
  }

  private IEnumerator MakeList(GameObject detailPopup, List<Gacha0063TicketItem> setupList)
  {
    foreach (Gacha0063TicketItem setup in setupList)
    {
      IEnumerator e = setup.LoadResource(detailPopup);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      yield return (object) null;
    }
  }
}
