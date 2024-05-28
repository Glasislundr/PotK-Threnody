// Decompiled with JetBrains decompiler
// Type: GuildMemberSort
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GuildMemberSort : SortAndFilter
{
  [SerializeField]
  private GuildMemberSortButton[] OrderBtn;
  [SerializeField]
  private GuildMemberSortButton[] SortBtns;
  private GuildMemberListBase listBase;
  private static readonly string labelContribution = "contribution";
  private static readonly string labelGuildJoin = "guild_join";
  private static readonly string labelLastPlay = "last_play";
  private static readonly string labelLevel = "Lv";

  public void Initialize(GuildMemberListBase list)
  {
    this.listBase = list;
    Persist<Persist.GuildMemberSortInfo> persist = this.listBase.GetPersist();
    if (persist != null)
    {
      try
      {
        this.listBase.SortType = persist.Data.sortType;
        this.listBase.OrderSortType = persist.Data.order;
      }
      catch
      {
        persist.Delete();
        this.listBase.SortType = GuildMemberSort.SORT_TYPES.Contribution;
        this.listBase.OrderSortType = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
      }
    }
    else
    {
      this.listBase.SortType = GuildMemberSort.SORT_TYPES.Contribution;
      this.listBase.OrderSortType = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    }
    this.SetOrderTypeBtn();
    this.SetValueWindow();
  }

  private void SetOrderTypeBtn()
  {
    if (this.listBase.OrderSortType == SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING)
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

  public void SetValueWindow()
  {
    foreach (GuildMemberSortButton sortBtn in this.SortBtns)
    {
      sortBtn.SpriteColorGray(false);
      sortBtn.TextColorGray(false);
      if (sortBtn.SortType == this.listBase.SortType)
      {
        sortBtn.SpriteColorGray(true);
        sortBtn.TextColorGray(true);
      }
    }
  }

  public void SetSortCategory(GuildMemberSort.SORT_TYPES type)
  {
    this.listBase.SortType = type;
    this.SetValueWindow();
  }

  public override void SaveData()
  {
    Persist<Persist.GuildMemberSortInfo> persist = this.listBase.GetPersist();
    if (persist == null)
      return;
    persist.Data.sortType = this.listBase.SortType;
    persist.Data.order = this.listBase.OrderSortType;
    persist.Flush();
  }

  public override void IbtnClose()
  {
    if (this.IsPush)
      return;
    ((Component) this).gameObject.GetComponent<NGTweenParts>().isActive = false;
  }

  public override void onBackButton() => this.IbtnClose();

  public override void IbtnOrder()
  {
    this.listBase.OrderSortType = SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING;
    this.SetOrderTypeBtn();
  }

  public override void IbtnOrderDec()
  {
    this.listBase.OrderSortType = SortAndFilter.SORT_TYPE_ORDER_BUY.DESCENDING;
    this.SetOrderTypeBtn();
  }

  public override void IbtnDicision()
  {
    this.SaveData();
    if (Object.op_Inequality((Object) this.listBase, (Object) null))
      this.listBase.Sort(this.listBase.SortType, this.listBase.OrderSortType);
    ((Component) this).gameObject.GetComponent<NGTweenParts>().isActive = false;
  }

  public UISprite SortSpriteLabel(GuildMemberSort.SORT_TYPES type, UISprite SortSprite)
  {
    string str1 = this.SetSortLabelSpriteName(type);
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

  private string SetSortLabelSpriteName(GuildMemberSort.SORT_TYPES type)
  {
    string str = string.Empty;
    switch (type)
    {
      case GuildMemberSort.SORT_TYPES.Contribution:
        str = GuildMemberSort.labelContribution;
        break;
      case GuildMemberSort.SORT_TYPES.Level:
        str = GuildMemberSort.labelLevel;
        break;
      case GuildMemberSort.SORT_TYPES.LastLoginAt:
        str = GuildMemberSort.labelLastPlay;
        break;
      case GuildMemberSort.SORT_TYPES.JoinAt:
        str = GuildMemberSort.labelGuildJoin;
        break;
    }
    return str;
  }

  public enum SORT_TYPES
  {
    None,
    Contribution,
    Level,
    LastLoginAt,
    JoinAt,
  }
}
