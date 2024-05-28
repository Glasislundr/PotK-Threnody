// Decompiled with JetBrains decompiler
// Type: UnitSelectMenuBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class UnitSelectMenuBase : UnitMenuBase
{
  [SerializeField]
  [Tooltip("販売用画面")]
  protected bool isForSale;
  [SerializeField]
  protected UnitSelectMenuBase.IconSelMode iconSelMode;
  [SerializeField]
  private int SELECT_MAX = 10;
  protected List<UnitIconInfo> selectedUnitIcons = new List<UnitIconInfo>();
  [SerializeField]
  protected bool isSelectNumPack = true;
  [SerializeField]
  private bool IsDisableFavoriteSelect;
  private List<int> overKillersEquipedUnitIDList;
  private List<int> cacheDeckPlayerUnitIdList;

  public int SelectMax
  {
    set => this.SELECT_MAX = value;
    get => this.SELECT_MAX;
  }

  public List<UnitIconInfo> SelectedUnitIcons => this.selectedUnitIcons;

  private List<int> CacheDeckPlayerUnitIdList
  {
    get
    {
      if (this.cacheDeckPlayerUnitIdList != null)
        return this.cacheDeckPlayerUnitIdList;
      this.cacheDeckPlayerUnitIdList = new List<int>();
      foreach (PlayerDeck playerDeck in SMManager.Get<PlayerDeck[]>())
      {
        if (playerDeck != null)
        {
          foreach (int? nullable in ((IEnumerable<int?>) playerDeck.player_unit_ids).Where<int?>((Func<int?, bool>) (x => x.HasValue)))
            this.cacheDeckPlayerUnitIdList.Add(nullable.Value);
        }
      }
      foreach (PlayerSeaDeck playerSeaDeck in SMManager.Get<PlayerSeaDeck[]>())
      {
        if (playerSeaDeck != null)
        {
          foreach (int? nullable in ((IEnumerable<int?>) playerSeaDeck.player_unit_ids).Where<int?>((Func<int?, bool>) (x => x.HasValue)))
            this.cacheDeckPlayerUnitIdList.Add(nullable.Value);
        }
      }
      foreach (ExploreDeck exploreDeck in SMManager.Get<ExploreDeck[]>())
      {
        if (exploreDeck != null)
        {
          foreach (int? nullable in ((IEnumerable<int?>) exploreDeck.player_unit_ids).Where<int?>((Func<int?, bool>) (x => x.HasValue)))
            this.cacheDeckPlayerUnitIdList.Add(nullable.Value);
        }
      }
      return this.cacheDeckPlayerUnitIdList;
    }
  }

  public override void IbtnClearS()
  {
    if (this.IsPush)
      return;
    foreach (UnitIconBase allUnitIcon in this.allUnitIcons)
    {
      if (Object.op_Inequality((Object) allUnitIcon, (Object) null) && allUnitIcon.PlayerUnit != (PlayerUnit) null)
      {
        UnitIconInfo unitInfoAll = this.GetUnitInfoAll(allUnitIcon.PlayerUnit);
        bool flag = unitInfoAll != null && unitInfoAll.button_enable;
        this.Deselect(allUnitIcon);
        if (((Behaviour) allUnitIcon.Button).enabled && flag)
          allUnitIcon.Gray = false;
      }
    }
    this.allUnitInfos.ForEach((Action<UnitIconInfo>) (v => v.select = -1));
    this.displayUnitInfos.ForEach((Action<UnitIconInfo>) (v => v.select = -1));
    this.selectedUnitIcons.Clear();
    this.UpdateInfomation();
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public virtual void UpdateInfomation()
  {
  }

  protected virtual bool IsSelectEx(UnitIconBase u) => false;

  protected virtual void SelectEx(UnitIconBase u, UnitIconInfo ui)
  {
  }

  public virtual bool SelectedUnitIsMax() => this.selectedUnitIcons.Count >= this.SelectMax;

  protected virtual void UnitInfoUpdate(UnitIconInfo info, bool enable, int cnt)
  {
    info.gray = enable;
    info.select = cnt;
  }

  public virtual void UpdateSelectIcon()
  {
    foreach (UnitIconInfo displayUnitInfo in this.displayUnitInfos)
    {
      if (Object.op_Inequality((Object) displayUnitInfo.icon, (Object) null) && ((Behaviour) displayUnitInfo.icon.Button).enabled)
      {
        if (this.SelectedUnitIsMax())
          displayUnitInfo.icon.Gray = !displayUnitInfo.gray;
        else if (displayUnitInfo.select == -1)
          displayUnitInfo.icon.Gray = displayUnitInfo.gray;
        else
          displayUnitInfo.icon.Gray = true;
      }
    }
    this.selectedUnitIcons.ForEachIndex<UnitIconInfo>((Action<UnitIconInfo, int>) ((u, n) =>
    {
      UnitIconInfo unitInfoDisplay = this.GetUnitInfoDisplay(u.playerUnit);
      if (unitInfoDisplay == null || !Object.op_Inequality((Object) unitInfoDisplay.icon, (Object) null))
        return;
      switch (this.iconSelMode)
      {
        case UnitSelectMenuBase.IconSelMode.Number:
          unitInfoDisplay.icon.Select(unitInfoDisplay.select);
          break;
        case UnitSelectMenuBase.IconSelMode.Check:
          if (unitInfoDisplay.icon.unit.IsNormalUnit)
          {
            unitInfoDisplay.icon.SelectByCheckIcon();
            break;
          }
          unitInfoDisplay.icon.HideCheckIcon();
          break;
        case UnitSelectMenuBase.IconSelMode.CheckAndTextNumber:
          unitInfoDisplay.icon.SelectByCheckAndNumber(unitInfoDisplay);
          break;
        default:
          Debug.LogError((object) ("想定していないIconSelMode: " + (object) this.iconSelMode));
          break;
      }
    }));
    this.UpdateInfomation();
  }

  protected void ReflectionSelectUnit()
  {
    this.selectedUnitIcons = this.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (v => v.select)).ToList<UnitIconInfo>();
    foreach (UnitIconInfo selectedUnitIcon in this.selectedUnitIcons)
    {
      UnitIconInfo unitInfoAll = this.GetUnitInfoAll(selectedUnitIcon.playerUnit);
      if (unitInfoAll != null)
      {
        unitInfoAll.SelectedCount = selectedUnitIcon.SelectedCount;
        unitInfoAll.select = selectedUnitIcon.select;
      }
    }
    this.selectedUnitIcons.Clear();
  }

  protected void ConsiderFavoriteUnit()
  {
    foreach (UnitIconInfo unitIconInfo in this.allUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => !v.playerUnit.favorite && v.select == -1)))
    {
      unitIconInfo.gray = false;
      unitIconInfo.button_enable = true;
    }
  }

  protected void IgnoreFavoriteUnit()
  {
    foreach (UnitIconInfo unitIconInfo in this.allUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => v.playerUnit.favorite)))
    {
      unitIconInfo.select = -1;
      unitIconInfo.gray = true;
      unitIconInfo.button_enable = false;
    }
  }

  protected void IgnoreOverkillers()
  {
    foreach (UnitIconInfo unitIconInfo in this.allUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit.unit.IsNormalUnit && !OverkillersUtil.checkDelete(x.playerUnit))))
    {
      unitIconInfo.select = -1;
      unitIconInfo.gray = true;
      unitIconInfo.is_overkillers = true;
      unitIconInfo.button_enable = false;
    }
  }

  protected void IgnoreDeckUnit(PlayerDeck[] playerDeck)
  {
    Action<PlayerUnit[]> action = (Action<PlayerUnit[]>) (units =>
    {
      foreach (PlayerUnit unit1 in units)
      {
        PlayerUnit unit = unit1;
        if (!(unit == (PlayerUnit) null))
        {
          UnitIconInfo unitIconInfo = this.allUnitInfos.FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit != (PlayerUnit) null && !x.removeButton && x.isNormalUnit && unit.id == x.playerUnit.id));
          if (unitIconInfo != null)
          {
            unitIconInfo.select = -1;
            unitIconInfo.gray = true;
            unitIconInfo.button_enable = false;
          }
        }
      }
    });
    foreach (PlayerDeck playerDeck1 in playerDeck)
    {
      if (playerDeck1 != null)
        action(playerDeck1.player_units);
    }
    PlayerSeaDeck[] playerSeaDeckArray = SMManager.Get<PlayerSeaDeck[]>();
    if (playerSeaDeckArray != null)
    {
      foreach (PlayerSeaDeck playerSeaDeck in playerSeaDeckArray)
      {
        if (playerSeaDeck != null)
          action(playerSeaDeck.player_units);
      }
    }
    ExploreDeck[] exploreDeckArray = SMManager.Get<ExploreDeck[]>();
    if (exploreDeckArray == null)
      return;
    foreach (ExploreDeck exploreDeck in exploreDeckArray)
    {
      if (exploreDeck != null)
        action(exploreDeck.player_units);
    }
  }

  protected bool IsExclusionUnitForLumpToutaMaterial(PlayerUnit playerUnit)
  {
    if (playerUnit.favorite || playerUnit.unit.rarity.index > 4 || playerUnit.buildup_count > 0 || playerUnit.level > 1 || (double) playerUnit.trust_rate > 0.0 || playerUnit.tower_is_entry || playerUnit.corps_is_entry || playerUnit.breakthrough_count > 0 || SMManager.Get<PlayerRentalPlayerUnitIds>() != null && SMManager.Get<PlayerRentalPlayerUnitIds>().rental_player_unit_ids != null && ((IEnumerable<int?>) SMManager.Get<PlayerRentalPlayerUnitIds>().rental_player_unit_ids).Contains<int?>(new int?(playerUnit.id)) || this.CacheDeckPlayerUnitIdList.Contains(playerUnit.id))
      return true;
    if (this.overKillersEquipedUnitIDList == null)
      this.overKillersEquipedUnitIDList = OverkillersUtil.createEquipedUnitIDList();
    return this.overKillersEquipedUnitIDList.Contains(playerUnit.id);
  }

  protected void CreateSelectUnitList()
  {
    this.selectedUnitIcons.Clear();
    this.selectedUnitIcons = this.allUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.select >= 0)).OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).ToList<UnitIconInfo>();
    this.selectedUnitIcons.ForEachIndex<UnitIconInfo>((Action<UnitIconInfo, int>) ((u, n) =>
    {
      u.select = n;
      this.UnitInfoUpdate(u, true, n);
    }));
  }

  public virtual void InitializeAllUnitInfosExtend(PlayerDeck[] playerDeck)
  {
    this.ReflectionSelectUnit();
    this.ConsiderFavoriteUnit();
    this.IgnoreFavoriteUnit();
    this.IgnoreOverkillers();
    this.IgnoreDeckUnit(playerDeck);
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      if (SMManager.Get<PlayerRentalPlayerUnitIds>() != null && allUnitInfo != null && SMManager.Get<PlayerRentalPlayerUnitIds>().rental_player_unit_ids != null && ((IEnumerable<int?>) SMManager.Get<PlayerRentalPlayerUnitIds>().rental_player_unit_ids).Contains<int?>(new int?(allUnitInfo.playerUnit.id)))
      {
        allUnitInfo.select = -1;
        allUnitInfo.gray = true;
        allUnitInfo.button_enable = false;
      }
    }
    this.CreateSelectUnitList();
  }

  protected virtual void Deselect(UnitIconBase unitIcon)
  {
    if (!unitIcon.Selected)
      return;
    unitIcon.Deselect();
    UnitIconInfo unitInfoAll1 = this.GetUnitInfoAll(unitIcon.PlayerUnit);
    if (unitInfoAll1 != null)
    {
      this.UnitInfoUpdate(unitInfoAll1, false, -1);
      this.selectedUnitIcons.Remove(unitInfoAll1);
    }
    UnitIconInfo unitInfoDisplay1 = this.GetUnitInfoDisplay(unitIcon.PlayerUnit);
    if (unitInfoDisplay1 != null)
      this.UnitInfoUpdate(unitInfoDisplay1, false, -1);
    if (!this.isSelectNumPack)
      return;
    this.selectedUnitIcons = this.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (v => v.select)).ToList<UnitIconInfo>();
    this.selectedUnitIcons.ForEachIndex<UnitIconInfo>((Action<UnitIconInfo, int>) ((u, n) =>
    {
      UnitIconInfo unitInfoAll2 = this.GetUnitInfoAll(u.playerUnit);
      if (unitInfoAll2 != null)
      {
        unitInfoAll2.select = n;
        this.UnitInfoUpdate(unitInfoAll2, true, n);
      }
      UnitIconInfo unitInfoDisplay2 = this.GetUnitInfoDisplay(u.playerUnit);
      if (unitInfoDisplay2 == null)
        return;
      unitInfoDisplay2.select = n;
    }));
  }

  protected int GetMinSelectIndex(int min = 0)
  {
    this.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).ForEach<UnitIconInfo>((Action<UnitIconInfo>) (x =>
    {
      if (min < x.select)
        return;
      min = x.select + 1;
    }));
    if (min > this.SelectMax)
      min = this.SelectMax;
    return min;
  }

  protected virtual void Select(UnitIconBase unitIcon)
  {
    UnitIconInfo unitInfoAll = this.GetUnitInfoAll(unitIcon.PlayerUnit);
    if (!unitInfoAll.button_enable)
      return;
    if (this.IsSelectEx(unitIcon))
      this.SelectEx(unitIcon, unitInfoAll);
    else if (unitIcon.Selected)
      this.UnSelect(unitIcon);
    else if (!this.SelectedUnitIsMax())
      this.OnSelect(unitIcon);
    this.UpdateInfomation();
  }

  public virtual void OnSelect(UnitIconBase unitIcon)
  {
    switch (this.iconSelMode)
    {
      case UnitSelectMenuBase.IconSelMode.Number:
        if (this.isSelectNumPack)
          unitIcon.Select(this.selectedUnitIcons.Count);
        else
          unitIcon.Select(this.GetMinSelectIndex());
        this.UnitInfoUpdates(unitIcon);
        break;
      case UnitSelectMenuBase.IconSelMode.Check:
        unitIcon.SelectByCheckIcon();
        this.UnitInfoUpdates(unitIcon);
        break;
      case UnitSelectMenuBase.IconSelMode.CheckAndTextNumber:
        this.UnitInfoUpdates(unitIcon);
        unitIcon.SelectByCheckAndNumber(this.GetUnitInfoAll(unitIcon.PlayerUnit));
        break;
      default:
        Debug.LogError((object) ("想定していないIconSelMode: " + (object) this.iconSelMode));
        break;
    }
    if (!this.SelectedUnitIsMax())
      return;
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      if (Object.op_Inequality((Object) allUnitInfo.icon, (Object) null) && ((Behaviour) allUnitInfo.icon.Button).enabled)
      {
        if (!allUnitInfo.button_enable)
          allUnitInfo.icon.Gray = allUnitInfo.gray;
        else
          allUnitInfo.icon.Gray = !allUnitInfo.gray;
      }
    }
  }

  protected void UnitInfoUpdates(UnitIconBase unitIcon)
  {
    UnitIconInfo unitInfoAll = this.GetUnitInfoAll(unitIcon.PlayerUnit);
    if (unitInfoAll != null)
    {
      this.UnitInfoUpdate(unitInfoAll, true, unitIcon.SelIndex);
      if (!this.selectedUnitIcons.Contains(unitInfoAll))
        this.selectedUnitIcons.Add(unitInfoAll);
    }
    UnitIconInfo unitInfoDisplay = this.GetUnitInfoDisplay(unitIcon.PlayerUnit);
    if (unitInfoDisplay == null)
      return;
    this.UnitInfoUpdate(unitInfoDisplay, true, unitIcon.SelIndex);
  }

  public void UnSelect(UnitIconBase unitIcon)
  {
    this.Deselect(unitIcon);
    this.UpdateSelectIcon();
  }

  protected override void ResetUnitIcon(int index)
  {
    base.ResetUnitIcon(index);
    this.Deselect(this.allUnitIcons[index]);
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    UnitSelectMenuBase unitSelectMenuBase = this;
    UnitIconBase unitIcon = unitSelectMenuBase.allUnitIcons[unit_index];
    unitSelectMenuBase.displayUnitInfos.Where<UnitIconInfo>((Func<UnitIconInfo, bool>) (a => Object.op_Equality((Object) a.icon, (Object) unitIcon))).ForEach<UnitIconInfo>((Action<UnitIconInfo>) (b => b.icon = (UnitIconBase) null));
    unitIcon.SetCounter(unitSelectMenuBase.displayUnitInfos[info_index].count);
    unitSelectMenuBase.displayUnitInfos[info_index].icon = unitIcon;
    IEnumerator e;
    if (unitSelectMenuBase.iconType == UnitMenuBase.IconType.Normal || unitSelectMenuBase.iconType == UnitMenuBase.IconType.EarthNormal || unitSelectMenuBase.iconType == UnitMenuBase.IconType.NormalWithHpGauge)
    {
      if (unitSelectMenuBase.displayUnitInfos[info_index].removeButton)
      {
        unitIcon.PlayerUnit = (PlayerUnit) null;
        unitIcon.SetRemoveButton();
      }
      else if (unitSelectMenuBase.displayUnitInfos[info_index].playerUnit.unit.IsMaterialUnit)
      {
        e = unitIcon.SetMaterialUnit(unitSelectMenuBase.displayUnitInfos[info_index].playerUnit, false, unitSelectMenuBase.getUnits());
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        e = unitIcon.SetPlayerUnit(unitSelectMenuBase.displayUnitInfos[info_index].playerUnit, unitSelectMenuBase.getUnits(), baseUnit, unitSelectMenuBase.isMaterial);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (unitSelectMenuBase.isForSale)
          unitSelectMenuBase.setLongPressEventForSale(unitSelectMenuBase.displayUnitInfos[info_index].icon, unitSelectMenuBase.getUnits());
      }
      ((UnitIcon) unitIcon).setBottom(unitSelectMenuBase.displayUnitInfos[info_index].playerUnit);
    }
    else if (unitSelectMenuBase.displayUnitInfos[info_index].playerUnit.unit.IsMaterialUnit)
    {
      e = unitIcon.SetMaterialUnit(unitSelectMenuBase.displayUnitInfos[info_index].playerUnit, baseUnit, false, unitSelectMenuBase.getUnits(), unitSelectMenuBase.displayUnitInfos[info_index].isTrustMaterial);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = unitIcon.SetPlayerUnit(unitSelectMenuBase.displayUnitInfos[info_index].playerUnit, unitSelectMenuBase.getUnits(), baseUnit, unitSelectMenuBase.isMaterial);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    unitIcon.Overkillers = unitSelectMenuBase.displayUnitInfos[info_index].is_overkillers;
    unitIcon.ForBattle = unitSelectMenuBase.displayUnitInfos[info_index].for_battle;
    unitIcon.TowerEntry = unitSelectMenuBase.displayUnitInfos[info_index].is_tower_entry;
    unitIcon.UnitRental = !Singleton<NGGameDataManager>.GetInstance().IsEarth && SMManager.Get<PlayerRentalPlayerUnitIds>() != null && unitSelectMenuBase.displayUnitInfos[info_index].is_rental;
    if (unitSelectMenuBase.displayUnitInfos[info_index].unit != null)
      unitIcon.CanAwake = unitSelectMenuBase.displayUnitInfos[info_index].unit.CanAwakeUnitFlag;
    unitIcon.UnitUsed = unitSelectMenuBase.displayUnitInfos[info_index].is_used;
    unitIcon.SetupDeckStatusBlink();
    unitIcon.Equip = unitSelectMenuBase.displayUnitInfos[info_index].equip;
    if (unitIcon is UnitIcon)
    {
      UnitIcon unitIcon1 = (UnitIcon) unitIcon;
      unitIcon1.EnabledExpireDate = unitSelectMenuBase.enabledExpireDate;
      unitIcon1.princessType.DispPrincessType(unitSelectMenuBase.displayUnitInfos[info_index].pricessType);
      unitIcon1.SpecialIconType = unitSelectMenuBase.displayUnitInfos[info_index].specialIconType;
      unitIcon1.SpecialIcon = unitSelectMenuBase.displayUnitInfos[info_index].isSpecialIcon;
    }
    else if (unitIcon is UnitDetailIcon)
    {
      UnitDetailIcon unitDetailIcon = (UnitDetailIcon) unitIcon;
      unitDetailIcon.UnitIcon.EnabledExpireDate = unitSelectMenuBase.enabledExpireDate;
      unitDetailIcon.UnitIcon.princessType.DispPrincessType(unitSelectMenuBase.displayUnitInfos[info_index].pricessType);
      unitDetailIcon.UnitIcon.ShowBottomInfos(unitSelectMenuBase.sortType);
    }
    unitIcon.SetCounter(unitSelectMenuBase.displayUnitInfos[info_index].count);
    unitIcon.SetSelectionCounter(unitSelectMenuBase.displayUnitInfos[info_index].SelectedCount);
    unitIcon.SelectMarker = unitSelectMenuBase.displayUnitInfos[info_index].selectMarker;
    if (unitSelectMenuBase.displayUnitInfos[info_index].alignmentSequence == 0)
    {
      if (unitSelectMenuBase.displayUnitInfos[info_index].select == -1)
      {
        unitIcon.Deselect();
      }
      else
      {
        switch (unitSelectMenuBase.iconSelMode)
        {
          case UnitSelectMenuBase.IconSelMode.Number:
            unitIcon.Select(unitSelectMenuBase.displayUnitInfos[info_index].select);
            break;
          case UnitSelectMenuBase.IconSelMode.Check:
            unitIcon.SelectByCheckIcon();
            break;
          case UnitSelectMenuBase.IconSelMode.CheckAndTextNumber:
            unitIcon.SelectByCheckAndNumber(unitSelectMenuBase.displayUnitInfos[info_index]);
            break;
          default:
            Debug.LogError((object) ("想定していないIconSelMode: " + (object) unitSelectMenuBase.iconSelMode));
            break;
        }
      }
    }
    if (!unitSelectMenuBase.displayUnitInfos[info_index].removeButton)
      unitIcon.ShowBottomInfo(unitSelectMenuBase.sortType);
    if (!((Component) unitIcon).gameObject.activeSelf)
      ((Component) unitIcon).gameObject.SetActive(true);
    unitSelectMenuBase.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index, baseUnit);
    if (this.isForSale)
      this.setLongPressEventForSale(this.displayUnitInfos[info_index].icon, this.getUnits());
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected void setLongPressEventForSale(UnitIconBase icon, PlayerUnit[] units)
  {
    icon.onLongPress = (Action<UnitIconBase>) (x => Unit0042Scene.changeSceneEvolutionUnit(true, x.PlayerUnit, units));
  }

  protected virtual void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    UnitIconInfo displayUnitInfo = this.displayUnitInfos[info_index];
    displayUnitInfo.gray = false;
    if (displayUnitInfo.select >= 0)
    {
      switch (this.iconSelMode)
      {
        case UnitSelectMenuBase.IconSelMode.Number:
          displayUnitInfo.icon.Select(displayUnitInfo.select);
          break;
        case UnitSelectMenuBase.IconSelMode.Check:
          if (displayUnitInfo.icon.unit.IsNormalUnit)
          {
            displayUnitInfo.icon.SelectByCheckIcon();
            break;
          }
          displayUnitInfo.icon.HideCheckIcon();
          break;
        case UnitSelectMenuBase.IconSelMode.CheckAndTextNumber:
          displayUnitInfo.icon.SelectByCheckAndNumber(displayUnitInfo);
          break;
        default:
          Debug.LogError((object) ("想定していないIconSelMode: " + (object) this.iconSelMode));
          break;
      }
      displayUnitInfo.gray = true;
    }
    allUnitIcon.SetCounter(displayUnitInfo.count);
    allUnitIcon.SetSelectionCounter(displayUnitInfo.SelectedCount);
    allUnitIcon.onClick = (Action<UnitIconBase>) (ui => this.Select(ui));
    if (displayUnitInfo.button_enable)
    {
      ((Behaviour) allUnitIcon.Button).enabled = true;
    }
    else
    {
      displayUnitInfo.gray = true;
      ((Behaviour) allUnitIcon.Button).enabled = true;
    }
    if (this.SelectedUnitIsMax() && displayUnitInfo.button_enable)
      allUnitIcon.Gray = !displayUnitInfo.gray;
    else
      allUnitIcon.Gray = displayUnitInfo.gray;
  }

  protected enum IconSelMode
  {
    Number,
    Check,
    CheckAndTextNumber,
  }
}
