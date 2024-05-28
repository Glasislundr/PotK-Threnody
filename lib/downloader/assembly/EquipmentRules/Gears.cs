// Decompiled with JetBrains decompiler
// Type: EquipmentRules.Gears
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EquipmentRules
{
  public static class Gears
  {
    private static bool execSequentialAnd(
      Delegate[] delegates,
      PlayerItem playerGear,
      GearGear gear,
      GearKind gearKind)
    {
      if (delegates != null)
      {
        for (int index = 0; index < delegates.Length; ++index)
        {
          if (!(bool) delegates[index].DynamicInvoke((object) playerGear, (object) gear, (object) gearKind))
            return false;
        }
      }
      return true;
    }

    public static List<PlayerItem> makeList(
      PlayerUnit baseUnit,
      PlayerItem[] targets,
      int slotIndex = 0,
      List<int> includeIds = null)
    {
      Func<PlayerItem, bool> checker = Gears.createFuncPossibleEquipped(baseUnit, slotIndex);
      return !includeIds.IsNullOrEmpty<int>() ? ((IEnumerable<PlayerItem>) targets).Where<PlayerItem>((Func<PlayerItem, bool>) (g => includeIds.Contains(g.id) || checker(g))).ToList<PlayerItem>() : ((IEnumerable<PlayerItem>) targets).Where<PlayerItem>(checker).ToList<PlayerItem>();
    }

    public static Func<PlayerItem, bool> createFuncPossibleEquipped(
      PlayerUnit baseUnit,
      int slotIndex,
      PlayerItem[] equippedPlayerGears = null,
      bool bIncludeBroken = false)
    {
      if (!baseUnit.isOpenedEquippedGear3)
        return Gears.createFuncPossibleEquippedByNormal(baseUnit, slotIndex, equippedPlayerGears, bIncludeBroken);
      UnitUnit unit = baseUnit.unit;
      GearKindEnum primaryGearKind = unit.primaryGearKind;
      GearGear[] gearGearArray;
      if (equippedPlayerGears == null)
        gearGearArray = new GearGear[3]
        {
          baseUnit.equippedGear?.gear,
          baseUnit.equippedGear2?.gear,
          baseUnit.equippedGear3?.gear
        };
      else
        gearGearArray = new GearGear[3]
        {
          equippedPlayerGears[0]?.gear,
          equippedPlayerGears[1]?.gear,
          equippedPlayerGears[2]?.gear
        };
      GearGear[] source1 = gearGearArray;
      bool[] array = ((IEnumerable<GearGear>) source1).Select<GearGear, bool>((Func<GearGear, bool>) (x => x != null && x.is_jingi)).ToArray<bool>();
      int[] numArray = new int[3]{ 2, 2, 0 };
      bool flag1 = array[numArray[slotIndex]];
      List<int> intList1;
      if (source1.Length <= slotIndex || source1[slotIndex] == null)
        intList1 = new List<int>();
      else
        intList1 = new List<int>() { source1[slotIndex].ID };
      List<int> includeGear = intList1;
      MasterDataTable.UnitJob jobData = baseUnit.getJobData();
      int gearClassificationPattern = jobData.classification_GearClassificationPattern.IsValid() ? jobData.classification_GearClassificationPattern.Value : 0;
      List<GearSpecificationOfEquipmentUnit> source2 = new List<GearSpecificationOfEquipmentUnit>();
      List<GearSpecificationOfEquipmentUnit> source3 = new List<GearSpecificationOfEquipmentUnit>();
      List<GearSpecificationOfEquipmentUnit> source4 = new List<GearSpecificationOfEquipmentUnit>();
      foreach (GearSpecificationOfEquipmentUnit specificationOfEquipmentUnit in MasterData.GearSpecificationOfEquipmentUnitList)
      {
        if (specificationOfEquipmentUnit.unit_id == unit.ID)
        {
          if (specificationOfEquipmentUnit.job_id.IsValid())
            source2.Add(specificationOfEquipmentUnit);
          else
            source3.Add(specificationOfEquipmentUnit);
        }
        else if (specificationOfEquipmentUnit.unit_id.IsInvalid())
        {
          int id = jobData.ID;
          int? jobId = specificationOfEquipmentUnit.job_id;
          int valueOrDefault = jobId.GetValueOrDefault();
          if (id == valueOrDefault & jobId.HasValue)
            source4.Add(specificationOfEquipmentUnit);
        }
      }
      List<int> intList2;
      List<int> specificationUnitJobGroups = source2.GroupBy<GearSpecificationOfEquipmentUnit, int>((Func<GearSpecificationOfEquipmentUnit, int>) (x => x.job_id.Value)).ToDictionary<IGrouping<int, GearSpecificationOfEquipmentUnit>, int, List<int>>((Func<IGrouping<int, GearSpecificationOfEquipmentUnit>, int>) (k => k.Key), (Func<IGrouping<int, GearSpecificationOfEquipmentUnit>, List<int>>) (v => v.Select<GearSpecificationOfEquipmentUnit, int>((Func<GearSpecificationOfEquipmentUnit, int>) (vv => vv.group_id)).ToList<int>())).TryGetValue(jobData.ID, out intList2) ? intList2 : new List<int>();
      int[] specificationUnits = source3.Select<GearSpecificationOfEquipmentUnit, int>((Func<GearSpecificationOfEquipmentUnit, int>) (x => x.group_id)).ToArray<int>();
      int[] specificationJobs = source4.Select<GearSpecificationOfEquipmentUnit, int>((Func<GearSpecificationOfEquipmentUnit, int>) (x => x.group_id)).ToArray<int>();
      bool flag2 = specificationUnitJobGroups.Any<int>() || specificationUnits.Length != 0 || specificationJobs.Length != 0;
      Gears.CheckPossible checkPossible = new Gears.CheckPossible(Gears.checkEquip);
      if (unit.awake_unit_flag)
      {
        switch (slotIndex)
        {
          case 0:
            if (flag1)
            {
              checkPossible += new Gears.CheckPossible(Gears.excludeJingi);
              break;
            }
            if (source1[2] != null && !array[2] && (source1[1] != null && !array[1] || (GearKindEnum) source1[2].kind_GearKind == primaryGearKind))
            {
              checkPossible += new Gears.CheckPossible(Gears.onlyJingi);
              break;
            }
            break;
          case 1:
            if (flag1)
            {
              checkPossible += new Gears.CheckPossible(Gears.excludeJingi);
              break;
            }
            if (source1[2] != null && !array[2] && (source1[0] != null && !array[0] || source1[2].kind.isAssist))
            {
              checkPossible += new Gears.CheckPossible(Gears.onlyJingi);
              break;
            }
            break;
          case 2:
            if (array[0] || array[1])
              checkPossible += new Gears.CheckPossible(Gears.excludeJingi);
            else if (source1[0] != null && !array[0] && source1[1] != null && !array[1])
              checkPossible += new Gears.CheckPossible(Gears.onlyJingi);
            checkPossible += new Gears.CheckPossible(Gears.noneManaSeed);
            break;
        }
      }
      else
      {
        if (flag1)
          checkPossible += new Gears.CheckPossible(Gears.excludeJingi);
        switch (slotIndex)
        {
          case 0:
            if (source1[2] != null && !flag1)
            {
              checkPossible += new Gears.CheckPossible(Gears.onlyJingi);
              break;
            }
            break;
          case 2:
            if (source1[0] != null && !flag1)
              checkPossible += new Gears.CheckPossible(Gears.onlyJingi);
            checkPossible += new Gears.CheckPossible(Gears.noneManaSeed);
            break;
        }
      }
      if (unit.IsAllEquipUnit)
      {
        switch (slotIndex)
        {
          case 0:
            checkPossible += (Gears.CheckPossible) ((p, gear, k) => Gears.checkAllEquipUnitSlot1(gear));
            break;
          case 2:
            checkPossible += (Gears.CheckPossible) ((p, gear, k) => Gears.checkAllEquipUnitSlot3(baseUnit, gear));
            break;
          default:
            checkPossible = (Gears.CheckPossible) ((p, g, k) => false);
            break;
        }
      }
      else if (unit.awake_unit_flag)
      {
        switch (slotIndex)
        {
          case 0:
            checkPossible += (Gears.CheckPossible) ((p, gear, k) => gear.kind_GearKind == unit.kind_GearKind && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
            break;
          case 1:
            checkPossible += (Gears.CheckPossible) ((p, gear, gearKind) => gearKind.isAssist && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
            break;
          case 2:
            Func<GearGear, GearKind, bool> funcCheckWeapon = source1[0] != null ? (array[0] ? (Func<GearGear, GearKind, bool>) ((arg1, arg2) => !arg1.is_jingi && arg2.Enum == primaryGearKind) : (Func<GearGear, GearKind, bool>) ((arg1, arg2) => arg1.is_jingi && arg2.Enum == primaryGearKind)) : (Func<GearGear, GearKind, bool>) ((_, arg2) => arg2.Enum == primaryGearKind);
            Func<GearGear, GearKind, bool> funcCheckAssist = source1[1] != null ? (array[1] ? (Func<GearGear, GearKind, bool>) ((arg1, arg2) => !arg1.is_jingi && arg2.isAssist) : (Func<GearGear, GearKind, bool>) ((arg1, arg2) => arg1.is_jingi && arg2.isAssist)) : (Func<GearGear, GearKind, bool>) ((_, arg2) => arg2.isAssist);
            Func<GearGear, GearKind, bool> funcCheckGear = (Func<GearGear, GearKind, bool>) ((arg1, arg2) => funcCheckWeapon(arg1, arg2) || funcCheckAssist(arg1, arg2));
            if (flag2)
            {
              checkPossible += (Gears.CheckPossible) ((p, gear, gearKind) =>
              {
                if (gear.is_jingi)
                {
                  if (!gear.specification_of_equipment_unit_group_id.HasValue)
                    return funcCheckGear(gear, gearKind);
                  return ((IEnumerable<int>) specificationUnits).Contains<int>(gear.specification_of_equipment_unit_group_id.Value) || ((IEnumerable<int>) specificationJobs).Contains<int>(gear.specification_of_equipment_unit_group_id.Value) || specificationUnitJobGroups.Contains(gear.specification_of_equipment_unit_group_id.Value);
                }
                return funcCheckGear(gear, gearKind) && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit);
              });
              break;
            }
            checkPossible += (Gears.CheckPossible) ((p, gear, gearKind) => funcCheckGear(gear, gearKind) && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
            break;
        }
      }
      else
      {
        switch (slotIndex)
        {
          case 0:
            checkPossible += (Gears.CheckPossible) ((p, gear, gearKind) => (gearKind.isAssist || gear.kind_GearKind == unit.kind_GearKind) && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
            break;
          case 2:
            if (flag2)
            {
              checkPossible += (Gears.CheckPossible) ((p, gear, gearKind) => gear.is_jingi ? (!gear.specification_of_equipment_unit_group_id.HasValue ? gearKind.isAssist || gearKind.Enum == primaryGearKind : ((IEnumerable<int>) specificationUnits).Contains<int>(gear.specification_of_equipment_unit_group_id.Value) || ((IEnumerable<int>) specificationJobs).Contains<int>(gear.specification_of_equipment_unit_group_id.Value) || specificationUnitJobGroups.Contains(gear.specification_of_equipment_unit_group_id.Value)) : (gearKind.isAssist || gearKind.Enum == primaryGearKind) && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
              break;
            }
            checkPossible += (Gears.CheckPossible) ((p, gear, gearKind) => (gearKind.isAssist || gearKind.Enum == primaryGearKind) && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
            break;
          default:
            checkPossible = (Gears.CheckPossible) ((p, g, k) => false);
            break;
        }
      }
      if (gearClassificationPattern.IsValid())
      {
        if (slotIndex != 2)
          checkPossible += (Gears.CheckPossible) ((p, g, k) =>
          {
            int? classificationPattern = g.classification_GearClassificationPattern;
            int num = gearClassificationPattern;
            return classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue || !k.is_attack;
          });
        else
          checkPossible += (Gears.CheckPossible) ((p, g, k) =>
          {
            if (!g.is_jingi)
            {
              int? classificationPattern = g.classification_GearClassificationPattern;
              int num = gearClassificationPattern;
              if (!(classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue))
                return !k.is_attack;
            }
            return true;
          });
      }
      Delegate[] delegates = checkPossible.GetInvocationList();
      return (Func<PlayerItem, bool>) (x =>
      {
        if (!bIncludeBroken && x.broken)
          return false;
        GearGear gear = x.gear;
        if (gear == null)
          return false;
        return includeGear.Contains(gear.ID) || Gears.execSequentialAnd(delegates, x, gear, gear.kind);
      });
    }

    private static Func<PlayerItem, bool> createFuncPossibleEquippedByNormal(
      PlayerUnit baseUnit,
      int slotIndex,
      PlayerItem[] equippedPlayerGears = null,
      bool bIncludeBroken = false)
    {
      UnitUnit unit = baseUnit.unit;
      if (equippedPlayerGears == null)
        equippedPlayerGears = new PlayerItem[2]
        {
          baseUnit.equippedGear,
          baseUnit.equippedGear2
        };
      List<int> intList;
      if (equippedPlayerGears.Length <= slotIndex || !(equippedPlayerGears[slotIndex] != (PlayerItem) null))
        intList = new List<int>();
      else
        intList = new List<int>()
        {
          equippedPlayerGears[slotIndex].gear.ID
        };
      List<int> includeGear = intList;
      MasterDataTable.UnitJob jobData = baseUnit.getJobData();
      int gearClassificationPattern = jobData.classification_GearClassificationPattern.IsValid() ? jobData.classification_GearClassificationPattern.Value : 0;
      Gears.CheckPossible checkPossible1 = new Gears.CheckPossible(Gears.checkEquip);
      Gears.CheckPossible checkPossible2;
      if (unit.IsAllEquipUnit)
        checkPossible2 = slotIndex != 0 ? (Gears.CheckPossible) ((p, g, k) => false) : checkPossible1 + (Gears.CheckPossible) ((p, gear, k) => Gears.checkAllEquipUnitSlot1(gear));
      else if (unit.awake_unit_flag)
      {
        switch (slotIndex)
        {
          case 0:
            checkPossible2 = checkPossible1 + (Gears.CheckPossible) ((p, gear, k) => gear.kind_GearKind == unit.kind_GearKind && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
            break;
          case 1:
            checkPossible2 = checkPossible1 + (Gears.CheckPossible) ((p, gear, gearKind) => gearKind.isAssist && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
            break;
          default:
            checkPossible2 = (Gears.CheckPossible) ((p, g, k) => false);
            break;
        }
      }
      else
        checkPossible2 = slotIndex != 0 ? (Gears.CheckPossible) ((p, g, k) => false) : checkPossible1 + (Gears.CheckPossible) ((p, gear, gearKind) => (gearKind.isAssist || gear.kind_GearKind == unit.kind_GearKind) && gear.enableEquipmentUnitWithoutCheckAllEquipUnit(baseUnit));
      if (gearClassificationPattern.IsValid())
        checkPossible2 += (Gears.CheckPossible) ((p, g, k) =>
        {
          int? classificationPattern = g.classification_GearClassificationPattern;
          int num = gearClassificationPattern;
          return classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue || !k.is_attack;
        });
      Delegate[] delegates = checkPossible2.GetInvocationList();
      return (Func<PlayerItem, bool>) (x =>
      {
        if (!bIncludeBroken && x.broken)
          return false;
        GearGear gear = x.gear;
        if (gear == null)
          return false;
        return includeGear.Contains(gear.ID) || Gears.execSequentialAnd(delegates, x, gear, gear.kind);
      });
    }

    private static bool checkEquip(PlayerItem playerGear, GearGear gear, GearKind gearKind)
    {
      return !gear.isReisou() && gearKind.isEquip;
    }

    private static bool noneManaSeed(PlayerItem playerGear, GearGear gear, GearKind gearKind)
    {
      return !gear.isManaSeed();
    }

    private static bool excludeJingi(PlayerItem playerGear, GearGear gear, GearKind gearKind)
    {
      return !gear.is_jingi;
    }

    private static bool onlyJingi(PlayerItem playerGear, GearGear gear, GearKind gearKind)
    {
      return gear.is_jingi;
    }

    private static bool checkAllEquipUnitSlot1(GearGear gear)
    {
      return !((IEnumerable<IgnoreGear>) MasterData.IgnoreGearList).Any<IgnoreGear>((Func<IgnoreGear, bool>) (v => v.gear_id == gear.ID));
    }

    private static bool checkAllEquipUnitSlot3(PlayerUnit playerUnit, GearGear gear)
    {
      return !((IEnumerable<IgnoreGear>) MasterData.IgnoreGearList).Any<IgnoreGear>((Func<IgnoreGear, bool>) (v => v.gear_id == gear.ID)) && (!gear.is_jingi || !gear.specification_of_equipment_unit_group_id.HasValue || !gear.specificationOfEquipmentUnits.Any<int>() && !gear.specificationOfEquipmentUnitJobs.Any<KeyValuePair<int, int?>>());
    }

    public static int? getIndexJingiSpace(
      GearGear[] gears,
      bool bAwakedUnit,
      bool bOpendEquippedGear3)
    {
      if (!bOpendEquippedGear3)
        return new int?();
      int? indexJingiSpace = new int?();
      int num = ((IEnumerable<GearGear>) gears).Count<GearGear>((Func<GearGear, bool>) (x => x == null));
      if (bAwakedUnit)
      {
        if (num == 0 || gears[2] != null && gears[2].is_jingi)
          return new int?();
        if (gears[0] != null && !gears[0].is_jingi && gears[1] != null && !gears[1].is_jingi)
          indexJingiSpace = new int?(2);
        if (!indexJingiSpace.HasValue && gears[2] != null && !gears[2].is_jingi)
        {
          GearKind kind = gears[2].kind;
          if (gears[1] == null && kind.isAssist)
            indexJingiSpace = new int?(1);
          else if (gears[0] == null && !kind.isAssist)
            indexJingiSpace = new int?(0);
        }
      }
      else
      {
        if (num <= 1)
          return new int?();
        if (((IEnumerable<GearGear>) gears).Any<GearGear>((Func<GearGear, bool>) (x => x != null && x.is_jingi)))
          return new int?();
        if (gears[0] != null)
          indexJingiSpace = new int?(2);
        else if (gears[2] != null)
          indexJingiSpace = new int?(0);
      }
      return indexJingiSpace;
    }

    private delegate bool CheckPossible(PlayerItem p, GearGear g, GearKind k);
  }
}
