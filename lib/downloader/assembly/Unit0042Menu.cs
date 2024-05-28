// Decompiled with JetBrains decompiler
// Type: Unit0042Menu
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
using UnitDetails;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/unit004_2/Unit0042Menu")]
public class Unit0042Menu : BackButtonMenuBase
{
  [SerializeField]
  [Tooltip("これより先にシーン遷移させない")]
  private bool isTerminal;
  [SerializeField]
  protected UILabel txt_CharacterName;
  [SerializeField]
  protected UILabel txt_JobName;
  [SerializeField]
  protected UI2DSprite weaponTypeIcon;
  [SerializeField]
  protected UI2DSprite rarityStarsIcon;
  [SerializeField]
  protected GameObject slcAwakening;
  [SerializeField]
  private GameObject LeftArrow;
  [SerializeField]
  private GameObject RightArrow;
  [SerializeField]
  private UIButton zoomBtn;
  [SerializeField]
  private SpriteSelectDirectButton[] groupSprites;
  [SerializeField]
  private GameObject slc_GroupBase;
  private TweenHeight tween_GroupBaseOpen;
  private TweenHeight tween_GroupBaseClose;
  [SerializeField]
  private GameObject dir_GroupSprite;
  private TweenPosition tween_GroupSpriteDirOpen;
  private TweenPosition tween_GroupSpriteDirClose;
  [SerializeField]
  private UIButton IbtnGroupTabIdle;
  [SerializeField]
  private GameObject dir_GroupPressed;
  private bool isGroupOpen;
  private bool isGroupTween;
  private int dispGroupCount;
  private float groupSpriteHeight = 46.5f;
  private int groupBaseHeightInit = 36;
  private const int TWEEN_GROUPID_START = 100;
  private const int TWEEN_GROUPID_END = 101;
  private readonly int DISPLAY_OBJECT_MAX = 4;
  protected int objectCnt;
  private List<GameObject> objectList;
  private Dictionary<GameObject, DetailMenuPrefab> detailMenuPrefabDict;
  private GameObject gearKindIcon;
  public NGxScroll scrollView;
  [SerializeField]
  private UICenterOnChild centerOnChild;
  private WheelScrollLog wheelLog;
  [SerializeField]
  [Tooltip("null ならクラスチェンジボタン制御しない")]
  private UIButton btnToJobChange_;
  [SerializeField]
  private GameObject topJobChangeButton_;
  [SerializeField]
  [Tooltip("クラスチェンジボタンオブジェクト(0:アクティブ/1:ディセーブル)")]
  private GameObject[] jobChangeButtons_;
  private bool isControlledJobChange_;
  private bool isDisabledJobChange_;
  [SerializeField]
  private Transform lnkEffectJobChange_;
  [SerializeField]
  private Transform[] effectJobChangeAnchorParents_;
  private GameObject prefabEffectJobChange_;
  private PlayerItem equippedGear;
  private PlayerItem equippedGear2;
  private PlayerItem equippedGear3;
  private PlayerUnit[] unitList;
  private int countChangedSetting;
  private Dictionary<int, bool> firstSetting = new Dictionary<int, bool>();
  private Dictionary<int, bool> changeSetting = new Dictionary<int, bool>();
  public GameObject detailPrefab;
  public GameObject gearKindIconPrefab;
  public GameObject gearIconPrefab;
  public GameObject skillDetailDialogPrefab;
  public GameObject specialPointDetailDialogPrefab;
  public GameObject terraiAbilityDialogPrefab;
  public GameObject profIconPrefab;
  public GameObject skillTypeIconPrefab;
  public GameObject skillfullnessIconPrefab;
  public GameObject commonElementIconPrefab;
  public GameObject spAtkTypeIconPrefab;
  public GameObject modelPrefab;
  private GameObject popupCallSkillDetailsPrefab;
  protected GameObject statusDetailPrefab;
  private bool isFavorited;
  [SerializeField]
  private GameObject dirFavorite;
  [SerializeField]
  private GameObject btnFavoritedOff;
  [SerializeField]
  private GameObject btnFavoritedOn;
  [Header("オーバーキラーズベース")]
  [SerializeField]
  [Tooltip("特攻アイコン数に連動して繋ぎ変える")]
  private GameObject[] positionsOverkillersBase;
  [SerializeField]
  private GameObject btnRecommend;
  [SerializeField]
  private GameObject btnCallSkill;
  [SerializeField]
  [Tooltip("下部ステータス部をイラストのスクロールに連動するのを止める")]
  private Transform bottomBase;
  private Dictionary<GameObject, NGTweenParts> dicBottomTweenParts;
  private Dictionary<GameObject, Unit0042Menu.StatusMode> dicStatusMode;
  private OverkillersUnitBaseTag tagOverkillersBase;
  private string playerId_;
  private int currentOverkillersBase;
  private GameObject groupDetailDialogPrefab;
  private int voice_id;
  private int currentIndex;
  private int infoIndex;
  private bool lightON = true;
  private bool isArrowBtn = true;
  private Control? controlFlags_;
  private bool isInitializing = true;
  private bool isUpdateLastReference;
  private bool isDisabledScroll;
  private bool isEnabledBottomScroll;
  private bool isLockBottomScroll;
  private float? oldScrollViewLocalX;
  private bool isScrollViewDragStart;
  private int scrollStartCurrent;
  private bool isEnabledRecommend;
  private Func<bool> hookPreOnBackButton_;
  private bool isVRdrag;
  private int centerStatus = -1;
  private const float WAIT_STATUS_CHANGE = 0.5f;
  private float waitStatusChange;
  private List<int> cacheDLPlayerUnitIDs = new List<int>();
  private List<int> cacheDLUnitIDs = new List<int>();
  private int countDoStartStatusPanel_;
  private Quest00214aMenu questMenu_;
  private WebAPI.Response.QuestProgressCharacter questDatas_;
  private static bool isRequestedResetCharacterQuests;

  public bool IsTerminal => this.isTerminal;

  public bool isEarthMode { get; private set; }

  public bool IsFriend { get; private set; }

  public bool IsStorage { get; private set; }

  public bool IsLimitMode { get; private set; }

  public bool IsGvgMode { get; private set; }

  public bool IsMaterial { get; private set; }

  public bool IsMemory { get; private set; }

  public PlayerUnit[] UnitList => this.unitList;

  public PlayerUnit baseUnit { get; private set; }

  public Future<GameObject>[] unityDetailPrefabs { get; set; }

  public GameObject overkillersSlotReleasePrefab { get; set; }

  public GameObject buguReleaseDialogPrefab { get; set; }

  public GameObject unitIconPrefab { get; set; }

  public GameObject overkillersBaseTagPrefab { get; set; }

  public GameObject skillLockIconPrefab { get; set; }

  public GameObject[] recommendPrefabs { get; set; }

  public GameObject levelDetailPrefab { get; set; }

  public GameObject StatusDetailPrefab => this.statusDetailPrefab;

  private string playerId => this.playerId_ ?? (this.playerId_ = Player.Current.id);

  public GameObject GroupDetailDialogPrefab => this.groupDetailDialogPrefab;

  private int characterID { get; set; }

  public int CurrentIndex
  {
    set => this.currentIndex = value;
    get => this.currentIndex;
  }

  public int InfoIndex
  {
    set => this.infoIndex = value;
    get => this.infoIndex;
  }

  public PlayerUnit CurrentUnit
  {
    get
    {
      return this.unitList == null || this.unitList.Length <= this.currentIndex ? (PlayerUnit) null : this.unitList[this.currentIndex];
    }
  }

  public GameObject CurrentScrollObject
  {
    get
    {
      return this.detailMenuPrefabDict == null || this.CurrentIndex < 0 ? (GameObject) null : this.detailMenuPrefabDict.FirstOrDefault<KeyValuePair<GameObject, DetailMenuPrefab>>((Func<KeyValuePair<GameObject, DetailMenuPrefab>, bool>) (x => x.Value.Index == this.CurrentIndex)).Key;
    }
  }

  public DetailMenuScrollViewParam.TabMode viewParamTabMode { get; private set; }

  public DetailMenuJobTab.TabMode viewJobTabMode { get; private set; } = DetailMenuJobTab.TabMode.JobChange;

  public bool LightON
  {
    get => this.lightON;
    set => this.lightON = value;
  }

  public virtual Control controlFlags
  {
    get
    {
      return !this.controlFlags_.HasValue ? (this.controlFlags_ = new Control?(((Component) this).GetComponent<Unit0042Scene>().bootParam.controlFlags)).Value : this.controlFlags_.Value;
    }
  }

  public void UpdateInfoIndicator(int idx)
  {
    this.InfoIndex = idx;
    if (this.detailMenuPrefabDict == null)
      return;
    foreach (KeyValuePair<GameObject, DetailMenuPrefab> keyValuePair in this.detailMenuPrefabDict)
    {
      GameObject key = keyValuePair.Key;
      if (!key.activeSelf)
        key.SetActive(true);
      keyValuePair.Value.SetInformationPanelIndex(this.InfoIndex);
    }
  }

  public void UpdateInfoIndicator(DetailMenuScrollViewParam.TabMode mode)
  {
    this.viewParamTabMode = mode;
    if (this.detailMenuPrefabDict == null)
      return;
    foreach (DetailMenuPrefab detailMenuPrefab in this.detailMenuPrefabDict.Values)
      detailMenuPrefab.SetInformationPanelTab(mode);
  }

  public void UpdateInfoIndicator(DetailMenuJobTab.TabMode mode)
  {
    this.viewJobTabMode = mode;
    if (this.detailMenuPrefabDict == null)
      return;
    foreach (DetailMenuPrefab detailMenuPrefab in this.detailMenuPrefabDict.Values)
      detailMenuPrefab.SetInformationPanelTab(mode);
  }

  public void setHookPreOnBackButton(Func<bool> func) => this.hookPreOnBackButton_ = func;

  public void clearHookPreOnBackButton(Func<bool> func)
  {
    if (this.hookPreOnBackButton_ == null || !(this.hookPreOnBackButton_ == func))
      return;
    this.hookPreOnBackButton_ = (Func<bool>) null;
  }

  public virtual void IbtnBack()
  {
    if (this.hookPreOnBackButton_ != null && this.hookPreOnBackButton_() || this.IsPushAndSet())
      return;
    if (Singleton<NGSceneManager>.GetInstance().LastHeaderType.HasValue)
    {
      Singleton<CommonRoot>.GetInstance().headerType = Singleton<NGSceneManager>.GetInstance().LastHeaderType.Value;
      Singleton<NGSceneManager>.GetInstance().LastHeaderType = new CommonRoot.HeaderType?();
    }
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<CommonRoot>.GetInstance().setStartScene("sea030_home", new object[1]
      {
        (object) false
      });
    Unit00486Scene[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<Unit00486Scene>();
    if (objectsOfTypeAll.Length != 0)
      objectsOfTypeAll[0].NotOnStartSceneAsync = true;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnIntimacy()
  {
  }

  public virtual void IbtnWpEquip()
  {
  }

  public virtual void IbtnZoom()
  {
    if (this.IsPushAndSet())
      return;
    Unit0043Scene.changeScene(true, this.unitList[this.CurrentIndex], false);
  }

  public void IbtnLeftArrow()
  {
    if (!this.isArrowBtn)
      return;
    this.isArrowBtn = false;
    int num = this.CurrentIndex - 1;
    if (num >= 0 && this.CenterOnChild(num))
      return;
    this.StartCoroutine(this.IsArrowBtnOn());
  }

  public void IbtnRightArrow()
  {
    if (!this.isArrowBtn)
      return;
    this.isArrowBtn = false;
    int num = this.CurrentIndex + 1;
    if (num <= this.unitList.Length - 1 && this.CenterOnChild(num))
      return;
    this.StartCoroutine(this.IsArrowBtnOn());
  }

  private void OnCocFinished()
  {
    this.isArrowBtn = true;
    GameObject centeredObject1 = this.centerOnChild.centeredObject;
    if (!Object.op_Implicit((Object) centeredObject1))
      return;
    UIScrollView componentInChildren1 = centeredObject1.GetComponentInChildren<UIScrollView>();
    if (Object.op_Implicit((Object) componentInChildren1))
      ((Behaviour) componentInChildren1).enabled = true;
    UICenterOnChild componentInChildren2 = centeredObject1.GetComponentInChildren<UICenterOnChild>();
    GameObject centeredObject2;
    DetailMenuScrollViewBase component;
    if (!Object.op_Implicit((Object) componentInChildren2) || !Object.op_Implicit((Object) (centeredObject2 = componentInChildren2.centeredObject)) || !Object.op_Implicit((Object) (component = centeredObject2.GetComponent<DetailMenuScrollViewBase>())))
      return;
    component.MarkAsChanged();
  }

  protected IEnumerator IsArrowBtnOn()
  {
    yield return (object) new WaitForSeconds(0.2f);
    this.isArrowBtn = true;
    UIScrollView componentInChildren = this.centerOnChild.centeredObject.GetComponentInChildren<UIScrollView>();
    if (Object.op_Inequality((Object) componentInChildren, (Object) null))
      ((Behaviour) componentInChildren).enabled = true;
  }

  private IEnumerator UpdateActive(GameObject go)
  {
    go.SetActive(false);
    yield return (object) null;
    go.SetActive(true);
  }

  private void SetTitleBarInfo(bool isSe, PlayerUnit target = null)
  {
    if (target == (PlayerUnit) null)
      target = this.CurrentUnit;
    this.LeftArrow.SetActive(true);
    this.RightArrow.SetActive(true);
    if (this.CurrentIndex == 0)
      this.LeftArrow.SetActive(false);
    if (this.CurrentIndex >= this.unitList.Length - 1)
      this.RightArrow.SetActive(false);
    if (this.isUpdateLastReference)
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = target.id;
    UnitUnit unit = target.unit;
    if (string.IsNullOrEmpty(unit.formal_name))
      this.txt_CharacterName.SetText(unit.name);
    else
      this.txt_CharacterName.SetText(unit.formal_name);
    if (Object.op_Inequality((Object) this.txt_JobName, (Object) null))
      this.txt_JobName.SetText(target.getJobData().name);
    this.gearKindIcon.SetActive(false);
    ((Component) this.rarityStarsIcon).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) this.slcAwakening, (Object) null))
      this.slcAwakening.SetActive(false);
    if (unit.IsNormalUnit)
    {
      this.gearKindIcon.SetActive(true);
      ((Component) this.rarityStarsIcon).gameObject.SetActive(true);
      if (Object.op_Inequality((Object) this.slcAwakening, (Object) null))
        this.slcAwakening.SetActive(unit.awake_unit_flag);
      this.gearKindIcon.GetComponent<GearKindIcon>().Init(unit.kind, target.GetElement());
      RarityIcon.SetRarity(target, this.rarityStarsIcon, true);
    }
    if (Object.op_Inequality((Object) this.dirFavorite, (Object) null))
    {
      if (this.IsFriend || this.IsLimitMode)
      {
        this.dirFavorite.SetActive(false);
      }
      else
      {
        this.dirFavorite.SetActive(true);
        this.SetFavorite(this.changeSetting[target.id]);
      }
    }
    this.DisplayGroupLogo(unit);
    if (Object.op_Inequality((Object) this.btnCallSkill, (Object) null))
    {
      if (this.IsFriend || this.IsGvgMode || target.is_guest)
        this.btnCallSkill.SetActive(false);
      else if (((IEnumerable<int>) SMManager.Get<Player>().call_skill_same_character_ids).Contains<int>(unit.same_character_id))
        this.btnCallSkill.SetActive(true);
      else
        this.btnCallSkill.SetActive(false);
    }
    this.setOverkillersBase(target);
    Unit0042Scene component = ((Component) this).GetComponent<Unit0042Scene>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.bootParam.playerUnit = target;
    if (this.isControlledJobChange_)
    {
      int index = JobChangeUtil.getJobChangePatterns(target) != null ? (this.isDisabledJobChange_ ? 1 : 0) : -1;
      ((IEnumerable<GameObject>) this.jobChangeButtons_).ToggleOnce(index);
      switch (index)
      {
        case 0:
          EventDelegate.Set(this.btnToJobChange_.onClick, (EventDelegate.Callback) (() =>
          {
            if (this.IsPushAndSet())
              return;
            Unit004JobChangeScene.changeScene(true, target.id, eventBackScene: (Action) (() =>
            {
              if (!Singleton<NGSceneManager>.GetInstance().SceneStack.Any<NGSceneManager.SceneWrapper>())
              {
                NGSceneManager.SceneLog aliveSceneLog = Singleton<NGSceneManager>.GetInstance().FindAliveSceneLog();
                if (aliveSceneLog != null && aliveSceneLog.args[2] is Unit0042Scene.BootParam bootParam2)
                  bootParam2.playerUnits = DetailMenu.CreateUpdatedUnitList(bootParam2.playerUnits);
              }
              NGMenuBase.backSceneBase();
            }));
          }));
          break;
        case 1:
          this.btnToJobChange_.onClick.Clear();
          break;
      }
    }
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (isSe)
      instance.playSE(Singleton<NGGameDataManager>.GetInstance().IsSea ? "SE_1043" : "SE_1005");
    this.ChangeCharacterVoice(unit, instance);
    if (!Object.op_Inequality((Object) this.zoomBtn, (Object) null))
      return;
    if ((component.bootParam.isMyGvgAtkDeck || component.bootParam.isMyGvgDefDeck) && component.bootParam.limited)
      ((UIButtonColor) this.zoomBtn).isEnabled = false;
    else
      ((UIButtonColor) this.zoomBtn).isEnabled = true;
  }

  private void setOverkillersBase(PlayerUnit currentUnit)
  {
    if (this.controlFlags.IsOff(Control.OverkillersBase))
      return;
    int baseUnitId = currentUnit.is_storage || !(currentUnit.player_id == this.playerId) ? -1 : currentUnit.overkillers_base_id;
    PlayerUnit playerUnit = baseUnitId > 0 ? Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == baseUnitId)) : (PlayerUnit) null;
    if (playerUnit != (PlayerUnit) null)
    {
      if (Object.op_Equality((Object) this.tagOverkillersBase, (Object) null))
      {
        this.tagOverkillersBase = this.overkillersBaseTagPrefab.Clone(this.positionsOverkillersBase[0].transform).GetComponent<OverkillersUnitBaseTag>();
        this.tagOverkillersBase.initialize(this.unitIconPrefab, this.scrollView.scrollView, new Action(this.onClickedOverkillersBaseIcon), new Action(this.onLongPressedOverkillersBaseIcon));
      }
      this.StopCoroutine("doSetOverkillersBase");
      DateTime dateTime = ServerTime.NowAppTime();
      this.changePositionOverkillersBase(!string.IsNullOrEmpty(currentUnit.SpecialEffectType((IEnumerable<QuestScoreBonusTimetable>) this.getActiveQuestScoreBonus(dateTime), (IEnumerable<UnitBonus>) UnitBonus.getActiveUnitBonus(dateTime))) ? 1 : 0);
      this.StartCoroutine("doSetOverkillersBase", (object) playerUnit);
    }
    else
      this.setActiveOverkilersBase(false);
  }

  private IEnumerator doSetOverkillersBase(PlayerUnit unit)
  {
    IEnumerator e = this.tagOverkillersBase.doReset(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void changePositionOverkillersBase(int index)
  {
    if (this.currentOverkillersBase == index || this.positionsOverkillersBase == null || this.positionsOverkillersBase.Length <= index)
      return;
    this.setActiveOverkilersBase(false);
    ((Component) this.tagOverkillersBase).gameObject.SetParentSafeLocalTransform(this.positionsOverkillersBase[index]);
    this.currentOverkillersBase = index;
  }

  private void setActiveOverkilersBase(bool flag)
  {
    if (!Object.op_Inequality((Object) this.tagOverkillersBase, (Object) null))
      return;
    ((Component) this.tagOverkillersBase).gameObject.SetActive(flag);
  }

  private void onClickedOverkillersBaseIcon() => this.onLongPressedOverkillersBaseIcon();

  private void onLongPressedOverkillersBaseIcon()
  {
    if (this.controlFlags.IsOff(Control.OverkillersMove))
      return;
    PlayerUnit currentUnit = this.CurrentUnit;
    if (!(currentUnit != (PlayerUnit) null))
      return;
    this.moveUnitPage(currentUnit.overkillers_base_id, this.tagOverkillersBase.objButton);
  }

  public void moveUnitPage(int unitId, GameObject objButton)
  {
    if (unitId <= 0)
      return;
    PlayerUnit target = Array.Find<PlayerUnit>(this.unitList, (Predicate<PlayerUnit>) (x => x.id == unitId));
    UIDragScrollView component = Object.op_Inequality((Object) objButton, (Object) null) ? objButton.GetComponent<UIDragScrollView>() : (UIDragScrollView) null;
    bool flag = Object.op_Inequality((Object) component, (Object) null);
    UIEventTrigger uiTrigger;
    if (flag)
    {
      ((Component) component).SendMessage("OnPress", (object) false);
      ((Behaviour) component).enabled = false;
      uiTrigger = objButton.GetOrAddComponent<UIEventTrigger>();
    }
    else
      uiTrigger = (UIEventTrigger) null;
    Action actMoveFinished = flag ? (Action) (() =>
    {
      if (!Object.op_Inequality((Object) objButton, (Object) null))
        return;
      ((Behaviour) objButton.GetComponent<UIDragScrollView>()).enabled = true;
    }) : (Action) (() => { });
    if (Object.op_Inequality((Object) uiTrigger, (Object) null))
      EventDelegate.Set(uiTrigger.onRelease, (EventDelegate.Callback) (() =>
      {
        actMoveFinished();
        Object.Destroy((Object) uiTrigger);
      }));
    this.StartCoroutine(this.doMoveUnitPage(target, actMoveFinished));
  }

  private IEnumerator doMoveUnitPage(PlayerUnit target, Action actMoveFinished)
  {
    if (target == (PlayerUnit) null)
    {
      Consts instance = Consts.GetInstance();
      yield return (object) PopupCommon.Show(instance.POPUP_004_TITLE_ERROR_MOVE_UNITPAGE, instance.POPUP_004_MESSAGE_ERROR_MOVE_UNITPAGE, actMoveFinished);
    }
    else
    {
      this.isLockBottomScroll = true;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      if (Object.op_Inequality((Object) this.bottomBase, (Object) null))
      {
        this.centerStatus = -1;
        this.startStatusFadeOut((GameObject) null, false);
      }
      float waitRestart = Time.fixedTime + 0.5f;
      int targetIndex = Array.FindIndex<PlayerUnit>(this.unitList, (Predicate<PlayerUnit>) (x => x.id == target.id));
      if (Mathf.Abs(targetIndex - this.currentIndex) > 1 || !this.CenterOnChild(targetIndex))
      {
        yield return (object) this.downloadOwnResource(targetIndex);
        this.UpdateObjectList(targetIndex);
        yield return (object) this.CreatePage(targetIndex);
        this.CenterOnChild(targetIndex, true);
        while (targetIndex != this.currentIndex)
          yield return (object) null;
        for (int n = targetIndex - 1; n <= targetIndex + 1 && n < this.unitList.Length; ++n)
        {
          if (n >= 0 && n != targetIndex)
          {
            this.UpdateObjectList(n);
            yield return (object) this.CreatePage(n);
          }
        }
        foreach (UIScrollView componentsInChild in this.detailMenuPrefabDict.First<KeyValuePair<GameObject, DetailMenuPrefab>>((Func<KeyValuePair<GameObject, DetailMenuPrefab>, bool>) (x => x.Value.Index == targetIndex)).Key.GetComponentsInChildren<UIScrollView>())
        {
          if (((Component) componentsInChild).gameObject.activeInHierarchy)
          {
            ((Component) componentsInChild).gameObject.SetActive(false);
            ((Component) componentsInChild).gameObject.SetActive(true);
          }
        }
        yield return (object) null;
      }
      else
      {
        SpringPanel component;
        while (targetIndex != this.currentIndex || Object.op_Equality((Object) (component = ((Component) this.scrollView.scrollView).GetComponent<SpringPanel>()), (Object) null) || ((Behaviour) component).enabled)
          yield return (object) null;
      }
      while ((double) waitRestart > (double) Time.fixedTime)
        yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      this.isLockBottomScroll = false;
    }
  }

  private void ChangeCharacterVoice(UnitUnit unit, NGSoundManager sm)
  {
    if (unit.IsMaterialUnit)
      return;
    int id = unit.character.ID;
    UnitVoicePattern unitVoicePattern = unit.unitVoicePattern;
    if (unitVoicePattern == null || this.characterID == id && this.voice_id == unitVoicePattern.ID)
      return;
    this.characterID = id;
    this.voice_id = unitVoicePattern.ID;
    sm.stopVoice();
    sm.playVoiceByID(unitVoicePattern, 42);
  }

  private void UpdateObjectList()
  {
    foreach (KeyValuePair<GameObject, DetailMenuPrefab> keyValuePair in this.detailMenuPrefabDict)
    {
      int index = keyValuePair.Value.Index;
      if ((index < this.CurrentIndex - 1 || index > this.CurrentIndex + 1) && !this.objectList.Contains(keyValuePair.Key))
        this.objectList.Add(keyValuePair.Key);
    }
  }

  private void UpdateObjectList(int includeIndex)
  {
    foreach (KeyValuePair<GameObject, DetailMenuPrefab> keyValuePair in this.detailMenuPrefabDict)
    {
      int index = keyValuePair.Value.Index;
      if (index == includeIndex)
      {
        if (this.objectList.Contains(keyValuePair.Key))
          this.objectList.Remove(keyValuePair.Key);
        this.objectList.Insert(0, keyValuePair.Key);
      }
      else if ((index < this.CurrentIndex - 1 || index > this.CurrentIndex + 1) && !this.objectList.Contains(keyValuePair.Key))
        this.objectList.Add(keyValuePair.Key);
    }
  }

  protected override void Update()
  {
    if (this.isInitializing)
    {
      this.isEnabledBottomScroll = this.scrollView.scrollView.isDragging;
    }
    else
    {
      base.Update();
      if (this.isDisabledScroll)
        return;
      int num1 = this.CurrentIndex;
      float x = ((Component) this.scrollView.scrollView).transform.localPosition.x;
      if (this.oldScrollViewLocalX.HasValue)
      {
        double num2 = (double) x;
        float? scrollViewLocalX = this.oldScrollViewLocalX;
        double valueOrDefault = (double) scrollViewLocalX.GetValueOrDefault();
        if (num2 == valueOrDefault & scrollViewLocalX.HasValue)
        {
          this.isVRdrag = false;
          goto label_8;
        }
      }
      int num3 = -(int) (((double) x - (double) this.scrollView.grid.cellWidth / 2.0) / (double) this.scrollView.grid.cellWidth);
      num1 = num3 < 0 ? 0 : (num3 < this.unitList.Length ? num3 : this.unitList.Length - 1);
      this.oldScrollViewLocalX = new float?(x);
label_8:
      if (this.detailMenuPrefabDict != null && this.controlFlags.IsOff(Control.OverkillersUnit))
      {
        if (Object.op_Inequality((Object) this.wheelLog, (Object) null))
        {
          this.isVRdrag |= (double) this.wheelLog.amount != 0.0;
          this.wheelLog.resetAmount();
        }
        this.isVRdrag |= this.scrollView.scrollView.isDragging;
        bool enable = !this.isVRdrag && !this.isLockBottomScroll;
        if (this.isEnabledBottomScroll != enable)
        {
          if (Object.op_Equality((Object) this.bottomBase, (Object) null))
          {
            foreach (DetailMenuPrefab detailMenuPrefab in this.detailMenuPrefabDict.Values)
              detailMenuPrefab.SetInformationPaneEnable(enable, detailMenuPrefab.Index < 0 || detailMenuPrefab.isInitalizing ? new int?() : new int?(this.InfoIndex));
          }
          else if (!enable)
          {
            this.centerStatus = -1;
            this.startStatusFadeOut((GameObject) null, false);
          }
          this.isEnabledBottomScroll = enable;
        }
      }
      if (this.CurrentIndex != num1)
      {
        int num4 = this.CurrentIndex < num1 ? 1 : 0;
        this.CurrentIndex = num1;
        this.SetTitleBarInfo(true);
        if (num4 != 0)
        {
          if (this.CurrentIndex < this.unitList.Length - 1)
          {
            this.UpdateObjectList();
            this.StartCoroutine(this.CreatePage(this.CurrentIndex + 1, isDownloadResourceCheck: true));
          }
        }
        else if (this.CurrentIndex > 0)
        {
          this.UpdateObjectList();
          this.StartCoroutine(this.CreatePage(this.CurrentIndex - 1, isDownloadResourceCheck: true));
        }
      }
      if (this.scrollView.scrollView.isDragging)
      {
        if (!this.isScrollViewDragStart)
        {
          this.isScrollViewDragStart = true;
          this.scrollStartCurrent = this.CurrentIndex;
        }
      }
      else
      {
        if (this.isScrollViewDragStart && this.scrollStartCurrent == this.CurrentIndex)
        {
          int currentIndex = this.CurrentIndex;
          double num5 = -(double) this.scrollView.grid.cellWidth * (double) this.CurrentIndex;
          float num6 = this.scrollView.grid.cellWidth * 0.25f;
          float num7 = (float) num5 - num6;
          float num8 = (float) num5 + num6;
          if ((double) ((Component) this.scrollView.scrollView).transform.localPosition.x <= (double) num7)
            ++currentIndex;
          else if ((double) ((Component) this.scrollView.scrollView).transform.localPosition.x >= (double) num8)
            --currentIndex;
          this.CenterOnChild(currentIndex <= this.unitList.Length ? currentIndex : this.unitList.Length - 1);
        }
        if (this.isScrollViewDragStart)
          this.waitStatusChange = 0.5f;
        this.isScrollViewDragStart = false;
      }
      if (!Object.op_Inequality((Object) this.bottomBase, (Object) null))
        return;
      if ((double) this.waitStatusChange > 0.0)
        this.waitStatusChange -= Time.deltaTime;
      if (!this.isEnabledBottomScroll || (double) this.waitStatusChange > 0.0 || this.currentIndex < 0 || this.centerStatus == this.currentIndex)
        return;
      GameObject currentScrollObject = this.CurrentScrollObject;
      this.startStatusFadeOut(currentScrollObject, true);
      this.StartCoroutine(this.doStartStatusPanel(currentScrollObject, false));
      this.centerStatus = this.currentIndex;
    }
  }

  private void CreateFirstFavoriteSetting()
  {
    this.countChangedSetting = 0;
    this.firstSetting.Clear();
    this.changeSetting.Clear();
    foreach (PlayerUnit playerUnit in ((IEnumerable<PlayerUnit>) this.unitList).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)))
    {
      this.firstSetting.Add(playerUnit.id, playerUnit.favorite);
      this.changeSetting.Add(playerUnit.id, playerUnit.favorite);
    }
  }

  public bool GetSetting(int id) => this.changeSetting[id];

  public void UpdateSetting(int id, bool flg)
  {
    if (this.changeSetting[id] == flg)
      return;
    this.changeSetting[id] = flg;
    ++this.countChangedSetting;
  }

  public void UploadFavorites(Action endWait)
  {
    this.StartCoroutine(this.doUploadFavorites(endWait));
  }

  public IEnumerator doUploadFavorites(Action endWait = null)
  {
    if (this.countChangedSetting == 0)
    {
      Action action = endWait;
      if (action != null)
        action();
    }
    else
    {
      this.countChangedSetting = 0;
      List<int> source1 = new List<int>();
      List<int> source2 = new List<int>();
      foreach (KeyValuePair<int, bool> keyValuePair in this.firstSetting)
      {
        bool flag = this.changeSetting[keyValuePair.Key];
        if (flag != keyValuePair.Value)
        {
          if (flag)
            source1.Add(keyValuePair.Key);
          else
            source2.Add(keyValuePair.Key);
        }
      }
      if (!source1.Any<int>() && !source2.Any<int>())
      {
        Action action = endWait;
        if (action != null)
          action();
      }
      else
      {
        foreach (KeyValuePair<int, bool> keyValuePair in this.changeSetting)
          this.firstSetting[keyValuePair.Key] = keyValuePair.Value;
        int[] array1 = source1.ToArray();
        int[] array2 = source2.ToArray();
        if (endWait == null)
          Singleton<CommonRoot>.GetInstance().loadingMode = 1;
        if (this.IsStorage)
          yield return (object) WebAPI.UnitReservesFavorite(array1, array2).Wait();
        else
          yield return (object) WebAPI.UnitFavorite(array1, array2).Wait();
        if (endWait == null)
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        else
          endWait();
      }
    }
  }

  protected virtual IEnumerator LoadPrefabs()
  {
    Unit0042Menu unit0042Menu = this;
    Future<GameObject> loader = (Future<GameObject>) null;
    bool is_sea = Singleton<NGGameDataManager>.GetInstance().IsSea;
    IEnumerator e;
    if (is_sea)
    {
      loader = new ResourceObject("Prefabs/unit004_2_sea/detail_sea").Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      loader = new ResourceObject("Prefabs/unit004_2/detail").Load<GameObject>();
      e = loader.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    unit0042Menu.detailPrefab = loader.Result;
    loader = !is_sea ? Res.Prefabs.ItemIcon.prefab.Load<GameObject>() : new ResourceObject("Prefabs/Sea/ItemIcon/prefab_sea").Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.gearIconPrefab = loader.Result;
    loader = Res.Icons.GearKindIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.gearKindIconPrefab = loader.Result;
    loader = PopupSkillDetails.createPrefabLoader(is_sea);
    yield return (object) loader.Wait();
    unit0042Menu.skillDetailDialogPrefab = loader.Result;
    loader = is_sea ? new ResourceObject("Prefabs/unit004_2_sea/SpecialPoint_DetailDialog_sea").Load<GameObject>() : new ResourceObject("Prefabs/unit004_2/SpecialPoint_DetailDialog").Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.specialPointDetailDialogPrefab = loader.Result;
    loader = is_sea ? new ResourceObject("Prefabs/unit004_2_sea/TerraiAbilityDialog_sea").Load<GameObject>() : new ResourceObject("Prefabs/unit004_2/TerraiAbilityDialog").Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.terraiAbilityDialogPrefab = loader.Result;
    loader = Res.Icons.GearProfiencyIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.profIconPrefab = loader.Result;
    loader = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.skillTypeIconPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/SkillFamily/SkillFamilyIcon").Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.skillfullnessIconPrefab = loader.Result;
    loader = Res.Icons.CommonElementIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.commonElementIconPrefab = loader.Result;
    loader = Res.Icons.SPAtkTypeIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.spAtkTypeIconPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/unit004_2/GroupDetailDialog").Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.groupDetailDialogPrefab = loader.Result;
    loader = is_sea ? Res.Prefabs.unit.dir_unit_status_detail_sea.Load<GameObject>() : new ResourceObject("Prefabs/unit/dir_X_unit_status_detail").Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.statusDetailPrefab = loader.Result;
    unit0042Menu.unityDetailPrefabs = PopupUnityValueDetail.createLoaders(is_sea);
    Future<GameObject>[] futureArray = unit0042Menu.unityDetailPrefabs;
    for (int index = 0; index < futureArray.Length; ++index)
      yield return (object) futureArray[index].Wait();
    futureArray = (Future<GameObject>[]) null;
    loader = PopupOverkillersSlotRelease.createLoader(is_sea);
    yield return (object) loader.Wait();
    unit0042Menu.overkillersSlotReleasePrefab = loader.Result;
    loader = PopupBuguSlotRelease.createLoader(is_sea);
    yield return (object) loader.Wait();
    unit0042Menu.buguReleaseDialogPrefab = loader.Result;
    loader = is_sea ? Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>() : Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    yield return (object) loader.Wait();
    unit0042Menu.unitIconPrefab = loader.Result;
    loader = OverkillersUnitBaseTag.createLoander(is_sea);
    yield return (object) loader.Wait();
    unit0042Menu.overkillersBaseTagPrefab = loader.Result;
    loader = new ResourceObject("Prefabs/BattleSkillIcon/dir_SkillLock").Load<GameObject>();
    yield return (object) loader.Wait();
    unit0042Menu.skillLockIconPrefab = loader.Result;
    loader = PopupXLevelDetail.createLoader();
    yield return (object) loader.Wait();
    unit0042Menu.levelDetailPrefab = loader.Result;
    if (unit0042Menu.isEnabledRecommend)
    {
      loader = PopupRecommendMenu.loadResource();
      yield return (object) loader.Wait();
      unit0042Menu.recommendPrefabs = new GameObject[1]
      {
        loader.Result
      };
    }
  }

  public void preOpenUnityPopup() => this.IsPush = true;

  public void preOpenRecomendPopup() => this.IsPush = true;

  public void preOpenCharacterQuestPopup() => this.IsPush = true;

  public IEnumerator Init(
    PlayerUnit playerUnit,
    PlayerUnit[] playerUnits,
    bool isFriend,
    bool limitMode,
    bool gvgMode,
    bool isEarthMode,
    bool isMaterial = false,
    PlayerItem equippedGear = null,
    PlayerItem equippedGear2 = null,
    PlayerItem equippedGear3 = null,
    bool isMemory = false,
    PlayerUnit baseUnit = null)
  {
    Unit0042Menu unit0042Menu = this;
    unit0042Menu.isInitializing = true;
    unit0042Menu.unitList = playerUnits;
    unit0042Menu.IsFriend = isFriend;
    unit0042Menu.IsLimitMode = limitMode | isMemory;
    unit0042Menu.IsGvgMode = gvgMode;
    unit0042Menu.equippedGear = equippedGear;
    unit0042Menu.equippedGear2 = equippedGear2;
    unit0042Menu.equippedGear3 = equippedGear3;
    unit0042Menu.IsStorage = playerUnit.is_storage;
    unit0042Menu.IsMaterial = isMaterial | isMemory | isFriend;
    unit0042Menu.IsMemory = isMemory;
    unit0042Menu.isEarthMode = isEarthMode;
    unit0042Menu.baseUnit = baseUnit;
    unit0042Menu.setEnabledArrowAnimation(true);
    unit0042Menu.isControlledJobChange_ = Object.op_Implicit((Object) unit0042Menu.btnToJobChange_);
    if (unit0042Menu.isControlledJobChange_)
    {
      if (unit0042Menu.controlFlags.IsOn(Control.CustomDeck) || unit0042Menu.IsMaterial || unit0042Menu.IsLimitMode || playerUnit.is_storage || Singleton<NGGameDataManager>.GetInstance().IsColosseum)
        unit0042Menu.isControlledJobChange_ = false;
      unit0042Menu.isDisabledJobChange_ = Singleton<NGSceneManager>.GetInstance().isMatchSceneNameInStack("^(unit004_training|unit004_JobChange)$");
      ((IEnumerable<GameObject>) unit0042Menu.jobChangeButtons_).ToggleOnce(-1);
      ((UIRect) unit0042Menu.topJobChangeButton_.GetComponent<UIWidget>()).alpha = 1f;
    }
    if (Object.op_Implicit((Object) unit0042Menu.slc_GroupBase))
    {
      foreach (TweenHeight componentsInChild in unit0042Menu.slc_GroupBase.GetComponentsInChildren<TweenHeight>())
      {
        if (((UITweener) componentsInChild).tweenGroup == 100)
          unit0042Menu.tween_GroupBaseOpen = componentsInChild;
        if (((UITweener) componentsInChild).tweenGroup == 101)
          unit0042Menu.tween_GroupBaseClose = componentsInChild;
      }
      foreach (TweenPosition componentsInChild in unit0042Menu.dir_GroupSprite.GetComponentsInChildren<TweenPosition>())
      {
        if (((UITweener) componentsInChild).tweenGroup == 100)
          unit0042Menu.tween_GroupSpriteDirOpen = componentsInChild;
        if (((UITweener) componentsInChild).tweenGroup == 101)
          unit0042Menu.tween_GroupSpriteDirClose = componentsInChild;
      }
    }
    if (unit0042Menu.IsMemory && PlayerTransmigrateMemoryPlayerUnitIds.Current != null)
      PlayerTransmigrateMemoryPlayerUnitIds.Current.AddMemoryData(playerUnit);
    if (Object.op_Inequality((Object) unit0042Menu.btnRecommend, (Object) null))
    {
      unit0042Menu.isEnabledRecommend = !(isFriend | limitMode | isMemory) && !unit0042Menu.IsStorage && !(playerUnit.player_id != Player.Current.id);
      unit0042Menu.btnRecommend.SetActive(unit0042Menu.isEnabledRecommend);
    }
    else
      unit0042Menu.isEnabledRecommend = false;
    IEnumerator e = unit0042Menu.LoadPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0042Menu.gearKindIcon = unit0042Menu.gearKindIconPrefab.Clone(((Component) unit0042Menu.weaponTypeIcon).transform);
    ((UIWidget) unit0042Menu.gearKindIcon.GetComponent<UI2DSprite>()).depth = ((UIWidget) unit0042Menu.weaponTypeIcon).depth + 1;
    unit0042Menu.CreateFirstFavoriteSetting();
    unit0042Menu.objectCnt = ((IEnumerable<PlayerUnit>) unit0042Menu.unitList).Count<PlayerUnit>();
    if (unit0042Menu.objectCnt > unit0042Menu.DISPLAY_OBJECT_MAX)
      unit0042Menu.objectCnt = unit0042Menu.DISPLAY_OBJECT_MAX;
    unit0042Menu.objectList = new List<GameObject>(unit0042Menu.objectCnt);
    if (Object.op_Inequality((Object) unit0042Menu.bottomBase, (Object) null))
    {
      unit0042Menu.dicStatusMode = new Dictionary<GameObject, Unit0042Menu.StatusMode>(unit0042Menu.objectCnt);
      if (unit0042Menu.dicBottomTweenParts != null)
      {
        foreach (GameObject gameObject in unit0042Menu.dicBottomTweenParts.Values.Select<NGTweenParts, GameObject>((Func<NGTweenParts, GameObject>) (x => !Object.op_Inequality((Object) x, (Object) null) ? (GameObject) null : ((Component) x).gameObject)))
        {
          if (!Object.op_Equality((Object) gameObject, (Object) null))
          {
            gameObject.SetActive(false);
            Object.Destroy((Object) gameObject);
          }
        }
        unit0042Menu.dicBottomTweenParts.Clear();
      }
      else
        unit0042Menu.dicBottomTweenParts = new Dictionary<GameObject, NGTweenParts>(unit0042Menu.objectCnt);
    }
    if (unit0042Menu.detailMenuPrefabDict != null)
    {
      foreach (GameObject key in unit0042Menu.detailMenuPrefabDict.Keys)
      {
        if (!Object.op_Equality((Object) key, (Object) null))
        {
          key.SetActive(false);
          Object.Destroy((Object) key);
        }
      }
      unit0042Menu.detailMenuPrefabDict.Clear();
    }
    else
      unit0042Menu.detailMenuPrefabDict = new Dictionary<GameObject, DetailMenuPrefab>(unit0042Menu.objectCnt);
    for (int index = 0; index < unit0042Menu.objectCnt; ++index)
    {
      GameObject key = unit0042Menu.detailPrefab.Clone();
      unit0042Menu.objectList.Add(key);
      unit0042Menu.scrollView.Add(key);
      DetailMenuPrefab component = key.GetComponent<DetailMenuPrefab>();
      DetailMenu normal = component.normal;
      unit0042Menu.detailMenuPrefabDict.Add(key, component);
      normal.ModelPos = new Vector3((float) (1000 * index), 0.0f, 0.0f);
      normal.isEarthMode = isEarthMode;
      normal.isMemory = unit0042Menu.IsMemory;
      if (Object.op_Inequality((Object) unit0042Menu.bottomBase, (Object) null))
      {
        GameObject gameObject = normal.purgeStatusPanel(unit0042Menu.scrollView.scrollView.panel.GetViewSize(), unit0042Menu.bottomBase);
        unit0042Menu.dicBottomTweenParts[key] = gameObject.GetComponent<NGTweenParts>();
        unit0042Menu.dicStatusMode[key] = Unit0042Menu.StatusMode.FadeOut;
      }
    }
    int? nullable = ((IEnumerable<PlayerUnit>) unit0042Menu.unitList).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == playerUnit.id));
    unit0042Menu.CurrentIndex = nullable.HasValue ? nullable.Value : 0;
    unit0042Menu.isUpdateLastReference = unit0042Menu.controlFlags.IsOff(Control.OverkillersUnit | Control.CustomDeck);
    if (unit0042Menu.isUpdateLastReference)
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = unit0042Menu.CurrentIndex;
    int start = unit0042Menu.CurrentIndex - 1 < 0 ? 0 : unit0042Menu.CurrentIndex - 1;
    int end = unit0042Menu.CurrentIndex + 1 >= ((IEnumerable<PlayerUnit>) unit0042Menu.unitList).Count<PlayerUnit>() ? ((IEnumerable<PlayerUnit>) unit0042Menu.unitList).Count<PlayerUnit>() - 1 : unit0042Menu.CurrentIndex + 1;
    unit0042Menu.lightON = true;
    yield return (object) unit0042Menu.downloadOwnResource(unit0042Menu.CurrentIndex);
    e = unit0042Menu.CreatePage(unit0042Menu.CurrentIndex, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unit0042Menu.isUpdateLastReference)
      unit0042Menu.CurrentIndex = Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex;
    ((Component) unit0042Menu.scrollView.scrollView).transform.localPosition = new Vector3(-unit0042Menu.scrollView.grid.cellWidth * (float) unit0042Menu.CurrentIndex, 0.0f, 0.0f);
    if (Object.op_Inequality((Object) unit0042Menu.bottomBase, (Object) null))
    {
      unit0042Menu.countDoStartStatusPanel_ = 0;
      yield return (object) unit0042Menu.doStartStatusPanel(unit0042Menu.CurrentScrollObject, true);
      unit0042Menu.centerStatus = unit0042Menu.currentIndex;
    }
    unit0042Menu.lightON = false;
    for (int unitIdx = start; unitIdx <= end; ++unitIdx)
    {
      if (unitIdx != unit0042Menu.CurrentIndex)
        unit0042Menu.StartCoroutine(unit0042Menu.CreatePage(unitIdx));
    }
    unit0042Menu.SetTitleBarInfo(false);
    for (int index = 0; index < unit0042Menu.objectList.Count; ++index)
      unit0042Menu.objectList[index].transform.localPosition = end < unit0042Menu.unitList.Length - 1 ? new Vector3(unit0042Menu.scrollView.grid.cellWidth * (float) (end + index + 1), 0.0f, 0.0f) : new Vector3(unit0042Menu.scrollView.grid.cellWidth * (float) (start - (index + 1)), 0.0f, 0.0f);
    if (!((Behaviour) unit0042Menu.scrollView.scrollView).enabled && unit0042Menu.unitList.Length > 1)
      ((Behaviour) unit0042Menu.scrollView.scrollView).enabled = true;
    unit0042Menu.isDisabledScroll = !((Behaviour) unit0042Menu.scrollView.scrollView).enabled || unit0042Menu.unitList.Length <= 1;
    unit0042Menu.wheelLog = !unit0042Menu.isDisabledScroll ? ((Component) unit0042Menu.scrollView.scrollView).GetComponent<WheelScrollLog>() : (WheelScrollLog) null;
    unit0042Menu.isInitializing = false;
    if (!playerUnit.unit.IsMaterialUnit && !Persist.tutorial.Data.Hints.ContainsKey("unit004_2"))
      Singleton<TutorialRoot>.GetInstance().ShowAdvice("unit004_2_unit");
    if (unit0042Menu.controlFlags.IsOn(Control.CustomDeck))
    {
      if (Object.op_Implicit((Object) unit0042Menu.btnFavoritedOff))
        ((UIButtonColor) unit0042Menu.btnFavoritedOff.GetComponentInChildren<UIButton>(true)).isEnabled = false;
      if (Object.op_Implicit((Object) unit0042Menu.btnFavoritedOn))
        ((UIButtonColor) unit0042Menu.btnFavoritedOn.GetComponentInChildren<UIButton>(true)).isEnabled = false;
    }
  }

  public virtual IEnumerator CreatePage(int unitIdx, bool isUpdate = true, bool isDownloadResourceCheck = false)
  {
    Unit0042Menu menu = this;
    KeyValuePair<GameObject, DetailMenuPrefab> keyValuePair = menu.detailMenuPrefabDict.FirstOrDefault<KeyValuePair<GameObject, DetailMenuPrefab>>((Func<KeyValuePair<GameObject, DetailMenuPrefab>, bool>) (x => x.Value.Index == unitIdx));
    if (Object.op_Inequality((Object) keyValuePair.Key, (Object) null))
      menu.objectList.Remove(keyValuePair.Key);
    else if (menu.objectList.Any<GameObject>())
    {
      if (isDownloadResourceCheck)
        yield return (object) menu.downloadOwnResource(unitIdx);
      GameObject go = menu.objectList.First<GameObject>();
      DetailMenuPrefab script = menu.detailMenuPrefabDict[go];
      menu.objectList.RemoveAt(0);
      Vector3 gridPos = ((Component) menu.scrollView.grid).transform.localPosition;
      go.transform.localPosition = new Vector3(menu.scrollView.grid.cellWidth * (float) unitIdx, 0.0f, 0.0f);
      yield return (object) null;
      DateTime dateTime = ServerTime.NowAppTime();
      SMManager.Get<QuestScoreBonusTimetable[]>();
      QuestScoreBonusTimetable[] activeQuestScoreBonus = menu.getActiveQuestScoreBonus(dateTime);
      UnitBonus[] activeUnitBonus = UnitBonus.getActiveUnitBonus(dateTime);
      IEnumerator e = script.Init(menu, unitIdx, menu.unitList[unitIdx], menu.InfoIndex, menu.IsLimitMode, activeQuestScoreBonus, activeUnitBonus, isUpdate, menu.IsMaterial, menu.baseUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (menu.IsFriend)
      {
        if (menu.equippedGear == (PlayerItem) null)
        {
          e = script.normal.setDefaultWeapon(0, menu.unitList[unitIdx].initial_gear);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else if (menu.IsGvgMode && menu.unitList[unitIdx].primary_equipped_gear == (PlayerItem) null)
      {
        e = script.normal.setDefaultWeapon(0, menu.unitList[unitIdx].initial_gear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      go.transform.localPosition = new Vector3(menu.scrollView.grid.cellWidth * (float) unitIdx, 0.0f, 0.0f);
      ((Component) menu.scrollView.grid).transform.localPosition = gridPos;
    }
  }

  private IEnumerator downloadOwnResource(int targetIndex)
  {
    Unit0042Menu unit0042Menu = this;
    if (!unit0042Menu.IsLimitMode)
    {
      List<PlayerUnit> source = new List<PlayerUnit>();
      for (int index = targetIndex - 1; index <= targetIndex + 1 && index < unit0042Menu.unitList.Length; ++index)
      {
        if (index >= 0)
        {
          PlayerUnit unit = unit0042Menu.unitList[index];
          if (!(unit == (PlayerUnit) null) && !unit0042Menu.cacheDLPlayerUnitIDs.Contains(unit.id))
          {
            source.Add(unit);
            unit0042Menu.cacheDLPlayerUnitIDs.Add(unit.id);
          }
        }
      }
      // ISSUE: reference to a compiler-generated method
      UnitUnit[] array = source.Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.job_abilities != null && x.job_abilities.Length != 0)).SelectMany<PlayerUnit, UnitUnit>(new Func<PlayerUnit, IEnumerable<UnitUnit>>(unit0042Menu.\u003CdownloadOwnResource\u003Eb__239_1)).Distinct<UnitUnit>().ToArray<UnitUnit>();
      if (array.Length != 0)
        yield return (object) OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) array, false);
    }
  }

  public bool isWaitStatusPanelInitializing => this.countDoStartStatusPanel_ > 0;

  private IEnumerator doStartStatusPanel(GameObject go, bool bImmediate)
  {
    Unit0042Menu unit0042Menu = this;
    if (unit0042Menu.dicStatusMode[go] == Unit0042Menu.StatusMode.FadeOut)
    {
      if (!bImmediate)
        unit0042Menu.IsPush = true;
      ++unit0042Menu.countDoStartStatusPanel_;
      DetailMenu detailMenu = unit0042Menu.detailMenuPrefabDict[go].normal;
      IEnumerator bottomInitial = detailMenu.getBottomInitial();
      if (bottomInitial != null)
      {
        unit0042Menu.dicStatusMode[go] = Unit0042Menu.StatusMode.Initialize;
        yield return (object) bottomInitial;
      }
      detailMenu.InformationScrollView.resetCenterItem(unit0042Menu.InfoIndex);
      if (unit0042Menu.dicStatusMode[go] == Unit0042Menu.StatusMode.CancelFadeIn)
      {
        unit0042Menu.dicBottomTweenParts[go].resetActive(false);
        unit0042Menu.dicStatusMode[go] = Unit0042Menu.StatusMode.FadeOut;
      }
      else
      {
        NGTweenParts dicBottomTweenPart = unit0042Menu.dicBottomTweenParts[go];
        if (bImmediate)
        {
          ((Component) dicBottomTweenPart).GetComponent<UIRect>().alpha = 1f;
          dicBottomTweenPart.resetActive(true);
        }
        else
        {
          ((Component) dicBottomTweenPart).GetComponent<UIRect>().alpha = 0.0f;
          dicBottomTweenPart.forceActive(true);
        }
        unit0042Menu.dicStatusMode[go] = Unit0042Menu.StatusMode.FadeIn;
      }
      detailMenu.ResetArmorSkillIcon();
      --unit0042Menu.countDoStartStatusPanel_;
      if (!bImmediate)
        unit0042Menu.IsPush = false;
    }
  }

  private void startStatusFadeOut(GameObject goExclude, bool bImmediate)
  {
    foreach (GameObject key in this.dicBottomTweenParts.Keys.ToList<GameObject>())
    {
      switch (this.dicStatusMode[key])
      {
        case Unit0042Menu.StatusMode.Initialize:
          this.dicStatusMode[key] = Unit0042Menu.StatusMode.CancelFadeIn;
          continue;
        case Unit0042Menu.StatusMode.FadeIn:
          this.dicStatusMode[key] = Unit0042Menu.StatusMode.FadeOut;
          if (bImmediate)
          {
            this.dicBottomTweenParts[key].resetActive(false);
            ((Component) this.dicBottomTweenParts[key]).GetComponent<UIRect>().alpha = 0.0f;
            continue;
          }
          this.dicBottomTweenParts[key].isActive = false;
          continue;
        default:
          continue;
      }
    }
  }

  private QuestScoreBonusTimetable[] getActiveQuestScoreBonus(DateTime nowTime)
  {
    QuestScoreBonusTimetable[] source = SMManager.Get<QuestScoreBonusTimetable[]>();
    return source == null ? new QuestScoreBonusTimetable[0] : ((IEnumerable<QuestScoreBonusTimetable>) source).Where<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => x.start_at < nowTime && x.end_at > nowTime)).ToArray<QuestScoreBonusTimetable>();
  }

  public bool CheckLength(PlayerUnit[] playerUnits)
  {
    return this.unitList == null || this.unitList.Length == playerUnits.Length;
  }

  public IEnumerator UpdateAllPage(PlayerUnit[] playerUnits, bool bCrossFade = false)
  {
    DateTime wait = DateTime.Now;
    if (Object.op_Inequality((Object) this.bottomBase, (Object) null))
    {
      this.startStatusFadeOut((GameObject) null, false);
      wait += TimeSpan.FromSeconds(0.5);
    }
    this.unitList = playerUnits;
    this.setEnabledArrowAnimation(true);
    this.CreateFirstFavoriteSetting();
    int start = Mathf.Max(this.CurrentIndex - 1, 0);
    int num = Mathf.Min(this.CurrentIndex + 1, this.unitList.Length - 1);
    IEnumerator e = this.updatePages(Enumerable.Range(start, num - start + 1).ToList<int>(), bCrossFade);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) this.bottomBase, (Object) null))
    {
      while (wait > DateTime.Now)
        yield return (object) null;
      GameObject currentScrollObject = this.CurrentScrollObject;
      if (Object.op_Inequality((Object) currentScrollObject, (Object) null))
        yield return (object) this.doStartStatusPanel(currentScrollObject, false);
    }
  }

  private IEnumerator updatePages(List<int> unitIdxs, bool bCrossFade)
  {
    Unit0042Menu menu = this;
    DateTime serverTime = ServerTime.NowAppTime();
    QuestScoreBonusTimetable[] source = SMManager.Get<QuestScoreBonusTimetable[]>();
    QuestScoreBonusTimetable[] tables = source != null ? ((IEnumerable<QuestScoreBonusTimetable>) source).Where<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => x.start_at < serverTime && x.end_at > serverTime)).ToArray<QuestScoreBonusTimetable>() : new QuestScoreBonusTimetable[0];
    UnitBonus[] unitBonus = UnitBonus.getActiveUnitBonus(serverTime);
    foreach (Transform child in ((Component) menu.scrollView.grid).transform.GetChildren())
    {
      DetailMenuPrefab script = menu.detailMenuPrefabDict[((Component) child).gameObject];
      if (unitIdxs.Contains(script.Index))
      {
        if (bCrossFade && menu.CurrentIndex == script.Index)
          script.normal.isCrossFade = true;
        IEnumerator e = script.Init(menu, script.Index, menu.unitList[script.Index], menu.InfoIndex, menu.IsLimitMode, tables, unitBonus, isMaterial: menu.IsMaterial, baseUnit: menu.baseUnit);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ((Component) script).transform.localPosition = new Vector3(menu.scrollView.grid.cellWidth * (float) script.Index, 0.0f, 0.0f);
      }
      script = (DetailMenuPrefab) null;
    }
  }

  public IEnumerator UpdateAllPageForEarth(PlayerUnit[] playerUnits, bool limitMode)
  {
    this.IsLimitMode = limitMode;
    this.IsMaterial = this.IsFriend;
    this.IsMemory = false;
    this.unitList = playerUnits;
    this.setEnabledArrowAnimation(true);
    this.CreateFirstFavoriteSetting();
    int start = Mathf.Max(this.CurrentIndex - 1, 0);
    int num = Mathf.Min(this.CurrentIndex + 1, this.unitList.Length - 1);
    yield return (object) this.updatePageForEarth(Enumerable.Range(start, num - start + 1).ToList<int>());
  }

  private IEnumerator updatePageForEarth(List<int> unitIdxs)
  {
    Unit0042Menu menu = this;
    DateTime serverTime = ServerTime.NowAppTime();
    QuestScoreBonusTimetable[] source = SMManager.Get<QuestScoreBonusTimetable[]>();
    QuestScoreBonusTimetable[] tables = source != null ? ((IEnumerable<QuestScoreBonusTimetable>) source).Where<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => x.start_at < serverTime && x.end_at > serverTime)).ToArray<QuestScoreBonusTimetable>() : new QuestScoreBonusTimetable[0];
    UnitBonus[] unitBonus = UnitBonus.getActiveUnitBonus(serverTime);
    foreach (Transform transform in ((Component) menu.scrollView.grid).transform.GetChildren().ToList<Transform>())
    {
      DetailMenuPrefab script = menu.detailMenuPrefabDict[((Component) transform).gameObject];
      if (unitIdxs.Contains(script.Index))
      {
        yield return (object) script.Init(menu, script.Index, menu.unitList[script.Index], menu.InfoIndex, menu.IsLimitMode, tables, unitBonus, isMaterial: menu.IsMaterial, baseUnit: menu.baseUnit);
        ((Component) script).transform.localPosition = new Vector3(menu.scrollView.grid.cellWidth * (float) script.Index, 0.0f, 0.0f);
      }
      script = (DetailMenuPrefab) null;
    }
  }

  public IEnumerator onEndSceneAsync()
  {
    yield return (object) this.doUploadFavorites();
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.stopVoice();
  }

  private void DisplayGroupLogo(UnitUnit playerUnit)
  {
    if (this.isEarthMode || this.groupSprites == null || this.groupSprites.Length == 0)
      return;
    int index = 0;
    foreach (Component groupSprite in this.groupSprites)
      groupSprite.gameObject.SetActive(false);
    UnitGroup groupInfo = Array.Find<UnitGroup>(MasterData.UnitGroupList, (Predicate<UnitGroup>) (x => x.unit_id == playerUnit.ID));
    if (groupInfo != null)
    {
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
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
      if (groupSmallCategory != null && groupSmallCategory.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(groupSmallCategory.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
      if (clothingCategory1 != null && clothingCategory1.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(clothingCategory1.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
      if (clothingCategory2 != null && clothingCategory2.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(clothingCategory2.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
      if (generationCategory != null && generationCategory.ID != 1)
      {
        this.groupSprites[index].SetSpriteName<string>(generationCategory.GetSpriteName(), true);
        ((Component) this.groupSprites[index]).gameObject.SetActive(true);
        ++index;
      }
    }
    if (!Object.op_Inequality((Object) this.slc_GroupBase, (Object) null))
      return;
    this.dispGroupCount = index;
    this.tween_GroupBaseOpen.to = (int) this.getGroupHeightTarget();
    this.tween_GroupBaseClose.from = (int) this.getGroupHeightTarget();
    this.tween_GroupSpriteDirOpen.to.y = this.getGroupPositionTarget();
    this.tween_GroupSpriteDirOpen.from.y = this.getGroupPositionInit();
    this.tween_GroupSpriteDirClose.to.y = this.getGroupPositionInit();
    this.tween_GroupSpriteDirClose.from.y = this.getGroupPositionTarget();
    this.setGroupPos();
    if (this.dispGroupCount != 0)
      return;
    ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(false);
    this.dir_GroupPressed.SetActive(false);
  }

  public void IbtnFavoriteToggle() => this.SetFavorite(!this.isFavorited);

  public void SetFavorite(bool active)
  {
    this.isFavorited = active;
    this.btnFavoritedOff.SetActive(!active);
    this.btnFavoritedOn.SetActive(active);
    this.UpdateSetting(this.unitList[this.CurrentIndex].id, this.isFavorited);
  }

  public void IbtnRecommend()
  {
    if (!this.isEnabledRecommend || this.recommendPrefabs == null || Object.op_Equality((Object) this.recommendPrefabs[0], (Object) null))
      return;
    if (this.controlFlags.IsOff(Control.CustomDeck))
      Unit0042Menu.actionPopupRecommend(this.unitList[this.CurrentIndex].id, false)(this.recommendPrefabs, (NGMenuBase) this);
    else
      PopupRecommendMenu.open(this.recommendPrefabs[0], this.unitList[this.CurrentIndex], bDisabledAccountStatus: true);
  }

  private static Action<GameObject[], NGMenuBase> actionPopupRecommend(
    int playerUnitId,
    bool bReturnScene)
  {
    return !bReturnScene ? (Action<GameObject[], NGMenuBase>) ((prefabs, menu) =>
    {
      PlayerUnit playerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == playerUnitId));
      if (!(playerUnit != (PlayerUnit) null) || prefabs.Length == 0)
        return;
      PopupRecommendMenu.open(prefabs[0], playerUnit, Unit0042Menu.createActionOnChangeScene(playerUnit), Unit0042Menu.createActionOnChangeQuest(menu, playerUnit), bDisabledChangeQuest: DetailMenuScrollViewParam.checkDisableQuestSelect());
    }) : (Action<GameObject[], NGMenuBase>) ((prefabs, menu) =>
    {
      PlayerUnit playerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == playerUnitId));
      if (!(playerUnit != (PlayerUnit) null) || prefabs.Length == 0)
        return;
      PopupRecommendMenu.openReturnScene(prefabs[0], playerUnit, Unit0042Menu.createActionOnChangeScene(playerUnit), Unit0042Menu.createActionOnChangeQuest(menu, playerUnit), false);
    });
  }

  private static Action<GameObject[], NGMenuBase> actionPopupReturnRecommendQuest(int playerUnitId)
  {
    return (Action<GameObject[], NGMenuBase>) ((prefabs, menu) =>
    {
      PlayerUnit playerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == playerUnitId));
      if (!(playerUnit != (PlayerUnit) null) || prefabs.Length == 0)
        return;
      PopupRecommendMenu.openReturnScene(prefabs[0], playerUnit, Unit0042Menu.createActionOnChangeScene(playerUnit), Unit0042Menu.createActionOnChangeQuest(menu, playerUnit), true);
    });
  }

  private static Action createActionOnChangeScene(PlayerUnit unit)
  {
    return (Action) (() =>
    {
      Singleton<NGSceneManager>.GetInstance().SaveCurrentChangeSceneParam();
      DetailMenuScrollViewParam.ModifySelectedPlayerUnitInChangeSceneParam(unit);
      NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
      instance.OpenPopup = Unit0042Menu.actionPopupRecommend(unit.id, true);
      instance.fromPopup = NGGameDataManager.FromPopup.Unit0042SceneRecommend;
    });
  }

  private static Action<Action> createActionOnChangeQuest(NGMenuBase menu, PlayerUnit unit)
  {
    return (Action<Action>) (endWait =>
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      Unit0042Menu unit0042Menu = (Unit0042Menu) menu;
      if (Object.op_Inequality((Object) unit0042Menu, (Object) null))
      {
        instance.StartCoroutine(unit0042Menu.doUploadFavorites(endWait));
      }
      else
      {
        if (endWait == null)
          return;
        endWait();
      }
    });
  }

  public void IbtnGroupOpen()
  {
    if (this.isGroupTween || this.isGroupOpen)
      return;
    ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(false);
    this.dir_GroupPressed.SetActive(true);
    NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), 100);
    this.isGroupOpen = true;
    this.isGroupTween = true;
  }

  public void onFinishedGroupOpen() => this.isGroupTween = false;

  public void IbtnGroupClose()
  {
    if (this.isGroupTween || !this.isGroupOpen)
      return;
    NGTween.playTweens(((Component) this).GetComponentsInChildren<UITweener>(true), 101);
    this.isGroupOpen = false;
    this.isGroupTween = true;
  }

  public void onFinishedGroupClose()
  {
    if (!this.isGroupTween)
      return;
    if (this.dispGroupCount > 0)
      ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(true);
    else
      ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(false);
    this.dir_GroupPressed.SetActive(false);
    this.isGroupTween = false;
  }

  private void setGroupPos()
  {
    if (this.isGroupOpen)
    {
      ((UIWidget) this.slc_GroupBase.GetComponent<UISprite>()).height = (int) this.getGroupHeightTarget();
      this.dir_GroupSprite.transform.localPosition = new Vector3(this.dir_GroupSprite.transform.localPosition.x, this.getGroupPositionTarget(), this.dir_GroupSprite.transform.localPosition.z);
      ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(false);
      this.dir_GroupPressed.SetActive(true);
    }
    else
    {
      ((UIWidget) this.slc_GroupBase.GetComponent<UISprite>()).height = this.groupBaseHeightInit;
      this.dir_GroupSprite.transform.localPosition = new Vector3(this.dir_GroupSprite.transform.localPosition.x, this.getGroupPositionInit(), this.dir_GroupSprite.transform.localPosition.z);
      ((Component) this.IbtnGroupTabIdle).gameObject.SetActive(true);
      this.dir_GroupPressed.SetActive(false);
    }
    this.isGroupTween = false;
  }

  private float getGroupHeightTarget()
  {
    return (float) ((double) this.groupBaseHeightInit + (double) this.groupSpriteHeight * (double) this.dispGroupCount + 8.0);
  }

  private float getGroupPositionTarget() => 0.0f;

  private float getGroupPositionInit()
  {
    return (float) (-(double) this.groupSpriteHeight * (double) this.dispGroupCount - 8.0);
  }

  public void IbtnCallSkill()
  {
    if (this.CurrentUnit == (PlayerUnit) null || this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenCallSkillPopup());
  }

  private IEnumerator OpenCallSkillPopup()
  {
    Unit0042Menu unit0042Menu = this;
    CallCharacter[] callCharacterList = MasterData.CallCharacterList;
    int key = 0;
    foreach (CallCharacter callCharacter in callCharacterList)
    {
      if (callCharacter.same_character_id == unit0042Menu.CurrentUnit.unit.same_character_id)
      {
        key = callCharacter.call_skill_id;
        break;
      }
    }
    if (key != 0)
    {
      BattleskillSkill skill = MasterData.BattleskillSkill[key];
      if (Object.op_Equality((Object) unit0042Menu.popupCallSkillDetailsPrefab, (Object) null))
      {
        Future<GameObject> prefabF = new ResourceObject(Singleton<NGGameDataManager>.GetInstance().IsSea ? "Prefabs/UnitGUIs/Popup_CallSkillDetails_Sea" : "Prefabs/UnitGUIs/Popup_CallSkillDetails").Load<GameObject>();
        IEnumerator e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        unit0042Menu.popupCallSkillDetailsPrefab = prefabF.Result;
        prefabF = (Future<GameObject>) null;
      }
      yield return (object) Singleton<PopupManager>.GetInstance().open(unit0042Menu.popupCallSkillDetailsPrefab).GetComponent<PopupCallSkill>().initialize(unit0042Menu.CurrentUnit.unit, skill, false);
      unit0042Menu.StartCoroutine(unit0042Menu.IsPushOff());
    }
  }

  private bool CenterOnChild(int num, bool disabledAnime = false)
  {
    if (num < 0)
      return false;
    foreach (KeyValuePair<GameObject, DetailMenuPrefab> keyValuePair in this.detailMenuPrefabDict)
    {
      if (keyValuePair.Value.Index == num)
      {
        if (disabledAnime)
          this.resetScrollViewPosition(keyValuePair.Key.transform.localPosition);
        // ISSUE: method pointer
        this.centerOnChild.onFinished = new SpringPanel.OnFinished((object) this, __methodptr(OnCocFinished));
        this.centerOnChild.CenterOn(keyValuePair.Key.transform);
        UIScrollView componentInChildren = this.centerOnChild.centeredObject.GetComponentInChildren<UIScrollView>();
        if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          ((Behaviour) componentInChildren).enabled = false;
        return true;
      }
    }
    return false;
  }

  private void resetScrollViewPosition(Vector3 target)
  {
    this.scrollView.scrollView.ResetHorizontalCenter(target.x);
  }

  public void onEndScene()
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.setEnabledArrowAnimation(false);
    this.scrollView.scrollView.Press(false);
  }

  private void setEnabledArrowAnimation(bool bEnabled)
  {
    TweenAlpha component1 = this.LeftArrow.GetComponent<TweenAlpha>();
    if (Object.op_Inequality((Object) component1, (Object) null))
    {
      ((Behaviour) component1).enabled = bEnabled;
      if (!bEnabled)
        component1.value = 0.0f;
    }
    TweenAlpha component2 = this.RightArrow.GetComponent<TweenAlpha>();
    if (!Object.op_Inequality((Object) component2, (Object) null))
      return;
    ((Behaviour) component2).enabled = bEnabled;
    if (bEnabled)
      return;
    component2.value = 0.0f;
  }

  public static void ResetCharacterQuests() => Unit0042Menu.isRequestedResetCharacterQuests = true;

  public void hideQuestMenu()
  {
    if (!Object.op_Implicit((Object) this.questMenu_) || !((Component) this.questMenu_).gameObject.activeSelf)
      return;
    this.questMenu_.Hide();
  }

  public void showCharacterQuests(PlayerUnit playerUnit)
  {
    Unit0042Menu.createMethodPopupCharacterQuests(playerUnit.id)((GameObject[]) null, (NGMenuBase) this);
  }

  private static Action<GameObject[], NGMenuBase> createMethodPopupCharacterQuests(int playerUnitId)
  {
    return (Action<GameObject[], NGMenuBase>) ((prefabs, mb) =>
    {
      Unit0042Menu unit0042Menu = (Unit0042Menu) mb;
      if (Object.op_Implicit((Object) unit0042Menu.questMenu_) && ((Component) unit0042Menu.questMenu_).gameObject.activeSelf)
        return;
      PlayerUnit playerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == playerUnitId));
      Singleton<PopupManager>.GetInstance().monitorCoroutine(unit0042Menu.doShowCharacterQuests(playerUnit));
    });
  }

  private IEnumerator doShowCharacterQuests(PlayerUnit playerUnit)
  {
    Unit0042Menu unit0042Menu = this;
    bool bLoading = false;
    IEnumerator e;
    if (!Object.op_Implicit((Object) unit0042Menu.questMenu_))
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
      bLoading = true;
      Future<GameObject> ld = new ResourceObject("Prefabs/unit004_2/CharacterQuestPanel").Load<GameObject>();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject go = ld.Result.Clone(((Component) unit0042Menu).transform.Find("MainPanel"));
      UIRect rTop = go.GetComponent<UIRect>();
      rTop.alpha = 0.0f;
      unit0042Menu.questMenu_ = go.GetComponent<Quest00214aMenu>();
      int cntFrame = 0;
      e = unit0042Menu.questMenu_.doLoadResources();
      while (e.MoveNext())
      {
        ++cntFrame;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      while (cntFrame < 2)
      {
        ++cntFrame;
        yield return (object) null;
      }
      foreach (UIRect uiRect in ((IEnumerable<UIRect>) go.GetComponentsInChildren<UIRect>(true)).Where<UIRect>((Func<UIRect, bool>) (x => x.isAnchored)))
      {
        uiRect.ResetAnchors();
        uiRect.UpdateAnchors();
      }
      rTop.alpha = 1f;
      ((Component) unit0042Menu).GetComponent<Unit0042Scene>().entryMenu((NGMenuBase) unit0042Menu.questMenu_);
      EventDelegate.Set(unit0042Menu.questMenu_.onClose, new EventDelegate.Callback(unit0042Menu.onCloseCharacterQuest));
      EventDelegate.Set(unit0042Menu.questMenu_.onCloseFinished, new EventDelegate.Callback(unit0042Menu.onCloseFinishedCharacterQuest));
      ld = (Future<GameObject>) null;
      go = (GameObject) null;
      rTop = (UIRect) null;
    }
    int sameid = playerUnit.unit.same_character_id;
    QuestCharacterS quest = Array.Find<QuestCharacterS>(MasterData.QuestCharacterSList, (Predicate<QuestCharacterS>) (x => QuestCharacterS.CheckIsReleased(x.start_at) && x.unit.same_character_id == sameid));
    if (unit0042Menu.questDatas_ == null || Unit0042Menu.isRequestedResetCharacterQuests)
    {
      Unit0042Menu.isRequestedResetCharacterQuests = false;
      if (!bLoading)
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
      bLoading = true;
      Future<WebAPI.Response.QuestProgressCharacter> webApi = WebAPI.QuestProgressCharacter((Action<WebAPI.Response.UserError>) (err =>
      {
        WebAPI.DefaultUserErrorCallback(err);
        MypageScene.ChangeSceneOnError();
      }));
      e = webApi.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if ((unit0042Menu.questDatas_ = webApi.Result) == null)
        yield break;
      else
        webApi = (Future<WebAPI.Response.QuestProgressCharacter>) null;
    }
    if (bLoading)
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    e = unit0042Menu.questMenu_.showPopup(quest.unit_UnitUnit, unit0042Menu.questDatas_, (Action<Action>) (endWait =>
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      NGSceneManager instance1 = Singleton<NGSceneManager>.GetInstance();
      NGGameDataManager instance2 = Singleton<NGGameDataManager>.GetInstance();
      instance1.SaveCurrentChangeSceneParam();
      instance2.setSceneChangeLog(instance1.exportSceneChangeLog());
      instance1.StartCoroutine(this.doUploadFavorites(endWait));
      DetailMenuScrollViewParam.ModifySelectedPlayerUnitInChangeSceneParam(playerUnit);
      instance2.OpenPopup = Unit0042Menu.createMethodPopupCharacterQuests(playerUnit.id);
      instance2.fromPopup = NGGameDataManager.FromPopup.Unit0042SceneCharacterQuest;
      instance2.IsFromPopupStageList = true;
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void onCloseCharacterQuest()
  {
  }

  private void onCloseFinishedCharacterQuest()
  {
    this.IsPush = false;
    this.questMenu_.IsPush = false;
  }

  public void changeJob(int jobId)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.doChangeJob(jobId));
  }

  private IEnumerator doChangeJob(int jobId)
  {
    Unit0042Menu unit0042Menu = this;
    PopupManager popupCntl = Singleton<PopupManager>.GetInstance();
    popupCntl.open((GameObject) null, isViewBack: false);
    PlayerUnit playerUnit = unit0042Menu.CurrentUnit;
    PlayerUnit afterUnit = JobChangeUtil.createPlayerUnit(playerUnit, jobId);
    Consts instance = Consts.GetInstance();
    Unit0042Menu.ConfirmPopup[] confirmPopupArray1 = new Unit0042Menu.ConfirmPopup[3];
    Unit0042Menu.ConfirmPopup confirmPopup1 = new Unit0042Menu.ConfirmPopup();
    confirmPopup1.checkOpen = (Func<bool>) (() => MasterData.UnitJob[jobId].new_cost > 0);
    confirmPopup1.title = instance.JOB_CHANGE_TITLE;
    confirmPopup1.desc = instance.JOB_CHANGE_DESC_COST_CHANGE;
    confirmPopupArray1[0] = confirmPopup1;
    confirmPopup1 = new Unit0042Menu.ConfirmPopup();
    confirmPopup1.checkOpen = (Func<bool>) (() =>
    {
      bool flag = false;
      PlayerTransmigrateMemoryPlayerUnitIds current = PlayerTransmigrateMemoryPlayerUnitIds.Current;
      if (current?.transmigrate_memory_player_unit_ids != null && current.transmigrate_memory_player_unit_ids.Length != 0)
      {
        int cur = playerUnit.id;
        flag = ((IEnumerable<int?>) current.transmigrate_memory_player_unit_ids).Any<int?>((Func<int?, bool>) (n => n.HasValue && n.Value == cur));
      }
      return flag;
    });
    confirmPopup1.title = instance.JOB_CHANGE_MEMORY_TITLE;
    confirmPopup1.desc = instance.JOB_CHANGE_MEMORY_DESC;
    confirmPopupArray1[1] = confirmPopup1;
    confirmPopup1 = new Unit0042Menu.ConfirmPopup();
    confirmPopup1.checkOpen = (Func<bool>) (() =>
    {
      if (playerUnit.equippedGear != (PlayerItem) null && afterUnit.equippedGear == (PlayerItem) null || playerUnit.equippedGear2 != (PlayerItem) null && afterUnit.equippedGear2 == (PlayerItem) null)
        return true;
      return playerUnit.equippedGear3 != (PlayerItem) null && afterUnit.equippedGear3 == (PlayerItem) null;
    });
    confirmPopup1.title = instance.JOB_CHANGE_GEAR_TITLE;
    confirmPopup1.desc = instance.JOB_CHANGE_GEAR_DESC;
    confirmPopupArray1[2] = confirmPopup1;
    Unit0042Menu.ConfirmPopup[] confirmPopupArray = confirmPopupArray1;
    for (int index = 0; index < confirmPopupArray.Length; ++index)
    {
      Unit0042Menu.ConfirmPopup confirmPopup2 = confirmPopupArray[index];
      if (confirmPopup2.checkOpen())
      {
        bool bWait = true;
        bool bCancel = false;
        PopupCommonNoYes.Show(confirmPopup2.title, confirmPopup2.desc, (Action) (() => bWait = false), (Action) (() =>
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
          unit0042Menu.IsPush = false;
          popupCntl.closeAll();
          yield break;
        }
      }
    }
    confirmPopupArray = (Unit0042Menu.ConfirmPopup[]) null;
    Singleton<NGSoundManager>.GetInstance().StopVoice();
    SimpleEffectJobChange effect = (SimpleEffectJobChange) null;
    unit0042Menu.StartCoroutine(unit0042Menu.doInitializeEffectJobChange((Action<SimpleEffectJobChange>) (s => effect = s)));
    CommonRoot cRoot = Singleton<CommonRoot>.GetInstance();
    cRoot.ShowLoadingLayer(1, true);
    bool bWait1 = true;
    unit0042Menu.StartCoroutine(unit0042Menu.doUploadFavorites((Action) (() => bWait1 = false)));
    while (bWait1)
      yield return (object) null;
    Future<WebAPI.Response.UnitJobchange> future = WebAPI.UnitJobchange(playerUnit.id, new int[0], jobId, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    while (!Object.op_Implicit((Object) effect))
      yield return (object) null;
    if (future.Result != null)
    {
      afterUnit = Array.Find<PlayerUnit>(future.Result.player_units, (Predicate<PlayerUnit>) (x => x.id == playerUnit.id));
      cRoot.HideLoadingLayer();
      if (Object.op_Implicit((Object) unit0042Menu.topJobChangeButton_))
        unit0042Menu.topJobChangeButton_.SetActive(false);
      effect.play(afterUnit);
      unit0042Menu.SetTitleBarInfo(false, afterUnit);
      PlayerUnit[] playerUnits = SMManager.Get<PlayerUnit[]>();
      PlayerUnit[] array = ((IEnumerable<PlayerUnit>) unit0042Menu.unitList).Select<PlayerUnit, PlayerUnit>((Func<PlayerUnit, PlayerUnit>) (x => Array.Find<PlayerUnit>(playerUnits, (Predicate<PlayerUnit>) (y => y.id == x.id)))).ToArray<PlayerUnit>();
      e = unit0042Menu.UpdateAllPage(array, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      while (Object.op_Implicit((Object) effect))
        yield return (object) null;
      if (Object.op_Implicit((Object) unit0042Menu.topJobChangeButton_))
      {
        TweenAlpha component = unit0042Menu.topJobChangeButton_.GetComponent<TweenAlpha>();
        component.value = 0.0f;
        ((UITweener) component).tweenFactor = 0.0f;
        ((UITweener) component).PlayForward();
        unit0042Menu.topJobChangeButton_.SetActive(true);
      }
      popupCntl.closeAll();
      unit0042Menu.IsPush = false;
    }
  }

  private IEnumerator doInitializeEffectJobChange(Action<SimpleEffectJobChange> eventReady)
  {
    IEnumerator e;
    if (!Object.op_Implicit((Object) this.prefabEffectJobChange_))
    {
      Future<GameObject> ld = SimpleEffectJobChange.createLoader();
      e = ld.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.prefabEffectJobChange_ = ld.Result;
      ld = (Future<GameObject>) null;
    }
    SimpleEffectJobChange ret = this.prefabEffectJobChange_.Clone(this.lnkEffectJobChange_).GetComponent<SimpleEffectJobChange>();
    e = ret.doInitialize(this.effectJobChangeAnchorParents_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    eventReady(ret);
  }

  private enum StatusMode
  {
    FadeOut,
    Initialize,
    FadeIn,
    CancelFadeIn,
  }

  private struct ConfirmPopup
  {
    public Func<bool> checkOpen;
    public string title;
    public string desc;
  }
}
