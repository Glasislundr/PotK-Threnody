// Decompiled with JetBrains decompiler
// Type: BattleUI05PunitiveExpeditionMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class BattleUI05PunitiveExpeditionMenu : ResultMenuBase
{
  [SerializeField]
  private GameObject baseObject;
  [SerializeField]
  private UILabel txtHuntingPointTitle;
  [SerializeField]
  private GameObject dirHuntingPointTitle;
  [SerializeField]
  private GameObject dirHuntingTotalPointTitle;
  [SerializeField]
  private NGxScroll scrollContiner;
  [SerializeField]
  private GameObject dirStageTotalHuntingPt;
  [SerializeField]
  private UILabel txtStageTotalHuntingPt;
  [SerializeField]
  private GameObject dirBase;
  [SerializeField]
  private GameObject dirPlayerTotalHuntingPt;
  [SerializeField]
  private UILabel txtPlayerTotalHuntingPt;
  [SerializeField]
  private GameObject dirTotalHunting;
  [SerializeField]
  private UILabel txtTotalHuntingForAllPt;
  [SerializeField]
  private GameObject dirGuildHunting;
  [SerializeField]
  private UILabel txtGuildHuntingForAllPt;
  [SerializeField]
  private GameObject dirContribution;
  [SerializeField]
  private UILabel txtContribution;
  private GameObject DirHuntingTargetPT;
  private BattleUI05PunitiveExpeditionMenu.AnimState state;
  private bool animatiFinish;
  private Action tapCallback;
  private bool toNext;
  private bool isGuild;

  public void onTapToNext() => this.toNext = true;

  private IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.DirHuntingTargetPT, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.battle.dir_hunting_target_pt.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.DirHuntingTargetPT = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  private void InitTargetPoint(EventBattleFinish eventInfo, BattleInfo info)
  {
    EventBattleFinishDestroy_enemys[] destroyEnemys = eventInfo.destroy_enemys;
    int bonusScore = 0;
    this.scrollContiner.Clear();
    this.scrollContiner.Reset();
    for (int index = 0; index < destroyEnemys.Length; ++index)
    {
      GameObject gameObject = this.DirHuntingTargetPT.Clone();
      gameObject.GetComponent<BattleUI05PunitiveExpeditionTargetScroll>().Init(destroyEnemys[index]);
      this.scrollContiner.Add(gameObject);
      bonusScore += destroyEnemys[index].bonus_point;
    }
    if (bonusScore > 0)
    {
      GameObject gameObject = this.DirHuntingTargetPT.Clone();
      gameObject.GetComponent<BattleUI05PunitiveExpeditionTargetScroll>().Init(bonusScore);
      this.scrollContiner.Add(gameObject);
    }
    if (((IEnumerable<UnitBonus>) UnitBonus.getActiveUnitBonus(ServerTime.NowAppTime(), new int?((int) info.quest_type), new int?(info.quest_s_id))).Count<UnitBonus>() > 0)
    {
      GameObject gameObject = this.DirHuntingTargetPT.Clone();
      gameObject.GetComponent<BattleUI05PunitiveExpeditionTargetScroll>().Init(eventInfo.bonus_rate);
      this.scrollContiner.Add(gameObject);
    }
    this.scrollContiner.ResolvePosition();
  }

  public override IEnumerator Init(BattleInfo info, BattleEnd result, int index)
  {
    IEnumerator e = this.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.isGuild = result.events[index].IsGuild();
    this.toNext = false;
    this.txtHuntingPointTitle.SetTextLocalize(result.events[index].period_name);
    this.txtStageTotalHuntingPt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) result.events[index].current_sum_point
      }
    }));
    this.txtPlayerTotalHuntingPt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
    {
      {
        (object) "point",
        (object) result.events[index].player_point
      }
    }));
    if (this.isGuild)
    {
      this.dirGuildHunting.SetActive(true);
      this.txtGuildHuntingForAllPt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) result.events[index].guild_point
        }
      }));
      this.dirContribution.SetActive(true);
      this.txtContribution.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_CONTRIBUTION_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) result.events[index].contribution
        }
      }));
    }
    else
    {
      this.dirTotalHunting.SetActive(true);
      this.txtTotalHuntingForAllPt.SetTextLocalize(Consts.Format(Consts.GetInstance().RESULT_RANKING_MENU_POINT, (IDictionary) new Hashtable()
      {
        {
          (object) "point",
          (object) result.events[index].all_player_point
        }
      }));
      this.dirContribution.SetActive(false);
    }
    this.InitTargetPoint(result.events[index], info);
  }

  public void ChangeState()
  {
    if (this.state == BattleUI05PunitiveExpeditionMenu.AnimState.End)
      return;
    if (this.state == BattleUI05PunitiveExpeditionMenu.AnimState.NowScore)
    {
      this.state = BattleUI05PunitiveExpeditionMenu.AnimState.TotalScore;
      Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1011");
      Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1012", delay: 0.4f);
    }
    else if (this.state == BattleUI05PunitiveExpeditionMenu.AnimState.TotalScore)
      this.state = BattleUI05PunitiveExpeditionMenu.AnimState.End;
    this.PlayAnimation();
  }

  private void DispNowScore()
  {
    this.dirBase.SetActive(true);
    this.dirHuntingPointTitle.SetActive(true);
  }

  private void DispTotalScore()
  {
    this.dirHuntingTotalPointTitle.SetActive(true);
    this.dirPlayerTotalHuntingPt.SetActive(true);
  }

  private void PlayAnimation()
  {
    if (this.state == BattleUI05PunitiveExpeditionMenu.AnimState.NowScoreInit || this.state == BattleUI05PunitiveExpeditionMenu.AnimState.NowScore)
    {
      if (this.state == BattleUI05PunitiveExpeditionMenu.AnimState.NowScoreInit)
      {
        Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1011");
        Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1012", delay: 0.5f);
        Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1012", delay: 0.7f);
        this.state = BattleUI05PunitiveExpeditionMenu.AnimState.NowScore;
      }
      this.DispNowScore();
    }
    else if (this.state == BattleUI05PunitiveExpeditionMenu.AnimState.TotalScore)
    {
      this.DispTotalScore();
    }
    else
    {
      if (this.state != BattleUI05PunitiveExpeditionMenu.AnimState.End)
        return;
      this.animatiFinish = true;
    }
  }

  public override IEnumerator Run()
  {
    BattleUI05PunitiveExpeditionMenu punitiveExpeditionMenu = this;
    yield return (object) new WaitForSeconds(0.5f);
    ((Component) punitiveExpeditionMenu).gameObject.SetActive(true);
    punitiveExpeditionMenu.state = BattleUI05PunitiveExpeditionMenu.AnimState.NowScoreInit;
    punitiveExpeditionMenu.animatiFinish = false;
    punitiveExpeditionMenu.PlayAnimation();
    while (!punitiveExpeditionMenu.animatiFinish)
      yield return (object) null;
    IEnumerator e = punitiveExpeditionMenu.WaitForTapping();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator WaitForTapping()
  {
    while (!this.toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap)
      {
        yield return (object) new WaitForSeconds(3f);
        this.onTapToNext();
      }
      yield return (object) null;
    }
    this.baseObject.SetActive(false);
  }

  private enum AnimState
  {
    NowScoreInit,
    NowScore,
    TotalScore,
    End,
  }
}
