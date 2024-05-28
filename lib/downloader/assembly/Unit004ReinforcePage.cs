// Decompiled with JetBrains decompiler
// Type: Unit004ReinforcePage
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
using UnitTraining;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/Training/ReinforcePage")]
public class Unit004ReinforcePage : Unit004TrainingPage
{
  [SerializeField]
  [Tooltip("素材表示枠下地の最低表示行数")]
  private int tilingLine_ = 5;
  [Header("ベース姫情報(dir_Status01)")]
  [SerializeField]
  private GameObject dynTumn;
  [SerializeField]
  private UILabel TxtHimeEnforceNumer;
  [SerializeField]
  private UILabel TxtHimeEnforceDenom;
  [SerializeField]
  private UILabel TxtHimeEnforceMaxNumer;
  [SerializeField]
  private UIButton ibtnReset;
  [SerializeField]
  private GameObject[] StatusGaugeBases;
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
  private UIButton ibtnReinforceAnimFade01;
  private GameObject rarityAlertPopupPrefab;
  private GameObject resetAlertPopupPrefab;
  private GameObject upperParameterPrefab;
  private GameObject upperParameterLabelPrefab;
  private GameObject skillTypeIconPrefab;
  private GameObject statusGaugePrefab;
  private GameObject DirSozaiItemPrefab;
  private UnitUnit baseUnitUnit_;
  private PlayerUnit[] selectUnits;
  private PlayerUnit[] duplicationSelectUnits;

  public override TrainingType page => TrainingType.Reinforce;

  protected override IEnumerator doInitialize()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReinforcePage unit004ReinforcePage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit004ReinforcePage.isResetBase_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unit004ReinforcePage.isResetBase_ = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004ReinforcePage.Init();
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
    Unit004ReinforcePage unit004ReinforcePage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit004ReinforcePage.isResetBase_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unit004ReinforcePage.isResetBase_ = modifiedBase;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004ReinforcePage.Init();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator doReset(PlayerUnit newBase)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004ReinforcePage unit004ReinforcePage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004ReinforcePage.mainMenu_.doReset(new Ingredients(TrainingType.Reinforce)
    {
      baseUnit = newBase
    });
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
    Unit004ReinforcePage unit004ReinforcePage = this;
    while (Singleton<CommonRoot>.GetInstance().isLoading)
      yield return (object) null;
    unit004ReinforcePage.showAdvice("unit004_20");
  }

  private PlayerUnit baseUnit => this.target_.baseUnit;

  public IEnumerator Init()
  {
    Unit004ReinforcePage unit004ReinforcePage = this;
    unit004ReinforcePage.baseUnitUnit_ = unit004ReinforcePage.baseUnit.unit;
    PlayerUnit[] materialPlayerUnits = unit004ReinforcePage.target_.materialPlayerUnits ?? new PlayerUnit[0];
    unit004ReinforcePage.selectUnits = ((IEnumerable<PlayerUnit>) materialPlayerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null)).ToArray<PlayerUnit>();
    List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
    foreach (PlayerUnit selectUnit in unit004ReinforcePage.selectUnits)
    {
      for (int index = 0; index < selectUnit.UnitIconInfo.SelectedCount; ++index)
        playerUnitList.Add(selectUnit);
    }
    PlayerUnit[] array = playerUnitList.ToArray();
    bool flag = unit004ReinforcePage.isResetBase_;
    if (!flag && unit004ReinforcePage.duplicationSelectUnits != null && unit004ReinforcePage.duplicationSelectUnits.Length == array.Length)
    {
      for (int index = 0; index < unit004ReinforcePage.duplicationSelectUnits.Length; ++index)
      {
        PlayerUnit duplicationSelectUnit = unit004ReinforcePage.duplicationSelectUnits[index];
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
    unit004ReinforcePage.duplicationSelectUnits = array;
    while (unit004ReinforcePage.isLoadingResources)
      yield return (object) null;
    yield return (object) unit004ReinforcePage.Show_dir_Status01();
    unit004ReinforcePage.scrollView.ClearBounds();
    ((Component) unit004ReinforcePage.grid).transform.Clear();
    int num = unit004ReinforcePage.grid.maxPerLine * unit004ReinforcePage.tilingLine_;
    if (unit004ReinforcePage.selectUnits.Length == 0 || ((IEnumerable<PlayerUnit>) unit004ReinforcePage.selectUnits).Count<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.UnitIconInfo.SelectedCount > 0)) <= 0)
    {
      ((Behaviour) unit004ReinforcePage.uiDragScrollView).enabled = false;
      unit004ReinforcePage.dir_attention.SetActive(true);
      unit004ReinforcePage.scrollBar.SetActive(false);
      unit004ReinforcePage.ibtn_change_UpperParameter.SetActive(false);
      for (int index = 0; index < num; ++index)
        unit004ReinforcePage.DirSozaiItemPrefab.CloneAndGetComponent<SozaiItem>(((Component) unit004ReinforcePage.grid).transform).SetOnlyWLine();
    }
    else
    {
      ((Behaviour) unit004ReinforcePage.uiDragScrollView).enabled = true;
      unit004ReinforcePage.dir_attention.SetActive(false);
      unit004ReinforcePage.scrollBar.SetActive(true);
      unit004ReinforcePage.ibtn_change_UpperParameter.SetActive(true);
      int numTile = Mathf.Max((unit004ReinforcePage.selectUnits.Length + unit004ReinforcePage.grid.maxPerLine - 1) / unit004ReinforcePage.grid.maxPerLine * unit004ReinforcePage.grid.maxPerLine, num);
      List<UIWidget> widgetList = new List<UIWidget>();
      for (int i = 0; i < numTile; ++i)
      {
        SozaiItem component1 = unit004ReinforcePage.DirSozaiItemPrefab.CloneAndGetComponent<SozaiItem>(((Component) unit004ReinforcePage.grid).transform);
        if (i >= unit004ReinforcePage.selectUnits.Length)
        {
          component1.SetOnlyWLine();
        }
        else
        {
          PlayerUnit materialPlayerUnit = unit004ReinforcePage.selectUnits[i];
          if (materialPlayerUnit.UnitIconInfo.SelectedCount <= 0)
          {
            component1.SetOnlyWLine();
          }
          else
          {
            UpperParameter component2 = unit004ReinforcePage.upperParameterPrefab.CloneAndGetComponent<UpperParameter>(component1.DirSozaiBase.transform);
            component2.Init(materialPlayerUnit);
            widgetList.AddRange((IEnumerable<UIWidget>) ((Component) component2).gameObject.GetComponentsInChildren<UIWidget>());
            UpperParameterLabel component3 = unit004ReinforcePage.upperParameterLabelPrefab.CloneAndGetComponent<UpperParameterLabel>(component1.DirSozaiBase2.transform);
            component3.Init(unit004ReinforcePage.baseUnit, materialPlayerUnit);
            widgetList.AddRange((IEnumerable<UIWidget>) ((Component) component3).gameObject.GetComponentsInChildren<UIWidget>());
            UnitIcon unitIcon = unit004ReinforcePage.unitIconPrefab_.CloneAndGetComponent<UnitIcon>(component1.LinkSozaiBase.transform);
            component1.UnitIcon = unitIcon;
            // ISSUE: reference to a compiler-generated method
            unitIcon.onClick = new Action<UnitIconBase>(unit004ReinforcePage.\u003CInit\u003Eb__34_3);
            unitIcon.EnabledExpireDate = true;
            PlayerMaterialUnit playerMaterialUnit = Array.Find<PlayerMaterialUnit>(SMManager.Get<PlayerMaterialUnit[]>(), (Predicate<PlayerMaterialUnit>) (x => x._unit == materialPlayerUnit.unit.ID));
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
            IEnumerator e = unitIcon.SetMaterialUnit(materialPlayerUnit, false, materialPlayerUnits);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
            unitIcon.SelectByCheckAndNumber(materialPlayerUnit.UnitIconInfo);
            unitIcon.Gray = false;
            UISprite component4 = unitIcon.Dir_select_check.GetComponent<UISprite>();
            if (Object.op_Inequality((Object) component4, (Object) null))
              ((Behaviour) component4).enabled = false;
            unitIcon = (UnitIcon) null;
          }
        }
      }
      unit004ReinforcePage.scrollView.AddBounds((IEnumerable<UIWidget>) widgetList);
      widgetList = (List<UIWidget>) null;
    }
    // ISSUE: method pointer
    unit004ReinforcePage.grid.onReposition = new UIGrid.OnReposition((object) unit004ReinforcePage, __methodptr(\u003CInit\u003Eb__34_2));
    unit004ReinforcePage.grid.Reposition();
    unit004ReinforcePage.grid.repositionNow = true;
    unit004ReinforcePage.SetWidthLine();
    unit004ReinforcePage.onIbtnChangeUpperParameter();
    unit004ReinforcePage.setCostZeny(CalcUnitCompose.priceStringth(unit004ReinforcePage.baseUnit, unit004ReinforcePage.duplicationSelectUnits));
    ((UIButtonColor) unit004ReinforcePage.ibtnReinforceAnimFade01).isEnabled = false;
    if (unit004ReinforcePage.duplicationSelectUnits.Length != 0)
      ((UIButtonColor) unit004ReinforcePage.ibtnReinforceAnimFade01).isEnabled = true;
  }

  protected override IEnumerator loadResources()
  {
    Unit004ReinforcePage unit004ReinforcePage = this;
    yield return (object) unit004ReinforcePage.doLoadCommonPrefab();
    Future<GameObject> popupPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) unit004ReinforcePage.rarityAlertPopupPrefab, (Object) null))
    {
      popupPrefabF = Res.Prefabs.popup.popup_004_8_3_2__anim_popup02.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004ReinforcePage.rarityAlertPopupPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004ReinforcePage.resetAlertPopupPrefab, (Object) null))
    {
      popupPrefabF = Res.Prefabs.popup.popup_004_unit_reinforce_reset__anim_popup01.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004ReinforcePage.resetAlertPopupPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004ReinforcePage.upperParameterPrefab, (Object) null))
    {
      popupPrefabF = new ResourceObject("Prefabs/UnitIcon/UpperParameter").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004ReinforcePage.upperParameterPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004ReinforcePage.upperParameterLabelPrefab, (Object) null))
    {
      popupPrefabF = new ResourceObject("Prefabs/UnitIcon/UpperParameterLabel").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004ReinforcePage.upperParameterLabelPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004ReinforcePage.skillTypeIconPrefab, (Object) null))
    {
      popupPrefabF = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004ReinforcePage.skillTypeIconPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004ReinforcePage.statusGaugePrefab, (Object) null))
    {
      popupPrefabF = Res.Prefabs.unit004_20.StatusGauge.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004ReinforcePage.statusGaugePrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit004ReinforcePage.DirSozaiItemPrefab, (Object) null))
    {
      popupPrefabF = new ResourceObject("Prefabs/unit004_8_4/dir_Sozai_Item").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit004ReinforcePage.DirSozaiItemPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
  }

  private IEnumerator Show_dir_Status01()
  {
    Unit004ReinforcePage unit004ReinforcePage = this;
    if (unit004ReinforcePage.isResetBase_)
      yield return (object) unit004ReinforcePage.initUnitIcon(unit004ReinforcePage.dynTumn, unit004ReinforcePage.baseUnit, true, true);
    unit004ReinforcePage.TxtHimeEnforceNumer.SetTextLocalize(unit004ReinforcePage.baseUnit.buildup_count + ((IEnumerable<PlayerUnit>) unit004ReinforcePage.selectUnits).Sum<PlayerUnit>((Func<PlayerUnit, int>) (x => x.UnitIconInfo.SelectedCount)));
    ((UIWidget) unit004ReinforcePage.TxtHimeEnforceNumer).color = unit004ReinforcePage.selectUnits.Length == 0 ? Color.white : Color.yellow;
    unit004ReinforcePage.TxtHimeEnforceDenom.SetTextLocalize(unit004ReinforcePage.baseUnit.buildup_limit);
    unit004ReinforcePage.TxtHimeEnforceMaxNumer.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT_00420_ENFORCE_MAX_NUMER, (IDictionary) new Hashtable()
    {
      {
        (object) "max",
        (object) unit004ReinforcePage.baseUnit.buildupLimitBreakCnt
      }
    }));
    unit004ReinforcePage.setStatusGauge(unit004ReinforcePage.statusGaugePrefab, unit004ReinforcePage.duplicationSelectUnits);
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

  private void setStatusGauge(GameObject prefabGauge, PlayerUnit[] materialPlayerUnits)
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
      this.baseUnit.self_total_hp,
      this.baseUnit.self_total_strength,
      this.baseUnit.self_total_intelligence,
      this.baseUnit.self_total_vitality,
      this.baseUnit.self_total_mind,
      this.baseUnit.self_total_agility,
      this.baseUnit.self_total_dexterity,
      this.baseUnit.self_total_lucky
    };
    int[] numArray2 = new int[8]
    {
      this.baseUnit.hp.level_up_max_status,
      this.baseUnit.strength.level_up_max_status,
      this.baseUnit.intelligence.level_up_max_status,
      this.baseUnit.vitality.level_up_max_status,
      this.baseUnit.mind.level_up_max_status,
      this.baseUnit.agility.level_up_max_status,
      this.baseUnit.dexterity.level_up_max_status,
      this.baseUnit.lucky.level_up_max_status
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
      this.baseUnit.hp.level,
      this.baseUnit.strength.level,
      this.baseUnit.intelligence.level,
      this.baseUnit.vitality.level,
      this.baseUnit.mind.level,
      this.baseUnit.agility.level,
      this.baseUnit.dexterity.level,
      this.baseUnit.lucky.level
    };
    int[] numArray5 = new int[8]
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
    int[] numArray6 = new int[8]
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
    int[] numArray7 = new int[8]
    {
      this.baseUnit.hp.buildupMaxCnt(this.baseUnit),
      this.baseUnit.strength.buildupMaxCnt(this.baseUnit),
      this.baseUnit.intelligence.buildupMaxCnt(this.baseUnit),
      this.baseUnit.vitality.buildupMaxCnt(this.baseUnit),
      this.baseUnit.mind.buildupMaxCnt(this.baseUnit),
      this.baseUnit.agility.buildupMaxCnt(this.baseUnit),
      this.baseUnit.dexterity.buildupMaxCnt(this.baseUnit),
      this.baseUnit.lucky.buildupMaxCnt(this.baseUnit)
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
    }).Max();
    int length = this.StatusGaugeBases.Length;
    for (int index = 0; index < length; ++index)
    {
      GameObject statusGaugeBase = this.StatusGaugeBases[index];
      statusGaugeBase.transform.Clear();
      prefabGauge.CloneAndGetComponent<Unit00420StatusGauge>(statusGaugeBase).Init(this.baseUnit, materialPlayerUnits, composeTypeArray[index], numArray1[index], numArray2[index], numArray3[index], numArray4[index], numArray5[index], numArray6[index], numArray7[index], flagArray[index], gaugeMax);
      NGTween.playTweens(NGTween.findTweenersAll(statusGaugeBase, false), 0);
    }
  }

  public void OnChangeUnit00487Scene()
  {
    if (this.IsPushAndSet())
      return;
    Unit00487Scene.changeScene(true, this.baseUnit, this.selectUnits, this.exceptionBackScene);
  }

  public void IbtnReset()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowResetAlertPopup());
  }

  private IEnumerator ShowResetAlertPopup()
  {
    Unit004ReinforcePage bMenu = this;
    PlayerMaterialUnit rUnit = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x.unit.is_buildup_only == 2));
    GameObject popup = bMenu.resetAlertPopupPrefab.Clone();
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Unit00420ResetPopup>().Init(bMenu, bMenu.baseUnit, rUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    popup.SetActive(true);
  }

  public void IbtnEnforce()
  {
    if (this.IsPushAndSet())
      return;
    bool isAlert = ((IEnumerable<PlayerUnit>) this.selectUnits).Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.rarity.index >= 2));
    bool isMemoryAlert = false;
    int?[] source = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.transmigrate_memory_player_unit_ids : new int?[0];
    foreach (PlayerUnit selectUnit in this.selectUnits)
    {
      PlayerUnit unit = selectUnit;
      if (unit != (PlayerUnit) null && !isMemoryAlert)
      {
        isMemoryAlert = ((IEnumerable<int?>) source).Any<int?>((Func<int?, bool>) (x =>
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
    if (isAlert | isMemoryAlert)
      Singleton<PopupManager>.GetInstance().open(this.rarityAlertPopupPrefab).GetComponent<Unit004832Menu>().Init(isAlert, isMemoryAlert, new Action(this.executeReinforce));
    else
      Debug.LogError((object) "Unit00420Menu: 強化素材が想定しているレアリティではありません");
  }

  private void executeReinforce()
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.doReinfoce());
  }

  private IEnumerator doReinfoce()
  {
    Unit004ReinforcePage unit004ReinforcePage = this;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      int[] material_player_material_unit_ids = new int[unit004ReinforcePage.selectUnits.Length];
      int[] material_player_material_unit_nums = new int[unit004ReinforcePage.selectUnits.Length];
      for (int index = 0; index < unit004ReinforcePage.selectUnits.Length; ++index)
      {
        material_player_material_unit_ids[index] = unit004ReinforcePage.selectUnits[index].id;
        material_player_material_unit_nums[index] = unit004ReinforcePage.selectUnits[index].UnitIconInfo.SelectedCount;
      }
      Future<WebAPI.Response.UnitBuildup> paramF = WebAPI.UnitBuildup(unit004ReinforcePage.baseUnit.id, material_player_material_unit_ids, material_player_material_unit_nums, (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        WebAPI.DefaultUserErrorCallback(error);
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
      }));
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (paramF.Result != null)
      {
        WebAPI.Response.UnitBuildup result1 = paramF.Result;
        // ISSUE: reference to a compiler-generated method
        PlayerUnit result2 = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), new Predicate<PlayerUnit>(unit004ReinforcePage.\u003CdoReinfoce\u003Eb__45_1));
        List<PlayerUnit> num_list = new List<PlayerUnit>();
        num_list.AddRange((IEnumerable<PlayerUnit>) unit004ReinforcePage.selectUnits);
        List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
        playerUnitList.Add(unit004ReinforcePage.baseUnit);
        playerUnitList.Add(result2);
        List<int> other_list = new List<int>();
        other_list.Add(1);
        other_list.Add(result1.increment_medal);
        other_list.Add(0);
        other_list.Add(0);
        Dictionary<string, object> showPopupData = unit004ReinforcePage.setShowPopupData(playerUnitList, result1);
        unit004ReinforcePage.setBackSceneFromResult(result2);
        unit004812Scene.changeScene(true, num_list, playerUnitList, other_list, showPopupData, Unit00468Scene.Mode.Unit00420);
        paramF = (Future<WebAPI.Response.UnitBuildup>) null;
      }
    }
  }

  private Dictionary<string, object> setShowPopupData(
    List<PlayerUnit> resultList,
    WebAPI.Response.UnitBuildup param)
  {
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    dictionary["unlockQuests"] = (object) new UnlockQuest[0];
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
}
