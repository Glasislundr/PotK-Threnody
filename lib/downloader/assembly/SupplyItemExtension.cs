// Decompiled with JetBrains decompiler
// Type: SupplyItemExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class SupplyItemExtension
{
  public static List<SupplyItem> DeckList(this List<SupplyItem> xs)
  {
    return xs.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.IsShowDeck)).ToList<SupplyItem>();
  }

  public static void Fill(this List<SupplyItem> xs)
  {
    foreach (SupplyItem supplyItem in xs.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.DeckIndex > 0)))
    {
      int num = xs.TotalQuantity(supplyItem.Supply.ID);
      if (num > supplyItem.Supply.battle_stack_limit)
        num = supplyItem.Supply.battle_stack_limit;
      supplyItem.SelectCount = num;
    }
  }

  public static bool RemoveDeck(this List<SupplyItem> xs, int select)
  {
    bool flag = false;
    SupplyItem target = xs.FindByIndex(select);
    if (target != null)
    {
      for (int index = 0; index < xs.Count; ++index)
      {
        foreach (SupplyItem supplyItem in xs.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.Supply.ID == target.Supply.ID)))
        {
          supplyItem.DeckIndex = 0;
          supplyItem.SelectCount = 0;
          flag = true;
        }
      }
    }
    return flag;
  }

  public static void RemoveAll(this List<SupplyItem> xs)
  {
    foreach (SupplyItem supplyItem in xs)
    {
      supplyItem.DeckIndex = 0;
      supplyItem.SelectCount = 0;
    }
  }

  public static SupplyItem FindByIndex(this List<SupplyItem> xs, int index)
  {
    using (IEnumerator<SupplyItem> enumerator = xs.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.DeckIndex == index)).GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        SupplyItem current = enumerator.Current;
        Debug.Log((object) current.DeckIndex);
        return current;
      }
    }
    return (SupplyItem) null;
  }

  public static int TotalSelectCount(this List<SupplyItem> xs, int entityId)
  {
    IEnumerable<SupplyItem> supplyItems = xs.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.Supply.ID == entityId));
    int num = 0;
    foreach (SupplyItem supplyItem in supplyItems)
      num += supplyItem.SelectCount;
    return num;
  }

  public static int TotalQuantity(this List<SupplyItem> xs, int entityId)
  {
    IEnumerable<SupplyItem> supplyItems = xs.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.Supply.ID == entityId));
    int num = 0;
    foreach (SupplyItem supplyItem in supplyItems)
      num += supplyItem.ItemQuantity;
    return num;
  }

  public static List<SupplyItem> Copy(this List<SupplyItem> xs)
  {
    return xs.Select<SupplyItem, SupplyItem>((Func<SupplyItem, SupplyItem>) (x => x.Copy())).ToList<SupplyItem>();
  }

  public static SupplyItem FindByEntityId(this List<SupplyItem> xs, int entityId)
  {
    foreach (SupplyItem byEntityId in xs)
    {
      if (byEntityId.Supply.ID == entityId)
        return byEntityId;
    }
    Debug.LogError((object) ("NOT FOUND ENTITY ID:" + (object) entityId));
    return (SupplyItem) null;
  }

  public static SupplyItem[] ShowSupplyItems(this List<SupplyItem> xs)
  {
    return xs.SelectMany<SupplyItem, SupplyItem>((Func<SupplyItem, IEnumerable<SupplyItem>>) (x => (IEnumerable<SupplyItem>) x.ShowSupplyItems())).ToArray<SupplyItem>();
  }
}
