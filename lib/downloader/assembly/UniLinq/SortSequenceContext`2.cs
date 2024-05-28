// Decompiled with JetBrains decompiler
// Type: UniLinq.SortSequenceContext`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace UniLinq
{
  internal class SortSequenceContext<TElement, TKey> : SortContext<TElement>
  {
    private Func<TElement, TKey> selector;
    private IComparer<TKey> comparer;
    private TKey[] keys;

    public SortSequenceContext(
      Func<TElement, TKey> selector,
      IComparer<TKey> comparer,
      SortDirection direction,
      SortContext<TElement> child_context)
      : base(direction, child_context)
    {
      this.selector = selector;
      this.comparer = comparer;
    }

    public override void Initialize(TElement[] elements)
    {
      if (this.child_context != null)
        this.child_context.Initialize(elements);
      this.keys = new TKey[elements.Length];
      for (int index = 0; index < this.keys.Length; ++index)
        this.keys[index] = this.selector(elements[index]);
    }

    public override int Compare(int first_index, int second_index)
    {
      int num = this.comparer.Compare(this.keys[first_index], this.keys[second_index]);
      if (num == 0)
      {
        if (this.child_context != null)
          return this.child_context.Compare(first_index, second_index);
        num = this.direction == SortDirection.Descending ? second_index - first_index : first_index - second_index;
      }
      return this.direction != SortDirection.Descending ? num : -num;
    }
  }
}
