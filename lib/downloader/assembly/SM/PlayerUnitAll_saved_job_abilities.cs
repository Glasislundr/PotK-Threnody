// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitAll_saved_job_abilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitAll_saved_job_abilities : KeyCompare
  {
    public int job_ability_id;
    public int level;

    public PlayerUnitAll_saved_job_abilities()
    {
    }

    public PlayerUnitAll_saved_job_abilities(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.job_ability_id = (int) (long) json[nameof (job_ability_id)];
      this.level = (int) (long) json[nameof (level)];
    }
  }
}
