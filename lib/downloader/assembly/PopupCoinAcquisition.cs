// Decompiled with JetBrains decompiler
// Type: PopupCoinAcquisition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupCoinAcquisition : BackButtonMenuBase
{
  [SerializeField]
  private UI2DSprite dynIcon;
  [SerializeField]
  private UILabel txtCoinName;
  [SerializeField]
  private UILabel txtCoinExplanation;
  [SerializeField]
  private UILabel txtAcquisitionValue;
  [SerializeField]
  private UIButton ibtnYes;
  [SerializeField]
  private UIButton ibtnNo;
  private int ticket_id;
  private int acquisitionValue;
  private string strCoinName = "{0}を獲得しました。";
  private string strAcquisitionValue = "[ffff00]{0}[ffffff]枚";
  private string strCoinExplanation = "[ffff00]※獲得したコインは直接付与されます";

  public IEnumerator Init(int common_ticket_id, int acquisition_quantity)
  {
    this.ticket_id = common_ticket_id;
    CommonTicket ticketMaster = MasterData.CommonTicket[this.ticket_id];
    Future<Sprite> future = ticketMaster.LoadIconMSpriteF();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dynIcon.sprite2D = future.Result;
    this.acquisitionValue = acquisition_quantity;
    this.txtCoinName.SetTextLocalize(string.Format(this.strCoinName, (object) ticketMaster.name));
    this.txtAcquisitionValue.SetTextLocalize(string.Format(this.strAcquisitionValue, (object) this.acquisitionValue));
    this.txtCoinExplanation.SetTextLocalize(this.strCoinExplanation);
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
