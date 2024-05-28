// Decompiled with JetBrains decompiler
// Type: MyPageStoryMenu
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
public class MyPageStoryMenu : MyPageSubMenu
{
  [Space(8f)]
  [SerializeField]
  private Transform CharaAnimAnchor;
  [SerializeField]
  private Transform MissionPanelAnchor;
  [SerializeField]
  private UI2DSprite StoryDecoration;
  [Header("Button Controller")]
  [SerializeField]
  public MypageEventButtonController MainButtonController;
  [SerializeField]
  public MypageEventButtonController SubButtonController;
  [SerializeField]
  public MypageEventButtonController EventButtonController;
  [Header("Pannel Mission Notice")]
  [SerializeField]
  private MypageMissionClearInfo MissionClearInfo;
  [Header("3D Effect Setting")]
  [SerializeField]
  private Transform mainMenuEffectPosition;
  [SerializeField]
  private UITexture mainMenuEffectTexture;
  [SerializeField]
  private Camera mainMenuEffectCamera;
  [SerializeField]
  private Transform mainMenuEffectParticlePosition;
  private GameObject FullScreenEffect;
  private MypagePanelMissionInfo NextPanelMission;
  private GameObject CharaAnimObj;
  private UI2DSprite[] CharaSprites;
  private UILabel[] CharaName;
  private UILabel CharaAnimEnglishName;
  private Animator CharaAnimator;
  private bool CharaAnimation;
  private string TweenName = string.Empty;
  private ulong? CharacterId;
  private int ButtonCount;
  private int ButtonTweenFinishedCount;
  private int NextPlayVoiceIndex;
  [NonSerialized]
  public bool VoiceSkipDirty;

  public bool isAnimePlaying
  {
    get
    {
      if (!Object.op_Inequality((Object) this.CharaAnimator, (Object) null) || !this.CharaAnimation)
        return false;
      AnimatorStateInfo animatorStateInfo = this.CharaAnimator.GetCurrentAnimatorStateInfo(0);
      return (double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0;
    }
  }

  public IEnumerator InitSceneAsync(MypageRootMenu rootMenu)
  {
    MyPageStoryMenu myPageStoryMenu = this;
    myPageStoryMenu.RootMenu = rootMenu;
    myPageStoryMenu.SoundMgr = Singleton<NGSoundManager>.GetInstance();
    IEnumerator e = myPageStoryMenu.CreateCharcterAnimation();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = myPageStoryMenu.CreateMissionInfo();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = myPageStoryMenu.CreateMainMenuEffect();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = myPageStoryMenu.CreateEventButtonIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator OnStartSceneAsync(bool fromEarthTop)
  {
    MyPageStoryMenu myPageStoryMenu = this;
    myPageStoryMenu.CharaAnimation = false;
    myPageStoryMenu.RootMenu.NotTouch.SetActive(true);
    myPageStoryMenu.InitTween();
    IEnumerator e = myPageStoryMenu.CharcterAnimationSetting(fromEarthTop, MypageMenuBase.HEAVEN_IN_BG_TWEENGROUP);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = myPageStoryMenu.SetStoryDecorateImage();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    myPageStoryMenu.SetupNextPanelMissionBanner();
    myPageStoryMenu.MissionClearInfoSetting();
    Animator component1 = ((Component) Singleton<CommonRoot>.GetInstance()).GetComponent<Animator>();
    if (Object.op_Implicit((Object) component1))
    {
      AnimatorStateInfo animatorStateInfo = component1.GetCurrentAnimatorStateInfo(0);
      component1.Play(((AnimatorStateInfo) ref animatorStateInfo).fullPathHash, 0, 0.0f);
    }
    Animator component2 = ((Component) myPageStoryMenu).GetComponent<Animator>();
    if (Object.op_Implicit((Object) component2))
    {
      AnimatorStateInfo animatorStateInfo = component2.GetCurrentAnimatorStateInfo(0);
      component2.Play(((AnimatorStateInfo) ref animatorStateInfo).fullPathHash, 0, 0.0f);
    }
    myPageStoryMenu.MainButtonController.UpdateButtonState();
    myPageStoryMenu.SubButtonController.UpdateButtonState();
    myPageStoryMenu.EventButtonController.UpdateButtonState();
  }

  public void OnIntroFinished(bool fromEarthTop)
  {
    this.MainButtonController.UpdateButtonState();
    this.SubButtonController.UpdateButtonState();
    this.EventButtonController.UpdateButtonState();
    Singleton<CommonRoot>.GetInstance().SetFooterExploreButtonEnable(this.MainButtonController.GetButton<ExploreButton>().IsActive());
    this.CharaAnimProc();
    if (fromEarthTop)
    {
      ((Component) this.MainButtonController).gameObject.SetActive(false);
      this.PlayTween(MypageMenuBase.START_TWEENGROUP_TOP);
      Singleton<CommonRoot>.GetInstance().StartCloudAnimEnd((Action) (() => ((Component) this.MainButtonController).gameObject.SetActive(true)));
    }
    else
      this.PlayTween(MypageMenuBase.START_TWEENGROUP);
    this.StartEventButtonIconEffect();
  }

  public void OnEndScene()
  {
    if (Object.op_Inequality((Object) this.SoundMgr, (Object) null) && ((Behaviour) this.SoundMgr).enabled)
      this.SoundMgr.stopVoice();
    this.HidePanelMission();
  }

  public override void OnModeSwitched(MypageRootMenu.Mode mode)
  {
    if (!Object.op_Inequality((Object) this.SoundMgr, (Object) null) || !((Behaviour) this.SoundMgr).enabled)
      return;
    this.SoundMgr.stopVoice();
  }

  protected void InitTween()
  {
    this.ButtonCount = 0;
    this.ButtonTweenFinishedCount = 0;
    ((IEnumerable<UITweener>) ((Component) this).GetComponentsInChildren<UITweener>(true)).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.onFinished.Clear();
      ((Behaviour) x).enabled = false;
    }));
  }

  public void PlayTween(int groupID)
  {
    UITweener[] array = ((IEnumerable<UITweener>) ((Component) this).GetComponentsInChildren<UITweener>()).Where<UITweener>((Func<UITweener, bool>) (x => x.tweenGroup == groupID)).ToArray<UITweener>();
    if (groupID == MypageMenuBase.START_TWEENGROUP_TOP)
      array = ((IEnumerable<UITweener>) array).Where<UITweener>((Func<UITweener, bool>) (x => !((Object) x).name.Equals("icon") && !((Object) x).name.Equals("GearKindIcon(Clone)") && !((Object) x).name.Equals("weapon_type") && !((Object) x).name.Equals("rarity_star") && !((Object) x).name.Equals("hime_type"))).ToArray<UITweener>();
    foreach (UITweener uiTweener in array)
    {
      ((Component) uiTweener).gameObject.SetActive(true);
      uiTweener.ResetToBeginning();
      uiTweener.PlayForward();
    }
  }

  private void ButtonTweenFinished()
  {
    ++this.ButtonTweenFinishedCount;
    if (this.ButtonCount != this.ButtonTweenFinishedCount)
      return;
    this.AllButtonTweenFinished();
  }

  private void AllButtonTweenFinished()
  {
    this.RootMenu.NotTouch.SetActive(false);
    if (MypageRootMenu.CurrentMode == MypageRootMenu.Mode.STORY)
      this.CharaStartVoice();
    this.MissionClearInfo.startDisplay();
  }

  private void AddStartTweenDelay(float addTime, int groupID)
  {
    List<MypageSlidePanelDragged> buttons = new List<MypageSlidePanelDragged>();
    ((IEnumerable<UITweener>) ((Component) this).GetComponentsInChildren<UITweener>(true)).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      if (x.tweenGroup != groupID)
        return;
      x.delay += addTime;
      EventDelegate.Add(x.onFinished, (EventDelegate.Callback) (() => x.delay -= addTime));
      foreach (Component component in buttons)
      {
        if (Object.op_Equality((Object) component.transform, (Object) ((Component) x).gameObject.transform))
        {
          EventDelegate.Add(x.onFinished, (EventDelegate.Callback) (() => this.ButtonTweenFinished()));
          break;
        }
      }
    }));
    this.ButtonCount = buttons.Count;
  }

  private IEnumerator CreateCharcterAnimation()
  {
    MyPageStoryMenu myPageStoryMenu = this;
    Future<GameObject> f = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/mypage/CharacterAnimator");
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    myPageStoryMenu.CharaAnimObj = f.Result.Clone(myPageStoryMenu.CharaAnimAnchor);
    Future<GameObject> prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/mypage/FullScreenEffect_anim");
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    myPageStoryMenu.FullScreenEffect = prefabF.Result.Clone(((Component) myPageStoryMenu).gameObject.transform);
    CharaAnimationConst component = myPageStoryMenu.CharaAnimObj.GetComponent<CharaAnimationConst>();
    myPageStoryMenu.CharaName = component.charaName;
    myPageStoryMenu.CharaAnimator = component.anim;
    myPageStoryMenu.CharaSprites = component.charaSprite;
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<UI2DSprite>) myPageStoryMenu.CharaSprites).ForEach<UI2DSprite>(new Action<UI2DSprite>(myPageStoryMenu.\u003CCreateCharcterAnimation\u003Eb__37_0));
    myPageStoryMenu.CharaAnimEnglishName = component._english_name;
    // ISSUE: reference to a compiler-generated method
    component.setFinishAction(new Action(myPageStoryMenu.\u003CCreateCharcterAnimation\u003Eb__37_1));
    myPageStoryMenu.CharaAnimObj.SetActive(false);
  }

  private IEnumerator CharcterAnimationSetting(bool fromEarthTop, int tweenID)
  {
    MyPageStoryMenu myPageStoryMenu = this;
    if (Object.op_Inequality((Object) myPageStoryMenu.CharaAnimObj, (Object) null))
      myPageStoryMenu.CharaAnimObj.SetActive(false);
    PlayerUnit unit = myPageStoryMenu.GetDisplayPlayerUnit();
    UnitUnit unitUnit = unit.unit;
    ulong charId = myPageStoryMenu.MakeCharacterId(unitUnit.ID, unit.job_id);
    if (myPageStoryMenu.CharacterId.HasValue)
    {
      long num = (long) charId;
      ulong? characterId = myPageStoryMenu.CharacterId;
      long valueOrDefault = (long) characterId.GetValueOrDefault();
      if (num == valueOrDefault & characterId.HasValue)
        goto label_16;
    }
    Future<Sprite> LeaderF = unitUnit.LoadSpriteLarge(unit.job_id, 1f);
    IEnumerator e = LeaderF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    for (int index = 0; index < myPageStoryMenu.CharaSprites.Length; ++index)
      myPageStoryMenu.CharaSprites[index].sprite2D = LeaderF.Result;
    for (int index = 0; index < myPageStoryMenu.CharaName.Length; ++index)
      myPageStoryMenu.CharaName[index].SetTextLocalize(unitUnit.name);
    if (Object.op_Inequality((Object) myPageStoryMenu.CharaAnimEnglishName, (Object) null))
      myPageStoryMenu.CharaAnimEnglishName.SetTextLocalize(unit.unit.english_name);
    myPageStoryMenu.CharacterId = new ulong?(charId);
    LeaderF = (Future<Sprite>) null;
label_16:
    if (unitUnit.ID != MypageMenuBase.LeaderID)
    {
      MypageMenuBase.PlayTouchVoiceList.Clear();
      myPageStoryMenu.NextPlayVoiceIndex = 0;
      if (unitUnit.unitVoicePattern != null && myPageStoryMenu.SoundMgr.LoadVoiceData(unitUnit.unitVoicePattern.file_name))
      {
        while (!myPageStoryMenu.SoundMgr.LoadedCueSheet(unitUnit.unitVoicePattern.file_name))
          yield return (object) null;
        HashSet<int> self = new HashSet<int>();
        foreach (int enableId in UnitHomeVoicePattern.GetEnableIDList())
        {
          if (myPageStoryMenu.SoundMgr.ExistsCueID(unitUnit.unitVoicePattern.file_name, enableId))
            self.Add(enableId);
        }
        MypageMenuBase.PlayTouchVoiceList = self.Shuffle<int>().ToList<int>();
      }
    }
    float addDelayTime = myPageStoryMenu.SetCharacterAnimControllerWithGetAddDelayTime(!fromEarthTop && unitUnit.ID != MypageMenuBase.LeaderID, unitUnit.ID);
    if (fromEarthTop)
    {
      Singleton<CommonRoot>.GetInstance().StartBGTween(tweenID);
      myPageStoryMenu.AddStartTweenDelay(addDelayTime, MypageMenuBase.START_TWEENGROUP_TOP);
    }
    else
      myPageStoryMenu.AddStartTweenDelay(addDelayTime, MypageMenuBase.START_TWEENGROUP);
  }

  private PlayerUnit GetDisplayPlayerUnit()
  {
    int mypage_unit_id = MypageUnitUtil.getUnitId();
    if (mypage_unit_id == 0)
      return this.GetDeckLeaderPlayerUnit();
    PlayerUnit displayPlayerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == mypage_unit_id));
    if (!(displayPlayerUnit == (PlayerUnit) null))
      return displayPlayerUnit;
    MypageUnitUtil.setDefaultUnitNotFound();
    return this.GetDeckLeaderPlayerUnit();
  }

  private PlayerUnit GetDeckLeaderPlayerUnit()
  {
    PlayerDeck[] playerDeckArray = SMManager.Get<PlayerDeck[]>();
    PlayerUnit leaderPlayerUnit = ((IEnumerable<PlayerUnit>) playerDeckArray[Persist.deckOrganized.Data.number].player_units).FirstOrDefault<PlayerUnit>();
    if (leaderPlayerUnit == (PlayerUnit) null)
      leaderPlayerUnit = ((IEnumerable<PlayerUnit>) playerDeckArray[0].player_units).First<PlayerUnit>();
    return leaderPlayerUnit;
  }

  private ulong MakeCharacterId(int unitId, int jobId) => ((ulong) jobId << 32) + (ulong) unitId;

  private float SetCharacterAnimControllerWithGetAddDelayTime(bool LeaderChange, int id)
  {
    float addDelayTime;
    if (LeaderChange)
    {
      this.TweenName = "Play";
      addDelayTime = 0.0f;
    }
    else
    {
      this.TweenName = "Fade";
      addDelayTime = 0.0f;
    }
    this.CharaAnimation = true;
    MypageMenuBase.LeaderID = id;
    return addDelayTime;
  }

  public void CharaAnimProc()
  {
    if (!this.CharaAnimation)
      return;
    this.CharaAnimObj.SetActive(true);
    this.CharaAnimator.SetBool(this.TweenName, true);
    Animator component1 = ((Component) Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>()).GetComponent<Animator>();
    if (Object.op_Implicit((Object) component1))
      component1.SetTrigger(this.TweenName);
    Animator component2 = ((Component) Singleton<CommonRoot>.GetInstance()).GetComponent<Animator>();
    if (Object.op_Implicit((Object) component2))
      component2.SetTrigger(this.TweenName);
    Animator component3 = ((Component) this).GetComponent<Animator>();
    if (Object.op_Implicit((Object) component3))
      component3.SetTrigger(this.TweenName);
    Animator component4 = this.FullScreenEffect.GetComponent<Animator>();
    if (!Object.op_Implicit((Object) component4))
      return;
    component4.SetTrigger(this.TweenName);
  }

  private void CharaStartVoice()
  {
    if (this.VoiceSkipDirty)
    {
      this.VoiceSkipDirty = false;
    }
    else
    {
      CharaVoiceCueEnum.CueID[] cueIdArray = new CharaVoiceCueEnum.CueID[6]
      {
        CharaVoiceCueEnum.CueID.MYPAGE_TIMESIGNAL_0_3,
        CharaVoiceCueEnum.CueID.MYPAGE_TIMESIGNAL_4_7,
        CharaVoiceCueEnum.CueID.MYPAGE_TIMESIGNAL_8_11,
        CharaVoiceCueEnum.CueID.MYPAGE_TIMESIGNAL_12_15,
        CharaVoiceCueEnum.CueID.MYPAGE_TIMESIGNAL_16_19,
        CharaVoiceCueEnum.CueID.MYPAGE_TIMESIGNAL_20_23
      };
      if (!MasterData.UnitUnit.ContainsKey(MypageMenuBase.LeaderID))
        return;
      this.SoundMgr.playVoiceByID(MasterData.UnitUnit[MypageMenuBase.LeaderID].unitVoicePattern, (int) cueIdArray[DateTime.Now.Hour / 4]);
    }
  }

  private void CharaTouchVoice()
  {
    if (Object.op_Equality((Object) this.SoundMgr, (Object) null))
      this.SoundMgr = Singleton<NGSoundManager>.GetInstance();
    this.SoundMgr.stopVoice();
    if (!MasterData.UnitUnit.ContainsKey(MypageMenuBase.LeaderID))
      return;
    UnitVoicePattern unitVoicePattern = MasterData.UnitUnit[MypageMenuBase.LeaderID].unitVoicePattern;
    if (Random.Range(0, 1000) < 500 && unitVoicePattern != null && this.SoundMgr.ExistsCueID(unitVoicePattern.file_name, 90))
    {
      this.SoundMgr.playVoiceByID(unitVoicePattern, 90);
    }
    else
    {
      if (MypageMenuBase.PlayTouchVoiceList.Count <= 0 || unitVoicePattern == null)
        return;
      this.SoundMgr.playVoiceByID(unitVoicePattern, MypageMenuBase.PlayTouchVoiceList[this.NextPlayVoiceIndex]);
      this.NextPlayVoiceIndex = (this.NextPlayVoiceIndex + 1) % MypageMenuBase.PlayTouchVoiceList.Count;
      if (this.NextPlayVoiceIndex != 0)
        return;
      MypageMenuBase.PlayTouchVoiceList = MypageMenuBase.PlayTouchVoiceList.Shuffle<int>().ToList<int>();
    }
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

  private IEnumerator CreateEventButtonIcon()
  {
    MypageEventButtonHorizontalAlignmentController buttonController = this.EventButtonController as MypageEventButtonHorizontalAlignmentController;
    if (!Object.op_Equality((Object) buttonController, (Object) null))
    {
      IEnumerator e = buttonController.InitSceneAsync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void StartEventButtonIconEffect()
  {
    MypageEventButtonHorizontalAlignmentController buttonController = this.EventButtonController as MypageEventButtonHorizontalAlignmentController;
    if (Object.op_Equality((Object) buttonController, (Object) null))
      return;
    buttonController.StartIconEffect();
  }

  private IEnumerator CreateMissionInfo()
  {
    IEnumerator e = this.MissionClearInfo.initOne();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> ft = new ResourceObject("Prefabs/mypage/dir_Mission_Panel").Load<GameObject>();
    e = ft.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.NextPanelMission = ft.Result.CloneAndGetComponent<MypagePanelMissionInfo>(this.MissionPanelAnchor);
    ((Component) this.NextPanelMission).gameObject.SetActive(false);
  }

  private void SetupNextPanelMissionBanner()
  {
    Player player = SMManager.Get<Player>();
    if (player.next_panel_mission_id > 0 && MasterData.BingoMission.ContainsKey(player.next_panel_mission_id))
    {
      ((Component) this.NextPanelMission).gameObject.SetActive(true);
      this.NextPanelMission.InitPanelMissionInfo(player.next_panel_mission_id);
    }
    else
      ((Component) this.NextPanelMission).gameObject.SetActive(false);
  }

  private void MissionClearInfoSetting() => this.MissionClearInfo.init(SMManager.Get<Player>());

  public void HidePanelMission()
  {
    ((Component) this.NextPanelMission).gameObject.SetActive(false);
    this.MissionClearInfo.clearAll();
  }

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
      this.StoryDecoration.sprite2D = f.Result;
      f = (Future<Sprite>) null;
    }
    else
      this.StoryDecoration.sprite2D = (Sprite) null;
  }
}
