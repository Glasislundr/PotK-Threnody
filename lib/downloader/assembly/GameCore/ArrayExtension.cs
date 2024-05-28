// Decompiled with JetBrains decompiler
// Type: GameCore.ArrayExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore
{
  public static class ArrayExtension
  {
    public static bool IsNullOrEmpty<T>(this T[] self) => self == null || self.Length == 0;

    public static bool IsNullOrLess<T>(this T[] self, int num) => self == null || self.Length < num;

    public static bool IsNullOrEmpty<T>(this List<T> self) => self == null || !self.Any<T>();
  }
}
