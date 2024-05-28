// Decompiled with JetBrains decompiler
// Type: SM.PlayerCustomDeckUnit_parameter_list
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using EquipmentRules;
using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerCustomDeckUnit_parameter_list : KeyCompare
  {
    public int[] over_killers_ids;
    public int job_id;
    public int[] player_reisou_ids;
    public int awake_skill_id;
    public int[] player_gear_ids;
    public int player_unit_id;
    public const int MAX_EQUIPMENTS = 1;

    public PlayerCustomDeckUnit_parameter_list()
    {
    }

    public PlayerCustomDeckUnit_parameter_list(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.over_killers_ids = ((IEnumerable<object>) json[nameof (over_killers_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.job_id = (int) (long) json[nameof (job_id)];
      this.player_reisou_ids = ((IEnumerable<object>) json[nameof (player_reisou_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.awake_skill_id = (int) (long) json[nameof (awake_skill_id)];
      this.player_gear_ids = ((IEnumerable<object>) json[nameof (player_gear_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      this.player_unit_id = (int) (long) json[nameof (player_unit_id)];
    }

    public bool isModified { get; private set; }

    public void setModified() => this.isModified = true;

    public void clearModified() => this.isModified = false;

    public int index { get; private set; }

    public void setIndex(int i)
    {
      if (this.index == i)
        return;
      this.index = i;
      this.isModified = true;
    }

    public void setOverkillers(int index, int id)
    {
      if (index >= this.over_killers_ids.Length || this.over_killers_ids[index] == id)
        return;
      this.over_killers_ids[index] = id;
      this.isModified = true;
    }

    public void setJob(int id, bool[] bClearGears)
    {
      if (this.job_id == id)
        return;
      this.job_id = id;
      this.isModified = true;
      if (bClearGears == null || this.player_gear_ids == null)
        return;
      for (int index = 0; index < this.player_gear_ids.Length && index < bClearGears.Length; ++index)
      {
        if (bClearGears[index])
        {
          this.player_gear_ids[index] = 0;
          this.player_reisou_ids[index] = 0;
        }
      }
    }

    public void setAwakeskill(int id)
    {
      if (this.awake_skill_id == id)
        return;
      this.awake_skill_id = id;
      this.isModified = true;
    }

    public bool setUnit(int id, PlayerUnit[] units, PlayerItem[] gears, PlayerAwakeSkill[] skills)
    {
      if (this.player_unit_id == id)
        return false;
      this.player_unit_id = id;
      this.isModified = true;
      PlayerUnit playerUnit1 = id != 0 ? Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == id)) : (PlayerUnit) null;
      if (playerUnit1 != (PlayerUnit) null)
      {
        this.job_id = playerUnit1.getJobData().ID;
        UnitUnit unit = playerUnit1.unit;
        if (!playerUnit1.can_equip_awake_skill)
          this.awake_skill_id = 0;
        else if (this.awake_skill_id != 0 && !unit.CanEquipAwakeSkill(Array.Find<PlayerAwakeSkill>(skills, (Predicate<PlayerAwakeSkill>) (x => x.id == this.awake_skill_id))))
          this.awake_skill_id = 0;
        int[] indexMapByCustomDeck = playerUnit1.equippedGearIndexMapByCustomDeck;
        int count = indexMapByCustomDeck[indexMapByCustomDeck.Length - 1] + 1;
        if (this.player_gear_ids.Length < count)
        {
          this.player_gear_ids = ((IEnumerable<int>) this.player_gear_ids).Concat<int>(System.Linq.Enumerable.Repeat<int>(0, count - this.player_gear_ids.Length)).ToArray<int>();
          this.player_reisou_ids = ((IEnumerable<int>) this.player_reisou_ids).Concat<int>(System.Linq.Enumerable.Repeat<int>(0, count - this.player_reisou_ids.Length)).ToArray<int>();
        }
        else if (this.player_gear_ids.Length > count)
        {
          this.player_gear_ids = ((IEnumerable<int>) this.player_gear_ids).Take<int>(count).ToArray<int>();
          this.player_reisou_ids = ((IEnumerable<int>) this.player_reisou_ids).Take<int>(count).ToArray<int>();
        }
        PlayerItem[] array1 = ((IEnumerable<int>) this.player_gear_ids).Select<int, PlayerItem>((Func<int, PlayerItem>) (i => !i.IsValid() ? (PlayerItem) null : Array.Find<PlayerItem>(gears, (Predicate<PlayerItem>) (x => x.id == i)))).ToArray<PlayerItem>();
        PlayerItem[] equippedPlayerGears = new PlayerItem[3];
        int primaryGearKind = (int) unit.primaryGearKind;
        PlayerUnitGearProficiency unitGearProficiency = Array.Find<PlayerUnitGearProficiency>(playerUnit1.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == primaryGearKind));
        for (int slotIndex = 0; slotIndex < array1.Length; ++slotIndex)
        {
          GearGear gear = array1[slotIndex]?.gear;
          if (array1[slotIndex] == (PlayerItem) null || !gear.checkCanEquipByProficiency(playerUnit1, gear.is_jingi ? unitGearProficiency : (PlayerUnitGearProficiency) null))
          {
            this.player_gear_ids[slotIndex] = 0;
            this.player_reisou_ids[slotIndex] = 0;
          }
          else if (Gears.createFuncPossibleEquipped(playerUnit1, slotIndex, equippedPlayerGears)(array1[slotIndex]))
          {
            equippedPlayerGears[slotIndex] = array1[slotIndex];
          }
          else
          {
            this.player_gear_ids[slotIndex] = 0;
            this.player_reisou_ids[slotIndex] = 0;
          }
        }
        int[] array2 = ((IEnumerable<int>) playerUnit1.over_killers_player_unit_ids).Select<int, int>((Func<int, int>) (i => i < 0 ? -1 : 0)).ToArray<int>();
        int numOverkillersSlot = unit.numOverkillersSlot;
        if (this.over_killers_ids.Length != numOverkillersSlot)
        {
          int length = this.over_killers_ids.Length;
          this.over_killers_ids = length <= numOverkillersSlot ? ((IEnumerable<int>) this.over_killers_ids).Concat<int>(((IEnumerable<int>) array2).Skip<int>(length).Take<int>(numOverkillersSlot - length)).ToArray<int>() : ((IEnumerable<int>) this.over_killers_ids).Take<int>(numOverkillersSlot).ToArray<int>();
        }
        for (int n = 0; n < this.over_killers_ids.Length; ++n)
        {
          if (array2[n] == -1)
            this.over_killers_ids[n] = -1;
          else if (this.over_killers_ids[n] <= 0)
            this.over_killers_ids[n] = 0;
          else if (this.over_killers_ids[n] == playerUnit1.id)
          {
            this.over_killers_ids[n] = 0;
          }
          else
          {
            PlayerUnit playerUnit2 = Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == this.over_killers_ids[n]));
            if (playerUnit2 == (PlayerUnit) null || !unit.checkEquipPossible(playerUnit2.unit))
              this.over_killers_ids[n] = 0;
          }
        }
      }
      return true;
    }

    private bool checkEquipped(PlayerUnit targetUnit, PlayerItem targetGear)
    {
      GearGear gear;
      if (!(gear = targetGear.gear).isEquipment(targetUnit))
        return false;
      if (!gear.kind.isNonWeapon)
      {
        MasterDataTable.UnitJob jobData = targetUnit.getJobData();
        if (jobData.classification_GearClassificationPattern.HasValue && jobData.classification_GearClassificationPattern.Value != 0)
        {
          int? classificationPattern = gear.classification_GearClassificationPattern;
          int num = jobData.classification_GearClassificationPattern.Value;
          if (!(classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue))
            return false;
        }
      }
      return true;
    }

    public bool checkBlank()
    {
      bool flag = true;
      if (this.player_unit_id != 0)
        flag = false;
      else if (this.awake_skill_id != 0)
        flag = false;
      else if (((IEnumerable<int>) this.over_killers_ids).Any<int>((Func<int, bool>) (i => i != 0 && i != -1)))
        flag = false;
      else if (((IEnumerable<int>) this.player_gear_ids).Any<int>((Func<int, bool>) (i => i != 0)))
        flag = false;
      return flag;
    }

    public PlayerCustomDeckUnit_parameter_list clone()
    {
      PlayerCustomDeckUnit_parameter_list unitParameterList = (PlayerCustomDeckUnit_parameter_list) this.MemberwiseClone();
      unitParameterList.over_killers_ids = ((IEnumerable<int>) this.over_killers_ids).ToArray<int>();
      unitParameterList.player_reisou_ids = ((IEnumerable<int>) this.player_reisou_ids).ToArray<int>();
      unitParameterList.player_gear_ids = ((IEnumerable<int>) this.player_gear_ids).ToArray<int>();
      unitParameterList.clearModified();
      return unitParameterList;
    }

    public PlayerCustomDeckUnit_parameter_list cloneBySave(UnitUnit unit)
    {
      PlayerCustomDeckUnit_parameter_list unitParameterList = (PlayerCustomDeckUnit_parameter_list) this.MemberwiseClone();
      unitParameterList.player_gear_ids = ((IEnumerable<int>) this.player_gear_ids).ToArray<int>();
      unitParameterList.player_reisou_ids = ((IEnumerable<int>) this.player_reisou_ids).ToArray<int>();
      for (int index = 0; index < unitParameterList.player_gear_ids.Length; ++index)
      {
        if (unitParameterList.player_gear_ids[index] == 0)
          unitParameterList.player_reisou_ids[index] = 0;
      }
      unitParameterList.over_killers_ids = ((IEnumerable<int>) this.over_killers_ids).Where<int>((Func<int, bool>) (i => i >= 0)).ToArray<int>();
      if (unitParameterList.job_id == unit.job_UnitJob)
        unitParameterList.job_id = 0;
      unitParameterList.clearModified();
      return unitParameterList;
    }

    public PlayerUnit createPlayerUnit(
      PlayerUnit[] units,
      PlayerItem[] gears = null,
      PlayerAwakeSkill[] awakeSkills = null)
    {
      PlayerUnit playerUnit;
      if (this.player_unit_id == 0 || (playerUnit = Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == this.player_unit_id))) == (PlayerUnit) null)
        return (PlayerUnit) null;
      PlayerUnit target = PlayerCustomDeck.isCustom(playerUnit) ? JobChangeUtil.createPlayerUnit(playerUnit, this.job_id, new Func<PlayerUnit, PlayerUnit>(PlayerCustomDeck.cloneByCustom)) : JobChangeUtil.createPlayerUnit(playerUnit, this.job_id, bCloneParam: true);
      this.setGears(target, gears, true);
      target.equip_gear_ids = (int?[]) null;
      if (awakeSkills != null && this.awake_skill_id != 0)
      {
        target.equip_awake_skill_ids = new int?[1]
        {
          new int?(this.awake_skill_id)
        };
        target.primary_equipped_awake_skill = Array.Find<PlayerAwakeSkill>(awakeSkills, (Predicate<PlayerAwakeSkill>) (x => x.id == this.awake_skill_id));
      }
      else
      {
        target.equip_awake_skill_ids = new int?[0];
        target.primary_equipped_awake_skill = (PlayerAwakeSkill) null;
      }
      target.usedPrimary = PlayerUnit.UsedPrimary.All;
      int[] array = ((IEnumerable<int>) target.over_killers_player_unit_ids).Select<int, int>((Func<int, int>) (i => i < 0 ? -1 : 0)).ToArray<int>();
      for (int index = 0; index < array.Length && index < this.over_killers_ids.Length; ++index)
      {
        if (array[index] == 0 && this.over_killers_ids[index] > 0)
          array[index] = this.over_killers_ids[index];
      }
      target.over_killers_player_unit_ids = array;
      target.clearOverkillersParameter();
      target.resetCacheOverkillersUnits(this.createOverkillers(units));
      target.resetOverkillersParameter();
      target.resetOverkillersSkills();
      return target;
    }

    private void setGears(PlayerUnit target, PlayerItem[] gears, bool bSetPrimaryReisou)
    {
      if (gears == null)
        return;
      int[] indexMapByCustomDeck = target.equippedGearIndexMapByCustomDeck;
      target.equip_gear_ids = new int?[indexMapByCustomDeck.Length];
      if (!this.player_gear_ids.IsNullOrEmpty<int>())
      {
        for (int index = 0; index < indexMapByCustomDeck.Length && indexMapByCustomDeck[index] < this.player_gear_ids.Length; ++index)
          target.equip_gear_ids[index] = new int?(this.player_gear_ids[indexMapByCustomDeck[index]]);
      }
      target.primary_equipped_gear = target.FindEquippedGear(gears)?.Clone();
      target.primary_equipped_gear2 = target.FindEquippedGear2(gears)?.Clone();
      target.primary_equipped_gear3 = target.FindEquippedGear3(gears)?.Clone();
      target.primary_equipped_reisou = (PlayerItem) null;
      target.primary_equipped_reisou2 = (PlayerItem) null;
      target.primary_equipped_reisou3 = (PlayerItem) null;
      int? nullable;
      if (target.primary_equipped_gear != (PlayerItem) null)
      {
        target.primary_equipped_gear.resetByCustomDeck(true);
        nullable = ((IEnumerable<int>) this.player_gear_ids).FirstIndexOrNull<int>((Func<int, bool>) (i => target.primary_equipped_gear.id == i));
        int index = nullable.Value;
        if (bSetPrimaryReisou)
          target.primary_equipped_reisou = this.createReisou(gears, index);
        else
          target.primary_equipped_gear.equipped_reisou_player_gear_id = this.getReisouId(index);
      }
      if (target.primary_equipped_gear2 != (PlayerItem) null)
      {
        target.primary_equipped_gear2.resetByCustomDeck(true);
        nullable = ((IEnumerable<int>) this.player_gear_ids).FirstIndexOrNull<int>((Func<int, bool>) (i => target.primary_equipped_gear2.id == i));
        int index = nullable.Value;
        if (bSetPrimaryReisou)
          target.primary_equipped_reisou2 = this.createReisou(gears, index);
        else
          target.primary_equipped_gear2.equipped_reisou_player_gear_id = this.getReisouId(index);
      }
      if (!(target.primary_equipped_gear3 != (PlayerItem) null))
        return;
      target.primary_equipped_gear3.resetByCustomDeck(true);
      nullable = ((IEnumerable<int>) this.player_gear_ids).FirstIndexOrNull<int>((Func<int, bool>) (i => target.primary_equipped_gear3.id == i));
      int index1 = nullable.Value;
      if (bSetPrimaryReisou)
        target.primary_equipped_reisou3 = this.createReisou(gears, index1);
      else
        target.primary_equipped_gear3.equipped_reisou_player_gear_id = this.getReisouId(index1);
    }

    public PlayerUnit createPlayerUnitByBattleResults(
      PlayerUnit original,
      PlayerItem[] gears,
      bool bSetPrimaryReisou = false)
    {
      PlayerUnit target = PlayerCustomDeck.isCustom(original) ? JobChangeUtil.createPlayerUnit(original, this.job_id, new Func<PlayerUnit, PlayerUnit>(PlayerCustomDeck.cloneByCustom)) : JobChangeUtil.createPlayerUnit(original, this.job_id, bCloneParam: true);
      this.setGears(target, gears, bSetPrimaryReisou);
      target.equip_awake_skill_ids = new int?[0];
      target.primary_equipped_awake_skill = (PlayerAwakeSkill) null;
      target.usedPrimary = PlayerUnit.UsedPrimary.All;
      target.over_killers_player_unit_ids = new int[0];
      target.clearOverkillersParameter();
      target.resetCacheOverkillersUnits((PlayerUnit[]) null);
      target.resetOverkillersSkills();
      return target;
    }

    public PlayerItem createGear(PlayerItem[] gears, int index, bool bUnitEquipped = false)
    {
      PlayerItem gear = this.getGear(gears, index);
      if (gear != (PlayerItem) null)
      {
        gear = gear.Clone();
        gear.resetByCustomDeck(bUnitEquipped);
      }
      return gear;
    }

    public PlayerItem getGear(PlayerItem[] gears, int index)
    {
      if (gears == null)
        return (PlayerItem) null;
      int n = this.getGearId(index);
      return n == 0 ? (PlayerItem) null : Array.Find<PlayerItem>(gears, (Predicate<PlayerItem>) (x => x.id == n));
    }

    public int getGearId(int index)
    {
      return this.player_gear_ids.Length <= index ? 0 : this.player_gear_ids[index];
    }

    public PlayerItem createReisou(PlayerItem[] gears, int index)
    {
      PlayerItem reisou = this.getReisou(gears, index);
      if (reisou != (PlayerItem) null)
        reisou = reisou.Clone();
      return reisou;
    }

    public PlayerItem getReisou(PlayerItem[] gears, int index)
    {
      if (gears == null)
        return (PlayerItem) null;
      int n = this.getReisouId(index);
      return n == 0 ? (PlayerItem) null : Array.Find<PlayerItem>(gears, (Predicate<PlayerItem>) (x => x.id == n));
    }

    public int getReisouId(int index)
    {
      return this.player_reisou_ids.Length <= index ? 0 : this.player_reisou_ids[index];
    }

    public PlayerUnit[] createOverkillers(PlayerUnit[] units)
    {
      return PlayerCustomDeck.isCustom(((IEnumerable<PlayerUnit>) units).FirstOrDefault<PlayerUnit>()) ? ((IEnumerable<int>) this.over_killers_ids).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (i =>
      {
        PlayerUnit target = i == 0 || i == -1 ? (PlayerUnit) null : Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == i));
        if (target != (PlayerUnit) null)
        {
          target = PlayerCustomDeck.cloneByCustom(target);
          ((PlayerCustomDeck.ImpPlayerUnit) target).overkillersBaseId = new int?(this.player_unit_id);
        }
        return target;
      })).ToArray<PlayerUnit>() : ((IEnumerable<int>) this.over_killers_ids).Select<int, PlayerUnit>((Func<int, PlayerUnit>) (i =>
      {
        PlayerUnit overkillers = i == 0 || i == -1 ? (PlayerUnit) null : Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == i));
        if (overkillers != (PlayerUnit) null)
        {
          overkillers = overkillers.Clone();
          overkillers.resetByCustomDeck();
        }
        return overkillers;
      })).ToArray<PlayerUnit>();
    }

    public void cleanupGears(int[] disappearedGears)
    {
      if (this.player_gear_ids == null)
        return;
      for (int index = 0; index < this.player_gear_ids.Length; ++index)
      {
        if (((IEnumerable<int>) disappearedGears).Contains<int>(this.player_gear_ids[index]))
        {
          this.player_gear_ids[index] = 0;
          if (this.player_reisou_ids != null && index < this.player_reisou_ids.Length)
            this.player_reisou_ids[index] = 0;
        }
      }
    }
  }
}
