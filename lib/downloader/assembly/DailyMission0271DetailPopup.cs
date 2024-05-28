// Decompiled with JetBrains decompiler
// Type: DailyMission0271DetailPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniLinq;
using UnityEngine;

#nullable disable
public class DailyMission0271DetailPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private GameObject button1;
  [SerializeField]
  private GameObject button2;
  [SerializeField]
  private GameObject rewardIcon;
  [SerializeField]
  private UILabel titleLabel;
  [SerializeField]
  private UILabel detailLabel;
  [SerializeField]
  private UILabel progressLabel;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private UIScrollView scrollView;
  private DailyMission0271PanelRoot.DailyMissionView view;
  private string changeScene;
  private int? arg1;
  private readonly string patternGuild = "^(guild|raid).+";

  public IEnumerator Init(DailyMission0271PanelRoot.DailyMissionView v, DailyMission0271Panel panel)
  {
    DailyMission0271DetailPopup mission0271DetailPopup = this;
    UIWidget w = ((Component) mission0271DetailPopup).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) w, (Object) null))
      ((UIRect) w).alpha = 0.0f;
    mission0271DetailPopup.view = v;
    mission0271DetailPopup.changeScene = mission0271DetailPopup.view.scene;
    mission0271DetailPopup.arg1 = mission0271DetailPopup.view.arg1;
    if (string.IsNullOrEmpty(mission0271DetailPopup.view.scene))
    {
      mission0271DetailPopup.button2.SetActive(false);
      mission0271DetailPopup.button1.SetActive(true);
    }
    else
    {
      mission0271DetailPopup.button1.SetActive(false);
      mission0271DetailPopup.button2.SetActive(true);
    }
    mission0271DetailPopup.titleLabel.SetTextLocalize(mission0271DetailPopup.view.name);
    mission0271DetailPopup.detailLabel.SetTextLocalize(mission0271DetailPopup.view.detail);
    mission0271DetailPopup.progressLabel.SetTextLocalize(mission0271DetailPopup.view.progressText);
    Future<GameObject> prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/dailymission027_1/dir_Mission_Reward");
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject listPrefab = prefabF.Result;
    MasterDataTable.BingoRewardGroup[] bingoRewardGroupArray = v.rewards;
    for (int index = 0; index < bingoRewardGroupArray.Length; ++index)
    {
      MasterDataTable.BingoRewardGroup bingoRewardGroup = bingoRewardGroupArray[index];
      e = listPrefab.CloneAndGetComponent<DailyMission0271MissonReward>(((Component) mission0271DetailPopup.grid).gameObject).Init(bingoRewardGroup.reward_type_id, bingoRewardGroup.reward_id, bingoRewardGroup.reward_quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    bingoRewardGroupArray = (MasterDataTable.BingoRewardGroup[]) null;
    mission0271DetailPopup.grid.repositionNow = true;
    mission0271DetailPopup.scrollView.ResetPosition();
    yield return (object) null;
    if (Object.op_Inequality((Object) w, (Object) null))
      ((UIRect) w).alpha = 1f;
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();

  public void onTry()
  {
    bool flag = true;
    if (!string.IsNullOrEmpty(this.changeScene))
    {
      Action<Action> action = (Action<Action>) (changeAction =>
      {
        if (changeAction == null)
          return;
        Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
        Singleton<NGSceneManager>.GetInstance().clearStack();
        changeAction();
      });
      if (this.changeScene == "mypage001_8_2")
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage001_8_2", false, (object) this.arg1);
      else if (this.changeScene == "unit004_6_0822")
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822", false, (object) Unit0046Scene.From.Normal);
      else if (this.changeScene == "unit004_6_0822_sea")
        Singleton<NGSceneManager>.GetInstance().changeScene("unit004_6_0822_sea", false, (object) Unit0046Scene.From.Normal);
      else if (this.changeScene == "quest002_4")
      {
        PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
        QuestStoryS questStoryS = (QuestStoryS) null;
        Quest00240723Menu.StoryMode storyMode = !this.arg1.HasValue || !MasterData.QuestStoryS.TryGetValue(this.arg1.Value, out questStoryS) ? Quest00240723Menu.StoryMode.LostRagnarok : (Quest00240723Menu.StoryMode) questStoryS.quest_xl_QuestStoryXL;
        if (this.arg1.HasValue && questStoryS != null && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x._quest_story_s == this.arg1.Value)))
          Quest00240723Scene.ChangeScene0024(false, MasterData.QuestStoryS[this.arg1.Value].quest_l_QuestStoryL, true);
        else
          Quest00240723Scene.ChangeScene0024(false, ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => (Quest00240723Menu.StoryMode) x.quest_story_s.quest_xl_QuestStoryXL == storyMode)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL)).Max(), true);
      }
      else if (this.changeScene == "quest002_20" || this.changeScene == "quest002_19" || this.changeScene == "quest002_26")
      {
        if (this.arg1.HasValue)
        {
          this.StartCoroutine(this.onBannerEventConnection(this.changeScene, this.arg1.Value));
          flag = false;
        }
        else
          Quest00217Scene.ChangeScene(false);
      }
      else if (this.changeScene == "quest002_17")
      {
        if (this.arg1.HasValue)
          Quest00217Scene.ChangeScene(false, this.arg1.Value);
        else
          Quest00217Scene.ChangeScene(false);
      }
      else if (this.changeScene == "gacha006_3")
      {
        if (this.arg1.HasValue)
          Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene, false, (object) this.arg1.Value);
        else
          Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene, false);
      }
      else if (this.changeScene == "quest002_30")
        Quest00230Scene.ChangeScene(false, this.arg1.Value);
      else if (this.changeScene.StartsWith("colosseum023"))
      {
        if (!SMManager.Get<Player>().GetFeatureColosseum() || !SMManager.Get<Player>().GetReleaseColosseum())
        {
          Singleton<PopupManager>.GetInstance().onDismiss();
          this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().DAILY_MISSION_0271_POPUP_TITLE, Consts.GetInstance().DAILY_MISSION_0271_COLOSSEUM));
          flag = false;
        }
        else
          Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene, true);
      }
      else if (this.changeScene == "unit004_top")
        Singleton<NGSceneManager>.GetInstance().changeScene("mypage", false);
      else if (this.changeScene.StartsWith("sea030_home"))
        action((Action) (() => Sea030HomeScene.ChangeScene(false)));
      else if (this.changeScene.StartsWith("sea030_quest"))
        action((Action) (() => Sea030_questScene.ChangeScene(false)));
      else if (this.changeScene.StartsWith("explore033_Top"))
        action((Action) (() => this.StartCoroutine(this.changeExploreScene())));
      else if (Regex.IsMatch(this.changeScene, this.patternGuild))
      {
        if (!(this.changeScene == "guild028_1"))
        {
          PlayerAffiliation current = PlayerAffiliation.Current;
          if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) != 0)
          {
            if (this.changeScene == "raid_top")
            {
              RaidTopScene.ChangeSceneBattleFinish();
              goto label_45;
            }
            else
            {
              Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene, true);
              goto label_45;
            }
          }
        }
        Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
        Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
        Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
        Singleton<NGGameDataManager>.GetInstance().IsSea = false;
        MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD);
      }
      else
        Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene, true);
    }
label_45:
    if (!flag)
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  private IEnumerator changeExploreScene()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    Explore033TopScene.changeScene(false);
  }

  private IEnumerator onBannerEventConnection(string changeSchene, int sID)
  {
    if (MasterData.QuestExtraS.ContainsKey(sID))
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Future<WebAPI.Response.QuestProgressExtra> Extra;
      IEnumerator e1;
      if (!WebAPI.IsResponsedAtRecent("QuestProgressExtra"))
      {
        Extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (error =>
        {
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          WebAPI.DefaultUserErrorCallback(error);
        }));
        e1 = Extra.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (Extra.Result != null)
          WebAPI.SetLatestResponsedAt("QuestProgressExtra");
        Extra = (Future<WebAPI.Response.QuestProgressExtra>) null;
      }
      if (!((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).Any<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s != null && x.quest_extra_s.ID == sID)))
      {
        Singleton<PopupManager>.GetInstance().onDismiss();
        Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
        e1 = time_popup.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        GameObject result = time_popup.Result;
        Singleton<PopupManager>.GetInstance().openAlert(result);
        yield break;
      }
      else
      {
        if (changeSchene == "quest002_26")
        {
          QuestScoreCampaignProgress[] ScoreCampaingProgress = SMManager.Get<QuestScoreCampaignProgress[]>();
          if (ScoreCampaingProgress == null)
          {
            Extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (e =>
            {
              Singleton<CommonRoot>.GetInstance().loadingMode = 0;
              WebAPI.DefaultUserErrorCallback(e);
            }));
            e1 = Extra.Wait();
            while (e1.MoveNext())
              yield return e1.Current;
            e1 = (IEnumerator) null;
            if (Extra.Result == null)
            {
              Singleton<CommonRoot>.GetInstance().loadingMode = 0;
              Singleton<PopupManager>.GetInstance().onDismiss();
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
              Singleton<CommonRoot>.GetInstance().loadingMode = 0;
              Singleton<PopupManager>.GetInstance().onDismiss();
              Consts instance = Consts.GetInstance();
              ModalWindow.Show(instance.DAILY_MISSION_0271_RANKING_TITILE, instance.DAILY_MISSION_0271_RANKING_TITILE, (Action) (() => { }));
              yield break;
            }
          }
          ScoreCampaingProgress = (QuestScoreCampaignProgress[]) null;
        }
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        if (changeSchene == "quest002_20")
          Quest00220Scene.ChangeScene00220(sID);
        else if (changeSchene == "quest002_19")
          Quest00219Scene.ChangeScene(sID);
        else if (changeSchene == "quest002_26")
          Quest00226Scene.ChangeScene(sID);
      }
    }
    Singleton<PopupManager>.GetInstance().onDismiss();
  }
}
