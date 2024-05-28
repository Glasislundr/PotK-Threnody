// Decompiled with JetBrains decompiler
// Type: SM.RouletteModuleRoulette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class RouletteModuleRoulette : KeyCompare
  {
    public bool is_campaign;
    public int deck_id;
    public bool can_roulette;
    public int id;

    public RouletteModuleRoulette()
    {
    }

    public RouletteModuleRoulette(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.is_campaign = (bool) json[nameof (is_campaign)];
      this.deck_id = (int) (long) json[nameof (deck_id)];
      this.can_roulette = (bool) json[nameof (can_roulette)];
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
