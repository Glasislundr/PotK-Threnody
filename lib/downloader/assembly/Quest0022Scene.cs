// Decompiled with JetBrains decompiler
// Type: Quest0022Scene
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
public class Quest0022Scene : NGSceneBase
{
  private static bool isInit;
  public Quest0022Menu menu;
  public BGChange bgchange;
  private static QuestStoryClearMessage clearmessage;
  private static BattleEndPlayer_review reviewSet;
  private const int DEFAULT_SEA_XL = 3;

  public static void ChangeSceneSea(bool stack, int XL, int L, int M)
  {
    Quest0022Scene.isInit = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_2_sea", (stack ? 1 : 0) != 0, (object) XL, (object) L, (object) M, (object) -1);
  }

  public static void ChangeScene0022(bool stack, int L, int M)
  {
    Quest0022Scene.isInit = true;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<NGSceneManager>.GetInstance().changeScene("quest002_2_sea", (stack ? 1 : 0) != 0, (object) L, (object) M);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("quest002_2", (stack ? 1 : 0) != 0, (object) L, (object) M);
  }

  public static void ChangeScene0022(bool stack, int L, int M, int S)
  {
    Quest0022Scene.isInit = true;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      Singleton<NGSceneManager>.GetInstance().changeScene("quest002_2_sea", (stack ? 1 : 0) != 0, (object) L, (object) M, (object) S);
    else
      Singleton<NGSceneManager>.GetInstance().changeScene("quest002_2", (stack ? 1 : 0) != 0, (object) L, (object) M, (object) S);
  }

  public static void ChangeScene0022(
    bool stack,
    int L,
    int M,
    int recent_s_id,
    int clearCount,
    BattleEndPlayer_review review)
  {
    Quest0022Scene.clearmessage = !Singleton<NGGameDataManager>.GetInstance().IsSea ? ((IEnumerable<QuestStoryClearMessage>) MasterData.QuestStoryClearMessageList).Where<QuestStoryClearMessage>((Func<QuestStoryClearMessage, bool>) (x => x.quest_s_id == recent_s_id)).Where<QuestStoryClearMessage>((Func<QuestStoryClearMessage, bool>) (x => !x.is_firsttime && clearCount > 0 || clearCount == 1)).FirstOrDefault<QuestStoryClearMessage>() : Quest0022Scene.convert(((IEnumerable<QuestSeaClearMessage>) MasterData.QuestSeaClearMessageList).Where<QuestSeaClearMessage>((Func<QuestSeaClearMessage, bool>) (x => x.quest_s_id == recent_s_id)).Where<QuestSeaClearMessage>((Func<QuestSeaClearMessage, bool>) (x => !x.is_firsttime && clearCount > 0 || clearCount == 1)).FirstOrDefault<QuestSeaClearMessage>());
    Quest0022Scene.reviewSet = (BattleEndPlayer_review) null;
    Quest0022Scene.ChangeScene0022(stack, L, M);
  }

  public static void ChangeSceneSea(
    bool stack,
    int XL,
    int L,
    int M,
    int recent_s_id,
    int clearCount,
    BattleEndPlayer_review review)
  {
    Quest0022Scene.clearmessage = Quest0022Scene.convert(Array.Find<QuestSeaClearMessage>(MasterData.QuestSeaClearMessageList, (Predicate<QuestSeaClearMessage>) (x =>
    {
      if (x.quest_s_id != recent_s_id)
        return false;
      return !x.is_firsttime && clearCount > 0 || clearCount == 1;
    })));
    Quest0022Scene.reviewSet = (BattleEndPlayer_review) null;
    Quest0022Scene.ChangeSceneSea(stack, XL, L, M);
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(1, 1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int XL, int L, int M, int S)
  {
    yield return (object) this.onStartSceneAsync_sea(XL, L, M, S);
  }

  public IEnumerator onStartSceneAsync(int L, int M)
  {
    IEnumerator e;
    if (Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      e = this.onStartSceneAsync_sea(0, L, M, -1);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.onStartSceneAsync(L, M, -1);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator onStartSceneAsync(int L, int M, int S)
  {
    Quest0022Scene quest0022Scene = this;
    if (Quest0022Scene.isInit)
    {
      Quest0022Scene.isInit = false;
      quest0022Scene.menu.tweenSettingDefault();
    }
    PlayerStoryQuestS[] StoryData = SMManager.Get<PlayerStoryQuestS[]>();
    int XL = 1;
    PlayerStoryQuestS playerStoryQuestS = ((IEnumerable<PlayerStoryQuestS>) StoryData).FirstOrDefault<PlayerStoryQuestS>((Func<PlayerStoryQuestS, bool>) (x => x.quest_story_s.quest_l_QuestStoryL == L));
    if (playerStoryQuestS != null)
      XL = playerStoryQuestS.quest_story_s.quest_xl_QuestStoryXL;
    PlayerStoryQuestS[] playerStoryQuestSArray = StoryData.S(XL, L, M);
    bool hardmode = playerStoryQuestSArray[0].quest_story_s.quest_l.origin_id.HasValue;
    quest0022Scene.bgchange.getCurrentBG();
    bool bgCreate = Object.op_Equality((Object) quest0022Scene.bgchange.Current, (Object) null);
    IEnumerator e;
    if (bgCreate)
    {
      if (hardmode)
      {
        int L1 = playerStoryQuestSArray[0].quest_story_s.quest_l.origin_id.Value;
        int num = ((IEnumerable<PlayerStoryQuestS>) StoryData.M(XL, L)).Select<PlayerStoryQuestS, int>((Func<PlayerStoryQuestS, int>) (x => x.quest_story_s.quest_m.priority)).First<int>();
        int M1 = playerStoryQuestSArray[0].quest_story_s.quest_m.priority - num + 1;
        e = quest0022Scene.bgchange.QuestBGprefabCreate(L1, M1, hardmode);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = quest0022Scene.bgchange.QuestBGprefabCreate(L, M, hardmode);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    quest0022Scene.menu.startAllpostween = new List<TweenPosition>();
    quest0022Scene.menu.startAllalphatween = new List<TweenAlpha>();
    ((IEnumerable<TweenPosition>) ((Component) quest0022Scene).GetComponentsInChildren<TweenPosition>()).ForEach<TweenPosition>((Action<TweenPosition>) (x => this.menu.startAllpostween.Add(x)));
    ((IEnumerable<TweenAlpha>) ((Component) quest0022Scene).GetComponentsInChildren<TweenAlpha>()).ForEach<TweenAlpha>((Action<TweenAlpha>) (x => this.menu.startAllalphatween.Add(x)));
    if (quest0022Scene.bgchange.NotTween)
    {
      quest0022Scene.menu.startAllpostween.ForEach((Action<TweenPosition>) (x =>
      {
        if (((UITweener) x).tweenGroup != -11 && ((UITweener) x).tweenGroup != -12)
          return;
        ((Component) x).gameObject.transform.localPosition = x.to;
      }));
      quest0022Scene.menu.startAllalphatween.ForEach((Action<TweenAlpha>) (x =>
      {
        if (((UITweener) x).tweenGroup != -11 && ((UITweener) x).tweenGroup != -12)
          return;
        ((UIRect) ((Component) x).GetComponent<UIWidget>()).alpha = x.to;
      }));
      quest0022Scene.bgchange.NotTween = false;
    }
    e = quest0022Scene.menu.Initialize(StoryData, XL, L, M, S, hardmode, bgCreate, S != -1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene()
  {
    this.menu.SceneStart = true;
    foreach (TweenPosition tweenPosition in this.menu.startAllpostween)
      ((UITweener) tweenPosition).tweenGroup = Math.Abs(((UITweener) tweenPosition).tweenGroup);
    foreach (TweenAlpha tweenAlpha in this.menu.startAllalphatween)
      ((UITweener) tweenAlpha).tweenGroup = Math.Abs(((UITweener) tweenAlpha).tweenGroup);
  }

  public void onStartScene(int XL, int L, int M, int S) => this.onStartScene(L, M);

  public void onStartScene(int L, int M)
  {
    if (Quest0022Scene.clearmessage != null)
      this.StartCoroutine(this.ClearPopup());
    else if (Quest0022Scene.reviewSet != null && Quest0022Scene.reviewSet.id != "")
      this.StartCoroutine(this.ReviewPopup());
    this.menu.SceneStart = true;
    foreach (TweenPosition tweenPosition in this.menu.startAllpostween)
      ((UITweener) tweenPosition).tweenGroup = Math.Abs(((UITweener) tweenPosition).tweenGroup);
    foreach (TweenAlpha tweenAlpha in this.menu.startAllalphatween)
      ((UITweener) tweenAlpha).tweenGroup = Math.Abs(((UITweener) tweenAlpha).tweenGroup);
    this.menu.HscrollButtonsAction();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void onStartScene(int L, int M, int S) => this.onStartScene(L, M);

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  private IEnumerator ClearPopup()
  {
    Quest0022Scene quest0022Scene = this;
    Future<GameObject> PopupF = Res.Prefabs.popup.popup_002_24__anim_popup.Load<GameObject>();
    IEnumerator e = PopupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(PopupF.Result);
    EventDelegate del = (EventDelegate) null;
    if (Quest0022Scene.reviewSet != null && Quest0022Scene.reviewSet.id != "")
    {
      // ISSUE: reference to a compiler-generated method
      del = new EventDelegate(new EventDelegate.Callback(quest0022Scene.\u003CClearPopup\u003Eb__19_0));
    }
    gameObject.GetComponent<ClearMessagePopup>().Init(Quest0022Scene.clearmessage.title, Quest0022Scene.clearmessage.message, del);
    Quest0022Scene.clearmessage = (QuestStoryClearMessage) null;
  }

  private IEnumerator ReviewPopup()
  {
    Future<GameObject> PopupF = Res.Prefabs.popup.popup_002_25__anim_popup.Load<GameObject>();
    IEnumerator e = PopupF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(PopupF.Result).GetComponent<ReviewMessagePopup>().Init(Quest0022Scene.reviewSet.title, Quest0022Scene.reviewSet.message, Quest0022Scene.reviewSet.id);
    Quest0022Scene.reviewSet = (BattleEndPlayer_review) null;
  }

  public override void onEndScene()
  {
    foreach (GameObject hscrollButton in this.menu.hscrollButtons)
      hscrollButton.GetComponent<Quest0022Hscroll>().centerAnimation(false);
    this.menu.indicator.SeEnable = false;
    this.menu.nowCenterObj = (GameObject) null;
    this.menu.SceneStart = false;
    this.menu.ButtonMove = false;
    this.menu.onEndScene();
  }

  public IEnumerator onStartSceneAsync_sea(int XL, int L, int M, int S)
  {
    Quest0022Scene quest0022Scene = this;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      quest0022Scene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      quest0022Scene.bgmName = seaHomeMap.bgm_cue_name;
    }
    if (Quest0022Scene.isInit)
    {
      Quest0022Scene.isInit = false;
      quest0022Scene.menu.tweenSettingDefault();
    }
    IEnumerator e = quest0022Scene.SetSeaBackground(L);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    quest0022Scene.menu.startAllpostween = new List<TweenPosition>();
    quest0022Scene.menu.startAllalphatween = new List<TweenAlpha>();
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<TweenPosition>) ((Component) quest0022Scene).GetComponentsInChildren<TweenPosition>()).ForEach<TweenPosition>(new Action<TweenPosition>(quest0022Scene.\u003ConStartSceneAsync_sea\u003Eb__23_0));
    // ISSUE: reference to a compiler-generated method
    ((IEnumerable<TweenAlpha>) ((Component) quest0022Scene).GetComponentsInChildren<TweenAlpha>()).ForEach<TweenAlpha>(new Action<TweenAlpha>(quest0022Scene.\u003ConStartSceneAsync_sea\u003Eb__23_1));
    if (quest0022Scene.bgchange.NotTween)
    {
      quest0022Scene.menu.startAllpostween.ForEach((Action<TweenPosition>) (x =>
      {
        if (((UITweener) x).tweenGroup != -11 && ((UITweener) x).tweenGroup != -12)
          return;
        ((Component) x).gameObject.transform.localPosition = x.to;
      }));
      quest0022Scene.menu.startAllalphatween.ForEach((Action<TweenAlpha>) (x =>
      {
        if (((UITweener) x).tweenGroup != -11 && ((UITweener) x).tweenGroup != -12)
          return;
        ((UIRect) ((Component) x).GetComponent<UIWidget>()).alpha = x.to;
      }));
      quest0022Scene.bgchange.NotTween = false;
    }
    PlayerSeaQuestS[] array = ((IEnumerable<PlayerSeaQuestS>) SMManager.Get<PlayerSeaQuestS[]>()).Where<PlayerSeaQuestS>((Func<PlayerSeaQuestS, bool>) (x => x.quest_sea_s != null)).ToArray<PlayerSeaQuestS>();
    e = quest0022Scene.menu.Initialize(array, XL, L, M, S, true, S != -1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetSeaBackground(int L)
  {
    Quest0022Scene quest0022Scene = this;
    Future<GameObject> bgF = new ResourceObject(string.Format("Prefabs/Quest/Story/BG/L_sea/{0}/prefab", (object) L)).Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      quest0022Scene.backgroundPrefab = bgF.Result;
  }

  private static QuestStoryClearMessage convert(QuestSeaClearMessage message)
  {
    if (message == null)
      return (QuestStoryClearMessage) null;
    return new QuestStoryClearMessage()
    {
      ID = message.ID,
      is_firsttime = message.is_firsttime,
      quest_s_id = message.quest_s_id,
      title = message.title,
      message = message.message
    };
  }
}
