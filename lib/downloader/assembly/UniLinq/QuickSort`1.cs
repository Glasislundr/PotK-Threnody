// Decompiled with JetBrains decompiler
// Type: UniLinq.QuickSort`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace UniLinq
{
  internal class QuickSort<TElement>
  {
    private TElement[] elements;
    private int[] indexes;
    private SortContext<TElement> context;

    private QuickSort(IEnumerable<TElement> source, SortContext<TElement> context)
    {
      List<TElement> elementList = new List<TElement>();
      foreach (TElement element in source)
        elementList.Add(element);
      this.elements = elementList.ToArray();
      this.indexes = QuickSort<TElement>.CreateIndexes(this.elements.Length);
      this.context = context;
    }

    private static int[] CreateIndexes(int length)
    {
      int[] indexes = new int[length];
      for (int index = 0; index < length; ++index)
        indexes[index] = index;
      return indexes;
    }

    private void PerformSort()
    {
      if (this.elements.Length <= 1)
        return;
      this.context.Initialize(this.elements);
      Array.Sort<int>(this.indexes, (IComparer<int>) this.context);
    }

    public static IEnumerable<TElement> Sort(
      IEnumerable<TElement> source,
      SortContext<TElement> context)
    {
      QuickSort<TElement> sorter = new QuickSort<TElement>(source, context);
      sorter.PerformSort();
      for (int i = 0; i < sorter.elements.Length; ++i)
        yield return sorter.elements[sorter.indexes[i]];
    }
  }
}
