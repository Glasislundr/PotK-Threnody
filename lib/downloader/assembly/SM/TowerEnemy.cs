// Decompiled with JetBrains decompiler
// Type: SM.TowerEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class TowerEnemy : KeyCompare
  {
    public float hitpoint_rate;
    public int id;

    public TowerEnemy()
    {
    }

    public TowerEnemy(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.hitpoint_rate = (float) (double) json[nameof (hitpoint_rate)];
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
