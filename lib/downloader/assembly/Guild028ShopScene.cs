// Decompiled with JetBrains decompiler
// Type: Guild028ShopScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild028ShopScene : NGSceneBase
{
  [SerializeField]
  private Guild028ShopMenu menu;

  public static void ChangeScene(bool isStack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("guild028_shop", isStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Future<WebAPI.Response.GuildtownGuildMedalShop> shopT = WebAPI.GuildtownGuildMedalShop((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = shopT.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (shopT.Result != null)
    {
      e1 = this.menu.Init(Res.Prefabs.shop007_4_1.vscroll7_4_1.Load<GameObject>());
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
    }
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;
}
