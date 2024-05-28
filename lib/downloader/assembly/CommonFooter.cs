// Decompiled with JetBrains decompiler
// Type: CommonFooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class CommonFooter : CommonFooterBase
{
  private GameObject coming_soon_Popup;
  [SerializeField]
  private GameObject footerMissionBadge;
  [SerializeField]
  private GameObject mypageMenuPopup;
  [SerializeField]
  private GameObject dirSeaButtonRoot;
  [SerializeField]
  private UIButton seaHomeButton;
  [SerializeField]
  private UIButton seaQuestButton;
  [SerializeField]
  private UIButton seaDeckButton;
  [SerializeField]
  private UIButton seaAlbumButton;
  [SerializeField]
  private GameObject badgeSea;
  [SerializeField]
  private GameObject badgeTalkSea;
  [SerializeField]
  private GameObject badgeTalkSea2;
  [SerializeField]
  private int L_id;
  [SerializeField]
  private bool hardMode;
  [SerializeField]
  private UIButton colosseumButton;
  [SerializeField]
  private GameObject colosseumConditoin;
  [SerializeField]
  private GameObject colosseumCampaign;
  [SerializeField]
  private GameObject pvpCampaign;
  [SerializeField]
  private UIButton exploreButton;
  [SerializeField]
  private GameObject baseHome;
  [SerializeField]
  private GameObject baseOther;
  [SerializeField]
  private GameObject questMenuPopup;
  private NGTweenParts _tp_base_home;
  private NGTweenParts _tp_base_other;
  private GameObject unitMenu;
  private Modified<Player> modifiedPlayer;
  private Modified<SM.GuildSignal> modifiedGuildSignal;
  private bool isOpenPopup;

  public bool IsActiveMyPageMenuPopup => this.mypageMenuPopup.activeSelf;

  private void Awake()
  {
    this._tp_base_home = this.baseHome.GetComponent<NGTweenParts>();
    this._tp_base_other = this.baseOther.GetComponent<NGTweenParts>();
  }

  private void Start()
  {
    this.modifiedGuildSignal = SMManager.Observe<SM.GuildSignal>();
    this.baseHome.SetActive(false);
    this.baseOther.SetActive(true);
    if (!Object.op_Implicit((Object) this.unitMenu))
      this.StartCoroutine(this.InitUnitMenuPrefab());
    Singleton<NGSceneManager>.GetInstance().act = (Action) (() =>
    {
      this.mypageMenuPopup.SetActive(false);
      if (!this.unitMenu.activeSelf)
        return;
      this.unitMenu.GetComponent<UnitMenu>().Close();
    });
  }

  private IEnumerator InitUnitMenuPrefab()
  {
    Future<GameObject> unit_menu = Res.Prefabs.TopScene.dir_unit_memu.Load<GameObject>();
    IEnumerator e = unit_menu.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitMenu = unit_menu.Result.Clone(Singleton<CommonRoot>.GetInstance().unitMenuParent.transform);
    this.unitMenu.SetActive(false);
  }

  public void ActiveHomeFooter(bool active)
  {
    this._tp_base_home.forceActive(active);
    this._tp_base_other.forceActive(!active);
  }

  public void CloseSubMenu()
  {
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (Object.op_Inequality((Object) this.unitMenu, (Object) null) && this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    if (!this.isOpenPopup)
      return;
    Singleton<PopupManager>.GetInstance().dismissWithoutAnim();
    this.IsPush = false;
    this.isOpenPopup = false;
  }

  public void onButtonMypageMenu()
  {
    if (this.mypageMenuPopup.activeSelf || !Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush)
      return;
    this.mypageMenuPopup.GetComponent<CommonFooterMenuAnimation>().OpenMenu();
    if (!this.modifiedPlayer.Value.IsSea())
      return;
    this.dirSeaButtonRoot.GetComponent<CommonFooterMenuAnimation>().OpenMenu();
  }

  public void onButtonMypageClose()
  {
    if (!this.mypageMenuPopup.activeSelf || !Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush)
      return;
    CommonFooterMenuAnimation component = this.mypageMenuPopup.GetComponent<CommonFooterMenuAnimation>();
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1004");
    component.CloseMenu();
    if (!this.modifiedPlayer.Value.IsSea())
      return;
    this.dirSeaButtonRoot.GetComponent<CommonFooterMenuAnimation>().CloseMenu();
  }

  private void SetTweenAlpha(TweenAlpha alpha, float from, float end, bool isOpen)
  {
    ((UITweener) alpha).ResetToBeginning();
    ((UITweener) alpha).duration = 0.25f;
    alpha.from = from;
    alpha.to = end;
    ((UITweener) alpha).onFinished.Clear();
    if (!isOpen)
      ((UITweener) alpha).onFinished.Add(new EventDelegate((EventDelegate.Callback) (() => this.mypageMenuPopup.SetActive(false))));
    ((UITweener) alpha).PlayForward();
  }

  public void onButtonOpenMypage()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush || !Singleton<NGSceneManager>.GetInstance().isSceneInitialized || Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount > 0)
      return;
    this.pvpCampaign.SetActive(Singleton<NGGameDataManager>.GetInstance().IsOpenPvpCampaign);
    this.questMenuPopup.SetActive(false);
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    Singleton<CommonRoot>.GetInstance().guildChatManager.CloseDetailedChat();
    if (Singleton<CommonRoot>.GetInstance().DailyMissionController.IsOpened)
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    CommonFooterMenuAnimation component = this.mypageMenuPopup.GetComponent<CommonFooterMenuAnimation>();
    if (this.mypageMenuPopup.activeSelf)
    {
      Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1004");
      component.CloseMenu();
      if (this.modifiedPlayer.Value.IsSea())
        this.dirSeaButtonRoot.GetComponent<CommonFooterMenuAnimation>().CloseMenu();
      this.mypageMenuPopup.SetActive(true);
    }
    else
    {
      Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1002");
      this.mypageMenuPopup.SetActive(true);
      component.OpenMenu();
      GuildUtil.GuildBadgeLabelType labelType = GuildUtil.GuildBadgeLabelType.none;
      if (PlayerAffiliation.Current != null && PlayerAffiliation.Current.guild != null)
      {
        switch (PlayerAffiliation.Current.guild.gvg_status)
        {
          case GvgStatus.can_entry:
            labelType = GuildUtil.GuildBadgeLabelType.entry;
            break;
          case GvgStatus.matching:
            labelType = GuildUtil.GuildBadgeLabelType.matching;
            break;
          case GvgStatus.preparing:
            labelType = GuildUtil.GuildBadgeLabelType.prepare;
            break;
          case GvgStatus.fighting:
            labelType = GuildUtil.GuildBadgeLabelType.battle;
            break;
        }
      }
      Singleton<CommonRoot>.GetInstance().SetGuildFooterBadgeLabel(labelType);
      if (!this.modifiedPlayer.Value.IsSea())
        return;
      this.dirSeaButtonRoot.GetComponent<CommonFooterMenuAnimation>().OpenMenu();
    }
  }

  public void onButtonHome()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.preChangeScene(true);
    if (Object.op_Inequality((Object) Singleton<ExploreSceneManager>.GetInstanceOrNull(), (Object) null))
      this.StartCoroutine(this.doHomeButtonOnExploreScene());
    else
      MypageScene.ChangeScene(MypageRootMenu.Mode.STORY);
  }

  private IEnumerator doHomeButtonOnExploreScene()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) Singleton<ExploreDataManager>.GetInstance().SaveSuspendData((Action) (() => { }));
    Singleton<ExploreSceneManager>.GetInstance().SetReloadDirty();
    MypageScene.ChangeScene(MypageRootMenu.Mode.STORY);
  }

  public void onButtonOpenChat()
  {
    if (!Singleton<CommonRoot>.GetInstance().GetFooterEnable() || Singleton<NGGameDataManager>.GetInstance().IsEarth)
      return;
    this.mypageMenuPopup.SetActive(false);
    if (Singleton<NGSceneManager>.GetInstance().sceneBase.currentSceneGuildChatDisplayingStatus == NGSceneBase.GuildChatDisplayingStatus.Closed)
    {
      Singleton<CommonRoot>.GetInstance().guildChatManager.OpenFooterGuildChat();
      this.ActiveHomeFooter(false);
    }
    else
      Singleton<CommonRoot>.GetInstance().guildChatManager.OpenDetailedChat();
  }

  public void onButtonOpenQuest()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush || !Singleton<NGSceneManager>.GetInstance().isSceneInitialized || Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount > 0)
      return;
    Singleton<CommonRoot>.GetInstance().guildChatManager.CloseDetailedChat();
    if (Singleton<CommonRoot>.GetInstance().DailyMissionController.IsOpened)
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    if (this.questMenuPopup.activeSelf)
    {
      this.onButtonQuestClose();
    }
    else
    {
      Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1002");
      this.mypageMenuPopup.SetActive(false);
      if (this.unitMenu.activeSelf)
        this.unitMenu.GetComponent<UnitMenu>().Close();
      this.questMenuPopup.GetComponent<CommonFooterMenuAnimation>().OpenMenu();
    }
    Singleton<NGMessageUI>.GetInstance().TurnOff();
  }

  public void onButtonQuestClose()
  {
    if (!this.questMenuPopup.activeSelf || !Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush)
      return;
    CommonFooterMenuAnimation component = this.questMenuPopup.GetComponent<CommonFooterMenuAnimation>();
    Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1004");
    component.CloseMenu();
  }

  public void onButtonOpenSeaMypage()
  {
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized || this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.preChangeScene(true);
    Sea030HomeScene.ChangeScene(false);
  }

  public void onButtonOpenGroundMypage()
  {
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized || this.IsPushAndSet())
      return;
    this.mypageMenuPopup.SetActive(false);
    this.preChangeScene(true);
    MypageScene sceneBase = Singleton<NGSceneManager>.GetInstance().sceneBase as MypageScene;
    if (Object.op_Implicit((Object) sceneBase))
      sceneBase.onStartEarthCloud();
    else
      EarthDataManager.startEarthScene((MonoBehaviour) this);
  }

  public void onButtonOpenSeaQuest()
  {
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized || this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    string pattern = "sea030_quest";
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != pattern) && Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    int num = Singleton<NGSceneManager>.GetInstance().isMatchSceneNameInStack(pattern) ? 1 : 0;
    this.preChangeScene(true);
    if (num != 0)
      this.StartCoroutine(this.doWaitDestroyLoadedScenes((Action) (() => Sea030_questScene.ChangeScene(false))));
    else
      Sea030_questScene.ChangeScene(false);
  }

  private IEnumerator doWaitDestroyLoadedScenes(Action actFinish)
  {
    yield return (object) Singleton<NGSceneManager>.GetInstance().destroyLoadedScenesImmediate();
    actFinish();
  }

  public void onButtonSeaAlbum()
  {
    if (this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    string str = "sea030_album";
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Sea030AlbumScene.ChangeScene(false);
  }

  public void onButtonSeaDeck()
  {
    if (this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    string str = "unit004_6_0822_sea";
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = true;
    Singleton<NGGameDataManager>.GetInstance().QuestType = new CommonQuestType?();
    Unit0046Scene.changeScene(false, isFromMypage: true);
  }

  public void onButtonOpenEventQuest()
  {
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized || this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    string str = "quest002_17";
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Quest00217Scene.ChangeScene(false);
  }

  public void onButtonOpenSideStory()
  {
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized || this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    string str = "quest002_SideStory_List";
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Quest002SideStoryScene.ChangeScene(false);
  }

  public void onButtonUnit()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() && Singleton<NGSceneManager>.GetInstance().sceneName == "unit004_9_8" || this.IsPush || !Singleton<NGSceneManager>.GetInstance().isSceneInitialized || Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount > 0)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (Singleton<CommonRoot>.GetInstance().DailyMissionController.IsOpened)
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    if (Object.op_Inequality((Object) Singleton<CommonRoot>.GetInstance().GetSeaHeaderComponent(), (Object) null))
      Singleton<CommonRoot>.GetInstance().GetSeaHeaderComponent().CloseMenu();
    if (!Object.op_Implicit((Object) this.unitMenu))
      this.StartCoroutine(this.InitUnitMenuPrefab());
    if (!this.unitMenu.activeSelf)
    {
      this.unitMenu.SetActive(true);
      this.unitMenu.GetComponent<UnitMenu>().Open();
    }
    else
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
  }

  public void onButtonWeapon()
  {
    string name = "bugu005_1";
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != name) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    this.changeScene(name, false);
  }

  public void onButtonGacha()
  {
    string str = "gacha006_3";
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush || !Singleton<NGSceneManager>.GetInstance().isSceneInitialized || Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount > 0)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (Singleton<CommonRoot>.GetInstance().DailyMissionController.IsOpened)
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(clearStackName: str);
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Singleton<NGSceneManager>.GetInstance().changeScene(str, false);
  }

  public void onButtonShop()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush || !Singleton<NGSceneManager>.GetInstance().isSceneInitialized || Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount > 0)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (Singleton<CommonRoot>.GetInstance().DailyMissionController.IsOpened)
      Singleton<CommonRoot>.GetInstance().DailyMissionController.Hide();
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    string str = "shop007_Top";
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    ShopTopScene.ChangeScene(false);
  }

  public void onButtonMission()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush || !Singleton<NGSceneManager>.GetInstance().isSceneInitialized || Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount > 0)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    Singleton<CommonRoot>.GetInstance().guildChatManager.CloseDetailedChat();
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    Singleton<CommonRoot>.GetInstance().DailyMissionController.Show();
  }

  public void onButtonMulti()
  {
    if (this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    string str = "versus026_1";
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Versus0261Scene.ChangeScene0261(false);
  }

  public void onButtonCharacter()
  {
    if (this.IsPush)
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    string str = "quest002_14";
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Quest00214Scene.ChangeScene(false);
  }

  public void enableExploreButton(bool enable)
  {
    ((UIButtonColor) this.exploreButton).isEnabled = enable;
  }

  public void onButtonExplore()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush || Object.op_Implicit((Object) Singleton<ExploreSceneManager>.GetInstanceOrNull()))
      return;
    this.mypageMenuPopup.SetActive(false);
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.destoryNonStackScenes();
    instance.destroyCurrentScene();
    if (Singleton<NGGameDataManager>.GetInstance().isEditCustomDeck)
      instance.quePreSceneChangeAsync.Enqueue(new Func<IEnumerator>(this.doFinalizeCustomDeck));
    Explore033TopScene.changeScene();
  }

  public void onButtonQuest()
  {
    if (this.IsPushAndSet())
      return;
    this.mypageMenuPopup.SetActive(false);
    this.questMenuPopup.SetActive(false);
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (Object.op_Inequality((Object) instance, (Object) null))
      instance.playSE("SE_1002");
    PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
    if (source != null && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_xl_QuestStoryXL == 4)))
      this.StartCoroutine(this.openStorySelectPopup());
    else
      Debug.LogError((object) "quests data is null or not exists LostRagnarok");
  }

  public void onButtonColosseum()
  {
    if (this.IsPush)
      return;
    PlayerUnit[] source = SMManager.Get<PlayerUnit[]>();
    if (((IEnumerable<PlayerUnit>) source).Count<PlayerUnit>() < 3)
    {
      this.StartCoroutine(this.openPopup008161UnitInsufficiency());
    }
    else
    {
      Player player = SMManager.Get<Player>();
      PlayerUnit[] array = ((IEnumerable<PlayerUnit>) source).OrderBy<PlayerUnit, int>((Func<PlayerUnit, int>) (x => x.cost)).ToArray<PlayerUnit>();
      int num = 0;
      for (int index = 0; index < 3; ++index)
        num += array[index].cost;
      if (player.max_cost >= num)
      {
        string str = "colosseum023_4";
        this.mypageMenuPopup.SetActive(false);
        if (!(Singleton<NGSceneManager>.GetInstance().sceneName != str) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
          return;
        this.preChangeScene(true);
        Singleton<NGGameDataManager>.GetInstance().IsSea = false;
        Colosseum0234Scene.ChangeScene(true);
      }
      else
        this.StartCoroutine(this.openPopup008161CostInsufficiency());
    }
  }

  public void onButtonInfo()
  {
    if (this.IsPush)
      return;
    string sceneName = "mypage001_8_1";
    this.mypageMenuPopup.SetActive(false);
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != sceneName) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, false);
  }

  public void onButtonPanelMission()
  {
    if (this.IsPush)
      return;
    string sceneName = "dailymission027_1";
    this.mypageMenuPopup.SetActive(false);
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != sceneName) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, false);
  }

  public void onButtonPresent()
  {
    if (this.IsPush)
      return;
    string sceneName = "mypage001_7";
    this.mypageMenuPopup.SetActive(false);
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != sceneName) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, false);
  }

  private IEnumerator openPopup008161UnitInsufficiency()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_16_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    GameObject pObj = Singleton<PopupManager>.GetInstance().open(result);
    pObj.SetActive(false);
    e = pObj.GetComponent<Friend008161Menu>().Init(Consts.GetInstance().COLOSSEUM_ALERT_TITLE_UNIT, Consts.GetInstance().COLOSSEUM_ALERT_MESSAGE_UNIT);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    pObj.SetActive(true);
  }

  private IEnumerator openPopup008161CostInsufficiency()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_16_1__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    GameObject pObj = Singleton<PopupManager>.GetInstance().open(result);
    pObj.SetActive(false);
    e = pObj.GetComponent<Friend008161Menu>().Init(Consts.GetInstance().COLOSSEUM_ALERT_TITLE_COST, Consts.GetInstance().COLOSSEUM_ALERT_MESSAGE_COST);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    pObj.SetActive(true);
  }

  public void onButtonMenu()
  {
    if (this.IsPush)
      return;
    string sceneName = "story001_9_1";
    this.mypageMenuPopup.SetActive(false);
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != sceneName) && !Singleton<NGGameDataManager>.GetInstance().IsSea)
      return;
    this.preChangeScene(true);
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    Singleton<NGSceneManager>.GetInstance().changeScene(sceneName, false);
  }

  public void onButtonGuild()
  {
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish() || this.IsPush || !Singleton<NGSceneManager>.GetInstance().isSceneInitialized || Singleton<NGSceneManager>.GetInstance().changeSceneQueueCount > 0)
      return;
    if (PlayerAffiliation.Current.guild != null)
    {
      WebAPI.Response.GuildTop guildTopResponse = Singleton<NGGameDataManager>.GetInstance().GuildTopResponse;
      if (guildTopResponse != null && PlayerAffiliation.Current.isGuildMember() && !Persist.guildEventCheck.Data.isGuildRaidTransition && guildTopResponse.raid_period != null)
      {
        Persist.guildEventCheck.Data.isGuildRaidTransition = true;
        Persist.guildEventCheck.Flush();
        string str = "raid_top";
        bool isStack = this.preChangeScene(clearStackName: str);
        this.mypageMenuPopup.SetActive(false);
        Singleton<NGSceneManager>.GetInstance().changeScene(str, isStack);
        return;
      }
      if (PlayerAffiliation.Current.onGvgTransition && !Persist.guildEventCheck.Data.isGuildBattleTransition)
      {
        Persist.guildEventCheck.Data.isGuildBattleTransition = true;
        Persist.guildEventCheck.Flush();
        string str = "guild028_2";
        bool isStack = this.preChangeScene(clearStackName: str);
        this.mypageMenuPopup.SetActive(false);
        Singleton<NGSceneManager>.GetInstance().changeScene(str, isStack);
        return;
      }
    }
    this.mypageMenuPopup.SetActive(false);
    if (this.unitMenu.activeSelf)
      this.unitMenu.GetComponent<UnitMenu>().Close();
    Singleton<NGMessageUI>.GetInstance().TurnOff();
    this.preChangeScene(true);
    MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD);
  }

  private bool preChangeScene(bool clearSceneStackAll = false, string clearStackName = null)
  {
    NGSceneManager instance1 = Singleton<NGSceneManager>.GetInstance();
    bool flag1 = false;
    if (instance1.sceneName == "mypage")
    {
      flag1 = true;
      instance1.destroyCurrentScene();
    }
    if (!string.IsNullOrEmpty(clearStackName))
    {
      if (instance1.sceneName == clearStackName)
      {
        flag1 = true;
        instance1.destroyCurrentScene();
      }
      if (instance1.clearStack(clearStackName) > 0)
        flag1 = true;
    }
    bool flag2 = false;
    NGGameDataManager instance2 = Singleton<NGGameDataManager>.GetInstance();
    instance2.lastReferenceUnitID = -1;
    instance2.lastReferenceUnitIndex = -1;
    instance2.clearScenePopupRecovery();
    NGSceneBase sceneBase = instance1.sceneBase;
    if (Object.op_Inequality((Object) sceneBase, (Object) null))
      sceneBase.IsPush = true;
    if (clearSceneStackAll)
    {
      instance1.destroyLoadedScenes();
    }
    else
    {
      flag2 = instance1.clearStackBeforeTopGlobalBack();
      instance1.destoryNonStackScenes();
    }
    if (instance2.isEditCustomDeck)
      instance1.quePreSceneChangeAsync.Enqueue(new Func<IEnumerator>(this.doFinalizeCustomDeck));
    return !flag1 && flag2;
  }

  private IEnumerator doFinalizeCustomDeck()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    return Singleton<NGGameDataManager>.GetInstance().doFinalizeEditCustomDeck();
  }

  private void Update()
  {
    if (this.modifiedPlayer == null)
    {
      this.modifiedPlayer = SMManager.Observe<Player>();
      this.modifiedPlayer.NotifyChanged();
    }
    if (this.modifiedPlayer.IsChangedOnce())
    {
      this.footerMissionBadge.SetActive(this.modifiedPlayer.Value.is_open_mission);
      bool flag = this.modifiedPlayer.Value.IsSea();
      ((UIButtonColor) this.seaHomeButton).isEnabled = flag;
      ((UIButtonColor) this.seaQuestButton).isEnabled = flag;
      ((UIButtonColor) this.seaAlbumButton).isEnabled = flag;
      ((UIButtonColor) this.seaDeckButton).isEnabled = flag;
      this.badgeSea.SetActive(flag);
      this.dirSeaButtonRoot.SetActive(flag);
      this.SetColosseumButtonActive(this.modifiedPlayer.Value.IsColosseum());
    }
    SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
    if (seaPlayer != null)
    {
      this.badgeTalkSea.SetActive(seaPlayer.is_released_sea_call && Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage);
      this.badgeTalkSea2.SetActive(seaPlayer.is_released_sea_call && Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage);
    }
    else
    {
      this.badgeTalkSea.SetActive(false);
      this.badgeTalkSea2.SetActive(false);
    }
    if (Singleton<NGGameDataManager>.GetInstance().IsEarth || this.modifiedGuildSignal == null || !this.modifiedGuildSignal.IsChangedOnce())
      return;
    SM.GuildSignal current = SM.GuildSignal.Current;
    if (current == null)
      return;
    if (current.existGvgEvent(GuildEventType.change_map_info))
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      if (instance.sceneName == "guild028_2" && Object.op_Inequality((Object) instance.sceneBase, (Object) null))
      {
        Guild0282Menu component = ((Component) instance.sceneBase).GetComponent<Guild0282Menu>();
        if (Object.op_Inequality((Object) component, (Object) null))
        {
          GuildEventGvg currentGvgEvent = current.getCurrentGvgEvent(GuildEventType.change_map_info);
          PlayerAffiliation.Current.guild = currentGvgEvent.guild;
          this.StartCoroutine(component.MapReload(currentGvgEvent));
        }
      }
    }
    this.SetGuildBadge();
  }

  private void SetColosseumButtonActive(bool isColloseumOpen)
  {
    ((UIButtonColor) this.colosseumButton).isEnabled = isColloseumOpen;
    this.colosseumConditoin.SetActive(!isColloseumOpen);
    this.colosseumCampaign.SetActive(isColloseumOpen && Singleton<NGGameDataManager>.GetInstance().IsOpenColosseumCampaign);
  }

  private void SetGuildBadge()
  {
    PlayerAffiliation current1 = PlayerAffiliation.Current;
    SM.GuildSignal current2 = SM.GuildSignal.Current;
    if (current1 == null || current2 == null)
      return;
    bool flag1 = false;
    if (!Persist.guildSetting.Exists)
    {
      Persist.guildSetting.Data.reset();
      Persist.guildSetting.Flush();
    }
    if (!Persist.guildSetting.Exists)
      return;
    if (!current1.isGuildMember())
    {
      Persist.gvgBattleEnvironment.Delete();
    }
    else
    {
      NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
      bool flag2 = instance.sceneName.Equals("guild028_1") || instance.sceneName.Equals("guild028_3") || instance.sceneName.Equals("raid_top");
      bool flag3 = Object.op_Inequality((Object) instance.sceneBase, (Object) null) && instance.sceneBase.currentSceneGuildChatDisplayingStatus == NGSceneBase.GuildChatDisplayingStatus.Opened;
      GuildRole? role = current1.role;
      GuildRole guildRole1 = GuildRole.master;
      if (!(role.GetValueOrDefault() == guildRole1 & role.HasValue))
      {
        role = current1.role;
        GuildRole guildRole2 = GuildRole.sub_master;
        if (!(role.GetValueOrDefault() == guildRole2 & role.HasValue))
        {
          GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant, false);
          GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newMember, false);
          goto label_15;
        }
      }
      if (current2.existNewApplicant())
      {
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant, true);
        flag1 = true;
      }
      if (current2.existNewMember() && current1.guild.auto_approval.auto_approval)
      {
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newMember, true);
        flag1 = true;
      }
label_15:
      if (((IEnumerable<GuildEventGift>) current2.gift_events).Any<GuildEventGift>((Func<GuildEventGift, bool>) (x => x.event_type == GuildEventType.incoming_gift)))
      {
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newGift, true);
        flag1 = true;
      }
      if (current2.existRoleChange())
      {
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.changeRole, true);
        flag1 = true;
      }
      if (current2.existNewTitle())
      {
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.newTitle, true);
        flag1 = true;
      }
      if (current2.existChatEvent(GuildEventType.post_new_chat) && !flag3)
      {
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.postNewChat, true);
        flag1 = true;
      }
      if (!flag2)
      {
        current2.existPlayershipEventType(GuildEventType.apply_applicant);
        if (current2.existPayloadEvent(GuildEventType.level_up))
        {
          GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.guildLevelup, true);
          flag1 = true;
        }
        if (current2.existBaseEvent(GuildEventType.base_rank_up))
        {
          GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.baseRankUp, true);
          flag1 = true;
        }
      }
      if (!GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newApplicant) && (flag2 || !GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.startHuntingEvent)) && (flag2 || !GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.receiveHuntingReward)) && (flag2 || !GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newMember)) && !GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.newGift) && !GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.guildLevelup))
        GuildUtil.getBadgeState(GuildUtil.GuildBadgeInfoType.baseRankUp);
      GuildEvent guildEvent = ((IEnumerable<GuildEvent>) current2.guild_events).OrderByDescending<GuildEvent, DateTime?>((Func<GuildEvent, DateTime?>) (x => x.created_at)).FirstOrDefault<GuildEvent>((Func<GuildEvent, bool>) (x => x.event_type == GuildEventType.gvg_entry || x.event_type == GuildEventType.gvg_matched || x.event_type == GuildEventType.gvg_started || x.event_type == GuildEventType.gvg_finished || x.event_type == GuildEventType.gvg_entry_expired || x.event_type == GuildEventType.gvg_entry_cancel || x.event_type == GuildEventType.guild_raid_started || x.event_type == GuildEventType.guild_raid_end));
      if (guildEvent != null)
      {
        flag1 = true;
        Persist.gvgBattleEnvironment.Delete();
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_entry, false);
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_matched, false);
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_started, false);
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.guild_raid, false);
        switch (guildEvent.event_type)
        {
          case GuildEventType.gvg_entry:
            GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_entry, true);
            break;
          case GuildEventType.gvg_matched:
            GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_matched, true);
            break;
          case GuildEventType.gvg_started:
            GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_started, true);
            break;
          case GuildEventType.gvg_entry_cancel:
            GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.gvg_entry, false);
            break;
          case GuildEventType.guild_raid_started:
            GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.guild_raid, true);
            break;
          case GuildEventType.guild_raid_end:
            GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.guild_raid, false);
            break;
        }
      }
      if (!flag1)
        return;
      Persist.guildSetting.Flush();
    }
  }

  private IEnumerator openStorySelectPopup()
  {
    CommonFooter behaviour = this;
    behaviour.IsPush = true;
    behaviour.isOpenPopup = true;
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    if (Singleton<NGSceneManager>.GetInstance().sceneName == "mypage")
    {
      NGSceneBase sceneBase = Singleton<NGSceneManager>.GetInstance().sceneBase;
    }
    Future<GameObject> prefabF = new ResourceObject("Prefabs/mypage/dir_story_menu").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject popup = prefabF.Result.Clone();
    popup.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    e = popup.GetComponent<PopupMypageStorySelect>().Initialize(new Action<int, bool>(behaviour.\u003CopenStorySelectPopup\u003Eb__74_0), new Action(behaviour.\u003CopenStorySelectPopup\u003Eb__74_1), (MonoBehaviour) behaviour);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true, isNonSe: true);
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1056");
  }
}
