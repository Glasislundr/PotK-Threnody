// Decompiled with JetBrains decompiler
// Type: MasterDataTable.BattleStage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace MasterDataTable
{
  [Serializable]
  public class BattleStage
  {
    public int ID;
    public int victory_condition_BattleVictoryCondition;
    public int map_BattleMap;
    public int map_offset_x;
    public int map_offset_y;
    public int map_width;
    public int map_height;
    public string field_player_bgm_file;
    public string field_player_bgm;
    public string field_enemy_bgm_file;
    public string field_enemy_bgm;
    public string duel_player_bgm_file;
    public string duel_player_bgm;
    public string duel_enemy_bgm_file;
    public string duel_enemy_bgm;
    public string bus_dsp_setting_name;
    public float bus_id_0_level;
    public float bus_id_1_level;
    public float bus_id_2_level;
    public float bus_id_3_level;
    public float bus_id_4_level;
    public float bus_id_5_level;
    public float bus_id_6_level;
    public float bus_id_7_level;
    public int recommend_strength;

    public void ApplyLandforms(Action<int, int, BattleMapLandform> f)
    {
      int num1 = this.map_offset_x + this.map_width;
      int num2 = this.map_offset_y + this.map_height;
      foreach (BattleMapLandform battleMapLandform in MasterData.BattleMapLandformList)
      {
        if (this.map.ID == battleMapLandform.map.ID && this.map_offset_x <= battleMapLandform.coordinate_x && battleMapLandform.coordinate_x < num1 && this.map_offset_y <= battleMapLandform.coordinate_y && battleMapLandform.coordinate_y < num2)
          f(battleMapLandform.coordinate_x - this.map_offset_x, battleMapLandform.coordinate_y - this.map_offset_y, battleMapLandform);
      }
    }

    public BattleStageEnemy[] Enemies
    {
      get
      {
        return ((IEnumerable<BattleStageEnemy>) MasterData.BattleStageEnemyList).Where<BattleStageEnemy>((Func<BattleStageEnemy, bool>) (x => x.stage.ID == this.ID)).ToArray<BattleStageEnemy>();
      }
    }

    public BattleStagePlayer[] Players
    {
      get
      {
        return ((IEnumerable<BattleStagePlayer>) MasterData.BattleStagePlayerList).Where<BattleStagePlayer>((Func<BattleStagePlayer, bool>) (x => x.stage.ID == this.ID)).ToArray<BattleStagePlayer>();
      }
    }

    public BattleEarthStageGuest[] Guests
    {
      get
      {
        return ((IEnumerable<BattleEarthStageGuest>) MasterData.BattleEarthStageGuestList).Where<BattleEarthStageGuest>((Func<BattleEarthStageGuest, bool>) (x => x.stage.ID == this.ID)).ToArray<BattleEarthStageGuest>();
      }
    }

    public BattleFieldEffectStage[] FieldEffectStages
    {
      get
      {
        return ((IEnumerable<BattleFieldEffectStage>) MasterData.BattleFieldEffectStageList).Where<BattleFieldEffectStage>((Func<BattleFieldEffectStage, bool>) (x => x.stage.ID == this.ID)).ToArray<BattleFieldEffectStage>();
      }
    }

    public float[] busLevel
    {
      get
      {
        return new float[8]
        {
          this.bus_id_0_level,
          this.bus_id_1_level,
          this.bus_id_2_level,
          this.bus_id_3_level,
          this.bus_id_4_level,
          this.bus_id_5_level,
          this.bus_id_6_level,
          this.bus_id_7_level
        };
      }
    }

    public static BattleStage Parse(MasterDataReader reader)
    {
      return new BattleStage()
      {
        ID = reader.ReadInt(),
        victory_condition_BattleVictoryCondition = reader.ReadInt(),
        map_BattleMap = reader.ReadInt(),
        map_offset_x = reader.ReadInt(),
        map_offset_y = reader.ReadInt(),
        map_width = reader.ReadInt(),
        map_height = reader.ReadInt(),
        field_player_bgm_file = reader.ReadString(true),
        field_player_bgm = reader.ReadString(true),
        field_enemy_bgm_file = reader.ReadString(true),
        field_enemy_bgm = reader.ReadString(true),
        duel_player_bgm_file = reader.ReadString(true),
        duel_player_bgm = reader.ReadString(true),
        duel_enemy_bgm_file = reader.ReadString(true),
        duel_enemy_bgm = reader.ReadString(true),
        bus_dsp_setting_name = reader.ReadString(true),
        bus_id_0_level = reader.ReadFloat(),
        bus_id_1_level = reader.ReadFloat(),
        bus_id_2_level = reader.ReadFloat(),
        bus_id_3_level = reader.ReadFloat(),
        bus_id_4_level = reader.ReadFloat(),
        bus_id_5_level = reader.ReadFloat(),
        bus_id_6_level = reader.ReadFloat(),
        bus_id_7_level = reader.ReadFloat(),
        recommend_strength = reader.ReadInt()
      };
    }

    public BattleVictoryCondition victory_condition
    {
      get
      {
        BattleVictoryCondition victoryCondition;
        if (!MasterData.BattleVictoryCondition.TryGetValue(this.victory_condition_BattleVictoryCondition, out victoryCondition))
          Debug.LogError((object) ("Key not Found: MasterData.BattleVictoryCondition[" + (object) this.victory_condition_BattleVictoryCondition + "]"));
        return victoryCondition;
      }
    }

    public BattleMap map
    {
      get
      {
        BattleMap map;
        if (!MasterData.BattleMap.TryGetValue(this.map_BattleMap, out map))
          Debug.LogError((object) ("Key not Found: MasterData.BattleMap[" + (object) this.map_BattleMap + "]"));
        return map;
      }
    }
  }
}
