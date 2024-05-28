// Decompiled with JetBrains decompiler
// Type: Unit004JobDialogUp
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
public class Unit004JobDialogUp : BackButtonMenuBase
{
  [SerializeField]
  private Transform dirJobAfter;
  [SerializeField]
  private UIGrid dynMaterial;
  [SerializeField]
  private UILabel[] txtMaterialNeedValue;
  [SerializeField]
  private UILabel[] txtMaterialPossessedValue;
  [SerializeField]
  private UILabel txtZenyNeed;
  [SerializeField]
  private UILabel txtZenyPossession;
  [SerializeField]
  private UILabel txtToutaNeed;
  [SerializeField]
  private UILabel txtToutaPossession;
  [SerializeField]
  private UILabel txtProficiencyNeed;
  [SerializeField]
  private UILabel txtProficiencyPossession;
  [SerializeField]
  private SpreadColorButton ibtnLevelUp;
  [SerializeField]
  private GameObject DialogBox;
  [SerializeField]
  private UILabel TxtDialogMaterialName;
  [SerializeField]
  private UILabel TxtDialogMaterialPlace;
  [SerializeField]
  private UILabel txtInneed;
  [SerializeField]
  private PopupSelectSliderController sliderController;
  [SerializeField]
  private UIScrollBar scrollBar;
  [SerializeField]
  private UIScrollView scrollView;
  private GameObject JobAfterPanel;
  private GameObject unitPrefab;
  private PlayerUnit _unit;
  private PlayerUnitJob_abilities _jobAbility;
  private bool activeLevelUpButton = true;
  private bool inneedMaterial;
  private bool isClassChangeScene;
  private Action<Action> onPreUpdateJobAbility_;
  private Action onUpdatedJobAbility_;
  private int countIconInitializing_;
  private int targetLevel;
  private readonly int LEVEL_LIMIT = 5;
  private readonly int MATERIAL_SLOT_NUM = 5;
  private Unit004JobAfter jobAfterPanel;
  private bool _isSea;
  private Color _textColor;
  private int requiredMoney;
  private int? requiredUnityValue;

  public IEnumerator Init(
    PlayerUnit unit,
    PlayerUnitJob_abilities jobAbility,
    Action eventUpdatedJobAbility,
    bool isClassChangeScene = false,
    Action<Action> eventPreUpdateJobAbility = null)
  {
    Unit004JobDialogUp unit004JobDialogUp = this;
    UIWidget component = ((Component) unit004JobDialogUp).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((UIRect) component).alpha = 0.0f;
    ((UIButtonColor) unit004JobDialogUp.ibtnLevelUp).isEnabled = false;
    unit004JobDialogUp.activeLevelUpButton = true;
    ((Component) unit004JobDialogUp.txtInneed).gameObject.SetActive(false);
    unit004JobDialogUp.DialogBox.SetActive(false);
    unit004JobDialogUp.onPreUpdateJobAbility_ = eventPreUpdateJobAbility;
    unit004JobDialogUp.onUpdatedJobAbility_ = eventUpdatedJobAbility;
    unit004JobDialogUp.requiredMoney = 0;
    unit004JobDialogUp.requiredUnityValue = new int?(0);
    unit004JobDialogUp._isSea = Singleton<NGGameDataManager>.GetInstance().IsSea;
    unit004JobDialogUp._textColor = !unit004JobDialogUp._isSea ? Color.white : new Color(0.305882365f, 0.305882365f, 0.305882365f);
    unit004JobDialogUp._unit = unit;
    unit004JobDialogUp._jobAbility = jobAbility;
    SMManager.Observe<Player>();
    IEnumerator e = unit004JobDialogUp.LoadPrefabs(isClassChangeScene);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = unit004JobDialogUp.JobAfterPanel.Clone(unit004JobDialogUp.dirJobAfter);
    unit004JobDialogUp.jobAfterPanel = gameObject.GetComponent<Unit004JobAfter>();
    e = unit004JobDialogUp.jobAfterPanel.Init(3, jobAbility, bActiveSkillZoom: true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Dictionary<UnitUnit, int> requiredMaterials = unit004JobDialogUp.getRequiredMaterials(unit004JobDialogUp.LEVEL_LIMIT);
    e = unit004JobDialogUp.loadMaterialIcons(requiredMaterials);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004JobDialogUp.dynMaterial.Reposition();
    unit004JobDialogUp.targetLevel = unit004JobDialogUp._jobAbility.level + 1;
    if (unit004JobDialogUp.targetLevel < unit004JobDialogUp.LEVEL_LIMIT)
    {
      // ISSUE: reference to a compiler-generated method
      unit004JobDialogUp.sliderController.Initialize((float) unit004JobDialogUp.targetLevel, (float) unit004JobDialogUp.LEVEL_LIMIT, (float) unit004JobDialogUp.targetLevel, new PopupSelectSliderController.SliderValueChangeListener(unit004JobDialogUp.\u003CInit\u003Eb__36_0));
    }
    else
    {
      unit004JobDialogUp.sliderController.Initialize((float) unit004JobDialogUp._jobAbility.level, (float) unit004JobDialogUp.LEVEL_LIMIT, (float) unit004JobDialogUp.targetLevel);
      unit004JobDialogUp.sliderController.LockSlider();
    }
    unit004JobDialogUp.onSliderValueChange(unit004JobDialogUp.targetLevel);
  }

  private IEnumerator loadMaterialIcons(Dictionary<UnitUnit, int> materials)
  {
    ++this.countIconInitializing_;
    foreach (KeyValuePair<UnitUnit, int> material in materials)
    {
      UnitUnit key = material.Key;
      yield return (object) this.doIconInitalize(this.unitPrefab.CloneAndGetComponent<UnitIconBase>(((Component) this.dynMaterial).transform), key, false);
    }
    --this.countIconInitializing_;
  }

  private void ControlAndAddItem(
    UnitIconBase icon,
    PlayerMaterialUnit[] playerUnitMaterial,
    UnitUnit materialRequested,
    int? quantityRequested,
    UILabel txtMaterialNeededLabel,
    UILabel txtMaterialPossessedLabel)
  {
    if (materialRequested != null)
    {
      PlayerMaterialUnit playerMaterialUnit = Array.Find<PlayerMaterialUnit>(playerUnitMaterial, (Predicate<PlayerMaterialUnit>) (x => x._unit == materialRequested.ID));
      if (playerMaterialUnit != null)
      {
        bool flag = false;
        int quantity = playerMaterialUnit.quantity;
        int? nullable = quantityRequested;
        int valueOrDefault = nullable.GetValueOrDefault();
        if (quantity < valueOrDefault & nullable.HasValue)
        {
          this.activeLevelUpButton = false;
          this.inneedMaterial = true;
          flag = true;
          ((UIWidget) txtMaterialNeededLabel).color = Color.red;
        }
        txtMaterialPossessedLabel.SetTextLocalize(playerMaterialUnit.quantity.ToString() + "体所持");
        icon.Gray = flag;
      }
      else
      {
        this.activeLevelUpButton = false;
        this.inneedMaterial = true;
        ((UIWidget) txtMaterialNeededLabel).color = Color.red;
        txtMaterialPossessedLabel.SetTextLocalize("0体所持");
        icon.Gray = true;
      }
      txtMaterialNeededLabel.SetTextLocalize(quantityRequested.ToString());
      ((Component) txtMaterialNeededLabel).gameObject.SetActive(true);
      ((Component) txtMaterialPossessedLabel).gameObject.SetActive(true);
    }
    else
    {
      ((Component) txtMaterialNeededLabel).gameObject.SetActive(false);
      ((Component) txtMaterialPossessedLabel).gameObject.SetActive(false);
    }
  }

  private void AddItemIcon(UnitIconBase item, UnitUnit sozai = null, bool isGray = false)
  {
    if (sozai != null)
    {
      Singleton<PopupManager>.GetInstance().monitorCoroutine(this.doIconInitalize(item, sozai, isGray));
    }
    else
    {
      UnitIcon unitIcon = (UnitIcon) item;
      ((Behaviour) unitIcon.Button).enabled = false;
      ((Collider) unitIcon.buttonBoxCollider).enabled = false;
      unitIcon.SetEmpty();
    }
  }

  private IEnumerator doIconInitalize(UnitIconBase icon, UnitUnit sozai, bool isGray)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004JobDialogUp unit004JobDialogUp = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      icon.Gray = isGray;
      // ISSUE: reference to a compiler-generated method
      icon.onClick = new Action<UnitIconBase>(unit004JobDialogUp.\u003CdoIconInitalize\u003Eb__40_0);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) icon.SetUnit(sozai, sozai.GetElement());
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator LoadPrefabs(bool isClassChangeScene)
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitPrefab = prefabF.Result;
    this.isClassChangeScene = isClassChangeScene;
    Future<GameObject> JobAfterPanelF = (Future<GameObject>) null;
    JobAfterPanelF = !Singleton<NGGameDataManager>.GetInstance().IsSea || isClassChangeScene ? Res.Prefabs.unit004_Job.Unit_job_after.Load<GameObject>() : Res.Prefabs.unit004_Job.Unit_job_after_sea.Load<GameObject>();
    e = JobAfterPanelF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.JobAfterPanel = JobAfterPanelF.Result;
  }

  private void ShowMaterialQuestInfo(UnitUnit material)
  {
    int num = !this.DialogBox.activeInHierarchy ? 1 : 0;
    this.DialogBox.SetActive(true);
    if (num != 0)
    {
      UITweener[] tweeners = NGTween.findTweeners(this.DialogBox, true);
      NGTween.playTweens(tweeners, NGTween.Kind.START_END);
      NGTween.playTweens(tweeners, NGTween.Kind.START);
      foreach (UITweener uiTweener in tweeners)
        uiTweener.onFinished.Clear();
    }
    this.TxtDialogMaterialName.SetText(material.name);
    UnitMaterialQuestInfo materialQuestInfo = ((IEnumerable<UnitMaterialQuestInfo>) MasterData.UnitMaterialQuestInfoList).SingleOrDefault<UnitMaterialQuestInfo>((Func<UnitMaterialQuestInfo, bool>) (x => x.unit_id == material.ID));
    if (materialQuestInfo == null)
      this.TxtDialogMaterialPlace.SetText("");
    else
      this.TxtDialogMaterialPlace.SetText(materialQuestInfo.long_desc);
  }

  public UITweener[] EndTweensMaterialQuestInfo(bool isForce = false)
  {
    if (!this.DialogBox.activeInHierarchy)
      return (UITweener[]) null;
    UITweener[] tweeners = NGTween.findTweeners(this.DialogBox, true);
    if (!isForce && ((IEnumerable<UITweener>) tweeners).Any<UITweener>((Func<UITweener, bool>) (x => ((Behaviour) x).enabled)))
      return (UITweener[]) null;
    NGTween.playTweens(tweeners, NGTween.Kind.START_END, true);
    NGTween.playTweens(tweeners, NGTween.Kind.END);
    return tweeners;
  }

  public void HideMaterialQuestInfo()
  {
    UITweener[] tweens = this.EndTweensMaterialQuestInfo();
    if (tweens == null)
      return;
    NGTween.setOnTweenFinished(tweens, (MonoBehaviour) this, "HideDialogBox");
  }

  private void HideDialogBox() => this.DialogBox.SetActive(false);

  public void IbtnLevelUp()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.openAnimationLevelUp());
  }

  private IEnumerator openAnimationLevelUp()
  {
    Unit004JobDialogUp unit004JobDialogUp = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    if (unit004JobDialogUp.onPreUpdateJobAbility_ != null)
    {
      bool bWait = true;
      unit004JobDialogUp.onPreUpdateJobAbility_((Action) (() => bWait = false));
      while (bWait)
        yield return (object) null;
    }
    Future<WebAPI.Response.UnitSaveJobAbility> paramF = WebAPI.UnitSaveJobAbility(unit004JobDialogUp._unit.id, unit004JobDialogUp._jobAbility.job_ability_id, unit004JobDialogUp.targetLevel, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = paramF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.UnitSaveJobAbility result = paramF.Result;
    if (result != null)
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      // ISSUE: reference to a compiler-generated method
      PlayerUnit unit = Array.Find<PlayerUnit>(result.player_units, new Predicate<PlayerUnit>(unit004JobDialogUp.\u003CopenAnimationLevelUp\u003Eb__47_1));
      if ((object) unit == null)
        unit = unit004JobDialogUp._unit;
      PlayerUnit newUnit = unit;
      // ISSUE: reference to a compiler-generated method
      PlayerUnitJob_abilities newJobAbility = Array.Find<PlayerUnitJob_abilities>(newUnit.job_abilities, new Predicate<PlayerUnitJob_abilities>(unit004JobDialogUp.\u003CopenAnimationLevelUp\u003Eb__47_2));
      Future<GameObject> prefab = (Future<GameObject>) null;
      prefab = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Animations.Unit_Level_Job.Unit_JobUP_sea.Load<GameObject>() : Res.Animations.Unit_Level_Job.Unit_JobUP.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = Singleton<PopupManager>.GetInstance().open(prefab.Result);
      popup.SetActive(false);
      e = popup.GetComponent<Unit004JobAnimJobUp>().Init(newUnit, newJobAbility, unit004JobDialogUp._jobAbility, unit004JobDialogUp.onUpdatedJobAbility_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Action updatedJobAbility = unit004JobDialogUp.onUpdatedJobAbility_;
      if (updatedJobAbility != null)
        updatedJobAbility();
    }
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  private Dictionary<UnitUnit, int> getRequiredMaterials(int level)
  {
    this.targetLevel = level;
    Dictionary<UnitUnit, int> requiredMaterials = new Dictionary<UnitUnit, int>();
    for (int level1 = this._jobAbility.level; level1 < this.targetLevel; ++level1)
    {
      foreach (KeyValuePair<UnitUnit, int> levelupMaterial in this._jobAbility.getLevelupMaterials(this._unit, level1))
      {
        if (requiredMaterials.ContainsKey(levelupMaterial.Key))
          requiredMaterials[levelupMaterial.Key] += levelupMaterial.Value;
        else
          requiredMaterials[levelupMaterial.Key] = levelupMaterial.Value;
      }
      JobCharacteristicsLevelupPattern characteristicsLevelupPattern;
      MasterData.JobCharacteristicsLevelupPattern.TryGetValue(this._jobAbility.master.levelup_patterns[level1], out characteristicsLevelupPattern);
      if (characteristicsLevelupPattern != null)
      {
        this.requiredMoney += characteristicsLevelupPattern.amount;
        this.requiredUnityValue = characteristicsLevelupPattern.culled_value;
      }
    }
    return requiredMaterials;
  }

  private void onSliderValueChange(int level)
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((UIRect) component).alpha = 0.0f;
    ((UIButtonColor) this.ibtnLevelUp).isEnabled = false;
    this.activeLevelUpButton = true;
    this.inneedMaterial = false;
    this.targetLevel = level;
    this.requiredMoney = 0;
    this.requiredUnityValue = new int?(0);
    Dictionary<UnitUnit, int> requiredMaterials = this.getRequiredMaterials(this.targetLevel);
    PlayerMaterialUnit[] playerUnitMaterial = SMManager.Get<PlayerMaterialUnit[]>();
    UnitIconBase[] componentsInChildren = ((Component) this.dynMaterial).GetComponentsInChildren<UnitIconBase>(true);
    int index = 0;
    foreach (UnitIconBase icon in componentsInChildren)
    {
      UnitIcon unitIcon = (UnitIcon) icon;
      bool flag = false;
      foreach (KeyValuePair<UnitUnit, int> keyValuePair in requiredMaterials)
      {
        UnitUnit key = keyValuePair.Key;
        int num = keyValuePair.Value;
        if (unitIcon.Unit.ID == key.ID)
        {
          ((Component) ((Component) this.txtMaterialNeedValue[index]).transform.parent).gameObject.SetActive(true);
          ((UIWidget) this.txtMaterialNeedValue[index]).color = this._textColor;
          if (this._isSea)
            ((Component) ((Component) this.txtMaterialPossessedValue[index]).transform.parent).gameObject.SetActive(true);
          this.ControlAndAddItem(icon, playerUnitMaterial, key, new int?(num), this.txtMaterialNeedValue[index], this.txtMaterialPossessedValue[index]);
          ((Component) icon).gameObject.SetActive(true);
          ++index;
          flag = true;
          break;
        }
      }
      if (!flag)
        ((Component) icon).gameObject.SetActive(false);
    }
    this.dynMaterial.Reposition();
    for (; index < this.txtMaterialNeedValue.Length; ++index)
    {
      ((Component) ((Component) this.txtMaterialNeedValue[index]).transform.parent).gameObject.SetActive(false);
      ((UIWidget) this.txtMaterialNeedValue[index]).color = this._textColor;
      if (this._isSea)
        ((Component) ((Component) this.txtMaterialPossessedValue[index]).transform.parent).gameObject.SetActive(false);
    }
    Modified<Player> modified = SMManager.Observe<Player>();
    this.txtZenyNeed.text = this.requiredMoney.ToString();
    this.txtZenyPossession.text = modified.Value.money.ToString();
    if (modified.Value.money < (long) this.requiredMoney)
    {
      ((UIWidget) this.txtZenyNeed).color = Color.red;
      this.activeLevelUpButton = false;
    }
    else
      ((UIWidget) this.txtZenyNeed).color = this._textColor;
    this.txtToutaNeed.SetTextLocalize(this.requiredUnityValue.ToString());
    this.txtToutaPossession.SetTextLocalize(this._unit.unityInt);
    double unityTotal = (double) this._unit.unityTotal;
    int? requiredUnityValue = this.requiredUnityValue;
    float? nullable = requiredUnityValue.HasValue ? new float?((float) requiredUnityValue.GetValueOrDefault()) : new float?();
    double valueOrDefault = (double) nullable.GetValueOrDefault();
    if (unityTotal < valueOrDefault & nullable.HasValue)
    {
      ((UIWidget) this.txtToutaNeed).color = Color.red;
      this.activeLevelUpButton = false;
    }
    else
      ((UIWidget) this.txtToutaNeed).color = this._textColor;
    this.scrollView.ResetPosition();
    if (requiredMaterials.Count > this.MATERIAL_SLOT_NUM)
    {
      ((UIProgressBar) this.scrollBar).value = 0.3f;
      ((UIProgressBar) this.scrollBar).ForceUpdate();
      ((Component) this.scrollBar).gameObject.SetActive(true);
    }
    else
    {
      ((Component) this.scrollBar).gameObject.SetActive(false);
      ((UIProgressBar) this.scrollBar).value = 0.0f;
    }
    ((UIButtonColor) this.ibtnLevelUp).isEnabled = this.activeLevelUpButton;
    if (this.inneedMaterial)
      ((Component) this.txtInneed).gameObject.SetActive(true);
    else
      ((Component) this.txtInneed).gameObject.SetActive(false);
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    ((UIRect) component).alpha = 1f;
  }
}
