// Decompiled with JetBrains decompiler
// Type: Bugu005ItemListMenuBase
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
public class Bugu005ItemListMenuBase : BackButtonMenuBase
{
  protected int iconWidth = ItemIcon.Width;
  protected int iconHeight = ItemIcon.Height;
  protected int iconColumnValue = ItemIcon.ColumnValue;
  protected int iconRowValue = ItemIcon.RowValue;
  protected int iconScreenValue = ItemIcon.ScreenValue;
  protected int iconMaxValue = ItemIcon.MaxValue;
  protected GameObject ItemIconPrefab;
  protected bool InitializeEnd;
  protected int itemCount = -1;
  protected int itemFavoriteCount = -1;
  protected int itemEquipCount = -1;
  protected float scroolStartY;
  protected bool isUpdateIcon;
  protected Vector3 scale = new Vector3(1f, 1f, 1f);
  protected List<ItemIcon> AllItemIcon = new List<ItemIcon>();
  protected List<InventoryItem> InventoryItems = new List<InventoryItem>();
  protected List<InventoryItem> DisplayItems = new List<InventoryItem>();
  protected bool[] filter = new bool[45];
  private ItemSortAndFilter.SORT_TYPES sortCategory = ItemSortAndFilter.SORT_TYPES.BranchOfWeapon;
  private SortAndFilter.SORT_TYPE_ORDER_BUY orderBuySort;
  protected bool[] reisouFilter = new bool[23];
  private ReisouSortAndFilter.SORT_TYPES reisouSortCategory = ReisouSortAndFilter.SORT_TYPES.BranchOfWeapon;
  private SortAndFilter.SORT_TYPE_ORDER_BUY reisouOrderBuySort;
  public bool isEquipFirst = true;
  protected GameObject SortPopupPrefab;
  protected bool isInitSortPrefab;
  [Tooltip("ソート処理無効化フラグ")]
  public bool isDisabledSort;
  [SerializeField]
  protected NGxScroll2 scroll;
  [SerializeField]
  protected ItemSortAndFilter.SORT_TYPES CurrentSortType;
  [SerializeField]
  protected ReisouSortAndFilter.SORT_TYPES CurrentReisouSortType;
  [SerializeField]
  protected UISprite SortSprite;
  [SerializeField]
  protected GameObject dir_noList;
  [SerializeField]
  protected bool enabledExpireDate;
  protected GameObject reisouPopupDualSkillPrefab;
  protected GameObject reisouPopupPrefab;
  protected GameCore.ItemInfo removeReisouInfo;
  protected Action removeReisouCallback;
  protected bool isReisouRemovePossible;
  protected bool isReisouFusionPossible;
  protected bool isReisouDrillingPossible;
  protected List<int> equipedReisouIdList = new List<int>();
  protected Dictionary<int, int> fusionReisouGearIdList = new Dictionary<int, int>();
  protected Dictionary<int, int> awakeSkillReisouGearIdList = new Dictionary<int, int>();
  protected long? revisionItemList_;
  protected long? revisionMaterialList_;
  protected GameCore.ItemInfo baseInfo;

  public bool[] Filter
  {
    get => this.filter;
    set => this.filter = value;
  }

  public ItemSortAndFilter.SORT_TYPES SortCategory
  {
    get => this.sortCategory;
    set => this.sortCategory = value;
  }

  public SortAndFilter.SORT_TYPE_ORDER_BUY OrderBuySort
  {
    get => this.orderBuySort;
    set => this.orderBuySort = value;
  }

  public bool[] ReisouFilter
  {
    get => this.reisouFilter;
    set => this.reisouFilter = value;
  }

  public ReisouSortAndFilter.SORT_TYPES ReisouSortCategory
  {
    get => this.reisouSortCategory;
    set => this.reisouSortCategory = value;
  }

  public SortAndFilter.SORT_TYPE_ORDER_BUY ReisouOrderBuySort
  {
    get => this.reisouOrderBuySort;
    set => this.reisouOrderBuySort = value;
  }

  protected Future<GameObject> GetSortAndFilterPopupGameObject()
  {
    return Res.Prefabs.popup.popup_Item_Sort__anim_popup01.Load<GameObject>();
  }

  public virtual Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return (Persist<Persist.ItemSortAndFilterInfo>) null;
  }

  public virtual Persist<Persist.ReisouSortAndFilterInfo> GetReisouPersist()
  {
    return (Persist<Persist.ReisouSortAndFilterInfo>) null;
  }

  protected virtual List<PlayerItem> GetItemList() => (List<PlayerItem>) null;

  protected virtual long GetRevisionItemList() => 0;

  protected virtual List<PlayerMaterialGear> GetMaterialList() => (List<PlayerMaterialGear>) null;

  protected virtual long GetRevisionMaterialList() => 0;

  protected virtual void UpdateInvetoryItem(InventoryItem invItem, PlayerItem item)
  {
    invItem.Item.Set(item, this.enabledExpireDate);
  }

  protected virtual void UpdateInvetoryItem(InventoryItem invItem, PlayerMaterialGear item)
  {
    invItem.Item.Set(item);
  }

  protected virtual void UpdateInventoryItemList()
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
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
  }

  protected void SetIconSize(int iconWidth, int iconHeight, Vector3 scale)
  {
    this.iconWidth = iconWidth;
    this.iconHeight = iconHeight;
    this.scale = scale;
  }

  protected virtual void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem displayItem = this.DisplayItems[inventoryItemIdx];
    itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.ChangeDetailScene(playeritem.ItemInfo));
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
    itemIcon.Gray = false;
    itemIcon.SelectedQuantity(0);
    itemIcon.Deselect();
    itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(this.ChangeDetailScene));
  }

  protected virtual IEnumerator InitExtension()
  {
    this.removeReisouInfo = (GameCore.ItemInfo) null;
    this.removeReisouCallback = (Action) null;
    this.isReisouRemovePossible = false;
    this.isReisouFusionPossible = false;
    this.isReisouDrillingPossible = false;
    yield break;
  }

  protected virtual void BottomInfoUpdate()
  {
  }

  protected virtual void AllItemIconUpdate()
  {
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    this.backScene();
  }

  public void resetRevisions()
  {
    this.revisionItemList_ = new long?();
    this.revisionMaterialList_ = new long?();
  }

  public virtual IEnumerator Init()
  {
    Bugu005ItemListMenuBase menu = this;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    menu.InitializeEnd = false;
    IEnumerator e = menu.LoadItemIconPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!menu.isDisabledSort && !menu.isInitSortPrefab)
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
      menu.SortPopupPrefab.GetComponent<ItemSortAndFilter>().Initialize(menu);
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
    if (menu.itemCount != itemListCnt || !menu.revisionItemList_.HasValue || menu.revisionItemList_.Value != verItemList || !menu.revisionMaterialList_.HasValue || menu.revisionMaterialList_.Value != verMaterialList || menu.itemFavoriteCount != itemListFavoriteCnt && !menu.Filter[26] || menu.itemEquipCount != itemListEquipCount)
    {
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
    }
    else
      menu.UpdateInventoryItemList();
    yield return (object) menu.ScrollViewReposition();
    menu.InitializeEnd = true;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    if (Object.op_Inequality((Object) menu.dir_noList, (Object) null))
      menu.dir_noList.SetActive(menu.DisplayItems.Count <= 0);
  }

  public virtual void onEndScene() => Singleton<PopupManager>.GetInstance().closeAll();

  protected IEnumerator LoadItemIconPrefab()
  {
    if (Object.op_Equality((Object) this.ItemIconPrefab, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.ItemIconPrefab = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
  }

  protected IEnumerator LoadSpriteCache()
  {
    if (this.InventoryItems.Count > this.iconMaxValue)
    {
      for (int i = this.iconMaxValue; i < this.InventoryItems.Count; ++i)
      {
        IEnumerator e = ItemIcon.LoadSprite(this.InventoryItems[i].Item);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  protected int GetItemListCnt(List<PlayerItem> itemList, List<PlayerMaterialGear> materialItemList)
  {
    int itemListCnt = itemList != null ? itemList.Sum<PlayerItem>((Func<PlayerItem, int>) (x => !x.isWeapon() ? x.quantity : 1)) : 0;
    if (materialItemList != null)
      itemListCnt += materialItemList.Sum<PlayerMaterialGear>((Func<PlayerMaterialGear, int>) (x => x.quantity));
    return itemListCnt;
  }

  protected int GetItemListFavoriteCnt(List<PlayerItem> itemList)
  {
    return itemList != null ? itemList.Count<PlayerItem>((Func<PlayerItem, bool>) (x => x.favorite)) : 0;
  }

  protected int GetItemListEquipCount(List<PlayerItem> itemList)
  {
    return itemList != null ? itemList.Count<PlayerItem>((Func<PlayerItem, bool>) (x => x.ForBattle)) : 0;
  }

  protected void CreateInvetoryItem(
    List<PlayerItem> itemList,
    List<PlayerMaterialGear> materialItemList)
  {
    if (itemList != null)
    {
      foreach (PlayerItem target in itemList)
        this.InventoryItems.Add(this.CreateInventoryItem(target));
    }
    if (materialItemList == null)
      return;
    foreach (PlayerMaterialGear materialItem in materialItemList)
    {
      PlayerMaterialGear item = materialItem;
      this.InventoryItems.Add(new InventoryItem(item, this.InventoryItems.Count<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.itemID == item.id))));
    }
  }

  protected virtual InventoryItem CreateInventoryItem(PlayerItem target)
  {
    return new InventoryItem(target, this.enabledExpireDate);
  }

  protected void DisplayIconAndBottomInfoUpdate()
  {
    this.AllItemIconUpdate();
    this.BottomInfoUpdate();
  }

  protected void CreatePlayerItems()
  {
    this.scroll.Clear();
    this.AllItemIcon.Clear();
    for (int index = 0; index < Mathf.Min(ItemIcon.MaxValue, this.InventoryItems.Count); ++index)
    {
      ItemIcon component = Object.Instantiate<GameObject>(this.ItemIconPrefab).GetComponent<ItemIcon>();
      component.EnabledExpireDate = this.enabledExpireDate;
      this.AllItemIcon.Add(component);
    }
    this.Sort(this.SortCategory, this.OrderBuySort, this.isEquipFirst);
    this.scroolStartY = ((Component) this.scroll.scrollView).transform.localPosition.y;
    this.StartCoroutine(this.LoadSpriteCache());
  }

  protected IEnumerator ScrollViewReposition()
  {
    ((Component) this.scroll).gameObject.SetActive(false);
    yield return (object) null;
    ((Component) this.scroll).gameObject.SetActive(true);
    yield return (object) null;
    this.SetScrollPos();
  }

  protected void SetScrollPos(int ItemID = -1)
  {
    int targetItemID = ItemID == -1 ? Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID : ItemID;
    if (targetItemID != -1)
    {
      this.scroll.CreateScrollPoint(this.iconHeight, this.DisplayItems.Count);
      int? nullable = this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => !x.removeButton)).Select<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).FirstIndexOrNull<int>((Func<int, bool>) (x => x == targetItemID));
      if (nullable.HasValue)
      {
        this.scroll.ResolvePosition(nullable.Value, this.DisplayItems.Count<InventoryItem>());
      }
      else
      {
        int referenceItemIndex = Singleton<NGGameDataManager>.GetInstance().lastReferenceItemIndex;
        if (referenceItemIndex != -1 && referenceItemIndex < this.DisplayItems.Count)
          this.scroll.ResolvePosition(referenceItemIndex, this.DisplayItems.Count<InventoryItem>());
        else
          this.scroll.ResolvePosition(this.DisplayItems.Count<InventoryItem>() - 1, this.DisplayItems.Count<InventoryItem>());
      }
    }
    else
    {
      this.scroll.ResolvePosition();
      this.scroll.scrollView.UpdatePosition();
    }
    Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = -1;
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public override void onBackButton() => this.IbtnBack();

  protected virtual void ChangeDetailScene(GameCore.ItemInfo item)
  {
    if (item == null)
      return;
    if (item.isReisou)
      this.OpenReisouDetailPopup(item);
    else if (item.isWeapon)
      Unit00443Scene.changeScene(true, item, this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).ToList<InventoryItem>());
    else if (item.isWeaponMaterial)
    {
      List<InventoryItem> list = this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.isWeaponMaterial)).ToList<InventoryItem>();
      Dictionary<int, InventoryItem> dictionary = new Dictionary<int, InventoryItem>();
      for (int index = 0; index < list.Count; ++index)
      {
        if (!dictionary.ContainsKey(list[index].Item.itemID))
          dictionary.Add(list[index].Item.itemID, list[index]);
      }
      GearGear[] array1 = dictionary.Values.Select<InventoryItem, GearGear>((Func<InventoryItem, GearGear>) (x => x.Item.gear)).ToArray<GearGear>();
      int[] array2 = dictionary.Values.Select<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.quantity)).ToArray<int>();
      int index1 = Array.FindIndex<GearGear>(array1, (Predicate<GearGear>) (x => x == item.gear));
      int[] array3 = dictionary.Values.Select<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToArray<int>();
      Guide01142Scene.changeScene(true, array1, array2, array3, index1, true);
    }
    else
    {
      GameCore.ItemInfo[] array = this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => !x.Item.isReisou && !x.Item.isWeapon && !x.Item.isWeaponMaterial)).Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)).ToArray<GameCore.ItemInfo>();
      int index = Array.FindIndex<GameCore.ItemInfo>(array, (Predicate<GameCore.ItemInfo>) (x => x == item));
      Bugu00561Scene.changeScene(true, item, array, index);
    }
  }

  protected void OpenReisouDetailPopup(GameCore.ItemInfo item)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenReisouDetailPopupAsync(item));
  }

  protected IEnumerator OpenReisouDetailPopupAsync(GameCore.ItemInfo item)
  {
    Bugu005ItemListMenuBase itemListMenuBase = this;
    if (item == null)
    {
      itemListMenuBase.StartCoroutine(itemListMenuBase.IsPushOff());
    }
    else
    {
      itemListMenuBase.removeReisouInfo = item;
      GameObject popup;
      Future<GameObject> popupPrefabF;
      IEnumerator e;
      if (item.gear.isMythologyReisou())
      {
        if (Object.op_Equality((Object) itemListMenuBase.reisouPopupDualSkillPrefab, (Object) null))
        {
          popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails_DualSkill").Load<GameObject>();
          e = popupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          itemListMenuBase.reisouPopupDualSkillPrefab = popupPrefabF.Result;
          popupPrefabF = (Future<GameObject>) null;
        }
        popup = itemListMenuBase.reisouPopupDualSkillPrefab.Clone();
        PopupReisouDetailsDualSkill script = popup.GetComponent<PopupReisouDetailsDualSkill>();
        popup.SetActive(false);
        e = script.Init(item, (PlayerItem) null, true, itemListMenuBase.removeReisouCallback, itemListMenuBase.isReisouRemovePossible, itemListMenuBase.isReisouFusionPossible, itemListMenuBase.isReisouDrillingPossible, (PlayerItem) null);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
        yield return (object) null;
        script.scrollResetPosition();
        popup = (GameObject) null;
        script = (PopupReisouDetailsDualSkill) null;
      }
      else
      {
        if (Object.op_Equality((Object) itemListMenuBase.reisouPopupPrefab, (Object) null))
        {
          popupPrefabF = new ResourceObject("Prefabs/UnitGUIs/PopupReisouSkillDetails").Load<GameObject>();
          e = popupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          itemListMenuBase.reisouPopupPrefab = popupPrefabF.Result;
          popupPrefabF = (Future<GameObject>) null;
        }
        popup = itemListMenuBase.reisouPopupPrefab.Clone();
        PopupReisouDetails script = popup.GetComponent<PopupReisouDetails>();
        popup.SetActive(false);
        e = script.Init(item, removeCallback: itemListMenuBase.removeReisouCallback, isRemovePossible: itemListMenuBase.isReisouRemovePossible, isFusionPossible: itemListMenuBase.isReisouFusionPossible, isDrillingPossible: itemListMenuBase.isReisouDrillingPossible);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        popup.SetActive(true);
        Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
        yield return (object) null;
        script.scrollResetPosition();
        popup = (GameObject) null;
        script = (PopupReisouDetails) null;
      }
      itemListMenuBase.StartCoroutine(itemListMenuBase.IsPushOff());
    }
  }

  public virtual void IbtnSort()
  {
    if (this.isDisabledSort || this.IsPush)
      return;
    this.ShowSortAndFilterPopup();
  }

  protected void ShowSortAndFilterPopup()
  {
    if (!Singleton<PopupManager>.GetInstance().isOpen)
    {
      if (!Object.op_Inequality((Object) this.SortPopupPrefab, (Object) null))
        return;
      GameObject prefab = this.SortPopupPrefab.Clone();
      ItemSortAndFilter sortAndFilter = prefab.GetComponent<ItemSortAndFilter>();
      sortAndFilter.Initialize(this, true);
      sortAndFilter.SetItemNum(this.InventoryItems.FilterBy(this.filter).ToList<InventoryItem>(), this.InventoryItems);
      sortAndFilter.SortFilterItemNum = (Action) (() => sortAndFilter.SetItemNum(this.InventoryItems.FilterBy(this.filter).ToList<InventoryItem>(), this.InventoryItems));
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
    }
    else
      this.IsPush = false;
  }

  public virtual void Sort(
    ItemSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isEquipFirst)
  {
    this.CurrentSortType = type;
    if (!this.isDisabledSort && Object.op_Inequality((Object) this.SortSprite, (Object) null))
      this.SortSprite = ItemSortAndFilter.SortSpriteLabel(type, this.SortSprite);
    List<InventoryItem> self = new List<InventoryItem>();
    for (int index = this.InventoryItems.Count - 1; index >= 0; --index)
    {
      if (this.baseInfo != null && !this.baseInfo.playerItem.isLimitMax() && this.InventoryItems[index].Item.gear != null && GearGear.CanSpecialDrill(this.baseInfo.gear, this.InventoryItems[index].Item.gear))
      {
        self.Add(this.InventoryItems[index]);
        this.InventoryItems.Remove(this.InventoryItems[index]);
      }
    }
    for (int index = this.InventoryItems.Count - 1; index >= 0; --index)
    {
      if (!this.InventoryItems[index].removeButton && this.InventoryItems[index].Item.isDrilling)
      {
        self.Add(this.InventoryItems[index]);
        this.InventoryItems.Remove(this.InventoryItems[index]);
      }
    }
    if (this.equipedReisouIdList != null)
    {
      foreach (InventoryItem inventoryItem in this.InventoryItems)
      {
        if (inventoryItem.Item != null && inventoryItem.Item.isReisou)
          inventoryItem.Item.ForBattle = this.equipedReisouIdList.Contains(inventoryItem.Item.itemID);
      }
    }
    this.DisplayItems = this.isDisabledSort ? this.InventoryItems.ToList<InventoryItem>() : this.InventoryItems.FilterBy(this.filter).SortBy(type, order, isEquipFirst).ToList<InventoryItem>();
    if (!this.isDisabledSort)
      self = self.SortBy(type, order, isEquipFirst).ToList<InventoryItem>();
    for (int index = 0; index < self.Count; ++index)
      this.InventoryItems.Insert(index, self[index]);
    this.DisplayItems = (this.isDisabledSort ? (IEnumerable<InventoryItem>) self : (IEnumerable<InventoryItem>) self.FilterBy(this.filter).ToList<InventoryItem>()).Concat<InventoryItem>((IEnumerable<InventoryItem>) this.DisplayItems).ToList<InventoryItem>();
    this.scroll.Reset();
    foreach (ItemIcon itemIcon in this.AllItemIcon)
    {
      ((Component) itemIcon).transform.parent = ((Component) this).transform;
      ((Component) itemIcon).gameObject.SetActive(false);
    }
    int max = Mathf.Min(this.iconMaxValue, this.DisplayItems.Count);
    for (int index = 0; index < max; ++index)
    {
      this.scroll.Add(((Component) this.AllItemIcon[index]).gameObject, this.iconWidth, this.iconHeight);
      ((Component) this.AllItemIcon[index]).gameObject.SetActive(true);
      ((Component) this.AllItemIcon[index]).transform.localScale = this.scale;
    }
    this.InventoryItems.ForEach((Action<InventoryItem>) (v => v.icon = (ItemIcon) null));
    this.StartCoroutine(this.CreateItemIconRange(max));
    this.scroll.CreateScrollPoint(this.iconHeight, this.DisplayItems.Count);
    this.scroll.ResolvePosition();
    if (!Object.op_Inequality((Object) this.dir_noList, (Object) null))
      return;
    this.dir_noList.SetActive(this.DisplayItems.Count <= 0);
  }

  public virtual void ReisouSort(
    ReisouSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isEquipFirst)
  {
    this.CurrentReisouSortType = type;
    if (Object.op_Inequality((Object) this.SortSprite, (Object) null))
      this.SortSprite = ReisouSortAndFilter.SortSpriteLabel(type, this.SortSprite);
    if (this.equipedReisouIdList != null)
    {
      foreach (InventoryItem inventoryItem in this.InventoryItems)
      {
        if (inventoryItem.Item != null && inventoryItem.Item.isReisou)
          inventoryItem.Item.ForBattle = this.equipedReisouIdList.Contains(inventoryItem.Item.itemID);
      }
    }
    this.DisplayItems = this.InventoryItems.ReisouFilterBy(this.reisouFilter).ReisouSortBy(type, order, isEquipFirst).ToList<InventoryItem>();
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
      ((Component) this.AllItemIcon[index]).transform.localScale = this.scale;
    }
    this.InventoryItems.ForEach((Action<InventoryItem>) (v => v.icon = (ItemIcon) null));
    this.StartCoroutine(this.CreateItemIconRange(Mathf.Min(this.iconMaxValue, this.DisplayItems.Count)));
    this.scroll.CreateScrollPoint(this.iconHeight, this.DisplayItems.Count);
    this.scroll.ResolvePosition();
  }

  private void ScrollIconUpdate(int inventoryItemsIndex, int count)
  {
    if (this.DisplayItems[inventoryItemsIndex].removeButton || ItemIcon.IsCache(this.DisplayItems[inventoryItemsIndex].Item))
      this.CreateItemIconCache(inventoryItemsIndex, count);
    else
      this.StartCoroutine(this.CreateItemIcon(inventoryItemsIndex, count));
  }

  private void ScrollUpdate()
  {
    if ((!this.InitializeEnd || this.DisplayItems.Count <= this.iconScreenValue) && !this.isUpdateIcon)
      return;
    int num1 = this.iconHeight * 2;
    float num2 = ((Component) this.scroll.scrollView).transform.localPosition.y - this.scroolStartY;
    float num3 = (float) (Mathf.Max(0, this.DisplayItems.Count - this.iconScreenValue - 1) / this.iconColumnValue * this.iconHeight);
    float num4 = (float) (this.iconHeight * this.iconRowValue);
    if ((double) num2 < 0.0)
      num2 = 0.0f;
    if ((double) num2 > (double) num3)
      num2 = num3;
    bool flag;
    do
    {
      flag = false;
      int count = 0;
      foreach (GameObject gameObject in this.scroll)
      {
        GameObject item = gameObject;
        float num5 = item.transform.localPosition.y + num2;
        int? nullable = this.DisplayItems.FirstIndexOrNull<InventoryItem>((Func<InventoryItem, bool>) (v => Object.op_Inequality((Object) v.icon, (Object) null) && Object.op_Equality((Object) ((Component) v.icon).gameObject, (Object) item)));
        if ((double) num5 > (double) num1)
        {
          item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y - num4, 0.0f);
          if (nullable.HasValue && nullable.Value + this.iconMaxValue < (this.DisplayItems.Count + 4) / 5 * 5)
          {
            if (nullable.Value + this.iconMaxValue >= this.DisplayItems.Count)
              item.SetActive(false);
            else
              this.ScrollIconUpdate(nullable.Value + this.iconMaxValue, count);
            flag = true;
          }
        }
        else if ((double) num5 < -((double) num4 - (double) num1))
        {
          int num6 = this.iconMaxValue;
          if (!item.activeSelf)
          {
            item.SetActive(true);
            num6 = 0;
          }
          if (nullable.HasValue && nullable.Value - num6 >= 0)
          {
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y + num4, 0.0f);
            this.ScrollIconUpdate(nullable.Value - num6, count);
            flag = true;
          }
        }
        else if (this.isUpdateIcon)
          this.ScrollIconUpdate(nullable.Value, count);
        ++count;
      }
    }
    while (flag);
    if (!this.isUpdateIcon)
      return;
    this.isUpdateIcon = false;
  }

  private void ResetItemIcon(int allItemIdx)
  {
    ((Component) this.AllItemIcon[allItemIdx]).gameObject.SetActive(false);
  }

  protected IEnumerator CreateItemIconRange(int max)
  {
    Singleton<CommonRoot>.GetInstance().isTouchBlock = true;
    for (int index = 0; index < this.AllItemIcon.Count; ++index)
      ((Component) this.AllItemIcon[index]).gameObject.SetActive(false);
    for (int i = 0; i < max; ++i)
    {
      if (ItemIcon.IsCache(this.DisplayItems[i].Item))
      {
        this.CreateItemIconCache(i, i);
      }
      else
      {
        IEnumerator e = this.CreateItemIcon(i, i);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    for (int index = 0; index < max; ++index)
      ((Component) this.AllItemIcon[index]).gameObject.SetActive(true);
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
  }

  private IEnumerator CreateItemIcon(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (a => Object.op_Equality((Object) a.icon, (Object) itemIcon))).ForEach<InventoryItem>((Action<InventoryItem>) (b => b.icon = (ItemIcon) null));
    this.DisplayItems[inventoryItemIdx].icon = itemIcon;
    if (this.DisplayItems[inventoryItemIdx].removeButton)
    {
      itemIcon.InitByRemoveButton();
    }
    else
    {
      IEnumerator e = itemIcon.InitByItemInfo(this.DisplayItems[inventoryItemIdx].Item);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.CreateItemIconAdvencedSetting(inventoryItemIdx, allItemIdx);
    itemIcon.ShowBottomInfo(this.CurrentSortType);
  }

  private void CreateItemIconCache(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (a => Object.op_Equality((Object) a.icon, (Object) itemIcon))).ForEach<InventoryItem>((Action<InventoryItem>) (b => b.icon = (ItemIcon) null));
    this.DisplayItems[inventoryItemIdx].icon = itemIcon;
    if (this.DisplayItems[inventoryItemIdx].removeButton)
      itemIcon.InitByRemoveButton();
    else
      itemIcon.InitByItemInfoCache(this.DisplayItems[inventoryItemIdx].Item);
    this.CreateItemIconAdvencedSetting(inventoryItemIdx, allItemIdx);
    itemIcon.ShowBottomInfo(this.CurrentSortType);
  }

  protected void SetFusionReisouGearIdList(List<PlayerItem> playerItems)
  {
    this.fusionReisouGearIdList = new Dictionary<int, int>();
    PlayerItem[] array = playerItems.ToArray();
    foreach (PlayerItem playerItem in playerItems)
    {
      if (!this.fusionReisouGearIdList.ContainsKey(playerItem.entity_id) && playerItem.isReisouFusionPossible(array))
        this.fusionReisouGearIdList.Add(playerItem.entity_id, playerItem.entity_id);
    }
  }

  protected void setAwakeSkillReisouGearIdList(GameCore.ItemInfo gearInfo, List<PlayerItem> playerItems)
  {
    this.awakeSkillReisouGearIdList = new Dictionary<int, int>();
    List<int> intList = new List<int>();
    foreach (GearReisouSkillWeaponGroup skillWeaponGroup in MasterData.GearReisouSkillWeaponGroupList)
    {
      if (skillWeaponGroup.gear_GearGear == gearInfo.gear.ID)
        intList.Add(skillWeaponGroup.group);
    }
    if (!intList.Any<int>())
      return;
    playerItems.ToArray();
    foreach (PlayerItem playerItem in playerItems)
    {
      if (!this.awakeSkillReisouGearIdList.ContainsKey(playerItem.entity_id))
      {
        if (playerItem.isMythologyReisou())
        {
          GearReisouFusion fusionMineRecipe = playerItem.GetReisouFusionMineRecipe();
          GearGear holyId = fusionMineRecipe.holy_ID;
          GearGear chaosId = fusionMineRecipe.chaos_ID;
          int reisouHolyLv = playerItem.ReisouHolyLv;
          int reisouChaosLv = playerItem.ReisouChaosLv;
          foreach (GearReisouSkill gearReisouSkill in MasterData.GearReisouSkillList)
          {
            if (gearReisouSkill.awake_weapon_group != 0 && (gearReisouSkill.gear.ID == holyId.ID && gearReisouSkill.release_rank <= reisouHolyLv || gearReisouSkill.gear.ID == chaosId.ID && gearReisouSkill.release_rank <= reisouChaosLv) && this.isAwakeGroupContains(intList, gearReisouSkill))
            {
              this.awakeSkillReisouGearIdList.Add(playerItem.entity_id, playerItem.entity_id);
              break;
            }
          }
        }
        else
        {
          foreach (GearReisouSkill gearReisouSkill in MasterData.GearReisouSkillList)
          {
            if (gearReisouSkill.awake_weapon_group != 0 && gearReisouSkill.gear.ID == playerItem.entity_id && gearReisouSkill.release_rank <= playerItem.gear_level && this.isAwakeGroupContains(intList, gearReisouSkill))
            {
              this.awakeSkillReisouGearIdList.Add(playerItem.entity_id, playerItem.entity_id);
              break;
            }
          }
        }
      }
    }
  }

  protected bool isAwakeGroupContains(List<int> group_list, GearReisouSkill skill)
  {
    foreach (int group in group_list)
    {
      if (skill.awake_weapon_group == group)
        return true;
    }
    return false;
  }

  private enum SortFilterPopupMode
  {
    None,
    Full,
    Material,
    Bugu,
    AlchemistMaterial,
    CompseMaterial,
  }
}
