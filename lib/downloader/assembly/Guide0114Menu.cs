// Decompiled with JetBrains decompiler
// Type: Guide0114Menu
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
public class Guide0114Menu : GuideZukanListManager
{
  [SerializeField]
  protected UILabel TxtTitle;
  private GameObject elementIconPrefab;

  public IEnumerator onInitMenuAsync()
  {
    Guide0114Menu guide0114Menu = this;
    Future<GameObject> loader = Res.Icons.CommonElementIcon.Load<GameObject>();
    yield return (object) loader.Wait();
    guide0114Menu.elementIconPrefab = loader.Result;
    loader = (Future<GameObject>) null;
    IEnumerator e = guide0114Menu.CreateZukanList(Res.Prefabs.popup.popup_011_4_1__anim_popup01.Load<GameObject>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public override int ZukanID(GuideUnitUnit unit) => unit.Gear.history_group_number;

  public override int ResourceID(GuideUnitUnit unit)
  {
    return unit.Gear.resource_reference_gear_id_GearGear;
  }

  public override Future<Sprite> ResourceSprite(GuideUnitUnit unit)
  {
    return unit.Gear.LoadSpriteThumbnail();
  }

  public override int HistroyNumber(GuideUnitUnit unit) => unit.Gear.history_group_number;

  public override DateTime? PublishedTime(GuideUnitUnit unit)
  {
    return new DateTime?(unit.Gear.published_at);
  }

  public override void InitializeInfo()
  {
    this.withNumberInfoList.Clear();
    List<PlayerGearHistory> list = ((IEnumerable<PlayerGearHistory>) SMManager.Get<PlayerGearHistory[]>()).OrderBy<PlayerGearHistory, int>((Func<PlayerGearHistory, int>) (x => x.gear_id)).Reverse<PlayerGearHistory>().ToList<PlayerGearHistory>();
    GuideUnitUnit addGear = (GuideUnitUnit) null;
    foreach (GuideUnitUnit unit in this.unitList)
    {
      GuideUnitUnit gear = unit;
      addGear = new GuideUnitUnit();
      foreach (GearGear gearGear in ((IEnumerable<GearGear>) MasterData.GearGearList).Where<GearGear>((Func<GearGear, bool>) (x => x.history_group_number == gear.Gear.history_group_number)).OrderBy<GearGear, int>((Func<GearGear, int>) (x => x.rarity.index)).Reverse<GearGear>().ToList<GearGear>())
      {
        foreach (PlayerGearHistory playerGearHistory in list)
        {
          if (gearGear.ID == playerGearHistory.gear_id)
          {
            addGear.Gear = gearGear;
            addGear.History = playerGearHistory.created_at;
            break;
          }
        }
        if (addGear.Gear != null)
          break;
      }
      WithNumberInfo withNumberInfo = new WithNumberInfo();
      if (addGear.Gear == null)
      {
        withNumberInfo.status = WithNumber.ZUKAN_STATUS.G_UNKNOWN;
        addGear = gear;
      }
      else
        withNumberInfo.status = WithNumber.ZUKAN_STATUS.G_NOT_UNKNOWN;
      withNumberInfo.unitData = addGear;
      withNumberInfo.gearKind = addGear.Gear.kind;
      withNumberInfo.element = addGear.Gear.GetElement();
      withNumberInfo.spriteCash = this.spriteCashList.Find((Predicate<SpriteCash>) (x => x.id == this.ZukanID(addGear)));
      withNumberInfo.elementIconPrefab = this.elementIconPrefab;
      this.withNumberInfoList.Add(withNumberInfo);
    }
  }

  public override void SortAndFilter()
  {
    List<PlayerGearHistory> list = ((IEnumerable<PlayerGearHistory>) SMManager.Get<PlayerGearHistory[]>()).ToList<PlayerGearHistory>();
    List<GearGear> drawGearList = new List<GearGear>();
    this.unitList.Clear();
    drawGearList.Clear();
    DateTime now = ServerTime.NowAppTime();
    ((IEnumerable<GearGear>) MasterData.GearGearList).ForEach<GearGear>((Action<GearGear>) (obj =>
    {
      if (obj.history_group_number == 0 || obj.published_at == new DateTime() || !(obj.published_at < now))
        return;
      drawGearList.Add(obj);
    }));
    foreach (GearGear gearGear in drawGearList)
    {
      GearGear gear = gearGear;
      if (!this.unitList.FirstIndexOrNull<GuideUnitUnit>((Func<GuideUnitUnit, bool>) (x => x.Gear.history_group_number == gear.history_group_number)).HasValue)
      {
        GuideUnitUnit guideUnitUnit = new GuideUnitUnit();
        guideUnitUnit.Gear = gear;
        int? nullable = list.FirstIndexOrNull<PlayerGearHistory>((Func<PlayerGearHistory, bool>) (x => x.gear_id == gear.ID));
        guideUnitUnit.History = nullable.HasValue ? list[nullable.Value].created_at : new DateTime();
        this.unitList.Add(guideUnitUnit);
      }
    }
    this.unitList = this.unitList.OrderBy<GuideUnitUnit, int>((Func<GuideUnitUnit, int>) (x => x.Gear.history_group_number)).ToList<GuideUnitUnit>();
    List<GuideUnitUnit> guideUnitUnitList1 = new List<GuideUnitUnit>();
    List<GuideUnitUnit> guideUnitUnitList2 = new List<GuideUnitUnit>();
    List<GearKindEnum> gearKindEnumList = Persist.guidGearSortAndFilter.Data.gearKindEnumList;
    List<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE> unitCategoryList = Persist.guidGearSortAndFilter.Data.unitCategoryList;
    GuideSortAndFilter.GUIDE_SORT_TYPE sortType = Persist.guidGearSortAndFilter.Data.sortType;
    GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE orderBuySort = Persist.guidGearSortAndFilter.Data.orderBuySort;
    if (gearKindEnumList.Count > 0)
    {
      foreach (GearKindEnum gearKindEnum in gearKindEnumList)
      {
        GearKindEnum gearKind = gearKindEnum;
        foreach (GuideUnitUnit unit1 in this.unitList)
        {
          GuideUnitUnit unit = unit1;
          if (((IEnumerable<GearGear>) MasterData.GearGearList).Where<GearGear>((Func<GearGear, bool>) (x => x.history_group_number == unit.Gear.history_group_number)).FirstIndexOrNull<GearGear>((Func<GearGear, bool>) (x =>
          {
            if ((GearKindEnum) x.kind_GearKind == gearKind)
              return true;
            if (gearKind != GearKindEnum.smith)
              return false;
            return x.kind_GearKind == 11 || x.kind_GearKind == 12 || x.kind_GearKind == 13;
          })).HasValue && !guideUnitUnitList1.Exists((Predicate<GuideUnitUnit>) (x => x.Gear.history_group_number == unit.Gear.history_group_number)))
            guideUnitUnitList1.Add(unit);
        }
      }
      this.unitList.Clear();
      this.unitList = guideUnitUnitList1;
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
                if (current.Gear.kind.isEquip)
                  guideUnitUnitList2.Add(current);
              }
              continue;
            }
          case 1:
            using (List<GuideUnitUnit>.Enumerator enumerator = this.unitList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GuideUnitUnit current = enumerator.Current;
                if (current.Gear.kind.Enum == GearKindEnum.smith && current.Gear.compose_kind.kind.Enum != GearKindEnum.smith)
                  guideUnitUnitList2.Add(current);
              }
              continue;
            }
          case 2:
            using (List<GuideUnitUnit>.Enumerator enumerator = this.unitList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                GuideUnitUnit current = enumerator.Current;
                if (current.Gear.kind.Enum == GearKindEnum.smith)
                {
                  if (current.Gear.compose_kind.kind.Enum == GearKindEnum.smith)
                    guideUnitUnitList2.Add(current);
                }
                else if (current.Gear.kind.Enum == GearKindEnum.drilling || current.Gear.kind.Enum == GearKindEnum.sea_present)
                  guideUnitUnitList2.Add(current);
                else if (current.Gear.kind.Enum == GearKindEnum.special_drilling)
                  guideUnitUnitList2.Add(current);
              }
              continue;
            }
          default:
            continue;
        }
      }
      this.unitList.Clear();
      this.unitList = guideUnitUnitList2;
    }
    switch (sortType)
    {
      case GuideSortAndFilter.GUIDE_SORT_TYPE.NEW:
        this.unitList = this.unitList.OrderByDescending<GuideUnitUnit, string>((Func<GuideUnitUnit, string>) (x => this.Order(x, orderBuySort == GuideSortAndFilter.GUIDE_ORDER_BUY_SORT_TYPE.FORWARD))).ToList<GuideUnitUnit>();
        break;
      case GuideSortAndFilter.GUIDE_SORT_TYPE.RARE:
        this.unitList = this.unitList.OrderBy<GuideUnitUnit, int>((Func<GuideUnitUnit, int>) (x => this.RarityOrderGear(x))).ToList<GuideUnitUnit>();
        break;
      case GuideSortAndFilter.GUIDE_SORT_TYPE.NUMBER:
        this.unitList = this.unitList.OrderBy<GuideUnitUnit, int>((Func<GuideUnitUnit, int>) (x => x.Gear.history_group_number)).ToList<GuideUnitUnit>();
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
