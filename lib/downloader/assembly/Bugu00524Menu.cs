// Decompiled with JetBrains decompiler
// Type: Bugu00524Menu
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
public class Bugu00524Menu : Bugu005SelectItemListMenuBase
{
  [SerializeField]
  private GameObject[] repairInterfaceGameObject;
  [SerializeField]
  private Bugu00524MultipleRepairInterface multipleRepairInterface;
  private GameObject popup0052Prefab;
  public bool isFromExplore;

  public GameObject GetItemIconPrefab() => this.ItemIconPrefab;

  public List<InventoryItem> GetSelectItem() => this.SelectItemList;

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu0052RepairSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.isWeapon() && x.broken)).ToList<PlayerItem>();
  }

  protected override IEnumerator InitExtension()
  {
    Bugu00524Menu menu = this;
    if (Object.op_Equality((Object) menu.popup0052Prefab, (Object) null))
    {
      Future<GameObject> prefab = Res.Prefabs.popup.popup_005_2__anim_popup01.Load<GameObject>();
      IEnumerator e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.popup0052Prefab = prefab.Result;
      prefab = (Future<GameObject>) null;
    }
    menu.multipleRepairInterface.Init(menu);
    menu.ClearSelectItem();
  }

  protected override void BottomInfoUpdate()
  {
    this.multipleRepairInterface.SetSelectItem(this.SelectItemList);
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

  private void RepairConfirmPopupYesBtnAction(bool isRareMadel, int price, int boostCnt)
  {
    if (this.SelectItemList == null || this.SelectItemList.Count == 0)
      return;
    this.StartCoroutine(this.RepairPoweredAPI(isRareMadel, price, boostCnt));
  }

  private IEnumerator ShowRepairConfirmPopup(
    Bugu0052MedalPopup.CurrencyKind kind,
    int price,
    int bCnt,
    Action<bool, int, int> act)
  {
    GameObject prefab = this.popup0052Prefab.Clone();
    prefab.GetComponent<Bugu0052MedalPopup>().Init(kind, price, bCnt, act);
    prefab.SetActive(false);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
    yield break;
  }

  public void ibtnRepairRareMedal(int rareMedalCnt)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ShowRepairConfirmPopup(Bugu0052MedalPopup.CurrencyKind.RareMedal, rareMedalCnt, 0, new Action<bool, int, int>(this.RepairConfirmPopupYesBtnAction)));
  }

  public void ibtnRepair(int price, int boostCnt)
  {
    if (this.IsPush)
      return;
    this.StartCoroutine(this.ShowRepairConfirmPopup(Bugu0052MedalPopup.CurrencyKind.Money, price, boostCnt, new Action<bool, int, int>(this.RepairConfirmPopupYesBtnAction)));
  }

  public IEnumerator RepairAPI()
  {
    Bugu00524Menu bugu00524Menu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.ItemGearRepair> future = WebAPI.ItemGearRepair(bugu00524Menu.SelectItemList.ToGearId().ToArray(), (Action<WebAPI.Response.UserError>) (e =>
      {
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result != null)
      {
        List<WebAPI.Response.ItemGearRepairRepair_results> result = ((IEnumerable<WebAPI.Response.ItemGearRepairRepair_results>) future.Result.repair_results).ToList<WebAPI.Response.ItemGearRepairRepair_results>();
        List<GameCore.ItemInfo> items = new List<GameCore.ItemInfo>();
        foreach (InventoryItem selectItem in bugu00524Menu.SelectItemList)
        {
          InventoryItem itm = selectItem;
          if (result.FirstOrDefault<WebAPI.Response.ItemGearRepairRepair_results>((Func<WebAPI.Response.ItemGearRepairRepair_results, bool>) (x => x.player_gear_id == itm.Item.itemID)) != null)
            items.Add(itm.Item);
        }
        e1 = bugu00524Menu.RepairUpdate();
        while (e1.MoveNext())
          yield return (object) null;
        e1 = (IEnumerator) null;
        if (bugu00524Menu.isFromExplore)
          Singleton<NGSceneManager>.GetInstance().clearStack();
        Bugu005415Scene.ChangeScene(true, items, result);
        future = (Future<WebAPI.Response.ItemGearRepair>) null;
        result = (List<WebAPI.Response.ItemGearRepairRepair_results>) null;
        items = (List<GameCore.ItemInfo>) null;
      }
    }
  }

  private IEnumerator RepairPoweredAPI(bool isReraMedal, int price, int boostCnt)
  {
    Bugu00524Menu bugu00524Menu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      int medal = isReraMedal ? price : 0;
      int bet = isReraMedal ? 0 : boostCnt;
      IEnumerator e1;
      if (bet == 0 && medal == 0)
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        e1 = bugu00524Menu.RepairAPI();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
      }
      else
      {
        int[] array = bugu00524Menu.SelectItemList.ToGearId().ToArray();
        Future<WebAPI.Response.ItemGearRepairList> future = WebAPI.ItemGearRepairList(bet, medal, array, (Action<WebAPI.Response.UserError>) (e =>
        {
          WebAPI.DefaultUserErrorCallback(e);
          MypageScene.ChangeSceneOnError();
        }));
        e1 = future.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (future.Result != null)
        {
          List<WebAPI.Response.ItemGearRepairListRepair_results> result = ((IEnumerable<WebAPI.Response.ItemGearRepairListRepair_results>) future.Result.repair_results).ToList<WebAPI.Response.ItemGearRepairListRepair_results>();
          List<GameCore.ItemInfo> items = new List<GameCore.ItemInfo>();
          foreach (InventoryItem selectItem in bugu00524Menu.SelectItemList)
          {
            InventoryItem itm = selectItem;
            if (result.FirstOrDefault<WebAPI.Response.ItemGearRepairListRepair_results>((Func<WebAPI.Response.ItemGearRepairListRepair_results, bool>) (x => x.player_gear_id == itm.Item.itemID)) != null)
              items.Add(itm.Item);
          }
          e1 = bugu00524Menu.RepairUpdate();
          while (e1.MoveNext())
            yield return (object) null;
          e1 = (IEnumerator) null;
          if (bugu00524Menu.isFromExplore)
            Singleton<NGSceneManager>.GetInstance().clearStack();
          Bugu005415Scene.ChangeScene(true, items, result);
          future = (Future<WebAPI.Response.ItemGearRepairList>) null;
          result = (List<WebAPI.Response.ItemGearRepairListRepair_results>) null;
          items = (List<GameCore.ItemInfo>) null;
        }
      }
    }
  }

  private IEnumerator RepairUpdate()
  {
    Bugu00524Menu bugu00524Menu1 = this;
    List<PlayerItem> itemList = bugu00524Menu1.GetItemList();
    List<PlayerMaterialGear> materialList = bugu00524Menu1.GetMaterialList();
    bugu00524Menu1.itemCount = bugu00524Menu1.GetItemListCnt(itemList, materialList);
    bugu00524Menu1.itemFavoriteCount = bugu00524Menu1.GetItemListFavoriteCnt(itemList);
    bugu00524Menu1.InventoryItems.Clear();
    bugu00524Menu1.CreateInvetoryItem(itemList, materialList);
    List<InventoryItem> selectItmList = new List<InventoryItem>();
    int num1 = 1;
    int num2 = bugu00524Menu1.SelectItemList.Count<InventoryItem>();
    Bugu00524Menu bugu00524Menu = bugu00524Menu1;
    for (int i = 0; i < num2; i++)
    {
      InventoryItem inventoryItem = bugu00524Menu1.InventoryItems.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemID == bugu00524Menu.SelectItemList[i].Item.itemID));
      if (inventoryItem != null)
      {
        inventoryItem.select = true;
        inventoryItem.index = num1;
        selectItmList.Add(inventoryItem);
      }
    }
    IEnumerator e = bugu00524Menu1.InitExtension();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    bugu00524Menu1.CreatePlayerItems();
    bugu00524Menu1.SelectItemList = selectItmList;
    bugu00524Menu1.UpdateSelectItemIndexWithInfo();
    yield return (object) bugu00524Menu1.ScrollViewReposition();
  }

  protected override void backScene()
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (this.isFromExplore)
    {
      if (instance.backScene())
        return;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitID = -1;
      Singleton<NGGameDataManager>.GetInstance().lastReferenceUnitIndex = -1;
      instance.destroyCurrentScene();
      instance.clearStack();
      Explore033TopScene.changeScene(false);
    }
    else
      base.backScene();
  }
}
