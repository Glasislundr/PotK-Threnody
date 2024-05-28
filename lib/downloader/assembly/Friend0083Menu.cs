// Decompiled with JetBrains decompiler
// Type: Friend0083Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
public class Friend0083Menu : BackButtonMenuBase
{
  private List<string> target_friend_ids = new List<string>();
  private Action callback;

  public void AddTargetFriendId(string id) => this.target_friend_ids.Add(id);

  private IEnumerator FriendRemoveAsync()
  {
    Friend0083Menu friend0083Menu = this;
    Singleton<PopupManager>.GetInstance().onDismiss();
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.isTouchBlock = true;
    common.loadingMode = 1;
    if (friend0083Menu.target_friend_ids.Count > 0)
    {
      // ISSUE: reference to a compiler-generated method
      Future<WebAPI.Response.FriendRemove> future = WebAPI.FriendRemove(friend0083Menu.target_friend_ids.ToArray(), new Action<WebAPI.Response.UserError>(friend0083Menu.\u003CFriendRemoveAsync\u003Eb__3_0));
      IEnumerator e = future.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (future.Result == null)
        yield break;
      else
        future = (Future<WebAPI.Response.FriendRemove>) null;
    }
    common.loadingMode = 0;
    common.isTouchBlock = false;
    Singleton<PopupManager>.GetInstance().onDismiss();
    if (friend0083Menu.callback != null)
      friend0083Menu.callback();
  }

  private void RemoveError(string err)
  {
    Debug.LogWarning((object) ("FriendRemove API ERROR CODE : " + err));
    if (err == "FRD012")
      Singleton<NGGameDataManager>.GetInstance().refreshHomeHome = true;
    Singleton<CommonRoot>.GetInstance().isTouchBlock = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    MypageScene.ChangeSceneOnError();
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.FriendRemoveAsync());
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  public void SetCallback(Action callback) => this.callback = callback;
}
