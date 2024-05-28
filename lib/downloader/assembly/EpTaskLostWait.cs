// Decompiled with JetBrains decompiler
// Type: EpTaskLostWait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;

#nullable disable
public class EpTaskLostWait : ExploreTask, IEpAdjustableTask
{
  private long mReqiredTime;

  private EpTaskLostWait()
  {
  }

  public static EpTaskLostWait Create()
  {
    EpTaskLostWait epTaskLostWait = new EpTaskLostWait();
    epTaskLostWait.mReqiredTime = Singleton<ExploreDataManager>.GetInstance().TimeConfig["LOST_WAIT"];
    epTaskLostWait.SetRandState();
    return epTaskLostWait;
  }

  public override Explore.STATE State() => Explore.STATE.LOST_WAIT;

  public override IEnumerator LoadAsync()
  {
    EpTaskLostWait epTaskLostWait = this;
    if (Singleton<ExploreDataManager>.GetInstance().IsNextFloor)
      yield return (object) Singleton<ExploreSceneManager>.GetInstance().ScreenEffect.LoadNextFloorEffect();
    epTaskLostWait.IsLoaded = true;
  }

  public override IEnumerator UpdateAsync()
  {
    EpTaskLostWait epTaskLostWait = this;
    epTaskLostWait.SetStartTime();
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    ExploreModelController model = Singleton<ExploreSceneManager>.GetInstance().Model;
    ExploreScreenEffectController screen = Singleton<ExploreSceneManager>.GetInstance().ScreenEffect;
    instance.IsWinRateUpdate = true;
    screen.OpenLostWaitTimeCounter(epTaskLostWait.GetReqiredTime());
    model.WaitPlayerUnit();
    model.SetCampVisible(true);
    model.LostWaitCampUnits();
    screen.TransitionFullOut();
    screen.StartInfoEffect();
    while (epTaskLostWait.ProcTime < epTaskLostWait.GetReqiredTime())
      yield return (object) null;
    screen.CloseTimeCounter();
    epTaskLostWait.IsFinished = true;
  }

  public override void PayOut()
  {
  }

  public override long GetReqiredTime() => this.mReqiredTime;

  public override long GetRestReqiredTime() => this.GetReqiredTime() - this.ProcTime;

  public override long GetTakeOverTime() => this.GetReqiredTime() - this.ProcTime;

  public override void OnBackExplore()
  {
    Singleton<ExploreSceneManager>.GetInstance().PlayLoopSe("SE_2452");
  }

  public void SetTakeOverTime(long adjustTime) => this.mReqiredTime += adjustTime;

  public override long OnBackGroundWork(long calcTime)
  {
    long num = calcTime - this.GetReqiredTime();
    this.mReqiredTime -= calcTime;
    return num;
  }

  public override long OnSimulate(ref long simElapsed, ref int simBoxCnt) => this.GetReqiredTime();

  public override void OnContinue(long restReqTime, long takeOverTime)
  {
    this.mReqiredTime = restReqTime;
  }
}
