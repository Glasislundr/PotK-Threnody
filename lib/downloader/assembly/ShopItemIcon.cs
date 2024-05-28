// Decompiled with JetBrains decompiler
// Type: ShopItemIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ShopItemIcon : MonoBehaviour
{
  [SerializeField]
  private UIButton button;
  [SerializeField]
  private UILabel ItemName;
  [SerializeField]
  private Transform ThumParent;
  [SerializeField]
  private UI2DSprite PackThum;
  [SerializeField]
  private GameObject PackThumMount;
  private GameObject UnitIconShop;
  private GameObject ItemIconShop;
  private GameObject UniqueIconShop;
  private string shopTime;
  [SerializeField]
  private UILabel LimitLabel;
  [SerializeField]
  private UILabel HavingNumLabel;
  [SerializeField]
  private UI2DSprite PayTypeThum;
  [SerializeField]
  private GameObject Paid;
  [SerializeField]
  private GameObject NotPaid;
  [SerializeField]
  private UILabel PriceLabel;
  [SerializeField]
  private Transform LimitPeriod;
  private BannerLimitEmphasie limitEmphasieScript;
  [SerializeField]
  private UI2DSprite Decoration;
  [SerializeField]
  private GameObject PackMark;
  [SerializeField]
  private GameObject SoldOut;
  private ShopItemListMenu menu;
  public ShopItemIconInfo info;

  public IEnumerator Init(ShopItemListMenu menu, UIScrollView scrollView, string shopTime)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ShopItemIcon shopItemIcon = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    shopItemIcon.menu = menu;
    shopItemIcon.shopTime = shopTime;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(shopItemIcon.button.onClick, new EventDelegate.Callback(shopItemIcon.\u003CInit\u003Eb__22_0));
    ((Component) shopItemIcon.button).GetComponent<UIDragScrollView>().scrollView = scrollView;
    shopItemIcon.UnitIconShop = ShopCommon.UnitIconPrefab.Clone(shopItemIcon.ThumParent);
    shopItemIcon.ItemIconShop = ShopCommon.ItemIconPrefab.Clone(shopItemIcon.ThumParent);
    shopItemIcon.UniqueIconShop = ShopCommon.UniqueIconPrefab.Clone(shopItemIcon.ThumParent);
    ((Component) shopItemIcon.PackThum).gameObject.SetActive(false);
    shopItemIcon.PackThumMount.SetActive(false);
    shopItemIcon.UnitIconShop.SetActive(false);
    shopItemIcon.ItemIconShop.SetActive(false);
    shopItemIcon.UniqueIconShop.SetActive(false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) shopItemIcon.UpdateView();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator UpdateView()
  {
    this.ItemName.text = this.info.ItemName;
    yield return (object) this.UpdateThum();
    if (this.info.playerShopArticle.article.shop.ID != 5000)
    {
      if (this.info.playerShopArticle.end_at.HasValue)
      {
        if (Object.op_Equality((Object) this.limitEmphasieScript, (Object) null))
        {
          this.limitEmphasieScript = ShopCommon.LimitEmphasiePrefab.Clone(this.LimitPeriod).GetComponent<BannerLimitEmphasie>();
          this.limitEmphasieScript.Init(ShopCommon.LoginTime, this.info.playerShopArticle.end_at);
        }
        else
          this.limitEmphasieScript.Init(ShopCommon.LoginTime, this.info.playerShopArticle.end_at);
        ((Component) this.limitEmphasieScript).gameObject.SetActive(true);
      }
      else if (Object.op_Inequality((Object) this.limitEmphasieScript, (Object) null))
        ((Component) this.limitEmphasieScript).gameObject.SetActive(false);
    }
    this.LimitLabel.text = ShopCommon.GetLimitCountText(this.info.LimitType, this.info.LimitCount);
    if (this.info.IsPack)
    {
      ((Component) this.HavingNumLabel).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.HavingNumLabel).gameObject.SetActive(true);
      this.HavingNumLabel.text = string.Format("所持数： {0}", (object) CommonRewardType.GetHaveCount((MasterDataTable.CommonRewardType) this.info.playerShopArticle.contents[0].reward_type_id, this.info.playerShopArticle.contents[0].reward_id));
    }
    this.PayTypeThum.sprite2D = ShopCommon.GetPayTypeIcon(this.info.PayType);
    if (this.info.PayType == CommonPayType.paid_coin)
    {
      this.Paid.SetActive(true);
      this.NotPaid.SetActive(false);
    }
    else
    {
      this.Paid.SetActive(false);
      this.NotPaid.SetActive(true);
    }
    this.PriceLabel.text = string.Format("{0}", (object) this.info.PayCount);
    if (this.info.LimitCount.HasValue)
    {
      int? limitCount = this.info.LimitCount;
      int num = 0;
      if (limitCount.GetValueOrDefault() <= num & limitCount.HasValue)
      {
        this.SoldOut.SetActive(true);
        yield break;
      }
    }
    this.SoldOut.SetActive(false);
  }

  private IEnumerator UpdateThum()
  {
    ShopItemIcon shopItemIcon = this;
    if (shopItemIcon.info.playerShopArticle.decoration_resource != "")
    {
      ((Component) shopItemIcon.Decoration).gameObject.SetActive(true);
      yield return (object) ShopCommon.CreateShopIcon(shopItemIcon.Decoration, shopItemIcon.info.playerShopArticle.decoration_resource);
    }
    else
      ((Component) shopItemIcon.Decoration).gameObject.SetActive(false);
    shopItemIcon.PackMark.SetActive(shopItemIcon.info.IsPack);
    if (shopItemIcon.info.playerShopArticle.icon_resource != "")
    {
      yield return (object) ShopCommon.CreateShopPack(shopItemIcon.PackThum, shopItemIcon.info.playerShopArticle.icon_resource);
      ((Component) shopItemIcon.PackThum).gameObject.SetActive(true);
      shopItemIcon.PackThumMount.SetActive(true);
      shopItemIcon.UnitIconShop.SetActive(false);
      shopItemIcon.ItemIconShop.SetActive(false);
      shopItemIcon.UniqueIconShop.SetActive(false);
    }
    else
    {
      ItemIcon gearItemIcon;
      ItemIcon supplyItemIcon;
      IEnumerator e;
      switch (shopItemIcon.info.CommonRewardType)
      {
        case MasterDataTable.CommonRewardType.unit:
        case MasterDataTable.CommonRewardType.material_unit:
          UnitIcon component1 = shopItemIcon.UnitIconShop.GetComponent<UnitIcon>();
          ((Component) component1.Button).gameObject.SetActive(false);
          UnitUnit unit = (UnitUnit) null;
          if (MasterData.UnitUnit.TryGetValue(shopItemIcon.info.RewardId, out unit))
          {
            e = component1.SetUnit(unit, unit.GetElement(), false);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          else
            component1.SetEmpty();
          ((Component) shopItemIcon.PackThum).gameObject.SetActive(false);
          shopItemIcon.UnitIconShop.SetActive(true);
          shopItemIcon.ItemIconShop.SetActive(false);
          shopItemIcon.UniqueIconShop.SetActive(false);
          shopItemIcon.UnitIconShop.transform.localPosition = new Vector3(0.0f, 7f, 0.0f);
          break;
        case MasterDataTable.CommonRewardType.supply:
          supplyItemIcon = shopItemIcon.ItemIconShop.GetComponent<ItemIcon>();
          supplyItemIcon.isButtonActive = false;
          SupplySupply supply = (SupplySupply) null;
          if (MasterData.SupplySupply.TryGetValue(shopItemIcon.info.RewardId, out supply))
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
          ((Component) shopItemIcon.PackThum).gameObject.SetActive(false);
          shopItemIcon.UnitIconShop.SetActive(false);
          shopItemIcon.ItemIconShop.SetActive(true);
          shopItemIcon.UniqueIconShop.SetActive(false);
          break;
        case MasterDataTable.CommonRewardType.gear:
        case MasterDataTable.CommonRewardType.material_gear:
        case MasterDataTable.CommonRewardType.gear_body:
          gearItemIcon = shopItemIcon.ItemIconShop.GetComponent<ItemIcon>();
          gearItemIcon.isButtonActive = false;
          GearGear gear = (GearGear) null;
          if (MasterData.GearGear.TryGetValue(shopItemIcon.info.RewardId, out gear))
          {
            e = gearItemIcon.InitByGear(gear, gear.GetElement(), shopItemIcon.info.CommonRewardType == MasterDataTable.CommonRewardType.gear_body);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            gearItemIcon.QuantitySupply = false;
          }
          else
            gearItemIcon.SetEmpty(true);
          gearItemIcon.isButtonActive = false;
          ((Component) shopItemIcon.PackThum).gameObject.SetActive(false);
          shopItemIcon.UnitIconShop.SetActive(false);
          shopItemIcon.ItemIconShop.SetActive(true);
          shopItemIcon.UniqueIconShop.SetActive(false);
          break;
        default:
          UniqueIcons component2 = shopItemIcon.UniqueIconShop.GetComponent<UniqueIcons>();
          component2.LabelActivated = false;
          switch (shopItemIcon.info.CommonRewardType)
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
              e = component2.SetMedal(shopItemIcon.info.RewardId);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            case MasterDataTable.CommonRewardType.battle_medal:
              e = component2.SetBattleMedal(shopItemIcon.info.RewardId);
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
              e = component2.SetKey(shopItemIcon.info.RewardId);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            case MasterDataTable.CommonRewardType.gacha_ticket:
              e = component2.SetGachaTicket(id: shopItemIcon.info.RewardId);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            case MasterDataTable.CommonRewardType.season_ticket:
              e = component2.SetSeasonTicket(id: shopItemIcon.info.RewardId);
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
              e = component2.SetKillersTicket(shopItemIcon.info.RewardId);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            case MasterDataTable.CommonRewardType.guild_town:
              e = component2.SetGuildMap(shopItemIcon.info.RewardId);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            case MasterDataTable.CommonRewardType.guild_facility:
              int id = 0;
              // ISSUE: reference to a compiler-generated method
              PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>(new Func<PlayerGuildFacility, bool>(shopItemIcon.\u003CUpdateThum\u003Eb__24_0));
              if (playerGuildFacility != null)
              {
                id = playerGuildFacility.unit.ID;
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                FacilityLevel facilityLevel = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).FirstOrDefault<FacilityLevel>(new Func<FacilityLevel, bool>(shopItemIcon.\u003CUpdateThum\u003Eb__24_1));
                if (facilityLevel != null)
                  id = facilityLevel.unit.ID;
              }
              if (id != 0)
              {
                e = component2.SetGuildFacility(id);
                while (e.MoveNext())
                  yield return e.Current;
                e = (IEnumerator) null;
                break;
              }
              break;
            case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
              e = component2.SetReincarnationTypeTicket(shopItemIcon.info.RewardId);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            case MasterDataTable.CommonRewardType.recovery_item:
              e = component2.SetRecoveryItemIconImage(shopItemIcon.info.RewardId);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
          }
          ((Component) shopItemIcon.PackThum).gameObject.SetActive(false);
          shopItemIcon.UnitIconShop.SetActive(false);
          shopItemIcon.ItemIconShop.SetActive(false);
          shopItemIcon.UniqueIconShop.SetActive(true);
          shopItemIcon.UniqueIconShop.transform.localPosition = new Vector3(0.0f, -6f, 0.0f);
          break;
      }
      gearItemIcon = (ItemIcon) null;
      supplyItemIcon = (ItemIcon) null;
    }
  }

  private void OnClick()
  {
    if (this.menu.IsPushAndSet())
      return;
    this.StartCoroutine(this.IOnClick());
  }

  private IEnumerator IOnClick()
  {
    Future<GameObject> prefabF = new ResourceObject("Prefabs/shop007_4_1/" + (!this.info.IsPack ? "popup_Shop_PurchaseConfirmation_Description" : "popup_Shop_PurchaseConfirmation_Pack_Description")).Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) Singleton<PopupManager>.GetInstance().open(prefabF.Result).GetComponent<ShopPurchaseConfirmation>().Init(this.info, this.shopTime);
  }
}
