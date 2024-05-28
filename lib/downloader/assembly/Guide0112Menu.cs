// Decompiled with JetBrains decompiler
// Type: Guide0112Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guide0112Menu : GuideZukanListManager
{
  [SerializeField]
  protected UILabel TxtTitle;

  public virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  public virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");

  public IEnumerator onInitMenuAsync()
  {
    IEnumerator e = this.CreateZukanList(Res.Prefabs.popup.popup_011_2_1__anim_popup01.Load<GameObject>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override int ResourceID(GuideUnitUnit unit)
  {
    return unit.Unit.resource_reference_unit_id_UnitUnit;
  }

  public override Future<Sprite> ResourceSprite(GuideUnitUnit unit)
  {
    return unit.Unit.LoadSpriteThumbnail();
  }

  public override int HistroyNumber(GuideUnitUnit unit) => unit.Unit.history_group_number;

  public override DateTime? PublishedTime(GuideUnitUnit unit) => unit.Unit.published_at;

  public override void InitializeInfo()
  {
    this.withNumberInfoList.Clear();
    List<PlayerUnitHistory> list = ((IEnumerable<PlayerUnitHistory>) SMManager.Get<PlayerUnitHistory[]>()).OrderBy<PlayerUnitHistory, int>((Func<PlayerUnitHistory, int>) (x => x.unit_id)).Reverse<PlayerUnitHistory>().ToList<PlayerUnitHistory>();
    GuideUnitUnit addUnit = (GuideUnitUnit) null;
    foreach (GuideUnitUnit unit1 in this.unitList)
    {
      GuideUnitUnit unit = unit1;
      addUnit = new GuideUnitUnit();
      foreach (UnitUnit unitUnit in ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.history_group_number == unit.Unit.history_group_number)).OrderBy<UnitUnit, int>((Func<UnitUnit, int>) (x => x.rarity.index)).Reverse<UnitUnit>().ToList<UnitUnit>())
      {
        foreach (PlayerUnitHistory playerUnitHistory in list)
        {
          if (unitUnit.ID == playerUnitHistory.unit_id)
          {
            addUnit.Unit = unitUnit;
            addUnit.History = playerUnitHistory.created_at;
            break;
          }
        }
        if (addUnit.Unit != null)
          break;
      }
      WithNumberInfo withNumberInfo = new WithNumberInfo();
      if (addUnit.Unit == null)
      {
        withNumberInfo.status = WithNumber.ZUKAN_STATUS.U_UNKNOWN;
        addUnit = unit;
      }
      else
        withNumberInfo.status = WithNumber.ZUKAN_STATUS.NOT_UNKNOWN;
      withNumberInfo.unitData = addUnit;
      withNumberInfo.gearKind = addUnit.Unit.kind;
      withNumberInfo.element = addUnit.Unit.GetElement();
      withNumberInfo.spriteCash = this.spriteCashList.Find((Predicate<SpriteCash>) (x => x.id == this.ZukanID(addUnit)));
      this.withNumberInfoList.Add(withNumberInfo);
    }
  }

  public override void SortAndFilter()
  {
    List<UnitUnit> categoryPlayerList = new List<UnitUnit>();
    this.unitList.Clear();
    categoryPlayerList.Clear();
    DateTime now = ServerTime.NowAppTime();
    ((IEnumerable<UnitUnit>) MasterData.UnitUnitList).Where<UnitUnit>((Func<UnitUnit, bool>) (x => x.character.category == UnitCategory.player && x.history_group_number < 20000)).ForEach<UnitUnit>((Action<UnitUnit>) (obj =>
    {
      if (obj.history_group_number == 0 || !obj.published_at.HasValue)
        return;
      DateTime? publishedAt = obj.published_at;
      DateTime dateTime = now;
      if ((publishedAt.HasValue ? (publishedAt.GetValueOrDefault() < dateTime ? 1 : 0) : 0) == 0)
        return;
      categoryPlayerList.Add(obj);
    }));
    List<PlayerUnitHistory> list = ((IEnumerable<PlayerUnitHistory>) SMManager.Get<PlayerUnitHistory[]>()).ToList<PlayerUnitHistory>();
    List<UnitUnit> historyUnitList = new List<UnitUnit>();
    foreach (PlayerUnitHistory playerUnitHistory in list)
      historyUnitList.Add(MasterData.UnitUnit[playerUnitHistory.unit_id]);
    historyUnitList = historyUnitList.OrderBy<UnitUnit, int>((Func<UnitUnit, int>) (x => x.rarity_UnitRarity)).ToList<UnitUnit>();
    foreach (UnitUnit unitUnit in categoryPlayerList)
    {
      UnitUnit unit = unitUnit;
      if (!this.unitList.FirstIndexOrNull<GuideUnitUnit>((Func<GuideUnitUnit, bool>) (x => x.Unit.history_group_number == unit.history_group_number)).HasValue)
      {
        GuideUnitUnit guideUnitUnit = new GuideUnitUnit();
        guideUnitUnit.Unit = unit;
        int? index2 = historyUnitList.FirstIndexOrNull<UnitUnit>((Func<UnitUnit, bool>) (x => x.history_group_number == unit.history_group_number));
        guideUnitUnit.History = !index2.HasValue ? new DateTime() : list.First<PlayerUnitHistory>((Func<PlayerUnitHistory, bool>) (x => x.unit_id == historyUnitList[index2.Value].ID)).created_at;
        this.unitList.Add(guideUnitUnit);
      }
    }
    this.unitList = this.unitList.OrderBy<GuideUnitUnit, int>((Func<GuideUnitUnit, int>) (x => x.Unit.history_group_number)).ToList<GuideUnitUnit>();
    List<GuideUnitUnit> guideUnitUnitList1 = new List<GuideUnitUnit>();
    List<GuideUnitUnit> guideUnitUnitList2 = new List<GuideUnitUnit>();
    List<GuideUnitUnit> guideUnitUnitList3 = new List<GuideUnitUnit>();
    List<GearKindEnum> gearKindEnumList = Persist.guidUnitSortAndFilter.Data.gearKindEnumList;
    List<int> familyOrNullList = Persist.guidUnitSortAndFilter.Data.unitFamilyOrNullList;
    List<GuideSortAndFilter.GUIDE_CATEGORY_TYPE> unitCategoryList = Persist.guidUnitSortAndFilter.Data.unitCategoryList;
    GuideSortAndFilter.GUIDE_SORT_TYPE sortType = Persist.guidUnitSortAndFilter.Data.sortType;
    GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE orderBuySort = Persist.guidUnitSortAndFilter.Data.orderBuySort;
    if (gearKindEnumList.Any<GearKindEnum>())
    {
      foreach (GearKindEnum gearKindEnum in gearKindEnumList)
      {
        GearKindEnum gearKind = gearKindEnum;
        guideUnitUnitList1.AddRange(this.unitList.Where<GuideUnitUnit>((Func<GuideUnitUnit, bool>) (x => (GearKindEnum) x.Unit.kind_GearKind == gearKind)));
      }
      this.unitList = guideUnitUnitList1;
    }
    if (familyOrNullList.Count > 0)
    {
      foreach (int num in familyOrNullList)
      {
        foreach (GuideUnitUnit unit in this.unitList)
        {
          if (num == 999)
          {
            if (unit.Unit.Families.Length == 0)
              guideUnitUnitList2.Add(unit);
          }
          else
          {
            foreach (UnitFamily family in unit.Unit.Families)
            {
              if (family == (UnitFamily) num && !guideUnitUnitList2.Contains(unit))
                guideUnitUnitList2.Add(unit);
            }
          }
        }
      }
      this.unitList.Clear();
      this.unitList = guideUnitUnitList2;
    }
    if (unitCategoryList.Count > 0)
    {
      foreach (int num in unitCategoryList)
      {
        switch (num)
        {
          case 0:
            using (List<GuideUnitUnit>.Enumerator enumerator = this.unitList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GuideUnitUnit current = enumerator.Current;
                if (current.Unit.IsNormalUnit)
                  guideUnitUnitList3.Add(current);
              }
              continue;
            }
          case 1:
            using (List<GuideUnitUnit>.Enumerator enumerator = this.unitList.FindAll((Predicate<GuideUnitUnit>) (x => x.Unit.IsSinkaUnit)).ToList<GuideUnitUnit>().GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GuideUnitUnit current = enumerator.Current;
                guideUnitUnitList3.Add(current);
              }
              continue;
            }
          case 2:
            using (List<GuideUnitUnit>.Enumerator enumerator = this.unitList.FindAll((Predicate<GuideUnitUnit>) (x => x.Unit.IsTougouUnit)).ToList<GuideUnitUnit>().GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GuideUnitUnit current = enumerator.Current;
                guideUnitUnitList3.Add(current);
              }
              continue;
            }
          case 3:
            using (List<GuideUnitUnit>.Enumerator enumerator = this.unitList.FindAll((Predicate<GuideUnitUnit>) (x => x.Unit.IsTenseiUnit)).ToList<GuideUnitUnit>().GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GuideUnitUnit current = enumerator.Current;
                guideUnitUnitList3.Add(current);
              }
              continue;
            }
          default:
            continue;
        }
      }
      this.unitList.Clear();
      this.unitList = guideUnitUnitList3;
    }
    switch (sortType)
    {
      case GuideSortAndFilter.GUIDE_SORT_TYPE.NEW:
        this.unitList = this.unitList.OrderByDescending<GuideUnitUnit, string>((Func<GuideUnitUnit, string>) (x => this.Order(x, orderBuySort == GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE.FORWARD))).ToList<GuideUnitUnit>();
        break;
      case GuideSortAndFilter.GUIDE_SORT_TYPE.RARE:
        this.unitList = this.unitList.OrderBy<GuideUnitUnit, int>((Func<GuideUnitUnit, int>) (x => this.RarityOrder(x))).ToList<GuideUnitUnit>();
        break;
      case GuideSortAndFilter.GUIDE_SORT_TYPE.NUMBER:
        this.unitList = this.unitList.OrderBy<GuideUnitUnit, int>((Func<GuideUnitUnit, int>) (x => x.Unit.history_group_number)).ToList<GuideUnitUnit>();
        break;
    }
    foreach (GameObject gameObject in this.sortText)
      gameObject.SetActive(false);
    this.sortText[(int) sortType].SetActive(true);
    if (orderBuySort == GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE.FORWARD)
      this.unitList.Reverse();
    this.InitializeInfo();
  }
}
