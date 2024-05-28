// Decompiled with JetBrains decompiler
// Type: Unit004RegressionScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/Unit/RegressionScene")]
public class Unit004RegressionScene : NGSceneBase
{
  private static readonly string DefaultName = "unit004_regression";
  private Unit004RegressionMenu menu_;
  private int runningCoroutine_;

  public static void changeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Unit004RegressionScene.DefaultName, true);
  }

  private Unit004RegressionMenu mainMenu
  {
    get
    {
      return !Object.op_Inequality((Object) this.menu_, (Object) null) ? (this.menu_ = this.menuBase as Unit004RegressionMenu) : this.menu_;
    }
  }

  public IEnumerator onStartSceneAsync()
  {
    Unit004RegressionScene unit004RegressionScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    unit004RegressionScene.runningCoroutine_ = 0;
    unit004RegressionScene.StartCoroutine(unit004RegressionScene.setBackground());
    yield return (object) unit004RegressionScene.mainMenu.Initialize();
    while (unit004RegressionScene.runningCoroutine_ > 0)
      yield return (object) null;
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().HideLoadingLayer();

  public IEnumerator onBackSceneAsync()
  {
    yield break;
  }

  public void onBackScene() => this.menu_.recoverPopup();

  private IEnumerator setBackground()
  {
    Unit004RegressionScene unit004RegressionScene = this;
    if (!Object.op_Inequality((Object) unit004RegressionScene.backgroundPrefab, (Object) null))
    {
      ++unit004RegressionScene.runningCoroutine_;
      Future<GameObject> ld = new ResourceObject("Prefabs/BackGround/DefaultBackground_storage").Load<GameObject>();
      yield return (object) ld.Wait();
      if (Object.op_Inequality((Object) ld.Result, (Object) null))
        unit004RegressionScene.backgroundPrefab = ld.Result;
      --unit004RegressionScene.runningCoroutine_;
    }
  }
}
