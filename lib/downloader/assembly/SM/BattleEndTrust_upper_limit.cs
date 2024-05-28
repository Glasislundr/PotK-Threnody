// Decompiled with JetBrains decompiler
// Type: SM.BattleEndTrust_upper_limit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class BattleEndTrust_upper_limit : KeyCompare
  {
    public int same_character_id;
    public int before_value;
    public int after_value;

    public BattleEndTrust_upper_limit()
    {
    }

    public BattleEndTrust_upper_limit(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.same_character_id = (int) (long) json[nameof (same_character_id)];
      this.before_value = (int) (long) json[nameof (before_value)];
      this.after_value = (int) (long) json[nameof (after_value)];
    }
  }
}
