// Decompiled with JetBrains decompiler
// Type: TowerBattleResult
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MiniJSON;
using System;
using System.Collections.Generic;

#nullable disable
public class TowerBattleResult
{
  private string salt_;

  public TowerBattleResult(string salt) => this.salt_ = salt;

  public Future<WebAPI.Response.TowerBattleFinish> webAPI(
    int battle_turn,
    string battle_uuid,
    int overkill_damage,
    float[] result_enemy_hitpoint_rate,
    int[] result_enemy_id,
    int[] result_enemy_kill_count,
    float[] result_player_unit_hitpoint_rate,
    int[] result_player_unit_id,
    int[] result_supply_id,
    int[] result_supply_use_quantity,
    bool win,
    Action<WebAPI.Response.UserError> userErrorCallback = null)
  {
    return WebAPI.TowerBattleFinish(battle_turn, battle_uuid, overkill_damage, result_enemy_hitpoint_rate, result_enemy_id, result_enemy_kill_count, result_player_unit_hitpoint_rate, result_player_unit_id, result_supply_id, result_supply_use_quantity, this.toJsonString(battle_turn, battle_uuid, overkill_damage, result_enemy_hitpoint_rate, result_enemy_id, result_enemy_kill_count, result_player_unit_hitpoint_rate, result_player_unit_id, result_supply_id, result_supply_use_quantity, win), win, userErrorCallback);
  }

  private string toJsonString(
    int battle_turn,
    string battle_uuid,
    int overkill_damage,
    float[] result_enemy_hitpoint_rate,
    int[] result_enemy_id,
    int[] result_enemy_kill_count,
    float[] result_player_unit_hitpoint_rate,
    int[] result_player_unit_id,
    int[] result_supply_id,
    int[] result_supply_use_quantity,
    bool win)
  {
    string text = Json.Serialize((object) new Dictionary<string, object>()
    {
      [nameof (battle_turn)] = (object) battle_turn,
      [nameof (battle_uuid)] = (object) battle_uuid,
      [nameof (overkill_damage)] = (object) overkill_damage,
      [nameof (result_enemy_hitpoint_rate)] = (object) result_enemy_hitpoint_rate,
      [nameof (result_enemy_id)] = (object) result_enemy_id,
      [nameof (result_enemy_kill_count)] = (object) result_enemy_kill_count,
      [nameof (result_player_unit_hitpoint_rate)] = (object) result_player_unit_hitpoint_rate,
      [nameof (result_player_unit_id)] = (object) result_player_unit_id,
      [nameof (result_supply_id)] = (object) result_supply_id,
      [nameof (result_supply_use_quantity)] = (object) result_supply_use_quantity,
      [nameof (win)] = (object) win
    });
    return AES.Encrypt(battle_uuid, this.salt_, text);
  }
}
