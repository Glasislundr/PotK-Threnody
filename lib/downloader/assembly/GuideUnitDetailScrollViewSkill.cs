// Decompiled with JetBrains decompiler
// Type: GuideUnitDetailScrollViewSkill
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
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class GuideUnitDetailScrollViewSkill : MonoBehaviour
{
  [SerializeField]
  protected UISprite slc_Leader_Skill_none;
  [SerializeField]
  protected GameObject dir_Leader_Skill_have;
  [SerializeField]
  protected UISprite slc_Attribute_Skill_none;
  [SerializeField]
  protected GameObject dir_Attribute_Skill_have;
  [SerializeField]
  protected UISprite slc_Increase_Skill_none;
  [SerializeField]
  protected GameObject dir_Increase_Skill_have;
  [SerializeField]
  protected UISprite slc_Multi_Skill_none;
  [SerializeField]
  protected GameObject dir_Multi_Skill_have;
  [SerializeField]
  protected UISprite[] slc_Command_Skill_none;
  [SerializeField]
  protected GameObject[] dir_Command_Skill_have;
  [SerializeField]
  protected UISprite[] slc_Grant_Skill_none;
  [SerializeField]
  protected GameObject[] dir_Grant_Skill_have;
  [SerializeField]
  protected UISprite[] slc_Duel_Skill_none;
  [SerializeField]
  protected GameObject[] dir_Duel_Skill_have;
  [SerializeField]
  protected UISprite slc_Extra_Skill_none;
  [SerializeField]
  protected UISprite slc_Extra_SkillNone_base;
  [SerializeField]
  protected UILabel txt_ExtraSkill;
  [SerializeField]
  protected GameObject dyn_skillDetailDialog;
  protected UnitSkill[] allSkills;
  protected List<int> prencessSkillIDs = new List<int>();
  protected UnitUnit unit;
  protected PlayerUnitHistory history;
  protected GameObject skillTypeIconPrefab;
  private GameObject skillLockIconPrefab;
  protected List<int> setSkills = new List<int>();
  private List<Tuple<BattleskillSkill, bool, GameObject>> lstSkillIcon;
  private GameObject skillDetailPrefab;
  private PopupSkillDetails.Param[] skillParams;

  public PlayerUnitSkills koyuDuel { get; private set; }

  public PlayerUnitSkills koyuMulti { get; private set; }

  public IEnumerator init(UnitUnit unit, PlayerUnitHistory history)
  {
    this.unit = unit;
    this.history = history;
    this.koyuDuel = this.koyuMulti = (PlayerUnitSkills) null;
    this.prencessSkillIDs.Clear();
    foreach (UnitSkill unitSkill in ((IEnumerable<UnitSkill>) unit.RememberUnitAllSkills()).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.unit_type != 0)))
      this.prencessSkillIDs.Add(unitSkill.skill_BattleskillSkill);
    this.allSkills = new UnitSkill[0];
    List<int> intList = new List<int>();
    int num = 0;
    if (history.skill_ids.Length != 0)
    {
      for (int index = 0; index < history.skill_ids.Length; ++index)
      {
        UnitSkill unitSkill = new UnitSkill();
        unitSkill.ID = 0;
        unitSkill.unit_UnitUnit = unit.ID;
        unitSkill.level = 1;
        unitSkill.skill_BattleskillSkill = history.skill_ids[index].Value;
        unitSkill.unit_type = 0;
        Array.Resize<UnitSkill>(ref this.allSkills, num + 1);
        this.allSkills[num++] = unitSkill;
        intList.Add(history.skill_ids[index].Value);
      }
    }
    UnitSkill[] array = ((IEnumerable<UnitSkill>) unit.RememberUnitSkills()).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.DispSkillList)).ToArray<UnitSkill>();
    if (array.Length != 0)
    {
      for (int index = 0; index < array.Length; ++index)
      {
        if (!intList.Contains(array[index].skill.ID) && !((IEnumerable<UnitSkill>) this.allSkills).Contains<UnitSkill>(array[index]))
        {
          Array.Resize<UnitSkill>(ref this.allSkills, num + 1);
          this.allSkills[num++] = array[index];
          intList.Add(array[index].skill.ID);
        }
      }
    }
    this.allSkills = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.DispSkillList)).ToArray<UnitSkill>();
    if (this.allSkills.Length != 0)
      this.allSkills = ((IEnumerable<UnitSkill>) this.allSkills).OrderBy<UnitSkill, int>((Func<UnitSkill, int>) (x => x.skill_BattleskillSkill)).ToArray<UnitSkill>();
    Future<GameObject> f = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillTypeIconPrefab = f.Result;
    Future<GameObject> loader = new ResourceObject("Prefabs/BattleSkillIcon/dir_SkillLock").Load<GameObject>();
    yield return (object) loader.Wait();
    this.skillLockIconPrefab = loader.Result;
    loader = (Future<GameObject>) null;
    loader = PopupSkillDetails.createPrefabLoader(false);
    yield return (object) loader.Wait();
    this.skillDetailPrefab = loader.Result;
    loader = (Future<GameObject>) null;
    this.setSkills.Clear();
    this.lstSkillIcon = new List<Tuple<BattleskillSkill, bool, GameObject>>(10);
    List<PopupSkillDetails.Param> first = this.setLSSkill();
    List<PopupSkillDetails.Param> second1 = this.setElementSkill();
    List<PopupSkillDetails.Param> second2 = this.setGrowthSkill();
    List<PopupSkillDetails.Param> second3 = this.setMultiSkill();
    List<PopupSkillDetails.Param> second4 = this.setCommandSkill();
    List<PopupSkillDetails.Param> second5 = this.setGrantSkill();
    List<PopupSkillDetails.Param> second6 = this.setDuelSkill();
    List<PopupSkillDetails.Param> second7 = this.setExtraSkill();
    this.skillParams = first.Concat<PopupSkillDetails.Param>((IEnumerable<PopupSkillDetails.Param>) second1).Concat<PopupSkillDetails.Param>((IEnumerable<PopupSkillDetails.Param>) second2).Concat<PopupSkillDetails.Param>((IEnumerable<PopupSkillDetails.Param>) second5).Concat<PopupSkillDetails.Param>((IEnumerable<PopupSkillDetails.Param>) second7).Concat<PopupSkillDetails.Param>((IEnumerable<PopupSkillDetails.Param>) second3).Concat<PopupSkillDetails.Param>((IEnumerable<PopupSkillDetails.Param>) second4).Concat<PopupSkillDetails.Param>((IEnumerable<PopupSkillDetails.Param>) second6).ToArray<PopupSkillDetails.Param>();
    PlayerUnitSkills skill = unit.SEASkill?.getSkill(0);
    bool unlockedSeaSkill = history.isUnlockedSEASkill;
    foreach (Tuple<BattleskillSkill, bool, GameObject> tuple in this.lstSkillIcon)
    {
      int? skillId = skill?.skill_id;
      int id = tuple.Item1.ID;
      if (skillId.GetValueOrDefault() == id & skillId.HasValue)
      {
        if (!unlockedSeaSkill)
        {
          UI2DSprite iconSprite = tuple.Item3.GetComponentInChildren<BattleSkillIcon>().iconSprite;
          if (Object.op_Inequality((Object) iconSprite, (Object) null))
            ((UIWidget) iconSprite).color = Color.gray;
        }
        else
          this.skillLockIconPrefab.Clone(tuple.Item3.transform.parent);
      }
      else
      {
        int? transformationGroupId = tuple.Item1.transformationGroupId;
        if (transformationGroupId.HasValue && transformationGroupId.Value != 0)
        {
          GameObject gameObject = this.skillLockIconPrefab.Clone(tuple.Item3.transform.parent);
          if (tuple.Item2)
          {
            UI2DSprite iconSprite = tuple.Item3.GetComponentInChildren<BattleSkillIcon>().iconSprite;
            if (Object.op_Inequality((Object) iconSprite, (Object) null))
              ((UIWidget) iconSprite).color = Color.gray;
          }
          else
            ((UIWidget) gameObject.GetComponentInChildren<UISprite>()).color = Color.gray;
        }
      }
    }
    this.lstSkillIcon = (List<Tuple<BattleskillSkill, bool, GameObject>>) null;
  }

  private List<PopupSkillDetails.Param> setLSSkill()
  {
    List<PopupSkillDetails.Param> objList = new List<PopupSkillDetails.Param>(1);
    List<UnitLeaderSkill> list = ((IEnumerable<UnitLeaderSkill>) MasterData.UnitLeaderSkillList).Where<UnitLeaderSkill>((Func<UnitLeaderSkill, bool>) (x => x.unit_UnitUnit == this.unit.ID)).ToList<UnitLeaderSkill>();
    if (list.Any<UnitLeaderSkill>())
    {
      BattleskillSkill skill = list[0].skill;
      this.StartCoroutine(this.LoadLSSkillIcon(this.dir_Leader_Skill_have, skill));
      objList.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Leader));
      ((Component) this.slc_Leader_Skill_none).gameObject.SetActive(false);
      this.dir_Leader_Skill_have.gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.slc_Leader_Skill_none).gameObject.SetActive(true);
      this.dir_Leader_Skill_have.gameObject.SetActive(false);
    }
    return objList;
  }

  private List<PopupSkillDetails.Param> setElementSkill()
  {
    List<PopupSkillDetails.Param> source = new List<PopupSkillDetails.Param>(1);
    foreach (UnitSkill allSkill in this.allSkills)
    {
      if (!this.setSkills.Contains(allSkill.skill_BattleskillSkill) && BattleskillSkill.InvestElementSkillIds.Contains(allSkill.skill_BattleskillSkill))
      {
        BattleskillSkill evoSkill = this.evolutionSkill(allSkill.skill);
        UnitSkill skill1 = Array.Find<UnitSkill>(this.allSkills, (Predicate<UnitSkill>) (x => x.skill_BattleskillSkill == evoSkill.ID));
        BattleskillSkill skill2 = skill1.skill;
        bool isLearn = ((IEnumerable<int?>) this.history.skill_ids).Contains<int?>(new int?(skill2.ID));
        this.createBattleSkillIcon(this.dir_Attribute_Skill_have, skill1, isLearn);
        source.Add(new PopupSkillDetails.Param(skill2, UnitParameter.SkillGroup.Element));
        break;
      }
    }
    if (!source.Any<PopupSkillDetails.Param>())
    {
      ((Component) this.slc_Attribute_Skill_none).gameObject.SetActive(true);
      this.dir_Attribute_Skill_have.gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.slc_Attribute_Skill_none).gameObject.SetActive(false);
      this.dir_Attribute_Skill_have.gameObject.SetActive(true);
    }
    return source;
  }

  private List<PopupSkillDetails.Param> setGrowthSkill()
  {
    List<PopupSkillDetails.Param> source = new List<PopupSkillDetails.Param>(1);
    foreach (UnitSkill allSkill in this.allSkills)
    {
      BattleskillSkill skill1 = allSkill.skill;
      if (!this.setSkills.Contains(skill1.ID) && skill1.skill_type == BattleskillSkillType.growth)
      {
        BattleskillSkill evoSkill = this.evolutionSkill(skill1);
        UnitSkill skill2 = Array.Find<UnitSkill>(this.allSkills, (Predicate<UnitSkill>) (x => x.skill_BattleskillSkill == evoSkill.ID));
        BattleskillSkill skill3 = skill2.skill;
        bool isLearn = ((IEnumerable<int?>) this.history.skill_ids).Contains<int?>(new int?(skill3.ID));
        this.createBattleSkillIcon(this.dir_Increase_Skill_have, skill2, isLearn);
        source.Add(new PopupSkillDetails.Param(skill3, UnitParameter.SkillGroup.Growth));
        break;
      }
    }
    if (!source.Any<PopupSkillDetails.Param>())
    {
      ((Component) this.slc_Increase_Skill_none).gameObject.SetActive(true);
      this.dir_Increase_Skill_have.gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.slc_Increase_Skill_none).gameObject.SetActive(false);
      this.dir_Increase_Skill_have.gameObject.SetActive(true);
    }
    return source;
  }

  private List<PopupSkillDetails.Param> setMultiSkill()
  {
    List<PopupSkillDetails.Param> source = new List<PopupSkillDetails.Param>(1);
    BattleskillSkill battleskillSkill = (BattleskillSkill) null;
    List<KeyValuePair<int, BattleskillSkill>> historyBattleSkills = MasterData.BattleskillSkill.Where<KeyValuePair<int, BattleskillSkill>>((Func<KeyValuePair<int, BattleskillSkill>, bool>) (x => ((IEnumerable<int?>) this.history.skill_ids).Contains<int?>(new int?(x.Value.ID)))).ToList<KeyValuePair<int, BattleskillSkill>>();
    Dictionary<int, UnitSkillEvolution> unitSkillEvolutionDict = ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Where<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit_UnitUnit == this.unit.ID)).ToDictionary<UnitSkillEvolution, int>((Func<UnitSkillEvolution, int>) (x => x.after_skill_BattleskillSkill));
    UnitSkillHarmonyQuest[] array1 = ((IEnumerable<UnitSkillHarmonyQuest>) MasterData.UnitSkillHarmonyQuestList).Where<UnitSkillHarmonyQuest>((Func<UnitSkillHarmonyQuest, bool>) (x => x.character_UnitCharacter == this.unit.character_UnitCharacter && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillHarmonyQuest>();
    if (array1.Length != 0)
    {
      List<KeyValuePair<int, BattleskillSkill>> list = ((IEnumerable<UnitSkillHarmonyQuest>) array1).Select<UnitSkillHarmonyQuest, KeyValuePair<int, BattleskillSkill>>((Func<UnitSkillHarmonyQuest, KeyValuePair<int, BattleskillSkill>>) (s => historyBattleSkills.FirstOrDefault<KeyValuePair<int, BattleskillSkill>>((Func<KeyValuePair<int, BattleskillSkill>, bool>) (fd =>
      {
        if (s.skill_BattleskillSkill == fd.Value.ID)
          return true;
        return unitSkillEvolutionDict.ContainsKey(fd.Value.ID) && s.skill_BattleskillSkill == unitSkillEvolutionDict[fd.Value.ID].before_skill_BattleskillSkill;
      })))).Where<KeyValuePair<int, BattleskillSkill>>((Func<KeyValuePair<int, BattleskillSkill>, bool>) (w => w.Value != null)).Distinct<KeyValuePair<int, BattleskillSkill>>().ToList<KeyValuePair<int, BattleskillSkill>>();
      if (list.Any<KeyValuePair<int, BattleskillSkill>>())
      {
        KeyValuePair<int, BattleskillSkill> keyValuePair = list.First<KeyValuePair<int, BattleskillSkill>>();
        if (!this.setSkills.Contains(keyValuePair.Value.ID))
          battleskillSkill = keyValuePair.Value;
      }
    }
    if (battleskillSkill == null)
    {
      UnitSkillIntimate[] array2 = ((IEnumerable<UnitSkillIntimate>) MasterData.UnitSkillIntimateList).Where<UnitSkillIntimate>((Func<UnitSkillIntimate, bool>) (x => x.unit_UnitUnit == this.unit.ID && x.skill.DispSkillList)).ToArray<UnitSkillIntimate>();
      if (array2.Length != 0)
      {
        List<KeyValuePair<int, BattleskillSkill>> list = ((IEnumerable<UnitSkillIntimate>) array2).Select<UnitSkillIntimate, KeyValuePair<int, BattleskillSkill>>((Func<UnitSkillIntimate, KeyValuePair<int, BattleskillSkill>>) (s => historyBattleSkills.FirstOrDefault<KeyValuePair<int, BattleskillSkill>>((Func<KeyValuePair<int, BattleskillSkill>, bool>) (fd =>
        {
          if (s.skill == fd.Value)
            return true;
          return unitSkillEvolutionDict.ContainsKey(fd.Value.ID) && s.skill_BattleskillSkill == unitSkillEvolutionDict[fd.Value.ID].before_skill_BattleskillSkill;
        })))).Where<KeyValuePair<int, BattleskillSkill>>((Func<KeyValuePair<int, BattleskillSkill>, bool>) (w => w.Value != null)).Distinct<KeyValuePair<int, BattleskillSkill>>().ToList<KeyValuePair<int, BattleskillSkill>>();
        if (list.Any<KeyValuePair<int, BattleskillSkill>>())
        {
          KeyValuePair<int, BattleskillSkill> keyValuePair = list.First<KeyValuePair<int, BattleskillSkill>>();
          if (!this.setSkills.Contains(keyValuePair.Value.ID))
            battleskillSkill = keyValuePair.Value;
        }
      }
    }
    if (battleskillSkill != null)
    {
      this.createBattleSkillIcon(this.dir_Multi_Skill_have, battleskillSkill);
      this.setIconEvent(this.dir_Multi_Skill_have, battleskillSkill);
      source.Add(new PopupSkillDetails.Param(battleskillSkill, UnitParameter.SkillGroup.Multi));
      if (battleskillSkill.haveKoyuDuelEffect)
      {
        this.koyuMulti = new PlayerUnitSkills();
        this.koyuMulti.skill_id = battleskillSkill.ID;
        this.koyuMulti.level = battleskillSkill.upper_level;
      }
    }
    if (source.Any<PopupSkillDetails.Param>())
    {
      ((Component) this.slc_Multi_Skill_none).gameObject.SetActive(false);
      this.dir_Multi_Skill_have.gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.slc_Multi_Skill_none).gameObject.SetActive(true);
      this.dir_Multi_Skill_have.gameObject.SetActive(false);
    }
    return source;
  }

  private List<PopupSkillDetails.Param> setCommandSkill()
  {
    List<PopupSkillDetails.Param> objList = new List<PopupSkillDetails.Param>(5);
    int index1 = 0;
    ((IEnumerable<UISprite>) this.slc_Command_Skill_none).ForEach<UISprite>((Action<UISprite>) (x => ((Component) x).gameObject.SetActive(true)));
    ((IEnumerable<GameObject>) this.dir_Command_Skill_have).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    UnitSkill[] skills1 = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.release)).ToArray<UnitSkill>();
    for (int i = 0; index1 < this.dir_Command_Skill_have.Length && i < skills.Length; i++)
    {
      if (!this.setSkills.Contains(skills[i].skill_BattleskillSkill) && !this.prencessSkillIDs.Any<int>((Func<int, bool>) (x => x == skills[i].skill_BattleskillSkill)))
      {
        BattleskillSkill evoSkill = this.evolutionSkill(skills[i].skill);
        UnitSkill skill1 = Array.Find<UnitSkill>(this.allSkills, (Predicate<UnitSkill>) (x => x.skill_BattleskillSkill == evoSkill.ID));
        BattleskillSkill skill2 = skill1.skill;
        bool isLearn = ((IEnumerable<int?>) this.history.skill_ids).Contains<int?>(new int?(skill2.ID));
        this.createBattleSkillIcon(this.dir_Command_Skill_have[index1++], skill1, isLearn);
        objList.Add(new PopupSkillDetails.Param(skill2, UnitParameter.SkillGroup.Release));
      }
    }
    UnitSkill[] skills2 = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.command)).ToArray<UnitSkill>();
    for (int i = 0; index1 < this.dir_Command_Skill_have.Length && i < skills.Length; i++)
    {
      if (!this.setSkills.Contains(skills[i].skill_BattleskillSkill) && !this.prencessSkillIDs.Any<int>((Func<int, bool>) (x => x == skills[i].skill_BattleskillSkill)))
      {
        BattleskillSkill evoSkill = this.evolutionSkill(skills[i].skill);
        UnitSkill skill3 = Array.Find<UnitSkill>(this.allSkills, (Predicate<UnitSkill>) (x => x.skill_BattleskillSkill == evoSkill.ID));
        BattleskillSkill skill4 = skill3.skill;
        bool isLearn = ((IEnumerable<int?>) this.history.skill_ids).Contains<int?>(new int?(skill4.ID));
        this.createBattleSkillIcon(this.dir_Command_Skill_have[index1++], skill3, isLearn);
        objList.Add(new PopupSkillDetails.Param(skill4, UnitParameter.SkillGroup.Command));
      }
    }
    if (index1 < this.dir_Command_Skill_have.Length && this.unit.hasSEASkills)
    {
      PlayerUnitSkills skill5 = this.unit.SEASkill.getSkill(0);
      BattleskillSkill skill6 = skill5.skill;
      this.createBattleSkillIcon(this.dir_Command_Skill_have[index1], skill6);
      this.setIconEvent(this.dir_Command_Skill_have[index1++], skill6);
      objList.Add(PopupSkillDetails.Param.createBySEASkillView(skill5, this.history.isUnlockedSEASkill, 0));
    }
    for (int index2 = 0; index2 < index1; ++index2)
    {
      ((Component) this.slc_Command_Skill_none[index2]).gameObject.SetActive(false);
      this.dir_Command_Skill_have[index2].gameObject.SetActive(true);
    }
    return objList;
  }

  private List<PopupSkillDetails.Param> setGrantSkill()
  {
    List<PopupSkillDetails.Param> objList = new List<PopupSkillDetails.Param>(8);
    int num = 0;
    ((IEnumerable<UISprite>) this.slc_Grant_Skill_none).ForEach<UISprite>((Action<UISprite>) (x => ((Component) x).gameObject.SetActive(true)));
    ((IEnumerable<GameObject>) this.dir_Grant_Skill_have).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    UnitSkill[] skills = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.passive)).ToArray<UnitSkill>();
    for (int i = 0; num < this.dir_Grant_Skill_have.Length && i < skills.Length; i++)
    {
      if (!this.setSkills.Contains(skills[i].skill_BattleskillSkill) && !this.prencessSkillIDs.Any<int>((Func<int, bool>) (x => x == skills[i].skill_BattleskillSkill)))
      {
        BattleskillSkill evoSkill = this.evolutionSkill(skills[i].skill);
        UnitSkill skill1 = Array.Find<UnitSkill>(this.allSkills, (Predicate<UnitSkill>) (x => x.skill_BattleskillSkill == evoSkill.ID));
        BattleskillSkill skill2 = skill1.skill;
        bool isLearn = ((IEnumerable<int?>) this.history.skill_ids).Contains<int?>(new int?(skill2.ID));
        this.createBattleSkillIcon(this.dir_Grant_Skill_have[num++], skill1, isLearn);
        objList.Add(new PopupSkillDetails.Param(skill2, UnitParameter.SkillGroup.Grant));
      }
    }
    for (int index = 0; index < num; ++index)
    {
      ((Component) this.slc_Grant_Skill_none[index]).gameObject.SetActive(false);
      this.dir_Grant_Skill_have[index].gameObject.SetActive(true);
    }
    return objList;
  }

  private List<PopupSkillDetails.Param> setDuelSkill()
  {
    List<PopupSkillDetails.Param> objList = new List<PopupSkillDetails.Param>(4);
    int num = 0;
    ((IEnumerable<UISprite>) this.slc_Duel_Skill_none).ForEach<UISprite>((Action<UISprite>) (x => ((Component) x).gameObject.SetActive(true)));
    ((IEnumerable<GameObject>) this.dir_Duel_Skill_have).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    UnitSkill[] skills = ((IEnumerable<UnitSkill>) this.allSkills).Where<UnitSkill>((Func<UnitSkill, bool>) (x => x.skill.skill_type == BattleskillSkillType.duel)).ToArray<UnitSkill>();
    for (int i = 0; num < this.dir_Duel_Skill_have.Length && i < skills.Length; i++)
    {
      if (!this.setSkills.Contains(skills[i].skill_BattleskillSkill) && !this.prencessSkillIDs.Any<int>((Func<int, bool>) (x => x == skills[i].skill_BattleskillSkill)))
      {
        BattleskillSkill evoSkill = this.evolutionSkill(skills[i].skill);
        UnitSkill skill1 = Array.Find<UnitSkill>(this.allSkills, (Predicate<UnitSkill>) (x => x.skill_BattleskillSkill == evoSkill.ID));
        BattleskillSkill skill2 = skill1.skill;
        bool isLearn = ((IEnumerable<int?>) this.history.skill_ids).Contains<int?>(new int?(skill2.ID));
        this.createBattleSkillIcon(this.dir_Duel_Skill_have[num++], skill1, isLearn);
        objList.Add(new PopupSkillDetails.Param(skill2, UnitParameter.SkillGroup.Duel));
        if (this.koyuDuel == null & isLearn && skill2.haveKoyuDuelEffect)
        {
          this.koyuDuel = new PlayerUnitSkills();
          this.koyuDuel.skill_id = skill2.ID;
          this.koyuDuel.level = skill2.upper_level;
        }
      }
    }
    for (int index = 0; index < num; ++index)
    {
      ((Component) this.slc_Duel_Skill_none[index]).gameObject.SetActive(false);
      this.dir_Duel_Skill_have[index].gameObject.SetActive(true);
    }
    return objList;
  }

  private List<PopupSkillDetails.Param> setExtraSkill()
  {
    ((Component) this.slc_Extra_Skill_none).gameObject.SetActive(this.unit.trust_target_flag);
    ((Component) this.slc_Extra_SkillNone_base).gameObject.SetActive(!this.unit.trust_target_flag);
    ((Component) this.txt_ExtraSkill).gameObject.SetActive(this.unit.trust_target_flag);
    return new List<PopupSkillDetails.Param>();
  }

  private void createBattleSkillIcon(GameObject parent, BattleskillSkill skill)
  {
    parent.transform.Clear();
    GameObject gameObject = this.skillTypeIconPrefab.Clone();
    ((UIWidget) gameObject.GetComponentInChildren<UI2DSprite>()).depth = ((UIWidget) ((Component) parent.transform).GetComponentInChildren<UI2DSprite>()).depth;
    gameObject.gameObject.SetParent(parent);
    this.StartCoroutine(gameObject.GetComponentInChildren<BattleSkillIcon>().Init(skill));
    this.setSkills.Add(skill.ID);
    this.lstSkillIcon.Add(Tuple.Create<BattleskillSkill, bool, GameObject>(skill, true, gameObject));
  }

  private void createBattleSkillIcon(GameObject parent, UnitSkill skill, bool isLearn)
  {
    if (isLearn)
    {
      this.createBattleSkillIcon(parent, skill.skill);
      this.setIconEvent(parent, skill.skill);
    }
    else
    {
      this.createBattleSkillIconNL(parent, skill.skill, skill.level);
      this.setIconEvent(parent, skill.skill);
    }
  }

  private void createBattleSkillIconNL(GameObject parent, BattleskillSkill skill, int level)
  {
    parent.transform.Clear();
    GameObject gameObject = this.skillTypeIconPrefab.Clone();
    ((UIWidget) gameObject.GetComponentInChildren<UI2DSprite>()).depth = ((UIWidget) ((Component) parent.transform).GetComponentInChildren<UI2DSprite>()).depth;
    gameObject.gameObject.SetParent(parent);
    gameObject.GetComponentInChildren<BattleSkillIcon>().EnableNeedLvIcon(level);
    this.StartCoroutine(gameObject.GetComponentInChildren<BattleSkillIcon>().Init(skill));
    this.setSkills.Add(skill.ID);
    this.lstSkillIcon.Add(Tuple.Create<BattleskillSkill, bool, GameObject>(skill, false, gameObject));
  }

  private IEnumerator LoadLSSkillIcon(GameObject parent, BattleskillSkill in_skill)
  {
    BattleFuncs.InvestSkill skill = new BattleFuncs.InvestSkill();
    skill.skill = in_skill;
    UI2DSprite LSSKill = ((Component) parent.transform).GetComponentInChildren<UI2DSprite>();
    Future<Sprite> spriteF = skill.skill.LoadBattleSkillIcon(skill);
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    LSSKill.sprite2D = spriteF.Result;
    this.setSkills.Add(skill.skill.ID);
    this.setIconEvent(parent, in_skill);
  }

  private void setIconEvent(GameObject obj, BattleskillSkill skill)
  {
    EventDelegate.Set(obj.GetComponentInChildren<UIButton>().onClick, (EventDelegate.Callback) (() => this.onButtonIcon(skill)));
  }

  public void onButtonIcon(BattleskillSkill skill)
  {
    PopupSkillDetails.Param[] skillParams = this.skillParams;
    int? nullable = skillParams != null ? ((IEnumerable<PopupSkillDetails.Param>) skillParams).FirstIndexOrNull<PopupSkillDetails.Param>((Func<PopupSkillDetails.Param, bool>) (x => x.skill == skill)) : new int?();
    if (!nullable.HasValue)
      return;
    PopupSkillDetails.show(this.skillDetailPrefab, this.skillParams, nullable.Value);
  }

  public BattleskillSkill evolutionSkill(BattleskillSkill skill)
  {
    int?[] skillIds = this.history.skill_ids;
    Dictionary<int, UnitSkillEvolution> dictionary = ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Where<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit_UnitUnit == this.unit.ID)).ToDictionary<UnitSkillEvolution, int>((Func<UnitSkillEvolution, int>) (x => x.before_skill_BattleskillSkill));
    return dictionary.ContainsKey(skill.ID) && ((IEnumerable<int?>) skillIds).Contains<int?>(new int?(dictionary[skill.ID].after_skill_BattleskillSkill)) ? dictionary[skill.ID].after_skill : skill;
  }
}
