// Decompiled with JetBrains decompiler
// Type: SM.EnemyTopInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class EnemyTopInfo : KeyCompare
  {
    public int unit_id;
    public int min_point;

    public EnemyTopInfo()
    {
    }

    public EnemyTopInfo(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.unit_id = (int) (long) json[nameof (unit_id)];
      this.min_point = (int) (long) json[nameof (min_point)];
    }
  }
}
