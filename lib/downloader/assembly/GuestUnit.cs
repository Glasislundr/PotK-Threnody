// Decompiled with JetBrains decompiler
// Type: GuestUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class GuestUnit
{
  public static int[] GetGuestsID(int stageID)
  {
    List<int> intList = new List<int>();
    foreach (KeyValuePair<int, BattleStageGuest> keyValuePair in MasterData.BattleStageGuest.Where<KeyValuePair<int, BattleStageGuest>>((Func<KeyValuePair<int, BattleStageGuest>, bool>) (x => x.Value.stage_BattleStage == stageID)).ToList<KeyValuePair<int, BattleStageGuest>>())
    {
      KeyValuePair<int, BattleStageGuest> unit = keyValuePair;
      if (MasterData.BattleStagePlayer.Any<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == stageID && x.Value.deck_position == unit.Value.deck_position)))
        intList.Add(unit.Value.ID);
    }
    return intList.ToArray();
  }
}
