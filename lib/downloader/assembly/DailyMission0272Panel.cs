// Decompiled with JetBrains decompiler
// Type: DailyMission0272Panel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using MissionData;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniLinq;
using UnityEngine;

#nullable disable
public class DailyMission0272Panel : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> iconObjectList;
  [SerializeField]
  private UILabel missionText;
  [SerializeField]
  private UILabel progressText;
  [SerializeField]
  private GameObject slcDisable;
  [SerializeField]
  private UILabel missionPointText;
  [SerializeField]
  private GameObject missionPoint;
  [SerializeField]
  private GameObject challengeButton;
  [SerializeField]
  private GameObject missionItemWithReceiveButton;
  [SerializeField]
  private GameObject missionItemWithoutReceiveButton;
  [SerializeField]
  private GameObject achievedButton;
  [SerializeField]
  private GameObject receivedButton;
  private DailyMissionController controller;
  private IMissionAchievement playerDailyMission;
  private List<DailyMission0272Panel.RewardViewModel> rewardModelList;
  private bool isClear;
  private DailyMission0272Panel.RewardProgress rewardProgress = DailyMission0272Panel.RewardProgress.None;
  private UISprite gagueSprite;
  private IMission mission;
  private int originGuageWidth;
  private DailyMissionWindow _baseMenu;
  private int _type;
  private bool isSetupComplete;
  private readonly string patternOpenUrl = "http(s)?://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?";
  private readonly string patternGuild = "^(guild|raid).+";

  public IEnumerator Init(
    DailyMissionController controller,
    IMissionAchievement pdm,
    DailyMissionWindow baseMenu,
    int type)
  {
    this.controller = controller;
    this.playerDailyMission = pdm;
    this._baseMenu = baseMenu;
    this._type = type;
    this.rewardModelList = new List<DailyMission0272Panel.RewardViewModel>();
    for (int index = 0; index < this.playerDailyMission.rewards.Length; ++index)
      this.rewardModelList.Add(new DailyMission0272Panel.RewardViewModel(this.playerDailyMission.rewards[index]));
    this.isClear = pdm.isCleared;
    this.rewardProgress = DailyMission0272Panel.RewardProgress.None;
    if (this.isClear)
      this.rewardProgress = pdm.isReceived ? DailyMission0272Panel.RewardProgress.Received : DailyMission0272Panel.RewardProgress.Receive;
    else if (pdm.isOwnCleared)
      this.rewardProgress = DailyMission0272Panel.RewardProgress.OwnCleared;
    this.slcDisable.SetActive(this.rewardProgress == DailyMission0272Panel.RewardProgress.Received && Object.op_Inequality((Object) this.slcDisable, (Object) null));
    for (int i = 0; i < this.rewardModelList.Count; ++i)
    {
      IEnumerator e = this.rewardModelList[i].LoadThumb(this.iconObjectList[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.mission = this.playerDailyMission.mission;
    this.SetGague(this.mission, this.playerDailyMission.progress_count);
    this.SetButtonsStatus(this.rewardProgress, this.playerDailyMission);
    this.isSetupComplete = true;
    yield return (object) null;
  }

  private void SetButtonsStatus(
    DailyMission0272Panel.RewardProgress progress,
    IMissionAchievement missionAchievement = null)
  {
    this.missionItemWithoutReceiveButton.SetActive(false);
    this.missionItemWithReceiveButton.SetActive(false);
    this.challengeButton.SetActive(false);
    this.receivedButton.SetActive(false);
    this.achievedButton.SetActive(false);
    switch (progress)
    {
      case DailyMission0272Panel.RewardProgress.None:
        this.missionItemWithoutReceiveButton.SetActive(true);
        this.challengeButton.SetActive(true);
        break;
      case DailyMission0272Panel.RewardProgress.Receive:
        this.missionItemWithReceiveButton.SetActive(true);
        break;
      case DailyMission0272Panel.RewardProgress.OwnCleared:
        this.missionItemWithoutReceiveButton.SetActive(true);
        this.achievedButton.SetActive(true);
        break;
      case DailyMission0272Panel.RewardProgress.Received:
        this.missionItemWithoutReceiveButton.SetActive(true);
        this.receivedButton.SetActive(true);
        break;
    }
  }

  public void ChangeClearState()
  {
    this.isClear = true;
    this.SetButtonsStatus(DailyMission0272Panel.RewardProgress.Receive);
    this.mission = this.playerDailyMission.mission;
    this.SetGague(this.mission, this.mission.progress_max);
  }

  public void SetGague(IMission mission, int count)
  {
    this.missionText.SetTextLocalize(mission.name);
    UILabel progressText = this.progressText;
    string missionProgressFmT2 = Consts.GetInstance().DAILY_MISSION_PROGRESS_FMT2;
    Hashtable args = new Hashtable();
    string progressNonmember;
    if (!mission.isGuild)
    {
      progressNonmember = count.ToString();
    }
    else
    {
      if (count <= 0)
      {
        PlayerAffiliation current = PlayerAffiliation.Current;
        if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) == 0)
        {
          progressNonmember = Consts.GetInstance().GUILD_MISSION_PROGRESS_NONMEMBER;
          goto label_6;
        }
      }
      progressNonmember = count.ToString();
    }
label_6:
    args.Add((object) nameof (count), (object) progressNonmember);
    args.Add((object) "max", (object) mission.progress_max);
    string text = Consts.Format(missionProgressFmT2, (IDictionary) args);
    progressText.SetTextLocalize(text);
    if (mission.missionType == MissionType.daily && mission.point != 0)
    {
      this.missionPointText.SetTextLocalize(mission.point);
      this.missionPoint.SetActive(true);
    }
    else
      this.missionPoint.SetActive(false);
  }

  public bool IsPush { get; set; }

  public bool IsPushAndSet()
  {
    if (this.IsPush)
      return true;
    this.IsPush = true;
    return false;
  }

  public void onPanelButton()
  {
    if (!this.isSetupComplete || this.IsPushAndSet())
      return;
    this._baseMenu.StartCoroutine(this.openPopup0273());
  }

  private IEnumerator receiveReward()
  {
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.loadingMode = 1;
    common.isLoading = true;
    IMissionAchievement[] datUpdate = (IMissionAchievement[]) null;
    IEnumerator e1 = ServerTime.WaitSync();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (this.playerDailyMission.isDaily)
    {
      Future<WebAPI.Response.DailymissionReceive> future = WebAPI.DailymissionReceive(this.playerDailyMission.mission_id, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
      }));
      e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      common.isLoading = false;
      common.loadingMode = 0;
      if (future.Result == null)
      {
        yield break;
      }
      else
      {
        datUpdate = ((IEnumerable<PlayerDailyMissionAchievement>) future.Result.player_daily_missions).Select<PlayerDailyMissionAchievement, IMissionAchievement>((Func<PlayerDailyMissionAchievement, IMissionAchievement>) (dm => Util.Create(dm))).ToArray<IMissionAchievement>();
        future = (Future<WebAPI.Response.DailymissionReceive>) null;
      }
    }
    else
    {
      Future<WebAPI.Response.GuildmissionReceive> future = WebAPI.GuildmissionReceive(this.playerDailyMission.mission_id, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
      }));
      e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      common.isLoading = false;
      common.loadingMode = 0;
      if (future.Result == null)
      {
        yield break;
      }
      else
      {
        datUpdate = ((IEnumerable<GuildMissionInfo>) future.Result.player_guild_missions).Select<GuildMissionInfo, IMissionAchievement>((Func<GuildMissionInfo, IMissionAchievement>) (gm => Util.Create(gm))).ToArray<IMissionAchievement>();
        future = (Future<WebAPI.Response.GuildmissionReceive>) null;
      }
    }
    Future<GameObject> clearPrefab = Res.Prefabs.battle.DailyMission_Clear.Load<GameObject>();
    e1 = clearPrefab.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    GameObject result = clearPrefab.Result;
    e1 = Singleton<PopupManager>.GetInstance().open(result).GetComponent<DailyMission0271Clear>().SetClearBonus(this.rewardModelList.ToArray());
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_0534");
    int[] types = new int[1]{ this._type };
    e1 = this._baseMenu.InitMissionList(datUpdate, types, false);
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (this.playerDailyMission.isDaily)
      this._baseMenu.InitializePointRewardSection(datUpdate);
  }

  private IEnumerator openPopup0273()
  {
    DailyMission0272Panel panel = this;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_027_3__anim_popup02.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    GameObject pObj = Singleton<PopupManager>.GetInstance().open(result, isNonSe: true, isNonOpenAnime: true);
    string str;
    if (!panel.mission.isDaily)
      str = Consts.Format(Consts.GetInstance().GUILD_MISSION_OWN_PROGRESS, (IDictionary) new Hashtable()
      {
        {
          (object) "count",
          (object) panel.playerDailyMission.own_progress_count
        },
        {
          (object) "max",
          (object) panel.mission.own_progress_max
        }
      });
    else
      str = panel.progressText.text;
    string progres = str;
    e = pObj.GetComponent<DailyMission0272DetailPopup>().Init(panel.mission.isDaily, panel.mission.ID, panel.mission.name, panel.mission.detail, progres, panel.mission.scene, panel.mission.condition, panel.rewardModelList, panel, panel.playerDailyMission.isCleared);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(pObj);
  }

  public void OnChallengeButton()
  {
    if (!this.isSetupComplete || this.IsPushAndSet())
      return;
    if (this.mission.scene.Length == 0)
    {
      this._baseMenu.StartCoroutine(this.openPopup0273());
    }
    else
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      if (instance.sceneName != this.mission.scene)
      {
        if (!this.mission.isDaily)
        {
          PlayerAffiliation current = PlayerAffiliation.Current;
          if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) == 0)
          {
            this.goToGuildTop();
            goto label_40;
          }
        }
        if (this.mission.scene.StartsWith("unit004_top"))
        {
          MypageScene.ChangeSceneOnError();
        }
        else
        {
          if (this.mission.isDaily && Regex.Match(this.mission.scene, this.patternOpenUrl).Success)
          {
            this.StartCoroutine(this.callOpenUrlApi());
            return;
          }
          if (this.mission.scene == "review")
          {
            this.StartCoroutine(this.callReviewApi());
            return;
          }
          if (this.mission.scene == "quest002_4")
          {
            PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
            QuestStoryS questStoryS = (QuestStoryS) null;
            Quest00240723Menu.StoryMode storyMode = !MasterData.QuestStoryS.TryGetValue(this.mission.condition, out questStoryS) ? Quest00240723Menu.StoryMode.LostRagnarok : (Quest00240723Menu.StoryMode) questStoryS.quest_xl_QuestStoryXL;
            instance.destroyLoadedScenes();
            instance.clearStack();
            if (questStoryS != null && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x._quest_story_s == this.mission.condition)))
            {
              Quest00240723Scene.ChangeScene0024(false, MasterData.QuestStoryS[this.mission.condition].quest_l_QuestStoryL, true);
            }
            else
            {
              int? nullable = ((IEnumerable<PlayerStoryQuestS>) source).Select<PlayerStoryQuestS, QuestStoryS>((Func<PlayerStoryQuestS, QuestStoryS>) (x => x.quest_story_s)).Where<QuestStoryS>((Func<QuestStoryS, bool>) (s => (Quest00240723Menu.StoryMode) s.quest_xl_QuestStoryXL == storyMode)).Select<QuestStoryS, int?>((Func<QuestStoryS, int?>) (s => new int?(s.quest_l_QuestStoryL))).Max();
              if (nullable.HasValue)
                Quest00240723Scene.ChangeScene0024(false, nullable.Value, true);
            }
          }
          else if (this.mission.scene == "gacha006_3")
          {
            instance.destroyLoadedScenes();
            instance.clearStack();
            instance.changeScene(this.mission.scene, false, (object) 2);
          }
          else if (this.mission.scene.StartsWith("colosseum023"))
          {
            Singleton<NGGameDataManager>.GetInstance().IsSea = false;
            if (!SMManager.Get<Player>().GetFeatureColosseum() || !SMManager.Get<Player>().GetReleaseColosseum())
            {
              Singleton<PopupManager>.GetInstance().onDismiss();
              this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().DAILY_MISSION_0271_POPUP_TITLE, Consts.GetInstance().DAILY_MISSION_0271_COLOSSEUM));
            }
            else
            {
              instance.destroyLoadedScenes();
              instance.clearStack();
              instance.changeScene(this.mission.scene, false);
            }
          }
          else if (this.mission.scene.StartsWith("sea030_home"))
          {
            instance.sceneBase.IsPush = true;
            instance.destroyLoadedScenes();
            instance.clearStack();
            Sea030HomeScene.ChangeScene(false);
          }
          else if (this.mission.scene.StartsWith("sea030_quest"))
          {
            instance.sceneBase.IsPush = true;
            instance.destroyLoadedScenes();
            instance.clearStack();
            Sea030_questScene.ChangeScene(false);
          }
          else if (this.mission.scene.StartsWith("explore033_Top"))
          {
            instance.sceneBase.IsPush = true;
            instance.destroyLoadedScenes();
            instance.clearStack();
            Explore033TopScene.changeScene(false);
          }
          else if (Regex.IsMatch(this.mission.scene, this.patternGuild))
          {
            if (!(this.mission.scene == "guild028_1"))
            {
              PlayerAffiliation current = PlayerAffiliation.Current;
              if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) != 0)
              {
                if (this.mission.scene == "raid_top")
                {
                  instance.destroyLoadedScenes();
                  instance.clearStack();
                  RaidTopScene.ChangeSceneBattleFinish(false);
                  goto label_40;
                }
                else
                {
                  instance.destroyLoadedScenes();
                  instance.clearStack();
                  instance.changeScene(this.mission.scene, false);
                  goto label_40;
                }
              }
            }
            this.goToGuildTop();
          }
          else
          {
            if (this.mission.scene == "versus026_1")
            {
              Singleton<NGGameDataManager>.GetInstance().IsSea = false;
              Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
            }
            instance.destroyLoadedScenes();
            instance.clearStack();
            instance.changeScene(this.mission.scene, false);
          }
        }
      }
label_40:
      this.IsPush = false;
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    }
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
      this.ChangeClearState();
      this.IsPush = false;
      Singleton<PopupManager>.GetInstance().onDismiss();
    }
  }

  private IEnumerator callOpenUrlApi()
  {
    Future<WebAPI.Response.DailymissionExternalLink> future = WebAPI.DailymissionExternalLink(this.mission.ID, (Action<WebAPI.Response.UserError>) (e =>
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
      Application.OpenURL(this.mission.scene);
      this.ChangeClearState();
      this.IsPush = false;
      Singleton<PopupManager>.GetInstance().onDismiss();
    }
  }

  public void OnReceiveButton()
  {
    if (!this.isSetupComplete || this.IsPushAndSet())
      return;
    Player player = SMManager.Get<Player>();
    SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
    PlayerSeaDeck[] playerSeaDeckArray = SMManager.Get<PlayerSeaDeck[]>();
    switch (this.playerDailyMission.rewards[0].reward_type_id)
    {
      case 11:
        if (player.ap >= Player.GetApOverChargeLimit())
        {
          ModalWindow.Show("AP回復", "APが上限のため、受け取れません。", (Action) (() => this.IsPush = false));
          return;
        }
        break;
      case 18:
        if (player.bp >= player.bp_max)
        {
          ModalWindow.Show("CP回復", "CPが上限のため、受け取れません。", (Action) (() => this.IsPush = false));
          return;
        }
        break;
      case 30:
        if (seaPlayer == null || playerSeaDeckArray == null || playerSeaDeckArray.Length == 0)
        {
          ModalWindow.Show("海上編未プレイ", "海上編をはじめていないため、DPは受け取れません。", (Action) (() => this.IsPush = false));
          return;
        }
        if (seaPlayer.dp >= seaPlayer.dp_max)
        {
          ModalWindow.Show("DP回復", "DPが上限のため、受け取れません。", (Action) (() => this.IsPush = false));
          return;
        }
        break;
    }
    this._baseMenu.StartCoroutine(this.receiveReward());
  }

  public void OnAchieveButton()
  {
    if (!this.isSetupComplete || this.IsPushAndSet())
      return;
    this._baseMenu.StartCoroutine(this.OpenChallengeAgainConfirmationPopup());
  }

  private IEnumerator OpenChallengeAgainConfirmationPopup()
  {
    DailyMission0272Panel mission0272Panel = this;
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = Singleton<PopupManager>.GetInstance().open(mission0272Panel.controller.challengeAgainConfirmationPopupPrefab).GetComponent<ChallengeAgainConfirmationPopupController>().Init(new Action(mission0272Panel.\u003COpenChallengeAgainConfirmationPopup\u003Eb__43_0), new Action(mission0272Panel.\u003COpenChallengeAgainConfirmationPopup\u003Eb__43_1));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private enum RewardProgress
  {
    None = -1, // 0xFFFFFFFF
    Receive = 0,
    OwnCleared = 1,
    Received = 2,
  }

  public class RewardViewModel
  {
    public int id;
    public int typeId;
    public int quantity;
    private string message;
    private string _name;

    public RewardViewModel(IMissionReward reward)
    {
      this.id = reward.reward_id;
      this.typeId = reward.reward_type_id;
      this.quantity = reward.reward_quantity;
      this.message = reward.reward_message;
    }

    public string Name
    {
      get
      {
        if (this._name == null)
          this._name = !Enum.IsDefined(typeof (MasterDataTable.CommonRewardType), (object) this.typeId) ? this.message : CommonRewardType.GetRewardName((MasterDataTable.CommonRewardType) this.typeId, this.id, this.quantity);
        return this._name;
      }
    }

    public bool IsCoin => this.typeId == 10;

    public string RewardMessage => this.message;

    public IEnumerator LoadThumb(GameObject go)
    {
      CreateIconObject target = go.GetOrAddComponent<CreateIconObject>();
      IEnumerator e = target.CreateThumbnail((MasterDataTable.CommonRewardType) this.typeId, this.id, this.quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) target.GetIcon(), (Object) null))
      {
        foreach (UIButton componentsInChild in target.GetIcon().GetComponentsInChildren<UIButton>())
        {
          ((Behaviour) componentsInChild).enabled = false;
          BoxCollider component = ((Component) componentsInChild).GetComponent<BoxCollider>();
          if (Object.op_Inequality((Object) component, (Object) null))
            ((Collider) component).enabled = false;
        }
      }
    }
  }
}
