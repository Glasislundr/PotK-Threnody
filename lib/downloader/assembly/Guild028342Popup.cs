// Decompiled with JetBrains decompiler
// Type: Guild028342Popup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Guild028342Popup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel popupTitle;
  [SerializeField]
  private UILabel popupDesc;
  private Guild0283Menu menu;

  public void Initialize(Guild0283Menu menu)
  {
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<UIWidget>(), (Object) null))
      ((UIRect) ((Component) this).GetComponent<UIWidget>()).alpha = 0.0f;
    this.menu = menu;
    this.popupTitle.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MENU_RESIGN_CONFIRM_TITLE));
    this.popupDesc.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_GUILD_MENU_RESIGN_DESC3));
  }

  public override void onBackButton() => Singleton<PopupManager>.GetInstance().closeAll();

  private IEnumerator LeaveFromGuild()
  {
    Guild028342Popup guild028342Popup = this;
    Singleton<PopupManager>.GetInstance().closeAllWithoutAnim();
    while (Singleton<PopupManager>.GetInstance().isOpenNoFinish)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1);
    // ISSUE: reference to a compiler-generated method
    Future<WebAPI.Response.GuildMembershipsLeave> ft = WebAPI.GuildMembershipsLeave(false, new Action<WebAPI.Response.UserError>(guild028342Popup.\u003CLeaveFromGuild\u003Eb__5_0));
    IEnumerator e1 = ft.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (ft.Result != null)
    {
      if (Persist.guildHeaderChat.Exists)
      {
        Persist.guildHeaderChat.Data.reset();
        Persist.guildHeaderChat.Flush();
      }
      if (Persist.guildEventCheck.Exists)
      {
        Persist.guildEventCheck.Data.reset();
        Persist.guildEventCheck.Flush();
      }
      Singleton<NGGameDataManager>.GetInstance().chatDataList.Clear();
      Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent().headerChat.ChatReset();
      Future<WebAPI.Response.GuildSignal> signal = WebAPI.GuildSignal((Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      e1 = signal.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (signal.Result != null)
      {
        Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
        Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
        Singleton<NGSceneManager>.GetInstance().clearStack();
        Guild02811Scene.ChangeScene();
      }
    }
  }

  public void onYesButton() => this.StartCoroutine(this.LeaveFromGuild());
}
