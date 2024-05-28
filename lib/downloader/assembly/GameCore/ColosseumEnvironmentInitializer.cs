// Decompiled with JetBrains decompiler
// Type: GameCore.ColosseumEnvironmentInitializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore
{
  public class ColosseumEnvironmentInitializer
  {
    private static BL.Skill createSkill(PlayerUnitSkills playerSkill)
    {
      BL.Skill skill = new BL.Skill();
      skill.id = playerSkill.skill.ID;
      skill.level = playerSkill.level;
      skill.initSkillCounts();
      return skill;
    }

    private static BL.Skill createSkill(GearGearSkill gearSkill)
    {
      BL.Skill skill = new BL.Skill();
      skill.id = gearSkill.skill.ID;
      skill.level = gearSkill.skill_level;
      skill.initSkillCounts();
      return skill;
    }

    private static BL.Skill createSkill(GearReisouSkill reisouSkill)
    {
      BL.Skill skill = new BL.Skill();
      skill.id = reisouSkill.skill.ID;
      skill.level = reisouSkill.skill_level;
      skill.initSkillCounts();
      return skill;
    }

    private static BL.Skill createSkill(PlayerAwakeSkill awakeSkill)
    {
      BL.Skill skill = new BL.Skill();
      skill.id = awakeSkill.masterData.ID;
      skill.level = awakeSkill.level;
      skill.initSkillCounts();
      return skill;
    }

    private static BL.MagicBullet createMagicBullet(PlayerUnitSkills playerSkill)
    {
      return new BL.MagicBullet()
      {
        skillId = playerSkill.skill.ID
      };
    }

    private static BL.MagicBullet createMagicBullet(GearGearSkill gearSkill)
    {
      return new BL.MagicBullet()
      {
        skillId = gearSkill.skill.ID
      };
    }

    private static BL.MagicBullet createMagicBullet(GearReisouSkill reisouSkill)
    {
      return new BL.MagicBullet()
      {
        skillId = reisouSkill.skill.ID
      };
    }

    private static BL.MagicBullet createMagicBullet(PlayerAwakeSkill awakeSkill)
    {
      return new BL.MagicBullet()
      {
        skillId = awakeSkill.masterData.ID
      };
    }

    private static BL.MagicBullet createMagicBullet(IAttackMethod attackMethod)
    {
      return new BL.MagicBullet()
      {
        skillId = attackMethod.skill.ID,
        attackMethod = attackMethod
      };
    }

    public static BL.Unit createUnitByPlayerUnit(PlayerUnit pu, int index, bool friend)
    {
      pu.resetOnceOverkillers();
      List<PlayerUnitSkills> list1 = ((IEnumerable<PlayerUnitSkills>) pu.skills).Concat<PlayerUnitSkills>((IEnumerable<PlayerUnitSkills>) pu.retrofitSkills).ToList<PlayerUnitSkills>();
      PlayerUnitSkills[] array1 = list1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)).ToArray<PlayerUnitSkills>();
      PlayerUnitSkills[] array2 = list1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.release)).ToArray<PlayerUnitSkills>();
      PlayerUnitSkills[] array3 = list1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).ToArray<PlayerUnitSkills>();
      PlayerUnitSkills[] array4 = list1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)).ToArray<PlayerUnitSkills>();
      IAttackMethod[] array5 = ((IEnumerable<IAttackMethod>) pu.battleOptionAttacks).Where<IAttackMethod>((Func<IAttackMethod, bool>) (v => v.kind.Enum == GearKindEnum.magic)).ToArray<IAttackMethod>();
      List<GearGearSkill> source1 = pu.equippedGear != (PlayerItem) null ? ((IEnumerable<GearGearSkill>) pu.equippedGear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)).ToList<GearGearSkill>() : new List<GearGearSkill>();
      List<GearGearSkill> source2 = pu.equippedGear != (PlayerItem) null ? ((IEnumerable<GearGearSkill>) pu.equippedGear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)).ToList<GearGearSkill>() : new List<GearGearSkill>();
      List<GearGearSkill> source3 = pu.equippedGear != (PlayerItem) null ? ((IEnumerable<GearGearSkill>) pu.equippedGear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).ToList<GearGearSkill>() : new List<GearGearSkill>();
      if (pu.equippedGear2 != (PlayerItem) null)
      {
        source1.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear2.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)));
        source2.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear2.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)));
        source3.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear2.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)));
      }
      if (pu.equippedGear3 != (PlayerItem) null)
      {
        source1.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear3.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)));
        source2.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear3.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)));
        source3.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear3.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)));
      }
      List<GearReisouSkill> source4 = !(pu.equippedGear != (PlayerItem) null) || !(pu.equippedReisou != (PlayerItem) null) ? new List<GearReisouSkill>() : ((IEnumerable<GearReisouSkill>) pu.equippedReisou.getReisouSkills(pu.equippedGear.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)).ToList<GearReisouSkill>();
      List<GearReisouSkill> source5 = !(pu.equippedGear != (PlayerItem) null) || !(pu.equippedReisou != (PlayerItem) null) ? new List<GearReisouSkill>() : ((IEnumerable<GearReisouSkill>) pu.equippedReisou.getReisouSkills(pu.equippedGear.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)).ToList<GearReisouSkill>();
      List<GearReisouSkill> source6 = !(pu.equippedGear != (PlayerItem) null) || !(pu.equippedReisou != (PlayerItem) null) ? new List<GearReisouSkill>() : ((IEnumerable<GearReisouSkill>) pu.equippedReisou.getReisouSkills(pu.equippedGear.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).ToList<GearReisouSkill>();
      if (pu.equippedGear2 != (PlayerItem) null && pu.equippedReisou2 != (PlayerItem) null)
      {
        source4.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou2.getReisouSkills(pu.equippedGear2.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)));
        source5.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou2.getReisouSkills(pu.equippedGear2.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)));
        source6.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou2.getReisouSkills(pu.equippedGear2.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)));
      }
      if (pu.equippedGear3 != (PlayerItem) null && pu.equippedReisou3 != (PlayerItem) null)
      {
        source4.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou3.getReisouSkills(pu.equippedGear3.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)));
        source5.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou3.getReisouSkills(pu.equippedGear3.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)));
        source6.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou3.getReisouSkills(pu.equippedGear3.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)));
      }
      PlayerAwakeSkill[] source7;
      if (pu.equippedExtraSkill == null)
        source7 = new PlayerAwakeSkill[0];
      else
        source7 = new PlayerAwakeSkill[1]
        {
          pu.equippedExtraSkill
        };
      PlayerAwakeSkill[] array6 = ((IEnumerable<PlayerAwakeSkill>) source7).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => v.masterData.skill_type == BattleskillSkillType.command)).ToArray<PlayerAwakeSkill>();
      PlayerAwakeSkill[] array7 = ((IEnumerable<PlayerAwakeSkill>) source7).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => v.masterData.skill_type == BattleskillSkillType.duel)).ToArray<PlayerAwakeSkill>();
      PlayerAwakeSkill[] array8 = ((IEnumerable<PlayerAwakeSkill>) source7).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => v.masterData.skill_type == BattleskillSkillType.magic)).ToArray<PlayerAwakeSkill>();
      BL.Skill skill = array2.Length == 0 ? (BL.Skill) null : ColosseumEnvironmentInitializer.createSkill(((IEnumerable<PlayerUnitSkills>) array2).First<PlayerUnitSkills>());
      List<BL.Skill> list2 = ((IEnumerable<PlayerUnitSkills>) array1).Select<PlayerUnitSkills, BL.Skill>((Func<PlayerUnitSkills, BL.Skill>) (v => ColosseumEnvironmentInitializer.createSkill(v))).ToList<BL.Skill>();
      IEnumerable<BL.MagicBullet> first1 = ((IEnumerable<PlayerUnitSkills>) array3).Select<PlayerUnitSkills, BL.MagicBullet>((Func<PlayerUnitSkills, BL.MagicBullet>) (v => ColosseumEnvironmentInitializer.createMagicBullet(v)));
      IEnumerable<BL.Skill> second1 = ((IEnumerable<PlayerUnitSkills>) array4).Select<PlayerUnitSkills, BL.Skill>((Func<PlayerUnitSkills, BL.Skill>) (v => ColosseumEnvironmentInitializer.createSkill(v)));
      IEnumerable<BL.Skill> second2 = source1.Select<GearGearSkill, BL.Skill>((Func<GearGearSkill, BL.Skill>) (v => ColosseumEnvironmentInitializer.createSkill(v)));
      IEnumerable<BL.Skill> second3 = source2.Select<GearGearSkill, BL.Skill>((Func<GearGearSkill, BL.Skill>) (v => ColosseumEnvironmentInitializer.createSkill(v)));
      IEnumerable<BL.MagicBullet> second4 = source3.Select<GearGearSkill, BL.MagicBullet>((Func<GearGearSkill, BL.MagicBullet>) (v => ColosseumEnvironmentInitializer.createMagicBullet(v)));
      IEnumerable<BL.Skill> second5 = source4.Select<GearReisouSkill, BL.Skill>((Func<GearReisouSkill, BL.Skill>) (v => ColosseumEnvironmentInitializer.createSkill(v)));
      IEnumerable<BL.Skill> second6 = source5.Select<GearReisouSkill, BL.Skill>((Func<GearReisouSkill, BL.Skill>) (v => ColosseumEnvironmentInitializer.createSkill(v)));
      IEnumerable<BL.MagicBullet> second7 = source6.Select<GearReisouSkill, BL.MagicBullet>((Func<GearReisouSkill, BL.MagicBullet>) (v => ColosseumEnvironmentInitializer.createMagicBullet(v)));
      IEnumerable<BL.Skill> second8 = ((IEnumerable<PlayerAwakeSkill>) array6).Select<PlayerAwakeSkill, BL.Skill>((Func<PlayerAwakeSkill, BL.Skill>) (v => ColosseumEnvironmentInitializer.createSkill(v)));
      IEnumerable<BL.Skill> first2 = ((IEnumerable<PlayerAwakeSkill>) array7).Select<PlayerAwakeSkill, BL.Skill>((Func<PlayerAwakeSkill, BL.Skill>) (v => ColosseumEnvironmentInitializer.createSkill(v)));
      IEnumerable<BL.MagicBullet> second9 = ((IEnumerable<PlayerAwakeSkill>) array8).Select<PlayerAwakeSkill, BL.MagicBullet>((Func<PlayerAwakeSkill, BL.MagicBullet>) (v => ColosseumEnvironmentInitializer.createMagicBullet(v)));
      IEnumerable<BL.MagicBullet> second10 = ((IEnumerable<IAttackMethod>) array5).Select<IAttackMethod, BL.MagicBullet>((Func<IAttackMethod, BL.MagicBullet>) (v => ColosseumEnvironmentInitializer.createMagicBullet(v)));
      BL.Weapon weapon = new BL.Weapon(pu);
      BL.Unit unitByPlayerUnit = new BL.Unit();
      unitByPlayerUnit.unitId = pu.unit.ID;
      unitByPlayerUnit.playerUnit = pu;
      unitByPlayerUnit.lv = pu.total_level;
      unitByPlayerUnit.spawnTurn = pu.spawn_turn;
      unitByPlayerUnit.weapon = weapon;
      unitByPlayerUnit.optionWeapons = ((IEnumerable<IAttackMethod>) pu.battleOptionAttacks).Where<IAttackMethod>((Func<IAttackMethod, bool>) (x => x.kind.Enum != GearKindEnum.magic)).Select<IAttackMethod, BL.Weapon>((Func<IAttackMethod, BL.Weapon>) (y => new BL.Weapon(y))).ToArray<BL.Weapon>();
      unitByPlayerUnit.gearLeftHand = pu.isLeftHandWeapon;
      unitByPlayerUnit.gearDualWield = pu.isDualWieldWeapon;
      unitByPlayerUnit.index = index;
      unitByPlayerUnit.friend = friend;
      unitByPlayerUnit.ougi = skill;
      unitByPlayerUnit.skills = list2.Concat<BL.Skill>(second2).Concat<BL.Skill>(second5).Concat<BL.Skill>(second8).ToArray<BL.Skill>();
      unitByPlayerUnit.magicBullets = first1.Concat<BL.MagicBullet>(second4).Concat<BL.MagicBullet>(second7).Concat<BL.MagicBullet>(second9).Concat<BL.MagicBullet>(second10).ToArray<BL.MagicBullet>();
      foreach (BL.MagicBullet magicBullet in unitByPlayerUnit.magicBullets)
        magicBullet.setPrefabName(unitByPlayerUnit.job);
      unitByPlayerUnit.duelSkills = first2.Concat<BL.Skill>(second1).Concat<BL.Skill>(second3).Concat<BL.Skill>(second6).OrderByDescending<BL.Skill, int>((Func<BL.Skill, int>) (x => x.skill.weight)).ThenBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).ThenByDescending<BL.Skill, int>((Func<BL.Skill, int>) (x => x.level)).ToArray<BL.Skill>();
      unitByPlayerUnit.skillfull_shield = XorShift.Range(1, 5);
      unitByPlayerUnit.skillfull_weapon = XorShift.Range(1, 5);
      return unitByPlayerUnit;
    }

    public static ColosseumEnvironment initializeData(
      ColosseumInitialData colosseumInitialData,
      ColosseumEnvironment env_)
    {
      ColosseumEnvironment colosseumEnvironment = new ColosseumEnvironment();
      if (!colosseumInitialData.transaction_id.Equals(Persist.colosseumEnv.Data.id))
      {
        Persist.colosseumEnv.Data.id = colosseumInitialData.transaction_id;
        Persist.colosseumEnv.Flush();
      }
      colosseumEnvironment.today = string.Format("{0:D2}{1:D2}", (object) colosseumInitialData.now.Month, (object) colosseumInitialData.now.Day);
      colosseumEnvironment.bonusList = colosseumInitialData.bonusList;
      colosseumEnvironment.colosseumTransactionID = colosseumInitialData.transaction_id;
      Dictionary<int, BL.Unit> dictionary1 = new Dictionary<int, BL.Unit>(5);
      Dictionary<int, PlayerItem> dictionary2 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary3 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary4 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary5 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary6 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary7 = new Dictionary<int, PlayerItem>(5);
      for (int index = 0; index < 5; ++index)
      {
        int key = 5 - index;
        PlayerUnit pu = (PlayerUnit) null;
        if (index < colosseumInitialData.player_unit_list.Length)
          pu = colosseumInitialData.player_unit_list[index];
        if (pu == (PlayerUnit) null)
        {
          dictionary1.Add(key, (BL.Unit) null);
          dictionary2.Add(key, (PlayerItem) null);
          dictionary3.Add(key, (PlayerItem) null);
          dictionary4.Add(key, (PlayerItem) null);
        }
        else
        {
          pu.primary_equipped_gear = pu.FindEquippedGear(colosseumInitialData.player_gear_list);
          pu.primary_equipped_gear2 = pu.FindEquippedGear2(colosseumInitialData.player_gear_list);
          pu.primary_equipped_gear3 = pu.FindEquippedGear3(colosseumInitialData.player_gear_list);
          pu.primary_equipped_reisou = pu.FindEquippedReisou(colosseumInitialData.player_gear_list, colosseumInitialData.player_reisou_list);
          pu.primary_equipped_reisou2 = pu.FindEquippedReisou2(colosseumInitialData.player_gear_list, colosseumInitialData.player_reisou_list);
          pu.primary_equipped_reisou3 = pu.FindEquippedReisou3(colosseumInitialData.player_gear_list, colosseumInitialData.player_reisou_list);
          dictionary2.Add(key, pu.primary_equipped_gear);
          dictionary3.Add(key, pu.primary_equipped_gear2);
          dictionary4.Add(key, pu.primary_equipped_gear3);
          dictionary5.Add(key, pu.primary_equipped_reisou);
          dictionary6.Add(key, pu.primary_equipped_reisou2);
          dictionary7.Add(key, pu.primary_equipped_reisou3);
          pu.primary_equipped_awake_skill = pu.FindEquippedExtraSkill(colosseumInitialData.player_extra_skill_list);
          pu.usedPrimary = PlayerUnit.UsedPrimary.All;
          BL.Unit unitByPlayerUnit = ColosseumEnvironmentInitializer.createUnitByPlayerUnit(pu, index, false);
          unitByPlayerUnit.isPlayerControl = true;
          unitByPlayerUnit.isPlayerForce = true;
          dictionary1.Add(key, unitByPlayerUnit);
        }
      }
      Dictionary<int, BL.Unit> dictionary8 = new Dictionary<int, BL.Unit>(5);
      Dictionary<int, PlayerItem> dictionary9 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary10 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary11 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary12 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary13 = new Dictionary<int, PlayerItem>(5);
      Dictionary<int, PlayerItem> dictionary14 = new Dictionary<int, PlayerItem>(5);
      for (int index = 0; index < 5; ++index)
      {
        int key = 5 - index;
        PlayerUnit pu = (PlayerUnit) null;
        if (index < colosseumInitialData.opponent_unit_list.Length)
          pu = colosseumInitialData.opponent_unit_list[index];
        if (pu == (PlayerUnit) null)
        {
          dictionary8.Add(key, (BL.Unit) null);
          dictionary9.Add(key, (PlayerItem) null);
          dictionary10.Add(key, (PlayerItem) null);
          dictionary11.Add(key, (PlayerItem) null);
        }
        else
        {
          pu.primary_equipped_gear = pu.FindEquippedGear(colosseumInitialData.opponent_gear_list);
          pu.primary_equipped_gear2 = pu.FindEquippedGear2(colosseumInitialData.opponent_gear_list);
          pu.primary_equipped_gear3 = pu.FindEquippedGear3(colosseumInitialData.opponent_gear_list);
          pu.primary_equipped_reisou = pu.FindEquippedReisou(colosseumInitialData.opponent_gear_list, colosseumInitialData.opponent_reisou_list);
          pu.primary_equipped_reisou2 = pu.FindEquippedReisou2(colosseumInitialData.opponent_gear_list, colosseumInitialData.opponent_reisou_list);
          pu.primary_equipped_reisou3 = pu.FindEquippedReisou3(colosseumInitialData.opponent_gear_list, colosseumInitialData.opponent_reisou_list);
          dictionary9.Add(key, pu.primary_equipped_gear);
          dictionary10.Add(key, pu.primary_equipped_gear2);
          dictionary11.Add(key, pu.primary_equipped_gear3);
          dictionary12.Add(key, pu.primary_equipped_reisou);
          dictionary13.Add(key, pu.primary_equipped_reisou2);
          dictionary14.Add(key, pu.primary_equipped_reisou3);
          pu.primary_equipped_awake_skill = pu.FindEquippedExtraSkill(colosseumInitialData.opponent_extra_skill_list);
          pu.usedPrimary = PlayerUnit.UsedPrimary.All;
          BL.Unit unitByPlayerUnit = ColosseumEnvironmentInitializer.createUnitByPlayerUnit(pu, index, false);
          unitByPlayerUnit.isPlayerControl = false;
          unitByPlayerUnit.isPlayerForce = false;
          dictionary8.Add(key, unitByPlayerUnit);
        }
      }
      colosseumEnvironment.playerUnitDict = dictionary1;
      colosseumEnvironment.playerGearDict = dictionary2;
      colosseumEnvironment.playerGearDict2 = dictionary3;
      colosseumEnvironment.playerGearDict3 = dictionary4;
      colosseumEnvironment.playerReisouDict = dictionary5;
      colosseumEnvironment.playerReisouDict2 = dictionary6;
      colosseumEnvironment.playerReisouDict3 = dictionary7;
      colosseumEnvironment.opponentUnitDict = dictionary8;
      colosseumEnvironment.opponentGearDict = dictionary9;
      colosseumEnvironment.opponentGearDict2 = dictionary10;
      colosseumEnvironment.opponentGearDict3 = dictionary11;
      colosseumEnvironment.opponentReisouDict = dictionary12;
      colosseumEnvironment.opponentReisouDict2 = dictionary13;
      colosseumEnvironment.opponentReisouDict3 = dictionary14;
      BL.Unit[] array1 = colosseumEnvironment.playerUnitDict.Values.ToArray<BL.Unit>();
      BL.Unit[] array2 = colosseumEnvironment.opponentUnitDict.Values.ToArray<BL.Unit>();
      foreach (BL.Unit unit in colosseumEnvironment.playerUnitDict.Values)
      {
        if (!(unit == (BL.Unit) null) && unit.is_leader)
        {
          foreach (PlayerUnitLeader_skills leaderSkill in unit.playerUnit.leader_skills)
            ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, leaderSkill.skill, leaderSkill.level);
        }
      }
      foreach (BL.Unit unit in colosseumEnvironment.opponentUnitDict.Values)
      {
        if (!(unit == (BL.Unit) null) && unit.is_leader)
        {
          foreach (PlayerUnitLeader_skills leaderSkill in unit.playerUnit.leader_skills)
            ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, leaderSkill.skill, leaderSkill.level);
        }
      }
      foreach (BL.Unit unit in colosseumEnvironment.playerUnitDict.Values)
      {
        if (!(unit == (BL.Unit) null))
        {
          PlayerAwakeSkill[] source;
          if (unit.playerUnit.equippedExtraSkill == null)
            source = new PlayerAwakeSkill[0];
          else
            source = new PlayerAwakeSkill[1]
            {
              unit.playerUnit.equippedExtraSkill
            };
          foreach (PlayerAwakeSkill playerAwakeSkill in ((IEnumerable<PlayerAwakeSkill>) source).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => v.masterData.skill_type == BattleskillSkillType.passive)).ToArray<PlayerAwakeSkill>())
          {
            if (!playerAwakeSkill.masterData.range_effect_passive_skill)
            {
              foreach (BattleskillEffect effect in playerAwakeSkill.masterData.Effects)
                unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, playerAwakeSkill.masterData, playerAwakeSkill.level, true));
            }
            else
              ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, playerAwakeSkill.masterData, playerAwakeSkill.level);
          }
        }
      }
      foreach (BL.Unit unit in colosseumEnvironment.opponentUnitDict.Values)
      {
        if (!(unit == (BL.Unit) null))
        {
          PlayerAwakeSkill[] source;
          if (unit.playerUnit.equippedExtraSkill == null)
            source = new PlayerAwakeSkill[0];
          else
            source = new PlayerAwakeSkill[1]
            {
              unit.playerUnit.equippedExtraSkill
            };
          foreach (PlayerAwakeSkill playerAwakeSkill in ((IEnumerable<PlayerAwakeSkill>) source).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => v.masterData.skill_type == BattleskillSkillType.passive)).ToArray<PlayerAwakeSkill>())
          {
            if (!playerAwakeSkill.masterData.range_effect_passive_skill)
            {
              foreach (BattleskillEffect effect in playerAwakeSkill.masterData.Effects)
                unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, playerAwakeSkill.masterData, playerAwakeSkill.level, true));
            }
            else
              ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, playerAwakeSkill.masterData, playerAwakeSkill.level);
          }
        }
      }
      foreach (BL.Unit unit in colosseumEnvironment.playerUnitDict.Values)
      {
        if (!(unit == (BL.Unit) null))
        {
          foreach (PlayerUnitSkills passiveSkill in unit.playerUnit.passiveSkills)
          {
            if (!passiveSkill.skill.range_effect_passive_skill)
            {
              foreach (BattleskillEffect effect in passiveSkill.skill.Effects)
                unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, passiveSkill.skill, passiveSkill.level, true));
            }
            else
              ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, passiveSkill.skill, passiveSkill.level);
          }
        }
      }
      foreach (BL.Unit unit in colosseumEnvironment.opponentUnitDict.Values)
      {
        if (!(unit == (BL.Unit) null))
        {
          foreach (PlayerUnitSkills passiveSkill in unit.playerUnit.passiveSkills)
          {
            if (!passiveSkill.skill.range_effect_passive_skill)
            {
              foreach (BattleskillEffect effect in passiveSkill.skill.Effects)
                unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, passiveSkill.skill, passiveSkill.level, true));
            }
            else
              ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, passiveSkill.skill, passiveSkill.level);
          }
        }
      }
      foreach (KeyValuePair<int, BL.Unit> keyValuePair in colosseumEnvironment.playerUnitDict)
      {
        BL.Unit unit = keyValuePair.Value;
        if (!(unit == (BL.Unit) null))
        {
          PlayerItem playerItem1 = colosseumEnvironment.playerGearDict[keyValuePair.Key];
          if (playerItem1 != (PlayerItem) null)
          {
            foreach (GearGearSkill skill in playerItem1.skills)
            {
              if (skill.skill.skill_type != BattleskillSkillType.passive || !skill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.skill_level, true, 1));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, skill.skill, skill.skill_level, 1);
            }
          }
          PlayerItem playerItem2 = colosseumEnvironment.playerGearDict2[keyValuePair.Key];
          if (playerItem2 != (PlayerItem) null)
          {
            foreach (GearGearSkill skill in playerItem2.skills)
            {
              if (skill.skill.skill_type != BattleskillSkillType.passive || !skill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.skill_level, true, 2));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, skill.skill, skill.skill_level, 2);
            }
          }
          PlayerItem playerItem3 = colosseumEnvironment.playerGearDict3[keyValuePair.Key];
          if (playerItem3 != (PlayerItem) null)
          {
            foreach (GearGearSkill skill in playerItem3.skills)
            {
              if (skill.skill.skill_type != BattleskillSkillType.passive || !skill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.skill_level, true, 3));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, skill.skill, skill.skill_level, 3);
            }
          }
          PlayerItem playerItem4 = colosseumEnvironment.playerReisouDict[keyValuePair.Key];
          if (playerItem4 != (PlayerItem) null && playerItem1 != (PlayerItem) null)
          {
            foreach (GearReisouSkill reisouSkill in playerItem4.getReisouSkills(playerItem1.entity_id))
            {
              if (reisouSkill.skill.skill_type != BattleskillSkillType.passive || !reisouSkill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in reisouSkill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, reisouSkill.skill, reisouSkill.skill_level, true, 1));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, reisouSkill.skill, reisouSkill.skill_level, 1);
            }
          }
          PlayerItem playerItem5 = colosseumEnvironment.playerReisouDict2[keyValuePair.Key];
          if (playerItem5 != (PlayerItem) null && playerItem2 != (PlayerItem) null)
          {
            foreach (GearReisouSkill reisouSkill in playerItem5.getReisouSkills(playerItem2.entity_id))
            {
              if (reisouSkill.skill.skill_type != BattleskillSkillType.passive || !reisouSkill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in reisouSkill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, reisouSkill.skill, reisouSkill.skill_level, true, 2));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, reisouSkill.skill, reisouSkill.skill_level, 2);
            }
          }
          PlayerItem playerItem6 = colosseumEnvironment.playerReisouDict3[keyValuePair.Key];
          if (playerItem6 != (PlayerItem) null && playerItem3 != (PlayerItem) null)
          {
            foreach (GearReisouSkill reisouSkill in playerItem6.getReisouSkills(playerItem3.entity_id))
            {
              if (reisouSkill.skill.skill_type != BattleskillSkillType.passive || !reisouSkill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in reisouSkill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, reisouSkill.skill, reisouSkill.skill_level, true, 3));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array1, array2, unit, reisouSkill.skill, reisouSkill.skill_level, 3);
            }
          }
        }
      }
      foreach (KeyValuePair<int, BL.Unit> keyValuePair in colosseumEnvironment.opponentUnitDict)
      {
        BL.Unit unit = keyValuePair.Value;
        if (!(unit == (BL.Unit) null))
        {
          PlayerItem playerItem7 = colosseumEnvironment.opponentGearDict[keyValuePair.Key];
          if (playerItem7 != (PlayerItem) null)
          {
            foreach (GearGearSkill skill in playerItem7.skills)
            {
              if (skill.skill.skill_type != BattleskillSkillType.passive || !skill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.skill_level, true, 1));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, skill.skill, skill.skill_level, 1);
            }
          }
          PlayerItem playerItem8 = colosseumEnvironment.opponentGearDict2[keyValuePair.Key];
          if (playerItem8 != (PlayerItem) null)
          {
            foreach (GearGearSkill skill in playerItem8.skills)
            {
              if (skill.skill.skill_type != BattleskillSkillType.passive || !skill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.skill_level, true, 2));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, skill.skill, skill.skill_level, 2);
            }
          }
          PlayerItem playerItem9 = colosseumEnvironment.opponentGearDict3[keyValuePair.Key];
          if (playerItem9 != (PlayerItem) null)
          {
            foreach (GearGearSkill skill in playerItem9.skills)
            {
              if (skill.skill.skill_type != BattleskillSkillType.passive || !skill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in skill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill.skill, skill.skill_level, true, 3));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, skill.skill, skill.skill_level, 3);
            }
          }
          PlayerItem playerItem10 = colosseumEnvironment.opponentReisouDict[keyValuePair.Key];
          if (playerItem7 != (PlayerItem) null && playerItem10 != (PlayerItem) null)
          {
            foreach (GearReisouSkill reisouSkill in playerItem10.getReisouSkills(playerItem7.entity_id))
            {
              if (reisouSkill.skill.skill_type != BattleskillSkillType.passive || !reisouSkill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in reisouSkill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, reisouSkill.skill, reisouSkill.skill_level, true, 1));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, reisouSkill.skill, reisouSkill.skill_level, 1);
            }
          }
          PlayerItem playerItem11 = colosseumEnvironment.opponentReisouDict2[keyValuePair.Key];
          if (playerItem8 != (PlayerItem) null && playerItem11 != (PlayerItem) null)
          {
            foreach (GearReisouSkill reisouSkill in playerItem11.getReisouSkills(playerItem8.entity_id))
            {
              if (reisouSkill.skill.skill_type != BattleskillSkillType.passive || !reisouSkill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in reisouSkill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, reisouSkill.skill, reisouSkill.skill_level, true, 2));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, reisouSkill.skill, reisouSkill.skill_level, 2);
            }
          }
          PlayerItem playerItem12 = colosseumEnvironment.opponentReisouDict3[keyValuePair.Key];
          if (playerItem9 != (PlayerItem) null && playerItem12 != (PlayerItem) null)
          {
            foreach (GearReisouSkill reisouSkill in playerItem12.getReisouSkills(playerItem9.entity_id))
            {
              if (reisouSkill.skill.skill_type != BattleskillSkillType.passive || !reisouSkill.skill.range_effect_passive_skill)
              {
                foreach (BattleskillEffect effect in reisouSkill.skill.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, reisouSkill.skill, reisouSkill.skill_level, true, 3));
              }
              else
                ColosseumEnvironmentInitializer.setRangeSkills(array2, array1, unit, reisouSkill.skill, reisouSkill.skill_level, 3);
            }
          }
        }
      }
      foreach (BL.Unit beUnit in colosseumEnvironment.playerUnitDict.Values)
      {
        if (!(beUnit == (BL.Unit) null))
        {
          beUnit.setParameter(Judgement.BattleParameter.FromBeColosseumUnit(beUnit, beUnit.playerUnit.equippedGear, beUnit.playerUnit.equippedGear2, beUnit.playerUnit.equippedReisou, beUnit.playerUnit.equippedReisou2, beUnit.playerUnit.equippedReisou3));
          beUnit.hp = beUnit.parameter.Hp;
          foreach (BL.MagicBullet magicBullet in beUnit.magicBullets)
            magicBullet.setAdditionalCost(beUnit.hp);
        }
      }
      foreach (BL.Unit beUnit in colosseumEnvironment.opponentUnitDict.Values)
      {
        if (!(beUnit == (BL.Unit) null))
        {
          beUnit.setParameter(Judgement.BattleParameter.FromBeColosseumUnit(beUnit, beUnit.playerUnit.equippedGear, beUnit.playerUnit.equippedGear2, beUnit.playerUnit.equippedReisou, beUnit.playerUnit.equippedReisou2, beUnit.playerUnit.equippedReisou3));
          beUnit.hp = beUnit.parameter.Hp;
          foreach (BL.MagicBullet magicBullet in beUnit.magicBullets)
            magicBullet.setAdditionalCost(beUnit.hp);
        }
      }
      IEnumerable<BL.Unit> units1 = colosseumEnvironment.playerUnitDict.Values.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x != (BL.Unit) null));
      IEnumerable<BL.Unit> units2 = colosseumEnvironment.opponentUnitDict.Values.Where<BL.Unit>((Func<BL.Unit, bool>) (x => x != (BL.Unit) null));
      foreach (BL.ISkillEffectListUnit beUnit in units1)
        BattleFuncs.createBattleSkillEffectParams(beUnit, units1, units2);
      foreach (BL.ISkillEffectListUnit beUnit in units2)
        BattleFuncs.createBattleSkillEffectParams(beUnit, units2, units1);
      return colosseumEnvironment;
    }

    public static void setRangeSkills(
      BL.Unit[] units,
      BL.Unit[] targets,
      BL.Unit unit,
      BattleskillSkill skill,
      int level,
      int gearIndex = 0)
    {
      if (skill.target_type == BattleskillTargetType.myself || skill.target_type == BattleskillTargetType.player_single || skill.target_type == BattleskillTargetType.player_range)
      {
        foreach (BL.Unit unit1 in units)
        {
          if (!(unit1 == (BL.Unit) null))
          {
            foreach (BattleskillEffect effect in skill.Effects)
              unit1.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex));
          }
        }
      }
      else if (skill.target_type == BattleskillTargetType.enemy_single || skill.target_type == BattleskillTargetType.enemy_range)
      {
        foreach (BL.Unit target in targets)
        {
          if (!(target == (BL.Unit) null))
          {
            foreach (BattleskillEffect effect in skill.Effects)
              target.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex));
          }
        }
      }
      else
      {
        if (skill.target_type != BattleskillTargetType.complex_single && skill.target_type != BattleskillTargetType.complex_range)
          return;
        foreach (BattleskillEffect effect in skill.Effects)
        {
          if (effect.is_targer_enemy)
          {
            foreach (BL.Unit target in targets)
            {
              if (!(target == (BL.Unit) null))
                target.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex));
            }
          }
          else
          {
            foreach (BL.Unit unit2 in units)
            {
              if (!(unit2 == (BL.Unit) null))
                unit2.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex));
            }
          }
        }
      }
    }
  }
}
