// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeTicketSelectionScene
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
public class Unit004ReincarnationTypeTicketSelectionScene : NGSceneBase
{
  private Unit004ReincarnationTypeTicketSelectionMenu mainMenu_;
  private DateTime current_time;

  public static void changeScene(bool isStack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_Reincarnation_Type_TicketSelection", isStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Unit004ReincarnationTypeTicketSelectionScene ticketSelectionScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    ticketSelectionScene.mainMenu_ = ticketSelectionScene.menuBase as Unit004ReincarnationTypeTicketSelectionMenu;
    ticketSelectionScene.current_time = ServerTime.NowAppTime();
    if (!WebAPI.IsResponsedAtRecent("UnittypeticketIndex", 4.0))
    {
      Future<WebAPI.Response.UnittypeticketIndex> future = WebAPI.UnittypeticketIndex((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      yield return (object) future.Wait();
      if (future.Result == null)
        yield break;
      else
        future = (Future<WebAPI.Response.UnittypeticketIndex>) null;
    }
    List<PlayerUnitTypeTicket> list1 = ((IEnumerable<PlayerUnitTypeTicket>) SMManager.Get<PlayerUnitTypeTicket[]>()).Where<PlayerUnitTypeTicket>((Func<PlayerUnitTypeTicket, bool>) (x => x.quantity > 0)).ToList<PlayerUnitTypeTicket>();
    UnitTypeTicket[] ticketsDataList = MasterData.UnitTypeTicketList;
    int[] active_ticket_ids = ((IEnumerable<UnitTypeTicket>) ticketsDataList).Where<UnitTypeTicket>((Func<UnitTypeTicket, bool>) (t => t.end_at.Value >= this.current_time)).Select<UnitTypeTicket, int>((Func<UnitTypeTicket, int>) (t => t.ID)).ToArray<int>();
    Func<PlayerUnitTypeTicket, bool> predicate = (Func<PlayerUnitTypeTicket, bool>) (at => ((IEnumerable<int>) active_ticket_ids).Contains<int>(at.ticket_id));
    List<PlayerUnitTypeTicket> list2 = list1.Where<PlayerUnitTypeTicket>(predicate).ToList<PlayerUnitTypeTicket>().OrderBy<PlayerUnitTypeTicket, int>((Func<PlayerUnitTypeTicket, int>) (x => this.GetTicketsPriority(ticketsDataList, x))).ToList<PlayerUnitTypeTicket>();
    yield return (object) ticketSelectionScene.mainMenu_.coInitialize(list2);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private int GetTicketsPriority(UnitTypeTicket[] ticketsDataList, PlayerUnitTypeTicket ticket)
  {
    return ((IEnumerable<UnitTypeTicket>) ticketsDataList).First<UnitTypeTicket>((Func<UnitTypeTicket, bool>) (x => x.ID == ticket.ticket_id)).priority;
  }

  public override void onEndScene()
  {
    base.onEndScene();
    this.mainMenu_.onEndMenu();
  }
}
