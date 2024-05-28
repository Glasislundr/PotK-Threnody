// Decompiled with JetBrains decompiler
// Type: Story0096Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story0096Scene : NGSceneBase
{
  [SerializeField]
  private Story0096Menu menu;

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(100111);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int id)
  {
    Story0096Scene story0096Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.GachaUnitBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    story0096Scene.backgroundPrefab = bgF.Result;
    e = story0096Scene.menu.Init(id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
