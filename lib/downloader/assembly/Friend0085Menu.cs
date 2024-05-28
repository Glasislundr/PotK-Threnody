// Decompiled with JetBrains decompiler
// Type: Friend0085Menu
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
public class Friend0085Menu : FriendBarBase
{
  [SerializeField]
  private GameObject DirBulk;
  [SerializeField]
  private GameObject DirNofriend;
  private Modified<PlayerFriend[]> friendsM;
  private const int Width = 612;
  private const int Height = 157;
  private const int ColumnValue = 1;
  private const int RowValue = 8;
  private const int ScreenValue = 5;

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Singleton<NGSceneManager>.GetInstance().backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnBulkApproval()
  {
    if (this.IsPushAndSet())
      return;
    Friend00819Scene.ChangeSceneApproval(false);
    Singleton<NGSceneManager>.GetInstance().destroyScene("friend008_5");
  }

  public void IbtnBulkDenial()
  {
    if (this.IsPushAndSet())
      return;
    Friend00819Scene.ChangeSceneDenial(false);
    Singleton<NGSceneManager>.GetInstance().destroyScene("friend008_5");
  }

  public IEnumerator InitFriendScroll()
  {
    Friend0085Menu friend0085Menu = this;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime now = ServerTime.NowAppTime();
    friend0085Menu.friendsM = SMManager.Observe<PlayerFriend[]>();
    friend0085Menu.friendsM.NotifyChanged();
    PlayerFriend[] friends = ((IEnumerable<PlayerFriend>) friend0085Menu.friendsM.Value).ReceivedFriendApplications();
    friend0085Menu.allFriendInfo.Clear();
    friend0085Menu.allFriendBar.Clear();
    Future<GameObject> prefabScrollF = Res.Prefabs.friend008_5.friendScroll.Load<GameObject>();
    e = prefabScrollF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabScrollF.Result;
    friend0085Menu.Initialize(now, 612, 157, 8, 5);
    friend0085Menu.CreateFriendInfo(friends);
    if (friend0085Menu.allFriendInfo.Count > 0)
    {
      e = friend0085Menu.CreateScrollBase(result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) friend0085Menu.DirBulk.transform).gameObject.SetActive(true);
      friend0085Menu.DirNofriend.SetActive(false);
    }
    else
    {
      ((Component) friend0085Menu.DirBulk.transform).gameObject.SetActive(false);
      friend0085Menu.DirNofriend.SetActive(true);
    }
    friend0085Menu.scroll.ResolvePosition();
    friend0085Menu.InitializeEnd();
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public IEnumerator ResetScroll()
  {
    IEnumerator e = this.InitFriendScroll();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void ResetScrollEvent() => this.StartCoroutine(this.ResetScroll());

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
    ((FriendScrollParts) this.allFriendBar[bar_index]).Set(this);
  }
}
