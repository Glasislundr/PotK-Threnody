// Decompiled with JetBrains decompiler
// Type: SM.StepupPack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class StepupPack : KeyCompare
  {
    public int id;
    public DateTime? end_at;

    public StepupPack()
    {
    }

    public StepupPack(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.id = (int) (long) json[nameof (id)];
      this.end_at = json[nameof (end_at)] == null ? new DateTime?() : new DateTime?(DateTime.Parse((string) json[nameof (end_at)]));
    }
  }
}
