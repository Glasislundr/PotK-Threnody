// Decompiled with JetBrains decompiler
// Type: SM.PlayerGearBuildupParam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerGearBuildupParam : KeyCompare
  {
    public int dexterity_add;
    public int mind_add;
    public int lucky_add;
    public int agility_add;
    public int intelligence_add;
    public int strength_add;
    public int vitality_add;
    public int hp_add;

    public PlayerGearBuildupParam()
    {
    }

    public PlayerGearBuildupParam(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.dexterity_add = (int) (long) json[nameof (dexterity_add)];
      this.mind_add = (int) (long) json[nameof (mind_add)];
      this.lucky_add = (int) (long) json[nameof (lucky_add)];
      this.agility_add = (int) (long) json[nameof (agility_add)];
      this.intelligence_add = (int) (long) json[nameof (intelligence_add)];
      this.strength_add = (int) (long) json[nameof (strength_add)];
      this.vitality_add = (int) (long) json[nameof (vitality_add)];
      this.hp_add = (int) (long) json[nameof (hp_add)];
    }
  }
}
