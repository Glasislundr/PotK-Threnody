// Decompiled with JetBrains decompiler
// Type: Unit00499SaveMemorySlotSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Unit00499SaveMemorySlotSelect : BackButtonMenuBase
{
  private PlayerUnit playerUnit;
  private List<PlayerUnit> recordUnitList;
  private GameObject listItem;
  [SerializeField]
  private NGxScroll scroll;
  private GameObject SaveMemoryConfirmPrefab;
  private GameObject MemoryDeletConfirmPrefab;
  private GameObject MemoryConfirmPrefab;
  [SerializeField]
  private UILabel txt_Description;
  public Action endUpdate;
  public bool isClose;
  private List<WeakReference<Coroutine>> ItemLoaders = new List<WeakReference<Coroutine>>();

  public IEnumerator DeleteUpdate()
  {
    this.scroll.Clear();
    IEnumerator e = this.Initialize(this.playerUnit, this.endUpdate);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Initialize(PlayerUnit playerUnit, Action endUpdate)
  {
    Unit00499SaveMemorySlotSelect menu = this;
    menu.endUpdate = endUpdate;
    menu.playerUnit = playerUnit;
    menu.recordUnitList = PlayerTransmigrateMemoryPlayerUnitIds.Current != null ? PlayerTransmigrateMemoryPlayerUnitIds.Current.PlayerUnits() : new List<PlayerUnit>();
    Future<GameObject> prefabF = (Future<GameObject>) null;
    IEnumerator e;
    if (Object.op_Equality((Object) menu.listItem, (Object) null))
    {
      prefabF = Res.Prefabs.unit004_9_9.dir_reinforce_memory_slot_list.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.listItem = prefabF.Result;
    }
    for (int index = 0; index < menu.recordUnitList.Count; ++index)
    {
      PlayerUnit recordUnit = menu.recordUnitList[index];
      GameObject gameObject = Object.Instantiate<GameObject>(menu.listItem);
      Unit00499MemorySlotItem component = gameObject.GetComponent<Unit00499MemorySlotItem>();
      menu.scroll.Add(gameObject);
      menu.ItemLoaders.Add(new WeakReference<Coroutine>(menu.StartCoroutine(component.Initialize(recordUnit, menu, index))));
    }
    menu.scroll.ResolvePosition();
    if (playerUnit == (PlayerUnit) null)
      menu.txt_Description.SetTextLocalize(Consts.GetInstance().MEMORY_SLOT);
    else if (menu.recordUnitList.Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => x == (PlayerUnit) null)))
      menu.txt_Description.SetTextLocalize(Consts.GetInstance().SAVE_MEMORY_SLOT);
    else
      menu.txt_Description.SetTextLocalize(Consts.GetInstance().SAVE_MEMORY_SLOT_LIMIT);
    if (Object.op_Equality((Object) menu.SaveMemoryConfirmPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_004_save_memory_confirm__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.SaveMemoryConfirmPrefab = prefabF.Result;
    }
    if (Object.op_Equality((Object) menu.MemoryDeletConfirmPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_004_memory_delet_confirm__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.MemoryDeletConfirmPrefab = prefabF.Result;
    }
    if (Object.op_Equality((Object) menu.MemoryConfirmPrefab, (Object) null))
    {
      prefabF = Res.Prefabs.popup.popup_004_memory_confirm__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      menu.MemoryConfirmPrefab = prefabF.Result;
    }
    menu.isClose = false;
  }

  public void IbtnBack() => Singleton<PopupManager>.GetInstance().dismiss();

  public override void onBackButton() => this.IbtnBack();

  public void ShowSaveMemoryConfirm(int index)
  {
    if (this.isClose || this.playerUnit == (PlayerUnit) null || PlayerTransmigrateMemoryPlayerUnitIds.Current != null && PlayerTransmigrateMemoryPlayerUnitIds.Current.PlayerUnits().Any<PlayerUnit>((Func<PlayerUnit, bool>) (x => x != (PlayerUnit) null && x.id == this.playerUnit.id)))
      return;
    Singleton<PopupManager>.GetInstance().open(this.SaveMemoryConfirmPrefab).GetComponent<Unit00499SaveMemoryConfirm>().Initialize(this.playerUnit, index, this);
  }

  public void ShowMemoryDeletConfirm(PlayerUnit unit, int index)
  {
    if (this.isClose)
      return;
    Singleton<PopupManager>.GetInstance().open(this.MemoryDeletConfirmPrefab).GetComponent<Unit00499MemoryDeleteConfirm>().Initialize(unit, index, this);
  }

  public void ShowMemoryConfirmPrefab(PlayerUnit unit, int index)
  {
    if (this.isClose)
      return;
    Singleton<PopupManager>.GetInstance().open(this.MemoryConfirmPrefab).GetComponent<Unit00499MemoryConfirm>().Initialize(index, this);
  }

  private void OnDestroy()
  {
    foreach (WeakReference<Coroutine> itemLoader in this.ItemLoaders)
    {
      Coroutine coroutine = (Coroutine) null;
      ref Coroutine local = ref coroutine;
      if (itemLoader.TryGetTarget(out local))
        this.StopCoroutine(coroutine);
    }
  }
}
