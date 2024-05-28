// Decompiled with JetBrains decompiler
// Type: EpTaskExplore
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EpTaskExplore : ExploreTask, IEpAdjustableTask
{
  private long mReqiredTime;
  private long mTakeOverTime;
  private GameObject exploreNextFloorPrefab;

  private EpTaskExplore()
  {
  }

  public static EpTaskExplore Create()
  {
    EpTaskExplore epTaskExplore = new EpTaskExplore();
    epTaskExplore.mReqiredTime = Singleton<ExploreDataManager>.GetInstance().TimeConfig["EXPLORE"];
    epTaskExplore.SetRandState();
    return epTaskExplore;
  }

  public override Explore.STATE State() => Explore.STATE.EXPLORE;

  public override IEnumerator LoadAsync()
  {
    EpTaskExplore epTaskExplore = this;
    if (Singleton<ExploreDataManager>.GetInstance().IsNextFloor)
      yield return (object) Singleton<ExploreSceneManager>.GetInstance().ScreenEffect.LoadNextFloorEffect();
    epTaskExplore.IsLoaded = true;
  }

  public override IEnumerator UpdateAsync()
  {
    EpTaskExplore epTaskExplore = this;
    epTaskExplore.SetStartTime();
    ExploreDataManager data = Singleton<ExploreDataManager>.GetInstance();
    ExploreModelController model = Singleton<ExploreSceneManager>.GetInstance().Model;
    ExploreScreenEffectController screenEffect = Singleton<ExploreSceneManager>.GetInstance().ScreenEffect;
    long startfloorElpseTime = data.FloorElapsedTime;
    data.IsWinRateUpdate = true;
    screenEffect.TransitionFullOut();
    model.SetCampVisible(false);
    model.RunPlayerUnit();
    model.ChangeMainCamera();
    screenEffect.StartInfoEffect();
    while (epTaskExplore.ProcTime < epTaskExplore.mTakeOverTime)
      yield return (object) null;
    while (epTaskExplore.ProcTime < epTaskExplore.GetReqiredTime())
    {
      data.FloorElapsedTime = startfloorElpseTime + (epTaskExplore.ProcTime - epTaskExplore.mTakeOverTime);
      yield return (object) null;
    }
    epTaskExplore.IsFinished = true;
    data.FloorElapsedTime = startfloorElpseTime + epTaskExplore.mReqiredTime;
  }

  public override void PayOut()
  {
  }

  public override long GetReqiredTime() => this.mReqiredTime + this.mTakeOverTime;

  public override long GetRestReqiredTime()
  {
    return Math.Min(this.GetReqiredTime() - this.ProcTime, this.mReqiredTime);
  }

  public override long GetTakeOverTime() => Math.Max(this.mTakeOverTime - this.ProcTime, 0L);

  public override void OnBackExplore()
  {
  }

  public void SetTakeOverTime(long adjustTime) => this.mTakeOverTime = adjustTime;

  public override long OnBackGroundWork(long calcTime)
  {
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    long val1 = calcTime - this.mTakeOverTime;
    this.mTakeOverTime = Math.Max(this.mTakeOverTime - calcTime, 0L);
    if (val1 > 0L)
    {
      long num = Math.Min(val1, this.mReqiredTime);
      instance.FloorElapsedTime += num;
      this.mReqiredTime -= num;
      val1 -= num;
    }
    return val1;
  }

  public override long OnSimulate(ref long simElapsed, ref int simBoxCnt)
  {
    simElapsed += this.mReqiredTime;
    return this.GetReqiredTime();
  }

  public override void OnContinue(long restReqTime, long takeOverTime)
  {
    this.mReqiredTime = restReqTime;
    this.mTakeOverTime = takeOverTime;
  }
}
