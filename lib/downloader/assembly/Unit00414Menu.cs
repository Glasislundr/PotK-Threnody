// Decompiled with JetBrains decompiler
// Type: Unit00414Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Unit00414Menu : UnitMenuBase
{
  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public virtual IEnumerator Init(
    Player player,
    PlayerMaterialUnit[] playerMaterialUnits,
    bool isEquip)
  {
    Unit00414Menu unit00414Menu = this;
    IEnumerator e = unit00414Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00414Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) null, (IEnumerable<PlayerMaterialUnit>) playerMaterialUnits, UnitSortAndFilter.SORT_TYPES.UnitID, SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING, false, false, false, false, true, false, (Action) null, unit00414Menu.isBattleFirst, unit00414Menu.isTowerEntry);
    e = unit00414Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00414Menu.InitializeEnd();
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    IEnumerator e = base.CreateUnitIcon(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.CreateUnitIconAction(info_index, unit_index);
  }

  protected virtual void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    unitIcon.onClick = (Action<UnitIconBase>) (ui => this.onClickMaterialIcon(unitIcon.PlayerUnit));
  }

  public void onClickMaterialIcon(PlayerUnit unit)
  {
    Unit0042Scene.changeScene(true, unit, this.getUnits());
  }
}
