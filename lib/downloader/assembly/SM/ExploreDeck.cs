// Decompiled with JetBrains decompiler
// Type: SM.ExploreDeck
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
  public class ExploreDeck : KeyCompare
  {
    public int deck_type_id;
    public int?[] player_unit_ids;

    public PlayerUnit[] player_units
    {
      get
      {
        Dictionary<int, PlayerUnit> dic = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).ToDictionary<PlayerUnit, int>((Func<PlayerUnit, int>) (unit => unit.id));
        return ((IEnumerable<int?>) this.player_unit_ids).Select<int?, PlayerUnit>((Func<int?, PlayerUnit>) (id => id.HasValue ? dic[id.Value] : (PlayerUnit) null)).ToArray<PlayerUnit>();
      }
    }

    public ExploreDeck()
    {
    }

    public ExploreDeck(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.deck_type_id = (int) (long) json[nameof (deck_type_id)];
      this.player_unit_ids = ((IEnumerable<object>) json[nameof (player_unit_ids)]).Select<object, int?>((Func<object, int?>) (s =>
      {
        long? nullable = (long?) s;
        return !nullable.HasValue ? new int?() : new int?((int) nullable.GetValueOrDefault());
      })).ToArray<int?>();
    }
  }
}
