// Decompiled with JetBrains decompiler
// Type: GuildInfoPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildInfoPopup
{
  public GameObject guildInfoPopup;
  public GameObject guildSendRequestPopup;
  public GameObject guildSendRequestResultPopup;
  public GameObject guildCancelRequestPopup;
  public GameObject guildCancelRequestResultPopup;
  public GameObject guildStatementPopup;
  public GameObject guildNgWordPopup;
  public GameObject guildFriendListPopup;
  public GameObject guildFriendListParts;
  private Action sendRequestCallback;
  private Action cancelRequestCallback;
  private Action requestMaintenanceCallback;
  private Action requestAlreadyGuildCallback;

  public IEnumerator ResourceLoad()
  {
    Future<GameObject> fgObj;
    IEnumerator e;
    if (Object.op_Equality((Object) this.guildInfoPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_1_4__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildInfoPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildSendRequestPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_1_5__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildSendRequestPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildSendRequestResultPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_1_5_1__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildSendRequestResultPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildCancelRequestPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_1_6__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildCancelRequestPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildCancelRequestResultPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_1_6_1__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildCancelRequestResultPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildStatementPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_1_1_7__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildStatementPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    if (Object.op_Equality((Object) this.guildNgWordPopup, (Object) null))
    {
      fgObj = Res.Prefabs.popup.popup_028_guild_ng_word__anim_popup01.Load<GameObject>();
      e = fgObj.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.guildNgWordPopup = fgObj.Result;
      fgObj = (Future<GameObject>) null;
    }
    Object.op_Equality((Object) this.guildFriendListPopup, (Object) null);
    Object.op_Equality((Object) this.guildFriendListParts, (Object) null);
  }

  public Action SendRequestCallback => this.sendRequestCallback;

  public Action CancelRequeestCallback => this.cancelRequestCallback;

  public void SetSendRequestCallback(Action action) => this.sendRequestCallback = action;

  public void SetCancelRequestCallback(Action action) => this.cancelRequestCallback = action;

  public Action RequestMaintenanceCallback => this.requestMaintenanceCallback;

  public void SetRequestMaintenanceCallback(Action action)
  {
    this.requestMaintenanceCallback = action;
  }

  public Action RequestAlreadyGuildCallback => this.requestAlreadyGuildCallback;

  public void SetRequestAlreadyGuildCallback(Action action)
  {
    this.requestAlreadyGuildCallback = action;
  }
}
