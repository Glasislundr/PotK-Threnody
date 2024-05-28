// Decompiled with JetBrains decompiler
// Type: BattleUI05PunitiveExpeditionResultMenu
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
public class BattleUI05PunitiveExpeditionResultMenu : ResultMenuBase
{
  [SerializeField]
  private GameObject HuntingEvent;
  private List<ResultMenuBase> sequences;
  private GameObject PunitiveExpeditionRewardMenuPrefab;
  private GameObject HuntingEventPrefab;
  private bool isInitialized;

  private IEnumerator LoadResources()
  {
    if (Object.op_Equality((Object) this.HuntingEventPrefab, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.battleUI_05.HuntingEvent.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.HuntingEventPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  public override IEnumerator Init(WebAPI.Response.EventTop eventTopInfo)
  {
    if (!this.isInitialized)
    {
      this.isInitialized = true;
      IEnumerator e = this.LoadResources();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.sequences = new List<ResultMenuBase>();
      GameObject popup = this.HuntingEventPrefab.Clone(this.HuntingEvent.transform);
      popup.SetActive(false);
      ResultMenuBase battleUI05PunitiveExpeditionRewardPopupMenuScript = (ResultMenuBase) popup.GetComponent<BattleUI05PunitiveExpeditionRewardPopupMenu>();
      e = battleUI05PunitiveExpeditionRewardPopupMenuScript.Init((BattleInfo) null, (BattleEnd) null, 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.sequences.Add(battleUI05PunitiveExpeditionRewardPopupMenuScript);
      ResultMenuBase battleUI05PunitiveExpeditionRewardMenuScript = (ResultMenuBase) popup.GetComponent<BattleUI05PunitiveExpeditionRewardMenu>();
      e = battleUI05PunitiveExpeditionRewardMenuScript.Init(eventTopInfo);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.sequences.Add(battleUI05PunitiveExpeditionRewardMenuScript);
      List<PunitiveExpeditionEventReward> list = MasterData.PunitiveExpeditionEventReward.Where<KeyValuePair<int, PunitiveExpeditionEventReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, bool>) (x =>
      {
        if (x.Value.period != eventTopInfo.period_id)
          return false;
        if (x.Value.point_type == EventPointType.personal && x.Value.point > eventTopInfo.player_point || x.Value.point_type == EventPointType.all && x.Value.point > eventTopInfo.all_player_point)
          return true;
        return x.Value.point_type == EventPointType.guild && x.Value.point > eventTopInfo.guild_point;
      })).Select<KeyValuePair<int, PunitiveExpeditionEventReward>, PunitiveExpeditionEventReward>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, PunitiveExpeditionEventReward>) (x => x.Value)).ToList<PunitiveExpeditionEventReward>();
      foreach (PunitiveExpeditionEventGuildReward guildReward in MasterData.PunitiveExpeditionEventGuildReward.Where<KeyValuePair<int, PunitiveExpeditionEventGuildReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, bool>) (x => x.Value.period == eventTopInfo.period_id && x.Value.point > eventTopInfo.guild_point)).Select<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, PunitiveExpeditionEventGuildReward>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, PunitiveExpeditionEventGuildReward>) (x => x.Value)).ToArray<PunitiveExpeditionEventGuildReward>())
      {
        PunitiveExpeditionEventReward expeditionEventReward = new PunitiveExpeditionEventReward();
        expeditionEventReward.ConvertGuildReward(guildReward);
        list.Add(expeditionEventReward);
      }
      if (list != null && list.Count > 0)
      {
        ResultMenuBase battleUI05PunitiveExpeditionNextRewardMenuScript = (ResultMenuBase) popup.GetComponent<BattleUI05PunitiveExpeditionNextRewardMenu>();
        e = battleUI05PunitiveExpeditionNextRewardMenuScript.Init(eventTopInfo);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.sequences.Add(battleUI05PunitiveExpeditionNextRewardMenuScript);
        battleUI05PunitiveExpeditionNextRewardMenuScript = (ResultMenuBase) null;
      }
    }
  }

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    if (!this.isInitialized)
    {
      this.isInitialized = true;
      IEnumerator e = this.LoadResources();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.sequences = new List<ResultMenuBase>();
      int len = result.events == null ? 0 : result.events.Length;
      if (len > 0)
      {
        int sequencesIdx = 0;
        for (int i = 0; i < len; ++i)
        {
          if (result.events[i].is_quest_target && result.events[i].current_sum_point > 0)
          {
            GameObject popup = this.HuntingEventPrefab.Clone(this.HuntingEvent.transform);
            BattleUI05PunitiveExpeditionMenu script = popup.GetComponent<BattleUI05PunitiveExpeditionMenu>();
            popup.SetActive(false);
            e = script.Init(info, result, i);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            this.sequences.Add((ResultMenuBase) script);
            e = this.sequences[sequencesIdx].Init(info, result, i);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            ++sequencesIdx;
            if (result.events[i].get_reward_ids != null && result.events[i].get_reward_ids.Length != 0 || result.events[i].get_guild_rward_ids != null && result.events[i].get_guild_rward_ids.Length != 0)
            {
              ResultMenuBase battleUI05PunitiveExpeditionRewardPopupMenuScript = (ResultMenuBase) popup.GetComponent<BattleUI05PunitiveExpeditionRewardPopupMenu>();
              e = battleUI05PunitiveExpeditionRewardPopupMenuScript.Init(info, result, i);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              this.sequences.Add(battleUI05PunitiveExpeditionRewardPopupMenuScript);
              ResultMenuBase battleUI05PunitiveExpeditionRewardMenuScript = (ResultMenuBase) popup.GetComponent<BattleUI05PunitiveExpeditionRewardMenu>();
              e = battleUI05PunitiveExpeditionRewardMenuScript.Init(info, result, i);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              this.sequences.Add(battleUI05PunitiveExpeditionRewardMenuScript);
              List<PunitiveExpeditionEventReward> list = MasterData.PunitiveExpeditionEventReward.Where<KeyValuePair<int, PunitiveExpeditionEventReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, bool>) (x =>
              {
                if (x.Value.period != result.events[i].period_id)
                  return false;
                if (x.Value.point_type == EventPointType.personal && x.Value.point > result.events[i].player_point || x.Value.point_type == EventPointType.all && x.Value.point > result.events[i].all_player_point)
                  return true;
                return x.Value.point_type == EventPointType.guild && x.Value.point > result.events[i].guild_point;
              })).Select<KeyValuePair<int, PunitiveExpeditionEventReward>, PunitiveExpeditionEventReward>((Func<KeyValuePair<int, PunitiveExpeditionEventReward>, PunitiveExpeditionEventReward>) (x => x.Value)).ToList<PunitiveExpeditionEventReward>();
              foreach (PunitiveExpeditionEventGuildReward guildReward in MasterData.PunitiveExpeditionEventGuildReward.Where<KeyValuePair<int, PunitiveExpeditionEventGuildReward>>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, bool>) (x => x.Value.period == result.events[i].period_id && x.Value.point > result.events[i].guild_point)).Select<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, PunitiveExpeditionEventGuildReward>((Func<KeyValuePair<int, PunitiveExpeditionEventGuildReward>, PunitiveExpeditionEventGuildReward>) (x => x.Value)).ToArray<PunitiveExpeditionEventGuildReward>())
              {
                PunitiveExpeditionEventReward expeditionEventReward = new PunitiveExpeditionEventReward();
                expeditionEventReward.ConvertGuildReward(guildReward);
                list.Add(expeditionEventReward);
              }
              if (list != null && list.Count > 0)
              {
                ResultMenuBase battleUI05PunitiveExpeditionNextRewardMenuScript = (ResultMenuBase) popup.GetComponent<BattleUI05PunitiveExpeditionNextRewardMenu>();
                e = battleUI05PunitiveExpeditionNextRewardMenuScript.Init(info, result, i);
                while (e.MoveNext())
                  yield return e.Current;
                e = (IEnumerator) null;
                this.sequences.Add(battleUI05PunitiveExpeditionNextRewardMenuScript);
                battleUI05PunitiveExpeditionNextRewardMenuScript = (ResultMenuBase) null;
              }
              battleUI05PunitiveExpeditionRewardPopupMenuScript = (ResultMenuBase) null;
              battleUI05PunitiveExpeditionRewardMenuScript = (ResultMenuBase) null;
            }
            popup = (GameObject) null;
            script = (BattleUI05PunitiveExpeditionMenu) null;
          }
        }
      }
    }
  }

  public override IEnumerator Run()
  {
    yield return (object) new WaitForSeconds(0.5f);
    List<ResultMenuBase>.Enumerator seqe = this.sequences.GetEnumerator();
    this.HuntingEvent.SetActive(true);
    while (seqe.MoveNext())
    {
      IEnumerator e = seqe.Current.Run();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.HuntingEvent.SetActive(false);
  }
}
