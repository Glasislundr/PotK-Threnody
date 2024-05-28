// Decompiled with JetBrains decompiler
// Type: Shop0074Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Shop0074Scene : NGSceneBase
{
  [SerializeField]
  private Shop0074Menu menu;

  public static void changeScene(bool stack)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("shop007_4", stack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Shop0074Scene shop0074Scene = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
    IEnumerator e1 = bgF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    shop0074Scene.backgroundPrefab = bgF.Result;
    if (!WebAPI.IsResponsedAtRecent("ShopStatus"))
    {
      Future<WebAPI.Response.ShopStatus> shoplistF = WebAPI.ShopStatus((Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      })).Then<WebAPI.Response.ShopStatus>((Func<WebAPI.Response.ShopStatus, WebAPI.Response.ShopStatus>) (result =>
      {
        Singleton<NGGameDataManager>.GetInstance().Parse(result);
        return result;
      }));
      e1 = shoplistF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (shoplistF.Result == null)
        yield break;
      else
        shoplistF = (Future<WebAPI.Response.ShopStatus>) null;
    }
    e1 = shop0074Scene.menu.Init(Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/shop007_4_1/vscroll7_4_1"));
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }
}
