// Decompiled with JetBrains decompiler
// Type: Battle01UIPlayerStatus
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
public class Battle01UIPlayerStatus : NGBattleMenuBase
{
  public NGTweenGaugeScale hpGauge;
  [SerializeField]
  protected SelectParts[] statusBase;
  [SerializeField]
  protected GameObject dirCharacterStatus;
  [SerializeField]
  protected GameObject dirCharacterSkill;
  [SerializeField]
  protected UI2DSprite character;
  [SerializeField]
  protected UI2DSprite[] weaponsIcons;
  [SerializeField]
  protected UILabel[] txt_CharacterName;
  [SerializeField]
  protected UILabel txt_Lv;
  [SerializeField]
  protected UILabel[] txt_Fighting;
  [SerializeField]
  protected UILabel txt_Hp;
  [SerializeField]
  protected UILabel txt_Hpmax;
  [SerializeField]
  protected UILabel txt_Hp_Large;
  [SerializeField]
  protected UILabel txt_Jobname;
  [SerializeField]
  protected UILabel[] txt_Movement;
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
  protected UILabel txt_Matk;
  [SerializeField]
  protected UILabel txt_Mdef;
  private BL.BattleModified<BL.Unit> modified;
  private BL.BattleModified<BL.UnitPosition> unitPositionModified;
  private BL.BattleModified<BL.SkillEffectList> skillEffectsModified;
  private BL.BattleModified<BL.ClassValue<List<BL.SkillEffect>>> panelEffectsModified;
  [SerializeField]
  protected UIButton dirAilments;
  [SerializeField]
  protected GameObject[] ailmentsPos;
  private GameObject battleSkillIconPrefab;
  private GameObject groupSkillsIconPrefab;
  private GameObject skillLockIconPrefab;
  private List<GameObject> groupIcons = new List<GameObject>();
  private List<GameObject> blinkGroup = new List<GameObject>();
  [SerializeField]
  private NGxBlinkEx blink;
  [HideInInspector]
  public bool blinkResetDirty;
  private GameObject skillDialog;
  [SerializeField]
  protected GameObject SkillDialogRoot;
  private DialogJobAbilityDetail jobAbilityDialog;
  [SerializeField]
  private GameObject JobAbilityDialogRoot;
  private UnitIcon unitIcon;
  private GearKindIcon[] gearIcons;
  [SerializeField]
  private UISprite[] slcCountry;
  private List<BattleSkillIcon> ailmentsIcons;
  private PopupSkillDetails.Param[] skillParams_;
  private GameObject popupInfoPrefab;
  private GameObject skillDialogPrefab;
  private GameObject jobAbilityDialogPrefab;
  private GameObject ailmentsDetailPrefab;
  private GameObject skillListPrefab;
  [SerializeField]
  protected UIGrid dir_ExtraSkill;
  [SerializeField]
  protected GameObject[] dyn_ExtraSkill;
  [SerializeField]
  protected UIGrid dir_Skill;
  [SerializeField]
  protected GameObject[] dyn_Unit_Skill;
  protected List<int> setSkills = new List<int>();
  protected List<GameObject> setSkillIconParents = new List<GameObject>();
  [SerializeField]
  protected UIButton btnUp;
  [SerializeField]
  protected UIButton btnDown;
  [SerializeField]
  protected GameObject dirSkillList;
  private const int c_skill_row_num = 8;
  private const int c_skill_column_num = 3;
  private const float c_skillPosY_base = 0.0f;
  private const float c_skillPosY_offset = 62f;
  private int index_skill_vertical;
  private int skill_num;
  [SerializeField]
  private AttackClassIcon iconAttackClass;
  private bool? isSea_;
  private bool? isEarth_;
  private Queue<KeyValuePair<int, BattleFuncs.InvestSkill>> loadAilmentIconTasks = new Queue<KeyValuePair<int, BattleFuncs.InvestSkill>>();
  private IEnumerator loadAilmentIconWork;
  private bool battleEnableUpdateDity;
  private bool skillInfoUpdateDity;
  private BL.StructValue<bool> waitAIActionCancel;
  private Color mGreen = new Color(0.0f, 0.863f, 0.118f);
  private Color mRed = new Color(0.98f, 0.0f, 0.0f);
  private Color mOrigin = new Color(1f, 1f, 1f);
  private Color mSeaOrigin = new Color(0.3f, 0.3f, 0.3f);
  private int metamorphosisGroupId;

  public Battle01StatusScrollParts battleStatusScrollParts { get; set; }

  private bool isSea
  {
    get
    {
      return !this.isSea_.HasValue ? (this.isSea_ = new bool?(this.battleManager.isSea)).Value : this.isSea_.Value;
    }
  }

  private bool isEarth
  {
    get
    {
      return !this.isEarth_.HasValue ? (this.isEarth_ = new bool?(this.battleManager.isEarth)).Value : this.isEarth_.Value;
    }
  }

  private void Awake()
  {
    ((Behaviour) this.blink).enabled = false;
    ((Behaviour) this.character).enabled = false;
    foreach (Behaviour weaponsIcon in this.weaponsIcons)
      weaponsIcon.enabled = false;
    foreach (UIWidget componentsInChild in ((Component) this).gameObject.GetComponentsInChildren<UIWidget>(true))
    {
      if (Object.op_Equality((Object) ((Component) componentsInChild).GetComponent<UIButton>(), (Object) null))
        ((UIRect) componentsInChild).alpha = 0.0f;
    }
  }

  private string pathPlayerStatusPrefab
  {
    get
    {
      return "Prefabs/battleUI_03/" + (this.env.core.battleInfo.isEarthMode ? "Battle030627_UI_PlayerStatus" : "Battle030627_UI_PlayerStatus_2");
    }
  }

  public override IEnumerator onInitAsync()
  {
    Future<GameObject> f = !this.isSea ? Res.Prefabs.UnitIcon.normal.Load<GameObject>() : Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIcon = f.Result.CloneAndGetComponent<UnitIcon>(((Component) this.character).gameObject.transform);
    NGUITools.AdjustDepth(((Component) this.unitIcon).gameObject, ((UIWidget) this.character).depth);
    this.unitIcon.SetBasedOnHeight(((UIWidget) this.character).height);
    f = Res.Icons.GearKindIcon.Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = f.Result;
    this.gearIcons = new GearKindIcon[this.weaponsIcons.Length];
    for (int index = 0; index < this.gearIcons.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.weaponsIcons[index], (Object) null))
      {
        this.gearIcons[index] = result.CloneAndGetComponent<GearKindIcon>(((Component) this.weaponsIcons[index]).gameObject.transform);
        NGUITools.AdjustDepth(((Component) this.gearIcons[index]).gameObject, ((UIWidget) this.weaponsIcons[index]).depth);
        this.gearIcons[index].SetBasedOnHeight(((UIWidget) this.weaponsIcons[index]).height);
      }
    }
    f = new ResourceObject(this.pathPlayerStatusPrefab).Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.popupInfoPrefab = f.Result;
    f = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.battleSkillIconPrefab = f.Result;
    f = new ResourceObject("Prefabs/BattleSkillIcon/dir_SkillLock").Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillLockIconPrefab = f.Result;
    f = Res.Prefabs.battle.GroupSkillsIcons.Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.groupSkillsIconPrefab = f.Result;
    Future<GameObject> skillDialogPrefabF;
    if (this.isEarth)
    {
      skillDialogPrefabF = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
      yield return (object) skillDialogPrefabF.Wait();
      this.skillDialogPrefab = skillDialogPrefabF.Result;
      this.skillDialog = this.skillDialogPrefab.Clone(this.SkillDialogRoot.transform);
      this.skillDialog.GetComponentInChildren<UIPanel>().depth += 30;
      this.skillDialog.SetActive(false);
      if (Object.op_Inequality((Object) this.JobAbilityDialogRoot, (Object) null))
      {
        Future<GameObject> jobAbilityDialogPrefabF = new ResourceObject("Prefabs/battle017_11_1_1/AbilityDetailDialog").Load<GameObject>();
        e = jobAbilityDialogPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.jobAbilityDialogPrefab = jobAbilityDialogPrefabF.Result;
        GameObject gameObject = this.jobAbilityDialogPrefab.Clone(this.JobAbilityDialogRoot.transform);
        UIPanel component1 = gameObject.GetComponent<UIPanel>();
        UIWidget component2 = this.JobAbilityDialogRoot.GetComponent<UIWidget>();
        if (Object.op_Inequality((Object) component1, (Object) null))
          component1.depth += Object.op_Inequality((Object) component2, (Object) null) ? component2.depth + 1 : 30;
        this.jobAbilityDialog = gameObject.GetComponent<DialogJobAbilityDetail>();
        this.jobAbilityDialog.isAutoClose = true;
        gameObject.SetActive(false);
        jobAbilityDialogPrefabF = (Future<GameObject>) null;
      }
      skillDialogPrefabF = (Future<GameObject>) null;
    }
    else
    {
      skillDialogPrefabF = PopupSkillDetails.createPrefabLoader(this.isSea);
      e = skillDialogPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.skillDialogPrefab = skillDialogPrefabF.Result;
      skillDialogPrefabF = (Future<GameObject>) null;
    }
    Future<GameObject> ailmentsDetailPrefabF = Res.Prefabs.battle.popup_Ailments_detail__anim_popup01.Load<GameObject>();
    e = ailmentsDetailPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ailmentsDetailPrefab = ailmentsDetailPrefabF.Result;
    this.ailmentsIcons = new List<BattleSkillIcon>();
    this.ClearSkillsInformations();
    f = !this.isSea ? Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/battle017_11_1_1/popup_SkillList") : Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/battle017_11_1_1/popup_SkillList_sea");
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillListPrefab = f.Result;
  }

  private IEnumerator doSetUnitIcon(BL.Unit unit)
  {
    IEnumerator e = this.unitIcon.SetUnit(unit.playerUnit, unit.metamorphosis, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIcon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
    this.unitIcon.isViewBackObject = false;
  }

  private void LoadAilmentIcon(int index, BattleFuncs.InvestSkill s)
  {
    this.loadAilmentIconTasks.Enqueue(new KeyValuePair<int, BattleFuncs.InvestSkill>(index, s));
    if (this.loadAilmentIconWork != null)
      return;
    this.loadAilmentIconWork = this.doSetAilmentIcon();
    this.StartCoroutine(this.loadAilmentIconWork);
  }

  private void StopLoadAilmentIcon()
  {
    if (this.loadAilmentIconWork != null)
      this.StopCoroutine(this.loadAilmentIconWork);
    this.loadAilmentIconWork = (IEnumerator) null;
    this.loadAilmentIconTasks.Clear();
  }

  private IEnumerator doSetAilmentIcon()
  {
    yield return (object) null;
    try
    {
      while (this.loadAilmentIconTasks.Count > 0)
      {
        KeyValuePair<int, BattleFuncs.InvestSkill> keyValuePair = this.loadAilmentIconTasks.Dequeue();
        BattleSkillIcon icon = this.ailmentsIcons[keyValuePair.Key];
        IEnumerator e = icon.Init(keyValuePair.Value);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (Object.op_Inequality((Object) icon, (Object) null))
          ((Component) icon).gameObject.SetActive(true);
        yield return (object) null;
        icon = (BattleSkillIcon) null;
      }
    }
    finally
    {
      this.loadAilmentIconWork = (IEnumerator) null;
    }
  }

  protected override void LateUpdate_Battle()
  {
    if (this.modified == null)
      return;
    Judgement.BattleParameter u = (Judgement.BattleParameter) null;
    bool flag1 = this.modified.isChangedOnce();
    bool flag2 = this.unitPositionModified.isChangedOnce();
    bool flag3 = this.skillEffectsModified.isChangedOnce();
    if (flag2)
    {
      BL.ClassValue<List<BL.SkillEffect>> skillEffects = BattleFuncs.getPanel(this.unitPositionModified.value).getSkillEffects();
      if (!BL.equalPanelSkillEffectList(this.panelEffectsModified.value.value, skillEffects.value))
        this.panelEffectsModified = BL.Observe<BL.ClassValue<List<BL.SkillEffect>>>(skillEffects);
    }
    bool flag4 = this.panelEffectsModified.isChangedOnce();
    if (flag1 | flag2 | flag3 | flag4)
    {
      if (u == null)
        u = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) this.modified.value);
      foreach (SelectParts selectParts in this.statusBase)
        selectParts.setValueNonTween((int) this.env.core.getForceID(this.modified.value));
      if (flag1)
      {
        this.UpdateUnitBaseInfo();
        this.skillInfoUpdateDity = true;
      }
      this.UpdateUnitParameter(u);
      if (flag1 | flag3 | flag4)
        this.battleEnableUpdateDity = true;
      foreach (UIWidget componentsInChild in ((Component) this).gameObject.GetComponentsInChildren<UIWidget>(true))
      {
        if (Object.op_Equality((Object) ((Component) componentsInChild).GetComponent<UIButton>(), (Object) null) && (double) ((UIRect) componentsInChild).alpha == 0.0)
          ((UIRect) componentsInChild).alpha = 1f;
      }
    }
    if (this.skillInfoUpdateDity && this.dirCharacterSkill.activeSelf)
    {
      this.SetSkillsInformations();
      this.skillInfoUpdateDity = false;
    }
    if (!this.battleManager.isBattleEnable)
      return;
    if (this.blinkResetDirty)
    {
      if (((Behaviour) this.blink).enabled)
        this.blink.ResetTween();
      this.blinkResetDirty = false;
    }
    if (!this.battleEnableUpdateDity)
      return;
    if (u == null)
      u = Judgement.BattleParameter.FromBeUnit((BL.ISkillEffectListUnit) this.modified.value);
    BL.Unit unit = this.modified.value;
    this.hpGauge.setValue(unit.hp, u.Hp);
    this.setText(this.txt_Hp, unit.hp);
    if (Object.op_Inequality((Object) this.txt_Hp_Large, (Object) null))
      this.setText(this.txt_Hp_Large, unit.hp);
    this.UpdateAlimentsIcons();
    this.battleEnableUpdateDity = false;
  }

  private void UpdateUnitBaseInfo()
  {
    BL.Unit unit1 = this.modified.value;
    this.StartCoroutine(this.doSetUnitIcon(unit1));
    UnitUnit unit2 = unit1.unit;
    UnitUnit unitUnit = unit2;
    SkillMetamorphosis metamorphosis = unit1.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    string name = unitUnit.getName(metamorphosisId);
    foreach (UILabel label in this.txt_CharacterName)
      this.setText(label, name);
    foreach (GearKindIcon gearIcon in this.gearIcons)
    {
      if (!Object.op_Equality((Object) gearIcon, (Object) null))
        gearIcon.Init(unit2.kind, unit1.playerUnit.GetElement());
    }
    if (this.slcCountry != null)
    {
      if (unit2.country_attribute.HasValue)
      {
        for (int index = 0; index < this.slcCountry.Length; ++index)
        {
          ((Component) this.slcCountry[index]).gameObject.SetActive(true);
          unit2.SetCuntrySpriteName(ref this.slcCountry[index]);
        }
      }
      else
      {
        for (int index = 0; index < this.slcCountry.Length; ++index)
        {
          unit2.SetNonCuntrySpriteName(ref this.slcCountry[index]);
          ((Component) this.slcCountry[index]).gameObject.SetActive(false);
        }
      }
    }
    if (!Object.op_Inequality((Object) this.iconAttackClass, (Object) null))
      return;
    GearGear equippedGearOrInitial = unit1.playerUnit.equippedGearOrInitial;
    if (equippedGearOrInitial != null)
    {
      this.iconAttackClass.Initialize(equippedGearOrInitial.hasAttackClass ? equippedGearOrInitial.gearClassification.attack_classification : unit1.playerUnit.initial_gear.gearClassification.attack_classification, equippedGearOrInitial.attachedElement);
      ((Component) this.iconAttackClass).gameObject.SetActive(true);
    }
    else
      ((Component) this.iconAttackClass).gameObject.SetActive(false);
  }

  private void UpdateUnitParameter(Judgement.BattleParameter u)
  {
    BL.Unit unit = this.modified.value;
    this.setText(this.txt_Lv, unit.lv);
    foreach (UILabel label in this.txt_Fighting)
      this.setColordText(label, u.Combat, u.CombatIncr);
    this.setColordText_BeforeStringNoColorChange(this.txt_Hpmax, u.Hp, u.HpIncr, "/");
    this.setText(this.txt_Jobname, unit.job.name);
    foreach (UILabel label in this.txt_Movement)
      this.setColordText(label, u.Move, u.MoveIncr);
    this.setColordTextChangeSea(this.txt_Attack, u.PhysicalAttack, u.PhysicalAttackIncr);
    this.setColordTextChangeSea(this.txt_Defense, u.PhysicalDefense, u.PhysicalDefenseIncr);
    this.setColordTextChangeSea(this.txt_Matk, u.MagicAttack, u.MagicAttackIncr);
    this.setColordTextChangeSea(this.txt_Mdef, u.MagicDefense, u.MagicDefenseIncr);
    this.setColordTextChangeSea(this.txt_Dexterity, u.Hit, u.HitIncr);
    this.setColordTextChangeSea(this.txt_Evasion, u.Evasion, u.EvasionIncr);
    this.setColordTextChangeSea(this.txt_Critical, u.Critical, u.CriticalIncr);
  }

  private void UpdateAlimentsIcons()
  {
    BL.Unit unit = this.modified.value;
    this.StopLoadAilmentIcon();
    ((Behaviour) this.blink).enabled = false;
    this.blinkGroup.Clear();
    foreach (Component ailmentsIcon in this.ailmentsIcons)
      Object.Destroy((Object) ailmentsIcon.gameObject);
    this.ailmentsIcons.Clear();
    foreach (Object groupIcon in this.groupIcons)
      Object.Destroy(groupIcon);
    this.groupIcons.Clear();
    int index1 = 0;
    int index2 = 0;
    int num = 0;
    foreach (BattleFuncs.InvestSkill investSkill in BattleFuncs.getInvestSkills(this.env.core.getUnitPosition(unit)))
    {
      if (index1 == 0)
        this.AddGroupIcons(num);
      if (index2 >= this.ailmentsPos.Length)
      {
        ++num;
        this.AddGroupIcons(num);
        index2 = 0;
      }
      BattleSkillIcon component = this.battleSkillIconPrefab.CloneAndGetComponent<BattleSkillIcon>(this.groupIcons[num].transform);
      ((Component) component).transform.position = this.ailmentsPos[index2].gameObject.transform.position;
      ((Component) component).transform.localScale = this.ailmentsPos[index2].gameObject.transform.localScale;
      component.SetDepth(((Component) this.dirAilments).GetComponent<UIWidget>().depth - 1);
      this.ailmentsIcons.Add(component);
      ((Component) component).gameObject.SetActive(false);
      this.LoadAilmentIcon(index1, investSkill);
      ++index1;
      ++index2;
    }
    if (this.groupIcons.Count > 1)
    {
      this.blink.SetChildren(this.blinkGroup.ToArray());
      ((Behaviour) this.blink).enabled = true;
    }
    else
      ((Behaviour) this.blink).enabled = false;
    if (index1 > 0)
      EventDelegate.Set(this.dirAilments.onClick, (EventDelegate.Callback) (() => this.onButtonAilments()));
    else
      this.dirAilments.onClick.Clear();
  }

  private void AddGroupIcons(int gIndex)
  {
    this.groupIcons.Add(this.groupSkillsIconPrefab.Clone(((Component) this.dirAilments).gameObject.transform));
    this.blinkGroup.Add(this.groupIcons[gIndex]);
  }

  public void setUnit(BL.Unit unit)
  {
    if (this.modified != null && this.modified.value.Equals(unit))
      return;
    this.setText(this.txt_Hp, unit.hp);
    this.hpGauge.setValue(unit.hp, unit.parameter.Hp, false);
    if (Object.op_Inequality((Object) this.txt_Hp_Large, (Object) null))
    {
      this.setText(this.txt_Hp_Large, unit.hp);
      if (unit.parameter.Hp > 999)
      {
        ((Component) this.txt_Hp_Large).gameObject.SetActive(true);
        ((Component) this.txt_Hp).gameObject.SetActive(false);
        ((Component) this.txt_Hpmax).gameObject.SetActive(false);
      }
      else
      {
        ((Component) this.txt_Hp_Large).gameObject.SetActive(false);
        ((Component) this.txt_Hp).gameObject.SetActive(true);
        ((Component) this.txt_Hpmax).gameObject.SetActive(true);
      }
    }
    this.modified = BL.Observe<BL.Unit>(unit);
    BL.UnitPosition unitPosition = this.env.core.getUnitPosition(unit);
    this.unitPositionModified = BL.Observe<BL.UnitPosition>(unitPosition);
    this.skillEffectsModified = BL.Observe<BL.SkillEffectList>(unit.skillEffects);
    this.panelEffectsModified = BL.Observe<BL.ClassValue<List<BL.SkillEffect>>>(BattleFuncs.getPanel(unitPosition).getSkillEffects());
  }

  public BL.Unit getUnit() => this.modified.value;

  private bool isWaitAIActionCancel
  {
    get
    {
      if (this.waitAIActionCancel == null)
        this.waitAIActionCancel = this.battleManager.getController<BattleStateController>().instWaitCurrentAIActionCancel;
      return this.waitAIActionCancel.value;
    }
  }

  public void onButtonInfo()
  {
    if (!this.battleManager.isBattleEnable || this.env.core.unitCurrent.unit == (BL.Unit) null || this.isWaitAIActionCancel)
      return;
    this.battleManager.popupOpen(this.popupInfoPrefab, nonBattleEnableControl: true);
  }

  public void OpenInfoPopup(BL.Unit unit)
  {
    if (!this.battleManager.isBattleEnable || this.isWaitAIActionCancel)
      return;
    GameObject gameObject = this.battleManager.popupOpen(this.popupInfoPrefab, nonBattleEnableControl: true);
    if (this.env.core.battleInfo.isEarthMode)
    {
      Battle03UIPlayerStatus component = gameObject.GetComponent<Battle03UIPlayerStatus>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.InitFromUnit(unit);
    }
    else
    {
      BattleUI01UnitInformation component = gameObject.GetComponent<BattleUI01UnitInformation>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.InitFromUnit(unit);
    }
    this.resetDirSkillListPos();
  }

  private void setColordText(UILabel label, int v, int bd, string before_string = "")
  {
    label.SetTextLocalize(before_string + (object) v);
    if (bd > 0)
      ((UIWidget) label).color = this.mGreen;
    else if (bd < 0)
      ((UIWidget) label).color = this.mRed;
    else
      ((UIWidget) label).color = this.mOrigin;
  }

  private void setColordTextChangeSea(UILabel label, int v, int bd, string before_string = "")
  {
    label.SetTextLocalize(before_string + (object) v);
    if (bd > 0)
      ((UIWidget) label).color = this.mGreen;
    else if (bd < 0)
      ((UIWidget) label).color = this.mRed;
    else
      ((UIWidget) label).color = this.isSea ? this.mSeaOrigin : this.mOrigin;
  }

  private void setColordText_BeforeStringNoColorChange(
    UILabel label,
    int v,
    int bd,
    string before_string = "")
  {
    ((UIWidget) label).color = this.mOrigin;
    string str = v.ToString();
    if (this.isSea)
      before_string = "[4e4e4e]" + before_string + "[-]";
    string text = bd <= 0 ? (bd >= 0 ? (!this.isSea ? before_string + str : before_string + "[4e4e4e]" + str + "[-]") : before_string + "[fa0000]" + str + "[-]") : before_string + "[00dc1e]" + str + "[-]";
    label.SetTextLocalize(text);
  }

  public void onButtonChangeMenu()
  {
    if (this.isWaitAIActionCancel)
      return;
    this.dirCharacterStatus.SetActive(!this.dirCharacterStatus.activeSelf);
    this.dirCharacterSkill.SetActive(!this.dirCharacterSkill.activeSelf);
    this.resetDirSkillListPos();
  }

  public void onButtonChangeMenuByPlayer()
  {
    foreach (Battle01UIPlayerStatus allPlayerStatu in this.battleStatusScrollParts.allPlayerStatus)
      allPlayerStatu.onButtonChangeMenu();
  }

  public void resetDirSkillListPos()
  {
    this.index_skill_vertical = 0;
    this.setDirSkillListPos();
    if (Object.op_Inequality((Object) this.btnUp, (Object) null))
      ((UIButtonColor) this.btnUp).isEnabled = false;
    if (!Object.op_Inequality((Object) this.btnDown, (Object) null))
      return;
    if (24 < this.skill_num)
      ((UIButtonColor) this.btnDown).isEnabled = true;
    else
      ((UIButtonColor) this.btnDown).isEnabled = false;
  }

  private void setDirSkillListPos()
  {
    if (Object.op_Equality((Object) this.dirSkillList, (Object) null))
      return;
    Vector3 localPosition = this.dirSkillList.transform.localPosition;
    localPosition.y = (float) (0.0 + 62.0 * (double) this.index_skill_vertical);
    this.dirSkillList.transform.localPosition = localPosition;
    this.setDispDirSkillList();
  }

  public void onButtonUp()
  {
    if (this.isWaitAIActionCancel || this.index_skill_vertical <= 0)
      return;
    --this.index_skill_vertical;
    this.setDirSkillListPos();
    ((UIButtonColor) this.btnUp).isEnabled = false;
    ((UIButtonColor) this.btnDown).isEnabled = true;
  }

  public void onButtonDown()
  {
    if (this.isWaitAIActionCancel || this.index_skill_vertical >= 1)
      return;
    ++this.index_skill_vertical;
    this.setDirSkillListPos();
    ((UIButtonColor) this.btnUp).isEnabled = true;
    ((UIButtonColor) this.btnDown).isEnabled = false;
  }

  private void setDispDirSkillList()
  {
    int num1 = 0;
    int num2 = 0;
    foreach (GameObject setSkillIconParent in this.setSkillIconParents)
    {
      setSkillIconParent.SetActive(num2 >= this.index_skill_vertical && num2 < this.index_skill_vertical + 3);
      ++num1;
      if (num1 >= 8)
      {
        num1 = 0;
        ++num2;
      }
    }
  }

  public void resetCharacterStatusMenu()
  {
    this.dirCharacterStatus.SetActive(true);
    this.dirCharacterSkill.SetActive(false);
  }

  private void ClearSkillsInformations()
  {
    for (int index = 0; index < this.dyn_ExtraSkill.Length; ++index)
    {
      foreach (Transform transform in this.dyn_ExtraSkill[index].transform)
      {
        ((Component) transform).gameObject.SetActive(false);
        Object.Destroy((Object) ((Component) transform).gameObject);
      }
    }
    for (int index = 0; index < this.dyn_Unit_Skill.Length; ++index)
    {
      foreach (Transform transform in this.dyn_Unit_Skill[index].transform)
      {
        ((Component) transform).gameObject.SetActive(false);
        Object.Destroy((Object) ((Component) transform).gameObject);
      }
    }
    foreach (GameObject gameObject in this.dyn_ExtraSkill)
      gameObject.GetComponent<UIButton>().onClick.Clear();
    foreach (GameObject gameObject in this.dyn_Unit_Skill)
      gameObject.GetComponent<UIButton>().onClick.Clear();
    this.setSkills.Clear();
    this.setSkillIconParents.Clear();
  }

  private void SetSkillsInformations()
  {
    this.ClearSkillsInformations();
    this.metamorphosisGroupId = this.modified.value.transformationGroupId;
    GameObject[] gameObjectArray = this.dyn_ExtraSkill != null ? ((IEnumerable<GameObject>) this.dyn_ExtraSkill).Concat<GameObject>((IEnumerable<GameObject>) this.dyn_Unit_Skill).ToArray<GameObject>() : this.dyn_Unit_Skill;
    bool flag = !this.isEarth;
    List<PopupSkillDetails.Param> objList = flag ? new List<PopupSkillDetails.Param>(gameObjectArray.Length) : (List<PopupSkillDetails.Param>) null;
    int index = 0;
    foreach (UnitParameter.SkillSortUnit sortedSkill in new UnitParameter(this.modified.value).sortedSkills)
    {
      if (gameObjectArray.Length > index)
      {
        if (flag)
          objList.Add(sortedSkill.toPopupParam);
        switch (sortedSkill.group)
        {
          case UnitParameter.SkillGroup.Leader:
            this.createLSSkillIcon(gameObjectArray[index++], sortedSkill.leaderSkill);
            break;
          case UnitParameter.SkillGroup.Element:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.elementSkill);
            break;
          case UnitParameter.SkillGroup.Multi:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.multiSkill);
            break;
          case UnitParameter.SkillGroup.Overkillers:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.overkillersSkill);
            break;
          case UnitParameter.SkillGroup.Release:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.releaseSkill);
            break;
          case UnitParameter.SkillGroup.Command:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.commandSkill);
            break;
          case UnitParameter.SkillGroup.Princess:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.princessSkill);
            break;
          case UnitParameter.SkillGroup.Grant:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.grantSkill);
            break;
          case UnitParameter.SkillGroup.Duel:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.duelSkill);
            break;
          case UnitParameter.SkillGroup.Equip:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.equipSkill);
            break;
          case UnitParameter.SkillGroup.Extra:
            this.createExtraSkillIcon(gameObjectArray[index++], sortedSkill.extraSkill);
            break;
          case UnitParameter.SkillGroup.JobAbility:
            this.createJobAbilityIcon(gameObjectArray[index++], sortedSkill.jobAbility.skill, sortedSkill.jobAbility.level);
            break;
          case UnitParameter.SkillGroup.Reisou:
            this.createBattleSkillIcon(gameObjectArray[index++], sortedSkill.reisouSkill);
            break;
          case UnitParameter.SkillGroup.SEA:
            this.createBattleSkillIcon(gameObjectArray[index], sortedSkill.SEASkill);
            this.setIconEvent(gameObjectArray[index++], sortedSkill.SEASkill);
            break;
        }
      }
      else
        break;
    }
    this.skill_num = index;
    this.resetDirSkillListPos();
    this.skillParams_ = flag ? objList.ToArray() : (PopupSkillDetails.Param[]) null;
  }

  private void createBattleSkillIcon(GameObject parent, BattleskillSkill skill)
  {
    BattleFuncs.InvestSkill s = new BattleFuncs.InvestSkill();
    s.skill = skill;
    s.isEnemyIcon = this.modified.value.playerUnit.is_enemy;
    GameObject gameObject = this.battleSkillIconPrefab.Clone(parent.transform);
    ((UIWidget) gameObject.GetComponentInChildren<UI2DSprite>()).depth = ((UIWidget) ((Component) parent.transform).GetComponentInChildren<UI2DSprite>()).depth;
    BattleSkillIcon componentInChildren = gameObject.GetComponentInChildren<BattleSkillIcon>();
    this.StartCoroutine(componentInChildren.Init(s));
    this.setSkills.Add(skill.ID);
    this.setSkillIconParents.Add(parent);
    int? transformationGroupId = skill.transformationGroupId;
    if (!transformationGroupId.HasValue)
      return;
    int metamorphosisGroupId = this.metamorphosisGroupId;
    int? nullable = transformationGroupId;
    int valueOrDefault = nullable.GetValueOrDefault();
    if (metamorphosisGroupId == valueOrDefault & nullable.HasValue)
      return;
    this.skillLockIconPrefab.Clone(parent.transform);
    ((UIWidget) componentInChildren.iconSprite).color = Color.gray;
  }

  private void createBattleSkillIcon(GameObject parent, PlayerUnitSkills skill)
  {
    this.createBattleSkillIcon(parent, skill.skill);
    if (this.isEarth)
      this.setIconEventOld(parent, skill.skill, skill.level);
    else
      this.setIconEvent(parent, skill.skill);
  }

  private void createBattleSkillIcon(GameObject parent, GearGearSkill skill)
  {
    this.createBattleSkillIcon(parent, skill.skill);
    if (this.isEarth)
      this.setIconEventOld(parent, skill.skill, skill.skill_level);
    else
      this.setIconEvent(parent, skill.skill);
  }

  private void createBattleSkillIcon(GameObject parent, GearReisouSkill skill)
  {
    this.createBattleSkillIcon(parent, skill.skill);
    if (this.isEarth)
      this.setIconEventOld(parent, skill.skill, skill.skill_level);
    else
      this.setIconEvent(parent, skill.skill);
  }

  private void createLSSkillIcon(GameObject parent, PlayerUnitLeader_skills in_skill)
  {
    this.createBattleSkillIcon(parent, in_skill.skill);
    if (this.isEarth)
      this.setIconEventOld(parent, in_skill.skill, in_skill.level);
    else
      this.setIconEvent(parent, in_skill.skill);
  }

  private void createExtraSkillIcon(GameObject parent, PlayerAwakeSkill awakeSkill)
  {
    this.createBattleSkillIcon(parent, awakeSkill.masterData);
    if (this.isEarth)
      this.setIconEventOld(parent, awakeSkill.masterData, awakeSkill.level);
    else
      this.setIconEvent(parent, awakeSkill.masterData);
  }

  private void createJobAbilityIcon(GameObject parent, BattleskillSkill skill, int level)
  {
    this.createBattleSkillIcon(parent, skill);
    if (this.isEarth)
      this.setJobAbilityIconEventOld(parent, skill, level);
    else
      this.setIconEvent(parent, skill);
  }

  private void setIconEvent(GameObject obj, BattleskillSkill skill)
  {
    UIButton componentInChildren = obj.GetComponentInChildren<UIButton>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    EventDelegate.Set(componentInChildren.onClick, (EventDelegate.Callback) (() => this.onButtonIcon(skill)));
  }

  private void onButtonIcon(BattleskillSkill skill)
  {
    PopupSkillDetails.Param[] skillParams = this.skillParams_;
    int? nullable = skillParams != null ? ((IEnumerable<PopupSkillDetails.Param>) skillParams).FirstIndexOrNull<PopupSkillDetails.Param>((Func<PopupSkillDetails.Param, bool>) (x => x.skill == skill)) : new int?();
    PlayerUnit playerUnit = this.modified?.value?.playerUnit;
    if (!nullable.HasValue || playerUnit == (PlayerUnit) null)
      return;
    PopupSkillDetails.show(this.skillDialogPrefab, this.skillParams_, nullable.Value, playerUnit.is_enemy, isAutoClose: true);
  }

  private void setIconEventOld(GameObject obj, BattleskillSkill skill, int level)
  {
    UIButton componentInChildren = obj.GetComponentInChildren<UIButton>();
    componentInChildren.onClick.Clear();
    EventDelegate.Add(componentInChildren.onClick, (EventDelegate.Callback) (() => this.onButtonIconOld(skill, level)));
  }

  private void setJobAbilityIconEventOld(GameObject obj, BattleskillSkill skill, int level)
  {
    UIButton componentInChildren = obj.GetComponentInChildren<UIButton>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    EventDelegate.Set(componentInChildren.onClick, (EventDelegate.Callback) (() => this.onButtonJobAbilityOld(skill, level)));
  }

  private void onButtonIconOld(BattleskillSkill skill, int level)
  {
    this.hideJobAbilityDialogOld();
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

  private void onButtonJobAbilityOld(BattleskillSkill skill, int level)
  {
    this.hideSkillDialogOld();
    if (!Object.op_Inequality((Object) this.jobAbilityDialog, (Object) null))
      return;
    ((Component) this.jobAbilityDialog).gameObject.SetActive(true);
    this.jobAbilityDialog.show(skill, level);
  }

  private void hideSkillDialogOld()
  {
    if (!Object.op_Inequality((Object) this.skillDialog, (Object) null))
      return;
    Battle0171111Event componentInChildren = this.skillDialog.GetComponentInChildren<Battle0171111Event>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.Hide();
  }

  private void hideJobAbilityDialogOld()
  {
    if (!Object.op_Inequality((Object) this.jobAbilityDialog, (Object) null))
      return;
    this.jobAbilityDialog.hide();
  }

  public void onButtonAilments()
  {
    if (!this.battleManager.isBattleEnable || this.env.core.unitCurrent.unit == (BL.Unit) null || this.isWaitAIActionCancel)
      return;
    this.StartCoroutine(this.openBattleUI01PopupAilmentsDetail());
  }

  private IEnumerator openBattleUI01PopupAilmentsDetail()
  {
    Battle01UIPlayerStatus battle01UiPlayerStatus = this;
    BL.Unit unit = battle01UiPlayerStatus.modified.value;
    IEnumerable<BattleFuncs.InvestSkill> investSkills = BattleFuncs.getInvestSkills(battle01UiPlayerStatus.env.core.getUnitPosition(unit));
    GameObject popup = battle01UiPlayerStatus.ailmentsDetailPrefab.Clone();
    BattleUI01PopupAilmentsDetail menu = popup.GetComponent<BattleUI01PopupAilmentsDetail>();
    popup.SetActive(false);
    IEnumerator e = menu.Init(investSkills);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01UiPlayerStatus.battleManager.popupOpen(popup, isCloned: true);
    menu.grid.Reposition();
    menu.scrollview.ResetPosition();
    popup.SetActive(true);
  }

  public void onButtonSkillList()
  {
    if (!this.battleManager.isBattleEnable || this.env.core.unitCurrent.unit == (BL.Unit) null || this.isWaitAIActionCancel)
      return;
    GameObject prefab = this.skillListPrefab.Clone();
    BattleUI01SkillList componentInChildren = prefab.GetComponentInChildren<BattleUI01SkillList>();
    if (Object.op_Implicit((Object) componentInChildren))
      componentInChildren.setData(this.modified);
    this.battleManager.popupOpen(prefab, isCloned: true);
  }
}
