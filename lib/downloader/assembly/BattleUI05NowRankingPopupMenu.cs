// Decompiled with JetBrains decompiler
// Type: BattleUI05NowRankingPopupMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class BattleUI05NowRankingPopupMenu : ResultMenuBase
{
  [SerializeField]
  private GameObject dir_RankingEvent;
  [SerializeField]
  private GameObject dir_StageTitle;
  private QuestScoreBattleFinishContext campaign;
  private GameObject nowRankPopupPrefab;

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    this.campaign = result.score_campaigns[0];
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.nowRankPopupPrefab, (Object) null))
    {
      Future<GameObject> nowRankPopupPrefabF = Res.Prefabs.battle.dir_RankingEvent_BattleResult.Load<GameObject>();
      IEnumerator e = nowRankPopupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.nowRankPopupPrefab = nowRankPopupPrefabF.Result;
      nowRankPopupPrefabF = (Future<GameObject>) null;
    }
  }

  public override IEnumerator Run()
  {
    bool toNext = false;
    GameObject obj = Singleton<PopupManager>.GetInstance().open(this.nowRankPopupPrefab);
    BattleUI05NowRankingPopup script = obj.GetComponent<BattleUI05NowRankingPopup>();
    obj.SetActive(false);
    script.Init(this.campaign.score_total, this.campaign.rank, this.campaign.rank_before);
    obj.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
    script.DispRankUp(this.campaign.rank_before > this.campaign.rank);
    obj.SetActive(true);
    script.SetCloseCallBack((Action) (() =>
    {
      toNext = true;
      Singleton<PopupManager>.GetInstance().onDismiss();
    }));
    while (!toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        script.waitFinish();
        script.OnPress();
      }
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(0.5f);
  }

  public override IEnumerator OnFinish()
  {
    this.dir_RankingEvent.SetActive(false);
    this.dir_StageTitle.SetActive(false);
    yield break;
  }
}
