// Decompiled with JetBrains decompiler
// Type: NGDuelUnit
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
public class NGDuelUnit : NGDuelUnitBase
{
  private const float TURN_TIME_OUT = 60f;
  private const float LONG_RANGE_REACH = 9f;
  protected NGDuelUnit.Status currentStatus = NGDuelUnit.Status.ST_NONE;
  private NGDuelManager mDuelManager;
  protected Battle0181CharacterStatus mPlayerUI;
  protected BL.MagicBullet mMagicBullet;
  protected List<BL.DuelTurn> mThisTurns = new List<BL.DuelTurn>();
  protected int mThisTurnsCurrent;
  protected int mThisTurnCount;
  protected List<BL.DuelTurn> mThisTurnDamages = new List<BL.DuelTurn>();
  protected int mDispDamageCount;
  protected NGDuelUnit mMyUnit;
  protected NGDuelUnit mEnemyUnit;
  protected bool isRemovedSleepEffect;
  private IntimateDuelSupport mSupport;
  private int mSupportHitIncr;
  private int mSupportEvasionIncr;
  private int mSupportCriticalIncr;
  private int mSupportCriticalEvasionIncr;
  private bool mInitialized;
  protected bool mIsPlayer;
  protected bool mIsDamageWait;
  protected bool mIsDodging;
  protected bool mFirstAttackDone;
  public float mOrgReach = 1f;
  public float mMyReach = 1f;
  public float mDistanceFromEnemy;
  protected float mDirToEnemy = -1f;
  private float mTurnSpentTime;
  public string currentClipStatus = "";
  protected Transform mInitPos;
  protected MissileWeapon mMissileWeapon;
  protected GameObject mMissileWeaponPrefab;
  protected GameObject mMissileWeaponReadyEffectPrefab;
  protected Dictionary<CommonElement, GameObject> mMissileWeaponElementEffectPrefabDic = new Dictionary<CommonElement, GameObject>();
  public GameObject mMadanPrefab;
  protected GameObject mEffectDuelSupport;
  protected List<GameObject> mRemoveObjects = new List<GameObject>();
  private bool mIsSuisei;
  protected int mSuiseiCount;
  private int mAttackMotionCount;
  public bool mIsLastAttack;
  public bool mIsFireFirstSuiseiMadan;
  private float mOrigSmoothTime;
  private NGSoundManager mSM;
  protected UnitVoicePattern mUnitVoicePattern;
  private bool mAttackVoicePlaying;
  protected List<GameObject> mBullets = new List<GameObject>();
  public bool mToNextTurn;
  public bool mToExecEndTurn;
  private bool mNowPlayKoyu;
  private bool mPlaiedKoyu;
  private GameObject mDrainBullet;
  private Linear mLinearX;
  private Linear mLinearY;
  private bool mRemoteSuiseiCameraFlag;
  public bool isInvalidDodgeMotion;
  private BL.Skill mInvokeDuelSkill;
  private List<BL.Skill> mDefenseDuelSkills;
  public DuelMoveTypeEnum mMoveType;
  private CommonElement? mUnitElement;
  private CommonElement[] mAttackElements;
  private Dictionary<CommonElement, DuelElementTrailEffect> mElementsTrailEffectDic = new Dictionary<CommonElement, DuelElementTrailEffect>();
  private Dictionary<CommonElement, DuelElementBulletEffect> mElementsBulletEffectDic = new Dictionary<CommonElement, DuelElementBulletEffect>();
  private string critical_attack1;
  private string nomal_attack1_last;
  private string nomal_attack_last;
  private string dual_single_attack;
  private string critical_attackS_last;
  private string critical_attack_last;
  private string dual_single_critical;
  private string nomal_attack_magic_last;
  private string critical_attack_magic_last;
  private string magic_single;
  private string magicS_single;
  private string skill_ss5_attack4_last;
  private string skill_attack1_last;
  private string skill_attack_last;
  private string skill_attack_throw_last;
  private string skill_ss5_attack_throw4_last;
  private string skill_ss5_attack_throw5_last;
  private string critical_attack_throw_last;
  private string nomal_attack_throw_last;
  private string skill_attack;
  private string trigger_option_attack;
  private string trigger_dual_single_option_attack;
  private string trigger_option_attack_throw;
  private string trigger_dual_single_option_attack_throw;
  private Transform oldUnitParent;
  private Vector3 oldCharactorTransLocalPos;
  private Quaternion oldCharactorTransLocalRot;
  private Ryusei_100111_ef playingRyuseiEf;
  private const string SWORD_BULLET_DEFAULT = "ef329_sword_bullet";
  private const string AXE_BULLET_DEFAULT = "ef325_axe_bullet";
  private const string SPEAR_BULLET_DEFAULT = "ef326_spear_bullet";
  private const string ARROW_BULLET_DEFAULT = "ef324_bow_bullet";

  public int[] beforeAilmentEffectIDs { get; private set; }

  public void AddAttackMotionCount()
  {
    ++this.mAttackMotionCount;
    if (this.thisTurn == null || this.thisTurn.suiseiResults == null || this.mAttackMotionCount != this.thisTurn.suiseiResults.Count - 1)
      return;
    this.setTrigToAnimator("to_ss_last");
  }

  public UnitVoicePattern UnitVoicePattern => this.mUnitVoicePattern;

  public List<string> controllerPreloadEffectList
  {
    get => this.mConfig == null ? new List<string>() : this.mConfig.preloadEffectFileNameList;
  }

  public CommonElement GetElement()
  {
    if (!this.mUnitElement.HasValue)
    {
      BL.Skill[] array = ((IEnumerable<BL.Skill>) this.mMyUnitData.duelSkills).Where<BL.Skill>((Func<BL.Skill, bool>) (skill => ((IEnumerable<BattleskillEffect>) skill.skill.Effects).Any<BattleskillEffect>((Func<BattleskillEffect, bool>) (ef => ef.effect_logic.Enum == BattleskillEffectLogicEnum.invest_element)))).ToArray<BL.Skill>();
      this.mUnitElement = array.Length == 0 ? new CommonElement?(CommonElement.none) : new CommonElement?(array[0].skill.element);
    }
    return this.mUnitElement.Value;
  }

  private void initAttackElements(DuelResult result)
  {
    this.mAttackElements = new CommonElement[((IEnumerable<BL.DuelTurn>) result.turns).Count<BL.DuelTurn>()];
    Func<NGDuelUnit, BL.DuelTurn, CommonElement> func = (Func<NGDuelUnit, BL.DuelTurn, CommonElement>) ((duelUnit, turn) =>
    {
      if (turn.attackStatus.magicBullet != null)
        return turn.attackStatus.magicBullet.element;
      CommonElement? overwriteElement = turn.attackStatus.GetOverwriteElement();
      return overwriteElement.HasValue ? overwriteElement.Value : duelUnit.GetElement();
    });
    int index = 0;
    foreach (BL.DuelTurn turn in result.turns)
    {
      this.mAttackElements[index] = CommonElement.none;
      if (result.isPlayerAttack == this.mIsPlayer && turn.isAtackker)
        this.mAttackElements[index] = func(this, turn);
      else if (result.isPlayerAttack != this.mIsPlayer && !turn.isAtackker)
        this.mAttackElements[index] = func(this, turn);
      ++index;
    }
  }

  public CommonElement GetAttackElement(int turnCount) => this.mAttackElements[turnCount];

  public DuelElementTrailEffect GetElementTrailEffect()
  {
    return this.GetElementTrailEffect(this.mThisTurnCount);
  }

  public DuelElementTrailEffect GetElementTrailEffect(int turnCount)
  {
    CommonElement attackElm = this.GetAttackElement(turnCount);
    if (!this.mElementsTrailEffectDic.ContainsKey(attackElm))
    {
      DuelElementTrailEffect elementTrailEffect = ((IEnumerable<DuelElementTrailEffect>) MasterData.DuelElementTrailEffectList).FirstOrDefault<DuelElementTrailEffect>((Func<DuelElementTrailEffect, bool>) (x => x.kind.Enum == this.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.kind.Enum && x.element == attackElm));
      this.mElementsTrailEffectDic.Add(attackElm, elementTrailEffect);
    }
    return this.mElementsTrailEffectDic[attackElm];
  }

  public DuelElementBulletEffect GetElementBulletEffect(string bullet_name)
  {
    return this.GetElementBulletEffect(bullet_name, this.mThisTurnCount);
  }

  public DuelElementBulletEffect GetElementBulletEffect(string bullet_name, int turnCount)
  {
    CommonElement attackElm = this.GetAttackElement(turnCount);
    if (!this.mElementsBulletEffectDic.ContainsKey(attackElm))
    {
      DuelElementBulletEffect elementBulletEffect = ((IEnumerable<DuelElementBulletEffect>) MasterData.DuelElementBulletEffectList).FirstOrDefault<DuelElementBulletEffect>((Func<DuelElementBulletEffect, bool>) (x => x.bullet_prefab_name == bullet_name && x.element == attackElm));
      this.mElementsBulletEffectDic.Add(attackElm, elementBulletEffect);
    }
    return this.mElementsBulletEffectDic[attackElm];
  }

  public DuelElementHitEffect GetElementHitEffect(string hit_effect_name)
  {
    return this.GetElementHitEffect(hit_effect_name, this.mThisTurnCount);
  }

  public DuelElementHitEffect GetElementHitEffect(string hit_effect_name, int turnCount)
  {
    CommonElement attackElm = this.GetAttackElement(turnCount);
    return ((IEnumerable<DuelElementHitEffect>) MasterData.DuelElementHitEffectList).FirstOrDefault<DuelElementHitEffect>((Func<DuelElementHitEffect, bool>) (x => x.original_effect_name == hit_effect_name && x.element == attackElm));
  }

  public int jobId => this.mMyUnitData.job.ID;

  public NGDuelUnit Enemy => this.mEnemyUnit;

  public List<BL.DuelTurn> thisTurns => this.mThisTurns;

  public BL.DuelTurn thisTurn
  {
    get
    {
      return this.mThisTurns.Count<BL.DuelTurn>() <= this.mThisTurnsCurrent ? (BL.DuelTurn) null : this.mThisTurns[this.mThisTurnsCurrent];
    }
  }

  public void incrementThisTurnsCurrent()
  {
    this.mThisTurnsCurrent = Math.Min(this.mThisTurns.Count<BL.DuelTurn>() - 1, this.mThisTurnsCurrent + 1);
  }

  public bool isAnyThisTurn => this.mThisTurns.Any<BL.DuelTurn>();

  public List<BL.DuelTurn> thisTurnDamages => this.mThisTurnDamages;

  public BL.DuelTurn thisTurnDamage
  {
    get
    {
      return this.mThisTurnDamages.Count<BL.DuelTurn>() <= this.mDispDamageCount ? (BL.DuelTurn) null : this.mThisTurnDamages[this.mDispDamageCount];
    }
  }

  public void incrementDispDamageCount()
  {
    this.mDispDamageCount = Math.Min(this.mThisTurnDamages.Count<BL.DuelTurn>() - 1, this.mDispDamageCount + 1);
  }

  public NGDuelUnit.Status currSts => this.currentStatus;

  public bool useDistance => this.mDuelManager.useDistance;

  public void TimeReset() => this.mTurnSpentTime = 0.0f;

  public NGDuelManager manager => this.mDuelManager;

  public bool isPlayVoice
  {
    get => Object.op_Inequality((Object) this.mSM, (Object) null) && this.mUnitVoicePattern != null;
  }

  public virtual IEnumerator Initialize(unitInfomation ui)
  {
    NGDuelUnit ngDuelUnit1 = this;
    ngDuelUnit1.mMyUnit = ((Component) ngDuelUnit1).GetComponent<NGDuelUnit>();
    ngDuelUnit1.mPlayerUI = ui.p;
    ngDuelUnit1.mEnemyUnit = ui.enemy;
    ngDuelUnit1.mIsPlayer = ui.isplayer;
    ngDuelUnit1.mMyUnitData = ui.bu;
    ngDuelUnit1.mMagicBullet = ui.mb;
    ngDuelUnit1.beforeAilmentEffectIDs = ui.beforeAilmentEffectIDs;
    ngDuelUnit1.isRemovedSleepEffect = false;
    ngDuelUnit1.mInitPos = ui.trs;
    ngDuelUnit1.mSupport = ui.support;
    ngDuelUnit1.mSupportHitIncr = ui.supportHitIncr;
    ngDuelUnit1.mSupportEvasionIncr = ui.supportEvasionIncr;
    ngDuelUnit1.mSupportCriticalIncr = ui.supportCriticalIncr;
    ngDuelUnit1.mSupportCriticalEvasionIncr = ui.supportCriticalEvasionIncr;
    ngDuelUnit1.mDuelManager = ui.mng;
    if (Object.op_Inequality((Object) ui.root3d, (Object) null))
      ngDuelUnit1.mRoot3D = ui.root3d;
    else
      ngDuelUnit1.mRoot3D = GameObject.Find("Duel3DRoot");
    if (ui.bu.unit.character.category == UnitCategory.enemy)
      ngDuelUnit1.mIsMonster = true;
    ((Component) ngDuelUnit1).transform.localScale = new Vector3(ngDuelUnit1.mMyUnitData.unit.duel_model_scale, ngDuelUnit1.mMyUnitData.unit.duel_model_scale, ngDuelUnit1.mMyUnitData.unit.duel_model_scale);
    ngDuelUnit1.setupBodyMeshEffect();
    SkillMetamorphosis metamorphosis1 = ui.metamorphosis;
    int metamorphosisId1 = metamorphosis1 != null ? metamorphosis1.metamorphosis_id : 0;
    string duelAnimatorControllerName = ngDuelUnit1.mMyUnitData.playerUnit.getDuelAnimatorControllerName(metamorphosisId1);
    ngDuelUnit1.mConfig = ((IEnumerable<DuelDuelConfig>) MasterData.DuelDuelConfigList).FirstOrDefault<DuelDuelConfig>((Func<DuelDuelConfig, bool>) (x => x.controller_name == duelAnimatorControllerName));
    if (ngDuelUnit1.mConfig == null)
      ngDuelUnit1.mConfig = ((IEnumerable<DuelDuelConfig>) MasterData.DuelDuelConfigList).FirstOrDefault<DuelDuelConfig>((Func<DuelDuelConfig, bool>) (x => x.controller_name == "Anim_dummy"));
    ngDuelUnit1.mDuelTime = new DuelTime(ngDuelUnit1.mConfig);
    ngDuelUnit1.mMoveType = ngDuelUnit1.mConfig.move_type;
    ngDuelUnit1.initAttackElements(ngDuelUnit1.mDuelManager.mDuelResult);
    if (ngDuelUnit1.mMyUnitData.unit.magic_warrior_flag && ngDuelUnit1.mMagicBullet != null)
    {
      ngDuelUnit1.mMyReach = 9f;
      ngDuelUnit1.mOrgReach = ngDuelUnit1.mConfig.myReach;
    }
    else
      ngDuelUnit1.mMyReach = ui.range <= 1 ? (ngDuelUnit1.mOrgReach = ngDuelUnit1.mConfig.myReach) : (ngDuelUnit1.mOrgReach = 9f);
    ngDuelUnit1.mCharacterAnimator = ((Component) ngDuelUnit1).gameObject.GetComponent<Animator>();
    IEnumerator e = ngDuelUnit1.createHelm();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ngDuelUnit1.createArmor();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ngDuelUnit1.createShield();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ngDuelUnit1.createWeapon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ngDuelUnit1.createVehicle();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ngDuelUnit1.loadDefaultAnimatorController();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if ((double) ngDuelUnit1.mInitPos.position.x < 0.0)
      ngDuelUnit1.mDirToEnemy = 1f;
    e = ngDuelUnit1.CreateCutinObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ngDuelUnit1.setDuelSupport();
    ngDuelUnit1.setDuelShadow();
    if (Object.op_Equality((Object) ((Component) ngDuelUnit1).gameObject.GetComponentInChildren<clipEffectPlayer>(), (Object) null))
      ((Component) ngDuelUnit1.mCharacterAnimator).gameObject.AddComponent<clipEffectPlayer>();
    e = ngDuelUnit1.loadMissileWeaponPrefab(ui.range);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ngDuelUnit1.mDuelTime == null)
    {
      Debug.LogWarning((object) "[NGDuelUnit] at Initialize mDuelTime create failed.");
      ngDuelUnit1.mDuelTime = new DuelTime();
    }
    ngDuelUnit1.adjustInitPos();
    ngDuelUnit1.mBipTransform = ((Component) ngDuelUnit1).gameObject.transform.GetChildInFind("Bip");
    if (ngDuelUnit1.mMyUnitData.hp < ngDuelUnit1.mMyUnitData.playerUnit.total_hp / 2)
    {
      ngDuelUnit1.setTrigToAnimator("to_damagewait");
      ngDuelUnit1.mIsDamageWait = true;
    }
    ngDuelUnit1.mSM = Singleton<NGSoundManager>.GetInstance();
    NGDuelUnit ngDuelUnit2 = ngDuelUnit1;
    UnitUnit unit = ngDuelUnit1.mMyUnitData.unit;
    SkillMetamorphosis metamorphosis2 = ngDuelUnit1.mMyUnitData.metamorphosis;
    int metamorphosisId2 = metamorphosis2 != null ? metamorphosis2.metamorphosis_id : 0;
    UnitVoicePattern voicePattern = unit.getVoicePattern(metamorphosisId2);
    ngDuelUnit2.mUnitVoicePattern = voicePattern;
    ngDuelUnit1.critical_attack1 = string.Format("{0}.critical_attack1", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.nomal_attack1_last = string.Format("{0}.nomal_attack1_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.nomal_attack_last = string.Format("{0}.nomal_attack_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.dual_single_attack = string.Format("{0}.attack_single", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.critical_attackS_last = string.Format("{0}.critical_attackS_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.critical_attack_last = string.Format("{0}.critical_attack_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.dual_single_critical = string.Format("{0}.attackS_single", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.nomal_attack_magic_last = string.Format("{0}.nomal_attack_magic_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.critical_attack_magic_last = string.Format("{0}.critical_attack_magic_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.magic_single = string.Format("{0}.magic_shingle", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.magicS_single = string.Format("{0}.magicS_shingle", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.skill_ss5_attack4_last = string.Format("{0}.skill_ss5_attack4_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.skill_attack1_last = string.Format("{0}.skill_attack1_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.skill_attack_last = string.Format("{0}.skill_attack_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.skill_attack_throw_last = string.Format("{0}.skill_attack_throw_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.skill_ss5_attack_throw4_last = string.Format("{0}.skill_ss5_attack_throw4_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.skill_ss5_attack_throw5_last = string.Format("{0}.skill_ss5_attack_throw5_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.critical_attack_throw_last = string.Format("{0}.critical_attack_throw_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.nomal_attack_throw_last = string.Format("{0}.nomal_attack_throw_last", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.skill_attack = string.Format("{0}.skill_attack", (object) ngDuelUnit1.mCharacterAnimator.GetLayerName(0));
    ngDuelUnit1.trigger_option_attack = string.Empty;
    ngDuelUnit1.trigger_dual_single_option_attack = string.Empty;
    ngDuelUnit1.trigger_option_attack_throw = string.Empty;
    ngDuelUnit1.trigger_dual_single_option_attack_throw = string.Empty;
    if (ui.mb?.attackMethod != null)
      ngDuelUnit1.trigger_option_attack = ui.mb.attackMethod.motionKey;
    else if (ui.weapon != null)
      ngDuelUnit1.trigger_option_attack = ui.weapon.attackMethod.motionKey;
    if (!ngDuelUnit1.trigger_option_attack.isEmptyOrWhitespace())
      ngDuelUnit1.trigger_dual_single_option_attack = "dual_" + ngDuelUnit1.trigger_option_attack;
    if (!ngDuelUnit1.trigger_option_attack.isEmptyOrWhitespace())
    {
      ngDuelUnit1.trigger_option_attack_throw = ngDuelUnit1.trigger_option_attack + "_throw";
      bool flag = false;
      foreach (AnimatorControllerParameter parameter in ngDuelUnit1.mCharacterAnimator.parameters)
      {
        if (parameter.name.Equals(ngDuelUnit1.trigger_option_attack_throw))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        ngDuelUnit1.trigger_option_attack_throw = ngDuelUnit1.trigger_option_attack;
    }
    if (!ngDuelUnit1.trigger_dual_single_option_attack.isEmptyOrWhitespace())
    {
      ngDuelUnit1.trigger_dual_single_option_attack_throw = ngDuelUnit1.trigger_dual_single_option_attack + "_throw";
      bool flag = false;
      foreach (AnimatorControllerParameter parameter in ngDuelUnit1.mCharacterAnimator.parameters)
      {
        if (parameter.name.Equals(ngDuelUnit1.trigger_dual_single_option_attack_throw))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        ngDuelUnit1.trigger_dual_single_option_attack_throw = ngDuelUnit1.trigger_dual_single_option_attack;
    }
    ngDuelUnit1.mInitialized = true;
  }

  public void SetAilmentEffect()
  {
    if (this.beforeAilmentEffectIDs == null)
      return;
    foreach (string name in ((IEnumerable<int>) this.beforeAilmentEffectIDs).Where<int>((Func<int, bool>) (x => MasterData.BattleskillAilmentEffect.ContainsKey(x))).Select<int, string>((Func<int, string>) (x => MasterData.BattleskillAilmentEffect[x].duel_effect_name)))
      Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(name, this.mBipTransform);
  }

  private void OnDestroy()
  {
    foreach (Object mRemoveObject in this.mRemoveObjects)
      Object.Destroy(mRemoveObject);
  }

  private void LateUpdate()
  {
    if (!this.mInitialized || this.mDuelManager.isWait || this.currentStatus == NGDuelUnit.Status.ST_DEATH)
      return;
    this.adjustShadowObj();
  }

  private void Update()
  {
    if (!this.mInitialized || this.mDuelManager.isWait || this.currentStatus == NGDuelUnit.Status.ST_DEATH)
      return;
    this.mTurnSpentTime += Time.deltaTime;
    AnimatorClipInfo[] animatorClipInfo = this.mCharacterAnimator.GetCurrentAnimatorClipInfo(0);
    AnimatorStateInfo animatorStateInfo = this.mCharacterAnimator.GetCurrentAnimatorStateInfo(0);
    if (!((AnimatorStateInfo) ref animatorStateInfo).IsName(this.critical_attack1))
    {
      if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.nomal_attack1_last))
      {
        if (!this.mIsLastAttack && ((Object) ((AnimatorClipInfo) ref animatorClipInfo[0]).clip).name.Equals("attack2"))
          this.mIsLastAttack = true;
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.nomal_attack_last) || ((AnimatorStateInfo) ref animatorStateInfo).IsName(this.dual_single_attack))
      {
        if (!this.mIsLastAttack)
          this.mIsLastAttack = true;
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.critical_attackS_last) || ((AnimatorStateInfo) ref animatorStateInfo).IsName(this.dual_single_critical))
      {
        if (!this.mIsLastAttack)
        {
          this.mIsLastAttack = true;
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.critical_attack_last))
      {
        if (!this.mIsLastAttack && ((Object) ((AnimatorClipInfo) ref animatorClipInfo[0]).clip).name.EndsWith("attackS"))
        {
          this.mIsLastAttack = true;
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.nomal_attack_magic_last) || ((AnimatorStateInfo) ref animatorStateInfo).IsName(this.magic_single))
      {
        if (!this.mIsLastAttack)
          this.mIsLastAttack = true;
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.critical_attack_magic_last) || ((AnimatorStateInfo) ref animatorStateInfo).IsName(this.magicS_single))
      {
        if (!this.mIsLastAttack)
        {
          this.mIsLastAttack = true;
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.skill_ss5_attack4_last))
      {
        if (!this.mIsLastAttack && ((Object) ((AnimatorClipInfo) ref animatorClipInfo[0]).clip).name.EndsWith("attackS"))
        {
          if ((double) this.mMyReach < 9.0)
          {
            this.mIsLastAttack = true;
          }
          else
          {
            if (this.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.kind.Enum == GearKindEnum.bow)
              this.mDuelManager.ActAttackBeginCamera();
            this.StartCoroutine(this.delayLastAttackOn(0.5f));
          }
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
        else if (!this.mIsLastAttack && ((Object) ((AnimatorClipInfo) ref animatorClipInfo[0]).clip).name.EndsWith("attack1"))
        {
          if ((double) this.mMyReach < 9.0)
          {
            this.mIsLastAttack = true;
          }
          else
          {
            if (this.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.kind.Enum == GearKindEnum.bow)
              this.StartCoroutine(this.bowSuiseiLastCameraWork());
            this.mIsLastAttack = true;
          }
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.skill_attack1_last))
      {
        if (!this.mIsLastAttack && ((Object) ((AnimatorClipInfo) ref animatorClipInfo[0]).clip).name.Equals("attack2"))
          this.mIsLastAttack = true;
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.skill_attack_last))
      {
        if (!this.mIsLastAttack)
        {
          this.mIsLastAttack = true;
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.skill_attack_throw_last))
      {
        if (!this.mIsLastAttack)
        {
          this.mIsLastAttack = true;
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.skill_ss5_attack_throw4_last))
      {
        if (!this.mIsLastAttack)
        {
          this.mIsLastAttack = true;
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.skill_ss5_attack_throw5_last))
      {
        if (!this.mIsLastAttack)
        {
          this.mIsLastAttack = true;
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.critical_attack_throw_last))
      {
        if (!this.mIsLastAttack)
        {
          this.mIsLastAttack = true;
          if (this.isPlayVoice)
            this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        }
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.nomal_attack_throw_last))
      {
        if (!this.mIsLastAttack)
          this.mIsLastAttack = true;
      }
      else if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this.skill_attack) && this.isPlayVoice && !this.mAttackVoicePlaying)
      {
        this.mSM.playVoiceByID(this.mUnitVoicePattern, 28);
        this.mAttackVoicePlaying = true;
      }
    }
    if (this.currentStatus == NGDuelUnit.Status.ST_READY_DODGE)
    {
      float x = ((Component) this.mMyUnit).gameObject.transform.position.x;
      float dodgeDistance = this.mDuelTime.dodgeDistance;
      float e = this.backward(x, dodgeDistance);
      float dodgeTime = this.mDuelTime.dodgeTime;
      this.mLinearX = new Linear(x, e, dodgeTime);
      this.currentStatus = NGDuelUnit.Status.ST_DODGE;
    }
    if (this.currentStatus == NGDuelUnit.Status.ST_DODGE)
    {
      this.mLinearX.Update(Time.deltaTime);
      if (!this.mLinearX.isEnd)
      {
        Vector3 position = this.baseGameObject.transform.position;
        position.x = this.mLinearX.value;
        this.baseGameObject.transform.position = position;
      }
      else
      {
        Vector3 position = this.baseGameObject.transform.position;
        position.x = this.mLinearX.value;
        this.baseGameObject.transform.position = position;
        this.mIsDodging = false;
        this.mToNextTurn = true;
        this.currentStatus = NGDuelUnit.Status.ST_WAIT;
      }
    }
    if (!this.isAnyThisTurn)
      return;
    if (this.currentStatus == NGDuelUnit.Status.ST_READY_BS)
    {
      float x = ((Component) this.mMyUnit).gameObject.transform.position.x;
      float e = this.backward(((Component) this.Enemy).gameObject.transform.position.x, Mathf.Abs(this.mInitPos.position.x) * 2f);
      float bsDuration = this.mDuelTime.bsDuration;
      this.mLinearX = new Linear(x, e, bsDuration);
      this.mLinearY = new Linear(((Component) this.mMyUnit).gameObject.transform.position.y, ((Component) this.mInitPos).transform.position.y, bsDuration);
      this.mOrigSmoothTime = this.mDuelManager.GetCameraSmoothTime();
      this.mDuelManager.SetCameraSmoothTime(0.075f);
      this.currentStatus = NGDuelUnit.Status.ST_BACKSTEP;
    }
    if (this.currentStatus == NGDuelUnit.Status.ST_BACKSTEP)
    {
      if ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < (double) this.mDuelTime.bsDelay)
        return;
      this.mLinearX.Update(Time.deltaTime);
      this.mLinearY.Update(Time.deltaTime);
      if (!this.mLinearX.isEnd)
      {
        Vector3 position = this.baseGameObject.transform.position;
        position.x = this.mLinearX.value;
        if (!this.mIsPlayer)
          position.y = this.mLinearY.value;
        this.baseGameObject.transform.position = position;
      }
      else
      {
        Vector3 position = this.baseGameObject.transform.position;
        position.x = this.mLinearX.value;
        position.y = ((Component) this.mInitPos).transform.position.y;
        this.baseGameObject.transform.position = position;
      }
      if (this.mLinearX.isEnd)
      {
        Vector3 position = this.baseGameObject.transform.position;
        position.x = this.mLinearX.value;
        position.y = ((Component) this.mInitPos).transform.position.y;
        this.baseGameObject.transform.position = position;
        if (this.mInvokeDuelSkill != null)
        {
          foreach (BL.DuelTurn mThisTurn in this.mThisTurns)
          {
            if (mThisTurn.dispDrainDamage <= 0)
              this.mToNextTurn = true;
            else if (!mThisTurn.isHit)
              this.mToNextTurn = true;
          }
        }
        else
          this.mToNextTurn = true;
        this.mDuelManager.SetCameraSmoothTime(this.mOrigSmoothTime);
        this.currentStatus = NGDuelUnit.Status.ST_NONE;
      }
    }
    int currentStatus = (int) this.currentStatus;
    if (this.mToNextTurn && this.Enemy.mToNextTurn && !this.mNowPlayKoyu && !this.Enemy.mNowPlayKoyu && this.mThisTurns.Any<BL.DuelTurn>())
    {
      bool flag = false;
      foreach (BL.DuelTurn mThisTurn in this.mThisTurns)
        flag |= mThisTurn.isDieDefender();
      if (flag)
      {
        if ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime >= 1.0)
        {
          this.mTurnSpentTime = 0.0f;
          this.mToNextTurn = false;
          this.Enemy.mToNextTurn = false;
          this.StartCoroutine("execEndTurn");
        }
      }
      else
      {
        this.mTurnSpentTime = 0.0f;
        this.mToNextTurn = false;
        this.Enemy.mToNextTurn = false;
        this.StartCoroutine("execEndTurn");
      }
    }
    if ((double) this.mTurnSpentTime <= 60.0)
      return;
    this.mTurnSpentTime = 0.0f;
    Debug.LogWarning((object) "NGDuelUnit this turn timed out");
    if (!this.Enemy.mPlayerUI.isHpDamaged)
    {
      foreach (BL.DuelTurn mThisTurn in this.mThisTurns)
      {
        this.Enemy.mPlayerUI.Damaged(mThisTurn.damage, new int?(mThisTurn.defenderDrainDamage));
        this.StartCoroutine(this.Enemy.playDamage(mThisTurn, mThisTurn.damage, mThisTurn.dispDamage, mThisTurn.defenderDrainDamage, mThisTurn.defenderDispDrainDamage, this.Enemy.mDefenseDuelSkills, mThisTurn.invokeSkillExtraInfo, 1f, mThisTurn.isDieDefender(), mThisTurn.isCritical, isHit: mThisTurn.isHit));
      }
    }
    this.StartCoroutine("execEndTurn");
  }

  public void setDefenderSkills(BL.Skill[] skills, bool bResetDisp)
  {
    this.resolveDuelSkills(skills, bResetDisp);
  }

  private void resolveDuelSkills(BL.Skill[] skills, bool bResetDisp)
  {
    this.mDefenseDuelSkills = new List<BL.Skill>();
    this.mInvokeDuelSkill = (BL.Skill) null;
    List<BL.Skill> skillList = (List<BL.Skill>) null;
    if (skills.Length != 0)
    {
      skillList = new List<BL.Skill>(skills.Length);
      foreach (BL.Skill skill in skills)
      {
        BattleskillGenre? genre1 = skill.genre1;
        BattleskillGenre battleskillGenre1 = BattleskillGenre.attack;
        if (genre1.GetValueOrDefault() == battleskillGenre1 & genre1.HasValue)
        {
          this.mInvokeDuelSkill = skill;
          skillList.Add(skill);
        }
        else
        {
          genre1 = skill.genre1;
          BattleskillGenre battleskillGenre2 = BattleskillGenre.defense;
          if (genre1.GetValueOrDefault() == battleskillGenre2 & genre1.HasValue)
            this.mDefenseDuelSkills.Add(skill);
          else
            skillList.Add(skill);
        }
      }
    }
    this.mPlayerUI.skillInvoke(skillList?.ToArray(), bResetDisp);
  }

  public IEnumerator startAttack(List<BL.DuelTurn> turns, int turn_count)
  {
    NGDuelUnit ngDuelUnit = this;
    BL.DuelTurn turn1 = turns[0];
    ngDuelUnit.InitTurn(turns, turn_count);
    bool isCritical = false;
    ngDuelUnit.mEnemyUnit.isInvalidDodgeMotion = false;
    bool bResetDisp = true;
    foreach (BL.DuelTurn turn in turns)
    {
      ngDuelUnit.resolveDuelSkills(turn.invokeDuelSkills, bResetDisp);
      ngDuelUnit.Enemy.setDefenderSkills(turn.invokeDefenderDuelSkills, bResetDisp);
      bResetDisp = false;
      if (((IEnumerable<BL.Skill>) turn.invokeDefenderDuelSkills).Count<BL.Skill>() > 0)
      {
        BL.Skill[] skillArray = turn.invokeDefenderDuelSkills;
        for (int index = 0; index < skillArray.Length; ++index)
        {
          BL.Skill ids = skillArray[index];
          if (ids.skill.haveKoyuDuelEffect)
          {
            if (!turn.isHit)
            {
              BattleskillGenre? genre1 = ids.skill.genre1;
              BattleskillGenre battleskillGenre = BattleskillGenre.defense;
              if (genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue)
                continue;
            }
            IEnumerator e = ngDuelUnit.Enemy.execKoyuSkill(ids);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        skillArray = (BL.Skill[]) null;
      }
      isCritical |= turn.isCritical;
      ngDuelUnit.mEnemyUnit.isInvalidDodgeMotion |= turn.isHit;
    }
    if (ngDuelUnit.mInvokeDuelSkill != null && turn1.attackCount <= 1)
    {
      if (ngDuelUnit.mInvokeDuelSkill.skill.haveKoyuDuelEffect)
      {
        if (turn1.suiseiResults != null && turn1.suiseiResults.Count > 1)
          ngDuelUnit.mIsSuisei = true;
        ngDuelUnit.StartCoroutine(ngDuelUnit.execKoyuSkill(ngDuelUnit.mInvokeDuelSkill));
      }
      else if (turn1.suiseiResults != null && turn1.suiseiResults.Count > 1)
      {
        ngDuelUnit.mIsSuisei = true;
        if ((double) ngDuelUnit.mMyReach < 9.0 || ngDuelUnit.IsEquippedLongRangeGear())
          ngDuelUnit.StartCoroutine(ngDuelUnit.execSuisei(turn_count));
        else
          ngDuelUnit.StartCoroutine(ngDuelUnit.execSuiseiRemote(turn_count));
      }
      else
      {
        if ((double) ngDuelUnit.mMyReach < 9.0 || ngDuelUnit.IsEquippedLongRangeGear())
          ngDuelUnit.StartCoroutine(ngDuelUnit.execSkillAttack(turn_count, ngDuelUnit.mInvokeDuelSkill));
        else
          ngDuelUnit.StartCoroutine(ngDuelUnit.execSkillRemoteAttack(turn_count, ngDuelUnit.mInvokeDuelSkill));
        ngDuelUnit.magicCostPay(turn1);
      }
    }
    else if (isCritical)
    {
      if (ngDuelUnit.mMyUnitData.unit.magic_warrior_flag && ngDuelUnit.mMagicBullet != null && ngDuelUnit.mMyUnitData.weapon.gear.kind.Enum != GearKindEnum.gun && ngDuelUnit.mMyUnitData.weapon.gear.kind.Enum != GearKindEnum.staff)
        ngDuelUnit.StartCoroutine(ngDuelUnit.execCriticalMagic(turn_count, turn1.isDualSingleAttack));
      else if ((double) ngDuelUnit.mMyReach < 9.0 || ngDuelUnit.IsEquippedLongRangeGear())
        ngDuelUnit.StartCoroutine(ngDuelUnit.execCritical(turn_count, turn1.isDualSingleAttack));
      else
        ngDuelUnit.StartCoroutine(ngDuelUnit.execCriticalRemote(turn_count, turn1.isDualSingleAttack));
      ngDuelUnit.magicCostPay(turn1);
    }
    else
    {
      if (ngDuelUnit.mMyUnitData.unit.magic_warrior_flag && ngDuelUnit.mMagicBullet != null && ngDuelUnit.mMyUnitData.weapon.gear.kind.Enum != GearKindEnum.gun && ngDuelUnit.mMyUnitData.weapon.gear.kind.Enum != GearKindEnum.staff)
        ngDuelUnit.StartCoroutine(ngDuelUnit.execNormalMagicAttack(turn_count, turn1.isDualSingleAttack));
      else if ((double) ngDuelUnit.mMyReach < 9.0 || ngDuelUnit.IsEquippedLongRangeGear())
        ngDuelUnit.StartCoroutine(ngDuelUnit.execNormalAttack(turn_count, turn1.isDualSingleAttack));
      else
        ngDuelUnit.StartCoroutine(ngDuelUnit.execNormalRemoteAttack(turn_count, turn1.isDualSingleAttack));
      ngDuelUnit.magicCostPay(turn1);
    }
  }

  private void magicCostPay(BL.DuelTurn turn1)
  {
    if (turn1.counterDamage <= 0 || this.mMagicBullet == null)
      return;
    this.mPlayerUI.Damaged(turn1.counterDamage);
  }

  private bool IsEquippedLongRangeGear()
  {
    GearGear weaponGearOrInitial = this.mMyUnitData.playerUnit.equippedWeaponGearOrInitial;
    switch (weaponGearOrInitial.kind.Enum)
    {
      case GearKindEnum.bow:
      case GearKindEnum.gun:
      case GearKindEnum.staff:
        return true;
      default:
        return weaponGearOrInitial.attack_type == GearAttackType.magic;
    }
  }

  private void InitTurn(List<BL.DuelTurn> turns, int turn_count)
  {
    this.mIsSuisei = false;
    this.mIsLastAttack = false;
    this.mIsFireFirstSuiseiMadan = false;
    this.mRemoteSuiseiCameraFlag = false;
    this.mTurnSpentTime = 0.0f;
    this.mToNextTurn = false;
    this.mToExecEndTurn = false;
    this.mPlayerUI.isHpDamaged = false;
    this.mNowPlayKoyu = false;
    this.mPlaiedKoyu = false;
    this.mIsDodging = false;
    this.mAttackVoicePlaying = false;
    this.mFirstAttackDone = false;
    this.mSuiseiCount = 0;
    this.mAttackMotionCount = 0;
    this.TimeReset();
    this.mThisTurns = turns;
    this.mThisTurnsCurrent = 0;
    this.mThisTurnCount = turn_count;
    this.mMagicBullet = this.mThisTurns[0].attackStatus.magicBullet;
    if (this.mMyUnitData.unit.magic_warrior_flag)
      this.mMyReach = this.mMagicBullet != null ? 9f : this.mOrgReach;
    this.mThisTurnDamages.Clear();
    foreach (BL.DuelTurn turn in turns)
      this.mThisTurnDamages.Add(turn);
    this.mDispDamageCount = 0;
    if (Object.op_Equality((Object) this.mEnemyUnit, (Object) null))
    {
      this.mEnemyUnit = this.mDuelManager.enemyUnit;
      if (Object.op_Equality((Object) null, (Object) this.mEnemyUnit))
        Debug.LogError((object) ("NGDuelUnit cannot get EnemyUnit(My Unit Id = " + (object) this.mMyUnitData.unitId + ")"));
    }
    this.mEnemyUnit.mToNextTurn = false;
    this.mEnemyUnit.TimeReset();
    this.mEnemyUnit.mThisTurns.Clear();
    this.mEnemyUnit.mThisTurnsCurrent = 0;
    this.mEnemyUnit.mThisTurnCount = turn_count;
    this.mEnemyUnit.mThisTurnDamages.Clear();
    this.mEnemyUnit.mDispDamageCount = 0;
    if (Object.op_Inequality((Object) this.mDuelCINew, (Object) null))
      ((Component) this.mDuelCINew).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) this.mDuelCI, (Object) null))
      ((Component) this.mDuelCI).gameObject.SetActive(false);
    foreach (GameObject mBullet in this.mBullets)
    {
      if (Object.op_Inequality((Object) null, (Object) mBullet))
        mBullet.SetActive(false);
    }
  }

  public int playWinVoice()
  {
    return this.isPlayVoice ? this.mSM.playVoiceByID(this.UnitVoicePattern, 60, 2) : -1;
  }

  private bool isShotMadan
  {
    get
    {
      return this.thisTurn != null && (this.thisTurn.attackStatus == null ? (this.thisTurn.isAtackker ? this.manager.mDuelResult.attackAttackStatus.magicBullet : this.manager.mDuelResult.defenseAttackStatus.magicBullet) : this.thisTurn.attackStatus.magicBullet) != null;
    }
  }

  public void SetCleanUpDuelSkillEffect()
  {
    if (Object.op_Inequality((Object) ((Component) this).gameObject.transform.parent, (Object) this.oldUnitParent) && Object.op_Inequality((Object) this.oldUnitParent, (Object) null))
    {
      ((Component) this).gameObject.transform.parent = this.oldUnitParent;
      ((Component) this).gameObject.transform.localPosition = this.oldCharactorTransLocalPos;
      ((Component) this).gameObject.transform.localRotation = this.oldCharactorTransLocalRot;
    }
    if (Object.op_Inequality((Object) null, (Object) this.playingRyuseiEf))
    {
      this.playingRyuseiEf.removeEffects();
      Ryusei_100111_ef component = ((Component) this).gameObject.GetComponent<Ryusei_100111_ef>();
      if (Object.op_Inequality((Object) component, (Object) null))
        Object.Destroy((Object) component);
      if (Object.op_Inequality((Object) this.mDuelManager, (Object) null))
        this.mDuelManager.EndKoyuCamera();
    }
    this.oldUnitParent = (Transform) null;
    this.playingRyuseiEf = (Ryusei_100111_ef) null;
  }

  private IEnumerator execNormalAttack(int turn_count, bool isDualSingleAttack)
  {
    NGDuelUnit ngDuelUnit = this;
    if (turn_count == 0)
    {
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTime1);
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.moveToAttack(ngDuelUnit.baseGameObject));
    string trigname = !isDualSingleAttack ? (string.IsNullOrEmpty(ngDuelUnit.trigger_option_attack) ? "nomal_attack" : ngDuelUnit.trigger_option_attack) : (string.IsNullOrEmpty(ngDuelUnit.trigger_dual_single_option_attack) ? "dual_single_attack" : ngDuelUnit.trigger_dual_single_option_attack);
    ngDuelUnit.setTrigToAnimator(trigname);
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTime2);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  private IEnumerator execNormalRemoteAttack(int turn_count, bool isDualSingleAttack)
  {
    NGDuelUnit ngDuelUnit = this;
    if (turn_count == 0)
    {
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTime1);
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackBeginOffsetTime);
    string trigname = !isDualSingleAttack ? (string.IsNullOrEmpty(ngDuelUnit.trigger_option_attack) ? "to_nomal_attack_throw" : ngDuelUnit.trigger_option_attack_throw) : (string.IsNullOrEmpty(ngDuelUnit.trigger_dual_single_option_attack_throw) ? "dual_single_attack_throw" : ngDuelUnit.trigger_dual_single_option_attack_throw);
    ngDuelUnit.setTrigToAnimator(trigname);
    yield return (object) new WaitForSeconds(3f);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  private IEnumerator execNormalMagicAttack(int turn_count, bool isDualSingleAttack)
  {
    NGDuelUnit ngDuelUnit = this;
    if (turn_count == 0)
    {
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTime1);
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackBeginOffsetTime);
    string trigname = !isDualSingleAttack ? (string.IsNullOrEmpty(ngDuelUnit.trigger_option_attack) ? "to_nomal_attack_magic" : ngDuelUnit.trigger_option_attack) : (string.IsNullOrEmpty(ngDuelUnit.trigger_dual_single_option_attack) ? "dual_shingle_magic" : ngDuelUnit.trigger_dual_single_option_attack);
    ngDuelUnit.setTrigToAnimator(trigname);
    yield return (object) new WaitForSeconds(3f);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  private IEnumerator execCritical(int turn_count, bool isDualSingleAttack)
  {
    NGDuelUnit ngDuelUnit = this;
    if ((double) ngDuelUnit.mDuelTime.criticalCutinWaitTime > 0.0 && ngDuelUnit.PlayCriticalCutin())
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.criticalCutinWaitTime);
    if (turn_count == 0)
    {
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTime1);
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.moveToAttack(ngDuelUnit.baseGameObject));
    string trigname = !isDualSingleAttack ? (string.IsNullOrEmpty(ngDuelUnit.trigger_option_attack) ? "critical_attack" : ngDuelUnit.trigger_option_attack) : (string.IsNullOrEmpty(ngDuelUnit.trigger_dual_single_option_attack) ? "dual_single_critical" : ngDuelUnit.trigger_dual_single_option_attack);
    ngDuelUnit.setTrigToAnimator(trigname);
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTimeCritical);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
    yield return (object) new WaitForSeconds(1f);
  }

  private IEnumerator execCriticalRemote(int turn_count, bool isDualSingleAttack)
  {
    NGDuelUnit ngDuelUnit = this;
    if ((double) ngDuelUnit.mDuelTime.criticalCutinWaitTime > 0.0 && ngDuelUnit.PlayCriticalCutin())
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.criticalCutinWaitTime);
    if (turn_count == 0)
    {
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTime1);
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackBeginOffsetTime);
    string trigname = !isDualSingleAttack ? (string.IsNullOrEmpty(ngDuelUnit.trigger_option_attack_throw) ? "to_critical_attack_throw" : ngDuelUnit.trigger_option_attack_throw) : (string.IsNullOrEmpty(ngDuelUnit.trigger_dual_single_option_attack_throw) ? "dual_single_critical_throw" : ngDuelUnit.trigger_dual_single_option_attack_throw);
    ngDuelUnit.setTrigToAnimator(trigname);
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTimeCritical);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  private IEnumerator execCriticalMagic(int turn_count, bool isDualSingleAttack)
  {
    NGDuelUnit ngDuelUnit = this;
    if ((double) ngDuelUnit.mDuelTime.criticalCutinWaitTime > 0.0 && ngDuelUnit.PlayCriticalCutin())
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.criticalCutinWaitTime);
    if (turn_count == 0)
    {
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTime1);
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackBeginOffsetTime);
    string trigname = !isDualSingleAttack ? (string.IsNullOrEmpty(ngDuelUnit.trigger_option_attack) ? "to_critical_attack_magic" : ngDuelUnit.trigger_option_attack) : (string.IsNullOrEmpty(ngDuelUnit.trigger_dual_single_option_attack) ? "dual_shingle_critical_magic" : ngDuelUnit.trigger_dual_single_option_attack);
    ngDuelUnit.setTrigToAnimator(trigname);
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackWaitTimeCritical);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  public IEnumerator execKoyuSkill(BL.Skill ids)
  {
    NGDuelUnit ngDuelUnit = this;
    if (ids.skill.duel_effect == null)
    {
      if (ngDuelUnit.thisTurn != null)
      {
        ngDuelUnit.mToNextTurn = true;
        yield return (object) ngDuelUnit.mEnemyUnit.playDamage(ngDuelUnit.thisTurn, ngDuelUnit.thisTurn.damage, ngDuelUnit.thisTurn.dispDamage, ngDuelUnit.thisTurn.defenderDrainDamage, ngDuelUnit.thisTurn.defenderDispDrainDamage, new List<BL.Skill>((IEnumerable<BL.Skill>) ngDuelUnit.thisTurn.invokeDefenderDuelSkills), ngDuelUnit.thisTurn.invokeSkillExtraInfo, ngDuelUnit.mEnemyUnit.thisTurn.attackStatus.elementAttackRate, ngDuelUnit.thisTurn.isDieDefender(), ngDuelUnit.thisTurn.isCritical, isHit: ngDuelUnit.thisTurn.isHit);
        if (ngDuelUnit.thisTurn.dispDrainDamage > 0)
          yield return (object) ngDuelUnit.showHealNumber();
      }
    }
    else
    {
      ngDuelUnit.mNowPlayKoyu = true;
      ngDuelUnit.mPlaiedKoyu = true;
      CommonRoot commonroot = Singleton<CommonRoot>.GetInstance();
      int orgDeapth = 0;
      if (Object.op_Inequality((Object) commonroot, (Object) null))
      {
        orgDeapth = commonroot.setBlackBGPanelDepth(-1);
        commonroot.isActiveBlackBGPanel = true;
      }
      Transform rootTransform = ngDuelUnit.baseGameObject.transform;
      Vector3 oldRootTransLocalPos = new Vector3(rootTransform.localPosition.x, rootTransform.localPosition.y, rootTransform.localPosition.z);
      Quaternion oldRootTransLocalRot = new Quaternion(rootTransform.localRotation.x, rootTransform.localRotation.y, rootTransform.localRotation.z, rootTransform.localRotation.w);
      ngDuelUnit.oldUnitParent = ((Component) ngDuelUnit).transform.parent;
      ngDuelUnit.oldCharactorTransLocalPos = new Vector3(((Component) ngDuelUnit).transform.localPosition.x, ((Component) ngDuelUnit).transform.localPosition.y, ((Component) ngDuelUnit).transform.localPosition.z);
      ngDuelUnit.oldCharactorTransLocalRot = new Quaternion(((Component) ngDuelUnit).transform.localRotation.x, ((Component) ngDuelUnit).transform.localRotation.y, ((Component) ngDuelUnit).transform.localRotation.z, ((Component) ngDuelUnit).transform.localRotation.w);
      yield return (object) new WaitForSecondsRealtime(0.3f);
      rootTransform.position = Vector3.zero;
      rootTransform.rotation = Quaternion.identity;
      if (Object.op_Inequality((Object) ngDuelUnit.mVehicleObject, (Object) null) && ids.skill.duel_effect.vehicle_link_off)
      {
        ((Component) ngDuelUnit).gameObject.transform.parent = ngDuelUnit.mVehicleObject.transform.parent;
        ((Component) ngDuelUnit).gameObject.transform.localPosition = Vector3.zero;
        ((Component) ngDuelUnit).gameObject.transform.localRotation = Quaternion.identity;
      }
      ResourceManager rm = Singleton<ResourceManager>.GetInstance();
      Future<RuntimeAnimatorController> fc = rm.Load<RuntimeAnimatorController>(ids.skill.duel_effect.duel_camera_animator_name);
      IEnumerator e = fc.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      RuntimeAnimatorController kkam = fc.Result;
      RuntimeAnimatorController origUnitCtrl = ngDuelUnit.mCharacterAnimator.runtimeAnimatorController;
      fc = rm.Load<RuntimeAnimatorController>(ids.skill.duel_effect.duel_animator_name);
      e = fc.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ngDuelUnit.playingRyuseiEf = ((Component) ngDuelUnit.mCharacterAnimator).gameObject.AddComponent<Ryusei_100111_ef>();
      ngDuelUnit.mCharacterAnimator.runtimeAnimatorController = fc.Result;
      ngDuelUnit.mDuelManager.ActKoyuCamera(kkam);
      RuntimeAnimatorController origVehicleCtrl = (RuntimeAnimatorController) null;
      if (Object.op_Inequality((Object) ngDuelUnit.mVehicleAnimator, (Object) null) && !string.IsNullOrEmpty(ids.skill.duel_effect.duel_vehicle_animator_name))
      {
        origVehicleCtrl = ngDuelUnit.mVehicleAnimator.runtimeAnimatorController;
        fc = rm.Load<RuntimeAnimatorController>(ids.skill.duel_effect.duel_vehicle_animator_name);
        e = fc.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ngDuelUnit.mVehicleAnimator.runtimeAnimatorController = fc.Result;
        ngDuelUnit.mVehicleAnimator.SetTrigger("Start");
      }
      ngDuelUnit.mDuelManager.currentCamera.GetComponent<Animator>().SetTrigger("Start");
      ngDuelUnit.mCharacterAnimator.SetTrigger("Start");
      ngDuelUnit.mEnemyUnit.warpToKoyuPosAsEnemy();
      if (Object.op_Inequality((Object) commonroot, (Object) null))
      {
        yield return (object) new WaitForSeconds(0.5f);
        commonroot.isActiveBlackBGPanel = false;
        yield return (object) new WaitForSeconds(0.3f);
        commonroot.setBlackBGPanelDepth(orgDeapth);
      }
      if (ngDuelUnit.isAnyThisTurn)
      {
        foreach (BL.DuelTurn mThisTurn in ngDuelUnit.mThisTurns)
        {
          if (mThisTurn.counterDamage != 0)
            ngDuelUnit.mPlayerUI.Damaged(mThisTurn.counterDamage);
        }
      }
      yield return (object) new WaitForSeconds(ids.skill.duel_effect.duel_koyu_wait_time);
      ngDuelUnit.SetActiveEquipeWeapon(true);
      ngDuelUnit.manager.SetActiveMap(true);
      if (Object.op_Inequality((Object) ngDuelUnit.mVehicleAnimator, (Object) null))
        ngDuelUnit.mVehicleAnimator.runtimeAnimatorController = origVehicleCtrl;
      ngDuelUnit.mCharacterAnimator.runtimeAnimatorController = origUnitCtrl;
      ngDuelUnit.SetCleanUpDuelSkillEffect();
      rootTransform.localPosition = oldRootTransLocalPos;
      rootTransform.localRotation = oldRootTransLocalRot;
      ngDuelUnit.adjustInitPosition();
      ngDuelUnit.mEnemyUnit.adjustInitPosition();
      yield return (object) new WaitForSeconds(0.5f);
      if (ngDuelUnit.thisTurn != null && ngDuelUnit.thisTurn.attackStatus != null && ngDuelUnit.thisTurn.attackStatus.isDrain)
        yield return (object) ngDuelUnit.healByMadan();
      ngDuelUnit.mNowPlayKoyu = false;
      ngDuelUnit.mToNextTurn = true;
    }
  }

  private IEnumerator execSuisei(int turn_count)
  {
    NGDuelUnit ngDuelUnit = this;
    ngDuelUnit.playAttackSWait();
    IEnumerator e = ngDuelUnit.loadSkillEffect("ef103_shootingstar_s");
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.suiseiAttackSWaitTime);
    if ((double) ngDuelUnit.mDuelTime.criticalCutinWaitTime > 0.0 && ngDuelUnit.PlaySkillCutin())
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.criticalCutinWaitTime);
    if (turn_count == 0)
    {
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.suiseiFirstAttackWaitTime);
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
    }
    float attack = ngDuelUnit.moveToAttack(ngDuelUnit.baseGameObject);
    if (ngDuelUnit.mDuelManager.useDistance)
      attack += ngDuelUnit.mDuelTime.suiseiAddTime;
    yield return (object) new WaitForSeconds(attack);
    ngDuelUnit.setTrigToAnimator("skill_ss5_attack");
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.suiseiAttack1WaitTime);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  private IEnumerator execSuiseiRemote(int turn_count)
  {
    NGDuelUnit ngDuelUnit = this;
    ngDuelUnit.setTrigToAnimator("to_skill_attackswait_throw");
    IEnumerator e = ngDuelUnit.loadSkillEffect("ef103_shootingstar_s");
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.suiseiAttackSWaitTime);
    if ((double) ngDuelUnit.mDuelTime.criticalCutinWaitTime > 0.0 && ngDuelUnit.PlaySkillCutin())
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.criticalCutinWaitTime);
    if (turn_count == 0)
    {
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.suiseiFirstAttackWaitTime);
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackBeginOffsetTime);
    ngDuelUnit.setTrigToAnimator("to_skill_ss5_attack_throw");
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.suiseiAttack1WaitTime);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  private IEnumerator execSkillAttack(int turn_count, BL.Skill skill)
  {
    NGDuelUnit ngDuelUnit = this;
    ngDuelUnit.playAttackSWait();
    if (skill.skill.duel_effect != null)
    {
      IEnumerator e = ngDuelUnit.loadSkillEffect(skill.skill.duel_effect.duel_effect_name);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.skillAttackSWaitTime);
    if ((double) ngDuelUnit.mDuelTime.criticalCutinWaitTime > 0.0 && ngDuelUnit.PlaySkillCutin())
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.criticalCutinWaitTime);
    if (turn_count == 0)
    {
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.skillFirstAttackWaitTime);
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
    }
    float attack = ngDuelUnit.moveToAttack(ngDuelUnit.baseGameObject);
    if (ngDuelUnit.mDuelManager.useDistance)
      attack += ngDuelUnit.mDuelTime.skillAddTime;
    yield return (object) new WaitForSeconds(attack);
    ngDuelUnit.setTrigToAnimator("skill_attack");
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.skillAttack1WaitTime);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  private IEnumerator execSkillRemoteAttack(int turn_count, BL.Skill skill)
  {
    NGDuelUnit ngDuelUnit = this;
    ngDuelUnit.setTrigToAnimator("to_skill_attackswait_throw");
    if (skill.skill.duel_effect != null)
    {
      IEnumerator e = ngDuelUnit.loadSkillEffect(skill.skill.duel_effect.duel_effect_name);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.skillAttackSWaitTime);
    if (Object.op_Inequality((Object) ngDuelUnit.mDuelCI, (Object) null) && (double) ngDuelUnit.mDuelTime.criticalCutinWaitTime > 0.0 && ngDuelUnit.PlaySkillCutin())
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.criticalCutinWaitTime);
    if (turn_count == 0)
    {
      yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.skillFirstAttackWaitTime);
      ngDuelUnit.mDuelManager.ActAttackBeginCamera();
    }
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.attackBeginOffsetTime);
    ngDuelUnit.setTrigToAnimator("to_skill_attack_throw");
    yield return (object) new WaitForSeconds(ngDuelUnit.mDuelTime.skillAttack1WaitTime);
    ngDuelUnit.mDuelManager.ActCameraToCenter();
  }

  private IEnumerator execEndTurn()
  {
    NGDuelUnit ngDuelUnit = this;
    if (!ngDuelUnit.mToExecEndTurn)
    {
      ngDuelUnit.mToExecEndTurn = true;
      if ((double) ngDuelUnit.mMyReach >= 9.0)
        yield return (object) new WaitForSeconds(2f);
      ngDuelUnit.ResetTriggers();
      ngDuelUnit.Enemy.ResetTriggers();
      ngDuelUnit.mDuelManager.ActCameraToCenter();
      yield return (object) new WaitForSeconds(0.5f);
      if (ngDuelUnit.isAnyThisTurn)
      {
        bool flag = false;
        foreach (BL.DuelTurn mThisTurn in ngDuelUnit.mThisTurns)
        {
          flag |= mThisTurn.isDieAttacker();
          flag |= mThisTurn.isDieDefender();
        }
        if (flag)
        {
          IEnumerator e = ngDuelUnit.loadDefaultAnimatorController();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      ngDuelUnit.mDuelManager.turnEnd();
    }
  }

  public void AtAttack1()
  {
    this.currentClipStatus = "attack1";
    this.currentStatus = NGDuelUnit.Status.ST_READY_A1;
    if (this.mFirstAttackDone)
    {
      this.attackForward(this.mDuelTime.attack1ForwardSpeed, this.mDuelTime.attack1ForwardDistance, this.mDuelTime.attack1ForwardEaseType, this.mDuelTime.attack1MoveStartTime);
    }
    else
    {
      if (this.mIsPlayer)
      {
        if ((double) this.mMyReach < 9.0)
          this.mDuelManager.WarpCameraToPrincess();
      }
      else if ((double) this.mMyReach < 9.0)
        this.mDuelManager.WarpCameraToEnemy();
      this.mFirstAttackDone = true;
    }
  }

  public void AtAttack2()
  {
    this.currentClipStatus = "attack2";
    this.currentStatus = NGDuelUnit.Status.ST_READY_A2;
    if (this.mFirstAttackDone)
    {
      this.attackForward(this.mDuelTime.attack2ForwardSpeed, this.mDuelTime.attack2ForwardDistance, this.mDuelTime.attack2ForwardEaseType, this.mDuelTime.attack2MoveStartTime);
    }
    else
    {
      if (this.mIsPlayer)
      {
        if ((double) this.mMyReach < 9.0)
          this.mDuelManager.WarpCameraToPrincess();
      }
      else if ((double) this.mMyReach < 9.0)
        this.mDuelManager.WarpCameraToEnemy();
      this.mFirstAttackDone = true;
    }
    if ((double) this.mMyReach >= 9.0)
      return;
    this.StartCoroutine(this.attackSubEffect());
  }

  public void AtAttackS()
  {
    this.currentClipStatus = "attackS";
    this.currentStatus = NGDuelUnit.Status.ST_READY_AS;
    if (this.mFirstAttackDone)
    {
      this.attackForward(this.mDuelTime.attackSForwardSpeed, this.mDuelTime.attackSForwardDistance, this.mDuelTime.attackSForwardEaseType, this.mDuelTime.attackSMoveStartTime);
    }
    else
    {
      if (this.mIsPlayer)
      {
        if ((double) this.mMyReach < 9.0)
          this.mDuelManager.WarpCameraToPrincess();
      }
      else if ((double) this.mMyReach < 9.0)
        this.mDuelManager.WarpCameraToEnemy();
      this.mFirstAttackDone = true;
    }
    if ((double) this.mMyReach >= 9.0)
      return;
    this.StartCoroutine(this.attackSubEffect());
  }

  private IEnumerator delayLastAttackOn(float time)
  {
    NGDuelUnit ngDuelUnit = this;
    yield return (object) new WaitForSeconds(time);
    if (ngDuelUnit.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.kind.Enum == GearKindEnum.bow)
      ngDuelUnit.mDuelManager.ChangeCameraToRemotoSuiseiHalf();
    ngDuelUnit.mIsLastAttack = true;
  }

  private IEnumerator execAttackSubEffect(float attackTimeLength, float attackDelay)
  {
    IEnumerator e = this.attackSubEffect(attackDelay);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(attackTimeLength);
    if (this.currentStatus != NGDuelUnit.Status.ST_BACKSTEP && this.currentStatus != NGDuelUnit.Status.ST_READY_BS && this.currentStatus != NGDuelUnit.Status.ST_READY_A2)
      this.currentStatus = NGDuelUnit.Status.ST_NONE;
  }

  protected IEnumerator shootReadyEffect(float wait = 0.0f)
  {
    IEnumerator e = this.missileWeaponAttackReadyEffect(wait);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected IEnumerator attackSubEffect(float wait = 0.0f)
  {
    NGDuelUnit ngDuelUnit = this;
    if ((double) wait > 0.0)
      yield return (object) new WaitForSeconds(wait);
    IEnumerator e;
    if (ngDuelUnit.isShotMadan)
    {
      e = ngDuelUnit.madanWeaponAttackSubEffect(wait);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (Object.op_Inequality((Object) ngDuelUnit.mMissileWeaponPrefab, (Object) null))
    {
      if (ngDuelUnit.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.kind_GearKind == 4)
      {
        e = ngDuelUnit.arrowAttackSubEffect(wait);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = ngDuelUnit.missileWeaponAttackSubEffect(wait);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  protected IEnumerator missileWeaponAttackReadyEffect(float wait)
  {
    NGDuelUnit ngDuelUnit = this;
    if (Object.op_Inequality((Object) ngDuelUnit.mMissileWeaponReadyEffectPrefab, (Object) null))
    {
      Transform rootTrans = ((Component) ngDuelUnit).transform.GetChildInFind("muzzle");
      if (Object.op_Inequality((Object) rootTrans, (Object) null))
        rootTrans = ((Component) ngDuelUnit).transform.GetChildInFind("weaponr");
      if ((double) wait > 0.0)
        yield return (object) new WaitForSeconds(wait);
      GameObject gameObject = ngDuelUnit.mMissileWeaponReadyEffectPrefab.Clone(rootTrans);
      ngDuelUnit.mRemoveObjects.Add(gameObject);
      rootTrans = (Transform) null;
    }
  }

  protected IEnumerator missileWeaponAttackSubEffect(float wait)
  {
    NGDuelUnit ngDuelUnit = this;
    if (Object.op_Inequality((Object) null, (Object) ngDuelUnit.mMissileWeaponPrefab))
    {
      GameObject mBullet = ngDuelUnit.mMissileWeaponPrefab.Clone(ngDuelUnit.mRoot3D.transform);
      GameObject elementEffectPrefab = ngDuelUnit.getMissileWeaponElementEffectPrefab();
      if (Object.op_Inequality((Object) elementEffectPrefab, (Object) null))
        elementEffectPrefab.Clone(mBullet.transform);
      ngDuelUnit.mRemoveObjects.Add(mBullet);
      MissileWeapon mw = mBullet.GetComponentInChildren<MissileWeapon>();
      Transform childInFind = ((Component) ngDuelUnit).gameObject.transform.GetChildInFind("weaponr");
      mw.handWeapon = (GameObject) null;
      if (childInFind.childCount > 0)
        mw.handWeapon = ((Component) childInFind.GetChild(0)).gameObject;
      if (Object.op_Equality((Object) mw.handWeapon, (Object) null))
        ((Component) mw).transform.position = childInFind.position;
      else
        ((Component) mw).transform.position = mw.handWeapon.transform.position;
      if ((double) wait != 0.0 && !ngDuelUnit.mIsSuisei)
        yield return (object) new WaitForSeconds(wait);
      mw.Fire(ngDuelUnit.mEnemyUnit.mBipTransform, ((Component) ngDuelUnit).gameObject, ngDuelUnit.thisTurn.isHit);
      ngDuelUnit.mBullets.Add(mBullet);
      mBullet = (GameObject) null;
      mw = (MissileWeapon) null;
    }
    yield return (object) null;
  }

  protected IEnumerator madanWeaponAttackSubEffect(float wait)
  {
    NGDuelUnit healtarget = this;
    if (healtarget.thisTurn != null)
    {
      BL.MagicBullet magicBullet1 = healtarget.thisTurn.attackStatus == null ? (healtarget.thisTurn.isAtackker ? healtarget.manager.mDuelResult.attackAttackStatus.magicBullet : healtarget.manager.mDuelResult.defenseAttackStatus.magicBullet) : healtarget.thisTurn.attackStatus.magicBullet;
      if (magicBullet1 == null)
      {
        yield break;
      }
      else
      {
        GameObject bulletObj = Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(magicBullet1.prefabName, healtarget.mRoot3D.transform);
        bulletObj.SetActive(true);
        MagicBullet magicBullet = bulletObj.GetComponentInChildren<MagicBullet>();
        if ((double) wait > 0.0 && !healtarget.mIsSuisei)
          yield return (object) new WaitForSeconds(wait);
        Transform muzzle = ((Component) healtarget).transform.GetChildInFind("muzzle");
        if (Object.op_Equality((Object) muzzle, (Object) null))
        {
          muzzle = ((Component) healtarget).transform.GetChildInFind("magicshotpoint_b");
          if (Object.op_Equality((Object) muzzle, (Object) null))
            muzzle = ((Component) healtarget).transform;
        }
        ((Component) magicBullet).transform.position = bulletObj.transform.position = muzzle.position;
        magicBullet.Fire(healtarget.mEnemyUnit, healtarget, muzzle, healtarget.thisTurn.isHit, healtarget.thisTurn.isCritical, healtarget.mMagicBullet.isDrain);
        healtarget.mBullets.Add(bulletObj);
        bulletObj = (GameObject) null;
        magicBullet = (MagicBullet) null;
      }
    }
    if (healtarget.mIsSuisei && healtarget.thisTurn.counterDamage != 0 && !healtarget.mIsFireFirstSuiseiMadan)
    {
      healtarget.mIsFireFirstSuiseiMadan = true;
      healtarget.mPlayerUI.Damaged(healtarget.thisTurn.counterDamage);
    }
    healtarget.incrementThisTurnsCurrent();
  }

  public string GetMissileHitEffect()
  {
    return Object.op_Inequality((Object) this.mMissileWeapon, (Object) null) ? this.mMissileWeapon.damageEffect : string.Empty;
  }

  protected IEnumerator arrowAttackSubEffect(float wait)
  {
    NGDuelUnit ngDuelUnit = this;
    if (Object.op_Inequality((Object) null, (Object) ngDuelUnit.mMissileWeaponPrefab))
    {
      GameObject gameObject = ngDuelUnit.mMissileWeaponPrefab.Clone(ngDuelUnit.mRoot3D.transform);
      GameObject elementEffectPrefab = ngDuelUnit.getMissileWeaponElementEffectPrefab();
      if (Object.op_Inequality((Object) elementEffectPrefab, (Object) null))
        elementEffectPrefab.Clone(gameObject.transform);
      ngDuelUnit.mRemoveObjects.Add(gameObject);
      MissileWeapon mw = gameObject.GetComponentInChildren<MissileWeapon>();
      Transform transform = ((Component) ngDuelUnit).transform.GetChildInFind("muzzle");
      if (Object.op_Equality((Object) transform, (Object) null))
        transform = ngDuelUnit.mBipTransform;
      ((Component) mw).transform.position = transform.position;
      ((Component) mw).transform.rotation = ((Component) ngDuelUnit).gameObject.transform.rotation;
      if ((double) wait != 0.0 && !ngDuelUnit.mIsSuisei)
        yield return (object) new WaitForSeconds(wait);
      mw.Fire(ngDuelUnit.mEnemyUnit.mBipTransform, ((Component) ngDuelUnit).gameObject, ngDuelUnit.thisTurn.isHit);
      mw = (MissileWeapon) null;
    }
    yield return (object) null;
  }

  private void attackForward(float speed, float distance, string easeType, float delay)
  {
    if (0.0 == (double) distance || (double) this.mMyReach >= 9.0)
      return;
    Hashtable hashtable = new Hashtable();
    GameObject baseGameObject = this.baseGameObject;
    float num = baseGameObject.transform.position.x + distance * this.mDirToEnemy;
    hashtable.Add((object) "x", (object) num);
    hashtable.Add((object) nameof (speed), (object) speed);
    hashtable.Add((object) nameof (delay), (object) delay);
    hashtable.Add((object) "easetype", (object) easeType);
    iTween.MoveTo(baseGameObject, hashtable);
  }

  protected float moveToAttack(GameObject target)
  {
    switch (this.mMoveType)
    {
      case DuelMoveTypeEnum.ground:
        return this.groundToAttack(target);
      case DuelMoveTypeEnum.fly:
        return this.flyToAttack(target);
      case DuelMoveTypeEnum.giant:
        return this.giantToAttack(target);
      default:
        return this.groundToAttack(target);
    }
  }

  protected float groundToAttack(GameObject target)
  {
    float attack = 0.0f;
    if (this.mConfig.noRunAttack == 0)
    {
      float num1 = (Mathf.Abs(((Component) this.Enemy).gameObject.transform.position.x - ((Component) this).gameObject.transform.position.x) - this.mMyReach) / this.mDuelTime.runSpeed;
      if ((double) this.mMyReach < 9.0)
      {
        float num2 = this.backward(((Component) this.Enemy).gameObject.transform.position.x, 1f + this.mMyReach);
        iTween.MoveTo(target, new Hashtable()
        {
          {
            (object) "x",
            (object) num2
          },
          {
            (object) "time",
            (object) num1
          },
          {
            (object) "delay",
            (object) 0.1f
          },
          {
            (object) "easetype",
            (object) this.mDuelTime.runEaseType
          }
        });
        this.setTrigToAnimator("to_run");
      }
      attack = num1 - this.mDuelTime.attackBeginOffsetTime;
    }
    return attack;
  }

  private void endVehicleCriticalMove() => this.currentStatus = NGDuelUnit.Status.ST_NONE;

  private Transform[] createRunPath(string path)
  {
    GameObject gameObject1 = GameObject.Find(path);
    if (Object.op_Equality((Object) null, (Object) gameObject1))
      return new Transform[0];
    List<GameObject> source = new List<GameObject>();
    foreach (Transform child in gameObject1.transform.GetChildren())
      source.Add(((Component) child).gameObject);
    List<Transform> transformList = new List<Transform>();
    foreach (GameObject gameObject2 in (IEnumerable<GameObject>) source.OrderBy<GameObject, string>((Func<GameObject, string>) (x => ((Object) x).name)))
    {
      if (gameObject2.activeInHierarchy)
        transformList.Add(gameObject2.transform);
    }
    return transformList.ToArray();
  }

  protected float flyToAttack(GameObject target)
  {
    float attack = 0.0f;
    if (this.mConfig.noRunAttack == 0)
    {
      float num = (Mathf.Abs(((Component) this.Enemy).gameObject.transform.position.x - target.transform.position.x) - this.mMyReach) / this.mDuelTime.runSpeed;
      if ((double) this.mMyReach < 9.0)
      {
        iTween.MoveTo(target, new Hashtable()
        {
          {
            (object) "time",
            (object) num
          },
          {
            (object) "delay",
            (object) 0.1f
          },
          {
            (object) "path",
            (object) this.createFlyPath()
          },
          {
            (object) "easetype",
            (object) this.mDuelTime.runEaseType
          }
        });
        this.setTrigToAnimator("to_run");
      }
      attack = num - this.mDuelTime.attackBeginOffsetTime;
    }
    return attack;
  }

  protected Transform[] createFlyPath()
  {
    Transform[] flyPath = new Transform[3];
    GameObject gameObject = GameObject.Find("path_fly_attack_mine");
    if (!this.mIsPlayer)
      gameObject = GameObject.Find("path_fly_attack_enemy");
    gameObject.transform.position = new Vector3(this.Enemy.baseGameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    flyPath[0] = gameObject.transform.GetChildInFind("path_1");
    flyPath[1] = gameObject.transform.GetChildInFind("path_2");
    flyPath[2] = gameObject.transform.GetChildInFind("path_3");
    return flyPath;
  }

  protected float giantToAttack(GameObject target)
  {
    float attack = 0.0f;
    if (this.mConfig.noRunAttack == 0)
    {
      float num = (Mathf.Abs(((Component) this.Enemy).gameObject.transform.position.x - target.transform.position.x) - this.mMyReach) / this.mDuelTime.runSpeed;
      if ((double) this.mMyReach < 9.0)
      {
        iTween.MoveTo(target, new Hashtable()
        {
          {
            (object) "time",
            (object) num
          },
          {
            (object) "delay",
            (object) 0.1f
          },
          {
            (object) "path",
            (object) this.createGiantPath()
          },
          {
            (object) "easetype",
            (object) this.mDuelTime.runEaseType
          }
        });
        this.setTrigToAnimator("to_run");
      }
      attack = num - this.mDuelTime.attackBeginOffsetTime;
    }
    return attack;
  }

  protected Transform[] createGiantPath()
  {
    Transform[] giantPath = new Transform[3];
    GameObject gameObject = GameObject.Find("path_large_attack_enemy");
    if (!this.mIsPlayer)
      gameObject = GameObject.Find("path_large_attack_enemy");
    gameObject.transform.position = new Vector3(this.Enemy.baseGameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    giantPath[0] = gameObject.transform.GetChildInFind("path_1");
    giantPath[1] = gameObject.transform.GetChildInFind("path_2");
    giantPath[2] = gameObject.transform.GetChildInFind("path_3");
    return giantPath;
  }

  public void damaged(bool force_lastattack = false, float delay = 0.0f)
  {
    if (force_lastattack)
      this.Enemy.mIsLastAttack = true;
    BL.DuelTurn thisTurnDamage = this.mEnemyUnit.thisTurnDamage;
    this.mEnemyUnit.incrementDispDamageCount();
    if (thisTurnDamage == null)
    {
      this.Enemy.mIsLastAttack = true;
    }
    else
    {
      int damage = thisTurnDamage.damage;
      int dispDamage = thisTurnDamage.dispDamage;
      int defenderDrainDamage = thisTurnDamage.defenderDrainDamage;
      int defenderDispDrainDamage = thisTurnDamage.defenderDispDrainDamage;
      float knockBackDistance1 = this.Enemy.mDuelTime.attack1ForwardKnockBack;
      if ((double) this.Enemy.mMyReach >= 9.0)
        knockBackDistance1 = 0.0f;
      else if (this.Enemy.currentClipStatus == "attack2")
        knockBackDistance1 = this.Enemy.mDuelTime.attack2ForwardKnockBack;
      else if (this.Enemy.currentClipStatus == "attackS")
        knockBackDistance1 = this.Enemy.mDuelTime.attackSForwardKnockBack;
      bool is_die = !thisTurnDamage.isAtackker ? thisTurnDamage.isDieAttacker() : thisTurnDamage.isDieDefender();
      if (this.mEnemyUnit.mInvokeDuelSkill == null)
      {
        int normalAttackCount = this.mEnemyUnit.mMyUnitData.playerUnit.normalAttackCount;
        NGDuelUnit.RandomDispType random_disp_type = NGDuelUnit.RandomDispType.None;
        if (normalAttackCount >= 2)
          random_disp_type = NGDuelUnit.RandomDispType.DualWield;
        float knockBackDistance2 = knockBackDistance1 / (float) normalAttackCount;
        this.StartCoroutine(this.playDamage(thisTurnDamage, damage, dispDamage, defenderDrainDamage, defenderDispDrainDamage, this.mDefenseDuelSkills, thisTurnDamage.invokeSkillExtraInfo, thisTurnDamage.attackStatus.elementAttackRate, is_die, thisTurnDamage.isCritical, delay, knockBackDistance2, thisTurnDamage.isHit, random_disp_type: random_disp_type));
      }
      else if (!this.mEnemyUnit.mIsSuisei)
      {
        this.StartCoroutine(this.playDamage(thisTurnDamage, damage, dispDamage, defenderDrainDamage, defenderDispDrainDamage, this.mDefenseDuelSkills, thisTurnDamage.invokeSkillExtraInfo, thisTurnDamage.attackStatus.elementAttackRate, is_die, thisTurnDamage.isCritical, knockBackDistance: knockBackDistance1, isHit: thisTurnDamage.isHit));
      }
      else
      {
        if (this.mEnemyUnit.mSuiseiCount >= thisTurnDamage.suiseiResults.Count)
          return;
        BL.SuiseiResult suiseiResult = thisTurnDamage.suiseiResults[this.mEnemyUnit.mSuiseiCount];
        ++this.mEnemyUnit.mSuiseiCount;
        this.Enemy.mIsLastAttack = thisTurnDamage.suiseiResults.Count == this.mEnemyUnit.mSuiseiCount;
        this.StartCoroutine(this.playDamage(thisTurnDamage, suiseiResult.damage, suiseiResult.dispDamage, suiseiResult.defenderDrainDamage, suiseiResult.defenderDispDrainDamage, new List<BL.Skill>((IEnumerable<BL.Skill>) suiseiResult.invokeDefenderDuelSkills), suiseiResult.invokeSkillExtraInfo, this.mEnemyUnit.thisTurn.attackStatus.elementAttackRate, is_die, suiseiResult.isCritical, knockBackDistance: knockBackDistance1, isHit: suiseiResult.isHit, random_disp_type: NGDuelUnit.RandomDispType.Suisei));
      }
    }
  }

  private IEnumerator playDamage(
    BL.DuelTurn turn,
    int damage,
    int dispDamage,
    int heal,
    int dispHeal,
    List<BL.Skill> defenderSkills,
    List<string> invokeSkillExtraInfo,
    float elementAttackRate,
    bool is_die,
    bool is_critical = false,
    float delay = 0.0f,
    float knockBackDistance = 0.0f,
    bool isHit = false,
    bool isDamageDisp = true,
    NGDuelUnit.RandomDispType random_disp_type = NGDuelUnit.RandomDispType.None)
  {
    NGDuelUnit ngDuelUnit = this;
    yield return (object) new WaitForSeconds(delay);
    if (!ngDuelUnit.isRemovedSleepEffect && damage >= 1)
    {
      ngDuelUnit.isRemovedSleepEffect = true;
      foreach (BL.SkillEffect skillEffect in ngDuelUnit.mMyUnitData.skillEffects.Where(BattleskillEffectLogicEnum.sleep))
      {
        BattleskillAilmentEffect ailmentEffect = skillEffect.baseSkill.ailment_effect;
        if (ailmentEffect != null && ((IEnumerable<int>) ngDuelUnit.beforeAilmentEffectIDs).Any<int>((Func<int, bool>) (x => x == ailmentEffect.ID)))
        {
          Transform transform = ngDuelUnit.mBipTransform.Find(ailmentEffect.duel_effect_name + "(Clone)");
          if (Object.op_Inequality((Object) transform, (Object) null))
            ((Component) transform).gameObject.SetActive(false);
        }
      }
    }
    Vector3 additional_pos = new Vector3();
    switch (random_disp_type)
    {
      case NGDuelUnit.RandomDispType.Suisei:
        additional_pos.x = (float) Random.Range(-3, 2) * 0.6f;
        additional_pos.z = (float) Random.Range(-3, 2) * 0.6f;
        additional_pos.y = (float) Random.Range(-3, 0) * 0.6f;
        break;
      case NGDuelUnit.RandomDispType.DualWield:
        additional_pos.x = (float) Random.Range(-3, 2) * 0.6f;
        additional_pos.z = (float) Random.Range(-3, 2) * 0.6f;
        additional_pos.y = turn.attackCount % 2 != 0 ? Random.Range(-3f, -1.75f) * 0.6f : Random.Range(-1.25f, 0.0f) * 0.6f;
        break;
    }
    if ((double) ngDuelUnit.Enemy.mMyReach >= 9.0)
    {
      if (!ngDuelUnit.mRemoteSuiseiCameraFlag && ngDuelUnit.Enemy.mInvokeDuelSkill != null)
      {
        if (ngDuelUnit.Enemy.thisTurn.invokeDuelSkills.Length != 0)
        {
          if (!ngDuelUnit.Enemy.mNowPlayKoyu && ngDuelUnit.Enemy.mIsSuisei)
            ngDuelUnit.mRemoteSuiseiCameraFlag = true;
          ngDuelUnit.WarpCameraToRemotePos();
        }
        else
          ngDuelUnit.WarpCameraToRemotePos();
      }
      else if (!ngDuelUnit.mRemoteSuiseiCameraFlag)
        ngDuelUnit.WarpCameraToRemotePos();
    }
    if (invokeSkillExtraInfo != null && invokeSkillExtraInfo.Any<string>((Func<string, bool>) (x => x == "absolute_defense")))
    {
      ngDuelUnit.playAbsoluteDefenseEffect();
      foreach (BL.Skill defenderSkill in defenderSkills)
      {
        BattleskillGenre? genre1 = defenderSkill.genre1;
        BattleskillGenre battleskillGenre = BattleskillGenre.defense;
        if (genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue)
          ngDuelUnit.mPlayerUI.skillInvoke(defenderSkill);
      }
      if (is_critical)
        ngDuelUnit.StartCoroutine(ngDuelUnit.playCriticalEffect(additional_pos));
    }
    else if (!isHit)
    {
      if (isDamageDisp)
        ngDuelUnit.StartCoroutine(ngDuelUnit.playMissEffect(additional_pos));
      if (!ngDuelUnit.mIsDodging && !ngDuelUnit.Enemy.mIsSuisei && !ngDuelUnit.isInvalidDodgeMotion)
      {
        ngDuelUnit.StartCoroutine(ngDuelUnit.playDodgeMotion());
        if ((double) ngDuelUnit.Enemy.mMyReach >= 9.0)
          ngDuelUnit.Enemy.mToNextTurn = true;
      }
    }
    else if (defenderSkills.Count != 0 && defenderSkills.Any<BL.Skill>((Func<BL.Skill, bool>) (x =>
    {
      BattleskillGenre? genre1 = x.genre1;
      BattleskillGenre battleskillGenre = BattleskillGenre.defense;
      return genre1.GetValueOrDefault() == battleskillGenre & genre1.HasValue;
    })))
    {
      foreach (BL.Skill defenderSkill in defenderSkills)
      {
        BattleskillGenre? nullable = defenderSkill.genre1;
        BattleskillGenre battleskillGenre1 = BattleskillGenre.defense;
        if (nullable.GetValueOrDefault() == battleskillGenre1 & nullable.HasValue)
        {
          ngDuelUnit.mPlayerUI.skillInvoke(defenderSkill);
          ngDuelUnit.playGuardMotion(damage, dispDamage, isDamageDisp, additional_pos, elementAttackRate, heal, dispHeal);
          nullable = defenderSkill.genre2;
          if (nullable.HasValue)
          {
            nullable = defenderSkill.genre2;
            BattleskillGenre battleskillGenre2 = BattleskillGenre.attack;
            int num = nullable.GetValueOrDefault() == battleskillGenre2 & nullable.HasValue ? 1 : 0;
          }
        }
      }
      if (is_critical)
        ngDuelUnit.StartCoroutine(ngDuelUnit.playCriticalEffect(additional_pos));
    }
    else if (ngDuelUnit.mMyUnitData.playerUnit.equippedGearOrInitial.kind.Enum == GearKindEnum.shield || ngDuelUnit.mMyUnitData.playerUnit.equippedGear2 != (PlayerItem) null && ngDuelUnit.mMyUnitData.playerUnit.equippedGear2.gear.kind.Enum == GearKindEnum.shield)
    {
      ngDuelUnit.playGuardMotion(damage, dispDamage, isDamageDisp, additional_pos, elementAttackRate, heal, dispHeal);
      if (ngDuelUnit.Enemy.mIsLastAttack & is_critical)
        ngDuelUnit.StartCoroutine(ngDuelUnit.playCriticalEffect(additional_pos));
    }
    else
    {
      ngDuelUnit.mDuelManager.ShakeCamera();
      ngDuelUnit.setTrigToAnimator("to_damage");
      if (0.0 != (double) knockBackDistance)
        ngDuelUnit.StartCoroutine(ngDuelUnit.damageKnockBack(knockBackDistance));
      if (isDamageDisp && dispDamage != 0)
      {
        ngDuelUnit.showDamageEffect(damage, dispDamage, additional_pos, elementAttackRate, heal, dispHeal);
        if (is_critical)
          ngDuelUnit.StartCoroutine(ngDuelUnit.playCriticalEffect(additional_pos));
      }
      yield return (object) new WaitForSeconds(1f);
    }
    if (is_die && ngDuelUnit.Enemy.mIsLastAttack)
    {
      if (ngDuelUnit.mMyUnitData.playerUnit.equippedGearOrInitial.kind.Enum == GearKindEnum.shield || ngDuelUnit.mMyUnitData.playerUnit.equippedGear2 != (PlayerItem) null && ngDuelUnit.mMyUnitData.playerUnit.equippedGear2.gear.kind.Enum == GearKindEnum.shield)
        yield return (object) new WaitForSeconds(0.5f);
      ngDuelUnit.setTrigToAnimator("damage_to_death");
      ngDuelUnit.setTrigToAnimator("to_death");
      if ((double) ngDuelUnit.Enemy.mMyReach >= 9.0)
      {
        ngDuelUnit.Enemy.mToNextTurn = true;
        if (ngDuelUnit.Enemy.mInvokeDuelSkill != null && !ngDuelUnit.Enemy.mPlaiedKoyu && (!turn.attackStatus.isMagic || !turn.attackStatus.isDrain) && turn.dispDrainDamage > 0)
          ngDuelUnit.StartCoroutine(ngDuelUnit.Enemy.playSikkokuHeal());
      }
      else if (ngDuelUnit.Enemy.mInvokeDuelSkill != null)
      {
        if (ngDuelUnit.Enemy.mInvokeDuelSkill.skill.haveKoyuDuelEffect)
          ngDuelUnit.Enemy.mToNextTurn = true;
        if (!ngDuelUnit.Enemy.mPlaiedKoyu && turn.dispDrainDamage > 0)
          ngDuelUnit.StartCoroutine(ngDuelUnit.Enemy.playSikkokuHeal());
      }
      if (turn.attackerAilmentEffects != null)
      {
        foreach (BattleskillAilmentEffect attackerAilmentEffect in turn.attackerAilmentEffects)
          Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(attackerAilmentEffect.duel_effect_name, ngDuelUnit.Enemy.mBipTransform);
      }
      ngDuelUnit.mDuelManager.ActCameraToCenter();
      if (is_critical)
        yield return (object) new WaitForSeconds(2f);
      else
        yield return (object) new WaitForSeconds(1.5f);
      ngDuelUnit.mToNextTurn = true;
    }
    else if (ngDuelUnit.Enemy.mIsLastAttack)
    {
      if (!ngDuelUnit.mIsDodging)
      {
        if ((double) ngDuelUnit.Enemy.mMyReach >= 9.0)
        {
          if (ngDuelUnit.Enemy.mInvokeDuelSkill != null && !ngDuelUnit.Enemy.mNowPlayKoyu && ngDuelUnit.Enemy.mIsSuisei)
            yield return (object) new WaitForSeconds(1f);
          if (turn.dispDrainDamage == 0 && turn.attackCount <= 1)
            ngDuelUnit.Enemy.mToNextTurn = true;
        }
        ngDuelUnit.StopCoroutine("defenceMoveToOrigPos");
        ngDuelUnit.StartCoroutine("defenceMoveToOrigPos");
        if (ngDuelUnit.Enemy.mInvokeDuelSkill != null && !ngDuelUnit.Enemy.mPlaiedKoyu && (!turn.attackStatus.isMagic || !turn.attackStatus.isDrain) && turn.dispDrainDamage > 0)
          ngDuelUnit.StartCoroutine(ngDuelUnit.Enemy.playSikkokuHeal());
      }
      if (turn.ailmentEffects != null)
      {
        foreach (BattleskillAilmentEffect ailmentEffect in turn.ailmentEffects)
          Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(ailmentEffect.duel_effect_name, ngDuelUnit.mBipTransform);
      }
      if (turn.attackerAilmentEffects != null)
      {
        foreach (BattleskillAilmentEffect attackerAilmentEffect in turn.attackerAilmentEffects)
          Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(attackerAilmentEffect.duel_effect_name, ngDuelUnit.Enemy.mBipTransform);
      }
    }
  }

  private IEnumerator damageKnockBack(float distance = 0.5f, float speed = 5f, string easeType = "EaseInQuad", float delay = 0.0f)
  {
    NGDuelUnit ngDuelUnit = this;
    yield return (object) new WaitForSeconds(delay);
    float t = distance / speed;
    float x = ngDuelUnit.baseGameObject.transform.position.x;
    float e = ngDuelUnit.backward(x, distance);
    Linear linearX = new Linear(x, e, t);
    while (!linearX.isEnd)
    {
      linearX.Update(Time.deltaTime);
      Vector3 position = ngDuelUnit.baseGameObject.transform.position;
      if (ngDuelUnit.mIsPlayer)
        position.x += (float) ((double) speed * (double) Time.deltaTime * -1.0);
      else
        position.x += speed * Time.deltaTime;
      ngDuelUnit.baseGameObject.transform.position = new Vector3(linearX.value, position.y, position.z);
      yield return (object) null;
    }
  }

  private void playGuardMotion(
    int damage,
    int dispDamage,
    bool isDamageDisp,
    Vector3 additional_pos,
    float elementAttackRate,
    int heal,
    int dispHeal)
  {
    this.setTrigToAnimator("to_guard");
    this.currentStatus = NGDuelUnit.Status.ST_GURAD;
    if (0.0 != (double) this.Enemy.mDuelTime.attack1ForwardKnockBack)
      this.StartCoroutine(this.damageKnockBack(this.Enemy.mDuelTime.attack1ForwardKnockBack, 2f, "easeOutQuad", 0.2f));
    if (isDamageDisp)
      this.showDamageEffect(damage, dispDamage, additional_pos, elementAttackRate, heal, dispHeal);
    if (!Object.op_Inequality((Object) null, (Object) this.mShieldObject))
      return;
    this.mShieldObject.SetActive(true);
    this.StartCoroutine(this.shieldOff());
  }

  private IEnumerator playDodgeMotion()
  {
    if (!this.mIsDodging)
    {
      this.mIsDodging = true;
      this.setTrigToAnimator("to_escape");
      yield break;
    }
  }

  private IEnumerator waitTillDodgeEnd()
  {
    if (this.mIsDodging)
      yield return (object) null;
    yield return (object) null;
  }

  private void setDuelShadow()
  {
    if (Object.op_Equality((Object) this.manager.mShadow, (Object) null))
      return;
    Transform parent = this.mBipTransform;
    if (Object.op_Equality((Object) null, (Object) parent))
      parent = ((Component) this).gameObject.transform;
    this.mEffectShadow = this.manager.mShadow.Clone(parent);
    this.mEffectShadow.transform.localScale = new Vector3(this.mMyUnitData.unit.duel_shadow_scale_x, this.mMyUnitData.unit.duel_shadow_scale_z, 1f);
  }

  private void setDuelSupport()
  {
    if (Object.op_Equality((Object) this.manager.mDuelSupport, (Object) null))
      return;
    this.mEffectDuelSupport = this.manager.mDuelSupport.Clone(((Component) this).gameObject.transform);
    Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(this.mEffectDuelSupport);
    if (!Object.op_Equality((Object) null, (Object) this.mEffectDuelSupport))
      return;
    Debug.LogError((object) "[NGDuelUnit] loadDuelSupport cannot load DuelSupport");
  }

  public void hideDuelSupport()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.mEffectDuelSupport))
      return;
    this.mEffectDuelSupport.SetActive(false);
  }

  public void showDuelSupport()
  {
    if (this.mIsDamageWait)
    {
      this.setTrigToAnimator("to_wait");
      this.mIsDamageWait = false;
    }
    this.Enemy.enemyCancelDamagewait();
    if (Object.op_Equality((Object) null, (Object) this.mEffectDuelSupport))
    {
      Debug.LogError((object) "[NGDuelUnit] showDuelSupport efDuelSupport is null");
    }
    else
    {
      DuelSupport component = this.mEffectDuelSupport.GetComponent<DuelSupport>();
      if (!Object.op_Inequality((Object) null, (Object) component) || this.mSupport == null)
        return;
      component.setNumbers(this.mSupport.hit + this.mSupportHitIncr, this.mSupport.evasion + this.mSupportEvasionIncr, this.mSupport.critical + this.mSupportCriticalIncr, this.mSupport.critical_evasion + this.mSupportCriticalEvasionIncr);
    }
  }

  protected override IEnumerator CreateCutinObject()
  {
    NGDuelUnit ngDuelUnit = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = ngDuelUnit.\u003C\u003En__0();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) ngDuelUnit.mDuelCINew, (Object) null))
    {
      Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(((Component) ngDuelUnit.mDuelCINew).gameObject);
      if (!ngDuelUnit.mDuelCINew.isTexExist)
        ngDuelUnit.mDuelCINew = (DuelCutin) null;
    }
    if (Object.op_Inequality((Object) ngDuelUnit.mDuelCI, (Object) null))
    {
      Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(((Component) ngDuelUnit.mDuelCI).gameObject);
      if (!ngDuelUnit.mDuelCI.isTexExist)
        ngDuelUnit.mDuelCI = (DuelCutin) null;
    }
  }

  protected override void SetCutinTexture(DuelCutin cutin, int unitId, DuelCutin.CUTINPOS pos)
  {
    Texture duelCutin = Singleton<NGDuelDataManager>.GetInstance().GetDuelCutin(unitId);
    if (Object.op_Inequality((Object) duelCutin, (Object) null))
      cutin.SetCutinTexture(duelCutin, pos);
    else
      base.SetCutinTexture(cutin, unitId, pos);
  }

  public void adjustInitPosition() => this.adjustPosition(this.mInitPos);

  public void adjustPosition(Transform pos)
  {
    this.baseGameObject.transform.position = pos.position;
    this.baseGameObject.transform.rotation = pos.rotation;
  }

  private void adjustInitPos()
  {
    if (Object.op_Equality((Object) null, (Object) this.mInitPos))
    {
      Debug.LogError((object) "[NGDuelUnit] at adjustInitPos Init Position is NULL.");
    }
    else
    {
      float x = ((Component) this.mInitPos).transform.position.x;
      float num = 1f;
      if ((double) x < 0.0)
        num = -1f;
      ((Component) this.mInitPos).transform.position = new Vector3((Mathf.Abs(x) + this.mDuelTime.characterInitOffset) * num, ((Component) this.mInitPos).transform.position.y, ((Component) this.mInitPos).transform.position.z);
      this.adjustPosition(this.mInitPos);
    }
  }

  private IEnumerator bowSuiseiLastCameraWork()
  {
    this.mDuelManager.ChangeCameraToRemotoSuiseiHalf();
    yield return (object) new WaitForSeconds(1f);
    this.WarpCameraToRemotePos(false);
  }

  private IEnumerator shieldOff(float duration = 2f)
  {
    NGDuelUnit ngDuelUnit = this;
    yield return (object) new WaitForSeconds(duration);
    if (Object.op_Inequality((Object) ngDuelUnit.mShieldObject, (Object) null))
      ngDuelUnit.mShieldObject.SetActive(false);
  }

  public IEnumerator loadWinAnimator()
  {
    NGDuelUnit ngDuelUnit = this;
    ngDuelUnit.currentStatus = NGDuelUnit.Status.ST_WIN;
    SkillMetamorphosis metamorphosis = ngDuelUnit.mMyUnitData.metamorphosis;
    int metamorphosisId = metamorphosis != null ? metamorphosis.metamorphosis_id : 0;
    Future<RuntimeAnimatorController> winCont = ngDuelUnit.mMyUnitData.playerUnit.LoadWinAnimator(metamorphosisId);
    IEnumerator e = winCont.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ngDuelUnit.mCharacterAnimator.runtimeAnimatorController = winCont.Result;
    ((Behaviour) ngDuelUnit.mCharacterAnimator).enabled = false;
  }

  public void SetAnimatorEnabled() => ((Behaviour) this.mCharacterAnimator).enabled = true;

  public void ResetTriggers()
  {
    this.mCharacterAnimator.ResetTrigger("to_run");
    if (Object.op_Inequality((Object) null, (Object) this.mVehicleAnimator))
      this.mVehicleAnimator.ResetTrigger("to_run");
    this.mCharacterAnimator.ResetTrigger("to_wait");
    if (Object.op_Inequality((Object) null, (Object) this.mVehicleAnimator))
      this.mVehicleAnimator.ResetTrigger("to_wait");
    this.mCharacterAnimator.ResetTrigger("to_ss_last");
    if (!Object.op_Inequality((Object) null, (Object) this.mVehicleAnimator))
      return;
    this.mVehicleAnimator.ResetTrigger("to_ss_last");
  }

  private IEnumerator loadSkillEffect(string effectName)
  {
    NGDuelUnit ngDuelUnit = this;
    if (!string.IsNullOrEmpty(effectName) && Object.op_Equality((Object) Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(effectName, ngDuelUnit.mBipTransform), (Object) null))
    {
      Future<GameObject> ef = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("BattleEffects/duel/{0}", (object) effectName));
      IEnumerator e = ef.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) ef.Result, (Object) null))
        ef.Result.Clone(ngDuelUnit.mBipTransform);
      ef = (Future<GameObject>) null;
    }
  }

  public IEnumerator defenceMoveToOrigPos()
  {
    this.mToNextTurn = true;
    yield break;
  }

  private void completeMoveToOrigPos() => this.mToNextTurn = true;

  private IEnumerator ChangeCameraCenterDelay(float delay = 0.0f)
  {
    yield return (object) new WaitForSeconds(delay);
    this.mDuelManager.ActCameraToCenter();
  }

  private void WarpCameraToRemotePos(bool isDamageSide = true)
  {
    if (isDamageSide)
    {
      if (this.mMyUnitData.playerUnit.is_enemy)
        this.mDuelManager.WarpCameraToEnemyAsRemote(((Component) this).gameObject.transform, 0.0f);
      else
        this.mDuelManager.WarpCameraToPrincessAsRemote(((Component) this).gameObject.transform, 0.0f);
    }
    else if (!this.mMyUnitData.playerUnit.is_enemy)
      this.mDuelManager.WarpCameraToEnemyAsRemote(((Component) this.Enemy).gameObject.transform, 0.0f);
    else
      this.mDuelManager.WarpCameraToPrincessAsRemote(((Component) this.Enemy).gameObject.transform, 0.0f);
    this.StartCoroutine(this.ChangeCameraCenterDelay(1f));
  }

  private void dispDamageEffect(int damage, int dispDamage, Vector3 additional_pos)
  {
    if (Object.op_Equality((Object) this.manager.mDamagePrefab, (Object) null))
      return;
    GameObject gameObject = this.manager.mDamagePrefab.Clone(this.mRoot3D.transform);
    gameObject.transform.localPosition = ((Component) this).transform.position;
    this.mRemoveObjects.Add(gameObject);
    DamageNumber component = gameObject.GetComponent<DamageNumber>();
    component.setDamage(dispDamage);
    Transform transform = this.mBipTransform;
    if (Object.op_Equality((Object) null, (Object) transform))
      transform = ((Component) this).gameObject.transform;
    Vector3 vector3 = Vector3.op_Addition(transform.position, additional_pos);
    ((Component) component).transform.position = new Vector3(vector3.x, vector3.y + 1.5f, vector3.z);
  }

  private void dispDamageEffectBiAttack(int damage, int dispDamage)
  {
    if (Object.op_Equality((Object) this.manager.mDamagePrefab, (Object) null))
      return;
    GameObject gameObject = this.manager.mDamagePrefab.Clone(this.mRoot3D.transform);
    gameObject.transform.localPosition = ((Component) this).transform.position;
    this.mRemoveObjects.Add(gameObject);
    DamageNumber component = gameObject.GetComponent<DamageNumber>();
    component.setDamage(dispDamage);
    Transform transform = this.mBipTransform;
    if (Object.op_Equality((Object) null, (Object) transform))
      transform = ((Component) this).gameObject.transform;
    Vector3 position = transform.position;
    ((Component) component).transform.position = new Vector3(position.x, position.y + 1.5f, position.z);
  }

  private void showDamageEffect(
    int damage,
    int dispDamage,
    Vector3 additional_pos,
    float elementAttackRate,
    int heal,
    int dispHeal)
  {
    this.mPlayerUI.Damaged(damage, new int?(heal));
    this.dispDamageEffect(damage, dispDamage, additional_pos);
    this.StartCoroutine(this.showSelfHealNumber(dispHeal, additional_pos));
    if ((double) elementAttackRate > 1.0)
      this.StartCoroutine(this.dispWeakEffect(additional_pos));
    if ((double) elementAttackRate >= 1.0)
      return;
    this.StartCoroutine(this.dispResistEffect(additional_pos));
  }

  private IEnumerator dispWeakEffect(Vector3 additional_pos)
  {
    NGDuelUnit ngDuelUnit = this;
    if (!Object.op_Equality((Object) ngDuelUnit.manager.mWeakEffect, (Object) null))
    {
      GameObject weakEffect = ngDuelUnit.manager.mWeakEffect.Clone(ngDuelUnit.mRoot3D.transform);
      Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(weakEffect);
      weakEffect.transform.position = Vector3.op_Addition(ngDuelUnit.mBipTransform.position, additional_pos);
      weakEffect.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
      weakEffect.SetActive(false);
    }
  }

  private IEnumerator dispResistEffect(Vector3 additional_pos)
  {
    NGDuelUnit ngDuelUnit = this;
    if (!Object.op_Equality((Object) ngDuelUnit.manager.mResistEffect, (Object) null))
    {
      GameObject resistEffect = ngDuelUnit.manager.mResistEffect.Clone(ngDuelUnit.mRoot3D.transform);
      Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(resistEffect);
      resistEffect.transform.position = Vector3.op_Addition(ngDuelUnit.mBipTransform.position, additional_pos);
      resistEffect.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
      resistEffect.SetActive(false);
    }
  }

  protected void setTrigToAnimator(string trigname)
  {
    if (Object.op_Inequality((Object) this.mCharacterAnimator, (Object) null))
      this.mCharacterAnimator.SetTrigger(trigname);
    if (!Object.op_Inequality((Object) this.mVehicleAnimator, (Object) null))
      return;
    this.mVehicleAnimator.SetTrigger(trigname);
  }

  private string getUnitVehicleWeaponName()
  {
    string vehicleWeaponName = "sword";
    if (this.mMyUnitData.unit.kind_GearKind == 2)
      vehicleWeaponName = "ax";
    else if (this.mMyUnitData.unit.kind_GearKind == 3)
      vehicleWeaponName = "spear";
    return vehicleWeaponName;
  }

  protected override IEnumerator createWeapon()
  {
    NGDuelUnit ngDuelUnit = this;
    switch (ngDuelUnit.mMyUnitData.unit.non_disp_weapon)
    {
      case 1:
        yield break;
      case 2:
        if ((double) ngDuelUnit.mMyReach != 9.0 || ngDuelUnit.mMagicBullet != null)
          yield break;
        else
          break;
    }
    Future<GameObject> pg = ngDuelUnit.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.LoadModel();
    IEnumerator e = pg.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = pg.Result;
    Transform[] transformArray = new Transform[2];
    if (ngDuelUnit.mMyUnitData.playerUnit.isDualWieldWeapon)
    {
      transformArray[0] = ((Component) ngDuelUnit).gameObject.transform.GetChildInFind("weaponl");
      transformArray[1] = ((Component) ngDuelUnit).gameObject.transform.GetChildInFind("weaponr");
    }
    else if (ngDuelUnit.mMyUnitData.playerUnit.isLeftHandWeapon)
      transformArray[0] = ((Component) ngDuelUnit).gameObject.transform.GetChildInFind("weaponl");
    else
      transformArray[1] = ((Component) ngDuelUnit).gameObject.transform.GetChildInFind("weaponr");
    for (int index = 0; index < transformArray.Length; ++index)
    {
      if (!Object.op_Equality((Object) transformArray[index], (Object) null))
      {
        GameObject gameObject = result.Clone(transformArray[index]);
        if (!Object.op_Equality((Object) gameObject, (Object) null))
        {
          Quaternion quaternion;
          switch (ngDuelUnit.mMyUnitData.playerUnit.unit.kind_GearKind)
          {
            case 4:
              quaternion = Quaternion.Euler(0.0f, -90f, -90f);
              break;
            case 5:
              quaternion = Quaternion.Euler(0.0f, 90f, -90f);
              break;
            default:
              quaternion = ngDuelUnit.mIsMonster ? Quaternion.Euler(0.0f, 0.0f, 180f) : Quaternion.Euler(0.0f, -180f, 0.0f);
              break;
          }
          gameObject.transform.localRotation = quaternion;
          ngDuelUnit.mWeaponObject[index] = gameObject;
        }
      }
    }
  }

  private float backward(float current, float distance)
  {
    return this.mIsPlayer ? current + distance : current - distance;
  }

  public void enemyCancelDamagewait()
  {
    if (!this.mIsDamageWait)
      return;
    this.setTrigToAnimator("to_wait");
    this.mIsDamageWait = false;
  }

  public bool isThisTurnHit()
  {
    if (!this.thisTurnDamage.isHit)
      return false;
    int damage = this.thisTurnDamage.damage;
    return true;
  }

  public int damageOfThisTurn() => this.thisTurnDamage.damage;

  public void playWinPose() => this.setTrigToAnimator("to_win");

  public void playRunPose()
  {
    double attack = (double) this.moveToAttack(this.baseGameObject);
  }

  public void playAttackSWait() => this.setTrigToAnimator("to_attackswait");

  private IEnumerator playCriticalEffect(Vector3 additional_pos)
  {
    NGDuelUnit ngDuelUnit = this;
    if (!Object.op_Equality((Object) null, (Object) ngDuelUnit.manager.mCriticalEffect))
    {
      GameObject critecalEffect = ngDuelUnit.manager.mCriticalEffect.Clone(ngDuelUnit.mRoot3D.transform);
      Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(critecalEffect);
      critecalEffect.transform.position = Vector3.op_Addition(ngDuelUnit.mBipTransform.position, additional_pos);
      critecalEffect.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
      critecalEffect.SetActive(false);
    }
  }

  public IEnumerator playCriticalFlash()
  {
    NGDuelUnit ngDuelUnit = this;
    if (!Object.op_Equality((Object) null, (Object) ngDuelUnit.manager.mCriticalFlash))
    {
      GameObject CriticalFlash = ngDuelUnit.manager.mCriticalFlash.Clone(ngDuelUnit.manager.mRoot3d.transform);
      Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(CriticalFlash);
      CriticalFlash.SetActive(true);
      CriticalFlash.transform.position = ngDuelUnit.mBipTransform.position;
      yield return (object) new WaitForSeconds(1.5f);
      CriticalFlash.SetActive(false);
    }
  }

  public void playCriticalFlash_CallStartCoroutine()
  {
    this.StartCoroutine(this.playCriticalFlash());
  }

  private IEnumerator playMissEffect(Vector3 additional_pos)
  {
    NGDuelUnit ngDuelUnit = this;
    if (!Object.op_Equality((Object) null, (Object) ngDuelUnit.manager.mMissEffect))
    {
      GameObject missEffect = ngDuelUnit.manager.mMissEffect.Clone(ngDuelUnit.mRoot3D.transform);
      Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(missEffect);
      missEffect.transform.position = Vector3.op_Addition(ngDuelUnit.mBipTransform.position, additional_pos);
      missEffect.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
      missEffect.SetActive(false);
    }
  }

  private void playAbsoluteDefenseEffect()
  {
    GameObject preloadDuelEffect = Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect("ef120_holy_shield_s", this.mDuelManager.mRoot3d.transform);
    Transform transform = ((Component) this).transform;
    preloadDuelEffect.transform.localRotation = transform.rotation;
    preloadDuelEffect.transform.localPosition = transform.position;
    preloadDuelEffect.transform.parent = transform;
  }

  public IEnumerator playSikkokuHeal()
  {
    NGDuelUnit ngDuelUnit = this;
    Future<GameObject> pf = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/ef321_lisire_bullet_2_s");
    IEnumerator e = pf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = pf.Result;
    ngDuelUnit.mDrainBullet = result.Clone(ngDuelUnit.mRoot3D.transform);
    Singleton<NGDuelDataManager>.GetInstance().AddDestroyList(ngDuelUnit.mDrainBullet);
    ngDuelUnit.mDrainBullet.transform.position = ngDuelUnit.Enemy.mBipTransform.position;
    Vector3 position = ((Component) ngDuelUnit.mInitPos).transform.position;
    position.y = 1f;
    ngDuelUnit.mDrainBullet.transform.LookAt(position);
    Hashtable hashtable = new Hashtable();
    hashtable.Clear();
    hashtable.Add((object) "position", (object) position);
    hashtable.Add((object) "looktarget", (object) position);
    hashtable.Add((object) "speed", (object) 12f);
    hashtable.Add((object) "delay", (object) 0.5f);
    hashtable.Add((object) "easetype", (object) (iTween.EaseType) 6);
    hashtable.Add((object) "oncomplete", (object) "onDrainFlyEnd");
    hashtable.Add((object) "oncompletetarget", (object) ((Component) ngDuelUnit).gameObject);
    iTween.MoveTo(ngDuelUnit.mDrainBullet, hashtable);
  }

  private void onDrainFlyEnd()
  {
    this.mDrainBullet.SetActive(false);
    this.StartCoroutine(this.healByMadan(2f));
  }

  public IEnumerator healByMadan(float scale = 1f)
  {
    NGDuelUnit ngDuelUnit = this;
    Future<GameObject> hepf = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/ef421_heal_target_s");
    IEnumerator e = hepf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    hepf.Result.Clone(ngDuelUnit.mBipTransform);
    e = ngDuelUnit.showHealNumber(scale);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void showHealNumber_CallStartCoroutine(float scale = 1f)
  {
    this.StartCoroutine(this.showHealNumber(scale));
  }

  public IEnumerator showHealNumber(float scale = 1f)
  {
    NGDuelUnit ngDuelUnit = this;
    int drainDamage = ngDuelUnit.thisTurn.drainDamage;
    ngDuelUnit.mPlayerUI.Healed(drainDamage);
    Future<GameObject> pf = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/ef031_heal_number_fe");
    IEnumerator e = pf.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) null, (Object) pf.Result))
    {
      Debug.LogError((object) "NGDuelUnit try to load ef031_heal_number_fe but result null");
      ngDuelUnit.mToNextTurn = true;
    }
    else
    {
      GameObject gameObject = pf.Result.Clone(ngDuelUnit.mBipTransform);
      if (Object.op_Equality((Object) null, (Object) gameObject))
      {
        Debug.LogError((object) "NGDuelUnit try to instantiate ef031_heal_number_fe but gameObject null");
        ngDuelUnit.mToNextTurn = true;
      }
      else
      {
        Vector3 position = gameObject.transform.position;
        ++position.y;
        gameObject.transform.position = position;
        Vector3 vector3;
        // ISSUE: explicit constructor call
        ((Vector3) ref vector3).\u002Ector(scale, scale, 1f);
        gameObject.transform.localScale = vector3;
        gameObject.gameObject.GetComponent<DamageNumber>().setDamage(ngDuelUnit.thisTurn.drainDamage);
        if (Object.op_Inequality((Object) ngDuelUnit.mVehicleAnimator, (Object) null) && (double) ngDuelUnit.mMyReach < 9.0)
          yield return (object) new WaitForSeconds(1f);
        ngDuelUnit.mToNextTurn = true;
      }
    }
  }

  public IEnumerator showSelfHealNumber(int dispHealNum, Vector3 additional_pos)
  {
    NGDuelUnit ngDuelUnit = this;
    if (dispHealNum != 0)
    {
      yield return (object) new WaitForSeconds(0.5f);
      Future<GameObject> pf = Singleton<ResourceManager>.GetInstance().Load<GameObject>("BattleEffects/duel/ef031_heal_number_fe");
      IEnumerator e = pf.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!Object.op_Equality((Object) null, (Object) pf.Result))
      {
        GameObject gameObject = pf.Result.Clone(ngDuelUnit.mRoot3D.transform);
        gameObject.transform.localPosition = ((Component) ngDuelUnit).transform.position;
        if (!Object.op_Equality((Object) null, (Object) gameObject))
        {
          ngDuelUnit.mRemoveObjects.Add(gameObject);
          DamageNumber component = gameObject.GetComponent<DamageNumber>();
          component.setDamage(dispHealNum);
          Transform transform = ngDuelUnit.mBipTransform;
          if (Object.op_Equality((Object) null, (Object) transform))
            transform = ((Component) ngDuelUnit).gameObject.transform;
          Vector3 vector3 = Vector3.op_Addition(transform.position, additional_pos);
          ((Component) component).transform.position = new Vector3(vector3.x, vector3.y + 1.5f, vector3.z);
        }
      }
    }
  }

  protected IEnumerator loadMissileWeaponPrefab(int range)
  {
    NGDuelUnit ngDuelUnit = this;
    string loadPrefab = string.Empty;
    string loadReadyEffectPrefab = string.Empty;
    switch (ngDuelUnit.mMyUnitData.playerUnit.equippedWeaponGearOrInitial.kind_GearKind)
    {
      case 1:
        if (range > 1)
        {
          loadPrefab = string.IsNullOrEmpty(ngDuelUnit.mMyUnitData.weapon.gear.bullet_prefab_name) ? "ef329_sword_bullet" : ngDuelUnit.mMyUnitData.weapon.gear.bullet_prefab_name;
          loadReadyEffectPrefab = ngDuelUnit.mMyUnitData.weapon.gear.shoot_ready_effect_prefab_name;
          break;
        }
        break;
      case 2:
        if (range > 1)
        {
          loadPrefab = string.IsNullOrEmpty(ngDuelUnit.mMyUnitData.weapon.gear.bullet_prefab_name) ? "ef325_axe_bullet" : ngDuelUnit.mMyUnitData.weapon.gear.bullet_prefab_name;
          loadReadyEffectPrefab = ngDuelUnit.mMyUnitData.weapon.gear.shoot_ready_effect_prefab_name;
          break;
        }
        break;
      case 3:
        if (range > 1)
        {
          loadPrefab = string.IsNullOrEmpty(ngDuelUnit.mMyUnitData.weapon.gear.bullet_prefab_name) ? "ef326_spear_bullet" : ngDuelUnit.mMyUnitData.weapon.gear.bullet_prefab_name;
          loadReadyEffectPrefab = ngDuelUnit.mMyUnitData.weapon.gear.shoot_ready_effect_prefab_name;
          break;
        }
        break;
      case 4:
        loadPrefab = string.IsNullOrEmpty(ngDuelUnit.mMyUnitData.weapon.gear.bullet_prefab_name) ? "ef324_bow_bullet" : ngDuelUnit.mMyUnitData.weapon.gear.bullet_prefab_name;
        loadReadyEffectPrefab = ngDuelUnit.mMyUnitData.weapon.gear.shoot_ready_effect_prefab_name;
        break;
    }
    if (!string.IsNullOrEmpty(loadPrefab))
    {
      IEnumerator e = ngDuelUnit.loadMissileWeaponPrefab(loadPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ngDuelUnit.mMissileWeapon = ngDuelUnit.mMissileWeaponPrefab.GetComponent<MissileWeapon>();
      if (!string.IsNullOrEmpty(loadReadyEffectPrefab))
      {
        e = ngDuelUnit.loadmMissileWeaponReadyEffectPrefab(loadReadyEffectPrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      int index1 = 0;
      CommonElement[] commonElementArray = ngDuelUnit.mAttackElements;
      for (int index2 = 0; index2 < commonElementArray.Length; ++index2)
      {
        int num = (int) commonElementArray[index2];
        DuelElementBulletEffect elementBulletEffect = ngDuelUnit.GetElementBulletEffect(loadPrefab, index1);
        if (elementBulletEffect != null)
          yield return (object) ngDuelUnit.loadmMissileWeaponElementEffectPrefab(elementBulletEffect);
        ++index1;
      }
      commonElementArray = (CommonElement[]) null;
    }
  }

  protected IEnumerator loadMissileWeaponPrefab(string missileName)
  {
    Future<GameObject> go = Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(string.Format("BattleEffects/duel/{0}", (object) missileName));
    IEnumerator e = go.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mMissileWeaponPrefab = go.Result;
  }

  protected IEnumerator loadmMissileWeaponReadyEffectPrefab(string readyEffectPrefabName)
  {
    Future<GameObject> go = Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(string.Format("BattleEffects/duel/{0}", (object) readyEffectPrefabName));
    IEnumerator e = go.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mMissileWeaponReadyEffectPrefab = go.Result;
  }

  protected IEnumerator loadmMissileWeaponElementEffectPrefab(
    DuelElementBulletEffect duelElmBulletEff)
  {
    if (!string.IsNullOrEmpty(duelElmBulletEff.effect_name) && !this.mMissileWeaponElementEffectPrefabDic.ContainsKey(duelElmBulletEff.element))
    {
      Future<GameObject> go = Singleton<ResourceManager>.GetInstance().LoadOrNull<GameObject>(string.Format("BattleEffects/duel/{0}", (object) duelElmBulletEff.bullet_prefab_name));
      IEnumerator e = go.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.mMissileWeaponElementEffectPrefabDic.Add(duelElmBulletEff.element, go.Result);
      go = (Future<GameObject>) null;
    }
  }

  protected GameObject getMissileWeaponElementEffectPrefab()
  {
    CommonElement attackElement = this.GetAttackElement(this.mThisTurnCount);
    return !this.mMissileWeaponElementEffectPrefabDic.ContainsKey(attackElement) ? (GameObject) null : this.mMissileWeaponElementEffectPrefabDic[attackElement];
  }

  protected IEnumerator loadMadanPrefab()
  {
    NGDuelUnit ngDuelUnit = this;
    if (ngDuelUnit.mMagicBullet != null)
    {
      Future<GameObject> fs = ngDuelUnit.mMagicBullet.skill.LoadDuelMagicBulletPrefab();
      IEnumerator e = fs.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ngDuelUnit.mMadanPrefab = fs.Result;
      GameObject mBullet = ngDuelUnit.mMadanPrefab.Clone(ngDuelUnit.mRoot3D.transform);
      e = mBullet.GetComponent<MagicBullet>().preloadPrefabs();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      mBullet.SetActive(false);
      ngDuelUnit.mBullets.Add(mBullet);
    }
  }

  public void warpToKoyuPosAsEnemy()
  {
    if (this.Enemy.mInvokeDuelSkill == null || !this.Enemy.mInvokeDuelSkill.skill.haveKoyuDuelEffect)
      return;
    this.baseGameObject.transform.position = this.Enemy.mInvokeDuelSkill.skill.duel_effect.getEnemyPosition;
    if (!this.mIsPlayer)
      return;
    this.baseGameObject.transform.localRotation = Quaternion.Euler(0.0f, 90f, 0.0f);
  }

  public void shootSomethingReady() => this.StartCoroutine(this.shootReadyEffect());

  public void shootSomething() => this.StartCoroutine(this.attackSubEffect());

  public void shootSomethingLastAttack()
  {
    this.mIsLastAttack = true;
    this.StartCoroutine(this.attackSubEffect());
  }

  public void playBackstepFromClip() => this.currentStatus = NGDuelUnit.Status.ST_READY_BS;

  public bool isThisTurnCritical() => this.thisTurnDamage.isCritical;

  public void SetWinMode() => this.currentStatus = NGDuelUnit.Status.ST_WIN;

  public void SetDodgeMode() => this.currentStatus = NGDuelUnit.Status.ST_READY_DODGE;

  public override void SetActiveEquipeWeapon(bool active)
  {
    foreach (GameObject gameObject in this.mWeaponObject)
    {
      if (Object.op_Inequality((Object) gameObject, (Object) null) && gameObject.activeSelf != active)
        gameObject.SetActive(active);
    }
  }

  public override void SetActiveShadow(bool active)
  {
    if (!Object.op_Inequality((Object) this.mEffectShadow, (Object) null) || this.mEffectShadow.activeSelf == active)
      return;
    this.mEffectShadow.SetActive(active);
  }

  public enum Status
  {
    ST_WAIT,
    ST_READY_A1,
    ST_ATTACK1,
    ST_READY_A2,
    ST_ATTACK2,
    ST_READY_THROW,
    ST_THROW,
    ST_READY_AS,
    ST_ATTACKS,
    ST_ATTACKS_WAIT,
    ST_READY_BS,
    ST_BACKSTEP,
    ST_DAMAGE,
    ST_DAMAGE_WAIT,
    ST_DEATH,
    ST_ESCAPE,
    ST_GURAD,
    ST_READY_RUN,
    ST_RUN,
    ST_READY_CRIT_CAM,
    ST_CRIT_CAM,
    ST_READY_RUN_AFTER_ATTACK,
    ST_RUN_AFTER_ATTACK,
    ST_SUPPORT,
    ST_READY_KAIHOU,
    ST_KAIHOU,
    ST_READY_DODGE,
    ST_DODGE,
    ST_WIN,
    ST_NONE,
  }

  private enum RandomDispType
  {
    None,
    Suisei,
    DualWield,
  }
}
