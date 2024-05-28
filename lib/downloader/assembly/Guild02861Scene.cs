// Decompiled with JetBrains decompiler
// Type: Guild02861Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild02861Scene : NGSceneBase
{
  [SerializeField]
  private Guild02861Menu menu;
  private bool isFaildSync;

  public static void ChangeScene(bool bStack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guild028_6_1", bStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    this.isFaildSync = false;
    Future<WebAPI.Response.GuildGiftSendList> send = WebAPI.GuildGiftSendList(false);
    IEnumerator e = send.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (send.Result == null)
    {
      this.isFaildSync = true;
      MypageScene.ChangeSceneOnError();
    }
    else
    {
      e = this.menu.Init(send.Result.player_send);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void onStartScene()
  {
    if (this.isFaildSync)
      return;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }
}
