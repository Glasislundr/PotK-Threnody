// Decompiled with JetBrains decompiler
// Type: SM.BattleEnd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class BattleEnd : KeyCompare
  {
    public int player_mvp_unit_id;
    public BattleEndGain_trust_info[] gain_trust_info;
    public BattleEndDrop_gear_entities[] drop_gear_entities;
    public BattleEndDrop_unit_type_ticket_entities[] drop_unit_type_ticket_entities;
    public int incr_friend_point;
    public int player_incr_exp;
    public BattleEndPlayer_character_intimates_in_battle[] player_character_intimates_in_battle;
    public PlayerItem[] after_player_gears;
    public BattleEndStage_clear_rewards[] stage_clear_rewards;
    public long player_incr_money;
    public BattleEndDrop_common_ticket_entities[] drop_common_ticket_entities;
    public BattleEndUnlock_messages[] unlock_messages;
    public BattleEndDrop_material_unit_entities[] drop_material_unit_entities;
    public EventBattleFinish[] events;
    public int deck_number;
    public BattleEndGet_sea_album_piece_counts[] get_sea_album_piece_counts;
    public int deck_type_id;
    public PlayerHelper[] battle_helpers;
    public BattleEndMission_complete_rewards[] mission_complete_rewards;
    public BattleEndDrop_unit_ticket_entities[] drop_unit_ticket_entities;
    public UnlockQuest[] unlock_quests;
    public int[] disappeared_player_gears;
    public int[] gettable_piece_same_character_ids;
    public Player before_player;
    public QuestScoreBattleFinishContext[] score_campaigns;
    public BattleEndBoost_stage_clear_rewards[] boost_stage_clear_rewards;
    public PlayerMissionHistory[] player_mission_results;
    public PlayerUnit[] before_player_units;
    public int[] receive_sea_album_ids;
    public BattleEndDrop_material_gear_entities[] drop_material_gear_entities;
    public BattleEndDrop_unit_entities[] drop_unit_entities;
    public PlayerItem[] before_player_gears;
    public PlayerUnit[] after_player_units;
    public BattleEndTrust_upper_limit[] trust_upper_limit;
    public BattleEndDrop_quest_key_entities[] drop_quest_key_entities;
    public BattleEndDrop_supply_entities[] drop_supply_entities;
    public UnlockIntimateSkill[] unlock_intimate_skills;
    public BattleEndDrop_gacha_ticket_entities[] drop_gacha_ticket_entities;
    public BattleEndPlayer_review player_review;

    public BattleEnd()
    {
    }

    public BattleEnd(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.player_mvp_unit_id = (int) (long) json[nameof (player_mvp_unit_id)];
      List<BattleEndGain_trust_info> endGainTrustInfoList = new List<BattleEndGain_trust_info>();
      foreach (object json1 in (List<object>) json[nameof (gain_trust_info)])
        endGainTrustInfoList.Add(json1 == null ? (BattleEndGain_trust_info) null : new BattleEndGain_trust_info((Dictionary<string, object>) json1));
      this.gain_trust_info = endGainTrustInfoList.ToArray();
      List<BattleEndDrop_gear_entities> dropGearEntitiesList = new List<BattleEndDrop_gear_entities>();
      foreach (object json2 in (List<object>) json[nameof (drop_gear_entities)])
        dropGearEntitiesList.Add(json2 == null ? (BattleEndDrop_gear_entities) null : new BattleEndDrop_gear_entities((Dictionary<string, object>) json2));
      this.drop_gear_entities = dropGearEntitiesList.ToArray();
      List<BattleEndDrop_unit_type_ticket_entities> typeTicketEntitiesList = new List<BattleEndDrop_unit_type_ticket_entities>();
      foreach (object json3 in (List<object>) json[nameof (drop_unit_type_ticket_entities)])
        typeTicketEntitiesList.Add(json3 == null ? (BattleEndDrop_unit_type_ticket_entities) null : new BattleEndDrop_unit_type_ticket_entities((Dictionary<string, object>) json3));
      this.drop_unit_type_ticket_entities = typeTicketEntitiesList.ToArray();
      this.incr_friend_point = (int) (long) json[nameof (incr_friend_point)];
      this.player_incr_exp = (int) (long) json[nameof (player_incr_exp)];
      List<BattleEndPlayer_character_intimates_in_battle> intimatesInBattleList = new List<BattleEndPlayer_character_intimates_in_battle>();
      foreach (object json4 in (List<object>) json[nameof (player_character_intimates_in_battle)])
        intimatesInBattleList.Add(json4 == null ? (BattleEndPlayer_character_intimates_in_battle) null : new BattleEndPlayer_character_intimates_in_battle((Dictionary<string, object>) json4));
      this.player_character_intimates_in_battle = intimatesInBattleList.ToArray();
      List<PlayerItem> playerItemList1 = new List<PlayerItem>();
      foreach (object json5 in (List<object>) json[nameof (after_player_gears)])
        playerItemList1.Add(json5 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json5));
      this.after_player_gears = playerItemList1.ToArray();
      List<BattleEndStage_clear_rewards> stageClearRewardsList1 = new List<BattleEndStage_clear_rewards>();
      foreach (object json6 in (List<object>) json[nameof (stage_clear_rewards)])
        stageClearRewardsList1.Add(json6 == null ? (BattleEndStage_clear_rewards) null : new BattleEndStage_clear_rewards((Dictionary<string, object>) json6));
      this.stage_clear_rewards = stageClearRewardsList1.ToArray();
      this.player_incr_money = (long) json[nameof (player_incr_money)];
      List<BattleEndDrop_common_ticket_entities> commonTicketEntitiesList = new List<BattleEndDrop_common_ticket_entities>();
      foreach (object json7 in (List<object>) json[nameof (drop_common_ticket_entities)])
        commonTicketEntitiesList.Add(json7 == null ? (BattleEndDrop_common_ticket_entities) null : new BattleEndDrop_common_ticket_entities((Dictionary<string, object>) json7));
      this.drop_common_ticket_entities = commonTicketEntitiesList.ToArray();
      List<BattleEndUnlock_messages> endUnlockMessagesList = new List<BattleEndUnlock_messages>();
      foreach (object json8 in (List<object>) json[nameof (unlock_messages)])
        endUnlockMessagesList.Add(json8 == null ? (BattleEndUnlock_messages) null : new BattleEndUnlock_messages((Dictionary<string, object>) json8));
      this.unlock_messages = endUnlockMessagesList.ToArray();
      List<BattleEndDrop_material_unit_entities> materialUnitEntitiesList = new List<BattleEndDrop_material_unit_entities>();
      foreach (object json9 in (List<object>) json[nameof (drop_material_unit_entities)])
        materialUnitEntitiesList.Add(json9 == null ? (BattleEndDrop_material_unit_entities) null : new BattleEndDrop_material_unit_entities((Dictionary<string, object>) json9));
      this.drop_material_unit_entities = materialUnitEntitiesList.ToArray();
      List<EventBattleFinish> eventBattleFinishList = new List<EventBattleFinish>();
      foreach (object json10 in (List<object>) json[nameof (events)])
        eventBattleFinishList.Add(json10 == null ? (EventBattleFinish) null : new EventBattleFinish((Dictionary<string, object>) json10));
      this.events = eventBattleFinishList.ToArray();
      this.deck_number = (int) (long) json[nameof (deck_number)];
      List<BattleEndGet_sea_album_piece_counts> albumPieceCountsList = new List<BattleEndGet_sea_album_piece_counts>();
      foreach (object json11 in (List<object>) json[nameof (get_sea_album_piece_counts)])
        albumPieceCountsList.Add(json11 == null ? (BattleEndGet_sea_album_piece_counts) null : new BattleEndGet_sea_album_piece_counts((Dictionary<string, object>) json11));
      this.get_sea_album_piece_counts = albumPieceCountsList.ToArray();
      this.deck_type_id = (int) (long) json[nameof (deck_type_id)];
      List<PlayerHelper> playerHelperList = new List<PlayerHelper>();
      foreach (object json12 in (List<object>) json[nameof (battle_helpers)])
        playerHelperList.Add(json12 == null ? (PlayerHelper) null : new PlayerHelper((Dictionary<string, object>) json12));
      this.battle_helpers = playerHelperList.ToArray();
      List<BattleEndMission_complete_rewards> missionCompleteRewardsList = new List<BattleEndMission_complete_rewards>();
      foreach (object json13 in (List<object>) json[nameof (mission_complete_rewards)])
        missionCompleteRewardsList.Add(json13 == null ? (BattleEndMission_complete_rewards) null : new BattleEndMission_complete_rewards((Dictionary<string, object>) json13));
      this.mission_complete_rewards = missionCompleteRewardsList.ToArray();
      List<BattleEndDrop_unit_ticket_entities> unitTicketEntitiesList = new List<BattleEndDrop_unit_ticket_entities>();
      foreach (object json14 in (List<object>) json[nameof (drop_unit_ticket_entities)])
        unitTicketEntitiesList.Add(json14 == null ? (BattleEndDrop_unit_ticket_entities) null : new BattleEndDrop_unit_ticket_entities((Dictionary<string, object>) json14));
      this.drop_unit_ticket_entities = unitTicketEntitiesList.ToArray();
      List<UnlockQuest> unlockQuestList = new List<UnlockQuest>();
      foreach (object json15 in (List<object>) json[nameof (unlock_quests)])
        unlockQuestList.Add(json15 == null ? (UnlockQuest) null : new UnlockQuest((Dictionary<string, object>) json15));
      this.unlock_quests = unlockQuestList.ToArray();
      this.disappeared_player_gears = ((IEnumerable<object>) json[nameof (disappeared_player_gears)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.gettable_piece_same_character_ids = ((IEnumerable<object>) json[nameof (gettable_piece_same_character_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.before_player = json[nameof (before_player)] == null ? (Player) null : new Player((Dictionary<string, object>) json[nameof (before_player)]);
      List<QuestScoreBattleFinishContext> battleFinishContextList = new List<QuestScoreBattleFinishContext>();
      foreach (object json16 in (List<object>) json[nameof (score_campaigns)])
        battleFinishContextList.Add(json16 == null ? (QuestScoreBattleFinishContext) null : new QuestScoreBattleFinishContext((Dictionary<string, object>) json16));
      this.score_campaigns = battleFinishContextList.ToArray();
      List<BattleEndBoost_stage_clear_rewards> stageClearRewardsList2 = new List<BattleEndBoost_stage_clear_rewards>();
      foreach (object json17 in (List<object>) json[nameof (boost_stage_clear_rewards)])
        stageClearRewardsList2.Add(json17 == null ? (BattleEndBoost_stage_clear_rewards) null : new BattleEndBoost_stage_clear_rewards((Dictionary<string, object>) json17));
      this.boost_stage_clear_rewards = stageClearRewardsList2.ToArray();
      List<PlayerMissionHistory> playerMissionHistoryList = new List<PlayerMissionHistory>();
      foreach (object json18 in (List<object>) json[nameof (player_mission_results)])
        playerMissionHistoryList.Add(json18 == null ? (PlayerMissionHistory) null : new PlayerMissionHistory((Dictionary<string, object>) json18));
      this.player_mission_results = playerMissionHistoryList.ToArray();
      List<PlayerUnit> playerUnitList1 = new List<PlayerUnit>();
      foreach (object json19 in (List<object>) json[nameof (before_player_units)])
        playerUnitList1.Add(json19 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json19));
      this.before_player_units = playerUnitList1.ToArray();
      this.receive_sea_album_ids = ((IEnumerable<object>) json[nameof (receive_sea_album_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      List<BattleEndDrop_material_gear_entities> materialGearEntitiesList = new List<BattleEndDrop_material_gear_entities>();
      foreach (object json20 in (List<object>) json[nameof (drop_material_gear_entities)])
        materialGearEntitiesList.Add(json20 == null ? (BattleEndDrop_material_gear_entities) null : new BattleEndDrop_material_gear_entities((Dictionary<string, object>) json20));
      this.drop_material_gear_entities = materialGearEntitiesList.ToArray();
      List<BattleEndDrop_unit_entities> dropUnitEntitiesList = new List<BattleEndDrop_unit_entities>();
      foreach (object json21 in (List<object>) json[nameof (drop_unit_entities)])
        dropUnitEntitiesList.Add(json21 == null ? (BattleEndDrop_unit_entities) null : new BattleEndDrop_unit_entities((Dictionary<string, object>) json21));
      this.drop_unit_entities = dropUnitEntitiesList.ToArray();
      List<PlayerItem> playerItemList2 = new List<PlayerItem>();
      foreach (object json22 in (List<object>) json[nameof (before_player_gears)])
        playerItemList2.Add(json22 == null ? (PlayerItem) null : new PlayerItem((Dictionary<string, object>) json22));
      this.before_player_gears = playerItemList2.ToArray();
      List<PlayerUnit> playerUnitList2 = new List<PlayerUnit>();
      foreach (object json23 in (List<object>) json[nameof (after_player_units)])
        playerUnitList2.Add(json23 == null ? (PlayerUnit) null : new PlayerUnit((Dictionary<string, object>) json23));
      this.after_player_units = playerUnitList2.ToArray();
      List<BattleEndTrust_upper_limit> endTrustUpperLimitList = new List<BattleEndTrust_upper_limit>();
      foreach (object json24 in (List<object>) json[nameof (trust_upper_limit)])
        endTrustUpperLimitList.Add(json24 == null ? (BattleEndTrust_upper_limit) null : new BattleEndTrust_upper_limit((Dictionary<string, object>) json24));
      this.trust_upper_limit = endTrustUpperLimitList.ToArray();
      List<BattleEndDrop_quest_key_entities> questKeyEntitiesList = new List<BattleEndDrop_quest_key_entities>();
      foreach (object json25 in (List<object>) json[nameof (drop_quest_key_entities)])
        questKeyEntitiesList.Add(json25 == null ? (BattleEndDrop_quest_key_entities) null : new BattleEndDrop_quest_key_entities((Dictionary<string, object>) json25));
      this.drop_quest_key_entities = questKeyEntitiesList.ToArray();
      List<BattleEndDrop_supply_entities> dropSupplyEntitiesList = new List<BattleEndDrop_supply_entities>();
      foreach (object json26 in (List<object>) json[nameof (drop_supply_entities)])
        dropSupplyEntitiesList.Add(json26 == null ? (BattleEndDrop_supply_entities) null : new BattleEndDrop_supply_entities((Dictionary<string, object>) json26));
      this.drop_supply_entities = dropSupplyEntitiesList.ToArray();
      List<UnlockIntimateSkill> unlockIntimateSkillList = new List<UnlockIntimateSkill>();
      foreach (object json27 in (List<object>) json[nameof (unlock_intimate_skills)])
        unlockIntimateSkillList.Add(json27 == null ? (UnlockIntimateSkill) null : new UnlockIntimateSkill((Dictionary<string, object>) json27));
      this.unlock_intimate_skills = unlockIntimateSkillList.ToArray();
      List<BattleEndDrop_gacha_ticket_entities> gachaTicketEntitiesList = new List<BattleEndDrop_gacha_ticket_entities>();
      foreach (object json28 in (List<object>) json[nameof (drop_gacha_ticket_entities)])
        gachaTicketEntitiesList.Add(json28 == null ? (BattleEndDrop_gacha_ticket_entities) null : new BattleEndDrop_gacha_ticket_entities((Dictionary<string, object>) json28));
      this.drop_gacha_ticket_entities = gachaTicketEntitiesList.ToArray();
      this.player_review = json[nameof (player_review)] == null ? (BattleEndPlayer_review) null : new BattleEndPlayer_review((Dictionary<string, object>) json[nameof (player_review)]);
    }
  }
}
