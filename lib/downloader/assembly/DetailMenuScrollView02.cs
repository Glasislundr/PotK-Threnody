// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollView02
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
public class DetailMenuScrollView02 : DetailMenuScrollViewBase
{
  [SerializeField]
  private UI2DSprite dynExtraskillEquiped;
  [SerializeField]
  private GameObject slcExtraskillIconNone;
  [SerializeField]
  private GameObject slcExtraskillIconLocked;
  [SerializeField]
  private UIButton dynExtraskillEquipedBtn;
  [SerializeField]
  private GameObject dirExtraskillIcon;
  [SerializeField]
  private GameObject dirExtraskillIconNone;
  [SerializeField]
  private GameObject dirMultiSkillIcon;
  [SerializeField]
  private GameObject dirMultiSkillIconNone;
  [SerializeField]
  protected GameObject[] gearRankObjects;
  [SerializeField]
  protected UILabel txt_Agility;
  [SerializeField]
  protected UILabel txt_Luck;
  [SerializeField]
  protected UILabel txt_Magic;
  [SerializeField]
  protected UILabel txt_Power;
  [SerializeField]
  protected UILabel txt_Protct;
  [SerializeField]
  protected UILabel txt_Spirit;
  [SerializeField]
  protected UILabel txt_Technique;
  [SerializeField]
  protected GameObject scl_AgilityMaxStar;
  [SerializeField]
  protected GameObject scl_LuckMaxStar;
  [SerializeField]
  protected GameObject scl_MagicMaxStar;
  [SerializeField]
  protected GameObject scl_PowerMaxStar;
  [SerializeField]
  protected GameObject scl_ProtctMaxStar;
  [SerializeField]
  protected GameObject scl_SpiritMaxStar;
  [SerializeField]
  protected GameObject scl_TechniqueMaxStar;
  [SerializeField]
  protected UILabel txt_Skillname;
  [SerializeField]
  protected UILabel txt_SkillDescription;
  [SerializeField]
  protected UI2DSprite[] dyn_UnitTypes;
  [SerializeField]
  protected UIButton[] dyn_UnitSkill;
  [SerializeField]
  protected UIWidget[] dyn_WeaponTypes;
  [SerializeField]
  protected UIButton[] dyn_WeaponSkills;
  [SerializeField]
  protected UIWidget[] dyn_SecondWeaponTypes;
  [SerializeField]
  protected UIButton[] dyn_SecondWeaponSkills;
  [SerializeField]
  protected GameObject[] dyn_WeaponNoneIcons;
  [SerializeField]
  protected UI2DSprite[] dyn_Weapon;
  [SerializeField]
  protected UI2DSprite[] dyn_IconRank;
  [SerializeField]
  private GameObject floatingSkillDialog;
  private Action<BattleskillSkill> showSkillDialog;
  private Action<int, int> showSkillLevel;
  [SerializeField]
  private DetailMenu menu;
  private Battle0171111Event floatingSkillDialogObject;
  private GameObject gearPro;
  private GameObject[] weapon;
  private GearProfiencyIcon[] gearProfiencyIcon;
  private BattleSkillIcon[] unitSkillIcon;
  private BattleSkillIcon[] battleSkillIcon;
  private PlayerUnit targetUnit;
  [SerializeField]
  protected UIGrid skillIconGrid;
  [SerializeField]
  protected UIGrid weaponNoneGrid;
  [SerializeField]
  protected GameObject[] slc_skill_icon_none;
  [SerializeField]
  protected GameObject[] dir_Unit_Skill;
  private PlayerAwakeSkill awakeSkill;
  private PlayerUnitSkills awakeSkill_enemy;
  protected List<int> setSkills = new List<int>();

  private void Awake()
  {
    this.weapon = new GameObject[this.dyn_Weapon.Length];
    this.gearProfiencyIcon = new GearProfiencyIcon[this.dyn_IconRank.Length];
    this.unitSkillIcon = new BattleSkillIcon[this.dyn_UnitTypes.Length];
    this.battleSkillIcon = new BattleSkillIcon[this.dyn_WeaponTypes.Length];
  }

  public override bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    this.targetUnit = playerUnit;
    ((Component) this).gameObject.SetActive(true);
    Judgement.NonBattleParameter nonBattleParameter = this.isMemory ? Judgement.NonBattleParameter.FromPlayerUnitMemory(playerUnit) : Judgement.NonBattleParameter.FromPlayerUnit(playerUnit);
    this.setText(this.txt_Power, nonBattleParameter.Strength);
    this.setText(this.txt_Magic, nonBattleParameter.Intelligence);
    this.setText(this.txt_Protct, nonBattleParameter.Vitality);
    this.setText(this.txt_Spirit, nonBattleParameter.Mind);
    this.setText(this.txt_Agility, nonBattleParameter.Agility);
    this.setText(this.txt_Technique, nonBattleParameter.Dexterity);
    this.setText(this.txt_Luck, nonBattleParameter.Luck);
    if (!this.isMemory)
    {
      this.scl_AgilityMaxStar.SetActive(playerUnit.agility.is_max);
      this.scl_LuckMaxStar.SetActive(playerUnit.lucky.is_max);
      this.scl_MagicMaxStar.SetActive(playerUnit.intelligence.is_max);
      this.scl_PowerMaxStar.SetActive(playerUnit.strength.is_max);
      this.scl_ProtctMaxStar.SetActive(playerUnit.vitality.is_max);
      this.scl_SpiritMaxStar.SetActive(playerUnit.mind.is_max);
      this.scl_TechniqueMaxStar.SetActive(playerUnit.dexterity.is_max);
    }
    else
    {
      this.scl_AgilityMaxStar.SetActive(playerUnit.is_memory_agility_max);
      this.scl_LuckMaxStar.SetActive(playerUnit.is_memory_lucky_max);
      this.scl_MagicMaxStar.SetActive(playerUnit.is_memory_intelligence_max);
      this.scl_PowerMaxStar.SetActive(playerUnit.is_memory_strength_max);
      this.scl_ProtctMaxStar.SetActive(playerUnit.is_memory_vitality_max);
      this.scl_SpiritMaxStar.SetActive(playerUnit.is_memory_mind_max);
      this.scl_TechniqueMaxStar.SetActive(playerUnit.is_memory_dexterity_max);
    }
    return true;
  }

  public override IEnumerator initAsync(
    PlayerUnit playerUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs)
  {
    DetailMenuScrollView02 menuScrollView02 = this;
    menuScrollView02.showSkillDialog = (Action<BattleskillSkill>) null;
    menuScrollView02.showSkillLevel = (Action<int, int>) null;
    if (menuScrollView02.showSkillDialog == null)
    {
      if (Object.op_Equality((Object) menuScrollView02.floatingSkillDialogObject, (Object) null))
      {
        menuScrollView02.floatingSkillDialog.transform.Clear();
        menuScrollView02.floatingSkillDialogObject = prefabs[0].Clone(menuScrollView02.floatingSkillDialog.transform).GetComponentInChildren<Battle0171111Event>();
      }
      menuScrollView02.floatingSkillDialogObject.setSkillLv(0);
      ((Component) ((Component) menuScrollView02.floatingSkillDialogObject).transform.parent).gameObject.SetActive(false);
      menuScrollView02.showSkillLevel = (Action<int, int>) ((lv, lvupper) => this.floatingSkillDialogObject.setSkillLv(lv, lvupper));
      menuScrollView02.showSkillDialog = (Action<BattleskillSkill>) (skill =>
      {
        this.floatingSkillDialogObject.setData(skill, "");
        this.floatingSkillDialogObject.Show();
      });
    }
    bool bHyphen = playerUnit.unit.IsAllEquipUnit;
    ((IEnumerable<PlayerUnitGearProficiency>) playerUnit.gear_proficiencies).ForEach<PlayerUnitGearProficiency>((Action<PlayerUnitGearProficiency>) (gp =>
    {
      if (gp.gear_kind_id == playerUnit.unit.kind_GearKind)
      {
        if (Object.op_Equality((Object) this.weapon[0], (Object) null))
        {
          ((Component) this.dyn_Weapon[0]).transform.Clear();
          this.weapon[0] = prefabs[1].Clone(((Component) this.dyn_Weapon[0]).transform);
        }
        this.weapon[0].GetComponent<GearKindIcon>().Init(gp.gear_kind_id);
        ((UIWidget) this.weapon[0].GetComponent<UI2DSprite>()).depth = ((UIWidget) this.dyn_Weapon[0]).depth + 1;
        if (Object.op_Equality((Object) this.gearProfiencyIcon[0], (Object) null))
        {
          ((Component) this.dyn_IconRank[0]).transform.Clear();
          this.gearProfiencyIcon[0] = prefabs[2].Clone(((Component) this.dyn_IconRank[0]).transform).GetComponent<GearProfiencyIcon>();
        }
        this.gearProfiencyIcon[0].Init(gp.level, bHyphen);
      }
      else
      {
        if (gp.gear_kind_id != 7)
          return;
        if (Object.op_Equality((Object) this.weapon[1], (Object) null))
        {
          ((Component) this.dyn_Weapon[1]).transform.Clear();
          this.weapon[1] = prefabs[1].Clone(((Component) this.dyn_Weapon[1]).transform);
        }
        this.weapon[1].GetComponent<GearKindIcon>().Init(gp.gear_kind_id);
        ((UIWidget) this.weapon[1].GetComponent<UI2DSprite>()).depth = ((UIWidget) this.dyn_Weapon[1]).depth + 1;
        if (Object.op_Equality((Object) this.gearProfiencyIcon[1], (Object) null))
        {
          ((Component) this.dyn_IconRank[1]).transform.Clear();
          this.gearProfiencyIcon[1] = prefabs[2].Clone(((Component) this.dyn_IconRank[1]).transform).GetComponent<GearProfiencyIcon>();
        }
        this.gearProfiencyIcon[1].Init(gp.level, bHyphen);
      }
    }));
    if (!menuScrollView02.isEarthMode)
    {
      if (playerUnit.leader_skill != null)
      {
        menuScrollView02.txt_Skillname.alignment = (NGUIText.Alignment) 1;
        menuScrollView02.txt_Skillname.SetTextLocalize(playerUnit.leader_skill.skill.name);
        menuScrollView02.txt_SkillDescription.SetTextLocalize(playerUnit.leader_skill.skill.description);
      }
      else
      {
        menuScrollView02.txt_Skillname.alignment = (NGUIText.Alignment) 2;
        menuScrollView02.txt_Skillname.SetTextLocalize("-");
        menuScrollView02.txt_SkillDescription.SetTextLocalize("");
      }
    }
    yield return (object) menuScrollView02.SetSkill(playerUnit, limitMode, prefabs);
    yield return (object) null;
  }

  private SortedDictionary<int, Tuple<BattleskillSkill, int, int, int>> GetInfoSKills(
    PlayerUnit playerUnit,
    int dynLength,
    bool isEarthMode)
  {
    Func<BattleskillSkillType, bool> IsNotLeaderSkill = (Func<BattleskillSkillType, bool>) (type => BattleskillSkillType.leader != type);
    Func<BattleskillSkillType, bool> IsNotLeaderOrMagicSkill = (Func<BattleskillSkillType, bool>) (type => BattleskillSkillType.leader != type && BattleskillSkillType.magic != type);
    SortedDictionary<int, Tuple<BattleskillSkill, int, int, int>> infoSkills = new SortedDictionary<int, Tuple<BattleskillSkill, int, int, int>>();
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    foreach (PlayerUnitSkills playerUnitSkills in ((IEnumerable<PlayerUnitSkills>) playerUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => IsNotLeaderSkill(x.skill.skill_type))))
    {
      if (!dictionary.ContainsKey(playerUnitSkills.skill_id))
        dictionary.Add(playerUnitSkills.skill_id, playerUnitSkills.level);
    }
    List<UnitSkill> list = ((IEnumerable<UnitSkill>) playerUnit.unit.RememberUnitSkills(playerUnit._unit_type)).Where<UnitSkill>((Func<UnitSkill, bool>) (x => IsNotLeaderOrMagicSkill(x.skill.skill_type))).ToList<UnitSkill>();
    int num1 = 0;
    int num2 = Math.Min(list.Count<UnitSkill>(), dynLength);
    for (int index = 0; index < num2; ++index)
    {
      UnitSkill unitSkill = list[index];
      BattleskillSkill battleskillSkill = playerUnit.evolutionSkill(unitSkill.skill);
      if (dictionary.ContainsKey(battleskillSkill.ID))
      {
        int num3 = Math.Min(dictionary[battleskillSkill.ID], battleskillSkill.upper_level);
        if (!infoSkills.ContainsKey(battleskillSkill.ID))
          infoSkills.Add(battleskillSkill.ID, new Tuple<BattleskillSkill, int, int, int>(battleskillSkill, num1++, num3, unitSkill.level));
      }
      else if (unitSkill.skill.skill_type != BattleskillSkillType.magic && !infoSkills.ContainsKey(battleskillSkill.ID))
        infoSkills.Add(battleskillSkill.ID, new Tuple<BattleskillSkill, int, int, int>(battleskillSkill, num1++, 0, unitSkill.level));
    }
    if (!isEarthMode)
    {
      foreach (UnitSkillCharacterQuest skillCharacterQuest1 in ((IEnumerable<UnitSkillCharacterQuest>) MasterData.UnitSkillCharacterQuestList).Where<UnitSkillCharacterQuest>((Func<UnitSkillCharacterQuest, bool>) (x => x.unit.ID == playerUnit.unit.ID && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillCharacterQuest>())
      {
        int key = 0;
        BattleskillSkill battleskillSkill1 = (BattleskillSkill) null;
        UnitSkillCharacterQuest skillCharacterQuest2 = skillCharacterQuest1;
        BattleskillSkill battleskillSkill2 = playerUnit.evolutionSkill(skillCharacterQuest2.skill);
        if (dictionary.ContainsKey(battleskillSkill2.ID))
        {
          key = battleskillSkill2.ID;
          battleskillSkill1 = battleskillSkill2;
        }
        else if (dictionary.ContainsKey(skillCharacterQuest2.skill_after_evolution))
        {
          key = skillCharacterQuest2.skill_after_evolution;
          battleskillSkill1 = skillCharacterQuest2.skillOfEvolution;
        }
        if (key != 0 && battleskillSkill1.skill_type != BattleskillSkillType.magic)
        {
          int num4 = Math.Min(dictionary[key], battleskillSkill1.upper_level);
          if (!infoSkills.ContainsKey(battleskillSkill1.ID))
            infoSkills.Add(battleskillSkill1.ID, new Tuple<BattleskillSkill, int, int, int>(battleskillSkill1, num1++, num4, 0));
        }
      }
    }
    return infoSkills;
  }

  private IEnumerator SetSkill(PlayerUnit playerUnit, bool limitMode, GameObject[] prefabs)
  {
    DetailMenuScrollView02 menuScrollView02 = this;
    ((IEnumerable<UIButton>) menuScrollView02.dyn_UnitSkill).ForEach<UIButton>((Action<UIButton>) (x => x.onClick.Clear()));
    ((IEnumerable<BattleSkillIcon>) menuScrollView02.unitSkillIcon).ForEach<BattleSkillIcon>((Action<BattleSkillIcon>) (x =>
    {
      if (Object.op_Equality((Object) x, (Object) null))
        return;
      ((Component) x).gameObject.SetActive(false);
    }));
    menuScrollView02.setSkills.Clear();
    bool isExistExtraskill = false;
    bool isMultiExtraskill = false;
    menuScrollView02.awakeSkill_enemy = (PlayerUnitSkills) null;
    IEnumerator e;
    if (Object.op_Inequality((Object) menuScrollView02.dynExtraskillEquiped, (Object) null) && Object.op_Inequality((Object) menuScrollView02.dynExtraskillEquipedBtn, (Object) null) && !menuScrollView02.isEarthMode)
    {
      ((Component) menuScrollView02.dynExtraskillEquiped).gameObject.SetActive(false);
      menuScrollView02.slcExtraskillIconLocked.SetActive(false);
      menuScrollView02.slcExtraskillIconNone.SetActive(false);
      menuScrollView02.dirExtraskillIconNone.SetActive(false);
      if (limitMode)
        EventDelegate.Set(menuScrollView02.slcExtraskillIconNone.GetComponent<UIButton>().onClick, (EventDelegate.Callback) (() => { }));
      if (playerUnit.is_storage)
      {
        EventDelegate.Set(menuScrollView02.slcExtraskillIconNone.GetComponent<UIButton>().onClick, (EventDelegate.Callback) (() => { }));
        ((Behaviour) menuScrollView02.slcExtraskillIconLocked.GetComponent<UIButton>()).enabled = false;
      }
      Future<Sprite> spriteF;
      if (playerUnit.unit.trust_target_flag && !playerUnit.is_guest)
      {
        menuScrollView02.dirExtraskillIconNone.SetActive(true);
        isExistExtraskill = true;
        menuScrollView02.awakeSkill = playerUnit.equippedExtraSkill;
        if (menuScrollView02.awakeSkill != null)
        {
          ((Component) menuScrollView02.dynExtraskillEquiped).gameObject.SetActive(true);
          spriteF = menuScrollView02.awakeSkill.masterData.LoadBattleSkillIcon();
          e = spriteF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          menuScrollView02.dynExtraskillEquiped.sprite2D = spriteF.Result;
          menuScrollView02.setSkills.Add(menuScrollView02.awakeSkill.skill_id);
          menuScrollView02.dirExtraskillIconNone.SetActive(true);
          spriteF = (Future<Sprite>) null;
        }
        else if (playerUnit.can_equip_awake_skill)
        {
          menuScrollView02.dynExtraskillEquiped.sprite2D = (Sprite) null;
          ((Component) menuScrollView02.dynExtraskillEquiped).gameObject.SetActive(true);
          menuScrollView02.slcExtraskillIconNone.SetActive(true);
          menuScrollView02.setupExtraSkillIconNoneSprite();
          isExistExtraskill = true;
        }
        else
          menuScrollView02.slcExtraskillIconLocked.SetActive(true);
      }
      else if (!playerUnit.is_guest && playerUnit.is_enemy)
      {
        PlayerUnitSkills playerUnitSkills = ((IEnumerable<PlayerUnitSkills>) playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.awake_skill_category_id != 1));
        if (playerUnitSkills != null && !menuScrollView02.setSkills.Contains(playerUnitSkills.skill.ID))
        {
          menuScrollView02.awakeSkill_enemy = playerUnitSkills;
          menuScrollView02.dirExtraskillIconNone.SetActive(true);
          isExistExtraskill = true;
          ((Component) menuScrollView02.dynExtraskillEquiped).gameObject.SetActive(true);
          spriteF = menuScrollView02.awakeSkill_enemy.skill.LoadBattleSkillIcon();
          e = spriteF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          menuScrollView02.dynExtraskillEquiped.sprite2D = spriteF.Result;
          menuScrollView02.setSkills.Add(menuScrollView02.awakeSkill_enemy.skill_id);
          menuScrollView02.dirExtraskillIconNone.SetActive(true);
          spriteF = (Future<Sprite>) null;
        }
      }
    }
    if (!menuScrollView02.isEarthMode)
    {
      PlayerUnitSkills skill = (PlayerUnitSkills) null;
      Dictionary<int, UnitSkillEvolution> unitSkillEvolutionDict = ((IEnumerable<UnitSkillEvolution>) MasterData.UnitSkillEvolutionList).Where<UnitSkillEvolution>((Func<UnitSkillEvolution, bool>) (x => x.unit.ID == playerUnit.unit.ID)).ToDictionary<UnitSkillEvolution, int>((Func<UnitSkillEvolution, int>) (x => x.after_skill.ID));
      UnitSkillHarmonyQuest[] array1 = ((IEnumerable<UnitSkillHarmonyQuest>) MasterData.UnitSkillHarmonyQuestList).Where<UnitSkillHarmonyQuest>((Func<UnitSkillHarmonyQuest, bool>) (x => x.character.ID == playerUnit.unit.character.ID && x.skill.skill_type != BattleskillSkillType.leader)).ToArray<UnitSkillHarmonyQuest>();
      if (array1.Length != 0)
      {
        List<PlayerUnitSkills> list = ((IEnumerable<UnitSkillHarmonyQuest>) array1).Select<UnitSkillHarmonyQuest, PlayerUnitSkills>((Func<UnitSkillHarmonyQuest, PlayerUnitSkills>) (s => ((IEnumerable<PlayerUnitSkills>) playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (fd =>
        {
          if (s.skill == fd.skill)
            return true;
          return unitSkillEvolutionDict.ContainsKey(fd.skill.ID) && s.skill == unitSkillEvolutionDict[fd.skill.ID].before_skill;
        })))).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (w => w != null)).Distinct<PlayerUnitSkills>().ToList<PlayerUnitSkills>();
        if (list.Any<PlayerUnitSkills>())
        {
          PlayerUnitSkills playerUnitSkills = list[0];
          if (!menuScrollView02.setSkills.Contains(playerUnitSkills.skill.ID))
            skill = playerUnitSkills;
        }
      }
      if (skill == null)
      {
        UnitSkillIntimate[] array2 = ((IEnumerable<UnitSkillIntimate>) MasterData.UnitSkillIntimateList).Where<UnitSkillIntimate>((Func<UnitSkillIntimate, bool>) (x => x.unit.ID == playerUnit.unit.ID && x.skill.DispSkillList)).ToArray<UnitSkillIntimate>();
        if (array2.Length != 0)
        {
          List<PlayerUnitSkills> list = ((IEnumerable<UnitSkillIntimate>) array2).Select<UnitSkillIntimate, PlayerUnitSkills>((Func<UnitSkillIntimate, PlayerUnitSkills>) (s => ((IEnumerable<PlayerUnitSkills>) playerUnit.skills).FirstOrDefault<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (fd =>
          {
            if (s.skill == fd.skill)
              return true;
            return unitSkillEvolutionDict.ContainsKey(fd.skill.ID) && s.skill == unitSkillEvolutionDict[fd.skill.ID].before_skill;
          })))).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (w => w != null)).Distinct<PlayerUnitSkills>().ToList<PlayerUnitSkills>();
          if (list.Any<PlayerUnitSkills>())
          {
            PlayerUnitSkills playerUnitSkills = list[0];
            if (!menuScrollView02.setSkills.Contains(playerUnitSkills.skill.ID))
              skill = playerUnitSkills;
          }
        }
      }
      if (skill != null)
      {
        menuScrollView02.dirMultiSkillIconNone.gameObject.SetActive(true);
        if (skill != null)
        {
          menuScrollView02.createBattleSkillIcon(menuScrollView02.dirMultiSkillIcon, skill, prefabs[3]);
          menuScrollView02.dirMultiSkillIcon.gameObject.SetActive(true);
        }
        else
          menuScrollView02.dirMultiSkillIcon.GetComponentInChildren<UI2DSprite>().sprite2D = (Sprite) null;
        isMultiExtraskill = true;
      }
      else
      {
        menuScrollView02.dirMultiSkillIcon.gameObject.SetActive(false);
        menuScrollView02.dirMultiSkillIconNone.gameObject.SetActive(false);
      }
    }
    else
    {
      if (Object.op_Inequality((Object) menuScrollView02.dirMultiSkillIcon, (Object) null))
        menuScrollView02.dirMultiSkillIcon.gameObject.SetActive(false);
      if (Object.op_Inequality((Object) menuScrollView02.dirMultiSkillIconNone, (Object) null))
        menuScrollView02.dirMultiSkillIconNone.gameObject.SetActive(false);
    }
    int idx = 0;
    SortedDictionary<int, Tuple<BattleskillSkill, int, int, int>> skillDict = new SortedDictionary<int, Tuple<BattleskillSkill, int, int, int>>();
    if (playerUnit.is_guest)
    {
      int num1 = 0;
      for (int index = 0; index < playerUnit.skills.Length; ++index)
      {
        PlayerUnitSkills skill1 = playerUnit.skills[index];
        BattleskillSkill skill2 = skill1.skill;
        if (skill2.skill_type != BattleskillSkillType.magic)
        {
          int num2 = Math.Min(skill1.level, skill2.upper_level);
          skillDict.Add(skill2.ID, new Tuple<BattleskillSkill, int, int, int>(skill2, num1++, num2, skill1.level));
        }
      }
    }
    else
      skillDict = menuScrollView02.GetInfoSKills(playerUnit, menuScrollView02.dyn_UnitSkill.Length, menuScrollView02.isEarthMode);
    if (skillDict != null && skillDict.Count > 0)
    {
      foreach (KeyValuePair<int, Tuple<BattleskillSkill, int, int, int>> keyValuePair in skillDict)
      {
        Tuple<BattleskillSkill, int, int, int> tuple = keyValuePair.Value;
        if (BattleskillSkill.InvestElementSkillIds.Contains(tuple.Item1.ID) && !menuScrollView02.setSkills.Contains(tuple.Item1.ID))
        {
          e = menuScrollView02.CreateSkillIcon(tuple.Item1, idx++, tuple.Item3, tuple.Item4, prefabs[3]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        }
      }
      foreach (KeyValuePair<int, Tuple<BattleskillSkill, int, int, int>> keyValuePair in skillDict)
      {
        Tuple<BattleskillSkill, int, int, int> tuple = keyValuePair.Value;
        if (tuple.Item1.skill_type == BattleskillSkillType.growth && !menuScrollView02.setSkills.Contains(tuple.Item1.ID))
        {
          e = menuScrollView02.CreateSkillIcon(tuple.Item1, idx++, tuple.Item3, tuple.Item4, prefabs[3]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        }
      }
      int len = 4;
      foreach (KeyValuePair<int, Tuple<BattleskillSkill, int, int, int>> keyValuePair in skillDict)
      {
        Tuple<BattleskillSkill, int, int, int> tuple = keyValuePair.Value;
        if (tuple.Item1.skill_type == BattleskillSkillType.duel && !menuScrollView02.setSkills.Contains(tuple.Item1.ID))
        {
          e = menuScrollView02.CreateSkillIcon(tuple.Item1, idx++, tuple.Item3, tuple.Item4, prefabs[3]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (--len <= 0)
            break;
        }
      }
      foreach (KeyValuePair<int, Tuple<BattleskillSkill, int, int, int>> keyValuePair in skillDict)
      {
        Tuple<BattleskillSkill, int, int, int> tuple = keyValuePair.Value;
        if (tuple.Item1.skill_type == BattleskillSkillType.release && !menuScrollView02.setSkills.Contains(tuple.Item1.ID))
        {
          e = menuScrollView02.CreateSkillIcon(tuple.Item1, idx++, tuple.Item3, tuple.Item4, prefabs[3]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        }
      }
      len = 4;
      foreach (KeyValuePair<int, Tuple<BattleskillSkill, int, int, int>> keyValuePair in skillDict)
      {
        Tuple<BattleskillSkill, int, int, int> tuple = keyValuePair.Value;
        if (tuple.Item1.skill_type == BattleskillSkillType.command && !menuScrollView02.setSkills.Contains(tuple.Item1.ID))
        {
          e = menuScrollView02.CreateSkillIcon(tuple.Item1, idx++, tuple.Item3, tuple.Item4, prefabs[3]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (--len <= 0)
            break;
        }
      }
      len = 9;
      foreach (KeyValuePair<int, Tuple<BattleskillSkill, int, int, int>> keyValuePair in skillDict)
      {
        Tuple<BattleskillSkill, int, int, int> tuple = keyValuePair.Value;
        if (tuple.Item1.skill_type == BattleskillSkillType.passive && !menuScrollView02.setSkills.Contains(tuple.Item1.ID))
        {
          e = menuScrollView02.CreateSkillIcon(tuple.Item1, idx++, tuple.Item3, tuple.Item4, prefabs[3]);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (--len <= 0)
            break;
        }
      }
    }
    skillDict = (SortedDictionary<int, Tuple<BattleskillSkill, int, int, int>>) null;
    if (menuScrollView02.isEarthMode)
    {
      if (playerUnit.equippedGear != (PlayerItem) null)
      {
        for (idx = 0; idx < menuScrollView02.dyn_WeaponTypes.Length; ++idx)
        {
          if (Object.op_Inequality((Object) menuScrollView02.battleSkillIcon[idx], (Object) null))
            ((Component) menuScrollView02.battleSkillIcon[idx]).gameObject.SetActive(false);
          menuScrollView02.dyn_WeaponSkills[idx].onClick.Clear();
          if (playerUnit.equippedGear.skills.Length > idx)
          {
            BattleskillSkill weaponSkill = playerUnit.equippedGear.skills[idx].skill;
            if (weaponSkill != null)
            {
              if (Object.op_Equality((Object) menuScrollView02.battleSkillIcon[idx], (Object) null))
              {
                menuScrollView02.battleSkillIcon[idx] = prefabs[3].Clone(((Component) menuScrollView02.dyn_WeaponTypes[idx]).transform).GetComponent<BattleSkillIcon>();
                menuScrollView02.battleSkillIcon[idx].SetDepth(menuScrollView02.dyn_WeaponTypes[idx].depth + 1);
              }
              ((Component) menuScrollView02.battleSkillIcon[idx]).gameObject.SetActive(true);
              e = menuScrollView02.battleSkillIcon[idx].Init(weaponSkill);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              menuScrollView02.setShowSkillDialog(menuScrollView02.dyn_WeaponSkills[idx], weaponSkill, playerUnit.equippedGear.skills[idx].skill_level);
            }
            weaponSkill = (BattleskillSkill) null;
          }
        }
      }
      else
      {
        for (int index = 0; index < menuScrollView02.dyn_WeaponTypes.Length; ++index)
        {
          if (!Object.op_Equality((Object) menuScrollView02.battleSkillIcon[index], (Object) null))
          {
            ((Component) menuScrollView02.battleSkillIcon[index]).gameObject.SetActive(false);
            menuScrollView02.dyn_WeaponSkills[index].onClick.Clear();
          }
        }
      }
    }
    if (!menuScrollView02.isEarthMode)
    {
      int num3 = (isExistExtraskill ? 1 : 0) + (isMultiExtraskill ? 1 : 0);
      int num4 = menuScrollView02.slc_skill_icon_none.Length - 1;
      for (int index = 0; index < 2; ++index)
      {
        bool flag = true;
        if (index < num3)
          flag = false;
        menuScrollView02.slc_skill_icon_none[num4 - index].gameObject.SetActive(flag);
        menuScrollView02.dir_Unit_Skill[num4 - index].gameObject.SetActive(flag);
      }
      menuScrollView02.skillIconGrid.Reposition();
      menuScrollView02.weaponNoneGrid.Reposition();
    }
  }

  private void createBattleSkillIcon(
    GameObject parent,
    BattleskillSkill skill,
    GameObject skillTypeIconPrefab)
  {
    GameObject gameObject = skillTypeIconPrefab.Clone();
    ((UIWidget) gameObject.GetComponentInChildren<UI2DSprite>()).depth = ((UIWidget) ((Component) parent.transform).GetComponentInChildren<UI2DSprite>()).depth;
    gameObject.gameObject.SetParent(parent);
    this.StartCoroutine(gameObject.GetComponentInChildren<BattleSkillIcon>().Init(skill));
    this.setSkills.Add(skill.ID);
  }

  private void createBattleSkillIcon(
    GameObject parent,
    PlayerUnitSkills skill,
    GameObject skillTypeIconPrefab)
  {
    this.createBattleSkillIcon(parent, skill.skill, skillTypeIconPrefab);
    EventDelegate.Add(parent.GetComponentInChildren<UIButton>().onClick, (EventDelegate.Callback) (() =>
    {
      if (this.showSkillDialog == null || this.showSkillLevel == null)
        return;
      this.showSkillDialog(skill.skill);
      this.showSkillLevel(skill.level, skill.skill.upper_level);
    }));
  }

  private void setShowSkillDialog(UIButton button, BattleskillSkill weaponSkill, int level)
  {
    EventDelegate.Add(button.onClick, (EventDelegate.Callback) (() =>
    {
      if (this.showSkillDialog == null)
        return;
      this.showSkillDialog(weaponSkill);
      this.showSkillLevel(level, weaponSkill.upper_level);
    }));
  }

  private IEnumerator CreateSkillIcon(
    BattleskillSkill sk,
    int idx,
    int unitSkillLv,
    int needLv,
    GameObject skillTypeIconPrefab)
  {
    if (idx < this.unitSkillIcon.Length)
    {
      this.setSkills.Add(sk.ID);
      if (Object.op_Equality((Object) this.unitSkillIcon[idx], (Object) null))
      {
        this.unitSkillIcon[idx] = skillTypeIconPrefab.Clone(((Component) this.dyn_UnitTypes[idx]).transform).GetComponent<BattleSkillIcon>();
        this.unitSkillIcon[idx].SetDepth(((UIWidget) this.dyn_UnitTypes[idx]).depth + 1);
      }
      if (unitSkillLv == 0)
        this.unitSkillIcon[idx].EnableNeedLvIcon(needLv);
      else
        this.unitSkillIcon[idx].DisableNeedDisp();
      ((Component) this.unitSkillIcon[idx]).gameObject.SetActive(true);
      IEnumerator e = this.unitSkillIcon[idx].Init(sk);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      EventDelegate.Add(this.dyn_UnitSkill[idx].onClick, (EventDelegate.Callback) (() =>
      {
        if (this.showSkillDialog == null || this.showSkillLevel == null)
          return;
        this.showSkillDialog(sk);
        this.showSkillLevel(unitSkillLv, sk.upper_level);
      }));
    }
  }

  public override void EndScene()
  {
    foreach (UIButton dynWeaponSkill in this.dyn_WeaponSkills)
    {
      if (((Component) dynWeaponSkill).gameObject.activeSelf)
      {
        if (Object.op_Inequality((Object) dynWeaponSkill, (Object) null))
          dynWeaponSkill.onClick.Clear();
        BattleSkillIcon componentInChildren = ((Component) dynWeaponSkill).GetComponentInChildren<BattleSkillIcon>();
        if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          Object.Destroy((Object) ((Component) componentInChildren).gameObject);
      }
    }
    foreach (UIButton uiButton in this.dyn_UnitSkill)
    {
      if (((Component) uiButton).gameObject.activeSelf)
      {
        BattleSkillIcon componentInChildren = ((Component) uiButton).GetComponentInChildren<BattleSkillIcon>();
        if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          Object.Destroy((Object) ((Component) componentInChildren).gameObject);
        if (Object.op_Inequality((Object) uiButton, (Object) null))
          uiButton.onClick.Clear();
      }
    }
  }

  public void onExtraSkillLockClick()
  {
    string placeholder = this.targetUnit.unit.IsSea ? Consts.GetInstance().popup_004_ExtraSkill_ReleaseCondition_text : Consts.GetInstance().popup_004_ExtraSkill_ReleaseCondition_text_second;
    ModalWindow.Show(Consts.GetInstance().popup_004_ExtraSkill_ReleaseCondition_title, Consts.Format(placeholder, (IDictionary) new Hashtable()
    {
      {
        (object) "percent",
        (object) PlayerUnit.GetExtraSkillReleaseRate()
      }
    }), (Action) (() => { }));
  }

  private void setupExtraSkillIconNoneSprite()
  {
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    string str = this.targetUnit.unit.canUseAllGearHackSkill ? "slc_skill_icon_base_unit_special-unit_60_62.png__GUI__common__common_prefab" : "slc_extraskill_icon_base_60_62.png__GUI__common__common_prefab";
    UISprite component1 = this.slcExtraskillIconNone.GetComponent<UISprite>();
    UIButton component2 = this.slcExtraskillIconNone.GetComponent<UIButton>();
    component1.spriteName = str;
    component2.normalSprite = str;
    component2.pressedSprite = str;
  }
}
