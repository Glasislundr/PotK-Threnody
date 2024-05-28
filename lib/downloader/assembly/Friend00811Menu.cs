// Decompiled with JetBrains decompiler
// Type: Friend00811Menu
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
public class Friend00811Menu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject LinkUnit;
  [SerializeField]
  private UILabel TxtDescription01;
  [SerializeField]
  private UILabel TxtDescription02;
  [SerializeField]
  private UILabel TxtLastplay;
  [SerializeField]
  private UILabel TxtLastplayTime;
  [SerializeField]
  private UILabel TxtPlayername;
  [SerializeField]
  private UILabel TxtPopuptitle;
  [SerializeField]
  private UILabel TxtLv;
  [SerializeField]
  private UI2DSprite Emblem;
  private WebAPI.Response.PlayerSearch targetResult;
  private List<string> target_friend_ids = new List<string>();

  public void AddTargetFriendsIds(string id) => this.target_friend_ids.Add(id);

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  private IEnumerator FriendApplyAsync()
  {
    Friend00811Menu friend00811Menu = this;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    if (friend00811Menu.target_friend_ids.Count >= 1)
    {
      if (((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).Friends().Length >= SMManager.Get<Player>().max_friends)
      {
        friend00811Menu.StartCoroutine(friend00811Menu.openPopup00813());
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        Future<WebAPI.Response.FriendApply> future = WebAPI.FriendApply(friend00811Menu.target_friend_ids.ToArray(), new Action<WebAPI.Response.UserError>(friend00811Menu.\u003CFriendApplyAsync\u003Eb__14_0));
        IEnumerator e = future.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        if (future.Result != null)
        {
          Singleton<PopupManager>.GetInstance().onDismiss(true);
          Singleton<CommonRoot>.GetInstance().isLoading = false;
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          if (future.HasResult)
            friend00811Menu.StartCoroutine(friend00811Menu.openPopup00815());
        }
      }
    }
  }

  public void IbtnPopupDemand()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
    this.StartCoroutine(this.FriendApplyAsync());
  }

  private IEnumerator openPopup00813()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_13__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().openAlert(prefab.Result);
  }

  private IEnumerator openPopup00814()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_14__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().openAlert(prefab.Result);
  }

  private IEnumerator openPopup00815()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_15__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().openAlert(prefab.Result);
  }

  private IEnumerator openPopup00816()
  {
    Future<GameObject> prefab = Res.Prefabs.popup.popup_008_16__anim_popup01.Load<GameObject>();
    IEnumerator e = prefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().openAlert(prefab.Result).GetComponent<Friend00816Menu>().SetTargetUserData(this.targetResult);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator SetTargetUserData()
  {
    this.TxtPlayername.SetText("テストプレイヤー");
    this.TxtLv.SetTextLocalize(77);
    this.TxtLastplayTime.SetTextLocalize("33秒前");
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Object.Instantiate<GameObject>(prefabF.Result);
    gameObject.transform.parent = this.LinkUnit.transform;
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localScale = Vector3.one;
    this.LinkUnit.SetActive(true);
    e = gameObject.GetComponent<UnitIcon>().SetUnit(MasterData.UnitUnit[100111], CommonElement.none, false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(0);
    e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Emblem.sprite2D = sprF.Result;
  }

  public IEnumerator SetTargetUserData(WebAPI.Response.PlayerSearch result)
  {
    this.targetResult = result;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime dateTime = ServerTime.NowAppTime();
    this.AddTargetFriendsIds(result.target_player.id);
    this.TxtDescription01.SetTextLocalize("ＩＤ：" + result.target_player.short_id);
    this.TxtPlayername.SetTextLocalize(result.target_player.name);
    this.TxtLv.SetTextLocalize(result.target_player.level);
    DateTime playerLastSignedInAt = result.target_player_helper.target_player_last_signed_in_at;
    this.TxtLastplayTime.SetText("[ffff00]" + (dateTime - playerLastSignedInAt).DisplayStringForFriendsGuildMember() + "[-]");
    Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject gameObject = Object.Instantiate<GameObject>(prefabF.Result);
    gameObject.transform.parent = this.LinkUnit.transform;
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localScale = Vector3.one;
    this.LinkUnit.SetActive(true);
    UnitIcon unitIcon = gameObject.GetComponent<UnitIcon>();
    e = unitIcon.setSimpleUnit(result.target_leader_unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitIcon.setLevelText(result.target_leader_unit);
    unitIcon.ShowBottomInfo(UnitSortAndFilter.SORT_TYPES.Level);
    Future<Sprite> sprF = EmblemUtility.LoadEmblemSprite(result.target_player_helper.current_emblem_id);
    e = sprF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.Emblem.sprite2D = sprF.Result;
  }
}
