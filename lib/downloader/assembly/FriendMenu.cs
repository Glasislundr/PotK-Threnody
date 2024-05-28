// Decompiled with JetBrains decompiler
// Type: FriendMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class FriendMenu : ResultMenuBase
{
  private int incr_friend_point;
  private PlayerHelper helper;
  private Gladiator gladiator;
  private bool isFriend;
  private bool isGladiator;
  private bool isHelper;
  private Action questAutoLapSkipAction;
  [SerializeField]
  private UIButton touchToNext;
  private bool toNext;

  public override IEnumerator Init(BattleInfo info, BattleEnd result)
  {
    this.helper = result.battle_helpers[0];
    this.incr_friend_point = result.incr_friend_point;
    this.isFriend = this.helper.is_friend;
    this.isGladiator = false;
    this.isHelper = true;
    yield break;
  }

  public override IEnumerator Init(
    ColosseumUtility.Info info,
    ResultMenuBase.Param param,
    Gladiator gladiator)
  {
    this.gladiator = gladiator;
    this.incr_friend_point = -1;
    this.isFriend = false;
    this.isGladiator = true;
    this.isHelper = false;
    yield break;
  }

  public override IEnumerator Init(WebAPI.Response.PvpPlayerFinish info)
  {
    this.helper = info.gladiators[0];
    this.incr_friend_point = -1;
    this.isFriend = false;
    this.isGladiator = true;
    this.isHelper = true;
    yield break;
  }

  public override IEnumerator Init(ResultMenuBase.Param param, Gladiator gladiator)
  {
    this.gladiator = gladiator;
    this.incr_friend_point = -1;
    this.isFriend = false;
    this.isGladiator = true;
    this.isHelper = false;
    yield break;
  }

  public override IEnumerator Run()
  {
    IEnumerator e;
    if (this.isGladiator)
    {
      e = this.popup02349();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else if (this.isFriend)
    {
      e = this.popup0202502();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.popup0202501();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    while (!this.toNext)
    {
      if (Singleton<NGGameDataManager>.GetInstance().questAutoLap && this.questAutoLapSkipAction != null)
        this.questAutoLapSkipAction();
      yield return (object) null;
    }
  }

  public override IEnumerator OnFinish()
  {
    yield break;
  }

  private IEnumerator popup0202501()
  {
    FriendMenu friendMenu = this;
    Future<GameObject> prefab = Res.Prefabs.battle.popup_020_25_01__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    FriendRequestPopupBase o = friendMenu.OpenPopup(prefab.Result).GetComponent<FriendRequestPopupBase>();
    e = o.Init(friendMenu.helper, friendMenu.incr_friend_point);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    o.SetCallback(new Action(friendMenu.\u003Cpopup0202501\u003Eb__15_0));
    friendMenu.questAutoLapSkipAction = new Action(o.IbtnNo);
  }

  private IEnumerator popup0202502()
  {
    FriendMenu friendMenu = this;
    Future<GameObject> prefab = Res.Prefabs.battle.popup_020_25_02__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Battle0202502Menu o = friendMenu.OpenPopup(prefab.Result).GetComponent<Battle0202502Menu>();
    e = o.Init(friendMenu.helper, friendMenu.incr_friend_point);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Add(o.OK.onClick, new EventDelegate.Callback(friendMenu.\u003Cpopup0202502\u003Eb__16_0));
    // ISSUE: reference to a compiler-generated method
    friendMenu.questAutoLapSkipAction = new Action(friendMenu.\u003Cpopup0202502\u003Eb__16_1);
  }

  private IEnumerator popup02349()
  {
    FriendMenu friendMenu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_023_4_9__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    FriendRequestPopupBase o = friendMenu.OpenPopup(prefab.Result).GetComponent<FriendRequestPopupBase>();
    if (friendMenu.isHelper)
    {
      e = o.Init(friendMenu.helper, -1);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = o.Init(friendMenu.gladiator);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    // ISSUE: reference to a compiler-generated method
    o.SetCallback(new Action(friendMenu.\u003Cpopup02349\u003Eb__17_0));
    friendMenu.questAutoLapSkipAction = new Action(o.IbtnNo);
  }
}
