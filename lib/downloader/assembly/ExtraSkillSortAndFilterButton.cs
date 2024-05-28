// Decompiled with JetBrains decompiler
// Type: ExtraSkillSortAndFilterButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ExtraSkillSortAndFilterButton : SortAndFilterButton
{
  [SerializeField]
  private ExtraSkillSortAndFilterButton OppositeBtn;
  [SerializeField]
  private ExtraSkillSortAndFilter.ModeTypes modelType;
  [SerializeField]
  private ExtraSkillSortAndFilter menu;
  [SerializeField]
  private ExtraSkillSortAndFilter.SORT_TYPES sortType;
  [SerializeField]
  private bool isAwakeSkillCategory;
  [SerializeField]
  private AwakeSkillCategory.Type awakeSkillType;
  [SerializeField]
  private ExtraSkillSortAndFilter.FILTER_TYPES filterType;
  [SerializeField]
  private UISprite[] LabelSprite;

  public ExtraSkillSortAndFilter.SORT_TYPES SortType => this.sortType;

  public ExtraSkillSortAndFilter.FILTER_TYPES FilterType => this.filterType;

  protected override void Awake() => base.Awake();

  public bool isShow()
  {
    if (!this.isAwakeSkillCategory)
      return true;
    AwakeSkillCategory awakeSkillCategory = ((IEnumerable<AwakeSkillCategory>) MasterData.AwakeSkillCategoryList).FirstOrDefault<AwakeSkillCategory>((Func<AwakeSkillCategory, bool>) (x => (AwakeSkillCategory.Type) x.ID == this.awakeSkillType));
    if (awakeSkillCategory == null)
      return true;
    DateTime? startAt = awakeSkillCategory.start_at;
    DateTime dateTime = ServerTime.NowAppTime();
    return startAt.HasValue && startAt.GetValueOrDefault() < dateTime;
  }

  private void Update()
  {
    foreach (UIWidget uiWidget in this.LabelSprite)
      uiWidget.color = ((UIWidget) this.Sprite).color;
  }

  public void TextColorGray(bool flag)
  {
    Color color = Color.gray;
    if (flag)
      color = Color.white;
    foreach (UIWidget uiWidget in this.LabelSprite)
      uiWidget.color = color;
  }

  public override void PressButton()
  {
    switch (this.modelType)
    {
      case ExtraSkillSortAndFilter.ModeTypes.Sort:
        this.menu.SetSortCategory(this.sortType);
        break;
      case ExtraSkillSortAndFilter.ModeTypes.Filter:
        this.menu.SetFilterType(this.filterType, Color.op_Equality(((UIButtonColor) this.Button).defaultColor, Color.gray));
        break;
    }
    if (!Object.op_Inequality((Object) this.OppositeBtn, (Object) null) || !Color.op_Equality(((UIButtonColor) this.Button).defaultColor, Color.white) || !Color.op_Equality(((UIButtonColor) this.OppositeBtn.Button).defaultColor, Color.white))
      return;
    this.OppositeBtn.PressButton();
  }
}
