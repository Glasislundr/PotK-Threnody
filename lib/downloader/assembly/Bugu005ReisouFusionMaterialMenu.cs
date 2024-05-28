// Decompiled with JetBrains decompiler
// Type: Bugu005ReisouFusionMaterialMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu005ReisouFusionMaterialMenu : Bugu005ReisouListMenu
{
  protected GameObject reisouFusionPopupPrefab;
  private PlayerItem baseItem;
  private int targetReisouGearId;
  private GearReisouFusion recipe;

  public IEnumerator Init(PlayerItem baseItem)
  {
    this.baseItem = baseItem;
    this.recipe = baseItem.GetReisouFusionPossibleRecipe(SMManager.Get<PlayerItem[]>());
    this.targetReisouGearId = baseItem.isHolyReisou() ? this.recipe.chaos_ID_GearGear : this.recipe.holy_ID_GearGear;
    // ISSUE: reference to a compiler-generated method
    yield return (object) this.\u003C\u003En__0();
  }

  protected override IEnumerator InitExtension()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu005ReisouFusionMaterialMenu fusionMaterialMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    fusionMaterialMenu.removeReisouInfo = (GameCore.ItemInfo) null;
    fusionMaterialMenu.removeReisouCallback = new Action(((Bugu005ReisouListMenu) fusionMaterialMenu).cbRemoveReisou);
    fusionMaterialMenu.isReisouFusionPossible = false;
    return false;
  }

  protected override List<PlayerItem> GetItemList()
  {
    List<PlayerItem> itemList = new List<PlayerItem>();
    this.equipedReisouIdList = new List<int>();
    foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
    {
      if (playerItem.isWeapon())
      {
        if (playerItem.equipped_reisou_player_gear_id != 0)
          this.equipedReisouIdList.Add(playerItem.equipped_reisou_player_gear_id);
      }
      else if (playerItem.entity_id == this.targetReisouGearId)
        itemList.Add(playerItem);
    }
    if (Object.op_Inequality((Object) this.dirNoItem, (Object) null))
      this.dirNoItem.SetActive(itemList.Count <= 0);
    return itemList;
  }

  public override void ReisouSort(
    ReisouSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isEquipFirst)
  {
    if (this.equipedReisouIdList != null)
    {
      foreach (InventoryItem inventoryItem in this.InventoryItems)
      {
        InventoryItem item = inventoryItem;
        item.Item.ForBattle = this.equipedReisouIdList.FirstOrDefault<int>((Func<int, bool>) (x => x == item.Item.itemID)) > 0;
      }
    }
    this.DisplayItems = this.InventoryItems.ReisouSortBy(ReisouSortAndFilter.SORT_TYPES.Rarity, SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING, false).ToList<InventoryItem>();
    this.scroll.Reset();
    this.AllItemIcon.ForEach((Action<ItemIcon>) (x =>
    {
      ((Component) x).transform.parent = ((Component) this).transform;
      ((Component) x).gameObject.SetActive(false);
    }));
    for (int index = 0; index < Mathf.Min(this.iconMaxValue, this.DisplayItems.Count); ++index)
    {
      this.scroll.Add(((Component) this.AllItemIcon[index]).gameObject, this.iconWidth, this.iconHeight);
      ((Component) this.AllItemIcon[index]).gameObject.SetActive(true);
    }
    this.InventoryItems.ForEach((Action<InventoryItem>) (v => v.icon = (ItemIcon) null));
    this.StartCoroutine(this.CreateItemIconRange(Mathf.Min(this.iconMaxValue, this.DisplayItems.Count)));
    this.scroll.CreateScrollPoint(this.iconHeight, this.DisplayItems.Count);
    this.scroll.ResolvePosition();
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem displayItem = this.DisplayItems[inventoryItemIdx];
    if (displayItem.Item.isSupply || displayItem.Item.isExchangable || displayItem.Item.isCompse || displayItem.Item.isWeaponMaterial)
    {
      itemIcon.QuantitySupply = true;
      itemIcon.EnableQuantity(displayItem.Item.quantity);
    }
    else
      itemIcon.QuantitySupply = false;
    displayItem.Item.ForBattle = this.equipedReisouIdList.Contains(itemIcon.ItemInfo.itemID);
    itemIcon.ForBattle = displayItem.Item.ForBattle;
    itemIcon.Favorite = displayItem.Item.favorite;
    itemIcon.SetupIconsBlink();
    itemIcon.SelectedQuantity(0);
    itemIcon.Deselect();
    if (itemIcon.ForBattle)
    {
      itemIcon.Gray = true;
      itemIcon.onClick = (Action<ItemIcon>) (_ => { });
    }
    else
    {
      itemIcon.Gray = false;
      itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.OpenReisouFusion(playeritem.ItemInfo));
    }
    itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(((Bugu005ItemListMenuBase) this).OpenReisouDetailPopup));
  }

  protected void OpenReisouFusion(GameCore.ItemInfo item)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenReisouFusionAsync(item));
  }

  protected IEnumerator OpenReisouFusionAsync(GameCore.ItemInfo item)
  {
    Bugu005ReisouFusionMaterialMenu fusionMaterialMenu = this;
    if (item == null)
    {
      fusionMaterialMenu.StartCoroutine(fusionMaterialMenu.IsPushOff());
    }
    else
    {
      IEnumerator e;
      if (Object.op_Equality((Object) fusionMaterialMenu.reisouFusionPopupPrefab, (Object) null))
      {
        Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_fusion").Load<GameObject>();
        e = popupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        fusionMaterialMenu.reisouFusionPopupPrefab = popupPrefabF.Result;
        popupPrefabF = (Future<GameObject>) null;
      }
      GameObject popup = fusionMaterialMenu.reisouFusionPopupPrefab.Clone();
      PopupReisouFusion component = popup.GetComponent<PopupReisouFusion>();
      popup.SetActive(false);
      e = component.Init(new Action(((Bugu005ItemListMenuBase) fusionMaterialMenu).IbtnBack), fusionMaterialMenu.recipe, fusionMaterialMenu.baseItem, item.playerItem);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      yield return (object) null;
      fusionMaterialMenu.StartCoroutine(fusionMaterialMenu.IsPushOff());
    }
  }

  public override void IbtnBack()
  {
    Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = this.baseItem.id;
    base.IbtnBack();
  }
}
