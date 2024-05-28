// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleskillDuelEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleskillDuelEffect
  {
    public int ID;
    public string duel_animator_name;
    public string duel_vehicle_animator_name;
    public string duel_camera_animator_name;
    public float duel_koyu_wait_time;
    public float duel_koyu_enemy_pos_x;
    public float duel_koyu_enemy_pos_y;
    public float duel_koyu_enemy_pos_z;
    public string duel_effect_name;
    public bool vehicle_link_off;

    public bool isKoyuDuelEffect
    {
      get
      {
        return !string.IsNullOrEmpty(this.duel_animator_name) && (double) this.duel_koyu_wait_time > 0.0;
      }
    }

    public List<string> preloadEffectFileNameList
    {
      get
      {
        return ((IEnumerable<BattleskillDuelEffectPreload>) MasterData.BattleskillDuelEffectPreloadList).Where<BattleskillDuelEffectPreload>((Func<BattleskillDuelEffectPreload, bool>) (x => this.ID == x.duel_effect_id)).Select<BattleskillDuelEffectPreload, string>((Func<BattleskillDuelEffectPreload, string>) (x => x.effect_file_name)).ToList<string>();
      }
    }

    public List<UnitUnit> preloadCutinUnitList
    {
      get
      {
        return ((IEnumerable<BattleskillDuelCutinPreload>) MasterData.BattleskillDuelCutinPreloadList).Where<BattleskillDuelCutinPreload>((Func<BattleskillDuelCutinPreload, bool>) (x => this.ID == x.duel_effect_id)).Select<BattleskillDuelCutinPreload, UnitUnit>((Func<BattleskillDuelCutinPreload, UnitUnit>) (x => x.unit)).ToList<UnitUnit>();
      }
    }

    public List<string> preloadClipEventEffectDataFileNameList
    {
      get
      {
        return ((IEnumerable<BattleskillDuelClipEventEffectDataPreload>) MasterData.BattleskillDuelClipEventEffectDataPreloadList).Where<BattleskillDuelClipEventEffectDataPreload>((Func<BattleskillDuelClipEventEffectDataPreload, bool>) (x => this.ID == x.duel_effect_id)).Select<BattleskillDuelClipEventEffectDataPreload, string>((Func<BattleskillDuelClipEventEffectDataPreload, string>) (x => x.clipeventeffectdata_file_name)).ToList<string>();
      }
    }

    public Vector3 getEnemyPosition
    {
      get
      {
        return new Vector3(this.duel_koyu_enemy_pos_x, this.duel_koyu_enemy_pos_y, this.duel_koyu_enemy_pos_z);
      }
    }

    public static BattleskillDuelEffect Parse(MasterDataReader reader)
    {
      return new BattleskillDuelEffect()
      {
        ID = reader.ReadInt(),
        duel_animator_name = reader.ReadString(true),
        duel_vehicle_animator_name = reader.ReadString(true),
        duel_camera_animator_name = reader.ReadString(true),
        duel_koyu_wait_time = reader.ReadFloat(),
        duel_koyu_enemy_pos_x = reader.ReadFloat(),
        duel_koyu_enemy_pos_y = reader.ReadFloat(),
        duel_koyu_enemy_pos_z = reader.ReadFloat(),
        duel_effect_name = reader.ReadString(true),
        vehicle_link_off = reader.ReadBool()
      };
    }
  }
}
