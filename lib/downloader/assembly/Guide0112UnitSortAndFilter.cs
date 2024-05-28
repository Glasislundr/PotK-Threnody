// Decompiled with JetBrains decompiler
// Type: Guide0112UnitSortAndFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Guide0112UnitSortAndFilter : GuideSortAndFilter
{
  public List<GuideSortAndFilterButton> dir_fil1 = new List<GuideSortAndFilterButton>();
  public List<GuideSortAndFilterButton> dir_fil2 = new List<GuideSortAndFilterButton>();
  public List<GuideSortAndFilterButton> dir_fil3 = new List<GuideSortAndFilterButton>();
  public List<GuideSortAndFilterButton> dir_sort = new List<GuideSortAndFilterButton>();
  public List<GuideSortAndFilterButton> dir_sort_on = new List<GuideSortAndFilterButton>();
  public List<GearKindEnum> gearKindEnumList = new List<GearKindEnum>();
  public List<int> unitFamilyOrNullList = new List<int>();
  public List<GuideSortAndFilter.GUIDE_CATEGORY_TYPE> unitCategoryList = new List<GuideSortAndFilter.GUIDE_CATEGORY_TYPE>();

  public override void Initialize(Action SortAction)
  {
    base.Initialize(SortAction);
    try
    {
      this.gearKindEnumList = new List<GearKindEnum>((IEnumerable<GearKindEnum>) Persist.guidUnitSortAndFilter.Data.gearKindEnumList);
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.gearKindEnumList = new List<GearKindEnum>((IEnumerable<GearKindEnum>) Persist.guidUnitSortAndFilter.Data.gearKindEnumList);
    }
    try
    {
      this.unitFamilyOrNullList = new List<int>((IEnumerable<int>) Persist.guidUnitSortAndFilter.Data.unitFamilyOrNullList);
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.unitFamilyOrNullList = new List<int>((IEnumerable<int>) Persist.guidUnitSortAndFilter.Data.unitFamilyOrNullList);
    }
    try
    {
      this.unitCategoryList = new List<GuideSortAndFilter.GUIDE_CATEGORY_TYPE>((IEnumerable<GuideSortAndFilter.GUIDE_CATEGORY_TYPE>) Persist.guidUnitSortAndFilter.Data.unitCategoryList);
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.unitCategoryList = new List<GuideSortAndFilter.GUIDE_CATEGORY_TYPE>((IEnumerable<GuideSortAndFilter.GUIDE_CATEGORY_TYPE>) Persist.guidUnitSortAndFilter.Data.unitCategoryList);
    }
    try
    {
      this.sortType = Persist.guidUnitSortAndFilter.Data.sortType;
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.sortType = Persist.guidUnitSortAndFilter.Data.sortType;
    }
    try
    {
      this.orderBuySort = Persist.guidUnitSortAndFilter.Data.orderBuySort;
    }
    catch (Exception ex)
    {
      Persist.guidGearSortAndFilter.Delete();
      this.orderBuySort = Persist.guidUnitSortAndFilter.Data.orderBuySort;
    }
    this.SetValueWindow();
  }

  public void SetValueWindow()
  {
    foreach (Guide0112UnitSortAndFilterButton button in this.dir_fil1)
      this.GrayCheck<GearKindEnum>(this.gearKindEnumList, button.gearKindEnum, (SortAndFilterButton) button);
    foreach (Guide0112UnitSortAndFilterButton button in this.dir_fil2)
      this.GrayCheck<int>(this.unitFamilyOrNullList, button.unitFamilyOrNull.GetFamily(), (SortAndFilterButton) button);
    foreach (Guide0112UnitSortAndFilterButton button in this.dir_fil3)
      this.GrayCheck<GuideSortAndFilter.GUIDE_CATEGORY_TYPE>(this.unitCategoryList, button.unitCategory, (SortAndFilterButton) button);
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
    this.unitFamilyOrNullList.Clear();
    this.unitCategoryList.Clear();
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
    Persist.guidUnitSortAndFilter.Data.gearKindEnumList = this.gearKindEnumList;
    Persist.guidUnitSortAndFilter.Data.unitFamilyOrNullList = this.unitFamilyOrNullList;
    Persist.guidUnitSortAndFilter.Data.unitCategoryList = this.unitCategoryList;
    Persist.guidUnitSortAndFilter.Data.sortType = this.sortType;
    Persist.guidUnitSortAndFilter.Data.orderBuySort = this.orderBuySort;
    Persist.guidUnitSortAndFilter.Flush();
  }

  public enum GUIDE_UNIT_SORT_FILTER_CATEGORY
  {
    GEAR,
    SUICIDE,
    CATEGORY,
    SORT1,
    SORT2,
  }
}
