﻿// Decompiled with JetBrains decompiler
// Type: SM.PlayerRentalPlayerUnitIds
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
  public class PlayerRentalPlayerUnitIds : KeyCompare
  {
    public int?[] rental_player_unit_ids;

    public PlayerRentalPlayerUnitIds()
    {
    }

    public PlayerRentalPlayerUnitIds(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.rental_player_unit_ids = ((IEnumerable<object>) json[nameof (rental_player_unit_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
    }
  }
}
