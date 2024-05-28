// Decompiled with JetBrains decompiler
// Type: PopupCoinDetail
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
public class PopupCoinDetail : BackButtonMenuBase
{
  [SerializeField]
  private UI2DSprite dynIcon;
  [SerializeField]
  private UILabel txtCoinName;
  [SerializeField]
  private UILabel txtCoinExplanation;
  [SerializeField]
  private UILabel txtProssessionValue;
  [SerializeField]
  private UIButton ibtnYes;
  [SerializeField]
  private UIButton ibtnNo;
  private int ticket_id;
  private int ticket_quantity;

  public IEnumerator Init(int? common_ticket_id)
  {
    PopupCoinDetail popupCoinDetail = this;
    popupCoinDetail.ticket_id = !common_ticket_id.HasValue ? 0 : common_ticket_id.Value;
    if (!MasterData.CommonTicket.ContainsKey(popupCoinDetail.ticket_id))
    {
      Debug.LogError((object) "id:{0}のcommon_ticketが存在しません。".F((object) popupCoinDetail.ticket_id));
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      PlayerCommonTicket playerCommonTicket = ((IEnumerable<PlayerCommonTicket>) SMManager.Get<PlayerCommonTicket[]>()).FirstOrDefault<PlayerCommonTicket>(new Func<PlayerCommonTicket, bool>(popupCoinDetail.\u003CInit\u003Eb__8_0));
      int quantity = playerCommonTicket == null ? 0 : playerCommonTicket.quantity;
      popupCoinDetail.ticket_quantity = NC.Clamp(0, Consts.GetInstance().SUBCOIN_DISP_MAX, quantity);
      CommonTicket ticketMaster = MasterData.CommonTicket[popupCoinDetail.ticket_id];
      Future<Sprite> future = ticketMaster.LoadIconMSpriteF();
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popupCoinDetail.dynIcon.sprite2D = future.Result;
      popupCoinDetail.txtCoinName.SetTextLocalize(ticketMaster.name);
      popupCoinDetail.txtCoinExplanation.SetTextLocalize(ticketMaster.description);
      popupCoinDetail.txtProssessionValue.SetTextLocalize(popupCoinDetail.ticket_quantity);
    }
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void onCoinExchangeButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.StartCoroutine(this.ChangeCoinShop());
  }

  public IEnumerator ChangeCoinShop()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (!WebAPI.IsResponsedAtRecent("ShopStatus"))
    {
      Future<WebAPI.Response.ShopStatus> shoplistF = WebAPI.ShopStatus((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
      })).Then<WebAPI.Response.ShopStatus>((Func<WebAPI.Response.ShopStatus, WebAPI.Response.ShopStatus>) (result =>
      {
        Singleton<NGGameDataManager>.GetInstance().Parse(result);
        return result;
      }));
      IEnumerator e1 = shoplistF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (shoplistF.Result == null)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        yield break;
      }
      else
        shoplistF = (Future<WebAPI.Response.ShopStatus>) null;
    }
    ShopCoinExchangeScene.changeScene(this.ticket_id);
  }
}
