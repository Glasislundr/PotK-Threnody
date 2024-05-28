// Decompiled with JetBrains decompiler
// Type: Bugu00521Menu
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
public class Bugu00521Menu : Bugu005SelectItemListMenuBase
{
  [SerializeField]
  private Transform[] materialTargetIcon;
  [SerializeField]
  private UILabel txtSlectedcountNum;
  [SerializeField]
  private UILabel txtSpendzenieNum;
  [SerializeField]
  private UIButton ibtnYes;
  private const float BOTTOM_ICON_SCALE = 0.6617647f;
  private List<GameCore.ItemInfo> FirstSelectItemList = new List<GameCore.ItemInfo>();
  private ItemIcon[] icons;
  private bool firstInit = true;

  public void SetFirstSelectItem(List<GameCore.ItemInfo> items) => this.FirstSelectItemList = items;

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu0052CompositeSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.isWeapon() && x.gear.kind.Enum != GearKindEnum.accessories || x.isComposeManaSeed())).ToList<PlayerItem>();
  }

  protected override List<PlayerMaterialGear> GetMaterialList()
  {
    List<PlayerMaterialGear> list = new List<PlayerMaterialGear>();
    ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x =>
    {
      if (x.isDilling() || x.isSpecialDilling())
        return false;
      if (x.isCompse())
        return true;
      return x.isWeaponMaterial() && x.gear.kind.Enum != GearKindEnum.accessories;
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
    itemIcon.ItemInfo.ForBattle = itemIcon.ForBattle;
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
    Bugu00521Menu bugu00521Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    int length = bugu00521Menu.materialTargetIcon.Length;
    bugu00521Menu.icons = new ItemIcon[length];
    for (int index = 0; index < length; ++index)
    {
      bugu00521Menu.materialTargetIcon[index].Clear();
      GameObject gameObject = bugu00521Menu.ItemIconPrefab.Clone(bugu00521Menu.materialTargetIcon[index]);
      gameObject.transform.localScale = new Vector3(0.6617647f, 0.6617647f);
      ItemIcon component = gameObject.GetComponent<ItemIcon>();
      component.EnabledExpireDate = bugu00521Menu.enabledExpireDate;
      bugu00521Menu.icons[index] = component;
    }
    if (bugu00521Menu.firstInit)
    {
      bugu00521Menu.firstInit = false;
      bugu00521Menu.SelectItemList.Clear();
      if (bugu00521Menu.FirstSelectItemList != null && bugu00521Menu.FirstSelectItemList.Count > 0)
      {
        // ISSUE: reference to a compiler-generated method
        bugu00521Menu.FirstSelectItemList = bugu00521Menu.FirstSelectItemList.Where<GameCore.ItemInfo>(new Func<GameCore.ItemInfo, bool>(bugu00521Menu.\u003CInitExtension\u003Eb__13_0)).ToList<GameCore.ItemInfo>();
        // ISSUE: reference to a compiler-generated method
        bugu00521Menu.FirstSelectItemList.ForEach(new Action<GameCore.ItemInfo>(bugu00521Menu.\u003CInitExtension\u003Eb__13_1));
      }
    }
    else if (bugu00521Menu.SelectItemList != null && bugu00521Menu.SelectItemList.Count > 0)
    {
      // ISSUE: reference to a compiler-generated method
      List<InventoryItem> list = bugu00521Menu.SelectItemList.Where<InventoryItem>(new Func<InventoryItem, bool>(bugu00521Menu.\u003CInitExtension\u003Eb__13_2)).ToList<InventoryItem>();
      bugu00521Menu.SelectItemList.Clear();
      // ISSUE: reference to a compiler-generated method
      Action<InventoryItem> action = new Action<InventoryItem>(bugu00521Menu.\u003CInitExtension\u003Eb__13_3);
      list.ForEach(action);
    }
    return false;
  }

  protected override void BottomInfoUpdate()
  {
    Player player = SMManager.Get<Player>();
    int count = this.SelectItemList != null ? this.SelectItemList.Count : 0;
    for (int index = 0; index < this.SelectMax; ++index)
    {
      if (index < count)
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
        if (this.enabledExpireDate)
          this.icons[index].resetExpireDate();
      }
      else
      {
        this.icons[index].SetEmpty(true);
        this.icons[index].onClick = (Action<ItemIcon>) (playeritem => { });
      }
    }
    this.txtSlectedcountNum.SetTextLocalize(count);
    ((UIWidget) this.txtSlectedcountNum).color = count < this.SelectMax ? Color.white : Color.red;
    int compositeCost = CalcItemCost.GetCompositeCost(this.SelectItemList);
    this.txtSpendzenieNum.SetTextLocalize(compositeCost);
    ((UIWidget) this.txtSpendzenieNum).color = player.money >= (long) compositeCost ? Color.white : Color.red;
    ((UIButtonColor) this.ibtnYes).isEnabled = count > 0 && player.money >= (long) compositeCost;
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
      else if (this.SelectItemList.Count < this.SelectMax)
        this.AddSelectItem(byItemIndex);
    }
    this.UpdateSelectItemIndexWithInfo();
  }

  protected override bool IsGrayIcon(InventoryItem item)
  {
    bool flag = this.DisableTouchIcon(item);
    if (this.SelectItemList.Count < this.SelectMax)
      return flag;
    return this.SelectItemList.FindByItem(item.Item) == null;
  }

  protected override bool DisableTouchIcon(InventoryItem item)
  {
    GearGear gear1 = item.Item.gear;
    if (item.Item.isDisappearItem && !item.Item.isComposeManaSeed && !item.Item.isExhaustedGear)
      return true;
    if (this.SelectItemList.Count<InventoryItem>() > 0 && item.Item.gear != null)
    {
      InventoryItem selectItem = this.SelectItemList[0];
      GearGear gear2 = selectItem.Item != null ? selectItem.Item.gear : (GearGear) null;
      if (gear2 != null)
      {
        if (!gear2.isComposeManaSeed())
        {
          if (this.DisableTouchIconDefault(item))
            return true;
          return item.Item.isWeapon && item.Item.isComposeManaSeed;
        }
        if (this.DisableTouchIconDefault(item) || gear2.ID != item.Item.masterID || item.Item.ForBattle && item != selectItem)
          return true;
        return !this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == item)) && this.SelectItemList.Where<InventoryItem>((Func<InventoryItem, bool>) (y => y != null && y.Item != null)).Sum<InventoryItem>((Func<InventoryItem, int>) (x => x.Item.gearAccessoryRemainingAmount)) >= gear2.manaSeedRecoveryLimit;
      }
    }
    return this.DisableTouchIconDefault(item);
  }

  private bool DisableTouchIconDefault(InventoryItem item)
  {
    if (item.Item == null)
      return !this.EnableForBattle || !this.EnableFavorite || !this.EnableBroken;
    if (!this.EnableForBattle && !item.Item.gear.isComposeManaSeed() && item.Item.ForBattle || !this.EnableFavorite && item.Item.favorite)
      return true;
    return !this.EnableBroken && item.Item.broken;
  }

  public override void IbtnBack()
  {
    if (this.IsPush)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    List<GameCore.ItemInfo> list = this.FirstSelectItemList.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x =>
    {
      InventoryItem inventoryItem = this.InventoryItems.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (i => i.Item.itemID == x.itemID));
      return inventoryItem != null && !inventoryItem.Item.favorite && !inventoryItem.Item.ForBattle;
    })).ToList<GameCore.ItemInfo>();
    List<InventoryItem> invList = new List<InventoryItem>();
    Action<GameCore.ItemInfo> action = (Action<GameCore.ItemInfo>) (x => invList.Add(new InventoryItem(x)));
    list.ForEach(action);
    Bugu0053Scene.changeScene(false, invList);
  }

  public void IbtnDecision()
  {
    if (this.IsPush)
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Bugu0053Scene.changeScene(false, this.SelectItemList);
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
