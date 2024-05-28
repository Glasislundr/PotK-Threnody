// Decompiled with JetBrains decompiler
// Type: Friend0089Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Friend0089Menu : BackButtonMenuBase
{
  private List<string> target_friend_ids = new List<string>();
  public bool is_back = true;
  private Action callback;

  public void SetBack(bool back) => this.is_back = back;

  public void InitPopup(string id, Action callback = null)
  {
    this.target_friend_ids.Add(id);
    this.callback = callback;
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.openPopup00892());
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  private IEnumerator openPopup00892()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.FriendReject> future = WebAPI.FriendReject(this.target_friend_ids.ToArray(), (Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        Singleton<PopupManager>.GetInstance().onDismiss();
        WebAPI.DefaultUserErrorCallback(e);
      }));
      IEnumerator e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result != null)
      {
        if (future.HasResult)
        {
          if (this.callback != null)
          {
            this.callback();
            yield return (object) new WaitForSeconds(0.5f);
          }
          Singleton<PopupManager>.GetInstance().onDismiss();
          Future<GameObject> prefabF = Res.Prefabs.popup.popup_8_9_2__anim_popup01.Load<GameObject>();
          e1 = prefabF.Wait();
          while (e1.MoveNext())
            yield return e1.Current;
          e1 = (IEnumerator) null;
          GameObject result = prefabF.Result;
          Singleton<PopupManager>.GetInstance().openAlert(result);
          prefabF = (Future<GameObject>) null;
        }
        Singleton<NGGameDataManager>.GetInstance().SetFriendRequestCount();
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        future = (Future<WebAPI.Response.FriendReject>) null;
      }
    }
  }
}
