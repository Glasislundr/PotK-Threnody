// Decompiled with JetBrains decompiler
// Type: UniLinq.SortContext`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace UniLinq
{
  internal abstract class SortContext<TElement> : IComparer<int>
  {
    protected SortDirection direction;
    protected SortContext<TElement> child_context;

    protected SortContext(SortDirection direction, SortContext<TElement> child_context)
    {
      this.direction = direction;
      this.child_context = child_context;
    }

    public abstract void Initialize(TElement[] elements);

    public abstract int Compare(int first_index, int second_index);
  }
}
