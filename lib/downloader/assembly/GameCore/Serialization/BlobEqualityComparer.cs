// Decompiled with JetBrains decompiler
// Type: GameCore.Serialization.BlobEqualityComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace GameCore.Serialization
{
  internal class BlobEqualityComparer : IEqualityComparer<object>
  {
    bool IEqualityComparer<object>.Equals(object x, object y)
    {
      if (x == null)
        return y == null;
      return x.GetType().IsValueType ? x.Equals(y) : x == y;
    }

    int IEqualityComparer<object>.GetHashCode(object obj) => this.GetHashCode();
  }
}
