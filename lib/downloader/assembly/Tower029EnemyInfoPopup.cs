// Decompiled with JetBrains decompiler
// Type: Tower029EnemyInfoPopup
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
public class Tower029EnemyInfoPopup : BackButtonMonoBehaiviour
{
  private const int WEAPON_ELEMENT_ICON_ID = 0;
  private const int SECOND_WEAPON_ELEMENT_ICON_ID = 5;
  private const string MAGIC_BULLET_NONE_NAME = "-";
  private const int SLC_STATUS_BASE_OWN_ICON_PLAYER = 0;
  private const int SLC_STATUS_BASE_OWN_ICON_GUEST = 1;
  [SerializeField]
  private Color mGreen = new Color(0.0f, 0.863f, 0.118f);
  [SerializeField]
  private Color mRed = new Color(0.98f, 0.0f, 0.0f);
  [SerializeField]
  protected GameObject backGround;
  [Space(10f)]
  [Header("Force Objects")]
  [SerializeField]
  protected GameObject[] dir_ForceHeader;
  [SerializeField]
  protected GameObject[] dir_ForceHpObjects;
  [SerializeField]
  protected GameObject[] dir_ForceJobObjects;
  [SerializeField]
  private GameObject[] slcStatus1BaseOwnIcons;
  [Header("Bassic")]
  [SerializeField]
  private Transform characterTransform;
  [SerializeField]
  protected NGTweenGaugeScale hpGauge;
  [SerializeField]
  protected UILabel txt_CharacterName;
  [SerializeField]
  protected UILabel txt_Lv;
  [SerializeField]
  protected UILabel txt_Hp;
  [SerializeField]
  protected UILabel txt_Hpmax;
  [SerializeField]
  protected UILabel txt_Jobname;
  [SerializeField]
  protected UILabel txt_Hp_Enemy;
  [SerializeField]
  protected UILabel txt_Hpmax_Enemy;
  [SerializeField]
  protected UILabel txt_Jobname_Enemy;
  [SerializeField]
  protected UISprite princessTypeSprite;
  [SerializeField]
  protected UI2DSprite iconWeaponType_;
  [Header("Status1")]
  [SerializeField]
  protected UILabel txt_Attack;
  [SerializeField]
  protected UILabel txt_Critical;
  [SerializeField]
  protected UILabel txt_Defense;
  [SerializeField]
  protected UILabel txt_Dexterity;
  [SerializeField]
  protected UILabel txt_Evasion;
  [SerializeField]
  protected UILabel txt_Fighting;
  [SerializeField]
  protected UILabel txt_Matk;
  [SerializeField]
  protected UILabel txt_Mdef;
  [SerializeField]
  protected UILabel txt_Movement;
  [Header("Status1 Buff Debuff")]
  [SerializeField]
  protected UILabel txt_AttackBD;
  [SerializeField]
  protected UILabel txt_CriticalBD;
  [SerializeField]
  protected UILabel txt_DefenseBD;
  [SerializeField]
  protected UILabel txt_DexterityBD;
  [SerializeField]
  protected UILabel txt_EvasionBD;
  [SerializeField]
  protected UILabel txt_MatkBD;
  [SerializeField]
  protected UILabel txt_MdefBD;
  [Header("Status2")]
  [SerializeField]
  protected UILabel txt_Agility;
  [SerializeField]
  protected UILabel txt_Luck;
  [SerializeField]
  protected UILabel txt_Magic;
  [SerializeField]
  protected UILabel txt_Power;
  [SerializeField]
  protected UILabel txt_Stability;
  [SerializeField]
  protected UILabel txt_Spirit;
  [SerializeField]
  protected UILabel txt_Technique;
  [Header("Status2 Buff Debuff")]
  [SerializeField]
  protected UILabel txt_AgilityBD;
  [SerializeField]
  protected UILabel txt_LuckBD;
  [SerializeField]
  protected UILabel txt_MagicBD;
  [SerializeField]
  protected UILabel txt_PowerBD;
  [SerializeField]
  protected UILabel txt_StabilityBD;
  [SerializeField]
  protected UILabel txt_SpiritBD;
  [SerializeField]
  protected UILabel txt_TechniqueBD;
  private List<PlayerUnitSkills> dispMagicBullets;
  [Header("Magic Bullet")]
  [SerializeField]
  protected Transform[] elementTypeIconParent;
  protected GameObject elementTypeIconPrefab;
  private GameObject[] elementTypeIcon = new GameObject[6];
  [SerializeField]
  protected UILabel[] txt_Magic_range;
  [SerializeField]
  protected UILabel[] txt_Magic_cost;
  protected GameObject skillTypeIconPrefab;
  protected GameObject skillDialogPrefab;
  private GameObject skillDialog;
  [Header("Skill List")]
  [SerializeField]
  private Transform abilityDialogParent;
  private GameObject abilityDialogPrefab;
  private DialogJobAbilityDetail abilityDialog;
  [SerializeField]
  protected GameObject SkillDialogRoot;
  [SerializeField]
  protected GameObject[] dyn_Skill;
  protected List<int> setSkills = new List<int>();
  private GameObject gearKindIconPrefab;
  private GameObject gearProfiencyIconPrefab;
  private bool isEnabledGearLayout_;
  [Header("Gear")]
  [SerializeField]
  protected TweenAlpha tweenAlphaFirstWeapon;
  [SerializeField]
  protected TweenAlpha tweenAlphaSecondWeapon;
  [SerializeField]
  private Tower029EnemyInfoPopup.GearLayout[] gearLayouts_;
  private GameObject gearIconPrefab_;
  private Queue<IEnumerator> queGearInit_;
  private BL.Unit pUnit;
  private Tower029EnemyInfoPopup.Force forceId;

  public IEnumerator InitializeAsync(BL.Unit playerUnit, Tower029EnemyInfoPopup.Force force)
  {
    this.forceId = force;
    this.pUnit = playerUnit;
    IEnumerator e = this.laodAndSetupObjects();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.initialize(this.pUnit);
  }

  private IEnumerator laodAndSetupObjects()
  {
    Future<GameObject> gearKindIconPrefabF = Res.Icons.GearKindIcon.Load<GameObject>();
    IEnumerator e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.gearKindIconPrefab = gearKindIconPrefabF.Result;
    Future<GameObject> skillTypeIconPrefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = skillTypeIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillTypeIconPrefab = skillTypeIconPrefabF.Result;
    Future<GameObject> elementTypeIconPrefabF = Res.Icons.CommonElementIcon.Load<GameObject>();
    e = elementTypeIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.elementTypeIconPrefab = elementTypeIconPrefabF.Result;
    Future<GameObject> gearProfiencyIconPrefabF = Res.Icons.GearProfiencyIcon.Load<GameObject>();
    e = gearProfiencyIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.gearProfiencyIconPrefab = gearProfiencyIconPrefabF.Result;
    Future<GameObject> skillDialogPrefabF = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
    e = skillDialogPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillDialogPrefab = skillDialogPrefabF.Result;
    this.skillDialog = this.skillDialogPrefab.Clone(this.SkillDialogRoot.transform);
    this.skillDialog.GetComponentInChildren<UIPanel>().depth += 30;
    this.skillDialog.SetActive(false);
    Future<GameObject> abilityDialogPrefabF;
    if (Object.op_Inequality((Object) this.abilityDialogParent, (Object) null))
    {
      abilityDialogPrefabF = new ResourceObject("Prefabs/battle017_11_1_1/AbilityDetailDialog").Load<GameObject>();
      e = abilityDialogPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.abilityDialogPrefab = abilityDialogPrefabF.Result;
      GameObject gameObject = this.abilityDialogPrefab.Clone(this.abilityDialogParent);
      UIWidget component1 = ((Component) this.abilityDialogParent).GetComponent<UIWidget>();
      UIPanel component2 = gameObject.GetComponent<UIPanel>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        component2.depth += Object.op_Inequality((Object) component1, (Object) null) ? component1.depth + 1 : 30;
      this.abilityDialog = gameObject.GetComponent<DialogJobAbilityDetail>();
      ((Component) this.abilityDialog).gameObject.SetActive(false);
      abilityDialogPrefabF = (Future<GameObject>) null;
    }
    this.setSkills.Clear();
    this.isEnabledGearLayout_ = this.gearLayouts_ != null && this.gearLayouts_.Length != 0;
    if (this.isEnabledGearLayout_)
    {
      this.queGearInit_ = new Queue<IEnumerator>(this.gearLayouts_.Length);
      abilityDialogPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = abilityDialogPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.gearIconPrefab_ = abilityDialogPrefabF.Result;
      abilityDialogPrefabF = (Future<GameObject>) null;
      foreach (Tower029EnemyInfoPopup.GearLayout gearLayout in this.gearLayouts_)
      {
        gearLayout.gear_ = this.createIcon(this.gearIconPrefab_, gearLayout.lnkIcon_).GetComponent<ItemIcon>();
        gearLayout.proficiencyKind_ = this.createIcon(this.gearKindIconPrefab, gearLayout.lnkProficiencyKind_).GetComponent<GearKindIcon>();
        gearLayout.proficiency_ = this.createIcon(this.gearProfiencyIconPrefab, gearLayout.lnkProficiency_).GetComponent<GearProfiencyIcon>();
        gearLayout.disabled();
      }
    }
  }

  private void initialize(BL.Unit v)
  {
    Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) v);
    this.SetGearNameAnimation(v);
    this.SetActiveForceObjects(v);
    ((IEnumerable<GameObject>) this.slcStatus1BaseOwnIcons).ToggleOnce(v.playerUnit.is_guest ? 1 : 0);
    UnitUnit unit = v.unit;
    if (Object.op_Inequality((Object) this.iconWeaponType_, (Object) null))
      this.iconWeaponType_.sprite2D = GearKindIcon.LoadSprite(unit.kind.Enum, v.GetElement());
    this.setText(this.txt_Agility, battleParameter.Agility);
    this.setText(this.txt_Luck, battleParameter.Luck);
    this.setText(this.txt_Magic, battleParameter.Intelligence);
    this.setText(this.txt_Power, battleParameter.Strength);
    this.setText(this.txt_Stability, battleParameter.Vitality);
    this.setText(this.txt_Spirit, battleParameter.Mind);
    this.setText(this.txt_Technique, battleParameter.Dexterity);
    this.setBDTextWrraper(this.txt_AgilityBD, battleParameter.AgilityIncr);
    this.setBDTextWrraper(this.txt_LuckBD, battleParameter.LuckIncr);
    this.setBDTextWrraper(this.txt_MagicBD, battleParameter.IntelligenceIncr);
    this.setBDTextWrraper(this.txt_PowerBD, battleParameter.StrengthIncr);
    this.setBDTextWrraper(this.txt_StabilityBD, battleParameter.VitalityIncr);
    this.setBDTextWrraper(this.txt_SpiritBD, battleParameter.MindIncr);
    this.setBDTextWrraper(this.txt_TechniqueBD, battleParameter.DexterityIncr);
    this.hpGauge.setValue(v.playerUnit.TowerHp, battleParameter.Hp, false);
    this.setText(this.txt_Hp, v.playerUnit.TowerHp);
    if (Object.op_Inequality((Object) this.txt_Hp_Enemy, (Object) null))
      this.setText(this.txt_Hp_Enemy, v.playerUnit.TowerHp);
    this.setText(this.txt_Hpmax, "/" + (object) battleParameter.Hp);
    if (Object.op_Inequality((Object) this.txt_Hpmax_Enemy, (Object) null))
      this.setText(this.txt_Hpmax_Enemy, "/" + (object) battleParameter.Hp);
    this.setText(this.txt_CharacterName, string.IsNullOrEmpty(unit.formal_name) ? unit.name : unit.formal_name);
    this.setText(this.txt_Lv, v.lv);
    this.setColordText(this.txt_Fighting, battleParameter.Combat, battleParameter.CombatIncr);
    this.setText(this.txt_Jobname, v.job.name);
    if (Object.op_Inequality((Object) this.txt_Jobname_Enemy, (Object) null))
      this.setText(this.txt_Jobname_Enemy, v.job.name);
    this.setColordText(this.txt_Movement, battleParameter.Move, battleParameter.MoveIncr);
    this.setText(this.txt_Attack, battleParameter.PhysicalAttack);
    this.setText(this.txt_Critical, battleParameter.Critical);
    this.setText(this.txt_Defense, battleParameter.PhysicalDefense);
    this.setText(this.txt_Dexterity, battleParameter.Hit);
    this.setText(this.txt_Evasion, battleParameter.Evasion);
    this.setText(this.txt_Matk, battleParameter.MagicAttack);
    this.setText(this.txt_Mdef, battleParameter.MagicDefense);
    this.setBDTextWrraper(this.txt_AttackBD, battleParameter.PhysicalAttackIncr);
    this.setBDTextWrraper(this.txt_CriticalBD, battleParameter.CriticalIncr);
    this.setBDTextWrraper(this.txt_DefenseBD, battleParameter.PhysicalDefenseIncr);
    this.setBDTextWrraper(this.txt_DexterityBD, battleParameter.HitIncr);
    this.setBDTextWrraper(this.txt_EvasionBD, battleParameter.EvasionIncr);
    this.setBDTextWrraper(this.txt_MatkBD, battleParameter.MagicAttackIncr);
    this.setBDTextWrraper(this.txt_MdefBD, battleParameter.MagicDefenseIncr);
    this.SetPrincessType(v);
    int num1 = 0;
    foreach (UnitParameter.SkillSortUnit sortedSkill in new UnitParameter(v).sortedSkills)
    {
      if (this.dyn_Skill.Length > num1)
      {
        switch (sortedSkill.group)
        {
          case UnitParameter.SkillGroup.Leader:
            this.StartCoroutine(this.LoadLSSkillIcon(this.dyn_Skill[num1++], sortedSkill.leaderSkill));
            break;
          case UnitParameter.SkillGroup.Element:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.elementSkill);
            break;
          case UnitParameter.SkillGroup.Multi:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.multiSkill);
            break;
          case UnitParameter.SkillGroup.Overkillers:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.overkillersSkill);
            break;
          case UnitParameter.SkillGroup.Release:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.releaseSkill);
            break;
          case UnitParameter.SkillGroup.Command:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.commandSkill);
            break;
          case UnitParameter.SkillGroup.Princess:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.princessSkill);
            break;
          case UnitParameter.SkillGroup.Grant:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.grantSkill);
            break;
          case UnitParameter.SkillGroup.Duel:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.duelSkill);
            break;
          case UnitParameter.SkillGroup.Equip:
            this.createBattleSkillIcon(this.dyn_Skill[num1++], sortedSkill.equipSkill);
            break;
          case UnitParameter.SkillGroup.Extra:
            this.StartCoroutine(this.LoadExtraSkillIcon(this.dyn_Skill[num1++], sortedSkill.extraSkill));
            break;
          case UnitParameter.SkillGroup.JobAbility:
            this.createJobAbilityIcon(this.dyn_Skill[num1++], sortedSkill.jobAbility);
            break;
        }
      }
      else
        break;
    }
    if (v.weapon.gear != null)
    {
      GameObject gameObject = this.elementTypeIcon[0] = this.createIcon(this.elementTypeIconPrefab, this.elementTypeIconParent[0]);
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        if (v.weapon.gear.elements.Length != 0)
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(v.weapon.gear.elements[0].element);
        else
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(CommonElement.none);
        BL.Unit.GearRange gearRange = v.gearRange();
        this.setText(this.txt_Magic_range[0], string.Format("{0} - {1}", (object) gearRange.Min, (object) gearRange.Max));
        UILabel label = this.txt_Magic_cost == null || this.txt_Magic_cost.Length == 0 ? (UILabel) null : this.txt_Magic_cost[0];
        if (Object.op_Inequality((Object) label, (Object) null))
        {
          BL.MagicBullet bulletByGear = this.getBulletByGear(v.magicBullets, v.playerUnit.equippedGear);
          this.setText(label, bulletByGear != null ? bulletByGear.cost.ToString() : "-");
        }
      }
    }
    PlayerItem equippedGear2 = v.playerUnit.equippedGear2;
    if (equippedGear2 != (PlayerItem) null && equippedGear2.gear != null)
    {
      GameObject gameObject = this.elementTypeIcon[5] = this.createIcon(this.elementTypeIconPrefab, this.elementTypeIconParent[5]);
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        if (equippedGear2.gear.elements.Length != 0)
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(equippedGear2.gear.elements[0].element);
        else
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(CommonElement.none);
        BL.Unit.GearRange gearRange = v.gearRange();
        this.setText(this.txt_Magic_range[5], string.Format("{0} - {1}", (object) gearRange.Min, (object) gearRange.Max));
        UILabel label = this.txt_Magic_cost == null || this.txt_Magic_cost.Length <= 5 ? (UILabel) null : this.txt_Magic_cost[5];
        if (Object.op_Inequality((Object) label, (Object) null))
        {
          BL.MagicBullet bulletByGear = this.getBulletByGear(v.magicBullets, equippedGear2);
          this.setText(label, bulletByGear != null ? bulletByGear.cost.ToString() : "-");
        }
      }
    }
    PlayerItem equippedGear3 = v.playerUnit.equippedGear3;
    if (equippedGear3 != (PlayerItem) null && equippedGear3.gear != null)
    {
      GameObject gameObject = this.elementTypeIcon[5] = this.createIcon(this.elementTypeIconPrefab, this.elementTypeIconParent[5]);
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        if (equippedGear3.gear.elements.Length != 0)
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(equippedGear3.gear.elements[0].element);
        else
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(CommonElement.none);
        BL.Unit.GearRange gearRange = v.gearRange();
        this.setText(this.txt_Magic_range[5], string.Format("{0} - {1}", (object) gearRange.Min, (object) gearRange.Max));
        UILabel label = this.txt_Magic_cost == null || this.txt_Magic_cost.Length <= 5 ? (UILabel) null : this.txt_Magic_cost[5];
        if (Object.op_Inequality((Object) label, (Object) null))
        {
          BL.MagicBullet bulletByGear = this.getBulletByGear(v.magicBullets, equippedGear3);
          this.setText(label, bulletByGear != null ? bulletByGear.cost.ToString() : "-");
        }
      }
    }
    if (v.playerUnit.skills != null)
    {
      this.dispMagicBullets = new List<PlayerUnitSkills>();
      for (int index = 1; index < 5; ++index)
      {
        this.setText(this.txt_Magic_range[index], "-");
        this.setText(this.txt_Magic_cost, index, "-");
      }
      int index1 = 1;
      foreach (PlayerUnitSkills magicSkill in v.playerUnit.magicSkills)
      {
        PlayerUnitSkills sk = magicSkill;
        if (this.elementTypeIconParent.Length > index1)
        {
          this.dispMagicBullets.Add(sk);
          this.elementTypeIcon[index1] = this.createIcon(this.elementTypeIconPrefab, this.elementTypeIconParent[index1]);
          this.elementTypeIcon[index1].GetComponentInChildren<CommonElementIcon>().Init(sk.skill.element);
          int num2 = sk.skill.min_range;
          int num3 = sk.skill.max_range;
          BL.MagicBullet mb = Array.Find<BL.MagicBullet>(v.magicBullets, (Predicate<BL.MagicBullet>) (x => x.skillId == sk.skill.ID));
          int num4 = 0;
          if (mb != null)
          {
            BL.Unit.MagicRange magicRange = v.magicRange(mb);
            num2 = magicRange.Min;
            num3 = magicRange.Max;
            num4 = mb.cost;
          }
          this.setText(this.txt_Magic_range[index1], string.Format("{0} - {1}", (object) num2, (object) num3));
          if (num4 > 0)
            this.setText(this.txt_Magic_cost, index1, num4.ToString());
          ++index1;
        }
        else
          break;
      }
    }
    if (this.isEnabledGearLayout_)
    {
      PlayerUnit playerUnit = v.playerUnit;
      HashSet<int> setKind = new HashSet<int>();
      int index2 = 0;
      GearGear gear1 = playerUnit.equippedGear?.gear;
      CommonElement element1 = CommonElement.none;
      int level1 = 1;
      GearKind k1;
      if (gear1 != null)
      {
        k1 = gear1.kind;
        if (gear1.elements != null && gear1.elements.Length != 0)
          element1 = gear1.elements[0].element;
      }
      else
        k1 = playerUnit.unit.kind;
      PlayerUnitGearProficiency unitGearProficiency1 = playerUnit.gear_proficiencies != null ? Array.Find<PlayerUnitGearProficiency>(playerUnit.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == k1.ID)) : (PlayerUnitGearProficiency) null;
      if (unitGearProficiency1 != null)
        level1 = unitGearProficiency1.level;
      this.gearLayouts_[index2].reset(gear1, element1, level1);
      if (gear1 == null)
        this.gearLayouts_[index2].setKind(k1);
      this.queGearInit_.Enqueue(this.gearLayouts_[index2].initAsync());
      setKind.Add(k1.ID);
      int index3 = index2 + 1;
      GearGear gear2 = playerUnit.equippedGear2?.gear;
      CommonElement element2 = CommonElement.none;
      int level2 = 1;
      GearKind k2;
      if (gear2 != null)
      {
        k2 = gear2.kind;
        if (gear2.elements != null && gear2.elements.Length != 0)
          element2 = gear2.elements[0].element;
      }
      else
        k2 = (GearKind) null;
      PlayerUnitGearProficiency unitGearProficiency2 = playerUnit.gear_proficiencies == null || k2 == null ? (PlayerUnitGearProficiency) null : Array.Find<PlayerUnitGearProficiency>(playerUnit.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == k2.ID));
      if (unitGearProficiency2 != null)
        level2 = unitGearProficiency2.level;
      else if (playerUnit.gear_proficiencies != null && k2 == null)
      {
        PlayerUnitGearProficiency unitGearProficiency3 = Array.Find<PlayerUnitGearProficiency>(playerUnit.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => !setKind.Contains(x.gear_kind_id)));
        if (unitGearProficiency3 != null)
        {
          level2 = unitGearProficiency3.level;
          MasterData.GearKind.TryGetValue(unitGearProficiency3.gear_kind_id, out k2);
        }
      }
      this.gearLayouts_[index3].reset(gear2, element2, level2);
      bool awakeUnitFlag = playerUnit.unit.awake_unit_flag;
      if (awakeUnitFlag && gear2 == null)
      {
        if (k2 == null)
          k2 = MasterData.GearKind[7];
        this.gearLayouts_[index3].setKind(k2);
      }
      this.queGearInit_.Enqueue(this.gearLayouts_[index3].initAsync(awakeUnitFlag));
    }
    this.StartCoroutine(this.lazyInitializeAsync(v));
  }

  private GameObject createIcon(GameObject prefab, Transform trans)
  {
    if (Object.op_Equality((Object) trans, (Object) null))
      return (GameObject) null;
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    UI2DSprite componentInChildren2 = ((Component) trans).GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren1).SetDimensions(((UIWidget) componentInChildren2).width, ((UIWidget) componentInChildren2).height);
    ((UIWidget) componentInChildren1).depth = ((UIWidget) this.backGround.GetComponentInChildren<UISprite>()).depth + 100;
    return icon;
  }

  private void SetActiveForceObjects(BL.Unit v)
  {
    foreach (GameObject gameObject in this.dir_ForceHeader)
      gameObject.SetActive(false);
    foreach (GameObject dirForceHpObject in this.dir_ForceHpObjects)
      dirForceHpObject.SetActive(false);
    foreach (GameObject dirForceJobObject in this.dir_ForceJobObjects)
      dirForceJobObject.SetActive(false);
    this.dir_ForceHeader[(int) this.forceId].SetActive(true);
    if (this.dir_ForceHpObjects.Length != 0)
      this.dir_ForceHpObjects[(int) this.forceId].SetActive(true);
    if (this.dir_ForceJobObjects.Length == 0)
      return;
    this.dir_ForceJobObjects[(int) this.forceId].SetActive(true);
  }

  private BL.MagicBullet getBulletByGear(BL.MagicBullet[] bullets, PlayerItem gear)
  {
    if (bullets == null || bullets.Length == 0 || gear == (PlayerItem) null)
      return (BL.MagicBullet) null;
    BattleskillSkill gs = Array.Find<GearGearSkill>(gear.skills, (Predicate<GearGearSkill>) (s => s.skill.skill_type == BattleskillSkillType.magic))?.skill;
    return gs == null ? (BL.MagicBullet) null : Array.Find<BL.MagicBullet>(bullets, (Predicate<BL.MagicBullet>) (s => s.skillId == gs.ID));
  }

  private void setText(UILabel[] labels, int index, string v)
  {
    if (labels == null || labels.Length <= index || !Object.op_Inequality((Object) labels[index], (Object) null))
      return;
    labels[index].SetTextLocalize(v);
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

  private void createJobAbilityIcon(GameObject parent, PlayerUnitSkills skill)
  {
    this.StartCoroutine(this.LoadLSSkillIcon(parent, skill.skill));
    this.setIconJobAbilityEvent(parent, skill);
  }

  private IEnumerator LoadLSSkillIcon(GameObject parent, BattleskillSkill in_skill)
  {
    BattleFuncs.InvestSkill skill = new BattleFuncs.InvestSkill();
    skill.skill = in_skill;
    skill.isEnemyIcon = this.pUnit.playerUnit.is_enemy;
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
    Tower029EnemyInfoPopup tower029EnemyInfoPopup = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      tower029EnemyInfoPopup.setIconEvent(parent, in_skill.skill, in_skill.level);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) tower029EnemyInfoPopup.StartCoroutine(tower029EnemyInfoPopup.LoadLSSkillIcon(parent, in_skill.skill));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
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
    this.setIconEvent(parent, awakeSkill.masterData, awakeSkill.level);
  }

  private void setIconEvent(GameObject obj, BattleskillSkill skill, int level)
  {
    EventDelegate.Add(obj.GetComponentInChildren<UIButton>().onClick, (EventDelegate.Callback) (() => this.onButtonIcon(skill, level)));
  }

  public void onButtonIcon(BattleskillSkill skill, int level)
  {
    this.hideJobAbilityDialog();
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

  private void setIconJobAbilityEvent(GameObject obj, PlayerUnitSkills skill)
  {
    UIButton component = obj.GetComponent<UIButton>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    EventDelegate.Set(component.onClick, (EventDelegate.Callback) (() => this.onClickedJobAbility(skill)));
  }

  private void onClickedJobAbility(PlayerUnitSkills skill)
  {
    if (skill == null)
      return;
    this.hideSkillDialog();
    if (Object.op_Equality((Object) this.abilityDialog, (Object) null))
      return;
    ((Component) this.abilityDialog).gameObject.SetActive(true);
    this.abilityDialog.isAutoClose = true;
    this.abilityDialog.show(skill);
  }

  private void hideJobAbilityDialog()
  {
    if (!Object.op_Inequality((Object) this.abilityDialog, (Object) null) || !((Component) this.abilityDialog).gameObject.activeSelf)
      return;
    this.abilityDialog.hide();
  }

  private void hideSkillDialog()
  {
    if (!Object.op_Inequality((Object) this.skillDialog, (Object) null) || !this.skillDialog.activeSelf)
      return;
    this.skillDialog.GetComponentInChildren<Battle0171111Event>(true).Hide();
  }

  private void SetPrincessType(BL.Unit blUnit)
  {
    if (Singleton<NGBattleManager>.GetInstance().isEarth)
      ((Component) this.princessTypeSprite).gameObject.SetActive(false);
    else if (blUnit.playerUnit.unit_type == null || blUnit.playerUnit.is_enemy)
    {
      ((Component) this.princessTypeSprite).gameObject.SetActive(false);
    }
    else
    {
      string str1 = "slc_Princess_";
      string str2;
      switch (blUnit.playerUnit.unit_type.Enum)
      {
        case UnitTypeEnum.ouki:
          str2 = str1 + "King";
          break;
        case UnitTypeEnum.meiki:
          str2 = str1 + "Life";
          break;
        case UnitTypeEnum.kouki:
          str2 = str1 + "Attack";
          break;
        case UnitTypeEnum.maki:
          str2 = str1 + "Magic";
          break;
        case UnitTypeEnum.syuki:
          str2 = str1 + "Defense";
          break;
        case UnitTypeEnum.syouki:
          str2 = str1 + "Technical";
          break;
        default:
          Debug.LogWarning((object) "タイプ不一致");
          return;
      }
      ((Component) this.princessTypeSprite).gameObject.SetActive(true);
      this.princessTypeSprite.spriteName = str2 + ".png__GUI__princess_type__princess_type_prefab";
      ((UIWidget) this.princessTypeSprite).width = this.princessTypeSprite.GetAtlasSprite().width;
      ((UIWidget) this.princessTypeSprite).height = this.princessTypeSprite.GetAtlasSprite().height;
    }
  }

  private IEnumerator lazyInitializeAsync(BL.Unit v)
  {
    IEnumerator q;
    if (this.queGearInit_ != null)
    {
      while (this.queGearInit_.Any<IEnumerator>())
      {
        q = this.queGearInit_.Dequeue();
        while (q.MoveNext())
          yield return q.Current;
        q = (IEnumerator) null;
      }
    }
    Future<GameObject> future = v.unit.LoadMypage();
    q = future.Wait();
    while (q.MoveNext())
      yield return q.Current;
    q = (IEnumerator) null;
    this.characterTransform.Clear();
    GameObject result = future.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      int nextDepth = NGUITools.CalculateNextDepth(((Component) this.characterTransform).gameObject);
      GameObject go = result.Clone(this.characterTransform);
      go.GetComponent<NGxMaskSpriteWithScale>().scale = 0.6f;
      q = v.unit.SetLargeSpriteWithMask(v.playerUnit.job_id, go, Res.GUI.battleUI_03.mask_Character.Load<Texture2D>(), nextDepth, 190, 25);
      while (q.MoveNext())
        yield return q.Current;
      q = (IEnumerator) null;
    }
  }

  private ResourceObject maskResource() => Res.GUI.battleUI_04.mask_Character_Own;

  public void onClose() => Singleton<PopupManager>.GetInstance().dismiss();

  public override void onBackButton() => this.onClose();

  public void onButtonMB1()
  {
    this.hideJobAbilityDialog();
    if (this.dispMagicBullets.Count < 1 || Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) null, (Object) componentInChildren))
      return;
    componentInChildren.setData(this.dispMagicBullets[0].skill);
    componentInChildren.setSkillLv(this.dispMagicBullets[0].level, this.dispMagicBullets[0].skill.upper_level);
    componentInChildren.Show();
  }

  public void onButtonMB2()
  {
    if (this.dispMagicBullets.Count < 2 || Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) null, (Object) componentInChildren))
      return;
    componentInChildren.setData(this.dispMagicBullets[1].skill);
    componentInChildren.setSkillLv(this.dispMagicBullets[1].level, this.dispMagicBullets[1].skill.upper_level);
    componentInChildren.Show();
  }

  public void onButtonMB3()
  {
    this.hideJobAbilityDialog();
    if (this.dispMagicBullets.Count < 3 || Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) null, (Object) componentInChildren))
      return;
    componentInChildren.setData(this.dispMagicBullets[2].skill);
    componentInChildren.setSkillLv(this.dispMagicBullets[2].level, this.dispMagicBullets[2].skill.upper_level);
    componentInChildren.Show();
  }

  public void onButtonMB4()
  {
    this.hideJobAbilityDialog();
    if (this.dispMagicBullets.Count < 4 || Object.op_Equality((Object) this.skillDialog, (Object) null))
      return;
    this.skillDialog.SetActive(true);
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) null, (Object) componentInChildren))
      return;
    componentInChildren.setData(this.dispMagicBullets[3].skill);
    componentInChildren.setSkillLv(this.dispMagicBullets[3].level, this.dispMagicBullets[3].skill.upper_level);
    componentInChildren.Show();
  }

  public void onButtonWP()
  {
  }

  private IEnumerator StartWeaponNameAnim()
  {
    yield return (object) new WaitForSeconds(0.5f);
    ((UITweener) this.tweenAlphaFirstWeapon).PlayForward();
    ((UITweener) this.tweenAlphaSecondWeapon).PlayForward();
  }

  private void SetGearNameAnimation(BL.Unit v)
  {
    if (Object.op_Equality((Object) this.tweenAlphaFirstWeapon, (Object) null) || Object.op_Equality((Object) this.tweenAlphaSecondWeapon, (Object) null))
      return;
    PlayerItem equippedGear = v.playerUnit.equippedGear;
    PlayerItem equippedGear2 = v.playerUnit.equippedGear2;
    ((Component) this.tweenAlphaFirstWeapon).gameObject.SetActive(true);
    ((Component) this.tweenAlphaSecondWeapon).gameObject.SetActive(equippedGear2 != (PlayerItem) null);
    ((Behaviour) this.tweenAlphaFirstWeapon).enabled = false;
    ((Behaviour) this.tweenAlphaSecondWeapon).enabled = false;
    if (equippedGear != (PlayerItem) null && equippedGear2 != (PlayerItem) null)
    {
      ((UITweener) this.tweenAlphaFirstWeapon).ResetToBeginning();
      ((UITweener) this.tweenAlphaSecondWeapon).ResetToBeginning();
      this.StartCoroutine(this.StartWeaponNameAnim());
    }
    else if (v.playerUnit.unit.awake_unit_flag)
    {
      if (equippedGear == (PlayerItem) null && equippedGear2 != (PlayerItem) null)
      {
        ((Component) this.tweenAlphaFirstWeapon).gameObject.SetActive(false);
        ((UIRect) ((Component) this.tweenAlphaSecondWeapon).GetComponent<UIWidget>()).alpha = 1f;
      }
      else
      {
        if (!(equippedGear != (PlayerItem) null) || !(equippedGear2 == (PlayerItem) null))
          return;
        ((Component) this.tweenAlphaSecondWeapon).gameObject.SetActive(false);
        ((UIRect) ((Component) this.tweenAlphaFirstWeapon).GetComponent<UIWidget>()).alpha = 1f;
      }
    }
    else
    {
      ((UIRect) ((Component) this.tweenAlphaFirstWeapon).GetComponent<UIWidget>()).alpha = 1f;
      ((UIRect) ((Component) this.tweenAlphaSecondWeapon).GetComponent<UIWidget>()).alpha = 0.0f;
    }
  }

  private void setText(UILabel label, string v) => label.SetTextLocalize(v);

  private void setText(UILabel label, int v) => label.SetTextLocalize(string.Concat((object) v));

  public void setBDTextWrraper(UILabel label, int v)
  {
    if (v > 0)
    {
      label.SetTextLocalize(Mathf.Abs(v));
      label.SetText("( +" + label.text + " )");
      ((UIWidget) label).color = this.mGreen;
    }
    else if (v < 0)
    {
      label.SetTextLocalize(Mathf.Abs(v));
      label.SetText("( -" + label.text + " )");
      ((UIWidget) label).color = this.mRed;
    }
    else
      label.SetText(" ");
  }

  private void setColordText(UILabel label, int v, int bd)
  {
    this.setParentText(label, v);
    if (bd > 0)
    {
      ((UIWidget) label).color = this.mGreen;
    }
    else
    {
      if (bd >= 0)
        return;
      ((UIWidget) label).color = this.mRed;
    }
  }

  private void setParentText(UILabel label, int v)
  {
    if (v >= 0)
    {
      label.SetTextLocalize(string.Concat((object) v));
    }
    else
    {
      if (v >= 0)
        return;
      label.SetTextLocalize(Mathf.Abs(v));
      label.SetText("-" + label.text);
    }
  }

  public enum Force
  {
    Player,
    Neutral,
    Enemy,
  }

  [Serializable]
  private class GearLayout
  {
    public GameObject objEnabled_;
    public GameObject objDisabled_;
    public Transform lnkIcon_;
    public Transform lnkProficiencyKind_;
    public Transform lnkProficiency_;
    [NonSerialized]
    public ItemIcon gear_;
    [NonSerialized]
    public GearKindIcon proficiencyKind_;
    [NonSerialized]
    public GearProfiencyIcon proficiency_;
    private GearGear masterGear_;
    private GearKind gearKind_;
    private CommonElement element_ = CommonElement.none;
    private int level_;

    public void disabled()
    {
      this.objEnabled_.SetActive(false);
      if (!Object.op_Inequality((Object) this.objDisabled_, (Object) null))
        return;
      this.objDisabled_.SetActive(true);
    }

    public void reset(GearGear gear, CommonElement element, int level)
    {
      this.masterGear_ = gear;
      this.gearKind_ = gear?.kind;
      this.element_ = element;
      this.level_ = level;
      this.objEnabled_.SetActive(false);
      if (!Object.op_Inequality((Object) this.objDisabled_, (Object) null))
        return;
      this.objEnabled_.SetActive(true);
    }

    public void setKind(GearKind kind) => this.gearKind_ = kind;

    public IEnumerator initAsync(bool displayEmpty = false)
    {
      bool flag = this.masterGear_ != null;
      if (Object.op_Inequality((Object) this.objDisabled_, (Object) null))
        this.objDisabled_.SetActive(!displayEmpty && !flag);
      if (!displayEmpty && !flag && Object.op_Inequality((Object) this.objDisabled_, (Object) null))
      {
        this.objEnabled_.SetActive(false);
      }
      else
      {
        this.objEnabled_.SetActive(true);
        if (this.masterGear_ != null)
        {
          IEnumerator e = this.gear_.InitByGear(this.masterGear_, this.element_);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          this.gear_.SetModeGear();
          this.gear_.SetEmpty(true);
        }
        this.gear_.BottomModeValue = ItemIcon.BottomMode.Visible_wIconNone;
        if (this.gearKind_ != null)
        {
          this.proficiencyKind_.Init(this.gearKind_, this.element_);
          this.proficiency_.Init(this.level_);
        }
      }
    }
  }
}
