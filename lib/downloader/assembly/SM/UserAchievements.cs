// Decompiled with JetBrains decompiler
// Type: SM.UserAchievements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class UserAchievements : KeyCompare
  {
    public int achievement_id;
    public int value;
    public bool unlocked;

    public UserAchievements()
    {
    }

    public UserAchievements(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.achievement_id = (int) (long) json[nameof (achievement_id)];
      this.value = (int) (long) json[nameof (value)];
      this.unlocked = (bool) json[nameof (unlocked)];
    }
  }
}
