// Decompiled with JetBrains decompiler
// Type: SM.StepupPackSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class StepupPackSet : KeyCompare
  {
    public int coin_group_id;
    public int step;
    public string name;

    public StepupPackSet()
    {
    }

    public StepupPackSet(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.coin_group_id = (int) (long) json[nameof (coin_group_id)];
      this.step = (int) (long) json[nameof (step)];
      this.name = (string) json[nameof (name)];
    }
  }
}
