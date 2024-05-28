// Decompiled with JetBrains decompiler
// Type: MypagePanelMissionInfo
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
public class MypagePanelMissionInfo : MonoBehaviour
{
  [SerializeField]
  private UILabel titel;
  [SerializeField]
  private UILabel description;
  private BingoMission bingoMission;

  public void InitPanelMissionInfo(int panel_id)
  {
    if (!MasterData.BingoMission.TryGetValue(panel_id, out this.bingoMission))
      return;
    this.titel.SetTextLocalize(this.bingoMission.name);
    this.description.SetTextLocalize(this.bingoMission.detail);
  }

  public void onMissionInfo()
  {
    if (this.bingoMission == null || string.IsNullOrEmpty(this.bingoMission.scene_name))
      return;
    string sceneName = this.bingoMission.scene_name;
    int? arg1 = this.bingoMission.scene_arg;
    switch (sceneName)
    {
      case "mypage001_8_2":
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_8_2", false, (object) arg1);
        break;
      case "unit004_6_0822":
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822", false, (object) Unit0046Scene.From.Normal);
        break;
      case "unit004_6_0822_sea":
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822_sea", false, (object) Unit0046Scene.From.Normal);
        break;
      case "quest002_4":
        PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
        QuestStoryS questStoryS = (QuestStoryS) null;
        Quest00240723Menu.StoryMode storyMode = !arg1.HasValue || !MasterData.QuestStoryS.TryGetValue(arg1.Value, out questStoryS) ? Quest00240723Menu.StoryMode.LostRagnarok : (Quest00240723Menu.StoryMode) questStoryS.quest_xl_QuestStoryXL;
        if (questStoryS != null && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x._quest_story_s == arg1.Value)))
        {
          Quest00240723Scene.ChangeScene0024(false, MasterData.QuestStoryS[arg1.Value].quest_l_QuestStoryL, true);
          break;
        }
        Quest00240723Scene.ChangeScene0024(false, ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => (Quest00240723Menu.StoryMode) x.quest_story_s.quest_xl_QuestStoryXL == storyMode)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL)).Max(), true);
        break;
      case "quest002_20":
      case "quest002_19":
      case "quest002_26":
        if (arg1.HasValue)
        {
          this.StartCoroutine(this.onBannerEventConnection(sceneName, arg1.Value));
          break;
        }
        Quest00217Scene.ChangeScene(false);
        break;
      case "gacha006_3":
        if (arg1.HasValue)
        {
          Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, false, (object) arg1.Value);
          break;
        }
        Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, false);
        break;
      case "quest002_30":
        Quest00230Scene.ChangeScene(false, arg1.Value);
        break;
      default:
        if (sceneName.StartsWith("colosseum023"))
        {
          if (!SMManager.Get<Player>().GetFeatureColosseum() || !SMManager.Get<Player>().GetReleaseColosseum())
          {
            this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().DAILY_MISSION_0271_POPUP_TITLE, Consts.GetInstance().DAILY_MISSION_0271_COLOSSEUM));
            break;
          }
          Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, true);
          break;
        }
        switch (sceneName)
        {
          case "sea030_quest":
            Sea030_questScene.ChangeScene(false);
            return;
          case "sea030_home":
            Sea030HomeScene.ChangeScene(false);
            return;
          case "sea030_album":
            Sea030AlbumScene.ChangeScene(false);
            return;
          default:
            if (sceneName.StartsWith("unit004_top"))
            {
              MypageScene.ChangeSceneOnError();
              return;
            }
            Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, true);
            return;
        }
    }
  }

  private IEnumerator onBannerEventConnection(string changeSchene, int sID)
  {
    Future<WebAPI.Response.QuestProgressExtra> Extra;
    IEnumerator e1;
    if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra"))
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        WebAPI.DefaultUserErrorCallback(error);
      }));
      e1 = Extra.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (Extra.Result != null)
        WebAPI.SetLatestResponsedAt("QuestProgressExtra");
      Extra = (Future<WebAPI.Response.QuestProgressExtra>) null;
    }
    if (!((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).Any<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s != null && x.quest_extra_s.ID == sID)))
    {
      Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
      e1 = time_popup.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject result = time_popup.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result);
    }
    else
    {
      if (changeSchene == "quest002_26")
      {
        QuestScoreCampaignProgress[] ScoreCampaingProgress = SMManager.Get<QuestScoreCampaignProgress[]>();
        if (ScoreCampaingProgress == null)
        {
          Extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
          e1 = Extra.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          if (Extra.Result == null)
          {
            yield break;
          }
          else
          {
            ScoreCampaingProgress = SMManager.Get<QuestScoreCampaignProgress[]>();
            Extra = (Future<WebAPI.Response.QuestProgressExtra>) null;
          }
        }
        int idl = MasterData.QuestExtraS[sID].quest_l_QuestExtraL;
        if (ScoreCampaingProgress != null)
        {
          e1 = ServerTime.WaitSync();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          QuestScoreCampaignProgress campaign = ((IEnumerable<QuestScoreCampaignProgress>) ScoreCampaingProgress).FirstOrDefault<QuestScoreCampaignProgress>((Func<QuestScoreCampaignProgress, bool>) (x => x.quest_extra_l == idl));
          if (campaign != null && CampaignQuest.GetEvetnTerm(campaign, ServerTime.NowAppTimeAddDelta()) == CampaignQuest.RankingEventTerm.aggregate)
          {
            Consts instance = Consts.GetInstance();
            ModalWindow.Show(instance.DAILY_MISSION_0271_RANKING_TITILE, instance.DAILY_MISSION_0271_RANKING_TITILE, (Action) (() => { }));
            yield break;
          }
        }
        ScoreCampaingProgress = (QuestScoreCampaignProgress[]) null;
      }
      if (changeSchene == "quest002_20")
        Quest00220Scene.ChangeScene00220(sID);
      else if (changeSchene == "quest002_19")
        Quest00219Scene.ChangeScene(sID);
      else if (changeSchene == "quest002_26")
        Quest00226Scene.ChangeScene(sID);
    }
  }
}
