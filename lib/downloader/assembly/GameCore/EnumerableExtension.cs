// Decompiled with JetBrains decompiler
// Type: GameCore.EnumerableExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace GameCore
{
  public static class EnumerableExtension
  {
    public static void ForEach<T>(this IEnumerable<T> self, Action<T> f)
    {
      foreach (T obj in self)
        f(obj);
    }

    public static void ForEachIndex<T>(this IEnumerable<T> self, Action<T, int> f)
    {
      int num = 0;
      foreach (T obj in self)
      {
        f(obj, num);
        checked { ++num; }
      }
    }

    public static void ForEach<T1, T2>(
      this IEnumerable<T1> source1,
      IEnumerable<T2> source2,
      Action<T1, T2> f)
    {
      using (IEnumerator<T1> enumerator1 = EnumerableWrapper.Create<T1>(source1).GetEnumerator())
      {
        using (IEnumerator<T2> enumerator2 = EnumerableWrapper.Create<T2>(source2).GetEnumerator())
        {
          while (enumerator1.MoveNext() && enumerator2.MoveNext())
            f(enumerator1.Current, enumerator2.Current);
        }
      }
    }

    public static void ForEach<T1, T2, T3>(
      this IEnumerable<T1> source1,
      IEnumerable<T2> source2,
      IEnumerable<T3> source3,
      Action<T1, T2, T3> f)
    {
      using (IEnumerator<T1> enumerator1 = EnumerableWrapper.Create<T1>(source1).GetEnumerator())
      {
        using (IEnumerator<T2> enumerator2 = EnumerableWrapper.Create<T2>(source2).GetEnumerator())
        {
          using (IEnumerator<T3> enumerator3 = EnumerableWrapper.Create<T3>(source3).GetEnumerator())
          {
            while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext())
              f(enumerator1.Current, enumerator2.Current, enumerator3.Current);
          }
        }
      }
    }

    public static string Join(this IEnumerable<string> self, string separator)
    {
      return string.Join(separator, self.ToArray<string>());
    }

    public static IEnumerable<TResult> Select<T1, T2, TResult>(
      this IEnumerable<T1> source1,
      IEnumerable<T2> source2,
      Func<T1, T2, TResult> func)
    {
      using (IEnumerator<T1> enumerator1 = EnumerableWrapper.Create<T1>(source1).GetEnumerator())
      {
        using (IEnumerator<T2> enumerator2 = EnumerableWrapper.Create<T2>(source2).GetEnumerator())
        {
          while (enumerator1.MoveNext() && enumerator2.MoveNext())
            yield return func(enumerator1.Current, enumerator2.Current);
        }
      }
    }

    public static string ToStringForChars(this IEnumerable<char> str)
    {
      return new string(str.ToArray<char>());
    }

    public static int? FirstIndexOrNull<T>(this IEnumerable<T> self, Func<T, bool> pred)
    {
      int num = 0;
      foreach (T obj in self)
      {
        if (pred(obj))
          return new int?(num);
        checked { ++num; }
      }
      return new int?();
    }

    public static IEnumerator WaitAll(this IEnumerable<IEnumerator> self)
    {
      for (IEnumerator[] array = self.Where<IEnumerator>((Func<IEnumerator, bool>) (e => e.MoveNext())).ToArray<IEnumerator>(); array.Length != 0; array = ((IEnumerable<IEnumerator>) array).Where<IEnumerator>((Func<IEnumerator, bool>) (e => e.MoveNext())).ToArray<IEnumerator>())
      {
        IEnumerator[] enumeratorArray = array;
        for (int index = 0; index < enumeratorArray.Length; ++index)
          yield return enumeratorArray[index].Current;
        enumeratorArray = (IEnumerator[]) null;
      }
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self)
    {
      return self.Shuffle<T>(new Random());
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self, Random random)
    {
      IList<T> list = self as IList<T>;
      T[] ar = self as T[];
      T[] xs = ar != null || ar != null ? (T[]) null : self.ToArray<T>();
      int[] ns = new int[list != null ? list.Count : (ar != null ? ar.Length : xs.Length)];
      for (int index = 0; index < ns.Length; ++index)
        ns[index] = index;
      int length = ns.Length;
      while (length > 1)
      {
        --length;
        int index = random.Next(length + 1);
        int num = ns[index];
        ns[index] = ns[length];
        ns[length] = num;
      }
      int i;
      if (list != null)
      {
        for (i = 0; i < ns.Length; ++i)
          yield return list[ns[i]];
      }
      else if (ar != null)
      {
        for (i = 0; i < ns.Length; ++i)
          yield return ar[ns[i]];
      }
      else
      {
        for (i = 0; i < ns.Length; ++i)
          yield return xs[ns[i]];
      }
    }
  }
}
