// Decompiled with JetBrains decompiler
// Type: Friend0081Menu
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
public class Friend0081Menu : FriendBarBase
{
  private List<PlayerFriend> friendList = new List<PlayerFriend>();
  public GameObject DirNoFriend;
  private const int Width = 612;
  private const int Height = 157;
  private const int ColumnValue = 1;
  private const int RowValue = 8;
  private const int ScreenValue = 5;
  private int lastIndex;
  private GameObject prefabScroll;

  public int LastIndex
  {
    set => this.lastIndex = value;
    get => this.lastIndex;
  }

  private IEnumerator BackSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Friend0081Menu friend0081Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    friend0081Menu.backScene();
    return false;
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public void IbtnInformation()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_5", true);
  }

  public void IbtnSearch()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_10", true);
  }

  public void InviteFriend()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_20", true);
  }

  public void IbtnSendMessage()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_17", true);
  }

  public void IbtnSort()
  {
    if (this.IsPush)
      return;
    try
    {
      Persist.sortOrder.Data.Friend = ++Persist.sortOrder.Data.Friend % 3;
    }
    catch (Exception ex)
    {
      Persist.sortOrder.Delete();
      Persist.sortOrder.Data.Friend = ++Persist.sortOrder.Data.Friend % 3;
    }
    this.SortFriendsData();
  }

  public IEnumerator UpdateList()
  {
    Friend0081Menu friend0081Menu = this;
    foreach (PlayerFriend friend1 in ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends())
    {
      PlayerFriend friend = friend1;
      FriendBarInfo friendBarInfo = friend0081Menu.allFriendInfo.Where<FriendBarInfo>((Func<FriendBarInfo, bool>) (x => x.friend.target_player_id == friend.target_player_id)).FirstOrDefault<FriendBarInfo>();
      if (friendBarInfo != null)
        friendBarInfo.friend = friend;
    }
    friend0081Menu.allFriendInfo = friend0081Menu.allFriendInfo.SortBy().ToList<FriendBarInfo>();
    for (int index = 0; index < friend0081Menu.allFriendInfo.Count; ++index)
      friend0081Menu.allFriendInfo[index].index = index;
    friend0081Menu.scroll.Clear();
    if (friend0081Menu.allFriendInfo.Count > 0)
    {
      IEnumerator e = friend0081Menu.CreateScrollBase(friend0081Menu.prefabScroll);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      friend0081Menu.DirNoFriend.SetActive(true);
    friend0081Menu.scroll.ResolvePosition(friend0081Menu.lastIndex, friend0081Menu.allFriendInfo.Count);
    friend0081Menu.lastIndex = 0;
    friend0081Menu.isUpdate = true;
  }

  public IEnumerator InitFriendScroll(PlayerFriend[] friends)
  {
    Friend0081Menu friend0081Menu = this;
    friend0081Menu.friendList.Clear();
    friend0081Menu.allFriendInfo.Clear();
    friend0081Menu.allFriendBar.Clear();
    friend0081Menu.DirNoFriend.SetActive(false);
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTime();
    if (Object.op_Equality((Object) friend0081Menu.prefabScroll, (Object) null))
    {
      Future<GameObject> prefabScrollF = Res.Prefabs.friend008_1._0081vscrollparts.Load<GameObject>();
      e = prefabScrollF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      friend0081Menu.prefabScroll = prefabScrollF.Result;
      prefabScrollF = (Future<GameObject>) null;
    }
    friend0081Menu.Initialize(now, 612, 157, 8, 5);
    friend0081Menu.SetSortLabel();
    friend0081Menu.CreateFriendInfo(friends);
    if (friend0081Menu.allFriendInfo.Count > 0)
    {
      e = friend0081Menu.CreateScrollBase(friend0081Menu.prefabScroll);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      friend0081Menu.DirNoFriend.SetActive(true);
    friend0081Menu.scroll.ResolvePosition();
    friend0081Menu.InitializeEnd();
  }

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    IEnumerator e = base.CreateScroll(info_index, bar_index);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.FriendScrollAction(info_index, bar_index);
  }

  private void FriendScrollAction(int info_index, int bar_index)
  {
    ((Friend0081ScrollParts) this.allFriendBar[bar_index]).Set(info_index, this);
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public override void onBackButton() => this.IbtnBack();
}
