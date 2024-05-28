// Decompiled with JetBrains decompiler
// Type: SM.TowerDeck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class TowerDeck : KeyCompare
  {
    public TowerDeckUnit[] tower_deck_units;

    public TowerDeck()
    {
    }

    public TowerDeck(Dictionary<string, object> json)
    {
      this._hasKey = false;
      List<TowerDeckUnit> towerDeckUnitList = new List<TowerDeckUnit>();
      foreach (object json1 in (List<object>) json[nameof (tower_deck_units)])
        towerDeckUnitList.Add(json1 == null ? (TowerDeckUnit) null : new TowerDeckUnit((Dictionary<string, object>) json1));
      this.tower_deck_units = towerDeckUnitList.ToArray();
    }
  }
}
