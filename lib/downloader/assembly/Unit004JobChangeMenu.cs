// Decompiled with JetBrains decompiler
// Type: Unit004JobChangeMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using JobChangeData;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit004JobChangeMenu : BackButtonMenuBase, IDetailMenuContainer
{
  [SerializeField]
  private Unit004JobChangeMenu.CursorType[] cursorTypes_ = new Unit004JobChangeMenu.CursorType[DefValues.NUM_CHANGETYPE];
  [Header("ジョブチェンジ選択カーソル")]
  [SerializeField]
  private Unit004JobChangeCursor[] cursors_ = new Unit004JobChangeCursor[DefValues.NUM_CHANGETYPE];
  [SerializeField]
  [Tooltip("2ライン表示～3ライン表示をセット")]
  private GameObject[] cursorLines_ = new GameObject[3];
  [Header("3Dモデル")]
  [SerializeField]
  private Transform top3DModel_;
  [SerializeField]
  private UI3DModel ui3DModel_;
  private GameObject backup3DModel_;
  [SerializeField]
  private GameObject objLoading3DModel_;
  [Header("Bottom")]
  [SerializeField]
  private Unit004JobChangeMaterialContainer materialContainer_;
  [SerializeField]
  private UILabel txtOwnedZeny_;
  [SerializeField]
  private UILabel txtCostZeny_;
  [SerializeField]
  private ButtonWrapper btnExecute_;
  [SerializeField]
  private GameObject maskReleased_;
  [SerializeField]
  private UILabel MaskReleasedTextAttention;
  [SerializeField]
  private UIButton MaskReleasedTextAttentionButton;
  [SerializeField]
  private Transform lnkDetail_;
  private bool isInitialize_ = true;
  private SceneParam param_;
  private long? wRevision_;
  private PlayerUnit entityUnit_;
  private int entityCursorIndex_;
  private PlayerUnitJob_abilities currentJobAbility_;
  private int firstIndex_;
  private int focusCursorIndex_;
  private PlayerUnit[] changeUnits_;
  private bool isExistChangePattern_;
  private Dictionary<string, Sprite> dicJobSelector_ = new Dictionary<string, Sprite>(DefValues.NUM_CHANGETYPE);
  private Dictionary<int, PlayerMaterialUnit[]> dicMaterials_;
  private Dictionary<int, int> dicCost_;
  private Dictionary<int, bool> dicCompletedMaterials_;
  private Dictionary<int, bool> dicUnlocked_;
  private Dictionary<int, bool> dictJobChangePatternsConditionsLocked = new Dictionary<int, bool>();
  private DetailMenuPrefab[] details_;
  private WebAPI.Response.UnitPreviewJob previewJob_;
  private Unit004JobChangeMaterialContainer[] materialSwapBuff_ = new Unit004JobChangeMaterialContainer[2];
  private int materialSwapIndex_;
  private Vector3? localTop3DModel_;
  private const int CENTER_FIRST_DETAIL = 2;
  private bool isLoading3DModel_;

  public bool isNeedReset
  {
    get
    {
      bool isNeedReset = !this.wRevision_.HasValue;
      long num = SMManager.Revision<PlayerUnit[]>();
      if (!isNeedReset)
        isNeedReset = this.wRevision_.Value != num;
      return isNeedReset;
    }
  }

  public GameObject detailPrefab { get; private set; }

  public GameObject gearKindIconPrefab { get; private set; }

  public GameObject gearIconPrefab { get; private set; }

  public GameObject skillDetailDialogPrefab { get; private set; }

  public GameObject specialPointDetailDialogPrefab { get; private set; }

  public GameObject profIconPrefab { get; private set; }

  public GameObject skillTypeIconPrefab { get; private set; }

  public GameObject skillfullnessIconPrefab { get; private set; }

  public GameObject commonElementIconPrefab { get; private set; }

  public GameObject spAtkTypeIconPrefab { get; private set; }

  public GameObject statusDetailPrefab { get; private set; }

  public GameObject skillListPrefab { get; private set; }

  public GameObject StatusDetailPrefab { get; private set; }

  public GameObject TrainingPrefa { get; private set; }

  public GameObject GroupDetailDialogPrefab { get; private set; }

  public GameObject detailJobAbilityPrefab { get; private set; }

  public GameObject terraiAbilityDialogPrefab { get; private set; }

  public GameObject unityDetailPrefab { get; private set; }

  public GameObject stageItemPrefab { get; private set; }

  public GameObject skillLockIconPrefab { get; private set; }

  private GameObject jobChangeConditionsPrefab { get; set; }

  private void Awake()
  {
    if (Object.op_Inequality((Object) this.top3DModel_, (Object) null))
      this.localTop3DModel_ = new Vector3?(new Vector3(this.top3DModel_.localPosition.x, this.top3DModel_.localPosition.y, this.top3DModel_.localPosition.z));
    this.backup3DModel_ = ((Component) this.ui3DModel_).gameObject;
    this.backup3DModel_.SetActive(false);
    this.ui3DModel_ = (UI3DModel) null;
  }

  public IEnumerator initializeAsync(SceneParam param)
  {
    Unit004JobChangeMenu unit004JobChangeMenu = this;
    unit004JobChangeMenu.isInitialize_ = true;
    unit004JobChangeMenu.previewJob_ = (WebAPI.Response.UnitPreviewJob) null;
    unit004JobChangeMenu.param_ = param;
    unit004JobChangeMenu.wRevision_ = new long?(SMManager.Revision<PlayerUnit[]>());
    PlayerUnit playerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (u => u.id == param.playerUnitId_));
    if (playerUnit == (PlayerUnit) null)
    {
      unit004JobChangeMenu.entityUnit_ = (PlayerUnit) null;
      unit004JobChangeMenu.isInitialize_ = false;
    }
    else
    {
      if (unit004JobChangeMenu.entityUnit_ != (PlayerUnit) null && unit004JobChangeMenu.entityUnit_.unit.resource_reference_unit_id_UnitUnit != playerUnit.unit.resource_reference_unit_id_UnitUnit)
        unit004JobChangeMenu.dicJobSelector_.Clear();
      unit004JobChangeMenu.entityUnit_ = playerUnit;
      unit004JobChangeMenu.currentJobAbility_ = (PlayerUnitJob_abilities) null;
      unit004JobChangeMenu.changeUnits_ = JobChangeUtil.createJobChangePattern(unit004JobChangeMenu.entityUnit_, out unit004JobChangeMenu.isExistChangePattern_);
      unit004JobChangeMenu.dicMaterials_ = new Dictionary<int, PlayerMaterialUnit[]>(unit004JobChangeMenu.changeUnits_.Length);
      unit004JobChangeMenu.dicCost_ = new Dictionary<int, int>(unit004JobChangeMenu.changeUnits_.Length);
      unit004JobChangeMenu.dicCompletedMaterials_ = new Dictionary<int, bool>(unit004JobChangeMenu.changeUnits_.Length);
      unit004JobChangeMenu.dicUnlocked_ = new Dictionary<int, bool>(unit004JobChangeMenu.changeUnits_.Length);
      PlayerMaterialUnit[] playerMaterials = SMManager.Get<PlayerMaterialUnit[]>();
      if (unit004JobChangeMenu.isExistChangePattern_)
      {
        Future<WebAPI.Response.UnitPreviewJob> dpd = WebAPI.UnitPreviewJob(unit004JobChangeMenu.entityUnit_.id, (Action<WebAPI.Response.UserError>) (error =>
        {
          WebAPI.DefaultUserErrorCallback(error);
          MypageScene.ChangeSceneOnError();
        }));
        yield return (object) dpd.Wait();
        unit004JobChangeMenu.previewJob_ = dpd.Result;
        dpd = (Future<WebAPI.Response.UnitPreviewJob>) null;
        JobChangePatterns jobChangePatterns = JobChangeUtil.getJobChangePatterns(unit004JobChangeMenu.entityUnit_);
        unit004JobChangeMenu.resetMaterialsParam(jobChangePatterns.job1_UnitJob, jobChangePatterns.materials1, playerMaterials);
        unit004JobChangeMenu.resetMaterialsParam(jobChangePatterns.job2_UnitJob, jobChangePatterns.materials2, playerMaterials);
        if (jobChangePatterns.job3_UnitJob.HasValue)
          unit004JobChangeMenu.resetMaterialsParam(jobChangePatterns.job3_UnitJob.Value, jobChangePatterns.materials3, playerMaterials);
        if (jobChangePatterns.job4_UnitJob.HasValue)
          unit004JobChangeMenu.resetMaterialsParam(jobChangePatterns.job4_UnitJob.Value, jobChangePatterns.materials4, playerMaterials);
        if (jobChangePatterns.job5_UnitJob.HasValue)
          unit004JobChangeMenu.resetMaterialsParam(jobChangePatterns.job5_UnitJob.Value, jobChangePatterns.materials5, playerMaterials);
        if (jobChangePatterns.job6_UnitJob.HasValue)
          unit004JobChangeMenu.resetMaterialsParam(jobChangePatterns.job6_UnitJob.Value, jobChangePatterns.materials6, playerMaterials);
        if (jobChangePatterns.job7_UnitJob.HasValue)
          unit004JobChangeMenu.resetMaterialsParam(jobChangePatterns.job7_UnitJob.Value, jobChangePatterns.materials7, playerMaterials);
        int[] patternsConditions = JobChangeUtil.GetJobIdChangePatternsConditions(unit004JobChangeMenu.entityUnit_, unit004JobChangeMenu.previewJob_);
        foreach (KeyValuePair<int, bool> keyValuePair in unit004JobChangeMenu.dicUnlocked_)
          unit004JobChangeMenu.dictJobChangePatternsConditionsLocked[keyValuePair.Key] = !keyValuePair.Value && ((IEnumerable<int>) patternsConditions).Contains<int>(keyValuePair.Key);
        yield return (object) OnDemandDownload.WaitLoadUnitResource(unit004JobChangeMenu.dicMaterials_.SelectMany<KeyValuePair<int, PlayerMaterialUnit[]>, UnitUnit>((Func<KeyValuePair<int, PlayerMaterialUnit[]>, IEnumerable<UnitUnit>>) (d => ((IEnumerable<PlayerMaterialUnit>) d.Value).Select<PlayerMaterialUnit, UnitUnit>((Func<PlayerMaterialUnit, UnitUnit>) (x => x.unit)))).Distinct<UnitUnit>(), false);
        if (unit004JobChangeMenu.entityUnit_.job_abilities != null && unit004JobChangeMenu.entityUnit_.job_abilities.Length != 0)
          yield return (object) OnDemandDownload.WaitLoadUnitResource(((IEnumerable<PlayerUnitJob_abilities>) unit004JobChangeMenu.entityUnit_.job_abilities).SelectMany<PlayerUnitJob_abilities, UnitUnit>((Func<PlayerUnitJob_abilities, IEnumerable<UnitUnit>>) (x =>
          {
            List<UnitUnit> unitUnitList = new List<UnitUnit>();
            if (x.master != null)
            {
              for (int level = x.level; level < x.master.levelup_patterns.Length; ++level)
              {
                List<KeyValuePair<UnitUnit, int>> levelupMaterials = x.getLevelupMaterials(this.entityUnit_, level);
                if (levelupMaterials != null)
                  unitUnitList.AddRange(levelupMaterials.Select<KeyValuePair<UnitUnit, int>, UnitUnit>((Func<KeyValuePair<UnitUnit, int>, UnitUnit>) (y => y.Key)));
              }
            }
            return (IEnumerable<UnitUnit>) unitUnitList;
          })).Distinct<UnitUnit>(), false);
      }
      Player player = SMManager.Get<Player>();
      unit004JobChangeMenu.txtOwnedZeny_.SetTextLocalize(player.money);
      PartsContainerJoint component = ((Component) unit004JobChangeMenu).GetComponent<PartsContainerJoint>();
      if (Object.op_Inequality((Object) component, (Object) null) && ((Behaviour) component).enabled)
        yield return (object) component.initializeAsync();
      unit004JobChangeMenu.entityCursorIndex_ = Array.FindIndex<PlayerUnit>(unit004JobChangeMenu.changeUnits_, (Predicate<PlayerUnit>) (u => u != (PlayerUnit) null && u.job_id == this.entityUnit_.job_id));
      if (unit004JobChangeMenu.entityCursorIndex_ == -1)
      {
        Debug.LogError((object) ("Entity JobID " + (object) unit004JobChangeMenu.entityUnit_.job_id + " is Not Found"));
        Singleton<NGSceneManager>.GetInstance().ChangeErrorPage();
      }
      else
      {
        unit004JobChangeMenu.firstIndex_ = unit004JobChangeMenu.focusCursorIndex_ = ((IEnumerable<int?>) unit004JobChangeMenu.entityUnit_.changed_job_ids).Any<int?>((Func<int?, bool>) (n =>
        {
          if (!n.HasValue)
            return false;
          int? nullable = n;
          return !(0 == nullable.GetValueOrDefault() & nullable.HasValue);
        })) ? unit004JobChangeMenu.entityCursorIndex_ : 1;
        if (unit004JobChangeMenu.previewJob_ != null)
        {
          for (int index = 0; index < unit004JobChangeMenu.changeUnits_.Length; ++index)
          {
            if (index != unit004JobChangeMenu.entityCursorIndex_ && !(unit004JobChangeMenu.changeUnits_[index] == (PlayerUnit) null) && unit004JobChangeMenu.changeUnits_[index].job_abilities != null)
            {
              foreach (PlayerUnitJob_abilities jobAbility in unit004JobChangeMenu.changeUnits_[index].job_abilities)
              {
                PlayerUnitJob_abilities job_ability = jobAbility;
                WebAPI.Response.UnitPreviewJobJob_abilities previewJobJobAbilities = Array.Find<WebAPI.Response.UnitPreviewJobJob_abilities>(unit004JobChangeMenu.previewJob_.job_abilities, (Predicate<WebAPI.Response.UnitPreviewJobJob_abilities>) (j => j.job_ability_id == job_ability.job_ability_id));
                if (previewJobJobAbilities != null)
                  job_ability.level = previewJobJobAbilities.level;
              }
            }
          }
        }
        yield return (object) unit004JobChangeMenu.initCursors();
        yield return (object) unit004JobChangeMenu.initMaterials();
        yield return (object) unit004JobChangeMenu.initDetails();
        yield return (object) unit004JobChangeMenu.init3DModels();
        unit004JobChangeMenu.isInitialize_ = false;
      }
    }
  }

  private void resetMaterialsParam(
    int jobId,
    JobChangeMaterials materials,
    PlayerMaterialUnit[] playerMaterials)
  {
    PlayerMaterialUnit[] materials1 = materials != null ? JobChangeUtil.createPlayerMaterialUnits(materials) : new PlayerMaterialUnit[0];
    this.dicMaterials_[jobId] = materials1;
    this.dicCost_[jobId] = materials != null ? materials.cost : -1;
    this.dicCompletedMaterials_[jobId] = JobChangeUtil.checkCompletedMaterials(playerMaterials, materials1);
    this.dicUnlocked_[jobId] = this.entityUnit_.unit.job_UnitJob == jobId || this.entityUnit_.job_id == jobId || this.previewJob_ != null && ((IEnumerable<int>) this.previewJob_.changed_job_ids).Contains<int>(jobId);
  }

  public IEnumerator repairAsync(bool resetSelect)
  {
    this.reset3DModelAmbientLightColor();
    if (resetSelect)
      yield return (object) this.doChangeFocusJob(this.entityCursorIndex_, true);
    else
      yield return (object) this.doLoad3DModel(this.changeUnits_[this.focusCursorIndex_]);
  }

  private void reset3DModelAmbientLightColor()
  {
    RenderSettings.ambientLight = Consts.GetInstance().UI3DMODEL_AMBIENT_COLOR;
  }

  private IEnumerator initCursors()
  {
    Player player = SMManager.Get<Player>();
    for (int i = 0; i < this.changeUnits_.Length; ++i)
    {
      bool isCurrent = i == this.firstIndex_;
      bool isDefaultJob = i == 0;
      if (this.setupCursor(i))
      {
        Unit004JobChangeCursor cursor = this.cursors_[i];
        PlayerUnit p = this.changeUnits_[i];
        if (!(p == (PlayerUnit) null))
        {
          UnitUnit unit = p.unit;
          MasterDataTable.UnitJob uj = p.getJobData();
          bool isJobX = i > 3;
          bool isShort = cursor.isShort;
          yield return (object) this.loadJobFrameSprite(unit, p.job_id, isCurrent, isDefaultJob, isShort, isJobX);
          Sprite jobFrameSprite = this.getJobFrameSprite(p.job_id, isCurrent, isDefaultJob, isShort, isJobX);
          ((Component) cursor).gameObject.SetActive(true);
          cursor.Initialize(uj, isCurrent, jobFrameSprite, this.createMethodSelectedJob(i));
          if (this.isExistChangePattern_)
          {
            bool unlock = this.dicUnlocked_[p.job_id];
            bool activeChange = i != this.entityCursorIndex_ && (unlock || this.dicCompletedMaterials_[p.job_id] && player.CheckZeny(this.dicCost_[p.job_id]));
            bool conditionsLocked = this.dictJobChangePatternsConditionsLocked[p.job_id];
            cursor.SetDecoration(unlock, activeChange, conditionsLocked);
          }
          cursor = (Unit004JobChangeCursor) null;
          p = (PlayerUnit) null;
          uj = (MasterDataTable.UnitJob) null;
        }
      }
    }
    ((IEnumerable<GameObject>) this.cursorLines_).ToggleOnceEx(this.changeUnits_[3] != (PlayerUnit) null ? 2 : (this.changeUnits_[2] != (PlayerUnit) null ? 1 : 0));
  }

  private EventDelegate.Callback createMethodSelectedJob(int index)
  {
    return (EventDelegate.Callback) (() => this.onSelectedJob(index));
  }

  private bool setupCursor(int index)
  {
    Unit004JobChangeMenu.CursorType cursorType = this.cursorTypes_[index];
    ((Component) cursorType.Long)?.gameObject.SetActive(false);
    ((Component) cursorType.Short)?.gameObject.SetActive(false);
    if (this.changeUnits_[index] == (PlayerUnit) null)
      return false;
    if (index > 3)
    {
      this.cursors_[index] = !this.dicUnlocked_[this.changeUnits_[index].job_id] ? cursorType.Short : cursorType.Long;
      return true;
    }
    if (index > 0)
    {
      PlayerUnit changeUnit1 = this.changeUnits_[index + 3];
      PlayerUnit changeUnit2 = this.changeUnits_[index];
      if (changeUnit1 != (PlayerUnit) null)
      {
        if (this.dicUnlocked_[changeUnit1.job_id])
          return false;
        this.cursors_[index] = cursorType.Short;
      }
      else
        this.cursors_[index] = cursorType.Long;
      return true;
    }
    this.cursors_[index] = cursorType.Long;
    return true;
  }

  private IEnumerator loadJobFrameSprite(
    UnitUnit unit,
    int jobid,
    bool bCurrent,
    bool isDefault,
    bool isShort,
    bool isJobX)
  {
    string key = this.createKeyJobFrameSprite(jobid, bCurrent, isDefault, isShort, isJobX);
    if (!this.dicJobSelector_.ContainsKey(key))
    {
      Future<Sprite> ldFrame = this.LoadSpriteJobFrame(bCurrent, isDefault, isShort, isJobX);
      yield return (object) ldFrame.Wait();
      this.dicJobSelector_[key] = ldFrame.Result;
    }
  }

  private Sprite getJobFrameSprite(
    int jobid,
    bool bCurrent,
    bool isDefault,
    bool isShort,
    bool isJobX)
  {
    return this.dicJobSelector_[this.createKeyJobFrameSprite(jobid, bCurrent, isDefault, isShort, isJobX)];
  }

  private string createKeyJobFrameSprite(
    int jobid,
    bool bCurrent,
    bool isDefault,
    bool isShort,
    bool isJobX)
  {
    return "cmn_" + (bCurrent ? "1" : "0") + "_" + (isDefault ? "1" : "0") + "_" + (isShort ? "1" : "0") + "_" + (isJobX ? "1" : "0");
  }

  public Future<Sprite> LoadSpriteJobFrame(
    bool isCurrent,
    bool isDefault,
    bool isShort,
    bool isJobX)
  {
    string path = "AssetBundle/Resources/Common/Unit/" + string.Format("ibtn_{0}jobFrame_{1}{2}{3}", isDefault ? (object) "Def_" : (object) "", isJobX ? (object) "x_" : (object) "", !isShort || isDefault ? (object) "" : (object) "short_", isCurrent ? (object) "Current" : (object) "Idle");
    return Singleton<ResourceManager>.GetInstance().Load<Sprite>(path);
  }

  private void setEventSelectedJob(UIButton button, int index)
  {
    EventDelegate.Set(button.onClick, (EventDelegate.Callback) (() => this.onSelectedJob(index)));
  }

  private void onSelectedJob(int index)
  {
    if (this.isCustomPushAndSet())
      return;
    this.changeFocusJob(index);
  }

  private void changeFocusJob(int index, bool isForce = false)
  {
    if (!isForce && this.focusCursorIndex_ == index)
      this.IsPush = false;
    else
      this.StartCoroutine(this.doChangeFocusJob(index));
  }

  private IEnumerator doChangeFocusJob(int index, bool bReset = false)
  {
    Unit004JobChangeMenu unit004JobChangeMenu = this;
    unit004JobChangeMenu.cursors_[unit004JobChangeMenu.focusCursorIndex_].SetFocus(false);
    unit004JobChangeMenu.cursors_[index].SetFocus(true);
    DetailMenuPrefab nowDetail = unit004JobChangeMenu.details_[unit004JobChangeMenu.focusCursorIndex_];
    DetailMenuPrefab nextDetail = unit004JobChangeMenu.details_[index];
    int pos = bReset ? 2 : nowDetail.getCenterItemPositionDiffMode();
    ((UIRect) ((Component) nextDetail.normal.InformationScrollView).GetComponent<UIWidget>()).alpha = 0.0f;
    nextDetail.setActiveDiffMode(true);
    nextDetail.resetItemPositionDiffMode(pos);
    yield return (object) null;
    if (unit004JobChangeMenu.focusCursorIndex_ != index)
      nowDetail.setActiveDiffMode(false);
    ((UIRect) ((Component) nextDetail.normal.InformationScrollView).GetComponent<UIWidget>()).alpha = 1f;
    PlayerUnit nowUnit = unit004JobChangeMenu.changeUnits_[unit004JobChangeMenu.focusCursorIndex_];
    PlayerUnit nextUnit = unit004JobChangeMenu.changeUnits_[index];
    unit004JobChangeMenu.focusCursorIndex_ = index;
    if (unit004JobChangeMenu.dictJobChangePatternsConditionsLocked[nextUnit.job_id])
      PopupJobChangeConditions.show(unit004JobChangeMenu.jobChangeConditionsPrefab, unit004JobChangeMenu.entityUnit_, index);
    yield return (object) unit004JobChangeMenu.doUpdateMaterials();
    if (bReset || JobChangeUtil.checkDiff3DModel(nowUnit, nextUnit))
      yield return (object) unit004JobChangeMenu.doLoad3DModel(unit004JobChangeMenu.changeUnits_[index]);
    if (!bReset)
      unit004JobChangeMenu.IsPush = false;
  }

  private IEnumerator initMaterials()
  {
    Unit004JobChangeMenu menu = this;
    if (Object.op_Equality((Object) menu.materialSwapBuff_[0], (Object) null))
    {
      Future<GameObject> ldIcon = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      yield return (object) ldIcon.Wait();
      menu.materialSwapBuff_[0] = menu.materialContainer_;
      menu.materialSwapBuff_[1] = ((Component) menu.materialContainer_).gameObject.Clone(((Component) menu.materialContainer_).transform.parent).GetComponent<Unit004JobChangeMaterialContainer>();
      menu.materialSwapBuff_[0].initialize(menu, ldIcon.Result);
      menu.materialSwapBuff_[1].initialize(menu, ldIcon.Result);
      menu.materialSwapIndex_ = 0;
      foreach (Unit004JobChangeMaterialContainer materialContainer in menu.materialSwapBuff_)
        materialContainer.isEnabled = true;
      yield return (object) null;
      foreach (Unit004JobChangeMaterialContainer materialContainer in menu.materialSwapBuff_)
        materialContainer.isEnabled = false;
      ldIcon = (Future<GameObject>) null;
    }
    yield return (object) menu.doUpdateMaterials();
  }

  private IEnumerator doUpdateMaterials()
  {
    Unit004JobChangeMenu unit004JobChangeMenu = this;
    if (unit004JobChangeMenu.isExistChangePattern_)
    {
      Unit004JobChangeMaterialContainer nowContainer = unit004JobChangeMenu.materialSwapBuff_[unit004JobChangeMenu.materialSwapIndex_];
      unit004JobChangeMenu.materialSwapIndex_ = unit004JobChangeMenu.materialSwapIndex_ + 1 & 1;
      Unit004JobChangeMaterialContainer nextContainer = unit004JobChangeMenu.materialSwapBuff_[unit004JobChangeMenu.materialSwapIndex_];
      Player player = SMManager.Get<Player>();
      int jobId = unit004JobChangeMenu.changeUnits_[unit004JobChangeMenu.focusCursorIndex_].job_id;
      PlayerMaterialUnit[] dicMaterial = unit004JobChangeMenu.dicMaterials_[jobId];
      nextContainer.setAlpha(0.0f);
      nextContainer.isEnabled = true;
      bool bUnlocked = unit004JobChangeMenu.dicUnlocked_[jobId];
      yield return (object) nextContainer.doUpdateMaterials(dicMaterial, unit004JobChangeMenu.focusCursorIndex_ == 0, bUnlocked, unit004JobChangeMenu.dictJobChangePatternsConditionsLocked[jobId]);
      int num = unit004JobChangeMenu.dicCost_[jobId];
      bool flag = player.CheckZeny(num);
      if (!bUnlocked && num >= 0)
      {
        unit004JobChangeMenu.txtCostZeny_.SetTextLocalize(num);
        ((UIWidget) unit004JobChangeMenu.txtCostZeny_).color = flag ? Color.white : Color.red;
        ((UIWidget) unit004JobChangeMenu.txtOwnedZeny_).color = Color.white;
      }
      else
      {
        unit004JobChangeMenu.txtCostZeny_.SetTextLocalize(Consts.GetInstance().JOBCHANGE_NULL_QUANTITY);
        ((UIWidget) unit004JobChangeMenu.txtCostZeny_).color = ((UIWidget) unit004JobChangeMenu.txtOwnedZeny_).color = Color.gray;
      }
      nowContainer.isEnabled = false;
      nextContainer.setAlpha(1f);
      unit004JobChangeMenu.btnExecute_.isEnabled = unit004JobChangeMenu.focusCursorIndex_ != unit004JobChangeMenu.entityCursorIndex_ && (bUnlocked || unit004JobChangeMenu.dicCompletedMaterials_[jobId] & flag) && !unit004JobChangeMenu.dictJobChangePatternsConditionsLocked[jobId];
      if (Object.op_Inequality((Object) unit004JobChangeMenu.maskReleased_, (Object) null))
      {
        unit004JobChangeMenu.maskReleased_.SetActive(((unit004JobChangeMenu.focusCursorIndex_ == 0 ? 0 : (unit004JobChangeMenu.focusCursorIndex_ != unit004JobChangeMenu.entityCursorIndex_ ? 1 : 0)) & (bUnlocked ? 1 : 0)) != 0 || unit004JobChangeMenu.dictJobChangePatternsConditionsLocked[jobId]);
        if (unit004JobChangeMenu.maskReleased_.activeSelf)
        {
          if (unit004JobChangeMenu.dictJobChangePatternsConditionsLocked[jobId])
          {
            ((Component) unit004JobChangeMenu.MaskReleasedTextAttentionButton).gameObject.SetActive(true);
            // ISSUE: reference to a compiler-generated method
            EventDelegate.Set(unit004JobChangeMenu.MaskReleasedTextAttentionButton.onClick, new EventDelegate.Callback(unit004JobChangeMenu.\u003CdoUpdateMaterials\u003Eb__140_0));
            unit004JobChangeMenu.MaskReleasedTextAttention.text = Consts.GetInstance().JOB_CHANGE_NOT_ENOUGH_LV;
          }
          else
          {
            ((Component) unit004JobChangeMenu.MaskReleasedTextAttentionButton).gameObject.SetActive(false);
            unit004JobChangeMenu.MaskReleasedTextAttention.text = Consts.GetInstance().JOB_CHANGE_NEED_NOT_MATERIAL_MONEY;
          }
        }
      }
    }
  }

  private IEnumerator initDetails()
  {
    Unit004JobChangeMenu menuContainer = this;
    if (!Object.op_Equality((Object) menuContainer.lnkDetail_, (Object) null))
    {
      // ISSUE: explicit non-virtual call
      if (Object.op_Equality((Object) __nonvirtual (menuContainer.detailPrefab), (Object) null))
        yield return (object) menuContainer.loadDetailPrefabs();
      if (menuContainer.details_ != null)
      {
        foreach (DetailMenuPrefab detail in menuContainer.details_)
        {
          if (Object.op_Inequality((Object) detail, (Object) null))
            Object.Destroy((Object) ((Component) detail).gameObject);
        }
      }
      menuContainer.details_ = new DetailMenuPrefab[menuContainer.changeUnits_.Length];
      for (int n = 0; n < menuContainer.changeUnits_.Length; ++n)
      {
        // ISSUE: explicit non-virtual call
        menuContainer.details_[n] = __nonvirtual (menuContainer.detailPrefab).Clone(menuContainer.lnkDetail_).GetComponent<DetailMenuPrefab>();
        if (n != menuContainer.entityCursorIndex_ && menuContainer.changeUnits_[n] == (PlayerUnit) null)
        {
          ((Component) menuContainer.details_[n]).gameObject.SetActive(false);
        }
        else
        {
          ((Component) menuContainer.details_[n]).gameObject.SetActive(true);
          IEnumerator e;
          if (n == menuContainer.entityCursorIndex_)
          {
            menuContainer.details_[n].normal.updatedJobAbility = new Action(menuContainer.updatedJobAbility);
            e = menuContainer.details_[n].initAsyncDiffMode(menuContainer.entityUnit_, (PlayerUnit) null, 2, (IDetailMenuContainer) menuContainer);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
          {
            e = menuContainer.details_[n].initAsyncDiffMode(menuContainer.changeUnits_[n], menuContainer.entityUnit_, 2, (IDetailMenuContainer) menuContainer);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          ((Component) menuContainer.details_[n]).gameObject.SetActive(n == menuContainer.firstIndex_);
        }
      }
    }
  }

  private void updatedJobAbility()
  {
    this.StopCoroutine("doResetScene");
    this.StartCoroutine("doResetScene");
  }

  private IEnumerator doResetScene()
  {
    while (Singleton<PopupManager>.GetInstance().isOpen || Singleton<PopupManager>.GetInstance().isRunningCoroutine)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) this.initializeAsync(this.param_);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  private IEnumerator loadDetailPrefabs()
  {
    Future<GameObject> loader = new ResourceObject("Prefabs/unit004_2/detail").Load<GameObject>();
    yield return (object) loader.Wait();
    this.detailPrefab = loader.Result;
    loader = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    yield return (object) loader.Wait();
    this.gearIconPrefab = loader.Result;
    loader = Res.Icons.GearKindIcon.Load<GameObject>();
    yield return (object) loader.Wait();
    this.gearKindIconPrefab = loader.Result;
    loader = PopupSkillDetails.createPrefabLoader(false);
    yield return (object) loader.Wait();
    this.skillDetailDialogPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/unit004_2/SpecialPoint_DetailDialog").Load<GameObject>();
    yield return (object) loader.Wait();
    this.specialPointDetailDialogPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/unit004_2/TerraiAbilityDialog").Load<GameObject>();
    yield return (object) loader.Wait();
    this.terraiAbilityDialogPrefab = loader.Result;
    loader = Res.Icons.GearProfiencyIcon.Load<GameObject>();
    yield return (object) loader.Wait();
    this.profIconPrefab = loader.Result;
    loader = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    yield return (object) loader.Wait();
    this.skillTypeIconPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/SkillFamily/SkillFamilyIcon").Load<GameObject>();
    yield return (object) loader.Wait();
    this.skillfullnessIconPrefab = loader.Result;
    loader = Res.Icons.CommonElementIcon.Load<GameObject>();
    yield return (object) loader.Wait();
    this.commonElementIconPrefab = loader.Result;
    loader = Res.Icons.SPAtkTypeIcon.Load<GameObject>();
    yield return (object) loader.Wait();
    this.spAtkTypeIconPrefab = loader.Result;
    loader = Res.Prefabs.unit.dir_unit_status_detail.Load<GameObject>();
    yield return (object) loader.Wait();
    this.statusDetailPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/battle017_11_1_1/popup_SkillList").Load<GameObject>();
    yield return (object) loader.Wait();
    this.skillListPrefab = loader.Result;
    loader = Res.Prefabs.unit004_Job.Unit_JobCharacteristic_Dialog.Load<GameObject>();
    yield return (object) loader.Wait();
    this.detailJobAbilityPrefab = loader.Result;
    loader = PopupJobChangeConditions.createLoader();
    yield return (object) loader.Wait();
    this.jobChangeConditionsPrefab = loader.Result;
    Future<GameObject>[] loaders = PopupUnityValueDetail.createLoaders(false);
    yield return (object) loaders[0].Wait();
    this.unityDetailPrefab = loaders[0].Result;
    yield return (object) loaders[1].Wait();
    this.stageItemPrefab = loaders[1].Result;
    loaders = (Future<GameObject>[]) null;
    loader = new ResourceObject("Prefabs/BattleSkillIcon/dir_SkillLock").Load<GameObject>();
    yield return (object) loader.Wait();
    this.skillLockIconPrefab = loader.Result;
  }

  private IEnumerator init3DModels()
  {
    this.reset3DModelAmbientLightColor();
    yield return (object) this.doLoad3DModel(this.changeUnits_[this.focusCursorIndex_]);
  }

  private IEnumerator doLoad3DModel(PlayerUnit playerUnit)
  {
    this.isLoading3DModel_ = true;
    if (Object.op_Inequality((Object) this.objLoading3DModel_, (Object) null))
      this.objLoading3DModel_.SetActive(true);
    yield return (object) this.doModelDestroy();
    this.ui3DModel_ = this.backup3DModel_.Clone(this.backup3DModel_.transform.parent).GetComponent<UI3DModel>();
    ((Component) this.ui3DModel_).gameObject.SetActive(true);
    if (Object.op_Inequality((Object) this.ui3DModel_.model_creater_, (Object) null))
      this.ui3DModel_.model_creater_.BaseModel = (GameObject) null;
    if (this.localTop3DModel_.HasValue)
    {
      Vector3 vector3 = this.localTop3DModel_.Value;
      UnitRenderingPattern renderingPattern = playerUnit.getJobData().rendering_pattern;
      if (renderingPattern != null)
      {
        vector3.x += renderingPattern.texture_x;
        vector3.y += renderingPattern.texture_y;
      }
      this.top3DModel_.localPosition = vector3;
      ((Component) this.top3DModel_).gameObject.SetActive(false);
      ((Component) this.top3DModel_).gameObject.SetActive(true);
    }
    if (playerUnit != (PlayerUnit) null)
      yield return (object) this.ui3DModel_.JobChange(playerUnit);
    if (Object.op_Inequality((Object) this.objLoading3DModel_, (Object) null))
      this.objLoading3DModel_.SetActive(false);
    this.isLoading3DModel_ = false;
  }

  private IEnumerator doModelDestroy()
  {
    if (Object.op_Inequality((Object) this.ui3DModel_, (Object) null))
      this.ui3DModel_.Remove();
    if (ModelUnits.Instance.ModelList != null)
    {
      for (int i = 0; i < ModelUnits.Instance.ModelList.Count; ++i)
      {
        if (Object.op_Inequality((Object) ModelUnits.Instance.ModelList[i], (Object) null))
          yield return (object) this.doDestroyPoolableObject(ModelUnits.Instance.ModelList[i].gameObject);
      }
    }
    if (Object.op_Inequality((Object) this.ui3DModel_, (Object) null))
    {
      Object.Destroy((Object) ((Component) this.ui3DModel_).gameObject);
      this.ui3DModel_ = (UI3DModel) null;
    }
    yield return (object) null;
  }

  private IEnumerator doDestroyPoolableObject(GameObject go)
  {
    while (Object.op_Implicit((Object) go))
    {
      ObjectPoolController.Destroy(go);
      if (!go.activeSelf)
        break;
      yield return (object) null;
    }
  }

  private void OnDisable() => this.clear3DModel();

  private void clear3DModel()
  {
    this.isLoading3DModel_ = false;
    if (!Object.op_Inequality((Object) this.ui3DModel_, (Object) null))
      return;
    this.ui3DModel_.Remove();
    ModelUnits.Instance.DestroyModelUnits();
  }

  protected override void Update()
  {
    base.Update();
    int num = this.isInitialize_ ? 1 : 0;
  }

  public override void onBackButton() => this.onClickedBack();

  public bool isCustomPushAndSet()
  {
    return this.isInitialize_ || this.isBlockTouch || this.IsPushAndSet();
  }

  private bool isBlockTouch
  {
    get => this.isLoading3DModel_ || Singleton<CommonRoot>.GetInstance().isLoading;
  }

  public void onClickedBack()
  {
    if (this.isCustomPushAndSet())
      return;
    if (this.param_.onBackScene_ != null)
      this.param_.onBackScene_();
    else
      this.backScene();
  }

  public void onClickedExecute()
  {
    if (this.isCustomPushAndSet())
      return;
    this.StartCoroutine(this.doCheckJobChange());
  }

  private IEnumerator doCheckJobChange()
  {
    Unit004JobChangeMenu unit004JobChangeMenu = this;
    PlayerUnit afterUnit = unit004JobChangeMenu.changeUnits_[unit004JobChangeMenu.focusCursorIndex_];
    PlayerUnit playerUnit = (PlayerUnit) null;
    if (unit004JobChangeMenu.focusCursorIndex_ > 3)
      playerUnit = unit004JobChangeMenu.changeUnits_[unit004JobChangeMenu.focusCursorIndex_ - 3];
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false);
    Consts instance = Consts.GetInstance();
    Unit004JobChangeMenu.ConfirmPopup[] confirmPopupArray1 = new Unit004JobChangeMenu.ConfirmPopup[5];
    confirmPopupArray1[0] = new Unit004JobChangeMenu.ConfirmPopup()
    {
      checkOpen_ = (Func<bool>) (() => !this.dicUnlocked_[afterUnit.job_id] && this.dicMaterials_[afterUnit.job_id].Length != 0 && this.focusCursorIndex_ < 4),
      title_ = instance.JOB_CHANGE_TITLE,
      desc_ = instance.JOB_CHANGE_DESC
    };
    confirmPopupArray1[1] = new Unit004JobChangeMenu.ConfirmPopup()
    {
      checkOpen_ = (Func<bool>) (() => !this.dicUnlocked_[afterUnit.job_id] && this.dicMaterials_[afterUnit.job_id].Length != 0 && this.focusCursorIndex_ > 3),
      title_ = instance.JOB_CHANGE_X_TITLE,
      desc_ = string.Format(instance.JOB_CHANGE_X_DESC, (object) afterUnit.getJobData().name, (object) playerUnit?.getJobData().name)
    };
    confirmPopupArray1[2] = new Unit004JobChangeMenu.ConfirmPopup()
    {
      checkOpen_ = (Func<bool>) (() => MasterData.UnitJob[afterUnit.job_id].new_cost > 0),
      title_ = instance.JOB_CHANGE_TITLE,
      desc_ = instance.JOB_CHANGE_DESC_COST_CHANGE
    };
    Unit004JobChangeMenu.ConfirmPopup confirmPopup1 = new Unit004JobChangeMenu.ConfirmPopup();
    confirmPopup1.checkOpen_ = (Func<bool>) (() =>
    {
      bool flag = false;
      PlayerTransmigrateMemoryPlayerUnitIds current = PlayerTransmigrateMemoryPlayerUnitIds.Current;
      if (current?.transmigrate_memory_player_unit_ids != null && current.transmigrate_memory_player_unit_ids.Length != 0)
      {
        int cur = this.entityUnit_.id;
        flag = ((IEnumerable<int?>) current.transmigrate_memory_player_unit_ids).Any<int?>((Func<int?, bool>) (n => n.HasValue && n.Value == cur));
      }
      return flag;
    });
    confirmPopup1.title_ = instance.JOB_CHANGE_MEMORY_TITLE;
    confirmPopup1.desc_ = instance.JOB_CHANGE_MEMORY_DESC;
    confirmPopupArray1[3] = confirmPopup1;
    confirmPopup1 = new Unit004JobChangeMenu.ConfirmPopup();
    confirmPopup1.checkOpen_ = (Func<bool>) (() =>
    {
      if (this.entityUnit_.equippedGear != (PlayerItem) null && afterUnit.equippedGear == (PlayerItem) null || this.entityUnit_.equippedGear2 != (PlayerItem) null && afterUnit.equippedGear2 == (PlayerItem) null)
        return true;
      return this.entityUnit_.equippedGear3 != (PlayerItem) null && afterUnit.equippedGear3 == (PlayerItem) null;
    });
    confirmPopup1.title_ = instance.JOB_CHANGE_GEAR_TITLE;
    confirmPopup1.desc_ = instance.JOB_CHANGE_GEAR_DESC;
    confirmPopupArray1[4] = confirmPopup1;
    Unit004JobChangeMenu.ConfirmPopup[] confirmPopupArray = confirmPopupArray1;
    for (int index = 0; index < confirmPopupArray.Length; ++index)
    {
      Unit004JobChangeMenu.ConfirmPopup confirmPopup2 = confirmPopupArray[index];
      if (confirmPopup2.checkOpen_())
      {
        bool bWait = true;
        bool bCancel = false;
        PopupCommonNoYes.Show(confirmPopup2.title_, confirmPopup2.desc_, (Action) (() => bWait = false), (Action) (() =>
        {
          bCancel = true;
          bWait = false;
        }));
        while (bWait)
          yield return (object) null;
        if (!bCancel)
          ;
        else
        {
          unit004JobChangeMenu.IsPush = false;
          Singleton<PopupManager>.GetInstance().closeAll();
          yield break;
        }
      }
    }
    confirmPopupArray = (Unit004JobChangeMenu.ConfirmPopup[]) null;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.UnitJobchange> future = WebAPI.UnitJobchange(unit004JobChangeMenu.entityUnit_.id, unit004JobChangeMenu.dicUnlocked_[afterUnit.job_id] ? new int[0] : ((IEnumerable<PlayerMaterialUnit>) unit004JobChangeMenu.dicMaterials_[afterUnit.job_id]).Select<PlayerMaterialUnit, int>((Func<PlayerMaterialUnit, int>) (m => m.id)).ToArray<int>(), afterUnit.job_id, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    yield return (object) future.Wait();
    if (future.Result != null)
    {
      afterUnit = Array.Find<PlayerUnit>(future.Result.player_units, (Predicate<PlayerUnit>) (pu => pu.id == this.entityUnit_.id));
      unit004JobChangeMenu.jobChanged(unit004JobChangeMenu.entityUnit_, afterUnit, future.Result.is_new);
    }
  }

  private void jobChanged(PlayerUnit beforeUnit, PlayerUnit afterUnit, bool isNew)
  {
    this.wRevision_ = new long?();
    PrincesEvolutionParam princesEvolutionParam1 = new PrincesEvolutionParam();
    princesEvolutionParam1.mode = Unit00499Scene.Mode.JobChange;
    princesEvolutionParam1.baseUnit = beforeUnit;
    princesEvolutionParam1.resultUnit = afterUnit;
    princesEvolutionParam1.fromEarth = false;
    princesEvolutionParam1.is_new = isNew;
    int count = 1;
    PrincesEvolutionParam princesEvolutionParam2 = princesEvolutionParam1;
    List<PlayerUnit> playerUnitList;
    if (!this.dicUnlocked_[afterUnit.job_id])
    {
      PlayerMaterialUnit[] dicMaterial = this.dicMaterials_[afterUnit.job_id];
      playerUnitList = (dicMaterial != null ? ((IEnumerable<PlayerMaterialUnit>) dicMaterial).Select<PlayerMaterialUnit, PlayerUnit>((Func<PlayerMaterialUnit, PlayerUnit>) (mu => PlayerUnit.CreateByPlayerMaterialUnit(mu, count++))).ToList<PlayerUnit>() : (List<PlayerUnit>) null) ?? new List<PlayerUnit>();
    }
    else
      playerUnitList = new List<PlayerUnit>();
    princesEvolutionParam2.materiaqlUnits = playerUnitList;
    unit00497Scene.ChangeScene(true, princesEvolutionParam1);
  }

  [Serializable]
  private class CursorType
  {
    public Unit004JobChangeCursor Short;
    public Unit004JobChangeCursor Long;
  }

  private struct ConfirmPopup
  {
    public Func<bool> checkOpen_;
    public string title_;
    public string desc_;
  }
}
