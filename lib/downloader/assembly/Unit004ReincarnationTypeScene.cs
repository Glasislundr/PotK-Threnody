// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeScene
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
public class Unit004ReincarnationTypeScene : NGSceneBase
{
  public Unit004ReincarnationTypeMenu menu;

  public static void changeScene(
    bool stack,
    UnitTypeTicket ticket,
    PlayerUnit playerUnit,
    UnitTypeEnum selectType)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("unit004_Reincarnation_Type", (stack ? 1 : 0) != 0, (object) ticket, (object) playerUnit, (object) selectType);
  }

  public IEnumerator onStartSceneAsync(
    UnitTypeTicket ticket,
    PlayerUnit playerUnit,
    UnitTypeEnum selectType)
  {
    PlayerUnit playerUnitBefore = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == playerUnit.id));
    if (playerUnitBefore == (PlayerUnit) null)
      playerUnitBefore = playerUnit;
    Future<WebAPI.Response.UnittypeticketParameter> paramF = WebAPI.UnittypeticketParameter(playerUnitBefore.id, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    yield return (object) paramF.Wait();
    if (paramF.Result != null)
    {
      PlayerUnit playerUnitAfter = ((IEnumerable<PlayerUnit>) paramF.Result.player_units).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => (UnitTypeEnum) x.unit_type.ID == selectType));
      yield return (object) this.menu.Init(ticket, playerUnitBefore, playerUnitAfter);
    }
  }
}
