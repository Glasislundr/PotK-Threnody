// Decompiled with JetBrains decompiler
// Type: NGDuelManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class NGDuelManager : MonoBehaviour
{
  private const int PLAYER_UNIT = 0;
  private const int ENEMY_UNIT = 1;
  private const string mStoryScene = "story009_3";
  [SerializeField]
  public Transform mMyFirstPosFar;
  [SerializeField]
  public Transform mEnemyFirstPosFar;
  [SerializeField]
  public Transform mMyFirstPosNear;
  [SerializeField]
  public Transform mEnemyFirstPosNear;
  [SerializeField]
  public UISprite mMyArrowUp;
  [SerializeField]
  public UISprite mMyArrowDown;
  [SerializeField]
  public UISprite mEArrowUp;
  [SerializeField]
  public UISprite mEArrowDown;
  [SerializeField]
  private Battle0181CharacterStatus mPlayerUI;
  [SerializeField]
  private Battle0181CharacterStatus mEnemyUI;
  [SerializeField]
  public Battle0181Menu mParentScene;
  [SerializeField]
  public float mDuelSupportDisplayTime;
  [SerializeField]
  public float mBossIntroDisplayTime;
  [SerializeField]
  public GameObject mRoot3d;
  [SerializeField]
  private DuelCameraManager mCameraManager;
  [SerializeField]
  public float DuelCameraDispTime;
  [SerializeField]
  public float DrawDispTime;
  [SerializeField]
  public float WinDispTime;
  [SerializeField]
  public float LoseDispTime;
  [SerializeField]
  public float RemoteCameraChangeDelay;
  [SerializeField]
  public bool useDistance;
  [SerializeField]
  private Light directionalLight;
  [SerializeField]
  private GameObject mPreloadGO;
  private NGDuelManager.DuelStatus currSts = NGDuelManager.DuelStatus.ST_NONE;
  private GameObject mMapObject;
  private Transform PlayerInitPosition;
  private Transform EnemyInitPosition;
  private List<BL.Story> mDuelStorys;
  private NGDuelUnit mPlayerDUnit;
  private NGDuelUnit mEnemyDUnit;
  private int mDuelTurn;
  private BL.DuelTurn mPreTurn;
  private bool _isWait;
  private bool mInitialized;
  private float wait_time;
  private Color duelAmbientLight = Color.white;
  private int endVoiceChannel = -1;
  private bool isNextFinish;
  private bool isReadyForSkip;
  private bool isSkipped;
  private int beforeBlackBGPanelDepth;
  private BattleMap mMapData;

  [HideInInspector]
  public DuelResult mDuelResult { get; private set; }

  public GameObject mDamagePrefab => Singleton<NGDuelDataManager>.GetInstance().mDamagePrefab;

  public GameObject mCriticalEffect => Singleton<NGDuelDataManager>.GetInstance().mCriticalEffect;

  public GameObject mMissEffect => Singleton<NGDuelDataManager>.GetInstance().mMissEffect;

  public GameObject mShadow => Singleton<NGDuelDataManager>.GetInstance().mShadow;

  public GameObject mDuelSupport => Singleton<NGDuelDataManager>.GetInstance().mDuelSupport;

  public GameObject mCriticalFlash => Singleton<NGDuelDataManager>.GetInstance().mCriticalFlash;

  public GameObject mWeakEffect => Singleton<NGDuelDataManager>.GetInstance().mWeakEffect;

  public GameObject mResistEffect => Singleton<NGDuelDataManager>.GetInstance().mResistEffect;

  public bool isWait
  {
    get => this._isWait;
    set
    {
      if (this._isWait == value)
        return;
      this._isWait = value;
      if (this._isWait)
        return;
      if (Object.op_Inequality((Object) this.mPlayerDUnit, (Object) null))
        this.mPlayerDUnit.TimeReset();
      if (!Object.op_Inequality((Object) this.mEnemyDUnit, (Object) null))
        return;
      this.mEnemyDUnit.TimeReset();
    }
  }

  public int duel_distance => this.mDuelResult.distance;

  public GameObject currentCamera
  {
    get
    {
      return !Object.op_Inequality((Object) this.mCameraManager, (Object) null) ? (GameObject) null : this.mCameraManager.currentCamera;
    }
  }

  public NGDuelUnit playerUnit => this.mPlayerDUnit;

  public NGDuelUnit enemyUnit => this.mEnemyDUnit;

  public NGDuelUnit getNGDuelUnit(int unit_type)
  {
    if (unit_type == 0)
      return this.mPlayerDUnit;
    return unit_type == 1 ? this.mEnemyDUnit : (NGDuelUnit) null;
  }

  public bool isDuelEnd => this.currSts == NGDuelManager.DuelStatus.ST_END;

  public static void actScreen(bool darken = true)
  {
    Singleton<CommonRoot>.GetInstance().isActiveBlackBGPanel = darken;
  }

  public IEnumerator Initialize(DuelResult duelResult, DuelEnvironment duelEnv, bool selfStart = true)
  {
    this.mInitialized = false;
    this.mDuelResult = duelResult;
    this.mDuelStorys = duelEnv.storys;
    NGDuelManager.actScreen();
    yield return (object) null;
    if (this.mDuelResult.distance == 1)
    {
      if (this.useDistance)
      {
        this.PlayerInitPosition = this.mMyFirstPosNear;
        this.EnemyInitPosition = this.mEnemyFirstPosNear;
      }
      else
      {
        this.PlayerInitPosition = this.mMyFirstPosFar;
        this.EnemyInitPosition = this.mEnemyFirstPosFar;
      }
    }
    else
    {
      this.PlayerInitPosition = this.mMyFirstPosFar;
      this.EnemyInitPosition = this.mEnemyFirstPosFar;
    }
    yield return (object) new WaitWhile((Func<bool>) (() => Singleton<NGDuelDataManager>.GetInstance().IsBackgroundPreloading()));
    IEnumerator e = this.preloadCommonDuelEffect();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.makePlayerUnit(duelResult);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.makeEnemyUnit(duelResult);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.mPlayerDUnit.Initialize(this.makeUnitInfo(duelResult, this.mEnemyDUnit, this.mPlayerUI, this.PlayerInitPosition));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.mEnemyDUnit.Initialize(this.makeUnitInfo(duelResult, this.mPlayerDUnit, this.mEnemyUI, this.EnemyInitPosition, false));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.preloadEffect(duelResult);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mPlayerDUnit.SetAilmentEffect();
    this.mEnemyDUnit.SetAilmentEffect();
    if (Object.op_Inequality((Object) null, (Object) this.mCameraManager))
    {
      this.mCameraManager.Initialize(false, false);
      if (duelEnv.stage.IsStage())
        yield return (object) BattleCameraFilter.Create(duelEnv.stage.stage, this.mCameraManager.DuelCamera, true);
    }
    e = this.createMap(duelEnv.stage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.MakeGCR(this.mDuelResult.playerUnit().weapon.gear.kind, this.mDuelResult.enemyUnit().weapon.gear.kind);
    this.currSts = NGDuelManager.DuelStatus.ST_INIT;
    this.mDuelTurn = 0;
    this.mPreTurn = (BL.DuelTurn) null;
    if (selfStart)
      this.StartDuel();
  }

  public void StartDuel()
  {
    this.mInitialized = true;
    this.isReadyForSkip = false;
    this.isSkipped = false;
    this.beforeBlackBGPanelDepth = Singleton<CommonRoot>.GetInstance().getBlackBGPanelDepth();
  }

  private void MakeGCR(GearKind attackerGearKind, GearKind defenderGearKind)
  {
    if (Object.op_Equality((Object) this.mMyArrowUp, (Object) null) || Object.op_Equality((Object) this.mMyArrowDown, (Object) null) || Object.op_Equality((Object) this.mEArrowUp, (Object) null) || Object.op_Equality((Object) this.mEArrowDown, (Object) null))
      return;
    GearKindCorrelations kindCorrelations = MasterData.UniqueGearKindCorrelationsBy(attackerGearKind, defenderGearKind);
    if (kindCorrelations == null)
    {
      ((Behaviour) this.mMyArrowUp).enabled = false;
      ((Behaviour) this.mMyArrowDown).enabled = false;
      ((Behaviour) this.mEArrowUp).enabled = false;
      ((Behaviour) this.mEArrowDown).enabled = false;
    }
    else if (kindCorrelations.is_advantage)
    {
      ((Behaviour) this.mMyArrowUp).enabled = true;
      ((Behaviour) this.mMyArrowDown).enabled = false;
      ((Behaviour) this.mEArrowUp).enabled = false;
      ((Behaviour) this.mEArrowDown).enabled = true;
    }
    else
    {
      ((Behaviour) this.mMyArrowUp).enabled = false;
      ((Behaviour) this.mMyArrowDown).enabled = true;
      ((Behaviour) this.mEArrowUp).enabled = true;
      ((Behaviour) this.mEArrowDown).enabled = false;
    }
  }

  private IEnumerator makePlayerUnit(DuelResult result)
  {
    BL.Unit beUnit = result.playerUnit();
    SkillMetamorphosis metamor = beUnit.metamorphosis;
    Future<GameObject> mp = beUnit.playerUnit.LoadModelDuel(metamor != null ? metamor.metamorphosis_id : 0);
    IEnumerator e = mp.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result1 = mp.Result;
    if (Object.op_Equality((Object) null, (Object) result1))
    {
      Debug.LogError((object) "[NGDuelManager] at makePlayerUnit failed to create prefab.");
    }
    else
    {
      GameObject goUnit = result1.Clone(this.mRoot3d.transform);
      this.mPlayerDUnit = goUnit.GetOrAddComponent<NGDuelUnit>();
      string attach_node;
      Future<GameObject> unitEffect = beUnit.playerUnit.LoadModelUnitAuraEffect(out attach_node, metamor != null ? metamor.metamorphosis_id : 0);
      Transform effectPoint = !string.IsNullOrEmpty(attach_node) ? goUnit.transform.GetChildInFind(attach_node) : (Transform) null;
      if (Object.op_Equality((Object) effectPoint, (Object) null))
        effectPoint = goUnit.transform.GetChildInFind("Bip");
      if (Object.op_Equality((Object) effectPoint, (Object) null))
        effectPoint = goUnit.transform.GetChildInFind("bip");
      if (Object.op_Inequality((Object) effectPoint, (Object) null))
      {
        e = unitEffect.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result2 = unitEffect.Result;
        if (Object.op_Inequality((Object) result2, (Object) null))
          result2.Clone(effectPoint);
      }
      e = beUnit.playerUnit.unit.ProcessAttachAwakeEffect(goUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (result.isExplore)
      {
        ExploreFloor floorData = Singleton<ExploreDataManager>.GetInstance().FloorData;
        clipEffectPlayer component = ((Component) this.mPlayerDUnit).GetComponent<clipEffectPlayer>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          switch (floorData.folder_path)
          {
            case 10001:
              component.setGroundStatus(MasterData.BattleLandform[13]);
              break;
            case 10002:
              component.setGroundStatus(MasterData.BattleLandform[91]);
              break;
          }
        }
      }
    }
  }

  private IEnumerator makeEnemyUnit(DuelResult result)
  {
    BL.Unit beUnit = result.enemyUnit();
    SkillMetamorphosis metamor = beUnit.metamorphosis;
    Future<GameObject> mp = beUnit.playerUnit.LoadModelDuel(metamor != null ? metamor.metamorphosis_id : 0);
    IEnumerator e = mp.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result1 = mp.Result;
    if (Object.op_Equality((Object) null, (Object) result1))
    {
      Debug.LogError((object) "[NGDuelManager] at makeEnemyUnit failed to create prefab.");
    }
    else
    {
      GameObject goUnit = result1.Clone(this.mRoot3d.transform);
      this.mEnemyDUnit = goUnit.GetOrAddComponent<NGDuelUnit>();
      string attach_node;
      Future<GameObject> unitEffect = beUnit.playerUnit.LoadModelUnitAuraEffect(out attach_node, metamor != null ? metamor.metamorphosis_id : 0);
      Transform effectPoint = !string.IsNullOrEmpty(attach_node) ? goUnit.transform.GetChildInFind(attach_node) : (Transform) null;
      if (Object.op_Equality((Object) effectPoint, (Object) null))
        effectPoint = goUnit.transform.GetChildInFind("Bip");
      if (Object.op_Equality((Object) effectPoint, (Object) null))
        effectPoint = goUnit.transform.GetChildInFind("bip");
      if (Object.op_Inequality((Object) effectPoint, (Object) null))
      {
        e = unitEffect.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject result2 = unitEffect.Result;
        if (Object.op_Inequality((Object) result2, (Object) null))
          result2.Clone(effectPoint);
      }
      e = beUnit.playerUnit.unit.ProcessAttachAwakeEffect(goUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (result.isExplore)
      {
        ExploreFloor floorData = Singleton<ExploreDataManager>.GetInstance().FloorData;
        clipEffectPlayer component = ((Component) this.mEnemyDUnit).GetComponent<clipEffectPlayer>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          switch (floorData.folder_path)
          {
            case 10001:
              component.setGroundStatus(MasterData.BattleLandform[13]);
              break;
            case 10002:
              component.setGroundStatus(MasterData.BattleLandform[91]);
              break;
          }
        }
      }
    }
  }

  private unitInfomation makeUnitInfo(
    DuelResult result,
    NGDuelUnit enemyUnit,
    Battle0181CharacterStatus cs,
    Transform initpos,
    bool isPlayer = true)
  {
    unitInfomation unitInfomation = new unitInfomation();
    BL.Unit unit = isPlayer ? result.playerUnit() : result.enemyUnit();
    int[] numArray = isPlayer ? result.playerUnitBeforeAilmentEffectIDs() : result.enemyUnitBeforeAilmentEffectIDs();
    if ((BL.Unit) null == unit)
    {
      Debug.LogError((object) "[NGDuelManager] at makeUnitInfo beUnit is NULL.");
      return (unitInfomation) null;
    }
    unitInfomation.bu = unit;
    if (Object.op_Equality((Object) null, (Object) cs))
    {
      Debug.LogError((object) "[NGDuelManager] at makeUnitInfo 0181CharaStatus is NULL.");
      return (unitInfomation) null;
    }
    unitInfomation.p = cs;
    if (Object.op_Equality((Object) null, (Object) enemyUnit))
    {
      Debug.LogError((object) "[NGDuelManager] at makeUnitInfo enemyUnit is NULL.");
      return (unitInfomation) null;
    }
    unitInfomation.enemy = enemyUnit;
    if (Object.op_Equality((Object) null, (Object) initpos))
      Debug.LogError((object) "[NGDuelManager] at makeUnitInfo InitPosition is NULL.");
    unitInfomation.trs = initpos;
    unitInfomation.isplayer = isPlayer;
    unitInfomation.beforeAilmentEffectIDs = numArray;
    unitInfomation.metamorphosis = unit.metamorphosis;
    unitInfomation.range = result.distance == 0 ? 1 : result.distance;
    AttackStatus attackStatus = isPlayer ? result.playerAttackStatus() : result.enemyAttackStatus();
    if (attackStatus != null)
    {
      if (attackStatus.magicBullet != null)
        unitInfomation.mb = attackStatus.magicBullet;
      else if (attackStatus.weapon != null)
        unitInfomation.weapon = attackStatus.weapon;
    }
    if (result.isPlayerAttack & isPlayer)
    {
      unitInfomation.support = result.attackDuelSupport;
      unitInfomation.supportHitIncr = result.attackDuelSupportHitIncr;
      unitInfomation.supportEvasionIncr = result.attackDuelSupportEvasionIncr;
      unitInfomation.supportCriticalIncr = result.attackDuelSupportCriticalIncr;
      unitInfomation.supportCriticalEvasionIncr = result.attackDuelSupportCriticalEvasionIncr;
    }
    else if (result.isPlayerAttack && !isPlayer)
    {
      unitInfomation.support = result.defenseDuelSupport;
      unitInfomation.supportHitIncr = result.defenseDuelSupportHitIncr;
      unitInfomation.supportEvasionIncr = result.defenseDuelSupportEvasionIncr;
      unitInfomation.supportCriticalIncr = result.defenseDuelSupportCriticalIncr;
      unitInfomation.supportCriticalEvasionIncr = result.defenseDuelSupportCriticalEvasionIncr;
    }
    else if (!result.isPlayerAttack & isPlayer)
    {
      unitInfomation.support = result.defenseDuelSupport;
      unitInfomation.supportHitIncr = result.defenseDuelSupportHitIncr;
      unitInfomation.supportEvasionIncr = result.defenseDuelSupportEvasionIncr;
      unitInfomation.supportCriticalIncr = result.defenseDuelSupportCriticalIncr;
      unitInfomation.supportCriticalEvasionIncr = result.defenseDuelSupportCriticalEvasionIncr;
    }
    else
    {
      unitInfomation.support = result.attackDuelSupport;
      unitInfomation.supportHitIncr = result.attackDuelSupportHitIncr;
      unitInfomation.supportEvasionIncr = result.attackDuelSupportEvasionIncr;
      unitInfomation.supportCriticalIncr = result.attackDuelSupportCriticalIncr;
      unitInfomation.supportCriticalEvasionIncr = result.attackDuelSupportCriticalEvasionIncr;
    }
    if (Object.op_Inequality((Object) null, (Object) this.mRoot3d))
      unitInfomation.root3d = this.mRoot3d;
    unitInfomation.mng = this;
    return unitInfomation;
  }

  private void Update()
  {
    if (!this.mInitialized || this.isWait)
      return;
    if (this.currSts == NGDuelManager.DuelStatus.ST_INIT && this.mInitialized)
      this.currSts = !this.mDuelResult.isFirstBoss ? (!this.mDuelResult.isBossBattle ? NGDuelManager.DuelStatus.ST_PREP_DUEL_START : NGDuelManager.DuelStatus.ST_PREP_DUEL_START_BOSS) : NGDuelManager.DuelStatus.ST_PREP_DUEL_INTRO_BOSS;
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_DUEL_START)
    {
      if (this.mDuelResult.isPlayerAttack)
      {
        if (this.mDuelResult.distance == 1 && this.useDistance)
          this.StartCoroutine(this.mCameraManager.changeCamera("startCam_02"));
        else
          this.StartCoroutine(this.mCameraManager.changeCamera("startCam_01"));
      }
      else if (this.mDuelResult.distance == 1 && this.useDistance)
        this.StartCoroutine(this.mCameraManager.changeCamera("2PstartCam_02"));
      else
        this.StartCoroutine(this.mCameraManager.changeCamera("2PstartCam_01"));
      this.currSts = !this.mDuelResult.isColosseum ? NGDuelManager.DuelStatus.ST_EXEC_DUEL_START : NGDuelManager.DuelStatus.ST_EXEC_COLOSSEUM_DUEL_START;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_DUEL_START)
    {
      if (this.mDuelStorys == null)
      {
        this.StartCoroutine(this.waitCurrentStatusEnd(NGDuelManager.DuelStatus.ST_PREP_BUFF, this.DuelCameraDispTime));
        this.currSts = NGDuelManager.DuelStatus.ST_NONE;
      }
      else if (this.mDuelStorys.Count<BL.Story>((Func<BL.Story, bool>) (x => x.type == BL.StoryType.duel_start)) > 0)
      {
        this.StartCoroutine(this.waitCurrentStatusEnd(NGDuelManager.DuelStatus.ST_PREP_DUEL_TALK, this.DuelCameraDispTime));
        this.currSts = NGDuelManager.DuelStatus.ST_NONE;
      }
      else
      {
        this.StartCoroutine(this.waitCurrentStatusEnd(NGDuelManager.DuelStatus.ST_PREP_BUFF, this.DuelCameraDispTime));
        this.currSts = NGDuelManager.DuelStatus.ST_NONE;
      }
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_COLOSSEUM_DUEL_START)
    {
      this.StartCoroutine(this.startDuelStartAnimation(NGDuelManager.DuelStatus.ST_PREP_BUFF, this.DuelCameraDispTime));
      this.currSts = NGDuelManager.DuelStatus.ST_NONE;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_DUEL_INTRO_BOSS)
    {
      this.StartCoroutine(this.mCameraManager.changeCamera("BossIntro_cam01"));
      this.mEnemyDUnit.playWinPose();
      this.wait_time = this.mBossIntroDisplayTime;
      this.currSts = NGDuelManager.DuelStatus.ST_EXEC_DUEL_INTRO_BOSS;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_DUEL_INTRO_BOSS)
    {
      this.wait_time -= Time.deltaTime;
      if (0.0 > (double) this.wait_time)
        this.currSts = NGDuelManager.DuelStatus.ST_PREP_DUEL_START_BOSS;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_DUEL_START_BOSS)
    {
      this.StartCoroutine(this.mCameraManager.changeCamera("BossBattleStartCam_01"));
      this.currSts = NGDuelManager.DuelStatus.ST_EXEC_DUEL_START_BOSS;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_DUEL_START_BOSS)
    {
      this.StartCoroutine(this.waitBossFirstCameraEnd());
      this.currSts = NGDuelManager.DuelStatus.ST_NONE;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_DUEL_TALK)
    {
      foreach (BL.Story mDuelStory in this.mDuelStorys)
      {
        if (mDuelStory.type == BL.StoryType.duel_start)
          Singleton<NGSceneManager>.GetInstance().changeScene("story009_3", true, (object) mDuelStory.scriptId);
      }
      this.isWait = true;
      this.StartCoroutine(this.waitCurrentStatusEnd(NGDuelManager.DuelStatus.ST_EXEC_DUEL_TALK, 0.1f));
      this.currSts = NGDuelManager.DuelStatus.ST_NONE;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_DUEL_TALK && Singleton<NGSceneManager>.GetInstance().isSceneInitialized)
      this.currSts = NGDuelManager.DuelStatus.ST_PREP_BUFF;
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_BUFF)
    {
      this.isReadyForSkip = true;
      this.mPlayerDUnit.showDuelSupport();
      this.mEnemyDUnit.showDuelSupport();
      this.wait_time = this.mDuelSupportDisplayTime;
      this.currSts = NGDuelManager.DuelStatus.ST_EXEC_BUFF;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_BUFF)
    {
      this.wait_time -= Time.deltaTime;
      if (0.0 > (double) this.wait_time)
      {
        this.currSts = NGDuelManager.DuelStatus.ST_PREP_COMMAND;
        this.mPlayerDUnit.hideDuelSupport();
      }
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_COMMAND)
    {
      this.buildTurns();
      this.currSts = NGDuelManager.DuelStatus.ST_EXEC_COMMAND;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_COMMAND)
    {
      if (Object.op_Equality((Object) this.mPlayerDUnit, (Object) null) || Object.op_Equality((Object) this.mEnemyDUnit, (Object) null))
        return;
      float distance = Math.Abs(((Component) this.mPlayerDUnit).transform.position.x - ((Component) this.mEnemyDUnit).transform.position.x);
      this.mPlayerDUnit.mDistanceFromEnemy = distance;
      this.mEnemyDUnit.mDistanceFromEnemy = distance;
      if (this.mPreTurn != null)
        this.mCameraManager.updateLookatPosition(distance, this.mPlayerDUnit, this.mEnemyDUnit, this.mPreTurn.isAtackker);
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_RESULT)
      this.currSts = !this.mDuelResult.isPlayerAttack || !this.mDuelResult.isDieAttack ? (this.mDuelResult.isPlayerAttack || !this.mDuelResult.isDieAttack ? (!this.mDuelResult.isPlayerAttack || !this.mDuelResult.isDieDefense ? (this.mDuelResult.isPlayerAttack || !this.mDuelResult.isDieDefense ? NGDuelManager.DuelStatus.ST_PREP_RES_DRAW : NGDuelManager.DuelStatus.ST_PREP_RES_LOSE) : NGDuelManager.DuelStatus.ST_PREP_RES_WIN) : NGDuelManager.DuelStatus.ST_PREP_RES_WIN) : NGDuelManager.DuelStatus.ST_PREP_RES_LOSE;
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_RES_LOSE)
    {
      this.mPlayerDUnit.adjustPosition(this.PlayerInitPosition);
      if (!this.mDuelResult.isBossBattle)
      {
        if (this.mDuelResult.distance > 1)
          this.StartCoroutine(this.mCameraManager.changeCamera("LoseCam_01"));
        else
          this.StartCoroutine(this.mCameraManager.changeCamera("LoseCam_r1"));
      }
      else
        this.StartCoroutine(this.mCameraManager.changeCamera("BossBattleAttackLoseCam_01"));
      this.currSts = this.mDuelStorys != null ? (this.mDuelStorys.Count<BL.Story>((Func<BL.Story, bool>) (x => x.type == BL.StoryType.duel_unit_dead)) <= 0 ? NGDuelManager.DuelStatus.ST_EXEC_RES_LOSE : NGDuelManager.DuelStatus.ST_PREP_LOSE_TALK) : NGDuelManager.DuelStatus.ST_EXEC_RES_LOSE;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_RES_LOSE)
    {
      this.currSts = NGDuelManager.DuelStatus.ST_NONE;
      this.StartCoroutine(this.waitCurrentStatusEnd(NGDuelManager.DuelStatus.ST_PREP_END, this.LoseDispTime));
      this.mParentScene.OnDuelLose(this.LoseDispTime);
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_LOSE_TALK)
    {
      foreach (BL.Story mDuelStory in this.mDuelStorys)
      {
        if (mDuelStory.type == BL.StoryType.duel_unit_dead)
          Singleton<NGSceneManager>.GetInstance().changeScene("story009_3", true, (object) mDuelStory.scriptId);
      }
      this.StartCoroutine(this.waitCurrentStatusEnd(NGDuelManager.DuelStatus.ST_EXEC_DUEL_TALK, 0.1f));
      this.currSts = NGDuelManager.DuelStatus.ST_NONE;
      this.isWait = true;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_LOSE_TALK && Singleton<NGSceneManager>.GetInstance().isSceneInitialized)
      this.currSts = NGDuelManager.DuelStatus.ST_EXEC_RES_LOSE;
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_RES_WIN)
    {
      this.currSts = NGDuelManager.DuelStatus.ST_NONE;
      this.StartCoroutine(this.playWin());
      this.mParentScene.OnDuelWin(this.WinDispTime);
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_RES_WIN && !Singleton<NGSoundManager>.GetInstance().IsVoicePlaying(this.endVoiceChannel))
      this.currSts = NGDuelManager.DuelStatus.ST_PREP_END;
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_RES_DRAW)
    {
      int num = this.mDuelResult.isBossBattle ? 1 : 0;
      this.currSts = NGDuelManager.DuelStatus.ST_EXEC_RES_DRAW;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_RES_DRAW)
    {
      this.StartCoroutine(this.waitCurrentStatusEnd(NGDuelManager.DuelStatus.ST_PREP_END, this.DrawDispTime));
      this.currSts = NGDuelManager.DuelStatus.ST_NONE;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_PREP_END)
    {
      this.isReadyForSkip = false;
      this.currSts = NGDuelManager.DuelStatus.ST_EXEC_END;
    }
    if (this.currSts == NGDuelManager.DuelStatus.ST_EXEC_END)
      this.currSts = NGDuelManager.DuelStatus.ST_END;
    if (this.currSts != NGDuelManager.DuelStatus.ST_END)
      return;
    this.FinishDuel();
    this.currSts = NGDuelManager.DuelStatus.ST_NONE;
  }

  private IEnumerator playWin()
  {
    NGDuelManager ngDuelManager = this;
    CommonRoot commonroot = Singleton<CommonRoot>.GetInstance();
    int orgDeapth = 0;
    if (Object.op_Inequality((Object) commonroot, (Object) null))
    {
      orgDeapth = commonroot.setBlackBGPanelDepth(-1);
      commonroot.isActiveBlackBGPanel = true;
      yield return (object) new WaitForSeconds(0.5f);
    }
    ngDuelManager.mPlayerDUnit.SetWinMode();
    string empty = string.Empty;
    if (!ngDuelManager.mDuelResult.isBossBattle)
    {
      if (ngDuelManager.duel_distance != 1)
        ngDuelManager.StartCoroutine(ngDuelManager.mCameraManager.changeCamera("WinCam_01", ngDuelManager.mPlayerDUnit.baseGameObject.transform, true));
    }
    else
      ngDuelManager.StartCoroutine(ngDuelManager.mCameraManager.changeCamera("BossBattleAttackWinCam_01", animaterWait: true));
    IEnumerator e = ngDuelManager.mCameraManager.changeCamera("WinCam_r1", ngDuelManager.mPlayerDUnit.baseGameObject.transform, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ngDuelManager.mPlayerDUnit.loadWinAnimator();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ngDuelManager.mCameraManager.SetAnimatorEnabled();
    ngDuelManager.mPlayerDUnit.SetAnimatorEnabled();
    ngDuelManager.endVoiceChannel = !ngDuelManager.mDuelResult.isExplore ? ngDuelManager.mPlayerDUnit.playWinVoice() : -1;
    if (ngDuelManager.endVoiceChannel == -1)
    {
      ngDuelManager.currSts = NGDuelManager.DuelStatus.ST_NONE;
      ngDuelManager.StartCoroutine(ngDuelManager.waitCurrentStatusEnd(NGDuelManager.DuelStatus.ST_PREP_END, ngDuelManager.WinDispTime));
    }
    else
      ngDuelManager.currSts = NGDuelManager.DuelStatus.ST_EXEC_RES_WIN;
    if (Object.op_Inequality((Object) commonroot, (Object) null))
    {
      commonroot.isActiveBlackBGPanel = false;
      yield return (object) new WaitForSeconds(0.5f);
      orgDeapth = commonroot.setBlackBGPanelDepth(orgDeapth);
    }
  }

  private void FinishDuel()
  {
    this.isReadyForSkip = false;
    if (Object.op_Inequality((Object) this.mPlayerDUnit, (Object) null))
      this.mPlayerDUnit.SetCleanUpDuelSkillEffect();
    if (Object.op_Inequality((Object) this.mEnemyDUnit, (Object) null))
      this.mEnemyDUnit.SetCleanUpDuelSkillEffect();
    this.mParentScene.backToBattle();
    this.mCameraManager.Reset();
    Object.Destroy((Object) this.mPlayerDUnit.baseGameObject);
    Object.Destroy((Object) this.mEnemyDUnit.baseGameObject);
    this.mPlayerDUnit = (NGDuelUnit) null;
    this.mEnemyDUnit = (NGDuelUnit) null;
    Singleton<NGDuelDataManager>.GetInstance().ClearOneTimeDuelCache();
    this.mInitialized = false;
    ((Component) this).gameObject.SetActive(false);
    this.mCameraManager.Initialize(true, this.isSkipped);
    this.isSkipped = false;
    Singleton<NGDuelDataManager>.GetInstance().SetActiveMap(false);
    Singleton<NGSoundManager>.GetInstance().StopSe();
    Singleton<CommonRoot>.GetInstance().isActiveBlackBGPanel = false;
    Singleton<CommonRoot>.GetInstance().setBlackBGPanelDepth(this.beforeBlackBGPanelDepth);
  }

  public void PlayerMoveToInitPos() => this.mPlayerDUnit.adjustPosition(this.PlayerInitPosition);

  public void Skip()
  {
    if (!this.mInitialized || this.isWait || !this.isReadyForSkip || this.currSts == NGDuelManager.DuelStatus.ST_INIT)
      return;
    Singleton<NGSoundManager>.GetInstance().stopVoice();
    this.currSts = NGDuelManager.DuelStatus.ST_PREP_END;
    this.isSkipped = true;
    this.isReadyForSkip = false;
  }

  private IEnumerator waitCurrentStatusEnd(NGDuelManager.DuelStatus status, float duration = 1.6f)
  {
    yield return (object) new WaitForSeconds(duration);
    this.currSts = status;
  }

  private IEnumerator startDuelStartAnimation(NGDuelManager.DuelStatus status, float duration = 1.6f)
  {
    IEnumerator e = this.mParentScene.createColosseumDuelInvokeSkillDisp(this.mPlayerDUnit.mMyUnitData, this.mEnemyDUnit.mMyUnitData, this.mDuelResult.playerAttackStatus(), this.mDuelResult.playerColosseumNAS(), this.mDuelResult.playerColosseumFirstAttack(), this.mDuelResult.enemyAttackStatus(), this.mDuelResult.enemyColosseumNAS(), this.mDuelResult.enemyColosseumFirstAttack());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(duration);
    this.currSts = status;
  }

  private IEnumerator waitBossFirstCameraEnd(float duration = 2f)
  {
    yield return (object) new WaitForSeconds(duration);
    this.currSts = this.mDuelStorys != null ? (this.mDuelStorys.Count<BL.Story>((Func<BL.Story, bool>) (x => x.type == BL.StoryType.duel_start)) != 0 ? NGDuelManager.DuelStatus.ST_PREP_DUEL_TALK : NGDuelManager.DuelStatus.ST_PREP_BUFF) : NGDuelManager.DuelStatus.ST_PREP_BUFF;
  }

  private IEnumerator createMap(BL.Stage stage)
  {
    if (!this.mDuelResult.isExplore)
    {
      bool isColosseum = this.mDuelResult.isColosseum;
      IEnumerator e = Singleton<NGDuelDataManager>.GetInstance().CreateMap(stage, isColosseum, this.mRoot3d.transform, this.directionalLight);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void ResetLight()
  {
    Singleton<NGDuelDataManager>.GetInstance().ResetLight(this.directionalLight);
  }

  private void adjustPosition()
  {
    this.mPlayerDUnit.adjustPosition(this.PlayerInitPosition);
    this.mEnemyDUnit.adjustPosition(this.EnemyInitPosition);
  }

  public void ResetDuelAmbient()
  {
    if (!this.mInitialized)
      return;
    RenderSettings.ambientLight = this.duelAmbientLight;
  }

  private void buildTurns()
  {
    if (this.mDuelResult.turns == null)
      this.currSts = NGDuelManager.DuelStatus.ST_END;
    else if (((IEnumerable<BL.DuelTurn>) this.mDuelResult.turns).Count<BL.DuelTurn>() == 0)
      this.currSts = NGDuelManager.DuelStatus.ST_END;
    else if (this.mDuelTurn >= ((IEnumerable<BL.DuelTurn>) this.mDuelResult.turns).Count<BL.DuelTurn>())
    {
      this.currSts = NGDuelManager.DuelStatus.ST_END;
    }
    else
    {
      BL.DuelTurn turn1 = this.mDuelResult.turns[this.mDuelTurn];
      int attackerIndex = this.mDuelResult.isPlayerAttack ? (turn1.isAtackker ? 0 : 1) : (turn1.isAtackker ? 1 : 0);
      if (((IEnumerable<BL.DuelTurn>) this.mDuelResult.turns).Count<BL.DuelTurn>() > this.mDuelTurn + turn1.attackCount)
      {
        BL.DuelTurn turn2 = this.mDuelResult.turns[this.mDuelTurn + turn1.attackCount];
        int num = this.mDuelResult.isPlayerAttack ? (turn2.isAtackker ? 0 : 1) : (turn2.isAtackker ? 1 : 0);
        for (int index = 0; index < turn2.attackCount; ++index)
          Singleton<NGDuelDataManager>.GetInstance().StartTurnEffectBackgroundLoad(this.mDuelTurn + turn1.attackCount + index);
      }
      if (this.mDuelResult.isColosseum && turn1.attackerStatus != null && turn1.defenderStatus != null)
      {
        if (this.mDuelResult.isPlayerAttack)
          this.mParentScene.ChangeStatus(turn1.attackerStatus, this.mDuelResult.playerColosseumFirstAttack(), turn1.defenderStatus, this.mDuelResult.enemyColosseumFirstAttack());
        else
          this.mParentScene.ChangeStatus(turn1.defenderStatus, this.mDuelResult.playerColosseumFirstAttack(), turn1.attackerStatus, this.mDuelResult.enemyColosseumFirstAttack());
      }
      List<BL.DuelTurn> turns = new List<BL.DuelTurn>();
      for (int index = 0; index < turn1.attackCount; ++index)
      {
        if (((IEnumerable<BL.DuelTurn>) this.mDuelResult.turns).Count<BL.DuelTurn>() > this.mDuelTurn + index)
          turns.Add(this.mDuelResult.turns[this.mDuelTurn + index]);
      }
      this.StartCoroutine(this.startAttack(attackerIndex, turns, this.mDuelTurn));
      this.mDuelTurn += turn1.attackCount;
      this.mPreTurn = turn1;
    }
  }

  private IEnumerator startAttack(int attackerIndex, List<BL.DuelTurn> turns, int turn_count)
  {
    int attackCount = turns[0].attackCount;
    for (int i = 0; i < attackCount; ++i)
      yield return (object) new WaitUntil((Func<bool>) (() => Singleton<NGDuelDataManager>.GetInstance().IsTurnEffectLoadFinished(turn_count + i)));
    this.mParentScene.OnUnitStartAttack(attackerIndex, turns);
    yield return (object) this.getNGDuelUnit(attackerIndex).startAttack(turns, turn_count);
  }

  public void turnEnd()
  {
    if (((IEnumerable<BL.DuelTurn>) this.mDuelResult.turns).Count<BL.DuelTurn>() > this.mDuelTurn)
    {
      this.currSts = NGDuelManager.DuelStatus.ST_PREP_COMMAND;
    }
    else
    {
      if (this.currSts >= NGDuelManager.DuelStatus.ST_PREP_RESULT && this.currSts <= NGDuelManager.DuelStatus.ST_END)
        return;
      this.currSts = NGDuelManager.DuelStatus.ST_PREP_RESULT;
    }
  }

  public bool skipDuel()
  {
    if (!this.mInitialized)
      return false;
    this.StopAllCoroutines();
    this.currSts = NGDuelManager.DuelStatus.ST_END;
    return true;
  }

  public void ActAttackBeginCamera()
  {
    this.mCameraManager.ActAttackBeginCamera(this.mDuelResult, this.mPlayerDUnit);
  }

  public void ActCameraToCenter() => this.mCameraManager.ActCameraToCenter();

  public void ActKoyuCamera(RuntimeAnimatorController rac)
  {
    this.mCameraManager.ActKoyuCamera(rac);
  }

  public void EnableKoyuCamera() => this.mCameraManager.EnableKoyuCamera();

  public void EndKoyuCamera() => this.mCameraManager.EndKoyuCamera();

  public void ActCameraToMe(NGDuelUnit myunit) => this.mCameraManager.ActCameraToMe(myunit);

  public void ShakeCamera() => this.mCameraManager.ShakeCamera();

  public void WarpCameraToPrincess()
  {
    this.mCameraManager.WarpCamera1stAttackPos(true, ((Component) this.mEnemyDUnit).gameObject.transform);
  }

  public void WarpCameraToEnemy()
  {
    this.mCameraManager.WarpCamera1stAttackPos(false, ((Component) this.mPlayerDUnit).gameObject.transform);
  }

  public void WarpCameraToPrincessAsRemote(Transform bullet, float delay = 2f)
  {
    this.StartCoroutine(this.mCameraManager.WarpCamera1stAttackPosRemoteEnemy(((Component) this.mEnemyDUnit).gameObject.transform, delay, bullet));
  }

  public void WarpCameraToEnemyAsRemote(Transform bullet = null, float delay = 2f)
  {
    this.StartCoroutine(this.mCameraManager.WarpCamera1stAttackPosRemoteEnemy(((Component) this.mPlayerDUnit).gameObject.transform, delay, bullet));
  }

  public void CameraChaseBullet(Transform bullet) => this.mCameraManager.LookCameraBullet(bullet);

  public void SetCameraSmoothTime(float f) => this.mCameraManager.smoothTime = f;

  public float GetCameraSmoothTime() => this.mCameraManager.smoothTime;

  public void ChangeCameraToRemotoSuiseiHalf()
  {
    this.StartCoroutine(this.mCameraManager.changeCamera("attack_long_lastattck_cam"));
  }

  private IEnumerator preloadCommonDuelEffect()
  {
    IEnumerator e = Singleton<NGDuelDataManager>.GetInstance().PreloadCommonDuelEffect();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator preloadEffect(DuelResult duelResult)
  {
    Singleton<NGDuelDataManager>.GetInstance().SetupTurnEffectBackgroundLoader(this.mDuelResult, this.mPlayerDUnit, this.mEnemyDUnit);
    for (int i = 0; i < duelResult.turns[0].attackCount; ++i)
    {
      Singleton<NGDuelDataManager>.GetInstance().StartTurnEffectBackgroundLoad(i);
      yield return (object) new WaitUntil((Func<bool>) (() => Singleton<NGDuelDataManager>.GetInstance().IsTurnEffectLoadFinished(i)));
    }
    IEnumerator e = Singleton<NGDuelDataManager>.GetInstance().LoadUnitControllerDuelEffect(this.mPlayerDUnit, this.mEnemyDUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGDuelDataManager>.GetInstance().AddPreloadDuelEffect(Singleton<NGDuelDataManager>.GetInstance().mDustEffect);
  }

  public void SetActiveMap(bool active)
  {
    Singleton<NGDuelDataManager>.GetInstance().SetActiveMap(active);
  }

  private enum DuelStatus
  {
    ST_INIT,
    ST_PREP_DUEL_START,
    ST_EXEC_DUEL_START,
    ST_EXEC_COLOSSEUM_DUEL_START,
    ST_PREP_DUEL_INTRO_BOSS,
    ST_EXEC_DUEL_INTRO_BOSS,
    ST_PREP_DUEL_START_BOSS,
    ST_EXEC_DUEL_START_BOSS,
    ST_PREP_DUEL_TALK,
    ST_EXEC_DUEL_TALK,
    ST_PREP_BUFF,
    ST_EXEC_BUFF,
    ST_PREP_COMMAND,
    ST_EXEC_COMMAND,
    ST_PREP_RESULT,
    ST_PREP_RES_LOSE,
    ST_EXEC_RES_LOSE,
    ST_PREP_LOSE_TALK,
    ST_EXEC_LOSE_TALK,
    ST_PREP_RES_WIN,
    ST_EXEC_RES_WIN,
    ST_PREP_RES_DRAW,
    ST_EXEC_RES_DRAW,
    ST_PREP_END,
    ST_EXEC_END,
    ST_END,
    ST_NONE,
  }
}
