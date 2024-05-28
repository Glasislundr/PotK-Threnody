// Decompiled with JetBrains decompiler
// Type: ResourceLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
internal class ResourceLoader : IResourceLoader
{
  private IEnumerator Load_(string path, Promise<Object> promise)
  {
    promise.Result = Resources.Load(path);
    yield break;
  }

  public Future<Object> Load(string path, ref ResourceInfo.Resource context)
  {
    return new Future<Object>((Func<Promise<Object>, IEnumerator>) (promise => this.Load_(path, promise)));
  }

  public Future<Object> DownloadOrCache(string path, ref ResourceInfo.Resource context)
  {
    return new Future<Object>((Func<Promise<Object>, IEnumerator>) (promise => this.Load_(path, promise)));
  }

  public Object LoadImmediatelyForSmallObject(string path, ref ResourceInfo.Resource context)
  {
    return Resources.Load(path);
  }
}
