// Decompiled with JetBrains decompiler
// Type: Story00912Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Story00912Scene : NGSceneBase
{
  [SerializeField]
  private Story00912Menu menu;

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(100111, 100111, 100111, 910011321);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int id, int id2, int id3, int questSId)
  {
    Story00912Scene story00912Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.GachaUnitBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    story00912Scene.backgroundPrefab = bgF.Result;
    e = story00912Scene.menu.Init(id, id2, id3, questSId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    story00912Scene.menu.ScrollContainerResolvePosition();
  }
}
