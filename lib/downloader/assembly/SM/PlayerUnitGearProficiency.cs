// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitGearProficiency
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitGearProficiency : KeyCompare
  {
    public int total_exp;
    public int exp_next;
    public int gear_kind_id;
    public int exp;
    public int level;

    public PlayerUnitGearProficiency()
    {
    }

    public PlayerUnitGearProficiency(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.total_exp = (int) (long) json[nameof (total_exp)];
      this.exp_next = (int) (long) json[nameof (exp_next)];
      this.gear_kind_id = (int) (long) json[nameof (gear_kind_id)];
      this.exp = (int) (long) json[nameof (exp)];
      this.level = (int) (long) json[nameof (level)];
    }
  }
}
