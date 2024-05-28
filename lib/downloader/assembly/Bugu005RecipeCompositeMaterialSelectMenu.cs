// Decompiled with JetBrains decompiler
// Type: Bugu005RecipeCompositeMaterialSelectMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class Bugu005RecipeCompositeMaterialSelectMenu : Bugu005SelectItemListMenuBase
{
  private const float BOTTOM_ICON_SCALE = 0.6617647f;
  [SerializeField]
  private Transform[] buguRecipePosition;
  [SerializeField]
  private UIButton ibtnYes;
  private bool firstInit = true;
  private ItemIcon[] icons;
  private GearGear mainGear;
  private List<GearCombineRecipe> allGearRecipes;
  private List<GameCore.ItemInfo> originalSelectedItems;

  public void SetMainGear(GearGear gear) => this.mainGear = gear;

  public void SetAllGearRecipes(List<GearCombineRecipe> gearRecipes)
  {
    this.allGearRecipes = gearRecipes;
  }

  public void SetOriginalSelectedItem(List<GameCore.ItemInfo> items)
  {
    this.originalSelectedItems = items;
  }

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu0052CompositeSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !x.broken)).ToList<PlayerItem>();
  }

  protected override List<PlayerMaterialGear> GetMaterialList()
  {
    List<PlayerMaterialGear> list = new List<PlayerMaterialGear>();
    ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x =>
    {
      if (x.isDilling() || x.isSpecialDilling())
        return false;
      return x.isCompse() || x.isWeaponMaterial();
    })).ForEach<PlayerMaterialGear>((Action<PlayerMaterialGear>) (x =>
    {
      int num = x.quantity < this.SelectMax ? x.quantity : this.SelectMax;
      for (int index = 0; index < num; ++index)
        list.Add(x);
    }));
    return list;
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem displayItem = this.DisplayItems[inventoryItemIdx];
    itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.SelectItemProc(playeritem.ItemInfo));
    if (displayItem.Item.isSupply || displayItem.Item.isExchangable || displayItem.Item.isCompse || displayItem.Item.isWeaponMaterial)
    {
      itemIcon.QuantitySupply = true;
      itemIcon.EnableQuantity(displayItem.Item.quantity);
    }
    else
      itemIcon.QuantitySupply = false;
    itemIcon.ForBattle = displayItem.Item.ForBattle;
    itemIcon.Favorite = displayItem.Item.favorite;
    itemIcon.Gray = this.IsGrayIcon(displayItem);
    if (this.DisableTouchIcon(displayItem))
      displayItem.Gray = true;
    if (displayItem.select)
    {
      if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Check)
      {
        itemIcon.SelectByCheckIcon();
        itemIcon.SelectedQuantity(displayItem.selectCount);
      }
      else
        itemIcon.Select(displayItem.index - 1);
    }
    else
    {
      itemIcon.SelectedQuantity(0);
      if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Check)
        itemIcon.DeselectByCheckIcon();
      else
        itemIcon.Deselect();
    }
    itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(((Bugu005ItemListMenuBase) this).ChangeDetailScene));
  }

  protected override IEnumerator InitExtension()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu005RecipeCompositeMaterialSelectMenu materialSelectMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    int length = materialSelectMenu.buguRecipePosition.Length;
    materialSelectMenu.icons = new ItemIcon[length];
    for (int index = 0; index < length; ++index)
    {
      materialSelectMenu.buguRecipePosition[index].Clear();
      GameObject gameObject = materialSelectMenu.ItemIconPrefab.Clone(materialSelectMenu.buguRecipePosition[index]);
      gameObject.transform.localScale = new Vector3(0.6617647f, 0.6617647f);
      ItemIcon component = gameObject.GetComponent<ItemIcon>();
      materialSelectMenu.icons[index] = component;
    }
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    foreach (InventoryItem inventoryItem in materialSelectMenu.InventoryItems)
    {
      if (!materialSelectMenu.isInRecipeList(inventoryItem))
        inventoryItemList.Add(inventoryItem);
    }
    foreach (InventoryItem inventoryItem in inventoryItemList)
      materialSelectMenu.InventoryItems.Remove(inventoryItem);
    if (materialSelectMenu.firstInit)
    {
      List<InventoryItem> invItem = new List<InventoryItem>();
      foreach (InventoryItem inventoryItem in materialSelectMenu.InventoryItems)
        invItem.Add(inventoryItem);
      List<GameCore.ItemInfo> itemInfoList = new List<GameCore.ItemInfo>();
      foreach (GameCore.ItemInfo originalSelectedItem in materialSelectMenu.originalSelectedItems)
      {
        InventoryItem byItem = invItem.FindByItem(originalSelectedItem);
        if (byItem != null)
        {
          itemInfoList.Add(byItem.Item);
          invItem.Remove(byItem);
        }
      }
      materialSelectMenu.originalSelectedItems.Clear();
      materialSelectMenu.originalSelectedItems = itemInfoList;
      materialSelectMenu.SelectItemList.Clear();
      if (materialSelectMenu.originalSelectedItems != null && materialSelectMenu.originalSelectedItems.Count > 0)
      {
        foreach (GameCore.ItemInfo findItem in materialSelectMenu.originalSelectedItems.ToList<GameCore.ItemInfo>())
        {
          if (findItem != null)
          {
            InventoryItem byItemIndex = materialSelectMenu.InventoryItems.FindByItemIndex(findItem);
            materialSelectMenu.PrimarySelectedItemList.Add(byItemIndex);
            materialSelectMenu.AddSelectItem(byItemIndex);
            if (byItemIndex.Item.favorite || byItemIndex.Item.ForBattle)
              materialSelectMenu.RemoveSelectItem(byItemIndex);
          }
        }
        materialSelectMenu.DisplayIconAndBottomInfoUpdate();
      }
      materialSelectMenu.firstInit = false;
    }
    else if (materialSelectMenu.SelectItemList != null && materialSelectMenu.SelectItemList.Count > 0)
    {
      List<InventoryItem> list = materialSelectMenu.SelectItemList.ToList<InventoryItem>();
      materialSelectMenu.SelectItemList.Clear();
      foreach (InventoryItem inventoryItem in list)
      {
        if (inventoryItem == null)
        {
          materialSelectMenu.SelectItemList.Add((InventoryItem) null);
        }
        else
        {
          InventoryItem byItemIndex = materialSelectMenu.InventoryItems.FindByItemIndex(inventoryItem.Item);
          materialSelectMenu.AddSelectItem(byItemIndex);
          if (byItemIndex.Item.favorite || byItemIndex.Item.ForBattle)
            materialSelectMenu.RemoveSelectItem(byItemIndex);
        }
      }
      materialSelectMenu.DisplayIconAndBottomInfoUpdate();
    }
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    return false;
  }

  private GearGear OriginalRecipeGear(int index)
  {
    GearCombineRecipe gearRecipe = this.allGearRecipes[0];
    List<GearGear> gearGearList = new List<GearGear>();
    gearGearList.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x => x.group_id == gearRecipe.material1_gear_id)));
    if (gearRecipe.material2_gear_id.HasValue)
      gearGearList.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material2GearId = gearRecipe.material2_gear_id;
        int valueOrDefault = material2GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material2GearId.HasValue;
      })));
    if (gearRecipe.material3_gear_id.HasValue)
      gearGearList.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material3GearId = gearRecipe.material3_gear_id;
        int valueOrDefault = material3GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material3GearId.HasValue;
      })));
    if (gearRecipe.material4_gear_id.HasValue)
      gearGearList.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material4GearId = gearRecipe.material4_gear_id;
        int valueOrDefault = material4GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material4GearId.HasValue;
      })));
    if (gearRecipe.material5_gear_id.HasValue)
      gearGearList.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material5GearId = gearRecipe.material5_gear_id;
        int valueOrDefault = material5GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material5GearId.HasValue;
      })));
    return gearGearList[index];
  }

  private int OriginalRankGear(int index)
  {
    GearCombineRecipe allGearRecipe = this.allGearRecipes[0];
    List<int> intList = new List<int>();
    intList.Add(this.SetRequestGearRank(allGearRecipe.material1_gear_rank));
    if (allGearRecipe.material2_gear_id.HasValue)
      intList.Add(this.SetRequestGearRank(allGearRecipe.material2_gear_rank));
    if (allGearRecipe.material3_gear_id.HasValue)
      intList.Add(this.SetRequestGearRank(allGearRecipe.material3_gear_rank));
    if (allGearRecipe.material4_gear_id.HasValue)
      intList.Add(this.SetRequestGearRank(allGearRecipe.material4_gear_rank));
    if (allGearRecipe.material5_gear_id.HasValue)
      intList.Add(this.SetRequestGearRank(allGearRecipe.material5_gear_rank));
    return intList[index];
  }

  private int SetRequestGearRank(int? rank) => rank.HasValue ? rank.Value : 0;

  private bool isInRecipeList(InventoryItem info)
  {
    GearGear gear = info.Item.gear;
    if (gear == null)
      return false;
    for (int i = 0; i < this.allGearRecipes.Count; i++)
    {
      int? nullable;
      if (this.allGearRecipes[i].material1_gear_id == gear.group_id)
      {
        nullable = this.allGearRecipes[i].material1_gear_rank;
        int gearLevel = info.Item.gearLevel;
        if (nullable.GetValueOrDefault() <= gearLevel & nullable.HasValue)
          return true;
      }
      if (this.allGearRecipes[i].material2_gear_id.HasValue && gear == ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material2GearId = this.allGearRecipes[i].material2_gear_id;
        int valueOrDefault = material2GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material2GearId.HasValue;
      })))
      {
        nullable = this.allGearRecipes[i].material2_gear_rank;
        int gearLevel = info.Item.gearLevel;
        if (nullable.GetValueOrDefault() <= gearLevel & nullable.HasValue)
          return true;
      }
      if (this.allGearRecipes[i].material3_gear_id.HasValue && gear == ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material3GearId = this.allGearRecipes[i].material3_gear_id;
        int valueOrDefault = material3GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material3GearId.HasValue;
      })))
      {
        nullable = this.allGearRecipes[i].material3_gear_rank;
        int gearLevel = info.Item.gearLevel;
        if (nullable.GetValueOrDefault() <= gearLevel & nullable.HasValue)
          return true;
      }
      if (this.allGearRecipes[i].material4_gear_id.HasValue && gear == ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material4GearId = this.allGearRecipes[i].material4_gear_id;
        int valueOrDefault = material4GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material4GearId.HasValue;
      })))
      {
        nullable = this.allGearRecipes[i].material4_gear_rank;
        int gearLevel = info.Item.gearLevel;
        if (nullable.GetValueOrDefault() <= gearLevel & nullable.HasValue)
          return true;
      }
      if (this.allGearRecipes[i].material5_gear_id.HasValue && gear == ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
      {
        int groupId = x.group_id;
        int? material5GearId = this.allGearRecipes[i].material5_gear_id;
        int valueOrDefault = material5GearId.GetValueOrDefault();
        return groupId == valueOrDefault & material5GearId.HasValue;
      })))
      {
        nullable = this.allGearRecipes[i].material5_gear_rank;
        int gearLevel = info.Item.gearLevel;
        if (nullable.GetValueOrDefault() <= gearLevel & nullable.HasValue)
          return true;
      }
    }
    return false;
  }

  protected override void AllItemIconUpdate() => base.AllItemIconUpdate();

  protected override void UpdateSelectItemIndex()
  {
    if (this.SelectMode != Bugu005SelectItemListMenuBase.SelectModeEnum.Num)
      return;
    int count = this.SelectItemList.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this.SelectItemList[index] != null)
        this.SelectItemList[index].index = index + 1;
    }
  }

  protected override void UpdateIcons()
  {
    for (int index = 0; index < this.SelectItemList.Count; ++index)
    {
      if (this.SelectItemList[index] != null)
      {
        this.SelectItemList[index].select = true;
        this.SelectItemList[index].Gray = true;
        if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Num)
          this.SelectItemList[index].index = index + 1;
      }
    }
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
  }

  protected override void AddSelectItem(InventoryItem invItem)
  {
    if (invItem == null || this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == invItem)))
      return;
    invItem.select = true;
    invItem.Gray = true;
    invItem.index = 0;
    if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Num)
      invItem.index = this.SelectItemList.Count<InventoryItem>() + 1;
    if (this.SelectItemList.Count < this.PrimarySelectedItemList.Count)
    {
      this.SelectItemList.Add(invItem);
    }
    else
    {
      for (int index = 0; index < this.SelectItemList.Count<InventoryItem>(); ++index)
      {
        if (this.SelectItemList[index] == null && this.PrimarySelectedItemList[index].Item.gear.group_id == invItem.Item.gear.group_id)
        {
          this.SelectItemList[index] = invItem;
          break;
        }
      }
    }
  }

  protected override void RemoveSelectItem(InventoryItem invItem)
  {
    if (invItem == null || !this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == invItem)))
      return;
    invItem.select = false;
    invItem.Gray = false;
    invItem.index = 0;
    this.SelectItemList[this.SelectItemList.IndexOf(invItem)] = (InventoryItem) null;
  }

  protected override void BottomInfoUpdate()
  {
    SMManager.Get<Player>();
    int count = this.SelectItemList != null ? this.SelectItemList.Count : 0;
    for (int index = 0; index < this.SelectMax; ++index)
    {
      if (index >= count)
      {
        this.icons[index].SetEmpty(true);
        this.icons[index].onClick = (Action<ItemIcon>) (playeritem => { });
      }
      else if (this.SelectItemList[index] != null)
      {
        this.icons[index].SetEmpty(false);
        if (ItemIcon.IsCache(this.SelectItemList[index].Item))
          this.icons[index].InitByItemInfoCache(this.SelectItemList[index].Item);
        else
          this.StartCoroutine(this.icons[index].InitByItemInfo(this.SelectItemList[index].Item));
        this.icons[index].onClick = (Action<ItemIcon>) (playeritem => { });
        this.icons[index].Gray = false;
        this.icons[index].EnableQuantity(0);
        this.icons[index].Deselect();
      }
      else
      {
        this.icons[index].SetEmpty(false);
        if (ItemIcon.IsCache(this.OriginalRecipeGear(index)))
        {
          GearGear gearGear = this.OriginalRecipeGear(index);
          this.icons[index].InitByGearCache(this.OriginalRecipeGear(index));
          this.icons[index].gear.rank.SetActive(false);
          if (!gearGear.isMaterial())
          {
            this.icons[index].gear.rank.SetActive(true);
            this.icons[index].gear.rank.GetComponent<UI2DSprite>().sprite2D = this.icons[index].rankSprite[this.OriginalRankGear(index) - 1];
          }
        }
        else
        {
          GearGear gear = this.OriginalRecipeGear(index);
          this.StartCoroutine(this.icons[index].InitByGear(gear));
          this.icons[index].gear.rank.SetActive(false);
          if (!gear.isMaterial())
          {
            this.icons[index].gear.rank.SetActive(true);
            this.icons[index].gear.rank.GetComponent<UI2DSprite>().sprite2D = this.icons[index].rankSprite[this.OriginalRankGear(index) - 1];
          }
        }
        this.icons[index].onClick = (Action<ItemIcon>) (playeritem => { });
        this.icons[index].Gray = true;
        this.icons[index].Deselect();
      }
    }
    ((UIButtonColor) this.ibtnYes).isEnabled = this.isSelectedItemAreInRecipe();
  }

  private bool isSelectedItemAreInRecipe()
  {
    List<GearGear> source1 = new List<GearGear>();
    foreach (InventoryItem selectItem in this.SelectItemList)
    {
      if (selectItem != null)
        source1.Add(selectItem.Item.gear);
    }
    for (int i = 0; i < this.allGearRecipes.Count; i++)
    {
      List<GearGear> source2 = new List<GearGear>();
      source2.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x => x.group_id == this.allGearRecipes[i].material1_gear_id)));
      if (this.allGearRecipes[i].material2_gear_id.HasValue)
        source2.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
        {
          int groupId = x.group_id;
          int? material2GearId = this.allGearRecipes[i].material2_gear_id;
          int valueOrDefault = material2GearId.GetValueOrDefault();
          return groupId == valueOrDefault & material2GearId.HasValue;
        })));
      if (this.allGearRecipes[i].material3_gear_id.HasValue)
        source2.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
        {
          int groupId = x.group_id;
          int? material3GearId = this.allGearRecipes[i].material3_gear_id;
          int valueOrDefault = material3GearId.GetValueOrDefault();
          return groupId == valueOrDefault & material3GearId.HasValue;
        })));
      if (this.allGearRecipes[i].material4_gear_id.HasValue)
        source2.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
        {
          int groupId = x.group_id;
          int? material4GearId = this.allGearRecipes[i].material4_gear_id;
          int valueOrDefault = material4GearId.GetValueOrDefault();
          return groupId == valueOrDefault & material4GearId.HasValue;
        })));
      if (this.allGearRecipes[i].material5_gear_id.HasValue)
        source2.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
        {
          int groupId = x.group_id;
          int? material5GearId = this.allGearRecipes[i].material5_gear_id;
          int valueOrDefault = material5GearId.GetValueOrDefault();
          return groupId == valueOrDefault & material5GearId.HasValue;
        })));
      if (this.UnorderedEqual<int>((ICollection<int>) source1.Select<GearGear, int>((Func<GearGear, int>) (x => x.group_id)).ToArray<int>(), (ICollection<int>) source2.Select<GearGear, int>((Func<GearGear, int>) (x => x.group_id)).ToArray<int>()))
        return true;
    }
    return false;
  }

  private bool UnorderedEqual<T>(ICollection<T> a, ICollection<T> b)
  {
    if (a.Count != b.Count)
      return false;
    Dictionary<T, int> dictionary = new Dictionary<T, int>();
    foreach (T key in (IEnumerable<T>) a)
    {
      int num;
      if (dictionary.TryGetValue(key, out num))
        dictionary[key] = num + 1;
      else
        dictionary.Add(key, 1);
    }
    foreach (T key in (IEnumerable<T>) b)
    {
      int num;
      if (!dictionary.TryGetValue(key, out num) || num == 0)
        return false;
      dictionary[key] = num - 1;
    }
    foreach (int num in dictionary.Values)
    {
      if (num != 0)
        return false;
    }
    return true;
  }

  protected override void SelectItemProc(GameCore.ItemInfo item)
  {
    InventoryItem byItemIndex = this.InventoryItems.FindByItemIndex(item);
    if (this.DisableTouchIcon(byItemIndex))
      return;
    if (byItemIndex != null)
    {
      if (byItemIndex.select)
        this.RemoveSelectItem(byItemIndex);
      else if (this.SelectItemList.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x != null)).ToList<InventoryItem>().Count < this.SelectMax)
        this.AddSelectItem(byItemIndex);
    }
    this.UpdateSelectItemIndexWithInfo();
  }

  protected override bool IsGrayIcon(InventoryItem item) => this.DisableTouchIcon(item);

  protected override bool DisableTouchIcon(InventoryItem item)
  {
    if (this.SelectItemList.Count<InventoryItem>() > 0 && item.Item.gear != null)
    {
      GearGear gear = item.Item.gear;
      if (gear != null)
      {
        if (!this.ItemCanBeSelected(item))
          return true;
        if (!gear.isComposeManaSeed())
        {
          if (base.DisableTouchIcon(item))
            return true;
          return item.Item.isWeapon && item.Item.isComposeManaSeed;
        }
        if (base.DisableTouchIcon(item))
          return true;
        return gear.isComposeManaSeed() && gear.ID != item.Item.gear.ID;
      }
    }
    return base.DisableTouchIcon(item);
  }

  private bool ItemCanBeSelected(InventoryItem item)
  {
    if (this.SelectItemList.Contains(item))
      return true;
    List<GearGear> selectItemGear = new List<GearGear>();
    foreach (InventoryItem selectItem in this.SelectItemList)
    {
      if (selectItem != null)
        selectItemGear.Add(selectItem.Item.gear);
    }
label_24:
    for (int i = 0; i < this.allGearRecipes.Count; i++)
    {
      List<GearGear> gearGearList = new List<GearGear>();
      gearGearList.Add(((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x => x.group_id == this.allGearRecipes[i].material1_gear_id)));
      if (this.allGearRecipes[i].material2_gear_id.HasValue)
      {
        GearGear gearGear = ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
        {
          int groupId = x.group_id;
          int? material2GearId = this.allGearRecipes[i].material2_gear_id;
          int valueOrDefault = material2GearId.GetValueOrDefault();
          return groupId == valueOrDefault & material2GearId.HasValue;
        }));
        gearGearList.Add(gearGear);
      }
      if (this.allGearRecipes[i].material3_gear_id.HasValue)
      {
        GearGear gearGear = ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
        {
          int groupId = x.group_id;
          int? material3GearId = this.allGearRecipes[i].material3_gear_id;
          int valueOrDefault = material3GearId.GetValueOrDefault();
          return groupId == valueOrDefault & material3GearId.HasValue;
        }));
        gearGearList.Add(gearGear);
      }
      if (this.allGearRecipes[i].material4_gear_id.HasValue)
      {
        GearGear gearGear = ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
        {
          int groupId = x.group_id;
          int? material4GearId = this.allGearRecipes[i].material4_gear_id;
          int valueOrDefault = material4GearId.GetValueOrDefault();
          return groupId == valueOrDefault & material4GearId.HasValue;
        }));
        gearGearList.Add(gearGear);
      }
      if (this.allGearRecipes[i].material5_gear_id.HasValue)
      {
        GearGear gearGear = ((IEnumerable<GearGear>) MasterData.GearGearList).FirstOrDefault<GearGear>((Func<GearGear, bool>) (x =>
        {
          int groupId = x.group_id;
          int? material5GearId = this.allGearRecipes[i].material5_gear_id;
          int valueOrDefault = material5GearId.GetValueOrDefault();
          return groupId == valueOrDefault & material5GearId.HasValue;
        }));
        gearGearList.Add(gearGear);
      }
      for (int m = 0; m < selectItemGear.Count; m++)
      {
        int? nullable = gearGearList.FirstIndexOrNull<GearGear>((Func<GearGear, bool>) (x => x.group_id == selectItemGear[m].group_id));
        if (nullable.HasValue)
          gearGearList.Remove(gearGearList[nullable.Value]);
        else
          goto label_24;
      }
      if (gearGearList.Any<GearGear>((Func<GearGear, bool>) (x => x.group_id == item.Item.gear.group_id)))
        return true;
    }
    return false;
  }

  public override void IbtnBack()
  {
    if (this.IsPush)
      return;
    foreach (InventoryItem primarySelectedItem in this.PrimarySelectedItemList)
    {
      InventoryItem byItemIndex = this.InventoryItems.FindByItemIndex(primarySelectedItem.Item);
      if (byItemIndex != null)
      {
        primarySelectedItem.Item.favorite = byItemIndex.Item.favorite;
        primarySelectedItem.Item.ForBattle = byItemIndex.Item.ForBattle;
      }
    }
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Singleton<NGSceneManager>.GetInstance().clearStack("bugu005_10");
    Bugu00510Scene.changeSceneMaterialRecipe(false, this.mainGear, this.PrimarySelectedItemList, this.allGearRecipes);
  }

  public override void IbtnClear()
  {
    if (this.IsPush || this.SelectItemList.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x != null)).Count<InventoryItem>() <= 0)
      return;
    for (int index = 0; index < this.SelectItemList.Count<InventoryItem>(); ++index)
    {
      if (this.SelectItemList[index] != null)
      {
        this.SelectItemList[index].select = false;
        this.SelectItemList[index].Gray = false;
        this.SelectItemList[index].index = 0;
        this.SelectItemList[index] = (InventoryItem) null;
      }
    }
    this.DisplayIconAndBottomInfoUpdate();
  }

  public void IbtnDecision()
  {
    if (this.IsPush)
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Singleton<NGSceneManager>.GetInstance().clearStack("bugu005_10");
    Bugu00510Scene.changeSceneMaterialRecipe(false, this.mainGear, this.SelectItemList, this.allGearRecipes);
  }

  public void onIbtnIcon1()
  {
    this.RemoveSelectItem(1);
    this.UpdateSelectItemIndexWithInfo();
  }

  public void onIbtnIcon2()
  {
    this.RemoveSelectItem(2);
    this.UpdateSelectItemIndexWithInfo();
  }

  public void onIbtnIcon3()
  {
    this.RemoveSelectItem(3);
    this.UpdateSelectItemIndexWithInfo();
  }

  public void onIbtnIcon4()
  {
    this.RemoveSelectItem(4);
    this.UpdateSelectItemIndexWithInfo();
  }

  public void onIbtnIcon5()
  {
    this.RemoveSelectItem(5);
    this.UpdateSelectItemIndexWithInfo();
  }
}
