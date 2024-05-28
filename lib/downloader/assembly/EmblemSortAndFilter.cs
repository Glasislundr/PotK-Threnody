// Decompiled with JetBrains decompiler
// Type: EmblemSortAndFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class EmblemSortAndFilter : SortAndFilter
{
  [SerializeField]
  private EmblemSortAndFilterTabButton[] ListBtns;
  [SerializeField]
  private EmblemSortAndFilterButton[] OrderBtn;
  [SerializeField]
  private GameObject[] ListObject;
  [SerializeField]
  private EmblemSortAndFilterButton[] SortBtns;
  [SerializeField]
  private EmblemSortAndFilterButton[] FilterBtns;
  private Title0241Menu menuBase;
  private EmblemSortAndFilter.ModeTypes modeType;
  private SortAndFilter.SORT_TYPE_ORDER_BUY orderBuySort;
  private Persist<Persist.EmblemSortAndFilterInfo> persist;
  private Action<SortInfo> SortActionExt;
  private readonly string[] SortLabelStr = new string[4]
  {
    Consts.GetInstance().SORT_POPUP_LABEL_NONE,
    Consts.GetInstance().SORT_POPUP_LABEL_GETORDER,
    Consts.GetInstance().SORT_POPUP_LABEL_RARITY,
    Consts.GetInstance().SORT_POPUP_LABEL_NUMORDER
  };

  public void IbtnSortBtn() => this.SetListTypeBtn(EmblemSortAndFilter.ModeTypes.Sort);

  public void IbtnFilterBtn() => this.SetListTypeBtn(EmblemSortAndFilter.ModeTypes.Filter);

  public void SetSortCategory(EmblemSortAndFilter.SORT_TYPES type)
  {
    this.menuBase.SortCategory = type;
    this.SetSortValueWindow();
  }

  public void SetFilterType(EmblemSortAndFilter.FILTER_TYPES type, bool flg)
  {
    this.menuBase.Filter[(int) type] = flg;
    this.SetFilterValueWindow();
  }

  public void Initialize(Title0241Menu menu, bool setUI = false)
  {
    this.menuBase = menu;
    Persist<Persist.EmblemSortAndFilterInfo> persist = this.menuBase.GetPersist();
    if (persist != null)
    {
      try
      {
        this.menuBase.SortCategory = persist.Data.sortType;
        if (!((IEnumerable<EmblemSortAndFilterButton>) this.SortBtns).Any<EmblemSortAndFilterButton>((Func<EmblemSortAndFilterButton, bool>) (x => x.SortType == this.menuBase.SortCategory)))
          this.menuBase.SortCategory = this.SortBtns[0].SortType;
        this.menuBase.OrderBySort = persist.Data.order;
        bool flag = this.FilterBtns == null || this.FilterBtns.Length == 0;
        for (int index = 0; index < Enum.GetNames(typeof (EmblemSortAndFilter.FILTER_TYPES)).Length; ++index)
          this.menuBase.Filter.SetValue((object) flag, index);
        foreach (EmblemSortAndFilterButton filterBtn in this.FilterBtns)
          this.menuBase.Filter.SetValue((object) persist.Data.filter[(int) filterBtn.FilterType], (int) filterBtn.FilterType);
        this.SetListTypeBtn(persist.Data.modeType);
      }
      catch
      {
        persist.Delete();
        this.menuBase.SortCategory = this.SortBtns[0].SortType;
        this.menuBase.OrderBySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
        foreach (EmblemSortAndFilterButton filterBtn in this.FilterBtns)
          this.menuBase.Filter.SetValue((object) true, (int) filterBtn.FilterType);
      }
    }
    else
    {
      this.menuBase.SortCategory = EmblemSortAndFilter.SORT_TYPES.GetOrder;
      this.menuBase.OrderBySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
      for (int index = 0; index < Enum.GetNames(typeof (EmblemSortAndFilter.FILTER_TYPES)).Length; ++index)
        this.menuBase.Filter.SetValue((object) true, index);
    }
    if (!setUI)
      return;
    this.SetListTypeBtn(persist.Data.modeType);
    this.SetOrderTypeBtn();
    this.SetSortValueWindow();
    this.SetFilterValueWindow();
  }

  private void SetOrderTypeBtn()
  {
    if (this.menuBase.OrderBySort == SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING)
    {
      this.OrderBtn[1].SpriteColorGray(true);
      this.OrderBtn[1].TextColorGray(true);
      this.OrderBtn[0].SpriteColorGray(false);
      this.OrderBtn[0].TextColorGray(false);
    }
    else
    {
      this.OrderBtn[1].SpriteColorGray(false);
      this.OrderBtn[1].TextColorGray(false);
      this.OrderBtn[0].SpriteColorGray(true);
      this.OrderBtn[0].TextColorGray(true);
    }
  }

  private void SetListTypeBtn(EmblemSortAndFilter.ModeTypes mode)
  {
    this.modeType = mode;
    ((IEnumerable<GameObject>) this.ListObject).ToggleOnce((int) mode);
    switch (mode)
    {
      case EmblemSortAndFilter.ModeTypes.Sort:
        this.ListBtns[0].SpriteColorGray(true);
        this.ListBtns[0].TextColorGray(true);
        this.ListBtns[1].SpriteColorGray(false);
        this.ListBtns[1].TextColorGray(false);
        break;
      case EmblemSortAndFilter.ModeTypes.Filter:
        this.ListBtns[0].SpriteColorGray(false);
        this.ListBtns[0].TextColorGray(false);
        this.ListBtns[1].SpriteColorGray(true);
        this.ListBtns[1].TextColorGray(true);
        break;
    }
    this.SetSortTabLabel();
    this.SetFilterTabLabel();
  }

  public void SetSortValueWindow()
  {
    this.SetSortTabLabel();
    this.SetFilterTabLabel();
    foreach (EmblemSortAndFilterButton sortBtn in this.SortBtns)
    {
      sortBtn.SpriteColorGray(false);
      sortBtn.TextColorGray(false);
      if (sortBtn.SortType == this.menuBase.SortCategory)
      {
        sortBtn.SpriteColorGray(true);
        sortBtn.TextColorGray(true);
      }
    }
  }

  public void SetFilterValueWindow()
  {
    this.SetSortTabLabel();
    this.SetFilterTabLabel();
    foreach (EmblemSortAndFilterButton filterBtn in this.FilterBtns)
    {
      if (!Object.op_Equality((Object) filterBtn, (Object) null))
      {
        filterBtn.SpriteColorGray(false);
        filterBtn.TextColorGray(false);
        if (this.menuBase.Filter[(int) filterBtn.FilterType])
        {
          filterBtn.SpriteColorGray(true);
          filterBtn.TextColorGray(true);
        }
      }
    }
  }

  private void SetSortTabLabel()
  {
    this.ListBtns[0].SetText(this.SortLabelStr[(int) this.menuBase.SortCategory]);
    if (this.modeType == EmblemSortAndFilter.ModeTypes.Sort)
      this.ListBtns[0].SetTextColor(new Color(1f, 1f, 0.0f));
    else
      this.ListBtns[0].SetTextColor(new Color(0.5f, 0.5f, 0.0f));
  }

  private void SetFilterTabLabel()
  {
    bool flag = false;
    for (int index = 0; index < Enum.GetNames(typeof (EmblemSortAndFilter.FILTER_TYPES)).Length; ++index)
    {
      if (this.menuBase.Filter[index] && index != 0)
      {
        flag = true;
        break;
      }
    }
    this.ListBtns[1].SetText(flag ? Consts.GetInstance().SORT_POPUP_LABEL_FILTER_ON : Consts.GetInstance().SORT_POPUP_LABEL_FILTER_OFF);
    if (this.modeType == EmblemSortAndFilter.ModeTypes.Filter)
    {
      if (flag)
        this.ListBtns[1].SetTextColor(new Color(1f, 1f, 0.0f));
      else
        this.ListBtns[1].SetTextColor(new Color(1f, 1f, 1f));
    }
    else if (flag)
      this.ListBtns[1].SetTextColor(new Color(0.5f, 0.5f, 0.0f));
    else
      this.ListBtns[1].SetTextColor(new Color(0.5f, 0.5f, 0.5f));
  }

  public override void IbtnDicision()
  {
    this.SaveData();
    if (Object.op_Inequality((Object) this.menuBase, (Object) null))
      this.menuBase.ListSort();
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void IbtnAllFilterSelect()
  {
    for (int index = 0; index < this.FilterBtns.Length; ++index)
      this.menuBase.Filter[(int) this.FilterBtns[index].FilterType] = true;
    this.SetFilterValueWindow();
  }

  public override void IbtnFilterClear()
  {
    for (int index = 0; index < Enum.GetNames(typeof (EmblemSortAndFilter.FILTER_TYPES)).Length; ++index)
      this.menuBase.Filter[index] = false;
    this.SetFilterValueWindow();
  }

  public override void IbtnOrder()
  {
    this.menuBase.OrderBySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.SetOrderTypeBtn();
  }

  public override void IbtnOrderDec()
  {
    this.menuBase.OrderBySort = SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING;
    this.SetOrderTypeBtn();
  }

  public override void SaveData()
  {
    Persist<Persist.EmblemSortAndFilterInfo> persist = this.menuBase.GetPersist();
    if (persist == null)
      return;
    persist.Data.order = this.menuBase.OrderBySort;
    persist.Data.sortType = this.menuBase.SortCategory;
    persist.Data.modeType = this.modeType;
    for (int index = 0; index < Enum.GetNames(typeof (EmblemSortAndFilter.FILTER_TYPES)).Length; ++index)
    {
      if (index < persist.Data.filter.Count)
        persist.Data.filter[index] = this.menuBase.Filter[index];
      else
        persist.Data.filter.Add(this.menuBase.Filter[index]);
    }
    persist.Flush();
  }

  public enum ModeTypes
  {
    Sort,
    Filter,
  }

  public enum SORT_TYPES
  {
    None,
    GetOrder,
    Rarity,
    NumOrder,
  }

  public enum FILTER_TYPES
  {
    None,
    Event,
    MultiBattle,
    Colosseum,
    Collabo,
    Sea,
    Other,
  }
}
