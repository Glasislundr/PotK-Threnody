// Decompiled with JetBrains decompiler
// Type: Startup00014MakeupMonthly
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
public class Startup00014MakeupMonthly : Startup00014Menu
{
  [SerializeField]
  private Animator mBonusRootAnim;
  [SerializeField]
  private UI2DSprite mBackground;
  [SerializeField]
  private UIPanel mEffectPanel;
  [Space(8f)]
  [SerializeField]
  private Transform mNaviCharaAnchor;
  [SerializeField]
  private UILabel mNaviMsgLbl;
  [SerializeField]
  private GameObject mNaviMsgNextArrow;
  [Space(8f)]
  [SerializeField]
  private Animator mRewardIconEffectAnim;
  [SerializeField]
  private CreateIconObject mTodayRewardIcon;
  [SerializeField]
  private CreateIconObject mMakeupRewardIcon;
  [Space(8f)]
  [SerializeField]
  private UILabel mMakeupIconDayLbl;
  [SerializeField]
  private UILabel mMakeupRestLbl;
  [SerializeField]
  private UILabel mLoginDaysLbl;
  [Space(8f)]
  [SerializeField]
  private GameObject mTopParts;
  [SerializeField]
  private float mTopPartsMenuPosY;
  [Space(8f)]
  [SerializeField]
  private NGxScroll mScroll;
  [SerializeField]
  private GameObject mScrollViewTop;
  [SerializeField]
  private Vector3 mLoginPosY;
  [SerializeField]
  private Vector3 mMenuPosY;
  [SerializeField]
  private AnchorTargetAdjustment2 mBottomAnchorAdjuster;
  [Space(8f)]
  [SerializeField]
  private UIButton mNextButton;
  [SerializeField]
  private UISprite mBackBtnSprite;
  [SerializeField]
  private UILabel mTitleLbl;
  [SerializeField]
  private UIButton mHelpButton;
  private int mLoginDay;
  private int mMakeupableCount;
  private int mMakeupableMax;
  private List<int> mMakeupableDays;
  private bool mIsLastDay;
  private NGxFaceSprite mNaviFace;
  private Startup00014MakeupRewardIcon[] mRewardIconList;
  private GameObject mGetMarkEffect;
  private GameObject mNextMarkEffect;
  private GameObject mStampInstance;
  private List<GameObject> mDestroyObjList = new List<GameObject>();
  private int mStepCount;
  private string[] mNaviMessageData;
  private string[] mNaviFaceData;
  private KeyValuePair<string, string>[] mNaviVoiceData;
  private bool mIsStamping;
  private bool mGoHelp;

  public bool isStartupScene => Object.op_Inequality((Object) this.Parent, (Object) null);

  public bool StepLock { set; get; }

  private bool isMakeupable => this.mMakeupableCount > 0 && this.mMakeupableDays.Count > 0;

  public override IEnumerator InitSceneAsync(PlayerLoginBonus loginBonus)
  {
    Startup00014MakeupMonthly startup00014MakeupMonthly = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    startup00014MakeupMonthly.IsPush = true;
    ((UIButtonColor) startup00014MakeupMonthly.mNextButton).isEnabled = false;
    startup00014MakeupMonthly.AdjustScrollArea(startup00014MakeupMonthly.mLoginPosY);
    IEnumerator e = startup00014MakeupMonthly.InitSceneAsync(loginBonus._loginbonus, loginBonus.received_reward_days, loginBonus.remain_fill_count, loginBonus.max_fill_count, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((UIButtonColor) startup00014MakeupMonthly.mNextButton).isEnabled = true;
    startup00014MakeupMonthly.StartCoroutine(startup00014MakeupMonthly.PlayEffect());
    startup00014MakeupMonthly.StartCoroutine(startup00014MakeupMonthly.IsPushOff());
  }

  public IEnumerator InitSceneAsync(WebAPI.Response.LoginbonusTop loginBonus)
  {
    Startup00014MakeupMonthly startup00014MakeupMonthly = this;
    startup00014MakeupMonthly.IsPush = true;
    ((UIButtonColor) startup00014MakeupMonthly.mNextButton).isEnabled = false;
    startup00014MakeupMonthly.AdjustTopParts(startup00014MakeupMonthly.mTopPartsMenuPosY);
    startup00014MakeupMonthly.AdjustScrollArea(startup00014MakeupMonthly.mMenuPosY);
    IEnumerator e = startup00014MakeupMonthly.InitSceneAsync(loginBonus.login_bonus_id, loginBonus.received_reward_days, loginBonus.remain_fill_count, loginBonus.max_fill_count, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (startup00014MakeupMonthly.isMakeupable)
    {
      startup00014MakeupMonthly.mRewardIconEffectAnim.Play("LoginBonus_MakeUp_Wait_anim");
      e = startup00014MakeupMonthly.ChangeNaviState(1, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      ((Component) startup00014MakeupMonthly.mRewardIconEffectAnim).gameObject.SetActive(false);
      e = startup00014MakeupMonthly.ChangeNaviState(2, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    startup00014MakeupMonthly.mNaviMsgNextArrow.SetActive(false);
    startup00014MakeupMonthly.mBonusRootAnim.Play("LoginBonus_In_anim");
    startup00014MakeupMonthly.StartCoroutine(startup00014MakeupMonthly.IsPushOff());
  }

  private IEnumerator InitSceneAsync(
    int loginBonusId,
    int[] loginDays,
    int fillRest,
    int fillMax,
    bool enableAnime)
  {
    this.mLoginDay = ((IEnumerable<int>) loginDays).Last<int>();
    this.mMakeupableDays = new List<int>();
    for (int i = 1; i < this.mLoginDay; i++)
    {
      if (Array.FindIndex<int>(loginDays, (Predicate<int>) (x => x == i)) == -1)
        this.mMakeupableDays.Add(i);
    }
    LoginbonusLoginbonus loginbonusLoginbonu = MasterData.LoginbonusLoginbonus[loginBonusId];
    LoginbonusReward[] rewardList = ((IEnumerable<LoginbonusReward>) MasterData.LoginbonusRewardList).Where<LoginbonusReward>((Func<LoginbonusReward, bool>) (x => x.loginbonus_LoginbonusLoginbonus == loginBonusId)).OrderBy<LoginbonusReward, int>((Func<LoginbonusReward, int>) (x => x.number)).ToArray<LoginbonusReward>();
    LoginbonusReward loginReward = rewardList[this.mLoginDay - 1];
    LoginbonusReward makeupReward = this.mMakeupableDays.Count > 0 ? rewardList[this.mMakeupableDays[0] - 1] : (LoginbonusReward) null;
    this.mIsLastDay = rewardList.Length == this.mLoginDay;
    this.SetTitle(loginbonusLoginbonu.name);
    this.SetMakeupableCount(fillRest, fillMax);
    this.mLoginDaysLbl.SetTextLocalize(string.Format("{0}回", (object) (loginDays.Length - (fillMax - fillRest))));
    IEnumerator e = this.CreateBackground(loginReward.back_ground);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.CreateNaviChara(loginReward);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.LoadIconEffects();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (makeupReward != null)
    {
      e = this.CreateMakeupRewardIcon(makeupReward);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (enableAnime)
    {
      e = this.CreateTodayRewardIcon(loginReward);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    e = this.CreateRewardListIcons(rewardList, enableAnime);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) ((Component) this.mScroll).transform.parent).gameObject.SetActive(false);
    if (Object.op_Inequality((Object) this.mHelpButton, (Object) null) && Persist.integralNoaTutorial.Data.beginnersQuest)
      ((Component) this.mHelpButton).gameObject.SetActive(false);
  }

  private void SetTitle(string title)
  {
    this.mTitleLbl.SetTextLocalize(title);
    if (this.isStartupScene)
      this.mBackBtnSprite.ChangeSprite("slc_button_text_close_34pt.png__GUI__common__common_prefab");
    else
      this.mBackBtnSprite.ChangeSprite("slc_button_text_back_34pt.png__GUI__common__common_prefab");
  }

  private void SetMakeupableCount(int rest, int max)
  {
    this.mMakeupableCount = rest;
    this.mMakeupableMax = max;
    this.mMakeupRestLbl.SetTextLocalize(string.Format("{0}/{1}", (object) this.mMakeupableCount, (object) this.mMakeupableMax));
  }

  public void OnStartConfirmScene() => this.mBottomAnchorAdjuster.LateAdjustDirty = true;

  private void AdjustTopParts(float posY)
  {
    Vector3 localPosition = this.mTopParts.transform.localPosition;
    this.mTopParts.transform.localPosition = new Vector3(localPosition.x, posY, localPosition.z);
  }

  private void AdjustScrollArea(Vector3 offsets)
  {
    Vector3 localPosition = this.mScrollViewTop.transform.localPosition;
    this.mScrollViewTop.transform.localPosition = new Vector3(localPosition.x, offsets.x, localPosition.z);
  }

  private IEnumerator CreateBackground(string name)
  {
    bool isBgChanged = false;
    if (!string.IsNullOrEmpty(name))
    {
      Future<Sprite> loadFt = new ResourceObject("Prefabs/BackGround/" + name).Load<Sprite>();
      IEnumerator e = loadFt.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Inequality((Object) loadFt.Result, (Object) null))
      {
        this.mBackground.sprite2D = loadFt.Result;
        ((UIWidget) this.mBackground).width = ((Texture) loadFt.Result.texture).width;
        ((UIWidget) this.mBackground).height = ((Texture) loadFt.Result.texture).height;
        isBgChanged = true;
      }
      loadFt = (Future<Sprite>) null;
    }
    if (isBgChanged)
      Singleton<CommonRoot>.GetInstance().isActiveBackground = false;
    else
      ((Component) this.mBackground).gameObject.SetActive(false);
  }

  private IEnumerator CreateNaviChara(LoginbonusReward reward)
  {
    Startup00014MakeupMonthly startup00014MakeupMonthly = this;
    int charaId = reward.character_id;
    int unitStoryId = charaId;
    int jobId = reward.job_id;
    float posX = reward.character_x;
    float posY = reward.character_y;
    float scale = reward.character_scale;
    Future<GameObject> naviF = (Future<GameObject>) null;
    Future<Sprite> spriteF = (Future<Sprite>) null;
    UnitUnit unitData = (UnitUnit) null;
    IEnumerator e;
    if (charaId <= 999)
    {
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<string>) Singleton<ResourceManager>.GetInstance().PathsFromMobUnit(charaId), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      naviF = MobUnits.LoadStory(charaId);
      spriteF = MobUnits.LoadSpriteLarge(charaId);
    }
    else
    {
      unitData = MasterData.UnitUnit[charaId];
      unitStoryId = unitData.resource_reference_unit_id_UnitUnit;
      e = OnDemandDownload.WaitLoadUnitResource((IEnumerable<UnitUnit>) new UnitUnit[1]
      {
        unitData
      }, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      naviF = unitData.LoadStory();
      spriteF = jobId == 0 ? unitData.LoadSpriteLarge() : unitData.LoadSpriteLarge(jobId, 1f);
    }
    e = naviF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject naviChara = naviF.Result;
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite mainSprite = spriteF.Result;
    Future<Texture2D> maskFt = new ResourceObject("Prefabs/startup000_14/image/unit_large_mask_login").Load<Texture2D>();
    e = maskFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = maskFt.Result;
    GameObject gameObject = naviChara.Clone(startup00014MakeupMonthly.mNaviCharaAnchor);
    gameObject.transform.localScale = new Vector3(scale, scale, 1f);
    gameObject.transform.localPosition = new Vector3(posX, posY, 0.0f);
    gameObject.GetComponent<UIWidget>().depth = 3;
    NGxMaskSpriteWithScale component = gameObject.GetComponent<NGxMaskSpriteWithScale>();
    if (charaId > 999)
      component.isTopFit = false;
    startup00014MakeupMonthly.ApplyUnitStoryTransform(ref component, unitStoryId, jobId);
    component.MainUI2DSprite.sprite2D = mainSprite;
    component.maskTexture = result;
    startup00014MakeupMonthly.mNaviFace = gameObject.GetComponent<NGxFaceSprite>();
    if (Object.op_Inequality((Object) startup00014MakeupMonthly.mNaviFace, (Object) null))
    {
      startup00014MakeupMonthly.mNaviFace.UnitID = charaId;
      if (jobId != 0)
        startup00014MakeupMonthly.mNaviFace.ExtID = new int?(jobId);
    }
    startup00014MakeupMonthly.CreateNaviMessageData(reward.reward_message, reward.next_reward_message, reward.no_makeup_message);
    startup00014MakeupMonthly.CreateNaviFaceData(reward.face1, reward.face2, reward.face3);
    startup00014MakeupMonthly.CreateNaviVoiceData(charaId, unitData, new string[3]
    {
      reward.que_name1,
      reward.que_name2,
      reward.que_name3
    });
  }

  private void ApplyUnitStoryTransform(
    ref NGxMaskSpriteWithScale naviObj,
    int unitStoryId,
    int jobId)
  {
    UnitExtensionStory unitExtensionStory;
    if (jobId != 0 && (unitExtensionStory = Array.Find<UnitExtensionStory>(MasterData.UnitExtensionStoryList, (Predicate<UnitExtensionStory>) (s => s.unit == unitStoryId && s.job_metamor_id == jobId))) != null)
    {
      naviObj.scale = unitExtensionStory.story_texture_scale;
      naviObj.xOffsetPixel = unitExtensionStory.story_texture_x;
      naviObj.yOffsetPixel = unitExtensionStory.story_texture_y;
    }
    else
    {
      UnitUnitStory unitUnitStory = (UnitUnitStory) null;
      if (!MasterData.UnitUnitStory.TryGetValue(unitStoryId, out unitUnitStory))
        return;
      naviObj.scale = unitUnitStory.story_texture_scale;
      naviObj.xOffsetPixel = unitUnitStory.story_texture_x;
      naviObj.yOffsetPixel = unitUnitStory.story_texture_y;
    }
  }

  private void CreateNaviMessageData(
    string todayMessage,
    string makeupMessage,
    string noMakeupMessage)
  {
    this.mNaviMessageData = new string[3]
    {
      todayMessage,
      makeupMessage,
      noMakeupMessage
    };
  }

  private void CreateNaviFaceData(string face1, string face2, string face3)
  {
    this.mNaviFaceData = new string[3]
    {
      face1,
      face2,
      face3
    };
  }

  private void CreateNaviVoiceData(int charaId, UnitUnit unitData, string[] que_name)
  {
    this.mNaviVoiceData = new KeyValuePair<string, string>[3];
    if (charaId == 0)
      que_name = new string[3]
      {
        "durin_navi_0062,VO_9999",
        "durin_0005,VO_9999",
        string.Empty
      };
    for (int index = 0; index < 3; ++index)
    {
      if (!string.IsNullOrEmpty(que_name[index]))
      {
        string[] strArray = que_name[index].Split(',');
        if (strArray.Length > 1)
          this.mNaviVoiceData[index] = new KeyValuePair<string, string>(strArray[1], strArray[0]);
        else if (strArray.Length != 0)
        {
          if (charaId <= 999)
            this.mNaviVoiceData[index] = new KeyValuePair<string, string>("VO_" + (object) charaId, strArray[0]);
          else if (unitData.unitVoicePattern != null)
            this.mNaviVoiceData[index] = new KeyValuePair<string, string>(unitData.unitVoicePattern.file_name, strArray[0]);
        }
      }
    }
  }

  private IEnumerator CreateTodayRewardIcon(LoginbonusReward reward)
  {
    IEnumerator e = this.mTodayRewardIcon.CreateThumbnail(reward.reward_type, reward.reward_id, reward.reward_quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator CreateMakeupRewardIcon(LoginbonusReward reward)
  {
    GameObject icon = this.mMakeupRewardIcon.GetIcon();
    if (Object.op_Inequality((Object) icon, (Object) null))
      Object.Destroy((Object) icon);
    IEnumerator e = this.mMakeupRewardIcon.CreateThumbnail(reward.reward_type, reward.reward_id, reward.reward_quantity, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mMakeupIconDayLbl.SetTextLocalize(string.Format("▼ {0}日目の埋め合わせ ▼", (object) reward.number));
  }

  private IEnumerator CreateRewardListIcons(LoginbonusReward[] rewardList, bool enableAnime)
  {
    Startup00014MakeupMonthly parent = this;
    Future<GameObject> iconFt = new ResourceObject("Prefabs/startup000_14/dir_Thum01").Load<GameObject>();
    IEnumerator e = iconFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject icon = iconFt.Result;
    // ISSUE: reference to a compiler-generated method
    Func<LoginbonusReward, int, bool> isGotten = new Func<LoginbonusReward, int, bool>(parent.\u003CCreateRewardListIcons\u003Eb__64_0);
    parent.mRewardIconList = new Startup00014MakeupRewardIcon[rewardList.Length];
    for (int i = 0; i < 31; ++i)
    {
      Startup00014MakeupRewardIcon rewardIcon = icon.CloneAndGetComponent<Startup00014MakeupRewardIcon>(((Component) parent.mScroll.grid).gameObject);
      if (i < rewardList.Length)
      {
        LoginbonusReward reward = rewardList[i];
        e = rewardIcon.Initialize(parent, reward, !enableAnime && reward.number == parent.mLoginDay, !enableAnime && reward.number == parent.mLoginDay + 1, !enableAnime ? isGotten(reward, parent.mLoginDay) : isGotten(reward, parent.mLoginDay - 1), parent.mMakeupableCount > 0 && parent.mMakeupableDays.FindIndex((Predicate<int>) (x => x == reward.number)) != -1);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        parent.mRewardIconList[i] = rewardIcon;
      }
      else
        rewardIcon.InitializeEmpty();
      rewardIcon = (Startup00014MakeupRewardIcon) null;
    }
    float num1 = Mathf.Ceil((float) parent.mLoginDay / (float) parent.mScroll.grid.maxPerLine);
    float num2 = (double) num1 < 3.0 ? 0.0f : ((double) num1 > 3.0 ? 1f : 0.5f);
    parent.mScroll.ResolvePosition(new Vector2(0.0f, num2));
  }

  private IEnumerator LoadIconEffects()
  {
    Future<GameObject> getEffectFt = new ResourceObject("Prefabs/startup000_14/dir_Get_Stamp").Load<GameObject>();
    IEnumerator e = getEffectFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mGetMarkEffect = getEffectFt.Result;
    Future<GameObject> nextEffectFt = new ResourceObject("Prefabs/startup000_14/dir_Next_anim").Load<GameObject>();
    e = nextEffectFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.mNextMarkEffect = nextEffectFt.Result;
  }

  private IEnumerator PlayEffect()
  {
    Singleton<NGSoundManager>.GetInstance();
    IEnumerator e;
    switch (this.mStepCount)
    {
      case 0:
        this.mBonusRootAnim.Play("LoginBonus_In_anim");
        this.mRewardIconEffectAnim.Play("LoginBonus_Get_anim");
        e = this.ChangeNaviState(0, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        while (Object.op_Equality((Object) this.mNaviMsgNextArrow.GetComponent<TweenPosition>(), (Object) null))
          yield return (object) null;
        ((Behaviour) this.mNaviMsgNextArrow.GetComponent<TweenPosition>()).enabled = true;
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
        break;
      case 1:
        this.StepLock = true;
        if (Object.op_Equality((Object) this.mStampInstance, (Object) null))
          this.Stamp();
        while (this.mIsStamping)
          yield return (object) null;
        this.mStampInstance.SetActive(false);
        ((UIButtonColor) this.mNextButton).isEnabled = false;
        if (this.isMakeupable)
        {
          this.mRewardIconEffectAnim.SetTrigger("isMakeUp");
          e = this.ChangeNaviState(1, true);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
        {
          ((Component) this.mRewardIconEffectAnim).gameObject.SetActive(false);
          e = this.ChangeNaviState(2, true);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        this.mNaviMsgNextArrow.SetActive(false);
        break;
    }
  }

  private IEnumerator ChangeNaviState(int index, bool playVoice)
  {
    if (Object.op_Inequality((Object) this.mNaviFace, (Object) null) && !string.IsNullOrEmpty(this.mNaviFaceData[index]))
    {
      IEnumerator e = this.mNaviFace.ChangeFace(this.mNaviFaceData[index]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (!string.IsNullOrEmpty(this.mNaviMessageData[index]) && this.mNaviMessageData[index].Contains("{0}"))
      this.mNaviMsgLbl.SetTextLocalize(string.Format(this.mNaviMessageData[index], (object) this.mMakeupableCount));
    else
      this.mNaviMsgLbl.SetTextLocalize(this.mNaviMessageData[index]);
    if (playVoice && !this.mNaviVoiceData[index].Equals((object) new KeyValuePair<string, string>()))
    {
      NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
      instance.stopVoice();
      instance.playVoiceByStringID(this.mNaviVoiceData[index].Key, this.mNaviVoiceData[index].Value);
    }
  }

  public void Stamp()
  {
    this.mRewardIconList[this.mLoginDay - 1].SetTodayMark(true);
    this.Stamp(this.mLoginDay - 1);
    if (this.mIsLastDay)
      return;
    this.mNextMarkEffect.Clone(((Component) this.mRewardIconList[this.mLoginDay]).transform);
  }

  private void Stamp(int index)
  {
    this.mIsStamping = true;
    Vector3 position = ((Component) this.mRewardIconList[index]).transform.position;
    this.mStampInstance = this.mGetMarkEffect.Clone(((Component) this.mEffectPanel).transform);
    this.mStampInstance.transform.position = position;
    ((UITweener) this.mStampInstance.GetComponentInChildren<TweenScale>()).SetOnFinished((EventDelegate.Callback) (() =>
    {
      Singleton<NGSoundManager>.GetInstance().PlaySe("SE_1033");
      this.mRewardIconList[index].SetGetMark(true);
      this.mIsStamping = false;
    }));
    this.mDestroyObjList.Add(this.mStampInstance);
  }

  public void OnNextStep()
  {
    if (this.StepLock || this.IsPushAndSet())
      return;
    ++this.mStepCount;
    this.StartCoroutine(this.PlayEffect());
    this.StartCoroutine(this.IsPushOff());
  }

  public void OnHelpButton()
  {
    if (this.IsPushAndSet())
      return;
    if (this.isStartupScene)
      this.Parent.GoHelp = true;
    else
      this.mGoHelp = true;
    this.mBonusRootAnim.SetTrigger("isOut");
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.mBonusRootAnim.SetTrigger("isOut");
  }

  public void EndScene()
  {
    foreach (GameObject mDestroyObj in this.mDestroyObjList)
    {
      mDestroyObj.transform.parent = (Transform) null;
      Object.Destroy((Object) mDestroyObj);
    }
    this.mDestroyObjList.Clear();
    this.mStampInstance = (GameObject) null;
    if (this.isStartupScene)
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else if (this.mGoHelp)
      Help0152Scene.ChangeScene(true, MasterData.HelpCategory[34]);
    else
      this.backScene();
  }

  public void OnBackScene()
  {
    this.mBonusRootAnim.Play("LoginBonus_In_anim");
    this.mGoHelp = false;
    this.StartCoroutine(this.IsPushOff());
  }

  public void OnMakeupButton(LoginbonusReward reward)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenMakeupConfirm(reward));
    this.StartCoroutine(this.IsPushOff());
  }

  private IEnumerator OpenMakeupConfirm(LoginbonusReward reward)
  {
    Future<GameObject> loadFt = new ResourceObject("Prefabs/Popup_Common/popup_WithThum_NoYes_Base").Load<GameObject>();
    IEnumerator e = loadFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = loadFt.Result;
    e = Singleton<PopupManager>.GetInstance().open(result).GetComponent<PopupWithThumNoYes>().Initialize(Consts.GetInstance().POPUP_LOGIN_BONUS_MAKEUP_CONFIRM_TITLE, Consts.GetInstance().POPUP_LOGIN_BONUS_MAKEUP_CONFIRM_MESSAGE, reward.reward_type, reward.reward_id, reward.reward_quantity, (Action) (() => this.StartCoroutine(this.SendMakeup(reward))), (Action) (() =>
    {
      this.IsPush = false;
      Singleton<PopupManager>.GetInstance().dismiss();
    }));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SendMakeup(LoginbonusReward reward)
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    Future<WebAPI.Response.LoginbonusFill> api = WebAPI.LoginbonusFill(reward.ID);
    IEnumerator e = api.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.LoginbonusFill result1 = api.Result;
    if (result1 == null)
    {
      while (Singleton<PopupManager>.GetInstance().ModalWindowIsOpen)
        yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
      if (this.isStartupScene)
        this.EndScene();
      else
        MypageScene.ChangeScene();
    }
    else
    {
      this.mMakeupableDays.Remove(reward.number);
      this.SetMakeupableCount(this.mMakeupableCount - 1, this.mMakeupableMax);
      Singleton<NGGameDataManager>.GetInstance().hasFillableLoginbonus = result1.has_fillable_loginbonus;
      if (this.isMakeupable)
      {
        e = this.CreateMakeupRewardIcon(Array.Find<Startup00014MakeupRewardIcon>(this.mRewardIconList, (Predicate<Startup00014MakeupRewardIcon>) (x => x.GetReward().number == this.mMakeupableDays[0])).GetReward());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = this.ChangeNaviState(1, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        ((Component) this.mRewardIconEffectAnim).gameObject.SetActive(false);
        foreach (Startup00014MakeupRewardIcon mRewardIcon in this.mRewardIconList)
          mRewardIcon.SetMakeupButton(false);
        e = this.ChangeNaviState(2, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      Future<GameObject> loadFt = new ResourceObject("Prefabs/Popup_Common/popup_WithThum_OK_Base").Load<GameObject>();
      e = loadFt.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result2 = loadFt.Result;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      e = Singleton<PopupManager>.GetInstance().open(result2).GetComponent<PopupWithThumOk>().Initialize(Consts.GetInstance().POPUP_LOGIN_BONUS_MAKEUP_TITLE, Consts.GetInstance().POPUP_LOGIN_BONUS_MAKEUP_MESSAGE, reward.reward_type, reward.reward_id, reward.reward_quantity, (Action) (() => this.StartCoroutine(this.WaitPopupCloseAndStamp(reward.number - 1))));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator WaitPopupCloseAndStamp(int index)
  {
    Startup00014MakeupMonthly startup00014MakeupMonthly = this;
    ((UIButtonColor) startup00014MakeupMonthly.mNextButton).isEnabled = true;
    PopupManager popupMgr = Singleton<PopupManager>.GetInstance();
    while (popupMgr.isOpen)
      yield return (object) null;
    startup00014MakeupMonthly.Stamp(index);
    while (startup00014MakeupMonthly.mIsStamping)
      yield return (object) null;
    yield return (object) new WaitForSeconds(1f);
    startup00014MakeupMonthly.mStampInstance.SetActive(false);
    startup00014MakeupMonthly.IsPush = false;
    ((UIButtonColor) startup00014MakeupMonthly.mNextButton).isEnabled = false;
  }
}
