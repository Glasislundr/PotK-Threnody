// Decompiled with JetBrains decompiler
// Type: Unit05412Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit05412Menu : Unit00412Menu
{
  [SerializeField]
  private UILabel TxtUnitCount;

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public override IEnumerator Init(
    Player player,
    PlayerUnit[] playerUnits,
    bool isEquip,
    bool forBattle = true)
  {
    Unit05412Menu unit05412Menu = this;
    unit05412Menu.SetIconType(UnitMenuBase.IconType.Normal);
    IEnumerator e = unit05412Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05412Menu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, (Persist<Persist.UnitSortAndFilterInfo>) null, isEquip, false, forBattle, false, false);
    e = unit05412Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit05412Menu.InitializeEnd();
    unit05412Menu.TxtNumber.SetTextLocalize(string.Format("{0}", (object) playerUnits.Length));
  }

  protected override void CreateUnitIconAction(int info_index, int unit_index)
  {
    UnitIconBase allUnitIcon;
    ((UnitIcon) (allUnitIcon = this.allUnitIcons[unit_index])).SetEarthButtonDetalEvent(this.allUnitInfos[info_index].playerUnit, this.getUnits());
    Action<UnitIconBase> action = (Action<UnitIconBase>) (ui => Unit0544Scene.ChangeScene(true, this.allUnitInfos[info_index].playerUnit, 1));
    allUnitIcon.onClick = action;
  }
}
