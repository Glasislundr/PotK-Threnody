// Decompiled with JetBrains decompiler
// Type: Unit00499MemorySlotItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Unit00499MemorySlotItem : NGMenuBase
{
  private Unit00499SaveMemorySlotSelect menu;
  private PlayerUnit unit;
  private int index;
  [SerializeField]
  private Transform dir_unit_thumb_container;
  [SerializeField]
  private Transform dir_reinforce_memory_slot_icon_container;
  [SerializeField]
  private GameObject ibtn_Popup_delet;
  [SerializeField]
  private UILabel txt_slot;

  public IEnumerator Initialize(
    PlayerUnit playerUnit,
    Unit00499SaveMemorySlotSelect menu,
    int index)
  {
    this.menu = menu;
    this.unit = playerUnit;
    this.index = index;
    Future<GameObject> prefabF = Res.Prefabs.unit004_9_9.slc_reinforce_memory_slot_icon.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UILabel component = prefabF.Result.Clone(this.dir_reinforce_memory_slot_icon_container).GetComponent<UILabel>();
    this.txt_slot.SetTextLocalize(Consts.Format(Consts.GetInstance().SAVE_MEMORY_SLOT_ITEM, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (index),
        (object) (index + 1)
      }
    }));
    int num = index + 1;
    component.SetTextLocalize(num);
    prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon unitIcon = prefabF.Result.Clone(this.dir_unit_thumb_container).GetComponent<UnitIcon>();
    bool flag = this.unit == (PlayerUnit) null;
    if (flag)
    {
      unitIcon.SetEmpty();
      ((Collider) unitIcon.buttonBoxCollider).enabled = false;
    }
    else
    {
      PlayerUnit[] playerUnits = new PlayerUnit[1]
      {
        this.unit
      };
      unitIcon.SetPressEvent((Action) (() => Singleton<PopupManager>.GetInstance().closeAll()));
      e = unitIcon.SetPlayerUnit(this.unit, playerUnits, (PlayerUnit) null, true, true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
      unitIcon.setMemoryLevelText();
    }
    this.ibtn_Popup_delet.SetActive(!flag);
  }

  public void IbtnDeteil()
  {
    if (this.IsPushAndSet())
      return;
    if (this.unit == (PlayerUnit) null)
      this.menu.ShowSaveMemoryConfirm(this.index);
    else
      this.menu.ShowMemoryConfirmPrefab(this.unit, this.index);
    this.StartCoroutine(this.IsPushOff());
  }

  public void IbtnDelete()
  {
    if (this.IsPushAndSet())
      return;
    this.menu.ShowMemoryDeletConfirm(this.unit, this.index);
    this.StartCoroutine(this.IsPushOff());
  }
}
