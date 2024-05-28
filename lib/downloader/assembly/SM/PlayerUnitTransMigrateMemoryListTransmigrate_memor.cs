// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitTransMigrateMemoryListTransmigrate_memory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitTransMigrateMemoryListTransmigrate_memory : KeyCompare
  {
    public int dexterity;
    public int agility;
    public int strength;
    public int level;
    public int intelligence;
    public int hp;
    public int mind;
    public int lucky;
    public int vitality;
    public int player_unit_id;

    public PlayerUnitTransMigrateMemoryListTransmigrate_memory()
    {
    }

    public PlayerUnitTransMigrateMemoryListTransmigrate_memory(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.dexterity = (int) (long) json[nameof (dexterity)];
      this.agility = (int) (long) json[nameof (agility)];
      this.strength = (int) (long) json[nameof (strength)];
      this.level = (int) (long) json[nameof (level)];
      this.intelligence = (int) (long) json[nameof (intelligence)];
      this.hp = (int) (long) json[nameof (hp)];
      this.mind = (int) (long) json[nameof (mind)];
      this.lucky = (int) (long) json[nameof (lucky)];
      this.vitality = (int) (long) json[nameof (vitality)];
      this.player_unit_id = (int) (long) json[nameof (player_unit_id)];
    }
  }
}
