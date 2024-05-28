// Decompiled with JetBrains decompiler
// Type: BattleUI05RankingMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleUI05RankingMenu : ResultMenuBase
{
  [SerializeField]
  private GameObject dir_RankingEvent;
  [SerializeField]
  private GameObject dir_StageTitle;
  [SerializeField]
  private UILabel txt_StageName;
  [SerializeField]
  private UILabel txt_GetPoint;
  [SerializeField]
  private UILabel txt_StageHishScore;
  [SerializeField]
  private UILabel txt_TotalPt;
  [SerializeField]
  private GameObject obj_NowHighScore;
  [SerializeField]
  private GameObject obj_EventTotalPt;
  [SerializeField]
  private GameObject baseObj;
  [SerializeField]
  private GameObject nextBtnObj;
  [SerializeField]
  private NGxScroll2 scroll;
  [SerializeField]
  private UIGrid grid;
  private GameObject breakDownPrefab;
  private GameObject highScorePrefab;
  private bool isEndNowScoreUpdate;
  private QuestScoreBattleFinishContext campaign;
  private bool toNext;
  private bool isQuestAutoLap;

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.campaign = result.score_campaigns[0];
    this.InitStagePoint(this.campaign.score_acquisitions, this.campaign.bonus_rate, ((IEnumerable<QuestScoreBonusTimetable>) SMManager.Get<QuestScoreBonusTimetable[]>()).Any<QuestScoreBonusTimetable>((Func<QuestScoreBonusTimetable, bool>) (x => info.quest_type == CommonQuestType.Extra && x.quest_s_id == info.quest_s_id)));
    if (info.extraQuest != null)
      this.txt_StageName.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_TOTAL_TITLE, (IDictionary) new Hashtable()
      {
        {
          (object) "name",
          (object) info.extraQuest.quest_extra_s.quest_m.name
        }
      }));
    this.txt_GetPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) this.campaign.battle_score
      }
    }));
    this.txt_StageHishScore.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) this.campaign.battle_score_max
      }
    }));
    if (Object.op_Inequality((Object) this.txt_TotalPt, (Object) null))
    {
      if (this.campaign.total_reward_exists)
        this.txt_TotalPt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
        {
          {
            (object) "point",
            (object) this.campaign.score_total
          }
        }));
      else
        this.txt_TotalPt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
        {
          {
            (object) "point",
            (object) "-"
          }
        }));
    }
  }

  private void InitStagePoint(
    QuestScoreAcquisition[] scores,
    string specialRate,
    bool activeSpecialRate)
  {
    for (int index = 0; index < scores.Length; ++index)
      this.breakDownPrefab.Clone(((Component) this.grid).transform).GetComponent<BattleUI05BreakDown>().SetPoint(scores[index].description, scores[index].score);
    if (!activeSpecialRate)
      return;
    this.breakDownPrefab.Clone(((Component) this.grid).transform).GetComponent<BattleUI05BreakDown>().SetSpecialRate(specialRate);
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> breakDownPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.breakDownPrefab, (Object) null))
    {
      breakDownPrefabF = Res.Prefabs.battle.dir_PtBreakdown.Load<GameObject>();
      e = breakDownPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.breakDownPrefab = breakDownPrefabF.Result;
      breakDownPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.highScorePrefab, (Object) null))
    {
      breakDownPrefabF = Res.Prefabs.battle.dir_RankingEvent_HighScore.Load<GameObject>();
      e = breakDownPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.highScorePrefab = breakDownPrefabF.Result;
      breakDownPrefabF = (Future<GameObject>) null;
    }
  }

  public override IEnumerator Run()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.isEndNowScoreUpdate = false;
    this.PlayAnimation();
    while (!this.isEndNowScoreUpdate)
      yield return (object) null;
    this.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
    IEnumerator e = this.WaitForTapping();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void PlayAnimation()
  {
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1011");
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1012", delay: 0.5f);
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1012", delay: 0.7f);
    this.DispNowScore();
    this.DispTotalScore();
  }

  public void DispNowScore()
  {
    this.dir_RankingEvent.SetActive(true);
    this.baseObj.SetActive(true);
    this.nextBtnObj.SetActive(true);
    this.dir_StageTitle.SetActive(true);
    this.grid.Reposition();
    this.scroll.ResolvePosition();
  }

  private void DispTotalScore()
  {
    if (!Object.op_Inequality((Object) this.obj_EventTotalPt, (Object) null))
      return;
    this.obj_EventTotalPt.gameObject.SetActive(true);
  }

  public void DispNowScoreUpdate()
  {
    if (this.campaign.battle_score_max_updated)
      this.CreateHighScore(this.obj_NowHighScore.transform, new Action(this.setEndDispNowScoreUpdate));
    else
      this.setEndDispNowScoreUpdate();
  }

  private void setEndDispNowScoreUpdate() => this.isEndNowScoreUpdate = true;

  private void CreateHighScore(Transform parent, Action callback = null)
  {
    UITweener componentInChildren = this.highScorePrefab.Clone(parent).GetComponentInChildren<UITweener>();
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1024", delay: 0.2f);
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
    {
      Action action = callback;
      if (action == null)
        return;
      action();
    }
    else
      componentInChildren.SetOnFinished(new EventDelegate((EventDelegate.Callback) (() =>
      {
        Action action = callback;
        if (action == null)
          return;
        action();
      })));
  }

  public void onTapToNext() => this.toNext = true;

  private IEnumerator WaitForTapping()
  {
    while (!this.toNext)
    {
      if (this.isQuestAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        this.onTapToNext();
      }
      yield return (object) null;
    }
  }
}
