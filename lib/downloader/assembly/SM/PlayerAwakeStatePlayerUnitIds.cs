// Decompiled with JetBrains decompiler
// Type: SM.PlayerAwakeStatePlayerUnitIds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerAwakeStatePlayerUnitIds : KeyCompare
  {
    public int[] awake_state_player_unit_ids;

    public PlayerAwakeStatePlayerUnitIds()
    {
    }

    public PlayerAwakeStatePlayerUnitIds(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.awake_state_player_unit_ids = ((IEnumerable<object>) json[nameof (awake_state_player_unit_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
    }
  }
}
