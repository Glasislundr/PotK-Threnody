// Decompiled with JetBrains decompiler
// Type: RouletteScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class RouletteScene : NGSceneBase
{
  public RouletteMenu menu;

  public IEnumerator onStartSceneAsync(bool isPreview)
  {
    yield return (object) this.onStartSceneAsync();
  }

  public IEnumerator onStartSceneAsync()
  {
    RouletteScene rouletteScene = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<GameObject> bgF = Res.Prefabs.BackGround.RouletteBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    rouletteScene.backgroundPrefab = bgF.Result;
    Singleton<CommonRoot>.GetInstance().setBackground(rouletteScene.backgroundPrefab);
    string str = "windows";
    e = OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) new string[2]
    {
      str + "/VO_9001_acb",
      str + "/VO_9001_awb"
    }, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = rouletteScene.menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(bool isPreview) => this.onStartScene();

  public void onStartScene()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    this.menu.PlayOttimoAnimation();
  }
}
