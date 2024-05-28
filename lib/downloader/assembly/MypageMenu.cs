// Decompiled with JetBrains decompiler
// Type: MypageMenu
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
public class MypageMenu : MypageMenuBase
{
  [SerializeField]
  public MypageEventButtonController EventButtonController;
  [SerializeField]
  private GameObject InfoNew;
  [SerializeField]
  private GameObject PresentNew;
  [SerializeField]
  private UILabel LblPresentNum;
  [SerializeField]
  private GameObject PanelMissionBtn;
  [SerializeField]
  private GameObject PanelMissionClear;
  [SerializeField]
  private MypagePanelMissionInfo NextPanelMission;
  [SerializeField]
  private GameObject DirPanelMissionBtn;
  [SerializeField]
  private GameObject DirPanelMissionEffect;
  [SerializeField]
  private UIButton ExploreBtn;
  [SerializeField]
  private GameObject ExploreProgressMaxBadge;
  [SerializeField]
  private MypageMissionClearInfo MissionClearInfo;
  [SerializeField]
  public Transform mainMenuEffectPosition;
  [SerializeField]
  public UITexture mainMenuEffectTexture;
  [SerializeField]
  public Camera mainMenuEffectCamera;
  [SerializeField]
  public Transform mainMenuEffectParticlePosition;
  [SerializeField]
  private UI2DSprite slc_story_start;
  [NonSerialized]
  public GameObject _full_screen_effect;
  private GameObject LoginPopup;
  private int LoginBonusCloseCounter;
  private int LevelUpBonusCloseCounter;
  private int LevelUpBonusCount;

  public bool isLoginPopupActive => Object.op_Inequality((Object) this.LoginPopup, (Object) null);

  public override IEnumerator InitSceneAsync()
  {
    MypageMenu mypageMenu = this;
    mypageMenu.sm = Singleton<NGSoundManager>.GetInstance();
    mypageMenu.BackBtnEnable = false;
    IEnumerator e = mypageMenu.CreateCharcterAnimation();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = mypageMenu.CreateLoginPopup();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = mypageMenu.CreateMissionClearInfo();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = mypageMenu.CreateMainMenuEffect();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override IEnumerator onStartSceneAsync(bool isCloudAnim, bool isReservedEventScript)
  {
    MypageMenu mypageMenu = this;
    Singleton<NGGameDataManager>.GetInstance().IsColosseum = false;
    mypageMenu.CharaAnimation = false;
    mypageMenu.InitTween();
    IEnumerator e = mypageMenu.SetStoryDecorateImage();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = mypageMenu.CharcterAnimationSetting(isCloudAnim, MypageMenuBase.HEAVEN_IN_BG_TWEENGROUP);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mypageMenu.PresentIconSetting();
    mypageMenu.InfoIconSetting();
    e = mypageMenu.PanelMissionIconSetting();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!isReservedEventScript)
      mypageMenu.MissionClearInfoSetting();
    Singleton<CommonRoot>.GetInstance().UpdateFooterGachaButton();
    mypageMenu.StartCoroutine(Singleton<CommonRoot>.GetInstance().UpdateFooterLimitedShopButton());
    Singleton<CommonRoot>.GetInstance().UpdateHeaderBikkuriIcon();
    Singleton<CommonRoot>.GetInstance().UpdateFooterBikkuriIcon();
    Singleton<CommonRoot>.GetInstance().UpdateFooterNewbiePacksIcon();
    mypageMenu.EventButtonController.UpdateButtonState();
    Animator component1 = ((Component) Singleton<CommonRoot>.GetInstance()).GetComponent<Animator>();
    if (Object.op_Implicit((Object) component1))
    {
      AnimatorStateInfo animatorStateInfo = component1.GetCurrentAnimatorStateInfo(0);
      component1.Play(((AnimatorStateInfo) ref animatorStateInfo).fullPathHash, 0, 0.0f);
    }
    Animator component2 = ((Component) mypageMenu).GetComponent<Animator>();
    if (Object.op_Implicit((Object) component2))
    {
      AnimatorStateInfo animatorStateInfo = component2.GetCurrentAnimatorStateInfo(0);
      component2.Play(((AnimatorStateInfo) ref animatorStateInfo).fullPathHash, 0, 0.0f);
    }
  }

  public void onStartScene(bool isCloudAnim)
  {
    this.ExploreButtonSetting();
    this.CharaAnimProc();
    if (isCloudAnim)
    {
      this.SetMainButtonActive(false);
      this.PlayTween(MypageMenuBase.START_TWEENGROUP_TOP);
      Singleton<CommonRoot>.GetInstance().StartCloudAnimEnd((Action) (() =>
      {
        this.SetMainButtonActive(true);
        this.PlayTween(MypageMenuBase.START_TWEEN_GROUP_JOGDIAL);
      }));
    }
    else
    {
      this.PlayTween(MypageMenuBase.START_TWEENGROUP);
      this.TutorialAdvice();
    }
  }

  public override void onEndScene()
  {
    if (Object.op_Inequality((Object) this.sm, (Object) null) && ((Behaviour) this.sm).enabled)
      this.sm.stopVoice();
    this.StopMissionClearInfo();
    this.HidePanelMission();
  }

  protected override void AllButtonTweenFinished()
  {
    base.AllButtonTweenFinished();
    if (!Object.op_Inequality((Object) this.MissionClearInfo, (Object) null))
      return;
    this.MissionClearInfo.startDisplay();
  }

  protected override IEnumerator CreateCharcterAnimation()
  {
    MypageMenu mypageMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = mypageMenu.\u003C\u003En__0();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/mypage/FullScreenEffect_anim");
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mypageMenu._full_screen_effect = prefabF.Result.Clone(((Component) mypageMenu).gameObject.transform);
    // ISSUE: reference to a compiler-generated method
    mypageMenu.CharaAnimObj.GetComponent<CharaAnimationConst>().setFinishAction(new Action(mypageMenu.\u003CCreateCharcterAnimation\u003Eb__29_0));
    mypageMenu.CharaAnimObj.SetActive(false);
  }

  protected override IEnumerator CharcterAnimationSetting(bool isCloudAnim, int tweenID)
  {
    MypageMenu mypageMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = mypageMenu.\u003C\u003En__1(isCloudAnim, tweenID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerUnit displayPlayerUnit = mypageMenu.GetDisplayPlayerUnit();
    if (Object.op_Inequality((Object) mypageMenu.CharaAnimEnglishName, (Object) null))
      mypageMenu.CharaAnimEnglishName.SetText(displayPlayerUnit.unit.english_name);
  }

  public override void CharaAnimProc()
  {
    base.CharaAnimProc();
    if (!this.CharaAnimation)
      return;
    Animator component = this._full_screen_effect.GetComponent<Animator>();
    if (!Object.op_Implicit((Object) component))
      return;
    component.SetTrigger(this.TweenName);
  }

  private IEnumerator CreateLoginPopup()
  {
    Future<GameObject> f = Res.Prefabs.popup.popup_000_14_4__anim_popup01.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.LoginPopup = f.Result;
  }

  private IEnumerator CreateMissionClearInfo()
  {
    IEnumerator e = this.MissionClearInfo.initOne();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CreateMainMenuEffect()
  {
    ((Component) this.mainMenuEffectCamera).gameObject.SetActive(true);
    Future<GameObject> dirEffectF = Res.Animations.mypage.dir_mainmenu_effect.Load<GameObject>();
    IEnumerator e = dirEffectF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    dirEffectF.Result.Clone(this.mainMenuEffectPosition);
    this.mainMenuEffectCamera.targetTexture = new RenderTexture(512, 256, -5);
    this.mainMenuEffectCamera.targetTexture.antiAliasing = 1;
    ((Texture) this.mainMenuEffectCamera.targetTexture).wrapMode = (TextureWrapMode) 1;
    ((Texture) this.mainMenuEffectCamera.targetTexture).filterMode = (FilterMode) 1;
    this.mainMenuEffectCamera.targetTexture.enableRandomWrite = false;
    this.mainMenuEffectCamera.farClipPlane = 1000f;
    ((UIWidget) this.mainMenuEffectTexture).mainTexture = (Texture) this.mainMenuEffectCamera.targetTexture;
    Future<GameObject> mainMenuEffectGlowF = Res.Animations.mypage.Particle_mainmenu_glow.Load<GameObject>();
    e = mainMenuEffectGlowF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mainMenuEffectGlowF.Result.Clone(this.mainMenuEffectParticlePosition);
  }

  public void SetMainButtonActive(bool active) => this.MainButtonRoot.SetActive(active);

  private void StartLoginPopup(List<PlayerLoginBonus> loginBonus, int loginBonusCount)
  {
    ((UIRect) this.MainPanel).alpha = 0.0f;
    Startup000144Menu component = Singleton<PopupManager>.GetInstance().open(this.LoginPopup).GetComponent<Startup000144Menu>();
    component.InitScene(loginBonus[this.LoginBonusCloseCounter]);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    component.OnOkButton = (Action) (() =>
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      ++this.LoginBonusCloseCounter;
      if (loginBonusCount <= this.LoginBonusCloseCounter)
      {
        List<LevelRewardSchemaMixin> playerLevelRewards = Singleton<NGGameDataManager>.GetInstance().playerLevelRewards;
        if (playerLevelRewards != null && playerLevelRewards.Count > 0)
        {
          this.InitLevelUpPopup(playerLevelRewards);
          this.StartLevelUpPopup(playerLevelRewards);
        }
        else
        {
          ((UIRect) this.MainPanel).alpha = 1f;
          this.CharaAnimProc();
          this.PlayTween(MypageMenuBase.START_TWEENGROUP);
        }
      }
      else
        this.StartLoginPopup(loginBonus, loginBonusCount);
    });
  }

  public IEnumerator DoLoginPopup(List<PlayerLoginBonus> loginBonus)
  {
    MypageMenu mypageMenu = this;
    ((UIRect) mypageMenu.MainPanel).alpha = 0.0f;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    foreach (PlayerLoginBonus loginBonu in loginBonus)
    {
      bool bWait = true;
      Startup000144Menu component = Singleton<PopupManager>.GetInstance().open(mypageMenu.LoginPopup).GetComponent<Startup000144Menu>();
      component.InitScene(loginBonu);
      component.OnOkButton = (Action) (() =>
      {
        Singleton<PopupManager>.GetInstance().onDismiss();
        bWait = false;
      });
      while (bWait)
        yield return (object) null;
    }
    ((UIRect) mypageMenu.MainPanel).alpha = 1f;
  }

  private void InitLevelUpPopup(List<LevelRewardSchemaMixin> levelupBonus)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    this.LevelUpBonusCount = levelupBonus.Count;
    this.LevelUpBonusCloseCounter = 0;
    ((UIRect) this.MainPanel).alpha = 0.0f;
  }

  private void StartLevelUpPopup(List<LevelRewardSchemaMixin> levelupBonus)
  {
    ModalWindow.Show(levelupBonus[this.LevelUpBonusCloseCounter].reward_title, levelupBonus[this.LevelUpBonusCloseCounter].reward_message, (Action) (() =>
    {
      ++this.LevelUpBonusCloseCounter;
      if (this.LevelUpBonusCount <= this.LevelUpBonusCloseCounter)
      {
        Singleton<NGGameDataManager>.GetInstance().playerLevelRewards = (List<LevelRewardSchemaMixin>) null;
        ((UIRect) this.MainPanel).alpha = 1f;
        this.CharaAnimProc();
        this.PlayTween(MypageMenuBase.START_TWEENGROUP);
        this.TutorialAdvice();
      }
      else
        this.StartLevelUpPopup(levelupBonus);
    }));
  }

  public IEnumerator DoLevelUpPopup()
  {
    MypageMenu mypageMenu = this;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    ((UIRect) mypageMenu.MainPanel).alpha = 0.0f;
    foreach (LevelRewardSchemaMixin playerLevelReward in Singleton<NGGameDataManager>.GetInstance().playerLevelRewards)
    {
      bool bWait = true;
      ModalWindow.Show(playerLevelReward.reward_title, playerLevelReward.reward_message, (Action) (() => bWait = false));
      while (bWait)
        yield return (object) null;
    }
    ((UIRect) mypageMenu.MainPanel).alpha = 1f;
    Singleton<NGGameDataManager>.GetInstance().playerLevelRewards = (List<LevelRewardSchemaMixin>) null;
  }

  private void MissionClearInfoSetting() => this.MissionClearInfo.init(SMManager.Get<Player>());

  private IEnumerator SetStoryDecorateImage()
  {
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTime();
    QuestCommonJogDecoration commonJogDecoration = ((IEnumerable<QuestCommonJogDecoration>) MasterData.QuestCommonJogDecorationList).FirstOrDefault<QuestCommonJogDecoration>((Func<QuestCommonJogDecoration, bool>) (x => now >= x.start_at.Value && now <= x.end_at.Value));
    if (commonJogDecoration != null)
    {
      Future<Sprite> f = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("Prefabs/mypage/storyquest_banner/{0}", (object) commonJogDecoration.banner_id));
      e = f.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.slc_story_start.sprite2D = f.Result;
      f = (Future<Sprite>) null;
    }
    else
      this.slc_story_start.sprite2D = (Sprite) null;
  }

  private void PresentIconSetting()
  {
    if (Object.op_Equality((Object) this.PresentNew, (Object) null))
      return;
    PlayerPresent[] playerPresentArray = SMManager.Get<PlayerPresent[]>();
    int num1 = 0;
    if (playerPresentArray != null)
    {
      foreach (PlayerPresent playerPresent in playerPresentArray)
      {
        if (!playerPresent.received_at.HasValue)
          ++num1;
      }
    }
    if (num1 > 0)
    {
      int num2 = num1;
      if (num2 > StaticConsts.PRESENT_COUNT_DISPLAY_MAX)
        num2 = StaticConsts.PRESENT_COUNT_DISPLAY_MAX;
      this.PresentNew.SetActive(true);
      if (!Object.op_Implicit((Object) this.LblPresentNum))
        return;
      this.LblPresentNum.SetTextLocalize(string.Format("[b]{0}[-]", (object) num2));
    }
    else
      this.PresentNew.SetActive(false);
  }

  private void InfoIconSetting()
  {
    if (Object.op_Equality((Object) this.InfoNew, (Object) null))
      return;
    OfficialInformationArticle[] source = SMManager.Get<OfficialInformationArticle[]>();
    if (source == null)
    {
      this.InfoNew.SetActive(false);
    }
    else
    {
      bool flag;
      try
      {
        flag = ((IEnumerable<OfficialInformationArticle>) source).Any<OfficialInformationArticle>((Func<OfficialInformationArticle, bool>) (w => !Persist.infoUnRead.Data.GetUnRead(w)));
      }
      catch
      {
        Persist.infoUnRead.Delete();
        flag = source != null && source.Length != 0;
      }
      this.InfoNew.SetActive(flag);
    }
  }

  private IEnumerator PanelMissionIconSetting()
  {
    if (!Object.op_Equality((Object) this.PanelMissionBtn, (Object) null) && !Object.op_Equality((Object) this.PanelMissionClear, (Object) null) && !Object.op_Equality((Object) this.NextPanelMission, (Object) null))
    {
      Player player = SMManager.Get<Player>();
      if (player.next_panel_mission_id > 0 && MasterData.BingoMission.ContainsKey(player.next_panel_mission_id))
      {
        ((Component) this.NextPanelMission).gameObject.SetActive(true);
        this.NextPanelMission.InitPanelMissionInfo(player.next_panel_mission_id);
      }
      else
        ((Component) this.NextPanelMission).gameObject.SetActive(false);
      this.PanelMissionClear.SetActive(player.is_open_bingo);
      this.DirPanelMissionBtn.SetActive(!player.is_bingo_end);
      this.DirPanelMissionEffect.SetActive(!player.is_bingo_end);
      this.PanelMissionBtn.SetActive(!player.is_bingo_end);
      yield break;
    }
  }

  public void ExploreButtonSetting()
  {
    ((UIButtonColor) this.ExploreBtn).isEnabled = true;
    this.ExploreProgressMaxBadge.SetActive(false);
    if (!Singleton<TutorialRoot>.GetInstance().IsTutorialFinish())
      return;
    ExploreDataManager instance = Singleton<ExploreDataManager>.GetInstance();
    if (instance.IsNotRegisteredDeck())
    {
      if (!(instance.LastSyncTime > ServerTime.NowAppTimeAddDelta()))
        return;
      ((UIButtonColor) this.ExploreBtn).isEnabled = false;
    }
    else if (instance.IsRankingAcceptanceFinish)
    {
      ((UIButtonColor) this.ExploreBtn).isEnabled = false;
    }
    else
    {
      if (!instance.NeedShowBadge)
        return;
      this.ExploreProgressMaxBadge.SetActive(true);
    }
  }

  public void StopMissionClearInfo()
  {
    if (Object.op_Equality((Object) this.MissionClearInfo, (Object) null))
      return;
    this.MissionClearInfo.clearAll();
  }

  public void HidePanelMission()
  {
    ((Component) this.NextPanelMission).gameObject.SetActive(false);
    this.PanelMissionBtn.gameObject.SetActive(false);
    this.DirPanelMissionBtn.SetActive(false);
    this.DirPanelMissionEffect.SetActive(false);
  }

  public IEnumerator DoExploreRankingPopup()
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Future<WebAPI.Response.ExploreRankingResult> futureF = WebAPI.ExploreRankingResult((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureF.Result != null)
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
}
