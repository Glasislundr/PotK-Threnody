// Decompiled with JetBrains decompiler
// Type: ShopCommon
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
public class ShopCommon
{
  public static GameObject UnitIconPrefab;
  public static GameObject ItemIconPrefab;
  public static GameObject UniqueIconPrefab;
  public static Sprite PayTypeZeny;
  public static Sprite PayTypeMedal;
  public static Sprite PayTypeBattleMedal;
  public static Sprite PayTypeCoin;
  public static Sprite PayTypeRaidJuel;
  public static Sprite PayTypeGuildMedal;
  public static Sprite PayTypeTowerMedal;
  public static GameObject LimitEmphasiePrefab;
  public static DateTime LoginTime;
  public static Color TabDisableTextColor = new Color(0.5019608f, 0.5019608f, 0.5019608f);

  public static IEnumerator Init()
  {
    Future<GameObject> unitIconPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = unitIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopCommon.UnitIconPrefab = unitIconPrefabF.Result;
    Future<GameObject> itemIconPrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = itemIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopCommon.ItemIconPrefab = itemIconPrefabF.Result;
    Future<GameObject> uniqueIconPrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    e = uniqueIconPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopCommon.UniqueIconPrefab = uniqueIconPrefabF.Result;
    Future<Sprite> zenySpriteF = Res.Icons.Zeny_Icon.Load<Sprite>();
    e = zenySpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopCommon.PayTypeZeny = zenySpriteF.Result;
    Future<Sprite> medalSpriteF = Res.Icons.Medal_Icon.Load<Sprite>();
    e = medalSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopCommon.PayTypeMedal = medalSpriteF.Result;
    Future<Sprite> battleMedalSpriteF = Res.Icons.BattleMedal_Icon.Load<Sprite>();
    e = battleMedalSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopCommon.PayTypeBattleMedal = battleMedalSpriteF.Result;
    Future<Sprite> coinSpriteF = Res.Icons.Kiseki_Icon.Load<Sprite>();
    e = coinSpriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopCommon.PayTypeCoin = coinSpriteF.Result;
    Future<GameObject> limitEmphasieF = new ResourceObject("Prefabs/shop007_1/dir_banner_limit_Emphasie").Load<GameObject>();
    e = limitEmphasieF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ShopCommon.LimitEmphasiePrefab = limitEmphasieF.Result;
    ShopCommon.LoginTime = ServerTime.NowAppTime();
  }

  public static IEnumerator CreateShopIcon(UI2DSprite sprite, string resourceName)
  {
    Future<Sprite> f = Singleton<ResourceManager>.GetInstance().Load<Sprite>("ShopIcon/" + resourceName);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    sprite.sprite2D = f.Result;
  }

  public static IEnumerator CreateShopPack(UI2DSprite sprite, string resourceName)
  {
    Future<Sprite> f = Singleton<ResourceManager>.GetInstance().Load<Sprite>("ShopPack/" + resourceName);
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    sprite.sprite2D = f.Result;
  }

  public static IEnumerator CreateThum(Transform parent, MasterDataTable.CommonRewardType commonRewardType, int id)
  {
    ItemIcon gearItemIcon;
    ItemIcon supplyItemIcon;
    IEnumerator e;
    switch (commonRewardType)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        UnitIcon component1 = ShopCommon.UnitIconPrefab.Clone(parent).GetComponent<UnitIcon>();
        ((Component) component1.Button).gameObject.SetActive(false);
        UnitUnit unit = (UnitUnit) null;
        if (MasterData.UnitUnit.TryGetValue(id, out unit))
        {
          e = component1.SetUnit(unit, unit.GetElement(), false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          break;
        }
        component1.SetEmpty();
        break;
      case MasterDataTable.CommonRewardType.supply:
        supplyItemIcon = ShopCommon.ItemIconPrefab.Clone(parent).GetComponent<ItemIcon>();
        SupplySupply supply = (SupplySupply) null;
        if (MasterData.SupplySupply.TryGetValue(id, out supply))
        {
          e = supplyItemIcon.InitBySupply(supply);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          supplyItemIcon.EnableQuantity(0);
          supplyItemIcon.BottomModeValue = ItemIcon.BottomMode.Nothing;
        }
        else
          supplyItemIcon.SetEmpty(true);
        supplyItemIcon.isButtonActive = false;
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        gearItemIcon = ShopCommon.ItemIconPrefab.Clone(parent).GetComponent<ItemIcon>();
        GearGear gear = (GearGear) null;
        if (MasterData.GearGear.TryGetValue(id, out gear))
        {
          e = gearItemIcon.InitByGear(gear, gear.GetElement(), commonRewardType == MasterDataTable.CommonRewardType.gear_body);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          gearItemIcon.QuantitySupply = false;
        }
        else
          gearItemIcon.SetEmpty(true);
        gearItemIcon.isButtonActive = false;
        break;
      default:
        UniqueIcons component2 = ShopCommon.UniqueIconPrefab.Clone(parent).GetComponent<UniqueIcons>();
        component2.LabelActivated = false;
        switch (commonRewardType)
        {
          case MasterDataTable.CommonRewardType.recover:
            e = component2.SetApRecover(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.max_item:
            e = component2.SetMaxItem(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.medal:
            e = component2.SetMedal(id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.battle_medal:
            e = component2.SetBattleMedal(id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.cp_recover:
            e = component2.SetCpRecover(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.quest_key:
            e = component2.SetKey(id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.gacha_ticket:
            e = component2.SetGachaTicket(id: id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.season_ticket:
            e = component2.SetSeasonTicket(id: id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.mp_recover:
            e = component2.SetMpRecover(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.unit_ticket:
            e = component2.SetKillersTicket(id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.guild_town:
            e = component2.SetGuildMap(id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.guild_facility:
            int id1 = 0;
            PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x.master.ID == id));
            if (playerGuildFacility != null)
            {
              id1 = playerGuildFacility.unit.ID;
            }
            else
            {
              FacilityLevel facilityLevel = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).FirstOrDefault<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.level == 1 && x.facility_MapFacility == id));
              if (facilityLevel != null)
                id1 = facilityLevel.unit.ID;
            }
            if (id1 != 0)
            {
              e = component2.SetGuildFacility(id1);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            }
            break;
          case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
            e = component2.SetReincarnationTypeTicket(id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.recovery_item:
            e = component2.SetRecoveryItemIconImage(id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
        }
        break;
    }
    gearItemIcon = (ItemIcon) null;
    supplyItemIcon = (ItemIcon) null;
  }

  public static Sprite GetPayTypeIcon(CommonPayType payType)
  {
    Sprite payTypeIcon;
    switch (payType)
    {
      case CommonPayType.coin:
      case CommonPayType.paid_coin:
        payTypeIcon = ShopCommon.PayTypeCoin;
        break;
      case CommonPayType.money:
        payTypeIcon = ShopCommon.PayTypeZeny;
        break;
      case CommonPayType.medal:
        payTypeIcon = ShopCommon.PayTypeMedal;
        break;
      case CommonPayType.battle_medal:
        payTypeIcon = ShopCommon.PayTypeBattleMedal;
        break;
      default:
        Debug.LogError((object) string.Format("想定していない支払い種別です. {0}", (object) payType));
        payTypeIcon = (Sprite) null;
        break;
    }
    return payTypeIcon;
  }

  public static string GetLimitCountText(int? limitType, int? limitCount)
  {
    string limitCountText;
    switch (limitType.HasValue ? (ShopCommon.LimitType) limitType.Value : ShopCommon.LimitType.None)
    {
      case ShopCommon.LimitType.Period:
      case ShopCommon.LimitType.OneMonth:
        limitCountText = string.Format("あと{0}回", (object) limitCount);
        break;
      case ShopCommon.LimitType.OneDay:
        limitCountText = string.Format("１日{0}回", (object) limitCount);
        break;
      default:
        limitCountText = "";
        break;
    }
    return limitCountText;
  }

  public static long GetHaveCount(CommonPayType payType)
  {
    long haveCount = 0;
    switch (payType)
    {
      case CommonPayType.coin:
        haveCount = (long) SMManager.Get<Player>().coin;
        break;
      case CommonPayType.money:
        haveCount = SMManager.Get<Player>().money;
        break;
      case CommonPayType.medal:
        haveCount = (long) SMManager.Get<Player>().medal;
        break;
      case CommonPayType.battle_medal:
        haveCount = (long) SMManager.Get<Player>().battle_medal;
        break;
      case CommonPayType.tower_medal:
        haveCount = (long) TowerUtil.TowerPlayer.tower_medal;
        break;
      case CommonPayType.paid_coin:
        haveCount = (long) SMManager.Get<Player>().paid_coin;
        break;
      case CommonPayType.guild_medal:
        haveCount = (long) PlayerAffiliation.Current.guild_medal;
        break;
      case CommonPayType.raid_medal:
        haveCount = (long) SMManager.Get<Player>().raid_medal;
        break;
      default:
        Debug.LogError((object) string.Format("想定していない支払い種別です. {0}", (object) payType));
        break;
    }
    return haveCount;
  }

  public static IEnumerator ShowSimpleDetailPopup(MasterDataTable.CommonRewardType rewardType, int rewardId)
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject detail = prefabF.Result.Clone();
    detail.SetActive(false);
    e = detail.GetComponent<ItemDetailPopupBase>().SetInfo(rewardType, rewardId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(detail, isCloned: true);
    detail.SetActive(true);
  }

  public static IEnumerator ShowMoreDetailPopup(MasterDataTable.CommonRewardType rewardType, int rewardId)
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject DetailPrefab = prefabF.Result;
    prefabF = new ResourceObject("Prefabs/popup/popup_031_map_detail__anim_popup01").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject MapDetailPrefab = prefabF.Result;
    prefabF = new ResourceObject("Prefabs/popup/popup_031_facility_detail__anim_popup01").Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    if (rewardType == MasterDataTable.CommonRewardType.guild_town)
      yield return (object) ShopCommon.SetMapDetailPopup(rewardId, MapDetailPrefab);
    else if (rewardType == MasterDataTable.CommonRewardType.guild_facility)
    {
      yield return (object) ShopCommon.SetFacilityDetailPopup(rewardId, result);
    }
    else
    {
      GameObject popup = Singleton<PopupManager>.GetInstance().open(DetailPrefab);
      popup.SetActive(false);
      e = popup.GetComponent<Shop00742Menu>().Init(rewardType, rewardId);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
    }
  }

  public static bool IsNewLimitedShop(DateTime serverTime)
  {
    foreach (LimitShopInfo limitShopInfo in Singleton<NGGameDataManager>.GetInstance().limitShopInfos)
    {
      if (ShopCommon.IsNewLimitedShopBanner(serverTime, limitShopInfo.banner_id))
        return true;
    }
    return false;
  }

  public static bool IsNewLimitedShopBanner(DateTime serverTime, int bannerId)
  {
    LimitShopInfo limitShopInfo = ((IEnumerable<LimitShopInfo>) Singleton<NGGameDataManager>.GetInstance().limitShopInfos).FirstOrDefault<LimitShopInfo>((Func<LimitShopInfo, bool>) (x => x.banner_id == bannerId));
    if (limitShopInfo == null)
      return false;
    DateTime limitShopStartAt = limitShopInfo.limit_shop_start_at;
    DateTime limitShopEndAt = limitShopInfo.limit_shop_end_at;
    DateTime dateTime = new DateTime();
    if (Persist.lastAccessTime.Data.limitedShopLatestLoginTimes.ContainsKey(limitShopInfo.banner_id))
      dateTime = Persist.lastAccessTime.Data.limitedShopLatestLoginTimes[limitShopInfo.banner_id];
    return limitShopStartAt < serverTime && serverTime < limitShopEndAt && dateTime < limitShopStartAt;
  }

  private static IEnumerator SetMapDetailPopup(int rewardId, GameObject prefab)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(prefab);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<PopupMapDetailMenu>().InitializeAsync(rewardId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  private static IEnumerator SetFacilityDetailPopup(int rewardId, GameObject prefab)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(prefab);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<PopupFacilityDetailMenu>().InitializeAsync(rewardId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public enum LimitType
  {
    None,
    Period,
    OneDay,
    OneMonth,
  }
}
