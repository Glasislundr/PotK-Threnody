// Decompiled with JetBrains decompiler
// Type: MapEditPopupConfirmExchangeMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MapEditPopupConfirmExchangeMap : MonoBehaviour
{
  [SerializeField]
  private UIButton btnOK_;
  [SerializeField]
  private UIButton btnNG_;
  [SerializeField]
  private UILabel txtName_;
  [SerializeField]
  private UILabel txtCost_;
  [SerializeField]
  private UILabel txtDescription_;
  [SerializeField]
  private GuildTownMapScroll mapCheck_;

  public static IEnumerator show(PlayerGuildTown town, Action eventOK, Action eventNG)
  {
    Future<GameObject> ldprefab = MapEdit.Prefabs.popup_confirm_exchange_map.Load<GameObject>();
    IEnumerator e = ldprefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (Object.op_Equality((Object) ldprefab.Result, (Object) null))
    {
      eventNG();
    }
    else
    {
      GameObject go = Singleton<PopupManager>.GetInstance().open(ldprefab.Result, isNonSe: true, isNonOpenAnime: true);
      MapEditPopupConfirmExchangeMap popup = go.GetComponent<MapEditPopupConfirmExchangeMap>();
      e = popup.initialize(town);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      bool ispressed = false;
      EventDelegate.Set(popup.btnOK_.onClick, (EventDelegate.Callback) (() =>
      {
        if (ispressed)
          return;
        ispressed = true;
        eventOK();
      }));
      EventDelegate.Set(popup.btnNG_.onClick, (EventDelegate.Callback) (() =>
      {
        if (ispressed)
          return;
        ispressed = true;
        eventNG();
      }));
      Singleton<PopupManager>.GetInstance().startOpenAnime(go);
      while (!ispressed)
        yield return (object) null;
      Singleton<PopupManager>.GetInstance().dismiss();
      while (go.activeSelf)
        yield return (object) null;
    }
  }

  private IEnumerator initialize(PlayerGuildTown town)
  {
    MapTown master = town.master;
    this.txtName_.SetTextLocalize(master.name);
    this.txtDescription_.SetTextLocalize(master.description);
    this.txtCost_.SetTextLocalize(master.cost_capacity);
    IEnumerator e = this.mapCheck_.InitializeAsync(master.stage_id, (List<Tuple<int, int>>) null);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}
