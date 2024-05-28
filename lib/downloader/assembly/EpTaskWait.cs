// Decompiled with JetBrains decompiler
// Type: EpTaskWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
public class EpTaskWait : ExploreTask, IEpAdjustableTask
{
  private long mReqiredTime;
  private long mTakeOverTime;

  private EpTaskWait()
  {
  }

  public static EpTaskWait Create()
  {
    EpTaskWait epTaskWait = new EpTaskWait();
    epTaskWait.mReqiredTime = Singleton<ExploreDataManager>.GetInstance().TimeConfig["WAIT"];
    epTaskWait.SetRandState();
    return epTaskWait;
  }

  public override Explore.STATE State() => Explore.STATE.WAIT;

  public override IEnumerator LoadAsync()
  {
    EpTaskWait epTaskWait = this;
    if (Singleton<ExploreDataManager>.GetInstance().IsNextFloor)
      yield return (object) Singleton<ExploreSceneManager>.GetInstance().ScreenEffect.LoadNextFloorEffect();
    epTaskWait.IsLoaded = true;
  }

  public override IEnumerator UpdateAsync()
  {
    EpTaskWait epTaskWait = this;
    epTaskWait.SetStartTime();
    ExploreModelController model = Singleton<ExploreSceneManager>.GetInstance().Model;
    ExploreScreenEffectController screen = Singleton<ExploreSceneManager>.GetInstance().ScreenEffect;
    bool isRevenge = Singleton<ExploreDataManager>.GetInstance().IsAliveLastDuelEnemy();
    model.ChangeMainCamera();
    model.WaitPlayerUnit();
    model.SetCampVisible(true);
    model.CloseTreasureBox();
    if (isRevenge)
    {
      screen.OpenLostWaitTimeCounter(epTaskWait.GetReqiredTime());
      model.LostWaitCampUnits();
    }
    else
    {
      screen.OpenWaitTimeCounter(epTaskWait.GetReqiredTime());
      model.WaitCampUnits();
    }
    screen.Transition2dOut();
    screen.TransitionFullOut();
    screen.StartInfoEffect();
    while (epTaskWait.ProcTime < epTaskWait.GetReqiredTime())
      yield return (object) null;
    screen.CloseTimeCounter();
    if (!isRevenge)
    {
      screen.Transition2dIn();
      yield return (object) screen.WaitForTransition2d();
      model.SetCampVisible(false);
      screen.Transition2dOut();
      yield return (object) screen.WaitForTransition2d();
    }
    epTaskWait.IsFinished = true;
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
    Singleton<ExploreSceneManager>.GetInstance().PlayLoopSe("SE_2452");
  }

  public void SetTakeOverTime(long adjustTime) => this.mTakeOverTime = adjustTime;

  public override long OnBackGroundWork(long calcTime)
  {
    long val1 = calcTime - this.mTakeOverTime;
    this.mTakeOverTime = Math.Max(this.mTakeOverTime - calcTime, 0L);
    if (val1 > 0L)
    {
      long num = Math.Min(val1, this.mReqiredTime);
      this.mReqiredTime -= num;
      val1 -= num;
    }
    return val1;
  }

  public override long OnSimulate(ref long simElapsed, ref int simBoxCnt) => this.GetReqiredTime();

  public override void OnContinue(long restReqTime, long takeOverTime)
  {
    this.mReqiredTime = restReqTime;
    this.mTakeOverTime = takeOverTime;
  }
}
