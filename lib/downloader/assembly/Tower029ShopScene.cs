// Decompiled with JetBrains decompiler
// Type: Tower029ShopScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029ShopScene : NGSceneBase
{
  private static readonly string DEFAULT_NAME = "tower029_medal_shop";

  public static void changeScene(bool isStack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Tower029ShopScene.DEFAULT_NAME, isStack);
  }

  public IEnumerator onStartSceneAsync()
  {
    Tower029ShopScene tower029ShopScene = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<GameObject> ldBG = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
    IEnumerator e1 = ldBG.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    tower029ShopScene.backgroundPrefab = ldBG.Result;
    Future<WebAPI.Response.TowerShopTop> shopT = WebAPI.TowerShopTop((Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    e1 = shopT.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (shopT.Result != null)
    {
      e1 = (tower029ShopScene.menuBase as Tower029ShopMenu).Init(Res.Prefabs.shop007_4_1.vscroll7_4_1.Load<GameObject>());
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      tower029ShopScene.bgmFile = TowerUtil.BgmFile;
      tower029ShopScene.bgmName = TowerUtil.BgmName;
    }
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;
}
