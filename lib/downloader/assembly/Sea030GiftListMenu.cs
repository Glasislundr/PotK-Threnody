// Decompiled with JetBrains decompiler
// Type: Sea030GiftListMenu
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
public class Sea030GiftListMenu : Bugu005ItemListMenuBase
{
  public static int[] playerRecipeIDList;
  public GameObject giftDetailsSea;
  public GameObject callGiftRecipeWindow;
  public Sea030GiftDetails giftDetails;
  public static bool isCreateCallItem;
  public static GearGear initPopupGear;
  private GameCore.ItemInfo[] gears;
  public static bool isCallGiftRecipeWindowOpen;

  public override IEnumerator Init()
  {
    Sea030GiftListMenu sea030GiftListMenu = this;
    if (Object.op_Equality((Object) sea030GiftListMenu.callGiftRecipeWindow, (Object) null))
    {
      Future<GameObject> callItemWindowPrefab = new ResourceObject("Prefabs/sea030_giftList/dir_giftDetails_sea").Load<GameObject>();
      IEnumerator e = callItemWindowPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      sea030GiftListMenu.giftDetailsSea = callItemWindowPrefab.Result;
      sea030GiftListMenu.callGiftRecipeWindow = sea030GiftListMenu.giftDetailsSea.Clone(((Component) sea030GiftListMenu).transform);
      sea030GiftListMenu.giftDetails = sea030GiftListMenu.callGiftRecipeWindow.GetComponent<Sea030GiftDetails>();
      sea030GiftListMenu.callGiftRecipeWindow.SetActive(false);
      // ISSUE: reference to a compiler-generated method
      yield return (object) sea030GiftListMenu.\u003C\u003En__0();
      callItemWindowPrefab = (Future<GameObject>) null;
    }
    float num = sea030GiftListMenu.scroll.scrollView.verticalScrollBar.value;
    sea030GiftListMenu.Sort(ItemSortAndFilter.SORT_TYPES.CallItem, SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING, false);
    sea030GiftListMenu.scroll.ResolvePosition(new Vector2(0.0f, num));
    Sea030GiftListMenu.isCallGiftRecipeWindowOpen = sea030GiftListMenu.callGiftRecipeWindow.activeSelf;
    yield return (object) null;
  }

  public IEnumerator OpenInitPopup()
  {
    bool isGray = false;
    List<CallGiftRecipe> list = ((IEnumerable<CallGiftRecipe>) MasterData.CallGiftRecipeList).Where<CallGiftRecipe>((Func<CallGiftRecipe, bool>) (x => x.success_gear_id_GearGear == Sea030GiftListMenu.initPopupGear.ID)).ToList<CallGiftRecipe>();
    if (list.Count >= 1 && !((IEnumerable<int>) Sea030GiftListMenu.playerRecipeIDList).Contains<int>(list[0].ID))
      isGray = true;
    this.Show(Sea030GiftListMenu.initPopupGear, isGray);
    yield return (object) null;
  }

  protected override List<PlayerMaterialGear> GetMaterialList()
  {
    List<PlayerMaterialGear> list = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.isCallGift())).ToList<PlayerMaterialGear>();
    GearGear[] array = ((IEnumerable<GearGear>) MasterData.GearGearList).Where<GearGear>((Func<GearGear, bool>) (x => x.isCallGift())).OrderByDescending<GearGear, int>((Func<GearGear, int>) (x => x.ID)).ToArray<GearGear>();
    List<PlayerMaterialGear> materialList = new List<PlayerMaterialGear>();
    foreach (GearGear gearGear in array)
    {
      PlayerMaterialGear playerMaterialGear1 = new PlayerMaterialGear();
      playerMaterialGear1.player_id = Player.Current.id;
      playerMaterialGear1.quantity = 0;
      playerMaterialGear1.gear_id = gearGear.ID;
      foreach (PlayerMaterialGear playerMaterialGear2 in list)
      {
        if (playerMaterialGear1.gear_id == playerMaterialGear2.gear_id)
        {
          playerMaterialGear1.id = playerMaterialGear2.id;
          playerMaterialGear1.quantity = playerMaterialGear2.quantity;
          break;
        }
      }
      materialList.Add(playerMaterialGear1);
    }
    return materialList;
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem item = this.DisplayItems[inventoryItemIdx];
    itemIcon.QuantitySupply = true;
    itemIcon.EnableQuantity(item.Item.quantity);
    List<CallGiftRecipe> list = ((IEnumerable<CallGiftRecipe>) MasterData.CallGiftRecipeList).Where<CallGiftRecipe>((Func<CallGiftRecipe, bool>) (x => x.success_gear_id_GearGear == item.Item.gear.ID)).ToList<CallGiftRecipe>();
    if (list.Count >= 1 && !((IEnumerable<int>) Sea030GiftListMenu.playerRecipeIDList).Contains<int>(list[0].ID))
      itemIcon.Gray = true;
    else
      itemIcon.Gray = false;
    itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.OpenCallGiftRecipe(playeritem.ItemInfo.gear, playeritem.Gray));
    itemIcon.SelectedQuantity(0);
    itemIcon.Deselect();
    this.gears = this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.gear.isCallGift())).Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)).ToArray<GameCore.ItemInfo>();
    itemIcon.EnableLongPressEvent(true, new Action<ItemIcon>(this.GearDetail));
  }

  private void GearDetail(ItemIcon itemIcon)
  {
    int index = Array.FindIndex<GameCore.ItemInfo>(this.gears, (Predicate<GameCore.ItemInfo>) (x => x == itemIcon.ItemInfo));
    Bugu00561Scene.changeScene(true, itemIcon.ItemInfo, this.gears, index);
  }

  public IEnumerator ReInit()
  {
    Sea030GiftListMenu sea030GiftListMenu = this;
    float f = sea030GiftListMenu.scroll.scrollView.verticalScrollBar.value;
    if (Sea030GiftListMenu.isCreateCallItem)
      Sea030GiftListMenu.isCreateCallItem = false;
    Sea030GiftListMenu.initPopupGear = (GearGear) null;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = sea030GiftListMenu.\u003C\u003En__0();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    sea030GiftListMenu.Sort(ItemSortAndFilter.SORT_TYPES.CallItem, SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING, false);
    sea030GiftListMenu.scroll.ResolvePosition(new Vector2(0.0f, f));
    yield return (object) null;
  }

  protected void OpenCallGiftRecipe(GearGear item, bool isGray)
  {
    if (this.callGiftRecipeWindow.activeSelf)
      return;
    this.Show(item, isGray);
  }

  public void Show(GearGear item, bool isGray)
  {
    this.giftDetails.SetMenu(this);
    this.StartCoroutine(this.giftDetails.ShowWindow(item, isGray));
    this.callGiftRecipeWindow.SetActive(true);
  }

  public void Hide()
  {
    this.callGiftRecipeWindow.SetActive(false);
    Sea030GiftListMenu.isCallGiftRecipeWindowOpen = false;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_1042");
  }

  public void onBackScene()
  {
    float num = this.scroll.scrollView.verticalScrollBar.value;
    this.Sort(ItemSortAndFilter.SORT_TYPES.CallItem, SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING, false);
    this.scroll.ResolvePosition(new Vector2(0.0f, num));
  }

  public override void onEndScene()
  {
    Sea030GiftListMenu.isCallGiftRecipeWindowOpen = false;
    base.onEndScene();
    Persist.sortOrder.Flush();
  }
}
