// Decompiled with JetBrains decompiler
// Type: SortInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections.Generic;

#nullable disable
public class SortInfo
{
  public UnitSortAndFilter.SORT_TYPES sortType = UnitSortAndFilter.SORT_TYPES.BranchOfAnArmy;
  public SortAndFilter.SORT_TYPE_ORDER_BUY orderType;
  public bool isBattleFirst;
  public bool isTowerEntry = true;
  public bool[] filters;
  public Dictionary<UnitGroupHead, List<int>> groupIDs;

  public SortInfo(
    UnitSortAndFilter.SORT_TYPES sType,
    SortAndFilter.SORT_TYPE_ORDER_BUY oType,
    bool[] filter,
    Dictionary<UnitGroupHead, List<int>> gIDs,
    bool battleFirst,
    bool towerEntry)
  {
    this.sortType = sType;
    this.orderType = oType;
    this.filters = filter;
    this.groupIDs = gIDs;
    this.isBattleFirst = battleFirst;
    this.isTowerEntry = towerEntry;
  }
}
