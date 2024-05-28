// Decompiled with JetBrains decompiler
// Type: Unit004CombinePage
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
using UnitTraining;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/Training/CombinePage")]
public class Unit004CombinePage : Unit004TrainingPage
{
  [SerializeField]
  [Tooltip("素材表示枠下地の最低表示行数")]
  private int tilingLine_ = 5;
  [Header("ベース姫情報(dir_Status01)")]
  [SerializeField]
  private GameObject dyn_thum;
  [SerializeField]
  private GameObject limitBreak;
  [SerializeField]
  private GameObject slc_Limitbreak;
  [SerializeField]
  private GameObject[] limitBreakIcon;
  [SerializeField]
  private UISprite[] limitBreakBlue;
  [SerializeField]
  private UISprite[] limitBreakGreen;
  [SerializeField]
  private UILabel TxtLv;
  [SerializeField]
  private UILabel TxtUnity;
  [SerializeField]
  private UILabel TxtBuildupUnity;
  [SerializeField]
  private NGTweenGaugeScale dirUnityGauge;
  [SerializeField]
  private GameObject dirDearDegree;
  [SerializeField]
  private UILabel txtDear;
  [SerializeField]
  private UILabel txtDearDegreeAmountNumer;
  [SerializeField]
  private UILabel txtDearDegreeAmountDenor;
  [SerializeField]
  private GameObject[] dir_skill;
  [SerializeField]
  private GameObject[] skillObject;
  [SerializeField]
  private GameObject[] skillUpObject;
  [SerializeField]
  private GameObject[] StatusGaugeBase;
  [Header("素材ユニット情報(dir_Status02)")]
  [SerializeField]
  private GameObject ibtn_change_UpperParameter;
  [SerializeField]
  private UIDragScrollView uiDragScrollView;
  [SerializeField]
  private ScrollViewSpecifyBounds scrollView;
  [SerializeField]
  private GameObject scrollBar;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private GameObject dir_attention;
  [Header("画面下部(Bottom)")]
  [SerializeField]
  private UIButton ibtnCombine;
  private GameObject upperParameterPrefab;
  private GameObject upperParameterLabelPrefab;
  private GameObject skillTypeIconPrefab;
  private GameObject statusGaugePrefab;
  private GameObject DirSozaiItemPrefab;
  private UnitUnit baseUnitUnit_;
  private PlayerUnit[] selectUnits;
  private PlayerUnit[] duplicationSelectUnits;
  private GameObject skillDetailDialogPrefab;
  private PopupSkillDetails.Param[] skillParams;
  private Action<BattleskillSkill> showSkillDialog;
  private Action<BattleskillSkill> showUnitSkillDialog;
  private Action<int, int> showSkillLevel;
  private Action<GameObject[], NGMenuBase> popupUnityDetail;
  private float trust_;
  private float maxTrust_;
  private bool isMaterialOverUsed;
  private GameObject resultBackground_;
  private Unit004813Menu resultMain_;

  public override TrainingType page => TrainingType.Combine;

  protected override IEnumerator doInitialize()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004CombinePage unit004CombinePage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit004CombinePage.isResetBase_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unit004CombinePage.isResetBase_ = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004CombinePage.Init();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected override void preChangeTarget(Ingredients targetNext, bool bModifiedBase)
  {
  }

  protected override IEnumerator doChange(bool modifiedBase)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004CombinePage unit004CombinePage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit004CombinePage.isResetBase_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unit004CombinePage.isResetBase_ = modifiedBase;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004CombinePage.Init();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected override void onEnabledEffectFinished()
  {
    base.onEnabledEffectFinished();
    this.StartCoroutine(this.doWaitShowAdvice());
  }

  private IEnumerator doWaitShowAdvice()
  {
    Unit004CombinePage unit004CombinePage = this;
    while (Singleton<CommonRoot>.GetInstance().isLoading)
      yield return (object) null;
    unit004CombinePage.showAdvice("unit004_8_3");
  }

  public GameObject[] unityDetailPrefabs { get; private set; }

  private PlayerUnit baseUnit => this.target_.baseUnit;

  private IEnumerator Init()
  {
    Unit004CombinePage unit004CombinePage1 = this;
    unit004CombinePage1.baseUnitUnit_ = unit004CombinePage1.baseUnit.unit;
    PlayerUnit[] materialPlayerUnits = unit004CombinePage1.target_.materialPlayerUnits ?? new PlayerUnit[0];
    unit004CombinePage1.selectUnits = ((IEnumerable<PlayerUnit>) materialPlayerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).ToArray<PlayerUnit>();
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    foreach (PlayerUnit selectUnit in unit004CombinePage1.selectUnits)
    {
      for (int index = 0; index < selectUnit.UnitIconInfo.SelectedCount; ++index)
        playerUnitList.Add(selectUnit);
    }
    PlayerUnit[] array = playerUnitList.ToArray();
    bool flag = unit004CombinePage1.isResetBase_;
    if (!flag && unit004CombinePage1.duplicationSelectUnits != null && unit004CombinePage1.duplicationSelectUnits.Length == array.Length)
    {
      for (int index = 0; index < unit004CombinePage1.duplicationSelectUnits.Length; ++index)
      {
        PlayerUnit duplicationSelectUnit = unit004CombinePage1.duplicationSelectUnits[index];
        PlayerUnit playerUnit = array[index];
        if (duplicationSelectUnit.id != playerUnit.id || duplicationSelectUnit._unit != playerUnit._unit)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        yield break;
    }
    unit004CombinePage1.duplicationSelectUnits = array;
    WebAPI.Response.UnitPreviewInheritancePreview_inheritance previewInheritance = (WebAPI.Response.UnitPreviewInheritancePreview_inheritance) null;
    IEnumerator e;
    if (unit004CombinePage1.baseUnitUnit_.IsEvolutioned)
    {
      Unit004CombinePage unit004CombinePage = unit004CombinePage1;
      int baseRarity = unit004CombinePage1.baseUnitUnit_.rarity.index;
      List<int> list = ((IEnumerable<PlayerUnit>) unit004CombinePage1.selectUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x =>
      {
        UnitUnit unit = x.unit;
        return unit004CombinePage.baseUnitUnit_.same_character_id == unit.same_character_id && baseRarity <= unit.rarity.index && UnitUtil.checkAnyInheritance(x);
      })).Select<PlayerUnit, int>((Func<PlayerUnit, int>) (u => u.id)).OrderBy<int, int>((Func<int, int>) (i => i)).ToList<int>();
      if (list.Any<int>())
      {
        NGGameDataManager gdm = Singleton<NGGameDataManager>.GetInstance();
        string ikey = gdm.makeKeyPreviewInheritance(unit004CombinePage1.baseUnit.id, list);
        if (!gdm.dicPreviewInheritance.TryGetValue(ikey, out previewInheritance))
        {
          Future<WebAPI.Response.UnitPreviewInheritance> future = WebAPI.UnitPreviewInheritance(unit004CombinePage1.baseUnit.id, list.ToArray(), (Action<WebAPI.Response.UserError>) (error =>
          {
            WebAPI.DefaultUserErrorCallback(error);
            Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
          }));
          e = future.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (future.Result != null)
            previewInheritance = future.Result.preview_inheritance;
          if (previewInheritance == null)
          {
            yield break;
          }
          else
          {
            gdm.dicPreviewInheritance.Add(ikey, previewInheritance);
            future = (Future<WebAPI.Response.UnitPreviewInheritance>) null;
          }
        }
        gdm = (NGGameDataManager) null;
        ikey = (string) null;
      }
    }
    while (unit004CombinePage1.isLoadingResources)
      yield return (object) null;
    yield return (object) unit004CombinePage1.Show_dir_Status01(previewInheritance, unit004CombinePage1.getTrustUpValue(unit004CombinePage1.duplicationSelectUnits));
    unit004CombinePage1.scrollView.ClearBounds();
    ((Component) unit004CombinePage1.grid).transform.Clear();
    UnitUnit nextUnit = unit004CombinePage1.checkAwaking() ?? unit004CombinePage1.baseUnitUnit_;
    int limitBreakCount = 0;
    int num1 = unit004CombinePage1.grid.maxPerLine * unit004CombinePage1.tilingLine_;
    if (unit004CombinePage1.selectUnits.Length == 0 || ((IEnumerable<PlayerUnit>) unit004CombinePage1.selectUnits).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.UnitIconInfo.SelectedCount > 0)) <= 0)
    {
      ((Behaviour) unit004CombinePage1.uiDragScrollView).enabled = false;
      unit004CombinePage1.dir_attention.SetActive(true);
      unit004CombinePage1.scrollBar.SetActive(false);
      unit004CombinePage1.ibtn_change_UpperParameter.SetActive(false);
      for (int index = 0; index < num1; ++index)
        unit004CombinePage1.DirSozaiItemPrefab.CloneAndGetComponent<SozaiItem>(((Component) unit004CombinePage1.grid).transform).SetOnlyWLine();
    }
    else
    {
      ((Behaviour) unit004CombinePage1.uiDragScrollView).enabled = true;
      unit004CombinePage1.dir_attention.SetActive(false);
      unit004CombinePage1.scrollBar.SetActive(true);
      unit004CombinePage1.ibtn_change_UpperParameter.SetActive(true);
      int numTile = Mathf.Max((unit004CombinePage1.selectUnits.Length + unit004CombinePage1.grid.maxPerLine - 1) / unit004CombinePage1.grid.maxPerLine * unit004CombinePage1.grid.maxPerLine, num1);
      List<UIWidget> widgetList = new List<UIWidget>();
      for (int i = 0; i < numTile; ++i)
      {
        SozaiItem component1 = unit004CombinePage1.DirSozaiItemPrefab.CloneAndGetComponent<SozaiItem>(((Component) unit004CombinePage1.grid).transform);
        if (i >= unit004CombinePage1.selectUnits.Length)
        {
          component1.SetOnlyWLine();
        }
        else
        {
          PlayerUnit materialPlayerUnit = unit004CombinePage1.selectUnits[i];
          UnitUnit material_unit = materialPlayerUnit.unit;
          if (materialPlayerUnit.UnitIconInfo.SelectedCount <= 0)
          {
            component1.SetOnlyWLine();
          }
          else
          {
            UpperParameter component2 = unit004CombinePage1.upperParameterPrefab.CloneAndGetComponent<UpperParameter>(component1.DirSozaiBase.transform);
            component2.Init(materialPlayerUnit);
            widgetList.AddRange((IEnumerable<UIWidget>) ((Component) component2).gameObject.GetComponentsInChildren<UIWidget>());
            UpperParameterLabel component3 = unit004CombinePage1.upperParameterLabelPrefab.CloneAndGetComponent<UpperParameterLabel>(component1.DirSozaiBase2.transform);
            component3.Init(unit004CombinePage1.baseUnit, materialPlayerUnit);
            widgetList.AddRange((IEnumerable<UIWidget>) ((Component) component3).gameObject.GetComponentsInChildren<UIWidget>());
            UnitIcon unitIcon = unit004CombinePage1.unitIconPrefab_.CloneAndGetComponent<UnitIcon>(component1.LinkSozaiBase.transform);
            component1.UnitIcon = unitIcon;
            // ISSUE: reference to a compiler-generated method
            unitIcon.onClick = new Action<UnitIconBase>(unit004CombinePage1.\u003CInit\u003Eb__56_8);
            unitIcon.EnabledExpireDate = true;
            if (material_unit.IsMaterialUnit)
            {
              PlayerMaterialUnit playerMaterialUnit = Array.Find<PlayerMaterialUnit>(SMManager.Get<PlayerMaterialUnit[]>(), (Predicate<PlayerMaterialUnit>) (x => x._unit == material_unit.ID));
              if (playerMaterialUnit != null)
              {
                ((Component) component1.TxtPossessionNum).gameObject.SetActive(true);
                component1.TxtPossessionNum.SetTextLocalize(Consts.Format(Consts.GetInstance().unit_004_9_9_possession_text, (IDictionary) new Hashtable()
                {
                  {
                    (object) "Count",
                    (object) playerMaterialUnit.quantity
                  }
                }));
              }
              e = unitIcon.SetMaterialUnit(materialPlayerUnit, false, materialPlayerUnits);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
            }
            else
            {
              ((Component) component1.TxtPossessionNum).gameObject.SetActive(false);
              e = unitIcon.SetPlayerUnitEvolution(materialPlayerUnit, materialPlayerUnits, unit004CombinePage1.baseUnit, true);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
            }
            unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
            unitIcon.SelectByCheckAndNumber(materialPlayerUnit.UnitIconInfo);
            unitIcon.Gray = false;
            UISprite component4 = unitIcon.Dir_select_check.GetComponent<UISprite>();
            if (Object.op_Inequality((Object) component4, (Object) null))
              ((Behaviour) component4).enabled = false;
            if (material_unit.same_character_id == unit004CombinePage1.baseUnitUnit_.same_character_id || material_unit.IsBreakThrough || material_unit.IsBreakThrougPureValueUp(unit004CombinePage1.baseUnit))
              limitBreakCount += !material_unit.IsBreakThrough ? (!material_unit.IsPureValueUp ? (unit004CombinePage1.baseUnitUnit_.rarity.index > material_unit.rarity.index ? materialPlayerUnit.unity_value + 1 : materialPlayerUnit.breakthrough_count + 1) : materialPlayerUnit.UnitIconInfo.SelectedCount) : materialPlayerUnit.UnitIconInfo.SelectedCount;
            int num2 = nextUnit.breakthrough_limit - unit004CombinePage1.baseUnit.breakthrough_count;
            if (limitBreakCount > num2)
              limitBreakCount = num2;
            materialPlayerUnit = (PlayerUnit) null;
            unitIcon = (UnitIcon) null;
          }
        }
      }
      unit004CombinePage1.scrollView.AddBounds((IEnumerable<UIWidget>) widgetList);
      widgetList = (List<UIWidget>) null;
    }
    // ISSUE: method pointer
    unit004CombinePage1.grid.onReposition = new UIGrid.OnReposition((object) unit004CombinePage1, __methodptr(\u003CInit\u003Eb__56_2));
    unit004CombinePage1.grid.Reposition();
    unit004CombinePage1.grid.repositionNow = true;
    unit004CombinePage1.SetWidthLine();
    unit004CombinePage1.onIbtnChangeUpperParameter();
    if (nextUnit != unit004CombinePage1.baseUnitUnit_)
    {
      UnitUnitParameter parameterData = nextUnit.parameter_data;
      unit004CombinePage1.TxtLv.SetTextLocalize(parameterData._initial_max_level + (unit004CombinePage1.baseUnit.breakthrough_count + limitBreakCount) * parameterData._level_per_breakthrough);
    }
    else
      unit004CombinePage1.TxtLv.SetTextLocalize((unit004CombinePage1.baseUnit.max_level + limitBreakCount * unit004CombinePage1.baseUnitUnit_._level_per_breakthrough).ToString());
    unit004CombinePage1.setLimitBreak(limitBreakCount);
    e = unit004CombinePage1.setSkillIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004CombinePage1.setUnityValue(unit004CombinePage1.duplicationSelectUnits);
    unit004CombinePage1.setPrice(unit004CombinePage1.duplicationSelectUnits);
    ((UIButtonColor) unit004CombinePage1.ibtnCombine).isEnabled = unit004CombinePage1.duplicationSelectUnits.Length != 0 && OverkillersUtil.checkDelete(((IEnumerable<PlayerUnit>) unit004CombinePage1.selectUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>());
  }

  private UnitUnit checkAwaking()
  {
    if (this.baseUnitUnit_.awake_unit_flag || !this.baseUnitUnit_.CanAwakeUnitFlag)
      return (UnitUnit) null;
    if (!((IEnumerable<PlayerUnit>) this.selectUnits).Any<PlayerUnit>((Func<PlayerUnit, bool>) (x =>
    {
      UnitUnit unit = x.unit;
      if (unit.same_character_id != this.baseUnitUnit_.same_character_id)
        return false;
      return unit.awake_unit_flag || x.hasAwakeState;
    })))
      return (UnitUnit) null;
    return Array.FindLast<UnitEvolutionPattern>(UnitEvolutionPattern.getGenealogy(this.baseUnitUnit_.ID), (Predicate<UnitEvolutionPattern>) (x => x.target_unit.awake_unit_flag))?.target_unit;
  }

  protected override IEnumerator loadResources()
  {
    Unit004CombinePage unit004CombinePage = this;
    yield return (object) unit004CombinePage.doLoadCommonPrefab();
    Future<GameObject> prefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) unit004CombinePage.upperParameterPrefab, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/UnitIcon/UpperParameter").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004CombinePage.upperParameterPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004CombinePage.upperParameterLabelPrefab, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/UnitIcon/UpperParameterLabel").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004CombinePage.upperParameterLabelPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004CombinePage.skillTypeIconPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004CombinePage.skillTypeIconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004CombinePage.statusGaugePrefab, (Object) null))
    {
      prefabF = Res.Prefabs.unit004_8_3.StatusGauge.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004CombinePage.statusGaugePrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004CombinePage.DirSozaiItemPrefab, (Object) null))
    {
      prefabF = new ResourceObject("Prefabs/unit004_8_4/dir_Sozai_Item").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004CombinePage.DirSozaiItemPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    e = unit004CombinePage.CreateSkillDetailDialog();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unit004CombinePage.unityDetailPrefabs == null)
    {
      unit004CombinePage.unityDetailPrefabs = new GameObject[2];
      Future<GameObject>[] loaders = PopupUnityValueDetail.createLoaders(false);
      yield return (object) loaders[0].Wait();
      unit004CombinePage.unityDetailPrefabs[0] = loaders[0].Result;
      yield return (object) loaders[1].Wait();
      unit004CombinePage.unityDetailPrefabs[1] = loaders[1].Result;
      loaders = (Future<GameObject>[]) null;
    }
  }

  private IEnumerator Show_dir_Status01(
    WebAPI.Response.UnitPreviewInheritancePreview_inheritance previewInheritance,
    int addedTrustValue)
  {
    Unit004CombinePage unit004CombinePage = this;
    if (unit004CombinePage.isResetBase_)
    {
      yield return (object) unit004CombinePage.initUnitIcon(unit004CombinePage.dyn_thum, unit004CombinePage.baseUnit, true, true);
      for (int index = 0; index < unit004CombinePage.limitBreakIcon.Length; ++index)
        unit004CombinePage.limitBreakIcon[index].SetActive(index < unit004CombinePage.baseUnitUnit_.breakthrough_limit);
    }
    unit004CombinePage.setTrust(addedTrustValue);
    unit004CombinePage.setStatusGauge(unit004CombinePage.statusGaugePrefab, unit004CombinePage.duplicationSelectUnits, previewInheritance);
  }

  private void SetWidthLine()
  {
    int num = 0;
    foreach (Component component in ((Component) this.grid).transform)
    {
      component.GetComponent<SozaiItem>().WLine.SetActive(num % this.grid.maxPerLine == 0);
      ++num;
    }
  }

  private void setTrust(int addTrustValue)
  {
    this.dirDearDegree.SetActive(false);
    if (!this.baseUnitUnit_.trust_target_flag)
      return;
    this.dirDearDegree.SetActive(true);
    if (this.baseUnitUnit_.IsSea)
      this.txtDear.SetTextLocalize(Consts.GetInstance().TXT_DEAR);
    else if (this.baseUnitUnit_.IsResonanceUnit)
      this.txtDear.SetTextLocalize(Consts.GetInstance().TXT_RELEVANCE);
    else
      this.txtDear.SetTextLocalize(Consts.GetInstance().TXT_DEAR);
    this.trust_ = this.baseUnit.trust_rate;
    float trustRateMax = (float) PlayerUnit.GetTrustRateMax();
    float? nullable1 = new float?();
    foreach (PlayerUnit selectUnit in this.selectUnits)
    {
      if (selectUnit.UnitIconInfo.SelectedCount > 0)
      {
        if (selectUnit.unit.IsTrustMaterial(this.baseUnit))
        {
          this.trust_ += selectUnit.unit.TrustMaterialUnit(this.baseUnit).increase_value * (float) selectUnit.UnitIconInfo.SelectedCount;
          this.trust_ += selectUnit.trust_rate;
          float increaseValue = selectUnit.unit.TrustMaterialUnit(this.baseUnit).increase_value;
          if (selectUnit.UnitIconInfo.SelectedCount == 1 && (double) selectUnit.trust_rate != (double) trustRateMax)
            increaseValue += selectUnit.trust_rate;
          nullable1 = nullable1.HasValue ? new float?(Mathf.Min(nullable1.Value, increaseValue)) : new float?(increaseValue);
        }
        else if (selectUnit.unit.IsPureValueUp)
        {
          this.trust_ += (float) PlayerUnit.GetTrustComposeRate() * (float) selectUnit.UnitIconInfo.SelectedCount;
          this.trust_ += selectUnit.trust_rate;
        }
        else if (this.baseUnitUnit_.same_character_id == selectUnit.unit.same_character_id)
        {
          this.trust_ += (float) PlayerUnit.GetTrustComposeRate();
          this.trust_ += selectUnit.trust_rate;
        }
        else if (this.baseUnitUnit_.character.ID == selectUnit.unit.character.ID)
          this.trust_ += selectUnit.unit.rarity.trust_rate * (float) (selectUnit.unity_value + 1);
      }
    }
    Consts.GetInstance();
    this.maxTrust_ = this.baseUnit.trust_max_rate + (float) addTrustValue;
    if ((double) this.maxTrust_ > (double) trustRateMax)
      this.maxTrust_ = trustRateMax;
    if (nullable1.HasValue)
    {
      float trust = this.trust_;
      float? nullable2 = nullable1;
      float? nullable3 = nullable2.HasValue ? new float?(trust - nullable2.GetValueOrDefault()) : new float?();
      float maxTrust = this.maxTrust_;
      if ((double) nullable3.GetValueOrDefault() > (double) maxTrust & nullable3.HasValue)
      {
        this.isMaterialOverUsed = true;
        goto label_26;
      }
    }
    this.isMaterialOverUsed = false;
label_26:
    if ((double) this.trust_ > (double) this.maxTrust_)
      this.trust_ = this.maxTrust_;
    if ((double) this.trust_ != (double) this.baseUnit.trust_rate)
      ((UIWidget) this.txtDearDegreeAmountNumer).color = Color.yellow;
    else
      ((UIWidget) this.txtDearDegreeAmountNumer).color = Color.white;
    if (addTrustValue > 0 && (double) this.baseUnit.trust_max_rate < (double) trustRateMax)
      ((UIWidget) this.txtDearDegreeAmountDenor).color = Color.yellow;
    else
      ((UIWidget) this.txtDearDegreeAmountDenor).color = Color.white;
    this.txtDearDegreeAmountDenor.SetTextLocalize((Math.Round((double) this.maxTrust_ * 100.0) / 100.0).ToString());
    this.txtDearDegreeAmountNumer.SetTextLocalize((Math.Round((double) this.trust_ * 100.0) / 100.0).ToString());
  }

  private Dictionary<int, float> SkillLevelUpRatio(
    PlayerUnit[] materialUnits,
    Dictionary<int, int> playerSkillDict)
  {
    Dictionary<int, float> dictionary = new Dictionary<int, float>();
    foreach (BattleskillSkill battleSkill in this.baseUnit.GetBattleSkills())
      dictionary[this.baseUnit.evolutionSkill(battleSkill).ID] = 0.0f;
    if (materialUnits.Length < 1)
      return dictionary;
    Tuple<PlayerUnitSkills, BattleskillSkill>[] array = ((IEnumerable<PlayerUnitSkills>) this.baseUnit.skills).Select<PlayerUnitSkills, Tuple<PlayerUnitSkills, BattleskillSkill>>((Func<PlayerUnitSkills, Tuple<PlayerUnitSkills, BattleskillSkill>>) (x => Tuple.Create<PlayerUnitSkills, BattleskillSkill>(x, this.baseUnit.evolutionSkill(x.skill)))).Where<Tuple<PlayerUnitSkills, BattleskillSkill>>((Func<Tuple<PlayerUnitSkills, BattleskillSkill>, bool>) (y => y.Item1.level < y.Item2.upper_level)).ToArray<Tuple<PlayerUnitSkills, BattleskillSkill>>();
    foreach (PlayerUnit materialUnit in materialUnits)
    {
      UnitUnit mu = materialUnit.unit;
      if (mu.same_character_id == this.baseUnitUnit_.same_character_id)
      {
        Dictionary<int, int> skillsDictionary = materialUnit.GetAcquireSkillsDictionary();
        foreach (Tuple<PlayerUnitSkills, BattleskillSkill> tuple in array)
        {
          int id = tuple.Item2.ID;
          int num;
          if (skillsDictionary.TryGetValue(id, out num) && num > 0)
          {
            dictionary[id] = 100f;
          }
          else
          {
            UnitRarity rarity = mu.rarity;
            if ((double) rarity.skill_levelup_rate > (double) dictionary[tuple.Item1.skill_id])
              dictionary[id] = (float) rarity.skill_levelup_rate;
          }
        }
      }
      else if (mu.IsPureValueUp)
      {
        foreach (Tuple<PlayerUnitSkills, BattleskillSkill> tuple in array)
        {
          int id = tuple.Item2.ID;
          if (dictionary.ContainsKey(id))
            dictionary[id] = 100f;
        }
      }
      else
      {
        foreach (Tuple<PlayerUnitSkills, BattleskillSkill> tuple in array)
        {
          Tuple<PlayerUnitSkills, BattleskillSkill> x = tuple;
          BattleskillSkill battleskillSkill = x.Item2;
          if (dictionary.ContainsKey(battleskillSkill.ID))
          {
            if (UnitDetailIcon.IsSkillUpMaterial(mu, this.baseUnit))
            {
              UnitSkillupSetting unitSkillupSetting = Array.Find<UnitSkillupSetting>(MasterData.UnitSkillupSettingList, (Predicate<UnitSkillupSetting>) (y => y.material_unit_id == mu.ID));
              bool flag = battleskillSkill.skill_type == (BattleskillSkillType) mu.skillup_type || mu.skillup_type == UnitDetailIcon.SKILL_ID_ALL;
              if (flag && unitSkillupSetting != null && unitSkillupSetting.skill_group.HasValue)
              {
                int num = unitSkillupSetting.skill_group.Value;
                flag = false;
                foreach (UnitSkillupSkillGroupSetting skillGroupSetting in MasterData.UnitSkillupSkillGroupSettingList)
                {
                  if (skillGroupSetting.group_id == num && skillGroupSetting.skill_id == battleskillSkill.ID)
                  {
                    flag = true;
                    break;
                  }
                }
              }
              if (flag)
              {
                float num = unitSkillupSetting != null ? unitSkillupSetting.levelup_ratio * 100f : 20f;
                if ((double) num > (double) dictionary[battleskillSkill.ID])
                  dictionary[battleskillSkill.ID] = num;
              }
            }
            else if (materialUnit.skills != null)
            {
              PlayerUnitSkills playerUnitSkills = Array.Find<PlayerUnitSkills>(materialUnit.skills, (Predicate<PlayerUnitSkills>) (y => x.Item1.skill_id == y.skill_id));
              if (playerUnitSkills != null)
              {
                float num = UnitSkillLevelUpProbability.Probability(x.Item1.level, playerUnitSkills.level) * 100f;
                if ((double) num > (double) dictionary[battleskillSkill.ID])
                  dictionary[battleskillSkill.ID] = num;
              }
            }
          }
        }
      }
    }
    return dictionary;
  }

  private IEnumerator CreateSkillDetailDialog()
  {
    Future<GameObject> loader = PopupSkillDetails.createPrefabLoader(false);
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillDetailDialogPrefab = loader.Result;
  }

  private void popupSkillDetail(BattleskillSkill skill)
  {
    PopupSkillDetails.Param[] skillParams = this.skillParams;
    int? nullable = skillParams != null ? ((IEnumerable<PopupSkillDetails.Param>) skillParams).FirstIndexOrNull<PopupSkillDetails.Param>((Func<PopupSkillDetails.Param, bool>) (x => x.skill == skill)) : new int?();
    if (!nullable.HasValue)
      return;
    PopupSkillDetails.show(this.skillDetailDialogPrefab, this.skillParams[nullable.Value]);
  }

  private void setPrice(PlayerUnit[] select)
  {
    this.setCostZeny(CalcUnitCompose.priceCompose(this.baseUnit, select));
  }

  private void setUnityValue(PlayerUnit[] materialUnits)
  {
    float unityValueCount = 0.0f;
    float buildupUnityCount = 0.0f;
    float MAX_UNITYVALUE = (float) PlayerUnit.GetUnityValueMax();
    if ((double) this.baseUnit.unity_value < (double) MAX_UNITYVALUE && materialUnits.Length != 0)
    {
      Func<float, float, float> func1 = (double) this.baseUnit.unityTotal < (double) MAX_UNITYVALUE ? (Func<float, float, float>) ((now, add) => Mathf.Min(now + add, MAX_UNITYVALUE)) : (Func<float, float, float>) ((a, b) => 0.0f);
      Func<float, float, float> func2 = (double) this.baseUnit.unity_value < (double) MAX_UNITYVALUE ? (Func<float, float, float>) ((now, add) => Mathf.Min(now + add, MAX_UNITYVALUE)) : (Func<float, float, float>) ((a, b) => 0.0f);
      foreach (PlayerUnit materialUnit in materialUnits)
      {
        UnitUnit unit = materialUnit.unit;
        if (unit.IsNormalUnit)
        {
          if (unit.same_character_id == this.baseUnitUnit_.same_character_id)
          {
            unityValueCount = Mathf.Min(unityValueCount + (float) materialUnit.unity_value + (float) PlayerUnit.GetUnityValue(), MAX_UNITYVALUE);
            buildupUnityCount = func1(buildupUnityCount, materialUnit.buildup_unity_value_f);
          }
        }
        else if (unit.is_unity_value_up)
        {
          UnitUnit unitUnit = unit;
          UnitUnit baseUnitUnit = this.baseUnitUnit_;
          Func<UnitFamily[]> funcGetFamilies = (Func<UnitFamily[]>) (() => this.baseUnit.Families);
          UnityValueUpPattern valueUpPattern;
          if ((valueUpPattern = unitUnit.FindValueUpPattern(baseUnitUnit, funcGetFamilies)) != null)
            buildupUnityCount = func1(buildupUnityCount, valueUpPattern.up_value);
          else if (unit.FindPureValueUpPattern(this.baseUnitUnit_) != null)
            unityValueCount = func2(unityValueCount, 1f);
        }
      }
    }
    if ((double) unityValueCount == 0.0 && (double) buildupUnityCount == 0.0)
    {
      ((UIWidget) this.TxtUnity).color = Color.white;
      ((UIWidget) this.TxtBuildupUnity).color = Color.white;
    }
    else
    {
      ((UIWidget) this.TxtUnity).color = Color.yellow;
      ((UIWidget) this.TxtBuildupUnity).color = Color.yellow;
    }
    double unityTotal = (double) this.baseUnit.unityTotal;
    double number = (double) Mathf.Min(this.baseUnit.unityTotal + unityValueCount + buildupUnityCount, MAX_UNITYVALUE);
    int integer = ((float) number).GetInteger();
    int decimalAsPercent = ((float) number).GetDecimalAsPercent();
    this.TxtUnity.SetTextLocalize(integer);
    this.TxtBuildupUnity.SetTextLocalize(decimalAsPercent.ToString() + "%");
    this.dirUnityGauge.setValue(decimalAsPercent, 99, false);
    int targetId = this.baseUnit.id;
    bool bDisabledSelect = this.isDisabledQuestSelect;
    this.popupUnityDetail = (Action<GameObject[], NGMenuBase>) ((prefabs, m) =>
    {
      if (prefabs.Length != 2)
        return;
      PlayerUnit target = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == targetId));
      if (target == (PlayerUnit) null)
        return;
      float unityValue = Mathf.Min((float) target.unity_value + unityValueCount, MAX_UNITYVALUE);
      float buildupUnity = Mathf.Min(target.buildup_unity_value_f + buildupUnityCount, MAX_UNITYVALUE);
      PopupUnityValueDetail.show(prefabs[0], prefabs[1], unityValue, buildupUnity, target, (Action<Action>) (endWait =>
      {
        NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
        instance.setSceneChangeLog(Singleton<NGSceneManager>.GetInstance().exportSceneChangeLog());
        instance.OpenPopup = this.popupUnityDetail;
        instance.returnSceneFromQuest = (Action) (() => Unit004CombinePage.returnSceneFromQuest(target));
        if (endWait == null)
          return;
        endWait();
      }), (Action<PopupUtility.SceneTo>) (_ => { }), bDisabledSelect);
    });
  }

  private bool isDisabledQuestSelect
  {
    get
    {
      string sceneStackPath = Singleton<NGSceneManager>.GetInstance().getSceneStackPath();
      return sceneStackPath != "" && sceneStackPath != "unit004_UnitTraining_List";
    }
  }

  private static void returnSceneFromQuest(PlayerUnit unit)
  {
    PlayerUnit playerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == unit.id));
    Unit004TrainingScene.changeCombine(false, playerUnit != (PlayerUnit) null ? playerUnit : unit);
  }

  private int getTrustUpValue(PlayerUnit[] materialUnits)
  {
    if (materialUnits == null)
      return 0;
    int num = 0;
    foreach (PlayerUnit materialUnit in materialUnits)
    {
      if (materialUnit.unit.IsNormalUnit && materialUnit.unit.same_character_id == this.baseUnitUnit_.same_character_id)
        num += (int) materialUnit.trust_max_rate;
      else if (materialUnit.unit.IsPureValueUp)
        num += (int) materialUnit.trust_max_rate;
    }
    return Mathf.Min(num, PlayerUnit.GetTrustRateMax());
  }

  private void setStatusGauge(
    GameObject prefabGauge,
    PlayerUnit[] materialPlayerUnits,
    WebAPI.Response.UnitPreviewInheritancePreview_inheritance previewInheritance)
  {
    CalcUnitCompose.ComposeType[] composeTypeArray = new CalcUnitCompose.ComposeType[8]
    {
      CalcUnitCompose.ComposeType.HP,
      CalcUnitCompose.ComposeType.STRENGTH,
      CalcUnitCompose.ComposeType.INTELLIGENCE,
      CalcUnitCompose.ComposeType.VITALITY,
      CalcUnitCompose.ComposeType.MIND,
      CalcUnitCompose.ComposeType.AGILITY,
      CalcUnitCompose.ComposeType.DEXTERITY,
      CalcUnitCompose.ComposeType.LUCKY
    };
    int[] numArray1 = new int[8]
    {
      this.baseUnit.hp.inheritance,
      this.baseUnit.strength.inheritance,
      this.baseUnit.intelligence.inheritance,
      this.baseUnit.vitality.inheritance,
      this.baseUnit.mind.inheritance,
      this.baseUnit.agility.inheritance,
      this.baseUnit.dexterity.inheritance,
      this.baseUnit.lucky.inheritance
    };
    int[] numArray2 = new int[8]
    {
      this.baseUnit.self_total_hp,
      this.baseUnit.self_total_strength,
      this.baseUnit.self_total_intelligence,
      this.baseUnit.self_total_vitality,
      this.baseUnit.self_total_mind,
      this.baseUnit.self_total_agility,
      this.baseUnit.self_total_dexterity,
      this.baseUnit.self_total_lucky
    };
    int[] numArray3 = new int[8]
    {
      this.baseUnit.hp.compose,
      this.baseUnit.strength.compose,
      this.baseUnit.intelligence.compose,
      this.baseUnit.vitality.compose,
      this.baseUnit.mind.compose,
      this.baseUnit.agility.compose,
      this.baseUnit.dexterity.compose,
      this.baseUnit.lucky.compose
    };
    int[] numArray4 = new int[8]
    {
      this.baseUnit.hp.buildup,
      this.baseUnit.strength.buildup,
      this.baseUnit.intelligence.buildup,
      this.baseUnit.vitality.buildup,
      this.baseUnit.mind.buildup,
      this.baseUnit.agility.buildup,
      this.baseUnit.dexterity.buildup,
      this.baseUnit.lucky.buildup
    };
    bool[] flagArray = new bool[8]
    {
      this.baseUnit.hp.is_max,
      this.baseUnit.strength.is_max,
      this.baseUnit.intelligence.is_max,
      this.baseUnit.vitality.is_max,
      this.baseUnit.mind.is_max,
      this.baseUnit.agility.is_max,
      this.baseUnit.dexterity.is_max,
      this.baseUnit.lucky.is_max
    };
    int[] previewInheritances = new int[8]
    {
      previewInheritance != null ? previewInheritance.hp : 0,
      previewInheritance != null ? previewInheritance.strength : 0,
      previewInheritance != null ? previewInheritance.intelligence : 0,
      previewInheritance != null ? previewInheritance.vitality : 0,
      previewInheritance != null ? previewInheritance.mind : 0,
      previewInheritance != null ? previewInheritance.agility : 0,
      previewInheritance != null ? previewInheritance.dexterity : 0,
      previewInheritance != null ? previewInheritance.lucky : 0
    };
    UnitTypeParameter unitTypeParameter = this.baseUnit.UnitTypeParameter;
    int gaugeMax = ((IEnumerable<int>) new int[8]
    {
      this.baseUnit.hp.initial + this.baseUnit.hp.inheritance + this.baseUnit.hp.level_up_max_status + unitTypeParameter.hp_compose_max,
      this.baseUnit.strength.initial + this.baseUnit.strength.inheritance + this.baseUnit.strength.level_up_max_status + unitTypeParameter.strength_compose_max,
      this.baseUnit.intelligence.initial + this.baseUnit.intelligence.inheritance + this.baseUnit.intelligence.level_up_max_status + unitTypeParameter.intelligence_compose_max,
      this.baseUnit.vitality.initial + this.baseUnit.vitality.inheritance + this.baseUnit.vitality.level_up_max_status + unitTypeParameter.vitality_compose_max,
      this.baseUnit.mind.initial + this.baseUnit.mind.inheritance + this.baseUnit.mind.level_up_max_status + unitTypeParameter.mind_compose_max,
      this.baseUnit.agility.initial + this.baseUnit.agility.inheritance + this.baseUnit.agility.level_up_max_status + unitTypeParameter.agility_compose_max,
      this.baseUnit.dexterity.initial + this.baseUnit.dexterity.inheritance + this.baseUnit.dexterity.level_up_max_status + unitTypeParameter.dexterity_compose_max,
      this.baseUnit.lucky.initial + this.baseUnit.lucky.inheritance + this.baseUnit.lucky.level_up_max_status + unitTypeParameter.lucky_compose_max
    }).Select<int, int>((Func<int, int, int>) ((v, i) => v + previewInheritances[i])).Max();
    int length = this.StatusGaugeBase.Length;
    for (int index = 0; index < length; ++index)
    {
      GameObject gameObject = this.StatusGaugeBase[index];
      gameObject.transform.Clear();
      prefabGauge.CloneAndGetComponent<Unit00483StatusGauge>(gameObject).Init(this.baseUnit, materialPlayerUnits, composeTypeArray[index], numArray1[index], previewInheritances[index], numArray2[index], numArray3[index], numArray4[index], flagArray[index], gaugeMax);
      NGTween.playTweens(NGTween.findTweenersAll(gameObject, false), 0);
    }
  }

  private void setLimitBreak(int limitBreakCount)
  {
    this.limitBreak.SetActive(true);
    ((IEnumerable<UISprite>) this.limitBreakBlue).SetActiveRange<UISprite>(true, 0, this.baseUnit.breakthrough_count);
    ((IEnumerable<UISprite>) this.limitBreakBlue).SetActiveRange<UISprite>(false, this.baseUnit.breakthrough_count);
    ((IEnumerable<UISprite>) this.limitBreakGreen).SetActives<UISprite>(false);
    int num = this.baseUnit.breakthrough_count + limitBreakCount;
    for (int breakthroughCount = this.baseUnit.breakthrough_count; breakthroughCount < num; ++breakthroughCount)
    {
      UISprite uiSprite = this.limitBreakGreen[breakthroughCount];
      ((Component) uiSprite).gameObject.SetActive(true);
      NGTween.playTweens(NGTween.findTweenersAll(((Component) uiSprite).gameObject, false), 0);
    }
    if (this.baseUnitUnit_.breakthrough_limit != 0)
      return;
    this.slc_Limitbreak.SetActive(false);
  }

  private IEnumerator CreateSkillIcon(BattleskillSkill sk, int idx, int unitSkillLv, int needLv)
  {
    BattleSkillIcon component = this.skillTypeIconPrefab.Clone(this.skillObject[idx].transform).GetComponent<BattleSkillIcon>();
    component.SetDepth(6);
    if (unitSkillLv == 0)
      component.EnableNeedLvIcon(needLv);
    IEnumerator e = component.Init(sk);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.skillObject[idx].GetComponent<UIButton>().onClick.Clear();
    EventDelegate.Add(this.skillObject[idx].GetComponent<UIButton>().onClick, (EventDelegate.Callback) (() => this.popupSkillDetail(sk)));
  }

  private void DisplaySkillLevelUpArrow(int idx, float rate)
  {
    if ((double) rate <= 0.0)
    {
      this.skillUpObject[idx].SetActive(false);
    }
    else
    {
      this.skillUpObject[idx].SetActive(true);
      string str1 = "slc_SkillUP";
      string str2 = (double) rate >= 40.0 ? ((double) rate >= 70.0 ? ((double) rate >= 100.0 ? str1 + "4" : str1 + "3") : str1 + "2") : str1 + "1";
      foreach (Component child in this.skillUpObject[idx].transform.GetChildren())
        child.gameObject.SetActive(false);
      Transform transform = this.skillUpObject[idx].transform.Find(str2);
      if (Object.op_Inequality((Object) transform, (Object) null))
        ((Component) transform).gameObject.SetActive(true);
    }
    NGTween.playTweens(NGTween.findTweenersAll(this.skillUpObject[idx], false), 0);
  }

  private IEnumerator setSkillIcon()
  {
    Unit004CombinePage unit004CombinePage = this;
    foreach (GameObject gameObject in unit004CombinePage.skillObject)
      gameObject.transform.Clear();
    Dictionary<int, int> playerSkillDict = new Dictionary<int, int>();
    foreach (PlayerUnitSkills acquireSkill in unit004CombinePage.baseUnit.GetAcquireSkills())
    {
      if (!playerSkillDict.ContainsKey(acquireSkill.skill_id))
        playerSkillDict.Add(acquireSkill.skill_id, acquireSkill.level);
    }
    UnitSkill[] skills1 = unit004CombinePage.baseUnit.GetSkills();
    UnitSkillCharacterQuest[] characterSkills = unit004CombinePage.baseUnit.GetCharacterSkills();
    UnitSkillHarmonyQuest[] harmonySkills = unit004CombinePage.baseUnit.GetHarmonySkills();
    UnitSkillIntimate[] intimateSkills = unit004CombinePage.baseUnit.GetIntimateSkills();
    IEnumerable<int> allSkillIDs = ((IEnumerable<UnitSkill>) skills1).Select<UnitSkill, int>((Func<UnitSkill, int>) (x => x.skill_BattleskillSkill)).Concat<int>(((IEnumerable<UnitSkillCharacterQuest>) characterSkills).Select<UnitSkillCharacterQuest, int>((Func<UnitSkillCharacterQuest, int>) (x => x.skill_BattleskillSkill))).Concat<int>(((IEnumerable<UnitSkillHarmonyQuest>) harmonySkills).Select<UnitSkillHarmonyQuest, int>((Func<UnitSkillHarmonyQuest, int>) (x => x.skill_BattleskillSkill))).Concat<int>(((IEnumerable<UnitSkillIntimate>) intimateSkills).Select<UnitSkillIntimate, int>((Func<UnitSkillIntimate, int>) (x => x.skill_BattleskillSkill))).Distinct<int>();
    List<int> second1 = new List<int>();
    foreach (int key in allSkillIDs)
      second1.Add(unit004CombinePage.baseUnit.evolutionSkill(MasterData.BattleskillSkill[key]).ID);
    allSkillIDs = allSkillIDs.Concat<int>((IEnumerable<int>) second1).Distinct<int>();
    List<int> second2 = new List<int>();
    // ISSUE: reference to a compiler-generated method
    foreach (UnitSkillCharacterQuest skillCharacterQuest in ((IEnumerable<UnitSkillCharacterQuest>) MasterData.UnitSkillCharacterQuestList).Where<UnitSkillCharacterQuest>(new Func<UnitSkillCharacterQuest, bool>(unit004CombinePage.\u003CsetSkillIcon\u003Eb__75_6)).ToArray<UnitSkillCharacterQuest>())
    {
      if (skillCharacterQuest.skill_after_evolution > 0)
        second2.Add(skillCharacterQuest.skill_after_evolution);
    }
    allSkillIDs = allSkillIDs.Concat<int>((IEnumerable<int>) second2).Distinct<int>();
    Dictionary<int, float> skillRatio = unit004CombinePage.SkillLevelUpRatio(unit004CombinePage.selectUnits, playerSkillDict);
    int skillIndex = 0;
    int len = unit004CombinePage.skillObject.Length;
    PlayerUnitSkills[] skills = ((IEnumerable<PlayerUnitSkills>) unit004CombinePage.baseUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type != BattleskillSkillType.magic && x.skill.skill_type != BattleskillSkillType.leader && !BattleskillSkill.InvestElementSkillIds.Contains(x.skill_id))).OrderBy<PlayerUnitSkills, int>((Func<PlayerUnitSkills, int>) (x => x.skill_id)).ToArray<PlayerUnitSkills>();
    List<PopupSkillDetails.Param> lstSkillParam = new List<PopupSkillDetails.Param>();
    PlayerUnitSkills unitSkill;
    BattleskillSkill skill;
    int currentLevel1;
    IEnumerator e;
    if (skillIndex < len - 1)
    {
      PlayerUnitSkills us = Array.Find<PlayerUnitSkills>(skills, (Predicate<PlayerUnitSkills>) (x => x.skill.skill_type == BattleskillSkillType.growth));
      if (us != null)
      {
        unitSkill = us;
        if (unitSkill != null && allSkillIDs.Any<int>((Func<int, bool>) (x => x == us.skill_id)))
        {
          skill = unit004CombinePage.baseUnit.evolutionSkill(unitSkill.skill);
          if (skill != null)
          {
            if (playerSkillDict.ContainsKey(skill.ID))
            {
              currentLevel1 = Math.Min(playerSkillDict[skill.ID], skill.upper_level);
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, currentLevel1, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Growth, new int?(currentLevel1)));
              if (currentLevel1 < skill.upper_level)
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[skill.ID]);
              else
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, 0.0f);
            }
            else
            {
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, 0, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[unitSkill.skill_id]);
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Growth));
            }
            ++skillIndex;
          }
          skill = (BattleskillSkill) null;
        }
        unitSkill = (PlayerUnitSkills) null;
      }
    }
    PlayerUnitSkills[] uss;
    int currentLevel2;
    if (skillIndex < len - 1)
    {
      uss = ((IEnumerable<PlayerUnitSkills>) skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type == BattleskillSkillType.duel)).ToArray<PlayerUnitSkills>();
      for (currentLevel1 = 0; currentLevel1 < uss.Length; ++currentLevel1)
      {
        PlayerUnitSkills us = uss[currentLevel1];
        unitSkill = us;
        if (us != null && allSkillIDs.Any<int>((Func<int, bool>) (x => x == us.skill_id)))
        {
          skill = unit004CombinePage.baseUnit.evolutionSkill(unitSkill.skill);
          if (skill != null)
          {
            if (playerSkillDict.ContainsKey(skill.ID))
            {
              currentLevel2 = Math.Min(playerSkillDict[skill.ID], skill.upper_level);
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, currentLevel2, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Duel, new int?(currentLevel2)));
              if (currentLevel2 < skill.upper_level)
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[skill.ID]);
              else
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, 0.0f);
            }
            else
            {
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, 0, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Duel));
              unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[unitSkill.skill_id]);
            }
            if (++skillIndex < len)
            {
              unitSkill = (PlayerUnitSkills) null;
              skill = (BattleskillSkill) null;
            }
            else
              break;
          }
        }
      }
      uss = (PlayerUnitSkills[]) null;
    }
    if (skillIndex < len - 1)
    {
      PlayerUnitSkills us = Array.Find<PlayerUnitSkills>(skills, (Predicate<PlayerUnitSkills>) (x => x.skill.skill_type == BattleskillSkillType.release));
      if (us != null)
      {
        unitSkill = us;
        if (unitSkill != null && allSkillIDs.Any<int>((Func<int, bool>) (x => x == us.skill_id)))
        {
          skill = unit004CombinePage.baseUnit.evolutionSkill(unitSkill.skill);
          if (skill != null)
          {
            if (playerSkillDict.ContainsKey(skill.ID))
            {
              currentLevel1 = Math.Min(playerSkillDict[skill.ID], skill.upper_level);
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, currentLevel1, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Release, new int?(currentLevel1)));
              if (currentLevel1 < skill.upper_level)
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[skill.ID]);
              else
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, 0.0f);
            }
            else
            {
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, 0, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Release));
              unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[unitSkill.skill_id]);
            }
            ++skillIndex;
          }
          skill = (BattleskillSkill) null;
        }
        unitSkill = (PlayerUnitSkills) null;
      }
    }
    if (skillIndex < len - 1)
    {
      uss = ((IEnumerable<PlayerUnitSkills>) skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type == BattleskillSkillType.command)).ToArray<PlayerUnitSkills>();
      for (currentLevel1 = 0; currentLevel1 < uss.Length; ++currentLevel1)
      {
        PlayerUnitSkills us = uss[currentLevel1];
        unitSkill = us;
        if (unitSkill != null && allSkillIDs.Any<int>((Func<int, bool>) (x => x == us.skill_id)))
        {
          skill = unit004CombinePage.baseUnit.evolutionSkill(unitSkill.skill);
          if (skill != null)
          {
            if (playerSkillDict.ContainsKey(skill.ID))
            {
              currentLevel2 = Math.Min(playerSkillDict[skill.ID], skill.upper_level);
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, currentLevel2, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Command, new int?(currentLevel2)));
              if (currentLevel2 < skill.upper_level)
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[skill.ID]);
              else
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, 0.0f);
            }
            else
            {
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, 0, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Command));
              unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[unitSkill.skill_id]);
            }
            if (++skillIndex < len)
            {
              unitSkill = (PlayerUnitSkills) null;
              skill = (BattleskillSkill) null;
            }
            else
              break;
          }
        }
      }
      uss = (PlayerUnitSkills[]) null;
    }
    if (skillIndex < len - 1)
    {
      uss = ((IEnumerable<PlayerUnitSkills>) skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (x => x.skill.skill_type == BattleskillSkillType.passive)).ToArray<PlayerUnitSkills>();
      for (currentLevel1 = 0; currentLevel1 < uss.Length; ++currentLevel1)
      {
        PlayerUnitSkills us = uss[currentLevel1];
        unitSkill = us;
        if (unitSkill != null && allSkillIDs.Any<int>((Func<int, bool>) (x => x == us.skill_id)))
        {
          skill = unit004CombinePage.baseUnit.evolutionSkill(unitSkill.skill);
          if (skill != null)
          {
            if (playerSkillDict.ContainsKey(skill.ID))
            {
              currentLevel2 = Math.Min(playerSkillDict[skill.ID], skill.upper_level);
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, currentLevel2, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Grant, new int?(currentLevel2)));
              if (currentLevel2 < skill.upper_level)
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[skill.ID]);
              else
                unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, 0.0f);
            }
            else
            {
              e = unit004CombinePage.CreateSkillIcon(skill, skillIndex, 0, unitSkill.level);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              lstSkillParam.Add(new PopupSkillDetails.Param(skill, UnitParameter.SkillGroup.Grant));
              unit004CombinePage.DisplaySkillLevelUpArrow(skillIndex, skillRatio[unitSkill.skill_id]);
            }
            if (++skillIndex < len)
            {
              unitSkill = (PlayerUnitSkills) null;
              skill = (BattleskillSkill) null;
            }
            else
              break;
          }
        }
      }
      uss = (PlayerUnitSkills[]) null;
    }
    unit004CombinePage.skillParams = lstSkillParam.ToArray();
    if (unit004CombinePage.dir_skill != null)
    {
      for (int index = 0; index < unit004CombinePage.dir_skill.Length; ++index)
        unit004CombinePage.dir_skill[index].SetActive(index < skillIndex);
    }
  }

  public void OnChangeUnit00486Scene()
  {
    if (this.IsPushAndSet())
      return;
    Unit00486Scene.changeScene(true, this.baseUnit, this.selectUnits, this.maxTrust_, this.exceptionBackScene);
  }

  private IEnumerator doCombine()
  {
    Unit004CombinePage unit004CombinePage = this;
    unit004CombinePage.endSequenceCombine();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    List<int> intList3 = new List<int>();
    List<int> openedEquippedGear3UnitIDs = new List<int>();
    HashSet<int> numberPlayerUnitIds = Singleton<NGGameDataManager>.GetInstance().opened_equip_number_player_unit_ids;
    for (int index = 0; index < unit004CombinePage.selectUnits.Length; ++index)
    {
      UnitUnit unit = unit004CombinePage.selectUnits[index].unit;
      if (unit.IsNormalUnit)
      {
        intList1.Add(unit004CombinePage.selectUnits[index].id);
        if (unit.same_character_id == unit004CombinePage.baseUnitUnit_.same_character_id && numberPlayerUnitIds.Contains(unit004CombinePage.selectUnits[index].id))
          openedEquippedGear3UnitIDs.Add(unit004CombinePage.selectUnits[index].id);
      }
      else
      {
        intList2.Add(unit004CombinePage.selectUnits[index].id);
        intList3.Add(unit004CombinePage.selectUnits[index].UnitIconInfo.SelectedCount);
      }
    }
    Singleton<PopupManager>.GetInstance().monitorCoroutine(unit004CombinePage.doLoadResult());
    int base_id = unit004CombinePage.baseUnit.id;
    Future<WebAPI.Response.UnitCompose> paramF = WebAPI.UnitCompose(base_id, intList2.ToArray(), intList3.ToArray(), intList1.ToArray(), (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      WebAPI.DefaultUserErrorCallback(error);
      Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
    }));
    yield return (object) paramF.Wait();
    if (paramF.Result != null)
    {
      WebAPI.Response.UnitCompose result = paramF.Result;
      Singleton<NGGameDataManager>.GetInstance().clearPreviewInheritance();
      Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) result.corps_player_unit_ids);
      PlayerUnit resultUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == base_id));
      if (openedEquippedGear3UnitIDs.Any<int>())
      {
        Singleton<NGGameDataManager>.GetInstance().Remove_opened_equip_number_player_unit_ids((IEnumerable<int>) openedEquippedGear3UnitIDs);
        Singleton<NGGameDataManager>.GetInstance().Add_opened_equip_number_player_unit_ids(base_id);
      }
      yield return (object) unit004CombinePage.doShowResult(resultUnit, unit004CombinePage.setShowPopupData(new List<PlayerUnit>()
      {
        unit004CombinePage.baseUnit,
        resultUnit
      }, result), new List<int>()
      {
        Convert.ToInt32(result.is_success),
        result.increment_medal,
        result.gain_trust_result.is_equip_awake_skill_release ? 1 : 0,
        result.gain_trust_result.has_new_player_awake_skill ? 1 : 0
      });
      unit004CombinePage.IsPush = false;
    }
  }

  private Dictionary<string, object> setShowPopupData(
    List<PlayerUnit> resultList,
    WebAPI.Response.UnitCompose param)
  {
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    dictionary["unlockQuests"] = (object) param.unlock_quests;
    Func<List<int>, List<int>, int> func = (Func<List<int>, List<int>, int>) ((list1, list2) =>
    {
      int num1 = 0;
      foreach (int num2 in list1)
      {
        if (!list2.Contains(num2))
          num1 = num2;
      }
      return num1;
    });
    List<int> list3 = ((IEnumerable<PlayerUnitSkills>) resultList[0].GetAcquireSkills()).Select<PlayerUnitSkills, int>((Func<PlayerUnitSkills, int>) (x => x.skill_id)).ToList<int>();
    List<int> list4 = ((IEnumerable<PlayerUnitSkills>) resultList[1].GetAcquireSkills()).Select<PlayerUnitSkills, int>((Func<PlayerUnitSkills, int>) (x => x.skill_id)).ToList<int>();
    dictionary["beforeSkillId"] = (object) func(list3, list4);
    dictionary["afterSkillId"] = (object) func(list4, list3);
    return dictionary;
  }

  private void changeSceneForCombine(
    List<PlayerUnit> materialList,
    List<PlayerUnit> resultList,
    List<int> otherList,
    Dictionary<string, object> showPopupData)
  {
    this.setBackSceneFromResult(resultList[1]);
    unit004812Scene.changeScene(true, materialList, resultList, otherList, showPopupData, Unit00468Scene.Mode.Unit0048);
  }

  public void IbtnCombine()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.IbtnCombineAsync());
  }

  private IEnumerator IbtnCombineAsync()
  {
    Unit004CombinePage unit004CombinePage = this;
    unit004CombinePage.beginSequenceCombine();
    UnitUnit unitUnit = unit004CombinePage.checkAwaking() ?? unit004CombinePage.baseUnitUnit_;
    if (unit004CombinePage.baseUnit.tower_is_entry && unit004CombinePage.baseUnitUnit_ != unitUnit || ((IEnumerable<PlayerUnit>) unit004CombinePage.selectUnits).Any<PlayerUnit>((Func<PlayerUnit, bool>) (unit => unit.tower_is_entry || unit.corps_is_entry)))
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected));
      if (isRejected)
      {
        unit004CombinePage.cancelCombineAsync();
        yield break;
      }
    }
    bool isRarity = ((IEnumerable<PlayerUnit>) unit004CombinePage.selectUnits).Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.rarity.index >= 2));
    bool isMemoryAlert = false;
    if (PlayerTransmigrateMemoryPlayerUnitIds.Current != null)
    {
      int?[] memoryPlayerUnitIds = PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids;
      foreach (PlayerUnit selectUnit in unit004CombinePage.selectUnits)
      {
        PlayerUnit unit = selectUnit;
        if (unit != (PlayerUnit) null && !isMemoryAlert)
        {
          isMemoryAlert = ((IEnumerable<int?>) memoryPlayerUnitIds).Any<int?>((Func<int?, bool>) (x =>
          {
            if (!x.HasValue)
              return false;
            int? nullable = x;
            int id = unit.id;
            return nullable.GetValueOrDefault() == id & nullable.HasValue;
          }));
          if (isMemoryAlert)
            break;
        }
      }
    }
    bool isAlertOverkillersSlot = ((IEnumerable<PlayerUnit>) unit004CombinePage.selectUnits).Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.isReleasedOverkillersSlot(0)));
    Consts consts = Consts.GetInstance();
    if (unit004CombinePage.isMaterialOverUsed)
    {
      bool bCancel = false;
      bool bWait = true;
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupCommon.Show(consts.POPUP_00484_ALERT_TITLE, consts.POPUP_00484_MATERIAL_OVER_USE, (Action) (() =>
      {
        bCancel = true;
        bWait = false;
      })));
      while (bWait)
        yield return (object) null;
      if (bCancel)
      {
        unit004CombinePage.cancelCombineAsync();
        yield break;
      }
    }
    if (unit004CombinePage.getTrustUpValue(unit004CombinePage.duplicationSelectUnits) > 0 && ((IEnumerable<PlayerUnit>) unit004CombinePage.duplicationSelectUnits).Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => (double) x.trust_rate >= (double) consts.TRUST_RATE_LEVEL_SIZE)))
    {
      bool bCancel = false;
      bool bWait = true;
      int num1 = Mathf.FloorToInt(unit004CombinePage.trust_ / consts.TRUST_RATE_LEVEL_SIZE);
      int num2 = 0 + Mathf.FloorToInt(unit004CombinePage.baseUnit.trust_rate / consts.TRUST_RATE_LEVEL_SIZE);
      for (int index = 0; index < unit004CombinePage.selectUnits.Length; ++index)
      {
        if (!unit004CombinePage.selectUnits[index].unit.IsTrustMaterial(unit004CombinePage.baseUnit))
          num2 += Mathf.FloorToInt(unit004CombinePage.selectUnits[index].trust_rate / consts.TRUST_RATE_LEVEL_SIZE);
      }
      int num3 = num1 - num2;
      int num4 = Mathf.CeilToInt(unit004CombinePage.maxTrust_ / consts.TRUST_RATE_LEVEL_SIZE) - Mathf.FloorToInt(unit004CombinePage.trust_ / consts.TRUST_RATE_LEVEL_SIZE);
      PopupCommonNoYes.Show(Consts.GetInstance().POPUP_00484_ALERT_TITLE, Consts.Format(Consts.GetInstance().POPUP_00484_SKILL_ACQUISITION_STATUS_AFTER_COMBINATION, (IDictionary) new Hashtable()
      {
        {
          (object) "count",
          (object) num3
        },
        {
          (object) "remaining",
          (object) num4
        }
      }), (Action) (() => bWait = false), (Action) (() =>
      {
        bCancel = true;
        bWait = false;
      }));
      while (bWait)
        yield return (object) null;
      if (bCancel)
      {
        unit004CombinePage.cancelCombineAsync();
        yield break;
      }
    }
    if (isRarity | isMemoryAlert | isAlertOverkillersSlot)
    {
      Consts instance = Consts.GetInstance();
      NGUIText.Alignment alignment = (NGUIText.Alignment) 2;
      string messageB = (string) null;
      NGUIText.Alignment alignmentB = (NGUIText.Alignment) 1;
      string message;
      if (isRarity)
      {
        message = instance.POPUP_00484_ALERT_RARITY;
        messageB = isMemoryAlert ? "\n" + instance.POPUP_00484_ALERT_MEMORY : string.Empty;
        if (isAlertOverkillersSlot)
          messageB += instance.POPUP_00484_ALERT_OVERKILLERS_SLOTS;
      }
      else
      {
        message = isMemoryAlert ? instance.POPUP_00484_ALERT_MEMORY : string.Empty;
        alignment = (NGUIText.Alignment) 1;
        if (isAlertOverkillersSlot)
          message += instance.POPUP_00484_ALERT_OVERKILLERS_SLOTS;
      }
      PopupCommonNoYes.Show(instance.POPUP_00484_ALERT_TITLE, message, new Action(unit004CombinePage.combine), new Action(unit004CombinePage.cancelCombineAsync), alignment, messageB, alignmentB, true);
    }
    else
      yield return (object) unit004CombinePage.doCombine();
  }

  private void combine()
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.doCombine());
  }

  private void beginSequenceCombine()
  {
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false);
  }

  private void endSequenceCombine()
  {
    if (!Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
  }

  private void cancelCombineAsync()
  {
    this.IsPush = false;
    this.endSequenceCombine();
  }

  public void onIbtnChangeUpperParameter()
  {
    foreach (Component component1 in ((Component) this.grid).transform)
    {
      SozaiItem component2 = component1.GetComponent<SozaiItem>();
      if (!Object.op_Equality((Object) component2.UnitIcon, (Object) null))
      {
        component2.UnitIcon.Dir_select_check.SetActive(component2.DirSozaiBase.activeSelf);
        component2.DirSozaiBase.SetActive(!component2.DirSozaiBase.activeSelf);
      }
    }
  }

  public void onClickedUnityValue()
  {
    Action<GameObject[], NGMenuBase> popupUnityDetail = this.popupUnityDetail;
    if (popupUnityDetail == null)
      return;
    popupUnityDetail(this.unityDetailPrefabs, (NGMenuBase) null);
  }

  private IEnumerator doLoadResult()
  {
    Unit004CombinePage unit004CombinePage = this;
    Future<GameObject> ld;
    if (Object.op_Equality((Object) unit004CombinePage.resultBackground_, (Object) null))
    {
      ld = Res.Prefabs.BackGround.UnitBackground_60.Load<GameObject>();
      yield return (object) ld.Wait();
      unit004CombinePage.resultBackground_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004CombinePage.resultMain_, (Object) null))
    {
      ld = new ResourceObject("Animations/Unit_Integration/dir_Unit_Integration").Load<GameObject>();
      yield return (object) ld.Wait();
      unit004CombinePage.resultMain_ = ld.Result.Clone(((Component) unit004CombinePage.mainMenu_).transform).GetComponent<Unit004813Menu>();
      unit004CombinePage.resultMain_.mode = Unit00468Scene.Mode.Unit0048;
      yield return (object) null;
      ((Component) unit004CombinePage.resultMain_).gameObject.SetActive(false);
      ld = (Future<GameObject>) null;
    }
  }

  private IEnumerator doShowResult(
    PlayerUnit resultUnit,
    Dictionary<string, object> showPopup,
    List<int> otherList)
  {
    Unit004CombinePage unit004CombinePage = this;
    while (Object.op_Equality((Object) unit004CombinePage.resultBackground_, (Object) null) || Object.op_Equality((Object) unit004CombinePage.resultMain_, (Object) null))
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    unit004CombinePage.mainMenu_.mainPanel.SetActive(false);
    Singleton<CommonRoot>.GetInstance().setBackground(unit004CombinePage.resultBackground_);
    bool bWait = true;
    unit004CombinePage.resultMain_.onFinished = (Action) (() => bWait = false);
    unit004CombinePage.resultMain_.showPopupData = showPopup;
    yield return (object) unit004CombinePage.resultMain_.setCharacter(unit004CombinePage.baseUnit, resultUnit, otherList);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    ((Component) unit004CombinePage.resultMain_).gameObject.SetActive(true);
    while (bWait)
      yield return (object) null;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    unit004CombinePage.mainMenu_.mainPanel.SetActive(true);
    ((Component) unit004CombinePage.resultMain_).gameObject.SetActive(false);
    unit004CombinePage.resultMain_.IsPush = false;
    Singleton<CommonRoot>.GetInstance().setBackground(Singleton<NGSceneManager>.GetInstance().sceneBase.backgroundPrefab);
    yield return (object) unit004CombinePage.mainMenu_.doReset(new Ingredients(TrainingType.Combine)
    {
      baseUnit = resultUnit
    });
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
  }
}
