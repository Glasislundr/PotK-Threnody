// Decompiled with JetBrains decompiler
// Type: BattleInfoUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public static class BattleInfoUtil
{
  public static IEnumerator doMakeExtraSoloBattleInfo(
    int playerUnitId,
    int questSId,
    Action<BattleInfo> createdBattleInfoCallback,
    Action<WebAPI.Response.UserError> userErrorCallback = null)
  {
    Future<WebAPI.Response.BattleExtraStart> future = WebAPI.BattleExtraStart(0, 4, playerUnitId, questSId, "", 0, userErrorCallback);
    yield return (object) future.Wait();
    if (future.Result == null)
      createdBattleInfoCallback((BattleInfo) null);
    else
      createdBattleInfoCallback(BattleInfoUtil.MakeBattleInfo(future.Result));
  }

  public static BattleInfo MakeBattleInfo(WebAPI.Response.BattleExtraStart starter)
  {
    PlayerHelper helper1;
    int[] guests;
    PlayerUnit[] user_units;
    if (starter.deck_type_id != 4)
    {
      for (int index = 0; index < starter.helpers.Length; ++index)
      {
        PlayerHelper helper2 = starter.helpers[index];
        helper2.leader_unit = starter.helper_player_units[index];
        helper2.leader_unit.importOverkillersUnits(starter.helper_player_unit_over_killers);
        helper2.leader_unit.primary_equipped_gear = helper2.leader_unit.FindEquippedGear(starter.helper_player_gears);
        helper2.leader_unit.primary_equipped_gear2 = helper2.leader_unit.FindEquippedGear2(starter.helper_player_gears);
        helper2.leader_unit.primary_equipped_gear3 = helper2.leader_unit.FindEquippedGear3(starter.helper_player_gears);
        helper2.leader_unit.primary_equipped_reisou = helper2.leader_unit.FindEquippedReisou(starter.helper_player_gears, starter.helper_player_reisou_gears);
        helper2.leader_unit.primary_equipped_reisou2 = helper2.leader_unit.FindEquippedReisou2(starter.helper_player_gears, starter.helper_player_reisou_gears);
        helper2.leader_unit.primary_equipped_reisou3 = helper2.leader_unit.FindEquippedReisou3(starter.helper_player_gears, starter.helper_player_reisou_gears);
        helper2.leader_unit.primary_equipped_awake_skill = helper2.leader_unit.FindEquippedExtraSkill(starter.helper_player_awake_skills);
      }
      helper1 = ((IEnumerable<PlayerHelper>) starter.helpers).FirstOrDefault<PlayerHelper>();
      guests = GuestUnit.GetGuestsID(starter.quest_s_id);
      user_units = starter.user_deck_units;
    }
    else
    {
      helper1 = (PlayerHelper) null;
      guests = new int[0];
      user_units = new PlayerUnit[1]
      {
        Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == starter.player_unit_id))
      };
    }
    return BattleInfo.MakeBattleInfo(starter.battle_uuid, (CommonQuestType) starter.quest_type, starter.quest_s_id, starter.deck_type_id, starter.quest_loop_count, starter.deck_number, helper1, starter.enemy, ((IEnumerable<WebAPI.Response.BattleExtraStartEnemy_item>) starter.enemy_item).Select<WebAPI.Response.BattleExtraStartEnemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleExtraStartEnemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), user_units, starter.user_deck_gears, starter.user_deck_enemy, ((IEnumerable<WebAPI.Response.BattleExtraStartUser_deck_enemy_item>) starter.user_deck_enemy_item).Select<WebAPI.Response.BattleExtraStartUser_deck_enemy_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleExtraStartUser_deck_enemy_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), starter.panel, ((IEnumerable<WebAPI.Response.BattleExtraStartPanel_item>) starter.panel_item).Select<WebAPI.Response.BattleExtraStartPanel_item, Tuple<int, int, int, int>>((Func<WebAPI.Response.BattleExtraStartPanel_item, Tuple<int, int, int, int>>) (x => Tuple.Create<int, int, int, int>(x.id, x.reward_type_id, x.reward_id, x.reward_quantity))).ToArray<Tuple<int, int, int, int>>(), guests, (PlayerUnit[]) null, (Tuple<int, int>[]) null);
  }
}
