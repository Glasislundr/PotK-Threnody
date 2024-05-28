// Decompiled with JetBrains decompiler
// Type: Popup02687Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup02687Menu : Popup02610Base
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private UILabel txtTopDescription;
  [SerializeField]
  private UILabel txtSeasonTicket;
  [SerializeField]
  private UILabel txtSeasonTicketNum;
  [SerializeField]
  private UILabel txtBotDescription;
  private bool isTicket;
  private PlayerSeasonTicket ticket;

  public override IEnumerator InitCoroutine(WebAPI.Response.PvpBoot pvpInfo, Versus02610Menu menu)
  {
    Popup02687Menu popup02687Menu = this;
    ((UIRect) ((Component) popup02687Menu).GetComponent<UIWidget>()).alpha = 0.0f;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e1 = popup02687Menu.\u003C\u003En__0(pvpInfo, menu);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    popup02687Menu.pvpInfo = pvpInfo;
    int cnt = pvpInfo.remaining_addition_matches;
    int max = pvpInfo.max_addition_matches;
    Future<WebAPI.Response.SeasonticketIndex> futureF = WebAPI.SeasonticketIndex((Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<PopupManager>.GetInstance().onDismiss();
      WebAPI.DefaultUserErrorCallback(e);
    }));
    e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureF.Result != null)
    {
      popup02687Menu.ticket = (PlayerSeasonTicket) null;
      int key = 1;
      if (futureF.Result.player_season_tickets.Length != 0)
      {
        popup02687Menu.ticket = futureF.Result.player_season_tickets[0];
        key = popup02687Menu.ticket.season_ticket_id;
      }
      int quantity = popup02687Menu.ticket == null ? 0 : popup02687Menu.ticket.quantity;
      popup02687Menu.isTicket = quantity > 0;
      Consts instance = Consts.GetInstance();
      popup02687Menu.txtTitle.SetText(instance.VERSUS_002687POPUP_TITLE);
      popup02687Menu.txtTopDescription.SetText(Consts.Format(instance.VERSUS_002687POPUP_DESCRIPTION1, (IDictionary) new Hashtable()
      {
        {
          (object) "cnt",
          (object) MasterData.SeasonTicketSeasonTicket[key].increase_match.ToLocalizeNumberText()
        }
      }));
      popup02687Menu.txtSeasonTicket.SetText(instance.VERSUS_002687POPUP_SEASONTICKET);
      popup02687Menu.txtSeasonTicketNum.SetText(quantity.ToLocalizeNumberText());
      popup02687Menu.txtBotDescription.SetText(Consts.Format(instance.VERSUS_002687POPUP_DESCRIPTION2, (IDictionary) new Hashtable()
      {
        {
          (object) "cnt",
          (object) cnt.ToLocalizeNumberText()
        },
        {
          (object) "max",
          (object) max.ToLocalizeNumberText()
        }
      }));
      yield return (object) new WaitForSeconds(0.5f);
      ((UIRect) ((Component) popup02687Menu).GetComponent<UIWidget>()).alpha = 1f;
    }
  }

  public void IbtnOk()
  {
    if (this.IsPushAndSet())
      return;
    if (this.isTicket)
      Singleton<NGGameDataManager>.GetInstance().StartCoroutine(this.UpSeasonAPI());
    else
      Singleton<NGGameDataManager>.GetInstance().StartCoroutine(this.ShortagePopup());
  }

  private IEnumerator UpSeasonAPI()
  {
    Popup02687Menu popup02687Menu = this;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Future<WebAPI.Response.SeasonticketSpend> future = WebAPI.SeasonticketSpend(1, popup02687Menu.ticket.season_ticket_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      e1 = popup02687Menu.CompPopup(popup02687Menu.pvpInfo.season_remaining_matches + 1);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  private IEnumerator CompPopup(int cnt)
  {
    Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_7_1__anim_popup01.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(pF.Result).GetComponent<Popup026871Menu>().Init(cnt);
  }

  private IEnumerator ShortagePopup()
  {
    Popup02687Menu popup02687Menu = this;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Future<GameObject> pF = Res.Prefabs.popup.popup_026_8_7_2__anim_popup01.Load<GameObject>();
    IEnumerator e = pF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.5f);
    Singleton<PopupManager>.GetInstance().open(pF.Result).GetComponent<Popup026872Menu>().Init(popup02687Menu.pvpInfo, popup02687Menu.menu);
  }

  public override void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    Singleton<NGGameDataManager>.GetInstance().StartCoroutine(this.Return2688Popup());
  }

  public override void onBackButton() => this.IbtnNo();
}
