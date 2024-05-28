// Decompiled with JetBrains decompiler
// Type: InventoryExtraSkillExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public static class InventoryExtraSkillExtension
{
  private static bool CheckLevel(InventoryExtraSkill invSkill, bool[] filters)
  {
    if (!filters[1] && !filters[2])
      return true;
    bool flag1 = false;
    bool flag2 = false;
    if (invSkill.level == invSkill.skill.masterData.upper_level)
      flag1 = filters[1];
    if (invSkill.level < invSkill.skill.masterData.upper_level)
      flag2 = filters[2];
    return flag1 | flag2;
  }

  private static bool CheckEquipment(InventoryExtraSkill invSkill, bool[] filters)
  {
    if (!filters[3] && !filters[4])
      return true;
    bool flag1 = false;
    bool flag2 = false;
    if (invSkill.forBattle)
      flag1 = filters[3];
    if (!invSkill.forBattle)
      flag2 = filters[4];
    return flag1 | flag2;
  }

  private static bool CheckGenere(BattleskillGenre genere, bool[] filters)
  {
    if (!filters[7] && !filters[9] && !filters[6] && !filters[5] && !filters[10] && !filters[8])
      return true;
    bool flag = false;
    switch (genere)
    {
      case BattleskillGenre.attack:
        flag = filters[7];
        break;
      case BattleskillGenre.heal:
        flag = filters[9];
        break;
      case BattleskillGenre.buff:
        flag = filters[6];
        break;
      case BattleskillGenre.debuff:
        flag = filters[5];
        break;
      case BattleskillGenre.ailment:
        flag = filters[10];
        break;
      case BattleskillGenre.defense:
        flag = filters[8];
        break;
    }
    return flag;
  }

  private static bool CheckGenere(InventoryExtraSkill invSkill, bool[] filters)
  {
    bool flag1 = false;
    bool flag2 = false;
    BattleskillGenre? genre1 = invSkill.skill.masterData.genre1;
    BattleskillGenre? genre2 = invSkill.skill.masterData.genre2;
    if (!genre1.HasValue && !genre2.HasValue)
      return false;
    if (genre1.HasValue)
      flag1 = InventoryExtraSkillExtension.CheckGenere(genre1.Value, filters);
    if (genre2.HasValue)
      flag2 = InventoryExtraSkillExtension.CheckGenere(genre2.Value, filters);
    return flag1 | flag2;
  }

  private static bool CheckFavorite(InventoryExtraSkill skill, bool[] filters)
  {
    if (!filters[11] && !filters[12])
      return true;
    return skill.favorite ? filters[11] : filters[12];
  }

  private static bool CheckCategory(InventoryExtraSkill skill, bool[] filters)
  {
    if (!filters[19] && !filters[13] && !filters[14] && !filters[15] && !filters[16] && !filters[17] && !filters[18] && !filters[20] && !filters[21] && !filters[23] && !filters[22] && !filters[24])
      return true;
    bool flag = false;
    switch (skill.skill.masterData.awake_skill_category_id)
    {
      case 2:
        flag = filters[19];
        break;
      case 3:
        flag = filters[13];
        break;
      case 4:
        flag = filters[14];
        break;
      case 5:
        flag = filters[15];
        break;
      case 6:
        flag = filters[16];
        break;
      case 7:
        flag = filters[17];
        break;
      case 8:
        flag = filters[18];
        break;
      case 9:
        flag = filters[20];
        break;
      case 10:
        flag = filters[21];
        break;
      case 11:
        flag = filters[22];
        break;
      case 12:
        flag = filters[23];
        break;
      case 13:
        flag = filters[24];
        break;
    }
    return flag;
  }

  public static IEnumerable<InventoryExtraSkill> SortBy(
    this IEnumerable<InventoryExtraSkill> self,
    ExtraSkillSortAndFilter.SORT_TYPES sortType,
    SortAndFilter.SORT_TYPE_ORDER_BUY order)
  {
    InventoryExtraSkill inventoryExtraSkill1 = (InventoryExtraSkill) null;
    List<InventoryExtraSkill> source = new List<InventoryExtraSkill>();
    List<InventoryExtraSkill> inventoryExtraSkillList1 = new List<InventoryExtraSkill>();
    foreach (InventoryExtraSkill inventoryExtraSkill2 in self)
    {
      if (inventoryExtraSkill2.removeButton)
        inventoryExtraSkill1 = inventoryExtraSkill2;
      else
        source.Add(inventoryExtraSkill2);
    }
    List<InventoryExtraSkill> inventoryExtraSkillList2;
    if (sortType != ExtraSkillSortAndFilter.SORT_TYPES.Level)
    {
      if (sortType != ExtraSkillSortAndFilter.SORT_TYPES.GetOrder)
        throw new Exception();
      inventoryExtraSkillList2 = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderBy<InventoryExtraSkill, int>((Func<InventoryExtraSkill, int>) (x => x.uniqueID)).ThenBy<InventoryExtraSkill, int>((Func<InventoryExtraSkill, int>) (x => x.skill == null ? int.MaxValue : x.skill.masterData.ID)).ToList<InventoryExtraSkill>() : source.OrderByDescending<InventoryExtraSkill, int>((Func<InventoryExtraSkill, int>) (x => x.uniqueID)).ThenByDescending<InventoryExtraSkill, int>((Func<InventoryExtraSkill, int>) (x => x.skill == null ? int.MaxValue : x.skill.masterData.ID)).ToList<InventoryExtraSkill>();
    }
    else
      inventoryExtraSkillList2 = order != SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING ? source.OrderBy<InventoryExtraSkill, int>((Func<InventoryExtraSkill, int>) (x => x.level)).ThenBy<InventoryExtraSkill, int>((Func<InventoryExtraSkill, int>) (x => x.skill == null ? int.MaxValue : x.skill.masterData.ID)).ToList<InventoryExtraSkill>() : source.OrderByDescending<InventoryExtraSkill, int>((Func<InventoryExtraSkill, int>) (x => x.level)).ThenByDescending<InventoryExtraSkill, int>((Func<InventoryExtraSkill, int>) (x => x.skill == null ? int.MaxValue : x.skill.masterData.ID)).ToList<InventoryExtraSkill>();
    if (inventoryExtraSkill1 != null)
      inventoryExtraSkillList2.Insert(0, inventoryExtraSkill1);
    return (IEnumerable<InventoryExtraSkill>) inventoryExtraSkillList2;
  }

  public static IEnumerable<InventoryExtraSkill> FilterBy(
    this IEnumerable<InventoryExtraSkill> self,
    bool[] filters)
  {
    InventoryExtraSkill inventoryExtraSkill1 = (InventoryExtraSkill) null;
    List<InventoryExtraSkill> source = new List<InventoryExtraSkill>();
    List<InventoryExtraSkill> inventoryExtraSkillList = new List<InventoryExtraSkill>();
    foreach (InventoryExtraSkill inventoryExtraSkill2 in self)
    {
      if (inventoryExtraSkill2.removeButton)
        inventoryExtraSkill1 = inventoryExtraSkill2;
      else
        source.Add(inventoryExtraSkill2);
    }
    List<InventoryExtraSkill> list = source.Where<InventoryExtraSkill>((Func<InventoryExtraSkill, bool>) (x => InventoryExtraSkillExtension.CheckLevel(x, filters) && InventoryExtraSkillExtension.CheckEquipment(x, filters) && InventoryExtraSkillExtension.CheckGenere(x, filters) && InventoryExtraSkillExtension.CheckFavorite(x, filters) && InventoryExtraSkillExtension.CheckCategory(x, filters))).ToList<InventoryExtraSkill>();
    if (inventoryExtraSkill1 != null)
      list.Insert(0, inventoryExtraSkill1);
    return (IEnumerable<InventoryExtraSkill>) list;
  }
}
