// Decompiled with JetBrains decompiler
// Type: UniLinq.ILookup`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace UniLinq
{
  public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
  {
    int Count { get; }

    IEnumerable<TElement> this[TKey key] { get; }

    bool Contains(TKey key);
  }
}
