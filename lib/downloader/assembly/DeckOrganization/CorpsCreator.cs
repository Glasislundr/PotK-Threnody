// Decompiled with JetBrains decompiler
// Type: DeckOrganization.CorpsCreator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace DeckOrganization
{
  public class CorpsCreator : Creator
  {
    private const int MIN_QUANTITY = 1;

    public CorpsCreator(PlayerUnit[] playerUnits, int max)
      : base((PlayerUnit[]) null, playerUnits, (List<Filter>) null, 1, max)
    {
    }

    protected override IEnumerator coMake()
    {
      CorpsCreator corpsCreator = this;
      if (corpsCreator.playerUnits_ != null && corpsCreator.playerUnits_.Length >= corpsCreator.minQuantity_)
      {
        IEnumerable<Unit> units = corpsCreator.getUnits();
        IEnumerator e = corpsCreator.coSelectionLevel(units);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }

    private IEnumerator coSelectionLevel(IEnumerable<Unit> units)
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      CorpsCreator corpsCreator = this;
      if (num != 0)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      List<Unit> unitList = new List<Unit>();
      int maxQuantity = corpsCreator.maxQuantity_;
      if (maxQuantity > 0)
      {
        foreach (Unit unit in corpsCreator.sortForCorps(units))
        {
          unit.setIndex(unitList.Count);
          unitList.Add(unit);
          if (--maxQuantity == 0)
            break;
        }
      }
      corpsCreator.setResult((IEnumerable<Unit>) unitList);
      return false;
    }

    private IEnumerable<Unit> sortForCorps(IEnumerable<Unit> units)
    {
      return (IEnumerable<Unit>) units.OrderByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.level)).ThenByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.combat)).ThenByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.total_hp));
    }
  }
}
