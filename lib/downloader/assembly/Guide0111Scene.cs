// Decompiled with JetBrains decompiler
// Type: Guide0111Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guide0111Scene : NGSceneBase
{
  public override IEnumerator onInitSceneAsync()
  {
    Guide0111Scene guide0111Scene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Future<GameObject> fBG = new ResourceObject("Prefabs/BackGround/GalleryBackground").Load<GameObject>();
    IEnumerator e = fBG.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    guide0111Scene.backgroundPrefab = fBG.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    if (Singleton<CommonRoot>.GetInstance().isLoading)
    {
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
  }

  public override void onSceneInitialized()
  {
    base.onSceneInitialized();
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public class UpdateInfo
  {
    public long? version;
    public long? verSub;
  }
}
