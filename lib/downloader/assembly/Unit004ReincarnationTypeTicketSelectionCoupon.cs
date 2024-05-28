// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeTicketSelectionCoupon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit004ReincarnationTypeTicketSelectionCoupon : MonoBehaviour
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
  private Unit004ReincarnationTypeTicketSelectionMenu menu_;
  private PlayerUnitTypeTicket coupon_;
  private UnitTypeTicket ticket_;

  public IEnumerator coInitialize(
    Unit004ReincarnationTypeTicketSelectionMenu menu,
    PlayerUnitTypeTicket coupon)
  {
    this.menu_ = menu;
    this.coupon_ = coupon;
    this.ticket_ = MasterData.UnitTypeTicket[coupon.ticket_id];
    Future<GameObject> ldicon = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    yield return (object) ldicon.Wait();
    GameObject result = ldicon.Result;
    if (Object.op_Inequality((Object) result, (Object) null))
    {
      UniqueIcons ticketIcon = result.Clone(this.linkThum_.transform).GetComponent<UniqueIcons>();
      yield return (object) ticketIcon.SetReincarnationTypeTicket(this.ticket_.ID);
      ticketIcon.LabelActivated = false;
      ticketIcon.BackGroundActivated = false;
      ticketIcon = (UniqueIcons) null;
    }
    this.txtTitle_.SetTextLocalize(this.ticket_.name);
    this.txtLimitDate_.SetTextLocalize(string.Format(Consts.GetInstance().UNIT_004_REINCARNATION_TYPE_TICKET_END_AT, (object) this.ticket_.end_at));
    this.txtNeed_.SetTextLocalize(this.ticket_.cost);
    if (coupon.quantity >= this.ticket_.cost)
    {
      this.txtProgress_.SetTextLocalize(coupon.quantity);
      ((Component) this.txtProgress_).gameObject.SetActive(true);
      ((Component) this.txtProgressRed_).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.txtProgress_).gameObject.SetActive(false);
      this.txtProgressRed_.SetTextLocalize(coupon.quantity);
      ((Component) this.txtProgressRed_).gameObject.SetActive(true);
    }
  }

  public void onClickedSelect() => this.menu_.selectedCoupon(this.ticket_);

  public void onClickedDescription() => this.menu_.onClickedDescription(this.ticket_);
}
