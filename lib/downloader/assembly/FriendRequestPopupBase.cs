// Decompiled with JetBrains decompiler
// Type: FriendRequestPopupBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class FriendRequestPopupBase : BackButtonMenuBase
{
  [SerializeField]
  protected UILabel TxtFriendName;
  [SerializeField]
  protected UILabel TxtFriendRequest;
  [SerializeField]
  protected UILabel TxtLastPlay;
  [SerializeField]
  protected UILabel TxtPopuptitle26;
  [SerializeField]
  protected UILabel TxtRecommend;
  [SerializeField]
  protected UI2DSprite Emblem;
  private const float LINK_WIDTH = 136f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 1f;
  [SerializeField]
  private GameObject linkChar;
  private List<string> target_friend_ids = new List<string>();
  private DateTime lastTime;
  private Action callback;
  [SerializeField]
  protected GameObject slc_Master;
  [SerializeField]
  protected GameObject slc_Friend;
  [SerializeField]
  protected GameObject slc_Guild;

  public void SetCallback(Action callback) => this.callback = callback;

  public IEnumerator Init(PlayerHelper helper, int incr_friend_point)
  {
    FriendRequestPopupBase requestPopupBase = this;
    requestPopupBase.IsPush = true;
    requestPopupBase.SetGameObject(requestPopupBase.slc_Friend, false);
    requestPopupBase.SetGameObject(requestPopupBase.slc_Guild, false);
    requestPopupBase.SetGameObject(requestPopupBase.slc_Master, false);
    if (helper.is_friend)
      requestPopupBase.SetGameObject(requestPopupBase.slc_Friend, true);
    else if (helper.is_guild_member)
      requestPopupBase.SetGameObject(requestPopupBase.slc_Guild, true);
    else
      requestPopupBase.SetGameObject(requestPopupBase.slc_Master, true);
    requestPopupBase.lastTime = helper.target_player_last_signed_in_at;
    IEnumerator e = requestPopupBase.Init(helper.leader_unit, helper.leader_unit.level, helper.target_player_id, helper.target_player_name, incr_friend_point, helper.current_emblem_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    requestPopupBase.IsPush = false;
  }

  public IEnumerator Init(Gladiator gladiator)
  {
    FriendRequestPopupBase requestPopupBase = this;
    requestPopupBase.IsPush = true;
    PlayerUnit byUnitunit = PlayerUnit.create_by_unitunit(gladiator);
    IEnumerator e = requestPopupBase.Init(byUnitunit, gladiator.leader_unit_level, gladiator.player_id, gladiator.name, -1, gladiator.current_emblem_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    requestPopupBase.IsPush = false;
  }

  private IEnumerator Init(
    PlayerUnit playerUnit,
    int level,
    string playerID,
    string playerName,
    int point,
    int emblemId)
  {
    IEnumerator e = this._Init(playerUnit, playerUnit.unit, level, playerID, playerName, point, emblemId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator Init(
    UnitUnit unit,
    int level,
    string playerID,
    string playerName,
    int point,
    int emblemId)
  {
    IEnumerator e = this._Init((PlayerUnit) null, unit, level, playerID, playerName, point, emblemId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator _Init(
    PlayerUnit playerUnit,
    UnitUnit unit,
    int level,
    string playerID,
    string playerName,
    int point,
    int emblemId)
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = prefabF.Result.Clone(this.linkChar.transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    if (playerUnit == (PlayerUnit) null)
    {
      e = unitScript.SetUnit(unit, unit.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = unitScript.SetUnit(playerUnit, unit.GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    unitScript.BottomModeValue = UnitIconBase.GetBottomMode(unit, playerUnit);
    unitScript.setLevelText(level.ToLocalizeNumberText());
    unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    this.AddTargetFriendsIds(playerID);
    this.SetText(playerName, point);
    e = this.SetEmblem(emblemId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetEmblem(int emblemId)
  {
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(emblemId);
    IEnumerator e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Emblem.sprite2D = sprF.Result;
  }

  public virtual void SetText(string name, int point)
  {
    this.TxtFriendName.SetTextLocalize(name);
    this.TxtLastPlay.SetTextLocalize("最終プレイ：" + (ServerTime.NowAppTime() - this.lastTime).DisplayStringForFriendsGuildMember());
  }

  public virtual void IbtnPopupYes()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().StartCoroutine(this.FriendApplyAsync());
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public void IbtnPopupYesWithoutClose()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().StartCoroutine(this.FriendApplyAsync());
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    if (this.callback != null)
      this.callback();
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  private IEnumerator FriendApplyAsync()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    FriendRequestPopupBase.\u003C\u003Ec__DisplayClass28_0 cDisplayClass280 = new FriendRequestPopupBase.\u003C\u003Ec__DisplayClass28_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass280.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass280.call = this.callback;
    if (SMManager.Get<Player>().friends_count >= SMManager.Get<Player>().max_friends)
    {
      // ISSUE: reference to a compiler-generated field
      Singleton<CommonRoot>.GetInstance().StartCoroutine(this.openPopup00813(cDisplayClass280.call));
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.FriendApply> future = WebAPI.FriendApply(this.target_friend_ids.ToArray(), new Action<WebAPI.Response.UserError>(cDisplayClass280.\u003CFriendApplyAsync\u003Eb__0));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (future.Result != null)
      {
        // ISSUE: reference to a compiler-generated field
        Singleton<CommonRoot>.GetInstance().StartCoroutine(this.openPopup00815(cDisplayClass280.call));
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        this.callback();
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  private IEnumerator openPopup00813(Action call)
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_13__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, (EventDelegate.Callback) (() => call()));
  }

  private IEnumerator openPopup00814(Action call)
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_14__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, (EventDelegate.Callback) (() => call()));
  }

  private IEnumerator openPopup00815(Action call)
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_15__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, (EventDelegate.Callback) (() => call()));
  }

  public void AddTargetFriendsIds(string id) => this.target_friend_ids.Add(id);

  private void SetGameObject(GameObject obj, bool active)
  {
    if (Object.op_Equality((Object) obj, (Object) null))
      return;
    obj.SetActive(active);
  }
}
