// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitReservesCount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitReservesCount : KeyCompare
  {
    public int count;
    public string id;

    public PlayerUnitReservesCount()
    {
    }

    public PlayerUnitReservesCount(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.count = (int) (long) json[nameof (count)];
      this.id = (string) json[nameof (id)];
    }
  }
}
