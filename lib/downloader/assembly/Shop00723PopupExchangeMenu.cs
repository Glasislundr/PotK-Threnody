// Decompiled with JetBrains decompiler
// Type: Shop00723PopupExchangeMenu
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
public class Shop00723PopupExchangeMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject topIconUnit_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtDetail_;
  [SerializeField]
  private GameObject objTypeRandom_;
  [SerializeField]
  private UIPopupList popupType_;
  [SerializeField]
  private UILabel popupValue_;
  [SerializeField]
  private UIButton btnOk_;
  [SerializeField]
  private UIButton btnCancel_;
  private Shop00723Menu menu_;
  private SelectTicketSelectSample sample_;
  private bool isInitialized_;
  private bool isTypeSelectable_;
  private bool isOpenPopup_;
  private bool isOK_;
  private List<UIButton> pauseButtons_ = new List<UIButton>();

  public IEnumerator coInitialize(
    Shop00723Menu menu,
    SelectTicketSelectSample unitSample,
    PlayerSelectTicketSummary playerUnitTicket,
    SM.SelectTicket unitTicket)
  {
    this.isInitialized_ = false;
    this.menu_ = menu;
    this.sample_ = unitSample;
    UnitIcon ui = menu.prefabIconUnit.Clone(this.topIconUnit_.transform).GetComponent<UnitIcon>();
    UnitUnit unitUnit = (UnitUnit) null;
    if (!MasterData.UnitUnit.TryGetValue(this.sample_.reward_id, out unitUnit))
      Debug.LogError((object) ("Key Not Found: " + (object) this.sample_.reward_id));
    UnitUnit unit = unitUnit;
    IEnumerator e = ui.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui.setLevelText(Consts.GetInstance().SHOP_00723_UNIT_LEVEL);
    ui.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    ((Collider) ui.buttonBoxCollider).enabled = false;
    this.txtName_.SetTextLocalize(unit.name);
    this.txtDetail_.SetTextLocalize(string.Format(Consts.GetInstance().SHOP_00723_POPUP_EXCHANGE_DETAIL, (object) unitTicket.cost));
    this.isOK_ = playerUnitTicket.quantity >= unitTicket.cost;
    this.isTypeSelectable_ = unitTicket.unit_type_selectable;
    if (this.isTypeSelectable_)
    {
      SelectTicketChoices selectTicketChoices = ((IEnumerable<SelectTicketChoices>) unitTicket.choices).First<SelectTicketChoices>((Func<SelectTicketChoices, bool>) (u => u.reward_id == unit.ID));
      this.popupType_.items.Clear();
      int[] source = new int[1]{ 7 };
      foreach (int unitType in selectTicketChoices.unit_types)
      {
        if (!((IEnumerable<int>) source).Contains<int>(unitType))
          this.popupType_.items.Add(MasterData.UnitType[unitType].name);
      }
      this.popupType_.value = Consts.GetInstance().SHOP_00723_REQUIRED_UNIT_TYPE;
      ((Component) this.popupType_).gameObject.SetActive(true);
      this.popupValue_.SetTextLocalize(Consts.GetInstance().SHOP_00723_REQUIRED_UNIT_TYPE);
      this.objTypeRandom_.SetActive(false);
      ((UIButtonColor) this.btnOk_).isEnabled = false;
    }
    else
    {
      ((Component) this.popupType_).gameObject.SetActive(false);
      this.objTypeRandom_.SetActive(true);
      ((UIButtonColor) this.btnOk_).isEnabled = this.isOK_;
    }
    this.isInitialized_ = true;
  }

  public void onPopupValueChanged()
  {
    if (!this.isInitialized_)
      return;
    string text = this.popupType_.value;
    if (!this.popupType_.items.Contains(text))
      return;
    if (!this.popupType_.items.Contains(this.popupValue_.text))
      ((UIButtonColor) this.btnOk_).isEnabled = this.isOK_;
    this.popupValue_.SetTextLocalize(text);
  }

  public void onOpenPopup()
  {
    if (this.isOpenPopup_)
      return;
    if (!this.popupType_.items.Contains(this.popupType_.value))
      this.popupType_.value = this.popupType_.items[0];
    this.isOpenPopup_ = true;
    if (((UIButtonColor) this.btnOk_).isEnabled)
    {
      this.pauseButtons_.Add(this.btnOk_);
      ((UIButtonColor) this.btnOk_).isEnabled = false;
    }
    if (((UIButtonColor) this.btnCancel_).isEnabled)
    {
      this.pauseButtons_.Add(this.btnCancel_);
      ((UIButtonColor) this.btnCancel_).isEnabled = false;
    }
    this.StartCoroutine("waitPopupClose");
  }

  private IEnumerator waitPopupClose()
  {
    yield return (object) null;
    while (this.popupType_.isOpen)
      yield return (object) null;
    foreach (UIButtonColor pauseButton in this.pauseButtons_)
      pauseButton.isEnabled = true;
    this.pauseButtons_.Clear();
    this.isOpenPopup_ = false;
  }

  private void OnDestroy()
  {
    if (!this.isOpenPopup_)
      return;
    this.isOpenPopup_ = false;
    this.StopCoroutine("waitPopupClose");
  }

  public override void onBackButton() => this.onClickCancel();

  public void onClickOk()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.menu_.doExchangeUnit(this.isTypeSelectable_ ? ((IEnumerable<MasterDataTable.UnitType>) MasterData.UnitTypeList).First<MasterDataTable.UnitType>((Func<MasterDataTable.UnitType, bool>) (ut => ut.name.Equals(this.popupValue_.text))).Enum : UnitTypeEnum.random);
  }

  public void onClickCancel()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.menu_.onClosedSelect();
  }
}
