// Decompiled with JetBrains decompiler
// Type: SM.FixedEntity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class FixedEntity : KeyCompare
  {
    public int single_fix_count;
    public string fix_reward;
    public int gacha_id;

    public FixedEntity()
    {
    }

    public FixedEntity(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.single_fix_count = (int) (long) json[nameof (single_fix_count)];
      this.fix_reward = (string) json[nameof (fix_reward)];
      this.gacha_id = (int) (long) json[nameof (gacha_id)];
    }
  }
}
