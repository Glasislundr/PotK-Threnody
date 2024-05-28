// Decompiled with JetBrains decompiler
// Type: Unit0044Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using EquipmentRules;
using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit0044Menu : Bugu005SelectItemListMenuBase
{
  [SerializeField]
  protected UILabel TxtNumberpossession;
  private GameObject StatusChangePopupPrefab;
  private GameObject EquipAlertPopupPrefab;
  private bool isOpened;
  private int changeGearIndex;
  private PlayerUnit basePlayerUnit;
  private List<int> baseEquippedGearIds;
  private HashSet<int> disabledTouchEquippedGearIds_;
  private PlayerUnitGearProficiency basePrimaryGearProficiency;
  private (PlayerItem src, PlayerItem[] dests, int[] destIndexes)? swapGears_;
  private bool isEarthMode;
  private PlayerItem[] currentEquippedGear_;

  public int ChangeGearIndex
  {
    set => this.changeGearIndex = value;
    get => this.changeGearIndex;
  }

  public EditGearParam EditParam { get; set; }

  public GearGear SpecialGear { get; set; }

  public PlayerUnit BasePlayerUnit
  {
    set
    {
      this.swapGears_ = new (PlayerItem, PlayerItem[], int[])?();
      this.disabledTouchEquippedGearIds_ = (HashSet<int>) null;
      this.currentEquippedGear_ = (PlayerItem[]) null;
      this.basePlayerUnit = value;
      if (this.basePlayerUnit == (PlayerUnit) null)
      {
        this.baseEquippedGearIds = (List<int>) null;
        this.basePrimaryGearProficiency = (PlayerUnitGearProficiency) null;
      }
      else
      {
        this.baseEquippedGearIds = ((IEnumerable<int?>) new int?[3]
        {
          this.basePlayerUnit.equippedGear?.id,
          this.basePlayerUnit.equippedGear2?.id,
          this.basePlayerUnit.equippedGear3?.id
        }).Where<int?>((Func<int?, bool>) (x => x.HasValue)).Select<int?, int>((Func<int?, int>) (y => y.Value)).ToList<int>();
        int primaryGearKind = (int) this.basePlayerUnit.unit.primaryGearKind;
        this.basePrimaryGearProficiency = Array.Find<PlayerUnitGearProficiency>(this.basePlayerUnit.gear_proficiencies, (Predicate<PlayerUnitGearProficiency>) (x => x.gear_kind_id == primaryGearKind));
      }
    }
    get => this.basePlayerUnit;
  }

  private (PlayerItem src, PlayerItem[] dests, int[] destIndexes) swapGears
  {
    get
    {
      if (this.swapGears_.HasValue)
        return this.swapGears_.Value;
      PlayerItem[] equippedGears = new PlayerItem[3]
      {
        this.basePlayerUnit.equippedGear,
        this.basePlayerUnit.equippedGear2,
        this.basePlayerUnit.equippedGear3
      };
      if (this.EditParam == null)
      {
        PlayerItem[] targetGears = this.targetGears;
        for (int index = 0; index < equippedGears.Length; ++index)
        {
          PlayerItem playerItem = equippedGears[index];
          int tmpId = (object) playerItem != null ? playerItem.id : 0;
          if (tmpId != 0)
            equippedGears[index] = Array.Find<PlayerItem>(targetGears, (Predicate<PlayerItem>) (x => x.id == tmpId));
        }
      }
      PlayerItem playerItem1 = equippedGears[1];
      bool flag = (object) playerItem1 != null && playerItem1.gear.is_jingi;
      int[] source;
      switch (this.changeGearIndex)
      {
        case 1:
          int[] numArray1;
          if (flag)
            numArray1 = (int[]) null;
          else
            numArray1 = new int[1]{ 2 };
          source = numArray1;
          break;
        case 2:
          source = new int[1]{ 2 };
          break;
        case 3:
          int[] numArray2;
          if (flag)
            numArray2 = new int[1]{ 1 };
          else
            numArray2 = new int[2]{ 0, 1 };
          source = numArray2;
          break;
        default:
          this.swapGears_ = new (PlayerItem, PlayerItem[], int[])?(((PlayerItem) null, new PlayerItem[0], (int[]) null));
          Debug.LogError((object) string.Format("changeGearIndex=\"{0}\" Fail!! (True= \"1-3\")", (object) this.changeGearIndex));
          return this.swapGears_.Value;
      }
      PlayerItem playerItem2 = equippedGears[this.changeGearIndex - 1];
      if (source == null)
      {
        this.swapGears_ = new (PlayerItem, PlayerItem[], int[])?((playerItem2, new PlayerItem[0], (int[]) null));
        return this.swapGears_.Value;
      }
      PlayerItem[] array1 = ((IEnumerable<int>) source).Select<int, PlayerItem>((Func<int, PlayerItem>) (i => equippedGears[i])).ToArray<PlayerItem>();
      if (this.SpecialGear != null && (equippedGears[0] != (PlayerItem) null || equippedGears[2] != (PlayerItem) null) && this.SpecialGear != equippedGears[2]?.gear)
      {
        source[0] = -1;
        array1[0] = (PlayerItem) null;
      }
      PlayerItem[] array2 = ((IEnumerable<PlayerItem>) equippedGears).ToArray<PlayerItem>();
      array2[this.changeGearIndex - 1] = (PlayerItem) null;
      if (playerItem2 != (PlayerItem) null)
      {
        for (int index = 0; index < source.Length; ++index)
        {
          int slotIndex = source[index];
          if (slotIndex >= 0)
          {
            array2[slotIndex] = (PlayerItem) null;
            if (!Gears.createFuncPossibleEquipped(this.basePlayerUnit, slotIndex, array2)(playerItem2))
            {
              source[index] = -1;
              array1[index] = (PlayerItem) null;
            }
            array2[slotIndex] = equippedGears[slotIndex];
          }
        }
      }
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        if (!(array1[index1] == (PlayerItem) null))
        {
          int index2 = source[index1];
          array2[index2] = (PlayerItem) null;
          if (!Gears.createFuncPossibleEquipped(this.basePlayerUnit, this.changeGearIndex - 1, array2)(array1[index1]))
          {
            source[index1] = -1;
            array1[index1] = (PlayerItem) null;
          }
          array2[index2] = equippedGears[index2];
        }
      }
      List<int> intList = new List<int>(source.Length);
      List<PlayerItem> playerItemList = new List<PlayerItem>(source.Length);
      for (int index = 0; index < source.Length; ++index)
      {
        if (source[index] >= 0)
        {
          intList.Add(source[index]);
          playerItemList.Add(array1[index]);
        }
      }
      PlayerItem[] array3 = playerItemList.ToArray();
      int[] array4 = array3.Length != 0 ? intList.ToArray() : (int[]) null;
      this.swapGears_ = new (PlayerItem, PlayerItem[], int[])?((playerItem2, array3, array4));
      return this.swapGears_.Value;
    }
  }

  public bool IsEarthMode
  {
    set => this.isEarthMode = value;
    get => this.isEarthMode;
  }

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    return Persist.unit0044SortAndFilter;
  }

  private PlayerItem[] targetGears => this.EditParam?.gears ?? SMManager.Get<PlayerItem[]>();

  protected override List<PlayerItem> GetItemList()
  {
    PlayerItem[] playerItemArray = this.targetGears;
    if (this.SpecialGear != null)
    {
      List<int> includeGears = ((IEnumerable<PlayerItem>) this.swapGears.dests).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x != (PlayerItem) null)).Select<PlayerItem, int>((Func<PlayerItem, int>) (y => y.id)).ToList<int>();
      playerItemArray = this.sortSpecialGears(((IEnumerable<PlayerItem>) playerItemArray).Where<PlayerItem>((Func<PlayerItem, bool>) (x => includeGears.Contains(x.id) || x.gear == this.SpecialGear))).ToArray<PlayerItem>();
    }
    (PlayerItem src, PlayerItem[] dests, int[] destIndexes) swapGears = this.swapGears;
    return swapGears.destIndexes != null ? Gears.makeList(this.basePlayerUnit, playerItemArray, this.changeGearIndex - 1, ((IEnumerable<PlayerItem>) swapGears.dests).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x != (PlayerItem) null)).Select<PlayerItem, int>((Func<PlayerItem, int>) (y => y.id)).ToList<int>()) : Gears.makeList(this.basePlayerUnit, playerItemArray, this.changeGearIndex - 1);
  }

  private IEnumerable<PlayerItem> sortSpecialGears(IEnumerable<PlayerItem> playerGears)
  {
    PlayerItem[] array = playerGears.ToArray<PlayerItem>();
    if (array.Length <= 1)
      return (IEnumerable<PlayerItem>) array;
    HashSet<int> intSet = new HashSet<int>(((IEnumerable<PlayerItem>) array).Select<PlayerItem, int>((Func<PlayerItem, int>) (x => x.id)));
    List<int> equippedGear = new List<int>(array.Length);
    foreach (PlayerUnit playerUnit in SMManager.Get<PlayerUnit[]>())
    {
      if (playerUnit.equip_gear_ids != null && playerUnit.equip_gear_ids.Length != 0)
      {
        foreach (int? equipGearId in playerUnit.equip_gear_ids)
        {
          if (equipGearId.HasValue && intSet.Contains(equipGearId.Value))
            equippedGear.Add(equipGearId.Value);
        }
      }
    }
    int equippedNum = this.baseEquippedGearIds.Count;
    return (IEnumerable<PlayerItem>) playerGears.OrderBy<PlayerItem, int>((Func<PlayerItem, int>) (y =>
    {
      int? nullable = this.baseEquippedGearIds.FirstIndexOrNull<int>((Func<int, bool>) (i => i == y.id));
      return !nullable.HasValue ? equippedNum : nullable.Value;
    })).ThenBy<PlayerItem, bool>((Func<PlayerItem, bool>) (z => equippedGear.Contains(z.id))).ThenByDescending<PlayerItem, int>((Func<PlayerItem, int>) (x => x.gear_level)).ThenBy<PlayerItem, int>((Func<PlayerItem, int>) (a => a.id));
  }

  protected override IEnumerator InitExtension()
  {
    Unit0044Menu unit0044Menu = this;
    if (unit0044Menu.SpecialGear == null && unit0044Menu.currentEquippedGear != (PlayerItem) null)
    {
      InventoryItem inventoryItem = new InventoryItem();
      unit0044Menu.InventoryItems.Insert(0, inventoryItem);
    }
    Future<GameObject> popupPrefabF;
    IEnumerator e;
    if (Object.op_Equality((Object) unit0044Menu.StatusChangePopupPrefab, (Object) null))
    {
      popupPrefabF = Res.Prefabs.popup.popup_004_4_1__anim_popup01.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit0044Menu.StatusChangePopupPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) unit0044Menu.EquipAlertPopupPrefab, (Object) null))
    {
      popupPrefabF = Res.Prefabs.popup.popup_004_12_4__anim_popup01.Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unit0044Menu.EquipAlertPopupPrefab = popupPrefabF.Result;
      popupPrefabF = (Future<GameObject>) null;
    }
  }

  private PlayerItem currentEquippedGear
  {
    get
    {
      if (this.currentEquippedGear_ != null)
        return this.currentEquippedGear_[0];
      PlayerItem playerGear;
      switch (this.changeGearIndex)
      {
        case 2:
          playerGear = this.basePlayerUnit.equippedGear2;
          break;
        case 3:
          playerGear = this.basePlayerUnit.equippedGear3;
          break;
        default:
          playerGear = this.basePlayerUnit.equippedGear;
          break;
      }
      if (this.EditParam == null)
        this.currentEquippedGear_ = new PlayerItem[1]
        {
          playerGear != (PlayerItem) null ? Array.Find<PlayerItem>(this.targetGears, (Predicate<PlayerItem>) (x => x.id == playerGear.id)) : (PlayerItem) null
        };
      else
        this.currentEquippedGear_ = new PlayerItem[1]
        {
          playerGear
        };
      return this.currentEquippedGear_[0];
    }
  }

  protected override void ChangeDetailScene(GameCore.ItemInfo item)
  {
    if (this.EditParam == null)
    {
      Unit00443Scene.changeSceneLimited(true, item, this.DisplayItems);
    }
    else
    {
      int reisouId = this.EditParam.deck.getEquippedReisouId(Array.Find<PlayerItem>(this.EditParam.gears, (Predicate<PlayerItem>) (x => x.id == item.itemID)).id);
      PlayerItem reisou = reisouId != 0 ? Array.Find<PlayerItem>(this.EditParam.gears, (Predicate<PlayerItem>) (x => x.id == reisouId)) : (PlayerItem) null;
      PlayerUnit playerUnit;
      this.EditParam.dicReference.TryGetValue(item.itemID, out playerUnit);
      Unit00443Scene.changeSceneCustomDeck(item.playerItem, playerUnit, reisou);
    }
  }

  protected override void BottomInfoUpdate()
  {
    int num = 0;
    foreach (PlayerItem targetGear in this.targetGears)
    {
      if (!targetGear.isSupply() && !targetGear.isReisou())
        ++num;
    }
    this.TxtNumberpossession.SetTextLocalize(string.Format("{0}/{1}", (object) num, (object) SMManager.Get<Player>().max_items));
  }

  protected override void SelectItemProc(GameCore.ItemInfo item)
  {
    if (this.InventoryItems.FindByItem(item).removeButton)
    {
      this.StartCoroutine(this.RemoveGearAsync());
    }
    else
    {
      this.ChangeGear(this.basePlayerUnit, this.currentEquippedGear, Array.Find<PlayerItem>(this.targetGears, (Predicate<PlayerItem>) (x => x.id == item.itemID)));
      this.UpdateSelectItemIndexWithInfo();
    }
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
      itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.StartCoroutine(this.RemoveGearAsync()));
      itemIcon.DisableLongPressEvent();
    }
    else
    {
      itemIcon.ForBattle = displayItem.Item.ForBattle;
      itemIcon.Favorite = displayItem.Item.favorite;
      itemIcon.onClick = (Action<ItemIcon>) (playeritem => this.SelectItemProc(playeritem.ItemInfo));
      if (this.IsGrayIcon(displayItem))
      {
        itemIcon.Gray = true;
        displayItem.Gray = true;
        itemIcon.onClick = (Action<ItemIcon>) (_ => { });
      }
      itemIcon.EnableLongPressEvent(new Action<GameCore.ItemInfo>(((Bugu005ItemListMenuBase) this).ChangeDetailScene));
    }
  }

  private HashSet<int> disabledTouchEquippedGearIds
  {
    get
    {
      if (this.disabledTouchEquippedGearIds_ != null)
        return this.disabledTouchEquippedGearIds_;
      (PlayerItem src, PlayerItem[] dests, int[] destIndexes) swaps = this.swapGears;
      this.disabledTouchEquippedGearIds_ = new HashSet<int>(swaps.destIndexes != null ? this.baseEquippedGearIds.Where<int>((Func<int, bool>) (i => !((IEnumerable<PlayerItem>) swaps.dests).Any<PlayerItem>((Func<PlayerItem, bool>) (x => x != (PlayerItem) null && x.id == i)))) : (IEnumerable<int>) this.baseEquippedGearIds);
      return this.disabledTouchEquippedGearIds_;
    }
  }

  protected override bool DisableTouchIcon(InventoryItem item)
  {
    if (item.Item == null)
      return true;
    GearGear gear = item.Item.gear;
    return !gear.checkCanEquipByProficiency(this.basePlayerUnit, gear.is_jingi ? this.basePrimaryGearProficiency : (PlayerUnitGearProficiency) null) || this.disabledTouchEquippedGearIds.Contains(item.Item.itemID);
  }

  private IEnumerator StatusPopup(PlayerUnit baseUnit, PlayerItem beforeGear, PlayerItem afterGear)
  {
    Future<GameObject> iconPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = iconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject iconPrefab = iconPrefabF.Result;
    bool bCustomSwap = this.EditParam != null && this.swapGears.destIndexes != null && ((IEnumerable<PlayerItem>) this.swapGears.dests).Any<PlayerItem>((Func<PlayerItem, bool>) (x =>
    {
      int id1 = afterGear.id;
      int? id2 = x?.id;
      int valueOrDefault = id2.GetValueOrDefault();
      return id1 == valueOrDefault & id2.HasValue;
    }));
    PlayerItem[] customReisous = this.EditParam != null ? ((IEnumerable<int>) this.EditParam.reisouIds).Select<int, PlayerItem>((Func<int, PlayerItem>) (i => !i.IsValid() ? (PlayerItem) null : Array.Find<PlayerItem>(this.EditParam.gears, (Predicate<PlayerItem>) (x => x.id == i)))).ToArray<PlayerItem>() : (PlayerItem[]) null;
    GameObject beforeGearIcon = Object.Instantiate<GameObject>(iconPrefab);
    ItemIcon beforeGearIconScript = beforeGearIcon.GetComponent<ItemIcon>();
    if (beforeGear != (PlayerItem) null)
    {
      e = beforeGearIconScript.InitByPlayerItem(beforeGear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = beforeGearIconScript.InitByGear((GearGear) null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      beforeGearIconScript.SetEmpty(true);
    }
    beforeGearIcon.SetActive(false);
    GameObject afterGearIcon = Object.Instantiate<GameObject>(iconPrefab);
    ItemIcon afterGearIconScript = afterGearIcon.GetComponent<ItemIcon>();
    if (afterGear != (PlayerItem) null)
    {
      if (this.EditParam != null && !bCustomSwap)
      {
        e = afterGearIconScript.InitByPlayerItem(afterGear, Reisou.checkPossibleEquipped(afterGear, customReisous[this.changeGearIndex - 1]));
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = afterGearIconScript.InitByPlayerItem(afterGear);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
    else
    {
      e = afterGearIconScript.InitByGear((GearGear) null);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      afterGearIconScript.SetEmpty(true);
    }
    afterGearIcon.SetActive(false);
    Future<GameObject> skillTypeIconLoader = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = skillTypeIconLoader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject skillTypeIconPrefab = skillTypeIconLoader.Result;
    List<GameObject> beforeSkillTypeIcons = new List<GameObject>();
    GearGearSkill[] gearGearSkillArray;
    int index;
    GameObject beforeSkillTypeIcon;
    if (beforeGear != (PlayerItem) null)
    {
      gearGearSkillArray = beforeGear.skills;
      for (index = 0; index < gearGearSkillArray.Length; ++index)
      {
        GearGearSkill gearGearSkill = gearGearSkillArray[index];
        beforeSkillTypeIcon = Object.Instantiate<GameObject>(skillTypeIconPrefab);
        e = beforeSkillTypeIcon.GetComponent<BattleSkillIcon>().Init(gearGearSkill.skill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        beforeSkillTypeIcon.SetActive(false);
        beforeSkillTypeIcons.Add(beforeSkillTypeIcon);
        beforeSkillTypeIcon = (GameObject) null;
      }
      gearGearSkillArray = (GearGearSkill[]) null;
    }
    List<GameObject> afterSkillTypeIcons = new List<GameObject>();
    if (afterGear != (PlayerItem) null)
    {
      gearGearSkillArray = afterGear.skills;
      for (index = 0; index < gearGearSkillArray.Length; ++index)
      {
        GearGearSkill gearGearSkill = gearGearSkillArray[index];
        beforeSkillTypeIcon = Object.Instantiate<GameObject>(skillTypeIconPrefab);
        e = beforeSkillTypeIcon.GetComponent<BattleSkillIcon>().Init(gearGearSkill.skill);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        beforeSkillTypeIcon.SetActive(false);
        afterSkillTypeIcons.Add(beforeSkillTypeIcon);
        beforeSkillTypeIcon = (GameObject) null;
      }
      gearGearSkillArray = (GearGearSkill[]) null;
    }
    GameObject statusPopup = this.StatusChangePopupPrefab.Clone();
    Unit00441Menu component = statusPopup.GetComponent<Unit00441Menu>();
    statusPopup.SetActive(false);
    beforeGearIcon.SetActive(false);
    afterGearIcon.SetActive(false);
    if (this.EditParam != null)
    {
      e = component.SetGear((PlayerUnit) null, baseUnit, beforeGear, afterGear, beforeGearIcon, afterGearIcon, beforeSkillTypeIcons.ToArray(), afterSkillTypeIcons.ToArray(), this.changeGearIndex, this.isEarthMode, (Action) (() => this.changeCustomDeckGear(afterGear.id)), bCustomSwap, customReisous);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = component.SetGear((PlayerUnit) null, baseUnit, beforeGear, afterGear, beforeGearIcon, afterGearIcon, beforeSkillTypeIcons.ToArray(), afterSkillTypeIcons.ToArray(), this.changeGearIndex, this.isEarthMode);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<PopupManager>.GetInstance().open(statusPopup, isCloned: true);
    statusPopup.SetActive(true);
    beforeGearIcon.SetActive(true);
    afterGearIcon.SetActive(true);
    this.isOpened = false;
  }

  private void ChangeGear(PlayerUnit baseUnit, PlayerItem beforeGear, PlayerItem afterGear)
  {
    if (afterGear != (PlayerItem) null)
    {
      if (this.EditParam == null && afterGear.ForBattle)
      {
        Singleton<PopupManager>.GetInstance().open(this.EquipAlertPopupPrefab).GetComponent<Unit004431Popup>().Init(((IEnumerable<PlayerUnit>) SMManager.Get<PlayerUnit[]>()).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => !x.unit.IsMaterialUnit)).ToArray<PlayerUnit>(), baseUnit, afterGear, this.changeGearIndex, this.isEarthMode);
      }
      else
      {
        if (this.isOpened)
          return;
        this.isOpened = true;
        this.StartCoroutine(this.StatusPopup(baseUnit, beforeGear, afterGear));
      }
    }
    else
      this.StartCoroutine(this.StatusPopup(baseUnit, beforeGear, afterGear));
  }

  private IEnumerator RemoveGearAsync()
  {
    Unit0044Menu unit0044Menu = this;
    if (unit0044Menu.EditParam != null)
    {
      unit0044Menu.changeCustomDeckGear(0);
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      IEnumerator e = RequestDispatcher.EquipGear(unit0044Menu.changeGearIndex, new int?(0), unit0044Menu.basePlayerUnit.id, (Action<WebAPI.Response.UserError>) (error =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        if (error == null)
          return;
        WebAPI.DefaultUserErrorCallback(error);
      }), unit0044Menu.isEarthMode);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      switch (GuildUtil.gvgPopupState)
      {
        case GuildUtil.GvGPopupState.None:
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          unit0044Menu.backScene();
          yield break;
        case GuildUtil.GvGPopupState.AtkTeam:
          // ISSUE: reference to a compiler-generated method
          e = GuildUtil.UpdateGuildDeckAttack(PlayerAffiliation.Current.guild_id, Player.Current.id, new Action(unit0044Menu.\u003CRemoveGearAsync\u003Eb__48_1));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        case GuildUtil.GvGPopupState.DefTeam:
          // ISSUE: reference to a compiler-generated method
          e = GuildUtil.UpdateGuildDeckDefanse(PlayerAffiliation.Current.guild_id, Player.Current.id, new Action(unit0044Menu.\u003CRemoveGearAsync\u003Eb__48_2));
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        default:
          unit0044Menu.backScene();
          break;
      }
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  private void changeCustomDeckGear(int nextGearId)
  {
    this.EditParam.onSetGear(this.EditParam.index, this.EditParam.slotNo, nextGearId);
    Singleton<PopupManager>.GetInstance().closeAll();
    this.backScene();
  }

  protected override InventoryItem CreateInventoryItem(PlayerItem target)
  {
    return this.EditParam == null ? new InventoryItem(target, this.enabledExpireDate) : new InventoryItem(target, this.enabledExpireDate, (Func<bool>) (() => this.EditParam.dicReference.ContainsKey(target.id)));
  }

  protected override void UpdateInvetoryItem(InventoryItem invItem, PlayerItem item)
  {
    if (this.EditParam == null)
      invItem.Item.Set(item, this.enabledExpireDate);
    else
      invItem.Item.Set(item, this.enabledExpireDate, (Func<bool>) (() => this.EditParam.dicReference.ContainsKey(item.id)));
  }

  public override void Sort(
    ItemSortAndFilter.SORT_TYPES type,
    SortAndFilter.SORT_TYPE_ORDER_BUY order,
    bool isEquipFirst)
  {
    this.CurrentSortType = type;
    if (!this.isDisabledSort && Object.op_Inequality((Object) this.SortSprite, (Object) null))
      this.SortSprite = ItemSortAndFilter.SortSpriteLabel(type, this.SortSprite);
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    (PlayerItem src, PlayerItem[] dests, int[] destIndexes) swaps = this.swapGears;
    if (swaps.src != (PlayerItem) null || swaps.destIndexes != null)
    {
      InventoryItem inventoryItem1 = this.InventoryItems.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (x => x.removeButton));
      if (inventoryItem1 != null)
      {
        inventoryItemList.Add(inventoryItem1);
        this.InventoryItems.Remove(inventoryItem1);
      }
      if (swaps.src != (PlayerItem) null)
      {
        InventoryItem inventoryItem2 = this.InventoryItems.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (x =>
        {
          int? itemId = x.Item?.itemID;
          int id = swaps.src.id;
          return itemId.GetValueOrDefault() == id & itemId.HasValue;
        }));
        if (inventoryItem2 != null)
        {
          inventoryItemList.Add(inventoryItem2);
          this.InventoryItems.Remove(inventoryItem2);
        }
      }
      for (int n = 0; n < swaps.Item2.Length; ++n)
      {
        if (!(swaps.Item2[n] == (PlayerItem) null))
        {
          InventoryItem inventoryItem3 = this.InventoryItems.FirstOrDefault<InventoryItem>((Func<InventoryItem, bool>) (x =>
          {
            int? itemId = x.Item?.itemID;
            int id = swaps.Item2[n].id;
            return itemId.GetValueOrDefault() == id & itemId.HasValue;
          }));
          if (inventoryItem3 != null)
          {
            inventoryItemList.Add(inventoryItem3);
            this.InventoryItems.Remove(inventoryItem3);
          }
        }
      }
    }
    this.DisplayItems = this.isDisabledSort ? this.InventoryItems.ToList<InventoryItem>() : this.InventoryItems.FilterBy(this.filter).SortBy(type, order, isEquipFirst).ToList<InventoryItem>();
    if (inventoryItemList.Any<InventoryItem>())
    {
      this.InventoryItems = inventoryItemList.Concat<InventoryItem>((IEnumerable<InventoryItem>) this.InventoryItems).ToList<InventoryItem>();
      this.DisplayItems = inventoryItemList.Concat<InventoryItem>((IEnumerable<InventoryItem>) this.DisplayItems).ToList<InventoryItem>();
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
}
