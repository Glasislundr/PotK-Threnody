// Decompiled with JetBrains decompiler
// Type: Unit00468RaidMenu
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
public class Unit00468RaidMenu : Unit00468Menu
{
  private int[] usedUnitIds;
  private bool isSimulated;

  public IEnumerator Init(
    PlayerUnit[] deck,
    PlayerUnit[] playerUnits,
    int[] usedUnitIds,
    bool isSimulated)
  {
    Unit00468RaidMenu unit00468RaidMenu = this;
    unit00468RaidMenu.usedUnitIds = usedUnitIds;
    unit00468RaidMenu.isSimulated = isSimulated;
    unit00468RaidMenu.isSelectNumPack = false;
    IEnumerator e = unit00468RaidMenu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468RaidMenu.serverTime = ServerTime.NowAppTime();
    unit00468RaidMenu.maxCost = SMManager.Get<Player>().max_cost;
    ((IEnumerable<GameObject>) unit00468RaidMenu.linkCharacters).ForEach<GameObject>((Action<GameObject>) (v => v.transform.Clear()));
    unit00468RaidMenu.updateTxtCostValue();
    playerUnits = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit)).ToArray<PlayerUnit>();
    unit00468RaidMenu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00468SortAndFilter, false, false, false, true, true, (Action) (() => this.InitializeAllUnitInfosExtend(deck)));
    e = unit00468RaidMenu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00468RaidMenu.CreateBottomInformationObject();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = unit00468RaidMenu.DisplaySelectUnit();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468RaidMenu.UpdateInfomation();
    unit00468RaidMenu.InitializeEnd();
  }

  private void InitializeAllUnitInfosExtend(PlayerUnit[] deck)
  {
    bool updateIndex = this.selectedUnitIcons.Count == 0;
    UnitIconInfo[] array = this.selectedUnitIcons.ToArray();
    this.selectedUnitIcons.Clear();
    foreach (UnitIconInfo allUnitInfo in this.allUnitInfos)
    {
      UnitIconInfo info = allUnitInfo;
      if (updateIndex)
      {
        info.select = -1;
        info.for_battle = false;
        info.is_used = false;
        int? nullable = ((IEnumerable<PlayerUnit>) deck).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (a => a != (PlayerUnit) null && a.id == info.playerUnit.id));
        if (nullable.HasValue)
        {
          info.for_battle = true;
          if (!((IEnumerable<int>) this.usedUnitIds).Contains<int>(info.playerUnit.id))
          {
            info.select = nullable.Value;
            this.selectedUnitIcons.Add(info);
          }
        }
        if (((IEnumerable<int>) this.usedUnitIds).Contains<int>(info.playerUnit.id))
          info.is_used = true;
      }
      else
      {
        info.select = -1;
        info.for_battle = false;
        info.is_used = false;
        if (((IEnumerable<PlayerUnit>) deck).FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (a => a != (PlayerUnit) null && a.id == info.playerUnit.id)).HasValue)
          info.for_battle = true;
        UnitIconInfo unitIconInfo = ((IEnumerable<UnitIconInfo>) array).FirstOrDefault<UnitIconInfo>((Func<UnitIconInfo, bool>) (x => x.playerUnit.id == info.playerUnit.id));
        if (unitIconInfo != null)
        {
          info.select = unitIconInfo.select;
          this.selectedUnitIcons.Add(info);
        }
        if (((IEnumerable<int>) this.usedUnitIds).Contains<int>(info.playerUnit.id))
          info.is_used = true;
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
    Unit00468RaidMenu unit00468RaidMenu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit00468RaidMenu.\u003C\u003En__0(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00468RaidMenu.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index, baseUnit);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void Select(UnitIconBase unitIcon)
  {
    if (!unitIcon.Selected && (((UnitIcon) unitIcon).BreakWeapon || unitIcon.UnitUsed))
      return;
    base.Select(unitIcon);
  }

  protected override void IconGraySetting(UnitIconBase unitIcon, UnitIconInfo info)
  {
    bool flag = this.CheckWeaponBreak(info.playerUnit);
    ((UnitIcon) unitIcon).BreakWeapon = flag;
    if (flag)
    {
      info.gray = true;
      unitIcon.Gray = true;
    }
    else if (info.is_used)
    {
      info.gray = true;
      unitIcon.Gray = true;
    }
    else
      base.IconGraySetting(unitIcon, info);
  }

  public override void IbtnClearS()
  {
    if (this.IsPush)
      return;
    foreach (UnitIconBase allUnitIcon in this.allUnitIcons)
    {
      if (Object.op_Inequality((Object) allUnitIcon, (Object) null) && allUnitIcon.PlayerUnit != (PlayerUnit) null)
      {
        this.Deselect(allUnitIcon);
        if (((UnitIcon) allUnitIcon).BreakWeapon || allUnitIcon.UnitUsed)
          allUnitIcon.Gray = true;
        else
          allUnitIcon.Gray = false;
      }
    }
    this.allUnitInfos.ForEach((Action<UnitIconInfo>) (v => v.select = -1));
    this.displayUnitInfos.ForEach((Action<UnitIconInfo>) (v => v.select = -1));
    this.selectedUnitIcons.Clear();
    this.UpdateInfomation();
  }

  private bool CheckWeaponBreak(PlayerUnit playerUnit)
  {
    bool flag = false;
    if (playerUnit.equippedGear != (PlayerItem) null)
      flag = playerUnit.equippedGear.broken;
    if (playerUnit.equippedGear2 != (PlayerItem) null)
      flag = playerUnit.equippedGear2.broken;
    if (playerUnit.equippedGear3 != (PlayerItem) null)
      flag = playerUnit.equippedGear3.broken;
    return flag;
  }

  public override void IbtnYes()
  {
    if (this.IsPush)
      return;
    this.StartCoroutine(this.DeckEditAsync());
  }

  protected override IEnumerator DeckEditAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit00468RaidMenu unit00468RaidMenu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GuildUtil.RaidDeck = unit00468RaidMenu.selectedUnitIcons.OrderBy<UnitIconInfo, int>((Func<UnitIconInfo, int>) (x => x.select)).Select<UnitIconInfo, PlayerUnit>((Func<UnitIconInfo, PlayerUnit>) (x => x.playerUnit)).ToArray<PlayerUnit>();
    unit00468RaidMenu.backScene();
    return false;
  }
}
