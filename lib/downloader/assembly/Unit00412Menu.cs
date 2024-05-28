// Decompiled with JetBrains decompiler
// Type: Unit00412Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Unit00412Menu : UnitMenuBase
{
  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public virtual IEnumerator Init(
    Player player,
    PlayerUnit[] playerUnits,
    bool isEquip,
    bool forBattle = true)
  {
    Unit00412Menu unit00412Menu = this;
    IEnumerator e = unit00412Menu.Initialize();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit00412Menu.InitializeInfo((IEnumerable<PlayerUnit>) playerUnits, (IEnumerable<PlayerMaterialUnit>) null, Persist.unit00412SortAndFilter, isEquip, false, forBattle, true, false);
    e = unit00412Menu.CreateUnitIcon();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    int length = SMManager.Get<PlayerUnit[]>().Length;
    unit00412Menu.TxtNumber.SetTextLocalize(string.Format("{0}/{1}", (object) length, (object) player.max_units));
    unit00412Menu.lastReferenceUnitID = -1;
    unit00412Menu.lastReferenceUnitIndex = -1;
    unit00412Menu.InitializeEnd();
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
    if (unitIcon.PlayerUnit == (PlayerUnit) null)
      Debug.LogError((object) "unit0412 CreateUnitIconAction PlayerUnit == null");
    unitIcon.onClick = (Action<UnitIconBase>) (ui =>
    {
      this.lastReferenceUnitID = unitIcon.PlayerUnit.id;
      this.lastReferenceUnitIndex = this.GetUnitInfoDisplayIndex(unitIcon.PlayerUnit);
      if (unitIcon.PlayerUnit.unit.awake_unit_flag)
        this.StartCoroutine(this.ShowGearSelectPopup(unitIcon.PlayerUnit));
      else
        Unit0044Scene.ChangeScene(true, unitIcon.PlayerUnit, 1);
    });
  }

  private IEnumerator ShowGearSelectPopup(PlayerUnit playerUnit)
  {
    if (!this.IsPushAndSet())
    {
      Future<GameObject> prefabF = new ResourceObject("Prefabs/popup/popup_004_equip_bugu_select__anim_popup01").Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = prefabF.Result.Clone();
      popup.SetActive(false);
      e = popup.GetComponent<Popup004EquipGearSelectMenu>().Init(playerUnit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    }
  }
}
