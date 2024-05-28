// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.IntArrayEqualityComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore.Serialization
{
  internal class IntArrayEqualityComparer : IEqualityComparer<int[]>
  {
    bool IEqualityComparer<int[]>.Equals(int[] x, int[] y)
    {
      if (x == null && y == null)
        return true;
      return x != null && y != null && ((IEnumerable<int>) x).SequenceEqual<int>((IEnumerable<int>) y);
    }

    int IEqualityComparer<int[]>.GetHashCode(int[] x)
    {
      int a = x.Length;
      int num = Math.Min(x.Length, 10);
      for (int index = 0; index < num; ++index)
        a = a.Combine(x[index]);
      return a;
    }
  }
}
