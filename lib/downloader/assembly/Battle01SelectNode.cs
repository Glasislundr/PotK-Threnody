// Decompiled with JetBrains decompiler
// Type: Battle01SelectNode
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
public class Battle01SelectNode : BattleMonoBehaviour
{
  public const string coin_path = "Prefabs/BattleCommon/coin/coin_prefab";
  public float dead_effect_time = 5f;
  public float pvp_dead_effect_time = 5f;
  private const string effect_retreat = "Retreat";
  private const string popupUnitDeadPlayerPrefab_path = "Prefabs/popup/Battle_Breakout";
  private const string popupUnitDeadEnemyPrefab_path = "Prefabs/popup/Battle_Defeat";
  private const string popupLeaderUnitDeadPlayerPrefab_path = "Prefabs/battle/Battle_Breakout_Leader";
  private const string popupLeaderUnitDeadEnemyPrefab_path = "Prefabs/battle/Battle_Defeat_Leader";
  [SerializeField]
  private NGTweenParts autoBattle;
  private BattleUI01LayoutAutoLink layoutAutoLink;
  [SerializeField]
  private NGTweenParts character_Act;
  [SerializeField]
  private NGTweenParts character_choosen;
  [SerializeField]
  private NGTweenParts character_choosen_enemy;
  [SerializeField]
  private NGTweenParts character_choosen_facility;
  [SerializeField]
  private NGTweenParts item_select;
  [SerializeField]
  private NGTweenParts item_subject;
  [SerializeField]
  private NGTweenParts skill_select;
  [SerializeField]
  private GameObject command_skill_name_sprite;
  [SerializeField]
  private GameObject call_skill_name_sprite;
  [SerializeField]
  private NGTweenParts skill_subject;
  [SerializeField]
  private NGTweenParts skill_use;
  [SerializeField]
  private NGTweenParts menu;
  [SerializeField]
  private NGTweenParts submenu;
  [SerializeField]
  private NGTweenParts item_button;
  [SerializeField]
  private NGTweenParts back;
  [SerializeField]
  private NGTweenParts back_up;
  [SerializeField]
  private NGTweenParts grandstatus_left;
  [SerializeField]
  private NGTweenParts grandstatus_right;
  [SerializeField]
  private Battle01TipEventWindow tipevent;
  [SerializeField]
  private NGTweenParts withdraw;
  [SerializeField]
  private GameObject talkButtonPoint;
  [SerializeField]
  private GameObject uiMaskPanel;
  [SerializeField]
  private GameObject uiMaskPanelReady;
  [SerializeField]
  private GameObject remain_turn;
  [SerializeField]
  private NGTweenParts pvp_position_arrange;
  [SerializeField]
  private NGTweenParts pvp_ready;
  [SerializeField]
  private GameObject pvp_ready_title;
  [SerializeField]
  private UIButton btnReady;
  [SerializeField]
  private UIButton btnDecide;
  [SerializeField]
  private GameObject autoButton;
  [SerializeField]
  private NGTweenParts dirCallSkill;
  [SerializeField]
  private UIButton ibtnCallSkill;
  [SerializeField]
  private GameObject dirCallGauge;
  [SerializeField]
  private GameObject slcCallSkillGauge;
  [SerializeField]
  private GameObject slcCallSkillEffects;
  [SerializeField]
  private GameObject dirCallSkillCharged;
  [SerializeField]
  private NGTweenParts dirCallSkillEnemy;
  [SerializeField]
  private UIButton ibtnCallSkillEnemy;
  [SerializeField]
  private GameObject dirCallGaugeEnemy;
  [SerializeField]
  private GameObject slcCallSkillGaugeEnemy;
  [SerializeField]
  private GameObject slcCallSkillEffectsEnemy;
  private GameObject enemyCallSkillDetailPrefab;
  private BL.BattleModified<BL.CurrentUnit> currentUnitModified;
  private BL.BattleModified<BL.PhaseState> phaseStateModified;
  private BL.BattleModified<BL.CallSkillState> playerCallSkillStateModified;
  private BL.BattleModified<BL.CallSkillState> enemyCallSkillStateModified;
  private BL.BattleModified<BL.StructValue<bool>> isAutoBattleModified;
  private BL.BattleModified<BL.ClassValue<List<BL.Item>>> itemModified;
  private List<BL.BattleModified<BL.UnitPosition>> playerUnitPositionModifiedList;
  private List<BL.BattleModified<BL.Unit>> unitModifiedList;
  private NGTweenParts current;
  private Stack<NGTweenParts> stack = new Stack<NGTweenParts>();
  private TreasureBoxManager tbManager;
  private BattleTimeManager btm;
  private GameObject coinPrefab;
  private GameObject popupUnitDeadPlayerPrefab;
  private GameObject popupUnitDeadEnemyPrefab;
  private GameObject popupLeaderUnitDeadPlayerPrefab;
  private GameObject popupLeaderUnitDeadEnemyPrefab;
  [SerializeField]
  private Battle01UIPlayerStatus enemyStatus;
  [SerializeField]
  private Battle01UIFacilityStatus facilityStatus;
  [SerializeField]
  private GameObject spdButton1x;
  [SerializeField]
  private GameObject spdButton2x;
  [SerializeField]
  private GameObject spdButton3x;
  [SerializeField]
  private GameObject spdButton4x;
  private const int SPEED_1 = 1;
  private const int SPEED_2 = 2;
  private const int SPEED_3 = 3;
  private const int SPEED_4 = 4;
  [SerializeField]
  private GameObject dirPvpExceptionLoading;
  [SerializeField]
  private GameObject dirPvpExceptionLoadingMask;
  private string loadingPvpExceptionPrefabPath = "Prefabs/common/Loading/Loading_PvpExceptionPrefab";
  private GameObject loadingPvpExceptionPrefab;
  private BattleInputObserver inputObserver;
  private int settingSpeed;
  [SerializeField]
  private Transform SkillNoticeAnchor;
  private BattleUI01CommandSkillNotice SkillNotice;
  [SerializeField]
  private Transform dynBattleSkillSEA;
  private BattleUI01ShortCutConfig shortCutConfig;
  private int triggerPlayerStartTurn;
  private Battle01PVPHeader pvpHeader;
  private GameObject prefabCallOwn;
  private GameObject prefabCallEnemy;
  private bool is_push;
  private bool isSEASkillCutinCompleted = true;

  public bool IsForceAutoDisable { get; private set; }

  public Battle01CommandBack CommandBack { get; private set; }

  public Battle01CommandWait CommandWait { get; private set; }

  public bool IsEnabledCommandWait
  {
    get
    {
      return ((Component) this.menu).gameObject.activeSelf && ((Component) this.menu).GetComponent<SelectParts>().getValue() == 0;
    }
  }

  public void OpenUnitInfoPopup(BL.Unit unit)
  {
    if (!Object.op_Inequality((Object) this.enemyStatus, (Object) null) || !(unit != (BL.Unit) null) || unit.isFacility)
      return;
    this.enemyStatus.OpenInfoPopup(unit);
  }

  public override IEnumerator onInitAsync()
  {
    Battle01SelectNode battle01SelectNode = this;
    Battle01PVPNode component = ((Component) battle01SelectNode).GetComponent<Battle01PVPNode>();
    if (Object.op_Inequality((Object) component, (Object) null))
      battle01SelectNode.pvpHeader = component.PVPHeader;
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    battle01SelectNode.shortCutConfig = ((Component) battle01SelectNode).GetComponent<BattleUI01ShortCutConfig>();
    battle01SelectNode.layoutAutoLink = Object.op_Inequality((Object) battle01SelectNode.autoBattle, (Object) null) ? ((Component) battle01SelectNode.autoBattle).GetComponent<BattleUI01LayoutAutoLink>() : (BattleUI01LayoutAutoLink) null;
    Future<GameObject> f = rm.Load<GameObject>(battle01SelectNode.battleManager.isSea ? "Prefabs/battle/dir_SkillExplanation_Display_Sea" : "Prefabs/battle/dir_SkillExplanation_Display");
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) battle01SelectNode.SkillNoticeAnchor, (Object) null))
      battle01SelectNode.SkillNotice = f.Result.Clone(battle01SelectNode.SkillNoticeAnchor).GetComponent<BattleUI01CommandSkillNotice>();
    f = rm.Load<GameObject>("Prefabs/BattleCommon/coin/coin_prefab");
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01SelectNode.coinPrefab = f.Result;
    if (battle01SelectNode.battleManager.isOvo)
    {
      f = rm.LoadOrNull<GameObject>("Prefabs/popup/Battle_Breakout");
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle01SelectNode.popupUnitDeadPlayerPrefab = f.Result;
      f = rm.LoadOrNull<GameObject>("Prefabs/popup/Battle_Defeat");
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle01SelectNode.popupUnitDeadEnemyPrefab = f.Result;
      f = rm.LoadOrNull<GameObject>("Prefabs/battle/Battle_Breakout_Leader");
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle01SelectNode.popupLeaderUnitDeadPlayerPrefab = f.Result;
      f = rm.LoadOrNull<GameObject>("Prefabs/battle/Battle_Defeat_Leader");
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle01SelectNode.popupLeaderUnitDeadEnemyPrefab = f.Result;
    }
    else
    {
      f = Res.Prefabs.popup.Battle01712aPrefab.Load<GameObject>();
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle01SelectNode.popupUnitDeadPlayerPrefab = f.Result;
    }
    UI2DSprite sprite;
    Future<Sprite> spriteF;
    Future<Material> matF;
    if (Object.op_Inequality((Object) battle01SelectNode.slcCallSkillGauge, (Object) null))
    {
      sprite = battle01SelectNode.slcCallSkillGauge.GetComponent<UI2DSprite>();
      string path1 = battle01SelectNode.battleManager.isSea ? "Prefabs/battle/Material/slc_Battle_CallSkill_Gauge_sea" : "Prefabs/battle/Material/slc_Battle_CallSkill_Gauge";
      spriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path1);
      e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sprite.sprite2D = spriteF.Result;
      string path2 = battle01SelectNode.battleManager.isSea ? "Prefabs/battle/Material/Gauge_mask_sea" : "Prefabs/battle/Material/Gauge_mask";
      matF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Material>(path2);
      e = matF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((UIWidget) sprite).material = matF.Result;
      sprite = (UI2DSprite) null;
      spriteF = (Future<Sprite>) null;
      matF = (Future<Material>) null;
    }
    if (Object.op_Inequality((Object) battle01SelectNode.slcCallSkillGaugeEnemy, (Object) null))
    {
      sprite = battle01SelectNode.slcCallSkillGaugeEnemy.GetComponent<UI2DSprite>();
      string path3 = "Prefabs/battle/Material/slc_BattleEnemy_CallSkill_Gauge";
      spriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path3);
      e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sprite.sprite2D = spriteF.Result;
      string path4 = "Prefabs/battle/Material/Gauge_mask_enemy";
      matF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Material>(path4);
      e = matF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((UIWidget) sprite).material = matF.Result;
      sprite = (UI2DSprite) null;
      spriteF = (Future<Sprite>) null;
      matF = (Future<Material>) null;
    }
    battle01SelectNode.InitCommand();
  }

  private void InitCommand()
  {
    this.CommandBack = ((Component) this.back_up).GetComponent<Battle01CommandBack>();
    this.CommandWait = ((Component) ((Component) this.menu).transform.Find("dir_Menu/dir_Menu_button1/ibtn_Wait")).GetComponent<Battle01CommandWait>();
  }

  public void initializeStage()
  {
    try
    {
      this.settingSpeed = Persist.battleTimeSetting.Data.speed;
    }
    catch (Exception ex)
    {
      Persist.battleTimeSetting.Delete();
      Persist.battleTimeSetting.Data = new Persist.BattleTimeSetting();
    }
    this.SetSpeed(this.settingSpeed);
    this.playerUnitPositionModifiedList = new List<BL.BattleModified<BL.UnitPosition>>();
    foreach (BL.Unit unit in this.env.core.playerUnits.value)
      this.playerUnitPositionModifiedList.Add(BL.Observe<BL.UnitPosition>(this.env.core.getUnitPosition(unit)));
    this.unitModifiedList = new List<BL.BattleModified<BL.Unit>>();
    foreach (BL.UnitPosition unitPosition in this.env.core.unitPositions.value)
      this.unitModifiedList.Add(BL.Observe<BL.Unit>(unitPosition.unit));
    this.setEnemyUnit(this.env.core.enemyUnits.value[0]);
    this.remain_turn.SetActive(this.env.core.condition.isTurn || this.env.core.condition.isElapsedTurn);
    this.triggerPlayerStartTurn = this.env.core.phaseState.turnCount;
  }

  protected override IEnumerator Start_Battle()
  {
    Battle01SelectNode node = this;
    node.currentUnitModified = BL.Observe<BL.CurrentUnit>(node.env.core.unitCurrent);
    node.phaseStateModified = BL.Observe<BL.PhaseState>(node.env.core.phaseState);
    node.playerCallSkillStateModified = BL.Observe<BL.CallSkillState>(node.env.core.playerCallSkillState);
    node.enemyCallSkillStateModified = BL.Observe<BL.CallSkillState>(node.env.core.enemyCallSkillState);
    node.isAutoBattleModified = BL.Observe<BL.StructValue<bool>>(node.env.core.isAutoBattle);
    node.itemModified = BL.Observe<BL.ClassValue<List<BL.Item>>>(node.env.core.itemListInBattle);
    node.initializeStage();
    node.selectPhaseDefault(node.env.core.phaseState);
    node.tbManager = node.battleManager.getManager<TreasureBoxManager>();
    node.btm = node.battleManager.getManager<BattleTimeManager>();
    node.battleManager.getController<BattleStateController>().setUiNode(node);
    node.battleManager.setUiNode(node);
    node.inputObserver = node.battleManager.getController<BattleInputObserver>();
    node.inputObserver.setFuncCheckCancelPressed(new Func<bool, bool>(node.checkCancelPressedOnInputObserver));
    node.inputObserver.setFuncCheckCancelClick(new Func<bool>(node.checkCancelClickOnInputObserver));
    if (Object.op_Equality((Object) node.shortCutConfig, (Object) null) && Object.op_Inequality((Object) node.autoButton, (Object) null))
      node.autoButton.GetComponent<NGTweenParts>().forceActive(node.env.core.battleInfo.isAutoBattleEnable);
    if (Object.op_Inequality((Object) node.dirCallSkill, (Object) null))
    {
      node.setCallSkillButton();
      Vector3 localPosition = ((Component) node.dirCallSkill).transform.localPosition;
      float x = ((Component) node.item_button).transform.localPosition.x;
      if (node.env.core.itemListInBattle.value.Count != 0)
        x += 120f;
      localPosition.x = x;
      ((Component) node.dirCallSkill).transform.localPosition = localPosition;
      if (Object.op_Inequality((Object) ((Component) node.dirCallSkill).gameObject.GetComponent<AnimationApplyBase>(), (Object) null))
        ((Component) node.dirCallSkill).gameObject.GetComponent<AnimationApplyBase>().Reset();
    }
    if (Object.op_Inequality((Object) node.dirCallSkillEnemy, (Object) null))
      node.setEnemyCallSkillButton();
    if (node.battleManager.isPvp && Object.op_Inequality((Object) node.dirPvpExceptionLoading, (Object) null))
    {
      if (Object.op_Equality((Object) node.loadingPvpExceptionPrefab, (Object) null))
      {
        Future<GameObject> tmpF = Singleton<ResourceManager>.GetInstance().Load<GameObject>(node.loadingPvpExceptionPrefabPath);
        IEnumerator e = tmpF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        node.loadingPvpExceptionPrefab = tmpF.Result;
        tmpF = (Future<GameObject>) null;
      }
      node.dirPvpExceptionLoading.transform.Clear();
      node.loadingPvpExceptionPrefab.Clone(node.dirPvpExceptionLoading.transform);
      node.battleManager.pvpManager.setUiNode(node);
      node.setActivePvpExceptionLoadingUI(false);
    }
  }

  private bool checkCancelPressedOnInputObserver(bool pressed)
  {
    return this.env.core.isAutoBattle.value || this.battleManager.getController<BattleStateController>().isWaitCurrentAIActionCancel || this.env.core.unitPositions.value.Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (u => u.isMoving(this.env)));
  }

  private bool checkCancelClickOnInputObserver()
  {
    return this.env.core.isAutoBattle.value || this.battleManager.getController<BattleStateController>().isWaitCurrentAIActionCancel || this.env.core.unitPositions.value.Any<BL.UnitPosition>((Func<BL.UnitPosition, bool>) (u => u.isMoving(this.env)));
  }

  private bool phasePlayerp()
  {
    return this.env.core.phaseState.state == BL.Phase.player || this.env.core.phaseState.state == BL.Phase.pvp_player_start;
  }

  private bool nodeDefaultp()
  {
    return Object.op_Equality((Object) this.current, (Object) this.autoBattle) || Object.op_Equality((Object) this.current, (Object) this.character_Act) || Object.op_Equality((Object) this.current, (Object) this.character_choosen) || Object.op_Equality((Object) this.current, (Object) this.character_choosen_enemy) || Object.op_Equality((Object) this.current, (Object) this.character_choosen_facility) || Object.op_Equality((Object) this.current, (Object) this.pvp_ready);
  }

  private bool checkAutoUIControl(NGTweenParts node)
  {
    if (!Object.op_Inequality((Object) this.layoutAutoLink, (Object) null))
      return false;
    return Object.op_Equality((Object) node, (Object) this.autoBattle) || Object.op_Equality((Object) node, (Object) this.character_Act);
  }

  private bool isDisposition
  {
    get
    {
      return this.env.core.phaseState.state == BL.Phase.pvp_disposition || this.env.core.phaseState.state == BL.Phase.pvp_wait_preparing;
    }
  }

  private bool menuActivep()
  {
    BL.Unit unit = this.env.core.unitCurrent.unit;
    return !this.isDisposition && unit != (BL.Unit) null && unit.isPlayerControl && this.env.core.currentPhaseUnitp(unit);
  }

  private bool itemActivep()
  {
    return !this.env.core.firstCompleted.value && this.env.core.itemListInBattle.value.Count != 0 && !this.menuActivep();
  }

  private bool callSkillActivep()
  {
    return this.battleManager.isPvp ? !this.env.core.playerCallSkillState.isSomeAction && this.env.core.playerCallSkillState.skillId != 0 && !this.menuActivep() : !this.env.core.firstCompleted.value && this.env.core.playerCallSkillState.skillId != 0 && !this.menuActivep();
  }

  private void setCurrent(NGTweenParts node, bool isStack = false, bool popStack = false)
  {
    if (Object.op_Equality((Object) node, (Object) null) & popStack)
      node = this.stack.Pop();
    bool isModeAuto = this.env.core.isAutoBattle.value;
    if (Object.op_Inequality((Object) this.current, (Object) node))
    {
      if (Object.op_Inequality((Object) this.current, (Object) null))
      {
        this.setScrollSeEnable(this.current, false);
        if (this.checkAutoUIControl(this.current))
          this.layoutAutoLink.activate(false, isModeAuto);
        else
          this.current.isActive = false;
        if (isStack)
          this.stack.Push(this.current);
        else if (!popStack)
          this.stack.Clear();
      }
      if (this.checkAutoUIControl(node))
        this.layoutAutoLink.activate(true, isModeAuto);
      else
        node.isActive = true;
      if (Object.op_Inequality((Object) this.layoutAutoLink, (Object) null))
        this.layoutAutoLink.flush(Object.op_Equality((Object) this.current, (Object) this.character_Act));
      this.current = node;
    }
    this.setScrollSeEnable(this.current, true);
    if (this.nodeDefaultp())
    {
      if (Object.op_Equality((Object) this.current, (Object) this.character_choosen) || Object.op_Equality((Object) this.current, (Object) this.character_choosen_enemy) || Object.op_Equality((Object) this.current, (Object) this.character_choosen_facility))
      {
        this.menu.isActive = this.menuActivep();
        this.item_button.isActive = false;
        if (this.isDisposition)
          this.pvp_position_arrange.isActive = true;
        else
          this.back_up.isActive = this.phasePlayerp();
      }
      else if (Object.op_Equality((Object) this.current, (Object) this.character_Act))
      {
        this.menu.isActive = false;
        this.item_button.isActive = this.itemActivep();
        this.back_up.isActive = false;
      }
      else if (Object.op_Equality((Object) this.current, (Object) this.pvp_ready))
      {
        this.menu.isActive = false;
        this.pvp_position_arrange.isActive = false;
      }
      else
      {
        this.menu.isActive = false;
        this.item_button.isActive = false;
        this.back_up.isActive = false;
      }
      this.back.isActive = false;
    }
    else
    {
      this.menu.isActive = false;
      this.item_button.isActive = false;
      this.back_up.isActive = true;
      this.back.isActive = false;
    }
    if (Object.op_Inequality((Object) this.dirCallSkill, (Object) null))
    {
      if (Object.op_Equality((Object) this.current, (Object) this.character_Act))
      {
        if (!this.dirCallSkill.isActive)
        {
          this.dirCallSkill.isActive = this.callSkillActivep();
          this.setCallSkillButton();
        }
      }
      else if (this.dirCallSkill.isActive)
        this.dirCallSkill.isActive = false;
    }
    if (Object.op_Equality((Object) this.current, (Object) this.character_choosen))
      this.setEnableBackUp(!this.env.core.currentUnitPosition.cantChangeCurrent);
    else if (Object.op_Equality((Object) this.current, (Object) this.skill_select) || Object.op_Equality((Object) this.current, (Object) this.skill_subject) || Object.op_Equality((Object) this.current, (Object) this.skill_use))
      this.setEnableBackUp(true);
    if (Object.op_Inequality((Object) this.shortCutConfig, (Object) null))
    {
      bool flag = !this.back_up.isActive && this.checkActiveShortCutConfig(this.current);
      if (this.shortCutConfig.isActive_ != flag)
        this.shortCutConfig.isActive_ = flag;
    }
    if (Object.op_Equality((Object) this.current, (Object) this.skill_subject))
      this.StartCoroutine(this.resetScrollViewSkillSubject());
    if (!Object.op_Equality((Object) this.current, (Object) this.item_subject))
      return;
    this.StartCoroutine(this.resetScrollViewItemSubject());
  }

  public IEnumerator resetScrollViewSkillSubject()
  {
    yield return (object) null;
    ((Component) this.skill_subject).gameObject.GetComponent<Battle01SkillSubject>().resetScrollView();
  }

  public IEnumerator resetScrollViewItemSubject()
  {
    yield return (object) null;
    ((Component) this.item_subject).gameObject.GetComponent<Battle01ItemSubject>().resetScrollView();
  }

  private bool checkActiveShortCutConfig(NGTweenParts node)
  {
    if (this.env.core.phaseState.state == BL.Phase.pvp_disposition)
      return false;
    return Object.op_Equality((Object) node, (Object) this.autoBattle) || Object.op_Equality((Object) node, (Object) this.character_Act) || Object.op_Equality((Object) node, (Object) this.character_choosen_enemy) || Object.op_Equality((Object) node, (Object) this.character_choosen_facility);
  }

  private void selectPhaseDefault(BL.PhaseState phase)
  {
    if (this.env.core.isAutoBattle.value)
    {
      this.setCurrent(this.autoBattle);
      if (phase.state != BL.Phase.player && phase.state != BL.Phase.enemy && phase.state != BL.Phase.neutral)
        return;
      this.uiMaskPanel.SetActive(false);
    }
    else
    {
      switch (phase.state)
      {
        case BL.Phase.none:
          break;
        case BL.Phase.player_start:
        case BL.Phase.player:
        case BL.Phase.turn_initialize:
        case BL.Phase.player_start_post:
        case BL.Phase.pvp_move_unit_waiting:
        case BL.Phase.pvp_player_start:
        case BL.Phase.battle_start:
        case BL.Phase.battle_start_init:
          if (this.env.core.unitCurrent.unit == (BL.Unit) null)
          {
            this.setCharacterAct();
            break;
          }
          if (this.env.core.unitCurrent.unit.isFacility)
          {
            this.setCharacterChoosenFacility();
            break;
          }
          if (this.env.core.getForceID(this.env.core.unitCurrent.unit) != BL.ForceID.player)
          {
            this.setCharacterChoosenEnemy();
            break;
          }
          this.setupUICantChangeCurrent(this.env.core.currentUnitPosition.cantChangeCurrent);
          this.setCharacterChoosen();
          break;
        case BL.Phase.enemy_start:
        case BL.Phase.enemy:
          if (this.env.core.unitCurrent.unit != (BL.Unit) null && this.env.core.unitCurrent.unit.isFacility)
          {
            this.setCharacterChoosenFacility();
            break;
          }
          this.setCharacterChoosenEnemy();
          break;
        case BL.Phase.player_end:
          this.enemyStatus.resetCharacterStatusMenu();
          if (this.env.core.unitCurrent.unit == (BL.Unit) null)
          {
            this.setCharacterAct();
            break;
          }
          this.setCharacterChoosen();
          break;
        case BL.Phase.finalize:
        case BL.Phase.suspend:
        case BL.Phase.all_dead_enemy:
        case BL.Phase.stageclear_pre:
        case BL.Phase.stageclear:
        case BL.Phase.gameover:
        case BL.Phase.surrender:
          this.uiMaskPanel.SetActive(true);
          break;
        case BL.Phase.all_dead_player:
          break;
        case BL.Phase.all_dead_neutral:
          break;
        case BL.Phase.pvp_disposition:
          if (this.env.core.unitCurrent.unit == (BL.Unit) null)
          {
            this.setPvpReady();
            break;
          }
          if (this.env.core.unitCurrent.unit.isFacility)
          {
            ((Component) ((Component) this.btnDecide).transform.parent).gameObject.SetActive(false);
            this.setCharacterChoosenFacility();
            break;
          }
          if (this.env.core.getForceID(this.env.core.unitCurrent.unit) != BL.ForceID.player)
          {
            ((Component) ((Component) this.btnDecide).transform.parent).gameObject.SetActive(false);
            this.setCharacterChoosenEnemy();
            break;
          }
          ((Component) ((Component) this.btnDecide).transform.parent).gameObject.SetActive(true);
          this.setCharacterChoosen();
          break;
        case BL.Phase.pvp_wait_preparing:
          if (this.env.core.unitCurrent.unit == (BL.Unit) null)
            this.setPvpReady();
          else
            this.setCharacterChoosen();
          if (Object.op_Inequality((Object) this.btnReady, (Object) null))
          {
            ((UIButtonColor) this.btnReady).isEnabled = false;
            ((Component) this.btnReady).gameObject.SetActive(false);
          }
          if (Object.op_Inequality((Object) this.btnDecide, (Object) null))
            ((UIButtonColor) this.btnDecide).isEnabled = false;
          if (!Object.op_Inequality((Object) this.pvp_ready_title, (Object) null))
            break;
          this.pvp_ready_title.SetActive(false);
          break;
        default:
          if (this.env.core.unitCurrent.unit == (BL.Unit) null)
          {
            this.setCharacterChoosen();
            break;
          }
          if (this.env.core.unitCurrent.unit.isFacility)
          {
            this.setCharacterChoosenFacility();
            break;
          }
          this.setCharacterChoosenEnemy();
          break;
      }
    }
  }

  private Vector3 createTargetVector(BL.DropData drop)
  {
    Transform labelTransform = ((Component) this.grandstatus_right).GetComponent<Battle01GrandStatusRight>().getLabelTransform(drop);
    return Object.op_Equality((Object) labelTransform, (Object) null) ? Vector3.zero : Singleton<CommonRoot>.GetInstance().getCamera().WorldToScreenPoint(labelTransform.position);
  }

  private void deadComplete(
    BL.Unit u,
    bool isKilledByPanelLandformEffect = false,
    bool dontDeadReserveToPoint = false)
  {
    u.setIsDead(true, this.env.core, this.env.core.phaseState.absoluteTurnCount, deadToImmediateRebirth: dontDeadReserveToPoint);
    this.env.core.getUnitPosition(u).completeActionUnit(this.env.core, true, isKilledByPanelLandformEffect);
    if (this.battleManager.useGameEngine && !dontDeadReserveToPoint)
      this.battleManager.gameEngine.deadReserveToPoint(u.playerUnit.is_enemy, !u.isFacility);
    BL.ForceID[] forceIdArray = new BL.ForceID[3]
    {
      BL.ForceID.player,
      BL.ForceID.enemy,
      BL.ForceID.neutral
    };
    foreach (BL.ForceID forceId in forceIdArray)
    {
      foreach (BL.Unit unit in this.env.core.forceUnits(forceId).value)
      {
        if (unit.isEnable && !unit.isDead && BattleFuncs.hasEnabledDeadCountEffects((BL.ISkillEffectListUnit) unit, (BL.ISkillEffectListUnit) u))
          unit.skillEffects.commit();
      }
    }
    if (!this.env.core.currentUnitPosition.cantChangeCurrent)
      return;
    this.env.core.unitCurrent.commit();
  }

  public Battle01SelectNode.MaskContinuer setMaskActive(
    bool v,
    Battle01SelectNode.MaskContinuer mc,
    bool forcibly = false)
  {
    if (mc == null)
      mc = new Battle01SelectNode.MaskContinuer();
    this.btm.setScheduleAction((Action) (() =>
    {
      CommonRoot instance = Singleton<CommonRoot>.GetInstance();
      if (forcibly)
      {
        instance.isActive3DUIMask = mc.backup3DUIMask = v;
        this.uiMaskPanel.SetActive(mc.backupUIMask = !this.env.core.isAutoBattle.value & v);
      }
      else if (v)
      {
        mc.backup3DUIMask = instance.isActive3DUIMask;
        mc.backupUIMask = this.uiMaskPanel.activeSelf;
        instance.isActive3DUIMask = v;
        this.uiMaskPanel.SetActive(v);
      }
      else
      {
        instance.isActive3DUIMask = mc.backup3DUIMask;
        this.uiMaskPanel.SetActive(mc.backupUIMask);
      }
      this.IsForceAutoDisable = this.phaseStateModified.value.state == BL.Phase.player && this.uiMaskPanel.activeSelf;
    }));
    return mc;
  }

  private void doEnemyDead(BL.Unit u, bool dontBackCamera)
  {
    BL.Panel fieldPanel = this.env.core.getFieldPanel(this.env.core.getUnitPosition(u));
    BE.PanelResource panelR = this.env.panelResource[fieldPanel];
    GameObject coin = (GameObject) null;
    Battle01SelectNode.MaskContinuer mc = this.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
    if (u.dropMoney > 0)
    {
      this.btm.setScheduleAction((Action) (() =>
      {
        coin = this.coinPrefab.Clone(panelR.gameObject.transform);
        coin.transform.localPosition = panelR.gameObject.GetComponent<BattlePanelParts>().getLocalPosition();
      }), 1.5f, (Action) (() => Object.Destroy((Object) coin)), isInsertMode: true);
      this.btm.setScheduleAction((Action) (() => this.env.core.dropMoney.value += (long) u.dropMoney));
    }
    int num = !u.hasDrop ? 0 : (!u.drop.isCompleted ? 1 : 0);
    BL.Unit cunit = this.env.core.unitCurrent.unit;
    if (num != 0)
      this.tbManager.execute(u.drop, fieldPanel, this.createTargetVector(u.drop), (Action<BL.DropData>) (drop => this.tipevent.open(drop, cunit)), (Action<BL.DropData>) (drop =>
      {
        drop.execute(cunit, this.env.core);
        this.tipevent.dismiss();
        this.deadComplete(u);
      }), 2f);
    else
      this.btm.setScheduleAction((Action) (() => this.deadComplete(u)), isInsertMode: true);
    this.setMaskActive(false, mc);
    List<BL.Story> sdl = this.env.core.getStoryDefeat(u.specificId);
    if (sdl != null && sdl.Count > 0)
      this.btm.setScheduleAction((Action) (() => sdl.ForEach((Action<BL.Story>) (story => this.battleManager.startStory(story)))), isInsertMode: true);
    if (dontBackCamera || !(this.env.core.unitCurrent.unit != (BL.Unit) null) || this.env.core.unitCurrent.unit.isDead || this.env.core.isAutoBattle.value || !this.env.core.unitCurrent.unit.isPlayerControl || !this.env.core.currentUnitPosition.cantChangeCurrent || this.env.core.currentUnitPosition.isCompleted || this.env.core.unitCurrent.unit.skillEffects.IsMoveSkillActionWaiting())
      return;
    this.btm.setTargetUnit(this.env.core.currentUnitPosition, 0.0f, isWaitCameraMove: true);
  }

  public bool hpCheckWithDeadEffects(
    BL.Unit unit,
    bool isKilledByPanelLandformEffect = false,
    bool dontBackCamera = false)
  {
    if (unit.isDead || unit.hp > 0)
      return false;
    BE.UnitResource unitR = this.env.unitResource[unit];
    this.btm.setTargetUnit(this.env.core.getUnitPosition(unit), 0.1f);
    if (unit.IsJumping)
    {
      this.btm.setScheduleAction((Action) (() =>
      {
        unitR.unitParts_.jumpMiss();
        BL.UnitPosition unitPosition = this.env.core.getUnitPosition(unit);
        BattleFuncs.getPanel(unitPosition.row, unitPosition.column).isJumping = false;
      }));
      this.btm.setEnableWait(new Func<bool>(unitR.unitParts_.checkEffectCompleted));
    }
    BL.SkillEffect immediateRebirthEffect = BattleFuncs.getImmediateRebirthEffects((BL.ISkillEffectListUnit) unit).FirstOrDefault<BL.SkillEffect>();
    bool existImmediateRebirth = immediateRebirthEffect != null;
    BattleUnitParts up = unitR.gameObject.GetComponent<BattleUnitParts>();
    if (Object.op_Inequality((Object) up, (Object) null))
    {
      Battle01SelectNode.MaskContinuer mc = this.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
      this.btm.setScheduleAction((Action) (() => up.dead(existImmediateRebirth)), 1f);
      this.setMaskActive(false, mc);
    }
    if (existImmediateRebirth)
    {
      this.deadComplete(unit, dontDeadReserveToPoint: true);
      BattleFuncs.useImmediateRebirthEffect((BL.ISkillEffectListUnit) unit, immediateRebirthEffect);
      bool forceResetCompleted = immediateRebirthEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.is_reset_completed) != 0;
      unit.rebirth(this.env.core, false, false, forceResetCompleted);
      int key = immediateRebirthEffect.effect.GetInt(BattleskillEffectLogicArgumentEnum.skill_id);
      if (key != 0 && MasterData.BattleskillSkill.ContainsKey(key))
      {
        BattleFuncs.ApplyChangeSkillEffectsOne changeSkillEffectsOne = new BattleFuncs.ApplyChangeSkillEffectsOne((BL.ISkillEffectListUnit) unit);
        changeSkillEffectsOne.doBefore();
        BattleskillSkill skill = MasterData.BattleskillSkill[key];
        foreach (BattleskillEffect effect in skill.Effects)
          unit.skillEffects.Add(BL.SkillEffect.FromMasterData(effect, skill, 1, investUnit: unit, investTurn: this.env.core.phaseState.absoluteTurnCount), checkEnableUnit: (BL.ISkillEffectListUnit) unit);
        changeSkillEffectsOne.doAfter();
        unit.commit();
      }
      if (Object.op_Inequality((Object) up, (Object) null))
      {
        up.SetEffectMode(true);
        Battle01SelectNode.MaskContinuer mc = this.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
        this.btm.setScheduleAction((Action) null, comleteCheckFunc: (Func<bool>) (() => !up.isActive));
        BattleskillEffect effect = immediateRebirthEffect.effect;
        BattleskillFieldEffect passiveEffect = effect.skill.passive_effect;
        GameObject gameObject = (GameObject) null;
        if (passiveEffect != null)
          gameObject = this.env.skillResource[effect.skill.passive_effect.ID].targetEffectPrefab;
        BattleEffects battleEffects = this.battleManager.battleEffects;
        BattleskillFieldEffect fe = passiveEffect;
        List<BL.Unit> targets = new List<BL.Unit>();
        targets.Add(unit);
        GameObject targetEffectPrefab = gameObject;
        Action<BL.Unit> targetEndAction = (Action<BL.Unit>) (u =>
        {
          up.rebirth();
          up.dispHpNumber(0, unit.hp);
          up.setHpGauge(0, unit.hp);
          up.SetEffectMode(false);
        });
        battleEffects.skillFieldEffectStartCore(fe, (BL.Unit) null, targets, (GameObject) null, (GameObject) null, targetEffectPrefab, (Action) null, (Action) null, (List<BL.Unit>) null, targetEndAction);
        this.btm.setScheduleAction((Action) null, 1f);
        this.setMaskActive(false, mc);
      }
      return true;
    }
    BL.ForceID forceId = this.env.core.getForceID(unit);
    if (this.battleManager.isOvo)
    {
      if (unit.isFacility)
      {
        this.deadComplete(unit);
      }
      else
      {
        if (unit.is_leader)
          this.OnTriggerGaugeLeaderEffect(unit);
        this.battleManager.battleEffects.startEffect("Retreat", this.pvp_dead_effect_time, (Action) (() => this.deadComplete(unit)), popupPrefab: this.GetOvoUnitDeadPopup(unit), alert: true, cloneAction: (Action<GameObject>) (po => po.GetComponentInChildren<Battle01712aMenu>().setUnit(unit)));
      }
    }
    else if (forceId == BL.ForceID.player)
    {
      if (unit.isFacility)
        this.deadComplete(unit);
      else
        this.battleManager.battleEffects.startEffect("Retreat", this.dead_effect_time, (Action) (() => this.deadComplete(unit, isKilledByPanelLandformEffect)), popupPrefab: this.popupUnitDeadPlayerPrefab, alert: true, cloneAction: (Action<GameObject>) (po => po.GetComponentInChildren<Battle01712aMenu>().setUnit(unit)), isButtonNoneState: true);
    }
    else
      this.btm.setScheduleAction((Action) (() => this.doEnemyDead(unit, dontBackCamera)), isInsertMode: true);
    if (!Persist.tutorial.Data.IsFinishTutorial())
    {
      TutorialRoot tr = Singleton<TutorialRoot>.GetInstance();
      if (!tr.isTutorialBattleFirstDead)
      {
        this.btm.setScheduleAction((Action) null, 2f);
        this.btm.setScheduleAction((Action) (() =>
        {
          tr.showAdvice(Consts.GetInstance().integralnoah_battle_tutorial2);
          tr.isTutorialBattleFirstDead = true;
          this.btm.setEnableWait(new Func<bool>(tr.checkAdviceCompleted));
        }));
      }
      else if (tr.isTutorialBattleFirstSpawn && !tr.isTutorialBattleSpawnDead)
      {
        this.btm.setScheduleAction((Action) null, 2f);
        this.btm.setScheduleAction((Action) (() =>
        {
          tr.showAdvice(Consts.GetInstance().integralnoah_battle_tutorial4);
          tr.isTutorialBattleSpawnDead = true;
          this.btm.setEnableWait(new Func<bool>(tr.checkAdviceCompleted));
        }));
      }
    }
    return true;
  }

  private GameObject GetOvoUnitDeadPopup(BL.Unit unit)
  {
    return unit.is_leader ? (this.env.core.getForceID(unit) != BL.ForceID.player ? this.popupLeaderUnitDeadEnemyPrefab : this.popupLeaderUnitDeadPlayerPrefab) : (this.env.core.getForceID(unit) != BL.ForceID.player ? this.popupUnitDeadEnemyPrefab : this.popupUnitDeadPlayerPrefab);
  }

  private void OnTriggerGaugeLeaderEffect(BL.Unit unit)
  {
    if (Object.op_Equality((Object) this.pvpHeader, (Object) null))
      return;
    if (this.env.core.getForceID(unit) == BL.ForceID.player)
      this.pvpHeader.PlayerGauge.OnTriggerLeaderEffect();
    else
      this.pvpHeader.EnemyGauge.OnTriggerLeaderEffect();
  }

  private void panelEventCheckWithEffects(BL.UnitPosition up)
  {
    if (!up.isCompleted || up.unit.isDead)
      return;
    BL.Panel panel = this.env.core.getFieldPanel(up);
    if (!panel.hasEvent)
      return;
    Battle01SelectNode.MaskContinuer mc = this.setMaskActive(true, (Battle01SelectNode.MaskContinuer) null);
    this.btm.setTargetPanel(panel, 0.0f, (Action) (() => { }), isWaitCameraMove: true);
    this.tbManager.execute(panel.fieldEvent, panel, this.createTargetVector(panel.fieldEvent), (Action<BL.DropData>) (drop => this.tipevent.open(drop, up.unit)), (Action<BL.DropData>) (drop =>
    {
      panel.executeEvent(up.unit, this.env.core);
      this.tipevent.dismiss();
      this.setMaskActive(false, mc);
    }), 2f);
  }

  protected override void Update_Battle()
  {
    if (!this.battleManager.isBattleEnable)
    {
      if (!Object.op_Equality((Object) this.current, (Object) this.skill_select))
        return;
      this.setCurrent(this.character_choosen);
    }
    else
    {
      bool flag1 = false;
      bool flag2 = false;
      foreach (BL.BattleModified<BL.Unit> unitModified in this.unitModifiedList)
      {
        if (unitModified.isChangedOnce())
        {
          BL.Unit unit = unitModified.value;
          if (!unit.isDead && unit.hp <= 0)
          {
            flag2 = true;
            this.btm.setScheduleAction((Action) (() =>
            {
              if (this.hpCheckWithDeadEffects(unit, unit.deadByItem, true))
                this.battleManager.isDeadEffectPlaying = true;
              unit.deadByItem = false;
            }), isInsertMode: true);
          }
          flag1 = true;
        }
      }
      if (flag2)
        this.btm.setScheduleAction((Action) (() =>
        {
          if (!this.battleManager.isDeadEffectPlaying)
            return;
          this.battleManager.isDeadEffectPlaying = false;
          if (!(this.env.core.unitCurrent.unit != (BL.Unit) null) || this.env.core.unitCurrent.unit.isDead || this.env.core.isAutoBattle.value || !this.env.core.unitCurrent.unit.isPlayerControl || !this.env.core.currentUnitPosition.cantChangeCurrent || this.env.core.currentUnitPosition.isCompleted || this.env.core.unitCurrent.unit.skillEffects.IsMoveSkillActionWaiting())
            return;
          this.btm.setTargetUnit(this.env.core.currentUnitPosition, 0.0f, isWaitCameraMove: true);
        }), isInsertMode: true);
      BL.UnitPosition cup = this.env.core.currentUnitPosition;
      if (flag1 && cup.cantChangeCurrent && cup.isActionComleted)
        this.btm.setScheduleAction((Action) (() =>
        {
          if (cup.movePanels.Count != 1)
            return;
          this.env.core.setSomeAction();
          cup.completeActionUnit(this.env.core);
        }));
      bool flag3 = false;
      BL.UnitPosition unitPosition1 = (BL.UnitPosition) null;
      foreach (BL.BattleModified<BL.UnitPosition> positionModified in this.playerUnitPositionModifiedList)
      {
        BL.UnitPosition up = positionModified.value;
        if (positionModified.isChangedOnce())
        {
          if (this.env.core.unitCurrent.unit == up.unit)
          {
            flag3 = true;
            unitPosition1 = up;
          }
          this.btm.setScheduleAction((Action) (() => this.panelEventCheckWithEffects(up)), isInsertMode: true);
        }
      }
      if (flag3 && unitPosition1 != null)
        this.setupUICantChangeCurrent(unitPosition1.cantChangeCurrent);
      if (this.currentUnitModified.isChangedOnce())
      {
        if (this.currentUnitModified.value.unit != (BL.Unit) null)
        {
          BL.UnitPosition unitPosition2 = this.env.core.getUnitPosition(this.currentUnitModified.value.unit);
          if (unitPosition2 != null)
            this.setupUICantChangeCurrent(unitPosition2.cantChangeCurrent);
        }
        else
          this.setupUICantChangeCurrent(false);
        if (this.stack.Count > 0)
          this.backToTop();
        else
          this.selectPhaseDefault(this.env.core.phaseState);
      }
      if (this.phaseStateModified.isChangedOnce())
      {
        BL.PhaseState phase = this.phaseStateModified.value;
        if (this.battleManager.isOvo)
        {
          this.setMaskActive(phase.state != BL.Phase.player && phase.state != BL.Phase.enemy && phase.state != BL.Phase.pvp_disposition && phase.state != BL.Phase.pvp_wait_preparing && phase.state != BL.Phase.pvp_start_init, (Battle01SelectNode.MaskContinuer) null, true);
          if (phase.state == BL.Phase.pvp_start_init)
          {
            if (Object.op_Inequality((Object) this.btnDecide, (Object) null))
              ((UIButtonColor) this.btnDecide).isEnabled = true;
            if (Object.op_Inequality((Object) this.uiMaskPanelReady, (Object) null))
              this.uiMaskPanelReady.SetActive(true);
          }
          else if (Object.op_Inequality((Object) this.uiMaskPanelReady, (Object) null))
            this.uiMaskPanelReady.SetActive(false);
        }
        else
          this.setMaskActive(phase.state != BL.Phase.player, (Battle01SelectNode.MaskContinuer) null, true);
        if (this.nodeDefaultp())
          this.selectPhaseDefault(phase);
      }
      if (Object.op_Inequality((Object) this.shortCutConfig, (Object) null))
      {
        if (this.shortCutConfig.requestAutoBattleModified_.isChangedOnce())
        {
          bool flag4 = this.shortCutConfig.requestAutoBattleModified_.value.value;
          if (this.env.core.isAutoBattle.value != flag4)
          {
            switch (this.env.core.phaseState.state)
            {
              case BL.Phase.gameover:
              case BL.Phase.surrender:
                this.shortCutConfig.requestAutoBattleModified_.notifyChanged();
                break;
              default:
                this.env.core.isAutoBattle.value = flag4;
                break;
            }
          }
        }
        if (this.shortCutConfig.requestSimpleBattleModified_.isChangedOnce())
        {
          bool flag5 = this.shortCutConfig.requestSimpleBattleModified_.value.value;
          if (this.battleManager.noDuelScene != flag5)
            this.battleManager.noDuelScene = flag5;
        }
      }
      if (this.isAutoBattleModified.isChangedOnce())
      {
        this.selectPhaseDefault(this.env.core.phaseState);
        this.uiMaskPanel.SetActive(this.isAutoBattleModified.value.value);
      }
      if (this.itemModified.isChangedOnce() && this.item_button.isActive)
        this.item_button.isActive = this.itemActivep();
      if (this.playerCallSkillStateModified.isChangedOnce() && Object.op_Inequality((Object) this.dirCallSkill, (Object) null))
        this.setCallSkillButton();
      if (this.enemyCallSkillStateModified.isChangedOnce() && Object.op_Inequality((Object) this.dirCallSkillEnemy, (Object) null))
        this.setEnemyCallSkillButton();
      if (this.battleManager.isPvnpc)
        this.battleManager.pvnpcManager.execNextState((BattleMonoBehaviour) this);
      else if (this.battleManager.isGvg)
        this.battleManager.gvgManager.execNextState((BattleMonoBehaviour) this);
      if (this.env.core.phaseState.state == BL.Phase.player_start && this.triggerPlayerStartTurn != this.env.core.phaseState.turnCount)
      {
        this.triggerPlayerStartTurn = this.env.core.phaseState.turnCount;
        this.updateSaveConfig();
      }
      else
      {
        switch (this.env.core.phaseState.state)
        {
          case BL.Phase.stageclear:
          case BL.Phase.gameover:
          case BL.Phase.surrender:
            this.updateSaveConfig();
            break;
        }
      }
    }
  }

  private void setCharacterAct()
  {
    if (this.battleManager.isOvo && !this.battleManager.gameEngine.isDisposition && Object.op_Inequality((Object) ((Component) this).GetComponent<Battle01PVPNode>(), (Object) null))
      this.setPvpReady();
    else
      this.setCurrent(this.character_Act);
  }

  private void setPvpReady() => this.setCurrent(this.pvp_ready);

  private void setEnemyUnit(BL.Unit unit)
  {
    if (Object.op_Equality((Object) this.enemyStatus, (Object) null))
      this.enemyStatus = ((Component) this.character_choosen_enemy).gameObject.GetComponentsInChildren<Battle01UIPlayerStatus>(true)[0];
    this.enemyStatus.setUnit(unit);
  }

  private void setFacilityUnit(BL.Unit unit)
  {
    if (Object.op_Equality((Object) this.facilityStatus, (Object) null))
      this.facilityStatus = ((Component) this.character_choosen_facility).gameObject.GetComponentsInChildren<Battle01UIFacilityStatus>(true)[0];
    this.facilityStatus.setUnit(unit);
  }

  private void setCharacterChoosenEnemy()
  {
    if (this.env.core.unitCurrent.unit != (BL.Unit) null && (this.battleManager.isOvo || this.env.core.getForceID(this.env.core.unitCurrent.unit) != BL.ForceID.player))
      this.setEnemyUnit(this.env.core.unitCurrent.unit);
    this.setCurrent(this.character_choosen_enemy);
  }

  private void setCharacterChoosenFacility()
  {
    if (this.env.core.unitCurrent.unit != (BL.Unit) null && this.env.core.unitCurrent.unit.isFacility)
      this.setFacilityUnit(this.env.core.unitCurrent.unit);
    this.setCurrent(this.character_choosen_facility);
  }

  private void setCharacterChoosen()
  {
    this.resetScrollPosition(this.character_choosen, this.env.core.unitCurrent.unit);
    this.setCurrent(this.character_choosen);
  }

  public void useOugi(BL.Unit unit, BL.Skill ougi) => this.useSkillSubject(unit, ougi);

  public void useSkill()
  {
    if (!this.phasePlayerp())
      return;
    Battle01SkillSelect componentInChildren = ((Component) this).GetComponentInChildren<Battle01SkillSelect>(true);
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    this.command_skill_name_sprite.SetActive(true);
    this.call_skill_name_sprite.SetActive(false);
    List<BL.Skill> list = this.env.core.getFieldSkills(this.env.core.unitCurrent.unit).OrderByDescending<BL.Skill, int>((Func<BL.Skill, int>) (x => !x.remain.HasValue ? 0 : x.remain.Value)).ThenByDescending<BL.Skill, int>((Func<BL.Skill, int>) (x => !x.skill.IsJobAbility ? 0 : 1)).ThenBy<BL.Skill, int>((Func<BL.Skill, int>) (x => x.skill.ID)).ToList<BL.Skill>();
    componentInChildren.setList(list);
    this.setCurrent(this.skill_select, true);
  }

  public void useSEA(BL.Unit unit, BL.Skill skill) => this.useSkillSubject(unit, skill);

  public void useSkillUse(
    BL.Skill skill,
    List<BL.Unit> targets,
    List<BL.Panel> panels,
    bool isSelectPanel)
  {
    if (!this.phasePlayerp())
      return;
    Battle01SkillUse componentInChildren = ((Component) this).GetComponentInChildren<Battle01SkillUse>(true);
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.setSkillTargets(this.env.core.unitCurrent.unit, skill, targets, panels, isSelectPanel);
    if (this.env.core.isSkillUseConfirmation.value || !componentInChildren.CanUseSkill())
      this.setCurrent(this.skill_use, true);
    else
      componentInChildren.SkillUse.onClick();
  }

  public void useSkillSubject(BL.Unit unit, BL.Skill skill)
  {
    if (!this.phasePlayerp())
      return;
    if (skill.skill.skill_type == BattleskillSkillType.call)
    {
      List<BL.Unit> targets = !this.env.core.playerCallSkillState.isCanUseCallSkill ? new List<BL.Unit>() : this.env.core.getCallSkillTargetUnits(skill).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToList<BL.Unit>();
      Battle01SkillUse componentInChildren = ((Component) this).GetComponentInChildren<Battle01SkillUse>(true);
      if (Object.op_Equality((Object) componentInChildren, (Object) null))
        return;
      componentInChildren.setCallSkillTargets(skill, targets);
      this.setCurrent(this.skill_use, true);
    }
    else
    {
      List<BL.Unit> provokeUnits = BattleFuncs.getProvokeUnits((BL.ISkillEffectListUnit) unit);
      List<BL.Unit> unitList;
      List<BL.Panel> panels;
      bool flag;
      bool isSelectPanel;
      if (skill.targetType != BattleskillTargetType.panel_single)
      {
        unitList = this.env.core.getSkillTargetUnits(this.env.core.currentUnitPosition, skill).Select<BL.UnitPosition, BL.Unit>((Func<BL.UnitPosition, BL.Unit>) (x => x.unit)).ToList<BL.Unit>();
        panels = new List<BL.Panel>();
        flag = skill.isNonSelect;
        if (provokeUnits != null)
        {
          List<BL.Unit> list = provokeUnits.Intersect<BL.Unit>((IEnumerable<BL.Unit>) unitList).ToList<BL.Unit>();
          if (flag)
          {
            if (!list.Any<BL.Unit>((Func<BL.Unit, bool>) (x => x.skillEffects.CanUseSkill(skill.skill, skill.level, (BL.ISkillEffectListUnit) x, this.env.core, (BL.ISkillEffectListUnit) unit, skill.nowUseCount) == 0)))
              unitList = list;
          }
          else
            unitList = list;
        }
        isSelectPanel = false;
      }
      else
      {
        unitList = new List<BL.Unit>();
        panels = provokeUnits != null ? new List<BL.Panel>() : BattleFuncs.getRangePanels(this.env.core.currentUnitPosition.row, this.env.core.currentUnitPosition.column, skill.range);
        flag = false;
        isSelectPanel = true;
      }
      if (flag)
      {
        this.useSkillUse(skill, unitList, panels, isSelectPanel);
      }
      else
      {
        Battle01SkillSubject componentInChildren = ((Component) this).GetComponentInChildren<Battle01SkillSubject>(true);
        if (Object.op_Equality((Object) componentInChildren, (Object) null))
          return;
        componentInChildren.setSkillTargets(unit, skill, unitList, panels, isSelectPanel);
        this.setCurrent(this.skill_subject, true);
      }
    }
  }

  public void useItem()
  {
    if (!this.phasePlayerp())
      return;
    this.setCurrent(this.item_select, true);
  }

  public IEnumerator dispCallSkillChargedOwn()
  {
    Battle01SelectNode battle01SelectNode = this;
    battle01SelectNode.env.core.playerCallSkillState.isChargedCallGauge = true;
    if (Object.op_Equality((Object) battle01SelectNode.prefabCallOwn, (Object) null))
    {
      Future<GameObject> f = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/battle/dir_CallSkillExplanation_Display_Own");
      IEnumerator e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battle01SelectNode.prefabCallOwn = f.Result;
      f = (Future<GameObject>) null;
    }
    GameObject obj = battle01SelectNode.prefabCallOwn.Clone(battle01SelectNode.dirCallSkillCharged.transform);
    TweenAlpha tween = obj.GetComponent<TweenAlpha>();
    ((UITweener) tween).PlayForward();
    yield return (object) new WaitForSeconds(1.9f);
    ((UITweener) tween).PlayReverse();
    yield return (object) new WaitForSeconds(1f);
    Object.Destroy((Object) obj);
  }

  public IEnumerator dispCallSkillChargedEnemy(bool offset)
  {
    Battle01SelectNode battle01SelectNode = this;
    if (!battle01SelectNode.battleManager.isGvg)
    {
      battle01SelectNode.env.core.enemyCallSkillState.isChargedCallGauge = true;
      if (Object.op_Equality((Object) battle01SelectNode.prefabCallEnemy, (Object) null))
      {
        Future<GameObject> f = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/battle/dir_CallSkillExplanation_Display_Enemy");
        IEnumerator e = f.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        battle01SelectNode.prefabCallEnemy = f.Result;
        f = (Future<GameObject>) null;
      }
      GameObject obj = battle01SelectNode.prefabCallEnemy.Clone(battle01SelectNode.dirCallSkillCharged.transform);
      if (!offset)
      {
        TweenPosition component = obj.GetComponent<TweenPosition>();
        component.to.y = component.from.y;
      }
      TweenAlpha tween = obj.GetComponent<TweenAlpha>();
      ((UITweener) tween).PlayForward();
      yield return (object) new WaitForSeconds(1.9f);
      ((UITweener) tween).PlayReverse();
      yield return (object) new WaitForSeconds(1f);
      Object.Destroy((Object) obj);
    }
  }

  public void useItemSubject(BL.Item item)
  {
    if (!this.phasePlayerp())
      return;
    Battle01ItemSubject[] componentsInChildren = ((Component) this).GetComponentsInChildren<Battle01ItemSubject>(true);
    if (componentsInChildren.Length == 0)
      return;
    List<BL.Unit> itemTargetUnits = this.env.core.getItemTargetUnits(item);
    componentsInChildren[0].setItemTargets(item, itemTargetUnits);
    this.setCurrent(this.item_subject, true);
  }

  public void useCallSkill()
  {
    if (!this.phasePlayerp())
      return;
    Battle01SkillSelect componentInChildren = ((Component) this).GetComponentInChildren<Battle01SkillSelect>(true);
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    this.command_skill_name_sprite.SetActive(false);
    this.call_skill_name_sprite.SetActive(true);
    List<BL.Skill> list = new List<BL.Skill>();
    list.Add(new BL.Skill()
    {
      id = this.env.core.playerCallSkillState.skillId,
      remain = new int?(this.env.core.playerCallSkillState.isUsedCallSkill ? 0 : 1)
    });
    if (list.Count == 0)
      return;
    componentInChildren.setList(list);
    this.setCurrent(this.skill_select, true);
  }

  public void backToTop()
  {
    this.stack.Clear();
    this.inputObserver.cancelTargetSelect();
    this.selectPhaseDefault(this.env.core.phaseState);
  }

  public void doDelayBacktoTopForCallSkill()
  {
    this.StartCoroutine(this.delayBacktoTopForCallSkill());
  }

  public IEnumerator delayBacktoTopForCallSkill()
  {
    Battle01SelectNode battle01SelectNode = this;
    if (!battle01SelectNode.env.core.isAutoBattle.value)
    {
      while (battle01SelectNode.SkillNotice.isView)
        yield return (object) null;
      battle01SelectNode.backToTop();
    }
  }

  public bool checkSkillNoticeCompleted()
  {
    return Object.op_Equality((Object) this.SkillNotice, (Object) null) || !this.SkillNotice.isView;
  }

  public void disableBackButton()
  {
    this.back_up.isActive = false;
    this.back.isActive = false;
  }

  public void onBack() => this._onBack();

  public void onBackWithWait(float wait) => this._onBack(wait);

  private void _onBack(float wait = 0.1f)
  {
    if (Object.op_Equality((Object) this.current, (Object) this.skill_subject) || Object.op_Equality((Object) this.current, (Object) this.item_subject) || Object.op_Equality((Object) this.current, (Object) this.skill_use))
      this.inputObserver.cancelTargetSelect();
    if (this.stack.Count > 0)
    {
      this.setCurrent((NGTweenParts) null, popStack: true);
    }
    else
    {
      this.btm.setCurrentUnit((BL.Unit) null, wait);
      this.selectPhaseDefault(this.env.core.phaseState);
    }
  }

  public bool canOpenMenu()
  {
    return !Object.op_Equality((Object) this.current, (Object) null) && Object.op_Equality((Object) this.current, (Object) this.character_Act);
  }

  private void setScrollSeEnable(NGTweenParts parts, bool enable)
  {
    foreach (NGHorizontalScrollParts componentsInChild in ((Component) parts).GetComponentsInChildren<NGHorizontalScrollParts>(true))
    {
      if (enable)
        this.StartCoroutine(this.WaitScrollSe(componentsInChild));
      else
        componentsInChild.SeEnable = false;
    }
  }

  private IEnumerator WaitScrollSe(NGHorizontalScrollParts obj)
  {
    yield return (object) new WaitForSeconds(1f);
    obj.SeEnable = true;
  }

  private void setupUICantChangeCurrent(bool cantchange)
  {
    this.setEnableScroll(this.character_choosen, !cantchange);
    this.setEnableBackUp(!cantchange);
    this.setEnableSubMenu(!cantchange);
  }

  private void setEnableScroll(NGTweenParts parts, bool enable)
  {
    NGHorizontalScrollParts componentInChildren = Object.op_Inequality((Object) parts, (Object) null) ? ((Component) parts).GetComponentInChildren<NGHorizontalScrollParts>() : (NGHorizontalScrollParts) null;
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    UIScrollView component = Object.op_Inequality((Object) componentInChildren.scrollView, (Object) null) ? componentInChildren.scrollView.GetComponent<UIScrollView>() : (UIScrollView) null;
    if (Object.op_Inequality((Object) component, (Object) null))
      ((Behaviour) component).enabled = enable;
    if (Object.op_Inequality((Object) componentInChildren.leftArrow, (Object) null))
      componentInChildren.leftArrow.SetActive(enable && componentInChildren.leftArrow.activeSelf);
    if (!Object.op_Inequality((Object) componentInChildren.rightArrow, (Object) null))
      return;
    componentInChildren.rightArrow.SetActive(enable && componentInChildren.rightArrow.activeSelf);
  }

  private void setEnableBackUp(bool enable)
  {
    if (Object.op_Inequality((Object) this.current, (Object) this.character_choosen))
      enable = true;
    UIButton component = Object.op_Inequality((Object) this.back_up, (Object) null) ? ((Component) this.back_up).GetComponent<UIButton>() : (UIButton) null;
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    ((UIButtonColor) component).isEnabled = enable;
  }

  private void setEnableSubMenu(bool enable)
  {
    GameObject btnSubMenu = this.getBtnSubMenu();
    UIButton component = Object.op_Inequality((Object) btnSubMenu, (Object) null) ? btnSubMenu.GetComponent<UIButton>() : (UIButton) null;
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    ((UIButtonColor) component).isEnabled = enable;
  }

  private GameObject getBtnSubMenu()
  {
    if (Object.op_Equality((Object) this.submenu, (Object) null))
      return (GameObject) null;
    Transform transform = ((Component) this.submenu).transform.Find("ibtn_menu");
    return !Object.op_Inequality((Object) transform, (Object) null) ? (GameObject) null : ((Component) transform).gameObject;
  }

  private void resetScrollPosition(NGTweenParts parts, BL.Unit unit)
  {
    if (Object.op_Equality((Object) parts, (Object) null) || unit == (BL.Unit) null)
      return;
    Battle01StatusScrollParts componentInChildren = ((Component) parts).GetComponentInChildren<Battle01StatusScrollParts>();
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    foreach (Battle01UIPlayerStatus allPlayerStatu in componentInChildren.allPlayerStatus)
      allPlayerStatu.blinkResetDirty = true;
    componentInChildren.resetScrollPosition(unit);
  }

  private IEnumerator AutoBattleStartPop()
  {
    Battle01SelectNode battle01SelectNode = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_017_18_16__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01SelectNode.battleManager.popupOpen(prefab.Result);
  }

  private IEnumerator TurnEndPop()
  {
    Battle01SelectNode battle01SelectNode = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_017_18_18__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01SelectNode.battleManager.popupOpen(prefab.Result);
  }

  public void IbtnAutoBattle()
  {
    if (this.IsPushCheck())
      return;
    this.env.core.isAutoBattle.value = true;
  }

  private int NextSpeed()
  {
    if (1 == this.settingSpeed)
      return 2;
    if (2 == this.settingSpeed)
      return 3;
    return 3 == this.settingSpeed ? 4 : 1;
  }

  private void SetSpeed(int speed)
  {
    if (Object.op_Equality((Object) this.spdButton1x, (Object) null) || Object.op_Equality((Object) this.spdButton2x, (Object) null) || Object.op_Equality((Object) this.spdButton3x, (Object) null) || Object.op_Equality((Object) this.spdButton4x, (Object) null))
      return;
    this.settingSpeed = Mathf.Clamp(speed, 1, 4);
    Time.timeScale = (float) this.settingSpeed;
    this.spdButton1x.SetActive(this.settingSpeed == 1);
    this.spdButton2x.SetActive(this.settingSpeed == 2);
    this.spdButton3x.SetActive(this.settingSpeed == 3);
    this.spdButton4x.SetActive(this.settingSpeed == 4);
  }

  public void IbtnSpeedButtonClicked()
  {
    if (this.IsPushCheck())
      return;
    this.SetSpeed(this.NextSpeed());
  }

  public void IbtnTurnEnd()
  {
    if (Object.op_Inequality((Object) this.shortCutConfig, (Object) null) && this.shortCutConfig.requestAutoBattleModified_.value.value || this.IsPushCheck())
      return;
    this.StartCoroutine(this.TurnEndPop());
  }

  public bool IsPush => this.is_push;

  private bool IsPushCheck()
  {
    if (this.battleManager.environment.core.phaseState.state == BL.Phase.gameover || this.battleManager.environment.core.phaseState.state == BL.Phase.surrender || this.is_push)
      return true;
    this.is_push = true;
    this.StartCoroutine(this.pushCancel());
    return false;
  }

  private IEnumerator pushCancel()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.is_push = false;
  }

  public void SavePVPConfig()
  {
    if (!this.battleManager.isPvp && !this.battleManager.isPvnpc)
      return;
    bool modifiedSpeed = Persist.battleTimeSetting.Data.speed != this.settingSpeed;
    if (!modifiedSpeed)
      return;
    this.StartCoroutine(this.doUpdateSaveConfig(false, false, modifiedSpeed));
  }

  private void updateSaveConfig()
  {
    bool modifiedAuto = false;
    if (!this.battleManager.isEarth && !this.battleManager.isOvo)
    {
      if (this.env.core.battleInfo.isAutoBattleEnable)
      {
        try
        {
          modifiedAuto = Persist.autoBattleSetting.Data.isAutoBattle != this.env.core.isAutoBattle.value;
        }
        catch
        {
          Persist.autoBattleSetting.Delete();
          Persist.autoBattleSetting.Data = new Persist.AutoBattleSetting();
          Persist.autoBattleSetting.Data.isItemMove = this.env.core.isAutoItemMove.value;
          modifiedAuto = true;
        }
      }
    }
    bool modifiedNoDuel = false;
    if (!this.battleManager.isPvp)
    {
      if (!this.battleManager.isPvnpc)
      {
        try
        {
          modifiedNoDuel = Persist.battleNoDuel.Data.noDuelScene != this.battleManager.noDuelScene;
        }
        catch
        {
          Persist.battleNoDuel.Delete();
          Persist.battleNoDuel.Data = new Persist.BattleNoDuel();
          modifiedNoDuel = true;
        }
      }
    }
    bool modifiedSpeed = false;
    if (!this.battleManager.isPvp)
    {
      if (!this.battleManager.isPvnpc)
      {
        try
        {
          modifiedSpeed = Persist.battleTimeSetting.Data.speed != this.settingSpeed;
        }
        catch
        {
          Persist.battleTimeSetting.Delete();
          Persist.battleTimeSetting.Data = new Persist.BattleTimeSetting();
          modifiedSpeed = true;
        }
      }
    }
    if (!(modifiedAuto | modifiedNoDuel | modifiedSpeed))
      return;
    this.StartCoroutine(this.doUpdateSaveConfig(modifiedAuto, modifiedNoDuel, modifiedSpeed));
  }

  private IEnumerator doUpdateSaveConfig(
    bool modifiedAuto,
    bool modifiedNoDuel,
    bool modifiedSpeed)
  {
    Battle01SelectNode battle01SelectNode = this;
    bool flag = false;
    if (modifiedAuto)
    {
      Persist.autoBattleSetting.Data.isAutoBattle = battle01SelectNode.env.core.isAutoBattle.value;
      Persist.autoBattleSetting.Flush();
      flag = true;
    }
    if (modifiedNoDuel)
    {
      if (flag)
        yield return (object) null;
      Persist.battleNoDuel.Data.noDuelScene = battle01SelectNode.battleManager.noDuelScene;
      Persist.battleNoDuel.Flush();
      flag = true;
    }
    if (modifiedSpeed)
    {
      if (flag)
        yield return (object) null;
      Persist.battleTimeSetting.Data.speed = battle01SelectNode.settingSpeed;
      Persist.battleTimeSetting.Flush();
    }
  }

  public void setActivePvpExceptionLoadingUI(bool value)
  {
    if (Object.op_Implicit((Object) this.dirPvpExceptionLoading))
      this.dirPvpExceptionLoading.SetActive(value);
    if (!Object.op_Implicit((Object) this.dirPvpExceptionLoadingMask))
      return;
    this.dirPvpExceptionLoadingMask.SetActive(value);
  }

  public void PlaySkillNotice(BL.Skill skill, bool isPlayer = false, float viewWait = 7f)
  {
    if (!Object.op_Inequality((Object) this.SkillNotice, (Object) null))
      return;
    this.SkillNotice.PlayView(skill, isPlayer, viewWait);
  }

  public void doCallSkillCutin(int same_character_id)
  {
    this.StartCoroutine(CallSkillCutin.Show(same_character_id));
  }

  private void setCallSkillButton()
  {
    BL.CallSkillState callSkillState = this.playerCallSkillStateModified.value;
    if (callSkillState.sameCharacterID == 0 || callSkillState.skillId == 0)
    {
      this.dirCallSkill.isActive = false;
      ((Component) this.dirCallSkill).gameObject.SetActive(false);
    }
    else
    {
      if (!this.dirCallSkill.isActive)
        return;
      ((Component) this.dirCallSkill).gameObject.SetActive(true);
      UISprite component = ((Component) this.ibtnCallSkill).gameObject.GetComponent<UISprite>();
      if (callSkillState.isUsedCallSkill)
      {
        ((UIWidget) component).color = Color.gray;
        ((UIButtonColor) this.ibtnCallSkill).defaultColor = ((UIButtonColor) this.ibtnCallSkill).hover = ((UIButtonColor) this.ibtnCallSkill).pressed = ((UIWidget) component).color;
        this.dirCallGauge.SetActive(false);
        this.slcCallSkillEffects.SetActive(false);
      }
      else if (callSkillState.isCallGaugeMax)
      {
        this.dirCallGauge.SetActive(false);
        this.slcCallSkillEffects.SetActive(true);
      }
      else
      {
        this.dirCallGauge.SetActive(true);
        this.slcCallSkillEffects.SetActive(false);
        ((UIWidget) this.slcCallSkillGauge.GetComponent<UI2DSprite>()).material.SetFloat("_xOffset", (float) ((double) (float) (1M - callSkillState.callGaugeRate) * 0.5));
        ((Component) this.dirCallSkill).GetComponent<UIPanel>().Refresh();
      }
    }
  }

  private void setEnemyCallSkillButton()
  {
    BL.CallSkillState callSkillState = this.enemyCallSkillStateModified.value;
    if (callSkillState.sameCharacterID == 0 || callSkillState.skillId == 0 || this.battleManager.isGvg)
      ((Component) this.dirCallSkillEnemy).gameObject.SetActive(false);
    else if (callSkillState.isUsedCallSkill)
    {
      this.dirCallGaugeEnemy.SetActive(false);
      this.slcCallSkillEffectsEnemy.SetActive(false);
    }
    else if (callSkillState.isCallGaugeMax)
    {
      this.dirCallGaugeEnemy.SetActive(false);
      this.slcCallSkillEffectsEnemy.SetActive(true);
    }
    else
    {
      this.dirCallGaugeEnemy.SetActive(true);
      this.slcCallSkillEffectsEnemy.SetActive(false);
      ((UIWidget) this.slcCallSkillGaugeEnemy.GetComponent<UI2DSprite>()).material.SetFloat("_xOffset", (float) ((double) (float) (1M - callSkillState.callGaugeRate) * 0.5));
      ((Component) this.dirCallSkillEnemy).GetComponent<UIPanel>().Refresh();
    }
  }

  public void IbtnEnemyCallSkill()
  {
    if (this.IsPushCheck())
      return;
    this.StartCoroutine(this.OpenCallSkillPopupEnemy());
  }

  private IEnumerator OpenCallSkillPopupEnemy()
  {
    Battle01SelectNode battle01SelectNode = this;
    if (battle01SelectNode.env.core.enemyCallSkillState.sameCharacterID != 0 && battle01SelectNode.env.core.enemyCallSkillState.skillId != 0)
    {
      if (Object.op_Equality((Object) battle01SelectNode.enemyCallSkillDetailPrefab, (Object) null))
      {
        Future<GameObject> prefabF = new ResourceObject("Prefabs/UnitGUIs/Popup_CallSkillDetails_Enemy").Load<GameObject>();
        IEnumerator e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        battle01SelectNode.enemyCallSkillDetailPrefab = prefabF.Result;
        prefabF = (Future<GameObject>) null;
      }
      BL.Skill skill = new BL.Skill();
      skill.id = battle01SelectNode.env.core.enemyCallSkillState.skillId;
      UnitUnit unit = (UnitUnit) null;
      foreach (UnitUnit unitUnit in MasterData.UnitUnitList)
      {
        if (unitUnit.same_character_id == battle01SelectNode.env.core.enemyCallSkillState.sameCharacterID)
        {
          unit = unitUnit;
          break;
        }
      }
      yield return (object) Singleton<PopupManager>.GetInstance().open(battle01SelectNode.enemyCallSkillDetailPrefab).GetComponent<PopupCallSkill>().initialize(unit, skill.skill, true, battle01SelectNode.env.core.enemyCallSkillState.callGaugeRate);
    }
  }

  public void doSEASkillCutin(PlayerUnit unit)
  {
    if (!this.isSEASkillCutinCompleted)
      return;
    this.isSEASkillCutinCompleted = false;
    this.StartCoroutine(this.doSEASkillCutinSync(unit));
  }

  private IEnumerator doSEASkillCutinSync(PlayerUnit unit)
  {
    yield return (object) SEASkillCutin.Show(this.dynBattleSkillSEA, unit, this.settingSpeed);
    this.isSEASkillCutinCompleted = true;
  }

  public bool checkSEASkillCutinCompleted() => this.isSEASkillCutinCompleted;

  private void OnDisable()
  {
  }

  public class MaskContinuer
  {
    public bool backup3DUIMask;
    public bool backupUIMask;
  }
}
