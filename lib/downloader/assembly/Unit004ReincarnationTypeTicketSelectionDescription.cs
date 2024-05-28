// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeTicketSelectionDescription
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
[AddComponentMenu("Popup/Unit/ReincarnationTypeTicketSelectionDescription")]
public class Unit004ReincarnationTypeTicketSelectionDescription : BackButtonMenuBase
{
  [SerializeField]
  private UILabel txtDescription_;
  [SerializeField]
  private UIScrollView scroll_;
  private UnitTypeTicket ticket_;

  public void initialize(UnitTypeTicket ticket) => this.ticket_ = ticket;

  private void Start()
  {
    this.txtDescription_.SetTextLocalize(this.ticket_.description);
    this.scroll_.ResetPosition();
  }

  public override void onBackButton() => this.OnIbtnBack();

  public void OnIbtnBack() => Singleton<PopupManager>.GetInstance().onDismiss();
}
