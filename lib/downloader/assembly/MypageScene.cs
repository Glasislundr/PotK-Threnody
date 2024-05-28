// Decompiled with JetBrains decompiler
// Type: MypageScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using Earth;
using GameCore;
using gu3.Device;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class MypageScene : NGSceneBase
{
  private MypageRootMenu Menu;
  private MypageTransition Transition;
  private BGChange BgChange;
  private string MyPageImagePath = string.Empty;
  private MypageIntroductionControl IntroControler;
  private bool isIntroEventFinished;
  private bool isConfirmDLC;
  private static bool sReadPickUpNotice;
  private static int sPickUpNoticeIndex;
  private static bool sReadPickUpUnit;

  public bool isAnimePlaying
  {
    get => Object.op_Inequality((Object) this.Menu, (Object) null) && this.Menu.isAnimePlaying;
  }

  public static void ChangeScene(
    MypageRootMenu.Mode menuMode = MypageRootMenu.Mode.NONE,
    bool guildRefresh = false,
    bool fromEarthTop = false,
    bool isEarthSuspend = false)
  {
    NGGameDataManager instance1 = Singleton<NGGameDataManager>.GetInstance();
    instance1.IsSea = false;
    instance1.IsEarth = false;
    instance1.IsColosseum = false;
    NGSceneManager instance2 = Singleton<NGSceneManager>.GetInstance();
    instance2.clearStack();
    instance2.destoryNonStackScenes();
    instance2.destroyLoadedScenes();
    if (guildRefresh)
      instance1.refreshGuildSetting = instance1.refreshGuildTop = true;
    if (menuMode == MypageRootMenu.Mode.GUILD && !PlayerAffiliation.Current.isGuildMember())
    {
      Guild02811Scene.ChangeScene();
    }
    else
    {
      if (menuMode != MypageRootMenu.Mode.NONE)
        MypageRootMenu.CurrentMode = menuMode;
      instance2.changeScene("mypage", false, (object) fromEarthTop, (object) isEarthSuspend);
    }
  }

  public override List<string> createResourceLoadList()
  {
    if (!Persist.integralNoaTutorial.Data.beginnersQuest)
    {
      List<string> resourceLoadList = new List<string>();
      this.isConfirmDLC = false;
      return resourceLoadList;
    }
    ResourceInfo resourceInfo = Singleton<ResourceManager>.GetInstance().ResourceInfo;
    HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) FileManager.GetEntries(DLC.ResourceDirectory));
    List<string> collection = new List<string>();
    List<string> stringList = new List<string>();
    foreach (ResourceInfo.Resource resource in resourceInfo)
    {
      switch (resource._value._path_type)
      {
        case ResourceInfo.PathType.AssetBundle:
          if (ResourceManager.IsInitialDLCTarget(resource._key, false, false))
          {
            collection.Add(resource._key);
            continue;
          }
          continue;
        case ResourceInfo.PathType.StreamingAssets:
          if (ResourceManager.IsInitialDLCTarget(resource._key, false, false))
          {
            stringList.Add(resource._key);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
    this.isConfirmDLC = true;
    collection.AddRange((IEnumerable<string>) collection);
    return collection;
  }

  public override bool createResourceLoadListConfirmDLC() => this.isConfirmDLC;

  public static void ChangeSceneOnError()
  {
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    Singleton<NGGameDataManager>.GetInstance().isCallHomeUpdateAllData = true;
    MypageScene.ChangeScene(MypageRootMenu.Mode.STORY);
  }

  public override IEnumerator onInitSceneAsync()
  {
    MypageScene mypageScene = this;
    mypageScene.Menu = mypageScene.menuBase as MypageRootMenu;
    mypageScene.Transition = ((Component) mypageScene).GetComponent<MypageTransition>();
    mypageScene.Menu.BackBtnEnable = false;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = mypageScene.Menu.InitSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(bool fromEarthTop, bool isEarthSuspend)
  {
    MypageScene mypageScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    mypageScene.isIntroEventFinished = false;
    mypageScene.Menu.BackBtnEnable = false;
    if (isEarthSuspend)
      mypageScene.CleanupEarthData();
    IEnumerator e;
    if (Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
    {
      e = new Future<NGGameDataManager.StartSceneProxyResult>(new Func<Promise<NGGameDataManager.StartSceneProxyResult>, IEnumerator>(Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxyImpl)).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = mypageScene.LoadBackGround();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mypageScene.BgChange = ((Component) mypageScene).GetComponent<BGChange>();
    e = mypageScene.Menu.OnStartSceneAsync(fromEarthTop);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene() => this.onStartScene(false, false);

  public void onStartScene(bool fromEarthTop, bool isEarthSuspend)
  {
    App.SetAutoSleep(true);
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.GetHeavenCommonFooter().IsPush = false;
    instance.ActiveBaseHomeMenu(true);
    instance.SetFooterEnable(true);
    this.Menu.OnStartScene();
    this.IntroControler = new MypageIntroductionControl();
    if (!fromEarthTop)
    {
      PlayerAffiliation crrPA = PlayerAffiliation.Current;
      PlayerAffiliation playerAffiliation = crrPA;
      bool flag = playerAffiliation != null && playerAffiliation.isGuildMember();
      this.IntroControler.addExecute((Func<int, bool>) (executed =>
      {
        List<PlayerLoginBonus> loginBonuses = Singleton<NGGameDataManager>.GetInstance().loginBonuses;
        return loginBonuses != null && loginBonuses.Any<PlayerLoginBonus>((Func<PlayerLoginBonus, bool>) (x => x.loginbonus.draw_type != LoginbonusDrawType.popup));
      }), new Func<int, IEnumerator>(this.doShowLoginBonus));
      if (!Persist.integralNoaTutorial.Data.beginnersQuest)
      {
        this.IntroControler.addExecute((Func<int, bool>) (executed => Singleton<NGGameDataManager>.GetInstance().playbackEventIds != null), new Func<int, IEnumerator>(this.doShowEventScript));
        this.IntroControler.addExecute((Func<int, bool>) (executed => this.IsImportantNotice()), new Func<int, IEnumerator>(this.doShowImportantNotice));
        this.IntroControler.addExecute((Func<int, bool>) (executed => this.IsUnitNotice()), new Func<int, IEnumerator>(this.doShowUnitNotice));
        this.IntroControler.addExecute((Func<int, bool>) (executed => Persist.gvgBattleEnvironment.Exists), new Func<int, IEnumerator>(this.ProcResumeGvg));
        this.IntroControler.addExecute((Func<int, bool>) (executed =>
        {
          List<PlayerLoginBonus> loginBonuses = Singleton<NGGameDataManager>.GetInstance().loginBonuses;
          return loginBonuses != null && loginBonuses.Any<PlayerLoginBonus>((Func<PlayerLoginBonus, bool>) (x => x.loginbonus.draw_type == LoginbonusDrawType.popup));
        }), new Func<int, IEnumerator>(this.doLoginPopupBonus));
        this.IntroControler.addExecute((Func<int, bool>) (executed =>
        {
          List<LevelRewardSchemaMixin> playerLevelRewards = Singleton<NGGameDataManager>.GetInstance().playerLevelRewards;
          return playerLevelRewards != null && playerLevelRewards.Count > 0;
        }), new Func<int, IEnumerator>(this.doLevelUpPopup));
        this.IntroControler.addExecute((Func<int, bool>) (executed => this.IsExploreRankingResult()), new Func<int, IEnumerator>(this.doExploreRankingPopup));
        this.IntroControler.addExecute((Func<int, bool>) (executed => !Singleton<ExploreDataManager>.GetInstance().IsNotRegisteredDeck() && !Singleton<ExploreDataManager>.GetInstance().LoginCalcDirty), new Func<int, IEnumerator>(this.doExploreBgCalc));
        this.IntroControler.addExecute((Func<int, bool>) (executed => Singleton<ExploreDataManager>.GetInstance().LoginCalcDirty), new Func<int, IEnumerator>(this.doShowExploreResult));
        this.IntroControler.addExecute((Func<int, bool>) (executed => crrPA != null && crrPA.status == GuildMembershipStatus.transfers), new Func<int, IEnumerator>(this.doShowGuildTransferPopup));
        if (flag)
        {
          this.IntroControler.addExecute((Func<int, bool>) (executed =>
          {
            WebAPI.Response.GuildTop guildTopResponse = this.Menu.GuildTopResponse;
            if (!guildTopResponse.raid_rank_period_id.HasValue || !guildTopResponse.raid_damage_rank.HasValue)
              return false;
            int? raidDamageRank = guildTopResponse.raid_damage_rank;
            int num = 1;
            return raidDamageRank.GetValueOrDefault() >= num & raidDamageRank.HasValue;
          }), new Func<int, IEnumerator>(this.doShowGuildRaidRankingResult));
          this.IntroControler.addExecute((Func<int, bool>) (executed => crrPA.guild.gvg_status == GvgStatus.finished), new Func<int, IEnumerator>(this.doShowGuildBattleResult));
          this.IntroControler.addExecute((Func<int, bool>) (executed => this.IsGuildLvUpEffect()), new Func<int, IEnumerator>(this.doShowGuildLvUpEffect));
          this.IntroControler.addExecute((Func<int, bool>) (executed => this.IsGuildBattleEntry()), new Func<int, IEnumerator>(this.doShowGuildBattleEntry));
          if (MypageRootMenu.CurrentMode == MypageRootMenu.Mode.GUILD)
            this.IntroControler.addExecute((Func<int, bool>) (executed => Singleton<NGGameDataManager>.GetInstance().HasReceivableGuildCheckIn), new Func<int, IEnumerator>(this.doShowGuildCheckin));
        }
        this.IntroControler.addExecute((Func<int, bool>) (executed => Singleton<NGGameDataManager>.GetInstance().has_exchangeable_subcoin), new Func<int, IEnumerator>(this.doCoinExchangePopup));
        this.IntroControler.addExecute((Func<int, bool>) (executed => Singleton<NGGameDataManager>.GetInstance().has_near_dead_subcoin), new Func<int, IEnumerator>(this.doCoinEndAtDayAfterTomorrowPopup));
      }
    }
    this.IntroControler.setEnd((Action<int>) (executed => this.finishedIntroScene(fromEarthTop, executed)));
    this.StartCoroutine(this.IntroControler.doExecute());
  }

  public IEnumerator onBackSceneAsync(bool fromEarthTop, bool isEarthSuspend)
  {
    IEnumerator e = this.onStartSceneAsync(false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onBackScene(bool fromEarthTop, bool isEarthSuspend)
  {
    this.onStartScene(false, false);
  }

  public override void onEndScene()
  {
    this.Menu.OnEndScene(this.isIntroEventFinished);
    Singleton<CommonRoot>.GetInstance().ActiveBaseHomeMenu(false);
  }

  public override IEnumerator onEndSceneAsync()
  {
    yield return (object) this.Menu.OnEndSceneAsync(this.isIntroEventFinished);
  }

  public void OnMenuModeChanged(MypageRootMenu.Mode mode)
  {
    if (mode != MypageRootMenu.Mode.GUILD)
      return;
    this.IntroControler = new MypageIntroductionControl();
    this.IntroControler.addExecute((Func<int, bool>) (executed => Singleton<NGGameDataManager>.GetInstance().HasReceivableGuildCheckIn), new Func<int, IEnumerator>(this.doShowGuildCheckin));
    this.StartCoroutine(this.IntroControler.doExecute());
  }

  private void finishedIntroScene(bool fromEarthTop, int executed)
  {
    this.isIntroEventFinished = true;
    if (Singleton<CommonRoot>.GetInstance().isLoading)
      this.StartCoroutine(this.doDelayHideLoadingLayer());
    this.Menu.OnIntroFinished(fromEarthTop);
  }

  private IEnumerator doDelayHideLoadingLayer()
  {
    yield return (object) new WaitForSeconds(0.1f);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void onClearSavedata()
  {
    Persist.auth.Delete();
    Persist.battleEnvironment.Delete();
    Persist.pvpSuspend.Delete();
    Persist.notification.Delete();
    Persist.sortOrder.Delete();
    Persist.tutorial.Delete();
    Persist.volume.Delete();
    Persist.earthData.Delete();
    Persist.missionHistory.Delete();
    Application.Quit();
  }

  public void onStartEarthCloud()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1039");
    this.StartCoroutine(this.Menu.CloudAnimationStart());
  }

  private IEnumerator LoadBackGround()
  {
    QuestBG questBG = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    bool flag = Object.op_Equality((Object) questBG, (Object) null);
    if (Object.op_Inequality((Object) questBG, (Object) null))
      flag = questBG.DiffStageLId && questBG.currentPos != 0;
    IEnumerator e;
    if (flag || Object.op_Equality((Object) this.BgChange, (Object) null) || Object.op_Equality((Object) questBG.current_xl, (Object) null) || Object.op_Equality((Object) questBG.current_l, (Object) null))
    {
      e = this.LoadBackGroundFromLastQuest();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (!((Object) questBG.current_xl.GetComponent<UI2DSprite>().sprite2D).name.Equals(this.MyPageImagePath) && Object.op_Inequality((Object) this.BgChange, (Object) null))
    {
      e = this.BgChange.SetXLBg();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.MyPageImagePath = ((Object) questBG.current_xl.GetComponent<UI2DSprite>().sprite2D).name;
    }
  }

  private IEnumerator LoadBackGroundFromLastQuest()
  {
    MypageScene mypageScene = this;
    IEnumerator e = ((Component) mypageScene).GetComponent<BGChange>().MypageBGprefabCreate();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    if (Object.op_Inequality((Object) backgroundComponent, (Object) null) && Object.op_Inequality((Object) backgroundComponent.current_xl, (Object) null))
      mypageScene.MyPageImagePath = ((Object) backgroundComponent.current_xl.GetComponent<UI2DSprite>().sprite2D).name;
  }

  private void CleanupEarthData()
  {
    MasterDataCache.GetList<int, UnitUnit>("UnitUnit", new Func<MasterDataReader, UnitUnit>(UnitUnit.Parse), (Func<UnitUnit, int>) (x => x.ID));
    MasterDataCache.GetList<int, UnitVoicePattern>("UnitVoicePattern", new Func<MasterDataReader, UnitVoicePattern>(UnitVoicePattern.Parse), (Func<UnitVoicePattern, int>) (x => x.ID));
    MasterDataCache.GetList<int, TipsTips>("TipsTips", new Func<MasterDataReader, TipsTips>(TipsTips.Parse), (Func<TipsTips, int>) (x => x.ID));
    MasterDataCache.GetList<int, UnitCharacter>("UnitCharacter", new Func<MasterDataReader, UnitCharacter>(UnitCharacter.Parse), (Func<UnitCharacter, int>) (x => x.ID));
    MasterDataCache.GetList<int, QuestStoryS>("QuestStoryS", new Func<MasterDataReader, QuestStoryS>(QuestStoryS.Parse), (Func<QuestStoryS, int>) (x => x.ID));
    MasterDataCache.GetList<int, QuestStoryL>("QuestStoryL", new Func<MasterDataReader, QuestStoryL>(QuestStoryL.Parse), (Func<QuestStoryL, int>) (x => x.ID));
    SMManager.Swap();
    MasterDataCache.SetGameMode(MasterDataCache.GameMode.HEAVEN);
    EarthDataManager.DestoryInstance();
  }

  public static void ClearCache()
  {
    MypageScene.sReadPickUpNotice = false;
    MypageScene.sPickUpNoticeIndex = 0;
    MypageScene.sReadPickUpUnit = false;
  }

  private bool IsImportantNotice()
  {
    OfficialInfoPopupSchema[] popupPickups = Singleton<NGGameDataManager>.GetInstance().officialInfoPopup.popup_pickups;
    if (popupPickups == null || !((IEnumerable<OfficialInfoPopupSchema>) popupPickups).Any<OfficialInfoPopupSchema>())
      return false;
    return !MypageScene.sReadPickUpNotice && Persist.noticeReadCheck.Data.CheckPickUpNoticeTime() || Persist.noticeReadCheck.Data.CheckPickUpNotice();
  }

  private IEnumerator doShowImportantNotice(int executed)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    OfficialInfoPopupSchema[] data = Singleton<NGGameDataManager>.GetInstance().officialInfoPopup.popup_pickups;
    Future<GameObject> ft = new ResourceObject("Prefabs/mypage/popup_ImportantNotice").Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject obj = ft.Result.Clone();
    obj.SetActive(false);
    e = obj.GetComponent<PopupNotice>().Initialize(data, MypageScene.sPickUpNoticeIndex);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PopupNotice popup = Singleton<PopupManager>.GetInstance().open(obj, isCloned: true).GetComponent<PopupNotice>();
    obj.SetActive(true);
    popup.Refresh();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    while (!popup.IsFinish)
      yield return (object) null;
    if (popup.SelectedInfo != null)
    {
      MypageScene.sPickUpNoticeIndex = popup.GetCurrentPageIndex();
      this.IntroControler.clear();
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      Mypage00182Scene.ChangeSceneDirect(popup.SelectedInfo);
      Singleton<PopupManager>.GetInstance().dismissWithoutAnim();
    }
    else
    {
      MypageScene.sReadPickUpNotice = true;
      MypageScene.sPickUpNoticeIndex = 0;
      Persist.noticeReadCheck.Data.UpdatePickUpNoticeTime();
      Persist.noticeReadCheck.Data.UpdatePickUpNotice();
      Persist.noticeReadCheck.Flush();
      Singleton<PopupManager>.GetInstance().dismiss();
    }
  }

  private bool IsUnitNotice()
  {
    OfficialInfoUnitPopup[] popupUnits = Singleton<NGGameDataManager>.GetInstance().officialInfoPopup.popup_units;
    return popupUnits != null && ((IEnumerable<OfficialInfoUnitPopup>) popupUnits).Any<OfficialInfoUnitPopup>() && !MypageScene.sReadPickUpUnit && Persist.noticeReadCheck.Data.CheckPickUpUnitTime();
  }

  private IEnumerator doShowUnitNotice(int executed)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    OfficialInfoUnitPopup[] data = Singleton<NGGameDataManager>.GetInstance().officialInfoPopup.popup_units;
    Future<GameObject> ft = new ResourceObject("Prefabs/mypage/popup_UnitNotice").Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject obj = ft.Result.Clone();
    obj.SetActive(false);
    e = obj.GetComponent<PopupNoticeUnit>().Initialize(data);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (obj.GetComponent<PopupNoticeUnit>().IsUnitExist())
    {
      PopupNoticeUnit popup = Singleton<PopupManager>.GetInstance().open(obj, isCloned: true).GetComponent<PopupNoticeUnit>();
      obj.SetActive(true);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      while (!popup.IsFinish)
        yield return (object) null;
      MypageScene.sReadPickUpUnit = true;
      if (popup.IsNonDispToday)
      {
        Persist.noticeReadCheck.Data.UpdatePickUpUnitTime();
        Persist.noticeReadCheck.Flush();
      }
      Singleton<PopupManager>.GetInstance().dismissWithoutAnim();
    }
  }

  private IEnumerator doShowLoginBonus(int executed)
  {
    this.IntroControler.clear();
    Startup00014Scene.changeScene(false);
    yield break;
  }

  private IEnumerator doShowEventScript(int executed)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    MypageScene mypageScene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Persist.eventStoryPlay.Data.SetReserveList(Singleton<NGGameDataManager>.GetInstance().playbackEventIds, mypageScene.sceneName);
    if (Persist.eventStoryPlay.Data.PlayEventScript(mypageScene.sceneName, 0, true))
      mypageScene.IntroControler.clear();
    return false;
  }

  private IEnumerator doLoginPopupBonus(int executed)
  {
    Future<GameObject> loginPopupFt = Res.Prefabs.popup.popup_000_14_4__anim_popup01.Load<GameObject>();
    IEnumerator e = loginPopupFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject loginPopupPrefab = loginPopupFt.Result;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    PlayerLoginBonus[] playerLoginBonusArray = Singleton<NGGameDataManager>.GetInstance().loginBonuses.Where<PlayerLoginBonus>((Func<PlayerLoginBonus, bool>) (x => x.loginbonus.draw_type == LoginbonusDrawType.popup)).ToArray<PlayerLoginBonus>();
    for (int index = 0; index < playerLoginBonusArray.Length; ++index)
    {
      PlayerLoginBonus loginBonus = playerLoginBonusArray[index];
      bool bWait = true;
      Startup000144Menu component = Singleton<PopupManager>.GetInstance().open(loginPopupPrefab).GetComponent<Startup000144Menu>();
      component.InitScene(loginBonus);
      component.OnOkButton = (Action) (() =>
      {
        Singleton<PopupManager>.GetInstance().onDismiss();
        bWait = false;
      });
      while (bWait)
        yield return (object) null;
    }
    playerLoginBonusArray = (PlayerLoginBonus[]) null;
  }

  private IEnumerator doLevelUpPopup(int executed)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    foreach (LevelRewardSchemaMixin playerLevelReward in Singleton<NGGameDataManager>.GetInstance().playerLevelRewards)
    {
      bool bWait = true;
      ModalWindow.Show(playerLevelReward.reward_title, playerLevelReward.reward_message, (Action) (() => bWait = false));
      while (bWait)
        yield return (object) null;
    }
    Singleton<NGGameDataManager>.GetInstance().playerLevelRewards = (List<LevelRewardSchemaMixin>) null;
  }

  private IEnumerator doExploreBgCalc(int executed)
  {
    ExploreDataManager explore = Singleton<ExploreDataManager>.GetInstance();
    if ((ServerTime.NowAppTime() - explore.LastSyncTime).TotalMilliseconds < (double) explore.TimeConfig["MYPAGE_UPDATE_SPAN"])
    {
      yield return (object) explore.LoadSuspendData(false);
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
      yield return (object) explore.LoadSuspendData(true);
      if (!explore.NeedShowBadge)
      {
        yield return (object) explore.ResumeExplore();
        explore.SetupLocalPush();
      }
      explore.ClearCache();
    }
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  private IEnumerator doShowExploreResult(int executed)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<GameObject> loader = new ResourceObject("Prefabs/explore033_Top/explore_Result").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = loader.Result;
    ExploreDataManager explore = Singleton<ExploreDataManager>.GetInstance();
    explore.LoginCalcDirty = false;
    ExploreResultReport report = Singleton<PopupManager>.GetInstance().open(result).GetComponent<ExploreResultReport>();
    e = report.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    e = report.ShowExpAnimation();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitUntil((Func<bool>) (() => report.IsFinish));
    Singleton<PopupManager>.GetInstance().dismiss();
    explore.ClearCache();
  }

  private bool IsExploreRankingResult()
  {
    return Singleton<ExploreDataManager>.GetInstance().HasRankingResult();
  }

  private IEnumerator doExploreRankingPopup(int executed)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Future<WebAPI.Response.ExploreRankingResult> futureF = WebAPI.ExploreRankingResult((Action<WebAPI.Response.UserError>) (e => Debug.LogError((object) ("MypageTopEvent Failed Load ExploreRankingResult : " + e.Code))));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureF.Result == null)
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    }
    else
    {
      WebAPI.Response.ExploreRankingResult resultData = futureF.Result;
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
      if (resultData.current_period_id.HasValue)
      {
        Persist.exploreRankingInfo.Data.lastPeriodId = resultData.current_period_id.Value;
        Persist.exploreRankingInfo.Data.isResultView = false;
      }
      else
        Persist.exploreRankingInfo.Data.isResultView = true;
      Persist.exploreRankingInfo.Flush();
      if (resultData.aggregate_period_id.HasValue)
      {
        int? aggregatePeriodId = resultData.aggregate_period_id;
        int num = 0;
        if (!(aggregatePeriodId.GetValueOrDefault() == num & aggregatePeriodId.HasValue) && resultData.rank != 0)
        {
          int period_id = resultData.aggregate_period_id.Value;
          int rank = resultData.rank;
          int defeatCount = resultData.defeat_count;
          int floor = MasterData.ExploreFloor[resultData.floor_id].floor;
          ExploreRankingRewardPopupSequence rankingRewardPopupSeq = new ExploreRankingRewardPopupSequence();
          yield return (object) rankingRewardPopupSeq.Init(period_id, rank, defeatCount, floor);
          if (resultData.ranking_rewards != null)
          {
            foreach (WebAPI.Response.ExploreRankingResultRanking_rewards rankingReward in resultData.ranking_rewards)
            {
              if (rankingReward.rewards != null)
              {
                foreach (WebAPI.Response.ExploreRankingResultRanking_rewardsRewards reward in rankingReward.rewards)
                  rankingRewardPopupSeq.addRewardData(rankingReward.condition_id, (MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity);
              }
            }
          }
          yield return (object) rankingRewardPopupSeq.Run();
        }
      }
    }
  }

  private bool IsGuildLvUpEffect()
  {
    return Persist.guildTopLevel.Data.level < PlayerAffiliation.Current.guild.appearance.level;
  }

  private IEnumerator doShowGuildLvUpEffect(int executed)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    Future<GameObject> ft = Res.Prefabs.guild028_2.GuildBase.Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject guildBaseObj = ft.Result;
    ft = (Future<GameObject>) null;
    ft = Res.Prefabs.guild028_2.GuildBase_for_levelup_anim.Load<GameObject>();
    e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject guildBaseEff = ft.Result;
    ft = (Future<GameObject>) null;
    ft = Res.Prefabs.popup.guild_base_levelup_anim.Load<GameObject>();
    e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GuildLevelUpAnimPopup guildLvUpAnimPopup = ft.Result.CloneAndGetComponent<GuildLevelUpAnimPopup>();
    ft = (Future<GameObject>) null;
    GuildImageCache guildImageCacheLvUp = new GuildImageCache();
    e = guildImageCacheLvUp.GuildBankLevelUpResourceLoad(1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    guildLvUpAnimPopup.Initialize(PlayerAffiliation.Current.guild.appearance.level, guildBaseObj, guildBaseEff, guildImageCacheLvUp);
    Singleton<PopupManager>.GetInstance().open(((Component) guildLvUpAnimPopup).gameObject, isCloned: true);
    Persist.guildTopLevel.Data.level = PlayerAffiliation.Current.guild.appearance.level;
    Persist.guildTopLevel.Flush();
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
  }

  private IEnumerator doShowGuildTransferPopup(int executed)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    bool bWait = true;
    ModalWindow.Show(Consts.GetInstance().GUILD_APPLY_APPLICANT_TITLE_CHANGE_GUILD, Consts.GetInstance().GUILD_APPLY_APPLICANT_MESSAGE_CHANGE_GUILD, (Action) (() => bWait = false));
    while (bWait)
      yield return (object) null;
  }

  private IEnumerator doCoinExchangePopup(int executed)
  {
    MypageScene mypageScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    Future<WebAPI.Response.HomeExchangeSubCoin> handler = WebAPI.HomeExchangeSubCoin();
    IEnumerator e = handler.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGGameDataManager>.GetInstance().has_exchangeable_subcoin = false;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets[] playerCommonTickets1 = handler.Result.exchange_player_common_tickets;
    IOrderedEnumerable<WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets> sort_tickets = ((IEnumerable<WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets>) playerCommonTickets1).OrderBy<WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets, DateTime>((Func<WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets, DateTime>) (x => MasterData.CommonTicketEndAt[x.ticket_id].end_at)).ThenBy<WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets, int>((Func<WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets, int>) (x => x.ticket_id));
    if (playerCommonTickets1.Length != 0)
    {
      int coin = SMManager.Get<Player>().coin;
      int coinInBattleHere = PurchaseBehavior.UsedCoinInBattleHere;
      Future<GameObject> prefabF = new ResourceObject("Prefabs/shop007_CoinExchange/popup_CoinExchange_Expired").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      foreach (WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets playerCommonTickets2 in (IEnumerable<WebAPI.Response.HomeExchangeSubCoinExchange_player_common_tickets>) sort_tickets)
      {
        if (playerCommonTickets2.gain_coin > 0)
        {
          GameObject prefab = prefabF.Result.Clone();
          GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
          mypageScene.StartCoroutine(gameObject.GetComponent<PopupCoinExchangeExpired>().Init(playerCommonTickets2.ticket_id, playerCommonTickets2.ticket_quantity, playerCommonTickets2.gain_coin));
          while (Singleton<PopupManager>.GetInstance().isOpen)
            yield return (object) null;
        }
      }
      prefabF = (Future<GameObject>) null;
    }
  }

  private IEnumerator doCoinEndAtDayAfterTomorrowPopup(int executed)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    yield return (object) null;
    Future<GameObject> prefab = new ResourceObject("Prefabs/mypage/popup_CoinLimit").Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(prefab.Result).GetComponent<PopupCoinLimit>().Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    Singleton<NGGameDataManager>.GetInstance().has_near_dead_subcoin = false;
  }

  private IEnumerator doShowGuildRaidRankingResult(int executed)
  {
    Singleton<NGGameDataManager>.GetInstance().refreshGuildTop = true;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    WebAPI.Response.GuildTop guildTopResponse = this.Menu.GuildTopResponse;
    int period_id = guildTopResponse.raid_rank_period_id.Value;
    long pt_damage = guildTopResponse.raid_total_damage.HasValue ? guildTopResponse.raid_total_damage.Value : 0L;
    int ranking = guildTopResponse.raid_damage_rank.Value;
    WebAPI.Response.GuildTopRaid_guild_ranking_rewards[] raid_guild_ranking_rewards = guildTopResponse.raid_guild_ranking_rewards;
    WebAPI.Response.GuildTopRaid_guild_ranking_guild_rewards[] raid_guild_ranking_rewardsExtra = guildTopResponse.raid_guild_ranking_guild_rewards;
    GuildRaidRankingRewardPopupSequence rankingRewardPopupSeq = new GuildRaidRankingRewardPopupSequence();
    IEnumerator e = rankingRewardPopupSeq.Init(period_id, ranking, guildTopResponse.player_affiliation.guild.guild_name, pt_damage);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (raid_guild_ranking_rewards != null)
    {
      foreach (WebAPI.Response.GuildTopRaid_guild_ranking_rewards guildRankingRewards in raid_guild_ranking_rewards)
      {
        if (guildRankingRewards.rewards != null)
        {
          foreach (WebAPI.Response.GuildTopRaid_guild_ranking_rewardsRewards reward in guildRankingRewards.rewards)
            rankingRewardPopupSeq.addRewardData(guildRankingRewards.condition_id, (MasterDataTable.CommonRewardType) reward.reward_type_id, reward.reward_id, reward.reward_quantity);
        }
      }
    }
    if (raid_guild_ranking_rewardsExtra != null)
    {
      foreach (WebAPI.Response.GuildTopRaid_guild_ranking_guild_rewards rankingGuildRewards in raid_guild_ranking_rewardsExtra)
      {
        if (rankingGuildRewards.rewards != null)
        {
          foreach (GuildRankingGuildReward reward in rankingGuildRewards.rewards)
            rankingRewardPopupSeq.addRewardData(rankingGuildRewards.condition_id, GuildUtil.getCommonRewardType(reward.reward_type), reward.reward_id, reward.reward_quantity, true);
        }
      }
    }
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    yield return (object) rankingRewardPopupSeq.Run();
  }

  public bool IsGuildBattleEntry()
  {
    GuildRegistration guild = PlayerAffiliation.Current.guild;
    if (guild.gvg_status != GvgStatus.can_entry)
      return false;
    Persist.GuildBattleUser data;
    try
    {
      data = Persist.guildBattleUser.Data;
    }
    catch
    {
      Persist.guildBattleUser.Delete();
      Persist.guildBattleUser.Data = new Persist.GuildBattleUser();
      Persist.guildBattleUser.Flush();
      data = Persist.guildBattleUser.Data;
    }
    GuildMembership guildMembership = Array.Find<GuildMembership>(guild.memberships, (Predicate<GuildMembership>) (x => x.player.player_id == Player.Current.id));
    if (guildMembership == null || guildMembership.role != GuildRole.master && guildMembership.role != GuildRole.sub_master)
      return false;
    int gvgId = guild.active_gvg_period_id.HasValue ? guild.active_gvg_period_id.Value : 0;
    if (data.guildID != guild.guild_id || data.roleNo != guildMembership._role || data.gvgID != gvgId || data.gvgCount != guild.gvg_count)
      data.reset(guild.guild_id, guildMembership._role, gvgId, guild.gvg_count);
    if (data.countTopIN > 0)
      return false;
    ++data.countTopIN;
    Persist.guildBattleUser.Flush();
    return true;
  }

  private IEnumerator doShowGuildBattleEntry(int executed)
  {
    Future<GameObject> ft = Res.Prefabs.popup.popup_028_guild_battle_entry__anim_popup01.Load<GameObject>();
    IEnumerator e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) ft.Result, (Object) null))
    {
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Singleton<PopupManager>.GetInstance().open(ft.Result);
      while (Singleton<PopupManager>.GetInstance().isOpen)
        yield return (object) null;
    }
  }

  private IEnumerator doShowGuildBattleResult(int executed)
  {
    Singleton<NGGameDataManager>.GetInstance().refreshGuildTop = true;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    Future<WebAPI.Response.GvgResult> webApi = WebAPI.GvgResult((Action<WebAPI.Response.UserError>) (e => Debug.LogError((object) ("MypageTopEvent Failed Load GuildBattleResult : " + e.Code))));
    IEnumerator e1 = webApi.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.GvgResult gvgResult = webApi.Result;
    if (gvgResult == null)
    {
      this.IntroControler.clear();
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      GameObject gvgResultObj = (GameObject) null;
      Future<GameObject> f;
      switch (gvgResult.score.battle_status)
      {
        case GvgBattleStatus.win:
          f = Res.Prefabs.guild.dir_guild_GB_result_win.Load<GameObject>();
          e1 = f.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          gvgResultObj = f.Result;
          break;
        case GvgBattleStatus.lose:
          f = Res.Prefabs.guild.dir_guild_GB_result_lose.Load<GameObject>();
          e1 = f.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          gvgResultObj = f.Result;
          break;
        case GvgBattleStatus.draw:
          f = Res.Prefabs.guild.dir_guild_GB_result_draw.Load<GameObject>();
          e1 = f.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          gvgResultObj = f.Result;
          break;
      }
      Future<GameObject> ftgResultAnim = Res.Prefabs.guild.dir_guild_GB_result_anim.Load<GameObject>();
      e1 = ftgResultAnim.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject gvgResultAnim = ftgResultAnim.Result;
      Future<GameObject> ftResultDetail = Res.Prefabs.guild.dir_guild_GB_result_detail.Load<GameObject>();
      e1 = ftResultDetail.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject gvgResultDetail = ftResultDetail.Result;
      Future<GameObject> ftGuildReward = Res.Prefabs.guild.dir_guild_GB_result_guild_reward.Load<GameObject>();
      e1 = ftGuildReward.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject gvgResultGuildReward = ftGuildReward.Result;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Singleton<PopupManager>.GetInstance().open((GameObject) null);
      GameObject clone = gvgResultAnim.Clone();
      clone.SetActive(false);
      e1 = clone.GetComponent<GuildGBResultAnim>().InitializeAsync(gvgResult);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      clone.GetComponent<GuildGBResultAnim>().Initialize(gvgResult, gvgResultObj);
      clone.SetActive(true);
      GameObject obj = Singleton<PopupManager>.GetInstance().open(clone, isCloned: true);
      while (Object.op_Inequality((Object) obj, (Object) null))
        yield return (object) null;
      obj = Singleton<PopupManager>.GetInstance().open(gvgResultDetail);
      obj.GetComponent<GuildGBResultDetail>().Initialize(gvgResult);
      while (Object.op_Inequality((Object) obj, (Object) null))
        yield return (object) null;
      clone = gvgResultGuildReward.Clone();
      clone.SetActive(false);
      e1 = clone.GetComponent<GuildGBResultReward>().Initialize(gvgResult);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      clone.SetActive(true);
      clone.GetComponent<GuildGBResultReward>().PositionReset();
      obj = Singleton<PopupManager>.GetInstance().open(clone, isCloned: true);
      while (Object.op_Inequality((Object) obj, (Object) null))
        yield return (object) null;
      Singleton<PopupManager>.GetInstance().dismiss();
    }
  }

  private IEnumerator ProcResumeGvg(int executed)
  {
    Consts consts = Consts.GetInstance();
    bool? restartGvg = new bool?();
    PlayerAffiliation current = PlayerAffiliation.Current;
    if ((current != null ? (current.isGuildMember() ? 1 : 0) : 0) != 0)
    {
      switch (PlayerAffiliation.Current.guild.gvg_status)
      {
        case GvgStatus.fighting:
        case GvgStatus.aggregating:
          bool showPopup;
          Action varificationProc = (Action) (() => ModalWindow.ShowYesNo(consts.POPUP_GUILD_BATTLE_NOT_RESUME_TITLE, consts.POPUP_GUILD_BATTLE_NOT_RESUME_DESC, (Action) (() =>
          {
            restartGvg = new bool?(false);
            showPopup = false;
          }), (Action) (() => showPopup = false)));
          while (!restartGvg.HasValue)
          {
            showPopup = true;
            ModalWindow.ShowYesNo(consts.POPUP_GUILD_BATTLE_RESUME_TITLE, consts.POPUP_GUILD_BATTLE_RESUME_DESC, (Action) (() =>
            {
              restartGvg = new bool?(true);
              showPopup = false;
            }), varificationProc);
            while (showPopup)
              yield return (object) null;
          }
          yield return (object) null;
          varificationProc = (Action) null;
          break;
      }
    }
    IEnumerator e;
    if (!restartGvg.HasValue)
      Persist.gvgBattleEnvironment.Delete();
    else if (restartGvg.Value)
    {
      e = this.ResumeGvg();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.ForceCloseGvg();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator ResumeGvg()
  {
    string errorCode = string.Empty;
    Future<WebAPI.Response.GvgBattleResume> ft = WebAPI.GvgBattleResume((Action<WebAPI.Response.UserError>) (e => errorCode = e.Code));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (ft.Result == null)
    {
      if (errorCode.Equals("GVG001"))
        ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_BATTLE_FINISH, (Action) (() =>
        {
          Persist.gvgBattleEnvironment.Delete();
          this.IntroControler.clear();
          MypageScene.ChangeSceneOnError();
        }));
      else if (errorCode.Equals("GVG006"))
        ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_BATTLE_EXPIRED, (Action) (() =>
        {
          Persist.gvgBattleEnvironment.Delete();
          this.IntroControler.clear();
          MypageScene.ChangeSceneOnError();
        }));
      else if (errorCode.Equals("GLD014"))
        ModalWindow.Show("GLD014", Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_BATTLE_EXPIRED, (Action) (() => { }));
      else
        ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_RESUME_ERROR_MESSAGE, (Action) (() =>
        {
          Persist.gvgBattleEnvironment.Delete();
          this.IntroControler.clear();
          MypageScene.ChangeSceneOnError();
        }));
      while (Singleton<PopupManager>.GetInstance().ModalWindowIsOpen)
        yield return (object) null;
    }
    else
    {
      GuildUtil.gvgBattleIDServer = ft.Result.battle_uuid;
      BattleInfo battleInfo = new BattleInfo()
      {
        gvg = true,
        pvp_restart = true
      };
      Singleton<NGBattleManager>.GetInstance().startBattle(battleInfo);
    }
  }

  private IEnumerator ForceCloseGvg()
  {
    string battleId = GuildUtil.gvgBattleIDLocal;
    Persist.gvgBattleEnvironment.Delete();
    IEnumerator e1;
    if (string.IsNullOrEmpty(battleId))
    {
      Future<WebAPI.Response.GvgBattleResume> wabApi = WebAPI.GvgBattleResume((Action<WebAPI.Response.UserError>) (e => Debug.LogError((object) ("MypageTopEvent Failed GvgBattleResume : " + e.Code))));
      e1 = wabApi.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (wabApi.Result != null)
        battleId = wabApi.Result.battle_uuid;
      wabApi = (Future<WebAPI.Response.GvgBattleResume>) null;
    }
    if (battleId.Equals(string.Empty))
    {
      ModalWindow.Show(Consts.GetInstance().POPUP_GUILD_BATTLE_CLOSE_ERROR_TITLE, Consts.GetInstance().POPUP_GUILD_BATTLE_CLOSE_ERROR_MESSAGE, (Action) (() => { }));
      while (Singleton<PopupManager>.GetInstance().ModalWindowIsOpen)
        yield return (object) null;
    }
    else if (!battleId.Equals("GVG006") && !battleId.Equals("GVG001") && !battleId.Equals("GLD014"))
    {
      e1 = WebAPI.GvgBattleForceClose(battleId, (Action<WebAPI.Response.UserError>) (e => Debug.LogError((object) ("MypageTopEvent Failed Close GuildBattle : " + e.Code)))).Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  private IEnumerator doShowGuildCheckin(int executed)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(2);
    Future<WebAPI.Response.GuildCheckin> webapi = WebAPI.GuildCheckin((Action<WebAPI.Response.UserError>) (e => Debug.LogError((object) ("MypageTopEvent Failed GuildCheckin : " + e.Code))));
    IEnumerator e1 = webapi.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (webapi.Result != null)
    {
      Singleton<NGGameDataManager>.GetInstance().HasReceivableGuildCheckIn = false;
      Future<GameObject> ft = new ResourceObject("Prefabs/guild/guild_CheckIn_anim").Load<GameObject>();
      e1 = ft.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Singleton<PopupManager>.GetInstance().open(ft.Result).GetComponent<PopupGuildCheckIn>().Initialize(webapi.Result);
    }
  }
}
