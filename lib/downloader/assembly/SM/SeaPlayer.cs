// Decompiled with JetBrains decompiler
// Type: SM.SeaPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class SeaPlayer : KeyCompare
  {
    public int dp_auto_healing_sec;
    public int dp_full_remain;
    public bool is_released_sea_call;
    public int dp;
    public int dp_max;

    public SeaPlayer()
    {
    }

    public SeaPlayer(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.dp_auto_healing_sec = (int) (long) json[nameof (dp_auto_healing_sec)];
      this.dp_full_remain = (int) (long) json[nameof (dp_full_remain)];
      this.is_released_sea_call = (bool) json[nameof (is_released_sea_call)];
      this.dp = (int) (long) json[nameof (dp)];
      this.dp_max = (int) (long) json[nameof (dp_max)];
    }
  }
}
