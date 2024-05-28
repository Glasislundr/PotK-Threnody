// Decompiled with JetBrains decompiler
// Type: Guild0286Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild0286Scene : NGSceneBase
{
  [SerializeField]
  private Guild0286Menu menu;
  private bool isFaildSync;

  public static void ChangeScene(bool bStack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guild028_6", bStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    this.isFaildSync = false;
    Future<WebAPI.Response.GuildGiftReceiveList> receive = WebAPI.GuildGiftReceiveList(false);
    IEnumerator e = receive.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (receive.Result == null)
    {
      this.isFaildSync = true;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      e = this.menu.Init(receive.Result.player_gift);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void onStartScene()
  {
    if (this.isFaildSync)
      return;
    this.currentSceneGuildChatDisplayingStatus = NGSceneBase.GuildChatDisplayingStatus.Closed;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }
}
