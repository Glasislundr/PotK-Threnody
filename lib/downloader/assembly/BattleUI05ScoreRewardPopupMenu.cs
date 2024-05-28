// Decompiled with JetBrains decompiler
// Type: BattleUI05ScoreRewardPopupMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI05ScoreRewardPopupMenu : ResultMenuBase
{
  private GameObject rewardPopupPrefab;
  private int currentCnt;
  private QuestScoreBattleFinishContext campaign;

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    this.campaign = result.score_campaigns[0];
    this.currentCnt = 0;
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.rewardPopupPrefab, (Object) null))
    {
      Future<GameObject> rewardPopupPrefabF = Res.Prefabs.battle.dir_RankingEvent_QuestPtTotalReward.Load<GameObject>();
      IEnumerator e = rewardPopupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.rewardPopupPrefab = rewardPopupPrefabF.Result;
      rewardPopupPrefabF = (Future<GameObject>) null;
    }
  }

  public override IEnumerator Run()
  {
    BattleUI05ScoreRewardPopupMenu scoreRewardPopupMenu = this;
    BattleUI05ScoreRewardPopup script = Singleton<PopupManager>.GetInstance().open(scoreRewardPopupMenu.rewardPopupPrefab).GetComponent<BattleUI05ScoreRewardPopup>();
    IEnumerator e = script.Init(scoreRewardPopupMenu.campaign.score_achivement_rewards[scoreRewardPopupMenu.currentCnt]);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ++scoreRewardPopupMenu.currentCnt;
    bool toNext = false;
    script.SetTapCallBack((Action) (() =>
    {
      if (this.IsPush)
        return;
      toNext = true;
    }));
    while (!toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        script.onFinish();
        script.onTap();
      }
      yield return (object) null;
    }
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (scoreRewardPopupMenu.currentCnt < scoreRewardPopupMenu.campaign.score_achivement_rewards.Length)
    {
      e = scoreRewardPopupMenu.Run();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public override IEnumerator OnFinish()
  {
    yield break;
  }
}
