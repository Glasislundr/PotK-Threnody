// Decompiled with JetBrains decompiler
// Type: Unit00411Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit00411Menu : UnitMenuBase
{
  [SerializeField]
  private Unit00410Menu unit00410Menu;

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public virtual IEnumerator Init(Player player, PlayerUnit[] playerUnits, bool isEquip)
  {
    Unit00411Menu unit00411Menu = this;
    if (Object.op_Implicit((Object) unit00411Menu.unit00410Menu))
      ((Behaviour) unit00411Menu.unit00410Menu).enabled = false;
    IEnumerator e = unit00411Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00411Menu.InitializeInfoEx((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00411SortAndFilter, isEquip, false, true, true, true, false);
    e = unit00411Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00411Menu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) playerUnits.Length, (object) player.max_units));
    unit00411Menu.SetTextPosession(playerUnits, player);
    unit00411Menu.InitializeEnd();
    if (unit00411Menu.lastReferenceUnitID != -1)
    {
      yield return (object) null;
      unit00411Menu.resolveScrollPosition(unit00411Menu.lastReferenceUnitID);
      yield return (object) null;
      unit00411Menu.setLastReference();
    }
  }

  public void SetTextPosession(PlayerUnit[] playerUnits, Player player)
  {
    this.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) playerUnits.Length, (object) player.max_units));
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
    if (unitIcon.Unit.IsMaterialUnit)
      unitIcon.onClick = (Action<UnitIconBase>) (ui => this.onClickMaterialIcon(unitIcon.PlayerUnit));
    else
      unitIcon.onClick = (Action<UnitIconBase>) (ui => this.onClickUnitlIcon(unitIcon.PlayerUnit));
  }

  public override IEnumerator UpdateInfoAndScroll(
    PlayerUnit[] playerUnits,
    PlayerMaterialUnit[] playerMaterialUnits = null)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Unit00411Menu unit00411Menu = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      unit00411Menu.SetNoListLable();
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated method
    this.\u003C\u003E2__current = (object) unit00411Menu.StartCoroutine(unit00411Menu.\u003C\u003En__1(playerUnits, playerMaterialUnits));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void onClickMaterialIcon(PlayerUnit unit)
  {
    Unit0042Scene.changeScene(true, unit, this.getUnits());
  }

  public void onClickUnitlIcon(PlayerUnit unit)
  {
    Unit0042Scene.changeScene(true, unit, this.getUnits());
  }

  public void onBtnUnitStorageList()
  {
    if (this.IsPushAndSet())
      return;
    if (!Object.op_Implicit((Object) this.unit00410Menu) || !((Behaviour) this.unit00410Menu).enabled)
      Unit004StorageScene.changeSceneListWithInitialize(true);
    else
      Unit004StorageScene.changeSceneSell(false, false);
  }

  public void onBtnUnitStorageIn()
  {
    if (this.IsPushAndSet())
      return;
    Unit004StorageInScene.changeScene(true);
  }

  public void onBtnUnitSell()
  {
    if (this.IsPushAndSet())
      return;
    Unit00468Scene.changeScene00410(false, Unit00410Menu.FromType.UnitList);
  }

  public void onBtnMaterialSell()
  {
    if (this.IsPushAndSet())
      return;
    Unit00468Scene.changeScene00410(false, Unit00410Menu.FromType.MaterialList);
  }

  public override void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().setStartScene("mypage");
    this.backScene();
  }
}
