// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollViewJob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitDetails;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class DetailMenuScrollViewJob : DetailMenuScrollViewBase
{
  [SerializeField]
  protected UILabel txt_Movement;
  [SerializeField]
  protected UILabel txt_SpWeapon;
  [SerializeField]
  protected UILabel txt_Restriction;
  [SerializeField]
  protected GameObject[] dyn_FamilyIcon;
  [SerializeField]
  protected DetailMenuJobAbilityParts[] dir_JobAbility;
  [SerializeField]
  protected GameObject dyn_familyDetailDialog;
  [SerializeField]
  protected GameObject dyn_terraiAbilityDialog;
  [SerializeField]
  protected UIButton TerraiAbilityButton;
  [SerializeField]
  protected UIButton JobTrainingButton;
  private bool isOpeningJobAbility;
  [SerializeField]
  private DetailMenu DetailMenuObject;
  private PlayerUnit playerUnit;
  private GameObject familyIconPrefab;
  private Unit0042FloatingFamilyDialog familyDiarogObject;
  private Unit0042FloatingTerraiAbilityDialog terraiAbilityDialog;
  [SerializeField]
  private DetailMenuJobTab jobTab;
  private Coroutine coroutineUpdater_;
  [SerializeField]
  private GameObject topNextFamilies;
  [SerializeField]
  private GameObject[] dyn_NextFamilyIcon;
  [SerializeField]
  private float localPosBeforeSingleFamily_x_ = -176f;
  private float? localPosBeforeDoubleFamily_x_;

  public UIButton[] JobAbilityButtons
  {
    get
    {
      UIButton[] jobAbilityButtons = new UIButton[this.dir_JobAbility.Length];
      for (int index = 0; index < this.dir_JobAbility.Length; ++index)
        jobAbilityButtons[index] = ((Component) this.dir_JobAbility[index]).GetComponent<UIButton>();
      return jobAbilityButtons;
    }
  }

  public override IEnumerator initAsync(
    PlayerUnit playerUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs)
  {
    this.playerUnit = playerUnit;
    this.familyIconPrefab = prefabs[0];
    GameObject prefab1 = prefabs[1];
    GameObject prefab2 = prefabs[2];
    if (Object.op_Inequality((Object) this.dyn_familyDetailDialog, (Object) null))
    {
      if (Object.op_Equality((Object) this.familyDiarogObject, (Object) null))
      {
        this.dyn_familyDetailDialog.transform.Clear();
        this.familyDiarogObject = prefab1.Clone(this.dyn_familyDetailDialog.transform).GetComponentInChildren<Unit0042FloatingFamilyDialog>();
      }
      ((Component) ((Component) this.familyDiarogObject).transform.parent).gameObject.SetActive(false);
    }
    if (Object.op_Inequality((Object) this.dyn_terraiAbilityDialog, (Object) null))
    {
      if (Object.op_Equality((Object) this.terraiAbilityDialog, (Object) null))
      {
        this.dyn_terraiAbilityDialog.transform.Clear();
        this.terraiAbilityDialog = prefab2.Clone(this.dyn_terraiAbilityDialog.transform).GetComponentInChildren<Unit0042FloatingTerraiAbilityDialog>();
      }
      ((Component) ((Component) this.terraiAbilityDialog).transform.parent).gameObject.SetActive(false);
    }
    this.setFamilyIcon();
    this.setJobAbility();
    yield break;
  }

  public override bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    MasterDataTable.UnitJob jobData = playerUnit.getJobData();
    this.txt_Movement.SetTextLocalize((this.isMemory ? Judgement.NonBattleParameter.FromPlayerUnitMemory(playerUnit) : Judgement.NonBattleParameter.FromPlayerUnit(playerUnit, this.controlFlags.IsOn(Control.SelfAbility))).Move);
    this.txt_SpWeapon.text = "";
    string str = string.Empty;
    if (!jobData.spWeaponName1.isEmptyOrWhitespace())
      str += Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_NAME.F((object) jobData.spWeaponName1);
    if (!jobData.spWeaponName2.isEmptyOrWhitespace())
      str = str + Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_SEPARATE_CHAR + Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_NAME.F((object) jobData.spWeaponName2);
    if (!string.IsNullOrEmpty(str))
      this.txt_SpWeapon.SetText("装備可能\n  " + str);
    if (string.IsNullOrEmpty(str) && jobData.classification_GearClassificationPattern.HasValue)
    {
      int? classificationPattern = jobData.classification_GearClassificationPattern;
      int num = 0;
      if (!(classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue))
      {
        this.txt_Restriction.SetText("武器制限\n  " + jobData.classification.name + "のみ");
        goto label_12;
      }
    }
    if (!string.IsNullOrEmpty(str) && jobData.classification_GearClassificationPattern.HasValue)
    {
      int? classificationPattern = jobData.classification_GearClassificationPattern;
      int num = 0;
      if (!(classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue))
        Debug.LogError((object) ("装備可能と装備制限が両方存在します: 装備可能→" + str + " 装備制限→" + jobData.classification.name));
    }
label_12:
    if (Object.op_Implicit((Object) this.TerraiAbilityButton))
    {
      ((Component) this.TerraiAbilityButton).gameObject.SetActive(false);
      ((UIButtonColor) this.TerraiAbilityButton).isEnabled = false;
      PlayerUnitJob_abilities[] jobAbilities = playerUnit.job_abilities;
      if (jobAbilities != null)
      {
        foreach (PlayerUnitJob_abilities unitJobAbilities in jobAbilities)
        {
          BattleskillSkill skill = unitJobAbilities.skill;
          if (skill != null && skill.IsLand)
          {
            ((Component) this.TerraiAbilityButton).gameObject.SetActive(true);
            ((UIButtonColor) this.TerraiAbilityButton).isEnabled = true;
            EventDelegate.Set(this.TerraiAbilityButton.onClick, (EventDelegate.Callback) (() =>
            {
              this.terraiAbilityDialog.SetData(skill);
              this.terraiAbilityDialog.Show();
            }));
            break;
          }
        }
      }
    }
    if (Object.op_Implicit((Object) this.JobTrainingButton))
    {
      ((Component) this.JobTrainingButton).gameObject.SetActive(false);
      ((UIButtonColor) this.JobTrainingButton).isEnabled = false;
      IEnumerable<AttackMethod> attackMethods = ((IEnumerable<AttackMethod>) MasterData.AttackMethodList).Where<AttackMethod>((Func<AttackMethod, bool>) (x => x.unit == playerUnit.unit && x.job.ID == playerUnit.job_id));
      if (attackMethods != null)
      {
        foreach (AttackMethod attackMethod in attackMethods)
        {
          if (attackMethod.skill != null)
          {
            ((Component) this.JobTrainingButton).gameObject.SetActive(true);
            ((UIButtonColor) this.JobTrainingButton).isEnabled = true;
            break;
          }
        }
      }
    }
    return true;
  }

  private void setFamilyIcon()
  {
    this.playerUnit.getJobData();
    foreach (GameObject gameObject in this.dyn_FamilyIcon)
      gameObject.gameObject.SetActive(false);
    UnitFamily[] families = this.playerUnit.Families;
    int index1 = 0;
    int length = families.Length;
    for (int index2 = 0; index1 < this.dyn_FamilyIcon.Length && index2 < length; ++index2)
    {
      UnitFamily family = families[index2];
      UnitFamilyValue familyValue = Array.Find<UnitFamilyValue>(MasterData.UnitFamilyValueList, (Predicate<UnitFamilyValue>) (x => (UnitFamily) x.ID == family));
      if (familyValue != null && familyValue.is_disp)
      {
        this.createFamilyIcon(this.dyn_FamilyIcon[index1], familyValue);
        this.dyn_FamilyIcon[index1].SetActive(true);
        ++index1;
      }
    }
    if (index1 > 0)
      return;
    this.createFamilyIcon(this.dyn_FamilyIcon[index1], MasterData.UnitFamilyValue[0]);
    this.dyn_FamilyIcon[index1].SetActive(true);
  }

  private void setJobAbility()
  {
    if (Object.op_Inequality((Object) this.jobTab, (Object) null))
    {
      this.DetailMenuObject.preUpdateJobAblility = this.getPreUpdatePlayerUnits();
      this.DetailMenuObject.updatedJobAbility = new Action(this.updateJobAbility);
      this.jobTab.initialize(this.playerUnit, this.isDisabledJobChange, new Action<DetailMenuJobAbilityParts, PlayerUnitJob_abilities, bool, bool>(this.DetailMenuObject.setJobAbility), new Action<UIButton, int>(this.setJobChange));
    }
    else
    {
      PlayerUnitJob_abilities[] jobAbilities = this.playerUnit.job_abilities;
      if (jobAbilities == null || jobAbilities.Length == 0)
      {
        ((IEnumerable<DetailMenuJobAbilityParts>) this.dir_JobAbility).Select<DetailMenuJobAbilityParts, GameObject>((Func<DetailMenuJobAbilityParts, GameObject>) (x => ((Component) x).gameObject)).SetActives(false);
      }
      else
      {
        int count = 0;
        this.DetailMenuObject.updatedJobAbility = new Action(this.updateJobAbility);
        for (int index = 0; index < jobAbilities.Length && count < this.dir_JobAbility.Length; ++index)
        {
          BattleskillSkill skill = jobAbilities[index].skill;
          if ((skill != null ? (skill.IsLand ? 1 : 0) : 1) == 0)
            this.DetailMenuObject.setJobAbility(this.dir_JobAbility[count++], jobAbilities[index]);
        }
        if (count >= this.dir_JobAbility.Length)
          return;
        ((IEnumerable<DetailMenuJobAbilityParts>) this.dir_JobAbility).Skip<DetailMenuJobAbilityParts>(count).Select<DetailMenuJobAbilityParts, GameObject>((Func<DetailMenuJobAbilityParts, GameObject>) (x => ((Component) x).gameObject)).SetActives(false);
      }
    }
  }

  private bool isDisabledJobChange
  {
    get
    {
      return this.controlFlags.IsAnyOn(Control.Limited | Control.CustomDeck) || this.playerUnit.is_storage || this.playerUnit.is_enemy || this.playerUnit.is_guest;
    }
  }

  private void setJobChange(UIButton btn, int jobId)
  {
    if (this.playerUnit.getJobData().ID != jobId)
    {
      ((UIButtonColor) btn).isEnabled = true;
      EventDelegate.Set(btn.onClick, (EventDelegate.Callback) (() => this.DetailMenuObject.onClickedJobChange(jobId)));
    }
    else
    {
      btn.onClick.Clear();
      ((UIButtonColor) btn).isEnabled = false;
    }
  }

  private Action<Action> getPreUpdatePlayerUnits()
  {
    Unit0042Menu inParents = NGUITools.FindInParents<Unit0042Menu>(((Component) this).gameObject);
    return !Object.op_Inequality((Object) inParents, (Object) null) ? (Action<Action>) null : new Action<Action>(inParents.UploadFavorites);
  }

  private void updateJobAbility()
  {
    if (this.coroutineUpdater_ != null)
      return;
    this.coroutineUpdater_ = Singleton<NGSceneManager>.GetInstance().StartCoroutine(this.doUpdateJobAbility());
  }

  private IEnumerator doUpdateJobAbility()
  {
    DetailMenuScrollViewJob menuScrollViewJob = this;
    while (Singleton<PopupManager>.GetInstance().isOpen || Singleton<PopupManager>.GetInstance().isRunningCoroutine)
      yield return (object) null;
    Unit0042Menu menu = NGUITools.FindInParents<Unit0042Menu>(((Component) menuScrollViewJob).gameObject);
    if (Object.op_Inequality((Object) menu, (Object) null))
    {
      Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false, isNonSe: true);
      yield return (object) null;
      PlayerUnit[] newList = SMManager.Get<PlayerUnit[]>();
      yield return (object) menu.UpdateAllPage(((IEnumerable<PlayerUnit>) menu.UnitList).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x => Array.Find<PlayerUnit>(newList, (Predicate<PlayerUnit>) (y => y.id == x.id)))).ToArray<PlayerUnit>());
      yield return (object) null;
      Singleton<PopupManager>.GetInstance().dismiss();
    }
    menuScrollViewJob.coroutineUpdater_ = (Coroutine) null;
  }

  private void createFamilyIcon(GameObject parent, UnitFamilyValue familyValue)
  {
    foreach (SkillfullnessIcon componentsInChild in parent.GetComponentsInChildren<SkillfullnessIcon>(true))
    {
      ((Component) componentsInChild).transform.parent = (Transform) null;
      Object.Destroy((Object) ((Component) componentsInChild).gameObject);
    }
    GameObject gameObject = this.familyIconPrefab.Clone();
    gameObject.gameObject.SetParent(parent);
    gameObject.GetComponent<SkillfullnessIcon>().InitKindId((UnitFamily) familyValue.ID);
    EventDelegate.Set(parent.GetComponentInChildren<UIButton>().onClick, (EventDelegate.Callback) (() => this.onButtonFamilyIcon(familyValue)));
  }

  public void onButtonFamilyIcon(UnitFamilyValue familyValue)
  {
    this.familyDiarogObject.setData(this.familyIconPrefab, familyValue);
    this.familyDiarogObject.Show();
  }

  public void onButtonOpenJobAbility()
  {
    if (Singleton<PopupManager>.GetInstance().isOpen || Singleton<PopupManager>.GetInstance().isRunningCoroutine || this.isOpeningJobAbility)
      return;
    this.StartCoroutine(this.ShowJobTrainingPopup());
  }

  private IEnumerator ShowJobTrainingPopup()
  {
    DetailMenuScrollViewJob menuScrollViewJob = this;
    menuScrollViewJob.isOpeningJobAbility = true;
    Future<GameObject> loader = (Future<GameObject>) null;
    loader = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.popup.popup_004_2_job_training_menu_sea__anim_popup01.Load<GameObject>() : Res.Prefabs.popup.popup_004_2_job_training_menu__anim_popup01.Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) loader.Result, (Object) null))
    {
      GameObject popup = loader.Result.Clone();
      popup.SetActive(false);
      Popup0042JobTrainingMenu script = popup.GetComponent<Popup0042JobTrainingMenu>();
      e = script.Init(menuScrollViewJob.playerUnit.unit, menuScrollViewJob.playerUnit.job_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      menuScrollViewJob.StartCoroutine(script.ResetScroll());
      menuScrollViewJob.isOpeningJobAbility = false;
    }
  }

  public override IEnumerator initAsyncDiffMode(
    PlayerUnit playerUnit,
    PlayerUnit prevUnit,
    IDetailMenuContainer menuContainer)
  {
    this.playerUnit = playerUnit;
    MasterDataTable.UnitJob jobData = playerUnit.getJobData();
    MasterDataTable.UnitJob unitJob = prevUnit?.getJobData() ?? jobData;
    string changedValueColor = jobData.ID == unitJob.ID ? "" : Consts.GetInstance().JOBCHANGE_CHANGED_VALUE_COLOR;
    int move = Judgement.NonBattleParameter.FromPlayerUnit(playerUnit, true).Move;
    int prev = prevUnit != (PlayerUnit) null ? Judgement.NonBattleParameter.FromPlayerUnit(prevUnit, true).Move : move;
    Util.SetTextIntegerWithStateColor(this.txt_Movement, move, prev);
    this.txt_SpWeapon.text = "";
    string str1 = string.Empty;
    if (!jobData.spWeaponName1.isEmptyOrWhitespace())
    {
      string str2;
      if (!(jobData.spWeaponName1 == unitJob.spWeaponName1))
        str2 = Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_NAME_CHANGE.F((object) jobData.spWeaponName1);
      else
        str2 = Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_NAME.F((object) jobData.spWeaponName1);
      str1 = str2;
    }
    if (!jobData.spWeaponName2.isEmptyOrWhitespace())
    {
      string str3 = str1;
      string weaponSeparateChar = Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_SEPARATE_CHAR;
      string str4;
      if (!(jobData.spWeaponName2 == unitJob.spWeaponName2))
        str4 = Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_NAME_CHANGE.F((object) jobData.spWeaponName2);
      else
        str4 = Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_NAME.F((object) jobData.spWeaponName2);
      str1 = str3 + weaponSeparateChar + str4;
    }
    if (!string.IsNullOrEmpty(str1))
      this.txt_SpWeapon.SetText("装備可能\n  " + changedValueColor + str1);
    int? classificationPattern;
    if (string.IsNullOrEmpty(str1) && jobData.classification_GearClassificationPattern.HasValue)
    {
      classificationPattern = jobData.classification_GearClassificationPattern;
      int num = 0;
      if (!(classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue))
      {
        this.txt_Restriction.SetText("武器制限\n  " + changedValueColor + jobData.classification.name + "のみ");
        goto label_18;
      }
    }
    if (!string.IsNullOrEmpty(str1) && jobData.classification_GearClassificationPattern.HasValue)
    {
      classificationPattern = jobData.classification_GearClassificationPattern;
      int num = 0;
      if (!(classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue))
        Debug.LogError((object) ("装備可能と装備制限が両方存在します: 装備可能→" + str1 + " 装備制限→" + jobData.classification.name));
    }
label_18:
    ((Component) this.TerraiAbilityButton).gameObject.SetActive(false);
    ((UIButtonColor) this.TerraiAbilityButton).isEnabled = false;
    PlayerUnitJob_abilities[] jobAbilities = playerUnit.job_abilities;
    if (jobAbilities != null)
    {
      foreach (PlayerUnitJob_abilities unitJobAbilities in jobAbilities)
      {
        BattleskillSkill skill = unitJobAbilities.skill;
        if (skill != null && skill.IsLand)
        {
          ((Component) this.TerraiAbilityButton).gameObject.SetActive(true);
          ((UIButtonColor) this.TerraiAbilityButton).isEnabled = true;
          EventDelegate.Set(this.TerraiAbilityButton.onClick, (EventDelegate.Callback) (() =>
          {
            this.terraiAbilityDialog.SetData(skill);
            this.terraiAbilityDialog.Show();
          }));
          break;
        }
      }
    }
    this.familyIconPrefab = menuContainer.skillfullnessIconPrefab;
    if (Object.op_Inequality((Object) this.dyn_familyDetailDialog, (Object) null))
    {
      if (Object.op_Equality((Object) this.familyDiarogObject, (Object) null))
      {
        this.dyn_familyDetailDialog.transform.Clear();
        this.familyDiarogObject = menuContainer.specialPointDetailDialogPrefab.Clone(this.dyn_familyDetailDialog.transform).GetComponentInChildren<Unit0042FloatingFamilyDialog>();
      }
      ((Component) ((Component) this.familyDiarogObject).transform.parent).gameObject.SetActive(false);
    }
    if (Object.op_Inequality((Object) this.dyn_terraiAbilityDialog, (Object) null))
    {
      if (Object.op_Equality((Object) this.terraiAbilityDialog, (Object) null))
      {
        this.dyn_terraiAbilityDialog.transform.Clear();
        this.terraiAbilityDialog = menuContainer.terraiAbilityDialogPrefab.Clone(this.dyn_terraiAbilityDialog.transform).GetComponentInChildren<Unit0042FloatingTerraiAbilityDialog>();
      }
      ((Component) ((Component) this.terraiAbilityDialog).transform.parent).gameObject.SetActive(false);
    }
    ((Component) this.JobTrainingButton).gameObject.SetActive(false);
    ((UIButtonColor) this.JobTrainingButton).isEnabled = false;
    IEnumerable<AttackMethod> attackMethods = ((IEnumerable<AttackMethod>) MasterData.AttackMethodList).Where<AttackMethod>((Func<AttackMethod, bool>) (x => x.unit == playerUnit.unit && x.job.ID == playerUnit.job_id));
    if (attackMethods != null)
    {
      foreach (AttackMethod attackMethod in attackMethods)
      {
        if (attackMethod.skill != null)
        {
          ((Component) this.JobTrainingButton).gameObject.SetActive(true);
          ((UIButtonColor) this.JobTrainingButton).isEnabled = true;
          break;
        }
      }
    }
    this.setFamilyInfo(prevUnit);
    this.setJobAbility(prevUnit);
    yield break;
  }

  private void setJobAbility(PlayerUnit prevUnit)
  {
    PlayerUnitJob_abilities[] jobAbilities = this.playerUnit.job_abilities;
    if (jobAbilities == null || jobAbilities.Length == 0)
    {
      ((IEnumerable<DetailMenuJobAbilityParts>) this.dir_JobAbility).SetActives<DetailMenuJobAbilityParts>(false);
    }
    else
    {
      bool bViewMode = prevUnit != (PlayerUnit) null;
      PlayerUnitJob_abilities[] source = bViewMode ? prevUnit.job_abilities : this.playerUnit.job_abilities;
      HashSet<int> intSet = source != null ? new HashSet<int>(((IEnumerable<PlayerUnitJob_abilities>) source).Select<PlayerUnitJob_abilities, int>((Func<PlayerUnitJob_abilities, int>) (x => x.skill_id)).Where<int>((Func<int, bool>) (i => i != 0))) : new HashSet<int>();
      int iStart = 0;
      foreach (PlayerUnitJob_abilities jobAbility in jobAbilities)
      {
        if (!jobAbility.skill.IsLand)
        {
          if (this.dir_JobAbility.Length > iStart)
            this.DetailMenuObject.setJobAbility(this.dir_JobAbility[iStart++], jobAbility, !intSet.Contains(jobAbility.skill_id), bViewMode);
          else
            break;
        }
      }
      if (this.dir_JobAbility.Length <= iStart)
        return;
      ((IEnumerable<DetailMenuJobAbilityParts>) this.dir_JobAbility).SetActiveRange<DetailMenuJobAbilityParts>(false, iStart);
    }
  }

  private void setFamilyInfo(PlayerUnit prevUnit)
  {
    UnitFamilyValue[] unitFamilyValues1 = prevUnit != (PlayerUnit) null ? this.getVisibleUnitFamilyValues(prevUnit) : (UnitFamilyValue[]) null;
    UnitFamilyValue[] unitFamilyValues2 = this.getVisibleUnitFamilyValues(this.playerUnit);
    if (Object.op_Equality((Object) this.topNextFamilies, (Object) null) || prevUnit == (PlayerUnit) null || unitFamilyValues1 != null && ((IEnumerable<UnitFamilyValue>) unitFamilyValues1).Select<UnitFamilyValue, int>((Func<UnitFamilyValue, int>) (a => a.ID)).SequenceEqual<int>(((IEnumerable<UnitFamilyValue>) unitFamilyValues2).Select<UnitFamilyValue, int>((Func<UnitFamilyValue, int>) (b => b.ID))))
    {
      this.inactivateGameObject(this.topNextFamilies);
      this.setFamilyIcons(this.dyn_FamilyIcon, unitFamilyValues2);
    }
    else
    {
      this.setFamilyIcons(this.dyn_FamilyIcon, unitFamilyValues1);
      this.topNextFamilies.SetActive(true);
      if (unitFamilyValues1.Length == 1)
      {
        if (!this.localPosBeforeDoubleFamily_x_.HasValue)
          this.localPosBeforeDoubleFamily_x_ = new float?(this.topNextFamilies.transform.localPosition.x);
        this.topNextFamilies.transform.localPosition = Vector2.op_Implicit(new Vector2(this.localPosBeforeSingleFamily_x_, this.topNextFamilies.transform.localPosition.y));
      }
      else if (this.localPosBeforeDoubleFamily_x_.HasValue)
        this.topNextFamilies.transform.localPosition = Vector2.op_Implicit(new Vector2(this.localPosBeforeDoubleFamily_x_.Value, this.topNextFamilies.transform.localPosition.y));
      this.setFamilyIcons(this.dyn_NextFamilyIcon, unitFamilyValues2);
    }
  }

  private void setFamilyIcons(GameObject[] links, UnitFamilyValue[] familyValues)
  {
    for (int index = 0; index < links.Length; ++index)
    {
      if (index >= familyValues.Length)
      {
        this.inactivateGameObject(links[index]);
      }
      else
      {
        links[index].SetActive(true);
        this.createFamilyIcon(links[index], familyValues[index]);
      }
    }
  }

  private UnitFamilyValue[] getVisibleUnitFamilyValues(PlayerUnit pu)
  {
    UnitFamilyValue unitFamilyValue;
    UnitFamilyValue[] array = ((IEnumerable<UnitFamily>) pu.Families).OrderBy<UnitFamily, int>((Func<UnitFamily, int>) (e => (int) e)).Select<UnitFamily, UnitFamilyValue>((Func<UnitFamily, UnitFamilyValue>) (e => !MasterData.UnitFamilyValue.TryGetValue((int) e, out unitFamilyValue) ? (UnitFamilyValue) null : unitFamilyValue)).Where<UnitFamilyValue>((Func<UnitFamilyValue, bool>) (d => d != null && d.is_disp)).ToArray<UnitFamilyValue>();
    if (array.Length != 0)
      return array;
    return new UnitFamilyValue[1]
    {
      MasterData.UnitFamilyValue[0]
    };
  }
}
