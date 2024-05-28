// Decompiled with JetBrains decompiler
// Type: AssetBundleLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
internal class AssetBundleLoader : IResourceLoader
{
  private static HashSet<string> loading = new HashSet<string>();
  private MonoBehaviour mono;

  public AssetBundleLoader(MonoBehaviour mono) => this.mono = mono;

  private IEnumerator InternalLoad(string path, string assetBundleName, Promise<Object> promise)
  {
    string url = "";
    url = !Persist.fileMoved.Data.isAllMoved ? DLC.ResourceDirectory + assetBundleName : DLC.GetPath(assetBundleName);
    while (AssetBundleLoader.loading.Contains(path))
      yield return (object) null;
    AssetBundleLoader.LoadingLock loadingLock = new AssetBundleLoader.LoadingLock(AssetBundleLoader.loading, path);
    AssetBundleCreateRequest asyncObj = AssetBundle.LoadFromFileAsync(url);
    yield return (object) asyncObj;
    AssetBundle assetBundle = asyncObj.assetBundle;
    if (Object.op_Equality((Object) assetBundle, (Object) null))
    {
      Debug.LogError((object) string.Format("Error to load AssetBundle {0}", (object) path));
    }
    else
    {
      promise.Result = assetBundle.LoadAsset(path);
      if (Object.op_Equality(promise.Result, (Object) null))
        Debug.LogError((object) string.Format("File not found: {0} in {1}", (object) path, (object) assetBundleName));
      this.mono.StartCoroutine(this.AssetBundleUnload(loadingLock, assetBundle));
    }
  }

  private IEnumerator Load_(string path, string assetBundleName, Promise<Object> promise)
  {
    yield return (object) this.mono.StartCoroutine(this.InternalLoad(path, assetBundleName, promise));
  }

  public Future<Object> Load(string path, ref ResourceInfo.Resource context)
  {
    string assetBundleName = context._value._file_name;
    return new Future<Object>((Func<Promise<Object>, IEnumerator>) (promise => this.Load_(path, assetBundleName, promise)));
  }

  private IEnumerator InternalDownloadOrCache(string path, string url, Promise<Object> promise)
  {
    while (AssetBundleLoader.loading.Contains(path))
      yield return (object) null;
    AssetBundleLoader.LoadingLock loadingLock = new AssetBundleLoader.LoadingLock(AssetBundleLoader.loading, path);
    using (WWW www = WWW.LoadFromCacheOrDownload(url, 1))
    {
      yield return (object) www;
      AssetBundle assetBundle = www.assetBundle;
      if (Object.op_Equality((Object) assetBundle, (Object) null))
      {
        Debug.LogError((object) string.Format("Error to load AssetBundle {0}", (object) path));
      }
      else
      {
        promise.Result = assetBundle.LoadAsset(path);
        if (Object.op_Equality(promise.Result, (Object) null))
          Debug.LogError((object) string.Format("File not found: {0} in {1}", (object) path, (object) url));
        this.mono.StartCoroutine(this.AssetBundleUnload(loadingLock, assetBundle));
      }
    }
  }

  private IEnumerator DownloadOrCache_(string path, string url, Promise<Object> promise)
  {
    yield return (object) this.mono.StartCoroutine(this.InternalDownloadOrCache(path, url, promise));
  }

  public Future<Object> DownloadOrCache(string path, ref ResourceInfo.Resource context)
  {
    string url = string.Format("{0}ab/{1}", (object) DLC.UrlBase, (object) context._value._file_name);
    return new Future<Object>((Func<Promise<Object>, IEnumerator>) (promise => this.DownloadOrCache_(path, url, promise)));
  }

  public Object LoadImmediatelyForSmallObject(string path, ref ResourceInfo.Resource context)
  {
    string fileName = context._value._file_name;
    string path1 = !Persist.fileMoved.Data.isAllMoved ? DLC.ResourceDirectory + fileName : DLC.GetPath(fileName);
    if (!CachedFile.Exists(path1))
      return (Object) null;
    byte[] numArray;
    try
    {
      numArray = File.ReadAllBytes(path1);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
      return (Object) null;
    }
    AssetBundle assetBundle = AssetBundle.LoadFromMemory(numArray);
    if (Object.op_Equality((Object) assetBundle, (Object) null))
      return (Object) null;
    Object @object = assetBundle.LoadAsset(path);
    if (Object.op_Equality(@object, (Object) null))
      Debug.LogError((object) string.Format("File not found: {0} in {1}", (object) path, (object) fileName));
    assetBundle.Unload(false);
    return @object;
  }

  private IEnumerator AssetBundleUnload(
    AssetBundleLoader.LoadingLock loadingLock,
    AssetBundle assetBundle)
  {
    yield return (object) new WaitForSeconds(0.2f);
    assetBundle.Unload(false);
    loadingLock.Dispose();
    assetBundle = (AssetBundle) null;
  }

  private class LoadingLock : IDisposable
  {
    private HashSet<string> loading;
    private string path;

    public LoadingLock(HashSet<string> loading, string path)
    {
      loading.Add(path);
      this.loading = loading;
      this.path = path;
    }

    public void Dispose() => this.loading.Remove(this.path);
  }
}
