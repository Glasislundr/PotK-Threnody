// Decompiled with JetBrains decompiler
// Type: Sea030_questScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Sea030Quest/Scene")]
public class Sea030_questScene : NGSceneBase
{
  private Sea030_questMenu menu;
  private UIScrollView scrollView;
  private static Dictionary<int, Vector3> scrollPositions = new Dictionary<int, Vector3>();
  private int? focusSid_;
  private bool isResetAnchor_ = true;

  public static void ChangeScene(bool isStack, bool scrollPosInitialize = true, bool forceInitialize = false)
  {
    if (scrollPosInitialize)
      Sea030_questScene.scrollPositions.Clear();
    if (SMManager.Get<PlayerSeaQuestS[]>() == null)
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    Singleton<NGGameDataManager>.GetInstance().IsSea = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_quest", (isStack ? 1 : 0) != 0, (object) forceInitialize);
  }

  public static void ChangeScene(bool isStack, int focusXL, int focusL, int focusM)
  {
    Sea030_questScene.scrollPositions.Clear();
    if (SMManager.Get<PlayerSeaQuestS[]>() == null)
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    Singleton<NGGameDataManager>.GetInstance().IsSea = true;
    Singleton<NGSceneManager>.GetInstance().changeScene("sea030_quest", (isStack ? 1 : 0) != 0, (object) focusXL, (object) focusL, (object) focusM);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Sea030_questScene sea030QuestScene = this;
    // ISSUE: reference to a compiler-generated method
    yield return (object) sea030QuestScene.\u003C\u003En__0();
    if (Object.op_Equality((Object) sea030QuestScene.menu, (Object) null))
    {
      Future<GameObject> ldMain = new ResourceObject("Prefabs/sea030_quest/dir_MapContainer").Load<GameObject>();
      yield return (object) ldMain.Wait();
      sea030QuestScene.isResetAnchor_ = true;
      sea030QuestScene.menu = ldMain.Result.Clone(((Component) sea030QuestScene).gameObject.transform).GetComponent<Sea030_questMenu>();
      sea030QuestScene.scrollView = (UIScrollView) sea030QuestScene.menu.scrollView;
      sea030QuestScene.menuBase = (NGMenuBase) sea030QuestScene.menu;
      ((Component) sea030QuestScene.menu).gameObject.SetActive(false);
      yield return (object) null;
      ldMain = (Future<GameObject>) null;
    }
  }

  public IEnumerator onStartSceneAsync(bool forceInitialize = false)
  {
    Sea030_questScene sea030QuestScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(4);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeaHomeMap seaHomeMap = ((IEnumerable<SeaHomeMap>) MasterData.SeaHomeMapList).ActiveSeaHomeMap(ServerTime.NowAppTimeAddDelta());
    if (seaHomeMap != null && !string.IsNullOrEmpty(seaHomeMap.bgm_cuesheet_name) && !string.IsNullOrEmpty(seaHomeMap.bgm_cue_name))
    {
      sea030QuestScene.bgmFile = seaHomeMap.bgm_cuesheet_name;
      sea030QuestScene.bgmName = seaHomeMap.bgm_cue_name;
    }
    e = new Future<NGGameDataManager.StartSceneProxyResult>(new Func<Promise<NGGameDataManager.StartSceneProxyResult>, IEnumerator>(Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxyImpl)).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (sea030QuestScene.isResetAnchor_)
    {
      ((Component) sea030QuestScene.menu).gameObject.SetActive(true);
      yield return (object) null;
      sea030QuestScene.isResetAnchor_ = false;
    }
    PlayerSeaQuestS[] StoryData = SMManager.Get<PlayerSeaQuestS[]>();
    sea030QuestScene.tweens = (UITweener[]) null;
    e = sea030QuestScene.menu.Init(StoryData, forceInitialize, sea030QuestScene.focusSid_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int currentSelectMapId = sea030QuestScene.menu.GetCurrentSelectMapID();
    if (Sea030_questScene.scrollPositions.ContainsKey(currentSelectMapId))
      sea030QuestScene.scrollView.mLocalPosition = Sea030_questScene.scrollPositions[currentSelectMapId];
    sea030QuestScene.focusSid_ = new int?();
    Sea030_questScene.scrollPositions.Clear();
    yield return (object) new WaitForSeconds(0.5f);
  }

  public IEnumerator onStartSceneAsync(int focusXL, int focusL, int focusM)
  {
    this.focusSid_ = Array.Find<QuestSeaS>(MasterData.QuestSeaSList, (Predicate<QuestSeaS>) (x => x.quest_xl_QuestSeaXL == focusXL && x.quest_l_QuestSeaL == focusL && x.quest_m_QuestSeaM == focusM))?.ID;
    yield return (object) this.onStartSceneAsync();
  }

  public void onStartScene(int focusXL, int focusL, int focusM) => this.onStartScene();

  public void onStartScene(bool forceInitialize = false)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public override IEnumerator onEndSceneAsync()
  {
    Sea030_questScene sea030QuestScene = this;
    int currentSelectMapId = sea030QuestScene.menu.GetCurrentSelectMapID();
    if (Sea030_questScene.scrollPositions.ContainsKey(currentSelectMapId))
      Sea030_questScene.scrollPositions[currentSelectMapId] = sea030QuestScene.scrollView.mLocalPosition;
    else
      Sea030_questScene.scrollPositions.Add(currentSelectMapId, sea030QuestScene.scrollView.mLocalPosition);
    float startTime = Time.time;
    while (!sea030QuestScene.isTweenFinished && (double) Time.time - (double) startTime < (double) sea030QuestScene.tweenTimeoutTime)
      yield return (object) null;
    ((Component) sea030QuestScene).gameObject.GetComponent<UIRect>().alpha = 0.0f;
    if (Object.op_Inequality((Object) sea030QuestScene.menu, (Object) null))
      sea030QuestScene.menu.onEndScene();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    sea030QuestScene.isTweenFinished = true;
    yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) sea030QuestScene.\u003C\u003En__1();
  }

  public IEnumerator onBackSceneAsync(int focusXL, int focusL, int focusM)
  {
    IEnumerator e = this.onStartSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onBackScene(int focusXL, int focusL, int focusM) => this.onStartScene();

  public IEnumerator onBackSceneAsync(bool forceInitialize)
  {
    IEnumerator e = this.onStartSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onBackScene(bool forceInitialize) => this.onStartScene();
}
