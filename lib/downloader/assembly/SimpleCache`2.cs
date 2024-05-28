// Decompiled with JetBrains decompiler
// Type: SimpleCache`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SimpleCache<Key, Value> where Value : Object
{
  private Dictionary<Key, WeakReference> cacheDic = new Dictionary<Key, WeakReference>();
  private Func<Key, Promise<Value>, IEnumerator> loader;

  public SimpleCache(
    Func<Key, Promise<Value>, IEnumerator> loader,
    Func<Value, long> getSize,
    long maxSize,
    Action<Key, Value> unload = null)
  {
    this.loader = loader;
  }

  private Value GetTarget(Key key)
  {
    return !this.cacheDic.ContainsKey(key) ? default (Value) : this.cacheDic[key].Target as Value;
  }

  private IEnumerator Run(Key key, Promise<Value> promise)
  {
    IEnumerator e = this.loader(key, promise);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Value target = this.GetTarget(key);
    if (Object.op_Inequality((Object) target, (Object) null))
      promise.Result = target;
    else
      this.SetValue(key, promise.Result);
  }

  public void SetValue(Key key, Value value)
  {
    if (!this.cacheDic.ContainsKey(key))
      this.cacheDic.Add(key, new WeakReference((object) value));
    else
      this.cacheDic[key].Target = (object) value;
  }

  public Value TryGet(Key key) => this.GetTarget(key);

  public Future<Value> Get(Key key)
  {
    Value target = this.GetTarget(key);
    return Object.op_Equality((Object) target, (Object) null) ? new Future<Value>((Func<Promise<Value>, IEnumerator>) (promise => this.Run(key, promise))) : Future.Single<Value>(target);
  }

  public void Clear()
  {
    HashSet<Key> keySet = new HashSet<Key>((IEnumerable<Key>) this.cacheDic.Keys);
    Dictionary<Key, WeakReference> dictionary = new Dictionary<Key, WeakReference>();
    foreach (KeyValuePair<Key, WeakReference> keyValuePair in this.cacheDic)
    {
      if (keyValuePair.Value.Target != null)
        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
    }
    this.cacheDic = dictionary;
    keySet.ExceptWith((IEnumerable<Key>) this.cacheDic.Keys);
  }

  private class Wrap
  {
    public Value value;

    public Wrap(Value value) => this.value = value;
  }
}
