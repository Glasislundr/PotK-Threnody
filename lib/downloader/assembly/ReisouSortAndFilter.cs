// Decompiled with JetBrains decompiler
// Type: ReisouSortAndFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ReisouSortAndFilter : SortAndFilter
{
  [SerializeField]
  private ReisouSortAndFilterButton[] OrderBtn;
  [SerializeField]
  private ReisouSortAndFilterButton[] FilterBtns;
  [SerializeField]
  private ReisouSortAndFilterButton[] SortBtns;
  [SerializeField]
  private ReisouSortAndFilterButton[] ListBtns;
  [SerializeField]
  private GameObject[] ListObject;
  [SerializeField]
  protected UILabel targetItemNum;
  private Bugu005ItemListMenuBase menuBase;
  [SerializeField]
  protected UILabel sortLabel;
  [SerializeField]
  protected UILabel filterLabel;
  private ReisouSortAndFilter.ModeTypes modeType;
  public Action SortFilterItemNum;
  [SerializeField]
  private SpreadColorButton isEquipBtn;
  private Dictionary<ReisouSortAndFilter.SORT_TYPES, string> sortLabelDic = new Dictionary<ReisouSortAndFilter.SORT_TYPES, string>()
  {
    {
      ReisouSortAndFilter.SORT_TYPES.None,
      "なし"
    },
    {
      ReisouSortAndFilter.SORT_TYPES.BranchOfWeapon,
      "武具種"
    },
    {
      ReisouSortAndFilter.SORT_TYPES.Rarity,
      "レアリティ"
    },
    {
      ReisouSortAndFilter.SORT_TYPES.Rank,
      "ランク"
    },
    {
      ReisouSortAndFilter.SORT_TYPES.GetOrder,
      "入手順"
    }
  };

  public void Initialize(Bugu005ItemListMenuBase menu, bool setUI = false)
  {
    this.menuBase = menu;
    Persist<Persist.ReisouSortAndFilterInfo> reisouPersist = this.menuBase.GetReisouPersist();
    if (reisouPersist != null)
    {
      try
      {
        this.menuBase.ReisouSortCategory = reisouPersist.Data.sortType;
        if (!((IEnumerable<ReisouSortAndFilterButton>) this.SortBtns).Any<ReisouSortAndFilterButton>((Func<ReisouSortAndFilterButton, bool>) (x => x.SortType == this.menuBase.ReisouSortCategory)))
          this.menuBase.ReisouSortCategory = this.SortBtns[0].SortType;
        this.menuBase.ReisouOrderBuySort = reisouPersist.Data.order;
        this.menuBase.isEquipFirst = reisouPersist.Data.isEquipFirst;
        int num = this.FilterBtns == null ? 1 : (this.FilterBtns.Length == 0 ? 1 : 0);
        for (int index = 0; index < 23; ++index)
          this.menuBase.ReisouFilter.SetValue((object) false, index);
        foreach (ReisouSortAndFilterButton filterBtn in this.FilterBtns)
        {
          if (!Object.op_Equality((Object) filterBtn, (Object) null))
            this.menuBase.ReisouFilter.SetValue((object) reisouPersist.Data.filter[(int) filterBtn.FilterType], (int) filterBtn.FilterType);
        }
      }
      catch
      {
        reisouPersist.Delete();
        this.menuBase.ReisouSortCategory = this.SortBtns[0].SortType;
        this.menuBase.ReisouOrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
        this.menuBase.isEquipFirst = true;
        foreach (ReisouSortAndFilterButton filterBtn in this.FilterBtns)
        {
          if (!Object.op_Equality((Object) filterBtn, (Object) null))
            this.menuBase.ReisouFilter.SetValue((object) true, (int) filterBtn.FilterType);
        }
      }
    }
    else
    {
      this.menuBase.ReisouSortCategory = ReisouSortAndFilter.SORT_TYPES.BranchOfWeapon;
      this.menuBase.ReisouOrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
      this.menuBase.isEquipFirst = true;
      for (int index = 0; index < 23; ++index)
        this.menuBase.ReisouFilter.SetValue((object) false, index);
    }
    ((Component) this.isEquipBtn).gameObject.SetActive(true);
    this.isEquipBtn.SetColor(this.menuBase.isEquipFirst ? Color.white : Color.gray);
    this.isEquipBtn.onClick.Clear();
    this.isEquipBtn.onClick.Add(new EventDelegate((EventDelegate.Callback) (() =>
    {
      this.menuBase.isEquipFirst = !this.menuBase.isEquipFirst;
      this.isEquipBtn.SetColor(this.menuBase.isEquipFirst ? Color.white : Color.gray);
    })));
    for (int index = 0; index < this.SortBtns.Length; ++index)
      ((Component) this.SortBtns[index]).gameObject.SetActive(true);
    if (setUI)
    {
      this.SetOrderTypeBtn();
      this.SetValueWindow();
      this.SetListTypeBtn(reisouPersist.Data.modeType);
    }
    this.SetSortTabLabel();
    this.SetFilterLabel();
    this.SetFilterLabelColor();
  }

  private void SetOrderTypeBtn()
  {
    if (this.menuBase.ReisouOrderBuySort == SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING)
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

  public void SetItemNum(List<InventoryItem> displayList, List<InventoryItem> allList)
  {
    int count1 = displayList.Count;
    int count2 = allList.Count;
    foreach (InventoryItem display in displayList)
    {
      if (display.removeButton)
      {
        --count1;
        --count2;
        break;
      }
    }
    this.targetItemNum.SetText((count1 <= 0 ? "[FF0000]" : "[FFFE27]") + (object) count1 + "[-]/" + (object) count2);
  }

  public void SetValueWindow()
  {
    foreach (ReisouSortAndFilterButton sortBtn in this.SortBtns)
    {
      sortBtn.SpriteColorGray(false);
      sortBtn.TextColorGray(false);
      if (sortBtn.SortType == this.menuBase.ReisouSortCategory)
      {
        sortBtn.SpriteColorGray(true);
        sortBtn.TextColorGray(true);
      }
    }
    foreach (ReisouSortAndFilterButton filterBtn in this.FilterBtns)
    {
      if (!Object.op_Equality((Object) filterBtn, (Object) null))
      {
        filterBtn.SpriteColorGray(false);
        filterBtn.TextColorGray(false);
        if (this.menuBase.ReisouFilter[(int) filterBtn.FilterType])
        {
          filterBtn.SpriteColorGray(true);
          filterBtn.TextColorGray(true);
        }
      }
    }
    this.SetFilterLabelColor();
  }

  private void SetFilterLabelColor()
  {
    bool flag = false;
    for (int index = 0; index < 23; ++index)
    {
      if (this.menuBase.ReisouFilter[index])
      {
        flag = true;
        break;
      }
    }
    if (this.modeType == ReisouSortAndFilter.ModeTypes.Sort)
    {
      ((UIWidget) this.sortLabel).color = new Color(1f, 1f, 0.0f);
      if (flag)
        ((UIWidget) this.filterLabel).color = new Color(0.5f, 0.5f, 0.0f);
      else
        ((UIWidget) this.filterLabel).color = new Color(0.5f, 0.5f, 0.5f);
    }
    else
    {
      ((UIWidget) this.sortLabel).color = new Color(0.5f, 0.5f, 0.0f);
      if (flag)
        ((UIWidget) this.filterLabel).color = new Color(1f, 1f, 0.0f);
      else
        ((UIWidget) this.filterLabel).color = new Color(1f, 1f, 1f);
    }
  }

  private void SetListTypeBtn(ReisouSortAndFilter.ModeTypes mode)
  {
    if (Object.op_Inequality((Object) this.ListBtns[0], (Object) null) && Object.op_Inequality((Object) this.ListBtns[1], (Object) null))
    {
      this.modeType = mode;
      ((IEnumerable<GameObject>) this.ListObject).ToggleOnceEx((int) this.modeType);
      if (mode == ReisouSortAndFilter.ModeTypes.Sort)
      {
        this.ListBtns[0].SpriteColorGray(true);
        this.ListBtns[0].TextColorGray(true);
        this.ListBtns[1].SpriteColorGray(false);
        this.ListBtns[1].TextColorGray(false);
      }
      else
      {
        this.ListBtns[0].SpriteColorGray(false);
        this.ListBtns[0].TextColorGray(false);
        this.ListBtns[1].SpriteColorGray(true);
        this.ListBtns[1].TextColorGray(true);
        Action sortFilterItemNum = this.SortFilterItemNum;
        if (sortFilterItemNum == null)
          return;
        sortFilterItemNum();
      }
    }
    else
    {
      this.modeType = ReisouSortAndFilter.ModeTypes.Sort;
      ((IEnumerable<GameObject>) this.ListObject).ToggleOnceEx((int) this.modeType);
    }
  }

  public void SetSortCategory(ReisouSortAndFilter.SORT_TYPES type)
  {
    this.menuBase.ReisouSortCategory = type;
    this.SetValueWindow();
    this.SetSortTabLabel();
  }

  public void SetFilterType(ReisouSortAndFilter.FILTER_TYPES type, bool flg)
  {
    this.menuBase.ReisouFilter[(int) type] = flg;
    this.SetValueWindow();
    Action sortFilterItemNum = this.SortFilterItemNum;
    if (sortFilterItemNum != null)
      sortFilterItemNum();
    this.SetFilterLabel();
  }

  private void SetSortTabLabel()
  {
    this.sortLabel.SetText(this.sortLabelDic[this.menuBase.ReisouSortCategory]);
  }

  private void SetFilterLabel()
  {
    bool flag = false;
    for (int index = 0; index < 23; ++index)
    {
      if (this.menuBase.ReisouFilter[index])
      {
        flag = true;
        break;
      }
    }
    if (flag)
    {
      this.filterLabel.SetText(Consts.GetInstance().SORT_POPUP_LABEL_FILTER_ON);
      ((UIWidget) this.filterLabel).color = new Color(1f, 1f, 0.0f);
    }
    else
    {
      this.filterLabel.SetText(Consts.GetInstance().SORT_POPUP_LABEL_FILTER_OFF);
      ((UIWidget) this.filterLabel).color = new Color(1f, 1f, 1f);
    }
  }

  public void IbtnSortBtn()
  {
    this.SetListTypeBtn(ReisouSortAndFilter.ModeTypes.Sort);
    this.SetFilterLabelColor();
  }

  public void IbtnFilterBtn()
  {
    this.SetListTypeBtn(ReisouSortAndFilter.ModeTypes.Filter);
    this.SetFilterLabelColor();
  }

  public override void IbtnDicision()
  {
    this.SaveData();
    if (Object.op_Inequality((Object) this.menuBase, (Object) null))
      this.menuBase.ReisouSort(this.menuBase.ReisouSortCategory, this.menuBase.ReisouOrderBuySort, this.menuBase.isEquipFirst);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void IbtnOrder()
  {
    this.menuBase.ReisouOrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.SetOrderTypeBtn();
  }

  public override void IbtnOrderDec()
  {
    this.menuBase.ReisouOrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING;
    this.SetOrderTypeBtn();
  }

  public override void SaveData()
  {
    Persist<Persist.ReisouSortAndFilterInfo> reisouPersist = this.menuBase.GetReisouPersist();
    if (reisouPersist == null)
      return;
    reisouPersist.Data.order = this.menuBase.ReisouOrderBuySort;
    reisouPersist.Data.sortType = this.menuBase.ReisouSortCategory;
    reisouPersist.Data.modeType = this.modeType;
    reisouPersist.Data.isEquipFirst = this.menuBase.isEquipFirst;
    for (int index = 0; index < 23; ++index)
    {
      if (index < reisouPersist.Data.filter.Count)
        reisouPersist.Data.filter[index] = this.menuBase.ReisouFilter[index];
      else
        reisouPersist.Data.filter.Add(this.menuBase.ReisouFilter[index]);
    }
    reisouPersist.Flush();
  }

  public override void IbtnAllFilterSelect()
  {
    for (int index = 0; index < this.FilterBtns.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.FilterBtns[index], (Object) null))
        this.menuBase.ReisouFilter[(int) this.FilterBtns[index].FilterType] = this.FilterBtns[index].FilterType != ReisouSortAndFilter.FILTER_TYPES.Equipment && this.FilterBtns[index].FilterType != ReisouSortAndFilter.FILTER_TYPES.NonEquipment;
    }
    this.SetValueWindow();
    Action sortFilterItemNum = this.SortFilterItemNum;
    if (sortFilterItemNum != null)
      sortFilterItemNum();
    this.SetFilterLabel();
  }

  public override void IbtnFilterClear()
  {
    for (int index = 0; index < 23; ++index)
      this.menuBase.ReisouFilter[index] = false;
    this.SetValueWindow();
    Action sortFilterItemNum = this.SortFilterItemNum;
    if (sortFilterItemNum != null)
      sortFilterItemNum();
    this.SetFilterLabel();
  }

  public static UISprite SortSpriteLabel(ReisouSortAndFilter.SORT_TYPES type, UISprite SortSprite)
  {
    string str1 = ReisouSortAndFilter.SetSortLabelSpriteName(type);
    if (!string.IsNullOrEmpty(str1))
    {
      string str2 = string.Format("slc_button_text_{0}_22pt.png__GUI__button_text__button_text_prefab", (object) str1);
      UISpriteData sprite = SortSprite.atlas.GetSprite(str2);
      if (sprite == null)
        return SortSprite;
      SortSprite.spriteName = str2;
      ((Component) SortSprite).GetComponent<UIWidget>().SetDimensions(sprite.width, sprite.height);
    }
    return SortSprite;
  }

  private static string SetSortLabelSpriteName(ReisouSortAndFilter.SORT_TYPES type)
  {
    string str = "";
    switch (type)
    {
      case ReisouSortAndFilter.SORT_TYPES.BranchOfWeapon:
        str = "weapontype";
        break;
      case ReisouSortAndFilter.SORT_TYPES.Rarity:
        str = "sort_rare";
        break;
      case ReisouSortAndFilter.SORT_TYPES.Rank:
        str = "rank";
        break;
      case ReisouSortAndFilter.SORT_TYPES.GetOrder:
        str = "sort_obtaining";
        break;
    }
    return str;
  }

  public enum ModeTypes
  {
    Sort,
    Filter,
  }

  public enum SORT_TYPES
  {
    None,
    BranchOfWeapon,
    Rarity,
    Rank,
    GetOrder,
  }

  public enum FILTER_TYPES
  {
    None,
    WeaponSword,
    WeaponAxe,
    WeaponSpear,
    WeaponBow,
    WeaponGun,
    WeaponStaff,
    WeaponShield,
    WeaponAccessories,
    Rare1,
    Rare2,
    Rare3,
    Rare4,
    Rare5,
    Rare6,
    Rare7,
    Holy,
    Chaos,
    Mythology,
    Undrilled,
    Drilled,
    Equipment,
    NonEquipment,
    Max,
  }
}
