// Decompiled with JetBrains decompiler
// Type: ColosseumUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ColosseumUtility
{
  public static IEnumerator CreateGearIcon(
    GameObject prefab,
    GearGear gear,
    Transform parent,
    bool isBottom = true)
  {
    ItemIcon icon = prefab.Clone(parent).GetComponent<ItemIcon>();
    IEnumerator e = icon.InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.gear.broken.SetActive(false);
    icon.gear.newGear.SetActive(false);
    icon.BottomModeValue = ItemIcon.BottomMode.Visible;
    icon.gear.bottom.SetActive(isBottom);
  }

  public static IEnumerator CreateWeaponMaterialIcon(
    GameObject prefab,
    GearGear gear,
    Transform parent,
    bool isBottom = true)
  {
    ItemIcon icon = prefab.Clone(parent).GetComponent<ItemIcon>();
    IEnumerator e = icon.InitByGear(gear, gear.GetElement(), true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.gear.broken.SetActive(false);
    icon.gear.newGear.SetActive(false);
    icon.BottomModeValue = ItemIcon.BottomMode.Visible;
    icon.gear.bottom.SetActive(isBottom);
  }

  public static IEnumerator CreateSupplyIcon(
    GameObject prefab,
    SupplySupply supply,
    Transform parent,
    bool isBottom = true)
  {
    ItemIcon icon = prefab.Clone(parent).GetComponent<ItemIcon>();
    IEnumerator e = icon.InitBySupply(supply);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    icon.gear.broken.SetActive(false);
    icon.gear.newGear.SetActive(false);
    icon.QuantitySupply = false;
    icon.BottomModeValue = ItemIcon.BottomMode.Visible;
    icon.gear.bottom.SetActive(isBottom);
  }

  public static IEnumerator CreateUnitIcon(
    GameObject prefab,
    UnitUnit unit,
    Transform parent,
    bool isBottom = true)
  {
    UnitIcon icon = prefab.Clone(parent).GetComponent<UnitIcon>();
    IEnumerator e = icon.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unit.IsMaterialUnit)
    {
      icon.RarityCenter();
    }
    else
    {
      icon.setLevelText("1");
      icon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    }
    icon.BottomBaseObject = isBottom;
  }

  public static IEnumerator CreateUniqueIcon(
    GameObject gearPrefab,
    Transform parent,
    int rewardTypeID,
    int rewardID,
    int rewardQuantity,
    bool isBottom = true)
  {
    IEnumerator e = ((Component) parent).gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail((MasterDataTable.CommonRewardType) rewardTypeID, rewardID, rewardQuantity, isBottom);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static IEnumerator CreateUniqueIcon(
    GameObject gearPrefab,
    Transform parent,
    MasterDataTable.CommonRewardType rewardTypeID,
    int rewardID,
    int rewardQuantity,
    bool isBottom = true)
  {
    IEnumerator e = ((Component) parent).gameObject.GetOrAddComponent<CreateIconObject>().CreateThumbnail(rewardTypeID, rewardID, rewardQuantity, isBottom);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public static int GetNextRankPoint(int point, int maxPoint)
  {
    return ColosseumUtility.GetRankInfo(point, maxPoint).to_point - point;
  }

  public static float GetNextRankRate(int point, int maxPoint)
  {
    int toPoint1 = ColosseumUtility.GetPrevRankInfo(point, maxPoint).to_point;
    int toPoint2 = ColosseumUtility.GetRankInfo(point, maxPoint).to_point;
    int num = toPoint1 != toPoint2 ? toPoint1 + 1 : 0;
    return (float) (point - num) / (float) (toPoint2 - num);
  }

  public static string GetRankName(int point)
  {
    return ColosseumUtility.GetRankInfo(point, int.MaxValue).name;
  }

  public static int GetRankID(int point) => ColosseumUtility.GetRankInfo(point, int.MaxValue).ID;

  public static ColosseumRank GetPrevRankInfo(int point, int maxPoint)
  {
    ColosseumRank[] array = ((IEnumerable<ColosseumRank>) MasterData.ColosseumRankList).Where<ColosseumRank>((Func<ColosseumRank, bool>) (v => v.to_point <= maxPoint)).OrderBy<ColosseumRank, int>((Func<ColosseumRank, int>) (v => v.to_point)).ToArray<ColosseumRank>();
    if (array.Length == 0)
      return (ColosseumRank) null;
    ColosseumRank prevRankInfo = array[0];
    foreach (ColosseumRank colosseumRank in array)
    {
      if (point <= colosseumRank.to_point)
        return prevRankInfo;
      prevRankInfo = colosseumRank;
    }
    return prevRankInfo;
  }

  public static ColosseumRankReward[] GetRankRewardFromPoint(int point)
  {
    ColosseumRank info = ColosseumUtility.GetRankInfo(point, int.MaxValue);
    return ((IEnumerable<ColosseumRankReward>) MasterData.ColosseumRankRewardList).Where<ColosseumRankReward>((Func<ColosseumRankReward, bool>) (v => v.rank_id == info.ID)).ToArray<ColosseumRankReward>();
  }

  public static ColosseumRankReward[] GetRankRewardFromID(int id)
  {
    return ((IEnumerable<ColosseumRankReward>) MasterData.ColosseumRankRewardList).Where<ColosseumRankReward>((Func<ColosseumRankReward, bool>) (v => v.rank_id == id)).ToArray<ColosseumRankReward>();
  }

  private static ColosseumRank GetRankInfo(int point, int maxPoint)
  {
    IOrderedEnumerable<ColosseumRank> source = ((IEnumerable<ColosseumRank>) MasterData.ColosseumRankList).Where<ColosseumRank>((Func<ColosseumRank, bool>) (v => v.to_point <= maxPoint)).OrderBy<ColosseumRank, int>((Func<ColosseumRank, int>) (v => v.to_point));
    foreach (ColosseumRank rankInfo in (IEnumerable<ColosseumRank>) source)
    {
      if (point <= rankInfo.to_point)
        return rankInfo;
    }
    return source.Last<ColosseumRank>();
  }

  public class Info
  {
    public bool rankingUpdated;
    public bool is_battle;
    public bool is_tutorial;
    public bool resume_able = true;
    public int next_battle_type;
    public ColosseumRecord colosseum_record;
    public Bonus[] bonus;
    public Gladiator[] gladiators;
    public Campaign[] campaigns;

    public Info()
    {
    }

    public Info(bool update, WebAPI.Response.ColosseumBoot boot)
    {
      this.rankingUpdated = update;
      this.SetBootInfo(boot);
    }

    public Info(bool update, WebAPI.Response.ColosseumTutorialBoot boot)
    {
      this.rankingUpdated = update;
      this.SetBootInfo(boot);
    }

    public Info(bool update, WebAPI.Response.ColosseumFinish finish)
    {
      this.rankingUpdated = update;
      this.SetBootInfo(finish);
    }

    public Info(bool update, WebAPI.Response.ColosseumTutorialFinish finish)
    {
      this.rankingUpdated = update;
      this.SetBootInfo(finish);
    }

    public void SetBootInfo(WebAPI.Response.ColosseumBoot boot)
    {
      this.is_battle = boot.is_battle;
      this.is_tutorial = boot.is_tutorial;
      this.resume_able = boot.resume_able;
      this.next_battle_type = boot.next_battle_type;
      this.colosseum_record = boot.colosseum_record;
      this.bonus = boot.bonus;
      this.gladiators = boot.gladiators;
      this.campaigns = boot.campaigns;
    }

    public void SetBootInfo(WebAPI.Response.ColosseumTutorialBoot boot)
    {
      this.is_battle = boot.is_battle;
      this.is_tutorial = boot.is_tutorial;
      this.resume_able = boot.resume_able;
      this.next_battle_type = boot.next_battle_type;
      this.colosseum_record = boot.colosseum_record;
      this.bonus = boot.bonus;
      this.gladiators = boot.gladiators;
      this.campaigns = boot.campaigns;
    }

    public void SetBootInfo(WebAPI.Response.ColosseumFinish finish)
    {
      this.is_battle = finish.is_battle;
      this.is_tutorial = finish.is_tutorial;
      this.next_battle_type = finish.next_battle_type;
      this.colosseum_record = finish.colosseum_record;
      this.bonus = finish.bonus;
      this.gladiators = finish.gladiators;
      this.campaigns = finish.campaigns;
    }

    public void SetBootInfo(WebAPI.Response.ColosseumTutorialFinish finish)
    {
      this.is_battle = finish.is_battle;
      this.is_tutorial = finish.is_tutorial;
      this.next_battle_type = finish.next_battle_type;
      this.colosseum_record = finish.colosseum_record;
      this.bonus = finish.bonus;
      this.gladiators = finish.gladiators;
    }
  }
}
