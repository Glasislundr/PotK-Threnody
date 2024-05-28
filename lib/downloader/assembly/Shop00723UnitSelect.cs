// Decompiled with JetBrains decompiler
// Type: Shop00723UnitSelect
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
public class Shop00723UnitSelect : MonoBehaviour
{
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private GameObject topIcon_;
  [SerializeField]
  private UIButton btnSkill_;
  [SerializeField]
  private UIButton btnSelect_;
  [SerializeField]
  private UILabel txtCount_;
  private Shop00723Menu menu_;
  public SelectTicketSelectSample sample_;
  private int limitCount;

  public IEnumerator coInitialize(
    Shop00723Menu menu,
    SelectTicketSelectSample unitSample,
    PlayerSelectTicketSummary playerUnitTicket,
    SM.SelectTicket unitTicket)
  {
    this.menu_ = menu;
    this.sample_ = unitSample;
    UnitUnit unitUnit = (UnitUnit) null;
    if (MasterData.UnitUnit.TryGetValue(this.sample_.reward_id, out unitUnit))
    {
      this.txtName_.SetTextLocalize(unitUnit.name);
    }
    else
    {
      this.txtName_.SetTextLocalize("");
      Debug.LogError((object) ("Key Not Found: " + (object) this.sample_.reward_id));
    }
    this.UpdateLimitCountLabel(playerUnitTicket, unitTicket);
    if (playerUnitTicket != null)
      this.SetButtonEnabled(playerUnitTicket.quantity, unitTicket.exchange_limit, this.limitCount);
    else
      this.SetButtonEnabled(0, unitTicket.exchange_limit, this.limitCount);
    UnitIcon ui = menu.prefabIconUnit.Clone(this.topIcon_.transform).GetComponent<UnitIcon>();
    UnitUnit unit = unitUnit;
    IEnumerator e = ui.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui.setLevelText(Consts.GetInstance().SHOP_00723_UNIT_LEVEL);
    ui.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    ((Collider) ui.buttonBoxCollider).enabled = false;
  }

  public void UpdateInfo(
    PlayerSelectTicketSummary playerUnitTicket,
    SelectTicketSelectSample exchangeUnitSample,
    int ticketQuantity,
    SM.SelectTicket unitTicket)
  {
    if (this.sample_ == exchangeUnitSample)
      this.UpdateLimitCountLabel(playerUnitTicket, unitTicket, true);
    if (unitTicket.exchange_limit)
      this.limitCount = this.UpdateLimitCount(playerUnitTicket, unitTicket, false, this.limitCount);
    this.SetButtonEnabled(ticketQuantity, unitTicket.exchange_limit, this.limitCount);
  }

  private void UpdateLimitCountLabel(
    PlayerSelectTicketSummary playerUnitTicket,
    SM.SelectTicket unitTicket,
    bool isInited = false)
  {
    bool isLimit = unitTicket.exchange_limit;
    if (isLimit)
    {
      this.limitCount = this.UpdateLimitCount(playerUnitTicket, unitTicket, isInited, this.limitCount);
      if (this.limitCount == int.MaxValue)
        isLimit = false;
    }
    this.SetLimitCountLabel(isLimit, this.limitCount);
  }

  private int UpdateLimitCount(
    PlayerSelectTicketSummary playerUnitTicket,
    SM.SelectTicket unitTicket,
    bool isInited,
    int count)
  {
    UnitUnit value = (UnitUnit) null;
    if (!MasterData.UnitUnit.TryGetValue(this.sample_.reward_id, out value))
      Debug.LogError((object) ("Key Not Found: " + (object) this.sample_.reward_id));
    SelectTicketChoices selectTicketChoices = ((IEnumerable<SelectTicketChoices>) unitTicket.choices).FirstOrDefault<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (u => u.reward_id == value.ID));
    if (selectTicketChoices == null)
      return 0;
    PlayerSelectTicketSummaryPlayer_exchange_count_list exchangeCountList = (PlayerSelectTicketSummaryPlayer_exchange_count_list) null;
    if (playerUnitTicket != null)
      exchangeCountList = ((IEnumerable<PlayerSelectTicketSummaryPlayer_exchange_count_list>) playerUnitTicket.player_exchange_count_list).FirstOrDefault<PlayerSelectTicketSummaryPlayer_exchange_count_list>((Func<PlayerSelectTicketSummaryPlayer_exchange_count_list, bool>) (u => u.reward_id == value.ID));
    return exchangeCountList == null || !selectTicketChoices.exchangeable_count.HasValue ? (!selectTicketChoices.exchangeable_count.HasValue ? int.MaxValue : selectTicketChoices.exchangeable_count.Value) : selectTicketChoices.exchangeable_count.Value - exchangeCountList.exchange_count;
  }

  private void SetLimitCountLabel(bool isLimit, int count)
  {
    if (isLimit)
      this.txtCount_.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_UNIT_SELECT_UNIT_COUNT, (object) count));
    else
      ((Component) this.txtCount_).gameObject.SetActive(false);
  }

  public void SubtractLimitCount(int nums)
  {
    this.limitCount -= nums;
    this.txtCount_.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_UNIT_SELECT_UNIT_COUNT, (object) this.limitCount));
  }

  private void SetButtonEnabled(int quantity, bool isLimit, int count = 0)
  {
    bool flag = true;
    if (isLimit)
      flag = count > 0;
    ((UIButtonColor) this.btnSelect_).isEnabled = flag && quantity > 0;
  }

  public void onClickSkill() => this.menu_.onClickSkill(this.sample_);

  public void onClickSelect() => this.menu_.onClickSelect(this.sample_);

  public SelectTicketSelectSample SampleUnit => this.sample_;
}
