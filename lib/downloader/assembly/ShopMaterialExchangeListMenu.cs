// Decompiled with JetBrains decompiler
// Type: ShopMaterialExchangeListMenu
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
public class ShopMaterialExchangeListMenu : BackButtonMenuBase
{
  private bool isInitialized_;
  public readonly int QUANTITY_DISPLAY_MAX = 999;
  [SerializeField]
  private UILabel txtTitle_;
  [SerializeField]
  private UILabel txtExpirationDate_;
  [SerializeField]
  private UILabel txtQuantity_;
  [SerializeField]
  private UILabel txtQuantityDesabled_;
  [SerializeField]
  private UILabel txtCost_;
  [SerializeField]
  private NGxScroll scroll_;
  private List<ShopMaterialExchangeItem> selectlist = new List<ShopMaterialExchangeItem>();
  private PlayerSelectTicketSummary playerMaterialTicket_;
  private SM.SelectTicket materialTicket_;
  private List<SelectTicketSelectSample> materialExchange_ = new List<SelectTicketSelectSample>();
  private SelectTicketSelectSample currentMaterialExchange__;
  private GameObject prefabMaterial_;
  private GameObject prefabExchange_;
  private GameObject prefabMaterialPack;
  private GameObject prefab0078;
  private GameObject prefabSkillDetail_;
  private int quantity;
  private GameObject detailPopup;
  private GameObject iconObj;
  public int quantity_;

  public GameObject prefabIconMaterial { get; private set; }

  public GameObject unitIconPrefab { get; private set; }

  public GameObject uniqueIconPrefab { get; private set; }

  public IEnumerator coInitialize(
    ShopMaterialExchangeListScene scene,
    SM.SelectTicket materialTicket,
    PlayerSelectTicketSummary playerMaterialTicket)
  {
    ShopMaterialExchangeListMenu exchangeListMenu1 = this;
    exchangeListMenu1.playerMaterialTicket_ = playerMaterialTicket;
    exchangeListMenu1.materialTicket_ = materialTicket;
    exchangeListMenu1.txtTitle_.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_TITLE_NAME, (object) exchangeListMenu1.materialTicket_.name));
    exchangeListMenu1.txtExpirationDate_.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_EXPIRATION_DATE, (object) exchangeListMenu1.materialTicket_.end_at));
    exchangeListMenu1.quantity_ = exchangeListMenu1.playerMaterialTicket_.quantity;
    exchangeListMenu1.txtCost_.SetTextLocalize(exchangeListMenu1.materialTicket_.cost);
    exchangeListMenu1.materialExchange_.Clear();
    foreach (KeyValuePair<int, SelectTicketSelectSample> keyValuePair in MasterData.SelectTicketSelectSample)
    {
      if (keyValuePair.Value.ticketID == exchangeListMenu1.materialTicket_.id && (keyValuePair.Value.entity_type == MasterDataTable.CommonRewardType.deck || !keyValuePair.Value.deckID.HasValue))
        exchangeListMenu1.materialExchange_.Add(keyValuePair.Value);
    }
    if (materialTicket.exchange_limit)
    {
      bool flag = true;
      List<SelectTicketSelectSample> ticketSelectSampleList = new List<SelectTicketSelectSample>();
      ShopMaterialExchangeListMenu exchangeListMenu = exchangeListMenu1;
      for (int i = exchangeListMenu1.materialExchange_.Count - 1; i >= 0; i--)
      {
        SelectTicketChoices selectTicketChoices = ((IEnumerable<SelectTicketChoices>) materialTicket.choices).FirstOrDefault<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (u => u.reward_id == exchangeListMenu.materialExchange_[i].reward_id));
        int num = selectTicketChoices == null || !selectTicketChoices.exchangeable_count.HasValue ? int.MaxValue : selectTicketChoices.exchangeable_count.Value;
        PlayerSelectTicketSummaryPlayer_exchange_count_list exchangeCountList = ((IEnumerable<PlayerSelectTicketSummaryPlayer_exchange_count_list>) playerMaterialTicket.player_exchange_count_list).FirstOrDefault<PlayerSelectTicketSummaryPlayer_exchange_count_list>((Func<PlayerSelectTicketSummaryPlayer_exchange_count_list, bool>) (u => u.reward_id == exchangeListMenu.materialExchange_[i].reward_id));
        if (exchangeCountList != null && selectTicketChoices.exchangeable_count.HasValue)
          num = selectTicketChoices.exchangeable_count.Value - exchangeCountList.exchange_count;
        if (selectTicketChoices != null)
        {
          if (selectTicketChoices.exchangeable_count.HasValue && num == 0)
          {
            SelectTicketSelectSample ticketSelectSample = exchangeListMenu1.materialExchange_[i];
            exchangeListMenu1.materialExchange_.RemoveAt(i);
            ticketSelectSampleList.Add(ticketSelectSample);
          }
          else
            flag = false;
        }
      }
      if (!flag)
      {
        ticketSelectSampleList.Reverse();
        for (int index = 0; index < ticketSelectSampleList.Count; ++index)
          exchangeListMenu1.materialExchange_.Add(ticketSelectSampleList[index]);
      }
      else
      {
        foreach (KeyValuePair<int, SelectTicketSelectSample> keyValuePair in MasterData.SelectTicketSelectSample)
        {
          if (keyValuePair.Value.ticketID == exchangeListMenu1.materialTicket_.id && (keyValuePair.Value.entity_type == MasterDataTable.CommonRewardType.deck || !keyValuePair.Value.deckID.HasValue))
            exchangeListMenu1.materialExchange_.Add(keyValuePair.Value);
        }
      }
    }
    bool iswait;
    Future<GameObject> ldPrefab;
    IEnumerator e;
    if (Object.op_Equality((Object) exchangeListMenu1.prefabMaterial_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.ShopMaterialExchangeListMenu.dir_material_exchange_list.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      exchangeListMenu1.prefabMaterial_ = ldPrefab.Result;
      if (Object.op_Equality((Object) exchangeListMenu1.prefabMaterial_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) exchangeListMenu1.prefabIconMaterial, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      exchangeListMenu1.prefabIconMaterial = ldPrefab.Result;
      if (Object.op_Equality((Object) exchangeListMenu1.prefabIconMaterial, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) exchangeListMenu1.unitIconPrefab, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      exchangeListMenu1.unitIconPrefab = ldPrefab.Result;
      if (Object.op_Equality((Object) exchangeListMenu1.unitIconPrefab, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) exchangeListMenu1.uniqueIconPrefab, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Icons.UniqueIconPrefab.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      exchangeListMenu1.uniqueIconPrefab = ldPrefab.Result;
      if (Object.op_Equality((Object) exchangeListMenu1.uniqueIconPrefab, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) exchangeListMenu1.prefabExchange_, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.popup.popup_material_exchange_confirmation__anim_popup01.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      exchangeListMenu1.prefabExchange_ = ldPrefab.Result;
      if (Object.op_Equality((Object) exchangeListMenu1.prefabExchange_, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) exchangeListMenu1.prefabMaterialPack, (Object) null))
    {
      iswait = true;
      ldPrefab = Res.Prefabs.popup.popup_material_exchange_pack_menu__anim_popup01.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
      {
        iswait = false;
        yield return e.Current;
      }
      e = (IEnumerator) null;
      exchangeListMenu1.prefabMaterialPack = ldPrefab.Result;
      if (Object.op_Equality((Object) exchangeListMenu1.prefabMaterialPack, (Object) null))
      {
        yield break;
      }
      else
      {
        if (iswait)
          yield return (object) null;
        ldPrefab = (Future<GameObject>) null;
      }
    }
    if (Object.op_Equality((Object) exchangeListMenu1.prefabSkillDetail_, (Object) null))
    {
      ldPrefab = PopupSkillDetails.createPrefabLoader(false);
      e = ldPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      exchangeListMenu1.prefabSkillDetail_ = ldPrefab.Result;
      ldPrefab = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) exchangeListMenu1.detailPopup, (Object) null))
    {
      ldPrefab = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
      e = ldPrefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      exchangeListMenu1.detailPopup = ldPrefab.Result;
      ldPrefab = (Future<GameObject>) null;
    }
    e = exchangeListMenu1.coInitializeMaterialSelect();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exchangeListMenu1.updateTicketQuantity();
    exchangeListMenu1.isInitialized_ = true;
  }

  private IEnumerator coInitializeMaterialSelect()
  {
    ShopMaterialExchangeListMenu menu = this;
    if (menu.materialExchange_ == null || menu.materialExchange_.Count == 0)
    {
      Debug.LogError((object) "materialExchange is null or empty");
    }
    else
    {
      menu.scroll_.Clear();
      menu.selectlist.Clear();
      foreach (SelectTicketSelectSample sample in menu.materialExchange_)
      {
        ShopMaterialExchangeItem component = menu.prefabMaterial_.Clone().GetComponent<ShopMaterialExchangeItem>();
        menu.scroll_.Add(((Component) component).gameObject, true);
        menu.selectlist.Add(component);
        IEnumerator e = component.coInitialize(menu, sample, menu.playerMaterialTicket_, menu.materialTicket_);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        menu.scroll_.ResolvePosition();
      }
    }
  }

  private void updateMaterialNum(int useCount)
  {
    if (this.materialExchange_ == null || this.materialExchange_.Count == 0)
    {
      Debug.LogError((object) "materialExchange is null or empty");
    }
    else
    {
      for (int index = 0; index < this.selectlist.Count; ++index)
      {
        if (this.selectlist[index].sample_.ID == this.currentMaterialExchange__.ID)
          this.selectlist[index].UpdateNums(this.GetItemCurrNum(this.selectlist[index].sample_));
      }
    }
  }

  private void subtractLimitCount(int useCount)
  {
    for (int index = 0; index < this.selectlist.Count; ++index)
    {
      if (this.selectlist[index].sample_.ID == this.currentMaterialExchange__.ID)
      {
        this.selectlist[index].SubtractLimitCount(useCount);
        break;
      }
    }
  }

  private void updateTicketQuantity(bool usedTicket = false, int useCount = 1)
  {
    if (usedTicket)
      this.quantity_ = this.playerMaterialTicket_ == null ? 0 : this.playerMaterialTicket_.quantity;
    int num1 = this.quantity_ >= this.materialTicket_.cost ? 1 : 0;
    int num2 = Mathf.Clamp(this.quantity_, 0, this.QUANTITY_DISPLAY_MAX);
    if (num1 != 0)
    {
      this.txtQuantity_.SetTextLocalize(num2);
      ((Component) this.txtQuantity_).gameObject.SetActive(true);
      ((Component) this.txtQuantityDesabled_).gameObject.SetActive(false);
    }
    else
    {
      ((Component) this.txtQuantity_).gameObject.SetActive(false);
      this.txtQuantityDesabled_.SetTextLocalize(num2);
      ((Component) this.txtQuantityDesabled_).gameObject.SetActive(true);
    }
  }

  public void onClickSelect(SelectTicketSelectSample material)
  {
    if (!this.isInitialized_ || this.IsPushAndSet())
      return;
    this.currentMaterialExchange__ = material;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.coPopupSelect());
  }

  public void onClickSkillDetail(SelectTicketSelectSample material)
  {
    PopupSkillDetails.show(this.prefabSkillDetail_, PopupSkillDetails.Param.createByShopSkillView(new PlayerAwakeSkill()
    {
      skill_id = material.reward_id
    }));
  }

  private IEnumerator coPopupSelect()
  {
    ShopMaterialExchangeListMenu menu = this;
    GameObject popup = Singleton<PopupManager>.GetInstance().open(menu.prefabExchange_.Clone(), isCloned: true);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<MaterialPopupExchangeMenu>().coInitialize(menu, menu.currentMaterialExchange__, menu.playerMaterialTicket_, menu.materialTicket_);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public IEnumerator SetIcon(
    SelectTicketSelectSample sample_,
    Transform tran,
    bool isDisplayCounter = false)
  {
    IEnumerator e;
    ItemIcon material;
    switch (sample_.entity_type)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        UnitIcon icon = this.unitIconPrefab.Clone(tran).GetComponent<UnitIcon>();
        UnitUnit unit = (UnitUnit) null;
        if (MasterData.UnitUnit.TryGetValue(sample_.reward_id, out unit))
        {
          e = icon.SetUnit(unit, unit.GetElement(), false);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (isDisplayCounter)
            icon.SetCounter(sample_.reward_value);
        }
        else
          ((Component) icon).GetComponent<UnitIcon>().SetEmpty();
        ((Component) icon).gameObject.SetActive(true);
        icon.onClick = (Action<UnitIconBase>) null;
        icon.onClick = (Action<UnitIconBase>) (x => this.ShowDetail(sample_.entity_type, sample_.reward_id));
        break;
      case MasterDataTable.CommonRewardType.supply:
        material = this.prefabIconMaterial.Clone(tran).GetComponent<ItemIcon>();
        e = material.InitByMaterialExchange(sample_.entity_type, sample_.reward_id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ((Component) material).gameObject.SetActive(true);
        material.onClick = (Action<ItemIcon>) null;
        material.onClick = (Action<ItemIcon>) (x => this.ShowDetail(sample_.entity_type, sample_.reward_id));
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        material = this.prefabIconMaterial.Clone(tran).GetComponent<ItemIcon>();
        e = material.InitByMaterialExchange(sample_.entity_type, sample_.reward_id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        ((Component) material).gameObject.SetActive(true);
        material.onClick = (Action<ItemIcon>) null;
        material.onClick = (Action<ItemIcon>) (x => this.ShowDetail(sample_.entity_type, sample_.reward_id));
        break;
      default:
        UniqueIcons component = this.uniqueIconPrefab.Clone(tran).GetComponent<UniqueIcons>();
        component.LabelActivated = false;
        ((Component) component).gameObject.SetActive(true);
        component.CreateButton();
        component.onClick = (Action<UniqueIcons>) null;
        component.onClick = (Action<UniqueIcons>) (x => this.ShowDetail(sample_.entity_type, sample_.reward_id));
        if (isDisplayCounter && (sample_.entity_type == MasterDataTable.CommonRewardType.unit_ticket || sample_.entity_type == MasterDataTable.CommonRewardType.gacha_ticket || sample_.entity_type == MasterDataTable.CommonRewardType.common_ticket || sample_.entity_type == MasterDataTable.CommonRewardType.season_ticket || sample_.entity_type == MasterDataTable.CommonRewardType.reincarnation_type_ticket))
        {
          component.onClick = (Action<UniqueIcons>) null;
          component.onClick = (Action<UniqueIcons>) (x => this.ShowTicketDetail(sample_.entity_type, sample_.reward_id));
        }
        switch (sample_.entity_type)
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
            e = component.SetKey(sample_.reward_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.gacha_ticket:
            e = component.SetGachaTicket(id: sample_.reward_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.season_ticket:
            e = component.SetSeasonTicket(id: sample_.reward_id);
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
            e = component.SetKillersTicket(sample_.reward_id);
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
            component.onClick = (Action<UniqueIcons>) null;
            component.onClick = (Action<UniqueIcons>) (x => this.onClickSkillDetail(sample_));
            this.quantity = ((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).Count<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.skill_id == sample_.reward_id));
            e = component.SetAwakeSkill(sample_.reward_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.guild_town:
            e = component.SetGuildMap(sample_.reward_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.guild_facility:
            int id = 0;
            PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x.master.ID == sample_.reward_id));
            if (playerGuildFacility != null)
            {
              id = playerGuildFacility.unit.ID;
            }
            else
            {
              FacilityLevel facilityLevel = ((IEnumerable<FacilityLevel>) MasterData.FacilityLevelList).FirstOrDefault<FacilityLevel>((Func<FacilityLevel, bool>) (x => x.level == 1 && x.facility_MapFacility == sample_.reward_id));
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
            e = component.SetReincarnationTypeTicket(sample_.reward_id);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case MasterDataTable.CommonRewardType.deck:
            e = component.SetMaterialPack(sample_.ticketID);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
        }
        break;
    }
    yield return (object) this.GetItemCurrNum(sample_);
  }

  public int GetItemCurrNum(SelectTicketSelectSample sample)
  {
    int num1 = 0;
    int num2 = 0;
    int num3 = num1 + SMManager.Get<PlayerItem[]>().AmountHavingTargetItem(sample.reward_id, sample.entity_type) + SMManager.Get<PlayerMaterialGear[]>().AmountHavingTargetItem(sample.reward_id);
    if (MasterData.UnitUnit.ContainsKey(sample.reward_id))
    {
      if (MasterData.UnitUnit[sample.reward_id].IsMaterialUnit)
      {
        PlayerMaterialUnit playerMaterialUnit = ((IEnumerable<PlayerMaterialUnit>) SMManager.Get<PlayerMaterialUnit[]>()).FirstOrDefault<PlayerMaterialUnit>((Func<PlayerMaterialUnit, bool>) (x => x._unit == sample.reward_id));
        if (playerMaterialUnit != null)
          num2 = playerMaterialUnit.quantity;
      }
      else
        num2 = SMManager.Get<PlayerUnit[]>().AmountHavingTargetUnit(sample.reward_id, sample.entity_type);
    }
    PlayerQuestKey playerQuestKey = sample.entity_type == MasterDataTable.CommonRewardType.quest_key ? ((IEnumerable<PlayerQuestKey>) SMManager.Get<PlayerQuestKey[]>()).Where<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x => x.quest_key_id == sample.reward_id)).FirstOrDefault<PlayerQuestKey>() : (PlayerQuestKey) null;
    int itemCurrNum;
    switch (sample.entity_type)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        itemCurrNum = num2;
        break;
      case MasterDataTable.CommonRewardType.supply:
        itemCurrNum = num3;
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
      case MasterDataTable.CommonRewardType.gear_body:
        itemCurrNum = num3;
        break;
      default:
        itemCurrNum = 0;
        switch (sample.entity_type)
        {
          case MasterDataTable.CommonRewardType.quest_key:
            itemCurrNum = playerQuestKey == null ? 0 : playerQuestKey.quantity;
            break;
          case MasterDataTable.CommonRewardType.gacha_ticket:
            PlayerGachaTicket playerGachaTicket = ((IEnumerable<PlayerGachaTicket>) SMManager.Get<Player>().gacha_tickets).FirstOrDefault<PlayerGachaTicket>((Func<PlayerGachaTicket, bool>) (x => x.ticket.ID == sample.reward_id));
            itemCurrNum = playerGachaTicket == null ? 0 : playerGachaTicket.quantity;
            break;
          case MasterDataTable.CommonRewardType.season_ticket:
            PlayerSeasonTicket playerSeasonTicket = ((IEnumerable<PlayerSeasonTicket>) SMManager.Get<PlayerSeasonTicket[]>()).FirstOrDefault<PlayerSeasonTicket>((Func<PlayerSeasonTicket, bool>) (x => x.season_ticket_id == sample.reward_id));
            itemCurrNum = playerSeasonTicket == null ? 0 : playerSeasonTicket.quantity;
            break;
          case MasterDataTable.CommonRewardType.unit_ticket:
            PlayerSelectTicketSummary selectTicketSummary = ((IEnumerable<PlayerSelectTicketSummary>) SMManager.Get<PlayerSelectTicketSummary[]>()).FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == sample.reward_id));
            itemCurrNum = ((IEnumerable<SM.SelectTicket>) SMManager.Get<SM.SelectTicket[]>()).FirstOrDefault<SM.SelectTicket>((Func<SM.SelectTicket, bool>) (x => x.id == sample.reward_id)) == null ? 0 : selectTicketSummary.quantity;
            break;
          case MasterDataTable.CommonRewardType.awake_skill:
            itemCurrNum = ((IEnumerable<PlayerAwakeSkill>) SMManager.Get<PlayerAwakeSkill[]>()).Count<PlayerAwakeSkill>((Func<PlayerAwakeSkill, bool>) (x => x.skill_id == sample.reward_id));
            break;
          case MasterDataTable.CommonRewardType.guild_facility:
            PlayerGuildFacility playerGuildFacility = ((IEnumerable<PlayerGuildFacility>) SMManager.Get<PlayerGuildFacility[]>()).FirstOrDefault<PlayerGuildFacility>((Func<PlayerGuildFacility, bool>) (x => x.master.ID == sample.reward_id));
            itemCurrNum = playerGuildFacility == null ? 0 : playerGuildFacility.hasnum;
            break;
          case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
            PlayerUnitTypeTicket playerUnitTypeTicket = ((IEnumerable<PlayerUnitTypeTicket>) SMManager.Get<PlayerUnitTypeTicket[]>()).FirstOrDefault<PlayerUnitTypeTicket>((Func<PlayerUnitTypeTicket, bool>) (x => x.ticket_id == sample.reward_id));
            itemCurrNum = playerUnitTypeTicket == null ? 0 : playerUnitTypeTicket.quantity;
            break;
        }
        break;
    }
    return itemCurrNum;
  }

  private IEnumerator ShowDetailPopup(MasterDataTable.CommonRewardType rType, int rID)
  {
    GameObject popup = Singleton<PopupManager>.GetInstance().open(this.detailPopup);
    popup.SetActive(false);
    IEnumerator e = popup.GetComponent<Shop00742Menu>().Init(rType, rID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    popup.SetActive(true);
  }

  public void ShowDetail(MasterDataTable.CommonRewardType rType, int rID)
  {
    if (!Shop00742Menu.IsEnableShowPopup(rType))
      return;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.ShowDetailPopup(rType, rID));
  }

  public void ShowTicketDetail(MasterDataTable.CommonRewardType rType, int rID)
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.ShowDetailPopup(rType, rID));
  }

  public void InitObj(GameObject icon)
  {
    this.iconObj = Object.Instantiate<GameObject>(icon);
    this.iconObj.transform.position = Vector2.op_Implicit(new Vector2(999f, 999f));
    this.iconObj.SetActive(false);
  }

  public void doExchangeMaterial(int nums, int useCount)
  {
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.coExchangeMaterial(nums, this.iconObj, useCount));
  }

  private IEnumerator coExchangeMaterial(int nums, GameObject icon, int useCount)
  {
    ShopMaterialExchangeListMenu exchangeListMenu1 = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Future<WebAPI.Response.SelectticketSpend> future = WebAPI.SelectticketSpend(exchangeListMenu1.currentMaterialExchange__.ID, 0, useCount, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<PopupManager>.GetInstance().closeAll();
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
      // ISSUE: reference to a compiler-generated method
      exchangeListMenu1.playerMaterialTicket_ = ((IEnumerable<PlayerSelectTicketSummary>) SMManager.Get<PlayerSelectTicketSummary[]>()).FirstOrDefault<PlayerSelectTicketSummary>(new Func<PlayerSelectTicketSummary, bool>(exchangeListMenu1.\u003CcoExchangeMaterial\u003Eb__49_1));
      exchangeListMenu1.updateTicketQuantity(true, useCount);
      exchangeListMenu1.materialExchange_.Clear();
      foreach (KeyValuePair<int, SelectTicketSelectSample> keyValuePair in MasterData.SelectTicketSelectSample)
      {
        if (keyValuePair.Value.ticketID == exchangeListMenu1.materialTicket_.id && (keyValuePair.Value.entity_type == MasterDataTable.CommonRewardType.deck || !keyValuePair.Value.deckID.HasValue))
          exchangeListMenu1.materialExchange_.Add(keyValuePair.Value);
      }
      if (exchangeListMenu1.materialTicket_.exchange_limit)
      {
        bool flag = true;
        List<SelectTicketSelectSample> ticketSelectSampleList = new List<SelectTicketSelectSample>();
        ShopMaterialExchangeListMenu exchangeListMenu = exchangeListMenu1;
        for (int i = exchangeListMenu1.materialExchange_.Count - 1; i >= 0; i--)
        {
          SelectTicketChoices selectTicketChoices = ((IEnumerable<SelectTicketChoices>) exchangeListMenu1.materialTicket_.choices).FirstOrDefault<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (u => u.reward_id == exchangeListMenu.materialExchange_[i].reward_id));
          int num = selectTicketChoices == null || !selectTicketChoices.exchangeable_count.HasValue ? int.MaxValue : selectTicketChoices.exchangeable_count.Value;
          PlayerSelectTicketSummaryPlayer_exchange_count_list exchangeCountList = (PlayerSelectTicketSummaryPlayer_exchange_count_list) null;
          if (exchangeListMenu1.playerMaterialTicket_ != null && exchangeListMenu1.playerMaterialTicket_.player_exchange_count_list != null)
            exchangeCountList = ((IEnumerable<PlayerSelectTicketSummaryPlayer_exchange_count_list>) exchangeListMenu1.playerMaterialTicket_.player_exchange_count_list).FirstOrDefault<PlayerSelectTicketSummaryPlayer_exchange_count_list>((Func<PlayerSelectTicketSummaryPlayer_exchange_count_list, bool>) (u => u.reward_id == exchangeListMenu.materialExchange_[i].reward_id));
          if (exchangeCountList != null && selectTicketChoices.exchangeable_count.HasValue)
            num = selectTicketChoices.exchangeable_count.Value - exchangeCountList.exchange_count;
          if (selectTicketChoices != null)
          {
            if (selectTicketChoices.exchangeable_count.HasValue && num == 0)
            {
              SelectTicketSelectSample ticketSelectSample = exchangeListMenu1.materialExchange_[i];
              exchangeListMenu1.materialExchange_.RemoveAt(i);
              ticketSelectSampleList.Add(ticketSelectSample);
            }
            else
              flag = false;
          }
        }
        if (!flag)
        {
          ticketSelectSampleList.Reverse();
          for (int index = 0; index < ticketSelectSampleList.Count; ++index)
            exchangeListMenu1.materialExchange_.Add(ticketSelectSampleList[index]);
        }
        else
        {
          foreach (KeyValuePair<int, SelectTicketSelectSample> keyValuePair in MasterData.SelectTicketSelectSample)
          {
            if (keyValuePair.Value.ticketID == exchangeListMenu1.materialTicket_.id && (keyValuePair.Value.entity_type == MasterDataTable.CommonRewardType.deck || !keyValuePair.Value.deckID.HasValue))
              exchangeListMenu1.materialExchange_.Add(keyValuePair.Value);
          }
        }
        if (exchangeListMenu1.quantity_ >= exchangeListMenu1.materialTicket_.cost)
        {
          e1 = exchangeListMenu1.coInitializeMaterialSelect();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
        }
      }
      if (exchangeListMenu1.quantity_ >= exchangeListMenu1.materialTicket_.cost)
        exchangeListMenu1.UpdateTicketLimit();
      else
        exchangeListMenu1.subtractLimitCount(useCount);
      if (exchangeListMenu1.currentMaterialExchange__.entity_type == MasterDataTable.CommonRewardType.deck)
      {
        // ISSUE: reference to a compiler-generated method
        ModalWindow.Show(Consts.GetInstance().VERSUS_0026872POPUP_TITLE2, string.Format(Consts.GetInstance().VERSUS_0026872POPUP_DESCRIPTION3, (object) exchangeListMenu1.currentMaterialExchange__.reward_title), new Action(exchangeListMenu1.\u003CcoExchangeMaterial\u003Eb__49_2));
      }
      else
        Singleton<PopupManager>.GetInstance().monitorCoroutine(exchangeListMenu1.OpenPopup0078(nums, icon, useCount));
      ShopTicketExchangeMenu.IsUpdate = true;
    }
  }

  private IEnumerator OpenPopup0078(int nums, GameObject icon, int useCount)
  {
    ShopMaterialExchangeListMenu exchangeListMenu = this;
    Future<GameObject> prefab0078F = Res.Prefabs.popup.popup_007_8__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab0078F.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    exchangeListMenu.prefab0078 = prefab0078F.Result.Clone();
    Shop0078Menu component = exchangeListMenu.prefab0078.GetComponent<Shop0078Menu>();
    icon.SetActive(true);
    exchangeListMenu.updateMaterialNum(useCount);
    component.InitObj(icon);
    // ISSUE: reference to a compiler-generated method
    component.InitDataSet(exchangeListMenu.currentMaterialExchange__.reward_title, exchangeListMenu.currentMaterialExchange__.reward_value, nums, new Action(exchangeListMenu.\u003COpenPopup0078\u003Eb__50_0), true, useCount);
    Singleton<PopupManager>.GetInstance().open(exchangeListMenu.prefab0078, isCloned: true);
    if (Object.op_Inequality((Object) exchangeListMenu.iconObj, (Object) null))
      Object.DestroyImmediate((Object) exchangeListMenu.iconObj);
  }

  private void UpdateTicketLimit()
  {
    foreach (ShopMaterialExchangeItem materialExchangeItem in this.selectlist)
      materialExchangeItem.UpdateInfo(this.playerMaterialTicket_, this.currentMaterialExchange__, this.quantity_, this.materialTicket_);
  }

  public void onBtnInfo(SelectTicketSelectSample sample)
  {
    if (!this.isInitialized_ || this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().monitorCoroutine(this.materialPopupInit(sample));
  }

  private IEnumerator materialPopupInit(SelectTicketSelectSample sample)
  {
    GameObject go = this.prefabMaterialPack.Clone();
    ((UIRect) Singleton<PopupManager>.GetInstance().open(go, isCloned: true, isNonSe: true, isNonOpenAnime: true).GetComponent<UIWidget>()).alpha = 0.0f;
    IEnumerator e = go.GetComponent<Shop007_MaterialExchangePackMenu>().InitMaterialIcon(sample);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(go);
  }

  public override void onBackButton()
  {
    PopupManager instance = Singleton<PopupManager>.GetInstance();
    if (instance.isOpen || instance.isRunningCoroutine)
      return;
    this.OnIbtnBack();
  }

  public void OnIbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    if (Object.op_Inequality((Object) this.iconObj, (Object) null))
      Object.DestroyImmediate((Object) this.iconObj);
    this.backScene();
  }
}
