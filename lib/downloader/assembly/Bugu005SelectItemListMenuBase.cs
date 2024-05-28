// Decompiled with JetBrains decompiler
// Type: Bugu005SelectItemListMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu005SelectItemListMenuBase : Bugu005ItemListMenuBase
{
  [SerializeField]
  protected Bugu005SelectItemListMenuBase.SelectModeEnum SelectMode;
  [SerializeField]
  protected bool EnableFavorite;
  [SerializeField]
  protected bool EnableForBattle;
  [SerializeField]
  protected bool EnableBroken;
  [SerializeField]
  protected int selectMax;
  protected List<InventoryItem> SelectItemList = new List<InventoryItem>();
  protected List<InventoryItem> PrimarySelectedItemList = new List<InventoryItem>();

  public int SelectMax
  {
    get => this.selectMax;
    set => this.selectMax = value;
  }

  protected virtual void SelectItemProc(GameCore.ItemInfo item)
  {
  }

  protected virtual bool IsGrayIcon(InventoryItem item)
  {
    if (this.DisableTouchIcon(item))
      return true;
    return this.SelectItemList.Count >= this.selectMax ? !item.Gray : item.Gray;
  }

  protected virtual bool DisableTouchIcon(InventoryItem item)
  {
    if (item.Item == null)
      return !this.EnableForBattle || !this.EnableFavorite || !this.EnableBroken;
    if (!this.EnableForBattle && item.Item.ForBattle || !this.EnableFavorite && item.Item.favorite)
      return true;
    return !this.EnableBroken && item.Item.broken;
  }

  protected override void UpdateInvetoryItem(InventoryItem invItem, PlayerItem item)
  {
    invItem.Item.Set(item);
    invItem.select = false;
    invItem.Gray = false;
    if (!this.DisableTouchIcon(invItem))
      return;
    this.RemoveSelectItem(invItem);
  }

  protected override void UpdateInvetoryItem(InventoryItem invItem, PlayerMaterialGear item)
  {
    invItem.Item.Set(item);
    invItem.select = false;
    invItem.Gray = false;
    if (!this.DisableTouchIcon(invItem))
      return;
    this.RemoveSelectItem(invItem);
  }

  protected override void UpdateInventoryItemList()
  {
    InventoryItem[] array1 = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
    {
      if (x.Item == null || x.removeButton)
        return false;
      return x.Item.isWeapon || x.Item.isSupply;
    })).ToArray<InventoryItem>();
    if (array1 != null && ((IEnumerable<InventoryItem>) array1).Any<InventoryItem>())
    {
      PlayerItem[] array2 = SMManager.Get<PlayerItem[]>();
      foreach (InventoryItem inventoryItem in array1)
      {
        InventoryItem invItem = inventoryItem;
        PlayerItem playerItem = Array.Find<PlayerItem>(array2, (Predicate<PlayerItem>) (x => x.id == invItem.Item.itemID));
        if (playerItem != (PlayerItem) null)
          this.UpdateInvetoryItem(invItem, playerItem);
      }
    }
    InventoryItem[] array3 = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
    {
      if (x.Item == null || x.removeButton)
        return false;
      return x.Item.isCompse || x.Item.isExchangable;
    })).ToArray<InventoryItem>();
    if (array3 != null && ((IEnumerable<InventoryItem>) array3).Any<InventoryItem>())
    {
      PlayerMaterialGear[] array4 = SMManager.Get<PlayerMaterialGear[]>();
      foreach (InventoryItem inventoryItem in array3)
      {
        InventoryItem invItem = inventoryItem;
        PlayerMaterialGear playerMaterialGear = Array.Find<PlayerMaterialGear>(array4, (Predicate<PlayerMaterialGear>) (x => x.id == invItem.Item.itemID));
        if (playerMaterialGear != (PlayerMaterialGear) null)
          this.UpdateInvetoryItem(invItem, playerMaterialGear);
      }
    }
    this.UpdateIcons();
  }

  protected virtual void UpdateIcons()
  {
    this.SelectItemList.ForEachIndex<InventoryItem>((Action<InventoryItem, int>) ((x, idx) =>
    {
      x.select = true;
      x.Gray = true;
      if (this.SelectMode != Bugu005SelectItemListMenuBase.SelectModeEnum.Num)
        return;
      x.index = idx + 1;
    }));
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
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
    itemIcon.FusionPossible = itemIcon.ItemInfo.FusionPossible = false;
    itemIcon.Gray = this.IsGrayIcon(displayItem);
    if (this.DisableTouchIcon(displayItem))
    {
      itemIcon.onClick = (Action<ItemIcon>) (_ => { });
      displayItem.Gray = true;
    }
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
    this.SelectItemList.Clear();
    yield break;
  }

  protected override void AllItemIconUpdate()
  {
    foreach (ItemIcon itemIcon in this.AllItemIcon)
    {
      ItemIcon icon = itemIcon;
      InventoryItem inventoryItem = this.InventoryItems.Find((Predicate<InventoryItem>) (x => x.Item == icon.ItemInfo));
      if (inventoryItem != null)
      {
        if (inventoryItem.select)
        {
          if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Check)
          {
            icon.SelectedQuantity(inventoryItem.selectCount);
            icon.SelectByCheckIcon();
          }
          else
            icon.Select(inventoryItem.index - 1);
          icon.Gray = this.IsGrayIcon(inventoryItem);
        }
        else
        {
          if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Check)
          {
            icon.SelectQuantity = false;
            icon.DeselectByCheckIcon();
          }
          else
            icon.Deselect();
          icon.Gray = this.IsGrayIcon(inventoryItem);
        }
      }
    }
  }

  public override IEnumerator Init()
  {
    Bugu005SelectItemListMenuBase menu = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    menu.InitializeEnd = false;
    IEnumerator e = menu.LoadItemIconPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!menu.isInitSortPrefab)
    {
      if (Object.op_Equality((Object) menu.SortPopupPrefab, (Object) null))
      {
        Future<GameObject> sortPopupPrefabF = menu.GetSortAndFilterPopupGameObject();
        if (sortPopupPrefabF != null)
        {
          e = sortPopupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          menu.SortPopupPrefab = sortPopupPrefabF.Result;
        }
        sortPopupPrefabF = (Future<GameObject>) null;
      }
      menu.SortPopupPrefab.GetComponent<ItemSortAndFilter>().Initialize((Bugu005ItemListMenuBase) menu);
    }
    List<PlayerItem> itemList = menu.GetItemList();
    List<PlayerMaterialGear> materialItemList = menu.GetMaterialList();
    int itemListCnt = menu.GetItemListCnt(itemList, materialItemList);
    int itemListFavoriteCnt = menu.GetItemListFavoriteCnt(itemList);
    long verItemList = menu.GetRevisionItemList();
    long verMaterialList = menu.GetRevisionMaterialList();
    int itemListEquipCount = menu.GetItemListEquipCount(itemList);
    if (!Singleton<NGGameDataManager>.GetInstance().IsEarth)
    {
      bool isLoading = Singleton<CommonRoot>.GetInstance().isLoading;
      if (!isLoading)
        Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
      List<string> paths = new List<string>();
      if (itemList != null)
      {
        for (int index = 0; index < itemList.Count; ++index)
        {
          if (itemList[index].gear != null && !itemList[index].gear.isReisou())
            paths.AddRange((IEnumerable<string>) itemList[index].gear.ResourcePaths());
          if (itemList[index].supply != null)
            paths.AddRange((IEnumerable<string>) itemList[index].supply.ResourcePaths());
        }
      }
      if (materialItemList != null)
      {
        for (int index = 0; index < materialItemList.Count; ++index)
        {
          if (materialItemList[index].gear != null)
            paths.AddRange((IEnumerable<string>) materialItemList[index].gear.ResourcePaths());
          if (materialItemList[index].supply != null)
            paths.AddRange((IEnumerable<string>) materialItemList[index].supply.ResourcePaths());
        }
      }
      e = OnDemandDownload.waitLoadSomethingResource((IEnumerable<string>) paths, false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!isLoading)
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
    }
    if (menu.itemCount == itemListCnt && menu.revisionItemList_.HasValue)
    {
      long num1 = verItemList;
      long? nullable = menu.revisionItemList_;
      long valueOrDefault1 = nullable.GetValueOrDefault();
      if (num1 == valueOrDefault1 & nullable.HasValue && menu.revisionMaterialList_.HasValue)
      {
        long num2 = verMaterialList;
        nullable = menu.revisionMaterialList_;
        long valueOrDefault2 = nullable.GetValueOrDefault();
        if (num2 == valueOrDefault2 & nullable.HasValue && (menu.itemFavoriteCount == itemListFavoriteCnt || menu.Filter[26]) && menu.itemEquipCount == itemListEquipCount)
        {
          menu.UpdateInventoryItemList();
          goto label_44;
        }
      }
    }
    menu.itemCount = itemListCnt;
    menu.itemFavoriteCount = itemListFavoriteCnt;
    menu.revisionItemList_ = new long?(verItemList);
    menu.revisionMaterialList_ = new long?(verMaterialList);
    menu.InventoryItems.Clear();
    menu.CreateInvetoryItem(itemList, materialItemList);
    e = menu.InitExtension();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    menu.CreatePlayerItems();
    menu.BottomInfoUpdate();
label_44:
    yield return (object) menu.ScrollViewReposition();
    menu.InitializeEnd = true;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  protected virtual void UpdateSelectItemIndex()
  {
    if (this.SelectMode != Bugu005SelectItemListMenuBase.SelectModeEnum.Num)
      return;
    int count = this.SelectItemList.Count;
    for (int index = 0; index < count; ++index)
      this.SelectItemList[index].index = index + 1;
  }

  protected virtual void AddSelectItem(InventoryItem invItem)
  {
    if (invItem == null || this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == invItem)))
      return;
    invItem.select = true;
    invItem.Gray = true;
    invItem.index = 0;
    if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Num)
      invItem.index = this.SelectItemList.Count<InventoryItem>() + 1;
    this.SelectItemList.Add(invItem);
  }

  protected virtual void RemoveSelectItem(InventoryItem invItem)
  {
    if (invItem == null || !this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == invItem)))
      return;
    invItem.select = false;
    invItem.Gray = false;
    invItem.index = 0;
    this.SelectItemList.Remove(invItem);
  }

  public void RemoveSelectItem(int idx)
  {
    this.RemoveSelectItem(this.InventoryItems.Find((Predicate<InventoryItem>) (x => x.index == idx)));
  }

  public virtual void ClearSelectItem()
  {
    foreach (InventoryItem selectItem in this.SelectItemList)
    {
      selectItem.select = false;
      selectItem.Gray = false;
      selectItem.index = 0;
    }
    this.SelectItemList.Clear();
    this.DisplayIconAndBottomInfoUpdate();
  }

  public virtual void IbtnClear()
  {
    if (this.IsPush)
      return;
    this.ClearSelectItem();
  }

  public void UpdateSelectItemIndexWithInfo()
  {
    this.UpdateSelectItemIndex();
    this.DisplayIconAndBottomInfoUpdate();
  }

  protected enum SelectModeEnum
  {
    Num,
    Check,
  }
}
