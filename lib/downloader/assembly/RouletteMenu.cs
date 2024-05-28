// Decompiled with JetBrains decompiler
// Type: RouletteMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class RouletteMenu : NGMenuBase
{
  [SerializeField]
  private Transform mainPanel;
  private Transform rouletteWheel;
  private TweenAlpha clearRouletteWheelTweenAlpha;
  private TweenAlpha blurredRouletteWheelTweenAlpha;
  private List<GameObject> awardWheelIconContainers;
  private UIButton awardDetailButton;
  private UIButton startButton;
  private UIButton returnButton;
  private GameObject DisabledStartButtonAdd;
  [SerializeField]
  private GameObject cutInContainer;
  private DuelCutin cutinController;
  [SerializeField]
  private RouletteAnimationController rouletteAnimationController;
  public RouletteMenu.WheelStatus currentWheelStatus;
  public RouletteMenu.BlurredWheelEffectStatus currentBlurredWheelStatus;
  private bool isCutInPlayed;
  public float maxSpeed = 1080f;
  public float accelerateTime = 1f;
  public float minSteadyTime = 3f;
  public float decelerateTime = 1f;
  private float acceleration;
  private float deceleration;
  private float accelerateAngle;
  private float minSteadyAngle;
  private float steadyAngle;
  private float decelerateAngle;
  private float steadyTime;
  public float blurEffectStartTime = 1f;
  public float blurEffectEndTime = 4f;
  public float cutInStartTime = 1.5f;
  private float currentPlayTime;
  public int awardCount = 8;
  private float angleIncrement;
  private List<float> awardAngles;
  private float originalWheelAngleZ;
  private float currentWheelAngleZ;
  [Range(0.0f, 1f)]
  public float accuracy = 0.75f;
  [Tooltip("「GET」演出のDelay")]
  public float showResultAwardPopupDelay = 0.5f;
  private int resultIndex;
  private List<RouletteR001FreeDeckEntity> rouletteDeckEntityList;
  private RouletteR001FreeDeckEntity resultDeckEntity;
  private int rouletteID;
  private int rouletteDeckID;
  private int rouletteDeckEntityID;
  private bool shouldShowCampaign;
  private string campaignURL;
  private bool canRoulette;
  public bool isDisplayingAwardDetailPopup;
  private GameObject awardResultPopupPrefab;
  private GameObject campaignPopupPrefab;
  private GameObject awardDetailPopupPrefab;
  private GameObject awardDetailPopupItemPrefab;
  private GameObject awardWheelIconPrefab;
  private GameObject cutInEffectPrefab;
  private string[] cutinEffectPrefabPaths = new string[4]
  {
    null,
    "Animations/battle_cutin/battle_cutin_prefab",
    "Animations/battle_cutin/battle_cutin_demon",
    "Animations/battle_cutin/battle_cutin_fairy"
  };
  private int[] cutinEffectSoundClipNames = new int[4]
  {
    0,
    451,
    452,
    453
  };
  private const int GEBARUTO_EARL_UNIT_ID = -999;
  private int SmallCuttinUnitId;
  private int NormalCuttinUnitId;
  private int LargeCuttinUnitId;
  private RouletteMain rouletteMainScript;
  private bool isNecessaryPrefabsLoaded;
  private bool isResultAwardPrefabsLoaded;
  private NGSoundManager soundManager;

  public bool IsPreview { get; set; }

  public IEnumerator Initialize()
  {
    RouletteMenu rouletteMenu = this;
    rouletteMenu.canRoulette = false;
    rouletteMenu.currentWheelStatus = RouletteMenu.WheelStatus.Stopped;
    rouletteMenu.soundManager = Singleton<NGSoundManager>.GetInstance();
    int moduleIndex = 0;
    int rouletteIndex = 0;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/roulette/roulette_Main/roulette_Main").Load<GameObject>();
    IEnumerator e1 = prefabF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    GameObject result = prefabF.Result;
    rouletteMenu.rouletteMainScript = result.Clone(rouletteMenu.mainPanel).GetComponent<RouletteMain>();
    rouletteMenu.rouletteAnimationController.Init(rouletteMenu.rouletteMainScript.animator, rouletteMenu.rouletteMainScript.serifLabel, ((IEnumerable<string>) rouletteMenu.rouletteMainScript.CharacterSefits).ToList<string>());
    rouletteMenu.PassOverMemberMainToMenu(rouletteMenu.rouletteMainScript);
    rouletteMenu.SetButtonEvent(rouletteMenu.rouletteMainScript);
    rouletteMenu.StartCoroutine(rouletteMenu.LoadNecessaryPrefabsCoroutine());
    while (!rouletteMenu.isNecessaryPrefabsLoaded)
      yield return (object) null;
    if (!rouletteMenu.IsPreview)
    {
      Future<WebAPI.Response.Roulette> rouletteFuture = WebAPI.Roulette((Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<NGGameDataManager>.GetInstance().isOpenRoulette = false;
        WebAPI.DefaultUserErrorCallback(e);
        Singleton<NGSceneManager>.GetInstance().backScene();
      }));
      e1 = rouletteFuture.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (!rouletteFuture.HasResult || rouletteFuture.Result == null)
      {
        yield break;
      }
      else
      {
        RouletteModuleRoulette[] roulette = rouletteFuture.Result.roulette_modules[moduleIndex].roulette;
        rouletteMenu.rouletteID = roulette[rouletteIndex].id;
        rouletteMenu.rouletteDeckID = roulette[rouletteIndex].deck_id;
        rouletteMenu.shouldShowCampaign = roulette[rouletteIndex].is_campaign;
        rouletteMenu.canRoulette = roulette[rouletteIndex].can_roulette;
        // ISSUE: reference to a compiler-generated method
        rouletteMenu.campaignURL = ((IEnumerable<RouletteR001FreeRoulette>) MasterData.RouletteR001FreeRouletteList).FirstOrDefault<RouletteR001FreeRoulette>(new Func<RouletteR001FreeRoulette, bool>(rouletteMenu.\u003CInitialize\u003Eb__69_2)).url;
        rouletteFuture = (Future<WebAPI.Response.Roulette>) null;
      }
    }
    // ISSUE: reference to a compiler-generated method
    rouletteMenu.rouletteDeckEntityList = ((IEnumerable<RouletteR001FreeDeckEntity>) MasterData.RouletteR001FreeDeckEntityList).Where<RouletteR001FreeDeckEntity>(new Func<RouletteR001FreeDeckEntity, bool>(rouletteMenu.\u003CInitialize\u003Eb__69_0)).ToList<RouletteR001FreeDeckEntity>();
    for (int i = 0; i < rouletteMenu.awardWheelIconContainers.Count; ++i)
    {
      foreach (Component component in rouletteMenu.awardWheelIconContainers[i].transform)
        Object.Destroy((Object) component.gameObject);
      rouletteMenu.awardWheelIconContainers[i].transform.Clear();
      e1 = rouletteMenu.awardWheelIconPrefab.Clone(rouletteMenu.awardWheelIconContainers[i].transform).GetComponent<RouletteAwardWheelIconController>().Init(rouletteMenu.rouletteDeckEntityList[i].reward_quantity ?? 1, rouletteMenu.rouletteDeckEntityList[i].reward_type_id, rouletteMenu.rouletteDeckEntityList[i].reward_id);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
    rouletteMenu.awardAngles = new List<float>(rouletteMenu.awardCount + 1);
    rouletteMenu.angleIncrement = 360f / (float) rouletteMenu.awardCount;
    for (int index = 0; index < rouletteMenu.awardCount + 1; ++index)
      rouletteMenu.awardAngles.Add((float) index * rouletteMenu.angleIncrement);
    if (!rouletteMenu.canRoulette)
    {
      ((UIButtonColor) rouletteMenu.awardDetailButton).isEnabled = true;
      ((UIButtonColor) rouletteMenu.returnButton).isEnabled = true;
      ((UIButtonColor) rouletteMenu.startButton).isEnabled = false;
      rouletteMenu.DisabledStartButtonAdd.SetActive(true);
      rouletteMenu.rouletteAnimationController.CanNotRoulettePlay();
    }
    else
    {
      rouletteMenu.DisabledStartButtonAdd.SetActive(false);
      rouletteMenu.SetButtonsAvailbility(false);
      rouletteMenu.StartCoroutine(rouletteMenu.LoadResultAwardPrefabsCoroutine());
      while (!rouletteMenu.isResultAwardPrefabsLoaded)
        yield return (object) null;
    }
  }

  private void PassOverMemberMainToMenu(RouletteMain rouletteMain)
  {
    this.awardWheelIconContainers = rouletteMain.awardWheelIconContainers;
    this.awardDetailButton = rouletteMain.awardDetailButton;
    this.startButton = rouletteMain.startButton;
    this.DisabledStartButtonAdd = rouletteMain.DisabledStartButtonAdd;
    this.returnButton = rouletteMain.returnButton;
    this.rouletteWheel = rouletteMain.rouletteWheel;
    this.clearRouletteWheelTweenAlpha = rouletteMain.clearRouletteWheelTweenAlpha;
    this.blurredRouletteWheelTweenAlpha = rouletteMain.blurredRouletteWheelTweenAlpha;
    this.SmallCuttinUnitId = rouletteMain.SmallCuttinUnitId;
    this.NormalCuttinUnitId = rouletteMain.NormalCuttinUnitId;
    this.LargeCuttinUnitId = rouletteMain.LargeCuttinUnitId;
  }

  private void SetButtonEvent(RouletteMain rouletteMain)
  {
    EventDelegate.Set(rouletteMain.SerifButton.onClick, new EventDelegate.Callback(this.rouletteAnimationController.OnTapOttimo));
    EventDelegate.Set(rouletteMain.BackButton.onClick, new EventDelegate.Callback(this.onBackButton));
    EventDelegate.Set(rouletteMain.AwardListButton.onClick, new EventDelegate.Callback(this.ShowAwardDetailPopup));
    EventDelegate.Set(rouletteMain.StartButton.onClick, new EventDelegate.Callback(this.OnClickStartButton));
  }

  public void PlayOttimoAnimation()
  {
    if (!this.canRoulette)
      return;
    this.rouletteAnimationController.CanRoulettePlay();
  }

  private void FixedUpdate()
  {
    if (this.currentWheelStatus == RouletteMenu.WheelStatus.Stopped || this.currentWheelStatus == RouletteMenu.WheelStatus.Preparing)
      return;
    this.UpdateRotationStatus();
  }

  public void SetButtonsAvailbility(bool isEnabled, float delay = 0.0f)
  {
    if ((double) delay > 0.0)
    {
      this.StartCoroutine(this.SetButtonsAvailbilityCoroutine(isEnabled, delay));
    }
    else
    {
      ((UIButtonColor) this.awardDetailButton).isEnabled = isEnabled;
      ((UIButtonColor) this.startButton).isEnabled = isEnabled;
      ((UIButtonColor) this.returnButton).isEnabled = isEnabled;
    }
  }

  private IEnumerator SetButtonsAvailbilityCoroutine(bool isEnabled, float delay = 0.0f)
  {
    yield return (object) new WaitForSeconds(delay);
    ((UIButtonColor) this.awardDetailButton).isEnabled = isEnabled;
    ((UIButtonColor) this.startButton).isEnabled = isEnabled;
    ((UIButtonColor) this.returnButton).isEnabled = isEnabled;
  }

  public void OnClickStartButton()
  {
    if ((double) this.maxSpeed == 0.0)
    {
      Debug.LogError((object) "maxSpeed cannot be 0!");
    }
    else
    {
      if (this.currentWheelStatus != RouletteMenu.WheelStatus.Stopped)
        return;
      this.currentWheelStatus = RouletteMenu.WheelStatus.Preparing;
      this.soundManager.playSE("SE_1059");
      this.SetButtonsAvailbility(false);
      this.StartCoroutine(this.PlayRoulette());
    }
  }

  private IEnumerator PlayRoulette()
  {
    RouletteMenu rouletteMenu = this;
    int previousLoadingMode = Singleton<CommonRoot>.GetInstance().loadingMode;
    Singleton<CommonRoot>.GetInstance().loadingMode = 2;
    if (!rouletteMenu.IsPreview)
    {
      Future<WebAPI.Response.RouletteR001FreePay> awardFuture = WebAPI.RouletteR001FreePay(rouletteMenu.rouletteID, (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        Singleton<NGSceneManager>.GetInstance().backScene();
      }));
      IEnumerator e1 = awardFuture.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (!awardFuture.HasResult || awardFuture.Result == null)
      {
        yield break;
      }
      else
      {
        int index = 0;
        rouletteMenu.rouletteDeckEntityID = awardFuture.Result.result[index].deck_entity_id;
        Singleton<NGGameDataManager>.GetInstance().isCanRoulette = awardFuture.Result.can_roulette;
        awardFuture = (Future<WebAPI.Response.RouletteR001FreePay>) null;
      }
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = previousLoadingMode;
    // ISSUE: reference to a compiler-generated method
    rouletteMenu.resultDeckEntity = rouletteMenu.rouletteDeckEntityList.FirstOrDefault<RouletteR001FreeDeckEntity>(new Func<RouletteR001FreeDeckEntity, bool>(rouletteMenu.\u003CPlayRoulette\u003Eb__77_0));
    rouletteMenu.resultIndex = rouletteMenu.rouletteDeckEntityList.IndexOf(rouletteMenu.resultDeckEntity);
    yield return (object) rouletteMenu.StartCoroutine(rouletteMenu.InitializeCutIn(rouletteMenu.resultDeckEntity.action_pattern_id));
    rouletteMenu.RotationStart(rouletteMenu.resultIndex);
  }

  private void RotationStart(int resultIndex)
  {
    this.acceleration = this.maxSpeed / this.accelerateTime;
    this.deceleration = this.maxSpeed / this.decelerateTime;
    this.accelerateAngle = this.maxSpeed * 0.5f * this.accelerateTime;
    this.decelerateAngle = this.maxSpeed * 0.5f * this.decelerateTime;
    this.minSteadyAngle = this.maxSpeed * this.minSteadyTime;
    this.originalWheelAngleZ = this.rouletteWheel.localEulerAngles.z;
    this.currentPlayTime = 0.0f;
    this.isCutInPlayed = false;
    this.currentWheelStatus = RouletteMenu.WheelStatus.Accelerate;
    float num = Random.Range(this.awardAngles[resultIndex] + (float) ((double) this.angleIncrement * (double) this.accuracy * 0.5), this.awardAngles[resultIndex + 1] - (float) ((double) this.angleIncrement * (double) this.accuracy * 0.5)) - (float) (((double) this.originalWheelAngleZ + (double) this.accelerateAngle + (double) this.minSteadyAngle + (double) this.decelerateAngle) % 360.0);
    if ((double) this.maxSpeed > 0.0 && (double) num < 0.0)
      num += 360f;
    else if ((double) this.maxSpeed < 0.0 && (double) num > 0.0)
      num -= 360f;
    this.steadyTime = this.minSteadyTime + num / this.maxSpeed;
    this.steadyAngle = this.minSteadyAngle + num;
    ((Component) this.awardDetailButton).gameObject.SetActive(false);
    ((Component) this.startButton).gameObject.SetActive(false);
    this.soundManager.playSE("SE_1060");
  }

  private void UpdateRotationStatus()
  {
    this.currentPlayTime += Time.fixedDeltaTime;
    if ((double) this.currentPlayTime - (double) this.accelerateTime - (double) this.steadyTime - (double) this.decelerateTime >= 0.0)
    {
      this.currentWheelStatus = RouletteMenu.WheelStatus.Stopped;
      this.currentWheelAngleZ = this.originalWheelAngleZ + this.accelerateAngle + this.steadyAngle + this.decelerateAngle;
      ((Component) this.awardDetailButton).gameObject.SetActive(true);
      ((Component) this.startButton).gameObject.SetActive(true);
      this.StartCoroutine(this.ShowResultAwardPopup());
    }
    else if ((double) this.currentPlayTime - (double) this.accelerateTime - (double) this.steadyTime >= 0.0)
    {
      this.currentWheelStatus = RouletteMenu.WheelStatus.Decerlerate;
      float num = this.currentPlayTime - this.accelerateTime - this.steadyTime;
      this.currentWheelAngleZ = (float) ((double) this.originalWheelAngleZ + (double) this.accelerateAngle + (double) this.steadyAngle + ((double) this.maxSpeed * (double) num - 0.5 * (double) this.deceleration * (double) num * (double) num));
    }
    else if ((double) this.currentPlayTime - (double) this.accelerateTime >= 0.0)
    {
      this.currentWheelStatus = RouletteMenu.WheelStatus.Steady;
      this.currentWheelAngleZ = (float) ((double) this.originalWheelAngleZ + (double) this.accelerateAngle + (double) (this.currentPlayTime - this.accelerateTime) * (double) this.maxSpeed);
    }
    else if ((double) this.currentPlayTime > 0.0)
    {
      this.currentWheelStatus = RouletteMenu.WheelStatus.Accelerate;
      this.currentWheelAngleZ = this.originalWheelAngleZ + 0.5f * this.acceleration * this.currentPlayTime * this.currentPlayTime;
    }
    this.rouletteWheel.localEulerAngles = new Vector3(0.0f, 0.0f, this.currentWheelAngleZ);
    if (this.currentBlurredWheelStatus == RouletteMenu.BlurredWheelEffectStatus.NotPlaying && (double) this.currentPlayTime >= (double) this.blurEffectStartTime && (double) this.currentPlayTime < (double) this.blurEffectEndTime)
    {
      ((UITweener) this.blurredRouletteWheelTweenAlpha).PlayForward();
      ((UITweener) this.clearRouletteWheelTweenAlpha).PlayForward();
      this.currentBlurredWheelStatus = RouletteMenu.BlurredWheelEffectStatus.Playing;
    }
    else if (this.currentBlurredWheelStatus == RouletteMenu.BlurredWheelEffectStatus.Playing && (double) this.currentPlayTime >= (double) this.blurEffectEndTime)
    {
      ((UITweener) this.blurredRouletteWheelTweenAlpha).PlayReverse();
      ((UITweener) this.clearRouletteWheelTweenAlpha).PlayReverse();
      this.currentBlurredWheelStatus = RouletteMenu.BlurredWheelEffectStatus.NotPlaying;
    }
    if (this.isCutInPlayed || (double) this.currentPlayTime < (double) this.cutInStartTime)
      return;
    this.PlayCutIn(this.resultDeckEntity.action_pattern_id);
    this.isCutInPlayed = true;
  }

  private IEnumerator LoadNecessaryPrefabsCoroutine()
  {
    RouletteMenu rouletteMenu = this;
    IEnumerator e = rouletteMenu.LoadNecessaryPrefabs();
    while (true)
    {
      object obj = (object) null;
      try
      {
        if (!e.MoveNext())
          break;
        obj = e.Current;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Catch exception: LoadNecessaryPrefabs: " + (object) ex));
        rouletteMenu.StartCoroutine(rouletteMenu.LoadNecessaryPrefabsCoroutine());
      }
      yield return obj;
    }
  }

  private IEnumerator LoadNecessaryPrefabs()
  {
    Future<GameObject> awardWheelIconPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.awardWheelIconPrefab, (Object) null))
    {
      awardWheelIconPrefabF = new ResourceObject("Prefabs/roulette/roulette_Common/AwardContent").Load<GameObject>();
      e = awardWheelIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.awardWheelIconPrefab = awardWheelIconPrefabF.Result;
      awardWheelIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.awardDetailPopupPrefab, (Object) null))
    {
      awardWheelIconPrefabF = new ResourceObject("Prefabs/roulette/roulette_Common/popup_Roulette_Reward__anim_popup01").Load<GameObject>();
      e = awardWheelIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.awardDetailPopupPrefab = awardWheelIconPrefabF.Result;
      awardWheelIconPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.awardDetailPopupItemPrefab, (Object) null))
    {
      awardWheelIconPrefabF = new ResourceObject("Prefabs/roulette/roulette_Common/dir_Roulette_RewardList").Load<GameObject>();
      e = awardWheelIconPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.awardDetailPopupItemPrefab = awardWheelIconPrefabF.Result;
      awardWheelIconPrefabF = (Future<GameObject>) null;
    }
    this.isNecessaryPrefabsLoaded = true;
  }

  private IEnumerator LoadResultAwardPrefabsCoroutine()
  {
    RouletteMenu rouletteMenu = this;
    IEnumerator e = rouletteMenu.LoadResultAwardPrefabs();
    while (true)
    {
      object obj = (object) null;
      try
      {
        if (!e.MoveNext())
          break;
        obj = e.Current;
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Catch exception: LoadResultAwardPrefabs: " + (object) ex));
        rouletteMenu.StartCoroutine(rouletteMenu.LoadResultAwardPrefabsCoroutine());
      }
      yield return obj;
    }
  }

  private IEnumerator LoadResultAwardPrefabs()
  {
    Future<GameObject> awardResultPopupPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) this.awardResultPopupPrefab, (Object) null))
    {
      awardResultPopupPrefabF = new ResourceObject("Prefabs/roulette/roulette_Common/dir_Result_Roulette_Get_reward").Load<GameObject>();
      e = awardResultPopupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.awardResultPopupPrefab = awardResultPopupPrefabF.Result;
      awardResultPopupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.campaignPopupPrefab, (Object) null) && this.shouldShowCampaign)
    {
      awardResultPopupPrefabF = new ResourceObject("Prefabs/roulette/roulette_Common/popup_Roulette_Campaign__anim_popup01").Load<GameObject>();
      e = awardResultPopupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.campaignPopupPrefab = awardResultPopupPrefabF.Result;
      awardResultPopupPrefabF = (Future<GameObject>) null;
    }
    this.isResultAwardPrefabsLoaded = true;
  }

  public void ShowAwardDetailPopup()
  {
    if (this.isDisplayingAwardDetailPopup)
      return;
    this.isDisplayingAwardDetailPopup = true;
    this.StartCoroutine(this.ShowAwardDetailPopupCoroutine());
  }

  private IEnumerator ShowResultAwardPopup()
  {
    yield return (object) new WaitForSeconds(this.showResultAwardPopupDelay);
    IEnumerator e = Singleton<PopupManager>.GetInstance().open(this.awardResultPopupPrefab, clip: "SE_1061").GetComponent<RouletteAwardResultPopupController>().Init(this.resultDeckEntity, this.shouldShowCampaign, this.campaignPopupPrefab, this.campaignURL);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator ShowAwardDetailPopupCoroutine()
  {
    RouletteMenu menu = this;
    IEnumerator e = Singleton<PopupManager>.GetInstance().open(menu.awardDetailPopupPrefab).GetComponent<RouletteAwardDetailPopupController>().Init(menu, menu.rouletteDeckEntityList, menu.awardDetailPopupItemPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator InitializeCutIn(int cutInPattern)
  {
    if (cutInPattern != 0)
    {
      Future<GameObject> cutInEffectBigPrefabF = new ResourceObject(this.cutinEffectPrefabPaths[cutInPattern]).Load<GameObject>();
      IEnumerator e = cutInEffectBigPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.cutInEffectPrefab = cutInEffectBigPrefabF.Result;
      foreach (Component component in this.cutInContainer.transform)
        Object.Destroy((Object) component.gameObject);
      this.cutInContainer.transform.Clear();
      this.cutInContainer.SetActive(false);
      this.cutinController = this.cutInEffectPrefab.Clone(this.cutInContainer.transform).GetComponent<DuelCutin>();
      Future<Sprite> cutInSprite;
      switch (cutInPattern)
      {
        case 1:
          cutInSprite = this.GetCutInSprite(this.SmallCuttinUnitId);
          break;
        case 2:
          cutInSprite = this.GetCutInSprite(this.NormalCuttinUnitId);
          break;
        case 3:
          cutInSprite = this.GetCutInSprite(this.LargeCuttinUnitId);
          break;
        default:
          Debug.LogError((object) ("想定していない演出パターンIDです: " + (object) cutInPattern));
          yield break;
      }
      e = this.cutinController.InitializeForRoulette(cutInSprite);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private Future<Sprite> GetCutInSprite(int cutInUnitId)
  {
    return cutInUnitId == -999 ? Singleton<ResourceManager>.GetInstance().Load<Sprite>("AssetBundle/Resources/Characters/ottimo/ottimo_cutin") : MasterData.UnitUnit[cutInUnitId].LoadCutin();
  }

  private void PlayCutIn(int cutInPattern)
  {
    if (cutInPattern == 0)
      return;
    this.cutInContainer.SetActive(true);
    ((Component) this.cutinController).gameObject.SetActive(true);
    this.cutinController.PlaySkillCutInForRoulette(this.cutinEffectSoundClipNames[cutInPattern]);
    this.cutinController.CameraCutin.transform.localPosition = new Vector3(0.0f, 0.0f, 10f);
    this.cutinController.CameraCutin.GetComponent<Camera>().depth = 0.1f;
  }

  public void onBackButton() => Singleton<NGSceneManager>.GetInstance().backScene();

  public enum WheelStatus
  {
    Stopped,
    Preparing,
    Accelerate,
    Steady,
    Decerlerate,
  }

  public enum BlurredWheelEffectStatus
  {
    NotPlaying,
    Playing,
  }
}
