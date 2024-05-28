// Decompiled with JetBrains decompiler
// Type: RecipeSortAndFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class RecipeSortAndFilter : SortAndFilter
{
  [SerializeField]
  private RecipeSortAndFilter.FILTER_TYPES[] ForceEnableFilterType;
  [SerializeField]
  private RecipeSortAndFilterButton[] OrderBtn;
  [SerializeField]
  private RecipeSortAndFilterButton[] FilterBtns;
  [SerializeField]
  private RecipeSortAndFilterButton[] SortBtns;
  [SerializeField]
  private RecipeSortAndFilterButton[] ListBtns;
  [SerializeField]
  private GameObject[] ListObject;
  [SerializeField]
  private UILabel targetItemNum;
  [SerializeField]
  private UILabel sortLabel;
  [SerializeField]
  private UILabel filterLabel;
  private RecipeSortAndFilter.MODE_TYPES modeType;
  [SerializeField]
  private SpreadColorButton isRecipeExistBtn;
  private Persist.RecipeSortAndFilterInfo info;
  private Action<List<bool>> onUpdateFilter;
  private Action<Persist.RecipeSortAndFilterInfo> onDicision;
  private Dictionary<RecipeSortAndFilter.SORT_TYPES, string> sortLabelDic = new Dictionary<RecipeSortAndFilter.SORT_TYPES, string>()
  {
    {
      RecipeSortAndFilter.SORT_TYPES.Recommended,
      "おすすめ"
    },
    {
      RecipeSortAndFilter.SORT_TYPES.BranchOfWeapon,
      "武具種"
    },
    {
      RecipeSortAndFilter.SORT_TYPES.Rarity,
      "レアリティ"
    },
    {
      RecipeSortAndFilter.SORT_TYPES.Name,
      "武具名"
    }
  };

  public void Initialize(
    Persist.RecipeSortAndFilterInfo info,
    Action<List<bool>> onUpdateFilter,
    Action<Persist.RecipeSortAndFilterInfo> onDicision)
  {
    this.info = CopyUtil.DeepCopy<Persist.RecipeSortAndFilterInfo>(info);
    this.onUpdateFilter = onUpdateFilter;
    this.onDicision = onDicision;
    this.SetOrderTypeBtn(info.order);
    this.SetValueWindow(info.sortType, info.filter);
    this.SetListTypeBtn(info.modeType);
    this.SetSortTabLabel(info.sortType);
    this.SetFilterTabLabel(info.filter);
    this.SetSortFilterLabelColor();
    this.InitColorRecipeExist(info.isRecipeExist);
  }

  private void SetOrderTypeBtn(SortAndFilter.SORT_TYPE_ORDER_BUY order)
  {
    if (order == SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING)
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

  public void SetItemNum(int displaynum, int allnum)
  {
    this.targetItemNum.SetText(((Func<int, string>) (num => num <= 0 ? "[FF0000]" : "[FFFE27]"))(displaynum) + (object) displaynum + "[-]/" + (object) allnum);
  }

  public void SetValueWindow(RecipeSortAndFilter.SORT_TYPES sortType, List<bool> filter)
  {
    this.SetSortButtonColor(sortType);
    this.SetFilterButtonColor(filter);
    this.SetSortFilterLabelColor();
  }

  private void SetSortButtonColor(RecipeSortAndFilter.SORT_TYPES sortType)
  {
    foreach (RecipeSortAndFilterButton sortBtn in this.SortBtns)
    {
      sortBtn.SpriteColorGray(false);
      sortBtn.TextColorGray(false);
      if (sortBtn.SortType == sortType)
      {
        sortBtn.SpriteColorGray(true);
        sortBtn.TextColorGray(true);
      }
    }
  }

  private void SetFilterButtonColor(List<bool> filter)
  {
    foreach (RecipeSortAndFilterButton filterBtn in this.FilterBtns)
    {
      if (!Object.op_Equality((Object) filterBtn, (Object) null))
      {
        filterBtn.SpriteColorGray(false);
        filterBtn.TextColorGray(false);
        if (filter[(int) filterBtn.FilterType])
        {
          filterBtn.SpriteColorGray(true);
          filterBtn.TextColorGray(true);
        }
      }
    }
  }

  private void SetSortFilterLabelColor()
  {
    bool flag = this.info.filter.Any<bool>((Func<bool, bool>) (v => v));
    if (this.modeType == RecipeSortAndFilter.MODE_TYPES.Sort)
    {
      ((UIWidget) this.sortLabel).color = new Color(1f, 1f, 0.0f);
      ((UIWidget) this.filterLabel).color = flag ? new Color(0.5f, 0.5f, 0.0f) : new Color(0.5f, 0.5f, 0.5f);
    }
    else
    {
      ((UIWidget) this.sortLabel).color = new Color(0.5f, 0.5f, 0.0f);
      ((UIWidget) this.filterLabel).color = flag ? new Color(1f, 1f, 0.0f) : new Color(1f, 1f, 1f);
    }
  }

  private void SetListTypeBtn(RecipeSortAndFilter.MODE_TYPES mode)
  {
    if (Object.op_Inequality((Object) this.ListBtns[0], (Object) null) && Object.op_Inequality((Object) this.ListBtns[1], (Object) null))
    {
      this.modeType = mode;
      ((IEnumerable<GameObject>) this.ListObject).ToggleOnceEx((int) this.modeType);
      if (mode == RecipeSortAndFilter.MODE_TYPES.Sort)
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
        if (this.onUpdateFilter != null)
          this.onUpdateFilter(this.info.filter);
      }
    }
    else
    {
      this.modeType = RecipeSortAndFilter.MODE_TYPES.Sort;
      ((IEnumerable<GameObject>) this.ListObject).ToggleOnceEx((int) this.modeType);
    }
    this.info.modeType = mode;
  }

  public void SetSortCategory(RecipeSortAndFilter.SORT_TYPES sortType)
  {
    this.info.sortType = sortType;
    this.SetValueWindow(sortType, this.info.filter);
    this.SetSortTabLabel(sortType);
  }

  public void SetFilterType(RecipeSortAndFilter.FILTER_TYPES type, bool flg)
  {
    this.info.filter[(int) type] = flg;
    this.SetFilterButtonColor(this.info.filter);
    this.SetSortFilterLabelColor();
    this.SetValueWindow(this.info.sortType, this.info.filter);
    if (this.onUpdateFilter != null)
      this.onUpdateFilter(this.info.filter);
    this.SetFilterTabLabel(this.info.filter);
  }

  private void SetSortTabLabel(RecipeSortAndFilter.SORT_TYPES sortTypes)
  {
    this.sortLabel.SetText(this.sortLabelDic[sortTypes]);
  }

  private void SetFilterTabLabel(List<bool> filter)
  {
    if (filter.Any<bool>((Func<bool, bool>) (v => v)))
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
    this.SetListTypeBtn(RecipeSortAndFilter.MODE_TYPES.Sort);
    this.SetSortFilterLabelColor();
  }

  public void IbtnFilterBtn()
  {
    this.SetListTypeBtn(RecipeSortAndFilter.MODE_TYPES.Filter);
    this.SetSortFilterLabelColor();
  }

  public override void IbtnDicision()
  {
    if (this.onDicision == null)
      return;
    this.onDicision(this.info);
  }

  public override void IbtnOrder()
  {
    this.info.order = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.SetOrderTypeBtn(SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING);
  }

  public override void IbtnOrderDec()
  {
    this.info.order = SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING;
    this.SetOrderTypeBtn(SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING);
  }

  public override void IbtnAllFilterSelect()
  {
    for (int index = 0; index < this.FilterBtns.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.FilterBtns[index], (Object) null))
        this.info.filter[(int) this.FilterBtns[index].FilterType] = true;
    }
    if (this.ForceEnableFilterType != null && this.ForceEnableFilterType.Length != 0)
    {
      foreach (int index in this.ForceEnableFilterType)
        this.info.filter[index] = true;
    }
    this.SetValueWindow(this.info.sortType, this.info.filter);
    if (this.onUpdateFilter != null)
      this.onUpdateFilter(this.info.filter);
    this.SetFilterTabLabel(this.info.filter);
  }

  public override void IbtnFilterClear()
  {
    for (int index = 0; index < 24; ++index)
      this.info.filter[index] = false;
    this.SetValueWindow(this.info.sortType, this.info.filter);
    if (this.onUpdateFilter != null)
      this.onUpdateFilter(this.info.filter);
    this.SetFilterTabLabel(this.info.filter);
  }

  public void IbtnRecipeExist()
  {
    this.info.isRecipeExist = !this.info.isRecipeExist;
    this.InitColorRecipeExist(this.info.isRecipeExist);
  }

  private void InitColorRecipeExist(bool enabled)
  {
    this.isRecipeExistBtn.SetColor(enabled ? Color.white : Color.gray);
  }

  public static UISprite SortSpriteLabel(RecipeSortAndFilter.SORT_TYPES type, UISprite SortSprite)
  {
    string str = RecipeSortAndFilter.SetSortLabelSpriteName(type);
    if (!string.IsNullOrEmpty(str))
    {
      UISpriteData sprite = SortSprite.atlas.GetSprite(str);
      if (sprite == null)
        return SortSprite;
      SortSprite.spriteName = str;
      ((Component) SortSprite).GetComponent<UIWidget>().SetDimensions(sprite.width, sprite.height);
    }
    return SortSprite;
  }

  private static string SetSortLabelSpriteName(RecipeSortAndFilter.SORT_TYPES type)
  {
    Func<string, string> func = (Func<string, string>) (v => "slc_button_text_" + v + "_22pt.png__GUI__button_text__button_text_prefab");
    switch (type)
    {
      case RecipeSortAndFilter.SORT_TYPES.Recommended:
        return "slc_recommend.png__GUI__button_text__button_text_prefab";
      case RecipeSortAndFilter.SORT_TYPES.BranchOfWeapon:
        return func("weapontype");
      case RecipeSortAndFilter.SORT_TYPES.Rarity:
        return func("sort_rare");
      case RecipeSortAndFilter.SORT_TYPES.Name:
        return func("sort_bugu_name");
      default:
        return "";
    }
  }

  public enum MODE_TYPES
  {
    Sort,
    Filter,
  }

  public enum SORT_TYPES
  {
    None,
    Recommended,
    BranchOfWeapon,
    Rarity,
    Name,
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
    AttackTypeNone,
    AttackTypeSlashing,
    AttackTypeBlow,
    AttackTypePiercing,
    AttackTypeShooting,
    AttackTypeMagic,
    Weapon,
    SpecialWeapon,
    Max,
  }
}
