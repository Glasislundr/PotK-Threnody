// Decompiled with JetBrains decompiler
// Type: UniLinq.OrderedSequence`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace UniLinq
{
  internal class OrderedSequence<TElement, TKey> : OrderedEnumerable<TElement>
  {
    private OrderedEnumerable<TElement> parent;
    private Func<TElement, TKey> selector;
    private IComparer<TKey> comparer;
    private SortDirection direction;

    internal OrderedSequence(
      IEnumerable<TElement> source,
      Func<TElement, TKey> key_selector,
      IComparer<TKey> comparer,
      SortDirection direction)
      : base(source)
    {
      this.selector = key_selector;
      this.comparer = comparer ?? (IComparer<TKey>) Comparer<TKey>.Default;
      this.direction = direction;
    }

    internal OrderedSequence(
      OrderedEnumerable<TElement> parent,
      IEnumerable<TElement> source,
      Func<TElement, TKey> keySelector,
      IComparer<TKey> comparer,
      SortDirection direction)
      : this(source, keySelector, comparer, direction)
    {
      this.parent = parent;
    }

    public override IEnumerator<TElement> GetEnumerator() => base.GetEnumerator();

    public override SortContext<TElement> CreateContext(SortContext<TElement> current)
    {
      SortContext<TElement> current1 = (SortContext<TElement>) new SortSequenceContext<TElement, TKey>(this.selector, this.comparer, this.direction, current);
      return this.parent != null ? this.parent.CreateContext(current1) : current1;
    }

    protected override IEnumerable<TElement> Sort(IEnumerable<TElement> source)
    {
      return QuickSort<TElement>.Sort(source, this.CreateContext((SortContext<TElement>) null));
    }
  }
}
