// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeTicketSelectionMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit004ReincarnationTypeTicketSelectionMenu : BackButtonMenuBase
{
  [SerializeField]
  private Unit004ReincarnationTypeTicketSelectionScene scene_;
  [SerializeField]
  private UIScrollView scroll_;
  [SerializeField]
  private UIGrid grid_;
  [SerializeField]
  private GameObject ticketsList_;
  [SerializeField]
  private GameObject noTicketTxt_;

  public IEnumerator coInitialize(List<PlayerUnitTypeTicket> coupons)
  {
    Unit004ReincarnationTypeTicketSelectionMenu menu = this;
    if (coupons.Count == 0)
    {
      menu.noTicketTxt_.SetActive(true);
      menu.ticketsList_.SetActive(false);
    }
    else
    {
      menu.noTicketTxt_.SetActive(false);
      menu.ticketsList_.SetActive(true);
      Future<GameObject> ldPrefab = new ResourceObject("Prefabs/unit004_Reincarnation_Type/slc_Ticket_list_Item_Reincarnation_Type").Load<GameObject>();
      yield return (object) ldPrefab.Wait();
      GameObject prefabCoupon = ldPrefab.Result;
      if (!Object.op_Equality((Object) prefabCoupon, (Object) null))
      {
        ldPrefab = (Future<GameObject>) null;
        foreach (PlayerUnitTypeTicket coupon in coupons)
          yield return (object) prefabCoupon.Clone(((Component) menu.grid_).transform).GetComponent<Unit004ReincarnationTypeTicketSelectionCoupon>().coInitialize(menu, coupon);
        menu.grid_.Reposition();
        menu.scroll_.ResetPosition();
      }
    }
  }

  public void onEndMenu()
  {
    foreach (Component child in ((Component) this.grid_).transform.GetChildren())
      Object.Destroy((Object) child.gameObject);
  }

  public void selectedCoupon(UnitTypeTicket ticket)
  {
    if (this.IsPushAndSet())
      return;
    Unit004ReincarnationTypeUnitSelectionScene.changeScene(true, ticket);
  }

  public void onClickedDescription(UnitTypeTicket ticket)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine("coPopupDescription", (object) ticket);
  }

  private IEnumerator coPopupDescription(UnitTypeTicket ticket)
  {
    Future<GameObject> ldprefab = new ResourceObject("Prefabs/popup/popup_004_reincarnation_type_ticket_details__anim_popup01").Load<GameObject>();
    yield return (object) ldprefab.Wait();
    GameObject result = ldprefab.Result;
    Singleton<PopupManager>.GetInstance().open(result).GetComponent<Unit004ReincarnationTypeTicketSelectionDescription>().initialize(ticket);
  }

  public override void onBackButton() => this.OnIbtnBack();

  public void OnIbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}
