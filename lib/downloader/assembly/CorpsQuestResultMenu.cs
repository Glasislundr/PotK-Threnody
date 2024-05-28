// Decompiled with JetBrains decompiler
// Type: CorpsQuestResultMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class CorpsQuestResultMenu : BattleUI05ResultMenu
{
  private int mCorpsId;
  private int[] mClearMissionHistorys;

  public override IEnumerator Init(BattleInfo info, WebAPI.Response.QuestCorpsBattleFinish result)
  {
    CorpsQuestResultMenu corpsQuestResultMenu = this;
    BattleEnd result1 = new BattleEnd();
    result1.after_player_units = result1.before_player_units = new PlayerUnit[0];
    result1.after_player_gears = result1.before_player_gears = new PlayerItem[0];
    result1.player_character_intimates_in_battle = new BattleEndPlayer_character_intimates_in_battle[0];
    result1.unlock_intimate_skills = new UnlockIntimateSkill[0];
    result1.trust_upper_limit = new BattleEndTrust_upper_limit[0];
    result1.before_player = Player.Current;
    result1.player_mission_results = ((IEnumerable<int>) result.clear_mission_ids).Select<int, PlayerMissionHistory>((Func<int, PlayerMissionHistory>) (x => new PlayerMissionHistory()
    {
      mission_id = x
    })).ToArray<PlayerMissionHistory>();
    result1.player_incr_money = result.player.money - result.before_player.money;
    PlayerCorps playerCorps = ((IEnumerable<PlayerCorps>) result.player_corps_list).First<PlayerCorps>();
    corpsQuestResultMenu.mCorpsId = playerCorps.corps_id;
    corpsQuestResultMenu.mClearMissionHistorys = playerCorps.cleared_mission_ids;
    corpsQuestResultMenu.BonusFlg = new List<bool>()
    {
      false,
      false,
      false,
      false
    };
    IEnumerator e = corpsQuestResultMenu.Init(info, result1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override IEnumerator StageMissionPopup()
  {
    CorpsQuestResultMenu corpsQuestResultMenu = this;
    if (corpsQuestResultMenu.isSkip)
    {
      corpsQuestResultMenu.skipPopupList.Add(corpsQuestResultMenu.StageMissionPopup());
    }
    else
    {
      CorpsMissionReward[] missions = ((IEnumerable<CorpsMissionReward>) MasterData.CorpsMissionRewardList).Where<CorpsMissionReward>((Func<CorpsMissionReward, bool>) (x => x.setting_id == this.mCorpsId)).OrderBy<CorpsMissionReward, int>((Func<CorpsMissionReward, int>) (x => x.priority)).ToArray<CorpsMissionReward>();
      int length = missions.Length;
      if (length >= 1)
      {
        GameObject popup = corpsQuestResultMenu.OpenPopup(corpsQuestResultMenu.mStageMissionPrefab);
        ((IEnumerable<UITweener>) popup.GetComponents<UITweener>()).ForEach<UITweener>((Action<UITweener>) (x =>
        {
          ((Behaviour) x).enabled = false;
          x.onFinished.Clear();
        }));
        Quest0022MissionDescriptions o = popup.GetComponent<Quest0022MissionDescriptions>();
        Quest0022MissionDescription oo = o.description;
        int num = oo.UpdateScrollViewHeight(length);
        for (int index = 0; index < length; ++index)
        {
          Quest0022MissionList component = corpsQuestResultMenu.mStageMissionItemPrefab.Clone(((Component) oo.grid).transform).GetComponent<Quest0022MissionList>();
          ((Component) component).transform.localPosition = new Vector3(0.0f, (float) -index * oo.grid.cellHeight, 0.0f);
          oo.MissionList.Add(component);
          oo.ThumbnailList.Add(((Component) component.LinkParent).gameObject);
        }
        oo.grid.Reposition();
        oo.scrollView.ResetPosition();
        IEnumerator e = oo.LoadAnimation(Mathf.FloorToInt((float) num / oo.grid.cellHeight));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        int[] clearedMissionIds = ((IEnumerable<PlayerMissionHistory>) corpsQuestResultMenu.result.player_mission_results).Select<PlayerMissionHistory, int>((Func<PlayerMissionHistory, int>) (x => x.mission_id)).ToArray<int>();
        for (int i = 0; i < missions.Length; ++i)
        {
          CorpsMissionReward mission = missions[i];
          bool clearFlag = true;
          int? nullable = ((IEnumerable<int>) corpsQuestResultMenu.mClearMissionHistorys).FirstIndexOrNull<int>((Func<int, bool>) (x => x == mission.ID));
          if (!((IEnumerable<int>) clearedMissionIds).Contains<int>(mission.ID))
            clearFlag = nullable.HasValue;
          if (oo.MissionList.Count > i)
          {
            e = oo.MissionList[i].SetValue(mission.name, clearFlag, mission.entity_type, mission.entity_id, mission.quantity, ((IEnumerable<int>) clearedMissionIds).Contains<int>(mission.ID), new CommonQuestType?(CommonQuestType.Corps));
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
        }
        if (Object.op_Inequality((Object) o, (Object) null))
        {
          oo.MissionList.ForEach((Action<Quest0022MissionList>) (x => x.InitEffects()));
          corpsQuestResultMenu.soundMgr.playSE("SE_1035");
          o.StartTweenClick(true);
          yield return (object) new WaitForSeconds(0.5f);
          bool completedMissionExists = false;
          if (oo.MissionList.Exists((Predicate<Quest0022MissionList>) (x => x.IsClear)))
          {
            ((Behaviour) oo.scrollView).enabled = false;
            completedMissionExists = true;
            oo.PlayMissionCompleteTitleAnimation();
          }
          yield return (object) new WaitForSeconds(1f);
          for (int index = 0; index < oo.MissionList.Count; ++index)
          {
            Quest0022MissionList mission = oo.MissionList[index];
            if (mission.IsClear)
              mission.ResultNowGet();
          }
          if (completedMissionExists)
            corpsQuestResultMenu.soundMgr.playSE("SE_1036");
          yield return (object) new WaitForSeconds(0.5f);
          bool isFinished = false;
          Action action = (Action) (() =>
          {
            oo.PlayMissionCompleteTitleAnimation("Anim_Out");
            o.StartTweenClick(false, (EventDelegate.Callback) (() => isFinished = true));
          });
          corpsQuestResultMenu.CreateTouchObject(new EventDelegate.Callback(action.Invoke), popup.transform.parent);
          EventDelegate.Add(oo.dragScrollViewButton.onClick, new EventDelegate.Callback(action.Invoke));
          yield return (object) new WaitForSeconds(0.5f);
          ((Behaviour) oo.scrollView).enabled = true;
          while (!isFinished)
            yield return (object) null;
          Singleton<PopupManager>.GetInstance().dismiss();
        }
      }
    }
  }
}
