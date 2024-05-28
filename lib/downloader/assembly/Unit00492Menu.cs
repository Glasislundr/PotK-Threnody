// Decompiled with JetBrains decompiler
// Type: Unit00492Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;

#nullable disable
public class Unit00492Menu : UnitMenuBase
{
  private Unit00492Menu.Param param_;

  public IEnumerator coInitialize(Unit00492Menu.Param param)
  {
    Unit00492Menu unit00492Menu = this;
    IEnumerator e = unit00492Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00492Menu.param_ = param;
    unit00492Menu.TxtPossessionUnit.SetTextLocalize(Consts.GetInstance().unit_004_9_2_evolution_material_possession_text);
    unit00492Menu.TxtNumber.SetTextLocalize(param.units_.Length);
    unit00492Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) param.units_, (IEnumerable<PlayerMaterialUnit>) null, UnitSortAndFilter.SORT_TYPES.GetOrder, SortAndFilter.SORT_TYPE_ORDER_BUY.ASCENDING, false, false, true, true, false, false, (Action) null, unit00492Menu.isBattleFirst, unit00492Menu.isTowerEntry);
    e = unit00492Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00492Menu.lastReferenceUnitID = -1;
    unit00492Menu.InitializeEnd();
  }

  public IEnumerator coUpdateUnits(PlayerUnit[] playerUnits)
  {
    Unit00492Menu unit00492Menu = this;
    List<PlayerUnit> playerUnitList1 = new List<PlayerUnit>();
    foreach (PlayerUnit unit in unit00492Menu.param_.units_)
    {
      PlayerUnit old = unit;
      playerUnitList1.Add(((IEnumerable<PlayerUnit>) playerUnits).First<PlayerUnit>((Func<PlayerUnit, bool>) (pu => pu.id == old.id)));
    }
    if (unit00492Menu.param_.baseUnit_ != (PlayerUnit) null)
    {
      // ISSUE: reference to a compiler-generated method
      PlayerUnit unit = ((IEnumerable<PlayerUnit>) playerUnits).First<PlayerUnit>(new Func<PlayerUnit, bool>(unit00492Menu.\u003CcoUpdateUnits\u003Eb__3_1));
      if (unit.favorite)
      {
        unit00492Menu.param_.baseUnit_ = (PlayerUnit) null;
        unit00492Menu.param_.onUpdate_(unit);
      }
      else
        unit00492Menu.param_.baseUnit_ = unit;
    }
    List<PlayerUnit> playerUnitList2 = new List<PlayerUnit>();
    foreach (PlayerUnit selectedUnit in unit00492Menu.param_.selectedUnits_)
    {
      PlayerUnit old = selectedUnit;
      PlayerUnit unit = ((IEnumerable<PlayerUnit>) playerUnits).First<PlayerUnit>((Func<PlayerUnit, bool>) (pu => pu.id == old.id));
      if (unit.favorite)
        unit00492Menu.param_.onUpdate_(unit);
      else
        playerUnitList2.Add(unit);
    }
    unit00492Menu.param_.selectedUnits_ = playerUnitList2.ToArray();
    IEnumerator e = unit00492Menu.UpdateInfoAndScroll(playerUnitList1.ToArray());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  private void onSelectUnitIcon(UnitIconBase icon)
  {
    if (this.IsPush)
      return;
    if (icon.PlayerUnit != (PlayerUnit) null)
    {
      this.IsPush = true;
      this.lastReferenceUnitID = icon.PlayerUnit.id;
      this.lastReferenceUnitIndex = this.GetUnitInfoDisplayIndex(icon.PlayerUnit);
      this.param_.onResult_(icon.PlayerUnit);
      this.backScene();
    }
    else
      Debug.LogWarning((object) "PlayerUnit Null : Unit00492Menu");
  }

  protected override IEnumerator CreateUnitIcon(
    int info_index,
    int unit_index,
    PlayerUnit baseUnit = null)
  {
    Unit00492Menu unit00492Menu = this;
    // ISSUE: reference to a compiler-generated method
    IEnumerator e = unit00492Menu.\u003C\u003En__0(info_index, unit_index, baseUnit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00492Menu.setupIconCustom(unit00492Menu.allUnitIcons[unit_index]);
  }

  protected override void CreateUnitIconCache(int info_index, int unit_index, PlayerUnit baseUnit = null)
  {
    base.CreateUnitIconCache(info_index, unit_index);
    this.setupIconCustom(this.allUnitIcons[unit_index]);
  }

  private void setupIconCustom(UnitIconBase icon)
  {
    if (icon.PlayerUnit == (PlayerUnit) null)
    {
      ((UIButtonColor) icon.Button).isEnabled = false;
    }
    else
    {
      PlayerUnit playerUnit = icon.PlayerUnit;
      if (!playerUnit.favorite && !playerUnit.CheckForNormalDeck() && !this.isSelected(playerUnit))
      {
        icon.Gray = false;
        icon.onClick = (Action<UnitIconBase>) (ui => this.onSelectUnitIcon(ui));
      }
      else if (this.param_.baseUnit_ != (PlayerUnit) null && this.param_.baseUnit_.id == playerUnit.id)
      {
        icon.SelectByCheckIcon(false);
        icon.onClick = (Action<UnitIconBase>) (ui => this.onSelectUnitIcon(ui));
      }
      else
      {
        if (this.isSelected(playerUnit))
          icon.SelectByCheckIcon();
        else
          icon.Gray = true;
        icon.onClick = (Action<UnitIconBase>) (ui => { });
      }
    }
  }

  private bool isSelected(PlayerUnit unit)
  {
    return this.param_.baseUnit_ != (PlayerUnit) null && this.param_.baseUnit_.id == unit.id || ((IEnumerable<PlayerUnit>) this.param_.selectedUnits_).Any<PlayerUnit>((Func<PlayerUnit, bool>) (pu => pu.id == unit.id));
  }

  public class Param
  {
    public PlayerUnit baseUnit_;
    public PlayerUnit[] selectedUnits_;
    public PlayerUnit[] units_;
    public Unit00492Menu.Param.EventUpdateUnit onUpdate_;
    public Unit00492Menu.Param.EventResult onResult_;

    public delegate void EventResult(PlayerUnit selected);

    public delegate void EventUpdateUnit(PlayerUnit unit);
  }
}
