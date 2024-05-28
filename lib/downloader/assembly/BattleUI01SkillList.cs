// Decompiled with JetBrains decompiler
// Type: BattleUI01SkillList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleUI01SkillList : BattleMonoBehaviour
{
  [SerializeField]
  protected GameObject dir_Replacement_Skill;
  [SerializeField]
  protected UIGrid grid_Replacement_Skil_none;
  [SerializeField]
  protected UISprite[] slc_Replacement_Skill_none;
  [SerializeField]
  protected UIGrid grid_Replacement_Skil;
  [SerializeField]
  protected GameObject[] dir_Replacement_Skill_have;
  [SerializeField]
  protected UISprite slc_line_Replacement_Skil;
  [SerializeField]
  protected GameObject dir_Unit_Skill;
  [SerializeField]
  protected UIGrid grid_Unit_Skil_none;
  [SerializeField]
  protected UISprite[] slc_Unit_Skill_none;
  [SerializeField]
  protected UIGrid grid_Unit_Skil;
  [SerializeField]
  protected GameObject[] dir_Unit_Skill_have;
  [SerializeField]
  protected UISprite slc_line_Unit_Skil;
  [SerializeField]
  protected GameObject dir_LS_Skill;
  [SerializeField]
  protected UIGrid grid_LS_Skil_none;
  [SerializeField]
  protected UISprite[] slc_LS_Skill_none;
  [SerializeField]
  protected UIGrid grid_LS_Skil;
  [SerializeField]
  protected GameObject[] dir_LS_Skill_have;
  [SerializeField]
  protected UISprite slc_line_LS_Skil;
  [SerializeField]
  protected GameObject dir_Duel_Skill;
  [SerializeField]
  protected UIGrid grid_Duel_Skil_none;
  [SerializeField]
  protected UISprite[] slc_Duel_Skill_none;
  [SerializeField]
  protected UIGrid grid_Duel_Skil;
  [SerializeField]
  protected GameObject[] dir_Duel_Skill_have;
  [SerializeField]
  protected UISprite slc_line_Duel_Skil;
  [SerializeField]
  protected GameObject dir_Command_Skill;
  [SerializeField]
  protected UIGrid grid_Command_Skil_none;
  [SerializeField]
  protected UISprite[] slc_Command_Skill_none;
  [SerializeField]
  protected UIGrid grid_Command_Skil;
  [SerializeField]
  protected GameObject[] dir_Command_Skill_have;
  [SerializeField]
  protected UISprite slc_line_Command_Skil;
  [SerializeField]
  protected GameObject dir_Grant_Skill;
  [SerializeField]
  protected UIGrid grid_Grant_Skil_none;
  [SerializeField]
  protected UISprite[] slc_Grant_Skill_none;
  [SerializeField]
  protected UIGrid grid_Grant_Skil;
  [SerializeField]
  protected GameObject[] dir_Grant_Skill_have;
  [SerializeField]
  protected UISprite slc_line_Grant_Skil;
  [SerializeField]
  protected GameObject dir_Grant_Skill_02;
  [SerializeField]
  protected UIGrid grid_Grant_Skil_02_none;
  [SerializeField]
  protected UISprite[] slc_Grant_Skill_02_none;
  [SerializeField]
  protected UIGrid grid_Grant_Skil_02;
  [SerializeField]
  protected GameObject[] dir_Grant_Skill_02_have;
  [SerializeField]
  protected UISprite slc_line_Grant_Skil_02;
  [SerializeField]
  protected GameObject dir_Armor_Skill;
  [SerializeField]
  protected UIGrid grid_Armor_Skil_none;
  [SerializeField]
  protected UISprite[] slc_Armor_Skill_none;
  [SerializeField]
  protected UIGrid grid_Armor_Skil;
  [SerializeField]
  protected GameObject[] dir_Armor_Skill_have;
  [SerializeField]
  protected UIScrollView scrollView;
  [SerializeField]
  protected UIGrid scrollGrid;
  [SerializeField]
  protected UIWidget Anchor_height;
  [SerializeField]
  protected GameObject dyn_skillDetailDialog;
  [SerializeField]
  protected UIButton ibtn_Popup_close;
  [SerializeField]
  protected GameObject dir_View_check;
  protected PlayerUnit playerUnit;
  protected BL.BattleModified<BL.Unit> modified;
  protected GameObject skillTypeIconPrefab;
  protected int skillIconDepth;
  protected List<int> setSkills = new List<int>();
  private GameObject skillDialog;
  private GameObject skillDialogPrefab;
  private GameObject skillDialogChange;
  private GameObject skillDialogChangePrefab;
  protected bool is_leader;
  protected bool is_friend;
  protected UnitSkill[] allSkills = new UnitSkill[0];
  protected UISprite bottomLineSpr;

  private bool setReplacementSkill()
  {
    int index = 0;
    bool flag = false;
    if (!Object.op_Inequality((Object) this.battleManager, (Object) null) || !this.battleManager.isEarth)
    {
      if (this.playerUnit.unit.trust_target_flag && !this.playerUnit.is_guest)
      {
        if (this.playerUnit.is_enemy && (!Object.op_Inequality((Object) this.battleManager, (Object) null) || !this.battleManager.isOvo))
        {
          flag = true;
        }
        else
        {
          PlayerAwakeSkill equippedExtraSkill = this.playerUnit.equippedExtraSkill;
          if (equippedExtraSkill != null)
            this.StartCoroutine(this.LoadExtraSkillIcon(this.dir_Replacement_Skill_have[index], equippedExtraSkill));
          else if (this.playerUnit.can_equip_awake_skill)
            this.SetExtraSkillIconNoneSprite(this.slc_Replacement_Skill_none[index], this.dir_Replacement_Skill_have[index]);
          return true;
        }
      }
      else if (!this.playerUnit.is_guest && this.playerUnit.is_enemy)
        flag = true;
    }
    if (flag)
    {
      PlayerUnitSkills skill = ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.awake_skill_category_id != 1));
      if (skill != null && !this.setSkills.Contains(skill.skill.ID))
      {
        this.createBattleSkillIcon(this.dir_Replacement_Skill_have[index], skill);
        return true;
      }
    }
    this.dir_Replacement_Skill_have[index].gameObject.transform.parent = (Transform) null;
    this.dir_Replacement_Skill_have[index].gameObject.SetActive(false);
    ((Component) this.slc_Replacement_Skill_none[index]).gameObject.transform.parent = (Transform) null;
    ((Component) this.slc_Replacement_Skill_none[index]).gameObject.SetActive(false);
    Object.Destroy((Object) this.dir_Replacement_Skill_have[index].gameObject);
    Object.Destroy((Object) ((Component) this.slc_Replacement_Skill_none[index]).gameObject);
    return false;
  }

  private bool setMultiSkill()
  {
    int index = 1;
    bool flag = false;
    if (this.playerUnit.skills != null && (!Object.op_Inequality((Object) this.battleManager, (Object) null) || !this.battleManager.isEarth))
    {
      Dictionary<int, UnitSkillEvolution> unitSkillEvolutionDict = ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Where<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit.ID == this.playerUnit.unit.ID)).ToDictionary<UnitSkillEvolution, int>((Func<UnitSkillEvolution, int>) (x => x.after_skill.ID));
      UnitSkillHarmonyQuest[] array1 = ((IEnumerable<UnitSkillHarmonyQuest>) MasterData.UnitSkillHarmonyQuestList).Where<UnitSkillHarmonyQuest>((Func<UnitSkillHarmonyQuest, bool>) (x => x.character.ID == this.playerUnit.unit.character.ID && x.skill.DispSkillList)).ToArray<UnitSkillHarmonyQuest>();
      if (array1.Length != 0)
      {
        List<PlayerUnitSkills> list = ((IEnumerable<UnitSkillHarmonyQuest>) array1).Select<UnitSkillHarmonyQuest, PlayerUnitSkills>((Func<UnitSkillHarmonyQuest, PlayerUnitSkills>) (s => ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (fd =>
        {
          if (s.skill == fd.skill)
            return true;
          return unitSkillEvolutionDict.ContainsKey(fd.skill.ID) && s.skill == unitSkillEvolutionDict[fd.skill.ID].before_skill;
        })))).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (w => w != null)).Distinct<PlayerUnitSkills>().ToList<PlayerUnitSkills>();
        if (list.Any<PlayerUnitSkills>())
        {
          PlayerUnitSkills skill = list[0];
          if (!this.setSkills.Contains(skill.skill.ID))
          {
            this.createBattleSkillIcon(this.dir_Replacement_Skill_have[index], skill);
            flag = true;
          }
        }
      }
      if (!flag)
      {
        UnitSkillIntimate[] array2 = ((IEnumerable<UnitSkillIntimate>) MasterData.UnitSkillIntimateList).Where<UnitSkillIntimate>((Func<UnitSkillIntimate, bool>) (x => x.unit.ID == this.playerUnit.unit.ID && x.skill.DispSkillList)).ToArray<UnitSkillIntimate>();
        if (array2.Length != 0)
        {
          List<PlayerUnitSkills> list = ((IEnumerable<UnitSkillIntimate>) array2).Select<UnitSkillIntimate, PlayerUnitSkills>((Func<UnitSkillIntimate, PlayerUnitSkills>) (s => ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (fd =>
          {
            if (s.skill == fd.skill)
              return true;
            return unitSkillEvolutionDict.ContainsKey(fd.skill.ID) && s.skill == unitSkillEvolutionDict[fd.skill.ID].before_skill;
          })))).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (w => w != null)).Distinct<PlayerUnitSkills>().ToList<PlayerUnitSkills>();
          if (list.Any<PlayerUnitSkills>())
          {
            PlayerUnitSkills skill = list[0];
            if (!this.setSkills.Contains(skill.skill.ID))
            {
              this.createBattleSkillIcon(this.dir_Replacement_Skill_have[index], skill);
              flag = true;
            }
          }
        }
      }
    }
    if (!flag)
    {
      this.dir_Replacement_Skill_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_Replacement_Skill_have[index].gameObject.SetActive(false);
      ((Component) this.slc_Replacement_Skill_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_Replacement_Skill_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Replacement_Skill_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_Replacement_Skill_none[index]).gameObject);
    }
    return flag;
  }

  private bool setReplacementSkillRow()
  {
    for (int index = 2; index < this.dir_Replacement_Skill_have.Length; ++index)
    {
      this.dir_Replacement_Skill_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_Replacement_Skill_have[index].gameObject.SetActive(false);
      ((Component) this.slc_Replacement_Skill_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_Replacement_Skill_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Replacement_Skill_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_Replacement_Skill_none[index]).gameObject);
    }
    int num = this.setReplacementSkill() | this.setMultiSkill() ? 1 : 0;
    this.grid_Replacement_Skil_none.Reposition();
    this.grid_Replacement_Skil.Reposition();
    if (num == 0)
    {
      this.dir_Replacement_Skill.gameObject.transform.parent = (Transform) null;
      this.dir_Replacement_Skill.gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Replacement_Skill.gameObject);
      return num != 0;
    }
    this.bottomLineSpr = this.slc_line_Replacement_Skil;
    return num != 0;
  }

  private bool setUnitSkillRow()
  {
    int num = 0;
    this.grid_Unit_Skil_none.Reposition();
    this.grid_Unit_Skil.Reposition();
    if (num == 0)
    {
      this.dir_Unit_Skill.gameObject.transform.parent = (Transform) null;
      this.dir_Unit_Skill.gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Unit_Skill.gameObject);
      return num != 0;
    }
    this.bottomLineSpr = this.slc_line_Unit_Skil;
    return num != 0;
  }

  private bool setLSSkill()
  {
    bool flag = false;
    int index = 0;
    PlayerUnitLeader_skills in_skill = (PlayerUnitLeader_skills) null;
    if (this.playerUnit.is_enemy && (this.playerUnit.is_enemy_leader || this.is_leader))
    {
      if (this.playerUnit.leader_skill != null)
      {
        in_skill = this.playerUnit.leader_skill;
        flag = true;
      }
    }
    else if (!this.playerUnit.is_enemy && this.playerUnit.leader_skill != null && (this.is_leader || this.is_friend))
    {
      in_skill = this.playerUnit.leader_skill;
      flag = true;
    }
    if (flag)
    {
      this.StartCoroutine(this.LoadLSSkillIcon(this.dir_LS_Skill_have[index], in_skill));
    }
    else
    {
      this.dir_LS_Skill_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_LS_Skill_have[index].gameObject.SetActive(false);
      ((Component) this.slc_LS_Skill_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_LS_Skill_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_LS_Skill_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_LS_Skill_none[index]).gameObject);
    }
    return flag;
  }

  private bool setElementSkill()
  {
    bool flag = false;
    int index1 = 1;
    for (int index2 = 0; index2 < this.allSkills.Length; ++index2)
    {
      if (!this.setSkills.Contains(this.allSkills[index2].skill.ID) && BattleskillSkill.InvestElementSkillIds.Contains(this.allSkills[index2].skill_BattleskillSkill))
      {
        BattleskillSkill skill = this.playerUnit.evolutionSkill(this.allSkills[index2].skill);
        PlayerUnitSkills skill1 = ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill_id == skill.ID));
        if (skill1 != null)
          this.createBattleSkillIcon(this.dir_LS_Skill_have[index1], skill1);
        else
          this.createBattleSkillIcon(this.dir_LS_Skill_have[index1], this.allSkills[index2], false);
        flag = true;
        break;
      }
    }
    if (!flag)
    {
      this.dir_LS_Skill_have[index1].gameObject.transform.parent = (Transform) null;
      this.dir_LS_Skill_have[index1].gameObject.SetActive(false);
      ((Component) this.slc_LS_Skill_none[index1]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_LS_Skill_none[index1]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_LS_Skill_have[index1].gameObject);
      Object.Destroy((Object) ((Component) this.slc_LS_Skill_none[index1]).gameObject);
    }
    return flag;
  }

  private bool setGrowthSkill()
  {
    bool flag = false;
    int index = 2;
    UnitSkill skill1 = ((IEnumerable<UnitSkill>) this.allSkills).FirstOrDefault<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.growth));
    if (skill1 != null && !this.setSkills.Contains(skill1.skill.ID))
    {
      BattleskillSkill skill = this.playerUnit.evolutionSkill(skill1.skill);
      PlayerUnitSkills skill2 = ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill_id == skill.ID));
      if (skill2 != null)
        this.createBattleSkillIcon(this.dir_LS_Skill_have[index], skill2);
      else
        this.createBattleSkillIcon(this.dir_LS_Skill_have[index], skill1, false);
      flag = true;
    }
    if (!flag)
    {
      this.dir_LS_Skill_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_LS_Skill_have[index].gameObject.SetActive(false);
      ((Component) this.slc_LS_Skill_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_LS_Skill_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_LS_Skill_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_LS_Skill_none[index]).gameObject);
    }
    return flag;
  }

  private bool setLSSkillRow()
  {
    int num = this.setLSSkill() | this.setElementSkill() | this.setGrowthSkill() ? 1 : 0;
    this.grid_LS_Skil_none.Reposition();
    this.grid_LS_Skil.Reposition();
    if (num == 0)
    {
      this.dir_LS_Skill.gameObject.transform.parent = (Transform) null;
      this.dir_LS_Skill.gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_LS_Skill.gameObject);
      return num != 0;
    }
    this.bottomLineSpr = this.slc_line_LS_Skil;
    return num != 0;
  }

  private bool setDuelSkill()
  {
    bool flag = false;
    int num = 0;
    UnitSkill[] array = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.duel)).ToArray<UnitSkill>();
    for (int index = 0; num < this.dir_Duel_Skill_have.Length && index < array.Length; ++index)
    {
      if (!this.setSkills.Contains(array[index].skill.ID))
      {
        BattleskillSkill skill = this.playerUnit.evolutionSkill(array[index].skill);
        PlayerUnitSkills skill1 = ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill_id == skill.ID));
        if (skill1 != null)
          this.createBattleSkillIcon(this.dir_Duel_Skill_have[num++], skill1);
        else
          this.createBattleSkillIcon(this.dir_Duel_Skill_have[num++], array[index], false);
        flag = true;
      }
    }
    for (int index = num; index < this.dir_Duel_Skill_have.Length; ++index)
    {
      this.dir_Duel_Skill_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_Duel_Skill_have[index].gameObject.SetActive(false);
      ((Component) this.slc_Duel_Skill_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_Duel_Skill_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Duel_Skill_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_Duel_Skill_none[index]).gameObject);
    }
    return flag;
  }

  private bool setDuelSkillRow()
  {
    int num = this.setDuelSkill() ? 1 : 0;
    this.grid_Duel_Skil_none.Reposition();
    this.grid_Duel_Skil.Reposition();
    if (num == 0)
    {
      this.dir_Duel_Skill.gameObject.transform.parent = (Transform) null;
      this.dir_Duel_Skill.gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Duel_Skill.gameObject);
      return num != 0;
    }
    this.bottomLineSpr = this.slc_line_Duel_Skil;
    return num != 0;
  }

  private bool setReleaseSkill()
  {
    bool flag = false;
    int index = 0;
    UnitSkill skill1 = ((IEnumerable<UnitSkill>) this.allSkills).FirstOrDefault<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.release));
    if (skill1 != null && !this.setSkills.Contains(skill1.skill.ID))
    {
      BattleskillSkill skill = this.playerUnit.evolutionSkill(skill1.skill);
      PlayerUnitSkills skill2 = ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill_id == skill.ID));
      if (skill2 != null)
        this.createBattleSkillIcon(this.dir_Command_Skill_have[index], skill2);
      else
        this.createBattleSkillIcon(this.dir_Command_Skill_have[index], skill1, false);
      flag = true;
    }
    if (!flag)
    {
      this.dir_Command_Skill_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_Command_Skill_have[index].gameObject.SetActive(false);
      ((Component) this.slc_Command_Skill_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_Command_Skill_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Command_Skill_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_Command_Skill_none[index]).gameObject);
    }
    return flag;
  }

  private bool setCommandSkill()
  {
    bool flag = false;
    int num1 = 1;
    int num2 = num1;
    UnitSkill[] array = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.command)).ToArray<UnitSkill>();
    for (int index = 0; num2 - num1 < this.dir_Command_Skill_have.Length && index < array.Length; ++index)
    {
      if (!this.setSkills.Contains(array[index].skill.ID))
      {
        BattleskillSkill skill = this.playerUnit.evolutionSkill(array[index].skill);
        PlayerUnitSkills skill1 = ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill_id == skill.ID));
        if (skill1 != null)
          this.createBattleSkillIcon(this.dir_Command_Skill_have[num2++], skill1);
        else
          this.createBattleSkillIcon(this.dir_Command_Skill_have[num2++], array[index], false);
        flag = true;
      }
    }
    for (int index = num2; index < this.dir_Command_Skill_have.Length; ++index)
    {
      this.dir_Command_Skill_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_Command_Skill_have[index].gameObject.SetActive(false);
      ((Component) this.slc_Command_Skill_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_Command_Skill_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Command_Skill_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_Command_Skill_none[index]).gameObject);
    }
    return flag;
  }

  private bool setCommandSkillRow()
  {
    int num = this.setReleaseSkill() | this.setCommandSkill() ? 1 : 0;
    this.grid_Command_Skil_none.Reposition();
    this.grid_Command_Skil.Reposition();
    if (num == 0)
    {
      this.dir_Command_Skill.gameObject.transform.parent = (Transform) null;
      this.dir_Command_Skill.gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Command_Skill.gameObject);
      return num != 0;
    }
    this.bottomLineSpr = this.slc_line_Command_Skil;
    return num != 0;
  }

  private bool setGrantSkill()
  {
    bool flag = false;
    int num = 0;
    UnitSkill[] array = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.passive)).ToArray<UnitSkill>();
    for (int index = 0; num < this.dir_Grant_Skill_have.Length && index < array.Length; ++index)
    {
      if (!this.setSkills.Contains(array[index].skill.ID))
      {
        BattleskillSkill skill = this.playerUnit.evolutionSkill(array[index].skill);
        PlayerUnitSkills skill1 = ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill_id == skill.ID));
        if (skill1 != null)
          this.createBattleSkillIcon(this.dir_Grant_Skill_have[num++], skill1);
        else
          this.createBattleSkillIcon(this.dir_Grant_Skill_have[num++], array[index], false);
        flag = true;
      }
    }
    if (array.Length > this.dir_Grant_Skill_have.Length)
      ((Component) this.slc_line_Grant_Skil).gameObject.SetActive(false);
    for (int index = num; index < this.dir_Grant_Skill_have.Length; ++index)
    {
      this.dir_Grant_Skill_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_Grant_Skill_have[index].gameObject.SetActive(false);
      ((Component) this.slc_Grant_Skill_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_Grant_Skill_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Grant_Skill_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_Grant_Skill_none[index]).gameObject);
    }
    return flag;
  }

  private bool setGrantSkill02()
  {
    bool flag = false;
    int num = 0;
    UnitSkill[] array = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.passive)).ToArray<UnitSkill>();
    for (int index = 0; num < this.dir_Grant_Skill_02_have.Length && index < array.Length; ++index)
    {
      if (!this.setSkills.Contains(array[index].skill.ID))
      {
        BattleskillSkill skill = this.playerUnit.evolutionSkill(array[index].skill);
        PlayerUnitSkills skill1 = ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill_id == skill.ID));
        if (skill1 != null)
          this.createBattleSkillIcon(this.dir_Grant_Skill_02_have[num++], skill1);
        else
          this.createBattleSkillIcon(this.dir_Grant_Skill_02_have[num++], array[index], false);
        flag = true;
      }
    }
    for (int index = num; index < this.dir_Grant_Skill_02_have.Length; ++index)
    {
      this.dir_Grant_Skill_02_have[index].gameObject.transform.parent = (Transform) null;
      this.dir_Grant_Skill_02_have[index].gameObject.SetActive(false);
      ((Component) this.slc_Grant_Skill_02_none[index]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_Grant_Skill_02_none[index]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Grant_Skill_02_have[index].gameObject);
      Object.Destroy((Object) ((Component) this.slc_Grant_Skill_02_none[index]).gameObject);
    }
    return flag;
  }

  private bool setGrantSkillRow()
  {
    int num = this.setGrantSkill() ? 1 : 0;
    this.grid_Grant_Skil_none.Reposition();
    this.grid_Grant_Skil.Reposition();
    if (num == 0)
    {
      this.dir_Grant_Skill.gameObject.transform.parent = (Transform) null;
      this.dir_Grant_Skill.gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Grant_Skill.gameObject);
      return num != 0;
    }
    this.bottomLineSpr = this.slc_line_Grant_Skil;
    return num != 0;
  }

  private bool setGrantSkill02Row()
  {
    int num = this.setGrantSkill02() ? 1 : 0;
    this.grid_Grant_Skil_02_none.Reposition();
    this.grid_Grant_Skil_02.Reposition();
    if (num == 0)
    {
      this.dir_Grant_Skill_02.gameObject.transform.parent = (Transform) null;
      this.dir_Grant_Skill_02.gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Grant_Skill_02.gameObject);
      return num != 0;
    }
    this.bottomLineSpr = this.slc_line_Grant_Skil_02;
    return num != 0;
  }

  private bool setArmorSkill()
  {
    bool flag = false;
    int index1 = 0;
    int num = 4;
    if (this.playerUnit.equippedGear != (PlayerItem) null)
    {
      for (int index2 = 0; index2 < this.playerUnit.equippedGear.skills.Length && index1 < num; ++index2)
      {
        if (!this.setSkills.Contains(this.playerUnit.equippedGear.skills[index2].skill.ID))
        {
          this.createBattleSkillIcon(this.dir_Armor_Skill_have[index1], this.playerUnit.equippedGear.skills[index2]);
          ++index1;
          flag = true;
        }
      }
    }
    PlayerItem equippedGear2 = this.playerUnit.equippedGear2;
    if (equippedGear2 != (PlayerItem) null)
    {
      for (int index3 = 0; index3 < equippedGear2.skills.Length && index1 < num; ++index3)
      {
        if (!this.setSkills.Contains(equippedGear2.skills[index3].skill.ID))
        {
          this.createBattleSkillIcon(this.dir_Armor_Skill_have[index1], equippedGear2.skills[index3]);
          ++index1;
          flag = true;
        }
      }
    }
    PlayerItem equippedGear3 = this.playerUnit.equippedGear3;
    if (equippedGear3 != (PlayerItem) null)
    {
      for (int index4 = 0; index4 < equippedGear3.skills.Length && index1 < num; ++index4)
      {
        if (!this.setSkills.Contains(equippedGear3.skills[index4].skill.ID))
        {
          this.createBattleSkillIcon(this.dir_Armor_Skill_have[index1], equippedGear3.skills[index4]);
          ++index1;
          flag = true;
        }
      }
    }
    for (int index5 = index1; index5 < num; ++index5)
    {
      this.dir_Armor_Skill_have[index5].gameObject.transform.parent = (Transform) null;
      this.dir_Armor_Skill_have[index5].gameObject.SetActive(false);
      ((Component) this.slc_Armor_Skill_none[index5]).gameObject.transform.parent = (Transform) null;
      ((Component) this.slc_Armor_Skill_none[index5]).gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Armor_Skill_have[index5].gameObject);
      Object.Destroy((Object) ((Component) this.slc_Armor_Skill_none[index5]).gameObject);
    }
    return flag;
  }

  private bool setArmorSkillRow()
  {
    int num = this.setArmorSkill() ? 1 : 0;
    this.grid_Armor_Skil_none.Reposition();
    this.grid_Armor_Skil.Reposition();
    if (num == 0)
    {
      this.dir_Armor_Skill.gameObject.transform.parent = (Transform) null;
      this.dir_Armor_Skill.gameObject.SetActive(false);
      Object.Destroy((Object) this.dir_Armor_Skill.gameObject);
      return num != 0;
    }
    this.bottomLineSpr = (UISprite) null;
    return num != 0;
  }

  public void setData(BL.BattleModified<BL.Unit> in_modified)
  {
    this.modified = in_modified;
    this.playerUnit = this.modified.value.playerUnit;
    this.is_leader = this.modified.value.is_leader;
    this.is_friend = this.modified.value.friend;
    this.skillDialogChangePrefab = (GameObject) null;
    this.skillDialogChange = (GameObject) null;
  }

  public void setData(PlayerUnit in_playerUnit, GameObject prefab = null)
  {
    this.playerUnit = in_playerUnit;
    this.is_leader = true;
    this.is_friend = true;
    this.skillDialogChangePrefab = prefab;
    this.skillDialogChange = (GameObject) null;
  }

  protected override IEnumerator Start_Battle()
  {
    this.dir_View_check.SetActive(false);
    if (Object.op_Inequality((Object) this.skillDialogChangePrefab, (Object) null))
    {
      List<int> intList = new List<int>();
      int num = 0;
      if (this.playerUnit.skills.Length != 0)
      {
        for (int index = 0; index < this.playerUnit.skills.Length; ++index)
        {
          UnitSkill unitSkill = new UnitSkill();
          unitSkill.ID = 0;
          unitSkill.unit_UnitUnit = this.playerUnit._unit;
          unitSkill.level = this.playerUnit.skills[index].level;
          unitSkill.skill_BattleskillSkill = this.playerUnit.skills[index].skill_id;
          unitSkill.unit_type = this.playerUnit._unit_type;
          Array.Resize<UnitSkill>(ref this.allSkills, num + 1);
          this.allSkills[num++] = unitSkill;
          intList.Add(this.playerUnit.skills[index].skill_id);
        }
        UnitSkill[] array = ((IEnumerable<UnitSkill>) this.playerUnit.unit.RememberUnitSkills(this.playerUnit._unit_type)).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.DispSkillList)).ToArray<UnitSkill>();
        for (int index = 0; index < array.Length; ++index)
        {
          if (!intList.Contains(array[index].skill_BattleskillSkill))
          {
            Array.Resize<UnitSkill>(ref this.allSkills, num + 1);
            this.allSkills[num++] = array[index];
            intList.Add(array[index].skill_BattleskillSkill);
          }
        }
        this.allSkills = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.DispSkillList)).ToArray<UnitSkill>();
      }
    }
    else
    {
      List<int> intList = new List<int>();
      int num = 0;
      if (this.playerUnit.skills.Length != 0)
      {
        for (int index = 0; index < this.playerUnit.skills.Length; ++index)
        {
          UnitSkill unitSkill = new UnitSkill();
          unitSkill.ID = 0;
          unitSkill.unit_UnitUnit = this.playerUnit._unit;
          unitSkill.level = this.playerUnit.skills[index].level;
          unitSkill.skill_BattleskillSkill = this.playerUnit.skills[index].skill_id;
          unitSkill.unit_type = this.playerUnit._unit_type;
          Array.Resize<UnitSkill>(ref this.allSkills, num + 1);
          this.allSkills[num++] = unitSkill;
          intList.Add(this.playerUnit.skills[index].skill_id);
        }
      }
      UnitSkill[] skills = ((IEnumerable<UnitSkill>) this.playerUnit.unit.RememberUnitSkills(this.playerUnit._unit_type)).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.DispSkillList)).ToArray<UnitSkill>();
      if (skills.Length != 0 && !this.playerUnit.is_enemy && !this.is_friend && !this.playerUnit.is_guest)
      {
        for (int i = 0; i < skills.Length; i++)
        {
          if (!intList.Contains(skills[i].skill.ID) && ((IEnumerable<PlayerUnitSkills>) this.playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill_id == skills[i].skill.ID)) != null && !((IEnumerable<UnitSkill>) this.allSkills).Contains<UnitSkill>(skills[i]))
          {
            Array.Resize<UnitSkill>(ref this.allSkills, num + 1);
            this.allSkills[num++] = skills[i];
            intList.Add(skills[i].skill.ID);
          }
        }
      }
      this.allSkills = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.DispSkillList)).ToArray<UnitSkill>();
    }
    if (this.allSkills.Length != 0)
      this.allSkills = ((IEnumerable<UnitSkill>) this.allSkills).OrderBy<UnitSkill, int>((Func<UnitSkill, int>) (x => x.skill_BattleskillSkill)).ToArray<UnitSkill>();
    Future<GameObject> f = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillTypeIconPrefab = f.Result;
    this.skillIconDepth = ((UIWidget) this.dir_Replacement_Skill.GetComponentInChildren<UISprite>()).depth + 100;
    Future<GameObject> skillDialogPrefabF = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
    e = skillDialogPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillDialogPrefab = skillDialogPrefabF.Result;
    this.skillDialog = this.skillDialogPrefab.Clone(this.dyn_skillDetailDialog.transform);
    this.skillDialog.GetComponentInChildren<UIPanel>().depth += 30;
    this.skillDialog.SetActive(false);
    skillDialogPrefabF = (Future<GameObject>) null;
    if (Object.op_Inequality((Object) this.skillDialogChangePrefab, (Object) null))
    {
      this.skillDialogChange = this.skillDialogChangePrefab.Clone(this.dyn_skillDetailDialog.transform);
      this.skillDialogChange.GetComponentInChildren<UIPanel>().depth += 30;
      this.skillDialogChange.SetActive(false);
    }
    this.SetSkillIconBaseSprite();
    this.bottomLineSpr = (UISprite) null;
    this.setReplacementSkillRow();
    this.setUnitSkillRow();
    this.setLSSkillRow();
    this.setDuelSkillRow();
    this.setCommandSkillRow();
    this.setGrantSkillRow();
    this.setGrantSkill02Row();
    this.setArmorSkillRow();
    if (Object.op_Inequality((Object) this.bottomLineSpr, (Object) null))
      ((Component) this.bottomLineSpr).gameObject.SetActive(false);
    this.scrollGrid.Reposition();
    int num1 = this.scrollGrid.maxPerLine;
    if (num1 < 1)
      num1 = 1;
    int num2 = ((Component) this.scrollGrid).transform.childCount;
    if (num2 > 7)
      num2 = 7;
    this.Anchor_height.height = (int) ((double) this.scrollGrid.cellHeight * (double) num2 / (double) num1);
    yield return (object) null;
    this.scrollView.ResetPosition();
    this.dir_View_check.SetActive(true);
  }

  public IEnumerator InitNotBattle()
  {
    IEnumerator e = this.Start_Battle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void createBattleSkillIcon(GameObject parent, BattleskillSkill skill)
  {
    GameObject gameObject = this.skillTypeIconPrefab.Clone();
    ((UIWidget) gameObject.GetComponentInChildren<UI2DSprite>()).depth = ((UIWidget) ((Component) parent.transform).GetComponentInChildren<UI2DSprite>()).depth;
    gameObject.gameObject.SetParent(parent);
    this.StartCoroutine(gameObject.GetComponentInChildren<BattleSkillIcon>().Init(skill));
    this.setSkills.Add(skill.ID);
  }

  private void createBattleSkillIcon(GameObject parent, PlayerUnitSkills skill)
  {
    this.createBattleSkillIcon(parent, skill.skill);
    this.setIconEvent(parent, skill.skill, skill.level);
  }

  private void createBattleSkillIcon(GameObject parent, GearGearSkill skill)
  {
    this.createBattleSkillIcon(parent, skill.skill);
    this.setIconEvent(parent, skill.skill, skill.skill_level);
  }

  private void createBattleSkillIcon(GameObject parent, BattleskillSkill skill, int level)
  {
    this.createBattleSkillIcon(parent, skill);
    this.setIconEvent(parent, skill, level);
  }

  private void createBattleSkillIcon(GameObject parent, UnitSkill skill, bool isLearn)
  {
    if (isLearn)
    {
      this.createBattleSkillIcon(parent, skill.skill);
      this.setIconEvent(parent, skill.skill, skill.level);
    }
    else
    {
      this.createBattleSkillIconNL(parent, skill.skill, skill.level);
      this.setIconEvent(parent, skill.skill, 0);
    }
  }

  private void createBattleSkillIconNL(GameObject parent, BattleskillSkill skill, int level)
  {
    GameObject gameObject = this.skillTypeIconPrefab.Clone();
    ((UIWidget) gameObject.GetComponentInChildren<UI2DSprite>()).depth = ((UIWidget) ((Component) parent.transform).GetComponentInChildren<UI2DSprite>()).depth;
    gameObject.gameObject.SetParent(parent);
    gameObject.GetComponentInChildren<BattleSkillIcon>().EnableNeedLvIcon(level);
    this.StartCoroutine(gameObject.GetComponentInChildren<BattleSkillIcon>().Init(skill));
    this.setSkills.Add(skill.ID);
  }

  private IEnumerator LoadExtraSkillIcon(GameObject parent, PlayerAwakeSkill awakeSkill)
  {
    UI2DSprite extraSKill = ((Component) parent.transform).GetComponentInChildren<UI2DSprite>();
    Future<Sprite> spriteF = awakeSkill.masterData.LoadBattleSkillIcon();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    extraSKill.sprite2D = spriteF.Result;
    this.setSkills.Add(awakeSkill.skill_id);
    this.setIconEvent_Change(parent, awakeSkill.masterData, awakeSkill.level);
  }

  private void SetExtraSkillIconNoneSprite(UISprite spr, GameObject parent)
  {
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      string str = this.playerUnit.unit.canUseAllGearHackSkill ? "slc_skill_icon_base_unit_special-unit_60_62.png__GUI__common__common_prefab" : "slc_extraskill_icon_base_60_62.png__GUI__common__common_prefab";
      ((Component) spr).GetComponent<UISprite>().spriteName = str;
    }
    if (!Object.op_Inequality((Object) this.skillDialogChange, (Object) null))
      return;
    EventDelegate.Add(parent.GetComponentInChildren<UIButton>().onClick, (EventDelegate.Callback) (() => this.changeEquipListScene()));
  }

  private IEnumerator LoadLSSkillIcon(GameObject parent, BattleskillSkill in_skill)
  {
    BattleFuncs.InvestSkill skill = new BattleFuncs.InvestSkill();
    skill.skill = in_skill;
    skill.isEnemyIcon = this.playerUnit.is_enemy;
    UI2DSprite LSSKill = ((Component) parent.transform).GetComponentInChildren<UI2DSprite>();
    Future<Sprite> spriteF = skill.skill.LoadBattleSkillIcon(skill);
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    LSSKill.sprite2D = spriteF.Result;
    this.setSkills.Add(skill.skill.ID);
  }

  private IEnumerator LoadLSSkillIcon(GameObject parent, PlayerUnitLeader_skills in_skill)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    BattleUI01SkillList battleUi01SkillList = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      battleUi01SkillList.setIconEvent(parent, in_skill.skill, in_skill.level);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) battleUi01SkillList.StartCoroutine(battleUi01SkillList.LoadLSSkillIcon(parent, in_skill.skill));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private void SetSkillIconBaseSprite()
  {
    if (Object.op_Equality((Object) this.battleManager, (Object) null) || this.battleManager.isSea)
      return;
    string name = "slc_skill_icon_base_unit_60_62.png__GUI__common__common_prefab";
    if (this.battleManager.isEarth)
      name = "slc_skill_icon_base_unit_zero_60_62.png__GUI__common__common_prefab";
    ((IEnumerable<UISprite>) this.slc_Unit_Skill_none).ForEach<UISprite>((Action<UISprite>) (f => f.spriteName = name));
    ((IEnumerable<UISprite>) this.slc_LS_Skill_none).ForEach<UISprite>((Action<UISprite>) (f => f.spriteName = name));
    ((IEnumerable<UISprite>) this.slc_Duel_Skill_none).ForEach<UISprite>((Action<UISprite>) (f => f.spriteName = name));
    ((IEnumerable<UISprite>) this.slc_Command_Skill_none).ForEach<UISprite>((Action<UISprite>) (f => f.spriteName = name));
    ((IEnumerable<UISprite>) this.slc_Grant_Skill_none).ForEach<UISprite>((Action<UISprite>) (f => f.spriteName = name));
  }

  public void onButtonClose() => this.IbtnNo();

  public void IbtnNo()
  {
    if (Object.op_Inequality((Object) this.battleManager, (Object) null))
      this.battleManager.popupDismiss();
    else
      Singleton<PopupManager>.GetInstance().dismiss();
  }

  private void setIconEvent(GameObject obj, BattleskillSkill skill, int level)
  {
    EventDelegate.Add(obj.GetComponentInChildren<UIButton>().onClick, (EventDelegate.Callback) (() => this.onButtonIcon(skill, level)));
  }

  public void onButtonIcon(BattleskillSkill skill, int level)
  {
    if (Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.setSkillProperty(true);
    componentInChildren.setData(skill, "");
    componentInChildren.setSkillLv(level, skill.upper_level);
    componentInChildren.Show();
  }

  private void setIconEvent_Change(GameObject obj, BattleskillSkill skill, int level)
  {
    UIButton componentInChildren = obj.GetComponentInChildren<UIButton>();
    if (Object.op_Inequality((Object) this.skillDialogChange, (Object) null))
      EventDelegate.Add(componentInChildren.onClick, (EventDelegate.Callback) (() => this.onButtonIcon_Change(skill, level)));
    else
      EventDelegate.Add(componentInChildren.onClick, (EventDelegate.Callback) (() => this.onButtonIcon(skill, level)));
  }

  public void onButtonIcon_Change(BattleskillSkill skill, int level)
  {
    if (Object.op_Equality((Object) this.skillDialogChange, (Object) null))
      return;
    this.skillDialogChange.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialogChange.GetComponentInChildren<Battle0171111Event>();
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.setTargetUnit(this.playerUnit);
    componentInChildren.setSkillProperty(true);
    componentInChildren.setData(skill, "");
    componentInChildren.setSkillLv(level, skill.upper_level);
    componentInChildren.Show();
  }

  private void changeEquipListScene()
  {
    this.IbtnNo();
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
    Unit004ExtraskillEquipListScene.changeScene(true, this.playerUnit);
  }

  public UIButton getCloseButton() => this.ibtn_Popup_close;
}
