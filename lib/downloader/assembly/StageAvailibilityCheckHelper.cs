// Decompiled with JetBrains decompiler
// Type: StageAvailibilityCheckHelper
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
public class StageAvailibilityCheckHelper : MonoBehaviour
{
  public IEnumerator PopupJudge(
    QuestConverterData StageData,
    Action<Action> act,
    Action<PopupUtility.SceneTo> changeOtherScene,
    bool Event,
    bool storyOnly)
  {
    StageAvailibilityCheckHelper availibilityCheckHelper = this;
    Player player = SMManager.Get<Player>();
    IEnumerator e;
    if (player.CheckMaxHavingUnit() || player.CheckLimitOverMaxUnitReserves())
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._999_5_1(changeOtherScene));
    else if (player.CheckMaxHavingGear())
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility._999_6_1(true));
    else if (player.CheckMaxHavingReisou())
      Singleton<PopupManager>.GetInstance().monitorCoroutine(PopupUtility.popupMaxReisou());
    else if (Event)
      availibilityCheckHelper.StartCoroutine(availibilityCheckHelper.QuestTimeCompare(StageData, act, storyOnly));
    else if (player.ap - StageData.lost_ap < 0)
    {
      if (player.ap_max - StageData.lost_ap < 0)
      {
        e = StageAvailibilityCheckHelper.maxAPshortage_Popoup();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = StageAvailibilityCheckHelper.noAP_Popup((Action) (() =>
        {
          act((Action) null);
          if (StageData.type == CommonQuestType.Story)
          {
            PlayerStoryQuestS story_quest = ((IEnumerable<PlayerStoryQuestS>) SMManager.Get<PlayerStoryQuestS[]>()).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.ID == StageData.id_S)).FirstOrDefault<PlayerStoryQuestS>();
            if (story_quest == null || !MasterData.QuestStoryS.ContainsKey(StageData.wave == null ? StageData.id_S : StageData.wave.first_quest_s_id))
              return;
            if (StageData.no_return_scene)
              this.setNoReturnScene(false);
            Quest0028Scene.changeScene(!StageData.no_return_scene, story_quest, storyOnly);
          }
          else
          {
            if (StageData.type != CommonQuestType.Sea)
              return;
            PlayerSeaQuestS sea_quest = ((IEnumerable<PlayerSeaQuestS>) SMManager.Get<PlayerSeaQuestS[]>()).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.quest_sea_s.ID == StageData.id_S)).FirstOrDefault<PlayerSeaQuestS>();
            if (sea_quest == null || !MasterData.QuestSeaS.ContainsKey(StageData.wave == null ? StageData.id_S : StageData.wave.first_quest_s_id))
              return;
            if (StageData.no_return_scene)
              this.setNoReturnScene(true);
            Quest0028Scene.changeScene(!StageData.no_return_scene, sea_quest, storyOnly);
          }
        }));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else
    {
      act((Action) null);
      if (StageData.type == CommonQuestType.Story)
      {
        PlayerStoryQuestS story_quest = ((IEnumerable<PlayerStoryQuestS>) SMManager.Get<PlayerStoryQuestS[]>()).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.ID == StageData.id_S)).FirstOrDefault<PlayerStoryQuestS>();
        if (story_quest != null && MasterData.QuestStoryS.ContainsKey(StageData.wave == null ? StageData.id_S : StageData.wave.first_quest_s_id))
        {
          if (StageData.no_return_scene)
            availibilityCheckHelper.setNoReturnScene(false);
          Quest0028Scene.changeScene(!StageData.no_return_scene, story_quest, storyOnly);
        }
      }
      else if (StageData.type == CommonQuestType.Sea)
      {
        PlayerSeaQuestS sea_quest = ((IEnumerable<PlayerSeaQuestS>) SMManager.Get<PlayerSeaQuestS[]>()).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.quest_sea_s.ID == StageData.id_S)).FirstOrDefault<PlayerSeaQuestS>();
        if (sea_quest != null && MasterData.QuestSeaS.ContainsKey(StageData.wave == null ? StageData.id_S : StageData.wave.first_quest_s_id))
        {
          if (StageData.no_return_scene)
            availibilityCheckHelper.setNoReturnScene(true);
          Quest0028Scene.changeScene(!StageData.no_return_scene, sea_quest, storyOnly);
        }
      }
    }
  }

  private static IEnumerator noAP_Popup(Action questChangeScene)
  {
    IEnumerator e = PopupUtility.RecoveryAP(true, questChangeScene);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private static IEnumerator maxAPshortage_Popoup()
  {
    Future<GameObject> popupF = Res.Prefabs.popup.popup_002_2_ap1__anim_popup01.Load<GameObject>();
    IEnumerator e = popupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popupF.Result);
  }

  private IEnumerator QuestTimeCompare(
    QuestConverterData StageData,
    Action<Action> act,
    bool storyOnly)
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerExtraQuestS quest = Array.Find<PlayerExtraQuestS>(((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).CheckMasterData().ToArray<PlayerExtraQuestS>(), (Predicate<PlayerExtraQuestS>) (x => x._quest_extra_s == StageData.id_S));
    bool flag = false;
    if (quest != null)
    {
      flag = ServerTime.NowAppTime() < quest.today_day_end_at;
      PlayerQuestGate playerQuestGate = quest.GetPlayerQuestGate();
      if (playerQuestGate != null)
      {
        DateTime dateTime = ServerTime.NowAppTime();
        DateTime? endAt = playerQuestGate.end_at;
        flag = endAt.HasValue && dateTime < endAt.GetValueOrDefault();
      }
    }
    Player current = Player.Current;
    if (flag)
    {
      if (StageData.is_skip_sortie)
        Singleton<PopupManager>.GetInstance().monitorCoroutine(StageAvailibilityCheckHelper.doSkipQuestExtraSortie(StageData, act));
      else if (current.ap - StageData.lost_ap < 0)
      {
        if (current.ap_max - StageData.lost_ap < 0)
        {
          e = StageAvailibilityCheckHelper.maxAPshortage_Popoup();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          e = StageAvailibilityCheckHelper.noAP_Popup((Action) (() =>
          {
            if (!MasterData.QuestExtraS.ContainsKey(StageData.wave == null ? StageData.id_S : StageData.wave.first_quest_s_id))
              return;
            act((Action) null);
            if (StageData.no_return_scene)
              this.setNoReturnScene(false);
            Quest0028Scene.changeScene(!StageData.no_return_scene, quest, storyOnly);
          }));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else
      {
        act((Action) null);
        if (StageData.no_return_scene)
          this.setNoReturnScene(false);
        BattleStage stage = MasterData.QuestExtraS[StageData.wave == null ? StageData.id_S : StageData.wave.first_quest_s_id].stage;
        Quest0028Scene.changeScene(!StageData.no_return_scene, quest, storyOnly);
      }
    }
    else
    {
      Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
      e = time_popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = time_popup.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result);
      time_popup = (Future<GameObject>) null;
    }
  }

  private static IEnumerator doSkipQuestExtraSortie(
    QuestConverterData questData,
    Action<Action> act)
  {
    Player current = Player.Current;
    if (current.ap - questData.lost_ap < 0)
    {
      if (current.ap_max - questData.lost_ap < 0)
        yield return (object) StageAvailibilityCheckHelper.maxAPshortage_Popoup();
      else
        yield return (object) StageAvailibilityCheckHelper.noAP_Popup((Action) (() => Singleton<PopupManager>.GetInstance().monitorCoroutine(StageAvailibilityCheckHelper.doSkipQuestExtraSortie(questData, act))));
    }
    else
    {
      Consts instance1 = Consts.GetInstance();
      PlayerUnit playerUnit = Array.Find<PlayerUnit>(SMManager.Get<PlayerUnit[]>(), (Predicate<PlayerUnit>) (x => x.id == questData.player_unit_id));
      PlayerItem equippedGear = playerUnit.equippedGear;
      PlayerItem equippedGear2 = playerUnit.equippedGear2;
      PlayerItem equippedGear3 = playerUnit.equippedGear3;
      if (equippedGear != (PlayerItem) null && equippedGear.broken || equippedGear2 != (PlayerItem) null && equippedGear2.broken || equippedGear3 != (PlayerItem) null && equippedGear3.broken)
      {
        int nWait = 0;
        PopupCommonNoYes.Show(instance1.EXPLORE_POPUP_REPAIR_CONFIRM_TITLE, instance1.EXPLORE_POPUP_REPAIR_CONFIRM_MESSAGE, (Action) (() => nWait = 1), (Action) (() => nWait = 2));
        while (nWait == 0)
          yield return (object) null;
        if (nWait != 2)
          Bugu00524Scene.ChangeScene(true);
      }
      else
      {
        int nWait = 0;
        PopupCommonNoYes.Show(instance1.POPUP_SKIP_SORTIE_TITLE, instance1.POPUP_SKIP_SORTIE_MESSAGE, (Action) (() => nWait = 1), (Action) (() => nWait = 2));
        while (nWait == 0)
          yield return (object) null;
        if (nWait != 2)
        {
          bool bWait = true;
          act((Action) (() => bWait = false));
          while (bWait)
            yield return (object) null;
          BattleInfo battleInfo = (BattleInfo) null;
          yield return (object) BattleInfoUtil.doMakeExtraSoloBattleInfo(playerUnit.id, questData.id_S, (Action<BattleInfo>) (info => battleInfo = info), (Action<WebAPI.Response.UserError>) (error =>
          {
            battleInfo = (BattleInfo) null;
            Singleton<CommonRoot>.GetInstance().isLoading = false;
            WebAPI.DefaultUserErrorCallback(error);
            Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
            Singleton<NGSceneManager>.GetInstance().clearStack();
            Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
          }));
          if (battleInfo != null)
          {
            NGBattleManager instance2 = Singleton<NGBattleManager>.GetInstance();
            instance2.deleteSavedEnvironment();
            instance2.startBattle(battleInfo);
          }
        }
      }
    }
  }

  private void setNoReturnScene(bool bSea)
  {
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
  }
}
