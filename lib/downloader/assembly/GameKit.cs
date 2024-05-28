// Decompiled with JetBrains decompiler
// Type: GameKit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GameKit : MonoBehaviour
{
  private static Action _signInCallback;

  private void Start()
  {
  }

  public static void ShowAchievements(Action callback = null)
  {
    GameKit.SignIn((Action) (() =>
    {
      if (callback == null)
        return;
      callback();
    }));
  }

  private static void SignIn(Action callback)
  {
  }

  private static void _SignIn(Action callback)
  {
  }

  private IEnumerator SyncForPlatform()
  {
    while (SMManager.Get<Player>() == null)
      yield return (object) null;
    Future<WebAPI.Response.GamekitAll> r = WebAPI.GamekitAll();
    IEnumerator e = r.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameKit.SyncAchievements(r.Result.achievements);
  }

  public static void SyncAchievements(object achievements)
  {
    foreach (Dictionary<string, object> achievement in (IEnumerable) achievements)
    {
      PlayerGameKit2AchievementResult achievementResult = new PlayerGameKit2AchievementResult(achievement);
      Singleton<SocialManager>.GetInstance().ReportProgress(achievementResult.achievement_id, (double) achievementResult.progress);
    }
  }

  public static void SyncAchievements(UserAchievements[] achievements)
  {
  }

  public void OnSignInFailed() => Debug.LogWarning((object) "**** OnSignInFailed() ****");

  public void OnSignInSucceeded()
  {
    Debug.LogWarning((object) "**** OnSignInSucceeded() ****");
    if (GameKit._signInCallback != null)
      GameKit._signInCallback();
    this.StartCoroutine(this.SyncForPlatform());
  }
}
