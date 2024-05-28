// Decompiled with JetBrains decompiler
// Type: DetailMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using EquipmentRules;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitDetails;
using UnityEngine;

#nullable disable
public class DetailMenu : DetailMenuBase
{
  public const int GearSlot1 = 0;
  public const int GearSlot2 = 1;
  public const int GearSlot3 = 2;
  public const int GearSlotNum = 3;
  private const int maxLoadTimesBeforeGC = 10;
  private static int currentLoadTimes = 0;
  private Vector3 modelPos;
  [SerializeField]
  protected UI2DSprite DynCharacter;
  public bool isFavorited;
  [SerializeField]
  private GameObject dirFavorite;
  [SerializeField]
  private GameObject btnFavoritedOff;
  [SerializeField]
  private GameObject btnFavoritedOn;
  [SerializeField]
  private GameObject btnIntimacy;
  [SerializeField]
  private GameObject dirMainWeapon;
  [SerializeField]
  private GameObject link_MainWeapon;
  [SerializeField]
  private GameObject dirMainWeapon2;
  [SerializeField]
  private GameObject link_MainWeapon2;
  [SerializeField]
  private GameObject slc_MainWeapon2_NonBase;
  [SerializeField]
  private GameObject dirBuguLock2;
  [SerializeField]
  private EffectBuguSlotLock effectBuguLock2;
  [SerializeField]
  private GameObject dirMainWeapon3;
  [SerializeField]
  private GameObject link_MainWeapon3;
  [SerializeField]
  private GameObject slc_MainWeapon3_NonBase;
  [SerializeField]
  private GameObject dirBuguLock3;
  [SerializeField]
  private EffectBuguSlotLock effectBuguLock3;
  [SerializeField]
  private GameObject dirMainWeaponMulti;
  [SerializeField]
  private GameObject[] link_MainWeaponMulti;
  private ItemIcon[] gearIcons;
  [SerializeField]
  private UnitDetailGroupButton[] groupBtns;
  [SerializeField]
  private SpriteSelectDirectButton[] groupSprites;
  [SerializeField]
  private UIGrid groupBtnGrid;
  [SerializeField]
  private NGHorizontalScrollParts informationScrollView;
  [SerializeField]
  private DetailMenuScrollViewBase[] informationPanels;
  [SerializeField]
  private GameObject dirSpecial;
  [SerializeField]
  private UISprite specialIcon;
  [SerializeField]
  private UILabel specialFactor;
  [SerializeField]
  private UILabel specialEventName;
  [SerializeField]
  private GameObject floatingSpecialPointDialog;
  private Unit0042FloatingSpecialPointDialog floatingSpecialPointDialogObject;
  [SerializeField]
  private UISprite slcCountry;
  [SerializeField]
  private UI2DSprite slcInclusion;
  [SerializeField]
  private GameObject floatingGroupDialog;
  private GameObject floatingGroupDialogObject;
  private static readonly string specialIconSpriteBaseName = "slc_icon_specific_effectiveness_{0}.png__GUI__unit_detail{1}__unit_detail{1}_prefab";
  private PlayerItem[] equippedGears;
  private PlayerItem[] equippedReisous;
  private PlayerUnit _playerUnit;
  private bool isLimit;
  private GameObject jobAbilityDetailPrefab;
  private GameObject jobAbilityUpPrefab;
  private GameObject jobAbilityDialogPrefab;
  private bool isModelLoading;
  private bool jingiBgSet;
  private NGSoundManager sm;
  public bool isEarthMode;
  public bool isMemory;
  [SerializeField]
  private UIButton IbtnUnitTraining;
  private IEnumerator doBottomInital_;
  [SerializeField]
  [Tooltip("主スクロール用Collider")]
  private BoxCollider[] mainScrollColliders;
  [SerializeField]
  [Tooltip("下部スクロール用Collider")]
  private BoxCollider[] bottomScrollColliders;
  [SerializeField]
  private EffectCharacterCrossFade crossFade_;
  private Action actionUpdate;
  private Transform trsTop;
  private Transform trsBottom;
  private int imageReferenceID_;
  private int imageJobID_;
  private bool deleteAnimationFirstReset_ = true;
  private Unit0042PickupMenu pickupMenu_;
  private List<int> deleteIndex_ = new List<int>(1) { 0 };

  private bool AwakeUnit
  {
    get => !(this._playerUnit == (PlayerUnit) null) && this._playerUnit.unit.awake_unit_flag;
  }

  public Vector3 ModelPos
  {
    set => this.modelPos = value;
    get => this.modelPos;
  }

  public NGHorizontalScrollParts InformationScrollView => this.informationScrollView;

  public PlayerUnit PlayerUnit => this._playerUnit;

  public PlayerUnit baseUnit { get; private set; }

  public PlayerItem EquippedGearSlot1
  {
    get => this.equippedGears != null ? this.equippedGears[0] : (PlayerItem) null;
  }

  public PlayerItem EquippedGearSlot2
  {
    get => this.equippedGears != null ? this.equippedGears[1] : (PlayerItem) null;
  }

  public PlayerItem EquippedReisouSlot1 => this.equippedReisous[0];

  public PlayerItem EquippedReisouSlot2 => this.equippedReisous[1];

  public bool isWaitBottomInitializing { get; private set; }

  public bool isPurgedBottom { get; private set; }

  public IEnumerator getBottomInitial()
  {
    IEnumerator doBottomInital = this.doBottomInital_;
    this.doBottomInital_ = (IEnumerator) null;
    return doBottomInital;
  }

  private void deleteMainScrollColliders()
  {
    if (this.mainScrollColliders == null)
      return;
    for (int index = 0; index < this.mainScrollColliders.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.mainScrollColliders[index], (Object) null))
      {
        UIDragScrollView component = ((Component) this.mainScrollColliders[index]).GetComponent<UIDragScrollView>();
        Object.Destroy((Object) this.mainScrollColliders[index]);
        if (Object.op_Inequality((Object) component, (Object) null))
          Object.Destroy((Object) component);
      }
    }
    this.mainScrollColliders = (BoxCollider[]) null;
  }

  private void deleteBottomScrollColliders()
  {
    if (this.bottomScrollColliders == null)
      return;
    for (int index = 0; index < this.bottomScrollColliders.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.bottomScrollColliders[index], (Object) null))
      {
        UIDragScrollView component = ((Component) this.bottomScrollColliders[index]).GetComponent<UIDragScrollView>();
        Object.Destroy((Object) this.bottomScrollColliders[index]);
        if (Object.op_Inequality((Object) component, (Object) null))
          Object.Destroy((Object) component);
      }
    }
    this.bottomScrollColliders = (BoxCollider[]) null;
  }

  public void IbtnIntimacy()
  {
    if (this.IsPushAndSet())
      return;
    if (this.isEarthMode)
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Earth;
      Singleton<NGSceneManager>.GetInstance().changeScene("unit054_2_2", true, (object) this._playerUnit);
    }
    else
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
      Singleton<NGSceneManager>.GetInstance().changeScene("unit004_2_2", true, (object) this._playerUnit);
    }
  }

  public void IbtnZoom()
  {
    if (this.IsPushAndSet())
      return;
    if (this.isEarthMode)
      Unit0543Scene.changeScene(true, this._playerUnit);
    else
      Unit0043Scene.changeScene(true, this._playerUnit, false);
  }

  public void IbtnWpEquip(int index)
  {
    if (this.IsPushAndSet() || this.isLimit)
      return;
    if (this.isEarthMode)
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Earth;
      Unit0544Scene.ChangeScene(true, this._playerUnit, 1);
    }
    else
    {
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
        Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
      Unit0044Scene.ChangeScene(true, this._playerUnit, index);
    }
  }

  public void IbtnFavoriteToggle() => this.SetFavorite(!this.isFavorited);

  public void SetFavorite(bool active)
  {
    if (!this.isEarthMode)
      return;
    this.isFavorited = active;
    this.btnFavoritedOff.SetActive(!active);
    this.btnFavoritedOn.SetActive(active);
    this.menu.UpdateSetting(this._playerUnit.id, this.isFavorited);
  }

  public void IbtnStatusDetail() => this.StartCoroutine(this.openStatusDetail(this._playerUnit));

  private IEnumerator openStatusDetail(PlayerUnit playerUnit)
  {
    DetailMenu detailMenu = this;
    GameObject pop = Singleton<PopupManager>.GetInstance().open(detailMenu.menu.StatusDetailPrefab, isNonOpenAnime: true);
    Unit004StatusDetailDialog dialog = pop.GetComponent<Unit004StatusDetailDialog>();
    ((Component) dialog).gameObject.SetActive(false);
    Future<Sprite> futureSprite = playerUnit.unit.LoadSpriteLarge(playerUnit.job_id, 1f);
    IEnumerator e = futureSprite.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    dialog.Initialize(detailMenu.PlayerUnit, futureSprite.Result, detailMenu.isMemory);
    Singleton<PopupManager>.GetInstance().startOpenAnime(pop);
  }

  public void IbtnTraining()
  {
    if (this.menu.IsPushAndSet())
      return;
    this.StartCoroutine(this.doChangeTraining());
  }

  private IEnumerator doChangeTraining()
  {
    DetailMenu detailMenu = this;
    if (Object.op_Inequality((Object) detailMenu.menu, (Object) null))
    {
      IEnumerator e = detailMenu.menu.doUploadFavorites();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    // ISSUE: reference to a compiler-generated method
    Unit004TrainingScene.changeCombine(true, Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), new Predicate<PlayerUnit>(detailMenu.\u003CdoChangeTraining\u003Eb__101_0)), exceptionBackScene: (Action) (() =>
    {
      Unit0042Menu.ResetCharacterQuests();
      if (Singleton<NGSceneManager>.GetInstance().SceneStack.Peek().args[2] is Unit0042Scene.BootParam bootParam2 && DetailMenu.IsLessUnitList(bootParam2.playerUnits))
        bootParam2.playerUnits = DetailMenu.CreateUpdatedUnitList(bootParam2.playerUnits);
      NGMenuBase.backSceneBase();
    }));
  }

  public static bool IsLessUnitList(PlayerUnit[] unitList)
  {
    PlayerUnit[] playerUnitArray = SMManager.Get<PlayerUnit[]>();
    return unitList != null && unitList.Length > playerUnitArray.Length;
  }

  public static PlayerUnit[] CreateUpdatedUnitList(PlayerUnit[] oldUnitList)
  {
    PlayerUnit[] array = SMManager.Get<PlayerUnit[]>();
    int length = oldUnitList.Length < array.Length ? oldUnitList.Length : array.Length;
    PlayerUnit[] updatedUnitList = new PlayerUnit[length];
    int index1 = 0;
    for (int index2 = 0; index1 < oldUnitList.Length && index2 < length; ++index1)
    {
      int tid = oldUnitList[index1].id;
      PlayerUnit playerUnit = Array.Find<PlayerUnit>(array, (Predicate<PlayerUnit>) (x => x.id == tid));
      if (playerUnit != (PlayerUnit) null)
      {
        updatedUnitList[index2] = playerUnit;
        ++index2;
      }
    }
    return updatedUnitList;
  }

  public void IbtnUnitQuest()
  {
    if (this.menu.IsPushAndSet())
      return;
    this.menu.showCharacterQuests(this._playerUnit);
  }

  public void IbtnJobTokusei0() => this.StartCoroutine(this.openTokusei(0));

  public void IbtnJobTokusei1() => this.StartCoroutine(this.openTokusei(1));

  public void IbtnJobTokusei2() => this.StartCoroutine(this.openTokusei(2));

  public void IbtnJobTokusei3() => this.StartCoroutine(this.openTokusei(3));

  public void setJobAbility(
    DetailMenuJobAbilityParts parts,
    PlayerUnitJob_abilities jobAbility,
    bool bDiff = false,
    bool bViewMode = false)
  {
    if (jobAbility == null)
    {
      ((Component) parts).gameObject.SetActive(false);
    }
    else
    {
      bool enabledLevelUp = !bViewMode && !this.isLimit && !this._playerUnit.is_storage && jobAbility.current_levelup_pattern != null;
      if (parts.Initialize(jobAbility, bDiff, enabledLevelUp))
      {
        ((Component) parts).gameObject.SetActive(true);
        EventDelegate.Set(((Component) parts).GetComponent<UIButton>().onClick, (EventDelegate.Callback) (() => this.StartCoroutine(this.doOpenJobAbility(jobAbility, enabledLevelUp))));
        if (!parts.hasOnClickLevelUp)
          return;
        if (enabledLevelUp)
          EventDelegate.Set(parts.onClickLevelUp, (EventDelegate.Callback) (() => this.StartCoroutine(this.doOpenJobAbilityLevelUp(jobAbility))));
        else
          parts.onClickLevelUp.Clear();
      }
      else
        ((Component) parts).gameObject.SetActive(false);
    }
  }

  private IEnumerator openTokusei(int range)
  {
    PlayerUnitJob_abilities[] jobAbilities = this._playerUnit.job_abilities;
    if (jobAbilities.Length != 0 && jobAbilities.Length > range && jobAbilities[range].master != null)
    {
      PlayerUnitJob_abilities jobAbility = jobAbilities[range];
      IEnumerator e = this.doOpenJobAbility(jobAbility, !this.isLimit && !this._playerUnit.is_storage && jobAbility.current_levelup_pattern != null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public Action<Action> preUpdateJobAblility { get; set; }

  public Action updatedJobAbility { get; set; }

  private IEnumerator doOpenJobAbility(PlayerUnitJob_abilities jobAbility, bool enabledLevelUp)
  {
    if (!Singleton<NGSoundManager>.GetInstance().IsVoiceStopAll())
      Singleton<NGSoundManager>.GetInstance().StopVoice(time: 0.0f);
    Future<GameObject> loader;
    IEnumerator e;
    GameObject popup;
    if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      if (Object.op_Equality((Object) this.jobAbilityDetailPrefab, (Object) null))
      {
        loader = new ResourceObject("Prefabs/unit004_2/popup_Detail_VertexFactor").Load<GameObject>();
        e = loader.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.jobAbilityDetailPrefab = loader.Result;
        loader = (Future<GameObject>) null;
      }
      if (enabledLevelUp)
      {
        popup = Singleton<PopupManager>.GetInstance().open(this.jobAbilityDetailPrefab, isNonSe: true, isNonOpenAnime: true);
        e = popup.GetComponent<Unit004JobAbilityDetail>().Init(this._playerUnit, jobAbility, true, this.updatedJobAbility, eventPreUpdateJobAbility: this.preUpdateJobAblility);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Singleton<PopupManager>.GetInstance().startOpenAnime(popup);
        popup = (GameObject) null;
      }
      else
      {
        popup = Singleton<PopupManager>.GetInstance().open(this.jobAbilityDetailPrefab, isNonSe: true, isNonOpenAnime: true);
        e = popup.GetComponent<Unit004JobAbilityDetail>().Init(this._playerUnit, jobAbility, false, (Action) null);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Singleton<PopupManager>.GetInstance().startOpenAnime(popup);
        popup = (GameObject) null;
      }
    }
    else if (enabledLevelUp)
    {
      e = this.doOpenJobAbilityLevelUp(jobAbility);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      if (Object.op_Equality((Object) this.jobAbilityDialogPrefab, (Object) null))
      {
        loader = Res.Prefabs.unit004_Job.Unit_JobCharacteristic_Dialog.Load<GameObject>();
        yield return (object) loader.Wait();
        this.jobAbilityDialogPrefab = loader.Result;
        loader = (Future<GameObject>) null;
      }
      popup = Singleton<PopupManager>.GetInstance().open(this.jobAbilityDialogPrefab);
      popup.SetActive(false);
      e = popup.GetComponent<Unit004JobDialog>().Init(jobAbility);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      popup = (GameObject) null;
    }
  }

  private IEnumerator doOpenJobAbilityLevelUp(PlayerUnitJob_abilities jobAbility)
  {
    IEnumerator e;
    if (Object.op_Equality((Object) this.jobAbilityUpPrefab, (Object) null))
    {
      Future<GameObject> prefab = !Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.unit004_Job.Unit_JobCharacteristic_UP_Dialog.Load<GameObject>() : Res.Prefabs.unit004_Job.Unit_JobCharacteristic_UP_Dialog_sea.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.jobAbilityUpPrefab = prefab.Result;
      prefab = (Future<GameObject>) null;
    }
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.jobAbilityUpPrefab, isNonSe: true, isNonOpenAnime: true);
    e = popup.GetComponent<Unit004JobDialogUp>().Init(this._playerUnit, jobAbility, this.updatedJobAbility, eventPreUpdateJobAbility: this.preUpdateJobAblility);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(popup);
  }

  private void setUpdate(Action actUpdate) => this.actionUpdate = actUpdate;

  private void Update()
  {
    if (this.actionUpdate == null)
      this.setUpdate(new Action(this.defaultUpdate));
    this.actionUpdate();
  }

  private void defaultUpdate()
  {
    if (this.isWaitBottomInitializing || Object.op_Equality((Object) this.menu, (Object) null) || this.menu.isWaitStatusPanelInitializing)
      return;
    if (this.menu.CurrentIndex == this.index)
    {
      this.informationScrollView.SeEnable = true;
      if (this.menu.InfoIndex == this.informationScrollView.selected)
        return;
      this.menu.InfoIndex = Math.Max(this.informationScrollView.selected, 0);
      this.menu.UpdateInfoIndicator(this.menu.InfoIndex);
    }
    else
      this.informationScrollView.SeEnable = false;
  }

  private void Update3DModelViewActive()
  {
    if (this.isModelLoading || Object.op_Equality((Object) this.menu, (Object) null) || !this.isEarthMode)
      return;
    if (this.informationScrollView.selected != 0)
    {
      if (!Object.op_Inequality((Object) ((Component) this.informationPanels[0]).GetComponent<DetailMenuScrollView01>().UI3DModel, (Object) null))
        return;
      ((Component) ((Component) this.informationPanels[0]).GetComponent<DetailMenuScrollView01>().UI3DModel).gameObject.SetActive(false);
    }
    else
    {
      if (!Object.op_Inequality((Object) ((Component) this.informationPanels[0]).GetComponent<DetailMenuScrollView01>().UI3DModel, (Object) null))
        return;
      ((Component) ((Component) this.informationPanels[0]).GetComponent<DetailMenuScrollView01>().UI3DModel).gameObject.SetActive(true);
    }
  }

  public override IEnumerator SetInformationPanelIndex(int infoIndex)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DetailMenu detailMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (Object.op_Equality((Object) detailMenu.menu, (Object) null))
      return false;
    if (detailMenu.menu.CurrentIndex != detailMenu.index && infoIndex != detailMenu.informationScrollView.selected)
      detailMenu.informationScrollView.resetCenterItem(infoIndex);
    detailMenu.Update3DModelViewActive();
    return false;
  }

  public void ResetArmorSkillIcon()
  {
    ((Component) this.informationPanels[1]).GetComponent<DetailMenuScrollViewSkill>().ResetArmorSkillIcon();
  }

  private IEnumerator InitInformationPanels(
    PlayerUnit playerUnit,
    int infoIndex,
    bool isLimit,
    bool isMaterial,
    bool isUpdate = false)
  {
    DetailMenu detailMenu = this;
    List<GameObject[]> prefabs;
    if (detailMenu.isEarthMode)
      prefabs = new List<GameObject[]>()
      {
        new GameObject[1]{ detailMenu.menu.skillDetailDialogPrefab },
        new GameObject[4]
        {
          detailMenu.menu.skillDetailDialogPrefab,
          detailMenu.menu.gearKindIconPrefab,
          detailMenu.menu.profIconPrefab,
          detailMenu.menu.skillTypeIconPrefab
        },
        new GameObject[4]
        {
          detailMenu.menu.skillDetailDialogPrefab,
          detailMenu.menu.gearKindIconPrefab,
          detailMenu.menu.commonElementIconPrefab,
          detailMenu.menu.spAtkTypeIconPrefab
        },
        (GameObject[]) null,
        (GameObject[]) null
      };
    else
      prefabs = new List<GameObject[]>()
      {
        new GameObject[8]
        {
          detailMenu.menu.skillDetailDialogPrefab,
          detailMenu.menu.gearKindIconPrefab,
          detailMenu.menu.profIconPrefab,
          detailMenu.menu.unityDetailPrefabs[0].Result,
          detailMenu.menu.overkillersSlotReleasePrefab,
          detailMenu.menu.unitIconPrefab,
          detailMenu.menu.unityDetailPrefabs[1].Result,
          detailMenu.menu.levelDetailPrefab
        },
        new GameObject[3]
        {
          detailMenu.menu.skillDetailDialogPrefab,
          detailMenu.menu.skillTypeIconPrefab,
          detailMenu.menu.skillLockIconPrefab
        },
        new GameObject[3]
        {
          detailMenu.menu.skillfullnessIconPrefab,
          detailMenu.menu.specialPointDetailDialogPrefab,
          detailMenu.menu.terraiAbilityDialogPrefab
        },
        new GameObject[3]
        {
          detailMenu.menu.skillDetailDialogPrefab,
          detailMenu.menu.gearKindIconPrefab,
          detailMenu.menu.commonElementIconPrefab
        }
      };
    Unit0042Scene scene = ((Component) detailMenu.menu).gameObject.GetComponent<Unit0042Scene>();
    bool isOverkillersView = Object.op_Inequality((Object) scene, (Object) null) && scene.bootParam.controlFlags.IsOn(Control.OverkillersUnit);
    if (isOverkillersView)
    {
      detailMenu.deleteMainScrollColliders();
      detailMenu.deleteBottomScrollColliders();
    }
    DetailMenuScrollViewParam viewParam = (DetailMenuScrollViewParam) null;
    for (int i = 0; i < detailMenu.informationPanels.Length; ++i)
    {
      DetailMenuScrollViewBase informationPanel = detailMenu.informationPanels[i];
      informationPanel.isEarthMode = detailMenu.isEarthMode;
      informationPanel.isMemory = detailMenu.isMemory;
      informationPanel.setControlFlags(scene);
      DetailMenuScrollViewParam menuScrollViewParam;
      if (Object.op_Inequality((Object) (menuScrollViewParam = informationPanel as DetailMenuScrollViewParam), (Object) null))
        menuScrollViewParam.changeTab(detailMenu.menu.viewParamTabMode);
      if (Object.op_Equality((Object) viewParam, (Object) null) && Object.op_Inequality((Object) menuScrollViewParam, (Object) null))
        viewParam = menuScrollViewParam;
      if (isOverkillersView && Object.op_Equality((Object) menuScrollViewParam, (Object) null))
      {
        if (((Component) informationPanel).gameObject.activeSelf)
          ((Component) informationPanel).gameObject.SetActive(false);
      }
      else if (informationPanel.Init(playerUnit, detailMenu.baseUnit))
      {
        if (!detailMenu.isPurgedBottom)
        {
          if (i == infoIndex)
          {
            IEnumerator e = informationPanel.initAsync(playerUnit, isLimit, isMaterial, prefabs[i]);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
            detailMenu.StartCoroutine(detailMenu.InitInformationPanelAsync(informationPanel, playerUnit, isLimit, isMaterial, prefabs[i]));
        }
        else
          yield return (object) informationPanel.initAsync(playerUnit, isLimit, isMaterial, prefabs[i]);
      }
    }
    detailMenu.informationScrollView.resetCenterItem(infoIndex);
    if (isOverkillersView)
    {
      ((Behaviour) detailMenu.informationScrollView.uiScrollView).enabled = false;
      detailMenu.informationScrollView.leftArrow.SetActive(false);
      detailMenu.informationScrollView.rightArrow.SetActive(false);
      detailMenu.informationScrollView.disabledClipping();
      if (Object.op_Inequality((Object) detailMenu.informationScrollView.dot, (Object) null))
        ((Component) detailMenu.informationScrollView.dot).gameObject.SetActive(false);
      if (Object.op_Inequality((Object) viewParam, (Object) null))
      {
        UIScrollView inParents = NGUITools.FindInParents<UIScrollView>(((Component) detailMenu).transform.parent);
        foreach (UIDragScrollView componentsInChild in ((Component) viewParam).GetComponentsInChildren<UIDragScrollView>(true))
          componentsInChild.scrollView = inParents;
      }
    }
    if (detailMenu.isEarthMode)
    {
      detailMenu.isModelLoading = true;
      // ISSUE: reference to a compiler-generated method
      yield return (object) ((Component) detailMenu.informationPanels[0]).GetComponent<DetailMenuScrollView01>().setModel(playerUnit, detailMenu.menu.modelPrefab, detailMenu.ModelPos, detailMenu.menu.LightON, new Action(detailMenu.\u003CInitInformationPanels\u003Eb__128_0));
    }
  }

  private IEnumerator InitInformationPanelAsync(
    DetailMenuScrollViewBase panel,
    PlayerUnit playerUnit,
    bool isLimit,
    bool isMaterial,
    GameObject[] prefabs)
  {
    yield return (object) new WaitWhile((Func<bool>) (() => Singleton<NGSceneManager>.GetInstance().IsDangerAsyncLoadResource()));
    yield return (object) panel.initAsync(playerUnit, isLimit, isMaterial, prefabs);
  }

  private IEnumerator ItemGearFavoriteAscync(
    int[] favorite_player_gear_ids,
    int[] un_favorite_player_gear_ids)
  {
    IEnumerator e = WebAPI.ItemGearFavorite(favorite_player_gear_ids, un_favorite_player_gear_ids).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onEndSceneAsync()
  {
    if (Object.op_Inequality((Object) this.sm, (Object) null))
    {
      this.sm.stopVoice();
      yield break;
    }
  }

  private void DisplayGroupLogo(UnitUnit playerUnit)
  {
    if (!this.isEarthMode)
      return;
    int index = 0;
    ((IEnumerable<UnitDetailGroupButton>) this.groupBtns).ForEach<UnitDetailGroupButton>((Action<UnitDetailGroupButton>) (x => ((Component) x).gameObject.SetActive(false)));
    UnitGroup groupInfo = ((IEnumerable<UnitGroup>) MasterData.UnitGroupList).FirstOrDefault<UnitGroup>((Func<UnitGroup, bool>) (x => x.unit_id == playerUnit.ID));
    if (groupInfo == null)
      return;
    UnitGroupLargeCategory groupLargeCategory = ((IEnumerable<UnitGroupLargeCategory>) MasterData.UnitGroupLargeCategoryList).FirstOrDefault<UnitGroupLargeCategory>((Func<UnitGroupLargeCategory, bool>) (x => x.ID == groupInfo.group_large_category_id_UnitGroupLargeCategory));
    UnitGroupSmallCategory groupSmallCategory = ((IEnumerable<UnitGroupSmallCategory>) MasterData.UnitGroupSmallCategoryList).FirstOrDefault<UnitGroupSmallCategory>((Func<UnitGroupSmallCategory, bool>) (x => x.ID == groupInfo.group_small_category_id_UnitGroupSmallCategory));
    UnitGroupClothingCategory clothingCategory1 = ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).FirstOrDefault<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x => x.ID == groupInfo.group_clothing_category_id_UnitGroupClothingCategory));
    UnitGroupClothingCategory clothingCategory2 = ((IEnumerable<UnitGroupClothingCategory>) MasterData.UnitGroupClothingCategoryList).FirstOrDefault<UnitGroupClothingCategory>((Func<UnitGroupClothingCategory, bool>) (x => x.ID == groupInfo.group_clothing_category_id_2_UnitGroupClothingCategory));
    UnitGroupGenerationCategory generationCategory = ((IEnumerable<UnitGroupGenerationCategory>) MasterData.UnitGroupGenerationCategoryList).FirstOrDefault<UnitGroupGenerationCategory>((Func<UnitGroupGenerationCategory, bool>) (x => x.ID == groupInfo.group_generation_category_id_UnitGroupGenerationCategory));
    if (groupLargeCategory == null && groupSmallCategory == null && clothingCategory1 == null && clothingCategory2 == null && generationCategory == null)
      return;
    if (groupLargeCategory != null && groupLargeCategory.ID != 1)
    {
      this.groupSprites[index].SetSpriteName<string>(groupLargeCategory.GetSpriteName(), true);
      ((Component) this.groupBtns[index]).gameObject.SetActive(true);
      ++index;
    }
    if (groupSmallCategory != null && groupSmallCategory.ID != 1)
    {
      this.groupSprites[index].SetSpriteName<string>(groupSmallCategory.GetSpriteName(), true);
      ((Component) this.groupBtns[index]).gameObject.SetActive(true);
      ++index;
    }
    if (clothingCategory1 != null && clothingCategory1.ID != 1)
    {
      this.groupSprites[index].SetSpriteName<string>(clothingCategory1.GetSpriteName(), true);
      ((Component) this.groupBtns[index]).gameObject.SetActive(true);
      ++index;
    }
    if (clothingCategory2 != null && clothingCategory2.ID != 1)
    {
      this.groupSprites[index].SetSpriteName<string>(clothingCategory2.GetSpriteName(), true);
      ((Component) this.groupBtns[index]).gameObject.SetActive(true);
      ++index;
    }
    if (generationCategory != null && generationCategory.ID != 1)
    {
      this.groupSprites[index].SetSpriteName<string>(generationCategory.GetSpriteName(), true);
      ((Component) this.groupBtns[index]).gameObject.SetActive(true);
      int num = index + 1;
    }
    this.groupBtnGrid.repositionNow = true;
  }

  public override IEnumerator Init(
    Unit0042Menu menu,
    int index,
    PlayerUnit playerUnit,
    int infoIndex,
    bool isLimit,
    bool isMaterial,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus,
    bool isUpdate = true,
    PlayerUnit baseUnit = null)
  {
    DetailMenu detailMenu = this;
    playerUnit.resetOnceOverkillers();
    detailMenu.menu = menu;
    detailMenu.index = index;
    detailMenu._playerUnit = playerUnit;
    detailMenu.resetEquipments();
    detailMenu.isLimit = isLimit;
    detailMenu.baseUnit = baseUnit;
    detailMenu.repositionTopBottom(menu.scrollView.scrollView.panel.GetViewSize());
    yield return (object) detailMenu.LoadCharacterImage(playerUnit);
    detailMenu.isWaitBottomInitializing = true;
    detailMenu.informationScrollView.SeEnable = false;
    if (detailMenu.isPurgedBottom)
      detailMenu.doBottomInital_ = detailMenu.doInitStatusPanel(true, infoIndex, isMaterial, tables, unitBonus, isUpdate);
    else
      yield return (object) detailMenu.doInitStatusPanel(false, infoIndex, isMaterial, tables, unitBonus, isUpdate);
  }

  private void resetEquipments()
  {
    this.gearIcons = new ItemIcon[3];
    this.equippedGears = new PlayerItem[3];
    this.equippedGears[0] = this._playerUnit.equippedGear;
    this.equippedGears[1] = this._playerUnit.equippedGear2;
    this.equippedGears[2] = this._playerUnit.equippedGear3;
    this.equippedReisous = new PlayerItem[3];
    this.equippedReisous[0] = this._playerUnit.equippedReisou;
    this.equippedReisous[1] = this._playerUnit.equippedReisou2;
    this.equippedReisous[2] = this._playerUnit.equippedReisou3;
  }

  private IEnumerator doInitStatusPanel(
    bool bInitAlpha,
    int infoIndex,
    bool isMaterial,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus,
    bool isUpdate)
  {
    DetailMenu detailMenu = this;
    if (bInitAlpha)
    {
      foreach (UITweener component in ((Component) detailMenu.trsBottom).GetComponents<UITweener>())
      {
        component.tweenFactor = 0.0f;
        ((Behaviour) component).enabled = false;
      }
      ((Component) detailMenu.trsBottom).GetComponent<UIRect>().alpha = 0.0f;
      ((Component) detailMenu.trsBottom).GetComponent<NGTweenParts>().resetActive(true);
    }
    if (Object.op_Inequality((Object) detailMenu.floatingSpecialPointDialog, (Object) null))
    {
      if (Object.op_Equality((Object) detailMenu.floatingSpecialPointDialogObject, (Object) null))
        detailMenu.floatingSpecialPointDialogObject = detailMenu.menu.specialPointDetailDialogPrefab.Clone(detailMenu.floatingSpecialPointDialog.transform).GetComponentInChildren<Unit0042FloatingSpecialPointDialog>();
      ((Component) ((Component) detailMenu.floatingSpecialPointDialogObject).transform.parent).gameObject.SetActive(false);
    }
    IEnumerator e = detailMenu.InitPlayer(detailMenu._playerUnit, infoIndex, detailMenu.menu.gearIconPrefab, tables, unitBonus);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = detailMenu.InitInformationPanels(detailMenu._playerUnit, infoIndex, detailMenu.isLimit, isMaterial, isUpdate);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) detailMenu.floatingGroupDialog, (Object) null))
    {
      if (Object.op_Equality((Object) detailMenu.floatingGroupDialogObject, (Object) null))
        detailMenu.floatingGroupDialogObject = detailMenu.menu.GroupDetailDialogPrefab.Clone(detailMenu.floatingGroupDialog.transform);
      detailMenu.floatingGroupDialogObject.SetActive(false);
      detailMenu.DisplayGroupLogo(detailMenu._playerUnit.unit);
    }
    if (detailMenu.isLimit)
      detailMenu.LimitMode();
    if ((isMaterial || detailMenu._playerUnit.is_storage || detailMenu.isLimit) && Object.op_Inequality((Object) detailMenu.IbtnUnitTraining, (Object) null))
      ((Component) detailMenu.IbtnUnitTraining).gameObject.SetActive(false);
    if (detailMenu._playerUnit.is_storage)
      detailMenu.LimitStorageMode();
    if (Object.op_Inequality((Object) detailMenu.slcCountry, (Object) null))
    {
      ((Component) detailMenu.slcCountry).gameObject.SetActive(false);
      if (detailMenu._playerUnit.unit.country_attribute.HasValue)
      {
        ((Component) detailMenu.slcCountry).gameObject.SetActive(true);
        detailMenu._playerUnit.unit.SetCuntrySpriteName(ref detailMenu.slcCountry);
      }
    }
    if (Object.op_Inequality((Object) detailMenu.slcInclusion, (Object) null))
    {
      ((Component) detailMenu.slcInclusion).gameObject.SetActive(false);
      if (detailMenu._playerUnit.unit.inclusion_ip.HasValue)
      {
        ((Component) detailMenu.slcInclusion).gameObject.SetActive(true);
        e = detailMenu._playerUnit.unit.SetInclusionIP(detailMenu.slcInclusion);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    detailMenu.isWaitBottomInitializing = false;
  }

  public GameObject purgeStatusPanel(Vector2 scrollViewPanelSize, Transform parent)
  {
    this.isPurgedBottom = true;
    this.repositionTopBottom(scrollViewPanelSize);
    this.trsBottom.parent = parent;
    this.trsBottom.localPosition = new Vector3(0.0f, this.trsBottom.localPosition.y, 0.0f);
    GameObject gameObject = ((Component) this.trsBottom).gameObject;
    gameObject.GetComponent<NGTweenParts>().resetActive(false);
    return gameObject;
  }

  private void repositionTopBottom(Vector2 scrollViewPanelSize)
  {
    if (Object.op_Equality((Object) this.trsTop, (Object) null))
    {
      this.trsTop = ((Component) this).transform.GetChildInFind("Top");
      this.trsTop.localPosition = new Vector3(0.0f, (float) (((double) scrollViewPanelSize.y - (double) ((Component) this.trsTop).GetComponent<UIWidget>().height) / 2.0), 0.0f);
    }
    if (!Object.op_Equality((Object) this.trsBottom, (Object) null))
      return;
    this.trsBottom = ((Component) this).transform.GetChildInFind("Bottom__anim_fade01");
    this.trsBottom.localPosition = new Vector3(0.0f, (float) -(((double) scrollViewPanelSize.y - (double) ((Component) this.trsBottom).GetComponent<UIWidget>().height) / 2.0), 0.0f);
  }

  private IEnumerator InitPlayer(
    PlayerUnit playerUnit,
    int infoIndex,
    GameObject gearIconPrefab,
    QuestScoreBonusTimetable[] tables,
    UnitBonus[] unitBonus)
  {
    DetailMenu detailMenu = this;
    if (!detailMenu.isEarthMode)
      detailMenu.SetFavorite(detailMenu.menu.GetSetting(playerUnit.id));
    if (Object.op_Inequality((Object) detailMenu.link_MainWeapon, (Object) null))
      detailMenu.link_MainWeapon.transform.GetChildren().ForEach<Transform>((Action<Transform>) (transform => Object.Destroy((Object) ((Component) transform).gameObject)));
    if (Object.op_Inequality((Object) detailMenu.link_MainWeapon2, (Object) null))
      detailMenu.link_MainWeapon2.transform.GetChildren().ForEach<Transform>((Action<Transform>) (transform => Object.Destroy((Object) ((Component) transform).gameObject)));
    if (Object.op_Inequality((Object) detailMenu.link_MainWeapon3, (Object) null))
      detailMenu.link_MainWeapon3.transform.GetChildren().ForEach<Transform>((Action<Transform>) (transform => Object.Destroy((Object) ((Component) transform).gameObject)));
    if (detailMenu.link_MainWeaponMulti != null)
    {
      for (int index = 0; index < detailMenu.link_MainWeaponMulti.Length; ++index)
        detailMenu.link_MainWeaponMulti[index].transform.GetChildren().ForEach<Transform>((Action<Transform>) (transform => Object.Destroy((Object) ((Component) transform).gameObject)));
    }
    IEnumerator e;
    if (detailMenu.isEarthMode)
    {
      if (Object.op_Inequality((Object) detailMenu.dirMainWeaponMulti, (Object) null))
      {
        detailMenu.dirMainWeapon.SetActive(!detailMenu.AwakeUnit);
        detailMenu.dirMainWeaponMulti.SetActive(detailMenu.AwakeUnit);
      }
      else if (Object.op_Inequality((Object) detailMenu.dirMainWeapon, (Object) null))
        detailMenu.dirMainWeapon.SetActive(true);
      if (detailMenu.AwakeUnit)
      {
        e = detailMenu.SetGearIconMulti(gearIconPrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = detailMenu.SetGearIconSingle(gearIconPrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else
    {
      if (Object.op_Inequality((Object) detailMenu.dirMainWeapon, (Object) null))
        detailMenu.dirMainWeapon.SetActive(true);
      if (Object.op_Inequality((Object) detailMenu.dirMainWeapon2, (Object) null))
        detailMenu.dirMainWeapon2.SetActive(detailMenu.AwakeUnit);
      if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon2_NonBase, (Object) null))
        detailMenu.slc_MainWeapon2_NonBase.SetActive(!detailMenu.AwakeUnit);
      if (detailMenu.isMine(detailMenu._playerUnit))
      {
        if (detailMenu._playerUnit.isPossibleEquippedGear3)
        {
          if (detailMenu.AwakeUnit)
          {
            if (Object.op_Inequality((Object) detailMenu.dirMainWeapon3, (Object) null))
              detailMenu.dirMainWeapon3.SetActive(detailMenu._playerUnit.isOpenedEquippedGear3);
            if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon3_NonBase, (Object) null))
              detailMenu.slc_MainWeapon3_NonBase.SetActive(!detailMenu._playerUnit.isOpenedEquippedGear3);
            detailMenu.dirBuguLock2.SetActive(false);
            detailMenu.dirBuguLock3.SetActive(!detailMenu._playerUnit.isOpenedEquippedGear3);
          }
          else
          {
            if (Object.op_Inequality((Object) detailMenu.dirMainWeapon2, (Object) null))
              detailMenu.dirMainWeapon2.SetActive(detailMenu._playerUnit.isOpenedEquippedGear3);
            if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon2_NonBase, (Object) null))
              detailMenu.slc_MainWeapon2_NonBase.SetActive(!detailMenu._playerUnit.isOpenedEquippedGear3);
            if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon3_NonBase, (Object) null))
              detailMenu.slc_MainWeapon3_NonBase.SetActive(true);
            detailMenu.dirBuguLock2.SetActive(!detailMenu._playerUnit.isOpenedEquippedGear3);
            detailMenu.dirBuguLock3.SetActive(false);
          }
        }
        else
        {
          if (Object.op_Inequality((Object) detailMenu.dirMainWeapon3, (Object) null))
            detailMenu.dirMainWeapon3.SetActive(false);
          if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon3_NonBase, (Object) null))
            detailMenu.slc_MainWeapon3_NonBase.SetActive(true);
          detailMenu.dirBuguLock2.SetActive(false);
          detailMenu.dirBuguLock3.SetActive(false);
        }
      }
      else
      {
        if (detailMenu._playerUnit.equip_gear_ids != null)
        {
          int num = detailMenu.AwakeUnit ? 2 : 1;
          if (detailMenu._playerUnit.equip_gear_ids.Length >= 3 || detailMenu._playerUnit.equip_gear_ids.Length > num)
            num = detailMenu._playerUnit.equip_gear_ids.Length;
          switch (num)
          {
            case 1:
              if (Object.op_Inequality((Object) detailMenu.dirMainWeapon2, (Object) null))
                detailMenu.dirMainWeapon2.SetActive(false);
              if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon2_NonBase, (Object) null))
                detailMenu.slc_MainWeapon2_NonBase.SetActive(true);
              if (Object.op_Inequality((Object) detailMenu.dirMainWeapon3, (Object) null))
                detailMenu.dirMainWeapon3.SetActive(false);
              if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon3_NonBase, (Object) null))
              {
                detailMenu.slc_MainWeapon3_NonBase.SetActive(true);
                break;
              }
              break;
            case 2:
              if (Object.op_Inequality((Object) detailMenu.dirMainWeapon2, (Object) null))
                detailMenu.dirMainWeapon2.SetActive(true);
              if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon2_NonBase, (Object) null))
                detailMenu.slc_MainWeapon2_NonBase.SetActive(false);
              if (Object.op_Inequality((Object) detailMenu.dirMainWeapon3, (Object) null))
                detailMenu.dirMainWeapon3.SetActive(false);
              if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon3_NonBase, (Object) null))
              {
                detailMenu.slc_MainWeapon3_NonBase.SetActive(true);
                break;
              }
              break;
            case 3:
              if (Object.op_Inequality((Object) detailMenu.dirMainWeapon2, (Object) null))
                detailMenu.dirMainWeapon2.SetActive(true);
              if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon2_NonBase, (Object) null))
                detailMenu.slc_MainWeapon2_NonBase.SetActive(false);
              if (Object.op_Inequality((Object) detailMenu.dirMainWeapon3, (Object) null))
                detailMenu.dirMainWeapon3.SetActive(true);
              if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon3_NonBase, (Object) null))
              {
                detailMenu.slc_MainWeapon3_NonBase.SetActive(false);
                break;
              }
              break;
          }
        }
        else
        {
          if (Object.op_Inequality((Object) detailMenu.dirMainWeapon2, (Object) null))
            detailMenu.dirMainWeapon2.SetActive(false);
          if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon2_NonBase, (Object) null))
            detailMenu.slc_MainWeapon2_NonBase.SetActive(true);
          if (Object.op_Inequality((Object) detailMenu.dirMainWeapon3, (Object) null))
            detailMenu.dirMainWeapon3.SetActive(false);
          if (Object.op_Inequality((Object) detailMenu.slc_MainWeapon3_NonBase, (Object) null))
            detailMenu.slc_MainWeapon3_NonBase.SetActive(true);
        }
        detailMenu.dirBuguLock2.SetActive(false);
        detailMenu.dirBuguLock3.SetActive(false);
      }
      detailMenu.jingiBgSet = false;
      e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon, 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (detailMenu.isMine(detailMenu._playerUnit))
      {
        if (detailMenu._playerUnit.isOpenedEquippedGear3)
        {
          if (detailMenu.AwakeUnit)
          {
            e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon2, 1);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon3, 2);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
          {
            e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon2, 2);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        else if (detailMenu.AwakeUnit)
        {
          e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon2, 1);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else if (detailMenu._playerUnit.equip_gear_ids != null)
      {
        if (detailMenu.AwakeUnit)
        {
          e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon2, 1);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (detailMenu._playerUnit.equip_gear_ids.Length >= 3)
          {
            e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon3, 2);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        else if (detailMenu._playerUnit.equip_gear_ids.Length >= 2)
        {
          e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon2, 2);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          e = detailMenu.SetGearIconSlot(gearIconPrefab, detailMenu.link_MainWeapon2, 1);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
    if (Object.op_Inequality((Object) detailMenu.dirSpecial, (Object) null))
    {
      string str1 = playerUnit.SpecialEffectType((IEnumerable<QuestScoreBonusTimetable>) tables, (IEnumerable<UnitBonus>) unitBonus);
      bool flag = !string.IsNullOrEmpty(str1);
      detailMenu.dirSpecial.SetActive(flag);
      if (flag)
      {
        string str2 = "×" + playerUnit.SpecialEffectFactor((IEnumerable<QuestScoreBonusTimetable>) tables, (IEnumerable<UnitBonus>) unitBonus);
        string str3 = playerUnit.SpecialEffectEventName((IEnumerable<QuestScoreBonusTimetable>) tables, (IEnumerable<UnitBonus>) unitBonus);
        string iconSpriteName = string.Format(DetailMenu.specialIconSpriteBaseName, (object) str1, Singleton<NGGameDataManager>.GetInstance().IsSea ? (object) "_sea" : (object) "");
        if (Object.op_Inequality((Object) detailMenu.specialIcon, (Object) null))
          detailMenu.specialIcon.spriteName = iconSpriteName;
        if (Object.op_Inequality((Object) detailMenu.specialFactor, (Object) null))
          detailMenu.specialFactor.SetTextLocalize(str2);
        if (Object.op_Inequality((Object) detailMenu.specialEventName, (Object) null))
          detailMenu.specialEventName.SetTextLocalize(str3);
        if (Object.op_Inequality((Object) detailMenu.floatingSpecialPointDialogObject, (Object) null))
          detailMenu.floatingSpecialPointDialogObject.setData(str2, str3, iconSpriteName);
      }
    }
  }

  public void IbtnSpecialPoint() => this.floatingSpecialPointDialogObject.Show();

  private bool isMine(PlayerUnit unit)
  {
    Unit0042Scene component = ((Component) this.menu).gameObject.GetComponent<Unit0042Scene>();
    bool flag = Object.op_Inequality((Object) component, (Object) null) && component.bootParam.isBuguLock;
    return ((unit.is_enemy ? 1 : (unit.is_guest ? 1 : 0)) | (flag ? 1 : 0)) == 0 && Player.Current.id == unit.player_id;
  }

  private IEnumerator SetGearIconSingle(GameObject gearIconPrefab)
  {
    IEnumerator e = this.SetGearIconSlot(gearIconPrefab, this.link_MainWeapon, 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetGearIconSlot(GameObject gearIconPrefab, GameObject linkObject, int slot)
  {
    DetailMenu detailMenu = this;
    detailMenu.gearIcons[slot] = gearIconPrefab.CloneAndGetComponent<ItemIcon>(linkObject.transform);
    ((Component) detailMenu.gearIcons[slot]).transform.localPosition = Vector3.zero;
    bool myGear;
    IEnumerator e;
    if (detailMenu.equippedGears[slot] != (PlayerItem) null)
    {
      myGear = detailMenu._playerUnit != (PlayerUnit) null && Player.Current.id == detailMenu._playerUnit.player_id;
      e = detailMenu.gearIcons[slot].InitByPlayerItem(detailMenu.equippedGears[slot], detailMenu.equippedReisous[slot], myGear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (detailMenu.equippedGears[slot].broken)
        detailMenu.gearIcons[slot].Broken = true;
      detailMenu.gearIcons[slot].BottomModeValue = ItemIcon.BottomMode.Visible_wIconNone;
      if (detailMenu.menu.controlFlags.IsOn(Control.CustomDeck))
      {
        switch (slot)
        {
          case 1:
            detailMenu.gearIcons[slot].EnableLongPressEvent(true, (Action<ItemIcon>) (_ => Unit00443Scene.changeSceneCustomDeck(this._playerUnit.equippedGear2, this._playerUnit, this._playerUnit.equippedReisou2)));
            break;
          case 2:
            detailMenu.gearIcons[slot].EnableLongPressEvent(true, (Action<ItemIcon>) (_ => Unit00443Scene.changeSceneCustomDeck(this._playerUnit.equippedGear3, this._playerUnit, this._playerUnit.equippedReisou3)));
            break;
          default:
            detailMenu.gearIcons[slot].EnableLongPressEvent(true, (Action<ItemIcon>) (_ => Unit00443Scene.changeSceneCustomDeck(this._playerUnit.equippedGear, this._playerUnit, this._playerUnit.equippedReisou)));
            break;
        }
      }
      else if (!detailMenu.isLimit)
        detailMenu.gearIcons[slot].EnableLongPressEvent();
      else
        detailMenu.gearIcons[slot].DisableLongPressEvent();
      if (myGear)
        detailMenu.gearIcons[slot].resetExpireDate();
    }
    else
    {
      myGear = detailMenu.EquipGearJingiBg(slot);
      if (myGear)
        detailMenu.jingiBgSet = true;
      e = detailMenu.gearIcons[slot].InitForEquipGear(myGear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (detailMenu.menu.controlFlags.IsOff(Control.CustomDeck))
      {
        detailMenu.gearIcons[slot].setEquipJinkiPlus(myGear);
        detailMenu.gearIcons[slot].setEquipPlus(!myGear);
        detailMenu.gearIcons[slot].EnableLongPressEventEmptyGear(new Action<int>(detailMenu.IbtnWpEquip), slot + 1);
      }
      else
        detailMenu.gearIcons[slot].DisableLongPressEvent();
    }
    detailMenu.gearIcons[slot].onClick = !detailMenu.menu.controlFlags.IsOff(Control.CustomDeck) ? (Action<ItemIcon>) (_ => { }) : (Action<ItemIcon>) (_ => this.IbtnWpEquip(slot + 1));
  }

  private bool EquipGearJingiBg(int index)
  {
    if (!this.isMine(this._playerUnit) || this.isLimit)
      return false;
    int? indexJingiSpace = Gears.getIndexJingiSpace(((IEnumerable<PlayerItem>) this.equippedGears).Select<PlayerItem, GearGear>((Func<PlayerItem, GearGear>) (x => x?.gear)).ToArray<GearGear>(), this._playerUnit.unit.awake_unit_flag, true);
    if (indexJingiSpace.HasValue)
    {
      int num = index;
      int? nullable = indexJingiSpace;
      int valueOrDefault = nullable.GetValueOrDefault();
      if (num == valueOrDefault & nullable.HasValue)
        return true;
    }
    return false;
  }

  private IEnumerator SetGearIconMulti(GameObject gearIconPrefab)
  {
    DetailMenu detailMenu = this;
    PlayerItem[] gears = new PlayerItem[detailMenu.link_MainWeaponMulti.Length];
    PlayerItem[] reisous = new PlayerItem[detailMenu.link_MainWeaponMulti.Length];
    gears[0] = detailMenu.EquippedGearSlot1;
    gears[1] = detailMenu.EquippedGearSlot2;
    reisous[0] = detailMenu.EquippedReisouSlot1;
    reisous[1] = detailMenu.EquippedReisouSlot2;
    for (int i = 0; i < detailMenu.link_MainWeaponMulti.Length; ++i)
    {
      detailMenu.gearIcons[i] = gearIconPrefab.CloneAndGetComponent<ItemIcon>(detailMenu.link_MainWeaponMulti[i].transform);
      ((Component) detailMenu.gearIcons[i]).transform.localPosition = Vector3.zero;
      IEnumerator e;
      if (gears[i] != (PlayerItem) null)
      {
        e = detailMenu.gearIcons[i].InitByPlayerItem(gears[i], reisous[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (gears[i].broken)
          detailMenu.gearIcons[i].Broken = true;
      }
      else
      {
        e = detailMenu.gearIcons[i].InitForEquipGear();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        detailMenu.gearIcons[i].setEquipPlus(true);
        detailMenu.gearIcons[i].EnableLongPressEventEmptyGear(new Action<int>(detailMenu.IbtnWpEquip), i + 1);
      }
      switch (i)
      {
        case 0:
          // ISSUE: reference to a compiler-generated method
          detailMenu.gearIcons[i].onClick = new Action<ItemIcon>(detailMenu.\u003CSetGearIconMulti\u003Eb__146_0);
          break;
        case 1:
          // ISSUE: reference to a compiler-generated method
          detailMenu.gearIcons[i].onClick = new Action<ItemIcon>(detailMenu.\u003CSetGearIconMulti\u003Eb__146_1);
          break;
      }
    }
  }

  public bool isCrossFade { get; set; }

  private IEnumerator LoadCharacterImage(PlayerUnit playerUnit)
  {
    UnitUnit u = playerUnit.unit;
    MasterDataTable.UnitJob j = playerUnit.getJobData();
    if (Object.op_Inequality((Object) this.DynCharacter.sprite2D, (Object) null) && this.imageReferenceID_ == u.resource_reference_unit_id_UnitUnit && this.imageJobID_ == j.ID)
    {
      this.isCrossFade = false;
    }
    else
    {
      if (this.isCrossFade && this.deleteAnimationFirstReset_)
      {
        this.deleteAnimationFirstReset_ = false;
        AnimationFirstReset component = ((Component) this.DynCharacter).GetComponent<AnimationFirstReset>();
        if (Object.op_Implicit((Object) component))
        {
          ((Behaviour) component).enabled = false;
          Object.Destroy((Object) component);
        }
      }
      Future<Sprite> loader = u.LoadJobFullSprite(j.ID);
      yield return (object) loader.Wait();
      if (Object.op_Equality((Object) this.DynCharacter.sprite2D, (Object) null) || ((Object) this.DynCharacter.sprite2D).name != ((Object) loader.Result).name)
      {
        if (this.isCrossFade && Object.op_Implicit((Object) this.crossFade_))
        {
          GameObject objFadeOut = ((Component) this.DynCharacter).gameObject.Clone(((Component) this.DynCharacter).transform.parent);
          yield return (object) null;
          this.DynCharacter.sprite2D = loader.Result;
          bool bWait = true;
          EventDelegate.Set(this.crossFade_.onFinished, (EventDelegate.Callback) (() => bWait = false));
          this.crossFade_.play(((Component) this.DynCharacter).gameObject, objFadeOut);
          while (bWait)
            yield return (object) null;
          objFadeOut = (GameObject) null;
        }
        else
          this.DynCharacter.sprite2D = loader.Result;
      }
      this.isCrossFade = false;
      this.imageReferenceID_ = u.resource_reference_unit_id_UnitUnit;
      this.imageJobID_ = j.ID;
      ++DetailMenu.currentLoadTimes;
      if (DetailMenu.currentLoadTimes >= 10)
      {
        Resources.UnloadUnusedAssets();
        DetailMenu.currentLoadTimes = 0;
      }
    }
  }

  public void LimitMode()
  {
    this.link_MainWeapon.GetComponent<UIButton>().onClick = (List<EventDelegate>) null;
    this.gearIcons[0].setEquipPlus(false);
    this.gearIcons[0].setEquipJinkiPlus(false);
    this.gearIcons[0].Broken = false;
    this.gearIcons[0].onClick = (Action<ItemIcon>) (_ => { });
    this.gearIcons[0].DisableLongPressEvent();
    if (Object.op_Inequality((Object) this.link_MainWeapon2, (Object) null))
      this.link_MainWeapon2.GetComponent<UIButton>().onClick = (List<EventDelegate>) null;
    if (Object.op_Inequality((Object) this.gearIcons[1], (Object) null))
    {
      this.gearIcons[1].setEquipPlus(false);
      this.gearIcons[1].setEquipJinkiPlus(false);
      this.gearIcons[1].Broken = false;
      this.gearIcons[1].onClick = (Action<ItemIcon>) (_ => { });
      this.gearIcons[1].DisableLongPressEvent();
    }
    if (Object.op_Inequality((Object) this.gearIcons[2], (Object) null))
    {
      this.gearIcons[2].setEquipPlus(false);
      this.gearIcons[2].setEquipJinkiPlus(false);
      this.gearIcons[2].Broken = false;
      this.gearIcons[2].onClick = (Action<ItemIcon>) (_ => { });
      this.gearIcons[2].DisableLongPressEvent();
    }
    if (!this.isEarthMode && Object.op_Inequality((Object) this.dirFavorite, (Object) null))
      this.dirFavorite.SetActive(false);
    if (!Object.op_Inequality((Object) this.btnIntimacy, (Object) null))
      return;
    this.btnIntimacy.SetActive(false);
  }

  public void LimitStorageMode()
  {
    if (Object.op_Inequality((Object) this.dirMainWeapon, (Object) null))
      this.dirMainWeapon.SetActive(false);
    if (!Object.op_Inequality((Object) this.dirMainWeaponMulti, (Object) null))
      return;
    this.dirMainWeaponMulti.SetActive(false);
  }

  public IEnumerator setDefaultWeapon(int index, GearGear gear)
  {
    if (index < this.gearIcons.Length && !Object.op_Equality((Object) this.gearIcons[index], (Object) null))
    {
      this.gearIcons[index].SetEmpty(false);
      if (gear != null)
      {
        IEnumerator e = this.gearIcons[index].InitByGear(gear, gear.GetElement());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator WaitScrollSe()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DetailMenu detailMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    detailMenu.Invoke("EnableScrollView", 0.3f);
    return false;
  }

  private void EnableScrollView()
  {
  }

  public void onEndScene()
  {
    foreach (DetailMenuScrollViewBase informationPanel in this.informationPanels)
      informationPanel.EndScene();
  }

  public void SetInformationPanelTab(DetailMenuScrollViewParam.TabMode mode)
  {
    foreach (DetailMenuScrollViewBase informationPanel in this.informationPanels)
    {
      if (informationPanel is DetailMenuScrollViewParam)
      {
        ((DetailMenuScrollViewParam) informationPanel).changeTab(mode);
        break;
      }
    }
  }

  public void SetInformationPanelTab(DetailMenuJobTab.TabMode mode)
  {
    foreach (DetailMenuScrollViewBase informationPanel in this.informationPanels)
    {
      if (informationPanel is DetailMenuScrollViewJob)
      {
        ((Component) informationPanel).GetComponent<DetailMenuJobTab>().changeTab(mode);
        break;
      }
    }
  }

  public void onClickedJobChange(int jobId)
  {
    if (!Object.op_Implicit((Object) this.menu))
      return;
    this.menu.changeJob(jobId);
  }

  public IEnumerator initAsyncDiffMode(
    PlayerUnit playerUnit,
    PlayerUnit prevUnit,
    int center,
    IDetailMenuContainer menuContainer)
  {
    DetailMenu detailMenu = this;
    detailMenu._playerUnit = playerUnit;
    detailMenu.gearIcons = new ItemIcon[3];
    detailMenu.equippedGears = new PlayerItem[3]
    {
      playerUnit.equippedGear,
      playerUnit.equippedGear2,
      playerUnit.equippedGear3
    };
    detailMenu.informationScrollView.SeEnable = false;
    UIWidget w = ((Component) detailMenu.informationScrollView).GetComponent<UIWidget>();
    ((UIRect) w).alpha = 0.0f;
    detailMenu.inactivateGameObject<UI2DSprite>(detailMenu.DynCharacter);
    detailMenu.inactivateGameObject(detailMenu.dirSpecial);
    detailMenu.inactivateGameObject(detailMenu.floatingSpecialPointDialog);
    detailMenu.inactivateGameObject(detailMenu.floatingGroupDialog);
    detailMenu.inactivateGameObject<UIButton>(detailMenu.IbtnUnitTraining);
    detailMenu.inactivateGameObject<UISprite>(detailMenu.slcCountry);
    detailMenu.inactivateGameObject<UI2DSprite>(detailMenu.slcInclusion);
    detailMenu.clearWeaponIcons();
    yield return (object) detailMenu.setWeaponIconsWithoutTouch(menuContainer.gearIconPrefab);
    yield return (object) detailMenu.InitInformationPanelsDiffMode(playerUnit, prevUnit, center, menuContainer);
    ((UIRect) w).alpha = 1f;
  }

  private IEnumerator InitInformationPanelsDiffMode(
    PlayerUnit playerUnit,
    PlayerUnit prevUnit,
    int center,
    IDetailMenuContainer menuContainer)
  {
    for (int i = 0; i < this.informationPanels.Length; ++i)
    {
      DetailMenuScrollViewBase informationPanel = this.informationPanels[i];
      informationPanel.isEarthMode = false;
      informationPanel.isMemory = false;
      informationPanel.setControlFlags(Control.SelfAbility);
      ((Component) informationPanel).gameObject.SetActive(true);
      yield return (object) informationPanel.initAsyncDiffMode(playerUnit, prevUnit, menuContainer);
    }
    this.informationScrollView.resetCenterItem(center);
  }

  private void inactivateGameObject<T>(T co) where T : Component
  {
    if (!Object.op_Inequality((Object) (object) co, (Object) null))
      return;
    co.gameObject.SetActive(false);
  }

  private void inactivateGameObject(GameObject go)
  {
    if (!Object.op_Inequality((Object) go, (Object) null))
      return;
    go.SetActive(false);
  }

  private void clearWeaponIcons()
  {
    if (Object.op_Inequality((Object) this.link_MainWeapon, (Object) null))
      this.link_MainWeapon.transform.Clear();
    if (Object.op_Inequality((Object) this.link_MainWeapon2, (Object) null))
      this.link_MainWeapon2.transform.Clear();
    if (Object.op_Inequality((Object) this.link_MainWeapon3, (Object) null))
      this.link_MainWeapon3.transform.Clear();
    if (this.link_MainWeaponMulti == null)
      return;
    for (int index = 0; index < this.link_MainWeaponMulti.Length; ++index)
      this.link_MainWeaponMulti[index].transform.Clear();
  }

  private IEnumerator setWeaponIconsWithoutTouch(GameObject gearIconPrefab)
  {
    GameObject[] dirWeapons = new GameObject[3]
    {
      this.dirMainWeapon,
      this.dirMainWeapon2,
      this.dirMainWeapon3
    };
    if (((IEnumerable<GameObject>) dirWeapons).Any<GameObject>((Func<GameObject, bool>) (x => Object.op_Inequality((Object) x, (Object) null))))
    {
      GameObject[] links = new GameObject[3]
      {
        this.link_MainWeapon,
        this.link_MainWeapon2,
        this.link_MainWeapon3
      };
      GameObject[] dirNoneWeapons = new GameObject[3]
      {
        null,
        this.slc_MainWeapon2_NonBase,
        this.slc_MainWeapon3_NonBase
      };
      GameObject[] dirLocks = new GameObject[3]
      {
        null,
        this.dirBuguLock2,
        this.dirBuguLock3
      };
      int[] indexMap = this._playerUnit.equippedGearIndexMapWithoutConditions;
      int lastIndex = this._playerUnit.isPossibleEquippedGear3 ? indexMap.Length - 1 : -1;
      for (int n = 0; n < dirWeapons.Length; ++n)
      {
        bool bActive = indexMap.Length > n;
        if (Object.op_Inequality((Object) dirWeapons[n], (Object) null))
        {
          dirWeapons[n].SetActive(bActive);
          bool bLock = false;
          if (bActive)
          {
            if (n != lastIndex || this.equippedGears[n] != (PlayerItem) null || this._playerUnit.isOpenedEquippedGear3)
              yield return (object) this.setGearIconSlotWithoutTouch(gearIconPrefab, links[n], indexMap[n]);
            else
              bLock = true;
          }
          if (Object.op_Inequality((Object) dirLocks[n], (Object) null))
          {
            if (bLock)
            {
              dirLocks[n].SetActive(false);
              bActive = false;
            }
            else
              dirLocks[n].SetActive(false);
          }
        }
        if (Object.op_Inequality((Object) dirNoneWeapons[n], (Object) null))
          dirNoneWeapons[n].SetActive(!bActive);
      }
    }
  }

  private IEnumerator setGearIconSlotWithoutTouch(
    GameObject gearIconPrefab,
    GameObject linkObject,
    int slot)
  {
    ItemIcon gIcon = gearIconPrefab.CloneAndGetComponent<ItemIcon>(linkObject.transform);
    this.gearIcons[slot] = gIcon;
    ((Component) gIcon).transform.localPosition = Vector3.zero;
    PlayerItem gItem = this.equippedGears[slot];
    if (gItem != (PlayerItem) null)
    {
      yield return (object) gIcon.InitByPlayerItem(gItem);
      if (gItem.broken)
        gIcon.Broken = true;
    }
    else
    {
      gIcon.SetModeGear();
      gIcon.SetEmpty(true);
    }
    gIcon.BottomModeValue = ItemIcon.BottomMode.Visible_wIconNone;
    gIcon.onClick = (Action<ItemIcon>) null;
    gIcon.gear.button.onClick.Clear();
  }

  public IEnumerator initPickup(
    Unit0042PickupMenu pickupMenu,
    int index,
    PlayerUnit playerUnit,
    int infoIndex)
  {
    DetailMenu detailMenu = this;
    if (detailMenu.deleteIndex_ != null)
    {
      Vector3[] array = ((IEnumerable<DetailMenuScrollViewBase>) detailMenu.informationPanels).Select<DetailMenuScrollViewBase, Vector3>((Func<DetailMenuScrollViewBase, Vector3>) (x => ((Component) x).transform.localPosition)).ToArray<Vector3>();
      List<GameObject> gameObjectList = new List<GameObject>(detailMenu.deleteIndex_.Count);
      DetailMenuScrollViewBase[] menuScrollViewBaseArray = new DetailMenuScrollViewBase[detailMenu.informationPanels.Length - detailMenu.deleteIndex_.Count];
      int num = 0;
      for (int index1 = 0; index1 < detailMenu.informationPanels.Length; ++index1)
      {
        if (detailMenu.deleteIndex_.Contains(index1))
          gameObjectList.Add(((Component) detailMenu.informationPanels[index1]).gameObject);
        else
          menuScrollViewBaseArray[num++] = detailMenu.informationPanels[index1];
      }
      foreach (GameObject gameObject in gameObjectList)
      {
        gameObject.SetActive(false);
        gameObject.transform.parent = (Transform) null;
        Object.Destroy((Object) gameObject);
      }
      for (int index2 = 0; index2 < menuScrollViewBaseArray.Length; ++index2)
        ((Component) menuScrollViewBaseArray[index2]).transform.localPosition = array[index2];
      detailMenu.informationPanels = menuScrollViewBaseArray;
      detailMenu.informationScrollView.resetDots();
      detailMenu.deleteIndex_ = (List<int>) null;
    }
    detailMenu.pickupMenu_ = pickupMenu;
    detailMenu.index = index;
    detailMenu._playerUnit = playerUnit;
    detailMenu.resetEquipments();
    detailMenu.isLimit = true;
    detailMenu.baseUnit = (PlayerUnit) null;
    detailMenu.setUpdate(new Action(detailMenu.updateForPickup));
    detailMenu.informationScrollView.SeEnable = false;
    yield return (object) detailMenu.LoadCharacterImage(playerUnit);
    detailMenu.isWaitBottomInitializing = true;
    detailMenu.doBottomInital_ = detailMenu.doInitPickupStatusPanel((IDetailMenuContainer) pickupMenu, infoIndex);
  }

  private IEnumerator doInitPickupStatusPanel(IDetailMenuContainer pickupMenu, int infoIndex)
  {
    foreach (UITweener component in ((Component) this.trsBottom).GetComponents<UITweener>())
    {
      component.tweenFactor = 0.0f;
      ((Behaviour) component).enabled = false;
    }
    ((Component) this.trsBottom).GetComponent<UIRect>().alpha = 0.0f;
    ((Component) this.trsBottom).GetComponent<NGTweenParts>().resetActive(true);
    yield return (object) this.doInitPickupInformationPanels(pickupMenu, infoIndex);
    if (Object.op_Inequality((Object) this.floatingGroupDialog, (Object) null))
    {
      if (Object.op_Equality((Object) this.floatingGroupDialogObject, (Object) null))
        this.floatingGroupDialogObject = pickupMenu.GroupDetailDialogPrefab.Clone(this.floatingGroupDialog.transform);
      this.floatingGroupDialogObject.SetActive(false);
      this.DisplayGroupLogo(this._playerUnit.unit);
    }
    if (Object.op_Inequality((Object) this.slcCountry, (Object) null))
    {
      ((Component) this.slcCountry).gameObject.SetActive(false);
      if (this._playerUnit.unit.country_attribute.HasValue)
      {
        ((Component) this.slcCountry).gameObject.SetActive(true);
        this._playerUnit.unit.SetCuntrySpriteName(ref this.slcCountry);
      }
    }
    if (Object.op_Inequality((Object) this.slcInclusion, (Object) null))
    {
      ((Component) this.slcInclusion).gameObject.SetActive(false);
      if (this._playerUnit.unit.inclusion_ip.HasValue)
      {
        ((Component) this.slcInclusion).gameObject.SetActive(true);
        IEnumerator e = this._playerUnit.unit.SetInclusionIP(this.slcInclusion);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    this.isWaitBottomInitializing = false;
  }

  private IEnumerator doInitPickupInformationPanels(IDetailMenuContainer container, int infoIndex)
  {
    List<GameObject[]> prefabs = new List<GameObject[]>()
    {
      new GameObject[3]
      {
        container.skillDetailDialogPrefab,
        container.skillTypeIconPrefab,
        container.skillLockIconPrefab
      },
      new GameObject[3]
      {
        container.skillfullnessIconPrefab,
        container.specialPointDetailDialogPrefab,
        null
      },
      new GameObject[3]
      {
        container.skillDetailDialogPrefab,
        container.gearKindIconPrefab,
        container.commonElementIconPrefab
      }
    };
    Control flags = Control.Limited | Control.Pickup;
    for (int i = 0; i < this.informationPanels.Length; ++i)
    {
      DetailMenuScrollViewBase informationPanel = this.informationPanels[i];
      informationPanel.isEarthMode = false;
      informationPanel.isMemory = false;
      informationPanel.setControlFlags(flags);
      if (informationPanel.Init(this._playerUnit, (PlayerUnit) null))
        yield return (object) informationPanel.initAsync(this._playerUnit, true, false, prefabs[i]);
    }
    this.informationScrollView.resetCenterItem(infoIndex);
  }

  private void updateForPickup()
  {
    if (this.isWaitBottomInitializing || this.pickupMenu_.isWaitStatusPanelInitializing || this.pickupMenu_.currentIndex != this.index || this.pickupMenu_.infoIndex == this.informationScrollView.selected)
      return;
    this.pickupMenu_.infoIndex = Math.Max(this.informationScrollView.selected, 0);
    this.pickupMenu_.updateInfoIndicator(this.pickupMenu_.infoIndex);
  }

  public void setPickupPanelIndex(int infoIndex)
  {
    if (this.pickupMenu_.currentIndex == this.index || infoIndex == this.informationScrollView.selected)
      return;
    this.informationScrollView.resetCenterItem(infoIndex);
  }
}
