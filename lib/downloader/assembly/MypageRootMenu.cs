// Decompiled with JetBrains decompiler
// Type: MypageRootMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class MypageRootMenu : BackButtonMenuBase
{
  private static bool sFirstPlay = true;
  private static MypageRootMenu.Mode sMode = MypageRootMenu.Mode.STORY;
  [SerializeField]
  private Animator mUiAnimController;
  [SerializeField]
  private MyPageCommonMenu mCommonMenu;
  [SerializeField]
  private MyPageStoryMenu mStoryMenu;
  [SerializeField]
  private Transform mGuildMenuAnchor;
  [SerializeField]
  public GameObject NotTouch;
  [Header("Windows Adjust Parts")]
  [SerializeField]
  private GameObject[] mWinAdjObj = new GameObject[0];
  [SerializeField]
  private Vector3[] mWinAdjPos = new Vector3[0];
  [SerializeField]
  private Vector3[] mWinAdjScl = new Vector3[0];
  private MypageScene mScene;
  private MyPageGuildMenu mGuildMenu;
  private GameObject mLoginPopupPrefab;
  private MypageRootMenu.GuildMenuMode mGuildMenuMode;

  public static MypageRootMenu.Mode CurrentMode
  {
    get => MypageRootMenu.sMode;
    set => MypageRootMenu.sMode = value;
  }

  public static void ClearCache()
  {
    MypageRootMenu.sFirstPlay = false;
    MypageRootMenu.sMode = MypageRootMenu.Mode.STORY;
    MypageMenuBase.LeaderID = 0;
    MypageMenuBase.PlayTouchVoiceList = new List<int>();
  }

  public WebAPI.Response.GuildTop GuildTopResponse { get; private set; }

  public MypageEventButtonController EventButtonController => this.mStoryMenu.EventButtonController;

  public bool isAnimePlaying => this.mStoryMenu.isAnimePlaying;

  public IEnumerator InitSceneAsync()
  {
    MypageRootMenu rootMenu = this;
    rootMenu.mScene = ((Component) rootMenu).GetComponent<MypageScene>();
    IEnumerator e = rootMenu.LoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = rootMenu.mCommonMenu.InitSceneAsync(rootMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = rootMenu.mStoryMenu.InitSceneAsync(rootMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = rootMenu.mGuildMenu.InitSceneAsync(rootMenu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    rootMenu.AdjustPartsOnWindows();
  }

  public IEnumerator OnStartSceneAsync(bool fromEarthTop)
  {
    IEnumerator e = Singleton<ExploreDataManager>.GetInstance().LoadSuspendData(false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.GuildTopResponse = Singleton<NGGameDataManager>.GetInstance().GuildTopResponse;
    if (this.GuildTopResponse == null || !PlayerAffiliation.Current.isGuildMember())
      MypageRootMenu.CurrentMode = MypageRootMenu.Mode.STORY;
    e = Singleton<CommonRoot>.GetInstance().UpdateAllButtonBadges();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mGuildMenuMode = MypageRootMenu.GuildMenuMode.FACILITY;
    e = this.mCommonMenu.OnStartSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.mStoryMenu.OnStartSceneAsync(fromEarthTop);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.mGuildMenu.OnStartSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    switch (MypageRootMenu.CurrentMode)
    {
      case MypageRootMenu.Mode.STORY:
        if (MypageRootMenu.sFirstPlay)
        {
          this.mUiAnimController.SetTrigger("Play");
          break;
        }
        this.mUiAnimController.SetTrigger("Fade");
        break;
      case MypageRootMenu.Mode.GUILD:
        this.mUiAnimController.SetTrigger("GuildHomeIn");
        break;
    }
    MypageRootMenu.sFirstPlay = false;
  }

  public void OnStartScene()
  {
    switch (MypageRootMenu.CurrentMode)
    {
      case MypageRootMenu.Mode.STORY:
        Singleton<CommonRoot>.GetInstance().isActiveBackground = true;
        break;
      case MypageRootMenu.Mode.GUILD:
        Singleton<CommonRoot>.GetInstance().isActiveBackground = false;
        break;
    }
  }

  public void OnIntroFinished(bool fromEarthTop)
  {
    this.mStoryMenu.OnIntroFinished(fromEarthTop);
    this.mGuildMenu.OnIntroFinished();
    if (!fromEarthTop)
      this.StartCoroutine(this.CheckAndStartTutorial());
    else
      this.BackBtnEnable = true;
    this.StartCoroutine(this.CheckAndStartHotDeal());
  }

  public void OnEndScene(bool introFinished)
  {
    if (!introFinished)
      return;
    this.mStoryMenu.OnEndScene();
    this.mGuildMenu.OnEndScene();
    this.mUiAnimController.SetTrigger("Out");
  }

  public IEnumerator OnEndSceneAsync(bool introFinished)
  {
    if (introFinished)
      yield return (object) this.WaitForUIAnimationFinish(0, (Action) (() => Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0)));
    else
      yield return (object) this.WaitForUIAnimationFinish(0, (Action) (() => MypageRootMenu.sFirstPlay = true));
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowGameEndPopoup());
  }

  private IEnumerator LoadResources()
  {
    Future<GameObject> guildMenuFt = new ResourceObject("Prefabs/mypage/dir_Guild").Load<GameObject>();
    IEnumerator e = guildMenuFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mGuildMenu = guildMenuFt.Result.CloneAndGetComponent<MyPageGuildMenu>(this.mGuildMenuAnchor);
    ((Object) ((Component) this.mGuildMenu).gameObject).name = "dir_Guild";
    Future<RuntimeAnimatorController> uiAnimFt = new ResourceObject("Animations/mypage/Anims/ExportAssets/mypage_UI").Load<RuntimeAnimatorController>();
    e = uiAnimFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mUiAnimController.runtimeAnimatorController = uiAnimFt.Result;
    Future<GameObject> loginPopupFt = Res.Prefabs.popup.popup_000_14_4__anim_popup01.Load<GameObject>();
    e = loginPopupFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mLoginPopupPrefab = loginPopupFt.Result;
  }

  private void AdjustPartsOnWindows()
  {
    for (int index = 0; index < this.mWinAdjObj.Length; ++index)
    {
      this.mWinAdjObj[index].transform.localPosition = this.mWinAdjPos[index];
      this.mWinAdjObj[index].transform.localScale = this.mWinAdjScl[index];
    }
  }

  private IEnumerator ShowGameEndPopoup()
  {
    string path = "Prefabs/popup/popup_gameend__anim_popup01";
    Future<GameObject> prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefabF.Result).GetComponent<PopupTitleBack>().Init();
  }

  public void SwitchMode()
  {
    if (this.IsPushAndSet())
      return;
    switch (MypageRootMenu.CurrentMode)
    {
      case MypageRootMenu.Mode.STORY:
        MypageRootMenu.CurrentMode = MypageRootMenu.Mode.GUILD;
        this.mUiAnimController.SetTrigger("GuildChange");
        break;
      case MypageRootMenu.Mode.GUILD:
        MypageRootMenu.CurrentMode = MypageRootMenu.Mode.STORY;
        this.mUiAnimController.SetTrigger("MypageChange");
        Singleton<CommonRoot>.GetInstance().isActiveBackground = true;
        break;
    }
    this.OnModeSwitch(MypageRootMenu.CurrentMode);
    this.StartCoroutine(this.WaitForUIAnimationFinish(0, (Action) (() =>
    {
      this.OnModeSwitched(MypageRootMenu.CurrentMode);
      this.IsPush = false;
    })));
  }

  private void OnModeSwitch(MypageRootMenu.Mode mode)
  {
    this.mCommonMenu.OnModeSwitch(mode);
    this.mStoryMenu.OnModeSwitch(mode);
    this.mGuildMenu.OnModeSwitch(mode);
  }

  private void OnModeSwitched(MypageRootMenu.Mode mode)
  {
    if (mode == MypageRootMenu.Mode.GUILD)
      Singleton<CommonRoot>.GetInstance().isActiveBackground = false;
    this.mCommonMenu.OnModeSwitched(mode);
    this.mStoryMenu.OnModeSwitched(mode);
    this.mGuildMenu.OnModeSwitched(mode);
    this.mScene.OnMenuModeChanged(mode);
  }

  public void SwitchGuildMenu()
  {
    if (this.IsPushAndSet())
      return;
    switch (this.mGuildMenuMode)
    {
      case MypageRootMenu.GuildMenuMode.FACILITY:
        this.mGuildMenuMode = MypageRootMenu.GuildMenuMode.RING;
        this.mUiAnimController.SetTrigger("GuildMenuIn");
        break;
      case MypageRootMenu.GuildMenuMode.RING:
        this.mGuildMenuMode = MypageRootMenu.GuildMenuMode.FACILITY;
        this.mUiAnimController.SetTrigger("GuildMenuOut");
        break;
    }
    this.StartCoroutine(this.WaitForUIAnimationFinish(1, (Action) (() => this.IsPush = false)));
  }

  private IEnumerator WaitForUIAnimationFinish(int layer, Action finishCallback = null)
  {
    AnimatorStateInfo animatorStateInfo;
    do
    {
      yield return (object) null;
      animatorStateInfo = this.mUiAnimController.GetCurrentAnimatorStateInfo(layer);
    }
    while ((double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0);
    if (finishCallback != null)
      finishCallback();
  }

  public virtual IEnumerator CloudAnimationStart()
  {
    MypageRootMenu mypageRootMenu = this;
    Singleton<CommonRoot>.GetInstance().StartBGTween(MypageMenuBase.HEAVEN_OUT_BG_TWEENGROUP);
    mypageRootMenu.mStoryMenu.PlayTween(MypageMenuBase.END_TWEENGROUP_TOP);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = Singleton<CommonRoot>.GetInstance().StartCloudAnim(MypageCloudAnim.HeavenOut, MypageCloudAnim.EarthIn, new Action(mypageRootMenu.\u003CCloudAnimationStart\u003Eb__43_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CheckAndStartTutorial()
  {
    MypageRootMenu mypageRootMenu = this;
    while (mypageRootMenu.isAnimePlaying)
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
      yield return (object) null;
    }
    if (!Persist.tutorial.Data.IsFinishTutorial())
      Singleton<TutorialRoot>.GetInstance().CurrentAdvise();
    else if (Persist.integralNoaTutorial.Data.beginnersQuest)
    {
      mypageRootMenu.StartBeginnersQuestAdvice01();
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
      mypageRootMenu.BackBtnEnable = true;
    }
  }

  private IEnumerator CheckAndStartHotDeal()
  {
    MypageRootMenu mypageRootMenu = this;
    if (MypageRootMenu.CurrentMode == MypageRootMenu.Mode.STORY)
    {
      while (Singleton<CommonRoot>.GetInstance().isLoading)
        yield return (object) null;
      HotDealInfo info = ((IEnumerable<HotDealInfo>) Singleton<NGGameDataManager>.GetInstance().HotDealInfo).FirstOrDefault<HotDealInfo>((Func<HotDealInfo, bool>) (x => x.IsActive && x.is_modal));
      if (info != null)
      {
        mypageRootMenu.IsPush = true;
        mypageRootMenu.mStoryMenu.VoiceSkipDirty = true;
        IEnumerator e = mypageRootMenu.WaitForUIAnimationFinish(0, (Action) (() => { }));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        HotDealGenerator hotDealGenerator = new HotDealGenerator();
        for (; info != null; info = ((IEnumerable<HotDealInfo>) Singleton<NGGameDataManager>.GetInstance().HotDealInfo).FirstOrDefault<HotDealInfo>((Func<HotDealInfo, bool>) (x => x.IsActive && x.is_modal)))
        {
          mypageRootMenu.IsPush = true;
          e = hotDealGenerator.DoDisplay(info);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          while (info.is_modal || Singleton<PopupManager>.GetInstance().isOpen)
            yield return (object) null;
        }
      }
    }
  }

  private void StartBeginnersQuestAdvice01()
  {
    Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("2020_home2_tutorial01", new Dictionary<string, Func<Transform, UIButton>>()
    {
      {
        "chapter_home_Quest",
        (Func<Transform, UIButton>) (root => ((Component) root.GetChildInFind("Bottom")).GetComponentInChildren<UIButton>())
      }
    }, (Action) (() =>
    {
      Singleton<CommonRoot>.GetInstance().GetHeavenCommonFooter().onButtonOpenQuest();
      this.StartBeginnersQuestAdvice02();
    }));
  }

  private void StartBeginnersQuestAdvice02()
  {
    Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("2020_home2_tutorial02", new Dictionary<string, Func<Transform, UIButton>>()
    {
      {
        "chapter_home_Quest_Event",
        (Func<Transform, UIButton>) (root => ((Component) root.GetChildInFind("Bottom")).GetComponentInChildren<UIButton>())
      }
    }, (Action) (() => Singleton<CommonRoot>.GetInstance().GetHeavenCommonFooter().onButtonOpenEventQuest()));
  }

  public enum Mode
  {
    NONE,
    STORY,
    GUILD,
  }

  public enum GuildMenuMode
  {
    FACILITY,
    RING,
  }
}
