// Decompiled with JetBrains decompiler
// Type: Guild0283Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild0283Scene : NGSceneBase
{
  [SerializeField]
  private Guild0283Menu menu;
  private bool isFaildSync;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guild028_3", true);
  }

  public IEnumerator onStartSceneAsync()
  {
    this.isFaildSync = false;
    Future<WebAPI.Response.GuildTop> guildTop = WebAPI.GuildTop(Persist.guildHeaderChat.Data.latestLogId);
    IEnumerator e = guildTop.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (guildTop.Result == null)
    {
      this.isFaildSync = true;
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      e = this.menu.InitializeAsync(guildTop.Result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void onStartScene()
  {
    if (this.isFaildSync)
      return;
    this.menu.Initialize();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }
}
