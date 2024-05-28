// Decompiled with JetBrains decompiler
// Type: Unit00499MemoryBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit00499MemoryBase : BackButtonMenuBase
{
  protected PlayerUnit unit;
  protected int index;
  [SerializeField]
  protected Unit00499UnitStatus before;
  [SerializeField]
  protected Unit00499UnitStatus after;
  protected Unit00499SaveMemorySlotSelect menu;
  protected Action endUpdate;
  [SerializeField]
  protected Transform dir_reinforce_memory_slot_icon_container;
  [SerializeField]
  protected UILabel txt_Description;

  protected IEnumerator Delete()
  {
    Unit00499MemoryBase unit00499MemoryBase = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = WebAPI.UnitDeleteTransmigrateMemory(unit00499MemoryBase.unit.id).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().dismiss();
    e = unit00499MemoryBase.menu.DeleteUpdate();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (unit00499MemoryBase.menu.endUpdate != null)
      unit00499MemoryBase.menu.endUpdate();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    unit00499MemoryBase.IsPush = false;
  }

  protected IEnumerator Save()
  {
    Unit00499MemoryBase unit00499MemoryBase = this;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = WebAPI.UnitSaveTransmigrateMemory(unit00499MemoryBase.index, unit00499MemoryBase.unit.id).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Inequality((Object) unit00499MemoryBase.menu, (Object) null))
      unit00499MemoryBase.menu.endUpdate();
    else
      unit00499MemoryBase.endUpdate();
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    unit00499MemoryBase.IsPush = false;
  }

  public void IbtnBack() => Singleton<PopupManager>.GetInstance().dismiss();

  public override void onBackButton() => this.IbtnBack();
}
