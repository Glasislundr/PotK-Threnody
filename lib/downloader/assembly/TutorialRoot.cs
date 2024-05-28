// Decompiled with JetBrains decompiler
// Type: TutorialRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniLinq;
using UnityEngine;

#nullable disable
public class TutorialRoot : Singleton<TutorialRoot>
{
  [NonSerialized]
  public const int BeginnerBannerID = 887;
  [NonSerialized]
  public const int TutorialTicketID = 675;
  [SerializeField]
  private Transform prefabRoot;
  [SerializeField]
  private TutorialProgress progress;
  [SerializeField]
  private TutorialAdvice advice;
  [SerializeField]
  private UIPanel mainPanel;
  [SerializeField]
  private UICamera uiCamera;
  [SerializeField]
  private UIRoot uiRoot;
  [SerializeField]
  private GameObject tutorialPagesGameObject;
  private bool isInitilizing;
  private bool isNowFinish;
  private bool player_is_create;
  private const string tutorial_finish_scene = "gacha";
  private int[] tutorial_gacha_unit_ids;
  private int[] tutorial_gacha_unit_types;
  private int[] tutorial_gacha_direction_types;
  private WebAPI.Response.TutorialTutorialRagnarokResume resume;
  public bool isTutorialBattleFirstDead;
  public bool isTutorialBattleFirstSpawn;
  public bool isTutorialBattleSpawnDead;

  public bool IsInitilizing => this.isInitilizing;

  public int[] Tutorial_gacha_unit_ids
  {
    set => this.tutorial_gacha_unit_ids = value;
    get => this.tutorial_gacha_unit_ids;
  }

  public int[] Tutorial_gacha_unit_types
  {
    set => this.tutorial_gacha_unit_types = value;
    get => this.tutorial_gacha_unit_types;
  }

  public int[] Tutorial_gacha_direction_types
  {
    set => this.tutorial_gacha_direction_types = value;
    get => this.tutorial_gacha_direction_types;
  }

  public WebAPI.Response.TutorialTutorialRagnarokResume Resume => this.resume;

  private GameObject wrap => ((Component) this.mainPanel).gameObject;

  public bool IsAdviced => this.advice.IsShow;

  public bool checkAdviceCompleted() => !this.IsAdviced;

  protected override void Initialize()
  {
    this.isNowFinish = false;
    this.isInitilizing = true;
    Object.Destroy((Object) ((Component) this.uiCamera).gameObject);
    UIRoot component = ((Component) Singleton<CommonRoot>.GetInstance()).GetComponent<UIRoot>();
    this.uiRoot.manualHeight = component.manualHeight;
    this.uiRoot.minimumHeight = component.minimumHeight;
    this.StartCoroutine(this.initAsync());
  }

  private IEnumerator initAsync()
  {
    TutorialRoot tutorialRoot = this;
    ((UIRect) tutorialRoot.mainPanel).alpha = 0.0f;
    yield return (object) Singleton<ResourceManager>.GetInstance().LoadResource(((Component) tutorialRoot).gameObject);
    ((UIRect) tutorialRoot.mainPanel).alpha = 1f;
    tutorialRoot.advice.Init();
    TutorialPageBase[] array = ((IEnumerable<TutorialPageBase>) tutorialRoot.tutorialPagesGameObject.GetComponentsInChildren<TutorialPageBase>(true)).OrderBy<TutorialPageBase, string>((Func<TutorialPageBase, string>) (x => ((Object) x).name)).ToArray<TutorialPageBase>();
    tutorialRoot.progress = new TutorialProgress(array, tutorialRoot.advice, tutorialRoot.wrap);
    // ISSUE: reference to a compiler-generated method
    tutorialRoot.progress.OnNextPageCallback = new Action(tutorialRoot.\u003CinitAsync\u003Eb__39_1);
    tutorialRoot.progress.OnFinishCallback = new Action(tutorialRoot.finish);
    tutorialRoot.wrap.SetActive(false);
    tutorialRoot.isInitilizing = false;
  }

  public void DisableTutorialFinishGUI() => this.IsTutorialFinish();

  public void DebugSetResume(
    WebAPI.Response.TutorialTutorialRagnarokResume resume)
  {
    this.resume = resume;
  }

  public void StartTutorial(
    WebAPI.Response.TutorialTutorialRagnarokResume resume)
  {
    Persist.battleEnvironment.Data = (BE) null;
    Persist.battleEnvironment.Delete();
    this.resume = resume;
    if (!Persist.tutorial.Exists)
      Persist.tutorial.Flush();
    this.progress.CurrentPageIndex = Persist.tutorial.Data.CurrentPage;
    if (this.progress.IsFinish())
    {
      Debug.LogError((object) "call Renbder() but tutorial is finished. so restart tutorial");
      this.progress.CurrentPageIndex = 0;
      Persist.tutorial.Data.SetPageIndex(0);
    }
    this.StartCoroutine(this.startTutorial());
  }

  private IEnumerator startTutorial()
  {
    this.wrap.SetActive(false);
    NGSceneManager sceneManager = Singleton<NGSceneManager>.GetInstance();
    sceneManager.clearStack();
    sceneManager.changeScene("empty", true);
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    while (!sceneManager.isSceneInitialized)
      yield return (object) null;
    while (this.isInitilizing)
      yield return (object) null;
    IEnumerator e = this.progress.Render();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.wrap.SetActive(true);
  }

  private void finish()
  {
    if (this.isNowFinish)
      return;
    Persist.battleEnvironment.Data = (BE) null;
    Persist.battleEnvironment.Delete();
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    this.wrap.SetActive(false);
    this.wrap.SetActive(false);
    this.StartCoroutine(this.signUpLoop(true, (Action) (() =>
    {
      this.endTutorial();
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
      Debug.Log((object) "tutorial end, go to next page");
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
      Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy((Action<NGGameDataManager.StartSceneProxyResult>) (_ =>
      {
        Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
        Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
        Singleton<NGGameDataManager>.GetInstance().IsSea = false;
        MypageScene.ChangeScene();
      }));
      this.isNowFinish = false;
    })));
    EventTracker.SendTutorialFinish();
    this.isNowFinish = true;
  }

  public void TutorialGachaAdvice()
  {
    if (Persist.newTutorialGacha.Data.tutorialGacha)
    {
      Singleton<TutorialRoot>.GetInstance().CurrentAdvise();
    }
    else
    {
      this.setTutorialGachaModule();
      Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_3", false);
      Singleton<NGSceneManager>.GetInstance().waitSceneAction((Action) (() =>
      {
        Singleton<NGSceneManager>.GetInstance().clearStack();
        Singleton<TutorialRoot>.GetInstance().ForceShowAdviceInNextButton("newchapter_gacha1_tutorial", new Dictionary<string, Func<Transform, UIButton>>()
        {
          {
            "chapter_gacha_ticket",
            (Func<Transform, UIButton>) (root =>
            {
              Transform childInFind = root.GetChildInFind("Middle");
              UIButton componentInChildren = ((Component) childInFind).GetComponentInChildren<UIButton>();
              GameObject gameObject = GameObject.Find("Gacha0063Gacha0063Scene UI Root").GetComponent<Gacha0063Scene>().GetTutorialGachaTop().Clone(childInFind.GetChildInFind("dir_gacha_ticket_position"));
              gameObject.SetActive(false);
              gameObject.SetActive(true);
              return componentInChildren;
            })
          }
        }, (Action) (() => this.StartCoroutine(Singleton<TutorialRoot>.GetInstance().TutorialGacha(true))));
      }));
    }
  }

  private void setTutorialGachaModule()
  {
    DateTime nowTime = ServerTime.NowAppTimeAddDelta();
    GachaTutorialPeriod gachaPeriod = ((IEnumerable<GachaTutorialPeriod>) MasterData.GachaTutorialPeriodList).Where<GachaTutorialPeriod>((Func<GachaTutorialPeriod, bool>) (x =>
    {
      if (x.start_at.HasValue)
      {
        DateTime dateTime1 = nowTime;
        DateTime? nullable = x.start_at;
        if ((nullable.HasValue ? (dateTime1 >= nullable.GetValueOrDefault() ? 1 : 0) : 0) != 0 && x.end_at.HasValue)
        {
          DateTime dateTime2 = nowTime;
          nullable = x.end_at;
          return nullable.HasValue && dateTime2 < nullable.GetValueOrDefault();
        }
      }
      return false;
    })).FirstOrDefault<GachaTutorialPeriod>();
    if (gachaPeriod == null)
      return;
    GachaTutorial gachaTutorial = ((IEnumerable<GachaTutorial>) MasterData.GachaTutorialList).Where<GachaTutorial>((Func<GachaTutorial, bool>) (x =>
    {
      int? gachaTutorialPeriod = x._period_id_GachaTutorialPeriod;
      int id = gachaPeriod.ID;
      return gachaTutorialPeriod.GetValueOrDefault() == id & gachaTutorialPeriod.HasValue;
    })).FirstOrDefault<GachaTutorial>();
    if (gachaTutorial == null)
      return;
    GachaModule gachaModule = new GachaModule()
    {
      description = new GachaDescription(),
      newentity = new GachaModuleNewentity[0],
      decks = new GachaModuleDecks[0],
      number = 1,
      stepup = new GachaStepup(),
      period = new GachaPeriod()
    };
    gachaModule.period.start_at = new DateTime?(gachaPeriod.start_at.HasValue ? gachaPeriod.start_at.Value : new DateTime(2010, 1, 1));
    gachaModule.period.end_at = new DateTime?(gachaPeriod.end_at.HasValue ? gachaPeriod.end_at.Value : new DateTime(2040, 1, 1));
    gachaModule.period.display_count_down = false;
    gachaModule.front_image_url = MasterData.GachaTutorialBannerList[0].front_image_url;
    gachaModule.gacha = new GachaModuleGacha[1]
    {
      new GachaModuleGacha()
    };
    gachaModule.gacha[0].count = 0;
    gachaModule.gacha[0].payment_id = gachaTutorial.payment_id;
    gachaModule.gacha[0].deck_id = gachaTutorial._deck_id;
    gachaModule.gacha[0].start_at = gachaModule.period.start_at;
    gachaModule.gacha[0].description = gachaTutorial.description;
    gachaModule.gacha[0].roll_count = 0;
    gachaModule.gacha[0].max_roll_count = gachaTutorial.max_roll_count;
    gachaModule.gacha[0].end_at = gachaModule.period.end_at;
    gachaModule.gacha[0].daily_count = 0;
    gachaModule.gacha[0].payment_amount = gachaTutorial.payment_amount;
    gachaModule.gacha[0].limit = gachaTutorial._limit;
    gachaModule.gacha[0].details = gachaModule.description;
    gachaModule.gacha[0].remain_count_for_reward = new int?();
    gachaModule.gacha[0].daily_limit = gachaTutorial._daily_limit;
    gachaModule.gacha[0].payment_type_id = gachaTutorial.payment_type_id_CommonPayType;
    gachaModule.gacha[0].button_url = (string) null;
    gachaModule.gacha[0].id = gachaTutorial.ID;
    gachaModule.gacha[0].max_roll_count = gachaTutorial.max_roll_count;
    gachaModule.gacha[0].name = gachaTutorial.name;
    gachaModule.type = 4;
    gachaModule.title_banner_url = MasterData.GachaTutorialBannerList[0].title_image_url;
    gachaModule.name = gachaTutorial.name;
    SMManager.UpdateList<GachaModule>(new GachaModule[1]
    {
      gachaModule
    });
  }

  public IEnumerator TutorialGacha(bool isStack)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0, true);
    yield return (object) new WaitForSeconds(0.1f);
    GachaModule[] gachaModuleArray = SMManager.Get<GachaModule[]>();
    IEnumerator e = GachaPlay.GetInstance().TutorialGacha("g030_tutorial", gachaModuleArray[0].gacha[0].id, (MasterDataTable.GachaType) 30, 1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGSceneManager>.GetInstance().changeScene("gacha006_effect", isStack);
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
  }

  public void EncryptAndSaveGachaData(
    WebAPI.Response.GachaG001ChargeMultiPay result_list)
  {
    if (!Persist.newTutorialGacha.Exists)
    {
      Persist.newTutorialGacha.Data.setDefault();
      Persist.newTutorialGacha.Flush();
    }
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    List<int> intList3 = new List<int>();
    foreach (PlayerUnit playerUnit in result_list.player_units)
    {
      intList1.Add(playerUnit.unit.ID);
      intList2.Add((int) playerUnit.unit_type.Enum);
    }
    foreach (WebAPI.Response.GachaG001ChargeMultiPayResult chargeMultiPayResult in result_list.result)
      intList3.Add(chargeMultiPayResult.direction_type_id);
    this.Tutorial_gacha_unit_ids = intList1.ToArray();
    this.Tutorial_gacha_unit_types = intList2.ToArray();
    this.Tutorial_gacha_direction_types = intList3.ToArray();
    string empty1 = string.Empty;
    for (int index = 0; index < this.tutorial_gacha_unit_ids.Length; ++index)
    {
      empty1 += (string) (object) this.tutorial_gacha_unit_ids[index];
      if (index < this.tutorial_gacha_unit_ids.Length - 1)
        empty1 += ",";
    }
    Persist.newTutorialGacha.Data.strUnitIDs = Convert.ToBase64String(Crypt.Encrypt(Encoding.UTF8.GetBytes(empty1)));
    string empty2 = string.Empty;
    for (int index = 0; index < this.Tutorial_gacha_unit_types.Length; ++index)
    {
      empty2 += (string) (object) this.Tutorial_gacha_unit_types[index];
      if (index < this.Tutorial_gacha_unit_types.Length - 1)
        empty2 += ",";
    }
    Persist.newTutorialGacha.Data.strUnitTypes = Convert.ToBase64String(Crypt.Encrypt(Encoding.UTF8.GetBytes(empty2)));
    string empty3 = string.Empty;
    for (int index = 0; index < this.Tutorial_gacha_direction_types.Length; ++index)
    {
      empty3 += (string) (object) this.Tutorial_gacha_direction_types[index];
      if (index < this.Tutorial_gacha_direction_types.Length - 1)
        empty3 += ",";
    }
    Persist.newTutorialGacha.Data.strDirectionTypes = Convert.ToBase64String(Crypt.Encrypt(Encoding.UTF8.GetBytes(empty3)));
    Persist.newTutorialGacha.Flush();
  }

  public bool DecryptGachaData()
  {
    if (string.IsNullOrEmpty(Persist.newTutorialGacha.Data.strUnitIDs) || string.IsNullOrEmpty(Persist.newTutorialGacha.Data.strUnitTypes) || string.IsNullOrEmpty(Persist.newTutorialGacha.Data.strDirectionTypes))
      return false;
    string[] strArray = Encoding.UTF8.GetString(Crypt.Decrypt(Convert.FromBase64String(Persist.newTutorialGacha.Data.strUnitIDs))).Split(',');
    List<int> intList = new List<int>();
    for (int index = 0; index < strArray.Length; ++index)
    {
      int result = 0;
      int.TryParse(strArray[index], out result);
      intList.Add(result);
      this.tutorial_gacha_unit_ids = intList.ToArray();
    }
    intList.Clear();
    string str1 = Encoding.UTF8.GetString(Crypt.Decrypt(Convert.FromBase64String(Persist.newTutorialGacha.Data.strUnitTypes)));
    char[] chArray1 = new char[1]{ ',' };
    foreach (string s in str1.Split(chArray1))
    {
      int result = 0;
      int.TryParse(s, out result);
      intList.Add(result);
      this.tutorial_gacha_unit_types = intList.ToArray();
    }
    intList.Clear();
    string str2 = Encoding.UTF8.GetString(Crypt.Decrypt(Convert.FromBase64String(Persist.newTutorialGacha.Data.strDirectionTypes)));
    char[] chArray2 = new char[1]{ ',' };
    foreach (string s in str2.Split(chArray2))
    {
      int result = 0;
      int.TryParse(s, out result);
      intList.Add(result);
      this.tutorial_gacha_direction_types = intList.ToArray();
    }
    return true;
  }

  public void EndTutorial()
  {
    this.progress.CurrentPageIndex = Persist.tutorial.Data.LastPageIndex;
    Persist.newTutorialGacha.Data.tutorialGacha = true;
    Persist.newTutorialGacha.Flush();
    Persist.EndTutorial();
  }

  private void endTutorial()
  {
    this.progress.CurrentPageIndex = Persist.tutorial.Data.LastPageIndex;
    Persist.tutorial.Data.SetTutorialFinish();
    Persist.tutorial.Flush();
    Persist.integralNoaTutorial.Data.SetTutorialFinish();
    Persist.integralNoaTutorial.Flush();
    Persist.newTutorialGacha.Data.clearGachaResult();
    Persist.newTutorialGacha.Flush();
  }

  public IEnumerator signUpLoop(bool isFinish, Action callback)
  {
    if (Persist.tutorial.Data.signupCalled && (WebAPI.LastPlayerBoot.player_is_create || this.player_is_create))
    {
      if (isFinish)
        yield return (object) this.debugTutorialGacha();
      callback();
    }
    else
    {
      Persist.Tutorial data = Persist.tutorial.Data;
      string name = string.IsNullOrEmpty(data.PlayerName) ? Consts.GetInstance().DEFAULT_PLAYER_NAME : data.PlayerName;
      Future<WebAPI.Response.PlayerSignup> future = WebAPI.PlayerSignup(data.MiniGameScore, name, 0);
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (future.HasResult && future.Result != null)
      {
        this.player_is_create = true;
        Persist.tutorial.Data.signupCalled = true;
        Persist.tutorial.Flush();
        if (isFinish)
          yield return (object) this.debugTutorialGacha();
        callback();
      }
      else
      {
        Consts instance = Consts.GetInstance();
        PopupCommonOkTitle.Show(instance.tutorial_fail_signup_title, instance.tutorial_fail_signup_text, (Action) (() => this.StartCoroutine(this.signUpLoop(isFinish, callback))), (Action) (() =>
        {
          Persist.tutorial.Data.PlayerName = "";
          Persist.tutorial.Data.CurrentPage = 0;
          Persist.tutorial.Flush();
          StartScript.Restart();
        }));
      }
    }
  }

  public IEnumerator debugTutorialGacha()
  {
    if (!Persist.newTutorialGacha.Data.tutorialGacha)
    {
      this.setTutorialGachaModule();
      GachaModule[] source = SMManager.Get<GachaModule[]>();
      if (source != null && ((IEnumerable<GachaModule>) source).Any<GachaModule>())
      {
        IEnumerator e = GachaPlay.GetInstance().ChargeGacha("g030_tutorial", 10, source[0].gacha[0].id, (MasterDataTable.GachaType) 30, 1);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      Persist.newTutorialGacha.Data.tutorialGacha = true;
    }
  }

  public bool IsTutorialFinish() => Persist.tutorial.Data.IsFinishTutorial();

  public void OnChangeSceneFinish(string sceneName)
  {
    string tipsMessage = this.getTipsMessage(sceneName);
    if (string.IsNullOrEmpty(tipsMessage))
      return;
    this.showAdvice(tipsMessage, sceneName);
  }

  public void OnBattleStateChange(BL env)
  {
    if (this.IsTutorialFinish())
    {
      this.FirstAnnihilated(env);
    }
    else
    {
      if (env.phaseState.state == BL.Phase.finalize)
      {
        Singleton<NGBattleManager>.GetInstance().isBattleEnable = false;
        Singleton<NGBattleManager>.GetInstance().popupOpen((GameObject) null);
      }
      this.StartCoroutine(this.onBattleStateChange(env.phaseState.state, env.phaseState.turnCount));
    }
  }

  public bool FirstAnnihilated(BL env)
  {
    if (env.phaseState.state == BL.Phase.finalize && !env.battleInfo.pvp && !env.battleInfo.gvg && !env.battleInfo.isEarthMode && env.battleInfo.quest_type != CommonQuestType.GuildRaid && env.battleInfo.isFirstAllDead && env.allDeadUnitsp(BL.ForceID.player))
    {
      Singleton<NGBattleManager>.GetInstance().isBattleEnable = false;
      if (!this.ShowAdvice("firstgameover", finishCallback: (Action) (() => Singleton<NGBattleManager>.GetInstance().isBattleEnable = true)))
      {
        Singleton<NGBattleManager>.GetInstance().isBattleEnable = true;
        Debug.LogError((object) "ERROR FirstAnnihilated ShowAdvice");
      }
    }
    return false;
  }

  public void ReleaseResources() => this.progress.ReleaseResources();

  public void CurrentAdvise()
  {
    TutorialPageBase tutorialPageBase = this.progress.currentOrNull();
    if (!Object.op_Inequality((Object) tutorialPageBase, (Object) null))
      return;
    tutorialPageBase.Advise();
  }

  public IEnumerator onBattleStateChange(BL.Phase state, int turn)
  {
    while (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized)
      yield return (object) null;
    TutorialBattlePage battle = this.progress.BattlePage();
    if (Object.op_Equality((Object) battle, (Object) null))
    {
      Debug.LogError((object) ("call OnBattleStateChange but not CurrentPageIndex=" + (object) this.progress.CurrentPageIndex));
    }
    else
    {
      switch (state)
      {
        case BL.Phase.player_start:
          yield return (object) new WaitForSeconds(1.5f);
          battle.OnPlayerTurnStart(turn);
          break;
        case BL.Phase.finalize:
          battle.OnBattleFinish();
          Time.timeScale = 1f;
          while (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized)
            yield return (object) null;
          Singleton<NGBattleManager>.GetInstance().popupCloseAll();
          break;
      }
    }
  }

  private string getTipsMessage(string tipsName, int id = 0)
  {
    string str;
    return Consts.GetInstance().tutorial.TryGetValue(tipsName, out str) && !Persist.tutorial.Data.Hints.ContainsKey(tipsName) ? str : string.Empty;
  }

  private string getTipsMessageForce(string tipsName, int id = 0)
  {
    string str;
    return Consts.GetInstance().tutorial.TryGetValue(tipsName, out str) ? str : string.Empty;
  }

  public bool isReadHint(string sceneName, int id)
  {
    return string.IsNullOrEmpty(this.getTipsMessage(sceneName, id));
  }

  private void doneReadHint(string sceneName)
  {
    if (!Consts.GetInstance().tutorial.ContainsKey(sceneName))
      return;
    Persist.tutorial.Data.Hints[sceneName] = true;
    Persist.tutorial.Flush();
  }

  public bool ShowAdvice(string sceneName = null, int id = 0, Action finishCallback = null)
  {
    string tipsMessage = this.getTipsMessage(sceneName, id);
    if (string.IsNullOrEmpty(tipsMessage))
      return false;
    this.showAdvice(tipsMessage, sceneName, finishCallback: finishCallback);
    return true;
  }

  public bool ForceShowAdvice(string sceneName = null, Action finishCallback = null)
  {
    return this.ForceShowAdviceInNextButton(sceneName, finishCallback: finishCallback);
  }

  public bool ForceShowAdviceInNextButton(
    string sceneName = null,
    Dictionary<string, Func<Transform, UIButton>> next_button_info = null,
    Action finishCallback = null)
  {
    string tipsMessageForce = this.getTipsMessageForce(sceneName);
    if (string.IsNullOrEmpty(tipsMessageForce))
      return false;
    this.showAdvice(tipsMessageForce, sceneName, next_button_info, finishCallback);
    return true;
  }

  public void showAdvice(
    string message,
    string sceneName = null,
    Dictionary<string, Func<Transform, UIButton>> next_button_info = null,
    Action finishCallback = null)
  {
    this.wrap.SetActive(true);
    this.advice.SetMessage(message, next_button_info);
    this.advice.FinishCallback = (Action) (() =>
    {
      if (sceneName != null)
      {
        this.doneReadHint(sceneName);
        Consts instance = Consts.GetInstance();
        if (sceneName.Equals("gacha"))
          PopupCommonYesNo.Show(instance.tutorial_finish_bulk_download_title, instance.tutorial_finish_bulk_download_text, (Action) (() => this.StartCoroutine(this.bulkDownLoadCheck())), (Action) (() => { }));
      }
      this.wrap.SetActive(false);
      if (finishCallback == null)
        return;
      finishCallback();
    });
  }

  private void doneEventQuestExplanation(int sceneID)
  {
    Persist.explanation.Data.Explanation[sceneID] = true;
    Persist.explanation.Flush();
  }

  public void showEventQuestExplanation(string message, int sceneID = -1)
  {
    this.wrap.SetActive(true);
    this.advice.SetMessage(message);
    this.advice.FinishCallback = (Action) (() =>
    {
      if (sceneID != -1)
        this.doneEventQuestExplanation(sceneID);
      this.wrap.SetActive(false);
    });
  }

  public void DebugTutorialStart()
  {
    Persist.tutorial.Data.SetPageIndex(0);
    Persist.tutorial.Delete();
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxy((Action<NGGameDataManager.StartSceneProxyResult>) (_ => this.StartTutorial((WebAPI.Response.TutorialTutorialRagnarokResume) null)));
  }

  public void DebugTutorialFinish()
  {
    this.wrap.SetActive(false);
    this.finish();
  }

  public void DebugTutorialAdvice(string message) => this.showAdvice(message);

  private IEnumerator bulkDownLoadCheck()
  {
    TutorialRoot tutorialRoot = this;
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.isTouchBlock = true;
    yield return (object) new WaitForSeconds(0.5f);
    common.isTouchBlock = false;
    common.isLoading = true;
    yield return (object) new WaitForSeconds(0.5f);
    long requiredSize = OnDemandDownload.SizeOfLoadAllUnits();
    common.isLoading = false;
    Consts instance = Consts.GetInstance();
    if (requiredSize > 0L)
    {
      // ISSUE: reference to a compiler-generated method
      PopupCommonYesNo.Show(instance.bulk_download_title, instance.bulkDownloadText(requiredSize), new Action(tutorialRoot.\u003CbulkDownLoadCheck\u003Eb__74_0), (Action) (() => { }));
    }
    else
      tutorialRoot.StartCoroutine(PopupCommon.Show(instance.bulk_download_title, instance.bulk_downloaded_text, (Action) (() => { })));
  }

  private IEnumerator doBulkDownload()
  {
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.isTouchBlock = true;
    yield return (object) new WaitForSeconds(0.5f);
    common.isTouchBlock = false;
    common.isLoading = true;
    yield return (object) new WaitForSeconds(0.5f);
    Debug.Log((object) "start bulk download");
    IEnumerator e = OnDemandDownload.WaitLoadAllUnits(false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    common.isLoading = false;
    MypageScene.ChangeScene();
  }

  public void resumeTutorialForGachaPage()
  {
    Persist.tutorial.Data.SetPageIndex(this.progress.GetTutoarialGachaPage());
    Persist.tutorial.Flush();
    Persist.integralNoaTutorial.Data.beginnersQuest = false;
    Persist.integralNoaTutorial.Flush();
  }
}
