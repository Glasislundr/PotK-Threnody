// Decompiled with JetBrains decompiler
// Type: ResourceDownloader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using gu3.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniLinq;
using UnityEngine;

#nullable disable
public static class ResourceDownloader
{
  private const int DELETE_MAX_YIELD_COUNT = 50;
  private static DLC assetBundle;
  private static DLC streamingAssets;
  private static DLC additions;
  private static string error;
  private static bool completed;
  private static bool isFirstDownLoadConfirm;

  private static IEnumerator DownloadJson(string url, Promise<string> promise)
  {
    using (WWW www = new WWW(url))
    {
      yield return (object) www;
      if (www.error != null)
      {
        Debug.LogError((object) www.error);
        promise.Exception = new Exception(www.error);
        yield break;
      }
      else
      {
        IEnumerator e = Singleton<ResourceManager>.GetInstance().SaveResourceInfo(www.bytes);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    promise.Result = (string) null;
  }

  public static ResourceDownloader.ProgressInfo Progress
  {
    get
    {
      if (ResourceDownloader.assetBundle == null || ResourceDownloader.streamingAssets == null)
        return (ResourceDownloader.ProgressInfo) null;
      ResourceDownloader.ProgressInfo progress;
      if (ResourceDownloader.isFirstDownLoadConfirm && ResourceDownloader.additions != null)
        progress = new ResourceDownloader.ProgressInfo()
        {
          Numerator = ResourceDownloader.additions.GetDownloadedSize(),
          Denominator = ResourceDownloader.additions.GetDownloadSize()
        };
      else
        progress = new ResourceDownloader.ProgressInfo()
        {
          Numerator = ResourceDownloader.assetBundle.GetDownloadedSize() + ResourceDownloader.streamingAssets.GetDownloadedSize() + (ResourceDownloader.additions != null ? ResourceDownloader.additions.GetDownloadedSize() : 0L),
          Denominator = ResourceDownloader.assetBundle.GetDownloadSize() + ResourceDownloader.streamingAssets.GetDownloadSize() + (ResourceDownloader.additions != null ? ResourceDownloader.additions.GetDownloadSize() : 0L)
        };
      return progress;
    }
  }

  public static bool Completed
  {
    get => ResourceDownloader.error != null || ResourceDownloader.completed;
    set => ResourceDownloader.completed = value;
  }

  public static string Error => ResourceDownloader.error;

  public static void Restart() => ResourceDownloader.error = (string) null;

  public static bool IsDlcVersionChange()
  {
    string str = "";
    try
    {
      if (File.Exists(ResourceManager.dlcVersionPath))
        str = File.ReadAllText(ResourceManager.dlcVersionPath, Encoding.UTF8);
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ex.ToString());
    }
    return str != StartupDownLoad.GetLastestVersion() || !File.Exists(ResourceManager.pathsJsonPath);
  }

  private static IEnumerator InternalStart(
    MonoBehaviour mono,
    string urlBase,
    string dlcVersion,
    bool confirmDLC,
    Action actionAbort)
  {
    while (!Caching.ready)
      yield return (object) null;
    bool dlcVersionChanged = ResourceDownloader.IsDlcVersionChange();
    IEnumerator e;
    if (dlcVersionChanged)
    {
      string jsonUrl = !Persist.normalDLC.Data.IsSound ? string.Format("{0}{1}_{2}.json", (object) urlBase, (object) Revision.ApplicationVersion, (object) dlcVersion) : string.Format("{0}{1}_{2}.json", (object) urlBase, (object) Revision.ApplicationVersion, (object) (dlcVersion + "_normal"));
label_5:
      Future<string> future = new Future<string>((Func<Promise<string>, IEnumerator>) (promise => ResourceDownloader.DownloadJson(jsonUrl, promise)));
      e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (future.Exception != null)
      {
        ResourceDownloader.error = future.Exception.ToString();
        Debug.LogError((object) ResourceDownloader.error);
        ResourceDownloader.error = Consts.GetInstance().dlc_fail_download_text;
        while (ResourceDownloader.error != null)
          yield return (object) null;
        goto label_5;
      }
    }
    e = Singleton<ResourceManager>.GetInstance().InitResourceInfo();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ResourceInfo resourceInfo = Singleton<ResourceManager>.GetInstance().ResourceInfo;
    int num = 0;
    string dlcResourceDirectory = DLC.ResourceDirectory;
    ResourceManager.CreateSaveDLCDir(dlcResourceDirectory);
    List<string> assetBundlePaths = new List<string>();
    List<string> streamingAssetPaths = new List<string>();
    HashSet<string> fileNames = new HashSet<string>();
    bool isMoved = Persist.fileMoved.Data.isAllMoved;
    fileNames = !isMoved ? new HashSet<string>((IEnumerable<string>) FileManager.GetEntries(dlcResourceDirectory)) : DLC.GetEntries();
    if (dlcVersionChanged)
    {
      ResourceInfo.Resource resource1 = resourceInfo.FirstOrDefault<ResourceInfo.Resource>((Func<ResourceInfo.Resource, bool>) (x => x._key == Singleton<NGSoundManager>.GetInstance().platform + "/VO_9999_acb"));
      bool isDLCMobVoice = resource1._key != null && resource1._value.Exists;
      bool isDLCOpMovie = !Persist.tutorial.Data.IsFinishTutorial();
      foreach (ResourceInfo.Resource resource2 in resourceInfo)
      {
        ResourceInfo.Value obj = resource2._value;
        switch (obj._path_type)
        {
          case ResourceInfo.PathType.AssetBundle:
            if (fileNames.Contains(obj._file_name))
            {
              if (isMoved)
                CachedFile.Add(DLC.GetPath(obj._file_name));
              else
                CachedFile.Add(dlcResourceDirectory + obj._file_name);
              fileNames.Remove(obj._file_name);
              break;
            }
            if (ResourceManager.IsInitialDLCTarget(resource2._key, isDLCMobVoice, isDLCOpMovie))
            {
              assetBundlePaths.Add(resource2._key);
              break;
            }
            break;
          case ResourceInfo.PathType.StreamingAssets:
            if (fileNames.Contains(obj._file_name))
            {
              if (isMoved)
                CachedFile.Add(DLC.GetPath(obj._file_name));
              else
                CachedFile.Add(dlcResourceDirectory + obj._file_name);
              fileNames.Remove(obj._file_name);
              break;
            }
            if (ResourceManager.IsInitialDLCTarget(resource2._key, isDLCMobVoice, isDLCOpMovie))
            {
              streamingAssetPaths.Add(resource2._key);
              break;
            }
            break;
        }
        if (++num >= 1000)
        {
          yield return (object) null;
          num = 0;
        }
      }
      foreach (string fileName in fileNames)
      {
        try
        {
          if (isMoved)
            File.Delete(DLC.GetPath(fileName));
          else
            File.Delete(dlcResourceDirectory + "/" + fileName);
        }
        catch (Exception ex)
        {
        }
        if (++num >= 200)
        {
          yield return (object) null;
          num = 0;
        }
      }
      ResourceDownloader.assetBundle = new DLC(resourceInfo, assetBundlePaths.ToArray(), true);
      ResourceDownloader.streamingAssets = new DLC(resourceInfo, streamingAssetPaths.ToArray(), true);
      ResourceDownloader.additions = (DLC) null;
      if (confirmDLC)
      {
        ResourceDownloader.isFirstDownLoadConfirm = true;
        List<string> addLstPath = new List<string>();
        foreach (string str in assetBundlePaths.Where<string>((Func<string, bool>) (x => x.StartsWith("MasterData"))))
          addLstPath.Add(str);
        foreach (string str in assetBundlePaths.Where<string>((Func<string, bool>) (x => x.StartsWith("GUI/tutorial_sozai"))))
          addLstPath.Add(str);
        foreach (string str in assetBundlePaths.Where<string>((Func<string, bool>) (x => x.StartsWith("Prefabs/Tutorial"))))
          addLstPath.Add(str);
        foreach (string str in streamingAssetPaths.Where<string>((Func<string, bool>) (x => x.Contains("BgmCueSheet") || x.Contains("SECueSheet") || x.Contains("punk_music"))))
          addLstPath.Add(str);
        yield return (object) ResourceDownloader.appendDownload(resourceInfo, (Action<DLC>) (r => ResourceDownloader.additions = r), addLstPath);
        DLC[] loaders = new DLC[1]
        {
          ResourceDownloader.additions
        };
        bool toNext = false;
        yield return (object) ModalDownloadWindow.Show((IEnumerable<DLC>) loaders, (Action) (() => toNext = true), string.Empty);
        if (!toNext)
        {
          actionAbort();
          yield break;
        }
      }
      else
      {
        ResourceDownloader.isFirstDownLoadConfirm = false;
        yield return (object) ResourceDownloader.appendDownload(resourceInfo, (Action<DLC>) (r => ResourceDownloader.additions = r));
      }
label_81:
      long requiredSize = ResourceDownloader.assetBundle.GetStoreSize() + ResourceDownloader.streamingAssets.GetStoreSize() + (ResourceDownloader.additions != null ? ResourceDownloader.additions.GetStoreSize() : 0L);
      long int64 = Convert.ToInt64(Sys.GetAvailableStorageBytes());
      if (requiredSize > int64)
      {
        ResourceDownloader.error = Consts.GetInstance().dlcNotEnoughDiskSpace(requiredSize);
        while (ResourceDownloader.error != null)
          yield return (object) null;
        goto label_81;
      }
      else
      {
        if (confirmDLC)
        {
label_87:
          e = ResourceDownloader.additions.Start(mono);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (ResourceDownloader.additions.Error != null)
          {
            ResourceDownloader.error = ResourceDownloader.additions.Error;
            while (ResourceDownloader.error != null)
              yield return (object) null;
            goto label_87;
          }
        }
        else
        {
label_94:
          e = ResourceDownloader.assetBundle.Start(mono);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (ResourceDownloader.assetBundle.Error != null)
          {
            ResourceDownloader.error = ResourceDownloader.assetBundle.Error;
            while (ResourceDownloader.error != null)
              yield return (object) null;
            goto label_94;
          }
          else
          {
label_101:
            e = ResourceDownloader.streamingAssets.Start(mono);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            if (ResourceDownloader.streamingAssets.Error != null)
            {
              ResourceDownloader.error = ResourceDownloader.streamingAssets.Error;
              while (ResourceDownloader.error != null)
                yield return (object) null;
              goto label_101;
            }
            else if (ResourceDownloader.additions != null)
            {
label_109:
              e = ResourceDownloader.additions.Start(mono);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              if (ResourceDownloader.additions.Error != null)
              {
                ResourceDownloader.error = ResourceDownloader.additions.Error;
                while (ResourceDownloader.error != null)
                  yield return (object) null;
                goto label_109;
              }
            }
          }
        }
        File.WriteAllText(ResourceManager.dlcVersionPath, dlcVersion, Encoding.UTF8);
        ResourceManager.DLCVersion = dlcVersion;
      }
    }
    else
    {
      foreach (ResourceInfo.Resource resource in resourceInfo)
      {
        ResourceInfo.Value obj = resource._value;
        switch (obj._path_type)
        {
          case ResourceInfo.PathType.AssetBundle:
            if (fileNames.Contains(obj._file_name))
            {
              if (isMoved)
              {
                CachedFile.Add(DLC.GetPath(obj._file_name));
                continue;
              }
              CachedFile.Add(dlcResourceDirectory + obj._file_name);
              continue;
            }
            continue;
          case ResourceInfo.PathType.StreamingAssets:
            if (fileNames.Contains(obj._file_name))
            {
              if (isMoved)
              {
                CachedFile.Add(DLC.GetPath(obj._file_name));
                continue;
              }
              CachedFile.Add(dlcResourceDirectory + obj._file_name);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    ResourceDownloader.completed = true;
  }

  private static IEnumerator appendDownload(
    ResourceInfo resourceInfo,
    Action<DLC> result,
    List<string> addLstPath = null)
  {
    string path = Path.Combine(Application.streamingAssetsPath, "first_dlc");
    string str1 = (string) null;
    if (File.Exists(path))
      str1 = File.ReadAllText(path, Encoding.UTF8);
    List<string> source = new List<string>();
    if (!string.IsNullOrEmpty(str1))
    {
      string str2 = "~/";
      string str3 = Singleton<NGSoundManager>.GetInstance().platform + "/";
      char[] chArray = new char[1]{ '\uFEFF' };
      using (StringReader stringReader = new StringReader(str1.TrimStart(chArray)))
      {
        while (stringReader.Peek() > -1)
        {
          string str4 = stringReader.ReadLine();
          if (!string.IsNullOrEmpty(str4))
          {
            if (str4.StartsWith(str2))
              source.Add(str3 + str4.Substring(str2.Length));
            else
              source.Add(str4);
          }
          else
            break;
        }
      }
    }
    if (addLstPath != null)
      source.AddRange((IEnumerable<string>) addLstPath);
    result(source.Any<string>() ? new DLC(resourceInfo, source.ToArray()) : (DLC) null);
    yield break;
  }

  public static Coroutine Start(
    MonoBehaviour mono,
    string urlBase,
    string dlcVersion,
    bool confirmDLC,
    Action actionAbort)
  {
    ResourceDownloader.completed = false;
    return mono.StartCoroutine(ResourceDownloader.InternalStart(mono, urlBase, dlcVersion, confirmDLC, actionAbort));
  }

  public static IEnumerator CleanCache(Action<int, int> progress)
  {
    if (Directory.Exists(DLC.ResourceDirectory))
    {
      ResourceDownloader.ClearDLC();
      CachedFile.Clear();
      Caching.ClearCache();
      IEnumerator e = ResourceDownloader.DeleteContents(progress);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      File.Delete(ResourceManager.dlcVersionPath);
      File.Delete(ResourceManager.pathsJsonPath);
      ResourceManager.alreadyDirPath.Clear();
    }
  }

  public static void ClearDLC()
  {
    ResourceDownloader.assetBundle = (DLC) null;
    ResourceDownloader.streamingAssets = (DLC) null;
  }

  public static IEnumerator DeleteContents(Action<int, int> progress)
  {
    bool isAllMoved = Persist.fileMoved.Data.isAllMoved;
    HashSet<string> stringSet;
    try
    {
      stringSet = !isAllMoved ? new HashSet<string>((IEnumerable<string>) FileManager.GetEntries(DLC.ResourceDirectory)) : DLC.GetEntries();
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
      yield break;
    }
    List<string> paths = new List<string>();
    foreach (string str in stringSet)
    {
      if (isAllMoved)
        paths.Add(DLC.GetPath(str));
      else
        paths.Add(Path.Combine(DLC.ResourceDirectory, str));
    }
    int count = 0;
    int num = 0;
    foreach (string path in paths)
    {
      try
      {
        File.Delete(path);
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
      if (++num >= 50)
      {
        yield return (object) null;
        num = 0;
      }
      progress(++count, paths.Count);
    }
  }

  public static IEnumerator DeleteContents(List<string> fileNames)
  {
    int num = 0;
    bool isMoved = Persist.fileMoved.Data.isAllMoved;
    foreach (string fileName in fileNames)
    {
      try
      {
        File.Delete(!isMoved ? Path.Combine(DLC.ResourceDirectory, fileName) : DLC.GetPath(fileName));
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
      if (++num >= 50)
      {
        yield return (object) null;
        num = 0;
      }
    }
  }

  public static IEnumerator ShowMoveDLCFile()
  {
    if (Persist.fileMoved.Data.isIncomplete)
      yield return (object) ResourceDownloader.MoveAllDLC();
    else if (!Persist.fileMoved.Data.isAskMoved && !Persist.fileMoved.Data.isAllMoved)
    {
      if (!File.Exists(ResourceManager.dlcVersionPath))
      {
        Persist.fileMoved.Data.isAskMoved = true;
        Persist.fileMoved.Data.isAllMoved = true;
        Persist.fileMoved.Data.isIncomplete = false;
        Persist.fileMoved.Flush();
      }
      else
      {
        bool isOk = false;
        bool isWait = false;
        ModalWindow.ShowYesNo("ファイル更新確認", "アプリ最適化のためアプリケーションファイルの更新を行います。\n[ff4500]※通信は発生しません。[-]\n[ff4500]※後でアプリ内で行うことも可能です。[-]", (Action) (() =>
        {
          isOk = true;
          isWait = true;
        }), (Action) (() =>
        {
          isOk = false;
          isWait = true;
        }));
        while (!isWait)
          yield return (object) null;
        if (isOk)
        {
          yield return (object) ResourceDownloader.MoveAllDLC();
        }
        else
        {
          Persist.fileMoved.Data.isAskMoved = true;
          Persist.fileMoved.Data.isAllMoved = false;
          Persist.fileMoved.Data.isIncomplete = false;
          Persist.fileMoved.Flush();
        }
      }
    }
  }

  public static IEnumerator MoveAllDLC()
  {
    App.SetAutoSleep(false);
    ModalWindow window = ModalWindow.Show("ファイル更新中", "", (Action) (() => { }));
    window.DisableOkButton();
    List<string> fileNames = new List<string>();
    foreach (string str in new HashSet<string>((IEnumerable<string>) FileManager.GetEntries(DLC.ResourceDirectory)))
    {
      if (str.Length != 1 && !(str == "temp"))
        fileNames.Add(str);
    }
    Persist.fileMoved.Data.isAskMoved = true;
    Persist.fileMoved.Data.isAllMoved = false;
    Persist.fileMoved.Data.isIncomplete = true;
    Persist.fileMoved.Flush();
    int i = 0;
    foreach (string str in fileNames)
    {
      string path = DLC.GetPath(str);
      ResourceManager.CreateSaveDLCDir(Path.GetDirectoryName(path));
      try
      {
        File.Move(Path.Combine(DLC.ResourceDirectory, str), path);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ex.ToString());
      }
      if (i % 33 == 0)
      {
        window.SetText(string.Format("しばらくお待ちください。\n{0}/{1}", (object) i.ToString(), (object) fileNames.Count.ToString()));
        yield return (object) null;
      }
      if (i % 100 == 0)
      {
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }
      ++i;
    }
    window.OnOk();
    Persist.fileMoved.Data.isAskMoved = true;
    Persist.fileMoved.Data.isAllMoved = true;
    Persist.fileMoved.Data.isIncomplete = false;
    Persist.fileMoved.Flush();
    App.SetAutoSleep(true);
  }

  public class ProgressInfo
  {
    public long Numerator;
    public long Denominator;
  }
}
