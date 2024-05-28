// Decompiled with JetBrains decompiler
// Type: System.Collections.IStructuralEquatable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
namespace System.Collections
{
  public interface IStructuralEquatable
  {
    bool Equals(object other, IEqualityComparer comparer);

    int GetHashCode(IEqualityComparer comparer);
  }
}
