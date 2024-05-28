// Decompiled with JetBrains decompiler
// Type: Battle03UIPlayerStatus
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
public class Battle03UIPlayerStatus : Battle02MenuBase
{
  private const int WEAPON_SKILL_ICON_ID = 10;
  private const int WEAPON_ELEMENT_ICON_ID = 0;
  private const int SECOND_WEAPON_ELEMENT_ICON_ID = 5;
  private const string MAGIC_BULLET_NONE_NAME = "-";
  private const int ACTIVE_SPA_NUM = 6;
  private const int CELL_WIDTH_SINGLE_WEAPON = 65;
  private const int CELL_WIDTH_DUAL_WEAPON = 54;
  private const int LINE_NUM_SINGLE_WEAPON = 6;
  private const int LINE_NUM_DUAL_WEAPON = 7;
  private const int DIR_FORCEHEADER_PLAYER = 0;
  private const int SLC_STATUS_BASE_OWN_ICON_PLAYER = 0;
  private const int SLC_STATUS_BASE_OWN_ICON_GUEST = 1;
  public NGTweenGaugeScale hpGauge;
  [SerializeField]
  private UI2DSprite iconWeaponType_;
  [SerializeField]
  protected GameObject[] dir_ForceHeader;
  [SerializeField]
  protected GameObject[] dir_ForceHpObjects;
  [SerializeField]
  protected GameObject[] dir_ForceJobObjects;
  [SerializeField]
  private GameObject[] slcStatus1BaseOwnIcons;
  private GameObject gearKindIcon01;
  private GameObject gearKindIcon02;
  [SerializeField]
  protected NGxMaskSprite link_CharacterMask;
  [SerializeField]
  private Transform characterTransform;
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
  protected Transform weaponGearKindIconParent;
  [SerializeField]
  protected Transform shieldGearKindIconParent;
  protected GameObject gearKindIconPrefab;
  [SerializeField]
  protected Transform weaponEquipKindIconParent;
  private GameObject weaponEquipIcon;
  [SerializeField]
  protected Transform secondWeaponEquipKindIconParent;
  private GameObject secondWeaponEquipIcon;
  [SerializeField]
  private UISprite princessTypeSprite;
  protected GameObject skillTypeIconPrefab;
  private List<PlayerUnitSkills> dispMagicBullets;
  [SerializeField]
  protected Transform[] elementTypeIconParent;
  protected GameObject elementTypeIconPrefab;
  private GameObject[] elementTypeIcon = new GameObject[6];
  [SerializeField]
  protected UILabel[] txt_Magic_name;
  [SerializeField]
  protected UILabel[] txt_Magic_range;
  [SerializeField]
  protected UILabel[] txt_Magic_cost;
  private bool enabledSPA_;
  [SerializeField]
  protected Transform[] spaTypeIconParent1;
  [SerializeField]
  protected Transform[] spaTypeIconParent2;
  protected GameObject spaTypeIconPrefab;
  private GameObject[] spaTypeIcon1;
  private GameObject[] spaTypeIcon2;
  [SerializeField]
  protected Transform gearProfiencyIconParentW;
  [SerializeField]
  protected Transform gearProfiencyIconParentS;
  protected GameObject gearProfiencyIconPrefab;
  private GameObject gearProfiencyIconW;
  private GameObject gearProfiencyIconS;
  protected GameObject skillDialogPrefab;
  private GameObject skillDialog;
  [SerializeField]
  private Transform abilityDialogParent;
  private GameObject abilityDialogPrefab;
  private DialogJobAbilityDetail abilityDialog;
  [SerializeField]
  protected GameObject backGround;
  [SerializeField]
  protected GameObject SkillDialogRoot;
  private new BL.BattleModified<BL.Unit> modified;
  [SerializeField]
  protected TweenAlpha tweenAlphaFirstWeapon;
  [SerializeField]
  protected TweenAlpha tweenAlphaSecondWeapon;
  [SerializeField]
  private UISprite slcCountry;
  [SerializeField]
  private UI2DSprite slcInclusion;
  [SerializeField]
  private UIButton ibtn_SkillList;
  private GameObject skillListPrefab;
  [SerializeField]
  protected GameObject[] dyn_Skill;
  [SerializeField]
  protected GameObject[] slc_Skill_none;
  protected List<int> setSkills = new List<int>();
  private bool isEnabledGearLayout_;
  [SerializeField]
  private Battle03UIPlayerStatus.GearLayout[] gearLayouts_;
  private GameObject gearIconPrefab_;
  private Queue<IEnumerator> queGearInit_;
  private bool fromHistoryPanel;
  private bool isInitFromUnit;
  private BL.Unit pUnit;
  private int isPlayerUnit;

  protected override IEnumerator Start_Battle()
  {
    Battle03UIPlayerStatus battle03UiPlayerStatus = this;
    if (!battle03UiPlayerStatus.fromHistoryPanel && !battle03UiPlayerStatus.isInitFromUnit)
      battle03UiPlayerStatus.setUnit(battle03UiPlayerStatus.env.core.unitCurrent.unit);
    else
      battle03UiPlayerStatus.setUnit(battle03UiPlayerStatus.pUnit);
    Future<GameObject> gearKindIconPrefabF = Res.Icons.GearKindIcon.Load<GameObject>();
    IEnumerator e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle03UiPlayerStatus.gearKindIconPrefab = gearKindIconPrefabF.Result;
    Future<GameObject> skillTypeIconPrefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = skillTypeIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle03UiPlayerStatus.skillTypeIconPrefab = skillTypeIconPrefabF.Result;
    Future<GameObject> elementTypeIconPrefabF = Res.Icons.CommonElementIcon.Load<GameObject>();
    e = elementTypeIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle03UiPlayerStatus.elementTypeIconPrefab = elementTypeIconPrefabF.Result;
    battle03UiPlayerStatus.enabledSPA_ = battle03UiPlayerStatus.spaTypeIconParent1 != null && battle03UiPlayerStatus.spaTypeIconParent1.Length == 6;
    Future<GameObject> spaTypeIconPrefabF;
    if (battle03UiPlayerStatus.enabledSPA_)
    {
      spaTypeIconPrefabF = Res.Icons.SPAtkTypeIcon.Load<GameObject>();
      e = spaTypeIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle03UiPlayerStatus.spaTypeIconPrefab = spaTypeIconPrefabF.Result;
      battle03UiPlayerStatus.spaTypeIcon1 = new GameObject[6];
      battle03UiPlayerStatus.spaTypeIcon2 = new GameObject[6];
      spaTypeIconPrefabF = (Future<GameObject>) null;
    }
    Future<GameObject> gearProfiencyIconPrefabF = Res.Icons.GearProfiencyIcon.Load<GameObject>();
    e = gearProfiencyIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle03UiPlayerStatus.gearProfiencyIconPrefab = gearProfiencyIconPrefabF.Result;
    Future<GameObject> skillDialogPrefabF = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
    e = skillDialogPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle03UiPlayerStatus.skillDialogPrefab = skillDialogPrefabF.Result;
    battle03UiPlayerStatus.skillDialog = battle03UiPlayerStatus.skillDialogPrefab.Clone(battle03UiPlayerStatus.SkillDialogRoot.transform);
    battle03UiPlayerStatus.skillDialog.GetComponentInChildren<UIPanel>().depth += 30;
    battle03UiPlayerStatus.skillDialog.SetActive(false);
    if (Object.op_Inequality((Object) battle03UiPlayerStatus.abilityDialogParent, (Object) null))
    {
      spaTypeIconPrefabF = (Future<GameObject>) null;
      spaTypeIconPrefabF = Singleton<NGGameDataManager>.GetInstance().IsSea ? new ResourceObject("Prefabs/battle017_11_1_1/AbilityDetailDialog_sea").Load<GameObject>() : new ResourceObject("Prefabs/battle017_11_1_1/AbilityDetailDialog").Load<GameObject>();
      e = spaTypeIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle03UiPlayerStatus.abilityDialogPrefab = spaTypeIconPrefabF.Result;
      GameObject gameObject = battle03UiPlayerStatus.abilityDialogPrefab.Clone(battle03UiPlayerStatus.abilityDialogParent);
      UIWidget component1 = ((Component) battle03UiPlayerStatus.abilityDialogParent).GetComponent<UIWidget>();
      UIPanel component2 = gameObject.GetComponent<UIPanel>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        component2.depth += Object.op_Inequality((Object) component1, (Object) null) ? component1.depth + 1 : 30;
      battle03UiPlayerStatus.abilityDialog = gameObject.GetComponent<DialogJobAbilityDetail>();
      ((Component) battle03UiPlayerStatus.abilityDialog).gameObject.SetActive(false);
      spaTypeIconPrefabF = (Future<GameObject>) null;
    }
    spaTypeIconPrefabF = !Object.op_Inequality((Object) battle03UiPlayerStatus.battleManager, (Object) null) || !battle03UiPlayerStatus.battleManager.isSea ? Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/battle017_11_1_1/popup_SkillList") : Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/battle017_11_1_1/popup_SkillList_sea");
    e = spaTypeIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle03UiPlayerStatus.skillListPrefab = spaTypeIconPrefabF.Result;
    spaTypeIconPrefabF = (Future<GameObject>) null;
    battle03UiPlayerStatus.gearKindIcon01 = battle03UiPlayerStatus.createIcon(battle03UiPlayerStatus.gearKindIconPrefab, battle03UiPlayerStatus.weaponGearKindIconParent);
    battle03UiPlayerStatus.gearKindIcon02 = battle03UiPlayerStatus.createIcon(battle03UiPlayerStatus.gearKindIconPrefab, battle03UiPlayerStatus.shieldGearKindIconParent);
    battle03UiPlayerStatus.weaponEquipIcon = battle03UiPlayerStatus.createIcon(battle03UiPlayerStatus.gearKindIconPrefab, battle03UiPlayerStatus.weaponEquipKindIconParent);
    battle03UiPlayerStatus.secondWeaponEquipIcon = battle03UiPlayerStatus.createIcon(battle03UiPlayerStatus.gearKindIconPrefab, battle03UiPlayerStatus.secondWeaponEquipKindIconParent);
    battle03UiPlayerStatus.setSkills.Clear();
    battle03UiPlayerStatus.isEnabledGearLayout_ = battle03UiPlayerStatus.gearLayouts_ != null && battle03UiPlayerStatus.gearLayouts_.Length != 0;
    if (battle03UiPlayerStatus.isEnabledGearLayout_)
    {
      battle03UiPlayerStatus.queGearInit_ = new Queue<IEnumerator>(battle03UiPlayerStatus.gearLayouts_.Length);
      spaTypeIconPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = spaTypeIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle03UiPlayerStatus.gearIconPrefab_ = spaTypeIconPrefabF.Result;
      spaTypeIconPrefabF = (Future<GameObject>) null;
      foreach (Battle03UIPlayerStatus.GearLayout gearLayout in battle03UiPlayerStatus.gearLayouts_)
      {
        gearLayout.gear_ = battle03UiPlayerStatus.createIcon(battle03UiPlayerStatus.gearIconPrefab_, gearLayout.lnkIcon_).GetComponent<ItemIcon>();
        gearLayout.proficiencyKind_ = battle03UiPlayerStatus.createIcon(battle03UiPlayerStatus.gearKindIconPrefab, gearLayout.lnkProficiencyKind_).GetComponent<GearKindIcon>();
        gearLayout.proficiency_ = battle03UiPlayerStatus.createIcon(battle03UiPlayerStatus.gearProfiencyIconPrefab, gearLayout.lnkProficiency_).GetComponent<GearProfiencyIcon>();
        gearLayout.disabled();
      }
    }
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

  protected override void LateUpdate_Battle()
  {
    if ((this.modified == null || !this.modified.isChangedOnce()) && !this.fromHistoryPanel)
      return;
    int num1 = this.txt_Magic_name == null ? 1 : (this.txt_Magic_name.Length == 0 ? 1 : 0);
    BL.Unit unit1 = this.fromHistoryPanel ? this.pUnit : this.modified.value;
    this.SetGearNameAnimation(unit1);
    this.SetActiveForceObjects(unit1);
    ((IEnumerable<GameObject>) this.slcStatus1BaseOwnIcons).ToggleOnce(unit1.playerUnit.is_guest ? 1 : 0);
    Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) unit1);
    UnitUnit unit2 = unit1.unit;
    if (Object.op_Inequality((Object) this.iconWeaponType_, (Object) null))
      this.iconWeaponType_.sprite2D = GearKindIcon.LoadSprite(unit2.kind.Enum, unit1.GetElement());
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
    PlayerItem equippedGear = unit1.playerUnit.equippedGear;
    PlayerItem equippedGear2_1 = unit1.playerUnit.equippedGear2;
    GearGear equippedGearOrInitial = unit1.playerUnit.equippedGearOrInitial;
    CommonElement element1 = equippedGearOrInitial.GetElement();
    if (equippedGear != (PlayerItem) null)
      element1 = equippedGear.GetElement();
    else if (equippedGear == (PlayerItem) null && equippedGear2_1 != (PlayerItem) null)
      element1 = equippedGear2_1.GetElement();
    if (Object.op_Inequality((Object) this.weaponEquipIcon, (Object) null))
      this.weaponEquipIcon.GetComponentInChildren<GearKindIcon>().Init(equippedGearOrInitial.kind, element1);
    if (equippedGear2_1 != (PlayerItem) null && Object.op_Inequality((Object) this.secondWeaponEquipIcon, (Object) null))
    {
      CommonElement element2 = equippedGear2_1.GetElement();
      this.secondWeaponEquipIcon.GetComponentInChildren<GearKindIcon>().Init(equippedGear2_1.gear.kind, element2);
    }
    if (!this.fromHistoryPanel)
    {
      this.hpGauge.setValue(unit1.hp, battleParameter.Hp, false);
      this.setText(this.txt_Hp, unit1.hp);
      if (Object.op_Inequality((Object) this.txt_Hp_Enemy, (Object) null))
        this.setText(this.txt_Hp_Enemy, unit1.hp);
    }
    else
    {
      this.hpGauge.setValue(battleParameter.Hp, battleParameter.Hp, false);
      if (Object.op_Inequality((Object) this.txt_Hp_Enemy, (Object) null))
        this.setText(this.txt_Hp_Enemy, battleParameter.Hp);
    }
    this.setText(this.txt_Hpmax, "/" + (object) battleParameter.Hp);
    if (Object.op_Inequality((Object) this.txt_Hpmax_Enemy, (Object) null))
      this.setText(this.txt_Hpmax_Enemy, "/" + (object) battleParameter.Hp);
    this.setText(this.txt_CharacterName, string.IsNullOrEmpty(unit2.formal_name) ? unit2.name : unit2.formal_name);
    this.setText(this.txt_Lv, unit1.lv);
    this.setColordText(this.txt_Fighting, battleParameter.Combat, battleParameter.CombatIncr);
    this.setText(this.txt_Jobname, unit1.job.name);
    if (Object.op_Inequality((Object) this.txt_Jobname_Enemy, (Object) null))
      this.setText(this.txt_Jobname_Enemy, unit1.job.name);
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
    this.SetPrincessType(unit1);
    int index1 = 0;
    foreach (UnitParameter.SkillSortUnit sortedSkill in new UnitParameter(unit1).sortedSkills)
    {
      if (this.dyn_Skill.Length > index1)
      {
        this.setActiveSkillNone(index1, false);
        switch (sortedSkill.group)
        {
          case UnitParameter.SkillGroup.Leader:
            this.StartCoroutine(this.LoadLSSkillIcon(this.dyn_Skill[index1++], sortedSkill.leaderSkill));
            break;
          case UnitParameter.SkillGroup.Element:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.elementSkill);
            break;
          case UnitParameter.SkillGroup.Multi:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.multiSkill);
            break;
          case UnitParameter.SkillGroup.Overkillers:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.overkillersSkill);
            break;
          case UnitParameter.SkillGroup.Release:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.releaseSkill);
            break;
          case UnitParameter.SkillGroup.Command:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.commandSkill);
            break;
          case UnitParameter.SkillGroup.Princess:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.princessSkill);
            break;
          case UnitParameter.SkillGroup.Grant:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.grantSkill);
            break;
          case UnitParameter.SkillGroup.Duel:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.duelSkill);
            break;
          case UnitParameter.SkillGroup.Equip:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.equipSkill);
            break;
          case UnitParameter.SkillGroup.Extra:
            this.StartCoroutine(this.LoadExtraSkillIcon(this.dyn_Skill[index1++], sortedSkill.extraSkill));
            break;
          case UnitParameter.SkillGroup.JobAbility:
            this.createJobAbilityIcon(this.dyn_Skill[index1++], sortedSkill.jobAbility);
            break;
          case UnitParameter.SkillGroup.Reisou:
            this.createBattleSkillIcon(this.dyn_Skill[index1++], sortedSkill.reisouSkill);
            break;
        }
      }
      else
        break;
    }
    int index2;
    if (unit1.weapon.gear != null)
    {
      GameObject gameObject = this.elementTypeIcon[0] = this.createIcon(this.elementTypeIconPrefab, this.elementTypeIconParent[0]);
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        if (unit1.weapon.gear.elements.Length != 0)
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(unit1.weapon.gear.elements[0].element);
        else
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(CommonElement.none);
        this.setText(this.txt_Magic_name, 0, unit1.playerUnit.equippedGearName);
        BL.Unit.GearRange gearRange = unit1.gearRange();
        this.setText(this.txt_Magic_range[0], string.Format("{0} - {1}", (object) gearRange.Min, (object) gearRange.Max));
        UILabel uiLabel = this.txt_Magic_cost == null || this.txt_Magic_cost.Length == 0 ? (UILabel) null : this.txt_Magic_cost[0];
        if (Object.op_Inequality((Object) uiLabel, (Object) null))
        {
          BL.MagicBullet bulletByGear = this.getBulletByGear(unit1.magicBullets, unit1.playerUnit.equippedGear);
          UILabel label = uiLabel;
          string v;
          if (bulletByGear == null)
          {
            v = "-";
          }
          else
          {
            index2 = bulletByGear.cost;
            v = index2.ToString();
          }
          this.setText(label, v);
        }
        if (this.enabledSPA_)
        {
          UnitFamily[] specialAttackTargets = unit1.playerUnit.equippedWeaponGearOrInitial.SpecialAttackTargets;
          Transform[] transformArray = new Transform[2]
          {
            this.spaTypeIconParent1[0],
            this.spaTypeIconParent2[0]
          };
          GameObject[] gameObjectArray = new GameObject[2]
          {
            this.spaTypeIcon1[0],
            this.spaTypeIcon2[0]
          };
          for (int index3 = 0; index3 < specialAttackTargets.Length && transformArray.Length > index3; ++index3)
          {
            gameObjectArray[index3] = this.createIcon(this.spaTypeIconPrefab, transformArray[index3]);
            gameObjectArray[index3].GetComponentInChildren<SPAtkTypeIcon>().InitKindId(specialAttackTargets[index3]);
          }
        }
      }
    }
    PlayerItem equippedGear2_2 = unit1.playerUnit.equippedGear2;
    if (equippedGear2_2 != (PlayerItem) null && equippedGear2_2.gear != null)
    {
      GameObject gameObject = this.elementTypeIcon[5] = this.createIcon(this.elementTypeIconPrefab, this.elementTypeIconParent[5]);
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        if (equippedGear2_2.gear.elements.Length != 0)
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(equippedGear2_2.gear.elements[0].element);
        else
          gameObject.GetComponentInChildren<CommonElementIcon>().Init(CommonElement.none);
        this.setText(this.txt_Magic_name, 5, unit1.playerUnit.equippedGearName2);
        BL.Unit.GearRange gearRange = unit1.gearRange();
        this.setText(this.txt_Magic_range[5], string.Format("{0} - {1}", (object) gearRange.Min, (object) gearRange.Max));
        UILabel uiLabel = this.txt_Magic_cost == null || this.txt_Magic_cost.Length <= 5 ? (UILabel) null : this.txt_Magic_cost[5];
        if (Object.op_Inequality((Object) uiLabel, (Object) null))
        {
          BL.MagicBullet bulletByGear = this.getBulletByGear(unit1.magicBullets, equippedGear2_2);
          UILabel label = uiLabel;
          string v;
          if (bulletByGear == null)
          {
            v = "-";
          }
          else
          {
            index2 = bulletByGear.cost;
            v = index2.ToString();
          }
          this.setText(label, v);
        }
        if (this.enabledSPA_)
        {
          UnitFamily[] specialAttackTargets = equippedGear2_2.gear.SpecialAttackTargets;
          Transform[] transformArray = new Transform[2]
          {
            this.spaTypeIconParent1[5],
            this.spaTypeIconParent2[5]
          };
          GameObject[] gameObjectArray = new GameObject[2]
          {
            this.spaTypeIcon1[5],
            this.spaTypeIcon2[5]
          };
          for (int index4 = 0; index4 < specialAttackTargets.Length && transformArray.Length > index4; ++index4)
          {
            gameObjectArray[index4] = this.createIcon(this.spaTypeIconPrefab, transformArray[index4]);
            gameObjectArray[index4].GetComponentInChildren<SPAtkTypeIcon>().InitKindId(specialAttackTargets[index4]);
          }
        }
      }
    }
    if (Object.op_Inequality((Object) this.slcCountry, (Object) null))
    {
      ((Component) this.slcCountry).gameObject.SetActive(false);
      if (unit2.country_attribute.HasValue)
      {
        ((Component) this.slcCountry).gameObject.SetActive(true);
        unit2.SetCuntrySpriteName(ref this.slcCountry);
      }
    }
    if (Object.op_Inequality((Object) this.slcInclusion, (Object) null))
    {
      ((Component) this.slcInclusion).gameObject.SetActive(false);
      if (unit2.inclusion_ip.HasValue)
      {
        ((Component) this.slcInclusion).gameObject.SetActive(true);
        this.StartCoroutine(unit2.SetInclusionIP(this.slcInclusion));
      }
    }
    if (unit1.playerUnit.skills != null)
    {
      this.dispMagicBullets = new List<PlayerUnitSkills>();
      for (int index5 = 1; index5 < 5; ++index5)
      {
        this.setText(this.txt_Magic_name, index5, "-");
        this.setText(this.txt_Magic_range[index5], "-");
        this.setText(this.txt_Magic_cost, index5, "-");
      }
      int index6 = 1;
      PlayerUnitSkills[] magicSkills = unit1.playerUnit.magicSkills;
      for (index2 = 0; index2 < magicSkills.Length; ++index2)
      {
        PlayerUnitSkills sk = magicSkills[index2];
        if (this.elementTypeIconParent.Length > index6)
        {
          this.dispMagicBullets.Add(sk);
          this.elementTypeIcon[index6] = this.createIcon(this.elementTypeIconPrefab, this.elementTypeIconParent[index6]);
          this.elementTypeIcon[index6].GetComponentInChildren<CommonElementIcon>().Init(sk.skill.element);
          this.setText(this.txt_Magic_name, index6, sk.skill.name);
          int num2 = sk.skill.min_range;
          int num3 = sk.skill.max_range;
          BL.MagicBullet mb = Array.Find<BL.MagicBullet>(unit1.magicBullets, (Predicate<BL.MagicBullet>) (x => x.skillId == sk.skill.ID));
          int num4 = 0;
          if (mb != null)
          {
            BL.Unit.MagicRange magicRange = unit1.magicRange(mb);
            num2 = magicRange.Min;
            num3 = magicRange.Max;
            num4 = mb.cost;
          }
          this.setText(this.txt_Magic_range[index6], string.Format("{0} - {1}", (object) num2, (object) num3));
          if (num4 > 0)
            this.setText(this.txt_Magic_cost, index6, num4.ToString());
          ++index6;
        }
        else
          break;
      }
    }
    bool isAllEquipUnit = unit1.playerUnit.unit.IsAllEquipUnit;
    if (Object.op_Inequality((Object) this.gearKindIcon01, (Object) null) && unit1.playerUnit.gear_proficiencies != null && unit1.playerUnit.gear_proficiencies.Length >= 1)
    {
      this.gearKindIcon01.GetComponent<GearKindIcon>().Init(unit1.playerUnit.gear_proficiencies[0].gear_kind_id);
      this.gearProfiencyIconW = this.createIcon(this.gearProfiencyIconPrefab, this.gearProfiencyIconParentW);
      this.gearProfiencyIconW.GetComponentInChildren<GearProfiencyIcon>().Init(unit1.playerUnit.gear_proficiencies[0].level, isAllEquipUnit);
    }
    if (Object.op_Inequality((Object) this.gearKindIcon02, (Object) null) && unit1.playerUnit.gear_proficiencies != null && unit1.playerUnit.gear_proficiencies.Length >= 2)
    {
      this.gearKindIcon02.GetComponent<GearKindIcon>().Init(unit1.playerUnit.gear_proficiencies[1].gear_kind_id);
      this.gearProfiencyIconS = this.createIcon(this.gearProfiencyIconPrefab, this.gearProfiencyIconParentS);
      this.gearProfiencyIconS.GetComponentInChildren<GearProfiencyIcon>().Init(unit1.playerUnit.gear_proficiencies[1].level, isAllEquipUnit);
    }
    bool flag = false;
    if (unit1.playerUnit.equippedGear != (PlayerItem) null && unit1.playerUnit.equippedGear.skills.Length != 0)
      flag = true;
    if (unit1.playerUnit.equippedGear2 != (PlayerItem) null && unit1.playerUnit.equippedGear2.skills.Length > 2)
      flag = true;
    if (!this.setSkills.Any<int>() && !flag && Object.op_Inequality((Object) this.ibtn_SkillList, (Object) null))
      ((UIButtonColor) this.ibtn_SkillList).isEnabled = false;
    if (this.isEnabledGearLayout_)
    {
      PlayerUnit playerUnit = unit1.playerUnit;
      HashSet<int> setKind = new HashSet<int>();
      int index7 = 0;
      GearGear gear1 = playerUnit.equippedGear?.gear;
      CommonElement element3 = CommonElement.none;
      int level1 = 1;
      GearKind k1;
      if (gear1 != null)
      {
        k1 = gear1.kind;
        if (gear1.elements != null && gear1.elements.Length != 0)
          element3 = gear1.elements[0].element;
      }
      else
        k1 = playerUnit.unit.kind;
      PlayerUnitGearProficiency unitGearProficiency1 = playerUnit.gear_proficiencies != null ? Array.Find<PlayerUnitGearProficiency>(playerUnit.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == k1.ID)) : (PlayerUnitGearProficiency) null;
      if (unitGearProficiency1 != null)
        level1 = unitGearProficiency1.level;
      this.gearLayouts_[index7].reset(gear1, element3, level1, playerUnit.unit.IsAllEquipUnit);
      if (gear1 == null)
        this.gearLayouts_[index7].setKind(k1);
      this.queGearInit_.Enqueue(this.gearLayouts_[index7].initAsync());
      setKind.Add(k1.ID);
      int index8 = index7 + 1;
      GearGear gear2 = playerUnit.equippedGear2?.gear;
      CommonElement element4 = CommonElement.none;
      int level2 = 1;
      GearKind k2;
      if (gear2 != null)
      {
        k2 = gear2.kind;
        if (gear2.elements != null && gear2.elements.Length != 0)
          element4 = gear2.elements[0].element;
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
      this.gearLayouts_[index8].reset(gear2, element4, level2, playerUnit.unit.IsAllEquipUnit);
      bool awakeUnitFlag = playerUnit.unit.awake_unit_flag;
      if (awakeUnitFlag && gear2 == null)
      {
        if (k2 == null)
          k2 = MasterData.GearKind[7];
        this.gearLayouts_[index8].setKind(k2);
      }
      this.queGearInit_.Enqueue(this.gearLayouts_[index8].initAsync(awakeUnitFlag));
    }
    this.StartCoroutine(this.initializeAsync(unit1));
  }

  private void SetActiveForceObjects(BL.Unit v)
  {
    foreach (GameObject gameObject in this.dir_ForceHeader)
      gameObject.SetActive(false);
    foreach (GameObject dirForceHpObject in this.dir_ForceHpObjects)
      dirForceHpObject.SetActive(false);
    foreach (GameObject dirForceJobObject in this.dir_ForceJobObjects)
      dirForceJobObject.SetActive(false);
    if (!this.fromHistoryPanel)
    {
      this.dir_ForceHeader[(int) this.env.core.getForceID(v)].SetActive(true);
      if (this.dir_ForceHpObjects.Length != 0)
        this.dir_ForceHpObjects[(int) this.env.core.getForceID(v)].SetActive(true);
      if (this.dir_ForceJobObjects.Length == 0)
        return;
      this.dir_ForceJobObjects[(int) this.env.core.getForceID(v)].SetActive(true);
    }
    else
    {
      this.dir_ForceHeader[this.isPlayerUnit].SetActive(true);
      if (this.dir_ForceHpObjects.Length != 0)
        this.dir_ForceHpObjects[this.isPlayerUnit].SetActive(true);
      if (this.dir_ForceJobObjects.Length == 0)
        return;
      this.dir_ForceJobObjects[this.isPlayerUnit].SetActive(true);
    }
  }

  private void setActiveSkillNone(int index, bool flag)
  {
    if (this.slc_Skill_none == null || this.slc_Skill_none.Length <= index || Object.op_Equality((Object) this.slc_Skill_none[index], (Object) null))
      return;
    this.slc_Skill_none[index].SetActive(flag);
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

  private void createBattleSkillIcon(GameObject parent, GearReisouSkill skill)
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
    skill.isEnemyIcon = (this.fromHistoryPanel ? this.pUnit : this.modified.value).playerUnit.is_enemy;
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
    Battle03UIPlayerStatus battle03UiPlayerStatus = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      battle03UiPlayerStatus.setIconEvent(parent, in_skill.skill, in_skill.level);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) battle03UiPlayerStatus.StartCoroutine(battle03UiPlayerStatus.LoadLSSkillIcon(parent, in_skill.skill));
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
      ((Component) this.princessTypeSprite).gameObject.SetActive(false);
    else
      ((Component) this.princessTypeSprite).gameObject.SetActive(UnitTypeIcon.SetAtlasSprite(this.princessTypeSprite, (UnitTypeEnum) blUnit.playerUnit._unit_type));
  }

  public new void setUnit(BL.Unit unit) => this.modified = BL.Observe<BL.Unit>(unit);

  public new BL.Unit getUnit() => this.modified.value;

  private IEnumerator initializeAsync(BL.Unit v)
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

  public void onClose()
  {
    if (!this.fromHistoryPanel)
    {
      if (Object.op_Inequality((Object) this.battleManager, (Object) null))
        this.battleManager.popupDismiss();
      else
        Singleton<PopupManager>.GetInstance().dismiss();
    }
    else
      Singleton<PopupManager>.GetInstance().dismiss();
  }

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

  public IEnumerator InitFromHistory(BL.Unit playerUnit, bool isPlayer)
  {
    Battle03UIPlayerStatus battle03UiPlayerStatus = this;
    battle03UiPlayerStatus.fromHistoryPanel = true;
    battle03UiPlayerStatus.isPlayerUnit = !isPlayer ? 2 : 0;
    battle03UiPlayerStatus.pUnit = playerUnit;
    IEnumerator e = battle03UiPlayerStatus.Start_Battle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle03UiPlayerStatus.LateUpdate_Battle();
  }

  public IEnumerator InitFromPVP(BL.Unit playerUnit, bool isPlayer)
  {
    Battle03UIPlayerStatus battle03UiPlayerStatus = this;
    battle03UiPlayerStatus.fromHistoryPanel = false;
    battle03UiPlayerStatus.isPlayerUnit = !isPlayer ? 2 : 0;
    battle03UiPlayerStatus.pUnit = playerUnit;
    battle03UiPlayerStatus.env.core.setCurrentUnitWithSetOnly(playerUnit);
    IEnumerator e = battle03UiPlayerStatus.Start_Battle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ++battle03UiPlayerStatus.modified.value.revision;
    battle03UiPlayerStatus.LateUpdate_Battle();
    battle03UiPlayerStatus.battleManager.getManager<BattleTimeManager>().setCurrentUnit((BL.Unit) null);
  }

  public void InitFromUnit(BL.Unit unit)
  {
    this.pUnit = unit;
    this.isInitFromUnit = true;
  }

  public void onButtonSkillList()
  {
    if (this.fromHistoryPanel)
    {
      this.StartCoroutine(this.loadSkillList());
    }
    else
    {
      GameObject prefab = this.skillListPrefab.Clone();
      BattleUI01SkillList componentInChildren = prefab.GetComponentInChildren<BattleUI01SkillList>();
      if (Object.op_Implicit((Object) componentInChildren))
        componentInChildren.setData(this.modified);
      if (Object.op_Inequality((Object) this.battleManager, (Object) null))
        this.battleManager.popupOpen(prefab, isCloned: true, nonBattleEnableControl: true);
      else
        Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    }
  }

  private IEnumerator loadSkillList()
  {
    GameObject popup = this.skillListPrefab.Clone();
    BattleUI01SkillList componentInChildren = popup.GetComponentInChildren<BattleUI01SkillList>();
    if (Object.op_Implicit((Object) componentInChildren))
    {
      componentInChildren.setData(this.modified.value.playerUnit);
      IEnumerator e = componentInChildren.InitNotBattle();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
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

    public void reset(GearGear gear, CommonElement element, int level, bool isAllEquipUnit)
    {
      this.masterGear_ = gear;
      this.gearKind_ = !isAllEquipUnit ? gear?.kind : MasterData.GearKind[8];
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
