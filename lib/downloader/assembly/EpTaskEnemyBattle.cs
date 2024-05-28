// Decompiled with JetBrains decompiler
// Type: EpTaskEnemyBattle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class EpTaskEnemyBattle : ExploreTask, IEpBattleTask
{
  private bool mNowDuel = true;
  private long mReqiredTime;
  private ExploreEnemy mEnemy;
  private DuelResult mDuelResult;
  private DuelEnvironment mDuelEnvironment;
  private int mDropRewardId;

  private EpTaskEnemyBattle()
  {
  }

  public static EpTaskEnemyBattle Create()
  {
    EpTaskEnemyBattle epTaskEnemyBattle = new EpTaskEnemyBattle();
    ExploreDataManager instance1 = Singleton<ExploreDataManager>.GetInstance();
    ExploreLotteryCore instance2 = Singleton<ExploreLotteryCore>.GetInstance();
    epTaskEnemyBattle.mEnemy = instance2.LotteryNextEnemy();
    epTaskEnemyBattle.mDuelResult = instance1.CreateDuelResult(epTaskEnemyBattle.mEnemy);
    long num1 = instance1.TimeConfig["BATTLE_BASE"];
    long num2 = instance1.TimeConfig["BATTLE_TURN"];
    epTaskEnemyBattle.mReqiredTime = num1 + (long) epTaskEnemyBattle.mDuelResult.turns.Length * num2;
    if (epTaskEnemyBattle.IsWin())
      epTaskEnemyBattle.mDropRewardId = instance2.LotteryEnemyDropRewardId(epTaskEnemyBattle.mEnemy);
    epTaskEnemyBattle.SetRandState();
    return epTaskEnemyBattle;
  }

  public override Explore.STATE State() => Explore.STATE.BATTLE;

  public override IEnumerator LoadAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EpTaskEnemyBattle epTaskEnemyBattle = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    epTaskEnemyBattle.mDuelEnvironment = Singleton<ExploreDataManager>.GetInstance().CreateDuelEnviroment();
    Singleton<ExploreDataManager>.GetInstance().LazyInitDuel(ref epTaskEnemyBattle.mDuelResult);
    epTaskEnemyBattle.IsLoaded = true;
    return false;
  }

  public override IEnumerator UpdateAsync()
  {
    EpTaskEnemyBattle epTaskEnemyBattle = this;
    epTaskEnemyBattle.SetStartTime();
    if (!Singleton<ExploreSceneManager>.GetInstance().IsBackScreen && !Singleton<CommonRoot>.GetInstance().isLoading)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    ExploreDataManager data = Singleton<ExploreDataManager>.GetInstance();
    ExploreModelController model = Singleton<ExploreSceneManager>.GetInstance().Model;
    ExploreScreenEffectController screen = Singleton<ExploreSceneManager>.GetInstance().ScreenEffect;
    string enemyName = epTaskEnemyBattle.mEnemy.unit.name;
    data.AddLog(string.Format("{0}に遭遇した", (object) enemyName), Color.white);
    model.OpenExclamationMark();
    yield return (object) new WaitForSeconds(0.75f);
    model.CloseExclamationMark();
    if (!Singleton<ExploreSceneManager>.GetInstance().IsBackScreen && !Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().PlayExitAnime();
      Singleton<ExploreSceneManager>.GetInstance().TopMenu.IsPush = true;
      screen.TransitionFullIn();
      yield return (object) screen.WaitForTransitionFull();
      model.SetBattleMapMode(true);
      data.CurrentWeakPoint = epTaskEnemyBattle.mEnemy.WeakPoint;
      if (Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount <= 0)
      {
        Explore033BattleScene.changeScene(true, epTaskEnemyBattle.mDuelResult, epTaskEnemyBattle.mDuelEnvironment);
        while (epTaskEnemyBattle.mNowDuel)
          yield return (object) null;
      }
      model.SetBattleMapMode(false);
    }
    else
    {
      screen.TransitionFullIn();
      yield return (object) screen.WaitForTransitionFull();
      for (int index = 0; index < epTaskEnemyBattle.mDuelResult.turns.Length; ++index)
      {
        if (index % 2 == 0)
          data.AddLog(string.Format("{0}に[fa0000]{1}[-]ダメージを与えた", (object) enemyName, (object) epTaskEnemyBattle.mDuelResult.turns[index].dispDamage), Color.white);
        else
          data.AddLog(string.Format("{0}から[fa0000]{1}[-]ダメージを受けた", (object) enemyName, (object) epTaskEnemyBattle.mDuelResult.turns[index].dispDamage), Color.white);
      }
    }
    // ISSUE: explicit non-virtual call
    if (__nonvirtual (epTaskEnemyBattle.IsWin()))
    {
      screen.TransitionFullOut();
      data.AddLog(string.Format("{0}に勝利した!", (object) enemyName), Color.white);
      data.AddLog(string.Format("{0}ユニット経験値を[00dc1e]獲得[-]した", (object) epTaskEnemyBattle.mEnemy.unit_exp), Color.white);
      data.AddLog(string.Format("{0}経験値を[00dc1e]獲得[-]した", (object) epTaskEnemyBattle.mEnemy.player_exp), Color.white);
      data.AddLog(string.Format("{0}ゼニーを[00dc1e]獲得[-]した", (object) epTaskEnemyBattle.mEnemy.zeny), Color.white);
      ExploreDropReward exploreDropReward;
      if (MasterData.ExploreDropReward.TryGetValue(epTaskEnemyBattle.mDropRewardId, out exploreDropReward))
        data.AddLog(string.Format("{0}を[00dc1e]獲得[-]した", (object) exploreDropReward.reward_title), Color.white);
    }
    else
      data.AddLog(string.Format("{0}に敗北した", (object) enemyName), Color32.op_Implicit(new Color32((byte) 250, (byte) 0, (byte) 0, byte.MaxValue)));
    epTaskEnemyBattle.IsFinished = true;
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
