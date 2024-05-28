// Decompiled with JetBrains decompiler
// Type: EpTaskEnemyRevenge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class EpTaskEnemyRevenge : ExploreTask, IEpBattleTask
{
  private bool mNowDuel = true;
  private long mReqiredTime;
  private ExploreEnemy mEnemy;
  private DuelResult mDuelResult;
  private DuelEnvironment mDuelEnvironment;
  private int mDropRewardId;

  private EpTaskEnemyRevenge()
  {
  }

  public static EpTaskEnemyRevenge Create()
  {
    EpTaskEnemyRevenge taskEnemyRevenge = new EpTaskEnemyRevenge();
    ExploreDataManager instance1 = Singleton<ExploreDataManager>.GetInstance();
    ExploreLotteryCore instance2 = Singleton<ExploreLotteryCore>.GetInstance();
    taskEnemyRevenge.mEnemy = instance1.GetLastDuelEnemy();
    taskEnemyRevenge.mDuelResult = instance1.CreateRevengeDuelResult();
    long num1 = instance1.TimeConfig["BATTLE_BASE"];
    long num2 = instance1.TimeConfig["BATTLE_TURN"];
    taskEnemyRevenge.mReqiredTime = num1 + (long) taskEnemyRevenge.mDuelResult.turns.Length * num2;
    if (taskEnemyRevenge.IsWin())
      taskEnemyRevenge.mDropRewardId = instance2.LotteryEnemyDropRewardId(taskEnemyRevenge.mEnemy);
    taskEnemyRevenge.SetRandState();
    return taskEnemyRevenge;
  }

  public override Explore.STATE State() => Explore.STATE.BATTLE;

  public override IEnumerator LoadAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EpTaskEnemyRevenge taskEnemyRevenge = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    taskEnemyRevenge.mDuelEnvironment = Singleton<ExploreDataManager>.GetInstance().CreateDuelEnviroment();
    Singleton<ExploreDataManager>.GetInstance().LazyInitDuel(ref taskEnemyRevenge.mDuelResult);
    taskEnemyRevenge.IsLoaded = true;
    return false;
  }

  public override IEnumerator UpdateAsync()
  {
    EpTaskEnemyRevenge taskEnemyRevenge = this;
    taskEnemyRevenge.SetStartTime();
    ExploreDataManager data = Singleton<ExploreDataManager>.GetInstance();
    ExploreModelController model = Singleton<ExploreSceneManager>.GetInstance().Model;
    ExploreScreenEffectController screen = Singleton<ExploreSceneManager>.GetInstance().ScreenEffect;
    string enemyName = taskEnemyRevenge.mEnemy.unit.name;
    data.AddLog(string.Format("{0}に遭遇した", (object) enemyName), Color.white);
    if (!Singleton<ExploreSceneManager>.GetInstance().IsBackScreen && !Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().PlayExitAnime();
      Singleton<ExploreSceneManager>.GetInstance().TopMenu.IsPush = true;
      screen.TransitionFullIn();
      yield return (object) screen.WaitForTransitionFull();
      model.SetCampVisible(false);
      model.SetBattleMapMode(true);
      data.CurrentWeakPoint = taskEnemyRevenge.mEnemy.WeakPoint;
      if (Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount <= 0)
      {
        Explore033BattleScene.changeScene(true, taskEnemyRevenge.mDuelResult, taskEnemyRevenge.mDuelEnvironment);
        while (taskEnemyRevenge.mNowDuel)
          yield return (object) null;
      }
      model.SetBattleMapMode(false);
    }
    else
    {
      screen.TransitionFullIn();
      yield return (object) screen.WaitForTransitionFull();
      for (int index = 0; index < taskEnemyRevenge.mDuelResult.turns.Length; ++index)
      {
        if (index % 2 == 0)
          data.AddLog(string.Format("{0}に[fa0000]{1}[-]ダメージを与えた", (object) enemyName, (object) taskEnemyRevenge.mDuelResult.turns[index].dispDamage), Color.white);
        else
          data.AddLog(string.Format("{0}から[fa0000]{1}[-]ダメージを受けた", (object) enemyName, (object) taskEnemyRevenge.mDuelResult.turns[index].dispDamage), Color.white);
      }
      model.SetCampVisible(false);
    }
    // ISSUE: explicit non-virtual call
    if (__nonvirtual (taskEnemyRevenge.IsWin()))
    {
      screen.TransitionFullOut();
      data.AddLog(string.Format("{0}に勝利した!", (object) enemyName), Color.white);
      data.AddLog(string.Format("{0}ユニット経験値を[00dc1e]獲得[-]した", (object) taskEnemyRevenge.mEnemy.unit_exp), Color.white);
      data.AddLog(string.Format("{0}経験値を[00dc1e]獲得[-]した", (object) taskEnemyRevenge.mEnemy.player_exp), Color.white);
      data.AddLog(string.Format("{0}ゼニーを[00dc1e]獲得[-]した", (object) taskEnemyRevenge.mEnemy.zeny), Color.white);
      ExploreDropReward exploreDropReward;
      if (MasterData.ExploreDropReward.TryGetValue(taskEnemyRevenge.mDropRewardId, out exploreDropReward))
        data.AddLog(string.Format("{0}を[00dc1e]獲得[-]した", (object) exploreDropReward.reward_title), Color.white);
    }
    else
      data.AddLog(string.Format("{0}に敗北した", (object) enemyName), Color32.op_Implicit(new Color32((byte) 250, (byte) 0, (byte) 0, byte.MaxValue)));
    taskEnemyRevenge.IsFinished = true;
  }

  public override void PayOut()
  {
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    if (this.IsWin())
    {
      instance.AddWinCount(this.mEnemy.ID);
      instance.ExploreBox.Add(this.mEnemy.zeny, this.mEnemy.trust_rate, this.mEnemy.player_exp, this.mEnemy.unit_exp);
      if (this.mDropRewardId == 0)
        return;
      instance.ExploreBox.AddReward(this.mDropRewardId);
    }
    else
      instance.AddLoseCount();
  }

  public override long GetReqiredTime() => this.mReqiredTime;

  public override long GetRestReqiredTime() => this.GetReqiredTime() - this.ProcTime;

  public override long GetTakeOverTime() => this.GetReqiredTime() - this.ProcTime;

  public override void OnBackExplore() => this.mNowDuel = false;

  public bool IsWin() => this.mDuelResult.isDieDefense;

  public void SetBattleCache()
  {
    Singleton<ExploreDataManager>.GetInstance().SetLastDuelCache(this.mEnemy, this.mDuelResult);
  }

  public override long OnBackGroundWork(long calcTime)
  {
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    this.SetBattleCache();
    this.PayOut();
    string name = this.mEnemy.unit.name;
    instance.AddLog(string.Format("{0}に遭遇した", (object) name), Color.white);
    if (this.IsWin())
    {
      instance.AddLog(string.Format("{0}に勝利した!", (object) name), Color.white);
      instance.AddLog(string.Format("{0}ユニット経験値を[00dc1e]獲得[-]した", (object) this.mEnemy.unit_exp), Color.white);
      instance.AddLog(string.Format("{0}経験値を[00dc1e]獲得[-]した", (object) this.mEnemy.player_exp), Color.white);
      instance.AddLog(string.Format("{0}ゼニーを[00dc1e]獲得[-]した", (object) this.mEnemy.zeny), Color.white);
      ExploreDropReward exploreDropReward;
      if (this.mDropRewardId != 0 && MasterData.ExploreDropReward.TryGetValue(this.mDropRewardId, out exploreDropReward))
        instance.AddLog(string.Format("{0}を[00dc1e]獲得[-]した", (object) exploreDropReward.reward_title), Color.white);
    }
    else
      instance.AddLog(string.Format("{0}に敗北した", (object) name), Color32.op_Implicit(new Color32((byte) 250, (byte) 0, (byte) 0, byte.MaxValue)));
    long num = calcTime - this.GetReqiredTime();
    this.mReqiredTime -= calcTime;
    return num;
  }

  public override long OnSimulate(ref long simElapsed, ref int simBoxCnt)
  {
    if (this.mDropRewardId != 0)
      ++simBoxCnt;
    return this.GetReqiredTime();
  }

  public override void OnContinue(long restReqTime, long takeOverTime)
  {
    this.mReqiredTime = restReqTime;
  }
}
