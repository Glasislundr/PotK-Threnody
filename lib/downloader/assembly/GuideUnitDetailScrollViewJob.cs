// Decompiled with JetBrains decompiler
// Type: GuideUnitDetailScrollViewJob
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
public class GuideUnitDetailScrollViewJob : MonoBehaviour
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
  protected GameObject[] dir_JobAbility;
  [SerializeField]
  protected UILabel[] txt_JobAbilityLv;
  [SerializeField]
  protected UILabel[] txt_JobAbilityName;
  [SerializeField]
  protected GameObject dyn_familyDetailDialog;
  [SerializeField]
  protected GameObject dyn_terraiAbilityDialog;
  private UnitUnit unit;
  private MasterDataTable.UnitJob job;
  private GameObject familyIconPrefab;
  private GameObject familyDiarogPrefab;
  private GameObject terraiAbilityDialogPrefab;
  private Unit0042FloatingFamilyDialog familyDiarogObject;
  private Unit0042FloatingTerraiAbilityDialog terraiAbilityDialog;
  [SerializeField]
  protected UIButton TerraiAbilityButton;
  [SerializeField]
  protected UIButton JobTrainingButton;
  private bool isOpeningJobAbility;
  private GameObject abilityDialogPrefab;
  private PlayerUnitJob_abilities[] jobAbilities;

  public IEnumerator init(UnitUnit unit, MasterDataTable.UnitJob job)
  {
    this.unit = unit;
    this.job = job;
    Future<GameObject> loader = (Future<GameObject>) null;
    loader = new ResourceObject("Prefabs/SkillFamily/SkillFamilyIcon").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.familyIconPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/unit004_2/SpecialPoint_DetailDialog").Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.familyDiarogPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/unit004_2/TerraiAbilityDialog").Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.terraiAbilityDialogPrefab = loader.Result;
    if (Object.op_Inequality((Object) this.dyn_familyDetailDialog, (Object) null))
    {
      if (Object.op_Equality((Object) this.familyDiarogObject, (Object) null))
      {
        this.dyn_familyDetailDialog.transform.Clear();
        this.familyDiarogObject = this.familyDiarogPrefab.Clone(this.dyn_familyDetailDialog.transform).GetComponentInChildren<Unit0042FloatingFamilyDialog>();
      }
      ((Component) ((Component) this.familyDiarogObject).transform.parent).gameObject.SetActive(false);
    }
    if (Object.op_Inequality((Object) this.dyn_terraiAbilityDialog, (Object) null))
    {
      if (Object.op_Equality((Object) this.terraiAbilityDialog, (Object) null))
      {
        this.dyn_terraiAbilityDialog.transform.Clear();
        this.terraiAbilityDialog = this.terraiAbilityDialogPrefab.Clone(this.dyn_terraiAbilityDialog.transform).GetComponentInChildren<Unit0042FloatingTerraiAbilityDialog>();
      }
      ((Component) ((Component) this.terraiAbilityDialog).transform.parent).gameObject.SetActive(false);
    }
    if (Object.op_Equality((Object) this.abilityDialogPrefab, (Object) null))
    {
      Future<GameObject> abilityDialogPrefabF = Res.Prefabs.unit004_Job.Unit_JobCharacteristic_Dialog.Load<GameObject>();
      yield return (object) abilityDialogPrefabF.Wait();
      this.abilityDialogPrefab = abilityDialogPrefabF.Result;
      abilityDialogPrefabF = (Future<GameObject>) null;
    }
    this.txt_Movement.SetTextLocalize(job.movement);
    this.setFamilyIcon();
    string str = string.Empty;
    if (!job.spWeaponName1.isEmptyOrWhitespace())
      str += Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_NAME.F((object) job.spWeaponName1);
    if (!job.spWeaponName2.isEmptyOrWhitespace())
      str = str + Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_SEPARATE_CHAR + Consts.GetInstance().UNIT_00420_JOB_SP_WEAPON_NAME.F((object) job.spWeaponName2);
    this.txt_SpWeapon.text = "";
    if (!string.IsNullOrEmpty(str))
      this.txt_SpWeapon.SetText("装備可能: " + str);
    this.txt_Restriction.text = "";
    int? classificationPattern;
    if (string.IsNullOrEmpty(str) && job.classification_GearClassificationPattern.HasValue)
    {
      classificationPattern = job.classification_GearClassificationPattern;
      int num = 0;
      if (!(classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue))
      {
        this.txt_Restriction.SetText("武器制限: " + job.classification.name + "のみ");
        goto label_31;
      }
    }
    if (!string.IsNullOrEmpty(str) && job.classification_GearClassificationPattern.HasValue)
    {
      classificationPattern = job.classification_GearClassificationPattern;
      int num = 0;
      if (!(classificationPattern.GetValueOrDefault() == num & classificationPattern.HasValue))
        Debug.LogError((object) ("装備可能と装備制限が両方存在します: 装備可能→" + str + " 装備制限→" + job.classification.name));
    }
label_31:
    ((Component) this.TerraiAbilityButton).gameObject.SetActive(false);
    ((UIButtonColor) this.TerraiAbilityButton).isEnabled = false;
    foreach (JobCharacteristics jobAbility1 in job.JobAbilities)
    {
      JobCharacteristics jobAbility = jobAbility1;
      if (jobAbility.skill.IsLand)
      {
        ((Component) this.TerraiAbilityButton).gameObject.SetActive(true);
        ((UIButtonColor) this.TerraiAbilityButton).isEnabled = true;
        EventDelegate.Set(this.TerraiAbilityButton.onClick, (EventDelegate.Callback) (() =>
        {
          this.terraiAbilityDialog.SetData(jobAbility.skill);
          this.terraiAbilityDialog.Show();
        }));
        break;
      }
    }
    ((Component) this.JobTrainingButton).gameObject.SetActive(false);
    ((UIButtonColor) this.JobTrainingButton).isEnabled = false;
    IEnumerable<AttackMethod> attackMethods = ((IEnumerable<AttackMethod>) MasterData.AttackMethodList).Where<AttackMethod>((Func<AttackMethod, bool>) (x => x.unit == unit && x.job == job));
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
    this.setJobAbility();
  }

  private void setFamilyIcon()
  {
    int index1 = 0;
    UnitFamily[] source = this.unit.FamiliesWithJob(this.job.ID);
    int num = ((IEnumerable<UnitFamily>) source).Count<UnitFamily>();
    for (int index2 = 0; index1 < this.dyn_FamilyIcon.Length && index2 < num; ++index2)
    {
      UnitFamily family = source[index2];
      UnitFamilyValue familyValue = ((IEnumerable<UnitFamilyValue>) MasterData.UnitFamilyValueList).FirstOrDefault<UnitFamilyValue>((Func<UnitFamilyValue, bool>) (x => (UnitFamily) x.ID == family));
      if (familyValue != null && familyValue.is_disp)
      {
        this.createFamilyIcon(this.dyn_FamilyIcon[index1], familyValue);
        this.dyn_FamilyIcon[index1].SetActive(true);
        ++index1;
      }
    }
    if (index1 <= 0)
    {
      UnitFamilyValue familyValue = ((IEnumerable<UnitFamilyValue>) MasterData.UnitFamilyValueList).FirstOrDefault<UnitFamilyValue>((Func<UnitFamilyValue, bool>) (x => x.ID == 0));
      this.createFamilyIcon(this.dyn_FamilyIcon[index1], familyValue);
      this.dyn_FamilyIcon[index1].SetActive(true);
      index1 = 1;
    }
    for (int index3 = index1; index3 < this.dyn_FamilyIcon.Length; ++index3)
      this.dyn_FamilyIcon[index3].SetActive(false);
  }

  private void setJobAbility()
  {
    ((IEnumerable<GameObject>) this.dir_JobAbility).ForEach<GameObject>((Action<GameObject>) (x => x.gameObject.SetActive(false)));
    if (this.job.JobAbilities.Length == 0)
      return;
    List<PlayerUnitJob_abilities> unitJobAbilitiesList = new List<PlayerUnitJob_abilities>(4);
    foreach (JobCharacteristics jobAbility in this.job.JobAbilities)
    {
      if (!jobAbility.skill.IsLand)
        unitJobAbilitiesList.Add(new PlayerUnitJob_abilities()
        {
          job_ability_id = jobAbility.ID,
          level = jobAbility.skill.upper_level
        });
    }
    this.jobAbilities = unitJobAbilitiesList.ToArray();
    for (int index = 0; index < this.dir_JobAbility.Length && index < this.jobAbilities.Length; ++index)
    {
      PlayerUnitJob_abilities jobAbility = this.jobAbilities[index];
      this.txt_JobAbilityLv[index].SetTextLocalize(jobAbility.level > 0 ? jobAbility.level.ToString() : Consts.GetInstance().SKILL_LEVEL_NONE);
      this.txt_JobAbilityName[index].SetText(jobAbility.skill.name);
      this.dir_JobAbility[index].SetActive(true);
    }
  }

  private void createFamilyIcon(GameObject parent, UnitFamilyValue familyValue)
  {
    parent.transform.Clear();
    GameObject gameObject = this.familyIconPrefab.Clone();
    gameObject.gameObject.SetParent(parent);
    gameObject.GetComponentInChildren<SkillfullnessIcon>().InitKindId((UnitFamily) familyValue.ID);
    EventDelegate.Set(parent.GetComponentInChildren<UIButton>().onClick, (EventDelegate.Callback) (() => this.onButtonFamilyIcon(familyValue)));
  }

  public void onButtonFamilyIcon(UnitFamilyValue familyValue)
  {
    this.familyDiarogObject.setData(this.familyIconPrefab, familyValue);
    this.familyDiarogObject.Show();
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1006");
  }

  public void IbtnJobTokusei0() => this.openJobAbilityDialog(0);

  public void IbtnJobTokusei1() => this.openJobAbilityDialog(1);

  public void IbtnJobTokusei2() => this.openJobAbilityDialog(2);

  public void IbtnJobTokusei3() => this.openJobAbilityDialog(3);

  private void openJobAbilityDialog(int range)
  {
    if (Object.op_Equality((Object) this.abilityDialogPrefab, (Object) null) || this.jobAbilities == null || this.jobAbilities.Length <= range)
      return;
    this.StartCoroutine(this.doOpenJobAbilityDialog(range));
  }

  private IEnumerator doOpenJobAbilityDialog(int range)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.abilityDialogPrefab);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Unit004JobDialog>().Init(this.jobAbilities[range]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public void onButtonOpenJobAbility()
  {
    if (Singleton<PopupManager>.GetInstance().isOpen || Singleton<PopupManager>.GetInstance().isRunningCoroutine || this.isOpeningJobAbility)
      return;
    this.StartCoroutine(this.ShowJobTrainingPopup());
  }

  private IEnumerator ShowJobTrainingPopup()
  {
    GuideUnitDetailScrollViewJob detailScrollViewJob = this;
    detailScrollViewJob.isOpeningJobAbility = true;
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
      e = script.Init(detailScrollViewJob.unit, detailScrollViewJob.job.ID);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      detailScrollViewJob.StartCoroutine(script.ResetScroll());
      detailScrollViewJob.isOpeningJobAbility = false;
    }
  }
}
