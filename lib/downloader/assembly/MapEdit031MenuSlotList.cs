// Decompiled with JetBrains decompiler
// Type: MapEdit031MenuSlotList
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
public class MapEdit031MenuSlotList : MapEditMenuBase
{
  [SerializeField]
  private GameObject topInterface_;
  private MapEditSaveSlotSelect selector_;

  public override MapEdit031TopMenu.EditState editState_ => MapEdit031TopMenu.EditState.SlotList;

  protected override IEnumerator initializeAsync()
  {
    MapEdit031MenuSlotList edit031MenuSlotList = this;
    Future<GameObject> ldprefab = MapEdit.Prefabs.map_save_slot_select.Load<GameObject>();
    IEnumerator e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!Object.op_Equality((Object) ldprefab.Result, (Object) null))
    {
      GameObject go = ldprefab.Result.Clone(edit031MenuSlotList.topInterface_.transform);
      edit031MenuSlotList.selector_ = go.GetComponent<MapEditSaveSlotSelect>();
      e = edit031MenuSlotList.selector_.initialize(SMManager.Get<PlayerGuildTownSlot[]>(), edit031MenuSlotList.topMenu_.data_.editId_, edit031MenuSlotList.topMenu_.data_.defaultSlot_, new Action<int, PlayerGuildTownSlotPosition[]>(edit031MenuSlotList.onClickedMapDetail), new Action<int>(edit031MenuSlotList.onClickedSelect), new Action(edit031MenuSlotList.onClickedClose));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      edit031MenuSlotList.addControlObject(go);
    }
  }

  protected override void onEnable() => this.ui3DEvent_.isEnabled_ = false;

  protected override void onDisable()
  {
  }

  public override void onBackButton()
  {
  }

  private void onClickedMapDetail(int maptownId, PlayerGuildTownSlotPosition[] facilitiesData)
  {
    if (this.waitAndSet())
      return;
    this.StartCoroutine(this.doPopupMapDetail(maptownId, facilitiesData));
  }

  private IEnumerator doPopupMapDetail(int maptownId, PlayerGuildTownSlotPosition[] facilitiesData)
  {
    MapEdit031MenuSlotList edit031MenuSlotList = this;
    Future<GameObject> ldDetail = MapEdit.Prefabs.popup_base_map_detail.Load<GameObject>();
    IEnumerator e = ldDetail.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) ldDetail.Result, (Object) null))
    {
      edit031MenuSlotList.clearWait();
    }
    else
    {
      List<Tuple<int, int>> list = facilitiesData != null ? ((IEnumerable<PlayerGuildTownSlotPosition>) facilitiesData).Select<PlayerGuildTownSlotPosition, Tuple<int, int>>((Func<PlayerGuildTownSlotPosition, Tuple<int, int>>) (f => new Tuple<int, int>(f.x, f.y))).ToList<Tuple<int, int>>() : (List<Tuple<int, int>>) null;
      GameObject go = Singleton<PopupManager>.GetInstance().open(ldDetail.Result, isNonSe: true, isNonOpenAnime: true);
      e = go.GetComponent<PopupMapDetailMenu>().InitializeAsync(maptownId, list);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().startOpenAnime(go);
      while (Singleton<PopupManager>.GetInstance().isOpen)
        yield return (object) null;
      edit031MenuSlotList.clearWait();
    }
  }

  private void onClickedSelect(int slotId)
  {
    if (this.waitAndSet())
      return;
    this.StartCoroutine(this.doConfirmSave(slotId));
  }

  private IEnumerator doConfirmSave(int slotId)
  {
    MapEdit031MenuSlotList edit031MenuSlotList = this;
    Consts instance = Consts.GetInstance();
    Hashtable args = new Hashtable()
    {
      {
        (object) "name",
        (object) instance.SAVE_SLOT_NAME
      },
      {
        (object) "num",
        (object) (slotId + 1)
      }
    };
    string message = Consts.Format(instance.MAPEDIT_031_MESSAGE_CONFIRM_SAVESLOT, (IDictionary) args);
    if (slotId == edit031MenuSlotList.topMenu_.data_.defaultSlot_)
      message += instance.MAPEDIT_031_NOTE_DEFAULTSLOT;
    if (edit031MenuSlotList.topMenu_.data_.checkModified(slotId))
      message += instance.MAPEDIT_031_NOTE_MODIFIED;
    bool bWait = true;
    bool bOK = false;
    ModalWindow.ShowYesNo(instance.MAPEDIT_031_TITLE_CONFIRM_SAVESLOT, message, (Action) (() =>
    {
      bOK = true;
      bWait = false;
    }), (Action) (() => bWait = false));
    while (bWait)
      yield return (object) null;
    if (bOK)
      edit031MenuSlotList.topMenu_.onChangedSlotAndSave(slotId);
    else
      edit031MenuSlotList.clearWait();
  }

  private void onClickedClose()
  {
    if (this.waitAndSet())
      return;
    this.topMenu_.backSlotList();
  }

  public IEnumerator updateInformation()
  {
    MapEdit031MenuSlotList edit031MenuSlotList = this;
    if (!Object.op_Equality((Object) edit031MenuSlotList.selector_, (Object) null))
    {
      IEnumerator e = edit031MenuSlotList.selector_.updateInformation(SMManager.Get<PlayerGuildTownSlot[]>(), edit031MenuSlotList.topMenu_.data_.editId_, edit031MenuSlotList.topMenu_.data_.defaultSlot_);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }
}
