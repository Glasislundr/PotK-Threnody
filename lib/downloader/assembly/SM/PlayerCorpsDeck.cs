// Decompiled with JetBrains decompiler
// Type: SM.PlayerCorpsDeck
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
  public class PlayerCorpsDeck : KeyCompare
  {
    public int corps_id;
    public int[] deck_player_unit_ids;

    public PlayerCorpsDeck()
    {
    }

    public PlayerCorpsDeck(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.corps_id = (int) (long) json[nameof (corps_id)];
      this.deck_player_unit_ids = ((IEnumerable<object>) json[nameof (deck_player_unit_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
    }
  }
}
