// Decompiled with JetBrains decompiler
// Type: PopupMypageStorySelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using MiniJSON;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class PopupMypageStorySelect : BackButtonPopupWindow
{
  private const int EarthQuestPologueMaxNumber = 8;
  public Animator[] buttonAnimators;
  private string ANIM_STORY_SELECT_OUT = "StorySelect_Out";
  [SerializeField]
  private UIButton btnLostRagnarokStory;
  [SerializeField]
  private UIButton btnHeavenStory;
  private bool isPush;
  private bool isDisabledTouchWhilePlayingAnime;
  private Action<int, bool> onSceneChange;
  private Action onClosePopup;
  private MonoBehaviour coroutineBehaviour;
  private Animator animator;
  private int L_num;
  private bool isHardMode;
  private bool closeOnly;

  private bool isPushAndSet()
  {
    if (this.isPush)
      return true;
    this.isPush = true;
    return false;
  }

  private void StartOutAnim(bool closeOnly = false)
  {
    this.animator.Play(this.ANIM_STORY_SELECT_OUT);
    for (int index = 0; index < this.buttonAnimators.Length; ++index)
      this.buttonAnimators[index].SetBool("isOut", true);
    this.closeOnly = closeOnly;
  }

  public IEnumerator Initialize(
    Action<int, bool> onSceneChange,
    Action onClosePopup,
    MonoBehaviour behaviour = null)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    PopupMypageStorySelect mypageStorySelect = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    mypageStorySelect.isPush = true;
    if (Object.op_Inequality((Object) behaviour, (Object) null))
      mypageStorySelect.coroutineBehaviour = behaviour;
    mypageStorySelect.onClosePopup = onClosePopup;
    ((Behaviour) mypageStorySelect.btnLostRagnarokStory).enabled = true;
    ((Behaviour) mypageStorySelect.btnHeavenStory).enabled = true;
    mypageStorySelect.onSceneChange = onSceneChange;
    mypageStorySelect.animator = ((Component) mypageStorySelect).GetComponent<Animator>();
    if (((Component) mypageStorySelect).gameObject.activeInHierarchy)
    {
      mypageStorySelect.isDisabledTouchWhilePlayingAnime = false;
      // ISSUE: reference to a compiler-generated method
      mypageStorySelect.StartCoroutine(mypageStorySelect.waitEndOpenAnimation(new Action(mypageStorySelect.\u003CInitialize\u003Eb__16_0)));
    }
    else
      mypageStorySelect.isDisabledTouchWhilePlayingAnime = true;
    return false;
  }

  public void OnEverAfter()
  {
    if (this.isPushAndSet())
      return;
    PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
    int key = -1;
    bool flag = false;
    QuestStoryS questStoryS;
    if (Persist.everAfterProcess.Exists && Persist.everAfterProcess.Data.lastEverAfterSId != 0 && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (q => q._quest_story_s == Persist.everAfterProcess.Data.lastEverAfterSId)) && MasterData.QuestStoryS.TryGetValue(Persist.everAfterProcess.Data.lastEverAfterSId, out questStoryS) && questStoryS.quest_xl_QuestStoryXL == 7)
    {
      key = questStoryS.quest_l_QuestStoryL;
      QuestStoryL questStoryL;
      if (MasterData.QuestStoryL.TryGetValue(key, out questStoryL) && questStoryL.origin_id.HasValue)
      {
        flag = true;
        key = questStoryL.origin_id.Value;
      }
    }
    if (key < 0 && source != null)
    {
      PlayerStoryQuestS[] array = ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_xl_QuestStoryXL == 7)).ToArray<PlayerStoryQuestS>();
      if (array.Length != 0)
      {
        key = ((IEnumerable<PlayerStoryQuestS>) array).Max<PlayerStoryQuestS>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL));
        QuestStoryL questStoryL;
        if (MasterData.QuestStoryL.TryGetValue(key, out questStoryL) && questStoryL.origin_id.HasValue)
        {
          flag = true;
          key = questStoryL.origin_id.Value;
        }
      }
      else
      {
        this.isPush = false;
        return;
      }
    }
    this.L_num = key;
    this.isHardMode = flag;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1058");
    this.StartOutAnim();
    if (this.onSceneChange == null)
      return;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
  }

  public void OnIntegralNoah()
  {
    if (this.isPushAndSet())
      return;
    PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
    int key = -1;
    bool flag = false;
    QuestStoryS questStoryS;
    if (Persist.integralNoahProcess.Exists && Persist.integralNoahProcess.Data.lastIntegralNoahSId != 0 && ((IEnumerable<PlayerStoryQuestS>) source).Any<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (q => q._quest_story_s == Persist.integralNoahProcess.Data.lastIntegralNoahSId)) && MasterData.QuestStoryS.TryGetValue(Persist.integralNoahProcess.Data.lastIntegralNoahSId, out questStoryS) && questStoryS.quest_xl_QuestStoryXL == 6)
    {
      key = questStoryS.quest_l_QuestStoryL;
      QuestStoryL questStoryL;
      if (MasterData.QuestStoryL.TryGetValue(key, out questStoryL) && questStoryL.origin_id.HasValue)
      {
        flag = true;
        key = questStoryL.origin_id.Value;
      }
    }
    if (key < 0 && source != null)
    {
      key = ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_xl_QuestStoryXL == 6)).Max<PlayerStoryQuestS>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL));
      QuestStoryL questStoryL;
      if (MasterData.QuestStoryL.TryGetValue(key, out questStoryL) && questStoryL.origin_id.HasValue)
      {
        flag = true;
        key = questStoryL.origin_id.Value;
      }
    }
    this.L_num = key;
    this.isHardMode = flag;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1058");
    this.StartOutAnim();
    if (this.onSceneChange == null)
      return;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
  }

  public void onLostRagnarokStory()
  {
    if (this.isPushAndSet())
      return;
    int key = -1;
    bool flag = false;
    LastPlayPlayerStoryQuestSIds playerStoryQuestSids = SMManager.Get<LastPlayPlayerStoryQuestSIds>();
    if (playerStoryQuestSids != null && playerStoryQuestSids.lost_ragnarok_quest_s_id.HasValue)
    {
      QuestStoryS questStoryS;
      if (MasterData.QuestStoryS.TryGetValue(playerStoryQuestSids.lost_ragnarok_quest_s_id.Value, out questStoryS))
      {
        key = questStoryS.quest_l_QuestStoryL;
        QuestStoryL questStoryL;
        if (MasterData.QuestStoryL.TryGetValue(key, out questStoryL) && questStoryL.origin_id.HasValue)
        {
          flag = true;
          key = questStoryL.origin_id.Value;
        }
      }
    }
    else if (key < 0)
    {
      PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
      if (source != null)
      {
        key = ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_xl_QuestStoryXL == 4)).Max<PlayerStoryQuestS>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL));
        QuestStoryL questStoryL;
        if (MasterData.QuestStoryL.TryGetValue(key, out questStoryL) && questStoryL.origin_id.HasValue)
        {
          flag = true;
          key = questStoryL.origin_id.Value;
        }
      }
    }
    this.L_num = key;
    this.isHardMode = flag;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1058");
    this.StartOutAnim();
    if (this.onSceneChange == null)
      return;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
  }

  public void onHeavenStory()
  {
    if (this.isPushAndSet())
      return;
    int key = -1;
    bool flag = false;
    LastPlayPlayerStoryQuestSIds playerStoryQuestSids = SMManager.Get<LastPlayPlayerStoryQuestSIds>();
    if (playerStoryQuestSids != null && playerStoryQuestSids.heaven_quest_s_id.HasValue)
    {
      QuestStoryS questStoryS;
      if (MasterData.QuestStoryS.TryGetValue(playerStoryQuestSids.heaven_quest_s_id.Value, out questStoryS))
      {
        key = questStoryS.quest_l_QuestStoryL;
        QuestStoryL questStoryL;
        if (MasterData.QuestStoryL.TryGetValue(key, out questStoryL) && questStoryL.origin_id.HasValue)
        {
          flag = true;
          key = questStoryL.origin_id.Value;
        }
      }
    }
    else if (key < 0)
    {
      PlayerStoryQuestS[] source = SMManager.Get<PlayerStoryQuestS[]>();
      if (source != null)
      {
        key = ((IEnumerable<PlayerStoryQuestS>) source).Where<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_xl_QuestStoryXL == 1)).Max<PlayerStoryQuestS>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_l_QuestStoryL));
        QuestStoryL questStoryL;
        if (MasterData.QuestStoryL.TryGetValue(key, out questStoryL) && questStoryL.origin_id.HasValue)
        {
          flag = true;
          key = questStoryL.origin_id.Value;
        }
      }
    }
    this.L_num = key;
    this.isHardMode = flag;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1058");
    this.StartOutAnim();
    if (this.onSceneChange == null)
      return;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
  }

  private IEnumerator ZeroApi()
  {
    Dictionary<string, object> serverJson = (Dictionary<string, object>) null;
    Future<WebAPI.Response.ZeroLoad> rq = WebAPI.ZeroLoad((Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<PopupManager>.GetInstance().closeAll();
    }));
    IEnumerator e = rq.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (rq.Result.has_data)
      serverJson = Json.Deserialize(rq.Result.player_data) as Dictionary<string, object>;
    bool flag = true;
    if (!Persist.oldEarthData.Exists && !Persist.earthData.Exists)
      flag = false;
    Dictionary<string, object> dictionary = (Dictionary<string, object>) null;
    if (serverJson != null)
      dictionary = (Dictionary<string, object>) serverJson["quest_progress"];
    if (flag)
      this.EnterEarthQuest(rq);
    else if (dictionary != null && (int) (long) dictionary["prolouge_index"] <= 8)
      this.ShowPopUp(rq);
    else if (!flag)
      this.ShowPopUp(rq);
    else
      this.EnterEarthQuest(rq);
  }

  private void ShowPopUp(Future<WebAPI.Response.ZeroLoad> rq)
  {
    this.isDisabledTouchWhilePlayingAnime = true;
    PopupCommonNoYes.Show(Consts.GetInstance().Bugu005RecipeCompositeMenu_Title, Consts.GetInstance().EARTH_QUEST_POPUP, (Action) (() =>
    {
      this.isDisabledTouchWhilePlayingAnime = false;
      Singleton<PopupManager>.GetInstance().dismiss();
      this.EnterEarthQuest(rq);
    }), (Action) (() => { }));
  }

  private void OnEnable()
  {
    if (!this.isDisabledTouchWhilePlayingAnime)
      return;
    this.isDisabledTouchWhilePlayingAnime = false;
    this.isPush = true;
    this.StartCoroutine(this.waitEndOpenAnimation((Action) (() => this.isPush = false)));
  }

  private void EnterEarthQuest(Future<WebAPI.Response.ZeroLoad> rq)
  {
    if (!Singleton<NGSceneManager>.GetInstance().isSceneInitialized)
      return;
    MasterDataCache.SetGameMode(MasterDataCache.GameMode.EARTH);
    this.isPush = true;
    this.StartOutAnim(true);
    NGSceneManager instance1 = Singleton<NGSceneManager>.GetInstance();
    if (instance1.sceneName == "mypage")
    {
      instance1.destroyCurrentScene();
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(2, true);
    }
    else
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    NGGameDataManager instance2 = Singleton<NGGameDataManager>.GetInstance();
    instance2.lastReferenceUnitID = -1;
    instance2.lastReferenceUnitIndex = -1;
    NGSceneBase sceneBase1 = instance1.sceneBase;
    if (Object.op_Inequality((Object) sceneBase1, (Object) null))
      sceneBase1.IsPush = true;
    instance1.destroyLoadedScenes();
    MypageScene sceneBase2 = Singleton<NGSceneManager>.GetInstance().sceneBase as MypageScene;
    EarthDataManager.SetZeroLoad(rq);
    if (Object.op_Implicit((Object) sceneBase2))
    {
      sceneBase2.onStartEarthCloud();
    }
    else
    {
      NGSoundManager instance3 = Singleton<NGSoundManager>.GetInstance();
      if (Object.op_Inequality((Object) instance3, (Object) null))
        instance3.playSE("SE_0055");
      EarthDataManager.startEarthScene(this.coroutineBehaviour);
    }
    if (this.onSceneChange == null)
      return;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
  }

  public void onButtonOpenGroundMypage()
  {
    if (this.isPushAndSet())
      return;
    this.StartCoroutine(this.ZeroApi());
  }

  public void ChangeScene()
  {
    this.StartCoroutine(this.procDismissPopup());
    if (this.closeOnly || this.onSceneChange == null)
      return;
    this.onSceneChange(this.L_num, this.isHardMode);
  }

  private IEnumerator procDismissPopup()
  {
    PopupMypageStorySelect mypageStorySelect = this;
    int outName = Animator.StringToHash(mypageStorySelect.ANIM_STORY_SELECT_OUT);
    while (true)
    {
      AnimatorStateInfo animatorStateInfo = mypageStorySelect.animator.GetCurrentAnimatorStateInfo(0);
      if (!((AnimatorStateInfo) ref animatorStateInfo).shortNameHash.Equals(outName) || (double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0)
        yield return (object) null;
      else
        break;
    }
    int outName2 = Animator.StringToHash("StoryBtn_Out");
    while (true)
    {
      AnimatorStateInfo animatorStateInfo = mypageStorySelect.buttonAnimators[0].GetCurrentAnimatorStateInfo(0);
      if (!((AnimatorStateInfo) ref animatorStateInfo).shortNameHash.Equals(outName2) || (double) ((AnimatorStateInfo) ref animatorStateInfo).normalizedTime < 1.0)
        yield return (object) null;
      else
        break;
    }
    foreach (Behaviour componentsInChild in ((Component) mypageStorySelect).GetComponentsInChildren<SpriteTransitionController>())
      componentsInChild.enabled = false;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  private IEnumerator waitEndOpenAnimation(Action callbackEnd)
  {
    AnimatorStateInfo animatorStateInfo1;
    do
    {
      yield return (object) null;
      animatorStateInfo1 = this.animator.GetCurrentAnimatorStateInfo(0);
    }
    while ((double) ((AnimatorStateInfo) ref animatorStateInfo1).normalizedTime < 1.0);
    Animator[] animators = ((IEnumerable<Animator>) this.buttonAnimators).Where<Animator>((Func<Animator, bool>) (x => Object.op_Inequality((Object) x, (Object) null) && ((Component) x).gameObject.activeInHierarchy)).ToArray<Animator>();
    while (animators.Length != 0)
    {
      yield return (object) null;
      List<Animator> dels = new List<Animator>(animators.Length);
      foreach (Animator animator in animators)
      {
        if (((Component) animator).gameObject.activeInHierarchy)
        {
          AnimatorStateInfo animatorStateInfo2 = animator.GetCurrentAnimatorStateInfo(0);
          if ((double) ((AnimatorStateInfo) ref animatorStateInfo2).normalizedTime < 1.0)
            continue;
        }
        dels.Add(animator);
      }
      if (dels.Any<Animator>())
        animators = ((IEnumerable<Animator>) animators).Where<Animator>((Func<Animator, bool>) (x => !dels.Contains(x))).ToArray<Animator>();
    }
    callbackEnd();
  }

  public override void onBackButton()
  {
    if (this.isPushAndSet())
      return;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1057");
    this.StartOutAnim(true);
  }

  private void OnDestroy()
  {
    if (this.onClosePopup == null)
      return;
    this.onClosePopup();
  }
}
