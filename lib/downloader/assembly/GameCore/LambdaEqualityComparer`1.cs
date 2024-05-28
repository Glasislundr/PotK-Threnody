// Decompiled with JetBrains decompiler
// Type: GameCore.LambdaEqualityComparer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace GameCore
{
  public class LambdaEqualityComparer<T> : IEqualityComparer<T>
  {
    private readonly Func<T, T, bool> _lambdaComparer;
    private readonly Func<T, int> _lambdaHash;

    public LambdaEqualityComparer(Func<T, T, bool> lambdaComparer)
      : this(lambdaComparer, (Func<T, int>) (o => 0))
    {
    }

    public LambdaEqualityComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
    {
      if (lambdaComparer == null)
        throw new ArgumentNullException(nameof (lambdaComparer));
      if (lambdaHash == null)
        throw new ArgumentNullException(nameof (lambdaHash));
      this._lambdaComparer = lambdaComparer;
      this._lambdaHash = lambdaHash;
    }

    public bool Equals(T x, T y) => this._lambdaComparer(x, y);

    public int GetHashCode(T obj) => this._lambdaHash(obj);
  }
}
