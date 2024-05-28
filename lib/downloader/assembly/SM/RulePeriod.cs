// Decompiled with JetBrains decompiler
// Type: SM.RulePeriod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class RulePeriod : KeyCompare
  {
    public int remaining_time;
    public int rule_period_id;

    public RulePeriod()
    {
    }

    public RulePeriod(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.remaining_time = (int) (long) json[nameof (remaining_time)];
      this.rule_period_id = (int) (long) json[nameof (rule_period_id)];
    }
  }
}
