// Decompiled with JetBrains decompiler
// Type: NGSceneBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class NGSceneBase : MonoBehaviour
{
  public string sceneName;
  public bool isTweenFinished;
  [SerializeField]
  protected NGMenuBase menuBase;
  protected NGMenuBase[] menuBases;
  public NGSceneBase.LockLayout lockLayout;
  private int? revisionIsSea_;
  public bool isActiveHeader = true;
  public CommonRoot.HeaderType headerType = CommonRoot.HeaderType.Keep;
  public bool isActiveFooter = true;
  public CommonRoot.FooterType footerType = CommonRoot.FooterType.Keep;
  public bool isActiveBackground = true;
  public bool continueBackground;
  public GameObject backgroundPrefab;
  public float tweenTimeoutTime;
  public NGSceneBase.GuildChatDisplayingStatus currentSceneGuildChatDisplayingStatus = NGSceneBase.GuildChatDisplayingStatus.Closed;
  [SerializeField]
  private bool isUseDLC = true;
  [SerializeField]
  private bool isBackScene = true;
  [SerializeField]
  [Tooltip("CommonFooter等汎用メニューからシーン遷移した後に戻って来たく無いシーンは外す")]
  private bool topGlobalBack = true;
  [SerializeField]
  protected string bgmName = "bgm001";
  [SerializeField]
  protected string bgmFile;
  public float bgmFadeDuration = 3f;
  public bool isDontAutoDestroy;
  protected UITweener[] tweens;
  public bool isAlphaActive;
  [SerializeField]
  [Tooltip("Tween再生をActiveなGameObjectに限定する")]
  protected bool isPlayTweensOnlyActiveObject;

  public bool IsPush
  {
    get
    {
      bool isPush = false;
      if (Object.op_Inequality((Object) this.menuBase, (Object) null))
        isPush |= this.menuBase.IsPush;
      if (this.menuBases != null)
        isPush |= ((IEnumerable<NGMenuBase>) this.menuBases).Any<NGMenuBase>((Func<NGMenuBase, bool>) (x => x.IsPush));
      return isPush;
    }
    set
    {
      if (Object.op_Inequality((Object) this.menuBase, (Object) null))
        this.menuBase.IsPush = value;
      if (this.menuBases == null)
        return;
      for (int index = 0; index < this.menuBases.Length; ++index)
        this.menuBases[index].IsPush = value;
    }
  }

  public bool stackIsSea { get; set; }

  public int revisionIsSea
  {
    get => !this.revisionIsSea_.HasValue ? 0 : this.revisionIsSea_.Value;
    set => this.revisionIsSea_ = new int?(value);
  }

  public bool IsModifiedIsSea
  {
    get
    {
      if (!this.revisionIsSea_.HasValue)
        return true;
      int revisionIsSea1 = Singleton<NGGameDataManager>.GetInstance().revisionIsSea;
      int? revisionIsSea2 = this.revisionIsSea_;
      int valueOrDefault = revisionIsSea2.GetValueOrDefault();
      return !(revisionIsSea1 == valueOrDefault & revisionIsSea2.HasValue);
    }
  }

  public bool checkIsUseDLC() => this.isUseDLC;

  public bool IsBackScene => this.isBackScene;

  public bool IsTopGlobalBack => this.topGlobalBack;

  public virtual bool needGarbageCollectionOnLoaded => true;

  private void Awake() => Singleton<NGSceneManager>.GetInstance().onChangeSceneAwake(this);

  public virtual IEnumerator Start()
  {
    yield break;
  }

  public void onTweenFinished() => this.isTweenFinished = true;

  protected UITweener[] getTweeners()
  {
    if (this.tweens == null)
      this.tweens = NGTween.findTweeners(((Component) this).gameObject, true);
    if (!this.isPlayTweensOnlyActiveObject)
      return this.tweens;
    List<UITweener> uiTweenerList = new List<UITweener>(this.tweens.Length);
    GameObject gameObject = (GameObject) null;
    bool flag = false;
    for (int index = 0; index < this.tweens.Length; ++index)
    {
      UITweener tween = this.tweens[index];
      if (!Object.op_Equality((Object) tween, (Object) null) && !Object.op_Equality((Object) ((Component) tween).gameObject, (Object) null))
      {
        if (Object.op_Inequality((Object) gameObject, (Object) ((Component) tween).gameObject))
        {
          gameObject = ((Component) tween).gameObject;
          flag = gameObject.activeInHierarchy;
        }
        if (flag)
          uiTweenerList.Add(tween);
      }
    }
    return uiTweenerList.ToArray();
  }

  public void startTweens()
  {
    UITweener[] tweens = this.getTweeners();
    bool flag1 = NGTween.playTweens(tweens, NGTween.Kind.START_END);
    bool isTweensError = NGTween.isTweensError;
    bool flag2 = NGTween.playTweens(tweens, NGTween.Kind.START) | flag1;
    if (NGTween.isTweensError | isTweensError)
    {
      this.tweens = (UITweener[]) null;
      tweens = (UITweener[]) null;
    }
    this.isTweenFinished = !flag2;
    this.tweenTimeoutTime = 0.3f;
    if (this.isTweenFinished || tweens == null)
      return;
    foreach (UITweener uiTweener in tweens)
    {
      int num1 = Mathf.Abs(uiTweener.tweenGroup);
      if (uiTweener.style == null && ((Component) uiTweener).gameObject.activeInHierarchy && (num1 == 11 || num1 == 12))
      {
        float num2 = uiTweener.duration + uiTweener.delay;
        if ((double) this.tweenTimeoutTime < (double) num2)
          this.tweenTimeoutTime = num2;
      }
    }
  }

  public IEnumerator setupCommonRoot()
  {
    CommonRoot root = Singleton<CommonRoot>.GetInstance();
    if (this.isActiveBackground && !this.continueBackground)
    {
      IEnumerator e = root.setBackgroundAsync(this.backgroundPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    root.isActiveBackground = this.isActiveBackground;
    if (this.headerType == CommonRoot.HeaderType.NormalOrSea)
      root.headerType = !Singleton<NGGameDataManager>.GetInstance().IsSea ? CommonRoot.HeaderType.Normal : CommonRoot.HeaderType.Sea;
    else if (this.headerType != CommonRoot.HeaderType.Keep)
      root.headerType = this.headerType;
    if (this.footerType != CommonRoot.FooterType.Keep)
      root.footerType = this.footerType;
    root.isActiveHeader = this.isActiveHeader;
    root.isActiveFooter = this.isActiveFooter;
  }

  public IEnumerator PlayBGM()
  {
    if (!string.IsNullOrEmpty(this.bgmName))
    {
      NGSoundManager sm = Singleton<NGSoundManager>.GetInstance();
      if (!Singleton<NGGameDataManager>.GetInstance().IsEarth && this.bgmName.Equals("bgm001") && string.IsNullOrEmpty(this.bgmFile))
      {
        IEnumerator e = ServerTime.WaitSync();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        CommonMypageSetting commonMypageSetting = ((IEnumerable<CommonMypageSetting>) MasterData.CommonMypageSettingList).FirstOrDefault<CommonMypageSetting>((Func<CommonMypageSetting, bool>) (x =>
        {
          DateTime dateTime1 = ServerTime.NowAppTime();
          DateTime? nullable = x.start_at;
          if ((nullable.HasValue ? (dateTime1 >= nullable.GetValueOrDefault() ? 1 : 0) : 0) == 0)
            return false;
          DateTime dateTime2 = ServerTime.NowAppTime();
          nullable = x.end_at;
          return nullable.HasValue && dateTime2 < nullable.GetValueOrDefault();
        }));
        if (commonMypageSetting != null)
        {
          this.bgmFile = commonMypageSetting.bgm_file;
          this.bgmName = commonMypageSetting.bgm_name;
        }
      }
      if (string.IsNullOrEmpty(this.bgmFile))
        sm.playBGMWithCrossFade(this.bgmName, this.bgmFadeDuration);
      else
        sm.PlayBgmFile(this.bgmFile, this.bgmName, fadeInTime: this.bgmFadeDuration, fadeOutTime: this.bgmFadeDuration);
      sm = (NGSoundManager) null;
    }
  }

  public void endTweens()
  {
    UITweener[] tweens = this.getTweeners();
    bool flag1 = NGTween.playTweens(tweens, NGTween.Kind.START_END, true);
    bool isTweensError = NGTween.isTweensError;
    bool flag2 = NGTween.playTweens(tweens, NGTween.Kind.END) | flag1;
    if (NGTween.isTweensError | isTweensError)
    {
      this.tweens = (UITweener[]) null;
      tweens = (UITweener[]) null;
    }
    this.isTweenFinished = !flag2;
    this.tweenTimeoutTime = 0.3f;
    if (this.isTweenFinished || tweens == null)
      return;
    foreach (UITweener uiTweener in tweens)
    {
      int num1 = Mathf.Abs(uiTweener.tweenGroup);
      if (uiTweener.style == null && ((Component) uiTweener).gameObject.activeInHierarchy && (num1 == 11 || num1 == 13))
      {
        float num2 = uiTweener.duration + uiTweener.delay;
        if ((double) this.tweenTimeoutTime < (double) num2)
          this.tweenTimeoutTime = num2;
      }
    }
  }

  public virtual void onSceneInitialized()
  {
    if (!Object.op_Inequality((Object) Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent(), (Object) null))
      return;
    Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().headerChat.SetNewIcon();
    if (Singleton<NGGameDataManager>.GetInstance().chatDataList.Count > 0)
      Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().headerChat.SetText(Singleton<NGGameDataManager>.GetInstance().chatDataList[0]);
    Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().headerChat.TextScrollStart();
  }

  public virtual void onEndScene()
  {
  }

  public virtual List<string> createResourceLoadList() => (List<string>) null;

  public virtual bool createResourceLoadListConfirmDLC() => false;

  public virtual IEnumerator onInitSceneAsync()
  {
    yield break;
  }

  public virtual IEnumerator onEndSceneAsync()
  {
    yield break;
  }

  public virtual IEnumerator onDestroySceneAsync()
  {
    yield break;
  }

  public Coroutine StartCoroutine(IEnumerator e)
  {
    return Singleton<NGSceneManager>.GetInstance().StartCoroutine(e);
  }

  public void SetupGuildChatForNextScene()
  {
    GuildChatManager guildChatManager = Singleton<CommonRoot>.GetInstance().guildChatManager;
    switch (this.currentSceneGuildChatDisplayingStatus)
    {
      case NGSceneBase.GuildChatDisplayingStatus.Opened:
        if (guildChatManager.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.NotOpened)
          guildChatManager.OpenGuildChat();
        else if (guildChatManager.GetGuildChatPaused())
          guildChatManager.ResumeAndShowGuildChat();
        guildChatManager.RefreshMessageItemIcon();
        if (!Persist.guildSetting.Exists)
          break;
        GuildUtil.setBadgeState(GuildUtil.GuildBadgeInfoType.postNewChat, false);
        Persist.guildSetting.Flush();
        break;
      case NGSceneBase.GuildChatDisplayingStatus.Paused:
        if (guildChatManager.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.NotOpened)
          break;
        guildChatManager.PauseAndHideGuildChat();
        break;
      case NGSceneBase.GuildChatDisplayingStatus.Closed:
        if (guildChatManager.GetCurrentGuildChatStatus() == GuildChatManager.GuildChatStatus.NotOpened)
          break;
        guildChatManager.CloseGuildChat();
        break;
      case NGSceneBase.GuildChatDisplayingStatus.Customized:
        Debug.LogError((object) "The status of guild chat is not initialized!!!");
        break;
    }
  }

  public void SetupGuildChatAfterEndScene()
  {
    GuildChatManager guildChatManager = Singleton<CommonRoot>.GetInstance().guildChatManager;
    if (guildChatManager.GetCurrentGuildChatStatus() != GuildChatManager.GuildChatStatus.DetailedView)
      return;
    guildChatManager.CloseDetailedChat();
  }

  protected IEnumerator SetupMatchedBackground(string bgName)
  {
    CommonRoot root = Singleton<CommonRoot>.GetInstance();
    this.continueBackground = true;
    if (!root.getCommonBackground().GetBgPrefabName().Equals(bgName))
    {
      Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/" + bgName).Load<GameObject>();
      IEnumerator e = bgF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) bgF.Result, (Object) null))
        this.backgroundPrefab = bgF.Result;
      e = root.setBackgroundAsync(this.backgroundPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public enum LockLayout
  {
    None,
    Heaven,
    ResetHeaven,
    ResetSea,
    ResetColosseum,
  }

  public enum GuildChatDisplayingStatus
  {
    Opened,
    Paused,
    Closed,
    NotChanged,
    Customized,
  }
}
