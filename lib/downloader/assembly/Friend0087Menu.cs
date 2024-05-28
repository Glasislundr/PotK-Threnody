// Decompiled with JetBrains decompiler
// Type: Friend0087Menu
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
public class Friend0087Menu : BackButtonMenuBase
{
  private List<string> target_friend_ids = new List<string>();
  private Action callBack;

  public void IsBack(bool is_back)
  {
  }

  public void Init0087PopUp(string id, Action callback = null)
  {
    this.target_friend_ids.Add(id);
    this.callBack = callback;
  }

  private IEnumerator openPopup00872(string friendName)
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_7_2__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Friend00872Menu>().SetTxtDescription1Left(Consts.Format(Consts.GetInstance().Friend0087Menu_SetTxtDescription1Left, (IDictionary) new Hashtable()
    {
      {
        (object) nameof (friendName),
        (object) friendName
      }
    }));
  }

  private IEnumerator openPopup0088(string reason)
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_8__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    Singleton<PopupManager>.GetInstance().openAlert(result).GetComponent<Friend0088Menu>().SetMessage(reason);
  }

  private IEnumerator FriendAcceptAsync()
  {
    Friend0087Menu friend0087Menu1 = this;
    Singleton<PopupManager>.GetInstance().onDismiss(true);
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.FriendAccept> future = WebAPI.FriendAccept(friend0087Menu1.target_friend_ids.ToArray(), new Action<WebAPI.Response.UserError>(friend0087Menu1.\u003CFriendAcceptAsync\u003Eb__6_0));
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (future.Result != null)
    {
      if (future.HasResult)
      {
        Friend0087Menu friend0087Menu = friend0087Menu1;
        if (friend0087Menu1.callBack != null)
        {
          friend0087Menu1.callBack();
          yield return (object) new WaitForSeconds(0.5f);
        }
        Singleton<PopupManager>.GetInstance().onDismiss(true);
        string playerName = "";
        ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ForEach<PlayerFriend>((Action<PlayerFriend>) (fr =>
        {
          if (friend0087Menu.target_friend_ids.Count <= 0 || fr.target_player_id.CompareTo(friend0087Menu.target_friend_ids[0]) != 0)
            return;
          playerName = fr.target_player_name;
        }));
        friend0087Menu1.UpdateReceivedFriend();
        friend0087Menu1.StartCoroutine(friend0087Menu1.openPopup00872(playerName));
      }
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  private void UpdateReceivedFriend()
  {
    SMManager.Get<Player>();
    ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends();
    PlayerFriend[] playerFriendArray = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications();
    if (playerFriendArray != null)
      Singleton<NGGameDataManager>.GetInstance().ReceivedFriendRequestCount = playerFriendArray.Length;
    else
      Singleton<NGGameDataManager>.GetInstance().ReceivedFriendRequestCount = 0;
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.FriendAcceptAsync());
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}
