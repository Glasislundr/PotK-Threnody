// Decompiled with JetBrains decompiler
// Type: Unit00499MemoryConfirm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit00499MemoryConfirm : Unit00499MemoryBase
{
  public void Initialize(int index, Unit00499SaveMemorySlotSelect menu)
  {
    this.index = index;
    this.menu = menu;
    this.unit = PlayerTransmigrateMemoryPlayerUnitIds.Current.PlayerUnits()[index];
    this.before.SetStatusText(this.unit);
    this.after.SetStatusTextMemory(this.unit);
    this.StartCoroutine(this.LoadUnit());
  }

  public IEnumerator LoadUnit()
  {
    Unit00499MemoryConfirm unit00499MemoryConfirm = this;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon component1 = prefabF.Result.Clone(unit00499MemoryConfirm.before.linkUnit.transform).GetComponent<UnitIcon>();
    PlayerUnit[] units = new PlayerUnit[1]
    {
      unit00499MemoryConfirm.unit
    };
    component1.RarityCenter();
    ((Collider) component1.buttonBoxCollider).enabled = true;
    ((Behaviour) component1.Button).enabled = true;
    component1.SetPressEvent((Action) (() => Singleton<PopupManager>.GetInstance().closeAll()));
    e = component1.SetPlayerUnit(unit00499MemoryConfirm.unit, units, (PlayerUnit) null, true, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon component2 = prefabF.Result.Clone(unit00499MemoryConfirm.after.linkUnit.transform).GetComponent<UnitIcon>();
    component2.RarityCenter();
    ((Collider) component2.buttonBoxCollider).enabled = true;
    ((Behaviour) component2.Button).enabled = true;
    component2.SetPressEvent((Action) (() => Singleton<PopupManager>.GetInstance().closeAll()));
    e = component2.SetPlayerUnit(unit00499MemoryConfirm.unit, units, (PlayerUnit) null, true, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    prefabF = Res.Prefabs.unit004_9_9.slc_reinforce_memory_slot_icon.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    prefabF.Result.Clone(unit00499MemoryConfirm.dir_reinforce_memory_slot_icon_container).GetComponent<UILabel>().SetTextLocalize(unit00499MemoryConfirm.index + 1);
  }
}
