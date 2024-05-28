// Decompiled with JetBrains decompiler
// Type: Unit0044ReisouMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit0044ReisouMenu : Bugu005SelectItemListMenuBase
{
  [SerializeField]
  protected UILabel TxtNumberpossession;
  [SerializeField]
  protected GameObject dirNoItem;
  protected GameObject reisouRemovePopupPrefab;
  private GameCore.ItemInfo gearInfo;
  private GameCore.ItemInfo reisouInfo;
  private bool isEarthMode;

  public EditReisouParam EditParam { get; set; }

  public GameCore.ItemInfo GearInfo
  {
    set => this.gearInfo = value;
    get => this.gearInfo;
  }

  public GameCore.ItemInfo ReisouInfo
  {
    set => this.reisouInfo = value;
    get => this.reisouInfo;
  }

  public bool IsEarthMode
  {
    set => this.isEarthMode = value;
    get => this.isEarthMode;
  }

  public override IEnumerator Init()
  {
    Unit0044ReisouMenu menu = this;
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
    // ISSUE: reference to a compiler-generated method
    yield return (object) menu.\u003C\u003En__0();
  }

  public override Persist<Persist.ReisouSortAndFilterInfo> GetReisouPersist()
  {
    return Persist.bugu005ReisouListSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    List<PlayerItem> playerItems = new List<PlayerItem>();
    this.equipedReisouIdList = new List<int>();
    PlayerItem[] playerItemArray = this.EditParam?.reisous ?? SMManager.Get<PlayerItem[]>();
    int kindGearKind = this.gearInfo.gear.kind_GearKind;
    if (this.EditParam == null)
    {
      foreach (PlayerItem playerItem in playerItemArray)
      {
        if (playerItem.isWeapon())
        {
          if (playerItem.equipped_reisou_player_gear_id != 0)
            this.equipedReisouIdList.Add(playerItem.equipped_reisou_player_gear_id);
        }
        else if (playerItem.isReisou() && playerItem.gear.kind_GearKind == kindGearKind)
          playerItems.Add(playerItem);
      }
    }
    else
    {
      this.equipedReisouIdList.AddRange((IEnumerable<int>) this.EditParam.dicReference.Keys);
      foreach (PlayerItem playerItem in playerItemArray)
      {
        if (playerItem.isReisou() && playerItem.gear.kind_GearKind == kindGearKind)
          playerItems.Add(playerItem);
      }
    }
    this.SetFusionReisouGearIdList(playerItems);
    this.setAwakeSkillReisouGearIdList(this.gearInfo, playerItems);
    if (Object.op_Inequality((Object) this.dirNoItem, (Object) null))
      this.dirNoItem.SetActive(playerItems.Count <= 0);
    return playerItems;
  }

  protected override IEnumerator InitExtension()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit0044ReisouMenu unit0044ReisouMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (unit0044ReisouMenu.reisouInfo != null)
    {
      InventoryItem inventoryItem = new InventoryItem();
      unit0044ReisouMenu.InventoryItems.Insert(0, inventoryItem);
    }
    unit0044ReisouMenu.removeReisouInfo = (GameCore.ItemInfo) null;
    unit0044ReisouMenu.removeReisouCallback = new Action(unit0044ReisouMenu.cbRemoveReisou);
    return false;
  }

  protected void cbRemoveReisou()
  {
    int itemId1 = this.removeReisouInfo.itemID;
    int? itemId2 = this.reisouInfo?.itemID;
    int valueOrDefault = itemId2.GetValueOrDefault();
    if (itemId1 == valueOrDefault & itemId2.HasValue)
    {
      this.backScene();
    }
    else
    {
      this.equipedReisouIdList.Remove(this.removeReisouInfo.itemID);
      foreach (InventoryItem displayItem in this.DisplayItems)
      {
        if (displayItem.Item.itemID == this.removeReisouInfo.itemID)
        {
          displayItem.Item.ForBattle = false;
          displayItem.Gray = false;
          break;
        }
      }
      foreach (ItemIcon itemIcon in this.AllItemIcon)
      {
        if (itemIcon.ItemInfo.itemID == this.removeReisouInfo.itemID)
        {
          itemIcon.ForBattle = false;
          itemIcon.ItemInfo.ForBattle = false;
          itemIcon.Gray = false;
          itemIcon.SetupIconsBlink();
          itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.SelectItemProc(playeritem.ItemInfo));
          itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(((Bugu005ItemListMenuBase) this).OpenReisouDetailPopup));
          break;
        }
      }
    }
  }

  protected override void BottomInfoUpdate()
  {
    this.TxtNumberpossession.SetTextLocalize(string.Format("{0}/{1}", (object) ((IEnumerable<PlayerItem>) (this.EditParam?.reisous ?? SMManager.Get<PlayerItem[]>())).Count<PlayerItem>((Func<PlayerItem, bool>) (x => x.isReisou())), (object) SMManager.Get<Player>().max_reisou_items));
  }

  protected override void SelectItemProc(GameCore.ItemInfo item)
  {
    if (this.InventoryItems.FindByItem(item).removeButton)
      this.StartCoroutine(this.callEquipReisouAPI((GameCore.ItemInfo) null));
    else
      this.StartCoroutine(this.callEquipReisouAPI(item));
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem displayItem = this.DisplayItems[inventoryItemIdx];
    itemIcon.Gray = false;
    itemIcon.QuantitySupply = false;
    if (displayItem.removeButton)
    {
      itemIcon.Favorite = false;
      itemIcon.ForBattle = false;
      itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.StartCoroutine(this.callEquipReisouAPI((GameCore.ItemInfo) null)));
      itemIcon.DisableLongPressEvent();
    }
    else
    {
      itemIcon.ForBattle = this.equipedReisouIdList.FirstIndexOrNull<int>((Func<int, bool>) (x => x == itemIcon.ItemInfo.playerItem.id)).HasValue;
      itemIcon.ItemInfo.ForBattle = itemIcon.ForBattle;
      itemIcon.SetupIconsBlink();
      itemIcon.onClick = !this.IsGrayIcon(displayItem) ? (Action<ItemIcon>) (playeritem => this.SelectItemProc(playeritem.ItemInfo)) : (this.EditParam != null ? (Action<ItemIcon>) (playeritem =>
      {
        if (this.gearInfo.reisou != (PlayerItem) null && this.gearInfo.reisou.id == playeritem.ItemInfo.itemID)
          return;
        this.SelectItemProc(playeritem.ItemInfo);
      }) : (Action<ItemIcon>) (playeritem =>
      {
        if (this.gearInfo.reisou != (PlayerItem) null && this.gearInfo.reisou.id == playeritem.ItemInfo.itemID)
          return;
        foreach (PlayerItem beforeWeapon in SMManager.Get<PlayerItem[]>())
        {
          if (beforeWeapon.isWeapon() && beforeWeapon.equipped_reisou_player_gear_id == playeritem.ItemInfo.itemID)
            this.StartCoroutine(this.OpenReisouRemovePopupAsync(beforeWeapon, playeritem.ItemInfo, this.gearInfo));
        }
      }));
      itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(((Bugu005ItemListMenuBase) this).OpenReisouDetailPopup));
    }
  }

  public IEnumerator OpenReisouRemovePopupAsync(
    PlayerItem beforeWeapon,
    GameCore.ItemInfo reisou,
    GameCore.ItemInfo currWeapon)
  {
    Unit0044ReisouMenu unit0044ReisouMenu = this;
    IEnumerator e;
    if (Object.op_Equality((Object) unit0044ReisouMenu.reisouRemovePopupPrefab, (Object) null))
    {
      Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_change_target").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit0044ReisouMenu.reisouRemovePopupPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    GameObject popup = unit0044ReisouMenu.reisouRemovePopupPrefab.Clone();
    PopupReisouChangeTarget component = popup.GetComponent<PopupReisouChangeTarget>();
    popup.SetActive(false);
    e = component.Init(beforeWeapon, reisou, currWeapon);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    yield return (object) null;
    unit0044ReisouMenu.StartCoroutine(unit0044ReisouMenu.IsPushOff());
  }

  protected override bool DisableTouchIcon(InventoryItem item)
  {
    return item.Item == null || item.Item.ForBattle;
  }

  public override void Sort(
    ItemSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isEquipFirst)
  {
    this.CurrentReisouSortType = this.ReisouSortCategory;
    if (Object.op_Inequality((Object) this.SortSprite, (Object) null))
      this.SortSprite = ReisouSortAndFilter.SortSpriteLabel(this.CurrentReisouSortType, this.SortSprite);
    if (this.equipedReisouIdList != null)
    {
      foreach (InventoryItem inventoryItem in this.InventoryItems)
      {
        if (inventoryItem.Item != null)
        {
          inventoryItem.Item.ForBattle = this.equipedReisouIdList.Contains(inventoryItem.Item.itemID);
          inventoryItem.Item.AwakeReisouSkill = this.awakeSkillReisouGearIdList.ContainsKey(inventoryItem.Item.masterID);
        }
      }
    }
    this.DisplayItems = this.InventoryItems.ReisouFilterBy(this.reisouFilter).ReisouSortBy(this.CurrentReisouSortType, order, isEquipFirst).ToList<InventoryItem>();
    if (this.gearInfo.reisou != (PlayerItem) null)
    {
      for (int index = 0; index < this.DisplayItems.Count; ++index)
      {
        if (this.DisplayItems[index].Item != null && this.gearInfo.reisou.id == this.DisplayItems[index].Item.itemID)
        {
          this.DisplayItems.Remove(this.DisplayItems[index]);
          break;
        }
      }
    }
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

  private IEnumerator callEquipReisouAPI(GameCore.ItemInfo item)
  {
    Unit0044ReisouMenu unit0044ReisouMenu = this;
    if (unit0044ReisouMenu.EditParam != null)
    {
      unit0044ReisouMenu.onSelectedCustomDeckReisou(item);
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      int num1;
      int num2;
      if (item != null)
        num2 = num1 = item.itemID;
      else
        num1 = num2 = 0;
      int num3 = num2;
      IEnumerator e = WebAPI.ItemGearReisouEquip(unit0044ReisouMenu.gearInfo.itemID, new int?(num3), (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        if (error == null)
          return;
        WebAPI.DefaultUserErrorCallback(error);
      })).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      switch (GuildUtil.gvgPopupState)
      {
        case GuildUtil.GvGPopupState.None:
label_19:
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          unit0044ReisouMenu.backScene();
          yield break;
        case GuildUtil.GvGPopupState.AtkTeam:
          // ISSUE: reference to a compiler-generated method
          e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id, new Action(unit0044ReisouMenu.\u003CcallEquipReisouAPI\u003Eb__30_1));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        case GuildUtil.GvGPopupState.DefTeam:
          // ISSUE: reference to a compiler-generated method
          e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id, new Action(unit0044ReisouMenu.\u003CcallEquipReisouAPI\u003Eb__30_2));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        default:
          unit0044ReisouMenu.backScene();
          break;
      }
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      goto label_19;
    }
  }

  private void onSelectedCustomDeckReisou(GameCore.ItemInfo info)
  {
    this.EditParam.onSetReisou(this.EditParam.index, this.EditParam.slotNo, info != null ? info.playerItem.id : 0);
    this.backScene();
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
}
