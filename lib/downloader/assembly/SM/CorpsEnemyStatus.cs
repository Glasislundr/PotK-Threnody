// Decompiled with JetBrains decompiler
// Type: SM.CorpsEnemyStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class CorpsEnemyStatus : KeyCompare
  {
    public int hp;
    public int id;

    public CorpsEnemyStatus()
    {
    }

    public CorpsEnemyStatus(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.hp = (int) (long) json[nameof (hp)];
      this.id = (int) (long) json[nameof (id)];
    }
  }
}
