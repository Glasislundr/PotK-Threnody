// Decompiled with JetBrains decompiler
// Type: Unit004682Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using CustomDeck;
using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit004682Menu : Unit00468Menu
{
  private int changeNumber;
  private HashSet<int> includeOverkillersIds;
  private const int FIRST_DECK = 0;
  private const int LAST_MEMBER = 1;
  private EditUnitParam customEdit_;

  protected override void Sort(SortInfo info) => this.BaseSort(info);

  protected override int GetUsedCost()
  {
    int cost = 0;
    this.selectedUnitIcons.ForEach((Action<UnitIconInfo>) (x => cost += x.playerUnit.cost));
    UnitIconInfo unitIconInfo = this.selectedUnitIcons.FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.select == this.changeNumber));
    if (unitIconInfo != null)
      cost -= unitIconInfo.playerUnit.cost;
    return cost;
  }

  protected override void updateTxtCostValue(int cost = 0) => this.totalCost = cost;

  public override void UpdateInfomation()
  {
  }

  public override void InitializeAllUnitInfosExtend(PlayerDeck playerDeck)
  {
    this.selectedUnitIcons.Clear();
    PlayerUnit[] playerUnits = playerDeck.player_units;
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      info.select = -1;
      info.for_battle = false;
      if (!info.removeButton)
      {
        int? nullable = ((IEnumerable<PlayerUnit>) playerUnits).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (a => a != (PlayerUnit) null && a.id == info.playerUnit.id));
        if (nullable.HasValue)
        {
          info.select = nullable.Value;
          info.for_battle = true;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    this.updateExcludeOverkillers(playerUnits);
    this.includeOverkillersIds = new HashSet<int>();
    if (playerUnits.Length > this.changeNumber && playerUnits[this.changeNumber] != (PlayerUnit) null)
    {
      PlayerUnit playerUnit = playerUnits[this.changeNumber];
      if (playerUnit.isAnyCacheOverkillersUnits)
      {
        for (int index = 0; index < playerUnit.cache_overkillers_units.Length; ++index)
        {
          if (playerUnit.cache_overkillers_units[index] != (PlayerUnit) null)
            this.includeOverkillersIds.Add(playerUnit.cache_overkillers_units[index].id);
        }
      }
      else
      {
        int overkillersBaseId;
        if ((overkillersBaseId = playerUnit.overkillers_base_id) > 0)
          this.includeOverkillersIds.Add(overkillersBaseId);
      }
      if (this.includeOverkillersIds.Count > 0)
        this.includeOverkillersIds.Add(playerUnit.id);
    }
    this.ReflectionSelectUnit();
    this.CreateSelectUnitList(true);
    this.updateTxtCostValue(this.GetUsedCost());
  }

  public override void InitializeAllUnitInfosExtend(EditUnitParam param)
  {
    this.selectedUnitIcons.Clear();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      info.select = -1;
      info.for_battle = false;
      if (!info.removeButton)
      {
        if (param.usedUnitIds != null)
          info.is_used = ((IEnumerable<int>) param.usedUnitIds).Contains<int>(info.playerUnit.id);
        int? nullable = ((IEnumerable<int>) param.deck.player_unit_ids).FirstIndexOrNull<int>((Func<int, bool>) (a => a != 0 && info.playerUnit.id == a));
        if (nullable.HasValue)
        {
          info.select = nullable.Value;
          info.for_battle = true;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    this.ReflectionSelectUnit();
    this.CreateSelectUnitList(false);
    this.updateTxtCostValue(this.GetUsedCost());
  }

  private bool GetRemoveBtnFlg(PlayerDeck playerDeck, int number)
  {
    bool removeBtnFlg = false;
    if (playerDeck.player_unit_ids[number].HasValue)
    {
      removeBtnFlg = true;
      int num = 0;
      foreach (PlayerUnit playerUnit in playerDeck.player_units)
      {
        if (playerUnit != (PlayerUnit) null)
          ++num;
      }
      if (playerDeck.deck_number == 0 && num <= 1)
        removeBtnFlg = false;
    }
    return removeBtnFlg;
  }

  public IEnumerator Init(
    Player player,
    PlayerDeck playerDeck,
    PlayerUnit[] playerUnits,
    int max_cost,
    int number,
    bool isEquip)
  {
    Unit004682Menu unit004682Menu = this;
    IEnumerator e = unit004682Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004682Menu.serverTime = ServerTime.NowAppTime();
    unit004682Menu.changeNumber = number;
    unit004682Menu.deck_type_id = playerDeck.deck_type_id;
    unit004682Menu.deck_number = playerDeck.deck_number;
    unit004682Menu.maxCost = max_cost;
    unit004682Menu.totalCost = 0;
    playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    unit004682Menu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00468SortAndFilter, isEquip, unit004682Menu.GetRemoveBtnFlg(playerDeck, number), false, true, true, (Action) (() => this.InitializeAllUnitInfosExtend(playerDeck)));
    e = unit004682Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004682Menu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) unit004682Menu.allUnitInfos.Count<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => !x.removeButton)), (object) player.max_units));
    unit004682Menu.InitializeEnd();
    if (unit004682Menu.lastReferenceUnitID != -1)
    {
      yield return (object) null;
      unit004682Menu.resolveScrollPosition(unit004682Menu.lastReferenceUnitID);
      yield return (object) null;
      unit004682Menu.setLastReference();
    }
  }

  public IEnumerator Init(EditUnitParam param)
  {
    Unit004682Menu unit004682Menu = this;
    unit004682Menu.customEdit_ = param;
    unit004682Menu.isEditCustomDeck = true;
    unit004682Menu.customDeck = param.deck;
    Player player = Player.Current;
    IEnumerator e = unit004682Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004682Menu.serverTime = ServerTime.NowAppTime();
    unit004682Menu.changeNumber = param.single.index;
    unit004682Menu.deck_type_id = param.deck.deck_type_id;
    unit004682Menu.deck_number = param.deck.deck_number;
    unit004682Menu.maxCost = player.max_cost;
    unit004682Menu.totalCost = 0;
    unit004682Menu.includeOverkillersIds = new HashSet<int>();
    unit004682Menu.excludeOverkillersIds = (HashSet<int>) null;
    unit004682Menu.updateExcludeOverkillers(param.units);
    unit004682Menu.InitializeInfo((IEnumerable<PlayerUnit>) param.units, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00468SortAndFilter, false, param.deck.unit_parameter_list[param.single.index].player_unit_id != 0, false, true, true, (Action) (() => this.InitializeAllUnitInfosExtend(param)));
    e = unit004682Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004682Menu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) unit004682Menu.allUnitInfos.Count<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => !x.removeButton)), (object) player.max_units));
    unit004682Menu.InitializeEnd();
    if (unit004682Menu.lastReferenceUnitID != -1)
    {
      yield return (object) null;
      unit004682Menu.resolveScrollPosition(unit004682Menu.lastReferenceUnitID);
      yield return (object) null;
      unit004682Menu.setLastReference();
    }
  }

  private void DeckRemove(UnitIconBase ui)
  {
    if (this.customEdit_ != null)
    {
      this.customEdit_.single.setUnit(this.customEdit_.single, 0);
      this.backScene();
    }
    else
    {
      ModelUnits.Instance.DestroyModelUnits();
      int? nullable = this.allUnitInfos.FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => v.playerUnit != (PlayerUnit) null && v.select == this.changeNumber));
      if (!nullable.HasValue)
        return;
      UnitIconInfo allUnitInfo = this.allUnitInfos[nullable.Value];
      if (allUnitInfo != null && !allUnitInfo.removeButton)
        allUnitInfo.select = -1;
      this.CreateSelectUnitList(true);
      this.StartCoroutine(this.DeckEditAsync());
    }
  }

  private void DeckEdit(UnitIconBase ui)
  {
    if (ui.Gray)
      return;
    if (this.customEdit_ != null)
    {
      this.customEdit_.single.setUnit(this.customEdit_.single, ui.PlayerUnit.id);
      this.backScene();
    }
    else
    {
      ModelUnits.Instance.DestroyModelUnits();
      int? nullable = this.allUnitInfos.FirstIndexOrNull<UnitIconInfo>((Func<UnitIconInfo, bool>) (v => v.playerUnit != (PlayerUnit) null && v.select == this.changeNumber));
      if (nullable.HasValue)
      {
        UnitIconInfo allUnitInfo = this.allUnitInfos[nullable.Value];
        if (allUnitInfo != null && !allUnitInfo.removeButton)
          allUnitInfo.select = -1;
      }
      UnitIconInfo unitInfoAll = this.GetUnitInfoAll(ui.PlayerUnit);
      if (unitInfoAll != null && !unitInfoAll.removeButton)
        unitInfoAll.select = this.changeNumber;
      this.CreateSelectUnitList(true);
      this.StartCoroutine(this.DeckEditAsync());
    }
  }

  protected override IEnumerator DeckEditAsync()
  {
    Unit004682Menu unit004682Menu = this;
    int[] array = unit004682Menu.selectedUnitIcons.Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>();
    IEnumerator e = unit004682Menu.DeckEditApi(unit004682Menu.deck_type_id, unit004682Menu.deck_number, array);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit004682Menu.backScene();
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    UnitIconInfo displayUnitInfo = this.displayUnitInfos[info_index];
    if (displayUnitInfo.removeButton)
    {
      allUnitIcon.onClick = (Action<UnitIconBase>) (ui => this.DeckRemove(ui));
      allUnitIcon.Gray = false;
      ((Behaviour) allUnitIcon.Button).enabled = true;
    }
    else
    {
      allUnitIcon.onClick = (Action<UnitIconBase>) (ui => this.DeckEdit(ui));
      if (this.isEditCustomDeck)
      {
        if (allUnitIcon.PlayerUnit.cost > this.maxCost - this.totalCost || displayUnitInfo.select != -1 && displayUnitInfo.select != this.changeNumber)
        {
          displayUnitInfo.gray = true;
          allUnitIcon.Gray = true;
          ((Behaviour) allUnitIcon.Button).enabled = true;
        }
        else
        {
          displayUnitInfo.gray = false;
          allUnitIcon.Gray = false;
          ((Behaviour) allUnitIcon.Button).enabled = true;
        }
        allUnitIcon.Overkillers = this.excludeOverkillersIds.Contains(allUnitIcon.PlayerUnit.id);
      }
      else
      {
        bool flag1 = this.includeOverkillersIds.Contains(allUnitIcon.PlayerUnit.id);
        bool flag2 = this.excludeOverkillersIds.Contains(allUnitIcon.PlayerUnit.id);
        if (allUnitIcon.PlayerUnit.cost > this.maxCost - this.totalCost || displayUnitInfo.select != -1 && displayUnitInfo.select != this.changeNumber || !flag1 & flag2)
        {
          displayUnitInfo.gray = true;
          allUnitIcon.Gray = true;
          ((Behaviour) allUnitIcon.Button).enabled = true;
          allUnitIcon.Overkillers = flag2;
        }
        else
        {
          displayUnitInfo.gray = false;
          allUnitIcon.Gray = false;
          ((Behaviour) allUnitIcon.Button).enabled = true;
          allUnitIcon.Overkillers = flag1;
        }
      }
      allUnitIcon.SetupDeckStatusBlink();
      allUnitIcon.SelectNumberBase.SetActive(false);
      if (!Singleton<NGGameDataManager>.GetInstance().IsSea)
        return;
      if (displayUnitInfo != null && displayUnitInfo.unit != null)
        ((UnitIcon) allUnitIcon).SetSeaPiece(displayUnitInfo.unit.GetPiece);
      else
        ((UnitIcon) allUnitIcon).SetSeaPiece(false);
    }
  }
}
