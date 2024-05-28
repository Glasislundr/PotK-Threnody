// Decompiled with JetBrains decompiler
// Type: Quest00282Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00282Scene : NGSceneBase
{
  public Quest00282Menu menu;
  public NGxScroll scroll;
  public bool isInit;

  public static void changeScene(
    bool stack,
    PlayerStoryQuestS story_quest,
    Action<PlayerHelper> eventSetHelper = null)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8_2", (stack ? 1 : 0) != 0, (object) story_quest, (object) eventSetHelper);
  }

  public static void changeScene(
    bool stack,
    PlayerQuestSConverter quest,
    Action<PlayerHelper> eventSetHelper = null)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8_2", (stack ? 1 : 0) != 0, (object) quest, (object) eventSetHelper);
  }

  public static void changeScene(
    bool stack,
    PlayerExtraQuestS extra_quest,
    Action<PlayerHelper> eventSetHelper = null)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8_2", (stack ? 1 : 0) != 0, (object) extra_quest, (object) eventSetHelper);
  }

  public static void changeScene(
    bool stack,
    PlayerCharacterQuestS char_quest,
    Action<PlayerHelper> eventSetHelper = null)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8_2", (stack ? 1 : 0) != 0, (object) char_quest, (object) eventSetHelper);
  }

  public static void changeScene(
    bool stack,
    PlayerSeaQuestS sea_quest,
    Action<PlayerHelper> eventSetHelper = null)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_8_2_sea", (stack ? 1 : 0) != 0, (object) sea_quest, (object) eventSetHelper);
  }

  public IEnumerator onStartSceneAsync(
    PlayerStoryQuestS story_quest,
    Action<PlayerHelper> eventSetHelper)
  {
    IEnumerator e = this.onStartSceneAsync(story_quest, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, eventSetHelper);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerQuestSConverter quest,
    Action<PlayerHelper> eventSetHelper)
  {
    IEnumerator e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, quest, (PlayerSeaQuestS) null, eventSetHelper);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerExtraQuestS extra_quest,
    Action<PlayerHelper> eventSetHelper)
  {
    IEnumerator e = this.onStartSceneAsync((PlayerStoryQuestS) null, extra_quest, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, eventSetHelper);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerCharacterQuestS char_quest,
    Action<PlayerHelper> eventSetHelper)
  {
    IEnumerator e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, char_quest, (PlayerQuestSConverter) null, (PlayerSeaQuestS) null, eventSetHelper);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerSeaQuestS sea_quest,
    Action<PlayerHelper> eventSetHelper)
  {
    IEnumerator e = this.onStartSceneAsync((PlayerStoryQuestS) null, (PlayerExtraQuestS) null, (PlayerCharacterQuestS) null, (PlayerQuestSConverter) null, sea_quest, eventSetHelper);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(
    PlayerStoryQuestS story_quest,
    PlayerExtraQuestS extra_quest,
    PlayerCharacterQuestS char_quest,
    PlayerQuestSConverter quest,
    PlayerSeaQuestS sea_quest,
    Action<PlayerHelper> eventSetHelper)
  {
    if (!this.isInit)
    {
      this.menu.setEventSetHelper(eventSetHelper);
      IEnumerator e = this.menu.InitPlayerDecks(story_quest, extra_quest, char_quest, quest, sea_quest);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Singleton<NGGameDataManager>.GetInstance().IsSea)
      {
        e = this.SetSeaBackground();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      this.isInit = true;
    }
  }

  public override IEnumerator onEndSceneAsync()
  {
    IEnumerator e = this.menu.onEndScene();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetSeaBackground()
  {
    Quest00282Scene quest00282Scene = this;
    Future<GameObject> bgF = new ResourceObject("Prefabs/BackGround/DefaultBackground_sea").Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) bgF.Result, (Object) null))
      quest00282Scene.backgroundPrefab = bgF.Result;
  }
}
