// Decompiled with JetBrains decompiler
// Type: Shop007231Coupon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop007231Coupon : MonoBehaviour
{
  [SerializeField]
  private UILabel txtTitle_;
  [SerializeField]
  private GameObject linkThum_;
  [SerializeField]
  private UILabel txtLimitDate_;
  [SerializeField]
  private UILabel txtNeed_;
  [SerializeField]
  private UILabel txtProgress_;
  [SerializeField]
  private UILabel txtProgressRed_;
  [SerializeField]
  private UIButton btnToShop_;
  private PlayerSelectTicketSummary playerUnitTicket_;
  private SelectTicket unitTicket_;
  private TicketType ticketType;

  public IEnumerator coInitializeMaterial(
    PlayerSelectTicketSummary playerUnitTicket,
    SelectTicket unitTicket)
  {
    Future<GameObject> ldicon = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = ldicon.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons ticketicon = ldicon.Result.Clone(this.linkThum_.transform).GetComponent<UniqueIcons>();
    if (unitTicket.category_id == 1)
    {
      e = ticketicon.SetKillersTicket(unitTicket.id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (unitTicket.category_id == 2)
    {
      e = ticketicon.SetMaterialTicket(unitTicket.id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    ticketicon.LabelActivated = false;
    ticketicon.BackGroundActivated = false;
    this.ticketType = TicketType.Material;
    this.playerUnitTicket_ = playerUnitTicket;
    this.unitTicket_ = unitTicket;
    this.txtTitle_.SetTextLocalize(unitTicket.name);
    this.txtLimitDate_.SetTextLocalize(unitTicket.end_at.HasValue ? string.Format(Consts.GetInstance().SHOP_00723_EXPIRATION_DATE, (object) unitTicket.end_at) : Consts.GetInstance().SHOP_00723_EXPIRATION_DATE_NONE);
    this.txtNeed_.SetTextLocalize(unitTicket.cost);
    if (playerUnitTicket.quantity >= unitTicket.cost)
    {
      this.txtProgress_.SetTextLocalize(playerUnitTicket.quantity);
      ((Component) this.txtProgress_).gameObject.SetActive(true);
      ((Component) this.txtProgressRed_).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.txtProgress_).gameObject.SetActive(false);
      this.txtProgressRed_.SetTextLocalize(playerUnitTicket.quantity);
      ((Component) this.txtProgressRed_).gameObject.SetActive(true);
    }
    ((UIButtonColor) this.btnToShop_).isEnabled = playerUnitTicket.quantity >= unitTicket.cost;
  }

  public IEnumerator coInitializeKillers(
    PlayerSelectTicketSummary playerUnitTicket,
    SelectTicket unitTicket)
  {
    Future<GameObject> ldicon = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = ldicon.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons ticketicon = ldicon.Result.Clone(this.linkThum_.transform).GetComponent<UniqueIcons>();
    e = ticketicon.SetKillersTicket(unitTicket.id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ticketicon.LabelActivated = false;
    ticketicon.BackGroundActivated = false;
    this.ticketType = TicketType.Killers;
    this.playerUnitTicket_ = playerUnitTicket;
    this.unitTicket_ = unitTicket;
    this.txtTitle_.SetTextLocalize(unitTicket.name);
    this.txtLimitDate_.SetTextLocalize(unitTicket.end_at.HasValue ? string.Format(Consts.GetInstance().SHOP_00723_EXPIRATION_DATE, (object) unitTicket.end_at) : Consts.GetInstance().SHOP_00723_EXPIRATION_DATE_NONE);
    this.txtNeed_.SetTextLocalize(unitTicket.cost);
    if (playerUnitTicket.quantity >= unitTicket.cost)
    {
      this.txtProgress_.SetTextLocalize(playerUnitTicket.quantity);
      ((Component) this.txtProgress_).gameObject.SetActive(true);
      ((Component) this.txtProgressRed_).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.txtProgress_).gameObject.SetActive(false);
      this.txtProgressRed_.SetTextLocalize(playerUnitTicket.quantity);
      ((Component) this.txtProgressRed_).gameObject.SetActive(true);
    }
    ((UIButtonColor) this.btnToShop_).isEnabled = playerUnitTicket.quantity >= unitTicket.cost;
  }

  public void onClickedSelect()
  {
    switch (this.ticketType)
    {
      case TicketType.Material:
        ShopMaterialExchangeListScene.changeScene(this.playerUnitTicket_);
        break;
      case TicketType.Killers:
        Shop00723Scene.changeScene(this.playerUnitTicket_);
        break;
    }
  }

  public void onClickedDescription() => this.StartCoroutine(this.coPopupDescription());

  private IEnumerator coPopupDescription()
  {
    Future<GameObject> ldprefab = Res.Prefabs.popup.popup_007_23_1__anim_popup01.Load<GameObject>();
    IEnumerator e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = ldprefab.Result;
    Singleton<PopupManager>.GetInstance().open(result).GetComponent<Shop007231Description>().initialize(this.unitTicket_);
  }
}
