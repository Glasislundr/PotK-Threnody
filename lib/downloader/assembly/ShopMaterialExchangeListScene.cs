// Decompiled with JetBrains decompiler
// Type: ShopMaterialExchangeListScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ShopMaterialExchangeListScene : NGSceneBase
{
  private static readonly string DEFAULT_NAME = "shop007_MaterialExchange_Item";
  private bool isInitialized_;
  private ShopMaterialExchangeListMenu mainMenu_;
  private PlayerSelectTicketSummary playerUnitTicket_;
  private SelectTicket unitTicket_;

  public bool isErrorTicketDateTime_ { get; private set; }

  public static void changeScene(PlayerSelectTicketSummary playerUnitTicket, bool isStack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(ShopMaterialExchangeListScene.DEFAULT_NAME, (isStack ? 1 : 0) != 0, (object) playerUnitTicket);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int ticketID)
  {
    ShopMaterialExchangeListScene exchangeListScene = this;
    exchangeListScene.isInitialized_ = false;
    exchangeListScene.isErrorTicketDateTime_ = false;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    exchangeListScene.mainMenu_ = exchangeListScene.menuBase as ShopMaterialExchangeListMenu;
    PlayerSelectTicketSummary[] player_unit_tickets = SMManager.Get<PlayerSelectTicketSummary[]>();
    Future<WebAPI.Response.ShopStatus> future = WebAPI.ShopStatus((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.ShopStatus result = future.Result;
    if (result != null)
    {
      if (result.select_tickets != null)
      {
        if (ticketID == 0)
          ((IEnumerable<SelectTicket>) result.select_tickets).FirstOrDefault<SelectTicket>();
        else
          ((IEnumerable<SelectTicket>) result.select_tickets).FirstOrDefault<SelectTicket>((Func<SelectTicket, bool>) (t => t.id == ticketID));
        PlayerSelectTicketSummary playerUnitTicket = ticketID != 0 ? ((IEnumerable<PlayerSelectTicketSummary>) player_unit_tickets).FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (pt => pt.ticket_id == ticketID)) : ((IEnumerable<PlayerSelectTicketSummary>) player_unit_tickets).FirstOrDefault<PlayerSelectTicketSummary>();
        e1 = exchangeListScene.onStartSceneAsync(playerUnitTicket);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
      else
      {
        e1 = exchangeListScene.coEndSceneErrorTicketDateTime();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
    }
  }

  public IEnumerator onStartSceneAsync(PlayerSelectTicketSummary playerUnitTicket)
  {
    ShopMaterialExchangeListScene scene = this;
    IEnumerator e;
    if (!scene.isInitialized_)
    {
      scene.isErrorTicketDateTime_ = false;
      scene.mainMenu_ = scene.menuBase as ShopMaterialExchangeListMenu;
      if (!Singleton<CommonRoot>.GetInstance().isLoading)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = true;
        yield return (object) null;
      }
      scene.playerUnitTicket_ = playerUnitTicket;
      if (scene.playerUnitTicket_ != null)
      {
        SelectTicket[] source = SMManager.Get<SelectTicket[]>();
        // ISSUE: reference to a compiler-generated method
        scene.unitTicket_ = ((IEnumerable<SelectTicket>) source).FirstOrDefault<SelectTicket>(new Func<SelectTicket, bool>(scene.\u003ConStartSceneAsync\u003Eb__12_0));
        if (scene.unitTicket_ != null)
        {
          e = scene.mainMenu_.coInitialize(scene, scene.unitTicket_, scene.playerUnitTicket_);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (scene.playerUnitTicket_.quantity >= scene.unitTicket_.cost)
          {
            scene.isInitialized_ = true;
          }
          else
          {
            e = scene.coEndSceneErrorTicketShortage();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            yield break;
          }
        }
        else
          goto label_15;
      }
      else
        goto label_15;
    }
    e = scene.coCheckTicketDateTime(scene.unitTicket_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!scene.isErrorTicketDateTime_)
    {
      scene.StartCoroutine(scene.coLateStartScene());
      yield break;
    }
label_15:
    e = scene.coEndSceneErrorTicketDateTime();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator coLateStartScene()
  {
    yield return (object) null;
    yield return (object) new WaitForSeconds(0.5f);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator coCheckTicketDateTime(SelectTicket unitTicket)
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime dateTime1 = ServerTime.NowAppTimeAddDelta();
    if (!(unitTicket.start_at > dateTime1))
    {
      DateTime? endAt = unitTicket.end_at;
      DateTime dateTime2 = dateTime1;
      if ((endAt.HasValue ? (endAt.GetValueOrDefault() <= dateTime2 ? 1 : 0) : 0) == 0)
        yield break;
    }
    this.isErrorTicketDateTime_ = true;
  }

  public IEnumerator coEndSceneErrorTicketDateTime()
  {
    bool berrorwait = true;
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.SHOP_00723_ERROR_DATE_TITLE, instance.SHOP_00723_ERROR_DATE_MESSAGE, (Action) (() => berrorwait = false));
    while (berrorwait)
      yield return (object) null;
    MypageScene.ChangeSceneOnError();
  }

  public IEnumerator coEndSceneErrorTicketShortage()
  {
    bool berrorwait = true;
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.SHOP_00723_ERROR_SHORTAGE_TITLE, instance.SHOP_00723_ERROR_SHORTAGE_MESSAGE, (Action) (() => berrorwait = false));
    while (berrorwait)
      yield return (object) null;
    MypageScene.ChangeSceneOnError();
  }
}
