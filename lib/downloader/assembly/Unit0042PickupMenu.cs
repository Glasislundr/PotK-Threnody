// Decompiled with JetBrains decompiler
// Type: Unit0042PickupMenu
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
[AddComponentMenu("Scenes/Unit/GachaPickup/Menu")]
public class Unit0042PickupMenu : BackButtonMenuBase, IDetailMenuContainer
{
  [SerializeField]
  private NGWrapScrollParts scroll_;
  [SerializeField]
  [Tooltip("scroll_.dotPrefab に格納するprefab 名")]
  private string pathDotPrefab_;
  [SerializeField]
  [Tooltip("主スクロールアイテム名")]
  private string pathDetailPrefab_;
  [SerializeField]
  private UIScrollView scrollView_;
  [SerializeField]
  private ScrollViewSpecifyBounds dotScrollView_;
  [SerializeField]
  private UILabel txtCharacterName_;
  [SerializeField]
  private UILabel txtJobName_;
  [SerializeField]
  private UI2DSprite weaponTypeIcon_;
  [SerializeField]
  private UI2DSprite rarityStarsIcon_;
  [SerializeField]
  private GameObject slcAwakening_;
  [SerializeField]
  private SpriteSelectDirect[] groupSprites_;
  [SerializeField]
  private GameObject slcGroupBase_;
  private TweenHeight twGroupBaseOpen_;
  private TweenHeight twGroupBaseClose_;
  [SerializeField]
  private GameObject dirGroupSprite_;
  private TweenPosition twGroupSpriteDirOpen_;
  private TweenPosition twGroupSpriteDirClose_;
  [SerializeField]
  private UIButton btnGroupTabIdle_;
  [SerializeField]
  private GameObject dirGroupPressed_;
  [SerializeField]
  private GameObject btnPlayDuelSkill_;
  [SerializeField]
  private Unit0042PickupMenu.TabContainer[] tabUnitTypes_;
  [SerializeField]
  private GameObject[] objArrows_;
  private bool isGroupOpen_;
  private bool isGroupTween_;
  private int dispGroupCount_;
  private const float HEIGHT_GROUPSPRITE = 46.5f;
  private const int HEIGHT_GROUPBASE = 36;
  private const int TWEEN_GROUPID_START = 100;
  private const int TWEEN_GROUPID_END = 101;
  private const int DISPLAY_OBJECT_MAX = 4;
  private int numEntity_;
  private int numUnit_;
  private GearKindIcon gearKindIcon_;
  private WheelScrollLog wheelLog_;
  private const UnitTypeEnum DEF_UNIT_TYPE = UnitTypeEnum.ouki;
  private UnitTypeEnum[] unitTypes_;
  private Dictionary<UnitTypeEnum, PlayerUnit>[] dicUnits_;
  [SerializeField]
  [Tooltip("下部ステータス再連結先")]
  private Transform bottomBase_;
  [SerializeField]
  private DuelDemoSettings duelDemoSettings_;
  private Dictionary<GameObject, Unit0042PickupMenu.UnitPage> dicPage_;
  private Dictionary<int, PlayerUnitSkills> dicDuelSkill_;
  private Queue<Unit0042PickupMenu.UnitPage> blankPages_ = new Queue<Unit0042PickupMenu.UnitPage>();
  private UIDragScrollView[] dragScrollViews_;
  private UIWidget[] dotWidgets_;
  private int characterId_;
  private int voiceId_;
  private Unit0042PickupMenu.UnitNode[] nodes_;
  private bool isInitializing_ = true;
  private bool isDisabledScroll_;
  private bool isEnabledBottomScroll_;
  private bool isLockBottomScroll_;
  private bool isWaitArrowMv_;
  private float? oldScrollViewLocalX_;
  private bool isScrollViewDragStart_;
  private Func<bool> hookPreOnBackButton_;
  private bool isVRdrag;
  private int centerStatus = -1;
  private const float WAIT_STATUS_CHANGE = 0.5f;
  private float waitStatusChange_;
  private const int MAX_LOAD_PAGE = 3;
  private int countDoStartStatusPanel_;
  private static readonly string DuelSceneName = "battle018_1";
  private bool isPlayDuelSkill_;

  public GameObject detailPrefab { get; private set; }

  public GameObject gearKindIconPrefab { get; private set; }

  public GameObject gearIconPrefab { get; private set; }

  public GameObject skillDetailDialogPrefab { get; private set; }

  public GameObject skillDetailDialog_changePrefab { get; private set; }

  public GameObject specialPointDetailDialogPrefab { get; private set; }

  public GameObject terraiAbilityDialogPrefab { get; private set; }

  public GameObject profIconPrefab { get; private set; }

  public GameObject skillTypeIconPrefab { get; private set; }

  public GameObject skillfullnessIconPrefab { get; private set; }

  public GameObject commonElementIconPrefab { get; private set; }

  public GameObject spAtkTypeIconPrefab { get; private set; }

  public GameObject modelPrefab { get; private set; }

  public GameObject skillListPrefab { get; private set; }

  public GameObject statusDetailPrefab { get; private set; }

  public GameObject StatusDetailPrefab { get; private set; }

  public GameObject TrainingPrefa { get; private set; }

  public GameObject GroupDetailDialogPrefab { get; private set; }

  public GameObject detailJobAbilityPrefab { get; private set; }

  public GameObject unityDetailPrefab { get; private set; }

  public GameObject stageItemPrefab { get; private set; }

  public Future<GameObject>[] unityDetailPrefabs { get; private set; }

  public GameObject overkillersSlotReleasePrefab { get; private set; }

  public GameObject unitIconPrefab { get; private set; }

  public GameObject overkillersBaseTagPrefab { get; private set; }

  public GameObject skillLockIconPrefab { get; private set; }

  public GameObject[] recommendPrefabs { get; private set; }

  public int currentIndex { get; set; } = -1;

  public int infoIndex { get; set; }

  private PlayerUnit currentUnit => this.getUnit(this.currentIndex);

  private PlayerUnit getUnit(int index)
  {
    if (this.numUnit_ == 0 || this.currentIndex < 0)
      return (PlayerUnit) null;
    PlayerUnit unit;
    if (!this.dicUnits_[index].TryGetValue(this.unitTypes_[index], out unit))
    {
      this.unitTypes_[index] = this.dicUnits_[index].Keys.First<UnitTypeEnum>();
      this.dicUnits_[index].TryGetValue(this.unitTypes_[index], out unit);
    }
    return unit;
  }

  private GameObject currentDetail
  {
    get
    {
      return this.dicPage_ == null || this.currentIndex < 0 ? (GameObject) null : this.dicPage_.FirstOrDefault<KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage>>((Func<KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage>, bool>) (x => x.Value.index == this.currentIndex)).Key;
    }
  }

  public DetailMenuScrollViewParam.TabMode viewParamTabMode { get; private set; }

  public DetailMenuJobTab.TabMode viewJobTabMode { get; private set; } = DetailMenuJobTab.TabMode.JobChange;

  public void updateInfoIndicator(int idx)
  {
    this.infoIndex = idx;
    if (this.dicPage_ == null)
      return;
    foreach (KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage> keyValuePair in this.dicPage_)
    {
      if (keyValuePair.Value.index >= 0)
        keyValuePair.Value.detail.setPickupPanelIndex(this.infoIndex);
    }
  }

  public void UpdateInfoIndicator(DetailMenuScrollViewParam.TabMode mode)
  {
    this.viewParamTabMode = mode;
    if (this.dicPage_ == null)
      return;
    foreach (Unit0042PickupMenu.UnitPage unitPage in this.dicPage_.Values)
      unitPage.detail.SetInformationPanelTab(mode);
  }

  public void UpdateInfoIndicator(DetailMenuJobTab.TabMode mode)
  {
    this.viewJobTabMode = mode;
    if (this.dicPage_ == null)
      return;
    foreach (Unit0042PickupMenu.UnitPage unitPage in this.dicPage_.Values)
      unitPage.detail.SetInformationPanelTab(mode);
  }

  public void setHookPreOnBackButton(Func<bool> func) => this.hookPreOnBackButton_ = func;

  public void clearHookPreOnBackButton(Func<bool> func)
  {
    if (this.hookPreOnBackButton_ == null || !(this.hookPreOnBackButton_ == func))
      return;
    this.hookPreOnBackButton_ = (Func<bool>) null;
  }

  public void IbtnZoom()
  {
    if (this.IsPushAndSet())
      return;
    Unit0043Scene.changeScene(true, this.currentUnit.unit, 0, true);
  }

  public void IbtnLeftArrow()
  {
    if (this.isWaitArrowMv_)
      return;
    this.movePage(-1);
  }

  public void IbtnRightArrow()
  {
    if (this.isWaitArrowMv_)
      return;
    this.movePage(1);
  }

  private void SetTitleBarInfo(bool isSe)
  {
    PlayerUnit currentUnit = this.currentUnit;
    UnitUnit unit = currentUnit.unit;
    if (string.IsNullOrEmpty(unit.formal_name))
      this.txtCharacterName_.SetText(unit.name);
    else
      this.txtCharacterName_.SetText(unit.formal_name);
    if (Object.op_Inequality((Object) this.txtJobName_, (Object) null))
      this.txtJobName_.SetText(currentUnit.getJobData().name);
    if (Object.op_Inequality((Object) this.slcAwakening_, (Object) null))
      this.slcAwakening_.SetActive(unit.awake_unit_flag);
    if (Object.op_Equality((Object) this.gearKindIcon_, (Object) null))
    {
      this.gearKindIcon_ = this.gearKindIconPrefab.Clone(((Component) this.weaponTypeIcon_).transform).GetComponent<GearKindIcon>();
      ((UIWidget) ((Component) this.gearKindIcon_).gameObject.GetComponent<UI2DSprite>()).depth = ((UIWidget) this.weaponTypeIcon_).depth + 1;
    }
    this.gearKindIcon_.Init(unit.kind, currentUnit.GetElement());
    RarityIcon.SetRarity(currentUnit, this.rarityStarsIcon_, true);
    this.displayGroupLogo(unit);
    this.setDuelSkill();
    this.setTabButton((UnitTypeEnum) currentUnit._unit_type);
    if (!this.isDisabledScroll_)
    {
      ((Component) this.dotScrollView_).GetComponent<TweenController>().isActive = true;
      this.dotScrollView_.ClearBounds();
      this.dotScrollView_.AddBound(this.dotWidgets_[this.currentIndex]);
      this.dotScrollView_.RestrictWithinBounds(false);
    }
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Equality((Object) instance, (Object) null))
      return;
    if (isSe)
      instance.playSE("SE_1005");
    this.changeCharacterVoice(unit, instance);
  }

  private void changeCharacterVoice(UnitUnit unit, NGSoundManager sm)
  {
    int characterUnitCharacter = unit.character_UnitCharacter;
    UnitVoicePattern unitVoicePattern = unit.unitVoicePattern;
    if (unitVoicePattern == null || this.characterId_ == characterUnitCharacter && this.voiceId_ == unitVoicePattern.ID)
      return;
    this.characterId_ = characterUnitCharacter;
    this.voiceId_ = unitVoicePattern.ID;
    sm.stopVoice();
    sm.playVoiceByID(unitVoicePattern, 42);
  }

  private void updateBlank()
  {
    foreach (Unit0042PickupMenu.UnitPage unitPage in this.dicPage_.Values)
    {
      if (!unitPage.isWaitLoad && this.calcDistance(unitPage.index) > 1 && !this.blankPages_.Contains(unitPage))
      {
        ((Component) unitPage.detail).gameObject.SetActive(false);
        unitPage.index = -1;
        this.blankPages_.Enqueue(unitPage);
      }
    }
  }

  private int calcDistance(int index)
  {
    if (this.currentIndex == index)
      return 0;
    int numUnit = this.numUnit_;
    return Mathf.Min((numUnit + this.currentIndex - index) % numUnit, (numUnit + index - this.currentIndex) % numUnit);
  }

  protected override void Update()
  {
    base.Update();
    this.updateScroll();
  }

  private void updateScroll()
  {
    if (this.isInitializing_)
    {
      this.isEnabledBottomScroll_ = this.scrollView_.isDragging;
    }
    else
    {
      if (this.isDisabledScroll_)
        return;
      int center = this.currentIndex;
      float x = ((Component) this.scrollView_).transform.localPosition.x;
      float dir = this.oldScrollViewLocalX_.HasValue ? this.oldScrollViewLocalX_.Value - x : 0.0f;
      if (!this.oldScrollViewLocalX_.HasValue || (double) dir != 0.0)
      {
        int itemSize = this.scroll_.content.itemSize;
        int num1 = itemSize / 2;
        int num2 = itemSize * this.numUnit_;
        float num3 = (x - (float) num1) % (float) num2;
        center = (double) num3 > 0.0 ? (int) ((double) num2 - (double) num3) / itemSize : -(int) ((double) num3 / (double) itemSize);
        this.oldScrollViewLocalX_ = new float?(x);
      }
      else
        this.isVRdrag = false;
      if (this.dicPage_ != null)
      {
        if (Object.op_Inequality((Object) this.wheelLog_, (Object) null))
        {
          this.isVRdrag |= (double) this.wheelLog_.amount != 0.0;
          this.wheelLog_.resetAmount();
        }
        this.isVRdrag |= this.scrollView_.isDragging;
        bool flag = !this.isVRdrag && !this.isLockBottomScroll_ && !this.isWaitArrowMv_;
        if (this.isEnabledBottomScroll_ != flag)
        {
          if (!flag)
          {
            this.centerStatus = -1;
            this.startStatusFadeOut((GameObject) null, false);
            ((Component) this.dotScrollView_).GetComponent<TweenController>().isActive = true;
          }
          this.isEnabledBottomScroll_ = flag;
        }
      }
      if (this.currentIndex != center)
      {
        List<int> loadReserves = this.createLoadReserves(center, dir);
        this.currentIndex = center;
        this.SetTitleBarInfo(true);
        this.updateBlank();
        for (int index = 0; index < loadReserves.Count; ++index)
        {
          if (!this.createPage(loadReserves[index]))
          {
            this.StartCoroutine(this.doWaitCreatePage(loadReserves.Skip<int>(index).ToList<int>()));
            break;
          }
        }
      }
      if (this.scrollView_.isDragging)
      {
        if (!this.isScrollViewDragStart_)
          this.isScrollViewDragStart_ = true;
      }
      else
      {
        if (this.isScrollViewDragStart_)
          this.waitStatusChange_ = 0.5f;
        this.isScrollViewDragStart_ = false;
      }
      if ((double) this.waitStatusChange_ > 0.0)
        this.waitStatusChange_ -= Time.deltaTime;
      if (!this.isEnabledBottomScroll_ || (double) this.waitStatusChange_ > 0.0 || this.currentIndex < 0 || this.centerStatus == this.currentIndex)
        return;
      GameObject currentDetail = this.currentDetail;
      this.startStatusFadeOut(currentDetail, true);
      this.StartCoroutine(this.doStartStatusPanel(currentDetail, false));
      this.centerStatus = this.currentIndex;
      ((Component) this.dotScrollView_).GetComponent<TweenController>().isActive = false;
    }
  }

  private void onItemChanged(int selected)
  {
  }

  private IEnumerator doWaitCreatePage(List<int> request)
  {
    int num = this.scrollView_.isDragging ? 1 : 0;
    this.setEnabledScrollViews(false);
    if (num != 0)
      this.scrollView_.Press(false);
    yield return (object) null;
    while (this.dicPage_.Any<KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage>>((Func<KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage>, bool>) (kv => kv.Value.isWaitLoad)))
      yield return (object) null;
    this.updateBlank();
    foreach (int unitIdx in request)
      yield return (object) this.doCreatePage(unitIdx);
    this.setEnabledScrollViews(true);
  }

  private UIDragScrollView[] dragScrollViews
  {
    get
    {
      if (this.dragScrollViews_ != null)
        return this.dragScrollViews_;
      this.dragScrollViews_ = ((IEnumerable<UIDragScrollView>) ((Component) this).GetComponentsInChildren<UIDragScrollView>()).Where<UIDragScrollView>((Func<UIDragScrollView, bool>) (x => Object.op_Equality((Object) x.scrollView, (Object) this.scrollView_))).ToArray<UIDragScrollView>();
      return this.dragScrollViews_;
    }
  }

  private void setEnabledScrollViews(bool bEnabled)
  {
    foreach (Behaviour dragScrollView in this.dragScrollViews)
      dragScrollView.enabled = bEnabled;
  }

  private List<int> createLoadReserves(int center, float dir = 0.0f)
  {
    int capacity = Mathf.Min(3, this.numUnit_);
    List<int> loadReserves = new List<int>(capacity)
    {
      center
    };
    if ((double) dir == 0.0)
    {
      if (loadReserves.Count < capacity)
        loadReserves.Add((center + 1) % this.numUnit_);
      if (loadReserves.Count < capacity)
        loadReserves.Add((this.numUnit_ + center - 1) % this.numUnit_);
    }
    else if ((double) dir > 0.0)
    {
      if (loadReserves.Count < capacity)
        loadReserves.Add((center + 1) % this.numUnit_);
      if (loadReserves.Count < capacity)
        loadReserves.Add((center + 2) % this.numUnit_);
    }
    else
    {
      if (loadReserves.Count < capacity)
        loadReserves.Add((this.numUnit_ + center - 1) % this.numUnit_);
      if (loadReserves.Count < capacity)
        loadReserves.Add((this.numUnit_ + center - 2) % this.numUnit_);
    }
    return loadReserves;
  }

  protected virtual IEnumerator LoadPrefabs()
  {
    Unit0042PickupMenu unit0042PickupMenu = this;
    Future<GameObject> loader = (Future<GameObject>) null;
    if (!Object.op_Implicit((Object) unit0042PickupMenu.scroll_.dotPrefab))
    {
      loader = new ResourceObject(unit0042PickupMenu.pathDotPrefab_).Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.scroll_.dotPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.detailPrefab)))
    {
      loader = new ResourceObject(unit0042PickupMenu.pathDetailPrefab_).Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.detailPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.gearKindIconPrefab)))
    {
      loader = Res.Icons.GearKindIcon.Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.gearKindIconPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.skillDetailDialogPrefab)))
    {
      loader = PopupSkillDetails.createPrefabLoader(false);
      yield return (object) loader.Wait();
      unit0042PickupMenu.skillDetailDialogPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.specialPointDetailDialogPrefab)))
    {
      loader = new ResourceObject("Prefabs/unit004_2/SpecialPoint_DetailDialog").Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.specialPointDetailDialogPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.skillTypeIconPrefab)))
    {
      loader = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.skillTypeIconPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.skillfullnessIconPrefab)))
    {
      loader = new ResourceObject("Prefabs/SkillFamily/SkillFamilyIcon").Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.skillfullnessIconPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.commonElementIconPrefab)))
    {
      loader = Res.Icons.CommonElementIcon.Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.commonElementIconPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.GroupDetailDialogPrefab)))
    {
      loader = new ResourceObject("Prefabs/unit004_2/GroupDetailDialog").Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.GroupDetailDialogPrefab = loader.Result;
    }
    if (!Object.op_Implicit((Object) unit0042PickupMenu.unitIconPrefab))
    {
      loader = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.unitIconPrefab = loader.Result;
    }
    // ISSUE: explicit non-virtual call
    if (!Object.op_Implicit((Object) __nonvirtual (unit0042PickupMenu.skillLockIconPrefab)))
    {
      loader = new ResourceObject("Prefabs/BattleSkillIcon/dir_SkillLock").Load<GameObject>();
      yield return (object) loader.Wait();
      unit0042PickupMenu.skillLockIconPrefab = loader.Result;
    }
    if (unit0042PickupMenu.recommendPrefabs == null)
    {
      loader = PopupRecommendMenu.loadResource();
      yield return (object) loader.Wait();
      unit0042PickupMenu.recommendPrefabs = new GameObject[1]
      {
        loader.Result
      };
    }
  }

  public IEnumerator Init(Dictionary<UnitTypeEnum, PlayerUnit>[] playerUnits)
  {
    Unit0042PickupMenu unit0042PickupMenu1 = this;
    unit0042PickupMenu1.isInitializing_ = true;
    unit0042PickupMenu1.isWaitArrowMv_ = false;
    ((IEnumerable<GameObject>) unit0042PickupMenu1.objArrows_).SetActives(false);
    unit0042PickupMenu1.dicUnits_ = playerUnits;
    unit0042PickupMenu1.numUnit_ = unit0042PickupMenu1.dicUnits_.Length;
    unit0042PickupMenu1.unitTypes_ = Enumerable.Repeat<UnitTypeEnum>(UnitTypeEnum.ouki, unit0042PickupMenu1.numUnit_).ToArray<UnitTypeEnum>();
    unit0042PickupMenu1.dragScrollViews_ = (UIDragScrollView[]) null;
    for (int index = 0; index < unit0042PickupMenu1.tabUnitTypes_.Length; ++index)
    {
      Unit0042PickupMenu unit0042PickupMenu = unit0042PickupMenu1;
      Unit0042PickupMenu.TabContainer tab = unit0042PickupMenu1.tabUnitTypes_[index];
      tab.top.SetActive(true);
      ((UIButtonColor) tab.button).isEnabled = false;
      tab.objDisabled.SetActive(false);
      EventDelegate.Set(tab.button.onClick, (EventDelegate.Callback) (() => unit0042PickupMenu.onClickedTabUnitType(tab.type)));
    }
    if (Object.op_Inequality((Object) unit0042PickupMenu1.slcGroupBase_, (Object) null))
    {
      foreach (TweenHeight componentsInChild in unit0042PickupMenu1.slcGroupBase_.GetComponentsInChildren<TweenHeight>())
      {
        switch (((UITweener) componentsInChild).tweenGroup)
        {
          case 100:
            unit0042PickupMenu1.twGroupBaseOpen_ = componentsInChild;
            break;
          case 101:
            unit0042PickupMenu1.twGroupBaseClose_ = componentsInChild;
            break;
        }
      }
      foreach (TweenPosition componentsInChild in unit0042PickupMenu1.dirGroupSprite_.GetComponentsInChildren<TweenPosition>())
      {
        switch (((UITweener) componentsInChild).tweenGroup)
        {
          case 100:
            unit0042PickupMenu1.twGroupSpriteDirOpen_ = componentsInChild;
            break;
          case 101:
            unit0042PickupMenu1.twGroupSpriteDirClose_ = componentsInChild;
            break;
        }
      }
    }
    yield return (object) unit0042PickupMenu1.LoadPrefabs();
    unit0042PickupMenu1.numEntity_ = Mathf.Min(unit0042PickupMenu1.numUnit_, 4);
    if (unit0042PickupMenu1.dicPage_ != null)
    {
      foreach (KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage> keyValuePair in unit0042PickupMenu1.dicPage_)
      {
        if (Object.op_Inequality((Object) keyValuePair.Value.tween, (Object) null))
          Object.Destroy((Object) ((Component) keyValuePair.Value.tween).gameObject);
        if (Object.op_Inequality((Object) keyValuePair.Key, (Object) null))
          Object.Destroy((Object) keyValuePair.Key);
      }
      unit0042PickupMenu1.dicPage_.Clear();
    }
    else
      unit0042PickupMenu1.dicPage_ = new Dictionary<GameObject, Unit0042PickupMenu.UnitPage>(unit0042PickupMenu1.numEntity_);
    unit0042PickupMenu1.dicDuelSkill_ = new Dictionary<int, PlayerUnitSkills>(unit0042PickupMenu1.numUnit_);
    unit0042PickupMenu1.scroll_.destroyParts(false);
    Unit0042PickupMenu.UnitNode node = unit0042PickupMenu1.createNode();
    unit0042PickupMenu1.nodes_ = new Unit0042PickupMenu.UnitNode[unit0042PickupMenu1.numUnit_];
    for (int index = 0; index < unit0042PickupMenu1.numUnit_; ++index)
      unit0042PickupMenu1.nodes_[index] = unit0042PickupMenu1.createNode(node);
    Object.Destroy((Object) node.center);
    yield return (object) null;
    ((Component) unit0042PickupMenu1.scroll_.content).gameObject.SetActive(true);
    unit0042PickupMenu1.scroll_.content.SortBasedOnScrollMovement();
    unit0042PickupMenu1.blankPages_.Clear();
    for (int index = 0; index < unit0042PickupMenu1.numEntity_; ++index)
    {
      // ISSUE: explicit non-virtual call
      GameObject key = __nonvirtual (unit0042PickupMenu1.detailPrefab).Clone(unit0042PickupMenu1.nodes_[index].center.transform);
      DetailMenuPrefab component = key.GetComponent<DetailMenuPrefab>();
      key.SetActive(false);
      Unit0042PickupMenu.UnitPage unitPage = new Unit0042PickupMenu.UnitPage(component);
      unit0042PickupMenu1.blankPages_.Enqueue(unitPage);
      unit0042PickupMenu1.dicPage_.Add(key, unitPage);
      DetailMenu normal = component.normal;
      normal.isEarthMode = false;
      normal.isMemory = false;
      GameObject gameObject = normal.purgeStatusPanel(unit0042PickupMenu1.scrollView_.panel.GetViewSize(), unit0042PickupMenu1.bottomBase_);
      unitPage.tween = gameObject.GetComponent<NGTweenParts>();
    }
    unit0042PickupMenu1.currentIndex = 0;
    List<int> initList = unit0042PickupMenu1.createLoadReserves(unit0042PickupMenu1.currentIndex);
    yield return (object) unit0042PickupMenu1.doCreatePage(initList[0]);
    ((Component) unit0042PickupMenu1.scrollView_).transform.localPosition = new Vector3((float) (-unit0042PickupMenu1.scroll_.content.itemSize * unit0042PickupMenu1.currentIndex), 0.0f, 0.0f);
    unit0042PickupMenu1.countDoStartStatusPanel_ = 0;
    yield return (object) unit0042PickupMenu1.doStartStatusPanel(unit0042PickupMenu1.currentDetail, true);
    unit0042PickupMenu1.centerStatus = unit0042PickupMenu1.currentIndex;
    for (int index = 1; index < initList.Count; ++index)
      unit0042PickupMenu1.createPage(initList[index]);
    if (unit0042PickupMenu1.numUnit_ > 1)
    {
      ((Behaviour) unit0042PickupMenu1.scrollView_).enabled = true;
      ((Behaviour) unit0042PickupMenu1.dotScrollView_).enabled = true;
      unit0042PickupMenu1.isDisabledScroll_ = false;
      unit0042PickupMenu1.wheelLog_ = ((Component) unit0042PickupMenu1.scrollView_).GetComponent<WheelScrollLog>();
      unit0042PickupMenu1.dotWidgets_ = new UIWidget[unit0042PickupMenu1.numUnit_];
      Transform transform = ((Component) unit0042PickupMenu1.scroll_.dot).transform;
      for (int index = 0; index < unit0042PickupMenu1.numUnit_; ++index)
        unit0042PickupMenu1.dotWidgets_[index] = ((Component) transform.GetChild(index)).GetComponentInChildren<UIWidget>(true);
      ((IEnumerable<GameObject>) unit0042PickupMenu1.objArrows_).SetActives(true);
    }
    else
    {
      ((Behaviour) unit0042PickupMenu1.scrollView_).enabled = false;
      ((Behaviour) unit0042PickupMenu1.dotScrollView_).enabled = false;
      unit0042PickupMenu1.isDisabledScroll_ = true;
      unit0042PickupMenu1.wheelLog_ = (WheelScrollLog) null;
      unit0042PickupMenu1.dotWidgets_ = (UIWidget[]) null;
    }
    unit0042PickupMenu1.SetTitleBarInfo(false);
    yield return (object) null;
    ((Component) unit0042PickupMenu1.dotScrollView_).GetComponent<TweenController>().isActive = false;
    while (((Component) unit0042PickupMenu1.dotScrollView_).gameObject.activeSelf)
      yield return (object) null;
    unit0042PickupMenu1.isInitializing_ = false;
  }

  private void onClickedTabUnitType(UnitTypeEnum type)
  {
    if (this.IsPushAndSet())
      return;
    if (!this.setTabButton(type))
      this.IsPush = false;
    else
      this.StartCoroutine("doUpdateCenter", (object) type);
  }

  private bool setTabButton(UnitTypeEnum type)
  {
    Dictionary<UnitTypeEnum, PlayerUnit> dicUnit = this.dicUnits_[this.currentIndex];
    if (!dicUnit.ContainsKey(type))
      return false;
    for (int index = 0; index < this.tabUnitTypes_.Length; ++index)
    {
      Unit0042PickupMenu.TabContainer tabUnitType = this.tabUnitTypes_[index];
      if (tabUnitType.type == type)
      {
        tabUnitType.top.SetActive(true);
        tabUnitType.objDisabled.SetActive(false);
        ((UIButtonColor) tabUnitType.button).isEnabled = false;
      }
      else if (dicUnit.ContainsKey(tabUnitType.type))
      {
        tabUnitType.top.SetActive(true);
        tabUnitType.objDisabled.SetActive(false);
        ((UIButtonColor) tabUnitType.button).isEnabled = true;
      }
      else
      {
        tabUnitType.top.SetActive(false);
        tabUnitType.objDisabled.SetActive(true);
      }
    }
    return true;
  }

  private IEnumerator doUpdateCenter(UnitTypeEnum nextType)
  {
    int currentIndex = this.currentIndex;
    if (this.unitTypes_[currentIndex] != nextType && this.dicUnits_[currentIndex].TryGetValue(nextType, out PlayerUnit _))
    {
      if (this.numUnit_ > 1)
        this.setEnabledScrollViews(false);
      DateTime wait = DateTime.Now;
      this.startStatusFadeOut((GameObject) null, false);
      wait += TimeSpan.FromSeconds(0.5);
      GameObject cur = this.currentDetail;
      this.unitTypes_[currentIndex] = nextType;
      yield return (object) this.doCreatePage(this.dicPage_[cur], this.currentIndex);
      while (wait > DateTime.Now)
        yield return (object) null;
      yield return (object) this.doStartStatusPanel(cur, false);
      if (this.numUnit_ > 1)
        this.setEnabledScrollViews(true);
    }
  }

  private Unit0042PickupMenu.UnitNode createNode()
  {
    Unit0042PickupMenu.UnitNode node = new Unit0042PickupMenu.UnitNode()
    {
      center = new GameObject("node")
    };
    node.center.transform.localScale = Vector3.one;
    node.center.transform.localPosition = Vector3.zero;
    node.center.transform.localRotation = Quaternion.identity;
    GameObject self = new GameObject("target");
    self.SetParent(node.center);
    node.target = self.transform;
    return node;
  }

  private Unit0042PickupMenu.UnitNode createNode(Unit0042PickupMenu.UnitNode original)
  {
    Unit0042PickupMenu.UnitNode node = new Unit0042PickupMenu.UnitNode()
    {
      center = this.scroll_.instantiateParts(original.center, false)
    };
    node.target = node.center.transform.GetChild(0);
    return node;
  }

  private bool createPage(int unitIdx)
  {
    if (this.dicPage_.Any<KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage>>((Func<KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage>, bool>) (x => x.Value.index == unitIdx)))
      return true;
    if (!this.blankPages_.Any<Unit0042PickupMenu.UnitPage>())
      return false;
    this.StartCoroutine(this.doCreatePage(this.blankPages_.Dequeue(), unitIdx));
    return true;
  }

  private IEnumerator doCreatePage(int unitIdx)
  {
    if (!this.dicPage_.Any<KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage>>((Func<KeyValuePair<GameObject, Unit0042PickupMenu.UnitPage>, bool>) (x => x.Value.index == unitIdx)) && this.blankPages_.Any<Unit0042PickupMenu.UnitPage>())
      yield return (object) this.doCreatePage(this.blankPages_.Dequeue(), unitIdx);
  }

  private IEnumerator doCreatePage(Unit0042PickupMenu.UnitPage page, int index)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit0042PickupMenu pickupMenu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      page.isWaitLoad = false;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    page.isWaitLoad = true;
    page.index = index;
    DetailMenuPrefab detail = page.detail;
    GameObject gameObject = ((Component) detail).gameObject;
    gameObject.SetActive(true);
    gameObject.SetParent(pickupMenu.nodes_[index].center);
    PlayerUnit unit = pickupMenu.getUnit(index);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) detail.initPickup(pickupMenu, index, unit, pickupMenu.infoIndex);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public bool isWaitStatusPanelInitializing => this.countDoStartStatusPanel_ > 0;

  private IEnumerator doStartStatusPanel(GameObject go, bool bImmediate)
  {
    Unit0042PickupMenu unit0042PickupMenu = this;
    Unit0042PickupMenu.UnitPage page = unit0042PickupMenu.dicPage_[go];
    if (page.mode == Unit0042PickupMenu.StatusMode.FadeOut)
    {
      if (!bImmediate)
        unit0042PickupMenu.IsPush = true;
      ++unit0042PickupMenu.countDoStartStatusPanel_;
      DetailMenu detailMenu = page.detail.normal;
      IEnumerator bottomInitial = detailMenu.getBottomInitial();
      if (bottomInitial != null)
      {
        page.mode = Unit0042PickupMenu.StatusMode.Initialize;
        yield return (object) bottomInitial;
      }
      detailMenu.InformationScrollView.resetCenterItem(unit0042PickupMenu.infoIndex);
      if (page.mode == Unit0042PickupMenu.StatusMode.CancelFadeIn)
      {
        page.tween.resetActive(false);
        page.mode = Unit0042PickupMenu.StatusMode.FadeOut;
      }
      else
      {
        NGTweenParts tween = page.tween;
        if (bImmediate)
        {
          ((Component) tween).GetComponent<UIRect>().alpha = 1f;
          tween.resetActive(true);
        }
        else
        {
          ((Component) tween).GetComponent<UIRect>().alpha = 0.0f;
          tween.forceActive(true);
        }
        unit0042PickupMenu.dicPage_[go].mode = Unit0042PickupMenu.StatusMode.FadeIn;
        page.detail.normal.InformationScrollView.SeEnable = true;
      }
      --unit0042PickupMenu.countDoStartStatusPanel_;
      if (!bImmediate)
        unit0042PickupMenu.IsPush = false;
    }
  }

  private void startStatusFadeOut(GameObject goExclude, bool bImmediate)
  {
    foreach (GameObject key in this.dicPage_.Keys.ToList<GameObject>())
    {
      if (!Object.op_Equality((Object) key, (Object) goExclude))
      {
        Unit0042PickupMenu.UnitPage unitPage = this.dicPage_[key];
        switch (unitPage.mode)
        {
          case Unit0042PickupMenu.StatusMode.Initialize:
            unitPage.mode = Unit0042PickupMenu.StatusMode.CancelFadeIn;
            continue;
          case Unit0042PickupMenu.StatusMode.FadeIn:
            unitPage.detail.normal.InformationScrollView.SeEnable = false;
            unitPage.mode = Unit0042PickupMenu.StatusMode.FadeOut;
            if (bImmediate)
            {
              unitPage.tween.resetActive(false);
              ((Component) unitPage.tween).GetComponent<UIRect>().alpha = 0.0f;
              continue;
            }
            unitPage.tween.isActive = false;
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void displayGroupLogo(UnitUnit playerUnit)
  {
    if (this.groupSprites_ == null || this.groupSprites_.Length == 0)
      return;
    int index = 0;
    ((IEnumerable<SpriteSelectDirect>) this.groupSprites_).SetActives<SpriteSelectDirect>(false);
    UnitGroup groupInfo = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == playerUnit.ID));
    if (groupInfo != null)
    {
      UnitGroupLargeCategory groupLargeCategory = Array.Find<UnitGroupLargeCategory>(MasterData.UnitGroupLargeCategoryList, (Predicate<UnitGroupLargeCategory>) (x => x.ID == groupInfo.group_large_category_id_UnitGroupLargeCategory));
      UnitGroupSmallCategory groupSmallCategory = Array.Find<UnitGroupSmallCategory>(MasterData.UnitGroupSmallCategoryList, (Predicate<UnitGroupSmallCategory>) (x => x.ID == groupInfo.group_small_category_id_UnitGroupSmallCategory));
      UnitGroupClothingCategory clothingCategory1 = Array.Find<UnitGroupClothingCategory>(MasterData.UnitGroupClothingCategoryList, (Predicate<UnitGroupClothingCategory>) (x => x.ID == groupInfo.group_clothing_category_id_UnitGroupClothingCategory));
      UnitGroupClothingCategory clothingCategory2 = Array.Find<UnitGroupClothingCategory>(MasterData.UnitGroupClothingCategoryList, (Predicate<UnitGroupClothingCategory>) (x => x.ID == groupInfo.group_clothing_category_id_2_UnitGroupClothingCategory));
      UnitGroupGenerationCategory generationCategory = Array.Find<UnitGroupGenerationCategory>(MasterData.UnitGroupGenerationCategoryList, (Predicate<UnitGroupGenerationCategory>) (x => x.ID == groupInfo.group_generation_category_id_UnitGroupGenerationCategory));
      if (groupLargeCategory == null && groupSmallCategory == null && clothingCategory1 == null && clothingCategory2 == null && generationCategory == null)
        return;
      if (groupLargeCategory != null && groupLargeCategory.ID != 1)
      {
        this.groupSprites_[index].SetSpriteName<string>(groupLargeCategory.GetSpriteName());
        ((Component) this.groupSprites_[index]).gameObject.SetActive(true);
        ++index;
      }
      if (groupSmallCategory != null && groupSmallCategory.ID != 1)
      {
        this.groupSprites_[index].SetSpriteName<string>(groupSmallCategory.GetSpriteName());
        ((Component) this.groupSprites_[index]).gameObject.SetActive(true);
        ++index;
      }
      if (clothingCategory1 != null && clothingCategory1.ID != 1)
      {
        this.groupSprites_[index].SetSpriteName<string>(clothingCategory1.GetSpriteName());
        ((Component) this.groupSprites_[index]).gameObject.SetActive(true);
        ++index;
      }
      if (clothingCategory2 != null && clothingCategory2.ID != 1)
      {
        this.groupSprites_[index].SetSpriteName<string>(clothingCategory2.GetSpriteName());
        ((Component) this.groupSprites_[index]).gameObject.SetActive(true);
        ++index;
      }
      if (generationCategory != null && generationCategory.ID != 1)
      {
        this.groupSprites_[index].SetSpriteName<string>(generationCategory.GetSpriteName());
        ((Component) this.groupSprites_[index]).gameObject.SetActive(true);
        ++index;
      }
    }
    if (!Object.op_Inequality((Object) this.slcGroupBase_, (Object) null))
      return;
    this.dispGroupCount_ = index;
    this.twGroupBaseOpen_.to = (int) this.getGroupHeightTarget();
    this.twGroupBaseClose_.from = (int) this.getGroupHeightTarget();
    this.twGroupSpriteDirOpen_.to.y = this.getGroupPositionTarget();
    this.twGroupSpriteDirOpen_.from.y = this.getGroupPositionInit();
    this.twGroupSpriteDirClose_.to.y = this.getGroupPositionInit();
    this.twGroupSpriteDirClose_.from.y = this.getGroupPositionTarget();
    this.setGroupPos();
    if (this.dispGroupCount_ != 0)
      return;
    ((Component) this.btnGroupTabIdle_).gameObject.SetActive(false);
    this.dirGroupPressed_.SetActive(false);
  }

  private void setDuelSkill()
  {
    if (Object.op_Equality((Object) this.btnPlayDuelSkill_, (Object) null))
      return;
    PlayerUnitSkills duelSkill;
    if (!this.dicDuelSkill_.TryGetValue(this.currentIndex, out duelSkill))
    {
      foreach (UnitParameter.SkillSortUnit sortedSkill in new UnitParameter(new BL.Unit()
      {
        playerUnit = this.currentUnit
      }).sortedSkills)
      {
        if (sortedSkill.group == UnitParameter.SkillGroup.Duel && sortedSkill.duelSkill.skill.haveKoyuDuelEffect)
        {
          duelSkill = sortedSkill.duelSkill;
          this.dicDuelSkill_.Add(this.currentIndex, duelSkill);
          break;
        }
      }
    }
    this.btnPlayDuelSkill_.SetActive(duelSkill != null);
  }

  public void IbtnRecommend()
  {
    if (this.recommendPrefabs == null || Object.op_Equality((Object) this.recommendPrefabs[0], (Object) null))
      return;
    PopupRecommendMenu.open(this.recommendPrefabs[0], this.currentUnit, bDisabledAccountStatus: true);
  }

  public void IbtnGroupOpen()
  {
    if (this.isGroupTween_ || this.isGroupOpen_)
      return;
    ((Component) this.btnGroupTabIdle_).gameObject.SetActive(false);
    this.dirGroupPressed_.SetActive(true);
    NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), 100);
    this.isGroupOpen_ = true;
    this.isGroupTween_ = true;
  }

  public void onFinishedGroupOpen() => this.isGroupTween_ = false;

  public void IbtnGroupClose()
  {
    if (this.isGroupTween_ || !this.isGroupOpen_)
      return;
    NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), 101);
    this.isGroupOpen_ = false;
    this.isGroupTween_ = true;
  }

  public void onFinishedGroupClose()
  {
    if (!this.isGroupTween_)
      return;
    if (this.dispGroupCount_ > 0)
      ((Component) this.btnGroupTabIdle_).gameObject.SetActive(true);
    else
      ((Component) this.btnGroupTabIdle_).gameObject.SetActive(false);
    this.dirGroupPressed_.SetActive(false);
    this.isGroupTween_ = false;
  }

  private void setGroupPos()
  {
    if (this.isGroupOpen_)
    {
      ((UIWidget) this.slcGroupBase_.GetComponent<UISprite>()).height = (int) this.getGroupHeightTarget();
      this.dirGroupSprite_.transform.localPosition = new Vector3(this.dirGroupSprite_.transform.localPosition.x, this.getGroupPositionTarget(), this.dirGroupSprite_.transform.localPosition.z);
      ((Component) this.btnGroupTabIdle_).gameObject.SetActive(false);
      this.dirGroupPressed_.SetActive(true);
    }
    else
    {
      ((UIWidget) this.slcGroupBase_.GetComponent<UISprite>()).height = 36;
      this.dirGroupSprite_.transform.localPosition = new Vector3(this.dirGroupSprite_.transform.localPosition.x, this.getGroupPositionInit(), this.dirGroupSprite_.transform.localPosition.z);
      ((Component) this.btnGroupTabIdle_).gameObject.SetActive(true);
      this.dirGroupPressed_.SetActive(false);
    }
    this.isGroupTween_ = false;
  }

  private float getGroupHeightTarget()
  {
    return (float) (36.0 + 46.5 * (double) this.dispGroupCount_ + 8.0);
  }

  private float getGroupPositionTarget() => 0.0f;

  private float getGroupPositionInit() => (float) (-46.5 * (double) this.dispGroupCount_ - 8.0);

  private bool movePage(int dir)
  {
    if (this.nodes_ == null || this.nodes_.Length <= 1)
      return false;
    this.isWaitArrowMv_ = true;
    this.waitStatusChange_ = 0.5f;
    Unit0042PickupMenu.UnitNode node = this.nodes_[this.currentIndex];
    node.target.localPosition = dir < 0 ? new Vector3((float) (-this.scroll_.content.itemSize * -dir), 0.0f, 0.0f) : new Vector3((float) (this.scroll_.content.itemSize * dir), 0.0f, 0.0f);
    // ISSUE: method pointer
    this.scroll_.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(OnCocFinished));
    this.scroll_.centerOnChild.CenterOn(node.target);
    this.setEnabledScrollViews(false);
    return true;
  }

  private void OnCocFinished()
  {
    this.isWaitArrowMv_ = false;
    this.scroll_.centerOnChild.onFinished = (SpringPanel.OnFinished) null;
    this.setEnabledScrollViews(true);
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void onClickedPlayDuelSkill()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.doPlayDuelSkill());
  }

  private IEnumerator doPlayDuelSkill()
  {
    Unit0042PickupMenu unit0042PickupMenu = this;
    Singleton<PopupManager>.GetInstance().open((GameObject) null, isViewBack: false);
    yield return (object) unit0042PickupMenu.duelDemoSettings_.preSetupLightmaps();
    PlayerUnitSkills koyuSkill = unit0042PickupMenu.dicDuelSkill_[unit0042PickupMenu.currentIndex];
    DuelEnvironment duelEnv = unit0042PickupMenu.duelDemoSettings_.makeEnvironment();
    Future<DuelResult> future = unit0042PickupMenu.duelDemoSettings_.makeResult(unit0042PickupMenu.currentUnit, koyuSkill);
    yield return (object) future.Wait();
    Singleton<PopupManager>.GetInstance().dismiss();
    if (future.Result == null)
    {
      unit0042PickupMenu.IsPush = false;
    }
    else
    {
      unit0042PickupMenu.isPlayDuelSkill_ = true;
      Singleton<NGSoundManager>.GetInstance().stopVoice();
      CommonRoot instance = Singleton<CommonRoot>.GetInstance();
      instance.ShowLoadingLayer(3);
      instance.isActiveFooter = false;
      Singleton<NGSceneManager>.GetInstance().changeScene(Unit0042PickupMenu.DuelSceneName, true, (object) future.Result, (object) duelEnv);
      unit0042PickupMenu.duelDemoSettings_.playBGM();
    }
  }

  public void onBackScene() => this.isPlayDuelSkill_ = false;

  public IEnumerator onEndSceneAsync()
  {
    if (!this.isPlayDuelSkill_)
    {
      this.finalizedDuelSettings();
      Singleton<NGSoundManager>.GetInstance().stopVoice();
      yield break;
    }
  }

  private void finalizedDuelSettings()
  {
    if (!this.duelDemoSettings_.isSetupedLightmaps)
      return;
    Singleton<NGDuelDataManager>.GetInstance().Init();
    this.duelDemoSettings_.clearLightmaps();
    Singleton<NGSceneManager>.GetInstance().destroyScene(Unit0042PickupMenu.DuelSceneName);
  }

  [Serializable]
  private class TabContainer
  {
    public UnitTypeEnum type;
    public GameObject top;
    public GameObject objDisabled;
    public UIButton button;
  }

  private enum StatusMode
  {
    FadeOut,
    Initialize,
    FadeIn,
    CancelFadeIn,
  }

  private class UnitPage
  {
    public int index;
    public bool isWaitLoad;
    public Unit0042PickupMenu.StatusMode mode;
    public NGTweenParts tween;

    public DetailMenuPrefab detail { get; private set; }

    public UnitPage(DetailMenuPrefab dm)
    {
      this.detail = dm;
      this.index = -1;
      this.isWaitLoad = false;
      this.mode = Unit0042PickupMenu.StatusMode.FadeOut;
    }
  }

  private class UnitNode
  {
    public GameObject center;
    public Transform target;
  }
}
