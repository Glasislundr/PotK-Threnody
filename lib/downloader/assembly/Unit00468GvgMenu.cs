// Decompiled with JetBrains decompiler
// Type: Unit00468GvgMenu
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
public class Unit00468GvgMenu : Unit00468Menu
{
  private GuildUtil.GvGPopupState state;

  private void InitializeAllUnitInfosExtend(GvgDeck gvgDeck)
  {
    bool updateIndex = this.selectedUnitIcons.Count<UnitIconInfo>() == 0;
    UnitIconInfo[] array = this.selectedUnitIcons.ToArray();
    this.selectedUnitIcons.Clear();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      if (updateIndex)
      {
        info.select = -1;
        info.for_battle = false;
        info.gray = false;
        int? nullable = ((IEnumerable<PlayerUnit>) gvgDeck.player_units).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (a => a != (PlayerUnit) null && a.id == info.playerUnit.id));
        if (nullable.HasValue)
        {
          info.gray = true;
          info.select = nullable.Value;
          info.for_battle = true;
          this.selectedUnitIcons.Add(info);
        }
      }
      else
      {
        info.select = -1;
        info.for_battle = false;
        info.gray = false;
        if (((IEnumerable<PlayerUnit>) gvgDeck.player_units).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (a => a != (PlayerUnit) null && a.id == info.playerUnit.id)).HasValue)
          info.for_battle = true;
        UnitIconInfo unitIconInfo = ((IEnumerable<UnitIconInfo>) array).FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit.id == info.playerUnit.id));
        if (unitIconInfo != null)
        {
          info.gray = true;
          info.select = unitIconInfo.select;
          this.selectedUnitIcons.Add(info);
        }
      }
    }
    this.updateExcludeOverkillers();
    this.ReflectionSelectUnit();
    this.CreateSelectUnitList(updateIndex);
    this.updateTxtCostValue(this.GetUsedCost());
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    Unit00468GvgMenu unit00468GvgMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit00468GvgMenu.\u003C\u003En__0(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIconBase allUnitIcon = unit00468GvgMenu.allUnitIcons[unit_index];
    if (!Object.op_Equality((Object) allUnitIcon, (Object) null))
    {
      unit00468GvgMenu.CreateUnitIconAction(info_index, unit_index);
      ((UnitIcon) allUnitIcon).BreakWeapon = false;
      if (!allUnitIcon.Selected)
      {
        ((UnitIcon) allUnitIcon).BreakWeapon = unit00468GvgMenu.CheckWeaponBreak(allUnitIcon);
        unit00468GvgMenu.IconGraySetting(allUnitIcon, unit00468GvgMenu.displayUnitInfos[info_index]);
      }
    }
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index, baseUnit);
    this.CreateUnitIconAction(info_index, unit_index);
    UnitIconBase allUnitIcon = this.allUnitIcons[unit_index];
    if (Object.op_Equality((Object) allUnitIcon, (Object) null))
      return;
    ((UnitIcon) allUnitIcon).BreakWeapon = false;
    if (allUnitIcon.Selected)
      return;
    ((UnitIcon) allUnitIcon).BreakWeapon = this.CheckWeaponBreak(allUnitIcon);
    this.IconGraySetting(allUnitIcon, this.displayUnitInfos[info_index]);
  }

  protected override void Select(UnitIconBase unitIcon)
  {
    if (!unitIcon.Selected && ((UnitIcon) unitIcon).BreakWeapon)
      return;
    base.Select(unitIcon);
    if (!this.CheckWeaponBreak(unitIcon))
      return;
    ((UnitIcon) unitIcon).BreakWeapon = true;
    unitIcon.Gray = true;
  }

  protected override void IconGraySetting(UnitIconBase unitIcon, UnitIconInfo info)
  {
    if (((UnitIcon) unitIcon).BreakWeapon)
      unitIcon.Gray = true;
    else
      base.IconGraySetting(unitIcon, info);
  }

  public override void IbtnClearS()
  {
    base.IbtnClearS();
    if (this.IsPush)
      return;
    foreach (UnitIconInfo displayUnitInfo in this.displayUnitInfos)
    {
      if (Object.op_Inequality((Object) displayUnitInfo.icon, (Object) null) && this.CheckWeaponBreak(displayUnitInfo.icon))
      {
        ((UnitIcon) displayUnitInfo.icon).BreakWeapon = true;
        displayUnitInfo.icon.Gray = true;
      }
    }
  }

  private bool CheckWeaponBreak(UnitIconBase unitIcon)
  {
    bool flag = false;
    if (unitIcon.PlayerUnit.equippedGear != (PlayerItem) null)
      flag = unitIcon.PlayerUnit.equippedGear.broken;
    if (unitIcon.PlayerUnit.equippedGear2 != (PlayerItem) null)
      flag = unitIcon.PlayerUnit.equippedGear2.broken;
    if (unitIcon.PlayerUnit.equippedGear3 != (PlayerItem) null)
      flag = unitIcon.PlayerUnit.equippedGear3.broken;
    return flag;
  }

  public IEnumerator Init(
    GvgDeck gvgDeck,
    PlayerUnit[] playerUnits,
    int max_cost,
    bool isEquip,
    GuildUtil.GvGPopupState state)
  {
    Unit00468GvgMenu unit00468GvgMenu = this;
    IEnumerator e = unit00468GvgMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468GvgMenu.serverTime = ServerTime.NowAppTime();
    unit00468GvgMenu.maxCost = max_cost;
    ((IEnumerable<GameObject>) unit00468GvgMenu.linkCharacters).ForEach<GameObject>((Action<GameObject>) (v => v.transform.Clear()));
    unit00468GvgMenu.updateTxtCostValue();
    playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    unit00468GvgMenu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00468SortAndFilter, isEquip, false, false, true, true, (Action) (() => this.InitializeAllUnitInfosExtend(gvgDeck)));
    e = unit00468GvgMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00468GvgMenu.CreateBottomInformationObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00468GvgMenu.DisplaySelectUnit();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468GvgMenu.UpdateInfomation();
    unit00468GvgMenu.state = state;
    unit00468GvgMenu.InitializeEnd();
  }

  protected override IEnumerator DeckEditAsync()
  {
    Unit00468GvgMenu unit00468GvgMenu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    int[] array = unit00468GvgMenu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).Select<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.playerUnit.id)).ToArray<int>();
    IEnumerator e;
    if (unit00468GvgMenu.state == GuildUtil.GvGPopupState.AtkTeam)
    {
      e = GuildUtil.EditGuildDeckAttack(array);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (unit00468GvgMenu.state == GuildUtil.GvGPopupState.DefTeam)
    {
      e = GuildUtil.EditGuildDeckDefense(array);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    unit00468GvgMenu.backScene();
  }

  public override void IbtnYes()
  {
    if (this.IsPush)
      return;
    this.StartCoroutine(this.DeckEditAsync());
  }

  public IEnumerator UpdateGvgDeck(Unit00468Scene.Mode mode, string player_id, string guild_id)
  {
    IEnumerator enumerator = (IEnumerator) null;
    switch (mode)
    {
      case Unit00468Scene.Mode.Unit00468GvgAtk:
        enumerator = GuildUtil.UpdateGuildDeckAttack(guild_id, player_id, (Action) (() => { }));
        break;
      case Unit00468Scene.Mode.Unit00468GvgDef:
        enumerator = GuildUtil.UpdateGuildDeckDefanse(guild_id, player_id, (Action) (() => { }));
        break;
    }
    if (enumerator != null)
    {
      IEnumerator ie = enumerator;
      while (ie.MoveNext())
        yield return ie.Current;
      ie = (IEnumerator) null;
    }
  }
}
