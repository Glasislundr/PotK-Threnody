// Decompiled with JetBrains decompiler
// Type: GameCore.BattleLogicInitializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using AI.Logic;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnitRegulation;
using UnityEngine;

#nullable disable
namespace GameCore
{
  public class BattleLogicInitializer
  {
    private BL.DropData createFieldEvent(Reward reward)
    {
      return new BL.DropData() { reward = reward };
    }

    private Future<BL.Skill> createSkill(PlayerUnitSkills playerSkill)
    {
      return Future.Single<BL.Skill>(this.createSkillCommon(playerSkill.skill_id, playerSkill.level));
    }

    private Future<BL.Skill> createSkill(GearGearSkill gearSkill)
    {
      return Future.Single<BL.Skill>(this.createSkillCommon(gearSkill.skill_BattleskillSkill, gearSkill.skill_level));
    }

    private Future<BL.Skill> createSkill(GearReisouSkill reisouSkill)
    {
      return Future.Single<BL.Skill>(this.createSkillCommon(reisouSkill.skill_BattleskillSkill, reisouSkill.skill_level));
    }

    private Future<BL.Skill> createSkill(PlayerAwakeSkill awakeSkill)
    {
      return Future.Single<BL.Skill>(this.createSkillCommon(awakeSkill.skill_id, awakeSkill.level));
    }

    private BL.Skill createSkillCommon(int skillId, int level)
    {
      BattleskillSkill battleskillSkill;
      if (!MasterData.BattleskillSkill.TryGetValue(skillId, out battleskillSkill))
      {
        Debug.LogError((object) ("Key not Found: MasterData.BattleskillSkill[" + (object) skillId + "]"));
        return (BL.Skill) null;
      }
      BL.Skill skillCommon = new BL.Skill();
      skillCommon.id = skillId;
      skillCommon.level = level;
      skillCommon.useTurn = battleskillSkill.charge_turn - (level - 1);
      if (battleskillSkill.max_use_count != 0 && skillCommon.useTurn < battleskillSkill.max_use_count)
        skillCommon.useTurn = battleskillSkill.max_use_count;
      skillCommon.remain = battleskillSkill.use_count == 0 ? new int?() : new int?(battleskillSkill.use_count + (level - 1));
      skillCommon.initSkillCounts();
      return skillCommon;
    }

    private Future<BL.MagicBullet> createMagicBullet(PlayerUnitSkills playerSkill)
    {
      return Future.Single<BL.MagicBullet>(new BL.MagicBullet()
      {
        skillId = playerSkill.skill.ID
      });
    }

    private Future<BL.MagicBullet> createMagicBullet(GearGearSkill gearSkill)
    {
      return Future.Single<BL.MagicBullet>(new BL.MagicBullet()
      {
        skillId = gearSkill.skill.ID
      });
    }

    private Future<BL.MagicBullet> createMagicBullet(GearReisouSkill reisouSkill)
    {
      return Future.Single<BL.MagicBullet>(new BL.MagicBullet()
      {
        skillId = reisouSkill.skill.ID
      });
    }

    private Future<BL.MagicBullet> createMagicBullet(PlayerAwakeSkill awakeSkill)
    {
      return Future.Single<BL.MagicBullet>(new BL.MagicBullet()
      {
        skillId = awakeSkill.masterData.ID
      });
    }

    private Future<BL.MagicBullet> createMagicBullet(IAttackMethod attackMethod)
    {
      return Future.Single<BL.MagicBullet>(new BL.MagicBullet()
      {
        skillId = attackMethod.skill.ID,
        attackMethod = attackMethod
      });
    }

    private IEnumerator createFutureUnit(
      Promise<BL.Unit> promise,
      PlayerUnit pu,
      int index,
      bool friend,
      Future<BL.Skill> ougiF,
      Future<BL.Skill> SEASkillF,
      Future<List<BL.Skill>> skillsF,
      Future<List<BL.MagicBullet>> magicBulletsF,
      Future<List<BL.Skill>> duelSkillsF,
      Future<List<BL.Skill>> gearCommandSkillsF,
      Future<List<BL.Skill>> gearDuelSkillsF,
      Future<List<BL.MagicBullet>> gearMagicBulletsF,
      Future<List<BL.Skill>> reisouCommandSkillsF,
      Future<List<BL.Skill>> reisouDuelSkillsF,
      Future<List<BL.MagicBullet>> reisouMagicBulletsF,
      Future<List<BL.Skill>> awakeCommandSkillsF,
      Future<List<BL.Skill>> awakeDuelSkillsF,
      Future<List<BL.MagicBullet>> awakeMagicBulletsF,
      Future<List<BL.MagicBullet>> optionMagicBulletsF,
      int call_same_character_id,
      bool disableSkills)
    {
      IEnumerator e = ougiF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = SEASkillF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = skillsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = magicBulletsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = duelSkillsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = gearCommandSkillsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = gearDuelSkillsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = gearMagicBulletsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = reisouCommandSkillsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = reisouDuelSkillsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = reisouMagicBulletsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = awakeCommandSkillsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = awakeDuelSkillsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = awakeMagicBulletsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      e = optionMagicBulletsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      BL.Skill result1 = ougiF.Result;
      BL.Skill result2 = SEASkillF.Result;
      List<BL.Skill> result3 = skillsF.Result;
      List<BL.MagicBullet> result4 = magicBulletsF.Result;
      List<BL.Skill> result5 = duelSkillsF.Result;
      List<BL.Skill> result6 = gearCommandSkillsF.Result;
      List<BL.Skill> result7 = gearDuelSkillsF.Result;
      List<BL.MagicBullet> result8 = gearMagicBulletsF.Result;
      List<BL.Skill> result9 = reisouCommandSkillsF.Result;
      List<BL.Skill> result10 = reisouDuelSkillsF.Result;
      List<BL.MagicBullet> result11 = reisouMagicBulletsF.Result;
      List<BL.Skill> result12 = awakeCommandSkillsF.Result;
      List<BL.Skill> result13 = awakeDuelSkillsF.Result;
      List<BL.MagicBullet> result14 = awakeMagicBulletsF.Result;
      List<BL.MagicBullet> result15 = optionMagicBulletsF.Result;
      BL.Weapon weapon = new BL.Weapon(pu);
      BL.Unit unit = new BL.Unit();
      unit.specificId = pu.id;
      unit.unitId = pu.unit.ID;
      unit.playerUnit = pu;
      unit.lv = pu.total_level;
      unit.spawnTurn = pu.spawn_turn;
      unit.weapon = weapon;
      unit.optionWeapons = ((IEnumerable<IAttackMethod>) pu.battleOptionAttacks).Where<IAttackMethod>((Func<IAttackMethod, bool>) (x => x.kind.Enum != GearKindEnum.magic && !disableSkills)).Select<IAttackMethod, BL.Weapon>((Func<IAttackMethod, BL.Weapon>) (y => new BL.Weapon(y))).ToArray<BL.Weapon>();
      unit.gearLeftHand = pu.isLeftHandWeapon;
      unit.gearDualWield = pu.isDualWieldWeapon;
      unit.AIMoveGroup = pu.ai_move_group;
      unit.AIMoveGroupOrder = pu.ai_move_group_order;
      unit.AIMoveTargetX = pu.ai_move_target_x;
      unit.AIMoveTargetY = pu.ai_move_target_y;
      unit.index = index;
      unit.friend = friend;
      unit.setCrippled(disableSkills);
      unit.ougi = result1;
      unit.SEASkill = result2;
      unit.skills = result3.Concat<BL.Skill>((IEnumerable<BL.Skill>) result6).Concat<BL.Skill>((IEnumerable<BL.Skill>) result9).Concat<BL.Skill>((IEnumerable<BL.Skill>) result12).ToArray<BL.Skill>();
      unit.duelSkills = result13.Concat<BL.Skill>((IEnumerable<BL.Skill>) result5).Concat<BL.Skill>((IEnumerable<BL.Skill>) result7).Concat<BL.Skill>((IEnumerable<BL.Skill>) result10).OrderByDescending<BL.Skill, int>((Func<BL.Skill, int>) (x => x.skill.weight)).ThenBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.id)).ThenByDescending<BL.Skill, int>((Func<BL.Skill, int>) (x => x.level)).ToArray<BL.Skill>();
      unit.magicBullets = result4.Concat<BL.MagicBullet>((IEnumerable<BL.MagicBullet>) result8).Concat<BL.MagicBullet>((IEnumerable<BL.MagicBullet>) result11).Concat<BL.MagicBullet>((IEnumerable<BL.MagicBullet>) result14).Concat<BL.MagicBullet>((IEnumerable<BL.MagicBullet>) result15).ToArray<BL.MagicBullet>();
      foreach (BL.MagicBullet magicBullet in unit.magicBullets)
        magicBullet.setPrefabName(unit.job);
      unit.skillfull_shield = XorShift.Range(1, 5);
      unit.skillfull_weapon = XorShift.Range(1, 5);
      unit.isSpawned = pu.spawn_turn <= 0;
      unit.isEnable = unit.isSpawned;
      unit.mIsExecCompletedSkillEffect = false;
      unit.isCallEntryReserve = unit.unit.same_character_id == call_same_character_id;
      promise.Result = unit;
    }

    public Future<BL.Unit> createUnitByPlayerUnit(
      PlayerUnit pu,
      int index,
      bool friend,
      int call_same_character_id,
      Checker checkRules)
    {
      pu.resetOnceOverkillers();
      bool disableSkills = checkRules != null && !checkRules(pu);
      int[] princessSkills = pu.getPrincessSkillIds(disableSkills).ToArray<int>();
      List<PlayerUnitSkills> source1 = !disableSkills ? ((IEnumerable<PlayerUnitSkills>) pu.skills).Concat<PlayerUnitSkills>((IEnumerable<PlayerUnitSkills>) pu.retrofitSkills).ToList<PlayerUnitSkills>() : ((IEnumerable<PlayerUnitSkills>) pu.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x =>
      {
        if (BattleskillSkill.InvestElementSkillIds.Contains(x.skill_id))
          return true;
        return x.skill.skill_type == BattleskillSkillType.magic && !((IEnumerable<int>) princessSkills).Contains<int>(x.skill_id);
      })).ToList<PlayerUnitSkills>();
      PlayerUnitSkills[] array1 = source1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)).ToArray<PlayerUnitSkills>();
      PlayerUnitSkills[] array2 = source1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.release)).ToArray<PlayerUnitSkills>();
      PlayerUnitSkills[] array3 = source1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.SEA)).ToArray<PlayerUnitSkills>();
      PlayerUnitSkills[] array4 = source1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).ToArray<PlayerUnitSkills>();
      PlayerUnitSkills[] array5 = source1.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)).ToArray<PlayerUnitSkills>();
      GearGearSkill[] source2 = !(pu.equippedGear != (PlayerItem) null) || disableSkills ? new GearGearSkill[0] : pu.equippedGear.skills;
      List<GearGearSkill> list1 = ((IEnumerable<GearGearSkill>) source2).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)).ToList<GearGearSkill>();
      List<GearGearSkill> list2 = ((IEnumerable<GearGearSkill>) source2).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)).ToList<GearGearSkill>();
      List<GearGearSkill> list3 = ((IEnumerable<GearGearSkill>) source2).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).ToList<GearGearSkill>();
      if (pu.equippedGear2 != (PlayerItem) null && !disableSkills)
      {
        list1.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear2.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)));
        list2.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear2.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)));
        list3.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear2.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)));
      }
      if (pu.equippedGear3 != (PlayerItem) null && !disableSkills)
      {
        list1.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear3.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)));
        list2.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear3.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)));
        list3.AddRange(((IEnumerable<GearGearSkill>) pu.equippedGear3.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)));
      }
      GearReisouSkill[] source3 = !(pu.equippedGear != (PlayerItem) null) || !(pu.equippedReisou != (PlayerItem) null) || disableSkills ? new GearReisouSkill[0] : pu.equippedReisou.getReisouSkills(pu.equippedGear.entity_id);
      List<GearReisouSkill> list4 = ((IEnumerable<GearReisouSkill>) source3).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)).ToList<GearReisouSkill>();
      List<GearReisouSkill> list5 = ((IEnumerable<GearReisouSkill>) source3).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)).ToList<GearReisouSkill>();
      List<GearReisouSkill> list6 = ((IEnumerable<GearReisouSkill>) source3).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).ToList<GearReisouSkill>();
      if (pu.equippedGear2 != (PlayerItem) null && pu.equippedReisou2 != (PlayerItem) null && !disableSkills)
      {
        list4.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou2.getReisouSkills(pu.equippedGear2.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)));
        list5.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou2.getReisouSkills(pu.equippedGear2.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)));
        list6.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou2.getReisouSkills(pu.equippedGear2.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)));
      }
      if (pu.equippedGear3 != (PlayerItem) null && pu.equippedReisou3 != (PlayerItem) null && !disableSkills)
      {
        list4.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou3.getReisouSkills(pu.equippedGear3.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.command)));
        list5.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou3.getReisouSkills(pu.equippedGear3.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.duel)));
        list6.AddRange(((IEnumerable<GearReisouSkill>) pu.equippedReisou3.getReisouSkills(pu.equippedGear3.entity_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)));
      }
      PlayerAwakeSkill[] source4;
      if (pu.equippedExtraSkill == null || disableSkills)
        source4 = new PlayerAwakeSkill[0];
      else
        source4 = new PlayerAwakeSkill[1]
        {
          pu.equippedExtraSkill
        };
      PlayerAwakeSkill[] array6 = ((IEnumerable<PlayerAwakeSkill>) source4).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => v.masterData.skill_type == BattleskillSkillType.command)).ToArray<PlayerAwakeSkill>();
      PlayerAwakeSkill[] array7 = ((IEnumerable<PlayerAwakeSkill>) source4).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => v.masterData.skill_type == BattleskillSkillType.duel)).ToArray<PlayerAwakeSkill>();
      PlayerAwakeSkill[] array8 = ((IEnumerable<PlayerAwakeSkill>) source4).Where<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (v => v.masterData.skill_type == BattleskillSkillType.magic)).ToArray<PlayerAwakeSkill>();
      Future<BL.Skill> ougiF = array2.Length == 0 ? Future.Single<BL.Skill>((BL.Skill) null) : this.createSkill(((IEnumerable<PlayerUnitSkills>) array2).First<PlayerUnitSkills>());
      Future<BL.Skill> SEASkillF = array3.Length == 0 ? Future.Single<BL.Skill>((BL.Skill) null) : this.createSkill(((IEnumerable<PlayerUnitSkills>) array3).First<PlayerUnitSkills>());
      Future<List<BL.Skill>> skillsF = ((IEnumerable<PlayerUnitSkills>) array1).Select<PlayerUnitSkills, Future<BL.Skill>>((Func<PlayerUnitSkills, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
      Future<List<BL.MagicBullet>> magicBulletsF = ((IEnumerable<PlayerUnitSkills>) array4).Select<PlayerUnitSkills, Future<BL.MagicBullet>>((Func<PlayerUnitSkills, Future<BL.MagicBullet>>) (v => this.createMagicBullet(v))).Sequence<BL.MagicBullet>();
      Future<List<BL.Skill>> duelSkillsF = ((IEnumerable<PlayerUnitSkills>) array5).Select<PlayerUnitSkills, Future<BL.Skill>>((Func<PlayerUnitSkills, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
      Future<List<BL.Skill>> gearCommandSkillsF = list1.Select<GearGearSkill, Future<BL.Skill>>((Func<GearGearSkill, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
      Future<List<BL.Skill>> gearDuelSkillsF = list2.Select<GearGearSkill, Future<BL.Skill>>((Func<GearGearSkill, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
      Future<List<BL.MagicBullet>> gearMagicBulletsF = list3.Select<GearGearSkill, Future<BL.MagicBullet>>((Func<GearGearSkill, Future<BL.MagicBullet>>) (v => this.createMagicBullet(v))).Sequence<BL.MagicBullet>();
      Future<List<BL.Skill>> reisouCommandSkillsF = list4.Select<GearReisouSkill, Future<BL.Skill>>((Func<GearReisouSkill, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
      Future<List<BL.Skill>> reisouDuelSkillsF = list5.Select<GearReisouSkill, Future<BL.Skill>>((Func<GearReisouSkill, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
      Future<List<BL.MagicBullet>> reisouMagicBulletsF = list6.Select<GearReisouSkill, Future<BL.MagicBullet>>((Func<GearReisouSkill, Future<BL.MagicBullet>>) (v => this.createMagicBullet(v))).Sequence<BL.MagicBullet>();
      Future<List<BL.Skill>> awakeCommandSkillsF = ((IEnumerable<PlayerAwakeSkill>) array6).Select<PlayerAwakeSkill, Future<BL.Skill>>((Func<PlayerAwakeSkill, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
      Future<List<BL.Skill>> awakeDuelSkillsF = ((IEnumerable<PlayerAwakeSkill>) array7).Select<PlayerAwakeSkill, Future<BL.Skill>>((Func<PlayerAwakeSkill, Future<BL.Skill>>) (v => this.createSkill(v))).Sequence<BL.Skill>();
      Future<List<BL.MagicBullet>> awakeMagicBulletsF = ((IEnumerable<PlayerAwakeSkill>) array8).Select<PlayerAwakeSkill, Future<BL.MagicBullet>>((Func<PlayerAwakeSkill, Future<BL.MagicBullet>>) (v => this.createMagicBullet(v))).Sequence<BL.MagicBullet>();
      Future<List<BL.MagicBullet>> optionMagicBulletsF = ((IEnumerable<IAttackMethod>) pu.battleOptionAttacks).Where<IAttackMethod>((Func<IAttackMethod, bool>) (v => v.kind.Enum == GearKindEnum.magic && !disableSkills)).Select<IAttackMethod, Future<BL.MagicBullet>>((Func<IAttackMethod, Future<BL.MagicBullet>>) (x => this.createMagicBullet(x))).Sequence<BL.MagicBullet>();
      return new Future<BL.Unit>((Func<Promise<BL.Unit>, IEnumerator>) (promise => this.createFutureUnit(promise, pu, index, friend, ougiF, SEASkillF, skillsF, magicBulletsF, duelSkillsF, gearCommandSkillsF, gearDuelSkillsF, gearMagicBulletsF, reisouCommandSkillsF, reisouDuelSkillsF, reisouMagicBulletsF, awakeCommandSkillsF, awakeDuelSkillsF, awakeMagicBulletsF, optionMagicBulletsF, call_same_character_id, disableSkills)));
    }

    private float CalcIndicatorLevel(IEnumerable<PlayerUnit> playerUnits)
    {
      return !playerUnits.Any<PlayerUnit>() ? 0.0f : playerUnits.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).Select<PlayerUnit, float>((Func<PlayerUnit, float>) (x => (float) x.level * x.unit.rarity.indicator_level_rate)).OrderByDescending<float, float>((Func<float, float>) (x => x)).Max();
    }

    private void initializeStage(BattleInfo info, BL env)
    {
      BattleStage stage = info.stage;
      env.initializeFeild(info.stageId);
      if (env.condition == null)
        env.condition = new BL.Condition();
      env.condition.id = stage.victory_condition.ID;
      Dictionary<int, Tuple<int, Reward>> dropDic = new Dictionary<int, Tuple<int, Reward>>();
      if (info.PanelItems != null)
        dropDic = ((IEnumerable<Tuple<int, Reward>>) info.PanelItems).ToDictionary<Tuple<int, Reward>, int>((Func<Tuple<int, Reward>, int>) (v =>
        {
          BattleStagePanelEvent battleStagePanelEvent = MasterData.BattleStagePanelEvent[v.Item1];
          return battleStagePanelEvent.initial_coordinate_x - 1 << 16 | battleStagePanelEvent.initial_coordinate_y - 1;
        }));
      info.SplitFacilityFromEnemyIds();
      BattleVictoryAreaCondition[] winArea = env.condition.winAreaConditoin;
      BattleVictoryAreaCondition[] loseArea = env.condition.loseAreaConditoin;
      BattleReinforcement[] battleReinforcements = info.EnemyReinforcements;
      stage.ApplyLandforms((Action<int, int, BattleMapLandform>) ((x, y, landform) =>
      {
        int key = x << 16 | y;
        Tuple<int, Reward> tuple = dropDic == null || !dropDic.ContainsKey(key) ? (Tuple<int, Reward>) null : dropDic[key];
        BL.DropData fieldEvent = tuple == null ? (BL.DropData) null : this.createFieldEvent(tuple.Item2);
        env.setFeildPanel(landform.ID, y, x, landform.landform.ID, tuple == null ? 0 : tuple.Item1, fieldEvent, winArea, loseArea, battleReinforcements);
      }));
    }

    private IEnumerator createEnemyUnits(
      BattleInfo info,
      List<BL.Unit> enemies,
      List<BL.Unit> userEnemies,
      BL env)
    {
      BattleLogicInitializer logicInitializer1 = this;
      int same_character_id = 0;
      if (info.enemyCallSkillParam != null)
        same_character_id = info.enemyCallSkillParam.same_character_id;
      int i;
      Future<BL.Unit> future;
      IEnumerator e;
      for (i = 0; i < info.Enemies.Length; ++i)
      {
        BattleStageEnemy enemy = info.Enemies[i];
        BattleLogicInitializer logicInitializer2 = logicInitializer1;
        BattleStageEnemy enemy1 = enemy;
        BattleLogicInitializer logicInitializer3 = logicInitializer1;
        PlayerUnit[] playerUnits1 = info.deck.player_units;
        PlayerUnit[] second;
        if (info.helper == null)
          second = new PlayerUnit[0];
        else
          second = new PlayerUnit[1]
          {
            info.helper.leader_unit
          };
        IEnumerable<PlayerUnit> playerUnits2 = ((IEnumerable<PlayerUnit>) playerUnits1).Concat<PlayerUnit>((IEnumerable<PlayerUnit>) second);
        double indicator_level = (double) logicInitializer3.CalcIndicatorLevel(playerUnits2);
        XorShift random = env.random;
        int endlessLoopCount = info.raidEndlessLoopCount;
        int questSId = info.quest_s_id;
        int num = info.raidBossDamage.Key == enemy.ID ? 1 : 0;
        PlayerUnit pu = PlayerUnit.FromEnemy(enemy1, (float) indicator_level, random, endlessLoopCount, questSId, num != 0);
        int index = i;
        int call_same_character_id = same_character_id;
        future = logicInitializer2.createUnitByPlayerUnit(pu, index, false, call_same_character_id, (Checker) null);
        e = future.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        future.Result.dropMoney = enemy.money;
        enemies.Add(future.Result);
        if (info.quest_type == CommonQuestType.Tower)
          future.Result.playerUnit.tower_hitpoint_rate = !info.enemyHpRate.ContainsKey(future.Result.playerUnit.id) ? 100f : info.enemyHpRate[future.Result.playerUnit.id];
        enemy = (BattleStageEnemy) null;
        future = (Future<BL.Unit>) null;
      }
      foreach (Tuple<int, Reward> tuple in ((IEnumerable<Tuple<int, Reward>>) info.EnemyItems).Where<Tuple<int, Reward>>((Func<Tuple<int, Reward>, bool>) (ei => enemies.Any<BL.Unit>((Func<BL.Unit, bool>) (x => x.playerUnit.id == ei.Item1)))))
      {
        Tuple<int, Reward> t = tuple;
        enemies.Single<BL.Unit>((Func<BL.Unit, bool>) (x => x.playerUnit.id == t.Item1)).drop = new BL.DropData()
        {
          reward = t.Item2
        };
      }
      if (info.UserEnemies != null)
      {
        for (i = 0; i < info.UserEnemies.Length; ++i)
        {
          if (info.user_units.Length > i && info.user_units[i] != (PlayerUnit) null)
          {
            BattleStageUserUnit enemyUser = info.UserEnemies[i];
            PlayerUnit userUnit = info.user_units[enemyUser.deck_position - 1];
            if (!(userUnit == (PlayerUnit) null))
            {
              PlayerItem equippedGear = userUnit.FindEquippedGear(info.user_items);
              PlayerItem equippedGear2 = userUnit.FindEquippedGear2(info.user_items);
              PlayerItem equippedGear3 = userUnit.FindEquippedGear3(info.user_items);
              PlayerItem reisou = (PlayerItem) null;
              PlayerItem reisou2 = (PlayerItem) null;
              PlayerItem reisou3 = (PlayerItem) null;
              userUnit.SetUserEnemyUnit(enemyUser, equippedGear, equippedGear2, equippedGear3, reisou, reisou2, reisou3, enemyUser.deck_position == 1);
              future = logicInitializer1.createUnitByPlayerUnit(userUnit, enemies.Count + i, false, same_character_id, (Checker) null);
              e = future.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              future.Result.dropMoney = enemyUser.money;
              Tuple<int, Reward> tuple = ((IEnumerable<Tuple<int, Reward>>) info.UserEnemyItems).SingleOrDefault<Tuple<int, Reward>>((Func<Tuple<int, Reward>, bool>) (x => x.Item1 == enemyUser.ID));
              if (tuple != null)
                future.Result.drop = new BL.DropData()
                {
                  reward = tuple.Item2
                };
              userEnemies.Add(future.Result);
              future = (Future<BL.Unit>) null;
            }
          }
        }
      }
    }

    private IEnumerator createFacilityUnits(BattleInfo info, List<BL.Unit> facilities, BL env)
    {
      if (info.facility_units != null)
      {
        for (int i = 0; i < info.facility_units.Length; ++i)
        {
          Future<BL.Unit> future = this.createUnitByPlayerUnit(info.facility_units[i], i, false, 0, (Checker) null);
          IEnumerator e = future.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          BL.Unit result = future.Result;
          BL.Facility facility1 = new BL.Facility();
          facility1.thisForce = result.playerUnit.is_enemy ? BL.ForceID.enemy : BL.ForceID.player;
          MapFacility facility2 = result.unit.facility;
          facility1.isTarget = facility2.is_target;
          facility1.isPutOn = facility2.is_puton;
          facility1.isView = facility2.is_view;
          result.facility = facility1;
          result.AIMoveGroupOrder = 100;
          facilities.Add(result);
          if (info.quest_type == CommonQuestType.Tower)
            result.playerUnit.tower_hitpoint_rate = !info.enemyHpRate.ContainsKey(result.playerUnit.id) ? 100f : info.enemyHpRate[result.playerUnit.id];
          future = (Future<BL.Unit>) null;
        }
      }
    }

    private IEnumerator initializeSkillFacilities(
      BattleInfo info,
      List<BL.Unit> units,
      bool isEnemy,
      List<BL.Unit> facilities,
      int unitIndex,
      int playerUnitId)
    {
      foreach (BL.Unit pu1 in units)
      {
        IEnumerable<BL.Skill> first = (IEnumerable<BL.Skill>) pu1.skills;
        if (pu1.hasOugi)
          first = first.Concat<BL.Skill>((IEnumerable<BL.Skill>) new BL.Skill[1]
          {
            pu1.ougi
          });
        foreach (BL.Skill skill1 in first)
        {
          BL.Skill skill = skill1;
          foreach (BattleskillEffect effect in ((IEnumerable<BattleskillEffect>) skill.skill.Effects).Where<BattleskillEffect>((Func<BattleskillEffect, bool>) (x => x.EffectLogic.Enum == BattleskillEffectLogicEnum.facility_creation && x.checkLevel(skill.level))))
          {
            int maxCount = effect.GetInt(BattleskillEffectLogicArgumentEnum.creation_max);
            for (int i = 0; i < maxCount; ++i)
            {
              PlayerUnit pu2 = PlayerUnit.FromFacility(MasterData.UnitUnit[effect.GetInt(BattleskillEffectLogicArgumentEnum.facility_unit_id)], playerUnitId);
              pu2.is_enemy = isEnemy;
              pu2.spawn_turn = int.MaxValue;
              Future<BL.Unit> future = this.createUnitByPlayerUnit(pu2, unitIndex, false, 0, (Checker) null);
              IEnumerator e = future.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              BL.Unit result = future.Result;
              result.isPlayerControl = false;
              BL.Facility facility1 = new BL.Facility();
              MapFacility facility2 = result.unit.facility;
              facility1.thisForce = result.playerUnit.is_enemy ? BL.ForceID.enemy : BL.ForceID.player;
              facility1.isTarget = facility2.is_target;
              facility1.isPutOn = facility2.is_puton;
              facility1.isView = facility2.is_view;
              facility1.skillUnitIndex = new int?(pu1.index);
              result.facility = facility1;
              result.AIMoveGroupOrder = 100;
              facilities.Add(result);
              if (info.quest_type == CommonQuestType.Tower)
                result.playerUnit.tower_hitpoint_rate = 100f;
              ++unitIndex;
              --playerUnitId;
              future = (Future<BL.Unit>) null;
            }
          }
        }
      }
    }

    public IEnumerator initializeWave(int wave, BattleInfo info, BL env)
    {
      if (info.isWave)
      {
        info.currentWave = env.currentWave = wave;
        IEnumerator e = Singleton<NGBattleManager>.GetInstance().initMasterData(info);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        List<BL.Unit> players = new List<BL.Unit>();
        List<BL.Unit> guests = new List<BL.Unit>();
        List<BL.Unit> friend = new List<BL.Unit>();
        List<BL.Unit> enemies = new List<BL.Unit>();
        List<BL.Unit> userEnemies = new List<BL.Unit>();
        foreach (BL.Unit unit in env.playerUnits.value)
        {
          foreach (BL.Unit invocationUnit in env.enemyUnits.value)
            unit.skillEffects.RemoveEffect(invocationUnit, env, (BL.ISkillEffectListUnit) unit);
          BattleFuncs.removeJumpEffects((BL.ISkillEffectListUnit) unit);
          if (unit.playerUnit.is_guest)
            guests.Add(unit);
          else if (unit.is_helper)
            friend.Add(unit);
          else
            players.Add(unit);
          unit.isEnable = false;
        }
        this.initializeStage(info, env);
        e = this.createEnemyUnits(info, enemies, userEnemies, env);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (env.enemyUnits == null)
          env.enemyUnits = new BL.ClassValue<List<BL.Unit>>(enemies.Concat<BL.Unit>((IEnumerable<BL.Unit>) userEnemies).ToList<BL.Unit>());
        else
          env.enemyUnits.value = enemies.Concat<BL.Unit>((IEnumerable<BL.Unit>) userEnemies).ToList<BL.Unit>();
        List<BL.Unit> facilities = new List<BL.Unit>();
        e = this.createFacilityUnits(info, facilities, env);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        int mapFacilityCount = facilities.Count;
        int unitIndex = mapFacilityCount;
        int playerUnitId = -10001;
        e = this.initializeSkillFacilities(info, env.playerUnits.value, false, facilities, unitIndex, playerUnitId);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        int num = facilities.Count - mapFacilityCount;
        unitIndex += num;
        playerUnitId -= num;
        e = this.initializeSkillFacilities(info, env.enemyUnits.value, true, facilities, unitIndex, playerUnitId);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (env.facilityUnits == null)
          env.facilityUnits = new BL.ClassValue<List<BL.Unit>>(facilities);
        else
          env.facilityUnits.value = facilities;
        this.setLeaderSkills(env.playerUnits.value, env.enemyUnits.value, targetOnly: true);
        this.setLeaderSkills(env.enemyUnits.value, env.playerUnits.value);
        this.setAwakeSkills(env.playerUnits.value, env.enemyUnits.value, targetOnly: true);
        this.setAwakeSkills(env.enemyUnits.value, env.playerUnits.value);
        this.setPassiveSkills(env.playerUnits.value, env.enemyUnits.value, targetOnly: true);
        this.setPassiveSkills(env.enemyUnits.value, env.playerUnits.value);
        this.setGearSkills(env.playerUnits.value, env.enemyUnits.value, targetOnly: true);
        this.setGearSkills(env.enemyUnits.value, env.playerUnits.value);
        this.setFacilitySkills(env.facilityUnits.value, env);
        foreach (BL.Unit unit in env.enemyUnits.value)
        {
          unit.hp = unit.parameter.Hp;
          foreach (BL.MagicBullet magicBullet in unit.magicBullets)
            magicBullet.setAdditionalCost(unit.hp);
          unit.checkActionRangeBySetHp = true;
        }
        foreach (BL.Unit unit in env.facilityUnits.value)
          unit.hp = unit.parameter.Hp;
        foreach (BL.ISkillEffectListUnit beUnit in env.enemyUnits.value)
          BattleFuncs.createBattleSkillEffectParams(beUnit, (IEnumerable<BL.Unit>) env.enemyUnits.value, (IEnumerable<BL.Unit>) env.playerUnits.value);
        e = this.createUnitPositions(info, env, players, guests, friend.Where<BL.Unit>((Func<BL.Unit, bool>) (pu => pu != (BL.Unit) null)).ToList<BL.Unit>(), enemies, userEnemies, facilities, mapFacilityCount);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.createFieldEffects(info.stage, env);
        env.setCurrentUnitWith((BL.Unit) null, (Action<BL.UnitPosition>) (_ => { }));
        env.setCurrentField(0, 0);
        env.phaseState.turnReset();
      }
    }

    private void setupStoryScript(BattleInfo battleInfo, BL env)
    {
      Tuple<StoryPlaybackTiming, int, object[]>[] tupleArray;
      if (battleInfo.isStoryEnable)
      {
        switch (battleInfo.quest_type)
        {
          case CommonQuestType.Story:
            tupleArray = ((IEnumerable<StoryPlaybackStoryDetail>) MasterData.QuestStoryS[battleInfo.quest_s_id].StoryDetails()).Select<StoryPlaybackStoryDetail, Tuple<StoryPlaybackTiming, int, object[]>>((Func<StoryPlaybackStoryDetail, Tuple<StoryPlaybackTiming, int, object[]>>) (x => x.toTuple())).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          case CommonQuestType.Character:
            tupleArray = ((IEnumerable<StoryPlaybackCharacterDetail>) MasterData.QuestCharacterS[battleInfo.quest_s_id].CharacterDetails()).Select<StoryPlaybackCharacterDetail, Tuple<StoryPlaybackTiming, int, object[]>>((Func<StoryPlaybackCharacterDetail, Tuple<StoryPlaybackTiming, int, object[]>>) (x => x.toTuple())).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          case CommonQuestType.Extra:
            tupleArray = ((IEnumerable<StoryPlaybackExtraDetail>) MasterData.QuestExtraS[battleInfo.quest_s_id].ExtraDetails()).Select<StoryPlaybackExtraDetail, Tuple<StoryPlaybackTiming, int, object[]>>((Func<StoryPlaybackExtraDetail, Tuple<StoryPlaybackTiming, int, object[]>>) (x => x.toTuple())).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          case CommonQuestType.Harmony:
            tupleArray = ((IEnumerable<StoryPlaybackHarmonyDetail>) MasterData.QuestHarmonyS[battleInfo.quest_s_id].HarmonyDetail()).Select<StoryPlaybackHarmonyDetail, Tuple<StoryPlaybackTiming, int, object[]>>((Func<StoryPlaybackHarmonyDetail, Tuple<StoryPlaybackTiming, int, object[]>>) (x => x.toTuple())).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          case CommonQuestType.Earth:
            tupleArray = ((IEnumerable<EarthQuestStoryPlayback>) MasterData.EarthQuestStoryPlaybackList).Where<EarthQuestStoryPlayback>((Func<EarthQuestStoryPlayback, bool>) (x => x.episode.ID == battleInfo.quest_s_id)).Select<EarthQuestStoryPlayback, Tuple<StoryPlaybackTiming, int, object[]>>((Func<EarthQuestStoryPlayback, Tuple<StoryPlaybackTiming, int, object[]>>) (x =>
            {
              int timing = (int) x.timing;
              int scriptId = x.script_id;
              object[] objArray;
              if (x.stage_enemy_BattleStageEnemy.HasValue)
                objArray = new object[2]
                {
                  (object) x.attack_timing_type,
                  (object) x.stage_enemy_BattleStageEnemy
                };
              else
                objArray = new object[0];
              return Tuple.Create<StoryPlaybackTiming, int, object[]>((StoryPlaybackTiming) timing, scriptId, objArray);
            })).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          case CommonQuestType.Tower:
            tupleArray = ((IEnumerable<TowerPlaybackStoryDetail>) MasterData.TowerPlaybackStoryDetailList).Where<TowerPlaybackStoryDetail>((Func<TowerPlaybackStoryDetail, bool>) (x => x.stage.stage_id == battleInfo.stageId)).Select<TowerPlaybackStoryDetail, Tuple<StoryPlaybackTiming, int, object[]>>((Func<TowerPlaybackStoryDetail, Tuple<StoryPlaybackTiming, int, object[]>>) (x => x.toTuple())).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          case CommonQuestType.Sea:
            tupleArray = ((IEnumerable<StoryPlaybackSeaDetail>) MasterData.QuestSeaS[battleInfo.quest_s_id].StoryDetails()).Select<StoryPlaybackSeaDetail, Tuple<StoryPlaybackTiming, int, object[]>>((Func<StoryPlaybackSeaDetail, Tuple<StoryPlaybackTiming, int, object[]>>) (x => x.toTuple())).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          case CommonQuestType.GuildRaid:
            tupleArray = ((IEnumerable<StoryPlaybackRaidDetail>) MasterData.StoryPlaybackRaidDetailList).Where<StoryPlaybackRaidDetail>((Func<StoryPlaybackRaidDetail, bool>) (x => x.stage_id == battleInfo.stageId && !Persist.raidStoryAlreadyRead.Data.isAlreadyRead(x.stage_id, x.script_id))).Select<StoryPlaybackRaidDetail, Tuple<StoryPlaybackTiming, int, object[]>>((Func<StoryPlaybackRaidDetail, Tuple<StoryPlaybackTiming, int, object[]>>) (x => x.toTuple())).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          case CommonQuestType.Corps:
            tupleArray = ((IEnumerable<CorpsPlaybackStoryDetail>) MasterData.CorpsPlaybackStoryDetailList).Where<CorpsPlaybackStoryDetail>((Func<CorpsPlaybackStoryDetail, bool>) (x => x.stage_id == battleInfo.stageId)).Select<CorpsPlaybackStoryDetail, Tuple<StoryPlaybackTiming, int, object[]>>((Func<CorpsPlaybackStoryDetail, Tuple<StoryPlaybackTiming, int, object[]>>) (x => x.toTuple())).ToArray<Tuple<StoryPlaybackTiming, int, object[]>>();
            break;
          default:
            Debug.LogError((object) "ここに来たらバグ");
            tupleArray = new Tuple<StoryPlaybackTiming, int, object[]>[0];
            break;
        }
      }
      else
        tupleArray = new Tuple<StoryPlaybackTiming, int, object[]>[0];
      Dictionary<StoryPlaybackTiming, BL.StoryType> dictionary = new Dictionary<StoryPlaybackTiming, BL.StoryType>()
      {
        {
          StoryPlaybackTiming.before_battle,
          BL.StoryType.battle_start
        },
        {
          StoryPlaybackTiming.located_player_unit,
          BL.StoryType.first_turn_start
        },
        {
          StoryPlaybackTiming.after_battle,
          BL.StoryType.battle_win
        },
        {
          StoryPlaybackTiming.duel_start,
          BL.StoryType.duel_start
        },
        {
          StoryPlaybackTiming.turn_start,
          BL.StoryType.turn_start
        },
        {
          StoryPlaybackTiming.in_area,
          BL.StoryType.unit_in_area
        },
        {
          StoryPlaybackTiming.defeat_player,
          BL.StoryType.defeat_player
        },
        {
          StoryPlaybackTiming.wave_clear,
          BL.StoryType.wave_clear
        }
      };
      List<BL.Story> v = new List<BL.Story>();
      foreach (Tuple<StoryPlaybackTiming, int, object[]> tuple in tupleArray)
        v.Add(new BL.Story(tuple.Item2, dictionary[tuple.Item1], tuple.Item3));
      env.storyList = new BL.ClassValue<List<BL.Story>>(v);
    }

    private void createFieldEffects(BattleStage stage, BL env)
    {
      List<BL.FieldEffect> v = new List<BL.FieldEffect>();
      Dictionary<BattleFieldEffectTiming, BL.FieldEffectType> dictionary = new Dictionary<BattleFieldEffectTiming, BL.FieldEffectType>()
      {
        {
          BattleFieldEffectTiming.before_battle,
          BL.FieldEffectType.battle_start
        },
        {
          BattleFieldEffectTiming.first_turn_start,
          BL.FieldEffectType.first_turn_start
        },
        {
          BattleFieldEffectTiming.turn_start,
          BL.FieldEffectType.turn_start
        },
        {
          BattleFieldEffectTiming.player_start,
          BL.FieldEffectType.player_start
        },
        {
          BattleFieldEffectTiming.neutral_start,
          BL.FieldEffectType.neutral_start
        },
        {
          BattleFieldEffectTiming.enemy_start,
          BL.FieldEffectType.enemy_start
        },
        {
          BattleFieldEffectTiming.stageclear,
          BL.FieldEffectType.stageclear
        },
        {
          BattleFieldEffectTiming.pvp_change_player,
          BL.FieldEffectType.pvp_change_player
        },
        {
          BattleFieldEffectTiming.pvp_change_enemy,
          BL.FieldEffectType.pvp_change_enemy
        }
      };
      foreach (BattleFieldEffectStage fieldEffectStage in stage.FieldEffectStages)
        v.Add(new BL.FieldEffect(fieldEffectStage.field_effect.ID, dictionary[fieldEffectStage.timing]));
      if (env.fieldEffectList == null)
        env.fieldEffectList = new BL.ClassValue<List<BL.FieldEffect>>(v);
      else
        env.fieldEffectList.value = v;
    }

    private BL.UnitPosition checkUnitPosition(BL.Unit u, BL env)
    {
      BL.UnitPosition unitPosition = (BL.UnitPosition) null;
      if (env.unitPositions != null && env.unitPositions.value != null)
        unitPosition = env.getUnitPosition(u);
      return unitPosition ?? new BL.UnitPosition();
    }

    private Tuple<List<BL.UnitPosition>, int> getGuestPosition(
      BattleInfo info,
      BL env,
      List<BL.Unit> guests,
      int positionId)
    {
      List<BL.UnitPosition> unitPositionList = new List<BL.UnitPosition>();
      if (info.quest_type == CommonQuestType.Earth || info.quest_type == CommonQuestType.EarthExtra)
        unitPositionList.AddRange(guests.Select<BL.Unit, BattleEarthStageGuest, BL.UnitPosition>((IEnumerable<BattleEarthStageGuest>) info.EarthGuests, (Func<BL.Unit, BattleEarthStageGuest, BL.UnitPosition>) ((a, b) =>
        {
          BL.UnitPosition guestPosition = this.checkUnitPosition(a, env);
          guestPosition.id = positionId++;
          guestPosition.unit = a;
          guestPosition.row = b.initial_coordinate_y - 1;
          guestPosition.column = b.initial_coordinate_x - 1;
          guestPosition.direction = b.initial_direction;
          guestPosition.resetOriginalPosition(env);
          return guestPosition;
        })).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (a => a != null)));
      else
        unitPositionList.AddRange(guests.Select<BL.Unit, BattleStageGuest, BL.UnitPosition>((IEnumerable<BattleStageGuest>) info.Guests, (Func<BL.Unit, BattleStageGuest, BL.UnitPosition>) ((a, b) =>
        {
          BL.UnitPosition guestPosition = this.checkUnitPosition(a, env);
          guestPosition.id = positionId++;
          guestPosition.unit = a;
          guestPosition.row = b.initial_coordinate_y(info.stageId) - 1;
          guestPosition.column = b.initial_coordinate_x(info.stageId) - 1;
          guestPosition.direction = b.initial_direction(info.stageId);
          guestPosition.resetOriginalPosition(env);
          return guestPosition;
        })).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (a => a != null)));
      return new Tuple<List<BL.UnitPosition>, int>(unitPositionList, positionId);
    }

    private IEnumerator createUnitPositions(
      BattleInfo info,
      BL env,
      List<BL.Unit> players,
      List<BL.Unit> guests,
      List<BL.Unit> friend,
      List<BL.Unit> enemies,
      List<BL.Unit> userEnemies,
      List<BL.Unit> facilities,
      int mapFacilityCount)
    {
      int unitPositionId = 0;
      BattleStage stage = info.stage;
      KeyValuePair<int, BattleStagePlayer>[] stagePlayers = MasterData.BattleStagePlayer.Where<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == info.stageId)).ToArray<KeyValuePair<int, BattleStagePlayer>>();
      List<BL.UnitPosition> list1 = players.Select<BL.Unit, BattleStagePlayer, BL.UnitPosition>((IEnumerable<BattleStagePlayer>) stage.Players, (Func<BL.Unit, BattleStagePlayer, BL.UnitPosition>) ((a, b) =>
      {
        if (a == (BL.Unit) null)
          return (BL.UnitPosition) null;
        BL.UnitPosition unitPositions = this.checkUnitPosition(a, env);
        unitPositions.id = unitPositionId++;
        unitPositions.unit = a;
        unitPositions.row = stagePlayers[a.index].Value.initial_coordinate_y - 1;
        unitPositions.column = stagePlayers[a.index].Value.initial_coordinate_x - 1;
        unitPositions.direction = stagePlayers[a.index].Value.initial_direction;
        unitPositions.resetOriginalPosition(env);
        return unitPositions;
      })).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (a => a != null)).ToList<BL.UnitPosition>();
      Tuple<List<BL.UnitPosition>, int> guestPosition = this.getGuestPosition(info, env, guests, unitPositionId);
      List<BL.UnitPosition> unitPositionList = guestPosition.Item1;
      unitPositionId = guestPosition.Item2;
      List<BL.UnitPosition> second1 = new List<BL.UnitPosition>();
      if (friend != null && friend.Count > 0)
      {
        BattleStagePlayer battleStagePlayer = MasterData.BattleStagePlayer.FirstOrDefault<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage.ID == info.stageId && x.Value.deck_position == Consts.GetInstance().DECK_POSITION_FRIEND)).Value;
        if (battleStagePlayer != null)
        {
          BL.UnitPosition unitPosition = this.checkUnitPosition(friend[0], env);
          unitPosition.id = unitPositionId++;
          unitPosition.unit = friend[0];
          unitPosition.row = battleStagePlayer.initial_coordinate_y - 1;
          unitPosition.column = battleStagePlayer.initial_coordinate_x - 1;
          unitPosition.direction = battleStagePlayer.initial_direction;
          unitPosition.resetOriginalPosition(env);
          second1.Add(unitPosition);
        }
      }
      List<BL.UnitPosition> list2 = enemies.Select<BL.Unit, BattleStageEnemy, BL.UnitPosition>((IEnumerable<BattleStageEnemy>) info.Enemies, (Func<BL.Unit, BattleStageEnemy, BL.UnitPosition>) ((a, b) =>
      {
        BL.UnitPosition unitPositions = this.checkUnitPosition(a, env);
        unitPositions.id = unitPositionId++;
        unitPositions.unit = a;
        unitPositions.row = b.initial_coordinate_y - 1;
        unitPositions.column = b.initial_coordinate_x - 1;
        unitPositions.direction = b.initial_direction;
        unitPositions.resetOriginalPosition(env);
        return unitPositions;
      })).ToList<BL.UnitPosition>();
      List<BL.UnitPosition> list3 = userEnemies.Select<BL.Unit, BattleStageUserUnit, BL.UnitPosition>((IEnumerable<BattleStageUserUnit>) info.UserEnemies, (Func<BL.Unit, BattleStageUserUnit, BL.UnitPosition>) ((a, b) =>
      {
        BL.UnitPosition unitPositions = this.checkUnitPosition(a, env);
        unitPositions.id = unitPositionId++;
        unitPositions.unit = a;
        unitPositions.row = b.initial_coordinate_y - 1;
        unitPositions.column = b.initial_coordinate_x - 1;
        unitPositions.direction = b.initial_direction;
        unitPositions.resetOriginalPosition(env);
        return unitPositions;
      })).ToList<BL.UnitPosition>();
      List<BL.UnitPosition> list4 = facilities.Take<BL.Unit>(mapFacilityCount).Select<BL.Unit, Tuple<int, int>, BL.UnitPosition>((IEnumerable<Tuple<int, int>>) info.facility_coordinates, (Func<BL.Unit, Tuple<int, int>, BL.UnitPosition>) ((a, b) =>
      {
        BL.UnitPosition unitPositions = this.checkUnitPosition(a, env);
        unitPositions.id = unitPositionId++;
        unitPositions.unit = a;
        unitPositions.row = b.Item2 - 1;
        unitPositions.column = b.Item1 - 1;
        unitPositions.resetOriginalPosition(env);
        return unitPositions;
      })).ToList<BL.UnitPosition>();
      List<BL.UnitPosition> list5 = facilities.Skip<BL.Unit>(mapFacilityCount).Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (a =>
      {
        BL.UnitPosition unitPositions = new BL.UnitPosition();
        unitPositions.id = unitPositionId++;
        unitPositions.unit = a;
        unitPositions.row = 0;
        unitPositions.column = 0;
        unitPositions.resetOriginalPosition(env);
        return unitPositions;
      })).ToList<BL.UnitPosition>();
      List<BL.UnitPosition> second2 = unitPositionList;
      List<BL.UnitPosition> list6 = list1.Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) second2).Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) second1).Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) list2).Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) list3).Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) list4).Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) list5).ToList<BL.UnitPosition>();
      if (env.unitPositions == null)
        env.unitPositions = new BL.ClassValue<List<BL.UnitPosition>>(list6);
      else
        env.unitPositions.value = list6;
      NGBattleAIManager manager = Singleton<NGBattleManager>.GetInstance().getManager<NGBattleAIManager>();
      if (Object.op_Inequality((Object) manager, (Object) null))
      {
        if (manager.ai is LispAILogic ai)
        {
          foreach (BL.UnitPosition unitPosition in env.unitPositions.value)
          {
            IEnumerator e = unitPosition.unit.InitAIExtention(ai);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        ai = (LispAILogic) null;
      }
    }

    private void setRangeSkills(
      List<BL.Unit> units,
      List<BL.Unit> targets,
      bool ownOnly,
      bool targetOnly,
      BL.Unit unit,
      BattleskillSkill skill,
      int level,
      int gearIndex = 0)
    {
      if (!targetOnly && skill.target_type == BattleskillTargetType.myself)
      {
        foreach (BattleskillEffect effect in skill.Effects)
          unit.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex), checkEnableUnit: (BL.ISkillEffectListUnit) unit);
      }
      else if (!targetOnly && (skill.target_type == BattleskillTargetType.player_single || skill.target_type == BattleskillTargetType.player_range))
      {
        foreach (BL.Unit unit1 in units)
        {
          foreach (BattleskillEffect effect in skill.Effects)
            unit1.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex), checkEnableUnit: (BL.ISkillEffectListUnit) unit1);
        }
      }
      else if (!ownOnly && (skill.target_type == BattleskillTargetType.enemy_single || skill.target_type == BattleskillTargetType.enemy_range))
      {
        foreach (BL.Unit target in targets)
        {
          foreach (BattleskillEffect effect in skill.Effects)
            target.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex), checkEnableUnit: (BL.ISkillEffectListUnit) target);
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
            if (!ownOnly)
            {
              foreach (BL.Unit target in targets)
                target.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex), checkEnableUnit: (BL.ISkillEffectListUnit) target);
            }
          }
          else if (!targetOnly)
          {
            foreach (BL.Unit unit2 in units)
              unit2.skillEffects.Add(BL.SkillEffect.FromMasterData(unit, effect, skill, level, true, gearIndex), checkEnableUnit: (BL.ISkillEffectListUnit) unit2);
          }
        }
      }
    }

    private void setLeaderSkills(
      List<BL.Unit> units,
      List<BL.Unit> targets,
      bool ownOnly = false,
      bool targetOnly = false)
    {
      foreach (BL.Unit unit in units)
      {
        if (!unit.crippled && (unit.is_leader || unit.friend || unit.playerUnit.is_enemy_leader || unit.playerUnit.is_guest && unit.is_helper))
        {
          foreach (PlayerUnitLeader_skills leaderSkill in unit.playerUnit.leader_skills)
            this.setRangeSkills(units, targets, ownOnly, targetOnly, unit, leaderSkill.skill, leaderSkill.level);
        }
      }
    }

    private void setPassiveSkillsCore(
      BL.Unit unit,
      List<BL.Unit> units,
      List<BL.Unit> targets,
      bool ownOnly,
      bool targetOnly)
    {
      foreach (PlayerUnitSkills passiveSkill in unit.playerUnit.passiveSkills)
      {
        if (!passiveSkill.skill.range_effect_passive_skill)
        {
          if (!targetOnly)
          {
            foreach (BattleskillEffect effect in passiveSkill.skill.Effects)
              unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, passiveSkill.skill, passiveSkill.level, true), checkEnableUnit: (BL.ISkillEffectListUnit) unit);
          }
        }
        else
          this.setRangeSkills(units, targets, ownOnly, targetOnly, unit, passiveSkill.skill, passiveSkill.level);
      }
    }

    private void setPassiveSkills(
      List<BL.Unit> units,
      List<BL.Unit> targets,
      bool ownOnly = false,
      bool targetOnly = false)
    {
      foreach (BL.Unit unit in units)
      {
        if (!unit.crippled)
          this.setPassiveSkillsCore(unit, units, targets, ownOnly, targetOnly);
      }
    }

    private void setGearSkills(
      List<BL.Unit> units,
      List<BL.Unit> targets,
      bool ownOnly = false,
      bool targetOnly = false)
    {
      Action<BL.Unit, PlayerItem, int> action1 = (Action<BL.Unit, PlayerItem, int>) ((unit, gear, gearIndex) =>
      {
        ((IEnumerable<GearGearSkill>) gear.skills).ToList<GearGearSkill>();
        foreach (GearGearSkill gearGearSkill in ((IEnumerable<GearGearSkill>) gear.skills).Where<GearGearSkill>((Func<GearGearSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.passive)).ToArray<GearGearSkill>())
        {
          if (!gearGearSkill.skill.range_effect_passive_skill)
          {
            if (!targetOnly)
            {
              foreach (BattleskillEffect effect in gearGearSkill.skill.Effects)
                unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, gearGearSkill.skill, gearGearSkill.skill_level, true, gearIndex), checkEnableUnit: (BL.ISkillEffectListUnit) unit);
            }
          }
          else
            this.setRangeSkills(units, targets, ownOnly, targetOnly, unit, gearGearSkill.skill, gearGearSkill.skill_level, gearIndex);
        }
      });
      Action<BL.Unit, int, PlayerItem, int> action2 = (Action<BL.Unit, int, PlayerItem, int>) ((unit, gear_id, reisou, gearIndex) =>
      {
        ((IEnumerable<GearReisouSkill>) reisou.getReisouSkills(gear_id)).ToList<GearReisouSkill>();
        foreach (GearReisouSkill gearReisouSkill in ((IEnumerable<GearReisouSkill>) reisou.getReisouSkills(gear_id)).Where<GearReisouSkill>((Func<GearReisouSkill, bool>) (v => v.skill.skill_type == BattleskillSkillType.passive)).ToArray<GearReisouSkill>())
        {
          if (!gearReisouSkill.skill.range_effect_passive_skill)
          {
            if (!targetOnly)
            {
              foreach (BattleskillEffect effect in gearReisouSkill.skill.Effects)
                unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, gearReisouSkill.skill, gearReisouSkill.skill_level, true, gearIndex), checkEnableUnit: (BL.ISkillEffectListUnit) unit);
            }
          }
          else
            this.setRangeSkills(units, targets, ownOnly, targetOnly, unit, gearReisouSkill.skill, gearReisouSkill.skill_level, gearIndex);
        }
      });
      foreach (BL.Unit unit in units)
      {
        if (!unit.crippled)
        {
          PlayerItem equippedGear = unit.playerUnit.equippedGear;
          if (equippedGear != (PlayerItem) null)
            action1(unit, equippedGear, 1);
          PlayerItem equippedGear2 = unit.playerUnit.equippedGear2;
          if (equippedGear2 != (PlayerItem) null)
            action1(unit, equippedGear2, 2);
          PlayerItem equippedGear3 = unit.playerUnit.equippedGear3;
          if (equippedGear3 != (PlayerItem) null)
            action1(unit, equippedGear3, 3);
          PlayerItem equippedReisou = unit.playerUnit.equippedReisou;
          if (equippedGear != (PlayerItem) null && equippedReisou != (PlayerItem) null)
            action2(unit, equippedGear.gear.ID, equippedReisou, 1);
          PlayerItem equippedReisou2 = unit.playerUnit.equippedReisou2;
          if (equippedGear2 != (PlayerItem) null && equippedReisou2 != (PlayerItem) null)
            action2(unit, equippedGear2.gear.ID, equippedReisou2, 2);
          PlayerItem equippedReisou3 = unit.playerUnit.equippedReisou3;
          if (equippedGear3 != (PlayerItem) null && equippedReisou3 != (PlayerItem) null)
            action2(unit, equippedGear3.gear.ID, equippedReisou3, 3);
        }
      }
    }

    private void setAwakeSkills(
      List<BL.Unit> units,
      List<BL.Unit> targets,
      bool ownOnly = false,
      bool targetOnly = false)
    {
      foreach (BL.Unit unit in units)
      {
        if (!unit.crippled)
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
              if (!targetOnly)
              {
                foreach (BattleskillEffect effect in playerAwakeSkill.masterData.Effects)
                  unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, playerAwakeSkill.masterData, playerAwakeSkill.level, true), checkEnableUnit: (BL.ISkillEffectListUnit) unit);
              }
            }
            else
              this.setRangeSkills(units, targets, ownOnly, targetOnly, unit, playerAwakeSkill.masterData, playerAwakeSkill.level);
          }
        }
      }
    }

    private void setFacilitySkills(
      List<BL.Unit> facilities,
      BL env,
      bool ownOnly = false,
      bool targetOnly = false)
    {
      foreach (BL.Unit facility in facilities)
      {
        switch (env.getForceID(facility))
        {
          case BL.ForceID.player:
            this.setPassiveSkillsCore(facility, env.playerUnits.value, env.enemyUnits.value, ownOnly, targetOnly);
            continue;
          case BL.ForceID.enemy:
            this.setPassiveSkillsCore(facility, env.enemyUnits.value, env.playerUnits.value, ownOnly, targetOnly);
            continue;
          default:
            continue;
        }
      }
    }

    private void setBonusSkills(List<BL.Unit> units, BattleInfo info)
    {
      foreach (BL.Unit unit in units)
      {
        foreach (Bonus pvpBonus in info.pvp_bonus_list)
        {
          if (Bonus.IsEnableBonus(unit, pvpBonus, info.pvp_start_date))
          {
            foreach (BattleskillEffect effect in pvpBonus.skill.Effects)
              unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, pvpBonus.skill, 1, true), checkEnableUnit: (BL.ISkillEffectListUnit) unit);
          }
        }
      }
    }

    private void setGvgBonusSkills(List<BL.Unit> units, GuildBaseBonusEffect[] bonus_list)
    {
      foreach (BL.Unit unit in units)
      {
        foreach (GuildBaseBonusEffect bonus in bonus_list)
        {
          foreach (BattleskillEffect effect in bonus.skill.Effects)
            unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, bonus.skill, 1, true), checkEnableUnit: (BL.ISkillEffectListUnit) unit);
        }
      }
    }

    private void setUnitInitialHp(
      BattleInfo battleInfo,
      BL.ClassValue<List<BL.Unit>> units,
      bool isFacility = false)
    {
      foreach (BL.Unit unit in units.value)
      {
        int hp = unit.parameter.Hp;
        switch (battleInfo.quest_type)
        {
          case CommonQuestType.Tower:
            unit.hp = TowerUtil.GetHp(hp, unit.playerUnit.TowerHpRate);
            break;
          case CommonQuestType.Corps:
            unit.hp = unit.isPlayerControl || !battleInfo.enemyRestHp.ContainsKey(unit.playerUnit.id) ? hp : battleInfo.enemyRestHp[unit.playerUnit.id];
            break;
          default:
            unit.hp = hp;
            break;
        }
        unit.initialHp = unit.hp;
        if (!isFacility)
        {
          foreach (BL.MagicBullet magicBullet in unit.magicBullets)
            magicBullet.setAdditionalCost(hp);
          unit.checkActionRangeBySetHp = true;
        }
      }
    }

    private void GetInitialDeadEnemyDrop(BL env)
    {
      foreach (BL.Unit unit in env.enemyUnits.value)
      {
        if (unit.initialHp < 1 && unit.hasDrop)
          unit.drop.execute((BL.Unit) null, env);
      }
    }

    private IEnumerator initializeData(BattleInfo battleInfo, BL env)
    {
      env.nextRandom();
      if (battleInfo.isWave)
        battleInfo.currentWave = env.currentWave;
      IEnumerator e = Singleton<NGBattleManager>.GetInstance().initMasterData(battleInfo);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.initializeStage(battleInfo, env);
      List<BL.Unit> players = new List<BL.Unit>();
      List<BL.Unit> guests = new List<BL.Unit>();
      List<BL.Unit> friend = new List<BL.Unit>();
      List<BL.Unit> enemies = new List<BL.Unit>();
      List<BL.Unit> userEnemies = new List<BL.Unit>();
      Checker checkUnitRules = battleInfo.checkUnitRules;
      int playerCnt;
      int i;
      Future<BL.Unit> future;
      if (!battleInfo.pvp && !battleInfo.gvg)
      {
        bool helperOverrWriteGuest = false;
        DeckInfo deck = battleInfo.deck;
        playerCnt = deck.player_units.Length;
        List<int> activeDeckPosition = new List<int>();
        for (int index = 0; index < playerCnt; ++index)
          activeDeckPosition.Add(1 + index);
        if (battleInfo.quest_type != CommonQuestType.Earth && battleInfo.quest_type != CommonQuestType.EarthExtra)
        {
          switch (battleInfo.quest_type)
          {
            case CommonQuestType.Tower:
            case CommonQuestType.Corps:
              activeDeckPosition = MasterData.BattleStagePlayer.Where<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == battleInfo.stageId)).Select<KeyValuePair<int, BattleStagePlayer>, int>((Func<KeyValuePair<int, BattleStagePlayer>, int>) (x => x.Value.deck_position)).ToList<int>();
              break;
            default:
              activeDeckPosition = MasterData.BattleStagePlayer.Where<KeyValuePair<int, BattleStagePlayer>>((Func<KeyValuePair<int, BattleStagePlayer>, bool>) (x => x.Value.stage_BattleStage == battleInfo.stageId && x.Value.deck_position != Consts.GetInstance().DECK_POSITION_FRIEND)).Select<KeyValuePair<int, BattleStagePlayer>, int>((Func<KeyValuePair<int, BattleStagePlayer>, int>) (x => x.Value.deck_position)).ToList<int>();
              break;
          }
          activeDeckPosition = activeDeckPosition.Except<int>((IEnumerable<int>) ((IEnumerable<BattleStageGuest>) battleInfo.Guests).Select<BattleStageGuest, int>((Func<BattleStageGuest, int>) (x => x.deck_position)).ToList<int>()).ToList<int>();
          playerCnt = activeDeckPosition.Count;
        }
        PlayerCharacterIntimate[] characterIntimate = SMManager.Get<PlayerCharacterIntimate[]>();
        if (Persist.tutorial.Data.IsFinishTutorial())
        {
          PlayerUnit[] myDeckUnit = playerCnt > 0 ? deck.player_units : (PlayerUnit[]) null;
          for (i = 0; i < playerCnt; ++i)
          {
            PlayerUnit pu = myDeckUnit[activeDeckPosition[i] - 1];
            if (pu != (PlayerUnit) null)
            {
              pu.SetIntimateList(characterIntimate);
              future = this.createUnitByPlayerUnit(pu, activeDeckPosition[i] - 1, false, battleInfo.playerCallSkillParam.same_character_id, (Checker) null);
              e = future.Wait();
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              players.Add(future.Result);
              future = (Future<BL.Unit>) null;
            }
            else
              players.Add((BL.Unit) null);
          }
          myDeckUnit = (PlayerUnit[]) null;
        }
        if (battleInfo.quest_type == CommonQuestType.Earth || battleInfo.quest_type == CommonQuestType.EarthExtra)
        {
          for (i = 0; i < battleInfo.EarthGuests.Length; ++i)
          {
            PlayerUnit pu = PlayerUnit.FromGuest(battleInfo.EarthGuests[i]);
            pu.SetIntimateList(characterIntimate);
            future = this.createUnitByPlayerUnit(pu, players.Count + i, false, 0, (Checker) null);
            e = future.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            guests.Add(future.Result);
            future = (Future<BL.Unit>) null;
          }
        }
        else
        {
          for (i = 0; i < battleInfo.Guests.Length; ++i)
          {
            BattleStageGuest guest = battleInfo.Guests[i];
            PlayerUnit pu = PlayerUnit.FromGuest(guest);
            pu.SetIntimateList(characterIntimate);
            future = this.createUnitByPlayerUnit(pu, guest.deck_position - 1, false, 0, (Checker) null);
            e = future.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            guests.Add(future.Result);
            if (guest.deck_position == Consts.GetInstance().DECK_POSITION_FRIEND)
              helperOverrWriteGuest = true;
            guest = (BattleStageGuest) null;
            future = (Future<BL.Unit>) null;
          }
        }
        if (!helperOverrWriteGuest && battleInfo.helper != null)
        {
          PlayerUnit leaderUnit = battleInfo.helper.leader_unit;
          leaderUnit.SetIntimateList(characterIntimate);
          leaderUnit.importOverkillersUnits(battleInfo.helper_overkillers);
          future = this.createUnitByPlayerUnit(leaderUnit, Consts.GetInstance().DECK_POSITION_FRIEND - 1, battleInfo.helper.enableLeaderSkill(), 0, (Checker) null);
          e = future.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          friend.Add(future.Result);
          future = (Future<BL.Unit>) null;
        }
        e = this.createEnemyUnits(battleInfo, enemies, userEnemies, env);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        env.playerUnits = new BL.ClassValue<List<BL.Unit>>(players.Where<BL.Unit>((Func<BL.Unit, bool>) (pu => pu != (BL.Unit) null)).Concat<BL.Unit>((IEnumerable<BL.Unit>) guests).Concat<BL.Unit>((IEnumerable<BL.Unit>) friend).OrderBy<BL.Unit, int>((Func<BL.Unit, int>) (x => x.index)).ToList<BL.Unit>());
        env.neutralUnits = new BL.ClassValue<List<BL.Unit>>(new List<BL.Unit>());
        env.enemyUnits = new BL.ClassValue<List<BL.Unit>>(enemies.Concat<BL.Unit>((IEnumerable<BL.Unit>) userEnemies).ToList<BL.Unit>());
        activeDeckPosition = (List<int>) null;
        characterIntimate = (PlayerCharacterIntimate[]) null;
      }
      else
      {
        for (playerCnt = 0; playerCnt < battleInfo.pvp_player_units.Length; ++playerCnt)
        {
          PlayerUnit pvpPlayerUnit = battleInfo.pvp_player_units[playerCnt];
          if (pvpPlayerUnit != (PlayerUnit) null)
          {
            pvpPlayerUnit.SetIntimateList(battleInfo.pvp_player_character_intimates);
            pvpPlayerUnit.primary_equipped_gear = pvpPlayerUnit.FindEquippedGear(battleInfo.pvp_player_items);
            pvpPlayerUnit.primary_equipped_gear2 = pvpPlayerUnit.FindEquippedGear2(battleInfo.pvp_player_items);
            pvpPlayerUnit.primary_equipped_gear3 = pvpPlayerUnit.FindEquippedGear3(battleInfo.pvp_player_items);
            pvpPlayerUnit.primary_equipped_reisou = pvpPlayerUnit.FindEquippedReisou(battleInfo.pvp_player_items, battleInfo.pvp_player_reisou_items);
            pvpPlayerUnit.primary_equipped_reisou2 = pvpPlayerUnit.FindEquippedReisou2(battleInfo.pvp_player_items, battleInfo.pvp_player_reisou_items);
            pvpPlayerUnit.primary_equipped_reisou3 = pvpPlayerUnit.FindEquippedReisou3(battleInfo.pvp_player_items, battleInfo.pvp_player_reisou_items);
            pvpPlayerUnit.primary_equipped_awake_skill = pvpPlayerUnit.FindEquippedExtraSkill(battleInfo.pvp_player_awake_skill);
            pvpPlayerUnit.resetUsedPrimary();
            int call_same_character_id = pvpPlayerUnit.is_enemy ? battleInfo.enemyCallSkillParam.same_character_id : battleInfo.playerCallSkillParam.same_character_id;
            future = this.createUnitByPlayerUnit(pvpPlayerUnit, playerCnt, false, call_same_character_id, checkUnitRules);
            e = future.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            players.Add(future.Result);
            future = (Future<BL.Unit>) null;
          }
          else
            players.Add((BL.Unit) null);
        }
        if (battleInfo.gvg_player_helpers != null && battleInfo.gvg_player_helpers.Length != 0)
        {
          PlayerUnit gvgPlayerHelper = battleInfo.gvg_player_helpers[0];
          if (gvgPlayerHelper != (PlayerUnit) null)
          {
            gvgPlayerHelper.SetIntimateList(new PlayerCharacterIntimate[0]);
            gvgPlayerHelper.primary_equipped_gear = gvgPlayerHelper.FindEquippedGear(battleInfo.gvg_player_helper_items);
            gvgPlayerHelper.primary_equipped_gear2 = gvgPlayerHelper.FindEquippedGear2(battleInfo.gvg_player_helper_items);
            gvgPlayerHelper.primary_equipped_gear3 = gvgPlayerHelper.FindEquippedGear3(battleInfo.gvg_player_helper_items);
            gvgPlayerHelper.primary_equipped_reisou = gvgPlayerHelper.FindEquippedReisou(battleInfo.gvg_player_helper_items, battleInfo.gvg_player_helper_reisou_items);
            gvgPlayerHelper.primary_equipped_reisou2 = gvgPlayerHelper.FindEquippedReisou2(battleInfo.gvg_player_helper_items, battleInfo.gvg_player_helper_reisou_items);
            gvgPlayerHelper.primary_equipped_reisou3 = gvgPlayerHelper.FindEquippedReisou3(battleInfo.gvg_player_helper_items, battleInfo.gvg_player_helper_reisou_items);
            gvgPlayerHelper.primary_equipped_awake_skill = gvgPlayerHelper.FindEquippedExtraSkill(battleInfo.gvg_player_helper_awake_skill);
            future = this.createUnitByPlayerUnit(gvgPlayerHelper, Consts.GetInstance().DECK_POSITION_FRIEND - 1, true, 0, checkUnitRules);
            e = future.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            players.Add(future.Result);
            future = (Future<BL.Unit>) null;
          }
          else
            players.Add((BL.Unit) null);
        }
        for (playerCnt = 0; playerCnt < battleInfo.pvp_enemy_units.Length; ++playerCnt)
        {
          PlayerUnit pvpEnemyUnit = battleInfo.pvp_enemy_units[playerCnt];
          if (pvpEnemyUnit != (PlayerUnit) null)
          {
            pvpEnemyUnit.SetIntimateList(battleInfo.pvp_enemy_character_intimates);
            pvpEnemyUnit.primary_equipped_gear = pvpEnemyUnit.FindEquippedGear(battleInfo.pvp_enemy_items);
            pvpEnemyUnit.primary_equipped_gear2 = pvpEnemyUnit.FindEquippedGear2(battleInfo.pvp_enemy_items);
            pvpEnemyUnit.primary_equipped_gear3 = pvpEnemyUnit.FindEquippedGear3(battleInfo.pvp_enemy_items);
            pvpEnemyUnit.primary_equipped_reisou = pvpEnemyUnit.FindEquippedReisou(battleInfo.pvp_enemy_items, battleInfo.pvp_enemy_reisou_items);
            pvpEnemyUnit.primary_equipped_reisou2 = pvpEnemyUnit.FindEquippedReisou2(battleInfo.pvp_enemy_items, battleInfo.pvp_enemy_reisou_items);
            pvpEnemyUnit.primary_equipped_reisou3 = pvpEnemyUnit.FindEquippedReisou3(battleInfo.pvp_enemy_items, battleInfo.pvp_enemy_reisou_items);
            pvpEnemyUnit.primary_equipped_awake_skill = pvpEnemyUnit.FindEquippedExtraSkill(battleInfo.pvp_enemy_awake_skill);
            pvpEnemyUnit.resetUsedPrimary();
            int call_same_character_id = pvpEnemyUnit.is_enemy ? battleInfo.enemyCallSkillParam.same_character_id : battleInfo.playerCallSkillParam.same_character_id;
            future = this.createUnitByPlayerUnit(pvpEnemyUnit, playerCnt, false, call_same_character_id, checkUnitRules);
            e = future.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            enemies.Add(future.Result);
            future = (Future<BL.Unit>) null;
          }
          else
            enemies.Add((BL.Unit) null);
        }
        if (battleInfo.gvg_enemy_helpers != null && battleInfo.gvg_enemy_helpers.Length != 0)
        {
          PlayerUnit gvgEnemyHelper = battleInfo.gvg_enemy_helpers[0];
          if (gvgEnemyHelper != (PlayerUnit) null)
          {
            gvgEnemyHelper.SetIntimateList(new PlayerCharacterIntimate[0]);
            gvgEnemyHelper.primary_equipped_gear = gvgEnemyHelper.FindEquippedGear(battleInfo.gvg_enemy_helper_items);
            gvgEnemyHelper.primary_equipped_gear2 = gvgEnemyHelper.FindEquippedGear2(battleInfo.gvg_enemy_helper_items);
            gvgEnemyHelper.primary_equipped_gear3 = gvgEnemyHelper.FindEquippedGear3(battleInfo.gvg_enemy_helper_items);
            gvgEnemyHelper.primary_equipped_reisou = gvgEnemyHelper.FindEquippedReisou(battleInfo.gvg_enemy_helper_items, battleInfo.gvg_enemy_helper_reisou_items);
            gvgEnemyHelper.primary_equipped_reisou2 = gvgEnemyHelper.FindEquippedReisou2(battleInfo.gvg_enemy_helper_items, battleInfo.gvg_enemy_helper_reisou_items);
            gvgEnemyHelper.primary_equipped_reisou3 = gvgEnemyHelper.FindEquippedReisou3(battleInfo.gvg_enemy_helper_items, battleInfo.gvg_enemy_helper_reisou_items);
            gvgEnemyHelper.primary_equipped_awake_skill = gvgEnemyHelper.FindEquippedExtraSkill(battleInfo.gvg_enemy_helper_awake_skill);
            future = this.createUnitByPlayerUnit(gvgEnemyHelper, Consts.GetInstance().DECK_POSITION_FRIEND - 1, true, 0, checkUnitRules);
            e = future.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            enemies.Add(future.Result);
            future = (Future<BL.Unit>) null;
          }
          else
            enemies.Add((BL.Unit) null);
        }
        env.playerUnits = new BL.ClassValue<List<BL.Unit>>(players.Where<BL.Unit>((Func<BL.Unit, bool>) (pu => pu != (BL.Unit) null)).ToList<BL.Unit>());
        env.neutralUnits = new BL.ClassValue<List<BL.Unit>>(new List<BL.Unit>());
        env.enemyUnits = new BL.ClassValue<List<BL.Unit>>(enemies.Where<BL.Unit>((Func<BL.Unit, bool>) (pu => pu != (BL.Unit) null)).ToList<BL.Unit>());
      }
      this.initializeCallSkillParam(battleInfo, env, env.playerUnits.value, env.enemyUnits.value);
      List<BL.Unit> facilities = new List<BL.Unit>();
      e = this.createFacilityUnits(battleInfo, facilities, env);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      int mapFacilityCount = facilities.Count;
      playerCnt = mapFacilityCount;
      i = -10001;
      e = this.initializeSkillFacilities(battleInfo, env.playerUnits.value, false, facilities, playerCnt, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      int num = facilities.Count - mapFacilityCount;
      playerCnt += num;
      i -= num;
      e = this.initializeSkillFacilities(battleInfo, env.enemyUnits.value, true, facilities, playerCnt, i);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      env.facilityUnits = new BL.ClassValue<List<BL.Unit>>(facilities);
      this.setLeaderSkills(env.playerUnits.value, env.enemyUnits.value);
      this.setLeaderSkills(env.enemyUnits.value, env.playerUnits.value);
      this.setAwakeSkills(env.playerUnits.value, env.enemyUnits.value);
      this.setAwakeSkills(env.enemyUnits.value, env.playerUnits.value);
      this.setPassiveSkills(env.playerUnits.value, env.enemyUnits.value);
      this.setPassiveSkills(env.enemyUnits.value, env.playerUnits.value);
      this.setGearSkills(env.playerUnits.value, env.enemyUnits.value);
      this.setGearSkills(env.enemyUnits.value, env.playerUnits.value);
      this.setFacilitySkills(env.facilityUnits.value, env);
      if (battleInfo.pvp && battleInfo.pvp_bonus_list != null)
      {
        this.setBonusSkills(env.playerUnits.value, battleInfo);
        this.setBonusSkills(env.enemyUnits.value, battleInfo);
      }
      if (battleInfo.gvg)
      {
        if (battleInfo.gvg_player_base_bonus_list != null)
          this.setGvgBonusSkills(env.playerUnits.value, battleInfo.gvg_player_base_bonus_list);
        if (battleInfo.gvg_enemy_base_bonus_list != null)
          this.setGvgBonusSkills(env.enemyUnits.value, battleInfo.gvg_enemy_base_bonus_list);
      }
      this.setUnitInitialHp(battleInfo, env.playerUnits);
      this.setUnitInitialHp(battleInfo, env.neutralUnits);
      this.setUnitInitialHp(battleInfo, env.enemyUnits);
      this.setUnitInitialHp(battleInfo, env.facilityUnits, true);
      if (battleInfo.quest_type == CommonQuestType.GuildRaid)
      {
        BL.Unit unit = env.enemyUnits.value.First<BL.Unit>((Func<BL.Unit, bool>) (u => u.playerUnit.id == battleInfo.raidBossDamage.Key));
        unit.hp -= battleInfo.raidBossDamage.Value;
        unit.initialHp = unit.hp;
      }
      this.GetInitialDeadEnemyDrop(env);
      foreach (BL.ISkillEffectListUnit beUnit in env.playerUnits.value)
        BattleFuncs.createBattleSkillEffectParams(beUnit, (IEnumerable<BL.Unit>) env.playerUnits.value, (IEnumerable<BL.Unit>) env.enemyUnits.value);
      foreach (BL.ISkillEffectListUnit beUnit in env.neutralUnits.value)
        BattleFuncs.createBattleSkillEffectParams(beUnit, (IEnumerable<BL.Unit>) env.neutralUnits.value, env.playerUnits.value.Concat<BL.Unit>((IEnumerable<BL.Unit>) env.enemyUnits.value));
      foreach (BL.ISkillEffectListUnit beUnit in env.enemyUnits.value)
        BattleFuncs.createBattleSkillEffectParams(beUnit, (IEnumerable<BL.Unit>) env.enemyUnits.value, (IEnumerable<BL.Unit>) env.playerUnits.value);
      if (!battleInfo.pvp && !battleInfo.gvg)
      {
        e = this.createUnitPositions(battleInfo, env, players, guests, friend, enemies, userEnemies, facilities, mapFacilityCount);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        int unitPositionId = 0;
        BattleStage stage = battleInfo.stage;
        List<BL.UnitPosition> first;
        List<BL.UnitPosition> second;
        if (battleInfo.pvp)
        {
          first = players.Select<BL.Unit, PvpStageFormation, BL.UnitPosition>(((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).Where<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.stage.ID == stage.ID && x.player_order == 0)), (Func<BL.Unit, PvpStageFormation, BL.UnitPosition>) ((a, b) =>
          {
            if (a == (BL.Unit) null)
              return (BL.UnitPosition) null;
            BL.UnitPosition unitPosition = new BL.UnitPosition();
            unitPosition.id = unitPositionId++;
            unitPosition.unit = a;
            unitPosition.row = b.formation_y - 1;
            unitPosition.column = b.formation_x - 1;
            unitPosition.direction = b.initial_direction;
            unitPosition.resetOriginalPosition(env);
            return unitPosition;
          })).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (a => a != null)).ToList<BL.UnitPosition>();
          second = enemies.Select<BL.Unit, PvpStageFormation, BL.UnitPosition>(((IEnumerable<PvpStageFormation>) MasterData.PvpStageFormationList).Where<PvpStageFormation>((Func<PvpStageFormation, bool>) (x => x.stage.ID == stage.ID && x.player_order == 1)), (Func<BL.Unit, PvpStageFormation, BL.UnitPosition>) ((a, b) =>
          {
            if (a == (BL.Unit) null)
              return (BL.UnitPosition) null;
            BL.UnitPosition unitPosition = new BL.UnitPosition();
            unitPosition.id = unitPositionId++;
            unitPosition.unit = a;
            unitPosition.row = b.formation_y - 1;
            unitPosition.column = b.formation_x - 1;
            unitPosition.direction = b.initial_direction;
            unitPosition.resetOriginalPosition(env);
            return unitPosition;
          })).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (a => a != null)).ToList<BL.UnitPosition>();
        }
        else if (battleInfo.gvg)
        {
          first = players.Select<BL.Unit, GvgStageFormation, BL.UnitPosition>(((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage.ID == stage.ID && x.player_order == 0)), (Func<BL.Unit, GvgStageFormation, BL.UnitPosition>) ((a, b) =>
          {
            if (a == (BL.Unit) null)
              return (BL.UnitPosition) null;
            BL.UnitPosition unitPosition = new BL.UnitPosition();
            unitPosition.id = unitPositionId++;
            unitPosition.unit = a;
            unitPosition.row = b.formation_y - 1;
            unitPosition.column = b.formation_x - 1;
            unitPosition.direction = b.initial_direction;
            unitPosition.resetOriginalPosition(env);
            return unitPosition;
          })).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (a => a != null)).ToList<BL.UnitPosition>();
          second = enemies.Select<BL.Unit, GvgStageFormation, BL.UnitPosition>(((IEnumerable<GvgStageFormation>) MasterData.GvgStageFormationList).Where<GvgStageFormation>((Func<GvgStageFormation, bool>) (x => x.stage.ID == stage.ID && x.player_order == 1)), (Func<BL.Unit, GvgStageFormation, BL.UnitPosition>) ((a, b) =>
          {
            if (a == (BL.Unit) null)
              return (BL.UnitPosition) null;
            BL.UnitPosition unitPosition = new BL.UnitPosition();
            unitPosition.id = unitPositionId++;
            unitPosition.unit = a;
            unitPosition.row = b.formation_y - 1;
            unitPosition.column = b.formation_x - 1;
            unitPosition.direction = b.initial_direction;
            unitPosition.resetOriginalPosition(env);
            return unitPosition;
          })).Where<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (a => a != null)).ToList<BL.UnitPosition>();
        }
        else
          first = second = new List<BL.UnitPosition>();
        List<BL.UnitPosition> list1 = facilities.Take<BL.Unit>(mapFacilityCount).Select<BL.Unit, Tuple<int, int>, BL.UnitPosition>((IEnumerable<Tuple<int, int>>) battleInfo.facility_coordinates, (Func<BL.Unit, Tuple<int, int>, BL.UnitPosition>) ((a, b) =>
        {
          BL.UnitPosition unitPosition = this.checkUnitPosition(a, env);
          unitPosition.id = unitPositionId++;
          unitPosition.unit = a;
          unitPosition.row = b.Item2 - 1;
          unitPosition.column = b.Item1 - 1;
          unitPosition.resetOriginalPosition(env);
          return unitPosition;
        })).ToList<BL.UnitPosition>();
        List<BL.UnitPosition> list2 = facilities.Skip<BL.Unit>(mapFacilityCount).Select<BL.Unit, BL.UnitPosition>((Func<BL.Unit, BL.UnitPosition>) (a =>
        {
          BL.UnitPosition unitPosition = new BL.UnitPosition();
          unitPosition.id = unitPositionId++;
          unitPosition.unit = a;
          unitPosition.row = 0;
          unitPosition.column = 0;
          unitPosition.resetOriginalPosition(env);
          return unitPosition;
        })).ToList<BL.UnitPosition>();
        List<BL.UnitPosition> list3 = first.Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) second).Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) list1).Concat<BL.UnitPosition>((IEnumerable<BL.UnitPosition>) list2).ToList<BL.UnitPosition>();
        env.unitPositions = new BL.ClassValue<List<BL.UnitPosition>>(list3);
      }
      env.setCurrentUnitWith((BL.Unit) null, (Action<BL.UnitPosition>) (_ => { }));
      if (!battleInfo.pvp && !battleInfo.gvg)
      {
        List<BL.Item> list = ((IEnumerable<PlayerItem>) battleInfo.items).Select<PlayerItem, BL.Item>((Func<PlayerItem, BL.Item>) (pi => pi.toBLItem())).ToList<BL.Item>();
        env.itemList = new BL.ClassValue<List<BL.Item>>(list.ToList<BL.Item>());
        env.itemListInBattle = new BL.ClassValue<List<BL.Item>>(list.ToList<BL.Item>());
      }
      else
      {
        env.itemList = new BL.ClassValue<List<BL.Item>>(new List<BL.Item>());
        env.itemListInBattle = new BL.ClassValue<List<BL.Item>>(new List<BL.Item>());
      }
      env.sight.value = 1;
      this.setupStoryScript(battleInfo, env);
      this.createFieldEffects(battleInfo.stage, env);
      if (battleInfo.pvp)
        env.initializeIntimatePvp();
      else
        env.initializeIntimate();
      env.battleLogger = new BL.BattleLogger(env);
    }

    private void initializeCallSkillParam(
      BattleInfo battleInfo,
      BL env,
      List<BL.Unit> players,
      List<BL.Unit> enemies)
    {
      BL.CallSkillState playerCallSkillState = env.playerCallSkillState;
      BL.CallSkillState enemyCallSkillState = env.enemyCallSkillState;
      CallCharacter[] callCharacterList = MasterData.CallCharacterList;
      if (battleInfo.playerCallSkillParam == null || battleInfo.enemyCallSkillParam == null)
        return;
      playerCallSkillState.sameCharacterID = battleInfo.playerCallSkillParam.same_character_id;
      playerCallSkillState.isSameCharacterJoin = false;
      playerCallSkillState.skillId = 0;
      if (playerCallSkillState.sameCharacterID != 0)
      {
        foreach (BL.Unit player in players)
        {
          if (!(player == (BL.Unit) null) && player.unit.same_character_id == playerCallSkillState.sameCharacterID)
          {
            playerCallSkillState.isSameCharacterJoin = true;
            break;
          }
        }
        foreach (CallCharacter callCharacter in callCharacterList)
        {
          if (callCharacter.same_character_id == playerCallSkillState.sameCharacterID)
          {
            playerCallSkillState.skillId = callCharacter.call_skill_id;
            break;
          }
        }
      }
      playerCallSkillState.intimateRank = battleInfo.playerCallSkillParam.intimate_rank;
      playerCallSkillState.playerRank = battleInfo.playerCallSkillParam.player_rank;
      enemyCallSkillState.sameCharacterID = battleInfo.enemyCallSkillParam.same_character_id;
      enemyCallSkillState.isSameCharacterJoin = false;
      enemyCallSkillState.skillId = 0;
      if (enemyCallSkillState.sameCharacterID != 0)
      {
        foreach (BL.Unit enemy in enemies)
        {
          if (!(enemy == (BL.Unit) null) && enemy.unit.same_character_id == enemyCallSkillState.sameCharacterID)
          {
            enemyCallSkillState.isSameCharacterJoin = true;
            break;
          }
        }
        foreach (CallCharacter callCharacter in callCharacterList)
        {
          if (callCharacter.same_character_id == enemyCallSkillState.sameCharacterID)
          {
            enemyCallSkillState.skillId = callCharacter.call_skill_id;
            break;
          }
        }
      }
      enemyCallSkillState.intimateRank = battleInfo.enemyCallSkillParam.intimate_rank;
      enemyCallSkillState.playerRank = battleInfo.enemyCallSkillParam.player_rank;
    }

    public IEnumerator doStart(BattleInfo battleInfo, BL env)
    {
      IEnumerator e = this.initializeData(battleInfo, env);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      foreach (BL.Unit unit in env.playerUnits.value)
        unit.isPlayerControl = true;
    }
  }
}
