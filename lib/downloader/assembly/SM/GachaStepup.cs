// Decompiled with JetBrains decompiler
// Type: SM.GachaStepup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class GachaStepup : KeyCompare
  {
    public int? total_count;
    public int? current_count;

    public GachaStepup()
    {
    }

    public GachaStepup(Dictionary<string, object> json)
    {
      this._hasKey = false;
      long? nullable1;
      int? nullable2;
      if (json[nameof (total_count)] != null)
      {
        nullable1 = (long?) json[nameof (total_count)];
        nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable2 = new int?();
      this.total_count = nullable2;
      int? nullable3;
      if (json[nameof (current_count)] != null)
      {
        nullable1 = (long?) json[nameof (current_count)];
        nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
      }
      else
        nullable3 = new int?();
      this.current_count = nullable3;
    }
  }
}
