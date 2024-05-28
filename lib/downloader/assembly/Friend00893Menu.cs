// Decompiled with JetBrains decompiler
// Type: Friend00893Menu
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
public class Friend00893Menu : BackButtonMenuBase
{
  [SerializeField]
  private string[] friend_ids;
  private Action<PlayerFriend[]> callback;

  public void setData(string[] friend_ids, Action<PlayerFriend[]> callback)
  {
    this.friend_ids = friend_ids;
    this.callback = callback;
  }

  private IEnumerator openPopup00894()
  {
    Friend00893Menu friend00893Menu = this;
    Future<GameObject> prefab00895F = Res.Prefabs.popup.popup_008_9_5__anim_popup01.Load<GameObject>();
    IEnumerator e1 = prefab00895F.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    GameObject prefab00895 = prefab00895F.Result;
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.FriendReject> future = WebAPI.FriendReject(friend00893Menu.friend_ids, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<PopupManager>.GetInstance().onDismiss();
      Singleton<PopupManager>.GetInstance().open(prefab00895);
      Singleton<CommonRoot>.GetInstance().isLoading = false;
    }));
    e1 = future.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (future.Result != null)
    {
      if (future.HasResult)
      {
        Singleton<PopupManager>.GetInstance().dismiss();
        Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_9_4__anim_popup01.Load<GameObject>();
        e1 = prefabF.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        GameObject result = prefabF.Result;
        result.GetComponent<Friend00894Menu>().SetRejectNumMessage(friend00893Menu.friend_ids.Length);
        Singleton<PopupManager>.GetInstance().open(result);
        prefabF = (Future<GameObject>) null;
      }
      if (friend00893Menu.callback != null)
      {
        friend00893Menu.callback(((IEnumerable<PlayerFriend>) future.Result.player_friends).ReceivedFriendApplications());
      }
      else
      {
        Debug.LogError((object) "Friend00893Menu callbackがnull");
        friend00893Menu.backScene();
      }
      Singleton<NGGameDataManager>.GetInstance().SetFriendRequestCount();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openPopup00894());
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
