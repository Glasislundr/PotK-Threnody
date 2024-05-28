// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitReservesLucky
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitReservesLucky : KeyCompare
  {
    public int level_up_max_status;
    public int compose;
    public bool is_max;
    public int level;
    public int initial;
    public int inheritance;
    public int buildup;
    public int transmigrate;

    public PlayerUnitReservesLucky()
    {
    }

    public PlayerUnitReservesLucky(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.level_up_max_status = (int) (long) json[nameof (level_up_max_status)];
      this.compose = (int) (long) json[nameof (compose)];
      this.is_max = (bool) json[nameof (is_max)];
      this.level = (int) (long) json[nameof (level)];
      this.initial = (int) (long) json[nameof (initial)];
      this.inheritance = (int) (long) json[nameof (inheritance)];
      this.buildup = (int) (long) json[nameof (buildup)];
      this.transmigrate = (int) (long) json[nameof (transmigrate)];
    }
  }
}
