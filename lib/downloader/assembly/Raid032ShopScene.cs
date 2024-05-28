// Decompiled with JetBrains decompiler
// Type: Raid032ShopScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Raid032ShopScene : NGSceneBase
{
  public static void ChangeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("raid032_shop", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Raid032ShopScene raid032ShopScene = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
    IEnumerator e1 = bgF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    raid032ShopScene.backgroundPrefab = bgF.Result;
    Future<WebAPI.Response.ShopStatus> shopT = WebAPI.ShopStatus((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    })).Then<WebAPI.Response.ShopStatus>((Func<WebAPI.Response.ShopStatus, WebAPI.Response.ShopStatus>) (result =>
    {
      Singleton<NGGameDataManager>.GetInstance().Parse(result);
      return result;
    }));
    e1 = shopT.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (shopT.Result != null)
    {
      e1 = (raid032ShopScene.menuBase as Raid032ShopMenu).Init(Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/shop007_4_1/vscroll7_4_1"));
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    }
  }
}
