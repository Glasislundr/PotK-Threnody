// Decompiled with JetBrains decompiler
// Type: Guide0114UnitSortAndFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Guide0114UnitSortAndFilter : GuideSortAndFilter
{
  public List<GuideSortAndFilterButton> dir_fil1 = new List<GuideSortAndFilterButton>();
  public List<GuideSortAndFilterButton> dir_fil1Accessories = new List<GuideSortAndFilterButton>();
  public List<GuideSortAndFilterButton> dir_fil3 = new List<GuideSortAndFilterButton>();
  public List<GuideSortAndFilterButton> dir_sort = new List<GuideSortAndFilterButton>();
  public List<GuideSortAndFilterButton> dir_sort_on = new List<GuideSortAndFilterButton>();
  public GameObject[] dir_weapon;
  private List<GearKindEnum> gearKindEnumList = new List<GearKindEnum>();
  private List<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE> gearCategoryList = new List<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE>();
  private const int DIR_NOT_ACCESSORIES = 0;
  private const int DIR_ACCESSORIES = 1;

  public List<GearKindEnum> GearKindEnumList => this.gearKindEnumList;

  public List<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE> GearCategoryList
  {
    get => this.gearCategoryList;
  }

  public override void Initialize(Action SortAction)
  {
    base.Initialize(SortAction);
    try
    {
      this.gearKindEnumList = new List<GearKindEnum>((IEnumerable<GearKindEnum>) Persist.guidGearSortAndFilter.Data.gearKindEnumList);
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.gearKindEnumList = new List<GearKindEnum>((IEnumerable<GearKindEnum>) Persist.guidGearSortAndFilter.Data.gearKindEnumList);
    }
    try
    {
      this.gearCategoryList = new List<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE>((IEnumerable<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE>) Persist.guidGearSortAndFilter.Data.unitCategoryList);
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.gearCategoryList = new List<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE>((IEnumerable<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE>) Persist.guidGearSortAndFilter.Data.unitCategoryList);
    }
    try
    {
      this.sortType = Persist.guidGearSortAndFilter.Data.sortType;
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.sortType = Persist.guidGearSortAndFilter.Data.sortType;
    }
    try
    {
      this.orderBuySort = Persist.guidGearSortAndFilter.Data.orderBuySort;
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.orderBuySort = Persist.guidGearSortAndFilter.Data.orderBuySort;
    }
    foreach (GameObject gameObject in this.dir_weapon)
      gameObject.SetActive(false);
    if (((IEnumerable<GearGear>) MasterData.GearGearList).Count<GearGear>((Func<GearGear, bool>) (x => x.kind_GearKind == 10)) > 0)
      this.dir_weapon[1].SetActive(true);
    else
      this.dir_weapon[0].SetActive(true);
    this.SetValueWindow();
  }

  public void SetValueWindow()
  {
    if (((IEnumerable<GearGear>) MasterData.GearGearList).Count<GearGear>((Func<GearGear, bool>) (x => x.kind_GearKind == 10)) > 0)
    {
      foreach (Guide0114UnitSortAndFilterButton dirFil1Accessory in this.dir_fil1Accessories)
        this.GrayCheck<GearKindEnum>(this.gearKindEnumList, dirFil1Accessory.gearKindEnum, (SortAndFilterButton) dirFil1Accessory);
    }
    else
    {
      foreach (Guide0114UnitSortAndFilterButton button in this.dir_fil1)
        this.GrayCheck<GearKindEnum>(this.gearKindEnumList, button.gearKindEnum, (SortAndFilterButton) button);
    }
    foreach (Guide0114UnitSortAndFilterButton button in this.dir_fil3)
      this.GrayCheck<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE>(this.gearCategoryList, button.gearCategory, (SortAndFilterButton) button);
    this.IbtnSortType(this.sortType);
    this.IbtnOrderBuySort(this.orderBuySort);
  }

  public void IbtnSortType(GuideSortAndFilter.GUIDE_SORT_TYPE sort)
  {
    this.sortType = sort;
    foreach (Component component in this.dir_sort)
      component.gameObject.SetActive(true);
    foreach (Component component in this.dir_sort_on)
      component.gameObject.SetActive(false);
    ((Component) this.dir_sort_on[(int) this.sortType]).gameObject.SetActive(true);
  }

  public override void IbtnFilterClear()
  {
    this.gearKindEnumList.Clear();
    this.gearCategoryList.Clear();
    this.SetValueWindow();
  }

  public override void IbtnDicision()
  {
    this.SaveData();
    if (this.sortAction != null)
      this.sortAction();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void SaveData()
  {
    Persist.guidGearSortAndFilter.Data.gearKindEnumList = this.gearKindEnumList;
    Persist.guidGearSortAndFilter.Data.unitCategoryList = this.gearCategoryList;
    Persist.guidGearSortAndFilter.Data.sortType = this.sortType;
    Persist.guidGearSortAndFilter.Data.orderBuySort = this.orderBuySort;
    Persist.guidGearSortAndFilter.Flush();
  }

  public enum GUIDE_UNIT_SORT_FILTER_CATEGORY
  {
    GEAR,
    CATEGORY,
    SORT1,
    SORT2,
  }
}
