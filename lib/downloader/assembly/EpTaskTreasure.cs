// Decompiled with JetBrains decompiler
// Type: EpTaskTreasure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class EpTaskTreasure : ExploreTask
{
  private long mReqiredTime;
  private int mRewardId;
  private ExploreDropReward mReward;

  private EpTaskTreasure()
  {
  }

  public static EpTaskTreasure Create()
  {
    EpTaskTreasure epTaskTreasure = new EpTaskTreasure();
    epTaskTreasure.mReqiredTime = Singleton<ExploreDataManager>.GetInstance().TimeConfig["TREASURE"];
    epTaskTreasure.mRewardId = Singleton<ExploreLotteryCore>.GetInstance().LotteryFloorDropRewardId();
    epTaskTreasure.SetRandState();
    return epTaskTreasure;
  }

  public override Explore.STATE State() => Explore.STATE.TREASURE;

  public override IEnumerator LoadAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EpTaskTreasure epTaskTreasure = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      epTaskTreasure.IsLoaded = true;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    MasterData.ExploreDropReward.TryGetValue(epTaskTreasure.mRewardId, out epTaskTreasure.mReward);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) Singleton<ExploreSceneManager>.GetInstance().ScreenEffect.LoadItemGetEffect();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override IEnumerator UpdateAsync()
  {
    EpTaskTreasure epTaskTreasure = this;
    epTaskTreasure.SetStartTime();
    if (!Singleton<ExploreSceneManager>.GetInstance().IsBackScreen && !Singleton<CommonRoot>.GetInstance().isLoading)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    ExploreModelController model = Singleton<ExploreSceneManager>.GetInstance().Model;
    ExploreScreenEffectController screen = Singleton<ExploreSceneManager>.GetInstance().ScreenEffect;
    screen.TransitionFullOut();
    model.OpenExclamationMark();
    yield return (object) new WaitForSeconds(0.75f);
    model.CloseExclamationMark();
    model.WaitPlayerUnit();
    model.ChangeTreasureCamera();
    model.PlayTreasureCamera();
    model.OpenTreasureBox();
    yield return (object) new WaitForSeconds(1f);
    Singleton<ExploreDataManager>.GetInstance().AddLog(string.Format("{0}を[00dc1e]獲得[-]した", (object) epTaskTreasure.mReward.reward_title), Color.white);
    if (!Singleton<ExploreSceneManager>.GetInstance().IsBackScreen && !Singleton<CommonRoot>.GetInstance().isLoading)
    {
      yield return (object) screen.OpenItemGetEffect(epTaskTreasure.mReward);
      yield return (object) new WaitForSeconds(3f);
      screen.CloseItemGetEffect();
    }
    else
      yield return (object) new WaitForSeconds(3f);
    screen.Transition2dIn();
    yield return (object) screen.WaitForTransition2d();
    model.CloseTreasureBox();
    screen.Transition2dOut();
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    epTaskTreasure.IsFinished = true;
  }

  public override void PayOut()
  {
    if (this.mRewardId == 0)
      return;
    Singleton<ExploreDataManager>.GetInstance().ExploreBox.AddReward(this.mRewardId);
  }

  public override long GetReqiredTime() => this.mReqiredTime;

  public override long GetRestReqiredTime() => this.GetReqiredTime() - this.ProcTime;

  public override long GetTakeOverTime() => this.GetReqiredTime() - this.ProcTime;

  public override void OnBackExplore()
  {
  }

  public override long OnBackGroundWork(long calcTime)
  {
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    this.PayOut();
    if (this.mRewardId != 0)
    {
      MasterData.ExploreDropReward.TryGetValue(this.mRewardId, out this.mReward);
      instance.AddLog(string.Format("{0}を[00dc1e]獲得[-]した", (object) this.mReward.reward_title), Color.white);
    }
    long num = calcTime - this.GetReqiredTime();
    this.mReqiredTime -= calcTime;
    return num;
  }

  public override long OnSimulate(ref long simElapsed, ref int simBoxCnt)
  {
    if (this.mRewardId != 0)
      ++simBoxCnt;
    return this.GetReqiredTime();
  }

  public override void OnContinue(long restReqTime, long takeOverTime)
  {
    this.mReqiredTime = restReqTime;
  }
}
