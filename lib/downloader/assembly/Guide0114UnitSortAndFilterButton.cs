// Decompiled with JetBrains decompiler
// Type: Guide0114UnitSortAndFilterButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;

#nullable disable
public class Guide0114UnitSortAndFilterButton : GuideSortAndFilterButton
{
  public Guide0114UnitSortAndFilter menu;
  public Guide0114UnitSortAndFilter.GUIDE_UNIT_SORT_FILTER_CATEGORY categoryID;
  public GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE gearCategory;

  public override void PressButton()
  {
    switch (this.categoryID)
    {
      case Guide0114UnitSortAndFilter.GUIDE_UNIT_SORT_FILTER_CATEGORY.GEAR:
        this.menu.IbtnFilter<GearKindEnum>(this.menu.GearKindEnumList, this.gearKindEnum, (SortAndFilterButton) this);
        break;
      case Guide0114UnitSortAndFilter.GUIDE_UNIT_SORT_FILTER_CATEGORY.CATEGORY:
        this.menu.IbtnFilter<GuideSortAndFilter.GUIDE_GEAR_CATEGORY_TYPE>(this.menu.GearCategoryList, this.gearCategory, (SortAndFilterButton) this);
        break;
      case Guide0114UnitSortAndFilter.GUIDE_UNIT_SORT_FILTER_CATEGORY.SORT1:
        this.menu.IbtnSortType(this.sort1);
        break;
      case Guide0114UnitSortAndFilter.GUIDE_UNIT_SORT_FILTER_CATEGORY.SORT2:
        this.menu.IbtnOrderBuySort(this.orderBuySort);
        break;
    }
  }
}
