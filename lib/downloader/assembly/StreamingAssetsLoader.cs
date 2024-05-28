// Decompiled with JetBrains decompiler
// Type: StreamingAssetsLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.IO;
using UnityEngine;

#nullable disable
internal class StreamingAssetsLoader : IResourceLoader
{
  private const int GC_LOADED_BYTE_SIZE = 1048576;

  public Future<Object> Load(string path, ref ResourceInfo.Resource context)
  {
    return Future.Single<Object>(this.LoadImmediatelyForSmallObject(path, ref context));
  }

  public Future<Object> DownloadOrCache(string path, ref ResourceInfo.Resource context)
  {
    return Future.Single<Object>(this.LoadImmediatelyForSmallObject(path, ref context));
  }

  public Object LoadImmediatelyForSmallObject(string path, ref ResourceInfo.Resource context)
  {
    if (context._value._object_type != ResourceInfo.ObjectType.None && context._value._object_type != ResourceInfo.ObjectType.Texture2D)
    {
      Debug.LogError((object) ("Unknown type: " + context._value._object_type.ToString()));
      return (Object) null;
    }
    Texture2D texture2D = new Texture2D(4, 4, (TextureFormat) 5, false, true);
    ((Texture) texture2D).wrapMode = (TextureWrapMode) 1;
    byte[] numArray = new byte[0];
    if (!ImageConversion.LoadImage(texture2D, !Persist.fileMoved.Data.isAllMoved ? File.ReadAllBytes(DLC.ResourceDirectory + context._value._file_name) : File.ReadAllBytes(DLC.GetPath(context._value._file_name))))
      Debug.LogError((object) ("Failed LoadImage: " + path));
    texture2D.Apply(false, true);
    ((Object) texture2D).name = path;
    return (Object) texture2D;
  }
}
