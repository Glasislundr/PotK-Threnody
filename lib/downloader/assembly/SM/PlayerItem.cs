// Decompiled with JetBrains decompiler
// Type: SM.PlayerItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using System.Reflection;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerItem : KeyCompare
  {
    private long? _playerUnitRevision;
    private bool _unitEquipped;
    private const string nameFormat = "{0}+{1}";
    private GearRankIncr _gearRankCache;
    private ReisouRankIncr _reisouRankCache;
    private ReisouRankIncr _mythologyReisouHolyRankCache;
    private int reisouHolyLv;
    private ReisouRankIncr _mythologyReisouChaosRankCache;
    private int reisouChaosLv;
    private PlayerMythologyGearStatus _playerMythologyGearStatusCache;
    public const int DEFAULT_LIMIT = 5;
    public int _entity_type;
    public int box_type_id;
    public bool broken;
    public int entity_id;
    public int equipped_reisou_player_gear_id;
    public bool favorite;
    public bool for_battle;
    public int gear_accessory_remaining_amount;
    public int gear_exp;
    public int gear_exp_next;
    public int gear_level;
    public int gear_level_limit;
    public int gear_level_limit_max;
    public int gear_level_unlimit;
    public int gear_total_exp;
    public int id;
    public bool is_new;
    public string player_id;
    public int quantity;

    public PlayerItem.BoxType boxType => (PlayerItem.BoxType) this.box_type_id;

    public PlayerItem(GearGear gear, MasterDataTable.CommonRewardType entity_type)
    {
      this.Set(gear, entity_type);
    }

    public PlayerItem(GearGear gear, PlayerMythologyGearStatus mythology_status)
    {
      this.Set(gear, MasterDataTable.CommonRewardType.gear);
      if (this.isHolyReisou())
      {
        this.gear_level = mythology_status.holy_gear_level;
        this.gear_level_limit = mythology_status.holy_gear_level_limit;
        this.gear_exp = mythology_status.holy_gear_exp;
        this.gear_exp_next = mythology_status.holy_gear_exp_next;
      }
      else
      {
        this.gear_level = mythology_status.chaos_gear_level;
        this.gear_level_limit = mythology_status.chaos_gear_level_limit;
        this.gear_exp = mythology_status.chaos_gear_exp;
        this.gear_exp_next = mythology_status.chaos_gear_exp_next;
      }
    }

    private void Set(GearGear gear, MasterDataTable.CommonRewardType entity_type)
    {
      this._hasKey = false;
      this.gear_accessory_remaining_amount = 0;
      this.entity_id = gear.ID;
      this.for_battle = false;
      this.box_type_id = 0;
      this._entity_type = (int) entity_type;
      this.equipped_reisou_player_gear_id = 0;
      this.favorite = false;
      this.gear_exp_next = 0;
      this.is_new = false;
      this.broken = false;
      this.player_id = "";
      this.gear_level_unlimit = 0;
      this.gear_level = 1;
      this.gear_level_limit_max = 1;
      this.gear_total_exp = 0;
      this.gear_exp = 0;
      this._key = (object) (this.id = 0);
      this.gear_level_limit = 0;
      this.quantity = 1;
    }

    public override bool Equals(object rhs) => this.Equals(rhs as PlayerItem);

    public override int GetHashCode() => 0;

    public bool Equals(PlayerItem rhs)
    {
      if ((object) rhs == null)
        return false;
      if ((object) this == (object) rhs)
        return true;
      return !(this.GetType() != rhs.GetType()) && this.id == rhs.id && this.entity_type == rhs.entity_type && this.player_id == rhs.player_id;
    }

    public static bool operator ==(PlayerItem lhs, PlayerItem rhs)
    {
      return (object) lhs == null ? (object) rhs == null : lhs.Equals(rhs);
    }

    public static bool operator !=(PlayerItem lhs, PlayerItem rhs) => !(lhs == rhs);

    private bool UnitEquipped
    {
      get
      {
        long num1 = SMManager.Revision<PlayerUnit[]>();
        if (this._playerUnitRevision.HasValue)
        {
          long? playerUnitRevision = this._playerUnitRevision;
          long num2 = num1;
          if (playerUnitRevision.GetValueOrDefault() == num2 & playerUnitRevision.HasValue)
            goto label_3;
        }
        this._playerUnitRevision = new long?(num1);
        PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
        this._unitEquipped = source != null && ((IEnumerable<PlayerUnit>) source).Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => ((IEnumerable<int?>) x.equip_gear_ids).Contains<int?>(new int?(this.id))));
label_3:
        return this._unitEquipped;
      }
    }

    public bool ForBattle
    {
      get
      {
        if (this.entity_type == MasterDataTable.CommonRewardType.gear)
          return this.UnitEquipped;
        return this.entity_type == MasterDataTable.CommonRewardType.supply && this.box_type_id == 2;
      }
    }

    public bool ForTower
    {
      get
      {
        if (this.entity_type == MasterDataTable.CommonRewardType.gear)
          return this.UnitEquipped;
        return this.entity_type == MasterDataTable.CommonRewardType.supply && this.box_type_id == 3;
      }
    }

    public bool ForCorps
    {
      get
      {
        if (this.entity_type == MasterDataTable.CommonRewardType.gear)
          return this.UnitEquipped;
        return this.entity_type == MasterDataTable.CommonRewardType.supply && this.box_type_id == 5;
      }
    }

    public bool ForGuild
    {
      get
      {
        if (this.entity_type == MasterDataTable.CommonRewardType.gear)
          return this.UnitEquipped;
        return this.entity_type == MasterDataTable.CommonRewardType.supply && this.box_type_id == 4;
      }
    }

    public string name
    {
      get
      {
        if (this.gear == null)
          return this.supply.name;
        if (this.gear_level_unlimit <= 0)
          return this.gear.name;
        return "{0}+{1}".F((object) this.gear.name, (object) this.gear_level_unlimit);
      }
    }

    public MasterDataTable.CommonRewardType entity_type => (MasterDataTable.CommonRewardType) this._entity_type;

    public GearGear gear
    {
      get
      {
        return this.entity_type == MasterDataTable.CommonRewardType.gear ? MasterData.GearGear[this.entity_id] : (GearGear) null;
      }
    }

    public SupplySupply supply
    {
      get
      {
        return this.entity_type == MasterDataTable.CommonRewardType.supply ? MasterData.SupplySupply[this.entity_id] : (SupplySupply) null;
      }
    }

    public PlayerItem equipReisou
    {
      get
      {
        return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == this.equipped_reisou_player_gear_id));
      }
    }

    public bool isReisouSet => this.equipped_reisou_player_gear_id != 0;

    public GearRankIncr gearRankIncr
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return (GearRankIncr) null;
        if (this._gearRankCache == null || this._gearRankCache != null && this._gearRankCache.gear_kind != this.gear.kind && this._gearRankCache.group_id != this.gear.rank_incr_group && this._gearRankCache.level != this.gear_level)
          this._gearRankCache = GearRankIncr.FromRank(this.gear.kind, this.gear.rank_incr_group, this.gear_level);
        return this._gearRankCache;
      }
    }

    public ReisouRankIncr reisouRankIncr
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return (ReisouRankIncr) null;
        if (this._reisouRankCache == null || this._reisouRankCache != null && this._reisouRankCache.gear_kind != this.gear.kind && this._reisouRankCache.group_id != this.gear.rank_incr_group && this._reisouRankCache.level != this.gear_level)
          this._reisouRankCache = ReisouRankIncr.FromRank(this.gear.kind, this.gear.rank_incr_group, this.gear_level);
        return this._reisouRankCache;
      }
    }

    public ReisouRankIncr mythologyReisouHolyRankIncr
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return (ReisouRankIncr) null;
        GearGear holyId = this.GetReisouFusionMineRecipe().holy_ID;
        if (this._mythologyReisouHolyRankCache == null || this._mythologyReisouHolyRankCache != null && this._mythologyReisouHolyRankCache.gear_kind != holyId.kind && this._mythologyReisouHolyRankCache.group_id != holyId.rank_incr_group && this._mythologyReisouHolyRankCache.level != this.ReisouHolyLv)
          this._mythologyReisouHolyRankCache = ReisouRankIncr.FromRank(holyId.kind, holyId.rank_incr_group, this.ReisouHolyLv);
        return this._mythologyReisouHolyRankCache;
      }
    }

    public int ReisouHolyLv
    {
      get
      {
        return this.reisouHolyLv > 0 ? this.reisouHolyLv : this.GetPlayerMythologyGearStatus().holy_gear_level;
      }
      set => this.reisouHolyLv = value;
    }

    public ReisouRankIncr mythologyReisouChaosRankIncr
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return (ReisouRankIncr) null;
        GearGear chaosId = this.GetReisouFusionMineRecipe().chaos_ID;
        if (this._mythologyReisouChaosRankCache == null || this._mythologyReisouChaosRankCache != null && this._mythologyReisouChaosRankCache.gear_kind != chaosId.kind && this._mythologyReisouChaosRankCache.group_id != chaosId.rank_incr_group && this._mythologyReisouChaosRankCache.level != this.ReisouChaosLv)
          this._mythologyReisouChaosRankCache = ReisouRankIncr.FromRank(chaosId.kind, chaosId.rank_incr_group, this.ReisouChaosLv);
        return this._mythologyReisouChaosRankCache;
      }
    }

    public int ReisouChaosLv
    {
      get
      {
        return this.reisouChaosLv > 0 ? this.reisouChaosLv : this.GetPlayerMythologyGearStatus().chaos_gear_level;
      }
      set => this.reisouChaosLv = value;
    }

    public void SetPlayerMythologyGearStatusCache(PlayerMythologyGearStatus value)
    {
      this._playerMythologyGearStatusCache = value;
    }

    public PlayerMythologyGearStatus GetPlayerMythologyGearStatus()
    {
      if (this._playerMythologyGearStatusCache != null)
        return this._playerMythologyGearStatusCache;
      foreach (PlayerMythologyGearStatus mythologyGearStatus in SMManager.Get<PlayerMythologyGearStatus[]>())
      {
        if (mythologyGearStatus.mythology_player_gear_id == this.id)
        {
          this._playerMythologyGearStatusCache = mythologyGearStatus;
          return this._playerMythologyGearStatusCache;
        }
      }
      return (PlayerMythologyGearStatus) null;
    }

    public int power
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.power + this.gearRankIncr.power;
        if (this.isMythologyReisou())
          return (this.gear.power + this.mythologyReisouHolyRankIncr.power) * this.mythologyReisouChaosRankIncr.power / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.power / 100 : this.gear.power + this.reisouRankIncr.power;
      }
    }

    public int physical_defense
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.physical_defense + this.gearRankIncr.physical_defense;
        if (this.isMythologyReisou())
          return (this.gear.physical_defense + this.mythologyReisouHolyRankIncr.physical_defense) * this.mythologyReisouChaosRankIncr.physical_defense / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.physical_defense / 100 : this.gear.physical_defense + this.reisouRankIncr.physical_defense;
      }
    }

    public int magic_defense
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.magic_defense + this.gearRankIncr.magic_defense;
        if (this.isMythologyReisou())
          return (this.gear.magic_defense + this.mythologyReisouHolyRankIncr.magic_defense) * this.mythologyReisouChaosRankIncr.magic_defense / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.magic_defense / 100 : this.gear.magic_defense + this.reisouRankIncr.magic_defense;
      }
    }

    public int hit
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.hit + this.gearRankIncr.hit;
        if (this.isMythologyReisou())
          return (this.gear.hit + this.mythologyReisouHolyRankIncr.hit) * this.mythologyReisouChaosRankIncr.hit / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.hit / 100 : this.gear.hit + this.reisouRankIncr.hit;
      }
    }

    public int critical
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.critical + this.gearRankIncr.critical;
        if (this.isMythologyReisou())
          return (this.gear.critical + this.mythologyReisouHolyRankIncr.critical) * this.mythologyReisouChaosRankIncr.critical / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.critical / 100 : this.gear.critical + this.reisouRankIncr.critical;
      }
    }

    public int evasion
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.evasion + this.gearRankIncr.evasion;
        if (this.isMythologyReisou())
          return (this.gear.evasion + this.mythologyReisouHolyRankIncr.evasion) * this.mythologyReisouChaosRankIncr.evasion / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.evasion / 100 : this.gear.evasion + this.reisouRankIncr.evasion;
      }
    }

    public int hp_incremental
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.hp_incremental + this.gearRankIncr.hp_incremental;
        if (this.isMythologyReisou())
          return (this.gear.hp_incremental + this.mythologyReisouHolyRankIncr.hp_incremental) * this.mythologyReisouChaosRankIncr.hp_incremental / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.hp_incremental / 100 : this.gear.hp_incremental + this.reisouRankIncr.hp_incremental;
      }
    }

    public int strength_incremental
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.strength_incremental + this.gearRankIncr.strength_incremental;
        if (this.isMythologyReisou())
          return (this.gear.strength_incremental + this.mythologyReisouHolyRankIncr.strength_incremental) * this.mythologyReisouChaosRankIncr.strength_incremental / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.strength_incremental / 100 : this.gear.strength_incremental + this.reisouRankIncr.strength_incremental;
      }
    }

    public int vitality_incremental
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.vitality_incremental + this.gearRankIncr.vitality_incremental;
        if (this.isMythologyReisou())
          return (this.gear.vitality_incremental + this.mythologyReisouHolyRankIncr.vitality_incremental) * this.mythologyReisouChaosRankIncr.vitality_incremental / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.vitality_incremental / 100 : this.gear.vitality_incremental + this.reisouRankIncr.vitality_incremental;
      }
    }

    public int intelligence_incremental
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.intelligence_incremental + this.gearRankIncr.intelligence_incremental;
        if (this.isMythologyReisou())
          return (this.gear.intelligence_incremental + this.mythologyReisouHolyRankIncr.intelligence_incremental) * this.mythologyReisouChaosRankIncr.intelligence_incremental / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.intelligence_incremental / 100 : this.gear.intelligence_incremental + this.reisouRankIncr.intelligence_incremental;
      }
    }

    public int mind_incremental
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.mind_incremental + this.gearRankIncr.mind_incremental;
        if (this.isMythologyReisou())
          return (this.gear.mind_incremental + this.mythologyReisouHolyRankIncr.mind_incremental) * this.mythologyReisouChaosRankIncr.mind_incremental / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.mind_incremental / 100 : this.gear.mind_incremental + this.reisouRankIncr.mind_incremental;
      }
    }

    public int agility_incremental
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.agility_incremental + this.gearRankIncr.agility_incremental;
        if (this.isMythologyReisou())
          return (this.gear.agility_incremental + this.mythologyReisouHolyRankIncr.agility_incremental) * this.mythologyReisouChaosRankIncr.agility_incremental / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.agility_incremental / 100 : this.gear.agility_incremental + this.reisouRankIncr.agility_incremental;
      }
    }

    public int dexterity_incremental
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.dexterity_incremental + this.gearRankIncr.dexterity_incremental;
        if (this.isMythologyReisou())
          return (this.gear.dexterity_incremental + this.mythologyReisouHolyRankIncr.dexterity_incremental) * this.mythologyReisouChaosRankIncr.dexterity_incremental / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.dexterity_incremental / 100 : this.gear.dexterity_incremental + this.reisouRankIncr.dexterity_incremental;
      }
    }

    public int lucky_incremental
    {
      get
      {
        if (this.entity_type != MasterDataTable.CommonRewardType.gear)
          return 0;
        if (!this.isReisou())
          return this.gear.lucky_incremental + this.gearRankIncr.lucky_incremental;
        if (this.isMythologyReisou())
          return (this.gear.lucky_incremental + this.mythologyReisouHolyRankIncr.lucky_incremental) * this.mythologyReisouChaosRankIncr.lucky_incremental / 100;
        return this.isChaosReisou() ? this.reisouRankIncr.lucky_incremental / 100 : this.gear.lucky_incremental + this.reisouRankIncr.lucky_incremental;
      }
    }

    public GearGearSkill[] skills
    {
      get
      {
        List<GearGearSkill> source1 = new List<GearGearSkill>();
        if (this.entity_type == MasterDataTable.CommonRewardType.gear)
        {
          List<GearGearSkill> gearGearSkillList = new List<GearGearSkill>();
          List<GearGearSkill> list = ((IEnumerable<GearGearSkill>) MasterData.GearGearSkillList).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.gear.ID == this.entity_id && x.isReleased(this))).ToList<GearGearSkill>();
          if (list.Count > 0)
          {
            foreach (IGrouping<int, GearGearSkill> source2 in list.GroupBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.skill_group)))
              source1.Add(source2.OrderByDescending<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.release_rank)).First<GearGearSkill>());
          }
        }
        return source1.OrderBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.skill_group)).ToArray<GearGearSkill>();
      }
    }

    public GearReisouSkill[] getReisouSkills(int equip_gear_id, bool isIgnoreWeaponGroup = false)
    {
      List<GearReisouSkill> source1 = new List<GearReisouSkill>();
      if (this.gear.isReisou())
      {
        List<GearReisouSkill> source2 = new List<GearReisouSkill>();
        List<GearReisouSkill> source3 = new List<GearReisouSkill>();
        if (this.isMythologyReisou())
        {
          GearReisouFusion fusionMineRecipe = this.GetReisouFusionMineRecipe();
          GearGear holyId = fusionMineRecipe.holy_ID;
          GearGear chaosId = fusionMineRecipe.chaos_ID;
          int reisouHolyLv = this.ReisouHolyLv;
          int reisouChaosLv = this.ReisouChaosLv;
          foreach (GearReisouSkill gearReisouSkill in MasterData.GearReisouSkillList)
          {
            if (isIgnoreWeaponGroup || PlayerItem.isReisouSkillAwakeWeaponGroup(equip_gear_id, gearReisouSkill.awake_weapon_group))
            {
              if (gearReisouSkill.gear.ID == holyId.ID && gearReisouSkill.release_rank <= reisouHolyLv)
                source2.Add(gearReisouSkill);
              else if (gearReisouSkill.gear.ID == chaosId.ID && gearReisouSkill.release_rank <= reisouChaosLv)
                source3.Add(gearReisouSkill);
            }
          }
        }
        else
        {
          foreach (GearReisouSkill gearReisouSkill in MasterData.GearReisouSkillList)
          {
            if ((isIgnoreWeaponGroup || PlayerItem.isReisouSkillAwakeWeaponGroup(equip_gear_id, gearReisouSkill.awake_weapon_group)) && gearReisouSkill.gear.ID == this.entity_id && gearReisouSkill.release_rank <= this.gear_level)
              source2.Add(gearReisouSkill);
          }
        }
        if (source2.Count > 0)
        {
          foreach (IGrouping<int, GearReisouSkill> source4 in source2.GroupBy<GearReisouSkill, int>((Func<GearReisouSkill, int>) (x => x.skill_group)))
            source1.Add(source4.OrderByDescending<GearReisouSkill, int>((Func<GearReisouSkill, int>) (x => x.release_rank)).First<GearReisouSkill>());
        }
        if (source3.Count > 0)
        {
          foreach (IGrouping<int, GearReisouSkill> source5 in source3.GroupBy<GearReisouSkill, int>((Func<GearReisouSkill, int>) (x => x.skill_group)))
            source1.Add(source5.OrderByDescending<GearReisouSkill, int>((Func<GearReisouSkill, int>) (x => x.release_rank)).First<GearReisouSkill>());
        }
      }
      return source1.OrderBy<GearReisouSkill, int>((Func<GearReisouSkill, int>) (x => x.skill_group)).ToArray<GearReisouSkill>();
    }

    public static bool isReisouSkillAwakeWeaponGroup(int equip_gear_id, int awake_weapon_group)
    {
      if (awake_weapon_group == 0)
        return true;
      foreach (GearReisouSkillWeaponGroup skillWeaponGroup in MasterData.GearReisouSkillWeaponGroupList)
      {
        if (skillWeaponGroup.group == awake_weapon_group && skillWeaponGroup.gear_GearGear == equip_gear_id)
          return true;
      }
      return false;
    }

    public static PlayerItem CreateForKey(int id)
    {
      PlayerItem forKey = new PlayerItem();
      forKey._hasKey = true;
      int num1;
      int num2 = num1 = id;
      forKey.id = num1;
      forKey._key = (object) num2;
      return forKey;
    }

    public bool isWeapon()
    {
      return !this.isSupply() && !this.isCompse() && !this.isExchangable() && !this.isReisou();
    }

    public bool isComposeManaSeed()
    {
      bool flag = false;
      if (!this.isSupply() && this.gear.isComposeManaSeed())
        flag = true;
      return flag;
    }

    public bool isManaSeed()
    {
      bool flag = false;
      if (!this.isSupply() && this.gear.isManaSeed())
        flag = true;
      return flag;
    }

    public bool isSupply() => this.entity_type == MasterDataTable.CommonRewardType.supply;

    public bool isCompse()
    {
      return !this.isSupply() && this.gear.kind.Enum == GearKindEnum.smith && this.gear.compose_kind.kind.Enum != GearKindEnum.smith;
    }

    public bool isExchangable()
    {
      if (this.gear == null)
        return false;
      GearGear gear = this.gear;
      return gear.kind.Enum == GearKindEnum.smith && gear.compose_kind.kind.Enum == GearKindEnum.smith;
    }

    public bool isReisou() => this.gear != null && this.gear.isReisou();

    public bool isHolyReisou() => this.gear != null && this.gear.isHolyReisou();

    public bool isChaosReisou() => this.gear != null && this.gear.isChaosReisou();

    public bool isMythologyReisou() => this.gear != null && this.gear.isMythologyReisou();

    public bool isReisouFusionPossible(PlayerItem[] playerItems)
    {
      if (!this.isHolyReisou() && !this.isChaosReisou())
        return false;
      if (this.isHolyReisou())
      {
        foreach (GearReisouFusion gearReisouFusion in MasterData.GearReisouFusion.Values)
        {
          GearReisouFusion recipe = gearReisouFusion;
          if (recipe.holy_ID_GearGear == this.entity_id && ((IEnumerable<PlayerItem>) playerItems).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_id == recipe.chaos_ID_GearGear)) != (PlayerItem) null)
            return true;
        }
      }
      else if (this.isChaosReisou())
      {
        foreach (GearReisouFusion gearReisouFusion in MasterData.GearReisouFusion.Values)
        {
          GearReisouFusion recipe = gearReisouFusion;
          if (recipe.chaos_ID_GearGear == this.entity_id && ((IEnumerable<PlayerItem>) playerItems).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_id == recipe.holy_ID_GearGear)) != (PlayerItem) null && ((IEnumerable<PlayerItem>) playerItems).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_id == recipe.chaos_ID_GearGear)) != (PlayerItem) null)
            return true;
        }
      }
      return false;
    }

    public GearReisouFusion GetReisouFusionPossibleRecipe(PlayerItem[] playerItems)
    {
      if (!this.isHolyReisou() && !this.isChaosReisou())
        return (GearReisouFusion) null;
      if (this.isHolyReisou())
      {
        foreach (GearReisouFusion fusionPossibleRecipe in MasterData.GearReisouFusion.Values)
        {
          if (fusionPossibleRecipe.holy_ID_GearGear == this.entity_id)
            return fusionPossibleRecipe;
        }
      }
      else if (this.isChaosReisou())
      {
        foreach (GearReisouFusion fusionPossibleRecipe in MasterData.GearReisouFusion.Values)
        {
          if (fusionPossibleRecipe.chaos_ID_GearGear == this.entity_id)
            return fusionPossibleRecipe;
        }
      }
      return (GearReisouFusion) null;
    }

    public GearReisouFusion GetReisouFusionMineRecipe()
    {
      if (!this.isMythologyReisou())
        return (GearReisouFusion) null;
      foreach (GearReisouFusion fusionMineRecipe in MasterData.GearReisouFusion.Values)
      {
        if (fusionMineRecipe.mythology_ID_GearGear == this.gear.ID)
          return fusionMineRecipe;
      }
      return (GearReisouFusion) null;
    }

    public int GetReisouRankLimit()
    {
      int reisouRankLimit = 1;
      foreach (ReisouRankIncr reisouRankIncr in MasterData.ReisouRankIncrList)
      {
        if (reisouRankIncr.gear_kind.Enum == this.gear.kind.Enum && reisouRankIncr.group_id == this.gear.rank_incr_group && reisouRankIncr.level >= reisouRankLimit)
          reisouRankLimit = reisouRankIncr.level;
      }
      return reisouRankLimit;
    }

    public CommonElement GetElement()
    {
      CommonElement element = CommonElement.none;
      IEnumerable<GearGearSkill> source = ((IEnumerable<GearGearSkill>) this.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.effect_logic.Enum == BattleskillEffectLogicEnum.invest_element))));
      if (source.Any<GearGearSkill>())
        element = source.First<GearGearSkill>().skill.element;
      return element;
    }

    public PlayerItem Clone() => (PlayerItem) this.MemberwiseClone();

    public bool isLevelMax() => this.gear_level_limit <= this.gear_level;

    public bool isLimitMax() => this.gear_level_limit_max <= this.gear_level_limit;

    public BL.Item toBLItem()
    {
      return new BL.Item()
      {
        playerItemId = this.id,
        itemId = this.supply.ID,
        amount = this.quantity,
        initialAmount = this.quantity
      };
    }

    public PlayerItem()
    {
    }

    public PlayerItem(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this._entity_type = (int) (long) json[nameof (_entity_type)];
      this.box_type_id = (int) (long) json[nameof (box_type_id)];
      this.broken = (bool) json[nameof (broken)];
      this.entity_id = (int) (long) json[nameof (entity_id)];
      this.equipped_reisou_player_gear_id = (int) (long) json[nameof (equipped_reisou_player_gear_id)];
      this.favorite = (bool) json[nameof (favorite)];
      this.for_battle = (bool) json[nameof (for_battle)];
      this.gear_accessory_remaining_amount = (int) (long) json[nameof (gear_accessory_remaining_amount)];
      this.gear_exp = (int) (long) json[nameof (gear_exp)];
      this.gear_exp_next = (int) (long) json[nameof (gear_exp_next)];
      this.gear_level = (int) (long) json[nameof (gear_level)];
      this.gear_level_limit = (int) (long) json[nameof (gear_level_limit)];
      this.gear_level_limit_max = (int) (long) json[nameof (gear_level_limit_max)];
      this.gear_level_unlimit = (int) (long) json[nameof (gear_level_unlimit)];
      this.gear_total_exp = (int) (long) json[nameof (gear_total_exp)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this.is_new = (bool) json[nameof (is_new)];
      this.player_id = (string) json[nameof (player_id)];
      this.quantity = (int) (long) json[nameof (quantity)];
    }

    public void resetByCustomDeck(bool bUnitEquipped = false)
    {
      this.equipped_reisou_player_gear_id = 0;
      if (bUnitEquipped)
      {
        this._playerUnitRevision = new long?(SMManager.Revision<PlayerUnit[]>());
        this._unitEquipped = true;
      }
      else
      {
        this._playerUnitRevision = new long?();
        this._unitEquipped = false;
      }
    }

    public void restoreByCustomDeck(PlayerItem src, Util.RestoreGear restoreGear)
    {
      foreach (FieldInfo field in typeof (PlayerItem).GetFields(BindingFlags.Instance | BindingFlags.Public))
        field.SetValue((object) this, field.GetValue((object) src));
      this._hasKey = true;
      this._key = (object) this.id;
      this.resetByCustomDeck();
      if (restoreGear == null)
        return;
      this.equipped_reisou_player_gear_id = restoreGear.equipped_reisou_player_gear_id;
    }

    public enum BoxType
    {
      Unused,
      Normal,
      Battle,
      Tower,
      Guild,
      Corps,
    }
  }
}
