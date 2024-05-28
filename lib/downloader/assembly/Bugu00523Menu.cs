// Decompiled with JetBrains decompiler
// Type: Bugu00523Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Bugu00523Menu : Bugu005SelectItemListMenuBase
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
  private bool firstInit = true;
  private GameCore.ItemInfo BaseItem;
  private List<GameCore.ItemInfo> FirstSelectItemList = new List<GameCore.ItemInfo>();
  private ItemIcon[] icons;

  public void SetFirstSelectItem(List<GameCore.ItemInfo> items, GameCore.ItemInfo target)
  {
    this.BaseItem = target;
    this.FirstSelectItemList = items;
  }

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu0052BuildupMaterialSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    List<Bugu0058Menu.PlayerItemSort> playerItemSortList = new List<Bugu0058Menu.PlayerItemSort>();
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.sword, false);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.axe, false);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.spear, false);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.bow, false);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.gun, false);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.staff, false);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.shield, false);
    return playerItemSortList.Where<Bugu0058Menu.PlayerItemSort>((Func<Bugu0058Menu.PlayerItemSort, bool>) (x => x.item.id != this.BaseItem.itemID)).Select<Bugu0058Menu.PlayerItemSort, PlayerItem>((Func<Bugu0058Menu.PlayerItemSort, PlayerItem>) (x => x.item)).ToList<PlayerItem>();
  }

  protected override IEnumerator InitExtension()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu00523Menu bugu00523Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    int length = bugu00523Menu.materialTargetIcon.Length;
    bugu00523Menu.icons = new ItemIcon[length];
    for (int index = 0; index < length; ++index)
    {
      bugu00523Menu.materialTargetIcon[index].Clear();
      GameObject gameObject = bugu00523Menu.ItemIconPrefab.Clone(bugu00523Menu.materialTargetIcon[index]);
      gameObject.transform.localScale = new Vector3(0.6617647f, 0.6617647f);
      ItemIcon component = gameObject.GetComponent<ItemIcon>();
      bugu00523Menu.icons[index] = component;
    }
    if (bugu00523Menu.firstInit)
    {
      bugu00523Menu.firstInit = false;
      bugu00523Menu.SelectItemList.Clear();
      if (bugu00523Menu.FirstSelectItemList != null && bugu00523Menu.FirstSelectItemList.Count > 0)
      {
        // ISSUE: reference to a compiler-generated method
        bugu00523Menu.FirstSelectItemList = bugu00523Menu.FirstSelectItemList.Where<GameCore.ItemInfo>(new Func<GameCore.ItemInfo, bool>(bugu00523Menu.\u003CInitExtension\u003Eb__12_0)).ToList<GameCore.ItemInfo>();
        // ISSUE: reference to a compiler-generated method
        bugu00523Menu.FirstSelectItemList.ForEach(new Action<GameCore.ItemInfo>(bugu00523Menu.\u003CInitExtension\u003Eb__12_1));
      }
    }
    else if (bugu00523Menu.SelectItemList != null && bugu00523Menu.SelectItemList.Count > 0)
    {
      // ISSUE: reference to a compiler-generated method
      List<InventoryItem> list = bugu00523Menu.SelectItemList.Where<InventoryItem>(new Func<InventoryItem, bool>(bugu00523Menu.\u003CInitExtension\u003Eb__12_2)).ToList<InventoryItem>();
      bugu00523Menu.SelectItemList.Clear();
      // ISSUE: reference to a compiler-generated method
      Action<InventoryItem> action = new Action<InventoryItem>(bugu00523Menu.\u003CInitExtension\u003Eb__12_3);
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
        this.icons[index].Deselect();
      }
      else
      {
        this.icons[index].SetEmpty(true);
        this.icons[index].onClick = (Action<ItemIcon>) (playeritem => { });
      }
    }
    this.txtSlectedcountNum.SetTextLocalize(count);
    ((UIWidget) this.txtSlectedcountNum).color = count < this.SelectMax ? Color.white : Color.red;
    int buildupCost = CalcItemCost.GetBuildupCost(this.SelectItemList.Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)).ToList<GameCore.ItemInfo>());
    this.txtSpendzenieNum.SetTextLocalize(buildupCost);
    ((UIWidget) this.txtSpendzenieNum).color = player.money >= (long) buildupCost ? Color.white : Color.red;
    ((UIButtonColor) this.ibtnYes).isEnabled = count > 0 && player.money >= (long) buildupCost;
  }

  protected override void SelectItemProc(GameCore.ItemInfo item)
  {
    InventoryItem byItem = this.InventoryItems.FindByItem(item);
    if (byItem != null)
    {
      if (byItem.select)
        this.RemoveSelectItem(byItem);
      else if (this.SelectItemList.Count < this.SelectMax)
        this.AddSelectItem(byItem);
    }
    this.UpdateSelectItemIndexWithInfo();
  }

  public override void IbtnBack()
  {
    if (this.IsPush)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Bugu0058Scene.changeScene(false, this.FirstSelectItemList.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x =>
    {
      InventoryItem inventoryItem = this.InventoryItems.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (i => i.Item.itemID == x.itemID));
      return inventoryItem != null && !inventoryItem.Item.favorite && !inventoryItem.Item.ForBattle;
    })).ToList<GameCore.ItemInfo>());
  }

  public void IbtnDecision()
  {
    if (this.IsPush)
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    Bugu0058Scene.changeScene(false, this.SelectItemList.Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)).ToList<GameCore.ItemInfo>());
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
