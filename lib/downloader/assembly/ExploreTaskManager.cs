// Decompiled with JetBrains decompiler
// Type: ExploreTaskManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (ExploreSceneManager))]
public class ExploreTaskManager : MonoBehaviour
{
  private Explore.STATE mState = Explore.STATE.INITIALIZE;
  private Queue<ExploreTask> mTasks = new Queue<ExploreTask>();
  private ExploreTask mCurrentTask;
  private IEnumerator mCurrentUpdate;
  private IEnumerator mCurrentLoading;
  private bool mFirstRunTask = true;
  private bool mPause = true;
  private long mTakeOverTime;
  private NGSceneManager mSceneManager;
  private ExploreSceneManager mExploreManager;
  private ExploreDataManager mExploreDataManager;

  public bool IsStateWait => this.mState == Explore.STATE.WAIT;

  public bool IsStateExlore => this.mState == Explore.STATE.EXPLORE;

  public bool IsStateLostWait => this.mState == Explore.STATE.LOST_WAIT;

  private void Awake()
  {
    this.mSceneManager = Singleton<NGSceneManager>.GetInstance();
    this.mExploreManager = Singleton<ExploreSceneManager>.GetInstance();
    this.mExploreDataManager = Singleton<ExploreDataManager>.GetInstance();
  }

  private void OnDestroy() => this.ClearTask();

  private void Update()
  {
    if (this.mPause || this.mState == Explore.STATE.INITIALIZE)
      return;
    if (this.mExploreDataManager.IsRankingPeriodFinished)
      this.StartCoroutine(this.TransitionMyPage());
    else if (this.mExploreManager.ReloadDirty)
    {
      if (this.mExploreManager.IsBackScreen)
        return;
      this.mExploreManager.StartReload();
    }
    else
    {
      if (this.mCurrentTask == null)
        this.StartTask();
      if (this.mTasks.Count < 1)
        this.AddNextTask();
      if (this.mCurrentTask == null || !this.mCurrentTask.IsFinished)
        return;
      this.CleanupCurrentTask();
    }
  }

  public void Pause(bool pause)
  {
    if (this.mPause == pause)
      return;
    this.mPause = pause;
    if (this.mPause)
    {
      if (this.mCurrentLoading != null)
        this.StopCoroutine(this.mCurrentLoading);
      if (this.mCurrentUpdate == null)
        return;
      this.StopCoroutine(this.mCurrentUpdate);
    }
    else
    {
      if (this.mCurrentLoading != null)
        this.StartCoroutine(this.mCurrentLoading);
      if (this.mCurrentUpdate == null)
        return;
      this.StartCoroutine(this.mCurrentUpdate);
    }
  }

  public void Initialize()
  {
    this.mState = Explore.STATE.INITIALIZE;
    this.ClearTask();
    this.mTakeOverTime = 0L;
    this.mFirstRunTask = true;
    this.mState = Explore.STATE.WAIT;
  }

  public void OnBackExploreScene()
  {
    if (this.mCurrentTask == null)
      return;
    this.mCurrentTask.OnBackExplore();
  }

  private void StartTask()
  {
    this.mCurrentTask = this.GetNextTask();
    if (this.mCurrentTask == null)
      return;
    Singleton<ExploreLotteryCore>.GetInstance().SetLastPlayTask(this.mCurrentTask);
    if (!this.mFirstRunTask && this.mCurrentTask is IEpAdjustableTask mCurrentTask1)
    {
      mCurrentTask1.SetTakeOverTime(this.mTakeOverTime);
      this.mTakeOverTime = 0L;
    }
    if (this.mCurrentTask is IEpBattleTask mCurrentTask2)
      mCurrentTask2.SetBattleCache();
    this.mCurrentUpdate = this.mCurrentTask.UpdateAsync();
    if (!this.mPause)
      this.StartCoroutine(this.mCurrentUpdate);
    this.mState = this.mCurrentTask.State();
    this.mCurrentTask.PayOut();
    this.mFirstRunTask = false;
  }

  private void CleanupCurrentTask()
  {
    if (this.mCurrentUpdate != null)
      this.StopCoroutine(this.mCurrentUpdate);
    if (this.mCurrentTask != null)
      this.mTakeOverTime += this.mCurrentTask.GetTakeOverTime();
    this.mCurrentUpdate = (IEnumerator) null;
    this.mCurrentTask = (ExploreTask) null;
  }

  private ExploreTask GetNextTask()
  {
    if (this.mTasks.Count < 1)
      return (ExploreTask) null;
    return !this.mTasks.Peek().IsLoaded ? (ExploreTask) null : this.mTasks.Dequeue();
  }

  private void AddNextTask()
  {
    ExploreTask exploreTask = Singleton<ExploreLotteryCore>.GetInstance().LotteryNextTask();
    this.mCurrentLoading = exploreTask.LoadAsync();
    if (!this.mPause)
      this.StartCoroutine(this.mCurrentLoading);
    this.mTasks.Enqueue(exploreTask);
  }

  private void ClearTask()
  {
    if (this.mCurrentLoading != null)
    {
      if (!this.mPause)
        this.StopCoroutine(this.mCurrentLoading);
      this.mCurrentLoading = (IEnumerator) null;
    }
    if (this.mCurrentUpdate != null)
    {
      if (!this.mPause)
        this.StopCoroutine(this.mCurrentUpdate);
      this.mCurrentUpdate = (IEnumerator) null;
    }
    this.mTasks.Clear();
    this.mCurrentTask = (ExploreTask) null;
  }

  private IEnumerator TransitionMyPage()
  {
    Singleton<PopupManager>.GetInstance().closeAll(true);
    this.mExploreManager.Pause(true);
    ExploreScreenEffectController screenEffect = this.mExploreManager.ScreenEffect;
    screenEffect.TransitionFullIn();
    yield return (object) screenEffect.WaitForTransitionFull();
    MypageScene.ChangeScene();
  }
}
