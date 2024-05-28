// Decompiled with JetBrains decompiler
// Type: GameCore.ColosseumInitialData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;

#nullable disable
namespace GameCore
{
  public class ColosseumInitialData
  {
    public string transaction_id;
    public PlayerUnit[] player_unit_list;
    public PlayerItem[] player_gear_list;
    public PlayerGearReisouSchema[] player_reisou_list;
    public PlayerAwakeSkill[] player_extra_skill_list;
    public PlayerUnit[] opponent_unit_list;
    public PlayerItem[] opponent_gear_list;
    public PlayerGearReisouSchema[] opponent_reisou_list;
    public PlayerAwakeSkill[] opponent_extra_skill_list;
    public Bonus[] bonusList;
    public DateTime now;

    public ColosseumInitialData()
    {
      this.transaction_id = "dummiy_date";
      this.player_unit_list = SMManager.Get<PlayerDeck[]>()[0].player_units;
      this.player_gear_list = SMManager.Get<PlayerItem[]>();
      this.player_reisou_list = SMManager.Get<PlayerGearReisouSchema[]>();
      this.player_extra_skill_list = SMManager.Get<PlayerAwakeSkill[]>();
      this.opponent_unit_list = SMManager.Get<PlayerDeck[]>()[0].player_units;
      this.opponent_gear_list = SMManager.Get<PlayerItem[]>();
      this.opponent_reisou_list = SMManager.Get<PlayerGearReisouSchema[]>();
      this.opponent_extra_skill_list = SMManager.Get<PlayerAwakeSkill[]>();
      this.bonusList = (Bonus[]) null;
      this.now = DateTime.Now;
    }

    public ColosseumInitialData(WebAPI.Response.ColosseumStart response, int bonusTypeID)
    {
      this.transaction_id = response.arena_transaction_id;
      this.player_unit_list = response.colosseum_player_units;
      this.attachOverkillers(this.player_unit_list, response.colosseum_player_over_killers);
      this.player_gear_list = response.colosseum_player_items;
      this.player_reisou_list = response.colosseum_player_reisou_items;
      this.player_extra_skill_list = response.colosseum_player_awake_skills;
      this.opponent_unit_list = response.colosseum_target_player_units;
      this.attachOverkillers(this.opponent_unit_list, response.colosseum_target_player_over_killers);
      this.opponent_gear_list = response.colosseum_target_player_items;
      this.opponent_reisou_list = response.colosseum_target_player_reisou_items;
      this.opponent_extra_skill_list = response.colosseum_target_player_awake_skills;
      this.bonusList = response.bonus;
      this.now = response.now;
    }

    private void attachOverkillers(PlayerUnit[] playerUnits, PlayerUnit[] overkillers)
    {
      if (playerUnits == null)
        return;
      for (int index = 0; index < playerUnits.Length; ++index)
      {
        if (playerUnits[index] != (PlayerUnit) null)
          playerUnits[index].importOverkillersUnits(overkillers);
      }
    }

    public ColosseumInitialData(WebAPI.Response.ColosseumTutorialStart response, int bonusTypeID)
    {
      this.transaction_id = response.arena_transaction_id;
      this.player_unit_list = response.colosseum_player_units;
      this.attachOverkillers(this.player_unit_list, response.colosseum_player_over_killers);
      this.player_gear_list = response.colosseum_player_items;
      this.player_reisou_list = response.colosseum_player_reisou_items;
      this.player_extra_skill_list = response.colosseum_player_awake_skills;
      this.opponent_unit_list = response.colosseum_target_player_units;
      this.attachOverkillers(this.opponent_unit_list, response.colosseum_target_player_over_killers);
      this.opponent_gear_list = response.colosseum_target_player_items;
      this.opponent_reisou_list = response.colosseum_target_player_reisou_items;
      this.opponent_extra_skill_list = response.colosseum_target_player_awake_skills;
      this.bonusList = response.bonus;
      this.now = response.now;
    }

    public ColosseumInitialData(WebAPI.Response.ColosseumResume response, int bonusTypeID)
    {
      this.transaction_id = response.arena_transaction_id;
      this.player_unit_list = response.colosseum_player_units;
      this.attachOverkillers(this.player_unit_list, response.colosseum_player_over_killers);
      this.player_gear_list = response.colosseum_player_items;
      this.player_reisou_list = response.colosseum_player_reisou_items;
      this.player_extra_skill_list = response.colosseum_player_awake_skills;
      this.opponent_unit_list = response.colosseum_target_player_units;
      this.attachOverkillers(this.opponent_unit_list, response.colosseum_target_player_over_killers);
      this.opponent_gear_list = response.colosseum_target_player_items;
      this.opponent_reisou_list = response.colosseum_target_player_reisou_items;
      this.opponent_extra_skill_list = response.colosseum_target_player_awake_skills;
      this.bonusList = response.bonus;
      this.now = response.now;
    }

    public ColosseumInitialData(WebAPI.Response.ColosseumTutorialResume response, int bonusTypeID)
    {
      this.transaction_id = response.arena_transaction_id;
      this.player_unit_list = response.colosseum_player_units;
      this.attachOverkillers(this.player_unit_list, response.colosseum_player_over_killers);
      this.player_gear_list = response.colosseum_player_items;
      this.player_reisou_list = response.colosseum_player_reisou_items;
      this.player_extra_skill_list = response.colosseum_player_awake_skills;
      this.opponent_unit_list = response.colosseum_target_player_units;
      this.attachOverkillers(this.opponent_unit_list, response.colosseum_target_player_over_killers);
      this.opponent_gear_list = response.colosseum_target_player_items;
      this.opponent_reisou_list = response.colosseum_target_player_reisou_items;
      this.opponent_extra_skill_list = response.colosseum_target_player_awake_skills;
      this.bonusList = response.bonus;
      this.now = response.now;
    }

    public ColosseumInitialData(WebAPI.Response.ExploreChallengeStart response, int bonusTypeID)
    {
      this.transaction_id = response.arena_transaction_id;
      this.player_unit_list = response.challenge_player_units;
      this.player_gear_list = response.challenge_player_items;
      this.player_reisou_list = response.colosseum_player_reisou_items;
      this.player_extra_skill_list = response.challenge_player_awake_skills;
      this.opponent_unit_list = response.challenge_target_player_units;
      this.opponent_gear_list = response.challenge_target_player_items;
      this.opponent_reisou_list = response.colosseum_target_player_reisou_items;
      this.opponent_extra_skill_list = response.challenge_target_player_awake_skills;
      this.bonusList = (Bonus[]) null;
      this.now = response.now;
    }
  }
}
