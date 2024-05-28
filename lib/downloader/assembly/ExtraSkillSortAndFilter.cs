// Decompiled with JetBrains decompiler
// Type: ExtraSkillSortAndFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ExtraSkillSortAndFilter : SortAndFilter
{
  private readonly string[] SortLabelStr = new string[3]
  {
    Consts.GetInstance().SORT_POPUP_LABEL_NONE,
    Consts.GetInstance().SORT_POPUP_LABEL_LEVEL,
    Consts.GetInstance().SORT_POPUP_LABEL_GETORDER
  };
  [SerializeField]
  private ExtraSkillSortAndFilterButton[] OrderBtn;
  [SerializeField]
  private ExtraSkillSortAndFilterButton[] FilterBtns;
  [SerializeField]
  private ExtraSkillSortAndFilterButton[] SortBtns;
  [SerializeField]
  private ExtraSkillSortAndFilterTabButton[] ListBtns;
  [SerializeField]
  private GameObject[] ListObject;
  [SerializeField]
  protected UILabel targetSkillNum;
  private Unit004ExtraSkillListMenuBase menuBase;
  private ExtraSkillSortAndFilter.ModeTypes modeType;
  public Action SortFilterSkillNum;

  public void Initialize(Unit004ExtraSkillListMenuBase menu, bool setUI = false)
  {
    this.menuBase = menu;
    Persist<Persist.ExtraSkillSortAndFilterInfo> persist = this.menuBase.GetPersist();
    if (persist != null)
    {
      try
      {
        this.menuBase.SortCategory = persist.Data.sortType;
        if (!((IEnumerable<ExtraSkillSortAndFilterButton>) this.SortBtns).Any<ExtraSkillSortAndFilterButton>((Func<ExtraSkillSortAndFilterButton, bool>) (x => x.SortType == this.menuBase.SortCategory)))
          this.menuBase.SortCategory = this.SortBtns[0].SortType;
        this.menuBase.OrderBuySort = persist.Data.order;
        int num = this.FilterBtns == null ? 1 : (this.FilterBtns.Length == 0 ? 1 : 0);
        for (int index = 0; index < 25; ++index)
          this.menuBase.Filter.SetValue((object) false, index);
        foreach (ExtraSkillSortAndFilterButton filterBtn in this.FilterBtns)
          this.menuBase.Filter.SetValue((object) persist.Data.filter[(int) filterBtn.FilterType], (int) filterBtn.FilterType);
      }
      catch
      {
        persist.Delete();
        this.menuBase.SortCategory = this.SortBtns[0].SortType;
        this.menuBase.OrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
        foreach (ExtraSkillSortAndFilterButton filterBtn in this.FilterBtns)
          this.menuBase.Filter.SetValue((object) false, (int) filterBtn.FilterType);
      }
    }
    else
    {
      this.menuBase.SortCategory = ExtraSkillSortAndFilter.SORT_TYPES.Level;
      this.menuBase.OrderBuySort = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
      for (int index = 0; index < 25; ++index)
        this.menuBase.Filter.SetValue((object) true, index);
    }
    if (!setUI)
      return;
    this.SetOrderTypeBtn();
    this.SetValueWindow();
    this.SetListTypeBtn(persist.Data.modeType);
  }

  private void SetSortTabLabel()
  {
    this.ListBtns[0].SetText(this.SortLabelStr[(int) this.menuBase.SortCategory]);
    if (this.modeType == ExtraSkillSortAndFilter.ModeTypes.Sort)
      this.ListBtns[0].SetTextColor(new Color(1f, 1f, 0.0f));
    else
      this.ListBtns[0].SetTextColor(new Color(0.5f, 0.5f, 0.0f));
  }

  private void SetFilterTabLabel()
  {
    bool flag = false;
    for (int index = 0; index < 25; ++index)
    {
      if (index != 0 && this.menuBase.Filter[index])
      {
        flag = true;
        break;
      }
    }
    this.ListBtns[1].SetText(flag ? Consts.GetInstance().SORT_POPUP_LABEL_FILTER_ON : Consts.GetInstance().SORT_POPUP_LABEL_FILTER_OFF);
    if (this.modeType == ExtraSkillSortAndFilter.ModeTypes.Filter)
    {
      if (flag)
        this.ListBtns[1].SetTextColor(new Color(1f, 1f, 0.0f));
      else
        this.ListBtns[1].SetTextColor(new Color(1f, 1f, 1f));
      if (this.SortFilterSkillNum == null)
        return;
      this.SortFilterSkillNum();
    }
    else if (flag)
      this.ListBtns[1].SetTextColor(new Color(0.5f, 0.5f, 0.0f));
    else
      this.ListBtns[1].SetTextColor(new Color(0.5f, 0.5f, 0.5f));
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

  public void SetSkillNum(List<InventoryExtraSkill> displayList, List<InventoryExtraSkill> allList)
  {
    int count1 = displayList.Count;
    int count2 = allList.Count;
    foreach (InventoryExtraSkill display in displayList)
    {
      if (display.removeButton)
      {
        --count1;
        --count2;
        break;
      }
    }
    this.targetSkillNum.SetText((count1 <= 0 ? "[FF0000]" : "[FFFE27]") + (object) count1 + "[-]/" + (object) count2);
  }

  public void SetValueWindow()
  {
    this.SetSortTabLabel();
    this.SetFilterTabLabel();
    foreach (ExtraSkillSortAndFilterButton sortBtn in this.SortBtns)
    {
      sortBtn.SpriteColorGray(false);
      sortBtn.TextColorGray(false);
      if (sortBtn.SortType == this.menuBase.SortCategory)
      {
        sortBtn.SpriteColorGray(true);
        sortBtn.TextColorGray(true);
      }
    }
    foreach (ExtraSkillSortAndFilterButton filterBtn in this.FilterBtns)
    {
      if (!Object.op_Equality((Object) filterBtn, (Object) null) && ((Component) filterBtn).gameObject.activeSelf)
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

  private void SetListTypeBtn(ExtraSkillSortAndFilter.ModeTypes mode)
  {
    if (Object.op_Inequality((Object) this.ListBtns[0], (Object) null) && Object.op_Inequality((Object) this.ListBtns[1], (Object) null))
    {
      this.modeType = mode;
      ((IEnumerable<GameObject>) this.ListObject).ToggleOnceEx((int) this.modeType);
      if (mode == ExtraSkillSortAndFilter.ModeTypes.Sort)
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
      }
    }
    else
    {
      this.modeType = ExtraSkillSortAndFilter.ModeTypes.Sort;
      ((IEnumerable<GameObject>) this.ListObject).ToggleOnceEx((int) this.modeType);
    }
    this.SetSortTabLabel();
    this.SetFilterTabLabel();
  }

  public void SetSortCategory(ExtraSkillSortAndFilter.SORT_TYPES type)
  {
    this.menuBase.SortCategory = type;
    this.SetValueWindow();
  }

  public void SetFilterType(ExtraSkillSortAndFilter.FILTER_TYPES type, bool flg)
  {
    this.menuBase.Filter[(int) type] = flg;
    this.SetValueWindow();
  }

  public void IbtnSortBtn() => this.SetListTypeBtn(ExtraSkillSortAndFilter.ModeTypes.Sort);

  public void IbtnFilterBtn() => this.SetListTypeBtn(ExtraSkillSortAndFilter.ModeTypes.Filter);

  public override void IbtnDicision()
  {
    this.SaveData();
    if (Object.op_Inequality((Object) this.menuBase, (Object) null))
      this.menuBase.Sort(this.menuBase.SortCategory, this.menuBase.OrderBuySort);
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
    Persist<Persist.ExtraSkillSortAndFilterInfo> persist = this.menuBase.GetPersist();
    if (persist == null)
      return;
    persist.Data.order = this.menuBase.OrderBuySort;
    persist.Data.sortType = this.menuBase.SortCategory;
    persist.Data.modeType = this.modeType;
    for (int index = 0; index < 25; ++index)
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
      this.menuBase.Filter[(int) this.FilterBtns[index].FilterType] = this.FilterBtns[index].FilterType != ExtraSkillSortAndFilter.FILTER_TYPES.Favorite && this.FilterBtns[index].FilterType != ExtraSkillSortAndFilter.FILTER_TYPES.NoFavorite && this.FilterBtns[index].FilterType != ExtraSkillSortAndFilter.FILTER_TYPES.Equipment && this.FilterBtns[index].FilterType != ExtraSkillSortAndFilter.FILTER_TYPES.OtherThenEquipment && this.FilterBtns[index].FilterType != ExtraSkillSortAndFilter.FILTER_TYPES.LevelMax && this.FilterBtns[index].FilterType != ExtraSkillSortAndFilter.FILTER_TYPES.OtherThenLevelMax;
    this.SetValueWindow();
  }

  public override void IbtnFilterClear()
  {
    for (int index = 0; index < 25; ++index)
      this.menuBase.Filter[index] = false;
    this.SetValueWindow();
  }

  public static UISprite SortSpriteLabel(
    ExtraSkillSortAndFilter.SORT_TYPES type,
    UISprite SortSprite)
  {
    string str1 = ExtraSkillSortAndFilter.SetSortLabelSpriteName(type);
    if (!string.IsNullOrEmpty(str1))
    {
      string str2 = string.Format("slc_Label_{0}.png__GUI__unit_title_short__unit_title_short_prefab", (object) str1);
      UISpriteData sprite = SortSprite.atlas.GetSprite(str2);
      if (sprite == null)
        return SortSprite;
      SortSprite.spriteName = str2;
      ((Component) SortSprite).GetComponent<UIWidget>().SetDimensions(sprite.width, sprite.height);
    }
    return SortSprite;
  }

  private static string SetSortLabelSpriteName(ExtraSkillSortAndFilter.SORT_TYPES type)
  {
    string str = "";
    switch (type)
    {
      case ExtraSkillSortAndFilter.SORT_TYPES.Level:
        str = "Lv";
        break;
      case ExtraSkillSortAndFilter.SORT_TYPES.GetOrder:
        str = "GetOrder";
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
    Level,
    GetOrder,
  }

  public enum FILTER_TYPES
  {
    None,
    LevelMax,
    OtherThenLevelMax,
    Equipment,
    OtherThenEquipment,
    Debuff,
    Buff,
    Attack,
    Defance,
    Heal,
    Ailment,
    Favorite,
    NoFavorite,
    Trust,
    SecondIllusion,
    SecondDevil,
    SecondAngel,
    SecondBeast,
    SecondFairy,
    Dress,
    SecondCommand,
    ThirdIntegral,
    School,
    ThirdImitate,
    FourthRagnarok,
    Max,
  }
}
