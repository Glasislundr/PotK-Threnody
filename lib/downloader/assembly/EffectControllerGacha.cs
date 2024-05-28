// Decompiled with JetBrains decompiler
// Type: EffectControllerGacha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EffectControllerGacha : EffectController
{
  [SerializeField]
  private List<GameObject> ballBlueList;
  [SerializeField]
  private List<GameObject> ballYellowList;
  [SerializeField]
  private List<GameObject> ballRedList;
  [SerializeField]
  private List<GameObject> ballRainbowList;
  [SerializeField]
  private List<GameObject> ballBlueToRedList;
  [SerializeField]
  private List<GameObject> ballYellowToRedList;
  [SerializeField]
  private List<GameObject> ballBlueToRainbowList;
  [SerializeField]
  private List<GameObject> ballYellowToRainbowList;
  [SerializeField]
  private List<GameObject> ballRedToRainbowList;
  [SerializeField]
  private GameObject ballBlue;
  [SerializeField]
  private GameObject ballYellow;
  [SerializeField]
  private GameObject ballRed;
  [SerializeField]
  private GameObject ballRainbow;
  [SerializeField]
  private Animator animatorBackGroundBlue;
  [SerializeField]
  private Animator animatorBackGroundYellow;
  [SerializeField]
  private Animator animatorBackGroundRed;
  [SerializeField]
  private Animator animatorBackGroundRainbow;
  [SerializeField]
  private Animator animatorBackGroundChangeToRed;
  [SerializeField]
  private Animator animatorBackGroundChangeToRainbow;
  [SerializeField]
  private Animator animatorCameraBlue;
  [SerializeField]
  private Animator animatorCameraYellow;
  [SerializeField]
  private Animator animatorCameraRed;
  [SerializeField]
  private Animator animatorCameraRainbow;
  [SerializeField]
  private Animator animatorCameraChangeToRed;
  [SerializeField]
  private Animator animatorCameraChangeToRainbow;
  [SerializeField]
  private Animator animatorEffectBlue;
  [SerializeField]
  private Animator animatorEffectYellow;
  [SerializeField]
  private Animator animatorEffectRed;
  [SerializeField]
  private Animator animatorEffectRainbow;
  [SerializeField]
  private Animator animatorBlueToRed;
  [SerializeField]
  private Animator animatorYellowToRed;
  [SerializeField]
  private Animator animatorBlueToRainbow;
  [SerializeField]
  private Animator animatorYellowToRainbow;
  [SerializeField]
  private Animator animatorRedToRainbow;
  private GachaResultData.Result[] resultList;
  private Dictionary<GachaDirectionType, EffectControllerGacha.GachaEffect> gachaEffects;
  private int currentIndex;
  private Coroutine loadResourcesCoroutine;
  private bool isSkip;

  public EffectControllerGachaDetail Detail { get; set; }

  public gacha006_effectMenu Menu { get; set; }

  public EffectControllerGacha.STATE State { get; set; }

  private void SetGachaEffect()
  {
    this.gachaEffects = new Dictionary<GachaDirectionType, EffectControllerGacha.GachaEffect>()
    {
      [GachaDirectionType.low] = new EffectControllerGacha.GachaEffect()
      {
        BallList = this.ballBlueList,
        BackGroundAnimator = this.animatorBackGroundBlue,
        CameraAnimator = this.animatorCameraBlue,
        EffectAnimator = this.animatorEffectBlue
      },
      [GachaDirectionType.middle] = new EffectControllerGacha.GachaEffect()
      {
        BallList = this.ballYellowList,
        BackGroundAnimator = this.animatorBackGroundYellow,
        CameraAnimator = this.animatorCameraYellow,
        EffectAnimator = this.animatorEffectYellow
      },
      [GachaDirectionType.high] = new EffectControllerGacha.GachaEffect()
      {
        BallList = this.ballRedList,
        BackGroundAnimator = this.animatorBackGroundRed,
        CameraAnimator = this.animatorCameraRed,
        EffectAnimator = this.animatorEffectRed
      },
      [GachaDirectionType.pickup] = new EffectControllerGacha.GachaEffect()
      {
        BallList = this.ballRainbowList,
        BackGroundAnimator = this.animatorBackGroundRainbow,
        CameraAnimator = this.animatorCameraRainbow,
        EffectAnimator = this.animatorEffectRainbow
      }
    };
  }

  private GachaResultData.Result CurrentResultData => this.resultList[this.currentIndex];

  private GachaDirectionType CurrentDirectionType => this.CurrentResultData.directionType;

  private EffectControllerGacha.GachaEffect CurrentEffect
  {
    get => this.gachaEffects[this.CurrentDirectionType];
  }

  private bool IsSingle => this.resultList.Length == 1;

  private bool HasNextResult => this.resultList.Length > this.currentIndex + 1;

  private void SetBallEffect()
  {
    this.gachaEffects.ForEach<KeyValuePair<GachaDirectionType, EffectControllerGacha.GachaEffect>>((System.Action<KeyValuePair<GachaDirectionType, EffectControllerGacha.GachaEffect>>) (pair =>
    {
      if (pair.Key == this.CurrentDirectionType)
        return;
      pair.Value.AnimatorList.ForEach((System.Action<Animator>) (animator => ((Component) animator).gameObject.SetActive(false)));
    }));
    this.gachaEffects[this.CurrentDirectionType].AnimatorList.ForEach((System.Action<Animator>) (animator => ((Component) animator).gameObject.SetActive(true)));
  }

  private void InitFlagAndValue()
  {
    this.State = EffectControllerGacha.STATE.SET;
    this.currentIndex = 0;
    this.SetGachaEffect();
    this.SetBallEffect();
    this.isAnimation = false;
    this.back_button_.SetActive(true);
  }

  private void CreateBall(GachaResultData.Result result, GameObject parent)
  {
    switch (result.isChangeEffect ? result.changeDirectionType.Value : result.directionType)
    {
      case GachaDirectionType.low:
        this.ballBlue.Clone(parent.transform);
        break;
      case GachaDirectionType.middle:
        this.ballYellow.Clone(parent.transform);
        break;
      case GachaDirectionType.high:
        this.ballRed.Clone(parent.transform);
        break;
      case GachaDirectionType.pickup:
        this.ballRainbow.Clone(parent.transform);
        break;
    }
  }

  private string GetSeForFirst()
  {
    switch (!this.CurrentResultData.isChangeEffect ? this.CurrentDirectionType : this.CurrentResultData.changeDirectionType.Value)
    {
      case GachaDirectionType.low:
        return !this.IsSingle ? "SE_0565" : "SE_0557";
      case GachaDirectionType.middle:
        return !this.IsSingle ? "SE_0566" : "SE_0558";
      case GachaDirectionType.high:
        return !this.IsSingle ? "SE_0567" : "SE_0559";
      default:
        return !this.IsSingle ? "SE_0568" : "SE_0560";
    }
  }

  private string GetSeForSecond()
  {
    switch (this.CurrentDirectionType)
    {
      case GachaDirectionType.low:
        return !this.IsSingle ? "SE_0569" : "SE_0561";
      case GachaDirectionType.middle:
        return !this.IsSingle ? "SE_0570" : "SE_0562";
      case GachaDirectionType.high:
        return !this.IsSingle ? "SE_0571" : "SE_0563";
      default:
        return !this.IsSingle ? "SE_0572" : "SE_0564";
    }
  }

  private bool[] GetLightBallMatrix(int length)
  {
    switch (length)
    {
      case 1:
        return new bool[10];
      case 2:
        bool[] lightBallMatrix = new bool[10];
        lightBallMatrix[2] = true;
        lightBallMatrix[7] = true;
        return lightBallMatrix;
      case 3:
        return new bool[10]
        {
          false,
          false,
          true,
          false,
          false,
          false,
          true,
          false,
          false,
          true
        };
      case 4:
        return new bool[10]
        {
          false,
          true,
          false,
          true,
          false,
          false,
          true,
          false,
          true,
          false
        };
      case 5:
        return new bool[10]
        {
          false,
          true,
          false,
          true,
          false,
          true,
          false,
          true,
          false,
          true
        };
      case 6:
        return new bool[10]
        {
          true,
          true,
          false,
          true,
          false,
          true,
          false,
          true,
          false,
          true
        };
      case 7:
        return new bool[10]
        {
          true,
          true,
          false,
          true,
          false,
          true,
          false,
          true,
          true,
          true
        };
      case 8:
        return new bool[10]
        {
          true,
          true,
          true,
          true,
          false,
          true,
          false,
          true,
          true,
          true
        };
      case 9:
        return new bool[10]
        {
          true,
          true,
          true,
          true,
          false,
          true,
          true,
          true,
          true,
          true
        };
      default:
        return new bool[10]
        {
          true,
          true,
          true,
          true,
          true,
          true,
          true,
          true,
          true,
          true
        };
    }
  }

  private void InitBall()
  {
    bool[] pattern = this.GetLightBallMatrix(this.resultList.Length);
    int index = 0;
    if (this.CurrentResultData.isChangeEffect)
    {
      if (this.CurrentResultData.directionType == GachaDirectionType.pickup)
      {
        GachaDirectionType? changeDirectionType = this.CurrentResultData.changeDirectionType;
        if (changeDirectionType.HasValue)
        {
          switch (changeDirectionType.GetValueOrDefault())
          {
            case GachaDirectionType.low:
              this.CurrentEffect.BallList = this.ballBlueToRainbowList;
              break;
            case GachaDirectionType.middle:
              this.CurrentEffect.BallList = this.ballYellowToRainbowList;
              break;
            case GachaDirectionType.high:
              this.CurrentEffect.BallList = this.ballRedToRainbowList;
              break;
          }
        }
      }
      else if (this.CurrentResultData.directionType == GachaDirectionType.high)
      {
        GachaDirectionType? changeDirectionType = this.CurrentResultData.changeDirectionType;
        if (changeDirectionType.HasValue)
        {
          switch (changeDirectionType.GetValueOrDefault())
          {
            case GachaDirectionType.low:
              this.CurrentEffect.BallList = this.ballBlueToRedList;
              break;
            case GachaDirectionType.middle:
              this.CurrentEffect.BallList = this.ballYellowToRedList;
              break;
          }
        }
      }
    }
    this.CurrentEffect.BallList.ForEachIndex<GameObject>((System.Action<GameObject, int>) ((ball, i) =>
    {
      foreach (Object @object in ball.transform)
        Object.Destroy(@object);
      if (!pattern[i])
        return;
      this.CreateBall(this.resultList[index++], ball);
    }));
  }

  private void SetDetail()
  {
    if (this.loadResourcesCoroutine != null)
      this.StopCoroutine(this.loadResourcesCoroutine);
    this.Detail.ResultData = this.CurrentResultData;
  }

  private void CreateDetail()
  {
    this.loadResourcesCoroutine = this.StartCoroutine(this.Detail.CreateResult(this.Menu.isPreview));
  }

  public IEnumerator SetNeedData(
    GachaResultData.Result[] resultList,
    GameObject backButton,
    bool isPreview)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EffectControllerGacha effectControllerGacha = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      effectControllerGacha.SetDetail();
      effectControllerGacha.State = EffectControllerGacha.STATE.INIT;
      effectControllerGacha.CreateDetail();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    effectControllerGacha.resultList = resultList;
    effectControllerGacha.back_button_ = backButton;
    effectControllerGacha.InitFlagAndValue();
    effectControllerGacha.InitBall();
    effectControllerGacha.Detail.SetActive(false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void PlaySoundForFirst()
  {
    string seForFirst = this.GetSeForFirst();
    Singleton<NGSoundManager>.GetInstance().stopSE();
    Singleton<NGSoundManager>.GetInstance().playSE(seForFirst);
  }

  public void PlaySoundForSecond()
  {
    string seForSecond = this.GetSeForSecond();
    Singleton<NGSoundManager>.GetInstance().StopSe(time: 1.2f);
    Singleton<NGSoundManager>.GetInstance().playSE(seForSecond);
    Singleton<NGSoundManager>.GetInstance().stopVoice();
    if (this.CurrentDirectionType != GachaDirectionType.pickup)
      return;
    Singleton<NGSoundManager>.GetInstance().playVoiceByID("VO_9000", 450);
  }

  public void PlayChangeEffectSEFirst()
  {
    Singleton<NGSoundManager>.GetInstance().stopSE();
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0577");
  }

  public void PlayChangeEffectSESecond()
  {
    Singleton<NGSoundManager>.GetInstance().StopSe(time: 1.2f);
    if (this.CurrentResultData.directionType == GachaDirectionType.high)
    {
      Singleton<NGSoundManager>.GetInstance().playSE("SE_0578");
    }
    else
    {
      if (this.CurrentResultData.directionType != GachaDirectionType.pickup)
        return;
      Singleton<NGSoundManager>.GetInstance().playSE("SE_0579");
    }
  }

  public void PlayChangeEffectBgm()
  {
    Singleton<NGSoundManager>.GetInstance().stopBGM();
    Singleton<NGSoundManager>.GetInstance().PlayBgmFile("BgmGacha", "bgm122", fadeInTime: 0.0f, fadeOutTime: 0.0f);
  }

  private void Init()
  {
    Singleton<NGSoundManager>.GetInstance().PlayBgmFile("BgmGacha", "bgm120");
    this.State = EffectControllerGacha.STATE.EFFECT_START;
  }

  private void AnimatorStart()
  {
    if (this.CurrentResultData.isChangeEffect)
    {
      this.StartProbabilityEffectShow(true);
    }
    else
    {
      this.SetGachaEffect();
      this.CurrentEffect.AnimatorStart();
    }
  }

  private void AnimatorLoop()
  {
    if (this.CurrentResultData.isChangeEffect)
    {
      this.PlayChangeEffectSEFirst();
      this.StartProbabilityEffectShow(false);
    }
    else
    {
      this.TurnOffProbabilityEffect();
      this.SetGachaEffect();
      this.CurrentEffect.AnimatorLoop();
    }
  }

  private void AnimatorEnd()
  {
    if (this.CurrentResultData.isChangeEffect)
    {
      this.CurrentEffect.AnimatorEnd();
    }
    else
    {
      this.SetGachaEffect();
      this.CurrentEffect.AnimatorEnd();
    }
  }

  private void AnimationStop()
  {
    this.StopProbabilityEffectAnimator();
    this.SetGachaEffect();
    this.gachaEffects.ForEach<KeyValuePair<GachaDirectionType, EffectControllerGacha.GachaEffect>>((System.Action<KeyValuePair<GachaDirectionType, EffectControllerGacha.GachaEffect>>) (x => x.Value.AnimationSetSpeed(0)));
  }

  public void StartProbabilityEffectShow(bool isFirst)
  {
    foreach (KeyValuePair<GachaDirectionType, EffectControllerGacha.GachaEffect> gachaEffect in this.gachaEffects)
      gachaEffect.Value.AnimatorList.ForEach((System.Action<Animator>) (animator => ((Component) animator).gameObject.SetActive(false)));
    this.TurnOffProbabilityEffect();
    if (this.CurrentResultData.directionType == GachaDirectionType.pickup)
    {
      GachaDirectionType? changeDirectionType = this.CurrentResultData.changeDirectionType;
      if (!changeDirectionType.HasValue)
        return;
      switch (changeDirectionType.GetValueOrDefault())
      {
        case GachaDirectionType.low:
          this.SetProbabilityEffectShow(isFirst, this.animatorBlueToRainbow, this.animatorBackGroundChangeToRainbow, this.animatorCameraChangeToRainbow);
          break;
        case GachaDirectionType.middle:
          this.SetProbabilityEffectShow(isFirst, this.animatorYellowToRainbow, this.animatorBackGroundChangeToRainbow, this.animatorCameraChangeToRainbow);
          break;
        case GachaDirectionType.high:
          this.SetProbabilityEffectShow(isFirst, this.animatorRedToRainbow, this.animatorBackGroundChangeToRainbow, this.animatorCameraChangeToRainbow);
          break;
      }
    }
    else
    {
      if (this.CurrentResultData.directionType != GachaDirectionType.high)
        return;
      GachaDirectionType? changeDirectionType = this.CurrentResultData.changeDirectionType;
      if (!changeDirectionType.HasValue)
        return;
      switch (changeDirectionType.GetValueOrDefault())
      {
        case GachaDirectionType.low:
          this.SetProbabilityEffectShow(isFirst, this.animatorBlueToRed, this.animatorBackGroundChangeToRed, this.animatorCameraChangeToRed);
          break;
        case GachaDirectionType.middle:
          this.SetProbabilityEffectShow(isFirst, this.animatorYellowToRed, this.animatorBackGroundChangeToRed, this.animatorCameraChangeToRed);
          break;
      }
    }
  }

  private void TurnOffProbabilityEffect()
  {
    ((Component) this.animatorBlueToRainbow).gameObject.SetActive(false);
    ((Component) this.animatorYellowToRainbow).gameObject.SetActive(false);
    ((Component) this.animatorRedToRainbow).gameObject.SetActive(false);
    ((Component) this.animatorBlueToRed).gameObject.SetActive(false);
    ((Component) this.animatorYellowToRed).gameObject.SetActive(false);
    ((Component) this.animatorBackGroundChangeToRed).gameObject.SetActive(false);
    ((Component) this.animatorCameraChangeToRed).gameObject.SetActive(false);
    ((Component) this.animatorBackGroundChangeToRainbow).gameObject.SetActive(false);
    ((Component) this.animatorCameraChangeToRainbow).gameObject.SetActive(false);
  }

  private void StopProbabilityEffectAnimator()
  {
    this.animatorBlueToRainbow.speed = 0.0f;
    this.animatorYellowToRainbow.speed = 0.0f;
    this.animatorRedToRainbow.speed = 0.0f;
    this.animatorBlueToRed.speed = 0.0f;
    this.animatorYellowToRed.speed = 0.0f;
    this.animatorBackGroundChangeToRed.speed = 0.0f;
    this.animatorCameraChangeToRed.speed = 0.0f;
    this.animatorBackGroundChangeToRainbow.speed = 0.0f;
    this.animatorCameraChangeToRainbow.speed = 0.0f;
  }

  private void SetProbabilityEffectShow(bool isFirst, Animator ball, Animator bg, Animator camera)
  {
    ((Component) ball).gameObject.SetActive(true);
    ((Component) bg).gameObject.SetActive(true);
    ((Component) camera).gameObject.SetActive(true);
    this.CurrentEffect.BackGroundAnimator = bg;
    this.CurrentEffect.EffectAnimator = ball;
    this.CurrentEffect.CameraAnimator = camera;
    this.CurrentEffect.AnimationSetSpeed(1);
    if (isFirst)
    {
      bg.Play("BG_start", 0, 0.0f);
      camera.Play("camera_start", 0, 0.0f);
      ball.Play("effects_start", 0, 0.0f);
    }
    else
    {
      bg.Play("BG_change", 0, 0.0f);
      camera.Play("camera_change", 0, 0.0f);
      ball.Play("effects_change", 0, 0.0f);
    }
  }

  public void StartDetailAnimation() => this.State = EffectControllerGacha.STATE.DETAIL_START;

  public void FinishDetailAnimation()
  {
    if (this.Detail.Current.IsPickUpAnimation)
    {
      this.State = EffectControllerGacha.STATE.WAIT;
    }
    else
    {
      if (this.State == EffectControllerGacha.STATE.DONE)
        return;
      this.State = EffectControllerGacha.STATE.END;
    }
  }

  private void EffectStart()
  {
    this.State = EffectControllerGacha.STATE.EFFECT_PLAY;
    this.isAnimation = true;
    this.AnimatorStart();
    this.PlaySoundForFirst();
  }

  private void EffectLoop()
  {
    ++this.currentIndex;
    this.State = EffectControllerGacha.STATE.EFFECT_PLAY;
    if (!this.CurrentResultData.isChangeEffect)
      Singleton<NGSoundManager>.GetInstance().PlayBgmFile("BgmGacha", "bgm121");
    this.SetBallEffect();
    this.AnimatorLoop();
    this.Detail.SetActive(false);
    this.SetDetail();
    this.CreateDetail();
    if (this.currentIndex % 10 != 0)
      return;
    Resources.UnloadUnusedAssets();
  }

  private void DetailStart()
  {
    this.State = EffectControllerGacha.STATE.DETAIL_PLAY;
    this.Detail.SetAniamationEnabled(false);
    this.StartCoroutine(this.DetailStartDelay());
  }

  private IEnumerator DetailStartDelay()
  {
    do
    {
      yield return (object) null;
    }
    while (!this.Detail.Current.IsLoaded);
    this.Detail.SetActive(true);
    this.Detail.SetAniamationEnabled(true);
    if (this.Detail.Current.IsPickUpAnimation)
    {
      if (!this.isSkip)
        this.Menu.PlayEffectWhiteFadeIn();
      this.Detail.PlaySoundForPickUp();
    }
    yield return (object) null;
    this.AnimationStop();
    if (this.currentIndex != 0)
      Singleton<NGSoundManager>.GetInstance().stopBGM();
  }

  private void ShowResult()
  {
    this.State = EffectControllerGacha.STATE.DONE;
    this.Menu.ShowResult();
  }

  private void End()
  {
    this.Menu.SetEffectWhiteFadeActive(false);
    while (this.HasNextResult)
    {
      if (this.isSkip)
      {
        ++this.currentIndex;
        this.SetDetail();
        if (this.Detail.Current.IsPickUpAnimation)
        {
          this.State = EffectControllerGacha.STATE.DETAIL_START;
          this.Detail.SetActive(true);
          this.CreateDetail();
          return;
        }
      }
      else
      {
        this.State = EffectControllerGacha.STATE.EFFECT_LOOP;
        this.Menu.PlayEffectWhiteFadeIn();
        this.AnimatorEnd();
        return;
      }
    }
    if (this.isSkip)
    {
      this.State = EffectControllerGacha.STATE.RESULT;
    }
    else
    {
      this.State = EffectControllerGacha.STATE.WAIT;
      this.AnimationStop();
      this.OnAnimationEnd();
    }
  }

  public void Next()
  {
    switch (this.State)
    {
      case EffectControllerGacha.STATE.EFFECT_PLAY:
        this.State = EffectControllerGacha.STATE.DETAIL_START;
        break;
      case EffectControllerGacha.STATE.DETAIL_PLAY:
        if (this.Detail.Current.IsPickUpAnimation || !this.Detail.Current.IsLoaded)
          break;
        this.State = EffectControllerGacha.STATE.END;
        break;
      case EffectControllerGacha.STATE.WAIT:
        if (this.HasNextResult)
        {
          this.State = EffectControllerGacha.STATE.END;
          break;
        }
        this.State = EffectControllerGacha.STATE.RESULT;
        break;
    }
  }

  public void Skip()
  {
    this.isSkip = true;
    if (this.Detail.Current.IsPickUpAnimation)
    {
      if (this.State == EffectControllerGacha.STATE.DETAIL_START || this.State == EffectControllerGacha.STATE.DETAIL_PLAY)
        return;
      if (this.State != EffectControllerGacha.STATE.WAIT)
      {
        this.State = EffectControllerGacha.STATE.DETAIL_START;
        return;
      }
    }
    this.State = EffectControllerGacha.STATE.END;
  }

  private void LateUpdate()
  {
    if (this.isSkip)
    {
      switch (this.State)
      {
        case EffectControllerGacha.STATE.EFFECT_START:
        case EffectControllerGacha.STATE.EFFECT_LOOP:
        case EffectControllerGacha.STATE.DETAIL_START:
          this.DetailStart();
          break;
        case EffectControllerGacha.STATE.END:
          this.End();
          break;
        case EffectControllerGacha.STATE.RESULT:
          this.ShowResult();
          break;
      }
    }
    else
    {
      switch (this.State)
      {
        case EffectControllerGacha.STATE.INIT:
          this.Init();
          break;
        case EffectControllerGacha.STATE.EFFECT_START:
          this.EffectStart();
          break;
        case EffectControllerGacha.STATE.EFFECT_LOOP:
          this.EffectLoop();
          break;
        case EffectControllerGacha.STATE.DETAIL_START:
          this.DetailStart();
          break;
        case EffectControllerGacha.STATE.END:
          this.End();
          break;
        case EffectControllerGacha.STATE.RESULT:
          this.ShowResult();
          break;
      }
    }
  }

  public enum STATE
  {
    SET,
    INIT,
    EFFECT_START,
    EFFECT_LOOP,
    EFFECT_PLAY,
    DETAIL_START,
    DETAIL_PLAY,
    END,
    WAIT,
    RESULT,
    DONE,
  }

  public class GachaEffect
  {
    private List<Animator> animatorList;

    public List<GameObject> BallList { get; set; }

    public Animator BackGroundAnimator { get; set; }

    public Animator CameraAnimator { get; set; }

    public Animator EffectAnimator { get; set; }

    public List<Animator> AnimatorList
    {
      get
      {
        if (this.animatorList == null)
          this.animatorList = new List<Animator>()
          {
            this.BackGroundAnimator,
            this.CameraAnimator,
            this.EffectAnimator
          };
        return this.animatorList;
      }
    }

    public void AnimationSetSpeed(int speed)
    {
      this.BackGroundAnimator.speed = (float) speed;
      this.CameraAnimator.speed = (float) speed;
      this.EffectAnimator.speed = (float) speed;
    }

    public void AnimatorStart()
    {
      this.AnimationSetSpeed(1);
      this.BackGroundAnimator.Play("BG_start", 0, 0.0f);
      this.CameraAnimator.Play("camera_start", 0, 0.0f);
      this.EffectAnimator.Play("effects_start", 0, 0.0f);
    }

    public void AnimatorLoop()
    {
      this.AnimationSetSpeed(1);
      this.BackGroundAnimator.Play("BG_loop", 0, 0.0f);
      this.CameraAnimator.Play("camera_loop", 0, 0.0f);
      this.EffectAnimator.Play("effects_loop", 0, 0.0f);
    }

    public void AnimatorEnd()
    {
      this.AnimationSetSpeed(1);
      this.BackGroundAnimator.Play("BG_end", 0, 0.0f);
      this.CameraAnimator.Play("camera_end", 0, 0.0f);
      this.EffectAnimator.Play("effects_end", 0, 0.0f);
    }
  }
}
