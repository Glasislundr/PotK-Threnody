// Decompiled with JetBrains decompiler
// Type: Unit004ReincarnationTypeTicketPopupExchangeMenu
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
public class Unit004ReincarnationTypeTicketPopupExchangeMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject topIconUnit_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UIPopupList popupType_;
  [SerializeField]
  private UILabel popupValue_;
  [SerializeField]
  private UIButton btnOk_;
  [SerializeField]
  private UIButton btnCancel_;
  [SerializeField]
  private UILabel labelSelectType;
  [SerializeField]
  private UILabel labelRandom;
  private UnitTypeTicket ticket_;
  private PlayerUnit playerUnit_;
  private bool isInitialized_;
  private bool isOpenPopup_;
  private List<UIButton> pauseButtons_ = new List<UIButton>();
  private UnitTypeEnum selectType_;
  private UnitTypeEnum[] unitTypes_;
  private Unit004ReincarnationTypeUnitSelectionMenu menu_;
  private Action<UnitTypeEnum, bool> onSelectPopup;

  public IEnumerator coInitialize(
    UnitTypeTicket ticket,
    PlayerUnit playerUnit,
    Unit004ReincarnationTypeUnitSelectionMenu menu,
    Action<UnitTypeEnum, bool> onSelect = null)
  {
    Unit004ReincarnationTypeTicketPopupExchangeMenu popupExchangeMenu = this;
    popupExchangeMenu.isInitialized_ = false;
    popupExchangeMenu.onSelectPopup = onSelect;
    popupExchangeMenu.ticket_ = ticket;
    popupExchangeMenu.playerUnit_ = playerUnit;
    popupExchangeMenu.unitTypes_ = playerUnit.unit.validUnitTypes;
    popupExchangeMenu.menu_ = menu;
    Future<GameObject> ldPrefab = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = ldPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon ui = ldPrefab.Result.Clone(popupExchangeMenu.topIconUnit_.transform).GetComponent<UnitIcon>();
    UnitUnit unit = playerUnit.unit;
    e = ui.SetPlayerUnit(playerUnit, new PlayerUnit[0], (PlayerUnit) null, false, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui.setLevelText(playerUnit);
    ui.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    ui.princessType.DispPrincessType(true);
    ((Collider) ui.buttonBoxCollider).enabled = false;
    popupExchangeMenu.txtName_.SetTextLocalize(unit.name);
    popupExchangeMenu.popupType_.items.Clear();
    if (ticket.unit_type_random_flag)
    {
      popupExchangeMenu.selectType_ = UnitTypeEnum.random;
      ((Component) popupExchangeMenu.popupType_).gameObject.SetActive(false);
    }
    else
    {
      foreach (MasterDataTable.UnitType unitType in MasterData.UnitTypeList)
      {
        if (((IEnumerable<UnitTypeEnum>) popupExchangeMenu.unitTypes_).Contains<UnitTypeEnum>(unitType.Enum))
          popupExchangeMenu.popupType_.items.Add(unitType.name);
      }
      popupExchangeMenu.popupType_.value = popupExchangeMenu.popupType_.items[0];
      ((Component) popupExchangeMenu.popupType_).gameObject.SetActive(true);
      popupExchangeMenu.popupValue_.SetTextLocalize(popupExchangeMenu.popupType_.value);
      // ISSUE: reference to a compiler-generated method
      popupExchangeMenu.selectType_ = ((IEnumerable<MasterDataTable.UnitType>) MasterData.UnitTypeList).FirstOrDefault<MasterDataTable.UnitType>(new Func<MasterDataTable.UnitType, bool>(popupExchangeMenu.\u003CcoInitialize\u003Eb__17_0)).Enum;
    }
    ((Component) popupExchangeMenu.labelSelectType).gameObject.SetActive(!ticket.unit_type_random_flag);
    ((Component) popupExchangeMenu.labelRandom).gameObject.SetActive(ticket.unit_type_random_flag);
    popupExchangeMenu.setBtnOkEnable();
    popupExchangeMenu.isInitialized_ = true;
  }

  private void setBtnOkEnable()
  {
    bool flag = true;
    if (flag && this.selectType_ == this.playerUnit_.unit_type.Enum)
      flag = false;
    ((UIButtonColor) this.btnOk_).isEnabled = flag;
  }

  public void onPopupValueChanged()
  {
    if (!this.isInitialized_)
      return;
    string curtxt = this.popupType_.value;
    if (!this.popupType_.items.Contains(curtxt))
      return;
    this.selectType_ = ((IEnumerable<MasterDataTable.UnitType>) MasterData.UnitTypeList).FirstOrDefault<MasterDataTable.UnitType>((Func<MasterDataTable.UnitType, bool>) (x => x.name == curtxt)).Enum;
    this.setBtnOkEnable();
    this.popupValue_.SetTextLocalize(curtxt);
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
    this.setBtnOkEnable();
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
    if (this.onSelectPopup != null)
    {
      this.onSelectPopup(this.selectType_, true);
      this.close();
    }
    else
      this.close();
  }

  public void onClickCancel()
  {
    if (this.IsPushAndSet())
      return;
    if (this.onSelectPopup != null)
      this.onSelectPopup(this.selectType_, false);
    this.close();
  }

  public void close()
  {
    this.menu_.isPopupOpen = false;
    Singleton<PopupManager>.GetInstance().dismiss();
  }
}
