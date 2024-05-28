// Decompiled with JetBrains decompiler
// Type: SM.PlayerMaterialGear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerMaterialGear : KeyCompare
  {
    public bool isEarthMode;
    private GearGear _gear;
    public string player_id;
    public int gear_id;
    public int id;
    public int quantity;

    public override bool Equals(object rhs) => this.Equals(rhs as PlayerMaterialGear);

    public override int GetHashCode() => 0;

    public bool Equals(PlayerMaterialGear rhs)
    {
      if ((object) rhs == null)
        return false;
      if ((object) this == (object) rhs)
        return true;
      return !(this.GetType() != rhs.GetType()) && this.id == rhs.id && this.entity_type == rhs.entity_type && this.player_id == rhs.player_id;
    }

    public static bool operator ==(PlayerMaterialGear lhs, PlayerMaterialGear rhs)
    {
      return (object) lhs == null ? (object) rhs == null : lhs.Equals(rhs);
    }

    public static bool operator !=(PlayerMaterialGear lhs, PlayerMaterialGear rhs) => !(lhs == rhs);

    public bool ForBattle => false;

    public string name => this.gear.name;

    public MasterDataTable.CommonRewardType entity_type => MasterDataTable.CommonRewardType.gear;

    public GearGear gear
    {
      get
      {
        if (this._gear == null)
          this._gear = MasterData.GearGear[this.gear_id];
        return this._gear;
      }
    }

    public SupplySupply supply => (SupplySupply) null;

    public int hp_incremental => 0;

    public int strength_incremental => this.gear.strength_incremental;

    public int vitality_incremental => this.gear.vitality_incremental;

    public int intelligence_incremental => this.gear.intelligence_incremental;

    public int mind_incremental => this.gear.mind_incremental;

    public int agility_incremental => this.gear.agility_incremental;

    public int dexterity_incremental => this.gear.dexterity_incremental;

    public int lucky_incremental => this.gear.lucky_incremental;

    public GearGearSkill[] skills => (GearGearSkill[]) null;

    public static PlayerMaterialGear CreateForKey(int id, int quantity)
    {
      PlayerMaterialGear forKey = new PlayerMaterialGear();
      forKey._hasKey = true;
      forKey._key = (object) id;
      forKey.quantity = quantity;
      return forKey;
    }

    public bool isWeapon() => !this.isSupply() && !this.isCompse() && !this.isExchangable();

    public bool isSupply() => this.entity_type == MasterDataTable.CommonRewardType.supply;

    public bool isCompse()
    {
      return !this.isSupply() && (this.gear.kind.Enum == GearKindEnum.smith && this.gear.compose_kind.kind.Enum != GearKindEnum.smith || this.gear.kind.Enum == GearKindEnum.drilling || this.gear.kind.Enum == GearKindEnum.special_drilling || this.gear.kind.Enum == GearKindEnum.sea_present);
    }

    public bool isDilling()
    {
      return !this.isSupply() && (this.gear.kind.Enum == GearKindEnum.drilling || this.gear.kind.Enum == GearKindEnum.sea_present);
    }

    public bool isSpecialDilling()
    {
      return !this.isSupply() && this.gear.kind.Enum == GearKindEnum.special_drilling;
    }

    public bool isExchangable()
    {
      return this.gear != null && !this.isSupply() && this.gear.kind.Enum == GearKindEnum.smith && this.gear.compose_kind.kind.Enum == GearKindEnum.smith;
    }

    public bool isWeaponMaterial() => this.gear != null && !this.gear.isMaterial();

    public bool isManaSeed() => this.gear != null && this.gear.isManaSeed();

    public bool isCallGift() => this.gear != null && this.gear.isCallGift();

    public CommonElement GetElement()
    {
      CommonElement element = CommonElement.none;
      IEnumerable<GearGearSkill> source = ((IEnumerable<GearGearSkill>) this.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (x => ((IEnumerable<BattleskillEffect>) x.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.effect_logic.Enum == BattleskillEffectLogicEnum.invest_element))));
      if (source.Any<GearGearSkill>())
        element = source.First<GearGearSkill>().skill.element;
      return element;
    }

    public PlayerMaterialGear()
    {
    }

    public PlayerMaterialGear(Dictionary<string, object> json)
    {
      this._hasKey = true;
      this.player_id = (string) json[nameof (player_id)];
      this.gear_id = (int) (long) json[nameof (gear_id)];
      this._key = (object) (this.id = (int) (long) json[nameof (id)]);
      this.quantity = (int) (long) json[nameof (quantity)];
    }
  }
}
