// Decompiled with JetBrains decompiler
// Type: SM.PlayerUnitAgility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerUnitAgility : KeyCompare
  {
    [SerializeField]
    private int overkillers_value_;
    public int level_up_max_status;
    public int compose;
    public bool is_max;
    public int level;
    public int x_level;
    public int initial;
    public int inheritance;
    public int buildup;
    public int transmigrate;

    public int overkillersValue => this.overkillers_value_;

    public void resetOverkillersValue(int value = 0) => this.overkillers_value_ = value;

    public PlayerUnitAgility()
    {
    }

    public PlayerUnitAgility(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.level_up_max_status = (int) (long) json[nameof (level_up_max_status)];
      this.compose = (int) (long) json[nameof (compose)];
      this.is_max = (bool) json[nameof (is_max)];
      this.level = (int) (long) json[nameof (level)];
      this.x_level = (int) (long) json[nameof (x_level)];
      this.initial = (int) (long) json[nameof (initial)];
      this.inheritance = (int) (long) json[nameof (inheritance)];
      this.buildup = (int) (long) json[nameof (buildup)];
      this.transmigrate = (int) (long) json[nameof (transmigrate)];
    }

    public bool isMax(int add = 0)
    {
      return this.is_max || this.compose + this.level + this.transmigrate + add >= this.level_up_max_status;
    }

    public int buildupMaxCnt(PlayerUnit unit)
    {
      int num = unit.buildup_limit - (unit.buildup_count - this.buildup);
      if (num > unit.unit.buildup_limit_release_id.agility_limit_release_cnt)
        num = unit.unit.buildup_limit_release_id.agility_limit_release_cnt;
      if (num > this.level_up_max_status - this.level)
        num = this.level_up_max_status - this.level;
      return num;
    }

    public int possibleBuildupCnt(PlayerUnit unit)
    {
      int num = unit.buildup_limit - (unit.buildup_count - this.buildup);
      if (num > unit.unit.buildup_limit_release_id.agility_limit_release_cnt)
        num = unit.unit.buildup_limit_release_id.agility_limit_release_cnt;
      if (num > unit.unit.buildup_limit_release_id.agility_limit_release_cnt - this.buildup)
        num = unit.unit.buildup_limit_release_id.agility_limit_release_cnt - this.buildup;
      if (num > this.level_up_max_status - this.level)
        num = this.level_up_max_status - this.level;
      return num;
    }

    public int MemoryLevelupStatus(int val)
    {
      int num = this.MemoryStatus(val);
      return num < this.level_up_max_status ? this.level_up_max_status : num;
    }

    public bool IsMemoryMax(int val) => this.MemoryStatus(val) >= this.level_up_max_status;

    private int MemoryStatus(int val) => this.compose + val + this.transmigrate;
  }
}
