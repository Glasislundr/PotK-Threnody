// Decompiled with JetBrains decompiler
// Type: SM.Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class Player : KeyCompare
  {
    private static int? ApOverChargeLimit;
    public string comment;
    public int[] call_skill_values;
    public int bp_full_remain;
    public int mp_max;
    public long money;
    public int ap_max;
    public string short_id;
    public int ap_auto_healing_sec;
    public int ap_full_remain;
    public int max_friends;
    public int ap;
    public int battle_medal;
    public int max_units;
    public int friend_point;
    public int[] call_skill_same_character_ids;
    public int continuation_date;
    public int max_items_cap;
    public int next_panel_mission_id;
    public bool is_bingo_end;
    public int max_cost;
    public int max_units_cap;
    public string id;
    public int common_coin;
    public DateTime mp_full_recovery_at;
    public int? game_over_count;
    public bool is_open_bingo;
    public int mp;
    public int ap_overflow;
    public int ext_max_unit_reserves;
    public int friends_count;
    public bool is_open_mission;
    public int bp_max;
    public int bp;
    public int raid_medal;
    public int max_reisou_items_cap;
    public int max_reisou_items;
    public int medal;
    public int exp_next;
    public string name;
    public string extension;
    public int level;
    public int free_coin;
    public int max_unit_reserves;
    public int max_items;
    public int total_exp;
    public PlayerGachaTicket[] gacha_tickets;
    public int max_friends_cap;
    public int reisou_jewel;
    public int max_unit_reserves_cap;
    public int exp;
    public int current_emblem_id;
    public int bp_auto_healing_sec;
    public int paid_coin;
    public PlayerClear_daily_mission_ids clear_daily_mission_ids;
    public PlayerCallDivorceHistory[] call_divorce_histories;

    public Player Clone() => (Player) this.MemberwiseClone();

    public static Player Current => SMManager.Get<Player>();

    public int coin => this.free_coin + this.paid_coin + this.common_coin;

    public int free_common_coin => this.free_coin + this.common_coin;

    public bool CheckZeny(int useZeny) => this.money >= (long) useZeny;

    public bool CheckLimitMaxItem() => this.max_items >= this.max_items_cap;

    public bool ExpandLimitItem(int num)
    {
      if (this.CheckLimitMaxItem())
        return false;
      this.max_items += num;
      return true;
    }

    public bool CheckMaxHavingGear()
    {
      return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && !x.isReisou())).Count<PlayerItem>() >= this.max_items;
    }

    public bool CheckMaxHavingReisou()
    {
      return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && x.isReisou())).Count<PlayerItem>() >= this.max_reisou_items;
    }

    public bool CheckLimitMaxUnit() => this.max_units >= this.max_units_cap;

    public bool CheckMaxHavingUnit() => SMManager.Get<PlayerUnit[]>().Length >= this.max_units;

    public bool CheckCapMaxUnit(int num) => num >= this.max_units_cap;

    public bool CheckCapMaxUnit() => this.CheckCapMaxUnit(SMManager.Get<PlayerUnit[]>().Length);

    public bool CheckLimitOverCapMaxUnit(int num) => num > this.max_units_cap;

    public bool CheckLimitOverMaxUnitReserves()
    {
      return SMManager.Get<PlayerUnitReservesCount>().count > this.max_unit_reserves;
    }

    public bool CheckLimitOverExtMaxUnitReserves(int num) => num > this.ext_max_unit_reserves;

    public bool CheckKiseki(int num) => this.coin >= num;

    public bool CheckCompensationKiseki(int num) => this.paid_coin >= num;

    public bool CheckFrendPoint(int num) => this.friend_point >= num;

    public bool CheckApFull() => this.ap >= this.ap_max;

    public bool CheckAp(int num) => this.ap >= num;

    private bool ContainsExtension(string str)
    {
      bool res = false;
      ((IEnumerable<string>) this.extension.Split(':')).ForEach<string>((Action<string>) (x =>
      {
        if (!(x == str))
          return;
        res = true;
      }));
      return res;
    }

    public bool GetFeatureColosseum() => true;

    public bool GetReleaseColosseum() => true;

    public bool GetReleaseSlot() => this.ContainsExtension("is_open_slot");

    public bool GetFeatureColosseumRanking() => this.ContainsExtension("colosseum_ranking");

    public bool IsColosseum() => this.GetFeatureColosseum() && this.GetReleaseColosseum();

    public bool IsPvp() => this.ContainsExtension("pvp");

    public bool IsClassMatch() => this.ContainsExtension("class_match");

    public bool IsClassMatchRanking() => this.ContainsExtension("class_ranking");

    public bool IsClassMatchShowRanking() => this.ContainsExtension("class_show_ranking");

    public bool IsMission() => this.ContainsExtension("dailymission");

    public bool IsCombiQuest() => this.ContainsExtension("combi_quest");

    public bool IsGearRecipe() => this.ContainsExtension("gear_recipe");

    public bool IsGearBuildup() => this.ContainsExtension("gear_buildup");

    public bool IsEnableDarkAndHoly() => this.ContainsExtension("enable_light_darkness");

    public bool IsUniteReinfoce() => this.ContainsExtension("unit_buildup");

    public bool IsGearDrilling() => this.ContainsExtension("gear_drilling");

    public bool IsGuildOpen() => this.ContainsExtension("guild");

    public bool IsGuildMatingOpen() => this.ContainsExtension("gvg");

    public bool IsLoginBonusMonthly() => this.ContainsExtension("loginbonus_monthly");

    public bool IsSea() => this.ContainsExtension("sea");

    public bool IsSeaDate() => this.ContainsExtension("sea_date");

    public static int GetApOverChargeLimit()
    {
      if (!Player.ApOverChargeLimit.HasValue)
      {
        int result = 0;
        int.TryParse(Consts.GetInstance().OVERCHARGE_LIMIT, out result);
        Player.ApOverChargeLimit = new int?(result);
      }
      return Player.ApOverChargeLimit.Value;
    }

    public BattleInfo.CallSkillParam GetCallSkillParam()
    {
      BattleInfo.CallSkillParam callSkillParam = new BattleInfo.CallSkillParam();
      callSkillParam.same_character_id = 0;
      callSkillParam.intimate_rank = 0;
      if (this.call_skill_same_character_ids.Length != 0 && this.call_skill_values.Length != 0)
      {
        callSkillParam.same_character_id = this.call_skill_same_character_ids[0];
        callSkillParam.intimate_rank = this.call_skill_values[0];
      }
      callSkillParam.player_rank = this.level;
      return callSkillParam;
    }

    public bool IsCalledUnit(UnitUnit unit)
    {
      return unit != null && this.IsCalledUnit(unit.same_character_id);
    }

    public bool IsCalledUnit(int same_character_id)
    {
      return ((IEnumerable<int>) this.call_skill_same_character_ids).Contains<int>(same_character_id);
    }

    public Player()
    {
    }

    public Player(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.comment = (string) json[nameof (comment)];
      this.call_skill_values = ((IEnumerable<object>) json[nameof (call_skill_values)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.bp_full_remain = (int) (long) json[nameof (bp_full_remain)];
      this.mp_max = (int) (long) json[nameof (mp_max)];
      this.money = (long) json[nameof (money)];
      this.ap_max = (int) (long) json[nameof (ap_max)];
      this.short_id = (string) json[nameof (short_id)];
      this.ap_auto_healing_sec = (int) (long) json[nameof (ap_auto_healing_sec)];
      this.ap_full_remain = (int) (long) json[nameof (ap_full_remain)];
      this.max_friends = (int) (long) json[nameof (max_friends)];
      this.ap = (int) (long) json[nameof (ap)];
      this.battle_medal = (int) (long) json[nameof (battle_medal)];
      this.max_units = (int) (long) json[nameof (max_units)];
      this.friend_point = (int) (long) json[nameof (friend_point)];
      this.call_skill_same_character_ids = ((IEnumerable<object>) json[nameof (call_skill_same_character_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.continuation_date = (int) (long) json[nameof (continuation_date)];
      this.max_items_cap = (int) (long) json[nameof (max_items_cap)];
      this.next_panel_mission_id = (int) (long) json[nameof (next_panel_mission_id)];
      this.is_bingo_end = (bool) json[nameof (is_bingo_end)];
      this.max_cost = (int) (long) json[nameof (max_cost)];
      this.max_units_cap = (int) (long) json[nameof (max_units_cap)];
      this.id = (string) json[nameof (id)];
      this.common_coin = (int) (long) json[nameof (common_coin)];
      this.mp_full_recovery_at = DateTime.Parse((string) json[nameof (mp_full_recovery_at)]);
      int? nullable1;
      if (json[nameof (game_over_count)] != null)
      {
        long? nullable2 = (long?) json[nameof (game_over_count)];
        nullable1 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      }
      else
        nullable1 = new int?();
      this.game_over_count = nullable1;
      this.is_open_bingo = (bool) json[nameof (is_open_bingo)];
      this.mp = (int) (long) json[nameof (mp)];
      this.ap_overflow = (int) (long) json[nameof (ap_overflow)];
      this.ext_max_unit_reserves = (int) (long) json[nameof (ext_max_unit_reserves)];
      this.friends_count = (int) (long) json[nameof (friends_count)];
      this.is_open_mission = (bool) json[nameof (is_open_mission)];
      this.bp_max = (int) (long) json[nameof (bp_max)];
      this.bp = (int) (long) json[nameof (bp)];
      this.raid_medal = (int) (long) json[nameof (raid_medal)];
      this.max_reisou_items_cap = (int) (long) json[nameof (max_reisou_items_cap)];
      this.max_reisou_items = (int) (long) json[nameof (max_reisou_items)];
      this.medal = (int) (long) json[nameof (medal)];
      this.exp_next = (int) (long) json[nameof (exp_next)];
      this.name = (string) json[nameof (name)];
      this.extension = (string) json[nameof (extension)];
      this.level = (int) (long) json[nameof (level)];
      this.free_coin = (int) (long) json[nameof (free_coin)];
      this.max_unit_reserves = (int) (long) json[nameof (max_unit_reserves)];
      this.max_items = (int) (long) json[nameof (max_items)];
      this.total_exp = (int) (long) json[nameof (total_exp)];
      List<PlayerGachaTicket> playerGachaTicketList = new List<PlayerGachaTicket>();
      foreach (object json1 in (List<object>) json[nameof (gacha_tickets)])
        playerGachaTicketList.Add(json1 == null ? (PlayerGachaTicket) null : new PlayerGachaTicket((Dictionary<string, object>) json1));
      this.gacha_tickets = playerGachaTicketList.ToArray();
      this.max_friends_cap = (int) (long) json[nameof (max_friends_cap)];
      this.reisou_jewel = (int) (long) json[nameof (reisou_jewel)];
      this.max_unit_reserves_cap = (int) (long) json[nameof (max_unit_reserves_cap)];
      this.exp = (int) (long) json[nameof (exp)];
      this.current_emblem_id = (int) (long) json[nameof (current_emblem_id)];
      this.bp_auto_healing_sec = (int) (long) json[nameof (bp_auto_healing_sec)];
      this.paid_coin = (int) (long) json[nameof (paid_coin)];
      this.clear_daily_mission_ids = json[nameof (clear_daily_mission_ids)] == null ? (PlayerClear_daily_mission_ids) null : new PlayerClear_daily_mission_ids((Dictionary<string, object>) json[nameof (clear_daily_mission_ids)]);
      List<PlayerCallDivorceHistory> callDivorceHistoryList = new List<PlayerCallDivorceHistory>();
      foreach (object json2 in (List<object>) json[nameof (call_divorce_histories)])
        callDivorceHistoryList.Add(json2 == null ? (PlayerCallDivorceHistory) null : new PlayerCallDivorceHistory((Dictionary<string, object>) json2));
      this.call_divorce_histories = callDivorceHistoryList.ToArray();
    }
  }
}
