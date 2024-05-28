// Decompiled with JetBrains decompiler
// Type: DLC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using gu3.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UniLinq;
using UnityEngine;

#nullable disable
public class DLC
{
  private const int CONSUMER_COUNT = 5;
  private Dictionary<string, DLC.Data> dataDic = new Dictionary<string, DLC.Data>();
  private string error;
  private DLC.Consumer[] consumers = new DLC.Consumer[0];

  public static string ResourceDirectory => StorageUtil.persistentDataPath + "/cache/";

  public static string ResourceTempDirectory => DLC.ResourceDirectory + "temp/";

  public static string UrlBase => ServerSelector.DlcUrl;

  public static string GetPath(string fileName)
  {
    return Path.Combine(Path.Combine(DLC.ResourceDirectory, (Convert.ToInt32(fileName[fileName.Length - 10]) % 4).ToString()), fileName);
  }

  public static HashSet<string> GetEntries()
  {
    List<string> collection = new List<string>();
    foreach (string entry in FileManager.GetEntries(DLC.ResourceDirectory))
    {
      if (entry.Length == 1)
      {
        string[] entries = FileManager.GetEntries(Path.Combine(DLC.ResourceDirectory, entry));
        collection.AddRange((IEnumerable<string>) ((IEnumerable<string>) entries).ToList<string>());
      }
    }
    return new HashSet<string>((IEnumerable<string>) collection);
  }

  public DLC(ResourceInfo resourceInfo, string[] paths, bool fileCheckDisable = false)
  {
    string[] array = new HashSet<string>((IEnumerable<string>) paths).Shuffle<string>().ToArray<string>();
    bool isAllMoved = Persist.fileMoved.Data.isAllMoved;
    foreach (string str in array)
    {
      ResourceInfo.Resource resource = resourceInfo.SearchResourceInfo(str);
      if (!string.IsNullOrEmpty(resource._key))
      {
        bool flag = !fileCheckDisable && (!isAllMoved ? CachedFile.Exists(DLC.ResourceDirectory + resource._value._file_name) : CachedFile.Exists(DLC.GetPath(resource._value._file_name)));
        switch (resource._value._path_type)
        {
          case ResourceInfo.PathType.AssetBundle:
            string fileName1 = resource._value._file_name;
            this.dataDic[str] = new DLC.Data()
            {
              Path = str,
              Name = fileName1,
              Exists = flag,
              AssetBundle = true,
              FileSize = (int) resource._value._file_size,
              Uncompress = false
            };
            continue;
          case ResourceInfo.PathType.StreamingAssets:
            string fileName2 = resource._value._file_name;
            this.dataDic[str] = new DLC.Data()
            {
              Path = str,
              Name = fileName2,
              Exists = flag,
              AssetBundle = false,
              FileSize = (int) resource._value._file_size,
              Uncompress = false
            };
            continue;
          default:
            continue;
        }
      }
    }
  }

  public DLC(List<ResourceInfo.Resource> resources)
  {
    foreach (ResourceInfo.Resource resource in new HashSet<ResourceInfo.Resource>((IEnumerable<ResourceInfo.Resource>) resources).Shuffle<ResourceInfo.Resource>().ToArray<ResourceInfo.Resource>())
    {
      if (!string.IsNullOrEmpty(resource._key))
      {
        switch (resource._value._path_type)
        {
          case ResourceInfo.PathType.AssetBundle:
            string fileName1 = resource._value._file_name;
            this.dataDic[resource._key] = new DLC.Data()
            {
              Path = resource._key,
              Name = fileName1,
              Exists = false,
              AssetBundle = true,
              FileSize = (int) resource._value._file_size,
              Uncompress = false
            };
            continue;
          case ResourceInfo.PathType.StreamingAssets:
            string fileName2 = resource._value._file_name;
            this.dataDic[resource._key] = new DLC.Data()
            {
              Path = resource._key,
              Name = fileName2,
              Exists = false,
              AssetBundle = false,
              FileSize = (int) resource._value._file_size,
              Uncompress = false
            };
            continue;
          default:
            continue;
        }
      }
    }
  }

  public long GetDownloadedSize(bool includesExistFile = false)
  {
    return (includesExistFile ? this.GetDownloadSize(true) - this.GetDownloadSize() : 0L) + ((IEnumerable<DLC.Consumer>) this.consumers).Sum<DLC.Consumer>((Func<DLC.Consumer, long>) (x => (long) x.Downloaded));
  }

  public bool DownloadRequired => this.GetDownloadSize() != 0L;

  public long GetDownloadSize(bool includesExistFile = false)
  {
    return this.dataDic.Where<KeyValuePair<string, DLC.Data>>((Func<KeyValuePair<string, DLC.Data>, bool>) (x => includesExistFile || !x.Value.Exists)).Sum<KeyValuePair<string, DLC.Data>>((Func<KeyValuePair<string, DLC.Data>, long>) (x => (long) x.Value.FileSize));
  }

  public long GetStoredSize(bool includesExistFile = false)
  {
    return (includesExistFile ? this.GetStoreSize(true) - this.GetStoreSize() : 0L) + ((IEnumerable<DLC.Consumer>) this.consumers).Sum<DLC.Consumer>((Func<DLC.Consumer, long>) (x => (long) x.Stored));
  }

  public long GetStoreSize(bool includesExistFile = false)
  {
    return this.dataDic.Where<KeyValuePair<string, DLC.Data>>((Func<KeyValuePair<string, DLC.Data>, bool>) (x => includesExistFile || !x.Value.Exists)).Sum<KeyValuePair<string, DLC.Data>>((Func<KeyValuePair<string, DLC.Data>, long>) (x => (long) x.Value.FileSize));
  }

  public string Error => this.error;

  public IEnumerator Start(MonoBehaviour mono)
  {
    this.error = (string) null;
    long storeSize = this.GetStoreSize();
    long int64 = Convert.ToInt64(Sys.GetAvailableStorageBytes());
    if (storeSize > int64)
    {
      this.error = Consts.GetInstance().dlcNotEnoughDiskSpace(storeSize);
    }
    else
    {
      if (this.consumers.Length == 0)
      {
        Dictionary<string, DLC.Data> dic = new Dictionary<string, DLC.Data>();
        foreach (KeyValuePair<string, DLC.Data> keyValuePair in this.dataDic)
        {
          if (!keyValuePair.Value.Exists && !keyValuePair.Value.Completed)
            dic.Add(keyValuePair.Key, keyValuePair.Value);
        }
        this.consumers = Enumerable.Range(0, 5).Select<int, DLC.Consumer>((Func<int, DLC.Consumer>) (n => new DLC.Consumer(dic))).ToArray<DLC.Consumer>();
        foreach (DLC.Consumer consumer in this.consumers)
          mono.StartCoroutine(consumer.Run());
      }
      else
      {
        foreach (DLC.Consumer consumer in this.consumers)
        {
          if (!consumer.Completed && !string.IsNullOrEmpty(consumer.Error))
            consumer.Restart();
        }
      }
      while (!((IEnumerable<DLC.Consumer>) this.consumers).All<DLC.Consumer>((Func<DLC.Consumer, bool>) (x => x.Completed)))
      {
        yield return (object) null;
        if (((IEnumerable<DLC.Consumer>) this.consumers).Any<DLC.Consumer>((Func<DLC.Consumer, bool>) (x => x.Error != null)))
        {
          foreach (DLC.Consumer consumer in this.consumers)
            consumer.Interrupt();
          this.error = Consts.GetInstance().dlc_fail_download_text;
          yield break;
        }
      }
      foreach (DLC.Consumer consumer in this.consumers)
        consumer.Dispose();
    }
  }

  private class WWWFile : IDisposable
  {
    private string url;
    private string path;
    private bool succeeded;
    private string error;
    private WWW www;
    private int beforeBytesDownloaded;

    public bool Succeeded => this.succeeded;

    public string Error => this.error;

    public WWWFile(string url, string path)
    {
      this.path = path;
      this.www = new WWW(url);
      this.beforeBytesDownloaded = 0;
    }

    public IEnumerator Wait(Action<int> progressCallback)
    {
      Stopwatch watch = new Stopwatch();
      while (!this.www.isDone)
      {
        int bytesDownloaded = this.www.bytesDownloaded;
        if (this.beforeBytesDownloaded == bytesDownloaded)
        {
          if (watch.ElapsedMilliseconds > 30000L)
          {
            this.error = string.Format("download wait time out. url: {0}, bytesDownloaded: {1}", (object) this.www.url, (object) bytesDownloaded);
            this.www.Dispose();
            yield break;
          }
        }
        else
        {
          watch.Restart();
          progressCallback(bytesDownloaded - this.beforeBytesDownloaded);
          this.beforeBytesDownloaded = bytesDownloaded;
        }
        yield return (object) new WaitForSeconds(0.1f);
      }
      progressCallback(this.www.bytesDownloaded - this.beforeBytesDownloaded);
      yield return (object) null;
      if (this.www.error != null)
      {
        this.error = this.www.error;
      }
      else
      {
        try
        {
          File.WriteAllBytes(this.path, this.www.bytes);
        }
        catch (Exception ex1)
        {
          try
          {
            File.Delete(this.path);
          }
          catch (Exception ex2)
          {
            this.error = string.Format("failed to delete file: {0}, exception: {1}", (object) this.path, (object) ex2);
          }
          this.error = string.Format("failed to write file: {0}, exception: {1}", (object) this.path, (object) ex1);
          yield break;
        }
        this.succeeded = this.error == null;
      }
    }

    public void Dispose()
    {
      if (this.www == null)
        return;
      this.www.Dispose();
    }
  }

  private class Data
  {
    public string Path;
    public string Name;
    public bool Exists;
    public bool AssetBundle;
    public int FileSize;
    public bool Uncompress;
    public bool Completed;
    public bool Downloading;

    public string Url
    {
      get => this.AssetBundle ? DLC.UrlBase + "ab/" + this.Name : DLC.UrlBase + "sa/" + this.Name;
    }

    public string StorePath => DLC.ResourceDirectory + this.Name;

    public string StoreTempPath => DLC.ResourceTempDirectory + this.Name;
  }

  private class Consumer : IDisposable
  {
    private bool interrupted;
    private Dictionary<string, DLC.Data> dataDic;
    private DLC.Data nowDownloadData;
    private int maxDownloadSize;
    private const int DEFAULT_MAXDOWNLOADSIZE = 5242880;
    private string error;
    private int downloaded;
    private int stored;

    public Consumer(Dictionary<string, DLC.Data> dataDic)
    {
      this.dataDic = dataDic;
      this.maxDownloadSize = this.dataDic.Any<KeyValuePair<string, DLC.Data>>() ? this.dataDic.Select<KeyValuePair<string, DLC.Data>, int>((Func<KeyValuePair<string, DLC.Data>, int>) (x => x.Value.FileSize)).Max() : 0;
      if (this.maxDownloadSize < 5242880)
        this.maxDownloadSize = 5242880;
      this.interrupted = false;
      this.nowDownloadData = (DLC.Data) null;
    }

    public void Dispose()
    {
    }

    private void RemoveCompletedData()
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, DLC.Data> keyValuePair in this.dataDic)
      {
        if (keyValuePair.Value.Completed)
          stringList.Add(keyValuePair.Key);
      }
      foreach (string key in stringList)
        this.dataDic.Remove(key);
    }

    private DLC.Data GetNext()
    {
      int num = 0;
      foreach (KeyValuePair<string, DLC.Data> keyValuePair in this.dataDic)
      {
        if (!keyValuePair.Value.Completed && keyValuePair.Value.Downloading)
          num += keyValuePair.Value.FileSize;
      }
      if (num >= this.maxDownloadSize)
        return (DLC.Data) null;
      foreach (KeyValuePair<string, DLC.Data> keyValuePair in this.dataDic)
      {
        if (!keyValuePair.Value.Completed && !keyValuePair.Value.Downloading)
          return keyValuePair.Value;
      }
      return (DLC.Data) null;
    }

    public string Error => this.error;

    public int Downloaded => this.downloaded;

    public int Stored => this.stored;

    public bool Completed => this.dataDic.Count == 0;

    public void Restart()
    {
      this.error = (string) null;
      this.interrupted = false;
      this.nowDownloadData = (DLC.Data) null;
    }

    public void Interrupt() => this.interrupted = true;

    private IEnumerator WWWRun_Loop_for_IsMoved(DLC.Data data)
    {
      DLC.Consumer consumer = this;
      int retry = 0;
      string writePath = DLC.GetPath(data.Name);
      ResourceManager.CreateSaveDLCDir(Path.GetDirectoryName(writePath));
      DLC.WWWFile www;
      while (true)
      {
        www = new DLC.WWWFile(data.Url, writePath);
        try
        {
          IEnumerator e = www.Wait(new Action<int>(consumer.addDownloadProgress));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (www.Error != null)
          {
            if (retry < 5)
            {
              ++retry;
              yield return (object) new WaitForSeconds(1f);
            }
            else
            {
              consumer.error = www.Error;
              data.Downloading = false;
              Debug.LogError((object) consumer.error);
              consumer.logForbiddenError(data.Url);
              yield break;
            }
          }
          else
            break;
        }
        finally
        {
          www?.Dispose();
        }
      }
      www = (DLC.WWWFile) null;
    }

    private IEnumerator WWWRun_Loop(DLC.Data data)
    {
      DLC.Consumer consumer = this;
      int retry = 0;
      DLC.WWWFile www;
      while (true)
      {
        www = new DLC.WWWFile(data.Url, data.StorePath);
        try
        {
          IEnumerator e = www.Wait(new Action<int>(consumer.addDownloadProgress));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (www.Error != null)
          {
            if (retry < 5)
            {
              ++retry;
              yield return (object) new WaitForSeconds(1f);
            }
            else
            {
              consumer.error = www.Error;
              data.Downloading = false;
              Debug.LogError((object) consumer.error);
              consumer.logForbiddenError(data.Url);
              yield break;
            }
          }
          else
            break;
        }
        finally
        {
          www?.Dispose();
        }
      }
      www = (DLC.WWWFile) null;
    }

    private IEnumerator Run_()
    {
      this.RemoveCompletedData();
      if (this.dataDic.Count % 100 == 0)
      {
        GC.Collect();
        GC.WaitForPendingFinalizers();
      }
      if (this.dataDic.Count != 0)
      {
        DLC.Data data = this.GetNext();
        this.nowDownloadData = data;
        if (data == null)
        {
          yield return (object) new WaitForSeconds(1f);
        }
        else
        {
          data.Downloading = true;
          this.error = (string) null;
          if (Persist.fileMoved.Data.isAllMoved)
            yield return (object) this.WWWRun_Loop_for_IsMoved(data);
          else
            yield return (object) this.WWWRun_Loop(data);
          if (this.error == null)
          {
            data.Downloading = false;
            data.Completed = true;
            this.stored += data.FileSize;
          }
        }
      }
    }

    public IEnumerator Run()
    {
      while (!this.Completed)
      {
        IEnumerator e = this.Run_();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        while (this.error != null && !this.Completed || this.interrupted && this.nowDownloadData != null && this.nowDownloadData.Completed)
          yield return (object) new WaitForSeconds(1f);
      }
    }

    private void addDownloadProgress(int count) => this.downloaded += count;

    private void logForbiddenError(string url)
    {
      if (!(this.error.Substring(0, 3) == "403"))
        return;
      List<int> intList = SA_Extensions_String.AllIndexesOf(url, "/", StringComparison.OrdinalIgnoreCase);
      if (intList.Count >= 2)
      {
        int startIndex = intList[intList.Count - 2] + 1;
        Debug.LogError((object) string.Format("Nothing assets file on s3: {0}", (object) url.Substring(startIndex)));
      }
      else
        Debug.LogError((object) string.Format("Does not download s3 assets file: {0}", (object) url));
    }
  }
}
