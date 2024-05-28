// Decompiled with JetBrains decompiler
// Type: Quest00229Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest00229Scene : NGSceneBase
{
  [SerializeField]
  private Quest00229Menu menu;

  public static void ChangeScene(int scoreCampaignID, bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_29", (stack ? 1 : 0) != 0, (object) scoreCampaignID);
  }

  public IEnumerator onStartSceneAsync(int scoreCampaignID)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.QuestRankingExtra> ranking = WebAPI.QuestRankingExtra(scoreCampaignID, (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(error);
      Singleton<NGSceneManager>.GetInstance().changeScene("quest002_17", false);
    }));
    IEnumerator e = ranking.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (ranking.Result != null)
    {
      e = this.menu.Init(ranking.Result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }
}
