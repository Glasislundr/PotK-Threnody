// Decompiled with JetBrains decompiler
// Type: Unit05411Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit05411Menu : Unit00411Menu
{
  [SerializeField]
  private UILabel TxtUnitCount;

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public override IEnumerator Init(Player player, PlayerUnit[] playerUnits, bool isEquip)
  {
    Unit05411Menu unit05411Menu = this;
    unit05411Menu.SetIconType(UnitMenuBase.IconType.Normal);
    IEnumerator e = unit05411Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05411Menu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, (Persist<Persist.UnitSortAndFilterInfo>) null, false, isEquip, false, false, false);
    e = unit05411Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05411Menu.InitializeEnd();
    unit05411Menu.TxtUnitCount.SetTextLocalize(unit05411Menu.allUnitInfos.Count);
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase unitIcon = this.allUnitIcons[unit_index];
    unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    ((UnitIcon) unitIcon).SetEarthButtonDetalEvent(this.allUnitInfos[info_index].playerUnit, this.getUnits());
    unitIcon.onClick = (Action<UnitIconBase>) (ui => Unit0542Scene.changeScene(true, unitIcon.PlayerUnit, this.getUnits()));
  }
}
