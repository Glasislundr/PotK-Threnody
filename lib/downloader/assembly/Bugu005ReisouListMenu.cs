// Decompiled with JetBrains decompiler
// Type: Bugu005ReisouListMenu
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
public class Bugu005ReisouListMenu : Bugu005ItemListMenuBase
{
  [SerializeField]
  protected UILabel TxtNumberPattern1;
  [SerializeField]
  protected UIButton BtnSort;
  [SerializeField]
  protected UIButton BtnReisouMixer;
  [SerializeField]
  protected GameObject dirNoItem;
  private bool needClearCache = true;

  public override IEnumerator Init()
  {
    Bugu005ReisouListMenu menu = this;
    if (Object.op_Equality((Object) menu.SortPopupPrefab, (Object) null))
    {
      Future<GameObject> SortPopupPrefabF = new ResourceObject("Prefabs/popup/popup_Item_Reisou_Sort__anim_popup01").Load<GameObject>();
      IEnumerator e = SortPopupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.SortPopupPrefab = SortPopupPrefabF.Result;
      SortPopupPrefabF = (Future<GameObject>) null;
    }
    menu.SortPopupPrefab.GetComponent<ReisouSortAndFilter>().Initialize((Bugu005ItemListMenuBase) menu);
    menu.isInitSortPrefab = true;
    if (Object.op_Inequality((Object) menu.BtnReisouMixer, (Object) null) && !PerformanceConfig.GetInstance().EnableReisouMixerButton)
      ((Component) menu.BtnReisouMixer).gameObject.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    yield return (object) menu.\u003C\u003En__0();
  }

  protected override IEnumerator InitExtension()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu005ReisouListMenu bugu005ReisouListMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    bugu005ReisouListMenu.removeReisouInfo = (GameCore.ItemInfo) null;
    bugu005ReisouListMenu.removeReisouCallback = new Action(bugu005ReisouListMenu.cbRemoveReisou);
    bugu005ReisouListMenu.isReisouRemovePossible = true;
    bugu005ReisouListMenu.isReisouFusionPossible = true;
    bugu005ReisouListMenu.isReisouDrillingPossible = true;
    return false;
  }

  public override Persist<Persist.ReisouSortAndFilterInfo> GetReisouPersist()
  {
    return Persist.bugu005ReisouListSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    List<PlayerItem> playerItems = new List<PlayerItem>();
    this.equipedReisouIdList = new List<int>();
    foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
    {
      if (playerItem.isWeapon())
      {
        if (playerItem.equipped_reisou_player_gear_id != 0)
          this.equipedReisouIdList.Add(playerItem.equipped_reisou_player_gear_id);
      }
      else if (playerItem.isReisou())
        playerItems.Add(playerItem);
    }
    this.SetFusionReisouGearIdList(playerItems);
    if (Object.op_Inequality((Object) this.dirNoItem, (Object) null))
      this.dirNoItem.SetActive(playerItems.Count <= 0);
    return playerItems;
  }

  protected override long GetRevisionItemList() => SMManager.Revision<PlayerItem[]>();

  protected override void BottomInfoUpdate()
  {
    InventoryItem[] array = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item != null && !x.removeButton && x.Item.isReisou)).ToArray<InventoryItem>();
    int maxReisouItems = SMManager.Get<Player>().max_reisou_items;
    this.TxtNumberPattern1.SetTextLocalize(Consts.Format(Consts.GetInstance().GEAR_0052_POSSESSION, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) ((IEnumerable<InventoryItem>) array).Count<InventoryItem>()
      },
      {
        (object) "max",
        (object) maxReisouItems
      }
    }));
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem displayItem = this.DisplayItems[inventoryItemIdx];
    itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.OpenReisouDetailPopup(playeritem.ItemInfo));
    if (displayItem.Item.isSupply || displayItem.Item.isExchangable || displayItem.Item.isCompse || displayItem.Item.isWeaponMaterial)
    {
      itemIcon.QuantitySupply = true;
      itemIcon.EnableQuantity(displayItem.Item.quantity);
    }
    else
      itemIcon.QuantitySupply = false;
    displayItem.Item.ForBattle = this.equipedReisouIdList.Contains(itemIcon.ItemInfo.itemID);
    itemIcon.ForBattle = displayItem.Item.ForBattle;
    itemIcon.ItemInfo.ForBattle = displayItem.Item.ForBattle;
    itemIcon.Favorite = displayItem.Item.favorite;
    displayItem.Item.FusionPossible = this.fusionReisouGearIdList.ContainsKey(itemIcon.ItemInfo.masterID);
    itemIcon.FusionPossible = displayItem.Item.FusionPossible;
    itemIcon.SetupIconsBlink();
    itemIcon.Gray = false;
    itemIcon.SelectedQuantity(0);
    itemIcon.Deselect();
    itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(((Bugu005ItemListMenuBase) this).OpenReisouDetailPopup));
  }

  protected virtual void cbRemoveReisou()
  {
    this.equipedReisouIdList.Remove(this.removeReisouInfo.itemID);
    foreach (InventoryItem displayItem in this.DisplayItems)
    {
      if (displayItem.Item.itemID == this.removeReisouInfo.itemID)
      {
        displayItem.Item.ForBattle = false;
        break;
      }
    }
    foreach (ItemIcon itemIcon in this.AllItemIcon)
    {
      if (itemIcon.ItemInfo.itemID == this.removeReisouInfo.itemID)
      {
        itemIcon.ForBattle = false;
        itemIcon.FusionPossible = itemIcon.ItemInfo.FusionPossible;
        itemIcon.SetupIconsBlink();
        break;
      }
    }
    this.ReisouSort(this.ReisouSortCategory, this.ReisouOrderBuySort, this.isEquipFirst);
  }

  public override void Sort(
    ItemSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isEquipFirst)
  {
    this.ReisouSort(this.ReisouSortCategory, this.ReisouOrderBuySort, isEquipFirst);
  }

  public void onBackScene()
  {
    if (Object.op_Inequality((Object) this.SortPopupPrefab, (Object) null))
      this.SortPopupPrefab.GetComponent<ReisouSortAndFilter>().Initialize((Bugu005ItemListMenuBase) this);
    float num = this.scroll.scrollView.verticalScrollBar.value;
    this.ReisouSort(this.ReisouSortCategory, this.ReisouOrderBuySort, this.isEquipFirst);
    this.scroll.ResolvePosition(new Vector2(0.0f, num));
    this.needClearCache = true;
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Persist.sortOrder.Flush();
    if (!this.needClearCache)
      return;
    ItemIcon.ClearCache();
  }

  public override void IbtnSort()
  {
    if (this.IsPush)
      return;
    this.ShowSortAndFilterPopup();
  }

  private new void ShowSortAndFilterPopup()
  {
    if (!Singleton<PopupManager>.GetInstance().isOpen)
    {
      if (!Object.op_Inequality((Object) this.SortPopupPrefab, (Object) null))
        return;
      GameObject prefab = this.SortPopupPrefab.Clone();
      ReisouSortAndFilter sortAndFilter = prefab.GetComponent<ReisouSortAndFilter>();
      sortAndFilter.Initialize((Bugu005ItemListMenuBase) this, true);
      sortAndFilter.SetItemNum(this.InventoryItems.ReisouFilterBy(this.reisouFilter).ToList<InventoryItem>(), this.InventoryItems);
      sortAndFilter.SortFilterItemNum = (Action) (() => sortAndFilter.SetItemNum(this.InventoryItems.ReisouFilterBy(this.reisouFilter).ToList<InventoryItem>(), this.InventoryItems));
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    }
    else
      this.IsPush = false;
  }

  public void IbtnSell()
  {
    if (this.IsPushAndSet())
      return;
    this.needClearCache = false;
    Bugu00525Scene.ChangeScene(true, Bugu00525Scene.Mode.Reisou);
  }

  public void IbtnReisouCreation()
  {
    if (this.IsPushAndSet())
      return;
    this.needClearCache = false;
    Bugu005ReisouCreationScene.ChangeScene(true);
  }

  public void IbtnReisouMixer()
  {
    if (this.IsPushAndSet())
      return;
    this.needClearCache = false;
    Bugu005ReisouMixerScene.ChangeScene(true);
  }
}
