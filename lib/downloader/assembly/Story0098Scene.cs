// Decompiled with JetBrains decompiler
// Type: Story0098Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Story0098Scene : NGSceneBase
{
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private Story0098Menu menu;

  public IEnumerator onStartSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync(bool connect)
  {
    int beforeLoadingMode = Singleton<CommonRoot>.GetInstance().loadingMode;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.menu.InitScene(connect);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.ScrollContainer.ResolvePosition();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = beforeLoadingMode;
  }
}
