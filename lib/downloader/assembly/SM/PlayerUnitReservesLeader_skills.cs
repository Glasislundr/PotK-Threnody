// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitReservesLeader_skills
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitReservesLeader_skills : KeyCompare
  {
    public int skill_id;
    public int level;

    public PlayerUnitReservesLeader_skills()
    {
    }

    public PlayerUnitReservesLeader_skills(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.skill_id = (int) (long) json[nameof (skill_id)];
      this.level = (int) (long) json[nameof (level)];
    }
  }
}
