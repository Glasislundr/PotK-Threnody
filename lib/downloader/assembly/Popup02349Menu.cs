// Decompiled with JetBrains decompiler
// Type: Popup02349Menu
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
public class Popup02349Menu : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtFriendName;
  [SerializeField]
  protected UILabel TxtFriendRequest;
  [SerializeField]
  protected UILabel TxtGetPoint;
  [SerializeField]
  protected UILabel TxtPopuptitle26;
  [SerializeField]
  protected UILabel TxtRecommend;
  private const float LINK_WIDTH = 136f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 1f;
  [SerializeField]
  private GameObject linkChar;
  private List<string> target_friend_ids = new List<string>();
  private Action callback;

  public void SetCallback(Action callback) => this.callback = callback;

  public IEnumerator Init(PlayerHelper helper, int incr_friend_point)
  {
    IEnumerator e = this.Init(helper.leader_unit.unit, helper.leader_unit.total_level, helper.target_player_id, helper.target_player_name, incr_friend_point);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(Gladiator gladiator)
  {
    IEnumerator e = this.Init(MasterData.UnitUnit[gladiator.leader_unit_id], gladiator.leader_unit_level, gladiator.player_id, gladiator.name, -1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator Init(
    UnitUnit unit,
    int level,
    string playerID,
    string playerName,
    int point)
  {
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = prefabF.Result.Clone(this.linkChar.transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    e = unitScript.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.BottomModeValue = UnitIconBase.GetBottomMode(unit, (PlayerUnit) null);
    unitScript.setLevelText(level.ToLocalizeNumberText());
    unitScript.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    this.AddTargetFriendsIds(playerID);
    this.SetText(playerName, point);
  }

  public void SetText(string name, int point)
  {
    this.TxtFriendName.SetTextLocalize(name);
    if (point > 0)
      this.TxtGetPoint.SetTextLocalize(Consts.Format(Consts.GetInstance().FRIEND_REQUEST_POPUPBASE, (IDictionary) new Hashtable()
      {
        {
          (object) nameof (point),
          (object) point
        }
      }));
    else
      ((Component) this.TxtGetPoint).gameObject.SetActive(false);
  }

  public virtual void IbtnPopupYes()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    Singleton<CommonRoot>.GetInstance().StartCoroutine(this.FriendApplyAsync());
  }

  public virtual void IbtnPopupNo()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    this.callback();
  }

  private IEnumerator FriendApplyAsync()
  {
    Popup02349Menu popup02349Menu = this;
    if (SMManager.Get<Player>().friends_count >= SMManager.Get<Player>().max_friends)
    {
      Singleton<CommonRoot>.GetInstance().StartCoroutine(popup02349Menu.openPopup00813());
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.FriendApply> future = WebAPI.FriendApply(popup02349Menu.target_friend_ids.ToArray(), new Action<WebAPI.Response.UserError>(popup02349Menu.\u003CFriendApplyAsync\u003Eb__18_0));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (future.Result != null)
      {
        if (future.HasResult)
          Singleton<CommonRoot>.GetInstance().StartCoroutine(popup02349Menu.openPopup00815());
        else
          popup02349Menu.callback();
      }
    }
  }

  private IEnumerator openPopup00813()
  {
    Popup02349Menu popup02349Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_13__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, new EventDelegate.Callback(popup02349Menu.\u003CopenPopup00813\u003Eb__19_0));
  }

  private IEnumerator openPopup00814()
  {
    Popup02349Menu popup02349Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_14__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, new EventDelegate.Callback(popup02349Menu.\u003CopenPopup00814\u003Eb__20_0));
  }

  private IEnumerator openPopup00815()
  {
    Popup02349Menu popup02349Menu = this;
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_15__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    EventDelegate.Add(Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<FriendApplicationResultPopup>().OK.onClick, new EventDelegate.Callback(popup02349Menu.\u003CopenPopup00815\u003Eb__21_0));
  }

  public void AddTargetFriendsIds(string id) => this.target_friend_ids.Add(id);
}
