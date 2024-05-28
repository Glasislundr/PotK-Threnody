// Decompiled with JetBrains decompiler
// Type: Bugu005ReisouMixerMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Bugu005ReisouMixerMenu : Bugu005SelectItemListMenuBase
{
  private const int subWidth = 123;
  private const int subHeight = 180;
  [SerializeField]
  protected UILabel txtZenyNumber;
  [SerializeField]
  protected UILabel txtSelectNumber;
  [SerializeField]
  protected UILabel txtAcquisitionsValue;
  [SerializeField]
  protected UI2DSprite slcItemIcon;
  [SerializeField]
  private UIButton ibtnYes;
  private Player player;
  private GameObject popupReisouMixerPrefab;
  private GameObject popupReisouMixerResultPrefab;
  private GameObject m_ItemIconPrefab;
  private GameObject m_CheckMaterialPopupPrefab;
  private GameObject m_CheckMaterialPopupPrefabMini;
  private const int CheckMaterialPopupMiniIconNum = 5;
  private Persist.ItemSortAndFilterInfo sortPersistLog;

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu005ReisouMixerSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    List<PlayerItem> itemList = new List<PlayerItem>();
    foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
    {
      if (playerItem.isWeapon() && !playerItem.isManaSeed())
        itemList.Add(playerItem);
    }
    return itemList;
  }

  protected override List<PlayerMaterialGear> GetMaterialList()
  {
    List<PlayerMaterialGear> materialList = new List<PlayerMaterialGear>();
    foreach (PlayerMaterialGear playerMaterialGear in SMManager.Get<PlayerMaterialGear[]>())
    {
      if (playerMaterialGear.isWeaponMaterial() && !playerMaterialGear.isManaSeed())
        materialList.Add(playerMaterialGear);
    }
    return materialList;
  }

  public override IEnumerator Init()
  {
    Bugu005ReisouMixerMenu bugu005ReisouMixerMenu = this;
    if (bugu005ReisouMixerMenu.sortPersistLog == null)
    {
      bugu005ReisouMixerMenu.sortPersistLog = new Persist.ItemSortAndFilterInfo();
      bugu005ReisouMixerMenu.sortPersistLog.setData(bugu005ReisouMixerMenu.GetPersist().Data);
    }
    Future<Sprite> spriteF = new ResourceObject("Icons/ChaosJewel_Icon").Load<Sprite>();
    yield return (object) spriteF.Wait();
    bugu005ReisouMixerMenu.slcItemIcon.sprite2D = spriteF.Result;
    spriteF = (Future<Sprite>) null;
    // ISSUE: reference to a compiler-generated method
    yield return (object) bugu005ReisouMixerMenu.\u003C\u003En__0();
  }

  protected override IEnumerator InitExtension()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu005ReisouMixerMenu bugu005ReisouMixerMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    bugu005ReisouMixerMenu.SetIconSize(123, 180, new Vector3(0.85f, 0.85f, 0.85f));
    bugu005ReisouMixerMenu.SelectItemList.Clear();
    bugu005ReisouMixerMenu.player = SMManager.Get<Player>();
    return false;
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem item = this.DisplayItems[inventoryItemIdx];
    itemIcon.SetRenseiIcon();
    itemIcon.ForBattle = item.Item.ForBattle;
    itemIcon.Favorite = item.Item.favorite;
    itemIcon.FusionPossible = itemIcon.ItemInfo.FusionPossible = false;
    itemIcon.Gray = this.IsGrayIcon(item);
    itemIcon.onClick = (Action<ItemIcon>) (x => this.SetIconOnClick(item.Item));
    if (this.DisableTouchIcon(item))
    {
      itemIcon.onClick = (Action<ItemIcon>) (_ => { });
      item.Gray = true;
    }
    if (item.Item.isWeaponMaterial)
      itemIcon.SetRenseiMaterialCount(item.Item.quantity);
    else
      itemIcon.SetRenseiMaterialCount(0);
    if (item.select)
      itemIcon.SetRenseiMaterialNum(item.Item.isTempSelectedCount ? item.Item.tempSelectedCount : 1);
    else
      itemIcon.SetRenseiMaterialNum(0);
    itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(((Bugu005ItemListMenuBase) this).ChangeDetailScene));
  }

  protected override void BottomInfoUpdate()
  {
    this.player = SMManager.Get<Player>();
    int reisouMixingCost = CalcItemCost.GetReisouMixingCost(this.SelectItemList);
    this.txtZenyNumber.SetTextLocalize(reisouMixingCost);
    ((UIWidget) this.txtZenyNumber).color = (long) reisouMixingCost < this.player.money ? Color.white : Color.red;
    int allSelectCount = this.getAllSelectCount();
    this.txtSelectNumber.SetTextLocalize("{0}/{1}".F((object) allSelectCount, (object) this.SelectMax));
    this.txtAcquisitionsValue.SetTextLocalize(this.getReisouJewel());
    ((UIButtonColor) this.ibtnYes).isEnabled = this.player.money >= (long) reisouMixingCost && allSelectCount > 0;
  }

  protected override void ChangeDetailScene(GameCore.ItemInfo item)
  {
    if (item == null)
      return;
    if (item.isReisou)
      this.OpenReisouDetailPopup(item);
    else if (item.isWeapon)
      Unit00443Scene.changeSceneForDrillingMaterial(true, item);
    else if (item.isWeaponMaterial)
      Guide01142Scene.changeScene(true, item);
    else
      Bugu00561Scene.changeScene(true, item);
  }

  protected override void SelectItemProc(GameCore.ItemInfo item)
  {
    InventoryItem byItemIndex = this.InventoryItems.FindByItemIndex(item);
    if (byItemIndex == null)
      return;
    byItemIndex.Item.isTempSelectedCount = item.isTempSelectedCount;
    byItemIndex.Item.tempSelectedCount = item.tempSelectedCount;
    if (byItemIndex.select)
    {
      this.RemoveSelectItem(byItemIndex);
      this.UpdateSelectItemIndexWithInfo();
    }
    else
    {
      if (this.getAllSelectCount() >= this.SelectMax)
        return;
      this.AddSelectItem(byItemIndex);
      this.UpdateSelectItemIndexWithInfo();
    }
  }

  protected override bool IsGrayIcon(InventoryItem item)
  {
    if (this.DisableTouchIcon(item))
      return true;
    return this.getAllSelectCount() >= this.selectMax ? !item.Gray : item.Gray;
  }

  protected int getAllSelectCount()
  {
    int allSelectCount = 0;
    foreach (InventoryItem selectItem in this.SelectItemList)
    {
      if (selectItem.Item.isTempSelectedCount)
        allSelectCount += selectItem.Item.tempSelectedCount;
      else
        ++allSelectCount;
    }
    return allSelectCount;
  }

  protected int getReisouJewel()
  {
    int reisouJewel = 0;
    foreach (InventoryItem selectItem in this.SelectItemList)
    {
      int num = 1;
      if (selectItem.Item.isTempSelectedCount)
        num = selectItem.Item.tempSelectedCount;
      reisouJewel += selectItem.Item.gear.rarity.combine_reisou_jewel * num;
    }
    return reisouJewel;
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
          icon.SetRenseiMaterialNum(inventoryItem.Item.isTempSelectedCount ? inventoryItem.Item.tempSelectedCount : 1);
          icon.Gray = this.IsGrayIcon(inventoryItem);
        }
        else
        {
          icon.SetRenseiMaterialNum(0);
          icon.Gray = this.IsGrayIcon(inventoryItem);
        }
      }
    }
  }

  private void SetIconOnClick(GameCore.ItemInfo info)
  {
    if (info.isWeaponMaterial)
      this.StartCoroutine(this.ShowMaterialsPopup(info));
    else
      this.SelectItemProc(info);
  }

  private IEnumerator ShowMaterialsPopup(GameCore.ItemInfo item)
  {
    Bugu005ReisouMixerMenu bugu005ReisouMixerMenu = this;
    if (!Singleton<PopupManager>.GetInstance().isOpen)
    {
      InventoryItem selectItem = bugu005ReisouMixerMenu.InventoryItems.FindByItemIndex(item);
      if (selectItem != null)
      {
        int allSelectCount = bugu005ReisouMixerMenu.getAllSelectCount();
        if (allSelectCount < bugu005ReisouMixerMenu.SelectMax || selectItem.select)
        {
          IEnumerator e;
          if (Object.op_Equality((Object) bugu005ReisouMixerMenu.popupReisouMixerPrefab, (Object) null))
          {
            Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_Mixer").Load<GameObject>();
            e = popupPrefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            bugu005ReisouMixerMenu.popupReisouMixerPrefab = popupPrefabF.Result;
            popupPrefabF = (Future<GameObject>) null;
          }
          int selectedCount = 0;
          if (selectItem.select)
            selectedCount = selectItem.Item.tempSelectedCount;
          GameObject popup = bugu005ReisouMixerMenu.popupReisouMixerPrefab.Clone();
          PopupReisouMixer component = popup.GetComponent<PopupReisouMixer>();
          popup.SetActive(false);
          int reisouMixingCost = CalcItemCost.GetReisouMixingCost(bugu005ReisouMixerMenu.SelectItemList);
          int maxCountLimit = bugu005ReisouMixerMenu.SelectMax - (allSelectCount - selectedCount);
          e = component.Init(selectItem, reisouMixingCost, maxCountLimit, selectedCount, new Action<InventoryItem, int>(bugu005ReisouMixerMenu.cbSelectMaterialNum));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          popup.SetActive(true);
          Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
        }
      }
    }
  }

  public void cbSelectMaterialNum(InventoryItem selectItem, int num)
  {
    if (num == 0)
    {
      if (!selectItem.select)
        return;
      this.RemoveSelectItem(selectItem);
      this.UpdateSelectItemIndexWithInfo();
    }
    else
    {
      selectItem.Item.isTempSelectedCount = true;
      selectItem.Item.tempSelectedCount = num;
      if (!selectItem.select)
        this.AddSelectItem(selectItem);
      this.UpdateSelectItemIndexWithInfo();
    }
  }

  public override void IbtnBack()
  {
    if (this.IsPush)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.backScene();
  }

  public void IbtnDecision()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowDecisionPopup());
  }

  private bool isSelectedHighRarity()
  {
    foreach (InventoryItem selectItem in this.SelectItemList)
    {
      if (selectItem.Item.gear.hasSpecificationOfEquipmentUnits || selectItem.Item.gear.rarity.index >= 4 || selectItem.Item.gearLevelLimit >= 6)
        return true;
    }
    return false;
  }

  private IEnumerator ShowDecisionPopup()
  {
    Bugu005ReisouMixerMenu bugu005ReisouMixerMenu = this;
    if (bugu005ReisouMixerMenu.isSelectedHighRarity())
    {
      Future<GameObject> ItemIconF;
      IEnumerator e;
      if (Object.op_Equality((Object) bugu005ReisouMixerMenu.m_ItemIconPrefab, (Object) null))
      {
        ItemIconF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
        e = ItemIconF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bugu005ReisouMixerMenu.m_ItemIconPrefab = ItemIconF.Result;
        ItemIconF = (Future<GameObject>) null;
      }
      GameObject obj;
      if (bugu005ReisouMixerMenu.SelectItemList.Count > 5)
      {
        if (Object.op_Equality((Object) bugu005ReisouMixerMenu.m_CheckMaterialPopupPrefab, (Object) null))
        {
          ItemIconF = new ResourceObject("Prefabs/popup/popup_Reisou_Mixer_Confirm").Load<GameObject>();
          e = ItemIconF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          bugu005ReisouMixerMenu.m_CheckMaterialPopupPrefab = ItemIconF.Result;
          ItemIconF = (Future<GameObject>) null;
        }
        obj = bugu005ReisouMixerMenu.m_CheckMaterialPopupPrefab.Clone();
      }
      else
      {
        if (Object.op_Equality((Object) bugu005ReisouMixerMenu.m_CheckMaterialPopupPrefabMini, (Object) null))
        {
          ItemIconF = new ResourceObject("Prefabs/popup/popup_Reisou_Mixer_Confirm_Mini").Load<GameObject>();
          e = ItemIconF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          bugu005ReisouMixerMenu.m_CheckMaterialPopupPrefabMini = ItemIconF.Result;
          ItemIconF = (Future<GameObject>) null;
        }
        obj = bugu005ReisouMixerMenu.m_CheckMaterialPopupPrefabMini.Clone();
      }
      // ISSUE: reference to a compiler-generated method
      e = obj.GetComponent<PopupReisouMixerConfirm>().Init(bugu005ReisouMixerMenu.SelectItemList, new Action(bugu005ReisouMixerMenu.\u003CShowDecisionPopup\u003Eb__34_2), bugu005ReisouMixerMenu.m_ItemIconPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(obj, isCloned: true);
      obj = (GameObject) null;
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      PopupCommonNoYes.Show(Consts.GetInstance().POPUP_REISOU_MIXER_DECISION_TITLE, Consts.GetInstance().POPUP_REISOU_MIXER_DECISION_MESSAGE, new Action(bugu005ReisouMixerMenu.\u003CShowDecisionPopup\u003Eb__34_0), (Action) (() => { }));
    }
    bugu005ReisouMixerMenu.IsPush = false;
  }

  private IEnumerator CallAPIMixer()
  {
    Bugu005ReisouMixerMenu bugu005ReisouMixerMenu = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    List<int> intList3 = new List<int>();
    foreach (InventoryItem selectItem in bugu005ReisouMixerMenu.SelectItemList)
    {
      if (selectItem.Item.isWeaponMaterial)
      {
        intList2.Add(selectItem.Item.itemID);
        intList3.Add(selectItem.Item.tempSelectedCount);
      }
      else
        intList1.Add(selectItem.Item.itemID);
    }
    Future<WebAPI.Response.ItemGearMixer> paramF = WebAPI.ItemGearMixer(intList3.ToArray(), intList1.ToArray(), intList2.ToArray(), (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = paramF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (paramF.Result != null)
    {
      ItemIcon.ClearCache();
      bugu005ReisouMixerMenu.UpdateInventoryItemList();
      e = bugu005ReisouMixerMenu.Init();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Equality((Object) bugu005ReisouMixerMenu.popupReisouMixerResultPrefab, (Object) null))
      {
        Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_Mixer_Result").Load<GameObject>();
        e = popupPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bugu005ReisouMixerMenu.popupReisouMixerResultPrefab = popupPrefabF.Result;
        popupPrefabF = (Future<GameObject>) null;
      }
      GameObject popup = bugu005ReisouMixerMenu.popupReisouMixerResultPrefab.Clone();
      PopupReisouMixerResult script = popup.GetComponent<PopupReisouMixerResult>();
      popup.SetActive(false);
      e = script.Init(paramF.Result.player_items, paramF.Result.reisou_jewel);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      yield return (object) null;
      script.scrollResetPosition();
    }
  }

  public override void IbtnClear()
  {
    if (this.IsPush)
      return;
    this.ClearSelectItem();
    this.DisplayIconAndBottomInfoUpdate();
  }

  public override void ClearSelectItem()
  {
    foreach (ItemIcon itemIcon in this.AllItemIcon)
    {
      if (itemIcon.ItemInfo != null)
      {
        itemIcon.ItemInfo.tempSelectedCount = 0;
        itemIcon.ItemInfo.isTempSelectedCount = false;
      }
    }
    foreach (InventoryItem displayItem in this.DisplayItems)
    {
      if (Object.op_Inequality((Object) displayItem.icon, (Object) null))
      {
        displayItem.icon.ItemInfo.tempSelectedCount = 0;
        displayItem.icon.ItemInfo.isTempSelectedCount = false;
      }
    }
    foreach (InventoryItem inventoryItem in this.InventoryItems)
    {
      if (inventoryItem.Item != null)
      {
        inventoryItem.Item.tempSelectedCount = 0;
        inventoryItem.Item.isTempSelectedCount = false;
      }
    }
    foreach (InventoryItem selectItem in this.SelectItemList)
    {
      selectItem.select = false;
      selectItem.Gray = false;
      selectItem.index = 0;
    }
    this.SelectItemList.Clear();
  }

  public void IbtnBatchSelection()
  {
    if (this.IsPushAndSet())
      return;
    this.BatchSelection();
    this.StartCoroutine(this.IsPushOff());
  }

  private void BatchSelection()
  {
    this.ClearSelectItem();
    int num1 = 0;
    foreach (InventoryItem displayItem in this.DisplayItems)
    {
      if (num1 < this.SelectMax)
      {
        if (!this.DisableTouchIcon(displayItem))
        {
          if (displayItem.Item.isWeaponMaterial)
          {
            if (!displayItem.select || !displayItem.Item.isTempSelectedCount || displayItem.Item.tempSelectedCount != displayItem.Item.quantity)
            {
              int num2 = Mathf.Min(this.SelectMax - num1, displayItem.Item.quantity - displayItem.Item.tempSelectedCount);
              if (num2 != 0)
              {
                displayItem.Item.isTempSelectedCount = true;
                displayItem.Item.tempSelectedCount += num2;
                num1 += num2;
                if (!displayItem.select)
                  this.AddSelectItem(displayItem);
              }
            }
          }
          else if (!displayItem.select)
          {
            ++num1;
            this.AddSelectItem(displayItem);
          }
        }
      }
      else
        break;
    }
    this.UpdateSelectItemIndexWithInfo();
  }

  public override void IbtnSort()
  {
    if (this.isDisabledSort || this.IsPush)
      return;
    this.sortPersistLog.setData(this.GetPersist().Data);
    this.ShowSortAndFilterPopup();
  }

  public override void Sort(
    ItemSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isEquipFirst)
  {
    if (this.GetPersist().Data.isDifference(this.sortPersistLog))
    {
      this.ClearSelectItem();
      this.DisplayIconAndBottomInfoUpdate();
    }
    base.Sort(type, order, isEquipFirst);
  }
}
