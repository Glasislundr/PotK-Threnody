// Decompiled with JetBrains decompiler
// Type: MapEditSaveSlotSelect
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
public class MapEditSaveSlotSelect : MonoBehaviour
{
  [SerializeField]
  private NGxScroll scroll_;
  [SerializeField]
  private UIButton btnClose_;
  private List<MapEditSaveSlotList> slotList_ = new List<MapEditSaveSlotList>();
  private bool initialized_;

  public IEnumerator initialize(
    PlayerGuildTownSlot[] slots,
    int current,
    int defaultSlot,
    Action<int, PlayerGuildTownSlotPosition[]> eventMapDetail,
    Action<int> eventSelect,
    Action eventClose)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MapEditSaveSlotSelect.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new MapEditSaveSlotSelect.\u003C\u003Ec__DisplayClass4_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.eventClose = eventClose;
    this.initialized_ = false;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(this.btnClose_.onClick, new EventDelegate.Callback(cDisplayClass40.\u003Cinitialize\u003Eb__0));
    if (slots == null)
    {
      this.initialized_ = true;
    }
    else
    {
      Future<GameObject> ldprefab = MapEdit.Prefabs.map_slot_list.Load<GameObject>();
      IEnumerator e = ldprefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (Object.op_Equality((Object) ldprefab.Result, (Object) null))
      {
        this.initialized_ = true;
      }
      else
      {
        Future<GameObject> ldMapIcon = Res.Icons.UniqueIconPrefab.Load<GameObject>();
        e = ldMapIcon.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (Object.op_Equality((Object) ldMapIcon.Result, (Object) null))
        {
          this.initialized_ = true;
        }
        else
        {
          this.scroll_.Reset();
          this.slotList_.Clear();
          PlayerGuildTownSlot[] playerGuildTownSlotArray = slots;
          for (int index = 0; index < playerGuildTownSlotArray.Length; ++index)
          {
            PlayerGuildTownSlot slot = playerGuildTownSlotArray[index];
            GameObject gameObject = ldprefab.Result.Clone();
            this.scroll_.Add(gameObject, true);
            MapEditSaveSlotList component = gameObject.GetComponent<MapEditSaveSlotList>();
            this.slotList_.Add(component);
            e = component.initialize(ldMapIcon.Result, slot, slot.slot_number == current, slot.slot_number == defaultSlot, eventMapDetail, eventSelect);
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
          }
          playerGuildTownSlotArray = (PlayerGuildTownSlot[]) null;
          // ISSUE: method pointer
          this.scroll_.GridReposition(new UIGrid.OnReposition((object) cDisplayClass40, __methodptr(\u003Cinitialize\u003Eb__1)));
          this.initialized_ = true;
        }
      }
    }
  }

  public IEnumerator updateInformation(PlayerGuildTownSlot[] slots, int current, int defaultSlot)
  {
    if (slots != null)
    {
      this.initialized_ = false;
      PlayerGuildTownSlot[] playerGuildTownSlotArray = slots;
      for (int index = 0; index < playerGuildTownSlotArray.Length; ++index)
      {
        PlayerGuildTownSlot slot = playerGuildTownSlotArray[index];
        MapEditSaveSlotList editSaveSlotList = this.slotList_.FirstOrDefault<MapEditSaveSlotList>((Func<MapEditSaveSlotList, bool>) (s => s.slotId_ == slot.slot_number));
        if (!Object.op_Equality((Object) editSaveSlotList, (Object) null))
        {
          IEnumerator e = editSaveSlotList.updateInformation(slot, slot.slot_number == current, slot.slot_number == defaultSlot);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      playerGuildTownSlotArray = (PlayerGuildTownSlot[]) null;
      this.initialized_ = true;
    }
  }
}
