// Decompiled with JetBrains decompiler
// Type: MapEdit031TopMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MapEdit;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class MapEdit031TopMenu : BackButtonMenuBase
{
  [SerializeField]
  private int sight_ = 2;
  [SerializeField]
  private GameObject root3D_;
  [SerializeField]
  private NGBattle3DObjectManager m3DObj_;
  [SerializeField]
  private MapEditCameraController cntlEditCamera_;
  [SerializeField]
  private SelectParts selectSight_;
  [SerializeField]
  private int swCost_ = 1;
  [SerializeField]
  private SelectParts selectCost_;
  [SerializeField]
  private MapEditUIEventTrigger uiEventTrigger_;
  [SerializeField]
  [Tooltip("地形情報の開始・終了アニメーション")]
  private NGTweenParts tweenGroundStatus_;
  [SerializeField]
  [Tooltip("設備情報表示の先頭位置")]
  private GameObject topInformation_;
  [SerializeField]
  [Tooltip("設置操作中のモデルに使う")]
  private Material matOrnamentSelected_;
  [SerializeField]
  [Tooltip("設置操作中の設置済みモデルに使う")]
  private Material matOrnamentNotLocation_;
  [SerializeField]
  private UIButton btnStorage_;
  [SerializeField]
  private Transform positionStorage_;
  [SerializeField]
  private UILabel txtTitleMap_;
  [SerializeField]
  private UILabel txtTitleSlot_;
  [SerializeField]
  private UILabel txtCost_;
  [SerializeField]
  [Tooltip("施設に憑くコスト表示のトップノード")]
  private GameObject topFacilityCost_;
  [SerializeField]
  [Tooltip("コスト表示を施設に憑ける際の高さ補正値")]
  private float yFacilityCost_ = 2f;
  [SerializeField]
  [Tooltip("施設に憑くコストの視点レベル毎の補正値")]
  private MapEdit031TopMenu.OffsetData[] facilityCostOffsets_ = new MapEdit031TopMenu.OffsetData[3];
  private bool initailzed_;
  private Dictionary<MapEdit031TopMenu.EditState, MapEditMenuBase> stateMenu_ = new Dictionary<MapEdit031TopMenu.EditState, MapEditMenuBase>();
  private MapEdit031MenuStorage storage_;
  private MapEdit031MenuSlotList slotList_;
  private MapEdit031TopMenu.EditState currentState_ = MapEdit031TopMenu.EditState.Initialize;
  private MapEdit031TopMenu.StateParam nextState_;
  private List<GameObject> nowActive_ = new List<GameObject>();
  private List<GameObject> requestDeactivate_ = new List<GameObject>();
  private List<GameObject> requestActivate_ = new List<GameObject>();
  private BL.StructValue<Vector2> facilityCostLocalPoint_ = new BL.StructValue<Vector2>();
  private BL.StructValue<Vector2> facilityCostLocalScale_ = new BL.StructValue<Vector2>();
  private MapEditData editData_;
  private Battle3DRoot battle3DRoot_;
  private Transform nodeOrnament_;
  private WeakReference wCurrentOrnament_;
  private MapEdit031TopMenu.TrackPanel lastTrackPanel_;
  private MapEdit031TopMenu.ControlEffectArea cntlEffectArea_;
  private NGBattleManager managerBattle_;
  private BE envBattle_;
  private GameObject prefabFacilityCost_;
  private MapEditFacilityInformation information_;
  private NGTweenParts tweenInformation_;
  private static int layer_mask_terrain_;
  private const float DISTANCE_RAYHIT = 1000f;
  private const float CYCLE_LOOP_CHANGE_EFFECT_AREA = 2f;

  public MapEditCameraController cntlCamera_ => this.cntlEditCamera_;

  public MapEditUIEventTrigger ui3DEvent_ => this.uiEventTrigger_;

  public bool isInitailzed_ => this.initailzed_;

  public void requestDeactivate(List<GameObject> objs)
  {
    if (objs == null || objs.Count <= 0)
      return;
    this.requestDeactivate_.AddRange((IEnumerable<GameObject>) objs);
  }

  public void requestActivate(List<GameObject> objs)
  {
    this.requestActivate_.AddRange((IEnumerable<GameObject>) objs);
  }

  public MapEditData data_ => this.editData_;

  public MapEditOrnament currentOrnament_
  {
    get
    {
      return this.wCurrentOrnament_ == null || !this.wCurrentOrnament_.IsAlive ? (MapEditOrnament) null : this.wCurrentOrnament_.Target as MapEditOrnament;
    }
    private set
    {
      if (Object.op_Equality((Object) this.currentOrnament_, (Object) value))
        return;
      this.wCurrentOrnament_ = Object.op_Inequality((Object) value, (Object) null) ? new WeakReference((object) value) : (WeakReference) null;
    }
  }

  public MapEditPanel currentPanel_ { get; private set; }

  private NGBattleManager bm_
  {
    get
    {
      if (Object.op_Equality((Object) this.managerBattle_, (Object) null))
      {
        this.managerBattle_ = Singleton<NGBattleManager>.GetInstance();
        this.setupBattleManager(this.managerBattle_);
      }
      return this.managerBattle_;
    }
  }

  private BE benv_
  {
    get
    {
      if (this.envBattle_ == null)
        this.envBattle_ = this.bm_.environment;
      return this.envBattle_;
    }
  }

  private bool isDisplayCost_ => this.swCost_ != 0;

  private bool isActiveUI3DEvent_
  {
    get
    {
      return ((Component) this.uiEventTrigger_).gameObject.activeSelf && this.uiEventTrigger_.isEnabled_;
    }
    set
    {
      if (this.isActiveUI3DEvent_ == value)
        return;
      if (value)
      {
        ((Component) this.uiEventTrigger_).gameObject.SetActive(true);
        this.uiEventTrigger_.isEnabled_ = true;
      }
      else
        this.uiEventTrigger_.isEnabled_ = false;
    }
  }

  public bool isEdit_ { get; private set; }

  public bool enabledInformation_ { get; private set; }

  public void setEnabledInformation(bool bEnabled)
  {
    if (Object.op_Equality((Object) this.information_, (Object) null))
      return;
    this.enabledInformation_ = bEnabled;
    if (!bEnabled)
      return;
    this.information_.updateInformation(this.currentOrnament_);
  }

  public bool isEnabledButtonStorage_
  {
    get
    {
      return Object.op_Inequality((Object) this.btnStorage_, (Object) null) && ((UIButtonColor) this.btnStorage_).isEnabled;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.btnStorage_, (Object) null))
        return;
      ((UIButtonColor) this.btnStorage_).isEnabled = value;
    }
  }

  public bool isActiveButtonStorage_
  {
    get
    {
      return Object.op_Inequality((Object) this.btnStorage_, (Object) null) && ((Component) this.btnStorage_).gameObject.activeSelf;
    }
    set
    {
      if (!Object.op_Inequality((Object) this.btnStorage_, (Object) null))
        return;
      ((Component) this.btnStorage_).gameObject.SetActive(value);
    }
  }

  public IEnumerator initialize(MapEditData editData, bool isEdit)
  {
    this.currentState_ = MapEdit031TopMenu.EditState.Initialize;
    this.currentOrnament_ = (MapEditOrnament) null;
    this.lastTrackPanel_ = (MapEdit031TopMenu.TrackPanel) null;
    this.currentPanel_ = (MapEditPanel) null;
    this.editData_ = editData;
    this.isEdit_ = isEdit;
    this.isActiveUI3DEvent_ = false;
    Future<Material> ldMat = MapEditFacilityShaderSetting.LoadMaterial("EditFacility_mat_000");
    IEnumerator e = ldMat.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.matOrnamentNotLocation_ = ldMat.Result;
    ldMat = (Future<Material>) null;
    ldMat = MapEditFacilityShaderSetting.LoadMaterial("EditFacility_mat_001");
    e = ldMat.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.matOrnamentSelected_ = ldMat.Result;
    ldMat = (Future<Material>) null;
    e = this.coInitUI();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.currentState_ = MapEdit031TopMenu.EditState.Start;
  }

  private IEnumerator Start()
  {
    MapEdit031TopMenu mapEdit031TopMenu = this;
    while (mapEdit031TopMenu.currentState_ != MapEdit031TopMenu.EditState.Start)
      yield return (object) null;
    IEnumerator e = mapEdit031TopMenu.coInit3DObj();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mapEdit031TopMenu.IsPush = true;
    // ISSUE: reference to a compiler-generated method
    mapEdit031TopMenu.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Start, new Action(mapEdit031TopMenu.\u003CStart\u003Eb__90_0));
  }

  private IEnumerator doWaitStart()
  {
    yield return (object) new WaitForEndOfFrame();
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
    yield return (object) new WaitForSeconds(1f);
    this.initailzed_ = true;
  }

  protected override void Update()
  {
    if (this.currentState_ == MapEdit031TopMenu.EditState.Initialize || this.currentState_ == MapEdit031TopMenu.EditState.Finished)
      return;
    base.Update();
    if (this.nextState_ != null)
    {
      MapEdit031TopMenu.StateParam nextState = this.nextState_;
      this.nextState_ = (MapEdit031TopMenu.StateParam) null;
      this.IsPush = false;
      if (nextState.onInit_ != null)
        nextState.onInit_();
      if (this.currentState_ != nextState.state_)
      {
        if (this.stateMenu_.ContainsKey(this.currentState_))
          this.stateMenu_[this.currentState_].isMenuActive_ = false;
        if (this.stateMenu_.ContainsKey(nextState.state_))
          this.stateMenu_[nextState.state_].isMenuActive_ = true;
        if (this.requestDeactivate_.Any<GameObject>())
        {
          List<GameObject> list = this.requestDeactivate_.Where<GameObject>((Func<GameObject, bool>) (o => !this.requestActivate_.Contains(o))).Distinct<GameObject>().ToList<GameObject>();
          this.activateObjects(list, false);
          foreach (GameObject gameObject in list)
            this.nowActive_.Remove(gameObject);
          this.requestDeactivate_.Clear();
        }
        if (this.requestActivate_.Any<GameObject>())
        {
          List<GameObject> list = this.requestActivate_.Where<GameObject>((Func<GameObject, bool>) (o => !this.nowActive_.Contains(o))).Distinct<GameObject>().ToList<GameObject>();
          this.activateObjects(list, true);
          this.nowActive_.AddRange((IEnumerable<GameObject>) list);
          this.requestActivate_.Clear();
        }
        this.currentState_ = nextState.state_;
      }
    }
    if (!this.enabledInformation_ || !Object.op_Inequality((Object) this.currentOrnament_, (Object) null))
      return;
    this.isActiveButtonStorage_ = false;
    if (Object.op_Inequality((Object) this.tweenInformation_, (Object) null))
    {
      if (this.tweenInformation_.isActive)
        return;
      this.tweenInformation_.isActive = true;
    }
    else
    {
      if (!Object.op_Inequality((Object) this.information_, (Object) null) || ((Component) this.information_).gameObject.activeSelf)
        return;
      ((Component) this.information_).gameObject.SetActive(true);
    }
  }

  private void activateObjects(List<GameObject> objs, bool bActive)
  {
    foreach (GameObject gameObject in objs)
    {
      NGTweenParts component = gameObject.GetComponent<NGTweenParts>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.forceActive(bActive);
      else
        gameObject.SetActive(bActive);
    }
  }

  private void setupBattleManager(NGBattleManager bm)
  {
    bm.setManagers(this.root3D_);
    bm.order = 0;
    bm.resetEnvironment();
    ((IEnumerable<BattleMonoBehaviour>) ((Component) bm).GetComponentsInChildren<BattleMonoBehaviour>()).ForEach<BattleMonoBehaviour>((Action<BattleMonoBehaviour>) (m => m.resetEnvironment()));
    ((IEnumerable<NGBattleMenuBase>) this.root3D_.GetComponentsInChildren<NGBattleMenuBase>()).ForEach<NGBattleMenuBase>((Action<NGBattleMenuBase>) (m => m.resetEnvironment()));
  }

  private IEnumerator coInitUI()
  {
    MapEdit031TopMenu menu = this;
    menu.stateMenu_.Clear();
    menu.storage_ = (MapEdit031MenuStorage) null;
    menu.slotList_ = (MapEdit031MenuSlotList) null;
    menu.nowActive_.Clear();
    menu.requestDeactivate_.Clear();
    menu.requestActivate_.Clear();
    if (Object.op_Inequality((Object) menu.selectSight_, (Object) null))
      menu.selectSight_.setFirstValue(menu.sight_);
    if (Object.op_Inequality((Object) menu.selectCost_, (Object) null))
      menu.selectCost_.setFirstValue(menu.swCost_);
    if (Object.op_Inequality((Object) menu.topFacilityCost_, (Object) null))
      menu.topFacilityCost_.SetActive(menu.isDisplayCost_);
    Future<GameObject> ldprefab;
    IEnumerator e;
    if (Object.op_Equality((Object) menu.information_, (Object) null) && Object.op_Inequality((Object) menu.topInformation_, (Object) null))
    {
      ldprefab = MapEdit.Prefabs.facility_info.Load<GameObject>();
      e = ldprefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) ldprefab.Result, (Object) null))
      {
        GameObject gameObject = ldprefab.Result.Clone(menu.topInformation_.transform);
        menu.information_ = gameObject.GetComponent<MapEditFacilityInformation>();
        menu.tweenInformation_ = gameObject.GetComponent<NGTweenParts>();
        menu.enabledInformation_ = false;
        if (Object.op_Inequality((Object) menu.information_, (Object) null))
          menu.information_.initialize(menu);
        if (Object.op_Inequality((Object) menu.tweenInformation_, (Object) null))
          menu.tweenInformation_.forceActive(false);
        else
          gameObject.SetActive(false);
      }
      ldprefab = (Future<GameObject>) null;
    }
    ldprefab = MapEdit.Prefabs.facility_cost.Load<GameObject>();
    e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.prefabFacilityCost_ = ldprefab.Result;
    ldprefab = (Future<GameObject>) null;
    MapEditMenuBase[] mapEditMenuBaseArray = ((Component) menu).GetComponents<MapEditMenuBase>();
    for (int index = 0; index < mapEditMenuBaseArray.Length; ++index)
    {
      MapEditMenuBase mapEditMenuBase = mapEditMenuBaseArray[index];
      if (!Object.op_Equality((Object) mapEditMenuBase, (Object) null))
      {
        menu.stateMenu_.Add(mapEditMenuBase.editState_, mapEditMenuBase);
        if (mapEditMenuBase.editState_ == MapEdit031TopMenu.EditState.Storage)
          menu.storage_ = mapEditMenuBase as MapEdit031MenuStorage;
        else if (mapEditMenuBase.editState_ == MapEdit031TopMenu.EditState.SlotList)
          menu.slotList_ = mapEditMenuBase as MapEdit031MenuSlotList;
        e = mapEditMenuBase.InitializeAsync();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    mapEditMenuBaseArray = (MapEditMenuBase[]) null;
    menu.activateObjects(menu.requestDeactivate_.Distinct<GameObject>().ToList<GameObject>(), false);
    menu.requestActivate_.Clear();
    menu.updateTitle();
    menu.updateCost();
  }

  private IEnumerator coInit3DObj(bool bClearedFacility = false)
  {
    BE env = this.benv_;
    env.core.sight.value = this.sight_;
    this.changeLocalPositionFacilityCost(this.sight_);
    MasterData.LoadBattleMapLandform(this.editData_.map_);
    env.core.initializeFeild(this.editData_.stage_.ID);
    if (env.core.condition == null)
      env.core.condition = new BL.Condition();
    env.core.condition.id = this.editData_.stage_.victory_condition.ID;
    this.editData_.stage_.ApplyLandforms((Action<int, int, BattleMapLandform>) ((x, y, landform) => env.core.setFeildPanel(landform.ID, y, x, landform.landform.ID, 0, (BL.DropData) null)));
    IEnumerator e = this.m3DObj_.initializeForEdit(env);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.battle3DRoot_ = this.m3DObj_.objectRoot.GetComponent<Battle3DRoot>();
    BattlePanelParts[] componentsInChildren = ((Component) this.battle3DRoot_.panelPoint).GetComponentsInChildren<BattlePanelParts>();
    int lengthRow = this.editData_.lengthRow_;
    int lengthColumn = this.editData_.lengthColumn_;
    foreach (BattlePanelParts battlePanelParts in componentsInChildren)
    {
      BL.Panel panel = battlePanelParts.getPanel();
      if (panel != null && panel.row < lengthRow && panel.column < lengthColumn)
      {
        int px = panel.column + 1;
        int py = panel.row + 1;
        BattleMapFacilitySetting mapFacilitySetting = this.editData_.facilitySetting_ != null ? ((IEnumerable<BattleMapFacilitySetting>) this.editData_.facilitySetting_).FirstOrDefault<BattleMapFacilitySetting>((Func<BattleMapFacilitySetting, bool>) (fs => fs.coordinate_y == py && fs.coordinate_x == px)) : (BattleMapFacilitySetting) null;
        GvgStageFormation gvgStageFormation = this.editData_.formation_ != null ? ((IEnumerable<GvgStageFormation>) this.editData_.formation_).FirstOrDefault<GvgStageFormation>((Func<GvgStageFormation, bool>) (fs => fs.formation_y == py && fs.formation_x == px)) : (GvgStageFormation) null;
        int num = gvgStageFormation != null ? gvgStageFormation.player_order : -1;
        this.editData_.matrix_[panel.row, panel.column] = MapEditPanel.attach(((Component) battlePanelParts).gameObject, panel, mapFacilitySetting != null, num == 1, num == 0);
      }
    }
    this.bm_.initialized = true;
    while (!this.cntlCamera_.initialized_)
      yield return (object) null;
    this.nodeOrnament_ = ((Component) this.battle3DRoot_.panelPoint).transform.GetChildInFind("Units");
    if (!bClearedFacility && this.editData_.originalPosition_ != null && this.editData_.originalPosition_.Length != 0)
    {
      PlayerGuildTownSlotPosition[] townSlotPositionArray = this.editData_.originalPosition_;
      for (int index = 0; index < townSlotPositionArray.Length; ++index)
      {
        PlayerGuildTownSlotPosition townSlotPosition = townSlotPositionArray[index];
        MapEditPanel p = this.getPanel(townSlotPosition.y - 1, townSlotPosition.x - 1);
        if (!Object.op_Equality((Object) p, (Object) null))
        {
          PlayerGuildFacility facility = this.storage_.getFacility(townSlotPosition.master_id);
          if (facility != null)
          {
            MapEditOrnament ornament = (MapEditOrnament) null;
            e = this.instantiateOrnament(facility, p.center_, (Action<MapEditOrnament>) (ret => ornament = ret));
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            if (Object.op_Inequality((Object) ornament, (Object) null))
              ornament.setLocation(p);
            p = (MapEditPanel) null;
          }
        }
      }
      townSlotPositionArray = (PlayerGuildTownSlotPosition[]) null;
    }
    int column = this.editData_.stage_.map_width / 2;
    BL.Panel fieldPanel = env.core.getFieldPanel(this.editData_.stage_.map_height / 2, column);
    this.setCurrentPanel((MapEditPanel) null, false, isNosmoothCamera: true, isInit: true);
    this.cntlCamera_.setLookAtTarget(fieldPanel, true);
  }

  private IEnumerator cleanup3DObj()
  {
    this.isActiveUI3DEvent_ = false;
    IEnumerator e = this.m3DObj_.cleanup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    NGBattleManager bm = this.bm_;
    bm.setManagers((GameObject) null);
    ((IEnumerable<NGBattleMenuBase>) this.root3D_.GetComponentsInChildren<NGBattleMenuBase>()).ForEach<NGBattleMenuBase>((Action<NGBattleMenuBase>) (m => m.resetEnvironment()));
    e = bm.cleanupBattle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.managerBattle_ = (NGBattleManager) null;
    this.envBattle_ = (BE) null;
  }

  public MapEditPanel getTouchPanel() => this.hitPanel(UICamera.lastTouchPosition);

  public MapEditPanel hitPanel(Vector2 pos)
  {
    return this.castPanel(NGBattle3DObjectManager.hitPanel(pos));
  }

  public MapEditPanel castPanel(BL.Panel panel)
  {
    return panel == null ? (MapEditPanel) null : this.editData_.matrix_[panel.row, panel.column];
  }

  public bool setCurrentPanel(
    MapEditPanel panel,
    bool panelSelected = true,
    bool cameraCenter = true,
    bool isNosmoothCamera = false,
    bool isInit = false)
  {
    if (!isInit && Object.op_Equality((Object) this.currentPanel_, (Object) panel))
      return false;
    if (this.lastTrackPanel_ != null && this.lastTrackPanel_.isSelected_)
      this.lastTrackPanel_.panel_.setSelected(false);
    if (Object.op_Inequality((Object) panel, (Object) null))
    {
      this.lastTrackPanel_ = new MapEdit031TopMenu.TrackPanel(panel, panelSelected);
      this.currentPanel_ = panel;
      if (panelSelected)
        panel.setSelected(true);
      if (cameraCenter)
        this.cntlCamera_.setLookAtTarget(panel.panel_, isNosmoothCamera);
    }
    else
    {
      this.lastTrackPanel_ = (MapEdit031TopMenu.TrackPanel) null;
      this.currentPanel_ = (MapEditPanel) null;
    }
    Singleton<NGBattleManager>.GetInstance().environment.core.fieldCurrent.value = isInit || Object.op_Equality((Object) panel, (Object) null) ? (BL.Panel) null : panel.panel_;
    if (Object.op_Inequality((Object) this.tweenGroundStatus_, (Object) null))
      this.tweenGroundStatus_.isActive = !isInit && Object.op_Inequality((Object) panel, (Object) null);
    return true;
  }

  public void clearCurrentPanel()
  {
    if (Object.op_Inequality((Object) this.currentPanel_, (Object) null))
    {
      if (this.lastTrackPanel_.isSelected_)
        this.currentPanel_.setSelected(false);
      this.lastTrackPanel_ = (MapEdit031TopMenu.TrackPanel) null;
      this.currentPanel_ = (MapEditPanel) null;
    }
    Singleton<NGBattleManager>.GetInstance().environment.core.fieldCurrent.value = (BL.Panel) null;
    if (!Object.op_Inequality((Object) this.tweenGroundStatus_, (Object) null))
      return;
    this.tweenGroundStatus_.isActive = false;
  }

  public void setCurrentOrnament(MapEditOrnament ornament) => this.currentOrnament_ = ornament;

  public void clearCurrentOrnamnet() => this.currentOrnament_ = (MapEditOrnament) null;

  public IEnumerator coEndSceneAsync()
  {
    IEnumerator e = this.cleanup3DObj();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override void onBackButton()
  {
  }

  public void onExitOK()
  {
    if (this.currentState_ == MapEdit031TopMenu.EditState.Finished)
      return;
    this.currentState_ = MapEdit031TopMenu.EditState.Finished;
    this.nextState_ = (MapEdit031TopMenu.StateParam) null;
    GuildUtil.last_edit_slot_no = this.editData_.editId_;
    this.backScene();
  }

  public void onExitNG()
  {
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
  }

  public void onClickedSightDistance()
  {
    if (Object.op_Equality((Object) this.selectSight_, (Object) null) || this.selectSight_.inclementLoop() == this.sight_)
      return;
    this.changeSightDistance(this.selectSight_.getValue());
  }

  private void changeSightDistance(int noSight)
  {
    int noSpeed = this.sight_ > noSight ? 1 : 0;
    this.sight_ = noSight;
    BE benv = this.benv_;
    benv.core.sight.value = noSight;
    this.cntlCamera_.sightDistance = this.bm_.sightDistances[benv.core.sight.value];
    this.changeLocalPositionFacilityCost(noSight);
    if (!Object.op_Inequality((Object) this.topFacilityCost_, (Object) null) || !this.topFacilityCost_.activeSelf)
      return;
    foreach (MapEditFacilityCost componentsInChild in this.topFacilityCost_.GetComponentsInChildren<MapEditFacilityCost>())
      componentsInChild.updateLocalPosition(false, noSpeed);
  }

  private void changeLocalPositionFacilityCost(int noSight)
  {
    if (this.facilityCostOffsets_.Length <= noSight)
      return;
    this.facilityCostLocalPoint_.value = this.facilityCostOffsets_[noSight].point_;
    this.facilityCostLocalScale_.value = this.facilityCostOffsets_[noSight].scale_;
  }

  public void onClickedCost()
  {
    if (Object.op_Equality((Object) this.selectCost_, (Object) null) || this.selectCost_.inclementLoop() == this.swCost_)
      return;
    this.changeSwitchCost(this.selectCost_.getValue());
  }

  private void changeSwitchCost(int sw)
  {
    this.swCost_ = sw;
    if (!Object.op_Inequality((Object) this.topFacilityCost_, (Object) null))
      return;
    this.topFacilityCost_.SetActive(this.isDisplayCost_);
  }

  public void onClickedMenu()
  {
    if (this.nextState_ != null || this.currentState_ == MapEdit031TopMenu.EditState.Menu || this.IsPushAndSet())
      return;
    MapEdit031TopMenu.StateParam stateParam = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Menu);
    if (this.currentState_ == MapEdit031TopMenu.EditState.Layout)
    {
      TrackOrnament trackOrnament = this.undoLocation();
      if (trackOrnament.isNew_)
      {
        this.stateMenu_[MapEdit031TopMenu.EditState.Effect].setProcessAsync(new Func<object, IEnumerator>(this.doReturnStorage), (object) stateParam);
        this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Effect);
        return;
      }
      this.currentOrnament_.setSelected(false);
      MapEditPanel panel = this.getPanel(trackOrnament.row_, trackOrnament.column_);
      if (Object.op_Inequality((Object) panel, (Object) null))
      {
        this.setCurrentPanel(panel, cameraCenter: false);
        this.cntlCamera_.setLookAtTarget(panel.center_);
      }
    }
    this.nextState_ = stateParam;
  }

  public void onClickedStorage()
  {
    if (this.nextState_ != null || this.currentState_ == MapEdit031TopMenu.EditState.Storage || this.IsPushAndSet())
      return;
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Storage);
  }

  public void startLayout(Action onInit = null)
  {
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Layout, onInit);
  }

  public void backLayout()
  {
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
  }

  public bool backStorage()
  {
    if (this.nextState_ != null)
      return false;
    if (this.currentState_ != MapEdit031TopMenu.EditState.View)
      this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
    return true;
  }

  public void onClickedMenuClose()
  {
    if (this.nextState_ != null || this.currentState_ != MapEdit031TopMenu.EditState.Menu || this.IsPushAndSet())
      return;
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
  }

  public void onClickedEnd()
  {
    if (this.nextState_ != null || this.currentState_ == MapEdit031TopMenu.EditState.End || this.IsPushAndSet())
      return;
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.End);
  }

  public MapEditPanel getPanel(int row, int column)
  {
    return row < 0 || row >= this.editData_.lengthRow_ || column < 0 || column >= this.editData_.lengthColumn_ ? (MapEditPanel) null : this.editData_.matrix_[row, column];
  }

  public void startLayout(MapEditOrnament ornament)
  {
    if (Object.op_Equality((Object) ornament, (Object) null))
      return;
    if (ornament.hasLocation_)
    {
      MapEditPanel mapEditPanel = this.editData_.matrix_[ornament.row_, ornament.column_];
      if (Object.op_Equality((Object) mapEditPanel.ornament_, (Object) ornament))
      {
        this.editData_.stackTrackOrnament_.Push(new TrackOrnament(ornament.ID_, ornament.row_, ornament.column_));
        mapEditPanel.setOrnament((MapEditOrnament) null);
      }
    }
    this.setCurrentOrnament(ornament);
    ornament.setSelected(true);
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Layout);
  }

  public bool setLocation()
  {
    MapEditOrnament currentOrnament = this.currentOrnament_;
    if (!Object.op_Inequality((Object) this.currentPanel_, (Object) null) || !Object.op_Inequality((Object) currentOrnament, (Object) null) || !this.currentPanel_.checkLocation())
      return false;
    currentOrnament.setLocation(this.currentPanel_);
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
    return true;
  }

  public TrackOrnament undoLocation()
  {
    MapEditOrnament currentOrnament = this.currentOrnament_;
    if (Object.op_Equality((Object) currentOrnament, (Object) null))
      return (TrackOrnament) null;
    int id = currentOrnament.ID_;
    TrackOrnament trackOrnament;
    if (this.editData_.stackTrackOrnament_.Count > 0 && this.editData_.stackTrackOrnament_.Peek().ID_ == id)
    {
      trackOrnament = this.editData_.stackTrackOrnament_.Pop();
      if (!trackOrnament.isNew_)
        currentOrnament.setLocation(this.editData_.matrix_[trackOrnament.row_, trackOrnament.column_], true);
    }
    else
      trackOrnament = new TrackOrnament(id);
    return trackOrnament;
  }

  public void changePanelDrawLayout()
  {
    MapEditPanel[,] matrix = this.editData_.matrix_;
    int upperBound1 = matrix.GetUpperBound(0);
    int upperBound2 = matrix.GetUpperBound(1);
    for (int lowerBound1 = matrix.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
    {
      for (int lowerBound2 = matrix.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
        matrix[lowerBound1, lowerBound2].changeDrawLayout();
    }
  }

  public void resetPanelDraw()
  {
    MapEditPanel[,] matrix = this.editData_.matrix_;
    int upperBound1 = matrix.GetUpperBound(0);
    int upperBound2 = matrix.GetUpperBound(1);
    for (int lowerBound1 = matrix.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
    {
      for (int lowerBound2 = matrix.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
        matrix[lowerBound1, lowerBound2].resetDraw();
    }
  }

  public void changeOrnamentDrawLayout()
  {
    MapEditOrnament currentOrnament = this.currentOrnament_;
    foreach (KeyValuePair<int, MapEditOrnament> keyValuePair in this.editData_.dicOrnament_)
    {
      if (!Object.op_Inequality((Object) currentOrnament, (Object) null) || keyValuePair.Key != currentOrnament.ID_)
        keyValuePair.Value.changeDrawNotLocation();
    }
  }

  public void resetOrnamentDraw()
  {
    MapEditOrnament currentOrnament = this.currentOrnament_;
    foreach (KeyValuePair<int, MapEditOrnament> keyValuePair in this.editData_.dicOrnament_)
    {
      if (!Object.op_Inequality((Object) currentOrnament, (Object) null) || keyValuePair.Key != currentOrnament.ID_)
        keyValuePair.Value.resetDraw();
    }
  }

  public void newOrnament(int id)
  {
    if (!Object.op_Equality((Object) this.storage_, (Object) null))
    {
      PlayerGuildFacility newFacility = this.storage_.useFacility(id);
      if (newFacility != null)
      {
        this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Effect, (Action) (() => this.stateMenu_[MapEdit031TopMenu.EditState.Effect].setProcessAsync(new Func<object, IEnumerator>(this.doCreateOrnament), (object) newFacility)));
        return;
      }
    }
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
  }

  private static int LAYER_MASK_TERRAIN
  {
    get
    {
      if (MapEdit031TopMenu.layer_mask_terrain_ == 0)
        MapEdit031TopMenu.layer_mask_terrain_ = 1 << LayerMask.NameToLayer("Terrain");
      return MapEdit031TopMenu.layer_mask_terrain_;
    }
  }

  private IEnumerator doCreateOrnament(object arg1)
  {
    PlayerGuildFacility newFacility = (PlayerGuildFacility) arg1;
    Vector3 firstpos;
    if (Object.op_Inequality((Object) this.currentPanel_, (Object) null))
    {
      firstpos = this.currentPanel_.center_;
    }
    else
    {
      Camera camera = this.cntlCamera_.Camera;
      firstpos = Vector3.op_Addition(((Component) camera).transform.position, Vector3.op_Multiply(((Component) camera).transform.forward, this.cntlCamera_.sightDistance));
    }
    RaycastHit raycastHit;
    Vector3 pos = !Physics.Raycast(this.cntlCamera_.Camera.ScreenPointToRay(UICamera.currentCamera.WorldToScreenPoint(((Component) this.positionStorage_).transform.position)), ref raycastHit, 1000f, MapEdit031TopMenu.LAYER_MASK_TERRAIN) ? firstpos : ((RaycastHit) ref raycastHit).point;
    MapEditOrnament ornament = (MapEditOrnament) null;
    IEnumerator e = this.instantiateOrnament(newFacility, pos, (Action<MapEditOrnament>) (ret => ornament = ret));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) ornament, (Object) null))
    {
      this.storage_.returnFacility(newFacility._master);
      this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
    }
    else
    {
      ornament.setSelected(true);
      ornament.setPosition(firstpos);
      while (ornament.isMove_)
        yield return (object) null;
      this.startLayout(ornament);
    }
  }

  private IEnumerator instantiateOrnament(
    PlayerGuildFacility facility,
    Vector3 pos,
    Action<MapEditOrnament> callbackReturn)
  {
    UnitUnit unit = facility.unit;
    Future<GameObject> ldPrefab = unit != null ? unit.LoadModelField() : (Future<GameObject>) null;
    IEnumerator e;
    if (ldPrefab != null)
    {
      e = ldPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (ldPrefab == null || Object.op_Equality((Object) ldPrefab.Result, (Object) null))
    {
      if (callbackReturn != null)
        callbackReturn((MapEditOrnament) null);
    }
    else
    {
      GameObject go = ldPrefab.Result.Clone(this.nodeOrnament_);
      go.transform.localPosition = Vector3.zero;
      go.transform.localScale = Vector3.one;
      go.transform.localRotation = Quaternion.identity;
      FacilityLevel facilityLevel = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).FirstOrDefault<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.level == facility.level && x.unit_UnitUnit == unit.ID));
      if (facilityLevel != null)
      {
        foreach (Transform parent in go.GetComponentsInChildren<Transform>())
        {
          if (((Object) parent).name == "attach_point")
          {
            Future<GameObject> f = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("Facility/3D/{0}", (object) facilityLevel.attach_point_name));
            e = f.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GameObject gameObject = f.Result.Clone(parent);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localRotation = Quaternion.identity;
            break;
          }
          parent = (Transform) null;
        }
      }
      MapEditOrnament ornament = MapEditOrnament.attach(go, facility, this.editData_.generateId_, pos, this.matOrnamentSelected_, this.matOrnamentNotLocation_);
      while (!ornament.isInitialized_)
        yield return (object) null;
      this.editData_.addOrnament(ornament);
      MapEditFacilityCost component = this.prefabFacilityCost_.Clone(this.topFacilityCost_.transform).GetComponent<MapEditFacilityCost>();
      component.initialize(this.cntlCamera_.Camera, UICamera.currentCamera, go, this.yFacilityCost_, unit.cost);
      component.setReferenceLocalPosition(this.facilityCostLocalPoint_, this.facilityCostLocalScale_);
      if (callbackReturn != null)
        callbackReturn(ornament);
    }
  }

  public void returnStorageWithEffect()
  {
    this.stateMenu_[MapEdit031TopMenu.EditState.Effect].setProcessAsync(new Func<object, IEnumerator>(this.doReturnStorage), (object) new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View));
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Effect);
  }

  private IEnumerator doReturnStorage(object arg1)
  {
    MapEdit031TopMenu.StateParam nextState = (MapEdit031TopMenu.StateParam) arg1;
    MapEditOrnament cur = this.currentOrnament_;
    if (Object.op_Equality((Object) cur, (Object) null))
    {
      this.nextState_ = nextState;
    }
    else
    {
      while (this.cntlCamera_.isCameraMove)
        yield return (object) null;
      cur.setSelected(true);
      cur.startMoveScreen(UICamera.currentCamera.WorldToScreenPoint(this.positionStorage_.position));
      while (cur.isMoveScreen_)
        yield return (object) null;
      this.returnStorage(cur);
      this.nextState_ = nextState;
    }
  }

  private void returnStorage(MapEditOrnament ornament)
  {
    if (Object.op_Equality((Object) ornament, (Object) null))
      return;
    if (Object.op_Equality((Object) this.currentOrnament_, (Object) ornament))
      this.currentOrnament_ = (MapEditOrnament) null;
    this.editData_.removeOrnament(ornament.ID_);
    if (ornament.facility_ != null)
      this.storage_.returnFacility(ornament.facility_._master);
    ornament.destroySelf();
  }

  public void returnStorageAll()
  {
    foreach (MapEditOrnament ornament in this.editData_.dicOrnament_.Select<KeyValuePair<int, MapEditOrnament>, MapEditOrnament>((Func<KeyValuePair<int, MapEditOrnament>, MapEditOrnament>) (kv => kv.Value)).ToList<MapEditOrnament>())
      this.returnStorage(ornament);
  }

  public void setDrawEffectArea(
    int center_row,
    int center_column,
    int minRange,
    int maxRange,
    BattleskillTargetType target)
  {
    this.resetDrawEffectArea();
    this.cntlEffectArea_ = new MapEdit031TopMenu.ControlEffectArea(center_row, center_column, minRange, maxRange, target);
    switch (target)
    {
      case BattleskillTargetType.player_range:
        this.changePanelDrawDefense(this.cntlEffectArea_.row_, this.cntlEffectArea_.column_, this.cntlEffectArea_.minRange_, this.cntlEffectArea_.maxRange_);
        break;
      case BattleskillTargetType.enemy_range:
        this.changePanelDrawOffense(this.cntlEffectArea_.row_, this.cntlEffectArea_.column_, this.cntlEffectArea_.minRange_, this.cntlEffectArea_.maxRange_);
        break;
      case BattleskillTargetType.complex_range:
        this.StartCoroutine("doLoopChangeDrawEffectArea");
        break;
      default:
        this.cntlEffectArea_ = (MapEdit031TopMenu.ControlEffectArea) null;
        break;
    }
  }

  public void resetDrawEffectArea()
  {
    if (this.cntlEffectArea_ == null)
      return;
    if (this.cntlEffectArea_.target_ == BattleskillTargetType.complex_range)
      this.StopCoroutine("doLoopChangeDrawEffectArea");
    this.resetPanelDrawEffect(this.cntlEffectArea_.row_, this.cntlEffectArea_.column_, this.cntlEffectArea_.minRange_, this.cntlEffectArea_.maxRange_);
    this.cntlEffectArea_ = (MapEdit031TopMenu.ControlEffectArea) null;
  }

  private IEnumerator doLoopChangeDrawEffectArea()
  {
    int count = 0;
    bool flag = false;
    while (this.cntlEffectArea_ != null && this.cntlEffectArea_.target_ == BattleskillTargetType.complex_range)
    {
      if (flag)
        this.resetPanelDrawEffect(this.cntlEffectArea_.row_, this.cntlEffectArea_.column_, this.cntlEffectArea_.minRange_, this.cntlEffectArea_.maxRange_);
      if ((count & 1) == 0)
        this.changePanelDrawDefense(this.cntlEffectArea_.row_, this.cntlEffectArea_.column_, this.cntlEffectArea_.minRange_, this.cntlEffectArea_.maxRange_);
      else
        this.changePanelDrawOffense(this.cntlEffectArea_.row_, this.cntlEffectArea_.column_, this.cntlEffectArea_.minRange_, this.cntlEffectArea_.maxRange_);
      yield return (object) new WaitForSeconds(2f);
      flag = true;
      ++count;
    }
  }

  public void changePanelDrawOffense(int centerRow, int centerColumn, int minRange, int maxRange)
  {
    foreach (MapEdit031TopMenu.PanelPos panelPos in this.makeEffectRange(centerRow, centerColumn, minRange, maxRange))
      this.editData_.matrix_[panelPos.row_, panelPos.column_].changeDrawOffense();
  }

  public void changePanelDrawDefense(int centerRow, int centerColumn, int minRange, int maxRange)
  {
    foreach (MapEdit031TopMenu.PanelPos panelPos in this.makeEffectRange(centerRow, centerColumn, minRange, maxRange))
      this.editData_.matrix_[panelPos.row_, panelPos.column_].changeDrawDefense();
  }

  public void resetPanelDrawEffect(int centerRow, int centerColumn, int minRange, int maxRange)
  {
    foreach (MapEdit031TopMenu.PanelPos panelPos in this.makeEffectRange(centerRow, centerColumn, minRange, maxRange))
      this.editData_.matrix_[panelPos.row_, panelPos.column_].resetDrawEffect();
  }

  private LinkedList<MapEdit031TopMenu.PanelPos> makeEffectRange(
    int centerRow,
    int centerColumn,
    int minRange,
    int maxRange)
  {
    LinkedList<MapEdit031TopMenu.PanelPos> ret = new LinkedList<MapEdit031TopMenu.PanelPos>();
    if (maxRange < 1)
      return ret;
    this.applyPanelEffectRange(centerRow, centerColumn, maxRange, (Action<int, int>) ((row, column) => ret.AddLast(new MapEdit031TopMenu.PanelPos(row, column))));
    --minRange;
    if (minRange > 0)
      this.applyPanelEffectRange(centerRow, centerColumn, minRange, (Action<int, int>) ((row, column) =>
      {
        for (LinkedListNode<MapEdit031TopMenu.PanelPos> node = ret.First; node != null; node = node.Next)
        {
          MapEdit031TopMenu.PanelPos panelPos = node.Value;
          if (panelPos.row_ == row && panelPos.column_ == column)
          {
            ret.Remove(node);
            break;
          }
        }
      }));
    return ret;
  }

  private void applyPanelEffectRange(
    int centerRow,
    int centerColumn,
    int range,
    Action<int, int> effect)
  {
    if (range < 0)
      return;
    int num1 = centerRow - range;
    int num2 = centerRow + range;
    int num3 = 0;
    if (num1 < 0)
    {
      num3 = -num1;
      num1 = 0;
    }
    int lengthRow = this.editData_.lengthRow_;
    if (num2 >= lengthRow)
      num2 = lengthRow - 1;
    int lengthColumn = this.editData_.lengthColumn_;
    for (; num1 <= num2; ++num1)
    {
      int num4 = centerColumn - num3;
      int num5 = centerColumn + num3;
      if (num4 < 0)
        num4 = 0;
      if (num5 >= lengthColumn)
        num5 = lengthColumn - 1;
      for (; num4 <= num5; ++num4)
        effect(num1, num4);
      num3 += num1 < centerRow ? 1 : -1;
    }
  }

  private void updateTitle()
  {
    MapTown mapTown = this.editData_.mapTown_;
    if (mapTown == null)
      return;
    this.txtTitleMap_.SetTextLocalize(mapTown.name);
    Consts instance = Consts.GetInstance();
    Hashtable args = new Hashtable()
    {
      {
        (object) "name",
        (object) instance.SAVE_SLOT_NAME
      },
      {
        (object) "num",
        (object) (this.editData_.editId_ + 1)
      }
    };
    this.txtTitleSlot_.SetTextLocalize(Consts.Format(instance.MAPEDIT_031_SLOT_NAME, (IDictionary) args));
  }

  public void updateCost()
  {
    MapTown mapTown = this.editData_.mapTown_;
    if (mapTown == null || Object.op_Equality((Object) this.storage_, (Object) null))
      return;
    Hashtable args = new Hashtable()
    {
      {
        (object) "used",
        (object) (mapTown.cost_capacity - this.storage_.remainingCost_)
      },
      {
        (object) "limited",
        (object) mapTown.cost_capacity
      }
    };
    this.txtCost_.SetTextLocalize(Consts.Format(Consts.GetInstance().MAPEDIT_031_FOOTER_COST, (IDictionary) args));
  }

  public void onClickedMapSelect()
  {
    if (this.nextState_ != null || this.currentState_ == MapEdit031TopMenu.EditState.MapCatalog || this.IsPushAndSet())
      return;
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.MapCatalog);
  }

  public void backMapCatalog()
  {
    if (this.nextState_ != null || this.currentState_ == MapEdit031TopMenu.EditState.Menu)
      return;
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
  }

  public void exchangeMap(int townId)
  {
    this.stateMenu_[MapEdit031TopMenu.EditState.Effect].setProcessAsync(new Func<object, IEnumerator>(this.doExchangeMap), (object) townId);
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Effect);
  }

  private IEnumerator doExchangeMap(object arg1)
  {
    MapEditData newEdit = new MapEditData(this.editData_.editId_, (int) arg1);
    if (newEdit.isError_)
    {
      this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      yield return (object) new WaitForSeconds(0.5f);
      this.initailzed_ = false;
      this.editData_ = newEdit;
      IEnumerator e = this.coInit3DObj(true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) this.storage_, (Object) null))
        this.storage_.resetFacilityCountAll(this.editData_.facilitySetting_.Length, this.editData_.mapTown_.cost_capacity);
      this.updateTitle();
      this.updateCost();
      this.initailzed_ = true;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      yield return (object) new WaitForSeconds(0.5f);
      this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.Menu);
    }
  }

  public void onClickedSave()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.doPopupConfirmSaveSlot());
  }

  private IEnumerator doPopupConfirmSaveSlot()
  {
    MapEdit031TopMenu mapEdit031TopMenu = this;
    bool bExec = false;
    IEnumerator e = MapEditPopupConfirmSaveSlot.show(mapEdit031TopMenu.editData_.editId_, mapEdit031TopMenu.editData_.defaultSlot_, mapEdit031TopMenu.editData_.checkModified(mapEdit031TopMenu.editData_.editId_), (Action) (() => bExec = true), (Action) (() => { }), new Action(mapEdit031TopMenu.gotoSlotList));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (bExec)
    {
      bool bErr = false;
      e = mapEdit031TopMenu.editData_.doSave((Action<WebAPI.Response.UserError>) (err =>
      {
        bErr = true;
        WebAPI.DefaultUserErrorCallback(err);
        if (!err.Code.Equals("GVG002"))
          return;
        MypageScene.ChangeSceneOnError();
      }));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!bErr && Object.op_Inequality((Object) mapEdit031TopMenu.slotList_, (Object) null))
      {
        e = mapEdit031TopMenu.slotList_.updateInformation();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private void gotoSlotList()
  {
    if (this.nextState_ != null || this.currentState_ == MapEdit031TopMenu.EditState.SlotList)
      return;
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.SlotList);
  }

  public void backSlotList()
  {
    this.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View);
  }

  public void onChangedSlotAndSave(int slotId)
  {
    this.StartCoroutine(this.doChangedSlotAndSave(slotId));
  }

  private IEnumerator doChangedSlotAndSave(int slotId)
  {
    MapEdit031TopMenu mapEdit031TopMenu = this;
    mapEdit031TopMenu.IsPush = true;
    mapEdit031TopMenu.nextState_ = new MapEdit031TopMenu.StateParam(MapEdit031TopMenu.EditState.View, (Action) (() => this.IsPush = true));
    bool bErr = false;
    IEnumerator e = mapEdit031TopMenu.editData_.doSave(slotId, (Action<WebAPI.Response.UserError>) (err =>
    {
      bErr = true;
      WebAPI.DefaultUserErrorCallback(err);
      if (!err.Code.Equals("GVG002"))
        return;
      MypageScene.ChangeSceneOnError();
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!bErr && Object.op_Inequality((Object) mapEdit031TopMenu.slotList_, (Object) null))
    {
      e = mapEdit031TopMenu.slotList_.updateInformation();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      mapEdit031TopMenu.updateTitle();
    }
    mapEdit031TopMenu.IsPush = false;
  }

  public enum EditState
  {
    Idle,
    Error,
    Initialize,
    Finished,
    Start,
    View,
    Layout,
    Menu,
    Storage,
    MapCatalog,
    SlotList,
    Effect,
    End,
  }

  private class StateParam
  {
    public MapEdit031TopMenu.EditState state_ { get; private set; }

    public Action onInit_ { get; private set; }

    public StateParam(MapEdit031TopMenu.EditState editstate, Action eventInit = null)
    {
      this.state_ = editstate;
      this.onInit_ = eventInit;
    }
  }

  [Serializable]
  public class OffsetData
  {
    public Vector2 point_ = Vector2.zero;
    public Vector2 scale_ = Vector2.one;
  }

  private class TrackPanel
  {
    public MapEditPanel panel_ { get; private set; }

    public bool isSelected_ { get; private set; }

    public TrackPanel(MapEditPanel panel, bool selected)
    {
      this.panel_ = panel;
      this.isSelected_ = selected;
    }
  }

  private class PanelPos
  {
    public int row_ { get; private set; }

    public int column_ { get; private set; }

    public PanelPos(int row, int column)
    {
      this.row_ = row;
      this.column_ = column;
    }
  }

  private class ControlEffectArea
  {
    public int row_ { get; private set; }

    public int column_ { get; private set; }

    public int minRange_ { get; private set; }

    public int maxRange_ { get; private set; }

    public BattleskillTargetType target_ { get; private set; }

    public ControlEffectArea(
      int center_row,
      int center_column,
      int minRange,
      int maxRange,
      BattleskillTargetType target)
    {
      this.row_ = center_row;
      this.column_ = center_column;
      this.minRange_ = minRange;
      this.maxRange_ = maxRange;
      this.target_ = target;
    }
  }
}
