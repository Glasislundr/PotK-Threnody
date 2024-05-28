// Decompiled with JetBrains decompiler
// Type: Shop0074Scroll
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
public class Shop0074Scroll : MonoBehaviour
{
  [SerializeField]
  private GameObject soldout;
  [SerializeField]
  private GameObject shortage;
  [SerializeField]
  private GameObject buy;
  [SerializeField]
  private UILabel quantityLabel;
  [SerializeField]
  private GameObject timerBase;
  [SerializeField]
  private UILabel timerLabel;
  [SerializeField]
  private UILabel piceLabel;
  [SerializeField]
  private UILabel priceLabel;
  [SerializeField]
  private UILabel ownLabel;
  [SerializeField]
  private UILabel titleLabel;
  [SerializeField]
  private UILabel descriptionLabel;
  [SerializeField]
  private BoxCollider ibtnDetailCollider;
  [SerializeField]
  private GameObject[] medalIconObject;
  [SerializeField]
  private GameObject dirThum;
  private GameObject unitIconObj;
  private GameObject itemIconObj;
  private GameObject uniqueIconObj;
  private bool _isUpdateEndTime;
  private PlayerQuestKey key;
  private PlayerSeasonTicket sTicket;
  private PlayerGachaTicket gTicket;
  private PlayerSelectTicketSummary playerUnitTicket;
  private PlayerGuildFacility facility;
  private PlayerUnitTypeTicket uTypeTicket;
  private PlayerRecoveryItem playerRecoveryItem;
  private GameObject linkTarget;
  private Action<long> _onPurchasedHolding;
  private Func<IEnumerator> _onPurchased;
  private long holding;
  private ShopArticleListMenu _menu;
  private int _playerItemQuantity;
  private int _playerUnitQuantity;
  private int _playerRecoveryItemQuantity;
  private const int QUEST_KEY_MAX = 9999;

  public PlayerShopArticle _playerShopArticle { get; set; }

  public Shop _shop { get; set; }

  public bool IsSoldOut { get; set; }

  public IEnumerator Init(
    PlayerShopArticle playerShopArticle,
    Shop shop,
    ShopArticleListMenu menu,
    Func<IEnumerator> onPurchased,
    Action<long> onPurchasedHolding,
    GameObject unitIconPrefab,
    GameObject itemIconPrefab,
    GameObject uniqueIconPrefab,
    bool isUpdateEndTime = true,
    long sub_holding = 0)
  {
    Shop0074Scroll shop0074Scroll1 = this;
    shop0074Scroll1._menu = menu;
    shop0074Scroll1._playerShopArticle = playerShopArticle;
    shop0074Scroll1._shop = shop;
    shop0074Scroll1._isUpdateEndTime = isUpdateEndTime;
    ShopContent content = shop0074Scroll1._playerShopArticle.article.ShopContents[0];
    int entity_id = content.entity_id;
    MasterDataTable.CommonRewardType entityType = content.entity_type;
    shop0074Scroll1._playerItemQuantity = 0;
    shop0074Scroll1._playerItemQuantity += SMManager.Get<PlayerItem[]>().AmountHavingTargetItem(entity_id, entityType);
    shop0074Scroll1._playerItemQuantity += SMManager.Get<PlayerMaterialGear[]>().AmountHavingTargetItem(entity_id);
    shop0074Scroll1._playerUnitQuantity = 0;
    shop0074Scroll1.key = (PlayerQuestKey) null;
    switch (entityType)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        if (MasterData.UnitUnit[entity_id].IsNormalUnit)
        {
          shop0074Scroll1._playerUnitQuantity = SMManager.Get<PlayerUnit[]>().AmountHavingTargetUnit(entity_id, entityType);
          break;
        }
        Shop0074Scroll shop0074Scroll2 = shop0074Scroll1;
        PlayerMaterialUnit playerMaterialUnit = Array.Find<PlayerMaterialUnit>(SMManager.Get<PlayerMaterialUnit[]>(), (Predicate<PlayerMaterialUnit>) (x => x._unit == entity_id));
        int quantity1 = playerMaterialUnit != null ? playerMaterialUnit.quantity : 0;
        shop0074Scroll2._playerUnitQuantity = quantity1;
        break;
      case MasterDataTable.CommonRewardType.quest_key:
        shop0074Scroll1.key = Array.Find<PlayerQuestKey>(SMManager.Get<PlayerQuestKey[]>(), (Predicate<PlayerQuestKey>) (x => x.quest_key_id == entity_id));
        break;
    }
    shop0074Scroll1._onPurchasedHolding = onPurchasedHolding;
    shop0074Scroll1._onPurchased = onPurchased;
    switch (shop0074Scroll1._menu.currencyType)
    {
      case ShopArticleListMenu.CurrencyType.RareMedal:
        shop0074Scroll1.holding = (long) SMManager.Get<Player>().medal;
        break;
      case ShopArticleListMenu.CurrencyType.BattleMedal:
        shop0074Scroll1.holding = (long) SMManager.Get<Player>().battle_medal;
        break;
      case ShopArticleListMenu.CurrencyType.Zeny:
        shop0074Scroll1.holding = SMManager.Get<Player>().money;
        break;
      case ShopArticleListMenu.CurrencyType.TowerMedal:
        Shop0074Scroll shop0074Scroll3 = shop0074Scroll1;
        TowerPlayer towerPlayer = TowerUtil.TowerPlayer;
        long towerMedal = towerPlayer != null ? (long) towerPlayer.tower_medal : 0L;
        shop0074Scroll3.holding = towerMedal;
        break;
      case ShopArticleListMenu.CurrencyType.GuildMedal:
        Shop0074Scroll shop0074Scroll4 = shop0074Scroll1;
        PlayerAffiliation current = PlayerAffiliation.Current;
        long guildMedal = current != null ? (long) current.guild_medal : 0L;
        shop0074Scroll4.holding = guildMedal;
        break;
      case ShopArticleListMenu.CurrencyType.RaidMedal:
        shop0074Scroll1.holding = (long) SMManager.Get<Player>().raid_medal;
        break;
      case ShopArticleListMenu.CurrencyType.SubCoin:
        shop0074Scroll1.holding = sub_holding;
        break;
      default:
        shop0074Scroll1.holding = 0L;
        break;
    }
    shop0074Scroll1.ArticleLimitValue(shop0074Scroll1._playerShopArticle.article, shop0074Scroll1._playerShopArticle, shop0074Scroll1.holding);
    shop0074Scroll1.titleLabel.SetTextLocalize(shop0074Scroll1._playerShopArticle.article.name);
    shop0074Scroll1.descriptionLabel.SetTextLocalize(shop0074Scroll1._playerShopArticle.article.description);
    if (Object.op_Inequality((Object) shop0074Scroll1.ibtnDetailCollider, (Object) null))
      ((Collider) shop0074Scroll1.ibtnDetailCollider).enabled = true;
    if (Object.op_Inequality((Object) shop0074Scroll1.unitIconObj, (Object) null))
    {
      Object.Destroy((Object) shop0074Scroll1.unitIconObj);
      shop0074Scroll1.unitIconObj = (GameObject) null;
    }
    if (Object.op_Inequality((Object) shop0074Scroll1.itemIconObj, (Object) null))
    {
      Object.Destroy((Object) shop0074Scroll1.itemIconObj);
      shop0074Scroll1.itemIconObj = (GameObject) null;
    }
    if (Object.op_Inequality((Object) shop0074Scroll1.uniqueIconObj, (Object) null))
    {
      Object.Destroy((Object) shop0074Scroll1.uniqueIconObj);
      shop0074Scroll1.uniqueIconObj = (GameObject) null;
    }
    int quantity = 0;
    if (Object.op_Inequality((Object) shop0074Scroll1.ibtnDetailCollider, (Object) null))
      ((Collider) shop0074Scroll1.ibtnDetailCollider).enabled = true;
    IEnumerator e;
    switch (content.entity_type)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        shop0074Scroll1.unitIconObj = shop0074Scroll1.createThumb(unitIconPrefab, new Vector3(-239f, 0.0f, 0.0f));
        quantity = shop0074Scroll1._playerUnitQuantity;
        UnitUnit unit = (UnitUnit) null;
        if (MasterData.UnitUnit.TryGetValue(content.entity_id, out unit))
        {
          e = shop0074Scroll1.unitIconObj.GetComponent<UnitIcon>().SetUnit(unit, unit.GetElement(), false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
        else
          shop0074Scroll1.unitIconObj.GetComponent<UnitIcon>().SetEmpty();
        shop0074Scroll1.unitIconObj.SetActive(true);
        break;
      case MasterDataTable.CommonRewardType.supply:
        shop0074Scroll1.itemIconObj = shop0074Scroll1.createThumb(itemIconPrefab, new Vector3(-239f, -10f, 0.0f));
        quantity = shop0074Scroll1._playerItemQuantity;
        e = shop0074Scroll1.itemIconObj.GetComponent<ItemIcon>().InitByShopContent(content);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        shop0074Scroll1.itemIconObj.gameObject.SetActive(true);
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        shop0074Scroll1.itemIconObj = shop0074Scroll1.createThumb(itemIconPrefab, new Vector3(-239f, 0.0f, 0.0f));
        quantity = shop0074Scroll1._playerItemQuantity;
        e = shop0074Scroll1.itemIconObj.GetComponent<ItemIcon>().InitByShopContent(content);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        shop0074Scroll1.itemIconObj.gameObject.SetActive(true);
        break;
      default:
        shop0074Scroll1.uniqueIconObj = shop0074Scroll1.createThumb(uniqueIconPrefab, new Vector3(-239f, -10f, 0.0f));
        UniqueIcons component = shop0074Scroll1.uniqueIconObj.GetComponent<UniqueIcons>();
        component.LabelActivated = false;
        shop0074Scroll1.uniqueIconObj.SetActive(true);
        quantity = -1;
        switch (content.entity_type)
        {
          case MasterDataTable.CommonRewardType.money:
            e = component.SetZeny();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.player_exp:
            e = component.SetPlayerExp(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.unit_exp:
            e = component.SetUnitExp(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.coin:
            e = component.SetKiseki();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.recover:
            e = component.SetApRecover(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.max_unit:
            e = component.SetMaxUnit(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.max_item:
            e = component.SetMaxItem(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.medal:
            e = component.SetMedal();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.friend_point:
            e = component.SetPoint();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.emblem:
            e = component.SetEmblem();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.battle_medal:
            e = component.SetBattleMedal();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.cp_recover:
            e = component.SetCpRecover(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.quest_key:
            quantity = shop0074Scroll1.key == null ? -1 : shop0074Scroll1.key.quantity;
            e = component.SetKey(content.entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.gacha_ticket:
            shop0074Scroll1.gTicket = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).FirstOrDefault<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket.ID == content.entity_id));
            quantity = shop0074Scroll1.gTicket == null ? -1 : shop0074Scroll1.gTicket.quantity;
            if (Object.op_Inequality((Object) shop0074Scroll1.ibtnDetailCollider, (Object) null))
              ((Collider) shop0074Scroll1.ibtnDetailCollider).enabled = false;
            e = component.SetGachaTicket(id: content.entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.season_ticket:
            shop0074Scroll1.sTicket = ((IEnumerable<PlayerSeasonTicket>) SMManager.Get<PlayerSeasonTicket[]>()).FirstOrDefault<PlayerSeasonTicket>((Func<PlayerSeasonTicket, bool>) (x => x.season_ticket_id == content.entity_id));
            quantity = shop0074Scroll1.sTicket == null ? -1 : shop0074Scroll1.sTicket.quantity;
            e = component.SetSeasonTicket(id: content.entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.mp_recover:
            e = component.SetMpRecover(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.unit_ticket:
            shop0074Scroll1.playerUnitTicket = ((IEnumerable<PlayerSelectTicketSummary>) SMManager.Get<PlayerSelectTicketSummary[]>()).FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == content.entity_id));
            quantity = shop0074Scroll1.playerUnitTicket == null ? -1 : shop0074Scroll1.playerUnitTicket.quantity;
            if (Object.op_Inequality((Object) shop0074Scroll1.ibtnDetailCollider, (Object) null))
              ((Collider) shop0074Scroll1.ibtnDetailCollider).enabled = false;
            e = component.SetKillersTicket(content.entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.stamp:
            e = component.SetStamp(0);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.awake_skill:
            quantity = ((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).Count<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.skill_id == content.entity_id));
            e = component.SetAwakeSkill(content.entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.guild_town:
            e = component.SetGuildMap(content.entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.guild_facility:
            int id = 0;
            shop0074Scroll1.facility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x.master.ID == content.entity_id));
            if (shop0074Scroll1.facility != null)
            {
              quantity = shop0074Scroll1.facility.hasnum;
              id = shop0074Scroll1.facility.unit.ID;
            }
            else
            {
              quantity = 0;
              FacilityLevel facilityLevel = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).FirstOrDefault<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.level == 1 && x.facility_MapFacility == content.entity_id));
              if (facilityLevel != null)
                id = facilityLevel.unit.ID;
            }
            if (id != 0)
            {
              e = component.SetGuildFacility(id);
              while (e.MoveNext())
                yield return e.Current;
              e = (IEnumerator) null;
              break;
            }
            break;
          case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
            shop0074Scroll1.uTypeTicket = ((IEnumerable<PlayerUnitTypeTicket>) SMManager.Get<PlayerUnitTypeTicket[]>()).FirstOrDefault<PlayerUnitTypeTicket>((Func<PlayerUnitTypeTicket, bool>) (x => x.ticket_id == content.entity_id));
            quantity = shop0074Scroll1.uTypeTicket == null ? -1 : shop0074Scroll1.uTypeTicket.quantity;
            if (Object.op_Inequality((Object) shop0074Scroll1.ibtnDetailCollider, (Object) null))
              ((Collider) shop0074Scroll1.ibtnDetailCollider).enabled = false;
            e = component.SetReincarnationTypeTicket(content.entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.challenge_point:
            e = component.SetItemIconCommonImage();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.recovery_item:
            shop0074Scroll1.playerRecoveryItem = ((IEnumerable<PlayerRecoveryItem>) SMManager.Get<PlayerRecoveryItem[]>()).FirstOrDefault<PlayerRecoveryItem>((Func<PlayerRecoveryItem, bool>) (x => x.recovery_item_id == content.entity_id));
            shop0074Scroll1._playerRecoveryItemQuantity = shop0074Scroll1.playerRecoveryItem == null ? 0 : shop0074Scroll1.playerRecoveryItem.quantity;
            quantity = shop0074Scroll1._playerRecoveryItemQuantity;
            if (Object.op_Inequality((Object) shop0074Scroll1.ibtnDetailCollider, (Object) null))
              ((Collider) shop0074Scroll1.ibtnDetailCollider).enabled = false;
            e = component.SetRecoveryItemIconImage(content.entity_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
        }
        break;
    }
    if (shop0074Scroll1._menu.currencyType == ShopArticleListMenu.CurrencyType.Zeny)
      shop0074Scroll1.piceLabel.SetTextLocalize(Consts.GetInstance().SHOP_0074_CURRENCY_TYPE_ZENY);
    else
      shop0074Scroll1.piceLabel.SetTextLocalize(Consts.GetInstance().SHOP_0074_CURRENCY_TYPE_DEFAULT);
    shop0074Scroll1.ownLabel.SetTextLocalize(quantity < 0 ? "-" : quantity.ToString());
    ((IEnumerable<GameObject>) shop0074Scroll1.medalIconObject).ToggleOnceEx((int) shop0074Scroll1._menu.currencyType);
    if (shop0074Scroll1._menu.currencyType == ShopArticleListMenu.CurrencyType.SubCoin)
      shop0074Scroll1.medalIconObject[6].SetActive(false);
  }

  private GameObject createThumb(GameObject prefab, Vector3 pos)
  {
    GameObject thumb = Object.Instantiate<GameObject>(prefab, this.dirThum.transform);
    thumb.transform.localScale = Vector3.one;
    thumb.transform.localPosition = pos;
    thumb.transform.localRotation = Quaternion.identity;
    return thumb;
  }

  public void ArticleLimitValue(
    ShopArticle shopArticle,
    PlayerShopArticle playerArticle,
    long holding)
  {
    bool flag = shopArticle.limit.HasValue && playerArticle.limit.Value <= 0 || shopArticle.daily_limit.HasValue && playerArticle.limit.Value <= 0;
    this.IsSoldOut = flag;
    if (flag)
    {
      this.soldout.SetActive(true);
      this.shortage.SetActive(false);
      this.buy.SetActive(false);
    }
    else if (holding < (long) shopArticle.price)
    {
      this.soldout.SetActive(false);
      this.shortage.SetActive(true);
      this.buy.SetActive(false);
    }
    else
    {
      this.shortage.SetActive(false);
      this.soldout.SetActive(false);
      this.buy.SetActive(true);
    }
    if (this._playerShopArticle.article.end_at.HasValue && this._isUpdateEndTime)
      this.UpdateServerTime();
    else
      this.disabledTimer();
    if ((this._playerShopArticle.article.limit.HasValue || this._playerShopArticle.article.daily_limit.HasValue) && this._playerShopArticle.limit.Value > 0)
    {
      ((Component) this.quantityLabel).gameObject.SetActive(true);
      this.quantityLabel.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_0074_SCROLL_ARTICLE_LIMIT_VALUE, (object) this._playerShopArticle.limit.Value));
    }
    else
      ((Component) this.quantityLabel).gameObject.SetActive(false);
    this.priceLabel.SetTextLocalize(this._playerShopArticle.article.price.ToString());
  }

  private IEnumerator openPopup0076()
  {
    Shop0074Scroll shop0074Scroll = this;
    IEnumerator e;
    if (shop0074Scroll._playerShopArticle.article.pay_type == CommonPayType.common_ticket)
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/shop007_CoinExchange/popup_CoinExchange_Confirmation").Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject prefab = prefabF.Result.Clone();
      GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      shop0074Scroll.StartCoroutine(gameObject.GetComponent<PopupCoinExchangeConfirmation>().Init(shop0074Scroll._playerShopArticle, shop0074Scroll.holding, shop0074Scroll._onPurchased));
    }
    else
    {
      Player player = SMManager.Get<Player>();
      TowerPlayer towerPlayer = TowerUtil.TowerPlayer;
      PlayerAffiliation playerAffiliation = PlayerAffiliation.Current;
      Future<GameObject> prefab0076F = Res.Prefabs.popup.popup_007_6.Load<GameObject>();
      e = prefab0076F.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject prefab = prefab0076F.Result.Clone();
      GameObject gameObject = Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      int? quantityLimit = new int?();
      int playerQuantity;
      switch (shop0074Scroll._playerShopArticle.article.ShopContents[0].entity_type)
      {
        case MasterDataTable.CommonRewardType.unit:
        case MasterDataTable.CommonRewardType.material_unit:
          playerQuantity = shop0074Scroll._playerUnitQuantity;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.unitIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.supply:
        case MasterDataTable.CommonRewardType.gear:
        case MasterDataTable.CommonRewardType.material_gear:
        case MasterDataTable.CommonRewardType.gear_body:
          playerQuantity = shop0074Scroll._playerItemQuantity;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.itemIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.quest_key:
          quantityLimit = new int?(9999);
          PlayerQuestKey key = shop0074Scroll.key;
          playerQuantity = key != null ? key.quantity : 0;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.gacha_ticket:
          playerQuantity = shop0074Scroll.gTicket == null ? 0 : shop0074Scroll.gTicket.quantity;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.season_ticket:
          playerQuantity = shop0074Scroll.sTicket == null ? 0 : shop0074Scroll.sTicket.quantity;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.unit_ticket:
          playerQuantity = shop0074Scroll.playerUnitTicket == null ? 0 : shop0074Scroll.playerUnitTicket.quantity;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.awake_skill:
          // ISSUE: reference to a compiler-generated method
          playerQuantity = ((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).Count<PlayerAwakeSkill>(new Func<PlayerAwakeSkill, bool>(shop0074Scroll.\u003CopenPopup0076\u003Eb__49_0));
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.guild_facility:
          // ISSUE: reference to a compiler-generated method
          PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>(new Func<PlayerGuildFacility, bool>(shop0074Scroll.\u003CopenPopup0076\u003Eb__49_1));
          playerQuantity = playerGuildFacility != null ? playerGuildFacility.hasnum : 0;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
          playerQuantity = shop0074Scroll.uTypeTicket == null ? 0 : shop0074Scroll.uTypeTicket.quantity;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.challenge_point:
          playerQuantity = Singleton<NGGameDataManager>.GetInstance().challenge_point;
          quantityLimit = new int?(Singleton<NGGameDataManager>.GetInstance().challenge_point_max);
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        case MasterDataTable.CommonRewardType.recovery_item:
          playerQuantity = shop0074Scroll._playerRecoveryItemQuantity;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
        default:
          playerQuantity = 0;
          prefab.GetComponent<Shop0076Menu>().InitObject(shop0074Scroll.uniqueIconObj.gameObject);
          break;
      }
      shop0074Scroll.StartCoroutine(gameObject.GetComponent<Shop0076Menu>().Init(shop0074Scroll._playerShopArticle, shop0074Scroll._playerShopArticle.article, player, towerPlayer, playerAffiliation, playerQuantity, shop0074Scroll._menu.ScrollList, shop0074Scroll._shop.articles, shop0074Scroll._onPurchased, shop0074Scroll._onPurchasedHolding, quantityLimit));
    }
  }

  private bool AddForStack()
  {
    foreach (PlayerItem playerItem in SMManager.Get<PlayerItem[]>())
    {
      if (playerItem.entity_id == this._playerShopArticle.article.ShopContents[0].entity_id && !playerItem.ForBattle && playerItem.quantity < 99)
        return true;
    }
    foreach (PlayerMaterialGear playerMaterialGear in SMManager.Get<PlayerMaterialGear[]>())
    {
      if (playerMaterialGear.gear_id == this._playerShopArticle.article.ShopContents[0].entity_id && playerMaterialGear.ForBattle && playerMaterialGear.quantity < 99)
        return true;
    }
    return false;
  }

  private void ButtonAction()
  {
    Player player = SMManager.Get<Player>();
    if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.supply || this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.material_gear || this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.gear_body || this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.material_unit)
      this.StartCoroutine(this.openPopup0076());
    else if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.gear)
    {
      if (MasterData.GearGear.ContainsKey(this._playerShopArticle.article.ShopContents[0].entity_id) && MasterData.GearGear[this._playerShopArticle.article.ShopContents[0].entity_id].isMaterial())
        this.StartCoroutine(this.openPopup0076());
      else if (player.CheckMaxHavingGear())
        this.StartCoroutine(PopupUtility._999_6_1(true));
      else if (player.CheckMaxHavingReisou())
        this.StartCoroutine(PopupUtility.popupMaxReisou());
      else
        this.StartCoroutine(this.openPopup0076());
    }
    else if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.stamp)
      this.StartCoroutine(this.openPopup0076());
    else if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.unit)
    {
      if (MasterData.UnitUnit.ContainsKey(this._playerShopArticle.article.ShopContents[0].entity_id) && MasterData.UnitUnit[this._playerShopArticle.article.ShopContents[0].entity_id].IsMaterialUnit)
        this.StartCoroutine(this.openPopup0076());
      else if (player.CheckMaxHavingUnit())
        this.StartCoroutine(PopupUtility._999_5_1());
      else
        this.StartCoroutine(this.openPopup0076());
    }
    else if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.quest_key)
    {
      if (this.key.max_quantity <= (this.key == null ? 0 : this.key.quantity))
        this.StartCoroutine(PopupUtility._999_15(MasterData.QuestkeyQuestkey[this.key.quest_key_id].name));
      else
        this.StartCoroutine(this.openPopup0076());
    }
    else if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.season_ticket)
    {
      if (this.sTicket != null && this.sTicket.max_quantity <= this.sTicket.quantity)
        this.StartCoroutine(PopupUtility._999_15(MasterData.SeasonTicketSeasonTicket[this.sTicket.season_ticket_id].name));
      else
        this.StartCoroutine(this.openPopup0076());
    }
    else if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.guild_facility)
    {
      if (this.facility != null && this.facility.hasnum >= this._playerShopArticle.article.ShopContents[0].upper_limit_count)
        this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().POPUP_GUILD_SHOP_LIMIT_OVER_TITLE, Consts.GetInstance().POPUP_GUILD_SHOP_LIMIT_OVER_DESC));
      else
        this.StartCoroutine(this.openPopup0076());
    }
    else if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.reincarnation_type_ticket)
    {
      if (this.uTypeTicket != null && this._playerShopArticle.article.ShopContents[0].upper_limit_check && this.uTypeTicket.quantity >= this._playerShopArticle.article.ShopContents[0].upper_limit_count)
        this.StartCoroutine(PopupCommon.Show(Consts.GetInstance().POPUP_GUILD_SHOP_LIMIT_OVER_TITLE, Consts.GetInstance().POPUP_GUILD_SHOP_LIMIT_OVER_DESC));
      else
        this.StartCoroutine(this.openPopup0076());
    }
    else if (this._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.challenge_point)
    {
      NGGameDataManager instance = Singleton<NGGameDataManager>.GetInstance();
      if (instance.challenge_point >= instance.challenge_point_max)
        this.StartCoroutine(PopupUtility._999_15(this._playerShopArticle.article.name));
      else
        this.StartCoroutine(this.openPopup0076());
    }
    else
      this.StartCoroutine(this.openPopup0076());
  }

  public void onBuy()
  {
    if (this._menu.IsPushAndSet())
      return;
    this.ButtonAction();
  }

  public void onChange()
  {
    if (this._menu.IsPushAndSet())
      return;
    this.ButtonAction();
  }

  public void onDetail()
  {
    switch (this._playerShopArticle.article.ShopContents[0].entity_type)
    {
      case MasterDataTable.CommonRewardType.gacha_ticket:
        break;
      case MasterDataTable.CommonRewardType.unit_ticket:
        break;
      case MasterDataTable.CommonRewardType.stamp:
        break;
      case MasterDataTable.CommonRewardType.challenge_point:
        break;
      case MasterDataTable.CommonRewardType.recovery_item:
        break;
      default:
        if (this._menu.IsPushAndSet())
          break;
        this.StartCoroutine(this.setDetailPopup());
        break;
    }
  }

  private IEnumerator setDetailPopup()
  {
    Shop0074Scroll shop0074Scroll = this;
    if (shop0074Scroll._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.guild_town)
      shop0074Scroll.StartCoroutine(shop0074Scroll.setMapDetailPopup());
    else if (shop0074Scroll._playerShopArticle.article.ShopContents[0].entity_type == MasterDataTable.CommonRewardType.guild_facility)
    {
      shop0074Scroll.StartCoroutine(shop0074Scroll.setFacilityDetailPopup());
    }
    else
    {
      GameObject popup = Singleton<PopupManager>.GetInstance().open(shop0074Scroll._menu.DetailPopup);
      popup.SetActive(false);
      IEnumerator e = popup.GetComponent<Shop00742Menu>().Init(shop0074Scroll._playerShopArticle.article.ShopContents[0].entity_type, shop0074Scroll._playerShopArticle);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
    }
  }

  private IEnumerator setMapDetailPopup()
  {
    if (!Object.op_Equality((Object) this._menu.MapDetailPopup, (Object) null))
    {
      GameObject popup = Singleton<PopupManager>.GetInstance().open(this._menu.MapDetailPopup);
      popup.SetActive(false);
      IEnumerator e = popup.GetComponent<PopupMapDetailMenu>().InitializeAsync(this._playerShopArticle.article.ShopContents[0]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
    }
  }

  private IEnumerator setFacilityDetailPopup()
  {
    if (!Object.op_Equality((Object) this._menu.FacilityDetailPopup, (Object) null))
    {
      GameObject popup = Singleton<PopupManager>.GetInstance().open(this._menu.FacilityDetailPopup);
      popup.SetActive(false);
      IEnumerator e = popup.GetComponent<PopupFacilityDetailMenu>().InitializeAsync(this._playerShopArticle.article.ShopContents[0].entity_id);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
    }
  }

  private void UpdateServerTime()
  {
    this.timerBase.SetActive(true);
    TimeSpan self = this._playerShopArticle.end_at.Value - ServerTime.NowAppTimeAddDelta();
    this.timerLabel.SetTextLocalize(Consts.Format(Consts.GetInstance().SHOP_0074_SCROLL_UPDATE_SERVER_TIME, (IDictionary) new Hashtable()
    {
      {
        (object) "time",
        (object) self.DisplayString()
      }
    }));
    ((Component) this.timerLabel).gameObject.SetActive(true);
  }

  private void disabledTimer()
  {
    this.timerBase.SetActive(false);
    ((Component) this.timerLabel).gameObject.SetActive(false);
  }
}
