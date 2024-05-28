// Decompiled with JetBrains decompiler
// Type: Friend00819Menu
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
public class Friend00819Menu : FriendBarBase
{
  private bool mode;
  [SerializeField]
  private NGxScroll ScrollContainer;
  [SerializeField]
  private UIButton btnDeselectAll;
  [SerializeField]
  private UIButton btnSelectAll;
  [SerializeField]
  private UIButton btnDecide;
  [SerializeField]
  private UILabel TxtTitle;
  private bool[] lst_is_checked;
  private DateTime now;
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
    Singleton<NGSceneManager>.GetInstance().changeScene("friend008_5", false);
    Singleton<NGSceneManager>.GetInstance().destroyScene("friend008_19");
  }

  public override void onBackButton() => this.IbtnBack();

  private IEnumerator openPopup00893()
  {
    Friend00819Menu friend00819Menu = this;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_9_3__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    Friend00893Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Friend00893Menu>();
    List<string> stringList = new List<string>();
    foreach (FriendBarInfo friendBarInfo in friend00819Menu.allFriendInfo)
    {
      if (friendBarInfo.select)
        stringList.Add(friendBarInfo.friend.target_player_id);
    }
    Singleton<NGGameDataManager>.GetInstance().SetFriendRequestCount();
    // ISSUE: reference to a compiler-generated method
    component.setData(stringList.ToArray(), new Action<PlayerFriend[]>(friend00819Menu.\u003CopenPopup00893\u003Eb__15_0));
  }

  private IEnumerator openPopup008202()
  {
    Friend00819Menu friend00819Menu = this;
    Future<GameObject> prefab008203F = Res.Prefabs.popup.popup_008_20_3__anim_popup01.Load<GameObject>();
    IEnumerator e1 = prefab008203F.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    GameObject prefab008203 = prefab008203F.Result;
    Future<GameObject> prefab0088F = Res.Prefabs.popup.popup_008_8__anim_popup01.Load<GameObject>();
    e1 = prefab0088F.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    GameObject prefab0088 = prefab0088F.Result;
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    List<string> friend_ids = new List<string>();
    foreach (FriendBarInfo friendBarInfo in friend00819Menu.allFriendInfo)
    {
      if (friendBarInfo.select)
        friend_ids.Add(friendBarInfo.friend.target_player_id);
    }
    Future<WebAPI.Response.FriendAccept> future = WebAPI.FriendAccept(friend_ids.ToArray(), (Action<WebAPI.Response.UserError>) (e =>
    {
      if (e.Code.CompareTo("") == 0)
        return;
      Debug.Log((object) e.Code);
      Singleton<PopupManager>.GetInstance().onDismiss();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      if (e.Code.CompareTo("FRD010") == 0)
        Singleton<PopupManager>.GetInstance().open(prefab0088).GetComponent<Friend0088Menu>().InitPopup(e.Reason, new Action(this.RefreshFriendRequests));
      else if (e.Code.CompareTo("FRD013") == 0)
        Singleton<PopupManager>.GetInstance().open(prefab008203);
      else
        WebAPI.DefaultUserErrorCallback(e);
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
        Future<GameObject> prefabF = Res.Prefabs.popup.popup_008_20_2__anim_popup01.Load<GameObject>();
        e1 = prefabF.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        GameObject result = prefabF.Result;
        result.GetComponent<Friend008202Menu>().SetAcceptNumMessage(friend_ids.Count);
        Singleton<PopupManager>.GetInstance().open(result);
        prefabF = (Future<GameObject>) null;
      }
      Singleton<NGGameDataManager>.GetInstance().SetFriendRequestCount();
      PlayerFriend[] friends = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications();
      e1 = friend00819Menu.RefreshScroll(friends);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }
  }

  public IEnumerator RefreshScroll(PlayerFriend[] friends)
  {
    IEnumerator e = this.InitFriendScroll(friends, this.mode);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void RefreshScrollEvent(PlayerFriend[] friends)
  {
    this.StartCoroutine(this.RefreshScroll(friends));
  }

  public void IbtnDecide()
  {
    if (this.IsPushAndSet())
      return;
    if (this.mode)
      this.StartCoroutine(this.openPopup00893());
    else
      this.StartCoroutine(this.openPopup008202());
  }

  public void IbtnDeselectAll()
  {
    if (this.IsPush)
      return;
    this.allFriendInfo.ForEach((Action<FriendBarInfo>) (x => x.select = false));
    foreach (Component component in this.allFriendBar)
      component.GetComponent<Friend00819Scroll>().CheckMarkUpdate();
    this.CheckSelect();
  }

  public void IbtnSelectAll()
  {
    if (this.IsPush)
      return;
    this.allFriendInfo.ForEach((Action<FriendBarInfo>) (x => x.select = true));
    foreach (Component component in this.allFriendBar)
      component.GetComponent<Friend00819Scroll>().CheckMarkUpdate();
    this.CheckSelect();
  }

  public void CheckSelect()
  {
    bool flag = false;
    foreach (FriendBarInfo friendBarInfo in this.allFriendInfo)
    {
      if (friendBarInfo.select)
      {
        flag = true;
        break;
      }
    }
    ((UIButtonColor) this.btnDecide).isEnabled = flag;
  }

  public IEnumerator InitFriendScroll(PlayerFriend[] friends, bool mode)
  {
    Friend00819Menu friend00819Menu = this;
    friend00819Menu.mode = mode;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    friend00819Menu.now = ServerTime.NowAppTime();
    if (mode)
      friend00819Menu.TxtTitle.SetTextLocalize(Consts.GetInstance().FRIEND_00819_MENU_1);
    else
      friend00819Menu.TxtTitle.SetTextLocalize(Consts.GetInstance().FRIEND_00819_MENU_2);
    friend00819Menu.allFriendInfo.Clear();
    friend00819Menu.allFriendBar.Clear();
    Future<GameObject> prefabF = Res.Prefabs.friend008_19.vscroll_810_12.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject friendScrollPrefab = prefabF.Result;
    e = Res.Prefabs.UnitIcon.normal.Load<GameObject>().Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    friend00819Menu.Initialize(friend00819Menu.now, 612, 157, 8, 5);
    friend00819Menu.CreateFriendInfo(friends);
    friend00819Menu.CheckSelect();
    if (friend00819Menu.allFriendInfo.Count > 0)
    {
      e = friend00819Menu.CreateScrollBase(friendScrollPrefab);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    friend00819Menu.scroll.ResolvePosition();
    friend00819Menu.InitializeEnd();
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
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
    FriendScrollBar friendScrollBar = this.allFriendBar[bar_index];
    FriendBarInfo info = this.allFriendInfo[info_index];
    ((Friend00819Scroll) friendScrollBar).Set(info.select, this.now, info, this);
  }

  private void RefreshFriendRequests() => this.StartCoroutine(this.RefreshFrendRequestsImpl());

  private IEnumerator RefreshFrendRequestsImpl()
  {
    IEnumerator e = this.ReloadFriendRequestsData();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.RefreshScroll(((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator ReloadFriendRequestsData()
  {
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = WebAPI.FriendFriends().Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }
}
