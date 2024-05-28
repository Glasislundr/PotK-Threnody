// Decompiled with JetBrains decompiler
// Type: DeckOrganization.TowerCreator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
namespace DeckOrganization
{
  public class TowerCreator : Creator
  {
    private TowerCreator.Selection selection_;
    private const int MIN_QUANTITY = 1;
    private const int MAX_QUANTITY = 50;
    private const int NUM_PER_ELEMENT = 8;
    private const int NUM_PER_GEARKIND = 8;

    public TowerCreator(PlayerUnit[] playerUnits, TowerCreator.Selection selection)
      : base((PlayerUnit[]) null, playerUnits, (List<Filter>) null, 1, 50)
    {
      this.selection_ = selection;
    }

    protected override IEnumerator coMake()
    {
      TowerCreator towerCreator = this;
      if (towerCreator.playerUnits_ != null && towerCreator.playerUnits_.Length >= towerCreator.minQuantity_)
      {
        IEnumerable<Unit> units = towerCreator.getUnits();
        IEnumerator e;
        switch (towerCreator.selection_)
        {
          case TowerCreator.Selection.Level:
            e = towerCreator.coSelectionLevel(units);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case TowerCreator.Selection.Element:
            e = towerCreator.coSelectionElement(units);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case TowerCreator.Selection.GearKind:
            e = towerCreator.coSelectionGearKind(units);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
          case TowerCreator.Selection.Favorite:
            e = towerCreator.coSelectionFavorite(units);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            break;
        }
      }
    }

    private IEnumerator coSelectionLevel(IEnumerable<Unit> units)
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      TowerCreator towerCreator = this;
      if (num != 0)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      List<Unit> regular = new List<Unit>();
      List<Unit> unitList = towerCreator.fillSubUnit(regular, units);
      towerCreator.setResult((IEnumerable<Unit>) unitList);
      return false;
    }

    private IEnumerator coSelectionElement(IEnumerable<Unit> units)
    {
      // ISSUE: reference to a compiler-generated field
      int num1 = this.\u003C\u003E1__state;
      TowerCreator towerCreator = this;
      if (num1 != 0)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      IOrderedEnumerable<IGrouping<TowerCreator.KeyElement, Unit>> orderedEnumerable = units.GroupBy<Unit, TowerCreator.KeyElement>((Func<Unit, TowerCreator.KeyElement>) (u =>
      {
        switch (u.unit_.GetElement())
        {
          case CommonElement.fire:
            return TowerCreator.KeyElement.Fire;
          case CommonElement.wind:
            return TowerCreator.KeyElement.Wind;
          case CommonElement.thunder:
            return TowerCreator.KeyElement.Thunder;
          case CommonElement.ice:
            return TowerCreator.KeyElement.Ice;
          case CommonElement.light:
            return TowerCreator.KeyElement.Light;
          case CommonElement.dark:
            return TowerCreator.KeyElement.Dark;
          default:
            return TowerCreator.KeyElement.None;
        }
      })).OrderBy<IGrouping<TowerCreator.KeyElement, Unit>, int>((Func<IGrouping<TowerCreator.KeyElement, Unit>, int>) (v => (int) v.Key));
      List<Unit> unitList1 = new List<Unit>();
      foreach (IGrouping<TowerCreator.KeyElement, Unit> grouping in (IEnumerable<IGrouping<TowerCreator.KeyElement, Unit>>) orderedEnumerable)
      {
        if (grouping.Key != TowerCreator.KeyElement.None)
        {
          IEnumerable<Unit> units1 = towerCreator.sortForTower((IEnumerable<Unit>) grouping);
          int num2 = 8;
          foreach (Unit unit in units1)
          {
            Unit u = unit;
            if (!unitList1.Any<Unit>((Func<Unit, bool>) (ru => ru.unit_.unit.ID == u.unit_.unit.ID)))
            {
              u.setIndex(unitList1.Count);
              unitList1.Add(u);
              if (--num2 == 0)
                break;
            }
          }
        }
      }
      List<Unit> unitList2 = towerCreator.fillSubUnit(unitList1, units);
      towerCreator.setResult((IEnumerable<Unit>) unitList2);
      return false;
    }

    private IEnumerator coSelectionGearKind(IEnumerable<Unit> units)
    {
      // ISSUE: reference to a compiler-generated field
      int num1 = this.\u003C\u003E1__state;
      TowerCreator towerCreator = this;
      if (num1 != 0)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      IOrderedEnumerable<IGrouping<TowerCreator.KeyGearKind, Unit>> orderedEnumerable = units.GroupBy<Unit, TowerCreator.KeyGearKind>((Func<Unit, TowerCreator.KeyGearKind>) (u =>
      {
        switch (u.unit_.unit.kind.Enum)
        {
          case GearKindEnum.sword:
            return TowerCreator.KeyGearKind.Sword;
          case GearKindEnum.axe:
            return TowerCreator.KeyGearKind.Axe;
          case GearKindEnum.spear:
            return TowerCreator.KeyGearKind.Spear;
          case GearKindEnum.bow:
            return TowerCreator.KeyGearKind.Bow;
          case GearKindEnum.gun:
            return TowerCreator.KeyGearKind.Gun;
          case GearKindEnum.staff:
            return TowerCreator.KeyGearKind.Staff;
          default:
            return TowerCreator.KeyGearKind.None;
        }
      })).OrderBy<IGrouping<TowerCreator.KeyGearKind, Unit>, int>((Func<IGrouping<TowerCreator.KeyGearKind, Unit>, int>) (v => (int) v.Key));
      List<Unit> unitList1 = new List<Unit>();
      foreach (IGrouping<TowerCreator.KeyGearKind, Unit> grouping in (IEnumerable<IGrouping<TowerCreator.KeyGearKind, Unit>>) orderedEnumerable)
      {
        if (grouping.Key != TowerCreator.KeyGearKind.None)
        {
          IEnumerable<Unit> units1 = towerCreator.sortForTower((IEnumerable<Unit>) grouping);
          int num2 = 8;
          foreach (Unit unit in units1)
          {
            Unit u = unit;
            if (!unitList1.Any<Unit>((Func<Unit, bool>) (ru => ru.unit_.unit.ID == u.unit_.unit.ID)))
            {
              u.setIndex(unitList1.Count);
              unitList1.Add(u);
              if (--num2 == 0)
                break;
            }
          }
        }
      }
      List<Unit> unitList2 = towerCreator.fillSubUnit(unitList1, units);
      towerCreator.setResult((IEnumerable<Unit>) unitList2);
      return false;
    }

    private IEnumerator coSelectionFavorite(IEnumerable<Unit> units)
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      TowerCreator towerCreator = this;
      if (num != 0)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      IEnumerable<Unit> units1 = towerCreator.sortForTower(units.Where<Unit>((Func<Unit, bool>) (u => u.unit_.favorite)));
      List<Unit> unitList1 = new List<Unit>();
      int maxQuantity = towerCreator.maxQuantity_;
      foreach (Unit unit in units1)
      {
        Unit ge = unit;
        if (!unitList1.Any<Unit>((Func<Unit, bool>) (ru => ru.unit_.unit.ID == ge.unit_.unit.ID)))
        {
          ge.setIndex(unitList1.Count);
          unitList1.Add(ge);
          if (--maxQuantity <= 0)
            break;
        }
      }
      List<Unit> unitList2 = towerCreator.fillSubUnit(unitList1, units);
      towerCreator.setResult((IEnumerable<Unit>) unitList2);
      return false;
    }

    private List<Unit> fillSubUnit(List<Unit> regular, IEnumerable<Unit> units)
    {
      int num = this.maxQuantity_ - regular.Count;
      if (num > 0)
      {
        foreach (Unit unit in this.sortForTower(units.Where<Unit>((Func<Unit, bool>) (u => !u.hasIndex))))
        {
          Unit u = unit;
          if (!regular.Any<Unit>((Func<Unit, bool>) (ru => ru.unit_.unit.ID == u.unit_.unit.ID)))
          {
            u.setIndex(regular.Count);
            regular.Add(u);
            if (--num == 0)
              break;
          }
        }
      }
      return regular;
    }

    private IEnumerable<Unit> sortForTower(IEnumerable<Unit> units)
    {
      return (IEnumerable<Unit>) units.OrderByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.level)).ThenByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.combat)).ThenByDescending<Unit, int>((Func<Unit, int>) (u => u.unit_.total_hp));
    }

    public enum Selection
    {
      Level,
      Element,
      GearKind,
      Favorite,
    }

    private enum KeyElement
    {
      None = 0,
      Fire = 1,
      Wind = 2,
      Thunder = 3,
      Ice = 4,
      Light = 5,
      Dark = 6,
      Num = 6,
    }

    private enum KeyGearKind
    {
      None = 0,
      Sword = 1,
      Axe = 2,
      Spear = 3,
      Bow = 4,
      Gun = 5,
      Num = 6,
      Staff = 6,
    }
  }
}
