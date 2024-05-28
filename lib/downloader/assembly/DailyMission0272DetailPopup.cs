// Decompiled with JetBrains decompiler
// Type: DailyMission0272DetailPopup
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
public class DailyMission0272DetailPopup : BackButtonMonoBehaiviour
{
  [SerializeField]
  private GameObject button1;
  [SerializeField]
  private GameObject button2;
  [SerializeField]
  private UILabel titleLabel;
  [SerializeField]
  private UILabel detailLabel;
  [SerializeField]
  private UILabel progressLabel;
  [SerializeField]
  private List<GameObject> dirThumbImageList;
  [SerializeField]
  private List<UILabel> rewardNameLabelList;
  [SerializeField]
  private List<GameObject> rewardList;
  [SerializeField]
  private GameObject objCleared;
  [SerializeField]
  private UIButton btnTry;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UIGrid grid;
  private readonly string patternOpenUrl = "http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?";
  private readonly string patternGuild = "^(guild|raid).+";
  private bool isDailyData_;
  private int missionId_;
  private DailyMission0272Panel panelObject;
  private string changeScene_ = "";
  private int condition_ = -1;

  public IEnumerator Init(
    bool isDailyData,
    int missionId,
    string title,
    string detail,
    string progres,
    string sceneName,
    int condition,
    List<DailyMission0272Panel.RewardViewModel> r,
    DailyMission0272Panel panel,
    bool isCleared)
  {
    DailyMission0272DetailPopup mission0272DetailPopup = this;
    UIWidget component = ((Component) mission0272DetailPopup).GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component, (Object) null))
      ((UIRect) component).alpha = 0.0f;
    mission0272DetailPopup.isDailyData_ = isDailyData;
    mission0272DetailPopup.missionId_ = missionId;
    mission0272DetailPopup.panelObject = panel;
    mission0272DetailPopup.button2.SetActive(false);
    mission0272DetailPopup.button1.SetActive(true);
    mission0272DetailPopup.titleLabel.SetTextLocalize(title);
    mission0272DetailPopup.detailLabel.SetTextLocalize(detail);
    mission0272DetailPopup.progressLabel.SetTextLocalize(progres);
    for (int index = 0; index < mission0272DetailPopup.rewardList.Count; ++index)
      mission0272DetailPopup.rewardList[index].SetActive(false);
    for (int i = 0; i < r.Count; ++i)
    {
      IEnumerator e = r[i].LoadThumb(mission0272DetailPopup.dirThumbImageList[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      mission0272DetailPopup.rewardNameLabelList[i].SetTextLocalize(r[i].Name);
      mission0272DetailPopup.rewardList[i].SetActive(true);
    }
    mission0272DetailPopup.grid.Reposition();
    mission0272DetailPopup.scrollView.ResetPosition();
    yield return (object) null;
  }

  public void IbtnNo()
  {
    this.panelObject.IsPush = false;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void onTry()
  {
    bool flag = true;
    if (!this.isDailyData_)
    {
      PlayerAffiliation current = PlayerAffiliation.Current;
      if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) == 0)
      {
        this.goToGuildTop();
        goto label_30;
      }
    }
    if (this.isDailyData_ && Regex.Match(this.changeScene_, this.patternOpenUrl).Success)
    {
      this.StartCoroutine("callOpenUrlApi");
      return;
    }
    if (this.changeScene_ == "review")
    {
      this.StartCoroutine(this.callReviewApi());
      return;
    }
    if (this.changeScene_ == "quest002_4")
    {
      PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
      QuestStoryS questStoryS = (QuestStoryS) null;
      Quest00240723Menu.StoryMode storyMode = !MasterData.QuestStoryS.TryGetValue(this.condition_, out questStoryS) ? Quest00240723Menu.StoryMode.LostRagnarok : (Quest00240723Menu.StoryMode) questStoryS.quest_xl_QuestStoryXL;
      if (questStoryS != null && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x._quest_story_s == this.condition_)))
      {
        Quest00240723Scene.ChangeScene0024(false, MasterData.QuestStoryS[this.condition_].quest_l_QuestStoryL, true);
      }
      else
      {
        int? nullable = ((IEnumerable<PlayerStoryQuestS>) source).Select<PlayerStoryQuestS, QuestStoryS>((Func<PlayerStoryQuestS, QuestStoryS>) (x => x.quest_story_s)).Where<QuestStoryS>((Func<QuestStoryS, bool>) (s => (Quest00240723Menu.StoryMode) s.quest_xl_QuestStoryXL == storyMode)).Select<QuestStoryS, int?>((Func<QuestStoryS, int?>) (s => new int?(s.quest_l_QuestStoryL))).Max();
        if (nullable.HasValue)
          Quest00240723Scene.ChangeScene0024(false, nullable.Value, true);
      }
    }
    else if (this.changeScene_ == "gacha006_3")
      Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene_, true, (object) 2);
    else if (this.changeScene_.StartsWith("colosseum023"))
    {
      if (!SMManager.Get<Player>().GetFeatureColosseum() || !SMManager.Get<Player>().GetReleaseColosseum())
      {
        Singleton<PopupManager>.GetInstance().onDismiss();
        this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().DAILY_MISSION_0271_POPUP_TITLE, Consts.GetInstance().DAILY_MISSION_0271_COLOSSEUM));
        flag = false;
      }
      else
        Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene_, true);
    }
    else if (this.changeScene_.StartsWith("sea030_home"))
    {
      Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Singleton<NGSceneManager>.GetInstance().clearStack();
      Sea030HomeScene.ChangeScene(false);
    }
    else if (this.changeScene_.StartsWith("sea030_quest"))
    {
      Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
      Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
      Singleton<NGSceneManager>.GetInstance().clearStack();
      Sea030_questScene.ChangeScene(false);
    }
    else if (Regex.IsMatch(this.changeScene_, this.patternGuild))
    {
      if (!(this.changeScene_ == "guild028_1"))
      {
        PlayerAffiliation current = PlayerAffiliation.Current;
        if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) != 0)
        {
          if (this.changeScene_ == "raid_top")
          {
            RaidTopScene.ChangeSceneBattleFinish();
            goto label_30;
          }
          else
          {
            Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene_, true);
            goto label_30;
          }
        }
      }
      this.goToGuildTop();
    }
    else
      Singleton<NGSceneManager>.GetInstance().changeScene(this.changeScene_, true);
label_30:
    if (flag)
      Singleton<PopupManager>.GetInstance().onDismiss();
    this.panelObject.IsPush = false;
    Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
  }

  private void goToGuildTop()
  {
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
    Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD);
  }

  private IEnumerator callReviewApi()
  {
    Future<WebAPI.Response.DailymissionReview> future = WebAPI.DailymissionReview((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      StoreUtil.OpenMyStore();
      this.panelObject.ChangeClearState();
      this.panelObject.IsPush = false;
      Singleton<PopupManager>.GetInstance().onDismiss();
    }
  }

  private IEnumerator callOpenUrlApi()
  {
    Future<WebAPI.Response.DailymissionExternalLink> future = WebAPI.DailymissionExternalLink(this.missionId_, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      Application.OpenURL(this.changeScene_);
      this.panelObject.ChangeClearState();
      this.panelObject.IsPush = false;
      Singleton<PopupManager>.GetInstance().onDismiss();
    }
  }
}
