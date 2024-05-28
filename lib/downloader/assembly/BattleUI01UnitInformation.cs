// Decompiled with JetBrains decompiler
// Type: BattleUI01UnitInformation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class BattleUI01UnitInformation : BackButtonPopupBase
{
  private const string MAGIC_BULLET_NONE_NAME = "-";
  private const int SLC_STATUS_BASE_OWN_ICON_PLAYER = 0;
  private const int SLC_STATUS_BASE_OWN_ICON_GUEST = 1;
  [SerializeField]
  private Color mGreen = new Color(0.0f, 0.863f, 0.118f);
  [SerializeField]
  private Color mRed = new Color(0.98f, 0.0f, 0.0f);
  public NGTweenGaugeScale hpGauge;
  [SerializeField]
  private UI2DSprite iconWeaponType_;
  [SerializeField]
  [Tooltip("[自軍|中立|敵]の順")]
  private GameObject[] dir_ForceHeader;
  [SerializeField]
  [Tooltip("[自軍|中立|敵]の順")]
  private GameObject[] dir_ForceHpObjects;
  [SerializeField]
  [Tooltip("[自軍|中立|敵]の順")]
  private GameObject[] dir_ForceJobObjects;
  [SerializeField]
  [Tooltip("[自軍|ゲスト]の順")]
  private GameObject[] slcStatus1BaseOwnIcons;
  [SerializeField]
  private NGxMaskSprite link_CharacterMask;
  [SerializeField]
  private Transform characterTransform;
  [SerializeField]
  private UILabel txt_Attack;
  [SerializeField]
  private UILabel txt_Critical;
  [SerializeField]
  private UILabel txt_Defense;
  [SerializeField]
  private UILabel txt_Dexterity;
  [SerializeField]
  private UILabel txt_Evasion;
  [SerializeField]
  private UILabel txt_Fighting;
  [SerializeField]
  private UILabel txt_Matk;
  [SerializeField]
  private UILabel txt_Mdef;
  [SerializeField]
  private UILabel txt_Movement;
  [SerializeField]
  private UILabel txt_AttackBD;
  [SerializeField]
  private UILabel txt_CriticalBD;
  [SerializeField]
  private UILabel txt_DefenseBD;
  [SerializeField]
  private UILabel txt_DexterityBD;
  [SerializeField]
  private UILabel txt_EvasionBD;
  [SerializeField]
  private UILabel txt_MatkBD;
  [SerializeField]
  private UILabel txt_MdefBD;
  [SerializeField]
  private UILabel txt_Agility;
  [SerializeField]
  private UILabel txt_Luck;
  [SerializeField]
  private UILabel txt_Magic;
  [SerializeField]
  private UILabel txt_Power;
  [SerializeField]
  private UILabel txt_Stability;
  [SerializeField]
  private UILabel txt_Spirit;
  [SerializeField]
  private UILabel txt_Technique;
  [SerializeField]
  private UILabel txt_AgilityBD;
  [SerializeField]
  private UILabel txt_LuckBD;
  [SerializeField]
  private UILabel txt_MagicBD;
  [SerializeField]
  private UILabel txt_PowerBD;
  [SerializeField]
  private UILabel txt_StabilityBD;
  [SerializeField]
  private UILabel txt_SpiritBD;
  [SerializeField]
  private UILabel txt_TechniqueBD;
  [SerializeField]
  private UILabel txt_CharacterName;
  [SerializeField]
  private UILabel txt_Lv;
  [SerializeField]
  private UILabel txt_Hp;
  [SerializeField]
  private UILabel txt_Hpmax;
  [SerializeField]
  private UILabel txt_Jobname;
  [SerializeField]
  private UILabel txt_Hp_Enemy;
  [SerializeField]
  private UILabel txt_Hpmax_Enemy;
  [SerializeField]
  private UILabel txt_Jobname_Enemy;
  private GameObject gearKindIconPrefab;
  [SerializeField]
  private UISprite princessTypeSprite;
  private GameObject skillTypeIconPrefab;
  private GameObject skillLockIconPrefab;
  private GameObject elementTypeIconPrefab;
  private GameObject gearProfiencyIconPrefab;
  [SerializeField]
  [Tooltip("アイコンのdepth")]
  private int iconDepth_ = 183;
  [SerializeField]
  private GameObject SkillDialogRoot;
  private GameObject skillDialogPrefab;
  private GameObject skillDialog;
  private PopupSkillDetails.Param[] popupSkillParams_;
  [SerializeField]
  private Transform attackMethodParent;
  private GameObject attackMethodPrefab;
  private GameObject attackMethodDialog;
  private AttackClassIcon attackClassIcon;
  [SerializeField]
  private BattleUI01UnitInformation.GearLayout[] gearLayouts_;
  private GameObject gearIconPrefab_;
  [SerializeField]
  [Tooltip("\"スキル一覧|攻撃一覧|追加攻撃\"の順でセット")]
  private BattleUI01UnitInformation.TabControl[] tabs_ = new BattleUI01UnitInformation.TabControl[Enum.GetValues(typeof (BattleUI01UnitInformationTab.Type)).Length];
  private int currentTab_;
  private NGBattleManager battleManager_;
  private BE env_;
  private BL.Phase currentPhase_;
  private int metamorphosisGroupId_;
  private bool fromHistoryPanel_;
  private bool displayMaxHp_;
  private BL.Unit pUnit_;
  private int currentForce_;

  private NGBattleManager battleManager
  {
    get
    {
      return !Object.op_Inequality((Object) this.battleManager_, (Object) null) ? (this.battleManager_ = Singleton<NGBattleManager>.GetInstance()) : this.battleManager_;
    }
  }

  private BE env
  {
    get
    {
      return this.env_ == null ? (this.env_ = Object.op_Inequality((Object) this.battleManager, (Object) null) ? this.battleManager.environment : (BE) null) : this.env_;
    }
  }

  private void setText(UILabel label, string v) => label.SetTextLocalize(v);

  private void setText(UILabel label, object v) => label.SetTextLocalize(v.ToString());

  private void setBDTextWrraper(UILabel label, int v)
  {
    if (Object.op_Equality((Object) label, (Object) null))
      return;
    if (v == 0)
    {
      label.SetText(string.Empty);
    }
    else
    {
      string str = Mathf.Abs(v).ToString() + " )";
      if (v > 0)
      {
        label.SetTextLocalize("( +" + str);
        ((UIWidget) label).color = this.mGreen;
      }
      else
      {
        label.SetTextLocalize("( -" + str);
        ((UIWidget) label).color = this.mRed;
      }
    }
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
      label.SetTextLocalize(v.ToString());
    else
      label.SetTextLocalize("-" + Mathf.Abs(v).ToString());
  }

  private IEnumerator Start()
  {
    if (!this.fromHistoryPanel_)
    {
      BL.Unit unit1 = this.pUnit_;
      if ((object) unit1 == null)
        unit1 = this.env.core.currentUnitPosition.unit;
      BL.Unit unit2 = unit1;
      this.setUnit(unit2, !unit2.playerUnit.is_enemy);
      this.currentPhase_ = this.env.core.phaseState.state;
      this.currentForce_ = (int) this.env.core.getForceID(unit2);
    }
    yield return (object) this.initializeAsync(this.pUnit_);
  }

  private GameObject createIcon(GameObject prefab, Transform trans)
  {
    if (Object.op_Equality((Object) trans, (Object) null))
      return (GameObject) null;
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    UI2DSprite componentInChildren2 = ((Component) trans).GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren1).SetDimensions(((UIWidget) componentInChildren2).width, ((UIWidget) componentInChildren2).height);
    ((UIWidget) componentInChildren1).depth = this.iconDepth_;
    return icon;
  }

  protected override void Update()
  {
    if (this.IsPush)
      return;
    base.Update();
    if (this.fromHistoryPanel_ || this.env.core.phaseState.state == this.currentPhase_)
      return;
    this.IsPushAndSet();
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  public void initializeWeapon(BattleUI01UnitInformation.WeaponRow row, GearGear gear)
  {
    row.top_.SetActive(true);
    PlayerItem equippedGear = this.pUnit_.playerUnit.equippedGear;
    if (equippedGear != (PlayerItem) null && equippedGear.gear != gear)
    {
      PlayerItem equippedGear2 = this.pUnit_.playerUnit.equippedGear2;
      if (equippedGear2 != (PlayerItem) null && equippedGear2.gear != gear)
        ;
    }
    if (gear.hasAttackClass)
      row.iconClass_.Initialize(gear.gearClassification.attack_classification);
    else
      row.iconClass_.Initialize(this.pUnit_.playerUnit.initial_gear.gearClassification.attack_classification);
    row.spriteElement_.sprite2D = this.loadElementIcon(gear.attachedElement);
    row.spriteKind_.sprite2D = this.loadGearKindIcon((GearKindEnum) gear.kind_GearKind);
    row.txtName_.SetTextLocalize(gear.name);
    BL.Unit.GearRange gearRange = this.pUnit_.gearRange();
    row.txtRange_.SetTextLocalize(this.toRangeString(gearRange.Min, gearRange.Max));
    this.setTextCost(row.txtCost_, new int?());
    ((Component) row.button_).GetComponent<Collider>().enabled = false;
  }

  public void initializeMagic(BattleUI01UnitInformation.WeaponRow row, PlayerUnitSkills magic)
  {
    BattleskillSkill s = magic.skill;
    row.top_.SetActive(true);
    GearAttackClassification attackClass = GearAttackClassification.magic;
    row.iconClass_.Initialize(attackClass);
    row.spriteElement_.sprite2D = this.loadElementIcon(s.element);
    row.spriteKind_.sprite2D = this.loadGearKindIcon(GearKindEnum.magic);
    row.txtName_.SetTextLocalize(s.name);
    int min = s.min_range;
    int max = s.max_range;
    BL.MagicBullet mb = Array.Find<BL.MagicBullet>(this.pUnit_.magicBullets, (Predicate<BL.MagicBullet>) (x => x.skillId == s.ID));
    int num = 0;
    if (mb != null)
    {
      BL.Unit.MagicRange magicRange = this.pUnit_.magicRange(mb);
      min = magicRange.Min;
      max = magicRange.Max;
      num = mb.cost;
    }
    row.txtRange_.SetTextLocalize(this.toRangeString(min, max));
    this.setTextCost(row.txtCost_, new int?(num));
    this.setEventPopupAttackMethod(row.button_, attackClass, s);
  }

  public void initializeOptionAttack(BattleUI01UnitInformation.WeaponRow row, IAttackMethod attack)
  {
    row.top_.SetActive(true);
    row.iconClass_.Initialize(attack.attackClass);
    row.spriteElement_.sprite2D = this.loadElementIcon(attack.skill.element);
    row.spriteKind_.sprite2D = this.loadGearKindIcon(attack.kind.Enum);
    row.txtName_.SetTextLocalize(attack.skill.name);
    int min = attack.skill.min_range;
    int max = attack.skill.max_range;
    BL.MagicBullet mb = Array.Find<BL.MagicBullet>(this.pUnit_.magicBullets, (Predicate<BL.MagicBullet>) (x => x.skillId == attack.skill.ID));
    int num = 0;
    if (mb != null)
    {
      BL.Unit.MagicRange magicRange = this.pUnit_.magicRange(mb);
      min = magicRange.Min;
      max = magicRange.Max;
      num = mb.cost;
    }
    row.txtRange_.SetTextLocalize(this.toRangeString(min, max));
    this.setTextCost(row.txtCost_, new int?(num));
    this.setEventPopupAttackMethod(row.button_, attack.attackClass, attack.skill);
  }

  private void setTextCost(UILabel label, int? cost)
  {
    if (cost.HasValue)
      label.SetTextLocalize(cost.Value.ToString());
    else
      label.SetTextLocalize("-");
  }

  private string toRangeString(int min, int max)
  {
    return string.Format("{0} - {1}", (object) min, (object) max);
  }

  private Sprite loadGearKindIcon(GearKindEnum kind)
  {
    return GearKindIcon.LoadSprite(kind, CommonElement.none);
  }

  private Sprite loadElementIcon(CommonElement element)
  {
    return this.elementTypeIconPrefab.GetComponent<CommonElementIcon>().getIcon(element);
  }

  private void SetActiveForceObjects()
  {
    this.toggleObjects(this.dir_ForceHeader, this.currentForce_);
    this.toggleObjects(this.dir_ForceHpObjects, this.currentForce_);
    this.toggleObjects(this.dir_ForceJobObjects, this.currentForce_);
  }

  private void toggleObjects(GameObject[] objects, int active)
  {
    GameObject gameObject = (GameObject) null;
    for (int index = 0; index < objects.Length; ++index)
    {
      if (index != active)
        objects[index].SetActive(false);
      else
        gameObject = objects[index];
    }
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    gameObject.SetActive(true);
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

  public void setPopupSkillParams(PopupSkillDetails.Param[] param)
  {
    this.popupSkillParams_ = param;
  }

  public IEnumerator createAndSetEventBattleSkillIcon(GameObject parent, BattleskillSkill skill)
  {
    yield return (object) this.createBattleSkillIcon(parent, skill);
    this.setIconEvent(parent, skill);
  }

  public IEnumerator createBattleSkillIcon(GameObject parent, PlayerUnitSkills skill)
  {
    yield return (object) this.createBattleSkillIcon(parent, skill.skill);
    this.setIconEvent(parent, skill.skill);
  }

  private IEnumerator createBattleSkillIcon(GameObject parent, BattleskillSkill skill)
  {
    GameObject icon = this.skillTypeIconPrefab.Clone(parent.transform);
    ((UIWidget) icon.GetComponentInChildren<UI2DSprite>()).depth = ((UIWidget) ((Component) parent.transform).GetComponentInChildren<UI2DSprite>()).depth;
    yield return (object) icon.GetComponentInChildren<BattleSkillIcon>().Init(skill);
    int? transformationGroupId = skill.transformationGroupId;
    if (transformationGroupId.HasValue)
    {
      int metamorphosisGroupId = this.metamorphosisGroupId_;
      int? nullable = transformationGroupId;
      int valueOrDefault = nullable.GetValueOrDefault();
      if (!(metamorphosisGroupId == valueOrDefault & nullable.HasValue))
      {
        this.skillLockIconPrefab.Clone(parent.transform);
        ((UIWidget) icon.GetComponentInChildren<BattleSkillIcon>().iconSprite).color = Color.gray;
      }
    }
  }

  public IEnumerator createBattleSkillIcon(GameObject parent, GearGearSkill skill)
  {
    yield return (object) this.createBattleSkillIcon(parent, skill.skill);
    this.setIconEvent(parent, skill.skill);
  }

  public IEnumerator createBattleSkillIcon(GameObject parent, GearReisouSkill skill)
  {
    yield return (object) this.createBattleSkillIcon(parent, skill.skill);
    this.setIconEvent(parent, skill.skill);
  }

  public IEnumerator createJobAbilityIcon(GameObject parent, PlayerUnitSkills skill)
  {
    yield return (object) this.LoadLSSkillIcon(parent, skill.skill);
    this.setIconJobAbilityEvent(parent, skill);
  }

  public IEnumerator LoadLSSkillIcon(GameObject parent, BattleskillSkill in_skill)
  {
    BattleFuncs.InvestSkill s = new BattleFuncs.InvestSkill();
    s.skill = in_skill;
    s.isEnemyIcon = this.pUnit_.playerUnit.is_enemy;
    UI2DSprite LSSKill = ((Component) parent.transform).GetComponentInChildren<UI2DSprite>();
    Future<Sprite> spriteF = s.skill.LoadBattleSkillIcon(s);
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    LSSKill.sprite2D = spriteF.Result;
  }

  public IEnumerator LoadLSSkillIcon(GameObject parent, PlayerUnitLeader_skills in_skill)
  {
    yield return (object) this.LoadLSSkillIcon(parent, in_skill.skill);
    this.setIconEvent(parent, in_skill.skill);
  }

  public IEnumerator LoadExtraSkillIcon(GameObject parent, PlayerAwakeSkill awakeSkill)
  {
    UI2DSprite extraSKill = ((Component) parent.transform).GetComponentInChildren<UI2DSprite>();
    Future<Sprite> spriteF = awakeSkill.masterData.LoadBattleSkillIcon();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    extraSKill.sprite2D = spriteF.Result;
    this.setIconEvent(parent, awakeSkill.masterData);
  }

  private void setIconEvent(GameObject obj, BattleskillSkill skill)
  {
    this.setEventPopupSkill(obj.GetComponentInChildren<UIButton>(), skill);
  }

  private void setEventPopupSkill(UIButton btn, BattleskillSkill skill)
  {
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.popupSkill(skill)));
  }

  private void setEventPopupSkill(
    UIButton btn,
    BattleskillSkill skill,
    UnitParameter.SkillGroup group,
    int level)
  {
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.popupSkill(skill, group, level)));
  }

  private void popupSkill(BattleskillSkill skill)
  {
    PopupSkillDetails.show(this.skillDialogPrefab, this.popupSkillParams_, ((IEnumerable<PopupSkillDetails.Param>) this.popupSkillParams_).FirstIndexOrNull<PopupSkillDetails.Param>((Func<PopupSkillDetails.Param, bool>) (x => x.skill == skill)).Value, this.pUnit_.playerUnit.is_enemy, isAutoClose: !this.fromHistoryPanel_);
  }

  private void popupSkill(BattleskillSkill skill, UnitParameter.SkillGroup group, int level)
  {
    PopupSkillDetails.show(this.skillDialogPrefab, new PopupSkillDetails.Param(skill, group, new int?(level)), this.pUnit_.playerUnit.is_enemy, isAutoClose: !this.fromHistoryPanel_);
  }

  private void setIconJobAbilityEvent(GameObject obj, PlayerUnitSkills skill)
  {
    UIButton component = obj.GetComponent<UIButton>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    EventDelegate.Set(component.onClick, (EventDelegate.Callback) (() => this.popupSkill(skill.skill)));
  }

  private void setEventPopupAttackMethod(
    UIButton btn,
    GearAttackClassification attackClass,
    BattleskillSkill detail)
  {
    if (!Object.op_Inequality((Object) btn, (Object) null))
      return;
    EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.popupAttackMethod(attackClass, detail)));
  }

  private void popupAttackMethod(GearAttackClassification attackClass, BattleskillSkill detail)
  {
    if (Object.op_Equality((Object) this.attackMethodParent, (Object) null))
      return;
    if (Object.op_Equality((Object) this.attackMethodDialog, (Object) null))
      this.attackMethodDialog = this.attackMethodPrefab.Clone(this.attackMethodParent);
    DetailAttackMenuDialog componentInChildren = this.attackMethodDialog.GetComponentInChildren<DetailAttackMenuDialog>();
    componentInChildren.setSkillProperty(true);
    componentInChildren.setData(detail, "", attackClass);
    componentInChildren.Show();
    this.attackMethodDialog.SetActive(true);
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

  private IEnumerator initializeAsync(BL.Unit v)
  {
    BattleUI01UnitInformation main = this;
    main.setTopObject(((Component) main).gameObject);
    UIWidget w = ((Component) main).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) w, (Object) null))
      ((UIRect) w).alpha = 0.0f;
    Future<GameObject> gearKindIconPrefabF = Res.Icons.GearKindIcon.Load<GameObject>();
    IEnumerator e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    main.gearKindIconPrefab = gearKindIconPrefabF.Result;
    gearKindIconPrefabF = (Future<GameObject>) null;
    gearKindIconPrefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    main.skillTypeIconPrefab = gearKindIconPrefabF.Result;
    gearKindIconPrefabF = (Future<GameObject>) null;
    gearKindIconPrefabF = new ResourceObject("Prefabs/BattleSkillIcon/dir_SkillLock").Load<GameObject>();
    yield return (object) gearKindIconPrefabF.Wait();
    main.skillLockIconPrefab = gearKindIconPrefabF.Result;
    gearKindIconPrefabF = (Future<GameObject>) null;
    gearKindIconPrefabF = Res.Icons.CommonElementIcon.Load<GameObject>();
    e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    main.elementTypeIconPrefab = gearKindIconPrefabF.Result;
    gearKindIconPrefabF = (Future<GameObject>) null;
    gearKindIconPrefabF = Res.Icons.GearProfiencyIcon.Load<GameObject>();
    e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    main.gearProfiencyIconPrefab = gearKindIconPrefabF.Result;
    gearKindIconPrefabF = (Future<GameObject>) null;
    gearKindIconPrefabF = PopupSkillDetails.createPrefabLoader(false);
    e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    main.skillDialogPrefab = gearKindIconPrefabF.Result;
    gearKindIconPrefabF = (Future<GameObject>) null;
    if (Object.op_Inequality((Object) main.attackMethodParent, (Object) null))
    {
      gearKindIconPrefabF = new ResourceObject("Prefabs/unit004_2/AttackMethodDialog").Load<GameObject>();
      yield return (object) gearKindIconPrefabF.Wait();
      main.attackMethodPrefab = gearKindIconPrefabF.Result;
      main.attackClassIcon = ((Component) main).gameObject.GetOrAddComponent<AttackClassIcon>();
      gearKindIconPrefabF = (Future<GameObject>) null;
    }
    gearKindIconPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    main.gearIconPrefab_ = gearKindIconPrefabF.Result;
    gearKindIconPrefabF = (Future<GameObject>) null;
    foreach (BattleUI01UnitInformation.GearLayout gearLayout in main.gearLayouts_)
    {
      gearLayout.gear_ = main.createIcon(main.gearIconPrefab_, gearLayout.lnkIcon_).GetComponent<ItemIcon>();
      gearLayout.proficiencyKind_ = main.createIcon(main.gearKindIconPrefab, gearLayout.lnkProficiencyKind_).GetComponent<GearKindIcon>();
      gearLayout.proficiency_ = main.createIcon(main.gearProfiencyIconPrefab, gearLayout.lnkProficiency_).GetComponent<GearProfiencyIcon>();
      gearLayout.disabled();
    }
    main.SetActiveForceObjects();
    ((IEnumerable<GameObject>) main.slcStatus1BaseOwnIcons).ToggleOnce(v.playerUnit.is_guest ? 1 : 0);
    Judgement.BattleParameter battleParameter = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) v, isUsePosition: !main.fromHistoryPanel_);
    UnitUnit unitUnit = v.unit;
    if (Object.op_Inequality((Object) main.iconWeaponType_, (Object) null))
      main.iconWeaponType_.sprite2D = GearKindIcon.LoadSprite(unitUnit.kind.Enum, v.GetElement());
    main.metamorphosisGroupId_ = main.fromHistoryPanel_ ? 0 : v.transformationGroupId;
    main.setText(main.txt_Agility, (object) battleParameter.Agility);
    main.setText(main.txt_Luck, (object) battleParameter.Luck);
    main.setText(main.txt_Magic, (object) battleParameter.Intelligence);
    main.setText(main.txt_Power, (object) battleParameter.Strength);
    main.setText(main.txt_Stability, (object) battleParameter.Vitality);
    main.setText(main.txt_Spirit, (object) battleParameter.Mind);
    main.setText(main.txt_Technique, (object) battleParameter.Dexterity);
    main.setBDTextWrraper(main.txt_AgilityBD, battleParameter.AgilityIncr);
    main.setBDTextWrraper(main.txt_LuckBD, battleParameter.LuckIncr);
    main.setBDTextWrraper(main.txt_MagicBD, battleParameter.IntelligenceIncr);
    main.setBDTextWrraper(main.txt_PowerBD, battleParameter.StrengthIncr);
    main.setBDTextWrraper(main.txt_StabilityBD, battleParameter.VitalityIncr);
    main.setBDTextWrraper(main.txt_SpiritBD, battleParameter.MindIncr);
    main.setBDTextWrraper(main.txt_TechniqueBD, battleParameter.DexterityIncr);
    int hp = battleParameter.Hp;
    int num = main.displayMaxHp_ ? battleParameter.Hp : v.hp;
    main.hpGauge.setValue(num, hp, false);
    main.setText(main.txt_Hp, (object) num);
    if (Object.op_Inequality((Object) main.txt_Hp_Enemy, (Object) null))
      main.setText(main.txt_Hp_Enemy, (object) num);
    main.setText(main.txt_Hpmax, "/" + (object) hp);
    if (Object.op_Inequality((Object) main.txt_Hpmax_Enemy, (Object) null))
      main.setText(main.txt_Hpmax_Enemy, "/" + (object) hp);
    BattleUI01UnitInformation ui01UnitInformation = main;
    UILabel txtCharacterName = main.txt_CharacterName;
    UnitUnit unitUnit1 = unitUnit;
    SkillMetamorphosis metamorphosis = v.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    string formalName = unitUnit1.getFormalName(metamorphosisId);
    ui01UnitInformation.setText(txtCharacterName, formalName);
    main.setText(main.txt_Lv, (object) v.lv);
    main.setColordText(main.txt_Fighting, battleParameter.Combat, battleParameter.CombatIncr);
    main.setText(main.txt_Jobname, v.job.name);
    if (Object.op_Inequality((Object) main.txt_Jobname_Enemy, (Object) null))
      main.setText(main.txt_Jobname_Enemy, v.job.name);
    main.setColordText(main.txt_Movement, battleParameter.Move, battleParameter.MoveIncr);
    main.setText(main.txt_Attack, (object) battleParameter.PhysicalAttack);
    main.setText(main.txt_Critical, (object) battleParameter.Critical);
    main.setText(main.txt_Defense, (object) battleParameter.PhysicalDefense);
    main.setText(main.txt_Dexterity, (object) battleParameter.Hit);
    main.setText(main.txt_Evasion, (object) battleParameter.Evasion);
    main.setText(main.txt_Matk, (object) battleParameter.MagicAttack);
    main.setText(main.txt_Mdef, (object) battleParameter.MagicDefense);
    main.setBDTextWrraper(main.txt_AttackBD, battleParameter.PhysicalAttackIncr);
    main.setBDTextWrraper(main.txt_CriticalBD, battleParameter.CriticalIncr);
    main.setBDTextWrraper(main.txt_DefenseBD, battleParameter.PhysicalDefenseIncr);
    main.setBDTextWrraper(main.txt_DexterityBD, battleParameter.HitIncr);
    main.setBDTextWrraper(main.txt_EvasionBD, battleParameter.EvasionIncr);
    main.setBDTextWrraper(main.txt_MatkBD, battleParameter.MagicAttackIncr);
    main.setBDTextWrraper(main.txt_MdefBD, battleParameter.MagicDefenseIncr);
    main.SetPrincessType(v);
    PlayerUnit pu = v.playerUnit;
    HashSet<int> setKind = new HashSet<int>();
    bool is_awake = pu.unit.awake_unit_flag;
    int index = 0;
    GearGear gear1 = pu.equippedGear?.gear;
    GearGear gear2 = pu.equippedReisou?.gear;
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
      k1 = pu.unit.kind;
    PlayerUnitGearProficiency unitGearProficiency1 = pu.gear_proficiencies != null ? Array.Find<PlayerUnitGearProficiency>(pu.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == k1.ID)) : (PlayerUnitGearProficiency) null;
    if (unitGearProficiency1 != null)
      level1 = unitGearProficiency1.level;
    main.gearLayouts_[index].reset(gear1, element1, level1, gear2, pu.unit.IsAllEquipUnit);
    if (gear1 == null)
      main.gearLayouts_[index].setKind(k1);
    yield return (object) main.gearLayouts_[index].initAsync();
    setKind.Add(k1.ID);
    ++index;
    GearGear gear3;
    GearGear gear4;
    if (is_awake)
    {
      gear3 = pu.equippedGear2?.gear;
      gear4 = pu.equippedReisou2?.gear;
    }
    else
    {
      gear3 = pu.equippedGear3?.gear;
      gear4 = pu.equippedReisou3?.gear;
    }
    CommonElement element2 = CommonElement.none;
    int level2 = 1;
    GearKind k2;
    if (gear3 != null)
    {
      k2 = gear3.kind;
      if (gear3.elements != null && gear3.elements.Length != 0)
        element2 = gear3.elements[0].element;
    }
    else
      k2 = (GearKind) null;
    if (is_awake)
    {
      PlayerUnitGearProficiency unitGearProficiency2 = pu.gear_proficiencies == null || k2 == null ? (PlayerUnitGearProficiency) null : Array.Find<PlayerUnitGearProficiency>(pu.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == k2.ID));
      if (unitGearProficiency2 != null)
        level2 = unitGearProficiency2.level;
      else if (pu.gear_proficiencies != null && k2 == null)
      {
        PlayerUnitGearProficiency unitGearProficiency3 = Array.Find<PlayerUnitGearProficiency>(pu.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => !setKind.Contains(x.gear_kind_id)));
        if (unitGearProficiency3 != null)
        {
          level2 = unitGearProficiency3.level;
          MasterData.GearKind.TryGetValue(unitGearProficiency3.gear_kind_id, out k2);
        }
      }
    }
    main.gearLayouts_[index].reset(gear3, element2, level2, gear4, pu.unit.IsAllEquipUnit);
    bool awakeUnitFlag1 = pu.unit.awake_unit_flag;
    if (awakeUnitFlag1 && gear3 == null)
    {
      if (k2 == null)
        k2 = MasterData.GearKind[7];
      main.gearLayouts_[index].setKind(k2);
    }
    yield return (object) main.gearLayouts_[index].initAsync(awakeUnitFlag1, !is_awake);
    ++index;
    GearGear gear5 = is_awake ? pu.equippedGear3?.gear : (GearGear) null;
    if (gear5 == null)
    {
      main.gearLayouts_[index].disabled();
    }
    else
    {
      GearGear gear6 = is_awake ? pu.equippedReisou3?.gear : (GearGear) null;
      CommonElement element3 = CommonElement.none;
      int level3 = 0;
      GearKind kind;
      if (gear5 != null)
      {
        kind = gear5.kind;
        if (gear5.elements != null && gear5.elements.Length != 0)
          element3 = gear5.elements[0].element;
      }
      else
        kind = (GearKind) null;
      main.gearLayouts_[index].reset(gear5, element3, level3, gear6, pu.unit.IsAllEquipUnit);
      bool awakeUnitFlag2 = pu.unit.awake_unit_flag;
      if (awakeUnitFlag2 && gear5 == null)
      {
        if (kind == null)
          kind = MasterData.GearKind[7];
        main.gearLayouts_[index].setKind(kind);
      }
      yield return (object) main.gearLayouts_[index].initAsync(awakeUnitFlag2, true);
    }
    gearKindIconPrefabF = unitUnit.LoadMypage();
    e = gearKindIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    main.characterTransform.Clear();
    GameObject result = gearKindIconPrefabF.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      int nextDepth = NGUITools.CalculateNextDepth(((Component) main.characterTransform).gameObject);
      GameObject go = result.Clone(main.characterTransform);
      go.GetComponent<NGxMaskSpriteWithScale>().scale = 0.6f;
      int jobOrMetamorId = v.playerUnit.job_id;
      if (!main.fromHistoryPanel_ && main.metamorphosisGroupId_ != 0)
      {
        BL.SkillEffect skillEffect = v.skillEffects.Where(BattleskillEffectLogicEnum.transformation).FirstOrDefault<BL.SkillEffect>((Func<BL.SkillEffect, bool>) (x => x.effect.GetInt(BattleskillEffectLogicArgumentEnum.transformation_group_id) == this.metamorphosisGroupId_));
        if (skillEffect != null)
        {
          SkillMetamorphosis skillMetamorphosis = MasterData.UniqueSkillMetamorphosisBy(unitUnit, skillEffect.baseSkill, main.metamorphosisGroupId_);
          if (skillMetamorphosis != null)
            jobOrMetamorId = skillMetamorphosis.metamorphosis_id;
        }
      }
      e = unitUnit.SetLargeSpriteWithMask(jobOrMetamorId, go, Res.GUI.battleUI_03.mask_Character.Load<Texture2D>(), nextDepth, 190, 25);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      gearKindIconPrefabF = (Future<GameObject>) null;
      for (int n = 0; n < main.tabs_.Length; ++n)
      {
        BattleUI01UnitInformation.TabControl tab = main.tabs_[n];
        tab.tab_.preInitialize(main, v);
        yield return (object) tab.tab_.initialize();
        if (n != 0)
          tab.setActive(false);
        tab = (BattleUI01UnitInformation.TabControl) null;
      }
      main.changeTab(BattleUI01UnitInformationTab.Type.Skill, true);
      yield return (object) null;
      if (Object.op_Inequality((Object) w, (Object) null))
        ((UIRect) w).alpha = 1f;
    }
  }

  public override void onBackButton() => this.onClickedClose();

  public void onClickedClose()
  {
    if (this.IsPushAndSet())
      return;
    if (!this.fromHistoryPanel_)
    {
      if (Object.op_Inequality((Object) this.battleManager, (Object) null))
        this.battleManager.popupDismiss();
      else
        Singleton<PopupManager>.GetInstance().dismiss();
    }
    else
      Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void InitFromHistory(BL.Unit playerUnit, bool isPlayer)
  {
    this.fromHistoryPanel_ = true;
    this.displayMaxHp_ = true;
    this.setUnit(playerUnit, isPlayer);
  }

  public void InitFromPVP(BL.Unit playerUnit, bool isPlayer)
  {
    this.fromHistoryPanel_ = false;
    this.displayMaxHp_ = false;
    this.setUnit(playerUnit, isPlayer);
  }

  public void InitFromTowerHistory(BL.Unit playerUnit, bool isPlayer)
  {
    this.fromHistoryPanel_ = true;
    this.displayMaxHp_ = false;
    this.setUnit(playerUnit, isPlayer);
  }

  public void InitFromUnit(BL.Unit unit) => this.pUnit_ = unit;

  private void setUnit(BL.Unit playerUnit, bool isPlayer)
  {
    this.currentForce_ = isPlayer ? 0 : 2;
    this.pUnit_ = playerUnit;
  }

  private void changeTab(BattleUI01UnitInformationTab.Type type, bool bInit = false)
  {
    int index = (int) type;
    if (!bInit && this.currentTab_ != index)
      this.tabs_[this.currentTab_].setActive(false);
    if (!bInit && this.currentTab_ == index)
      return;
    this.tabs_[index].setActive(true);
    this.currentTab_ = index;
  }

  public void onClickedTabSkill() => this.changeTab(BattleUI01UnitInformationTab.Type.Skill);

  public void onClickedTabWeapon() => this.changeTab(BattleUI01UnitInformationTab.Type.Weapon);

  public void onClickedTabOption() => this.changeTab(BattleUI01UnitInformationTab.Type.Option);

  [Serializable]
  public class WeaponRow
  {
    [SerializeField]
    [Tooltip("先頭オブジェクト")]
    public GameObject top_;
    [SerializeField]
    [Tooltip("攻撃区分")]
    public AttackClassIcon iconClass_;
    [SerializeField]
    [Tooltip("武具種")]
    public UI2DSprite spriteKind_;
    [SerializeField]
    [Tooltip("攻撃属性")]
    public UI2DSprite spriteElement_;
    [SerializeField]
    [Tooltip("ボタン")]
    public UIButton button_;
    [SerializeField]
    [Tooltip("名前")]
    public UILabel txtName_;
    [SerializeField]
    [Tooltip("範囲")]
    public UILabel txtRange_;
    [SerializeField]
    [Tooltip("コスト")]
    public UILabel txtCost_;
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
    private GearGear masterReisou_;
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

    public void reset(
      GearGear gear,
      CommonElement element,
      int level,
      GearGear reisou,
      bool isAllEquipUnit)
    {
      this.masterGear_ = gear;
      this.masterReisou_ = reisou;
      this.gearKind_ = !isAllEquipUnit ? gear?.kind : MasterData.GearKind[8];
      this.element_ = element;
      this.level_ = level;
      this.objEnabled_.SetActive(false);
      if (!Object.op_Inequality((Object) this.objDisabled_, (Object) null))
        return;
      this.objEnabled_.SetActive(true);
    }

    public void setKind(GearKind kind) => this.gearKind_ = kind;

    public IEnumerator initAsync(bool displayEmpty = false, bool is_hide_proficiency = false)
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
          IEnumerator e = this.gear_.InitByGear(this.masterGear_, this.element_, setReisou: this.masterReisou_);
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
          this.proficiency_.Init(this.level_, this.gearKind_.isHideProficiency | is_hide_proficiency);
        }
        foreach (Collider componentsInChild in ((Component) this.gear_).GetComponentsInChildren<Collider>())
          componentsInChild.enabled = false;
      }
    }
  }

  [Serializable]
  private class TabControl
  {
    public UIButton button_;
    public BattleUI01UnitInformationTab tab_;

    public void setActive(bool v)
    {
      ((UIButtonColor) this.button_).isEnabled = !v;
      this.tab_.isEnabled = v;
    }
  }
}
