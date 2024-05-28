// Decompiled with JetBrains decompiler
// Type: Shop0078Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;

#nullable disable
public class Shop0078Scene : NGSceneBase
{
  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = new Future<NGGameDataManager.StartSceneProxyResult>(new Func<Promise<NGGameDataManager.StartSceneProxyResult>, IEnumerator>(Singleton<NGGameDataManager>.GetInstance().StartSceneAsyncProxyImpl)).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
