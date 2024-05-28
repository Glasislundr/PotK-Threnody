// Decompiled with JetBrains decompiler
// Type: Friend0081Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Friend0081Scene : NGSceneBase
{
  [SerializeField]
  private UILabel TxtNumber;
  [SerializeField]
  private GameObject BadgeIcon;
  [SerializeField]
  private Friend0081Menu menu;
  private int friendCnt;
  private GameObject badgeGo;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_1", true);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Future<GameObject> badgeIconF = Res.Prefabs.BadgeIcon.prefab.Load<GameObject>();
    IEnumerator e = badgeIconF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.badgeGo = badgeIconF.Result.Clone();
    this.badgeGo.SetParent(this.BadgeIcon);
    e = WebAPI.FriendFriends().Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.friendCnt = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends().Length;
  }

  public IEnumerator onStartSceneAsync()
  {
    this.UpdateInfomation();
    IEnumerator e = this.menu.InitFriendScroll(((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.friendCnt = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends().Length;
  }

  public IEnumerator onBackSceneAsync()
  {
    this.UpdateInfomation();
    PlayerFriend[] friends = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends();
    IEnumerator e;
    if (this.friendCnt == friends.Length)
    {
      e = this.menu.UpdateList();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.menu.InitFriendScroll(friends);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    this.friendCnt = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends().Length;
  }

  public void onStartScene()
  {
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public override void onEndScene() => Persist.sortOrder.Flush();

  private void UpdateInfomation()
  {
    Player player = SMManager.Get<Player>();
    PlayerFriend[] playerFriendArray1 = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends();
    PlayerFriend[] playerFriendArray2 = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications();
    this.menu.DirNoFriend.SetActive(playerFriendArray1.Length == 0);
    this.TxtNumber.SetTextLocalize(playerFriendArray1.Length.ToString("D3") + "/" + player.max_friends.ToString("D3"));
    ButtonBadge component = this.badgeGo.GetComponent<ButtonBadge>();
    if (playerFriendArray2 != null)
    {
      Singleton<NGGameDataManager>.GetInstance().ReceivedFriendRequestCount = playerFriendArray2.Length;
      component.set(playerFriendArray2.Length);
    }
    else
    {
      Singleton<NGGameDataManager>.GetInstance().ReceivedFriendRequestCount = 0;
      component.set(0);
    }
  }
}
