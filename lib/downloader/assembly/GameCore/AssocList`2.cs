// Decompiled with JetBrains decompiler
// Type: GameCore.AssocList`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore
{
  public class AssocList<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable,
    IDictionary,
    ICollection
  {
    private int count;
    private TKey[] keys;
    private TValue[] values;

    public AssocList()
      : this(4)
    {
    }

    public AssocList(int capacity)
    {
      this.keys = new TKey[capacity];
      this.values = new TValue[capacity];
      this.count = 0;
    }

    public AssocList(IDictionary<TKey, TValue> dictionary)
      : this(dictionary.Count)
    {
      dictionary.Keys.CopyTo(this.keys, 0);
      dictionary.Values.CopyTo(this.values, 0);
      Array.Sort<TKey, TValue>(this.keys, this.values);
      this.count = dictionary.Count;
    }

    public AssocList(IEnumerable<KeyValuePair<TKey, TValue>> source, int count, bool sorted)
      : this(count)
    {
      int index = 0;
      foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
      {
        this.keys[index] = keyValuePair.Key;
        this.values[index] = keyValuePair.Value;
        ++index;
      }
      this.count = count;
      if (sorted)
        return;
      Array.Sort<TKey, TValue>(this.keys, this.values);
    }

    public AssocList(TKey[] keys, TValue[] values, int count)
    {
      this.keys = keys;
      this.values = values;
      this.count = count;
    }

    public int Count => this.count;

    public int Capacity
    {
      get => this.keys.Length;
      set
      {
        if (value <= this.keys.Length)
          return;
        TKey[] destinationArray1 = new TKey[value];
        TValue[] destinationArray2 = new TValue[value];
        if (this.count > 0)
        {
          Array.Copy((Array) this.keys, 0, (Array) destinationArray1, 0, this.count);
          Array.Copy((Array) this.values, 0, (Array) destinationArray2, 0, this.count);
        }
        this.keys = destinationArray1;
        this.values = destinationArray2;
      }
    }

    public void Add(TKey key, TValue value)
    {
      int num = Array.BinarySearch<TKey>(this.keys, 0, this.count, key);
      if (num >= 0)
        throw new InvalidOperationException(string.Format("already inserted: {0}", (object) key));
      this.Insert(~num, key, value);
    }

    public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> kvs, int count)
    {
      this.EnsureCapacity(this.count + count);
      foreach (KeyValuePair<TKey, TValue> kv in kvs)
      {
        this.keys[this.count] = kv.Key;
        this.values[this.count] = kv.Value;
        ++this.count;
      }
      Array.Sort<TKey, TValue>(this.keys, this.values, 0, this.count);
    }

    public bool Remove(TKey key)
    {
      int index = this.IndexOf(key);
      if (index >= 0)
        this.RemoveAt(index);
      return index >= 0;
    }

    public void Clear()
    {
      Array.Clear((Array) this.keys, 0, this.count);
      Array.Clear((Array) this.values, 0, this.count);
      this.count = 0;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      int index = this.IndexOf(key);
      if (index >= 0)
      {
        value = this.values[index];
        return true;
      }
      value = default (TValue);
      return false;
    }

    public bool ContainsKey(TKey key) => this.IndexOf(key) >= 0;

    public TValue this[TKey key]
    {
      get
      {
        int index = this.IndexOf(key);
        return index >= 0 ? this.values[index] : throw new InvalidOperationException(string.Format("key not found: {0}", (object) key));
      }
      set
      {
        int index = this.IndexOf(key);
        if (index >= 0)
          this.values[index] = value;
        else
          this.Insert(~index, key, value);
      }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      for (int i = 0; i < this.count; ++i)
        yield return new KeyValuePair<TKey, TValue>(this.keys[i], this.values[i]);
    }

    public TKey[] Keys
    {
      get
      {
        TKey[] destinationArray = new TKey[this.count];
        Array.Copy((Array) this.keys, (Array) destinationArray, destinationArray.Length);
        return destinationArray;
      }
    }

    public TValue[] Values
    {
      get
      {
        TValue[] destinationArray = new TValue[this.count];
        Array.Copy((Array) this.values, (Array) destinationArray, destinationArray.Length);
        return destinationArray;
      }
    }

    public void RemoveWhere(Func<KeyValuePair<TKey, TValue>, bool> predicate)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < this.count; ++index2)
      {
        KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(this.keys[index2], this.values[index2]);
        if (!predicate(keyValuePair))
        {
          if (index1 != index2)
          {
            this.keys[index1] = this.keys[index2];
            this.values[index1] = this.values[index2];
          }
          ++index1;
        }
      }
      for (int index3 = index1; index3 < this.count; ++index3)
      {
        this.keys[index3] = default (TKey);
        this.values[index3] = default (TValue);
      }
      this.count = index1;
    }

    public override string ToString()
    {
      return "{" + Enumerable.Range(0, this.count).Select<int, string>((Func<int, string>) (i => "\"{0}\":{1}".F((object) this.keys[i], (object) this.values[i]))).Join(", ") + "}";
    }

    private void RemoveAt(int index)
    {
      --this.count;
      if (index < this.count)
      {
        Array.Copy((Array) this.keys, index + 1, (Array) this.keys, index, this.count - index);
        Array.Copy((Array) this.values, index + 1, (Array) this.values, index, this.count - index);
      }
      this.keys[this.count] = default (TKey);
      this.values[this.count] = default (TValue);
    }

    private int IndexOf(TKey key)
    {
      int num1 = 0;
      int num2 = this.count - 1;
      while (num1 <= num2)
      {
        int index = num1 + (num2 - num1 >> 1);
        int num3 = Comparer<TKey>.Default.Compare(this.keys[index], key);
        if (num3 == 0)
          return index;
        if (num3 < 0)
          num1 = index + 1;
        else
          num2 = index - 1;
      }
      return ~num1;
    }

    private void Insert(int index, TKey key, TValue value)
    {
      this.EnsureCapacity(this.count + 1);
      if (index < this.count)
      {
        Array.Copy((Array) this.keys, index, (Array) this.keys, index + 1, this.count - index);
        Array.Copy((Array) this.values, index, (Array) this.values, index + 1, this.count - index);
      }
      this.keys[index] = key;
      this.values[index] = value;
      ++this.count;
    }

    private void EnsureCapacity(int min)
    {
      if (min <= this.keys.Length)
        return;
      int num = this.keys.Length == 0 ? 4 : this.keys.Length * 2;
      while (num < min)
        num *= 2;
      this.Capacity = num;
    }

    private void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      for (int index = 0; index < this.count; ++index)
        array[arrayIndex + index] = new KeyValuePair<TKey, TValue>(this.keys[index], this.values[index]);
    }

    bool IDictionary.IsReadOnly => false;

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

    bool IDictionary.IsFixedSize => false;

    int ICollection<KeyValuePair<TKey, TValue>>.Count => this.Count;

    int ICollection.Count => this.Count;

    object ICollection.SyncRoot => (object) null;

    bool ICollection.IsSynchronized => false;

    void IDictionary.Add(object key, object value) => this.Add((TKey) key, (TValue) value);

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => this.Add(key, value);

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
      this.Add(item.Key, item.Value);
    }

    bool IDictionary<TKey, TValue>.Remove(TKey key) => this.Remove(key);

    void IDictionary.Remove(object key) => this.Remove((TKey) key);

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
      TValue y;
      if (!this.TryGetValue(item.Key, out y) || !EqualityComparer<TValue>.Default.Equals(item.Value, y))
        return false;
      this.Remove(item.Key);
      return true;
    }

    bool IDictionary.Contains(object key) => this.ContainsKey((TKey) key);

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
      int index = this.IndexOf(item.Key);
      return index >= 0 && EqualityComparer<TValue>.Default.Equals(item.Value, this.values[index]);
    }

    bool IDictionary<TKey, TValue>.ContainsKey(TKey key) => this.ContainsKey(key);

    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
    {
      return this.TryGetValue(key, out value);
    }

    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
      get => this[key];
      set => this[key] = value;
    }

    object IDictionary.this[object key]
    {
      get => (object) this[(TKey) key];
      set => this[(TKey) key] = (TValue) value;
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => (ICollection<TKey>) this.Keys;

    ICollection IDictionary.Keys => (ICollection) this.Keys;

    ICollection<TValue> IDictionary<TKey, TValue>.Values => (ICollection<TValue>) this.Values;

    ICollection IDictionary.Values => (ICollection) this.Values;

    void IDictionary.Clear() => this.Clear();

    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => this.Clear();

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(
      KeyValuePair<TKey, TValue>[] array,
      int arrayIndex)
    {
      this.CopyTo(array, arrayIndex);
    }

    void ICollection.CopyTo(Array array, int index)
    {
      this.CopyTo((KeyValuePair<TKey, TValue>[]) array, index);
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
      return (IDictionaryEnumerator) new AssocList<TKey, TValue>.DictEnum(this.GetEnumerator());
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private class DictEnum : IDictionaryEnumerator, IEnumerator
    {
      private IEnumerator<KeyValuePair<TKey, TValue>> e;

      public DictEnum(IEnumerator<KeyValuePair<TKey, TValue>> e) => this.e = e;

      public bool MoveNext() => this.e.MoveNext();

      public void Reset() => this.e.Reset();

      public object Current => (object) this.Entry;

      public object Key => (object) this.e.Current.Key;

      public object Value => (object) this.e.Current.Value;

      public DictionaryEntry Entry
      {
        get
        {
          KeyValuePair<TKey, TValue> current = this.e.Current;
          __Boxed<TKey> key = (object) current.Key;
          current = this.e.Current;
          __Boxed<TValue> local = (object) current.Value;
          return new DictionaryEntry((object) key, (object) local);
        }
      }
    }
  }
}
