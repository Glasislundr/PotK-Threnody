// Decompiled with JetBrains decompiler
// Type: Quest002SideStoryScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/quest002_17/SideStoryScene")]
public class Quest002SideStoryScene : NGSceneBase
{
  public static readonly string DefaultSceneName = "quest002_SideStory_List";
  public Quest002SideStoryMenu menu;
  private bool IsInit;
  private bool offAlphaByDisable_;

  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Quest002SideStoryScene.DefaultSceneName, stack);
  }

  public static void ChangeScene(bool stack, int activeTabIndex)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Quest002SideStoryScene.DefaultSceneName, (stack ? 1 : 0) != 0, (object) activeTabIndex);
  }

  public static void backOrChangeScene(int activeTabIndex)
  {
    if (Singleton<NGSceneManager>.GetInstance().backScene(Quest002SideStoryScene.DefaultSceneName))
      return;
    Quest002SideStoryScene.ChangeScene(false, activeTabIndex);
  }

  public IEnumerator onStartSceneAsync()
  {
    yield return (object) this.onStartSceneAsync(8);
  }

  public IEnumerator onStartSceneAsync(int activeTabIndex)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    yield return (object) null;
    ((Component) this.menu).GetComponent<UIRect>().alpha = 1f;
    IEnumerator e;
    if (!this.IsInit)
    {
      this.IsInit = true;
      e = this.ProgressExtra(activeTabIndex);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.menu.UpdateTime();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void onStartScene()
  {
    Persist.eventStoryPlay.Data.SetReserveList(Singleton<NGGameDataManager>.GetInstance().playbackEventIds, this.sceneName);
    if (Persist.eventStoryPlay.Data.PlayEventScript(this.sceneName, 0))
      return;
    this.StartCoroutine(this.waitDisplayMenu());
  }

  public void onStartScene(int activeTabIndex) => this.onStartScene();

  private IEnumerator ProgressExtra(int activeTabIndex)
  {
    Future<WebAPI.Response.QuestProgressExtra> extra = WebAPI.QuestProgressExtra((Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = extra.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (extra.Result != null)
    {
      WebAPI.SetLatestResponsedAt("QuestProgressExtra");
      PlayerExtraQuestS[] ExtraData = SMManager.Get<PlayerExtraQuestS[]>();
      QuestExtraTimetable questExtraTimetable = SMManager.Get<QuestExtraTimetable>();
      e = this.menu.Init(ExtraData, extra.Result.sort_categories, questExtraTimetable.emphasis, questExtraTimetable.notice, extra.Result.player_created_at, activeTabIndex);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator waitDisplayMenu()
  {
    yield return (object) new WaitForSeconds(0.1f);
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public override void onEndScene() => this.offAlphaByDisable_ = true;

  private void OnDisable()
  {
    if (this.offAlphaByDisable_)
      ((Component) this.menu).GetComponent<UIRect>().alpha = 0.0f;
    this.offAlphaByDisable_ = false;
  }
}
