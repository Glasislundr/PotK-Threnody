// Decompiled with JetBrains decompiler
// Type: ExploreDataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Explore;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-2)]
public class ExploreDataManager : Singleton<ExploreDataManager>
{
  public ChallengeNpc[] Gladiators;
  private DuelCache mLastDuelCache;
  public const int DECK_UNIT_COUNT_MAX = 5;
  private EpPlayerUnit[] mBlUnits = new EpPlayerUnit[5];
  public const int PROGRESS_RATIO_MAX = 1000000;
  private long mFloorElapsedTime;
  public const int MAX_LOG_VIEW = 20;
  private Queue<LogData> mLogQueue = new Queue<LogData>();

  public SortedDictionary<int, int> DefeatEnemyIDs { get; private set; } = new SortedDictionary<int, int>();

  public int WinCount { get; private set; }

  public int LoseCount { get; private set; }

  public void AddWinCount(int enemyID)
  {
    if (!this.DefeatEnemyIDs.ContainsKey(enemyID))
      this.DefeatEnemyIDs.Add(enemyID, 0);
    this.DefeatEnemyIDs[enemyID]++;
    ++this.WinCount;
  }

  public void AddLoseCount() => ++this.LoseCount;

  public void ResetBattleCount()
  {
    this.WinCount = 0;
    this.LoseCount = 0;
  }

  public int GetWinRate() => this.GetWinRate(this.WinCount, this.LoseCount);

  public int GetWinRate(int win, int lose)
  {
    int num = win + lose;
    int winRate = 0;
    if (num > 0)
      winRate = Mathf.FloorToInt((float) win * 100f / (float) num);
    return winRate;
  }

  public bool IsWinRateUpdate { get; set; }

  public SortedDictionary<int, ExploreEnemy> FloorEnemyList { get; private set; }

  public WeakPoint CurrentWeakPoint { get; set; }

  private IEnumerator UpdateFloorEnemyList()
  {
    ExploreDataManager exploreDataManager = this;
    exploreDataManager.FloorEnemyList = new SortedDictionary<int, ExploreEnemy>();
    if (exploreDataManager.RankingPeriod != null)
    {
      yield return (object) MasterData.LoadExploreEnemy(exploreDataManager.NowFloor);
      int key = 0;
      // ISSUE: reference to a compiler-generated method
      foreach (ExploreEnemy exploreEnemy in ((IEnumerable<ExploreEnemy>) MasterData.ExploreEnemyList).Where<ExploreEnemy>(new Func<ExploreEnemy, bool>(exploreDataManager.\u003CUpdateFloorEnemyList\u003Eb__31_0)))
      {
        key += exploreEnemy.lotteryRatio;
        exploreDataManager.FloorEnemyList.Add(key, exploreEnemy);
      }
      MasterDataCache.Unload("ExploreEnemy");
    }
  }

  private BL.Unit ConvertPlayerUnitToBlUnit(PlayerUnit pUnit, bool isEnemy)
  {
    IEnumerable<BL.MagicBullet> source = ((IEnumerable<PlayerUnitSkills>) pUnit.skills).Where<PlayerUnitSkills>((Func<PlayerUnitSkills, bool>) (v => v.skill.skill_type == BattleskillSkillType.magic)).Select<PlayerUnitSkills, BL.MagicBullet>((Func<PlayerUnitSkills, BL.MagicBullet>) (v => new BL.MagicBullet()
    {
      skillId = v.skill.ID
    }));
    BL.Unit blUnit = new BL.Unit();
    blUnit.unitId = pUnit.unit.ID;
    blUnit.playerUnit = pUnit;
    blUnit.lv = pUnit.total_level;
    blUnit.spawnTurn = pUnit.spawn_turn;
    blUnit.friend = false;
    blUnit.ougi = (BL.Skill) null;
    blUnit.skills = new BL.Skill[0];
    blUnit.magicBullets = source.ToArray<BL.MagicBullet>();
    blUnit.skillfull_shield = XorShift.Range(1, 5);
    blUnit.skillfull_weapon = XorShift.Range(1, 5);
    if (!isEnemy)
    {
      blUnit.weapon = new BL.Weapon(pUnit);
      blUnit.gearLeftHand = pUnit.isLeftHandWeapon;
      blUnit.gearDualWield = pUnit.isDualWieldWeapon;
    }
    BL.Skill skill1 = (BL.Skill) null;
    foreach (PlayerUnitSkills skill2 in pUnit.skills)
    {
      if (BattleskillSkill.InvestElementSkillIds.Contains(skill2.skill_id))
      {
        skill1 = new BL.Skill();
        skill1.id = skill2.skill_id;
        break;
      }
    }
    if (skill1 != null)
      blUnit.duelSkills = new BL.Skill[1]{ skill1 };
    else
      blUnit.duelSkills = new BL.Skill[0];
    return blUnit;
  }

  public void LazyInitDuel(ref DuelResult result)
  {
    this.LazyInitEnemyBlUnit(ref result.defense);
    bool gearDualWield1 = result.attack.gearDualWield;
    bool gearDualWield2 = result.defense.gearDualWield;
    foreach (BL.DuelTurn turn in result.turns)
      turn.isDualSingleAttack = turn.isAtackker ? gearDualWield1 : gearDualWield2;
  }

  private void LazyInitEnemyBlUnit(ref BL.Unit enemy)
  {
    PlayerUnit playerUnit = enemy.playerUnit;
    enemy.weapon = new BL.Weapon(playerUnit);
    enemy.gearLeftHand = playerUnit.isLeftHandWeapon;
    enemy.gearDualWield = playerUnit.isDualWieldWeapon;
  }

  public DuelEnvironment CreateDuelEnviroment()
  {
    return new DuelEnvironment()
    {
      stage = new BL.Stage(505)
    };
  }

  public void SetLastDuelCache(ExploreEnemy eEnemy, DuelResult result)
  {
    this.mLastDuelCache = new DuelCache(eEnemy, result.defenseDamage);
  }

  public ExploreEnemy GetLastDuelEnemy() => this.mLastDuelCache.Enemy;

  public bool IsAliveLastDuelEnemy()
  {
    return this.mLastDuelCache.Enemy != null && this.mLastDuelCache.Enemy.hp > this.mLastDuelCache.Damage;
  }

  public DuelResult CreateRevengeDuelResult()
  {
    return this.CreateDuelResult(this.mLastDuelCache.Enemy, this.mLastDuelCache.Damage);
  }

  public DuelResult CreateDuelResult(ExploreEnemy eEnemy, int enemyDamage = 0)
  {
    DuelResult duelResult = new DuelResult();
    duelResult.isPlayerAttack = true;
    BL.Unit blUnit1 = this.mBlUnits[0].BlUnit;
    ExploreDataManager.EpBattleParam playerParam = ExploreDataManager.EpBattleParam.CreatePlayerParam(this.mBlUnits, eEnemy.WeakPoint);
    blUnit1.exploreHp = blUnit1.initialHp = playerParam.Hp;
    blUnit1.isPlayerControl = true;
    duelResult.moveUnit = blUnit1;
    duelResult.attack = blUnit1;
    duelResult.attackAttackStatus = this.CreateAttackStatus(duelResult.attack, playerParam.WeakPointCoefficient);
    BL.Unit blUnit2 = this.ConvertPlayerUnitToBlUnit(eEnemy.PlayerUnit, true);
    ExploreDataManager.EpBattleParam enemyParam = ExploreDataManager.EpBattleParam.CreateEnemyParam(blUnit2);
    blUnit2.initialHp = enemyParam.Hp;
    blUnit2.exploreHp = blUnit2.initialHp - enemyDamage;
    blUnit2.isPlayerControl = false;
    duelResult.defense = blUnit2;
    duelResult.defenseAttackStatus = this.CreateAttackStatus(duelResult.defense, enemyParam.WeakPointCoefficient);
    List<BL.DuelTurn> duelTurnList = new List<BL.DuelTurn>();
    int exploreHp1 = duelResult.attack.exploreHp;
    int exploreHp2 = duelResult.defense.exploreHp;
    int num = 0;
    while (exploreHp1 > 0 && exploreHp2 > 0)
    {
      BL.DuelTurn duelTurn;
      if (num % 2 == 0)
      {
        duelTurn = this.CreatePlayerTurn(duelResult.attackAttackStatus, playerParam, enemyParam);
        exploreHp2 -= duelTurn.damage;
      }
      else
      {
        duelTurn = this.CreateEnemyTurn(duelResult.defenseAttackStatus, enemyParam, playerParam);
        exploreHp1 -= duelTurn.damage;
      }
      duelTurn.attackerRestHp = exploreHp1;
      duelTurn.defenderRestHp = exploreHp2;
      duelTurnList.Add(duelTurn);
      ++num;
    }
    duelResult.turns = duelTurnList.ToArray();
    duelResult.attackDamage = duelResult.attackFromDamage = duelResult.attack.initialHp - exploreHp1;
    duelResult.defenseDamage = duelResult.defenseFromDamage = duelResult.defense.initialHp - exploreHp2;
    duelResult.isDieAttack = exploreHp1 < 1;
    duelResult.isDieDefense = exploreHp2 < 1;
    duelResult.isBossBattle = false;
    duelResult.isFirstBoss = false;
    duelResult.isColosseum = false;
    duelResult.isExplore = true;
    duelResult.disableDuelSkillEffects = true;
    duelResult.beforeAttakerAilmentEffectIDs = new int[0];
    duelResult.beforeDefenderAilmentEffectIDs = new int[0];
    duelResult.distance = 1;
    return duelResult;
  }

  private static bool IsMagic(BL.Unit unit)
  {
    UnitUnit uUnit = unit.unit;
    if (uUnit.IsAllEquipUnit)
    {
      GearGear weaponGearOrInitial = unit.playerUnit.equippedWeaponGearOrInitial;
      return weaponGearOrInitial.kind_GearKind == 5 || weaponGearOrInitial.kind_GearKind == 6;
    }
    if (uUnit.kind_GearKind == 5 || uUnit.kind_GearKind == 6)
      return true;
    if (uUnit.kind_GearKind == 8)
    {
      UnitTypeSettings unitTypeSettings = Array.Find<UnitTypeSettings>(MasterData.UnitTypeSettingsList, (Predicate<UnitTypeSettings>) (x =>
      {
        if (!x.unit_UnitUnit.HasValue)
          return false;
        int id = uUnit.ID;
        int? unitUnitUnit = x.unit_UnitUnit;
        int valueOrDefault = unitUnitUnit.GetValueOrDefault();
        return id == valueOrDefault & unitUnitUnit.HasValue;
      }));
      if (unitTypeSettings != null && unitTypeSettings.attack_type == GearAttackType.magic)
        return true;
    }
    return false;
  }

  private AttackStatus CreateAttackStatus(BL.Unit unit, float weakPointCoefficient)
  {
    AttackStatus attackStatus = new AttackStatus();
    UnitUnit unit1 = unit.unit;
    CommonElement commonElement = unit.duelSkills.Length != 0 ? unit.duelSkills[0].skill.element : CommonElement.none;
    attackStatus.attackRate = 1f;
    attackStatus.normalDamageRate = 1f;
    attackStatus.isMagic = ExploreDataManager.IsMagic(unit);
    attackStatus.magicBullet = attackStatus.isMagic ? ((IEnumerable<BL.MagicBullet>) unit.magicBullets).First<BL.MagicBullet>((Func<BL.MagicBullet, bool>) (x => x.isAttack)) : (BL.MagicBullet) null;
    attackStatus.attackElements = new List<CommonElement>(1);
    if (attackStatus.magicBullet != null)
      attackStatus.attackElements.Add(attackStatus.magicBullet.element);
    else
      attackStatus.attackElements.Add(commonElement);
    attackStatus.elementAttackRate = weakPointCoefficient;
    attackStatus.isAbsoluteCounterAttack = false;
    attackStatus.duelParameter = new Judgement.BeforeDuelParameter();
    attackStatus.duelParameter.AttackCount = 1;
    attackStatus.duelParameter.DamageRate = 1f;
    attackStatus.duelParameter.BaseDamage = 10;
    return attackStatus;
  }

  private BL.DuelTurn CreatePlayerTurn(
    AttackStatus attackStatus,
    ExploreDataManager.EpBattleParam attackerParam,
    ExploreDataManager.EpBattleParam defenderParam)
  {
    BL.DuelTurn epDuelTurn = this.CreateEpDuelTurn();
    epDuelTurn.isAtackker = true;
    epDuelTurn.attackStatus = attackStatus;
    int num1;
    int num2 = num1 = Mathf.Max(1, attackerParam.Atk - defenderParam.Def);
    epDuelTurn.dispDamage = num1;
    int num3;
    int num4 = num3 = num2;
    epDuelTurn.realDamage = num3;
    epDuelTurn.damage = num4;
    return epDuelTurn;
  }

  private BL.DuelTurn CreateEnemyTurn(
    AttackStatus attackStatus,
    ExploreDataManager.EpBattleParam attackerParam,
    ExploreDataManager.EpBattleParam defenderParam)
  {
    BL.DuelTurn epDuelTurn = this.CreateEpDuelTurn();
    epDuelTurn.isAtackker = false;
    epDuelTurn.attackStatus = attackStatus;
    int num1;
    int num2 = num1 = Mathf.Max(1, attackerParam.Atk - defenderParam.Def);
    epDuelTurn.dispDamage = num1;
    int num3;
    int num4 = num3 = num2;
    epDuelTurn.realDamage = num3;
    epDuelTurn.damage = num4;
    return epDuelTurn;
  }

  private BL.DuelTurn CreateEpDuelTurn()
  {
    return new BL.DuelTurn()
    {
      isHit = true,
      isCritical = false,
      skillIds = new int[0],
      invokeDuelSkills = new BL.Skill[0],
      invokeDefenderDuelSkills = new BL.Skill[0],
      investSkillIds = new int[0],
      invokeAttackerDuelSkillEffectIds = new List<int>(),
      invokeDefenderDuelSkillEffectIds = new List<int>(),
      invokeSkillExtraInfo = new List<string>(),
      damageShareDamage = new List<int>(),
      damageShareSkillEffect = new List<BL.UseSkillEffect>(),
      attackCount = 1
    };
  }

  public Dictionary<string, long> TimeConfig { get; private set; }

  public TimeZoneInfo MasterTimeZone { get; private set; }

  public ExploreBox ExploreBox { get; private set; } = new ExploreBox();

  protected override void Initialize()
  {
    this.TimeConfig = ((IEnumerable<ExploreTimeConfig>) MasterData.ExploreTimeConfigList).ToDictionary<ExploreTimeConfig, string, long>((Func<ExploreTimeConfig, string>) (x => x.key), (Func<ExploreTimeConfig, long>) (x => (long) x.milli_sec));
    this.MasterTimeZone = TimeZoneInfo.CreateCustomTimeZone("Asia/Tokyo", new TimeSpan(9, 0, 0), "(UTC+09:00) 大阪、札幌、東京", "Asia/Tokyo");
  }

  public void ClearCache()
  {
    this.mBlUnits = (EpPlayerUnit[]) null;
    this.mLastDuelCache = new DuelCache();
    this.FloorDropDeck = (SortedDictionary<int, int>) null;
    this.FloorEnemyList = (SortedDictionary<int, ExploreEnemy>) null;
    this.DefeatEnemyIDs = (SortedDictionary<int, int>) null;
    this.LoginReportInfo = (LoginReportInfo) null;
  }

  public IEnumerator LoadSuspendData(bool loadFull, bool force = false)
  {
    if (Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      if (!force)
      {
        while (this.IsResuming)
          yield return (object) null;
      }
      ExploreProgress suspendData = SMManager.Get<ExploreProgress>();
      this.FloorData = MasterData.ExploreFloor[suspendData.floor_id];
      this.FrontFloorId = suspendData.head_floor_id;
      this.FloorElapsedTime = (long) ((Decimal) this.FloorRequiredTime * (Decimal) suspendData.progress / 1000000M);
      this.ExploreBox = new ExploreBox();
      this.ExploreBox.AddRewards(suspendData.box_reward_ids);
      if (this.LastSyncTime <= ServerTime.NowAppTimeAddDelta())
      {
        this.IsRankingAcceptanceFinish = false;
        this.RankingPeriod = ((IEnumerable<ExploreRankingPeriod>) MasterData.ExploreRankingPeriodList).OrderBy<ExploreRankingPeriod, DateTime>((Func<ExploreRankingPeriod, DateTime>) (x => x.end_at.Value)).FirstOrDefault<ExploreRankingPeriod>((Func<ExploreRankingPeriod, bool>) (x => this.ConvertMasterTimeToLocal(x.end_at.Value) > this.LastSyncTime));
      }
      else
      {
        this.IsRankingAcceptanceFinish = true;
        this.RankingPeriod = ((IEnumerable<ExploreRankingPeriod>) MasterData.ExploreRankingPeriodList).OrderByDescending<ExploreRankingPeriod, DateTime>((Func<ExploreRankingPeriod, DateTime>) (x => x.end_at.Value)).FirstOrDefault<ExploreRankingPeriod>((Func<ExploreRankingPeriod, bool>) (x => this.ConvertMasterTimeToLocal(x.end_at.Value) <= this.LastSyncTime));
      }
      if (loadFull)
      {
        this.UpdateFloorDropDeck();
        yield return (object) this.UpdateFloorEnemyList();
        this.mLastDuelCache = new DuelCache();
        ExploreEnemy eEnemy = this.FloorEnemyList.Values.FirstOrDefault<ExploreEnemy>((Func<ExploreEnemy, bool>) (x =>
        {
          int id = x.ID;
          int? lastEnemyId = suspendData.last_enemy_id;
          int valueOrDefault = lastEnemyId.GetValueOrDefault();
          return id == valueOrDefault & lastEnemyId.HasValue;
        }));
        if (eEnemy != null)
        {
          int lastDamage = suspendData.last_damage;
          if (eEnemy.hp > lastDamage)
            this.mLastDuelCache = new DuelCache(eEnemy, lastDamage);
        }
        this.DefeatEnemyIDs = new SortedDictionary<int, int>();
        this.WinCount = suspendData.win_count;
        this.LoseCount = suspendData.lose_count;
        this.IsWinRateUpdate = true;
        this.LoginReportInfo = new LoginReportInfo();
        Singleton<ExploreLotteryCore>.GetInstance().LoadSuspendData();
      }
    }
  }

  public DateTime LastSyncTime => SMManager.Get<ExploreProgress>().timestamp.Value;

  public DateTime ConvertMasterTimeToLocal(DateTime time)
  {
    return this.MasterTimeZone == null ? time : TimeZoneInfo.ConvertTime(time, this.MasterTimeZone, TimeZoneInfo.Local);
  }

  public bool IsResuming { get; private set; }

  public IEnumerator ResumeExplore()
  {
    yield return (object) this.ResumeExplore(this.LastSyncTime);
  }

  public IEnumerator ResumeExplore(DateTime calcStartTime)
  {
    ExploreDataManager exploreDataManager = this;
    while (exploreDataManager.IsResuming)
      yield return (object) null;
    exploreDataManager.IsResuming = true;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTimeAddDelta();
    if (exploreDataManager.LastSyncTime <= now)
    {
      exploreDataManager.IsRankingAcceptanceFinish = false;
      exploreDataManager.UpdateBlUnitsFromPlayerDeck();
      // ISSUE: reference to a compiler-generated method
      exploreDataManager.RankingPeriod = ((IEnumerable<ExploreRankingPeriod>) MasterData.ExploreRankingPeriodList).OrderBy<ExploreRankingPeriod, DateTime>((Func<ExploreRankingPeriod, DateTime>) (x => x.end_at.Value)).FirstOrDefault<ExploreRankingPeriod>(new Func<ExploreRankingPeriod, bool>(exploreDataManager.\u003CResumeExplore\u003Eb__72_3));
      TimeSpan span;
      if (exploreDataManager.RankingPeriod == null || exploreDataManager.ConvertMasterTimeToLocal(exploreDataManager.RankingPeriod.end_at.Value) >= now)
      {
        span = now - calcStartTime;
      }
      else
      {
        Future<WebAPI.Response.ExploreExploreGet> api = WebAPI.ExploreExploreGet();
        e = api.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (!api.HasResult)
        {
          yield break;
        }
        else
        {
          if (exploreDataManager.LastSyncTime <= now)
          {
            e = exploreDataManager.LoadSuspendData(true, true);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            span = now - exploreDataManager.LastSyncTime;
          }
          else
          {
            exploreDataManager.IsRankingAcceptanceFinish = true;
            span = exploreDataManager.ConvertMasterTimeToLocal(exploreDataManager.RankingPeriod.end_at.Value) - calcStartTime;
          }
          api = (Future<WebAPI.Response.ExploreExploreGet>) null;
        }
      }
      int beforBoxCount = exploreDataManager.ExploreBox.GetRewardsId().Count;
      // ISSUE: reference to a compiler-generated method
      yield return (object) Singleton<ExploreLotteryCore>.GetInstance().CalcTaskOnBackGround(span, errorCallback: new Action(exploreDataManager.\u003CResumeExplore\u003Eb__72_4));
      if (exploreDataManager.LoginCalcDirty)
      {
        int count = exploreDataManager.ExploreBox.GetRewardsId().Count;
        List<int> range = exploreDataManager.ExploreBox.GetRewardsId().GetRange(beforBoxCount, count - beforBoxCount);
        exploreDataManager.LoginReportInfo.SetReport(exploreDataManager.DefeatEnemyIDs.Sum<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, int>) (x => x.Value)), exploreDataManager.ExploreBox.PlayerExp, exploreDataManager.ExploreBox.Zeny);
        exploreDataManager.LoginReportInfo.AddReport(span, range.ToArray());
      }
      span = new TimeSpan();
    }
    else
    {
      exploreDataManager.IsRankingAcceptanceFinish = true;
      // ISSUE: reference to a compiler-generated method
      exploreDataManager.RankingPeriod = ((IEnumerable<ExploreRankingPeriod>) MasterData.ExploreRankingPeriodList).OrderByDescending<ExploreRankingPeriod, DateTime>((Func<ExploreRankingPeriod, DateTime>) (x => x.end_at.Value)).FirstOrDefault<ExploreRankingPeriod>(new Func<ExploreRankingPeriod, bool>(exploreDataManager.\u003CResumeExplore\u003Eb__72_1));
      exploreDataManager.LoginCalcDirty = false;
    }
    exploreDataManager.IsResuming = false;
  }

  private SuspendData CreateSuspendData()
  {
    SuspendData data = new SuspendData();
    if (this.IsAliveLastDuelEnemy())
    {
      data.EnemyId = this.mLastDuelCache.Enemy.ID;
      data.EnemyDamage = this.mLastDuelCache.Damage;
    }
    data.DefeatEnemyIds = this.DefeatEnemyIDs.Select<KeyValuePair<int, int>, int>((Func<KeyValuePair<int, int>, int>) (x => x.Key)).ToArray<int>();
    data.DefeatEnemyCounts = this.DefeatEnemyIDs.Select<KeyValuePair<int, int>, int>((Func<KeyValuePair<int, int>, int>) (x => x.Value)).ToArray<int>();
    Singleton<ExploreLotteryCore>.GetInstance().SetSuspendData(ref data);
    return data;
  }

  public IEnumerator SaveSuspendData(Action errorCallback, bool noErrorPopup = false)
  {
    SuspendData suspendData = this.CreateSuspendData();
    Future<WebAPI.Response.ExploreExploreUpdate> api = WebAPI.ExploreExploreUpdate(this.ExploreBox.GetRewardsId().ToArray(), suspendData.RandCnt, suspendData.DefeatEnemyCounts, suspendData.DefeatEnemyIds, this.FloorData.ID, suspendData.EnemyDamage, new int?(suspendData.EnemyId), this.LoseCount, this.Progress, suspendData.State, suspendData.TakeOver, suspendData.Rest, this.WinCount, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (noErrorPopup)
        return;
      WebAPI.DefaultUserErrorCallback(e);
    }));
    IEnumerator e1 = api.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (api.Result == null)
    {
      if (errorCallback != null)
        errorCallback();
    }
    else
    {
      this.DefeatEnemyIDs = new SortedDictionary<int, int>();
      Singleton<NGGameDataManager>.GetInstance().challenge_point = api.Result.challenge_point;
      this.UpdateBlUnitsFromPlayerDeck();
      this.SetupLocalPush();
    }
  }

  public IEnumerator GetBoxReward(Action errorCallback, Action isLimitCallback)
  {
    SuspendData suspendData = this.CreateSuspendData();
    Future<WebAPI.Response.ExploreExploreReward> api = WebAPI.ExploreExploreReward(this.ExploreBox.GetRewardsId().ToArray(), suspendData.RandCnt, suspendData.DefeatEnemyCounts, suspendData.DefeatEnemyIds, this.FloorData.ID, suspendData.EnemyDamage, new int?(suspendData.EnemyId), this.LoseCount, this.Progress, suspendData.State, suspendData.TakeOver, suspendData.Rest, this.WinCount);
    IEnumerator e = api.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (api.Result == null)
    {
      if (errorCallback != null)
        errorCallback();
    }
    else
    {
      if (api.Result.is_limit && isLimitCallback != null)
        isLimitCallback();
      this.DefeatEnemyIDs = new SortedDictionary<int, int>();
      this.ReloadExploreBox();
      this.UpdateBlUnitsFromPlayerDeck();
      this.SetupLocalPush();
    }
  }

  public IEnumerator EditDeck(
    int deckIndex,
    int[] playerUnitIds,
    int totalCombat,
    Action errorCallback)
  {
    this.ResetBattleCount();
    SuspendData suspendData = this.CreateSuspendData();
    Future<WebAPI.Response.ExploreDeckEdit> api = WebAPI.ExploreDeckEdit(this.ExploreBox.GetRewardsId().ToArray(), suspendData.RandCnt, deckIndex, suspendData.DefeatEnemyCounts, suspendData.DefeatEnemyIds, this.FloorData.ID, suspendData.EnemyDamage, new int?(suspendData.EnemyId), this.LoseCount, playerUnitIds, this.Progress, suspendData.State, suspendData.TakeOver, totalCombat, suspendData.Rest, this.WinCount);
    IEnumerator e = api.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (api.Result == null)
    {
      if (errorCallback != null)
        errorCallback();
    }
    else
    {
      this.DefeatEnemyIDs = new SortedDictionary<int, int>();
      this.UpdateBlUnitsFromPlayerDeck();
      this.SetupLocalPush();
    }
  }

  public IEnumerator MoveFloor(int targetFloorId, Action errorCallback)
  {
    SuspendData suspendData = this.CreateSuspendData();
    Future<WebAPI.Response.ExploreExploreMoveFloor> api = WebAPI.ExploreExploreMoveFloor(this.ExploreBox.GetRewardsId().ToArray(), suspendData.RandCnt, suspendData.DefeatEnemyCounts, suspendData.DefeatEnemyIds, this.FloorData.ID, suspendData.EnemyDamage, new int?(suspendData.EnemyId), this.LoseCount, this.Progress, suspendData.State, suspendData.TakeOver, targetFloorId, suspendData.Rest, this.WinCount);
    IEnumerator e = api.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (api.Result == null)
    {
      if (errorCallback != null)
        errorCallback();
    }
    else
    {
      this.DefeatEnemyIDs = new SortedDictionary<int, int>();
      this.UpdateBlUnitsFromPlayerDeck();
      this.SetupLocalPush();
    }
  }

  public void SetupLocalPush()
  {
    if (this.IsResuming)
      return;
    int maxSec = Mathf.Clamp(this.RankingPeriod != null ? (int) (this.ConvertMasterTimeToLocal(this.RankingPeriod.end_at.Value) - ServerTime.NowAppTimeAddDelta()).TotalSeconds : 0, 0, 43200);
    TimeSpan spanBoxMax;
    TimeSpan spanProgMax;
    if (Singleton<ExploreLotteryCore>.GetInstance().IsNeedLocalNotification(maxSec, out spanBoxMax, out spanProgMax))
    {
      DateTime dateTime1 = ServerTime.NowAppTimeAddDelta();
      DateTime dateTime2 = dateTime1 + spanBoxMax;
      Persist.notification.Data.ExploreBoxSpan = dateTime2.Hour > 7 ? (int) spanBoxMax.TotalSeconds : 0;
      dateTime2 = dateTime1 + spanProgMax;
      Persist.notification.Data.ExploreProgSpan = dateTime2.Hour > 7 ? (int) spanProgMax.TotalSeconds : 0;
    }
    else
    {
      Persist.notification.Data.ExploreBoxSpan = 0;
      Persist.notification.Data.ExploreProgSpan = 0;
    }
    Persist.notification.Flush();
  }

  public void ReloadExploreBox()
  {
    this.ExploreBox = new ExploreBox();
    this.ExploreBox.AddRewards(SMManager.Get<ExploreProgress>().box_reward_ids);
  }

  public ExploreRankingPeriod RankingPeriod { get; private set; }

  public bool IsRankingPeriodFinished
  {
    get
    {
      return this.RankingPeriod == null || this.ConvertMasterTimeToLocal(this.RankingPeriod.end_at.Value) <= ServerTime.NowAppTimeAddDelta();
    }
  }

  public bool IsRankingAcceptanceFinish { get; private set; }

  public bool HasRankingResult() => false;

  public void UpdateRankingViewInfo()
  {
  }

  public bool LoginCalcDirty { get; set; }

  public LoginReportInfo LoginReportInfo { get; private set; }

  public void StartLoginCalc()
  {
    this.LoginCalcDirty = true;
    this.StartCoroutine(this.ResumeExplore());
  }

  public bool IsNextFloor { get; set; }

  public ExploreFloor FloorData { get; private set; }

  public int NowFloor => this.FloorData.floor;

  public int FrontFloorId { get; private set; }

  public bool IsFrontFloor => this.FloorData.ID == this.FrontFloorId;

  public IEnumerator AddFloor()
  {
    int preFloor = this.FloorData.floor;
    yield return (object) this.LoadSuspendData(true);
    if (this.FloorData.floor != preFloor && this.IsFrontFloor)
    {
      this.IsNextFloor = true;
      this.AddLog(string.Format("{0}{1}階の探索を開始した", (object) this.FloorData.name, (object) this.NowFloor), Color.white);
    }
  }

  public int Progress
  {
    get => (int) ((Decimal) this.FloorElapsedTime / (Decimal) this.FloorRequiredTime * 1000000M);
  }

  public long FloorRequiredTime
  {
    get => this.FloorData == null ? long.MaxValue : (long) (this.FloorData.required_seconds * 1000);
  }

  public long FloorElapsedTime
  {
    set => this.mFloorElapsedTime = Math.Min(value, this.FloorRequiredTime);
    get => this.mFloorElapsedTime;
  }

  public void AddLog(string message, Color color) => this.AddLog(new LogData(message, color));

  public void AddLog(LogData logData)
  {
    this.mLogQueue.Enqueue(logData);
    if (this.mLogQueue.Count <= 20)
      return;
    this.mLogQueue.Dequeue();
  }

  public IEnumerable<LogData> GetAllLog() => (IEnumerable<LogData>) this.mLogQueue;

  public bool IsLatestLog(LogData logData)
  {
    return this.mLogQueue.Count > 0 && this.mLogQueue.Last<LogData>() == logData;
  }

  public List<LogData> GetNewLogDatas(LogData lastGotLog)
  {
    List<LogData> newLogDatas = new List<LogData>();
    foreach (LogData logData in this.mLogQueue.Reverse<LogData>())
    {
      if (logData != lastGotLog)
        newLogDatas.Add(logData);
      else
        break;
    }
    newLogDatas.Reverse();
    return newLogDatas;
  }

  public void CleanLog() => this.mLogQueue = new Queue<LogData>();

  public SortedDictionary<int, int> FloorDropDeck { get; private set; }

  private void UpdateFloorDropDeck()
  {
    this.FloorDropDeck = new SortedDictionary<int, int>();
    int key = 0;
    foreach (ExploreDropTable exploreDropTable in ((IEnumerable<ExploreDropTable>) MasterData.ExploreDropTableList).Where<ExploreDropTable>((Func<ExploreDropTable, bool>) (x => x.deck_id == this.FloorData.drop_deck_id)))
    {
      key += exploreDropTable.drop_ratio;
      this.FloorDropDeck.Add(key, exploreDropTable.drop_reward_id);
    }
  }

  public bool IsNotRegisteredDeck()
  {
    ExploreDeck[] exploreDeckArray = SMManager.Get<ExploreDeck[]>();
    return exploreDeckArray == null || exploreDeckArray.Length < 2 || exploreDeckArray[0] == null || exploreDeckArray[0].player_unit_ids.Length < 1 || exploreDeckArray[1] == null || exploreDeckArray[1].player_unit_ids.Length < 1;
  }

  public PlayerUnit GetExploreMainUnit() => this.mBlUnits[0].BlUnit.playerUnit;

  public PlayerUnit[] GetExploreUnits()
  {
    return ((IEnumerable<EpPlayerUnit>) this.mBlUnits).Select<EpPlayerUnit, PlayerUnit>((Func<EpPlayerUnit, PlayerUnit>) (x => x?.BlUnit.playerUnit)).ToArray<PlayerUnit>();
  }

  public void UpdateBlUnitsFromPlayerDeck()
  {
    ExploreDeck[] exploreDeckArray = SMManager.Get<ExploreDeck[]>();
    if (exploreDeckArray == null)
      return;
    PlayerUnit[] playerUnits = exploreDeckArray[0].player_units;
    this.mBlUnits = new EpPlayerUnit[5];
    for (int index = 0; index < playerUnits.Length; ++index)
    {
      if (!(playerUnits[index] == (PlayerUnit) null))
        this.mBlUnits[index] = new EpPlayerUnit(this.ConvertPlayerUnitToBlUnit(playerUnits[index], false), index);
    }
  }

  public PlayerDeck GetExploreDeck() => this.GetDeck(0);

  public PlayerDeck GetChallengeDeck() => this.GetDeck(1);

  public PlayerDeck GetDeck(int deck_index)
  {
    PlayerDeck deck = new PlayerDeck();
    ExploreDeck[] exploreDeckArray = SMManager.Get<ExploreDeck[]>();
    if (exploreDeckArray == null || exploreDeckArray[deck_index] == null)
    {
      Debug.LogError((object) "deck is empty! -- if (exploreDeck == null || exploreDeck[deck_index] == null)");
      return deck;
    }
    deck.player_unit_ids = new int?[5];
    int index = 0;
    foreach (int? playerUnitId in exploreDeckArray[deck_index].player_unit_ids)
    {
      int? unit_id = playerUnitId;
      PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (u =>
      {
        if (!(u != (PlayerUnit) null))
          return false;
        int id = u.id;
        int? nullable = unit_id;
        int valueOrDefault = nullable.GetValueOrDefault();
        return id == valueOrDefault & nullable.HasValue;
      }));
      if (playerUnit != (PlayerUnit) null)
      {
        deck.player_units[index] = playerUnit;
        deck.player_unit_ids[index] = unit_id;
        ++index;
      }
    }
    return deck;
  }

  public IEnumerator RegistrationInitialDeck(Action errorCallback)
  {
    ExploreDataManager exploreDataManager = this;
    bool isError = false;
    PlayerDeck playerDeck = ((IEnumerable<PlayerDeck>) SMManager.Get<PlayerDeck[]>()).First<PlayerDeck>();
    int[] player_unit_ids = ((IEnumerable<int?>) playerDeck.player_unit_ids).Where<int?>((Func<int?, bool>) (x => x.HasValue)).Select<int?, int>((Func<int?, int>) (x => x.Value)).ToArray<int>();
    int totalCombat = playerDeck.total_combat;
    // ISSUE: reference to a compiler-generated method
    ExploreFloor exploreFloor = ((IEnumerable<ExploreFloor>) MasterData.ExploreFloorList).FirstOrDefault<ExploreFloor>(new Func<ExploreFloor, bool>(exploreDataManager.\u003CRegistrationInitialDeck\u003Eb__147_2));
    int floorId = exploreFloor != null ? exploreFloor.ID : 1;
    Future<WebAPI.Response.ExploreDeckEdit> webApi = WebAPI.ExploreDeckEdit(new int[0], "0", 1, new int[0], new int[0], floorId, 0, new int?(), 0, player_unit_ids, 0, 0, 0, totalCombat, 0, 0);
    IEnumerator e = webApi.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    isError |= webApi.Result == null;
    webApi = (Future<WebAPI.Response.ExploreDeckEdit>) null;
    if (!isError)
    {
      webApi = WebAPI.ExploreDeckEdit(new int[0], "0", 2, new int[0], new int[0], floorId, 0, new int?(), 0, player_unit_ids, 0, 0, 0, totalCombat, 0, 0);
      e = webApi.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      isError |= webApi.Result == null;
      webApi = (Future<WebAPI.Response.ExploreDeckEdit>) null;
    }
    if (isError)
      errorCallback();
  }

  public ExploreDataManager.PopupStateType mPopupState { get; set; }

  public bool IsReopenPopup => this.mPopupState != 0;

  public bool IsPopupStateDeckEdit
  {
    get => this.mPopupState == ExploreDataManager.PopupStateType.ePopup_DeckEdit;
  }

  public bool IsPopupStateChallenge
  {
    get => this.mPopupState == ExploreDataManager.PopupStateType.ePopup_Challenge;
  }

  public void InitReopenPopupState()
  {
    this.mPopupState = ExploreDataManager.PopupStateType.ePopup_None;
  }

  public void SetReopenPopupStateDeckEdit()
  {
    this.mPopupState = ExploreDataManager.PopupStateType.ePopup_DeckEdit;
  }

  public void SetReopenPopupStateChallenge()
  {
    this.mPopupState = ExploreDataManager.PopupStateType.ePopup_Challenge;
  }

  public bool NeedShowBadge
  {
    get
    {
      return this.IsFrontFloor && this.Progress >= 1000000 && Singleton<NGGameDataManager>.GetInstance().challenge_point > 0 || this.ExploreBox.IsRewardsMax;
    }
  }

  private struct EpBattleParam
  {
    public int Hp;
    public int Atk;
    public int Def;
    public float WeakPointCoefficient;

    public static ExploreDataManager.EpBattleParam CreatePlayerParam(
      EpPlayerUnit[] team,
      WeakPoint enemyWeakPoint)
    {
      ExploreDataManager.EpBattleParam playerParam = new ExploreDataManager.EpBattleParam();
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      foreach (EpPlayerUnit epUnit in team)
      {
        if (epUnit != null)
        {
          int hp = epUnit.Hp;
          int def = epUnit.Def;
          int atk = epUnit.Atk;
          float effectiveCoefficient = enemyWeakPoint.GetEffectiveCoefficient(epUnit);
          int num4 = (int) ((double) atk * (double) effectiveCoefficient);
          int num5 = (int) ((double) def * (double) effectiveCoefficient);
          num1 += hp;
          num3 += num5;
          num2 += num4;
          if ((double) playerParam.WeakPointCoefficient < (double) effectiveCoefficient)
            playerParam.WeakPointCoefficient = effectiveCoefficient;
        }
      }
      playerParam.Hp = num1;
      playerParam.Atk = num2;
      playerParam.Def = num3;
      return playerParam;
    }

    public static ExploreDataManager.EpBattleParam CreateEnemyParam(BL.Unit enemy)
    {
      return new ExploreDataManager.EpBattleParam()
      {
        Hp = enemy.playerUnit.hp.initial,
        Atk = Mathf.Max(enemy.playerUnit.strength.initial, enemy.playerUnit.intelligence.initial),
        Def = enemy.playerUnit.vitality.initial,
        WeakPointCoefficient = 1f
      };
    }
  }

  public enum PopupStateType
  {
    ePopup_None,
    ePopup_DeckEdit,
    ePopup_Challenge,
  }
}
