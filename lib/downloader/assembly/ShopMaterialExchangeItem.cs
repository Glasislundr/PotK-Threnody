// Decompiled with JetBrains decompiler
// Type: ShopMaterialExchangeItem
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
public class ShopMaterialExchangeItem : MonoBehaviour
{
  [SerializeField]
  private UIButton btnSelect_;
  [SerializeField]
  private UIButton btnInfo_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtInfo_;
  [SerializeField]
  private UILabel txtCount_;
  [SerializeField]
  private GameObject txtCountSprite;
  [SerializeField]
  private UILabel materialNum;
  [SerializeField]
  private GameObject topIcon_;
  [SerializeField]
  private UIButton glassBtn_;
  [SerializeField]
  private BoxCollider glassCollider_;
  [SerializeField]
  private UILabel lastExchangeCount;
  private ShopMaterialExchangeListMenu menu_;
  public SelectTicketSelectSample sample_;
  public int limitCount;
  public bool exchange_limit;
  private int currNums;

  public IEnumerator coInitialize(
    ShopMaterialExchangeListMenu menu,
    SelectTicketSelectSample sample,
    PlayerSelectTicketSummary playerUnitTicket,
    SM.SelectTicket unitTicket)
  {
    ShopMaterialExchangeItem materialExchangeItem = this;
    materialExchangeItem.menu_ = menu;
    materialExchangeItem.sample_ = sample;
    materialExchangeItem.exchange_limit = unitTicket.exchange_limit;
    materialExchangeItem.txtName_.SetTextLocalize(materialExchangeItem.sample_.reward_title);
    materialExchangeItem.txtInfo_.SetTextLocalize(materialExchangeItem.sample_.reward_info);
    if (unitTicket.exchange_limit)
    {
      SelectTicketChoices selectTicketChoices = ((IEnumerable<SelectTicketChoices>) unitTicket.choices).FirstOrDefault<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (u => u.reward_id == this.sample_.reward_id));
      materialExchangeItem.limitCount = selectTicketChoices == null || !selectTicketChoices.exchangeable_count.HasValue ? int.MaxValue : selectTicketChoices.exchangeable_count.Value;
      PlayerSelectTicketSummaryPlayer_exchange_count_list exchangeCountList = (PlayerSelectTicketSummaryPlayer_exchange_count_list) null;
      if (playerUnitTicket != null && playerUnitTicket.player_exchange_count_list != null)
        exchangeCountList = ((IEnumerable<PlayerSelectTicketSummaryPlayer_exchange_count_list>) playerUnitTicket.player_exchange_count_list).FirstOrDefault<PlayerSelectTicketSummaryPlayer_exchange_count_list>((Func<PlayerSelectTicketSummaryPlayer_exchange_count_list, bool>) (u => u.reward_id == this.sample_.reward_id));
      if (exchangeCountList != null && selectTicketChoices.exchangeable_count.HasValue)
        materialExchangeItem.limitCount = selectTicketChoices.exchangeable_count.Value - exchangeCountList.exchange_count;
    }
    materialExchangeItem.SetButtonEnabled(playerUnitTicket != null ? playerUnitTicket.quantity : 0, unitTicket.exchange_limit, materialExchangeItem.limitCount);
    ((Component) materialExchangeItem.lastExchangeCount).gameObject.SetActive(unitTicket.exchange_limit);
    if (unitTicket.exchange_limit)
    {
      string unitSelectUnitCount = Consts.GetInstance().SHOP_00723_UNIT_SELECT_UNIT_COUNT;
      materialExchangeItem.lastExchangeCount.SetTextLocalize(string.Format(unitSelectUnitCount, (object) materialExchangeItem.limitCount));
    }
    IEnumerator coroutine = materialExchangeItem.menu_.SetIcon(materialExchangeItem.sample_, materialExchangeItem.topIcon_.transform);
    yield return (object) materialExchangeItem.StartCoroutine(coroutine);
    materialExchangeItem.currNums = (int) coroutine.Current;
    bool flag = materialExchangeItem.sample_.entity_type != MasterDataTable.CommonRewardType.deck;
    ((Component) materialExchangeItem.txtCount_).gameObject.SetActive(flag);
    materialExchangeItem.txtCountSprite.gameObject.SetActive(flag);
    materialExchangeItem.txtCount_.SetTextLocalize(materialExchangeItem.currNums);
    materialExchangeItem.materialNum.SetTextLocalize(materialExchangeItem.sample_.reward_value);
    materialExchangeItem.btnSelect_.onClick.Clear();
    materialExchangeItem.btnSelect_.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.menu_.onClickSelect(this.sample_))));
    if (materialExchangeItem.sample_.entity_type == MasterDataTable.CommonRewardType.awake_skill)
    {
      ((Collider) materialExchangeItem.glassCollider_).enabled = true;
      materialExchangeItem.glassBtn_.onClick.Clear();
      materialExchangeItem.glassBtn_.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.menu_.onClickSkillDetail(this.sample_))));
    }
    else if (materialExchangeItem.sample_.entity_type == MasterDataTable.CommonRewardType.unit_ticket || materialExchangeItem.sample_.entity_type == MasterDataTable.CommonRewardType.gacha_ticket || materialExchangeItem.sample_.entity_type == MasterDataTable.CommonRewardType.common_ticket || materialExchangeItem.sample_.entity_type == MasterDataTable.CommonRewardType.season_ticket || materialExchangeItem.sample_.entity_type == MasterDataTable.CommonRewardType.reincarnation_type_ticket)
    {
      ((Collider) materialExchangeItem.glassCollider_).enabled = true;
      materialExchangeItem.glassBtn_.onClick.Clear();
      materialExchangeItem.glassBtn_.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => this.menu_.ShowTicketDetail(this.sample_.entity_type, this.sample_.reward_id))));
    }
    else
      ((Collider) materialExchangeItem.glassCollider_).enabled = false;
    if (materialExchangeItem.sample_.entity_type == MasterDataTable.CommonRewardType.deck)
    {
      ((Component) materialExchangeItem.btnInfo_).gameObject.SetActive(true);
      materialExchangeItem.btnInfo_.onClick.Clear();
      materialExchangeItem.btnInfo_.onClick.Add(new EventDelegate((EventDelegate.Callback) (() => menu.onBtnInfo(this.sample_))));
    }
    else
      ((Component) materialExchangeItem.btnInfo_).gameObject.SetActive(false);
  }

  public void UpdateInfo(
    PlayerSelectTicketSummary playerUnitTicket,
    SelectTicketSelectSample exchangeUnitSample,
    int ticketQuantity,
    SM.SelectTicket unitTicket)
  {
    bool exchangeLimit = unitTicket.exchange_limit;
    if (unitTicket.exchange_limit)
    {
      SM.SelectTicket[] source = SMManager.Get<SM.SelectTicket[]>();
      if (playerUnitTicket != null)
        unitTicket = ((IEnumerable<SM.SelectTicket>) source).FirstOrDefault<SM.SelectTicket>((Func<SM.SelectTicket, bool>) (t => t.id == playerUnitTicket.ticket_id));
      SelectTicketChoices selectTicketChoices = (SelectTicketChoices) null;
      if (unitTicket != null)
        selectTicketChoices = ((IEnumerable<SelectTicketChoices>) unitTicket.choices).FirstOrDefault<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (u => u.reward_id == this.sample_.reward_id));
      this.limitCount = selectTicketChoices == null || !selectTicketChoices.exchangeable_count.HasValue ? int.MaxValue : selectTicketChoices.exchangeable_count.Value;
      PlayerSelectTicketSummaryPlayer_exchange_count_list exchangeCountList = (PlayerSelectTicketSummaryPlayer_exchange_count_list) null;
      playerUnitTicket = ((IEnumerable<PlayerSelectTicketSummary>) SMManager.Get<PlayerSelectTicketSummary[]>()).FirstOrDefault<PlayerSelectTicketSummary>((Func<PlayerSelectTicketSummary, bool>) (x => x.ticket_id == this.sample_.ticketID));
      if (playerUnitTicket != null)
        exchangeCountList = ((IEnumerable<PlayerSelectTicketSummaryPlayer_exchange_count_list>) playerUnitTicket.player_exchange_count_list).FirstOrDefault<PlayerSelectTicketSummaryPlayer_exchange_count_list>((Func<PlayerSelectTicketSummaryPlayer_exchange_count_list, bool>) (u => u.reward_id == this.sample_.reward_id));
      if (exchangeCountList != null && selectTicketChoices.exchangeable_count.HasValue)
        this.limitCount = selectTicketChoices.exchangeable_count.Value - exchangeCountList.exchange_count;
    }
    this.SetButtonEnabled(ticketQuantity, exchangeLimit, this.limitCount);
    ((Component) this.lastExchangeCount).gameObject.SetActive(exchangeLimit);
    if (!exchangeLimit)
      return;
    this.lastExchangeCount.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_UNIT_SELECT_UNIT_COUNT, (object) this.limitCount));
  }

  private void SetButtonEnabled(int quantity, bool isLimit, int count = 0)
  {
    bool flag = true;
    if (isLimit)
      flag = count > 0;
    ((UIButtonColor) this.btnSelect_).isEnabled = flag && quantity > 0;
  }

  public void UpdateNums(int nums)
  {
    this.txtCount_.SetTextLocalize(nums);
    this.lastExchangeCount.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_UNIT_SELECT_UNIT_COUNT, (object) this.limitCount));
  }

  public void SubtractLimitCount(int nums)
  {
    this.limitCount -= nums;
    this.lastExchangeCount.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_UNIT_SELECT_UNIT_COUNT, (object) this.limitCount));
  }
}
