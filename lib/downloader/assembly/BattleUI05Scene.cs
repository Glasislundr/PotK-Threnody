// Decompiled with JetBrains decompiler
// Type: BattleUI05Scene
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
public class BattleUI05Scene : NGSceneBase
{
  [SerializeField]
  private GameObject touchToNext;
  private List<ResultMenuBase> sequences;
  private bool isInitialized;
  private bool isStarted;
  private bool toNextSequence;
  private ResultMenuBase nowPlayBase;
  private static DateTime serverTime;
  private DebugText debugText;
  private bool isQuestAutoLap;
  private GameObject PunitiveExpeditionRewardMenuPrefab;
  private GameObject PunitiveExpeditionNextRewardMenuPrefab;
  private static WebAPI.Response.GuildraidBattleFinish guildRaidBattleFinishResponse;

  public override void onEndScene()
  {
    this.stopMenuSequencer();
    foreach (ResultMenuBase sequence in this.sequences)
    {
      if (Object.op_Inequality((Object) sequence, (Object) null))
        sequence.OnRemove();
    }
    this.sequences.Clear();
  }

  public override List<string> createResourceLoadList()
  {
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    Func<PlayerUnit, IEnumerable<string>> selector = (Func<PlayerUnit, IEnumerable<string>>) (x => (IEnumerable<string>) rm.PathsFromUnit(x.unit));
    return ((IEnumerable<PlayerUnit>) source).SelectMany<PlayerUnit, string>(selector).ToList<string>();
  }

  public void IbtnTouchToNext()
  {
    this.toNextSequence = true;
    if (!Object.op_Inequality((Object) this.nowPlayBase, (Object) null))
      return;
    this.nowPlayBase.isSkip = true;
  }

  public static void ChangeScene(
    BattleInfo info,
    bool isWin,
    WebAPI.Response.BattleStoryFinish result)
  {
    BattleUI05Scene.ChangeScene(info, isWin, result.battle_finish);
  }

  public static void ChangeScene(
    BattleInfo info,
    bool isWin,
    WebAPI.Response.BattleWaveFinish result)
  {
    BattleUI05Scene.ChangeScene(info, isWin, result.battle_finish);
  }

  public static void ChangeScene(
    BattleInfo info,
    bool isWin,
    WebAPI.Response.SeaBattleFinish result)
  {
    BattleUI05Scene.ChangeScene(info, isWin, result.battle_finish);
  }

  public static void ChangeScene(
    BattleInfo info,
    bool isWin,
    bool isGameOver,
    bool isRetire,
    WebAPI.Response.GuildraidBattleFinish result)
  {
    Singleton<NGGameDataManager>.GetInstance().SetTablePieceSameCharacterIds(result.battle_finish.gettable_piece_same_character_ids);
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.clearStack();
    instance.destroyCurrentScene();
    instance.destroyLoadedScenes();
    if (isWin && info.isSea && Persist.seaLastSortie.Data.saveClearedS(info.seaQuest._quest_sea_s))
      Persist.seaLastSortie.Flush();
    if (isRetire)
      Raid032BattleScene.changeScene(false, result.loop_count, result.quest_s_id, info.isSimulation, true);
    else if (result.is_simulation)
      Raid032BattleResultScene.ChangeScene(info, result);
    else if (isGameOver)
    {
      Raid032BattleResultScene.ChangeScene(info, result);
    }
    else
    {
      BattleUI05Scene.guildRaidBattleFinishResponse = result;
      instance.changeScene("battleUI_05", false, (object) info, (object) isWin, (object) result.battle_finish);
    }
  }

  public static void ChangeScene(BattleInfo info, bool isWin, BattleEnd result)
  {
    Singleton<NGGameDataManager>.GetInstance().SetTablePieceSameCharacterIds(result.gettable_piece_same_character_ids);
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.clearStack();
    instance.destroyCurrentScene();
    instance.destroyLoadedScenes();
    if (isWin)
    {
      if (info.isSea && Persist.seaLastSortie.Data.saveClearedS(info.seaQuest._quest_sea_s))
        Persist.seaLastSortie.Flush();
      if (Singleton<NGGameDataManager>.GetInstance().IsSea && info.seaQuest != null)
        instance.changeScene("battleUI_05_sea", false, (object) info, (object) isWin, (object) result);
      else
        instance.changeScene("battleUI_05", false, (object) info, (object) isWin, (object) result);
    }
    else
      BattleUI05Scene.ReturnQuestScene(info, isWin, result);
  }

  public static void TutorialChangeScene(int stageId, int deckId, int questSId, PlayerUnit unit)
  {
    BattleInfo info = Singleton<NGBattleManager>.GetInstance().battleInfo ?? new BattleInfo();
    info.quest_type = CommonQuestType.Story;
    info.battleId = "";
    info.stageId = stageId;
    info.helper = (PlayerHelper) null;
    info.deckIndex = deckId;
    info.isAutoBattleEnable = false;
    info.isRetreatEnable = false;
    info.isStoryEnable = false;
    info.enemy_items = new Tuple<int, GameCore.Reward>[0];
    info.enemy_ids = new int[0];
    info.user_enemy_ids = new int[0];
    info.storyQuest = new PlayerStoryQuestS();
    info.quest_s_id = questSId;
    info.playerCallSkillParam = new BattleInfo.CallSkillParam();
    info.enemyCallSkillParam = new BattleInfo.CallSkillParam();
    WebAPI.Request.BattleFinish battleFinish = new WebAPI.Request.BattleFinish()
    {
      quest_type = info.quest_type,
      win = true,
      is_game_over = false,
      battle_uuid = info.battleId,
      player_money = 0,
      battle_turn = 0,
      continue_count = 0,
      week_element_attack_count = 0,
      week_kind_attack_count = 0
    };
    WebAPI.Response.BattleStoryFinish result = new WebAPI.Response.BattleStoryFinish()
    {
      battle_finish = new BattleEnd()
    };
    result.battle_finish.before_player_units = ((IEnumerable<PlayerUnit>) Singleton<TutorialRoot>.GetInstance().Resume.player_units).ToArray<PlayerUnit>();
    result.battle_finish.after_player_units = ((IEnumerable<PlayerUnit>) Singleton<TutorialRoot>.GetInstance().Resume.player_units).ToArray<PlayerUnit>();
    int? nullable = ((IEnumerable<PlayerUnit>) result.battle_finish.after_player_units).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.ID == unit.unit.ID));
    result.battle_finish.after_player_units[nullable.Value] = unit;
    result.battle_finish.before_player_gears = ((IEnumerable<PlayerItem>) Singleton<TutorialRoot>.GetInstance().Resume.player_items).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear)).ToArray<PlayerItem>();
    result.battle_finish.after_player_gears = ((IEnumerable<PlayerItem>) Singleton<TutorialRoot>.GetInstance().Resume.player_items).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear)).ToArray<PlayerItem>();
    result.battle_finish.battle_helpers = new PlayerHelper[0];
    result.battle_finish.stage_clear_rewards = new BattleEndStage_clear_rewards[0];
    result.battle_finish.boost_stage_clear_rewards = new BattleEndBoost_stage_clear_rewards[0];
    result.battle_finish.unlock_messages = new BattleEndUnlock_messages[0];
    result.battle_finish.score_campaigns = new QuestScoreBattleFinishContext[0];
    result.battle_finish.drop_unit_entities = new BattleEndDrop_unit_entities[0];
    result.battle_finish.drop_material_unit_entities = new BattleEndDrop_material_unit_entities[0];
    result.battle_finish.drop_gear_entities = new BattleEndDrop_gear_entities[0];
    result.battle_finish.drop_material_gear_entities = new BattleEndDrop_material_gear_entities[0];
    result.battle_finish.drop_supply_entities = new BattleEndDrop_supply_entities[0];
    result.battle_finish.player_character_intimates_in_battle = new BattleEndPlayer_character_intimates_in_battle[0];
    result.battle_finish.unlock_intimate_skills = new UnlockIntimateSkill[0];
    result.battle_finish.trust_upper_limit = new BattleEndTrust_upper_limit[0];
    result.battle_finish.before_player = Player.Current;
    result.battle_finish.player_mission_results = new PlayerMissionHistory[0];
    BattleUI05Scene.ChangeScene(info, true, result);
  }

  public static void ReturnQuestScene(BattleInfo info, bool isWin, BattleEnd result)
  {
    NGGameDataManager instance1 = Singleton<NGGameDataManager>.GetInstance();
    instance1.QuestType = new CommonQuestType?();
    NGSceneManager instance2 = Singleton<NGSceneManager>.GetInstance();
    if (instance2.HasSavedChangeSceneParam() && instance1.IsFromPopupStageList)
    {
      NGSceneManager.ChangeSceneParam changeSceneParam = instance2.GetSavedChangeSceneParam();
      Action action = (Action) null;
      switch (changeSceneParam.sceneName)
      {
        case "unit004_JobChange":
          instance1.fromPopup = NGGameDataManager.FromPopup.Unit004JobChangeScene;
          instance2.importSceneChangeLog(BattleUI05Scene.overwriteSceneArgs(instance1.getSceneChangeLog()));
          break;
        case "unit004_2":
        case "unit004_2_sea":
          if (instance1.fromPopup != NGGameDataManager.FromPopup.Unit0042SceneCharacterQuest)
            instance1.fromPopup = NGGameDataManager.FromPopup.Unit0042SceneUnity;
          instance2.importSceneChangeLog(BattleUI05Scene.overwriteSceneArgs(instance1.getSceneChangeLog()));
          break;
        case "unit004_training":
          instance1.fromPopup = NGGameDataManager.FromPopup.Unit004Combine;
          instance2.importSceneChangeLog(BattleUI05Scene.overwriteSceneArgs(instance1.getSceneChangeLog()));
          action = instance1.returnSceneFromQuest;
          break;
        case "quest002_20_1":
          instance1.fromPopup = NGGameDataManager.FromPopup.Quest002201Scene;
          instance2.importSceneChangeLog(BattleUI05Scene.overwriteSceneArgs(instance1.getSceneChangeLog()));
          break;
        default:
          instance1.fromPopup = NGGameDataManager.FromPopup.None;
          break;
      }
      if (action == null)
        instance2.changeScene(changeSceneParam.sceneName, changeSceneParam.isStack, changeSceneParam.args);
      else
        action();
      instance2.ClearSavedChangeSceneParam();
      instance1.IsFromPopupStageList = false;
      instance1.returnSceneFromQuest = (Action) null;
      instance1.setSceneChangeLog();
    }
    else
    {
      if (instance2.HasSavedChangeSceneParam())
        instance2.ClearSavedChangeSceneParam();
      if (instance1.fromPopup != NGGameDataManager.FromPopup.None)
        instance1.clearScenePopupRecovery();
      switch (info.quest_type)
      {
        case CommonQuestType.Story:
          QuestStoryS questStoryS = ((IEnumerable<UnlockQuest>) result.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 1)).Select<UnlockQuest, QuestStoryS>((Func<UnlockQuest, QuestStoryS>) (x => MasterData.QuestStoryS[x.quest_s_id])).FirstOrDefault<QuestStoryS>() ?? MasterData.QuestStoryS[info.quest_s_id];
          int clearCount1 = isWin ? info.quest_loop_count + 1 : 0;
          Quest0022Scene.ChangeScene0022(false, questStoryS.quest_l.ID, questStoryS.quest_m.ID, info.quest_s_id, clearCount1, result.player_review);
          break;
        case CommonQuestType.Character:
          Quest00214Scene.ChangeScene(false, MasterData.QuestCharacterS[info.quest_s_id].unit.ID);
          break;
        case CommonQuestType.Extra:
          QuestExtraS questExtraS = MasterData.QuestExtraS[info.quest_s_id];
          if (info.isTempDeck)
          {
            PlayerExtraQuestS[] source = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).S(questExtraS.quest_l_QuestExtraL, questExtraS.quest_m_QuestExtraM, true);
            if ((source.Length != 0 ? (UnityValueUpItemQuest.makeSkipSortieQuestUnityValueUp(((IEnumerable<PlayerExtraQuestS>) source).Select<PlayerExtraQuestS, int>((Func<PlayerExtraQuestS, int>) (x => x._quest_extra_s))).Count == source.Length ? 1 : 0) : 0) != 0)
            {
              Quest002201Scene.changeScene(false, questExtraS.quest_l_QuestExtraL, questExtraS.quest_m_QuestExtraM);
              break;
            }
          }
          QuestExtraS nextQuest = ((IEnumerable<UnlockQuest>) result.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 3)).Select<UnlockQuest, QuestExtraS>((Func<UnlockQuest, QuestExtraS>) (x => MasterData.QuestExtraS[x.quest_s_id])).FirstOrDefault<QuestExtraS>() ?? questExtraS;
          QuestExtraM questM = questExtraS.quest_m;
          int questExtraCategory;
          if (questM == null)
          {
            QuestExtraL questL = questExtraS.quest_l;
            questExtraCategory = questL != null ? questL.category_QuestExtraCategory : 0;
          }
          else
            questExtraCategory = questM.category_QuestExtraCategory;
          int activeTabIndex = questExtraCategory;
          PlayerExtraQuestS[] resultExtraData = ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).S(nextQuest.quest_l_QuestExtraL, nextQuest.quest_m_QuestExtraM) ?? new PlayerExtraQuestS[0];
          bool flag = false;
          if (resultExtraData != null)
          {
            PlayerExtraQuestS playerExtraQuestS = ((IEnumerable<PlayerExtraQuestS>) resultExtraData).FirstOrDefault<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s.ID == nextQuest.ID));
            if (playerExtraQuestS != null && playerExtraQuestS.remain_battle_count.HasValue && playerExtraQuestS.remain_battle_count.Value <= 0)
              flag = true;
          }
          if (result.score_campaigns.Length != 0)
          {
            if (flag || resultExtraData == null || resultExtraData.Length == 0 || ServerTime.NowAppTime() > resultExtraData[0].today_day_end_at || ServerTime.NowAppTimeAddDelta() > result.score_campaigns[0].end_at)
            {
              Quest00217Scene.ChangeScene(false, activeTabIndex);
              break;
            }
            BattleUI05Scene.ReturnRankingQuestScene(result.score_campaigns[0], nextQuest);
            break;
          }
          if (nextQuest.extra_quest_area == CommonExtraQuestArea.key)
          {
            PlayerQuestGate[] source = SMManager.Get<PlayerQuestGate[]>();
            PlayerQuestGate playerQuestGate = ((IEnumerable<PlayerQuestGate>) source).FirstOrDefault<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => ((IEnumerable<int>) x.quest_ids).Any<int>((Func<int, bool>) (y => ((IEnumerable<PlayerExtraQuestS>) resultExtraData).Any<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (z => z._quest_extra_s == y))))));
            if (!flag && resultExtraData.Length != 0 && playerQuestGate != null)
            {
              DateTime dateTime = ServerTime.NowAppTime();
              DateTime? endAt = playerQuestGate.end_at;
              if ((endAt.HasValue ? (dateTime > endAt.GetValueOrDefault() ? 1 : 0) : 0) == 0 && playerQuestGate.in_progress)
              {
                Quest00220Scene.ChangeScene00220(false, nextQuest.quest_l_QuestExtraL, nextQuest.quest_m_QuestExtraM, isKeyQuest: true);
                break;
              }
            }
            PlayerExtraQuestS[] self = SMManager.Get<PlayerExtraQuestS[]>();
            IEnumerable<int> idGateS = ((IEnumerable<PlayerQuestGate>) source).SelectMany<PlayerQuestGate, int>((Func<PlayerQuestGate, IEnumerable<int>>) (s => (IEnumerable<int>) s.quest_ids));
            int questLQuestExtraL = nextQuest.quest_l_QuestExtraL;
            if (((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) self).M(questLQuestExtraL)).All<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => idGateS.Contains<int>(x._quest_extra_s))))
            {
              Quest002171Scene.ChangeScene(false);
              break;
            }
            if (nextQuest.seek_type == QuestExtra.SeekType.L)
            {
              Quest00219Scene.ChangeScene(nextQuest.ID, false);
              break;
            }
            Quest00217Scene.ChangeScene(false, activeTabIndex);
            break;
          }
          if (flag || resultExtraData == null || resultExtraData.Length == 0 || ServerTime.NowAppTime() > resultExtraData[0].today_day_end_at)
          {
            if (activeTabIndex > 5 && activeTabIndex < 9)
            {
              Quest002SideStoryScene.ChangeScene(false, activeTabIndex);
              break;
            }
            Quest00217Scene.ChangeScene(false, activeTabIndex);
            break;
          }
          Quest00220Scene.ChangeScene00220(false, nextQuest.quest_l_QuestExtraL, nextQuest.quest_m_QuestExtraM);
          break;
        case CommonQuestType.Harmony:
          Quest00214Scene.ChangeScene(false, info.quest_s_id, true);
          break;
        case CommonQuestType.Sea:
          try
          {
            Persist.seaHomeUnitDate.Data.DisplaySameUnitIDs.Clear();
            Persist.seaHomeUnitDate.Flush();
          }
          catch
          {
            Persist.seaHomeUnitDate.Delete();
          }
          QuestSeaS questSeaS = ((IEnumerable<UnlockQuest>) result.unlock_quests).Where<UnlockQuest>((Func<UnlockQuest, bool>) (x => x.quest_type == 9)).Select<UnlockQuest, QuestSeaS>((Func<UnlockQuest, QuestSeaS>) (x => MasterData.QuestSeaS[x.quest_s_id])).FirstOrDefault<QuestSeaS>() ?? MasterData.QuestSeaS[info.quest_s_id];
          int clearCount2 = isWin ? info.quest_loop_count + 1 : 0;
          Quest0022Scene.ChangeSceneSea(false, questSeaS.quest_xl_QuestSeaXL, questSeaS.quest_l_QuestSeaL, questSeaS.quest_m_QuestSeaM, info.quest_s_id, clearCount2, result.player_review);
          break;
        case CommonQuestType.GuildRaid:
          Raid032BattleResultScene.ChangeScene(info, BattleUI05Scene.guildRaidBattleFinishResponse);
          BattleUI05Scene.guildRaidBattleFinishResponse = (WebAPI.Response.GuildraidBattleFinish) null;
          break;
        default:
          MypageScene.ChangeScene();
          break;
      }
    }
  }

  private static List<NGSceneManager.SavedSceneLog> overwriteSceneArgs(
    List<NGSceneManager.SavedSceneLog> logs)
  {
    if (logs == null || !logs.Any<NGSceneManager.SavedSceneLog>())
      return logs;
    foreach (NGSceneManager.SavedSceneLog log in logs)
    {
      switch (log.name)
      {
        case "unit004_unit_list":
        case "unit004_6_8":
          if (log.args.Length != 0 && log.args[0] is Unit00468Scene.Mode)
          {
            switch ((Unit00468Scene.Mode) log.args[0])
            {
              case Unit00468Scene.Mode.Unit00468:
              case Unit00468Scene.Mode.Unit00468withFocusLastReference:
                if (log.args.Length == 2)
                {
                  PlayerDeck playerDeck = (PlayerDeck) log.args[1];
                  log.createParams = (Func<object[]>) (() => new object[2]
                  {
                    (object) Unit00468Scene.Mode.Unit00468withFocusLastReference,
                    (object) playerDeck
                  });
                  continue;
                }
                continue;
              case Unit00468Scene.Mode.Unit004682:
              case Unit00468Scene.Mode.Unit004682withFocusLastReference:
                if (log.args.Length == 2)
                {
                  Unit0046Menu.OneFormationInfo info = (Unit0046Menu.OneFormationInfo) log.args[1];
                  log.createParams = (Func<object[]>) (() => new object[2]
                  {
                    (object) Unit00468Scene.Mode.Unit004682withFocusLastReference,
                    (object) info
                  });
                  continue;
                }
                continue;
              case Unit00468Scene.Mode.Unit00411:
              case Unit00468Scene.Mode.Unit00411withFocusLastReference:
                log.createParams = (Func<object[]>) (() => new object[1]
                {
                  (object) Unit00468Scene.Mode.Unit00411withFocusLastReference
                });
                continue;
              default:
                continue;
            }
          }
          else
            continue;
        case "unit004_UnitTraining_List":
          log.createParams = (Func<object[]>) (() => new object[1]
          {
            (object) -1
          });
          continue;
        case "tower029_unit_selection":
          if (log.args.Length == 3)
          {
            object arg0 = log.args[0];
            object arg1 = log.args[1];
            object arg2 = log.args[2];
            log.createParams = (Func<object[]>) (() => new object[4]
            {
              arg0,
              arg1,
              arg2,
              (object) -1
            });
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    return logs;
  }

  private static void ReturnRankingQuestScene(
    QuestScoreBattleFinishContext campaign,
    QuestExtraS nextQuest)
  {
    switch (CampaignQuest.GetEvetnTerm(campaign, BattleUI05Scene.serverTime))
    {
      case CampaignQuest.RankingEventTerm.normal:
        Quest00220Scene.ChangeScene00220(false, nextQuest.quest_l_QuestExtraL, nextQuest.quest_m_QuestExtraM);
        break;
      case CampaignQuest.RankingEventTerm.receive:
        Quest00217Scene.ChangeScene(false);
        break;
      case CampaignQuest.RankingEventTerm.aggregate:
        Quest00217Scene.ChangeScene(false);
        break;
    }
  }

  public IEnumerator onStartSceneAsync(BattleInfo info, bool isWin, BattleEnd result)
  {
    BattleUI05Scene battleUi05Scene = this;
    if (!battleUi05Scene.isInitialized)
    {
      battleUi05Scene.isInitialized = true;
      IEnumerator e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      {
        SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
        if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
        {
          battleUi05Scene.bgmFile = seaHomeMap.bgm_cuesheet_name;
          battleUi05Scene.bgmName = seaHomeMap.bgm_cue_name;
        }
      }
      switch (info.quest_type)
      {
        case CommonQuestType.Story:
          if (battleUi05Scene.checkNeedProgress(info.storyQuest))
          {
            e = WebAPI.QuestProgressStory((Action<WebAPI.Response.UserError>) (error => WebAPI.DefaultUserErrorCallback(error))).Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            WebAPI.SetLatestResponsedAt("QuestProgressStory");
            break;
          }
          break;
        case CommonQuestType.Character:
          if (battleUi05Scene.checkNeedProgress(info.charaQuest))
          {
            e = WebAPI.QuestProgressCharacter((Action<WebAPI.Response.UserError>) (error => WebAPI.DefaultUserErrorCallback(error))).Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            WebAPI.SetLatestResponsedAt("QuestProgressCharacter");
            WebAPI.SetLatestResponsedAt("QuestProgressHarmony");
            break;
          }
          break;
        case CommonQuestType.Extra:
          if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra") || !Singleton<NGGameDataManager>.GetInstance().IsConnectedResultQuestProgressExtra)
          {
            e = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (error => WebAPI.DefaultUserErrorCallback(error))).Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            WebAPI.SetLatestResponsedAt("QuestProgressExtra");
            break;
          }
          break;
        case CommonQuestType.Harmony:
          if (battleUi05Scene.checkNeedProgress(info.harmonyQuest))
          {
            e = WebAPI.QuestProgressHarmony((Action<WebAPI.Response.UserError>) (error => WebAPI.DefaultUserErrorCallback(error))).Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            WebAPI.SetLatestResponsedAt("QuestProgressHarmony");
            break;
          }
          break;
      }
      Singleton<NGGameDataManager>.GetInstance().IsConnectedResultQuestProgressExtra = false;
      e = battleUi05Scene.setBackGround(info);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      battleUi05Scene.sequences = new List<ResultMenuBase>()
      {
        (ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05ResultMenu>(),
        (ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05RewardMenu>(),
        (ResultMenuBase) null
      };
      bool enabledBtnResortie = !info.pvp;
      if (enabledBtnResortie)
      {
        if (info.storyQuest != null)
          enabledBtnResortie = Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() && !info.storyQuest.quest_story_s.story_only;
        else if (info.extraQuest != null)
          enabledBtnResortie = !info.extraQuest.quest_extra_s.story_only;
        else if (info.charaQuest != null)
          enabledBtnResortie = !info.charaQuest.quest_character_s.story_only;
        else if (info.harmonyQuest != null)
          enabledBtnResortie = !info.harmonyQuest.quest_harmony_s.story_only;
        else if (info.seaQuest != null)
          enabledBtnResortie = !info.seaQuest.quest_sea_s.story_only;
      }
      if (!enabledBtnResortie && result.battle_helpers.Length != 0)
        battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<FriendMenu>());
      if (result.stage_clear_rewards.Length != 0)
        battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05ClearBonusMenu>());
      if (result.boost_stage_clear_rewards.Length != 0)
        battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05ClearBonusLimitedMenu>());
      if (result.unlock_messages.Length != 0)
      {
        BattleUI05HardModeOpenMenu popup = ((Component) battleUi05Scene).GetComponent<BattleUI05HardModeOpenMenu>();
        e = popup.Init(info, result);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        battleUi05Scene.sequences.Add((ResultMenuBase) popup);
        popup = (BattleUI05HardModeOpenMenu) null;
      }
      if (result.score_campaigns.Length != 0)
      {
        ResultMenuBase component = (ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05RankingMenu>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          battleUi05Scene.sequences.Add(component);
          if (!result.score_campaigns[0].score_ranking_disabled)
            battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05NowRankingPopupMenu>());
          if (result.score_campaigns[0].score_achivement_rewards.Length != 0)
            battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05ScoreRewardPopupMenu>());
          if (result.score_campaigns[0].total_reward_exists && result.score_campaigns[0].score_total_rewards.Length != 0)
            battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05TotalScoreRewardPopupMenu>());
        }
      }
      if (result.events != null && result.events.Length != 0)
        battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05PunitiveExpeditionResultMenu>());
      if (result.get_sea_album_piece_counts != null && result.get_sea_album_piece_counts.Length != 0)
        battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<Popup030SeaPieceGetMenu>());
      if (result.receive_sea_album_ids != null && result.receive_sea_album_ids.Length != 0)
        battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<BattleUI05AlbumCompleteMenu>());
      if (enabledBtnResortie)
        battleUi05Scene.sequences.Add((ResultMenuBase) ((Component) battleUi05Scene).GetComponent<ResortieWithFriendMenu>());
      e = battleUi05Scene.InitMenus(info, isWin, result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private bool checkNeedProgress(PlayerStoryQuestS quest)
  {
    bool flag = quest.remain_battle_count.HasValue;
    if (flag && quest.end_at.HasValue && ServerTime.NowAppTimeAddDelta() > quest.end_at.Value)
      flag = false;
    return flag;
  }

  private bool checkNeedProgress(PlayerCharacterQuestS quest) => quest.remain_battle_count.HasValue;

  private bool checkNeedProgress(PlayerHarmonyQuestS quest) => quest.remain_battle_count.HasValue;

  private IEnumerator setBackGround(BattleInfo info)
  {
    BattleUI05Scene battleUi05Scene = this;
    string path = battleUi05Scene.setPathStory(info);
    Future<GameObject> bgF = Res.Prefabs.BackGround.ResultBackGround.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject bg = bgF.Result;
    Future<Sprite> bgSpriteF = Singleton<ResourceManager>.GetInstance().LoadOrNull<Sprite>(path);
    e = bgSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgSpriteF.Result, (Object) null))
    {
      bg.GetComponent<UI2DSprite>().sprite2D = bgSpriteF.Result;
      battleUi05Scene.backgroundPrefab = bg;
    }
  }

  public void onStartScene(BattleInfo info, bool isWin, BattleEnd result)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (this.isStarted)
      return;
    this.isStarted = true;
    this.startMenuSequencer(info, isWin, result);
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  private void startMenuSequencer(BattleInfo info, bool isWin, BattleEnd result)
  {
    this.isQuestAutoLap = Singleton<NGGameDataManager>.GetInstance().questAutoLap;
    this.StartCoroutine("RunMenusAsync", (object) Tuple.Create<BattleInfo, bool, BattleEnd>(info, isWin, result));
  }

  private void stopMenuSequencer() => this.StopCoroutine("RunMenusAsync");

  private IEnumerator RunMenusAsync(Tuple<BattleInfo, bool, BattleEnd> param)
  {
    yield return (object) this.RunMenus(param.Item1, param.Item2, param.Item3);
  }

  private string setPathStory(BattleInfo info)
  {
    switch (info.quest_type)
    {
      case CommonQuestType.Story:
        return !Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() ? Consts.GetInstance().DEFULAT_BACKGROUND : this.setStoryPath(info.storyQuest.quest_story_s);
      case CommonQuestType.Character:
        return this.setCharaPath(MasterData.QuestCharacterS[info.quest_s_id]);
      case CommonQuestType.Extra:
        return this.setExtraPath(info.extraQuest.quest_extra_s);
      case CommonQuestType.Harmony:
        return this.setHarmonyPath(MasterData.QuestHarmonyS[info.quest_s_id]);
      case CommonQuestType.Sea:
        return this.setSeaPath(info.seaQuest.quest_sea_s);
      default:
        return "";
    }
  }

  private string setStoryPath(QuestStoryS quest) => quest.GetBackgroundPath();

  private string setExtraPath(QuestExtraS quest) => quest.GetBackgroundPath();

  private string setCharaPath(QuestCharacterS quest) => quest.GetBackgroundPath();

  private string setHarmonyPath(QuestHarmonyS quest) => quest.GetBackgroundPath();

  private string setSeaPath(QuestSeaS quest) => quest.GetBackgroundPath();

  private IEnumerator InitMenus(BattleInfo info, bool isWin, BattleEnd result)
  {
    this.touchToNext.SetActive(false);
    foreach (ResultMenuBase sequence in this.sequences)
    {
      if (Object.op_Inequality((Object) sequence, (Object) null))
      {
        IEnumerator e = sequence.Init(info, result);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator RunMenus(BattleInfo info, bool isWin, BattleEnd result)
  {
    List<ResultMenuBase>.Enumerator seqe = this.sequences.GetEnumerator();
    IEnumerator e;
    while (seqe.MoveNext())
    {
      this.nowPlayBase = seqe.Current;
      if (!Object.op_Equality((Object) this.nowPlayBase, (Object) null))
      {
        this.touchToNext.SetActive(true);
        e = this.nowPlayBase.Run();
        while (e.MoveNext())
        {
          if (this.isQuestAutoLap && Object.op_Inequality((Object) this.nowPlayBase, (Object) null))
            this.nowPlayBase.isSkip = true;
          yield return e.Current;
        }
        e = (IEnumerator) null;
        this.toNextSequence = false;
        if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
        {
          Singleton<TutorialRoot>.GetInstance().CurrentAdvise();
          yield break;
        }
        else
        {
          while (!this.toNextSequence)
          {
            if (this.isQuestAutoLap)
              this.toNextSequence = true;
            yield return (object) null;
          }
          this.toNextSequence = false;
          e = this.nowPlayBase.OnFinish();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      else
        break;
    }
    this.nowPlayBase = (ResultMenuBase) null;
    this.touchToNext.SetActive(true);
    ((Collider) this.touchToNext.GetComponent<BoxCollider>()).enabled = false;
    while (seqe.MoveNext())
    {
      e = seqe.Current.Run();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    BattleUI05Scene.serverTime = ServerTime.NowAppTime();
    BattleUI05Scene.ReturnQuestScene(info, isWin, result);
  }

  public void onBackScene(BattleInfo info, bool isWin, BattleEnd result)
  {
    BattleUI05Scene.ReturnQuestScene(info, isWin, result);
  }

  private void Update()
  {
  }
}
