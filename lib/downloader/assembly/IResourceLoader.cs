// Decompiled with JetBrains decompiler
// Type: IResourceLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using UnityEngine;

#nullable disable
internal interface IResourceLoader
{
  Future<Object> Load(string path, ref ResourceInfo.Resource context);

  Future<Object> DownloadOrCache(string path, ref ResourceInfo.Resource context);

  Object LoadImmediatelyForSmallObject(string path, ref ResourceInfo.Resource context);
}
