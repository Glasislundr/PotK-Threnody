// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitX_job_proficiencies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitX_job_proficiencies : KeyCompare
  {
    public int proficiency;
    public int job_id;

    public PlayerUnitX_job_proficiencies()
    {
    }

    public PlayerUnitX_job_proficiencies(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.proficiency = (int) (long) json[nameof (proficiency)];
      this.job_id = (int) (long) json[nameof (job_id)];
    }
  }
}
