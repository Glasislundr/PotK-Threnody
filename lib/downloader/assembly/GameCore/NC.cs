// Decompiled with JetBrains decompiler
// Type: GameCore.NC
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
namespace GameCore
{
  public static class NC
  {
    private static GameGlobalVariable<Random> r = GameGlobalVariable<Random>.New();

    public static T RandomChoice<T>(T[] xs) => xs[NC.r.Get().Next(0, xs.Length)];

    public static T RandomChoice<T>(List<T> xs) => NC.RandomChoice<T>(xs.ToArray());

    public static int Lot(int[] weights, Random r)
    {
      int num1 = r.Next(0, ((IEnumerable<int>) weights).Sum() - 1);
      int num2 = 0;
      for (int index = 0; index < weights.Length; ++index)
      {
        num2 += weights[index];
        if (num2 > num1)
          return index;
      }
      return -1;
    }

    public static int Lot(int[] weights) => NC.Lot(weights, NC.r.Get());

    public static T Lot<T>(T[] xs, Func<T, int> weightFunc)
    {
      int[] weights = new int[xs.Length];
      for (int index = 0; index < weights.Length; ++index)
        weights[index] = weightFunc(xs[index]);
      return xs[NC.Lot(weights, NC.r.Get())];
    }

    public static float Clampf(float min, float max, float val)
    {
      return Mathf.Min(max, Mathf.Max(min, val));
    }

    public static int Clamp(int min, int max, int val) => Math.Min(max, Math.Max(min, val));

    public static bool IsReach(int min, int max, int val) => val >= min && val <= max;

    public static int Distance(int x1, int y1, int x2, int y2)
    {
      return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }
  }
}
