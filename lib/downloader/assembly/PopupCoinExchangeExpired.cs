// Decompiled with JetBrains decompiler
// Type: PopupCoinExchangeExpired
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class PopupCoinExchangeExpired : MonoBehaviour
{
  [SerializeField]
  private UI2DSprite dynCoinIcon;
  [SerializeField]
  private UILabel txtDescription1;
  [SerializeField]
  private GameObject txtDescription2;
  [SerializeField]
  private UILabel txtCoinNum;
  [SerializeField]
  private UILabel txtHimeNum;

  public IEnumerator Init(int ticket_id, int ticket_entity, int kiseki_entity)
  {
    if (!MasterData.CommonTicket.ContainsKey(ticket_id) || !MasterData.CommonTicketEndAt.ContainsKey(ticket_id))
    {
      Debug.LogError((object) "id:{0}のcommon_ticketが存在しないか、期限が設定されていません。".F((object) ticket_id));
    }
    else
    {
      CommonTicket ticketMaster = MasterData.CommonTicket[ticket_id];
      Future<Sprite> future = ticketMaster.LoadIconSSpriteF();
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dynCoinIcon.sprite2D = future.Result;
      DateTime endAt = MasterData.CommonTicketEndAt[ticket_id].end_at;
      if (kiseki_entity >= 1)
      {
        this.txtDescription1.SetTextLocalize(Consts.GetInstance().SHOP_POPUP_COIN_EXPIRED_DESCRIPTION.F((object) endAt.Year, (object) endAt.Month, (object) endAt.Day, (object) ticketMaster.name));
        this.txtDescription2.SetActive(true);
      }
      else
      {
        this.txtDescription1.SetTextLocalize(Consts.GetInstance().SHOP_POPUP_COIN_EXPIRED_DESCRIPTION_KISEKI_NONE.F((object) ticketMaster.name, (object) ticket_entity));
        this.txtDescription2.SetActive(false);
      }
      this.txtCoinNum.SetTextLocalize(Consts.GetInstance().SHOP_POPUP_COIN_EXPIRED_COIN_NUM.F((object) ticket_entity));
      this.txtHimeNum.SetTextLocalize(kiseki_entity);
    }
  }

  public void IbtnOK() => Singleton<PopupManager>.GetInstance().onDismiss();
}
