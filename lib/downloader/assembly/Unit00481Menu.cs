// Decompiled with JetBrains decompiler
// Type: Unit00481Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Unit00481Menu : Unit0048Menu
{
  public override IEnumerator Init(Player player, PlayerUnit[] playerUnits, bool isEquip)
  {
    Unit00481Menu unit00481Menu = this;
    IEnumerator e = unit00481Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerUnit[] array = ((IEnumerable<PlayerUnit>) playerUnits).Where<PlayerUnit>((Func<PlayerUnit, bool>) (x => x.unit.IsNormalUnit && x.buildup_limit > 0)).ToArray<PlayerUnit>();
    unit00481Menu.InitializeInfo((IEnumerable<PlayerUnit>) array, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00481SortAndFilter, isEquip, false, true, true, false);
    e = unit00481Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int length = SMManager.Get<PlayerUnit[]>().Length;
    unit00481Menu.unitCount = length;
    unit00481Menu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) length, (object) player.max_units));
    Singleton<PopupManager>.GetInstance().closeAll();
    unit00481Menu.InitializeEnd();
  }
}
