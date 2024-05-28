// Decompiled with JetBrains decompiler
// Type: Battle01TipEventHP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle01TipEventHP : Battle01TipEventBase
{
  private UnitIcon unitIcon;

  public override IEnumerator onInitAsync()
  {
    Battle01TipEventHP battle01TipEventHp = this;
    Future<GameObject> f = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    battle01TipEventHp.unitIcon = battle01TipEventHp.cloneIcon<UnitIcon>(f.Result);
    battle01TipEventHp.selectIcon(0);
  }

  private IEnumerator doSetIcon(UnitUnit unit)
  {
    IEnumerator e = this.unitIcon.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.unitIcon.BottomModeValue = UnitIconBase.BottomMode.Nothing;
  }

  public override void setData(BL.DropData e, BL.Unit unit)
  {
  }
}
