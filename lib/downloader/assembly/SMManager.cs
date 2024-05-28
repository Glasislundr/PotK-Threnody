// Decompiled with JetBrains decompiler
// Type: SMManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SMManager
{
  private static GameGlobalVariable<Dictionary<System.Type, SMManager.Value>> dic = GameGlobalVariable<Dictionary<System.Type, SMManager.Value>>.New();
  private static GameGlobalVariable<Dictionary<System.Type, SMManager.Value>> backup = GameGlobalVariable<Dictionary<System.Type, SMManager.Value>>.New();

  private static void CopyDic<T>()
  {
    if (!SMManager.dic.Get().ContainsKey(typeof (T)))
      return;
    SMManager.Value obj1 = SMManager.dic.Get()[typeof (T)];
    if (SMManager.backup.Get().ContainsKey(typeof (T)))
    {
      SMManager.Value obj2 = SMManager.backup.Get()[typeof (T)];
    }
    else
      SMManager.backup.Get().Add(typeof (T), obj1);
  }

  public static void Swap()
  {
    SMManager.CopyDic<Player>();
    SMManager.CopyDic<NGGameDataManager.TimeCounter>();
    GameGlobalVariable<Dictionary<System.Type, SMManager.Value>> backup = SMManager.backup;
    SMManager.backup = SMManager.dic;
    SMManager.dic = backup;
  }

  public static Modified<T> Observe<T>() where T : class => new Modified<T>(0L);

  public static void Change<T>() where T : class => SMManager.Change<T>(SMManager.Get<T>());

  public static void Change<T>(T data) where T : class
  {
    if (!SMManager.Contains<T>())
    {
      SMManager.dic.Get().Add(typeof (T), new SMManager.Value()
      {
        revision = 1L,
        obj = (object) data
      });
    }
    else
    {
      SMManager.Value obj = SMManager.dic.Get()[typeof (T)];
      ++obj.revision;
      obj.obj = (object) data;
    }
  }

  public static void UpdateList<T>(T[] data, bool clear = false) where T : KeyCompare
  {
    if (!SMManager.Contains<T[]>())
    {
      SMManager.dic.Get().Add(typeof (T[]), new SMManager.Value()
      {
        revision = 1L,
        obj = (object) data
      });
    }
    else
    {
      SMManager.Value obj = SMManager.dic.Get()[typeof (T[])];
      bool flag = false;
      T[] objArray = obj.obj as T[];
      if (data.Length != 0)
        flag = data[0].hasKey;
      else if (objArray != null && objArray.Length != 0)
        flag = objArray[0].hasKey;
      if (flag && !clear)
      {
        Dictionary<object, T> dictionary = ((IEnumerable<T>) (obj.obj as T[])).ToDictionary<T, object>((Func<T, object>) (x => x.Key));
        for (int index = 0; index < data.Length; ++index)
        {
          if (!dictionary.ContainsKey(data[index].Key))
            dictionary.Add(data[index].Key, data[index]);
          else
            dictionary[data[index].Key] = data[index];
        }
        obj.obj = (object) dictionary.Values.ToArray<T>();
      }
      else
        obj.obj = (object) data;
      ++obj.revision;
    }
  }

  public static void DeleteList<T>(object[] ids) where T : KeyCompare
  {
    if (ids.Length == 0 || !SMManager.Contains<T[]>())
      return;
    SMManager.Value obj = SMManager.dic.Get()[typeof (T[])];
    if (!(obj.obj is T[] source) || source.Length == 0 || !source[0].hasKey)
      return;
    Dictionary<object, T> dictionary = ((IEnumerable<T>) source).ToDictionary<T, object>((Func<T, object>) (x => x.Key));
    for (int index = 0; index < ids.Length; ++index)
    {
      object id = ids[index];
      if (dictionary.ContainsKey(id))
        dictionary.Remove(id);
    }
    obj.obj = (object) dictionary.Values.ToArray<T>();
    ++obj.revision;
  }

  public static bool Contains<T>() where T : class => SMManager.dic.Get().ContainsKey(typeof (T));

  public static T Get<T>() where T : class
  {
    return !SMManager.Contains<T>() ? default (T) : (T) SMManager.dic.Get()[typeof (T)].obj;
  }

  public static long Revision<T>() where T : class
  {
    return !SMManager.Contains<T>() ? 0L : SMManager.dic.Get()[typeof (T)].revision;
  }

  private class Value
  {
    public long revision;
    public object obj;
  }
}
