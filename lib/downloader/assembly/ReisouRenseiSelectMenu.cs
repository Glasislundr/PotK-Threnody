// Decompiled with JetBrains decompiler
// Type: ReisouRenseiSelectMenu
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
public class ReisouRenseiSelectMenu : Bugu005SelectItemListMenuBase
{
  private const int subWidth = 123;
  private const int subHeight = 180;
  public UIButton AutoRenseiBtn;
  private const int SELECT_MAX = 99;
  [SerializeField]
  protected UILabel NumSpendZenie3;
  [SerializeField]
  protected UILabel NumSelectedCount3;
  [SerializeField]
  protected UILabel NumProsession3;
  [SerializeField]
  protected UILabel TxtHolyReisouAddValue;
  [SerializeField]
  protected UILabel TxtChaosReisouAddValue;
  [SerializeField]
  private UIButton ibtnYes;
  private ReisouRenseiSelectMenu.DrillingType drillingType;
  private GameCore.ItemInfo BaseItem;
  private List<InventoryItem> FirstSelectItemList = new List<InventoryItem>();
  private bool firstInit = true;
  private int addRenseiValue;
  private int addHolyValue;
  private int addChaosValue;
  private int currentRenseiMaxExp;
  private int currentSeihouMaxExp;
  private int currentKontonMaxExp;
  private int residualRenseiExp;
  private int residualSeihouExp;
  private int residualKontonExp;
  private int maxUpNum;
  private Player player;
  private BuguMaterialsPopup materialPopup;
  private int[] reisouRenseiMasterID = new int[18]
  {
    9010001,
    9020001,
    9030001,
    9040001,
    9050001,
    9060001,
    9070001,
    9080001,
    9310001,
    9320001,
    9330001,
    9340001,
    9350001,
    9360001,
    9370001,
    9380001,
    9080002,
    9380002
  };

  public void SetFirstSelectItem(
    ReisouRenseiSelectMenu.DrillingType dType,
    List<InventoryItem> items,
    GameCore.ItemInfo target)
  {
    this.drillingType = dType;
    this.BaseItem = target;
    this.baseInfo = this.BaseItem;
    this.FirstSelectItemList = items;
    this.SelectMax = 99;
  }

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.bugu0052DrillingMaterialSortAndFilter;
  }

  protected override List<PlayerItem> GetItemList()
  {
    List<PlayerItem> itemList = new List<PlayerItem>();
    this.equipedReisouIdList = new List<int>();
    bool flag1 = false;
    if (this.BaseItem.playerItem.gear_level_limit == this.BaseItem.playerItem.gear_level_limit_max && this.BaseItem.playerItem.gear_level == this.BaseItem.playerItem.gear_level_limit)
      flag1 = true;
    bool flag2 = false;
    if (this.BaseItem.playerItem.isMythologyReisou())
    {
      PlayerMythologyGearStatus mythologyGearStatus = this.BaseItem.playerItem.GetPlayerMythologyGearStatus();
      if (mythologyGearStatus.holy_gear_level >= mythologyGearStatus.holy_gear_level_limit && mythologyGearStatus.chaos_gear_level >= mythologyGearStatus.chaos_gear_level_limit)
        flag2 = true;
    }
    else if (this.BaseItem.isEquipReisouLvMax)
      flag2 = true;
    if (flag1 & flag2)
      return itemList;
    GearReisouType reisouType = this.BaseItem.gear.reisou_type;
    foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
    {
      if (playerItem.isWeapon() && playerItem.equipped_reisou_player_gear_id != 0)
        this.equipedReisouIdList.Add(playerItem.equipped_reisou_player_gear_id);
      if (playerItem.isReisou())
      {
        if (flag2 || reisouType != GearReisouType.mythology && playerItem.gear.reisou_type != reisouType)
          continue;
      }
      else if (flag1)
        continue;
      if (this.drillingType == ReisouRenseiSelectMenu.DrillingType.Normal)
      {
        if (this.IsActiveDrillingItem(playerItem))
          itemList.Add(playerItem);
      }
      else if (this.IsActiveSpecialDrillingItem(playerItem))
        itemList.Add(playerItem);
    }
    List<PlayerItem> playerItems = new List<PlayerItem>();
    foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
    {
      if (playerItem.isReisou())
        playerItems.Add(playerItem);
    }
    this.SetFusionReisouGearIdList(playerItems);
    return itemList;
  }

  private bool IsActiveDrillingItem(PlayerItem item)
  {
    return item.isReisou() && this.BaseItem != null && item.id != this.BaseItem.itemID && item.gear != null && this.IsActiveDrillingIcon(item.gear);
  }

  private bool IsActiveDrillingMaterial(PlayerMaterialGear item)
  {
    return item.gear.isReisou() && this.BaseItem != null && item.id != this.BaseItem.itemID && item.gear != null && this.IsActiveDrillingIcon(item.gear);
  }

  private bool IsActiveDrillingIcon(GearGear gear)
  {
    if (gear.disappearance_num.HasValue || !gear.kind.isEquip && gear.kind.Enum != GearKindEnum.drilling && gear.kind.Enum != GearKindEnum.sea_present)
      return false;
    return gear.isReisou() ? !this.BaseItem.isEquipReisouLvMax : gear.kind.Enum == this.BaseItem.gear.kind.Enum || gear.kind.Enum == GearKindEnum.drilling || gear.kind.Enum == GearKindEnum.sea_present;
  }

  private bool IsActiveSpecialDrillingItem(PlayerItem item)
  {
    if (this.BaseItem == null || item.id == this.BaseItem.itemID || item.gear == null)
      return false;
    if (!item.isReisou())
      return GearGear.CanSpecialDrill(this.BaseItem.gear, item.gear);
    return !this.BaseItem.isEquipReisouLvMax;
  }

  private bool IsActiveSpecialDrillingMaterial(PlayerMaterialGear item)
  {
    if (this.BaseItem == null || item.id == this.BaseItem.itemID || item.gear == null || item.gear.kind.Enum == GearKindEnum.special_drilling && (this.BaseItem.gear.rarity_GearRarity != item.gear.rarity_GearRarity || this.BaseItem.playerItem.isLimitMax()))
      return false;
    if (!item.gear.drilling_exp_mythology_id.HasValue)
      return GearGear.CanSpecialDrill(this.BaseItem.gear, item.gear);
    if (this.BaseItem.playerItem.isMythologyReisou())
    {
      PlayerMythologyGearStatus mythologyGearStatus = this.BaseItem.playerItem.GetPlayerMythologyGearStatus();
      if (mythologyGearStatus.holy_gear_level >= mythologyGearStatus.holy_gear_level_limit && mythologyGearStatus.chaos_gear_level >= mythologyGearStatus.chaos_gear_level_limit)
        return false;
    }
    else if (this.BaseItem.isEquipReisouLvMax || item.gear.drilling_exp_mythology_id.HasValue && item.gear.drilling_exp_mythology_id.Value != 0 && ((IEnumerable<GearDrillingExpMythology>) MasterData.GearDrillingExpMythologyList).FirstOrDefault<GearDrillingExpMythology>((Func<GearDrillingExpMythology, bool>) (x => x.ID == item.gear.drilling_exp_mythology_id.Value)).reisou_type_id != this.BaseItem.playerItem.gear.reisou_type)
      return false;
    return true;
  }

  protected override List<PlayerMaterialGear> GetMaterialList()
  {
    List<PlayerMaterialGear> res = new List<PlayerMaterialGear>();
    if (this.drillingType == ReisouRenseiSelectMenu.DrillingType.Normal)
      ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => this.IsActiveDrillingMaterial(x) || this.IsActiveSpecialDrillingMaterial(x))).ForEach<PlayerMaterialGear>((Action<PlayerMaterialGear>) (x => res.Add(x)));
    else
      ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => this.IsActiveSpecialDrillingMaterial(x))).ForEach<PlayerMaterialGear>((Action<PlayerMaterialGear>) (x => res.Add(x)));
    return res;
  }

  public override void Sort(
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
    for (int index = 0; index < this.DisplayItems.Count; ++index)
    {
      if (this.DisplayItems[index].Item.isReisou && this.baseInfo != null && this.baseInfo.reisou != (PlayerItem) null && this.baseInfo.reisou.id == this.DisplayItems[index].Item.itemID)
        this.DisplayItems.Remove(this.DisplayItems[index]);
    }
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

  protected override IEnumerator InitExtension()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ReisouRenseiSelectMenu renseiSelectMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    renseiSelectMenu.SetIconSize(123, 180, new Vector3(0.85f, 0.85f, 0.85f));
    if (renseiSelectMenu.firstInit)
    {
      renseiSelectMenu.firstInit = false;
      renseiSelectMenu.SelectItemList.Clear();
      renseiSelectMenu.AutoRenseiBtn.onClick.Clear();
      // ISSUE: reference to a compiler-generated method
      renseiSelectMenu.AutoRenseiBtn.onClick.Add(new EventDelegate(new EventDelegate.Callback(renseiSelectMenu.\u003CInitExtension\u003Eb__38_0)));
      if (renseiSelectMenu.FirstSelectItemList != null && renseiSelectMenu.FirstSelectItemList.Count > 0)
      {
        // ISSUE: reference to a compiler-generated method
        renseiSelectMenu.FirstSelectItemList = renseiSelectMenu.FirstSelectItemList.Where<InventoryItem>(new Func<InventoryItem, bool>(renseiSelectMenu.\u003CInitExtension\u003Eb__38_1)).ToList<InventoryItem>();
      }
      // ISSUE: reference to a compiler-generated method
      renseiSelectMenu.FirstSelectItemList.ForEach(new Action<InventoryItem>(renseiSelectMenu.\u003CInitExtension\u003Eb__38_2));
    }
    else if (renseiSelectMenu.SelectItemList != null && renseiSelectMenu.SelectItemList.Count > 0)
    {
      // ISSUE: reference to a compiler-generated method
      List<InventoryItem> list = renseiSelectMenu.SelectItemList.Where<InventoryItem>(new Func<InventoryItem, bool>(renseiSelectMenu.\u003CInitExtension\u003Eb__38_5)).ToList<InventoryItem>();
      renseiSelectMenu.SelectItemList.Clear();
      // ISSUE: reference to a compiler-generated method
      Action<InventoryItem> action = new Action<InventoryItem>(renseiSelectMenu.\u003CInitExtension\u003Eb__38_6);
      list.ForEach(action);
    }
    if (renseiSelectMenu.BaseItem.playerItem != (PlayerItem) null)
    {
      if (renseiSelectMenu.BaseItem.playerItem.isMythologyReisou())
      {
        PlayerMythologyGearStatus mythologyGearStatus = renseiSelectMenu.BaseItem.playerItem.GetPlayerMythologyGearStatus();
        // ISSUE: reference to a compiler-generated method
        renseiSelectMenu.currentSeihouMaxExp = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>(new Func<ReisouRankExp, bool>(renseiSelectMenu.\u003CInitExtension\u003Eb__38_7)).from_exp;
        // ISSUE: reference to a compiler-generated method
        renseiSelectMenu.currentKontonMaxExp = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>(new Func<ReisouRankExp, bool>(renseiSelectMenu.\u003CInitExtension\u003Eb__38_8)).from_exp;
        renseiSelectMenu.residualSeihouExp = renseiSelectMenu.currentSeihouMaxExp - mythologyGearStatus.holy_gear_total_exp;
        renseiSelectMenu.residualKontonExp = renseiSelectMenu.currentKontonMaxExp - mythologyGearStatus.chaos_gear_total_exp;
      }
      else if (renseiSelectMenu.BaseItem.playerItem.isHolyReisou())
      {
        // ISSUE: reference to a compiler-generated method
        renseiSelectMenu.currentSeihouMaxExp = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>(new Func<ReisouRankExp, bool>(renseiSelectMenu.\u003CInitExtension\u003Eb__38_3)).from_exp;
        renseiSelectMenu.residualSeihouExp = renseiSelectMenu.currentSeihouMaxExp - renseiSelectMenu.BaseItem.playerItem.gear_total_exp;
      }
      else if (renseiSelectMenu.BaseItem.playerItem.isChaosReisou())
      {
        // ISSUE: reference to a compiler-generated method
        renseiSelectMenu.currentKontonMaxExp = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>(new Func<ReisouRankExp, bool>(renseiSelectMenu.\u003CInitExtension\u003Eb__38_4)).from_exp;
        renseiSelectMenu.residualKontonExp = renseiSelectMenu.currentKontonMaxExp - renseiSelectMenu.BaseItem.playerItem.gear_total_exp;
      }
    }
    renseiSelectMenu.player = SMManager.Get<Player>();
    renseiSelectMenu.removeReisouCallback = new Action(renseiSelectMenu.cbRemoveReisou);
    renseiSelectMenu.isReisouRemovePossible = true;
    return false;
  }

  protected override void CreateItemIconAdvencedSetting(int inventoryItemIdx, int allItemIdx)
  {
    ItemIcon itemIcon = this.AllItemIcon[allItemIdx];
    InventoryItem item = this.DisplayItems[inventoryItemIdx];
    itemIcon.SetRenseiIcon();
    if (!itemIcon.Gray && itemIcon.onClick == null)
      itemIcon.onClick = (Action<ItemIcon>) (x => this.SetIconOnClick(item.Item));
    if (item.Item.isSupply || item.Item.isExchangable || item.Item.isCompse || item.Item.isWeaponMaterial)
      itemIcon.SetRenseiMaterialCount(item.Item.quantity);
    else
      itemIcon.SetRenseiMaterialCount(0);
    itemIcon.ForBattle = item.Item.ForBattle;
    itemIcon.Favorite = item.Item.favorite;
    itemIcon.FusionPossible = itemIcon.ItemInfo.FusionPossible = false;
    if (!this.JudgeRensei(item.Item) && !item.select && !item.Item.ForBattle && !item.Item.favorite && !item.Item.broken)
    {
      if (this.IsMaxUp(itemIcon.ItemInfo) && this.SelectItemList.Count < this.selectMax && this.JudgeMaxUp())
      {
        itemIcon.Gray = false;
        if (itemIcon.onClick == null)
          itemIcon.onClick = (Action<ItemIcon>) (x => this.SetIconOnClick(item.Item));
      }
      else
      {
        itemIcon.Gray = true;
        itemIcon.onClick = (Action<ItemIcon>) null;
      }
    }
    else
      itemIcon.Gray = this.IsGrayIcon(item);
    if (this.DisableTouchIcon(item))
    {
      itemIcon.onClick = (Action<ItemIcon>) (_ => { });
      item.Gray = true;
    }
    if (item.select)
    {
      if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Check)
      {
        itemIcon.SelectByCheckIcon();
        itemIcon.SelectedQuantity(item.selectCount);
      }
      else
        itemIcon.SetRenseiMaterialNum(item.Item.isTempSelectedCount ? item.Item.tempSelectedCount : 1);
    }
    else
    {
      itemIcon.SelectedQuantity(0);
      if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Check)
        itemIcon.DeselectByCheckIcon();
      else
        itemIcon.SetRenseiMaterialNum(0);
    }
    itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(((Bugu005ItemListMenuBase) this).ChangeDetailScene));
    itemIcon.SetRenseiMaxUpMark(this.IsMaxUp(itemIcon.ItemInfo));
    if (!itemIcon.Gray && itemIcon.onClick == null)
      itemIcon.onClick = (Action<ItemIcon>) (x => this.SetIconOnClick(item.Item));
    if (itemIcon.ItemInfo == null || !itemIcon.ItemInfo.isReisou)
      return;
    itemIcon.ItemInfo.ForBattle = this.equipedReisouIdList.Contains(itemIcon.ItemInfo.itemID);
    itemIcon.ForBattle = itemIcon.ItemInfo.ForBattle;
    if (itemIcon.ForBattle)
    {
      itemIcon.onClick = (Action<ItemIcon>) (_ => { });
      itemIcon.Gray = true;
    }
    itemIcon.ItemInfo.FusionPossible = this.fusionReisouGearIdList.ContainsKey(itemIcon.ItemInfo.masterID);
    itemIcon.FusionPossible = itemIcon.ItemInfo.FusionPossible;
    itemIcon.SetupIconsBlink();
  }

  protected void cbRemoveReisou()
  {
    this.equipedReisouIdList.Remove(this.removeReisouInfo.itemID);
    foreach (InventoryItem displayItem in this.DisplayItems)
    {
      if (displayItem.Item.reisou != (PlayerItem) null && displayItem.Item.reisou.id == this.removeReisouInfo.itemID)
        displayItem.icon.gear.dynReisouEffect.transform.Clear();
      if (displayItem.Item.itemID == this.removeReisouInfo.itemID)
      {
        displayItem.Item.ForBattle = false;
        displayItem.Gray = false;
        break;
      }
    }
    foreach (ItemIcon itemIcon in this.AllItemIcon)
    {
      if (itemIcon.ItemInfo != null && itemIcon.ItemInfo.itemID == this.removeReisouInfo.itemID)
      {
        itemIcon.ItemInfo.ForBattle = false;
        itemIcon.ForBattle = itemIcon.ItemInfo.ForBattle;
        itemIcon.FusionPossible = itemIcon.ItemInfo.FusionPossible;
        itemIcon.SetupIconsBlink();
        itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.SelectItemProc(playeritem.ItemInfo));
        itemIcon.Gray = false;
      }
    }
  }

  protected override void BottomInfoUpdate()
  {
    this.player = SMManager.Get<Player>();
    NGGameDataManager.Boost boostInfo = Singleton<NGGameDataManager>.GetInstance().BoostInfo;
    Decimal num1 = boostInfo == null ? 1.0M : boostInfo.getDiscountGearDrilling(this.BaseItem?.gear);
    List<InventoryItem> items = new List<InventoryItem>();
    List<InventoryItem> inventoryItemList1 = new List<InventoryItem>();
    int count = this.SelectItemList.Count;
    for (int index = 0; index < count; ++index)
    {
      InventoryItem selectItem = this.SelectItemList[index];
      List<InventoryItem> inventoryItemList2;
      if (!selectItem.Item.isReisou)
      {
        if (selectItem.Item.gear.drilling_exp_mythology_id.HasValue)
        {
          int? drillingExpMythologyId = selectItem.Item.gear.drilling_exp_mythology_id;
          int num2 = 0;
          if (!(drillingExpMythologyId.GetValueOrDefault() == num2 & drillingExpMythologyId.HasValue))
            goto label_5;
        }
        inventoryItemList2 = items;
        goto label_6;
      }
label_5:
      inventoryItemList2 = inventoryItemList1;
label_6:
      List<InventoryItem> inventoryItemList3 = inventoryItemList2;
      if (selectItem.Item.isTempSelectedCount && this.IsMaxUp(selectItem.Item))
        inventoryItemList3.AddRange(Enumerable.Repeat<InventoryItem>(selectItem, selectItem.Item.tempSelectedCount));
      else
        inventoryItemList3.Add(selectItem);
    }
    int num3 = (int) (num1 * (Decimal) CalcItemCost.GetDrillingCost(this.BaseItem, items));
    if (inventoryItemList1.Any<InventoryItem>())
      num3 += (int) CalcItemCost.GetDrillingCost(this.BaseItem, inventoryItemList1);
    this.NumSpendZenie3.SetTextLocalize(num3);
    ((UIWidget) this.NumSpendZenie3).color = (long) num3 < this.player.money ? Color.white : Color.red;
    this.NumSelectedCount3.SetTextLocalize("{0}/{1}".F((object) this.SelectItemList.Count<InventoryItem>(), (object) 99));
    this.NumProsession3.SetTextLocalize(this.addRenseiValue);
    Dictionary<GearReisouType, int> outExp = new Dictionary<GearReisouType, int>()
    {
      {
        GearReisouType.holy,
        0
      },
      {
        GearReisouType.chaos,
        0
      }
    };
    foreach (InventoryItem selectItem in this.SelectItemList)
    {
      if ((selectItem.Item.isDrilling || selectItem.Item.gear.kind.Enum == GearKindEnum.sea_present) && selectItem.Item.gear.drilling_exp_mythology_id.HasValue)
      {
        for (int index = 0; index < selectItem.Item.tempSelectedCount; ++index)
          this.calcReisouExp(ref outExp, selectItem.Item.playerItem, selectItem.Item.gear);
      }
      else
        this.calcReisouExp(ref outExp, selectItem.Item.playerItem, selectItem.Item.gear);
    }
    this.TxtHolyReisouAddValue.SetTextLocalize(outExp[GearReisouType.holy]);
    this.TxtChaosReisouAddValue.SetTextLocalize(outExp[GearReisouType.chaos]);
    ((UIButtonColor) this.ibtnYes).isEnabled = this.player.money >= (long) num3;
  }

  private int reisouValue(GearGear gear, int total_exp, Decimal boostRate = 1.0M)
  {
    int num = 0;
    if (!gear.drilling_exp_mythology_id.HasValue || gear.drilling_exp_mythology_id.Value == 0)
      num = ReisouDrilling.GetReisouDrilling(gear.rarity.index);
    return (int) (boostRate * Math.Floor((Decimal) num + (Decimal) total_exp) * (Decimal) gear.drilling_rate);
  }

  protected override void ChangeDetailScene(GameCore.ItemInfo item)
  {
    if (item == null)
      return;
    if (item.isReisou)
      this.OpenReisouDetailPopup(item);
    else if (item.isWeapon)
      Unit00443Scene.changeSceneForDrillingMaterial(true, item, this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.isWeapon)).ToList<InventoryItem>());
    else if (item.isWeaponMaterial)
    {
      IEnumerable<InventoryItem> source = this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.isWeaponMaterial));
      GearGear[] array1 = source.Select<InventoryItem, GearGear>((Func<InventoryItem, GearGear>) (x => x.Item.gear)).ToArray<GearGear>();
      int[] array2 = source.Select<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.quantity)).ToArray<int>();
      int[] array3 = source.Select<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToArray<int>();
      int index = Array.FindIndex<GearGear>(array1, (Predicate<GearGear>) (x => x == item.gear));
      Guide01142Scene.changeScene(true, array1, array2, array3, index, item.isWeaponMaterial);
    }
    else
    {
      GameCore.ItemInfo[] array = this.DisplayItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => !x.Item.isReisou && !x.Item.isWeapon && !x.Item.isWeaponMaterial)).Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)).ToArray<GameCore.ItemInfo>();
      int index = Array.FindIndex<GameCore.ItemInfo>(array, (Predicate<GameCore.ItemInfo>) (x => x == item));
      Bugu00561Scene.changeScene(true, item, array, index);
    }
  }

  protected override void SelectItemProc(GameCore.ItemInfo item)
  {
    InventoryItem byItemIndex = this.InventoryItems.FindByItemIndex(item);
    if (byItemIndex != null)
    {
      byItemIndex.Item.isTempSelectedCount = item.isTempSelectedCount;
      byItemIndex.Item.tempSelectedCount = item.tempSelectedCount;
      if (byItemIndex.select)
        this.RemoveSelectItem(byItemIndex);
      else if (this.SelectItemList.Count < this.SelectMax)
        this.AddSelectItem(byItemIndex);
    }
    this.UpdateSelectItemIndexWithInfo();
  }

  protected override void AllItemIconUpdate()
  {
    NGGameDataManager.Boost boostInfo = Singleton<NGGameDataManager>.GetInstance().BoostInfo;
    this.addRenseiValue = (int) ((boostInfo == null ? 1.0M : boostInfo.getBonusRateGearDrilling(this.BaseItem?.gear)) * this.SelectItemList.Sum<InventoryItem>((Func<InventoryItem, Decimal>) (x => !x.Item.isReisou && !x.Item.gear.drilling_exp_mythology_id.HasValue ? Math.Floor((Decimal) GearDrilling.GetGearDrilling(x.Item.gearLevel, x.Item.gear.rarity.index) * (Decimal) x.Item.gear.drilling_rate * (Decimal) (x.Item.isTempSelectedCount ? x.Item.tempSelectedCount : 1)) : 0M)));
    Dictionary<GearReisouType, int> outExp = new Dictionary<GearReisouType, int>()
    {
      {
        GearReisouType.holy,
        0
      },
      {
        GearReisouType.chaos,
        0
      }
    };
    this.maxUpNum = 0;
    for (int index1 = 0; index1 < this.SelectItemList.Count; ++index1)
    {
      InventoryItem selectItem = this.SelectItemList[index1];
      if (this.IsMaxUp(selectItem.Item))
      {
        if (!this.IsMaterialPopup(selectItem.Item))
        {
          ++this.maxUpNum;
        }
        else
        {
          for (int index2 = 0; index2 < selectItem.Item.tempSelectedCount; ++index2)
            ++this.maxUpNum;
        }
      }
      if ((selectItem.Item.isDrilling || selectItem.Item.gear.kind.Enum == GearKindEnum.sea_present) && selectItem.Item.gear.drilling_exp_mythology_id.HasValue)
      {
        for (int index3 = 0; index3 < selectItem.Item.tempSelectedCount; ++index3)
          this.calcReisouExp(ref outExp, selectItem.Item.playerItem, selectItem.Item.gear);
      }
      else
        this.calcReisouExp(ref outExp, selectItem.Item.playerItem, selectItem.Item.gear);
    }
    this.addHolyValue = outExp[GearReisouType.holy];
    this.addChaosValue = outExp[GearReisouType.chaos];
    foreach (ItemIcon itemIcon in this.AllItemIcon)
    {
      ItemIcon icon = itemIcon;
      InventoryItem inventoryItem = this.InventoryItems.Find((Predicate<InventoryItem>) (x => x.Item == icon.ItemInfo));
      if (inventoryItem != null)
      {
        if (inventoryItem.select)
        {
          if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Check)
          {
            icon.SelectedQuantity(inventoryItem.selectCount);
            icon.SelectByCheckIcon();
          }
          else
            icon.SetRenseiMaterialNum(inventoryItem.Item.isTempSelectedCount ? inventoryItem.Item.tempSelectedCount : 1);
          if (!this.JudgeRensei(inventoryItem.Item) && !inventoryItem.select && !inventoryItem.Item.ForBattle && !inventoryItem.Item.favorite && !inventoryItem.Item.broken)
          {
            if (this.IsMaxUp(icon.ItemInfo) && this.SelectItemList.Count < this.selectMax && this.JudgeMaxUp())
            {
              icon.Gray = false;
              if (icon.onClick == null)
                icon.onClick = (Action<ItemIcon>) (x => this.SetIconOnClick(icon.ItemInfo));
            }
            else
            {
              icon.Gray = true;
              icon.onClick = (Action<ItemIcon>) null;
            }
          }
          else
            icon.Gray = this.IsGrayIcon(inventoryItem);
        }
        else
        {
          if (this.SelectMode == Bugu005SelectItemListMenuBase.SelectModeEnum.Check)
          {
            icon.SelectQuantity = false;
            icon.DeselectByCheckIcon();
          }
          else
            icon.SetRenseiMaterialNum(0);
          if (!this.JudgeRensei(inventoryItem.Item) && !inventoryItem.select && !inventoryItem.Item.ForBattle && !inventoryItem.Item.favorite && !inventoryItem.Item.broken)
          {
            if (this.IsMaxUp(icon.ItemInfo) && this.SelectItemList.Count < this.selectMax && this.JudgeMaxUp())
            {
              icon.Gray = false;
              if (icon.onClick == null)
                icon.onClick = (Action<ItemIcon>) (x => this.SetIconOnClick(icon.ItemInfo));
            }
            else
            {
              icon.Gray = true;
              icon.onClick = (Action<ItemIcon>) null;
            }
          }
          else
            icon.Gray = this.IsGrayIcon(inventoryItem);
        }
      }
    }
  }

  private void calcReisouExp(
    ref Dictionary<GearReisouType, int> outExp,
    PlayerItem item,
    GearGear gear)
  {
    GearReisouType? reisouType = item?.gear?.reisou_type;
    int? drillingExpMythologyId1 = (int?) gear?.drilling_exp_mythology_id;
    if (!reisouType.HasValue && (!drillingExpMythologyId1.HasValue || drillingExpMythologyId1.Value == 0))
      return;
    if (gear.drilling_exp_mythology_id.HasValue)
    {
      GearDrillingExpMythology drillingExpMythology = ((IEnumerable<GearDrillingExpMythology>) MasterData.GearDrillingExpMythologyList).FirstOrDefault<GearDrillingExpMythology>((Func<GearDrillingExpMythology, bool>) (x =>
      {
        int id = x.ID;
        int? drillingExpMythologyId2 = gear.drilling_exp_mythology_id;
        int valueOrDefault = drillingExpMythologyId2.GetValueOrDefault();
        return id == valueOrDefault & drillingExpMythologyId2.HasValue;
      }));
      int? nullable = drillingExpMythology.holy;
      int total_exp1 = nullable ?? 0;
      nullable = drillingExpMythology.chaos;
      int total_exp2 = nullable ?? 0;
      outExp[GearReisouType.holy] += this.reisouValue(gear, total_exp1);
      outExp[GearReisouType.chaos] += this.reisouValue(gear, total_exp2);
    }
    if (!reisouType.HasValue)
      return;
    switch (reisouType.GetValueOrDefault())
    {
      case GearReisouType.holy:
        outExp[GearReisouType.holy] += this.reisouValue(gear, item.gear_total_exp);
        break;
      case GearReisouType.chaos:
        outExp[GearReisouType.chaos] += this.reisouValue(gear, item.gear_total_exp);
        break;
      case GearReisouType.mythology:
        PlayerMythologyGearStatus mythologyGearStatus = item.GetPlayerMythologyGearStatus();
        outExp[GearReisouType.holy] += this.reisouValue(gear, mythologyGearStatus.holy_gear_total_exp);
        outExp[GearReisouType.chaos] += this.reisouValue(gear, mythologyGearStatus.chaos_gear_total_exp);
        break;
    }
  }

  private bool JudgeMaxUp()
  {
    return this.BaseItem.gearLevelLimit + this.maxUpNum < this.BaseItem.playerItem.gear_level_limit_max;
  }

  private bool JudgeRensei(GameCore.ItemInfo info)
  {
    if (info.playerItem == (PlayerItem) null && (!info.gear.drilling_exp_mythology_id.HasValue || info.gear.drilling_exp_mythology_id.Value == 0))
    {
      if (this.addRenseiValue >= this.residualRenseiExp)
        return false;
    }
    else if (info.playerItem != (PlayerItem) null && !info.playerItem.isReisou())
    {
      if (this.addRenseiValue >= this.residualRenseiExp)
        return false;
    }
    else
    {
      int? drillingExpMythologyId;
      if (info.gear.drilling_exp_mythology_id.HasValue)
      {
        drillingExpMythologyId = info.gear.drilling_exp_mythology_id;
        int num = 1;
        if (drillingExpMythologyId.GetValueOrDefault() == num & drillingExpMythologyId.HasValue)
          return this.addHolyValue < this.residualSeihouExp;
      }
      if (info.gear.drilling_exp_mythology_id.HasValue)
      {
        drillingExpMythologyId = info.gear.drilling_exp_mythology_id;
        int num = 2;
        if (drillingExpMythologyId.GetValueOrDefault() == num & drillingExpMythologyId.HasValue)
          return this.addChaosValue < this.residualKontonExp;
      }
      if (info.playerItem.isMythologyReisou() && (this.addHolyValue >= this.residualSeihouExp || this.addChaosValue >= this.residualKontonExp) || info.playerItem.isHolyReisou() && this.addHolyValue >= this.residualSeihouExp || info.playerItem.isChaosReisou() && this.addChaosValue >= this.residualKontonExp)
        return false;
    }
    return true;
  }

  protected override bool IsGrayIcon(InventoryItem item)
  {
    bool flag = base.IsGrayIcon(item);
    if (Object.op_Inequality((Object) item.icon, (Object) null))
    {
      if (!flag || flag && item.select)
      {
        if (item.icon.onClick == null)
          item.icon.onClick = (Action<ItemIcon>) (x => this.SetIconOnClick(item.Item));
      }
      else if (flag && !item.select)
        item.icon.onClick = (Action<ItemIcon>) null;
    }
    return flag;
  }

  private void SetIconOnClick(GameCore.ItemInfo info)
  {
    if (info.broken)
      return;
    if (!this.IsMaterialPopup(info))
      this.SelectItemProc(info);
    else
      this.StartCoroutine(this.ShowMaterialsPopup(this.InventoryItems.FindByItemIndex(info)));
  }

  private IEnumerator ShowMaterialsPopup(InventoryItem item)
  {
    ReisouRenseiSelectMenu renseiSelectMenu = this;
    Future<GameObject> f = new ResourceObject("Prefabs/Popup_Common/popup_BundleIntegration_Unit_Base").Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    renseiSelectMenu.materialPopup = Singleton<PopupManager>.GetInstance().open(f.Result).GetComponent<BuguMaterialsPopup>();
    int maxUpNum = renseiSelectMenu.maxUpNum;
    if (renseiSelectMenu.IsMaxUp(item.Item) && item.Item.isTempSelectedCount)
      maxUpNum -= item.Item.tempSelectedCount;
    int addRenseiValue = renseiSelectMenu.addRenseiValue;
    if (item.Item.gear.drilling_exp_mythology_id.HasValue)
    {
      int? drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
      int num1 = 0;
      if (!(drillingExpMythologyId.GetValueOrDefault() == num1 & drillingExpMythologyId.HasValue))
      {
        drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
        int num2 = 1;
        if (drillingExpMythologyId.GetValueOrDefault() == num2 & drillingExpMythologyId.HasValue)
          addRenseiValue = renseiSelectMenu.addHolyValue;
        drillingExpMythologyId = item.Item.gear.drilling_exp_mythology_id;
        int num3 = 2;
        if (drillingExpMythologyId.GetValueOrDefault() == num3 & drillingExpMythologyId.HasValue)
          addRenseiValue = renseiSelectMenu.addChaosValue;
      }
    }
    e = renseiSelectMenu.materialPopup.Init(item, renseiSelectMenu.BaseItem, addRenseiValue, maxUpNum);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    renseiSelectMenu.materialPopup.SetOnOKClick(new Action<InventoryItem>(renseiSelectMenu.\u003CShowMaterialsPopup\u003Eb__51_0));
  }

  private bool IsMaterialPopup(GameCore.ItemInfo info)
  {
    return info.isDrilling || info.isWeaponMaterial || info.gear.kind.Enum == GearKindEnum.sea_present;
  }

  private bool IsMaxUp(GameCore.ItemInfo info)
  {
    return this.BaseItem != null && !this.BaseItem.playerItem.isLimitMax() && info.gear != null && GearGear.CanSpecialDrill(this.BaseItem.gear, info.gear);
  }

  public override void IbtnBack()
  {
    if (this.IsPush)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    if (Singleton<NGGameDataManager>.GetInstance().IsColosseum)
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    List<InventoryItem> list = this.FirstSelectItemList.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
    {
      InventoryItem inventoryItem = this.InventoryItems.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (i => i.Item.itemID == x.Item.itemID));
      return inventoryItem != null && !inventoryItem.Item.favorite && !inventoryItem.Item.ForBattle && !inventoryItem.Item.broken;
    })).ToList<InventoryItem>();
    this.ClearData();
    this.BaseItem = new GameCore.ItemInfo(((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == this.BaseItem.itemID)));
    ReisouRenseiScene.changeScene(false, this.BaseItem.playerItem, list);
  }

  public void IbtnDecision()
  {
    if (this.IsPush)
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    ReisouRenseiScene.changeScene(false, this.BaseItem.playerItem, this.SelectItemList);
  }

  public void ClearData()
  {
    for (int index = 0; index < this.SelectItemList.Count; ++index)
    {
      this.SelectItemList[index].Item.isTempSelectedCount = false;
      this.SelectItemList[index].Item.tempSelectedCount = 0;
    }
  }

  public override void IbtnClear()
  {
    base.IbtnClear();
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
      inventoryItem.Item.tempSelectedCount = 0;
      inventoryItem.Item.isTempSelectedCount = false;
    }
    this.SelectItemList.Clear();
    this.FirstSelectItemList.Clear();
  }

  private void AutoRensei()
  {
    this.IbtnClear();
    int tempRankUp = 0;
    if (this.BaseItem.gearLevelLimit < this.BaseItem.playerItem.gear_level_limit_max)
    {
      List<InventoryItem> list = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => this.DisplayItems.Contains(x))).ToList<InventoryItem>();
      for (int index = 0; index < list.Count; ++index)
      {
        int num = Mathf.Max(0, this.BaseItem.playerItem.gear_level_limit_max - this.BaseItem.gearLevelLimit - tempRankUp);
        if (this.IsMaxUp(list[index].Item) && list[index].Item.gearLevel == 1 && tempRankUp < this.BaseItem.playerItem.gear_level_limit_max - this.BaseItem.gearLevelLimit && !list[index].Item.favorite && !list[index].Item.ForBattle && !list[index].Item.isEquipReisou_ && list[index].Item.gear.kind.Enum != GearKindEnum.special_drilling && !list[index].Item.broken)
        {
          if (list[index].Item.isWeaponMaterial)
          {
            if (list[index].Item.quantity < num)
              num = list[index].Item.quantity;
            list[index].Item.isTempSelectedCount = num > 0;
            list[index].Item.tempSelectedCount = num;
            this.AddSelectItem(list[index]);
            tempRankUp += num;
          }
          else
          {
            this.AddSelectItem(list[index]);
            tempRankUp++;
          }
        }
      }
    }
    int num1 = 0;
    int num2 = 99;
    if (this.BaseItem.gearLevel < this.BaseItem.gearLevelLimit)
    {
      List<InventoryItem> list = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
      {
        if (!this.DisplayItems.Contains(x) || x.Item.gear.kind.Enum != GearKindEnum.drilling)
          return false;
        return !x.Item.gear.drilling_exp_mythology_id.HasValue || x.Item.gear.drilling_exp_mythology_id.Value == 0;
      })).OrderBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.gear.rarity.index)).ToList<InventoryItem>();
      for (int index = 0; index < list.Count; ++index)
      {
        if (this.SelectItemList.Count > 99)
          return;
        int num3 = Math.Max(0, ((IEnumerable<GearRankExp>) MasterData.GearRankExpList).FirstOrDefault<GearRankExp>((Func<GearRankExp, bool>) (x => x.ID == Mathf.Min(this.BaseItem.gearLevelLimit + tempRankUp, this.BaseItem.playerItem.gear_level_limit_max))).from_exp - this.BaseItem.playerItem.gear_total_exp - num1);
        if (num3 > 0)
        {
          BoostBonusGearDrilling bonusGearDrilling = (!list[index].Item.isReisou ? Singleton<NGGameDataManager>.GetInstance().BoostInfo : (NGGameDataManager.Boost) null)?.findBonusGearDrilling(this.BaseItem.gear);
          if (bonusGearDrilling != null)
          {
            Decimal increasePrice = (Decimal) bonusGearDrilling.increase_price;
          }
          int num4 = (int) ((bonusGearDrilling == null ? 1.0M : (Decimal) bonusGearDrilling.boot_rate) * (list[index].Item.isReisou ? 0M : Math.Floor((Decimal) GearDrilling.GetGearDrilling(list[index].Item.gearLevel, list[index].Item.gear.rarity.index) * (Decimal) list[index].Item.gear.drilling_rate)));
          int num5 = (int) Math.Ceiling((double) num3 / (double) num4);
          if (num5 > num2)
            num5 = num2;
          if (list[index].Item.quantity < num5)
            num5 = list[index].Item.quantity;
          list[index].Item.isTempSelectedCount = num5 > 0;
          list[index].Item.tempSelectedCount = num5;
          this.AddSelectItem(list[index]);
          num1 += num5 * num4;
        }
        else
          break;
      }
    }
    if (this.BaseItem.playerItem != (PlayerItem) null && !this.BaseItem.isEquipReisouLvMax)
    {
      Dictionary<GearReisouType, int> outExp = new Dictionary<GearReisouType, int>()
      {
        {
          GearReisouType.holy,
          0
        },
        {
          GearReisouType.chaos,
          0
        }
      };
      if (this.BaseItem.playerItem.isMythologyReisou())
      {
        List<InventoryItem> list = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => this.DisplayItems.Contains(x) && x.Item.gear.isHolyReisou() && !this.equipedReisouIdList.Contains(x.Item.itemID) && ((IEnumerable<int>) this.reisouRenseiMasterID).Contains<int>(x.Item.masterID) && x.Item.gearLevel == 1 && x.Item.gear.rarity.index == 1)).OrderBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.masterID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>().Concat<InventoryItem>((IEnumerable<InventoryItem>) this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => this.DisplayItems.Contains(x) && x.Item.gear.isChaosReisou() && !this.equipedReisouIdList.Contains(x.Item.itemID) && ((IEnumerable<int>) this.reisouRenseiMasterID).Contains<int>(x.Item.masterID) && x.Item.gearLevel == 1 && x.Item.gear.rarity.index == 1)).OrderBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.masterID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID))).ToList<InventoryItem>();
        PlayerMythologyGearStatus mythologyGearStatus = this.BaseItem.playerItem.GetPlayerMythologyGearStatus();
        int fromExp1 = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>((Func<ReisouRankExp, bool>) (x => x.ID == this.BaseItem.playerItem.gear_level_limit)).from_exp;
        int fromExp2 = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>((Func<ReisouRankExp, bool>) (x => x.ID == this.BaseItem.playerItem.gear_level_limit)).from_exp;
        int num6 = fromExp1 - mythologyGearStatus.holy_gear_total_exp;
        int chaosGearTotalExp = mythologyGearStatus.chaos_gear_total_exp;
        int num7 = fromExp2 - chaosGearTotalExp;
        for (int index = 0; index < list.Count && this.SelectItemList.Count < 99; ++index)
        {
          if (list[index].Item.gear.isHolyReisou() && outExp[GearReisouType.holy] < num6)
          {
            this.calcReisouExp(ref outExp, list[index].Item.playerItem, list[index].Item.gear);
            list[index].Item.isTempSelectedCount = true;
            list[index].Item.tempSelectedCount = 1;
            this.AddSelectItem(list[index]);
          }
          if (list[index].Item.gear.isChaosReisou() && outExp[GearReisouType.chaos] < num7)
          {
            this.calcReisouExp(ref outExp, list[index].Item.playerItem, list[index].Item.gear);
            list[index].Item.isTempSelectedCount = true;
            list[index].Item.tempSelectedCount = 1;
            this.AddSelectItem(list[index]);
          }
        }
      }
      else if (this.BaseItem.playerItem.isHolyReisou())
      {
        List<InventoryItem> list = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => this.DisplayItems.Contains(x) && x.Item.gear.isHolyReisou() && !this.equipedReisouIdList.Contains(x.Item.itemID) && ((IEnumerable<int>) this.reisouRenseiMasterID).Contains<int>(x.Item.masterID) && x.Item.gearLevel == 1 && x.Item.gear.rarity.index == 1)).OrderBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.masterID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>();
        int num8 = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>((Func<ReisouRankExp, bool>) (x => x.ID == this.BaseItem.playerItem.gear_level_limit)).from_exp - this.BaseItem.playerItem.gear_total_exp;
        for (int index = 0; index < list.Count && this.SelectItemList.Count < 99; ++index)
        {
          if (outExp[GearReisouType.holy] < num8)
          {
            this.calcReisouExp(ref outExp, list[index].Item.playerItem, list[index].Item.gear);
            list[index].Item.isTempSelectedCount = true;
            list[index].Item.tempSelectedCount = 1;
            this.AddSelectItem(list[index]);
          }
        }
      }
      else if (this.BaseItem.playerItem.isChaosReisou())
      {
        List<InventoryItem> list = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x => this.DisplayItems.Contains(x) && x.Item.gear.isChaosReisou() && !this.equipedReisouIdList.Contains(x.Item.itemID) && ((IEnumerable<int>) this.reisouRenseiMasterID).Contains<int>(x.Item.masterID) && x.Item.gearLevel == 1 && x.Item.gear.rarity.index == 1)).OrderBy<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.masterID)).ThenByDescending<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToList<InventoryItem>();
        int num9 = ((IEnumerable<ReisouRankExp>) MasterData.ReisouRankExpList).FirstOrDefault<ReisouRankExp>((Func<ReisouRankExp, bool>) (x => x.ID == this.BaseItem.playerItem.gear_level_limit)).from_exp - this.BaseItem.playerItem.gear_total_exp;
        for (int index = 0; index < list.Count && this.SelectItemList.Count < 99; ++index)
        {
          if (outExp[GearReisouType.chaos] < num9)
          {
            this.calcReisouExp(ref outExp, list[index].Item.playerItem, list[index].Item.gear);
            list[index].Item.isTempSelectedCount = true;
            list[index].Item.tempSelectedCount = 1;
            this.AddSelectItem(list[index]);
          }
        }
      }
    }
    if (this.SelectItemList.Count <= 0)
      ModalWindow.Show(Consts.GetInstance().AUTO_RENSEI_TITLE, Consts.GetInstance().AUTO_RENSEI_TITLE_TEXT, (Action) (() => this.IsPush = false));
    this.UpdateSelectItemIndexWithInfo();
  }

  public enum DrillingType
  {
    Normal,
    Special,
  }
}
