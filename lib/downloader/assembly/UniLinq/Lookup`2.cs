// Decompiled with JetBrains decompiler
// Type: UniLinq.Lookup`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace UniLinq
{
  public class Lookup<TKey, TElement> : 
    IEnumerable<IGrouping<TKey, TElement>>,
    IEnumerable,
    ILookup<TKey, TElement>
  {
    private IGrouping<TKey, TElement> nullGrouping;
    private Dictionary<TKey, IGrouping<TKey, TElement>> groups;

    public int Count => this.nullGrouping != null ? this.groups.Count + 1 : this.groups.Count;

    public IEnumerable<TElement> this[TKey key]
    {
      get
      {
        if ((object) key == null && this.nullGrouping != null)
          return (IEnumerable<TElement>) this.nullGrouping;
        IGrouping<TKey, TElement> grouping;
        return (object) key != null && this.groups.TryGetValue(key, out grouping) ? (IEnumerable<TElement>) grouping : (IEnumerable<TElement>) new TElement[0];
      }
    }

    internal Lookup(Dictionary<TKey, List<TElement>> lookup, IEnumerable<TElement> nullKeyElements)
    {
      this.groups = new Dictionary<TKey, IGrouping<TKey, TElement>>(lookup.Comparer);
      foreach (KeyValuePair<TKey, List<TElement>> keyValuePair in lookup)
        this.groups.Add(keyValuePair.Key, (IGrouping<TKey, TElement>) new Grouping<TKey, TElement>(keyValuePair.Key, (IEnumerable<TElement>) keyValuePair.Value));
      if (nullKeyElements == null)
        return;
      this.nullGrouping = (IGrouping<TKey, TElement>) new Grouping<TKey, TElement>(default (TKey), nullKeyElements);
    }

    public IEnumerable<TResult> ApplyResultSelector<TResult>(
      Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
      if (this.nullGrouping != null)
        yield return resultSelector(this.nullGrouping.Key, (IEnumerable<TElement>) this.nullGrouping);
      foreach (IGrouping<TKey, TElement> grouping in this.groups.Values)
        yield return resultSelector(grouping.Key, (IEnumerable<TElement>) grouping);
    }

    public bool Contains(TKey key)
    {
      return (object) key == null ? this.nullGrouping != null : this.groups.ContainsKey(key);
    }

    public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
    {
      if (this.nullGrouping != null)
        yield return this.nullGrouping;
      foreach (IGrouping<TKey, TElement> grouping in this.groups.Values)
        yield return grouping;
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
