// Decompiled with JetBrains decompiler
// Type: Unit0549Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Unit0549Menu : Unit00411Menu
{
  public IEnumerator Init(PlayerUnit[] playerUnits)
  {
    Unit0549Menu unit0549Menu = this;
    unit0549Menu.SetIconType(UnitMenuBase.IconType.Normal);
    IEnumerator e = unit0549Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0549Menu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, (Persist<Persist.UnitSortAndFilterInfo>) null, false, false, false, false, false);
    e = unit0549Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0549Menu.InitializeEnd();
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    ((UnitIcon) unitIcon).SetEarthButtonDetalEvent(this.allUnitInfos[info_index].playerUnit, this.getUnits());
    if (unitIcon.Unit.IsMaterialUnit)
      unitIcon.onClick = (Action<UnitIconBase>) (ui => this.ChangeEvolutionScene(unitIcon.PlayerUnit));
    else
      unitIcon.onClick = (Action<UnitIconBase>) (ui => this.ChangeEvolutionScene(unitIcon.PlayerUnit));
  }

  private void ChangeEvolutionScene(PlayerUnit selectUnit)
  {
    Unit05499Scene.ChangeScene(true, selectUnit);
  }
}
