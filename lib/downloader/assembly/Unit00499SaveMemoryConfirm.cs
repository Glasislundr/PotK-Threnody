// Decompiled with JetBrains decompiler
// Type: Unit00499SaveMemoryConfirm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit00499SaveMemoryConfirm : Unit00499MemoryBase
{
  public void Initialize(PlayerUnit unit, int index, Unit00499SaveMemorySlotSelect menu)
  {
    this.unit = unit;
    this.index = index;
    this.menu = menu;
    this.before.SetStatusText(unit);
    this.StartCoroutine(this.LoadUnit());
    this.txt_Description.SetTextLocalize(Consts.Format(Consts.GetInstance().SAVE_MEMORY_SLOT_DESCRIPTION, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (index),
        (object) (index + 1)
      }
    }));
  }

  public IEnumerator LoadUnit()
  {
    Unit00499SaveMemoryConfirm saveMemoryConfirm = this;
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon component = prefabF.Result.Clone(saveMemoryConfirm.before.linkUnit.transform).GetComponent<UnitIcon>();
    PlayerUnit[] playerUnits = new PlayerUnit[1]
    {
      saveMemoryConfirm.unit
    };
    component.RarityCenter();
    ((Collider) component.buttonBoxCollider).enabled = true;
    ((Behaviour) component.Button).enabled = true;
    component.SetPressEvent((Action) (() => Singleton<PopupManager>.GetInstance().closeAll()));
    e = component.SetPlayerUnit(saveMemoryConfirm.unit, playerUnits, (PlayerUnit) null, true, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void IbtnDecision()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.Save());
    this.menu.isClose = true;
  }
}
