// Decompiled with JetBrains decompiler
// Type: UniLinq.Check
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;

#nullable disable
namespace UniLinq
{
  internal static class Check
  {
    public static void Source(object source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
    }

    public static void Source1AndSource2(object source1, object source2)
    {
      if (source1 == null)
        throw new ArgumentNullException(nameof (source1));
      if (source2 == null)
        throw new ArgumentNullException(nameof (source2));
    }

    public static void SourceAndFuncAndSelector(object source, object func, object selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (func == null)
        throw new ArgumentNullException(nameof (func));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
    }

    public static void SourceAndFunc(object source, object func)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (func == null)
        throw new ArgumentNullException(nameof (func));
    }

    public static void SourceAndSelector(object source, object selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
    }

    public static void SourceAndPredicate(object source, object predicate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (predicate == null)
        throw new ArgumentNullException(nameof (predicate));
    }

    public static void FirstAndSecond(object first, object second)
    {
      if (first == null)
        throw new ArgumentNullException(nameof (first));
      if (second == null)
        throw new ArgumentNullException(nameof (second));
    }

    public static void SourceAndKeySelector(object source, object keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
    }

    public static void SourceAndKeyElementSelectors(
      object source,
      object keySelector,
      object elementSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
    }

    public static void SourceAndKeyResultSelectors(
      object source,
      object keySelector,
      object resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
    }

    public static void SourceAndCollectionSelectorAndResultSelector(
      object source,
      object collectionSelector,
      object resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (collectionSelector == null)
        throw new ArgumentNullException(nameof (collectionSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
    }

    public static void SourceAndCollectionSelectors(
      object source,
      object collectionSelector,
      object selector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (collectionSelector == null)
        throw new ArgumentNullException(nameof (collectionSelector));
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
    }

    public static void JoinSelectors(
      object outer,
      object inner,
      object outerKeySelector,
      object innerKeySelector,
      object resultSelector)
    {
      if (outer == null)
        throw new ArgumentNullException(nameof (outer));
      if (inner == null)
        throw new ArgumentNullException(nameof (inner));
      if (outerKeySelector == null)
        throw new ArgumentNullException(nameof (outerKeySelector));
      if (innerKeySelector == null)
        throw new ArgumentNullException(nameof (innerKeySelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
    }

    public static void GroupBySelectors(
      object source,
      object keySelector,
      object elementSelector,
      object resultSelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (keySelector == null)
        throw new ArgumentNullException(nameof (keySelector));
      if (elementSelector == null)
        throw new ArgumentNullException(nameof (elementSelector));
      if (resultSelector == null)
        throw new ArgumentNullException(nameof (resultSelector));
    }
  }
}
