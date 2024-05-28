// Decompiled with JetBrains decompiler
// Type: Bugu00525Menu
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
public class Bugu00525Menu : Bugu005SelectItemListMenuBase
{
  [SerializeField]
  protected UIButton DecisionButton;
  [SerializeField]
  protected UILabel NumProsession3;
  [SerializeField]
  protected UILabel NumSelectedCount3;
  [SerializeField]
  protected UILabel NumSpendZenie3;
  private GameObject popup005711Prefab;
  private GameObject popup005513Prefab;
  private GameObject popup005514Prefab;
  private GameObject popup005512Prefab;
  private Bugu00525Scene.Mode mode = Bugu00525Scene.Mode.Material;
  [SerializeField]
  protected UILabel TitleLabel;
  [SerializeField]
  private GameObject SortButton;
  [SerializeField]
  private GameObject TitleBarShort;
  [SerializeField]
  private GameObject TitleBarLong;
  [SerializeField]
  protected UILabel AnnotationLabel;
  [SerializeField]
  protected UISprite spriteButtonTxt;
  [SerializeField]
  protected GameObject dirNoItem;
  private bool needClearCache = true;

  public void EnableSortButton(bool enable)
  {
    if (Object.op_Inequality((Object) this.SortButton, (Object) null))
      this.SortButton.SetActive(enable);
    if (Object.op_Inequality((Object) this.TitleBarShort, (Object) null))
      this.TitleBarShort.SetActive(enable);
    if (!Object.op_Inequality((Object) this.TitleBarLong, (Object) null))
      return;
    this.TitleBarLong.SetActive(!enable);
  }

  public IEnumerator Init(Bugu00525Scene.Mode mode)
  {
    Bugu00525Menu menu = this;
    menu.mode = mode;
    switch (menu.mode)
    {
      case Bugu00525Scene.Mode.Weapon:
        menu.spriteButtonTxt.ChangeSprite("slc_button_text_material_28pt.png__GUI__button_text__button_text_prefab");
        menu.TitleLabel.SetTextLocalize(Consts.GetInstance().TitleWeaponSell);
        menu.AnnotationLabel.SetTextLocalize(Consts.GetInstance().AnnotationWeaponSell);
        menu.EnableSortButton(true);
        break;
      case Bugu00525Scene.Mode.WeaponMaterial:
        menu.spriteButtonTxt.ChangeSprite("slc_button_text_bugu_28pt.png__GUI__button_text__button_text_prefab");
        menu.TitleLabel.SetTextLocalize(Consts.GetInstance().TitleWeaponMaterialSell);
        menu.AnnotationLabel.SetTextLocalize(Consts.GetInstance().AnnotationItemSell);
        menu.EnableSortButton(true);
        break;
      case Bugu00525Scene.Mode.Material:
        menu.TitleLabel.SetTextLocalize(Consts.GetInstance().TitleMaterialSell);
        menu.AnnotationLabel.SetTextLocalize(Consts.GetInstance().AnnotationItemSell);
        menu.EnableSortButton(true);
        break;
      case Bugu00525Scene.Mode.Supply:
        menu.TitleLabel.SetTextLocalize(Consts.GetInstance().TitleSupplySell);
        menu.AnnotationLabel.SetTextLocalize(Consts.GetInstance().AnnotationItemSell);
        menu.EnableSortButton(false);
        break;
      case Bugu00525Scene.Mode.Reisou:
        menu.TitleLabel.SetTextLocalize(Consts.GetInstance().TitleReisouSell);
        menu.AnnotationLabel.SetTextLocalize(Consts.GetInstance().AnnotationReisouSell);
        menu.EnableSortButton(true);
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
        break;
    }
    // ISSUE: reference to a compiler-generated method
    yield return (object) menu.\u003C\u003En__0();
  }

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    switch (this.mode)
    {
      case Bugu00525Scene.Mode.Weapon:
        return Persist.bugu0052SortAndFilter;
      case Bugu00525Scene.Mode.WeaponMaterial:
        return Persist.bugu005WeaponMaterialListSortAndFilter;
      case Bugu00525Scene.Mode.Material:
        return Persist.bugu005MaterialListSortAndFilter;
      case Bugu00525Scene.Mode.Supply:
        return Persist.bugu005SupplyListSortAndFilter;
      case Bugu00525Scene.Mode.Reisou:
        return (Persist<Persist.ItemSortAndFilterInfo>) null;
      default:
        Debug.LogError((object) "Invalid Mode Selected");
        return Persist.bugu005MaterialListSortAndFilter;
    }
  }

  public override Persist<Persist.ReisouSortAndFilterInfo> GetReisouPersist()
  {
    return this.mode == Bugu00525Scene.Mode.Reisou ? Persist.bugu005ReisouListSortAndFilter : (Persist<Persist.ReisouSortAndFilterInfo>) null;
  }

  protected override List<PlayerItem> GetItemList()
  {
    switch (this.mode)
    {
      case Bugu00525Scene.Mode.Weapon:
        return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !x.isSupply() && !x.isReisou())).ToList<PlayerItem>();
      case Bugu00525Scene.Mode.WeaponMaterial:
        return (List<PlayerItem>) null;
      case Bugu00525Scene.Mode.Material:
        return (List<PlayerItem>) null;
      case Bugu00525Scene.Mode.Supply:
        List<PlayerItem> list1 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.isSupply())).ToList<PlayerItem>();
        List<PlayerItem> list2 = list1.Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id == 1)).ToList<PlayerItem>();
        foreach (IGrouping<int, PlayerItem> grouping in list1.Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.box_type_id != 1)).GroupBy<PlayerItem, int>((Func<PlayerItem, int>) (x => x.supply.ID)))
        {
          PlayerItem pi = grouping.FirstOrDefault<PlayerItem>();
          if (!(pi == (PlayerItem) null))
          {
            pi = pi.Clone();
            pi.quantity = 0;
            grouping.ForEach<PlayerItem>((Action<PlayerItem>) (item => pi.quantity += item.quantity));
            list2.Add(pi);
          }
        }
        return list2;
      case Bugu00525Scene.Mode.Reisou:
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
        return playerItems;
      default:
        return (List<PlayerItem>) null;
    }
  }

  protected override long GetRevisionItemList()
  {
    switch (this.mode)
    {
      case Bugu00525Scene.Mode.Weapon:
      case Bugu00525Scene.Mode.Supply:
        return SMManager.Revision<PlayerItem[]>();
      default:
        return 0;
    }
  }

  protected override List<PlayerMaterialGear> GetMaterialList()
  {
    switch (this.mode)
    {
      case Bugu00525Scene.Mode.Weapon:
        return (List<PlayerMaterialGear>) null;
      case Bugu00525Scene.Mode.WeaponMaterial:
        return ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.isWeaponMaterial())).ToList<PlayerMaterialGear>();
      case Bugu00525Scene.Mode.Material:
        return ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => !x.isWeaponMaterial() && !x.isCallGift())).ToList<PlayerMaterialGear>();
      case Bugu00525Scene.Mode.Supply:
        return (List<PlayerMaterialGear>) null;
      case Bugu00525Scene.Mode.Reisou:
        return (List<PlayerMaterialGear>) null;
      default:
        return (List<PlayerMaterialGear>) null;
    }
  }

  protected override long GetRevisionMaterialList()
  {
    switch (this.mode)
    {
      case Bugu00525Scene.Mode.WeaponMaterial:
      case Bugu00525Scene.Mode.Material:
        return SMManager.Revision<PlayerMaterialGear[]>();
      default:
        return 0;
    }
  }

  protected override IEnumerator InitExtension()
  {
    Bugu00525Menu bugu00525Menu = this;
    Future<GameObject> prefab;
    IEnumerator e;
    if (Object.op_Equality((Object) bugu00525Menu.popup005711Prefab, (Object) null))
    {
      prefab = Res.Prefabs.popup.popup_005_7_11__anim_popup01.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu00525Menu.popup005711Prefab = prefab.Result;
      prefab = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) bugu00525Menu.popup005513Prefab, (Object) null))
    {
      prefab = Res.Prefabs.popup.popup_005_5_13__anim_popup01.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu00525Menu.popup005513Prefab = prefab.Result;
      prefab = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) bugu00525Menu.popup005514Prefab, (Object) null))
    {
      prefab = Res.Prefabs.popup.popup_005_5_14__anim_popup01.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu00525Menu.popup005514Prefab = prefab.Result;
      prefab = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) bugu00525Menu.popup005512Prefab, (Object) null))
    {
      prefab = Res.Prefabs.popup.popup_005_5_12__anim_popup01.Load<GameObject>();
      e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bugu00525Menu.popup005512Prefab = prefab.Result;
      prefab = (Future<GameObject>) null;
    }
    bugu00525Menu.SelectItemList.Clear();
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    base.CreateItemIconAdvencedSetting(inventoryItemIdx, allItemIdx);
    if (this.mode != Bugu00525Scene.Mode.Reisou)
      return;
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem displayItem = this.DisplayItems[inventoryItemIdx];
    displayItem.Item.ForBattle = this.equipedReisouIdList.Contains(displayItem.Item.itemID);
    itemIcon.ForBattle = displayItem.Item.ForBattle;
    displayItem.Item.FusionPossible = this.fusionReisouGearIdList.ContainsKey(displayItem.Item.masterID);
    itemIcon.FusionPossible = displayItem.Item.FusionPossible;
    itemIcon.SetupIconsBlink();
  }

  protected override void BottomInfoUpdate()
  {
    Player player = SMManager.Get<Player>();
    int num1;
    int num2;
    if (this.mode == Bugu00525Scene.Mode.Reisou)
    {
      num1 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Count<PlayerItem>((Func<PlayerItem, bool>) (x => x.isReisou()));
      num2 = player.max_reisou_items;
    }
    else
    {
      num1 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Count<PlayerItem>((Func<PlayerItem, bool>) (x => !x.isSupply() && !x.isReisou()));
      num2 = player.max_items;
    }
    long saleValue = 0;
    this.SelectItemList.ForEach((Action<InventoryItem>) (item => saleValue += item.GetSellPrice()));
    this.NumSpendZenie3.SetTextLocalize(saleValue);
    ((UIWidget) this.NumSpendZenie3).color = saleValue + player.money <= Consts.GetInstance().MONEY_MAX ? Color.white : Color.red;
    int selectItemNum = 0;
    selectItemNum = this.SelectItemList.Count<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.isWeaponOrReisou));
    this.SelectItemList.Where<InventoryItem>((Func<InventoryItem, bool>) (x => !x.Item.isWeapon)).ForEach<InventoryItem>((Action<InventoryItem>) (item => selectItemNum += item.selectCount));
    this.NumSelectedCount3.SetTextLocalize(selectItemNum);
    ((UIWidget) this.NumSelectedCount3).color = selectItemNum < this.SelectMax ? Color.white : Color.red;
    this.NumProsession3.SetTextLocalize(Consts.Format(Consts.GetInstance().GEAR_0052_POSSESSION, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) num1
      },
      {
        (object) "max",
        (object) num2
      }
    }));
    if (Object.op_Inequality((Object) this.dirNoItem, (Object) null))
      this.dirNoItem.SetActive(num1 <= 0);
    ((UIButtonColor) this.DecisionButton).isEnabled = selectItemNum > 0 && this.SelectMax >= selectItemNum;
  }

  protected override void SelectItemProc(GameCore.ItemInfo item)
  {
    InventoryItem byItem = this.InventoryItems.FindByItem(item);
    if (byItem == null)
      return;
    if (byItem.select)
    {
      if (!item.isWeaponOrReisou)
      {
        this.StartCoroutine(this.CountSelectPopUp(byItem));
      }
      else
      {
        this.RemoveSelectItem(byItem);
        this.UpdateSelectItemIndexWithInfo();
      }
    }
    else
    {
      if (this.SelectItemList.Count >= this.SelectMax)
        return;
      if (!item.isWeaponOrReisou)
      {
        this.StartCoroutine(this.CountSelectPopUp(byItem));
      }
      else
      {
        this.AddSelectItem(byItem);
        this.UpdateSelectItemIndexWithInfo();
      }
    }
  }

  public virtual void IbtnDecision()
  {
    if (this.IsPushAndSet())
      return;
    if (this.SelectItemList.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.gear != null)).Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.gear.rarity.index >= 3)).ToList<InventoryItem>().Count > 0)
      this.StartCoroutine(this.SellWarningPopUp(new Action(this.CallSellAPI)));
    else
      this.StartCoroutine(this.SellCheckPopUp(new Action(this.CallSellAPI)));
  }

  private IEnumerator CountSelectPopUp(InventoryItem item)
  {
    Bugu00525Menu menu = this;
    GameObject popup = menu.popup005711Prefab.Clone();
    IEnumerator e = popup.GetComponent<Bugu005711Menu>().InitSceneAsync(item, menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(false);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    popup.SetActive(true);
  }

  protected IEnumerator SellCheckPopUp(Action action)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu00525Menu bugu00525Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Player player = SMManager.Get<Player>();
    long saleValue = 0;
    bugu00525Menu.SelectItemList.ForEach((Action<InventoryItem>) (item => saleValue += item.GetSellPrice()));
    GameObject prefab = bugu00525Menu.popup005513Prefab.Clone();
    prefab.GetComponent<Popup005513Menu>().Init(action, saleValue + player.money > Consts.GetInstance().MONEY_MAX);
    prefab.SetActive(false);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
    return false;
  }

  protected IEnumerator SellWarningPopUp(Action action)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu00525Menu bugu00525Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Player player = SMManager.Get<Player>();
    long saleValue = 0;
    bugu00525Menu.SelectItemList.ForEach((Action<InventoryItem>) (item => saleValue += item.GetSellPrice()));
    GameObject prefab = bugu00525Menu.popup005514Prefab.Clone();
    prefab.GetComponent<Popup005513Menu>().Init(action, saleValue + player.money > Consts.GetInstance().MONEY_MAX);
    prefab.SetActive(false);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
    return false;
  }

  protected IEnumerator SellResultPopUp(long resultMoney)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu00525Menu bugu00525Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    bugu00525Menu.IsPush = true;
    GameObject prefab = bugu00525Menu.popup005512Prefab.Clone();
    prefab.GetComponent<Popup005512Menu>().SetTextWithMoney(resultMoney);
    prefab.SetActive(false);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    prefab.SetActive(true);
    return false;
  }

  public void SetSellCount(InventoryItem item, int count)
  {
    item.selectCount = count;
    if (count != 0)
    {
      if (!this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == item)))
        this.AddSelectItem(item);
    }
    else if (this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == item)))
      this.RemoveSelectItem(item);
    this.UpdateSelectItemIndexWithInfo();
  }

  public virtual void CallSellAPI() => this.StartCoroutine(this.ExecuteSellAPI());

  private IEnumerator ExecuteSellAPI()
  {
    Bugu00525Menu bugu00525Menu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      long beforeMoney = SMManager.Get<Player>().money;
      Future<WebAPI.Response.ItemSell> future = WebAPI.ItemSell(bugu00525Menu.SelectItemList.ToEntityIdByMaterial().ToArray(), bugu00525Menu.SelectItemList.ToSelectQuantityByMaterial().ToArray(), bugu00525Menu.SelectItemList.ToGearId().ToArray(), bugu00525Menu.SelectItemList.ToEntityIdBySupply().ToArray(), bugu00525Menu.SelectItemList.ToSelectQuantityBySupply().ToArray(), (Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result != null)
      {
        e1 = bugu00525Menu.Init();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        yield return (object) new WaitForSeconds(0.5f);
        long resultMoney = future.Result.player.money - beforeMoney;
        e1 = bugu00525Menu.SellResultPopUp(resultMoney);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        future = (Future<WebAPI.Response.ItemSell>) null;
      }
    }
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
      List<PlayerItem> itemList = this.GetItemList();
      foreach (InventoryItem inventoryItem in array1)
      {
        InventoryItem invItem = inventoryItem;
        PlayerItem playerItem = itemList.Find((Predicate<PlayerItem>) (x => x.id == invItem.Item.itemID));
        if (playerItem != (PlayerItem) null)
          this.UpdateInvetoryItem(invItem, playerItem);
      }
    }
    InventoryItem[] array2 = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
    {
      if (x.Item == null || x.removeButton)
        return false;
      return x.Item.isCompse || x.Item.isExchangable;
    })).ToArray<InventoryItem>();
    if (array2 != null && ((IEnumerable<InventoryItem>) array2).Any<InventoryItem>())
    {
      PlayerMaterialGear[] array3 = SMManager.Get<PlayerMaterialGear[]>();
      foreach (InventoryItem inventoryItem in array2)
      {
        InventoryItem invItem = inventoryItem;
        PlayerMaterialGear playerMaterialGear = Array.Find<PlayerMaterialGear>(array3, (Predicate<PlayerMaterialGear>) (x => x.id == invItem.Item.itemID));
        if (playerMaterialGear != (PlayerMaterialGear) null)
          this.UpdateInvetoryItem(invItem, playerMaterialGear);
      }
    }
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

  public override void Sort(
    ItemSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isEquipFirst)
  {
    if (this.mode != Bugu00525Scene.Mode.Reisou)
      base.Sort(type, order, isEquipFirst);
    else
      this.ReisouSort(this.ReisouSortCategory, this.ReisouOrderBuySort, isEquipFirst);
  }

  protected virtual void OnEnable()
  {
    if (!this.scroll.scrollView.isDragging)
      return;
    this.scroll.scrollView.Press(false);
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Persist.sortOrder.Flush();
    if (!this.needClearCache)
      return;
    ItemIcon.ClearCache();
  }

  public override void IbtnBack()
  {
    this.needClearCache = false;
    base.IbtnBack();
  }

  public void IbtnStorage()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    switch (this.mode)
    {
      case Bugu00525Scene.Mode.Weapon:
        Bugu00525Scene.ChangeScene(false, Bugu00525Scene.Mode.WeaponMaterial);
        break;
      case Bugu00525Scene.Mode.WeaponMaterial:
        Bugu00525Scene.ChangeScene(false, Bugu00525Scene.Mode.Weapon);
        break;
    }
  }

  public override void IbtnSort()
  {
    if (this.mode == Bugu00525Scene.Mode.Reisou)
    {
      if (this.IsPush)
        return;
      this.ShowReisouSortAndFilterPopup();
    }
    else
      base.IbtnSort();
  }

  private void ShowReisouSortAndFilterPopup()
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
}
