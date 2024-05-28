// Decompiled with JetBrains decompiler
// Type: GuildDefGuestSelectPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildDefGuestSelectPopup : GuildGuestListBase
{
  private const int Width = 616;
  private const int Height = 154;
  private const int ColumnValue = 1;
  private const int RowValue = 11;
  private const int ScreenValue = 6;
  private GuildDefTeamPopup parent;
  private GuildDefTeamEditPopup teamEditPopup;
  private GvgCandidate[] friends;

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public IEnumerator InitializeAsync(GuildDefTeamPopup parent, GuildDefTeamEditPopup editPopup)
  {
    this.parent = parent;
    this.teamEditPopup = editPopup;
    yield break;
  }

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    GuildDefGuestSelectPopup guestSelectPopup = this;
    if (bar_index < guestSelectPopup.allGuestBar.Count && info_index < guestSelectPopup.allGuestInfo.Count)
    {
      GuildGuestSelectScroll scrollParts = guestSelectPopup.allGuestBar[bar_index];
      GuestBarInfo guestBarInfo = guestSelectPopup.allGuestInfo[info_index];
      guestBarInfo.scrollParts = scrollParts;
      // ISSUE: reference to a compiler-generated method
      IEnumerator e = scrollParts.InitializeAsync(guestBarInfo.friend, GuildGuestSelectScroll.MODE.Def, new Action<GvgCandidate>(guestSelectPopup.\u003CCreateScroll\u003Eb__10_0));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) scrollParts).gameObject.SetActive(true);
    }
  }

  public IEnumerator InitFriendListScroll(GvgCandidate[] friends)
  {
    GuildDefGuestSelectPopup guestSelectPopup = this;
    guestSelectPopup.allGuestInfo.Clear();
    guestSelectPopup.allGuestBar.Clear();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime nowTime = ServerTime.NowAppTime();
    guestSelectPopup.Initialize(nowTime, 616, 154, 11, 6);
    guestSelectPopup.CreateFriendInfo(friends);
    if (guestSelectPopup.allGuestInfo.Count > 0)
    {
      Future<GameObject> prefabF = Res.Prefabs.guild028_2.dir_guild_battle_friend_list.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = prefabF.Result;
      e = guestSelectPopup.CreateScrollBase(result);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      prefabF = (Future<GameObject>) null;
    }
    guestSelectPopup.scroll.ResolvePosition();
    guestSelectPopup.scroll.scrollView.UpdatePosition();
    guestSelectPopup.InitializeEnd();
  }

  public IEnumerator CreateFriendList(GvgCandidate[] friends)
  {
    this.friends = friends;
    IEnumerator e = this.InitFriendListScroll(friends);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.InitScrollPosition();
  }

  public void InitScrollPosition() => this.scroll.scrollView.ResetPosition();

  public override void onBackButton()
  {
    if (this.IsPushAndSet() || !((Component) this).gameObject.activeSelf || this.parent.Mode != GuildDefTeamPopup.MODE.Guest)
      return;
    this.parent.ShowTeamEdit();
  }
}
