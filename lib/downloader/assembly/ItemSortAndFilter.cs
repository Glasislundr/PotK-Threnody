// Decompiled with JetBrains decompiler
// Type: ItemSortAndFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ItemSortAndFilter : SortAndFilter
{
  [SerializeField]
  protected ItemSortAndFilter.FILTER_TYPES[] ForceEnableFilterType;
  [SerializeField]
  private ItemSortAndFilterButton[] OrderBtn;
  [SerializeField]
  private ItemSortAndFilterButton[] FilterBtns;
  [SerializeField]
  private ItemSortAndFilterButton[] SortBtns;
  [SerializeField]
  private ItemSortAndFilterButton[] ListBtns;
  [SerializeField]
  private GameObject[] ListObject;
  [SerializeField]
  protected UILabel targetItemNum;
  private Bugu005ItemListMenuBase menuBase;
  [SerializeField]
  protected UILabel sortLabel;
  [SerializeField]
  protected UILabel filterLabel;
  private ItemSortAndFilter.ModeTypes modeType;
  public Action SortFilterItemNum;
  [SerializeField]
  private SpreadColorButton isEquipBtn;
  private Dictionary<ItemSortAndFilter.SORT_TYPES, string> sortLabelDic = new Dictionary<ItemSortAndFilter.SORT_TYPES, string>()
  {
    {
      ItemSortAndFilter.SORT_TYPES.BranchOfWeapon,
      "武具種"
    },
    {
      ItemSortAndFilter.SORT_TYPES.Attribute,
      "属性"
    },
    {
      ItemSortAndFilter.SORT_TYPES.Rarity,
      "レアリティ"
    },
    {
      ItemSortAndFilter.SORT_TYPES.Rank,
      "ランク"
    },
    {
      ItemSortAndFilter.SORT_TYPES.RankMax,
      "最大ランク"
    },
    {
      ItemSortAndFilter.SORT_TYPES.Favorite,
      "お気に入り"
    },
    {
      ItemSortAndFilter.SORT_TYPES.GetOrder,
      "入手順"
    },
    {
      ItemSortAndFilter.SORT_TYPES.Category,
      "カテゴリ"
    },
    {
      ItemSortAndFilter.SORT_TYPES.Name,
      "武具名"
    },
    {
      ItemSortAndFilter.SORT_TYPES.HistoryGroupNumber,
      "図鑑番号"
    }
  };

  public void Initialize(Bugu005ItemListMenuBase menu, bool setUI = false)
  {
    this.menuBase = menu;
    Persist<Persist.ItemSortAndFilterInfo> persist = this.menuBase.GetPersist();
    if (persist != null)
    {
      try
      {
        this.menuBase.SortCategory = persist.Data.sortType;
        if (!((IEnumerable<ItemSortAndFilterButton>) this.SortBtns).Any<ItemSortAndFilterButton>((Func<ItemSortAndFilterButton, bool>) (x => x.SortType == this.menuBase.SortCategory)))
          this.menuBase.SortCategory = this.SortBtns[0].SortType;
        this.menuBase.OrderBuySort = persist.Data.order;
        this.menuBase.isEquipFirst = persist.Data.isEquipFirst;
        int num1 = this.FilterBtns == null ? 1 : (this.FilterBtns.Length == 0 ? 1 : 0);
        for (int index = 0; index < 45; ++index)
          this.menuBase.Filter.SetValue((object) false, index);
        if (this.ForceEnableFilterType != null && this.ForceEnableFilterType.Length != 0)
        {
          foreach (int index in this.ForceEnableFilterType)
            this.menuBase.Filter.SetValue((object) true, index);
        }
        int num2 = persist.Data.filter.Count<bool>();
        foreach (ItemSortAndFilterButton filterBtn in this.FilterBtns)
        {
          if (!Object.op_Equality((Object) filterBtn, (Object) null) && filterBtn.FilterType < (ItemSortAndFilter.FILTER_TYPES) num2)
            this.menuBase.Filter.SetValue((object) persist.Data.filter[(int) filterBtn.FilterType], (int) filterBtn.FilterType);
        }
      }
      catch
      {
        persist.Delete();
        this.menuBase.SortCategory = this.SortBtns[0].SortType;
        this.menuBase.OrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
        this.menuBase.isEquipFirst = true;
        foreach (ItemSortAndFilterButton filterBtn in this.FilterBtns)
        {
          if (!Object.op_Equality((Object) filterBtn, (Object) null))
            this.menuBase.Filter.SetValue((object) true, (int) filterBtn.FilterType);
        }
      }
    }
    else
    {
      this.menuBase.SortCategory = ItemSortAndFilter.SORT_TYPES.BranchOfWeapon;
      this.menuBase.OrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
      this.menuBase.isEquipFirst = true;
      for (int index = 0; index < 45; ++index)
        this.menuBase.Filter.SetValue((object) false, index);
    }
    if (Singleton<NGSceneManager>.GetInstance().sceneName != "bugu005_material_list" && Singleton<NGSceneManager>.GetInstance().sceneName != "bugu005_sell")
    {
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
    }
    else
    {
      ((Component) this.isEquipBtn).gameObject.SetActive(false);
      for (int index = 0; index < this.SortBtns.Length; ++index)
      {
        if (this.SortBtns[index].SortType == ItemSortAndFilter.SORT_TYPES.Rank || this.SortBtns[index].SortType == ItemSortAndFilter.SORT_TYPES.RankMax || this.SortBtns[index].SortType == ItemSortAndFilter.SORT_TYPES.GetOrder)
          ((Component) this.SortBtns[index]).gameObject.SetActive(false);
      }
    }
    if (setUI)
    {
      this.SetOrderTypeBtn();
      this.SetValueWindow();
      this.SetListTypeBtn(persist.Data.modeType);
    }
    this.SetSortTabLabel();
    this.SetFilterLabel();
    this.SetFilterLabelColor();
  }

  private void SetOrderTypeBtn()
  {
    if (this.menuBase.OrderBuySort == SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING)
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
    foreach (ItemSortAndFilterButton sortBtn in this.SortBtns)
    {
      sortBtn.SpriteColorGray(false);
      sortBtn.TextColorGray(false);
      if (sortBtn.SortType == this.menuBase.SortCategory)
      {
        sortBtn.SpriteColorGray(true);
        sortBtn.TextColorGray(true);
      }
    }
    foreach (ItemSortAndFilterButton filterBtn in this.FilterBtns)
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
    this.SetFilterLabelColor();
  }

  private void SetFilterLabelColor()
  {
    bool flag = false;
    for (int index = 0; index < 45; ++index)
    {
      if (this.menuBase.Filter[index])
      {
        flag = true;
        break;
      }
    }
    if (this.modeType == ItemSortAndFilter.ModeTypes.Sort)
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

  private void SetListTypeBtn(ItemSortAndFilter.ModeTypes mode)
  {
    if (Object.op_Inequality((Object) this.ListBtns[0], (Object) null) && Object.op_Inequality((Object) this.ListBtns[1], (Object) null))
    {
      this.modeType = mode;
      ((IEnumerable<GameObject>) this.ListObject).ToggleOnceEx((int) this.modeType);
      if (mode == ItemSortAndFilter.ModeTypes.Sort)
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
        if (this.SortFilterItemNum == null)
          return;
        this.SortFilterItemNum();
      }
    }
    else
    {
      this.modeType = ItemSortAndFilter.ModeTypes.Sort;
      ((IEnumerable<GameObject>) this.ListObject).ToggleOnceEx((int) this.modeType);
    }
  }

  public void SetSortCategory(ItemSortAndFilter.SORT_TYPES type)
  {
    this.menuBase.SortCategory = type;
    this.SetValueWindow();
    this.SetSortTabLabel();
  }

  public void SetFilterType(ItemSortAndFilter.FILTER_TYPES type, bool flg)
  {
    this.menuBase.Filter[(int) type] = flg;
    this.SetValueWindow();
    if (this.SortFilterItemNum != null)
      this.SortFilterItemNum();
    this.SetFilterLabel();
  }

  private void SetSortTabLabel()
  {
    this.sortLabel.SetText(this.sortLabelDic[this.menuBase.SortCategory]);
  }

  private void SetFilterLabel()
  {
    bool flag = false;
    for (int index = 0; index < 45; ++index)
    {
      if (this.menuBase.Filter[index])
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
    this.SetListTypeBtn(ItemSortAndFilter.ModeTypes.Sort);
    this.SetFilterLabelColor();
  }

  public void IbtnFilterBtn()
  {
    this.SetListTypeBtn(ItemSortAndFilter.ModeTypes.Filter);
    this.SetFilterLabelColor();
  }

  public override void IbtnDicision()
  {
    this.SaveData();
    if (Object.op_Inequality((Object) this.menuBase, (Object) null))
      this.menuBase.Sort(this.menuBase.SortCategory, this.menuBase.OrderBuySort, this.menuBase.isEquipFirst);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void IbtnOrder()
  {
    this.menuBase.OrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.SetOrderTypeBtn();
  }

  public override void IbtnOrderDec()
  {
    this.menuBase.OrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING;
    this.SetOrderTypeBtn();
  }

  public override void SaveData()
  {
    Persist<Persist.ItemSortAndFilterInfo> persist = this.menuBase.GetPersist();
    if (persist == null)
      return;
    persist.Data.order = this.menuBase.OrderBuySort;
    persist.Data.sortType = this.menuBase.SortCategory;
    persist.Data.modeType = this.modeType;
    persist.Data.isEquipFirst = this.menuBase.isEquipFirst;
    for (int index = 0; index < 45; ++index)
    {
      if (index < persist.Data.filter.Count)
        persist.Data.filter[index] = this.menuBase.Filter[index];
      else
        persist.Data.filter.Add(this.menuBase.Filter[index]);
    }
    persist.Flush();
  }

  public override void IbtnAllFilterSelect()
  {
    for (int index = 0; index < this.FilterBtns.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.FilterBtns[index], (Object) null))
        this.menuBase.Filter[(int) this.FilterBtns[index].FilterType] = this.FilterBtns[index].FilterType != ItemSortAndFilter.FILTER_TYPES.Favorite && this.FilterBtns[index].FilterType != ItemSortAndFilter.FILTER_TYPES.NoFavorite && this.FilterBtns[index].FilterType != ItemSortAndFilter.FILTER_TYPES.LevelMax && this.FilterBtns[index].FilterType != ItemSortAndFilter.FILTER_TYPES.OtherLevelMax && this.FilterBtns[index].FilterType != ItemSortAndFilter.FILTER_TYPES.Equipment && this.FilterBtns[index].FilterType != ItemSortAndFilter.FILTER_TYPES.NonEquipment && this.FilterBtns[index].FilterType != ItemSortAndFilter.FILTER_TYPES.WeaponGear && this.FilterBtns[index].FilterType != ItemSortAndFilter.FILTER_TYPES.ReisouGear;
    }
    if (this.ForceEnableFilterType != null && this.ForceEnableFilterType.Length != 0)
    {
      foreach (int index in this.ForceEnableFilterType)
        this.menuBase.Filter[index] = true;
    }
    this.SetValueWindow();
    if (this.SortFilterItemNum != null)
      this.SortFilterItemNum();
    this.SetFilterLabel();
  }

  public override void IbtnFilterClear()
  {
    for (int index = 0; index < 45; ++index)
      this.menuBase.Filter[index] = false;
    this.SetValueWindow();
    if (this.SortFilterItemNum != null)
      this.SortFilterItemNum();
    this.SetFilterLabel();
  }

  public static UISprite SortSpriteLabel(ItemSortAndFilter.SORT_TYPES type, UISprite SortSprite)
  {
    string str1 = ItemSortAndFilter.SetSortLabelSpriteName(type);
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

  private static string SetSortLabelSpriteName(ItemSortAndFilter.SORT_TYPES type)
  {
    string str = "";
    switch (type)
    {
      case ItemSortAndFilter.SORT_TYPES.BranchOfWeapon:
        str = "weapontype";
        break;
      case ItemSortAndFilter.SORT_TYPES.Attribute:
        str = "element";
        break;
      case ItemSortAndFilter.SORT_TYPES.Rarity:
        str = "sort_rare";
        break;
      case ItemSortAndFilter.SORT_TYPES.Rank:
        str = "rank";
        break;
      case ItemSortAndFilter.SORT_TYPES.RankMax:
        str = "maximumrank";
        break;
      case ItemSortAndFilter.SORT_TYPES.GetOrder:
        str = "sort_obtaining";
        break;
      case ItemSortAndFilter.SORT_TYPES.Category:
        str = "sort_categoly";
        break;
      case ItemSortAndFilter.SORT_TYPES.Name:
        str = "sort_bugu_name";
        break;
      case ItemSortAndFilter.SORT_TYPES.HistoryGroupNumber:
        str = "sort_book_number";
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
    Attribute,
    Rarity,
    Rank,
    RankMax,
    Favorite,
    GetOrder,
    Category,
    Name,
    HistoryGroupNumber,
    CallItem,
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
    WeaponSmith,
    ElementNone,
    ElementFire,
    ElementWind,
    ElementThunder,
    ElementIce,
    ElementLight,
    ElementDark,
    Rare1,
    Rare2,
    Rare3,
    Rare4,
    Rare5,
    Rare6,
    Money,
    Weapon,
    SpecialWeapon,
    Favorite,
    NoFavorite,
    Alchemist,
    Drilled,
    WeaponMaterial,
    Rare7,
    LevelMax,
    OtherLevelMax,
    AttackTypeNone,
    AttackTypeSlashing,
    AttackTypeBlow,
    AttackTypePiercing,
    AttackTypeShooting,
    AttackTypeMagic,
    Present,
    Equipment,
    NonEquipment,
    WeaponGear,
    ReisouGear,
    Max,
  }
}
