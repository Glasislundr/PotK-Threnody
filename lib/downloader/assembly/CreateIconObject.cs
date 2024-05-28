// Decompiled with JetBrains decompiler
// Type: CreateIconObject
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
public class CreateIconObject : MonoBehaviour
{
  public const int PAID_KISEKI_ID = -999;
  private bool isSea_;
  private UniqueIcons uniqueIcon;
  private GameObject objIcon;
  private GameObject detailPopup;
  private MasterDataTable.CommonRewardType rewardType;
  private int rewardID;
  private int quantity;
  private bool isDetail;
  private int[] cantDetailItems = new int[14]
  {
    6,
    7,
    8,
    9,
    11,
    12,
    13,
    16,
    18,
    22,
    27,
    30,
    37,
    99
  };
  private bool isReisou;

  public GameObject GetIcon() => this.objIcon;

  private static ResourceObject normal_sea => new ResourceObject("Prefabs/Sea/UnitIcon/normal_sea");

  private static ResourceObject prefab_sea => new ResourceObject("Prefabs/Sea/ItemIcon/prefab_sea");

  private static ResourceObject UniqueIconPrefab_sea
  {
    get => new ResourceObject("Icons/UniqueIconPrefab_sea");
  }

  public IEnumerator CreateThumbnail(
    MasterDataTable.CommonRewardType rewardType,
    int rewardID,
    int quantity = 0,
    bool visibleBottom = true,
    bool isButton = true,
    CommonQuestType? questType = null,
    bool isRouletteWheelIcon = false)
  {
    CreateIconObject createIconObject = this;
    bool isQuantity = quantity != 0;
    createIconObject.isSea_ = Singleton<NGGameDataManager>.GetInstance().IsSea && questType.HasValue && questType.Value != CommonQuestType.Sea;
    createIconObject.uniqueIcon = (UniqueIcons) null;
    createIconObject.objIcon = (GameObject) null;
    GameObject linkTarget = ((Component) createIconObject).gameObject;
    createIconObject.isDetail = false;
    createIconObject.isReisou = false;
    Future<GameObject> PrefabF;
    UnitIcon link;
    Future<GameObject> gearPrefabF;
    ItemIcon gearIcon;
    GearGear gear;
    Future<GameObject> supplyPrefabF;
    ItemIcon supplyPrefab;
    Future<GameObject> prefabF;
    FacilityIcon FacilityIcon;
    IEnumerator e;
    switch (rewardType)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        PrefabF = createIconObject.isSea_ ? CreateIconObject.normal_sea.Load<GameObject>() : Res.Prefabs.UnitIcon.normal.Load<GameObject>();
        e = PrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        link = PrefabF.Result.CloneAndGetComponent<UnitIcon>(linkTarget);
        UnitUnit unit1 = (UnitUnit) null;
        if (MasterData.UnitUnit.TryGetValue(rewardID, out unit1))
        {
          e = link.SetUnit(unit1, unit1.GetElement(), false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (!visibleBottom)
            link.BottomModeValue = UnitIconBase.BottomMode.Nothing;
          if (!isButton)
            ((Component) link.Button).gameObject.SetActive(false);
          link.SetCounter(quantity, true);
        }
        else
          link.SetEmpty();
        createIconObject.objIcon = ((Component) link).gameObject;
        if (isRouletteWheelIcon)
        {
          link.isViewBackObject = false;
          break;
        }
        break;
      case MasterDataTable.CommonRewardType.supply:
        supplyPrefabF = createIconObject.isSea_ ? CreateIconObject.prefab_sea.Load<GameObject>() : Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
        e = supplyPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        supplyPrefab = supplyPrefabF.Result.Clone(linkTarget.transform).GetComponent<ItemIcon>();
        if (isRouletteWheelIcon)
          supplyPrefab.supply.back.SetActive(false);
        SupplySupply supply = (SupplySupply) null;
        if (MasterData.SupplySupply.TryGetValue(rewardID, out supply))
        {
          e = supplyPrefab.InitBySupply(supply);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (isQuantity)
            supplyPrefab.EnableQuantity(quantity);
          else
            supplyPrefab.QuantitySupply = false;
          if (!visibleBottom)
            supplyPrefab.BottomModeValue = ItemIcon.BottomMode.Nothing;
          if (!isButton)
            supplyPrefab.DisableLongPressEvent();
        }
        else
          supplyPrefab.SetEmpty(true);
        createIconObject.objIcon = ((Component) supplyPrefab).gameObject;
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        gearPrefabF = createIconObject.isSea_ ? CreateIconObject.prefab_sea.Load<GameObject>() : Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
        e = gearPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        gearIcon = gearPrefabF.Result.Clone(linkTarget.transform).GetComponent<ItemIcon>();
        if (isRouletteWheelIcon)
          gearIcon.gear.item_back.SetActive(false);
        gear = (GearGear) null;
        bool isWeaponMaterial = rewardType == MasterDataTable.CommonRewardType.gear_body;
        if (MasterData.GearGear.TryGetValue(rewardID, out gear))
        {
          e = gearIcon.InitByGear(gear, gear.GetElement(), isWeaponMaterial, isRouletteWheelIcon: isRouletteWheelIcon);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (isQuantity)
            gearIcon.EnableQuantityBonus(quantity);
          else
            gearIcon.QuantitySupply = false;
          if (!visibleBottom)
            gearIcon.BottomModeValue = ItemIcon.BottomMode.Nothing;
          if (!isButton)
            gearIcon.DisableLongPressEvent();
          createIconObject.isReisou = gear.isReisou();
        }
        else
          gearIcon.SetEmpty(true);
        createIconObject.objIcon = ((Component) gearIcon).gameObject;
        break;
      case MasterDataTable.CommonRewardType.guild_facility:
        prefabF = new ResourceObject("Prefabs/FacilityIcon/dir_facility_thumb").Load<GameObject>();
        e = prefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        FacilityIcon = prefabF.Result.Clone(linkTarget.transform).GetComponent<FacilityIcon>();
        UnitUnit unit2 = (UnitUnit) null;
        if (MasterData.UnitUnit.TryGetValue(rewardID, out unit2))
        {
          e = FacilityIcon.SetUnit(PlayerUnit.FromFacility(unit2), visibleBottom, true);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
          FacilityIcon.SetEmpty();
        createIconObject.objIcon = ((Component) FacilityIcon).gameObject;
        break;
      default:
        Func<IEnumerator>[] funcArray = new Func<IEnumerator>[41]
        {
          null,
          null,
          null,
          null,
          (Func<IEnumerator>) (() => this.uniqueIcon.SetZeny(quantity)),
          null,
          (Func<IEnumerator>) (() => this.uniqueIcon.SetPlayerExp(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetUnitExp(quantity)),
          null,
          null,
          (Func<IEnumerator>) (() => this.uniqueIcon.SetKiseki(quantity, rewardID == -999)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetApRecover(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetMaxUnit(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetMaxItem(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetMedal(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetPoint(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetEmblem()),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetBattleMedal(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetCpRecover(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetKey(rewardID, quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetGachaTicket(quantity, rewardID)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetSeasonTicket(quantity, rewardID)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetMpRecover(quantity)),
          (Func<IEnumerator>) (() => this.SetTicketIcon(rewardID, quantity)),
          null,
          (Func<IEnumerator>) (() => this.uniqueIcon.SetStamp(quantity)),
          null,
          null,
          (Func<IEnumerator>) (() => this.uniqueIcon.SetTowerMedal(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetAwakeSkill(rewardID)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetDpRecover(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetGuildMedal(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetGuildMap(rewardID)),
          null,
          (Func<IEnumerator>) (() => this.uniqueIcon.SetReincarnationTypeTicket(rewardID, quantity)),
          null,
          (Func<IEnumerator>) (() => this.uniqueIcon.SetRaidMedal(quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetItemIconCommonImage()),
          null,
          (Func<IEnumerator>) (() => this.uniqueIcon.SetRecoveryItemIconImage(rewardID, quantity)),
          (Func<IEnumerator>) (() => this.uniqueIcon.SetCommonTicket(rewardID, quantity))
        };
        if ((MasterDataTable.CommonRewardType) funcArray.Length > rewardType && funcArray[(int) rewardType] != null)
        {
          e = createIconObject.SetUniqueIcon(funcArray[(int) rewardType], linkTarget.transform, visibleBottom, isRouletteWheelIcon);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        if (isButton)
        {
          createIconObject.uniqueIcon.CreateButton();
          break;
        }
        break;
    }
    PrefabF = (Future<GameObject>) null;
    link = (UnitIcon) null;
    gearPrefabF = (Future<GameObject>) null;
    gearIcon = (ItemIcon) null;
    gear = (GearGear) null;
    supplyPrefabF = (Future<GameObject>) null;
    supplyPrefab = (ItemIcon) null;
    prefabF = (Future<GameObject>) null;
    FacilityIcon = (FacilityIcon) null;
    if (Object.op_Inequality((Object) createIconObject.uniqueIcon, (Object) null))
      createIconObject.objIcon = ((Component) createIconObject.uniqueIcon).gameObject;
  }

  private IEnumerator SetTicketIcon(int rewardID, int quantity = 0)
  {
    MasterDataTable.SelectTicket selectTicket = (MasterDataTable.SelectTicket) null;
    IEnumerator e;
    if (MasterData.SelectTicket.TryGetValue(rewardID, out selectTicket))
    {
      if (selectTicket.category == SelectTicketCategory.Unit)
      {
        e = this.uniqueIcon.SetKillersTicket(rewardID, quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = this.uniqueIcon.SetMaterialTicket(rewardID, quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  private IEnumerator SetUniqueIcon(
    Func<IEnumerator> f,
    Transform t,
    bool vB,
    bool isRouletteWheelIcon = false)
  {
    IEnumerator e = this.CreateUniqueIcon(t, isRouletteWheelIcon);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = f();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.uniqueIcon.LabelActivated = vB;
  }

  private IEnumerator CreateUniqueIcon(Transform linktarget, bool isRouletteWheelIcon = false)
  {
    Future<GameObject> iconPrefabF = this.isSea_ ? CreateIconObject.UniqueIconPrefab_sea.Load<GameObject>() : Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = iconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.uniqueIcon = iconPrefabF.Result.Clone(linktarget).GetComponent<UniqueIcons>();
    if (isRouletteWheelIcon)
      this.uniqueIcon.BackGroundActivated = false;
  }

  public void SetDetailEvent(
    MasterDataTable.CommonRewardType rewardType,
    int rewardID,
    int quantity,
    GameObject popupPrefab = null)
  {
    this.rewardType = rewardType;
    this.rewardID = rewardID;
    this.quantity = quantity;
    this.isDetail = false;
    if (((IEnumerable<int>) this.cantDetailItems).Any<int>((Func<int, bool>) (x => (MasterDataTable.CommonRewardType) x == rewardType)))
      return;
    switch (rewardType)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        UnitIcon component1 = this.objIcon.GetComponent<UnitIcon>();
        component1.onClick = (Action<UnitIconBase>) (x => this.onDetail());
        component1.onLongPress = (Action<UnitIconBase>) (x => this.onDetail());
        break;
      case MasterDataTable.CommonRewardType.supply:
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        bool isGear = true;
        if (rewardType == MasterDataTable.CommonRewardType.supply)
          isGear = false;
        ItemIcon component2 = this.objIcon.GetComponent<ItemIcon>();
        component2.onClick = (Action<ItemIcon>) (x => this.onDetail());
        component2.EnableLongPressEvent(isGear, new Action<ItemIcon>(this.onDetailByItemIcon));
        break;
      default:
        this.uniqueIcon.onClick = (Action<UniqueIcons>) (x => this.onDetail());
        this.uniqueIcon.onLongPress = (Action<UniqueIcons>) (x => this.onDetail());
        break;
    }
    this.StartCoroutine(this.SetDetailPopupPrefab(popupPrefab));
  }

  public IEnumerator SetDetailPopupPrefab(GameObject popupPrefab = null)
  {
    if (Object.op_Equality((Object) popupPrefab, (Object) null))
    {
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.detailPopup = prefabF.Result;
      prefabF = (Future<GameObject>) null;
    }
    else
      this.detailPopup = popupPrefab;
  }

  private void onDetail()
  {
    if (this.isDetail)
      return;
    this.isDetail = true;
    this.StartCoroutine(this.openDetail(this.rewardType, this.rewardID, this.quantity));
  }

  private void onDetailByItemIcon(ItemIcon itemIcon) => this.onDetail();

  private IEnumerator openDetail(MasterDataTable.CommonRewardType rewardType, int rewardID, int quantity)
  {
    if (this.isReisou)
    {
      GearGear gear = (GearGear) null;
      MasterData.GearGear.TryGetValue(rewardID, out gear);
      PlayerItem playerItemDummy = new PlayerItem(gear, MasterDataTable.CommonRewardType.gear);
      GameCore.ItemInfo itemInfo = new GameCore.ItemInfo(playerItemDummy);
      yield return (object) this.objIcon.GetComponent<ItemIcon>().OpenReisouDetailPopupAsync(itemInfo, playerItemDummy);
      this.isDetail = false;
    }
    else
    {
      if (Object.op_Equality((Object) this.detailPopup, (Object) null))
        yield return (object) this.SetDetailPopupPrefab();
      GameObject detail = this.detailPopup.Clone();
      detail.SetActive(false);
      IEnumerator e = detail.GetComponent<ItemDetailPopupBase>().SetInfo(rewardType, rewardID, quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(detail, isCloned: true);
      detail.SetActive(true);
      this.isDetail = false;
    }
  }

  public void addComponentUniqueIconDragScrollView()
  {
    if (!Object.op_Inequality((Object) this.uniqueIcon, (Object) null))
      return;
    ((Component) this.uniqueIcon).gameObject.AddComponent<UIDragScrollView>();
  }

  public void DisableLongPressEvent()
  {
    switch (this.rewardType)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        ((Component) this.objIcon.GetComponent<UnitIcon>().Button).gameObject.SetActive(false);
        break;
      case MasterDataTable.CommonRewardType.supply:
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        this.objIcon.GetComponent<ItemIcon>().DisableLongPressEvent();
        break;
      default:
        if (!Object.op_Implicit((Object) this.uniqueIcon))
          break;
        this.uniqueIcon.DisableLongPressEvent();
        break;
    }
  }
}
