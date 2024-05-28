// Decompiled with JetBrains decompiler
// Type: ExploreLotteryCore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Explore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
[DefaultExecutionOrder(-1)]
public class ExploreLotteryCore : Singleton<ExploreLotteryCore>
{
  private const long FULL_CALC_TIME_MAX = 86400000;
  private ExploreLotteryCore.ExploreRandom mRandom;
  private bool mTaskReloadDirty;
  private bool mDeckEditedDirty;
  private ExploreTask mLastPlayTask;
  private ExploreDataManager mDataManager;

  protected override void Initialize()
  {
    this.mDataManager = Singleton<ExploreDataManager>.GetInstance();
  }

  public IEnumerator CalcTaskOnBackGround(TimeSpan calcSpan, bool easyCalc = true, Action errorCallback = null)
  {
    if (calcSpan.Ticks < 0L)
    {
      errorCallback();
      Debug.LogError((object) "explore can`t calc minus span");
    }
    else
    {
      long restCalcTime = easyCalc ? Math.Min((long) calcSpan.TotalMilliseconds, 86400000L) : (long) calcSpan.TotalMilliseconds;
      long easyCalcTime = easyCalc ? (long) calcSpan.TotalMilliseconds - 86400000L : 0L;
      long beforeFloorElapsedTime = this.mDataManager.FloorElapsedTime;
      int beforeDefeatCount = this.mDataManager.DefeatEnemyIDs.Sum<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, int>) (x => x.Value));
      int beforeRewardCount = this.mDataManager.ExploreBox.GetRewardsId().Count;
      bool deckEdited = this.mDeckEditedDirty;
      this.mDeckEditedDirty = false;
      if (this.mLastPlayTask != null)
        restCalcTime = this.mLastPlayTask.OnBackGroundWork(restCalcTime);
      float realtimeSinceStartup = Time.realtimeSinceStartup;
      for (; restCalcTime > 0L; restCalcTime = this.mLastPlayTask.OnBackGroundWork(restCalcTime))
      {
        if ((double) Time.realtimeSinceStartup - (double) realtimeSinceStartup > 0.033333335071802139)
        {
          yield return (object) null;
          realtimeSinceStartup = Time.realtimeSinceStartup;
        }
        this.SetLastPlayTask(this.LotteryNextTask());
      }
      if (easyCalcTime > 0L)
        this.EasyCalc(easyCalcTime, beforeFloorElapsedTime, beforeDefeatCount, beforeRewardCount);
      if (deckEdited)
      {
        this.mDeckEditedDirty = true;
        long takeOverTime = this.mLastPlayTask.GetTakeOverTime();
        this.SetLastPlayTask(this.LotteryNextTask());
        (this.mLastPlayTask as IEpAdjustableTask).SetTakeOverTime(takeOverTime);
      }
    }
  }

  public void EasyCalc(
    long calcTime,
    long beforFloorElapsedTime,
    int beforDefeatCount,
    int beforRewardCount)
  {
    this.mDataManager.FloorElapsedTime += (this.mDataManager.FloorElapsedTime - beforFloorElapsedTime) * calcTime / 86400000L;
    int num1 = (int) ((long) (this.mDataManager.DefeatEnemyIDs.Sum<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, int>) (x => x.Value)) - beforDefeatCount) * calcTime / 86400000L);
    ExploreEnemy exploreEnemy = this.mDataManager.FloorEnemyList.First<KeyValuePair<int, ExploreEnemy>>().Value;
    if (!this.mDataManager.DefeatEnemyIDs.ContainsKey(exploreEnemy.ID))
      this.mDataManager.DefeatEnemyIDs.Add(exploreEnemy.ID, 0);
    this.mDataManager.DefeatEnemyIDs[exploreEnemy.ID] += num1;
    this.mDataManager.ExploreBox.Add(exploreEnemy.zeny * num1, exploreEnemy.trust_rate * (float) num1, exploreEnemy.player_exp * num1, exploreEnemy.unit_exp * num1);
    long num2 = (long) (this.mDataManager.ExploreBox.GetRewardsId().Count - beforRewardCount) * calcTime / 86400000L;
    for (int index = 0; (long) index < num2 && !this.mDataManager.ExploreBox.IsRewardsMax; ++index)
      this.mDataManager.ExploreBox.AddReward(this.LotteryFloorDropRewardId());
  }

  public bool IsNeedLocalNotification(
    int maxSec,
    out TimeSpan spanBoxMax,
    out TimeSpan spanProgMax)
  {
    ExploreTask nowTask = this.mLastPlayTask;
    int seed = SMManager.Get<ExploreProgress>().seed.Value;
    string randState = nowTask.GetRandState();
    long floorElapsedTime = this.mDataManager.FloorElapsedTime;
    long floorRequiredTime = this.mDataManager.FloorRequiredTime;
    int count = this.mDataManager.ExploreBox.GetRewardsId().Count;
    int num1 = 40;
    bool flag1 = count >= num1;
    bool flag2 = floorElapsedTime >= floorRequiredTime || Singleton<NGGameDataManager>.GetInstance().challenge_point < 1;
    int num2 = 0;
    int seconds1 = 0;
    int seconds2 = 0;
    while (num2 < maxSec)
    {
      num2 += (int) (nowTask.OnSimulate(ref floorElapsedTime, ref count) / 1000L);
      nowTask = this.lotteryNextTask(nowTask);
      if (!flag1 && count >= num1)
      {
        flag1 = true;
        seconds1 = num2;
      }
      if (!flag2 && floorElapsedTime >= floorRequiredTime)
      {
        flag2 = true;
        seconds2 = num2;
      }
      if (flag1 & flag2)
        break;
    }
    spanBoxMax = new TimeSpan(0, 0, seconds1);
    spanProgMax = new TimeSpan(0, 0, seconds2);
    this.mRandom = new ExploreLotteryCore.ExploreRandom(seed, long.Parse(randState));
    if (flag1 && seconds1 > 0)
      return true;
    return flag2 && seconds2 > 0;
  }

  public void SetTaskReloadDirty() => this.mTaskReloadDirty = true;

  public void SetDeckEditedDirty() => this.mDeckEditedDirty = true;

  public void SetLastPlayTask(ExploreTask task) => this.mLastPlayTask = task;

  public void SetSuspendData(ref SuspendData data)
  {
    if (this.mLastPlayTask == null)
      return;
    data.State = (int) this.mLastPlayTask.State();
    data.Rest = (int) this.mLastPlayTask.GetRestReqiredTime();
    data.TakeOver = (int) this.mLastPlayTask.GetTakeOverTime();
    data.RandCnt = this.mLastPlayTask.GetRandState();
  }

  public void LoadSuspendData()
  {
    ExploreProgress exploreProgress = SMManager.Get<ExploreProgress>();
    this.mRandom = new ExploreLotteryCore.ExploreRandom(exploreProgress.seed.Value, long.Parse(exploreProgress.count));
    switch (exploreProgress.state)
    {
      case 2:
        this.mLastPlayTask = (ExploreTask) EpTaskWait.Create();
        this.mLastPlayTask.OnContinue((long) exploreProgress.waiting_time, (long) exploreProgress.takeover_time);
        break;
      case 3:
        this.mLastPlayTask = (ExploreTask) EpTaskExplore.Create();
        this.mLastPlayTask.OnContinue((long) exploreProgress.waiting_time, (long) exploreProgress.takeover_time);
        break;
      case 4:
        this.mLastPlayTask = (ExploreTask) EpTaskExplore.Create();
        (this.mLastPlayTask as IEpAdjustableTask).SetTakeOverTime((long) exploreProgress.takeover_time);
        break;
      case 5:
        this.mLastPlayTask = !this.mDataManager.IsAliveLastDuelEnemy() ? (ExploreTask) EpTaskExplore.Create() : (ExploreTask) EpTaskLostWait.Create();
        (this.mLastPlayTask as IEpAdjustableTask).SetTakeOverTime((long) exploreProgress.takeover_time);
        break;
      case 6:
        this.mLastPlayTask = (ExploreTask) EpTaskLostWait.Create();
        this.mLastPlayTask.OnContinue((long) exploreProgress.waiting_time, (long) exploreProgress.takeover_time);
        break;
      default:
        this.mLastPlayTask = (ExploreTask) null;
        break;
    }
  }

  public ExploreTask LotteryNextTask() => this.lotteryNextTask(this.mLastPlayTask);

  private ExploreTask lotteryNextTask(ExploreTask nowTask)
  {
    ExploreTask exploreTask;
    if (this.mDeckEditedDirty)
    {
      this.mDeckEditedDirty = false;
      exploreTask = (ExploreTask) EpTaskWait.Create();
    }
    else if (nowTask == null)
      exploreTask = (ExploreTask) EpTaskExplore.Create();
    else if (this.mTaskReloadDirty)
    {
      if (nowTask is IEpAdjustableTask)
      {
        exploreTask = nowTask;
      }
      else
      {
        long takeOverTime = nowTask.GetTakeOverTime();
        exploreTask = this.lotteryNextTask(nowTask.State());
        (exploreTask as IEpAdjustableTask).SetTakeOverTime(takeOverTime);
      }
    }
    else
      exploreTask = this.lotteryNextTask(nowTask.State());
    this.mTaskReloadDirty = false;
    return exploreTask;
  }

  private ExploreTask lotteryNextTask(Explore.STATE nowState)
  {
    switch (nowState)
    {
      case Explore.STATE.WAIT:
        return this.mDataManager.IsAliveLastDuelEnemy() ? (ExploreTask) EpTaskEnemyRevenge.Create() : (ExploreTask) EpTaskExplore.Create();
      case Explore.STATE.EXPLORE:
        return this.mRandom.Range(0, 10000) < this.mDataManager.FloorData.encount_ratio ? (this.mDataManager.FloorEnemyList.Count > 0 ? (ExploreTask) EpTaskEnemyBattle.Create() : (ExploreTask) EpTaskExplore.Create()) : (this.mDataManager.ExploreBox.IsRewardsMax ? (ExploreTask) EpTaskExplore.Create() : (ExploreTask) EpTaskTreasure.Create());
      case Explore.STATE.BATTLE:
        return this.mDataManager.IsAliveLastDuelEnemy() ? (ExploreTask) EpTaskLostWait.Create() : (ExploreTask) EpTaskExplore.Create();
      case Explore.STATE.LOST_WAIT:
        return this.mDataManager.IsAliveLastDuelEnemy() ? (ExploreTask) EpTaskEnemyRevenge.Create() : (ExploreTask) EpTaskEnemyBattle.Create();
      default:
        return (ExploreTask) EpTaskExplore.Create();
    }
  }

  public ExploreEnemy LotteryNextEnemy()
  {
    SortedDictionary<int, ExploreEnemy> floorEnemyList = this.mDataManager.FloorEnemyList;
    int ran = this.mRandom.Range(0, floorEnemyList.Max<KeyValuePair<int, ExploreEnemy>>((Func<KeyValuePair<int, ExploreEnemy>, int>) (x => x.Key)));
    return floorEnemyList.FirstOrDefault<KeyValuePair<int, ExploreEnemy>>((Func<KeyValuePair<int, ExploreEnemy>, bool>) (x => x.Key > ran)).Value;
  }

  public int LotteryEnemyDropRewardId(ExploreEnemy enemy)
  {
    if (this.mRandom.Range(0, 10000) >= enemy.drop_ratio)
      return 0;
    SortedDictionary<int, int> dropDeck = enemy.DropDeck;
    int ran = this.mRandom.Range(0, dropDeck.Max<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, int>) (x => x.Key)));
    return dropDeck.FirstOrDefault<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, bool>) (x => x.Key > ran)).Value;
  }

  public int LotteryFloorDropRewardId()
  {
    SortedDictionary<int, int> floorDropDeck = this.mDataManager.FloorDropDeck;
    int ran = this.mRandom.Range(0, floorDropDeck.Max<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, int>) (x => x.Key)));
    return floorDropDeck.FirstOrDefault<KeyValuePair<int, int>>((Func<KeyValuePair<int, int>, bool>) (x => x.Key > ran)).Value;
  }

  public long GetRandomCount() => this.mRandom.Count;

  private class ExploreRandom
  {
    private Random.State mState;

    public long Count { get; private set; }

    private ExploreRandom()
    {
    }

    public ExploreRandom(int seed, long counter)
    {
      Random.State state = Random.state;
      Random.InitState(seed);
      for (int index = 0; (long) index < counter; ++index)
      {
        double num = (double) Random.value;
      }
      this.Count = counter;
      this.mState = Random.state;
      Random.state = state;
    }

    public int Range(int min, int max)
    {
      Random.State state = Random.state;
      ++this.Count;
      Random.state = this.mState;
      int num = Random.Range(min, max);
      this.mState = Random.state;
      Random.state = state;
      return num;
    }

    public float value
    {
      get
      {
        Random.State state = Random.state;
        ++this.Count;
        Random.state = this.mState;
        double num = (double) Random.value;
        this.mState = Random.state;
        Random.state = state;
        return (float) num;
      }
    }
  }
}
