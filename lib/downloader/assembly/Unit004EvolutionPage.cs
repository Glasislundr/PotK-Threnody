// Decompiled with JetBrains decompiler
// Type: Unit004EvolutionPage
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
[AddComponentMenu("Scenes/Unit/Training/EvolutionPage")]
public class Unit004EvolutionPage : Unit004TrainingPage
{
  private UnitUnit baseUnitUnit_;
  [SerializeField]
  private Unit004TrainingUnitStatus beforeEvolution_;
  [SerializeField]
  private Unit004TrainingUnitStatus afterEvolution_;
  [SerializeField]
  private Unit004TrainingUnitStatus beforeAwake_;
  [SerializeField]
  private Unit004TrainingUnitStatus afterAwake_;
  private Unit004TrainingUnitStatus beforeStatus_;
  private Unit004TrainingUnitStatus afterStatus_;
  [SerializeField]
  private UILabel txtTitle_;
  [SerializeField]
  private GameObject dirEvolutionMaterials_;
  [SerializeField]
  private GameObject dialogBox_;
  [SerializeField]
  private UILabel txtMaterialName_;
  [SerializeField]
  private UILabel txtMaterialPlace_;
  [SerializeField]
  [Tooltip("進化不可理由Top")]
  private GameObject comShortage_;
  [SerializeField]
  [Tooltip("進化不可理由表示:資金不足/素材不足/Level不足/お気に入り設定/淘汰値不足/進化限界、の順に配置")]
  private GameObject[] comShortages_;
  [SerializeField]
  private UILabel txtShortageLevel_;
  [SerializeField]
  private GameObject[] lnkEvolutionUnits_;
  [SerializeField]
  private UIButton evolutionBtn_;
  [SerializeField]
  private UIButton awakeBtn_;
  [SerializeField]
  private UILabel unityValueShortage;
  private List<int> materialUnitIds_;
  private List<int> materialMaterialUnitIds_;
  private int currentEvolutionPatternId_;
  private PlayerUnit[] afterUnits_;
  private Unit004TrainingPage.ErrorFlag errFlags_;
  private EvolutionType evolutionType_;
  private UnitEvolutionPattern[] evolutionPatterns_;
  private static readonly string ALERT_COLOR = "[ffff00]";
  private Dictionary<int, Unit004EvolutionPage.EvolutionSelectMap> dicSelector_;
  public NGHorizontalScrollParts indicator_;
  private GameObject indicatorPrefab_;
  private bool isMovingIndicator_;
  private float positionIndicator_;
  private bool realityEvolutionButton_;
  protected Dictionary<int, UnitUnit[]> evolutionMaterialDict_;
  protected Dictionary<int, GameObject[]> linkEvolutionUnitsDict_;
  private const int SINGLE_PATTERN_EVOLUTION = 1;

  public override TrainingType page => TrainingType.Evolution;

  protected override IEnumerator doInitialize()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004EvolutionPage unit004EvolutionPage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit004EvolutionPage.isWaitInitalize_ = false;
      unit004EvolutionPage.isResetBase_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unit004EvolutionPage.isResetBase_ = true;
    unit004EvolutionPage.isWaitInitalize_ = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004EvolutionPage.Init();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected override IEnumerator loadResources()
  {
    Unit004EvolutionPage unit004EvolutionPage = this;
    yield return (object) unit004EvolutionPage.doLoadCommonPrefab();
    if (Object.op_Equality((Object) unit004EvolutionPage.indicatorPrefab_, (Object) null))
    {
      Future<GameObject> indicatorPrefabF = new ResourceObject("Prefabs/unit004_training/dir_UnitEvolution").Load<GameObject>();
      yield return (object) indicatorPrefabF.Wait();
      unit004EvolutionPage.indicatorPrefab_ = indicatorPrefabF.Result;
      indicatorPrefabF = (Future<GameObject>) null;
    }
  }

  protected override void preChangeTarget(Ingredients targetNext, bool bModifiedBase)
  {
  }

  protected override IEnumerator doChange(bool modifiedBase)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004EvolutionPage unit004EvolutionPage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit004EvolutionPage.isWaitInitalize_ = false;
      unit004EvolutionPage.isResetBase_ = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    unit004EvolutionPage.isResetBase_ = modifiedBase;
    unit004EvolutionPage.isWaitInitalize_ = true;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004EvolutionPage.Init();
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
    Unit004EvolutionPage unit004EvolutionPage = this;
    while (Singleton<CommonRoot>.GetInstance().isLoading)
      yield return (object) null;
    string name = unit004EvolutionPage.evolutionType_ == EvolutionType.Normal ? "unit004_9_9_evo" : string.Empty;
    unit004EvolutionPage.showAdvice(name);
  }

  protected override bool isDisabledAfterUnitIconButton
  {
    get => this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Limit);
  }

  private PlayerUnit baseUnit => this.target_.baseUnit;

  private GvgDeckCostOver gvgDeckCostOver
  {
    get
    {
      WebAPI.Response.UnitEvolutionParameter estimatesEvolution = this.target_.estimatesEvolution;
      return estimatesEvolution == null ? GvgDeckCostOver.None : (GvgDeckCostOver) estimatesEvolution.gvg_deck_cost_over_status;
    }
  }

  private IEnumerator Init()
  {
    Unit004EvolutionPage unit004EvolutionPage = this;
    if (unit004EvolutionPage.isResetBase_)
    {
      unit004EvolutionPage.baseUnitUnit_ = unit004EvolutionPage.baseUnit.unit;
      List<UnitEvolutionPattern> result1 = new List<UnitEvolutionPattern>();
      unit004EvolutionPage.evolutionType_ = UnitUtil.getEvolutionType(unit004EvolutionPage.baseUnitUnit_, ref result1);
      unit004EvolutionPage.evolutionPatterns_ = result1.ToArray();
      int num = (int) unit004EvolutionPage.errFlags_.Reset(unit004EvolutionPage.evolutionType_ != EvolutionType.Limit ? Unit004TrainingPage.ErrorFlag.Clear : Unit004TrainingPage.ErrorFlag.Limit);
      List<GameObject> selfs1 = new List<GameObject>(8)
      {
        unit004EvolutionPage.dialogBox_
      };
      List<GameObject> selfs2 = new List<GameObject>(8);
      switch (unit004EvolutionPage.evolutionType_)
      {
        case EvolutionType.Normal:
          selfs1.AddRange((IEnumerable<GameObject>) new GameObject[3]
          {
            ((Component) unit004EvolutionPage.beforeAwake_).gameObject,
            ((Component) unit004EvolutionPage.afterAwake_).gameObject,
            ((Component) unit004EvolutionPage.awakeBtn_).gameObject
          });
          selfs2.AddRange((IEnumerable<GameObject>) new GameObject[4]
          {
            ((Component) unit004EvolutionPage.beforeEvolution_).gameObject,
            ((Component) unit004EvolutionPage.afterEvolution_).gameObject,
            ((Component) unit004EvolutionPage.evolutionBtn_).gameObject,
            unit004EvolutionPage.dirEvolutionMaterials_
          });
          unit004EvolutionPage.beforeStatus_ = unit004EvolutionPage.beforeEvolution_;
          unit004EvolutionPage.afterStatus_ = unit004EvolutionPage.afterEvolution_;
          ((UIButtonColor) unit004EvolutionPage.evolutionBtn_).isEnabled = false;
          break;
        case EvolutionType.Awake:
          selfs1.AddRange((IEnumerable<GameObject>) new GameObject[3]
          {
            ((Component) unit004EvolutionPage.beforeEvolution_).gameObject,
            ((Component) unit004EvolutionPage.afterEvolution_).gameObject,
            ((Component) unit004EvolutionPage.evolutionBtn_).gameObject
          });
          selfs2.AddRange((IEnumerable<GameObject>) new GameObject[4]
          {
            ((Component) unit004EvolutionPage.beforeAwake_).gameObject,
            ((Component) unit004EvolutionPage.afterAwake_).gameObject,
            unit004EvolutionPage.dirEvolutionMaterials_,
            ((Component) unit004EvolutionPage.awakeBtn_).gameObject
          });
          unit004EvolutionPage.beforeStatus_ = unit004EvolutionPage.beforeAwake_;
          unit004EvolutionPage.afterStatus_ = unit004EvolutionPage.afterAwake_;
          ((UIButtonColor) unit004EvolutionPage.awakeBtn_).isEnabled = true;
          break;
        case EvolutionType.CommonAwake:
          selfs1.AddRange((IEnumerable<GameObject>) new GameObject[3]
          {
            ((Component) unit004EvolutionPage.beforeEvolution_).gameObject,
            ((Component) unit004EvolutionPage.afterEvolution_).gameObject,
            ((Component) unit004EvolutionPage.evolutionBtn_).gameObject
          });
          selfs2.AddRange((IEnumerable<GameObject>) new GameObject[4]
          {
            ((Component) unit004EvolutionPage.beforeAwake_).gameObject,
            ((Component) unit004EvolutionPage.afterAwake_).gameObject,
            unit004EvolutionPage.dirEvolutionMaterials_,
            ((Component) unit004EvolutionPage.awakeBtn_).gameObject
          });
          unit004EvolutionPage.beforeStatus_ = unit004EvolutionPage.beforeAwake_;
          unit004EvolutionPage.afterStatus_ = unit004EvolutionPage.afterAwake_;
          ((UIButtonColor) unit004EvolutionPage.awakeBtn_).isEnabled = true;
          break;
        default:
          selfs1.AddRange((IEnumerable<GameObject>) new GameObject[4]
          {
            ((Component) unit004EvolutionPage.beforeAwake_).gameObject,
            ((Component) unit004EvolutionPage.afterAwake_).gameObject,
            ((Component) unit004EvolutionPage.awakeBtn_).gameObject,
            unit004EvolutionPage.dirEvolutionMaterials_
          });
          selfs2.AddRange((IEnumerable<GameObject>) new GameObject[3]
          {
            ((Component) unit004EvolutionPage.beforeEvolution_).gameObject,
            ((Component) unit004EvolutionPage.afterEvolution_).gameObject,
            ((Component) unit004EvolutionPage.evolutionBtn_).gameObject
          });
          unit004EvolutionPage.beforeStatus_ = unit004EvolutionPage.beforeEvolution_;
          unit004EvolutionPage.afterStatus_ = unit004EvolutionPage.afterEvolution_;
          ((UIButtonColor) unit004EvolutionPage.evolutionBtn_).isEnabled = false;
          break;
      }
      selfs1.SetActives(false);
      selfs2.SetActives(true);
      if (unit004EvolutionPage.evolutionType_ != EvolutionType.Limit && unit004EvolutionPage.target_.estimatesEvolution == null)
      {
        int[] array = result1.Select<UnitEvolutionPattern, int>((Func<UnitEvolutionPattern, int>) (x => x.ID)).ToArray<int>();
        int base_player_material_unit_id;
        int base_player_unit_id;
        if (unit004EvolutionPage.baseUnitUnit_.IsNormalUnit)
        {
          base_player_material_unit_id = 0;
          base_player_unit_id = unit004EvolutionPage.baseUnit.id;
        }
        else
        {
          base_player_material_unit_id = unit004EvolutionPage.baseUnit.id;
          base_player_unit_id = 0;
        }
        Future<WebAPI.Response.UnitEvolutionParameter> paramF = WebAPI.UnitEvolutionParameter(base_player_material_unit_id, base_player_unit_id, 0, array, (Action<WebAPI.Response.UserError>) (e =>
        {
          WebAPI.DefaultUserErrorCallback(e);
          Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
        }));
        yield return (object) paramF.Wait();
        if (paramF.Result == null)
        {
          yield break;
        }
        else
        {
          unit004EvolutionPage.target_.estimatesEvolution = paramF.Result;
          WebAPI.Response.UnitEvolutionParameter result2 = paramF.Result;
          Func<PlayerUnit, PlayerUnit> changePlayerUnitId = (Func<PlayerUnit, PlayerUnit>) (pu =>
          {
            pu.id = -1;
            return pu;
          });
          unit004EvolutionPage.afterUnits_ = ((IEnumerable<PlayerUnit>) result2.target_player_units).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x => changePlayerUnitId(x))).Concat<PlayerUnit>(((IEnumerable<PlayerMaterialUnit>) result2.target_player_material_units).Select<PlayerMaterialUnit, PlayerUnit>((Func<PlayerMaterialUnit, PlayerUnit>) (y => changePlayerUnitId(PlayerUnit.CreateByPlayerMaterialUnit(y))))).ToArray<PlayerUnit>();
          paramF = (Future<WebAPI.Response.UnitEvolutionParameter>) null;
        }
      }
      else if (unit004EvolutionPage.target_.estimatesEvolution != null)
      {
        WebAPI.Response.UnitEvolutionParameter estimatesEvolution = unit004EvolutionPage.target_.estimatesEvolution;
        Func<PlayerUnit, PlayerUnit> changePlayerUnitId = (Func<PlayerUnit, PlayerUnit>) (pu =>
        {
          pu.id = -1;
          return pu;
        });
        unit004EvolutionPage.afterUnits_ = ((IEnumerable<PlayerUnit>) estimatesEvolution.target_player_units).Concat<PlayerUnit>(((IEnumerable<PlayerMaterialUnit>) estimatesEvolution.target_player_material_units).Select<PlayerMaterialUnit, PlayerUnit>((Func<PlayerMaterialUnit, PlayerUnit>) (y => changePlayerUnitId(PlayerUnit.CreateByPlayerMaterialUnit(y))))).ToArray<PlayerUnit>();
      }
      else
        unit004EvolutionPage.afterUnits_ = (PlayerUnit[]) null;
      while (unit004EvolutionPage.isLoadingResources)
        yield return (object) null;
      yield return (object) unit004EvolutionPage.initBeforePlayer();
      yield return (object) unit004EvolutionPage.initIndicator();
    }
  }

  private IEnumerator doEvolution()
  {
    Unit004EvolutionPage unit004EvolutionPage = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      int base_player_material_unit_id;
      int base_player_unit_id;
      if (unit004EvolutionPage.baseUnitUnit_.IsNormalUnit)
      {
        base_player_material_unit_id = 0;
        base_player_unit_id = unit004EvolutionPage.baseUnit.id;
      }
      else
      {
        base_player_material_unit_id = unit004EvolutionPage.baseUnit.id;
        base_player_unit_id = 0;
      }
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.UnitEvolution> paramF = WebAPI.UnitEvolution(base_player_material_unit_id, base_player_unit_id, unit004EvolutionPage.materialMaterialUnitIds_.ToArray(), unit004EvolutionPage.materialUnitIds_.ToArray(), unit004EvolutionPage.currentEvolutionPatternId_, (Action<WebAPI.Response.UserError>) (error =>
      {
        WebAPI.DefaultUserErrorCallback(error);
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
      }));
      IEnumerator e = paramF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (paramF.Result != null)
      {
        e = OnDemandDownload.WaitLoadHasUnitResource(false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        UnitEvolutionResultData.GetInstance().SetData(paramF.Result);
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) paramF.Result.corps_player_unit_ids);
        UnitUnit targetUnit = MasterData.UnitEvolutionPattern[unit004EvolutionPage.currentEvolutionPatternId_].target_unit;
        PlayerUnit result = (PlayerUnit) null;
        if (targetUnit.IsMaterialUnit)
        {
          foreach (PlayerMaterialUnit playerMaterialUnit in paramF.Result.player_material_units)
          {
            if (targetUnit.ID == playerMaterialUnit._unit)
            {
              result = PlayerUnit.CreateByPlayerMaterialUnit(playerMaterialUnit);
              break;
            }
          }
        }
        else if (unit004EvolutionPage.baseUnitUnit_.IsNormalUnit)
        {
          foreach (PlayerUnit playerUnit in paramF.Result.player_units)
          {
            if (playerUnit.id == unit004EvolutionPage.baseUnit.id)
            {
              result = playerUnit;
              break;
            }
          }
        }
        else
        {
          foreach (PlayerUnit playerUnit in paramF.Result.player_units)
          {
            if (targetUnit.ID == playerUnit._unit)
            {
              result = playerUnit;
              break;
            }
          }
        }
        List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
        foreach (GameObject lnkEvolutionUnit in unit004EvolutionPage.lnkEvolutionUnits_)
        {
          UnitIcon componentInChildren = ((Component) lnkEvolutionUnit.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
          if (componentInChildren.PlayerUnit != (PlayerUnit) null)
            playerUnitList.Add(componentInChildren.PlayerUnit);
        }
        PrincesEvolutionParam princesEvolutionParam = new PrincesEvolutionParam();
        princesEvolutionParam.materiaqlUnits = playerUnitList;
        princesEvolutionParam.is_new = paramF.Result.is_new;
        princesEvolutionParam.baseUnit = unit004EvolutionPage.baseUnit;
        princesEvolutionParam.resultUnit = result;
        switch (unit004EvolutionPage.evolutionType_)
        {
          case EvolutionType.Awake:
            princesEvolutionParam.mode = Unit00499Scene.Mode.AwakeUnit;
            break;
          case EvolutionType.CommonAwake:
            princesEvolutionParam.mode = Unit00499Scene.Mode.CommonAwakeUnit;
            break;
          default:
            princesEvolutionParam.mode = Unit00499Scene.Mode.Evolution;
            break;
        }
        unit004EvolutionPage.setBackSceneFromResult(result);
        unit00497Scene.ChangeScene(true, princesEvolutionParam);
        paramF = (Future<WebAPI.Response.UnitEvolution>) null;
      }
    }
  }

  private IEnumerator initBeforePlayer()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004EvolutionPage unit004EvolutionPage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit004EvolutionPage.beforeStatus_.SetStatusText(unit004EvolutionPage.baseUnit);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004EvolutionPage.initUnitIcon(unit004EvolutionPage.beforeStatus_.lnkUnit, unit004EvolutionPage.baseUnit, true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator initAfterPlayer(PlayerUnit target)
  {
    Unit004EvolutionPage unit004EvolutionPage = this;
    yield return (object) unit004EvolutionPage.initUnitIcon(unit004EvolutionPage.afterStatus_.lnkUnit, target, false);
    if (unit004EvolutionPage.evolutionType_ == EvolutionType.Normal)
      unit004EvolutionPage.beforeStatus_.setParamDetails(target);
    unit004EvolutionPage.afterStatus_.SetStatusText(target, unit004EvolutionPage.evolutionType_ == EvolutionType.Limit);
    if (unit004EvolutionPage.evolutionType_ == EvolutionType.Normal)
      unit004EvolutionPage.afterStatus_.setParamDetails(unit004EvolutionPage.baseUnit);
  }

  private PlayerUnit[] getMaterialPlayerUnits()
  {
    return ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (unit => this.materialUnitIds_.Contains(unit.id))).ToArray<PlayerUnit>();
  }

  private bool checkEnableButton(int money)
  {
    int index = -1;
    if ((long) money > Player.Current.money)
    {
      index = 0;
      int num = (int) this.errFlags_.On(Unit004TrainingPage.ErrorFlag.Money);
    }
    if (this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Material))
      index = 1;
    if (this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Level))
      index = 2;
    if (this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Favorite))
      index = 3;
    if (this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.UnityValue))
    {
      index = 4;
      this.unityValueShortage.SetTextLocalize(string.Format("必要淘汰値{0}", (object) ((IEnumerable<UnitEvolutionPattern>) this.evolutionPatterns_).First<UnitEvolutionPattern>((Func<UnitEvolutionPattern, bool>) (p => p.ID == this.currentEvolutionPatternId_)).threshold_unity_value));
    }
    if (this.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Limit))
      index = 5;
    bool flag = this.errFlags_.Any();
    if (flag)
      ((IEnumerable<GameObject>) this.comShortages_).ToggleOnce(index);
    this.comShortage_.SetActive(flag);
    return !flag;
  }

  private void ShowMaterialQuestInfo(UnitUnit materail)
  {
    int num = !this.dialogBox_.activeInHierarchy ? 1 : 0;
    this.dialogBox_.SetActive(true);
    if (num != 0)
    {
      UITweener[] tweeners = NGTween.findTweeners(this.dialogBox_, true);
      NGTween.playTweens(tweeners, NGTween.Kind.START_END);
      NGTween.playTweens(tweeners, NGTween.Kind.START);
      foreach (UITweener uiTweener in tweeners)
        uiTweener.onFinished.Clear();
    }
    this.txtMaterialName_.SetText(materail.name);
    UnitMaterialQuestInfo materialQuestInfo = ((IEnumerable<UnitMaterialQuestInfo>) MasterData.UnitMaterialQuestInfoList).SingleOrDefault<UnitMaterialQuestInfo>((Func<UnitMaterialQuestInfo, bool>) (x => x.unit_id == materail.ID));
    if (materialQuestInfo == null)
      this.txtMaterialPlace_.SetText("");
    else
      this.txtMaterialPlace_.SetText(materialQuestInfo.long_desc);
  }

  public UITweener[] EndTweensMaterialQuestInfo(bool isForce = false)
  {
    if (!this.dialogBox_.activeInHierarchy)
      return (UITweener[]) null;
    UITweener[] tweeners = NGTween.findTweeners(this.dialogBox_, true);
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

  private void HideDialogBox() => this.dialogBox_.SetActive(false);

  public void IbtnCom()
  {
    if (this.errFlags_.Any() || this.isMovingIndicator_ || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.IbtnComAsync());
  }

  private IEnumerator IbtnComAsync()
  {
    Unit004EvolutionPage unit004EvolutionPage = this;
    if (unit004EvolutionPage.baseUnit.tower_is_entry || unit004EvolutionPage.baseUnit.corps_is_entry || ((IEnumerable<PlayerUnit>) unit004EvolutionPage.getMaterialPlayerUnits()).Any<PlayerUnit>((Func<PlayerUnit, bool>) (unit => unit.tower_is_entry || unit.corps_is_entry)))
    {
      bool isRejected = false;
      yield return (object) PopupManager.StartTowerEntryUnitWarningPopupProc((Action<bool>) (selected => isRejected = !selected));
      if (isRejected)
        yield break;
    }
    Consts instance = Consts.GetInstance();
    string message;
    if (unit004EvolutionPage.gvgDeckCostOver != GvgDeckCostOver.None)
    {
      string str;
      switch (unit004EvolutionPage.gvgDeckCostOver)
      {
        case GvgDeckCostOver.Attack:
          str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_attack_message;
          break;
        case GvgDeckCostOver.Defense:
          str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_defense_message;
          break;
        case GvgDeckCostOver.Both:
          str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_both_message;
          break;
        default:
          str = string.Empty;
          break;
      }
      message = Consts.Format(instance.unit_004_9_9_confirm_evolution_gvg_cost_over_message, (IDictionary) new Hashtable()
      {
        {
          (object) "deck",
          (object) str
        }
      });
    }
    else
      message = instance.unit_004_9_9_confirm_evolution_message;
    // ISSUE: reference to a compiler-generated method
    if (((IEnumerable<int?>) (PlayerTransmigrateMemoryPlayerUnitIds.Current?.transmigrate_memory_player_unit_ids ?? new int?[0])).Any<int?>(new Func<int?, bool>(unit004EvolutionPage.\u003CIbtnComAsync\u003Eb__52_1)))
      message += instance.unit_004_9_confirm_memory_alert;
    string messageSub = string.Empty;
    if (unit004EvolutionPage.baseUnit.breakthrough_count < unit004EvolutionPage.baseUnitUnit_.breakthrough_limit)
      messageSub = Consts.GetInstance().UNIT004TRAINING_ALERT_LIMITBREAK;
    if (unit004EvolutionPage.beforeStatus_.growthDegree <= GrowthDegree.A)
    {
      if (!string.IsNullOrEmpty(messageSub))
        messageSub += "\n";
      messageSub += Consts.Format(Consts.GetInstance().UNIT004TRAINING_ALERT_LESSEQUAL_GROWTHDEGREE, (IDictionary) new Hashtable()
      {
        {
          (object) "rank",
          (object) GrowthDegree.A
        }
      });
    }
    if (!string.IsNullOrEmpty(messageSub))
      messageSub = Unit004EvolutionPage.ALERT_COLOR + messageSub;
    yield return (object) unit004EvolutionPage.confirmExecute(instance.unit_004_9_9_confirm_evolution_title, message, messageSub, unit004EvolutionPage.currentEvolutionPatternId_, unit004EvolutionPage.doEvolution());
  }

  public void IbtnAwake()
  {
    if (this.errFlags_.Any() || this.isMovingIndicator_ || this.IsPushAndSet())
      return;
    Consts instance = Consts.GetInstance();
    string message;
    if (this.gvgDeckCostOver != GvgDeckCostOver.None)
    {
      string str;
      switch (this.gvgDeckCostOver)
      {
        case GvgDeckCostOver.Attack:
          str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_attack_message;
          break;
        case GvgDeckCostOver.Defense:
          str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_defense_message;
          break;
        case GvgDeckCostOver.Both:
          str = instance.unit_004_9_9_confirm_evolution_gvg_cost_over_both_message;
          break;
        default:
          str = string.Empty;
          break;
      }
      message = Consts.Format(instance.unit_004_9_9_confirm_evolution_gvg_cost_over_message, (IDictionary) new Hashtable()
      {
        {
          (object) "deck",
          (object) str
        }
      });
    }
    else
      message = instance.unit_004_9_9_confirm_awake_message;
    this.StartCoroutine(this.confirmExecute(instance.unit_004_9_9_confirm_awake_title, message, (string) null, this.currentEvolutionPatternId_, this.doEvolution()));
  }

  private IEnumerator confirmExecute(
    string title,
    string message,
    string messageSub,
    int currentPatternId,
    IEnumerator execute)
  {
    Unit004EvolutionPage unit004EvolutionPage = this;
    int nWait = 0;
    Action yes = (Action) (() => nWait = 1);
    Action no = (Action) (() => nWait = 2);
    if (string.IsNullOrEmpty(messageSub))
      PopupCommonNoYes.Show(title, message, yes, no);
    else
      PopupCommonNoYes.Show(title, message, yes, no, messageB: messageSub, alignmentB: (NGUIText.Alignment) 2);
    while (nWait == 0 && currentPatternId == unit004EvolutionPage.currentEvolutionPatternId_)
      yield return (object) null;
    if (nWait == 2 || currentPatternId != unit004EvolutionPage.currentEvolutionPatternId_)
      unit004EvolutionPage.IsPush = false;
    else
      unit004EvolutionPage.StartCoroutine(execute);
  }

  private static bool isUnitSelectable(UnitUnit unit) => unit.IsNormalUnit;

  private static Unit004EvolutionPage.UnitCondition getUnitCondition(
    PlayerUnit playerUnit,
    PlayerDeck[] decks)
  {
    foreach (PlayerDeck deck in decks)
    {
      if (deck != null)
      {
        foreach (PlayerUnit playerUnit1 in deck.player_units)
        {
          if (!(playerUnit1 == (PlayerUnit) null) && playerUnit1.id == playerUnit.id)
            return Unit004EvolutionPage.UnitCondition.Organized;
        }
      }
    }
    return !playerUnit.favorite ? Unit004EvolutionPage.UnitCondition.Normal : Unit004EvolutionPage.UnitCondition.Favarite;
  }

  private IEnumerator initEvolutionUnits(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits,
    Unit00499EvolutionIndicator eIndicator,
    int patternId,
    UnitUnit[] evoUnits)
  {
    Unit004EvolutionPage unit004EvolutionPage = this;
    UILabel[] label = eIndicator.linkEvolutionUnitsPossessionLabel;
    GameObject[] objects = eIndicator.linkEvolutionUnits;
    Unit00499EvolutionIndicator.ButtonIcon[] selectIcons = eIndicator.linkSelectableUnits;
    if (unit004EvolutionPage.dicSelector_ == null)
      unit004EvolutionPage.dicSelector_ = new Dictionary<int, Unit004EvolutionPage.EvolutionSelectMap>();
    Unit004EvolutionPage.EvolutionSelectMap selector;
    if (unit004EvolutionPage.dicSelector_.ContainsKey(patternId))
    {
      selector = unit004EvolutionPage.dicSelector_[patternId];
      selector.updateNormalUnit(playerUnits);
    }
    else
    {
      selector = Unit004EvolutionPage.EvolutionSelectMap.create(unit004EvolutionPage.baseUnit, playerUnits, playerMaterialUnits, evoUnits);
      selector.selectAuto(false);
      unit004EvolutionPage.dicSelector_.Add(patternId, selector);
    }
    foreach (GameObject gameObject in objects)
      gameObject.transform.Clear();
    bool canCompleted = ((IEnumerable<bool>) selector.trySelecting()).Where<bool>((Func<bool, bool>) (b => b)).ToArray<bool>().Length == selector.samples_.Length;
    for (int n = 0; n < objects.Length; ++n)
    {
      GameObject gameObject = objects[n];
      UnitUnit evoUnit = n < evoUnits.Length ? evoUnits[n] : (UnitUnit) null;
      Unit004EvolutionPage.SelectCell selectCell = n < selector.selected_.Length ? selector.selected_[n] : (Unit004EvolutionPage.SelectCell) null;
      GameObject top = selectIcons == null || n >= selectIcons.Length ? (GameObject) null : selectIcons[n].top_;
      if (Object.op_Inequality((Object) top, (Object) null))
        top.SetActive(canCompleted && evoUnit != null && selector.isSelectable(evoUnit.ID));
      UnitIcon component = unit004EvolutionPage.unitIconPrefab_.CloneAndGetComponent<UnitIcon>(gameObject.transform);
      IEnumerator enumerator;
      if (evoUnit != null)
      {
        if (selectCell != null)
        {
          PlayerUnit unit = selectCell.unit_;
          enumerator = evoUnit.IsMaterialUnit ? component.SetMaterialUnit(unit, false, (PlayerUnit[]) null) : component.SetPlayerUnit(unit, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
          if (unit.favorite)
          {
            int num = (int) unit004EvolutionPage.errFlags_.On(Unit004TrainingPage.ErrorFlag.Favorite);
          }
        }
        else
        {
          enumerator = component.SetUnit(evoUnit, evoUnit.GetElement(), true);
          int num = (int) unit004EvolutionPage.errFlags_.On(Unit004TrainingPage.ErrorFlag.Material);
        }
      }
      else
      {
        enumerator = component.SetPlayerUnit((PlayerUnit) null, (PlayerUnit[]) null, (PlayerUnit) null, false, false);
        component.BackgroundModeValue = UnitIcon.BackgroundMode.PlayerShadow;
      }
      yield return (object) enumerator;
    }
    int column = 0;
    foreach (GameObject gameObject in objects)
    {
      UnitIcon icon = ((Component) gameObject.GetComponent<UI2DSprite>()).GetComponentInChildren<UnitIcon>();
      if (column < selector.samples_.Length)
      {
        ((Behaviour) icon.Button).enabled = true;
        ((Collider) icon.buttonBoxCollider).enabled = true;
        int num = 0;
        if (icon.unit.IsMaterialUnit)
        {
          PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) playerMaterialUnits).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => icon.Unit.ID == x.unit.ID));
          if (playerMaterialUnit != null)
            num = playerMaterialUnit.quantity;
          if (icon.Unit.ID == unit004EvolutionPage.baseUnit.unit.ID)
            --num;
          icon.RarityCenter();
          unit004EvolutionPage.setEventMaterialClicked(icon, (LongPressButton) null, selector, column, canCompleted);
        }
        else
        {
          if (icon.PlayerUnit != (PlayerUnit) null)
            icon.setBottom(icon.PlayerUnit);
          else
            icon.setLevelText("1");
          icon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
          num = selector.sources_[icon.Unit.ID].Length;
          unit004EvolutionPage.setEventMaterialClicked(icon, selectIcons == null || column >= selectIcons.Length ? (LongPressButton) null : selectIcons[column].button_, selector, column, canCompleted);
        }
        label[column].SetTextLocalize(Consts.Format(Consts.GetInstance().unit_004_9_9_possession_text, (IDictionary) new Hashtable()
        {
          {
            (object) "Count",
            (object) num
          }
        }));
        ((Component) label[column]).gameObject.SetActive(true);
      }
      else
      {
        ((Behaviour) icon.Button).enabled = false;
        ((Collider) icon.buttonBoxCollider).enabled = false;
        icon.SetEmpty();
        icon.setSilhouette(false);
        ((Component) label[column]).gameObject.SetActive(false);
        gameObject.GetComponentInChildren<UIButton>().onClick.Clear();
      }
      ++column;
    }
  }

  private void setEventMaterialClicked(
    UnitIcon icon,
    LongPressButton toSelect,
    Unit004EvolutionPage.EvolutionSelectMap selector,
    int column,
    bool canCompleted)
  {
    if (Object.op_Equality((Object) toSelect, (Object) null) || !canCompleted)
    {
      icon.onClick = (Action<UnitIconBase>) (x => this.ShowMaterialQuestInfo(x.Unit));
      icon.SetButtonDetailEvent(icon.PlayerUnit, ((IEnumerable<Unit004EvolutionPage.SelectCell>) selector.selected_).Where<Unit004EvolutionPage.SelectCell>((Func<Unit004EvolutionPage.SelectCell, bool>) (x => x != null)).Select<Unit004EvolutionPage.SelectCell, PlayerUnit>((Func<Unit004EvolutionPage.SelectCell, PlayerUnit>) (c => c.unit_)).ToArray<PlayerUnit>());
    }
    else
    {
      ((UIButtonColor) icon.Button).isEnabled = false;
      ((Collider) icon.buttonBoxCollider).enabled = false;
      EventDelegate.Set(toSelect.onClick, (EventDelegate.Callback) (() =>
      {
        if (this.IsPushAndSet())
          return;
        this.doUnitSelector(selector, column);
      }));
      if (icon.PlayerUnit != (PlayerUnit) null)
        EventDelegate.Set(toSelect.onLongPress, (EventDelegate.Callback) (() => Unit0042Scene.changeScene(true, icon.PlayerUnit, ((IEnumerable<Unit004EvolutionPage.SelectCell>) selector.selected_).Where<Unit004EvolutionPage.SelectCell>((Func<Unit004EvolutionPage.SelectCell, bool>) (x => x != null)).Select<Unit004EvolutionPage.SelectCell, PlayerUnit>((Func<Unit004EvolutionPage.SelectCell, PlayerUnit>) (c => c.unit_)).ToArray<PlayerUnit>())));
      else
        toSelect.onLongPress.Clear();
    }
  }

  private void doUnitSelector(Unit004EvolutionPage.EvolutionSelectMap selector, int column)
  {
    UnitUnit sample = selector.samples_[column];
    Unit00492Menu.Param param = new Unit00492Menu.Param()
    {
      baseUnit_ = selector.selected_[column] != null ? selector.selected_[column].unit_ : (PlayerUnit) null
    };
    param.selectedUnits_ = ((IEnumerable<Unit004EvolutionPage.SelectCell>) selector.selected_).Where<Unit004EvolutionPage.SelectCell>((Func<Unit004EvolutionPage.SelectCell, bool>) (c => c != null && (param.baseUnit_ == (PlayerUnit) null || param.baseUnit_.id != c.unit_.id) && c.unit_.unit.ID == sample.ID)).Select<Unit004EvolutionPage.SelectCell, PlayerUnit>((Func<Unit004EvolutionPage.SelectCell, PlayerUnit>) (cc => cc.unit_)).ToArray<PlayerUnit>();
    param.units_ = ((IEnumerable<Unit004EvolutionPage.SelectCell>) selector.sources_[sample.ID]).Select<Unit004EvolutionPage.SelectCell, PlayerUnit>((Func<Unit004EvolutionPage.SelectCell, PlayerUnit>) (c => c.unit_)).ToArray<PlayerUnit>();
    param.onUpdate_ = (Unit00492Menu.Param.EventUpdateUnit) (unit =>
    {
      Unit004EvolutionPage.SelectCell selectCell = ((IEnumerable<Unit004EvolutionPage.SelectCell>) selector.selected_).FirstOrDefault<Unit004EvolutionPage.SelectCell>((Func<Unit004EvolutionPage.SelectCell, bool>) (c => c != null && c.unit_.id == unit.id));
      if (selectCell == null)
        return;
      PlayerDeck[] decks = SMManager.Get<PlayerDeck[]>();
      if (Unit004EvolutionPage.getUnitCondition(unit, decks) == Unit004EvolutionPage.UnitCondition.Normal)
        return;
      selector.setUnselected(selectCell.column_);
    });
    param.onResult_ = (Unit00492Menu.Param.EventResult) (result =>
    {
      if (result == (PlayerUnit) null || param.baseUnit_ != (PlayerUnit) null && param.baseUnit_.id == result.id)
        return;
      selector.setSelected(((IEnumerable<Unit004EvolutionPage.SelectCell>) selector.sources_[sample.ID]).First<Unit004EvolutionPage.SelectCell>((Func<Unit004EvolutionPage.SelectCell, bool>) (c => c.unit_.id == result.id)), column);
    });
    Unit00468Scene.changeScene00492EvolutionMaterial(true, param);
  }

  private void updateCheckEnableButton(int patternId)
  {
    Unit004EvolutionPage.EvolutionSelectMap evolutionSelectMap;
    if (this.dicSelector_ != null && this.dicSelector_.TryGetValue(patternId, out evolutionSelectMap) && evolutionSelectMap.isCompletedSelect)
    {
      int num1 = (int) this.errFlags_.Off(Unit004TrainingPage.ErrorFlag.Material);
    }
    else
    {
      int num2 = (int) this.errFlags_.On(Unit004TrainingPage.ErrorFlag.Material);
    }
    int num3 = (int) this.errFlags_.Off(Unit004TrainingPage.ErrorFlag.Favorite);
  }

  public List<int> getEvolutionMaterialSelectedUnitIds(int patternId)
  {
    return this.dicSelector_ == null || !this.dicSelector_.ContainsKey(patternId) ? new List<int>() : ((IEnumerable<Unit004EvolutionPage.SelectCell>) this.dicSelector_[patternId].selected_).Where<Unit004EvolutionPage.SelectCell>((Func<Unit004EvolutionPage.SelectCell, bool>) (c => c != null && c.unit_.unit.IsNormalUnit)).Select<Unit004EvolutionPage.SelectCell, int>((Func<Unit004EvolutionPage.SelectCell, int>) (cc => cc.unit_.id)).ToList<int>();
  }

  public List<int> getEvolutionMaterialSelectedMaterialIds(int patternId)
  {
    return this.dicSelector_ == null || !this.dicSelector_.ContainsKey(patternId) ? new List<int>() : ((IEnumerable<Unit004EvolutionPage.SelectCell>) this.dicSelector_[patternId].selected_).Where<Unit004EvolutionPage.SelectCell>((Func<Unit004EvolutionPage.SelectCell, bool>) (c => c != null && c.unit_.unit.IsMaterialUnit)).Select<Unit004EvolutionPage.SelectCell, int>((Func<Unit004EvolutionPage.SelectCell, int>) (cc => cc.unit_.id)).ToList<int>();
  }

  private bool isEvolutionButtonEnabled => this.realityEvolutionButton_ && !this.isMovingIndicator_;

  private IEnumerator WaitScrollSe()
  {
    this.indicator_.SeEnable = true;
    yield return (object) null;
  }

  private void Update()
  {
    this.isMovingIndicator_ = false;
    if (this.isWaitInitalize_ || this.evolutionPatterns_ == null || this.evolutionPatterns_.Length == 0 || this.afterUnits_ == null || this.afterUnits_.Length == 0)
      return;
    int selected = this.indicator_.selected;
    if (selected >= this.evolutionPatterns_.Length || selected < 0)
      return;
    if (this.evolutionPatterns_.Length > 1 && (double) this.positionIndicator_ != (double) this.indicator_.scrollView.transform.localPosition.x)
    {
      this.isMovingIndicator_ = true;
      this.positionIndicator_ = this.indicator_.scrollView.transform.localPosition.x;
    }
    if (this.evolutionPatterns_[selected].ID != this.currentEvolutionPatternId_)
    {
      this.HideMaterialQuestInfo();
      this.StopCoroutine("processByswipeIndicator");
      this.standbyEvolutionButton();
      this.StartCoroutine("processByswipeIndicator", (object) selected);
    }
    else if (((UIButtonColor) this.evolutionBtn_).isEnabled != this.isEvolutionButtonEnabled)
      ((UIButtonColor) this.evolutionBtn_).isEnabled = this.isEvolutionButtonEnabled;
    bool flag1 = Object.op_Inequality((Object) Singleton<PopupManager>.GetInstanceOrNull(), (Object) null) && Singleton<PopupManager>.GetInstance().isOpen;
    bool flag2 = Object.op_Inequality((Object) Singleton<CommonRoot>.GetInstanceOrNull(), (Object) null) && Singleton<CommonRoot>.GetInstance().isInputBlock;
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized || Singleton<TutorialRoot>.GetInstance().IsAdviced || !Input.GetKeyUp((KeyCode) 27) || flag1 || flag2 || Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount != 0)
      return;
    this.mainMenu_.onClickedBack();
  }

  private IEnumerator processByswipeIndicator(int selectedIdx)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit004EvolutionPage unit004EvolutionPage = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit004EvolutionPage.CheckEvolutionPossible();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    UnitEvolutionPattern evolutionPattern = unit004EvolutionPage.evolutionPatterns_[selectedIdx];
    unit004EvolutionPage.currentEvolutionPatternId_ = evolutionPattern.ID;
    unit004EvolutionPage.materialUnitIds_ = unit004EvolutionPage.getEvolutionMaterialSelectedUnitIds(unit004EvolutionPage.currentEvolutionPatternId_);
    unit004EvolutionPage.materialMaterialUnitIds_ = unit004EvolutionPage.getEvolutionMaterialSelectedMaterialIds(unit004EvolutionPage.currentEvolutionPatternId_);
    unit004EvolutionPage.setCostZeny((long) evolutionPattern.money);
    unit004EvolutionPage.lnkEvolutionUnits_ = unit004EvolutionPage.linkEvolutionUnitsDict_[unit004EvolutionPage.currentEvolutionPatternId_];
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) unit004EvolutionPage.initAfterPlayer(unit004EvolutionPage.afterUnits_[selectedIdx]);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator initIndicator()
  {
    Unit004EvolutionPage unit004EvolutionPage = this;
    unit004EvolutionPage.evolutionMaterialDict_ = unit004EvolutionPage.baseUnitUnit_.EvolutionUnits;
    unit004EvolutionPage.linkEvolutionUnitsDict_ = new Dictionary<int, GameObject[]>();
    unit004EvolutionPage.isMovingIndicator_ = false;
    unit004EvolutionPage.standbyEvolutionButton();
    if (unit004EvolutionPage.errFlags_.IsOn(Unit004TrainingPage.ErrorFlag.Limit))
    {
      unit004EvolutionPage.checkEnableButton(0);
      unit004EvolutionPage.currentEvolutionPatternId_ = 0;
      ((Component) unit004EvolutionPage.indicator_).gameObject.SetActive(false);
      unit004EvolutionPage.txtMyZeny_.SetTextLocalize(Player.Current.money);
      unit004EvolutionPage.txtCost_.SetTextLocalize("");
      yield return (object) unit004EvolutionPage.initAfterPlayer(unit004EvolutionPage.baseUnit);
    }
    else
    {
      ((Component) unit004EvolutionPage.indicator_).gameObject.SetActive(true);
      // ISSUE: reference to a compiler-generated method
      int lastIndex = ((IEnumerable<UnitEvolutionPattern>) unit004EvolutionPage.evolutionPatterns_).FirstIndexOrNull<UnitEvolutionPattern>(new Func<UnitEvolutionPattern, bool>(unit004EvolutionPage.\u003CinitIndicator\u003Eb__80_0)) ?? -1;
      if (lastIndex < 0)
      {
        unit004EvolutionPage.currentEvolutionPatternId_ = ((IEnumerable<UnitEvolutionPattern>) unit004EvolutionPage.evolutionPatterns_).First<UnitEvolutionPattern>().ID;
        lastIndex = 0;
      }
      unit004EvolutionPage.indicator_.SeEnable = false;
      PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
      PlayerMaterialUnit[] playerMaterialUnits = SMManager.Get<PlayerMaterialUnit[]>();
      unit004EvolutionPage.indicator_.destroyParts(false);
      UnitEvolutionPattern[] evolutionPatternArray = unit004EvolutionPage.evolutionPatterns_;
      for (int index = 0; index < evolutionPatternArray.Length; ++index)
      {
        UnitEvolutionPattern evolutionPattern = evolutionPatternArray[index];
        yield return (object) unit004EvolutionPage.CreateIndicator(evolutionPattern.ID, playerUnits, playerMaterialUnits);
      }
      evolutionPatternArray = (UnitEvolutionPattern[]) null;
      unit004EvolutionPage.indicator_.resetScrollView();
      unit004EvolutionPage.indicator_.setItemPositionQuick(lastIndex);
      unit004EvolutionPage.positionIndicator_ = unit004EvolutionPage.indicator_.scrollView.transform.localPosition.x;
      yield return (object) unit004EvolutionPage.processByswipeIndicator(lastIndex);
      ((Component) unit004EvolutionPage.indicator_.dot).gameObject.SetActive(true);
      if (unit004EvolutionPage.evolutionPatterns_.Length == 1)
        ((Component) unit004EvolutionPage.indicator_.dot).gameObject.SetActive(false);
      unit004EvolutionPage.StartCoroutine(unit004EvolutionPage.WaitScrollSe());
    }
  }

  private void standbyEvolutionButton()
  {
    ((UIButtonColor) this.evolutionBtn_).isEnabled = false;
    this.realityEvolutionButton_ = false;
  }

  private IEnumerator CreateIndicator(
    int evolutionPatternId,
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits)
  {
    Unit00499EvolutionIndicator component = this.indicator_.instantiateParts(this.indicatorPrefab_).GetComponent<Unit00499EvolutionIndicator>();
    this.linkEvolutionUnitsDict_[evolutionPatternId] = component.linkEvolutionUnits;
    yield return (object) this.initEvolutionUnits(playerUnits, playerMaterialUnits, component, evolutionPatternId, this.evolutionMaterialDict_[evolutionPatternId]);
  }

  private void CheckEvolutionPossible()
  {
    UnitEvolutionPattern evolutionPattern = ((IEnumerable<UnitEvolutionPattern>) this.evolutionPatterns_).First<UnitEvolutionPattern>((Func<UnitEvolutionPattern, bool>) (p => p.ID == this.currentEvolutionPatternId_));
    if (this.baseUnit.level < evolutionPattern.threshold_level && this.baseUnitUnit_.IsNormalUnit)
    {
      int num = (int) this.errFlags_.On(Unit004TrainingPage.ErrorFlag.Level);
      this.txtShortageLevel_.SetTextLocalize(Consts.Format(Consts.GetInstance().UNIT004TRAINING_SHORTAGE_LEVEL, (IDictionary) new Hashtable()
      {
        {
          (object) "level",
          (object) evolutionPattern.threshold_level
        }
      }));
    }
    if (this.evolutionType_ == EvolutionType.CommonAwake && (double) this.baseUnit.unityTotal < (double) evolutionPattern.threshold_unity_value)
    {
      int num1 = (int) this.errFlags_.On(Unit004TrainingPage.ErrorFlag.UnityValue);
    }
    this.updateCheckEnableButton(this.currentEvolutionPatternId_);
    this.realityEvolutionButton_ = this.checkEnableButton(evolutionPattern.money);
    switch (this.evolutionType_)
    {
      case EvolutionType.Normal:
        ((UIButtonColor) this.evolutionBtn_).isEnabled = this.isEvolutionButtonEnabled;
        break;
      case EvolutionType.Awake:
        ((UIButtonColor) this.awakeBtn_).isEnabled = this.isEvolutionButtonEnabled;
        break;
      case EvolutionType.CommonAwake:
        ((UIButtonColor) this.awakeBtn_).isEnabled = this.isEvolutionButtonEnabled;
        break;
    }
  }

  private enum UnitCondition
  {
    Normal,
    Selected,
    Favarite,
    Organized,
  }

  private class SelectCell
  {
    public PlayerUnit unit_ { get; private set; }

    public void setUnit(PlayerUnit u) => this.unit_ = u;

    public int id_ { get; private set; }

    public Unit004EvolutionPage.UnitCondition condition_ { get; private set; }

    public void setCondition(Unit004EvolutionPage.UnitCondition uc) => this.condition_ = uc;

    public bool isSelectable => this.condition_ == Unit004EvolutionPage.UnitCondition.Normal;

    public int column_ { get; private set; }

    public void setColumn(int column = -1)
    {
      this.column_ = column;
      if (column >= 0)
      {
        this.condition_ = Unit004EvolutionPage.UnitCondition.Selected;
      }
      else
      {
        if (this.condition_ != Unit004EvolutionPage.UnitCondition.Selected)
          return;
        this.condition_ = Unit004EvolutionPage.UnitCondition.Normal;
      }
    }

    public SelectCell(PlayerUnit unit, int id, Unit004EvolutionPage.UnitCondition uc = Unit004EvolutionPage.UnitCondition.Normal)
    {
      this.unit_ = unit;
      this.id_ = id;
      this.condition_ = uc;
      this.column_ = -1;
    }
  }

  private class EvolutionSelectMap
  {
    private Dictionary<int, bool> dicSelectable_;

    public UnitUnit[] samples_ { get; private set; }

    public Dictionary<int, Unit004EvolutionPage.SelectCell[]> sources_ { get; private set; }

    public Unit004EvolutionPage.SelectCell[] selected_ { get; private set; }

    public int selectedCount_ { get; private set; }

    public bool isCompletedSelect => this.selectedCount_ == this.selected_.Length;

    public bool isSelectable(int id) => this.dicSelectable_[id];

    public static Unit004EvolutionPage.EvolutionSelectMap create(
      PlayerUnit baseUnit,
      PlayerUnit[] playerunits,
      PlayerMaterialUnit[] materials,
      UnitUnit[] evosamples)
    {
      if (baseUnit == (PlayerUnit) null || playerunits == null || playerunits.Length == 0 || evosamples == null || evosamples.Length == 0)
        return (Unit004EvolutionPage.EvolutionSelectMap) null;
      Unit004EvolutionPage.EvolutionSelectMap evolutionSelectMap = new Unit004EvolutionPage.EvolutionSelectMap();
      evolutionSelectMap.samples_ = evosamples;
      evolutionSelectMap.selectedCount_ = 0;
      int length = evosamples.Length;
      evolutionSelectMap.selected_ = new Unit004EvolutionPage.SelectCell[length];
      List<int> list = ((IEnumerable<UnitUnit>) evosamples).Select<UnitUnit, int>((Func<UnitUnit, int>) (s => s.ID)).Distinct<int>().ToList<int>();
      Dictionary<int, List<Unit004EvolutionPage.SelectCell>> dictionary = list.ToDictionary<int, int, List<Unit004EvolutionPage.SelectCell>>((Func<int, int>) (k => k), (Func<int, List<Unit004EvolutionPage.SelectCell>>) (k => new List<Unit004EvolutionPage.SelectCell>()));
      evolutionSelectMap.dicSelectable_ = list.ToDictionary<int, int, bool>((Func<int, int>) (k => k), (Func<int, bool>) (k => false));
      int num = 1;
      PlayerDeck[] decks = SMManager.Get<PlayerDeck[]>();
      foreach (PlayerUnit playerunit in playerunits)
      {
        List<Unit004EvolutionPage.SelectCell> selectCellList;
        if (baseUnit.id != playerunit.id && dictionary.TryGetValue(playerunit.unit.ID, out selectCellList))
          selectCellList.Add(new Unit004EvolutionPage.SelectCell(playerunit, num++, Unit004EvolutionPage.getUnitCondition(playerunit, decks)));
      }
      foreach (List<Unit004EvolutionPage.SelectCell> source in dictionary.Values)
      {
        if (source.Any<Unit004EvolutionPage.SelectCell>())
        {
          UnitUnit unit = source.First<Unit004EvolutionPage.SelectCell>().unit_.unit;
          evolutionSelectMap.dicSelectable_[unit.ID] = Unit004EvolutionPage.isUnitSelectable(unit);
        }
      }
      foreach (PlayerMaterialUnit material in materials)
      {
        List<Unit004EvolutionPage.SelectCell> selectCellList;
        if (dictionary.TryGetValue(material.unit.ID, out selectCellList))
        {
          int quantity = material.quantity;
          for (int count = 0; count < quantity && count < length; ++count)
            selectCellList.Add(new Unit004EvolutionPage.SelectCell(PlayerUnit.CreateByPlayerMaterialUnit(material, count), num++));
        }
      }
      evolutionSelectMap.sources_ = dictionary.ToDictionary<KeyValuePair<int, List<Unit004EvolutionPage.SelectCell>>, int, Unit004EvolutionPage.SelectCell[]>((Func<KeyValuePair<int, List<Unit004EvolutionPage.SelectCell>>, int>) (k => k.Key), (Func<KeyValuePair<int, List<Unit004EvolutionPage.SelectCell>>, Unit004EvolutionPage.SelectCell[]>) (k => k.Value.ToArray()));
      return evolutionSelectMap;
    }

    public void selectAuto(bool ignoreSelectable = true)
    {
      for (int column = 0; column < this.samples_.Length; ++column)
      {
        UnitUnit sample = this.samples_[column];
        if (!ignoreSelectable || !this.isSelectable(sample.ID))
        {
          Unit004EvolutionPage.SelectCell cell = ((IEnumerable<Unit004EvolutionPage.SelectCell>) this.sources_[sample.ID]).FirstOrDefault<Unit004EvolutionPage.SelectCell>((Func<Unit004EvolutionPage.SelectCell, bool>) (s => s.isSelectable));
          if (cell != null)
            this.setSelected(cell, column);
        }
      }
    }

    public void setSelected(Unit004EvolutionPage.SelectCell cell, int column)
    {
      if (cell == null)
      {
        this.setUnselected(column);
      }
      else
      {
        if (cell.column_ == column)
          return;
        this.setUnselected(cell);
        if (column < 0 || column >= this.selected_.Length)
          return;
        this.setUnselected(column);
        this.selected_[column] = cell;
        cell.setColumn(column);
        ++this.selectedCount_;
      }
    }

    public void setUnselected(Unit004EvolutionPage.SelectCell cell)
    {
      this.setUnselected(cell.column_);
    }

    public void setUnselected(int column)
    {
      if (column < 0 || column >= this.selected_.Length || this.selected_[column] == null)
        return;
      Unit004EvolutionPage.SelectCell selectCell = this.selected_[column];
      this.selected_[column] = (Unit004EvolutionPage.SelectCell) null;
      selectCell.setColumn();
      --this.selectedCount_;
    }

    public bool[] trySelecting()
    {
      bool[] flagArray = new bool[this.samples_.Length];
      Dictionary<int, Queue<PlayerUnit>> dictionary = new Dictionary<int, Queue<PlayerUnit>>();
      foreach (KeyValuePair<int, Unit004EvolutionPage.SelectCell[]> source in this.sources_)
      {
        Queue<PlayerUnit> playerUnitQueue = new Queue<PlayerUnit>();
        foreach (Unit004EvolutionPage.SelectCell selectCell in source.Value)
          playerUnitQueue.Enqueue(selectCell.unit_);
        dictionary.Add(source.Key, playerUnitQueue);
      }
      for (int index = 0; index < this.samples_.Length; ++index)
      {
        Queue<PlayerUnit> playerUnitQueue = dictionary[this.samples_[index].ID];
        flagArray[index] = playerUnitQueue.Count > 0;
        if (flagArray[index])
          playerUnitQueue.Dequeue();
      }
      return flagArray;
    }

    public bool updateNormalUnit(PlayerUnit[] playerUnits)
    {
      bool flag = false;
      PlayerDeck[] decks = SMManager.Get<PlayerDeck[]>();
      foreach (Unit004EvolutionPage.SelectCell[] source in this.sources_.Values)
      {
        if (source.Length != 0 && ((IEnumerable<Unit004EvolutionPage.SelectCell>) source).First<Unit004EvolutionPage.SelectCell>().unit_.unit.IsNormalUnit)
        {
          foreach (Unit004EvolutionPage.SelectCell selectCell in source)
          {
            Unit004EvolutionPage.SelectCell ic = selectCell;
            ic.setUnit(((IEnumerable<PlayerUnit>) playerUnits).First<PlayerUnit>((Func<PlayerUnit, bool>) (pu => pu.id == ic.unit_.id)));
            Unit004EvolutionPage.UnitCondition unitCondition = Unit004EvolutionPage.getUnitCondition(ic.unit_, decks);
            if (ic.condition_ != unitCondition)
            {
              if (ic.condition_ == Unit004EvolutionPage.UnitCondition.Selected)
              {
                if (unitCondition == Unit004EvolutionPage.UnitCondition.Favarite || unitCondition == Unit004EvolutionPage.UnitCondition.Organized)
                {
                  this.setUnselected(ic.column_);
                  ic.setCondition(unitCondition);
                  flag = true;
                }
              }
              else
              {
                ic.setCondition(unitCondition);
                flag = true;
              }
            }
          }
        }
      }
      return flag;
    }
  }
}
