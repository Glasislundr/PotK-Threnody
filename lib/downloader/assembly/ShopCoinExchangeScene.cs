// Decompiled with JetBrains decompiler
// Type: ShopCoinExchangeScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class ShopCoinExchangeScene : NGSceneBase
{
  [SerializeField]
  private ShopCoinExchangeMenu menu;
  private static readonly string SCENE_NAME = "shop007_CoinExchange";

  public static void changeScene(bool isStack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(ShopCoinExchangeScene.SCENE_NAME, isStack);
  }

  public static void changeScene(int id, bool isStack = true)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(ShopCoinExchangeScene.SCENE_NAME, (isStack ? 1 : 0) != 0, (object) id);
  }

  public IEnumerator onStartSceneAsync()
  {
    yield return (object) this.onStartSceneAsync(0);
  }

  public IEnumerator onStartSceneAsync(int id)
  {
    ShopCoinExchangeScene coinExchangeScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<GameObject> bgF = Res.Prefabs.BackGround.ShopBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    coinExchangeScene.backgroundPrefab = bgF.Result;
    yield return (object) coinExchangeScene.menu.Init(id);
  }

  public void onStartScene()
  {
    this.menu.InitArticleScroll();
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public void onStartScene(int id) => this.onStartScene();
}
