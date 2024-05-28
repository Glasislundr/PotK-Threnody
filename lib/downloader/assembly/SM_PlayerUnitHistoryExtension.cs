// Decompiled with JetBrains decompiler
// Type: SM_PlayerUnitHistoryExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SM_PlayerUnitHistoryExtension
{
  public static PlayerUnitHistory[] AllPlayers(this PlayerUnitHistory[] self)
  {
    return ((IEnumerable<PlayerUnitHistory>) self).Where<PlayerUnitHistory>((Func<PlayerUnitHistory, bool>) (x => MasterData.UnitUnit[x.unit_id].character.category == UnitCategory.player)).ToArray<PlayerUnitHistory>();
  }

  public static PlayerUnitHistory[] AllEnemies(this PlayerUnitHistory[] self)
  {
    return ((IEnumerable<PlayerUnitHistory>) self).Where<PlayerUnitHistory>((Func<PlayerUnitHistory, bool>) (x => MasterData.UnitUnit[x.unit_id].character.category == UnitCategory.enemy)).ToArray<PlayerUnitHistory>();
  }
}
