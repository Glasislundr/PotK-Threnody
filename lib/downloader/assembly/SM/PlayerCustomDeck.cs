// Decompiled with JetBrains decompiler
// Type: SM.PlayerCustomDeck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using MasterDataTable;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace SM
{
  [Serializable]
  public class PlayerCustomDeck : KeyCompare
  {
    public int deck_type_id;
    public int cost_limit;
    public int member_limit;
    public int[] player_unit_ids;
    public PlayerCustomDeckUnit_parameter_list[] unit_parameter_list;
    public int deck_number;
    public string name;
    private bool isModified_;

    public PlayerCustomDeck()
    {
    }

    public PlayerCustomDeck(Dictionary<string, object> json)
    {
      this._hasKey = false;
      this.deck_type_id = (int) (long) json[nameof (deck_type_id)];
      this.cost_limit = (int) (long) json[nameof (cost_limit)];
      this.member_limit = (int) (long) json[nameof (member_limit)];
      this.player_unit_ids = ((IEnumerable<object>) json[nameof (player_unit_ids)]).Select<object, int>((Func<object, int>) (s => (int) (long) s)).ToArray<int>();
      List<PlayerCustomDeckUnit_parameter_list> unitParameterListList = new List<PlayerCustomDeckUnit_parameter_list>();
      foreach (object json1 in (List<object>) json[nameof (unit_parameter_list)])
        unitParameterListList.Add(json1 == null ? (PlayerCustomDeckUnit_parameter_list) null : new PlayerCustomDeckUnit_parameter_list((Dictionary<string, object>) json1));
      this.unit_parameter_list = unitParameterListList.ToArray();
      this.deck_number = (int) (long) json[nameof (deck_number)];
      this.name = json[nameof (name)] == null ? (string) null : (string) json[nameof (name)];
    }

    public static DeckInfo createDeckInfo(PlayerCustomDeck deck)
    {
      return (DeckInfo) new PlayerCustomDeck.ImpPlayerCustomDeck(deck);
    }

    public static DeckInfo createGuildRaidDeckInfo(PlayerCustomDeck deck)
    {
      return (DeckInfo) new PlayerCustomDeck.ImpGuildRaidCustomDeck(deck);
    }

    public bool isClone { get; private set; }

    public bool isModified
    {
      get
      {
        return this.isModified_ || ((IEnumerable<PlayerCustomDeckUnit_parameter_list>) this.unit_parameter_list).Any<PlayerCustomDeckUnit_parameter_list>((Func<PlayerCustomDeckUnit_parameter_list, bool>) (x => x.isModified));
      }
      private set => this.isModified_ = value;
    }

    public void clearModified()
    {
      this.isModified_ = false;
      for (int index = 0; index < this.unit_parameter_list.Length; ++index)
        this.unit_parameter_list[index].clearModified();
    }

    public PlayerCustomDeck cloneWork(PlayerUnit[] units, PlayerItem[] gears)
    {
      PlayerCustomDeck playerCustomDeck = (PlayerCustomDeck) this.MemberwiseClone();
      playerCustomDeck.isClone = true;
      playerCustomDeck.clearModified();
      playerCustomDeck.player_unit_ids = new int[this.member_limit];
      playerCustomDeck.unit_parameter_list = new PlayerCustomDeckUnit_parameter_list[this.member_limit];
      int i;
      for (i = 0; i < this.player_unit_ids.Length; ++i)
      {
        int unitId = this.player_unit_ids[i];
        PlayerUnit original = Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == unitId));
        playerCustomDeck.player_unit_ids[i] = unitId;
        PlayerCustomDeckUnit_parameter_list unitParameterList1 = Array.Find<PlayerCustomDeckUnit_parameter_list>(this.unit_parameter_list, (Predicate<PlayerCustomDeckUnit_parameter_list>) (x => x.player_unit_id == unitId));
        PlayerCustomDeckUnit_parameter_list unitParameterList2 = unitParameterList1.clone();
        playerCustomDeck.unit_parameter_list[i] = unitParameterList2;
        UnitUnit unit = original.unit;
        int[] indexMapByCustomDeck = original.equippedGearIndexMapByCustomDeck;
        int length = indexMapByCustomDeck[indexMapByCustomDeck.Length - 1] + 1;
        if (unitParameterList2.player_gear_ids.Length < length)
        {
          PlayerUnit unitByBattleResults = unitParameterList1.createPlayerUnitByBattleResults(original, gears, true);
          unitParameterList2.player_gear_ids = new int[length];
          unitParameterList2.player_reisou_ids = new int[length];
          int[] numArray1 = new int[3];
          PlayerItem equippedGear = unitByBattleResults.equippedGear;
          numArray1[0] = (object) equippedGear != null ? equippedGear.id : 0;
          PlayerItem equippedGear2 = unitByBattleResults.equippedGear2;
          numArray1[1] = (object) equippedGear2 != null ? equippedGear2.id : 0;
          PlayerItem equippedGear3 = unitByBattleResults.equippedGear3;
          numArray1[2] = (object) equippedGear3 != null ? equippedGear3.id : 0;
          int[] numArray2 = numArray1;
          int[] numArray3 = new int[3];
          PlayerItem equippedReisou = unitByBattleResults.equippedReisou;
          numArray3[0] = (object) equippedReisou != null ? equippedReisou.id : 0;
          PlayerItem equippedReisou2 = unitByBattleResults.equippedReisou2;
          numArray3[1] = (object) equippedReisou2 != null ? equippedReisou2.id : 0;
          PlayerItem equippedReisou3 = unitByBattleResults.equippedReisou3;
          numArray3[2] = (object) equippedReisou3 != null ? equippedReisou3.id : 0;
          int[] numArray4 = numArray3;
          for (int index1 = 0; index1 < indexMapByCustomDeck.Length; ++index1)
          {
            int index2 = indexMapByCustomDeck[index1];
            unitParameterList2.player_gear_ids[index2] = numArray2[index2];
            unitParameterList2.player_reisou_ids[index2] = numArray4[index2];
          }
        }
        unitParameterList2.over_killers_ids = new int[unit.numOverkillersSlot];
        for (int index = 0; index < unitParameterList2.over_killers_ids.Length; ++index)
        {
          int num = -1;
          if (index < unitParameterList1.over_killers_ids.Length)
            num = unitParameterList1.over_killers_ids[index];
          else if (index < original.over_killers_player_unit_ids.Length && original.over_killers_player_unit_ids[index] >= 0)
            num = 0;
          unitParameterList2.over_killers_ids[index] = num;
        }
        unitParameterList2.setIndex(i);
        unitParameterList2.clearModified();
      }
      for (; i < playerCustomDeck.player_unit_ids.Length; ++i)
      {
        playerCustomDeck.unit_parameter_list[i] = new PlayerCustomDeckUnit_parameter_list()
        {
          over_killers_ids = new int[0],
          player_reisou_ids = new int[1],
          player_gear_ids = new int[1]
        };
        playerCustomDeck.unit_parameter_list[i].setIndex(i);
        playerCustomDeck.unit_parameter_list[i].clearModified();
      }
      return playerCustomDeck;
    }

    public PlayerCustomDeck cloneBySave(PlayerUnit[] units)
    {
      PlayerCustomDeck playerCustomDeck = (PlayerCustomDeck) this.MemberwiseClone();
      playerCustomDeck.isClone = true;
      playerCustomDeck.player_unit_ids = ((IEnumerable<int>) this.player_unit_ids).Where<int>((Func<int, bool>) (i => i != 0)).ToArray<int>();
      playerCustomDeck.unit_parameter_list = new PlayerCustomDeckUnit_parameter_list[playerCustomDeck.player_unit_ids.Length];
      for (int i = 0; i < playerCustomDeck.unit_parameter_list.Length; ++i)
      {
        int unitId = playerCustomDeck.player_unit_ids[i];
        UnitUnit unit = Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == unitId)).unit;
        playerCustomDeck.unit_parameter_list[i] = Array.Find<PlayerCustomDeckUnit_parameter_list>(this.unit_parameter_list, (Predicate<PlayerCustomDeckUnit_parameter_list>) (x => x.player_unit_id == unitId)).cloneBySave(unit);
        playerCustomDeck.unit_parameter_list[i].setIndex(i);
      }
      return playerCustomDeck;
    }

    public SaveDeckUnitIds[] createSaveParams(int maxSpace)
    {
      SaveDeckUnitIds[] saveParams = new SaveDeckUnitIds[Mathf.Max(this.member_limit, maxSpace)];
      for (int index = 0; index < this.unit_parameter_list.Length; ++index)
      {
        PlayerCustomDeckUnit_parameter_list unitParameter = this.unit_parameter_list[index];
        saveParams[index] = new SaveDeckUnitIds()
        {
          gears = ((IEnumerable<int>) unitParameter.player_gear_ids).ToArray<int>(),
          reisous = ((IEnumerable<int>) unitParameter.player_reisou_ids).ToArray<int>(),
          skill = unitParameter.awake_skill_id,
          job = unitParameter.job_id,
          overkillers = ((IEnumerable<int>) unitParameter.over_killers_ids).ToArray<int>()
        };
      }
      for (int length = this.unit_parameter_list.Length; length < saveParams.Length; ++length)
        saveParams[length] = new SaveDeckUnitIds()
        {
          gears = new int[0],
          reisous = new int[0],
          skill = 0,
          job = 0,
          overkillers = new int[0]
        };
      return saveParams;
    }

    public void swap(int aIndex, int bIndex)
    {
      this.isModified = true;
      int playerUnitId = this.player_unit_ids[aIndex];
      this.player_unit_ids[aIndex] = this.player_unit_ids[bIndex];
      this.player_unit_ids[bIndex] = playerUnitId;
      PlayerCustomDeckUnit_parameter_list unitParameter = this.unit_parameter_list[aIndex];
      this.unit_parameter_list[aIndex] = this.unit_parameter_list[bIndex];
      this.unit_parameter_list[bIndex] = unitParameter;
      this.unit_parameter_list[aIndex].setIndex(aIndex);
      this.unit_parameter_list[bIndex].setIndex(bIndex);
    }

    public bool updateUnits(
      int[] unitIds,
      PlayerUnit[] units,
      PlayerItem[] gears,
      PlayerAwakeSkill[] skills,
      Action<int, int> callbackSwap)
    {
      bool flag = false;
      for (int aIndex = 0; aIndex < unitIds.Length; ++aIndex)
      {
        if (unitIds[aIndex] != 0 && (aIndex >= this.player_unit_ids.Length || unitIds[aIndex] != this.player_unit_ids[aIndex]))
        {
          int bIndex = Array.IndexOf<int>(this.player_unit_ids, unitIds[aIndex]);
          if (bIndex >= 0)
          {
            this.swap(aIndex, bIndex);
            if (callbackSwap != null)
              callbackSwap(aIndex, bIndex);
            flag = true;
          }
        }
      }
      for (int index1 = 0; index1 < this.unit_parameter_list.Length; ++index1)
      {
        PlayerCustomDeckUnit_parameter_list unitParameter = this.unit_parameter_list[index1];
        for (int index2 = 0; index2 < unitParameter.over_killers_ids.Length; ++index2)
        {
          if (unitParameter.over_killers_ids[index2] > 0 && ((IEnumerable<int>) unitIds).Contains<int>(unitParameter.over_killers_ids[index2]))
            unitParameter.setOverkillers(index2, 0);
        }
      }
      int index;
      for (index = 0; index < unitIds.Length && index < this.unit_parameter_list.Length; ++index)
      {
        this.player_unit_ids[index] = unitIds[index];
        if (unitIds[index] != this.unit_parameter_list[index].player_unit_id)
          flag |= this.unit_parameter_list[index].setUnit(unitIds[index], units, gears, skills);
      }
      for (; index < this.unit_parameter_list.Length; ++index)
      {
        this.player_unit_ids[index] = 0;
        flag |= this.unit_parameter_list[index].setUnit(0, (PlayerUnit[]) null, (PlayerItem[]) null, (PlayerAwakeSkill[]) null);
      }
      return flag;
    }

    public bool updateUnit(
      int unitIndex,
      int unitId,
      PlayerUnit[] units,
      PlayerItem[] gears,
      PlayerAwakeSkill[] skills,
      Action<int, int> callbackSwap)
    {
      bool flag = unitId != this.player_unit_ids[unitIndex];
      if (unitId == 0)
      {
        this.player_unit_ids[unitIndex] = unitId;
        this.unit_parameter_list[unitIndex].setUnit(0, (PlayerUnit[]) null, (PlayerItem[]) null, (PlayerAwakeSkill[]) null);
      }
      else if (flag)
      {
        int bIndex = Array.IndexOf<int>(this.player_unit_ids, unitId);
        if (bIndex >= 0)
        {
          this.swap(unitIndex, bIndex);
          if (callbackSwap != null)
            callbackSwap(unitIndex, bIndex);
        }
        else
        {
          for (int index1 = 0; index1 < this.unit_parameter_list.Length; ++index1)
          {
            PlayerCustomDeckUnit_parameter_list unitParameter = this.unit_parameter_list[index1];
            for (int index2 = 0; index2 < unitParameter.over_killers_ids.Length; ++index2)
            {
              if (unitParameter.over_killers_ids[index2] > 0 && unitId == unitParameter.over_killers_ids[index2])
              {
                unitParameter.setOverkillers(index2, 0);
                index1 = this.unit_parameter_list.Length;
                break;
              }
            }
          }
          this.player_unit_ids[unitIndex] = unitId;
          this.unit_parameter_list[unitIndex].setUnit(unitId, units, gears, skills);
        }
      }
      return flag;
    }

    public bool setGear(PlayerItem[] gears, int index, int slotNo, int gearId)
    {
      PlayerCustomDeckUnit_parameter_list unitParameter = this.unit_parameter_list[index];
      if (unitParameter.player_gear_ids[slotNo] == gearId)
        return false;
      this.isModified = true;
      unitParameter.setModified();
      if (gearId != 0)
      {
        int index1 = ((IEnumerable<int>) unitParameter.player_gear_ids).FirstIndexOrNull<int>((Func<int, bool>) (gi => gi == gearId)) ?? -1;
        if (index1 != -1)
        {
          int playerGearId = unitParameter.player_gear_ids[slotNo];
          unitParameter.player_gear_ids[slotNo] = unitParameter.player_gear_ids[index1];
          unitParameter.player_gear_ids[index1] = playerGearId;
          int playerReisouId = unitParameter.player_reisou_ids[slotNo];
          unitParameter.player_reisou_ids[slotNo] = unitParameter.player_reisou_ids[index1];
          unitParameter.player_reisou_ids[index1] = playerReisouId;
          return true;
        }
        for (int index2 = 0; index2 < this.unit_parameter_list.Length; ++index2)
        {
          if (index2 != index)
          {
            for (int index3 = 0; index3 < this.unit_parameter_list[index2].player_gear_ids.Length; ++index3)
            {
              if (this.unit_parameter_list[index2].player_gear_ids[index3] == gearId)
              {
                this.unit_parameter_list[index2].player_gear_ids[index3] = 0;
                this.unit_parameter_list[index2].setModified();
                index2 = this.unit_parameter_list.Length;
                break;
              }
            }
          }
        }
        int reisouId = unitParameter.player_reisou_ids[slotNo];
        if (reisouId != 0 && Array.Find<PlayerItem>(gears, (Predicate<PlayerItem>) (x => x.id == gearId)).gear.kind_GearKind != Array.Find<PlayerItem>(gears, (Predicate<PlayerItem>) (x => x.id == reisouId)).gear.kind_GearKind)
          unitParameter.player_reisou_ids[slotNo] = 0;
      }
      unitParameter.player_gear_ids[slotNo] = gearId;
      return true;
    }

    public bool setReisou(int index, int slotNo, int reisouId)
    {
      PlayerCustomDeckUnit_parameter_list unitParameter = this.unit_parameter_list[index];
      if (unitParameter.player_gear_ids[slotNo] == reisouId)
        return false;
      this.isModified = true;
      unitParameter.setModified();
      if (reisouId != 0)
      {
        for (int index1 = 0; index1 < this.unit_parameter_list.Length; ++index1)
        {
          for (int index2 = 0; index2 < this.unit_parameter_list[index1].player_reisou_ids.Length; ++index2)
          {
            if (this.unit_parameter_list[index1].player_reisou_ids[index2] == reisouId)
            {
              this.unit_parameter_list[index1].player_reisou_ids[index2] = 0;
              this.unit_parameter_list[index1].setModified();
              index1 = this.unit_parameter_list.Length;
              break;
            }
          }
        }
      }
      unitParameter.player_reisou_ids[slotNo] = reisouId;
      return true;
    }

    public bool setJob(int index, int jobId, bool[] bClearGears)
    {
      PlayerCustomDeckUnit_parameter_list unitParameter = this.unit_parameter_list[index];
      if (unitParameter.job_id == jobId)
        return false;
      this.isModified = true;
      unitParameter.setJob(jobId, bClearGears);
      return true;
    }

    public bool setOverkillers(int index, int slotNo, int unitId)
    {
      PlayerCustomDeckUnit_parameter_list unitParameter1 = this.unit_parameter_list[index];
      if (unitParameter1.over_killers_ids[slotNo] == unitId)
        return false;
      this.isModified = true;
      unitParameter1.setModified();
      if (unitId != 0)
      {
        for (int index1 = 0; index1 < this.unit_parameter_list.Length; ++index1)
        {
          PlayerCustomDeckUnit_parameter_list unitParameter2 = this.unit_parameter_list[index1];
          if (index1 == index)
          {
            for (int index2 = 0; index2 < unitParameter2.over_killers_ids.Length; ++index2)
            {
              if (unitParameter2.over_killers_ids[index2] == unitId)
              {
                unitParameter2.over_killers_ids[index2] = 0;
                index1 = this.unit_parameter_list.Length;
                break;
              }
            }
          }
          else
          {
            for (int index3 = 0; index3 < unitParameter2.over_killers_ids.Length; ++index3)
            {
              if (unitParameter2.over_killers_ids[index3] == unitId)
              {
                unitParameter2.over_killers_ids[index3] = 0;
                unitParameter2.setModified();
                index1 = this.unit_parameter_list.Length;
                break;
              }
            }
          }
        }
      }
      unitParameter1.over_killers_ids[slotNo] = unitId;
      return true;
    }

    public bool setAwakeSkill(int index, int skillId)
    {
      PlayerCustomDeckUnit_parameter_list unitParameter = this.unit_parameter_list[index];
      if (unitParameter.awake_skill_id == skillId)
        return false;
      this.isModified = true;
      if (skillId != 0)
      {
        for (int index1 = 0; index1 < this.unit_parameter_list.Length; ++index1)
        {
          if (index1 != index && this.unit_parameter_list[index1].awake_skill_id == skillId)
          {
            this.unit_parameter_list[index1].setAwakeskill(0);
            break;
          }
        }
      }
      unitParameter.setAwakeskill(skillId);
      return true;
    }

    public PlayerUnit[] createPlayerUnits(
      PlayerUnit[] units,
      PlayerItem[] gears,
      PlayerAwakeSkill[] awakeSkills)
    {
      return ((IEnumerable<int>) this.player_unit_ids).Where<int>((Func<int, bool>) (i => i != 0)).Select<int, PlayerCustomDeckUnit_parameter_list>((Func<int, PlayerCustomDeckUnit_parameter_list>) (n => Array.Find<PlayerCustomDeckUnit_parameter_list>(this.unit_parameter_list, (Predicate<PlayerCustomDeckUnit_parameter_list>) (x => x.player_unit_id == n)))).Select<PlayerCustomDeckUnit_parameter_list, PlayerUnit>((Func<PlayerCustomDeckUnit_parameter_list, PlayerUnit>) (y => y.createPlayerUnit(units, gears, awakeSkills))).ToArray<PlayerUnit>();
    }

    public Dictionary<int, PlayerUnit> createGearReference(PlayerUnit[] units)
    {
      Dictionary<int, PlayerUnit> gearReference = new Dictionary<int, PlayerUnit>(this.unit_parameter_list.Length);
      foreach (PlayerCustomDeckUnit_parameter_list unitParameter in this.unit_parameter_list)
      {
        PlayerCustomDeckUnit_parameter_list p = unitParameter;
        if (p.player_unit_id != 0)
        {
          PlayerUnit playerUnit = Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == p.player_unit_id));
          for (int index = 0; index < p.player_gear_ids.Length; ++index)
          {
            if (p.player_gear_ids[index] != 0)
              gearReference[p.player_gear_ids[index]] = playerUnit;
          }
        }
      }
      return gearReference;
    }

    public int getEquippedReisouId(int gearId)
    {
      foreach (PlayerCustomDeckUnit_parameter_list unitParameter in this.unit_parameter_list)
      {
        if (unitParameter.player_unit_id != 0)
        {
          int? nullable = ((IEnumerable<int>) unitParameter.player_gear_ids).FirstIndexOrNull<int>((Func<int, bool>) (i => i == gearId));
          if (nullable.HasValue)
            return unitParameter.player_reisou_ids[nullable.Value];
        }
      }
      return 0;
    }

    public Dictionary<int, Tuple<PlayerUnit, PlayerItem>> createReisouReference(
      PlayerUnit[] units,
      PlayerItem[] gears)
    {
      Dictionary<int, Tuple<PlayerUnit, PlayerItem>> reisouReference = new Dictionary<int, Tuple<PlayerUnit, PlayerItem>>(this.unit_parameter_list.Length);
      foreach (PlayerCustomDeckUnit_parameter_list unitParameter in this.unit_parameter_list)
      {
        PlayerCustomDeckUnit_parameter_list p = unitParameter;
        if (p.player_unit_id != 0)
        {
          PlayerUnit playerUnit = Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == p.player_unit_id));
          for (int n = 0; n < p.player_reisou_ids.Length; ++n)
          {
            if (p.player_gear_ids[n] != 0 && p.player_reisou_ids[n] != 0)
              reisouReference[p.player_reisou_ids[n]] = Tuple.Create<PlayerUnit, PlayerItem>(playerUnit, Array.Find<PlayerItem>(gears, (Predicate<PlayerItem>) (y => y.id == p.player_gear_ids[n])));
          }
        }
      }
      return reisouReference;
    }

    public Dictionary<int, PlayerUnit> createOverkillersReference(PlayerUnit[] units)
    {
      Dictionary<int, PlayerUnit> overkillersReference = new Dictionary<int, PlayerUnit>(this.unit_parameter_list.Length);
      foreach (PlayerCustomDeckUnit_parameter_list unitParameter in this.unit_parameter_list)
      {
        PlayerCustomDeckUnit_parameter_list p = unitParameter;
        if (p.player_unit_id != 0)
        {
          PlayerUnit playerUnit = Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == p.player_unit_id));
          for (int index = 0; index < p.over_killers_ids.Length; ++index)
          {
            switch (p.over_killers_ids[index])
            {
              case -1:
              case 0:
                continue;
              default:
                overkillersReference[p.over_killers_ids[index]] = playerUnit;
                continue;
            }
          }
        }
      }
      return overkillersReference;
    }

    public PlayerUnit[] createOverkillers(PlayerUnit[] units)
    {
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      for (int n = 0; n < this.player_unit_ids.Length; ++n)
      {
        if (this.player_unit_ids[n] != 0)
        {
          PlayerCustomDeckUnit_parameter_list unitParameterList = Array.Find<PlayerCustomDeckUnit_parameter_list>(this.unit_parameter_list, (Predicate<PlayerCustomDeckUnit_parameter_list>) (x => x.player_unit_id == this.player_unit_ids[n]));
          if (unitParameterList != null)
            playerUnitList.AddRange(((IEnumerable<PlayerUnit>) unitParameterList.createOverkillers(units)).Where<PlayerUnit>((Func<PlayerUnit, bool>) (y => y != (PlayerUnit) null)));
        }
      }
      return playerUnitList.ToArray();
    }

    public Dictionary<int, PlayerUnit> createAwakeSkillReference(PlayerUnit[] units)
    {
      Dictionary<int, PlayerUnit> awakeSkillReference = new Dictionary<int, PlayerUnit>(this.unit_parameter_list.Length);
      foreach (PlayerCustomDeckUnit_parameter_list unitParameter in this.unit_parameter_list)
      {
        PlayerCustomDeckUnit_parameter_list p = unitParameter;
        if (p.player_unit_id != 0 && p.awake_skill_id != 0)
          awakeSkillReference[p.awake_skill_id] = Array.Find<PlayerUnit>(units, (Predicate<PlayerUnit>) (x => x.id == p.player_unit_id));
      }
      return awakeSkillReference;
    }

    public void cleanupGears(int[] disappearedGears)
    {
      for (int index = 0; index < this.unit_parameter_list.Length; ++index)
        this.unit_parameter_list[index].cleanupGears(disappearedGears);
    }

    public PlayerUnit[] player_units
    {
      get
      {
        if (!((IEnumerable<int>) this.player_unit_ids).Any<int>((Func<int, bool>) (i => i != 0)))
          return System.Linq.Enumerable.Repeat<PlayerUnit>((PlayerUnit) null, this.member_limit).ToArray<PlayerUnit>();
        PlayerUnit[] units = SMManager.Get<PlayerUnit[]>();
        PlayerItem[] array = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear != null)).ToArray<PlayerItem>();
        PlayerAwakeSkill[] awakeSkills = SMManager.Get<PlayerAwakeSkill[]>();
        PlayerUnit[] playerUnits = new PlayerUnit[this.member_limit];
        for (int index = 0; index < playerUnits.Length; ++index)
        {
          int unitId = index < this.player_unit_ids.Length ? this.player_unit_ids[index] : 0;
          PlayerCustomDeckUnit_parameter_list unitParameterList = unitId != 0 ? Array.Find<PlayerCustomDeckUnit_parameter_list>(this.unit_parameter_list, (Predicate<PlayerCustomDeckUnit_parameter_list>) (x => x.player_unit_id == unitId)) : (PlayerCustomDeckUnit_parameter_list) null;
          if (unitParameterList != null)
            playerUnits[index] = unitParameterList.createPlayerUnit(units, array, awakeSkills);
        }
        return playerUnits;
      }
    }

    public int total_combat
    {
      get
      {
        return ((IEnumerable<PlayerUnit>) this.player_units).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => !(x != (PlayerUnit) null) ? 0 : Judgement.NonBattleParameter.FromPlayerUnit(x).Combat));
      }
    }

    public int cost
    {
      get
      {
        return ((IEnumerable<PlayerUnit>) this.player_units).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => (object) x == null ? 0 : x.cost));
      }
    }

    public static PlayerUnit cloneByCustom(PlayerUnit target)
    {
      return (PlayerUnit) new PlayerCustomDeck.ImpPlayerUnit(target);
    }

    public static bool isCustom(PlayerUnit target) => target is PlayerCustomDeck.ImpPlayerUnit;

    public static PlayerItem cloneByCustom(PlayerItem target)
    {
      return (PlayerItem) new PlayerCustomDeck.ImpPlayerGear(target);
    }

    public static bool isCustom(PlayerItem target) => target is PlayerCustomDeck.ImpPlayerGear;

    private class ImpPlayerCustomDeck : DeckInfo
    {
      private PlayerCustomDeck deck_;
      private PlayerUnit[] player_units_;
      private int? total_combat_;
      private int? cost_;

      public ImpPlayerCustomDeck(PlayerCustomDeck d) => this.deck_ = d;

      public override void resetDeck<T>(T d)
      {
        if (typeof (T) != typeof (PlayerCustomDeck))
        {
          Debug.LogError((object) string.Format("failed! ImpPlayerCustomDeck.resetDeck({0});", (object) typeof (T)));
        }
        else
        {
          this.deck_ = (object) d as PlayerCustomDeck;
          this.player_units_ = (PlayerUnit[]) null;
          this.total_combat_ = new int?();
          this.cost_ = new int?();
        }
      }

      public override bool isNormal => false;

      public override PlayerDeck deck => (PlayerDeck) null;

      public override bool isSea => false;

      public override PlayerSeaDeck seaDeck => (PlayerSeaDeck) null;

      public override bool isCustom => true;

      public override PlayerCustomDeck customDeck => this.deck_;

      public override string name => this.deck_.name;

      public override int deck_type_id => this.deck_.deck_type_id;

      public override int member_limit => this.deck_.member_limit;

      public override int[] player_unit_ids => this.deck_.player_unit_ids;

      public override int cost_limit => this.deck_.cost_limit;

      public override int deck_number => this.deck_.deck_number;

      public override PlayerUnit[] player_units
      {
        get => this.player_units_ ?? (this.player_units_ = this.deck_.player_units);
      }

      public override int total_combat
      {
        get
        {
          return !this.total_combat_.HasValue ? (this.total_combat_ = new int?(this.deck_.total_combat)).Value : this.total_combat_.Value;
        }
      }

      public override int cost
      {
        get
        {
          return !this.cost_.HasValue ? (this.cost_ = new int?(this.deck_.cost)).Value : this.cost_.Value;
        }
      }
    }

    private class ImpGuildRaidCustomDeck : PlayerCustomDeck.ImpPlayerCustomDeck
    {
      public ImpGuildRaidCustomDeck(PlayerCustomDeck d)
        : base(d)
      {
      }
    }

    public class ImpPlayerUnit : PlayerUnit
    {
      public int? overkillersBaseId { get; set; }

      protected override int? getOverkillersBaseId() => this.overkillersBaseId;

      public ImpPlayerUnit(PlayerUnit src)
      {
        foreach (FieldInfo field in typeof (PlayerUnit).GetFields(BindingFlags.Instance | BindingFlags.Public))
          field.SetValue((object) this, field.GetValue((object) src));
        this._hasKey = true;
        this._key = (object) this.id;
        this.resetCacheMember();
        this.resetByCustomDeck();
      }
    }

    private class ImpPlayerGear : PlayerItem
    {
      public ImpPlayerGear(PlayerItem src)
      {
        foreach (FieldInfo field in typeof (PlayerItem).GetFields(BindingFlags.Instance | BindingFlags.Public))
          field.SetValue((object) this, field.GetValue((object) src));
        this._hasKey = true;
        this._key = (object) this.id;
        this.resetByCustomDeck();
      }
    }
  }
}
