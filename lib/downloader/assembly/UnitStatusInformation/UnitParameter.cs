// Decompiled with JetBrains decompiler
// Type: UnitStatusInformation.UnitParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace UnitStatusInformation
{
  public class UnitParameter
  {
    private UnitUnit masterUnit_;
    private MasterDataTable.UnitJob masterJob_;
    private Judgement.NonBattleParameter nonBattleParam_;
    private Judgement.BattleParameter battleParam_;
    private PlayerUnitSkills[] displaySkills_;
    private LinkedList<PlayerUnitSkills> tempSkills_;

    public bool disableSkills { get; private set; }

    public bool isInBattle { get; private set; }

    public bool isInColosseum { get; private set; }

    public bool isMemory { get; private set; }

    public BL.Unit battleUnit { get; private set; }

    public PlayerUnit playerUnit { get; private set; }

    public UnitUnit unit => this.masterUnit_ ?? (this.masterUnit_ = this.playerUnit.unit);

    public MasterDataTable.UnitJob job
    {
      get => this.masterJob_ ?? (this.masterJob_ = this.playerUnit.getJobData());
    }

    public Judgement.NonBattleParameter nonBattleParam
    {
      get
      {
        return this.nonBattleParam_ ?? (this.nonBattleParam_ = this.isMemory ? Judgement.NonBattleParameter.FromPlayerUnitMemory(this.playerUnit) : Judgement.NonBattleParameter.FromPlayerUnit(this.playerUnit));
      }
    }

    public Judgement.BattleParameter battleParam
    {
      get
      {
        return this.battleParam_ ?? (this.battleParam_ = this.isInColosseum ? Judgement.BattleParameter.FromBeColosseumUnit(this.battleUnit, this.playerUnit.equippedGear, this.playerUnit.equippedGear2, this.playerUnit.equippedReisou, this.playerUnit.equippedReisou2, this.playerUnit.equippedReisou3) : Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) this.battleUnit));
      }
    }

    public UnitParameter(PlayerUnit pu, bool isMemory = false) => this.initCommon(pu, isMemory);

    public UnitParameter(BL.Unit bu, bool bColosseum = false) => this.initCommon(bu, bColosseum);

    private void initCommon(PlayerUnit unit, bool bMemory)
    {
      this.battleUnit = (BL.Unit) null;
      this.playerUnit = unit;
      this.disableSkills = false;
      this.isInBattle = false;
      this.isInColosseum = false;
      this.isMemory = bMemory;
    }

    private void initCommon(BL.Unit unit, bool bColosseum)
    {
      this.battleUnit = unit;
      this.playerUnit = unit.playerUnit;
      this.disableSkills = unit.crippled;
      this.isInBattle = true;
      this.isInColosseum = bColosseum;
      this.isMemory = false;
    }

    private PlayerUnitSkills[] displaySkills
    {
      get => this.displaySkills_ ?? (this.displaySkills_ = this.getDisplaySkills());
    }

    private PlayerUnitSkills[] getDisplaySkills()
    {
      return this.playerUnit?.skills != null ? ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x =>
      {
        BattleskillSkill skill = x.skill;
        return skill.DispSkillList && skill.skill_type != BattleskillSkillType.growth;
      })).OrderBy<PlayerUnitSkills, int>((Func<PlayerUnitSkills, int>) (y => y.skill_id)).ToArray<PlayerUnitSkills>() : (PlayerUnitSkills[]) null;
    }

    public UnitParameter.SkillSortUnit[] sortedSkills => this.getSkillsInBattle();

    private UnitParameter.SkillSortUnit[] getSkillsInBattle()
    {
      this.tempSkills_ = this.displaySkills != null ? new LinkedList<PlayerUnitSkills>((IEnumerable<PlayerUnitSkills>) this.displaySkills) : new LinkedList<PlayerUnitSkills>();
      List<UnitParameter.SkillSortUnit> skillSortUnitList = new List<UnitParameter.SkillSortUnit>();
      if (!this.disableSkills)
      {
        this.setLeaderSkill(skillSortUnitList);
        this.setPrincessSkill(skillSortUnitList);
      }
      this.setElementSkill(skillSortUnitList);
      if (!this.disableSkills)
      {
        this.setMultiSkill(skillSortUnitList);
        this.setOverkillersSkill(skillSortUnitList);
        this.setReleaseSkill(skillSortUnitList);
        this.setCommandSkill(skillSortUnitList);
      }
      this.setGrantSkill(skillSortUnitList, this.disableSkills);
      if (!this.disableSkills)
      {
        this.setDuelSkill(skillSortUnitList);
        this.setEquipSkill(skillSortUnitList);
        this.setExtraSkill(skillSortUnitList);
        this.setJobAbility(skillSortUnitList);
        this.setReisouSkill(skillSortUnitList);
        this.setSEASkill(skillSortUnitList);
      }
      this.tempSkills_ = (LinkedList<PlayerUnitSkills>) null;
      return skillSortUnitList.OrderBy<UnitParameter.SkillSortUnit, int>((Func<UnitParameter.SkillSortUnit, int>) (s => s.priority)).ToArray<UnitParameter.SkillSortUnit>();
    }

    private void addSingle(List<UnitParameter.SkillSortUnit> lst, UnitParameter.SkillSortUnit su)
    {
      if (lst.Any<UnitParameter.SkillSortUnit>((Func<UnitParameter.SkillSortUnit, bool>) (x => x.id == su.id)))
        return;
      lst.Add(su);
    }

    private List<UnitParameter.SkillSortUnit> setLeaderSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      PlayerUnitLeader_skills leaderSkill = this.playerUnit.leader_skill;
      if (leaderSkill != null)
      {
        if (this.playerUnit.is_enemy)
        {
          if (this.playerUnit.is_enemy_leader || this.battleUnit.is_leader || this.battleUnit.friend)
            this.addSingle(lstSort, UnitParameter.SkillSortUnit.create(leaderSkill));
        }
        else if (this.battleUnit.is_leader || this.battleUnit.friend)
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.create(leaderSkill));
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setElementSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      foreach (PlayerUnitSkills tempSkill in this.tempSkills_)
      {
        if (BattleskillSkill.InvestElementSkillIds.Contains(tempSkill.skill_id))
        {
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByElement(tempSkill));
          this.tempSkills_.Remove(tempSkill);
          break;
        }
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setMultiSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      Dictionary<int, UnitSkillEvolution> unitSkillEvolutionDict = ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Where<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit_UnitUnit == this.playerUnit._unit)).ToDictionary<UnitSkillEvolution, int>((Func<UnitSkillEvolution, int>) (x => x.after_skill_BattleskillSkill));
      UnitUnit unitUnit = this.playerUnit.unit;
      int charId = unitUnit.character_UnitCharacter;
      bool flag = false;
      UnitSkillHarmonyQuest[] array1 = ((IEnumerable<UnitSkillHarmonyQuest>) MasterData.UnitSkillHarmonyQuestList).Where<UnitSkillHarmonyQuest>((Func<UnitSkillHarmonyQuest, bool>) (x => x.character_UnitCharacter == charId && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillHarmonyQuest>();
      if (array1.Length != 0)
      {
        foreach (PlayerUnitSkills skill in ((IEnumerable<UnitSkillHarmonyQuest>) array1).Select<UnitSkillHarmonyQuest, PlayerUnitSkills>((Func<UnitSkillHarmonyQuest, PlayerUnitSkills>) (s => this.tempSkills_.FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (fd =>
        {
          if (s.skill_BattleskillSkill == fd.skill_id)
            return true;
          UnitSkillEvolution unitSkillEvolution;
          return unitSkillEvolutionDict.TryGetValue(fd.skill_id, out unitSkillEvolution) && s.skill_BattleskillSkill == unitSkillEvolution.before_skill_BattleskillSkill;
        })))).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (w => w != null)).Distinct<PlayerUnitSkills>().ToArray<PlayerUnitSkills>())
        {
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByMulti(skill));
          this.tempSkills_.Remove(skill);
          flag = true;
        }
      }
      if (!flag)
      {
        UnitSkillIntimate[] array2 = ((IEnumerable<UnitSkillIntimate>) MasterData.UnitSkillIntimateList).Where<UnitSkillIntimate>((Func<UnitSkillIntimate, bool>) (x => x.unit_UnitUnit == unitUnit.ID && x.skill.DispSkillList)).ToArray<UnitSkillIntimate>();
        if (array2.Length != 0)
        {
          PlayerUnitSkills[] array3 = ((IEnumerable<UnitSkillIntimate>) array2).Select<UnitSkillIntimate, PlayerUnitSkills>((Func<UnitSkillIntimate, PlayerUnitSkills>) (s => this.tempSkills_.FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (fd =>
          {
            if (s.skill_BattleskillSkill == fd.skill_id)
              return true;
            UnitSkillEvolution unitSkillEvolution;
            return unitSkillEvolutionDict.TryGetValue(fd.skill_id, out unitSkillEvolution) && s.skill_BattleskillSkill == unitSkillEvolution.before_skill_BattleskillSkill;
          })))).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (w => w != null)).Distinct<PlayerUnitSkills>().ToArray<PlayerUnitSkills>();
          if (array3.Length != 0)
          {
            foreach (PlayerUnitSkills skill in array3)
            {
              this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByMulti(skill));
              this.tempSkills_.Remove(skill);
            }
          }
        }
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setOverkillersSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      foreach (PlayerUnitSkills overkillersSkill in this.playerUnit.equippedOverkillersSkills)
        this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByOverkillers(overkillersSkill));
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setReleaseSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      foreach (PlayerUnitSkills skill in this.tempSkills_.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type == BattleskillSkillType.release)).ToArray<PlayerUnitSkills>())
      {
        this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByRelease(skill));
        this.tempSkills_.Remove(skill);
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setCommandSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      foreach (PlayerUnitSkills skill in this.tempSkills_.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type == BattleskillSkillType.command)).ToArray<PlayerUnitSkills>())
      {
        this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByCommand(skill));
        this.tempSkills_.Remove(skill);
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setPrincessSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      HashSet<int> skillKeys = new HashSet<int>(((IEnumerable<UnitSkill>) this.playerUnit.unit.RememberUnitAllSkills()).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_type == this.playerUnit._unit_type)).Select<UnitSkill, int>((Func<UnitSkill, int>) (y => y.skill_BattleskillSkill)));
      if (!skillKeys.Any<int>())
        return lstSort;
      foreach (PlayerUnitSkills skill in this.tempSkills_.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => skillKeys.Contains(x.skill_id))).ToArray<PlayerUnitSkills>())
      {
        this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByPrincess(skill));
        this.tempSkills_.Remove(skill);
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setGrantSkill(
      List<UnitParameter.SkillSortUnit> lstSort,
      bool bDisableSkills)
    {
      foreach (PlayerUnitSkills skill in !bDisableSkills ? this.tempSkills_.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type == BattleskillSkillType.passive)).ToArray<PlayerUnitSkills>() : this.tempSkills_.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => UnitParameter.checkGrantAlways(x.skill))).ToArray<PlayerUnitSkills>())
      {
        this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByGrant(skill));
        this.tempSkills_.Remove(skill);
      }
      return lstSort;
    }

    private static bool checkGrantAlways(BattleskillSkill skill)
    {
      if (skill.skill_type == BattleskillSkillType.passive)
      {
        BattleskillGenre? nullable = skill.genre1;
        if (7 == (int) nullable.GetValueOrDefault() & nullable.HasValue)
        {
          nullable = skill.genre2;
          return !nullable.HasValue;
        }
      }
      return false;
    }

    private List<UnitParameter.SkillSortUnit> setDuelSkill(List<UnitParameter.SkillSortUnit> lstSort)
    {
      foreach (PlayerUnitSkills skill in this.tempSkills_.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type == BattleskillSkillType.duel)).ToArray<PlayerUnitSkills>())
      {
        this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByDuel(skill));
        this.tempSkills_.Remove(skill);
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setEquipSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      PlayerItem equippedGear = this.playerUnit.equippedGear;
      if (equippedGear != (PlayerItem) null)
      {
        foreach (GearGearSkill skill in equippedGear.skills)
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.create(skill));
      }
      PlayerItem equippedGear2 = this.playerUnit.equippedGear2;
      if (equippedGear2 != (PlayerItem) null)
      {
        foreach (GearGearSkill skill in equippedGear2.skills)
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.create(skill));
      }
      PlayerItem equippedGear3 = this.playerUnit.equippedGear3;
      if (equippedGear3 != (PlayerItem) null)
      {
        foreach (GearGearSkill skill in equippedGear3.skills)
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.create(skill));
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setExtraSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      if (this.playerUnit.unit.trust_target_flag && !this.playerUnit.is_guest)
      {
        PlayerAwakeSkill equippedExtraSkill = this.playerUnit.equippedExtraSkill;
        if (equippedExtraSkill != null)
        {
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByExtra(equippedExtraSkill));
        }
        else
        {
          foreach (PlayerUnitSkills skill in this.tempSkills_.Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.awake_skill_category_id != 1)).ToArray<PlayerUnitSkills>())
          {
            this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByExtra(skill));
            this.tempSkills_.Remove(skill);
          }
        }
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setJobAbility(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      if (this.playerUnit.job_abilities != null && ((IEnumerable<PlayerUnitJob_abilities>) this.playerUnit.job_abilities).Any<PlayerUnitJob_abilities>((Func<PlayerUnitJob_abilities, bool>) (x => x.job_ability_id != 0)))
      {
        HashSet<int> ignoreIds = new HashSet<int>(((IEnumerable<PlayerUnitJob_abilities>) this.playerUnit.job_abilities).Where<PlayerUnitJob_abilities>((Func<PlayerUnitJob_abilities, bool>) (x => x.job_ability_id != 0 && x.skill2_id != 0)).Select<PlayerUnitJob_abilities, int>((Func<PlayerUnitJob_abilities, int>) (y => y.skill2_id)));
        foreach (PlayerUnitSkills skill in ((IEnumerable<PlayerUnitSkills>) this.playerUnit.retrofitSkills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.IsJobAbility && !ignoreIds.Contains(x.skill_id))))
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByJobAbility(skill));
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setReisouSkill(
      List<UnitParameter.SkillSortUnit> lstSort)
    {
      PlayerItem equippedGear = this.playerUnit.equippedGear;
      PlayerItem equippedReisou = this.playerUnit.equippedReisou;
      if (equippedGear != (PlayerItem) null && equippedReisou != (PlayerItem) null)
      {
        foreach (GearReisouSkill reisouSkill in equippedReisou.getReisouSkills(equippedGear.entity_id))
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByReisou(reisouSkill));
      }
      PlayerItem equippedGear2 = this.playerUnit.equippedGear2;
      PlayerItem equippedReisou2 = this.playerUnit.equippedReisou2;
      if (equippedGear2 != (PlayerItem) null && equippedReisou2 != (PlayerItem) null)
      {
        foreach (GearReisouSkill reisouSkill in equippedReisou2.getReisouSkills(equippedGear2.entity_id))
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByReisou(reisouSkill));
      }
      PlayerItem equippedGear3 = this.playerUnit.equippedGear3;
      PlayerItem equippedReisou3 = this.playerUnit.equippedReisou3;
      if (equippedGear3 != (PlayerItem) null && equippedReisou3 != (PlayerItem) null)
      {
        foreach (GearReisouSkill reisouSkill in equippedReisou3.getReisouSkills(equippedGear3.entity_id))
          this.addSingle(lstSort, UnitParameter.SkillSortUnit.createByReisou(reisouSkill));
      }
      return lstSort;
    }

    private List<UnitParameter.SkillSortUnit> setSEASkill(List<UnitParameter.SkillSortUnit> lstSort)
    {
      PlayerUnitSkills seaSkill = this.playerUnit.SEASkill;
      if (seaSkill != null)
        this.addSingle(lstSort, UnitParameter.SkillSortUnit.createBySEASkill(seaSkill, true, this.playerUnit.countEquippedOverkillers));
      return lstSort;
    }

    public enum SkillGroup
    {
      None,
      Leader,
      Element,
      Growth,
      Multi,
      Overkillers,
      Release,
      Command,
      Princess,
      Grant,
      Duel,
      Equip,
      Extra,
      JobAbility,
      Reisou,
      SEA,
    }

    private enum SkillPriority
    {
      Unknown,
      Leader,
      Element,
      Growth,
      Release,
      Command,
      SEA,
      Grant,
      Duel,
      Equip,
      Extra,
      Princess,
      Multi,
      Overkillers,
      JobAbility,
      Reisou,
    }

    public abstract class SkillSortUnit
    {
      private UnitParameter.SkillPriority? priority_;

      public UnitParameter.SkillGroup group { get; private set; }

      public int id { get; private set; }

      public SkillSortUnit(UnitParameter.SkillGroup g, int skillId)
      {
        this.group = g;
        this.id = skillId;
      }

      public int priority
      {
        get
        {
          return (int) (this.priority_.HasValue ? new UnitParameter.SkillPriority?(this.priority_.Value) : (this.priority_ = new UnitParameter.SkillPriority?(this.getPriority()))).Value;
        }
      }

      private UnitParameter.SkillPriority getPriority()
      {
        UnitParameter.SkillPriority result;
        return Enum.TryParse<UnitParameter.SkillPriority>(this.group.ToString(), out result) ? result : UnitParameter.SkillPriority.Unknown;
      }

      public virtual PlayerUnitLeader_skills leaderSkill => (PlayerUnitLeader_skills) null;

      public virtual PlayerUnitSkills elementSkill => (PlayerUnitSkills) null;

      public virtual PlayerUnitSkills multiSkill => (PlayerUnitSkills) null;

      public virtual PlayerUnitSkills overkillersSkill => (PlayerUnitSkills) null;

      public virtual PlayerUnitSkills releaseSkill => (PlayerUnitSkills) null;

      public virtual PlayerUnitSkills commandSkill => (PlayerUnitSkills) null;

      public virtual PlayerUnitSkills princessSkill => (PlayerUnitSkills) null;

      public virtual PlayerUnitSkills grantSkill => (PlayerUnitSkills) null;

      public virtual PlayerUnitSkills duelSkill => (PlayerUnitSkills) null;

      public virtual GearGearSkill equipSkill => (GearGearSkill) null;

      public virtual PlayerAwakeSkill extraSkill => (PlayerAwakeSkill) null;

      public virtual PlayerUnitSkills jobAbility => (PlayerUnitSkills) null;

      public virtual GearReisouSkill reisouSkill => (GearReisouSkill) null;

      public virtual BattleskillSkill SEASkill => (BattleskillSkill) null;

      public abstract PopupSkillDetails.Param toPopupParam { get; }

      public static UnitParameter.SkillSortUnit create(PlayerUnitLeader_skills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.LeaderSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByElement(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.ElementSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByMulti(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.MultiSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByOverkillers(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.OverkillersSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByRelease(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.ReleaseSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByCommand(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.CommandSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByPrincess(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.PrincessSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByGrant(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.GrantSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByDuel(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.DuelSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit create(GearGearSkill skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.EquipSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByReisou(GearReisouSkill skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.ReisouSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByExtra(PlayerAwakeSkill skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.ExtraSkillSortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createByExtra(PlayerUnitSkills skill)
      {
        PlayerAwakeSkill skill1 = new PlayerAwakeSkill();
        int skillId;
        int num = skillId = skill.skill_id;
        skill1.id = skillId;
        skill1.skill_id = num;
        skill1.level = skill.level;
        return UnitParameter.SkillSortUnit.createByExtra(skill1);
      }

      public static UnitParameter.SkillSortUnit createByJobAbility(PlayerUnitSkills skill)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.JobAbilitySortUnit(skill);
      }

      public static UnitParameter.SkillSortUnit createBySEASkill(
        PlayerUnitSkills skill,
        bool bUnlocked,
        int num)
      {
        return (UnitParameter.SkillSortUnit) new UnitParameter.SEASkillSortUnit(skill.skill, bUnlocked, num);
      }
    }

    private class LeaderSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitLeader_skills skill_;

      public override PlayerUnitLeader_skills leaderSkill => this.skill_;

      public LeaderSkillSortUnit(PlayerUnitLeader_skills skill)
        : base(UnitParameter.SkillGroup.Leader, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_);
      }
    }

    private class ElementSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills elementSkill => this.skill_;

      public ElementSkillSortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.Element, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class MultiSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills multiSkill => this.skill_;

      public MultiSkillSortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.Multi, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class OverkillersSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills overkillersSkill => this.skill_;

      public OverkillersSkillSortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.Overkillers, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class ReleaseSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills releaseSkill => this.skill_;

      public ReleaseSkillSortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.Release, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class CommandSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills commandSkill => this.skill_;

      public CommandSkillSortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.Command, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class PrincessSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills princessSkill => this.skill_;

      public PrincessSkillSortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.Princess, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class GrantSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills grantSkill => this.skill_;

      public GrantSkillSortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.Grant, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class DuelSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills duelSkill => this.skill_;

      public DuelSkillSortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.Duel, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class EquipSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private GearGearSkill skill_;

      public override GearGearSkill equipSkill => this.skill_;

      public EquipSkillSortUnit(GearGearSkill skill)
        : base(UnitParameter.SkillGroup.Equip, skill.skill_BattleskillSkill)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_);
      }
    }

    private class ReisouSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private GearReisouSkill skill_;

      public override GearReisouSkill reisouSkill => this.skill_;

      public ReisouSkillSortUnit(GearReisouSkill skill)
        : base(UnitParameter.SkillGroup.Reisou, skill.skill_BattleskillSkill)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_);
      }
    }

    private class ExtraSkillSortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerAwakeSkill skill_;

      public override PlayerAwakeSkill extraSkill => this.skill_;

      public ExtraSkillSortUnit(PlayerAwakeSkill skill)
        : base(UnitParameter.SkillGroup.Extra, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_);
      }
    }

    private class JobAbilitySortUnit : UnitParameter.SkillSortUnit
    {
      private PlayerUnitSkills skill_;

      public override PlayerUnitSkills jobAbility => this.skill_;

      public JobAbilitySortUnit(PlayerUnitSkills skill)
        : base(UnitParameter.SkillGroup.JobAbility, skill.skill_id)
      {
        this.skill_ = skill;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get => new PopupSkillDetails.Param(this.skill_, this.group);
      }
    }

    private class SEASkillSortUnit : UnitParameter.SkillSortUnit
    {
      private BattleskillSkill skill_;
      private bool isUnlocked_;
      private int numEquipped_;

      public override BattleskillSkill SEASkill => this.skill_;

      public SEASkillSortUnit(BattleskillSkill skill, bool bUnlocked, int num)
        : base(UnitParameter.SkillGroup.SEA, skill.ID)
      {
        this.skill_ = skill;
        this.isUnlocked_ = bUnlocked;
        this.numEquipped_ = num;
      }

      public override PopupSkillDetails.Param toPopupParam
      {
        get
        {
          return PopupSkillDetails.Param.createBySEASkillView(this.skill_, this.isUnlocked_, this.numEquipped_);
        }
      }
    }
  }
}
