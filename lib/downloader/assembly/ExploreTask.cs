// Decompiled with JetBrains decompiler
// Type: ExploreTask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public abstract class ExploreTask
{
  private float mStartTime;
  private string mRandState;

  protected long ProcTime
  {
    get
    {
      return (double) this.mStartTime == 0.0 ? 0L : (long) (((double) Time.realtimeSinceStartup - (double) this.mStartTime) * 1000.0);
    }
  }

  public bool IsLoaded { get; protected set; }

  public bool IsFinished { get; protected set; }

  protected void SetStartTime() => this.mStartTime = Time.realtimeSinceStartup;

  public void SetRandState()
  {
    this.mRandState = Singleton<ExploreLotteryCore>.GetInstance().GetRandomCount().ToString();
  }

  public string GetRandState() => this.mRandState;

  public abstract Explore.STATE State();

  public abstract IEnumerator LoadAsync();

  public abstract IEnumerator UpdateAsync();

  public abstract void PayOut();

  public abstract long GetReqiredTime();

  public abstract long GetRestReqiredTime();

  public abstract long GetTakeOverTime();

  public abstract void OnBackExplore();

  public abstract long OnBackGroundWork(long calcTime);

  public abstract long OnSimulate(ref long simElapsed, ref int simBoxCnt);

  public abstract void OnContinue(long restReqTime, long takeOverTime);
}
