// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitXJobStatus
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
  public class PlayerUnitXJobStatus : KeyCompare
  {
    public int[] levels;
    public int total_exp;

    public PlayerUnitXJobStatus()
    {
    }

    public PlayerUnitXJobStatus(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.levels = ((IEnumerable<object>) json[nameof (levels)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.total_exp = (int) (long) json[nameof (total_exp)];
    }
  }
}
