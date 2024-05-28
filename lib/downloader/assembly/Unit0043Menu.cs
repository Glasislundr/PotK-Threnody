// Decompiled with JetBrains decompiler
// Type: Unit0043Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using ModelViewer;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

#nullable disable
public class Unit0043Menu : BackButtonMenuBase
{
  private PlayerUnit targetPlayerUnit;
  private UnitUnit targetUnit;
  private MasterDataTable.UnitJob targetJob;
  private UnitCharacter targetChar;
  private UnitUnit lastCharacterQuestUnit;
  private QuestCharacterS questEpisode2;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtCvName;
  [SerializeField]
  protected UILabel TxtIllustratorname;
  [SerializeField]
  protected UILabel BookNumber;
  [SerializeField]
  protected GameObject DynCharacter;
  [SerializeField]
  protected UI2DSprite dynIllust;
  [SerializeField]
  protected GameObject camera3dModel;
  [SerializeField]
  protected UITexture modelTex3d;
  [SerializeField]
  protected GameObject illustController;
  [SerializeField]
  private IllustrationController controller;
  [SerializeField]
  private UIPanel mainPanel;
  [SerializeField]
  private GameObject dir_3DModel;
  private GameObject modelPrefab;
  private UI3DModel ui3DModel;
  private Camera cam3d;
  private bool initFinished;
  private float oldTextureWidth;
  [SerializeField]
  protected Transform weaponTypeIconParent;
  [SerializeField]
  protected GameObject weaponTypeIconPrefab;
  private GameObject goWeaponType;
  [SerializeField]
  private UI2DSprite rarityStarsTypeIconParent;
  [SerializeField]
  private GameObject slcAwakening;
  [SerializeField]
  private UISprite slcCountry;
  [SerializeField]
  private UI2DSprite slcInclusion;
  [SerializeField]
  private GameObject containerR;
  [SerializeField]
  private GameObject containerL;
  [Header("Unit Pannel")]
  [SerializeField]
  private GameObject unitContainer;
  [SerializeField]
  private UILabel txtHeight;
  [SerializeField]
  private UILabel txtWeight;
  [SerializeField]
  private UILabel txtBust;
  [SerializeField]
  private UILabel txtWaist;
  [SerializeField]
  private UILabel txtHip;
  [SerializeField]
  private UILabel txtBirthday;
  [SerializeField]
  private UILabel txtConstellation;
  [SerializeField]
  private UILabel txtBloodgroup;
  [SerializeField]
  private UILabel txtBirthplace;
  [SerializeField]
  private UILabel txtFavorite;
  [SerializeField]
  private UILabel txtInterest;
  [SerializeField]
  private UILabel txtIntroduction;
  [Header("Enemy Pannel")]
  [SerializeField]
  private GameObject enemyContainer;
  [SerializeField]
  private UILabel txtEnemyIntroduction;
  [SerializeField]
  private UILabel txtEnemySubjugateNum;
  [SerializeField]
  private UILabel txtEnemyJobName;
  [Header("イベント画像")]
  [SerializeField]
  private GameObject dirEventImage;
  [SerializeField]
  private UI2DSprite slcEventImage;
  [SerializeField]
  private UIButton btnShowEventImage;
  [Header("姫CUTIN")]
  public UIButton viewBtn;
  public GameObject unitViewObj;
  public GameObject messageObj;
  public UIButton trustViewBtn;
  public UISprite trustViewBtnSprite;
  public UIButton gachaViewBtn;
  public UIButton awakeViewBtn;
  public UIButton callViewBtn;
  public Transform bg;
  private GameObject trustEffectprefab;
  private GameObject gearEffectprefab;
  private const string trustBtnSprite = "slc_button_text_kyoumei_18pt.png__GUI__004-3_sozai__004-3_sozai_prefab";
  private const string loveBtnSprite = "slc_button_text_favorability_18pt.png__GUI__004-3_sozai__004-3_sozai_prefab";
  [Header("SEA関連")]
  public GameObject specialViewObj;
  public UIButton SEAIllustBtn;
  public UIButton SEAScenarioBtn;
  private string unlockingScenarioConditions;
  private int SEAScriptId;
  private PlayerUnitHistory SEAHistory;
  [Header("姫VOICE")]
  public GameObject voicePanel;
  public UIButton voiceBtn;
  public UIGrid scrollGrid;
  public NGxScroll2 scroll;
  public UIScrollBar scrollBar;
  public UILabel lockedVoiceCount;
  public UILabel voiceCount;
  public int lastPlayCueID = -1;
  private List<Unit0043VoiceItem> voiceItemList = new List<Unit0043VoiceItem>();
  private List<Unit0043Menu.VoiceInfo> voiceInfo = new List<Unit0043Menu.VoiceInfo>();
  private GameObject voiceItem;
  private int lockedCount;
  private bool canScrollIllustration = true;
  private bool isLibrary;
  private bool nohave;
  private const string bgObjName = "bg";
  private Ray ray;
  private RaycastHit[] hits;
  private const int iconScreenValue = 5;
  private const int iconWidth = 578;
  private const int iconHeight = 75;
  private const int iconRowValue = 10;
  private const int iconMaxValue = 10;
  private float scrool_start_y;
  private const int iconColumnValue = 1;
  private bool isPlayCutin;
  [Header("デュエル再生用のパラメータ")]
  [SerializeField]
  private DuelDemoSettings duelDemoSettings;
  [SerializeField]
  private GameObject btnPlayDuelSkill;
  [SerializeField]
  private GameObject btnPlayMultiDuelSkill;
  [Header("UnitCategory毎のUIセット")]
  [SerializeField]
  private Unit0043Menu.UnitCategoryLayout[] uiLayouts;
  [Header("アニメーションステート名テーブル")]
  [SerializeField]
  private Unit0043Menu.PairActionKey[] pairActionKeys;
  [Header("誓約解除後")]
  [SerializeField]
  private GameObject pledgeBaseObj;
  [SerializeField]
  private GameObject pledgeFirstObj;
  [SerializeField]
  private GameObject pledgeHimeObj;
  [SerializeField]
  private UILabel pledgeHimeNum;
  private int current;
  private Unit0043Menu.UnitCategoryLayout currentLayout;
  private bool isLoading;
  private bool isFooterEnable;
  private int? playingAction;
  private Future<RuntimeAnimatorController> winAnimator;
  private static readonly string DefineNameButtonClose = "ibtn_Close";
  private string storeNameBtnShowActions;
  private PlayerUnitSkills duelSkill;
  private PlayerUnitSkills multiDuelSkill;
  private bool isPersonalUnit;
  private bool isEnabledEventImage;
  private string unlockingConditions;
  private PlayerCallDivorceHistory hitDivorce;
  private static readonly string KeyQuestProgress = "QuestProgressCharacter";
  private const int INDEX_EPISODE_2 = 1;
  private const int REWARD_LEVEL = 90;
  private int voiceAction = -1;
  private static readonly string DuelSceneName = "battle018_1";

  public bool isChangedSceneDuelDemo { get; set; }

  public virtual void IbtnBack()
  {
    if (this.isLoading || this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(this.isFooterEnable);
    this.backScene();
    this.finalizedDuelSettings();
  }

  public override void onBackButton()
  {
    if (this.dirEventImage.activeSelf)
      this.setActiveEventImage(false);
    else
      this.IbtnBack();
  }

  public IEnumerator Init(Unit0043Scene.BootParam bp)
  {
    Unit0043Menu unit0043Menu = this;
    unit0043Menu.initFinished = false;
    unit0043Menu.isLoading = true;
    unit0043Menu.targetPlayerUnit = bp.target;
    unit0043Menu.isLibrary = bp.isLibrary;
    unit0043Menu.targetUnit = unit0043Menu.targetPlayerUnit.unit;
    unit0043Menu.targetJob = unit0043Menu.targetPlayerUnit.getJobData();
    unit0043Menu.targetChar = unit0043Menu.targetUnit.character;
    unit0043Menu.isPersonalUnit = unit0043Menu.isEnabledEventImage = unit0043Menu.targetChar.category == UnitCategory.player && (unit0043Menu.targetPlayerUnit.player_id == Player.Current.id || bp.isLibrary);
    Modified<Player> modified = SMManager.Observe<Player>();
    unit0043Menu.pledgeBaseObj.SetActive(false);
    if (unit0043Menu.isPersonalUnit && unit0043Menu.targetChar.category == UnitCategory.player)
    {
      PlayerCallDivorceHistory[] divorceHistories = modified.Value.call_divorce_histories;
      unit0043Menu.hitDivorce = (PlayerCallDivorceHistory) null;
      if (divorceHistories != null)
      {
        foreach (PlayerCallDivorceHistory callDivorceHistory in divorceHistories)
        {
          if (callDivorceHistory.same_character_id == unit0043Menu.targetUnit.same_character_id)
            unit0043Menu.hitDivorce = callDivorceHistory;
        }
        if (unit0043Menu.hitDivorce != null)
        {
          unit0043Menu.pledgeBaseObj.SetActive(true);
          if (unit0043Menu.hitDivorce.divorce_number == 1)
          {
            unit0043Menu.pledgeFirstObj.SetActive(true);
            unit0043Menu.pledgeHimeObj.SetActive(false);
          }
          else
          {
            unit0043Menu.pledgeFirstObj.SetActive(false);
            unit0043Menu.pledgeHimeObj.SetActive(true);
            unit0043Menu.pledgeHimeNum.SetTextLocalize(unit0043Menu.hitDivorce.divorce_number);
          }
        }
      }
    }
    if (unit0043Menu.isEnabledEventImage)
    {
      int? illustReleaseId = unit0043Menu.MasterDataUnitSEASkill?.illust_release_id;
      if (illustReleaseId.HasValue)
      {
        switch (illustReleaseId.GetValueOrDefault())
        {
          case 0:
          case 1:
            unit0043Menu.lastCharacterQuestUnit = unit0043Menu.targetUnit;
            goto label_16;
          case 2:
            break;
          default:
            goto label_16;
        }
      }
      // ISSUE: reference to a compiler-generated method
      QuestCharacterS[] array = ((IEnumerable<QuestCharacterS>) MasterData.QuestCharacterSList).Where<QuestCharacterS>(new Func<QuestCharacterS, bool>(unit0043Menu.\u003CInit\u003Eb__129_11)).OrderBy<QuestCharacterS, int>((Func<QuestCharacterS, int>) (y => y.priority)).ToArray<QuestCharacterS>();
      unit0043Menu.lastCharacterQuestUnit = array.Length != 0 ? array[0].unit : (UnitUnit) null;
      unit0043Menu.questEpisode2 = array.Length > 1 ? array[1] : (QuestCharacterS) null;
      unit0043Menu.isEnabledEventImage = unit0043Menu.questEpisode2 != null;
    }
label_16:
    if (unit0043Menu.isLibrary)
    {
      PlayerUnitHistory[] source = SMManager.Get<PlayerUnitHistory[]>();
      if (source != null)
      {
        // ISSUE: reference to a compiler-generated method
        unit0043Menu.SEAHistory = ((IEnumerable<PlayerUnitHistory>) source).FirstOrDefault<PlayerUnitHistory>(new Func<PlayerUnitHistory, bool>(unit0043Menu.\u003CInit\u003Eb__129_13));
      }
    }
    unit0043Menu.TxtTitle.SetTextLocalize(unit0043Menu.targetUnit.name);
    unit0043Menu.unitViewObj.SetActive(false);
    foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
    {
      if (playerUnit.id == unit0043Menu.targetPlayerUnit.id)
        unit0043Menu.nohave = true;
    }
    if (unit0043Menu.isLibrary || !unit0043Menu.nohave)
    {
      ((Component) unit0043Menu.viewBtn).gameObject.SetActive(false);
      ((Component) unit0043Menu.voiceBtn).gameObject.SetActive(false);
    }
    // ISSUE: reference to a compiler-generated method
    unit0043Menu.btnShowEventImage.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_0)));
    // ISSUE: reference to a compiler-generated method
    unit0043Menu.SEAIllustBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_1)));
    UnitSEASkill SEA = unit0043Menu.MasterDataUnitSEASkill;
    IEnumerator e;
    if (SEA != null)
    {
      e = MasterData.LoadScriptScript(SEA.script_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (MasterData.ScriptScript != null && MasterData.ScriptScript.ContainsKey(SEA.script_id))
        unit0043Menu.SEAScriptId = SEA.script_id;
      else
        ((Component) unit0043Menu.SEAScenarioBtn).gameObject.SetActive(false);
      if (unit0043Menu.isLibrary)
      {
        if (unit0043Menu.SEAHistory != null && unit0043Menu.SEAHistory.isUnlockedSEASkill)
        {
          // ISSUE: reference to a compiler-generated method
          unit0043Menu.SEAScenarioBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_2)));
        }
        else
        {
          ((UIButtonColor) unit0043Menu.SEAScenarioBtn).isEnabled = false;
          unit0043Menu.unlockingScenarioConditions = Consts.GetInstance().SPECIAL_UNLOCKINGCONDITIONS_OVERSLOT;
        }
      }
      else if (!unit0043Menu.targetPlayerUnit.isReleasedOverkillersSlot(2))
      {
        ((UIButtonColor) unit0043Menu.SEAScenarioBtn).isEnabled = false;
        unit0043Menu.unlockingScenarioConditions = Consts.GetInstance().SPECIAL_UNLOCKINGCONDITIONS_OVERSLOT;
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        unit0043Menu.SEAScenarioBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_3)));
      }
    }
    else
      ((Component) unit0043Menu.SEAScenarioBtn).gameObject.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    unit0043Menu.viewBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_4)));
    // ISSUE: reference to a compiler-generated method
    unit0043Menu.trustViewBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_5)));
    // ISSUE: reference to a compiler-generated method
    unit0043Menu.gachaViewBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_6)));
    // ISSUE: reference to a compiler-generated method
    unit0043Menu.callViewBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_7)));
    // ISSUE: reference to a compiler-generated method
    unit0043Menu.awakeViewBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_8)));
    unit0043Menu.SetUnitViewBtnState();
    // ISSUE: reference to a compiler-generated method
    unit0043Menu.voiceBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(unit0043Menu.\u003CInit\u003Eb__129_9)));
    if (!unit0043Menu.isPersonalUnit && unit0043Menu.targetChar.category == UnitCategory.player)
    {
      unit0043Menu.current = unit0043Menu.uiLayouts.Length - 1;
    }
    else
    {
      unit0043Menu.current = 0;
      for (int index = 0; index < unit0043Menu.uiLayouts.Length; ++index)
      {
        if (unit0043Menu.uiLayouts[index].category == unit0043Menu.targetChar.category)
        {
          unit0043Menu.current = index;
          break;
        }
      }
    }
    ((IEnumerable<Unit0043Menu.UnitCategoryLayout>) unit0043Menu.uiLayouts).Select<Unit0043Menu.UnitCategoryLayout, GameObject>((Func<Unit0043Menu.UnitCategoryLayout, GameObject>) (x => x.dirTop)).ToggleOnce(unit0043Menu.current);
    unit0043Menu.currentLayout = unit0043Menu.uiLayouts[unit0043Menu.current];
    if (Object.op_Implicit((Object) unit0043Menu.currentLayout.dirNode))
    {
      Vector3 localPosition;
      if (unit0043Menu.currentLayout.storeNodeX.HasValue)
      {
        unit0043Menu.currentLayout.btnProfile.transform.localPosition = Vector2.op_Implicit(unit0043Menu.currentLayout.storePos.Value);
        localPosition = unit0043Menu.currentLayout.dirNode.transform.localPosition;
        localPosition.x = unit0043Menu.currentLayout.storeNodeX.Value;
        unit0043Menu.currentLayout.dirNode.transform.localPosition = localPosition;
      }
      else
      {
        unit0043Menu.currentLayout.storePos = new Vector2?(Vector2.op_Implicit(unit0043Menu.currentLayout.btnProfile.transform.localPosition));
        unit0043Menu.currentLayout.storeNodeX = new float?(unit0043Menu.currentLayout.dirNode.transform.localPosition.x);
      }
      if (!unit0043Menu.isEnabledEventImage)
      {
        unit0043Menu.currentLayout.btnProfile.transform.localPosition = ((Component) unit0043Menu.btnShowEventImage).transform.localPosition;
        if (!unit0043Menu.isLibrary)
        {
          localPosition = unit0043Menu.currentLayout.dirNode.transform.localPosition;
          localPosition.x = unit0043Menu.currentLayout.nodeXWithoutShowEventImage;
          unit0043Menu.currentLayout.dirNode.transform.localPosition = localPosition;
        }
      }
    }
    string str1;
    switch (unit0043Menu.targetUnit.kind.Enum)
    {
      case GearKindEnum.bow:
      case GearKindEnum.gun:
      case GearKindEnum.staff:
        str1 = "far";
        break;
      default:
        str1 = "near";
        break;
    }
    for (int index1 = 0; index1 < unit0043Menu.currentLayout.actionParameters.Length; ++index1)
    {
      Unit0043Menu.ActionParameter actionParameter = unit0043Menu.currentLayout.actionParameters[index1];
      string str2 = actionParameter.perGearKind ? actionParameter.label + "_" + str1 : actionParameter.label;
      actionParameter.states = (string[]) null;
      for (int index2 = 0; index2 < unit0043Menu.pairActionKeys.Length; ++index2)
      {
        if (unit0043Menu.pairActionKeys[index2].label == str2)
        {
          actionParameter.states = unit0043Menu.pairActionKeys[index2].states;
          break;
        }
      }
    }
    unit0043Menu.isFooterEnable = Singleton<CommonRoot>.GetInstance().GetFooterEnable();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    e = unit0043Menu.LoadPrefabs();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) unit0043Menu.setup3DModel(true);
    yield return (object) unit0043Menu.setEventImage();
    if (unit0043Menu.targetChar.category == UnitCategory.player)
    {
      unit0043Menu.duelSkill = bp.koyuDuelSkill;
      unit0043Menu.multiDuelSkill = bp.koyuMultiSkill;
      unit0043Menu.btnPlayDuelSkill.SetActive(unit0043Menu.duelSkill != null);
      unit0043Menu.btnPlayMultiDuelSkill.SetActive(unit0043Menu.multiDuelSkill != null);
      if (unit0043Menu.btnPlayDuelSkill.activeSelf || unit0043Menu.btnPlayMultiDuelSkill.activeSelf)
        yield return (object) unit0043Menu.duelDemoSettings.preSetupLightmaps();
    }
    if (unit0043Menu.targetUnit.cv_name.CompareTo("") == 0)
      unit0043Menu.containerL.SetActive(false);
    else
      unit0043Menu.TxtCvName.SetTextLocalize(unit0043Menu.targetUnit.cv_name);
    if (unit0043Menu.targetUnit.illustrator_name.CompareTo("") == 0)
      unit0043Menu.containerR.SetActive(false);
    else
      unit0043Menu.TxtIllustratorname.SetTextLocalize(unit0043Menu.targetUnit.illustrator_name);
    string text = "NO." + string.Format("{0:D4}", (object) (unit0043Menu.targetUnit.history_group_number % 10000));
    unit0043Menu.BookNumber.SetTextLocalize(text);
    unit0043Menu.goWeaponType = unit0043Menu.createIcon(unit0043Menu.weaponTypeIconPrefab, unit0043Menu.weaponTypeIconParent);
    unit0043Menu.goWeaponType.GetComponent<GearKindIcon>().Init(unit0043Menu.targetUnit.kind, unit0043Menu.targetPlayerUnit.GetElement());
    RarityIcon.SetRarity(unit0043Menu.targetUnit, unit0043Menu.rarityStarsTypeIconParent, true, bp.isSame);
    e = unit0043Menu.SetIllustration();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0043Menu.SetUnitInformations();
    if (Object.op_Inequality((Object) unit0043Menu.slcCountry, (Object) null))
    {
      ((Component) unit0043Menu.slcCountry).gameObject.SetActive(false);
      if (unit0043Menu.targetUnit.country_attribute.HasValue)
      {
        ((Component) unit0043Menu.slcCountry).gameObject.SetActive(true);
        unit0043Menu.targetUnit.SetCuntrySpriteName(ref unit0043Menu.slcCountry);
      }
    }
    if (Object.op_Inequality((Object) unit0043Menu.slcInclusion, (Object) null))
    {
      ((Component) unit0043Menu.slcInclusion).gameObject.SetActive(false);
      if (unit0043Menu.targetUnit.inclusion_ip.HasValue)
      {
        ((Component) unit0043Menu.slcInclusion).gameObject.SetActive(true);
        e = unit0043Menu.targetUnit.SetInclusionIP(unit0043Menu.slcInclusion);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    unit0043Menu.controller.Init(unit0043Menu.mainPanel.width);
    if (Object.op_Inequality((Object) unit0043Menu.slcAwakening, (Object) null))
      unit0043Menu.slcAwakening.SetActive(false);
    if (unit0043Menu.targetUnit.awake_unit_flag)
      unit0043Menu.slcAwakening.SetActive(true);
    ((UIRect) unit0043Menu.mainPanel).alpha = 1f;
    unit0043Menu.cam3d = unit0043Menu.camera3dModel.GetComponent<Camera>();
    unit0043Menu.SetRenderTexture();
    ((Component) unit0043Menu.modelTex3d).gameObject.SetActive(false);
    if (!((Component) unit0043Menu.SEAScenarioBtn).gameObject.activeSelf)
      ((Component) unit0043Menu.SEAIllustBtn).gameObject.transform.localPosition = new Vector3(70f, 0.0f, 0.0f);
    if (!((Component) unit0043Menu.SEAIllustBtn).gameObject.activeSelf)
      ((Component) unit0043Menu.SEAScenarioBtn).transform.localPosition = new Vector3(70f, 0.0f, 0.0f);
    yield return (object) new WaitForEndOfFrame();
    unit0043Menu.isLoading = false;
    unit0043Menu.initFinished = true;
  }

  public IEnumerator InitOnBackScene()
  {
    if (Object.op_Inequality((Object) this.ui3DModel, (Object) null))
    {
      ((Component) this.ui3DModel).gameObject.SetActive(false);
      Object.Destroy((Object) ((Component) this.ui3DModel).gameObject);
    }
    yield return (object) this.setup3DModel(false);
    ((UIRect) this.mainPanel).alpha = 1f;
  }

  private IEnumerator setup3DModel(bool bResetCamera)
  {
    this.ui3DModel = this.modelPrefab.Clone(this.dir_3DModel.transform).GetComponent<UI3DModel>();
    this.ui3DModel.lightOn = true;
    yield return (object) this.ui3DModel.Unit3D(this.targetPlayerUnit);
    if (Object.op_Inequality((Object) this.ui3DModel.model_creater_.UnitAnimator, (Object) null))
    {
      if (this.currentLayout.actionParameters != null && this.currentLayout.actionButtons != null && this.currentLayout.actionParameters.Length == this.currentLayout.actionButtons.Length)
      {
        for (int actionNo = 0; actionNo < this.currentLayout.actionParameters.Length; ++actionNo)
          this.setActionButton(actionNo);
      }
      if (this.targetChar.category == UnitCategory.player)
      {
        this.winAnimator = this.targetPlayerUnit.LoadWinAnimator();
        yield return (object) this.winAnimator.Wait();
      }
      else
        this.winAnimator = (Future<RuntimeAnimatorController>) null;
    }
    ViewerCameraController component1 = this.camera3dModel.GetComponent<ViewerCameraController>();
    if (Object.op_Inequality((Object) component1, (Object) null))
    {
      component1.Initialize(((Component) this.ui3DModel.ModelTarget).transform, this.targetUnit.vehicle_model_name != null, bResetCamera);
      ModelController component2 = this.camera3dModel.GetComponent<ModelController>();
      if (Object.op_Inequality((Object) component2, (Object) null))
        ((Behaviour) component2).enabled = false;
    }
    else
    {
      ModelController component3 = this.camera3dModel.GetComponent<ModelController>();
      if (Object.op_Inequality((Object) component3, (Object) null))
      {
        component3.target = ((Component) this.ui3DModel.ModelTarget).transform;
        component3.unitWithAnimals = this.targetUnit.vehicle_model_name != null;
      }
    }
  }

  private PlayerUnitSkills createPlayerUnitSkill(int? skillId)
  {
    if (!skillId.HasValue)
      return (PlayerUnitSkills) null;
    return new PlayerUnitSkills()
    {
      skill_id = skillId.Value
    };
  }

  private void setActionButton(int actionNo)
  {
    UIButton actionButton = this.currentLayout.actionButtons[actionNo];
    if (this.currentLayout.actionParameters[actionNo].states != null)
      EventDelegate.Set(actionButton.onClick, (EventDelegate.Callback) (() => this.playAction(actionNo)));
    else
      actionButton.onClick.Clear();
  }

  protected override void Update()
  {
    if (this.initFinished)
    {
      base.Update();
      if ((double) this.oldTextureWidth != (double) ((UIWidget) this.modelTex3d).width)
      {
        this.cam3d.targetTexture.Release();
        this.SetRenderTexture();
      }
      this.ScrollUpdate();
    }
    this.ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);
    this.hits = Physics.RaycastAll(this.ray);
    this.canScrollIllustration = true;
    for (int index = 0; index < this.hits.Length; ++index)
    {
      if (((Object) ((RaycastHit) ref this.hits[index]).transform).name == "bg")
      {
        this.canScrollIllustration = false;
        break;
      }
    }
    this.controller.CanScrollIllustration(this.canScrollIllustration);
  }

  private void SetRenderTexture()
  {
    this.cam3d.targetTexture = new RenderTexture(((UIWidget) this.modelTex3d).width, ((UIWidget) this.modelTex3d).height, 16);
    this.oldTextureWidth = (float) ((UIWidget) this.modelTex3d).width;
    this.cam3d.targetTexture.antiAliasing = 1;
    ((Texture) this.cam3d.targetTexture).wrapMode = (TextureWrapMode) 1;
    ((Texture) this.cam3d.targetTexture).filterMode = (FilterMode) 1;
    this.cam3d.targetTexture.enableRandomWrite = false;
    this.cam3d.farClipPlane = 1000f;
    ((UIWidget) this.modelTex3d).mainTexture = (Texture) this.cam3d.targetTexture;
  }

  private IEnumerator LoadPrefabs()
  {
    Future<GameObject> loader = Res.Prefabs.gacha006_8.slc_3DModel.Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.modelPrefab = loader.Result;
    loader = Res.Prefabs.unit004_3.dir_voice_list.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.voiceItem = loader.Result;
  }

  private void SetUnitInformations()
  {
    UnitCharacter targetChar = this.targetChar;
    int result;
    string text;
    if (targetChar.birthday.Length == 4 && int.TryParse(targetChar.birthday, out result))
      text = Consts.Format(Consts.GetInstance().unit004_2_birthday, (IDictionary) new Hashtable()
      {
        {
          (object) "month",
          (object) (result / 100).ToString()
        },
        {
          (object) "date",
          (object) (result % 100).ToString()
        }
      });
    else
      text = targetChar.birthday;
    if (targetChar.category == UnitCategory.player)
    {
      this.txtBust.SetTextLocalize(targetChar.bust);
      this.txtHip.SetTextLocalize(targetChar.hip);
      this.txtWaist.SetTextLocalize(targetChar.waist);
      this.txtHeight.SetTextLocalize(targetChar.height);
      this.txtWeight.SetTextLocalize(targetChar.weight);
      this.txtBirthday.SetTextLocalize(text);
      this.txtBirthplace.SetTextLocalize(targetChar.origin);
      this.txtBloodgroup.SetTextLocalize(targetChar.blood_type);
      this.txtInterest.SetTextLocalize(targetChar.hobby);
      this.txtFavorite.SetTextLocalize(targetChar.favorite);
      this.txtConstellation.SetTextLocalize(targetChar.zodiac_sign);
      this.txtIntroduction.SetTextLocalize(this.targetUnit.description);
    }
    else
    {
      this.txtEnemyIntroduction.SetTextLocalize(this.targetUnit.description);
      this.txtEnemyJobName.SetTextLocalize(this.targetJob.name);
      int defeat = 0;
      if (SMManager.Get<PlayerEnemyHistory[]>() != null)
        ((IEnumerable<PlayerEnemyHistory>) ((IEnumerable<PlayerEnemyHistory>) SMManager.Get<PlayerEnemyHistory[]>()).ToArray<PlayerEnemyHistory>()).ForEach<PlayerEnemyHistory>((Action<PlayerEnemyHistory>) (obj =>
        {
          if (obj.unit_id != this.targetUnit.ID)
            return;
          defeat += obj.defeat;
        }));
      if (defeat > 99999)
        defeat = 99999;
      this.txtEnemySubjugateNum.SetTextLocalize(defeat);
    }
  }

  private GameObject createIcon(GameObject prefab, Transform trans)
  {
    GameObject icon = prefab.Clone(trans);
    UI2DSprite componentInChildren1 = icon.GetComponentInChildren<UI2DSprite>();
    UI2DSprite componentInChildren2 = ((Component) trans).GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren1).SetDimensions(((UIWidget) componentInChildren2).width, ((UIWidget) componentInChildren2).height);
    ((UIWidget) componentInChildren1).depth = ((UIWidget) componentInChildren1).depth + 1000;
    ((Component) componentInChildren1).transform.localPosition = Vector3.zero;
    return icon;
  }

  public IEnumerator SetCharacterLargeImage()
  {
    Future<GameObject> goFuture = this.targetUnit.LoadMypage();
    IEnumerator e = goFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject go = goFuture.Result.Clone(this.DynCharacter.transform);
    go.transform.localPosition = new Vector3(this.targetUnit.illust_pattern.illust_x, this.targetUnit.illust_pattern.illust_y, 0.0f);
    go.transform.localScale = new Vector3(this.targetUnit.illust_pattern.illust_scale, this.targetUnit.illust_pattern.illust_scale, this.targetUnit.illust_pattern.illust_scale);
    e = this.targetUnit.SetLargeSpriteSnap(go, 0);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.targetUnit.SetLargeSpriteWithMask(go, Res.GUI._004_3_sozai.mask_chara.Load<Texture2D>(), 5, y: -80);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetIllustration()
  {
    Future<Sprite> illustF = this.targetUnit.LoadHiResSprite(this.targetJob.ID);
    if (illustF == null)
    {
      ((Behaviour) this.dynIllust).enabled = false;
    }
    else
    {
      IEnumerator e = illustF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.dynIllust.sprite2D = illustF.Result;
    }
  }

  private IEnumerator setEventImage()
  {
    if (!this.isEnabledEventImage)
    {
      ((Component) this.btnShowEventImage).gameObject.SetActive(false);
    }
    else
    {
      ((Object) ((Component) this.dirEventImage.GetComponentInChildren<UIButton>(true)).gameObject).name = Unit0043Menu.DefineNameButtonClose;
      if (SMManager.Get<PlayerCharacterQuestS[]>() == null && this.questEpisode2 != null && !WebAPI.IsResponsedAtContainsKey(Unit0043Menu.KeyQuestProgress))
      {
        yield return (object) ServerTime.WaitSync();
        Future<WebAPI.Response.QuestProgressCharacter> wapi = WebAPI.QuestProgressCharacter((Action<WebAPI.Response.UserError>) (error =>
        {
          WebAPI.DefaultUserErrorCallback(error);
          MypageScene.ChangeSceneOnError();
        }));
        yield return (object) wapi.Wait();
        if (wapi.Result == null)
        {
          yield break;
        }
        else
        {
          WebAPI.SetLatestResponsedAt(Unit0043Menu.KeyQuestProgress);
          WebAPI.SetLatestResponsedAt("QuestProgressHarmony");
          wapi = (Future<WebAPI.Response.QuestProgressCharacter>) null;
        }
      }
      if (!this.isActiveRewardImage)
      {
        ((UIButtonColor) this.SEAIllustBtn).isEnabled = false;
        UnitSEASkill dataUnitSeaSkill = this.MasterDataUnitSEASkill;
        if (dataUnitSeaSkill != null)
        {
          if (this.isLibrary && dataUnitSeaSkill.illust_release_id == 1)
          {
            this.unlockingConditions = "";
            yield break;
          }
          else
          {
            switch (dataUnitSeaSkill.illust_release_id)
            {
              case 0:
                this.unlockingConditions = Consts.GetInstance().SPECIAL_UNLOCKINGCONDITIONS_OVERSLOT;
                yield break;
              case 1:
                this.unlockingConditions = Consts.GetInstance().SPECIAL_UNLOCKINGCONDITIONS_LEVEL;
                yield break;
              case 2:
                this.unlockingConditions = Consts.GetInstance().SPECIAL_UNLOCKINGCONDITIONS_QUEST;
                yield break;
            }
          }
        }
        else
        {
          this.unlockingConditions = Consts.GetInstance().SPECIAL_UNLOCKINGCONDITIONS_QUEST;
          yield break;
        }
      }
      this.unlockingConditions = (string) null;
      string eventImageName = this.lastCharacterQuestUnit.getEventImageName();
      if (eventImageName == null)
      {
        ((UIButtonColor) this.SEAIllustBtn).isEnabled = false;
      }
      else
      {
        Future<Sprite> ldImage = Singleton<ResourceManager>.GetInstance().Load<Sprite>(eventImageName);
        yield return (object) ldImage.Wait();
        if (Object.op_Inequality((Object) ldImage.Result, (Object) null))
        {
          this.slcEventImage.sprite2D = ldImage.Result;
          ((UIButtonColor) this.SEAIllustBtn).isEnabled = true;
        }
        else
          ((UIButtonColor) this.SEAIllustBtn).isEnabled = false;
      }
    }
  }

  private UnitSEASkill MasterDataUnitSEASkill
  {
    get
    {
      UnitSEASkill dataUnitSeaSkill;
      MasterData.UnitSEASkill.TryGetValue(this.targetUnit.same_character_id, out dataUnitSeaSkill);
      return dataUnitSeaSkill;
    }
  }

  private bool isActiveRewardImage
  {
    get
    {
      UnitSEASkill dataUnitSeaSkill = this.MasterDataUnitSEASkill;
      if (dataUnitSeaSkill != null)
      {
        switch (dataUnitSeaSkill.illust_release_id)
        {
          case 0:
            if (!this.isLibrary)
              return this.targetPlayerUnit.isReleasedOverkillersSlot(2);
            return this.SEAHistory != null && this.SEAHistory.isUnlockedSEASkill;
          case 1:
            return !this.isLibrary && this.targetPlayerUnit.total_level >= 90;
          case 2:
            PlayerCharacterQuestS[] array1 = SMManager.Get<PlayerCharacterQuestS[]>();
            if (array1 == null || this.questEpisode2 == null)
              return false;
            PlayerCharacterQuestS playerCharacterQuestS1 = Array.Find<PlayerCharacterQuestS>(array1, (Predicate<PlayerCharacterQuestS>) (x => x._quest_character_s == this.questEpisode2.ID));
            return playerCharacterQuestS1 != null && playerCharacterQuestS1.is_clear;
          default:
            return false;
        }
      }
      else
      {
        if (this.questEpisode2 == null)
          return false;
        PlayerCharacterQuestS[] array2 = SMManager.Get<PlayerCharacterQuestS[]>();
        if (array2 == null)
          return false;
        PlayerCharacterQuestS playerCharacterQuestS2 = Array.Find<PlayerCharacterQuestS>(array2, (Predicate<PlayerCharacterQuestS>) (x => x._quest_character_s == this.questEpisode2.ID));
        return playerCharacterQuestS2 != null && playerCharacterQuestS2.is_clear;
      }
    }
  }

  public void CloseContainers()
  {
    if (this.IsPush)
      return;
    this.unitContainer.SetActive(false);
    this.enemyContainer.SetActive(false);
    this.currentLayout.btnCloseContainer.SetActive(false);
  }

  public void OpenContainer()
  {
    if (this.isLoading || this.IsPush)
      return;
    this.currentLayout.btnCloseContainer.SetActive(true);
    if (this.targetChar.category == UnitCategory.player)
    {
      this.unitContainer.SetActive(true);
      this.unitViewObj.SetActive(false);
      this.voicePanel.SetActive(false);
    }
    else
      this.enemyContainer.SetActive(true);
  }

  public void Show3dModel()
  {
    if (this.isLoading || this.IsPush)
      return;
    this.currentLayout.btn3dModel.SetActive(false);
    this.currentLayout.btn2dIllust.SetActive(true);
    this.currentLayout.btnActions.SetActive(true);
    this.camera3dModel.SetActive(true);
    ((Component) this.modelTex3d).gameObject.SetActive(true);
    this.illustController.SetActive(false);
    ((Component) this.viewBtn).gameObject.SetActive(false);
    ((Component) this.voiceBtn).gameObject.SetActive(false);
    this.unitViewObj.SetActive(false);
    this.voicePanel.SetActive(false);
  }

  public void Show2dIllust()
  {
    if (this.isLoading || this.IsPush)
      return;
    this.illustController.GetComponent<IllustrationController>().InitSize();
    this.currentLayout.btn3dModel.SetActive(true);
    this.currentLayout.btn2dIllust.SetActive(false);
    this.currentLayout.btnActions.SetActive(false);
    this.camera3dModel.SetActive(false);
    ((Component) this.modelTex3d).gameObject.SetActive(false);
    this.illustController.SetActive(true);
    if (this.isLibrary || !this.nohave)
      return;
    ((Component) this.viewBtn).gameObject.SetActive(true);
    ((Component) this.voiceBtn).gameObject.SetActive(true);
  }

  public void onEndScene()
  {
    Resources.UnloadUnusedAssets();
    NGSoundManager instanceOrNull = Singleton<NGSoundManager>.GetInstanceOrNull();
    if (!Object.op_Inequality((Object) instanceOrNull, (Object) null))
      return;
    instanceOrNull.stopVoice();
  }

  private void playAction(int actionNo)
  {
    if (this.IsPush)
      return;
    if (this.playingAction.HasValue)
    {
      int num = actionNo;
      int? playingAction = this.playingAction;
      int valueOrDefault = playingAction.GetValueOrDefault();
      if (num == valueOrDefault & playingAction.HasValue)
        return;
    }
    this.StopCoroutine("doPlayAction");
    Singleton<NGSoundManager>.GetInstance().StopVoice();
    this.voiceAction = -1;
    if (this.playingAction.HasValue && this.currentLayout.actionParameters[this.playingAction.Value].winAnimation && Object.op_Inequality((Object) this.winAnimator.Result, (Object) null))
      this.ui3DModel.model_creater_.ResetAnimator();
    this.disableShieldModel();
    this.StartCoroutine("doPlayAction", (object) actionNo);
  }

  private IEnumerator doPlayAction(int actionNo)
  {
    Unit0043Menu unit0043Menu = this;
    unit0043Menu.playingAction = new int?(actionNo);
    Unit0043Menu.ActionParameter actionParameter = unit0043Menu.currentLayout.actionParameters[actionNo];
    IEnumerator e;
    if (!actionParameter.winAnimation || Object.op_Equality((Object) unit0043Menu.winAnimator.Result, (Object) null))
    {
      e = unit0043Menu.ui3DModel.TryPlayAction(actionParameter.states, new Action<bool>(unit0043Menu.startEndAction), actionParameter.waitNext);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      unit0043Menu.startEndAction(true);
      e = unit0043Menu.ui3DModel.PlayAction(unit0043Menu.winAnimator.Result, unit0043Menu.targetPlayerUnit.getWinAnimatorControllerName(), actionParameter.states[0], actionParameter.waitNext, new Func<IEnumerator>(unit0043Menu.playVoiceWait));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit0043Menu.startEndAction(false);
    }
    unit0043Menu.playingAction = new int?();
  }

  private void startEndAction(bool started)
  {
    if (started)
    {
      if (!this.playingAction.HasValue)
        return;
      Unit0043Menu.ActionParameter actionParameter = this.currentLayout.actionParameters[this.playingAction.Value];
      if ((double) actionParameter.displayShield > 0.0)
        this.StartCoroutine("doDisplayShield", (object) actionParameter.displayShield);
      NGSoundManager instanceOrNull = Singleton<NGSoundManager>.GetInstanceOrNull();
      CharaVoiceCueEnum.CueID? voice;
      if (Object.op_Equality((Object) instanceOrNull, (Object) null) || !(voice = actionParameter.voice).HasValue)
        return;
      int channel = actionParameter.winAnimation ? 2 : -1;
      this.voiceAction = instanceOrNull.playVoiceByID(this.targetUnit.unitVoicePattern, (int) voice.Value, channel);
    }
    else
      this.disableShieldModel();
  }

  private void disableShieldModel()
  {
    if (Object.op_Equality((Object) this.ui3DModel.model_creater_.EquipShieldModel, (Object) null) || !this.ui3DModel.model_creater_.EquipShieldModel.activeSelf)
      return;
    this.ui3DModel.model_creater_.EquipShieldModel.SetActive(false);
    this.StopCoroutine("doDisplayShield");
  }

  private IEnumerator doDisplayShield(float wait)
  {
    if (!Object.op_Equality((Object) this.ui3DModel.model_creater_.EquipShieldModel, (Object) null))
    {
      this.ui3DModel.model_creater_.EquipShieldModel.SetActive(true);
      yield return (object) new WaitForSeconds(wait);
      this.ui3DModel.model_creater_.EquipShieldModel.SetActive(false);
    }
  }

  private IEnumerator playVoiceWait()
  {
    if (this.voiceAction != -1)
    {
      while (Singleton<NGSoundManager>.GetInstance().IsVoicePlaying(this.voiceAction))
        yield return (object) null;
      this.voiceAction = -1;
    }
  }

  public void OnClickedActions()
  {
    if (this.IsPush)
      return;
    this.currentLayout.dirActions.SetActive(!this.currentLayout.dirActions.activeSelf);
    this.specialViewObj.SetActive(false);
    this.StartCoroutine("doChangeNameBtnShowActions");
  }

  private IEnumerator doChangeNameBtnShowActions()
  {
    if (string.IsNullOrEmpty(this.currentLayout.storeNameBtnShowActions))
      this.currentLayout.storeNameBtnShowActions = ((Object) this.currentLayout.btnActions).name;
    yield return (object) null;
    ((Object) this.currentLayout.btnActions).name = this.currentLayout.dirActions.activeSelf ? Unit0043Menu.DefineNameButtonClose : this.currentLayout.storeNameBtnShowActions;
  }

  public void ShowEventImage()
  {
    if (this.isLoading || this.IsPush)
      return;
    this.unitViewObj.SetActive(false);
    this.voicePanel.SetActive(false);
    this.setActiveEventImage(true);
  }

  public void HideEventImage() => this.setActiveEventImage(false);

  private void setActiveEventImage(bool flag)
  {
    NGTweenParts component = this.dirEventImage.GetComponent<NGTweenParts>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.isActive = flag;
    else
      this.dirEventImage.SetActive(flag);
  }

  public void ShowUnlockingConditions()
  {
    if (this.isLoading || this.IsPush || string.IsNullOrEmpty(this.unlockingConditions))
      return;
    Singleton<NGMessageUI>.GetInstance().SetMessageByPosType(this.unlockingConditions);
  }

  public void ShowUnlockingSEAScenarioConditions()
  {
    if (this.isLoading || this.IsPush || string.IsNullOrEmpty(this.unlockingScenarioConditions))
      return;
    Singleton<NGMessageUI>.GetInstance().SetMessageByPosType(this.unlockingScenarioConditions);
  }

  public void onClickedPlayDuelSkill()
  {
    if (this.duelSkill == null || this.IsPushAndSet())
      return;
    this.resetAction();
    this.StartCoroutine(this.doPlayDuelSkill(false));
  }

  public void onClickedPlayMultiDuelSkill()
  {
    if (this.multiDuelSkill == null || this.IsPushAndSet())
      return;
    this.resetAction();
    this.StartCoroutine(this.doPlayDuelSkill(true));
  }

  public void SEAScenarioPlay() => Story0093Scene.changeScene009_3(true, this.SEAScriptId, false);

  private IEnumerator doPlayDuelSkill(bool multi)
  {
    Unit0043Menu unit0043Menu = this;
    PlayerUnitSkills koyuSkill = multi ? unit0043Menu.multiDuelSkill : unit0043Menu.duelSkill;
    DuelEnvironment duelEnv = unit0043Menu.duelDemoSettings.makeEnvironment();
    Future<DuelResult> future = unit0043Menu.duelDemoSettings.makeResult(unit0043Menu.targetPlayerUnit, koyuSkill);
    yield return (object) future.Wait();
    if (future.Result == null)
    {
      unit0043Menu.IsPush = false;
    }
    else
    {
      unit0043Menu.isChangedSceneDuelDemo = true;
      Singleton<NGSceneManager>.GetInstance().changeScene(Unit0043Menu.DuelSceneName, true, (object) future.Result, (object) duelEnv);
      unit0043Menu.duelDemoSettings.playBGM();
    }
  }

  private void resetAction()
  {
    if (!this.playingAction.HasValue)
      return;
    this.StopCoroutine("doPlayAction");
    Singleton<NGSoundManager>.GetInstance().StopVoice();
    this.voiceAction = -1;
    if (this.currentLayout.actionParameters[this.playingAction.Value].winAnimation && Object.op_Inequality((Object) this.winAnimator.Result, (Object) null))
      this.ui3DModel.model_creater_.ResetAnimator();
    else
      this.ui3DModel.ResetWaitAction();
    this.disableShieldModel();
    this.playingAction = new int?();
  }

  private void finalizedDuelSettings()
  {
    if (!this.duelDemoSettings.isSetupedLightmaps)
      return;
    Singleton<NGDuelDataManager>.GetInstance().Init();
    this.duelDemoSettings.clearLightmaps();
    Singleton<NGSceneManager>.GetInstance().destroyScene(Unit0043Menu.DuelSceneName);
  }

  private void SetUnitViewBtnState()
  {
    ((Component) this.trustViewBtn).gameObject.SetActive(this.targetUnit.IsResonanceUnit || this.targetUnit.IsSea);
    this.trustViewBtnSprite.spriteName = this.targetUnit.IsSea ? "slc_button_text_favorability_18pt.png__GUI__004-3_sozai__004-3_sozai_prefab" : "slc_button_text_kyoumei_18pt.png__GUI__004-3_sozai__004-3_sozai_prefab";
    if (((Component) this.trustViewBtn).gameObject.activeSelf && (double) this.targetPlayerUnit.trust_rate < 100.0)
    {
      ((UIButtonColor) this.trustViewBtn).hover = ((UIButtonColor) this.trustViewBtn).pressed = ((UIButtonColor) this.trustViewBtn).defaultColor = Color.gray;
      this.trustViewBtn.pressedSprite = this.trustViewBtn.normalSprite;
      ((UIWidget) ((Component) this.trustViewBtn).GetComponent<UISprite>()).color = Color.gray;
      foreach (UIWidget componentsInChild in ((Component) this.trustViewBtn).GetComponentsInChildren<UISprite>())
        componentsInChild.color = Color.gray;
    }
    ((Component) this.awakeViewBtn).gameObject.SetActive(false);
    if (this.targetUnit.CanAwakeUnitFlag)
    {
      ((Component) this.awakeViewBtn).gameObject.SetActive(true);
      ((UIButtonColor) this.awakeViewBtn).defaultColor = Color.gray;
      ((UIWidget) ((Component) this.awakeViewBtn).GetComponent<UISprite>()).color = Color.gray;
      foreach (UIWidget componentsInChild in ((Component) this.awakeViewBtn).GetComponentsInChildren<UISprite>())
        componentsInChild.color = Color.gray;
    }
    if (this.targetUnit.awake_unit_flag)
      ((Component) this.awakeViewBtn).gameObject.SetActive(true);
    ((Component) this.callViewBtn).gameObject.SetActive(((IEnumerable<int>) SMManager.Get<Player>().call_skill_same_character_ids).Contains<int>(this.targetPlayerUnit.unit.same_character_id) || this.hitDivorce != null);
    if (((Component) this.callViewBtn).gameObject.activeSelf)
    {
      if (!((Component) this.trustViewBtn).gameObject.activeSelf && ((Component) this.awakeViewBtn).gameObject.activeSelf)
      {
        ((Component) this.gachaViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x - (double) ((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width - 10.0), ((Component) this.gachaViewBtn).transform.localPosition.y));
        ((Component) this.awakeViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2(((Component) this.bg).transform.localPosition.x, ((Component) this.gachaViewBtn).transform.localPosition.y));
        ((Component) this.callViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x + (double) ((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width + 10.0), ((Component) this.gachaViewBtn).transform.localPosition.y));
      }
      else if (((Component) this.trustViewBtn).gameObject.activeSelf && !((Component) this.awakeViewBtn).gameObject.activeSelf)
      {
        ((Component) this.trustViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x - (double) ((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width - 10.0), ((Component) this.gachaViewBtn).transform.localPosition.y));
        ((Component) this.gachaViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2(((Component) this.bg).transform.localPosition.x, ((Component) this.gachaViewBtn).transform.localPosition.y));
        ((Component) this.callViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x + (double) ((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width + 10.0), ((Component) this.gachaViewBtn).transform.localPosition.y));
      }
      else
      {
        ((Component) this.trustViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x - (double) (((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width / 2) - 10.0) - (float) ((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width, ((Component) this.gachaViewBtn).transform.localPosition.y));
        ((Component) this.gachaViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2(((Component) this.bg).transform.localPosition.x - (float) (((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width / 2), ((Component) this.gachaViewBtn).transform.localPosition.y));
        ((Component) this.awakeViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x + (double) (((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width / 2) + 10.0), ((Component) this.gachaViewBtn).transform.localPosition.y));
        ((Component) this.callViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x + (double) (((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width / 2) + 20.0) + (float) ((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width, ((Component) this.gachaViewBtn).transform.localPosition.y));
      }
    }
    else
    {
      if (!((Component) this.trustViewBtn).gameObject.activeSelf && ((Component) this.awakeViewBtn).gameObject.activeSelf)
      {
        ((Component) this.gachaViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2(((Component) this.bg).transform.localPosition.x - (float) (((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width / 2), ((Component) this.gachaViewBtn).transform.localPosition.y));
        ((Component) this.awakeViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x + (double) (((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width / 2) + 10.0), ((Component) this.gachaViewBtn).transform.localPosition.y));
      }
      if (!((Component) this.trustViewBtn).gameObject.activeSelf || ((Component) this.awakeViewBtn).gameObject.activeSelf)
        return;
      ((Component) this.trustViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2(((Component) this.bg).transform.localPosition.x - (float) (((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width / 2), ((Component) this.gachaViewBtn).transform.localPosition.y));
      ((Component) this.gachaViewBtn).transform.localPosition = Vector2.op_Implicit(new Vector2((float) ((double) ((Component) this.bg).transform.localPosition.x + (double) (((UIWidget) ((Component) this.gachaViewBtn).GetComponent<UISprite>()).width / 2) + 10.0), ((Component) this.gachaViewBtn).transform.localPosition.y));
    }
  }

  private void SetMessageInfo(string str)
  {
    this.messageObj.SetActive(false);
    this.messageObj.SetActive(true);
    this.messageObj.GetComponentInChildren<UILabel>().SetTextLocalize(str);
    ((UITweener) this.messageObj.GetComponent<TweenAlpha>()).ResetToBeginning();
    ((UITweener) this.messageObj.GetComponent<TweenAlpha>()).PlayForward();
  }

  private IEnumerator PlayTrustView()
  {
    Unit0043Menu unit0043Menu = this;
    if (!unit0043Menu.isPlayCutin)
    {
      unit0043Menu.isPlayCutin = true;
      unit0043Menu.messageObj.SetActive(false);
      if ((double) unit0043Menu.targetPlayerUnit.trust_rate < 100.0)
      {
        if (!unit0043Menu.targetUnit.IsSea)
          unit0043Menu.SetMessageInfo(Consts.GetInstance().unit_004_3_trust_text);
        else
          unit0043Menu.SetMessageInfo(Consts.GetInstance().unit_004_3_love_text);
        unit0043Menu.isPlayCutin = false;
      }
      else if (unit0043Menu.targetUnit.ID == 0)
      {
        unit0043Menu.isPlayCutin = false;
      }
      else
      {
        Consts instance = Consts.GetInstance();
        PlayerUnit baseUnit = new PlayerUnit()
        {
          _unit = unit0043Menu.targetUnit.ID,
          trust_rate = instance.TRUST_RATE_LEVEL_SIZE
        };
        GameObject Prefab = (GameObject) null;
        Future<GameObject> prefabF;
        IEnumerator e;
        if (baseUnit.unit.IsSea)
        {
          if (Object.op_Equality((Object) unit0043Menu.trustEffectprefab, (Object) null))
          {
            prefabF = new ResourceObject("Animations/extraskill/FavorabilityRatingEffect").Load<GameObject>();
            e = prefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            Prefab = unit0043Menu.trustEffectprefab = prefabF.Result;
            prefabF = (Future<GameObject>) null;
          }
          else
            Prefab = unit0043Menu.trustEffectprefab;
        }
        else if (baseUnit.unit.IsResonanceUnit)
        {
          if (Object.op_Equality((Object) unit0043Menu.gearEffectprefab, (Object) null))
          {
            prefabF = new ResourceObject("Animations/common_gear_skill/CommonGearSkillEffect").Load<GameObject>();
            e = prefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            Prefab = unit0043Menu.gearEffectprefab = prefabF.Result;
            prefabF = (Future<GameObject>) null;
          }
          else
            Prefab = unit0043Menu.gearEffectprefab;
        }
        if (Object.op_Equality((Object) Prefab, (Object) null))
          unit0043Menu.isPlayCutin = false;
        else if (Object.op_Equality((Object) Singleton<CommonRoot>.GetInstance(), (Object) null) || Object.op_Equality((Object) Singleton<CommonRoot>.GetInstance().LoadTmpObj, (Object) null))
        {
          unit0043Menu.isPlayCutin = false;
        }
        else
        {
          Prefab = Prefab.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
          if (!Object.op_Equality((Object) Prefab, (Object) null))
          {
            // ISSUE: reference to a compiler-generated method
            e = Prefab.GetComponent<FavorabilityRatingEffect>().Init(FavorabilityRatingEffect.AnimationType.SkillRelease, baseUnit, new Action(unit0043Menu.\u003CPlayTrustView\u003Eb__178_0), isPreview: true);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            Singleton<PopupManager>.GetInstance().open(Prefab, isCloned: true);
            Prefab.GetComponent<FavorabilityRatingEffect>().StartEffect();
            yield return (object) null;
          }
        }
      }
    }
  }

  private void PlayGachaView()
  {
    if (this.targetUnit.ID == 0 || this.isPlayCutin)
      return;
    GachaResultData instance = GachaResultData.GetInstance();
    WebAPI.Response.GachaG001ChargePay gachaG001ChargePay = new WebAPI.Response.GachaG001ChargePay();
    gachaG001ChargePay.result = new WebAPI.Response.GachaG001ChargePayResult[1]
    {
      new WebAPI.Response.GachaG001ChargePayResult()
      {
        reward_result_quantity = 1,
        reward_result_id = this.targetPlayerUnit.id,
        is_new = true,
        reward_type_id = 1,
        direction_type_id = 4
      }
    };
    gachaG001ChargePay.additional_items = new WebAPI.Response.GachaG001ChargePayAdditional_items[0];
    WebAPI.Response.GachaG001ChargePay data = gachaG001ChargePay;
    if (data == null)
      return;
    instance.SetData(data);
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_effect", true, (object) true);
    Singleton<NGSoundManager>.GetInstance().StopBgm();
  }

  private void PlayAwakeView()
  {
    if (this.targetUnit.ID == 0 || this.isPlayCutin)
      return;
    if (this.targetUnit.CanAwakeUnitFlag)
    {
      this.SetMessageInfo(Consts.GetInstance().unit_004_3_awake_text);
    }
    else
    {
      if (!this.targetUnit.awake_unit_flag)
        return;
      List<PlayerUnit> playerUnitList = new List<PlayerUnit>();
      PrincesEvolutionParam princesEvolutionParam = new PrincesEvolutionParam();
      SMManager.Get<PlayerUnit[]>();
      PlayerUnit resultPlayerUnit = new PlayerUnit()
      {
        _unit = this.targetUnit.ID
      };
      int[] genealogyIds = UnitEvolutionPattern.getGenealogyIds(resultPlayerUnit._unit);
      int? nullable = ((IEnumerable<int>) genealogyIds).FirstIndexOrNull<int>((Func<int, bool>) (x => x == resultPlayerUnit._unit));
      if (!nullable.HasValue || nullable.Value <= 0)
        return;
      PlayerUnit playerUnit = new PlayerUnit()
      {
        _unit = genealogyIds[nullable.Value - 1]
      };
      princesEvolutionParam.materiaqlUnits = playerUnitList;
      princesEvolutionParam.is_new = true;
      princesEvolutionParam.baseUnit = playerUnit;
      princesEvolutionParam.resultUnit = resultPlayerUnit;
      princesEvolutionParam.isPreview = true;
      princesEvolutionParam.mode = playerUnit.unit.CanAwakeUnitFlag ? (playerUnit.unit.ID != 101414 ? Unit00499Scene.Mode.CommonAwakeUnit : Unit00499Scene.Mode.AwakeUnit) : Unit00499Scene.Mode.Evolution;
      unit00497Scene.ChangeScene(true, princesEvolutionParam);
    }
  }

  private void InitUnitVoiceData()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    string fileName = this.targetUnit.unitVoicePattern.file_name;
    int num = 0;
    this.lockedCount = 0;
    this.voiceItemList = new List<Unit0043VoiceItem>();
    foreach (KeyValuePair<int, UnitVoiceView> keyValuePair in MasterData.UnitVoiceView)
    {
      if (instance.IsEffectiveCueID(fileName, keyValuePair.Value.Cue_ID))
      {
        this.voiceInfo.Add(new Unit0043Menu.VoiceInfo()
        {
          voiceView = keyValuePair.Value
        });
        ++num;
        if (keyValuePair.Value.Condition == 1)
        {
          if (this.targetPlayerUnit.is_trust && (double) this.targetPlayerUnit.trust_rate < (double) keyValuePair.Value.ConditionValue)
            ++this.lockedCount;
        }
        else if (keyValuePair.Value.Condition == 2 && this.targetPlayerUnit.is_trust && this.targetPlayerUnit.unit.rarity.index < keyValuePair.Value.ConditionValue)
          ++this.lockedCount;
      }
    }
    this.lockedVoiceCount.SetTextLocalize(num - this.lockedCount);
    this.voiceCount.SetTextLocalize(num);
  }

  protected void ScrollUpdate()
  {
    if (!this.initFinished || this.voiceInfo.Count <= 5)
      return;
    int num1 = 150;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scrool_start_y;
    float num3 = (float) (Mathf.Max(0, this.voiceInfo.Count - 5 - 1) / 1 * 75);
    float num4 = 750f;
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    bool flag;
    do
    {
      flag = false;
      int num5 = 0;
      foreach (GameObject gameObject in this.scroll)
      {
        GameObject unit = gameObject;
        float num6 = unit.transform.localPosition.y + num2;
        if ((double) num6 > (double) num1)
        {
          int? nullable = this.voiceInfo.FirstIndexOrNull<Unit0043Menu.VoiceInfo>((Func<Unit0043Menu.VoiceInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
          int info_index = nullable.HasValue ? nullable.Value + 10 : this.voiceInfo.Count;
          if (nullable.HasValue && info_index < this.voiceInfo.Count)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y - num4, 0.0f);
            if (info_index >= this.voiceInfo.Count)
            {
              unit.SetActive(false);
            }
            else
            {
              this.ResetScroll(num5);
              this.CreateScroll(info_index, num5);
            }
            flag = true;
          }
        }
        else if ((double) num6 < -((double) num4 - (double) num1))
        {
          int num7 = 10;
          if (!unit.activeSelf)
          {
            unit.SetActive(true);
            num7 = 0;
          }
          int? nullable = this.voiceInfo.FirstIndexOrNull<Unit0043Menu.VoiceInfo>((Func<Unit0043Menu.VoiceInfo, bool>) (v => Object.op_Inequality((Object) v.scroll, (Object) null) && Object.op_Equality((Object) ((Component) v.scroll).gameObject, (Object) unit)));
          int info_index = nullable.HasValue ? nullable.Value - num7 : -1;
          if (nullable.HasValue && info_index >= 0)
          {
            unit.transform.localPosition = new Vector3(unit.transform.localPosition.x, unit.transform.localPosition.y + num4, 0.0f);
            this.ResetScroll(num5);
            this.CreateScroll(info_index, num5);
            flag = true;
          }
        }
        ++num5;
      }
    }
    while (flag);
  }

  private void CreateScroll(int info_index, int unit_index)
  {
    Unit0043VoiceItem scroll = this.voiceItemList[unit_index];
    this.voiceInfo.Where<Unit0043Menu.VoiceInfo>((Func<Unit0043Menu.VoiceInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<Unit0043Menu.VoiceInfo>((Action<Unit0043Menu.VoiceInfo>) (b => b.scroll = (Unit0043VoiceItem) null));
    this.voiceInfo[info_index].scroll = scroll;
    scroll.Init(this, this.voiceInfo[info_index].voiceView, this.targetPlayerUnit, ref this.lockedCount, (Action<string>) (x => this.SetMessageInfo(x)));
    ((Component) scroll).gameObject.SetActive(true);
  }

  private void ResetScroll(int index)
  {
    Unit0043VoiceItem scroll = this.voiceItemList[index];
    ((Component) scroll).gameObject.SetActive(false);
    this.voiceInfo.Where<Unit0043Menu.VoiceInfo>((Func<Unit0043Menu.VoiceInfo, bool>) (a => Object.op_Equality((Object) a.scroll, (Object) scroll))).ForEach<Unit0043Menu.VoiceInfo>((Action<Unit0043Menu.VoiceInfo>) (b => b.scroll = (Unit0043VoiceItem) null));
  }

  private void CreateScrollBase(GameObject prefab)
  {
    this.voiceItemList.Clear();
    for (int index = 0; index < Mathf.Min(10, this.voiceInfo.Count); ++index)
      this.voiceItemList.Add(Object.Instantiate<GameObject>(prefab).GetComponent<Unit0043VoiceItem>());
    this.scroll.Reset();
    for (int index = 0; index < Mathf.Min(10, this.voiceItemList.Count); ++index)
      this.scroll.AddColumn1(((Component) this.voiceItemList[index]).gameObject, 578, 75, true);
    this.scroll.CreateScrollPointHeight(75, this.voiceInfo.Count);
    this.scroll.ResolvePosition();
    for (int index = 0; index < Mathf.Min(10, this.voiceInfo.Count); ++index)
      this.ResetScroll(index);
    for (int index = 0; index < Mathf.Min(10, this.voiceInfo.Count); ++index)
      this.CreateScroll(index, index);
  }

  public void OpenConfession()
  {
    Sea030CallSkillReleaseScene.changeScene(true, this.targetUnit.same_character_id);
  }

  [Conditional("UNITY_EDITOR")]
  private static void debugLogWarning(string message) => Debug.LogWarning((object) message);

  [Serializable]
  private class ActionParameter
  {
    public string label;
    public bool perGearKind;
    public float waitNext = 1f;
    public bool winAnimation;
    public string voiceName = string.Empty;
    public float displayShield;

    public string[] states { get; set; }

    public CharaVoiceCueEnum.CueID? voice
    {
      get
      {
        if (string.IsNullOrEmpty(this.voiceName))
          return new CharaVoiceCueEnum.CueID?();
        CharaVoiceCueEnum.CueID result;
        return !Enum.TryParse<CharaVoiceCueEnum.CueID>(this.voiceName, out result) ? new CharaVoiceCueEnum.CueID?() : new CharaVoiceCueEnum.CueID?(result);
      }
    }
  }

  [Serializable]
  private class UnitCategoryLayout
  {
    public UnitCategory category;
    public GameObject dirTop;
    public GameObject dirNode;
    public float nodeXWithoutShowEventImage;
    public GameObject btnCloseContainer;
    public GameObject btn3dModel;
    public GameObject btn2dIllust;
    public GameObject btnActions;
    public GameObject dirActions;
    public GameObject btnProfile;
    [Tooltip("アニメーション再生パラメータ")]
    public Unit0043Menu.ActionParameter[] actionParameters;
    [Tooltip("actionParameters[]に対応するボタン")]
    public UIButton[] actionButtons;

    public string storeNameBtnShowActions { get; set; }

    public Vector2? storePos { get; set; }

    public float? storeNodeX { get; set; }
  }

  [Serializable]
  private class PairActionKey
  {
    public string label;
    public string[] states;
  }

  private enum IllustReleaseType
  {
    Overkillers,
    Level,
    Quest,
  }

  public class VoiceInfo
  {
    public Unit0043VoiceItem scroll;

    public UnitVoiceView voiceView { get; set; }

    public Unit0043Menu.VoiceInfo TempCopy()
    {
      Unit0043Menu.VoiceInfo voiceInfo = (Unit0043Menu.VoiceInfo) this.MemberwiseClone();
      voiceInfo.scroll = (Unit0043VoiceItem) null;
      return voiceInfo;
    }
  }
}
