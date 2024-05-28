// Decompiled with JetBrains decompiler
// Type: GameCore.ItemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace GameCore
{
  public class ItemInfo
  {
    public ItemInfo.ItemType itemType;
    public string name;
    public int itemID;
    public int masterID;
    public bool broken;
    public int gearLevel;
    public int gearLevelLimit;
    public int gearLevelUnLimit;
    public int gearExp;
    public int gearExpNext;
    public int gearAccessoryRemainingAmount;
    public int quantity;
    public bool ForBattle;
    public bool FusionPossible;
    public bool AwakeReisouSkill;
    public bool favorite;
    public bool isNew;
    public int sameItemIdx;
    public bool isEquipReisou_;
    public bool isEquipReisouLvMax_;
    public PlayerItem reisou;
    public bool enabledExpireDate_;
    public bool isTempSelectedCount;
    public int tempSelectedCount;

    public ItemInfo(ItemInfo.ItemType type, int sameIdx = 0, bool bExpireDate = false)
    {
      this.itemID = 0;
      this.masterID = 0;
      this.broken = false;
      this.gearLevel = 1;
      this.gearLevelLimit = 0;
      this.gearLevelUnLimit = 0;
      this.gearExp = 0;
      this.gearExpNext = 0;
      this.gearAccessoryRemainingAmount = 0;
      this.quantity = 1;
      this.ForBattle = false;
      this.FusionPossible = false;
      this.AwakeReisouSkill = false;
      this.favorite = false;
      this.isNew = false;
      this.itemType = type;
      this.sameItemIdx = sameIdx;
      this.isEquipReisou_ = false;
      this.isEquipReisouLvMax_ = false;
      this.enabledExpireDate_ = bExpireDate;
    }

    public ItemInfo(PlayerItem item, bool bExpireDate = false, Func<bool> checkEquipped = null)
    {
      this.Set(item, bExpireDate, checkEquipped);
    }

    public ItemInfo(
      PlayerItem gear,
      PlayerItem reisou,
      bool bExpireDate = false,
      Func<bool> checkEquipped = null)
    {
      this.Set(gear, bExpireDate, checkEquipped, new Tuple<PlayerItem>(reisou));
    }

    public ItemInfo(PlayerMaterialGear item, int sameIndex = 0)
    {
      this.Set(item);
      this.sameItemIdx = sameIndex;
    }

    public void Set(
      PlayerItem item,
      bool bExpireDate = false,
      Func<bool> checkEquipped = null,
      Tuple<PlayerItem> setReisou = null)
    {
      this.itemID = item.id;
      this.broken = item.broken;
      this.gearLevel = item.gear_level;
      this.gearLevelLimit = item.gear_level_limit;
      this.gearLevelUnLimit = item.gear_level_unlimit;
      this.gearExp = item.gear_exp;
      this.gearExpNext = item.gear_exp_next;
      this.gearAccessoryRemainingAmount = item.gear_accessory_remaining_amount;
      this.name = item.name;
      this.quantity = item.quantity < 1 ? 1 : item.quantity;
      this.ForBattle = checkEquipped != null ? checkEquipped() : item.ForBattle || item.ForTower || item.ForCorps || item.ForGuild;
      this.FusionPossible = false;
      this.AwakeReisouSkill = false;
      this.favorite = item.favorite;
      this.isNew = item.is_new;
      if (item.isSupply())
      {
        this.itemType = ItemInfo.ItemType.Supply;
        this.masterID = item.supply.ID;
        this.quantity = item.quantity;
        this.enabledExpireDate_ = false;
      }
      else
      {
        this.masterID = item.gear.ID;
        if (item.isWeapon() || item.isReisou())
        {
          this.itemType = ItemInfo.ItemType.Gear;
          this.reisou = setReisou != null ? setReisou.Item1 : (item.equipped_reisou_player_gear_id != 0 ? item.equipReisou : (PlayerItem) null);
          this.isEquipReisou_ = this.reisou != (PlayerItem) null;
          if (this.isEquipReisou_)
            this.isEquipReisouLvMax_ = this.reisou.gear_level >= this.reisou.gear_level_limit;
        }
        else if (item.isCompse())
        {
          this.itemType = ItemInfo.ItemType.Compse;
          bExpireDate = false;
        }
        else if (item.isExchangable())
        {
          this.itemType = ItemInfo.ItemType.Exchangable;
          bExpireDate = false;
        }
        this.enabledExpireDate_ = bExpireDate;
      }
    }

    public void Set(PlayerMaterialGear item)
    {
      this.itemID = item.id;
      this.masterID = item.gear_id;
      this.broken = false;
      this.gearLevel = 1;
      this.gearLevelLimit = 0;
      this.gearLevelUnLimit = 0;
      this.gearExp = 0;
      this.gearExpNext = 0;
      this.gearAccessoryRemainingAmount = 0;
      this.name = item.name;
      this.quantity = item.quantity;
      this.ForBattle = false;
      this.FusionPossible = false;
      this.AwakeReisouSkill = false;
      this.favorite = false;
      this.isNew = false;
      if (item.isCompse())
        this.itemType = ItemInfo.ItemType.Compse;
      else if (item.isExchangable())
      {
        this.itemType = ItemInfo.ItemType.Exchangable;
      }
      else
      {
        if (!item.isWeaponMaterial())
          return;
        this.itemType = ItemInfo.ItemType.WeaponMaterial;
      }
    }

    public GearGear gear
    {
      get
      {
        return this.itemType == ItemInfo.ItemType.Supply ? (GearGear) null : MasterData.GearGear[this.masterID];
      }
    }

    public SupplySupply supply
    {
      get
      {
        return this.itemType != ItemInfo.ItemType.Supply ? (SupplySupply) null : MasterData.SupplySupply[this.masterID];
      }
    }

    public bool isWeapon
    {
      get
      {
        return this.itemType == ItemInfo.ItemType.Gear && !MasterData.GearGear[this.masterID].isReisou();
      }
    }

    public bool isReisou
    {
      get
      {
        return this.itemType == ItemInfo.ItemType.Gear && MasterData.GearGear[this.masterID].isReisou();
      }
    }

    public bool isWeaponOrReisou => this.itemType == ItemInfo.ItemType.Gear;

    public bool isEquipReisou => this.isEquipReisou_;

    public bool isEquipReisouLvMax => this.isEquipReisouLvMax_;

    public int GetSortLevel()
    {
      int sortLevel;
      if (this.gear.isMythologyReisou() && (!this.gear.drilling_exp_mythology_id.HasValue || this.gear.drilling_exp_mythology_id.Value == 0))
      {
        PlayerMythologyGearStatus mythologyGearStatus = this.playerItem.GetPlayerMythologyGearStatus();
        sortLevel = mythologyGearStatus.holy_gear_level + mythologyGearStatus.chaos_gear_level;
      }
      else
        sortLevel = this.gearLevel + this.gearLevelUnLimit;
      return sortLevel;
    }

    public int GetSortLevelLimit()
    {
      int sortLevelLimit;
      if (this.gear.isMythologyReisou())
      {
        PlayerMythologyGearStatus mythologyGearStatus = this.playerItem.GetPlayerMythologyGearStatus();
        sortLevelLimit = mythologyGearStatus.holy_gear_level_limit + mythologyGearStatus.chaos_gear_level_limit;
      }
      else
        sortLevelLimit = this.gearLevelLimit;
      return sortLevelLimit;
    }

    public bool isSupply => this.itemType == ItemInfo.ItemType.Supply;

    public bool isExchangable => this.itemType == ItemInfo.ItemType.Exchangable;

    public bool isCompse => this.itemType == ItemInfo.ItemType.Compse;

    public bool isDrilling
    {
      get
      {
        if (this.gear == null)
          return false;
        return this.gear.kind.Enum == GearKindEnum.drilling || this.gear.kind.Enum == GearKindEnum.special_drilling;
      }
    }

    public bool isComposeManaSeed
    {
      get
      {
        return this.gear != null && this.gear.isComposeManaSeed() && this.gearAccessoryRemainingAmount < this.gear.manaSeedRecoveryLimit;
      }
    }

    public bool isWeaponMaterial => this.itemType == ItemInfo.ItemType.WeaponMaterial;

    public bool isDisappearItem
    {
      get
      {
        GearGear gearGear;
        return this.itemType == ItemInfo.ItemType.Gear && MasterData.GearGear.TryGetValue(this.masterID, out gearGear) && gearGear.disappearance_num.HasValue;
      }
    }

    public bool isExhaustedGear
    {
      get
      {
        if (this.isDisappearItem)
        {
          int accessoryRemainingAmount = this.gearAccessoryRemainingAmount;
          int? disappearanceNum = this.gear.disappearance_num;
          int valueOrDefault = disappearanceNum.GetValueOrDefault();
          if (accessoryRemainingAmount < valueOrDefault & disappearanceNum.HasValue)
            return true;
        }
        return false;
      }
    }

    public bool isUsedGear
    {
      get
      {
        if (this.isDisappearItem)
        {
          int accessoryRemainingAmount = this.gearAccessoryRemainingAmount;
          int? disappearanceNum = this.gear.disappearance_num;
          int valueOrDefault = disappearanceNum.GetValueOrDefault();
          if (!(accessoryRemainingAmount == valueOrDefault & disappearanceNum.HasValue))
            return true;
        }
        return false;
      }
    }

    public bool hasExpireDateGear
    {
      get
      {
        GearGear gearGear;
        return this.itemType == ItemInfo.ItemType.Gear && MasterData.GearGear.TryGetValue(this.masterID, out gearGear) && gearGear.expire_date_GearExpireDate.HasValue;
      }
    }

    public bool isUsedOrHasExpireDateGear
    {
      get
      {
        GearGear gearGear;
        if (this.itemType != ItemInfo.ItemType.Gear || !MasterData.GearGear.TryGetValue(this.masterID, out gearGear))
          return false;
        if (gearGear.expire_date_GearExpireDate.HasValue)
          return true;
        if (!gearGear.disappearance_num.HasValue)
          return false;
        int accessoryRemainingAmount = this.gearAccessoryRemainingAmount;
        int? disappearanceNum = gearGear.disappearance_num;
        int valueOrDefault = disappearanceNum.GetValueOrDefault();
        return !(accessoryRemainingAmount == valueOrDefault & disappearanceNum.HasValue);
      }
    }

    public string Name() => this.name;

    public string Description()
    {
      return this.itemType == ItemInfo.ItemType.Supply ? this.supply.description : string.Empty;
    }

    public long SellPrice()
    {
      return this.itemType == ItemInfo.ItemType.Supply ? (long) this.supply.sell_price : (long) this.gear.sell_price;
    }

    public int RepairPrice()
    {
      int num = 0;
      if (this.itemType == ItemInfo.ItemType.Gear)
        num = this.gear.repair_price * (this.gearLevel + 1);
      return num;
    }

    public CommonElement GetElement()
    {
      CommonElement element = CommonElement.none;
      PlayerItem playerItem = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == this.itemID));
      if (playerItem != (PlayerItem) null)
      {
        IEnumerable<GearGearSkill> source = ((IEnumerable<GearGearSkill>) playerItem.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.effect_logic.Enum == BattleskillEffectLogicEnum.invest_element))));
        if (source.Any<GearGearSkill>())
          element = source.First<GearGearSkill>().skill.element;
      }
      return element;
    }

    public GearGearSkill[] skills
    {
      get
      {
        List<GearGearSkill> source1 = new List<GearGearSkill>();
        if (this.isWeaponOrReisou)
        {
          List<GearGearSkill> list = ((IEnumerable<GearGearSkill>) MasterData.GearGearSkillList).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => x.gear.ID == this.masterID && x.isReleased(this))).ToList<GearGearSkill>();
          if (list.Count > 0)
          {
            foreach (IGrouping<int, GearGearSkill> source2 in list.GroupBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.skill_group)))
              source1.Add(source2.OrderByDescending<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.release_rank)).First<GearGearSkill>());
          }
        }
        return source1.OrderBy<GearGearSkill, int>((Func<GearGearSkill, int>) (x => x.skill_group)).ToArray<GearGearSkill>();
      }
    }

    public int hp_incremental => !this.isWeaponOrReisou ? 0 : this.gear.hp_incremental;

    public int strength_incremental => !this.isWeaponOrReisou ? 0 : this.gear.strength_incremental;

    public int vitality_incremental => !this.isWeaponOrReisou ? 0 : this.gear.vitality_incremental;

    public int intelligence_incremental
    {
      get => !this.isWeaponOrReisou ? 0 : this.gear.intelligence_incremental;
    }

    public int mind_incremental => !this.isWeaponOrReisou ? 0 : this.gear.mind_incremental;

    public int agility_incremental => !this.isWeaponOrReisou ? 0 : this.gear.agility_incremental;

    public int dexterity_incremental
    {
      get => !this.isWeaponOrReisou ? 0 : this.gear.dexterity_incremental;
    }

    public int lucky_incremental => !this.isWeaponOrReisou ? 0 : this.gear.lucky_incremental;

    public PlayerItem playerItem
    {
      get
      {
        return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == this.itemID));
      }
    }

    public Future<Sprite> LoadSpriteThumbnail()
    {
      return this.itemType == ItemInfo.ItemType.Supply ? this.supply.LoadSpriteThumbnail() : this.gear.LoadSpriteThumbnail();
    }

    public enum ItemType
    {
      Gear,
      Compse,
      Exchangable,
      Supply,
      WeaponMaterial,
    }
  }
}
