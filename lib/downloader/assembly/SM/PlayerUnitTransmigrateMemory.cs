// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitTransmigrateMemory
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
  public class PlayerUnitTransmigrateMemory : KeyCompare
  {
    public int?[] player_unit_ids;

    public PlayerUnitTransmigrateMemory()
    {
    }

    public PlayerUnitTransmigrateMemory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.player_unit_ids = ((IEnumerable<object>) json[nameof (player_unit_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
    }
  }
}
