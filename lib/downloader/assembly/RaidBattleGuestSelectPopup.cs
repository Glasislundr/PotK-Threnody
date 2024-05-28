// Decompiled with JetBrains decompiler
// Type: RaidBattleGuestSelectPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class RaidBattleGuestSelectPopup : GuildGuestListBase
{
  public UIButton[] elementBtn;
  private static CommonElement currentElement = CommonElement.none;
  private static int currentTabIndex = 0;
  private GuildRaidPlayerHelpers[] noneElementRentalUnits;
  private Dictionary<CommonElement, GuildRaidPlayerHelpers[]> elementDic = new Dictionary<CommonElement, GuildRaidPlayerHelpers[]>();
  private Dictionary<CommonElement, string[]> usedUnitDic = new Dictionary<CommonElement, string[]>();
  private const int Width = 616;
  private const int Height = 184;
  private const int ColumnValue = 1;
  private const int RowValue = 11;
  private const int ScreenValue = 6;
  private RaidBattlePreparationPopup parent;

  public static int GetSelectedCategoryId()
  {
    return RaidBattleGuestSelectPopup.currentElement != CommonElement.none ? (int) RaidBattleGuestSelectPopup.currentElement : 0;
  }

  private void SetElementBtnState(int index)
  {
    for (int index1 = 0; index1 < this.elementBtn.Length; ++index1)
    {
      SpreadColorButton component = ((Component) this.elementBtn[index1]).gameObject.GetComponent<SpreadColorButton>();
      if (index1 != index)
      {
        component.SetColor(Color.gray);
        ((UIWidget) ((Component) this.elementBtn[index1]).gameObject.GetComponentInChildren<UITexture>(true)).color = Color.gray;
      }
      else
      {
        component.SetColor(Color.white);
        ((UIWidget) ((Component) this.elementBtn[index1]).gameObject.GetComponentInChildren<UITexture>(true)).color = Color.white;
      }
    }
  }

  private IEnumerator ChangeElement(CommonElement type, int index)
  {
    RaidBattleGuestSelectPopup guestSelectPopup = this;
    RaidBattleGuestSelectPopup.currentElement = type;
    RaidBattleGuestSelectPopup.currentTabIndex = index;
    guestSelectPopup.SetElementBtnState(index);
    IEnumerator coroutine = guestSelectPopup.GetElementHelpers();
    yield return (object) guestSelectPopup.StartCoroutine(coroutine);
    GuildRaidPlayerHelpers[] current = (GuildRaidPlayerHelpers[]) coroutine.Current;
    yield return (object) guestSelectPopup.StartCoroutine(guestSelectPopup.InitFriendListScroll(current));
    guestSelectPopup.InitScrollPosition();
  }

  public IEnumerator OnClickElementBtn(CommonElement type, int index)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RaidBattleGuestSelectPopup guestSelectPopup = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    guestSelectPopup.StartCoroutine(guestSelectPopup.ChangeElement(type, index));
    return false;
  }

  public IEnumerator InitializeAsync(
    RaidBattlePreparationPopup parent,
    GuildRaidPlayerHelpers[] helpers,
    string[] usedHelpers)
  {
    RaidBattleGuestSelectPopup guestSelectPopup = this;
    guestSelectPopup.parent = parent;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(guestSelectPopup.elementBtn[0].onClick, new EventDelegate.Callback(guestSelectPopup.\u003CInitializeAsync\u003Eb__16_0));
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(guestSelectPopup.elementBtn[1].onClick, new EventDelegate.Callback(guestSelectPopup.\u003CInitializeAsync\u003Eb__16_1));
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(guestSelectPopup.elementBtn[2].onClick, new EventDelegate.Callback(guestSelectPopup.\u003CInitializeAsync\u003Eb__16_2));
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(guestSelectPopup.elementBtn[3].onClick, new EventDelegate.Callback(guestSelectPopup.\u003CInitializeAsync\u003Eb__16_3));
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(guestSelectPopup.elementBtn[4].onClick, new EventDelegate.Callback(guestSelectPopup.\u003CInitializeAsync\u003Eb__16_4));
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(guestSelectPopup.elementBtn[5].onClick, new EventDelegate.Callback(guestSelectPopup.\u003CInitializeAsync\u003Eb__16_5));
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Set(guestSelectPopup.elementBtn[6].onClick, new EventDelegate.Callback(guestSelectPopup.\u003CInitializeAsync\u003Eb__16_6));
    guestSelectPopup.SetElementBtnState(RaidBattleGuestSelectPopup.currentTabIndex);
    guestSelectPopup.elementDic.Add(RaidBattleGuestSelectPopup.currentElement, helpers);
    guestSelectPopup.usedUnitDic.Add(RaidBattleGuestSelectPopup.currentElement, usedHelpers);
    IEnumerator e = guestSelectPopup.InitFriendListScroll(helpers);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Show()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RaidBattleGuestSelectPopup guestSelectPopup = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    ((Component) guestSelectPopup).gameObject.SetActive(true);
    guestSelectPopup.InitScrollPosition();
    return false;
  }

  public void Hide() => ((Component) this).gameObject.SetActive(false);

  private IEnumerator GetElementHelpers()
  {
    RaidBattleGuestSelectPopup guestSelectPopup = this;
    if (!guestSelectPopup.elementDic.ContainsKey(RaidBattleGuestSelectPopup.currentElement))
    {
      Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
      Future<WebAPI.Response.GuildraidBattleHelper> ft = WebAPI.GuildraidBattleHelper(RaidBattleGuestSelectPopup.GetSelectedCategoryId(), new Action<WebAPI.Response.UserError>(guestSelectPopup.webErrorCallback));
      IEnumerator e = ft.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (ft.Result == null)
        Debug.LogError((object) "helpers.Result is null");
      WebAPI.Response.GuildraidBattleHelper result = ft.Result;
      guestSelectPopup.usedUnitDic.Add(RaidBattleGuestSelectPopup.currentElement, result.used_helpers);
      guestSelectPopup.elementDic.Add(RaidBattleGuestSelectPopup.currentElement, result.helpers);
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      yield return (object) result.helpers;
      ft = (Future<WebAPI.Response.GuildraidBattleHelper>) null;
    }
    else
      yield return (object) guestSelectPopup.elementDic[RaidBattleGuestSelectPopup.currentElement];
  }

  private void webErrorCallback(WebAPI.Response.UserError error)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string title;
    string message;
    if (error.Code == "GRB016")
    {
      title = Consts.GetInstance().GUILD_RAID_BOSS_WAS_ALREADY_DIED_TITLE;
      message = Consts.GetInstance().GUILD_RAID_BOSS_WAS_ALREADY_DIED_MESSAGE;
    }
    else
    {
      title = error.Code;
      message = error.Reason;
    }
    ModalWindow.Show(title, message, (Action) (() =>
    {
      this.IsPush = false;
      Singleton<NGSceneManager>.GetInstance();
      if (error.Code == "GRB016")
        this.onBackButton();
      else
        MypageScene.ChangeSceneOnError();
    }));
  }

  public IEnumerator InitFriendListScroll(GuildRaidPlayerHelpers[] helpers)
  {
    RaidBattleGuestSelectPopup guestSelectPopup = this;
    guestSelectPopup.allGuestInfo.Clear();
    guestSelectPopup.allGuestBar.Clear();
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime nowTime = ServerTime.NowAppTime();
    guestSelectPopup.Initialize(nowTime, 616, 184, 11, 6);
    List<GuildRaidPlayerHelpers> raidPlayerHelpersList = new List<GuildRaidPlayerHelpers>();
    if (guestSelectPopup.usedUnitDic.ContainsKey(RaidBattleGuestSelectPopup.currentElement))
    {
      string[] source = guestSelectPopup.usedUnitDic[RaidBattleGuestSelectPopup.currentElement];
      for (int index = 0; index < helpers.Length; ++index)
      {
        if (!((IEnumerable<string>) source).Contains<string>(helpers[index].target_player_id))
          raidPlayerHelpersList.Add(helpers[index]);
      }
      guestSelectPopup.CreateFriendInfo(raidPlayerHelpersList.ToArray());
    }
    else
      guestSelectPopup.CreateFriendInfo(helpers);
    if (guestSelectPopup.allGuestInfo.Count > 0)
    {
      Future<GameObject> prefabF = Res.Prefabs.guild028_2.raid_battle_friend_list.Load<GameObject>();
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

  protected override void Update()
  {
    base.Update();
    this.ScrollUpdate();
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.parent.OnBackButton();
  }

  protected override IEnumerator CreateScroll(int info_index, int bar_index)
  {
    RaidBattleGuestSelectPopup guestSelectPopup = this;
    if (bar_index < guestSelectPopup.allGuestBar.Count && info_index < guestSelectPopup.allGuestInfo.Count)
    {
      GuildGuestSelectScroll scrollParts = guestSelectPopup.allGuestBar[bar_index];
      GuestBarInfo guestBarInfo = guestSelectPopup.allGuestInfo[info_index];
      guestBarInfo.scrollParts = scrollParts;
      IEnumerator e = scrollParts.InitializeAsync(guestBarInfo.friend, GuildGuestSelectScroll.MODE.Atk, new Action<GvgCandidate>(guestSelectPopup.onGuestDecided), true);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ((Component) scrollParts).gameObject.SetActive(true);
    }
  }

  protected void onGuestDecided(GvgCandidate friend)
  {
    if (this.IsPushAndSet())
      return;
    this.parent.OnGuestDecided(friend);
  }
}
