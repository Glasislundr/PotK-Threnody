// Decompiled with JetBrains decompiler
// Type: Friend0086Scene
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
public class Friend0086Scene : NGSceneBase
{
  [SerializeField]
  private Friend0086Menu menu;
  [SerializeField]
  private GameObject Dir008_2;
  [SerializeField]
  private GameObject Dir008_6;
  private Friend0086Scene.Mode debugMode;
  private static readonly string SCENE_NAME = "friend008_6";

  public void onStartScene()
  {
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
      return;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public IEnumerator onStartSceneAsync()
  {
    PlayerFriend[] receivedFriend = ((IEnumerable<PlayerFriend>) SMManager.Get<PlayerFriend[]>()).ReceivedFriendApplications();
    ((Component) this.menu).gameObject.SetActive(false);
    switch (this.debugMode)
    {
      case Friend0086Scene.Mode.Accept:
        this.Dir008_2.SetActive(false);
        this.Dir008_6.SetActive(true);
        break;
      case Friend0086Scene.Mode.Friend:
        this.Dir008_2.SetActive(true);
        this.Dir008_6.SetActive(false);
        break;
    }
    Future<WebAPI.Response.FriendDetail> ft = WebAPI.FriendDetail(receivedFriend[0].target_player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (ft.Result != null)
    {
      SM.FriendDetail friendDetail = ft.Result.friend_detail;
      int friendStatus = ft.Result.friend_status;
      bool isFavorite = ft.Result.is_favorite;
      e1 = this.menu.setData(receivedFriend[0].target_player_id, friendDetail, friendStatus, isFavorite);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      ((Component) this.menu).gameObject.SetActive(true);
    }
  }

  public void onStartScene(
    string in_target_player_id,
    Friend0086Scene.Mode mode,
    ResourceObject backGroundResource)
  {
    this.onStartScene();
  }

  public void onStartScene(
    Versus02613Scene.BootParam param,
    Friend0086Scene.Mode mode,
    ResourceObject backGroundResource)
  {
    this.onStartScene();
  }

  public static void changeScene(
    bool isStack,
    Versus02613Scene.BootParam param,
    string player_id,
    Friend0086Scene.Mode mode,
    ResourceObject backGroundResource)
  {
    Versus02613Scene.BootArgument bootArgument = new Versus02613Scene.BootArgument(Friend0086Scene.SCENE_NAME, param.current, player_id: player_id);
    param.push(bootArgument);
    Singleton<NGSceneManager>.GetInstance().changeScene(Friend0086Scene.SCENE_NAME, (isStack ? 1 : 0) != 0, (object) param, (object) mode, (object) backGroundResource);
  }

  public IEnumerator onStartSceneAsync(
    Versus02613Scene.BootParam param,
    Friend0086Scene.Mode mode,
    ResourceObject backGroundResource)
  {
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    Singleton<CommonRoot>.GetInstance().setDisableFooterColor(true);
    this.menu.bootParam = param;
    yield return (object) this.onStartSceneAsync(param.current.playerId, mode, backGroundResource);
  }

  public override void onEndScene()
  {
    base.onEndScene();
    if (this.menu.bootParam == null)
      return;
    Singleton<CommonRoot>.GetInstance().setDisableFooterColor(false);
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
  }

  public IEnumerator onStartSceneAsync(
    string in_target_player_id,
    Friend0086Scene.Mode mode,
    ResourceObject backGroundResource)
  {
    Friend0086Scene friend0086Scene = this;
    ((Component) friend0086Scene.menu).gameObject.SetActive(false);
    if (backGroundResource != null)
    {
      Future<GameObject> bgF = backGroundResource.Load<GameObject>();
      yield return (object) bgF.Wait();
      friend0086Scene.backgroundPrefab = bgF.Result;
      friend0086Scene.menu.isContinueBackground = false;
      bgF = (Future<GameObject>) null;
    }
    else
      friend0086Scene.menu.isContinueBackground = true;
    switch (mode)
    {
      case Friend0086Scene.Mode.Accept:
        friend0086Scene.Dir008_2.SetActive(false);
        friend0086Scene.Dir008_6.SetActive(true);
        break;
      case Friend0086Scene.Mode.Friend:
        friend0086Scene.Dir008_2.SetActive(true);
        friend0086Scene.Dir008_6.SetActive(false);
        break;
    }
    Future<WebAPI.Response.FriendDetail> ft = WebAPI.FriendDetail(in_target_player_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (ft.Result != null)
    {
      SM.FriendDetail friendDetail = ft.Result.friend_detail;
      int friendStatus = ft.Result.friend_status;
      bool isFavorite = ft.Result.is_favorite;
      e1 = friend0086Scene.menu.setData(in_target_player_id, friendDetail, friendStatus, isFavorite);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      ((Component) friend0086Scene.menu).gameObject.SetActive(true);
    }
  }

  public enum Mode
  {
    Accept,
    Friend,
  }
}
