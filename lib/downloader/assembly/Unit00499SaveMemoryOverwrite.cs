// Decompiled with JetBrains decompiler
// Type: Unit00499SaveMemoryOverwrite
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
public class Unit00499SaveMemoryOverwrite : Unit00499MemoryBase
{
  public void Initialize(PlayerUnit unit, Action endUpdate)
  {
    this.endUpdate = endUpdate;
    List<PlayerUnit> self = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.PlayerUnits() : new List<PlayerUnit>();
    int? nullable1 = self.FirstIndexOrNull<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.id == unit.id));
    if (nullable1.HasValue)
    {
      this.unit = self[nullable1.Value];
      this.index = nullable1.Value;
      this.before.SetStatusText(this.unit);
      this.after.SetStatusTextMemory(this.unit);
      this.StartCoroutine(this.LoadUnit());
    }
    UILabel txtDescription = this.txt_Description;
    string overwriteDescription = Consts.GetInstance().SAVE_MEMORY_SLOT_OVERWRITE_DESCRIPTION;
    Hashtable args = new Hashtable();
    int? nullable2 = nullable1;
    args.Add((object) "index", (object) (nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() + 1) : new int?()));
    string text = Consts.Format(overwriteDescription, (IDictionary) args);
    txtDescription.SetTextLocalize(text);
  }

  public IEnumerator LoadUnit()
  {
    Unit00499SaveMemoryOverwrite saveMemoryOverwrite = this;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon component1 = prefabF.Result.Clone(saveMemoryOverwrite.before.linkUnit.transform).GetComponent<UnitIcon>();
    PlayerUnit[] units = new PlayerUnit[1]
    {
      saveMemoryOverwrite.unit
    };
    component1.RarityCenter();
    ((Collider) component1.buttonBoxCollider).enabled = true;
    ((Behaviour) component1.Button).enabled = true;
    component1.SetPressEvent((Action) (() => Singleton<PopupManager>.GetInstance().closeAll()));
    e = component1.SetPlayerUnit(saveMemoryOverwrite.unit, units, (PlayerUnit) null, true, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon component2 = prefabF.Result.Clone(saveMemoryOverwrite.after.linkUnit.transform).GetComponent<UnitIcon>();
    component2.RarityCenter();
    ((Collider) component2.buttonBoxCollider).enabled = true;
    ((Behaviour) component2.Button).enabled = true;
    component2.SetPressEvent((Action) (() => Singleton<PopupManager>.GetInstance().closeAll()));
    e = component2.SetPlayerUnit(saveMemoryOverwrite.unit, units, (PlayerUnit) null, true, true);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    prefabF = Res.Prefabs.unit004_9_9.slc_reinforce_memory_slot_icon.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    prefabF.Result.Clone(saveMemoryOverwrite.dir_reinforce_memory_slot_icon_container).GetComponent<UILabel>().SetTextLocalize(saveMemoryOverwrite.index + 1);
  }

  public void IbtnDecision()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.Save());
  }
}
