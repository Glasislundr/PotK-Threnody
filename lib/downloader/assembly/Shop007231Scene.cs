// Decompiled with JetBrains decompiler
// Type: Shop007231Scene
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

#nullable disable
public class Shop007231Scene : NGSceneBase
{
  private static readonly string NAME_SCENE = "shop007_23_1";
  private Shop007231Menu mainMenu_;

  public static void changeScene(bool isStack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Shop007231Scene.NAME_SCENE, isStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Shop007231Scene shop007231Scene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    shop007231Scene.mainMenu_ = shop007231Scene.menuBase as Shop007231Menu;
    IEnumerator e1;
    if (!WebAPI.IsResponsedAtRecent("ShopStatus"))
    {
      Future<WebAPI.Response.ShopStatus> future = WebAPI.ShopStatus((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      })).Then<WebAPI.Response.ShopStatus>((Func<WebAPI.Response.ShopStatus, WebAPI.Response.ShopStatus>) (result =>
      {
        Singleton<NGGameDataManager>.GetInstance().Parse(result);
        return result;
      }));
      e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result == null)
        yield break;
      else
        future = (Future<WebAPI.Response.ShopStatus>) null;
    }
    PlayerSelectTicketSummary[] source1 = SMManager.Get<PlayerSelectTicketSummary[]>();
    if (source1 != null)
    {
      SM.SelectTicket[] source2 = SMManager.Get<SM.SelectTicket[]>();
      List<PlayerSelectTicketSummary> playerUnitTickets = ((IEnumerable<PlayerSelectTicketSummary>) source1).Where<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x != null && x.quantity > 0)).ToList<PlayerSelectTicketSummary>();
      for (int i = playerUnitTickets.Count - 1; i >= 0; i--)
      {
        MasterDataTable.SelectTicket selectTicket = ((IEnumerable<MasterDataTable.SelectTicket>) MasterData.SelectTicketList).FirstOrDefault<MasterDataTable.SelectTicket>((Func<MasterDataTable.SelectTicket, bool>) (x => x.ID == playerUnitTickets[i].ticket_id));
        if (selectTicket != null && selectTicket.category != SelectTicketCategory.Unit)
          playerUnitTickets.Remove(playerUnitTickets[i]);
      }
      int[] ticketIds = playerUnitTickets.Select<PlayerSelectTicketSummary, int>((Func<PlayerSelectTicketSummary, int>) (pt => pt.ticket_id)).ToArray<int>();
      IEnumerable<SM.SelectTicket> source3 = ((IEnumerable<SM.SelectTicket>) source2).Where<SM.SelectTicket>((Func<SM.SelectTicket, bool>) (t => ((IEnumerable<int>) ticketIds).Contains<int>(t.id)));
      MasterDataTable.SelectTicket[] ticketsDataList = MasterData.SelectTicketList;
      IOrderedEnumerable<SM.SelectTicket> unitTickets = source3.OrderBy<SM.SelectTicket, DateTime>((Func<SM.SelectTicket, DateTime>) (x => !x.end_at.HasValue ? DateTime.MaxValue : x.end_at.Value)).ThenBy<SM.SelectTicket, int>((Func<SM.SelectTicket, int>) (s => this.GetTicketsPriority(ticketsDataList, playerUnitTickets.FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == s.id)))));
      e1 = shop007231Scene.mainMenu_.coInitialize((IEnumerable<SM.SelectTicket>) unitTickets, playerUnitTickets);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }
    else
    {
      e1 = shop007231Scene.coEndSceneErrorCouponDateTime();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  private int GetTicketsPriority(
    MasterDataTable.SelectTicket[] ticketDataList,
    PlayerSelectTicketSummary playerUnitTicket)
  {
    return ((IEnumerable<MasterDataTable.SelectTicket>) ticketDataList).First<MasterDataTable.SelectTicket>((Func<MasterDataTable.SelectTicket, bool>) (x => x.ID == playerUnitTicket.ticket_id)).priority;
  }

  private IEnumerator coEndSceneErrorCouponDateTime()
  {
    bool berrorwait = true;
    Consts instance = Consts.GetInstance();
    ModalWindow.Show(instance.SHOP_00723_ERROR_DATE_TITLE, instance.SHOP_00723_ERROR_DATE_MESSAGE, (Action) (() => berrorwait = false));
    while (berrorwait)
      yield return (object) null;
    MypageScene.ChangeSceneOnError();
  }

  public override void onEndScene()
  {
    base.onEndScene();
    this.mainMenu_.onEndMenu();
  }
}
