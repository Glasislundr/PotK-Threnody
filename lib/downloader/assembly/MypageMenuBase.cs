// Decompiled with JetBrains decompiler
// Type: MypageMenuBase
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
public abstract class MypageMenuBase : BackButtonMenuBase
{
  private static readonly int INFO_READ_JUDGE_NUM = 3;
  public static readonly int START_TWEENGROUP = 22;
  public static readonly int END_TWEENGROUP = 33;
  public static readonly int START_TWEENGROUP_TOP = 50;
  public static readonly int END_TWEENGROUP_TOP = 51;
  public static readonly int START_TWEEN_GROUP_JOGDIAL = 122;
  public static readonly int END_TWEEN_GROUP_JOGDIAL = 133;
  public static readonly int HEAVEN_OUT_BG_TWEENGROUP = 151;
  public static readonly int HEAVEN_IN_BG_TWEENGROUP = 152;
  public static readonly int EARTH_OUT_BG_TWEENGROUP = 153;
  public static readonly int EARTH_IN_BG_TWEENGROUP = 150;
  public static int LeaderID;
  public static List<int> PlayTouchVoiceList = new List<int>();
  [SerializeField]
  private const float MoviePlayTime = 1.5f;
  [SerializeField]
  protected UIPanel MainPanel;
  [SerializeField]
  protected GameObject NotTouch;
  [SerializeField]
  protected GameObject MainButtonRoot;
  [SerializeField]
  protected Transform CharaAnimAnchor;
  protected NGSoundManager sm;
  protected GameObject CharaAnimObj;
  protected UI2DSprite[] CharaSprites;
  protected UILabel[] CharaName;
  protected UILabel CharaAnimEnglishName;
  protected Animator CharaAnimator;
  protected bool CharaAnimation;
  protected string TweenName = string.Empty;
  protected ulong? CharacterId;
  protected int ButtonCount;
  protected int ButtonTweenFinishedCount;
  protected int NextPlayVoiceIndex;

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

  public abstract IEnumerator InitSceneAsync();

  public abstract IEnumerator onStartSceneAsync(bool isCloudAnim, bool isReservedEventScript);

  public abstract void onEndScene();

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowGameEndPopoup());
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

  protected void InitTween()
  {
    this.ButtonCount = 0;
    this.ButtonTweenFinishedCount = 0;
    this.NotTouch.SetActive(true);
    ((IEnumerable<UITweener>) ((Component) this).GetComponentsInChildren<UITweener>(true)).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.onFinished.Clear();
      ((Behaviour) x).enabled = false;
    }));
  }

  public void PlayTween(int groupID)
  {
    UITweener[] uiTweenerArray = ((Component) this).GetComponentsInChildren<UITweener>();
    if (groupID == MypageMenuBase.START_TWEENGROUP_TOP)
      uiTweenerArray = ((IEnumerable<UITweener>) uiTweenerArray).Where<UITweener>((Func<UITweener, bool>) (x => !((Object) x).name.Equals("icon") && !((Object) x).name.Equals("GearKindIcon(Clone)") && !((Object) x).name.Equals("weapon_type") && !((Object) x).name.Equals("rarity_star") && !((Object) x).name.Equals("hime_type"))).ToArray<UITweener>();
    ((IEnumerable<UITweener>) uiTweenerArray).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      if (x.tweenGroup != groupID)
        return;
      ((Component) x).gameObject.SetActive(true);
      x.ResetToBeginning();
      x.PlayForward();
    }));
  }

  protected void ButtonTweenFinished()
  {
    ++this.ButtonTweenFinishedCount;
    if (this.ButtonCount != this.ButtonTweenFinishedCount)
      return;
    this.AllButtonTweenFinished();
  }

  protected virtual void AllButtonTweenFinished()
  {
    this.NotTouch.SetActive(false);
    this.CharaStartVoice();
  }

  public void ResetTweenDelay()
  {
    ((IEnumerable<UITweener>) ((Component) this).GetComponentsInChildren<UITweener>(true)).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      if (x.tweenGroup != MypageMenuBase.START_TWEENGROUP || (double) x.delay < 1.5)
        return;
      x.delay -= 1.5f;
    }));
  }

  protected virtual void AddStartTweenDelay(float addTime, int groupID)
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

  protected virtual IEnumerator CreateCharcterAnimation()
  {
    MypageMenuBase mypageMenuBase = this;
    Future<GameObject> f = new ResourceObject("Prefabs/mypage/" + (Singleton<NGGameDataManager>.GetInstance().IsEarth ? "CharacterAnimator_ground" : "CharacterAnimator")).Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mypageMenuBase.CharaAnimObj = f.Result.Clone(mypageMenuBase.CharaAnimAnchor);
    CharaAnimationConst component = mypageMenuBase.CharaAnimObj.GetComponent<CharaAnimationConst>();
    mypageMenuBase.CharaName = component.charaName;
    mypageMenuBase.CharaAnimator = component.anim;
    mypageMenuBase.CharaSprites = component.charaSprite;
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<UI2DSprite>) mypageMenuBase.CharaSprites).ForEach<UI2DSprite>(new Action<UI2DSprite>(mypageMenuBase.\u003CCreateCharcterAnimation\u003Eb__43_0));
    mypageMenuBase.CharaAnimEnglishName = component._english_name;
  }

  protected virtual IEnumerator CharcterAnimationSetting(bool isCloudAnim, int tweenID)
  {
    if (Object.op_Inequality((Object) this.CharaAnimObj, (Object) null))
      this.CharaAnimObj.SetActive(false);
    PlayerUnit displayPlayerUnit = this.GetDisplayPlayerUnit();
    UnitUnit unitUnit = displayPlayerUnit.unit;
    ulong charId = this.MakeCharacterId(unitUnit.ID, displayPlayerUnit.job_id);
    if (this.CharacterId.HasValue)
    {
      long num = (long) charId;
      ulong? characterId = this.CharacterId;
      long valueOrDefault = (long) characterId.GetValueOrDefault();
      if (num == valueOrDefault & characterId.HasValue)
        goto label_14;
    }
    Future<Sprite> LeaderF = unitUnit.LoadSpriteLarge(displayPlayerUnit.job_id, 1f);
    IEnumerator e = LeaderF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    for (int index = 0; index < this.CharaSprites.Length; ++index)
      this.CharaSprites[index].sprite2D = LeaderF.Result;
    for (int index = 0; index < this.CharaName.Length; ++index)
      this.CharaName[index].SetTextLocalize(unitUnit.name);
    this.CharacterId = new ulong?(charId);
    LeaderF = (Future<Sprite>) null;
label_14:
    if (unitUnit.ID != MypageMenuBase.LeaderID)
    {
      MypageMenuBase.PlayTouchVoiceList.Clear();
      this.NextPlayVoiceIndex = 0;
      if (unitUnit.unitVoicePattern != null && this.sm.LoadVoiceData(unitUnit.unitVoicePattern.file_name))
      {
        while (!this.sm.LoadedCueSheet(unitUnit.unitVoicePattern.file_name))
          yield return (object) null;
        HashSet<int> self = new HashSet<int>();
        foreach (int enableId in UnitHomeVoicePattern.GetEnableIDList())
        {
          if (this.sm.ExistsCueID(unitUnit.unitVoicePattern.file_name, enableId))
            self.Add(enableId);
        }
        MypageMenuBase.PlayTouchVoiceList = self.Shuffle<int>().ToList<int>();
      }
    }
    float addDelayTime = this.SetCharacterAnimControllerWithGetAddDelayTime(unitUnit.ID != MypageMenuBase.LeaderID && !isCloudAnim, unitUnit.ID);
    if (isCloudAnim)
    {
      Singleton<CommonRoot>.GetInstance().StartBGTween(tweenID);
      this.AddStartTweenDelay(addDelayTime, MypageMenuBase.START_TWEENGROUP_TOP);
      this.AddStartTweenDelay(addDelayTime, MypageMenuBase.START_TWEEN_GROUP_JOGDIAL);
    }
    else
      this.AddStartTweenDelay(addDelayTime, MypageMenuBase.START_TWEENGROUP);
  }

  protected PlayerUnit GetDisplayPlayerUnit()
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

  protected float SetCharacterAnimControllerWithGetAddDelayTime(bool LeaderChange, int id)
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

  public virtual void CharaAnimProc()
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
    if (!Object.op_Implicit((Object) component3))
      return;
    component3.SetTrigger(this.TweenName);
  }

  private void CharaStartVoice()
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
    this.sm.playVoiceByID(MasterData.UnitUnit[MypageMenuBase.LeaderID].unitVoicePattern, (int) cueIdArray[DateTime.Now.Hour / 4]);
  }

  protected void CharaTouchVoice()
  {
    if (Object.op_Equality((Object) this.sm, (Object) null))
      this.sm = Singleton<NGSoundManager>.GetInstance();
    this.sm.stopVoice();
    if (!MasterData.UnitUnit.ContainsKey(MypageMenuBase.LeaderID))
      return;
    UnitVoicePattern unitVoicePattern = MasterData.UnitUnit[MypageMenuBase.LeaderID].unitVoicePattern;
    if (Random.Range(0, 1000) < 500 && unitVoicePattern != null && this.sm.ExistsCueID(unitVoicePattern.file_name, 90))
    {
      this.sm.playVoiceByID(unitVoicePattern, 90);
    }
    else
    {
      if (MypageMenuBase.PlayTouchVoiceList.Count <= 0 || unitVoicePattern == null)
        return;
      this.sm.playVoiceByID(unitVoicePattern, MypageMenuBase.PlayTouchVoiceList[this.NextPlayVoiceIndex]);
      this.NextPlayVoiceIndex = (this.NextPlayVoiceIndex + 1) % MypageMenuBase.PlayTouchVoiceList.Count;
      if (this.NextPlayVoiceIndex != 0)
        return;
      MypageMenuBase.PlayTouchVoiceList = MypageMenuBase.PlayTouchVoiceList.Shuffle<int>().ToList<int>();
    }
  }

  public virtual IEnumerator CloudAnimationStart()
  {
    MypageMenuBase mypageMenuBase = this;
    Singleton<CommonRoot>.GetInstance().StartBGTween(MypageMenuBase.HEAVEN_OUT_BG_TWEENGROUP);
    mypageMenuBase.PlayTween(MypageMenuBase.END_TWEENGROUP_TOP);
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = Singleton<CommonRoot>.GetInstance().StartCloudAnim(MypageCloudAnim.HeavenOut, MypageCloudAnim.EarthIn, new Action(mypageMenuBase.\u003CCloudAnimationStart\u003Eb__52_0));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void TutorialAdvice() => this.StartCoroutine(this.Advice());

  private IEnumerator Advice()
  {
    MypageMenuBase mypageMenuBase = this;
    UITweener[] tweens = ((Component) mypageMenuBase).GetComponentsInChildren<UITweener>();
    while (((IEnumerable<UITweener>) tweens).Any<UITweener>((Func<UITweener, bool>) (x => ((Component) x).gameObject.activeInHierarchy && x.tweenGroup == MypageMenuBase.START_TWEENGROUP && ((Behaviour) x).enabled)))
    {
      mypageMenuBase.NotTouch.SetActive(true);
      yield return (object) null;
    }
    mypageMenuBase.NotTouch.SetActive(false);
    mypageMenuBase.BackBtnEnable = true;
    if (!Persist.tutorial.Data.IsFinishTutorial())
      Singleton<TutorialRoot>.GetInstance().CurrentAdvise();
    else if (Persist.integralNoaTutorial.Data.beginnersQuest)
    {
      ServerTime.NowAppTime();
      Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("newchapter_home2_tutorial", new Dictionary<string, Func<Transform, UIButton>>()
      {
        {
          "chapter_home2",
          (Func<Transform, UIButton>) (root => (UIButton) ((Component) root.GetChildInFind("Top")).GetComponentInChildren<FloatButton>())
        }
      });
    }
  }
}
