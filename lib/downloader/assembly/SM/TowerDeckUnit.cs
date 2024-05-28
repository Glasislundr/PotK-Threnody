// Decompiled with JetBrains decompiler
// Type: SM.TowerDeckUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class TowerDeckUnit : KeyCompare
  {
    public int position_id;
    public int player_unit_id;

    public TowerDeckUnit()
    {
    }

    public TowerDeckUnit(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.position_id = (int) (long) json[nameof (position_id)];
      this.player_unit_id = (int) (long) json[nameof (player_unit_id)];
    }
  }
}
