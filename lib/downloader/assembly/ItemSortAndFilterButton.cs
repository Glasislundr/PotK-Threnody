// Decompiled with JetBrains decompiler
// Type: ItemSortAndFilterButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ItemSortAndFilterButton : SortAndFilterButton
{
  [SerializeField]
  private ItemSortAndFilterButton OppositeBtn;
  [SerializeField]
  private ItemSortAndFilter.ModeTypes modelType;
  [SerializeField]
  private ItemSortAndFilter menu;
  [SerializeField]
  private ItemSortAndFilter.SORT_TYPES sortType;
  [SerializeField]
  private ItemSortAndFilter.FILTER_TYPES filterType;
  [SerializeField]
  private UISprite[] LabelSprite;

  public ItemSortAndFilter.SORT_TYPES SortType => this.sortType;

  public ItemSortAndFilter.FILTER_TYPES FilterType => this.filterType;

  protected override void Awake() => base.Awake();

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
      case ItemSortAndFilter.ModeTypes.Sort:
        this.menu.SetSortCategory(this.sortType);
        break;
      case ItemSortAndFilter.ModeTypes.Filter:
        this.menu.SetFilterType(this.filterType, Color.op_Equality(((UIButtonColor) this.Button).defaultColor, Color.gray));
        break;
    }
    if (!Object.op_Inequality((Object) this.OppositeBtn, (Object) null) || !Color.op_Equality(((UIButtonColor) this.Button).defaultColor, Color.white) || !Color.op_Equality(((UIButtonColor) this.OppositeBtn.Button).defaultColor, Color.white))
      return;
    this.OppositeBtn.PressButton();
  }
}
