// Decompiled with JetBrains decompiler
// Type: GuildAtkGuestSelectPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class GuildAtkGuestSelectPopup : GuildGuestListBase
{
  private const int Width = 616;
  private const int Height = 154;
  private const int ColumnValue = 1;
  private const int RowValue = 11;
  private const int ScreenValue = 6;
  private GuildBattleSortiePopup sortiePopup;
  private GvgCandidate[] friends;
  private GuildBattlePreparationPopup parent;
  private Guild0282Menu guild0282Menu;

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    GuildAtkGuestSelectPopup guestSelectPopup = this;
    if (bar_index < guestSelectPopup.allGuestBar.Count && info_index < guestSelectPopup.allGuestInfo.Count)
    {
      GuildGuestSelectScroll scrollParts = guestSelectPopup.allGuestBar[bar_index];
      GuestBarInfo guestBarInfo = guestSelectPopup.allGuestInfo[info_index];
      guestBarInfo.scrollParts = scrollParts;
      // ISSUE: reference to a compiler-generated method
      IEnumerator e = scrollParts.InitializeAsync(guestBarInfo.friend, GuildGuestSelectScroll.MODE.Atk, new Action<GvgCandidate>(guestSelectPopup.\u003CCreateScroll\u003Eb__9_0));
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) scrollParts).gameObject.SetActive(true);
    }
  }

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public IEnumerator InitializeAsync(
    Guild0282Menu menu,
    GuildBattlePreparationPopup parent,
    GuildBattleSortiePopup sortiePopup,
    GvgCandidate[] friends)
  {
    this.guild0282Menu = menu;
    this.parent = parent;
    this.friends = friends;
    this.sortiePopup = sortiePopup;
    IEnumerator e = this.InitFriendListScroll(friends);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void SetGvgPopup()
  {
    this.guild0282Menu.SetGvgPopup(GuildUtil.GvGPopupState.Sortie, (Action) (() =>
    {
      ((Component) this).gameObject.SetActive(true);
      this.IsPush = false;
    }));
  }

  public IEnumerator InitFriendListScroll(GvgCandidate[] friends)
  {
    GuildAtkGuestSelectPopup guestSelectPopup = this;
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
  }

  public void InitScrollPosition()
  {
    this.scroll.ResolvePosition();
    if (this.IsInitialize)
      return;
    this.InitializeEnd();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet() || !((Component) this).gameObject.activeSelf || this.parent.Mode != GuildBattlePreparationPopup.MODE.Guest)
      return;
    GuildUtil.gvgFriendAttack = (GvgCandidate) null;
    this.guild0282Menu.closePopup();
  }
}
