// Decompiled with JetBrains decompiler
// Type: Gsc.Network.Data.EntityList`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Gsc.Network.Data
{
  public class EntityList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : Entity<T>
  {
    private readonly SortedList<string, T> entityList;

    public bool IsReadOnly => true;

    public int Count => this.entityList.Count;

    public EntityList(SortedList<string, T> arg) => this.entityList = arg;

    public bool ContainsKey(string key) => this.entityList.ContainsKey(key);

    public bool Contains(T value) => this.ContainsKey(value.pk);

    public bool TryGetValue(string key, out T value)
    {
      if (!this.entityList.TryGetValue(key, out value))
        return false;
      value = EntityList<T>.CloneEntity(value);
      return true;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException();
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException();
      if (array.Length + arrayIndex < this.entityList.Count)
        throw new ArithmeticException();
      IList<T> values = this.entityList.Values;
      for (int index = values.Count - 1; index >= 0; --index)
        array[index + arrayIndex] = EntityList<T>.CloneEntity(values[index]);
    }

    public int IndexOf(T value) => this.entityList.IndexOfKey(value.pk);

    public IEnumerator<T> GetEnumerator()
    {
      IList<T> values = this.entityList.Values;
      for (int i = 0; i < values.Count; ++i)
        yield return EntityList<T>.CloneEntity(values[i]);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public T this[int index]
    {
      get => EntityList<T>.CloneEntity(this.entityList.Values[index]);
      set => throw new NotSupportedException();
    }

    private static T CloneEntity(T entity)
    {
      T obj = entity.Clone();
      obj.ResolveRefs();
      return obj;
    }

    [Obsolete("Not Supported", true)]
    public void Insert(int index, T value)
    {
    }

    [Obsolete("Not Supported", true)]
    public void RemoveAt(int index)
    {
    }

    [Obsolete("Not Supported", true)]
    public void Add(T value)
    {
    }

    [Obsolete("Not Supported", true)]
    public void Clear()
    {
    }

    [Obsolete("Not Supported", true)]
    public bool Remove(T value) => false;
  }
}
