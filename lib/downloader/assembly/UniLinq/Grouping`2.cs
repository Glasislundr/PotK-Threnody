// Decompiled with JetBrains decompiler
// Type: UniLinq.Grouping`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace UniLinq
{
  internal class Grouping<K, T> : IGrouping<K, T>, IEnumerable<T>, IEnumerable
  {
    private K key;
    private IEnumerable<T> group;

    public Grouping(K key, IEnumerable<T> group)
    {
      this.group = group;
      this.key = key;
    }

    public K Key
    {
      get => this.key;
      set => this.key = value;
    }

    public IEnumerator<T> GetEnumerator() => this.group.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.group.GetEnumerator();
  }
}
