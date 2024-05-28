// Decompiled with JetBrains decompiler
// Type: Sea030HomeMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using LocaleTimeZone;
using MasterDataTable;
using SA.Foundation.Time;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnitStatusInformation;
using UnityEngine;

#nullable disable
public class Sea030HomeMenu : BackButtonMenuBase
{
  [SerializeField]
  private SeaHomeManager manager;
  [SerializeField]
  private GameObject homeRoot;
  [SerializeField]
  private GameObject buttonsRoot;
  [SerializeField]
  private GameObject[] cameraButtons;
  [SerializeField]
  private GameObject talkBackButton;
  [SerializeField]
  private GameObject talkRoot;
  [SerializeField]
  private GameObject dynUnit2D;
  [SerializeField]
  private UILabel characterNameLabel;
  [SerializeField]
  private UILabel textLabel;
  [SerializeField]
  private UI2DSprite unitThum;
  [SerializeField]
  private UIButton BtnDate;
  [SerializeField]
  private UIButton BtnDateBadMood;
  [SerializeField]
  private GameObject commingSoon;
  [SerializeField]
  private UIButton BtnPresent;
  [SerializeField]
  private GameObject GaugeObject;
  [SerializeField]
  private LoveGaugeController loveGauge;
  [SerializeField]
  private UILabel trustLabel;
  [SerializeField]
  private GameObject dirExtraSkill;
  [SerializeField]
  private UI2DSprite slcExtraSkill;
  [SerializeField]
  private GameObject resultEffectObject;
  [SerializeField]
  private Animator resultEffectAnimator;
  [SerializeField]
  private Animator unitAnimator;
  [SerializeField]
  private GameObject resultGaugeEffectObject;
  [SerializeField]
  private Animator resultGaugeEffectAnimator;
  [SerializeField]
  private UILabel trustUpLabel;
  [SerializeField]
  private Animator trustUpLabelAnimator;
  [SerializeField]
  private GameObject floatingSkillDialogRoot;
  [SerializeField]
  private GameObject callDialogRoot;
  [SerializeField]
  private Transform seaMessageRoot;
  [SerializeField]
  private GameObject seaTalkBtn;
  [SerializeField]
  private GameObject seaTalkBtn2;
  [SerializeField]
  private GameObject badgeTalkSea;
  [SerializeField]
  private GameObject badgeTalkSea2;
  private SeaHomeManager.UnitConrtolleData current2DUnitData;
  private GameObject callPopupPrefab;
  private GameObject presentPopupPrefab;
  private GameObject dateSelectPopupPrefab;
  private GameObject dateConfirmPopupPrefab;
  private GameObject favorabilityRatingEffectPopupPrefab;
  private Sea030HomeCallPopup callPopupObject;
  private GameObject seaMessageObj;
  private SeaNoticeManager noticeManager;
  private GameObject seaHomeTutorial;
  private PopupSkillDetails.Param skillParams;
  private GameObject skillDetailDialogPrefab;
  private Sea030HomeMenu.HomeMode mode;
  private UITweener[] tweeners;
  private NGxFaceSprite unitFace;
  private NGxEyeSprite unitEye;
  private SeaHomeUnitEyeBlink eyeBlink;
  private List<int> serifIndex;
  private int serifIndexCount;
  private SeaDateDateSpotDisplaySetting dateSetting;
  private List<int> dateFlows;
  private int? quizId;
  private int nowFlowIndex;
  private bool existSelect;
  private Dictionary<int, int> eventSelectData = new Dictionary<int, int>();
  public bool isChangeSelectSpot;
  public bool isReturnSelectSpot;
  public bool isSelectedSpot;
  private const int AppreciationToTalkTween = 3012;
  private const int TalkToAppreciationTween = 3021;
  private const int CAMERABUTTON_OFF = 0;
  private const int CAMERABUTTON_ON = 1;
  private bool isInit;
  private int LoginBonusCloseCounter;
  private int LevelUpBonusCloseCounter;
  private int LevelUpBonusCount;
  private bool nowMenuTween;
  private Modified<Player> playerModified;
  private bool isDepartDate;
  private List<ParticleSystem> resultParticleList;
  private List<UISprite> resultUISpriteList;
  private bool isCall;
  private GameObject callTouchEffect;
  private DisplayTouchEffectController displayTouchEffectController;
  private Coroutine coroutine;
  public static bool IsAutoCallButton;

  public SeaDateDateSpotDisplaySetting DateSetting
  {
    get => this.dateSetting;
    set => this.dateSetting = value;
  }

  public List<int> DateFlows
  {
    get => this.dateFlows;
    set => this.dateFlows = value;
  }

  public int? QuizId
  {
    get => this.quizId;
    set => this.quizId = value;
  }

  public int NowFlowIndex
  {
    get => this.nowFlowIndex;
    set => this.nowFlowIndex = value;
  }

  public bool ExistSelect
  {
    get => this.existSelect;
    set => this.existSelect = value;
  }

  public Dictionary<int, int> EventSelectData
  {
    get => this.eventSelectData;
    set => this.eventSelectData = value;
  }

  public bool openningNoticePrefab { get; set; }

  public IEnumerator onStartSceneAsync(bool isDuringDate)
  {
    Sea030HomeMenu sea030HomeMenu1 = this;
    if (sea030HomeMenu1.playerModified == null)
      sea030HomeMenu1.playerModified = SMManager.Observe<Player>();
    ((Component) sea030HomeMenu1.manager).gameObject.SetActive(true);
    sea030HomeMenu1.mode = Sea030HomeMenu.HomeMode.Appreciation;
    sea030HomeMenu1.ActiveHome(true);
    sea030HomeMenu1.talkRoot.SetActive(false);
    sea030HomeMenu1.talkBackButton.SetActive(false);
    sea030HomeMenu1.resultEffectObject.SetActive(false);
    sea030HomeMenu1.resultGaugeEffectObject.SetActive(false);
    sea030HomeMenu1.tweeners = ((Component) sea030HomeMenu1).gameObject.GetComponentsInChildren<UITweener>(true);
    ((IEnumerable<UITweener>) sea030HomeMenu1.tweeners).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      if (x.tweenGroup != 3012 && x.tweenGroup != 3021)
        return;
      x.ResetToBeginning();
    }));
    yield return (object) null;
    if (sea030HomeMenu1.manager.CameraMode == SeaHomeCameraController.CameraMode.NORMAL)
      ((IEnumerable<GameObject>) sea030HomeMenu1.cameraButtons).ToggleOnce(0);
    else
      ((IEnumerable<GameObject>) sea030HomeMenu1.cameraButtons).ToggleOnce(1);
    sea030HomeMenu1.characterNameLabel.SetTextLocalize(string.Empty);
    sea030HomeMenu1.textLabel.SetTextLocalize(string.Empty);
    IEnumerator e = sea030HomeMenu1.manager.Init();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    sea030HomeMenu1.displayTouchEffectController = Singleton<CommonRoot>.GetInstance().TouchEffectController;
    Future<GameObject> loadFt = new ResourceObject("Animations/common/common_Tap/dir_Tap_effect_Sea").Load<GameObject>();
    e = loadFt.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) loadFt.Result, (Object) null))
    {
      sea030HomeMenu1.callTouchEffect = Object.Instantiate<GameObject>(loadFt.Result);
      sea030HomeMenu1.displayTouchEffectController.SetEffect(sea030HomeMenu1.callTouchEffect);
      sea030HomeMenu1.displayTouchEffectController.SetEnable(false);
    }
    Future<GameObject> futureCallPopupPrefab;
    if (Object.op_Equality((Object) sea030HomeMenu1.callPopupPrefab, (Object) null))
    {
      futureCallPopupPrefab = Res.Prefabs.popup.popup_030_sea_mypage_call__anim_fade.Load<GameObject>();
      e = futureCallPopupPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sea030HomeMenu1.callPopupPrefab = futureCallPopupPrefab.Result;
      futureCallPopupPrefab = (Future<GameObject>) null;
    }
    else if (Object.op_Inequality((Object) sea030HomeMenu1.callPopupObject, (Object) null))
    {
      sea030HomeMenu1.callPopupObject.ScrollReset();
      sea030HomeMenu1.callPopupObject.Hide();
      sea030HomeMenu1.manager.AllUnitShow();
      sea030HomeMenu1.manager.SetCameraAutoFocus(true);
      sea030HomeMenu1.IsPush = false;
    }
    if (Object.op_Equality((Object) sea030HomeMenu1.presentPopupPrefab, (Object) null))
    {
      futureCallPopupPrefab = Res.Prefabs.popup.popup_030_sea_present__anim_fade.Load<GameObject>();
      e = futureCallPopupPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sea030HomeMenu1.presentPopupPrefab = futureCallPopupPrefab.Result;
      futureCallPopupPrefab = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) sea030HomeMenu1.dateSelectPopupPrefab, (Object) null))
    {
      futureCallPopupPrefab = Res.Prefabs.popup.popup_030_sea_mypage_date_select__anim_fade.Load<GameObject>();
      e = futureCallPopupPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sea030HomeMenu1.dateSelectPopupPrefab = futureCallPopupPrefab.Result;
      futureCallPopupPrefab = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) sea030HomeMenu1.dateConfirmPopupPrefab, (Object) null))
    {
      futureCallPopupPrefab = Res.Prefabs.popup.popup_030_sea_mypage_date_confirm__anim_fade.Load<GameObject>();
      e = futureCallPopupPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sea030HomeMenu1.dateConfirmPopupPrefab = futureCallPopupPrefab.Result;
      futureCallPopupPrefab = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) sea030HomeMenu1.favorabilityRatingEffectPopupPrefab, (Object) null))
    {
      futureCallPopupPrefab = Res.Animations.extraskill.FavorabilityRatingEffect.Load<GameObject>();
      e = futureCallPopupPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sea030HomeMenu1.favorabilityRatingEffectPopupPrefab = futureCallPopupPrefab.Result;
      futureCallPopupPrefab = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) sea030HomeMenu1.skillDetailDialogPrefab, (Object) null))
    {
      futureCallPopupPrefab = PopupSkillDetails.createPrefabLoader(true);
      e = futureCallPopupPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sea030HomeMenu1.skillDetailDialogPrefab = futureCallPopupPrefab.Result;
      futureCallPopupPrefab = (Future<GameObject>) null;
    }
    if (isDuringDate)
    {
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.SeaDateResume> futureAPI = WebAPI.SeaDateResume(new Action<WebAPI.Response.UserError>(sea030HomeMenu1.\u003ConStartSceneAsync\u003Eb__102_1));
      e = futureAPI.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.id == futureAPI.Result.player_unit_id));
      sea030HomeMenu1.existSelect = futureAPI.Result.happening_id > 0 || futureAPI.Result.quiz_id > 0;
      sea030HomeMenu1.dateFlows = ((IEnumerable<int>) futureAPI.Result.script_ids).ToList<int>();
      sea030HomeMenu1.nowFlowIndex = 0;
      sea030HomeMenu1.eventSelectData.Clear();
      sea030HomeMenu1.quizId = Sea030HomeMenu.hasQuiz(futureAPI.Result.date_flow) ? new int?(futureAPI.Result.quiz_id) : new int?();
      sea030HomeMenu1.dateSetting = ((IEnumerable<SeaDateDateSpotDisplaySetting>) MasterData.SeaDateDateSpotDisplaySettingList).FirstOrDefault<SeaDateDateSpotDisplaySetting>((Func<SeaDateDateSpotDisplaySetting, bool>) (x => x.datespot.ID == futureAPI.Result.spot.ID && x.time_zone.WithIn(futureAPI.Result.started_at)));
      e = sea030HomeMenu1.InternalChangeTalkMode(new SeaHomeManager.UnitConrtolleData(playerUnit.unit.ID, playerUnit));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      List<int> dateFlows = sea030HomeMenu1.dateFlows;
      Sea030HomeMenu sea030HomeMenu2 = sea030HomeMenu1;
      int nowFlowIndex = sea030HomeMenu1.nowFlowIndex;
      int num = nowFlowIndex + 1;
      sea030HomeMenu2.nowFlowIndex = num;
      int index = nowFlowIndex;
      Story0093Scene.changeSceneModeDate(true, dateFlows[index], sea030HomeMenu1.current2DUnitData.Unit, sea030HomeMenu1.dateSetting.GetImageHash(), new Action<int, int>(sea030HomeMenu1.EventSelected), isSea: new bool?(true), questionId: sea030HomeMenu1.quizId);
    }
    sea030HomeMenu1.seaTalkBtn2.SetActive(sea030HomeMenu1.mode == Sea030HomeMenu.HomeMode.Talk && SMManager.Get<SeaPlayer>().is_released_sea_call);
    SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
    if (seaPlayer != null)
      sea030HomeMenu1.seaTalkBtn.SetActive(seaPlayer.is_released_sea_call);
    if (Singleton<NGGameDataManager>.GetInstance().playerTalkMessage != null && (Persist.seaHomeUnitDate.Data.messageID != Singleton<NGGameDataManager>.GetInstance().playerTalkMessage.message_id || Persist.seaHomeUnitDate.Data.messageUnitID != Singleton<NGGameDataManager>.GetInstance().playerTalkMessage.same_character_id || Persist.seaHomeUnitDate.Data.timeStamp != SA_Unix_Time.ToUnixTime(Singleton<NGGameDataManager>.GetInstance().playerTalkMessage.created_at)))
      Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage = true;
    sea030HomeMenu1.UpdateBadgeTalk(seaPlayer);
    if (!isDuringDate)
      sea030HomeMenu1.OpenSeaMessage();
    if (Sea030HomeMenu.IsAutoCallButton)
    {
      Sea030HomeMenu.IsAutoCallButton = false;
      sea030HomeMenu1.OnCall();
    }
    sea030HomeMenu1.isInit = true;
  }

  public IEnumerator onBackSceneAsync()
  {
    Sea030HomeMenu sea030HomeMenu1 = this;
    bool flag1 = false;
    IEnumerator e1;
    if (!sea030HomeMenu1.isInit)
    {
      e1 = sea030HomeMenu1.onStartSceneAsync(false);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      flag1 = true;
    }
    if (sea030HomeMenu1.dateFlows != null && !sea030HomeMenu1.isSelectedSpot)
    {
      if (sea030HomeMenu1.dateFlows.Count > sea030HomeMenu1.nowFlowIndex)
      {
        List<int> dateFlows = sea030HomeMenu1.dateFlows;
        Sea030HomeMenu sea030HomeMenu2 = sea030HomeMenu1;
        int nowFlowIndex = sea030HomeMenu1.nowFlowIndex;
        int num = nowFlowIndex + 1;
        sea030HomeMenu2.nowFlowIndex = num;
        int index = nowFlowIndex;
        Story0093Scene.changeSceneModeDate(true, dateFlows[index], sea030HomeMenu1.current2DUnitData.Unit, sea030HomeMenu1.dateSetting.GetImageHash(), new Action<int, int>(sea030HomeMenu1.EventSelected), isSea: new bool?(true), questionId: sea030HomeMenu1.quizId);
      }
      else if (sea030HomeMenu1.existSelect && sea030HomeMenu1.eventSelectData.Count > 0)
      {
        Sea030HomeMenu sea030HomeMenu = sea030HomeMenu1;
        bool isSeaSeasonEnd = false;
        Future<WebAPI.Response.SeaDateChoice> futureAPI = WebAPI.SeaDateChoice(sea030HomeMenu1.eventSelectData.OrderBy<KeyValuePair<int, int>, int>((Func<KeyValuePair<int, int>, int>) (x => x.Key)).Last<KeyValuePair<int, int>>().Value + 1, (Action<WebAPI.Response.UserError>) (e =>
        {
          if (string.Equals(e.Code, "SEA000"))
          {
            isSeaSeasonEnd = true;
            sea030HomeMenu.StartCoroutine(PopupUtility.SeaError(e));
          }
          else
            WebAPI.DefaultUserErrorCallback(e);
        }));
        e1 = futureAPI.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (!isSeaSeasonEnd)
        {
          sea030HomeMenu1.existSelect = futureAPI.Result.happening_id > 0 || futureAPI.Result.quiz_id > 0;
          sea030HomeMenu1.dateFlows = ((IEnumerable<int>) futureAPI.Result.script_ids).ToList<int>();
          sea030HomeMenu1.nowFlowIndex = 0;
          sea030HomeMenu1.eventSelectData.Clear();
          sea030HomeMenu1.quizId = Sea030HomeMenu.hasQuiz(futureAPI.Result.date_flow) ? new int?(futureAPI.Result.quiz_id) : new int?();
          List<int> dateFlows = sea030HomeMenu1.dateFlows;
          Sea030HomeMenu sea030HomeMenu3 = sea030HomeMenu1;
          int nowFlowIndex = sea030HomeMenu1.nowFlowIndex;
          int num = nowFlowIndex + 1;
          sea030HomeMenu3.nowFlowIndex = num;
          int index = nowFlowIndex;
          Story0093Scene.changeSceneModeDate(true, dateFlows[index], sea030HomeMenu1.current2DUnitData.Unit, sea030HomeMenu1.dateSetting.GetImageHash(), new Action<int, int>(sea030HomeMenu1.EventSelected), isSea: new bool?(true), questionId: sea030HomeMenu1.quizId);
        }
      }
      else
      {
        Sea030HomeMenu sea030HomeMenu = sea030HomeMenu1;
        bool isSeaSeasonEnd = false;
        Future<WebAPI.Response.SeaDateFinish> futureAPI = WebAPI.SeaDateFinish((Action<WebAPI.Response.UserError>) (e =>
        {
          if (string.Equals(e.Code, "SEA000"))
          {
            isSeaSeasonEnd = true;
            sea030HomeMenu.StartCoroutine(PopupUtility.SeaError(e));
          }
          else
          {
            WebAPI.DefaultUserErrorCallback(e);
            MypageScene.ChangeSceneOnError();
          }
        }));
        e1 = futureAPI.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (!isSeaSeasonEnd)
        {
          float trust_up = futureAPI.Result.trust_up;
          // ISSUE: reference to a compiler-generated method
          PlayerUnit updatePlayerUnit = ((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).FirstOrDefault<PlayerUnit>(new Func<PlayerUnit, bool>(sea030HomeMenu1.\u003ConBackSceneAsync\u003Eb__103_3));
          if (updatePlayerUnit != (PlayerUnit) null)
          {
            trust_up = updatePlayerUnit.trust_rate - sea030HomeMenu1.current2DUnitData.PlayerUnit.trust_rate;
            sea030HomeMenu1.current2DUnitData.PlayerUnit = updatePlayerUnit;
            sea030HomeMenu1.SetUnitData(sea030HomeMenu1.current2DUnitData);
          }
          bool flag2 = false;
          if (futureAPI.Result.player_call_letters != null)
          {
            Singleton<NGGameDataManager>.GetInstance().callLetter = futureAPI.Result.player_call_letters;
            foreach (PlayerCallLetter playerCallLetter in futureAPI.Result.player_call_letters)
            {
              if (playerCallLetter.same_character_id == updatePlayerUnit.unit.same_character_id)
                flag2 = true;
            }
          }
          if (futureAPI.Result.latest_talk_message != null)
          {
            Singleton<NGGameDataManager>.GetInstance().playerTalkMessage = futureAPI.Result.latest_talk_message;
            sea030HomeMenu1.OpenSeaMessage();
            if (Persist.seaHomeUnitDate.Data.messageID != futureAPI.Result.latest_talk_message.message_id || Persist.seaHomeUnitDate.Data.messageUnitID != futureAPI.Result.latest_talk_message.same_character_id || Persist.seaHomeUnitDate.Data.timeStamp != SA_Unix_Time.ToUnixTime(futureAPI.Result.latest_talk_message.created_at))
              Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage = true;
            sea030HomeMenu1.UpdateBadgeTalk(SMManager.Get<SeaPlayer>());
          }
          bool flag3 = false;
          CallCharacter[] callCharacterList = MasterData.CallCharacterList;
          DateTime dateTime1 = TimeZoneInfo.ConvertTime(ServerTime.NowAppTimeAddDelta(), Japan.CreateTimeZone());
          foreach (CallCharacter callCharacter in callCharacterList)
          {
            if (callCharacter.same_character_id == updatePlayerUnit.unit.same_character_id)
            {
              DateTime dateTime2 = dateTime1;
              DateTime? startAt = callCharacter.start_at;
              if ((startAt.HasValue ? (dateTime2 > startAt.GetValueOrDefault() ? 1 : 0) : 0) != 0)
                flag3 = true;
            }
          }
          SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
          if (((seaPlayer == null || !seaPlayer.is_released_sea_call || flag2 ? 0 : ((double) updatePlayerUnit.trust_rate >= (double) float.Parse(Consts.GetInstance().CALL_TRUST_NUM) ? 1 : 0)) & (flag3 ? 1 : 0)) != 0)
          {
            Future<GameObject> prefabF = new ResourceObject("Prefabs/sea030_talk/popup_030_sea_TalkIDExchange__anim_fade").Load<GameObject>();
            e1 = prefabF.Wait();
            while (e1.MoveNext())
              yield return e1.Current;
            e1 = (IEnumerator) null;
            GameObject result = prefabF.Result;
            GameObject go = Singleton<PopupManager>.GetInstance().open(result, isViewBack: false, clip: "SE_1044");
            // ISSUE: reference to a compiler-generated method
            yield return (object) go.GetComponent<Sea030CommonPopup>().initialize(updatePlayerUnit, futureAPI.Result.player_call_letters, new Action<PlayerTalkMessage>(sea030HomeMenu1.\u003ConBackSceneAsync\u003Eb__103_4));
            yield return (object) Singleton<PopupManager>.GetInstance().monitorCoroutine(go.GetComponent<Sea030CommonPopup>().PopupEnd());
            prefabF = (Future<GameObject>) null;
            go = (GameObject) null;
          }
          SeaDateResult seaDateResult = sea030HomeMenu1.GetSeaDateResult(futureAPI.Result.trust_up);
          if (seaDateResult != null)
          {
            sea030HomeMenu1.SetSerif(seaDateResult.serif, seaDateResult.face, seaDateResult.voice_cue_name);
            sea030HomeMenu1.PlayLoveResultEffect(seaDateResult.result_staging.home_result, trust_up, futureAPI.Result.gain_trust_result, updatePlayerUnit);
            sea030HomeMenu1.eyeBlink.StartBlink(2f);
          }
          ((Component) sea030HomeMenu1.manager).gameObject.SetActive(true);
          sea030HomeMenu1.dateFlows = (List<int>) null;
          sea030HomeMenu1.existSelect = false;
          sea030HomeMenu1.nowFlowIndex = 0;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        }
      }
    }
    else
    {
      ((Component) sea030HomeMenu1.manager).gameObject.SetActive(true);
      if (!sea030HomeMenu1.manager.isUnitInit && !flag1)
      {
        e1 = sea030HomeMenu1.manager.Init();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
      if (sea030HomeMenu1.isReturnSelectSpot || sea030HomeMenu1.isSelectedSpot)
        sea030HomeMenu1.StartCoroutine(sea030HomeMenu1.onBackForSelectSpot());
      if (Object.op_Inequality((Object) sea030HomeMenu1.callPopupObject, (Object) null) && sea030HomeMenu1.callPopupObject.IsInitialize)
      {
        if (sea030HomeMenu1.callPopupObject.VerifyUnitLength())
        {
          sea030HomeMenu1.callPopupObject.UpdateUnitData();
        }
        else
        {
          e1 = sea030HomeMenu1.manager.Init();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          Object.Destroy((Object) ((Component) sea030HomeMenu1.callPopupObject).gameObject);
          sea030HomeMenu1.callPopupObject = (Sea030HomeCallPopup) null;
          e1 = sea030HomeMenu1.OpneCallPopup();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
      }
      if (Object.op_Inequality((Object) sea030HomeMenu1.callPopupObject, (Object) null) && sea030HomeMenu1.callPopupObject.IsShow)
        sea030HomeMenu1.displayTouchEffectController.SetEnable(false);
      else
        sea030HomeMenu1.displayTouchEffectController.SetEnable(sea030HomeMenu1.isCall);
    }
  }

  private void RefreshTalkMessage(PlayerTalkMessage latest_talk_message)
  {
    if (latest_talk_message != null)
    {
      Singleton<NGGameDataManager>.GetInstance().playerTalkMessage = latest_talk_message;
      if (Persist.seaHomeUnitDate.Data.messageID != latest_talk_message.message_id || Persist.seaHomeUnitDate.Data.messageUnitID != latest_talk_message.same_character_id || Persist.seaHomeUnitDate.Data.timeStamp != SA_Unix_Time.ToUnixTime(latest_talk_message.created_at))
        Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage = true;
    }
    this.OpenSeaMessage();
    this.UpdateBadgeTalk(SMManager.Get<SeaPlayer>());
  }

  private IEnumerator onBackForSelectSpot()
  {
    Sea030HomeMenu sea030HomeMenu1 = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) new WaitForSeconds(0.3f);
    if (sea030HomeMenu1.isSelectedSpot)
    {
      while (Singleton<CommonRoot>.GetInstance().isTouchBlock)
        yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    }
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    if (sea030HomeMenu1.isSelectedSpot)
    {
      NGSoundManager sm = Singleton<NGSoundManager>.GetInstance();
      int playVoiceIndex = -1;
      SeaDateSerifAtDepart serifDepart = sea030HomeMenu1.GetSerifDepart();
      if (serifDepart != null)
        playVoiceIndex = sea030HomeMenu1.SetSerif(serifDepart.serif, serifDepart.face, serifDepart.voice_cue_name, new Hashtable()
        {
          {
            (object) "dateName",
            (object) sea030HomeMenu1.dateSetting.date_name
          }
        });
      sm.PlaySe("SE_2702");
      yield return (object) new WaitForSeconds(1f);
      while (playVoiceIndex >= 0 && sm.IsVoicePlaying(playVoiceIndex))
        yield return (object) null;
      Singleton<CommonRoot>.GetInstance().isActiveHeader = false;
      List<int> dateFlows = sea030HomeMenu1.dateFlows;
      Sea030HomeMenu sea030HomeMenu2 = sea030HomeMenu1;
      int nowFlowIndex = sea030HomeMenu1.nowFlowIndex;
      int num = nowFlowIndex + 1;
      sea030HomeMenu2.nowFlowIndex = num;
      int index = nowFlowIndex;
      Story0093Scene.changeSceneModeDate(true, dateFlows[index], sea030HomeMenu1.current2DUnitData.Unit, sea030HomeMenu1.dateSetting.GetImageHash(), new Action<int, int>(sea030HomeMenu1.EventSelected), isSea: new bool?(true), questionId: sea030HomeMenu1.quizId);
      sm = (NGSoundManager) null;
    }
    if (sea030HomeMenu1.current2DUnitData != null)
      sea030HomeMenu1.SetUnitData(sea030HomeMenu1.current2DUnitData);
    sea030HomeMenu1.isReturnSelectSpot = false;
    sea030HomeMenu1.isSelectedSpot = false;
  }

  public void onStartScene()
  {
    this.InitializeResultEffectList<ParticleSystem>(ref this.resultParticleList);
    this.InitializeResultEffectList<UISprite>(ref this.resultUISpriteList);
    this.manager.ResetAmbientLight();
  }

  private void InitializeResultEffectList<T>(ref List<T> list) where T : Component
  {
    if (list != null)
      return;
    list = new List<T>((IEnumerable<T>) this.resultEffectObject.GetComponentsInChildren<T>(true));
    list.AddRange((IEnumerable<T>) this.resultGaugeEffectObject.GetComponentsInChildren<T>(true));
  }

  private void StopResultEffectList()
  {
    ((Behaviour) this.resultEffectAnimator).enabled = false;
    ((Behaviour) this.resultGaugeEffectAnimator).enabled = false;
    this.resultParticleList.ForEach((Action<ParticleSystem>) (p =>
    {
      p.Stop(true, (ParticleSystemStopBehavior) 0);
      ((Component) p).gameObject.SetActive(false);
    }));
    this.resultUISpriteList.ForEach((Action<UISprite>) (s => ((Component) s).gameObject.SetActive(false)));
    ((Behaviour) this.trustUpLabelAnimator).enabled = false;
    ((Component) this.trustUpLabel).gameObject.SetActive(false);
  }

  private void ActiveHome(bool state, bool camBtn = false)
  {
    if (camBtn)
    {
      this.buttonsRoot.SetActive(state);
      this.seaTalkBtn.SetActive(state && SMManager.Get<SeaPlayer>().is_released_sea_call);
    }
    else
      this.homeRoot.SetActive(state);
  }

  public void onEndScene()
  {
    this.StopResultEffectList();
    this.StopCoroutine("LoadUnit2D");
    if (!this.ExistDateFlows() && !this.isChangeSelectSpot && this.mode == Sea030HomeMenu.HomeMode.Talk)
      this.ChangeAppreciationMode();
    this.manager.onEndScene();
    ((Component) this.manager).gameObject.SetActive(false);
    this.displayTouchEffectController.SetEnable(false);
    this.isChangeSelectSpot = false;
  }

  public void OnChangeCameraMode()
  {
    if (this.IsPush)
      return;
    this.manager.ChangeCameraMode();
    if (this.manager.CameraMode == SeaHomeCameraController.CameraMode.NORMAL)
    {
      ((IEnumerable<GameObject>) this.cameraButtons).ToggleOnce(0);
      this.ActiveHome(true, true);
    }
    else
    {
      ((IEnumerable<GameObject>) this.cameraButtons).ToggleOnce(1);
      this.ActiveHome(false, true);
    }
  }

  public void OnChangeTalkMode(SeaHomeManager.UnitConrtolleData unitData)
  {
    if (this.IsPush)
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1040");
    this.ChangeTalkMode(unitData);
  }

  public void OnCall()
  {
    if (this.mode == Sea030HomeMenu.HomeMode.Talk || this.IsPushAndSet())
      return;
    this.manager.AllUnitHide();
    this.manager.SetCameraAutoFocus(false);
    if (this.manager.CameraMode == SeaHomeCameraController.CameraMode.NORMAL)
      ((IEnumerable<GameObject>) this.cameraButtons).ToggleOnce(0);
    else
      ((IEnumerable<GameObject>) this.cameraButtons).ToggleOnce(1);
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.loadingMode = 1;
    instance.isLoading = true;
    this.StartCoroutine("OpneCallPopup");
  }

  public void OnSeaQuest()
  {
    if (this.mode == Sea030HomeMenu.HomeMode.Talk || this.IsPushAndSet())
      return;
    Singleton<NGGameDataManager>.GetInstance().IsSea = true;
    if (!(Singleton<NGSceneManager>.GetInstance().sceneName != "sea030_quest"))
      return;
    this.ClearSceneStacks();
    Sea030_questScene.ChangeScene(true);
  }

  private void ClearSceneStacks()
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    instance.sceneBase.IsPush = true;
    instance.destroyLoadedScenes();
    instance.clearStack();
  }

  public void UpdateBadgeTalk(SeaPlayer seaPlayer)
  {
    this.badgeTalkSea.SetActive(seaPlayer.is_released_sea_call && Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage);
    this.badgeTalkSea2.SetActive(seaPlayer.is_released_sea_call && Singleton<NGGameDataManager>.GetInstance().unReadTalkMessage);
  }

  private IEnumerator OpneCallPopup()
  {
    Sea030HomeMenu sea030HomeMenu = this;
    if (Object.op_Equality((Object) sea030HomeMenu.callPopupObject, (Object) null))
    {
      GameObject gameObject = sea030HomeMenu.callPopupPrefab.Clone(sea030HomeMenu.callDialogRoot.transform);
      sea030HomeMenu.callPopupObject = gameObject.GetComponent<Sea030HomeCallPopup>();
    }
    IEnumerator e;
    if (Object.op_Inequality((Object) sea030HomeMenu.callPopupObject, (Object) null) && !sea030HomeMenu.callPopupObject.IsInitialize)
    {
      // ISSUE: reference to a compiler-generated method
      e = sea030HomeMenu.callPopupObject.Init(((IEnumerable<SeaHomeUnitController>) sea030HomeMenu.manager.UnitControlers).Select<SeaHomeUnitController, SeaHomeManager.UnitConrtolleData>((Func<SeaHomeUnitController, SeaHomeManager.UnitConrtolleData>) (x => x.UnitData)).ToArray<SeaHomeManager.UnitConrtolleData>(), new Action<SeaHomeManager.UnitConrtolleData>(sea030HomeMenu.ChangeTalkMode), new Action(sea030HomeMenu.\u003COpneCallPopup\u003Eb__117_1), sea030HomeMenu.manager.IsShowAllGuest);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = sea030HomeMenu.callPopupObject.UpdateUnitIcon();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.isLoading = false;
    instance.loadingMode = 0;
    sea030HomeMenu.callPopupObject.Show();
    sea030HomeMenu.callPopupObject.StartLoadThum();
  }

  public void OnPresent()
  {
    if (this.mode == Sea030HomeMenu.HomeMode.Appreciation || this.IsPushAndSet())
      return;
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.loadingMode = 1;
    instance.isLoading = true;
    this.StartCoroutine("OpnePresentPopup");
  }

  private IEnumerator OpnePresentPopup()
  {
    Sea030HomeMenu sea030HomeMenu = this;
    GameObject popup = sea030HomeMenu.presentPopupPrefab.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
    IEnumerator e = popup.GetComponent<Sea030HomePresentPopup>().Init(sea030HomeMenu.current2DUnitData.PlayerUnit, new Action<GameCore.ItemInfo, int>(sea030HomeMenu.PresentGive));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    CommonRoot instance = Singleton<CommonRoot>.GetInstance();
    instance.isLoading = false;
    instance.loadingMode = 0;
    popup.SetActive(false);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    popup.SetActive(true);
  }

  private void OpenSeaMessage()
  {
    if (Singleton<NGGameDataManager>.GetInstance().playerTalkMessage == null)
    {
      if (!Object.op_Inequality((Object) this.noticeManager, (Object) null))
        return;
      ((Component) this.noticeManager).gameObject.SetActive(false);
    }
    else
    {
      if (Object.op_Inequality((Object) this.noticeManager, (Object) null))
        Object.DestroyImmediate((Object) ((Component) this.noticeManager).gameObject);
      this.coroutine = this.StartCoroutine(this.OpenNoticePrefab());
    }
  }

  public void OnDate()
  {
    if (this.mode == Sea030HomeMenu.HomeMode.Appreciation || this.IsPushAndSet())
      return;
    this.isChangeSelectSpot = true;
    if (this.coroutine != null)
      this.StopCoroutine(this.coroutine);
    Sea030DateScene.changeScene(true, this, this.current2DUnitData);
    if (!Object.op_Inequality((Object) this.noticeManager, (Object) null))
      return;
    Object.DestroyImmediate((Object) ((Component) this.noticeManager).gameObject);
  }

  public void OnDateBadMood()
  {
    Singleton<NGMessageUI>.GetInstance().SetMessageByPosType(Consts.GetInstance().CALL_HOME_BAD_MOOD_PUSH_BUTTON);
  }

  public void OnBack()
  {
    if (this.IsPush)
      return;
    this.ChangeAppreciationMode();
  }

  public void OnClicExtraSKill()
  {
    PlayerUnit playerUnit = this.current2DUnitData.PlayerUnit;
    UnitSkillAwake[] awakeSkills = playerUnit.GetAwakeSkills();
    if (awakeSkills == null || awakeSkills.Length == 0)
      return;
    UnitSkillAwake s = awakeSkills[0];
    this.skillParams = new PopupSkillDetails.Param(s.skill, UnitParameter.SkillGroup.Extra);
    string empty = string.Empty;
    if ((double) s.need_affection > (double) playerUnit.trust_rate)
    {
      Consts.Format(Consts.GetInstance().popup_004_ExtraSkill_affection_condition, (IDictionary) new Hashtable()
      {
        {
          (object) "percent",
          (object) s.need_affection
        }
      });
    }
    else
    {
      string affectionComplete = Consts.GetInstance().popup_004_ExtraSkill_affection_complete;
    }
    PopupSkillDetails.show(this.skillDetailDialogPrefab, PopupSkillDetails.Param.createByUnitView(s, playerUnit));
  }

  private void FinishTween()
  {
    this.nowMenuTween = false;
    this.ActiveHome(this.mode == Sea030HomeMenu.HomeMode.Appreciation);
    this.talkRoot.SetActive(this.mode == Sea030HomeMenu.HomeMode.Talk);
    this.talkBackButton.SetActive(this.mode == Sea030HomeMenu.HomeMode.Talk);
  }

  public void ChangeTalkMode(SeaHomeManager.UnitConrtolleData unitData)
  {
    this.IsPush = false;
    if (this.mode == Sea030HomeMenu.HomeMode.Talk)
      return;
    List<SwitchUnitComponentBase> list = ((IEnumerable<SwitchUnitComponentBase>) ((Component) this).GetComponentsInChildren<SwitchUnitComponentBase>(true)).ToList<SwitchUnitComponentBase>();
    for (int index = 0; index < list.Count; ++index)
      list[index].SwitchMaterial(unitData.UnitID);
    this.mode = Sea030HomeMenu.HomeMode.Talk;
    this.talkRoot.SetActive(true);
    this.talkBackButton.SetActive(true);
    SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
    if (seaPlayer != null)
      this.seaTalkBtn2.SetActive(seaPlayer.is_released_sea_call);
    this.manager.AllUnitHide();
    this.manager.SetCameraAutoFocus(false);
    this.characterNameLabel.SetTextLocalize(unitData.Unit.name);
    this.textLabel.SetTextLocalize(string.Empty);
    if (this.current2DUnitData == null || this.current2DUnitData != null && this.current2DUnitData.UnitID != unitData.UnitID)
    {
      this.current2DUnitData = unitData;
      this.StartCoroutine("LoadUnit2D", (object) unitData);
    }
    else
    {
      this.current2DUnitData = unitData;
      this.SetUnitData(unitData);
      this.StartCoroutine(this.waitTweenPlaySerif());
    }
    this.tweeners = ((IEnumerable<UITweener>) this.tweeners).Where<UITweener>((Func<UITweener, bool>) (x => Object.op_Inequality((Object) x, (Object) null))).ToArray<UITweener>();
    ((IEnumerable<UITweener>) this.tweeners).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.onFinished.Clear();
      ((Behaviour) x).enabled = false;
    }));
    if (!NGTween.playTweens(this.tweeners, 3012))
      return;
    this.nowMenuTween = true;
    UITweener uiTweener = ((IEnumerable<UITweener>) this.tweeners).Where<UITweener>((Func<UITweener, bool>) (x => ((Component) x).gameObject.activeInHierarchy && ((Behaviour) x).enabled)).OrderByDescending<UITweener, float>((Func<UITweener, float>) (x => x.delay + x.duration)).FirstOrDefault<UITweener>();
    if (!Object.op_Inequality((Object) uiTweener, (Object) null))
      return;
    uiTweener.onFinished.Add(new EventDelegate(new EventDelegate.Callback(this.FinishTween)));
  }

  public IEnumerator InternalChangeTalkMode(SeaHomeManager.UnitConrtolleData unitData)
  {
    Sea030HomeMenu sea030HomeMenu = this;
    sea030HomeMenu.mode = Sea030HomeMenu.HomeMode.Talk;
    sea030HomeMenu.ActiveHome(false);
    List<SwitchUnitComponentBase> list = ((IEnumerable<SwitchUnitComponentBase>) ((Component) sea030HomeMenu).GetComponentsInChildren<SwitchUnitComponentBase>(true)).ToList<SwitchUnitComponentBase>();
    for (int index = 0; index < list.Count; ++index)
      list[index].SwitchMaterial(unitData.UnitID);
    sea030HomeMenu.talkRoot.SetActive(true);
    sea030HomeMenu.talkBackButton.SetActive(true);
    sea030HomeMenu.manager.AllUnitHide();
    sea030HomeMenu.manager.SetCameraAutoFocus(false);
    sea030HomeMenu.characterNameLabel.SetTextLocalize(unitData.Unit.name);
    sea030HomeMenu.textLabel.SetTextLocalize(string.Empty);
    if (sea030HomeMenu.current2DUnitData == null || sea030HomeMenu.current2DUnitData != null && sea030HomeMenu.current2DUnitData.UnitID != unitData.UnitID)
    {
      sea030HomeMenu.current2DUnitData = unitData;
      IEnumerator e = sea030HomeMenu.LoadUnit2D(sea030HomeMenu.current2DUnitData, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      sea030HomeMenu.current2DUnitData = unitData;
      sea030HomeMenu.SetUnitData(unitData);
    }
  }

  private void SetUnitData(SeaHomeManager.UnitConrtolleData unitData)
  {
    Player player = SMManager.Get<Player>();
    if (unitData.PlayerUnit == (PlayerUnit) null)
    {
      this.BtnDate.EnableImmediately(false);
      this.BtnDateBadMood.EnableImmediately(false);
      this.BtnPresent.EnableImmediately(false);
      this.GaugeObject.SetActive(false);
      ((Component) this.BtnDateBadMood).gameObject.SetActive(false);
      ((Component) this.BtnDate).gameObject.SetActive(true);
    }
    else
    {
      bool flag1 = player.IsSeaDate() && !((IEnumerable<SeaDateProhibition>) MasterData.SeaDateProhibitionList).Any<SeaDateProhibition>((Func<SeaDateProhibition, bool>) (x =>
      {
        int? characterIdUnitUnit = x.same_character_id_UnitUnit;
        int sameCharacterId = unitData.PlayerUnit.unit.same_character_id;
        return characterIdUnitUnit.GetValueOrDefault() == sameCharacterId & characterIdUnitUnit.HasValue;
      }));
      if (this.IsBadMood(unitData))
      {
        ((Component) this.BtnDateBadMood).gameObject.SetActive(true);
        ((Component) this.BtnDate).gameObject.SetActive(false);
        this.BtnDateBadMood.EnableImmediately(flag1);
      }
      else
      {
        ((Component) this.BtnDateBadMood).gameObject.SetActive(false);
        ((Component) this.BtnDate).gameObject.SetActive(true);
        this.BtnDate.EnableImmediately(flag1);
      }
      this.BtnPresent.EnableImmediately(true);
      this.GaugeObject.SetActive(true);
      this.StartCoroutine(this.loveGauge.setValue((int) unitData.PlayerUnit.trust_rate, (int) unitData.PlayerUnit.trust_rate, (int) unitData.PlayerUnit.trust_max_rate, (int) Consts.GetInstance().TRUST_RATE_LEVEL_SIZE, false));
      this.trustLabel.SetTextLocalize(string.Format("{0}{1}", (object) (Math.Round((double) unitData.PlayerUnit.trust_rate * 100.0) / 100.0), (object) Consts.GetInstance().PERCENT));
      if (unitData.PlayerUnit != (PlayerUnit) null)
      {
        UnitSkillAwake[] awakeSkills = unitData.PlayerUnit.GetAwakeSkills();
        bool flag2 = awakeSkills != null && awakeSkills.Length != 0;
        this.dirExtraSkill.SetActive(flag2);
        if (flag2)
        {
          if ((double) awakeSkills[0].need_affection <= (double) unitData.PlayerUnit.trust_rate)
            ((UIWidget) this.slcExtraSkill).color = Color.white;
          else
            ((UIWidget) this.slcExtraSkill).color = Color.gray;
        }
      }
    }
    this.commingSoon.SetActive(!player.IsSeaDate());
    bool flag = false;
    if (unitData.PlayerUnit != (PlayerUnit) null)
    {
      if (Mathf.Approximately(unitData.PlayerUnit.trust_rate, (float) PlayerUnit.GetTrustRateMax()))
      {
        try
        {
          if (!Persist.seaHomeUnitDate.Data.TrustMaxSameUnitIDs.Contains(unitData.Unit.same_character_id))
          {
            flag = true;
            Persist.seaHomeUnitDate.Data.TrustMaxSameUnitIDs.Add(unitData.Unit.same_character_id);
            Persist.seaHomeUnitDate.Flush();
          }
        }
        catch
        {
          Persist.seaHomeUnitDate.Delete();
        }
      }
    }
    int trustRate = this.IsBadMood(unitData) ? 0 : (int) unitData.PlayerUnit.trust_rate;
    this.serifIndexCount = 0;
    SeaHomeSerif[] serifs = ((IEnumerable<SeaHomeSerif>) MasterData.SeaHomeSerifList).GetSerifs(ServerTime.NowAppTimeAddDelta(), unitData.Unit, unitData.PlayerUnit != (PlayerUnit) null, trustRate);
    this.serifIndex = new List<int>();
    foreach (SeaHomeSerif seaHomeSerif in serifs)
    {
      for (int index = 0; index < seaHomeSerif.weiht; ++index)
        this.serifIndex.Add(seaHomeSerif.ID);
    }
    this.serifIndex = this.serifIndex.Shuffle<int>().ToList<int>();
    if (flag)
    {
      SeaHomeSerif seaHomeSerif = ((IEnumerable<SeaHomeSerif>) serifs).FirstOrDefault<SeaHomeSerif>((Func<SeaHomeSerif, bool>) (x => x.is_rare));
      if (seaHomeSerif != null)
        this.serifIndex.Insert(0, seaHomeSerif.ID);
    }
    this.isCall = unitData.PlayerUnit != (PlayerUnit) null && player.IsCalledUnit(unitData.PlayerUnit.unit.same_character_id);
    if (this.mode != Sea030HomeMenu.HomeMode.Talk)
      this.displayTouchEffectController.SetEnable(false);
    else
      this.displayTouchEffectController.SetEnable(this.isCall);
  }

  private IEnumerator LoadUnit2D(SeaHomeManager.UnitConrtolleData unitData)
  {
    IEnumerator e = this.LoadUnit2D(unitData, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadUnit2D(SeaHomeManager.UnitConrtolleData unitData, bool playSerif)
  {
    this.dirExtraSkill.SetActive(false);
    this.SetUnitData(unitData);
    UnitUnit Unit = unitData.Unit;
    this.dynUnit2D.transform.Clear();
    Future<GameObject> future = Unit.LoadStory();
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite sprite = (Sprite) null;
    Future<Sprite> spriteFuture;
    if (Unit.ExistSpriteStory())
    {
      spriteFuture = Unit.LoadSpriteStory();
      e = spriteFuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sprite = spriteFuture.Result;
      spriteFuture = (Future<Sprite>) null;
    }
    GameObject gameObject = future.Result.Clone(this.dynUnit2D.transform);
    Unit.SetStoryData(gameObject);
    NGxMaskSpriteWithScale component1 = gameObject.GetComponent<NGxMaskSpriteWithScale>();
    if (Object.op_Inequality((Object) sprite, (Object) null))
      component1.MainUI2DSprite.sprite2D = sprite;
    component1.SetMaskEnable(false);
    UIWidget component2 = this.dynUnit2D.GetComponent<UIWidget>();
    if (Object.op_Inequality((Object) component2, (Object) null))
    {
      UIWidget w = gameObject.GetComponent<UIWidget>();
      w.depth = component2.depth;
      UIWidget[] componentsInChildren = gameObject.GetComponentsInChildren<UIWidget>();
      ((IEnumerable<UIWidget>) componentsInChildren).Where<UIWidget>((Func<UIWidget, bool>) (v => ((Object) ((Component) v).transform).name == "face")).ForEach<UIWidget>((Action<UIWidget>) (v => v.depth = w.depth + 1));
      ((IEnumerable<UIWidget>) componentsInChildren).Where<UIWidget>((Func<UIWidget, bool>) (v => ((Object) ((Component) v).transform).name == "eye")).ForEach<UIWidget>((Action<UIWidget>) (v => v.depth = w.depth + 2));
    }
    this.eyeBlink = gameObject.GetOrAddComponent<SeaHomeUnitEyeBlink>();
    this.unitFace = gameObject.GetComponent<NGxFaceSprite>();
    this.unitEye = gameObject.GetComponent<NGxEyeSprite>();
    this.eyeBlink.Init(Unit, this.unitFace, this.unitEye);
    Future<Sprite> thumFuture = Unit.LoadSpriteThumbnail();
    e = thumFuture.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = thumFuture.Result;
    this.unitThum.sprite2D = result;
    UI2DSprite unitThum1 = this.unitThum;
    Rect rect = result.rect;
    int width = (int) ((Rect) ref rect).width;
    ((UIWidget) unitThum1).width = width;
    UI2DSprite unitThum2 = this.unitThum;
    rect = result.rect;
    int height = (int) ((Rect) ref rect).height;
    ((UIWidget) unitThum2).height = height;
    if (unitData.PlayerUnit != (PlayerUnit) null)
    {
      PlayerUnit playerUnit = unitData.PlayerUnit;
      UnitSkillAwake[] awakeSkills = playerUnit.GetAwakeSkills();
      bool flag = awakeSkills != null && awakeSkills.Length != 0;
      this.dirExtraSkill.SetActive(flag);
      if (flag)
      {
        UnitSkillAwake awakeSkill = awakeSkills[0];
        spriteFuture = awakeSkill.skill.LoadBattleSkillIcon();
        e = spriteFuture.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.slcExtraSkill.sprite2D = spriteFuture.Result;
        if ((double) awakeSkill.need_affection <= (double) playerUnit.trust_rate)
          ((UIWidget) this.slcExtraSkill).color = Color.white;
        else
          ((UIWidget) this.slcExtraSkill).color = Color.gray;
        awakeSkill = (UnitSkillAwake) null;
        spriteFuture = (Future<Sprite>) null;
      }
      playerUnit = (PlayerUnit) null;
    }
    Singleton<ResourceManager>.GetInstance().ClearCache();
    Resources.UnloadUnusedAssets();
    if (playSerif)
    {
      while (this.nowMenuTween)
        yield return (object) null;
      this.ChangeSerif();
    }
  }

  private IEnumerator waitTweenPlaySerif()
  {
    yield return (object) null;
    while (this.nowMenuTween)
      yield return (object) null;
    this.ChangeSerif();
  }

  public void ChangeSerif()
  {
    if (this.serifIndex.Count <= 0)
      return;
    if (this.serifIndexCount >= this.serifIndex.Count)
    {
      this.serifIndex = this.serifIndex.Shuffle<int>().ToList<int>();
      this.serifIndexCount = 0;
    }
    SeaHomeSerif seaHomeSerif = MasterData.SeaHomeSerif[this.serifIndex[this.serifIndexCount++]];
    this.SetSerif(seaHomeSerif.serif, seaHomeSerif.face, seaHomeSerif.voice_cue_name);
    this.eyeBlink.StartBlink(2f);
  }

  public void TapSerif()
  {
    this.ChangeSerif();
    if (!this.isCall)
      return;
    this.unitAnimator.SetTrigger("call_touch");
  }

  public int SetSerif(string serif, string face, string voice_cue, Hashtable formtTables = null)
  {
    int num = -1;
    this.eyeBlink.StopBlink();
    if (formtTables == null || formtTables != null && !formtTables.ContainsKey((object) "userName"))
    {
      if (formtTables == null)
        formtTables = new Hashtable();
      formtTables.Add((object) "userName", (object) this.playerModified.Value.name);
    }
    this.textLabel.SetTextLocalize(Consts.Format(serif, (IDictionary) formtTables));
    if (!string.IsNullOrEmpty(face))
    {
      this.StartCoroutine(this.unitFace.ChangeFace(face));
      this.StartCoroutine(this.unitEye.ChangeEye("normal"));
    }
    if (!string.IsNullOrEmpty(voice_cue))
      num = Singleton<NGSoundManager>.GetInstance().playVoiceByStringID(this.current2DUnitData.Unit.unitVoicePattern, voice_cue);
    return num;
  }

  public void ChangeAppreciationMode()
  {
    if (this.mode == Sea030HomeMenu.HomeMode.Appreciation)
      return;
    this.ResetParticalObj();
    this.mode = Sea030HomeMenu.HomeMode.Appreciation;
    this.ActiveHome(true);
    SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
    this.seaTalkBtn2.SetActive(false);
    if (seaPlayer != null)
      this.seaTalkBtn.SetActive(seaPlayer.is_released_sea_call);
    this.manager.AllUnitShow();
    this.manager.SetCameraAutoFocus(true);
    this.tweeners = ((IEnumerable<UITweener>) this.tweeners).Where<UITweener>((Func<UITweener, bool>) (x => Object.op_Inequality((Object) x, (Object) null))).ToArray<UITweener>();
    ((IEnumerable<UITweener>) this.tweeners).ForEach<UITweener>((Action<UITweener>) (x =>
    {
      x.onFinished.Clear();
      ((Behaviour) x).enabled = false;
    }));
    if (NGTween.playTweens(this.tweeners, 3021))
    {
      this.nowMenuTween = true;
      UITweener uiTweener = ((IEnumerable<UITweener>) this.tweeners).Where<UITweener>((Func<UITweener, bool>) (x => ((Component) x).gameObject.activeInHierarchy && ((Behaviour) x).enabled)).OrderByDescending<UITweener, float>((Func<UITweener, float>) (x => x.delay + x.duration)).FirstOrDefault<UITweener>();
      if (Object.op_Inequality((Object) uiTweener, (Object) null))
        uiTweener.onFinished.Add(new EventDelegate(new EventDelegate.Callback(this.FinishTween)));
    }
    this.displayTouchEffectController.SetEnable(false);
  }

  public void OnSeaTalkBtn()
  {
    this.isChangeSelectSpot = true;
    if (this.coroutine != null)
      this.StopCoroutine(this.coroutine);
    SeaTalkPartnerScene.ChangeScene(this);
    if (!Object.op_Inequality((Object) this.noticeManager, (Object) null))
      return;
    Object.DestroyImmediate((Object) ((Component) this.noticeManager).gameObject);
  }

  private void PresentGive(GameCore.ItemInfo present, int selected)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.StartCoroutine(this.internalPresentGive(present, selected));
  }

  private IEnumerator internalPresentGive(GameCore.ItemInfo present, int selected)
  {
    Sea030HomeMenu sea030HomeMenu = this;
    CommonRoot cr = Singleton<CommonRoot>.GetInstance();
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    List<int> intList = new List<int>();
    for (int index = 0; index < selected; ++index)
      intList.Add(present.itemID);
    // ISSUE: reference to a compiler-generated method
    PlayerCallLetter unit = ((IEnumerable<PlayerCallLetter>) Singleton<NGGameDataManager>.GetInstance().callLetter).Where<PlayerCallLetter>(new Func<PlayerCallLetter, bool>(sea030HomeMenu.\u003CinternalPresentGive\u003Eb__138_0)).FirstOrDefault<PlayerCallLetter>();
    cr.loadingMode = 1;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.SeaPresentGive> future = WebAPI.SeaPresentGive(intList.ToArray(), sea030HomeMenu.current2DUnitData.PlayerUnit.id, new Action<WebAPI.Response.UserError>(sea030HomeMenu.\u003CinternalPresentGive\u003Eb__138_1));
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (future.Result == null)
    {
      cr.isTouchBlock = false;
      cr.isLoading = false;
      cr.loadingMode = 0;
    }
    else
    {
      cr.isTouchBlock = false;
      cr.isLoading = false;
      cr.loadingMode = 0;
      // ISSUE: reference to a compiler-generated method
      PlayerUnit playerUnit = ((IEnumerable<PlayerUnit>) future.Result.player_units).FirstOrDefault<PlayerUnit>(new Func<PlayerUnit, bool>(sea030HomeMenu.\u003CinternalPresentGive\u003Eb__138_2));
      float trustUp = 0.0f;
      if (playerUnit != (PlayerUnit) null)
      {
        trustUp = playerUnit.trust_rate - sea030HomeMenu.current2DUnitData.PlayerUnit.trust_rate;
        sea030HomeMenu.manager.UpdateUnitData(playerUnit);
        sea030HomeMenu.current2DUnitData.PlayerUnit = playerUnit;
        sea030HomeMenu.SetUnitData(sea030HomeMenu.current2DUnitData);
      }
      SeaPresentPresentAffinity presentPresentAffnity = SeaPresentPresentAffinity.GetSeaPresentPresentAffnity(sea030HomeMenu.current2DUnitData.Unit, present.gear.ID);
      SeaPresentPresentResult seaPresentResult = sea030HomeMenu.GetSeaPresentResult(presentPresentAffnity.affinity);
      if (seaPresentResult != null)
      {
        sea030HomeMenu.SetSerif(seaPresentResult.serif, seaPresentResult.face, seaPresentResult.voice_cue_name);
        sea030HomeMenu.PlayLoveResultEffect(seaPresentResult.affinity.home_result, trustUp, future.Result.gain_trust_result, playerUnit);
        sea030HomeMenu.eyeBlink.StartBlink(2f);
      }
      if (unit != null && future.Result.player_call_letters.Length != 0 && unit.mood_status == 3 && unit.mood_status > future.Result.player_call_letters[0].mood_status)
      {
        unit.mood_status = future.Result.player_call_letters[0].mood_status;
        Singleton<NGMessageUI>.GetInstance().SetMessageByPosType("姫の機嫌が回復し\n再びデートに誘えるようになりました");
        sea030HomeMenu.SetUnitData(sea030HomeMenu.current2DUnitData);
      }
      if (unit != null && future.Result.latest_talk_message != null)
        sea030HomeMenu.RefreshTalkMessage(future.Result.latest_talk_message);
      cr.isTouchBlock = false;
    }
  }

  private void DepartForDate(SeaDateDateSpotDisplaySetting setting)
  {
    if (this.isDepartDate)
      return;
    this.isDepartDate = true;
    this.IsPush = true;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    this.StartCoroutine(this.internalDepartForDate(setting));
  }

  private IEnumerator internalDepartForDate(SeaDateDateSpotDisplaySetting setting)
  {
    Sea030HomeMenu sea030HomeMenu1 = this;
    bool isSeaSeasonEnd = false;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim(true);
    CommonRoot cr = Singleton<CommonRoot>.GetInstance();
    cr.isTouchBlock = true;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    cr.loadingMode = 1;
    cr.isLoading = true;
    Future<WebAPI.Response.SeaDateStart> futureAPI = WebAPI.SeaDateStart(setting.datespot.ID, sea030HomeMenu1.current2DUnitData.PlayerUnit.id, (Action<WebAPI.Response.UserError>) (e =>
    {
      if (string.Equals(e.Code, "SEA000"))
      {
        isSeaSeasonEnd = true;
        this.StartCoroutine(PopupUtility.SeaError(e));
      }
      else
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }
    }));
    IEnumerator e1 = futureAPI.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (futureAPI.Result == null)
    {
      if (!isSeaSeasonEnd)
      {
        cr.isLoading = false;
        cr.loadingMode = 0;
        cr.isTouchBlock = false;
        sea030HomeMenu1.isDepartDate = false;
        sea030HomeMenu1.IsPush = false;
        Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
        cr.isLoading = true;
        e1 = new Future<NGGameDataManager.StartSceneProxyResult>(new Func<Promise<NGGameDataManager.StartSceneProxyResult>, IEnumerator>(Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxyImpl)).Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        cr.isLoading = false;
      }
    }
    else
    {
      sea030HomeMenu1.existSelect = futureAPI.Result.happening_id > 0 || futureAPI.Result.quiz_id > 0;
      cr.isLoading = false;
      NGSoundManager sm = Singleton<NGSoundManager>.GetInstance();
      int playVoiceIndex = -1;
      SeaDateSerifAtDepart serifDepart = sea030HomeMenu1.GetSerifDepart();
      if (serifDepart != null)
        playVoiceIndex = sea030HomeMenu1.SetSerif(serifDepart.serif, serifDepart.face, serifDepart.voice_cue_name, new Hashtable()
        {
          {
            (object) "dateName",
            (object) setting.date_name
          }
        });
      sm.PlaySe("SE_2702");
      yield return (object) new WaitForSeconds(1f);
      while (playVoiceIndex >= 0 && sm.IsVoicePlaying(playVoiceIndex))
        yield return (object) null;
      sea030HomeMenu1.dateSetting = setting;
      sea030HomeMenu1.dateFlows = ((IEnumerable<int>) futureAPI.Result.script_ids).ToList<int>();
      sea030HomeMenu1.nowFlowIndex = 0;
      sea030HomeMenu1.eventSelectData.Clear();
      sea030HomeMenu1.quizId = Sea030HomeMenu.hasQuiz(futureAPI.Result.date_flow) ? new int?(futureAPI.Result.quiz_id) : new int?();
      cr.loadingMode = 3;
      cr.isLoading = true;
      List<int> dateFlows = sea030HomeMenu1.dateFlows;
      Sea030HomeMenu sea030HomeMenu2 = sea030HomeMenu1;
      int nowFlowIndex = sea030HomeMenu1.nowFlowIndex;
      int num = nowFlowIndex + 1;
      sea030HomeMenu2.nowFlowIndex = num;
      int index = nowFlowIndex;
      Story0093Scene.changeSceneModeDate(true, dateFlows[index], sea030HomeMenu1.current2DUnitData.Unit, sea030HomeMenu1.dateSetting.GetImageHash(), new Action<int, int>(sea030HomeMenu1.EventSelected), isSea: new bool?(true), questionId: sea030HomeMenu1.quizId);
      cr.isTouchBlock = false;
      sea030HomeMenu1.isDepartDate = false;
      sea030HomeMenu1.IsPush = false;
      sea030HomeMenu1.SetSerif(string.Empty, "normal", string.Empty);
    }
  }

  public static bool hasQuiz(SeaDateDateFlow dateFlow)
  {
    return dateFlow == SeaDateDateFlow.happening_choices || dateFlow == SeaDateDateFlow.quiz_choices;
  }

  private void EventSelected(int index, int choice) => this.eventSelectData[index] = choice;

  private void ResetPlayLoveResultEffect()
  {
    this.resultEffectObject.SetActive(false);
    this.resultGaugeEffectObject.SetActive(false);
    ((Component) this.trustUpLabel).gameObject.SetActive(false);
  }

  public void ResetParticalObj()
  {
    this.ResetPlayLoveResultEffect();
    List<Transform> list = ((IEnumerable<Transform>) this.resultEffectObject.GetComponentsInChildren<Transform>(true)).ToList<Transform>();
    for (int index = 0; index < list.Count; ++index)
      ((Component) list[index]).gameObject.SetActive(false);
  }

  private void PlayLoveResultEffect(
    SeaHomeResult result,
    float trustUp,
    GainTrustResult gainTrustResult,
    PlayerUnit unit)
  {
    if (result == null)
      return;
    this.resultEffectObject.SetActive(result.effect_on);
    this.resultGaugeEffectObject.SetActive(result.gauge_effect_on);
    if (!string.IsNullOrEmpty(result.effect_trigger))
    {
      ((Behaviour) this.resultEffectAnimator).enabled = true;
      this.resultEffectAnimator.SetTrigger(result.effect_trigger);
      this.unitAnimator.SetTrigger(result.effect_trigger);
    }
    if (!string.IsNullOrEmpty(result.gauge_effect_trigger))
    {
      ((Behaviour) this.resultGaugeEffectAnimator).enabled = true;
      this.resultGaugeEffectAnimator.SetTrigger(result.gauge_effect_trigger);
    }
    if ((double) trustUp > 0.0)
    {
      ((Component) this.trustUpLabel).gameObject.SetActive(true);
      this.trustUpLabel.SetTextLocalize(string.Format("+{0:f2}{1}", (object) trustUp, (object) Consts.GetInstance().PERCENT));
      ((Behaviour) this.trustUpLabelAnimator).enabled = true;
      this.trustUpLabelAnimator.SetTrigger("dear_degree_up");
    }
    else
      ((Component) this.trustUpLabel).gameObject.SetActive(false);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Consts instance = Consts.GetInstance();
    bool isFirstTimeGaugeMax = (double) unit.trust_rate >= (double) instance.TRUST_RATE_LEVEL_SIZE && (double) unit.trust_rate - (double) trustUp < (double) instance.TRUST_RATE_LEVEL_SIZE;
    this.StartCoroutine(this.PlayGainTrustResult(gainTrustResult, unit, 2f, isFirstTimeGaugeMax));
  }

  private IEnumerator PlayGainTrustResult(
    GainTrustResult result,
    PlayerUnit unit,
    float wait,
    bool isFirstTimeGaugeMax = false)
  {
    FavorabilityRatingEffect.AnimationType type = FavorabilityRatingEffect.AnimationType.None;
    if (result.is_equip_awake_skill_release)
      type = FavorabilityRatingEffect.AnimationType.SkillFrameRelease;
    else if (result.has_new_player_awake_skill)
      type = FavorabilityRatingEffect.AnimationType.SkillRelease;
    if (type == FavorabilityRatingEffect.AnimationType.None)
    {
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    }
    else
    {
      yield return (object) new WaitForSeconds(wait);
      FavorabilityRatingEffect script = this.favorabilityRatingEffectPopupPrefab.CloneAndGetComponent<FavorabilityRatingEffect>(Singleton<CommonRoot>.GetInstance().LoadTmpObj);
      IEnumerator e = script.Init(type, unit, (Action) (() =>
      {
        if (result.is_equip_awake_skill_release && result.has_new_player_awake_skill)
        {
          result.is_equip_awake_skill_release = false;
          this.StartCoroutine(this.PlayGainTrustResult(result, unit, 0.0f));
        }
        else
          Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
        Singleton<PopupManager>.GetInstance().dismiss();
      }), !isFirstTimeGaugeMax);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(((Component) script).gameObject, isCloned: true);
      ((Component) script).GetComponent<FavorabilityRatingEffect>().StartEffect();
    }
  }

  public bool ExistDateFlows() => this.dateFlows != null;

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    if (this.mode == Sea030HomeMenu.HomeMode.Talk)
    {
      this.ChangeAppreciationMode();
      this.IsPush = false;
    }
    else if (Object.op_Inequality((Object) this.callPopupObject, (Object) null) && this.callPopupObject.IsShow)
    {
      this.IsPush = false;
    }
    else
    {
      if (this.mode != Sea030HomeMenu.HomeMode.Appreciation)
        return;
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized)
        return;
      if (Singleton<NGSceneManager>.GetInstance().sceneName == "mypage")
      {
        MypageScene sceneBase = Singleton<NGSceneManager>.GetInstance().sceneBase as MypageScene;
        if (Object.op_Inequality((Object) sceneBase, (Object) null) && sceneBase.isAnimePlaying)
          return;
      }
      Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
      MypageScene.ChangeScene();
    }
  }

  public void LoginPopupStart(
    List<PlayerLoginBonus> loginBonuses,
    int loginBonusCount,
    Action finish)
  {
    this.LoginBonusCloseCounter = 0;
    this.LoginPopupShow(loginBonuses, loginBonusCount, finish);
  }

  private void LoginPopupShow(
    List<PlayerLoginBonus> loginBonuses,
    int loginBonusCount,
    Action finish)
  {
    ModalWindow.Show(loginBonuses[this.LoginBonusCloseCounter].loginbonus.name, loginBonuses[this.LoginBonusCloseCounter].rewards[0].client_reward_message, (Action) (() =>
    {
      Singleton<NGGameDataManager>.GetInstance().loginBonuses.Remove(loginBonuses[this.LoginBonusCloseCounter]);
      ++this.LoginBonusCloseCounter;
      if (loginBonusCount <= this.LoginBonusCloseCounter)
      {
        List<LevelRewardSchemaMixin> playerLevelRewards = Singleton<NGGameDataManager>.GetInstance().playerLevelRewards;
        if (playerLevelRewards != null && playerLevelRewards.Count > 0)
          this.LevelUpPopupStart(playerLevelRewards, finish);
        else
          finish();
      }
      else
        this.LoginPopupShow(loginBonuses, loginBonusCount, finish);
    }));
  }

  public void LevelUpPopupStart(List<LevelRewardSchemaMixin> levelRewards, Action finish)
  {
    this.LevelUpBonusCloseCounter = 0;
    this.LevelUpBonusCount = levelRewards.Count;
    this.LevelUpPopupShow(levelRewards, finish);
  }

  private void LevelUpPopupShow(List<LevelRewardSchemaMixin> levelRewards, Action finish)
  {
    ModalWindow.Show(levelRewards[this.LevelUpBonusCloseCounter].reward_title, levelRewards[this.LevelUpBonusCloseCounter].reward_message, (Action) (() =>
    {
      ++this.LevelUpBonusCloseCounter;
      if (this.LevelUpBonusCount <= this.LevelUpBonusCloseCounter)
      {
        Singleton<NGGameDataManager>.GetInstance().playerLevelRewards = (List<LevelRewardSchemaMixin>) null;
        if (finish == null)
          return;
        finish();
      }
      else
        this.LevelUpPopupShow(levelRewards, finish);
    }));
  }

  public SeaDateSerifAtDepart GetSerifDepart()
  {
    return ((IEnumerable<SeaDateSerifAtDepart>) MasterData.SeaDateSerifAtDepartList).FirstOrDefault<SeaDateSerifAtDepart>((Func<SeaDateSerifAtDepart, bool>) (x => x.same_character_id_UnitUnit.HasValue && x.same_character_id_UnitUnit.Value == this.current2DUnitData.Unit.same_character_id && x.trust_provision.WithIn((int) this.current2DUnitData.PlayerUnit.trust_rate))) ?? ((IEnumerable<SeaDateSerifAtDepart>) MasterData.SeaDateSerifAtDepartList).FirstOrDefault<SeaDateSerifAtDepart>((Func<SeaDateSerifAtDepart, bool>) (x => x.character_id.HasValue && x.character_id.Value == this.current2DUnitData.Unit.character.ID && x.trust_provision.WithIn((int) this.current2DUnitData.PlayerUnit.trust_rate)));
  }

  private SeaDateResult GetSeaDateResult(float trust_up)
  {
    return ((IEnumerable<SeaDateResult>) MasterData.SeaDateResultList).FirstOrDefault<SeaDateResult>((Func<SeaDateResult, bool>) (x => x.same_character_id_UnitUnit.HasValue && x.same_character_id_UnitUnit.Value == this.current2DUnitData.Unit.same_character_id && x.result_staging.WithIn(trust_up))) ?? ((IEnumerable<SeaDateResult>) MasterData.SeaDateResultList).FirstOrDefault<SeaDateResult>((Func<SeaDateResult, bool>) (x => x.character_id.HasValue && x.character_id.Value == this.current2DUnitData.Unit.character.ID && x.result_staging.WithIn(trust_up)));
  }

  private SeaPresentPresentResult GetSeaPresentResult(SeaPresentAffinity affinity)
  {
    return ((IEnumerable<SeaPresentPresentResult>) MasterData.SeaPresentPresentResultList).FirstOrDefault<SeaPresentPresentResult>((Func<SeaPresentPresentResult, bool>) (x => x.same_character_id_UnitUnit.HasValue && x.same_character_id_UnitUnit.Value == this.current2DUnitData.Unit.same_character_id && affinity.ID == x.affinity.ID)) ?? ((IEnumerable<SeaPresentPresentResult>) MasterData.SeaPresentPresentResultList).FirstOrDefault<SeaPresentPresentResult>((Func<SeaPresentPresentResult, bool>) (x => x.character_id.HasValue && x.character_id.Value == this.current2DUnitData.Unit.character.ID && affinity.ID == x.affinity.ID));
  }

  public bool IsBadMood(SeaHomeManager.UnitConrtolleData unitData)
  {
    if (unitData == null || unitData.PlayerUnit == (PlayerUnit) null)
      return true;
    if (Singleton<NGGameDataManager>.GetInstance().callLetter == null)
      return false;
    PlayerCallLetter playerCallLetter = ((IEnumerable<PlayerCallLetter>) Singleton<NGGameDataManager>.GetInstance().callLetter).Where<PlayerCallLetter>((Func<PlayerCallLetter, bool>) (x => x.same_character_id == unitData.Unit.same_character_id)).FirstOrDefault<PlayerCallLetter>();
    return playerCallLetter != null && playerCallLetter.mood_status == 3;
  }

  public IEnumerator OpenSeaHomeTutorial()
  {
    Sea030HomeMenu sea030HomeMenu = this;
    SeaPlayer seaPlayer = SMManager.Get<SeaPlayer>();
    if (seaPlayer != null && seaPlayer.is_released_sea_call && (!Persist.seaTutorialData.Exists || Persist.seaTutorialData.Exists && !Persist.seaTutorialData.Data.seaHomeTutorial))
    {
      sea030HomeMenu.openningNoticePrefab = true;
      IEnumerator e;
      if (Object.op_Equality((Object) sea030HomeMenu.seaHomeTutorial, (Object) null))
      {
        Future<GameObject> ft = new ResourceObject("Prefabs/unit004_2/popup_sea_tutorial").Load<GameObject>();
        e = ft.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        sea030HomeMenu.seaHomeTutorial = ft.Result;
        ft = (Future<GameObject>) null;
      }
      // ISSUE: reference to a compiler-generated method
      GameObject popup = Singleton<PopupManager>.GetInstance().open(sea030HomeMenu.seaHomeTutorial, isNonSe: true, isNonOpenAnime: true, closeAnim: new Action(sea030HomeMenu.\u003COpenSeaHomeTutorial\u003Eb__157_0));
      e = popup.GetComponent<SimpleScrollContentsPopup>().Initialize((Action) (() =>
      {
        Persist.seaTutorialData.Data.seaHomeTutorial = true;
        Persist.seaTutorialData.Flush();
        Singleton<PopupManager>.GetInstance().dismiss();
      }));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().startOpenAnime(popup);
      popup = (GameObject) null;
    }
  }

  private IEnumerator OpenNoticePrefab()
  {
    Sea030HomeMenu homeMenu = this;
    Future<GameObject> futureseaMessageObj = Res.Prefabs.popup.popup_030_sea_mypage_dir_notice.Load<GameObject>();
    IEnumerator e = futureseaMessageObj.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    homeMenu.seaMessageObj = futureseaMessageObj.Result;
    GameObject gameObject = homeMenu.seaMessageObj.Clone(homeMenu.seaMessageRoot);
    homeMenu.noticeManager = gameObject.GetComponent<SeaNoticeManager>();
    homeMenu.noticeManager.Hide();
    yield return (object) new WaitForSeconds(1f);
    while (Singleton<PopupManager>.GetInstance().isOpen || Singleton<TutorialRoot>.GetInstance().IsAdviced || homeMenu.openningNoticePrefab)
      yield return (object) null;
    homeMenu.noticeManager.Init(homeMenu);
    homeMenu.noticeManager.Show();
  }

  private enum HomeMode
  {
    Appreciation,
    Talk,
  }
}
