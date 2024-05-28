// Decompiled with JetBrains decompiler
// Type: MasterDataCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public static class MasterDataCache
{
  private const int TARGET_SIZE = 20480;
  private const int THREASHOLD = 20971520;
  private static Dictionary<string, MasterDataCache.DataBase> cache = new Dictionary<string, MasterDataCache.DataBase>();
  private static long counter = 0;
  private static MasterDataCache.GameMode currentMode = MasterDataCache.GameMode.HEAVEN;

  public static void CacheClear()
  {
    MasterDataCache.cache.Clear();
    BattleLandform.CacheClear();
    MasterDataCache.currentMode = MasterDataCache.GameMode.HEAVEN;
  }

  public static void SetGameMode(MasterDataCache.GameMode mode)
  {
    MasterDataCache.cache.Clear();
    BattleLandform.CacheClear();
    MasterDataCache.currentMode = mode;
  }

  private static IEnumerable<string> CleanTargets()
  {
    foreach (string key in MasterDataCache.cache.Keys)
    {
      if (MasterDataCache.cache[key].Size > 20480 && !MasterDataCache.cache[key].Locked)
        yield return key;
    }
  }

  private static void Clean()
  {
    HashSet<string> source = new HashSet<string>(MasterDataCache.CleanTargets());
    int num1 = source.Sum<string>((Func<string, int>) (x => MasterDataCache.cache[x].Size));
    while (num1 > 20971520)
    {
      long num2 = long.MaxValue;
      string key1 = (string) null;
      foreach (string key2 in source)
      {
        if (num2 > MasterDataCache.cache[key2].Counter)
        {
          num2 = MasterDataCache.cache[key2].Counter;
          key1 = key2;
        }
      }
      num1 -= MasterDataCache.cache[key1].Size;
      MasterDataCache.cache.Remove(key1);
      source.Remove(key1);
    }
  }

  private static IEnumerable<KeyValuePair<K, T>> AssocListSelect<K, T>(T[] list, Func<T, K> getID)
  {
    T[] objArray = list;
    for (int index = 0; index < objArray.Length; ++index)
    {
      T obj = objArray[index];
      yield return new KeyValuePair<K, T>(getID(obj), obj);
    }
    objArray = (T[]) null;
  }

  private static void Load<K, T>(
    string key,
    string fileName,
    Func<MasterDataReader, T> create,
    Func<T, K> getID,
    bool locked)
  {
    MasterDataCache.Clean();
    string empty = string.Empty;
    string path;
    if (MasterDataCache.currentMode == MasterDataCache.GameMode.EARTH)
    {
      path = "MasterData/p0/{0}".F((object) fileName);
      if (!Singleton<ResourceManager>.GetInstance().Contains(path))
        path = "MasterData/{0}".F((object) fileName);
    }
    else
      path = "MasterData/{0}".F((object) fileName);
    TextAsset textAsset = Singleton<ResourceManager>.GetInstance().LoadImmediatelyForSmallObject<TextAsset>(path);
    if (Object.op_Equality((Object) textAsset, (Object) null))
      return;
    byte[] bytes = textAsset.bytes;
    MasterDataReader masterDataReader = new MasterDataReader(bytes);
    T[] list = new T[masterDataReader.Length];
    for (int index = 0; index < masterDataReader.Length; ++index)
      list[index] = create(masterDataReader);
    AssocList<K, T> assocList = new AssocList<K, T>(MasterDataCache.AssocListSelect<K, T>(list, getID), masterDataReader.Length, false);
    MasterDataCache.Data<K, T> data1 = new MasterDataCache.Data<K, T>();
    data1.Dict = assocList;
    data1.List = list;
    data1.Size = bytes.Length;
    data1.Locked = locked;
    data1.LoadedFileName = fileName;
    data1.Counter = MasterDataCache.counter++;
    MasterDataCache.Data<K, T> data2 = data1;
    MasterDataCache.cache[key] = (MasterDataCache.DataBase) data2;
  }

  private static IEnumerator LoadFromDownloadOrCache<K, T>(
    string key,
    string fileName,
    Func<MasterDataReader, T> create,
    Func<T, K> getID,
    bool locked)
  {
    MasterDataCache.Clean();
    string empty = string.Empty;
    string path;
    if (MasterDataCache.currentMode == MasterDataCache.GameMode.EARTH)
    {
      path = "MasterData/p0/{0}".F((object) fileName);
      if (!Singleton<ResourceManager>.GetInstance().Contains(path))
        path = "MasterData/{0}".F((object) fileName);
    }
    else
      path = "MasterData/{0}".F((object) fileName);
    Future<TextAsset> text = Singleton<ResourceManager>.GetInstance().LoadFromDownloadOrCache<TextAsset>(path);
    if (text != null)
    {
      IEnumerator e = text.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      byte[] bytes = text.Result.bytes;
      MasterDataReader masterDataReader = new MasterDataReader(bytes);
      T[] list = new T[masterDataReader.Length];
      for (int index = 0; index < masterDataReader.Length; ++index)
        list[index] = create(masterDataReader);
      AssocList<K, T> assocList = new AssocList<K, T>(MasterDataCache.AssocListSelect<K, T>(list, getID), masterDataReader.Length, false);
      MasterDataCache.Data<K, T> data1 = new MasterDataCache.Data<K, T>();
      data1.Dict = assocList;
      data1.List = list;
      data1.Size = bytes.Length;
      data1.Locked = locked;
      data1.LoadedFileName = fileName;
      data1.Counter = MasterDataCache.counter++;
      MasterDataCache.Data<K, T> data2 = data1;
      MasterDataCache.cache[key] = (MasterDataCache.DataBase) data2;
    }
  }

  private static MasterDataCache.Data<K, T> GetOrCreate<K, T>(
    string key,
    Func<MasterDataReader, T> create,
    Func<T, K> getID)
  {
    MasterDataCache.DataBase dataBase;
    if (MasterDataCache.cache.TryGetValue(key, out dataBase))
    {
      MasterDataCache.Data<K, T> data = (MasterDataCache.Data<K, T>) dataBase;
      data.Counter = MasterDataCache.counter++;
      return data;
    }
    MasterDataCache.Load<K, T>(key, key, create, getID, false);
    return MasterDataCache.cache.TryGetValue(key, out dataBase) ? (MasterDataCache.Data<K, T>) dataBase : (MasterDataCache.Data<K, T>) null;
  }

  public static AssocList<K, T> Get<K, T>(
    string key,
    Func<MasterDataReader, T> create,
    Func<T, K> getID)
  {
    return MasterDataCache.GetOrCreate<K, T>(key, create, getID)?.Dict;
  }

  public static T[] GetList<K, T>(string key, Func<MasterDataReader, T> create, Func<T, K> getID)
  {
    return MasterDataCache.GetOrCreate<K, T>(key, create, getID)?.List;
  }

  public static IEnumerator LoadPartial<K, T>(
    string key,
    string fileName,
    Func<MasterDataReader, T> create,
    Func<T, K> getID)
  {
    if (!MasterDataCache.cache.ContainsKey(key) || !(MasterDataCache.cache[key].LoadedFileName == fileName))
    {
      IEnumerator e = MasterDataCache.LoadFromDownloadOrCache<K, T>(key, fileName, create, getID, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public static T Unique<K, T, M>(
    string key,
    string name,
    M m,
    Func<T, M> getKey,
    Func<MasterDataReader, T> create,
    Func<T, K> getID)
  {
    MasterDataCache.Data<K, T> data = MasterDataCache.GetOrCreate<K, T>(key, create, getID);
    if (data == null)
      return default (T);
    if (data.Unique == null)
      data.Unique = new AssocList<string, object>();
    object obj1;
    if (data.Unique.TryGetValue(name, out obj1))
    {
      T obj2;
      return !((AssocList<M, T>) obj1).TryGetValue(m, out obj2) ? default (T) : obj2;
    }
    AssocList<M, T> assocList = new AssocList<M, T>(MasterDataCache.AssocListSelect<M, T>(data.List, getKey), data.List.Length, false);
    data.Unique[name] = (object) assocList;
    T obj3;
    return !assocList.TryGetValue(m, out obj3) ? default (T) : obj3;
  }

  private static IEnumerable<KeyValuePair<M, T[]>> Select<M, T>(Dictionary<M, List<T>> d)
  {
    foreach (KeyValuePair<M, List<T>> keyValuePair in d)
      yield return new KeyValuePair<M, T[]>(keyValuePair.Key, keyValuePair.Value.ToArray());
  }

  public static T[] Where<K, T, M>(
    string key,
    string name,
    M m,
    Func<T, M> getKey,
    Func<MasterDataReader, T> create,
    Func<T, K> getID)
  {
    MasterDataCache.Data<K, T> data = MasterDataCache.GetOrCreate<K, T>(key, create, getID);
    if (data == null)
      return (T[]) null;
    if (data.Where == null)
      data.Where = new AssocList<string, object>();
    object obj1;
    if (data.Where.TryGetValue(name, out obj1))
    {
      T[] objArray;
      return !((AssocList<M, T[]>) obj1).TryGetValue(m, out objArray) ? new T[0] : objArray;
    }
    Dictionary<M, List<T>> d = new Dictionary<M, List<T>>();
    foreach (T obj2 in data.List)
    {
      M key1 = getKey(obj2);
      if (!d.ContainsKey(key1))
        d.Add(key1, new List<T>());
      d[key1].Add(obj2);
    }
    AssocList<M, T[]> assocList = new AssocList<M, T[]>(MasterDataCache.Select<M, T>(d), d.Count, false);
    data.Where[name] = (object) assocList;
    T[] objArray1;
    return !assocList.TryGetValue(m, out objArray1) ? new T[0] : objArray1;
  }

  public static void Unload(string key)
  {
    if (!MasterDataCache.cache.ContainsKey(key))
      return;
    MasterDataCache.cache.Remove(key);
  }

  public enum GameMode
  {
    HEAVEN = 1,
    EARTH = 2,
  }

  private class DataBase
  {
    public int Size;
    public long Counter;
    public string LoadedFileName;
    public bool Locked;
    public AssocList<string, object> Unique;
    public AssocList<string, object> Where;
  }

  private class Data<K, T> : MasterDataCache.DataBase
  {
    public AssocList<K, T> Dict;
    public T[] List;
  }
}
